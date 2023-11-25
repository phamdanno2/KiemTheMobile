using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.AutoFight
{
    /// <summary>
    /// Khung chọn vật phẩm
    /// </summary>
    public class UIAutoFight_SelectItem : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Prefab ô vật phẩm
        /// </summary>
        [SerializeField]
        private UIButtonSprite UIButton_ItemPrefab;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform chứa danh sách vật phẩm
        /// </summary>
        private RectTransform transformItemList = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện khi kỹ năng được chọn
        /// </summary>
        public Action<ItemData> ItemSelected { get; set; }

        /// <summary>
        /// Có đang hiển thị khung
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }

        /// <summary>
        /// Danh sách vật phẩm tương ứng cần chọn
        /// </summary>
        public List<ItemData> Items { get; set; } = new List<ItemData>();
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformItemList = this.UIButton_ItemPrefab.transform.parent.gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
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
        /// Sự kiện khi Button vật phẩm được chọn
        /// </summary>
        /// <param name="skillData"></param>
        private void ButtonItem_Clicked(ItemData itemData)
        {
            this.ItemSelected?.Invoke(itemData);
            this.ButtonClose_Clicked();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi bỏ qua một số Frame nhất định
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrame(int skip, Action callback)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            callback?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            this.StartCoroutine(this.ExecuteSkipFrame(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformItemList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách kỹ năng
        /// </summary>
        private void ClearItemList()
        {
            foreach (Transform child in this.transformItemList.transform)
            {
                if (child.gameObject != this.UIButton_ItemPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Thêm vật phẩm
        /// </summary>
        /// <param name="itemData"></param>
        private void AddItem(ItemData itemData)
        {
            UIButtonSprite uiButtonItem = GameObject.Instantiate<UIButtonSprite>(this.UIButton_ItemPrefab);
            uiButtonItem.gameObject.SetActive(true);
            uiButtonItem.transform.SetParent(this.transformItemList, false);

            uiButtonItem.BundleDir = itemData.IconBundleDir;
            uiButtonItem.AtlasName = itemData.IconAtlasName;
            uiButtonItem.NormalSpriteName = itemData.Icon;
            uiButtonItem.DisabledSpriteName = itemData.Icon;

            uiButtonItem.Click = () => {
                this.ButtonItem_Clicked(itemData);
            };
        }

        /// <summary>
        /// Làm mới khung
        /// </summary>
        private void Refresh()
        {
            this.ClearItemList();

            /// Duyệt danh sách và thêm vật phẩm tương ứng vào khung
            foreach (ItemData itemData in this.Items)
            {
                this.AddItem(itemData);
            }
            this.RebuildLayout();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiện đối tượng
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
            this.Refresh();
        }

        /// <summary>
        /// Ẩn đối tượng
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
