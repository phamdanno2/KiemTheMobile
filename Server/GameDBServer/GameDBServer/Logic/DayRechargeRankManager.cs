using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Data;
using GameDBServer.DB;
using GameDBServer.Logic.Rank;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 充值排行的缓存，目前只有合服充值返利用到
    /// </summary>
    public class DayRechargeRankManager
    {
        /// <summary>
        /// 合服时期充值排行个数
        /// </summary>
        private const int HeFuRankCount = 4;


        /// <summary>
        /// 合服时期充值排行缓存
        /// key根据根据Global.GetOffsetDay生成
        /// </summary>
        private Dictionary<int, List<InputKingPaiHangData> > RechargeRankDict = new Dictionary<int, List<InputKingPaiHangData> > ();

        /// <summary>
        /// 返回某天的记录
        /// </summary>
        public List<InputKingPaiHangData> GetRankByDay(DBManager dbMgr, int day)
        {
            List<InputKingPaiHangData> ranklist = null;

            int currDay = Global.GetOffsetDay(DateTime.Now);

            // 如果要取明天的记录 直接返回null
            if (day > currDay)
                return null;

            // 如果取昨天的，先去缓存里找 找到了就返回
            if (day < currDay)
            {
                lock (RechargeRankDict)
                {
                    // 检查缓存是否已经存在
                    if (RechargeRankDict.ContainsKey(day))
                    {
                        ranklist = RechargeRankDict[day];
                        return ranklist;
                    }
                }
            }

            List<int> minGateValueList = new List<int>();
            for (int i = 0; i < HeFuRankCount; i++)
                minGateValueList.Add(1);

            DateTime now = Global.GetRealDate(day); ;
            string startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

            ranklist = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, minGateValueList, HeFuRankCount);
            if (null == ranklist)
                return null;

            //更新最大等级角色名称 和区号
            foreach (var item in ranklist)
            {
                Global.GetUserMaxLevelRole(dbMgr, item.UserID, out item.MaxLevelRoleName, out item.MaxLevelRoleZoneID);
            }

            lock (RechargeRankDict)
            {
                RechargeRankDict[day] = ranklist;
            }

            return ranklist;
        }


        /// <summary>
        /// 返回某人某天的排行
        /// </summary>
        public int GetRoleRankByDay(DBManager dbMgr, string userid, int day)
        {
            List<InputKingPaiHangData> ranklist = GetRankByDay(dbMgr, day);
            if (null == ranklist)
                return 0;

            int rank = 0;
            foreach (var item in ranklist)
            {
                if (string.Compare(userid, item.UserID) == 0)
                    return rank + 1;

                ++rank;
            }

            return 0;
        }
    }
}
