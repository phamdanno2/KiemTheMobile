using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin thành viên bang hội
    /// </summary>
    [ProtoContract]
    public class BangHuiMemberData
    {
        /// <summary>
        /// ID khu vực
        /// </summary>
        [ProtoMember(1)]
        public int ZoneID { get; set; } = 0;

        /// <summary>
        /// ID thành viên
        /// </summary>
        [ProtoMember(2)]
        public int RoleID { get; set; } = 0;

        /// <summary>
        /// Tên thành viên
        /// </summary>
        [ProtoMember(3)]
        public string RoleName { get; set; } = "";

        /// <summary>
        /// Môn phái
        /// </summary>
        [ProtoMember(4)]
        public int Occupation { get; set; } = 0;

        /// <summary>
        /// Chức vụ trong Bang
        /// </summary>
        [ProtoMember(5)]
        public int BHZhiwu { get; set; } = 0;

        /// <summary>
        /// Danh hiệu bang
        /// </summary>
        [ProtoMember(6)]
        public string ChengHao { get; set; } = "";

        /// <summary>
        /// Bang cống
        /// </summary>
        [ProtoMember(7)]
        public int BangGong { get; set; } = 0;

        /// <summary>
        /// Cấp độ
        /// </summary>
        [ProtoMember(8)]
        public int Level { get; set; } = 0;

        /// <summary>
        /// Trạng thái Online
        /// </summary>
        [ProtoMember(9)]
        public int OnlineState { get; set; } = 0;
    }
}
