#if !UNITY_IOS

using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameFramework.Logic;
using Server.Data;
using Server.Tools;
using UnityEngine.Networking;
using UnityEngine.Purchasing;

namespace FSPlay.KiemVu.Factory.InAppPurchase
{
    /// <summary>
    /// Hệ thống In-App-Purchase (IAP)
    /// </summary>
    public partial class IAPManager
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        private void InitializePurchasing()
        {
            /// Nếu đã kết nối rồi thì thôi
            if (this.IsInitialized())
            {
                return;
            }

            /// Tạo mới đối tượng kết nối đến Store
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            /// Duyệt danh sách gói hàng
            foreach (string productID in this.Products)
            {
                /// Thêm gói hàng vào
                builder.AddProduct(productID, ProductType.Consumable);
            }

            /// Khởi tạo đối tượng theo thiết lập đã cho
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Trả về giá bán của gói hàng trên Store
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public string GetProductPriceOnStore(string productID)
        {
            /// Nếu Store Controller đã được khởi tạo
            if (this.m_StoreController != null && this.m_StoreController.products != null)
            {
                /// Gói hàng tương ứng
                Product product = this.m_StoreController.products.WithID(productID);
                /// Nếu tồn tại
                if (product != null)
                {
                    /// Trả về kết quả
                    return product.metadata.localizedPriceString;
                }
            }
            /// Không tìm thấy
            return "";
        }

        /// <summary>
        /// Gửi yêu cầu mua tới gs
        /// </summary>
        /// <param name="PaymentRequest"></param>
        public void RequestPayment(PaymentRequest PaymentRequest)
        {
         

            if (PaymentRequest != null)
            {
                SimpleHttpTask.HttpPost("https://sdk.kt2009.mobi/PaymentCreate.aspx", null, DataHelper.ObjectToBytes(PaymentRequest), (request) =>
                {
                    this.RequestPaymentRep(request);
                }, 10f);
            }
        }

        /// <summary>
        /// Sau khi kiểm tra có kết quả, thực thi hàm này
        /// </summary>
        /// <param name="request"></param>
        private void RequestPaymentRep(UnityWebRequest request)
        {
            KTGlobal.HideLoadingFrame();

            try
            {
                if (request != null && string.IsNullOrEmpty(request.error))
                {
                    byte[] returnBytes = request.downloadHandler.data;

                    if (returnBytes != null && returnBytes.Length > 0)
                    {
                        PaymentRequestRep responseData = DataHelper.BytesToObject<PaymentRequestRep>(returnBytes, 0, returnBytes.Length);

                        if (responseData != null)
                        {
                            int Status = responseData.Status;

                            if (Status == 0)
                            {
                                // Nếu trả về 0 thì mua
                                m_StoreController.InitiatePurchase(responseData.ProductBuy, responseData.TransID);
                                SDKSession.TrasnID = responseData.TransID;
                            }
                            else if (Status < 0)
                            {
                                Super.ShowMessageBox("Lỗi thanh toán", responseData.Msg, true);
                            }
                        }
                    }
                    else
                    {
                        Super.ShowMessageBox("Lỗi đăng nhập", "Hệ thống thanh toán đang bảo trì vui lòng quay lại sau", true);
                    }
                }
                else
                {
                    Super.ShowMessageBox("Lỗi thanh toán", "Không thể kết nối với máy chủ", true);
                    /// Hủy đối tượng
                    request.downloadHandler.Dispose();
                    request.Dispose();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Thực hiện mua gói hàng
        /// </summary>
        /// <param name="productId">ID gói hàng</param>
        public void BuyProductID(string productId)
        {

            /// Nếu đối tượng đã được khởi tạo
            if (this.IsInitialized())
            {
                KTGlobal.ShowLoadingFrame("Vui lòng đợi khởi tạo gói mua");

                /// Tìm trên Store gói hàng có ID tương ứng
                Product product = this.m_StoreController.products.WithID(productId);

                /// Nếu tìm thấy thì có thể mua được
                if (product != null && product.availableToPurchase)
                {
                    KTDebug.LogError("END SEND PAYMENT CREATE ");

                    KTDebug.Log(string.Format("Purchasing product asychronously: ID = '{0}', TransactionID = {1}", product.definition.id, product.transactionID));

                    PaymentRequest _RequeustBuy = new PaymentRequest();
                    _RequeustBuy.DeviceID = KTGlobal.DeviceInfo;
                    _RequeustBuy.PackageName = productId;
#if UNITY_IOS
                       _RequeustBuy.PlatForm = "IOS";
#elif UNITY_ANDROID
                    _RequeustBuy.PlatForm = "ANDROID";
#elif UNITY_EDITOR
                       _RequeustBuy.PlatForm = "PC";
#endif

                    _RequeustBuy.RoleID = Global.Data.RoleData.RoleID;
                    _RequeustBuy.ServerID = Global.Data.ServerData.LastServer.nServerID;
                    _RequeustBuy.TransID = product.transactionID;
                    _RequeustBuy.UserToken = SDKSession.AccessToken;

                    KTDebug.LogError("START SEND PAYMENT CREATE ");
                    this.RequestPayment(_RequeustBuy);

                    /// Thực hiện mua hàng
                }
                /// Nếu không tìm thấy
                else
                {
                    KTGlobal.HideLoadingFrame();
                    KTDebug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            /// Nếu đối tượng chưa được khởi tạo
            else
            {
                KTGlobal.HideLoadingFrame();
                KTDebug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
    }
}
#endif