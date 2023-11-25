using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 角色日常数据
    /// </summary>
    [ProtoContract]
    public class RoleDailyData
    {
        /// <summary>
        /// 经验日ID
        /// </summary>
        [ProtoMember(1)]
        public int ExpDayID = 0;

        /// <summary>
        /// 今日经验
        /// </summary>
        [ProtoMember(2)]
        public int TodayExp = 0;

        /// <summary>
        /// 灵力日ID
        /// </summary>
        [ProtoMember(3)]
        public int LingLiDayID = 0;

        /// <summary>
        /// 今日灵力
        /// </summary>
        [ProtoMember(4)]
        public int TodayLingLi = 0;

        /// <summary>
        /// 杀BOSS日ID
        /// </summary>
        [ProtoMember(5)]
        public int KillBossDayID = 0;

        /// <summary>
        /// 今日杀BOSS数量
        /// </summary>
        [ProtoMember(6)]
        public int TodayKillBoss = 0;

        /// <summary>
        /// 副本通关日ID
        /// </summary>
        [ProtoMember(7)]
        public int FuBenDayID = 0;

        /// <summary>
        /// 今日副本通关数量
        /// </summary>
        [ProtoMember(8)]
        public int TodayFuBenNum = 0;

        /// <summary>
        /// 五行奇阵日ID
        /// </summary>
        [ProtoMember(9)]
        public int WuXingDayID = 0;

        /// <summary>
        /// 五行奇阵领取奖励数量
        /// </summary>
        [ProtoMember(10)]
        public int WuXingNum = 0;
    }
}
