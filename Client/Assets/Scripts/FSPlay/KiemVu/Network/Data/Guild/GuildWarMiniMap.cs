using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin bản đồ tranh đoạt
    /// </summary>
    [ProtoContract]
    public class GuildWarMiniMap
    {
        /// <summary>
        /// Bản đồ thuộc về Bang nào
        /// </summary>
        [ProtoMember(1)]
        public int GuildID { get; set; }

        /// <summary>
        /// Tên bang hội
        /// </summary>
        [ProtoMember(2)]
        public string GuildName { get; set; }

        /// <summary>
        /// ID bản đồ
        /// </summary>
        [ProtoMember(3)]
        public int MapID { get; set; }

        /// <summary>
        /// Mã màu của bang này
        /// </summary>
        [ProtoMember(4)]
        public string HexColor { get; set; }

        /// <summary>
        /// 0: Lảnh Thổ | Màu của HEXColor<br/>
        /// 1: Thành Chính | HÌnh cái NGôi Sao<br/>
        /// 2: Thân THủ Thôn | hình cái ngôn nhà<br/>
        /// 3: Nhấy nháy tấn công hình cái kiếm chéo vào nhau<br/>
        /// 4: Nhấp nháy lân cận 1 cái gạch ngang<br/>
        /// 5: Lân cận 1 cái gạch ngang nhưng không nhấp nháy
        /// </summary>
        [ProtoMember(5)]
        public int MapType { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        [ProtoMember(6)]
        public int Tax { get; set; }
    }
}
