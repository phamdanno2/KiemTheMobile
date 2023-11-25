using UnityEngine;
using System.Collections;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Factory.UIManager;
using FSPlay.KiemVu.Utilities.Threading;
using FSPlay.KiemVu.Logic.BackgroundWork;
#if !UNITY_IOS
using FSPlay.KiemVu.Factory.InAppPurchase;
#endif
using FSPlay.KiemVu.Factory.ObjectsManager;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
#region Private fields
    /// <summary>
    /// Thời điểm lần trước Click di chuyển (tránh trường hợp di chuyển liên tục dẫn đến Bug)
    /// </summary>
    private long LastClickMoveTicks = 0;

    /// <summary>
    /// Cho phép Click di chuyển sau khoảng tương ứng
    /// </summary>
    private int RefreshClickMoveAfterTicks = 500;
#endregion

#region Init
    /// <summary>
    /// Khởi tạo UI Game
    /// </summary>
    protected void InitializeGameInterface()
    {
        /// Khởi tạo các đối tượng quản lý
        this.InitManagers();

        /// JoyStick
        this.InitJoyStick();
        /// Chữ trang trí
        this.InitDecorationText();
        /// Thanh Progress Bar
        this.InitProgressBar();

        /// Khung Skill Bar
        this.InitBottomBar();

        /// Khung danh sách Button sự kiện phía trên
        this.InitTopFunctionButtons();

        /// Khung đối thoại nhanh với NPC
        this.InitUIShortcutNPCTalk();

        /// Khung thông tin chỉ số nhân vật ở Main UI
        this.InitPlayerSelfInfo();
        this.InitializeObjectRoleFace();
        this.InitializeMonsterFace();

        /// Tooltip soi các chức năng người chơi khác
        this.InitializeUIBrowseOtherRoleInfo();
        this.HideBrowseOtherRoleInfo();

        /// Hiện khung Chat Mini
        this.InitChatBoxMini();
        /// Hiện kênh chat đặc biệt
        this.InitSpecialChatBox();

        /// Khung MiniTaskBox và MiniTeamFrame
        this.InitMiniTaskAndTeamFrame();

        /// Khung người chơi xung quanh là kẻ địch
        this.InitUINearbyEnemyPlayer();

        /// Khung người chơi lân cận
        this.InitUINearbyPlayer();
        /// Ẩn khung người chơi lân cận
        this.HideUINearbyPlayers();

        /// Khung MiniEventBroadboard
        this.InitMiniEventBroadboard();
        /// Ẩn khung MiniEventBroadboard
        this.UIEventBroadboardMini.gameObject.SetActive(false);

        /// Khung hiện số liên trảm
        this.InitUIStreakKillNotification();
        /// Ẩn khung hiện số liên trảm
        this.UIStreakKillNotification.gameObject.SetActive(false);

        /// Khởi tạo Button sự kiện đặc biệt
        this.InitUISpecialEventButtons();

        this.StartUITimer();
        this.ExecuteSystemSettings();

        /// Ẩn tất cả khung mặt đối tượng
        this.HideAllFace();
    }

    /// <summary>
    /// Khởi tạo các đối tượng quản lý
    /// </summary>
    private void InitManagers()
    {
        UIBagManager.NewInstance();
        UIMoneyManager.NewInstance();
        this.gameObject.AddComponent<KTBackgroundWorkManager>();
        this.gameObject.AddComponent<KTAutoFightManager>();
        this.gameObject.AddComponent<KTStoryBoard>();
        this.gameObject.AddComponent<UIHintItemManager>();
        this.gameObject.AddComponent<UIBottomTextManager>();
        this.gameObject.AddComponent<AutoPathManager>();
        AutoPathManager.Instance.PrepareData();
        this.gameObject.AddComponent<AutoQuest>();
        UIRoleAvartaManager.NewInstance();
        this.gameObject.AddComponent<KTTimerManager>();
        this.gameObject.AddComponent<KTAutoPickUpItemAround>();
#if !UNITY_IOS
        this.gameObject.AddComponent<IAPManager>();
#endif
        this.gameObject.AddComponent<KTVoiceChatManager>();
    }

    /// <summary>
    /// Luồng thực thi cập nhật UI
    /// </summary>
    private Coroutine uiTimer;

    /// <summary>
    /// Thực thi cập nhật UI sau mỗi 0.5s
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExecuteUITimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (null == scene)
            {
                continue;
            }

        }
    }

    /// <summary>
    /// Bắt đầu luồng thực thi cập nhật UI
    /// </summary>
    private void StartUITimer()
    {
        if (this.uiTimer != null)
        {
            this.StopCoroutine(this.uiTimer);
        }
        this.uiTimer = this.StartCoroutine(this.ExecuteUITimer());
    }
#endregion
}

