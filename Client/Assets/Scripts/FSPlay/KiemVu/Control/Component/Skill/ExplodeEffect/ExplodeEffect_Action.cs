using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System.Collections;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component.Skill
{
    /// <summary>
    /// Quản lý hiệu ứng
    /// </summary>
    public partial class ExplodeEffect
    {
        #region Define
        /// <summary>
        /// Thân đối tượng
        /// </summary>
        [SerializeField]
        private GameObject Body;

        /// <summary>
        /// Đối tượng thực hiện động tác
        /// </summary>
        private new BulletExplodeEffectAnimation2D animation = null;
        #endregion

        #region Private methods
        /// <summary>
        /// Khởi tạo động tác
        /// </summary>
        private void InitActions()
        {
            
        }

        /// <summary>
        /// Thiết lập Sorting Order
        /// </summary>
        private void SortingOrderHandler()
        {
            Vector2 currentPos = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(currentPos.x, currentPos.y, currentPos.y / 10000);
        }
        #endregion

        /// <summary>
        /// Thiết lập hiển thị Model
        /// </summary>
        /// <param name="isVisible"></param>
        private void SetModelVisible(bool isVisible)
        {
            this.Body.gameObject.SetActive(isVisible);
        }


        /// <summary>
        /// Tạm dừng thực hiện tất cả động tác
        /// </summary>
        public void PauseAllActions()
        {
            this.animation.Pause();
        }

        /// <summary>
        /// Tiếp tục thực hiện động tác
        /// </summary>
        public void ResumeActions()
        {
            /// Nếu thiết lập không hiện hiệu ứng nổ
            if (KTSystemSetting.HideSkillExplodeEffect)
            {
                return;
            }

            this.animation.Resume();
        }

        /// <summary>
        /// Luồng thực thi hiệu ứng
        /// </summary>
        private Coroutine actionCoroutine;

        /// <summary>
        /// Tiếp tục thực hiện trạng thái hiện tại
        /// </summary>
        private void ContinueCurrentState()
        {
            /// Thực thi thiết lập
            this.ExecuteSetting();
            /// Nếu sau khi thực thi thiết lập trả ra kết quả ẩn đối tượng thì bỏ qua
            if (this.lastHideRole)
            {
                this.Destroy();
                return;
            }

            this.RefreshAction();
            this.Play();
        }

        /// <summary>
        /// Làm mới động tác
        /// </summary>
        public void RefreshAction()
		{
            this.animation.ResID = this.ResID;
		}

        /// <summary>
        /// Thực hiện hiệu ứng
        /// </summary>
        public void Play()
        {
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }

            /// Nếu không có mục tiêu thì Play tại vị trí chỉ định
            if (this.Target == null)
            {
                this.gameObject.transform.localPosition = this.Position;
            }

            /// <summary>
            /// Thực hiện chạy hiệu ứng
            /// </summary>
            IEnumerator DoPlay()
            {
                if (this.Delay > 0)
				{
                    yield return new WaitForSeconds(this.Delay);
				}
                yield return this.animation.DoActionAsync(0.5f);
                yield return new WaitForSeconds(0.5f);
                this.Destroy();
            }
            this.actionCoroutine = this.StartCoroutine(DoPlay());
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        public void Destroy()
        {
            this.StopAllCoroutines();
            this.ResID = -1;
            this.Position = default;
            this.Target = null;
            this.Delay = 0;
            this.isReady = false;
            this.Destroyed?.Invoke();
            this.Destroyed = null;
            KTObjectPoolManager.Instance.ReturnToPool(this.gameObject);
        }
    }
}
