using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.Main.Bag;
using FSPlay.KiemVu.UI.Main.Exchange;
using Server.Data;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Utilities.UnityUI;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung giao dịch
    /// </summary>
    public class UIExchange : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;
        
        /// <summary>
        /// Lưới túi đồ
        /// </summary>
        [SerializeField]
        private UIBag_Grid UIBagGrid;

        /// <summary>
        /// Lưới ô giao dịch của bản thân
        /// </summary>
        [SerializeField]
        private UIExchange_ExchangeBagGrid UIExchangeBag_SelfBag;

        /// <summary>
        /// Text số đồng bản thân giao dịch
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_SelfMoney;

        /// <summary>
        /// Button xác nhận
        /// </summary>
        [SerializeField]
        private UIButtonSprite UIButton_SelfConfirm;

        /// <summary>
        /// Button thêm tiền
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_InputMoney;

        /// <summary>
        /// Lưới ô giao dịch của đối phương
        /// </summary>
        [SerializeField]
        private UIExchange_ExchangeBagGrid UIExchangeBag_PartnerBag;

        /// <summary>
        /// Ký hiệu thông báo đối phương xác nhận không
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_PartnerConfirm;

        /// <summary>
        /// Text số đồng bản thân giao dịch
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PartnerMoney;

        /// <summary>
        /// Text phí giao dịch
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ExchangeTax;

        /// <summary>
        /// Button giao dịch
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Exchange;

        /// <summary>
        /// Sprite button xác nhận
        /// </summary>
        [SerializeField]
        private string ButtonConfirm_NormalSprite;

        /// <summary>
        /// Sprite button xác nhận ở trạng thái kích hoạt
        /// </summary>
        [SerializeField]
        private string ButtonConfirm_ActiveSprite;
        #endregion

        #region Constants
        /// <summary>
        /// % tiền thuế phải chịu
        /// </summary>
        private const int ExchangeMoneyTax = 10;
        #endregion

        #region Private fields
        
        #endregion

        #region Properties
        /// <summary>
        /// ID đối phương
        /// </summary>
        public int PartnerID { get; set; } = -1;

        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện xác nhận
        /// </summary>
        public Action Confirm { get; set; }

        /// <summary>
        /// Thêm vật phẩm vào sàn giao dịch
        /// </summary>
        public Action<GoodsData> AddItemToExchangeField { get; set; }

        /// <summary>
        /// Xóa vật phẩm khỏi sàn giao dịch
        /// </summary>
        public Action<GoodsData> RemoveItemFromExchangeField { get; set; }

        /// <summary>
        /// Thay đổi giá trị tiền đặt lên sàn giao dịch
        /// </summary>
        public Action<int> AddMoneyToExchangeField { get; set; }

        /// <summary>
        /// Sự kiện giao dịch
        /// </summary>
        public Action Exchange { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.UIButton_Exchange.interactable = false;
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_InputMoney.onClick.AddListener(this.ButtonInputMoney_Clicked);
            this.UIButton_SelfConfirm.Click = this.ButtonConfirm_Clicked;
            this.UIButton_Exchange.onClick.AddListener(this.ButtonExchange_Clicked);
            this.UIBagGrid.BagItemClicked = (itemGD) => {
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
                        this.AddItemToExchangeField?.Invoke(itemGD);
                        KTGlobal.CloseItemInfo();
                    }));
                    KTGlobal.ShowItemInfo(itemGD, buttons);
                }
            };
            this.UIExchangeBag_SelfBag.ItemClick = (itemGD) => {
                /// Dữ liệu vật phẩm
                if (!Loader.Loader.Items.TryGetValue(itemGD.GoodsID, out Entities.Config.ItemData itemData))
                {
                    return;
                }

                List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
                buttons.Add(new KeyValuePair<string, Action>("Gỡ xuống", () => {
                    this.RemoveItemFromExchangeField?.Invoke(itemGD);
                    KTGlobal.CloseItemInfo();
                }));
                KTGlobal.ShowItemInfo(itemGD, buttons);
            };
            this.UIExchangeBag_PartnerBag.ItemClick = (itemGD) => {
                KTGlobal.ShowItemInfo(itemGD);
            };
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button thêm bạc được ấn
        /// </summary>
        private void ButtonInputMoney_Clicked()
        {
            KTGlobal.ShowInputNumber("Nhập số tiền bạc giao dịch.", 0, Global.Data.RoleData.Money, 0, (value) => {
                this.AddMoneyToExchangeField?.Invoke(value);
            });
        }

        /// <summary>
        /// Sự kiện khi Button khóa được ấn
        /// </summary>
        private void ButtonConfirm_Clicked()
        {
            /// Nếu không có dữ liệu giao dịch
            if (Global.Data.ExchangeDataItem == null)
            {
                KTGlobal.AddNotification("Dữ liệu phiên giao dịch không tồn tại!");
                return;
            }
            /// ID bản thân
            int roleID = Global.Data.RoleData.RoleID;
            /// ID đối phương
            int partnerRoleID = this.PartnerID;

            /// Thông tin giao dịch
            ExchangeData exchangeData = Global.Data.ExchangeDataItem;
            /// Nếu bản thân không phải đối tượng của phiên
            if (exchangeData.RequestRoleID != roleID && exchangeData.AgreeRoleID != roleID)
            {
                KTGlobal.AddNotification("Dữ liệu phiên giao dịch bị lỗi!");
                return;
            }
            else if (exchangeData.RequestRoleID != partnerRoleID && exchangeData.AgreeRoleID != partnerRoleID)
            {
                KTGlobal.AddNotification("Dữ liệu phiên giao dịch bị lỗi!");
                return;
            }

            this.Confirm?.Invoke();
            /// Khóa Button không cho ấn nữa
            this.UIButton_SelfConfirm.Enable = false;
            this.UIButton_InputMoney.interactable = false;
        }

        /// <summary>
        /// Sự kiện khi Button giao dịch được ấn
        /// </summary>
        private void ButtonExchange_Clicked()
        {
            /// Nếu không có dữ liệu giao dịch
            if (Global.Data.ExchangeDataItem == null)
            {
                KTGlobal.AddNotification("Dữ liệu phiên giao dịch không tồn tại!");
                return;
            }
            /// ID bản thân
            int roleID = Global.Data.RoleData.RoleID;
            /// ID đối phương
            int partnerRoleID = this.PartnerID;

            /// Thông tin giao dịch
            ExchangeData exchangeData = Global.Data.ExchangeDataItem;

            /// Nếu cả hai đã xác nhận
            if (exchangeData.LockDict[roleID] == 1 && exchangeData.LockDict[partnerRoleID] == 1)
            {
                this.Exchange?.Invoke();
                this.UIButton_Exchange.interactable = false;
            }
            /// Nếu một trong 2 chưa xác nhận
            else
            {
                KTGlobal.AddNotification("Cần cả 2 bên xác nhận mới có thể tiến hành giao dịch!");
            }
        }
        #endregion

        #region Private methods
        
        #endregion

        #region Public methods
        /// <summary>
        /// Cập nhật thông tin đối phương
        /// </summary>
        /// <param name="inputMoney"></param>
        /// <param name="inputItems"></param>
        /// <param name="confirmed"></param>
        public void RefreshExchangeData()
        {
            /// Nếu không có dữ liệu giao dịch
            if (Global.Data.ExchangeDataItem == null)
            {
                return;
            }

            /// ID bản thân
            int roleID = Global.Data.RoleData.RoleID;
            /// ID đối phương
            int partnerRoleID = this.PartnerID;

            /// Thông tin giao dịch
            ExchangeData exchangeData = Global.Data.ExchangeDataItem;
            /// Nếu bản thân không phải đối tượng của phiên
            if (exchangeData.RequestRoleID != roleID && exchangeData.AgreeRoleID != roleID)
            {
                return;
            }
            else if (exchangeData.RequestRoleID != partnerRoleID && exchangeData.AgreeRoleID != partnerRoleID)
            {
                return;
            }

            /// Trạng thái xác nhận của cả 2
            bool selfConfirmed = false;
            bool partnerConfirmed = false;

            /// Cập nhật thông tin bản thân
            {
                this.UIExchangeBag_SelfBag.Clear();

                /// Nếu danh sách vật phẩm của bản thân tồn tại
                if (exchangeData.GoodsDict != null && exchangeData.GoodsDict.TryGetValue(roleID, out List<GoodsData> goodsData))
                {
                    if (goodsData != null)
                    {
                        foreach (GoodsData itemGD in goodsData)
                        {
                            /// Thêm vật phẩm vào sàn giao dịch
                            this.UIExchangeBag_SelfBag.AddItem(itemGD);
                        }
                    }
                }

                /// Cập nhật số tiền
                if (exchangeData.MoneyDict != null && exchangeData.MoneyDict.TryGetValue(roleID, out int money))
                {
                    this.UIText_SelfMoney.text = KTGlobal.GetDisplayMoney(money);
                }
                else
                {
                    this.UIText_SelfMoney.text = "0";
                }

                /// Cập nhật trạng thái xác nhận
                if (exchangeData.LockDict != null && exchangeData.LockDict.TryGetValue(roleID, out int status))
                {
                    this.UIButton_SelfConfirm.NormalSpriteName = status == 0 ? this.ButtonConfirm_NormalSprite : this.ButtonConfirm_ActiveSprite;
                    this.UIButton_SelfConfirm.DisabledSpriteName = status == 0 ? this.ButtonConfirm_NormalSprite : this.ButtonConfirm_ActiveSprite;
                    this.UIButton_SelfConfirm.Refresh(true);
                    /// Cập nhật trạng thái xác nhận
                    selfConfirmed = status == 1;
                }
                else
                {
                    this.UIButton_SelfConfirm.NormalSpriteName = this.ButtonConfirm_NormalSprite;
                    this.UIButton_SelfConfirm.DisabledSpriteName = this.ButtonConfirm_NormalSprite;
                    this.UIButton_SelfConfirm.Refresh(true);
                }
            }

            /// Cập nhật thông tin đối phương
            {
                this.UIExchangeBag_PartnerBag.Clear();

                /// Nếu danh sách vật phẩm của bản thân tồn tại
                if (exchangeData.GoodsDict != null && exchangeData.GoodsDict.TryGetValue(partnerRoleID, out List<GoodsData> goodsData))
                {
                    if (goodsData != null)
                    {
                        foreach (GoodsData itemGD in goodsData)
                        {
                            /// Thêm vật phẩm vào sàn giao dịch
                            this.UIExchangeBag_PartnerBag.AddItem(itemGD);
                        }
                    }
                }

                /// Cập nhật số tiền
                if (exchangeData.MoneyDict != null && exchangeData.MoneyDict.TryGetValue(partnerRoleID, out int money))
                {
                    this.UIText_PartnerMoney.text = KTGlobal.GetDisplayMoney(money);
                    /// Phí mất
                    int tax = money * UIExchange.ExchangeMoneyTax / 100;
                    this.UIText_ExchangeTax.text = KTGlobal.GetDisplayMoney(tax);
                }
                else
                {
                    this.UIText_PartnerMoney.text = "0";
                    this.UIText_ExchangeTax.text = "0";
                }

                /// Cập nhật trạng thái xác nhận
                if (exchangeData.LockDict != null && exchangeData.LockDict.TryGetValue(partnerRoleID, out int status))
                {
                    this.UIImage_PartnerConfirm.SpriteName = status == 0 ? this.ButtonConfirm_NormalSprite : this.ButtonConfirm_ActiveSprite;
                    this.UIImage_PartnerConfirm.Load();
                    /// Cập nhật trạng thái xác nhận
                    partnerConfirmed = status == 1;
                }
                else
                {
                    this.UIImage_PartnerConfirm.SpriteName = this.ButtonConfirm_NormalSprite;
                    this.UIImage_PartnerConfirm.Load();
                }
            }

            /// Thiết lập trạng thái tương tác cho Button giao dịch
            this.UIButton_Exchange.interactable = selfConfirmed && partnerConfirmed;
        }
        #endregion
    }
}
