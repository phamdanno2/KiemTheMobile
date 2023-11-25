using GameServer.KiemThe.CopySceneEvents.Model;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.XiaoYaoGu
{
	/// <summary>
	/// Script ải Tiêu Dao Cốc - mở cơ quan, giết quái và boss
	/// </summary>
	public class XoYo_Script_UnlockTriggerAndKillMonster : CopySceneEvent
	{
		#region Constants
		/// <summary>
		/// Thời gian mỗi lần thông báo thông tin sự kiện tới người chơi
		/// </summary>
		private const int NotifyActivityInfoToPlayersEveryTick = 5000;
		#endregion

		#region Private fields
		/// <summary>
		/// Đối tượng Mutex dùng khóa Lock
		/// </summary>
		private readonly object Mutex = new object();

		/// <summary>
		/// Thời điểm thông báo cập nhật thông tin sự kiện tới tất cả người chơi lần trước
		/// </summary>
		private long LastNotifyTick;

		/// <summary>
		/// Bước hiện tại của hoạt động
		/// </summary>
		private int nStep;

		/// <summary>
		/// Danh sách quái đang chờ sinh ra theo thời gian
		/// </summary>
		private readonly List<XoYo.MonsterInfo> waitToBeSpawnedMonsters = new List<XoYo.MonsterInfo>();

		/// <summary>
		/// Danh sách Boss đang chờ sinh ra theo thời gian
		/// </summary>
		private readonly List<XoYo.BossInfo> waitToBeSpawnedBosses = new List<XoYo.BossInfo>();

		/// <summary>
		/// Thời điểm hoàn thành ải
		/// </summary>
		private long completedTicks = 0;

		/// <summary>
		/// Đã tạo Boss sau khi đánh chết toàn bộ quái chưa
		/// </summary>
		private bool CreatedBossAfterKillingAllMonsters = false;
		#endregion

		#region Properties
		/// <summary>
		/// Thông tin ải
		/// </summary>
		public XoYo.MapInfo MapInfo { get; private set; }

		/// <summary>
		/// Cấp độ hoạt động
		/// </summary>
		public int Level
		{
			get
			{
				return this.CopyScene.Level;
			}
		}

		/// <summary>
		/// ID nhóm
		/// </summary>
		public int TeamID { get; set; }

		/// <summary>
		/// Độ khó của ải
		/// </summary>
		public int Difficulty { get; set; }

		/// <summary>
		/// Thứ tự ải
		/// </summary>
		public int StageID { get; set; }
		#endregion

		#region Core CopySceneEvent
		/// <summary>
		/// Script ải Tiêu Dao Cốc - giết quái và boss
		/// </summary>
		/// <param name="copyScene"></param>
		/// <param name="mapInfo"></param>
		public XoYo_Script_UnlockTriggerAndKillMonster(KTCopyScene copyScene, XoYo.MapInfo mapInfo) : base(copyScene)
		{
			/// Thiết lập thông tin ải
			this.MapInfo = mapInfo;
			/// Danh sách quái được sinh ra theo thời gian
			this.waitToBeSpawnedMonsters = this.MapInfo.Monsters.Where(x => x.SpawnAfter > 0).ToList();
			/// Danh sách Boss được sinh ra theo thời gian
			this.waitToBeSpawnedBosses = this.MapInfo.Bosses.Where(x => x.SpawnAfter > 0).ToList();
		}

		/// <summary>
		/// Hàm này gọi khi bắt đầu ải
		/// </summary>
		protected override void OnStart()
		{
			/// Xóa toàn bộ NPC
			this.RemoveAllNPCs();
			/// Xóa toàn bộ quái
			this.RemoveAllMonsters();
			/// Xóa toàn bộ điểm thu thập
			this.RemoveAllGrowPoints();
			/// Xóa toàn bộ cổng dịch chuyển
			this.RemoveAllDynamicAreas();
			/// Cập nhật thời điểm thông báo gần nhất
			this.LastNotifyTick = 0;
			/// Cập nhật Step của hoạt động
			this.nStep = 0;
		}

		/// <summary>
		/// Hàm này gọi liên tục trong ải
		/// </summary>
		protected override void OnTick()
		{
			/// Kick người chơi không có nhóm
			this.KickOutTeamlessPlayers();

			/// Đang trong thời gian chờ mở ải
			if (this.LifeTimeTicks <= XoYo.BeginWaitTime)
			{
				/// Nếu đã đến thời gian thông báo thông tin sự kiện
				if (KTGlobal.GetCurrentTimeMilis() - this.LastNotifyTick >= XoYo_Script_UnlockTriggerAndKillMonster.NotifyActivityInfoToPlayersEveryTick)
				{
					/// Đánh dấu thời gian thông báo thông tin sự kiện
					this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
					/// Cập nhật thông tin sự kiện
					this.UpdateEventDetailsToPlayers(this.CopyScene.Name, XoYo.BeginWaitTime - this.LifeTimeTicks, "Chuẩn bị mở ải");
				}
			}
			/// Nếu đã bắt đầu ải
			else
			{
				/// Nếu Step = 0
				if (this.nStep == 0)
				{
					/// Chuyển qua Step 1
					this.nStep = 1;
					/// Sinh quái
					this.CreateImmediateMonsters();
					/// Sinh cơ quan
					this.CreateImmediateTriggers();
					/// Nếu không có quái, và không có cơ quan
					if (this.MapInfo.Monsters.Count <= 0 && this.MapInfo.Triggers.Count <= 0)
					{
						/// Sinh Boss
						this.CreateImmediateBosses();

						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt Boss");
					}
					else if (this.MapInfo.Triggers.Count <= 0)
					{
						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
					}
					else
					{
						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Khai mở toàn bộ cơ quan");
					}
				}
				/// Nếu Step = 1
				else if (this.nStep == 1)
				{
					/// Sinh quái và Boss theo thời gian
					this.CreateObjectsInTime();
				}
				/// Nếu Step = 2
				else if (this.nStep == 2)
				{
					/// Chuyển qua Step 3
					this.nStep = 3;
					/// Đánh dấu thời điểm hoàn thành ải
					this.completedTicks = KTGlobal.GetCurrentTimeMilis();
					/// Cập nhật thông tin sự kiện
					this.UpdateEventDetailsToPlayers(this.CopyScene.Name, Math.Min(XoYo.FinishWaitTime, this.CopyScene.DurationTicks - this.LifeTimeTicks), "Hoàn thành ải!");
					/// Cập nhật thông tin sự kiện tới toàn bộ đội viên đang ở phòng chờ
					this.UpdateEventDetailsToWaitingRoomPlayers(this.CopyScene.Name, Math.Min(XoYo.FinishWaitTime, this.CopyScene.DurationTicks - this.LifeTimeTicks), "Hoàn thành ải!");
					/// Tăng số ải đã vượt qua cho toàn thể người chơi còn sống
					this.IncreaseStageIndexToAllPlayers();
				}
				/// Nếu Step = 3
				else if (this.nStep == 3)
				{
					/// Nếu đã đến thời gian rời phụ bản
					if (KTGlobal.GetCurrentTimeMilis() - this.completedTicks >= XoYo.FinishWaitTime)
					{
						/// Dừng không làm gì nữa
						this.nStep = 100;
						/// Nếu số ải đã vượt quá
						if (this.StageID >= XoYo.MaxStage)
						{
							/// Đưa người chơi về bản đồ báo danh
							this.KickOutPlayers();
						}
						else
						{
							/// Chọn người chơi bất kỳ trong nhóm
							KPlayer selectedPlayer = this.teamPlayers.Where(x => x != null && x.TeamID == this.TeamID).FirstOrDefault();
							/// Nếu tồn tại
							if (selectedPlayer != null)
							{
								/// Bắt đầu ải mới
								XoYo_EventScript.Begin(selectedPlayer, this.StageID + 1, this.Difficulty);
								/// XÓa danh sách người chơi
								this.teamPlayers.Clear();
							}
						}
						/// Hủy phụ bản
						this.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Hàm này gọi khi kết thúc ải
		/// </summary>
		protected override void OnClose()
		{
			this.waitToBeSpawnedMonsters.Clear();
			this.waitToBeSpawnedBosses.Clear();

			/// Nếu số ải đã vượt quá
			if (this.StageID >= XoYo.MaxStage)
			{
				/// Đưa người chơi về bản đồ báo danh
				this.KickOutPlayers();
			}
			else if (this.LifeTimeTicks >= 10000)
			{
				/// Chọn người chơi bất kỳ trong nhóm
				KPlayer selectedPlayer = this.teamPlayers.Where(x => x != null && x.TeamID == this.TeamID).FirstOrDefault();
				/// Bắt đầu ải mới
				XoYo_EventScript.Begin(selectedPlayer, this.StageID + 1, this.Difficulty);
			}
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi giết quái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="obj"></param>
		public override void OnKillObject(KPlayer player, GameObject obj)
		{
			base.OnKillObject(player, obj);

			lock (this.Mutex)
			{
				/// Nếu là giết quái hoặc Boss
				if (obj is Monster)
				{
					/// Đối tượng quái tương ứng
					Monster monster = obj as Monster;
					/// Nếu là quái
					if (monster.Tag != null && monster.Tag == "Monster")
					{
						/// Tổng số cơ quan còn lại
						int totalTriggers = this.GetTotalGrowPoints();
						/// Nếu vẫn còn cơ quan chưa mở
						if (totalTriggers > 0)
						{
							/// Cập nhật thông tin sự kiện
							this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Khai mở toàn bộ cơ quan");
							return;
						}

						/// Tổng số quái còn lại
						int totalMonsters = this.GetTotalMonsters(x => x.Tag != null && x.Tag == "Monster");
						/// Nếu đã hết quái
						if (totalMonsters <= 0)
						{
							/// Nếu ải không có Boss
							if (this.MapInfo.Bosses.Count <= 0)
							{
								/// Chuyển qua Step 2
								this.nStep = 2;
							}
							/// Nếu ải có Boss
							else
							{
								/// Nếu ải có quái và Boss đang chờ sinh ra theo thời gian
								if (this.waitToBeSpawnedMonsters.Count > 0)
								{
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
								}
								else if (this.waitToBeSpawnedBosses.Count > 0)
								{
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Đợi Boss xuất hiện");
								}
								/// Nếu chưa tạo Boss sau khi giết hết quái
								else if (!this.CreatedBossAfterKillingAllMonsters)
								{
									/// Đánh dấu đã tạo Boss
									this.CreatedBossAfterKillingAllMonsters = true;
									/// Tạo Boss
									this.CreateImmediateBosses();
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt Boss");
								}
							}
						}
						else
						{
							/// Cập nhật thông tin sự kiện
							this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái");
						}
					}
					/// Nếu là Boss
					else if (monster.Tag != null && monster.Tag == "Boss")
					{
						/// Tổng số quái còn lại
						int totalMonsters = this.GetTotalMonsters(x => x.Tag != null && (x.Tag == "Monster" || x.Tag == "Boss"));
						/// Nếu đã hết quái
						if (totalMonsters <= 0)
						{
							/// Nếu ải có quái và Boss đang chờ sinh ra theo thời gian
							if (this.waitToBeSpawnedMonsters.Count > 0)
							{
								/// Cập nhật thông tin sự kiện
								this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
							}
							else if (this.waitToBeSpawnedBosses.Count > 0)
							{
								/// Cập nhật thông tin sự kiện
								this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
							}
							else
							{
								/// Chuyển qua Step 2
								this.nStep = 2;
							}
						}
					}
				}
			}
			
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi chết
		/// </summary>
		/// <param name="killer"></param>
		/// <param name="player"></param>
		public override void OnPlayerDie(GameObject killer, KPlayer player)
		{
			base.OnPlayerDie(killer, player);

			/// Nếu đây là ải cuối
			if (this.StageID >= XoYo.MaxStage)
			{
				/// Cho sống lại ở bản đồ báo danh
				PlayerManager.Relive(player, XoYo.RegisterMap.ID, XoYo.RegisterMap.EnterPosX, XoYo.RegisterMap.EnterPosY, this.CopyScene.ReliveHPPercent, this.CopyScene.ReliveMPPercent, this.CopyScene.ReliveStaminaPercent);
			}
			else
			{
				/// Cho sống lại ở bản đồ chờ
				PlayerManager.Relive(player, XoYo.WaitingMap.ID, XoYo.WaitingMap.EnterPosX, XoYo.WaitingMap.EnterPosY, this.CopyScene.ReliveHPPercent, this.CopyScene.ReliveMPPercent, this.CopyScene.ReliveStaminaPercent);
			}

			/// Danh vọng tương ứng
			XoYo.ReputeInfo reputeInfo = XoYo.ReputeInfos.Where(x => x.Difficulty == this.Difficulty).FirstOrDefault();
			/// Toác gì đó
			if (reputeInfo == null)
			{
				return;
			}
			/// Tăng điểm danh vọng Tiêu Dao Cốc tương ứng
			KTGlobal.AddRepute(player, 503, reputeInfo.LossRound);
			/// Tăng điểm uy danh tương ứng
			player.Prestige += reputeInfo.LossRound;

			/// Thêm điểm tích lũy tháng
			XoYo_EventScript.AddCurrentMonthStoragePoint(player, reputeInfo.LossRound);

			/// Gửi tin nhắn thông báo
			KTGlobal.SendDefaultChat(player, string.Format("Nhận <color=yellow>{0}</color> điểm Uy danh và danh vọng Tiêu Dao Cốc.", reputeInfo.LossRound));
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi vào phụ bản
		/// </summary>
		/// <param name="player"></param>
		public override void OnPlayerEnter(KPlayer player)
		{
			base.OnPlayerEnter(player);

			/// Nếu không nằm trong nhóm tương ứng
			if (player.TeamID != this.TeamID)
			{
				this.KickOutPlayer(player);
			}

			/// Mở bảng thông báo hoạt động
			this.OpenEventBroadboard(player, XoYo.EventID);
			/// Chuyển trạng thái PK hòa bình
			player.PKMode = (int) PKMode.Peace;
			/// Cập nhật thông tin sự kiện
			this.UpdateEventDetailsToPlayers(this.CopyScene.Name, XoYo.BeginWaitTime - this.LifeTimeTicks, "Chuẩn bị mở ải");
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi rời phụ bản
		/// </summary>
		/// <param name="player"></param>
		/// <param name="toMap"></param>
		public override void OnPlayerLeave(KPlayer player, GameMap toMap)
		{
			base.OnPlayerLeave(player, toMap);

			/// Nếu không phải bị đẩy ra phòng chờ
			if (toMap.MapCode != XoYo.WaitingMap.ID)
			{
				/// Đóng bảng thông báo hoạt động
				this.CloseEventBroadboard(player, XoYo.EventID);
			}
			else
			{
				this.UpdateEventDetails(player, this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Hãy kiên nhẫn đợi ải này kết thúc!");
			}
			/// Chuyển trạng thái PK hòa bình
			player.PKMode = (int) PKMode.Peace;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Sinh tạo ra cùng lúc với phụ bản
		/// </summary>
		private void CreateImmediateMonsters()
		{
			/// Duyệt danh sách quái
			foreach (XoYo.MonsterInfo monsterInfo in this.MapInfo.Monsters)
			{
				/// Nếu là loại quái sinh ra cùng lúc với phụ bản
				if (monsterInfo.SpawnAfter == -1)
				{
					/// Ngũ hành
					KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
					/// Hướng quay
					KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
					/// Mức máu
					int hp = monsterInfo.BaseHP + monsterInfo.HPIncreaseEachLevel * this.Level;
					/// Tạo quái
					GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, monsterInfo.ID, this.CopyScene.ID, 1, monsterInfo.PosX, monsterInfo.PosY, monsterInfo.Name, monsterInfo.Title, hp, this.Level, dir, series, monsterInfo.AIType, -1, -1, monsterInfo.AIScriptID, "Monster", (monster) => {
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
					}, 65535);
				}
			}
		}

		/// <summary>
		/// Tạo Boss sinh ra khi đánh hết quái
		/// </summary>
		private void CreateImmediateBosses()
		{
			/// Duyệt danh sách Boss
			foreach (XoYo.BossInfo bossInfo in this.MapInfo.Bosses)
			{
				/// Nếu là loại Boss sinh ra sau khi tiêu diệt hết quái
				if (bossInfo.SpawnAfter == -1)
				{
					/// Ngũ hành
					KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
					/// Hướng quay
					KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
					/// Mức máu
					int hp = bossInfo.BaseHP + bossInfo.HPIncreaseEachLevel * this.Level;
					/// Tạo quái
					GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, bossInfo.ID, this.CopyScene.ID, 1, bossInfo.PosX, bossInfo.PosY, bossInfo.Name, bossInfo.Title, hp, this.Level, dir, series, bossInfo.AIType, -1, -1, bossInfo.AIScriptID, "Boss", (monster) => {
						/// Miễn dịch toàn bộ trạng thái ngũ hành
						monster.m_IgnoreAllSeriesStates = true;

						/// Nếu có kỹ năng
						if (bossInfo.Skills.Count > 0)
						{
							/// Duyệt danh sách kỹ năng
							foreach (SkillLevelRef skill in bossInfo.Skills)
							{
								/// Thêm vào danh sách kỹ năng dùng của quái
								monster.CustomAISkills.Add(skill);
							}
						}

						/// Nếu có vòng sáng
						if (bossInfo.Auras.Count > 0)
						{
							/// Duyệt danh sách vòng sáng
							foreach (SkillLevelRef aura in bossInfo.Auras)
							{
								/// Kích hoạt vòng sáng
								monster.UseSkill(aura.SkillID, aura.Level, monster);
							}
						}
					}, 65535);
				}
			}
		}

		/// <summary>
		/// Tải cơ quan sinh ra
		/// </summary>
		private void CreateImmediateTriggers()
		{
			/// Duyệt danh sách cơ quan
			foreach (XoYo.TriggerInfo  triggerInfo in this.MapInfo.Triggers)
			{
				GrowPoint growPoint = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(triggerInfo.ID, triggerInfo.Name, -1, -1, triggerInfo.CollectTick, true), triggerInfo.PosX, triggerInfo.PosY);
				growPoint.GrowPointCollectCompleted = (player) => {
					this.RemoveGrowPoint(growPoint);

					/// Danh sách hộ vệ
					List<XoYo.TriggerGuardianInfo> guardians = this.MapInfo.Guardians.Where(x => x.TriggerIndex == triggerInfo.Index).ToList();
					/// Nếu có hộ vệ
					if (guardians.Count > 0)
					{
						/// Nếu có hộ vệ
						foreach (XoYo.TriggerGuardianInfo guardianInfo in guardians)
						{
							/// Ngũ hành
							KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
							/// Hướng quay
							KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
							/// Mức máu
							int hp = guardianInfo.BaseHP + guardianInfo.HPIncreaseEachLevel * this.Level;
							/// Tạo quái
							GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, guardianInfo.ID, this.CopyScene.ID, 1, guardianInfo.PosX, guardianInfo.PosY, guardianInfo.Name, guardianInfo.Title, hp, this.Level, dir, series, guardianInfo.AIType, -1, -1, guardianInfo.AIScriptID, "Monster", (monster) => {
								/// Nếu có kỹ năng
								if (guardianInfo.Skills.Count > 0)
								{
									/// Duyệt danh sách kỹ năng
									foreach (SkillLevelRef skill in guardianInfo.Skills)
									{
										/// Thêm vào danh sách kỹ năng dùng của quái
										monster.CustomAISkills.Add(skill);
									}
								}

								/// Nếu có vòng sáng
								if (guardianInfo.Auras.Count > 0)
								{
									/// Duyệt danh sách vòng sáng
									foreach (SkillLevelRef aura in guardianInfo.Auras)
									{
										/// Kích hoạt vòng sáng
										monster.UseSkill(aura.SkillID, aura.Level, monster);
									}
								}
							}, 65535);
						}
					}
					/// Nếu không có hộ vệ
					else
					{
						/// Tổng số cơ quan còn lại
						int totalTriggers = this.GetTotalGrowPoints();
						/// Nếu vẫn còn cơ quan chưa mở
						if (totalTriggers > 0)
						{
							/// Cập nhật thông tin sự kiện
							this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Khai mở toàn bộ cơ quan");
							return;
						}

						/// Tổng số quái còn lại
						int totalMonsters = this.GetTotalMonsters(x => x.Tag != null && x.Tag == "Monster");
						/// Nếu đã hết quái
						if (totalMonsters <= 0)
						{
							/// Nếu ải không có Boss
							if (this.MapInfo.Bosses.Count <= 0)
							{
								/// Chuyển qua Step 2
								this.nStep = 2;
							}
							/// Nếu ải có Boss
							else
							{
								/// Nếu ải có quái và Boss đang chờ sinh ra theo thời gian
								if (this.waitToBeSpawnedMonsters.Count > 0)
								{
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
								}
								else if (this.waitToBeSpawnedBosses.Count > 0)
								{
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Đợi Boss xuất hiện");
								}
								else
								{
									/// Tạo Boss
									this.CreateImmediateBosses();
									/// Cập nhật thông tin sự kiện
									this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt Boss");
								}
							}
						}
						else
						{
							/// Cập nhật thông tin sự kiện
							this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái");
						}
					}
				};
			}
		}

		/// <summary>
		/// Sinh các đối tượng theo thời gian
		/// </summary>
		private void CreateObjectsInTime()
		{
			/// Nếu tồn tại danh sách chờ thêm
			if (this.waitToBeSpawnedMonsters.Count > 0)
			{
				/// Danh sách quái cần xóa
				List<XoYo.MonsterInfo> toRemoveWaitingMonsters = null;
				/// Duyệt danh sách quái
				foreach (XoYo.MonsterInfo monsterInfo in this.waitToBeSpawnedMonsters)
				{
					/// Nếu đã đến thời gian sinh
					if (this.LifeTimeTicks >= monsterInfo.SpawnAfter + XoYo.BeginWaitTime)
					{
						/// Thêm vào danh sách cần xóa
						if (toRemoveWaitingMonsters == null)
						{
							toRemoveWaitingMonsters = new List<XoYo.MonsterInfo>();
						}
						toRemoveWaitingMonsters.Add(monsterInfo);

						/// Ngũ hành
						KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
						/// Hướng quay
						KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
						/// Mức máu
						int hp = monsterInfo.BaseHP + monsterInfo.HPIncreaseEachLevel * this.Level;
						/// Tạo quái
						GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, monsterInfo.ID, this.CopyScene.ID, 1, monsterInfo.PosX, monsterInfo.PosY, monsterInfo.Name, monsterInfo.Title, hp, this.Level, dir, series, monsterInfo.AIType, -1, -1, monsterInfo.AIScriptID, "Monster", (monster) => {
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
						}, 65535);
					}
				}

				/// Nếu tồn tại danh sách cần xóa
				if (toRemoveWaitingMonsters != null)
				{
					/// Duyệt danh sách cần xóa
					foreach (XoYo.MonsterInfo monsterInfo in toRemoveWaitingMonsters)
					{
						/// Xóa khỏi danh sách
						this.waitToBeSpawnedMonsters.Remove(monsterInfo);
					}

					/// Tổng số cơ quan còn lại
					int totalTriggers = this.GetTotalGrowPoints();
					/// Nếu vẫn còn cơ quan chưa mở
					if (totalTriggers > 0)
					{
						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Khai mở toàn bộ cơ quan");
						return;
					}
					else
					{
						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt toàn bộ quái vật");
					}

					/// Làm rỗng danh sách chờ xóa
					toRemoveWaitingMonsters.Clear();
				}
			}

			/// Nếu tồn tại danh sách chờ thêm
			if (this.waitToBeSpawnedBosses.Count > 0)
			{
				/// Danh sách quái cần xóa
				List<XoYo.BossInfo> toRemoveWaitingBosses = null;
				/// Duyệt danh sách quái
				foreach (XoYo.BossInfo bossInfo in this.waitToBeSpawnedBosses)
				{
					/// Nếu đã đến thời gian sinh
					if (this.LifeTimeTicks >= bossInfo.SpawnAfter + XoYo.BeginWaitTime)
					{
						/// Thêm vào danh sách cần xóa
						if (toRemoveWaitingBosses == null)
						{
							toRemoveWaitingBosses = new List<XoYo.BossInfo>();
						}
						toRemoveWaitingBosses.Add(bossInfo);

						/// Ngũ hành
						KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
						/// Hướng quay
						KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
						/// Mức máu
						int hp = bossInfo.BaseHP + bossInfo.HPIncreaseEachLevel * this.Level;
						/// Tạo quái
						GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, bossInfo.ID, this.CopyScene.ID, 1, bossInfo.PosX, bossInfo.PosY, bossInfo.Name, bossInfo.Title, hp, this.Level, dir, series, bossInfo.AIType, -1, -1, bossInfo.AIScriptID, "Boss", (monster) => {
							/// Miễn dịch toàn bộ trạng thái ngũ hành
							monster.m_IgnoreAllSeriesStates = true;

							/// Nếu có kỹ năng
							if (bossInfo.Skills.Count > 0)
							{
								/// Duyệt danh sách kỹ năng
								foreach (SkillLevelRef skill in bossInfo.Skills)
								{
									/// Thêm vào danh sách kỹ năng dùng của quái
									monster.CustomAISkills.Add(skill);
								}
							}

							/// Nếu có vòng sáng
							if (bossInfo.Auras.Count > 0)
							{
								/// Duyệt danh sách vòng sáng
								foreach (SkillLevelRef aura in bossInfo.Auras)
								{
									/// Kích hoạt vòng sáng
									monster.UseSkill(aura.SkillID, aura.Level, monster);
								}
							}
						}, 65535);
					}
				}

				/// Nếu tồn tại danh sách cần xóa
				if (toRemoveWaitingBosses != null)
				{
					/// Duyệt danh sách cần xóa
					foreach (XoYo.BossInfo bossInfo in toRemoveWaitingBosses)
					{
						/// Xóa khỏi danh sách
						this.waitToBeSpawnedBosses.Remove(bossInfo);
					}

					/// Đánh dấu thời gian thông báo thông tin sự kiện
					this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
					/// Cập nhật thông tin sự kiện
					this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Tiêu diệt Boss");

					/// Làm rỗng danh sách chờ xóa
					toRemoveWaitingBosses.Clear();
				}
			}
		}

		/// <summary>
		/// Đưa người chơi trở lại bản đồ báo danh
		/// </summary>
		/// <param name="player"></param>
		private void KickOutPlayer(KPlayer player)
		{
			GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, XoYo.RegisterMap.ID, XoYo.RegisterMap.EnterPosX, XoYo.RegisterMap.EnterPosY, (int) player.CurrentDir);
		}

		/// <summary>
		/// Đưa tất cả người chơi trở lại bản đồ báo danh
		/// </summary>
		private void KickOutPlayers()
		{
			foreach (KPlayer player in this.GetPlayers())
			{
				this.KickOutPlayer(player);
			}
		}

		/// <summary>
		/// Kick toàn bộ người chơi không có nhóm ra khỏi phụ bản
		/// </summary>
		private void KickOutTeamlessPlayers()
		{
			foreach (KPlayer player in this.GetPlayers())
			{
				if (player.TeamID != this.TeamID)
				{
					this.KickOutPlayer(player);
					/// Xóa khỏi danh sách
					this.teamPlayers.Remove(player);
				}
			}
		}

		/// <summary>
		/// Trả về danh sách thành viên trong nhóm đang ở phòng chờ
		/// </summary>
		/// <returns></returns>
		private List<KPlayer> GetWaitingRoomTeammates()
		{
			/// Danh sách người chơi
			List<KPlayer> players = new List<KPlayer>();
			/// Duyệt danh sách thành viên nhóm
			foreach (KPlayer player in this.teamPlayers)
			{
				/// Nếu là đội viên và đang ở phòng chờ
				if (player.TeamID == this.TeamID && player.CurrentMapCode == XoYo.WaitingMap.ID)
				{
					players.Add(player);
				}
			}
			/// Không tìm thấy trả ra danh sách rỗng
			return players;
		}

		/// <summary>
		/// Cập nhật thông tin ải cho người chơi ở phòng chờ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="eventTimeMilis"></param>
		/// <param name="contents"></param>
		private void UpdateEventDetailsToWaitingRoomPlayers(string name, long eventTimeMilis, params string[] contents)
		{
			/// Duyệt danh sách người chơi ở phòng chờ
			foreach (KPlayer player in this.GetWaitingRoomTeammates())
			{
				this.UpdateEventDetails(player, name, eventTimeMilis, contents);
			}
		}

		/// <summary>
		/// Tăng thứ tự ải hiện tại lên với tất cả người chơi
		/// </summary>
		private void IncreaseStageIndexToAllPlayers()
		{
			foreach (KPlayer player in this.GetPlayers())
			{
				/// Số ải đã vượt qua hôm nay
				int todayPassedStages = player.GetValueOfDailyRecore((int) AcitvityRecore.XoYo_StagesPassedToday);
				/// Nếu chưa có thì khởi tạo
				if (todayPassedStages == -1)
				{
					todayPassedStages = 0;
				}
				/// Tăng số ải lên
				todayPassedStages++;
				/// Thiết lập số ải đã vượt qua hôm nay
				player.SetValueOfDailyRecore((int) AcitvityRecore.XoYo_StagesPassedToday, todayPassedStages);

				/// Danh vọng tương ứng
				XoYo.ReputeInfo reputeInfo = XoYo.ReputeInfos.Where(x => x.Difficulty == this.Difficulty).FirstOrDefault();
				/// Toác gì đó
				if (reputeInfo == null)
				{
					continue;
				}

				/// Số điểm danh vọng có thêm
				int reputeAdded = 0;
				switch (todayPassedStages)
				{
					case 1:
					{
						reputeAdded = reputeInfo.OneRound;
						break;
					}
					case 2:
					{
						reputeAdded = reputeInfo.TwoRound;
						break;
					}
					case 3:
					{
						reputeAdded = reputeInfo.ThreeRound;
						break;
					}
					case 4:
					{
						reputeAdded = reputeInfo.FourRound;
						break;
					}
					case 5:
					{
						reputeAdded = reputeInfo.FiveRound;
						break;
					}
				}
				/// Tăng điểm danh vọng Tiêu Dao Cốc tương ứng
				KTGlobal.AddRepute(player, 503, reputeAdded);
				/// Tăng điểm uy danh tương ứng
				player.Prestige += reputeAdded;

				/// Thêm điểm tích lũy tháng
				XoYo_EventScript.AddCurrentMonthStoragePoint(player, reputeAdded);

				/// Gửi tin nhắn thông báo
				KTGlobal.SendDefaultChat(player, string.Format("Nhận <color=yellow>{0}</color> điểm Uy danh và danh vọng Tiêu Dao Cốc.", reputeAdded));
			}
		}
		#endregion
	}
}
