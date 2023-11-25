using GameServer.KiemThe.Core.Activity.LuckyCircle;
using GameServer.KiemThe.Core.Activity.SeashellCircle;
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

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý gói tin sự kiện Bách Bảo Rương
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Xử lý yêu cầu từ Client thao tác Vòng quay may mắn
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ResponseLuckyCircleRequest(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData;

            try
            {
                cmdData = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length > 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), cmdData.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Người chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu chưa mở
                if (!KTLuckyCircleManager.IsOpened())
                {
                    PlayerManager.ShowNotification(client, "Chức năng hiện chưa được mở!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Yêu cầu tương ứng
                int requestID = int.Parse(fields[0]);

                /*
                /// Nếu là yêu cầu mở vòng quay
                if (requestID == 0)
                {
                    KTLuckyCircleManager.OpenCircle(client);
                }
                /// Nếu là yêu cầu thực hiện quay vòng quay Vòng quay may mắn
                else */if (requestID == 1)
                {
                    int method = int.Parse(fields[1]);
                    /// Nếu quay thất bại
                    if (!KTLuckyCircleManager.StartTurn(client, (KTLuckyCircleManager.LuckyCirclePlayMethod) method, out int stopPos))
                    {
                        /// Nếu có phần quà chưa nhận
                        if (KTLuckyCircleManager.HasAward(client))
                        {
                            G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                            {
                                Items = null,
                                LastStopPos = client.LuckyCircle_LastStopPos,
                                Fields = new int[] { 3, 0, 1 },
                            };
                            client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                        }
                        /// Nếu không có phần quà
                        else
                        {
                            G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                            {
                                Items = null,
                                LastStopPos = client.LuckyCircle_LastStopPos,
                                Fields = new int[] { 3, 1, 0 },
                            };
                            client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                        }
                    }
                    /// Nếu quay thành công
                    else
                    {
                        G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                        {
                            Items = null,
                            LastStopPos = stopPos,
                            Fields = new int[] { 1, KTGlobal.GetRandomNumber(3, 5) },
                        };
                        client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                    }
                }
                /// Nếu là yêu cầu nhận quà Vòng quay may mắn
                else if (requestID == 2)
                {
                    /// Vị trí dừng trước
                    int lastStopPos = client.LuckyCircle_LastStopPos;
                    /// Nếu nhận thưởng thất bại
                    if (!KTLuckyCircleManager.GetAward(client))
                    {
                        /// Nếu có phần quà chưa nhận
                        if (KTLuckyCircleManager.HasAward(client))
                        {
                            G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                            {
                                Items = null,
                                LastStopPos = client.LuckyCircle_LastStopPos,
                                Fields = new int[] { 3, 0, 1 },
                            };
                            client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                        }
                        /// Nếu không có phần quà
                        else
                        {
                            G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                            {
                                Items = null,
                                LastStopPos = client.LuckyCircle_LastStopPos,
                                Fields = new int[] { 3, 1, 0 },
                            };
                            client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                        }
                    }
                    /// Nếu nhận thưởng thành công
                    else
                    {
                        G2C_LuckyCircle luckyCircle = new G2C_LuckyCircle()
                        {
                            Items = null,
                            LastStopPos = lastStopPos,
                            Fields = new int[] { 2 },
                        };
                        client.SendPacket<G2C_LuckyCircle>((int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE, luckyCircle);
                    }
                }

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
