using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using HSGameEngine.GameEngine.Network.Protocol;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Network
{
	/// <summary>
	/// Quản lý tương tác với Socket
	/// </summary>
	public static partial class KT_TCPHandler
	{
		#region Tạo bang
		/// <summary>
		/// Gửi yêu cầu tạo bang
		/// </summary>
		/// <param name="guildName"></param>
		public static void SendCreateGuild(string guildName)
		{
			string strCmd = string.Format("{0}", guildName);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_CREATE)));
		}

		/// <summary>
		/// Nhận thông báo kết quả tạo bang hội
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveCreateGuild(string[] fields)
		{
			/// Kết quả trả về
			int ret = int.Parse(fields[0]);
			/// ID bang hội
			int guildID = int.Parse(fields[1]);
			/// Tên bang hội
			string guildName = fields[2];

			/// Nếu thành công
			if (ret == 0)
			{
				/// Thiết lập ID bang hội bản thân
				Global.Data.RoleData.GuildID = guildID;
				Global.Data.RoleData.GuildRank = (int) GuildRank.Master;
				/// Mở khung bang hội tổng quan
				KT_TCPHandler.SendGetGuildInfo();
			}
		}
		#endregion

		#region Lấy thông tin bang
		/// <summary>
		/// Gửi yêu cầu lấy thông tin bang hội của bản thân
		/// </summary>
		public static void SendGetGuildInfo()
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_GETINFO)));
		}

		/// <summary>
		/// Nhận thông báo thông tin bang hội
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildInfo(int cmdID, byte[] cmdData, int length)
		{
			GuildInfomation guildInfo = DataHelper.BytesToObject<GuildInfomation>(cmdData, 0, length);
			if (guildInfo == null)
			{
				KTGlobal.AddNotification("Có lỗi khi truy vấn thông tin bang hội, hãy thử lại!");
				return;
			}

			/// Mở khung bang hội tổng quan
			PlayZone.GlobalPlayZone.OpenUIGuild(guildInfo);
		}
		#endregion

		#region Danh sách thành viên
		/// <summary>
		/// Gửi yêu cầu truy vấn danh sách thành viên bang hội
		/// </summary>
		/// <param name="pageID"></param>
		public static void SendGetGuildMembers(int pageID)
		{
			string strCmd = string.Format("{0}:{1}:{2}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID, pageID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_GETMEMBERLIST)));
		}

		/// <summary>
		/// Nhận thông báo truy vấn danh sách thành viên bang hội
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildMembers(int cmdID, byte[] cmdData, int length)
		{
			GuildMemberData guildMemberData = DataHelper.BytesToObject<GuildMemberData>(cmdData, 0, length);
			if (guildMemberData == null)
			{
				KTGlobal.AddNotification("Có lỗi khi truy vấn danh sách thành viên bang hội, hãy thử lại!");
				return;
			}

			/// Nếu chưa mở khung
			if (PlayZone.GlobalPlayZone.UIGuildMemberList == null)
			{
				/// Mở khung thành viên bang hội
				PlayZone.GlobalPlayZone.OpenUIGuildMemberList(guildMemberData);
			}
			/// Nếu đã mở khung
			else
			{
				PlayZone.GlobalPlayZone.UIGuildMemberList.Data = guildMemberData;
				PlayZone.GlobalPlayZone.UIGuildMemberList.Refresh();
			}
		}
		#endregion

		#region Thay đổi chức vị
		/// <summary>
		/// Gửi yêu cầu thay đổi chức vị thành viên
		/// </summary>
		/// <param name="roleID"></param>
		/// <param name="rank"></param>
		public static void SendChangeGuildMemberRank(int roleID, int rank)
		{
			string strCmd = string.Format("{0}:{1}", roleID, rank);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK)));
		}

		/// <summary>
		/// Nhận thông báo chức vụ bản thân thay đổi
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveChangeGuildMemberRank(string[] fields)
		{
			int rank = int.Parse(fields[0]);

			/// Nếu bản thân thay đổi chức
			if (rank != -1)
			{
				Global.Data.RoleData.GuildRank = rank;
			}
			
			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildMemberList != null)
			{
				/// Đóng khung
				PlayZone.GlobalPlayZone.CloseUIGuildMemberList();

				/// Mở lại khung
				KT_TCPHandler.SendGetGuildMembers(1);
			}
		}
		#endregion

		#region Trục xuất tộc
		/// <summary>
		/// Gửi yêu càu trục xuất tộc
		/// </summary>
		/// <param name="familyID"></param>
		public static void SendGuildKickoutFamily(int familyID)
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.GuildID, familyID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_KICKFAMILY)));
		}

		/// <summary>
		/// Nhận thông báo trục xuất gia tộc
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveKickoutFamily(string[] fields)
		{
			int ret = int.Parse(fields[0]);
			
			/// Nếu là tộc của bản thân bị Kick
			if (ret == 1)
			{
				Global.Data.RoleData.GuildID = 0;
				Global.Data.RoleData.GuildRank = (int) GuildRank.Member;
			}

			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildMemberList != null)
			{
				/// Đóng khung
				PlayZone.GlobalPlayZone.CloseUIGuildMemberList();

				/// Mở lại khung
				KT_TCPHandler.SendGetGuildMembers(1);
			}
		}
		#endregion

		#region Danh sách ưu tú
		/// <summary>
		/// Gửi yêu cầu truy vấn danh sách ưu tú bang
		/// </summary>
		/// <param name="pageID"></param>
		public static void SendGetGuildExcellenceMembers(int pageID)
		{
			string strCmd = string.Format("{0}:{1}:{2}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID, pageID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_GETGIFTED)));
		}

		/// <summary>
		/// Nhận thông báo truy vấn danh sách ưu tú bang
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildExcellenceMembers(int cmdID, byte[] cmdData, int length)
		{
			GuildVoteInfo voteInfo = DataHelper.BytesToObject<GuildVoteInfo>(cmdData, 0, length);
			if (voteInfo == null)
			{
				KTGlobal.AddNotification("Có lỗi khi truy vấn danh sách thành viên ưu tú, hãy thử lại!");
				return;
			}

			/// Nếu chưa mở khung
			if (PlayZone.GlobalPlayZone.UIGuildVote == null)
			{
				/// Mở khung thành viên bang hội
				PlayZone.GlobalPlayZone.OpenUIGuildVote(voteInfo);
			}
			/// Nếu đã mở khung
			else
			{
				PlayZone.GlobalPlayZone.UIGuildVote.Data = voteInfo;
				PlayZone.GlobalPlayZone.UIGuildVote.Refresh();
			}
		}
		#endregion

		#region Danh sách quan hàm
		/// <summary>
		/// Gửi yêu cầu truy vấn thông tin Top quan hàm
		/// </summary>
		public static void SendGetGuildOfficialRankInfo()
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_OFFICE_RANK)));
		}

		/// <summary>
		/// Nhận thông báo truy vấn thông tin Top quan hàm
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildOfficialRankInfo(int cmdID, byte[] cmdData, int length)
		{
			OfficeRankInfo officialRankInfo = DataHelper.BytesToObject<OfficeRankInfo>(cmdData, 0, length);
			if (officialRankInfo == null)
			{
				KTGlobal.AddNotification("Có lỗi khi truy vấn danh sách quan hàm, hãy thử lại!");
				return;
			}

			/// Mở khung
			PlayZone.GlobalPlayZone.OpenUIGuildOfficialRank(officialRankInfo);
		}
		#endregion

		#region Bầu ưu tú
		/// <summary>
		/// Gửi yêu cầu bầu ưu tú cho thành viên
		/// </summary>
		/// <param name="memberRoleID"></param>
		public static void SendVoteExcellenceMember(int memberRoleID)
		{
			string strCmd = string.Format("{0}:{1}:{2}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID, memberRoleID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_VOTEGIFTED)));
		}

		/// <summary>
		/// Nhận thông báo bầu ưu tú thành công cho thành viên
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveVoteExcellenceMember(string[] fields)
		{
			/// Tên thành viên được bầu
			string roleName = fields[0];

			/// Nếu đang hiện khung
			if (PlayZone.GlobalPlayZone.UIGuildVote != null)
			{
				/// Gửi yêu cầu truy vấn danh sách ưu
				KT_TCPHandler.SendGetGuildExcellenceMembers(1);
			}
		}
		#endregion

		#region Cống hiến vào bang
		/// <summary>
		/// Gửi yêu cầu cống hiến bạc vào bang
		/// </summary>
		/// <param name="amount"></param>
		public static void SendDedicateMoneyToGuild(int amount)
		{
			string strCmd = string.Format("{0}:{1}:{2}", Global.Data.RoleData.RoleID, Global.Data.RoleData.GuildID, amount);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_DONATE)));
		}

		/// <summary>
		/// Nhận thông báo cống hiến bạc vào bang thành công
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveDedicateMoneyToGuild(string[] fields)
		{
			/// Số bạc bang quỹ hiện có
			int guildMoney = int.Parse(fields[0]);

			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildDedication != null)
			{
				PlayZone.GlobalPlayZone.UIGuildDedication.GuildMoney = guildMoney;
				PlayZone.GlobalPlayZone.UIGuildDedication.Refresh();
			}
		}
		#endregion

		#region Hoạt động
		#region Tranh đoạt lãnh thổ
		/// <summary>
		/// Truy vấn thông tin tranh đoạt lãnh thổ
		/// </summary>
		public static void SendGetGuildColonyDisputeInfo()
		{
			string strCmd = string.Format("{0}", Global.Data.RoleData.GuildID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_TERRITORY)));
		}

		/// <summary>
		/// Nhận thông báo thông tin tranh đoạt lãnh thổ
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildColonyDisputeInfo(int cmdID, byte[] cmdData, int length)
		{
			TerritoryInfo colonyInfo = DataHelper.BytesToObject<TerritoryInfo>(cmdData, 0, length);
			if (colonyInfo == null)
			{
				KTGlobal.HideLoadingFrame();
				KTGlobal.AddNotification("Có lỗi khi truy vấn danh sách lãnh thổ, hãy thử lại!");
				return;
			}

			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildActivity != null)
			{
				PlayZone.GlobalPlayZone.UIGuildActivity.UIColonyDispute.Data = colonyInfo;
				PlayZone.GlobalPlayZone.UIGuildActivity.UIColonyDispute.Refresh();
			}
		}

		/// <summary>
		/// Gửi yêu cầu thiết lập thành chính
		/// </summary>
		/// <param name="colonyID"></param>
		public static void SendSetGuildColonyAsMainCastle(int colonyMapID)
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.GuildID, colonyMapID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_SETCITY)));
		}

		/// <summary>
		/// Nhận thông báo thiết lập lãnh thổ thành thành chính thành công
		/// </summary>
		public static void ReceiveSetGuildColonyAsMainCastle()
		{
			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildActivity != null)
			{
				/// Truy vấn lại thông tin lãnh thổ
				KT_TCPHandler.SendGetGuildColonyDisputeInfo();
			}
		}

		/// <summary>
		/// Gửi yêu cầu thiết lập thuế lên lãnh thổ
		/// </summary>
		/// <param name="colonyMapID"></param>
		/// <param name="taxRate"></param>
		public static void SendSetGuildColonyTax(int colonyMapID, int taxRate)
		{
			string strCmd = string.Format("{0}:{1}:{2}", Global.Data.RoleData.GuildID, colonyMapID, taxRate);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_SETTAX)));
		}

		/// <summary>
		/// Nhận thông báo thiết lập thuế lên lãnh thổ thành công
		/// </summary>
		public static void ReceiveSetGuildColonyTax()
		{
			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuildActivity != null)
			{
				/// Truy vấn lại thông tin lãnh thổ
				KT_TCPHandler.SendGetGuildColonyDisputeInfo();
			}
		}
		#endregion
		#endregion

		#region Thoát khỏi bang
		/// <summary>
		/// Gửi yêu cầu thoát tộc khỏi bang hội
		/// </summary>
		public static void SendFamilyQuitGuild()
		{
			string strCmd = string.Format("{0}", Global.Data.RoleData.GuildID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_FAMILYQUIT)));
		}
		#endregion

		#region Thay đổi lợi tức
		/// <summary>
		/// Gửi yêu cầu thay đổi lợi tức bang
		/// </summary>
		/// <param name="rate"></param>
		public static void SendChangeGuildProfit(int rate)
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.GuildID, rate);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_CHANGE_MAXWITHDRAW)));
		}
		
		/// <summary>
		/// Nhận thông báo sửa lợi tức bang hội thành công
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveChangeGuildProfit(string[] fields)
		{
			int rate = int.Parse(fields[0]);
			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuild != null)
			{
				PlayZone.GlobalPlayZone.UIGuild.Data.MaxWithDraw = rate;
				PlayZone.GlobalPlayZone.UIGuild.Refresh();
			}
		}
		#endregion

		#region Thay đổi tôn chỉ
		/// <summary>
		/// Gửi yêu cầu thay đổi tôn chỉ bang
		/// </summary>
		/// <param name="slogan"></param>
		public static void SendChangeGuildSlogan(string slogan)
		{
			GuildChangeSlogan changeSlogan = new GuildChangeSlogan()
			{
				Slogan = slogan,
				GuildID = Global.Data.RoleData.GuildID,
			};
			byte[] bytes = DataHelper.ObjectToBytes<GuildChangeSlogan>(changeSlogan);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_CHANGE_NOTIFY)));
		}

		/// <summary>
		/// Nhận thông báo sửa tôn chỉ bang hội thành công
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveChangeGuildSlogan(int cmdID, byte[] cmdData, int length)
		{
			GuildChangeSlogan changeSlogan = DataHelper.BytesToObject<GuildChangeSlogan>(cmdData, 0, length);
			if (changeSlogan == null)
			{
				return;
			}

			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuild != null)
			{
				PlayZone.GlobalPlayZone.UIGuild.Data.Notify = changeSlogan.Slogan;
				PlayZone.GlobalPlayZone.UIGuild.Refresh();
			}
		}
		#endregion

		#region Danh sách cổ tức bang hội
		/// <summary>
		/// Gửi yêu cầu truy vấn danh sách cổ tức bang hội
		/// </summary>
		/// <param name="pageID"></param>
		public static void SendGetGuildShareList(int pageID)
		{
			string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.GuildID, pageID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_GETSHARE)));
		}

		/// <summary>
		/// Nhận thông báo truy vấn danh sách cổ tức bang hội
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="cmdData"></param>
		/// <param name="length"></param>
		public static void ReceiveGetGuildShareList(int cmdID, byte[] cmdData, int length)
		{
			GuildShareInfo guildShareInfo = DataHelper.BytesToObject<GuildShareInfo>(cmdData, 0, length);
			if (guildShareInfo == null)
			{
				KTGlobal.AddNotification("Có lỗi khi truy vấn danh sách cổ tức bang hội, hãy thử lại!");
				return;
			}

			/// Nếu chưa mở khung
			if (PlayZone.GlobalPlayZone.UIGuildShare == null)
			{
				/// Mở khung thành viên bang hội
				PlayZone.GlobalPlayZone.OpenUIGuildShare(guildShareInfo);
			}
			/// Nếu đã mở khung
			else
			{
				PlayZone.GlobalPlayZone.UIGuildShare.Data = guildShareInfo;
				PlayZone.GlobalPlayZone.UIGuildShare.Refresh();
			}
		}
		#endregion

		#region Xin vào bang
		/// <summary>
		/// Gửi yêu cầu xin vào bang hội
		/// </summary>
		/// <param name="guildID"></param>
		/// <param name="roleID"></param>
		public static void SendAskToJoinGuild(int guildID, int roleID)
		{
			string strCmd = string.Format("{0}:{1}", guildID, roleID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_ASKJOIN)));
		}

		/// <summary>
		/// Nhận yêu cầu xin vào bang hội
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveAskToJoinGuild(string[] fields)
		{
			/// Nếu bản thân chưa có bang
			if (Global.Data.RoleData.GuildID <= 0)
			{
				return;
			}
			/// Nếu bản thân không phải bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master)
			{
				return;
			}

			/// ID người chơi xin vào
			int partnerRoleID = int.Parse(fields[0]);
			/// Tên người chơi xin vào
			string partnerRoleName = fields[1];
			/// ID tộc
			int partnerFamilyID = int.Parse(fields[2]);
			/// Tên tộc
			string partnerFamilyName = fields[3];

			/// Mở bảng thông báo
			KTGlobal.ShowMessageBox("Thông báo", string.Format("<color=#24c1ff>[{0}]</color>, tộc trưởng của gia tộc <color=yellow>[{1}]</color> xin vào bang hội của bạn, đồng ý không?", partnerRoleName, partnerFamilyName), () => {
				KT_TCPHandler.SendResponseAskToJoinGuildRequest(1, partnerRoleID, partnerFamilyID);
			}, () => {
				KT_TCPHandler.SendResponseAskToJoinGuildRequest(0, partnerRoleID, partnerFamilyID);
			});
		}

		/// <summary>
		/// Gửi yêu cầu trả lời đơn xin vào bang
		/// </summary>
		/// <param name="response"></param>
		/// <param name="partnerRoleID"></param>
		/// <param name="partnerFamilyID"></param>
		public static void SendResponseAskToJoinGuildRequest(int response, int partnerRoleID, int partnerFamilyID)
		{
			string strCmd = string.Format("{0}:{1}:{2}:{3}", response, partnerRoleID, partnerFamilyID, Global.Data.RoleData.GuildID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_RESPONSEASK)));
		}
		#endregion

		#region Mời vào bang
		/// <summary>
		/// Gửi lời mời vào bang tới người chơi tương ứng
		/// </summary>
		/// <param name="partnerRoleID"></param>
		public static void SendInviteToGuild(int partnerRoleID)
		{
			string strCmd = string.Format("{0}", partnerRoleID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_INVITE)));
		}

		/// <summary>
		/// Nhận thông báo lời mời vào bang
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveInviteToGuild(string[] fields)
		{
			/// Nếu bản thân đã có bang
			if (Global.Data.RoleData.GuildID > 0)
			{
				return;
			}
			/// Nếu bản thân không phải tộc trưởng
			else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master)
			{
				return;
			}

			/// ID người mời
			int inviterRoleID = int.Parse(fields[0]);
			/// Tên người mời
			string inviterRoleName = fields[1];
			/// ID bang
			int inviterGuildID = int.Parse(fields[2]);
			/// Tên bang
			string inviterGuildName = fields[3];

			/// Mở bảng thông báo
			KTGlobal.ShowMessageBox("Thông báo", string.Format("<color=#24c1ff>[{0}]</color>, bang chủ của bang <color=yellow>[{1}]</color> chiêu mộ gia tộc của bạn, đồng ý không?", inviterRoleName, inviterGuildName), () => {
				KT_TCPHandler.SendResponseInviteToGuild(inviterRoleID, inviterGuildID, 1);
			}, () => {
				KT_TCPHandler.SendResponseInviteToGuild(inviterRoleID, inviterGuildID, 0);
			});
		}

		/// <summary>
		/// Gửi phản hồi lời mời vào bang
		/// </summary>
		/// <param name="inviterRoleID"></param>
		/// <param name="inviterGuildID"></param>
		/// <param name="response"></param>
		public static void SendResponseInviteToGuild(int inviterRoleID, int inviterGuildID, int response)
		{
			string strCmd = string.Format("{0}:{1}:{2}:{3}:{4}", response, inviterRoleID, inviterGuildID, Global.Data.RoleData.FamilyID, Global.Data.RoleData.RoleID);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_RESPONSEINVITE)));
		}
		#endregion

		#region Rút tài sản
		/// <summary>
		/// Gửi yêu cầu rút tài sản của bản thân ở bang hội
		/// </summary>
		/// <param name="amount"></param>
		public static void SendWithdrawSelfGuildMoney(int amount)
		{
			string strCmd = string.Format("{0}", amount);
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILD_DOWTIHDRAW)));
		}

		/// <summary>
		/// Nhận thông báo rút thành công tài sản cá nhân ở bang hội
		/// </summary>
		/// <param name="fields"></param>
		public static void ReceiveWithdrawSelfGuildMoney(string[] fields)
		{
			int selfMoneyLeft = int.Parse(fields[0]);
			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UIGuild != null)
			{
				PlayZone.GlobalPlayZone.UIGuild.Data.RoleGuildMoney = selfMoneyLeft;
				PlayZone.GlobalPlayZone.UIGuild.Refresh();
			}
		}
		#endregion

		#region Truy vấn thông tin danh sách lãnh thổ của các bang hội
		/// <summary>
		/// Gửi yêu cầu truy vấn thông tin danh sách lãnh thổ của các bang hội
		/// </summary>
		public static void SendGetGuildsColonyMaps()
		{
			string strCmd = "";
			byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
			GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GETTERRORY_DATA)));
		}

		/// <summary>
		/// Nhận thông tin danh sách lãnh thổ của các bang hội
		/// </summary>
		/// <param name="cmdID"></param>
		/// <param name="bytesData"></param>
		/// <param name="length"></param>
		public static void ReceiveGuildsColonyMaps(int cmdID, byte[] bytesData, int length)
		{
			KTGlobal.HideLoadingFrame();

			List<GuildWarMiniMap> territoryData = DataHelper.BytesToObject<List<GuildWarMiniMap>>(bytesData, 0, length);
			if (territoryData == null)
			{
				return;
			}

			/// Nếu đang mở khung
			if (PlayZone.GlobalPlayZone.UILocalMap != null)
			{
				PlayZone.GlobalPlayZone.UILocalMap.RefreshColonyMap(territoryData);
			}
		}
		#endregion
	}
}
