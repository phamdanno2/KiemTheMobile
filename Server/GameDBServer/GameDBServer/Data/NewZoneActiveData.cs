using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 新服活动数据，用于除冲级达人以外数据，屠魔勇士，充值达人，消费达人，返利达人
    /// </summary>
    [ProtoContract]
    public class NewZoneActiveData
    {
        /// <summary>
        /// 充值返利
        /// </summary>
        [ProtoMember(1)]
        public int YuanBao;

        /// <summary>
        /// 活动id
        /// </summary>
        [ProtoMember(2)]
        public int ActiveId;

        /// <summary>
        /// 领取状态
        /// </summary>
        [ProtoMember(3)]
        public int GetState;

        /// <summary>
        /// 排行榜数据
        /// </summary>
        [ProtoMember(4)]
        public List<PaiHangItemData> Ranklist;


    }
}
