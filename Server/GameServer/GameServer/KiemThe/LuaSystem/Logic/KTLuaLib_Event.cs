using GameServer.KiemThe.CopySceneEvents.Family;
using GameServer.KiemThe.CopySceneEvents.MiJingFuBen;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.CopySceneEvents.XiaoYaoGu;
using GameServer.KiemThe.CopySceneEvents.YouLongGe;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameEvents.BaiHuTang;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.LuaSystem.Entities;
using GameServer.KiemThe.LuaSystem.Entities.Builder;
using GameServer.Logic;
using GameServer.Server;
using MoonSharp.Interpreter;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using GameServer.KiemThe.GameEvents.EmperorTomb;
using GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen;

namespace GameServer.KiemThe.LuaSystem.Logic
{
	/// <summary>
	/// Cung cấp thư viện dùng cho Lua, liên quan đến sự kiện, hoạt động
	/// </summary>
	[MoonSharpUserData]
	public static class KTLuaLib_Event
	{
		#region Quái và NPC
		/// <summary>
		/// Tạo Monster ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_MonsterBuilder CreateMonsterBuilder(Lua_Scene scene)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
			{
				LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on EventManager.CreateMonsterBuilder, scene is not exist!"));
				return null;
			}

			/// Trả về đối tượng tương ứng
			return new Lua_MonsterBuilder()
			{
				Scene = scene,
			};
		}

		/// <summary>
		/// Xóa quái tương ứng
		/// </summary>
		/// <param name="monster"></param>
		public static bool DeleteMonster(Lua_Monster monster)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(monster.RefObject.CurrentMapCode, out GameMap gameMap))
			{
				return false;
			}
			monster.RefObject.MonsterZoneNode?.DestroyMonster(monster.RefObject);
			return true;
		}

		/// <summary>
		/// Tạo Monster ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_NPCBuilder CreateNPCBuilder(Lua_Scene scene)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
			{
				LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on EventManager.CreateNPCBuilder, scene is not exist!"));
				return null;
			}

			/// Trả về đối tượng tương ứng
			return new Lua_NPCBuilder()
			{
				Scene = scene,
			};
		}

		/// <summary>
		/// Xóa NPC tương ứng
		/// </summary>
		/// <param name="npc"></param>
		public static bool DeleteNPC(Lua_NPC npc)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(npc.RefObject.MapCode, out GameMap gameMap))
			{
				return false;
			}
			NPCGeneralManager.RemoveMapNpc(npc.RefObject.MapCode, npc.RefObject.CopyMapID, npc.RefObject.NPCID);
			return true;
		}
		#endregion

		#region Điểm thu thập và khu vực động
		/// <summary>
		/// Tạo điểm thu thập ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_GrowPointBuilder CreateGrowPointBuilder(Lua_Scene scene)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
			{
				LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on EventManager.CreateGrowPointBuilder, scene is not exist!"));
				return null;
			}

			/// Trả về đối tượng tương ứng
			return new Lua_GrowPointBuilder()
			{
				Scene = scene,
			};
		}

		/// <summary>
		/// Xóa điểm thu thập tương ứng
		/// </summary>
		/// <param name="growPoint"></param>
		public static bool DeleteGrowPoint(Lua_GrowPoint growPoint)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(growPoint.RefObject.CurrentMapCode, out GameMap gameMap))
			{
				return false;
			}
			KTGrowPointManager.Remove(growPoint.RefObject);
			return true;
		}

		/// <summary>
		/// Tạo khu vực động ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_DynamicAreaBuilder CreateDynamicAreaBuilder(Lua_Scene scene)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
			{
				LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on EventManager.CreateDynamicAreaBuilder, scene is not exist!"));
				return null;
			}

			/// Trả về đối tượng tương ứng
			return new Lua_DynamicAreaBuilder()
			{
				Scene = scene,
			};
		}

		/// <summary>
		/// Xóa khu vực động tương ứng
		/// </summary>
		/// <param name="dynArea"></param>
		public static bool DeleteDynamicArea(Lua_DynamicArea dynArea)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(dynArea.RefObject.CurrentMapCode, out GameMap gameMap))
			{
				return false;
			}
			KTDynamicAreaManager.Remove(dynArea.RefObject);
			return true;
		}

		/// <summary>
		/// Tạo BOT ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_BotBuilder CreateBotBuilder(Lua_Scene scene)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
			{
				LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on EventManager.CreateBotBuilder, scene is not exist!"));
				return null;
			}

			/// Trả về đối tượng tương ứng
			return new Lua_BotBuilder()
			{
				Scene = scene,
			};
		}

		/// <summary>
		/// Xóa BOT tương ứng
		/// </summary>
		/// <param name="dynArea"></param>
		public static bool DeleteBot(Lua_Bot bot)
		{
			/// Nếu bản đồ không tồn tại
			if (!GameManager.MapMgr.DictMaps.TryGetValue(bot.RefObject.CurrentMapCode, out GameMap gameMap))
			{
				return false;
			}
			KTBotManager.Remove(bot.RefObject);
			return true;
		}
		#endregion

		#region Bản đồ
		/// <summary>
		/// Tìm bản đồ có ID tương ứng
		/// </summary>
		/// <param name="sceneID"></param>
		/// <returns></returns>
		public static Lua_Scene GetScene(int sceneID)
		{
			GameMap gameMap = GameManager.MapMgr.GetGameMap(sceneID);
			if (gameMap == null)
			{
				return null;
			}
			return new Lua_Scene()
			{
				RefObject = gameMap,
			};
		}
		#endregion

		#region Phụ bản
		/// <summary>
		/// Gửi yêu cầu mở khung thông tin sự kiện ở góc trái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="eventID"></param>
		public static void OpenEventBroadboard(Lua_Player player, int eventID)
		{
			G2C_EventState state = new G2C_EventState();
			state.EventID = eventID;
			state.State = 1;
			player.RefObject.SendPacket<G2C_EventState>((int) TCPGameServerCmds.CMD_KT_EVENT_STATE, state);
		}

		/// <summary>
		/// Gửi yêu cầu đóng khung thông tin sự kiện ở góc trái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="eventID"></param>
		public static void CloseEventBroadboard(Lua_Player player, int eventID)
		{
			G2C_EventState state = new G2C_EventState();
			state.EventID = eventID;
			state.State = 0;
			player.RefObject.SendPacket<G2C_EventState>((int) TCPGameServerCmds.CMD_KT_EVENT_STATE, state);
		}

		/// <summary>
		/// Cập nhật thông tin sự kiện vòa khung ở góc trái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="name"></param>
		/// <param name="eventTimeMilis"></param>
		/// <param name="contents"></param>
		public static void UpdateEventDetails(Lua_Player player, string name, long eventTimeMilis, params string[] contents)
		{
			G2C_EventNotification eventNotification = new G2C_EventNotification();
			eventNotification.EventName = name;
			int eventTimeSec = (int) (eventTimeMilis / 1000);
			if (eventTimeSec > 0)
			{
				eventNotification.ShortDetail = string.Format("TIME|{0}", eventTimeSec);
			}
			else
			{
				eventNotification.ShortDetail = "Đã kết thúc!";
			}
			eventNotification.TotalInfo = new List<string>();
			eventNotification.TotalInfo.AddRange(contents);
			player.RefObject.SendPacket<G2C_EventNotification>((int) TCPGameServerCmds.CMD_KT_EVENT_NOTIFICATION, eventNotification);
		}

		/// <summary>
		/// Cập nhật thông tin sự kiện vòa khung ở góc trái
		/// </summary>
		/// <param name="player"></param>
		/// <param name="name"></param>
		/// <param name="eventShortDesc"></param>
		/// <param name="contents"></param>
		public static void UpdateEventDetails(Lua_Player player, string name, string eventShortDesc, params string[] contents)
		{
			G2C_EventNotification eventNotification = new G2C_EventNotification();
			eventNotification.EventName = name;
			eventNotification.ShortDetail = eventShortDesc;
			eventNotification.TotalInfo = new List<string>();
			eventNotification.TotalInfo.AddRange(contents);
			player.RefObject.SendPacket<G2C_EventNotification>((int) TCPGameServerCmds.CMD_KT_EVENT_NOTIFICATION, eventNotification);
		}
		#endregion

		#region Drop
		/// <summary>
		/// Tạo vật phẩm rơi ở bản đồ tương ứng
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static Lua_ItemDropBuilder CreateDropToMapBuilder(Lua_Scene scene)
		{
			/// Trả về đối tượng tương ứng
			return new Lua_ItemDropBuilder()
			{
				Scene = scene,
			};
		}
		#endregion

		#region Sự kiện đặc biệt
		#region Bạch Hổ Đường
		/// <summary>
		/// Có phải thời gian báo danh Bạch Hổ Đường không
		/// </summary>
		/// <returns></returns>
		public static bool IsBaiHuTangRegisterTime()
		{
			return BaiHuTang.BeginRegistered;
		}

		/// <summary>
		/// Kiểm tra người chơi đã tham gia Bạch Hổ Đường hôm nay chưa
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool BaiHuTang_HasCompletedToday(Lua_Player player)
		{
			return BaiHuTang_ActivityScript.BaiHuTang_HasCompletedToday(player.RefObject);
		}

		/// <summary>
		/// Thiết lập đánh dấu đã tham gia Bạch Hổ Đường ngày hôm nay
		/// </summary>
		/// <param name="player"></param>
		public static void BaiHuTang_SetEnteredToday(Lua_Player player)
		{
			BaiHuTang_ActivityScript.BaiHuTang_SetEnteredToday(player.RefObject);
		}
		#endregion

		#region Tiêu Dao Cốc
		/// <summary>
		/// Kiểm tra điều kiện vào Tiêu Dao Cốc
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string XoYo_CheckCondition(Lua_Player player)
		{
			return XoYo_EventScript.CheckCondition(player.RefObject);
		}

		/// <summary>
		/// Bắt đầu Tiêu Dao Cốc
		/// </summary>
		/// <param name="player"></param>
		/// <param name="difficulty"></param>
		public static void XoYo_Begin(Lua_Player player, int difficulty)
		{
			XoYo_EventScript.Begin(player.RefObject, 1, difficulty);
		}

		/// <summary>
		/// Trả về điểm tích lũy của người chơi trong tháng
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int XoYo_GetCurrentMonthStoragePoint(Lua_Player player)
		{
			return XoYo_EventScript.GetCurrentMonthStoragePoint(player.RefObject);
		}

		/// <summary>
		/// Thực hiện nhận thưởng Tiêu Dao Cốc tháng vừa rồi
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string XoYo_GetLastMonthAward(Lua_Player player)
		{
			return XoYo_EventScript.GetLastMonthAward(player.RefObject);
		}

		/// <summary>
		/// Trả về thứ hạng Tiêu Dao Cốc của bản thân trong tháng này
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int XoYo_GetCurrentMonthRank(Lua_Player player)
		{
			return XoYo_EventScript.GetCurrentMonthStorageRank(player.RefObject);
		}
		#endregion

		#region Bí cảnh
		/// <summary>
		/// Kiểm tra điều kiện vào Bí Cảnh
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string MiJing_CheckCondition(Lua_Player player)
		{
			return MiJing_EventScript.CheckCondition(player.RefObject);
		}

		/// <summary>
		/// Bắt đầu bí cảnh
		/// </summary>
		/// <param name="player"></param>
		public static void MiJing_Begin(Lua_Player player)
		{
			MiJing_EventScript.Begin(player.RefObject);
		}
		#endregion

		#region Du Long Các
		/// <summary>
		/// Kiểm tra điều kiện vào Du Long Các
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string YouLong_CheckCondition(Lua_Player player)
		{
			return YouLong_EventScript.CheckCondition(player.RefObject);
		}

		/// <summary>
		/// Bắt đầu Du Long Các
		/// </summary>
		/// <param name="player"></param>
		public static void YouLong_Begin(Lua_Player player)
		{
			YouLong_EventScript.Begin(player.RefObject);
		}
		#endregion

		#region Vượt ải gia tộc
		/// <summary>
		/// Trả về danh sách thành viên tộc có thể tham gia vượt ải
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string FamilyFuBen_GetNearByFamilyMembersToJoinEventDescription(Lua_Player player)
		{
			return FamilyFuBen_EventScript.GetNearByFamilyMembersToJoinEventDescription(player.RefObject);
		}

		/// <summary>
		/// Kiểm tra điều kiện tham gia Vượt ải
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string FamilyFuBen_CheckCondition(Lua_Player player)
		{
			return FamilyFuBen_EventScript.CheckCondition(player.RefObject);
		}

		/// <summary>
		/// Bắt đầu Vượt ải gia tộc
		/// </summary>
		/// <param name="player"></param>
		public static void FamilyFuBen_Begin(Lua_Player player)
		{
			FamilyFuBen_EventScript.Begin(player.RefObject);
		}

		/// <summary>
		/// Kiểm tra điều kiện sử dụng Câu Hồn Ngọc
		/// </summary>
		/// <param name="player"></param>
		public static string FamilyFuBen_UseCallBossItem_CheckCondition(Lua_Player player)
		{
			return FamilyFuBen_EventScript.UseCallBossItem_CheckCondition(player.RefObject);
		}

		/// <summary>
		/// Kết thúc sử dụng Câu Hồn Ngọc
		/// </summary>
		/// <param name="player"></param>
		/// <param name="bossID"></param>
		public static void FamilyFuBen_FinishUsingCallBossItem(Lua_Player player, int bossID)
		{
			FamilyFuBen_EventScript.UseCallBossItem(player.RefObject, bossID);
		}
        #endregion

        #region Võ lâm liên đấu
		/// <summary>
		/// Trả về loại Võ lâm liên đấu trong tháng này
		/// </summary>
		/// <returns></returns>
		public static string TeamBattle_GetCurrentMonthTeamBattleType()
        {
			return TeamBattle_ActivityScript.GetCurrentMonthTeamBattleType();
        }

		/// <summary>
		/// Kiểm tra có phải thời gian diễn ra Võ lâm liên đấu không
		/// </summary>
		/// <returns></returns>
		public static bool TeamBattle_IsRegisterTime()
        {
			return TeamBattle_ActivityScript.IsRegisterTime();
        }

		/// <summary>
		/// Kiểm tra người chơi đã có chiến đội đăng ký tham gia Võ lâm liên đấu chưa
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool TeamBattle_IsRegistered(Lua_Player player)
        {
			return TeamBattle_ActivityScript.GetTeamInfo(player.RefObject) != null;
        }

		/// <summary>
		/// Trả về thông tin chiến đội bản thân
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string TeamBattle_GetMyTeamInfo(Lua_Player player)
        {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("Thông tin chiến đội bản thân:");
			TeamBattle.TeamBattleInfo teamInfo = TeamBattle_ActivityScript.GetTeamInfo(player.RefObject);
			/// Nếu tồn tại
			if (teamInfo != null)
            {
				/// Tên nhóm
				builder.AppendLine(string.Format("  - Chiến đội: <color=yellow><b>{0}</b></color>", teamInfo.Name));
				/// Danh sách thành viên
				builder.AppendLine(string.Format("  - Thành viên: <color=#52d4ff>{0}</color>", string.Join("<color=white>,</color> ", teamInfo.Members.Values)));
				/// Thời gian thành lập
				builder.AppendLine(string.Format("  - Thời gian thành lập: <color=green>{0}</color>", teamInfo.RegisterTime.ToString("HH:mm - dd/MM/yyyy")));
				/// Tổng số trận đấu đã tham dự
				builder.AppendLine(string.Format("  - Tổng số trận đã đấu: <color=#ff61c2>{0} trận</color>", teamInfo.TotalBattles));
				/// Tổng điểm đạt được
				builder.AppendLine(string.Format("  - Tổng điểm: <color=#ffb833>{0} điểm</color>", teamInfo.Point));
				/// Bậc thi đấu của chiến đội
				builder.AppendLine(string.Format("  - Bậc thi đấu: <color=#0afffb>{0}</color>", teamInfo.Stage));
				/// Thời gian thắng trận lần cuối
				if (teamInfo.LastWinTime != DateTime.MinValue)
                {
					builder.AppendLine(string.Format("  - Thắng trận cuối: <color=#0afffb>{0}</color>", teamInfo.LastWinTime.ToString("HH:mm - dd/MM/yyyy")));
				}
                else
                {
					builder.AppendLine(string.Format("  - Thắng trận cuối: <color=#0afffb>{0}</color>", "Chưa có"));
				}
				/// Nếu chưa cập nhật xếp hạng
				if (teamInfo.Rank == 0 || teamInfo.LastUpdateRankTime == DateTime.MinValue)
                {
					/// Xếp hạng
					builder.AppendLine(string.Format("  - Xếp hạng: <color=#c247ff>{0}</color>", "Chưa cập nhật"));
				}
                else
                {
					/// Xếp hạng
					builder.AppendLine(string.Format("  - Xếp hạng: <color=#c247ff>{0}</color>", teamInfo.Rank));
					/// Thời gian cập nhật xếp hạng
					builder.AppendLine(string.Format("  - Thời gian cập nhật: <color=#ffa31a>{0}</color>", teamInfo.LastUpdateRankTime.ToString("HH:mm - dd/MM/yyyy")));
				}
            }
			/// Chưa tồn tại
            else
            {
				builder.AppendLine("Chưa có thông tin.");
            }
			return builder.ToString();
        }

		/// <summary>
		/// Tạo nhóm đăng ký tham gia Võ lâm liên đấu
		/// </summary>
		/// <param name="player"></param>
		/// <param name="teamName"></param>
		/// <returns></returns>
		public static string TeamBattle_CreateTeam(Lua_Player player, string teamName)
        {
			return TeamBattle_ActivityScript.CreateTeam(player.RefObject, teamName);
        }

		/// <summary>
		/// Kiểm tra hôm nay có diễn ra Võ lâm liên đấu không
		/// </summary>
		/// <returns></returns>
		public static bool TeamBattle_IsBattleTimeToday()
        {
			return TeamBattle_ActivityScript.IsBattleTimeToday();
		}

		/// <summary>
		/// Chuyển người chơi đến bản đồ hội trường Võ lâm liên đấu
		/// </summary>
		/// <param name="player"></param>
		public static void TeamBattle_MoveToBattleHall(Lua_Player player)
        {
			TeamBattle_ActivityScript.MoveToBattleHall(player.RefObject);
        }

		/// <summary>
		/// Trả về bậc của trận đấu kế tiếp
		/// </summary>
		/// <returns></returns>
		public static int TeamBattle_GetNextBattleStage()
        {
			return TeamBattle_ActivityScript.GetNextBattleStage();
        }

		/// <summary>
		/// Trả về thời gian diễn ra Võ lâm liên đấu gần nhất tính từ hiện tại
		/// </summary>
		/// <returns></returns>
		public static string TeamBattle_GetNextBattleTime()
        {
			/// Mốc thời gian gần nhất
			DateTime? eventTime = TeamBattle_ActivityScript.GetNextBattleTime();
            /// Nếu không có kết quả
            if (eventTime == null)
            {
                return "FAILED";
            }
			/// Trả về kết quả
			return eventTime.Value.ToString("HH:mm - dd/MM/yyyy");
        }

		/// <summary>
		/// Đăng ký tham chiến trận đấu tiếp theo Võ lâm liên đấu
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string TeamBattle_RegisterForNextBattle(Lua_Player player)
        {
			return TeamBattle_ActivityScript.RegisterForBattle(player.RefObject);
        }

		/// <summary>
		/// Kiểm tra chiến đội đã đăng ký trận đấu tiếp theo Võ lâm liên đấu chưa
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool TeamBattle_IsRegisteredForNextBattle(Lua_Player player)
        {
			return TeamBattle_ActivityScript.IsRegisteredForBattle(player.RefObject);
        }

		/// <summary>
		/// Trả về tổng số chiến đội trong Võ lâm liên đấu đã báo danh trận đấu kế tiếp
		/// </summary>
		/// <returns></returns>
		public static int TeamBattle_GetTotalRegisteredForNextBattleTeams()
        {
			return TeamBattle_ActivityScript.GetTotalRegisteredTeams();
		}

		/// <summary>
		/// Kiểm tra chiến đội bản thân trong Võ lâm liên đấu có phần thưởng để nhận không
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool TeamBattle_IsHavingAwards(Lua_Player player)
        {
			return TeamBattle_ActivityScript.IsHavingAwards(player.RefObject, out _);
        }

		/// <summary>
		/// Nhận phần thưởng chiến đội bản thân trong Võ lâm liên đấu
		/// </summary>
		/// <param name="player"></param>
		public static string TeamBattle_GetAwards(Lua_Player player)
        {
			return TeamBattle_ActivityScript.GetAwards(player.RefObject);
        }

		/// <summary>
		/// Truy vấn thông tin Top chiến đội trong Võ lâm liên đấu
		/// </summary>
		/// <param name="player"></param>
		public static void TeamBattle_QueryTopTeam(Lua_Player player)
        {
			/// Nếu thao tác quá nhanh
			if (KTGlobal.GetCurrentTimeMilis() - player.RefObject.LastQueryTeamBattleTicks < 1000)
            {
				PlayerManager.ShowNotification(player.RefObject, "Thao tác quá nhanh, hãy thử lại sau giây lát!");
				return;
            }
			/// Đánh dấu thời điểm cập nhật
			player.RefObject.LastQueryTeamBattleTicks = KTGlobal.GetCurrentTimeMilis();

			/// Kết quả
			List<TeamBattle.TeamBattleInfo> teamBattles = TeamBattle_ActivityScript.GetTopTeams();
			/// Toác
			if (teamBattles == null)
            {
				PlayerManager.ShowNotification(player.RefObject, "Bảng xếp hạng chưa được cập nhật!");
				return;
			}

			/// Tạo gói tin gửi về Client
			player.RefObject.SendPacket<List<TeamBattle.TeamBattleInfo>>((int) TCPGameServerCmds.CMD_DB_TEAMBATTLE, teamBattles);
        }
        #endregion

        #region Tần Lăng
		/// <summary>
		/// Kiểm tra điều kiện tiến vào Tần lăng
		/// </summary>
		/// <returns></returns>
		public static string EmperorTomb_EnterMap_CheckCondition(Lua_Player player)
        {
			return EmperorTomb_ActivityScript.EnterMap_CheckCondition(player.RefObject);
        }

		/// <summary>
		/// Dịch chuyển người chơi vào Tần Lăng
		/// </summary>
		/// <param name="player"></param>
		public static void EmperorTomb_MoveToMap(Lua_Player player)
        {
			EmperorTomb_ActivityScript.MoveToEmperorTomb(player.RefObject);
        }
		#endregion

		#region Quân doanh
		/// <summary>
		/// Kiểm tra điều kiện tham gia Quân doanh
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string MilitaryCamp_CheckCondition(Lua_Player player, int index)
		{
			return MilitaryCamp_EventScript.CheckCondition(player.RefObject, index);
		}

		/// <summary>
		/// Bắt đầu Quân doanh
		/// </summary>
		/// <param name="player"></param>
		/// <param name="index"></param>
		public static void MilitaryCamp_Begin(Lua_Player player, int index)
		{
			MilitaryCamp_EventScript.Begin(player.RefObject, index);
		}
		#endregion
		#endregion
	}
}
