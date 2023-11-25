﻿using GameServer.Core.Executor;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace GameServer.KiemThe.GameEvents.GuildWarManager
{
    /// <summary>
    /// Class sẽ quản lý toàn bộ việc chiến đấu của ward
    /// </summary>
    public class GuidWarManager
    {
        #region GuidWarManagerDef

        private static GuidWarManager instance = new GuidWarManager();

        /// <summary>
        /// Config của
        /// </summary>
        public GuildWardConfig _Config = new GuildWardConfig();

        /// <summary>
        /// Thời gian tick gần đây nhất
        /// </summary>
        public long LastTick { get; set; }

        public long LastProtectTick { get; set; }

        public long LastUpdateMiniMap { get; set; }

        public long LastUpdateScore { get; set; }

        /// <summary>
        /// Thời gian notify gần đây nhất
        /// </summary>
        public long LastNofity { get; set; }

        public bool IsLoadingMiniMap { get; set; }

        public long LastSpamCamp { get; set; }

        public GUILDWARDSTATUS BATTLESTATE { get; set; }

        public List<GuildWarMiniMap> _CaheMiniMap = new List<GuildWarMiniMap>();
        /// <summary>
        /// Danh sách các bản đồ đã được tuyên chiến từ NPC
        /// </summary>

        public List<GuildMapRegisterFight> TotalMapRegisterFight = new List<GuildMapRegisterFight>();

        /// <summary>
        /// Danh sách bản đồ sẽ chạy sự kiện ward
        /// </>
        public ConcurrentDictionary<int, GuildWarMap> TotalMapWillBeWar = new ConcurrentDictionary<int, GuildWarMap>();

        // Cần 1 cái j đó để lưu điểm tích lũy của từng thằng ở đây
        /// <summary>
        ///  Chứa toàn bộ điểm của đợt công thành chiến của các người chơi được tham gia war
        /// </summary>
        public ConcurrentDictionary<int, ConcurrentDictionary<int, GuildWarPlayer>> WarScore = new ConcurrentDictionary<int, ConcurrentDictionary<int, GuildWarPlayer>>();

        // Danh sách điểm số của GUILD update liên tục
        public ConcurrentDictionary<int, List<TerritoryReport>> TotalReportUpdate = new ConcurrentDictionary<int, List<TerritoryReport>>();

        // Tạo 1 danh sách để lưu điểm của các bang lại
        public ConcurrentDictionary<int, List<GuildTotalScore>> TotalScore = new ConcurrentDictionary<int, List<GuildTotalScore>>();

        /// <summary>
        /// Dict chứa kết quả cuối cùng của quộc chiến
        /// </summary>
        public ConcurrentDictionary<int, List<FightResult>> FinalResult = new ConcurrentDictionary<int, List<FightResult>>();

        public BlockingCollection<string> TotalColorDict = new BlockingCollection<string>();

        //Bgwork do work
        public BackgroundWorker worker = new BackgroundWorker();

        public ConcurrentDictionary<int, string> ColorSelect = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Toàn bộ thông tin Guild Info
        /// </summary>
        public ConcurrentDictionary<int, string> TotalGuildInfo = new ConcurrentDictionary<int, string>();

        // CÁI NÀY KEY CÓ LẼ LÊN LÀ BANG

        /// <summary>
        /// Đường dẫn tới files confgi
        /// </summary>
        public string _GuildMapConfig = "Config/KT_Battle/GuildWarConfig.xml";

        /// <summary>
        /// Hàm Instance()
        /// </summary>
        /// <returns></returns>
        public static GuidWarManager getInstance()
        {
            return instance;
        }

        #endregion GuidWarManagerDef

        /// <summary>
        /// Đếm số thành viên đã tuyên chiến bản đồ này
        /// </summary>
        /// <param name="MapID"></param>
        /// <returns></returns>
        public int CountRegisterFight(int MapID)
        {
            int Count = TotalMapRegisterFight.Where(x => x.MapId == MapID).Count();

            return Count;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            LastUpdateMiniMap = TimeUtil.NOW();
            IsLoadingMiniMap = false;

            //Thiết lập 1 BGWWROK MỚI
            worker = new BackgroundWorker();
            worker.DoWork += Event_Prosecc;
            worker.RunWorkerCompleted += Evemt_Complete;

            string Files = Global.GameResPath(_GuildMapConfig);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(GuildWardConfig));

                _Config = serializer.Deserialize(stream) as GuildWardConfig;
            }

            // Khởi tạo timer tính toán khởi chạy cho sự kiện cứ 2 giấy tick 1 lần delay 5s
            ScheduleExecutor2.Instance.scheduleExecute(new NormalScheduleTask("GuldWarManager", ProsecBattle), 5 * 1000, 2000);

            //Setup Dict Màu trước
            TotalColorDict.Add("#f44336");
            TotalColorDict.Add("#744700");
            TotalColorDict.Add("#ce7e00");
            TotalColorDict.Add("#8fce00");
            TotalColorDict.Add("#2986cc");
            TotalColorDict.Add("#16537e");
            TotalColorDict.Add("#6a329f");
            TotalColorDict.Add("#c90076");
            TotalColorDict.Add("#4c1130");
            TotalColorDict.Add("#f70067");
            TotalColorDict.Add("#0a0a0a");
            TotalColorDict.Add("#a405fa");

            TotalColorDict.Add("#19212e");
            TotalColorDict.Add("#911744");
            TotalColorDict.Add("#115c1e");
        }

        private void Evemt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG:" + e.Error.ToString());
            }
        }

        private void Event_Prosecc(object sender, DoWorkEventArgs e)
        {
            try
            {
                ///NẾu đang không có battle nào diễn ra
                if (BATTLESTATE == GUILDWARDSTATUS.STATUS_NULL)
                {
                    if (TimeUtil.NOW() - LastUpdateMiniMap >= 300000 && IsLoadingMiniMap == false)
                    {
                        IsLoadingMiniMap = true;

                        //Call SETUP MINIMAP
                        this.SetupMiniMap();
                    }

                    List<int> DayOfWeek = _Config.DayOfWeek;

                    int Today = TimeUtil.GetWeekDay1To7(DateTime.Now);

                    //Check xem hôm này có phỉa là ngày diễn ra sự kiện
                    if (DayOfWeek.Contains(Today))
                    {
                        // Nếu là ngày diễn ra sự kiện
                        DateTime Now = DateTime.Now;

                        if (Now.Hour == _Config.OpenTime.Hours && Now.Minute == _Config.OpenTime.Minute && BATTLESTATE == GUILDWARDSTATUS.STATUS_NULL)
                        {
                            LastTick = TimeUtil.NOW();

                            // Chuyển sang trạng thái cho phép tuyên chiến
                            BATTLESTATE = GUILDWARDSTATUS.STATUS_REGISTER;

                            LogManager.WriteLog(LogTypes.GuildWarManager, "Battle Change State ==> " + BATTLESTATE.ToString());

                            KTGlobal.SendSystemEventNotification("Lãnh thổ chiến đang bước vào thời kỳ tuyên chiến, hãy đến Quan Lãnh Thổ các thành để xác định mục tiêu chinh chiến!");
                        }
                    }
                } // Nếu là thời gian đăng ký thì làm gì đó
                else if (BATTLESTATE == GUILDWARDSTATUS.STATUS_REGISTER)
                {
                    if (TimeUtil.NOW() >= LastTick + ((_Config.Activity.SIGN_UP_DULATION * 1000) - (5 * 60 * 1000)) && BATTLESTATE == GUILDWARDSTATUS.STATUS_REGISTER)
                    {
                        BATTLESTATE = GUILDWARDSTATUS.STATUS_PREPARSTART;

                        KTGlobal.SendSystemEventNotification("Chỉ còn 5 phút nữa tranh đoạt lãnh thổ sẽ bắt đầu Các Bang Hội hãy mau chóng tới NPC Quan Lãnh Thổ để tuyên chiến!");

                        LogManager.WriteLog(LogTypes.GuildWarManager, " Battle Change State ==> " + BATTLESTATE.ToString());
                    }
                } // Start sự kiện tranh đoạt lãnh thổ
                else if (BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPARSTART)
                {
                    if (TimeUtil.NOW() >= LastTick + (_Config.Activity.SIGN_UP_DULATION * 1000) && BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPARSTART)
                    {
                        LastTick = TimeUtil.NOW();

                        // Khởi tọa toàn bộ các bản đồ có war
                        BATTLESTATE = GUILDWARDSTATUS.STATUS_START;

                        KTGlobal.SendSystemEventNotification("Lãnh thổ chiến đã bước vào thời kỳ Chinh Chiến,các Bang Hội hãy mau di chuyển tới địa điểm chiến đấu!");

                        // Khởi tạo toàn bộ bản đồ war
                        this.CreateAllActiveWarMap();

                        this.CreateMapEvent();

                        this.FillMoreInfoToMiniMap();

                        LogManager.WriteLog(LogTypes.GuildWarManager, " Battle Change State ==> " + BATTLESTATE.ToString());
                        // CREATE TOÀN BỘ BẢN ĐỒ CHIẾN
                    }
                }
                else if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND) // Nếu chiến trường đang bắt dầu
                {
                    if (TimeUtil.NOW() - LastNofity >= 7 * 1000 && (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND))
                    {
                        LastNofity = TimeUtil.NOW();

                        // Gửi thông báo mỗi 5s đã sửa lại
                        NotifyToAllEvery5Secon();
                        // Thực hiện udpate guild ranking đã sửa lại
                        UpdateGuildRanking();
                    }

                    if (TimeUtil.NOW() - LastUpdateScore >= 60 * 1000 && (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND))
                    {
                        LastUpdateScore = TimeUtil.NOW();

                        // Cứ mỗi 60s sẽ cộng điểm cho bang đó tùy vào số trụ chiểm được
                        UpdateScoreForGuildEvery60SEC();
                    }

                    // Cứ 30s spam 1 lần camp
                    if (TimeUtil.NOW() - LastProtectTick >= 30 * 1000 && (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND))
                    {
                        LastProtectTick = TimeUtil.NOW();

                        this.SpamCamp();
                        // Cứ mỗi 30s sẽ cộng điểm bảo vệ cho người chơi và EXP
                        UpdateExpEvery30Sec();
                    }

                    if (TimeUtil.NOW() >= LastTick + ((_Config.Activity.WAR_DULATION * 1000) - (5 * 60 * 1000)) && BATTLESTATE == GUILDWARDSTATUS.STATUS_START)
                    {
                        KTGlobal.SendSystemEventNotification("Chỉ còn 5 phút nữa tranh đoạt sẽ kết thúc,các Bang hãy mau chóng đẩy sập long trụ!");

                        BATTLESTATE = GUILDWARDSTATUS.STATUS_PREPAREEND;

                        LogManager.WriteLog(LogTypes.GuildWarManager, " Battle Change State ==> " + BATTLESTATE.ToString());
                    }

                    if (TimeUtil.NOW() >= LastTick + (_Config.Activity.WAR_DULATION * 1000) && BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
                    {
                        LastTick = TimeUtil.NOW();

                        KTGlobal.SendSystemEventNotification("Tranh đoạt lãnh thổ đã tới hồi kết thúc! Hãy tới Quan Lãnh Thổ để nhận thưởng");

                        // Chuyển sang trạng thái end
                        BATTLESTATE = GUILDWARDSTATUS.STATUS_END;

                        //Gọi update bảng xếp hạng lần cuối cùng
                        UpdateGuildRanking();
                        // Thiết lập lãnh thổ đọc ghi DB

                        // Clear toàn bộ cột trụ quá , boss,
                        // Unlock status cho toàn bộ thành viên tham gia
                        ResetBattle();

                        SetupFinishTerritory();

                        // Thực hiện ghi vào DB
                        WriterToDatabase();

                        //Reload lại toàn bộ thông tin lãnh thổ
                        ReloadAllTerritoryInfo();

                        //todo : Làm thêm quả xếp hạng ở NPC khi nó ấn vào phần thưởng

                        LogManager.WriteLog(LogTypes.GuildWarManager, " Battle Change State ==> " + BATTLESTATE.ToString());

                        _CaheMiniMap = new List<GuildWarMiniMap>();

                        SetupMiniMap();
                    }

                    if (TimeUtil.NOW() >= LastTick + (_Config.Activity.WAR_CLEAR * 1000) && BATTLESTATE == GUILDWARDSTATUS.STATUS_END)
                    {
                        LastTick = TimeUtil.NOW();

                        BATTLESTATE = GUILDWARDSTATUS.STATUS_CLEAR;

                        LogManager.WriteLog(LogTypes.SongJinBattle, " Battle Change State ==> " + BATTLESTATE.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void SetupMiniMap()
        {
            try
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "SETUP MINI MAP");

                TerritoryInfo _GetAllTeroryInfo = this.GetTotalTerritoryInfo(0);

                if (_GetAllTeroryInfo != null)
                {
                    if (_GetAllTeroryInfo.Territorys != null)
                    {
                        // Lấy ra tất cả các lãnh thổ hiện tại của máy chủ
                        foreach (Territory _Territory in _GetAllTeroryInfo.Territorys)
                        {
                            //Đọc ra bản đồ
                            int MAPID = _Territory.MapID;
                            // Đang thuộc về bang nào
                            int Guild_ID = _Territory.GuildID;
                            // lấy ra bản đồ cơ sở từ DB cái này ko cần sửa
                            var Find = _Config.WarMapConfigs.Where(x => x.MapID == MAPID).FirstOrDefault();

                            GuildWarMiniMap _Demo1 = new GuildWarMiniMap();
                            _Demo1.GuildID = _Territory.GuildID;
                            _Demo1.GuildName = _Territory.GuildName;
                            _Demo1.Tax = Find.Star;
                            _Demo1.MapID = _Territory.MapID;

                            if (Find.IsFightMapID)
                            {
                                _Demo1.MapType = 2;
                            }
                            else
                            {
                                if (_Territory.IsMainCity == 1)
                                {
                                    _Demo1.MapType = 1;
                                }
                                else
                                {
                                    _Demo1.MapType = 0;
                                }
                            }

                            if (ColorSelect.TryGetValue(Guild_ID, out string Color))
                            {
                                _Demo1.HexColor = Color;
                            }
                            else
                            {
                                string _ColorSelct = TotalColorDict.Take();
                                _Demo1.HexColor = _ColorSelct;
                                ColorSelect.TryAdd(Guild_ID, _ColorSelct);
                            }

                            _CaheMiniMap.Add(_Demo1);

                            // Nếu như đang trong thời gian tranh đoạt xảy ra lấy ra tất cả các map liền kề để nhấp nháy
                            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
                            {
                                if (!this.IsTanThuThon(Find.MapID))
                                {
                                    // Lấy ra toàn bộ các bản đồ liền kề với bản đồ này
                                    GuildMapConfig NearMapConfig = _Config.NearMapConfig.Where(x => x.MapId == Find.MapID).FirstOrDefault();

                                    if (NearMapConfig != null)
                                    {
                                        List<int> TotalMapNear = NearMapConfig.NearMap;

                                        foreach (int MapNear in TotalMapNear)
                                        {
                                            var FindMapWar = _Config.WarMapConfigs.Where(x => x.MapID == MapNear).FirstOrDefault();

                                            if (FindMapWar != null)
                                            {
                                                var findExits = _CaheMiniMap.Where(x => x.MapID == MapNear).FirstOrDefault();
                                                if (findExits == null)
                                                {
                                                    _Demo1 = new GuildWarMiniMap();
                                                    _Demo1.GuildID = _Territory.GuildID;
                                                    _Demo1.GuildName = "Bản đồ liền kề" + _Territory.GuildName;
                                                    _Demo1.Tax = FindMapWar.Star;
                                                    _Demo1.MapID = FindMapWar.MapID;
                                                    _Demo1.MapType = 3;

                                                    if (ColorSelect.TryGetValue(Guild_ID, out string ColorPick))
                                                    {
                                                        _Demo1.HexColor = ColorPick;
                                                    }
                                                    else
                                                    {
                                                        string _ColorSelct = TotalColorDict.Take();
                                                        _Demo1.HexColor = _ColorSelct;
                                                        ColorSelect.TryAdd(Guild_ID, _ColorSelct);
                                                    }

                                                    _CaheMiniMap.Add(_Demo1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG GetAllMiniMapInfo :" + ex.ToString());
            }
        }

        /// <summary>
        /// Fill thông tin vào bản đồ mini map chỉ 1 lần duy nhất khi vào game
        /// </summary>
        public void FillMoreInfoToMiniMap()
        {
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                try
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "START FillMoreInfoToMiniMap !");

                    foreach (GuildMapRegisterFight _MapFight in TotalMapRegisterFight.ToList())
                    {
                        // Lãnh thổ nào mà bang mình tuyên chiến
                        //  if (_MapFight.GuildID == client.GuildID)
                        {
                            // Tìm lại  để chắc chắn bản đồ này có nằm trong danh sách sẽ mở tranh đoạt
                            var FindMapWar = _Config.WarMapConfigs.Where(x => x.MapID == _MapFight.MapId).FirstOrDefault();
                            // Nếu như bản đồ này có mở tranh đoạt
                            if (FindMapWar != null)
                            {
                                GuildWarMiniMap _Demo1 = new GuildWarMiniMap();
                                _Demo1.GuildID = _MapFight.GuildID;
                                _Demo1.GuildName = "Tuyên Chiến :" + _MapFight.GuildName;
                                _Demo1.Tax = FindMapWar.Star;
                                _Demo1.MapID = FindMapWar.MapID;
                                _Demo1.MapType = 3;

                                if (ColorSelect.TryGetValue(_MapFight.GuildID, out string Color))
                                {
                                    _Demo1.HexColor = Color;
                                }
                                else
                                {
                                    if (TotalColorDict.Count > 0)
                                    {
                                        string _ColorSelct = TotalColorDict.Take();
                                        _Demo1.HexColor = _ColorSelct;
                                        ColorSelect.TryAdd(_MapFight.GuildID, _ColorSelct);
                                    }
                                    else
                                    {
                                        _Demo1.HexColor = "#115c1e";
                                        ColorSelect.TryAdd(_MapFight.GuildID, "#115c1e");
                                    }
                                }

                                var findExits = _CaheMiniMap.Where(x => x.MapID == FindMapWar.MapID).FirstOrDefault();
                                if (findExits == null)
                                {
                                    _CaheMiniMap.Add(_Demo1);
                                }
                            }
                            else
                            {
                                LogManager.WriteLog(LogTypes.GuildWarManager, "Bản đô [" + _MapFight.MapName + "] không mở tranh đoạt lãnh thổ");
                            }
                        }
                    }

                    LogManager.WriteLog(LogTypes.GuildWarManager, "END FillMoreInfoToMiniMap!");
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Lấy ra toàn bộ thông tin bản đồ mini map
        /// </summary>
        /// <returns></returns>
        public List<GuildWarMiniMap> GetAllMiniMapInfo(KPlayer client)
        {
            return _CaheMiniMap;
        }

        /// <summary>
        /// Tạo toàn bộ ActiveWarMap
        /// </summary>
        public void CreateAllActiveWarMap()
        {
            // Lấy ra toàn bộ bản đồ lãnh thổ của game
            TerritoryInfo _GetAllTeroryInfo = this.GetTotalTerritoryInfo(0);

            LogManager.WriteLog(LogTypes.GuildWarManager, "START CreateAllActiveWarMap");

            try
            {
                if (_GetAllTeroryInfo != null)
                {
                    if (_GetAllTeroryInfo.Territorys != null)
                    {
                        LogManager.WriteLog(LogTypes.GuildWarManager, "CREATE ALL ACTIVE WAR:" + _GetAllTeroryInfo.Territorys.Count);

                        // Lấy ra tất cả các lãnh thổ hiện tại của máy chủ
                        foreach (Territory _Territory in _GetAllTeroryInfo.Territorys)
                        {
                            //Đọc ra bản đồ
                            int MAPID = _Territory.MapID;

                            // Đang thuộc về bang nào
                            int Guild_ID = _Territory.GuildID;

                            // Phải lưu cái thông tin này vào thông tin bang hội để lấy ra sau này cần vì mình ko viết Packet ghi vào DB
                            if (!TotalGuildInfo.ContainsKey(Guild_ID))
                            {
                                TotalGuildInfo.TryAdd(Guild_ID, _Territory.GuildName);
                            }

                            // lấy ra bản đồ cơ sở từ DB cái này ko cần sửa
                            var Find = _Config.WarMapConfigs.Where(x => x.MapID == MAPID).FirstOrDefault();

                            LogManager.WriteLog(LogTypes.GuildWarManager, "TÌM MAP CONFIG CỦA BẢN ĐỒ:" + Find.MapName);

                            if (!TotalMapWillBeWar.ContainsKey(Find.ReadMapID))
                            {   // Adđ luôn bản đồ này vào dánh ách sẽ active war
                                GuildWarMap _WarMap = new GuildWarMap(_Config);

                                _WarMap.MapName = Find.MapName;
                                _WarMap.BeLongGuild = Guild_ID;
                                _WarMap.BeLongGuildName = _Territory.GuildName;
                                _WarMap.MapId = Find.ReadMapID;
                                _WarMap.IsFightMap = Find.IsFightMapID;
                                // Add cho bang này vào danh sách
                                _WarMap.TotalGuildCanJoinBattle.Add(_Territory.GuildID);
                                // Add Bản đồ này vào danh sách sẽ dựng cột lên
                                TotalMapWillBeWar.TryAdd(Find.ReadMapID, _WarMap);
                            }
                            else
                            {
                                if (TotalMapWillBeWar.TryGetValue(Find.ReadMapID, out GuildWarMap _OutValue))
                                {
                                    // Add thằng này vào danh sách có thể tham gia trận chiến
                                    _OutValue.BeLongGuild = Guild_ID;
                                    _OutValue.BeLongGuildName = _Territory.GuildName;
                                    _OutValue.TotalGuildCanJoinBattle.Add(_Territory.GuildID);
                                }
                            }

                            // FIND TIẾP DANH SÁCH LIỀN KỀ
                            // Nếu đây không là là tân thủ thôn thì tìm các bản đồ liền kề

                            if (!this.IsTanThuThon(Find.MapID))
                            {
                                LogManager.WriteLog(LogTypes.GuildWarManager, "Tìm bản đồ liền kề của  bản đồ :" + Find.MapName);

                                // Lấy ra toàn bộ các bản đồ liền kề với bản đồ này
                                GuildMapConfig NearMapConfig = _Config.NearMapConfig.Where(x => x.MapId == Find.MapID).FirstOrDefault();

                                if (NearMapConfig != null)
                                {
                                    List<int> TotalMapNear = NearMapConfig.NearMap;

                                    foreach (int MapNear in TotalMapNear)
                                    {
                                        var FindMapWar = _Config.WarMapConfigs.Where(x => x.MapID == MapNear).FirstOrDefault();

                                        if (FindMapWar != null)
                                        {
                                            if (!TotalMapWillBeWar.ContainsKey(FindMapWar.MapID))
                                            {
                                                GuildWarMap _WarMap = new GuildWarMap(_Config);

                                                _WarMap.IsFightMap = false;
                                                _WarMap.MapId = FindMapWar.MapID;
                                                _WarMap.MapName = FindMapWar.MapName;

                                                // NẾu bang này không có trong danh sách thì thêm vào
                                                if (!_WarMap.TotalGuildCanJoinBattle.Contains(Guild_ID))
                                                {
                                                    _WarMap.TotalGuildCanJoinBattle.Add(Guild_ID);
                                                }

                                                // Add Bản đồ này vào danh sách sẽ dựng cột lên
                                                TotalMapWillBeWar.TryAdd(FindMapWar.MapID, _WarMap);
                                            }
                                            else
                                            {
                                                if (TotalMapWillBeWar.TryGetValue(FindMapWar.MapID, out GuildWarMap _OutValue))
                                                {
                                                    if (!_OutValue.TotalGuildCanJoinBattle.Contains(Guild_ID))
                                                    {
                                                        // Thêm bang này vào danh sách có thể tham gia war
                                                        _OutValue.TotalGuildCanJoinBattle.Add(Guild_ID);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogManager.WriteLog(LogTypes.GuildWarManager, "CANT FILD MAP CONFIG :" + MapNear);
                                        }
                                    }
                                }
                                else
                                {
                                    LogManager.WriteLog(LogTypes.GuildWarManager, "Không tìm thấy bản đồ liền kề của bản đồ :" + Find.MapName);
                                }
                            }
                        }
                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "END CreateAllActiveWarMap");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG CreateAllActiveWarMap:" + ex.ToString());
            }
            // STEP 2 FILL NỐT các bản đồ đã đăng ký vào bản đồ chiến

            foreach (GuildMapRegisterFight _MapFight in TotalMapRegisterFight)
            {
                // Tìm lại  để chắc chắn bản đồ này có nằm trong danh sách sẽ mở tranh đoạt
                var FindMapWar = _Config.WarMapConfigs.Where(x => x.MapID == _MapFight.MapId).FirstOrDefault();

                if (!TotalGuildInfo.ContainsKey(_MapFight.GuildID))
                {
                    TotalGuildInfo.TryAdd(_MapFight.GuildID, _MapFight.GuildName);
                }

                // Nếu như bản đồ này có mở tranh đoạt
                if (FindMapWar != null)
                {
                    // Nếu bản đồ này chưa từng có trong danh sách ward

                    if (!TotalMapWillBeWar.ContainsKey(FindMapWar.ReadMapID))
                    {
                        // Tạo mới 1 bản đồ nếu mà chưa tồn tại
                        GuildWarMap _WarMap = new GuildWarMap(_Config);

                        // Nếu là tân thủ thôn
                        _WarMap.MapId = FindMapWar.ReadMapID;
                        _WarMap.IsFightMap = FindMapWar.IsFightMapID;
                        _WarMap.MapName = FindMapWar.MapName;

                        // Add cho bang này vào danh sách
                        if (!_WarMap.TotalGuildCanJoinBattle.Contains(_MapFight.GuildID))
                        {
                            _WarMap.TotalGuildCanJoinBattle.Add(_MapFight.GuildID);
                        }

                        // Add Bản đồ này vào danh sách sẽ dựng cột lên
                        TotalMapWillBeWar.TryAdd(FindMapWar.ReadMapID, _WarMap);
                    }
                    else
                    {
                        // NẾu mà bản đồ này có nằm trong danh sách các bản đồ sẽ khởi tạo war
                        if (TotalMapWillBeWar.TryGetValue(FindMapWar.ReadMapID, out GuildWarMap _OutValue))
                        {
                            if (!_OutValue.TotalGuildCanJoinBattle.Contains(_MapFight.GuildID))
                            {
                                _OutValue.TotalGuildCanJoinBattle.Add(_MapFight.GuildID);
                            }
                        }
                    }
                }
                else
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "Bản đô [" + _MapFight.MapName + "] không mở tranh đoạt lãnh thổ");
                }
            }
        }

        #region ScoreDef

        /// <summary>
        /// Cứ mỗi 60s mỗi trụ sẽ cộng cho bang nào đang chiếm giữ 50 điểm
        /// </summary>
        public void UpdateScoreForGuildEvery60SEC()
        {
            try
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "UpdateScoreForGuildEvery60SEC");

                foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
                {
                    GuildWarMap MapEvent = entry.Value;

                    List<int> GuildBeWar = MapEvent.TotalGuildCanJoinBattle;

                    foreach (int GuildID in GuildBeWar)
                    {
                        int TotalLongTru = MapEvent.CountGuildObjective(GuildID);

                        if (TotalLongTru > 0)
                        {
                            int FINALPOINT = TotalLongTru * 5;

                            if (TotalGuildInfo.TryGetValue(GuildID, out string GUILDNAME))
                            {
                                this.UpdateGuildPoint(GuildID, GUILDNAME, FINALPOINT, MapEvent.MapId, MapEvent.MapName);
                            }
                            //ĐOẠN NÀY CẦN XEM LẠI
                        }
                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "UpdateScoreForGuildEvery60SEC END");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void UpdateExpEvery30Sec()
        {
            try
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "UpdateExpEvery30Sec");

                foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
                {
                    GuildWarMap MapEvent = entry.Value;
                    MapEvent.UpdateEXP();
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "UpdateExpEvery30Sec END");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void UpdateGuildRanking()
        {
            ConcurrentDictionary<int, List<TerritoryReport>> TmpReport = new ConcurrentDictionary<int, List<TerritoryReport>>();

            try
            {
                // Duyệt toàn bộ điểm các của GUILD

                LogManager.WriteLog(LogTypes.GuildWarManager, "DO UpdateGuildRanking");

                foreach (KeyValuePair<int, List<GuildTotalScore>> entry in TotalScore)
                {
                    int MapID = entry.Key;

                    var Find = _Config.WarMapConfigs.Where(x => x.ReadMapID == MapID).FirstOrDefault();

                    List<GuildTotalScore> TotalGuildScore = entry.Value;

                    foreach (GuildTotalScore _Recore in TotalGuildScore)
                    {
                        TmpReport.TryGetValue(_Recore.GuildID, out List<TerritoryReport> _OutValue);

                        if (_OutValue != null)
                        {
                            TerritoryReport _Report = new TerritoryReport();
                            _Report.MapName = Find.MapName;
                            _Report.TotalPoint = _Recore.Score;
                            _Report.Rank = _Recore.TmpRank;

                            _OutValue.Add(_Report);
                        }
                        else
                        {
                            List<TerritoryReport> _NewListValue = new List<TerritoryReport>();

                            TerritoryReport _Report = new TerritoryReport();
                            _Report.MapName = Find.MapName;
                            _Report.TotalPoint = _Recore.Score;
                            _Report.Rank = _Recore.TmpRank;

                            _NewListValue.Add(_Report);

                            TmpReport.TryAdd(_Recore.GuildID, _NewListValue);
                        }
                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "END UpdateGuildRanking");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }

            this.TotalReportUpdate = TmpReport;
            // Sau khi duyệt xong bắt đầu xếp hạng
        }

        public void UpdateGuildPoint(int GuildID, string GuildName, int Score, int MapID, string MapName)
        {
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                // Nếu như có cái bản đồ này trong danh sách rồi thì
                if (TotalScore.TryGetValue(MapID, out List<GuildTotalScore> _GuildValue))
                {
                    // Lấy ra thằng có điểm cao nhất
                    int TopGuild = -1;
                    var Find = _GuildValue.OrderByDescending(x => x.Score).FirstOrDefault();

                    if (Find != null)
                    {
                        TopGuild = Find.GuildID;
                    }

                    // Nếu như tìm thấy nó trong danh sách
                    var find = _GuildValue.Where(x => x.GuildID == GuildID).FirstOrDefault();
                    if (find != null)
                    {
                        find.Score = find.Score + Score;
                        int RankSoft = _GuildValue.OrderByDescending(x => x.Score).ToList().FindIndex(x => x.GuildID == GuildID);
                        find.TmpRank = RankSoft + 1;
                    }
                    else
                    {
                        GuildTotalScore _Score = new GuildTotalScore();
                        _Score.GuildID = GuildID;
                        _Score.GuildName = GuildName;
                        _Score.Score = Score;

                        _GuildValue.Add(_Score);

                        int RankSoft = _GuildValue.OrderByDescending(x => x.Score).ToList().FindIndex(x => x.GuildID == GuildID);
                        _Score.TmpRank = RankSoft + 1;
                    }

                    if (TopGuild != -1)
                    {
                        var FindAfter = _GuildValue.OrderByDescending(x => x.Score).FirstOrDefault();

                        if (FindAfter != null)
                        {
                            if (TopGuild != Find.GuildID)
                            {
                                long Now = TimeUtil.NOW();

                                string MSG = "Chiến báo : Bang ta đã dành được lợi thế ở <b>" + KTGlobal.CreateStringByColor(MapName, ColorType.Importal) + "</b>";

                                string MSGNeft = "Chiến báo : Bang ta đã bị thất thế tại  <b>" + KTGlobal.CreateStringByColor(MapName, ColorType.Importal) + "</b>";

                                // Nếu đã qua 1 phút mà chưa thông báo gì
                                if (Now - Find.LastArlet > 60000)
                                {
                                    Find.LastArlet = Now;
                                    // Thông báo cho bang bị mất lợi thế
                                    KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, Find.GuildID, MSGNeft, null, "");
                                }

                                if (Now - FindAfter.LastArlet > 60000)
                                {
                                    FindAfter.LastArlet = Now;
                                    // Thông báo cho bang đang có lợi thế
                                    KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, Find.GuildID, MSG, null, "");
                                }
                            }
                        }
                    }

                    // TODO THỰC HIỆN SOFT LẠI CHO NÓ NHẸ
                }
                else
                {
                    List<GuildTotalScore> _NewGuildValue = new List<GuildTotalScore>();

                    GuildTotalScore _Score = new GuildTotalScore();
                    _Score.GuildID = GuildID;
                    _Score.GuildName = GuildName;
                    _Score.Score = Score;
                    _Score.TmpRank = 1;

                    _NewGuildValue.Add(_Score);

                    // add vào trong map
                    TotalScore.TryAdd(MapID, _NewGuildValue);
                }
            }
        }

        // Xử lý điểm khi người chơi giết người chơi
        public void OnDie(GameObject Kill, GameObject BeKill)
        {
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                try
                {
                    if (Kill is KPlayer && BeKill is KPlayer)
                    {
                        KPlayer kPlayer_Kill = (KPlayer)Kill;

                        KPlayer kPlayer_BeKill = (KPlayer)BeKill;

                        int CurentBeKilLScore = 0;

                        // Lấy ra danh sách các thành viên trong bang hội
                        if (WarScore.TryGetValue(kPlayer_BeKill.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _OutList))
                        {
                            _OutList.TryGetValue(kPlayer_BeKill.RoleID, out GuildWarPlayer _PlayerOut);

                            if (_PlayerOut != null)
                            {
                                CurentBeKilLScore = _PlayerOut.Score;
                            }
                        }

                        int MaxScoreCanGet = GetPointCanGet(CurentBeKilLScore);

                        UpdateScore(kPlayer_Kill, MaxScoreCanGet, 0, 1, false);
                        UpdateScore(kPlayer_BeKill, 0, 0, 0, true);

                        if (kPlayer_Kill.TeamID != -1)
                        {
                            int MaxScoreCanGetByTeam = GetPointCanGetByTeam(CurentBeKilLScore);

                            List<KPlayer> TotalMember = kPlayer_Kill.Teammates;

                            foreach (KPlayer member in TotalMember)
                            {
                                // Nếu như không nằm trong bán kính thì không cho ăn ké
                                if (!Global.InCircle(new Point(member.PosX, member.PosY), kPlayer_Kill.CurrentPos, 1000))
                                {
                                    continue;
                                }

                                if (member.RoleID != kPlayer_BeKill.RoleID)
                                {
                                    //Cho ăn ké của đồng  đội
                                    UpdateScore(member, MaxScoreCanGetByTeam, 0, 1, false);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "OnDie TOÁC :" + ex.ToString());
                }
            }
        }

        public void SendKillStreak(KPlayer InputPlayer, int Count)
        {
            G2C_KillStreak _State = new G2C_KillStreak();

            _State.KillNumber = Count;

            if (InputPlayer.IsOnline())
            {
                InputPlayer.SendPacket<G2C_KillStreak>((int)TCPGameServerCmds.CMD_KT_KILLSTREAK, _State);
            }
        }

        public int GetPointCanGetByTeam(int CurentScore)
        {
            int Point = 0;

            if (CurentScore >= 0 && CurentScore < 2000)
            {
                Point = 1;
            }
            else if (CurentScore >= 2000 && CurentScore < 6000)
            {
                Point = 2;
            }
            else if (CurentScore >= 6000 && CurentScore < 8000)
            {
                Point = 7;
            }
            else if (CurentScore >= 8000 && CurentScore < 10000)
            {
                Point = 15;
            }
            else if (CurentScore >= 10000)
            {
                Point = 30;
            }

            return Point;
        }

        public int GetPointCanGet(int CurentScore)
        {
            int Point = 0;

            if (CurentScore >= 0 && CurentScore < 2000)
            {
                Point = 37;
            }
            else if (CurentScore >= 2000 && CurentScore < 6000)
            {
                Point = 75;
            }
            else if (CurentScore >= 6000 && CurentScore < 8000)
            {
                Point = 100;
            }
            else if (CurentScore >= 8000 && CurentScore < 10000)
            {
                Point = 150;
            }
            else if (CurentScore >= 10000)
            {
                Point = 300;
            }

            return Point;
        }

        public string GetRankTitleByPoint(int CurentScore)
        {
            string Title = "";

            if (CurentScore >= 0 && CurentScore < 2000)
            {
                Title = "<color=#ffffff>Binh sĩ</color>";
            }
            else if (CurentScore >= 2000 && CurentScore < 6000)
            {
                Title = "<color=#109de8>Hiệu úy</color>";
            }
            else if (CurentScore >= 6000 && CurentScore < 8000)
            {
                Title = "<color=#b268c4>Thống lĩnh</color>";
            }
            else if (CurentScore >= 8000 && CurentScore < 10000)
            {
                Title = "<color=#fac241>Phó tướng</color>";
            }
            else if (CurentScore >= 10000)
            {
                Title = "<color=#f2fa00>Đại Tướng</color>";
            }

            return Title;
        }

        public void UpdateScore(KPlayer _Player, int Score, int DesotryCount, int KillCount, bool IsRessetStreak)
        {
            int GuildID = _Player.GuildID;

            try
            {
                Console.WriteLine("Update SCORE :" + _Player.RoleName + "| POINT :" + Score);
                // Chắn chắn thằng này phải có trong danh sách mới được tính điểm
                if (WarScore.TryGetValue(_Player.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _OutList))
                {
                    if (_OutList.TryGetValue(_Player.RoleID, out GuildWarPlayer find))
                    {
                        if (find != null)
                        {
                            // Cộng điểm cho thằng người chơi này
                            find.Score = find.Score + Score;
                            // Nếu như có dấu hiệu phá hủy
                            if (DesotryCount > 0)
                            {
                                // Cộng tích lũy công trình
                                find.DestroyCount = find.DestroyCount + DesotryCount;
                            }
                            // Nếu như có ghi nhận giết người
                            if (KillCount > 0)
                            {
                                // Cập số người đã giết
                                find.KillCount = find.KillCount + KillCount;
                            }
                            // nếu bị reset Steak
                            if (IsRessetStreak)
                            {
                                // Thì thực hiện reset lại streak
                                find.CurentKillSteak = 0;
                            }
                            else
                            {
                                find.CurentKillSteak = find.CurentKillSteak + KillCount;

                                if (find.CurentKillSteak > find.MaxKillSteak)
                                {
                                    find.MaxKillSteak = find.CurentKillSteak;
                                }

                                if (find.CurentKillSteak > 3)
                                {
                                    SendKillStreak(_Player, find.CurentKillSteak);
                                }
                            }

                            // Lấy ra danh hiệu của người chơi có thể nhận được
                            string Title = GetRankTitleByPoint(find.Score);

                            if (_Player.TempTitle != Title)
                            {
                                //Sét lại danh hiệu cho người chơi
                                _Player.TempTitle = Title;
                            }

                            //  Tạo ra 1 cái TMP để lấy thứ tự
                            List<GuildWarPlayer> _TMP = _OutList.Values.OrderByDescending(x => x.Score).ToList();

                            //WarScore[_Player.GuildID] = _TMP;

                            var findIndex = _TMP.FindIndex(x => x._Player.RoleID == _Player.RoleID);

                            find.CurentRank = findIndex + 1;

                            // TODO : UPDATE TILE RANK FOR PLAYER

                            SendNotifyLoop(find);
                        }
                    }
                }

                Console.WriteLine("Update SCORE DONE :" + _Player.RoleName + "| POINT :" + Score);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "UpdateScore TOÁC :" + ex.ToString());
            }

            //  Console.WriteLine("Update SCORE :" + _Player.RoleName + "| POINT :" + Score);
        }

        #endregion ScoreDef

        #region DatabaseRequest

        /// <summary>
        /// lấy ra thông tin lãnh thổ của bang hiện tại
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public TerritoryInfo GetTerritoryInfo(int GuildID, int serverId = GameManager.LocalServerId)
        {
            try
            {
                byte[] bytesData = null;
                if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer3(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_KT_GUILD_TERRITORY, string.Format("{0}", GuildID), out bytesData, serverId))
                {
                    return null;
                }

                if (null == bytesData || bytesData.Length <= 6)
                {
                    return null;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);

                TerritoryInfo TerritoryData = DataHelper.BytesToObject<TerritoryInfo>(bytesData, 6, length - 2);

                if (null == TerritoryData)
                {
                    return null;
                }

                return TerritoryData;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Update Data
        /// </summary>
        public void UpdateTerritoryData(int MapID, string MapName, int GuildID, int Star, int Tax, int ZoneID, int IsMainCity, int Type)
        {
            TotalGuildInfo.TryGetValue(GuildID, out string GuildName);

            if (Type == 1)
            {
                // Bang bạn đã chiếm được
                string Notify = "Bang hội đã chiếm đóng được:<b>" + KTGlobal.CreateStringByColor(MapName, ColorType.Green) + "</b>";

                KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, GuildID, Notify, null, "");
            }
            else
            {
                string Notify = "Lảnh thổ đã bị đánh mất:<b>" + KTGlobal.CreateStringByColor(MapName, ColorType.Importal) + "</b>";

                KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, GuildID, Notify, null, "");
            }

            string CMDUPDATE = MapID + ":" + DataHelper.EncodeBase64(MapName) + ":" + GuildID + ":" + Star + ":" + Tax + ":" + ZoneID + ":" + IsMainCity + ":" + Type;

            string[] UpdateValue = Global.SendToDB((int)TCPGameServerCmds.CMD_KT_GUILD_UPDATE_TERRITORY, CMDUPDATE, GameManager.LocalServerId);

            if (UpdateValue[1] == "1")
            {
                if (Type == 0)
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + Type + "] Xóa lãnh thổ thành công :" + MapName);
                }
                else
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + Type + "] Cập nhật lãnh thổ thành công :" + MapName + "| MapID :" + MapID + "| GuildID :" + GuildID + "| Star :" + Star + "| Tax :" + Tax + "| ZoneID :" + ZoneID + "| IsMainCity :" + IsMainCity);
                }
            }
            else
            {
                if (Type == 0)
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + Type + "] XXXXóa lãnh thổ thất bại :" + MapName);
                }
                else
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + Type + "] CCCCCập nhật lãnh thổ thất bại :" + MapName + "| MapID :" + MapID + "| GuildID :" + GuildID + "| Star :" + Star + "| Tax :" + Tax + "| ZoneID :" + ZoneID + "| IsMainCity :" + IsMainCity);
                }
            }
        }

        /// <summary>
        /// Lấy ra toàn bộ thông tin của bản đô fcuar máy chủ hiện tại
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public TerritoryInfo GetTotalTerritoryInfo(int GuildID, int serverId = GameManager.LocalServerId)
        {
            byte[] bytesData = null;
            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer3(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_KT_GUILD_ALLTERRITORY, string.Format("{0}", GuildID), out bytesData, serverId))
            {
                return null;
            }

            if (null == bytesData || bytesData.Length <= 6)
            {
                return null;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);

            TerritoryInfo TerritoryData = DataHelper.BytesToObject<TerritoryInfo>(bytesData, 6, length - 2);

            if (null == TerritoryData)
            {
                return null;
            }

            return TerritoryData;
        }

        public MiniGuildInfo GetMiniGuildInfo(int GuildID, int serverId = GameManager.LocalServerId)
        {
            byte[] bytesData = null;
            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer3(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_KT_GUILD_GETMINIGUILDINFO, string.Format("{0}", GuildID), out bytesData, serverId))
            {
                return null;
            }

            if (null == bytesData || bytesData.Length <= 6)
            {
                return null;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);

            MiniGuildInfo _MiniGUild = DataHelper.BytesToObject<MiniGuildInfo>(bytesData, 6, length - 2);

            if (null == _MiniGUild)
            {
                return null;
            }

            return _MiniGUild;
        }

        public void ReloadAllTerritoryInfo()
        {
            string CMDUPDATE = "1";

            string[] UpdateValue = Global.SendToDB((int)TCPGameServerCmds.CMD_KT_GUILD_ALLTERRITORY, CMDUPDATE, GameManager.LocalServerId);
        }

        /// <summary>
        ///  Kiểm tra xem đây có phải tân thủ thôn hay không
        /// </summary>
        /// <param name="MapID"></param>
        /// <returns></returns>
        public bool IsTanThuThon(int MapID)
        {
            var find = _Config.WarMapConfigs.Where(x => x.MapID == MapID).FirstOrDefault();
            if (find != null)
            {
                return find.IsFightMapID;
            }

            return false;
        }

        #endregion DatabaseRequest

        #region NpClick

        /// <summary>
        /// Quản lãnh thổ click
        /// </summary>
        /// <param name="map"></param>
        /// <param name="npc"></param>
        /// <param name="client"></param>
        public void NpcClick(NPC npc, KPlayer client)
        {
            Console.Write(_Config);

            KNPCDialog _NpcDialog = new KNPCDialog();

            Dictionary<int, string> Selections = new Dictionary<int, string>();

            // Nếu mà chiến trường đã kết thúc thì add thêm cái nhận thưởng
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_REGISTER || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPARSTART)
            {
                if (client.GuildID > 0)
                {
                    // _NpcDialog.Text = "Có thể tấn công lãnh thổ lân cận đã bị chiếm lĩnh.Bang của bạn có thể tuyên chiến với các lãnh thổ sau:";

                    if (client.GuildRank == (int)GuildRank.Master)
                    {
                        TerritoryInfo territoryInfo = GetTerritoryInfo(client.GuildID);

                        if (territoryInfo != null)
                        {
                            Console.WriteLine(territoryInfo);

                            // Nếu như trường hợp còn cái dái khô không có lãnh thổ nào thì cho nó được đnáh chiếm các map tân thủ thôn
                            if (territoryInfo.Territorys == null)
                            {
                                _NpcDialog.Text += "<b>Vì Bang Hội của bạn chưa chiếm được 1 lãnh thổ nào.Nên bang hội của bạn chỉ có thể chọn 1 trong số các Tân Thủ Thôn Sau để chiếm đóng</b><br>Sau khi chiếm đóng được 1 tân thủ thôn bất kỳ.Ở lần tranh đoạt đợt sau Bang Hội của bạn có thể chọn tùy ý 1 lãnh thổ để chiếm đóng!";

                                List<OpenConfig> TotalMapCanFight = _Config.WarMapConfigs.Where(x => x.FightMapID > 0).ToList();

                                // Chỉ lấy ra tân thủ thôn nếu như thằng này chưa chiếm được lãnh thổ nào
                                foreach (OpenConfig _OpenMap in TotalMapCanFight)
                                {
                                    Selections.Add(_OpenMap.MapID, _OpenMap.MapName + " | Đã tuyên chiến:" + CountRegisterFight(_OpenMap.MapID));
                                }
                            } // Nếu như bang này đã có lãnh thổ
                            else if (territoryInfo.Territorys != null)
                            {
                                // TH1 : Nếu bang này chỉ có 1 lãnh thổ
                                if (territoryInfo.TerritoryCount > 0)
                                {
                                    // Kiểm tra xem các lãnh thổ bang này chiếm được có phải là tân thủ thôn hay không
                                    bool IsAllTanThuThon = true;

                                    foreach (Territory _Terriory in territoryInfo.Territorys)
                                    {
                                        //Nếu đây là tân thủ thôn
                                        if (!IsTanThuThon(_Terriory.MapID))
                                        {
                                            IsAllTanThuThon = false;
                                            break;
                                        }
                                    }

                                    // Nếu tất cả đều là tân thủ thôn thì được khiếu chiến 1 bản đồ bất kỳ
                                    if (IsAllTanThuThon)
                                    {
                                        _NpcDialog.Text += "<b><color=yellow>Vì Bang Hội của bạn đang chiếm giữ Tân Thủ Thôn và chưa có 1 lãnh thổ nào nên Bang Hội của bạn sẽ được chọn 1 lãnh thổ bất kỳ để tuyên chiến để xây dựng thành chính</b></color><br><br>Hãy cân nhắc kỹ trước khi chọn vì nó sẽ là vị trí chiến lược ảnh hưởng rất lớn tới chiến thuật sau này!";

                                        // Chỉ lấy ra tân thủ thôn nếu như thằng này chưa chiếm được lãnh thổ nào
                                        foreach (OpenConfig _OpenMap in _Config.WarMapConfigs.Where(x => x.FightMapID == -1))
                                        {
                                            Selections.Add(_OpenMap.MapID, _OpenMap.MapName + " | Đã tuyên chiến:" + CountRegisterFight(_OpenMap.MapID));
                                        }
                                    } // Nếu như đã có lãnh thổ
                                    else
                                    {
                                        var findMainCity = territoryInfo.Territorys.Where(x => x.IsMainCity == 1).FirstOrDefault();

                                        // Chắc chắn là có thành chính thì mới có liền kề
                                        if (findMainCity != null)
                                        {
                                            _NpcDialog.Text += "<b>Bang bạn đang có [" + findMainCity.MapName + "] đang là thành chính!Bạn chỉ có thể đánh chiếm các khu liền với lãnh thổ của bang mình mà không cần tuyên chiến bao gồm:<br><br>";
                                        }
                                        else
                                        {
                                            _NpcDialog.Text += "<b>Bang bạn chỉ có thể đánh chiếm các khu liền với lãnh thổ của bang mình mà không cần tuyên chiến bao gồm:<br><br>";
                                        }

                                        // Duyệt lại toàn bộ các lãnh thổ

                                        foreach (Territory _AreadyFix in territoryInfo.Territorys)
                                        {
                                            // Chắc chắn bản đồ này phỉa không có tân thủ thôn
                                            if (!IsTanThuThon(_AreadyFix.MapID))
                                            {
                                                // Tìm ra bản đồ liền kề với lãnh thổ hiện tại
                                                GuildMapConfig NearMapConfig = _Config.NearMapConfig.Where(x => x.MapId == _AreadyFix.MapID).FirstOrDefault();

                                                if (NearMapConfig != null)
                                                {
                                                    // Lấy ra toàn bộ bản đồ liền kề
                                                    List<int> TotalMapNear = NearMapConfig.NearMap;

                                                    foreach (int MapNear in TotalMapNear)
                                                    {
                                                        // Tìm xem có bản đồ này không
                                                        var FindOpenConfig = _Config.WarMapConfigs.Where(x => x.MapID == MapNear).FirstOrDefault();

                                                        if (FindOpenConfig != null)
                                                        {
                                                            // Tức là lãnh thổ này phỉa chưa có trong lãnh thổ của bang thì mới được phép tuyên chuyến
                                                            var FINDEXITS = territoryInfo.Territorys.Where(x => x.MapID == MapNear).FirstOrDefault();

                                                            if (FINDEXITS == null)
                                                            {
                                                                _NpcDialog.Text += "<color=green>" + FindOpenConfig.MapName + "</color><br>";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + client.GuildName + "] Bản đồ liền kề này không mở tranh đoạt :" + MapNear);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + client.GuildName + "] không tìm thấy bản đồ liền kề của :" + _AreadyFix.MapID);
                                                }
                                            }
                                            else
                                            {
                                                LogManager.WriteLog(LogTypes.GuildWarManager, "[" + client.GuildName + "] Kiểm tra bản đồ :" + _AreadyFix.MapID + "==> Là tân thủ thôn nên bỏ qua");
                                            }
                                        }
                                    }
                                }
                                else // Nếu chưa có lãnh thổ nào thì cho đánh thôn
                                {
                                    _NpcDialog.Text += "<b>Vì Bang Hội của bạn chưa chiếm được 1 lãnh thổ nào.Nên bang hội của bạn chỉ có thể chọn 1 trong số các Tân Thủ Thôn Sau để chiếm đóng</b><br>Sau khi chiếm đóng được 1 tân thủ thôn bất kỳ.Ở lần tranh đoạt đợt sau Bang Hội của bạn có thể chọn tùy ý 1 lãnh thổ để chiếm đóng!";

                                    List<OpenConfig> TotalMapCanFight = _Config.WarMapConfigs.Where(x => x.FightMapID > 0).ToList();

                                    // Chỉ lấy ra tân thủ thôn nếu như thằng này chưa chiếm được lãnh thổ nào
                                    foreach (OpenConfig _OpenMap in TotalMapCanFight)
                                    {
                                        Selections.Add(_OpenMap.MapID, _OpenMap.MapName + " | Đã tuyên chiến:" + CountRegisterFight(_OpenMap.MapID));
                                    }
                                }
                            }
                        }
                        else
                        {
                            _NpcDialog.Text += "<b>Vì Bang Hội của bạn chưa chiếm được 1 lãnh thổ nào.Nên bang hội của bạn chỉ có thể chọn 1 trong số các Tân Thủ Thôn Sau để chiếm đóng</b><br>Sau khi chiếm đóng được 1 tân thủ thôn bất kỳ.Ở lần tranh đoạt đợt sau Bang Hội của bạn có thể chọn tùy ý 1 lãnh thổ để chiếm đóng!";

                            List<OpenConfig> TotalMapCanFight = _Config.WarMapConfigs.Where(x => x.FightMapID > 0).ToList();

                            // Chỉ lấy ra tân thủ thôn nếu như thằng này chưa chiếm được lãnh thổ nào
                            foreach (OpenConfig _OpenMap in TotalMapCanFight)
                            {
                                Selections.Add(_OpenMap.MapID, _OpenMap.MapName + " | Đã tuyên chiến:" + CountRegisterFight(_OpenMap.MapID));
                            }
                        }
                    }
                    else
                    {
                        _NpcDialog.Text = "Hãy bảo bang chủ của ngươi tới gặp ta!";
                    }
                }
                else
                {
                    _NpcDialog.Text = "Hãy gia nhập một bang hội rồi hãy quay lại gặp ta!";
                }
            }
            else if (BATTLESTATE == GUILDWARDSTATUS.STATUS_END)
            {
                _NpcDialog.Text = "Xin chào :" + client.RoleName + " Bang hội <b>" + KTGlobal.CreateStringByColor(client.GuildName, ColorType.Green) + "</b> sau thời gian chinh chiến Bang của bạn đã đạt được kết quả sau:<br><br><br> ";

                if (FinalResult.TryGetValue(client.GuildID, out List<FightResult> result))
                {
                    if (result != null)
                    {
                        List<FightResult> ChiemDuoc = result.Where(x => x.Type == 0).ToList();
                        if (ChiemDuoc.Count > 0)
                        {
                            _NpcDialog.Text += "<b><color=green>Lãnh thổ đánh chiếm được :</color></b><br>";

                            foreach (FightResult _LanhTho in ChiemDuoc)
                            {
                                _NpcDialog.Text += "<b> " + _LanhTho.MapName + " </b><br>";
                            }
                        }

                        List<FightResult> BaoVe = result.Where(x => x.Type == 1).ToList();
                        if (BaoVe.Count > 0)
                        {
                            _NpcDialog.Text += "<b><color=green>Lãnh đã bảo vệ thành công :</color></b><br>";

                            foreach (FightResult _LanhTho in BaoVe)
                            {
                                _NpcDialog.Text += "<b> " + _LanhTho.MapName + " </b><br>";
                            }
                        }

                        List<FightResult> DeMat = result.Where(x => x.Type == 2).ToList();
                        if (DeMat.Count > 0)
                        {
                            _NpcDialog.Text += "<b><color=red>Lãnh thổ bị đánh chiếm :</color></b><br>";

                            foreach (FightResult _LanhTho in DeMat)
                            {
                                _NpcDialog.Text += "<b> " + _LanhTho.MapName + " </b><br>";
                            }
                        }

                        List<FightResult> ThuHoi = result.Where(x => x.Type == 3).ToList();
                        if (ThuHoi.Count > 0)
                        {
                            _NpcDialog.Text += "<b><color=red>Lãnh thổ bị hệ thống thu hồi :</color></b><br>";

                            foreach (FightResult _LanhTho in ThuHoi)
                            {
                                _NpcDialog.Text += "<b> " + _LanhTho.MapName + " </b><br>";
                            }
                        }
                    }
                }

                _NpcDialog.Text += "<br><br>Trong giai đoạn chinh chiến bạn nhận được :<br>";

                // Lấy ra dict chứa điểm của thằng này
                // Thử lấy ra
                if (WarScore.TryGetValue(client.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _Out))
                {
                    if (_Out != null)
                    {
                        _Out.TryGetValue(client.RoleID, out GuildWarPlayer find);

                        if (find != null)
                        {
                            _NpcDialog.Text += "<b>" + find.Score + " điểm </b> công trạng cá nhân!<br>";
                        }
                    }

                    int SelectIndex = _Out.Values.OrderByDescending(x => x.Score).ToList().FindIndex(x => x._Player.RoleID == client.RoleID) + 1;

                    int BoxCanEarn = 0;

                    int FinalDanhVong = 0;

                    int MaxDanhVong = 50;

                    int Percent = GetPercentInGuild(client);

                    if (Percent == -1)
                    {
                        _NpcDialog.Text += "<b>Thành tích quá thấp không được xếp hạng</b>";
                    }
                    else
                    {
                        if (Percent > 0 && Percent <= 10)
                        {
                            _NpcDialog.Text += "Đạt được <b><color=yellow>Nhất Kỵ Đương Thiên (Xếp hạng trong bang trước 10%)</color></b>";
                        }
                        else if (Percent > 10 && Percent <= 25)
                        {
                            _NpcDialog.Text += "Đạt được <b><color=yellow>Chiến Công Hiển Hách (Xếp hạng trong bang trước 25%)</color></b>";
                        }
                        else if (Percent > 25 && Percent <= 45)
                        {
                            _NpcDialog.Text += "Đạt được <b><color=yellow>Hãn Mã Công Lao (Xếp hạng trong bang trước 45%)</color></b>";
                        }
                        else if (Percent > 45 && Percent <= 70)
                        {
                            _NpcDialog.Text += "Đạt được <b><color=yellow>Phá Quân Hổ Vệ (Xếp hạng trong bang trước 70%)</color></b>";
                        }
                        else if (Percent > 70)
                        {
                            _NpcDialog.Text += "Đạt được <b><color=yellow>Dũng Sĩ Sa Trường (Xếp hạng trong bang sau 70%)</color></b>";
                        }

                        int PercentLess = 100 - Percent;

                        FinalDanhVong = (MaxDanhVong * PercentLess) / 100;

                        if (SelectIndex == 1)
                        {
                            BoxCanEarn = 14;
                        }
                        else if (SelectIndex == 2)
                        {
                            BoxCanEarn = 12;
                        }
                        else if (SelectIndex == 3)
                        {
                            BoxCanEarn = 10;
                        }
                        else if (SelectIndex >= 4 && SelectIndex <= 10)
                        {
                            BoxCanEarn = 8;
                        }
                        else if (SelectIndex > 10)
                        {
                            BoxCanEarn = 3;
                        }

                        _NpcDialog.Text += "<br>Xét thấy 2 điểm trên, đặc biệt khen thưởng cho ngươi <color=green>" + FinalDanhVong + "</color> điểm danh vọng lãnh thổ, <color=green>" + BoxCanEarn + "</color> rương tranh đoạt lãnh thổ.";
                    }
                }

                Selections.Add(1, "Ta muốn nhận thưởng");

                Selections.Add(1000, "Ta sẽ quay lại sau!");
            }
            else
            {
                _NpcDialog.Text = "Tranh đoạn chưa bắt đầu vui lòng quay lại sau";
            }

            //// Menu này lúc nào cũng có

            //Selections.Add(-2, "Ta muốn rời khỏi chiến trường");

            Action<TaskCallBack> ActionWork = (x) => DoActionSelect(client, npc, x);

            _NpcDialog.OnSelect = ActionWork;

            _NpcDialog.Selections = Selections;

            //_NpcDialog.Text = Text;

            _NpcDialog.Show(npc, client);
        }

        public int GetPercentInGuild(KPlayer client)
        {
            int Pecent = -1;

            if (WarScore.TryGetValue(client.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _Out))
            {
                if (_Out != null)
                {
                    //SORT LẠI
                    List<GuildWarPlayer> _TMP = _Out.Values.OrderByDescending(x => x.Score).ToList();

                    if (_TMP != null)
                    {
                        int YourIndex = _TMP.FindIndex(x => x._Player.RoleID == client.RoleID);

                        int Total = _TMP.Count;

                        Pecent = (YourIndex * 100) / Total;
                    }
                }
            }

            return Pecent;
        }

        private void DoActionSelect(KPlayer client, NPC npc, TaskCallBack x)
        {
            KNPCDialog _NpcDialog = new KNPCDialog();

            Dictionary<int, string> Selections = new Dictionary<int, string>();
            // TODO CLICK VÀO CÁI GÌ

            Console.WriteLine("CLICK TO :" + x.SelectID);

            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_REGISTER || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPARSTART)
            {
                // Tức là tuyên chiến
                if (x.SelectID < 1000)
                {
                    var find = _Config.WarMapConfigs.Where(g => g.MapID == x.SelectID).FirstOrDefault();
                    if (find != null)
                    {
                        _NpcDialog.Text = "Bạn có chắc chắn muốn tuyên chiến với lãnh thổ :" + find.MapName;

                        int Count = CountRegisterFight(find.MapID);

                        if (Count > 0)
                        {
                            _NpcDialog.Text += "<br><br>Hiện bản đồ này được tuyển chiến bởi các Bang :<br><br>";

                            List<GuildMapRegisterFight> _RegíterFight = TotalMapRegisterFight.Where(g => g.MapId == x.SelectID).ToList();

                            foreach (GuildMapRegisterFight _Guild in _RegíterFight)
                            {
                                _NpcDialog.Text += "<b><color=red>" + _Guild.GuildName + "</color></b>";
                            }
                        }

                        Selections.Add(x.SelectID, "Ta Muốn Tuyên Chiến");

                        Selections.Add(1000, "Ta sẽ quay lại sau!");
                    }
                }
            }
            else if (BATTLESTATE == GUILDWARDSTATUS.STATUS_END)
            {
                //Click Nhận thưởng
                if (x.SelectID == 1)
                {
                    if (WarScore.TryGetValue(client.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _Out))
                    {
                        if (_Out != null)
                        {
                            _Out.TryGetValue(client.RoleID, out GuildWarPlayer FindExits);

                            //  var FindExits = _Out.Where(xx => xx._Player.RoleID == client.RoleID).FirstOrDefault();

                            if (FindExits != null)
                            {
                                if (!FindExits.IsReviceReward)
                                {
                                    // Lấy ra thức hạng hiện tại
                                    int SelectIndex = _Out.Values.OrderByDescending(xx => xx.Score).ToList().FindIndex(xx => xx._Player.RoleID == client.RoleID) + 1;

                                    int BoxCanEarn = 0;

                                    int FinalDanhVong = 0;

                                    int MaxDanhVong = 50;

                                    int Percent = GetPercentInGuild(client);

                                    if (Percent == -1 || client.m_Level < 80)
                                    {
                                        _NpcDialog.Text += "Thành tích quá thấp không thể nhận thưởng";
                                    }
                                    else
                                    {
                                        int PercentLess = 100 - Percent;

                                        FinalDanhVong = (MaxDanhVong * PercentLess) / 100;

                                        if (SelectIndex == 1)
                                        {
                                            BoxCanEarn = 14;
                                        }
                                        else if (SelectIndex == 2)
                                        {
                                            BoxCanEarn = 12;
                                        }
                                        else if (SelectIndex == 3)
                                        {
                                            BoxCanEarn = 10;
                                        }
                                        else if (SelectIndex >= 4 && SelectIndex <= 10)
                                        {
                                            BoxCanEarn = 8;
                                        }
                                        else if (SelectIndex > 10)
                                        {
                                            BoxCanEarn = 3;
                                        }

                                        if (KTGlobal.IsHaveSpace(BoxCanEarn, client))
                                        {
                                            FindExits.IsReviceReward = true;

                                            _NpcDialog.Text += "Nhận thưởng thành công!";

                                            if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 591, BoxCanEarn, 0, "TDLT", false, 1, false, Global.ConstGoodsEndTime))
                                            {
                                                PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm chế tạo");
                                            }

                                            int CAMP = 801;

                                            int MAXMONEYCANGET = 10000;

                                            int FINALMONEY = (MAXMONEYCANGET * PercentLess) / 100;

                                            KTGlobal.AddRepute(client, CAMP, FinalDanhVong);

                                            //Update tích lũy cá nhân
                                            KTGlobal.AddMoney(client, FINALMONEY, MoneyType.GuildMoney, "TDLT");

                                            //Update tiền cho bagn hội
                                            KTGlobal.UpdateGuildMoney(1890, client.GuildID, client);
                                        }
                                        else
                                        {
                                            _NpcDialog.Text += "Túi đồ của bạn không đủ chỗ trống!";
                                        }
                                    }
                                }
                                else
                                {
                                    _NpcDialog.Text += "Bạn đã nhận thưởng rồi!";
                                }
                            }
                            else
                            {
                                _NpcDialog.Text += "Không tìm thấy người chơi";
                            }
                        }
                    }
                }
            }

            Action<TaskCallBack> ActionWork = (h) => DoConfirm(client, npc, h);

            _NpcDialog.OnSelect = ActionWork;

            _NpcDialog.Selections = Selections;

            _NpcDialog.Show(npc, client);
        }

        /// <summary>
        /// xác nhận cái việc chọn
        /// </summary>
        /// <param name="client"></param>
        /// <param name="npc"></param>
        /// <param name="h"></param>
        private void DoConfirm(KPlayer client, NPC npc, TaskCallBack h)
        {
            KNPCDialog _NpcDialog = new KNPCDialog();

            // Nếu là muốn nghĩa lại
            if (h.SelectID == 1000)
            {
                KT_TCPHandler.CloseDialog(client);
            }
            else
            {
                if (BATTLESTATE == GUILDWARDSTATUS.STATUS_REGISTER || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPARSTART)
                {
                    int GuildId = client.GuildID;

                    MiniGuildInfo _GetInfo = GetMiniGuildInfo(GuildId);

                    // Mở lại đoạn này tránh spam bang hội
                    if (_GetInfo != null)
                    {
                        //if (_GetInfo.TotalPrestige < 10000)
                        //{
                        //    _NpcDialog.Text = "Bang hội phỉa đủ 10.000 UY danh mới có thể tuyên chiến!";
                        //    _NpcDialog.Show(npc, client);

                        //    return;
                        //}

                        //if (_GetInfo.TotalMember < 30)
                        //{
                        //    _NpcDialog.Text = "Bang hội phải đủ 30 người mới có thể tuyên chiến";
                        //    _NpcDialog.Show(npc, client);

                        //    return;
                        //}

                        //if (_GetInfo.MoneyStore < 10000000)
                        //{
                        //    _NpcDialog.Text = "Quỹ bang hội phỉa từ 1000v mới có thể tuyên chiến";
                        //    _NpcDialog.Show(npc, client);

                        //    return;
                        //}

                        var find = TotalMapRegisterFight.Where(x => x.GuildID == GuildId && x.MapId == h.SelectID).FirstOrDefault();
                        if (find != null)
                        {
                            _NpcDialog.Text = "Bang hội của bạn đã tuyên chiến với lãnh thổ này rồi";
                            _NpcDialog.Show(npc, client);

                            return;
                        }
                        else
                        {
                            // Nếu bang này trước đó đã chiếm được 1 tân thủ thôn
                            TerritoryInfo territoryInfo = GetTerritoryInfo(client.GuildID);

                            bool IsAllTanThuThon = true;

                            if (territoryInfo != null)
                            {
                                if (territoryInfo.Territorys != null)
                                {
                                    foreach (Territory _Terriory in territoryInfo.Territorys)
                                    {
                                        //Nếu đây là tân thủ thôn
                                        if (!IsTanThuThon(_Terriory.MapID))
                                        {
                                            IsAllTanThuThon = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            // Nếu tất cả đều là tân thủ thôn thì được khiếu chiến 1 bản đồ bất kỳ
                            if (IsAllTanThuThon)
                            {
                                var FindRegister = TotalMapRegisterFight.Where(x => x.GuildID == GuildId).FirstOrDefault();

                                if (FindRegister != null)
                                {
                                    _NpcDialog.Text = "Trong lần tuyên chiến này bạn chỉ có thể tuyên chiến 1 lãnh thổ để thiết lập căn cứ thành chính không thể tuyên chiến thêm lãnh thổ khác nữa";
                                }
                                else
                                {
                                    OpenConfig _Find = _Config.WarMapConfigs.Where(x => x.MapID == h.SelectID).FirstOrDefault();
                                    if (_Find != null)
                                    {
                                        GuildMapRegisterFight _ActiveFight = new GuildMapRegisterFight();
                                        _ActiveFight.MapId = _Find.MapID;
                                        _ActiveFight.GuildName = client.GuildName;
                                        _ActiveFight.GuildID = client.GuildID;
                                        _ActiveFight.MapName = _Find.MapName;

                                        TotalMapRegisterFight.Add(_ActiveFight);

                                        _NpcDialog.Text = "Tuyên chiến thành công với lãnh thổ <b>[" + _Find.MapName + "]</b>";

                                        string MSG = KTGlobal.CreateStringByColor("Bang hội đã Tuyên Chiến Thành công với lãnh thổ [" + _Find.MapName + "] !", ColorType.Importal);

                                        KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.GuildID, MSG, null, "");
                                    }
                                }
                            }
                            else // Nếu không phải tức là cho nó khiêu chiến thỏa mái
                            {
                                OpenConfig _Find = _Config.WarMapConfigs.Where(x => x.MapID == h.SelectID).FirstOrDefault();
                                if (_Find != null)
                                {
                                    GuildMapRegisterFight _ActiveFight = new GuildMapRegisterFight();
                                    _ActiveFight.MapId = _Find.MapID;
                                    _ActiveFight.GuildName = client.GuildName;
                                    _ActiveFight.GuildID = client.GuildID;
                                    _ActiveFight.MapName = _Find.MapName;

                                    TotalMapRegisterFight.Add(_ActiveFight);

                                    _NpcDialog.Text = "Tuyên chiến thành công với lãnh thổ <b>[" + _Find.MapName + "]</b>";
                                }
                            }

                            _NpcDialog.Show(npc, client);
                        }
                    }
                }
            }
        }

        #endregion NpClick

        #region GMCommand

        /// <summary>
        /// Lệnh gm chạy để bắt đầu chiến đấu
        /// </summary>
        public void BattleForceStart()
        {
            BATTLESTATE = GUILDWARDSTATUS.STATUS_REGISTER;

            LastTick = TimeUtil.NOW();

            LogManager.WriteLog(LogTypes.GuildWarManager, "Battle Change State ==> " + BATTLESTATE.ToString());

            KTGlobal.SendSystemEventNotification("Lãnh thổ chiến đang bước vào thời kỳ tuyên chiến, hãy đến Quan Lãnh Thổ các thành để xác định mục tiêu chinh chiến!");
        }

        /// <summary>
        /// Hàm GM gọi end battle nếu mà toác thì nó vẫn ngon
        /// </summary>
        public void BattleForceEnd()
        {
            LastTick = TimeUtil.NOW();

            KTGlobal.SendSystemEventNotification("Tranh đoạt lãnh thổ đã tới hồi kết thúc! Hãy tới Quan Lãnh Thổ để nhận thưởng");

            // Chuyển sang trạng thái end
            BATTLESTATE = GUILDWARDSTATUS.STATUS_END;

            //Gọi update bảng xếp hạng lần cuối cùng
            UpdateGuildRanking();
            // Thiết lập lãnh thổ đọc ghi DB

            // Clear toàn bộ cột trụ quá , boss,
            // Unlock status cho toàn bộ thành viên tham gia
            ResetBattle();

            SetupFinishTerritory();

            // Thực hiện ghi vào DB
            WriterToDatabase();

            //Reload lại toàn bộ thông tin lãnh thổ
            ReloadAllTerritoryInfo();

            //todo : Làm thêm quả xếp hạng ở NPC khi nó ấn vào phần thưởng

            LogManager.WriteLog(LogTypes.GuildWarManager, " Battle Change State ==> " + BATTLESTATE.ToString());

            _CaheMiniMap = new List<GuildWarMiniMap>();

            SetupMiniMap();
        }

        #endregion GMCommand

        #region Battle Controller

        public void SwitchOffStatus()
        {
            try
            {
                foreach (KeyValuePair<int, ConcurrentDictionary<int, GuildWarPlayer>> entry in WarScore)
                {
                    // Lấy ra danh sách người chơi
                    ConcurrentDictionary<int, GuildWarPlayer> TotalPlayer = entry.Value;

                    // Set lại state cho người chơi
                    foreach (GuildWarPlayer _client in TotalPlayer.Values)
                    {
                        this.ChangeState(_client._Player, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, ex.ToString());
            }
        }

        public void CreateMapEvent()
        {
            foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
            {
                GuildWarMap MapEvent = entry.Value;

                // Tạo long trụ khi sự bắt đầu
                MapEvent.CreateBuildingWhenStart();

                //Acvie sang trạng thái chiến đấu cho người chơi
                MapEvent.ActiveAllStatusAndPKModelInMap();
            }
        }

        /// <summary>
        /// Spam camp cho người chơi
        /// </summary>
        public void SpamCamp()
        {
            LogManager.WriteLog(LogTypes.GuildWarManager, "CALL SPAM CAMP ");

            try
            {
                foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
                {
                    GuildWarMap MapEvent = entry.Value;

                    //Acvie sang trạng thái chiến đấu cho người chơi
                    MapEvent.ActiveAllStatusAndPKModelInMap();
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void ResetBattle()
        {
            ResetMap();
            SwitchOffStatus();
        }

        /// <summary>
        /// Setup lại lãnh thổ sau khi kết túc
        /// </summary>
        public void SetupFinishTerritory()
        {
            // Duyệt ra tất cả các lãnh thổ
            foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
            {
                GuildWarMap MapEvent = entry.Value;
                int MAPID = entry.Key;

                //Lấy ra thông tin bản đồ
                var find = _Config.WarMapConfigs.Where(x => x.ReadMapID == MAPID).FirstOrDefault();
                //  Tìm ra cái bản đồ này
                if (find != null)
                {
                    //Lấy ra thực thể quản lý điểm ở cái bản đồ này
                    TotalScore.TryGetValue(MAPID, out List<GuildTotalScore> _OutList);

                    //Nếu như có dánh sách điểm
                    if (_OutList != null)
                    {
                        // Ghi lại logs điểm các bên cho đỡ cãi
                        foreach (GuildTotalScore _GuildLogs in _OutList)
                        {
                            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + _GuildLogs.GuildName + "][" + _GuildLogs.GuildID + "] ==> MAPNAME : [" + find.MapName + "] |  SCORE :" + _GuildLogs.Score);
                        }
                        // Lấy ra  thằng có điểm cao nhất
                        var FindTopGuild = _OutList.OrderByDescending(x => x.Score).FirstOrDefault();

                        if (FindTopGuild != null)
                        {
                            //Lấy ra danh sách điểm cuối cùng của cái bang này xem bản đồ nào có kết quả thế nào
                            if (FinalResult.TryGetValue(FindTopGuild.GuildID, out List<FightResult> TotalMap))
                            {
                                FightResult _Rep = new FightResult();
                                _Rep.MapID = find.MapID;
                                _Rep.MapName = find.MapName;
                                _Rep.IsFightMap = find.IsFightMapID;

                                // Nếu map này trước đó đã có chủ
                                if (MapEvent.BeLongGuild != -1)
                                {
                                    // Gán lại chủ cũ vào kết quả
                                    _Rep.OldGuildID = MapEvent.BeLongGuild;

                                    if (MapEvent.BeLongGuild == FindTopGuild.GuildID)
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.Type = 1;
                                    }
                                    else
                                    {
                                        //Nếu là bị chiếm thì ghi vào cái GUILD mới
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.Type = 0;

                                        if (FinalResult.TryGetValue(MapEvent.BeLongGuild, out List<FightResult> BelongGuildTotalMap))
                                        {
                                            FightResult _RepBelong = new FightResult();
                                            _RepBelong.MapID = find.MapID;
                                            _RepBelong.MapName = find.MapName;
                                            _RepBelong.Type = 2;
                                            _RepBelong.IsFightMap = find.IsFightMapID;
                                            _RepBelong.NewGuildID = FindTopGuild.GuildID;
                                            _RepBelong.OldGuildID = MapEvent.BeLongGuild;
                                            BelongGuildTotalMap.Add(_RepBelong);
                                        }
                                        else
                                        {
                                            FightResult _RepBelong = new FightResult();
                                            _RepBelong.MapID = find.MapID;
                                            _RepBelong.MapName = find.MapName;
                                            _RepBelong.Type = 2;
                                            _RepBelong.IsFightMap = find.IsFightMapID;
                                            _RepBelong.NewGuildID = FindTopGuild.GuildID;
                                            _RepBelong.OldGuildID = MapEvent.BeLongGuild;

                                            List<FightResult> NewBelongGuildTotalMap = new List<FightResult>();
                                            NewBelongGuildTotalMap.Add(_RepBelong);
                                            FinalResult.TryAdd(MapEvent.BeLongGuild, NewBelongGuildTotalMap);
                                        }
                                    }
                                }
                                else
                                {
                                    if (MapEvent.CountGuildObjective(FindTopGuild.GuildID) >= 3)
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.OldGuildID = MapEvent.BeLongGuild;
                                        _Rep.Type = 0;
                                    }
                                    else
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.OldGuildID = MapEvent.BeLongGuild;
                                        _Rep.Type = 3;
                                    }
                                }

                                TotalMap.Add(_Rep);
                            }
                            else
                            {
                                FightResult _Rep = new FightResult();
                                _Rep.MapID = find.MapID;
                                _Rep.MapName = find.MapName;
                                _Rep.IsFightMap = find.IsFightMapID;

                                if (MapEvent.BeLongGuild != -1)
                                {
                                    _Rep.OldGuildID = MapEvent.BeLongGuild;
                                    if (MapEvent.BeLongGuild == FindTopGuild.GuildID)
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.Type = 1;
                                    }
                                    else
                                    {
                                        _Rep.Type = 0;
                                        _Rep.NewGuildID = FindTopGuild.GuildID;

                                        if (FinalResult.TryGetValue(MapEvent.BeLongGuild, out List<FightResult> BelongGuildTotalMap))
                                        {
                                            FightResult _RepBelong = new FightResult();
                                            _RepBelong.MapID = find.MapID;
                                            _RepBelong.MapName = find.MapName;
                                            _RepBelong.Type = 2;
                                            _RepBelong.IsFightMap = find.IsFightMapID;
                                            _RepBelong.NewGuildID = FindTopGuild.GuildID;
                                            _RepBelong.OldGuildID = MapEvent.BeLongGuild;
                                            BelongGuildTotalMap.Add(_RepBelong);
                                        }
                                        else
                                        {
                                            FightResult _RepBelong = new FightResult();
                                            _RepBelong.MapID = find.MapID;
                                            _RepBelong.MapName = find.MapName;
                                            _RepBelong.Type = 2;
                                            _RepBelong.IsFightMap = find.IsFightMapID;

                                            _RepBelong.NewGuildID = FindTopGuild.GuildID;
                                            _RepBelong.OldGuildID = MapEvent.BeLongGuild;

                                            List<FightResult> NewBelongGuildTotalMap = new List<FightResult>();
                                            NewBelongGuildTotalMap.Add(_RepBelong);

                                            FinalResult.TryAdd(MapEvent.BeLongGuild, NewBelongGuildTotalMap);
                                        }
                                    }
                                }
                                else
                                {
                                    if (MapEvent.CountGuildObjective(FindTopGuild.GuildID) >= 3)
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.OldGuildID = MapEvent.BeLongGuild;
                                        _Rep.Type = 0;
                                    }
                                    else
                                    {
                                        _Rep.NewGuildID = FindTopGuild.GuildID;
                                        _Rep.OldGuildID = MapEvent.BeLongGuild;
                                        // Để mất
                                        _Rep.Type = 3;
                                    }
                                }

                                List<FightResult> NewTotalMap = new List<FightResult>();
                                NewTotalMap.Add(_Rep);

                                // Thực hiện adđ vào
                                FinalResult.TryAdd(FindTopGuild.GuildID, NewTotalMap);
                            }
                        }
                    }
                }
            }

            //foreach (KeyValuePair<int, List<FightResult>> entry in FinalResult)
            //{
            //    int GUILDID = entry.Key;

            //    List<FightResult> _Total = entry.Value;

            //    //Nếu có 1 bản đồ không phải là lãnh thỏ
            //    var FindFist = _Total.Where(x => x.IsFightMap == false && x.Type == 0 && x.Type == 1).FirstOrDefault();

            //    if (FindFist != null)
            //    {
            //        var FindTanThuThon = _Total.Where(x => x.IsFightMap == true).ToList();

            //        foreach (FightResult _FightResult in FindTanThuThon)
            //        {
            //            //SET LẠI KIỂU HỆ THỐNG THU HỒI
            //            _FightResult.Type = 3;

            //            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + _FightResult.MapName + "] WILL BE REMOVE :" + GUILDID);
            //        }

            //        FinalResult[GUILDID] = _Total;
            //    }

            //}
        }

        //Code gì đó để ghi vào DB
        public void WriterToDatabase()
        {
            foreach (KeyValuePair<int, List<FightResult>> entry in this.FinalResult)
            {
                int GUILDID = entry.Key;

                List<FightResult> _Total = entry.Value;

                //Lấy 1 bản đồ đã chiếm được
                var FindNormalMap = _Total.Where(x => x.IsFightMap == false && (x.Type == 0 || x.Type == 1)).FirstOrDefault();

                foreach (FightResult _Fight in _Total)
                {
                    int Type = 1;

                    //if (_Fight.Type == 2 || _Fight.Type == 3)
                    //{
                    //    Type = 0;
                    //}

                    var FindRedMap = _Config.WarMapConfigs.Where(x => x.MapID == _Fight.MapID).FirstOrDefault();

                    // Nếu kết quả trả về là 2 thì cần xác định 2 thứ
                    if (_Fight.Type == 2)
                    {
                        // tức là thằng mới này bị làm mất bản đồ này nên ko cần xử lý
                        if (_Fight.OldGuildID != _Fight.NewGuildID && _Fight.OldGuildID == GUILDID)
                        {
                            string Notify = "Lảnh thổ đã bị đánh mất:<b>" + KTGlobal.CreateStringByColor(FindRedMap.MapName, ColorType.Importal) + "</b>";

                            KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, _Fight.OldGuildID, Notify, null, "");

                            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + GUILDID + "] Bản đồ này do bang để mất nên không cần chỉnh sửa DB  :" + FindRedMap.MapName);

                            continue;
                        }
                    }
                    if (_Fight.Type == 3)
                    {
                        Type = 0;
                    }

                    bool IsMustRemove = false;
                    // Nếu mà tìm được 1 bản đồ là lãnh thổ rồi mà thằng này còn có tân thủ thôn thì chắc chắn phải xóa
                    if (FindNormalMap != null)
                    {
                        LogManager.WriteLog(LogTypes.GuildWarManager, "[" + GUILDID + "] Tìm được bản đồ bình thường đã chiếm được là :" + FindNormalMap.MapName);
                        IsMustRemove = true;
                    }

                    if (FindRedMap != null)
                    {
                        // nếu đây là tân thủ thôn mà phải loại bỏ vì đã có lãnh thổ thì mới loại bỏ
                        if (FindRedMap.IsFightMapID && IsMustRemove)
                        {
                            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + GUILDID + "]CHECK BẢN ĐỒ :" + FindRedMap.MapName + "========> CẦN PHẢI XÓA");

                            Type = 0;
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.GuildWarManager, "[" + GUILDID + "]CHECK BẢN ĐỒ :" + FindRedMap.MapName + "========> KHÔNG CẦN PHỈA XÓA :" + IsMustRemove.ToString()); ;
                        }

                        if (FindRedMap.IsFightMapID)
                        {
                            // Nếu đây là tân thủ thôn thì ko thể là thành chính
                            UpdateTerritoryData(_Fight.MapID, _Fight.MapName, GUILDID, 2, 2, GameManager.ServerLineID, 0, Type);
                        }
                        else // Nếu đây là map giã ngoại mà lã lãnh thổ đầu tiên thì cho nó là thành chính
                        {
                            // Lấy ra danh sách lãnh thổ hiện tại của guild này
                            TerritoryInfo territoryInfo = GetTerritoryInfo(GUILDID);

                            if (territoryInfo != null)
                            {
                                if (territoryInfo.Territorys != null)
                                {
                                    var findexits = territoryInfo.Territorys.Where(x => x.IsMainCity == 1).FirstOrDefault();

                                    if (findexits != null)
                                    {
                                        UpdateTerritoryData(_Fight.MapID, _Fight.MapName, GUILDID, 2, 2, GameManager.ServerLineID, 0, Type);
                                    }
                                    else
                                    {
                                        UpdateTerritoryData(_Fight.MapID, _Fight.MapName, GUILDID, 2, 2, GameManager.ServerLineID, 1, Type);
                                    }
                                }
                                else // NẾU CHƯA CÓ LÃNH THỔ NÀO THÌ ĐÂY CHẮC CHẮN PHẢI LÀ THÔN
                                {
                                    UpdateTerritoryData(_Fight.MapID, _Fight.MapName, GUILDID, 2, 2, GameManager.ServerLineID, 0, Type);
                                }
                            }
                            else
                            {
                                UpdateTerritoryData(_Fight.MapID, _Fight.MapName, GUILDID, 2, 2, GameManager.ServerLineID, 0, Type);
                            }
                        }
                    }
                    else
                    {
                        LogManager.WriteLog(LogTypes.GuildWarManager, "TOAC  CANT FIND MAP ID :" + _Fight.MapID);
                    }
                }
            }
        }

        public void ResetMap()
        {
            try
            {
                foreach (KeyValuePair<int, GuildWarMap> entry in TotalMapWillBeWar)
                {
                    GuildWarMap MapEvent = entry.Value;
                    // Tạo long trụ khi sự bắt đầu
                    MapEvent.ClearAllMonster();
                    MapEvent.RemoveAllForbid();
                }
            }
            catch
            {
            }
        }

        public bool CanChangePKStatus(KPlayer client)
        {
            // nếu đang trong thời gian chiến đấu
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                int MapCode = client.CurrentMapCode;

                if (TotalMapWillBeWar.ContainsKey(MapCode))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion Battle Controller

        /// <summary>
        /// Battle Proseccsing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>

        private void ProsecBattle(object sender, EventArgs e)
        {
            /// Nếu mà ko bận thì gọi liên tục

            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
            else
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "Bussy EVENT worker!");
            }
        }

        #region Notify To Player

        public GuildWarReport GetReport(KPlayer client)
        {
            GuildWarReport _Report = new GuildWarReport();

            // Đây là điểm của bản thân mình

            // Tích lũy cá nhân

            //SET = 0 trước
            CurrentPoint _PontCaNhan = new CurrentPoint();
            _PontCaNhan.KillCount = 0;
            _PontCaNhan.Point = 0;
            _PontCaNhan.TowerDesotryCount = 0;

            List<GuildWarRanking> _GuildWarRanking = new List<GuildWarRanking>();

            ///Thử lấy ra
            if (WarScore.TryGetValue(client.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _Out))
            {
                if (_Out != null)
                {
                    //var find = _Out.Where(x => x._Player.RoleID == client.RoleID).FirstOrDefault();

                    _Out.TryGetValue(client.RoleID, out GuildWarPlayer find);

                    if (find != null)
                    {
                        _PontCaNhan.KillCount = find.KillCount;
                        _PontCaNhan.Point = find.Score;
                        _PontCaNhan.TowerDesotryCount = find.DestroyCount;

                        _Report._CurrentPoint = _PontCaNhan;
                    }
                }

                // TẠO RA CÁI TMP ĐỂ SẮP XẾP

                List<GuildWarPlayer> _TMP = _Out.Values.OrderByDescending(x => x.Score).ToList();

                int MAX = _TMP.Count;

                if (MAX > 20)
                {
                    MAX = 20;
                }
                // Đây alf điểm của 20 thằng trong bang
                for (int i = 0; i < MAX; i++)
                {
                    GuildWarPlayer _TmpPlayer = _TMP[i];
                    GuildWarRanking _Rank = new GuildWarRanking();
                    _Rank.Point = _TmpPlayer.Score;
                    _Rank.RoleName = _TmpPlayer._Player.RoleName;
                    _GuildWarRanking.Add(_Rank);
                }
            }

            _Report._GuildWarRanking = _GuildWarRanking;
            // Tới phần khó rồi lấy ra các lãnh thổ bang mình đang có và đứng hạng mấy
            List<TerritoryReport> _TerritoryReport = new List<TerritoryReport>();

            TotalReportUpdate.TryGetValue(client.GuildID, out _TerritoryReport);

            _Report._TerritoryReport = _TerritoryReport;

            return _Report;
        }

        public bool CanRespwan(KPlayer client)
        {
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                TotalMapWillBeWar.TryGetValue(client.CurrentMapCode, out GuildWarMap _OutMap);
                if (_OutMap != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi người chơi vào bản đồ này ĐM phần này toác cái cứt j đó ==> tới việc lỗi camp
        /// </summary>
        /// <param name="client"></param>
        public void OnEnterMap(KPlayer client)
        {
            // Nếu đang trong thời gian Battle xảy ra
            if (BATTLESTATE == GUILDWARDSTATUS.STATUS_START || BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
            {
                try
                {
                    //   LogManager.WriteLog(LogTypes.GuildWarManager, "CHECK STATE PLAYER :" + client.RoleName + "| ROLEID " + client.RoleID + "| GUILDNAME :" + client.GuildName + "|MAPCODE :" + client.CurrentMapCode);

                    //Nếu như thằng này có bang hội | TODO sửa lại cấm bọn ko có bang hội cũng phải toạch khi vào bản đồ này
                    if (client.GuildID > 0)
                    {
                        // Nếu bản đồ này đang nằm trong danh sách đánh
                        if (TotalMapWillBeWar.TryGetValue(client.CurrentMapCode, out GuildWarMap _OutValue))
                        {
                            if (_OutValue != null)
                            {
                                //  LogManager.WriteLog(LogTypes.GuildWarManager, "BẢN ĐỒ CÓ NẰM TRONG KHU VỰC TRANH ĐOẠT :" + client.RoleName + "| ROLEID " + client.RoleID + "| GUILDNAME :" + client.GuildName + "|MAPCODE :" + client.CurrentMapCode);
                                // Nếu bang của bạn có xảy ra chiến sự với bang này thì thực hiện chuyển PK sang bang hội
                                if (_OutValue.TotalGuildCanJoinBattle.Contains(client.GuildID))
                                {
                                    //  LogManager.WriteLog(LogTypes.GuildWarManager, "NGƯỜI CHƠI NÀY ĐÃ THAM GIA VÀO KHU VỰC TRANH ĐOẠT CỦA BANG :" + client.RoleName + "| ROLEID " + client.RoleID + "| GUILDNAME :" + client.GuildName + "|MAPCODE :" + client.CurrentMapCode);

                                    // Chuyển sang STATE 1 Để hiện bảng
                                    this.ChangeState(client, 1);
                                    // Tự động chuyển sang chế độ PK Bang hội
                                    client.PKMode = (int)PKMode.Custom;
                                    // SET camp cho thằng này về bang hội
                                    client.Camp = client.GuildID;

                                    // client.StopAllActiveFights();
                                    //SEND NOTIFY cho client

                                    if (WarScore.TryGetValue(client.GuildID, out ConcurrentDictionary<int, GuildWarPlayer> _OutList))
                                    {
                                        //  var find = _OutList.Where(x => x._Player.RoleID == client.RoleID).FirstOrDefault();

                                        _OutList.TryGetValue(client.RoleID, out GuildWarPlayer find);

                                        if (find != null)
                                        {
                                            find._Player = client;
                                        }
                                        else
                                        {
                                            GuildWarPlayer _PlayerJoinWar = new GuildWarPlayer();
                                            _PlayerJoinWar.GuildID = client.GuildID;
                                            _PlayerJoinWar.CurentRank = -1;
                                            _PlayerJoinWar.KillCount = 0;
                                            _PlayerJoinWar.DestroyCount = 0;
                                            _PlayerJoinWar.MaxKillSteak = 0;
                                            _PlayerJoinWar.IsReviceReward = false;
                                            _PlayerJoinWar.CurentKillSteak = 0;
                                            _PlayerJoinWar.Score = 0;
                                            _PlayerJoinWar._Player = client;

                                            // Thực hiện add vào danh sách
                                            _OutList.TryAdd(client.RoleID, _PlayerJoinWar);
                                        }
                                    }
                                    else   // Nếu đây là thành viên đầu tiên của bang này vào trong bản đồ thì ta sẽ thống kê
                                    {
                                        ConcurrentDictionary<int, GuildWarPlayer> _NewList = new ConcurrentDictionary<int, GuildWarPlayer>();

                                        GuildWarPlayer _PlayerJoinWar = new GuildWarPlayer();
                                        _PlayerJoinWar.GuildID = client.GuildID;
                                        _PlayerJoinWar.CurentRank = -1;
                                        _PlayerJoinWar.KillCount = 0;
                                        _PlayerJoinWar.MaxKillSteak = 0;
                                        _PlayerJoinWar.DestroyCount = 0;
                                        _PlayerJoinWar.IsReviceReward = false;
                                        _PlayerJoinWar.CurentKillSteak = 0;
                                        _PlayerJoinWar.Score = 0;
                                        _PlayerJoinWar._Player = client;

                                        //T hực hiện add vào danh sách mới
                                        _NewList.TryAdd(client.RoleID, _PlayerJoinWar);

                                        // thực hiện add vào danh sách
                                        WarScore.TryAdd(client.GuildID, _NewList);
                                    }
                                }
                                else // NẾu ko phỉa người có thể tham gia chiến trận
                                {
                                    //  LogManager.WriteLog(LogTypes.GuildWarManager, "NGƯỜI CHƠI NÀY KHÔNG NẰM TRONG KHU VỰC MAP TRANH ĐOẠT :" + client.RoleName + "| ROLEID " + client.RoleID + "| GUILDNAME :" + client.GuildName + "|MAPCODE :" + client.CurrentMapCode);
                                    // Nhét silient vào mồm
                                    client.ForbidUsingSkill = true;
                                    client.Camp = -1;
                                    client.TempTitle = "";
                                    client.PKMode = (int)PKMode.Peace;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.GuildWarManager, "[" + client.RoleName + "]OnEnterMap BUG :" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Kích hoạt sự kiện khi người chơi rời bản đồ
        /// </summary>
        /// <param name="client"></param>
        public void OnLeaverMap(KPlayer client, GameMap toMap)
        {
            // Nếu đang trong thời gian Battle xảy ra
            if (BATTLESTATE <= GUILDWARDSTATUS.STATUS_CLEAR)
            {
                //Khả năng sẽ spam nếu cần

                // Nếu thằng này đang trong bản đồ tranh đoạt

                TotalMapWillBeWar.TryGetValue(client.CurrentMapCode, out GuildWarMap _MapOutValue);
                if (_MapOutValue != null)
                {
                    // mà thằng này cso bang
                    if (client.GuildID > 0)
                    {
                        // Nếu như bản đồ muốn tới lại ko phải bản đồ tranh đoạt thì trả lại camp bình thường cho nó
                        if (!TotalMapWillBeWar.TryGetValue(toMap.MapCode, out GuildWarMap _OutValue))
                        {
                            client.PKMode = (int)PKMode.Peace;
                            // Mở khóa nếu là người chơi thường
                            client.ForbidUsingSkill = false;
                            this.ChangeState(client, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cập nhật điểm mỗi 5 giây
        /// </summary>
        public void NotifyToAllEvery5Secon()
        {
            try
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "DO NotifyToAllEvery5Secon");

                // lấy ra tộc
                foreach (KeyValuePair<int, ConcurrentDictionary<int, GuildWarPlayer>> entry in WarScore)
                {
                    /// Lấy ra  danh sách player
                    ConcurrentDictionary<int, GuildWarPlayer> TotalPlayer = entry.Value;

                    // send notify loop cho toàn bộ ae
                    foreach (GuildWarPlayer _Notify in TotalPlayer.Values)
                    {
                        SendNotifyLoop(_Notify);
                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "DO NotifyToAllEvery5Secon END");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void SendNotifyLoop(GuildWarPlayer player)
        {
            long SEC = (LastTick + _Config.Activity.WAR_DULATION * 1000) - TimeUtil.NOW();

            int FinalSec = (int)(SEC / 1000);

            G2C_EventNotification _Notify = new G2C_EventNotification();

            _Notify.EventName = "Tranh đoạt lãnh thổ";

            _Notify.ShortDetail = "TIME|" + FinalSec;

            _Notify.TotalInfo = new List<string>();

            _Notify.TotalInfo.Add("Tích lũy cá nhân :" + player.Score);

            _Notify.TotalInfo.Add("Phá long trụ :" + player.DestroyCount);

            _Notify.TotalInfo.Add("Giết địch :" + player.KillCount);

            //  _Notify.TotalInfo.Add("Liên trảm :" + player.MaxKillSteak);

            if (player.CurentRank != -1)
            {
                _Notify.TotalInfo.Add("Hạng Hiện Tại :" + player.CurentRank);
            }
            else
            {
                _Notify.TotalInfo.Add("Hạng Hiện Tại : Chưa xếp hạng");
            }

            if (player._Player.IsOnline())
            {
                player._Player.SendPacket<G2C_EventNotification>((int)TCPGameServerCmds.CMD_KT_EVENT_NOTIFICATION, _Notify);
            }
            else
            {
                //Console.WriteLine("OFFLINE");
            }
        }

        #endregion Notify To Player

        /// <summary>
        /// Đổi state sang 50 có phím bấm Công báo ở góc và thời gian còn lại
        /// </summary>
        /// <param name="Player"></param>
        public void ChangeState(KPlayer Player, int State)
        {
            G2C_EventState _State = new G2C_EventState();

            _State.EventID = 50;
            _State.State = State;
            if (Player.IsOnline())
            {
                Player.SendPacket<G2C_EventState>((int)TCPGameServerCmds.CMD_KT_EVENT_STATE, _State);
            }
            else
            {
                //Console.WriteLine("OFFLINE");
            }
        }
    }
}