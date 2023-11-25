using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// MiniData bang hội
    /// </summary>
    [ProtoContract]
    public class BangHuiMiniData
    {
        /// <summary>
        /// ID bang hội
        /// </summary>
        [ProtoMember(1)]
        public int BHID = 0;

        /// <summary>
        /// Tên bang hội
        /// </summary>
        [ProtoMember(2)]
        public string BHName = "";

        /// <summary>
        /// ID khu vực
        /// </summary>
        [ProtoMember(3)]
        public int ZoneID = 0;
    }
}