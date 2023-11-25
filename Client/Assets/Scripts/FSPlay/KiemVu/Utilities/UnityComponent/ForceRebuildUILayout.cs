using UnityEngine;
using UnityEngine.UI;

namespace FSPlay.KiemVu.Utilities.UnityComponent
{
    /// <summary>
    /// Buộc đối tượng Layout trong UnityEngine.Canvas build lại
    /// </summary>
    public class ForceRebuildUILayout : TTMonoBehaviour
    {
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            if (this.gameObject.GetComponent<RectTransform>() != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.GetComponent<RectTransform>());
            }
        }
    }
}

