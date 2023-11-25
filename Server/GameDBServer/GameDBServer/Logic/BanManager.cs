using GameDBServer.DB;
using MySQLDriverCS;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 角色禁止登陆管理
    /// </summary>
    public class BanManager
    {
        #region 基础数据

        /// <summary>
        /// 存储禁止登陆的角色字典
        /// </summary>
        private static Dictionary<string, int> _RoleNameDict = new Dictionary<string, int>();

        /// <summary>
        /// 存储禁止登陆的角色时间字典
        /// </summary>
        private static Dictionary<string, long> _RoleNameTicksDict = new Dictionary<string, long>();

        #endregion 基础数据

        #region 基本操作

        /// <summary>
        /// 设置是否禁止某个角色名称
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="state"></param>
        public static void BanRoleName(string roleName, int state)
        {
            lock (_RoleNameDict)
            {
                _RoleNameDict[roleName] = state;
            }

            lock (_RoleNameTicksDict)
            {
                if (state > 0)
                {
                    _RoleNameTicksDict[roleName] = DateTime.Now.Ticks / 10000;
                }
                else
                {
                    _RoleNameTicksDict[roleName] = 0;
                }
            }
        }

        /// <summary>
        /// 查询是否被禁止登陆
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static int IsBanRoleName(string roleName)
        {
            int state = 0;
            lock (_RoleNameDict)
            {
                if (!_RoleNameDict.TryGetValue(roleName, out state))
                {
                    state = 0;
                }
            }

            if (state > 0)
            {
                lock (_RoleNameTicksDict)
                {
                    long oldTicks = 0;
                    if (!_RoleNameTicksDict.TryGetValue(roleName, out oldTicks))
                    {
                        state = 0;
                    }
                    else
                    {
                        long nowTicks = DateTime.Now.Ticks / 10000;
                        if (nowTicks - oldTicks >= (state * 60 * 1000))
                        {
                            state = 0;
                        }
                    }
                }
            }

            return state;
        }

        #endregion 基本操作

        #region ----------Ban

        public static int GmBanCheckAdd(DBManager dbMgr, int roleID, string banIDs)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                String strTableName = "t_ban_check";
                string cmdText = string.Format("INSERT INTO {0} (roleID, banIDs, logTime) VALUES({1}, '{2}', now())", strTableName, roleID, banIDs);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        public static int GmBanCheckClear(DBManager dbMgr)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                String strTableName = "t_ban_check";
                string cmdText = string.Format("DELETE FROM {0}", strTableName);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        public static int GmBanLogAdd(DBManager dbMgr, int zoneID, string userID,int roleID,int banType,string banID, int banCount, string deviceID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                String strTableName = "t_ban_log";
                string cmdText = string.Format(
                    "INSERT INTO {0} (zoneID, userID, roleID, banType, banID, banCount, logTime, deviceID) VALUES({1}, '{2}', {3}, {4}, '{5}',{6}, now(), '{7}')",
                    strTableName, zoneID, userID, roleID, banType, banID, banCount, deviceID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }



        #endregion

    }
}
