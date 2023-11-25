﻿using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Thuộc tính nhân vật (dùng trong khung thuộc tính)
    /// </summary>
    [ProtoContract]
    public class RoleAttributes
    {
        /// <summary>
        /// Tiềm năng hiện tại
        /// </summary>
        [ProtoMember(1)]
        public int RemainPoint { get; set; }

        /// <summary>
        /// Lực tay
        /// </summary>
        [ProtoMember(2)]
        public int Damage { get; set; }

        /// <summary>
        /// Chí mạng
        /// </summary>
        [ProtoMember(3)]
        public int Crit { get; set; }

        /// <summary>
        /// Tốc chạy
        /// </summary>
        [ProtoMember(4)]
        public int MoveSpeed { get; set; }

        /// <summary>
        /// Tốc đánh
        /// </summary>
        [ProtoMember(5)]
        public int AtkSpeed { get; set; }

        /// <summary>
        /// Chính xác
        /// </summary>
        [ProtoMember(6)]
        public int Hit { get; set; }

        /// <summary>
        /// Né tránh
        /// </summary>
        [ProtoMember(7)]
        public int Dodge { get; set; }

        /// <summary>
        /// Vật phòng
        /// </summary>
        [ProtoMember(8)]
        public int Def { get; set; }

        /// <summary>
        /// Độc phòng
        /// </summary>
        [ProtoMember(9)]
        public int PoisonRes { get; set; }

        /// <summary>
        /// Lôi phòng
        /// </summary>
        [ProtoMember(10)]
        public int LightningRes { get; set; }

        /// <summary>
        /// Lôi phòng
        /// </summary>
        [ProtoMember(11)]
        public int IceRes { get; set; }

        /// <summary>
        /// Lôi phòng
        /// </summary>
        [ProtoMember(12)]
        public int FireRes { get; set; }

        /// <summary>
        /// Sức
        /// </summary>
        [ProtoMember(13)]
        public int Str { get; set; }

        /// <summary>
        /// Thân
        /// </summary>
        [ProtoMember(14)]
        public int Dex { get; set; }

        /// <summary>
        /// Ngoại
        /// </summary>
        [ProtoMember(15)]
        public int Sta { get; set; }

        /// <summary>
        /// Nội
        /// </summary>
        [ProtoMember(16)]
        public int Int { get; set; }

        /// <summary>
        /// Danh sách các thuộc tính khác
        /// </summary>
        [ProtoMember(17)]
        public Dictionary<int, int> OtherProperties { get; set; }
    }
}
