using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý luồng thực thi Buff của nhân vật
    /// </summary>
    public class KTPlayerTimerManager
    {
        #region Singleton - Instance

        /// <summary>
        /// Đối tượng quản lý Timer của đối tượng
        /// </summary>
        public static KTPlayerTimerManager Instance { get; private set; }

        /// <summary>
        /// Private constructor
        /// </summary>
        private KTPlayerTimerManager() : base() { }

        #endregion Singleton - Instance

        #region Initialize

        /// <summary>
        /// Hàm này gọi đến khởi tạo đối tượng
        /// </summary>
        public static void Init()
        {
            KTPlayerTimerManager.Instance = new KTPlayerTimerManager();
            KTPlayerTimerManager.Instance.StartTimer();
        }

        #endregion Initialize

        #region Define
        /// <summary>
        /// Thông tin yêu cầu
        /// </summary>
        private class QueueItem
		{
            /// <summary>
            /// Loại yêu cầu
            /// <para>1: Thêm, 0: Xóa</para>
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// Người chơi tương ứng
            /// </summary>
            public KPlayer Player { get; set; }
		}
		#endregion

		#region Core
		/// <summary>
		/// Worker
		/// </summary>
		private BackgroundWorker[] worker;

        /// <summary>
        /// Worker cập nhật vị trí
        /// </summary>
        private BackgroundWorker[] updateGridWorker;

        /// <summary>
        /// Bắt đầu chạy Timer
        /// </summary>
        private void StartTimer()
		{
            /// Khởi tạo Worker Logic
            {
                /// Thời gian báo cáo trạng thái lần cuối
                long[] lastReportStateTick = new long[ServerConfig.Instance.MaxPlayerStoryBoardThread];
                /// Thời gian thực thi công việc lần cuối
                long[] lastWorkerBusyTick = new long[ServerConfig.Instance.MaxPlayerStoryBoardThread];
                /// Khởi tạo worker
                this.worker = new BackgroundWorker[ServerConfig.Instance.MaxPlayerStoryBoardThread];
                for (int i = 0; i < this.worker.Length; i++)
                {
                    this.worker[i] = new BackgroundWorker();
                    this.worker[i].DoWork += this.DoBackgroundWork;
                    this.worker[i].RunWorkerCompleted += this.Worker_Completed;
                    /// Thời gian báo cáo trạng thái lần cuối
                    lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
                    /// Thời gian thực thi công việc lần cuối
                    lastWorkerBusyTick[i] = 0;
                }

                /// Tạo Timer riêng
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.AutoReset = true;
                timer.Interval = 500;
                timer.Elapsed += (o, e) => {
                    /// Nếu có lệnh ngắt Server
                    if (Program.NeedExitServer)
                    {
                        LogManager.WriteLog(LogTypes.TimerReport, string.Format("Terminate => {0}", "PlayerTimer"));
                        /// Ngừng luồng
                        timer.Stop();
                        return;
                    }

                    for (int i = 0; i < this.worker.Length; i++)
                    {
                        if (KTGlobal.GetCurrentTimeMilis() - lastReportStateTick[i] >= 5000)
                        {
                            LogManager.WriteLog(LogTypes.TimerReport, "Tick alive => PlayerTimer");
                            lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
                        }

                        if (this.worker[i].IsBusy)
                        {
                            /// Quá thời gian thì Cancel
                            if (KTGlobal.GetCurrentTimeMilis() - lastWorkerBusyTick[i] >= 2000)
                            {
                                /// Cập nhật thời gian thực thi công việc lần cuối
                                lastWorkerBusyTick[i] = KTGlobal.GetCurrentTimeMilis();
                                LogManager.WriteLog(LogTypes.TimerReport, string.Format("Timeout => PlayerTimer"));
                            }
                        }
                        else
                        {
                            /// Cập nhật thời gian thực thi công việc lần cuối
                            lastWorkerBusyTick[i] = KTGlobal.GetCurrentTimeMilis();
                            /// Thực thi công việc
                            if (!this.worker[i].IsBusy)
                            {
                                this.worker[i].RunWorkerAsync(i);
                            }
                        }
                    }
                };
                timer.Start();
            }

            /// Khởi tạo Worker cập nhật vị trí
            {
                /// Thời gian báo cáo trạng thái lần cuối
                long[] lastReportStateTick = new long[ServerConfig.Instance.MaxUpdateGridThread];
                /// Thời gian thực thi công việc lần cuối
                long[] lastWorkerBusyTick = new long[ServerConfig.Instance.MaxUpdateGridThread];
                /// Khởi tạo worker
                this.updateGridWorker = new BackgroundWorker[ServerConfig.Instance.MaxUpdateGridThread];
                for (int i = 0; i < this.updateGridWorker.Length; i++)
                {
                    this.updateGridWorker[i] = new BackgroundWorker();
                    this.updateGridWorker[i].DoWork += this.DoUpdateGridBackgroundWork;
                    this.updateGridWorker[i].RunWorkerCompleted += this.Worker_Completed;
                    /// Thời gian báo cáo trạng thái lần cuối
                    lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
                    /// Thời gian thực thi công việc lần cuối
                    lastWorkerBusyTick[i] = 0;
                }

                /// Tạo Timer riêng
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.AutoReset = true;
                timer.Interval = 200;
                timer.Elapsed += (o, e) => {
                    /// Nếu có lệnh ngắt Server
                    if (Program.NeedExitServer)
                    {
                        LogManager.WriteLog(LogTypes.TimerReport, string.Format("Terminate => {0}", "PlayerUpdateGridTimer"));
                        /// Ngừng luồng
                        timer.Stop();
                        return;
                    }

                    for (int i = 0; i < this.updateGridWorker.Length; i++)
                    {
                        if (KTGlobal.GetCurrentTimeMilis() - lastReportStateTick[i] >= 5000)
                        {
                            LogManager.WriteLog(LogTypes.TimerReport, "Tick alive => PlayerUpdateGridTimer");
                            lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
                        }

                        if (this.updateGridWorker[i].IsBusy)
                        {
                            /// Quá thời gian thì Cancel
                            if (KTGlobal.GetCurrentTimeMilis() - lastWorkerBusyTick[i] >= 2000)
                            {
                                /// Cập nhật thời gian thực thi công việc lần cuối
                                lastWorkerBusyTick[i] = KTGlobal.GetCurrentTimeMilis();
                                LogManager.WriteLog(LogTypes.TimerReport, string.Format("Timeout => PlayerUpdateGridTimer"));
                            }
                        }
                        else
                        {
                            /// Cập nhật thời gian thực thi công việc lần cuối
                            lastWorkerBusyTick[i] = KTGlobal.GetCurrentTimeMilis();
                            /// Thực thi công việc
                            if (!this.updateGridWorker[i].IsBusy)
                            {
                                this.updateGridWorker[i].RunWorkerAsync(i);
                            }
                        }
                    }
                };
                timer.Start();
            }
        }

        /// <summary>
        /// Sự kiện khi Background Worker hoàn tất công việc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogManager.WriteLog(LogTypes.Exception, e.Error.ToString());
            }
        }

        /// <summary>
		/// Thực hiện công việc
		/// </summary>
		private void DoBackgroundWork(object sender, DoWorkEventArgs e)
		{
			try
            {
                /// ID luồng
                int workerID = (int) e.Argument;

                /// Duyệt danh sách chờ
                while (!this.waitingQueue.IsEmpty)
				{
                    if (this.waitingQueue.TryDequeue(out QueueItem item))
					{
                        if (item.Type == 1)
						{
                            /// Thêm
                            this.players[item.Player.RoleID] = item.Player;
                        }
                        else if (item.Type == 0)
						{
                            /// Xóa
                            this.players.TryRemove(item.Player.RoleID, out _);
                        }
					}
				}

                /// Duyệt danh sách người chơi
                List<int> keys = this.players.Keys.ToList();
                foreach (int key in keys)
				{
                    /// Nếu người chơi không tồn tại
                    if (!this.players.TryGetValue(key, out KPlayer player))
                    {
                        continue;
                    }

                    /// Nếu đây không phải việc của Worker này
                    if (player.StaticID % ServerConfig.Instance.MaxPlayerStoryBoardThread != workerID)
                    {
                        continue;
                    }

                    /// Nếu đang Logout
                    if (player.ClosingClientStep > 0)
                    {
                        continue;
                    }

                    try
                    {
                        /// Thực thi sự kiện
                        player.Tick();
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                    }
                }
            }
            catch (Exception ex)
			{
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
		}

        /// <summary>
		/// Thực hiện công việc cập nhật lưới
		/// </summary>
		private void DoUpdateGridBackgroundWork(object sender, DoWorkEventArgs e)
		{
			try
            {
                /// ID luồng
                int workerID = (int) e.Argument;

                /// Duyệt danh sách người chơi
                List<int> keys = this.players.Keys.ToList();
                foreach (int key in keys)
				{
                    /// Nếu người chơi không tồn tại
                    if (!this.players.TryGetValue(key, out KPlayer player))
                    {
                        continue;
                    }

                    /// Nếu đây không phải việc của Worker này
                    if (player.StaticID % ServerConfig.Instance.MaxUpdateGridThread != workerID)
                    {
                        continue;
                    }

                    /// Nếu đang Logout
                    if (player.ClosingClientStep > 0)
                    {
                        continue;
                    }

                    try
                    {
                        /// Thực thi sự kiện
                        ClientManager.DoSpriteMapGridMove(player);
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                    }

                    //Thread.Sleep(2);
                }
            }
            catch (Exception ex)
			{
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
		}
        #endregion

        /// <summary>
        /// Queue chứa danh sách người chơi
        /// </summary>
        private readonly ConcurrentQueue<QueueItem> waitingQueue = new ConcurrentQueue<QueueItem>();

        /// <summary>
        /// Danh sách người chơi
        /// </summary>
        private readonly ConcurrentDictionary<int, KPlayer> players = new ConcurrentDictionary<int, KPlayer>();

        /// <summary>
        /// Thêm người chơi vào danh sách
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(KPlayer player)
		{
            this.waitingQueue.Enqueue(new QueueItem()
            {
                Type = 1,
                Player = player,
            });
        }

        /// <summary>
        /// Xóa người chơi khỏi danh sách
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(KPlayer player)
		{
            this.waitingQueue.Enqueue(new QueueItem()
            {
                Type = 0,
                Player = player,
            });
        }
    }
}