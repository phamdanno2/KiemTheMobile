using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung tạo tộc
    /// </summary>
    public class UICreateFamily : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Button tạo gia tộc
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_CreateFamily;

        /// Button hủy bỏ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Cancel;

        /// <summary>
        /// Input nhập tên tộc
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInputFied_NameFamily;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }
        /// <summary>
        /// Sự kiện tạo khung
        /// </summary>
        public Action <string> CreateFamily { get; set; }
        /// <summary>
        /// Sự kiện đong khung
        /// </summary>
        public Action Cancel { get; set; }
        #endregion

        #region core Monobehaviour
        /// <summary>
        /// Hàm này được gọi ở frame đầu tiền
        /// </summary>
        private void Start()
        {
            this.InitPerfabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        private void InitPerfabs()
        {
            this.UIButton_Cancel.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_CreateFamily.onClick.AddListener(this.ButtonCreateFamily_Clicked);
            this.UIButton_Close.onClick.AddListener(this.ButtonCancel_Clicked);
        }
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }
        /// <summary>
        /// Sự kiện tạo bang
        /// </summary>
        private void ButtonCreateFamily_Clicked()
        {
            string familyName = this.UIInputFied_NameFamily.text;
            /// Nếu chưa nhập tên
            if (string.IsNullOrEmpty(familyName))
            {
                KTGlobal.AddNotification("Hãy nhập tên gia tộc!");
                return;
            }
            /// Kiểm tra có ký tự đặc biệt không
            else if (!familyName.CheckValidString())
			{
                KTGlobal.AddNotification("Tên không thể chứa ký tự đặc biệt!");
                return;
			}

            this.CreateFamily?.Invoke(familyName);
        }
        /// <summary>
        /// Sự kiện đóng
        /// </summary>
        private void ButtonCancel_Clicked()
        {
            this.Cancel?.Invoke();
        }
        #endregion
    }
}
