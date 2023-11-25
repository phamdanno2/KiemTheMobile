using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 专享活动数据
    /// </summary>
    [ProtoContract]
    public class SpecActInfoDB
    {
        // 组ID
        [ProtoMember(1)]
        public int GroupID = 0;

        // 活动ID
        [ProtoMember(2)]
        public int ActID = 0;

        // 购买数量
        [ProtoMember(3)]
        public int PurNum = 0;

        // 计数信息
        [ProtoMember(4)]
        public int CountNum = 0;

        // 活动状态 激活 1 关闭 0
        [ProtoMember(5)]
        public short Active = 0;
    }
}