using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// Perfab chức vụ
    /// </summary>
    public class UIFamily_MemberList_ApproveFrame_RankInfo : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton;

        /// <summary>
        /// Text chức vụ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RankName;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện chọn chức vụ
        /// </summary>
        public Action Click { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string RankName
        {
            get
            {
                return this.UIText_RankName.text;
            }
            set
            {
                this.UIText_RankName.text = value;
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
            this.UIButton.onClick.AddListener(this.Button_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button được ấn
        /// </summary>
        private void Button_Clicked()
        {
            this.Click?.Invoke();
        }
        #endregion
    }
}

