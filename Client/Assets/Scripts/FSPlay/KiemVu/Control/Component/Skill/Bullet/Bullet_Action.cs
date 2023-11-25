﻿using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component.Skill
{
    /// <summary>
    /// Quản lý động tác
    /// </summary>
    public partial class Bullet
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
        private new BulletAnimation2D animation = null;

        #region Trail
        /// <summary>
        /// Hiệu ứng đổ bóng thân
        /// </summary>
        private SpriteTrailRenderer Trail_Body;
        #endregion
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
            /// Nếu thiết lập không hiện đạn
            if (KTSystemSetting.HideSkillBullet)
            {
                return;
            }

            this.animation.Resume();
        }

        /// <summary>
        /// Luồng thực thi hiệu ứng Async
        /// </summary>
        private Coroutine actionCoroutine;

        /// <summary>
        /// Luồng thực hiện bay
        /// </summary>
        private Coroutine flyCoroutine;

        /// <summary>
        /// Kích hoạt hiệu ứng bóng
        /// </summary>
        /// <param name="isActivated"></param>
        public void ActivateTrailEffect(bool isActivated)
        {
            /// Nếu FPS không phải mức cao thì mặc định Disable
            if (MainGame.Instance.GetRenderQuality() != MainGame.RenderQuality.High)
            {
                isActivated = false;
            }

            this.Trail_Body.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
        }

        /// <summary>
        /// Làm mới động tác
        /// </summary>
        public void RefreshAction()
		{
            this.animation.ResID = this.ResID;
		}

        /// <summary>
        /// Thực hiện bay
        /// </summary>
        public void Fly(bool isChangePos = false)
        {
            this.State = BulletState.Flying;
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            this.StartDelay();

            IEnumerator WaitUntilReady()
            {
                while (!this.isReady)
                {
                    yield return null;
                }
                this.DoFly(isChangePos);
            }
            if (!this.isReady)
            {
                this.StartCoroutine(WaitUntilReady());
            }
            else
            {
                this.DoFly(isChangePos);
            }
        }

        /// <summary>
        /// Thực hiện đạn bay
        /// </summary>
        private void DoFly(bool isChangeDir)
        {
            if (!this || !this.gameObject || !this.gameObject.activeSelf)
            {
                return;
            }
            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            if (this.flyCoroutine != null)
            {
                this.StopCoroutine(this.flyCoroutine);
            }

            /// Nếu không có mục tiêu đuổi
            if (this.ChaseTarget == null)
            {
                /// Nếu không phải đạn đứng yên tại chỗ thì bắt đầu bay
                if (!isChangeDir && this.FromPos != this.ToPos && this.Velocity > 0)
                {
                    this.flyCoroutine = this.StartCoroutine(this.DoLinearFly());
                }
                /// Nếu là đạn đứng yên một chỗ
                else if (!isChangeDir)
                {
                    this.flyCoroutine = this.StartCoroutine(this.DoStaticExplode());
                }
                /// Lỗi gì đó
                else
                {
                    /// Ngừng thực hiện động tác
                    this.StopAnimation();
                    /// Tự hủy
                    this.Destroy();
                }
            }
            /// Nếu có mục tiêu đuổi
            else
            {
                this.flyCoroutine = this.StartCoroutine(this.DoChaseTarget());
            }
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        public void Destroy()
        {
            this.StopAllCoroutines();
            this.State = BulletState.None;
            this.ResID = 0;
            this.FromPos = Vector2.zero;
            this.ToPos = Vector2.zero;
            this.ChaseTarget = null;
            this.Velocity = 0;
            this.MaxLifeTime = 20f;
            this.AnimationLifeTime = 0.5f;
            this.Direction = Direction16.None;
            this.RepeatAnimation = false;
            this.Delay = 0;
            this.Caster = null;
            this.isReady = false;
            this.actionCoroutine = null;
            this.flyCoroutine = null;
            this.Destroyed?.Invoke();
            this.Destroyed = null;
            KTObjectPoolManager.Instance.ReturnToPool(this.gameObject);
        }
    }
}
