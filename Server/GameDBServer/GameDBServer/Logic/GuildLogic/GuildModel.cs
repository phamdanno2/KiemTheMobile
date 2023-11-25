using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server.Data
{
    public enum GuildRank
    {
        /// <summary>
        /// Bang chúng
        /// </summary>
        Member = 0,

        /// <summary>
        /// Bang chủ
        /// </summary>
        Master = 1,

        /// <summary>
        /// Phó bang chủ
        /// </summary>
        ViceMaster = 2,

        /// <summary>
        /// Trưởng lão
        /// </summary>
        Ambassador = 3,

        /// <summary>
        /// Tinh anh
        /// </summary>
        Elite = 4,

        /// <summary>
        /// Tổng số
        /// </summary>
        Count,
    }


    [ProtoContract]
    public class MiniGuildInfo
    {
        [ProtoMember(1)]
        public int GuildId { get; set; }
        [ProtoMember(2)]
        public string GuildName { get; set; }
        [ProtoMember(3)]
        public int MoneyStore { get; set; }
        [ProtoMember(4)]
        public int TotalPrestige { get; set; }

        [ProtoMember(5)]
        public int TotalMember { get; set; }
    }

    /// <summary>
    /// Thực thể bang hội
    /// </summary>
    public class Guild
    {
        public long LastCacluationShare { get; set; }

        /// <summary>
        /// Id GUild
        /// </summary>
        public int GuildID { get; set; }

        /// <summary>
        /// Tên Bang hội
        /// </summary>
        public string GuildName { get; set; }

        /// <summary>
        /// Quỹ thưởng
        /// </summary>
        public int MoneyBound { get; set; }

        /// <summary>
        /// Quỹ bang
        /// </summary>
        public int MoneyStore { get; set; }

        /// <summary>
        /// ZONEID
        /// </summary>
        public int ZoneID { get; set; }

        /// <summary>
        /// Thông Báo Bang
        /// </summary>
        public string Notify { get; set; }

        /// <summary>
        /// Tổng số lãnh thổ
        /// </summary>
        public int TotalTerritory { get; set; }

        /// <summary>
        /// Tỉ lệ tối đa có thể rút
        /// </summary>
        public int MaxWithDraw { get; set; }

        /// <summary>
        /// Bang chủ là ai
        /// </summary>
        public int Leader { get; set; }

        /// <summary>
        /// Ngày tạo bang
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Danh sách cổ tức thành viên
        /// </summary>
        public List<GuildShare> GuildShare { get; set; }


        public List<GuildShare> CacheGuildShare { get; set; }

        /// <summary>
        /// Danh sách gia tộc
        /// </summary>
        public string Familys { get; set; }

        /// <summary>
        /// Update GuildMember
        /// </summary>
        public ConcurrentDictionary<int, GuildMember> GuildMember { get; set; }




        /// <summary>
        /// Danh sách lãnh thổ
        /// </summary>
        public List<Territory> Territorys { get; set; }

        public List<GuildVote> GuildVotes { get; set; }
    }

    #region CLIENT TCP

    /// <summary>
    /// Thành viên ưu tú
    /// </summary>
    [ProtoContract]
    public class OfficeRankInfo
    {
        /// <summary>
        /// Cấp hiện tại của bang
        /// </summary>
        [ProtoMember(1)]
        public int GuildRank { get; set; }

        [ProtoMember(2)]
        public List<OfficeRankMember> OffcieRankMember { get; set; }
    }

    [ProtoContract]
    public class OfficeRankMember
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public string RoleName { get; set; }

        [ProtoMember(3)]
        public string RankTile { get; set; }

        [ProtoMember(4)]
        public string OfficeRankTitle { get; set; }

        [ProtoMember(5)]
        public int RoleID { get; set; }

        [ProtoMember(6)]
        public int RankNum { get; set; }
    }

    /// <summary>
    /// Thông tin ưu tú bang
    /// </summary>
    [ProtoContract]
    public class GuildVoteInfo
    {
        /// <summary>
        /// ID tuần
        /// </summary>
        [ProtoMember(1)]
        public int WEEID { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        [ProtoMember(2)]
        public string Start { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [ProtoMember(3)]
        public string End { get; set; }

        /// <summary>
        /// Thành viên mà bản thân đã bầu
        /// </summary>
        [ProtoMember(4)]
        public string VoteFor { get; set; }

        /// <summary>
        /// Danh sách thành viên ưu tú
        /// </summary>
        [ProtoMember(5)]
        public List<GuildMember> GuildMember { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(6)]
        public int TotalPage { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        [ProtoMember(7)]
        public int PageIndex { get; set; }
    }

    [ProtoContract]
    public class GuildInfomation
    {
        /// <summary>
        /// Id Bang hội
        /// </summary>
        ///
        [ProtoMember(1)]
        public int GuildID { get; set; }

        /// <summary>
        /// Tên bang hội
        /// </summary>
        [ProtoMember(2)]
        public string GuildName { get; set; }

        /// <summary>
        /// Tổng số gia tộc
        /// </summary>
        [ProtoMember(3)]
        public int FamilyCount { get; set; }

        /// <summary>
        /// Tổng số thành viên
        /// </summary>
        [ProtoMember(4)]
        public int MemberCount { get; set; }

        /// <summary>
        /// Tổng số lãnh thổ
        /// </summary>
        [ProtoMember(5)]
        public int TerritoryCount { get; set; }

        /// <summary>
        /// Tổng uy danh
        /// </summary>
        [ProtoMember(6)]
        public int TotalPrestige { get; set; }

        /// <summary>
        /// Quỹ bang
        /// </summary>
        [ProtoMember(7)]
        public int MoneyStore { get; set; }

        /// <summary>
        /// Lợi tức tối đa có thể rút
        /// </summary>
        [ProtoMember(8)]
        public int MaxWithDraw { get; set; }

        /// <summary>
        /// Quỹ thưởng
        /// </summary>
        [ProtoMember(9)]
        public int MoneyBound { get; set; }

        /// <summary>
        /// Tài sản cá nhân
        /// </summary>
        [ProtoMember(10)]
        public int RoleGuildMoney { get; set; }

        /// <summary>
        /// Thông báo bang hội
        /// </summary>
        [ProtoMember(11)]
        public string Notify { get; set; }

        /// <summary>
        /// Tên bang chủ
        /// </summary>
        [ProtoMember(12)]
        public string GuildMasterName { get; set; }
    }

    [ProtoContract]
    public class FamilyObj
    {
        [ProtoMember(1)]
        public int FamilyID { get; set; }

        [ProtoMember(2)]
        public string FamilyName { get; set; }

        [ProtoMember(3)]
        public int MemberCount { get; set; }

        [ProtoMember(4)]
        public int TotalpPrestige { get; set; }
    }

    /// <summary>
    /// Thông tin thành viên bang hội
    /// </summary>
    [ProtoContract]
    public class GuildMemberData
    {
        /// <summary>
        /// Danh sách tộc
        /// </summary>
        [ProtoMember(1)]
        public List<FamilyObj> TotalFamilyMemeber { get; set; }

        /// <summary>
        /// Danh sách thành viên
        /// </summary>
        [ProtoMember(2)]
        public List<GuildMember> TotalGuildMember { get; set; }

        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        [ProtoMember(3)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(4)]
        public int TotalPage { get; set; }
    }

    /// <summary>
    /// Thành viên trong bang
    /// </summary>
    ///
    [ProtoContract]
    public class GuildMember
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
        /// ID của GUild hiện tại
        /// </summary>
        [ProtoMember(7)]
        public int GuildID { get; set; }

        /// <summary>
        /// ID gia tộc
        /// </summary>
        [ProtoMember(8)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        [ProtoMember(9)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Tài sản cá nhân
        /// </summary>
        [ProtoMember(10)]
        public int GuildMoney { get; set; }

        /// <summary>
        /// Trạng thái online
        /// </summary>
        [ProtoMember(11)]
        public int OnlienStatus { get; set; }

        /// <summary>
        /// ZoneID nào
        /// </summary>
        [ProtoMember(12)]
        public int ZoneID { get; set; }

        /// <summary>
        /// Nhận bao nhiêu phiếu bầu ưu tú
        /// </summary>
        [ProtoMember(13)]
        public int TotalVote { get; set; }

        /// <summary>
        /// Hạng ưu tú
        /// </summary>
        [ProtoMember(14)]
        public int VoteRank { get; set; }
    }

    [ProtoContract]
    public class TerritoryInfo
    {
        [ProtoMember(1)]
        public int TerritoryCount { get; set; }

        [ProtoMember(2)]
        public List<Territory> Territorys { get; set; }

    }

    #endregion CLIENT TCP

    /// <summary>
    /// Bầu ưu tú
    /// </summary>
    public class GuildVote
    {
        /// <summary>
        /// ID vote
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ZONEID
        /// </summary>
        public int ZoneID { get; set; }

        /// <summary>
        /// ID bang hội
        /// </summary>
        public int GuildID { get; set; }

        /// <summary>
        /// AI thực hiện vote
        /// </summary>
        public int RoleVote { get; set; }

        /// <summary>
        /// Số phiếu bỏ cho người được vote
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// WEEKID
        /// </summary>
        public int WeekID { get; set; }

        /// <summary>
        /// Role nào nhận được vote
        /// </summary>
        public int RoleReviceVote { get; set; }
    }

    /// <summary>
    /// Danh sách lãnh thổ của bang
    /// </summary>
    ///
    [ProtoContract]
    public class Territory
    {
        /// <summary>
        /// ID
        /// </summary>
        ///
        [ProtoMember(1)]
        public int ID { get; set; }

        /// <summary>
        /// MapID
        /// </summary>
        ///
        [ProtoMember(2)]
        public int MapID { get; set; }

        /// <summary>
        /// MapNAME
        /// </summary>
        ///
        [ProtoMember(3)]
        public string MapName { get; set; }

        /// <summary>
        /// ID BANG
        /// </summary>
        ///
        [ProtoMember(4)]
        public int GuildID { get; set; }

        /// <summary>
        /// Số sao của lãnh thổ
        /// </summary>
        ///
        [ProtoMember(5)]
        public int Star { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        ///
        [ProtoMember(6)]
        public int Tax { get; set; }

        /// <summary>
        /// ID khu vực
        /// </summary>
        ///
        [ProtoMember(7)]
        public int ZoneID { get; set; }

        /// <summary>
        /// Có phải thành phố chính không
        /// </summary>
        ///
        [ProtoMember(8)]
        public int IsMainCity { get; set; }

        [ProtoMember(9)]
        public string GuildName { get; set; }
    }

    /// <summary>
    /// Thông tin cổ tức
    /// </summary>
    [ProtoContract]
    public class GuildShareInfo
    {
        /// <summary>
        /// Tổng số trang
        /// </summary>
        [ProtoMember(1)]
        public int TotalPage { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        [ProtoMember(2)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Danh sách thành viên có cổ tức
        /// </summary>
        [ProtoMember(3)]
        public List<GuildShare> MemberList { get; set; }
    }

    /// <summary>
    /// Cổ tức của các thành viên
    /// </summary>
    ///
    [ProtoContract]
    public class GuildShare
    {
        /// <summary>
        /// ID
        /// </summary>
        [ProtoMember(1)]
        public int ID { get; set; }

        /// <summary>
        /// Id của role
        /// </summary>
        [ProtoMember(2)]
        public int RoleID { get; set; }

        /// <summary>
        /// ID gia tộc
        /// </summary>
        [ProtoMember(3)]
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        [ProtoMember(4)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Cấp độ của thành viên
        /// </summary>
        [ProtoMember(5)]
        public int RoleLevel { get; set; }

        /// <summary>
        /// FactionID
        /// </summary>
        [ProtoMember(6)]
        public int FactionID { get; set; }

        /// <summary>
        /// Tỉ lệ %
        /// </summary>
        [ProtoMember(7)]
        public double Share { get; set; }

        /// <summary>
        /// Tên thành viên
        /// </summary>
        [ProtoMember(8)]
        public string RoleName { get; set; }

        /// <summary>
        /// Hạng của thành viên tại BANG
        /// </summary>
        [ProtoMember(9)]
        public int Rank { get; set; }
    }

    /// <summary>
	/// Sửa tôn chỉ bang hội
	/// </summary>
	[ProtoContract]
    public class GuildChangeSlogan
    {
        /// <summary>
        /// ID bang hội
        /// </summary>
        [ProtoMember(1)]
        public int GuildID { get; set; }

        /// <summary>
        /// Tôn chỉ bang hội
        /// </summary>
        [ProtoMember(2)]
        public string Slogan { get; set; }
    }
}