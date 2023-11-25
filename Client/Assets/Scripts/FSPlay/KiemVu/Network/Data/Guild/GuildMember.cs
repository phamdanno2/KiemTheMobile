using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thành viên trong bang
    /// </summary>
    [ProtoContract]
    public class GuildMember
    {
        /// <summary>
        /// ID Role
        /// </summary>
        [ProtoMember(1)]
        public int RoleID { get; set; }

        /// <summary>
        /// Tên của thành viên
        /// </summary>
        [ProtoMember(2)]
        public string RoleName { get; set; }

        /// <summary>
        /// Phái nào
        /// </summary>
        [ProtoMember(3)]
        public int FactionID { get; set; }

        /// <summary>
        /// Cấp bậc
        /// </summary>
        [ProtoMember(4)]
        public int Rank { get; set; }

        /// <summary>
        /// Cấp độ
        /// </summary>
        [ProtoMember(5)]
        public int Level { get; set; }

        /// <summary>
        /// Uy danh hiện có
        /// </summary>
        [ProtoMember(6)]
        public int Prestige { get; set; }

        /// <summary>
        /// ID của GUild hiện tại
        /// </summary>
        [ProtoMember(7)]
        public int GuildID { get; set; }

        /// <summary>
        /// ID gia tộc
        /// </summary>
        [ProtoMember(8)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        [ProtoMember(9)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Tài sản cá nhân
        /// </summary>
        [ProtoMember(10)]
        public int GuildMoney { get; set; }

        /// <summary>
        /// Trạng thái online
        /// </summary>
        [ProtoMember(11)]
        public int OnlienStatus { get; set; }

        /// <summary>
        /// ZoneID nào
        /// </summary>
        [ProtoMember(12)]
        public int ZoneID { get; set; }

        /// <summary>
        /// Nhận bao nhiêu phiếu bầu ưu tú
        /// </summary>
        [ProtoMember(13)]
        public int TotalVote { get; set; }

        /// <summary>
        /// Hạng ưu tú
        /// </summary>
        [ProtoMember(14)]
        public int VoteRank { get; set; }
    }
}
