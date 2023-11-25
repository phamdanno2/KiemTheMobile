using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Màn hình chào người chơi lúc đăng nhập vào Game
    /// </summary>
    public class UIWelcomeDialog : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text tiêu đề
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Title;

        /// <summary>
        /// Text nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Content;

        /// <summary>
        /// Button OK
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_OK;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi nút OK được ấn
        /// </summary>
        public Action OnOK { get; set; }

        /// <summary>
        /// Tiêu đề khung
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
        /// Nội dung
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
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_OK.onClick.AddListener(this.ButtonOK_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button OK được ấn
        /// </summary>
        private void ButtonOK_Clicked()
        {
            this.OnOK?.Invoke();
        }
        #endregion
    }
}

