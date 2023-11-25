//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// 选择角色的数据定义 选角
    /// </summary>
    [ProtoContract]
    public class RoleData4Selector
    {
        /// <summary>
        /// 当前的角色ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID = 0;

        /// <summary>
        /// 当前的角色ID
        /// </summary>
        [ProtoMember(2)]
        public string RoleName = "";

        /// <summary>
        /// 当前角色的性别
        /// </summary>
        [ProtoMember(3)]
        public int RoleSex = 0;

        /// <summary>
        /// 角色职业
        /// </summary>
        [ProtoMember(4)]
        public int Occupation = 0;

        /// <summary>
        /// 角色级别
        /// </summary>
        [ProtoMember(5)]
        public int Level = 1;

        /// <summary>
        /// 角色所属的帮派
        /// </summary>
        [ProtoMember(6)]
        public int Faction = 0;

        /// <summary>
        /// 称号
        /// </summary>
        [ProtoMember(7)]
        public string OtherName = "";

        /// <summary>
        /// 物品数据
        /// </summary>
        [ProtoMember(8)]
        public List<GoodsData> GoodsDataList = null;

        /// <summary>
        /// 被崇拜次数
        /// </summary>
        [ProtoMember(9)]
        public int AdmiredCount = 0;

        /// <summary>
        /// 二态功能设置，参考ESettingBitFlag
        /// </summary>
        [ProtoMember(10)]
        public long SettingBitFlags;

        /// <summary>
        /// zone id
        /// </summary>
        [ProtoMember(11)]
        public int ZoneId;
    }
}