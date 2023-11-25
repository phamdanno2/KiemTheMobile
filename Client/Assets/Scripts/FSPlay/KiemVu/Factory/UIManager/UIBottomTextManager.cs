using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.CoreUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Factory.UIManager
{
    /// <summary>
    /// Quản lý UIBottomText
    /// </summary>
    public class UIBottomTextManager : TTMonoBehaviour
    {
        #region Singleton - Instance
        /// <summary>
        /// Quản lý UIBottomText
        /// </summary>
        public static UIBottomTextManager Instance { get; private set; }
        #endregion

        #region Constants
        /// <summary>
        /// Thời gian Delay mỗi lần thực thi 1 phần tử
        /// </summary>
        private const float ShowDelayEach = 0.75f;
        #endregion

        #region Private fields
        /// <summary>
        /// Danh sách các phần tử
        /// </summary>
        private readonly Queue<UIFlyingText> elements = new Queue<UIFlyingText>();
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            UIBottomTextManager.Instance = this;
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.StartCoroutine(this.DoLogic());
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Hiển thị thông tin Text tương ứng
        /// </summary>
        /// <param name="uiText"></param>
        private void DoPlayText(UIFlyingText uiText)
        {
            /// Hiện đối tượng
            uiText.gameObject.SetActive(true);
            /// Thực thi
            uiText.Play();
        }

        /// <summary>
        /// Thực thi Logic
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoLogic()
        {
            while (true)
            {
                /// Nếu tồn tại phần tử cần Hint
                if (this.elements.Count > 0)
                {
                    /// Lấy phần tử tương ứng ra khỏi danh sách
                    UIFlyingText uiText = this.elements.Dequeue();

                    /// Thực hiện biểu diễn
                    this.DoPlayText(uiText);
                }
                yield return new WaitForSeconds(UIBottomTextManager.ShowDelayEach);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Làm rỗng danh sách
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
        }

        /// <summary>
        /// Thêm thông báo
        /// </summary>
        /// <param name="uiText"></param>
        public void AddText(UIFlyingText uiText)
        {
            /// Ẩn đối tượng
            uiText.gameObject.SetActive(false);
            /// Thêm vào danh sách chờ
            this.elements.Enqueue(uiText);
        }
        #endregion
    }
}
