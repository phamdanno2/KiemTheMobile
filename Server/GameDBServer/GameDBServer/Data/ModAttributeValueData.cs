using System;
using ProtoBuf;

namespace Server.Data
{
    public enum RoleAttribyteTypes
    {
        RongYao, //(天梯)荣耀
    }

    [ProtoContract]
    public class RoleAttributeValueData
    {
        /// <summary>
        /// 值类型
        /// </summary>
        [ProtoMember(1)]
        public int RoleAttribyteType;

        /// <summary>
        /// 变化值
        /// </summary>
        [ProtoMember(2)]
        public int AddVAlue;

        /// <summary>
        /// 目标值
        /// </summary>
        [ProtoMember(3)]
        public int Targetvalue;
    }
}