using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FSPlay.GameEngine.Logic;
using FSPlay.GameFramework.Logic;
using UnityEngine.Networking;
using System.Xml.Linq;
using FSPlay.Drawing;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Control.Component;

namespace FSPlay.KiemVu.UI.LoadingMap
{
    /// <summary>
    /// Khung tải bản đồ
    /// </summary>
    public class UILoadingMap : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text thông báo của Progress Bar
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ProgressBarText;

        /// <summary>
        /// Text phần trăm tiến trình
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ProgresBarPercentText;

        /// <summary>
        /// Text trang trí
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_DecorationText;

        /// <summary>
        /// Progress Bar tiến trình
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Slider UISlider_ProgressBar;

        /// <summary>
        /// Kích hoạt chế độ Debug
        /// </summary>
        [SerializeField]
        private bool EnableDebug = true;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi hoàn tất công việc
        /// </summary>
        public EventHandler WorkFinished { get; set; } = null;

        /// <summary>
        /// Sự kiện khi đối tượng bị xóa
        /// </summary>
        public Action DoDestroy { get; set; }

        /// <summary>
        /// ID bản đồ cần tải xuống
        /// </summary>
        public int MapCode { get; set; } = 0;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            //this.WorkFinished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng bị hủy
        /// </summary>
        private void OnDestroy()
        {
            this.DoDestroy?.Invoke();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Thiết lập ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIText_DecorationText.text = "";
            this.UIText_ProgressBarText.text = "Đang tải tài nguyên bản đồ, xin chờ giây lát...";
            this.UIText_ProgresBarPercentText.text = "0%";
            this.UISlider_ProgressBar.value = 0;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Luồng thực hiện tải tài nguyên 2D
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoLoad2DMapRes()
        {
            GameObject root2DScene = GameObject.Find("Scene 2D Root");
            if (root2DScene != null)
            {
                GameObject.Destroy(root2DScene);
                yield return null;
            }
            root2DScene = new GameObject("Scene 2D Root");
            root2DScene.transform.localPosition = Vector3.zero;

            GameObject go = new GameObject("Empty Scene");
            go.transform.SetParent(root2DScene.transform, false);
            go.transform.localPosition = Vector3.zero;
            Map map2D = go.AddComponent<Map>();
            Global.CurrentMap = map2D;
            map2D.MapCode = this.MapCode;
            map2D.LeaderPosition = new Vector2(Global.Data.RoleData.PosX, Global.Data.RoleData.PosY);
            map2D.ReportProgress = this.ReportProgressBar;
            map2D.Finish = () => {
                this.WorkFinished?.Invoke(this, EventArgs.Empty);
            };
            map2D.Load();

            this.UIText_DecorationText.text = map2D.Name;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Gọi đến báo cáo tiến trình tải xuống
        /// </summary>
        /// <param name="percent"></param>
        public void ReportProgressBar(int percent, string message)
        {
            this.UIText_ProgresBarPercentText.text = string.Format("{0}%", percent);

            this.UISlider_ProgressBar.value = percent / (float)100f;
            if (this.EnableDebug)
            {
                this.UIText_ProgressBarText.text = message;
            }
        }

        /// <summary>
        /// Bắt đầu tải xuống tài nguyên bản đồ
        /// </summary>
        public void BeginLoad2DMapRes()
        {
            this.StartCoroutine(this.DoLoad2DMapRes());
        }
        #endregion
    }
}