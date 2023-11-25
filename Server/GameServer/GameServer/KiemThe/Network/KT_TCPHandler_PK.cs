using GameServer.Core.Executor;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameEvents.GuildWarManager;
using GameServer.Logic;
using GameServer.Server;
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
    /// Quản lý PK
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Tỷ thí

        /// <summary>
        /// Thực thi gửi lời mời tỷ thí lên người chơi tương ứng
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ResponseAskChallenge(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string strCmd;

            try
            {
                strCmd = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = strCmd.Split(':');
                /// Nếu số lượng gửi về không thỏa mãn
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// ID đối tượng tương ứng
                int targetID = int.Parse(fields[0]);

                /// Người chơi tương ứng gửi gói tin
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(client.CurrentMapCode);
                /// Nếu bản đồ không cho phép tỷ thí
                if (gameMap != null && (!gameMap.AllowChallenge || !gameMap.AllowPK))
                {
                    PlayerManager.ShowNotification(client, "Bản đồ này không chấp nhận người chơi tỷ thí nhau!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Đối tượng tỷ thí
                KPlayer target = GameManager.ClientMgr.FindClient(targetID);
                /// Nếu không tìm thấy đối tượng
                if (target == null)
                {
                    PlayerManager.ShowNotification(client, "Người chơi này không tồn tại hoặc đã rời mạng!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng trùng với bản thân
                if (target == client)
                {
                    PlayerManager.ShowNotification(client, "Không thể tự thách đấu chính mình!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng đang tỷ thí cùng
                if (target.IsChallengeWith(client))
                {
                    PlayerManager.ShowNotification(client, "Đang tỷ thí với đối phương, không thể thách đấu thêm!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng đang tỷ thí với người khác
                if (target.IsChallengeTime())
                {
                    PlayerManager.ShowNotification(client, "Đối phương đang tỷ thí với người chơi khác, không thể thách đấu!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối phương đã chết
                if (target.IsDead())
                {
                    PlayerManager.ShowNotification(client, "Đối phương đã tử nạn, không thể thách đấu!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối phương ở bản đồ khác
                if (target.CurrentMapCode != client.CurrentMapCode)
                {
                    PlayerManager.ShowNotification(client, "Đối phương đang ở bản đồ khác, không thể thách đấu!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Gửi lời mời thách đấu
                KT_TCPHandler.SendAskChallengeRequest(client, target);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gửi lời mời thách đấu đến người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        public static void SendAskChallengeRequest(KPlayer player, KPlayer target)
        {
            try
            {
                string strCmd = string.Format("{0}:{1}", player.RoleID, player.RoleName);
                GameManager.ClientMgr.SendToClient(target, strCmd, (int) TCPGameServerCmds.CMD_KT_ASK_CHALLENGE);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Thực thi yêu cầu đồng ý hoặc từ chối tỷ thí với người chơi tương ứng
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ResponseResponseChallenge(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string strCmd;

            try
            {
                strCmd = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = strCmd.Split(':');
                /// Nếu số lượng gửi về không thỏa mãn
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// ID đối tượng gửi lời mời tỷ thí
                int inviterID = int.Parse(fields[0]);
                /// Loại thao tác (0: Từ chối, 1: Đồng ý)
                int type = int.Parse(fields[1]);

                /// Người chơi tương ứng gửi gói tin
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(client.CurrentMapCode);
                /// Nếu bản đồ không cho phép tỷ thí
                if (gameMap != null && (!gameMap.AllowChallenge || !gameMap.AllowPK))
                {
                    PlayerManager.ShowNotification(client, "Bản đồ này không chấp nhận người chơi tỷ thí nhau!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Đối tượng gửi lời mời tỷ thí
                KPlayer inviter = GameManager.ClientMgr.FindClient(inviterID);
                /// Nếu không tìm thấy đối tượng
                if (inviter == null)
                {
                    PlayerManager.ShowNotification(client, "Đối phương không tồn tại hoặc đã rời mạng!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng trùng với bản thân
                if (inviter == client)
                {
                    PlayerManager.ShowNotification(client, "Không thể tự thách đấu với bản thân!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu thao tác là từ chối
                if (type == 0)
                {
                    PlayerManager.ShowNotification(inviter, "Đối phương từ chối lời mời thách đấu!");
                }
                /// Nếu thao tác là đồng ý
                else if (type == 1)
                {
                    /// Nếu đối tượng đang tỷ thí cùng
                    if (inviter.IsChallengeWith(client))
                    {
                        PlayerManager.ShowNotification(client, "Đang tỷ thí với đối phương, không thể thách đấu thêm!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu đối tượng đang tỷ thí với người khác
                    if (inviter.IsChallengeTime())
                    {
                        PlayerManager.ShowNotification(client, "Đối phương đang tỷ thí với người chơi khác, không thể thách đấu!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu đối phương đã chết
                    if (inviter.IsDead())
                    {
                        PlayerManager.ShowNotification(client, "Đối phương đã tử nạn, không thể thách đấu!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu đối phương ở bản đồ khác
                    if (inviter.CurrentMapCode != client.CurrentMapCode)
                    {
                        PlayerManager.ShowNotification(client, "Đối phương đang ở bản đồ khác, không thể thách đấu!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Bắt đầu thách đấu
                    client.BeginChallenge(inviter);
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
        /// Gửi thông báo bắt đầu tỷ thí
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        public static void SendBeginChallenge(KPlayer player, KPlayer target)
        {
            try
            {
                string strCmd = string.Format("{0}:{1}", target.RoleID, target.RoleName);
                GameManager.ClientMgr.SendToClient(player, strCmd, (int) TCPGameServerCmds.CMD_KT_G2C_START_CHALLENGE);
                if (target != null)
                {
                    string strCmd1 = string.Format("{0}:{1}", player.RoleID, player.RoleName);
                    GameManager.ClientMgr.SendToClient(target, strCmd1, (int) TCPGameServerCmds.CMD_KT_G2C_START_CHALLENGE);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Gửi thông báo kết thúc tỷ thí
        /// </summary>
        /// <param name="player"></param>
        /// <param name="winner"></param>
        public static void SendStopChallenge(KPlayer player, KPlayer target, KPlayer winner)
        {
            try
            {
                string strCmd = string.Format("{0}:{1}:{2}", winner != null ? winner.RoleID : -1, target != null ? target.RoleID : -1, target != null ? target.RoleName : "");
                GameManager.ClientMgr.SendToClient(player, strCmd, (int) TCPGameServerCmds.CMD_KT_G2C_STOP_CHALLENGE);
                if (target != null)
                {
                    string _strCmd = string.Format("{0}:{1}:{2}", winner != null ? winner.RoleID : -1, player != null ? player.RoleID : -1, player != null ? player.RoleName : "");
                    GameManager.ClientMgr.SendToClient(target, _strCmd, (int) TCPGameServerCmds.CMD_KT_G2C_STOP_CHALLENGE);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        #endregion Tỷ thí

        #region Trạng thái PK
        /// <summary>
        /// Thay đổi trạng thái PK
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteChangePKModeCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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

                int roleID = Convert.ToInt32(fields[0]);
                int pkMode = Convert.ToInt32(fields[1]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(client.CurrentMapCode);



                /// Nếu bản đồ không cho phép đổi trạng thái PK
                if (gameMap != null && !gameMap.AllowChangePKStatus)
                {
                    PlayerManager.ShowNotification(client, "Bản đồ này không cho phép chủ động chuyển trạng thái PK!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đang trong bản đồ tranh đoạt lãnh thổ sẽ không cho phép thay đổi trạng thái PK
                if (!GuidWarManager.getInstance().CanChangePKStatus(client))
                {
                    PlayerManager.ShowNotification(client, "Bạn đang trong bản đồ tranh đoạt lãnh thổ không thể thay đổi trạng thái PK!");
                    return TCPProcessCmdResults.RESULT_OK;

                }

                /// Nếu trạng thái PK không hợp lý
                if (pkMode < (int) PKMode.Peace || pkMode >= (int) PKMode.Count || pkMode == (int) PKMode.Custom)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("PK mode changed faild, CMD={0}, Client={1}, RoleID={2}, ReceivedPKMode={3}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), roleID, pkMode));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu trạng thái PK trùng với trạng thái hiện tại
                if (client.PKMode == pkMode)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu chuyển về trạng thái hòa bình
                if (pkMode == (int) PKMode.Peace)
                {
                    if (KTGlobal.GetCurrentTimeMilis() - client.LastChangePKModeToFight < KTGlobal.TimeCooldownChangingPKModeToFight)
                    {
                        long tickLeft = KTGlobal.TimeCooldownChangingPKModeToFight - KTGlobal.GetCurrentTimeMilis() + client.LastChangePKModeToFight;
                        tickLeft /= 1000;
                        /// Số phút còn lại
                        int nMinute = (int) tickLeft / 60;
                        int nSec = (int) tickLeft - nMinute * 60;
                        PlayerManager.ShowNotification(client, string.Format("Phải chờ {0}:{1} nữa mới có thể chuyển về trạng thái Luyện công!", nMinute, nSec));
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else
                {
                    /// Cập nhật thời gian đổi trạng thái PK mới
                    client.LastChangePKModeToFight = KTGlobal.GetCurrentTimeMilis();
                }

                /// Cập nhật trạng thái PK
                client.PKMode = pkMode;

                ///// Lưu trạng thái PK vào DB
                //GameManager.DBCmdMgr.AddDBCmd((int) TCPGameServerCmds.CMD_DB_UPDATEPKMODE_CMD, cmdData, null, client.ServerId);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gửi gói tin thông báo tới tất cả người chơi xung quanh trạng thái PK của đối tượng thay đổi
        /// </summary>
        /// <param name="go"></param>
        public static void SendToOthersMyPKModeAndCampChanged(GameObject go)
        {
            try
            {
                string cmdData = string.Format("{0}:{1}:{2}", go.RoleID, (go is KPlayer) ? (go as KPlayer).PKMode : 0, go.Camp);

                /// Tìm tất cả người chơi xung quanh để gửi gói tin
                List<object> listObjects = Global.GetAll9Clients(go);
                if (listObjects == null)
                {
                    return;
                }

                foreach (object obj in listObjects)
                {
                    if (obj is KPlayer && (!go.IsInvisible() || (go.IsInvisible() && go.VisibleTo(obj as KPlayer))))
                    {
                        GameManager.ClientMgr.SendToClient((KPlayer) obj, cmdData, (int) TCPGameServerCmds.CMD_SPR_CHGPKMODE);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
        #endregion

        #region Trị PK
        /// <summary>
        /// Gửi gói tin thông báo tới tất cả người chơi xung quanh trị PK của đối tượng thay đổi
        /// </summary>
        /// <param name="client"></param>
        public static void SendToOthersMyPKValueChanged(KPlayer client)
        {
            try
            {
                string cmdData = string.Format("{0}:{1}", client.RoleID, client.PKValue);

                /// Tìm tất cả người chơi xung quanh để gửi gói tin
                List<object> listObjects = Global.GetAll9Clients(client);
                if (listObjects == null)
                {
                    return;
                }

                foreach (object obj in listObjects)
                {
                    if (obj is KPlayer && (!client.IsInvisible() || (client.IsInvisible() && client.VisibleTo(obj as KPlayer))))
                    {
                        GameManager.ClientMgr.SendToClient((KPlayer) obj, cmdData, (int) TCPGameServerCmds.CMD_SPR_CHGPKVAL);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Cập nhật trị PK của đối tượng vào DB
        /// </summary>
        /// <param name="client"></param>
        public static void UpdatePKValueToDB(KPlayer client)
        {
            try
            {
                GameManager.DBCmdMgr.AddDBCmd((int) TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD, string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, 0), null, client.ServerId);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
        #endregion

        #region Tuyên chiến
        /// <summary>
        /// Thực thi yêu cầu tuyên chiến với người chơi tương ứng
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ResponseActiveFight(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string strCmd;

            try
            {
                strCmd = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = strCmd.Split(':');
                /// Nếu số lượng gửi về không thỏa mãn
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// ID đối tượng tuyên chiến
                int targetID = int.Parse(fields[0]);

                /// Người chơi tương ứng gửi gói tin
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(client.CurrentMapCode);
                /// Nếu bản đồ không cho phép tuyên chiến
                if (gameMap != null && (!gameMap.AllowFightTarget || !gameMap.AllowPK))
                {
                    PlayerManager.ShowNotification(client, "Bản đồ này không cho phép người tuyên chiến lẫn nhau!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đang trong sự kiện tranh đoạt lãnh thổ
                if (!GuidWarManager.getInstance().CanChangePKStatus(client))
                {
                    PlayerManager.ShowNotification(client, "Bản đồ tranh đoạt lãnh thổ không thể chủ động tuyên chiến người chơi khác!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Đối tượng tuyên chiến
                KPlayer target = GameManager.ClientMgr.FindClient(targetID);
                /// Nếu không tìm thấy đối tượng
                if (target == null)
                {
                    PlayerManager.ShowNotification(client, "Đối phương không tồn tại hoặc đã rời mạng!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối tượng trùng với bản thân
                if (target == client)
                {
                    PlayerManager.ShowNotification(client, "Không thể tự tuyên chiến với bản thân!");
                    return TCPProcessCmdResults.RESULT_OK;
                }


                /// Nếu đối phương đã chết
                if (target.IsDead())
                {
                    PlayerManager.ShowNotification(client, "Đối phương đã tử nạn, không thể tuyên chiến!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đối phương ở bản đồ khác
                if (target.CurrentMapCode != client.CurrentMapCode)
                {
                    PlayerManager.ShowNotification(client, "Đối phương đang ở bản đồ khác, không thể tuyên chiến!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu cùng tổ đội
                if (target.TeamID != -1 && client.TeamID == target.TeamID)
				{
                    PlayerManager.ShowNotification(client, "Trong cùng nhóm không thể tuyên chiến lẫn nhau!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Bắt đầu tuyên chiến
                client.StartActiveFight(target);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gửi thông báo đối tượng tuyên chiến nhau
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        public static void SendStartActiveFight(KPlayer player, KPlayer target)
        {
            try
            {
                string strCmd = string.Format("{0}:{1}", target.RoleID, target.RoleName);
                GameManager.ClientMgr.SendToClient(player, strCmd, (int) TCPGameServerCmds.CMD_KT_G2C_START_ACTIVEFIGHT);
                if (target != null)
                {
                    string strCmd1 = string.Format("{0}:{1}", player.RoleID, player.RoleName);
                    GameManager.ClientMgr.SendToClient(target, strCmd1, (int) TCPGameServerCmds.CMD_KT_G2C_START_ACTIVEFIGHT);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Gửi thông báo kết thúc tuyên chiến
        /// </summary>
        /// <param name="player"></param>
        /// <param name="winner"></param>
        public static void SendStopActiveFight(KPlayer player, KPlayer target)
        {
            try
            {
                string strCmd = string.Format("{0}:{1}", target.RoleID, target.RoleName);
                GameManager.ClientMgr.SendToClient(player, strCmd, (int) TCPGameServerCmds.CMD_KT_G2C_STOP_ACTIVEFIGHT);
                if (target != null)
                {
                    string strCmd1 = string.Format("{0}:{1}", player.RoleID, player.RoleName);
                    GameManager.ClientMgr.SendToClient(target, strCmd1, (int) TCPGameServerCmds.CMD_KT_G2C_STOP_ACTIVEFIGHT);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
        #endregion
    }
}
