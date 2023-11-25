using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 排行榜项数据
    /// </summary>
    [ProtoContract]
    public class PaiHangItemData
    {
        /// <summary>
        /// 角色的ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID;

        /// <summary>
        /// 角色的名称
        /// </summary>
        [ProtoMember(2)]
        public string RoleName;

        /// <summary>
        /// 数据1
        /// </summary>
        [ProtoMember(3)]
        public int Val1;

        // 新增数据 [12/10/2013 LiaoWei]
        /// <summary>
        /// 数据2
        /// </summary>
        [ProtoMember(4)]
        public int Val2;

        // 新增数据 [12/10/2013 LiaoWei]
        /// <summary>
        /// 数据3
        /// </summary>
        [ProtoMember(5)]
        public int Val3;

        // 新增数据 [12/10/2013 LiaoWei]
        /// <summary>
        /// 数据3
        /// </summary>
        [ProtoMember(6)]
        public string uid;
    }
}
