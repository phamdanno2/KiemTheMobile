using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Thông tin lãnh thổ của bang
    /// </summary>
	[ProtoContract]
    public class TerritoryInfo
    {
        /// <summary>
        /// Tổng số lãnh thổ
        /// </summary>
        [ProtoMember(1)]
        public int TerritoryCount { get; set; }

        /// <summary>
        /// Danh sách lãnh thổ
        /// </summary>
        [ProtoMember(2)]
        public List<Territory> Territorys { get; set; }

    }
}
