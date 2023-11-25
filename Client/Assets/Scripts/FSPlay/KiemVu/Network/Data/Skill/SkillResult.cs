using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Kết quả của kỹ năng gửi về Client
    /// </summary>
    [ProtoContract]
    public class SkillResult
    {
        /// <summary>
        /// ID đối tượng xuất chiêu
        /// </summary>
        [ProtoMember(1)]
        public int CasterID { get; set; }

        /// <summary>
        /// ID đối tượng ảnh hưởng
        /// </summary>
        [ProtoMember(2)]
        public int TargetID { get; set; }

        /// <summary>
        /// Loại kỹ năng
        /// </summary>
        [ProtoMember(3)]
        public int Type { get; set; }

        /// <summary>
        /// Sát thương nhận được
        /// </summary>
        [ProtoMember(4)]
        public int Damage { get; set; }

        /// <summary>
        /// Lượng máu hiện tại
        /// </summary>
        [ProtoMember(5)]
        public int TargetCurrentHP { get; set; }
    }
}
