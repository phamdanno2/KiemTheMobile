using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Data;
using Server.Tools;
using GameServer.Server;
using GameServer.Core.Executor;
using GameServer.KiemThe.Core;

namespace GameServer.Logic
{
    /// <summary>
    /// 群邮件的管理器
    /// </summary>
    class GroupMailManager
    {
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

        // 请求新的邮件列表
        public static void RequestNewGroupMailList()
        {
            List<GroupMailData> GroupMailList = null;
            {
                GroupMailList = Global.SendToDB<List<GroupMailData>, string>((int)TCPGameServerCmds.CMD_DB_REQUESTNEWGMAILLIST, string.Format("{0}", LastMaxGroupMailID), GameManager.LocalServerId);
            }

            lock (GroupMailDataDict)
            {
                if (null != GroupMailList && GroupMailList.Count > 0)
                {
                    foreach (var item in GroupMailList)
                    {
                        //GroupMailDataEx gmailDataEx = new GroupMailDataEx(item);
                        //gmailDataEx.ConditionList = GameManager.SystemMagicActionMgr.ParseActions(gmailDataEx.GMailData.Conditions);
                        AddGroupMailData(item);

                        // 记录加载到哪了
                        if (item.GMailID > LastMaxGroupMailID)
                        {
                            LastMaxGroupMailID = item.GMailID;
                        }
                    }

                }
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
        /// 判断一个client是否符合条件
        /// </summary>
        private static bool InConditions(KPlayer client, GroupMailData gmailData)
        {
            // 没条件
            if (string.IsNullOrEmpty(gmailData.Conditions))
            {
                return true;
            }

            long currTicks = TimeUtil.NOW() * 10000;
            if (currTicks < gmailData.InputTime || currTicks > gmailData.EndTime)
            {
                return false;
            }

            string[] strFields = gmailData.Conditions.Split('|');

            bool bError = false;
            for (int i = 0; i < strFields.Length; i++)
            { 
                string[] strKey = strFields[i].Split(',');

                // 大于等于这个等级才发
                if ("level" == strKey[0])
                {
                    if (strKey.Length != 2)
                    {
                        bError = true;
                        break;
                    }

                    int currLvl = 0 + client.m_Level;
                    int cfgLvl = Global.SafeConvertToInt32(strKey[1]);
                    if (currLvl < cfgLvl)
                    {
                        return false;
                    }
                }
                // 在某个等级段才发
                else if ("levelrange" == strKey[0])
                {
                    if (strKey.Length != 3)
                    {
                        bError = true;
                        break;
                    }

                    int currLvl = 0 + client.m_Level;
                    int cfgLvlMin = Global.SafeConvertToInt32(strKey[1]);
                    int cfgLvlMax = Global.SafeConvertToInt32(strKey[2]);

                    if (currLvl < cfgLvlMin || currLvl > cfgLvlMax)
                    {
                        return false;
                    }
                }
                // ...loginrange,2015-04-01,2015-04-02
                else if ("loginrange" == strKey[0])
                {
                    if (strKey.Length != 3)
                    {
                        bError = true;
                        break;
                    }

                    string strBeginTime = strKey[1];
                    string strEndTime = strKey[2];

                    DateTime beginTime = DateTime.Parse(strBeginTime);
                    DateTime endTime = DateTime.Parse(strEndTime);

                    if (!Global.CheckRoleIsLoginByTime(client, beginTime, endTime))
                    {
                        // 对于这种以后不会再达到的条件，就设置成不再发送 省得老进行判断
                        // 像等级范围那种可能会进入到条件范围的，就不设置这个标志
                        if (TimeUtil.NOW() * 10000 > endTime.Ticks)
                        {
                            SetGMailNeverSend(client, gmailData.GMailID, -1);
                        }
                        return false;
                    }
                }
                else
                {
                    break;
                }
            }

            // 发现错误的条件 在日志里记录，并修改内存中玩家的领取标记但不记录数据库
            if (bError)
            {
                SetGMailIsSend(client, gmailData.GMailID);
                LogManager.WriteLog(LogTypes.Error, string.Format("GroupMailManager::InConditions Error Conditions={0}", gmailData.Conditions));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查用户是否有新的群邮件 并发送
        /// </summary>
        public static void CheckRoleGroupMail(KPlayer client)
        {
            long currTicks = TimeUtil.NOW();
            if (client.LastCheckGMailTick + 60 * 1000 > currTicks)
            {
                return;
            }
            client.LastCheckGMailTick = currTicks;

            lock (GroupMailDataDict)
            {
                foreach (var item in GroupMailDataDict)
                {
                    // 如果这个邮件已经发送过了
                    if (IfGMailIsSend(client, item.Value.GMailID))
                    {
                        continue;
                    }
                    if (!InConditions(client, item.Value))
                    {
                        continue;
                    }

                    // 给玩家发送这条群邮件
                    SendGMail2Role(client, item.Value);
                }
            }
        }

        /// <summary>
        /// 给玩家发送邮件
        /// </summary>
        private static void SendGMail2Role(KPlayer client, GroupMailData gmailData)
        { 
            // 先做记录
            if (!SetGMailNeverSend(client, gmailData.GMailID, 0))
                return;

            int mailID = KTMailManager.SendSystemMailToPlayer(client, gmailData.GoodsList, gmailData.Subject, gmailData.Content, 0, 0);

            // 把mailid存起来
            SetGMailNeverSend(client, gmailData.GMailID, mailID);
        }

        #region 玩家群邮件是否发送的记录

        /// <summary>
        /// 检查是否已经发送了某封群邮件
        /// </summary>
        public static bool IfGMailIsSend(KPlayer client, int gmailID)
        {
            lock (client.GroupMailRecordList)
            {
                return client.GroupMailRecordList.IndexOf(gmailID) >= 0;
            }
        }

        /// <summary>
        /// 设置已经发送了某封群邮件
        /// </summary>
        public static void SetGMailIsSend(KPlayer client, int gmailID)
        {
            lock (client.GroupMailRecordList)
            {
                if (client.GroupMailRecordList.IndexOf(gmailID) < 0)
                {
                    client.GroupMailRecordList.Add(gmailID);
                }
            }
        }

        /// <summary>
        /// 将某封群邮件设置为不再发送
        /// </summary>
        public static bool SetGMailNeverSend(KPlayer client, int gmailID, int mailID)
        {
            // 向DB提交申请
            string dbCmds = string.Format("{0}:{1}:{2}", client.RoleID, gmailID, mailID);
            string[] dbFields = null;
            Global.RequestToDBServer(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_DB_MODIFYROLEGMAIL, dbCmds, out dbFields, client.ServerId);
            if (null == dbFields || dbFields.Length != 1)
            {
                return false;
            }

            int result = Convert.ToInt32(dbFields[0]);
            if (result <= 0)
            {
                return false;
            }

            SetGMailIsSend(client, gmailID);
            return true;
        }

        #endregion

    }
}
