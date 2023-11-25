using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Dữ liệu cửa hàng của người chơi bày bán
    /// </summary>
    [ProtoContract]
    public class StallData
    {
        /// <summary>
        /// ID cửa hàng
        /// </summary>
        [ProtoMember(1)]
        public int StallID;

        /// <summary>
        /// ID người chơi bán hàng
        /// </summary>
        [ProtoMember(2)]
        public int RoleID;

        /// <summary>
        /// Tên cửa hàng
        /// </summary>
        [ProtoMember(3)]
        public string StallName;

        /// <summary>
        /// Quảng cáo cửa hàng
        /// </summary>
        [ProtoMember(4)]
        public string StallMessage;

        /// <summary>
        /// Danh sách vật phẩm
        /// </summary>
        [ProtoMember(5)]
        public List<GoodsData> GoodsList;

        /// <summary>
        /// Danh sách giá vật phẩm theo GoodsDbID
        /// </summary>
        [ProtoMember(6)]
        public Dictionary<int, int> GoodsPriceDict;

        /// <summary>
        /// Thời điểm thêm vào
        /// </summary>
        [ProtoMember(7)]
        public long AddDateTime;

        /// <summary>
        /// Bắt đầu mở bán
        /// </summary>
        [ProtoMember(8)]
        public int Start;
    }
}