using System;
using System.Collections.Generic;

using Server.Tools;
using GameDBServer.Logic;


namespace GameDBServer.Server
{
    public class TCPCmdDispatcher
    {
        private static readonly TCPCmdDispatcher instance = new TCPCmdDispatcher();

        /// <summary>
        /// 指令参数个数映射<指令ID， 参数个数>
        /// </summary>
        //private Dictionary<int, short> cmdParamNumMapping = new Dictionary<int, short>();
        private Dictionary<int, ICmdProcessor> cmdProcesserMapping = new Dictionary<int, ICmdProcessor>();

        private TCPCmdDispatcher() { }

        public static TCPCmdDispatcher getInstance()
        {
            return instance;
        }

        public void initialize()
        {
        }

        public void registerProcessor(int cmdId, /*short paramNum,*/ ICmdProcessor processor)
        {
            //cmdParamNumMapping.Add(cmdId, paramNum);
            cmdProcesserMapping.Add(cmdId, processor);
        }

        public TCPProcessCmdResults dispathProcessor(GameServerClient client, int nID, byte[] data, int count)
        {
            //tring cmdData = null;

//             try
//             {
//                 cmdData = new UTF8Encoding().GetString(data, 0, count);
//             }
//             catch (Exception)
//             {
//                 LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(client.CurrentSocket)));
//                 return TCPProcessCmdResults.RESULT_FAILED;
//             }

            try
            {
//                 //获取指令参数数量
//                 short cmdParamNum = -1;
//                 if (!cmdParamNumMapping.TryGetValue(nID, out cmdParamNum))
//                 {
//                     LogManager.WriteLog(LogTypes.Error, string.Format("未注册指令, CMD={0}, Client={1}",
//                         (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(client.CurrentSocket)));
//                     return TCPProcessCmdResults.RESULT_FAILED;
//                 };
                //解析用户名称和用户密码
//                 string[] cmdParams = cmdData.Split(':');
//                 if (cmdParams.Length != cmdParamNum)
//                 {
//                     LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
//                         (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(client.CurrentSocket), cmdParams.Length));
//                     return TCPProcessCmdResults.RESULT_FAILED;
//                 }

                
                //获取相对应的指令处理器
                ICmdProcessor cmdProcessor = null;
                if (!cmdProcesserMapping.TryGetValue(nID, out cmdProcessor))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Unhandled packet, CMD={0}, Client={1}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(client.CurrentSocket)));

                    client.sendCmd((int)TCPGameServerCmds.CMD_DB_ERR_RETURN, "0");
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                cmdProcessor.processCmd(client, nID, data, count);

                return TCPProcessCmdResults.RESULT_OK;

            }
            catch (Exception ex)
            {
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.CurrentSocket), false);
            }

            client.sendCmd((int)TCPGameServerCmds.CMD_DB_ERR_RETURN, "0");
            return TCPProcessCmdResults.RESULT_DATA;
        }

    }
}
