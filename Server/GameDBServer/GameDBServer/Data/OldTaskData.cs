using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 旧的任务记录
    /// </summary>
    [ProtoContract]    
    public class OldTaskData
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [ProtoMember(1)]        
        public int TaskID;

        /// <summary>
        /// 做过的数量
        /// </summary>
        [ProtoMember(2)]        
        public int DoCount;

        [ProtoMember(3)]
        public int TaskClass;
    }
}
