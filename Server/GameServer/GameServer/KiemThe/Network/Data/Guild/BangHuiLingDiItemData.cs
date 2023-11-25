using ProtoBuf;
using System;

namespace Server.Data
{
    /// <summary>
    /// 领地占领数据(简单)
    /// </summary>
    [ProtoContract]
    public class BangHuiLingDiItemData
    {
        /// <summary>
        /// 领地ID
        /// </summary>
        [ProtoMember(1)]
        public int LingDiID = 0;

        /// <summary>
        /// 帮派的ID
        /// </summary>
        [ProtoMember(2)]
        public int BHID = 0;

        /// <summary>
        /// 区的ID
        /// </summary>
        [ProtoMember(3)]
        public int ZoneID = 0;

        /// <summary>
        /// 帮派的名称
        /// </summary>
        [ProtoMember(4)]
        public string BHName = "";

        /// <summary>
        /// 领地税率
        /// </summary>
        [ProtoMember(5)]
        public int LingDiTax = 0;

        /// <summary>
        /// 战盟战争请求字段
        /// </summary>
        [ProtoMember(6)]
        public String WarRequest = "";

        /// <summary>
        /// 领地每日奖励领取日
        /// </summary>
        [ProtoMember(7)]
        public int AwardFetchDay = -1;
    }
}