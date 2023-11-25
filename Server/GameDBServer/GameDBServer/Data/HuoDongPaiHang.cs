using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 活动排行数据
    /// </summary>
    [ProtoContract]
    public class HuoDongPaiHangData
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID;

        /// <summary>
        /// 角色名称
        /// </summary>
        [ProtoMember(2)]
        public string RoleName;

        /// <summary>
        /// 角色区号
        /// </summary>
        [ProtoMember(3)]
        public int ZoneID;

        /// <summary>
        /// 排行类型
        /// </summary>
        [ProtoMember(4)]
        public int Type;

        /// <summary>
        /// 排行
        /// </summary>
        [ProtoMember(5)]
        public int PaiHang;

        /// <summary>
        /// 排行时间
        /// </summary>
        [ProtoMember(6)]
        public string PaiHangTime;

        /// <summary>
        /// 排行值，用于计算排行的值，比如经脉值，等级，坐骑实力等
        /// </summary>
        [ProtoMember(7)]
        public int PaiHangValue;
    }
}
