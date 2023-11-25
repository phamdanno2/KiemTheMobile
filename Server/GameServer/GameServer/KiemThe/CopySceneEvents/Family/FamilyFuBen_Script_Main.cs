using GameServer.KiemThe.CopySceneEvents.Model;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using Server.Tools;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.Family
{
	/// <summary>
	/// Script chính điều khiển phụ bản Vượt Ải Gia Tộc
	/// </summary>
	public class FamilyFuBen_Script_Main : CopySceneEvent
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
		/// Bước của luật chơi theo trình tự
		/// </summary>
		private int currentGameStep = 0;

		/// <summary>
		/// Bước hiện tại của trò chơi đã hoàn tất chưa
		/// </summary>
		private bool currentGameStepCompleted = true;

		/// <summary>
		/// Đã hoàn thành vượt ải chưa
		/// </summary>
		private bool isCompleted = false;

		/// <summary>
		/// Tổng số Boss được triệu hồi nhờ Câu Hồn Ngọc
		/// </summary>
		private int totalBossesSummoned = 0;
		#endregion

		#region Core CopySceneEvent
		/// <summary>
		/// Script chính điều khiển phụ bản Vượt Ải Gia Tộc
		/// </summary>
		/// <param name="copyScene"></param>
		public FamilyFuBen_Script_Main(KTCopyScene copyScene) : base(copyScene)
		{
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
			/// Đang trong thời gian chờ mở ải
			if (this.LifeTimeTicks <= FamilyFuBen.Config.PrepareTime)
			{
				/// Nếu đã đến thời gian thông báo thông tin sự kiện
				if (KTGlobal.GetCurrentTimeMilis() - this.LastNotifyTick >= FamilyFuBen_Script_Main.NotifyActivityInfoToPlayersEveryTick)
				{
					/// Đánh dấu thời gian thông báo thông tin sự kiện
					this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
					/// Cập nhật thông tin sự kiện
					this.UpdateEventDetailsToPlayers(this.CopyScene.Name, FamilyFuBen.Config.PrepareTime - this.LifeTimeTicks, "Chuẩn bị mở ải");
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
				}
				/// Nếu Step = 1
				else if (this.nStep == 1)
				{
					/// Nếu đã đến thời gian thông báo thông tin sự kiện
					if (KTGlobal.GetCurrentTimeMilis() - this.LastNotifyTick >= FamilyFuBen_Script_Main.NotifyActivityInfoToPlayersEveryTick)
					{
						/// Đánh dấu thời gian thông báo thông tin sự kiện
						this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
						/// Cập nhật thông tin sự kiện
						this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Khai mở toàn bộ cơ quan và tiêu diệt hộ vệ");
					}

					lock (this.Mutex)
                    {
						/// Nếu chưa hoàn tất bước hiện tại của trò chơi
						if (!this.currentGameStepCompleted)
						{
							return;
						}

						/// Thực thi Logic khi hoàn thành bước hiện tại
						this.CompleteCurrentStep();

						/// Chuyển qua bước tiếp theo
						this.currentGameStep++;
						/// Đánh dấu chưa hoàn thành bước hiện tại
						this.currentGameStepCompleted = false;

						/// Bắt đầu trò chơi ở bước tương ứng
						bool ret = this.BeginNewStep();

						/// Nếu không thể bắt đầu tức đã qua hết toàn bộ các bước
						if (!ret)
						{
							/// Chuyển qua Step = 2
							this.nStep = 2;

							/// Đánh dấu thời gian thông báo thông tin sự kiện
							this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
							/// Cập nhật thông tin sự kiện
							this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, "Hoàn thành vượt ải");
						}
					}
				}
				/// Nếu Step = 2
				else if (this.nStep == 2)
				{
					/// Đánh dấu đã hoàn thành vượt ải
					this.isCompleted = true;
					/// Chuyển qua Step = 100
					this.nStep = 100;
				}
			}
		}

		/// <summary>
		/// Hàm này gọi khi kết thúc ải
		/// </summary>
		protected override void OnClose()
		{
			/// Đưa người chơi về bản đồ báo danh
			this.KickOutPlayers();
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
				/// Nếu vẫn còn cơ quan chưa mở
				if (this.GetGrowPoints().Any(x => x.Data.ResID != 20016))
				{
					//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:OnKillObject step {0} => growpoint still exist. RoleID = {1}, RoleName = {2}, Family = {3}", this.currentGameStep, player?.RoleID, player?.RoleName, player?.FamilyName));
					/// Bỏ qua
					return;
				}

				/// Tổng số quái
				int monsterCount = this.GetTotalMonsters();
				/// Nếu vẫn còn quái
				if (monsterCount > 0)
				{
					//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:OnKillObject step {0} => monster left: {1}. RoleID = {2}, RoleName = {3}, Family = {4}", this.currentGameStep, monsterCount, player?.RoleID, player?.RoleName, player?.FamilyName));
					/// Bỏ qua
					return;
				}

				/// Đánh dấu đã hoàn thành bước hiện tại
				this.currentGameStepCompleted = true;
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
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi vào phụ bản
		/// </summary>
		/// <param name="player"></param>
		public override void OnPlayerEnter(KPlayer player)
		{
			base.OnPlayerEnter(player);
			/// Mở bảng thông báo hoạt động
			this.OpenEventBroadboard(player, FamilyFuBen.Config.EventID);
			/// Chuyển trạng thái PK hòa bình
			player.PKMode = (int) PKMode.Peace;
			/// Cập nhật thông tin sự kiện
			this.UpdateEventDetailsToPlayers(this.CopyScene.Name, FamilyFuBen.Config.PrepareTime - this.LifeTimeTicks, "Chuẩn bị mở ải");
		}

		/// <summary>
		/// Hàm này gọi đến khi người chơi rời phụ bản
		/// </summary>
		/// <param name="player"></param>
		/// <param name="toMap"></param>
		public override void OnPlayerLeave(KPlayer player, GameMap toMap)
		{
			base.OnPlayerLeave(player, toMap);
			/// Chuyển trạng thái PK hòa bình
			player.PKMode = (int) PKMode.Peace;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Đưa người chơi trở lại bản đồ báo danh
		/// </summary>
		/// <param name="player"></param>
		private void KickOutPlayer(KPlayer player)
		{
			GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, this.CopyScene.OutMapCode, this.CopyScene.OutPosX, this.CopyScene.OutPosY, (int) player.CurrentDir);
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
		/// Thực hiện Logic cổng dịch chuyển
		/// </summary>
		/// <param name="teleportInfo"></param>
		/// <param name="player"></param>
		private void DoTeleportLogic(FamilyFuBen.GameRule.TeleportInfo teleportInfo, KPlayer player)
		{
			/// Nếu là dịch đến cùng vị trí trong bản đồ
			if (teleportInfo.ToMapID == this.CopyScene.MapCode || teleportInfo.ToMapID == -1)
			{
				/// Nếu là mặc định dịch ra ngoài
				if (teleportInfo.ToPosX == -1 && teleportInfo.ToPosY == -1)
				{
					/// Trở về vị trí báo danh
					this.KickOutPlayer(player);
				}
				else
				{
					/// Thực hiện chuyển vị trí
					GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, teleportInfo.ToPosX, teleportInfo.ToPosY, (int) player.CurrentDir);
				}
			}
			/// Nếu là dịch đến bản đồ khác
			else
			{
				/// Thực hiện dịch đến bản đồ tương ứng
				GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, teleportInfo.ToMapID, teleportInfo.ToPosX, teleportInfo.ToPosY, (int) player.CurrentDir);
			}
		}

		/// <summary>
		/// Tạo các cổng dịch chuyển khởi tạo cùng bước trò chơi
		/// </summary>
		/// <param name="step"></param>
		private void CreateImmediateTeleports(FamilyFuBen.GameRule.StepInfo step)
		{
			/// Duyệt danh sách cổng dịch chuyển
			foreach (FamilyFuBen.GameRule.TeleportInfo teleportInfo in step.Teleports)
			{
				/// Nếu là cổng dịch chuyển khởi tạo cùng bước trò chơi
				if (teleportInfo.SpawnImmediate)
				{
					/// Tạo cổng dịch chuyển
					KDynamicArea teleport = KTDynamicAreaManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, teleportInfo.Name, 5340, teleportInfo.PosX, teleportInfo.PosY, this.DurationTicks - this.LifeTimeTicks, 2f, 100, -1, null);
					teleport.OnEnter = (obj) => {
						if (obj is KPlayer player)
						{
							this.DoTeleportLogic(teleportInfo, player);
						}
					};
				}
			}
		}

		/// <summary>
		/// Tạo các cổng dịch chuyển khởi tạo khi hoàn thành bước trò chơi
		/// </summary>
		/// <param name="step"></param>
		private void CreateCompleteStageTeleports(FamilyFuBen.GameRule.StepInfo step)
		{
			/// Duyệt danh sách cổng dịch chuyển
			foreach (FamilyFuBen.GameRule.TeleportInfo teleportInfo in step.Teleports)
			{
				/// Nếu là cổng dịch chuyển khởi tạo cùng bước trò chơi
				if (!teleportInfo.SpawnImmediate)
				{
					/// Tạo cổng dịch chuyển
					KDynamicArea teleport = KTDynamicAreaManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, teleportInfo.Name, 5340, teleportInfo.PosX, teleportInfo.PosY, this.DurationTicks - this.LifeTimeTicks, 2f, 100, -1, null);
					teleport.OnEnter = (obj) => {
						if (obj is KPlayer player)
						{
							this.DoTeleportLogic(teleportInfo, player);
						}
					};
				}
			}
		}

		/// <summary>
		/// Tải cơ quan sinh ra
		/// </summary>
		/// <param name="step"></param>
		private void CreateImmediateTriggers(FamilyFuBen.GameRule.StepInfo step)
		{
			/// Duyệt danh sách cơ quan
			foreach (FamilyFuBen.GameRule.TriggerInfo triggerInfo in step.Triggers)
			{
				GrowPoint growPoint = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(triggerInfo.ID, triggerInfo.Name, -1, -1, triggerInfo.CollectTick, true), triggerInfo.PosX, triggerInfo.PosY);
				growPoint.GrowPointCollectCompleted = (player) => {
					/// Thông báo người chơi
					this.NotifyAllPlayers("Hộ vệ cơ quan đã xuất hiện, mau mau tiêu diệt toàn bộ!");

					/// Xóa cơ quan
					this.RemoveGrowPoint(growPoint);

					/// Danh sách hộ vệ
					List<FamilyFuBen.GameRule.TriggerGuardianInfo> guardians = step.Guardians.Where(x => x.TriggerIndex == triggerInfo.Index).ToList();
					/// Tổng số người chơi trong phụ bản
					int totalPlayers = this.GetTotalPlayers();
					/// Duyệt danh sách hộ vệ
					foreach (FamilyFuBen.GameRule.TriggerGuardianInfo guardianInfo in guardians)
					{
						/// Ngũ hành
						KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
						/// Hướng quay
						KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
						/// Mức máu
						int hp = guardianInfo.BaseHP + guardianInfo.HPIncreaseEachLevel * totalPlayers;
						/// Tạo quái
						GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, guardianInfo.ID, this.CopyScene.ID, 1, guardianInfo.PosX, guardianInfo.PosY, guardianInfo.Name, guardianInfo.Title, hp, 80, dir, series, guardianInfo.AIType, -1, -1, guardianInfo.AIScriptID, "Monster", (monster) => {
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

					//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:DoingStep step {0} => create guardians ok, total = {1}. RoleID = {2}, RoleName = {3}, Family = {4}", this.currentGameStep, guardians.Count, player?.RoleID, player?.RoleName, player?.FamilyName));
				};
			}

			/// Thông báo người chơi
			this.NotifyAllPlayers("Cơ quan đã xuất hiện, hãy mau mau khai mở!");
			/// Người chơi bất kỳ
			KPlayer _player = this.GetPlayers().FirstOrDefault();
			//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:DoingStep step {0} => create trigger ok, total = {1}. RoleID = {2}, RoleName = {3}, Family = {4}", this.currentGameStep, step.Triggers.Count, _player?.RoleID, _player?.RoleName, _player?.FamilyName));
		}

		/// <summary>
		/// Tạo Boss tương ứng
		/// </summary>
		/// <param name="step"></param>
		private void CreateImmediateBosses(FamilyFuBen.GameRule.StepInfo step)
		{
			/// Tổng số người chơi trong phụ bản
			int totalPlayers = this.GetTotalPlayers();
			/// Duyệt danh sách Boss
			foreach (FamilyFuBen.GameRule.BossInfo bossInfo in step.Bosses)
			{
				/// Ngũ hành
				KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
				/// Hướng quay
				KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
				/// Mức máu
				int hp = bossInfo.BaseHP + bossInfo.HPIncreaseEachLevel * totalPlayers;
				/// Tạo Boss
				GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, bossInfo.ID, this.CopyScene.ID, 1, bossInfo.PosX, bossInfo.PosY, bossInfo.Name, bossInfo.Title, hp, 80, dir, series, bossInfo.AIType, -1, -1, bossInfo.AIScriptID, "Boss", (monster) => {
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

			/// Thông báo người chơi
			this.NotifyAllPlayers("Boss đã xuất hiện, hãy mau mau tiêu diệt!");
			/// Người chơi bất kỳ
			KPlayer player = this.GetPlayers().FirstOrDefault();
			//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:DoingStep step {0} => create boss ok, total = {1}. RoleID = {2}, RoleName = {3}, Family = {4}", this.currentGameStep, step.Bosses.Count, player?.RoleID, player?.RoleName, player?.FamilyName));
		}

		/// <summary>
		/// Thực hiện Logic của bước hiện tại
		/// </summary>
		private void CompleteCurrentStep()
		{
			/// Người chơi bất kỳ
			KPlayer player = this.GetPlayers().FirstOrDefault();
			/// Nếu là bước 0 tức chưa bắt đầu
			if (this.currentGameStep == 0)
            {
				//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen => Begin new CopyScene. RoleID = {0}, RoleName = {1}, Family = {2}", player?.RoleID, player?.RoleName, player?.FamilyName));
				return;
            }
			/// Bước hiện tại
			FamilyFuBen.GameRule.StepInfo stepInfo = FamilyFuBen.Rule.Steps.Where(x => x.ID == this.currentGameStep).FirstOrDefault();
			/// Nếu không tìm thấy
			if (stepInfo == null)
			{
				//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:CompleteCurrentStep step {0} => stepInfo = null. RoleID = {1}, RoleName = {2}, Family = {3}", this.currentGameStep, player?.RoleID, player?.RoleName, player?.FamilyName));
				/// Bỏ qua
				return;
			}

			/// Tạo cổng Teleport sinh ra khi hoàn thành
			this.CreateCompleteStageTeleports(stepInfo);

			//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:CompleteCurrentStep step {0} => OK. RoleID = {1}, RoleName = {2}, Family = {3}", this.currentGameStep, player?.RoleID, player?.RoleName, player?.FamilyName));
		}

		/// <summary>
		/// Bắt đầu trò chơi ở bước tương ứng
		/// </summary>
		/// <param name="step"></param>
		private bool BeginNewStep()
		{
			/// Bước hiện tại
			FamilyFuBen.GameRule.StepInfo stepInfo = FamilyFuBen.Rule.Steps.Where(x => x.ID == this.currentGameStep).FirstOrDefault();
			/// Người chơi bất kỳ
			KPlayer player = this.GetPlayers().FirstOrDefault();
			/// Nếu không tìm thấy
			if (stepInfo == null)
			{
				//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:BeginNewStep step {0} => stepInfo = null. RoleID = {1}, RoleName = {2}, Family = {3}", this.currentGameStep, player?.RoleID, player?.RoleName, player?.FamilyName));
				/// Trả về kết quả không thể bắt đầu bước mới
				return false;
			}

			/// Tạo cổng Teleport mặc định cùng bước tương ứng
			this.CreateImmediateTeleports(stepInfo);

			/// Tạo cơ quan
			this.CreateImmediateTriggers(stepInfo);

			/// Tạo Boss
			this.CreateImmediateBosses(stepInfo);

			/// Nếu không có gì
			if (stepInfo.Triggers.Count <= 0 && stepInfo.Bosses.Count <= 0)
			{
				/// Đánh dấu hoàn thành bước hiện tại
				this.currentGameStepCompleted = true;
			}

			/// Thông báo người chơi
			this.NotifyAllPlayers("Cửa đã mở, hãy nhanh chân!");

			//LogManager.WriteLog(LogTypes.CopyScene, string.Format("FamilyFuBen:BeginNewStep step {0} => OK. RoleID = {1}, RoleName = {2}, Family = {3}", this.currentGameStep, player?.RoleID, player?.RoleName, player?.FamilyName));

			/// Trả về kết quả bắt đầu thành công
			return true;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Kiểm tra điều kiện dùng Câu Hồn Ngọc để tạo Boss
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public string UseCallBossItem_CheckCondition(KPlayer player)
		{
			/// Nếu chưa hoàn thành vượt ải
			if (!this.isCompleted)
			{
				return "Chưa hoàn thành Vượt ải, không thể sử dụng Câu Hồn Ngọc!";
			}
			/// Nếu đã triệu hồi quá số lượng
			else if (this.totalBossesSummoned >= FamilyFuBen.Config.MaxCallBoss)
			{
				return string.Format("Chỉ có thể triệu hồi tối đa {0} Boss Võ lâm cao thủ!", FamilyFuBen.Config.MaxCallBoss);
			}

			/// Trả về kết quả triệu hồi thành công
			return "OK";
		}

		/// <summary>
		/// Sử dụng Câu Hồn Ngọc tạo Boss
		/// </summary>
		/// <param name="player"></param>
		/// <param name="bossID"></param>
		public void UseCallBossItem(KPlayer player, int bossID)
		{
			/// Nếu chưa hoàn thành vượt ải
			if (!this.isCompleted)
			{
				PlayerManager.ShowNotification(player, "Chưa hoàn thành Vượt ải, không thể sử dụng Câu Hồn Ngọc!");
				return;
			}
			/// Nếu đã triệu hồi quá số lượng
			else if (this.totalBossesSummoned >= FamilyFuBen.Config.MaxCallBoss)
			{
				PlayerManager.ShowNotification(player, string.Format("Chỉ có thể triệu hồi tối đa {0} Boss Võ lâm cao thủ!", FamilyFuBen.Config.MaxCallBoss));
				return;
			}

			/// Tăng số lượng Boss đã triệu hồi
			this.totalBossesSummoned++;

			/// Tạo Boss tương ứng
			GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, bossID, this.CopyScene.ID, 1, player.PosX, player.PosY, "", "", -1, -1, Entities.Direction.NONE, KE_SERIES_TYPE.series_none, MonsterAIType.Boss, -1, -1, -1, "SpecialBoss", null, 65535);

			/// Thông báo triệu hồi thành công
			PlayerManager.ShowNotification(player, "Triệu hồi thành công!");
		}
		#endregion
	}
}
