using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin bang hội
    /// </summary>
    [ProtoContract]
    public class GuildInfomation
    {
        /// <summary>
        /// Id Bang hội
        /// </summary>
        [ProtoMember(1)]
        public int GuildID { get; set; }

        /// <summary>
        /// Tên bang hội
        /// </summary>
        [ProtoMember(2)]
        public string GuildName { get; set; }

        /// <summary>
        /// Tổng số gia tộc
        /// </summary>
        [ProtoMember(3)]
        public int FamilyCount { get; set; }

        /// <summary>
        /// Tổng số thành viên
        /// </summary>
        [ProtoMember(4)]
        public int MemberCount { get; set; }

        /// <summary>
        /// Tổng số lãnh thổ
        /// </summary>
        [ProtoMember(5)]
        public int TerritoryCount { get; set; }

        /// <summary>
        /// Tổng uy danh
        /// </summary>
        [ProtoMember(6)]
        public int TotalPrestige { get; set; }

        /// <summary>
        /// Quỹ bang
        /// </summary>
        [ProtoMember(7)]
        public int MoneyStore { get; set; }

        /// <summary>
        /// Lợi tức tối đa có thể rút
        /// </summary>
        [ProtoMember(8)]
        public int MaxWithDraw { get; set; }

        /// <summary>
        /// Quỹ thưởng
        /// </summary>
        [ProtoMember(9)]
        public int MoneyBound { get; set; }

        /// <summary>
        /// Tài sản cá nhân
        /// </summary>
        [ProtoMember(10)]
        public int RoleGuildMoney { get; set; }

        /// <summary>
        /// Thông báo bang hội
        /// </summary>
        [ProtoMember(11)]
        public string Notify { get; set; }

        /// <summary>
        /// Tên bang chủ
        /// </summary>
        [ProtoMember(12)]
        public string GuildMasterName { get; set; }
    }
}
