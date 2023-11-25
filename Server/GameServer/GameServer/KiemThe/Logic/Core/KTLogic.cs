using GameServer.Core.Executor;
using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Repute;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.GameEvents.FactionBattle;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.Logic.Manager.Shop;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;

using GameServer.Logic.CheatGuard;
using GameServer.Logic.SecondPassword;

using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.Logic
{
    public class KTLogic
    {
        /// <summary>
        /// Sử dụng sysn dữ liệu cho client
        /// Duy nhất 1 lần tại ProcessInitGameCmd
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static RoleData GetMyselfRoleData(KPlayer client)
        {
            RoleData roleData = new RoleData()
            {
                RoleID = client.RoleID,
                RoleName = client.RoleName,
                RoleSex = client.RoleSex,
                FactionID = client.m_cPlayerFaction.GetFactionId(),
                SubID = client.m_cPlayerFaction.GetRouteId(),

                Level = client.m_Level,
                GuildID = client.GuildID,
                FamilyID = client.FamilyID,
                Money = client.Money,
                BoundMoney = client.BoundMoney,
                MaxExperience = client.m_nNextLevelExp,
                Experience = client.m_Experience,
                PKMode = client.PKMode,
                PKValue = client.PKValue,
                MapCode = client.MapCode,
                RoleDirection = client.RoleDirection,
                PosX = client.PosX,
                PosY = client.PosY,

                MaxHP = client.m_CurrentLifeMax,
                CurrentHP = client.m_CurrentLife,

                MaxMP = client.m_CurrentManaMax,
                CurrentMP = client.m_CurrentMana,

                CurrentStamina = client.m_CurrentStamina,
                MaxStamina = client.m_CurrentStaminaMax,

                TaskDataList = client.TaskDataList,
                RolePic = client.RolePic,
                BagNum = client.BagNum,
                GoodsDataList = client.GoodsDataList,
                MainQuickBarKeys = client.MainQuickBarKeys,
                OtherQuickBarKeys = client.OtherQuickBarKeys,
                Token = client.Token,
                BoundToken = client.BoundToken,

                TeamID = client.TeamID,
                TeamLeaderRoleID = KTTeamManager.GetTeamLeader(client.TeamID) == null ? -1 : KTTeamManager.GetTeamLeader(client.TeamID).RoleID,

                SkillDataList = client.SkillDataList,

                BufferDataList = client.BufferDataList,
                ZoneID = client.ZoneID,

                GuildRank = client.GuildRank,
                FamilyRank = client.FamilyRank,

                StoreMoney = client.StoreMoney,

                MoveSpeed = client.GetCurrentRunSpeed(),
                Camp = client.Camp,

                // Fill giá trị tinh lực hoạt lực
                MakePoint = client.GetMakePoint(),
                GatherPoint = client.GetGatherPoint(),

                WorldHonor = client.WorldHonor,
                TotalValue = client.GetTotalValue(),
                Prestige = client.Prestige,

                IsRiding = client.IsRiding,

                /// Kỹ năng sống
                LifeSkills = client.GetLifeSkills(),

                /// Danh hiệu
                Title = !string.IsNullOrEmpty(client.TempTitle) ? client.TempTitle : client.Title,
                GuildTitle = string.IsNullOrEmpty(client.GuildTitle) ? client.FamilyTitle : client.GuildTitle,

                Repute = client.GetRepute(),

                QuestInfo = client.GetQuestInfo(),

                NPCTaskStateList = TaskManager.getInstance().GetNPCTaskState(client),

                StallName = "",

                AttackSpeed = client.GetCurrentAttackSpeed(),
                CastSpeed = client.GetCurrentCastSpeed(),

                SkillPoint = client.GetCurrentSkillPoints(),

                GMAuth = KTGMCommandManager.IsGM(client) ? 1 : 0,

                OfficeRank = client.OfficeRank,

                SelfTitles = client.RoleTitles.Keys.ToList(),
                SelfCurrentTitleID = client.CurrentRoleTitleID,
            };

            /// Thiết lập hệ thống và Auto
            try
            {
                roleData.AutoSettings = Global.GetRoleParamsStringWithNullFromDB(client, RoleParamName.AutoSettings);
            }
            catch (Exception)
            {
                roleData.AutoSettings = "";
            }
            try
            {
                roleData.SystemSettings = Global.GetRoleParamsStringWithNullFromDB(client, RoleParamName.SystemSettings);
            }
            catch (Exception)
            {
                roleData.SystemSettings = "";
            }
            return roleData;
        }

        /// <summary>
        /// Trả về thông tin người chơi khác, phục vụ cho soi thông tin trang bị
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static RoleData GetOtherPlayerRoleData(KPlayer client)
        {
            /// Thông tin trang bị trên người
            List<GoodsData> goodsDataList = null;
            if (client.GoodsDataList != null)
            {
                goodsDataList = new List<GoodsData>();
                lock (client.GoodsDataList)
                {
                    /// Duyệt danh sách vật phẩm hiện có
                    for (int i = 0; i < client.GoodsDataList.Count; i++)
                    {
                        /// Nếu là trang bị
                        if (client.GoodsDataList[i].Using >= 0)
                        {
                            /// Thêm vào danh sách
                            goodsDataList.Add(client.GoodsDataList[i]);
                        }
                    }
                }
            }

            /// Tạo mới đối tượng
            RoleData roleData = new RoleData()
            {
                RoleID = client.RoleID,
                RoleName = client.RoleName,
                RoleSex = client.RoleSex,
                FactionID = client.m_cPlayerFaction.GetFactionId(),
                Level = client.m_Level,
                RolePic = client.RolePic,
                GoodsDataList = goodsDataList,
                ZoneID = client.ZoneID,
            };

            return roleData;
        }

        /// <summary>
        /// Lưu toàn bộ Buff của nhân vật vào DB
        /// </summary>
        /// <param name="client"></param>
        public static void UpdateAllDBBufferData(KPlayer client)
        {
            if (null == client.BufferDataList)
            {
                return;
            }

            lock (client.BufferDataList)
            {
                // LẤY RA TOÀN BỘ BUFF HIENJ TẠI CỦA PALYER
                for (int i = 0; i < client.BufferDataList.Count; i++)
                {
                    {
                        GameDb.UpdateDBBufferData(client, client.BufferDataList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// XÓA 1 BUFF CỦA CLIENT
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bufferID"></param>
        public static void RemoveBufferData(KPlayer client, int bufferID)
        {
            if (null == client.BufferDataList)
            {
                return;
            }

            BufferData bufferData = null;
            lock (client.BufferDataList)
            {
                for (int i = 0; i < client.BufferDataList.Count; i++)
                {
                    if (client.BufferDataList[i].BufferID == bufferID)
                    {
                        bufferData = client.BufferDataList[i];
                        bufferData.StartTime = 0;
                        bufferData.BufferSecs = 0;
                        bufferData.BufferVal = 0;
                        break;
                    }
                }
            }

            if (null == bufferData)
            {
                return;
            }

            // Lưu lại thông tin buff
            GameDb.UpdateDBBufferData(client, bufferData);
        }

        #region RoleLoginPram

        /// <summary>
        /// LOAD RA CHỈ SỐ KHI NHÂN VẬT LOGIN GAME
        /// </summary>
        /// <param name="client"></param>
        public static void InitRoleLoginPrams(KPlayer client)
        {
            int nCurVal = 0;

            nCurVal = Global.GetRoleParamsInt32FromDB(client, RoleParamName.CurHP);
            //  SET LẠI CHỈ SỐ MÁU HIỆN TẠI CHO NHÂN VẬT
            client.m_CurrentLife = nCurVal;

            nCurVal = Global.GetRoleParamsInt32FromDB(client, RoleParamName.CurMP);
            //  SET LẠI CHỈ SỐ MANA HIỆN TẠI CHO NHÂN VẬT
            client.m_CurrentMana = nCurVal;
            //Stamina
            nCurVal = Global.GetRoleParamsInt32FromDB(client, RoleParamName.CurStamina);
            //  SET LẠI CHỈ SỐ THỂ LỰC HIỆN TẠI CHO NHÂN VẬT
            client.m_CurrentStamina = nCurVal;

            string LifeSkill = Global.GetRoleParamsStringWithDB(client, RoleParamName.LifeSkill);

            if (LifeSkill != null)
            {
                if (LifeSkill.Length > 0)
                {
                    try
                    {
                        byte[] Base64Decode = Convert.FromBase64String(LifeSkill);

                        Dictionary<int, LifeSkillPram> _LifeSkillData = DataHelper.BytesToObject<Dictionary<int, LifeSkillPram>>(Base64Decode, 0, Base64Decode.Length);

                        client.SetLifeSkills(_LifeSkillData);
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, "Có lỗi khi khởi tạo lại LIFESKILL :" + ex.ToString());
                        Dictionary<int, LifeSkillPram> _LifeSkillData = new Dictionary<int, LifeSkillPram>();

                        for (int i = 1; i < 12; i++)
                        {
                            LifeSkillPram _LifeSkill = new LifeSkillPram();
                            _LifeSkill.LifeSkillID = i;
                            _LifeSkill.LifeSkillLevel = 1;
                            _LifeSkill.LifeSkillExp = 0;

                            _LifeSkillData.Add(i, _LifeSkill);
                        }

                        client.SetLifeSkills(_LifeSkillData);
                    }
                }
            }
            else
            {
                Dictionary<int, LifeSkillPram> _LifeSkillData = new Dictionary<int, LifeSkillPram>();

                for (int i = 1; i < 12; i++)
                {
                    LifeSkillPram _LifeSkill = new LifeSkillPram();
                    _LifeSkill.LifeSkillID = i;
                    _LifeSkill.LifeSkillLevel = 1;
                    _LifeSkill.LifeSkillExp = 0;

                    _LifeSkillData.Add(i, _LifeSkill);
                }

                client.SetLifeSkills(_LifeSkillData);
            }

            string ReputeInfoStr = Global.GetRoleParamsStringWithDB(client, RoleParamName.ReputeInfo);

            if (ReputeInfoStr != null)
            {
                if (ReputeInfoStr.Length > 0)
                {
                    try
                    {
                        byte[] Base64Decode = Convert.FromBase64String(ReputeInfoStr);

                        List<ReputeInfo> _ReputeInfo = DataHelper.BytesToObject<List<ReputeInfo>>(Base64Decode, 0, Base64Decode.Length);

                        client.SetReputeInfo(_ReputeInfo);
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, "Có lỗi khi khởi tạo lại ReputeInfoStr :" + ex.ToString());

                        List<ReputeInfo> _ReputeInfo = new List<ReputeInfo>();

                        foreach (Camp _Camp in ReputeManager._ReputeConfig.Camp)
                        {
                            foreach (Class _Class in _Camp.Class)
                            {
                                int DBID = _Camp.Id * 100 + _Class.Id;
                                int Level = 1;
                                int Exp = 0;

                                ReputeInfo _Info = new ReputeInfo();

                                _Info.DBID = DBID;
                                _Info.Level = Level;
                                _Info.Exp = Exp;

                                _ReputeInfo.Add(_Info);
                            }
                        }

                        client.SetReputeInfo(_ReputeInfo);
                    }
                }
            }
            else
            {
                List<ReputeInfo> _ReputeInfo = new List<ReputeInfo>();

                foreach (Camp _Camp in ReputeManager._ReputeConfig.Camp)
                {
                    foreach (Class _Class in _Camp.Class)
                    {
                        int DBID = _Camp.Id * 100 + _Class.Id;
                        int Level = 1;
                        int Exp = 0;

                        ReputeInfo _Info = new ReputeInfo();

                        _Info.DBID = DBID;
                        _Info.Level = Level;
                        _Info.Exp = Exp;

                        _ReputeInfo.Add(_Info);
                    }
                }

                client.SetReputeInfo(_ReputeInfo);
            }

            /// Chuỗi mã hóa danh hiệu
            string roleTitlesInfo = Global.GetRoleParamsStringWithDB(client, RoleParamName.RoleTitles);
            try
            {
                /// Nếu toác
                if (roleTitlesInfo == null)
                {
                    throw new Exception();
                }

                string[] fields = roleTitlesInfo.Split('|');
                int currentTitleID = int.Parse(fields[0]);
                ConcurrentDictionary<int, long> titles = new ConcurrentDictionary<int, long>();
                for (int i = 1; i < fields.Length; i++)
                {
                    string[] data = fields[i].Split('_');
                    int titleID = int.Parse(data[0]);
                    long startTime = long.Parse(data[1]);
                    titles[titleID] = startTime;
                }
                /// ID danh hiệu hiện tại
                client.CurrentRoleTitleID = currentTitleID;
                /// Danh sách danh hiệu hiện tại
                client.RoleTitles = titles;
            }
            catch (Exception ex)
            {
                client.RoleTitles = new ConcurrentDictionary<int, long>();
                client.CurrentRoleTitleID = -1;
            }

            //SET LẠI CẤP ĐỘ CHO THỰC THỂ
            client.m_Level = client.GetRoleData().Level;

            //SET Lại Exp Hiện tại Cho CLient
            client.m_Experience = (int)client.GetRoleData().Experience;

            lock (client.PropPointMutex)
            {
                /// Load base của nhân vật trước tiên
                KTAttributesModifier.LoadRoleBaseAttributes(client);

                /// Tổng điểm tiềm năng được cộng thêm từ các loại bánh đã ăn
                int additionRemainPoint = Global.GetRoleParamsInt32FromDB(client, RoleParamName.TotalPropPoint);
                client.SetBonusRemainPotentialPoints(additionRemainPoint);

                /// Yổng điểm kỹ năng được cộng thêm từ các loại bánh đã ăn
                int additionSkillPoint = Global.GetRoleParamsInt32FromDB(client, RoleParamName.TotalSkillPoint);
                client.SetBonusSkillPoint(additionSkillPoint, false);

                /// Tổng điểm tiềm năng có từ Base
                int baseRemainPoint = client.GetBaseRemainPotentialPoints();

                // Console.WriteLine("Bonus Remain = " + additionRemainPoint + " - " + "Base Remain = " + baseRemainPoint + " - " + "Base SkillPoint = " + client.GetBaseSkillPoint());

                /// Thuộc tính từng chỉ số đã cộng
                int STR = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropStrength);
                int ENER = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropIntelligence);
                int DEX = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropDexterity);
                int VIT = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropConstitution);

                int totalRemainPoint = baseRemainPoint + additionRemainPoint - STR - ENER - DEX - VIT;

                /// Kiểm tra nếu có BUG thì tiến hành reset toàn bộ
                if (totalRemainPoint < 0)
                {
                    totalRemainPoint = baseRemainPoint + additionRemainPoint;
                    STR = 0;
                    ENER = 0;
                    DEX = 0;
                    VIT = 0;
                }

                // Kiểm tra xem điểm phân phối có đúng theo quyy tắc không

                if (!client.CheckAssignPotential(ref STR, ref DEX, ref VIT, ref ENER))
                {
                    client.UnAssignPotential();
                }

                client.ChangeStrength(STR);
                client.ChangeEnergy(ENER);
                client.ChangeDexterity(DEX);
                client.ChangeVitality(VIT);

                // TINH LỰC HOẠT LỰC
                int GatherPoint = Global.GetRoleParamsInt32FromDB(client, RoleParamName.GatherPoint);
                int MakePoint = Global.GetRoleParamsInt32FromDB(client, RoleParamName.MakePoint);

                // SET GIÁ TRỊ CHO TINH LỰC HOẠT LỰC
                if (GatherPoint > 0)
                {
                    client.ChangeCurGatherPoint(GatherPoint);
                }

                if (MakePoint > 0)
                {
                    client.ChangeCurMakePoint(MakePoint);
                }

                int nVerifyBuffProp = Global.GetRoleParamsInt32FromDB(client, RoleParamName.VerifyBuffProp);

                /// ĐOẠN NÀY LÀ ĐOẠN TẠM THỜI BYPASS ĐỂ CHO VÀO GAME
                //client.m_CurrentLife = client.m_CurrentLifeMax;
                //client.m_CurrentMana = client.m_CurrentManaMax;

                //client.m_CurrentStamina = client.m_CurrentStaminaMax;

                /// Trạng thái có cưỡi ngựa hay không
                int horseRidingParam = Global.GetRoleParamsInt32FromDB(client, RoleParamName.HorseToggleOn);
                client.IsRiding = horseRidingParam == 1;

                // Read Ra thông tin nhiệm vụ tuần hoàn

                // Nhiệm vụ bạo văn đồng
                int CurBVDTaskID = Global.GetRoleParamsInt32FromDB(client, RoleParamName.CurBVDTaskID);
                int QuestBVDTodayCount = Global.GetRoleParamsInt32FromDB(client, RoleParamName.QuestBVDTodayCount);
                int CanncelQuestBVD = Global.GetRoleParamsInt32FromDB(client, RoleParamName.CanncelQuestBVD);
                int QuestBVDStreakCount = Global.GetRoleParamsInt32FromDB(client, RoleParamName.QuestBVDStreakCount);

                client.QuestBVDStreakCount = QuestBVDStreakCount;
                client.CurenQuestIDBVD = CurBVDTaskID;
                client.QuestBVDTodayCount = QuestBVDTodayCount;
                client.CanncelQuestBVD = CanncelQuestBVD;



                if (client.OldTasks != null)
                {
                    var findmaxMain = client.OldTasks.Where(x => x.TaskClass == 0).ToList();

                    if (findmaxMain.Count > 0)
                    {
                        List<int> TaskInput = findmaxMain.Select(x => x.TaskID).ToList();

                        int LastMainTask = findmaxMain.Last().TaskID;

                        QuestInfo MainTask = new QuestInfo();
                        MainTask.TaskClass = 0;
                        MainTask.CurTaskIndex = LastMainTask;
                        client.AddQuestInfo(MainTask);
                    }
                    else
                    {
                        QuestInfo MainTask = new QuestInfo();
                        MainTask.TaskClass = 0;
                        MainTask.CurTaskIndex = -1;
                        client.AddQuestInfo(MainTask);
                    }


                }
                else
                {
                    QuestInfo MainTask = new QuestInfo();
                    MainTask.TaskClass = 0;
                    MainTask.CurTaskIndex = -1;
                    client.AddQuestInfo(MainTask);

                    QuestInfo SUBTASK = new QuestInfo();
                    SUBTASK.TaskClass = 5;
                    SUBTASK.CurTaskIndex = -1;
                    client.AddQuestInfo(SUBTASK);
                }

                // Đọc ra các ghi chép nhật ký theo ngày
                string DailyRecore = Global.GetRoleParamsStringWithDB(client, RoleParamName.DailyRecore);
                int DayID = DateTime.Now.DayOfYear;

                if (DailyRecore != null)
                {
                    if (DailyRecore.Length > 0)
                    {
                        try
                        {
                            byte[] Base64Decode = Convert.FromBase64String(DailyRecore);

                            DailyDataRecore _DailyDataRecore = DataHelper.BytesToObject<DailyDataRecore>(Base64Decode, 0, Base64Decode.Length);

                            client._DailyDataRecore = _DailyDataRecore;

                            if (_DailyDataRecore.DayID != DayID)
                            {
                                // Nếu như khác ngày thì reset hết dữ liệu luôn
                                DailyDataRecore _NewDailyDataRecore = new DailyDataRecore();
                                _NewDailyDataRecore.DayID = DayID;
                                _NewDailyDataRecore.EventRecoding = new Dictionary<int, int>();

                                client._DailyDataRecore = _NewDailyDataRecore;
                            }
                        }
                        catch (Exception ex)
                        {
                            DailyDataRecore _DailyDataRecore = new DailyDataRecore();
                            _DailyDataRecore.DayID = DayID;
                            _DailyDataRecore.EventRecoding = new Dictionary<int, int>();
                            client._DailyDataRecore = _DailyDataRecore;
                        }
                    }
                }
                else
                {
                    DailyDataRecore _DailyDataRecore = new DailyDataRecore();
                    _DailyDataRecore.DayID = DayID;
                    _DailyDataRecore.EventRecoding = new Dictionary<int, int>();
                    client._DailyDataRecore = _DailyDataRecore;
                }

                // Đọc ra các ghi chép nhật ký theo tuần
                string WeekRecore = Global.GetRoleParamsStringWithDB(client, RoleParamName.WeekRecore);
                int WeekID = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);

                if (WeekRecore != null)
                {
                    if (WeekRecore.Length > 0)
                    {
                        try
                        {
                            byte[] Base64Decode = Convert.FromBase64String(WeekRecore);

                            WeekDataRecore _WeekDataRecore = DataHelper.BytesToObject<WeekDataRecore>(Base64Decode, 0, Base64Decode.Length);

                            client._WeekDataRecore = _WeekDataRecore;

                            if (_WeekDataRecore.WeekID != WeekID)
                            {
                                WeekDataRecore _NewWeekDataRecore = new WeekDataRecore();
                                _NewWeekDataRecore.WeekID = WeekID;
                                _NewWeekDataRecore.EventRecoding = new Dictionary<int, int>();

                                client._WeekDataRecore = _NewWeekDataRecore;
                            }
                        }
                        catch (Exception ex)
                        {
                            WeekDataRecore _WeekDataRecore = new WeekDataRecore();
                            _WeekDataRecore.WeekID = WeekID;
                            _WeekDataRecore.EventRecoding = new Dictionary<int, int>();
                            client._WeekDataRecore = _WeekDataRecore;
                        }
                    }
                }
                else
                {
                    WeekDataRecore _WeekDataRecore = new WeekDataRecore();
                    _WeekDataRecore.WeekID = WeekID;
                    _WeekDataRecore.EventRecoding = new Dictionary<int, int>();
                    client._WeekDataRecore = _WeekDataRecore;
                }

                //int ReviceFistLoginReward = client.GetValueOfWeekRecore(111111);
                //if (ReviceFistLoginReward == -1)
                //{
                //    SubRep _REP = KTGlobal.AddMoney(client, 1000000, MoneyType.Dong, "NEWBIE");
                //    if (_REP.IsOK)
                //    {
                //        client.SetValueOfWeekRecore(111111, 1);
                //    }
                //}

                ///Bản ghi vĩnh viễn
                string ForeverRecore = Global.GetRoleParamsStringWithDB(client, RoleParamName.ForeverRecore);
                if (ForeverRecore != null)
                {
                    if (ForeverRecore.Length > 0)
                    {
                        try
                        {
                            byte[] Base64Decode = Convert.FromBase64String(ForeverRecore);

                            ForeverRecore _ForeverRecore = DataHelper.BytesToObject<ForeverRecore>(Base64Decode, 0, Base64Decode.Length);

                            client._ForeverRecore = _ForeverRecore;
                        }
                        catch (Exception ex)
                        {
                            ForeverRecore _ForeverRecore = new ForeverRecore();

                            _ForeverRecore.EventRecoding = new Dictionary<int, int>();
                            client._ForeverRecore = _ForeverRecore;
                        }
                    }
                    else
                    {
                        ForeverRecore _ForeverRecore = new ForeverRecore();
                        _ForeverRecore.EventRecoding = new Dictionary<int, int>();
                        client._ForeverRecore = _ForeverRecore;
                    }
                }
                else
                {
                    ForeverRecore _ForeverRecore = new ForeverRecore();
                    _ForeverRecore.EventRecoding = new Dictionary<int, int>();
                    client._ForeverRecore = _ForeverRecore;
                }

                // Lấy ra thời gian ủy thác bạch cầu hoàn
                client.baijuwan = Global.GetRoleParamsInt32FromDB(client, RoleParamName.MeditateTime);
                client.baijuwanpro = Global.GetRoleParamsInt32FromDB(client, RoleParamName.NotSafeMeditateTime);

                /// Nhận thưởng tải game hay chưa
                client.ReviceBounsDownload = Global.GetRoleParamsInt32FromDB(client, RoleParamName.TreasureJiFen);

                long LastOffLineTime = client.LastOfflineTime;

                long TimeNow = TimeUtil.NOW();

                long TotalOfflineTime = TimeNow - LastOffLineTime;

                int SECOFFLINE = (int)TotalOfflineTime / 1000;

                if (client.baijuwan > 0 || client.baijuwanpro > 0)
                {
                    if (SECOFFLINE < 0)
                    {
                        SECOFFLINE = 0;
                    }

                    if (SECOFFLINE > 0)
                    {
                        int TOTALOFFFMIN = 0;

                        // QUY ĐỔI RA PHÚT
                        int MIN = SECOFFLINE / 60;
                        TOTALOFFFMIN = MIN;
                        int TIMEPRO = 0;
                        int TIMENORMAL = 0;
                        if (client.baijuwanpro > 0)
                        {
                            // Nếu thời gian rời mạng > lớn hơn thời gian đại bạch cầu hoàn có

                            if (MIN >= client.baijuwanpro)
                            {
                                // Thời gian ủy thác đại bạch cầu hoàn = max số phút hiện có
                                TIMEPRO = client.baijuwanpro;
                                // Set thời gian ủy thác đại bạch cầu hoàn về 0
                                client.baijuwanpro = 0;

                                // Số phút còn lại để tính cho bạch cầu hoàn
                                MIN = MIN - TIMEPRO;
                            }
                            else
                            {
                                // Nếu số thời gian đại bạch cầu hoàn còn dư sức thì

                                TIMEPRO = MIN;

                                client.baijuwanpro = client.baijuwanpro - MIN;
                                // Số phút ủy thác về 0
                                MIN = 0;
                            }
                        }
                        // Nếu số phút của bạch cầu hoàn còn lớn hơn 0
                        if (client.baijuwan > 0 && MIN > 0)
                        {
                            if (MIN >= client.baijuwan)
                            {
                                // Thời gian ủy thác  bạch cầu hoàn = max số phút hiện có
                                TIMENORMAL = client.baijuwan;
                                // Set thời gian ủy thác  bạch cầu hoàn về 0
                                client.baijuwan = 0;
                            }
                            else
                            {
                                // Nếu số thời gian  bạch cầu hoàn còn dư sức thì

                                TIMENORMAL = MIN;
                                client.baijuwan = client.baijuwan - MIN;
                            }
                        }

                        int BASEEXPERN = KPlayerSetting.GetBaseExpLevel(client.m_Level);

                        double EXPPERSECON = (BASEEXPERN / 60);

                        double EXPGIAN = 0;

                        double EXPBCH = 0;

                        double EXPDAIBCH = 0;

                        if (TIMEPRO > 0)
                        {
                            EXPGIAN = (TIMEPRO * 60) * EXPPERSECON * 1.3;

                            EXPDAIBCH = EXPGIAN;
                        }

                        if (TIMENORMAL > 0)
                        {
                            EXPBCH = (TIMENORMAL * 60) * EXPPERSECON;
                            EXPGIAN += (TIMENORMAL * 60) * EXPPERSECON;
                        }

                        var ActivExpLoginMap = KTKTAsyncTask.Instance.ScheduleExecuteAsync(new DelayFuntionAsyncTask("ExpCallAddDelay", new Action(() => PlayerManager.NotifyBCH(client, EXPGIAN, EXPBCH, EXPDAIBCH, TIMEPRO, TIMENORMAL, TOTALOFFFMIN))), 10 * 1000);
                    }
                }

                // Tính toán về nhiệm vụ
                int MINOFF = SECOFFLINE / 60;
                // Nếu tổng thời gian offline mà lớn hơn 1 ngày
                if (MINOFF >= 1440)
                {
                    int DayHave = MINOFF / 1440;


                    // Reset lại số lượng của QUEST bạo văn đồng
                    client.CanncelQuestBVD = 0;
                    client.QuestBVDTodayCount = 0;
                    client.QuestBVDStreakCount = 0;

                    // Đánh dấu lại đã reset bạo văn đồng
                    client.SetValueOfDailyRecore((int)AcitvityRecore.BVD, 1);
                }
                else // Nếu chưa offline được 1 ngày thì check xem hôm nay đã liếm q hải tặc chưa
                {


                    int BVD = client.GetValueOfDailyRecore((int)AcitvityRecore.BVD);
                    if (BVD != 1)
                    {
                        // Nếu mà chưa cộng thì tiến hành cộng số lượt
                        client.CanncelQuestBVD = 0;
                        client.QuestBVDTodayCount = 0;
                        client.QuestBVDStreakCount = 0;
                        // Đánh dấu hôm nay đã được + quest hải tặc rồi
                        client.SetValueOfDailyRecore((int)AcitvityRecore.BVD, 1);
                    }
                }

                // Đọc ra số thời gian online
                client.DayOnlineSecond = Global.GetRoleParamsInt32FromDB(client, RoleParamName.DayOnlineSecond);
                client.BakDayOnlineSecond = client.DayOnlineSecond;
                client.DayOnlineRecSecond = TimeUtil.NOW();

                // Đăng nhập liên tục => cái này cần cho maketing phân tích sau này
                client.SeriesLoginNum = Global.GetRoleParamsInt32FromDB(client, RoleParamName.SeriesLoginCount);

                client.OpenGridTime = Global.GetRoleParamsInt32FromDB(client, RoleParamName.OpenGridTick);

                //DỮ LIỆU THẺ THÁNG
                client.YKDetail.ParseFrom(Global.GetRoleParamByName(client, RoleParamName.YueKaInfo));



            }


            var ExpCallAddDelay = KTKTAsyncTask.Instance.ScheduleExecuteAsync(new DelayFuntionAsyncTask("ExpCallAddDelay", new Action(() => PlayerManager.NotifyExpChange(client))), 10 * 1000);
            //GHI CHÉP LẠI THỜI GIAN LINE
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ROLE_ONLINE,
                   string.Format("{0}:{1}:{2}:{3}",
                   client.RoleID,
                   GameManager.ServerLineID,
                   client.OnlineActiveVal,
                   Global.GetSocketRemoteIP(client)),
                   null, client.ServerId);
        }

        #endregion RoleLoginPram

        #region RoleLogoutAction

        /// <summary>
        /// Thực hiện toàn bộ LOGIC xử lý khi nhân vật LOGOUT
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public static void Logout(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            try
            {
                /// Xóa khỏi danh sách theo địa chỉ IP
                GameManager.OnlineUserSession.RemoveClientFromIPAddressList(client);

                /// Ngừng StoryBoard tương ứng
                KTPlayerStoryBoardEx.Instance.Remove(client);

                /// Nếu đang ở trong bản đồ Liên đấu hội trường
                if (TeamBattle.IsInTeamBattleMap(client))
                {
                    /// Thực thi sự kiện rời bản đồ hội trường
                    TeamBattle_ActivityScript.OnPlayerLeave(client, null);
                }

                /// Rời nhóm hiện tại
                if (client.TeamID != -1 && KTTeamManager.IsTeamExist(client.TeamID))
                {
                    client.LeaveTeam();
                }

                //Call Global Event LOGOUT cho các classs khác biết nhân vật đang LOGOUT
                GlobalEventSource.getInstance().fireEvent(new PlayerLogoutEventObject(client));

                //FOCE GỬI LỆNH BẮT DB PHẢI LƯU TRỮ CÁC THÔNG TIN  | TIỀN | EXP |
                GameDb.ProcessDBCmdByTicks(client, true);

                LogManager.WriteLog(LogTypes.Logout, "[" + client.RoleID + "][" + client.RoleName + "][" + client.MapCode + "|" + client.PosX + "|" + client.PosY + "]  HOẠT LỰC :" + client.GetGatherPoint() + " | TINH LỰC :" + client.GetMakePoint() + "| BẠC :" + client.Money + "| BẠC KHÓA :" + client.BoundMoney + "| ĐỒNG  :" + client.Token + "| ĐỒNG KHÓA :" + client.BoundToken);

                /// Gọi hàm tương ứng
                client.OnQuitGame();

                /// Nếu đang trong phụ bản
                if (client.CopyMapID != -1 && CopySceneEventManager.IsCopySceneExist(client.CopyMapID, client.CurrentMapCode))
                {
                    /// Phụ bản tương ứng
                    KTCopyScene copyScene = CopySceneEventManager.GetCopyScene(client.CopyMapID, client.CurrentMapCode);
                    /// Thực thi sự kiện Disconnected
                    CopySceneEventManager.OnPlayerDisconnected(copyScene, client);
                }

                //LƯU LẠI ROLE DATA BY PRAMENTER
                GameDb.ProcessDBRoleParamCmdByTicks(client, true);

                //Thực Hiện Lệnh LƯU LẠI ĐỘ BỀN CỦA TOÀN BỘ TRANG BỊ
                GameDb.ProcessDBEquipStrongCmdByTicks(client, true);

                //Lưu dữ liệu các hoạt động trong ngày
                GameDb.UpdateHuoDongDBCommand(pool, client);

                // Gỡ item khỏi giỏ bán hàng
                if (!client.ClientSocket.IsKuaFuLogin && client.SaleGoodsDataList.Count > 0)
                {
                    SaleRoleManager.RemoveSaleRoleItem(client.RoleID);
                    SaleGoodsManager.RemoveSaleGoodsItems(client);
                }



                //Cập nhật thời gian online và thời gian bảo vệ tân thủ
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEONLINETIME, string.Format("{0}:{1}:{2}", client.RoleID, client.TotalOnlineSecs, 0), null, client.ServerId);

                /// Cập nhật trị PK
                KT_TCPHandler.UpdatePKValueToDB(client);

                //Remove CLIENT
                GameManager.ClientMgr.RemoveClient(client);

                List<Object> objsList = Global.GetAll9GridObjects(client);

                //Thông báo cho các người chơi xung quanh levave game
                Global.GameClientHandleOldObjs(client, objsList);

                /// Xóa Dic tầm nhìn của client
                client.ClearVisibleObjects(true);

                /// Mở khóa các gói vật phẩm do người chơi đánh rơi ra cho người chơi khác có thể nhặt
                GameManager.GoodsPackMgr.UnLockGoodsPackItem(client);

                // Remove DIC RoleName2ID
                RoleName2IDs.RemoveRoleName(Global.FormatRoleName(client, client.RoleName));

                //OnLogout

                FactionBattleManager.FactionLogout(client);

                //Xử lý giao dịch khi bị ngắt kết nối
                ProcessExchangeData(sl, pool, client);

                //Nếu mà máu hiện tại là chết ==> đưa về thành
                if (client.m_CurrentLife <= 0)
                {
                    /// Thông tin điểm hồi sinh
                    KT_TCPHandler.GetPlayerDefaultRelivePos(client, out int mapCode, out int posX, out int posY);
                    client.MapCode = mapCode;
                    client.PosX = posX;
                    client.PosY = posY;
                    client.ReportPosTicks = 0;
                }

                //Ghi lại thông tin vị trí đang đứng
                if (Global.CanRecordPos(client))
                {
                    //SEND CMD CHO DB
                    GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_POS,
                        string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, client.MapCode,
                        client.RoleDirection, client.PosX, client.PosY),
                        null, client.ServerId);
                }

                //Cập nhật trạng thái OFFLINE ở DB
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ROLE_OFFLINE,
                    string.Format("{0}:{1}:{2}:{3}:{4}",
                    client.RoleID,
                    GameManager.ServerLineID,
                    Global.GetSocketRemoteIP(client),
                    client.OnlineActiveVal,
                    TimeUtil.NOW()),
                    null, client.ServerId);

                //Ghi lại event logout ra files logs
                Global.AddRoleLogoutEvent(client);

                // Update các thuộc tính cho nhân vật
                UpdateRoleParamsInfo(client);

                // Làm trống hàng đợi để người chơi khác có thể vào nếu max LIMIT LOGIN
                GameManager.loginWaitLogic.AddToAllow(client.strUserID, GameManager.loginWaitLogic.GetConfig(GameServer.Logic.LoginWaiting.LoginWaitLogic.UserType.Normal, GameServer.Logic.LoginWaiting.LoginWaitLogic.ConfigType.LogouAllowMSeconds));

                // Ghi mật khẩu cấp 2 túi đồ VV
                SecondPasswordManager.OnUsrLogout(client.strUserID);
                SpeedUpTickCheck.Instance().OnLogout(client);

                try
                {
                    string ip = RobotTaskValidator.getInstance().GetIp(client);

                    string analysisLog = string.Format("logout server={0} account={1} player={2} dev_id={3} exp={4}", GameManager.ServerId, client.strUserID, client.RoleID, string.IsNullOrEmpty(client.deviceID) ? "" : client.deviceID, ip);
                    LogManager.WriteLog(LogTypes.Analysis, analysisLog);
                }
                catch { }

                /// Đánh dấu đã rời mạng
                client.LogoutState = true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
            }
        }

        #endregion RoleLogoutAction

        #region UpdateRolePrams

        /// <summary>
        /// Lưu lại các PRAM TRƯỚC KHI THOÁT
        /// </summary>
        /// <param name="client"></param>
        public static void UpdateRoleParamsInfo(KPlayer client)
        {
            // Lưu lại giá trị máu và MP hiện tại

            if (client.m_CurrentLife < 0)
            {
                client.m_CurrentLife = client.m_CurrentLifeMax;
            }
            if (client.m_CurrentMana < 0)
            {
                client.m_CurrentMana = client.m_CurrentManaMax;
            }
            if (client.m_CurrentStamina < 0)
            {
                client.m_CurrentStamina = client.m_CurrentStaminaMax;
            }

            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CurHP, client.m_CurrentLife, true);

            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CurMP, client.m_CurrentMana, true);

            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CurStamina, client.m_CurrentStamina, true);

            lock (client.PropPointMutex)
            {
                /// Điểm tiềm năng được cộng thêm từ bánh
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.TotalPropPoint, client.GetBonusRemainPotentialPoints(), true);
                /// TODO lưu điểm kỹ năng được cộng thêm từ bánh

                // LƯU VÀO DB 4 chỉ số
                // SỨC , TRÍ , NHANH, THỂ
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.sPropStrength, client.GetStrength(), true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.sPropIntelligence, client.GetEnergy(), true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.sPropDexterity, client.GetDexterity(), true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.sPropConstitution, client.GetVitality(), true);

                // LƯU THÊM 2 chỉ số TINH LỰC HOẠT LỰC

                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.GatherPoint, client.GetGatherPoint(), true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.MakePoint, client.GetMakePoint(), true);

                // LƯU LẠI ĐÃ NHẬN QUÀ TẢI CHƯA
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.TreasureJiFen, client.ReviceBounsDownload, true);

                // LƯU LẠI THÔNG IN KỸ NĂNG SỐNG
                Global.SaveRoleParamsStringToDB(client, RoleParamName.LifeSkill, client.LifeSkillToString, true);

                // LƯU LẠI VINH DỰ VÕ LÂM
                Global.SaveRoleParamsStringToDB(client, RoleParamName.ReputeInfo, client.ReputeInfoToString, true);

                /// Lưu lại danh sách danh hiệu
                Global.SaveRoleParamsStringToDB(client, RoleParamName.RoleTitles, client.RoleTitlesInfoString, true);

                // Lưu lại các thông tin tới nhiệm vụ tuần hoàn
                // NHIỆM VỤ BẠO VĂN ĐỒNG
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CurBVDTaskID, client.CurenQuestIDBVD, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.QuestBVDTodayCount, client.QuestBVDTodayCount, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CanncelQuestBVD, client.CanncelQuestBVD, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.QuestBVDStreakCount, client.QuestBVDStreakCount, true);
                // Nhiệm vụ hải tặc



                // Lưu lại các bản ghi hàng ngày
                Global.SaveRoleParamsStringToDB(client, RoleParamName.DailyRecore, client.DailyRecoreString, true);
                //Lưu lại các bản ghi tạm thời trong tuần
                Global.SaveRoleParamsStringToDB(client, RoleParamName.WeekRecore, client.WeekRecoreString, true);
                // Lưu lại các bản ghi vĩnh viễn
                Global.SaveRoleParamsStringToDB(client, RoleParamName.ForeverRecore, client.ForeverRecoreString, true);
                // LƯU LẠI THỜI GIAN ỦY THÁC BẠCH CẦU HOÀN

                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.MeditateTime, client.baijuwan, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.NotSafeMeditateTime, client.baijuwanpro, true);
            }

            // Lưu lại thời gian đăng nhập
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.DayOnlineSecond, client.DayOnlineSecond, true);

            // Lưu lại đăng nhập liên tục
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.SeriesLoginCount, client.SeriesLoginNum, true);

            // Lưu lại thời gian mở túi đồ
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.OpenGridTick, client.OpenGridTime, true);

            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.OpenPortableGridTick, client.OpenPortableGridTime, true);

            /// Lưu lại số điểm Kinh nghiệm Tu Luyện Châu còn lại
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.XiuLianZhu, client.XiuLianZhu_Exp, true);
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.XiuLianZhu_TotalTime, client.XiuLianZhu_TotalTime, true);
        }

        #endregion UpdateRolePrams

        #region XỬ LÝ SỰ KIỆN KHI THAY ĐỔI NGÀY ĐĂNG NHẬP

        /// <summary>
        /// Xử lý các sự kiện khi qua 1 ngày
        /// </summary>
        /// <param name="client"></param>
        public static void ChangeDayLoginNum(KPlayer client)
        {
            int dayID = TimeUtil.NowDateTime().DayOfYear;

            if (dayID == client.LoginDayID)
            {
                return;
            }

            client.LoginDayID = dayID;

            UpdateEventNextDay(client);
        }

        public static void UpdateEventNextDay(KPlayer client)
        {
            // RESET LẠI BVD
            client.CanncelQuestBVD = 0;
            client.QuestBVDTodayCount = 0;
            client.QuestBVDStreakCount = 0;

            client.SetValueOfDailyRecore((int)AcitvityRecore.BVD, 1);



            // Xử lý các quà tặng khi đăng nhập
            // HuodongCachingMgr.OnJieriRoleLogin(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            //Ghi lại tích lũy đăng nhập
            ChengJiuManager.OnRoleLogin(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            HuodongCachingMgr.ProcessDayOnlineSecs(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //Check trạng thái nhiệm vụ tuần hoàn
            Global.InitRoleDailyTaskData(client, true);


            // Cập nhật thẻ tháng cho ngày mới
            CardMonthManager.UpdateNewDay(client);



            // Ghi lại nhật ký đăng nhập
            Global.UpdateRoleLoginRecord(client);


            client.SendPacket((int)TCPGameServerCmds.CMD_SYNC_CHANGE_DAY_SERVER, string.Format("{0}", TimeUtil.NOW() * 10000));


        }

        #endregion XỬ LÝ SỰ KIỆN KHI THAY ĐỔI NGÀY ĐĂNG NHẬP



        #region Xử lý giao dịch khi thoát game

        /// <summary>
        /// Xử lý giao dịch khi ngắt kết nối
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public static void ProcessExchangeData(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.ExchangeID > 0)
            {
                ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(client.ExchangeID);
                if (null != ed)
                {
                    int otherRoleID = (ed.RequestRoleID == client.RoleID) ? ed.AgreeRoleID : ed.RequestRoleID;

                    // TÌm thằng đối phương đang giao dịch cùng

                    KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                    if (null != otherClient) //Nếu thằng này vẫn đang online
                    {
                        if (otherClient.ExchangeID > 0 && otherClient.ExchangeID == client.ExchangeID)
                        {
                            GameManager.GoodsExchangeMgr.RemoveData(client.ExchangeID);

                            Global.RestoreExchangeData(otherClient, ed);

                            otherClient.ExchangeID = 0;
                            otherClient.ExchangeTicks = 0;

                            //Thông báo về là hủy giao dịch
                            GameManager.ClientMgr.NotifyGoodsExchangeCmd(sl, pool, client.RoleID, otherRoleID, null, otherClient, client.ExchangeID, (int)GoodsExchangeCmds.Cancel);
                        }
                    }
                }
            }
        }

        #endregion Xử lý giao dịch khi thoát game

        #region Skill Logic

        /// <summary>
        /// Có phải kẻ địch không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsOpposite(GameObject obj, GameObject target)
        {
            /// Toác gì đó
            if (obj == null || target == null)
			{
                return false;
			}

            /// Nếu là chính mình thì bỏ qua
            if (obj == target)
            {
                return false;
            }

            /// Nếu là người chơi
            if (obj.IsPlayer() && target.IsPlayer())
            {
                KPlayer objPlayer = obj as KPlayer;
                KPlayer targetPlayer = target as KPlayer;

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(obj.CurrentMapCode);
                /// Nếu bản đồ không cho phép đánh nhau
                if (gameMap != null && !gameMap.AllowPK)
                {
                    return false;
                }

                /// Nếu đang trong trạng thái tỷ thí
                if (objPlayer.IsChallengeWith(targetPlayer))
                {
                    return true;
                }

                /// Nếu đang tuyên chiến cùng
                if (objPlayer.IsActiveFightWith(targetPlayer))
                {
                    return true;
                }

                /// Nếu là trạng thái PK đặc biệt
                if (objPlayer.PKMode == (int) PKMode.Custom && targetPlayer.PKMode == (int) PKMode.Custom)
                {
                    /// Trả về kết quả Camp khác nhau
                    return objPlayer.Camp != -1 && targetPlayer.Camp != -1 && objPlayer.Camp != targetPlayer.Camp;
                }

                /// Nếu cùng nhóm thì không cho đánh nhau
                if (KTLogic.IsTeamMate(targetPlayer, objPlayer))
                {
                    return false;
                }

                /// Nếu cùng bang hoặc tộc thì không cho đánh nhau
                if (KTLogic.IsTeamMate(objPlayer, targetPlayer) || KTLogic.IsSameFamily(objPlayer, targetPlayer) || KTLogic.IsGuildMate(objPlayer, targetPlayer))
				{
                    return false;
				}

                /// Nếu 1 trong 2 có trạng thái PK đồ sát
                if (objPlayer.PKMode == (int)PKMode.All || targetPlayer.PKMode == (int)PKMode.All)
                {
                    return true;
                }
                /// Nếu 1 trong 2 có trạng thái PK Server
                else if (objPlayer.PKMode == (int) PKMode.Server || targetPlayer.PKMode == (int) PKMode.Server)
                {
                    return objPlayer.ZoneID != targetPlayer.ZoneID;
                }
                /// Nếu cả 2 có trạng thái PK nhóm, hoặc bang
                else if (((objPlayer.PKMode == (int)PKMode.Team || objPlayer.PKMode == (int) PKMode.Guild) && (targetPlayer.PKMode == (int)PKMode.Team || targetPlayer.PKMode == (int) PKMode.Guild)))
                {
                    return true;
                }
                /// Nếu một trong 2 có trạng thái PK thiện ác
                else if (objPlayer.PKMode == (int)PKMode.Moral || targetPlayer.PKMode == (int)PKMode.Moral)
                {
                    /// Nếu bản thân có trạng thái thiện ác và đối phương có sát khí
                    if (objPlayer.PKMode == (int)PKMode.Moral && targetPlayer.PKValue > 0)
                    {
                        return true;
                    }
                    /// Nếu đối phượng có trạng thái thiện ác và bản thân có sát khí
                    else if (targetPlayer.PKMode == (int)PKMode.Moral && objPlayer.PKValue > 0)
                    {
                        return true;
                    }
                }

                /// Không thỏa mãn thì không đánh nhau được
                return false;
            }
			/// Nếu một trong 2 không phải người
			else
			{
                /// Nếu một trong 2 là DynamicNPC
                if ((obj is Monster objMonster && objMonster.MonsterType == MonsterAIType.DynamicNPC) || (target is Monster targetMonster && targetMonster.MonsterType == MonsterAIType.DynamicNPC))
                {
                    return false;
                }

                /// Nếu bản thân là quái và có Camp -1 thì thôi
                if (obj is Monster && obj.Camp == -1)
                {
                    return false;
                }
                /// Nếu đối phương là quái và có Camp -1 thì thôi
                if (target is Monster && target.Camp == -1)
                {
                    return false;
                }

                /// Theo Camp
                return obj.Camp != target.Camp;
            }
        }

        /// <summary>
        /// Có thể tấn công đối phương được không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CanAttack(GameObject obj, GameObject target)
        {
            /// Nếu không phải kẻ địch
            if (!KTLogic.IsOpposite(obj, target))
            {
                return false;
            }

            /// Nếu là người chơi
            if (obj.IsPlayer() && target.IsPlayer())
            {
                KPlayer objPlayer = obj as KPlayer;
                KPlayer targetPlayer = target as KPlayer;

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(obj.CurrentMapCode);
                /// Nếu bản đồ không cho phép đánh nhau
                if (gameMap != null && !gameMap.AllowPK)
                {
                    return false;
                }

                /// Nếu đang trong trạng thái tỷ thí
                if (objPlayer.IsChallengeWith(targetPlayer))
                {
                    return true;
                }

                /// Nếu đang tuyên chiến cùng
                if (objPlayer.IsActiveFightWith(targetPlayer))
                {
                    return true;
                }

                /// Nếu là trạng thái PK đặc biệt
                if (objPlayer.PKMode == (int) PKMode.Custom && targetPlayer.PKMode == (int) PKMode.Custom)
                {
                    /// Trả về kết quả Camp khác nhau
                    return objPlayer.Camp != -1 && targetPlayer.Camp != -1 && objPlayer.Camp != targetPlayer.Camp;
                }

                /// Nếu cùng nhóm thì không cho đánh nhau
                if (KTLogic.IsTeamMate(objPlayer, targetPlayer))
                {
                    return false;
                }

                /// Nếu cùng bang hoặc tộc thì không cho đánh nhau
                if (KTLogic.IsTeamMate(objPlayer, targetPlayer) || KTLogic.IsSameFamily(objPlayer, targetPlayer) || KTLogic.IsGuildMate(objPlayer, targetPlayer))
                {
                    return false;
                }

                /// Nếu bản thân đang trong trạng thái luyện công
                if (objPlayer.PKMode == (int)PKMode.Peace)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// Nếu không phải người chơi
            return true;
        }

        /// <summary>
        /// Có phải đồng đội không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsTeamMate(KPlayer obj, KPlayer target)
        {
            return obj.TeamID != -1 && obj.TeamID == target.TeamID;
        }

        /// <summary>
        /// Có cùng bang không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsGuildMate(KPlayer obj, KPlayer target)
        {
            /// Nếu cả hai đều không có bang
            if (obj.GuildID <= 0 && target.GuildID <= 0)
            {
                return false;
            }
            return target.GuildID == obj.GuildID;
        }

        /// <summary>
        /// Có cùng tộc không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsSameFamily(KPlayer obj, KPlayer target)
        {
            /// Nếu cả hai đều không có tộc
            if (obj.FamilyID <= 0 && target.FamilyID <= 0)
            {
                return false;
            }
            return obj.FamilyID == target.FamilyID;
        }

        /// <summary>
        /// Kiểm tra đối tượng có nằm trong khoảng cách cho trước không
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static bool IsInDist(GameObject obj, GameObject target, float distance)
        {
            return KTLogic.GetDistance(obj, target) <= distance;
        }

        /// <summary>
        /// Trả về khoảng cách giữa 2 đối tượng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetDistance(GameObject obj, GameObject target)
        {
            /// Nếu khác bản đồ
            if (obj.CurrentMapCode != target.CurrentMapCode)
            {
                return -1;
            }

            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((int)obj.CurrentPos.X, (int)obj.CurrentPos.Y);
            UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

            return UnityEngine.Vector2.Distance(objPos, targetPos);
        }

        /// <summary>
        /// Trả về danh sách đối tượng xung quanh
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMap"></param>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetNearByObjectsAtPos<T>(int mapCode, int copyMap, UnityEngine.Vector2 pos, float distance, int count = -1) where T : GameObject
        {
            List<T> list = new List<T>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(mapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out GameMap map);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng GameObject
                                if (obj is T)
                                {
                                    T target = obj as T;

                                    /// Nếu không cùng phụ bản
                                    if (target.CurrentCopyMapID != copyMap)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(pos, targetPos);

                                    /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                    if (distanceToPoint <= distance)
                                    {
                                        /// Thêm đối tượng vào danh sách
                                        list.Add(target);
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);

                                        /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                        if (count != -1 && list.Count >= count)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách đối tượng xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetNearByObjects<T>(GameObject go, float distance, int count = -1) where T : GameObject
        {
            List<T> list = new List<T>();

            UnityEngine.Vector2 pos = new UnityEngine.Vector2((int)go.CurrentPos.X, (int)go.CurrentPos.Y);

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng GameObject
                                if (obj is T)
                                {
                                    T target = obj as T;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(pos, targetPos);

                                    /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                    if (distanceToPoint <= distance)
                                    {
                                        /// Thêm đối tượng vào danh sách
                                        list.Add(target);
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);

                                        /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                        if (count != -1 && list.Count >= count)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách người chơi xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<KPlayer> GetNearByPlayers(GameObject go, float distance, int count = -1)
        {
            List<KPlayer> list = new List<KPlayer>();

            UnityEngine.Vector2 pos = new UnityEngine.Vector2((int)go.CurrentPos.X, (int)go.CurrentPos.Y);

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng GameObject
                                if (obj is KPlayer)
                                {
                                    KPlayer target = obj as KPlayer;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(pos, targetPos);

                                    /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                    if (distanceToPoint <= distance)
                                    {
                                        /// Thêm đối tượng vào danh sách
                                        list.Add(target);
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);

                                        /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                        if (count != -1 && list.Count >= count)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách đội viên xung quanh
        /// </summary>
        /// <param name="player"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<KPlayer> GetNearByTeammates(KPlayer player, float distance, int count = -1)
        {
            List<KPlayer> list = new List<KPlayer>();

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)player.CurrentPos.X, (float)player.CurrentPos.Y);

            /// Duyệt toàn bộ danh sách đội viên trong nhóm
            foreach (KPlayer teammate in player.Teammates)
            {
                /// Nếu không trong cùng phụ bản
                if (teammate.CurrentCopyMapID != player.CurrentCopyMapID)
                {
                    continue;
                }

                /// Nếu nằm chung bản đồ
                if (teammate.CurrentMapCode == player.CurrentMapCode)
                {
                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)teammate.CurrentPos.X, (int)teammate.CurrentPos.Y);
                    /// Nếu nằm trong vùng tìm kiếm
                    if (UnityEngine.Vector2.Distance(objPos, targetPos) <= distance)
                    {
                        list.Add(teammate);
                    }

                    /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                    if (count != -1 && list.Count >= count)
                    {
                        break;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách đối tượng xung quanh có chung CampID
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<GameObject> GetNearBySameCampObject(GameObject go, float distance, int count = -1)
        {
            List<GameObject> list = new List<GameObject>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng GameObject
                                if (obj is GameObject)
                                {
                                    GameObject target = obj as GameObject;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.CanAttack(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng không phải là kẻ địch
                                    if (!target.IsDead() && !isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu là kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách đối tượng xung quanh có chung CampID
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetNearBySameCampObject<T>(T go, float distance, int count = -1) where T : GameObject
        {
            List<T> list = new List<T>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng GameObject
                                if (obj is T)
                                {
                                    T target = obj as T;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.IsOpposite(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng không phải là kẻ địch
                                    if (!target.IsDead() && !isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu là kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách người chơi hòa bình xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<KPlayer> GetNearByPeacePlayersReductCost(GameObject go, float distance, int count = -1)
        {
            List<KPlayer> list = new List<KPlayer>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng người chơi
                                if (obj is KPlayer)
                                {
                                    KPlayer target = obj as KPlayer;

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng cùng phe thì add vào danh sách
                                    if (!target.IsDead() && target.Camp == go.Camp)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu là kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách người chơi hòa bình xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<KPlayer> GetNearByPeacePlayers(GameObject go, float distance, int count = -1)
        {
            List<KPlayer> list = new List<KPlayer>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng người chơi
                                if (obj is KPlayer)
                                {
                                    KPlayer target = obj as KPlayer;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.IsOpposite(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng không phải là kẻ địch
                                    if (!target.IsDead() && !isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu là kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách kẻ địch xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<GameObject> GetNearByEnemies(GameObject go, float distance, int count = -1)
        {
            List<GameObject> list = new List<GameObject>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng có thể tấn công
                                if (obj is GameObject && obj != go)
                                {
                                    GameObject target = obj as GameObject;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }
                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.CanAttack(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng là kẻ địch
                                    if (!target.IsDead() && isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu không phải kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách kẻ địch xung quanh
        /// </summary>
        /// <param name="go"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetNearByEnemies<T>(GameObject go, float distance, int count = -1) where T : GameObject
        {
            List<T> list = new List<T>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);

            /// Vị trí bản thân
            UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);

            if (mapGrid != null && map != null)
            {
                UnityEngine.Vector2 pos = new UnityEngine.Vector2((float)go.CurrentPos.X, (float)go.CurrentPos.Y);
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng có thể tấn công
                                if (obj is T && obj != go)
                                {
                                    T target = obj as T;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.CanAttack(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(objPos, targetPos);

                                    /// Nếu đối tượng là kẻ địch
                                    if (!target.IsDead() && isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu không phải kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách kẻ địch xung quanh vị trí
        /// </summary>
        /// <param name="go"></param>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<GameObject> GetEnemiesAroundPos(GameObject go, UnityEngine.Vector2 pos, float distance, int count = -1)
        {
            List<GameObject> list = new List<GameObject>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);
            if (mapGrid != null && map != null)
            {
                /// Tọa độ lưới
                UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, pos);

                /// Nếu tọa độ này chưa được tìm
                if (!mark.Contains(gridPos.ToString()))
                {
                    /// Đánh dấu đã tìm quanh tọa độ này
                    mark.Add(gridPos.ToString());

                    /// Danh sách đối tượng xung quanh
                    List<object> objectsAround = mapGrid.FindObjects((int)pos.x, (int)pos.y, (int)distance, count != -1);

                    if (objectsAround != null)
                    {
                        /// Duyệt toàn bộ danh sách đối tượng xung quanh
                        foreach (object obj in objectsAround)
                        {
                            /// Nếu đối tượng chưa được xét
                            if (!markObject.Contains(obj))
                            {
                                /// Nếu là đối tượng có thể tấn công
                                if (obj is GameObject && obj != go)
                                {
                                    GameObject target = obj as GameObject;

                                    /// Nếu không trong cùng phụ bản
                                    if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                    {
                                        continue;
                                    }

                                    /// Vị trí đối tượng
                                    UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                    /// Có phải kẻ địch không
                                    bool isOpposite = KTLogic.CanAttack(go, target);
                                    /// Khoảng cách từ vị trí của đối tượng tới điểm
                                    float distanceToPoint = UnityEngine.Vector2.Distance(pos, targetPos);

                                    /// Nếu đối tượng là kẻ địch
                                    if (!target.IsDead() && isOpposite)
                                    {
                                        /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                        if (distanceToPoint <= distance)
                                        {
                                            /// Thêm đối tượng vào danh sách
                                            list.Add(target);
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);

                                            /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                            if (count != -1 && list.Count >= count)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    /// Nếu không phải kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                    else
                                    {
                                        /// Đánh dấu đã xét đối tượng
                                        markObject.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Trả về danh sách kẻ địch nằm trong phạm vi 2 điểm với bán kính quét tương ứng
        /// </summary>
        /// <param name="go"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="radius"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<GameObject> GetEnemiesBetweenTwoPoints(GameObject go, UnityEngine.Vector2 fromPos, UnityEngine.Vector2 toPos, float radius, int count = -1)
        {
            List<GameObject> list = new List<GameObject>();

            HashSet<string> mark = new HashSet<string>();
            HashSet<object> markObject = new HashSet<object>();
            /// Bản đồ hiện tại
            GameManager.MapGridMgr.DictGrids.TryGetValue(go.CurrentMapCode, out MapGrid mapGrid);
            GameManager.MapMgr.DictMaps.TryGetValue(go.CurrentMapCode, out GameMap map);
            if (mapGrid != null && map != null)
            {
                /// Vector hướng
                UnityEngine.Vector2 dirVector = toPos - fromPos;

                /// Tổng số điểm
                int totalPoints = (int)(dirVector.magnitude / radius) + 1;

                /// Danh sách các điểm nằm giữa 2 điểm cho trước
                List<UnityEngine.Vector2> points = KTMath.GetPointsBetweenTwoPoints(fromPos, toPos, totalPoints);

                ///// Debug hiện khối Debug Object
                //KT_TCPHandler.ShowDebugObjects(go.CurrentMapCode, points, (int)radius, 10f);

                /// Duyệt toàn bộ danh sách các điểm
                foreach (UnityEngine.Vector2 point in points)
                {
                    /// Tọa độ lưới
                    UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(map, point);

                    /// Nếu tọa độ này chưa được tìm
                    if (!mark.Contains(gridPos.ToString()))
                    {
                        /// Đánh dấu đã tìm quanh tọa độ này
                        mark.Add(gridPos.ToString());

                        /// Danh sách đối tượng xung quanh
                        List<object> objectsAround = mapGrid.FindObjects((int)point.x, (int)point.y, (int)radius, count != -1);

                        if (objectsAround != null)
                        {
                            /// Duyệt toàn bộ danh sách đối tượng xung quanh
                            foreach (object obj in objectsAround)
                            {
                                /// Nếu đối tượng chưa được xét
                                if (!markObject.Contains(obj))
                                {
                                    /// Nếu là đối tượng có thể tấn công
                                    if (obj is GameObject && obj != go)
                                    {
                                        GameObject target = obj as GameObject;

                                        /// Nếu không trong cùng phụ bản
                                        if (target.CurrentCopyMapID != go.CurrentCopyMapID)
                                        {
                                            continue;
                                        }

                                        /// Vị trí đối tượng
                                        UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((int)target.CurrentPos.X, (int)target.CurrentPos.Y);

                                        /// Có phải kẻ địch không
                                        bool isOpposite = KTLogic.CanAttack(go, target);
                                        /// Khoảng cách từ vị trí của đối tượng tới điểm
                                        float distanceToPoint = UnityEngine.Vector2.Distance(point, targetPos);

                                        /// Nếu đối tượng là kẻ địch
                                        if (!target.IsDead() && isOpposite)
                                        {
                                            /// Nếu đối tượng nằm trong vùng ảnh hưởng
                                            if (distanceToPoint <= radius)
                                            {
                                                /// Thêm đối tượng vào danh sách
                                                list.Add(target);
                                                /// Đánh dấu đã xét đối tượng
                                                markObject.Add(obj);

                                                /// Nếu số mục tiêu đã vượt quá giới hạn tìm kiếm
                                                if (count != -1 && list.Count >= count)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        /// Nếu không phải kẻ địch thì đánh dấu để sau không bị lặp lại nữa
                                        else
                                        {
                                            /// Đánh dấu đã xét đối tượng
                                            markObject.Add(obj);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        #endregion Skill Logic

        #region Kỹ năng

        /// <summary>
        /// Danh sách kỹ năng tấn công tân thủ
        /// </summary>
        private static readonly List<int> ListNewbieAttackSkill = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        /// <summary>
        /// Thêm kỹ năng tân thủ cho người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        public static void AddNewbieAttackSkills(KPlayer player)
        {
            /// Nếu không có kỹ năng nào
            if (player.SkillDataList == null)
            {
                player.SkillDataList = new List<SkillData>();
            }

            /// Duyệt danh sách các kỹ năng tân thủ
            foreach (int skillID in KTLogic.ListNewbieAttackSkill)
            {
                SkillData dbSkill = player.SkillDataList.Where(x => x.SkillID == skillID).FirstOrDefault();
                /// Nếu chưa tồn tại
                if (dbSkill == null)
                {
                    dbSkill = new SkillData()
                    {
                        SkillID = skillID,
                        SkillLevel = 1,
                        BonusLevel = 0,
                        CanStudy = true,
                        LastUsedTick = 0,
                        CooldownTick = 0,
                    };
                    player.SkillDataList.Add(dbSkill);
                }
                /// Nếu đã tồn tại
                else
                {
                    dbSkill.SkillLevel = 1;
                    dbSkill.BonusLevel = 0;
                    dbSkill.CanStudy = true;
                    dbSkill.LastUsedTick = 0;
                    dbSkill.CooldownTick = 0;
                }
            }
        }

        #endregion Kỹ năng
    }
}