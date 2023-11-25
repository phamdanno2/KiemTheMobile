using GameServer.Core.Executor;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameEvents.Interface;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.GameEvents.EmperorTomb
{
    /// <summary>
    /// Script chính thực thi sự kiện Tần Lăng
    /// </summary>
    public class EmperorTomb_Script_Main : GameMapEvent
    {
        #region Define
        /// <summary>
        /// Thông tin tầng
        /// </summary>
        private readonly EmperorTomb.EventDetail.Stage stageInfo;

        /// <summary>
        /// Danh sách Boss đã được tạo ra
        /// </summary>
        private readonly HashSet<string> createdBosses;

        /// <summary>
        /// Thời điểm tiếp theo tạo MiniBoss tương ứng
        /// </summary>
        private readonly ConcurrentDictionary<EmperorTomb.EventDetail.Stage.MiniBossInfo, long> nextCreateMiniBosses;

        /// <summary>
        /// Thời điểm Tick lần trước
        /// </summary>
        private long lastTicks = 0;
        #endregion

        #region Core GameMapEvent
        /// <summary>
        /// Script chính thực thi sự kiện Tần Lăng
        /// </summary>
        /// <param name="map"></param>
        /// <param name="activity"></param>
        public EmperorTomb_Script_Main(GameMap map, KTActivity activity) : base(map, activity)
        {
            this.stageInfo = EmperorTomb.Event.Stages[map.MapCode];
            this.createdBosses = new HashSet<string>();
            this.nextCreateMiniBosses = new ConcurrentDictionary<EmperorTomb.EventDetail.Stage.MiniBossInfo, long>();
        }

        /// <summary>
        /// Sự kiện bắt đầu
        /// </summary>
        protected override void OnStart()
        {
            /// Xóa toàn bộ quái
            this.RemoveAllMonsters();
            /// Xóa toàn bộ NPC
            this.RemoveAllNPCs();
            /// Xóa toàn bộ cổng dịch chuyển
            this.RemoveAllDynamicAreas();

            /// Tạo quái
            this.CreateMonsters();
            /// Tạo NPC
            this.CreateNPCs();
            /// Tạo cổng dịch chuyển
            this.CreateTeleports();
        }

        /// <summary>
        /// Sự kiện Tick
        /// </summary>
        protected override void OnTick()
        {
            /// Nếu chưa đến thời gian
            if (KTGlobal.GetCurrentTimeMilis() - this.lastTicks < 5000)
            {
                return;
            }
            /// Đánh dấu thời gian
            this.lastTicks = KTGlobal.GetCurrentTimeMilis();
            /// Tạo Boss ở thời gian tương ứng
            this.SpawnBossInTime();
            /// Tạo MiniBoss
            this.SpawnMiniBossInTime();
        }

        /// <summary>
        /// Sự kiện kết thúc
        /// </summary>
        protected override void OnClose()
        {
            /// Làm rỗng danh sách Boss đã tạo
            this.createdBosses.Clear();

            /// Duyệt danh sách người chơi
            foreach (KPlayer player in this.GetPlayers())
            {
                /// Đưa ra khỏi Tần Lăng
                EmperorTomb_ActivityScript.KickOut(player);
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi vào bản đồ sự kiện
        /// </summary>
        /// <param name="player"></param>
        public override void OnPlayerEnter(KPlayer player)
        {
            base.OnPlayerEnter(player);

            /// Mở bảng thông báo hoạt động
            this.OpenEventBroadboard(player, 400);
            /// Thời gian còn lại ở Tần Lăng
            int totalSecLeft = EmperorTomb_ActivityScript.GetTodayTotalSecLeft(player);
            /// Cập nhật thông tin
            this.UpdateEventDetailsToPlayers("Thời gian còn lại", totalSecLeft, "Đánh quái và săn Boss");

            /// Hệ số nhân phi phong
            int mantleMultiply = player.GetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_LastMantleMultiply);
            /// Nếu chưa cập nhật hệ số nhân phi phong
            if (mantleMultiply == -1)
            {
                /// Cập nhật lại
                mantleMultiply = EmperorTomb_ActivityScript.GetMultiplyNightPearl(player);
                /// Lưu lại
                player.SetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_LastMantleMultiply, mantleMultiply);
                /// Ghi Log
                LogManager.WriteLog(LogTypes.EmperorTomb, string.Format("[MULTIPLY] {0} (ID: {1}), Multiply = {2}", player.RoleID, player.RoleName, mantleMultiply));
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi rời khỏi bản đồ sự kiện
        /// </summary>
        /// <param name="player"></param>
        /// <param name="toMap"></param>
        public override void OnPlayerLeave(KPlayer player, GameMap toMap)
        {
            base.OnPlayerLeave(player, toMap);

            /// Toác
            if (toMap == null)
            {
                return;
            }

            /// Nếu bản đồ đích không thuộc Tần Lăng
            if (!EmperorTomb.Event.Stages.ContainsKey(toMap.MapCode))
            {
                /// Đóng bảng thông báo hoạt động
                this.CloseEventBroadboard(player, 400);
            }

            /// Hủy khu an toàn
            player.IsInsideSafeZone = false;
        }

        /// <summary>
        /// Sự kiện khi người chơi ấn nút hồi sinh
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool OnPlayerClickReliveButton(KPlayer player)
        {
            /// Đưa về khu an toàn tầng 1
            PlayerManager.Relive(player, EmperorTomb.Event.Relive.MapID, EmperorTomb.Event.Relive.PosX, EmperorTomb.Event.Relive.PosY, 100, 100, 100);
            /// Trả về True
            return true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Tạo quái
        /// </summary>
        private void CreateMonsters()
        {
            /// Duyệt danh sách thiết lập quái
            foreach (EmperorTomb.EventDetail.Stage.MonsterInfo monsterInfo in this.stageInfo.Monsters)
            {
                /// Hướng quay
                Entities.Direction dir = Entities.Direction.NONE;
                /// Mức máu
                int hp = monsterInfo.BaseHP + monsterInfo.HPIncreaseEachLevel * EmperorTomb.Config.MonsterLevel;
                /// Ngũ hành
                KE_SERIES_TYPE series = monsterInfo.Series == -1 ? KE_SERIES_TYPE.series_none : (KE_SERIES_TYPE) monsterInfo.Series;

                /// Tạo quái
                GameManager.MonsterZoneMgr.AddDynamicMonsters(this.Map.MapCode, monsterInfo.ID, -1, 1, monsterInfo.PosX, monsterInfo.PosY, monsterInfo.Name, monsterInfo.Title, hp, EmperorTomb.Config.MonsterLevel, dir, series, monsterInfo.AIType, monsterInfo.RespawnTicks, -1, monsterInfo.AIScriptID, null, (monster) => {
                    /// Nếu có kỹ năng
                    if (monsterInfo.Skills.Count > 0)
                    {
                        /// Duyệt danh sách kỹ năng
                        foreach (SkillLevelRef skill in monsterInfo.Skills)
                        {
                            /// Thêm vào danh sách kỹ năng dùng của quái
                            monster.CustomAISkills.Add(skill);
                        }
                    }

                    /// Nếu có vòng sáng
                    if (monsterInfo.Auras.Count > 0)
                    {
                        /// Duyệt danh sách vòng sáng
                        foreach (SkillLevelRef aura in monsterInfo.Auras)
                        {
                            /// Kích hoạt vòng sáng
                            monster.UseSkill(aura.SkillID, aura.Level, monster);
                        }
                    }
                }, 65535, () => {
                    /// Nếu hoạt động đã bị hủy thì thôi
                    return !this.isDisposed;
                });
            }
        }

        /// <summary>
        /// Tạo Boss tương ứng
        /// </summary>
        /// <param name="bossInfo"></param>
        /// <param name="onDie"></param>
        /// <param name="onTick"></param>
        /// <param name="onTakeDamage"></param>
        private void CreateBoss(EmperorTomb.EventDetail.Stage.BossInfo bossInfo, Action<GameObject> onDie, Action<GameObject> onTick)
        {
            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Mức máu
            int hp = bossInfo.BaseHP + bossInfo.HPIncreaseEachLevel * EmperorTomb.Config.MonsterLevel;
            /// Ngũ hành
            KE_SERIES_TYPE series = bossInfo.Series == -1 ? KE_SERIES_TYPE.series_none : (KE_SERIES_TYPE) bossInfo.Series;

            /// Tạo Boss
            GameManager.MonsterZoneMgr.AddDynamicMonsters(this.Map.MapCode, bossInfo.ID, -1, 1, bossInfo.PosX, bossInfo.PosY, bossInfo.Name, bossInfo.Title, hp, EmperorTomb.Config.MonsterLevel, dir, series, bossInfo.AIType, -1, -1, bossInfo.AIScriptID, null, (boss) => {
                /// Thông báo
                this.NotifyAllPlayers(string.Format("{0} đã xuất hiện, anh hùng hào kiệt hãy nhanh chân tiêu diệt!", boss.RoleName));
                /// Cho hiện trên Minimap
                boss.UpdatePositionToLocalMapContinuously = true;
                /// Thực thi sự kiện Tick
                boss.OnTick = () => {
                    onTick?.Invoke(boss);
                };

                /// Nếu có kỹ năng
                if (bossInfo.Skills.Count > 0)
                {
                    /// Duyệt danh sách kỹ năng
                    foreach (SkillLevelRef skill in bossInfo.Skills)
                    {
                        /// Thêm vào danh sách kỹ năng dùng của quái
                        boss.CustomAISkills.Add(skill);
                    }
                }

                /// Nếu có vòng sáng
                if (bossInfo.Auras.Count > 0)
                {
                    /// Duyệt danh sách vòng sáng
                    foreach (SkillLevelRef aura in bossInfo.Auras)
                    {
                        /// Kích hoạt vòng sáng
                        boss.UseSkill(aura.SkillID, aura.Level, boss);
                    }
                }
            }, 65535, null, onDie);
        }

        /// <summary>
        /// Tạo cổng dịch chuyển tương ứng
        /// </summary>
        private void CreateTeleports()
        {
            /// Duyệt danh sách cổng dịch chuyển
			foreach (EmperorTomb.EventDetail.Stage.TeleportInfo teleportInfo in this.stageInfo.Teleports)
            {
                /// Tạo cổng dịch chuyển
                KDynamicArea teleport = KTDynamicAreaManager.Add(this.Map.MapCode, -1, teleportInfo.Name, 5340, teleportInfo.PosX, teleportInfo.PosY, -1, 2f, 100, -1, null);
                teleport.OnEnter = (obj) => {
                    if (obj is KPlayer player)
                    {
                        this.DoTeleportLogic(teleportInfo, player);
                    }
                };
            }
        }

        /// <summary>
        /// Tạo NPC tương ứng
        /// </summary>
        private void CreateNPCs()
        {
            /// Duyệt danh sách NPC
            foreach (EmperorTomb.EventDetail.Stage.NPCInfo npcInfo in this.stageInfo.NPCs)
            {
                /// Tạo NPC
                NPCGeneralManager.AddNewNPC(this.Map.MapCode, -1, npcInfo.ID, npcInfo.PosX, npcInfo.PosY, "", "", Entities.Direction.DOWN_RIGHT, npcInfo.ScriptID, "", (npc) => {
                    npc.VisibleOnMinimap = true;
                    npc.MinimapName = npc.Name;
                });
            }
        }

        /// <summary>
        /// Tạo Boss ở thời gian tương ứng
        /// </summary>
        private void SpawnBossInTime()
        {
            /// Thời gian hiện tại
            DateTime now = DateTime.Now;
            /// Giờ
            int hour = now.Hour;
            /// Phút
            int minute = now.Minute;
            /// Ngày
            int day = now.Day;
            /// Tháng
            int month = now.Month;
            /// Năm
            int year = now.Year;
            /// Mã hóa thời gian
            string dateTimeKey = string.Format("{0}_{1}_{2}_{3}_{4}", day, month, year, hour, minute);
            /// Nếu đã tạo Boss ở thời gian này rồi thì thôi
            if (this.createdBosses.Contains(dateTimeKey))
            {
                return;
            }
            /// Thông tin Boss ở thời gian này
            EmperorTomb.EventDetail.Stage.BossInfo bossInfo = this.stageInfo.Bosses.Where(x => x.SpawnAt.Hour == hour && x.SpawnAt.Minute == minute).FirstOrDefault();
            /// Nếu toác
            if (bossInfo == null)
            {
                return;
            }
            /// Đánh dấu đã tạo Boss ở thời gian này
            this.createdBosses.Add(dateTimeKey);
            /// Thời điểm lần trước cập nhật Boss thuộc về đội nào
            long lastTickUpdateBelonging = KTGlobal.GetCurrentTimeMilis();
            /// Tạo Boss tương ứng
            this.CreateBoss(bossInfo, (killer) => {
                /// Nếu không phải người chơi
                if (!(killer is KPlayer killerPlayer))
                {
                    return;
                }

                /// Thực hiện Logic giết Boss
                this.ProcessPlayerKillBossLogic(killerPlayer, bossInfo);
            }, (boss) => {
                /// % máu hiện tại
                int nHPPercent = (int) (boss.m_CurrentLife / (float) boss.m_CurrentLifeMax * 100);
                /// Nếu còn trên 50% máu
                if (nHPPercent >= 50)
                {
                    /// Miễn dịch sát thương ngoại
                    boss.m_ImmuneToPhysicDamage = true;
                    boss.m_ImmuneToMagicDamage = false;
                }
                /// Nếu còn dưới 50% máu
                else
                {
                    /// Miễn dịch sát thương nội
                    boss.m_ImmuneToMagicDamage = true;
                    boss.m_ImmuneToPhysicDamage = false;
                }

                /// Nếu chưa đến thời điểm cập nhật
                if (KTGlobal.GetCurrentTimeMilis() - lastTickUpdateBelonging < 5000)
                {
                    return;
                }
                /// Đánh dấu thời điểm cập nhật
                lastTickUpdateBelonging = KTGlobal.GetCurrentTimeMilis();

                /// Đối tượng quái tương ứng
                Monster monster = boss as Monster;
                /// Toác
                if (monster == null)
                {
                    return;
                }

                /// Nếu có sát thương
                if (monster.DamageTakeRecord.Count > 0)
                {
                    /// Thời điểm hiện tại
                    long currentTicks = TimeUtil.NOW();

                    /// Lấy ra toàn bộ các damage mà 30s chưa cập nhật
                    List<Monster.DamgeGroup> needRessetDamages = monster.DamageTakeRecord.Values.Where(x => currentTicks - x.LastUpdateTime > 30000).ToList();
                    /// Reset Damage
                    monster.ResetDamge(needRessetDamages);

                    /// Danh sách sát thương cao nhất
                    Monster.DamgeGroup topDamage = monster.DamageTakeRecord.Values.OrderByDescending(x => x.TotalDamage).FirstOrDefault();
                    /// Nếu có sát thương
                    if (topDamage.TotalDamage > 0)
                    {
                        /// Thời gian Reset
                        long resetTime = (30000 - currentTicks + topDamage.LastUpdateTime) / 1000;
                        /// Nếu là nhóm
                        if (topDamage.IsTeam)
                        {
                            /// Đội trưởng
                            KPlayer teamLeader = KTTeamManager.GetTeamLeader(topDamage.ID);
                            /// Nếu tồn tại
                            if (teamLeader != null)
                            {
                                monster.Title = "<b><color=#00ff2a>(Thuộc đội: " + teamLeader.RoleName + ", còn: " + resetTime + "s)</color></b>";
                            }
                        }
                        else
                        {
                            /// Người chơi tương ứng
                            KPlayer player = GameManager.ClientMgr.FindClient(topDamage.ID);
                            /// Nếu tồn tại
                            if (player != null)
                            {
                                monster.Title = "<b><color=#00ff2a>(Thuộc về: " + player.RoleName + ", còn: " + resetTime + "s)</color></b>";
                            }
                        }
                    }
                    else
                    {
                        monster.Title = "(Không thuộc về ai)";
                    }
                }
                else
                {
                    monster.Title = "(Không thuộc về ai)";
                }
            });
        }

        /// <summary>
        /// Tạo MiniBoss ở thời gian tương ứng
        /// </summary>
        private void SpawnMiniBossInTime()
        {
            /// Duyệt danh sách MiniBoss
            foreach (EmperorTomb.EventDetail.Stage.MiniBossInfo miniBossInfo in this.stageInfo.MiniBosses)
            {
                /// Nếu chưa tồn tại trong danh sách
                if (!this.nextCreateMiniBosses.TryGetValue(miniBossInfo, out long nextCreateMiniBossTicks))
                {
                    /// Tạo mới
                    this.nextCreateMiniBosses[miniBossInfo] = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(miniBossInfo.MinDuration, miniBossInfo.MaxDuration);
                    /// Bỏ qua
                    continue;
                }

                /// Nếu chưa đánh bại Boss cũ
                if (nextCreateMiniBossTicks == -1)
                {
                    /// Bỏ qua
                    continue;
                }

                /// Nếu chưa đến thời gian
                if (KTGlobal.GetCurrentTimeMilis() < nextCreateMiniBossTicks)
                {
                    /// Bỏ qua
                    continue;
                }
                /// Đánh dấu thời gian tiếp theo tạo MiniBoss
                this.nextCreateMiniBosses[miniBossInfo] = -1;

                /// Chọn Boss ngẫu nhiên
                EmperorTomb.EventDetail.Stage.BossInfo bossInfo = miniBossInfo.Bosses[KTGlobal.GetRandomNumber(0, miniBossInfo.Bosses.Count - 1)];

                /// Toác gì đó
                if (bossInfo == null)
                {
                    /// Đánh dấu thời gian tiếp theo tạo MiniBoss
                    this.nextCreateMiniBosses[miniBossInfo] = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(miniBossInfo.MinDuration, miniBossInfo.MaxDuration);
                    /// Bỏ qua
                    continue;
                }

                /// Tạo Boss
                this.CreateBoss(bossInfo, (killer) => {
                    /// Đánh dấu thời gian tiếp theo tạo MiniBoss
                    this.nextCreateMiniBosses[miniBossInfo] = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(miniBossInfo.MinDuration, miniBossInfo.MaxDuration);

                    /// Nếu không phải người chơi
                    if (!(killer is KPlayer killerPlayer))
                    {
                        return;
                    }

                    /// Thực hiện Logic giết Boss
                    this.ProcessPlayerKillBossLogic(killerPlayer, bossInfo);
                }, (boss) => {
                    /// Đối tượng quái tương ứng
                    Monster monster = boss as Monster;
                    /// Toác
                    if (monster == null)
                    {
                        return;
                    }

                    /// Nếu có sát thương
                    if (monster.DamageTakeRecord.Count > 0)
                    {
                        /// Thời điểm hiện tại
                        long currentTicks = TimeUtil.NOW();

                        /// Lấy ra toàn bộ các damage mà 30s chưa cập nhật
                        List<Monster.DamgeGroup> needRessetDamages = monster.DamageTakeRecord.Values.Where(x => currentTicks - x.LastUpdateTime > 30000).ToList();
                        /// Reset Damage
                        monster.ResetDamge(needRessetDamages);

                        /// Danh sách sát thương cao nhất
                        Monster.DamgeGroup topDamage = monster.DamageTakeRecord.Values.OrderByDescending(x => x.TotalDamage).FirstOrDefault();
                        /// Nếu có sát thương
                        if (topDamage.TotalDamage > 0)
                        {
                            /// Thời gian Reset
                            long resetTime = (30000 - currentTicks + topDamage.LastUpdateTime) / 1000;
                            /// Nếu là nhóm
                            if (topDamage.IsTeam)
                            {
                                /// Đội trưởng
                                KPlayer teamLeader = KTTeamManager.GetTeamLeader(topDamage.ID);
                                /// Nếu tồn tại
                                if (teamLeader != null)
                                {
                                    monster.Title = "<b><color=#00ff2a>(Thuộc đội: " + teamLeader.RoleName + ", còn: " + resetTime + "s)</color></b>";
                                }
                            }
                            else
                            {
                                /// Người chơi tương ứng
                                KPlayer player = GameManager.ClientMgr.FindClient(topDamage.ID);
                                /// Nếu tồn tại
                                if (player != null)
                                {
                                    monster.Title = "<b><color=#00ff2a>(Thuộc về: " + player.RoleName + ", còn: " + resetTime + "s)</color></b>";
                                }
                            }
                        }
                        else
                        {
                            monster.Title = "(Không thuộc về ai)";
                        }
                    }
                    else
                    {
                        monster.Title = "(Không thuộc về ai)";
                    }
                });
            }
        }

        /// <summary>
        /// Thực hiện Logic cổng dịch chuyển
        /// </summary>
        /// <param name="teleportInfo"></param>
        /// <param name="player"></param>
        private void DoTeleportLogic(EmperorTomb.EventDetail.Stage.TeleportInfo teleportInfo, KPlayer player)
        {
            /// Nếu thông tin bản đồ đến không tồn tại
            if (!EmperorTomb.Event.Stages.TryGetValue(teleportInfo.ToMapID, out EmperorTomb.EventDetail.Stage nextStageInfo))
            {
                return;
            }

            /// Thông tin bản đồ kế tiếp
            GameMap map = GameManager.MapMgr.GetGameMap(nextStageInfo.MapID);
            /// Toác
            if (map == null)
            {
                PlayerManager.ShowNotification(player, "Bản đồ chưa được mở!");
                return;
            }

            /// Thông tin Tần Lăng hôm nay
            int todayData = player.GetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_TodayData);
            /// Nếu chưa có gì
            if (todayData == -1)
            {
                todayData = 0;
            }
            /// Chuyển sang dạng String
            string strData = todayData.ToString();
            /// Nếu ngày hôm nay chưa ăn Dạ Minh Châu ở tầng tương ứng
            if (!strData.Contains(nextStageInfo.ID.ToString()))
            {
                /// Số lượng Dạ Minh Châu yêu cầu
                int nightPearlCount = nextStageInfo.RequireNightPearl * (EmperorTomb_ActivityScript.GetMultiplyNightPearl(player) + 1);

                /// Nếu yêu cầu Dạ Minh Châu
                if (nightPearlCount > 0)
                {
                    /// Số Dạ Minh Châu hiện có
                    int currentNightPearl = ItemManager.GetItemCountInBag(player, EmperorTomb.Config.NightPearlItemID);

                    /// Nếu trên người không có Dạ Minh Châu
                    if (currentNightPearl < nightPearlCount)
                    {
                        /// Thông báo
                        PlayerManager.ShowNotification(player, string.Format("Khí độc tại [{0}] rất nhiều, để đảm bảo an toàn, ngươi cần sử dụng {1} viên Tần Lăng Dạ Minh Châu mới có thể đi tiếp.", map.MapName, nightPearlCount));
                        /// Bỏ qua
                        return;
                    }

                    /// Xóa Dạ Minh Châu
                    if (!ItemManager.RemoveItemFromBag(player, EmperorTomb.Config.NightPearlItemID, nightPearlCount))
                    {
                        /// Số Dạ Minh Châu còn lại
                        int _afterNightPearl = ItemManager.GetItemCountInBag(player, EmperorTomb.Config.NightPearlItemID);

                        /// Ghi Log
                        LogManager.WriteLog(LogTypes.EmperorTomb, string.Format("[FAILED] Player {0} (ID: {1}), total value: {2}, cost {3} Night Pearl to enter stage {4}. Before Night Pearl: {5}, after Night Pearl: {6}", player.RoleName, player.RoleID, player.GetTotalValue(), nightPearlCount, nextStageInfo.ID, currentNightPearl, _afterNightPearl));
                        /// Thông báo
                        PlayerManager.ShowNotification(player, "Không thể xóa Tần Lăng Dạ Minh Châu, hãy thử lại!");
                        /// Bỏ qua
                        return;
                    }

                    /// Số Dạ Minh Châu còn lại
                    int afterNightPearl = ItemManager.GetItemCountInBag(player, EmperorTomb.Config.NightPearlItemID);

                    /// Ghi Log
                    LogManager.WriteLog(LogTypes.EmperorTomb, string.Format("[SUCCESS] Player {0} (ID: {1}), total value: {2}, cost {3} Night Pearl to enter stage {4}. Before Night Pearl: {5}, after Night Pearl: {6}", player.RoleName, player.RoleID, player.GetTotalValue(), nightPearlCount, nextStageInfo.ID, currentNightPearl, afterNightPearl));
                }
                /// Nếu không yêu cầu Dạ Minh Châu
                else
                {
                    /// Ghi Log
                    LogManager.WriteLog(LogTypes.EmperorTomb, string.Format("[SUCCESS] Player {0} (ID: {1}), total value: {2}, freely enter stage {3}.", player.RoleName, player.RoleID, player.GetTotalValue(), nextStageInfo.ID));
                }

                /// Đánh dấu đã ăn Dạ Minh Châu ở tầng này
                strData += nextStageInfo.ID.ToString();
                /// Lưu lại kết quả
                todayData = int.Parse(strData);
                player.SetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_TodayData, todayData);
            }

            /// Dịch chuyển đến tầng tương ứng
            GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, teleportInfo.ToMapID, teleportInfo.ToPosX, teleportInfo.ToPosY, (int) player.CurrentDir);
        }

        /// <summary>
        /// Thực thi Logic người chơi giết Boss
        /// </summary>
        /// <param name="killerPlayer"></param>
        /// <param name="bossInfo"></param>
        private void ProcessPlayerKillBossLogic(KPlayer killerPlayer, EmperorTomb.EventDetail.Stage.BossInfo bossInfo)
        {
            /// Nếu thằng này không có bang và tộc
            if (killerPlayer.GuildID <= 0 && killerPlayer.FamilyID <= 0)
            {
                /// Thông báo
                this.NotifyAllPlayers(string.Format("Người chơi {0} đã đánh bại {1}.", killerPlayer.RoleName, bossInfo.Name));

                /// Tăng uy danh cho thằng giết
                killerPlayer.Prestige += bossInfo.Prestige;
                /// Thông báo
                KTGlobal.SendDefaultChat(killerPlayer, string.Format("Nhận được <color=yellow>{0} Uy danh</color>.", bossInfo.Prestige));

                /// Nếu có nhóm
                if (killerPlayer.TeamID != -1 && KTTeamManager.IsTeamExist(killerPlayer.TeamID))
                {
                    /// Duyệt danh sách thành viên nhóm
                    foreach (KPlayer teammate in killerPlayer.Teammates)
                    {
                        /// Nếu là bản thân
                        if (teammate == killerPlayer)
                        {
                            continue;
                        }

                        /// Tăng uy danh
                        teammate.Prestige += bossInfo.Prestige;
                        /// Thông báo
                        KTGlobal.SendDefaultChat(teammate, string.Format("Nhận được <color=yellow>{0} Uy danh</color>.", bossInfo.Prestige));
                    }
                }
            }
            else
            {
                /// Thông báo
                this.NotifyAllPlayers(string.Format("Người chơi {0} thuộc bang hội {1} đã đánh bại {2}.", killerPlayer.RoleName, killerPlayer.GuildName, bossInfo.Name));

                /// Tăng uy danh cho thằng giết
                killerPlayer.Prestige += bossInfo.Prestige;
                /// Thông báo
                KTGlobal.SendDefaultChat(killerPlayer, string.Format("Nhận được <color=yellow>{0} Uy danh</color>.", bossInfo.Prestige));

                /// Danh sách người chơi
                List<KPlayer> players = this.GetPlayers();
                /// Duyệt danh sách người chơi
                foreach (KPlayer player in players)
                {
                    /// Nếu là bản thân
                    if (player == killerPlayer)
                    {
                        continue;
                    }
                    /// Nếu khác bang và tộc
                    else if (player.GuildID != killerPlayer.GuildID && player.FamilyID != killerPlayer.FamilyID)
                    {
                        continue;
                    }
                    /// Nếu khác đội
                    else if (killerPlayer.TeamID == -1 || !KTTeamManager.IsTeamExist(killerPlayer.TeamID) || killerPlayer.TeamID != player.TeamID)
                    {
                        continue;
                    }

                    /// Tăng uy danh
                    player.Prestige += bossInfo.Prestige;
                    /// Thông báo
                    KTGlobal.SendDefaultChat(player, string.Format("Nhận được <color=yellow>{0} Uy danh</color>.", bossInfo.Prestige));
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sự kiện Tick người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        public void OnPlayerTick(KPlayer player)
        {
            /// Nếu chưa chuyển Map xong
            if (player.WaitingForChangeMap)
            {
                return;
            }

            /// Nếu đã đến thời gian
            if (KTGlobal.GetCurrentTimeMilis() - player.LastEmperorTombTicks >= 10000)
            {
                /// Mở bảng thông báo hoạt động
                this.OpenEventBroadboard(player, 400);
                /// Thời gian còn lại ở Tần Lăng
                int totalSecLeft = EmperorTomb_ActivityScript.GetTodayTotalSecLeft(player);
                /// Cập nhật thông tin
                this.UpdateEventDetailsToPlayers("Thời gian còn lại", totalSecLeft, "Đánh quái và săn Boss");
            }
            /// Đánh dấu thời gian Tick
            player.LastEmperorTombTicks = KTGlobal.GetCurrentTimeMilis();

            /// Vị trí của bản thân
            UnityEngine.Vector2 currentPos = new UnityEngine.Vector2(player.PosX, player.PosY);

            /// Có tìm thấy khu an toàn ở vị trí đang đứng không
            bool isFound = false;
            /// Duyệt danh sách khu an toàn
            foreach (EmperorTomb.EventDetail.Stage.SafeZoneInfo safeZoneInfo in this.stageInfo.SafeZones)
            {
                /// Vị trí khu an toàn
                UnityEngine.Vector2 safeZonePos = new UnityEngine.Vector2(safeZoneInfo.PosX, safeZoneInfo.PosY);
                /// Khoảng cách
                float distance = UnityEngine.Vector2.Distance(currentPos, safeZonePos);
                /// Nếu nằm trong phạm vi
                if (distance < safeZoneInfo.Radius)
                {
                    /// Đánh dấu đang ở trong khu an toàn
                    player.IsInsideSafeZone = true;
                    /// Đánh dấu có tìm thấy
                    isFound = true;
                    /// Thoát
                    break;
                }
            }

            /// Nếu không tìm thấy
            if (!isFound)
            {
                /// Đánh dấu nằm ngoài khu an toàn
                player.IsInsideSafeZone = false;

                /// Hệ số phi phong hiện tại
                int currentMantleMultiply = EmperorTomb_ActivityScript.GetMultiplyNightPearl(player);
                /// Hệ số nhân phi phong
                int mantleMultiply = player.GetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_LastMantleMultiply);
                /// Nếu khác nhau
                if (mantleMultiply != -1 && mantleMultiply != currentMantleMultiply)
                {
                    PlayerManager.ShowNotification(player, "Cấp độ tài phú không phù hợp với lần đầu vào Tần Lăng trong ngày, tự động bị trục xuất!");
                    EmperorTomb_ActivityScript.KickOut(player);

                    /// Ghi Log
                    LogManager.WriteLog(LogTypes.EmperorTomb, string.Format("[KICKOUT] {0} (ID: {1}), CurrentMultiply = {2}, StartMultiply = {3}", player.RoleName, player.RoleID, currentMantleMultiply, mantleMultiply));
                    return;
                }
            }
        }
        #endregion
    }
}
