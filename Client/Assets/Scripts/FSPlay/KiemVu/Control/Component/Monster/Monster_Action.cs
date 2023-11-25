using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component
{
    public partial class Monster
    {
        #region Define
        /// <summary>
        /// Cơ thể quái vật
        /// </summary>
        [SerializeField]
        private GameObject Body;

        /// <summary>
        /// Bóng quái vật
        /// </summary>
        [SerializeField]
        private GameObject Shadow;

        /// <summary>
        /// Luồng thực thi lệnh động tác ĐỨNG
        /// </summary>
        private Coroutine routineQueryStand = null;

        /// <summary>
        /// Đối tượng thực hiện động tác
        /// </summary>
        private new MonsterAnimation2D animation = null;

        #region Trail
        /// <summary>
        /// Hiệu ứng đổ bóng thân
        /// </summary>
        private SpriteTrailRenderer Trail_Body;
        #endregion
        #endregion

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

        private SpriteRenderer bodySpriteRenderer;

        /// <summary>
        /// Chiều cao của đối tượng
        /// </summary>
        public int Height
        {
            get
            {

                int height;
                if (this.RefObject.SpriteType == GameEngine.Logic.GSpriteTypes.NPC)
                {
                    height = (int) this.bodySpriteRenderer.size.y * 3 / 4;
                }
                else
                {
                    height = (int) this.bodySpriteRenderer.size.y * 5 / 6;
                }
                return height;
            }
        }

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
        /// Cập nhật dữ liệu
        /// </summary>
        public void UpdateData()
        {
            this.animation.ResID = this.ResID;
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

            this.Trail_Body.Enable = KTSystemSetting.DisableTrailEffect ? false : isActivated;
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
        /// Thiết lập hiển thị Model
        /// </summary>
        /// <param name="isVisible"></param>
        private void SetModelVisible(bool isVisible)
        {
            this.Body.gameObject.SetActive(isVisible);
        }

        /// <summary>
        /// Tiếp tục thực hiện động tác hiện tại
        /// </summary>
        public void ResumeCurrentAction()
        {
            /// Thực thi thiết lập
            this.ExecuteSetting();
            /// Nếu sau khi thực thi thiết lập trả ra kết quả ẩn đối tượng thì bỏ qua
            if (this.lastHideRole)
            {
                return;
            }

            switch (this.RefObject.CurrentAction)
            {
                case KE_NPC_DOING.do_none:
                case KE_NPC_DOING.do_stand:
                    this.Stand();
                    break;
                case KE_NPC_DOING.do_run:
                    this.Run();
                    break;
                case KE_NPC_DOING.do_attack:
                    this.Attack(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_rushattack:
                    this.SpecialAttack(this.currentActionFrameSpeed);
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
                case KE_NPC_DOING.do_hurt:
                    this.Hurt(this.currentActionFrameSpeed);
                    break;
                case KE_NPC_DOING.do_death:
                    this.Die();
                    break;
            }

            if (this.animation.IsPausing)
            {
                this.animation.Resume();
            }
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
            /// Nếu thiết lập không hiện NPC
            if (KTSystemSetting.HideNPC)
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

            this.StopAllCoroutines();
            this.RefObject = null;
            this.ResID = null;
            this.UIHeader = null;
            this.UIMinimapReference = null;
            this._ShowHPBar = false;
            this._ShowElemental = false;
            this._ShowMinimapName = false;
            this._ShowMinimapIcon = false;
            this._NameColor = default;
            this._UIHeaderOffset = default;
            this._MinimapNameColor = default;
            this._MinimapIconSize = default;
            this.currentActionFrameSpeed = 0f;
            this.otherParam = 0;
            this._Direction = default;
            this.routineQueryStand = null;
            this.actionCoroutine = null;
            this.uiHeaderOffsetChanged = false;
            this.SetModelVisible(true);
            this.lastHideRole = false;
            this.Destroyed?.Invoke();
            this.Destroyed = null;
            this.GetComponent<AudioSource>().clip = null;

            KTObjectPoolManager.Instance.ReturnToPool(this.gameObject);
        }
    }
}
