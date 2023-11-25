using FSPlay.GameEngine.Logic;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Minimap
{
    /// <summary>
    /// Đối tượng biểu diễn trên bản đồ nhỏ
    /// </summary>
    public class WorldNavigationMapObject : TTMonoBehaviour
    {
        /// <summary>
        /// Đối tượng được tham chiếu
        /// </summary>
        public GameObject ReferenceObject = null;

        /// <summary>
        /// Hàm này chạy liên tục mỗi frame
        /// </summary>
        private void Update()
        {
            /// Nếu bản đồ hiện tại không cho phép mở bản đồ nhỏ
            if (!Loader.Loader.Maps[Global.Data.RoleData.MapCode].ShowMiniMap)
            {
                return;
            }

            if (this.ReferenceObject != null)
            {
                this.transform.localPosition = KTGlobal.WorldPositionToWorldNavigationMapPosition(this.ReferenceObject.transform.localPosition);
            }
        }
    }
}
