using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Factory.Cache;
using FSPlay.KiemVu.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSPlay.KiemVu.Utilities.UnityComponent
{
    /// <summary>
    /// Quản lý hiệu ứng rơi vật phẩm xuống đất
    /// </summary>
    public class DropItemAnimation2D : TTMonoBehaviour
    {
        #region Define
        #region Reference Object
        /// <summary>
        /// Thân vật phẩm
        /// </summary>
        public SpriteRenderer Body { get; set; }
        #endregion

        private ItemData _Data = null;
        /// <summary>
        /// ID vật phẩm
        /// </summary>
        public ItemData Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;

                this.OnDataChanged();
            }
        }

        /// <summary>
        /// Sự kiện khi bắt đầu thực hiện động tác
        /// </summary>
        public Action OnStart { get; set; }

        /// <summary>
        /// Độ thu phóng
        /// </summary>
        public float Scale { get; set; } = 1f;
        #endregion

        #region Private methods
        /// <summary>
        /// Sự kiện khi dữ liệu vật phẩm thay đổi
        /// </summary>
        private void OnDataChanged()
        {

        }

        /// <summary>
        /// Phương thức Async tải động tác hiện tại
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadBodyAsync()
        {
            yield return ItemCacheManager.Instance.LoadSprites(this._Data.ItemID);
        }

        /// <summary>
        /// Thực hiện động tác
        /// </summary>
        /// <returns></returns>
        private IEnumerator Play(Sprite sprite)
        {
            if (sprite == null)
            {
                yield break;
            }

            this.Body.sprite = sprite;
            this.Body.drawMode = SpriteDrawMode.Sliced;
            this.Body.size = sprite.rect.size * this.Scale;

            yield break;
        }

        /// <summary>
        /// Thực hiện quay đối tượng
        /// </summary>
        /// <param name="fromAngle"></param>
        /// <param name="toAngle"></param>
        /// <param name="duration"></param>
        /// <param name="frameCount"></param>
        /// <returns></returns>
        private IEnumerator Rotate(int fromAngle, int toAngle, float duration, int frameCount)
        {
            this.Body.transform.localRotation = Quaternion.Euler(0, 0, fromAngle);
            float lifeTime = 0f;
            float tick = frameCount <= 0 ? -1 : duration / frameCount;

            if (tick == -1)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(tick);
            }

            while (true)
            {
                lifeTime += tick == -1 ? Time.deltaTime : tick;
                if (lifeTime >= duration)
                {
                    break;
                }

                float percent = lifeTime / duration;

                float nextAngle = fromAngle + (toAngle - fromAngle) * percent;
                this.Body.transform.localRotation = Quaternion.Euler(0, 0, nextAngle);

                if (tick == -1)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(tick);
                }
            }
            this.Body.transform.localRotation = Quaternion.Euler(0, 0, toAngle);
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Đánh dấu vừa thực hiện hàm Awake
        /// </summary>
        private bool justAwaken = false;

        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.justAwaken = true;
        }

        /// <summary>
        /// Hàm này gọi đến khi bắt đầu frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.justAwaken = false;
        }

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng bị ẩn
        /// </summary>
        private void OnDisable()
        {
            if (this.justAwaken)
            {
                this.justAwaken = false;
                return;
            }

            this.Body.sprite = null;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Thực hiện động tác
        /// </summary>
        /// <returns></returns>
        public IEnumerator DoActionAsync()
        {
            if (!this.gameObject.activeSelf)
            {
                yield break;
            }

            #region Load sprites
            int totalLoaded = 0;
            IEnumerator DoLoadBody()
            {
                yield return this.LoadBodyAsync();
                totalLoaded++;
            }
            KTResourceManager.Instance.StartCoroutine(DoLoadBody());

            while (totalLoaded < 1)
            {
                yield return null;
            }
            #endregion

            if (!this.gameObject || !this.gameObject.activeSelf)
            {
                yield break;
            }

            Sprite sprite = KTResourceManager.Instance.GetSubAsset<Sprite>(this._Data.MapSpriteBundleDir, this._Data.MapSpriteAtlasName, this._Data.View);
            /// Nếu không tìm thấy
            if (sprite == null)
            {
                sprite = KTResourceManager.Instance.GetSubAsset<Sprite>("Resources/Item/MapSprite.unity3d", "MapSprite", "hoicham_nho");
            }
            yield return this.Play(sprite);
        }

        /// <summary>
        /// Thực hiện quay đối tượng
        /// </summary>
        /// <param name="fromAngle"></param>
        /// <param name="toAngle"></param>
        /// <param name="duration"></param>
        /// <param name="frameCount"></param>
        /// <returns></returns>
        public IEnumerator DoRotateAsync(int fromAngle, int toAngle, float duration, int frameCount = -1)
        {
            if (!this.gameObject || !this.gameObject.activeSelf)
            {
                yield break;
            }

            yield return this.Rotate(fromAngle, toAngle, duration, frameCount);
        }
        #endregion
    }
}
