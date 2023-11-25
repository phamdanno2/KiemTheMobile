using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDBServer.DB;
using Server.Data;
using Server.Tools;
using GameDBServer.Server;
using Server.Protocol;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 群邮件的管理器
    /// </summary>
    class GroupMailManager
    {
        /// <summary>
        /// 上次扫描新邮件的时间
        /// </summary>
        private static long LastScanGroupMailTicks = DateTime.Now.Ticks / 10000;

        /// <summary>
        /// 上次扫描到的新邮件的最大id
        /// </summary>
        private static int LastMaxGroupMailID = 0;

        /// <summary>
        /// 缓存锁
        /// </summary>
        private static object GroupMailDataDict_Mutex = new object();

        /// <summary>
        // 群邮件字典缓存
        /// </summary>
        private static Dictionary<int, GroupMailData> GroupMailDataDict = new Dictionary<int, GroupMailData>();

        /// <summary>
        /// 清空缓存 下次重新加载
        /// </summary>
        public static void ResetData()
        {
            lock (GroupMailDataDict_Mutex)
            {
                LastMaxGroupMailID = 0;
                GroupMailDataDict.Clear();
            }
        }

        /// <summary>
        /// 扫描新邮件
        /// </summary>
        public static void ScanLastGroupMails(DBManager dbMgr)
        {
            long nowTicks = DateTime.Now.Ticks / 10000;
            if (nowTicks - LastScanGroupMailTicks < (30 * 1000))
            {
                return;
            }

            LastScanGroupMailTicks = nowTicks;

            // 扫描新邮件
            List<GroupMailData> GroupMailList = DBQuery.ScanNewGroupMailFromTable(dbMgr, LastMaxGroupMailID);

            if (null != GroupMailList && GroupMailList.Count > 0)
            {
                foreach (var item in GroupMailList)
                {
                    AddGroupMailData(item);

                    // 记录加载到哪了
                    if (item.GMailID > LastMaxGroupMailID)
                    {
                        LastMaxGroupMailID = item.GMailID;
                    }
                }

                // 通知GS
                string gmCmdData = string.Format("-notifygmail ");
                ChatMsgManager.AddGMCmdChatMsg(-1, gmCmdData);
            }
        }

        /// <summary>
        /// 增加群邮件数据
        /// </summary>
        private static void AddGroupMailData(GroupMailData gmailData)
        {
            lock (GroupMailDataDict_Mutex)
            {
                GroupMailDataDict[gmailData.GMailID] = gmailData;
            }
        }

        /// <summary>
        /// 获取指定id之后的列表
        /// </summary>
        private static List<GroupMailData> GetGroupMailList(int beginID)
        {
            List<GroupMailData> GroupMailList = null;

            foreach (var item in GroupMailDataDict)
            {
                if (item.Value.GMailID <= beginID)
                {
                    continue;
                }

                if (null == GroupMailList)
                {
                    GroupMailList = new List<GroupMailData>();
                }

                GroupMailList.Add(item.Value);
            }

            return GroupMailList;
        }


        /// <summary>
        /// 收到gs的数据请求
        /// </summary>
        public static TCPProcessCmdResults RequestNewGroupMailList(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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

                int beginID = Convert.ToInt32(fields[0]);

                List<GroupMailData> GroupMailList = GetGroupMailList(beginID);
                byte[] retBytes = DataHelper.ObjectToBytes<List<GroupMailData>>(GroupMailList);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, retBytes, 0, retBytes.Length, nID);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 修改玩家群邮件的状态
        /// </summary>
        public static TCPProcessCmdResults ModifyGMailRecord(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int gmailID = Convert.ToInt32(fields[1]);
                int mailID = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("发起请求的角色不存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int result = DBWriter.ModifyGMailRecord(dbMgr, roleID, gmailID, mailID);

                if (result > 0)
                {
                    // 更新用户缓存
                    lock (dbRoleInfo)
                    {
                        if (null == dbRoleInfo.GroupMailRecordList)
                        {
                            dbRoleInfo.GroupMailRecordList = new List<int>();
                        }

                        if (dbRoleInfo.GroupMailRecordList.IndexOf(gmailID) < 0)
                        {
                            dbRoleInfo.GroupMailRecordList.Add(gmailID);
                        }
                    }
                }

                string strcmd = string.Format("{0}", result);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            return TCPProcessCmdResults.RESULT_DATA;
        }
    }
}
