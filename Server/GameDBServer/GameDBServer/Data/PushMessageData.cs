using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    // 推送消息数据  [5/3/2014 LiaoWei]
    
    /// <summary>
    /// 任务奖励数据
    /// </summary>
    [ProtoContract]
    public class PushMessageData
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ProtoMember(1)]
        public string UserID = "";

        /// <summary>
        /// 推送ID
        /// </summary>
        [ProtoMember(2)]
        public string PushID = "";

        /// <summary>
        /// 登陆时间
        /// </summary>
        [ProtoMember(3)]
        public string LastLoginTime = "";

    }
}
