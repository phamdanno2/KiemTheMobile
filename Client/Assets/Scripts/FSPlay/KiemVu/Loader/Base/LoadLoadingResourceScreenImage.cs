using FSPlay.GameEngine.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Tải xuống ảnh của màn hình tải dữ liệu
    /// </summary>
    public class LoadLoadingResourceScreenImage : TTMonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Sự kiện tải xuống hoàn tất
        /// </summary>
        public Action Done { get; set; }

        /// <summary>
        /// Sự kiện tải xuống thất bại
        /// </summary>
        public Action<string> Faild { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.StartCoroutine(this.StartDownloadImage());
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Bắt đầu tải tài nguyên
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartDownloadImage()
        {
            /// Tải màn hình Loading Screen
            yield return this.DownloadLoadingResourcesScreen();

            /// Thực thi sự kiện tải hoàn tất
            this.Done?.Invoke();
            /// Xóa đối tượng
            this.Destroy();
        }

        /// <summary>
        /// Tải màn hình LoadingResources
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownloadLoadingResourcesScreen()
        {
            /// Đường dẫn tải
            string fullPath = string.Format("{0}{1}/Zip/{2}", MainGame.GameInfo.CdnUrl, Global.GetDeviceForWebURL(), MainGame.GameInfo.LoadingScreenFileName);
            /// Tạo yêu cầu
            UnityWebRequest request = new UnityWebRequest(fullPath);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            /// Nếu có lỗi gì đó
            if (!string.IsNullOrEmpty(request.error))
            {
                /// Thực thi sự kiện tải bị lõi
                this.Faild?.Invoke(request.error);
                yield break;
            }

            /// Byte dữ liệu
            byte[] byteData = request.downloadHandler.data;
            /// Ghi ra File
            Utils.SaveBytesToFile(MainGame.GameInfo.LoadingScreenFileName, byteData);
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        private void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }
        #endregion
    }
}
