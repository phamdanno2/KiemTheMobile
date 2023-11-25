using GameDBServer.DB;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic.Rank
{
    // 玩家的排行数据
    /*public class UserRankData
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID;

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID;

        /// <summary>
        /// 名次
        /// </summary>
        public int Ranking;

        /// <summary>
        /// 排行值，用于计算排行的值
        /// </summary>
        public int RankValue;
    }*/

    /// <summary>
    /// 排行榜数据结构
    /// </summary>
    public class RankData
    {
        // 排行最后一次从数据库加载的时间
        // 由于充值带有延时性，在时间临界点附近缓存中的数值可能会和数据库中的数据不一致
        // 如果当前时间已在活动时间之外，则从数据库重新加载一次保证排行的准确性
        public double QueryFromDBTime;

        // 最大排行数量
        public double MaxRankCount;

        // 排行规则
        public List<int> minGateValueList = null;

        // 排行榜数据
        public List<InputKingPaiHangData> RankDataList = null;
    }

    // 活动的类型
    public enum RankType
    {
        UnKnown = 0,    
        Charge = 1,     // 充值
        Consume = 2,    // 消费
    }

    /// <summary>
    /// 排行榜缓存字典的Key
    /// </summary>
    public class RankDataKey
    {
        public RankDataKey()
        {
            rankType = RankType.UnKnown;
            StartDate = "2011-11-11 11:11:11";
            EndDate = "2011-11-11 11:11:11";
        }

        /// <summary>
        /// 生成一个缓存Key
        /// </summary>
        public RankDataKey(RankType type, string fromDate, string toDate, List<int> minValueList)
        {
            rankType = type;

            // 由于各个活动传递过来的date格式不一致
            // 直接做Key会导致找不到对应的缓存
            DateTime tmpTime;
            if (DateTime.TryParse(fromDate, out tmpTime))
            {
                StartDate = tmpTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (DateTime.TryParse(toDate, out tmpTime))
            {
                EndDate = tmpTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            minGateValueList = minValueList;
        }

        /// <summary>
        /// 生成一个缓存key
        /// </summary>
        public RankDataKey(RankType type, DateTime fromDate, DateTime toDate, List<int> minValueList)
        {
            rankType = type;
            StartDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
            EndDate = toDate.ToString("yyyy-MM-dd HH:mm:ss");
            minGateValueList = minValueList;
        }

        /// <summary>
        /// 活动类型
        /// </summary>
        public RankType rankType = RankType.UnKnown;

        /// <summary>
        /// 活动起始时间
        /// </summary>
        public string StartDate = "2011-11-11 11:11:11";

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public string EndDate = "2011-11-11 11:11:11";

        /// <summary>
        /// 排行的档位
        /// 有可能活动类型、起始时间相同，但是排行的档次不同~~~ 所以还得把档次也加上
        /// </summary>
        private List<int> minGateValueList = null;

        private static char SplitChar = '_';

        /// <summary>
        /// 取得一个活动的string 用做字典的key
        /// </summary>
        public string GetKey()
        {
            string keyStr = ((int)rankType).ToString() + "_" + StartDate + "_" + EndDate;

            if (null != minGateValueList)
            {
                foreach (var item in minGateValueList)
                {
                    keyStr += "_";
                    keyStr += item.ToString();
                }
            }

            return keyStr;
        }

        /// <summary>
        /// 把一个string转回RankDataKey
        /// </summary>
        public static RankDataKey GetKeyFromStr(string key)
        {
            string[] fields = key.Split(SplitChar);
            if (null == fields || fields.Length < 3)
            {
                return null;
            }

            RankDataKey rankDataKey = new RankDataKey();
            rankDataKey.rankType = (RankType)Global.SafeConvertToInt32(fields[0]);
            rankDataKey.StartDate = fields[1];
            rankDataKey.EndDate = fields[2];
            for (int i = 3; i < fields.Length; i++)
            {
                if (null == rankDataKey.minGateValueList)
                {
                    rankDataKey.minGateValueList = new List<int>();
                }
                rankDataKey.minGateValueList.Add(int.Parse(fields[i]));
            }

            return rankDataKey;
        }
    }

    // 玩家充值/消费排行的缓存列表
    public class RankCacheManager
    {
        #region 成员

        /// <summary>
        /// 排行缓存的锁
        /// </summary>
        private Object RankDataDictLock = new Object();

        /// <summary>
        /// 排行的缓存
        /// key类似：1_2015-0101-2015-01-02
        /// </summary>
        private Dictionary<string, RankData> RankDataDict = new Dictionary<string, RankData>();

        /// <summary>
        /// 打印当前缓存内容
        /// GM专用
        /// </summary>
        public void PrintfRankData()
        {
            LogManager.WriteLog(LogTypes.Error, "RankDataDict开始输出");
            lock (RankDataDictLock)
            {
                // 如果没在缓存里 则说明没有玩家进行过查询操作 第一次查询会从数据库加载 无需再这里做处理
                foreach (var item in RankDataDict)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("RankDataKey = {0}", item.Key));
                    foreach (var rankData in item.Value.RankDataList)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("rankData 名次={0}, UserID={1}, 数值={2}, 更新时间={3}", rankData.PaiHang, rankData.UserID, rankData.PaiHangValue, rankData.PaiHangTime));
                    }
                }
            }
            LogManager.WriteLog(LogTypes.Error, "RankDataDict结束输出");
        }

        #endregion

        #region 玩家行为

        /// <summary>
        /// 玩家某些操作的响应 例如充值/消费
        /// </summary>
        public void OnUserDoSomething(int roleID, RankType rankType, int value)
        {
            DBManager dbMgr = DBManager.getInstance();
            if (null == dbMgr)
            {
                return;
            }

            // 查找玩家数据
            DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
            if (null == roleInfo)
            {
                return;
            }

            double currSecond = Global.GetOffsetSecond(DateTime.Now);
            // 更新玩家数据
            /*int newValue = */roleInfo.RankValue.AddUserRankValue(rankType, value);

            lock (RankDataDictLock)
            {
                // 如果没在缓存里 则说明没有玩家进行过查询操作 第一次查询会从数据库加载 无需再这里做处理
                foreach (var item in RankDataDict)
                {
                    RankDataKey rankDataKey = RankDataKey.GetKeyFromStr(item.Key);

                    if (null == rankDataKey)
                    {
                        continue;
                    }

                    // 不是同一种类型，不计入排行榜
                    if (rankType != rankDataKey.rankType)
                    {
                        continue;
                    }

                    double startTime = Global.GetOffsetSecond(DateTime.Parse(rankDataKey.StartDate));
                    double endTime = Global.GetOffsetSecond(DateTime.Parse(rankDataKey.EndDate));

                    // 不在活动时间内，不计入排行榜
                    if (currSecond < startTime || currSecond > endTime)
                    {
                        continue;
                    }

                    bool bExist = false;
                    // 如果玩家已经在排行榜中，则直接刷新排行榜的值
                    foreach (var rankData in item.Value.RankDataList)
                    {
                        // 充值比较userid
                        if ((RankType.Charge == rankType && rankData.UserID == roleInfo.UserID)
                            // 消费比较roleid
                            || (RankType.Consume == rankType && rankData.UserID == roleInfo.RoleID.ToString()))
                        {
                            rankData.PaiHangTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            rankData.PaiHangValue += value;
                            bExist = true;
                            break;
                        }
                    }
                    // 如果玩家没在排行榜中，则把玩家的数据add到列表中并重新排序
                    if (!bExist)
                    {
                        int userRankValue = roleInfo.RankValue.GetRankValue(rankDataKey);
                        // 更新排行榜数据
                        InputKingPaiHangData phData = new InputKingPaiHangData()
                        {
                            // 充值用userid 消费用roleid
                            UserID = RankType.Charge == rankType ? roleInfo.UserID : roleInfo.RoleID.ToString(),
                            PaiHang = 0,
                            PaiHangTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            // ?????
                            PaiHangValue = userRankValue,// + value,
                        };
                        item.Value.RankDataList.Add(phData);
                    }

                    BuildRank(item.Value);
                }
            }
        }

        #endregion

        #region

        /// <summary>
        /// 取得一份排行数据的副本
        /// </summary>
        public List<InputKingPaiHangData> GetRankDataList(RankData rankData)
        {
            lock (RankDataDictLock)
            {
                // 进行深拷贝
                byte[] retBytes = DataHelper.ObjectToBytes<List<InputKingPaiHangData>>(rankData.RankDataList);
                return DataHelper.BytesToObject<List<InputKingPaiHangData>>(retBytes, 0, retBytes.Length);
            }
        }

        /// <summary>
        /// 直接从缓存里取数据
        /// </summary>
        public RankData GetRankDataFromCache(RankDataKey key)
        {
            RankData tmpRankData = null;
            
            lock (RankDataDictLock)
            {
                if (RankDataDict.ContainsKey(key.GetKey()))
                {
                    tmpRankData = RankDataDict[key.GetKey()];
                }
            }
            return tmpRankData;
        }

        /// <summary>
        /// 取排行榜数据
        /// 先从缓存里取，内部有一些规则可能需要重新从数据库加载
        /// </summary>
        public RankData GetRankData(RankDataKey key, List<int> minGateValueList, int maxPaiHang)
        {
            DBManager dbMgr = DBManager.getInstance();
            if (null == dbMgr)
            {
                return null;
            }

            RankData tmpRankData = null;
            double currSecond = Global.GetOffsetSecond(DateTime.Now);

            tmpRankData = GetRankDataFromCache(key);

            if (null != tmpRankData)
            {
                double endTime = Global.GetOffsetSecond(DateTime.Parse(key.EndDate));
                // 如果活动还没结束 直接返回缓存里的数值
                if (endTime >= currSecond)
                {
                    return tmpRankData;
                }

                // 如果从数据库重新加载一的时间在结束时间之后 直接返回缓存里的数值
                if (tmpRankData.QueryFromDBTime > endTime)
                {
                    return tmpRankData;
                }
            }

            lock (RankDataDictLock)
            {
                // 走到这里有两种情况：1.服务器还没有加载该排行榜，2.结束了，需要重新从数据库里加载准确排行
                tmpRankData = InitRankData(key, minGateValueList, maxPaiHang);
                RankDataDict[key.GetKey()] = tmpRankData;
                return tmpRankData;
            }
        }

        /// <summary>
        /// 初始化某排行榜
        /// </summary>
        public RankData InitRankData(RankDataKey key, List<int> minGateValueList, int maxPaiHang)
        {
            DBManager dbMgr = DBManager.getInstance();
            if (null == dbMgr)
            {
                return null;
            }

            //返回排行信息
            RankData DBRankData = null;

            if (RankType.Charge == key.rankType)
            {
                DBRankData = GetUserInputRank(dbMgr, key.StartDate, key.EndDate, minGateValueList, maxPaiHang);
            }
            else if (RankType.Consume == key.rankType)
            {
                DBRankData = GetUserConsumeRank(dbMgr, key.StartDate, key.EndDate, minGateValueList, maxPaiHang);
            }
            
            return DBRankData;
        }

        /// <summary>
        /// 对data中的排行榜数据重新进行排序
        /// </summary>
        private void BuildRank(RankData rankData)
        {
            if (null == rankData)
            {
                return ;
            }

            // 先进行下排序
            rankData.RankDataList.Sort(delegate(InputKingPaiHangData x, InputKingPaiHangData y)
            {
                if (y.PaiHangValue == x.PaiHangValue)
                {
                    double xTime = Global.GetOffsetSecond(DateTime.Parse(x.PaiHangTime));
                    double yTime = Global.GetOffsetSecond(DateTime.Parse(y.PaiHangTime));

                    // 如果两个玩家的排行相同，那么先充值的排在前面
                    return (int)(xTime - yTime);
                }

                // suitid 从大到小
                return y.PaiHangValue - x.PaiHangValue;
            });

            List<InputKingPaiHangData> listPaiHang = new List<InputKingPaiHangData>();

            // 如果档次不为空就进行删选
            if (null != rankData.minGateValueList)
            {
                //上一个玩家的排行 0 表示上个玩家没有排行
                int preUserPaiHang = 0;

                //重整排行表
                for (int n = 0; n < rankData.RankDataList.Count; n++)
                {
                    InputKingPaiHangData phData = rankData.RankDataList[n];
                    phData.PaiHang = -1;

                    //不满足该排行位置需要的最低数值要求，则依次降低排名
                    for (int i = preUserPaiHang; i < rankData.minGateValueList.Count; i++)
                    {
                        if (phData.PaiHangValue >= rankData.minGateValueList[i])
                        {
                            phData.PaiHang = i + 1;
                            listPaiHang.Add(phData);

                            preUserPaiHang = phData.PaiHang;

                            break;
                        }
                    }

                    //这个靠前的排行数据没有满足排行值限制的最小要求，则其后的排行数据也满足不了
                    if (phData.PaiHang < 0 || phData.PaiHang >= rankData.minGateValueList.Count)
                    {
                        break;
                    }
                }

                rankData.RankDataList = listPaiHang;
            }
            else
            {
                // 无需处理
            }
        }

        /// <summary>
        /// 从数据库中获取玩家充值排行榜
        /// </summary>
        public RankData GetUserInputRank(DBManager dbMgr, string fromDate, string toDate, List<int> minGateValueList, int maxPaiHang)
        {
            double currTime = Global.GetOffsetSecond(DateTime.Now);

            List<InputKingPaiHangData> listPaiHangReal = DBQuery.GetUserInputPaiHang(dbMgr, fromDate, toDate, maxPaiHang);

            RankData tmpRankData = new RankData();
            tmpRankData.QueryFromDBTime = currTime;
            tmpRankData.MaxRankCount = maxPaiHang;
            tmpRankData.minGateValueList = minGateValueList;
            tmpRankData.RankDataList = listPaiHangReal;

            // 重置排行
            BuildRank(tmpRankData);

            return tmpRankData;
        }

        /// <summary>
        /// 从数据库中获取玩家消费排行榜
        /// </summary>
        public RankData GetUserConsumeRank(DBManager dbMgr, string fromDate, string toDate, List<int> minGateValueList, int maxPaiHang)
        {
            double currTime = Global.GetOffsetSecond(DateTime.Now);

            //返回排行信息
            List<InputKingPaiHangData> listPaiHangReal = DBQuery.GetUserUsedMoneyPaiHang(dbMgr, fromDate, toDate, maxPaiHang);

            RankData tmpRankData = new RankData();
            tmpRankData.QueryFromDBTime = currTime;
            tmpRankData.MaxRankCount = maxPaiHang;
            tmpRankData.minGateValueList = minGateValueList;
            tmpRankData.RankDataList = listPaiHangReal;

            // 重置排行
            BuildRank(tmpRankData);

            return tmpRankData;
        }
        
        #endregion
    }
}
