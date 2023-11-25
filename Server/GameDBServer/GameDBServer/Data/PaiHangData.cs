using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 排行榜数据
    /// </summary>
    [ProtoContract]
    public class PaiHangData
    {
        /// <summary>
        /// 返回的排行榜类型
        /// </summary>
        [ProtoMember(1)]
        public int PaiHangType;

        /// <summary>
        /// 排行榜列表数据
        /// </summary>
        [ProtoMember(2)]
        public List<PaiHangItemData> PaiHangList = null;
    }
}
