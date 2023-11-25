using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin dữ liệu tiền tệ và vật phẩm giao dịch
    /// </summary>
    [ProtoContract]
    public class ExchangeData
    {
        /// <summary>
        /// ID phiên giao dịch
        /// </summary>
        [ProtoMember(1)]
        public int ExchangeID { get; set; }

        /// <summary>
        /// ID đối tượng mở giao dịch
        /// </summary>
        [ProtoMember(2)]
        public int RequestRoleID { get; set; }

        /// <summary>
        /// ID đối tượng giao dịch cùng
        /// </summary>
        [ProtoMember(3)]
        public int AgreeRoleID { get; set; }

        /// <summary>
        /// Danh sách vật phẩm giao dịch của 2 bên
        /// </summary>
        [ProtoMember(4)]
        public Dictionary<int, List<GoodsData>> GoodsDict { get; set; }

        /// <summary>
        /// Danh sách tiền đặt vào của 2 bên
        /// </summary>
        [ProtoMember(5)]
        public Dictionary<int, int> MoneyDict { get; set; }

        /// <summary>
        /// Trạng thái khóa của 2 bên
        /// </summary>
        [ProtoMember(6)]
        public Dictionary<int, int> LockDict { get; set; }

        /// <summary>
        /// Trạng thái hoàn tất của 2 bên
        /// </summary>
        [ProtoMember(7)]
        public Dictionary<int, int> DoneDict { get; set; }

        /// <summary>
        /// Thời gian thêm vào (Mili giây)
        /// </summary>
        [ProtoMember(8)]
        public long AddDateTime { get; set; }

        /// <summary>
        /// Trạng thái hoàn thành
        /// </summary>
        [ProtoMember(9)]
        public int Done { get; set; }

        /// <summary>
        /// Danh sách đồng đặt vào của 2 bên
        /// </summary>
        [ProtoMember(10)]
        public Dictionary<int, int> YuanBaoDict { get; set; }
    }
}
