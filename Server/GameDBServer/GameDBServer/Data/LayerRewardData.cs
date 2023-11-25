using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDBServer.DB;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 万魔塔扫荡单层数据
    /// </summary>
    [ProtoContract]
    public class SingleLayerRewardData
    {
        /// <summary>
        /// 层编号
        /// </summary>
        [ProtoMember(1)]
        public int nLayerOrder;

        /// <summary>
        /// 经验
        /// </summary>
        [ProtoMember(2)]
        public int nExp;

        /// <summary>
        /// 金币
        /// </summary>
        [ProtoMember(3)]
        public int nMoney;

        /// <summary>
        /// 星魂
        /// </summary>
        [ProtoMember(4)]
        public int nXinHun;

        /// <summary>
        /// 物品列表
        /// </summary>
        [ProtoMember(5)]
        public List<GoodsData> sweepAwardGoodsList = null;
    }

    /// <summary>
    /// 万魔塔扫荡数据
    /// </summary>
    [ProtoContract]
    public class LayerRewardData
    {
        /// <summary>
        /// 多层万魔塔奖励信息
        /// </summary>
        [ProtoMember(1)]
        public List<SingleLayerRewardData> WanMoTaLayerRewardList = null;     
    }

    /// <summary>
    /// 修改万魔塔数据时的信息结构
    /// </summary>
    [ProtoContract]
    public class ModifyWanMotaData
    {
        /// <summary>
        /// 参数信息
        /// </summary>
        [ProtoMember(1)]
        public string strParams;

        /// <summary>
        /// 扫荡奖励信息
        /// </summary>
        [ProtoMember(2)]
        public string strSweepReward;
    }

    /// <summary>
    /// 万魔塔数据
    /// </summary>
    [ProtoContract]
    public class WanMotaInfo
    {
        /// <summary>
        /// 万魔塔排行榜数据
        /// </summary>
        private PlayerWanMoTaRankingData rankingData;


        /// <summary>
        /// 角色ID
        /// </summary>
        [ProtoMember(1)]
        [DBMapping(ColumnName = "roleID")]
        public int nRoleID;

        /// <summary>
        /// 角色名
        /// </summary>
        [ProtoMember(2)]
        [DBMapping(ColumnName = "roleName")]
        public string strRoleName;

        /// <summary>
        /// 刷新时间
        /// </summary>
        [ProtoMember(3)]
        [DBMapping(ColumnName = "flushTime")]
        public long lFlushTime = 0;

        /// <summary>
        /// 通过层数
        /// </summary>
        [ProtoMember(4)]
        [DBMapping(ColumnName = "passLayerCount")]
        public int nPassLayerCount = 0;

        /// <summary>
        /// 扫荡成功的层数
        /// </summary>
        [ProtoMember(5)]
        [DBMapping(ColumnName = "sweepLayer")]
        public int nSweepLayer = 0;

        /// <summary>
        /// 各层扫荡成功后奖励各层奖励数据:经验、金钱、物品数据(字符串形式)
        /// </summary>
        [ProtoMember(6)]
        [DBMapping(ColumnName = "sweepReward")]
        public string strSweepReward = "";

        /// <summary>
        /// 扫荡开始时间
        /// </summary>
        [ProtoMember(7)]
        [DBMapping(ColumnName = "sweepBeginTime")]
        public long lSweepBeginTime = 0;

        /// <summary>
        /// 通关最高层
        /// </summary>
        [ProtoMember(8)]
        public int nTopPassLayerCount;

        /// <summary>
        /// 获取玩家万魔塔排行榜数据
        /// </summary>
        /// <returns></returns>
        public PlayerWanMoTaRankingData getPlayerWanMoTaRankingData()
        {
            return new PlayerWanMoTaRankingData(this);
        }
    }    
}
