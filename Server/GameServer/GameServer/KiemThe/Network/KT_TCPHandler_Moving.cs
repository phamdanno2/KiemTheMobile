using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý gói tin
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Client bắt đầu di chuyển
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteMoveCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            SpriteMoveData cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SpriteMoveData>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                if (null == cmdData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// ID đối tượng
                int roleID = cmdData.RoleID;
                /// Vị trí đích X
                int toX = cmdData.ToX;
                /// Vị trí đích Y
                int toY = cmdData.ToY;
                /// Vị trí bắt đầu X
                int fromX = cmdData.FromX;
                /// Vị trí bắt đầu Y
                int fromY = cmdData.FromY;

                /// Người chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    //LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu đã chết thì thôi
                if (client.IsDead() || KTGlobal.GetCurrentTimeMilis() - client.LastDeadTicks < 1000)
				{
                    return TCPProcessCmdResults.RESULT_OK;
				}

                /// Dừng StoryBoard hiện tại
                KTPlayerStoryBoardEx.Instance.Remove(client);

                /// Vị trí hiện tại
                int currentPosX = client.PosX;
                int currentPosY = client.PosY;

                /// Nếu đang đợi chuyển POS thì thôi
                if (client.WaitingForChangePos)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                ///// Nếu đang trong thời gian chờ thực hiện động tác xuất chiêu
                //if (client.m_cPlayerFaction.IsPhysical())
                //{
                //    if (!KTGlobal.FinishedUseSkillAction(client, client.GetCurrentAttackSpeed()))
                //    {
                //        return TCPProcessCmdResults.RESULT_OK;
                //    }
                //}
                //else
                //{
                //    if (!KTGlobal.FinishedUseSkillAction(client, client.GetCurrentCastSpeed()))
                //    {
                //        return TCPProcessCmdResults.RESULT_OK;
                //    }
                //}

                /// Nếu đang bày bán
                if (client.StallDataItem != null)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đang đợi chuyển cảnh
                if (client.WaitingForChangeMap)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// ID bản đồ hiện tại
                int mapCode = client.CurrentMapCode;

                /// Thông tin bản đồ hiện tại
                GameMap gameMap = GameManager.MapMgr.GetGameMap(mapCode);
                if (null == gameMap)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu đang Blink thì thôi
                if (client.IsBlinking())
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Đoạn đường di chuyển
                string pathString = cmdData.PathString;

                /// Tốc độ di chuyển hiện tại
                int moveSpeed = client.GetCurrentRunSpeed();

                //-------------fix jackson đóng thông báo
                //Console.WriteLine("START MOVE ! ");
                /// Kiểm tra vị trí hiện tại của người chơi và vị trí truyền về từ Client xem có hợp lệ không
                if (!client.SpeedCheatDetector.Validate(fromX, fromY))
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu không thể chủ động di chuyển
                if (!client.IsCanPositiveMove())
                {
                    //Console.WriteLine("CMD_MOVE => can not positive move");
                    //LogManager.WriteLog(LogTypes.RolePosition, string.Format("SPR_MOVE => can not positive move. RoleID = {0}, ClientPos = ({1},{2})", roleID, fromX, fromY, client.PosX, client.PosY));

                    /// Thay đổi vị trí hiện tại của Client
                    GameManager.ClientMgr.ForceChangePosNotifySelfOnly(client, fromX, fromY);
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Gói tin gửi cho người chơi khác thông báo đối tượng di chuyển
                SpriteNotifyOtherMoveData moveData = new SpriteNotifyOtherMoveData()
                {
                    RoleID = client.RoleID,
                    FromX = currentPosX,
                    FromY = currentPosY,
                    ToX = toX,
                    ToY = toY,
                    PathString = cmdData.PathString,
                    Action = (int) KE_NPC_DOING.do_run,
                };
                /// Thông báo cho người chơi khác
                GameManager.ClientMgr.NotifyOthersMyMoving(tcpMgr.MySocketListener, pool, moveData, client, nID);

                pathString += string.Format("|{0}_{1}", toX, toY);
                /// Thực hiện StoryBoard
                KTPlayerStoryBoardEx.Instance.Add(client, pathString);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gói tin từ Client gửi lên Server thông báo đối tượng ngừng di chuyển
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteStopMoveCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            SpriteStopMove cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SpriteStopMove>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                int roleID = cmdData.RoleID;
                int posX = cmdData.PosX;
                int posY = cmdData.PosY;
                long stopTick = cmdData.StopTick;
                int direction = cmdData.Direction;

                stopTick -= 50;

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                //if (client.m_cPlayerFaction.IsPhysical())
                //{
                //    /// Nếu đang trong thời gian chờ thực hiện động tác xuất chiêu
                //    if (!KTGlobal.FinishedUseSkillAction(client, client.GetCurrentAttackSpeed()))
                //    {
                //        return TCPProcessCmdResults.RESULT_OK;
                //    }
                //}
                //else
                //{
                //    /// Nếu đang trong thời gian chờ thực hiện động tác xuất chiêu
                //    if (!KTGlobal.FinishedUseSkillAction(client, client.GetCurrentCastSpeed()))
                //    {
                //        return TCPProcessCmdResults.RESULT_OK;
                //    }
                //}

                /// Vị trí hiện tại
                int currentPosX = client.PosX;
                int currentPosY = client.PosY;

                /// Dừng thực thi StoryBoard
                //KTPlayerStoryBoard.Instance.Remove(client, false, true, delayPacket);
                KTPlayerStoryBoardEx.Instance.Remove(client, false);

                /// Nếu đã chết thì thôi
                if (client.IsDead() || KTGlobal.GetCurrentTimeMilis() - client.LastDeadTicks < 1000)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Gửi vị trí hiện tại cho các Client khác
                GameManager.ClientMgr.NotifyOthersStopMyMoving(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);

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
