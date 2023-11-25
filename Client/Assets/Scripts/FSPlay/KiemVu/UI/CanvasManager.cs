using FSPlay.KiemVu.UI.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.CoreUI;
using FSPlay.KiemVu.UI.Main.SytemNotification;
using FSPlay.KiemVu.UI.Main;

namespace FSPlay.KiemVu.UI
{
    /// <summary>
    /// Đối tượng Canvas
    /// </summary>
    public class CanvasManager : TTMonoBehaviour
    {
        #region Singleton Instance
        /// <summary>
        /// Đối tượng Canvas
        /// </summary>
        public static CanvasManager Instance { get; private set; }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            CanvasManager.Instance = this;
        }
        #endregion

        #region Define
        /// <summary>
        /// Gốc chứa UI tĩnh
        /// </summary>
        [SerializeField]
        private GameObject StaticUIRoot;

        /// <summary>
        /// Gốc chứa UI động
        /// </summary>
        [SerializeField]
        private GameObject DynamicUIRoot;

        /// <summary>
        /// Gốc chứa UI dưới cùng
        /// </summary>
        [SerializeField]
        private GameObject UnderLayUIRoot;

        /// <summary>
        /// Gốc chứa các UI trong màn hình Game
        /// </summary>
        [SerializeField]
        private GameObject MainUIRoot;

        /// <summary>
        /// Gốc chứa các UI ưu tiên hiển thị ở đầu
        /// </summary>
        [SerializeField]
        private GameObject OnTopUIRoot;

        /// <summary>
        /// Bảng thông báo
        /// </summary>
        public UIMessageBox UIMessageBox;

        /// <summary>
        /// Khung Tooltip góc trên chính giữa màn hình
        /// </summary>
        public UINotificationTip UINotificationTip;

        /// <summary>
        /// Khung chữ chạy hệ thống
        /// </summary>
        public UISystemNotification UISystemNotification;

        /// <summary>
        /// Khung tải dữ liệu gì đó
        /// </summary>
        public UILoadingProgress UILoadingProgress;

        /// <summary>
        /// Khung hiển thị máu, tên trên đầu nhân vật
        /// </summary>
        public UIRoleHeader UIRoleHeader;
        #endregion

        #region Public methods
        /// <summary>
        /// Tải UI từ Prefab tương ứng
        /// </summary>
        /// <param name="name"></param>
        public T LoadUIPrefab<T>(string name) where T : MonoBehaviour
        {
            T prefab = Resources.Load<T>("KiemVu/Prefabs/UINew/" + name);
            T obj = GameObject.Instantiate<T>(prefab);
            return obj;
        }

        /// <summary>
        /// Thêm UI vào Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void AddUI(MonoBehaviour ui)
        {
            ui.gameObject.transform.SetParent(this.DynamicUIRoot.transform, false);
        }

        /// <summary>
        /// Thêm UI vào Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void AddUI(RectTransform ui)
        {
            ui.gameObject.transform.SetParent(this.DynamicUIRoot.transform, false);
        }

        /// <summary>
        /// Thêm UI vào MainUI trong Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void AddMainUI(MonoBehaviour ui)
        {
            ui.gameObject.transform.SetParent(this.MainUIRoot.transform, false);
        }

        /// <summary>
        /// Thêm UI vào Canvas
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="isOnTop"></param>
        public void AddUI(MonoBehaviour ui, bool isOnTop)
        {
            if (!isOnTop)
            {
                ui.gameObject.transform.SetParent(this.DynamicUIRoot.transform, false);
            }
            else
            {
                ui.gameObject.transform.SetParent(this.OnTopUIRoot.transform, false);
            }
        }

        /// <summary>
        /// Thêm UI vào UnderLayer của Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void AddUnderLayerUI(MonoBehaviour ui)
        {
            ui.gameObject.transform.SetParent(this.UnderLayUIRoot.transform, false);
        }

        /// <summary>
        /// Xóa Main UI
        /// </summary>
        public void DestroyMainUI()
        {
            foreach (Transform childUI in this.MainUIRoot.transform)
            {
                GameObject.Destroy(childUI.gameObject);
            }
        }

        /// <summary>
        /// Xóa On Top UI
        /// </summary>
        public void DestroyOnTopUI()
        {
            foreach (Transform childUI in this.OnTopUIRoot.transform)
            {
                GameObject.Destroy(childUI.gameObject);
            }
        }

        /// <summary>
        /// Xóa Under Layer UI
        /// </summary>
        public void DestroyUnderLayerUI()
        {
            foreach (Transform childUI in this.UnderLayUIRoot.transform)
            {
                GameObject.Destroy(childUI.gameObject);
            }
        }

        /// <summary>
        /// Xóa Dynamic UI
        /// </summary>
        public void DestroyDynamicUI()
        {
            this.DestroyMainUI();
            foreach (Transform childUI in this.DynamicUIRoot.transform)
            {
                if (childUI.gameObject != this.MainUIRoot.gameObject)
                {
                    GameObject.Destroy(childUI.gameObject);
                }
            }
        }

        /// <summary>
        /// Xóa UI khỏi Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void RemoveUI(MonoBehaviour ui)
        {
            GameObject.Destroy(ui.gameObject);
        }
        #endregion
    }
}
