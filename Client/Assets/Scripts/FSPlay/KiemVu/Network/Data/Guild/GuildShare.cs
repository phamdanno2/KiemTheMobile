using ProtoBuf;

namespace Server.Data
{
	/// <summary>
	/// Cổ tức của các thành viên
	/// </summary>
	[ProtoContract]
    public class GuildShare
    {
        /// <summary>
        /// ID
        /// </summary>
        [ProtoMember(1)]
        public int ID { get; set; }

        /// <summary>
        /// Id của role
        /// </summary>
        [ProtoMember(2)]
        public int RoleID { get; set; }

        /// <summary>
        /// ID gia tộc
        /// </summary>
        [ProtoMember(3)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        [ProtoMember(4)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Cấp độ của thành viên
        /// </summary>
        [ProtoMember(5)]
        public int RoleLevel { get; set; }

        /// <summary>
        /// FactionID
        /// </summary>
        [ProtoMember(6)]
        public int FactionID { get; set; }

        /// <summary>
        /// Tỉ lệ %
        /// </summary>
        [ProtoMember(7)]
        public double Share { get; set; }

        /// <summary>
        /// Tên thành viên
        /// </summary>
        [ProtoMember(8)]
        public string RoleName { get; set; }

        /// <summary>
        /// Hạng của thành viên tại BANG
        /// </summary>
        [ProtoMember(9)]
        public int Rank { get; set; }
    }
}
