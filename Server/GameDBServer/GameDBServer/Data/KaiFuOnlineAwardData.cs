using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 开服在线奖励项数据
    /// </summary>
    [ProtoContract]
    public class KaiFuOnlineAwardData
    {
        /// <summary>
        /// 中奖的角色ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID = 0;

        /// <summary>
        /// 中奖的角色区编号
        /// </summary>
        [ProtoMember(2)]
        public int ZoneID = 0;

        /// <summary>
        /// 中奖的角色的名称
        /// </summary>
        [ProtoMember(3)]
        public string RoleName = "";

        /// <summary>
        /// 中奖的日期ID
        /// </summary>
        [ProtoMember(4)]
        public int DayID = 0;

        /// <summary>
        /// 中奖时的总的角色数量
        /// </summary>
        [ProtoMember(5)]
        public int TotalRoleNum = 0;
    }
}
