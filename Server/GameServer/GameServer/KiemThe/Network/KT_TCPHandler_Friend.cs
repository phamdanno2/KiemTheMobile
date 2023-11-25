using GameServer.Core.Executor;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý bạn bè
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Lấy danh sách bạn bè
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGetFriendsCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length < 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds) nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int) TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Global.SafeConvertToInt32(fields[0]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Truy vấn lại trên DB để lấy danh sách bạn bè
                TCPProcessCmdResults ret = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);
                /// Nếu có kết quả
                if (ret == TCPProcessCmdResults.RESULT_DATA)
                {
                    List<FriendData> friendData = DataHelper.BytesToObject<List<FriendData>>(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);
                    /// Lưu thông tin bạn bè vào
                    client.FriendDataList = friendData;
                }
                else
                {
                    return ret;
                }

                /// Tạo mới danh sách bạn bè tạm thời
                List<FriendData> friendList = new List<FriendData>(client.FriendDataList);
                /// Duyệt danh sách bạn bè
                foreach (FriendData friend in friendList)
                {
                    /// Đối tượng người chơi tương ứng
                    KPlayer friendPlayer = GameManager.ClientMgr.FindClient(friend.OtherRoleID);
                    /// Nếu tìm thấy người chơi tức là đang online
                    if (friendPlayer != null)
                    {
                        friend.OnlineState = 1;
                        friend.MapCode = friendPlayer.CurrentMapCode;
                        /// Dấu tọa độ không cho biết
                        friend.PosX = 0;
                        friend.PosY = 0;
                    }
                    else
                    {
                        friend.OnlineState = 0;
                        friend.MapCode = -1;
                        /// Dấu tọa độ không cho biết
                        friend.PosX = 0;
                        friend.PosY = 0;
                    }
                }
                /// Sắp xếp ưu tiên Online ở trên
                client.FriendDataList = friendList.OrderByDescending(x => x.OnlineState).ToList();

                /// Gửi gói tin lại cho người chơi
                byte[] _cmdData = DataHelper.ObjectToBytes<List<FriendData>>(friendList);
                GameManager.ClientMgr.SendToClient(client, _cmdData, (int) TCPGameServerCmds.CMD_SPR_GETFRIENDS);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Thêm bạn
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteAddFriendCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int dbID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);
                string otherName = fields[2];
                int friendType = Convert.ToInt32(fields[3]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu trùng với bản thân
                if (roleID == client.RoleID)
                {
                    PlayerManager.ShowNotification(client, "Không thể thêm bạn với chính mình!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (friendType < 0 || friendType > 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Socket params faild, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Giới hạn Gửi yêu cầu kết bạn liên tục
                if (CreateRoleLimitManager.Instance().AddFriendSlotTicks > 0 && TimeUtil.NOW() - client._AddFriendTicks[friendType] < CreateRoleLimitManager.Instance().AddFriendSlotTicks)
                {
                    PlayerManager.ShowNotification(client, "Thao tác quá nhanh, hãy đợi giây lát!");
                    return TCPProcessCmdResults.RESULT_OK;
                }
                client._AddFriendTicks[friendType] = TimeUtil.NOW();

                /// Nếu là yêu cầu thêm bạn
                if (friendType == 0)
                {
                    /// Người chơi gửi yêu cầu
                    int otherRoleID = RoleName2IDs.FindRoleIDByName(otherName);
                    KPlayer player = GameManager.ClientMgr.FindClient(otherRoleID);
                    /// Nếu người chơi không tồn tại
                    if (player == null)
                    {
                        PlayerManager.ShowNotification(client, "Người chơi không tồn tại hoặc đã rời mạng!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu không có yêu cầu từ người chơi tương ứng
                    if (!player.IsAskingToBeFriendWith(client))
                    {
                        PlayerManager.ShowNotification(client, "Người chơi này không gửi yêu cầu kết bạn tới bạn!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Xóa người chơi khỏi danh sách yêu cầu thêm bạn
                    player.RemoveAskingToBeFriend(client);

                    /// Thêm bạn
                    GameManager.ClientMgr.AddFriend(tcpMgr, tcpClientPool, pool, client, dbID, -1, otherName, friendType);
                }
                /// Nếu không phải thêm bạn thì chỉ xử lý 1 bên
                else
                {
                    GameManager.ClientMgr.AddFriend(tcpMgr, tcpClientPool, pool, client, dbID, -1, otherName, friendType);
                }

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Xóa bạn
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteRemoveFriendCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int dbID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Thực hiện xóa bạn
                GameManager.ClientMgr.RemoveFriend(tcpMgr, tcpClientPool, pool, client, dbID);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Phản hồi yêu cầu gửi lời mời thêm bạn
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteAskFriend(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length < 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds) nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int) TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// ID người chơi muốn hỏi
                int roleID = Global.SafeConvertToInt32(fields[0]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu trùng với bản thân
                if (roleID == client.RoleID)
                {
                    PlayerManager.ShowNotification(client, "Không thể thêm bạn với chính mình!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Người chơi tương ứng
                KPlayer player = GameManager.ClientMgr.FindClient(roleID);
                /// Nếu không tìm thấy người chơi
                if (player == null)
                {
                    PlayerManager.ShowNotification(client, "Người chơi không tồn tại hoặc đã rời mạng.");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu người chơi đã tồn tại trong danh sách yêu cầu thêm bạn của đối tượng
                if (client.IsAskingToBeFriendWith(player))
                {
                    PlayerManager.ShowNotification(client, "Đã gửi lời mời đến người chơi này, hãy kiên nhẫn đợi phản hồi!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đã là bạn bè
                if (client.FriendDataList != null && client.FriendDataList.Any(x => x != null && x.OtherRoleID == roleID))
				{
                    PlayerManager.ShowNotification(client, "Hai bên đã là bằng hữu, không cần gửi yêu cầu kết ban thêm!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Thêm người chơi vào danh sách yêu cầu thêm bạn
                client.AddAskingToBeFriend(player);

                /// Gói dữ liệu Mini của người chơi và gửi đi
                RoleDataMini rd = new RoleDataMini()
                {
                    RoleID = client.RoleID,
                    RoleName = client.RoleName,
                    FactionID = client.m_cPlayerFaction.GetFactionId(),
                    Level = client.m_Level,
                };
                byte[] _cmdData = DataHelper.ObjectToBytes<RoleDataMini>(rd);
                GameManager.ClientMgr.SendToClient(player, _cmdData, (int) TCPGameServerCmds.CMD_SPR_ASKFRIEND);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Phản hồi yêu cầu từ chối lời mời thêm bạn
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteRejectFriend(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length < 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds) nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int) TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// ID đối tượng mời
                int roleID = Global.SafeConvertToInt32(fields[0]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Người chơi tương ứng
                KPlayer player = GameManager.ClientMgr.FindClient(roleID);
                /// Nếu không tìm thấy người chơi
                if (player == null)
                {
                    PlayerManager.ShowNotification(client, "Người chơi không tồn tại hoặc đã rời mạng.");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng không đã tồn tại trong danh sách yêu cầu thêm bạn của người chơi
                if (!player.IsAskingToBeFriendWith(client))
                {
                    PlayerManager.ShowNotification(client, "Không tồn tại yêu cầu tương ứng!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Xóa người chơi khỏi danh sách yêu cầu thêm bạn
                player.RemoveAskingToBeFriend(client);

                /// Gửi phản hồi từ chối yêu cầu thêm bạn
                byte[] _cmdData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", client.RoleID, client.RoleName));
                GameManager.ClientMgr.SendToClient(player, _cmdData, (int) TCPGameServerCmds.CMD_SPR_REJECTFRIEND);
                byte[] __cmdData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", client.RoleID, player.RoleID));
                GameManager.ClientMgr.SendToClient(client, __cmdData, (int) TCPGameServerCmds.CMD_SPR_REJECTFRIEND);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
    }
}
