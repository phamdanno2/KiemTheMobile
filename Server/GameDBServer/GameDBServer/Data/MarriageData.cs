using System.Collections.Generic;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 结婚数据
    /// </summary>
    [ProtoContract]
    public class MarriageData
    {
        /// <summary>
        /// 配偶的ID
        /// </summary>
        [ProtoMember(1)]
        public int nSpouseID = -1;

        /// <summary>
        /// 类型 -1 = 未结婚 1 = 丈夫 2 = 妻子
        /// </summary>
        [ProtoMember(2)]
        public sbyte byMarrytype = -1;

        /// <summary>
        /// 婚戒ID
        /// </summary>
        [ProtoMember(3)]
        public int nRingID = -1;

        /// <summary>
        /// 亲密度
        /// </summary>
        [ProtoMember(4)]
        public int nGoodwillexp = 0;

        /// <summary>
        /// 亲密星级
        /// </summary>
        [ProtoMember(5)]
        public sbyte byGoodwillstar = 0;

        /// <summary>
        /// 亲密阶数
        /// </summary>
        [ProtoMember(6)]
        public sbyte byGoodwilllevel = 0;

        /// <summary>
        /// 已收玫瑰数量
        /// </summary>
        [ProtoMember(7)]
        public int nGivenrose = 0;

        /// <summary>
        /// 爱情宣言
        /// </summary>
        [ProtoMember(8)]
        public string strLovemessage = "";

        /// <summary>
        /// 自动拒绝求婚
        /// </summary>
        [ProtoMember(9)]
        public sbyte byAutoReject = 0;

        /// <summary>
        /// 婚戒更新时间
        /// </summary>
        [ProtoMember(10)]
        public string ChangTime = "1900-01-01 12:00:00";
    }

    /// <summary>
    /// 发送配偶的结婚数据
    /// </summary>
    [ProtoContract]
    public class MarriageData_EX
    {
        /// <summary>
        /// 结婚数据
        /// </summary>
        [ProtoMember(1)]
        public MarriageData myMarriageData = new MarriageData();

        /// <summary>
        /// 玩家名称
        /// </summary>
        [ProtoMember(2)]
        public string roleName = "";

        /// <summary>
        /// 角色职业
        /// </summary>
        [ProtoMember(3)]
        public int Occupation = 0;

        /// <summary>
        /// 角色ID
        /// </summary>
        [ProtoMember(4)]
        public int nRoleID = 0;
    }
}
