#if !UNITY_IOS

using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace FSPlay.KiemVu.Factory.InAppPurchase
{
    /// <summary>
    /// Hệ thống In-App-Purchase (IAP)
    /// </summary>
	public partial class IAPManager : MonoBehaviour
    {
        #region Singleton - Instance
        /// <summary>
        /// Hệ thống In-App-Purchase (IAP)
        /// </summary>
        public static IAPManager Instance { get; private set; }
		#endregion

		#region Define
		/// <summary>
		/// Controller của Unity
		/// </summary>
		private IStoreController m_StoreController;
        /// <summary>
        /// Hệ thống của Store
        /// </summary>
        private IExtensionProvider m_StoreExtensionProvider;

		/// <summary>
		/// Danh sách gói hàng được bán
		/// </summary>
		private readonly List<string> Products = new List<string>();
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			IAPManager.Instance = this;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Khởi tạo đối tượng
		/// </summary>
		public void Init(List<string> products)
		{
			/// Nếu không có gói hàng nào thì thôi
			if (products == null || products.Count <= 0)
			{
				return;
			}

			/// Nếu chưa thiết lập Controller
			if (this.m_StoreController == null)
			{
				/// Thiết lập danh sách gói hàng
				this.Products.AddRange(products);
				/// Thiết lập
				this.InitializePurchasing();
			}
		}

		/// <summary>
		/// Đã khởi tạo chưa
		/// </summary>
		/// <returns></returns>
		public bool IsInitialized()
        {
            return this.m_StoreController != null && this.m_StoreExtensionProvider != null;
        }
		#endregion
	}
}
#endif
