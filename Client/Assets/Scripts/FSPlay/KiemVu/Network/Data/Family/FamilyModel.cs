using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{ /// <summary>
  /// Gia tộc
  /// </summary>
  ///
    [ProtoContract]
    public class Family
    {
        /// <summary>
        /// ID Của gia tộc
        /// </summary>
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
        ///  Danh sách gia tộc
        /// </summary>
        [ProtoMember(6)]
        public string RequestNotify { get; set; }

        /// <summary>
        /// Thông tin thành viên
        /// </summary>
        ///
        [ProtoMember(7)]
        public List<FamilyMember> Members { get; set; }

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
    }

    /// <summary>
    /// Danh sách gia tộc
    /// </summary>
    [ProtoContract]
    public class FamilyList
    {
        /// <summary>
        /// Tổng số gia tộc
        /// </summary>
        [ProtoMember(1)]
        public int TotalFamily{ get; set; }
        /// <summary>
        /// Danh sách gia tộc
        /// </summary>
         [ProtoMember(2)]
        public List<FamilyInfo> FamilyLists { get; set; }
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
        /// <summary>
        /// tổng điểm uy danh
        /// </summary>

        [ProtoMember(4)]
        public int TotalPoint { get; set; }
        /// <summary>
        /// Slogan của bang
        /// </summary>
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

        /// <summary>
        /// Trạng thái Online
        /// </summary>
        [ProtoMember(7)]
        public int OnlienStatus { get; set; }
    }
}
