using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Tổ đội
    /// <summary>
    /// Khung tổ đội
    /// </summary>
    public UITeamFrame UITeamFrame { get; protected set; } = null;

    /// <summary>
    /// Hiện khung tổ đội
    /// </summary>
    public void ShowTeamFrame(List<RoleDataMini> members)
    {
        if (this.UITeamFrame != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UITeamFrame = canvas.LoadUIPrefab<UITeamFrame>("MainGame/UITeamFrame");
        canvas.AddUI(this.UITeamFrame);
        this.UITeamFrame.Ready = () => {
            /// Duyệt danh sách thành viên và thêm vào nhóm
            this.UITeamFrame.AddTeammates(members);
        };
        this.UITeamFrame.Close = this.HideTeamFrame;
        this.UITeamFrame.CreateTeam = () => {
            /// Nếu đã có nhóm rồi
            if (Global.Data.RoleData.TeamID != -1)
            {
                return;
            }

            KT_TCPHandler.SendCreateTeam();
        };
        this.UITeamFrame.LeaveTeam = () => {
            /// Nếu không có nhóm
            if (Global.Data.RoleData.TeamID == -1)
            {
                return;
            }

            KT_TCPHandler.SendLeaveTeam();
        };
        this.UITeamFrame.KickOut = (roleData) => {
            /// Nếu không có đối tượng người chơi cần trục xuất
            if (roleData == null)
            {
                return;
            }
            /// Nếu bản thân không phải đội trưởng
            else if (Global.Data.RoleData.TeamLeaderRoleID != Global.Data.RoleData.RoleID)
            {
                return;
            }
            /// Nếu đối tượng được chọn là bản thân
            else if (Global.Data.RoleData.RoleID == roleData.RoleID)
            {
                return;
            }

            KT_TCPHandler.SendKickOutTeammate(roleData.RoleID);
        };
        this.UITeamFrame.ApproveTeamLeader = (roleData) => {
            /// Nếu không có đối tượng
            if (roleData == null)
            {
                return;
            }
            /// Nếu bản thân không phải đội trưởng
            else if (Global.Data.RoleData.TeamLeaderRoleID != Global.Data.RoleData.RoleID)
            {
                return;
            }
            /// Nếu đối tượng được chọn là bản thân
            else if (Global.Data.RoleData.RoleID == roleData.RoleID)
            {
                return;
            }

            KT_TCPHandler.SendApproveTeamLeader(roleData.RoleID);
        };
        this.UITeamFrame.AgreeJoinTeam = (roleData) => {
            /// Nếu không có đối tượng người chơi cần trục xuất
            if (roleData == null)
            {
                return;
            }
            /// Nếu đã có trong nhóm
            else if (this.UITeamFrame.IsTeammateExist(roleData.RoleID))
            {
                this.UITeamFrame.RemoveWaitingPlayer(roleData.RoleID);
                return;
            }
            /// Nếu bản thân không có nhóm
            else if (Global.Data.RoleData.TeamID == -1)
            {
                KTGlobal.AddNotification("Bạn chưa có nhóm, không thể thêm thành viên vào!");
                return;
            }

            KT_TCPHandler.SendAgreeToJoinTeam(roleData.RoleID, Global.Data.RoleData.TeamID);
        };
        this.UITeamFrame.RefuseJoinTeam = (roleData) => {
            /// Nếu không có đối tượng người chơi cần trục xuất
            if (roleData == null)
            {
                return;
            }
            /// Nếu bản thân không có nhóm
            else if (Global.Data.RoleData.TeamID == -1)
            {
                KTGlobal.AddNotification("Bạn chưa có nhóm, không thể thêm thành viên vào!");
                return;
            }

            KT_TCPHandler.SendRefuseToJoinTeam(-1, roleData.RoleID, 1);
        };
    }

    /// <summary>
    /// Ẩn khung tổ đội
    /// </summary>
    private void HideTeamFrame()
    {
        if (this.UITeamFrame != null)
        {
            GameObject.Destroy(this.UITeamFrame.gameObject);
            this.UITeamFrame = null;
        }
    }
    #endregion
}
