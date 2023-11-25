using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// Khung thay đổi tôn chỉ gia tộc
    /// </summary>
    public class UIFamily_ChangeSlogan : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button Xác nhận
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Accept;

        /// <summary>
        /// Button hủy bỏ thay đổi tôn chỉ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Refuse;

        /// <summary>
        /// Button trở lại
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Back;

        /// <summary>
        /// Input tôn chỉ gia tộc
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_Slogan;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện thay đổi tôn chỉ gia tộc
        /// </summary>
        public Action<string> ChangeSlogan { get; set; }

        /// <summary>
        /// Sự kiện trở lại
        /// </summary>
        public Action Back { get; set; }

        /// <summary>
        /// Tôn chỉ hiện tại
        /// </summary>
        public string CurrentSlogan
		{
			get
			{
                return this.UIInput_Slogan.text;
			}
			set
			{
                this.UIInput_Slogan.text = value;
			}
		}
        #endregion

        #region core MonoBehaviour
        /// <summary>
        /// Hàm này được ở frame đầu tiền
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Back.onClick.AddListener(this.ButtonBack_Clicked);
            this.UIButton_Accept.onClick.AddListener(this.ButtonAccept_Clicked);
            this.UIButton_Refuse.onClick.AddListener(this.ButtonRefuse_Cliced);
        }

        /// <summary>
        /// Sự kiện khi Button quay trở lại được ấn
        /// </summary>
        private void ButtonBack_Clicked()
        {
            this.Hide();
            this.Back?.Invoke();
        }

        /// <summary>
        /// Sự kiện xác nhận
        /// </summary>
        private void ButtonAccept_Clicked()
        {
            /// Tôn chỉ
            string slogan = Utils.BasicNormalizeString(this.UIInput_Slogan.text);
            /// Nếu chưa nhập
            if (string.IsNullOrEmpty(slogan))
            {
                KTGlobal.AddNotification("Chưa nhập tôn chỉ!");
                return;
            }

            /// Thực thi sự kiện thay đổi tôn chỉ
            this.ChangeSlogan?.Invoke(slogan);
        }

        /// <summary>
        /// Sự kiện hủy bỏ thay đổi tôn chỉ gia tộc
        /// </summary>
        private void ButtonRefuse_Cliced()
        {
            this.Hide();
            this.Back?.Invoke();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiện khung
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        /// <summary>
        /// Ẩn khung
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
