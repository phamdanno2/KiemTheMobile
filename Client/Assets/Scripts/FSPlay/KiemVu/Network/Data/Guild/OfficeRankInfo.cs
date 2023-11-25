using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Thông tin quan hàm
    /// </summary>
    [ProtoContract]
    public class OfficeRankInfo
    {
        /// <summary>
        /// Cấp hiện tại của bang
        /// </summary>
        [ProtoMember(1)]
        public int GuildRank { get; set; }

        /// <summary>
        /// Danh sách thành viên và quan hàm Top đầu
        /// </summary>
        [ProtoMember(2)]
        public List<OfficeRankMember> OffcieRankMember { get; set; }
    }
}
