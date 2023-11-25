using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Utilities.UnityComponent
{
    /// <summary>
    /// Dịch chuyển đối tượng
    /// </summary>
    public class SpriteTransition : TTMonoBehaviour
    {
        #region Define
        /// <summary>
        /// Bắt đầu
        /// </summary>
        [SerializeField]
        private Vector2 _From;

        /// <summary>
        /// Kết thúc
        /// </summary>
        [SerializeField]
        private Vector2 _To;

        /// <summary>
        /// Gia tốc
        /// </summary>
        [SerializeField]
        private float _Acceleration;

        /// <summary>
        /// Vận tốc ban đầu
        /// </summary>
        [SerializeField]
        private float _Velocity;

        /// <summary>
        /// Tự thực thi
        /// </summary>
        [SerializeField]
        private bool _AutoPlay;
        #endregion

        #region Private fields
        /// <summary>
        /// Luồng thực thi hiệu ứng
        /// </summary>
        private Coroutine animationCoroutine;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi đối tượng hoàn tất di chuyển
        /// </summary>
        public Action Done { get; set; }

        /// <summary>
        /// Gia tốc
        /// </summary>
        public float Acceleration
        {
            get
            {
                return this._Acceleration;
            }
            set
            {
                this._Acceleration = value;
            }
        }

        /// <summary>
        /// Vận tốc ban đầu
        /// </summary>
        public float Velocity
        {
            get
            {
                return this._Velocity;
            }
            set
            {
                this._Velocity = value;
            }
        }

        /// <summary>
        /// Vị trí ban đầu
        /// </summary>
        public Vector2 FromPos
        {
            get
            {
                return this._From;
            }
            set
            {
                this._From = value;
            }
        }

        /// <summary>
        /// Vị trí đích
        /// </summary>
        public Vector2 ToPos
        {
            get
            {
                return this._To;
            }
            set
            {
                this._To = value;
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            if (this._AutoPlay)
            {
                this.Play();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi hiệu ứng di chuyển
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private IEnumerator DoTransition()
        {
            /// Vector hướng
            Vector2 dirVector = this._To - this._From;
            /// Quãng đường cần di chuyển
            float distance = Vector2.Distance(this._From, this._To);
            /// Thời gian di chuyển
            float duration;
            /// Nếu có gia tốc
            if (this._Acceleration != 0)
            {
                /// t = (-v0 + Sqrt(v0 ^ 2 + as)) / a
                duration = (-this._Velocity + Mathf.Sqrt(this._Velocity * this._Velocity + 2 * this._Acceleration * distance)) / this._Acceleration;
            }
            else
            {
                /// t = s / v
                duration = distance / this._Velocity;
            }

            /// Thiết lập vị trí ban đầu
            this.transform.localPosition = this._From;
            /// Bỏ qua Frame
            yield return null;

            /// Thời gian tồn tại
            float lifeTime = 0f;
            /// Lặp liên tục chừng nào còn tồn tại
            while (lifeTime < duration)
            {
                /// Tăng thời gian
                lifeTime += Time.deltaTime;
                /// Quãng đường mới (s = v0 * t + 1/2 * a * t^2)
                float newDistance = this._Velocity * lifeTime + 0.5f * this._Acceleration * lifeTime * lifeTime;
                /// Tọa độ mới
                Vector2 newPos = KTMath.FindPointInVectorWithDistance(this._From, dirVector, newDistance);
                /// Cập nhật vị trí mới
                this.transform.localPosition = newPos;
                /// Bỏ qua Frame
                yield return null;
            }

            /// Thiết lập vị trí đích
            this.transform.localPosition = this._To;
            /// Bỏ qua Frame
            yield return null;
            /// Thực thi sự kiện di chuyển hoàn tất
            this.Done?.Invoke();
        }

        /// <summary>
        /// Thực thi hiệu ứng
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoAnimation()
        {
            yield return this.DoTransition();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Ngừng thực thi hiệu ứng
        /// </summary>
        public void Stop()
        {
            if (this.animationCoroutine != null)
            {
                this.StopCoroutine(this.animationCoroutine);
                this.animationCoroutine = null;
            }
        }

        /// <summary>
        /// Thực thi hiệu ứng
        /// </summary>
        public void Play()
        {
            /// Nếu đối tượng không được kích hoạt
            if (!this.gameObject)
            {
                return;
            }
            /// Ngừng thực thi hiệu ứng cũ
            this.Stop();
            /// Thực thi hiệu ứng
            this.animationCoroutine = this.StartCoroutine(this.DoAnimation());
        }
        #endregion
    }
}
