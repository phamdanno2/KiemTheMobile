using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using Server.Data;

namespace GameDBServer.Data
{
    /// <summary>
    /// GiftCode礼包数据
    /// </summary>
    [ProtoContract]
    public class GiftCodeAwardData
    {
        /// <summary>
        /// Dbid
        /// </summary>
        [ProtoMember(1)]
        public int Dbid = 0;

        /// <summary>
        /// userid
        /// </summary>
        [ProtoMember(2)]
        public string UserId = "";

        /// <summary>
        /// RoleID
        /// </summary>
        [ProtoMember(3)]
        public int RoleID = 0;

        /// <summary>
        /// 礼品id
        /// </summary>
        [ProtoMember(4)]
        public string GiftId = "";

        /// <summary>
        /// 礼品码
        /// </summary>
        [ProtoMember(5)]
        public string CodeNo = "";
    }
}
