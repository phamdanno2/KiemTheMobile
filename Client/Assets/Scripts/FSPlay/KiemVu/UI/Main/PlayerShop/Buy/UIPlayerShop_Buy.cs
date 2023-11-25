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
    public class UIPlayerShop_Buy : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Prefab vật phẩm được bán
        /// </summary>
        [SerializeField]
        private UIPlayerShop_Buy_Item UIItem_Prefab;

        /// <summary>
        /// Khung xác nhận mua vật phẩm
        /// </summary>
        [SerializeField]
        private UIPlayerShop_Buy_ConfirmBuyFrame UI_BuyItemConfirmFrame;
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
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện mua vật phẩm
        /// </summary>
        public Action<GoodsData> Buy { get; set; }
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
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi vật phẩm trong cửa hàng được chọn
        /// </summary>
        /// <param name="uiBuyItem"></param>
        private void ButtonItem_Clicked(UIPlayerShop_Buy_Item uiBuyItem)
        {
            this.UI_BuyItemConfirmFrame.Data = uiBuyItem.Data;
            this.UI_BuyItemConfirmFrame.Price = uiBuyItem.Price;
            this.UI_BuyItemConfirmFrame.Buy = () => {
                this.Buy?.Invoke(uiBuyItem.Data);
                this.UI_BuyItemConfirmFrame.Hide();
            };
            this.UI_BuyItemConfirmFrame.Show();
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
        private UIPlayerShop_Buy_Item FindItemSlot(GoodsData itemGD)
        {
            foreach (Transform child in this.transformItemsList.transform)
            {
                if (child.gameObject != this.UIItem_Prefab.gameObject)
                {
                    UIPlayerShop_Buy_Item uiBuyItem = child.gameObject.GetComponent<UIPlayerShop_Buy_Item>();
                    if (uiBuyItem.Data.Id == itemGD.Id)
                    {
                        return uiBuyItem;
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
            UIPlayerShop_Buy_Item uiBuyItem = this.FindItemSlot(itemGD);
            /// Nếu ô cũ tồn tại thì Update giá trị vào
            if (uiBuyItem != null)
            {
                uiBuyItem.Data = itemGD;
                uiBuyItem.Price = price;
                uiBuyItem.Buy = () => {
                    this.ButtonItem_Clicked(uiBuyItem);
                };
                return;
            }

            /// Tạo mới UI
            uiBuyItem = GameObject.Instantiate<UIPlayerShop_Buy_Item>(this.UIItem_Prefab);
            uiBuyItem.transform.SetParent(this.transformItemsList, false);
            uiBuyItem.gameObject.SetActive(true);
            /// Thiết lập giá trị
            uiBuyItem.Data = itemGD;
            uiBuyItem.Price = price;
            uiBuyItem.Buy = () => {
                this.ButtonItem_Clicked(uiBuyItem);
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
                /// Dữ liệu cửa hàng của đối phương
                StallData targetShopData = Global.Data.OtherStallDataItem;

                /// Nếu không có cửa hàng
                if (targetShopData == null)
                {
                    this.ButtonClose_Clicked();
                    return;
                }

                /// Duyệt danh sách vật phẩm và thêm vào
                if (targetShopData.GoodsList != null && targetShopData.GoodsPriceDict != null)
                {
                    foreach (GoodsData itemGD in targetShopData.GoodsList)
                    {
                        if (targetShopData.GoodsPriceDict.TryGetValue(itemGD.Id, out int price))
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
