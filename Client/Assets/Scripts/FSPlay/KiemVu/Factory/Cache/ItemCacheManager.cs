using FSPlay.KiemVu.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSPlay.KiemVu.Factory.Cache
{
    /// <summary>
    /// Lớp quản lý Cache vật phẩm
    /// </summary>
    public class ItemCacheManager : TTMonoBehaviour
    {
        #region Singleton - Instance
        /// <summary>
        /// Đối tượng quản lý Cache hiệu ứng đạn nổ
        /// </summary>
        public static ItemCacheManager Instance { get; private set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            ItemCacheManager.Instance = this;
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {

        }

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame
        /// </summary>
        private void Update()
        {

        }
        #endregion

        #region Public methods
        /// <summary>
        /// Tải xuống Sprite và thêm vào danh sách trong Cache (nếu chưa được tải xuống)
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public IEnumerator LoadSprites(int itemID)
        {
            /// Nếu Res không tồn tại
            if (!Loader.Loader.Items.TryGetValue(itemID, out FSPlay.KiemVu.Entities.Config.ItemData itemData))
            {
                yield break;
            }

            /// Nếu Bundle không tồn tại
            if (string.IsNullOrEmpty(itemData.MapSpriteBundleDir))
            {
                yield break;
            }

            yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<UnityEngine.Sprite>(itemData.MapSpriteBundleDir, itemData.MapSpriteAtlasName, true);
        }
        #endregion
    }
}
