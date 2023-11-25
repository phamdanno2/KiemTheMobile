using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung thiết lập tên cửa hàng
    /// </summary>
    public class UIPlayerShop_SetShopName : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Input tên cửa hàng
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_ShopName;

        /// <summary>
        /// Button xác nhận
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Accept;

        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện xác nhận
        /// </summary>
        public Action<string> Accept { get; set; }
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
            this.UIButton_Accept.onClick.AddListener(this.ButtonAccept_Clicked);
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
        /// Sự kiện khi Button xác nhận được ấn
        /// </summary>
        private void ButtonAccept_Clicked()
        {
            string shopName = this.UIInput_ShopName.text;
            /// Nếu chưa nhập tên cửa hàng
            if (string.IsNullOrEmpty(shopName))
            {
                KTGlobal.AddNotification("Hãy nhập tên cửa hàng!");
            }

            this.Accept?.Invoke(Utils.BasicNormalizeString(shopName));
        }
        #endregion
    }
}
