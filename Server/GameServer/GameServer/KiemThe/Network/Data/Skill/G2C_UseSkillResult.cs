using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Gói tin gửi từ Server về Client thông báo kết quả thao tác đối tượng dùng chiêu
    /// </summary>
    [ProtoContract]
    public class G2C_UseSkillResult
    {
        /// <summary>
        /// Kết quả
        /// </summary>
        [ProtoMember(1)]
        public int Result { get; set; }

        /// <summary>
        /// Vị trí X
        /// </summary>
        [ProtoMember(2)]
        public int PosX { get; set; }

        /// <summary>
        /// Vị trí Y
        /// </summary>
        [ProtoMember(3)]
        public int PosY { get; set; }

        /// <summary>
        /// Vị trí ra chiêu
        /// </summary>
        public UnityEngine.Vector2 Position
        {
            get
            {
                return new UnityEngine.Vector2(this.PosX, this.PosY);
            }
        }
    }
}
