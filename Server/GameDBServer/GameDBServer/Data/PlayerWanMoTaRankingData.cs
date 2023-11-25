using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using GameDBServer.DB;

namespace Server.Data
{
    /// <summary>
    /// 万魔塔排行榜数据封装类
    /// </summary>
    [ProtoContract]
    public class PlayerWanMoTaRankingData : IComparable<PlayerWanMoTaRankingData>
    {
        public WanMotaInfo wanmotaData;

        public PlayerWanMoTaRankingData(WanMotaInfo wanmotaData)
        {
            this.wanmotaData = wanmotaData;
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        [ProtoMember(1)]
        public int roleId 
        {
            get { return wanmotaData.nRoleID; }
        }

        /// <summary>
        /// 玩家名称
        /// </summary>
        [ProtoMember(2)]
        public string roleName
        {
            get { return wanmotaData.strRoleName; }
        }

        /// <summary>
        /// 玩家战力
        /// </summary>
        [ProtoMember(3)]
        public long flushTime
        {
            get { return wanmotaData.lFlushTime; }
        }

        /// <summary>
        /// 玩家排名
        /// </summary>
        [ProtoMember(4)]
        public int passLayerCount
        {
            get { return wanmotaData.nPassLayerCount; }
        }

        private PaiHangItemData paiHangItemData = new PaiHangItemData();

        public void UpdateData(WanMotaInfo data)
        {
            wanmotaData = data;
        }

        public PaiHangItemData getPaiHangItemData()
        {
            paiHangItemData.RoleID = roleId;
            paiHangItemData.RoleName = roleName;
            paiHangItemData.Val1 = passLayerCount;

            return paiHangItemData;
        }


        public int CompareTo(PlayerWanMoTaRankingData other)
        {
            if (this.passLayerCount == other.passLayerCount)
            {
                // 通关层数相同，则按刷新时间从小到大排序
                return this.flushTime < other.flushTime ? -1 : this.flushTime == other.flushTime ? 0 : 1;
            }
            else
            {
                return this.passLayerCount < other.passLayerCount ? 1 : -1;
            }
        }
    }
}
