﻿using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.ItemBox
{
    /// <summary>
    /// Ô chứa vật phẩm
    /// </summary>
    public class UIItemBox : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Đối tượng Button
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton;

        /// <summary>
        /// Icon vật phẩm
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_ItemIcon;

        /// <summary>
        /// Icon biểu tượng không đủ điều kiện sử dụng vật phẩm
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_LockIcon;

        /// <summary>
        /// Icon vật phẩm không khóa
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_UnboundIcon;

        /// <summary>
        /// Icon biểu tượng ô vật phẩm chưa được mở
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_UnopenedIcon;

        /// <summary>
        /// Text loại ô vật phẩm (nếu có vật phẩm thì Text sẽ biến mất)
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ItemType;

        /// <summary>
        /// Text số lượng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Quantity;

        /// <summary>
        /// Text độ bền
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Durability;

        /// <summary>
        /// Text hạn sử dụng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Duration;

        /// <summary>
        /// Hiệu ứng cường hóa 1
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIAnimatedImage_EnhanceEffect_1;

        /// <summary>
        /// Hiệu ứng cường hóa 2
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIAnimatedImage_EnhanceEffect_2;

        /// <summary>
        /// Hiệu ứng cường hóa 3
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIAnimatedImage_EnhanceEffect_3;

        /// <summary>
        /// Hiệu ứng cường hóa 4
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIAnimatedImage_EnhanceEffect_4;

        /// <summary>
        /// Hiệu ứng sử dụng vật phẩm thành công
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIAnimatedImage_UseItemSuccessfully;
        #endregion


        #region Properties
        private GoodsData _Data = null;
        /// <summary>
        /// Dữ liệu vật phẩm
        /// </summary>
        public GoodsData Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;
                this.DoRefresh();
            }
        }

        /// <summary>
        /// Ô vật phẩm này đã được mở chưa
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return this.UIImage_UnopenedIcon.gameObject.activeSelf;
            }
            set
            {
                this.UIImage_UnopenedIcon.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Đường dẫn file Bundle chứa Icon
        /// </summary>
        private string IconBundleDir
        {
            set
            {
                this.UIImage_ItemIcon.BundleDir = value;
            }
        }

        /// <summary>
        /// Tên Atlas chứa Icon
        /// </summary>
        private string IconAtlasName
        {
            set
            {
                this.UIImage_ItemIcon.AtlasName = value;
            }
        }

        /// <summary>
        /// Tên Sprite Icon của vật phẩm
        /// </summary>
        private string IconSpriteName
        {
            set
            {
                this.UIImage_ItemIcon.SpriteName = value;
            }
        }

        private int _Quantity;
        /// <summary>
        /// Số lượng vật phẩm (nếu = 1 thì không hiện)
        /// </summary>
        private int Quantity
        {
            set
            {
                this._Quantity = value;
                this.UIText_Quantity.text = value.ToString();
                this.UIText_Quantity.gameObject.SetActive(value > 1);
            }
        }

        private int _Duration = -1;
        /// <summary>
        /// Thời hạn sử dụng (tính theo giây)
        /// </summary>
        private int Duration
        {
            set
            {
                this._Duration = value;
                this.UpdateDurationText();
            }
        }

        private int _Durability = -1;
        /// <summary>
        /// Độ bền
        /// </summary>
        private int Durability
        {
            set
            {
                this._Durability = value;
                this.UpdateDurability();
            }
        }

        private int _EnhanceType = 0;
        /// <summary>
        /// Loại cường hóa (0: không có, 1: Cường hóa màu cam, 2: Cường hóa màu cam chói, 3: Cường hóa màu vàng, 4: Cường hóa màu vàng chói)
        /// </summary>
        private int EnhanceType
        {
            set
            {
                this._EnhanceType = value;
                this.UIAnimatedImage_EnhanceEffect_1.gameObject.SetActive(value == 1);
                this.UIAnimatedImage_EnhanceEffect_2.gameObject.SetActive(value == 2);
                this.UIAnimatedImage_EnhanceEffect_3.gameObject.SetActive(value == 3);
                this.UIAnimatedImage_EnhanceEffect_4.gameObject.SetActive(value == 4);
            }
        }

        /// <summary>
        /// Hiển thị loại ô vật phẩm (nếu có vật phẩm tồn tại thì dòng chữ này biến mất)
        /// </summary>
        public string TypeText
        {
            get
            {
                return this.UIText_ItemType.text;
            }
            set
            {
                this.UIText_ItemType.text = value;
            }
        }

        /// <summary>
        /// Vật phẩm có thể sử dụng không
        /// </summary>
        private bool CanBeUsed
        {
            set
            {
                this.UIImage_LockIcon.gameObject.SetActive(!value);
            }
        }

        /// <summary>
        /// Vật phẩm có khóa không
        /// </summary>
        private bool Bound
        {
            set
            {
                this.UIImage_UnboundIcon.gameObject.SetActive(!value);
            }
        }

        /// <summary>
        /// Sự kiện khi đối tượng được Click
        /// </summary>
        public Action Click { get; set; }
        #endregion


        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            if (this.Click == null)
            {
                this.Click = () => {
                    KTGlobal.ShowItemInfo(this.Data);
                };
            }
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.Refresh();
            this.InitPrefab();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefab()
        {
            this.UIButton.onClick.AddListener(this.UIButton_Clicked);
            
        }

        /// <summary>
        /// Sự kiện khi Button vật phẩm được ấn
        /// </summary>
        private void UIButton_Clicked()
        {
            this.Click?.Invoke();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Cập nhật hiển thị thời gian vật phẩm
        /// </summary>
        private void UpdateDurationText()
        {
            string text;
            if (this._Duration == -1)
            {
                text = "";
                this.UIText_Duration.gameObject.SetActive(false);
            }
            else if (this._Duration <= 3600)
            {
                text = "< 1h";
                this.UIText_Duration.gameObject.SetActive(true);
            }
            else if (this._Duration < 86400)
            {
                text = string.Format("{0}h", this._Duration / 3600);
                this.UIText_Duration.gameObject.SetActive(true);
            }
            else
            {
                text = string.Format("{0}d", this._Duration / 86400);
                this.UIText_Duration.gameObject.SetActive(true);
            }

            this.UIText_Duration.text = text;
        }

        /// <summary>
        /// Cập nhật hiển thị độ bền
        /// </summary>
        private void UpdateDurability()
        {
            string text;
            if (this._Durability == -1)
            {
                text = "";
                this.UIText_Durability.gameObject.SetActive(false);
            }
            else if (this._Durability >= 70)
            {
                text = string.Format("<color={0}>{1}%</color>", "#57ff29", this._Durability);
                this.UIText_Durability.gameObject.SetActive(true);
            }
            else if (this._Durability >= 30)
            {
                text = string.Format("<color={0}>{1}%</color>", "#ffbf29", this._Durability);
                this.UIText_Durability.gameObject.SetActive(true);
            }
            else
            {
                text = string.Format("<color={0}>{1}%</color>", "#ff2929", this._Durability);
                this.UIText_Durability.gameObject.SetActive(true);
            }

            this.UIText_Durability.text = text;
        }

        /// <summary>
        /// Làm mới đối tượng
        /// </summary>
        private void DoRefresh()
        {
            /// Mặc định không có hiệu ứng cường hóa
            this.EnhanceType = 0;

            /// Nếu vật phẩm không tồn tại
            if (this._Data == null)
            {
                this.UIText_ItemType.gameObject.SetActive(true);
                this.UIImage_LockIcon.gameObject.SetActive(false);
                this.UIImage_ItemIcon.gameObject.SetActive(false);
                this.UIText_Quantity.gameObject.SetActive(false);
                this.UIText_Duration.gameObject.SetActive(false);
                this.UIText_Durability.gameObject.SetActive(false);
                this.UIImage_UnboundIcon.gameObject.SetActive(false);
                return;
            }
            if (!Loader.Loader.Items.TryGetValue(this._Data.GoodsID, out Entities.Config.ItemData itemData))
            {
                this.UIText_ItemType.gameObject.SetActive(true);
                this.UIImage_LockIcon.gameObject.SetActive(false);
                this.UIImage_ItemIcon.gameObject.SetActive(false);
                this.UIText_Quantity.gameObject.SetActive(false);
                this.UIText_Duration.gameObject.SetActive(false);
                this.UIText_Durability.gameObject.SetActive(false);
                this.UIImage_UnboundIcon.gameObject.SetActive(false);
                return;
            }

            this.UIImage_LockIcon.gameObject.SetActive(false);
            this.UIImage_ItemIcon.gameObject.SetActive(true);
            this.UIText_ItemType.gameObject.SetActive(false);
            this.UIText_Duration.gameObject.SetActive(true);
            this.IconBundleDir = itemData.IconBundleDir;
            this.IconAtlasName = itemData.IconAtlasName;
            this.IconSpriteName = itemData.Icon;
            try
            {
                this.UIImage_ItemIcon.Load();
            }
            catch (Exception ex)
            {
                KTDebug.LogError(ex.ToString());

                /// Load icon mặc định
                this.UIImage_ItemIcon.SpriteName = "hoicham";
                this.UIImage_ItemIcon.BundleDir = "Icon/EquipIcon1.unity3d";
                this.UIImage_ItemIcon.AtlasName = "EquipIcon1";
                this.UIImage_ItemIcon.Load();
            }
            /// Nếu là trang bị
            if (itemData.IsEquip)
            {
                this.CanBeUsed = KTGlobal.IsCanUseEquip(this._Data);
            }
            else
            {
                this.CanBeUsed = true;
            }

            /// Nếu là trang bị
            if (itemData.IsEquip)
            {
                this.UIText_Quantity.gameObject.SetActive(true);
                this.UIText_Durability.gameObject.SetActive(true);
                this.Durability = this._Data.Strong;
                this.Quantity = 1;
                /// Loại cường hóa
                this.EnhanceType = KTGlobal.GetWeaponEnhanceType(this._Data);
                this.UpdateDurability();
            }
            /// Nếu là vật phẩm
            else
            {
                this.UIText_Quantity.gameObject.SetActive(false);
                this.UIText_Durability.gameObject.SetActive(false);
                this.Quantity = this._Data.GCount;
            }

            /// Nếu không có thời hạn
            if (string.IsNullOrEmpty(this._Data.Endtime))
            {
                this.Duration = -1;
            }
            else
            {
                DateTime endTime = DateTime.Parse(this._Data.Endtime);
                if (endTime > DateTime.Now)
                {
                    TimeSpan diff = endTime - DateTime.Now;
                    int diffSec = (int) diff.TotalSeconds;
                    this.Duration = diffSec;
                }
                else
                {
                    this.Duration = -1;
                }
            }

            /// Nếu không phải trang bị
            if (!itemData.IsEquip)
            {
                /// Đánh dấu vật phẩm khóa hay không khóa
                this.Bound = this.Data.Binding == 1;
            }
            else
            {
                this.UIImage_UnboundIcon.gameObject.SetActive(false);
            }
            
            this.UpdateDurationText();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Làm mới đối tượng
        /// </summary>
        public void Refresh()
        {
            this.DoRefresh();
        }

        /// <summary>
        /// Làm mới số lượng
        /// </summary>
        public void RefreshQuantity()
        {
            this.Quantity = this._Data.GCount;
        }

        /// <summary>
        /// Thiết lập có hiển thị số lượng không
        /// </summary>
        /// <param name="isShow"></param>
        public void SetShowQuantity(bool isShow)
        {
            this.UIText_Quantity.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// Thực thi hiệu ứng dùng vật phẩm thành công
        /// </summary>
        public void PlayUseItemSuccessfullyEffect()
		{
            this.UIAnimatedImage_UseItemSuccessfully.Play();
		}
        #endregion
    }
}