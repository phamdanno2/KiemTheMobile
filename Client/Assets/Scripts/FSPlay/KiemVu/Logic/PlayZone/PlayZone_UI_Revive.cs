using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main.Revive;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Khung hồi sinh
    /// <summary>
    /// Khung hồi sinh
    /// </summary>
    public UIReviveFrame UIReviveFrame { get; protected set; } = null;

    /// <summary>
    /// Hiển thị khung hồi sinh
    /// </summary>
    public void ShowReviveFrame(string message, bool allowReviveAtPos)
    {
        if (this.UIReviveFrame != null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.UIReviveFrame.Message = message;
            }

            this.UIReviveFrame.AllowReviveAtPos = allowReviveAtPos;
            this.UIReviveFrame.BackToCity = () => {
                KT_TCPHandler.ClientRevive(1);
            };
            this.UIReviveFrame.ReviveAtPos = () => {
                KT_TCPHandler.ClientRevive(2);
            };
            this.UIReviveFrame.UseRevivePill = () => {
                KT_TCPHandler.ClientRevive(3);
            };
            return;
        }
        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIReviveFrame = canvas.LoadUIPrefab<UIReviveFrame>("MainGame/UIReviveFrame");
        canvas.AddUI(this.UIReviveFrame);

        this.UIReviveFrame.Message = message;
        this.UIReviveFrame.AllowReviveAtPos = allowReviveAtPos;
        this.UIReviveFrame.BackToCity = () => {
            KT_TCPHandler.ClientRevive(1);
        };
        this.UIReviveFrame.ReviveAtPos = () => {
            KT_TCPHandler.ClientRevive(2);
        };
        this.UIReviveFrame.UseRevivePill = () => {
            KT_TCPHandler.ClientRevive(3);
        };
    }

    /// <summary>
    /// Đóng khung hồi sinh
    /// </summary>
    public void CloseReviveFrame()
    {
        if (this.UIReviveFrame == null)
        {
            return;
        }
        GameObject.Destroy(this.UIReviveFrame.gameObject);
        this.UIReviveFrame = null;
    }
    #endregion
}
