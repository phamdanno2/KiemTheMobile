using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Dữ liệu vật phẩm gửi về từ Server
    /// </summary>
    [ProtoContract]
    public class ItemGenByteData
    {
        /// <summary>
        /// Tổng số thuộc tính cơ bản
        /// </summary>
        [ProtoMember(1)]
        public int BasicPropCount { get; set; }

        /// <summary>
        /// Danh sách chỉ số thuộc tính cơ bản
        /// </summary>

        [ProtoMember(2)]
        public List<int> BasicPropValue { get; set; }

        /// <summary>
        /// Có thuộc tính mật tịch không
        /// </summary>

        [ProtoMember(3)]
        public bool HaveBookProperties { get; set; }

        /// <summary>
        /// Danh sách thuộc tính mật tịch
        /// </summary>

        [ProtoMember(4)]
        public List<int> BookPropertyValue { get; set; }

        /// <summary>
        /// Tổng số dòng xanh lá
        /// </summary>

        [ProtoMember(5)]
        public int GreenPropCount { get; set; }

        /// <summary>
        /// Danh sách thuộc tính xanh lá
        /// </summary>

        [ProtoMember(6)]
        public List<int> GreenPropValue { get; set; }

        /// <summary>
        /// Tổng số dòng ẩn
        /// </summary>

        [ProtoMember(7)]
        public int HiddenProbsCount { get; set; }

        /// <summary>
        /// Danh sách thuộc tính dòng ẩn
        /// </summary>

        [ProtoMember(8)]
        public List<int> HiddenProbsValue { get; set; }
    }
}
