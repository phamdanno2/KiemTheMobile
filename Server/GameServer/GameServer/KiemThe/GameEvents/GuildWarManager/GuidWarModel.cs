using GameServer.Logic;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GameServer.KiemThe.GameEvents.GuildWarManager
{


    /// <summary>
    /// Config các bản đồ sẽ mửo
    /// </summary>
    ///
    [XmlRoot(ElementName = "OpenConfig")]
    public class OpenConfig
    {
        [XmlAttribute(AttributeName = "MapID")]
        public int MapID { get; set; }

        [XmlAttribute(AttributeName = "MapName")]
        public string MapName { get; set; }

        [XmlAttribute(AttributeName = "FightMapID")]
        public int FightMapID { get; set; }

        [XmlAttribute(AttributeName = "Star")]
        public int Star { get; set; }

        /// <summary>
        /// Tọa độ đặt của NPC,Quái,Long Trụ
        /// </summary>
        ///
        [XmlElement(ElementName = "ObjectPostion")]
        public List<ObjectivePostion> ObjectPostion { get; set; }

        [XmlIgnore]
        public int ReadMapID
        {

            get
            {
                if (this.MapID == 1 || this.MapID == 2 || this.MapID == 7 || this.MapID == 6 || this.MapID == 3 || this.MapID == 4 || this.MapID == 5 || this.MapID == 8)
                {
                    return this.FightMapID;
                }
                else
                {
                    return this.MapID;
                }

            }

        }


        [XmlIgnore]
        public bool IsFightMapID
        {
            get
            {
                if (this.MapID == 1 || this.MapID == 2 || this.MapID == 7 || this.MapID == 6 || this.MapID == 3 || this.MapID == 4 || this.MapID == 5 || this.MapID == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

    }


    [XmlRoot(ElementName = "WardConfigAward")]
    public class WardConfigAward
    {
        [XmlAttribute(AttributeName = "Point")]
        public int Point { get; set; }
        [XmlAttribute(AttributeName = "Rank")]
        public int Rank { get; set; }
        [XmlAttribute(AttributeName = "BoxCount")]
        public int BoxCount { get; set; }
        [XmlAttribute(AttributeName = "GuildPoint")]
        public int GuildPoint { get; set; }
        [XmlAttribute(AttributeName = "GuildMoney")]
        public int GuildMoney { get; set; }


    }

    [XmlRoot(ElementName = "WarAcitivity")]
    public class WarAcitivity
    {
        /// <summary>
        /// Thời gian đăng ký
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "SIGN_UP_DULATION")]
        public int SIGN_UP_DULATION { get; set; }

        /// <summary>
        /// Thời gian hỗn chiến
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "WAR_DULATION")]
        public int WAR_DULATION { get; set; }

        /// <summary>
        /// Thời gian nghỉ hỗn chiến
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "WAR_STOP")]
        public int WAR_STOP { get; set; }

        /// <summary>
        /// Thời gian nghỉ PVP nhặt cờ
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "WAR_CLEAR")]
        public int WAR_CLEAR { get; set; }


    }
    /// <summary>
    /// Tọa độ đặt
    /// </summary>
    ///
    [XmlRoot(ElementName = "ObjectivePostion")]
    public class ObjectivePostion
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public int ID { get; set; }
        [XmlAttribute(AttributeName = "Posx")]
        public int Posx { get; set; }
        [XmlAttribute(AttributeName = "PosY")]
        public int PosY { get; set; }

        [XmlAttribute(AttributeName = "IsMonster")]
        public bool IsMonster { get; set; }

    }

    [XmlRoot(ElementName = "TimeBattle")]
    public class TimeBattle
    {
        [XmlAttribute(AttributeName = "Hours")]
        public int Hours { get; set; }

        [XmlAttribute(AttributeName = "Minute")]
        public int Minute { get; set; }
    }


    /// <summary>
    /// Config tổng tranh đoạt lãnh thổ
    /// </summary>
    public class GuildWardConfig
    {
        // Ngày nào sẽ diễn ra sự kiện
        [XmlElement(ElementName = "DayOfWeek")]
        public List<int> DayOfWeek { get; set; }


        // Giờ nào sẽ mở sự kiện
        [XmlElement(ElementName = "OpenTime")]
        public TimeBattle OpenTime { get; set; }


        /// <summary>
        /// Hoạt động thoe thời gian cảu activity
        /// </summary>
        [XmlElement(ElementName = "Activity")]
        public WarAcitivity Activity { get; set; }

        /// <summary>
        /// Tổng số bản đồ sẽ mở tranh đoạt lãnh thổ
        /// </summary>
        ///
        [XmlElement(ElementName = "WarMapConfigs")]
        public List<OpenConfig> WarMapConfigs = new List<OpenConfig>();

        /// <summary>
        /// Danh sách các bản đồ liền kề sẽ config để tìm ra các bản đồ automatic war
        /// </summary>
        ///
        [XmlElement(ElementName = "NearMapConfig")]
        public List<GuildMapConfig> NearMapConfig { get; set; }

        /// <summary>
        /// Danh sách phần quà có thể nhận
        /// </summary>

        [XmlElement(ElementName = "Award")]
        public List<WardConfigAward> Award { get; set; }
    }





    [XmlRoot(ElementName = "GuildMapConfig")]
    public class GuildMapConfig
    {
        [XmlAttribute(AttributeName = "MapId")]
        public int MapId { get; set; }

        [XmlAttribute(AttributeName = "MapName")]
        public string MapName { get; set; }

        [XmlAttribute(AttributeName = "MapType")]
        public string MapType { get; set; }

        [XmlAttribute(AttributeName = "FightMapId")]
        public int FightMapId { get; set; }

        [XmlAttribute(AttributeName = "Star")]
        public int Star { get; set; }

        [XmlElement(ElementName = "NearMap")]
        public List<int> NearMap { get; set; }


    }


    /// <summary>
    /// Đối tượng người chơi ở trong map tranh đoạn
    /// </summary>
    public class GuildWarPlayer
    {
        public KPlayer _Player { get; set; }

        public int GuildID { get; set; }

        public int Score { get; set; }

        public int DestroyCount { get; set; }

        public int KillCount { get; set; }

        public int CurentRank { get; set; }

        public int MaxKillSteak { get; set; }

        public int CurentKillSteak { get; set; }


        public bool IsReviceReward { get; set; }

    }

    /// <summary>
    /// Danh sách các bản đồ đã đăng ký tuyên chiến
    /// </summary>
    public class GuildMapRegisterFight
    {
        /// <summary>
        /// Id của bang hội
        /// </summary>
        public int GuildID { get; set; }

        /// <summary>
        /// Tên bang hội đăng ký
        /// </summary>
        public string GuildName { get; set; }

        /// <summary>
        /// Bản đồ đã tuyên chiến
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Tên bản đồ
        /// </summary>
        public string MapName { get; set; }

    }




    public class GuildInfo
    {
        public int GuildID { get; set; }

        public string GuildName { get; set; }


    }







    /// <summary>
    /// Thực thể bang
    /// </summary>
    public class GuildTotalScore
    {
        public int Score { get; set; }
        public int GuildID { get; set; }
        public string GuildName { get; set; }

        public long LastArlet { get; set; }


        public int TmpRank { get; set; }
    }



    public class FightResult
    {
        public int MapID { get; set; }

        public string MapName { get; set; }

        /// <summary>
        /// Đây là kiểu
        /// 0 : Chiếm được
        /// 1 : Bảo vệ được
        /// 2 : Để mất
        /// 3 : Hệ thống thu hồi lại
        /// </summary>
        public int Type { get; set; }


        public bool IsFightMap { get; set; }


        public int OldGuildID { get; set; }

        public int NewGuildID { get; set; }
    }




    public enum GUILDWARDSTATUS
    {
        // Không phải thời gian diễn ra sự kiện
        STATUS_NULL = 0,
        // Tuyên Chiến
        STATUS_REGISTER = 1,

        STATUS_PREPARSTART = 2,
        // Chinh chiến
        STATUS_START = 3,
        // Ngưng chiến
        STATUS_PREPAREEND = 4,
        // Kết quả
        STATUS_END = 5,
        //Rest lại chiến trường
        STATUS_CLEAR = 6,
    }

}
