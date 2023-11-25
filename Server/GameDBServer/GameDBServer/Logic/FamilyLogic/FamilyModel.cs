using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Server.Data
{

    /// <summary>
    /// Danh hiệu gia tộc
    /// </summary>
    public enum FamilyRank
    {
        /// <summary>
        /// Thành Viên
        /// </summary>
        Member = 0,
        /// <summary>
        /// Tộc Trưởng
        /// </summary>
        Master = 1,
        /// <summary>
        /// Tộc Phó
        /// </summary>
        ViceMaster = 2,
        /// <summary>

        Count,
    }
    /// <summary>
    /// Gia tộc
    /// </summary>
    ///
    [ProtoContract]
    public class Family
    {
        /// <summary>
        /// ID Của gia tộc
        /// </summary>
        ///

        [ProtoMember(1)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        ///
        [ProtoMember(2)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Tộc trưởng
        /// </summary>
        ///
        [ProtoMember(3)]
        public int Learder { get; set; }

        /// <summary>
        /// Tổng Uy Danh
        /// </summary>
        ///
        [ProtoMember(4)]
        public int TotalPoint { get; set; }

        /// <summary>
        /// Thông báo gia tộc
        /// </summary>
        ///
        [ProtoMember(5)]
        public string Notification { get; set; }

        /// <summary>
        /// Danh sách thành viên
        /// </summary>
        ///

        [ProtoMember(6)]
        public string RequestNotify { get; set; }

        /// <summary>
        /// Thông tin thành viên
        /// </summary>
        ///
        [ProtoMember(7)]
        public List<FamilyMember> Members { get; set; } = new List<FamilyMember>();

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        ///
        [ProtoMember(8)]
        public DateTime DateCreate { get; set; }

        [ProtoMember(9)]
        public List<RequestJoin> JoinRequest { get; set; }

        /// <summary>
        /// Tiền gia tộc được cộng hàng tuần
        /// </summary>
        [ProtoMember(10)]
        public int FamilyMoney { get; set; }
        
        /// <summary>
        /// Số lần đã vượt ải gia tộc trong tuần
        /// </summary>
        [ProtoMember(11)]
        public int WeeklyFubenCount { get; set; }

        /// </summary>
        [ProtoMember(12)]
        public int PageCount { get; set; }

    }

    [ProtoContract]
    public class ChangeSlogenFamily
    {
        /// <summary>
        /// ID Của gia tộc
        /// </summary>
        [ProtoMember(1)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Chuỗi Slogen gia tộc
        /// </summary>
        [ProtoMember(2)]
        public string Slogen { get; set; }
    }



    [ProtoContract]
    public class FamilyInfo
    {
        [ProtoMember(1)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        ///
        [ProtoMember(2)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Tộc trưởng
        /// </summary>
        ///
        [ProtoMember(3)]
        public string Learder { get; set; }

        [ProtoMember(4)]
        public int TotalPoint { get; set; }

        [ProtoMember(5)]
        public string RequestNotify { get; set; }

        [ProtoMember(6)]
        public int FamilyZoneID { get; set; }


        /// <summary>
        /// Tổng số thành viên
        /// </summary>
        [ProtoMember(7)]
        public int TotalMember { get; set; }

        [ProtoMember(8)]
        public string GuildName { get; set; }

    }

    [ProtoContract]
    public class RequestJoin
    {

        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public int RoleID { get; set; }

        [ProtoMember(3)]
        public string RoleName { get; set; }

        [ProtoMember(4)]
        public int RoleFactionID { get; set; }

        [ProtoMember(5)]
        public int FamilyID { get; set; }

        [ProtoMember(6)]
        public int RoleLevel { get; set; }

        [ProtoMember(7)]
        public int RolePrestige { get; set; }

        [ProtoMember(8)]
        public DateTime TimeRequest { get; set; }
    }

    /// <summary>
    /// Thành viên gia tộc
    /// </summary>
    ///
    [ProtoContract]
    public class FamilyMember
    {
        /// <summary>
        /// ID Role
        /// </summary>
        ///
        [ProtoMember(1)]
        public int RoleID { get; set; }

        /// <summary>
        /// Tên của thành viên
        /// </summary>
        ///
        [ProtoMember(2)]
        public string RoleName { get; set; }

        /// <summary>
        /// Phái nào
        /// </summary>
        ///
        [ProtoMember(3)]
        public int FactionID { get; set; }

        /// <summary>
        /// Cấp bậc
        /// </summary>
        ///
        [ProtoMember(4)]
        public int Rank { get; set; }

        /// <summary>
        /// Cấp độ
        /// </summary>
        ///
        [ProtoMember(5)]
        public int Level { get; set; }

        /// <summary>
        /// Uy danh hiện có
        /// </summary>
        ///
        [ProtoMember(6)]
        public int Prestige { get; set; }

        [ProtoMember(7)]
        public int OnlienStatus { get; set; }
    }
}