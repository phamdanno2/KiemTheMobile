using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Network.Skill;
using FSPlay.KiemVu.UI.Main.MainUI.SkillBar;
using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.MainUI
{
    /// <summary>
    /// Khung kỹ năng ở Main UI
    /// </summary>
    public class UISkillBar : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button Skill chính
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_Main;

        /// <summary>
        /// Button Skill phụ 1
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_SubSkill1;

        /// <summary>
        /// Button Skill phụ 2
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_SubSkill2;

        /// <summary>
        /// Button Skill phụ 3
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_SubSkill3;

        /// <summary>
        /// Button Skill phụ 3
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_SubSkill4;

        /// <summary>
        /// Button kỹ năng vòng sáng
        /// </summary>
        [SerializeField]
        private UISkillBar_SkillButton UI_SkillButton_AruaSkill;

        /// <summary>
        /// Toggle đổi tay
        /// </summary>
        [SerializeField]
        private UIToggleSprite UIToggle_SkillChangeHand;

        /// <summary>
        /// Button chuyển trạng thái cưỡi
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ToggleMount;

        /// <summary>
        /// Button nhảy
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Jump;

        /// <summary>
        /// Button ngồi
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Sit;

        /// <summary>
        /// Button mở Auto Farm
        /// </summary>
        [SerializeField]
        private UIButtonSprite UIButton_AutoFarm;

        /// <summary>
        /// Button mở Auto PK
        /// </summary>
        [SerializeField]
        private UIButtonSprite UIButton_AutoPK;

        /// <summary>
        /// Toggle khóa di chuyển JoyStick
        /// </summary>
        [SerializeField]
        private UIToggleSprite UIToggle_LockJoyStickMove;

        /// <summary>
        /// Sprite mở Auto Farm
        /// </summary>
        [SerializeField]
        private string AutoFarmSprite_Enable;

        /// <summary>
        /// Sprite ngừng Auto Farm
        /// </summary>
        [SerializeField]
        private string AutoFarmSprite_Disable;

        /// <summary>
        /// Sprite mở Auto PK
        /// </summary>
        [SerializeField]
        private string AutoPKSprite_Enable;

        /// <summary>
        /// Sprite ngừng Auto PK
        /// </summary>
        [SerializeField]
        private string AutoPKSprite_Disable;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi nút Skill được ấn
        /// </summary>
        public Action<int, bool> SkillButtonClicked { get; set; }

        /// <summary>
        /// Sự kiện khi nút Skill được giữ
        /// </summary>
        public Action<int> SkillButtonHeld { get; set; }

        /// <summary>
        /// Sự kiện khi nút kỹ năng vòng sáng được ấn
        /// </summary>
        public Action AruaButtonClicked { get; set; }

        /// <summary>
        /// Sự kiện khi nút kỹ năng vòng sáng được giữ
        /// </summary>
        public Action AruaButtonHeld { get; set; }

        /// <summary>
        /// Sự kiện khi nút nhảy được ấn
        /// </summary>
        public Action JumpClicked { get; set; }

        /// <summary>
        /// Sự kiện khi nút ngồi được ấn
        /// </summary>
        public Action SitClicked { get; set; }

        /// <summary>
        /// Sự kiện mở Auto Farm
        /// </summary>
        public Action AutoFarm { get; set; }

        /// <summary>
        /// Sự kiện mở Auto PK
        /// </summary>
        public Action AutoPK { get; set; }

        /// <summary>
        /// Sự kiện mở khung thiết lập tự đánh
        /// </summary>
        public Action OpenAutoSetting { get; set; }

        /// <summary>
        /// Sự kiện khi nút đổi trạng thái cưỡi được ấn
        /// </summary>
        public Action ToggleMountClicked { get; set; }

        /// <summary>
        /// Sự kiện khóa di chuyển JoyStick
        /// </summary>
        public Action<bool> LockJoyStickMove { get; set; }

        /// <summary>
        /// Đang hiển thị kỹ năng tay trái
        /// </summary>
        public bool ShowMainSkill
        {
            get
            {
                return this.UIToggle_SkillChangeHand.Active;
            }
            set
            {
                this.UIToggle_SkillChangeHand.Active = value;
            }
        }

        private bool _AutoFarmEnable = false;
        /// <summary>
        /// Kích hoạt Auto Farm
        /// </summary>
        public bool AutoFarmEnable
        {
            get
            {
                return this._AutoFarmEnable;
            }
            set
            {
                this._AutoFarmEnable = value;

                if (value)
                {
                    this.UIButton_AutoFarm.NormalSpriteName = this.AutoFarmSprite_Enable;
                    this.UIButton_AutoFarm.Refresh(true);
                }
                else
                {
                    this.UIButton_AutoFarm.NormalSpriteName = this.AutoFarmSprite_Disable;
                    this.UIButton_AutoFarm.Refresh(true);
                }
            }
        }

        private bool _AutoPKEnable = false;
        /// <summary>
        /// Kích hoạt Auto PK
        /// </summary>
        public bool AutoPKEnable
		{
			get
			{
                return this._AutoPKEnable;
			}
			set
			{
                this._AutoPKEnable = value;

                if (value)
				{
                    this.UIButton_AutoPK.NormalSpriteName = this.AutoPKSprite_Enable;
                    this.UIButton_AutoPK.Refresh(true);
                }
				else
				{
                    this.UIButton_AutoPK.NormalSpriteName = this.AutoPKSprite_Disable;
                    this.UIButton_AutoPK.Refresh(true);
                }
			}
		}

        /// <summary>
        /// Kích hoạt vòng sáng đang được thiết lập
        /// </summary>
        public bool ActivateArua { get; private set; } = false;

        /// <summary>
        /// ID kỹ năng vòng sáng hiện tại
        /// </summary>
        public int AruaSkillID { get; private set; } = -1;
        #endregion

        /// <summary>
        /// Danh sách kỹ năng được xếp vào ô kỹ năng nhanh
        /// </summary>
        private readonly int[] skills = new int[10];

        /// <summary>
        /// Luồng tự động lưu lại thay đổi thiết lập ở khung kỹ năng
        /// </summary>
        private Coroutine autoSaveWhenSkillChangedCoroutine = null;

        /// <summary>
        /// Luồng tự lưu lại thay đổi kỹ năng vòng sáng
        /// </summary>
        private Coroutine autoSaveAruaCoroutine = null;

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.ShowMainSkill = true;
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIToggle_SkillChangeHand.OnSelected = this.ToggleChangeSkill_Selected;
            this.UIButton_Jump.onClick.AddListener(this.ButtonJump_Clicked);
            this.UIButton_Sit.onClick.AddListener(this.ButtonSit_Clicked);
            this.UIButton_ToggleMount.onClick.AddListener(this.ButtonToggleMount_Clicked);
            this.UIButton_AutoFarm.Click = this.ButtonAutoFarm_Selected;
            this.UIButton_AutoPK.Click = this.ButtonAutoPK_Selected;
            this.UIToggle_LockJoyStickMove.OnSelected = this.ToggleLockJoyStickMove_Selected;
            this.UIButton_AutoFarm.gameObject.GetComponent<UIHoverableObject>().Hover = this.ToggleAutoAttack_Hovered;
            this.UIButton_AutoPK.gameObject.GetComponent<UIHoverableObject>().Hover = this.ToggleAutoAttack_Hovered;

            for (int i = 0; i < this.skills.Length; i++)
            {
                this.skills[i] = -1;
            }

            this.RefreshSkillIcon();
            this.RefreshCooldowns();
        }

        /// <summary>
        /// Sự kiện khi Toggle đổi tay được ấn
        /// </summary>
        /// <param name="isSelected"></param>
        private void ToggleChangeSkill_Selected(bool isSelected)
        {
            /// Nếu chọn kỹ năng ở tay trái
            if (isSelected)
			{
                KTGlobal.AddNotification("<color=red>Kỹ năng <color=yellow>tay trái</color>, các kỹ năng tấn công sẽ <color=yellow>tự chọn mục tiêu</color>.</color>");
			}
            /// Nếu chọn kỹ năng ở tay phải
			else
			{
                KTGlobal.AddNotification("<color=red>Kỹ năng <color=yellow>tay phải</color>, các kỹ năng tấn công <color=yellow>theo hướng phía trước của nhân vật</color>.</color>");
			}

            this.RefreshSkillIcon();
            this.RefreshCooldowns();
        }

        /// <summary>
        /// Sự kiện khi nút Nhảy được ấn
        /// </summary>
        private void ButtonJump_Clicked()
        {
            this.JumpClicked?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi nút Ngồi được ấn
        /// </summary>
        private void ButtonSit_Clicked()
        {
            this.SitClicked?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi nút Chuyển trạng thái cưỡi được ấn
        /// </summary>
        private void ButtonToggleMount_Clicked()
        {
            this.ToggleMountClicked?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button kỹ năng được giữ
        /// </summary>
        private void ButtonSkill_Hold(int position)
        {
            this.SkillButtonHeld?.Invoke(position);
        }

        /// <summary>
        /// Sự kiện khi Button Auto Farm
        /// </summary>
        private void ButtonAutoFarm_Selected()
        {
            this.AutoFarm?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button Auto PK
        /// </summary>
        private void ButtonAutoPK_Selected()
        {
            this.AutoPK?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button kỹ năng vòng sáng được giữ
        /// </summary>
        private void ButtonArua_Held()
        {
            this.AruaButtonHeld?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Toggle khóa di chuyển JoyStick được chọn
        /// </summary>
        private void ToggleLockJoyStickMove_Selected(bool isSelected)
        {
            this.LockJoyStickMove?.Invoke(isSelected);
        }

        /// <summary>
        /// Sự kiện khi Toggle tự đánh được giữ chọn
        /// </summary>
        private void ToggleAutoAttack_Hovered()
        {
            this.OpenAutoSetting?.Invoke();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thêm kỹ năng vào khung
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="skillButton"></param>
        private void ItemToSkillButton(int skillID, UISkillBar_SkillButton skillButton, int index, bool ignoreTarget)
        {
            if (!Loader.Loader.Skills.TryGetValue(skillID, out Entities.Config.SkillDataEx skillData))
            {
                skillButton.SkillID = -1;
                skillButton.ShowIcon = false;
                skillButton.Click = () => {
                    //this.ButtonSkill_Hold(index);
                };
                skillButton.Hold = () => {
                    //this.ButtonSkill_Hold(index);
                };
                skillButton.CooldownTick = 0f;
                skillButton.CurrentCountDownTime = 0f;
                return;
            }

            skillButton.SkillID = skillID;
            skillButton.ShowIcon = true;
            skillButton.IconBundleDir = skillData.IconBundleDir;
            skillButton.IconAtlasName = skillData.IconAtlasName;
            skillButton.IconSpriteName = skillData.Icon;
            skillButton.Refresh();

            /// TODO check cooldown

            skillButton.Click = () => {
                this.SkillButtonClicked?.Invoke(skillID, ignoreTarget);
            };
            skillButton.Hold = () => {
                //this.ButtonSkill_Hold(index);
                this.SkillButtonClicked?.Invoke(skillID, ignoreTarget);
            };
        }

        /// <summary>
        /// Tự động lưu lại thay đổi ở QuickKey
        /// </summary>
        /// <returns></returns>
        private IEnumerator AutoSaveChangeOnQuickKey()
        {
            yield return new WaitForSeconds(10f);
            KTTCPSkillManager.SendSaveQuickKey(this.skills.ToList());

            this.autoSaveWhenSkillChangedCoroutine = null;
        }

        /// <summary>
        /// Tự động lưu thay đổi ở ô vòng sáng
        /// </summary>
        /// <returns></returns>
        private IEnumerator AutoSaveAndActivateArua()
        {
            yield return new WaitForSeconds(1f);
            KTTCPSkillManager.SendSaveAndActivateAruaKey(this.AruaSkillID, this.ActivateArua ? 1 : 0);

            this.autoSaveAruaCoroutine = null;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Cập nhật hiển thị Cooldown cho kỹ năng
        /// </summary>
        /// <param name="skillID"></param>
        public void UpdateSkillCooldown(int skillID)
        {
            if (Global.Data.RoleData.SkillDataList == null)
            {
                return;
            }

            SkillData skillData = Global.Data.RoleData.SkillDataList.Where(x => x.SkillID == skillID).FirstOrDefault();
            if (skillData != null)
            {
                float cooldown = skillData.CooldownTick / 1000f;
                float cooldownLeft = (skillData.CooldownTick - KTGlobal.GetServerTime() + skillData.LastUsedTick) / 1000f;
                if (this.UI_SkillButton_Main.SkillID == skillData.SkillID)
                {
                    this.UI_SkillButton_Main.CooldownTick = cooldown;
                    this.UI_SkillButton_Main.CurrentCountDownTime = cooldownLeft;
                }
                if (this.UI_SkillButton_SubSkill1.SkillID == skillData.SkillID)
                {
                    this.UI_SkillButton_SubSkill1.CooldownTick = cooldown;
                    this.UI_SkillButton_SubSkill1.CurrentCountDownTime = cooldownLeft;
                }
                if (this.UI_SkillButton_SubSkill2.SkillID == skillData.SkillID)
                {
                    this.UI_SkillButton_SubSkill2.CooldownTick = cooldown;
                    this.UI_SkillButton_SubSkill2.CurrentCountDownTime = cooldownLeft;
                }
                if (this.UI_SkillButton_SubSkill3.SkillID == skillData.SkillID)
                {
                    this.UI_SkillButton_SubSkill3.CooldownTick = cooldown;
                    this.UI_SkillButton_SubSkill3.CurrentCountDownTime = cooldownLeft;
                }
                if (this.UI_SkillButton_SubSkill4.SkillID == skillData.SkillID)
                {
                    this.UI_SkillButton_SubSkill4.CooldownTick = cooldown;
                    this.UI_SkillButton_SubSkill4.CurrentCountDownTime = cooldownLeft;
                }
            }
        }

        /// <summary>
        /// Cập nhật hiển thị Cooldown cho toàn bộ kỹ năng
        /// </summary>
        public void RefreshCooldowns()
        {
            if (Global.Data.RoleData.SkillDataList == null)
            {
                return;
            }
            foreach (SkillData skillData in Global.Data.RoleData.SkillDataList)
            {
                if (skillData != null)
                {
                    float cooldown = skillData.CooldownTick / 1000f;
                    float cooldownLeft = (skillData.CooldownTick - KTGlobal.GetServerTime() + skillData.LastUsedTick) / 1000f;
                    if (this.UI_SkillButton_Main.SkillID == skillData.SkillID)
                    {
                        this.UI_SkillButton_Main.CooldownTick = cooldown;
                        this.UI_SkillButton_Main.CurrentCountDownTime = cooldownLeft;
                    }
                    if (this.UI_SkillButton_SubSkill1.SkillID == skillData.SkillID)
                    {
                        this.UI_SkillButton_SubSkill1.CooldownTick = cooldown;
                        this.UI_SkillButton_SubSkill1.CurrentCountDownTime = cooldownLeft;
                    }
                    if (this.UI_SkillButton_SubSkill2.SkillID == skillData.SkillID)
                    {
                        this.UI_SkillButton_SubSkill2.CooldownTick = cooldown;
                        this.UI_SkillButton_SubSkill2.CurrentCountDownTime = cooldownLeft;
                    }
                    if (this.UI_SkillButton_SubSkill3.SkillID == skillData.SkillID)
                    {
                        this.UI_SkillButton_SubSkill3.CooldownTick = cooldown;
                        this.UI_SkillButton_SubSkill3.CurrentCountDownTime = cooldownLeft;
                    }
                    if (this.UI_SkillButton_SubSkill4.SkillID == skillData.SkillID)
                    {
                        this.UI_SkillButton_SubSkill4.CooldownTick = cooldown;
                        this.UI_SkillButton_SubSkill4.CurrentCountDownTime = cooldownLeft;
                    }
                }
            }
        }

        /// <summary>
        /// Làm mới danh sách kỹ năng
        /// </summary>
        public void RefreshSkillIcon()
        {
            string quickKey = Global.Data.RoleData.MainQuickBarKeys;
            string[] keys = quickKey.Split('|');
            if (keys.Length == 10)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    try
                    {
                        this.skills[i] = int.Parse(keys[i]);
                    }
                    catch (Exception)
                    {
                        this.skills[i] = -1;
                    }
                }
            }

            /// Duyệt danh sách kỹ năng, chỉnh lại vũ khí tương ứng
            for (int i = 0; i < this.skills.Length; i++)
            {
                /// Nếu là skill tân thủ
                if (this.skills[i] != -1 && KTGlobal.ListNewbieAttackSkill.Contains(this.skills[i]))
                {
                    /// Nếu tồn tại kỹ năng tân thủ đánh thường quyền
                    if (Loader.Loader.Skills.TryGetValue(KTGlobal.NewbieHandAttackSkill, out SkillDataEx newbieHandAttackSkillData))
                    {
                        /// Loại vũ khí tương ứng
                        SkillDataEx weaponSkillData = KTGlobal.GetNewbieSkillCorrespondingToCurrentWeapon();
                        /// Nếu không tồn tại kỹ năng tương ứng vũ khí thì thiết lập kỹ năng đấm
                        if (weaponSkillData == null)
                        {
                            this.skills[i] = newbieHandAttackSkillData.ID;
                        }
                        else
                        {
                            this.skills[i] = weaponSkillData.ID;
                        }
                    }
                }
            }

            /// Nếu không có kỹ năng nào ở vị trí chính tay trái
            if (this.skills[0] == -1)
            {
                /// Nếu tồn tại kỹ năng tân thủ đánh thường quyền
                if (Loader.Loader.Skills.TryGetValue(KTGlobal.NewbieHandAttackSkill, out SkillDataEx newbieHandAttackSkillData))
                {
                    /// Loại vũ khí tương ứng
                    SkillDataEx weaponSkillData = KTGlobal.GetNewbieSkillCorrespondingToCurrentWeapon();
                    /// Nếu không tồn tại kỹ năng tương ứng vũ khí thì thiết lập kỹ năng đấm
                    if (weaponSkillData == null)
                    {
                        this.skills[0] = newbieHandAttackSkillData.ID;
                    }
                    else
                    {
                        this.skills[0] = weaponSkillData.ID;
                    }
                }
            }

            if (this.ShowMainSkill)
            {
                this.ItemToSkillButton(this.skills[0], this.UI_SkillButton_Main, 0, false);
                this.ItemToSkillButton(this.skills[1], this.UI_SkillButton_SubSkill1, 1, false);
                this.ItemToSkillButton(this.skills[2], this.UI_SkillButton_SubSkill2, 2, false);
                this.ItemToSkillButton(this.skills[3], this.UI_SkillButton_SubSkill3, 3, false);
                this.ItemToSkillButton(this.skills[4], this.UI_SkillButton_SubSkill4, 4, false);
            }
            else
            {
                this.ItemToSkillButton(this.skills[5], this.UI_SkillButton_Main, 5, true);
                this.ItemToSkillButton(this.skills[6], this.UI_SkillButton_SubSkill1, 6, true);
                this.ItemToSkillButton(this.skills[7], this.UI_SkillButton_SubSkill2, 7, true);
                this.ItemToSkillButton(this.skills[8], this.UI_SkillButton_SubSkill3, 8, true);
                this.ItemToSkillButton(this.skills[9], this.UI_SkillButton_SubSkill4, 9, true);
            }
        }

        /// <summary>
        /// Làm mới kỹ năng vòng sáng
        /// </summary>
        public void RefreshAruaIcon()
        {
            string aruaKey = Global.Data.RoleData.OtherQuickBarKeys;
            string[] keys = aruaKey.Split('_');
            if (keys.Length == 2)
            {
                try
                {
                    this.AruaSkillID = int.Parse(keys[0]);
                    this.ActivateArua = int.Parse(keys[1]) == 1;
                }
                catch (Exception) { }
            }

            this.AddAruaSkill(this.AruaSkillID);
        }

        /// <summary>
        /// Thêm kỹ năng vào vị trí tương ứng
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="index"></param>
        public void AddSkill(int skillID, int index)
        {
            if (index < 0 || index > 9)
            {
                return;
            }
            else if (!Loader.Loader.Skills.TryGetValue(skillID, out Entities.Config.SkillDataEx skillData))
            {
                return;
            }

            this.skills[index] = skillID;
            if (index == 0 || index == 5)
            {
                this.ItemToSkillButton(this.skills[index], this.UI_SkillButton_Main, index, index == 5);
            }
            else if (index == 1 || index == 6)
            {
                this.ItemToSkillButton(this.skills[index], this.UI_SkillButton_SubSkill1, index, index == 6);
            }
            else if (index == 2 || index == 7)
            {
                this.ItemToSkillButton(this.skills[index], this.UI_SkillButton_SubSkill2, index, index == 7);
            }
            else if (index == 3 || index == 8)
            {
                this.ItemToSkillButton(this.skills[index], this.UI_SkillButton_SubSkill3, index, index == 8);
            }
            else if (index == 4 || index == 9)
            {
                this.ItemToSkillButton(this.skills[index], this.UI_SkillButton_SubSkill4, index, index == 9);
            }
            this.UpdateSkillCooldown(skillID);

            string quickKey = string.Join("|", this.skills);
            Global.Data.RoleData.MainQuickBarKeys = quickKey;

            if (this.autoSaveWhenSkillChangedCoroutine != null)
            {
                this.StopCoroutine(this.autoSaveWhenSkillChangedCoroutine);
            }
            this.autoSaveWhenSkillChangedCoroutine = this.StartCoroutine(this.AutoSaveChangeOnQuickKey());
        }

        /// <summary>
        /// Thêm kỹ năng vòng sáng vào vị trí tương ứng
        /// </summary>
        /// <param name="skillID"></param>
        public void AddAruaSkill(int skillID)
        {
            if (!Loader.Loader.Skills.TryGetValue(skillID, out Entities.Config.SkillDataEx skillData))
            {
                this.UI_SkillButton_AruaSkill.SkillID = -1;
                this.UI_SkillButton_AruaSkill.ShowIcon = false;
                this.UI_SkillButton_AruaSkill.Click = () => {
                    //this.ButtonArua_Held();
                };
                this.UI_SkillButton_AruaSkill.CooldownTick = 0f;
                this.UI_SkillButton_AruaSkill.CurrentCountDownTime = 0f;
                this.UI_SkillButton_AruaSkill.ActivateAruaEffect = false;
                this.UI_SkillButton_AruaSkill.Hold = () => {
                    //this.ButtonArua_Held();
                };
                return;
            }

            this.UI_SkillButton_AruaSkill.SkillID = skillID;
            this.UI_SkillButton_AruaSkill.ShowIcon = true;
            this.UI_SkillButton_AruaSkill.IconBundleDir = skillData.IconBundleDir;
            this.UI_SkillButton_AruaSkill.IconAtlasName = skillData.IconAtlasName;
            this.UI_SkillButton_AruaSkill.IconSpriteName = skillData.Icon;
            this.UI_SkillButton_AruaSkill.Refresh();
            this.UI_SkillButton_AruaSkill.Click = () => {
                this.UI_SkillButton_AruaSkill.ActivateAruaEffect = !this.UI_SkillButton_AruaSkill.ActivateAruaEffect;
                this.ActivateArua = this.UI_SkillButton_AruaSkill.ActivateAruaEffect;
                string aruaKey = string.Format("{0}_{1}", this.AruaSkillID, this.ActivateArua ? 1 : 0);
                Global.Data.RoleData.OtherQuickBarKeys = aruaKey;
                this.AruaButtonClicked?.Invoke();
            };
            this.UI_SkillButton_AruaSkill.Hold = () => {
                //this.ButtonArua_Held();
            };
            this.UI_SkillButton_AruaSkill.ActivateAruaEffect = this.ActivateArua;

            /// Thiết lập ID vòng sáng tương ứng
            this.AruaSkillID = skillID;

            string aruaKey = string.Format("{0}_{1}", this.AruaSkillID, this.ActivateArua ? 1 : 0);
            Global.Data.RoleData.OtherQuickBarKeys = aruaKey;
            
            if (this.autoSaveAruaCoroutine != null)
            {
                this.StopCoroutine(this.autoSaveAruaCoroutine);
            }
            this.autoSaveAruaCoroutine = this.StartCoroutine(this.AutoSaveAndActivateArua());
        }
        #endregion
    }
}