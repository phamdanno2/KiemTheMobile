using GameServer.Core.Executor;
using System.Collections.Generic;

namespace GameServer.Logic
{
    /// <summary>
    /// 角色禁止登陆管理
    /// </summary>
    public class BanManager
    {
        #region 基础数据

        /// <summary>
        /// 封禁原因
        /// </summary>
        public enum BanReason
        {
            UseSpeedSoftware = 1, // 使用加速软件
            RobotTask = 2,          // 外挂
            TradeException = 3,     // 交易异常
        }

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
        public static void BanRoleName(string roleName, int banMinutes, int reason = 1)
        {
            lock (_RoleNameDict)
            {
                _RoleNameDict[roleName] = reason;
            }

            lock (_RoleNameTicksDict)
            {
                if (banMinutes > 0)
                {
                    _RoleNameTicksDict[roleName] = TimeUtil.NOW() + (banMinutes * 60 * 1000);
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
        public static int IsBanRoleName(string roleName, out int leftSecs)
        {
            leftSecs = 0;
            int reason = 0;
            lock (_RoleNameDict)
            {
                if (!_RoleNameDict.TryGetValue(roleName, out reason))
                {
                    reason = 0;
                }
            }

            if (reason > 0)
            {
                lock (_RoleNameTicksDict)
                {
                    long timeout = 0;
                    if (_RoleNameTicksDict.TryGetValue(roleName, out timeout) == false)
                    {
                        reason = 0;
                    }
                    else
                    {
                        long nowTicks = TimeUtil.NOW();
                        if (nowTicks >= timeout)
                        {
                            reason = 0;
                        }
                        else
                        {
                            leftSecs = (int)((timeout - nowTicks) / 1000);
                        }
                    }
                }
            }

            return reason;
        }

        #endregion 基本操作

        #region 内存中直接封账号

        private static object m_HourBanDictMutex = new object();

        private static Dictionary<int, Dictionary<string, int>> m_HourBanDict = new Dictionary<int, Dictionary<string, int>>();

        private static int m_UpdateHour = Global.GetOffsetHour(TimeUtil.NowDateTime());

        public static void BanUserID2Memory(string userID)
        {
            int nCurrHour = Global.GetOffsetHour(TimeUtil.NowDateTime());

            lock (m_HourBanDictMutex)
            {
                if (m_HourBanDict.ContainsKey(nCurrHour))
                {
                    if (!m_HourBanDict[nCurrHour].ContainsKey(userID))
                    {
                        m_HourBanDict[nCurrHour][userID] = 1;
                    }
                }
                else
                {
                    Dictionary<string, int> tmpDict = new Dictionary<string, int>();
                    tmpDict[userID] = 1;
                    m_HourBanDict[nCurrHour] = tmpDict;
                }
            }
        }

        public static void ClearBanMemory(int nHour)
        {
            int nCurrHour = Global.GetOffsetHour(TimeUtil.NowDateTime());
            int nMinHour = nCurrHour - nHour;

            // 要把这个时间段内的记录清空

            lock (m_HourBanDictMutex)
            {
                List<int> tempList = new List<int>();

                foreach (var item in m_HourBanDict)
                {
                    // 当前小时到 之前的一个小时都删掉
                    if (item.Key <= nCurrHour && item.Key >= nMinHour)
                    {
                        tempList.Add(item.Key);
                    }
                }

                foreach (var item in tempList)
                {
                    m_HourBanDict.Remove(item);
                }
            }
        }

        public static void CheckBanMemory()
        {
            int nCurrHour = Global.GetOffsetHour(TimeUtil.NowDateTime());

            if (m_UpdateHour == nCurrHour)
            {
                return;
            }

            int maxBanHour = 24;
            int nMinHour = nCurrHour - maxBanHour;
            lock (m_HourBanDictMutex)
            {
                List<int> tempList = new List<int>();

                foreach (var item in m_HourBanDict)
                {
                    if (item.Key < nMinHour)
                    {
                        tempList.Add(item.Key);
                    }
                }

                foreach (var item in tempList)
                {
                    m_HourBanDict.Remove(item);
                }
            }

            m_UpdateHour = nCurrHour;
        }

        public static bool IsBanInMemory(string userID)
        {
            //int maxBanHour = GameManager.GameConfigMgr.GetGameConfigItemInt("fileban_hour", 24);
            //int nCurrHour = Global.GetOffsetHour(TimeUtil.NowDateTime());
            //int nMinHour = nCurrHour - maxBanHour;

            lock (m_HourBanDictMutex)
            {
                foreach (var item in m_HourBanDict)
                {
                    if (item.Value.ContainsKey(userID))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion 内存中直接封账号
    }
}