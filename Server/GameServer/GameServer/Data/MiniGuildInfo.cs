using ProtoBuf;

namespace Server.Data
{

    [ProtoContract]
    public class MiniGuildInfo
    {
        [ProtoMember(1)]
        public int GuildId { get; set; }
        [ProtoMember(2)]
        public string GuildName { get; set; }
        [ProtoMember(3)]
        public int MoneyStore { get; set; }
        [ProtoMember(4)]
        public int TotalPrestige { get; set; }

        [ProtoMember(5)]
        public int TotalMember { get; set; }
    }
}
