﻿using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Thực hiện động tác
    /// </summary>
    public partial class Character
    {
        /// <summary>
        /// Thời gian thực thi động tác hiện tại
        /// </summary>
        private float currentActionFrameSpeed = 0f;

        /// <summary>
        /// Tham biến khác
        /// </summary>
        private int otherParam = 0;

        #region Ngồi
        /// <summary>
        /// Thực hiện động tác ngồi
        /// </summary>
        /// <param name="duration"></param>
        public void Sit(float duration)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu đang trong trạng thái cưỡi
            else if (this.Data.IsRiding)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Sit, this.Direction, duration));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion

        #region Nhảy
        /// <summary>
        /// Thực hiện động tác nhảy
        /// </summary>
        public void Jump(float duration)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu đang trong trạng thái cưỡi
            else if (this.Data.IsRiding)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Jump, this.Direction, duration));

            this.ActivateTrailEffect(true);

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = duration;
            this.otherParam = 0;
        }
        #endregion

        #region Đứng
        /// <summary>
        /// Đứng ngay lập tức
        /// </summary>
        public void Stand()
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.NormalStand, this.Direction, KTGlobal.StandActionTime, true, 0f));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion

        #region Đứng ở trạng thái tấn công
        /// <summary>
        /// Đứng ngay lập tức
        /// </summary>
        public void FightStand()
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.FightStand, this.Direction, KTGlobal.StandActionTime, true, 0f));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion

        #region Chạy
        /// <summary>
        /// Thực hiện động tác di chuyển
        /// </summary>
        public void Run()
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            /// Thời gian thực hiện 1 vòng động tác
            float framePlaySpeed = KTGlobal.MoveSpeedToFrameDuration(this.RefObject.MoveSpeed);

            if (this.Data.IsRiding)
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Run, this.Direction, framePlaySpeed, true, 0, true));
            }
            else
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Run, this.Direction, framePlaySpeed, true, 0, true));
            }

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion

        #region Đi bộ
        /// <summary>
        /// Thực hiện động tác đi bộ
        /// </summary>
        public void Walk()
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            /// Tốc độ thực thi 1 vòng động tác
            float framePlaySpeed = KTGlobal.MoveSpeedToFrameDuration(this.RefObject.MoveSpeed);

            if (this.Data.IsRiding)
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Walk, this.Direction, framePlaySpeed, true, 0, true));
            }
            else
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Walk, this.Direction, framePlaySpeed, true, 0, true));
            }

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion

        #region Tấn công
        /// <summary>
        /// Thực hiện động tác tấn công
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        public void Attack(float framePlaySpeed)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.NormalAttack, this.Direction, framePlaySpeed, false, 0, false));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = 0;
        }
        #endregion

        #region Tấn công đặc biệt
        /// <summary>
        /// Thực hiện động tác tấn công
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        public void SpecialAttack(float framePlaySpeed)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.CritAttack, this.Direction, framePlaySpeed, false, 0, false));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = 0;
        }
        #endregion

        #region Tấn công nhiều lần
        /// <summary>
        /// Thực hiện động tác tấn công số lần cố định liên tiếp không ảnh hưởng tốc đánh
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        /// <param name="count"></param>
        public void AttackMultipleTimes(float framePlaySpeed, int count)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }

            int nAttackCount = 0;
            void DoAttackContinuously()
            {
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.NormalAttack, this.Direction, framePlaySpeed, false, 0, false, () => {
                    nAttackCount++;
                    if (nAttackCount >= count)
                    {
                        return;
                    }
                    else
                    {
                        DoAttackContinuously();
                    }
                }));
            }
            DoAttackContinuously();

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = count;
        }
        #endregion

        #region Tấn công nhiều lần đặc biệt
        /// <summary>
        /// Thực hiện động tác tấn công số lần cố định liên tiếp không ảnh hưởng tốc đánh
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        /// <param name="count"></param>
        public void SpecialAttackMultipleTimes(float framePlaySpeed, int count)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }

            int nAttackCount = 0;
            void DoAttackContinuously()
            {
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.CritAttack, this.Direction, framePlaySpeed, false, 0, false, () => {
                    nAttackCount++;
                    if (nAttackCount >= count)
                    {
                        return;
                    }
                    else
                    {
                        DoAttackContinuously();
                    }
                }));
            }
            DoAttackContinuously();

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = count;
        }
        #endregion

        #region Chạy tấn công
        /// <summary>
        /// Thực hiện động tác di chuyển
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        public void RunAttack(float framePlaySpeed)
        {
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.Data.IsRiding)
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.RunAttack, this.Direction, framePlaySpeed, true, 0, true));
            }
            else
            {
                if (this.actionCoroutine != null)
                {
                    this.StopCoroutine(this.actionCoroutine);
                }
                this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.RunAttack, this.Direction, framePlaySpeed, true, 0, true));
            }

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = 0;
        }
        #endregion

        #region Sử dụng phép
        /// <summary>
        /// Thực hiện động tác xuất chiêu
        /// </summary>
        /// <param name="framePlaySpeed"></param>
        public void PlayMagicAction(float framePlaySpeed)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Magic, this.Direction, framePlaySpeed, false, 0, false));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = 0;
        }
        #endregion

        #region Bị thương
        /// <summary>
        /// Thực hiện động tác bị thọ thương
        /// </summary>
        public void Hurt(float framePlaySpeed)
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu đối tượng đã chết
            if (this.RefObject.IsDeath)
            {
                return;
            }
            /// Nếu thiết lập không hiện nhân vật
            else if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Wound, this.Direction, framePlaySpeed, false, 0, false));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = framePlaySpeed;
            this.otherParam = 0;
        }
        #endregion

        #region Chết
        /// <summary>
        /// Thực hiện động tác chết
        /// </summary>
        public void Die()
        {
            this.ActivateTrailEffect(false);
            if (!this.gameObject)
            {
                return;
            }
            else if (!this.gameObject.activeSelf)
            {
                return;
            }

            /// Nếu thiết lập không hiện nhân vật
            if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
            }
            this.actionCoroutine = this.StartCoroutine(this.animation.DoActionAsync(this.Data.IsRiding, PlayerActionType.Die, this.Direction, KTGlobal.DieActionTime));

            /// Cập nhật thời gian thực hiện động tác hiện tại
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
        }
        #endregion
    }
}
