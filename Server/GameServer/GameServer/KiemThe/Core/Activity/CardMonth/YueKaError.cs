using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Activity.CardMonth
{
   
    public enum YueKaError
    {
        YK_Success,
        YK_CannotAward_HasNotYueKa,     //不是月卡用户, 不可领取
        YK_CannotAward_DayHasPassed,    //领奖日期已经过去, 不可领取
        YK_CannotAward_AlreadyAward,    //今日已领取, 不可领取
        YK_CannotAward_TimeNotReach,    //领奖日期还未到达, 不可领取
        YK_CannotAward_BagNotEnough,    //背包位置不足, 不可领取
        YK_CannotAward_ParamInvalid,    //传来的参数错误
        YK_CannotAward_ConfigError,     //服务器配置文件错误
        YK_CannotAward_DBError,         //服务器操作数据库错误
    }
}