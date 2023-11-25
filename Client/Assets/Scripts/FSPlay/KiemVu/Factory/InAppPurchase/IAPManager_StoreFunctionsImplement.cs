#if !UNITY_IOS

using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameFramework.Logic;
using Server.Data;
using Server.Tools;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace FSPlay.KiemVu.Factory.InAppPurchase
{
    /// <summary>
    /// Hệ thống In-App-Purchase (IAP)
    /// </summary>
    public partial class IAPManager : IStoreListener
    {
        /// <summary>
        /// Hàm này gọi khi hệ thống khởi tạo thành công
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            KTDebug.Log("OnInitialized: PASS");

            this.m_StoreController = controller;
            this.m_StoreExtensionProvider = extensions;
        }

        /// <summary>
        /// Hàm này gọi khi hệ thống khởi tạo thất bại
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            KTGlobal.HideLoadingFrame();
        }

        public void RequestVerifyPayment(RecipeVerify PaymentRequest)
        {
            KTGlobal.ShowLoadingFrame("Xác thực hóa đơn");
            if (PaymentRequest != null)
            {
                SimpleHttpTask.HttpPost("https://sdk.kt2009.mobi/PaymentVerify.aspx", null, DataHelper.ObjectToBytes(PaymentRequest), (request) =>
                {
                    this.PaymentVerifyRep(request);
                }, 10f);
            }
        }

        /// <summary>
        /// Sau khi kiểm tra có kết quả, thực thi hàm này
        /// </summary>
        /// <param name="request"></param>
        private void PaymentVerifyRep(UnityWebRequest request)
        {
            KTGlobal.HideLoadingFrame();

            try
            {
                if (request != null && string.IsNullOrEmpty(request.error))
                {
                    byte[] returnBytes = request.downloadHandler.data;

                    if (returnBytes != null && returnBytes.Length > 0)
                    {
                        PaymentVerifyRep responseData = DataHelper.BytesToObject<PaymentVerifyRep>(returnBytes, 0, returnBytes.Length);
                        if (responseData != null)
                        {
                            int Status = responseData.Status;

                            if (Status == 0)
                            {
                                Super.ShowMessageBox("Mua gói thành công", responseData.Msg, true);
                            }
                            else if (Status < 0)
                            {
                                Super.ShowMessageBox("Lỗi xác thực thanh toán", responseData.Msg, true);
                            }
                        }
                    }
                    else
                    {
                        Super.ShowMessageBox("Lỗi xác thực", "Hệ thống thanh toán đang bảo trì vui lòng quay lại sau", true);
                    }
                }
                else
                {
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
        /// Hàm này gọi khi có sự kiện thực hiện mua hàng thành công
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {

        
            /// ID gói hàng đã mua
            string productID = args.purchasedProduct.definition.id;
            /// Thông tin hóa đơn mua hàng
            string recipeID = args.purchasedProduct.receipt;
            /// ID Transaction
            string transactionID = args.purchasedProduct.transactionID;

            /// Gửi thông tin hóa đơn và ID gói hàng lên Server
            KTDebug.LogError("Buy product => ID = " + productID + " - RecipeID = " + recipeID + " - TransactionID = " + transactionID + "|"  + Global.Data.RoleData.ZoneID);

            RecipeVerify _Verify = new RecipeVerify();
            _Verify.RoleID = Global.Data.RoleData.RoleID;
            _Verify.TransID = SDKSession.TrasnID;
            _Verify.UserToken = SDKSession.AccessToken;
            _Verify.PurchaseData = recipeID;
            _Verify.ServerID = Global.Data.RoleData.ZoneID;

            KTDebug.LogError("START SEND PAYMENT VERRIFY ");

            this.RequestVerifyPayment(_Verify);

            KTDebug.LogError("END SEND PAYMENT VERIFY ");

          



            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Hàm này gọi khi có sự kiện thực hiện mua hàng thất bại
        /// </summary>
        /// <param name="product"></param>
        /// <param name="failureReason"></param>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            KTGlobal.HideLoadingFrame();
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing
            // this reason with the user to guide their troubleshooting actions.
            KTDebug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
}
#endif