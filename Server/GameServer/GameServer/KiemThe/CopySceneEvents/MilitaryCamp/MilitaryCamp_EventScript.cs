using GameServer.KiemThe.Logic;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Script phụ bản Quân doanh
    /// </summary>
    public static class MilitaryCamp_EventScript
    {
        /// <summary>
		/// Đã mở phụ bản Quân doanh chưa
		/// </summary>
		public const bool MilitaryCamp_Open = true;

		/// <summary>
		/// Trả về tổng số lượt đã tham gia Quân doanh trong tuần của người chơi tương ứng
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private static int GetTotalParticipatedTimesThisWeek(KPlayer player, int index)
		{
			/// Mã hóa (số đầu tiên mặc định là 1, 3 số tiếp theo là số lượt đã tham gia Hậu Sơn Phục Ngưu trong tuần, 3 số tiếp là Bách Man Sơn, 3 số cuối là Hải Lăng Vương Mộ)
			int nTimes = player.GetValueOfWeekRecore((int) WeeklyActivityRecord.MilitaryCamp_TotalParticipatedTimes);
			if (nTimes < 0)
			{
				nTimes = 1000000000;
			}

			/// Số lượt đã tham gia
			int rounds = 0;

			/// Loại phụ bản là gì
			switch (index)
			{
				/// Hậu Sơn Phục Ngưu
				case (int) MilitaryCamp.EventType.FootHills:
				{
					rounds = nTimes % 1000000000 / 1000000;
					break;
				}
				/// Bách Man Sơn
				case (int) MilitaryCamp.EventType.MountainPeak:
				{
					rounds = nTimes % 1000000 / 1000;
					break;
				}
				/// Hải Lăng Vương Mộ
				case (int) MilitaryCamp.EventType.RoyalTomb:
				{
					rounds = nTimes % 1000;
					break;
				}
			}

			return rounds;
		}

		/// <summary>
		/// Thiết lập tổng số lượt đã tham gia Quân doanh trong tuần của người chơi tương ứng
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		/// <param name="nTimes"></param>
		private static void SetTotalParticipatedTimesThisWeek(KPlayer player, int index, int nTimes)
        {
			/// Gốc
			int value = player.GetValueOfWeekRecore((int) WeeklyActivityRecord.MilitaryCamp_TotalParticipatedTimes);
			if (value < 0)
			{
				value = 1000000000;
			}

			/// Hậu Sơn Phục Ngưu
			int footHillsRounds = value % 1000000000 / 1000000;
			/// Bách Man Sơn
			int mountainPeakRounds = nTimes % 1000000 / 1000;
			/// Hải Lăng Vương Mộ
			int royalTombRounds = nTimes % 1000;

			/// Loại phụ bản là gì
			switch (index)
			{
				/// Hậu Sơn Phục Ngưu
				case (int) MilitaryCamp.EventType.FootHills:
				{
					footHillsRounds = nTimes;
					break;
				}
				/// Bách Man Sơn
				case (int) MilitaryCamp.EventType.MountainPeak:
				{
					mountainPeakRounds = nTimes;
					break;
				}
				/// Hải Lăng Vương Mộ
				case (int) MilitaryCamp.EventType.RoyalTomb:
				{
					royalTombRounds = nTimes;
					break;
				}
			}

			/// Thiết lập lại
			value = 1000000000 + footHillsRounds * 1000000 + mountainPeakRounds * 1000 + royalTombRounds;
			/// Lưu lại
			player.SetValueOfWeekRecore((int) WeeklyActivityRecord.MilitaryCamp_TotalParticipatedTimes, value);
		}

		/// <summary>
		/// Trả về phu bản theo ID
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static MilitaryCamp.EventInfo GetEvent(int index)
        {
			/// Loại phụ bản là gì
			switch (index)
			{
				/// Hậu Sơn Phục Ngưu
				case (int) MilitaryCamp.EventType.FootHills:
				{
					return MilitaryCamp.FootHills;
				}
				/// Bách Man Sơn
				case (int) MilitaryCamp.EventType.MountainPeak:
				{
					return MilitaryCamp.MountainPeak;
				}
				/// Hải Lăng Vương Mộ
				case (int) MilitaryCamp.EventType.RoyalTomb:
				{
					return MilitaryCamp.RoyalTomb;
				}
				/// Toác
				default:
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Kiểm tra có đủ điều kiện tham gia Quân doanh không
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string CheckCondition(KPlayer player, int index)
        {
			/// Nếu đóng Quân doanh
			if (!MilitaryCamp_EventScript.MilitaryCamp_Open)
			{
				return "Phụ bản Quân doanh hiện chưa mở!";
			}

			/// Danh sách thành viên
			List<KPlayer> teammates = player.Teammates;

			/// Nếu không có nhóm
			if (player.TeamID == -1 || !KTTeamManager.IsTeamExist(player.TeamID))
			{
				return "Phó bản Quân Doanh rất nguy hiểm, ngươi hãy lập tổ đội trước tiên!";
			}
			/// Nếu không phải đội trưởng
			else if (player.TeamLeader != null && player.TeamLeader != player)
			{
				return "Chỉ đội trưởng mới có thể thao tác!";
			}
			/// Nếu nhóm không đủ người
			else if (teammates.Count < MilitaryCamp.Condition.LimitMembers)
			{
				return string.Format("Nhóm cần tối thiểu <color=green>{0} thành viên</color> mới có thể tham gia phụ bản Quân doanh.", MilitaryCamp.Condition.LimitMembers);
			}
			/// Nếu cấp độ không đủ
			else if (player.m_Level < MilitaryCamp.Condition.RequireLevel)
			{
				return string.Format("Yêu cầu cấp độ tối thiểu <color=green>{0}</color> mới có thể tham gia phụ bản Quân doanh.", MilitaryCamp.Condition.RequireLevel);
			}
			/// Nếu đã tham gia quá số lượt trong tuần rồi thì thôi
			else if (MilitaryCamp_EventScript.GetTotalParticipatedTimesThisWeek(player, index) >= MilitaryCamp.Condition.RoundsPerWeek)
			{
				return string.Format("Ngươi đã tham gia đủ {0} lượt {1} trong tuần, không thể tham gia nữa!", MilitaryCamp.Condition.RoundsPerWeek, MilitaryCamp_EventScript.GetEvent(index).Name);
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
				if (teammate.m_Level < MilitaryCamp.Condition.RequireLevel)
				{
					notEnoughLevelPlayers.Add(string.Format("<color=#4dbeff>[{0}]</color>", teammate.RoleName));
				}
			}

			/// Nếu tồn tại danh sách đội viên không đủ cấp
			if (notEnoughLevelPlayers.Count > 0)
			{
				return string.Format("Trong tổ đội có {0} cấp độ không đủ <color=green>{1}</color>, không thể tham gia!", string.Join(", ", notEnoughLevelPlayers), MilitaryCamp.Condition.RequireLevel);
			}

			/// Danh sách đội viên đã tham gia quá số lượt trong ngày
			List<string> alreadyAttempMaxRoundTodayPlayers = new List<string>();
			/// Kiểm tra nhóm
			foreach (KPlayer teammate in teammates)
			{
				/// Nếu đã tham gia rồi
				if (MilitaryCamp_EventScript.GetTotalParticipatedTimesThisWeek(player, index) >= MilitaryCamp.Condition.RoundsPerWeek)
				{
					alreadyAttempMaxRoundTodayPlayers.Add(string.Format("<color=#4dbeff>[{0}]</color>", teammate.RoleName));
				}
			}

			/// Nếu tồn tại danh sách đội viên đã tham gia quá số lượt trong ngày
			if (alreadyAttempMaxRoundTodayPlayers.Count > 0)
			{
				return string.Format("Trong tổ đội có {0} đã tham gia quá {1} lượt {2} trong tuần!", string.Join(", ", alreadyAttempMaxRoundTodayPlayers), MilitaryCamp.Condition.RoundsPerWeek, MilitaryCamp_EventScript.GetEvent(index).Name);
			}

			/// Thông tin phụ bản
			MilitaryCamp.EventInfo eventInfo = MilitaryCamp_EventScript.GetEvent(index);
			/// Toác
			if (eventInfo == null)
			{
				return "Sự kiện bị lỗi. Hãy thử lại sau!";
			}

			/// Trả về kết quả có thể tham gia
			return "OK";
		}

		/// <summary>
		/// Bắt đầu Quân doanh
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		public static void Begin(KPlayer player, int index)
        {
			/// Nếu đóng Quân doanh
			if (!MilitaryCamp_EventScript.MilitaryCamp_Open)
			{
				return;
			}

			/// Nếu lỗi gì đó
			if (player == null)
			{
				return;
			}

			/// Danh sách thành viên nhóm
			List<KPlayer> teammates = player.Teammates;

			/// Cấp độ cao nhất thành viên trong nhóm
			int nLevel = teammates.Max(x => x.m_Level);

			/// Thông tin phụ bản
			MilitaryCamp.EventInfo eventInfo = MilitaryCamp_EventScript.GetEvent(index);
			/// Toác
			if (eventInfo == null)
            {
				return;
            }

			/// Duyệt danh sách thành viên
			foreach (KPlayer teammate in teammates)
            {
				/// Số lượt đã tham gia
				int nTimes = MilitaryCamp_EventScript.GetTotalParticipatedTimesThisWeek(teammate, index);
				/// Tăng số lượt lên
				nTimes++;
				/// Lưu lại
				MilitaryCamp_EventScript.SetTotalParticipatedTimesThisWeek(teammate, index, nTimes);
            }

			/// Bản đồ tương ứng
			int mapID = eventInfo.Config.MapID;
			GameMap map = GameManager.MapMgr.GetGameMap(mapID);
			/// Tạo mới phụ bản
			KTCopyScene copyScene = new KTCopyScene(map, eventInfo.Duration)
			{
				AllowReconnect = false,
				EnterPosX = eventInfo.Config.EnterPosX,
				EnterPosY = eventInfo.Config.EnterPosY,
				Level = nLevel,
				Name = eventInfo.Name,
				OutMapCode = eventInfo.Config.OutMapID,
				OutPosX = eventInfo.Config.OutPosX,
				OutPosY = eventInfo.Config.OutPosY,
				ReliveHPPercent = 100,
				ReliveMPPercent = 100,
				ReliveStaminaPercent = 100,
				ReliveMapCode = mapID,
				RelivePosX = eventInfo.Config.EnterPosX,
				RelivePosY = eventInfo.Config.EnterPosY,
			};

			/// Bắt đầu phụ bản với Script tương ứng
			MilitaryCamp_Script_Main script = new MilitaryCamp_Script_Main(copyScene, eventInfo);
			script.Begin(teammates);
		}
	}
}
