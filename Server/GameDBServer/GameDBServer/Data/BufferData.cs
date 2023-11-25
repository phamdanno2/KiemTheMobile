using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Đối tượng Buff
    /// </summary>
    [ProtoContract]
    public class BufferData
    {
        /// <summary>
        /// Buffer DbID
        /// </summary>
        [ProtoMember(1)]
        public int BufferID = 0;

        /// <summary>
        /// Thời gian kích hoạt Buff (Milis)
        /// </summary>
        [ProtoMember(2)]
        public long StartTime = 0;

        /// <summary>
        /// Thời gian tồn tại Buff (Milis)
        /// </summary>
        [ProtoMember(3)]
        public long BufferSecs = 0;

        /// <summary>
        /// Cấp độ Buff
        /// </summary>
        [ProtoMember(4)]
        public long BufferVal = 0;

        /// <summary>
        /// Loại Buff
        /// </summary>
        [ProtoMember(5)]
        public int BufferType = 0;

        /// <summary>
        /// Thuộc tính tùy chỉnh
        /// </summary>
        [ProtoMember(6)]
        public string CustomProperty = "";
    }
}
