using ProtoBuf;

namespace Server.Data
{
	/// <summary>
	/// Thông tin gia tộc
	/// </summary>
	[ProtoContract]
    public class FamilyObj
    {
        /// <summary>
        /// ID tộc
        /// </summary>
        [ProtoMember(1)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên tộc
        /// </summary>
        [ProtoMember(2)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Tổng số thành viên
        /// </summary>
        [ProtoMember(3)]
        public int MemberCount { get; set; }

        /// <summary>
        /// Tổng uy danh
        /// </summary>
        [ProtoMember(4)]
        public int TotalpPrestige { get; set; }
    }
}
