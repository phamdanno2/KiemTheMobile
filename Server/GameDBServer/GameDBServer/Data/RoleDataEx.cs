using ProtoBuf;
using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Đối tượng RoleDataEx chứa thêm thông tin cho các sự kiện event
    /// </summary>
    [ProtoContract]
    public class RoleDataEx
    {
        /// <summary>
        /// ID của nhân vật
        /// </summary>
        [ProtoMember(1)]
        public int RoleID = 0;

        /// <summary>
        /// Tên của nhân vật
        /// </summary>
        [ProtoMember(2)]
        public string RoleName = "";

        /// <summary>
        /// Giới tính của nhân vật
        /// </summary>
        [ProtoMember(3)]
        public int RoleSex = 0;

        /// <summary>
        /// Phái
        /// </summary>
        [ProtoMember(4)]
        public int FactionID = 0;

        /// <summary>
        /// Cấp độ
        /// </summary>
        [ProtoMember(5)]
        public int Level = 1;

        /// <summary>
        /// Bạc Khóa
        /// </summary>
        [ProtoMember(6)]
        public int Money = 0;

        /// <summary>
        /// Bạc Thường
        /// </summary>
        [ProtoMember(7)]
        public int Money2 = 0;

        /// <summary>
        /// Kinh nghiệm
        /// </summary>
        [ProtoMember(8)]
        public long Experience = 0;

        /// <summary>
        /// Chế độ PK
        /// </summary>
        [ProtoMember(9)]
        public int PKMode = 0;

        /// <summary>
        /// Giá trị PK
        /// </summary>
        [ProtoMember(10)]
        public int PKValue = 0;

        /// <summary>
        /// Bản đồ hiện tịa
        /// </summary>
        [ProtoMember(11)]
        public int MapCode = 0;

        /// <summary>
        /// Tọa độ X hiện tại
        /// </summary>
        [ProtoMember(12)]
        public int PosX { get; set; } = 0;

        /// <summary>
        /// Tọa độ Y hiện tại
        /// </summary>
        [ProtoMember(13)]
        public int PosY = 0;

        /// <summary>
        /// Hướng của nhân vật
        /// </summary>
        [ProtoMember(14)]
        public int RoleDirection = 0;

        /// <summary>
        /// Lượng máu tối đa của nhân vật
        /// </summary>
        [ProtoMember(15)]
        public int MaxHP = 0;

        /// <summary>
        /// Lượng mana tối đa
        /// </summary>
        [ProtoMember(16)]
        public int MaxMP = 0;

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        [ProtoMember(17)]
        public List<OldTaskData> OldTasks = null;

        /// <summary>
        /// Ảnh Avata của nhân vật
        /// </summary>
        [ProtoMember(18)]
        public int RolePic = 0;

        /// <summary>
        /// Số ô đang mở hiện tại
        /// </summary>
        [ProtoMember(19)]
        public int BagNum = 0;

        /// <summary>
        /// Danh Sách nhiệm vụ
        /// </summary>
        [ProtoMember(20)]
        public List<TaskData> TaskDataList = null;

        /// <summary>
        /// Danh sách vật phẩm đang có
        /// </summary>
        [ProtoMember(21)]
        public List<GoodsData> GoodsDataList = null;

        /// <summary>
        /// Tên khác
        /// </summary>
        [ProtoMember(22)]
        public string OtherName = "";

        /// <summary>
        /// Danh sách SKill Tay trái
        /// </summary>
        [ProtoMember(23)]
        public string MainQuickBarKeys = "";

        /// <summary>
        /// Danh sách skill tay phải
        /// </summary>
        [ProtoMember(24)]
        public string OtherQuickBarKeys = "";

        /// <summary>
        /// Số lần đăng nhập
        /// </summary>
        [ProtoMember(25)]
        public int LoginNum = 0;

        /// <summary>
        /// Tiền đồng
        /// </summary>
        [ProtoMember(26)]
        public int Token = 0;

        /// <summary>
        /// Số giây chiến còn lại
        /// </summary>
        [ProtoMember(27)]
        public int LeftFightSeconds = 0;

        /// <summary>
        /// Danh sách bạn bè
        /// </summary>
        [ProtoMember(28)]
        public List<FriendData> FriendDataList = null;

        /// <summary>
        /// Tổng số thời gian online
        /// </summary>
        [ProtoMember(29)]
        public int TotalOnlineSecs = 0;

        /// <summary>
        /// Thời gian đăng nhập gần đây
        /// </summary>
        [ProtoMember(30)]
        public long LastOfflineTime = 0;

        /// <summary>
        /// Danh sách kỹ năng
        /// </summary>
        [ProtoMember(31)]
        public List<SkillData> SkillDataList = null;

        /// <summary>
        /// Thời gian đăng ký
        /// </summary>
        [ProtoMember(32)]
        public long RegTime = 0;

        /// <summary>
        /// Bán hàng
        /// </summary>
        [ProtoMember(33)]
        public List<GoodsData> SaleGoodsDataList = null;

        /// <summary>
        /// Danh sách bufff
        /// </summary>
        [ProtoMember(34)]
        public List<BufferData> BufferDataList = null;

        /// <summary>
        /// Nhiệm vụ hàng ngày
        /// </summary>
        [ProtoMember(35)]
        public List<DailyTaskData> MyDailyTaskDataList = null;

        /// <summary>
        /// TỔng số skill
        /// </summary>
        [ProtoMember(36)]
        public int NumSkillID = 0;

        /// <summary>
        /// Kho di động
        /// </summary>
        [ProtoMember(37)]
        public PortableBagData MyPortableBagData = null;

        /// <summary>
        /// Quà tặng cái này phải giữ cho sự kiện open server
        /// </summary>
        [ProtoMember(38)]
        public HuodongData MyHuodongData = null;

        /// <summary>
        /// Nhiệm vú chính tuyến
        /// </summary>
        [ProtoMember(39)]
        public int MainTaskID = 0;

        /// <summary>
        /// Điểmmm PK
        /// </summary>
        [ProtoMember(40)]
        public int PKPoint = 0;

        /// <summary>
        /// Dữ liệu hằng ngày của người chơi | số lượng đi bạch hổ đường | số lượng đi tiêu dao cốc vv
        /// </summary>
        [ProtoMember(41)]
        public RoleDailyData MyRoleDailyData = null;

        /// <summary>
        /// TỔng số bosss bị giết
        /// </summary>
        [ProtoMember(42)]
        public int KillBoss = 0;

        /// <summary>
        /// ZoneID của máy chủ
        /// </summary>
        [ProtoMember(43)]
        public int ZoneID = 0;

        /// <summary>
        ///  Bạc khóa
        /// </summary>
        [ProtoMember(44)]
        public int BoundToken = 0;

        /// <summary>
        /// ID Email cuối cùng
        /// </summary>
        [ProtoMember(45)]
        public int LastMailID = 0;

        /// <summary>
        ///  Vàng
        /// </summary>
        [ProtoMember(46)]
        public int BoundMoney = 0;

        /// <summary>
        /// Sử dụng vật phẩm limit
        /// </summary>
        [ProtoMember(47)]
        public List<GoodsLimitData> GoodsLimitDataList = null;

        /// <summary>
        /// Dữ liệu nhân vật
        /// </summary>
        [ProtoMember(48)]
        public Dictionary<string, RoleParamsData> RoleParamsDict = null;

        /// <summary>
        ///  Có bị band chat không
        /// </summary>
        [ProtoMember(49)]
        public int BanChat = 0;

        /// <summary>
        ///  Có bị cấm đăng nhập không
        /// </summary>
        [ProtoMember(50)]
        public int BanLogin = 0;

        /// <summary>
        ///  Có phải người chơi mới không
        /// </summary>
        [ProtoMember(51)]
        public int IsFlashPlayer = 0;

        // MU项目增加字段 [12/10/2013 LiaoWei]
        /// <summary>
        /// Thời gian hạn chế
        /// </summary>
        [ProtoMember(52)]
        public int AdmiredCount = 0;

        // MU项目增加字段 [4/23/2014 LiaoWei]
        /// <summary>
        /// Push Messenger từ SDK
        /// </summary>
        [ProtoMember(53)]
        public string PushMessageID = "";

        /// <summary>
        /// Đồng khóa store ở thủ khố
        /// </summary>
        [ProtoMember(54)]
        public long Store_Yinliang = 0;

        /// <summary>
        /// Tiền ở thủ khố
        /// </summary>
        [ProtoMember(55)]
        public int Store_Money = 0;

        [ProtoMember(56)]
        public List<int> GroupMailRecordList = null;

        [ProtoMember(57)]
        public int Potential = 0;

        [ProtoMember(58)]
        public Dictionary<int, Dictionary<int, SevenDayItemData>> SevenDayActDict = null;

        [ProtoMember(59)]
        public long BantTradeToTicks;

        /// <summary>
        /// ID Nhánh
        /// </summary>

        [ProtoMember(60)]
        public int SubID = 0;

        /// <summary>
        /// ID môn phái
        /// </summary>

        [ProtoMember(61)]
        public int Vitality = 0;

        /// <summary>
        /// Nộ khí
        /// </summary>
        [ProtoMember(62)]
        public int Rage = 0;

        [ProtoMember(63)]
        public int Energy = 0;

        [ProtoMember(64)]
        public UserMiniData userMiniData;

        /// <summary>
        /// TaskID lưu lại việc nạp thẻ
        /// </summary>
        [ProtoMember(65)]
        public int CZTaskID = 0;

        /// <summary>
        /// ID gia tộc
        /// </summary>
        [ProtoMember(66)]
        public int FamilyID = 0;

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        [ProtoMember(67)]
        public string FamilyName = "";

        /// <summary>
        /// Hạng của ban thân trong gia tộc
        /// </summary>
        [ProtoMember(68)]
        public int FamilyRank = 0;

        /// <summary>
        /// ID Bang hội
        /// </summary>
        [ProtoMember(69)]
        public int GuildID { get; set; } = 0;

        /// <summary>
        /// Tên bang hội
        /// </summary>
        [ProtoMember(70)]
        public string GuildName = "";

        /// <summary>
        /// Xếp hạng rank
        /// </summary>
        [ProtoMember(71)]
        public int GuildRank { get; set; } = 0;


        /// <summary>
        /// Tài sản cá nhân trong bang
        /// </summary>
        [ProtoMember(72)]
        public int RoleGuildMoney { get; set; } = 0;

        /// <summary>
        /// Uy Danh Giang Hồ
        /// </summary>
        [ProtoMember(73)]
        public int Prestige = 0;

        /// <summary>
        ///Quan hàm của nhân vật
        /// </summary>
        [ProtoMember(74)]
        public int OfficeRank = 0;
    }
}