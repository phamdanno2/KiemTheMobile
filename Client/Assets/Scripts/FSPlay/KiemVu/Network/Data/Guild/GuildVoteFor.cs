using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Thông tin ưu tú bang
    /// </summary>
    [ProtoContract]
    public class GuildVoteInfo
    {
        /// <summary>
        /// ID tuần
        /// </summary>
        [ProtoMember(1)]
        public int WEEID { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        [ProtoMember(2)]
        public string Start { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [ProtoMember(3)]
        public string End { get; set; }

        /// <summary>
        /// Thành viên mà bản thân đã bầu
        /// </summary>
        [ProtoMember(4)]
        public string VoteFor { get; set; }

        /// <summary>
        /// Danh sách thành viên ưu tú
        /// </summary>
        [ProtoMember(5)]
        public List<GuildMember> GuildMember { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(6)]
        public int TotalPage { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        [ProtoMember(7)]
        public int PageIndex { get; set; }
    }
}
