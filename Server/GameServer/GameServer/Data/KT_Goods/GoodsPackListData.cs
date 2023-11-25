using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// 掉落包裹的物品列表通知数据
    /// </summary>
    [ProtoContract]
    public class GoodsPackListData
    {
        /// <summary>
        /// 掉落包裹的流水ID
        /// </summary>
        [ProtoMember(1)]
        public int AutoID = -1;

        /// <summary>
        /// 物品列表
        /// </summary>
        [ProtoMember(2)]
        public List<GoodsData> GoodsDataList = null;

        /// <summary>
        /// 打开的状态
        /// </summary>
        [ProtoMember(3)]
        public int OpenState = -1;

        /// <summary>
        /// 返回的错误码
        /// </summary>
        [ProtoMember(4)]
        public int RetError = 0;

        /// <summary>
        /// 剩余的时间(毫秒)
        /// </summary>
        [ProtoMember(5)]
        public long LeftTicks = 0;

        /// <summary>
        /// 包裹允许打开的时间
        /// </summary>
        [ProtoMember(6)]
        public long PackTicks = -1;
    }
}