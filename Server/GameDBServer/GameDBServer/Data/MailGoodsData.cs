using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 邮件项数据
    /// </summary>
    [ProtoContract]
    public class MailGoodsData
    {
        /// <summary>
        /// 数据库流水ID
        /// </summary>
        [ProtoMember(1)]
        public int Id;

        /// <summary>
        /// MailID
        /// </summary>
        [ProtoMember(2)]
        public int MailID = 0;

        /// <summary>
        /// 物品ID
        /// </summary>
        [ProtoMember(3)]
        public int GoodsID = 0;

        /// <summary>
        /// 锻造级别
        /// </summary>
        [ProtoMember(4)]
        public int Forge_level = 0;

        /// <summary>
        /// 物品的品质(某些装备会分品质，不同的品质属性不同，用户改变属性后要记录下来)
        /// </summary>
        [ProtoMember(5)]
        public int Quality = 0;

        /// <summary>
        /// 根据品质随机抽取的扩展属性的索引列表
        /// </summary>
        [ProtoMember(6)]
        public string Props = "";

        /// <summary>
        /// 物品数量
        /// </summary>
        [ProtoMember(7)]
        public int GCount = 0;

        /// <summary>
        /// 是否绑定的物品(绑定的物品不可交易, 不可摆摊)
        /// </summary>
        [ProtoMember(8)]
        public int Binding = 0;

        /// <summary>
        /// 原始孔洞的数量
        /// </summary>
        [ProtoMember(9)]
        public int OrigHoleNum = 0;

        /// <summary>
        /// 人民币打孔的数量
        /// </summary>
        [ProtoMember(10)]
        public int RMBHoleNum = 0;

        /// <summary>
        /// 根据品质随机抽取的扩展属性的索引列表
        /// </summary>
        [ProtoMember(11)]
        public string Jewellist = ""; 

        /// <summary>
        /// 精锻属性
        /// </summary>
        [ProtoMember(12)]
        public int AddPropIndex = 0;

        /// <summary>
        /// 天生属性
        /// </summary>
        [ProtoMember(13)]
        public int BornIndex = 0;

        /// <summary>
        /// 幸运值
        /// </summary>
        [ProtoMember(14)]
        public int Lucky = 0;

        /// <summary>
        /// 耐久值
        /// </summary>
        [ProtoMember(15)]
        public int Strong = 0;

        // 新增物品属性 [12/13/2013 LiaoWei]
        /// <summary>
        /// 卓越信息 -- 一个32位int 每位代表一个卓越属性
        /// </summary>
        [ProtoMember(16)]
        public int ExcellenceInfo = 0;

        // 新增物品属性 [12/18/2013 LiaoWei]
        /// <summary>
        /// 追加等级
        /// </summary>
        [ProtoMember(17)]
        public int AppendPropLev = 0;

        // 新增物品属性 [2/17/2014 LiaoWei]
        /// <summary>
        /// 追加等级
        /// </summary>
        [ProtoMember(18)]
        public int EquipChangeLifeLev = 0;
    }
}
