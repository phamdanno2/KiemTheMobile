using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 物品使用限制数据
    /// </summary>
    [ProtoContract]
    public class GoodsLimitData
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(1)]
        public int GoodsID;

        /// <summary>
        /// 日期ID
        /// </summary>
        [ProtoMember(2)]
        public int DayID;

        /// <summary>
        /// 已经使用次数
        /// </summary>
        [ProtoMember(3)]
        public int UsedNum;
    }
}
