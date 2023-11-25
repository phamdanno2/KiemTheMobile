using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    // 充值平台类型
    enum ChargePlatformType
    {
        CPT_Unknown = 0,
        CPT_App = 1,
        CPT_Android = 2,
        CPT_YueYu = 3
    }

    [ProtoContract]
    public class SingleChargeData
    {
        /// <summary>
        /// 充值档信息
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<int, int> singleData = new Dictionary<int, int>();

        /// <summary>
        /// 存储本平台的月卡价格
        /// </summary>
        [ProtoMember(2)]
        public int YueKaMoney = 0;

        /// <summary>
        /// 平台类型
        /// </summary>
        [ProtoMember(3)]
        public int ChargePlatType = (int)ChargePlatformType.CPT_Unknown;
    }

}
