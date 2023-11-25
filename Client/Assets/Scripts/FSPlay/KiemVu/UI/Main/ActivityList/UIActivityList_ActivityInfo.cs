using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.Main.ItemBox;
using Assets.Scripts.FSPlay.KiemVu.Entities.Config;
using System.Collections;
using Server.Data;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;

namespace FSPlay.KiemVu.UI.Main.ActivityList
{
    /// <summary>
    /// Thông tin hoạt động trong khung danh sách hoạt động
    /// </summary>
    public class UIActivityList_ActivityInfo : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text tên hoạt động
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ActivityName;

        /// <summary>
        /// Text thời gian hoạt động
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ActivityTime;

        /// <summary>
        /// Text yêu cầu cấp độ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ActivityLevel;

        /// <summary>
        /// Prefab ô vật phẩm
        /// </summary>
        [SerializeField]
        private UIItemBox UI_ItemPrefab;

        /// <summary>
        /// Button tham gia
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Join;

        /// <summary>
        /// Mark chưa mở
        /// </summary>
        [SerializeField]
        private RectTransform UIMark_Unopened;

        /// <summary>
        /// Mark đã kết thúc
        /// </summary>
        [SerializeField]
        private RectTransform UIMark_Ended;

        /// <summary>
        /// Mark đang diễn ra
        /// </summary>
        [SerializeField]
        private RectTransform UIMark_Opening;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách vật phẩm
        /// </summary>
        private RectTransform transformItemList = null;
        #endregion

        #region Properties
        private ActivityXML _Data;
        /// <summary>
        /// Thông tin hoạt động
        /// </summary>
        public ActivityXML Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;
                /// Làm mới hiển thị
                this.Refresh();
            }
        }
        #endregion

        #region Core MonoBahaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformItemList = this.UI_ItemPrefab.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.Refresh();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng bị hủy kích hoạt
        /// </summary>
        private void OnDisable()
        {
            this.ClearItemSlots();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Join.onClick.AddListener(this.ButtonJoin_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button tham gia được ấn
        /// </summary>
        private void ButtonJoin_Clicked()
        {
            /// Nếu có Tips thì hiện Tips
            if (!string.IsNullOrEmpty(this._Data.MsgTips))
            {
                KTGlobal.AddNotification(this._Data.MsgTips);
            }
            /// Nếu không có Tips thì cho chạy đến NPC
            else
            {
                /// Tự dịch chuyển đến NPC tương ứng
                KTGlobal.QuestAutoFindPathToNPC(this._Data.MapCode, this._Data.NPCID, () => {
                    AutoQuest.Instance.StopAutoQuest();
                    AutoPathManager.Instance.StopAutoPath();
                    GSprite sprite = KTGlobal.FindNearestNPCByResID(this._Data.NPCID);
                    if (sprite == null)
                    {
                        KTGlobal.AddNotification("Không tìm thấy NPC tương ứng!");
                        return;
                    }
                    Global.Data.TargetNpcID = sprite.RoleID - GameFramework.Logic.SpriteBaseIds.NpcBaseId;
                    Global.Data.GameScene.NPCClick(sprite);
                });
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi sự kiện bỏ qua một số Frame
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrames(int skip, Action work)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            work?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện danh sách vật phẩm
        /// </summary>
        private void RebuildLayout()
        {
            /// Nếu đối tượng không được kích hoạt
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            /// Thực thi xây lại giao diện ở Frame tiếp theo
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformItemList);
            }));
        }

        /// <summary>
        /// Tìm một vị trí trống trong danh sách các vật phẩm
        /// </summary>
        private UIItemBox FindEmptySlot()
        {
            foreach (Transform child in this.transformItemList.transform)
            {
                if (child.gameObject != this.UI_ItemPrefab.gameObject)
                {
                    UIItemBox uiItemBox = child.GetComponent<UIItemBox>();
                    if (uiItemBox.Data == null)
                    {
                        return uiItemBox;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Làm rỗng danh sách vật phẩm
        /// </summary>
        private void ClearItemSlots()
        {
            foreach (Transform child in this.transformItemList.transform)
            {
                if (child.gameObject != this.UI_ItemPrefab.gameObject)
                {
                    UIItemBox uiItemBox = child.GetComponent<UIItemBox>();
                    uiItemBox.Data = null;
                    uiItemBox.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Thêm vật phẩm vào danh sách
        /// </summary>
        /// <param name="itemGD"></param>
        private void AppendItem(GoodsData itemGD)
        {
            /// Tìm ô trống nào đó
            UIItemBox uiItemBox = this.FindEmptySlot();
            /// Nếu tìm thấy
            if (uiItemBox != null)
            {
                uiItemBox.gameObject.SetActive(true);
                uiItemBox.Data = itemGD;
            }
            /// Nếu không tìm thấy
            else
            {
                uiItemBox = GameObject.Instantiate<UIItemBox>(this.UI_ItemPrefab);
                uiItemBox.transform.SetParent(this.transformItemList, false);
                uiItemBox.gameObject.SetActive(true);
                uiItemBox.Data = itemGD;
            }
        }

        /// <summary>
        /// Làm mới
        /// </summary>
        private void Refresh()
        {
            /// Làm rỗng danh sách vật phẩm
            this.ClearItemSlots();
            /// Làm rỗng tên hoạt động
            this.UIText_ActivityName.text = "";
            /// Làm rỗng thời gian
            this.UIText_ActivityTime.text = "";
            /// Làm rỗng cấp độ yêu cầu
            this.UIText_ActivityLevel.text = "";
            /// Hủy trạng thái toàn bộ Button
            this.UIButton_Join.gameObject.SetActive(false);
            this.UIMark_Ended.gameObject.SetActive(false);
            this.UIMark_Unopened.gameObject.SetActive(false);
            this.UIMark_Opening.gameObject.SetActive(false);

            /// Nếu đối tượng chưa được kích hoạt
            if (!this.gameObject.activeSelf)
            {
                /// Bỏ qua
                return;
            }
            /// Nếu không có dữ liệu
            else if (this._Data == null)
            {
                return;
            }

            /// Duyệt danh sách phần quà
            foreach (int itemID in this._Data.ItemReward)
            {
                /// Nếu vật phẩm tồn tại
                if (Loader.Loader.Items.TryGetValue(itemID, out ItemData itemData))
                {
                    GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
                    /// Thêm vào danh sách
                    this.AppendItem(itemGD);
                }
            }

            /// Tên hoạt động
            this.UIText_ActivityName.text = this._Data.ActivityName;
            /// Thời gian diễn ra
            this.UIText_ActivityTime.text = this._Data.GetNearestActivityTimes();
            /// Cấp độ yêu cầu
            if (Global.Data.RoleData.Level < this._Data.LevelJoin)
            {
                this.UIText_ActivityLevel.text = string.Format("<color=red>Yêu cầu cấp {0}</color>", this._Data.LevelJoin);
            }
            else
            {
                this.UIText_ActivityLevel.text = string.Format("Yêu cầu cấp {0}", this._Data.LevelJoin);
            }
            /// Trạng thái hoạt động
            ActivityXML.ActivityState state = this._Data.GetActivityState();
            /// Xem trạng thái gì để cập nhật hiển thị
            switch (state)
            {
                case ActivityXML.ActivityState.NOTOPEN:
                {
                    this.UIButton_Join.gameObject.SetActive(false);
                    this.UIMark_Ended.gameObject.SetActive(false);
                    this.UIMark_Unopened.gameObject.SetActive(true);
                    this.UIMark_Opening.gameObject.SetActive(false);
                    break;
                }
                case ActivityXML.ActivityState.OPEN:
                {
                    this.UIButton_Join.gameObject.SetActive(false);
                    this.UIMark_Ended.gameObject.SetActive(false);
                    this.UIMark_Unopened.gameObject.SetActive(false);
                    this.UIMark_Opening.gameObject.SetActive(true);
                    break;
                }
                case ActivityXML.ActivityState.CANJOIN:
                {
                    this.UIButton_Join.gameObject.SetActive(true);
                    this.UIMark_Ended.gameObject.SetActive(false);
                    this.UIMark_Unopened.gameObject.SetActive(false);
                    this.UIMark_Opening.gameObject.SetActive(false);
                    break;
                }
                case ActivityXML.ActivityState.HASEND:
                {
                    this.UIButton_Join.gameObject.SetActive(false);
                    this.UIMark_Ended.gameObject.SetActive(true);
                    this.UIMark_Unopened.gameObject.SetActive(false);
                    this.UIMark_Opening.gameObject.SetActive(false);
                    break;
                }
            }

            /// Xây lại giao diện
            this.RebuildLayout();
        }
        #endregion
    }
}
