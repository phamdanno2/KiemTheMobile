using GameDBServer.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic.Rank
{
    /// <summary>
    /// 玩家在排行榜中的缓存结构
    /// </summary>
    public class UserRankValue
    {
        // 排行从数据加载的时间
        // 由于充值带有延时性，如果当前时间已在活动时间之外
        // 则从数据库重新加载一次保证排行的准确性
        public double QueryFromDBTime = 0;

        // 活动的起始时间
        public double BeginTime = 0;

        // 活动的结束时间
        public double EndTime = 0;

        // 排行榜
        public int RankValue = 0;
    }

    /// <summary>
    /// 玩家充值消费的缓存值
    /// </summary>
    public class UserRankValueCache
    {
        private int roleID = 0;

        // 缓存的锁
        private static Object UserRankValueDictLock = new Object();

        // 缓存内容
        private ConcurrentDictionary<string, UserRankValue> DictUserRankValue = new ConcurrentDictionary<string, UserRankValue>();

        /// <summary>
        /// 初始化 关联对象
        /// </summary>
        public void Init(int rid)
        {
            roleID = rid;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            //lock (UserRankValueDictLock)
            {
                DictUserRankValue.Clear();
            }
        }

        /// <summary>
        /// 取得缓存中的值
        /// </summary>
        public UserRankValue GetRankValueFromCache(RankDataKey key)
        {
            UserRankValue tmpRankData = null;

            //lock (UserRankValueDictLock)
            {
                if (DictUserRankValue.ContainsKey(key.GetKey()))
                {
                    tmpRankData = DictUserRankValue[key.GetKey()];
                }
            }
            return tmpRankData;
        }

        /// <summary>
        /// 修改缓存中的值
        /// </summary>
        public int AddUserRankValue(RankType ActType, int addValue)
        {
            double currSecond = Global.GetOffsetSecond(DateTime.Now);

            //lock (UserRankValueDictLock)
            {
                foreach (var item in DictUserRankValue)
                {
                    RankDataKey key = RankDataKey.GetKeyFromStr(item.Key.ToString());
                    if (null == key)
                    {
                        continue;
                    }

                    double startTime = Global.GetOffsetSecond(DateTime.Parse(key.StartDate));
                    double endTime = Global.GetOffsetSecond(DateTime.Parse(key.EndDate));

                    // 相同的活动类型，并且在活动期间内
                    if (ActType == key.rankType && currSecond >= startTime && currSecond <= endTime)
                    {
                        item.Value.RankValue += addValue;
                        //return item.Value.RankValue;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 先从缓存里取，内部有一些规则可能会重新从数据库加载
        /// </summary>
        private UserRankValue GetRankValueStruct(RankDataKey key)
        {
            UserRankValue tmpRankData = null;
            double currSecond = Global.GetOffsetSecond(DateTime.Now);

            tmpRankData = GetRankValueFromCache(key);

            if (null != tmpRankData)
            {
                // 如果活动还没结束 直接返回缓存里的数值
                if (tmpRankData.EndTime >= currSecond)
                {
                    return tmpRankData;
                }

                // 如果从数据库重新加载一的时间在结束时间之后 直接返回缓存里的数值
                if (tmpRankData.QueryFromDBTime > tmpRankData.EndTime)
                {
                    return tmpRankData;
                }
            }

            //lock (UserRankValueDictLock)
            {
                // 走到这里有两种情况：1.服务器还没有加载该排行榜，2.结束了，需要重新从数据库里加载准确排行
                tmpRankData = InitRankValue(key);
                DictUserRankValue[key.GetKey()] = tmpRankData;
                return tmpRankData;
            }
        }

        /// <summary>
        /// 直接取int型的数值
        /// 起始时间始终在变化的活动，请不要使用该系统
        /// </summary
        public int GetRankValue(RankDataKey key)
        {
            UserRankValue tmpRankData = null;

            tmpRankData = GetRankValueStruct(key);

            return null == tmpRankData ? 0 : tmpRankData.RankValue;
        }

        /// <summary>
        /// 从数据库里加载数据
        /// </summary
        public UserRankValue InitRankValue(RankDataKey key)
        {
            DBManager dbMgr = DBManager.getInstance();
            if (null == dbMgr)
            {
                return null;
            }

            //返回排行信息
            UserRankValue DBRankValue = null;

            if (RankType.Charge == key.rankType)
            {
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null != roleInfo)
                {
                    DBRankValue = GetUserInputRankVaule(dbMgr, roleInfo.UserID, roleInfo.ZoneID, key.StartDate, key.EndDate);
                }
            }
            else if (RankType.Consume == key.rankType)
            {
                DBRankValue = GetUserConsumeRankValue(dbMgr, key.StartDate, key.EndDate);
            }

            return DBRankValue;
        }

        /// <summary>
        /// 获取玩家充值额
        /// </summary
        public UserRankValue GetUserInputRankVaule(DBManager dbMgr, string userid, int zoneid, string fromDate, string toDate)
        {
            double currTime = Global.GetOffsetSecond(DateTime.Now);

            int input = DBQuery.GetUserInputMoney(dbMgr, userid, zoneid, fromDate, toDate);
            // 将其转换为元宝数量
            input = Global.TransMoneyToYuanBao(input);

            UserRankValue tmpRankData = new UserRankValue();
            tmpRankData.QueryFromDBTime = currTime;
            tmpRankData.BeginTime = Global.GetOffsetSecond(DateTime.Parse(fromDate));
            tmpRankData.EndTime = Global.GetOffsetSecond(DateTime.Parse(toDate));
            tmpRankData.RankValue = input;
            return tmpRankData;
        }

        /// <summary>
        /// 获取玩家消费额
        /// </summary
        public UserRankValue GetUserConsumeRankValue(DBManager dbMgr, string fromDate, string toDate)
        {
            double currTime = Global.GetOffsetSecond(DateTime.Now);

            int consume = DBQuery.GetUserUsedMoney(dbMgr, roleID, fromDate, toDate);

            UserRankValue tmpRankData = new UserRankValue();
            tmpRankData.QueryFromDBTime = currTime;
            tmpRankData.BeginTime = Global.GetOffsetSecond(DateTime.Parse(fromDate));
            tmpRankData.EndTime = Global.GetOffsetSecond(DateTime.Parse(toDate));
            tmpRankData.RankValue = consume;
            return tmpRankData;
        }
    }
}
