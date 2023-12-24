using GameServer.Logic;
using Server.Tools;
using System;
using System.Diagnostics;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
	/// <summary>
	/// Logic của StoryBoard
	/// </summary>
	public partial class KTPlayerStoryBoardEx
	{
		#region Start
		/// <summary>
		/// Bắt đầu StoryBoard tương ứng
		/// </summary>
		/// <param name="storyBoard"></param>
		private void StartStoryBoard(PlayerStoryBoard storyBoard)
		{
            try
            {
                /// Nếu dính trạng thái không thể di chuyển
                if (!storyBoard.Owner.IsCanMove())
                {
                    this.Remove(storyBoard.Owner);
                    return;
                }

                /// Cập nhật động tác đang thực hiện
                storyBoard.Owner.m_eDoing = storyBoard.Action;

                /// Nếu hàng đợi rỗng
                if (storyBoard.Paths.Count <= 0)
                {
                    return;
                }

                /// Vị trí hiện tại
                UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((int) storyBoard.Owner.CurrentPos.X, (int) storyBoard.Owner.CurrentPos.Y);
                /// Nếu vị trí hiện tại trùng với vị trí đầu tiên trong hàng đợi thì bỏ qua vị trí đầu
                if (storyBoard.Paths.TryPeek(out UnityEngine.Vector2 nextPos) && currentPos == nextPos)
                {
                    storyBoard.Paths.TryDequeue(out _);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
		#endregion

		#region Stop
		/// <summary>
		/// Ngừng StoryBoard tương ứng
		/// </summary>
		/// <param name="storyBoard"></param>
		private void StopStoryBoard(PlayerStoryBoard storyBoard)
		{
            try
            {
                /// Nếu đang thực hiện động tác ngồi
                if (storyBoard.Owner.m_eDoing == Entities.KE_NPC_DOING.do_sit)
                {
                    storyBoard.Owner.m_eDoing = Entities.KE_NPC_DOING.do_sit;
                }
                /// Nếu đang thực hiện động tác nhảy
                else if (storyBoard.Owner.m_eDoing == Entities.KE_NPC_DOING.do_jump)
                {
                    storyBoard.Owner.m_eDoing = Entities.KE_NPC_DOING.do_jump;
                }
                /// Nếu không có StoryBoard cũ
                else if (!this.HasStoryBoard(storyBoard.Owner))
                {
                    storyBoard.Owner.m_eDoing = Entities.KE_NPC_DOING.do_stand;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
		#endregion

		#region Tick
		/// <summary>
		/// Tick thực hiện StoryBoard
		/// </summary>
		/// <param name="timer"></param>
		private void TickStoryBoard(PlayerStoryBoard storyBoard)
        {
            try
            {
                /// Nếu StoryBoard Tick cũ chưa hoàn thành
                if (!storyBoard.HasCompletedLastMove)
				{
                    return;
				}

                /// Nếu dính trạng thái không thể di chuyển
                if (!storyBoard.Owner.IsCanMove())
                {
                    //Console.WriteLine("Can not move, removed by own storyboard");
                    this.Remove(storyBoard.Owner);
                    return;
                }

                ///// Thực hiện xóa Buff bảo vệ tân thủ
                //storyBoard.Owner.RemoveChangeMapProtectionBuff();

                /// Đánh dấu thời điểm lần cuối thực thi StoryBoard
                storyBoard.Owner.LastStoryBoardTicks = KTGlobal.GetCurrentTimeMilis();

                /// Đánh dấu chưa hoàn thành
                storyBoard.HasCompletedLastMove = false;


               

                /// Vị trí trước đó
                UnityEngine.Vector2 previousPos = new UnityEngine.Vector2((int) storyBoard.Owner.CurrentPos.X, (int) storyBoard.Owner.CurrentPos.Y);

                /// Thời gian Tick hiện tại
                long currentTick = KTGlobal.GetCurrentTimeMilis();
                /// Thời gian Tick lần trước
                long lastTick = storyBoard.LastTick;
                /// Khoảng thời gian cần mô phỏng (giây)
                float elapseTime = (currentTick - lastTick) / 1000f;
                /// Cập nhật thời gian Tick hiện tại
                storyBoard.LastTick = currentTick;

                //-----fix jackson đóng thông báo
                //Console.WriteLine("TOTAL TIME NEED TO MOVE :" + elapseTime);

                //Console.WriteLine("Tick storyboard, elapse time = " + elapseTime);

                /// Thực hiện di chuyển đối tượng trong khoảng thời gian cần mô phỏng
                bool needToRemove = KTStoryBoardLogic.StepMove(storyBoard.Owner, storyBoard.Paths, elapseTime, storyBoard.Action);

                GameMap gameMap = GameManager.MapMgr.GetGameMap(storyBoard.Owner.CurrentMapCode);
                /// Vị trí mới sau khi di chuyển
                UnityEngine.Vector2 newPos = new UnityEngine.Vector2((int) storyBoard.Owner.CurrentPos.X, (int) storyBoard.Owner.CurrentPos.Y);
                /// Tọa độ lưới
                UnityEngine.Vector2 newGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, newPos);

                ///// Nếu 2 vị trí trùng nhau
                //if (UnityEngine.Vector2.Distance(previousPos, newPos) <= 1)
                //{
                //    Console.WriteLine("Removed due to issue on storyboard");
                //    /// Lỗi gì đó nên xóa
                //    this.Remove(storyBoard.Owner);
                //    return;
                //}

                /// Nếu tọa độ lưới cũ khác tọa độ lưới mới
                if (newGridPos != storyBoard.LastGridPos)
                {
                    /// Cập nhật vị trí đối tượng vào Map
                    GameManager.MapGridMgr.DictGrids.TryGetValue(storyBoard.Owner.CurrentMapCode, out MapGrid mapGrid);
                    if (mapGrid != null)
                    {
                        if(!mapGrid.MoveObject(-1, -1, (int) storyBoard.Owner.CurrentPos.X, (int) storyBoard.Owner.CurrentPos.Y, storyBoard.Owner))
                        {
                            Console.WriteLine("TOAC KHI DICH CHUYEN NHAN VATTTTTTTTTTTTTTTTTTTT");
                        }
                    }

                    ///// Thực hiện gọi hàm cập nhật di chuyển
                    //ClientManager.DoSpriteMapGridMove(storyBoard.Owner);

                    /// Cập nhật tọa độ lưới cũ
                    storyBoard.LastGridPos = newGridPos;
                }

                /// Đánh dấu đã hoàn thành
                storyBoard.HasCompletedLastMove = true;

                //--------fix jackson đóng thông báo
                //Console.WriteLine("FINISH MOVE AT :" + storyBoard.Owner._DebugTime.Elapsed.TotalMilliseconds);

                /// Nếu cần thiết phải xóa StoryBoard
                if (needToRemove)
                {
                    //Console.WriteLine("Removed by own storyboard");
                    this.Remove(storyBoard.Owner, false);
                    return;
                }
            }
            catch (Exception ex)
            {
                /// Xóa StoryBoard vì lỗi
                this.Remove(storyBoard.Owner);
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
		#endregion
	}
}
