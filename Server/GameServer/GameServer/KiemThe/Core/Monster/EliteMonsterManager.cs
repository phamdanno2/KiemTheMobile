using GameServer.Core.Executor;
using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.KiemThe;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using static GameServer.Logic.Monster;

namespace GameServer.Logic
{
    /// <summary>
    ///  Quản lý tinh anh
    /// </summary>
    public class EliteMonsterManager
    {
        private static EliteMonsterManager instance = new EliteMonsterManager();

        private BossModel _Boss = new BossModel();

        public static string BossPath = "Config/KT_Monster/BossModel.xml";

        public static EliteMonsterManager getInstance()
        {
            return instance;
        }

        public List<int> IsNotify = new List<int>();

        public void ClearRecore()
        {
            IsNotify.Clear();
        }

        /// <summary>
        /// Dict quản lý việc ghi lại nhật ký chết của quái
        /// </summary>
        public static ConcurrentDictionary<int, MonsterRecoreDie> MonsterDieCount = new ConcurrentDictionary<int, MonsterRecoreDie>();

        public static ConcurrentDictionary<int, EliteMark> EliteRecoredDie = new ConcurrentDictionary<int, EliteMark>();

        public static ConcurrentDictionary<int, List<int>> RandomMap = new ConcurrentDictionary<int, List<int>>();



        public void SetupAllBoss()
        {

            string Files = Global.GameResPath(BossPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(BossModel));
                _Boss = serializer.Deserialize(stream) as BossModel;

            }
        }



        ///// <summary>
        ///// Thời điểm lần trước kiểm tra tạo quái
        ///// </summary>
        //private readonly ConcurrentDictionary<int, long> LastCheckCreateMonsterTicks = new ConcurrentDictionary<int, long>();

        /// <summary>
        /// Tạo quái tinh anh hoặc thủ lĩnh
        /// </summary>
        /// <param name="_Monster"></param>
        /// <param name="MonsterLevel"></param>
        /// <param name="ZoneID"></param>
        public void CreateMonster(Monster _Monster, int MonsterLevel, int ZoneID)
        {


            // Nếu mà cấp quái là 1 hoặc 2 thì mới check xem quái đã chết hay chưa
            if (MonsterLevel == 1 || MonsterLevel == 2)
            {
                /// Danh sách DynArea xung quanh
                List<KDynamicArea> dynAreas = KTDynamicAreaManager.GetMapDynamicAreas(_Monster.CurrentMapCode);
                /// Nếu có bất kỳ lửa trại nào xung quanh
                if (dynAreas.Any((dynArea) =>
                {
                /// Nếu không phải lửa trại
                if (dynArea.ResID != 20022)
                    {
                        return false;
                    }
                /// Khoảng cách
                double distance = Global.GetTwoPointDistance(_Monster.CurrentPos, dynArea.CurrentPos);
                /// Trong khoảng 600
                return distance <= 600;
                }))
                {
                    return;
                }
            }

            MonsterAIType aiType = MonsterAIType.Normal;

            aiType = MonsterAIType.Elite;

            /// Ngũ hành
            KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;

            int RandomSeri = KTGlobal.GetRandomNumber(1, 5);

            if (_Monster.m_Series != KE_SERIES_TYPE.series_none)
            {
                series = (KE_SERIES_TYPE)RandomSeri;
            }
            else
            {
                series = _Monster.m_Series;
            }

            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;

            int RandomDir = KTGlobal.GetRandomNumber(0, 7);

            dir = (KiemThe.Entities.Direction)RandomDir;

            int Level = _Monster.MonsterInfo.Level;

            double PosX = _Monster.CurrentPos.X;
            double PosY = _Monster.CurrentPos.Y;

            int HpBase = _Monster.MonsterInfo.MaxHP;

            int FinalHp = 0;

            string MonsterName = _Monster.MonsterInfo.Name;

            // Nếu là tinh anh máu gấp 80 lần
            if (MonsterLevel == 1)
            {
                aiType = MonsterAIType.Elite;
                FinalHp = HpBase * 40;
                MonsterName = KTGlobal.CreateStringByColor("Tinh Anh - " + MonsterName, ColorType.Blue);
            }
            else if (MonsterLevel == 2) // Nếu là thủ lĩnh máu gấp 150 lần
            {
                aiType = MonsterAIType.Leader;
                FinalHp = HpBase * 70;
                MonsterName = KTGlobal.CreateStringByColor("Thủ Lĩnh - " + MonsterName, ColorType.Pure);
            }
            else if (MonsterLevel == 3) // Nếu là bosss
            {
                aiType = MonsterAIType.Boss;
                FinalHp = -1;
                MonsterName = KTGlobal.CreateStringByColor(MonsterName, ColorType.Importal);
            }

            // Kiểm tra xem ông này có trong list chưa nếu có rồi thì đánh dấu là chưa chết
            if(EliteRecoredDie.TryGetValue(ZoneID,out EliteMark _Mark))
            {
                _Mark.IsBoss = false;
                _Mark.Monster = _Monster;
                _Mark.IsDie = false;

                EliteRecoredDie[ZoneID] = _Mark;
            }
            else
            {
                EliteMark _NewMark = new EliteMark();
                _NewMark.Monster = _Monster;
                _NewMark.IsDie = false;
                _NewMark.IsBoss = false;
                // Nếu chưa có trong danh sách thì tạo mới
                EliteRecoredDie.TryAdd(ZoneID, _NewMark);
            }



            GameManager.MonsterZoneMgr.AddDynamicMonsters(_Monster.CurrentMapCode, _Monster.MonsterInfo.ExtensionID, -1, 1, (int)PosX, (int)PosY, MonsterName, "", FinalHp, Level, dir, series, aiType, -1, -1, -1, "ELITE|" + ZoneID, null);
        }


        public class EliteMark
        {
            public Monster Monster { get; set; }

            public bool IsBoss { get; set; }

            public bool IsDie { get; set; }

            public DateTime LastUpdate { get; set; }

        }

        public class MonsterRecoreDie
        {
            public int DieCount { get; set; }

            public Monster Monster { get; set; }

            public int ZoneID { get; set; }

            public long LastReviceElite { get; set; }

            public long LastReviceLeader { get; set; }
        }

        /// <summary>
        /// Kiểm tra có thể tạo Tinh anh hoặc thủ lĩnh không
        /// </summary>
        /// <param name="ZoneID"></param>
        /// <param name="Level"></param>
        /// <returns></returns>
        public bool CanRespanElite(int ZoneID, int Level)
        {
            if (!GameManager.ServerStarting)
            {
                long TimeNow = TimeUtil.NOW();

                if (Level == 3)
                {
                    if (EliteRecoredDie.TryGetValue(ZoneID, out EliteMark _Mark))
                    {
                        DateTime Now = DateTime.Now;
                        DateTime LastUdpate = _Mark.LastUpdate;

                        // nếu con boss này đã ra được hơn tiếng thì cho phép ra tiếp
                        if((Now-LastUdpate).TotalHours>1)
                        {
                            return true;
                        }

                        // Nếu mà chết rồi thì cho phép hồi sinh
                        if (_Mark.IsDie == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (MonsterDieCount.TryGetValue(ZoneID, out MonsterRecoreDie _Recore))
                    {
                        if (Level == 1)
                        {
                            /// nếu giết 50 con sẽ ra tinh anh
                            if (_Recore.DieCount > 30 && TimeNow - _Recore.LastReviceElite > 100000)
                            {
                                if (EliteRecoredDie.TryGetValue(ZoneID, out EliteMark _Mark))
                                {
                                    /// Nếu mà chết rồi thì cho phép hồi sinh
                                    if (_Mark.IsDie == true)
                                    {
                                        _Recore.LastReviceElite = TimeNow;
                                        /// khi cho tạo thủ lĩnh thì reset lại về 0 ddeerc ount lại
                                        /// _Recore.DieCount = 0;
                                        return true;
                                    }
                                    else
                                    {
                                        /// Không thì thôi
                                        return false;
                                    }
                                }
                                else
                                {
                                    ///_Recore.LastReviceElite = TimeNow;
                                    return true;
                                }
                            }
                        }

                        if (Level == 2)
                        {
                            /// nếu giết hơn 100 con sẽ ra thủ lĩnh
                            if (_Recore.DieCount > 50 && TimeNow - _Recore.LastReviceLeader > 120000)
                            {
                                if (EliteRecoredDie.TryGetValue(ZoneID, out EliteMark _Mark))
                                {
                                    /// Nếu mà chết rồi thì cho phép hồi sinh
                                    if (_Mark.IsDie == true)
                                    {
                                        _Recore.LastReviceLeader = TimeNow;
                                        /// khi cho tạo thủ lĩnh thì reset lại về 0 ddeerc ount lại
                                        _Recore.DieCount = 0;
                                        return true;
                                    }
                                    else
                                    {
                                        /// Không thì thôi
                                        return false;
                                    }
                                }
                                else
                                {
                                    ///_Recore.LastReviceLeader = TimeNow;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        //public bool BossRespan(int Mapcode, Monster BossInfo)
        //{
        //    bool CanReSwsapn = false;

        //    GameMap _Map = GameManager.MapMgr.GetGameMap(Mapcode);

        //    if (_Map != null)
        //    {
        //        // Nếu là máy không phỉa là liên máy chủ
        //        if (!GameManager.IsKuaFuServer)
        //        {
        //            // Nếu là bản đồ liên máy chủ thì không ra boss
        //            if (KuaFuMapManager.getInstance().IsKuaFuMap(Mapcode))
        //            {
        //                return false;
        //            }

        //        }

        //        // Nếu đây là liên máy chủ chắc chắn ra
        //        if (GameManager.IsKuaFuServer)
        //        {
        //            LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]LIÊN MÁY CHỦ CHẮC CHẮN SẼ RA BOSS :" + BossInfo.MonsterInfo.Name);

        //            KTGlobal.SendSystemChat("Cao thủ võ lâm " + KTGlobal.CreateStringByColor(BossInfo.MonsterInfo.Name, ColorType.Importal) + " đã xuất hiện tại [" + _Map.MapName + "] các nhân sĩ hãy tìm đến và tiêu diệt");
        //            // Nếu đã có trong list thì đánh dấu lại là nó chưa chết
        //            if (EliteRecoredDie.TryGetValue(BossInfo.MonsterInfo.ExtensionID, out EliteMark _Mark))
        //            {
        //                //Set lại boss để tính toán cho chuẩn
        //                _Mark.Monster = BossInfo;
        //                _Mark.IsDie = false;
        //                _Mark.IsBoss = true;
        //                EliteRecoredDie[BossInfo.MonsterInfo.ExtensionID] = _Mark;
        //            }
        //            else
        //            {
        //                EliteMark _NewMark = new EliteMark();
        //                _NewMark.Monster = BossInfo;
        //                _NewMark.IsDie = false;
        //                _NewMark.IsBoss = true;
        //                // Nếu chưa có thì tạo mới và đánh dấu nó chưa chết
        //                EliteRecoredDie.TryAdd(BossInfo.MonsterInfo.ExtensionID, _NewMark);
        //            }

        //            return true;
        //        }
        //        else
        //        {

        //            LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]Check xem boss có thể hồi sinh không :" + BossInfo.MonsterInfo.Name);


        //           // Nếu là boss thưởng thì kiểm tra xem trong dánh ách bản đồ ra ngẫu nhiên đã có nó chưa
        //            //if (RandomMap.TryGetValue(BossInfo.MonsterInfo.ExtensionID, out List<int> Value))
        //            //{
        //            //   // Nếu map này đã nằm trong danh sách thì chắc chắn ko ra
        //            //    if (Value.Contains(_Map.MapCode))
        //            //    {
        //            //        LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]BẢN ĐỒ  NÀY ĐÃ RA RỒI NÊN KO RA NỮA :" + BossInfo.MonsterInfo.Name);


        //            //        return false;
        //            //    }
        //            //    else
        //            //    {
        //            //      //  Cho map này vào danh sách đen lần sau sẽ không ra tiếp
        //            //        Value.Add(_Map.MapCode);
        //            //    }


        //            //}
        //            //else
        //            //{
        //            //    List<int> _MapCode = new List<int>();
        //            //    _MapCode.Add(_Map.MapCode);

        //            //    RandomMap.TryAdd(BossInfo.MonsterInfo.ExtensionID, _MapCode);
        //            //}



        //            if (CanRespanElite(BossInfo.MonsterInfo.ExtensionID, 3))
        //            {
        //                LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]Check xem boss có thể hồi sinh không :" + BossInfo.MonsterInfo.Name + "==> Có thể hồi sinh vì đã chết");


        //                KTGlobal.SendSystemEventNotification("Cao thủ võ lâm " + KTGlobal.CreateStringByColor(BossInfo.MonsterInfo.Name, ColorType.Importal) + " đã xuất hiện tại [" + _Map.MapName + "] các nhân sĩ hãy tìm đến và tiêu diệt");


        //                KTGlobal.SendSystemChat("Cao thủ võ lâm " + KTGlobal.CreateStringByColor(BossInfo.MonsterInfo.Name, ColorType.Importal) + " đã xuất hiện tại [" + _Map.MapName + "] các nhân sĩ hãy tìm đến và tiêu diệt");

        //                // Nếu đã có trong list thì đánh dấu lại là nó chưa chết
        //                if (EliteRecoredDie.TryGetValue(BossInfo.MonsterInfo.ExtensionID, out EliteMark _Mark))
        //                {
        //                    _Mark.Monster = BossInfo;
        //                    _Mark.IsDie = false;
        //                    _Mark.IsBoss = true;
        //                    _Mark.LastUpdate = DateTime.Now;
        //                    EliteRecoredDie[BossInfo.MonsterInfo.ExtensionID] = _Mark;
        //                }
        //                else
        //                {
        //                    EliteMark _NewMark = new EliteMark();
        //                    _NewMark.Monster = BossInfo;
        //                    _NewMark.IsDie = false;
        //                    _NewMark.LastUpdate = DateTime.Now;
        //                    _NewMark.IsBoss = true;
        //                    // Nếu chưa có thì tạo mới và đánh dấu nó chưa chết
        //                    EliteRecoredDie.TryAdd(BossInfo.MonsterInfo.ExtensionID, _NewMark);
        //                }
        //                //  CreateMonster(BossInfo, BossInfo.m_Level, BossInfo.MonsterInfo.ExtensionID);

        //                CanReSwsapn = true;
        //            }
        //            else
        //            {
        //                //try
        //                //{
        //                //    List<object> objs = GameManager.MonsterMgr.GetObjectsByMap(Mapcode);
        //                //    if (objs != null)
        //                //    {
        //                //        /// Duyệt danh sách
        //                //        foreach (object obj in objs)
        //                //        {
        //                //            if (obj is Monster)
        //                //            {
        //                //                Monster _MonsterBoss = ((Monster)obj);
        //                //                if(BossInfo.MonsterInfo.ExtensionID == _MonsterBoss.MonsterInfo.ExtensionID)
        //                //                {
        //                //                    _MonsterBoss.MonsterZoneNode?.DestroyMonster(obj as Monster);
        //                //                }
        //                //            }
        //                //        }
        //                //    }
        //                //}
        //                //catch(Exception ex)
        //                //{
        //                //    LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]CODE RA BOSS MỚI TOÁC :" + BossInfo.MonsterInfo.Name + "BUG :" +ex.ToString());
        //                //}

        //                LogManager.WriteLog(LogTypes.GameMapEvents, "[" + _Map.MapName + "]Check xem boss có thể hồi sinh không :" + BossInfo.MonsterInfo.Name + "==> Không thể hồi sinh vì vẫn sống");
        //            }
        //        }
        //    }

        //    return CanReSwsapn;
        //}

        /// <summary>
        /// Funtion ghi laị nhật ký quái chết theo bãi
        /// </summary>
        /// <param name="ZoneID"></param>
        /// <param name="MonsterID"></param>
        public void AddDieRecore(int ZoneID, Monster Monsterinput)
        {
            if (!GameManager.ServerStarting)
            {
                if (MonsterDieCount.TryGetValue(ZoneID, out MonsterRecoreDie _Recore))
                {
                    _Recore.DieCount = _Recore.DieCount + 1;
                }
                else
                {
                    MonsterRecoreDie NewRecore = new MonsterRecoreDie();
                    NewRecore.DieCount = 1;
                    NewRecore.Monster = Monsterinput;
                    NewRecore.ZoneID = ZoneID;

                    MonsterDieCount[ZoneID] = NewRecore;
                }
            }
        }

        public bool showdown()
        {
            return true;
        }

        public bool startup()
        {
            ScheduleExecutor2.Instance.scheduleExecute(new NormalScheduleTask("ELITERWPAN", ProseccTick), 5 * 1000, 2000);
		///	ScheduleExecutor2.Instance.scheduleExecute(new NormalScheduleTask("ELITERWPAN", ProseccTick), 15 * 1000, 2000);

            return true;
        }

        /// <summary>
        ///  Hàm thực hiện quản lý đám tinh anh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProseccTick(object sender, EventArgs e)
        {
            List<int> keys = MonsterDieCount.Keys.ToList();
            // Check lại lịch sử monster đã chết
            foreach (int key in keys)
            {
                if (!MonsterDieCount.TryGetValue(key, out MonsterRecoreDie value))
                {
                    continue;
                }

                if (CanRespanElite(value.ZoneID, 1))
                {
                    CreateMonster(value.Monster, 1, value.ZoneID);
                }

                if (CanRespanElite(value.ZoneID, 2))
                {
                    CreateMonster(value.Monster, 2, value.ZoneID);
                }
            }


            //Thực hiện hành vi ra boss theo thời gian
            List<RespanBoss> TotalBoss = _Boss.TotalBoss;

            DateTime TimeNow = DateTime.Now;

            foreach(RespanBoss boss in TotalBoss)
            {

                var findbosscanrespan = boss._TimeRespan.Where(x => x.Hour == TimeNow.Hour && x.Min == TimeNow.Minute && x.IsRespan == false).FirstOrDefault();

                if (findbosscanrespan != null)
                {
                    //Đánh dấu là đã ra
                    findbosscanrespan.IsRespan = true;

                    int Random = KTGlobal.GetRandomNumber(0, boss.ListMap.Count - 1);

                    Postion _Post = boss.ListMap[Random];

                    KTGlobal.SendSystemEventNotification("Cao thủ võ lâm <b>" + KTGlobal.CreateStringByColor(boss.BossName, ColorType.Importal) + "</b> đã xuất hiện tại <b>" + KTGlobal.CreateStringByColor(_Post.MapName,ColorType.Green) + "</b> các nhân sĩ hãy tìm đến và tiêu diệt");

                   // KTGlobal.SendSystemChat("Cao thủ võ lâm " + KTGlobal.CreateStringByColor(boss.BossName, ColorType.Importal) + " đã xuất hiện tại [" + _Post.MapName + "] các nhân sĩ hãy tìm đến và tiêu diệt");

                    GameManager.MonsterZoneMgr.AddDynamicMonsters(_Post.MapCode, boss.BossID, -1, 1, _Post.PosX, _Post.PosY, boss.BossName, "", -1, -1, 0, KE_SERIES_TYPE.series_none, MonsterAIType.Boss, -1, -1, -1, "", (monster) => {


                        if (EliteRecoredDie.TryGetValue(monster.MonsterInfo.ExtensionID, out EliteMark _Mark))
                        {
                            _Mark.Monster = monster;
                            _Mark.IsDie = false;
                            _Mark.IsBoss = true;
                            _Mark.LastUpdate = DateTime.Now;
                            EliteRecoredDie[monster.MonsterInfo.ExtensionID] = _Mark;
                        }
                        else
                        {
                            EliteMark _NewMark = new EliteMark();
                            _NewMark.Monster = monster;
                            _NewMark.IsDie = false;
                            _NewMark.LastUpdate = DateTime.Now;
                            _NewMark.IsBoss = true;
                            // Nếu chưa có thì tạo mới và đánh dấu nó chưa chết
                            EliteRecoredDie.TryAdd(monster.MonsterInfo.ExtensionID, _NewMark);
                        }

                    });

                }

            }


            try
            {
                // Tính toán xem boss thuộc về ai

                if (EliteRecoredDie.Count > 0)
                {

                    //Lấy ra danh sách boss
                    List<EliteMark> _TotalMark = EliteRecoredDie.Values.Where(x => x.IsBoss == true).ToList();

                    // Remove tất cả các damage mà 30s không được update

                    long Now = TimeUtil.NOW();


                    foreach (EliteMark _Mark in _TotalMark)
                    {
                        Monster _Monster = _Mark.Monster;

                        if (_Monster.DamageTakeRecord.Count > 0)
                        {
                            //Lấy ra toàn bộ các damge mà 30s chưa cập nhật
                            List<DamgeGroup> NeedRessetDamge = _Monster.DamageTakeRecord.Values.Where(x => Now - x.LastUpdateTime > 30000).ToList();

                            _Monster.ResetDamge(NeedRessetDamge);
                        }


                    }


                    _TotalMark = EliteRecoredDie.Values.Where(x => x.IsBoss == true).ToList();


                    foreach (EliteMark _Mark in _TotalMark)
                    {
                        Monster _Monster = _Mark.Monster;

                        if (_Monster.DamageTakeRecord.Count > 0)
                        {
                            var FindTop = _Monster.DamageTakeRecord.Values.OrderByDescending(x => x.TotalDamage).FirstOrDefault();

                            if (FindTop.TotalDamage > 0)
                            {
                                long ResetTime = (30000 - (Now - FindTop.LastUpdateTime)) / 1000;

                                if (FindTop.IsTeam)
                                {



                                    KPlayer _Player = KTTeamManager.GetTeamLeader(FindTop.ID);
                                    if (_Player != null)
                                    {
                                        _Monster.Title = "<b><color=#00ff2a>(Thuộc đội: " + _Player.RoleName + ", còn: " + ResetTime + "s)</color></b>";
                                    }

                                }
                                else
                                {

                                    KPlayer client = GameManager.ClientMgr.FindClient(FindTop.ID);
                                    if (client != null)
                                    {
                                        _Monster.Title = "<b><color=#00ff2a>(Thuộc về: " + client.RoleName + ", còn: " + ResetTime + "s)</color></b>";

                                    }


                                }
                            }
                            else
                            {
                                _Monster.Title = "(Không thuộc về ai)";

                                DateTime CurentTime = DateTime.Now;
                                DateTime LastUdpate = _Mark.LastUpdate;

                                // nếu con boss này đã ra được hơn tiếng thì cho phép ra tiếp
                                if ((CurentTime - LastUdpate).TotalHours > 3)
                                {
                                    if(EliteRecoredDie.TryRemove(_Monster.MonsterInfo.ExtensionID, out EliteMark _Elite))
                                    {
                                        _Monster.MonsterZoneNode?.DestroyMonster(_Monster);
                                    }

                                }
                            }


                        }
                        else
                        {
                            _Monster.Title = "(Không thuộc về ai)";

                            DateTime CurentTime = DateTime.Now;
                            DateTime LastUdpate = _Mark.LastUpdate;

                            // nếu con boss này đã ra được hơn tiếng thì cho phép ra tiếp
                            if ((CurentTime - LastUdpate).TotalHours > 3)
                            {
                                if (EliteRecoredDie.TryRemove(_Monster.MonsterInfo.ExtensionID, out EliteMark _Elite))
                                {
                                    _Monster.MonsterZoneNode?.DestroyMonster(_Monster);
                                }

                            }
                        }
                        //Lấy ra toàn bộ các damge mà 30s chưa cập nhật


                    }



                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }


        }


        /// <summary>
        /// Đánh dấu quái đã chết để quái hồi sinh tiếp
        /// </summary>
        /// <param name="monster"></param>
        public void OnMonsterDie(Monster monster)
        {
            try
            {
                if (!string.IsNullOrEmpty(monster.Tag))
                {
                    // Nếu con quái chết là ELITE thì update lại DICT là nó đã chết
                    if (monster.Tag.Contains("ELITE"))
                    {
                        int ZoneID = Int32.Parse(monster.Tag.Split('|')[1]);

                        if (EliteRecoredDie.TryGetValue(ZoneID, out EliteMark _Mark))
                        {
                            _Mark.IsDie = true;
                            // Đánh dấu là nó đã chết
                            EliteRecoredDie[ZoneID] = _Mark;
                        }
                    }
                    else
                    {
                        if (EliteRecoredDie.TryGetValue(monster.MonsterInfo.ExtensionID, out EliteMark _Mark))
                        {
                            _Mark.IsDie = true;
                            // Đánh dấu là nó đã chết
                            EliteRecoredDie[monster.MonsterInfo.ExtensionID] = _Mark;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogManager.WriteLog(LogTypes.GameMapEvents, "BUG RESPAN :" + exx.ToString());
            }
        }

    }
}