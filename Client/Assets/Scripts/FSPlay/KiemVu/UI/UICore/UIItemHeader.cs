using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FSPlay.KiemVu.UI.UICore
{
    /// <summary>
    /// Thanh tên vật phẩm
    /// </summary>
    public class UIItemHeader : TTMonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text tên vật phẩm
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Name;

        /// <summary>
        /// Tọa độ đặt (theo màn hình)
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;
        #endregion

        #region Properties
        /// <summary>
        /// Tên vật phẩm
        /// </summary>
        public string Name
        {
            get
            {
                return this.UIText_Name.text;
            }
            set
            {
                this.UIText_Name.text = value;
            }
        }

        /// <summary>
        /// Màu tên vật phẩm
        /// </summary>
        public Color NameColor
        {
            get
            {
                return this.UIText_Name.color;
            }
            set
            {
                this.UIText_Name.color = value;
            }
        }

        /// <summary>
        /// Vị trí ban đầu
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
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Đợi chút (tránh bug chữ trắng)
        /// </summary>
        private bool skipFirst;

        /// <summary>
        /// Vị trí ban đầu thiết lập
        /// </summary>
        private Vector2 firstOffset;

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.InitPrefabs();
            this.firstOffset = this._Offset;
        }

        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
                //this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(0.1f));
        }

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame
        /// </summary>
        private void Update()
        {
            if (this.skipFirst)
            {
                return;
            }
            this.UpdatePos();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
                //this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(0.1f));
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
        /// Cập nhật vị trí tương ứng vị trí của đối tượng
        /// </summary>
        private void UpdatePos()
        {
            if (this.ReferenceObject == null)
            {
                return;
            }

            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + this._Offset;
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            this.StopAllCoroutines();
            this.Name = "";
            this.NameColor = default;
            this.ReferenceObject = null;
            this.Offset = this.firstOffset;
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }

        /// <summary>
        /// Buộc đối tượng cập nhật lại vị trí của mình
        /// </summary>
        public void ForceSynsPositionImmediately()
        {
            this.UpdatePos();
        }
        #endregion
    }
}
