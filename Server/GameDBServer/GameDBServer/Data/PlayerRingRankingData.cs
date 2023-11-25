using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using GameDBServer.DB;

namespace Server.Data
{
    [ProtoContract]
    public class RingRankingInfo
    {
        /// <summary>
        /// 翅膀塔排行榜数据
        /// </summary>
        private PlayerRingRankingData rankingData;

        /// <summary>
        /// 角色ID
        /// </summary>
        [ProtoMember(1)]
        [DBMapping(ColumnName = "roleid")]
        public int nRoleID;

        /// <summary>
        /// 角色名
        /// </summary>
        [ProtoMember(2)]
        [DBMapping(ColumnName = "rolename")]
        public string strRoleName;

        /// <summary>
        /// 进阶数
        /// </summary>
        [ProtoMember(3)]
        [DBMapping(ColumnName = "goodwilllevel")]
        public int byGoodwilllevel = 0;

        // <summary>
        /// 升星数
        /// </summary>
        [ProtoMember(4)]
        [DBMapping(ColumnName = "goodwillstar")]
        public int byGoodwillstar = 0;

        /// <summary>
        /// 婚戒ID
        /// </summary>
        [ProtoMember(5)]
        [DBMapping(ColumnName = "ringid")]
        public int nRingID;

        /// <summary>
        /// 翅膀创建时间
        /// </summary>
        [ProtoMember(6)]
        [DBMapping(ColumnName = "changtime")]
        public String strAddTime = "";

        /// <summary>
        /// 获取玩家结婚排行榜数据
        /// </summary>
        /// <returns></returns>
        public PlayerRingRankingData getPlayerRingRankingData()
        {
            return new PlayerRingRankingData(this);
        }
    }

    /// <summary>
    /// 婚姻排行榜数据封装类
    /// </summary>
    [ProtoContract]
    public class PlayerRingRankingData : IComparable<PlayerRingRankingData>
    {
        public RingRankingInfo ringData;

        public PlayerRingRankingData(RingRankingInfo ringData)
        {
            this.ringData = ringData;
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        [ProtoMember(1)]
        public int roleId
        {
            get { return ringData.nRoleID; }
        }

        /// <summary>
        /// 玩家名称
        /// </summary>
        [ProtoMember(2)]
        public string roleName
        {
            get { return ringData.strRoleName; }
        }

        /// <summary>
        /// 婚戒ID
        /// </summary>
        [ProtoMember(3)]
        public int RingID
        {
            get { return ringData.nRingID; }
        }

        /// <summary>
        /// 结婚添加时间
        /// </summary>
        [ProtoMember(4)]
        public string RingAddTime
        {
            get { return ringData.strAddTime; }
        }

        /// <summary>
        /// 亲密度阶数
        /// </summary>
        [ProtoMember(5)]
        public int GoodWillLevel
        {
            get { return ringData.byGoodwilllevel; }
        }

        /// <summary>
        /// 亲密度星数
        /// </summary>
        [ProtoMember(6)]
        public int GoodWillStar
        {
            get { return ringData.byGoodwillstar; }
        }

        private PaiHangItemData paiHangItemData = new PaiHangItemData();

        public void UpdateData(RingRankingInfo data)
        {
            ringData = data;
        }

        public PaiHangItemData getPaiHangItemData()
        {
            paiHangItemData.RoleID = roleId;
            paiHangItemData.RoleName = roleName;
            paiHangItemData.Val1 = GoodWillLevel;
            paiHangItemData.Val2 = GoodWillStar;
            paiHangItemData.Val3 = RingID;

            return paiHangItemData;
        }


        public int CompareTo(PlayerRingRankingData other)
        {
            if (this.GoodWillLevel == other.GoodWillLevel)
            {
                if (this.GoodWillStar == other.GoodWillStar)
                {
                    int nRet = String.Compare(this.RingAddTime, other.RingAddTime);
                    // 阶数和星数相同，则按刷新时间从小到大排序
                    return nRet < 0 ? -1 : nRet == 0 ? 0 : 1;
                }
                else
                {
                    return this.GoodWillStar < other.GoodWillStar ? 1 : -1;
                }
            }
            else
            {
                return this.GoodWillLevel < other.GoodWillLevel ? 1 : -1;
            }
        }
    }
}
