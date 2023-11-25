using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Control.Component;
using FSPlay.GameEngine.Logic;
using System.Collections;

namespace FSPlay.KiemVu.UI.UICore
{
    /// <summary>
    /// Thông tin sạp hàng trên đầu người chơi
    /// </summary>
    public class UIRoleShopName : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button mở sạp hàng
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_OpenShop;

        /// <summary>
        /// Text tên sạp hàng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ShopName;

        /// <summary>
        /// Vị trí điểm đặt so với màn hình
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Đợi chút (tránh bug chữ trắng)
        /// </summary>
        private bool skipFirst;
        #endregion

        #region Properties
        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject { get; set; }

        /// <summary>
        /// Tên sạp hàng
        /// </summary>
        public string ShopName
        {
            get
            {
                return this.UIText_ShopName.text;
            }
            set
            {
                this.UIText_ShopName.text = value;
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
        /// Có phải Leader không
        /// </summary>
        public bool IsLeader { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
                this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(this.IsLeader ? 1f : 0.1f));
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

            /// Nếu là Leader thì không cần update liên tục
            if (!this.IsLeader)
            {
                this.UpdatePos();
            }
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
                this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(this.IsLeader ? 1f : 0.1f));

            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_OpenShop.onClick.RemoveAllListeners();
            this.UIButton_OpenShop.onClick.AddListener(this.ButtonOpenShop_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button mở cửa hàng được ấn
        /// </summary>
        private void ButtonOpenShop_Clicked()
        {
            /// Nếu không có đối tượng
            if (this.ReferenceObject == null)
            {
                return;
            }
            /// Nếu là bản thân
            else if (this.IsLeader)
            {
                return;
            }

            Character character = this.ReferenceObject.GetComponent<Character>();
            /// Nếu đối tượng không phải người chơi
            if (character == null)
            {
                return;
            }

            /// Thực hiện Click vào Shop của người chơi
            Global.Data.GameScene.PlayerShopClick(character.RefObject);
        }
        #endregion

        #region Private methods
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
        /// Ẩn đối tượng
        /// </summary>
        public void Hide()
        {
            /// Ngắt tất cả luồng đang thực thi
            this.StopAllCoroutines();
            /// Ẩn đối tượng
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Hiện đối tượng
        /// </summary>
        public void Show()
        {
            /// Ngắt tất cả luồng đang thực thi
            this.StopAllCoroutines();
            /// Ẩn đối tượng
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            this.Hide();
            this.ReferenceObject = null;
            this.UIText_ShopName.text = "";
            this.UIButton_OpenShop.onClick.RemoveAllListeners();
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
