using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Utilities.UnityComponent;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Quản lý các sự kiện xảy ra với đối tượng
    /// </summary>
    public partial class Character : IEvent
    {
        /// <summary>
        /// Hàm này gọi đến ngay khi đối tượng được tạo ra
        /// </summary>
        private void InitEvents()
        {
            if (this.Model.transform.parent.GetComponent<ClickableCollider2D>() != null)
            {
                this.Model.transform.parent.GetComponent<ClickableCollider2D>().OnClick = () => {
                    this.OnClick();
                };
            }
        }

        /// <summary>
        /// Sự kiện khi đối tượng được chọn
        /// </summary>
        public void OnClick()
        {
            /// Nếu là người chơi khác
            if (Global.Data.RoleData.RoleID != this.RefObject.RoleID)
            {
                SkillManager.SelectedTarget = this.RefObject;
                KTAutoFightManager.Instance.ChangeAutoFightTarget(this.RefObject);
                //KTDebug.LogError(string.Format("{0} is clicked.", this.RefObject.RoleName));
                Global.Data.GameScene.OtherRoleClick(this.RefObject);
            }
        }

        /// <summary>
        /// Sự kiện khi vị trí của đối tượng thay đổi
        /// </summary>
        public void OnPositionChanged()
        {

        }
    }
}
