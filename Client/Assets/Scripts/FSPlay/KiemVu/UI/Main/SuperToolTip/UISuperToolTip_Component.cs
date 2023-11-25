﻿using FSPlay.KiemVu.UI.Main.ItemBox;
using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using static FSPlay.KiemVu.UI.Main.UISuperToolTip;

namespace FSPlay.KiemVu.UI.Main.SuperToolTip
{
    /// <summary>
    /// Đối tượng Tooltip khung thông tin trang bị, vật phẩm hoặc kỹ năng
    /// </summary>
    public class UISuperToolTip_Component : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Ô hiển thị ảnh vật phẩm
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIItemBox_Icon;

        /// <summary>
        /// Background ô Item
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_ItemBoxBackground;

        /// <summary>
        /// Tên vật phẩm hoặc kỹ năng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Title;

        /// <summary>
        /// Image Prefab sao
        /// </summary>
        [SerializeField]
        private UIEquipStarIcon UIImage_StarPrefab;

        /// <summary>
        /// Text mô tả ngắn
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ShortDescription;

        /// <summary>
        /// Text nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Content;

        /// <summary>
        /// Text tên tác giả
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_AuthorName;

        /// <summary>
        /// Text giá bán / mua
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Price;

        /// <summary>
        /// Prefab Button chức năng
        /// </summary>
        [SerializeField]
        private UIButtonSprite UIButton_FunctionPrefab;
        #endregion

        #region Properties
        /// <summary>
        /// Đường dẫn Bundle chứa Icon
        /// </summary>
        public string IconBundleDir
        {
            get
            {
                return this.UIItemBox_Icon.BundleDir;
            }
            set
            {
                this.UIItemBox_Icon.BundleDir = value;
            }
        }

        /// <summary>
        /// Tên Atlas chứa Icon
        /// </summary>
        public string IconAtlasName
        {
            get
            {
                return this.UIItemBox_Icon.AtlasName;
            }
            set
            {
                this.UIItemBox_Icon.AtlasName = value;
            }
        }

        /// <summary>
        /// Tên Sprite Icon
        /// </summary>
        public string IconSpriteName
        {
            get
            {
                return this.UIItemBox_Icon.SpriteName;
            }
            set
            {
                this.UIItemBox_Icon.SpriteName = value;
            }
        }

        /// <summary>
        /// Tự điều chỉnh kích thước ảnh sao cho vừa với kích thước ảnh gốc
        /// </summary>
        public bool IconPixelPerfect
        {
            get
            {
                return this.UIItemBox_Icon.PixelPerfect;
            }
            set
            {
                this.UIItemBox_Icon.PixelPerfect = value;
            }
        }

        /// <summary>
        /// Tiêu đề khung (tên vật phẩm hoặc kỹ năng)
        /// </summary>
        public string Title
        {
            get
            {
                return this.UIText_Title.text;
            }
            set
            {
                this.UIText_Title.text = value;
            }
        }

        /// <summary>
        /// Màu tiêu đề khung
        /// </summary>
        public Color TitleColor
        {
            get
            {
                return this.UIText_Title.color;
            }
            set
            {
                this.UIText_Title.color = value;
            }
        }

        /// <summary>
        /// Tổng số sao
        /// </summary>
        public float TotalStar { get; set; }

        /// <summary>
        /// Màu sao của trang bị
        /// </summary>
        public SuperToolTipEquipStarColor EquipStarColor { get; set; }

        /// <summary>
        /// Nội dung ToolTip
        /// </summary>
        public string Content
        {
            get
            {
                return this.UIText_Content.text;
            }
            set
            {
                this.UIText_Content.text = value;
            }
        }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        public string ShortDesc
        {
            get
            {
                return this.UIText_ShortDescription.text;
            }
            set
            {
                this.UIText_ShortDescription.text = value;
            }
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        public Action Close { get; set; }

        private bool _ShowItemBoxBackground = true;
        /// <summary>
        /// Hiển thị Background ô Item
        /// </summary>
        public bool ShowItemBoxBackground
        {
            get
            {
                return this._ShowItemBoxBackground;
            }
            set
            {
                this._ShowItemBoxBackground = value;

                Color color = this.UIImage_ItemBoxBackground.color;
                color.a = value ? 1f : 0f;
                this.UIImage_ItemBoxBackground.color = color;
            }
        }

        /// <summary>
        /// Tên tác giả
        /// </summary>
        public string AuthorName
        {
            get
            {
                return this.UIText_AuthorName.text;
            }
            set
            {
                this.UIText_AuthorName.text = value;
            }
        }

        /// <summary>
        /// Chuỗi ghi giá cả
        /// </summary>
        public string PriceText
        {
            get
            {
                return this.UIText_Price.text;
            }
            set
            {
                this.UIText_Price.text = value;
            }
        }

        private bool _ShowBottomDesc = false;
        /// <summary>
        /// Hiện khung mô tả bên dưới cùng
        /// </summary>
        public bool ShowBottomDesc
        {
            get
            {
                return this._ShowBottomDesc;
            }
            set
            {
                this._ShowBottomDesc = value;
                this.transformBottomDesc.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Danh sách Button chức năng
        /// </summary>
        public List<KeyValuePair<string, Action>> Buttons { get; set; } = new List<KeyValuePair<string, Action>>();

        private bool _ShowFunctionButtons = false;
        /// <summary>
        /// Hiện danh sách Buttons chức năng
        /// </summary>
        public bool ShowFunctionButtons
        {
            get
            {
                return this._ShowFunctionButtons;
            }
            set
            {
                this._ShowFunctionButtons = value;
                if (this.transformFunctionButtonsFrame != null)
                {
                    this.transformFunctionButtonsFrame.gameObject.SetActive(value);
                }
            }
        }
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform khung mô tả phía dưới
        /// </summary>
        private RectTransform transformBottomDesc = null;

        /// <summary>
        /// RectTransform danh sách Button chức năng
        /// </summary>
        private RectTransform transformButtonsBox = null;

        /// <summary>
        /// RectTransform danh sách Button chức năng
        /// </summary>
        private RectTransform transformFunctionButtonsFrame = null;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformBottomDesc = this.UIText_AuthorName.transform.parent.gameObject.GetComponent<RectTransform>();
            if (this.UIButton_FunctionPrefab != null)
			{
                this.transformButtonsBox = this.UIButton_FunctionPrefab.transform.parent.gameObject.GetComponent<RectTransform>();
                this.transformFunctionButtonsFrame = this.transformButtonsBox.parent.gameObject.GetComponent<RectTransform>();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực hiện sự kiện bỏ qua một số lượng Frame nhất định
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrame(int skip, Action function)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            function?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            this.StartCoroutine(this.ExecuteSkipFrame(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.UIImage_StarPrefab.transform.parent.GetComponent<RectTransform>());
            }));
        }

        /// <summary>
        /// Xóa rỗng khung chứa sao
        /// </summary>
        private void ClearStarBox()
        {
            foreach (Transform child in this.UIImage_StarPrefab.transform.parent)
            {
                if (child.gameObject != this.UIImage_StarPrefab.gameObject && child.gameObject != this.UIText_ShortDescription.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Xóa rỗng danh sách Button chức năng
        /// </summary>
        private void ClearFunctionButtons()
        {
            foreach (Transform child in this.transformButtonsBox.transform)
            {
                if (child.gameObject != this.UIButton_FunctionPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Hiện các Button chức năng
        /// </summary>
        private void BuildFunctionButtons()
        {
            foreach (KeyValuePair<string, Action> buttonInfo in this.Buttons)
            {
                UIButtonSprite uiButton = GameObject.Instantiate<UIButtonSprite>(this.UIButton_FunctionPrefab);
                uiButton.gameObject.SetActive(true);
                uiButton.transform.SetParent(this.transformButtonsBox, false);
                uiButton.Name = buttonInfo.Key;
                uiButton.Click = buttonInfo.Value;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Khởi tạo SuperToolTip
        /// </summary>
        public void Build()
        {
            this.ClearStarBox();
            if (this.TotalStar > 0)
            {
                for (int i = 1; i <= Math.Min(12, (int) this.TotalStar); i++)
                {
                    UIEquipStarIcon uiStar = GameObject.Instantiate<UIEquipStarIcon>(this.UIImage_StarPrefab);
                    uiStar.transform.SetParent(this.UIImage_StarPrefab.transform.parent, false);
                    uiStar.gameObject.SetActive(true);
                    switch (this.EquipStarColor)
                    {
                        case SuperToolTipEquipStarColor.Basic:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Basic;
                            break;
                        case SuperToolTipEquipStarColor.Blue:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Blue;
                            break;
                        case SuperToolTipEquipStarColor.Orange:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Orange;
                            break;
                        case SuperToolTipEquipStarColor.Purple:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Purple;
                            break;
                        case SuperToolTipEquipStarColor.Yellow:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Yellow;
                            break;
                    }
                }

                float left = this.TotalStar - (int) this.TotalStar;
                if (left >= 0.5)
                {
                    UIEquipStarIcon uiStar = GameObject.Instantiate<UIEquipStarIcon>(this.UIImage_StarPrefab);
                    uiStar.transform.SetParent(this.UIImage_StarPrefab.transform.parent, false);
                    uiStar.gameObject.SetActive(true);
                    switch (this.EquipStarColor)
                    {
                        case SuperToolTipEquipStarColor.Basic:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Half_Basic;
                            break;
                        case SuperToolTipEquipStarColor.Blue:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Half_Blue;
                            break;
                        case SuperToolTipEquipStarColor.Orange:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Half_Orange;
                            break;
                        case SuperToolTipEquipStarColor.Purple:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Half_Purple;
                            break;
                        case SuperToolTipEquipStarColor.Yellow:
                            uiStar.Type = UIEquipStarIcon.EquipStarType.Half_Yellow;
                            break;
                    }
                }
            }

            this.transformBottomDesc.gameObject.SetActive(this._ShowBottomDesc);

            if (this.UIButton_FunctionPrefab != null)
			{
                this.transformButtonsBox.gameObject.SetActive(this._ShowFunctionButtons);
                this.ClearFunctionButtons();
                if (this._ShowFunctionButtons)
                {
                    this.BuildFunctionButtons();
                }
            }

            this.RebuildLayout();

            try
            {
                this.UIItemBox_Icon.Load();
            }
            catch (Exception ex)
            {
                /// Load icon mặc định
                this.UIItemBox_Icon.SpriteName = "hoicham";
                this.UIItemBox_Icon.BundleDir = "Icon/EquipIcon1.unity3d";
                this.UIItemBox_Icon.AtlasName = "EquipIcon1";
                this.UIItemBox_Icon.Load();
            }
        }
        #endregion
    }
}
