using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Đối tượng vật phẩm rơi ở Map
    /// </summary>
    [ProtoContract]
    public class NewGoodsPackData
    {
        /// <summary>
        /// ID chủ nhân
        /// </summary>
        [ProtoMember(1)]
        public int OwnerRoleID { get; set; }

        /// <summary>
        /// ID tự động
        /// </summary>
        [ProtoMember(2)]
        public int AutoID { get; set; }

        /// <summary>
        /// Vị trí X
        /// </summary>
        [ProtoMember(3)]
        public int PosX { get; set; }

        /// <summary>
        /// Vị trí Y
        /// </summary>
        [ProtoMember(4)]
        public int PosY { get; set; }

        /// <summary>
        /// ID vật phẩm
        /// </summary>
        [ProtoMember(5)]
        public int GoodsID { get; set; }

        /// <summary>
        /// Thời điểm tạo (giờ GS)
        /// </summary>
        [ProtoMember(6)]
        public long ProductTicks { get; set; }

        /// <summary>
        /// ID nhóm
        /// </summary>
        [ProtoMember(7)]
        public int TeamID { get; set; }

        /// <summary>
        /// Mã màu HTML
        /// </summary>
        [ProtoMember(8)]
        public string HTMLColor { get; set; }

        /// <summary>
        /// Tổng số
        /// </summary>
        [ProtoMember(9)]
        public int GoodCount { get; set; }

        /// <summary>
        /// Tổng số sao
        /// </summary>
        [ProtoMember(10)]
        public int Star { get; set; }

        /// <summary>
        /// Tổng số sao
        /// </summary>
        [ProtoMember(11)]
        public int EnhanceLevel { get; set; }
    }
}
