﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 搜索角色的数据
    /// </summary>
    [ProtoContract]
    public class SearchRoleData
    {
        /// <summary>
        /// 成员角色ID
        /// </summary>
        [ProtoMember(1)]
        public int RoleID = 0;

        /// <summary>
        /// 成员角色名称
        /// </summary>
        [ProtoMember(2)]
        public string RoleName;

        /// <summary>
        /// 角色的性别
        /// </summary>
        [ProtoMember(3)]
        public int RoleSex = 0;

        /// <summary>
        /// 成员的等级
        /// </summary>
        [ProtoMember(4)]
        public int Level = 0;

        /// <summary>
        /// 成员的职业
        /// </summary>
        [ProtoMember(5)]
        public int Occupation = 0;

        /// <summary>
        /// 所在的地图的编号
        /// </summary>
        [ProtoMember(6)]
        public int MapCode = 0;

        /// <summary>
        /// 所在位置的X坐标
        /// </summary>
        [ProtoMember(7)]
        public int PosX = 0;

        /// <summary>
        /// 所在位置的X坐标
        /// </summary>
        [ProtoMember(8)]
        public int PosY = 0;

        /// <summary>
        /// 帮会的ID
        /// </summary>
        [ProtoMember(9)]
        public int Faction = 0;

        /// <summary>
        /// 帮会的名称
        /// </summary>
        [ProtoMember(10)]
        public string BHName = "";
    }
}