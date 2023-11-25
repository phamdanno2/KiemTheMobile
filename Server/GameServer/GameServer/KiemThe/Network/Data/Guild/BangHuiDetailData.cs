using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// Thông tin thành viên bang hội
    /// </summary>
    [ProtoContract]
    public class BangHuiMgrItemData
    {
        /// <summary>
        /// ID khu vực
        /// </summary>
        [ProtoMember(1)]
        public int ZoneID { get; set; } = 0;

        /// <summary>
        /// ID nhân vật
        /// </summary>
        [ProtoMember(2)]
        public int RoleID { get; set; } = 0;

        /// <summary>
        /// Tên nhân vật
        /// </summary>
        [ProtoMember(3)]
        public string RoleName { get; set; } = "";

        /// <summary>
        /// Môn phái
        /// </summary>
        [ProtoMember(4)]
        public int Occupation { get; set; } = 0;

        /// <summary>
        /// Chức vụ trong bang
        /// </summary>
        [ProtoMember(5)]
        public int BHZhiwu { get; set; } = 0;

        /// <summary>
        /// Danh hiệu bang
        /// </summary>
        [ProtoMember(6)]
        public string ChengHao { get; set; } = "";

        /// <summary>
        /// Bang cống
        /// </summary>
        [ProtoMember(7)]
        public int BangGong { get; set; } = 0;

        /// <summary>
        /// Cấp độ
        /// </summary>
        [ProtoMember(8)]
        public int Level { get; set; } = 0;

        /// <summary>
        /// Trạng thái hiện tại (0: Offline, 1: Online)
        /// </summary>
        [ProtoMember(9)]
        public int OnlineState { get; set; } = 0;
    }

    /// <summary>
    /// Thông tin chi tiết bang hội
    /// </summary>
    [ProtoContract]
    public class BangHuiDetailData
    {
        /// <summary>
        /// ID bang
        /// </summary>
        [ProtoMember(1)]
        public int BHID { get; set; } = 0;

        /// <summary>
        /// Tên bang
        /// </summary>
        [ProtoMember(2)]
        public string BHName { get; set; } = "";

        /// <summary>
        /// ID khu vực
        /// </summary>
        [ProtoMember(3)]
        public int ZoneID { get; set; } = 0;

        /// <summary>
        /// ID bang chủ
        /// </summary>
        [ProtoMember(4)]
        public int BZRoleID { get; set; } = 0;

        /// <summary>
        /// Tên bang chủ
        /// </summary>
        [ProtoMember(5)]
        public string BZRoleName { get; set; } = "";

        /// <summary>
        /// Tổng số thành viên
        /// </summary>
        [ProtoMember(6)]
        public int TotalNum { get; set; } = 0;

        /// <summary>
        /// Cấp độ
        /// </summary>
        [ProtoMember(7)]
        public int TotalLevel { get; set; } = 0;

        /// <summary>
        /// Tôn chỉ bang
        /// </summary>
        [ProtoMember(8)]
        public string BHBulletin { get; set; } = "";

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        [ProtoMember(9)]
        public string BuildTime { get; set; } = "";

        /// <summary>
        /// Tên cờ
        /// </summary>
        [ProtoMember(10)]
        public string QiName { get; set; } = "";

        /// <summary>
        /// Cấp bậc bang hội
        /// </summary>
        [ProtoMember(11)]
        public int QiLevel { get; set; } = 0;

        /// <summary>
        /// Quản lý danh sách thành viên
        /// </summary>
        [ProtoMember(12)]
        public List<BangHuiMgrItemData> MgrItemList { get; set; } = null;

        /// <summary>
        /// Bang hội đã được xác minh chưa
        /// </summary>
        [ProtoMember(13)]
        public int IsVerify { get; set; } = 0;

        /// <summary>
        /// Tổng số bạc quỹ bang
        /// </summary>
        [ProtoMember(14)]
        public int TotalMoney { get; set; } = 0;

        /// <summary>
        /// Chiến công đổi bạc hôm nay
        /// </summary>
        [ProtoMember(15)]
        public int TodayZhanGongForGold = 0;

        /// <summary>
        /// Có thể đổi tên bao nhiêu lần nữa
        /// </summary>
        [ProtoMember(16)]
        public int CanModNameTimes { get; set; } = 0;
    }
}
