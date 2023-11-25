using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Thông tin thành viên bang hội
    /// </summary>
    [ProtoContract]
    public class GuildMemberData
    {
        /// <summary>
        /// Danh sách tộc
        /// </summary>
        [ProtoMember(1)]
        public List<FamilyObj> TotalFamilyMemeber { get; set; }

        /// <summary>
        /// Danh sách thành viên
        /// </summary>
        [ProtoMember(2)]
        public List<GuildMember> TotalGuildMember { get; set; }

        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        [ProtoMember(3)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(4)]
        public int TotalPage { get; set; }
    }
}
