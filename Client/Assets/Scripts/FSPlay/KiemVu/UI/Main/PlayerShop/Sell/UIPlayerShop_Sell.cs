using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.Main.Bag;
using FSPlay.KiemVu.UI.Main.PlayerShop;
using System.Collections;
using Server.Data;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung bán hàng
    /// </summary>
    public class UIPlayerShop_Sell : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Túi đồ
        /// </summary>
        [SerializeField]
        private UIBag_Grid UIBagGrid;

        /// <summary>
        /// Prefab vật phẩm được bán
        /// </summary>
        [SerializeField]
        private UIPlayerShop_Sell_Item UIItem_Prefab;

        /// <summary>
        /// Khung thêm vật phẩm bán
        /// </summary>
        [SerializeField]
        private UIPlayerShop_Sell_AddItemToSellFrame UI_AddItemToSellFrame;

        /// <summary>
        /// RectTransform khung
        /// </summary>
        [SerializeField]
        private RectTransform UI_Frame;

        /// <summary>
        /// Button chuyển trạng thái ẩn hiện của khung cửa hàng
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ToggleStallVisible;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách vật phẩm bán
        /// </summary>
        private RectTransform transformItemsList = null;

        /// <summary>
        /// Luồng thực thi xây lại giao diện
        /// </summary>
        private Coroutine rebuildLayoutCoroutine = null;

        /// <summary>
        /// Vị trí đặt ban đầu của khung
        /// </summary>
        private Vector2 firstFrameOffset;

        /// <summary>
        /// Vị trí ẩn của khung
        /// </summary>
        private readonly Vector2 InvisibleFrameOffset = new Vector2(0, -99999);
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện thêm vật phẩm vào cửa hàng để bán
        /// </summary>
        public Action<GoodsData, int> AddItemToSell { get; set; }

        /// <summary>
        /// Xóa vật phẩm khỏi danh sách bán
        /// </summary>
        public Action<GoodsData> RemoveItemFromSell { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformItemsList = this.UIItem_Prefab.transform.parent.GetComponent<RectTransform>();
            this.firstFrameOffset = this.UI_Frame.anchoredPosition;
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.ClearItemsList();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIBagGrid.BagItemClicked = this.BagGridItem_Clicked;
            this.UIButton_ToggleStallVisible.onClick.AddListener(this.ButtonToggleStallVisible_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi vật phẩm trong túi đồ được ấn
        /// </summary>
        /// <param name="itemGD"></param>
        private void BagGridItem_Clicked(GoodsData itemGD)
        {
            /// Dữ liệu vật phẩm
            if (!Loader.Loader.Items.TryGetValue(itemGD.GoodsID, out Entities.Config.ItemData itemData))
            {
                return;
            }

            /// Nếu vật phẩm đã khóa
            if (itemGD.Binding == 1)
            {
                KTGlobal.ShowItemInfo(itemGD);
            }
            else
            {
                List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
                buttons.Add(new KeyValuePair<string, Action>("Đặt lên", () => {
                    this.UI_AddItemToSellFrame.Data = itemGD;
                    this.UI_AddItemToSellFrame.Sell = () => {
                        this.AddItemToSell?.Invoke(itemGD, this.UI_AddItemToSellFrame.Price);
                        this.UI_AddItemToSellFrame.Hide();
                    };

                    this.UI_AddItemToSellFrame.Show();
                    KTGlobal.CloseItemInfo();
                }));
                KTGlobal.ShowItemInfo(itemGD, buttons);
            }
        }

        /// <summary>
        /// Sự kiện khi vật phẩm trong danh sách bán được ấn
        /// </summary>
        /// <param name="uiSellItem"></param>
        private void SellItem_Clicked(UIPlayerShop_Sell_Item uiSellItem)
        {
            /// Dữ liệu vật phẩm
            if (!Loader.Loader.Items.TryGetValue(uiSellItem.Data.GoodsID, out Entities.Config.ItemData itemData))
            {
                return;
            }

            List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
            buttons.Add(new KeyValuePair<string, Action>("Gỡ xuống", () => {
                KTGlobal.ShowMessageBox("Gỡ vật phẩm", "Xác nhận gỡ vật phẩm khỏi cửa hàng?", () => {
                    this.RemoveItemFromSell?.Invoke(uiSellItem.Data);
                    KTGlobal.CloseItemInfo();
                }, true);
            }));
            KTGlobal.ShowItemInfo(uiSellItem.Data, buttons);
        }

        /// <summary>
        /// Sự kiện khi Button đổi trạng thái ẩn hiện của cửa hàng được ấn
        /// </summary>
        private void ButtonToggleStallVisible_Clicked()
        {
            /// Nếu đang ẩn
            if (this.UI_Frame.anchoredPosition == this.InvisibleFrameOffset)
            {
                /// Hiện
                this.UI_Frame.anchoredPosition = this.firstFrameOffset;
            }
            /// Nếu đang hiện
            else
            {
                /// Ẩn
                this.UI_Frame.anchoredPosition = this.InvisibleFrameOffset;
            }
        }
        #endregion

        #region Private fields
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
            if (this.rebuildLayoutCoroutine != null)
            {
                this.StopCoroutine(this.rebuildLayoutCoroutine);
            }
            this.rebuildLayoutCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformItemsList);
                this.rebuildLayoutCoroutine = null;
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách vật phẩm
        /// </summary>
        private void ClearItemsList()
        {
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            this.RebuildLayout();
        }

        /// <summary>
        /// Tìm ô chữa vật phẩm tương ứng
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        private UIPlayerShop_Sell_Item FindItemSlot(GoodsData itemGD)
        {
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    UIPlayerShop_Sell_Item uiSellItem = child.gameObject.GetComponent<UIPlayerShop_Sell_Item>();
                    if (uiSellItem.Data.Id == itemGD.Id)
                    {
                        return uiSellItem;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Thêm vật phẩm vào danh sách
        /// </summary>
        /// <param name="itemGD"></param>
        /// <param name="price"></param>
        private void AddItemToList(GoodsData itemGD, int price)
        {
            UIPlayerShop_Sell_Item uiSellItem = this.FindItemSlot(itemGD);
            /// Nếu ô cũ tồn tại thì Update giá trị vào
            if (uiSellItem != null)
            {
                uiSellItem.Data = itemGD;
                uiSellItem.Price = price;
                uiSellItem.ItemClick = () => {
                    this.SellItem_Clicked(uiSellItem);
                };
                return;
            }

            /// Tạo mới UI
            uiSellItem = GameObject.Instantiate<UIPlayerShop_Sell_Item>(this.UIItem_Prefab);
            uiSellItem.transform.SetParent(this.transformItemsList, false);
            uiSellItem.gameObject.SetActive(true);
            /// Thiết lập giá trị
            uiSellItem.Data = itemGD;
            uiSellItem.Price = price;
            uiSellItem.ItemClick = () => {
                this.SellItem_Clicked(uiSellItem);
            };

            /// Xây lại giao diện
            this.RebuildLayout();
        }
        #endregion

        #region Public fields
        /// <summary>
        /// Làm mới giao diện cửa hàng
        /// </summary>
        public void RefreshShop()
        {
            /// Xóa toàn bộ vật phẩm
            this.ClearItemsList();

            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                /// Dữ liệu cửa hàng của bản thân
                StallData myselfShopData = Global.Data.StallDataItem;

                /// Nếu không có cửa hàng
                if (myselfShopData == null)
                {
                    this.ButtonClose_Clicked();
                    return;
                }

                /// Duyệt danh sách vật phẩm và thêm vào
                if (myselfShopData.GoodsList != null && myselfShopData.GoodsPriceDict != null)
                {
                    foreach (GoodsData itemGD in myselfShopData.GoodsList)
                    {
                        if (myselfShopData.GoodsPriceDict.TryGetValue(itemGD.Id, out int price))
                        {
                            this.AddItemToList(itemGD, price);
                        }
                    }
                }
            }));
        }
        #endregion
    }
}
