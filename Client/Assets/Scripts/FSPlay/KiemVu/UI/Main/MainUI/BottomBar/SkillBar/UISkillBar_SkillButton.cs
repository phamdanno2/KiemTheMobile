using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FSPlay.KiemVu.UI.Main.MainUI.SkillBar
{
    /// <summary>
    /// Button kỹ năng ở khung SkillBar
    /// </summary>
    public class UISkillBar_SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Define
        /// <summary>
        /// Button skill
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Skill;

        /// <summary>
        /// Image mask thời gian phục hồi kỹ năng
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_CooldownMask;

        /// <summary>
        /// Icon skill
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_Icon;

        /// <summary>
        /// Ảnh động hiệu ứng vòng sáng
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIImage_AruaEffect;

        /// <summary>
        /// Ảnh động hiệu ứng hết thời gian phục hồi kỹ năng
        /// </summary>
        [SerializeField]
        private UIAnimatedImage UIImage_CompleteAnimation;
        #endregion

        #region Properties
        /// <summary>
        /// ID kỹ năng
        /// </summary>
        public int SkillID { get; set; }

        /// <summary>
        /// Bundle chứa Icon
        /// </summary>
        public string IconBundleDir
        {
            get
            {
                return this.UIImage_Icon.BundleDir;
            }
            set
            {
                this.UIImage_Icon.BundleDir = value;
            }
        }

        /// <summary>
        /// Tên Atlas chứa Icon
        /// </summary>
        public string IconAtlasName
        {
            get
            {
                return this.UIImage_Icon.AtlasName;
            }
            set
            {
                this.UIImage_Icon.AtlasName = value;
            }
        }

        /// <summary>
        /// Tên Sprite Icon
        /// </summary>
        public string IconSpriteName
        {
            get
            {
                return this.UIImage_Icon.SpriteName;
            }
            set
            {
                this.UIImage_Icon.SpriteName = value;
            }
        }

        /// <summary>
        /// Có hiện Icon không
        /// </summary>
        public bool ShowIcon
        {
            get
            {
                return this.UIImage_Icon.gameObject.activeSelf;
            }
            set
            {
                this.UIImage_Icon.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Thời gian phục hồi
        /// </summary>
        public float CooldownTick { get; set; }

        /// <summary>
        /// Thời gian phực hồi hiện tại đã chạy còn
        /// </summary>
        public float CurrentCountDownTime { get; set; } = 0f;

        /// <summary>
        /// Sự kiện khi Skill được kích hoạt
        /// </summary>
        public Action Click { get; set; }

        /// <summary>
        /// Sự kiện khi người dùng giữ tại vị trí liên tục
        /// </summary>
        public Action Hold { get; set; }

        /// <summary>
        /// Kích hoạt hiệu ứng động của vòng sáng
        /// </summary>
        public bool ActivateAruaEffect
        {
            get
            {
                if (this.UIImage_AruaEffect == null)
                {
                    return false;
                }
                return this.UIImage_AruaEffect.gameObject.activeSelf;
            }
            set
            {
                if (this.UIImage_AruaEffect == null)
                {
                    return;
                }
                this.UIImage_AruaEffect.gameObject.SetActive(value);
                if (value)
                {
                    this.UIImage_AruaEffect.Play();
                }
            }
        }
        #endregion

        /// <summary>
        /// Chuột đang được giữ ấn xuống
        /// </summary>
        private bool mouseDown = true;

        /// <summary>
        /// Thời gian giữ chuột
        /// </summary>
        private float holdTime = 0f;

        /// <summary>
        /// Đánh dấu thời gian trước đó có Cooldown không
        /// </summary>
        private bool lastIsCooldown = false;

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.mouseDown = false;
        }

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame
        /// </summary>
        private void Update()
        {
            if (this.mouseDown)
            {
                this.holdTime += Time.deltaTime;
                if (this.holdTime >= 0.1f)
                {
                    this.Hold?.Invoke();
                    this.holdTime = 0;
                    //this.mouseDown = false;
                }
            }
            else
            {
                this.holdTime = 0;
            }

            /// Nếu kỹ năng đang trong trạng thái hồi
            if (this.CooldownTick > 0 && this.UIImage_CooldownMask != null)
            {
                if (this.CurrentCountDownTime > 0)
                {
                    /// Cập nhật thời gian Cooldown
                    this.CurrentCountDownTime -= Time.deltaTime;

                    /// Tính % thời gian đã qua
                    float percent = this.CurrentCountDownTime / this.CooldownTick;

                    /// Hiển thị lên UI Mask thời gian phục hồi kỹ năng
                    this.UIImage_CooldownMask.fillAmount = percent;

                    /// Đánh dấu trước đó có Cooldown
                    this.lastIsCooldown = true;
                }
                else
                {
                    /// Nếu trước đó có Cooldown thì thực hiện hiệu ứng hết Cooldown
                    if (this.lastIsCooldown)
                    {
                        if (this.UIImage_CompleteAnimation != null)
                        {
                            this.UIImage_CompleteAnimation.Play();
                        }
                    }

                    /// Đánh dấu trước đó không có Cooldown
                    this.lastIsCooldown = false;

                    this.CurrentCountDownTime = 0;
                    this.CooldownTick = 0;

                    /// Hiển thị lên UI Mask thời gian phục hồi kỹ năng
                    this.UIImage_CooldownMask.fillAmount = 0;
                }
            }
			else
			{
                /// Đánh dấu trước đó không có Cooldown
                this.lastIsCooldown = false;

                this.CurrentCountDownTime = 0;
                this.CooldownTick = 0;

                if (this.UIImage_CooldownMask != null)
				{
                    /// Hiển thị lên UI Mask thời gian phục hồi kỹ năng
                    this.UIImage_CooldownMask.fillAmount = 0;
                }
            }
        }

        /// <summary>
        /// Bắt sự kiện Click vào đối tượng
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            this.mouseDown = true;
        }

        /// <summary>
        /// Bắt sự kiện khi ngừng Click đối tượng
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (this.mouseDown)
            {
                this.Click?.Invoke();
                this.mouseDown = false;
            }
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Skill.onClick.AddListener(this.ButtonSkill_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button Skill được ấn
        /// </summary>
        private void ButtonSkill_Clicked()
        {
            //this.Click?.Invoke();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Làm mới
        /// </summary>
        public void Refresh()
        {
            this.holdTime = 0f;
            this.mouseDown = false;
            this.UIImage_Icon.Load();
        }
        #endregion
    }
}