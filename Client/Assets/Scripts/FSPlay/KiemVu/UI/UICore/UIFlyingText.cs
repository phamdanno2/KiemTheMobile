using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FSPlay.KiemVu.UI.CoreUI
{
    /// <summary>
    /// Đối tượng chữ bay hiển thị sát thương
    /// </summary>
    public class UIFlyingText : TTMonoBehaviour
    {
        #region Defines
        /// <summary>
        /// Tọa độ điểm đặt ban đầu (theo màn hình)
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;

        /// <summary>
        /// Vector biểu diễn vị trí bay (theo màn hình)
        /// </summary>
        [SerializeField]
        private Vector2 _FlyVector;

        /// <summary>
        /// Thời gian bay
        /// </summary>
        [SerializeField]
        private float _Duration;
        #endregion

        #region Properties
        /// <summary>
        /// Tọa độ điểm đặt ban đầu (theo màn hình)
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                return this._Offset;
            }
            set
            {
                this._Offset = value;
            }
        }

        /// <summary>
        /// Vector biểu diễn vị trí bay (theo màn hình)
        /// </summary>
        public Vector2 FlyVector
        {
            get
            {
                return this._FlyVector;
            }
            set
            {
                this._FlyVector = value;
            }
        }

        /// <summary>
        /// Thời gian bay
        /// </summary>
        public float Duration
        {
            get
            {
                return this._Duration;
            }
            set
            {
                this._Duration = value;
            }
        }

        /// <summary>
        /// Giá trị
        /// </summary>
        public string Text
        {
            get
            {
                return this.UIText.text;
            }
            set
            {
                this.UIText.text = value;
            }
        }

        /// <summary>
        /// Màu chữ
        /// </summary>
        public Color Color
        {
            get
            {
                return this.UIText.color;
            }
            set
            {
                this.UIText.color = value;
            }
        }

        /// <summary>
        /// Thời gian Delay trước khi thực hiện hiệu ứng
        /// </summary>
        public float Delay { get; set; } = 0f;

        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject { get; set; }

        /// <summary>
        /// Sự kiện khi đối tượng bị hủy
        /// </summary>
        public Action Destroyed { get; set; }

        /// <summary>
        /// Sự kiện khi đối tượng bị hủy
        /// </summary>
        public Action Playing { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Text
        /// </summary>
        private TextMeshProUGUI UIText;

        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Màu sắc ban đầu
        /// </summary>
        private Color firstColor;

        /// <summary>
        /// Vector chỉ hướng bay ban đầu
        /// </summary>
        private Vector2 firstFlyVector;

        /// <summary>
        /// Tọa độ ban đầu
        /// </summary>
        private Vector2 originAnchoredPos;

        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.originAnchoredPos = this.rectTransform.anchoredPosition;
            this.UIText = this.gameObject.GetComponent<TextMeshProUGUI>();
            this.firstColor = this.UIText.color;
            this.firstFlyVector = this._FlyVector;
        }

        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {

        }

        /// <summary>
        /// Luồng thực hiện hiệu ứng
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator DoPlay(Vector2 fromPos, Vector2 toPos, float duration)
        {
            /// Thực hiện sự kiện Play
            this.Playing?.Invoke();

            Vector2 fromScreenPos = Global.MainCamera.WorldToScreenPoint(fromPos);
            this.gameObject.transform.position = fromScreenPos;
            yield return null;

            Vector2 dirVector = toPos - fromPos;
            float tickTime = 0;

            while (tickTime < duration)
            {
                float percent = tickTime / duration;
                Vector2 newPos = fromPos + dirVector * percent;

                Vector2 newScreenPos = Global.MainCamera.WorldToScreenPoint(newPos);
                this.gameObject.transform.position = newScreenPos;

                yield return null;
                tickTime += Time.deltaTime;
            }

            Vector2 toScreenPos = Global.MainCamera.WorldToScreenPoint(toPos);
            this.gameObject.transform.position = toScreenPos;
            yield return null;

            this.Destroy();
        }

        /// <summary>
        /// Luồng thực hiện hiệu ứng theo vị trí Local
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator DoPlayLocal(Vector2 fromPos, Vector2 toPos, float duration)
        {
            /// Thực hiện sự kiện Play
            this.Playing?.Invoke();

            this.rectTransform.anchoredPosition = fromPos;
            yield return null;

            Vector2 dirVector = toPos - fromPos;
            float tickTime = 0;

            while (tickTime < duration)
            {
                float percent = tickTime / duration;
                Vector2 newPos = fromPos + dirVector * percent;

                this.rectTransform.anchoredPosition = newPos;

                yield return null;
                tickTime += Time.deltaTime;
            }

            this.rectTransform.anchoredPosition = toPos;
            yield return null;

            this.Destroy();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Thực hiện hiệu ứng
        /// </summary>
        public void Play()
        {
            this.StopAllCoroutines();

            if (this.UIText == null)
            {
                this.UIText = this.gameObject.GetComponent<TextMeshProUGUI>();
            }

            this.gameObject.transform.position = Vector3.zero;
            IEnumerator DelayThenExecute()
            {
                if (this.Delay > 0)
                {
                    yield return new WaitForSeconds(this.Delay);
                }

                if (this.ReferenceObject != null)
                {
                    Vector2 fromPos = (Vector2) this.ReferenceObject.transform.position + this._Offset;
                    Vector2 toPos = (Vector2) this.ReferenceObject.transform.position + this._Offset + this._FlyVector;
                    yield return this.DoPlay(fromPos, toPos, this._Duration);
                }
                else
                {
                    Vector2 fromPos = this.originAnchoredPos + this._Offset;
                    Vector2 toPos = this.originAnchoredPos + this._Offset + this._FlyVector;
                    yield return this.DoPlayLocal(fromPos, toPos, this._Duration);
                }
            }
            this.StartCoroutine(DelayThenExecute());
        }

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            this.StopAllCoroutines();
            this.Offset = default;
            this.FlyVector = this.firstFlyVector;
            this.Duration = 0f;
            this.Text = "";
            this.Color = this.firstColor;
            this.Delay = 0f;
            this.ReferenceObject = null;
            this.Playing = null;
            this.Destroyed?.Invoke();
            this.Destroyed = null;
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }
        #endregion
    }
}