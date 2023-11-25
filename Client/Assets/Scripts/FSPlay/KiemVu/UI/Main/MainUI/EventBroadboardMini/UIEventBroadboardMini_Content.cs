using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main.MainUI.EventBroadboardMini
{
    /// <summary>
    /// Nội dung trong khung hoạt động sự kiện phụ bản Mini
    /// </summary>
    public class UIEventBroadboardMini_Content : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Text;
        #endregion

        #region Properties
        /// <summary>
        /// Text nội dung
        /// </summary>
        public string Text
        {
            get
            {
                return this.UIText_Text.text;
            }
            set
            {
                this.UIText_Text.text = value;
            }
        }
        #endregion

        #region Core MonoBehaviour
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

        }
        #endregion
    }
}
