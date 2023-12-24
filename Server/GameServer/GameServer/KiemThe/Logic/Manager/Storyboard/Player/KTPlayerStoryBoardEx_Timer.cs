﻿using GameServer.KiemThe.Entities;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace GameServer.KiemThe.Logic
{
	/// <summary>
	/// Quản lý Timer của StoryBoard
	/// </summary>
	public partial class KTPlayerStoryBoardEx
	{

		

		#region Constants
		/// <summary>
		/// Thời điểm Tick
		/// </summary>
		private const int PeriodTick = 200;
		#endregion

		#region Define
		/// <summary>
		/// Định nghĩa StoryBoard của người chơi
		/// </summary>
		private class PlayerStoryBoard
		{
			/// <summary>
			/// Chủ nhân
			/// </summary>
			public KPlayer Owner { get; set; }

			/// <summary>
			/// ID đối tượng
			/// </summary>
			public int RoleID { get; set; }

			/// <summary>
			/// Đường đi
			/// </summary>
			public ConcurrentQueue<UnityEngine.Vector2> Paths { get; set; }

			/// <summary>
			/// Thời gian thực thi lần trước
			/// </summary>
			public long LastTick { get; set; } = 0;

			/// <summary>
			/// Thời gian thực thi lần trước
			/// </summary>
			public long LastTickTimer { get; set; } = 0;

			/// <summary>
			/// Tọa độ lưới lần tước
			/// </summary>
			public UnityEngine.Vector2 LastGridPos { get; set; }

			/// <summary>
			/// Loại động tác
			/// </summary>
			public KE_NPC_DOING Action { get; set; }

			/// <summary>
			/// Đã hoàn tất di chuyển trước chưa
			/// </summary>
			public bool HasCompletedLastMove { get; set; }

			/// <summary>
			/// Đã đến thời gian Tick chưa
			/// </summary>
			public bool IsTickTime
			{
				get
				{
					return KTGlobal.GetCurrentTimeMilis() - this.LastTickTimer >= KTPlayerStoryBoardEx.PeriodTick;
				}
			}
		}

		/// <summary>
		/// Danh sách StoryBoard đang được thực thi
		/// </summary>
		private readonly ConcurrentDictionary<int, PlayerStoryBoard> objectTimers = new ConcurrentDictionary<int, PlayerStoryBoard>();
		#endregion

		#region Core
		/// <summary>
		/// Worker
		/// </summary>
		private BackgroundWorker[] worker;

		/// <summary>
		/// Bắt đầu luồng thực thi
		/// </summary>
		public void StartTimer()
		{
			/// Thời gian báo cáo trạng thái lần cuối
			long[] lastReportStateTick = new long[ServerConfig.Instance.MaxPlayerStoryBoardThread];
			/// Thời gian thực thi công việc lần cuối
			long[] lastWorkerBusyTick = new long[ServerConfig.Instance.MaxPlayerStoryBoardThread];

			/// Tạo mới Worker
			this.worker = new BackgroundWorker[ServerConfig.Instance.MaxPlayerStoryBoardThread];
			for (int i = 0; i < this.worker.Length; i++)
            {
				this.worker[i] = new BackgroundWorker();
				this.worker[i].DoWork += this.DoBackgroundWork;
				this.worker[i].RunWorkerCompleted += this.Worker_Completed;

				lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
				lastWorkerBusyTick[i] = 0;
			}

			/// Tạo Timer riêng
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.AutoReset = true;
			timer.Interval = 10;
			timer.Elapsed += (o, e) => {
				/// Nếu có lệnh ngắt Server
				if (Program.NeedExitServer)
				{
					LogManager.WriteLog(LogTypes.TimerReport, string.Format("Terminate => {0}", "KTPlayerStoryBoard"));
					/// Ngừng luồng
					timer.Stop();
					return;
				}

				/// Duyệt toàn bộ danh sách Worker
				for (int i = 0; i < this.worker.Length; i++)
                {
					if (KTGlobal.GetCurrentTimeMilis() - lastReportStateTick[i] >= 5000)
					{
						LogManager.WriteLog(LogTypes.TimerReport, string.Format("Tick alive => KTPlayerStoryBoard[{0}]", i));
						/// Cập nhật thời gian báo cáo công việc lần cuối
						lastReportStateTick[i] = KTGlobal.GetCurrentTimeMilis();
					}

					if (this.worker[i].IsBusy)
					{
						/// Quá thời gian thì Cancel
						if (KTGlobal.GetCurrentTimeMilis() - lastWorkerBusyTick[i] >= 2000)
						{
							LogManager.WriteLog(LogTypes.TimerReport, string.Format("Timeout => KTPlayerStoryBoard[{0}]", i));
							/// Cập nhật thời gian thực thi công việc lần cuối
							lastWorkerBusyTick[i] = KTGlobal.GetCurrentTimeMilis();
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

				//Console.WriteLine("BGWORK START TICK!");
				/// ID Worker
				int workerID = (int) e.Argument;

				/// Danh sách cần xóa
				List<int> toRemoves = null;

				/// Danh sách khóa
				List<int> keys = this.objectTimers.Keys.ToList();
				/// Duyệt danh sách StoryBoard
				foreach (int key in keys)
				{
                    /// Nếu StoryBoard không có trong danh sách
                    if (!this.objectTimers.TryGetValue(key, out PlayerStoryBoard storyBoard))
					{
						/// Bỏ qua
						continue;
					}

					/// Nếu NULL thì xóa
					if (storyBoard == null)
					{
						/// Thêm vào danh sách cần xóa
						toRemoves.Add(key);
						continue;
					}
					/// Toác tiếp
					else if (storyBoard.Owner == null)
					{
						/// Thêm vào danh sách cần xóa
						toRemoves.Add(key);
						continue;
					}

					/// Nếu không phải công việc do Worker này xử lý thì thôi
					if (storyBoard.Owner.StaticID % ServerConfig.Instance.MaxPlayerStoryBoardThread != workerID)
                    {
						continue;
                    }

					/// Nếu đã đến thời gian Tick
					if (storyBoard.IsTickTime)
					{
						/// Đánh dấu thời điểm Tick
						storyBoard.LastTickTimer = KTGlobal.GetCurrentTimeMilis();
						/// Thực thi sự kiện Tick
						this.ExecuteAction(() => {
                            //-----fix jackson đóng thông báo
                            //Console.WriteLine("TICK START AT:" + storyBoard.Owner._DebugTime.Elapsed.TotalMilliseconds);
                            this.TickStoryBoard(storyBoard);
						}, (ex) => {
							/// Thực thi sự kiện Destroy
							this.StopStoryBoard(storyBoard);
						});
					}
				}

				/// Nếu tồn tại danh sách cần xóa
				if (toRemoves != null)
				{
					foreach (int key in toRemoves)
					{
						/// Xóa
						this.objectTimers.TryRemove(key, out _);
					}
					/// Làm rỗng danh sách cần xóa
					toRemoves.Clear();
					toRemoves = null;
				}
			}
			catch (Exception ex)
			{
				LogManager.WriteLog(LogTypes.Exception, ex.ToString());
			}
		}

		/// <summary>
		/// Thực thi sự kiện gì đó
		/// </summary>
		/// <param name="work"></param>
		/// <param name="onError"></param>
		private void ExecuteAction(Action work, Action<Exception> onError)
		{
			try
			{
				work?.Invoke();
			}
			catch (Exception ex)
			{
				LogManager.WriteLog(LogTypes.Exception, ex.ToString());
				onError?.Invoke(ex);
			}
		}
		#endregion
	}
}
