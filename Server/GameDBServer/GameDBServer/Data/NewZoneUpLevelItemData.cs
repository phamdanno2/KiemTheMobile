using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
namespace Server.Data
{
    [ProtoContract]
    class NewZoneUpLevelItemData
    {
      
            /// <summary>
            /// 剩余数量
            /// </summary>
            [ProtoMember(1)]
            public int LeftNum;

            /// <summary>
            /// 是否领取
            /// </summary>
            [ProtoMember(2)]
            public bool GetAward;
      
    }
}
