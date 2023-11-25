using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 活动送礼相关数据
    /// </summary>
    [ProtoContract]
    public class HuodongData
    {
        /// <summary>
        /// 登录周ID
        /// </summary>
        [ProtoMember(1)]
        public string LastWeekID = "";

        /// <summary>
        /// 登录日ID
        /// </summary>
        [ProtoMember(2)]
        public string LastDayID = "";

        /// <summary>
        /// 周连续登录次数
        /// </summary>
        [ProtoMember(3)]
        public int LoginNum = 0;

        /// <summary>
        /// 见面有礼领取步骤
        /// </summary>
        [ProtoMember(4)]
        public int NewStep = 0;

        /// <summary>
        /// 领取上一个见面有礼步骤的时间
        /// </summary>
        [ProtoMember(5)]
        public long StepTime = 0;

        /// <summary>
        /// 上个月的在线时长
        /// </summary>
        [ProtoMember(6)]
        public int LastMTime = 0;

        /// <summary>
        /// 本月的标记ID
        /// </summary>
        [ProtoMember(7)]
        public string CurMID = "";

        /// <summary>
        /// 本月的在线时长
        /// </summary>
        [ProtoMember(8)]
        public int CurMTime = 0;

        /// <summary>
        /// 已经领取的送礼活动ID
        /// </summary>
        [ProtoMember(9)]
        public int SongLiID = 0;

        /// <summary>
        /// 登录有礼的领取状态
        /// </summary>
        [ProtoMember(10)]
        public int LoginGiftState = 0;

        /// <summary>
        /// 在线有礼的领取状态
        /// </summary>
        [ProtoMember(11)]
        public int OnlineGiftState = 0;

        /// <summary>
        /// 限时登录活动ID
        /// </summary>
        [ProtoMember(12)]
        public int LastLimitTimeHuoDongID = 0;

        /// <summary>
        /// 限时登录日ID
        /// </summary>
        [ProtoMember(13)]
        public int LastLimitTimeDayID = 0;

        /// <summary>
        /// 限时登录日累计登录次数
        /// </summary>
        [ProtoMember(14)]
        public int LimitTimeLoginNum = 0;

        /// <summary>
        /// 限时登录日累计领取状态
        /// </summary>
        [ProtoMember(15)]
        public int LimitTimeGiftState = 0;

        // MU新增加 每日在线奖励相关 begin[1/12/2014 LiaoWei]
        /// <summary>
        /// 每日在线奖励的ID
        /// </summary>
        [ProtoMember(16)]
        public int EveryDayOnLineAwardStep = 0;

        /// <summary>
        /// 领取上一个每日在线奖励的日期
        /// </summary>
        [ProtoMember(17)]
        public int GetEveryDayOnLineAwardDayID = 0;

        /// <summary>
        /// 连续登陆奖励领取到第几步了
        /// </summary>
        [ProtoMember(18)]
        public int SeriesLoginGetAwardStep = 0;

        /// <summary>
        /// 连续登陆领取奖励的日期
        /// </summary>
        [ProtoMember(19)]
        public int SeriesLoginAwardDayID = 0;

        /// <summary>
        /// 连续登陆领取奖励的列表
        /// </summary>
        [ProtoMember(20)]
        public string SeriesLoginAwardGoodsID = "";

        /// <summary>
        /// 每日在线领取奖励的列表
        /// </summary>
        [ProtoMember(21)]
        public string EveryDayOnLineAwardGoodsID = "";

        // MU新增加 每日在线奖励相关 end[1/12/2014 LiaoWei]
    }
}
