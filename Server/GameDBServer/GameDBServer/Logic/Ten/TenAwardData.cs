using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using Server.Data;

namespace GameDBServer.Logic.Ten
{
    /// <summary>
    /// 礼包数据
    /// </summary>
    [ProtoContract]
    public class TenAwardData
    {
        /// <summary>
        /// 礼包id
        /// </summary>
        [ProtoMember(1)]
        public int AwardID = 0;

        /// <summary>
        /// 礼包name
        /// </summary>
        [ProtoMember(2)]
        public string AwardName = "";

        /// <summary>
        /// 礼包标识
        /// </summary>
        [ProtoMember(3)]
        public string DbKey = "";

        /// <summary>
        /// 每天最多数量
        /// </summary>
        [ProtoMember(4)]
        public int DayMaxNum = 0;

        /// <summary>
        /// 只能领取数量
        /// </summary>
        [ProtoMember(5)]
        public int OnlyNum = 0;

        /// <summary>
        /// 奖励物品列表
        /// </summary>
        [ProtoMember(6)]
        public List<GoodsData> AwardGoods = null;

        /// <summary>
        /// 邮件标题
        /// </summary>
        [ProtoMember(7)]
        public string MailTitle = "";

        /// <summary>
        /// 邮件内容
        /// </summary>
        [ProtoMember(8)]
        public string MailContent = "";

        /// <summary>
        /// 状态
        /// </summary>
        [ProtoMember(9)]
        public int State = 0;

        /// <summary>
        /// 数据库id
        /// </summary>
        [ProtoMember(10)]
        public int DbID = 0;

        /// <summary>
        /// 角色id
        /// </summary>
        [ProtoMember(11)]
        public int RoleID = 0;

        /// <summary>
        /// 邮件发送人
        /// </summary>
        [ProtoMember(12)]
        public string MailUser = "";

        [ProtoMember(13)]
        public DateTime BeginTime = DateTime.MinValue;

        [ProtoMember(14)]
        public DateTime EndTime = DateTime.MinValue;

        [ProtoMember(15)]
        public int RoleLevel = 0;

        [ProtoMember(16)]
        public string UserID = "";

    }
}

