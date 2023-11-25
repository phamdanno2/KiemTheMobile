using ProtoBuf;

namespace Server.Data
{
	/// <summary>
	/// Danh sách lãnh thổ của bang
	/// </summary>
	[ProtoContract]
    public class Territory
    {
        /// <summary>
        /// ID
        /// </summary>
        [ProtoMember(1)]
        public int ID { get; set; }

        /// <summary>
        /// MapID
        /// </summary>
        [ProtoMember(2)]
        public int MapID { get; set; }

        /// <summary>
        /// MapNAME
        /// </summary>
        [ProtoMember(3)]
        public string MapName { get; set; }

        /// <summary>
        /// ID BANG
        /// </summary>
        [ProtoMember(4)]
        public int GuildID { get; set; }

        /// <summary>
        /// Số sao của lãnh thổ
        /// </summary>
        [ProtoMember(5)]
        public int Star { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        [ProtoMember(6)]
        public int Tax { get; set; }

        /// <summary>
        /// ID khu vực
        /// </summary>
        [ProtoMember(7)]
        public int ZoneID { get; set; }

        /// <summary>
        /// Có phải thành phố chính không
        /// </summary>
        [ProtoMember(8)]
        public int IsMainCity { get; set; }
    }
}
