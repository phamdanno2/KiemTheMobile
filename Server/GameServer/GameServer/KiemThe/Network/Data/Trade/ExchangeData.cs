using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Cấu trúc của 1 session giao dịch
    /// </summary>
    [ProtoContract]
    public class ExchangeData
    {
        /// <summary>
        /// ID phiên giao dịch
        /// </summary>
        [ProtoMember(1)]
        public int ExchangeID;

        /// <summary>
        /// Người yêu cầu giao dịch
        /// </summary>
        [ProtoMember(2)]
        public int RequestRoleID;

        /// <summary>
        /// Người đồng ý giao dịch
        /// </summary>
        [ProtoMember(3)]
        public int AgreeRoleID;

        /// <summary>
        /// Danh sách vật phẩm giao dịch
        /// </summary>
        [ProtoMember(4)]
        public Dictionary<int, List<GoodsData>> GoodsDict;

        /// <summary>
        /// Tiền giao dịch
        /// </summary>
        [ProtoMember(5)]
        public Dictionary<int, int> MoneyDict;

        /// <summary>
        /// Danh sách đã lock
        /// </summary>
        [ProtoMember(6)]
        public Dictionary<int, int> LockDict;

        /// <summary>
        /// Danh sách sách hoàn tất giao dịch
        /// </summary>
        [ProtoMember(7)]
        public Dictionary<int, int> DoneDict;

        /// <summary>
        /// Thời gian add vào
        /// </summary>
        [ProtoMember(8)]
        public long AddDateTime;

        /// <summary>
        /// Có hoàn thành không
        /// </summary>
        [ProtoMember(9)]
        public int Done;

        /// <summary>
        /// KNB
        /// </summary>
        [ProtoMember(10)]
        public Dictionary<int, int> YuanBaoDict;
    }
}