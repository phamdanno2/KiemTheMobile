using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 角色跨服数据
    /// </summary>
    [ProtoContract]
    public class RoleKuaFuDayLogData
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID = 0;

        /// <summary>
        /// 日期
        /// </summary>
        [ProtoMember(2)]
        public string Day = "2000-1-1";

        /// <summary>
        /// 区号
        /// </summary>
        [ProtoMember(3)]
        public int ZoneId = 0;

        /// <summary>
        /// 排队次数
        /// </summary>
        [ProtoMember(4)]
        public int SignupCount = 0;

        /// <summary>
        /// 进入游戏次数
        /// </summary>
        [ProtoMember(5)]
        public int StartGameCount = 0;

        /// <summary>
        /// 成功计数
        /// </summary>
        [ProtoMember(6)]
        public int SuccessCount = 0;

        /// <summary>
        /// 失败计数
        /// </summary>
        [ProtoMember(7)]
        public int FaildCount = 0;

        /// <summary>
        /// 跨服活动类型
        /// </summary>
        [ProtoMember(8)]
        public int GameType = 0;
    }
}
