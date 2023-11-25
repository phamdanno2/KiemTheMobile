using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.SpecialEvents.FactionBattle;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
	#region Bảng xếp hạng
	/// <summary>
	/// Khung bảng xếp hạng
	/// </summary>
	public UIRanking UIRanking { get; protected set; }

    /// <summary>
    /// Hiển thị khung bảng xếp hạng
    /// </summary>
    /// <param name="attributes"></param>
    public void ShowUIRanking()
    {
        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIRanking = canvas.LoadUIPrefab<UIRanking>("MainGame/UIRanking");
        canvas.AddUI(this.UIRanking);

        this.UIRanking.Close = this.CloseUIRanking;
        this.UIRanking.QueryPage = (type, pageID) => {
            KTGlobal.ShowLoadingFrame("Đang tải dữ liệu bảng xếp hạng...");

            KT_TCPHandler.SendQueryPlayerRanking(type, pageID);
        };
    }

    /// <summary>
    /// Đóng khung bảng xếp hạng
    /// </summary>
    public void CloseUIRanking()
    {
        if (this.UIRanking != null)
        {
            GameObject.Destroy(this.UIRanking.gameObject);
            this.UIRanking = null;
            return;
        }
    }
    #endregion
}
