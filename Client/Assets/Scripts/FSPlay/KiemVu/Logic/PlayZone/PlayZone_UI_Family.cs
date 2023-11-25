using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.Family;
using Server.Data;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Gia Tộc
    #region Tạo tộc

    /// <summary>
    /// Khung Tạo gia tộc
    /// </summary>
    public UICreateFamily UICreateFamily { get; protected set; }

    /// <summary>
    /// Mở Khung Tạo gia tộc
    /// </summary>
    /// <param name="data"></param>
    public void OpenUICreateFamily()
    {
        if (this.UICreateFamily != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UICreateFamily = canvas.LoadUIPrefab<UICreateFamily>("MainGame/Family/UICreateFamily");
        canvas.AddUI(this.UICreateFamily);
        UICreateFamily.CreateFamily = (NameFamily) => {
            KT_TCPHandler.SpriteCreateFamily(NameFamily);
        };
        UICreateFamily.Close = () => {
            this.ClosUICreateFamily();
        };
        UICreateFamily.Cancel = () => {
            this.ClosUICreateFamily();
        };
    }

    /// <summary>
    /// Đóng Khung Tạo gia tộc
    /// </summary>
    public void ClosUICreateFamily()
    {
        if (this.UICreateFamily != null)
        {
            GameObject.Destroy(this.UICreateFamily.gameObject);
            this.UICreateFamily = null;
        }
    }

    #endregion tạo tộc

    #region Danh sách thành viên tộc

    /// <summary>
    /// Khung thành viên trong gia tộc
    /// </summary>
    public UIFamily UIFamily { get; protected set; }

    /// <summary>
    /// Mở Khung Tạo gia tộc
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIFamily(Family data)
    {
        if (this.UIFamily != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIFamily = canvas.LoadUIPrefab<UIFamily>("MainGame/Family/UIFamily");
        canvas.AddUI(this.UIFamily);
        this.UIFamily.Data = data;
        this.UIFamily.Close = () => {
            this.ClosUIFamily();
        };

        this.UIFamily.AcceptToJoinFamily = (roleID) => {
            KT_TCPHandler.SendToSeverFamilyResponseRequest(roleID, 1);
        };
        this.UIFamily.RefuseToJoinFamily = (roleID) => {
            KT_TCPHandler.SendToSeverFamilyResponseRequest(roleID, 0);
        };
        this.UIFamily.KickOut = (memberInfo) => {
            KT_TCPHandler.SendToSeverKickMemberOutFamily(memberInfo.RoleID);
        };
        this.UIFamily.QuitFamily = () => {
            KT_TCPHandler.SendToSeverOutFamily();
            /// Đóng khung
            this.ClosUIFamily();
        };
        this.UIFamily.Approve = (memberInfo, rank) => {
            KT_TCPHandler.SendToSeverChangeRank(memberInfo.RoleID, rank);
        };
        this.UIFamily.ChangeSlogan = (slogan) => {
            KT_TCPHandler.SendToSeverChangeSlogsn(slogan);
        };
        this.UIFamily.ChangeAskToJoinFamilyRequirement = (requirement) => {
            KT_TCPHandler.SendToSeverChangeRequestFamily(requirement);
        };
    }

    /// <summary>
    /// Đóng Khung Tạo gia tộc
    /// </summary>
    public void ClosUIFamily()
    {
        if (this.UIFamily != null)
        {
            GameObject.Destroy(this.UIFamily.gameObject);
            this.UIFamily = null;
        }
    }

    #endregion Danh sách thành viên tộc

    #region Danh sách tộc

    /// <summary>
    /// Khung Tạo gia tộc
    /// </summary>
    public UIFamily_UIFamilyList UIFamilyList { get; protected set; }

    /// <summary>
    /// Mở Khung xin vào ra tộc
    /// </summary>
    /// <param name="data"></param>
    public void OpenUIFamilyList(List<FamilyInfo> data)
    {
        if (this.UIFamilyList != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIFamilyList = canvas.LoadUIPrefab<UIFamily_UIFamilyList>("MainGame/Family/UIFamilyList");
        canvas.AddUI(this.UIFamilyList);
        UIFamilyList.Data = data;
        UIFamilyList.Close = () => {
            this.ClosUIFamilyList();
        };
        UIFamilyList.AskToJoinGuild = (familID) => {
            KT_TCPHandler.SendToSeverFamilyAskToJoin(familID);
        };
    }

    /// <summary>
    /// Đóng Khung Tạo gia tộc
    /// </summary>
    public void ClosUIFamilyList()
    {
        if (this.UIFamilyList != null)
        {
            GameObject.Destroy(this.UIFamilyList.gameObject);
            this.UIFamilyList = null;
        }
    }
    #endregion Danh sách tộc
    #endregion
}
