using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace FSPlay.KiemVu.UI.MessageBox
{
    /// <summary>
    /// Khung Message Box
    /// </summary>
    public class UIMessageBox : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Tiêu đề khung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_TitleText;

        /// <summary>
        /// Nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ContentText;

        /// <summary>
        /// Button OK
        /// </summary>
        [SerializeField]
        private Button UIButton_OK;

        /// <summary>
        /// Button Cancel
        /// </summary>
        [SerializeField]
        private Button UIButton_Cancel;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi nút OK được ấn
        /// </summary>
        public Action OnOK { get; set; }

        /// <summary>
        /// Sự kiện khi nút Hủy bỏ được ấn
        /// </summary>
        public Action OnCancel { get; set; }

        /// <summary>
        /// Hiện Button OK
        /// </summary>
        public bool ShowButtonOK
        {
            get
            {
                return this.UIButton_OK.gameObject.activeSelf;
            }
            set
            {
                this.UIButton_OK.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Hiện Button hủy bỏ
        /// </summary>
        public bool ShowButtonCancel
        {
            get
            {
                return this.UIButton_Cancel.gameObject.activeSelf;
            }
            set
            {
                this.UIButton_Cancel.gameObject.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content
        {
            get
            {
                return this.UIText_ContentText.text;
            }
            set
            {
                this.UIText_ContentText.text = value;
            }
        }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title
        {
            get
            {
                return this.UIText_TitleText.text;
            }
            set
            {
                this.UIText_TitleText.text = value;
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến tại frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Thiết lập ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_OK.onClick.AddListener(this.ButtonOK_Clicked);
            this.UIButton_Cancel.onClick.AddListener(this.ButtonCancel_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button OK được ấn
        /// </summary>
        private void ButtonOK_Clicked()
        {
            this.OnOK?.Invoke();
            this.Hide();
        }

        /// <summary>
        /// Sự kiện khi Button hủy bỏ được ấn
        /// </summary>
        private void ButtonCancel_Clicked()
        {
            this.OnCancel?.Invoke();
            this.Hide();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiện bảng thông báo
        /// </summary>
        public void Show()
        {
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Ẩn bảng thông báo
        /// </summary>
        public void Hide()
        {
            if (this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}

