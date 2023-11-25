using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin thành viên và quan hàm tương ứng
    /// </summary>
    [ProtoContract]
    public class OfficeRankMember
    {
        /// <summary>
        /// Thứ tự thành viên
        /// </summary>
        [ProtoMember(1)]
        public int ID { get; set; }

        /// <summary>
        /// Tên thành viên
        /// </summary>
        [ProtoMember(2)]
        public string RoleName { get; set; }

        /// <summary>
        /// Chức quan
        /// </summary>
        [ProtoMember(3)]
        public string RankTile { get; set; }

        /// <summary>
        /// Quan hàm
        /// </summary>
        [ProtoMember(4)]
        public string OfficeRankTitle { get; set; }

        /// <summary>
        /// ID thành viên
        /// </summary>
        [ProtoMember(5)]
        public int RoleID { get; set; }

        /// <summary>
        /// Chức quan dạng số
        /// </summary>
        [ProtoMember(6)]
        public int RankNum { get; set; }
    }
}
