using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.XiaoYaoGu
{
	/// <summary>
	/// Script sự kiện Tiêu Dao Cốc
	/// </summary>
	public static class XoYo_EventScript
	{
		/// <summary>
		/// Mở Tiêu Dao Cốc không
		/// </summary>
		public const bool XoYo_Open = true;

		/// <summary>
		/// Mở tích điểm Tiêu Dao theo tháng
		/// </summary>
		public const bool XoYo_EnableMonthlyStorage = false;

		/// <summary>
		/// Thêm điểm tích lũy Tiêu Dao Cốc tháng này
		/// </summary>
		/// <param name="player"></param>
		/// <param name="storagePoint"></param>
		public static bool AddCurrentMonthStoragePoint(KPlayer player, int point)
		{
			/// Nếu chưa mở
			if (!XoYo_EventScript.XoYo_EnableMonthlyStorage)
			{
				return false;
			}

			DateTime nowTime = DateTime.Now;
			DateTime firstDayOfMonth = new DateTime(nowTime.Year, nowTime.Month, 1);
			DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			/// Tổng điểm
			int storagePoint = KTGlobal.GetRecoreByType(player, (int) MonthlyActivityRecord.XoYo_StoragePoint, firstDayOfMonth, lastDayOfMonth);
			if (storagePoint < 0)
			{
				storagePoint = 0;
			}
			storagePoint += point;
			return KTGlobal.AddRecoreByType(player, (int) MonthlyActivityRecord.XoYo_StoragePoint, storagePoint);
		}

		/// <summary>
		/// Trả về tổng số điểm tích lũy tháng trước của người chơi
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int GetLastMonthStoragePoint(KPlayer player)
		{
			DateTime nowTime = DateTime.Now;
			DateTime firstDayOfMonth = new DateTime(nowTime.Year, nowTime.Month, 1).AddMonths(-1);
			DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			/// Tổng điểm
			int storagePoint = KTGlobal.GetRecoreByType(player, (int) MonthlyActivityRecord.XoYo_StoragePoint, firstDayOfMonth, lastDayOfMonth);
			if (storagePoint < 0)
			{
				storagePoint = 0;
			}
			return storagePoint;
		}

		/// <summary>
		/// Trả về tổng số điểm tích lũy tháng này của người chơi
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int GetCurrentMonthStoragePoint(KPlayer player)
		{
			DateTime nowTime = DateTime.Now;
			DateTime firstDayOfMonth = new DateTime(nowTime.Year, nowTime.Month, 1);
			DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			/// Tổng điểm
			int storagePoint = KTGlobal.GetRecoreByType(player, (int) MonthlyActivityRecord.XoYo_StoragePoint, firstDayOfMonth, lastDayOfMonth);
			if (storagePoint < 0)
			{
				storagePoint = 0;
			}
			return storagePoint;
		}

		/// <summary>
		/// Kiểm tra người chơi này đã nhận thưởng tích lũy Tiêu Dao Cốc tháng vừa rồi chưa
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool HasAlreadyGottenLastMonthAward(KPlayer player)
		{
			return KTGlobal.GetMarkValue(player, DateTime.Now.Month.ToString(), (int) MonthlyActivityRecord.XoYo_StoragePoint) == 1;
		}

		/// <summary>
		/// Đánh dấu người chơi này đã nhận thưởng tích lũy tháng trước
		/// </summary>
		/// <param name="player"></param>
		public static bool MarkAsAlreadyGottenLastMonthAward(KPlayer player)
		{
			return KTGlobal.UpdateMarkValue(player, DateTime.Now.Month.ToString(), (int) MonthlyActivityRecord.XoYo_StoragePoint, 1);
		}

		/// <summary>
		/// Trả về thứ hạng tích lũy Tiêu Dao Cốc tháng vừa rồi
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int GetLastMonthStorageRank(KPlayer player)
		{
			//DateTime nowTime = DateTime.Now;
			//DateTime firstDayOfMonth = new DateTime(nowTime.Year, nowTime.Month, 1).AddMonths(-1);
			//DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			//return KTGlobal.GetRankByMarkAndTimeRanger((int) MonthlyActivityRecord.XoYo_StoragePoint, firstDayOfMonth, lastDayOfMonth, )
			return 1;
		}

		/// <summary>
		/// Trả về thứ hạng tích lũy Tiêu Dao Cốc tháng này
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int GetCurrentMonthStorageRank(KPlayer player)
		{
			//DateTime nowTime = DateTime.Now;
			//DateTime firstDayOfMonth = new DateTime(nowTime.Year, nowTime.Month, 1);
			//DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			//return KTGlobal.GetRankByMarkAndTimeRanger((int) MonthlyActivityRecord.XoYo_StoragePoint, firstDayOfMonth, lastDayOfMonth, )
			return 1;
		}

		/// <summary>
		/// Thực hiện nhận thưởng Tiêu Dao Cốc tháng vừa rồi
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetLastMonthAward(KPlayer player)
		{
			/// Nếu chưa mở
			if (!XoYo_EventScript.XoYo_EnableMonthlyStorage)
			{
				return "Chức năng chưa mở!";
			}

			/// Nếu đã nhận rồi
			if (XoYo_EventScript.HasAlreadyGottenLastMonthAward(player))
			{
				return "Ngươi đã nhận thưởng tích lũy Tiêu Dao Cốc tháng vừa rồi, không thể nhận thêm!";
			}

			/// Thứ hạng tháng vừa rồi
			int rank = XoYo_EventScript.GetLastMonthStorageRank(player);

			/// Quà tương ứng theo hạng
			XoYo.MonthlyStorageInfo.RankInfo rankInfo = XoYo.MonthlyStorageInfos.AwardsByRange.Where(x => x.FromRank <= rank && x.ToRank >= rank).FirstOrDefault();
			/// Nếu không có quà ở thứ hạng này
			if (rankInfo == null)
			{
				return "Thứ hạng của ngươi không đủ để nhận thưởng!";
			}

			/// Tổng số ô trống cần
			int totalSpacesNeed = 0;
			/// Duyệt danh sách quà thưởng
			foreach (GoodsData itemGD in rankInfo.Awards)
			{
				/// Tăng tổng số ô cần
				totalSpacesNeed += KTGlobal.GetTotalSpacesNeedToTakeItem(itemGD.GoodsID, itemGD.GCount);
			}

			/// Nếu túi không đủ ô trống
			if (!KTGlobal.IsHaveSpace(totalSpacesNeed, player))
			{
				return string.Format("Cần sắp xếp tối thiểu {0} ô trống trong túi để nhận thưởng!", totalSpacesNeed);
			}

			/// Đánh dấu đã nhận thưởng tháng này
			if (!XoYo_EventScript.MarkAsAlreadyGottenLastMonthAward(player))
			{
				return "Có lỗi trong quá trình thao tác, hãy thử lại sau!";
			}

			/// Danh sách quà thưởng
			foreach (GoodsData itemGD in rankInfo.Awards)
			{
				/// Thêm quà tương ứng
				ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, itemGD.GoodsID, itemGD.GCount, 0, "XoYo_EventScript", true, 0, false, Global.ConstGoodsEndTime, "", -1);
			}

			/// Tổng điểm tích lũy tháng vừa rồi
			int lastMonthStoragePoint = XoYo_EventScript.GetLastMonthStoragePoint(player);

			/// Trả về kết quả
			return string.Format("Tháng vừa rồi, ngươi đạt tổng tích lũy <color=yellow>{0} danh vọng</color>, xếp hạng <color=green>{1}</color>, nhận thưởng tích lũy Tiêu Dao Cốc thành công!", lastMonthStoragePoint, rank);
		}

		/// <summary>
		/// Kiểm tra người chơi tương ứng có thể vào Tiêu Dao Cốc không
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string CheckCondition(KPlayer player)
		{
			/// Nếu đóng vượt ải Tiêu Dao Cốc
			if (!XoYo_EventScript.XoYo_Open)
			{
				return "Hoạt động vượt ải Tiêu Dao Cốc hiện chưa mở!";
			}

			List<KPlayer> teammates = player.Teammates;

			/// Nếu không có nhóm
			if (player.TeamID == -1 || !KTTeamManager.IsTeamExist(player.TeamID))
			{
				return "Trong Tiêu Dao Cốc rất nguy hiểm, ngươi hãy lập tổ đội trước tiên!";
			}
			/// Nếu không phải đội trưởng
			else if (player.TeamLeader != null && player.TeamLeader != player)
			{
				return "Chỉ đội trưởng mới có thể thao tác!";
			}
			/// Nếu nhóm không đủ người
			else if (teammates.Count < XoYo.LimitMember)
			{
				return string.Format("Nhóm cần tối thiểu <color=green>{0} thành viên</color> mới có thể tham gia vượt ải Tiêu Dao Cốc.", XoYo.LimitMember);
			}
			/// Nếu cấp độ không đủ
			else if (player.m_Level < XoYo.RequireLevel)
			{
				return string.Format("Yêu cầu cấp độ tối thiểu <color=green>{0}</color> mới có thể tham gia vượt ải Tiêu Dao Cốc.", XoYo.RequireLevel);
			}
			/// Nếu đã tham gia trong ngày rồi thì thôi
			else if (CopySceneEventManager.GetCopySceneTotalEnterTimesToday(player, AcitvityRecore.XoYo) >= XoYo.LimitRoundEachDay)
			{
				return "Ngươi đã tham gia đủ số lượt trong ngày, không thể tham gia nữa!";
			}
			/// Nếu số lượng phụ bản đã đạt tối đa
			else if (CopySceneEventManager.CurrentCopyScenesCount >= CopySceneEventManager.LimitCopyScenes)
			{
				return "Số lượng phụ bản đã đạt giới hạn, hãy thử lại lúc khác!";
			}

			/// Kiểm tra xem có thành viên không trong khu vực
			foreach (KPlayer teammate in teammates)
			{
				/// Nếu khác bản đồ
				if (teammate.MapCode != player.MapCode)
				{
					return "Hãy tập hợp đủ thành viên tới chỗ ta trước khi tham gia!";
				}
			}

			/// Danh sách đội viên không đủ cấp
			List<string> notEnoughLevelPlayers = new List<string>();
			/// Kiểm tra nhóm
			foreach (KPlayer teammate in teammates)
			{
				/// Nếu đã tham gia rồi
				if (teammate.m_Level < XoYo.RequireLevel)
				{
					notEnoughLevelPlayers.Add(string.Format("<color=#4dbeff>[{0}]</color>", teammate.RoleName));
				}
			}

			/// Nếu tồn tại danh sách đội viên không đủ cấp
			if (notEnoughLevelPlayers.Count > 0)
			{
				return string.Format("Trong tổ đội có {0} cấp độ không đủ <color=green>{1}</color>, không thể tham gia!", string.Join(", ", notEnoughLevelPlayers), XoYo.RequireLevel);
			}

			/// Danh sách đội viên đã tham gia quá số lượt trong ngày
			List<string> alreadyAttempMaxRoundTodayPlayers = new List<string>();
			/// Kiểm tra nhóm
			foreach (KPlayer teammate in teammates)
			{
				/// Nếu đã tham gia rồi
				if (CopySceneEventManager.GetCopySceneTotalEnterTimesToday(teammate, AcitvityRecore.XoYo) >= XoYo.LimitRoundEachDay)
				{
					alreadyAttempMaxRoundTodayPlayers.Add(string.Format("<color=#4dbeff>[{0}]</color>", teammate.RoleName));
				}
			}

			/// Nếu tồn tại danh sách đội viên đã tham gia quá số lượt trong ngày
			if (alreadyAttempMaxRoundTodayPlayers.Count > 0)
			{
				return string.Format("Trong tổ đội có {0} đã tham gia quá số lượt trong ngày!", string.Join(", ", alreadyAttempMaxRoundTodayPlayers));
			}

			/// Trả về kết quả có thể tham gia
			return "OK";
		}

		/// <summary>
		/// Bắt đầu Tiêu Dao Cốc
		/// </summary>
		/// <param name="player"></param>
		/// <param name="stageID"></param>
		/// <param name="difficulty"></param>
		public static void Begin(KPlayer player, int stageID, int difficulty)
		{
			/// Nếu lỗi gì đó
			if (player == null || stageID <= 0 || stageID > XoYo.MaxStage || difficulty < 1 || difficulty > 3)
			{
				return;
			}

			/// Danh sách thành viên nhóm
			List<KPlayer> teammates = player.Teammates;

			/// Cấp độ cao nhất thành viên trong nhóm
			int nLevel = teammates.Max(x => x.m_Level);

			/// Danh sách các ải có cùng độ khó
			List<XoYo.MapInfo> stageDifficultyMaps = XoYo.Events.Where(x => x.Difficulty == difficulty).ToList();
			/// Chọn một ải ngẫu nhiên có cùng độ khó 
			XoYo.MapInfo mapInfo = stageDifficultyMaps[KTGlobal.GetRandomNumber(0, stageDifficultyMaps.Count - 1)];

			/// Nếu là ải đầu tiên
			if (stageID == 1)
			{
				/// Duyệt danh sách thành viên, đánh dấu đã tham gia Tiêu Dao Cốc
				foreach (KPlayer teammate in teammates)
				{
					int totalEnterTimes = CopySceneEventManager.GetCopySceneTotalEnterTimesToday(teammate, AcitvityRecore.XoYo);
					totalEnterTimes++;
					CopySceneEventManager.SetCopySceneTotalEnterTimesToday(teammate, AcitvityRecore.XoYo, totalEnterTimes);
				}
			}

			/// Bản đồ tương ứng
			int mapID = mapInfo.ID;
			GameMap map = GameManager.MapMgr.GetGameMap(mapID);
			/// Tạo mới phụ bản
			KTCopyScene copyScene = new KTCopyScene(map, mapInfo.Duration + XoYo.BeginWaitTime + XoYo.FinishWaitTime)
			{
				AllowReconnect = false,
				EnterPosX = mapInfo.EnterPosX,
				EnterPosY = mapInfo.EnterPosY,
				Level = nLevel,
				Name = string.Format("Tiêu Dao Cốc ({0}) - ải {1}", difficulty == 1 ? "Dễ" : difficulty == 2 ? "Trung" : "Khó", stageID),
				OutMapCode = XoYo.RegisterMap.ID,
				OutPosX = XoYo.RegisterMap.EnterPosX,
				OutPosY = XoYo.RegisterMap.EnterPosY,
				ReliveHPPercent = 100,
				ReliveMPPercent = 100,
				ReliveStaminaPercent = 100,
				ReliveMapCode = XoYo.RegisterMap.ID,
				RelivePosX = XoYo.RegisterMap.EnterPosX,
				RelivePosY = XoYo.RegisterMap.EnterPosY,
			};
			/// Bắt đầu phụ bản với Script tương ứng
			switch (mapInfo.Type)
			{
				case XoYo.XoYoEventType.KillMonster:
				{
					XoYo_Script_KillMonster script = new XoYo_Script_KillMonster(copyScene, mapInfo)
					{
						TeamID = player.TeamID,
						Difficulty = difficulty,
						StageID = stageID,
					};
					script.Begin(teammates);
					break;
				}
				case XoYo.XoYoEventType.HideTrapAndKillMonster:
				{
					XoYo_Script_HideTrapAndKillMonster script = new XoYo_Script_HideTrapAndKillMonster(copyScene, mapInfo)
					{
						TeamID = player.TeamID,
						Difficulty = difficulty,
						StageID = stageID,
					};
					script.Begin(teammates);
					break;
				}
				case XoYo.XoYoEventType.UnlockTriggerAndKillMonster:
				{
					XoYo_Script_UnlockTriggerAndKillMonster script = new XoYo_Script_UnlockTriggerAndKillMonster(copyScene, mapInfo)
					{
						TeamID = player.TeamID,
						Difficulty = difficulty,
						StageID = stageID,
					};
					script.Begin(teammates);
					break;
				}
			}
		}
	}
}
