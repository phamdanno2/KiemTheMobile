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
    /// Quản lý thông tin phiên bản
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Phản hồi thông tin phiên bản Client
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessClientPushVersionCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// ID người chơi
                int roleID = Convert.ToInt32(fields[0]);
                /// Thông tin gì đó hiện chưa dùng
                int tempParam = Convert.ToInt32(fields[1]);
                /// Phiên bản App
                int mainExeVer = Global.SafeConvertToInt32(fields[2]);
                /// Phiên bản tài nguyên
                int resVer = Global.SafeConvertToInt32(fields[3]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null != client)
                {
                    client.MainExeVer = mainExeVer;
                    client.ResVer = resVer;
                }

                bool NeedUpdate = false;

                //if(GameManager.ClientCore != mainExeVer)
                //{
                //    NeedUpdate = true;
                //}

                //if (GameManager.ClientResVer != resVer)
                //{
                //    NeedUpdate = true;
                //}

                //if (NeedUpdate)
                //{
                //    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(TCPOutPacketPool.getInstance(), string.Format("{0}:{1}:{2}", 1, GameManager.ClientCore, GameManager.ClientResVer), nID);
                //    return TCPProcessCmdResults.RESULT_DATA;
                //}

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
