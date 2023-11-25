using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
	/// <summary>
	/// Thông tin cổ tức
	/// </summary>
	[ProtoContract]
    public class GuildShareInfo
    {
        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(1)]
        public int TotalPage { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        [ProtoMember(2)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Danh sách thành viên có cổ tức
        /// </summary>
        [ProtoMember(3)]
        public List<GuildShare> MemberList { get; set; }
    }
}
