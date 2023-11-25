using ProtoBuf;

namespace Server.Data
{
    [ProtoContract]
    public class ChangeSlogenFamily
    {
        /// <summary>
        /// ID Của gia tộc
        /// </summary>
        [ProtoMember(1)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Chuỗi Slogen gia tộc
        /// </summary>
        [ProtoMember(2)]
        public string Slogen { get; set; }
    }
}