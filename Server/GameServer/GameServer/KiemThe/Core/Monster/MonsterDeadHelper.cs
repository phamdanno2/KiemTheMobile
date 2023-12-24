using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.Logic.Manager.Battle;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Classs thực hiện toàn bộ các sự kiện khi quái chết
    /// </summary>
    public class MonsterDeadHelper
    {
        // Exp trên 1 level
        public static Dictionary<int, int> DefaultExpPerLevel = new Dictionary<int, int>();

        /// <summary>
        /// Hồi phục trên level
        /// </summary>
        public static Dictionary<int, int> LifeReplenish = new Dictionary<int, int>();

        /// <summary>
        /// Chính xác trên 1 level
        /// </summary>
        public static Dictionary<int, int> AR = new Dictionary<int, int>();

        /// <summary>
        /// Phòng thủ trên 1 level
        /// </summary>
        public static Dictionary<int, int> Defense = new Dictionary<int, int>();

        /// <summary>
        /// MIn Dagme
        /// </summary>
        public static Dictionary<int, int> MinDamage = new Dictionary<int, int>();

        /// <summary>
        /// Max Dagme
        /// </summary>
        public static Dictionary<int, int> MaxDamage = new Dictionary<int, int>();

        public static Dictionary<int, int> Resist = new Dictionary<int, int>();

        public static Dictionary<int, int> Damage = new Dictionary<int, int>();

        public static Dictionary<int, int> BossExpPerLevel = new Dictionary<int, int>();

        public static void ExpInit()
        {
            DefaultExpPerLevel.Add(1, 50);
            DefaultExpPerLevel.Add(9, 50);

            DefaultExpPerLevel.Add(10, 100);
            DefaultExpPerLevel.Add(19, 100);

            DefaultExpPerLevel.Add(20, 150);
            DefaultExpPerLevel.Add(29, 150);

            DefaultExpPerLevel.Add(30, 200);
            DefaultExpPerLevel.Add(39, 200);

            DefaultExpPerLevel.Add(40, 300);
            DefaultExpPerLevel.Add(49, 300);

            DefaultExpPerLevel.Add(50, 400);
            DefaultExpPerLevel.Add(59, 400);

            DefaultExpPerLevel.Add(60, 500);
            DefaultExpPerLevel.Add(69, 500);

            DefaultExpPerLevel.Add(70, 650);
            DefaultExpPerLevel.Add(79, 650);

            DefaultExpPerLevel.Add(80, 800);
            DefaultExpPerLevel.Add(89, 800);

            DefaultExpPerLevel.Add(90, 850);
            DefaultExpPerLevel.Add(99, 850);

            DefaultExpPerLevel.Add(100, 900);
            DefaultExpPerLevel.Add(109, 900);

            DefaultExpPerLevel.Add(110, 950);
            DefaultExpPerLevel.Add(119, 950);

            DefaultExpPerLevel.Add(120, 1000);
            DefaultExpPerLevel.Add(129, 1000);

            DefaultExpPerLevel.Add(130, 1100);
            DefaultExpPerLevel.Add(139, 1100);

            DefaultExpPerLevel.Add(140, 1250);
            DefaultExpPerLevel.Add(150, 1250);

            LifeReplenish.Add(1, 5);
            LifeReplenish.Add(10, 10);
            LifeReplenish.Add(20, 65);
            LifeReplenish.Add(60, 250);
            LifeReplenish.Add(100, 750);
            LifeReplenish.Add(150, 750);

            AR.Add(1, 30);
            AR.Add(10, 70);
            AR.Add(100, 700);
            AR.Add(150, 1050);

            Defense.Add(1, 5);
            Defense.Add(10, 5);
            Defense.Add(11, 10);
            Defense.Add(100, 200);
            Defense.Add(150, 300);

            Resist.Add(1, 20);
            Resist.Add(9, 28);
            Resist.Add(10, 65);
            Resist.Add(100, 245);
            Resist.Add(150, 245);

            //Damage.Add(1, 1);
            //Damage.Add(9, 10);
            //Damage.Add(10, 20);
            //Damage.Add(30, 45);
            //Damage.Add(60, 120);
            //Damage.Add(100, 230);
            //Damage.Add(150, 230);

            Damage.Add(1, 1);
            Damage.Add(9, 2);
            Damage.Add(10, 5);
            Damage.Add(30, 10);
            Damage.Add(60, 30);
            Damage.Add(100, 50);
            Damage.Add(150, 50);
        }

        public static int GetMaxDamgePerLevel(int MonsterLevel)
        {
            return 10;
        }

        public static int GetMinDamgePerLevel(int MonsterLevel)
        {
            return 1;
        }

        public static int GetDefenseLevel(int MonsterLevel)
        {
            if (Defense.TryGetValue(MonsterLevel, out int Key))
            {
                return Key;
            }

            var findmin = Defense.OrderByDescending(x => x.Key).Where(x => x.Key <= MonsterLevel).FirstOrDefault();

            var findmax = Defense.OrderBy(x => x.Key).Where(x => x.Key >= MonsterLevel).FirstOrDefault();

            double LeveDif = findmax.Key - findmin.Key;

            double ValueDif = findmax.Value - findmin.Value;

            double PerValue = (ValueDif / LeveDif);

            int DamgeFind = MonsterLevel - findmin.Key;

            double ValueAdd = (PerValue * DamgeFind) + findmin.Value;

            return (int)ValueAdd;
        }

        public static int GetARLevel(int MonsterLevel)
        {
            if (AR.TryGetValue(MonsterLevel, out int Key))
            {
                return Key;
            }

            var findmin = AR.OrderByDescending(x => x.Key).Where(x => x.Key <= MonsterLevel).FirstOrDefault();

            var findmax = AR.OrderBy(x => x.Key).Where(x => x.Key >= MonsterLevel).FirstOrDefault();

            double LeveDif = findmax.Key - findmin.Key;

            double ValueDif = findmax.Value - findmin.Value;

            double PerValue = (ValueDif / LeveDif);

            int DamgeFind = MonsterLevel - findmin.Key;

            double ValueAdd = (PerValue * DamgeFind) + findmin.Value;

            return (int)ValueAdd;
        }

        public static int GetLifeReplenishLevel(int MonsterLevel)
        {
            if (LifeReplenish.TryGetValue(MonsterLevel, out int Key))
            {
                return Key;
            }

            var findmin = LifeReplenish.OrderByDescending(x => x.Key).Where(x => x.Key <= MonsterLevel).FirstOrDefault();

            var findmax = LifeReplenish.OrderBy(x => x.Key).Where(x => x.Key >= MonsterLevel).FirstOrDefault();

            double LeveDif = findmax.Key - findmin.Key;

            double ValueDif = findmax.Value - findmin.Value;

            double PerValue = (ValueDif / LeveDif);

            int DamgeFind = MonsterLevel - findmin.Key;

            double ValueAdd = (PerValue * DamgeFind) + findmin.Value;

            return (int)ValueAdd;
        }

        public static int GetResistPerLevel(int MonsterLevel)
        {
            if (Resist.TryGetValue(MonsterLevel, out int Key))
            {
                return Key;
            }

            var findmin = Resist.OrderByDescending(x => x.Key).Where(x => x.Key <= MonsterLevel).FirstOrDefault();

            var findmax = Resist.OrderBy(x => x.Key).Where(x => x.Key >= MonsterLevel).FirstOrDefault();

            double LeveDif = findmax.Key - findmin.Key;

            double ValueDif = findmax.Value - findmin.Value;

            double PerValue = (ValueDif / LeveDif);

            int DamgeFind = MonsterLevel - findmin.Key;

            double ValueAdd = (PerValue * DamgeFind) + findmin.Value;

            return (int)ValueAdd;
        }

        public static double GetDamgeBYSeries(DAMAGE_TYPE DamgeType, KE_SERIES_TYPE Series)
        {
            double OutputData = 1;

            switch (Series)
            {
                case KE_SERIES_TYPE.series_earth:
                    if (DamgeType != DAMAGE_TYPE.damage_light)
                    {
                        return 0.5;
                    }
                    break;

                case KE_SERIES_TYPE.series_fire:
                    if (DamgeType != DAMAGE_TYPE.damage_fire)
                    {
                        return 0.5;
                    }
                    break;

                case KE_SERIES_TYPE.series_metal:
                    if (DamgeType != DAMAGE_TYPE.damage_physics)
                    {
                        return 0.5;
                    }
                    break;

                case KE_SERIES_TYPE.series_wood:
                    if (DamgeType != DAMAGE_TYPE.damage_poison)
                    {
                        return 0.5;
                    }
                    break;

                case KE_SERIES_TYPE.series_water:
                    if (DamgeType != DAMAGE_TYPE.damage_cold)
                    {
                        return 0.5;
                    }
                    break;
            }
            return OutputData;
        }

        public static int GetDamagePerLevel(int MonsterLevel, DAMAGE_TYPE DamgeType, KE_SERIES_TYPE Series)
        {
            if (Damage.TryGetValue(MonsterLevel, out int Key))
            {
                return Key;
            }

            var findmin = Damage.OrderByDescending(x => x.Key).Where(x => x.Key <= MonsterLevel).FirstOrDefault();

            var findmax = Damage.OrderBy(x => x.Key).Where(x => x.Key >= MonsterLevel).FirstOrDefault();

            double LeveDif = findmax.Key - findmin.Key;

            double ValueDif = findmax.Value - findmin.Value;

            double PerValue = (ValueDif / LeveDif);

            int DamgeFind = MonsterLevel - findmin.Key;

            double ValueAdd = (PerValue * DamgeFind) + findmin.Value;

            double FinalValue = GetDamgeBYSeries(DamgeType, Series) * ValueAdd;

            return (int)ValueAdd;
        }

        /// Tính toán lượng kinh nghiệm nhận được khi người chơi giết quái
        public static int _CalcPlayerExpByLevel(int nExp, int nSelfLevel, int nTarLevel)
        {
            int nSubLevel = nSelfLevel - nTarLevel;
            int nGetExp = 0;

            if (nSelfLevel >= 100)          // Nếu cấp bản thân mà trên 100
            {
                // Nếu mà Con quái có cấp >= 90 thì thôi ko giảm exp
                if (nTarLevel >= 90)
                    nGetExp = nExp;
                else
                    nGetExp = 1;
            }
            else if (nSubLevel < -13)       // (-~,-13)		Nếu mà đánh quái > 13 level thì EXP nhận được là 1
            {
                nGetExp = 1;
            }
            else if (nSubLevel < -5)        //Nếu đánh quái > 5 cấp thì EXP bị chia ra
            {
                nGetExp = nExp * (14 + nSubLevel) / 10;
            }
            else if (nSubLevel <= 5)        //Nếu đánh quái <= 5 cấp thì EXP giữ nguyên 100
            {
                nGetExp = nExp;
            }
            else if (nSubLevel <= 20)       //Nếu cấp bản thân mà > 20 cấp quái thì EXP bị chia ra
            {
                nGetExp = nExp * (30 - nSubLevel) / 25;
            }
            else                            //Các trường hợp còn lại EXP / 2.5
            {
                nGetExp = nExp * 2 / 5;
            }

            if (nGetExp <= 0)  // Nếu EXP chia ra nhỏ hơn 0 thì expx nhận được là 1
                nGetExp = 1;

            //-------------fix jackson thêm tỷ lệ EXP train quái
            if(ServerConfig.Instance.ExpRate > 0)
                nGetExp = (int)(nGetExp * ServerConfig.Instance.ExpRate);

            return nGetExp;
        }

        public static void CreateFireCampStep1(GameMap gameMap, int XPos, int YPos, KPlayer Host, int copySceneID = -1)
        {
            GrowPointXML _Config = new GrowPointXML();
            _Config.CollectTick = 10000;
            _Config.Name = "Đống củi";
            _Config.ResID = 20016;
            _Config.RespawnTime = -100;
            _Config.InteruptIfTakeDamage = false;
            _Config.ScriptID = -1;

            Action<KPlayer> ActionWork = (x) => CreateFireCampStep2(gameMap, Host, XPos, YPos, copySceneID);

            /// Tạo 1 lửa trại
            GrowPoint growPoint = new GrowPoint()
            {
                ID = KTGrowPointManager.AutoIndexManager.GetNewID(),
                Data = _Config,
                Name = _Config.Name,
                ObjectType = ObjectTypes.OT_GROWPOINT,
                MapCode = gameMap.MapCode,
                CurrentCopyMapID = copySceneID,
                CurrentPos = new System.Windows.Point(XPos, YPos),
                CurrentGrid = new System.Windows.Point(XPos / gameMap.MapGridWidth, YPos / gameMap.MapGridHeight),
                RespawnTime = _Config.RespawnTime,
                ScriptID = _Config.ScriptID,
                LifeTime = 300000,
                Alive = true,
                ConditionCheck = (player) =>
                {
                    if (Host.RoleID != player.RoleID)
                    {
                        if (Host.TeamID == -1)
                        {
                            PlayerManager.ShowNotification(player, "Lửa trại này thuộc về :" + Host.RoleName);
                            return false;
                        }
                        else if (Host.TeamID != -1)
                        {
                            if (Host.TeamID != player.TeamID)
                            {
                                PlayerManager.ShowNotification(player, "Lửa trại này thuộc về :" + Host.RoleName);
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                },

                GrowPointCollectCompleted = ActionWork,
            };
            /// Thực hiện tự động xóa
            growPoint.ProcessAutoRemoveTimeout();
            KTGrowPointManager.GrowPoints[growPoint.ID] = growPoint;
            /// Thêm điểm thu thập vào đối tượng quản lý map
            KTGrowPointManager.AddToMap(growPoint);
        }

        public static void CreateFireCampStep2(GameMap gameMap, KPlayer host, int PosX, int PosY, int copySceneID)
        {
            KDynamicArea dynArea = KTDynamicAreaManager.Add(gameMap.MapCode, copySceneID, "Lửa Trại", 20022, PosX, PosY, 60 * 5, 5, 500, -1, "LUATRAI_" + host.RoleID + "");
            dynArea.OnStayTick = (go) =>
            {
                if (go is KPlayer player)
                {
                    if (host.TeamID == ((KPlayer)go).TeamID && host.TeamID != -1)
                    {
                        MonsterDeadHelper.FireCampTick(gameMap, player);
                    }
                }
            };
        }

        private static void FireCampTick(GameMap gameMap, KPlayer player)
        {
            /// Nếu không thấy người chơi
            if (player == null)
            {
                return;
            }
            ExpProsec(gameMap, player);
        }

        private static void ExpProsec(GameMap gameMap, GameObject x)
        {
            int EXPGAIN = 2500;

            EXPGAIN += EXPGAIN * (x as KPlayer).m_nExpEnhancePercent / 100;
            EXPGAIN += EXPGAIN * (x as KPlayer).m_nExpAddtionP / 100;

            /// Nếu có rượu thì tăng x2 kinh nghiệm
            if (x.Buffs.HasBuff(378))
            {
                EXPGAIN *= 2;
            }

            GameManager.ClientMgr.ProcessRoleExperience(x as KPlayer, EXPGAIN, true, false, true, "LUATRAI");
        }

        /// <summary>
        /// Thực hiện tăng kinh nghiệm tu luyện mật tịch
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ExpInput"></param>
        public static void BookProseccExp(KPlayer player, int ExpInput, bool includeXiuLianZhu = true)
        {
            if (includeXiuLianZhu)
            {
                /// Nếu không có thời gian Tu Luyện
                if (player.XiuLianZhu_Exp <= 0)
                {
                    return;
                }
            }


            if (player.GoodsDataList != null)
            {
                GoodsData findbook = null;
                lock (player.GoodsDataList)
                {
                    findbook = player.GoodsDataList.Where(x => x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_BOOK).FirstOrDefault();
                }

                if (findbook != null)
                {
                    int LevelBook = 0;
                    if (findbook.OtherParams.TryGetValue(ItemPramenter.Pram_1, out string BookLevel))
                    {
                        LevelBook = Int32.Parse(BookLevel);
                    }
                    int BoookExp = 0;
                    if (findbook.OtherParams.TryGetValue(ItemPramenter.Pram_2, out string ExpLevel))
                    {
                        BoookExp = Int32.Parse(ExpLevel);
                    }

                    if (LevelBook < 100)
                    {
                        ItemData BookData = ItemManager.GetItemTemplate(findbook.GoodsID);
                        if (BookData != null)
                        {
                            int ExpMax = ItemManager.GetMaxbookEXP(LevelBook, BookData.Level);
                            int ExpCanGain = ItemManager.GetEXPEarnPerExp(ExpInput);
                            //int ExpCanGain = 1000; /// TẠM THỜI SET CHẾT
                            int MaxExpNext = 0;

                            // Nếu số exp nhận được mà lớn hơn 0
                            if (ExpCanGain > 0)
                            {
                                if (ExpCanGain + BoookExp > ExpMax)
                                {
                                    LevelBook = LevelBook + 1;
                                    BoookExp = (ExpCanGain + BoookExp) - ExpMax;

                                    MaxExpNext = ItemManager.GetMaxbookEXP(LevelBook, BookData.Level);
                                }
                                else
                                {
                                    BoookExp = BoookExp + ExpCanGain;
                                    MaxExpNext = ExpMax;
                                }

                                if (includeXiuLianZhu)
                                {
                                    /// Lấy Min của kinh nghiệm còn lại
                                    ExpCanGain = Math.Min(ExpCanGain, player.XiuLianZhu_Exp);
                                    /// Giảm kinh nghiệm mật tịch tương ứng
                                    player.XiuLianZhu_Exp -= ExpCanGain;
                                }


                                // LƯU LẠI BOOK

                                Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

                                TotalUpdate.Add(UPDATEITEM.ROLEID, player.RoleID);
                                TotalUpdate.Add(UPDATEITEM.ITEMDBID, findbook.Id);

                                Dictionary<ItemPramenter, string> OtherParams = new Dictionary<ItemPramenter, string>();

                                OtherParams.Add(ItemPramenter.Pram_1, LevelBook + "");
                                OtherParams.Add(ItemPramenter.Pram_2, BoookExp + "");
                                OtherParams.Add(ItemPramenter.Pram_3, MaxExpNext + "");

                                TotalUpdate.Add(UPDATEITEM.OTHER_PRAM, OtherParams);

                                bool IsWriterToDb = false;

                                long Now = TimeUtil.NOW();

                                // Nếu train quái liên tục thì phải 120s mới được ghi vào DB 1 lần tránh spam db
                                if (Now - player.LastBookExpTicks > 120000)
                                {
                                    player.LastBookExpTicks = Now;
                                    IsWriterToDb = true;
                                }

                                if (!ItemManager.UpdateItemPrammenter(TotalUpdate, findbook, player, "BOOKEXPCHANGE", IsWriterToDb))
                                {
                                    PlayerManager.ShowNotification(player, "Có lỗi khi nhận kinh nghiệm mật tịch");
                                }
                                else
                                {
                                    PlayerManager.ShowNotification(player, "Nhận được " + ExpCanGain + " kinh nghiệm tu luyện, kinh nghiệm còn lại " + player.XiuLianZhu_Exp + ".");

                                    /// Thêm kinh nghiệm cho kỹ năng tương ứng
                                    if (KSkill.GetSkillData(BookData.BookProperty.SkillID1) != null)
                                    {
                                        player.Skills.AddSkillExp(BookData.BookProperty.SkillID1, ExpCanGain);
                                    }
                                    if (KSkill.GetSkillData(BookData.BookProperty.SkillID2) != null)
                                    {
                                        player.Skills.AddSkillExp(BookData.BookProperty.SkillID2, ExpCanGain);
                                    }
                                    if (KSkill.GetSkillData(BookData.BookProperty.SkillID3) != null)
                                    {
                                        player.Skills.AddSkillExp(BookData.BookProperty.SkillID3, ExpCanGain);
                                    }
                                    if (KSkill.GetSkillData(BookData.BookProperty.SkillID4) != null)
                                    {
                                        player.Skills.AddSkillExp(BookData.BookProperty.SkillID4, ExpCanGain);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Thực hiện Logic khi quái chết
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="monster"></param>
        public static void ProcessMonsterDeadByClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, Monster monster)
        {

            // Gọi vào hàm quản lý quái
            EliteMonsterManager.getInstance().OnMonsterDie(monster);

            // Tổng lượng EXP nhận được khi đánh quái
            int MonsterExpRevice = monster.m_Experience;

            // Nếu monster không có EXP được thiết lập sẵn thực hiện tính toán đựa trên TABLE EXP mặc định
            if (MonsterExpRevice == 0)
            {
                var find = DefaultExpPerLevel.Where(x => x.Key > monster.m_Level).FirstOrDefault();

                MonsterExpRevice = find.Value;
            }

            int UYDANHCANGET = 0;

            int MoneyPresinal = 0;
            int MoneyGuild = 0;

            if ((int)monster.MonsterType == 3)
            {
                if (monster.m_Level == 55)
                {
                    MoneyPresinal = 3150;
                    MoneyGuild = 15750;
                    UYDANHCANGET = 2;
                }
                else if (monster.m_Level == 75)
                {
                    MoneyPresinal = 5250;
                    MoneyGuild = 26250;
                    UYDANHCANGET = 3;
                }
                else if (monster.m_Level == 95)
                {
                    MoneyPresinal = 15750;
                    MoneyGuild = 78750;
                    UYDANHCANGET = 5;
                }
                MonsterExpRevice = MonsterExpRevice * 250;


                var FindTop = monster.DamageTakeRecord.Values.OrderByDescending(x => x.TotalDamage).FirstOrDefault();

                if (FindTop != null)
                {
                    if (FindTop.TotalDamage > 0)
                    {
                        if (!FindTop.IsTeam)
                        {
                            KPlayer _client = GameManager.ClientMgr.FindClient(FindTop.ID);
                            if (_client != null)
                            {
                                client = _client;
                            }
                        }
                        else
                        {
                            KPlayer _client = KTTeamManager.GetTeamLeader(FindTop.ID);
                            if (_client != null)
                            {
                                client = _client;
                            }
                        }

                    }
                }
            }

            if ((int)monster.MonsterType == 1 || (int)monster.MonsterType == 2 || (int)monster.MonsterType == 3)
            {
                GameMap Map = GameManager.MapMgr.DictMaps[monster.CurrentMapCode];

                // Nếu ở trong tống kim thì ko có lửa trại
                if (!Battel_SonJin_Manager.IsInBattle(client))
                {
                    CreateFireCampStep1(Map, (int)monster.CurrentPos.X, (int)monster.CurrentPos.Y, client, monster.CurrentCopyMapID);
                }
            }
            // TODO : Nếu là boss thì EXP X 250

            // NẾu tema có đội thì lấy ra
            if (client.TeamID != -1)
            {
                List<KPlayer> TotalMember = client.Teammates;

                // Tính ra số EXP Cuối cùng sau khi được X lên với rate của tổ đội
                double ExpRate = 1.0 * (100 + TotalMember.Count + 1) / 100;

                double nMainExp = (MonsterExpRevice * ExpRate);

                // Lấy ra tối đa số EXP có thể nhận từ tổng lượng sát thương gây lên quái
                int ToalExpCanGet = 90;


                // tính ra số EXP cuối cùng
                double ExpLess = (nMainExp * ToalExpCanGet) / 100;

                // Lấy ra số exp tối đa mà thành viên được SHARE nếu không phải thằng dứt điểm
                double ExpShare = (ExpLess * KTGlobal.KD_MAX_TEAMATE_EXP_SHARE) / 100;

                foreach (KPlayer member in TotalMember)
                {
                    // nếu thằng thành viên này mà khác với map của thằng host thì đéo được exp
                    if (member.MapCode != client.MapCode)
                    {
                        continue;
                    }
                    // Nếu mà không trong phạm vi quái chết thì đéo được EXP
                    if (!Global.InCircle(new Point(member.PosX, member.PosY), monster.CurrentPos, 2000))
                    {
                        continue;
                    }

                    // nếu đã chết thì đéo được EXP
                    if (member.IsDead())
                    {
                        continue;
                    }

                    int FinalExp = 0;

                    // Nếu không phải thằng đã lasthit quái
                    if (member.RoleID != client.RoleID)
                    {
                        FinalExp = _CalcPlayerExpByLevel((int)ExpShare, member.m_Level, monster.m_Level);
                        /// Kinh nghiệm nhận được từ đồng đội khi đánh quái
                        if (member.m_nShareExpP > 0)
                        {
                            /// Cộng thêm lượng kinh nghiệm tương ứng từ đồng đội khi đánh quái
                            FinalExp += FinalExp * member.m_nShareExpP / 100;
                        }
                    }
                    else // Nếu là thằng đã lasthit quái
                    {
                        FinalExp = _CalcPlayerExpByLevel((int)ExpLess, member.m_Level, monster.m_Level);
                    }


                    if (UYDANHCANGET > 0)
                    {
                        member.Prestige = member.Prestige + UYDANHCANGET;
                    }

                    if (MoneyPresinal > 0 && member.GuildID > 0)
                    {
                        //Update tài sản bang hội
                        KTGlobal.AddMoney(member, MoneyPresinal, MoneyType.GuildMoney, "BOSSVLCT");

                    }

                    if (MoneyGuild > 0 && member.GuildID > 0)
                    {
                        //Update tài sản bang hội
                        KTGlobal.UpdateGuildMoney(MoneyGuild, member.GuildID, client);
                    }

                    // Console.WriteLine("EXP :" + FinalExp + "| NAME :" + member.RoleName);

                    /// EXP cộng thêm từ TU LUYỆN châu buff các kiểu
                    FinalExp += FinalExp * member.m_nExpEnhancePercent / 100;
                    FinalExp += FinalExp * member.m_nExpAddtionP / 100;

                    // Thực hiện AddExp nhưng không ghi vào DB vì sẽ rất COST
                    GameManager.ClientMgr.ProcessRoleExperience(member, FinalExp, true, false, true, "MONSTERKILL");

                    MonsterDeadHelper.BookProseccExp(client, FinalExp);

                    // Do Task khi giết quái xong
                    ProcessTask.Process(sl, pool, member, monster.RoleID, monster.MonsterInfo.ExtensionID, -1, TaskTypes.KillMonster);

                    // TODO : add các sự kiện khác cần giết quái ở đây
                }
            }
            else // Nếu là giết quái solo một mình
            {
                // Lấy ra tối đa số EXP có thể nhận từ tổng lượng sát thương gây lên quái
                int ToalExpCanGet = 100;

                // tính ra số EXP cuối cùng
                int ExpLess = (MonsterExpRevice * ToalExpCanGet) / 100;

                // EXP cộng thêm từ TU LUYỆN châu buff các kiểu
                ExpLess += ExpLess * client.m_nExpEnhancePercent / 100;
                ExpLess += ExpLess * client.m_nExpAddtionP / 100;

                int FinalExp = _CalcPlayerExpByLevel(ExpLess, client.m_Level, monster.m_Level);

                if (client.IsDead())
                {
                    return;
                }

                // Cộng điểm uy danh
                if (UYDANHCANGET > 0)
                {
                    client.Prestige = client.Prestige + UYDANHCANGET;
                }

                // Cộng tài sản cá nhân
                if (MoneyPresinal > 0 && client.GuildID > 0)
                {
                    //Update tài sản bang hội
                    KTGlobal.AddMoney(client, MoneyPresinal, MoneyType.GuildMoney, "BOSSVLCT");

                }
                // Cộng tài sản bang
                if (MoneyGuild > 0 && client.GuildID > 0)
                {
                    //Update tài sản bang hội
                    KTGlobal.UpdateGuildMoney(MoneyGuild, client.GuildID, client);
                }


                //TODO CHECK lại book nếu có tu luyện châu
                //Console.WriteLine("FINAL EXP :" + FinalExp);

                // Thực hiện AddExp nhưng không ghi vào DB vì sẽ rất COST
                GameManager.ClientMgr.ProcessRoleExperience(client, FinalExp, true, false, true);

                //TODO CHECK lại book nếu có tu luyện châu

                MonsterDeadHelper.BookProseccExp(client, FinalExp);

                // Do Task khi giết quái xong
                ProcessTask.Process(sl, pool, client, monster.RoleID, monster.MonsterInfo.ExtensionID, -1, TaskTypes.KillMonster);

                // TODO : add các sự kiện khác cần giết quái ở đây
            }
        }
    }
}