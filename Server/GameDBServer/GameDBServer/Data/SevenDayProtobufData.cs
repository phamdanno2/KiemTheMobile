using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 七日活动每个活动的每个子项的信息
    /// </summary>
    [ProtoContract]
    public class SevenDayItemData
    {
        /*
         * 领奖标识 
         * 七日登录：1 == 已领取, 否则为未领取
         * 七日目标：1 == 已领取, 否则为未领取
         * 七日抢购：不使用此字段
         * 七日充值：1 == 已领取, 否则为未领取
         */
        [ProtoMember(1)]
        public int AwardFlag;

        /*
         * 附加参数
         *  七日登录：1 == 当天登录了，否则表示未登录
         *  七日目标：表示该项已经达成的总和
         *  七日抢购：该项已购买个数
         *  七日充值：该项充值金额
         */
        [ProtoMember(2)]
        public int Params1;

        // 保留，暂时没有用到
        [ProtoMember(3)]
        public int Params2;
    }

    /// <summary>
    /// 客户端查询七日活动信息
    /// </summary>
    [ProtoContract]
    public class SevenDayActQueryData
    {
        // 查询的那一个活动  SevenDayActivityType.xml
        [ProtoMember(1)]
        public int ActivityType;

        // 活动的具体信息 key：每一个活动配置文件中的id字段
        [ProtoMember(2)]
        public Dictionary<int, SevenDayItemData> ItemDict;
    }

    /// <summary>
    /// 更新数据到DB
    /// </summary>
    [ProtoContract]
    public class SevenDayUpdateDbData
    {
        [ProtoMember(1)]
        public int RoleId;

        [ProtoMember(2)]
        public int ActivityType;

        [ProtoMember(3)]
        public int Id;

        [ProtoMember(4)]
        public SevenDayItemData Data;
    }
}
