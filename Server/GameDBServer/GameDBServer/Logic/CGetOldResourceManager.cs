using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using Server.Data;
using Server.TCP;
using Server.Protocol;
using Server.Tools;
using System.Net;
using System.Net.Sockets;
using GameDBServer.DB;
using System.Text.RegularExpressions;
using GameDBServer.Server;

namespace GameDBServer.Logic
{
    class CGetOldResourceManager
    {
        public static TCPProcessCmdResults ProcessQueryGetResourceInfo(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
             string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {

                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("发起请求的角色不存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求发起写数据库的操作
                Dictionary<int, OldResourceInfo> TmpDict = DBQuery.QueryResourceGetInfo(dbMgr, roleID);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Dictionary<int, OldResourceInfo>>(TmpDict, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;

            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            return TCPProcessCmdResults.RESULT_FAILED;
        }


        public static TCPProcessCmdResults ProcessUpdateGetResourceInfo(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count,out TCPOutPacket tcpOutPacket)
        {
            
            tcpOutPacket = null;
            try
            {
                byte[] retBytes = DataHelper.ObjectToBytes<int>(0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, retBytes, 0, retBytes.Length, nID);

                Dictionary<int, Dictionary<int, OldResourceInfo>> tmpdict = null;
                tmpdict = DataHelper.BytesToObject<Dictionary<int, Dictionary<int, OldResourceInfo>>>(data, 0, count);
                Dictionary<int, OldResourceInfo> dict = tmpdict.Values.ToArray < Dictionary<int, OldResourceInfo>>()[0];
                if (tmpdict == null/* || dict == null || dict.Count == 0*/)
                {
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                int roleid = tmpdict.Keys.ToArray<int>()[0];

                for (int i = 1; i < (int)CandoType.MaxCandoTypeNum; i++)
                {
                    OldResourceInfo info = null;
                    if (dict != null)
                        dict.TryGetValue(i, out info);
                    int ret = DBWriter.UpdateResourceGetInfo(dbMgr, roleid, i, info);
                    if (ret <= 0)
                    {
                        //添加任务失败
                        LogManager.WriteLog(LogTypes.Error, string.Format("更新资源找回失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleid));
                    }

                    retBytes = DataHelper.ObjectToBytes<int>(ret);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, retBytes, 0, retBytes.Length, nID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            return TCPProcessCmdResults.RESULT_DATA;
        }
    }
}
