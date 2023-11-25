using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.UI.Main.MainUI.RadarMap;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.MainUI
{
    /// <summary>
    /// Đối tượng bản đồ nhỏ ở góc
    /// </summary>
    public class UIRadarMap : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Texture bản đồ nhỏ chiếu bởi RadarMapCamera
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.RawImage UIImage_MapTexture;

        /// <summary>
        /// Mask ẩn bản đồ nhỏ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_MaskInvisibleMinimap;

        /// <summary>
        /// Nút hiện bản đồ nhỏ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Show;

        /// <summary>
        /// Nút ẩn bản đồ nhỏ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Hide;

        /// <summary>
        /// Text tên bản đồ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MapName;

        /// <summary>
        /// Text tọa độ hiện tại
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Position;

        /// <summary>
        /// Nút mở bản đồ khu vực
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ToLocalMap;

        /// <summary>
        /// Tọa độ khi hiện
        /// </summary>
        [SerializeField]
        private Vector2 VisiblePosition;

        /// <summary>
        /// Tọa độ khi ẩn
        /// </summary>
        [SerializeField]
        private Vector2 InvisiblePosition;

        /// <summary>
        /// Tốc độ thực hiện hiệu ứng
        /// </summary>
        [SerializeField]
        private float AnimationDuration = 2f;

        /// <summary>
        /// Text Debug tọa độ thực
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_DebugWorldPos;

        /// <summary>
        /// Khay thuốc
        /// </summary>
        [SerializeField]
        private UIRadarMap_QuickMedicineBox UIMedicineBox;
        #endregion

        #region Properties
        /// <summary>
        /// Tên bản đồ
        /// </summary>
        public string MapName
        {
            get
            {
                return this.UIText_MapName.text;
            }
            set
            {
                this.UIText_MapName.text = value;
            }
        }

        private Vector2 _LeaderPosition;
        /// <summary>
        /// Tọa độ của Leader
        /// </summary>
        public Vector2 LeaderPosition
        {
            get
            {
                return this._LeaderPosition;
            }
            set
            {
                this._LeaderPosition = value;
                this.UpdateLeaderPosition();
            }
        }

        /// <summary>
        /// Sự kiện chuyển tới khung bản đồ khu vực
        /// </summary>
        public Action GoToLocalMap { get; set; }

        /// <summary>
        /// Sự kiện ấn dùng thuốc ở khay
        /// </summary>
        public Action<GoodsData> UseMedicine { get; set; }

        /// <summary>
        /// Sự kiện thuốc được chọn
        /// </summary>
        public Action<GoodsData, GoodsData> MedicineSelected { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Transform
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Show.onClick.AddListener(this.ToggleVisible);
            this.UIButton_Hide.onClick.AddListener(this.ToggleVisible);
            this.UIButton_ToLocalMap.onClick.AddListener(this.ButtonGoToLocalMap_Clicked);

            this.UIButton_Show.gameObject.SetActive(false);
            this.UIButton_Hide.gameObject.SetActive(true);

            /// Nếu không phải GM
            if (Global.Data.RoleData.GMAuth != 1)
            {
                this.UIText_DebugWorldPos.gameObject.SetActive(false);
            }
            
            this.UIMedicineBox.UseMedicine = this.UseMedicine;
            this.UIMedicineBox.MedicineSelected = this.MedicineSelected;
        }

        /// <summary>
        /// Luồng thực thi hiệu ứng ẩn khung trong khoảng thời gian
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator DoHide(float duration)
        {
            this.UIButton_Hide.gameObject.SetActive(false);
            this.UIButton_Show.gameObject.SetActive(false);

            float dTime = 0;
            Vector2 diffVector = this.InvisiblePosition - this.VisiblePosition;
            while (dTime < duration)
            {
                float percent = dTime / duration;
                Vector2 newPos = this.VisiblePosition + diffVector * percent;
                this.rectTransform.anchoredPosition = newPos;
                yield return null;
                dTime += Time.deltaTime;
            }
            this.rectTransform.anchoredPosition = this.InvisiblePosition;

            this.UIButton_Show.gameObject.SetActive(true);
        }

        /// <summary>
        /// Luồng thực thi hiệu ứng hiện khung trong khoảng thời gian
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator DoShow(float duration)
        {
            this.UIButton_Hide.gameObject.SetActive(false);
            this.UIButton_Show.gameObject.SetActive(false);

            float dTime = 0;
            Vector2 diffVector = this.VisiblePosition - this.InvisiblePosition;
            while (dTime < duration)
            {
                float percent = dTime / duration;
                Vector2 newPos = this.InvisiblePosition + diffVector * percent;
                this.rectTransform.anchoredPosition = newPos;
                yield return null;
                dTime += Time.deltaTime;
            }
            this.rectTransform.anchoredPosition = this.VisiblePosition;

            this.UIButton_Hide.gameObject.SetActive(true);
        }

        /// <summary>
        /// Ẩn/Hiện khung bản đồ
        /// </summary>
        /// <param name="isShow"></param>
        private void ToggleVisible()
        {
            bool isVisible = this.UIButton_Hide.gameObject.activeSelf;
            if (isVisible)
            {
                this.StartCoroutine(this.DoHide(this.AnimationDuration));
            }
            else
            {
                this.StartCoroutine(this.DoShow(this.AnimationDuration));
            }
        }

        /// <summary>
        /// Chuyển qua khung bản đồ khu vực
        /// </summary>
        private void ButtonGoToLocalMap_Clicked()
        {
            /// Nếu bản đồ hiện tại không cho phép mở bản đồ nhỏ
            if (!Loader.Loader.Maps[Global.Data.RoleData.MapCode].ShowMiniMap)
			{
                KTGlobal.AddNotification("Ở đây không được sử dụng bản đồ khu vực!");
                return;
			}
            this.GoToLocalMap?.Invoke();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Cập nhật tọa độ của Leader
        /// </summary>
        private void UpdateLeaderPosition()
        {
            this.UIText_Position.text = string.Format("{0},{1}", this._LeaderPosition.x, this._LeaderPosition.y);
            /// Nếu là GM
            if (Global.Data.RoleData.GMAuth == 1)
            {
                this.UIText_DebugWorldPos.text = string.Format("{0},{1}", Global.Data.Leader.PosX, Global.Data.Leader.PosY);
            }
        }
		#endregion

		#region Public methods
        /// <summary>
        /// Làm mới đối tượng
        /// </summary>
        public void Refresh()
		{
            this.UIImage_MaskInvisibleMinimap.gameObject.SetActive(!Loader.Loader.Maps[Global.Data.RoleData.MapCode].ShowMiniMap);
		}
		#endregion
	}
}