using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using FSPlay.KiemVu.Factory;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Control.Component;

namespace FSPlay.KiemVu.UI.CoreUI
{
    /// <summary>
    /// Khung chat trên đầu đối tượng
    /// </summary>
    public class UIRoleHeaderChat : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText;

        /// <summary>
        /// Thời gian giữ
        /// </summary>
        [SerializeField]
        private float _KeepTime = 5f;

        /// <summary>
        /// Thời gian ẩn
        /// </summary>
        [SerializeField]
        private float _FadeTime = 1f;

        /// <summary>
        /// Vị trí điểm đặt so với màn hình
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;

        /// <summary>
        /// Tọa độ đặt khi cưỡi ngựa
        /// </summary>
        [SerializeField]
        private Vector2 _RiderOffset;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Background đối tượng
        /// </summary>
        private UnityEngine.UI.Image uiImage;

        /// <summary>
        /// Luồng thực thi hiệu ứng ẩn
        /// </summary>
        private Coroutine fadeOutCoroutine = null;

        /// <summary>
        /// Component Character
        /// </summary>
        private Character componentCharacter = null;

        /// <summary>
        /// Luồng cập nhật vị trí
        /// </summary>
        private Coroutine updatePosCoroutine = null;
        #endregion

        #region Properties
        /// <summary>
        /// Thời gian giữ
        /// </summary>
        public float KeepTime
        {
            get
            {
                return this._KeepTime;
            }
            set
            {
                this._KeepTime = value;
            }
        }

        private GameObject _ReferenceObject;
        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject
        {
            get
            {
                return this._ReferenceObject;
            }
            set
            {
                this._ReferenceObject = value;
                if (value != null)
                {
                    /// Gắn lại thành phần tương ứng
                    this.componentCharacter = value.GetComponent<Character>();
                }
                else
                {
                    this.componentCharacter = null;
                }
            }
        }

        /// <summary>
        /// Tọa độ điểm đặt
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
        /// Đánh dấu có phải Leader không
        /// </summary>
        public bool IsLeader { get; set; } = false;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.uiImage = this.GetComponent<UnityEngine.UI.Image>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.StopAllCoroutines();
            this.StartCoroutine(this.ExecuteAfterSec(1f, () => {
                /// Nếu không phải Leader
                if (!this.IsLeader)
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    /// Cập nhật tọa độ
                    this.updatePosCoroutine = this.StartCoroutine(this.UpdatePosContinuously());
                }
                else
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    this.updatePosCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(5, () => {
                        this.UpdatePos_Leader();
                    }));
                }
            }));
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.StopAllCoroutines();
            this.StartCoroutine(this.ExecuteAfterSec(1f, () => {
                /// Nếu không phải Leader
                if (!this.IsLeader)
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    /// Cập nhật tọa độ
                    this.updatePosCoroutine = this.StartCoroutine(this.UpdatePosContinuously());
                }
                else
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    this.updatePosCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(5, () => {
                        this.UpdatePos_Leader();
                    }));
                }
            }));
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng bị hủy kích hoạt
        /// </summary>
        private void OnDisable()
        {

        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi bỏ qua một số Frame
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrames(int skip, Action callback)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            callback?.Invoke();
        }

        /// <summary>
        /// Thực thi sự kiện sau khoảng thời gian
        /// </summary>
        /// <param name="delaySec"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteAfterSec(float delaySec, Action work)
        {
            yield return new WaitForSeconds(delaySec);
            work?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
            }));
        }

        /// <summary>
        /// Thực thi luồng đợi một khoảng sau đó tự hủy
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoKeepAndFadeOut()
        {
            /// Đổi Alpha màu khung
            Color color = this.uiImage.color;
            color.a = 1f;
            this.uiImage.color = color;
            /// Đổi Alpha màu chữ
            color = this.UIText.color;
            color.a = 1f;
            this.UIText.color = color;

            /// Nếu có thời gian đợi
            if (this._KeepTime > 0)
            {
                yield return new WaitForSeconds(this._KeepTime);
            }

            /// Thực hiện ẩn
            float lifeTime = 0f;
            while (lifeTime < this._FadeTime)
            {
                float percent = lifeTime / this._FadeTime;
                /// Đổi Alpha màu khung
                color = this.uiImage.color;
                color.a = 1f - percent;
                this.uiImage.color = color;
                /// Đổi Alpha màu chữ
                color = this.UIText.color;
                color.a = 1f - percent;
                this.UIText.color = color;
                /// Bỏ qua frame
                yield return null;
                /// Tăng thời gian
                lifeTime += Time.deltaTime;
            }
            this.Hide();

            this.fadeOutCoroutine = null;
        }

        /// <summary>
        /// Cập nhật vị trí của Leader
        /// </summary>
        private void UpdatePos_Leader()
        {
            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + (this.componentCharacter != null && this.componentCharacter.RefObject.RoleData.IsRiding ? this._RiderOffset : this._Offset);
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(0, screenPos.y);

            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, this.GetComponent<RectTransform>().anchoredPosition.y);
        }

        /// <summary>
        /// Cập nhật vị trí tương ứng vị trí của đối tượng
        /// </summary>
        private void UpdatePos()
        {
            if (this.ReferenceObject == null)
            {
                return;
            }

            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + (this.componentCharacter != null && this.componentCharacter.RefObject.RoleData.IsRiding ? this._RiderOffset : this._Offset);
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y);
        }

        /// <summary>
        /// Thực thi cập nhật vị trí liên tục
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePosContinuously()
        {
            /// Lặp liên tục
            while (true)
            {
                /// Bỏ qua Frame
                yield return new WaitForSeconds(0.1f);
                /// Cập nhật tọa độ
                this.UpdatePos();
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Ẩn đối tượng
        /// </summary>
        public void Hide()
        {
            /// Ngắt tất cả luồng đang thực thi
            this.StopAllCoroutines();

            this.UIText.text = "";
            /// Đổi Alpha màu khung
            Color color = this.uiImage.color;
            color.a = 0f;
            this.uiImage.color = color;
            /// Đổi Alpha màu chữ
            color = this.UIText.color;
            color.a = 0f;
            this.UIText.color = color;
        }

        /// <summary>
        /// Hiện đối tượng
        /// </summary>
        public void Show(string chatContent)
        {
            /// Ngắt tất cả luồng đang thực thi
            if (this.fadeOutCoroutine != null)
            {
                this.StopCoroutine(this.fadeOutCoroutine);
            }

            this.RebuildLayout();
            this.UIText.text = chatContent;
            this.fadeOutCoroutine = this.StartCoroutine(this.DoKeepAndFadeOut());
        }

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            this.Hide();
            this.ReferenceObject = null;
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }

        /// <summary>
        /// Buộc đối tượng cập nhật lại vị trí của mình
        /// </summary>
        public void ForceSynsPositionImmediately()
        {
            /// Nếu là Leader
            if (this.IsLeader)
            {
                this.UpdatePos_Leader();
            }
            else
            {
                this.UpdatePos();
            }
        }
        #endregion
    }
}
