using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Entities;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng quy định thời gian xuất hiện của quái vật
    /// </summary>
    public class BirthTimePoint
    {
        /// <summary>
        /// Giờ
        /// </summary>
        public int BirthHour = 0;

        /// <summary>
        /// Phút
        /// </summary>
        public int BirthMinute = 0;
    };

    /// <summary>
    /// Thời gian xuất hiện của quái vật theo ngày trong tuâng
    /// </summary>
    public class BirthTimeForDayOfWeek
    {
        /// <summary>
        /// Thời gian xuất hiện
        /// </summary>
        public BirthTimePoint BirthTime;

        /// <summary>
        /// Ngày trong tuần
        /// </summary>
        public int BirthDayOfWeek = 0;
    };

    /// <summary>
    /// Dữ liệu quái vật
    /// </summary>
    public class MonsterStaticInfo
    {
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// ID khu
        /// </summary>
        public int ExtensionID { get; set; }

        /// <summary>
        /// Cấp độ
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Danh hiệu quái
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Kinh nghiệm có được khi giết
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Sinh lực tối đa
        /// </summary>
        public int MaxHP { get; set; }

        /// <summary>
        /// Phạm vi hoạt động (Đuổi mục tiêu)
        /// </summary>
        public int SeekRange { get; set; }

        /// <summary>
        /// Ngũ hành
        /// </summary>
        public KE_SERIES_TYPE Elemental { get; set; }

        /// <summary>
        /// Tốc độ di chuyển
        /// </summary>
        public int MoveSpeed { get; set; }

        /// <summary>
        /// Tốc đánh
        /// </summary>
        public int AtkSpeed { get; set; }

        /// <summary>
        /// Cờ PK
        /// </summary>
        public int Camp { get; set; }

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        public int AIID { get; set; }

        /// <summary>
        /// ID Script Lua điều khiển
        /// </summary>
        public int ScriptID { get; set; }

        /// <summary>
        /// Loại quái
        /// </summary>
        public MonsterAIType MonsterType { get; set; }

        /// <summary>
        /// Danh sách kỹ năng vòng sáng
        /// </summary>
        public string Auras { get; set; }

        /// <summary>
        /// Danh sách kỹ năng được sử dụng
        /// </summary>
        public string Skills { get; set; }
    }

    /// <summary>
    /// Quản lý điểm xuất hiện quái vật
    /// </summary>
    public class MonsterZone
    {
        #region Thuộc tính cơ bản

        /// <summary>
        /// ID bản đồ
        /// </summary>
        public int MapCode
        {
            get;
            set;
        }

        /// <summary>
        /// ID khu
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// ID quái trong khu
        /// </summary>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// Vị trí X
        /// </summary>
        public int ToX
        {
            get;
            set;
        }

        /// <summary>
        /// Vị trí Y
        /// </summary>
        public int ToY
        {
            get;
            set;
        }

        /// <summary>
        /// Phạm vi xuất hiện xung quanh
        /// </summary>
        public int Radius
        {
            get;
            set;
        }

        /// <summary>
        /// Tổng số quái
        /// </summary>
        public int TotalNum
        {
            get;
            set;
        }

        /// <summary>
        /// Thời gian xuất hiện quái
        /// </summary>
        public int Timeslot
        {
            get;
            set;
        }

        /// <summary>
        /// Khoảng cách đuổi mục tiêu tối đa
        /// </summary>
        public int PursuitRadius
        {
            get;
            set;
        }

        /// <summary>
        /// Loại sản sinh quái, 0: Sản sinh theo thời gian, 1: 按照时间点爆怪, 2: Hệ thống điều khiển，người dùng chủ động gọi, 3 用户主动召唤区域，和2的功能一致，但比
        /// 2的功能丰富，2主要用于实现之前的生肖宝典刷怪，而3召唤出来的怪都是一次性的，可由玩家控制或者不控制的系统零时怪
        /// </summary>
        public int BirthType
        {
            get;
            set;
        }

        /// <summary>
        /// Loại sản sinh quái vật (0-1)
        /// </summary>
        public int ConfigBirthType
        {
            get;
            set;
        }

        /// <summary>
        /// Sau bao nhiêu ngày từ lúc mở máy chủ thì bắt đầu sinh quái vật
        /// <para>Nhỏ hơn 0: Ngay khi mở máy chủ</para>
        /// <para>Lớn hơn 0: Số ngày tiếp theo đến thời điểm sinh quái vật</para>
        /// </summary>
        public int SpawnMonstersAfterKaiFuDays
        {
            get;
            set;
        }

        /// <summary>
        /// Số ngày sinh quái
        /// <para>Nhỏ hơn 0: Sinh liên tục</para>
        /// <para>Lớn hơn 0: SỐ ngày đếm ngược đến lần sinh tiếp theo</para>
        /// </summary>
        public int SpawnMonstersDays
        {
            get;
            set;
        }

        /// <summary>
        /// Ngày trong tuần sinh quái vật
        /// <para>Nếu để trống tức là liên tục</para>
        /// </summary>
        public List<BirthTimeForDayOfWeek> SpawnMonstersDayOfWeek
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách các thời điểm sản sinh quái
        /// </summary>
        public List<BirthTimePoint> BirthTimePointList
        {
            get;
            set;
        }

        /// <summary>
        /// Tỷ lệ sinh quái (tối đa 100)
        /// </summary>
        public int BirthRate
        {
            get;
            set;
        }

        /// <summary>
        /// Đã giết toàn bộ quái trong khu vực này chưa.
        /// <para>Sử dụng khi loại sản sinh quái là 4</para>
        /// </summary>
        private bool HasSystemKilledAllOfThisZone = false;

        /// <summary>
        /// Có phải phụ bản không
        /// </summary>
        public bool IsFuBenMap { get; set; } = false;

        /// <summary>
        /// Loại quái
        /// </summary>
        public MonsterTypes MonsterType = MonsterTypes.Normal;

        /// <summary>
        /// Thời điểm phục sinh cuối của khu vực
        /// </summary>
        private long LastReloadTicks = 0;

        /// <summary>
        /// Thời điểm tiêu diệt quái cuối của khu vực
        /// </summary>
        private long LastDestroyTicks = 0;

        /// <summary>
        /// Ngày tạo quái cuối
        /// </summary>
        private int LastBirthDayID = -1;

        /// <summary>
        /// Thời điểm sinh quái cuối
        /// </summary>
        private BirthTimePoint LastBirthTimePoint = null;

        /// <summary>
        /// ID điểm sinh quái cuối
        /// </summary>
        private int LastBirthTimePointIndex = -1;

        #endregion Thuộc tính cơ bản

        #region Thông tin quái

        /// <summary>
        /// Dữ liệu quái
        /// </summary>
        private MonsterStaticInfo MonsterInfo = new MonsterStaticInfo();

        /// <summary>
        /// Trả về dữ liệu quái
        /// </summary>
        public MonsterStaticInfo GetMonsterInfo()
        {
            return this.MonsterInfo;
        }

        #endregion Thông tin quái

        #region Danh sách quái

        /// <summary>
        /// Danh sách quái
        /// </summary>
        private List<Monster> MonsterList = new List<Monster>(100);

        #endregion Danh sách quái

        #region Quản lý phụ bản

        /// <summary>
        /// Quái vật trong phụ bản
        /// </summary>
        private Monster SeedMonster = null;

        #endregion Quản lý phụ bản

        #region Tải quái

        /// <summary>
        /// Tải dữ liệu quái vật
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="monsterZone"></param>
        /// <param name="monsterInfo"></param>
        /// <param name="roleID"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="coordinate"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="aiType"></param>
        private void LoadMonster(Monster monster, MonsterZone monsterZone, MonsterStaticInfo monsterInfo, int roleID, string name, string title, int hp, Point coordinate, int moveSpeed, MonsterAIType aiType)
        {
            monster.RoleName = monsterInfo.Name;

            monster.Title = title;
            monster.FormatSystemName = name;
            monster.MonsterZoneNode = monsterZone;
            monster.MonsterInfo = monsterInfo;
            monster.RoleID = roleID;
            monster.m_CurrentLife = hp;
            monster.ChangeLifeMax(hp, 0, 0);
            monster.CurrentPos = coordinate;
            monster.ChangeRunSpeed(moveSpeed, 0, 0);
            monster.MonsterType = aiType;
            monster.m_Level = monsterInfo.Level;
            monster.Camp = monsterInfo.Camp;
            monster.ChangeAttackSpeed(monsterInfo.AtkSpeed, 0);
            monster.ChangeCastSpeed(monsterInfo.AtkSpeed, 0);
            monster.ChangeAttackRating(MonsterDeadHelper.GetARLevel(monsterInfo.Level), 0, 0);
            monster.ChangeDefend(MonsterDeadHelper.GetDefenseLevel(monsterInfo.Level), 0, 0);
            monster.SetPhysicsDamage(MonsterDeadHelper.GetMinDamgePerLevel(monsterInfo.Level), MonsterDeadHelper.GetMaxDamgePerLevel(monsterInfo.Level));
            monster.ScriptID = monsterInfo.ScriptID;
            monster.AIID = monsterInfo.AIID;
            monster.SeekRange = monsterInfo.SeekRange;

            monster.m_CurrentLifeReplenish = MonsterDeadHelper.GetLifeReplenishLevel(monsterInfo.Level);

            /// Ngũ hành
            if (this.MonsterInfo.Elemental <= KE_SERIES_TYPE.series_none || this.MonsterInfo.Elemental >= KE_SERIES_TYPE.series_num)
            {
                int Value = KTGlobal.GetRandomNumber((int)KE_SERIES_TYPE.series_none + 1, (int)KE_SERIES_TYPE.series_num - 1);
                monster.m_Series = (KE_SERIES_TYPE)Value;
            }
            else
            {
                monster.m_Series = this.MonsterInfo.Elemental;
            }

            monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_physics, monster.m_Series));
            monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_cold, monster.m_Series));
            monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_fire, monster.m_Series));
            monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_light, monster.m_Series));
            monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_poison, monster.m_Series));

            monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_physics, monster.m_Series));
            monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_cold, monster.m_Series));
            monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_fire, monster.m_Series));
            monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_light, monster.m_Series));
            monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetDamagePerLevel(monsterInfo.Level, DAMAGE_TYPE.damage_poison, monster.m_Series));

            monster.SetResist(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetResistPerLevel(monsterInfo.Level));
            monster.SetResist(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetResistPerLevel(monsterInfo.Level));
            monster.SetResist(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetResistPerLevel(monsterInfo.Level));
            monster.SetResist(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetResistPerLevel(monsterInfo.Level));
            monster.SetResist(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetResistPerLevel(monsterInfo.Level));
        }

        /// <summary>
        /// Copy dữ liệu quái vật
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private Monster CopyMonster(Monster oldMonster)
        {
            Monster monster = oldMonster.Clone();
            return monster;
        }

        /// <summary>
        /// Hủy quái vật
        /// <para>Khi hủy quái vật, tham chiếu sẽ bị xóa bỏ, đồng thời cũng giải phóng bộ nhớ</para>
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="isAutoRemove"></param>
        public void DestroyMonster(Monster monster)
        {
            lock (this.MonsterList)
            {
                /// Xóa khỏi danh sách quái của khu vực
                this.MonsterList.Remove(monster);
            }

            /// Giảm số lượng quái
            Monster.DecMonsterCount();

            /// Xóa khỏi bản đồ
            GameManager.MapGridMgr.DictGrids[MapCode].RemoveObject(monster);

            /// Xóa khỏi đối tượng quản lý quái
            GameManager.MonsterMgr.RemoveMonster(monster);

            /// Trả lại ID quái cho hệ thống để tái sử dụng
            GameManager.MonsterIDMgr.PushBack(monster.RoleID);

            /// Hủy đối tượng
            monster.Dispose();
        }

        /// <summary>
        /// Đọc dữ liệu quái từ file Monster trong bản đồ
        /// </summary>
        /// <param name="monsterXml"></param>
        /// <returns></returns>
        private Monster InitMonster(XElement monsterXml)
        {
            /// Lấy dữ liệu bản đồ tương ứng
            GameMap gameMap = GameManager.MapMgr.DictMaps[MapCode];

            /// Tạo mới quái
            Monster monster = new Monster();

            /// Cập nhật ID cho quái
            int roleID = (int)GameManager.MonsterIDMgr.GetNewID(MapCode);
            monster.UniqueID = Global.GetUniqueID();

            /// Tải xuống dữ liệu quái
            this.LoadMonster(
                    monster,
                    this,
                    this.MonsterInfo,
                    roleID,
                    string.Format("Role_{0}", roleID),
                    monsterXml.Attribute("Title").Value,
                    int.Parse(monsterXml.Attribute("MaxHP").Value),
                    Global.GetMapPointByGridXY(ObjectTypes.OT_MONSTER, this.MapCode, this.ToX, this.ToY, this.Radius, 0, true),
                    this.MonsterInfo.MoveSpeed,
                    (MonsterAIType)int.Parse(monsterXml.Attribute("MonsterType").Value)
                );

            return monster;
        }

        /// <summary>
        /// Kiểm tra với tỷ lệ tương ứng có thể tái sinh quái không
        /// </summary>
        private bool CanRealiveByRate()
        {
            if (BirthRate >= 10000)
            {
                return true;
            }

            int randNum = Global.GetRandomNumber(1, 10001);
            if (randNum <= BirthRate)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tải thông tin cấu hình quái
        /// </summary>
        public void LoadStaticMonsterInfo()
        {
            string fileName = string.Format("Config/KT_Monster/Monsters.xml");

            XElement xml = GameManager.MonsterZoneMgr.AllMonstersXml;
            if (xml == null)
            {
                throw new Exception(string.Format("Can not load file: {0}, file not found!", fileName));
            }

            XElement monsterXml = Global.GetSafeXElement(xml, "Monster", "ID", this.Code.ToString());
            this.MonsterInfo = GameManager.MonsterMgr.GetMonsterStaticInfo(monsterXml);
        }

        /// <summary>
        /// Tải danh sách quái
        /// </summary>
        public void LoadMonsters()
        {
            string fileName = string.Format("Config/KT_Monster/Monsters.xml");

            XElement xml = GameManager.MonsterZoneMgr.AllMonstersXml;
            if (xml == null)
            {
                throw new Exception(string.Format("Can not load file: {0}, file not found!", fileName));
            }

            XElement monsterXml = Global.GetSafeXElement(xml, "Monster", "ID", Code.ToString());

            /// Thêm quái vào danh sách
            MonsterNameManager.AddMonsterName(Code, Global.GetSafeAttributeStr(monsterXml, "Name"));

            /// Tạo đối tượng quái
            Monster monster = null;

            /// Nếu không phải phụ bản
            if (!this.IsFuBenMap)
            {
                for (int i = 0; i < this.TotalNum; i++)
                {
                    /// Phân tích dữ liệu quái
                    monster = this.InitMonster(monsterXml);
                    this.MonsterType = (MonsterTypes)monster.MonsterType;

                    /// Thêm quái vào danh sách
                    this.MonsterList.Add(monster);

                    /// Thêm quái vào đối tượng quản lý
                    GameManager.MonsterMgr.AddMonster(monster);
                }
            }
            /// Nếu là phụ bản
            else
            {
                /// Phân tích dữ liệu quái
                monster = InitMonster(monsterXml);
                this.MonsterType = (MonsterTypes)monster.MonsterType;

                this.SeedMonster = monster;
            }
        }

        /// <summary>
        /// Tải thông tin tĩnh của quái tạm thời
        /// </summary>
        /// <returns></returns>
        public Monster LoadDynamicMonsterSeed()
        {
            string fileName = string.Format("Config/KT_Monster/Monsters.xml");
            XElement xml = null;

            try
            {
                xml = XElement.Load(Global.GameResPath(fileName));
                if (null == xml)
                {
                    throw new Exception(string.Format("Can not load file: {0}, file not found!", fileName));
                }
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Can not load file: {0}, file not found!", fileName));
            }

            XElement monsterXml = Global.GetSafeXElement(xml, "Monster", "ID", Code.ToString());

            /// Thêm vào danh sách quản lý
            MonsterNameManager.AddMonsterName(Code, Global.GetSafeAttributeStr(monsterXml, "Name"));

            /// Tải dữ liệu
            return InitMonster(monsterXml);
        }

        #endregion Tải quái

        #region Quản lý tái sinh

        /// <summary>
        /// Trả về thời điểm tái sinh tiếp theo
        /// </summary>
        /// <returns></returns>
        public string GetNextBirthTimePoint()
        {
            if (BirthType == (int)MonsterBirthTypes.CreateDayOfWeek && SpawnMonstersDayOfWeek != null)
            {
                DateTime nowTime = TimeUtil.NowDateTime();
                int dayOfWeek = 0;
                int birthHour = 0;
                int birthMinite = 0;

                int nextIndex = -1;
                if (LastBirthTimePointIndex >= 0)
                {
                    nextIndex = (LastBirthTimePointIndex + 1) % SpawnMonstersDayOfWeek.Count;
                }
                else
                {
                    DateTime now = TimeUtil.NowDateTime();
                    int time1 = (int)now.DayOfWeek * 1440 + now.Hour * 60 + now.Minute;

                    for (int i = 0; i < SpawnMonstersDayOfWeek.Count; i++)
                    {
                        int time2 = SpawnMonstersDayOfWeek[i].BirthDayOfWeek * 1440 + SpawnMonstersDayOfWeek[i].BirthTime.BirthHour * 60 + SpawnMonstersDayOfWeek[i].BirthTime.BirthMinute;
                        if (time1 <= time2)
                        {
                            nextIndex = i;
                            break;
                        }
                    }
                    if (nextIndex < 0)
                    {
                        nextIndex = 0;
                    }
                }

                dayOfWeek = (int)SpawnMonstersDayOfWeek[nextIndex].BirthDayOfWeek;
                birthHour = SpawnMonstersDayOfWeek[nextIndex].BirthTime.BirthHour;
                birthMinite = SpawnMonstersDayOfWeek[nextIndex].BirthTime.BirthMinute;

                return string.Format("{0}${1}${2}", birthHour, birthMinite, dayOfWeek);
            }
            else
            {
                if (null == BirthTimePointList)
                {
                    return "";
                }

                //原子操作
                int lastBirthTimePointIndex = LastBirthTimePointIndex;

                //上次爆怪物的时间点的索引
                int nextIndex = 0;
                if (lastBirthTimePointIndex >= 0)
                {
                    nextIndex = (lastBirthTimePointIndex + 1) % BirthTimePointList.Count;
                }
                else
                {
                    DateTime now = TimeUtil.NowDateTime();
                    int time2 = now.Hour * 60 + now.Minute;
                    for (int i = 0; i < BirthTimePointList.Count; i++)
                    {
                        int time1 = BirthTimePointList[i].BirthHour * 60 + BirthTimePointList[i].BirthMinute;
                        if (time2 <= time1)
                        {
                            nextIndex = i;
                            break;
                        }
                    }
                }

                return string.Format("{0}${1}", BirthTimePointList[nextIndex].BirthHour, BirthTimePointList[nextIndex].BirthMinute);
            }
        }

        /// <summary>
        /// Có thể tái sinh ở thời điểm tương ứng
        /// </summary>
        /// <param name="birthTimePoint"></param>
        /// <returns></returns>
        private bool CanBirthOnTimePoint(DateTime now, BirthTimePoint birthTimePoint)
        {
            if (now.DayOfYear == LastBirthDayID) //如果天数相同
            {
                if (null != LastBirthTimePoint)
                {
                    if (LastBirthTimePoint.BirthHour == birthTimePoint.BirthHour &&
                            LastBirthTimePoint.BirthMinute == birthTimePoint.BirthMinute)
                    {
                        return false; //说明今天的这个时间点已经爆过怪物了
                    }
                }
            }

            if (now.Hour != birthTimePoint.BirthHour)
            {
                return false;
            }

            int minMinute = birthTimePoint.BirthMinute;
            int maxMinute = birthTimePoint.BirthMinute + 1;
            return (now.Minute >= minMinute && now.Minute <= maxMinute);
        }

        /// <summary>
        /// Có thể tái sinh ở thời điểm tương ứng trong ngày của tuần
        /// </summary>
        /// <param name="birthTimePoint"></param>
        /// <returns></returns>
        private bool CanBirthOnTimePointForWeekOfDay(DateTime now, BirthTimePoint birthTimePoint)
        {
            if (now.DayOfYear == LastBirthDayID) //如果天数相同
            {
                if (null != LastBirthTimePoint)
                {
                    if (LastBirthTimePoint.BirthHour == birthTimePoint.BirthHour &&
                            LastBirthTimePoint.BirthMinute == birthTimePoint.BirthMinute)
                    {
                        return false; //说明今天的这个时间点已经爆过怪物了
                    }
                }
            }

            if (now.Hour != birthTimePoint.BirthHour)
            {
                return false;
            }

            int minMinute = birthTimePoint.BirthMinute;
            int maxMinute = birthTimePoint.BirthMinute + 1;
            return (now.Minute >= minMinute && now.Minute <= maxMinute);
        }

        public void ReloadMonsters(SocketListener sl, TCPOutPacketPool pool)
        {
            if (IsFuBenMap)
            {
                return;
            }

            DateTime now = TimeUtil.NowDateTime();

            if (!CanTodayReloadMonsters() || !CanTodayReloadMonstersForDayOfWeek())
            {
                if (!HasSystemKilledAllOfThisZone)
                {
                    //SystemKillAllMonstersOfThisZone();
                    HasSystemKilledAllOfThisZone = true;
                }
                return;
            }

            HasSystemKilledAllOfThisZone = false;

            // code == -1的动态区域，有异常
            if (Code > 0 && ConfigBirthType == (int)MonsterBirthTypes.AfterJieRiDays)
            {
                try
                {
                    LoadStaticMonsterInfo();
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Error, "reload jieri boss monster failed", ex);
                }
            }

            if ((int)MonsterBirthTypes.TimeSpan == BirthType)
            {
                long ticks = now.Ticks;
                if (LastReloadTicks <= 0 || ticks - LastReloadTicks >= (1 * 1000 * 10000))
                {
                    LastReloadTicks = ticks;

                    MonsterRealive(sl, pool);
                }
            }
            else if ((int)MonsterBirthTypes.TimePoint == BirthType)
            {
                if (null != BirthTimePointList)
                {
                    int nextIndex = 0;
                    if (LastBirthTimePointIndex >= 0)
                    {
                        nextIndex = (LastBirthTimePointIndex + 1) % BirthTimePointList.Count;
                    }
                    else
                    {
                        for (int i = 0; i < BirthTimePointList.Count; i++)
                        {
                            if (CanBirthOnTimePoint(now, BirthTimePointList[i]))
                            {
                                nextIndex = i;
                                break;
                            }
                        }
                    }

                    BirthTimePoint birthTimePoint = BirthTimePointList[nextIndex];

                    if (CanBirthOnTimePoint(now, birthTimePoint))
                    {
                        LastBirthTimePointIndex = nextIndex;

                        LastBirthTimePoint = birthTimePoint;

                        LastBirthDayID = TimeUtil.NowDateTime().DayOfYear;

                        MonsterRealive(sl, pool);
                    }
                }
            }
            else if ((int)MonsterBirthTypes.CreateDayOfWeek == BirthType)
            {
                if (SpawnMonstersDayOfWeek == null)
                    return;

                DateTime nowTime = TimeUtil.NowDateTime();
                DayOfWeek nDayOfWeek = nowTime.DayOfWeek;

                for (int i = 0; i < SpawnMonstersDayOfWeek.Count; ++i)
                {
                    int nDay = SpawnMonstersDayOfWeek[i].BirthDayOfWeek;

                    if (nDay == (int)nDayOfWeek)
                    {
                        BirthTimePoint time = SpawnMonstersDayOfWeek[i].BirthTime;

                        //判断是否能够爆怪
                        if (CanBirthOnTimePoint(now, time))
                        {
                            //记住这次爆怪的索引
                            LastBirthTimePointIndex = i;

                            //记住这次爆怪的时间点
                            LastBirthTimePoint = time;

                            //上次爆怪物的天ID
                            LastBirthDayID = TimeUtil.NowDateTime().DayOfYear;

                            //重新刷新怪
                            MonsterRealive(sl, pool);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断现在是否能刷怪，主要针对原始刷怪类型为4的配置进行开服后多少天的刷怪控制
        /// 如果不能刷怪，则外部需要系统强行杀死所有本区域的怪
        /// </summary>
        /// <returns></returns>
        public Boolean CanTodayReloadMonsters()
        {
            if (SpawnMonstersAfterKaiFuDays <= 0 && SpawnMonstersDays <= 0)
            {
                return true;
            }

            DateTime kaifuTime = Global.GetKaiFuTime();
            if (ConfigBirthType == (int)MonsterBirthTypes.AfterHeFuDays)
            {
                // 检查是否开启了该活动
                HeFuActivityConfig config = HuodongCachingMgr.GetHeFuActivityConfing();
                if (null == config)
                    return false;
                if (!config.InList((int)ActivityTypes.HeFuBossAttack))
                    return false;

                kaifuTime = Global.GetHefuStartDay();
            }
            else if (ConfigBirthType == (int)MonsterBirthTypes.AfterJieRiDays)
            {
                // 检查是否开启了该活动
                JieriActivityConfig config = HuodongCachingMgr.GetJieriActivityConfig();
                if (null == config)
                    return false;
                if (!config.InList((int)ActivityTypes.JieriBossAttack))
                    return false;

                kaifuTime = Global.GetJieriStartDay();
            }

            DateTime now = TimeUtil.NowDateTime();
            int days2Kaifu = Global.GetDaysSpanNum(now, kaifuTime) + 1;

            if (SpawnMonstersAfterKaiFuDays <= 0 || days2Kaifu >= SpawnMonstersAfterKaiFuDays)
            {
                if (SpawnMonstersDays <= 0 || days2Kaifu < SpawnMonstersDays + SpawnMonstersAfterKaiFuDays)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断现在是否能刷怪 针对MU新增的一周哪天能刷怪 [1/10/2014 LiaoWei]
        /// </summary>
        /// <returns></returns>
        public bool CanTodayReloadMonstersForDayOfWeek()
        {
            if (SpawnMonstersDayOfWeek == null)
                return true;

            if (ConfigBirthType != (int)MonsterBirthTypes.CreateDayOfWeek) // 周几刷怪[1/10/2014 LiaoWei]
                return true;

            DateTime now = TimeUtil.NowDateTime();
            DayOfWeek nDayOfWeek = now.DayOfWeek;

            for (int i = 0; i < SpawnMonstersDayOfWeek.Count; ++i)
            {
                int nDay = SpawnMonstersDayOfWeek[i].BirthDayOfWeek;

                if (nDay == (int)nDayOfWeek)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Cập nhật lại tọa độ quái
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        private void RepositionMonster(Monster monster, int toX, int toY)
        {
            GameManager.MapGridMgr.DictGrids[MapCode].MoveObject(-1, -1, toX, toY, monster);
        }

        /// <summary>
        /// Hàm này thực hiện việc hồi sinh quái đã chehets
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void MonsterRealive(SocketListener sl, TCPOutPacketPool pool, int copyMapID = -1, int birthCount = 65535)
        {
            int haveBirthCount = 0;
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (null == MonsterList[i])
                {
                    continue;
                }
                /// Nếu đã tạo quá số lượng ban đầu
                if (haveBirthCount >= birthCount)
                {
                    break;
                }

                if (-1 != copyMapID)
                {
                    if (MonsterList[i].CurrentCopyMapID != copyMapID)
                    {
                        continue;
                    }
                }

                /// Nếu quái không còn sống
                if (!MonsterList[i].Alive)
                {
                    if (((int)MonsterBirthTypes.TimeSpan == BirthType || (int)MonsterBirthTypes.CopyMapLike == BirthType) && Timeslot > 0) //如果是按照时间段来刷怪的这里要进行判断 // 增加副本复活时间 [3/3/2014 LiaoWei]
                    {
                        long monsterRealiveTimeslot = ((long)Timeslot * 1000L * 10000L);
                        if (TimeUtil.NOW() * 10000 - MonsterList[i].LastDeadTicks < monsterRealiveTimeslot)
                        {
                            continue;
                        }
                    }

                    /// Nếu loại tái sinh là theo thời điểm hoặc theo ngày trong tuần
                    if (BirthType == (int)MonsterBirthTypes.TimePoint || BirthType == (int)MonsterBirthTypes.CreateDayOfWeek)
                    {
                        // Nếu là boss gọi tới con quản lý bosss
                        if (MonsterList[i].MonsterType == MonsterAIType.Boss)
                        {
                            // Nếu ko được phép mọc thì bỏ qua
                            //if (!EliteMonsterManager.getInstance().BossRespan(this.MapCode, MonsterList[i]))
                            {
                                continue;
                            }
                        }
                    }

                    /// Nếu đã đến thời điểm tái sinh
                    if (this.CanRealiveByRate())
                    {
                        /// Vị trí điểm hồi sinh
                        Point pt = MonsterList[i].Relive();

                        /// Thiết lập vị trí của quái
                        this.RepositionMonster(MonsterList[i], (int)pt.X, (int)pt.Y);

                        /// Lấy danh sách người chơi xung quanh
                        List<Object> listObjs = Global.GetAll9Clients(MonsterList[i]);
                        /// Gửi gói tin thông báo cho tất cả Client xung quanh quái vật tại sinh
                        GameManager.ClientMgr.NotifyMonsterRelive(sl, pool, MonsterList[i], MapCode, MonsterList[i].CurrentCopyMapID, MonsterList[i].RoleID, (int)MonsterList[i].CurrentPos.X, (int)MonsterList[i].CurrentPos.Y, (int)MonsterList[i].CurrentDir, (int)MonsterList[i].m_Series, MonsterList[i].GetMonsterData().HP, listObjs);

                        haveBirthCount++;

                        if (MonsterList[i].MonsterType == 0)
                        {
                            int ZoneID = this.MapCode * this.ID;

                            if (this.TotalNum >= 10)
                            {
                                EliteMonsterManager.getInstance().AddDieRecore(ZoneID, MonsterList[i]);
                            }
                        }
                        /// Thực hiện thao tác sống lại
                        MonsterList[i].DoRelive();
                    }
                }
            }
        }

        #endregion Quản lý tái sinh

        #region Phụ bản

        /// <summary>
        /// Xóa quái vật đã chết
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        public void DestroyDeadMonsters()
        {
            if (BirthType == (int)MonsterBirthTypes.CopyMapLike)
            {
                return;
            }

            long ticks = TimeUtil.NOW() * 10000;
            long monsterDestroyTimeslot = (30 * 1000 * 10000);
            if (ticks - LastDestroyTicks < monsterDestroyTimeslot)
            {
                return;
            }

            LastDestroyTicks = ticks;

            List<Monster> monsterList = new List<Monster>();
            bool bExistNull = false;
            for (int i = 0; i < this.MonsterList.Count; i++)
            {
                if (null == this.MonsterList[i])
                {
                    bExistNull = true;
                    continue;
                }
                if (!this.MonsterList[i].Alive)
                {
                    monsterList.Add(this.MonsterList[i]);
                }
            }

            for (int i = 0; i < monsterList.Count; i++)
            {
                this.DestroyMonster(monsterList[i]);
            }

            if (bExistNull)
            {
                this.MonsterList.RemoveAll((x) => { return null == x; });
            }
        }

        #region Quái thường

        /// <summary>
        /// XÓa quái thường đã chết
        /// </summary>
        public void DestroyDeadDynamicMonsters()
        {
            if (IsDynamicZone())
            {
                DestroyDeadMonsters();
            }
        }

        /// <summary>
        /// /// Kiểm tra có phải khu vực quái động không
        /// </summary>
        /// <returns></returns>
        public bool IsDynamicZone()
        {
            return (int)MonsterBirthTypes.CrossMap == BirthType;
        }

        /// <summary>
        /// Tải quái vật tạm thời từ thông tin khu tương ứng
        /// </summary>
        public void LoadDynamicMonsters(MonsterZoneQueueItem monsterZoneQueueItem)
        {
            if (!IsDynamicZone())
            {
                return;
            }

            if (null == monsterZoneQueueItem || null == monsterZoneQueueItem.SeedMonster)
            {
                return;
            }

            for (int i = 0; i < monsterZoneQueueItem.BirthCount; i++)
            {
                Monster monster = this.CopyMonster(monsterZoneQueueItem.SeedMonster);
                int roleID = (int)GameManager.MonsterIDMgr.GetNewID(MapCode);
                monster.RoleID = roleID;
                monster.UniqueID = Global.GetUniqueID();
                monster.FormatSystemName = string.Format("Role_{0}", roleID);
                monster.CurrentCopyMapID = monsterZoneQueueItem.CopyMapID;
                monster.MonsterType = monsterZoneQueueItem.MonsterType;

                /// Nếu có thiết lập Camp
                if (monsterZoneQueueItem.Camp != -1)
                {
                    monster.Camp = monsterZoneQueueItem.Camp;
                }

                /// Nếu có thiết lập cấp độ
                if (monsterZoneQueueItem.Level != -1)
                {
                    monster.m_Level = monsterZoneQueueItem.Level;
                }
                /// Nếu có thiết lập mức sinh lực
                if (monsterZoneQueueItem.HP != -1)
                {
                    monster.ResetLife();
                    monster.ChangeLifeMax(monsterZoneQueueItem.HP, 0, 0);
                    monster.m_CurrentLife = monster.m_CurrentLifeMax;
                }

                /// Nếu có thiết lập tên
                if (!string.IsNullOrEmpty(monsterZoneQueueItem.Name))
                {
                    monster.RoleName = monsterZoneQueueItem.Name;
                }
                /// Nếu có thiết lập danh hiệu
                if (!string.IsNullOrEmpty(monsterZoneQueueItem.Title))
                {
                    monster.Title = monsterZoneQueueItem.Title;
                }
                /// Nếu có thiết lập hướng
                if (monsterZoneQueueItem.Dir != KiemThe.Entities.Direction.NONE)
                {
                    monster.CurrentDir = monsterZoneQueueItem.Dir;
                }
                else
                {
                    monster.CurrentDir = (KiemThe.Entities.Direction)KTGlobal.GetRandomNumber((int)KiemThe.Entities.Direction.DOWN, (int)KiemThe.Entities.Direction.Count - 1);
                }
                /// Nếu có thiết lập ngũ hành
                if (monsterZoneQueueItem.Series != KE_SERIES_TYPE.series_none)
                {
                    monster.m_Series = monsterZoneQueueItem.Series;
                }
                monster.MonsterType = monsterZoneQueueItem.MonsterType;
                monster.ScriptID = monsterZoneQueueItem.ScriptID;

                /// Nếu có thiết lập AIScript
                if (monsterZoneQueueItem.AIID != -1)
                {
                    monster.AIID = monsterZoneQueueItem.AIID;
                }
                monster.Tag = monsterZoneQueueItem.Tag;

                // SET MÁU ME CÁC THỨ

                monster.ChangeAttackRating(MonsterDeadHelper.GetARLevel(monsterZoneQueueItem.Level), 0, 0);
                monster.ChangeDefend(MonsterDeadHelper.GetDefenseLevel(monsterZoneQueueItem.Level), 0, 0);
                monster.SetPhysicsDamage(MonsterDeadHelper.GetMinDamgePerLevel(monsterZoneQueueItem.Level), MonsterDeadHelper.GetMaxDamgePerLevel(monsterZoneQueueItem.Level));

                monster.m_CurrentLifeReplenish = MonsterDeadHelper.GetLifeReplenishLevel(monsterZoneQueueItem.Level);

                monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_physics, monster.m_Series));
                monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_cold, monster.m_Series));
                monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_fire, monster.m_Series));
                monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_light, monster.m_Series));
                monster.SetSeriesDamagePhysics(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_poison, monster.m_Series));

                monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_physics, monster.m_Series));
                monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_cold, monster.m_Series));
                monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_fire, monster.m_Series));
                monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_light, monster.m_Series));
                monster.AddSeriesDamageMagics(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetDamagePerLevel(monsterZoneQueueItem.Level, DAMAGE_TYPE.damage_poison, monster.m_Series));

                monster.SetResist(DAMAGE_TYPE.damage_cold, MonsterDeadHelper.GetResistPerLevel(monsterZoneQueueItem.Level));
                monster.SetResist(DAMAGE_TYPE.damage_fire, MonsterDeadHelper.GetResistPerLevel(monsterZoneQueueItem.Level));
                monster.SetResist(DAMAGE_TYPE.damage_light, MonsterDeadHelper.GetResistPerLevel(monsterZoneQueueItem.Level));
                monster.SetResist(DAMAGE_TYPE.damage_poison, MonsterDeadHelper.GetResistPerLevel(monsterZoneQueueItem.Level));
                monster.SetResist(DAMAGE_TYPE.damage_physics, MonsterDeadHelper.GetResistPerLevel(monsterZoneQueueItem.Level));

                /// Khu vực quản lý
                monster.MonsterZoneNode = this;

                /// Tọa độ hiện tại
                monster.CurrentPos = new Point(monsterZoneQueueItem.ToX, monsterZoneQueueItem.ToY);

                /// Đánh dấu vị trí ban đầu
                monster.InitializePos = monster.CurrentPos;
                monster.StartPos = monster.StartPos;

                /// Nếu có tái sinh sau khi chết
                if (monsterZoneQueueItem.RespawnTick != -1)
                {
                    monster.OnDieCallback = (killer) =>
                    {
                        GameManager.MonsterZoneMgr.AddMonsterToRespawn(monsterZoneQueueItem);

                        // nếu như con này có chế độ gọi callback thì callback phát cho đỡ buồn
                        monsterZoneQueueItem.OnDieCallback?.Invoke(killer);
                    };
                }
                else
                {
                    // Nếu khi con quái này chết mà có hàm callback
                    if (monsterZoneQueueItem.OnDieCallback != null)
                    {
                        monster.OnDieCallback = (killer) =>
                        {
                            // Gọi hàm toạch khi monster chết
                            monsterZoneQueueItem.OnDieCallback?.Invoke(killer);
                        };
                    }
                    else
                    {
                        monster.OnDieCallback = null;
                    }
                }

                /// Thêm quái vào danh sách quái hiện tại của khu
                this.MonsterList.Add(monster);

                /// Thêm quái vào hệ thống quản lý quái
                GameManager.MonsterMgr.AddMonster(monster);

                /// Gọi hàm khởi động
                monster.Awake();

                /// Lưới bản đồ đang đứng
                GameManager.MapGridMgr.DictGrids.TryGetValue(MapCode, out MapGrid mapGrid);
                /// Cập nhật vị trí đối tượng vào Map
                if (mapGrid != null)
                {

                    mapGrid.MoveObject(-1, -1, (int)monster.CurrentPos.X, (int)monster.CurrentPos.Y, monster);
                }

                /// Thực hiện Callback khi hoàn tất
                monsterZoneQueueItem.Done?.Invoke(monster);
            }
        }

        #endregion Quái thường

        #endregion Phụ bản
    }
}