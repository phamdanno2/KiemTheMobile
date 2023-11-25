using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic;
using Server.Data;
using System.Collections.Generic;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    /// <summary>
    /// Sự kiện khi người chơi vào Game
    /// </summary>
    public void OnEnterGame()
    {
        /// Làm mới thiết lập bản đồ nhỏ
        this.UIRadarMap.Refresh();

        /// Xây danh sách nhiệm vụ đã hoàn thành
        Global.Data.BuildCompletedTasks();

        /// Nếu không phải bản đồ chờ Tiêu Dao Cốc
        if (Global.Data.GameScene.MapCode != 304)
        {
            /// Ẩn khung MiniEventBroadboard
            this.UIEventBroadboardMini.gameObject.SetActive(false);
            /// Hiện khung nhiệm vụ tổ đội mini
            this.UIMiniTaskAndTeamFrame.gameObject.SetActive(true);
            /// Làm mới bảng nhiệm vụ
            if (this.UIMiniTaskAndTeamFrame != null)
            {
                this.UIMiniTaskAndTeamFrame.UIMiniTaskBox.RefreshTasks();
            }
        }
        else
        {
            /// Hiện khung MiniEventBroadboard
            this.UIEventBroadboardMini.gameObject.SetActive(true);
            /// Ẩn khung nhiệm vụ tổ đội mini
            this.UIMiniTaskAndTeamFrame.gameObject.SetActive(false);
        }

        /// Làm mới danh sách nhóm
        if (Global.Data.RoleData.TeamID == -1)
        {
            Global.Data.Teammates.Clear();
            if (this.UIMiniTaskAndTeamFrame != null)
            {
                this.UIMiniTaskAndTeamFrame.UITeamFrame.RemoveAllTeamMembers();
            }
        }

        /// Thực hiện Recover lại các hiệu ứng có trên người
        if (Global.Data.RoleData.BufferDataList != null)
        {
            foreach (BufferData buff in Global.Data.RoleData.BufferDataList)
            {
                if (Loader.Skills.TryGetValue(buff.BufferID, out FSPlay.KiemVu.Entities.Config.SkillDataEx skillData))
                {
                    Global.Data.Leader.AddBuff(skillData.StateEffectID, buff.BufferSecs == -1 ? -1 : buff.BufferSecs - (KTGlobal.GetServerTime() - buff.StartTime));
                }
            }

            if (this.UIRolePart != null)
            {
                this.UIRolePart.UIBuffList.RefreshDataList();
            }

            if (this.UIBottomBar != null)
            {
                this.UIBottomBar.UISkillBar.RefreshAruaIcon();
                this.UIBottomBar.UISkillBar.RefreshCooldowns();
                this.UIBottomBar.UISkillBar.RefreshSkillIcon();
            }
        }
        else
        {
            Global.Data.RoleData.BufferDataList = new List<BufferData>();
        }
        /// End
        if(!KTAutoFightManager.Instance.DoingAutoSell)
        {
            /// Ngừng tự đánh
            KTAutoFightManager.Instance.StopAutoFight();
        }    
       
    }

    /// <summary>
    /// Chuyển cảnh
    /// </summary>
    public void OnChangeSceneUI()
    {
        this.HideFace();
        this.CloseNPCDialog();
        this.CloseUIQuickKeyChooser();
        this.CloseUIRoleInfo();
        this.CloseUIRoleRemainPoint();
        this.CloseUISkillTree();
        //this.CloseWelcomeDialog();
        this.CloseSystemSetting();
        /// Đóng khung bản đồ khu vực
        this.CloseWorldNavigationWindow();
        /// Đóng khung cửa hàng
        this.CloseUIPlayerShop_Buy();
        this.CloseUIPlayerShop_Sell();
        /// Đóng khung giao dịch
        if (this.UIExchange != null)
        {
            this.UIExchange.Close?.Invoke();
        }
        /// Xóa thông tin bán hàng
        Global.Data.StallDataItem = null;
        Global.Data.OtherStallDataItem = null;
        /// Xóa thông tin giao dịch
        Global.Data.ExchangeID = -1;
        Global.Data.ExchangeDataItem = null;

        /// Xóa thông tin tỷ thí
        KTGlobal.ChallengePartnerID = -1;
        /// Xóa đối tượng đang tuyên chiến cùng
        KTGlobal.ActiveFightWith.Clear();
        /// Ẩn khung người chơi xung quanh
        this.HideUINearbyPlayers();
        /// Ẩn các Button đặc biệt của sự kiện
        this.UISpecialEventButtons.HideAllButtons();

        /// Ẩn khung hiện số liên trảm
        this.UIStreakKillNotification.gameObject.SetActive(false);
    }

    /// <summary>
    /// Thiết lập Main UI cho bản đồ mới
    /// </summary>
    /// <param name="visible"></param>
    public void SetMainUIForScene()
    {
        /// Làm mới bảng nhiệm vụ Mini
        if (this.UIMiniTaskAndTeamFrame != null)
        {
            this.UIMiniTaskAndTeamFrame.UIMiniTaskBox.RefreshTasks();
        }

        /// Làm mới danh sách nhóm Mini

        ///// Hiển thị màn hình chào người chơi
        //if (this.UIWelcomeDialog == null)
        //{
        //    this.ShowWelcomeDialog();
        //}
    }
}
