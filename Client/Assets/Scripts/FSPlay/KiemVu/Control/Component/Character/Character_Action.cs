using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.ActionSet;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Quản lý động tác nhân vật
    /// </summary>
    public partial class Character
    {
        #region Define
        /// <summary>
        /// Bóng
        /// </summary>
        [SerializeField]
        private GameObject Shadow;

        /// <summary>
        /// Bóng khi cưỡi
        /// </summary>
        [SerializeField]
        private GameObject Shadow_Horse;

        /// <summary>
        /// Đầu
        /// </summary>
        [SerializeField]
        private GameObject Head;

        /// <summary>
        /// Tóc
        /// </summary>
        [SerializeField]
        private GameObject Hair;

        /// <summary>
        /// Thân
        /// </summary>
        [SerializeField]
        private GameObject Body;

        /// <summary>
        /// Tay trái
        /// </summary>
        [SerializeField]
        private GameObject LeftArm;

        /// <summary>
        /// Tay phải
        /// </summary>
        [SerializeField]
        private GameObject RightArm;

        /// <summary>
        /// Vũ khí tay trái
        /// </summary>
        [SerializeField]
        private GameObject LeftWeapon;

        /// <summary>
        /// Vũ khí tay phải
        /// </summary>
        [SerializeField]
        private GameObject RightWeapon;

        /// <summary>
        /// Phi phong
        /// </summary>
        [SerializeField]
        private GameObject Coat;

        #region Horse
        /// <summary>
        /// Đầu ngựa
        /// </summary>
        [SerializeField]
        private GameObject HorseHead;

        /// <summary>
        /// Thân ngựa
        /// </summary>
        [SerializeField]
        private GameObject HorseBody;

        /// <summary>
        /// Đuôi ngựa
        /// </summary>
        [SerializeField]
        private GameObject HorseTail;
        #endregion

        /// <summary>
        /// Đối tượng thực hiện động tác nhân vật
        /// </summary>
        private new CharacterAnimation2D animation = null;

        /// <summary>
        /// Hiệu ứng cường hóa vũ khí trái
        /// </summary>
        [SerializeField]
        private GameObject LeftWeaponEnhanceEffects;

        /// <summary>
        /// Hiệu ứng cường hóa vũ khí phải
        /// </summary>
        [SerializeField]
        private GameObject RightWeaponEnhanceEffects;

        #region Trail
        /// <summary>
        /// Hiệu ứng đổ bóng đầu
        /// </summary>
        private SpriteTrailRenderer Trail_Head;

        /// <summary>
        /// Hiệu ứng đổ bóng tóc
        /// </summary>
        private SpriteTrailRenderer Trail_Hair;

        /// <summary>
        /// Hiệu ứng đổ bóng thân
        /// </summary>
        private SpriteTrailRenderer Trail_Body;

        /// <summary>
        /// Hiệu ứng đổ bóng tay trái
        /// </summary>
        private SpriteTrailRenderer Trail_LeftArm;

        /// <summary>
        /// Hiệu ứng đổ bóng tay phải
        /// </summary>
        private SpriteTrailRenderer Trail_RightArm;

        /// <summary>
        /// Hiệu ứng đổ bóng vũ khí trái
        /// </summary>
        private SpriteTrailRenderer Trail_LeftWeapon;

        /// <summary>
        /// Hiệu ứng đổ bóng vũ khí phải
        /// </summary>
        private SpriteTrailRenderer Trail_RightWeapon;

        /// <summary>
        /// Hiệu ứng đổ bóng phi phong
        /// </summary>
        private SpriteTrailRenderer Trail_Coat;

        /// <summary>
        /// Hiệu ứng đổ bóng đầu ngựa
        /// </summary>
        private SpriteTrailRenderer Trail_HorseHead;

        /// <summary>
        /// Hiệu ứng đổ bóng thân ngựa
        /// </summary>
        private SpriteTrailRenderer Trail_HorseBody;

        /// <summary>
        /// Hiệu ứng đổ bóng đuôi ngựa
        /// </summary>
        private SpriteTrailRenderer Trail_HorseTail;
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Model của đối tượng
        /// </summary>
        public GameObject Model
        {
            get
            {
                return this.Body.transform.parent.gameObject;
            }
        }

        private Vector3? _OriginModelPos;
        /// <summary>
        /// Vị trí gốc của Model
        /// </summary>
        public Vector3 OriginModelPos
        {
            get
            {
                if (!this._OriginModelPos.HasValue)
                {
                    this._OriginModelPos = this.Model.transform.localPosition;
                }
                return this._OriginModelPos.Value;
            }
        }
        #endregion

        #region Loader
        /// <summary>
        /// Thay đổi trang phục
        /// </summary>
        /// <param name="armorID"></param>
        public void ChangeArmor(int armorID)
        {
            this.Data.ArmorID = armorID;
            this.ReloadAnimation();
        }

        /// <summary>
        /// Thay đổi mũ
        /// </summary>
        /// <param name="helmID"></param>
        public void ChangeHelm(int helmID)
        {
            this.Data.HelmID = helmID;
            this.ReloadAnimation();
        }

        /// <summary>
        /// Thay đổi vũ khí
        /// </summary>
        /// <param name="weaponID"></param>
        public void ChangeWeapon(int weaponID)
        {
            this.Data.WeaponID = weaponID;
            this.ReloadAnimation();
        }

        /// <summary>
        /// Thay đổi phi phong
        /// </summary>
        /// <param name="mantleID"></param>
        public void ChangeMantle(int mantleID)
        {
            this.Data.MantleID = mantleID;
            this.ReloadAnimation();
        }

        /// <summary>
        /// Thay đổi ngựa
        /// </summary>
        /// <param name="horseID"></param>
        public void ChangeHorse(int horseID)
        {
            this.Data.HorseID = horseID;
            this.ReloadAnimation();
        }

        /// <summary>
        /// Cập nhật RoleData
        /// </summary>
        public void UpdateRoleData()
        {
            this.animation.Data = this.Data;
        }
        #endregion

        /// <summary>
        /// Luồng thực thi hiệu ứng Async
        /// </summary>
        private Coroutine actionCoroutine;

        /// <summary>
        /// Hàm này gọi đến ngay khi đối tượng được tạo ra
        /// </summary>
        private void InitAction()
        {
        }

        /// <summary>
        /// Luồng thực hiện tải lại động tác
        /// </summary>
        private Coroutine reloadAnimationCoroutine = null;

        /// <summary>
        /// Tải lại động tác
        /// </summary>
        private void ReloadAnimation()
        {
            if (this.reloadAnimationCoroutine != null)
            {
                this.StopCoroutine(this.reloadAnimationCoroutine);
            }
            this.reloadAnimationCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                this.animation.Reload();
                this.ResumeCurrentAction();
                this.reloadAnimationCoroutine = null;
            }));
        }

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

            this.Trail_Head.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_Hair.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_Body.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_LeftArm.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_LeftWeapon.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_RightArm.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_RightWeapon.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_Coat.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_HorseHead.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_HorseBody.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
            this.Trail_HorseTail.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
        }


        /// <summary>
        /// Thiết lập Sorting Order
        /// </summary>
        public void SortingOrderHandler()
        {
            Vector2 currentPos = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(currentPos.x, currentPos.y, currentPos.y / 10000);
        }

        /// <summary>
        /// Tiếp tục thực hiện động tác hiện tại
        /// </summary>
        public void ResumeCurrentAction()
        {
            /// Làm mới trạng thái cưỡi
            this.UpdateRidingState();
            /// Thực thi thiết lập
            this.ExecuteSetting();
            /// Nếu sau khi thực thi thiết lập trả ra kết quả ẩn đối tượng thì bỏ qua
            if (this.lastHideRole)
            {
                return;
            }

            /// Nếu là động tác cưỡi ngựa thì chuyển sang bóng ngựa
            if (this.Data.IsRiding)
            {
                this.Shadow_Horse.gameObject.SetActive(true);
                this.Shadow.gameObject.SetActive(false);
            }
            else
            {
                this.Shadow_Horse.gameObject.SetActive(false);
                this.Shadow.gameObject.SetActive(true);
            }

            switch (this.RefObject.CurrentAction)
            {
                case KE_NPC_DOING.do_sit:
                    this.Sit(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_jump:
                    this.Jump(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_stand:
                    this.Stand();
                    break;
                case KE_NPC_DOING.do_run:
                    this.Run();
                    break;
                case KE_NPC_DOING.do_walk:
                    this.Walk();
                    break;
                case KE_NPC_DOING.do_attack:
                    this.Attack(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_rushattack:
                    this.SpecialAttack(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_magic:
                    this.PlayMagicAction(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_hurt:
                    this.Hurt(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_death:
                    this.Die();
                    break;
                case KE_NPC_DOING.do_manyattack:
                    this.AttackMultipleTimes(this.currentActionFrameSpeed, this.otherParam);
                    break;
                case KE_NPC_DOING.do_runattackmany:
                    this.SpecialAttackMultipleTimes(this.currentActionFrameSpeed, this.otherParam);
                    break;
                case KE_NPC_DOING.do_runattack:
                    this.RunAttack(this.currentActionFrameSpeed);
                    break;
            }

            if (this.animation.IsPausing)
            {
                this.animation.Resume();
            }
        }

        /// <summary>
        /// Thiết lập hiển thị Model
        /// </summary>
        /// <param name="isVisible"></param>
        private void SetModelVisible(bool isVisible)
        {
            this.Body.gameObject.SetActive(isVisible);
            this.LeftArm.gameObject.SetActive(isVisible);
            this.LeftWeapon.gameObject.SetActive(isVisible);
            this.RightArm.gameObject.SetActive(isVisible);
            this.RightWeapon.gameObject.SetActive(isVisible);
            this.Head.gameObject.SetActive(isVisible);
            this.Hair.gameObject.SetActive(isVisible);
            this.Coat.gameObject.SetActive(isVisible);
            this.HorseHead.gameObject.SetActive(isVisible);
            this.HorseBody.gameObject.SetActive(isVisible);
            this.HorseTail.gameObject.SetActive(isVisible);
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
            /// Nếu thiết lập không hiện nhân vật
            if ((this.RefObject == Global.Data.Leader && KTSystemSetting.HideRole) || (this.RefObject != Global.Data.Leader && KTSystemSetting.HideOtherRole))
            {
                return;
            }

            this.animation.Resume();
        }

        /// <summary>
        /// Thay đổi màu của đối tượng
        /// </summary>
        /// <param name="color"></param>
        public void MixColor(Color color)
        {
            this.groupColor.Color = color;
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        public void Destroy()
        {
            this.RemoveAllEffects();
            this.DestroyUI();

            this.MaxAlpha = 1f;
            this.groupColor.Alpha = 1f;
            this.StopAllCoroutines();
            this.RefObject = null;
            this.Data = null;
            this._ShowMPBar = false;
            this._ShowMinimapName = false;
            this._ShowMinimapIcon = false;
            this._MinimapNameColor = default;
            this._MinimapIconSize = default;
            this._Direction = default;
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
            this.UIHeader = null;
            this.UIMinimapReference = null;
            this.UIRoleShopName = null;
            this.actionCoroutine = null;
            this.reloadAnimationCoroutine = null;
            this.SetModelVisible(true);
            this.lastHideRole = false;
            this.Destroyed?.Invoke();
            this.Destroyed = null;
            this.GetComponent<AudioSource>().clip = null;

            KTObjectPoolManager.Instance.ReturnToPool(this.gameObject);
        }
    }
}
