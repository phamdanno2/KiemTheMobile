using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.Guild;
using FSPlay.KiemVu.UI.Main.Guild.GuidDedication;
using FSPlay.KiemVu.UI.Main.Guild.GuildActivity;
using FSPlay.KiemVu.UI.Main.Guild.GuildMemberList;
using FSPlay.KiemVu.UI.Main.Guild.GuildOfficialRank;
using FSPlay.KiemVu.UI.Main.Guild.GuildShare;
using FSPlay.KiemVu.UI.Main.Guild.GuildVote;
using Server.Data;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Bang hội
    #region Tạo bang
    /// <summary>
    /// Khung tạo bang
    /// </summary>
    public UICreateGuild UICreateGuild { get; protected set; }

    /// <summary>
    /// Mở khung tạo bang
    /// </summary>
    public void OpenUICreateGuild()
    {
        if (this.UICreateGuild != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UICreateGuild = canvas.LoadUIPrefab<UICreateGuild>("MainGame/Guild/UICreateGuild");
        canvas.AddUI(this.UICreateGuild);

        this.UICreateGuild.Close = this.CloseUICreateGuild;
        this.UICreateGuild.CreateGuild = (guildName) => {
            /// Gửi yêu cầu tạo bang
            KT_TCPHandler.SendCreateGuild(guildName);
            /// Đóng khung
            this.CloseUICreateGuild();
        };
    }

    /// <summary>
    /// Đóng khung tạo bang
    /// </summary>
    public void CloseUICreateGuild()
    {
        if (this.UICreateGuild != null)
        {
            GameObject.Destroy(this.UICreateGuild.gameObject);
            this.UICreateGuild = null;
        }
    }
    #endregion

    #region Bang hội tổng quan
    /// <summary>
    /// Khung thông tin bang hội tổng quan
    /// </summary>
    public UIGuild UIGuild { get; protected set; }

    /// <summary>
    /// Mở khung thông tin bang hội tổng quan
    /// </summary>
    /// <param name="guildInfo"></param>
    public void OpenUIGuild(GuildInfomation guildInfo)
    {
        if (this.UIGuild != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuild = canvas.LoadUIPrefab<UIGuild>("MainGame/Guild/UIGuild");
        canvas.AddUI(this.UIGuild);

        this.UIGuild.Data = guildInfo;
        this.UIGuild.Close = this.CloseUIGuild;
        this.UIGuild.OpenGuildMemberList = () => {
            /// Gửi yêu cầu truy vấn thông tin thành viên bang
            KT_TCPHandler.SendGetGuildMembers(1);
        };
        this.UIGuild.OpenGuildDedication = () => {
            /// Mở khung cống hiến bang hội
            this.OpenUIGuildDedication(guildInfo.MoneyStore);
        };
        this.UIGuild.OpenGuildActivity = () => {
            /// Mở khung hoạt động bang hội
            this.OpenUIGuildActivity();
        };
        this.UIGuild.OpenGuildOpenGuildShare = () => {
            /// Gửi yêu cầu truy vấn thông tin cổ tức bang hội
            KT_TCPHandler.SendGetGuildShareList(1);
        };
        this.UIGuild.OpenGuildExcellenceVote = () => {
            /// Gửi yêu cầu truy vấn danh sách ưu
            KT_TCPHandler.SendGetGuildExcellenceMembers(1);
        };
        this.UIGuild.LeaveGuild = () => {
            /// Gửi yêu cầu thoát gia tộc khỏi bang
            KT_TCPHandler.SendFamilyQuitGuild();
        };
        this.UIGuild.ChangeSlogan = (slogan) => {
            /// Gửi yêu cầu thay đổi tôn chỉ bang hội
            KT_TCPHandler.SendChangeGuildSlogan(slogan);
        };
        this.UIGuild.SetProfit = (rate) => {
            /// Gửi yêu cầu thiết lập lợi tức bang hội
            KT_TCPHandler.SendChangeGuildProfit(rate);
        };
        this.UIGuild.WithdrawSelfMoney = (amount) => {
            /// Gửi yêu cầu rút tài sản cá nhân
            KT_TCPHandler.SendWithdrawSelfGuildMoney(amount);
        };
    }

    /// <summary>
    /// Đóng khung thông tin bang hội tổng quan
    /// </summary>
    public void CloseUIGuild()
    {
        if (this.UIGuild != null)
        {
            GameObject.Destroy(this.UIGuild.gameObject);
            this.UIGuild = null;
        }
    }
    #endregion

    #region Danh sách thành viên bang hội
    /// <summary>
    /// Khung danh sách thành viên bang hội
    /// </summary>
    public UIGuildMemberList UIGuildMemberList { get; protected set; }

    /// <summary>
    /// Mở khung danh sách gia tộc và thành viên bang hội
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIGuildMemberList(GuildMemberData data)
    {
        if (this.UIGuildMemberList != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildMemberList = canvas.LoadUIPrefab<UIGuildMemberList>("MainGame/Guild/UIGuildMemberList");
        canvas.AddUI(this.UIGuildMemberList);

        this.UIGuildMemberList.Data = data;
        this.UIGuildMemberList.Close = () => {
            /// Đóng khung
            this.CloseUIGuildMemberList();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
        this.UIGuildMemberList.KickOutFamily = (familyID) => {
            /// Gửi yêu cầu trục xuất tộc
            KT_TCPHandler.SendGuildKickoutFamily(familyID);
        };
        this.UIGuildMemberList.BrowseInfo = (roleID) => {
            /// Gửi gói tin yêu cầu kiểm tra thông tin người chơi
            KT_TCPHandler.SendCheckPlayerInfo(roleID);
        };
        this.UIGuildMemberList.PrivateChat = (roleName) => {
            if (roleName == Global.Data.RoleData.RoleName)
            {
                KTGlobal.AddNotification("Không thể tự Chat với chính mình!");
                return;
            }
            /// Thực hiện Chat mật
            KTGlobal.OpenPrivateChatBoxWith(roleName);
        };
        this.UIGuildMemberList.AddFriend = (roleID) => {
            /// Gửi yêu cầu kết bạn
            GameInstance.Game.SpriteAskFriend(roleID);
        };
        this.UIGuildMemberList.Approve = (roleID, rank) => {
            /// Gửi yêu cầu bổ nhiệm thành viên
            KT_TCPHandler.SendChangeGuildMemberRank(roleID, rank);
        };
        this.UIGuildMemberList.QueryGuildMemberList = (pageID) => {
            /// Gửi yêu cầu truy vấn thông tin thành viên ở trang tương ứng
            KT_TCPHandler.SendGetGuildMembers(pageID);
        };
    }

    /// <summary>
    /// Đóng khung danh sách gia tộc và thành viên bang hội
    /// </summary>
    public void CloseUIGuildMemberList()
    {
        if (this.UIGuildMemberList != null)
        {
            GameObject.Destroy(this.UIGuildMemberList.gameObject);
            this.UIGuildMemberList = null;
        }
    }
    #endregion

    #region Danh sách cổ tức bang hội
    /// <summary>
    /// Danh sách cổ tức bang hội
    /// </summary>
    public UIGuildShare UIGuildShare { get; protected set; }

    /// <summary>
    /// Mở khung cổ tức bang hội
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIGuildShare(GuildShareInfo data)
    {
        if (this.UIGuildShare != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildShare = canvas.LoadUIPrefab<UIGuildShare>("MainGame/Guild/UIGuildShare");
        canvas.AddUI(this.UIGuildShare);

        this.UIGuildShare.Data = data;
        this.UIGuildShare.Close = () => {
            this.CloseUIGuildShare();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
        this.UIGuildShare.OpenGuildOfficalRank = () => {
            this.CloseUIGuildShare();
            /// Gửi yêu cầu truy vấn thông tin Top 10 quan hàm bang hội
            KT_TCPHandler.SendGetGuildOfficialRankInfo();
        };
        this.UIGuildShare.QueryGuildShare = (pageID) => {
            /// Gửi yêu cầu truy vấn thông tin cổ tức bang hội trang tương ứng
            KT_TCPHandler.SendGetGuildShareList(pageID);
        };
    }

    /// <summary>
    /// Đóng khung cổ tức bang hội
    /// </summary>
    public void CloseUIGuildShare()
    {
        if (this.UIGuildShare != null)
        {
            GameObject.Destroy(this.UIGuildShare.gameObject);
            this.UIGuildShare = null;
        }
    }
    #endregion

    #region Top 10 quan hàm bang hội
    /// <summary>
    /// Khung Top 10 quan hàm bang hội
    /// </summary>
    public UIGuildOfficialRank UIGuildOfficialRank { get; protected set; }

    /// <summary>
    /// Mở khung Top 10 quan hàm bang hội
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIGuildOfficialRank(OfficeRankInfo data)
    {
        if (this.UIGuildOfficialRank != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildOfficialRank = canvas.LoadUIPrefab<UIGuildOfficialRank>("MainGame/Guild/UIGuildOfficialRank");
        canvas.AddUI(this.UIGuildOfficialRank);

        this.UIGuildOfficialRank.Data = data;
        this.UIGuildOfficialRank.Close = () => {
            this.CloseUIGuildOfficialRank();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
    }

    /// <summary>
    /// Đóng khung Top 10 quan hàm bang hội
    /// </summary>
    public void CloseUIGuildOfficialRank()
    {
        if (this.UIGuildOfficialRank != null)
        {
            GameObject.Destroy(this.UIGuildOfficialRank.gameObject);
            this.UIGuildOfficialRank = null;
        }
    }
    #endregion

    #region Ưu tú bang hội
    /// <summary>
    /// Khung ưu tú bang hội
    /// </summary>
    public UIGuildVote UIGuildVote { get; protected set; }

    /// <summary>
    /// Mở khung danh sách ưu tú bang hội
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIGuildVote(GuildVoteInfo data)
    {
        if (this.UIGuildVote != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildVote = canvas.LoadUIPrefab<UIGuildVote>("MainGame/Guild/UIGuildVote");
        canvas.AddUI(this.UIGuildVote);

        this.UIGuildVote.Data = data;
        this.UIGuildVote.Close = () => {
            this.CloseUIGuildVote();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
        this.UIGuildVote.Vote = (roleID) => {
            /// Gửi yêu cầu bầu ưu tú cho thành viên
            KT_TCPHandler.SendVoteExcellenceMember(roleID);
        };
        this.UIGuildVote.QueryGuildVote = (pageID) => {
            /// Gửi yêu cầu truy vấn danh sách ưu
            KT_TCPHandler.SendGetGuildExcellenceMembers(pageID);
        };
    }

    /// <summary>
    /// Đóng khung ưu tú bang hội
    /// </summary>
    public void CloseUIGuildVote()
    {
        if (this.UIGuildVote != null)
        {
            GameObject.Destroy(this.UIGuildVote.gameObject);
            this.UIGuildVote = null;
        }
    }
    #endregion

    #region Cống hiến bang hội
    /// <summary>
    /// Khung cống hiến bang hội
    /// </summary>
    public UIGuildDedication UIGuildDedication { get; protected set; }

    /// <summary>
    /// Mở khung cống hiến bang hội
    /// </summary>
    /// <param name="guildMoney"></param>
    public void OpenUIGuildDedication(int guildMoney)
    {
        if (this.UIGuildDedication != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildDedication = canvas.LoadUIPrefab<UIGuildDedication>("MainGame/Guild/UIGuildDedication");
        canvas.AddUI(this.UIGuildDedication);

        this.UIGuildDedication.Close = () => {
            this.CloseUIGuildDedication();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
        this.UIGuildDedication.GuildMoney = guildMoney;
        this.UIGuildDedication.Dedicate = (amount) => {
            /// Gửi yêu cầu cống hiến vào bang
            KT_TCPHandler.SendDedicateMoneyToGuild(amount);
        };
    }

    /// <summary>
    /// Đóng khung cống hiến bang hội
    /// </summary>
    public void CloseUIGuildDedication()
    {
        if (this.UIGuildDedication != null)
        {
            GameObject.Destroy(this.UIGuildDedication.gameObject);
            this.UIGuildDedication = null;
        }
    }
    #endregion

    #region Hoạt động bang hội
    /// <summary>
    /// Khung hoạt động bang hội
    /// </summary>
    public UIGuildActivity UIGuildActivity { get; protected set; }

    /// <summary>
    /// Mở khung hoạt động bang hội
    /// </summary>
    public void OpenUIGuildActivity()
    {
        if (this.UIGuildActivity != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIGuildActivity = canvas.LoadUIPrefab<UIGuildActivity>("MainGame/Guild/UIGuildActivity");
        canvas.AddUI(this.UIGuildActivity);

        this.UIGuildActivity.Close = () => {
            this.CloseUIGuildActivity();
            /// Nếu chưa có bang hội
            if (Global.Data.RoleData.GuildID <= 0)
            {
                return;
            }
            /// Gửi yêu cầu truy vấn thông tin bang hội
            KT_TCPHandler.SendGetGuildInfo();
        };
        this.UIGuildActivity.UIColonyDispute.QueryData = () => {
            /// Gửi yêu cầu truy vấn thông tin lãnh thổ
            KT_TCPHandler.SendGetGuildColonyDisputeInfo();
        };
        this.UIGuildActivity.UIColonyDispute.SetTax = (mapID, tax) => {
            /// Gửi yêu cầu thiết lập thuế lên lãnh thổ
            KT_TCPHandler.SendSetGuildColonyTax(mapID, tax);
        };
        this.UIGuildActivity.UIColonyDispute.SetMainCastle = (mapID) => {
            /// Gửi yêu cầu thiết lập thành chính
            KT_TCPHandler.SendSetGuildColonyAsMainCastle(mapID);
        };
    }

    /// <summary>
    /// Đóng khung hoạt động bang hội
    /// </summary>
    public void CloseUIGuildActivity()
    {
        if (this.UIGuildActivity != null)
        {
            GameObject.Destroy(this.UIGuildActivity.gameObject);
            this.UIGuildActivity = null;
        }
    }
    #endregion
    #endregion
}
