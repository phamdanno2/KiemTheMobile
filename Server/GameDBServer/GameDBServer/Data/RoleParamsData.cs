using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 角色参数
    /// </summary>
    [ProtoContract]
    public class RoleParamsData
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [ProtoMember(1)]
        public string ParamName = "";

        /// <summary>
        /// 今日经验
        /// </summary>
        [ProtoMember(2)]
        public string ParamValue = "";

        /// <summary>
        /// 最早更新失败时间，不序列化
        /// </summary>
        public long UpdateFaildTicks;

        /// <summary>
        /// 类型信息
        /// </summary>
        public RoleParamType ParamType;
    }

    /// <summary>
    /// 角色参数存储位置和类型信息
    /// </summary>
    public class RoleParamType
    {
        public enum ValueTypes
        {
            Normal,
            Char128,
            Long,
        }

        public readonly string VarName;
        public readonly string ParamName;
        public readonly string TableName;
        public readonly string IdxName;
        public readonly string ColumnName;
        public readonly int ParamIndex;
        public readonly int IdxKey;
        public readonly int Type;

        public readonly string KeyString;

        public RoleParamType(string varName, string paramName, string tableName, string idxName, string columnName, int idxKey, int paramIndex, int type)
        {
            VarName = varName;
            ParamName = paramName;
            TableName = tableName;
            IdxName = idxName;
            ColumnName = columnName;
            IdxKey = idxKey;
            ParamIndex = paramIndex;
            Type = type;
            if (Type > 0)
            {
                KeyString = IdxKey.ToString();
            }
            else
            {
                KeyString = "\'" + ParamName + '\'';
            }
        }
    }
}
