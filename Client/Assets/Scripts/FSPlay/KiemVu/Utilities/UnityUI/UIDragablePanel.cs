using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FSPlay.KiemVu.Utilities.UnityUI
{
    /// <summary>
    /// Panel có thể kéo qua kéo lại
    /// </summary>
    public class UIDragablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Define
        /// <summary>
        /// Đối tượng Frame tương tác
        /// </summary>
        [SerializeField]
        private RectTransform UITransform_Frame;
        #endregion

        #region Private fileds
        /// <summary>
        /// Vị trí con trỏ trước
        /// </summary>
        private Vector2 lastMousePosition;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {

        }
        #endregion

        #region Core
        /// <summary>
        /// Hàm này kiểm tra đối tượng cửa sổ có nằm trong màn hình không
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            bool isInside = false;
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            int visibleCorners = 0;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            foreach (Vector3 corner in corners)
            {
                if (rect.Contains(corner))
                {
                    visibleCorners++;
                }
            }
            if (visibleCorners == 4)
            {
                isInside = true;
            }
            return isInside;
        }

        /// <summary>
        /// Hàm này gọi đến trong suốt quá trình di chuyển con trỏ
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - lastMousePosition;
            RectTransform rect = this.UITransform_Frame;

            Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
            Vector3 oldPos = rect.position;
            rect.position = newPosition;
            if (!this.IsRectTransformInsideSreen(rect))
            {
                rect.position = oldPos;
            }
            this.lastMousePosition = currentMousePosition;
        }

        /// <summary>
        /// Hàm này gọi đến khi bắt đầu sự kiện kéo thả
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.lastMousePosition = eventData.position;
        }

        /// <summary>
        /// Hàm này gọi đến khi kết thúc quá trình kéo thả
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
        }
        #endregion
    }
}
