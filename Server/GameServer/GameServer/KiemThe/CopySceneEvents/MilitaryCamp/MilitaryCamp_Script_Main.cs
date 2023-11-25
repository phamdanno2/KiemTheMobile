using GameServer.KiemThe.CopySceneEvents.Model;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Script chính điều khiển phụ bản Quân doanh
    /// </summary>
    public partial class MilitaryCamp_Script_Main : CopySceneEvent
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
        /// Loại phụ bản
        /// </summary>
        private readonly MilitaryCamp.EventInfo EventData;

        /// <summary>
        /// Thứ tự tầng hiện tại
        /// </summary>
        private int currentStageID = 0;

        /// <summary>
        /// Danh sách nhiệm vụ đã hoàn thành
        /// </summary>
        private readonly HashSet<int> completedTasks = new HashSet<int>();

        /// <summary>
        /// Danh sách nhiệm vụ đang làm
        /// </summary>
        private readonly HashSet<int> doingTasks = new HashSet<int>();

        /// <summary>
        /// Danh sách cơ quan cản đường đã được mở
        /// </summary>
        private readonly HashSet<int> openedObstacles = new HashSet<int>();

        /// <summary>
        /// Thời điểm hoàn thành phụ bản
        /// </summary>
        private long eventCompletedTicks = -1;
        #endregion

        #region Core CopySceneEvent
        /// <summary>
        /// Script chính điều khiển phụ bản Quân doanh
        /// </summary>
        /// <param name="copyScene"></param>
        /// <param name="data"></param>
        public MilitaryCamp_Script_Main(KTCopyScene copyScene, MilitaryCamp.EventInfo data) : base(copyScene)
        {
            /// Đánh dấu phụ bản
            this.EventData = data;
        }

        /// <summary>
        /// Hàm này gọi khi bắt đầu phụ bản
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
            /// Tạo các NPC tĩnh
            this.CreateStaticNPCs();
            /// Bắt đầu từ tầng 1
            this.currentStageID = 0;
            /// Bắt đầu
            this.BeginStage();
        }

        /// <summary>
        /// Hàm này gọi liên tục trong phụ bản
        /// </summary>
        protected override void OnTick()
        {
            /// Nếu đã đến thời gian thông báo thông tin sự kiện
            if (KTGlobal.GetCurrentTimeMilis() - this.LastNotifyTick >= MilitaryCamp_Script_Main.NotifyActivityInfoToPlayersEveryTick)
            {
                /// Đánh dấu thời gian thông báo thông tin sự kiện
                this.LastNotifyTick = KTGlobal.GetCurrentTimeMilis();
                /// Cập nhật thông tin sự kiện
                this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, this.GetEventBroadboardDetailText());
            }

            /// Nếu đã hoàn thành phụ bản
            if (this.eventCompletedTicks != -1)
            {
                /// Nếu đã hết thời gian đếm lùi
                if (KTGlobal.GetCurrentTimeMilis() - this.eventCompletedTicks >= 30000)
                {
                    /// Đưa tất cả người chơi ra khỏi bản đồ
                    this.KickOutPlayers();
                    /// Hủy phụ bản
                    this.Dispose();
                    /// Bỏ qua
                    return;
                }
            }

            /// Nếu tầng hiện tại đã hoàn thành
            if (this.IsCurrentStageCompleted())
            {
                /// Thực thi sự kiện hoàn tất Stage
                this.CompleteStage();
                /// Chuyển sang tầng tiếp theo
                this.currentStageID++;
                /// Nếu đã hoàn thành phụ bản
                if (this.currentStageID >= this.EventData.Stages.Count)
                {
                    /// Hoàn thành phụ bản
                    this.CompleteEvent();
                    /// Bỏ qua
                    return;
                }
            }
            /// Nếu tâng hiện tại chưa hoàn thành
            else
            {
                /// Theo dõi nhiệm vụ
                this.HandleTasks();
            }
        }

        /// <summary>
        /// Hàm này gọi khi kết thúc phụ bản
        /// </summary>
        protected override void OnClose()
        {
            this.completedTasks.Clear();
            this.doingTasks.Clear();
            this.openedObstacles.Clear();
        }

		/// <summary>
		/// Hàm này gọi đến khi người chơi giết quái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="obj"></param>
		public override void OnKillObject(KPlayer player, GameObject obj)
		{
			base.OnKillObject(player, obj);
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
			this.OpenEventBroadboard(player, MilitaryCamp.EventID);
			/// Chuyển trạng thái PK hòa bình
			player.PKMode = (int) PKMode.Peace;
			/// Cập nhật thông tin sự kiện
			this.UpdateEventDetailsToPlayers(this.CopyScene.Name, this.CopyScene.DurationTicks - this.LifeTimeTicks, this.GetEventBroadboardDetailText());
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
        /// Trả về chuỗi theo dõi tiến độ phụ bản
        /// </summary>
        /// <returns></returns>
        private string GetEventBroadboardDetailText()
        {
            return "ABCXYZ";
        }

        /// <summary>
        /// Tạo các NPC tĩnh
        /// </summary>
        private void CreateStaticNPCs()
        {
            /// Duyệt danh sách NPC tĩnh
            foreach (MilitaryCamp.EventInfo.NPCInfo npcInfo in this.EventData.NPCs)
            {
                /// Tạo NPC tương ứng
                this.CreateStaticNPC(npcInfo);
            }
        }

        /// <summary>
        /// Tạo cơ quan thuộc Stage tương ứng
        /// </summary>
        /// <param name="stageInfo"></param>
        private void CreateStageTriggers(MilitaryCamp.EventInfo.StageInfo stageInfo)
        {
            /// Duyệt danh sách cơ quan mở đường
            foreach (MilitaryCamp.EventInfo.StageInfo.TriggerData.KeyTriggerInfo triggerInfo in stageInfo.Triggers.KeyTriggers.Values)
            {
                /// Tạo cơ quan
                GrowPoint trigger = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(triggerInfo.ID, triggerInfo.Name, -1, -1, triggerInfo.CollectTick, true), triggerInfo.PosX, triggerInfo.PosY);
                trigger.GrowPointCollectCompleted = (player) => {
                    /// Xóa cơ quan
                    this.RemoveGrowPoint(trigger);
                    /// Mở cơ quan cản đường tương ứng
                    this.openedObstacles.Add(triggerInfo.ObstacleTriggerID);
                };
            }

            /// Duyệt danh sách cơ quan cản đường
            foreach (MilitaryCamp.EventInfo.StageInfo.TriggerData.ObstacleTriggerInfo triggerInfo in stageInfo.Triggers.ObstacleTriggers.Values)
            {
                /// Tạo cơ quan
                NPCGeneralManager.AddNewNPC(this.CopyScene.MapCode, this.CopyScene.ID, triggerInfo.ID, triggerInfo.PosX, triggerInfo.PosY, triggerInfo.Name, "", Entities.Direction.DOWN_RIGHT, -1, "", (trigger) => {
                    /// Sự kiện Click
                    trigger.Click = (player) => {
                        /// Tạo NPC Dialog
                        KNPCDialog dialog = new KNPCDialog();
                        dialog.Owner = player;
                        dialog.Text = "Đường đi phía trước bị cản đường bởi cơ quan. Sau khi khai mở mới có thể đi qua!";
                        dialog.Selections = new Dictionary<int, string>()
                        {
                            { -1, "Cho ta qua" },
                            { -1000, "Kết thúc đối thoại" },
                        };
                        dialog.OnSelect = (x) => {
                            /// Giao vật phẩm
                            if (x.SelectID == -1)
                            {
                                /// Đóng NPCDialog
                                KT_TCPHandler.CloseDialog(player);

                                /// Nếu chưa khai mở
                                if (!this.openedObstacles.Contains(triggerInfo.TriggerID))
                                {
                                    /// Thông báo chưa khai mở
                                    PlayerManager.ShowNotification(player, "Cơ quan chưa được khai mở, không thể đi qua!");
                                    /// Bỏ qua
                                    return;
                                }
                                /// Nếu đã khai mở
                                else
                                {
                                    /// Dịch chuyển vào trong
                                    GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, triggerInfo.ToPosX, triggerInfo.ToPosY, (int) player.CurrentDir);
                                }
                            }
                            /// Toác
                            else if (x.SelectID == -1000)
                            {
                                /// Đóng NPCDialog
                                KT_TCPHandler.CloseDialog(player);
                            }
                        };
                        KTNPCDialogManager.AddNPCDialog(dialog);
                        dialog.Show(trigger, player);
                    };
                });
            }

            /// Duyệt danh sách bẫy
            foreach (MilitaryCamp.EventInfo.StageInfo.TriggerData.TrapInfo trapInfo in stageInfo.Triggers.Traps)
            {
                /// Tạo bẫy
                KDynamicArea trap = KTDynamicAreaManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, trapInfo.Name, trapInfo.ID, trapInfo.PosX, trapInfo.PosY, -1, 1f, 100, -1, null);
                trap.OnEnter = (obj) => {
                    if (obj is KPlayer player)
                    {
                        /// Thông báo dính phải bẫy
                        PlayerManager.ShowNotification(player, trapInfo.TouchMessage);
                        /// Quay về vị trí xuất phát
                        GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, this.CopyScene.EnterPosX, this.CopyScene.EnterPosY, (int) player.CurrentDir);
                    }
                };
            }
        }

        /// <summary>
        /// Tạo quái thuộc Stage tương ứng
        /// </summary>
        /// <param name="stageInfo"></param>
        private void CreateStageMonsters(MilitaryCamp.EventInfo.StageInfo stageInfo)
        {
            /// Duyệt danh sách quái
            foreach (MilitaryCamp.EventInfo.StageInfo.MonsterInfo monsterInfo in stageInfo.Monsters)
            {
                /// Tạo quái
                this.CreateMonster(monsterInfo);
            }
        }

        /// <summary>
        /// Tạo cổng dịch chuyển lúc bắt đầu Stage tương ứng
        /// </summary>
        /// <param name="stageInfo"></param>
        private void CreateStageImmediateTeleports(MilitaryCamp.EventInfo.StageInfo stageInfo)
        {
            /// Duyệt danh sách cổng dịch chuyển
            foreach (MilitaryCamp.EventInfo.StageInfo.TeleportInfo teleportInfo in stageInfo.Teleports)
            {
                /// Nếu là cổng dịch chuyển lúc bắt đầu Stage
                if (teleportInfo.SpawnImmediate)
                {
                    /// Tạo cổng
                    this.CreateTeleport(teleportInfo);
                }
            }
        }

        /// <summary>
        /// Tạo cổng dịch chuyển lúc hoàn thành Stage tương ứng
        /// </summary>
        /// <param name="stageInfo"></param>
        private void CreateStageDoneTeleports(MilitaryCamp.EventInfo.StageInfo stageInfo)
        {
            /// Duyệt danh sách cổng dịch chuyển
            foreach (MilitaryCamp.EventInfo.StageInfo.TeleportInfo teleportInfo in stageInfo.Teleports)
            {
                /// Nếu là cổng dịch chuyển lúc kết thúc Stage
                if (!teleportInfo.SpawnImmediate)
                {
                    /// Tạo cổng
                    this.CreateTeleport(teleportInfo);
                }
            }
        }

        /// <summary>
        /// Bắt đầu Stage
        /// </summary>
        private void BeginStage()
        {
            /// Thông tin tầng
            MilitaryCamp.EventInfo.StageInfo stageInfo = this.EventData.Stages[this.currentStageID];
            /// Xóa danh sách cơ quan cản đường đã mở
            this.openedObstacles.Clear();
            /// Tạo cơ quan
            this.CreateStageTriggers(stageInfo);
            /// Tạo quái
            this.CreateStageMonsters(stageInfo);
            /// Tạo cổng dịch chuyển bắt đầu
            this.CreateStageImmediateTeleports(stageInfo);

            /// Thiết lập lại danh sách nhiệm vụ đã hoàn thành
            this.completedTasks.Clear();
            /// Thiết lập lại danh sách nhiệm vụ đang thực hiện
            this.doingTasks.Clear();
        }

        /// <summary>
        /// Đã hoàn thành tầng hiện tại chưa
        /// </summary>
        /// <returns></returns>
        private bool IsCurrentStageCompleted()
        {
            /// Thông tin tầng
            MilitaryCamp.EventInfo.StageInfo stageInfo = this.EventData.Stages[this.currentStageID];
            /// Nếu đã hoàn thành tất cả chuỗi nhiệm vụ của tầng thì hoàn thành tầng hiện tại
            return stageInfo.Tasks.Count == this.completedTasks.Count;
        }

        /// <summary>
        /// Hoàn thành Stage
        /// </summary>
        private void CompleteStage()
        {
            /// Thông tin tầng
            MilitaryCamp.EventInfo.StageInfo stageInfo = this.EventData.Stages[this.currentStageID];
            /// Tạo cổng dịch chuyển hoàn thành
            this.CreateStageDoneTeleports(stageInfo);
        }

        /// <summary>
        /// Hoàn thành phụ bản
        /// </summary>
        private void CompleteEvent()
        {
            /// Đánh dấu thời điểm hoàn thành phụ bản
            this.eventCompletedTicks = KTGlobal.GetCurrentTimeMilis();
            /// Thông báo hoàn thành
            this.NotifyAllPlayers(string.Format("Hoàn thành {0}, sau 30s sẽ rời khỏi phụ bản!", this.EventData.Name));
        }

        /// <summary>
        /// Xử lý các nhiệm vụ của tầng
        /// </summary>
        private void HandleTasks()
        {
            /// Thông tin tầng
            MilitaryCamp.EventInfo.StageInfo stageInfo = this.EventData.Stages[this.currentStageID];

            /// Duyệt danh sách nhiệm vụ
            foreach (MilitaryCamp.EventInfo.StageInfo.TaskInfo task in stageInfo.Tasks)
            {
                /// Nếu nhiệm vụ này đã được hoàn thành
                if (this.completedTasks.Contains(task.ID))
                {
                    /// Bỏ qua
                    continue;
                }

                /// Nếu nhiệm vụ này đang được thực hiện
                if (this.doingTasks.Contains(task.ID))
                {
                    /// Đã hoàn thành chưa
                    bool isCompleted = false;

                    /// Loại nhiệm vụ là gì
                    switch (task.Type)
                    {
                        /// Thu thập vật phẩm
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.CollectGrowPoints:
                        {
                            isCompleted = this.Track_CollectGrowPoints(task);
                            break;
                        }
                        /// Tiêu diệt Boss
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.KillBoss:
                        {
                            isCompleted = this.Track_KillBoss(task);
                            break;
                        }
                        /// Giao vật phẩm cho NPC
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.GiveGoodsToNPC:
                        {
                            isCompleted = this.Track_GiveGoodsToNPC(task);
                            break;
                        }
                        /// Mở cơ quan và tiêu diệt quái
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenTriggerAndKillMonsters:
                        {
                            isCompleted = this.Track_OpenTriggerAndKillMonsters(task);
                            break;
                        }
                        /// Hộ tống NPC
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.TransferNPC:
                        {
                            isCompleted = this.Track_TransferNPC(task);
                            break;
                        }
                        /// Mở toàn bộ cơ quan theo thứ tự
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenOrderedTriggers:
                        {
                            isCompleted = this.Track_OpenOrderedTriggers(task);
                            break;
                        }
                        /// Mở cơ quan
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenTrigger:
                        {
                            isCompleted = this.Track_OpenTrigger(task);
                            break;
                        }
                        /// Đoán số
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.GuessNumber:
                        {
                            isCompleted = this.Track_GuessNumber(task);
                            break;
                        }
                        /// Mở toàn bộ cơ quan cùng lúc
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenAllTriggersSameMoment:
                        {
                            isCompleted = this.Track_OpenAllTriggersSameMoment(task);
                            break;
                        }
                        /// Mở cơ quan và dập lửa
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenAndProtectAllTriggers:
                        {
                            isCompleted = this.Track_OpenAndProtectAllTriggers(task);
                            break;
                        }
                    }

                    /// Nếu đã hoàn thành
                    if (isCompleted)
                    {
                        /// Thông báo hoàn thành nhiệm vụ
                        this.NotifyAllPlayers(string.Format("Hoàn thành nhiệm vụ: {0}", task.Name));

                        /// Thêm nhiệm vụ vào danh sách đã hoàn thành
                        this.completedTasks.Add(task.ID);
                        /// Xóa nhiệm vụ khỏi danh sách đang làm
                        this.doingTasks.Remove(task.ID);

                        /// Reset toàn bộ dữ liệu nhiệm vụ
                        this.Reset_CollectGrowPoints();
                        this.Reset_KillBoss();
                        this.Reset_GiveGoodsToNPC();
                        this.Reset_OpenTriggerAndKillMonsters();
                        this.Reset_TransferNPC();
                        this.Reset_OpenOrderedTriggers();
                        this.Reset_OpenTrigger();
                        this.Reset_GuessNumber();
                        this.Reset_OpenAllTriggersSameMoment();
                        this.Reset_OpenAndProtectAllTriggers();
                    }
                }
                /// Nếu nhiệm vụ này chưa được thực hiện
                else
                {
                    /// Nếu không đủ điều kiện nhận nhiệm vụ
                    if (!this.IsAbleToTakeTask(task))
                    {
                        /// Bỏ qua
                        continue;
                    }

                    /// Loại nhiệm vụ là gì
                    switch (task.Type)
                    {
                        /// Thu thập vật phẩm
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.CollectGrowPoints:
                        {
                            this.Begin_CollectGrowPoints(task);
                            break;
                        }
                        /// Tiêu diệt Boss
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.KillBoss:
                        {
                            this.Begin_KillBoss(task);
                            break;
                        }
                        /// Giao vật phẩm cho NPC
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.GiveGoodsToNPC:
                        {
                            this.Begin_GiveGoodsToNPC(task);
                            break;
                        }
                        /// Mở cơ quan và tiêu diệt quái
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenTriggerAndKillMonsters:
                        {
                            this.Begin_OpenTriggerAndKillMonsters(task);
                            break;
                        }
                        /// Hộ tống NPC
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.TransferNPC:
                        {
                            this.Begin_TransferNPC(task);
                            break;
                        }
                        /// Mở toàn bộ cơ quan theo thứ tự
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenOrderedTriggers:
                        {
                            this.Begin_OpenOrderedTriggers(task);
                            break;
                        }
                        /// Mở cơ quan
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenTrigger:
                        {
                            this.Begin_OpenTrigger(task);
                            break;
                        }
                        /// Đoán số
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.GuessNumber:
                        {
                            this.Begin_GuessNumber(task);
                            break;
                        }
                        /// Mở toàn bộ cơ quan cùng lúc
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenAllTriggersSameMoment:
                        {
                            this.Begin_OpenAllTriggersSameMoment(task);
                            break;
                        }
                        /// Mở cơ quan và dập lửa
                        case MilitaryCamp.EventInfo.StageInfo.TaskInfo.TaskType.OpenAndProtectAllTriggers:
                        {
                            this.Begin_OpenAndProtectAllTriggers(task);
                            break;
                        }
                    }

                    /// Thông báo bắt đầu nhiệm vụ
                    this.NotifyAllPlayers(string.Format("Bắt đầu nhiệm vụ: {0}", task.Name));
                }
            }
        }

        /// <summary>
        /// Có đủ điều kiện nhận nhiệm vụ không
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private bool IsAbleToTakeTask(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Nếu không cần hoàn thành nhiệm vụ nào trước đó
            if (task.RequireTasks.Count <= 0)
            {
                /// OK
                return true;
            }

            /// Duyệt danh sách cần hoàn thành trước
            foreach (int taskID in task.RequireTasks)
            {
                /// Nếu chưa hoàn thành
                if (!this.completedTasks.Contains(taskID))
                {
                    /// Toác
                    return false;
                }
            }

            /// OK
            return true;
        }

        /// <summary>
		/// Đưa người chơi trở lại bản đồ báo danh
		/// </summary>
		/// <param name="player"></param>
		private void KickOutPlayer(KPlayer player)
        {
            GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, this.EventData.Config.OutMapID, this.EventData.Config.OutPosX, this.EventData.Config.OutPosY, (int) player.CurrentDir);
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
        #endregion

        #region Support methods
        /// <summary>
        /// Tạo điểm thu thập tương ứng
        /// </summary>
        /// <param name="data"></param>
        private void CreateGrowPoints(MilitaryCamp.EventInfo.StageInfo.TaskInfo.GrowPointInfo data)
        {
            /// <summary>
            /// Thực hiện tạo điểm thu thập tại vị trí tương ứng
            /// </summary>
            /// <param name="posX"></param>
            /// <param name="posY"></param>
            void DoCreate(int posX, int posY)
            {
                /// Tạo điểm thu thập
                GrowPoint growPoint = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(data.ID, data.Name, data.DisapearAfterBeenCollected ? -1 : data.RespawnTicks, -1, data.CollectTick, true), posX, posY);
                growPoint.ConditionCheck = (player) => {
                    /// Nếu túi đã đầy
                    if (!KTGlobal.IsHaveSpace(1, player))
                    {
                        PlayerManager.ShowNotification(player, "Túi đã đầy, không thể thu thập!");
                        return false;
                    }

                    /// OK
                    return true;
                };
                growPoint.GrowPointCollectCompleted = (player) => {
                    /// Nếu có đánh dấu xóa sau khi thu thập
                    if (data.DisapearAfterBeenCollected)
                    {
                        /// Xóa
                        this.RemoveGrowPoint(growPoint);
                    }
                    /// Thêm vật phẩm tương ứng vào người
                    ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, data.ItemID, 1, 0, "MilitaryCamp_EventScript", true, data.BoundAfterBeenCollected ? 1 : 0, false, Global.ConstGoodsEndTime, "", -1);
                };
            }

            /// Nếu tổng số điểm thu thập cần sinh ra lớn hơn số vị trí điểm thu thập được thiết lập
            if (data.Count < data.Positions.Count)
            {
                /// Chọn ngẫu nhiên đè nhau
                for (int i = 1; i <= data.Count; i++)
                {
                    /// Vị trí ngẫu nhiên
                    int randomPos = KTGlobal.GetRandomNumber(0, data.Positions.Count - 1);
                    /// Tạo điểm thu thập tại vị trí tương ứng
                    DoCreate(data.Positions[randomPos].x, data.Positions[randomPos].y);
                }
            }
            /// Nếu tổng số điểm thu thập cần sinh ra bằng số vị trí điểm thu thập được thiết lập
            else if (data.Count == data.Positions.Count)
            {
                /// Duyệt danh sách vị trí
                foreach (UnityEngine.Vector2Int pos in data.Positions)
                {
                    /// Tạo điểm thu thập tại vị trí tương ứng
                    DoCreate(pos.x, pos.y);
                }
            }
            /// Nếu tổng số điểm thu thập cần sinh ra nhỏ hơn số vị trí điểm thu thập được thiết lập
            else
            {
                /// Danh sách vị trí ngẫu nhiên
                List<UnityEngine.Vector2Int> randomPositions = data.Positions.RandomRange(data.Count).ToList();
                /// Duyệt danh sách vị trí
                foreach (UnityEngine.Vector2Int pos in randomPositions)
                {
                    /// Tạo điểm thu thập tại vị trí tương ứng
                    DoCreate(pos.x, pos.y);
                }
            }
        }

        /// <summary>
        /// Tạo cơ quan
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        private void CreateTrigger(MilitaryCamp.EventInfo.StageInfo.TaskInfo.TriggerInfo data, Action<GrowPoint, KPlayer> onOpen)
        {
            /// Tạo cơ quan
            GrowPoint trigger = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(data.ID, data.Name, -1, -1, data.CollectTick, true), data.PosX, data.PosY);
            trigger.GrowPointCollectCompleted = (player) => {
                /// Thực thi sự kiện khi mở cơ quan
                onOpen?.Invoke(trigger, player);
            };
        }

        /// <summary>
        /// Tạo cơ quan mở theo thứ tự
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onOpen"></param>
        private void CreateIndexTriggers(MilitaryCamp.EventInfo.StageInfo.TaskInfo.IndexTriggerInfo data, Action<GrowPoint, KPlayer, MilitaryCamp.EventInfo.StageInfo.TaskInfo.IndexTriggerInfo.OrderedTriggerInfo> onOpen)
        {
            /// <summary>
            /// Thực hiện tạo cơ quan tại vị trí tương ứng
            /// </summary>
            /// <param name="triggerInfo"></param>
            /// <param name="posX"></param>
            /// <param name="posY"></param>
            void DoCreate(MilitaryCamp.EventInfo.StageInfo.TaskInfo.IndexTriggerInfo.OrderedTriggerInfo triggerInfo, int posX, int posY)
            {
                /// Tạo cơ quan
                GrowPoint trigger = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(triggerInfo.ID, triggerInfo.Name, -1, -1, triggerInfo.CollectTick, true), posX, posY);
                trigger.GrowPointCollectCompleted = (player) => {
                    /// Thực thi sự kiện khi mở cơ quan
                    onOpen?.Invoke(trigger, player, triggerInfo);
                };
            }

            /// Nếu tổng số cơ quan cần sinh ra lớn hơn số vị trí cơ quan được thiết lập
            if (data.Triggers.Count < data.Positions.Count)
            {
                /// Chọn ngẫu nhiên đè nhau
                for (int i = 0; i < data.Triggers.Count - 1; i++)
                {
                    /// Vị trí ngẫu nhiên
                    int randomPos = KTGlobal.GetRandomNumber(0, data.Positions.Count - 1);
                    /// Tạo cơ quan tại vị trí tương ứng
                    DoCreate(data.Triggers[i], data.Positions[randomPos].x, data.Positions[randomPos].y);
                }
            }
            /// Nếu tổng số cơ quan cần sinh ra bằng số vị trí cơ quan được thiết lập
            else if (data.Triggers.Count == data.Positions.Count)
            {
                /// Duyệt danh sách cơ quan
                for (int i = 0; i < data.Triggers.Count - 1; i++)
                {
                    /// Tạo cơ quan tại vị trí tương ứng
                    DoCreate(data.Triggers[i], data.Positions[i].x, data.Positions[i].y);
                }
            }
            /// Nếu tổng số cơ quan cần sinh ra nhỏ hơn số vị trí cơ quan được thiết lập
            else
            {
                /// Danh sách vị trí ngẫu nhiên
                List<UnityEngine.Vector2Int> randomPositions = data.Positions.RandomRange(data.Triggers.Count).ToList();
                /// Duyệt danh sách cơ quan
                for (int i = 0; i < data.Triggers.Count - 1; i++)
                {
                    /// Tạo cơ quan tại vị trí tương ứng
                    DoCreate(data.Triggers[i], randomPositions[i].x, randomPositions[i].y);
                }
            }
        }

        /// <summary>
        /// Tạo thánh hỏa
        /// </summary>
        /// <param name="data"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="onExplode"></param>
        private void CreateHolyFire(MilitaryCamp.EventInfo.StageInfo.TaskInfo.ProtectTriggerInfo.HolyFireInfo data, int posX, int posY, Action onExplode)
        {
            /// Tạo thánh hỏa
            GrowPoint holyFire = KTGrowPointManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, GrowPointXML.Parse(data.ID, data.Name, -1, -1, data.CollectTick, true), posX, posY);
            holyFire.GrowPointCollectCompleted = (player) => {
                /// Hủy thánh hỏa
                this.RemoveGrowPoint(holyFire);
            };
            /// Thiết lập thời gian đếm lùi phát nổ
            this.SetTimeout(data.ActivateTicks, () => {
                /// Nếu thánh hỏa không còn tồn tại
                if (!holyFire.Alive)
                {
                    /// Bỏ qua
                    return;
                }
                /// Thực hiện phát nổ
                onExplode?.Invoke();
            });
        }

        /// <summary>
        /// Xóa toàn bộ thánh hỏa
        /// <param name="data"></param>
        /// </summary>
        private void RemoveAllHolyFires(MilitaryCamp.EventInfo.StageInfo.TaskInfo.ProtectTriggerInfo.HolyFireInfo data)
        {
            /// Duyệt danh sách điểm thu thập
            foreach (GrowPoint growPoint in this.GetGrowPoints())
            {
                /// Nếu đây là thánh hỏa
                if (growPoint.Data.ResID == data.ID && growPoint.Data.Name == data.Name)
                {
                    /// Hủy thánh hỏa
                    this.RemoveGrowPoint(growPoint);
                }
            }
        }

        /// <summary>
        /// Tạo quái tương ứng
        /// </summary>
        /// <param name="data"></param>
        private void CreateMonster(MilitaryCamp.EventInfo.StageInfo.MonsterInfo data, Action<GameObject> onDieCallback = null)
        {
            /// Ngũ hành
            KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Mức máu
            int hp = data.BaseHP + data.HPIncreaseEachLevel * this.CopyScene.Level;
            /// Tạo quái
            GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, data.ID, this.CopyScene.ID, 1, data.PosX, data.PosY, data.Name, data.Title, hp, 80, dir, series, data.AIType, -1, -1, data.AIScriptID, "Monster", (monster) => {
                /// Nếu có kỹ năng
                if (data.Skills.Count > 0)
                {
                    /// Duyệt danh sách kỹ năng
                    foreach (SkillLevelRef skill in data.Skills)
                    {
                        /// Thêm vào danh sách kỹ năng dùng của quái
                        monster.CustomAISkills.Add(skill);
                    }
                }

                /// Nếu có vòng sáng
                if (data.Auras.Count > 0)
                {
                    /// Duyệt danh sách vòng sáng
                    foreach (SkillLevelRef aura in data.Auras)
                    {
                        /// Kích hoạt vòng sáng
                        monster.UseSkill(aura.SkillID, aura.Level, monster);
                    }
                }
            }, 65535, null, onDieCallback);
        }

        /// <summary>
        /// Tạo Boss tương ứng
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onDieCallback"></param>
        private void CreateBoss(MilitaryCamp.EventInfo.StageInfo.BossInfo data, Action<GameObject> onDieCallback = null)
        {
            /// Ngũ hành
            KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Mức máu
            int hp = data.BaseHP + data.HPIncreaseEachLevel * this.CopyScene.Level;
            /// Tạo Boss
            GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, data.ID, this.CopyScene.ID, 1, data.PosX, data.PosY, data.Name, data.Title, hp, 80, dir, series, data.AIType, -1, -1, data.AIScriptID, "Boss", (monster) => {
                /// Nếu có kỹ năng
                if (data.Skills.Count > 0)
                {
                    /// Duyệt danh sách kỹ năng
                    foreach (SkillLevelRef skill in data.Skills)
                    {
                        /// Thêm vào danh sách kỹ năng dùng của quái
                        monster.CustomAISkills.Add(skill);
                    }
                }

                /// Nếu có vòng sáng
                if (data.Auras.Count > 0)
                {
                    /// Duyệt danh sách vòng sáng
                    foreach (SkillLevelRef aura in data.Auras)
                    {
                        /// Kích hoạt vòng sáng
                        monster.UseSkill(aura.SkillID, aura.Level, monster);
                    }
                }
            }, 65535, null, onDieCallback);
        }

        /// <summary>
        /// Tạo cổng dịch chuyển tương ứng
        /// </summary>
        /// <param name="data"></param>
        private void CreateTeleport(MilitaryCamp.EventInfo.StageInfo.TeleportInfo data)
        {
            /// Tạo cổng dịch chuyển
            KDynamicArea teleport = KTDynamicAreaManager.Add(this.CopyScene.MapCode, this.CopyScene.ID, data.Name, 5340, data.PosX, data.PosY, this.DurationTicks - this.LifeTimeTicks, 2f, 100, -1, null);
            teleport.OnEnter = (obj) => {
                if (obj is KPlayer player)
                {
                    /// Thực hiện chuyển vị trí
					GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, data.ToPosX, data.ToPosY, (int) player.CurrentDir);
                }
            };
        }

        /// <summary>
        /// Tạo NPC động tương ứng
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        private void CreateDynamicNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo.NPCInfo data, Action<NPC> callback)
        {
            /// Tạo NPC
            NPCGeneralManager.AddNewNPC(this.CopyScene.MapCode, this.CopyScene.ID, data.ID, data.PosX, data.PosY, data.Name, data.Title, Entities.Direction.DOWN_RIGHT, -1, "", callback);
        }

        /// <summary>
        /// Tạo NPC di chuyển tương ứng
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        private void CreateMovingNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo.MovingNPCInfo data, Action<Monster> callback)
        {
            /// Tạo NPC di chuyển
            GameManager.MonsterZoneMgr.AddDynamicMonsters(this.CopyScene.MapCode, data.ID, -1, 1, data.PosX, data.PosX, data.Name, data.Title, 100, this.CopyScene.Level, Entities.Direction.DOWN, Entities.KE_SERIES_TYPE.series_none, Entities.MonsterAIType.DynamicNPC, -1, -1, -1, "", (npc) => {
                /// Miễn dịch toàn bộ
                npc.m_CurrentStatusImmunity = true;
                npc.m_CurrentInvincibility = 1;
                /// Sử dụng thuật toán A* tìm đường
                npc.UseAStarPathFinder = true;
                /// Thực hiện di chuyển đến vị trí tương ứng
                npc.MoveTo(new System.Windows.Point(data.ToPosX, data.ToPosY), true);

                //    /// Sự kiện Tick
                //    npc.OnTick = () => {
                //        /// Đánh dấu khoảng cách hộ tống không thỏa mãn
                //        bool isNearNPC = false;
                //        /// Duyệt danh sách người chơi
                //        foreach (KPlayer player in this.teamPlayers)
                //        {
                //            /// Nếu ở gần
                //            if (KTGlobal.GetDistanceBetweenPoints(player.CurrentPos, npc.CurrentPos) <= data.Radius)
                //            {
                //                /// Đánh dấu khoảng cách hộ tống thỏa mãn
                //                isNearNPC = true;
                //                /// Không cần tìm thêm nữa
                //                break;
                //            }
                //        }

                //        /// Nếu tất cả người chơi đều ở quá xa NPC
                //        if (!isNearNPC)
                //        {
                //            /// Hủy NPC
                //            this.RemoveMonster(npc);
                //            /// Hủy thông tin NPC di chuyển
                //            this.movingNPC = null;
                //            /// Thông báo hộ tống thất bại
                //            this.NotifyAllPlayers(string.Format("Hộ tống {0} thất bại, khoảng cách quá xa. Hãy thử lại!", npc.RoleName));
                //        }
                //    };
            });
        }

        /// <summary>
        /// Tạo NPC tĩnh tương ứng điều khiển bởi Script Lua
        /// </summary>
        /// <param name="data"></param>
        private void CreateStaticNPC(MilitaryCamp.EventInfo.NPCInfo data)
        {
            /// Tạo NPC
			NPCGeneralManager.AddNewNPC(this.CopyScene.MapCode, this.CopyScene.ID, data.ID, data.PosX, data.PosY, data.Name, data.Title, Entities.Direction.DOWN_RIGHT, data.ScriptID, "", null);
        }
        #endregion
    }
}
