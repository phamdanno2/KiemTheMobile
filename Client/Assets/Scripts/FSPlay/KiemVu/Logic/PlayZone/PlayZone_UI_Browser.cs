﻿using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.MainUI;
using Server.Data;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Tìm người chơi
    /// <summary>
    /// Khung tìm người chơi
    /// </summary>
    public UIBrowsePlayer UIBrowsePlayer { get; protected set; } = null;

    /// <summary>
    /// Hiện khung tìm người chơi
    /// </summary>
    public void ShowUIBrowsePlayer()
    {
        if (this.UIBrowsePlayer != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIBrowsePlayer = canvas.LoadUIPrefab<UIBrowsePlayer>("MainGame/UIBrowsePlayer");
        canvas.AddUI(this.UIBrowsePlayer);

        this.UIBrowsePlayer.Close = this.CloseUIBrowsePlayer;
        this.UIBrowsePlayer.BrowsePlayer = (playerName) => {
            KT_TCPHandler.SendBrowsePlayer(playerName);
        };
        this.UIBrowsePlayer.PrivateChat = (rd) => {
            /// Mở khung Chat và trỏ đến Chat mật
            KTGlobal.OpenPrivateChatBoxWith(rd.RoleName);
        };
        this.UIBrowsePlayer.AddFriend = (rd) => {
            /// Gửi gói tin yêu cầu thêm bạn
            GameInstance.Game.SpriteAskFriend(rd.RoleID);
        };
        this.UIBrowsePlayer.AddEnemy = (rd) => {
            /// Gửi gói tin yêu cầu thêm kẻ thù
            GameInstance.Game.SpriteAddFriend(-1, rd.RoleID, rd.RoleName, 2);
        };
        this.UIBrowsePlayer.AddToBlackList = (rd) => {
            /// Gửi gói tin yêu cầu thêm kẻ thù
            GameInstance.Game.SpriteAddFriend(-1, rd.RoleID, rd.RoleName, 1);
        };
        this.UIBrowsePlayer.CheckLocation = (rd) => {
            /// Gửi gói tin yêu cầu kiểm tra vị trí người chơi
            KT_TCPHandler.SendCheckPlayerLocation(rd.RoleID);
        };
        this.UIBrowsePlayer.InviteToTeam = (rd) => {
            /// Nếu bản thân không có nhóm
            if (Global.Data.RoleData.TeamID == -1)
            {
                return;
            }
            /// Nếu bản thân không phải trưởng nhóm
            else if (Global.Data.RoleData.TeamLeaderRoleID != Global.Data.RoleData.RoleID)
            {
                return;
            }
            /// Nếu không có dữ liệu đối phương
            else if (rd == null)
            {
                return;
            }
            /// Nếu đối phương đã có nhóm
            else if (rd.TeamID != -1)
            {
                return;
            }

            KT_TCPHandler.SendInviteToTeam(rd.RoleID);
        };
        this.UIBrowsePlayer.AskToJoinTeam = (rd) => {
            /// Nếu bản thân đã có nhóm
            if (Global.Data.RoleData.TeamID != -1)
            {
                return;
            }
            /// Nếu không có dữ liệu đối phương
            else if (rd == null)
            {
                return;
            }
            /// Nếu đối phương chưa có nhóm
            else if (rd.TeamID == -1)
            {
                return;
            }

            KT_TCPHandler.SendAskToJoinTeam(rd.RoleID);
        };
    }

    /// <summary>
    /// Đóng khung tìm người chơi
    /// </summary>
    public void CloseUIBrowsePlayer()
    {
        if (this.UIBrowsePlayer != null)
        {
            GameObject.Destroy(this.UIBrowsePlayer.gameObject);
            this.UIBrowsePlayer = null;
        }
    }
    #endregion

    #region Kẻ địch xung quanh
    /// <summary>
    /// Khung kẻ địch xung quanh là người chơi
    /// </summary>
    public UINearbyEnemyPlayer UINearbyEnemyPlayer { get; protected set; }

    /// <summary>
    /// Khởi tạo khung kẻ địch xung quanh là người chơi
    /// </summary>
    public void InitUINearbyEnemyPlayer()
    {
        if (this.UINearbyEnemyPlayer != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UINearbyEnemyPlayer = canvas.LoadUIPrefab<UINearbyEnemyPlayer>("MainGame/MainUI/UINearbyEnemyPlayer");
        canvas.AddUI(this.UINearbyEnemyPlayer);

        this.UINearbyEnemyPlayer.Click = (roleData) => {
            if (Global.Data.OtherRoles.TryGetValue(roleData.RoleID, out _))
            {
                GSprite sprite = KTGlobal.FindSpriteByID(roleData.RoleID);
                if (sprite == null)
                {
                    return;
                }

                /// Thông báo chọn mục tiêu thành công
                KTGlobal.AddNotification(string.Format("Chọn mục tiêu {0}", roleData.RoleName));

                SkillManager.SelectedTarget = sprite;
                KTAutoFightManager.Instance.ChangeAutoFightTarget(sprite);
                Global.Data.GameScene.OtherRoleClick(sprite);
            }
        };
    }
    #endregion

    #region Người chơi xung quanh
    /// <summary>
    /// Khung danh sách người chơi xung quanh
    /// </summary>
    public UINearbyPlayer UINearbyPlayer { get; protected set; }

    /// <summary>
    /// Hiển thị khung chứa danh sách người chơi xung quanh
    /// </summary>
    public void InitUINearbyPlayer()
    {
        if (this.UINearbyPlayer != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UINearbyPlayer = canvas.LoadUIPrefab<UINearbyPlayer>("MainGame/MainUI/UINearbyPlayers");
        canvas.AddUI(this.UINearbyPlayer);

        this.UINearbyPlayer.PlayerSelected = (roleData) => {
            if (Global.Data.OtherRoles.TryGetValue(roleData.RoleID, out _))
            {
                GSprite sprite = KTGlobal.FindSpriteByID(roleData.RoleID);
                if (sprite == null)
                {
                    return;
                }
                SkillManager.SelectedTarget = sprite;
                KTAutoFightManager.Instance.ChangeAutoFightTarget(sprite);
                Global.Data.GameScene.OtherRoleClick(sprite);
            }
        };
    }

    /// <summary>
    /// Hiện khung chứa danh sách người chơi xung quanh
    /// </summary>
    public void ShowUINearbyPlayers()
    {
        if (this.UINearbyPlayer != null)
        {
            this.UINearbyPlayer.Show();
        }
    }

    /// <summary>
    /// Ẩn khung chứa danh sách người chơi xung quanh
    /// </summary>
    public void HideUINearbyPlayers()
    {
        if (this.UINearbyPlayer != null)
        {
            this.UINearbyPlayer.Hide();
        }
    }
    #endregion

    #region PlayerInfo
    /// <summary>
    /// Khung thông tin người chơi
    /// </summary>
    public UIPlayerInfo UIPlayerInfo { get; protected set; }

    /// <summary>
    /// Mở khung thông tin người chơi
    /// </summary>
    /// <param name="rd"></param>
    public void OpenUIPlayerInfo(RoleDataMini rd)
    {
        if (this.UIPlayerInfo != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIPlayerInfo = canvas.LoadUIPrefab<UIPlayerInfo>("MainGame/UIPlayerInfo");
        canvas.AddUI(this.UIPlayerInfo);

        this.UIPlayerInfo.Data = rd;
        this.UIPlayerInfo.Close = this.CloseUIPlayerInfo;
    }

    /// <summary>
    /// Đóng khung thông tin người chơi
    /// </summary>
    public void CloseUIPlayerInfo()
    {
        /// Nếu đang mở
        if (this.UIPlayerInfo != null)
        {
            GameObject.Destroy(this.UIPlayerInfo.gameObject);
            this.UIPlayerInfo = null;
        }
    }
    #endregion
}
