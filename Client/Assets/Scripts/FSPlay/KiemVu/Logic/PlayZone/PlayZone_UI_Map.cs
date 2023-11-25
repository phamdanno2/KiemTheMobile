using FSPlay.Drawing;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main.LocalMap;
using FSPlay.KiemVu.UI.Main.MainUI;
using Server.Data;
using System.Linq;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Radar Map
    /// <summary>
    /// Radar Map
    /// </summary>
    public UIRadarMap UIRadarMap { get; private set; } = null;

    /// <summary>
    /// Khởi tạo Radar Map
    /// </summary>
    protected void InitRadarMap()
    {
        if (this.UIRadarMap != null)
        {
            GameObject.Destroy(this.UIRadarMap.gameObject);
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIRadarMap = canvas.LoadUIPrefab<UIRadarMap>("MainGame/MainUI/UIRadarMap");
        canvas.AddMainUI(this.UIRadarMap);

        this.UIRadarMap.GoToLocalMap = () => {
            this.ShowWorldNavigationWindow();
        };
        this.UIRadarMap.UseMedicine = (itemGD) => {
            /// Nếu đang trong trạng thái bị khống chế
            if (!Global.Data.Leader.CanDoLogic)
            {
                KTGlobal.AddNotification("Trong trạng thái bị khống chế không thể sử dụng vật phẩm!");
                return;
            }

            /// Cấu hình vật phẩm
            ItemData itemData = null;
            if (!Loader.Items.TryGetValue(itemGD.GoodsID, out itemData))
            {
                KTGlobal.AddNotification("Vật phẩm bị lỗi, hãy thông báo với hỗ trợ để được xử lý!");
                return;
            }

            /// Nếu không thể sử dụng, và không phải thuốc
            if (!itemData.IsScriptItem && !itemData.IsMedicine)
            {
                KTGlobal.AddNotification("Vật phẩm này không thể sử dụng được!");
                return;
            }

            /// Sử dụng
            GameInstance.Game.SpriteUseGoods(itemGD.Id);
        };
        this.UIRadarMap.MedicineSelected = (itemGD1, itemGD2) => {
            /// Lưu thiết lập vào hệ thống
            KTAutoAttackSetting.SaveSettings();
        };
        this.LoadRadarMap();
    }

    /// <summary>
    /// Tải xuống bản đồ
    /// </summary>
    protected void LoadRadarMap()
    {
        if (this.UIRadarMap == null)
        {
            return;
        }

        if (FSPlay.KiemVu.Loader.Loader.Maps.TryGetValue(this.scene.MapCode, out FSPlay.KiemVu.Entities.Config.Map map))
        {
            this.UIRadarMap.MapName = map.Name;
        }
    }
    #endregion

    #region Bản đồ
    /// <summary>
    /// Bản đồ
    /// </summary>
    public UILocalMap UILocalMap { get; private set; }

    /// <summary>
    /// Đóng bản đồ
    /// </summary>
    protected void CloseWorldNavigationWindow()
    {
        if (this.UILocalMap != null)
        {
            this.UILocalMap.Hide();
        }
    }

    /// <summary>
    /// Hiển thị bản đồ
    /// </summary>
    /// <param name="caching"></param>
    protected void ShowWorldNavigationWindow()
    {
        if (this.UILocalMap == null)
        {
            this.UILocalMap = CanvasManager.Instance.LoadUIPrefab<UILocalMap>("MainGame/UILocalMap");
            CanvasManager.Instance.AddUI(this.UILocalMap);
        }

        if (Loader.Maps.TryGetValue(this.scene.MapCode, out FSPlay.KiemVu.Entities.Config.Map map))
        {
            this.UILocalMap.RealMapSize = new Vector2(map.Width, map.Height);
            this.UILocalMap.LocalMapName = map.Name;
            this.UILocalMap.LocalMapSprite = Global.CurrentMap.LocalMapSprite;
            this.UILocalMap.ListNPCs = this.scene.CurrentMapData.NpcList;
            this.UILocalMap.ListTeleport = this.scene.CurrentMapData.Teleport;
            this.UILocalMap.ListTrainArea = this.scene.CurrentMapData.MonsterList;
            this.UILocalMap.ListZone = this.scene.CurrentMapData.Zone;
            this.UILocalMap.LeaderPosition = new Vector2(this.scene.GetLeader().PosX, this.scene.GetLeader().PosY);
            this.UILocalMap.LocalMapClicked = (position) => {
                /// Nếu đang trong trạng thái khinh công thì không thao tác
                if (Global.Data.Leader.CurrentAction == KE_NPC_DOING.do_jump)
                {
                    return;
                }
                /// Nếu Leader đã chết
                else if (Global.Data.Leader.IsDeath || Global.Data.Leader.HP <= 0)
                {
                    return;
                }
                /// Nếu đang bày bán
                else if (Global.Data.StallDataItem != null)
                {
                    KTGlobal.AddNotification("Trong trạng thái bán hàng không thể di chuyển!");
                    return;
                }
                /// Nếu Leader đang bị khóa bởi kỹ năng
                else if (!Global.Data.Leader.CanPositiveMove)
                {
                    KTGlobal.AddNotification("Đang trong trạng thái bị khống chế, không thể di chuyển!");
                    return;
                }
                /// Nếu chưa thực hiện xong động tác trước
                else if (!Global.Data.Leader.IsReadyToMove)
                {
                    return;
                }
                /// Nếu đang trong thời gian thực hiện động tác dùng kỹ năng
                else if (!KTGlobal.FinishedUseSkillAction)
                {
                    return;
                }
                /// Nếu đang đợi dùng kỹ năng
                else if (SkillManager.IsWaitingToUseSkill)
                {
                    return;
                }

                /// Nếu có ngựa nhưng không trong trạng thái cưỡi
                GoodsData horseGD = Global.Data.RoleData.GoodsDataList?.Where(x => x.Using == (int) KE_EQUIP_POSITION.emEQUIPPOS_HORSE).FirstOrDefault();
                if (horseGD != null && !Global.Data.Leader.ComponentCharacter.Data.IsRiding)
                {
                    KT_TCPHandler.SendChangeToggleHorseState();
                }

                /// TODO xử lý thêm nếu có Truyền Tống Phù

                KTLeaderMovingManager.AutoFindRoad(new Point((int) position.x, (int) position.y));
            };
            this.UILocalMap.Close = () => {
                this.CloseWorldNavigationWindow();
            };
            this.UILocalMap.GoToMap = (mapCode) => {
                /// Ẩn khung
                this.UILocalMap.Hide();

                KTGlobal.QuestAutoFindPathToMap(mapCode, () => {
                    AutoQuest.Instance.StopAutoQuest();
                    AutoPathManager.Instance.StopAutoPath();
                });
            };
            this.UILocalMap.GoToNPC = (mapCode, npcID) => {
                KTGlobal.QuestAutoFindPathToNPC(mapCode, npcID, () => {
                    AutoQuest.Instance.StopAutoQuest();
                    AutoPathManager.Instance.StopAutoPath();
                    GSprite sprite = KTGlobal.FindNearestNPCByResID(npcID);
                    if (sprite == null)
                    {
                        KTGlobal.AddNotification("Không tìm thấy NPC tương ứng!");
                        return;
                    }
                    Global.Data.TargetNpcID = sprite.RoleID - FSPlay.GameFramework.Logic.SpriteBaseIds.NpcBaseId;
                    Global.Data.GameScene.NPCClick(sprite);
                });
            };
            this.UILocalMap.QueryTerritoryList = () => {
                KTGlobal.ShowLoadingFrame("Đang truy vấn dữ liệu...");
                /// Gửi yêu cầu truy vấn thông tin danh sách lãnh thổ
                KT_TCPHandler.SendGetGuildsColonyMaps();
            };
            this.UILocalMap.Show();
        }
    }
    #endregion
}
