using ProtoBuf;

namespace Server.Data
{
	/// <summary>
	/// Sửa tôn chỉ bang hội
	/// </summary>
	[ProtoContract]
	public class GuildChangeSlogan
	{
        /// <summary>
        /// ID bang hội
        /// </summary>
        [ProtoMember(1)]
        public int GuildID { get; set; }

        /// <summary>
        /// Tôn chỉ bang hội
        /// </summary>
        [ProtoMember(2)]
        public string Slogan { get; set; }
    }
}
