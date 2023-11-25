using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 服务器线路列表数据
    /// </summary>
    [ProtoContract]
    public class ServerListData
    {
        /// <summary>
        /// 返回的错误码
        /// </summary>
        [ProtoMember(1)]
        public int RetCode = 0;

        /// <summary>
        /// 当前的角色数量
        /// </summary>
        [ProtoMember(2)]
        public int RolesCount = 0;

        /// <summary>
        /// 线路列表数据
        /// </summary>
        [ProtoMember(3)]
        public List<LineData> LineDataList = null;
    }
}
