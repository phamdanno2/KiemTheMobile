using FSPlay.KiemVu.UI.CoreUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSPlay.KiemVu.UI
{
    /// <summary>
    /// Đối tượng Minimap Canvas
    /// </summary>
    public class MinimapCanvasManager : MonoBehaviour
    {
        #region Singleton Instance
        /// <summary>
        /// Đối tượng Canvas
        /// </summary>
        public static MinimapCanvasManager Instance { get; private set; }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            MinimapCanvasManager.Instance = this;
        }
        #endregion

        #region Define
        /// <summary>
        /// Khung biểu diễn đối tượng trên bản đồ nhỏ
        /// </summary>
        public UIMinimapReference UIMinimapReference;
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
        /// Thêm UI vào Minimap Canvas
        /// </summary>
        /// <param name="ui"></param>
        public void AddUI(MonoBehaviour ui)
        {
            ui.gameObject.transform.SetParent(this.gameObject.transform, false);
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

