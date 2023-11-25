using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 任务数据
    /// </summary>
    [ProtoContract]
    public class TaskData
    {
        /// <summary>
        /// 数据库ID
        /// </summary>
        [ProtoMember(1)]
        public int DbID;

        /// <summary>
        /// 已经接受的任务列表
        /// </summary>
        [ProtoMember(2)]
        public int DoingTaskID;

        /// <summary>
        /// 已经接受的任务数值列表1
        /// </summary>
        [ProtoMember(3)]
        public int DoingTaskVal1;

        /// <summary>
        /// 已经接受的任务数值列表2
        /// </summary>
        [ProtoMember(4)]
        public int DoingTaskVal2;

        /// <summary>
        /// 已经接受的任务追踪列表
        /// </summary>
        [ProtoMember(5)]
        public int DoingTaskFocus;

        /// <summary>
        /// 任务添加的时间(单位秒)
        /// </summary>
        [ProtoMember(6)]
        public long AddDateTime;

        /// <summary>
        /// 任务奖励数据
        /// </summary>
        [ProtoMember(7)]
        public TaskAwardsData TaskAwards = null;

        /// <summary>
        /// 已经做过的次数
        /// </summary>
        [ProtoMember(8)]
        public int DoneCount = 0;

        /// <summary>
        /// 任务星级信息 [12/3/2013 LiaoWei]
        /// </summary>
        [ProtoMember(9)]
        public int StarLevel = 0;
    }
}
