using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Cầu hình Thực thể Buff
    /// </summary>
    [ProtoContract]
    public class BufferData
    {
        /// <summary>
        /// BuffID
        /// </summary>
        [ProtoMember(1)]
        public int BufferID = 0;

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [ProtoMember(2)]
        public long StartTime = 0;

        /// <summary>
        /// Thời gian tồn tại
        /// </summary>
        [ProtoMember(3)]
        public long BufferSecs = 0;

        /// <summary>
        /// Cấp độ Buff
        /// </summary>
        [ProtoMember(4)]
        public long BufferVal = 0;

        /// <summary>
        /// Loại buff
        /// </summary>
        [ProtoMember(5)]
        public int BufferType = 0;

        /// <summary>
        /// ProDict tùy chọn (có từ vật phẩm)
        /// </summary>
        [ProtoMember(6)]
        public string CustomProperty = "";
    }

    /// <summary>
    /// Cầu hình thực thể buff khác
    /// </summary>
    [ProtoContract]
    public class OtherBufferData
    {
        /// <summary>
        /// ID buff
        /// </summary>
        [ProtoMember(1)]
        public int BufferID = 0;

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [ProtoMember(2)]
        public long StartTime = 0;

        /// <summary>
        /// Thời gian tồn tại
        /// </summary>
        [ProtoMember(3)]
        public long BufferSecs = 0;

        /// <summary>
        /// Thời gian hiệu ứng
        /// </summary>
        [ProtoMember(4)]
        public long BufferVal = 0;

        /// <summary>
        ///Loại buff
        /// </summary>
        [ProtoMember(5)]
        public int BufferType = 0;

        /// <summary>
        /// Active lên RoleId nào
        /// </summary>
        [ProtoMember(6)]
        public int RoleID = 0;
    }

    /// <summary>
    /// Thực thể buff mini
    /// </summary>
    [ProtoContract]
    public class BufferDataMini
    {
        /// <summary>
        /// ID Buff
        /// </summary>
        [ProtoMember(1)]
        public int BufferID = 0;

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [ProtoMember(2)]
        public long StartTime = 0;

        /// <summary>
        /// Thời gian tồn tại
        /// </summary>
        [ProtoMember(3)]
        public long BufferSecs = 0;

        /// <summary>
        /// Thời gian kích hoạt
        /// </summary>
        [ProtoMember(4)]
        public long BufferVal = 0;

        /// <summary>
        ///  Kiểu buff
        /// </summary>
        [ProtoMember(5)]
        public int BufferType = 0;
    }
}