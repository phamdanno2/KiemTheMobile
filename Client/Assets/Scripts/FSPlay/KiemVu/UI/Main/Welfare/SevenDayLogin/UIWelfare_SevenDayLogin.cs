using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.UI.Main.Welfare.SevenDayLogin;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.Welfare
{
    /// <summary>
    /// Khung quà đăng nhập 7 ngày
    /// </summary>
    public class UIWelfare_SevenDayLogin : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Prefab ô vật phẩm thưởng
        /// </summary>
        [SerializeField]
        private UIWelfare_SevenDayLogin_SlotItemBox UIItem_Prefab;

        /// <summary>
        /// Button nhận thưởng
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Get;

        /// <summary>
        /// Text thông tin đã Online
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_OnlineDetails;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách vật phẩm
        /// </summary>
        private RectTransform transformItemsList = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện nhận thưởng
        /// </summary>
        public Action<SevenDaysLoginItem> Get { get; set; }

        /// <summary>
        /// Sự kiện gửi yêu cầu lấy thông tin đăng nhập 7 ngày
        /// </summary>
        public Action QueryGetSevenDayLoginInfo { get; set; }

        /// <summary>
        /// Dữ liệu đăng nhập 7 ngày
        /// </summary>
        public SevenDaysLogin Data { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformItemsList = this.UIItem_Prefab.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            /// Gửi yêu cầu truy vấn phúc lợi đăng nhập 7 ngày
            this.QueryGetSevenDayLoginInfo?.Invoke();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Get.onClick.AddListener(this.ButtonGet_Clicked);
            /// Ẩn button nhận
            this.UIButton_Get.interactable = false;
        }

        /// <summary>
        /// Sự kiện khi Button nhận được ấn
        /// </summary>
        private void ButtonGet_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null)
            {
                KTGlobal.AddNotification("Không có gì để nhận!");
                return;
            }
            /// Nếu không có gì để nhận
            else if (this.Data.CurrentAwardInfo == null)
            {
                KTGlobal.AddNotification("Không có gì để nhận!");
                return;
            }

            /// Thực hiện sự kiện quay nhận
            this.Get?.Invoke(this.Data.CurrentAwardInfo);
            /// Ẩn button nhận
            this.UIButton_Get.interactable = false;
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
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformItemsList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách vật phẩm
        /// </summary>
        private void ClearItems()
        {
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Thêm phần thưởng mốc tương ứng
        /// </summary>
        /// <param name="awardInfo"></param>
        private void AddAward(SevenDaysLoginItem awardInfo)
        {
            /// Danh sách vật phẩm Random
            List<GoodsData> randomItems = new List<GoodsData>();

            /// Đã nhận chưa
            bool alreadyGotten = awardInfo.Days <= this.Data.SeriesLoginGetAwardStep;
            /// Nếu đã nhận rồi thì lấy ra thông tin vật phẩm đã nhận tương ứng
            if (alreadyGotten && this.Data.GetReceivedAwardItemInfo(awardInfo.Days, out int itemID, out int itemQuantity))
            {
                /// Nếu vật phẩm tồn tại
                if (Loader.Loader.Items.TryGetValue(itemID, out ItemData itemData))
                {
                    randomItems.Clear();
                    /// Tạo mới vật phẩm
                    GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
                    itemGD.Binding = 1;
                    itemGD.GCount = itemQuantity;
                    randomItems.Add(itemGD);
                }
            }
            else
            {
                /// Duyệt danh sách Random và thêm vào
                foreach (RollAwardItem awardItem in awardInfo.RollAwardItem)
                {
                    /// Nếu vật phẩm tồn tại
                    if (Loader.Loader.Items.TryGetValue(awardItem.ItemID, out ItemData itemData))
                    {
                        /// Tạo mới vật phẩm
                        GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
                        itemGD.Binding = 1;
                        itemGD.GCount = awardItem.Number;
                        randomItems.Add(itemGD);
                    }
                }
            }

            UIWelfare_SevenDayLogin_SlotItemBox uiSlotItem = GameObject.Instantiate<UIWelfare_SevenDayLogin_SlotItemBox>(this.UIItem_Prefab);
            uiSlotItem.transform.SetParent(this.transformItemsList, false);
            uiSlotItem.gameObject.SetActive(true);
            uiSlotItem.CanGet = awardInfo.Days == this.Data.SeriesLoginGetAwardStep + 1;
            uiSlotItem.AlreadyGotten = alreadyGotten;
            uiSlotItem.Items = randomItems;
            uiSlotItem.Day = awardInfo.Days;
            uiSlotItem.Refresh();
        }

        /// <summary>
        /// Tìm thông tin phần quà có day tương ứng
        /// </summary>
        /// <param name="day"></param>
        private UIWelfare_SevenDayLogin_SlotItemBox FindAward(int day)
        {
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    UIWelfare_SevenDayLogin_SlotItemBox uiSlotItem = child.GetComponent<UIWelfare_SevenDayLogin_SlotItemBox>();
                    if (uiSlotItem.Day == day)
                    {
                        return uiSlotItem;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Làm mới dữ liệu
        /// </summary>
        public void Refresh()
        {
            /// Làm rỗng danh sách vật phẩm
            this.ClearItems();
            /// Ẩn button nhận
            this.UIButton_Get.interactable = false;
            /// Làm rỗng Text trạng thái Online
            this.UIText_OnlineDetails.text = "";

            /// Nếu không có dữ liệu
            if (this.Data == null)
            {
                KTGlobal.AddNotification("Có lỗi khi tải dữ liệu phúc lợi đăng nhập 7 ngày, hãy báo hỗ trợ để được trợ giúp!");
                this.UIText_OnlineDetails.text = "Có lỗi khi tải dữ liệu phúc lợi đăng nhập 7 ngày, hãy báo hỗ trợ để được trợ giúp!";
                PlayZone.GlobalPlayZone.HideUIWelfare();
                return;
            }

            /// Nếu sự kiện chưa mở
            if (!this.Data.IsOpen)
            {
                KTGlobal.AddNotification("Phúc lợi này chưa được mở!");
                this.UIText_OnlineDetails.text = "Phúc lợi này chưa được mở!";
                return;
            }

            /// Cập nhật thông tin tổng thời gian đã đăng nhập
            this.UIText_OnlineDetails.text = string.Format("Bạn đã đăng nhập liên tiếp <color=green>{0} ngày</color>.{1}", this.Data.SeriesLoginNum, this.Data.HasSomethingToGet ? "Ấn <color=green>Nhận</color> để nhận quà." : "");

            /// Xây danh sách quà
            foreach (SevenDaysLoginItem itemInfo in this.Data.SevenDaysLoginItem)
            {
                /// Tạo ô quà mốc tương ứng
                this.AddAward(itemInfo);
            }

            /// Nếu đang mở khung phúc lợi
            if (PlayZone.GlobalPlayZone.UIWelfare != null)
            {
                /// Hint có quà ở khung phúc lợi Online
                PlayZone.GlobalPlayZone.UIWelfare.HintLogin(this.Data.HasSomethingToGet);
            }

            /// Nếu tồn tại quà có thể nhận thì làm sáng Button nhận
            this.UIButton_Get.interactable = this.Data.HasSomethingToGet;

            /// Xây lại giao diện
            this.RebuildLayout();
        }

        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="loginNum"></param>
        /// <param name="stepID"></param>
        public void UpdateState(int loginNum, int stepID)
        {
            this.Data.SeriesLoginNum = loginNum;
            this.Data.SeriesLoginGetAwardStep = stepID;

            /// Cập nhật trạng thái cho toàn bộ vật phẩm bên trong
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    UIWelfare_SevenDayLogin_SlotItemBox uiSlotItem = child.GetComponent<UIWelfare_SevenDayLogin_SlotItemBox>();
                    /// Đã nhận chưa
                    bool alreadyGotten = uiSlotItem.Day <= this.Data.SeriesLoginGetAwardStep;
                    uiSlotItem.CanGet = uiSlotItem.Day == this.Data.SeriesLoginGetAwardStep + 1;
                    uiSlotItem.AlreadyGotten = alreadyGotten;
                    uiSlotItem.Refresh();
                }
            }

            /// Cập nhật thông tin tổng thời gian đã đăng nhập
            this.UIText_OnlineDetails.text = string.Format("Bạn đã đăng nhập liên tiếp <color=green>{0} ngày</color>.{1}", this.Data.SeriesLoginNum, this.Data.HasSomethingToGet ? "Ấn <color=green>Nhận</color> để nhận quà." : "");

            /// Nếu đang mở khung phúc lợi
            if (PlayZone.GlobalPlayZone.UIWelfare != null)
            {
                /// Hint có quà ở khung phúc lợi Online
                PlayZone.GlobalPlayZone.UIWelfare.HintLogin(this.Data.HasSomethingToGet);
            }

            /// Nếu tồn tại quà có thể nhận thì làm sáng Button nhận
            this.UIButton_Get.interactable = this.Data.HasSomethingToGet;
        }

        /// <summary>
        /// Thực hiện quay quà ở mốc tương ứng
        /// </summary>
        /// <param name="stepIndex"></param>
        /// <param name="itemID"></param>
        /// <param name="itemNumber"></param>
        public void StartRoll(int stepIndex, int itemID, int itemNumber)
        {
            /// Ô quà tương ứng
            UIWelfare_SevenDayLogin_SlotItemBox uiItemBox = this.FindAward(stepIndex);
            /// Nếu không tồn tại
            if (uiItemBox == null)
            {
                return;
            }
            /// Thực hiện quay
            uiItemBox.Play(itemID, itemNumber);
        }
        #endregion
    }
}
