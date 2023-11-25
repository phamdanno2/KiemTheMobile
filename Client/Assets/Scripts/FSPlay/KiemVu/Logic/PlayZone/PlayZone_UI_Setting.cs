using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using System.Linq;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Thiết lập Auto
    /// <summary>
    /// Khung thiết lập Auto
    /// </summary>
    public UIAutoFight UIAutoFight { get; protected set; } = null;

    /// <summary>
    /// Mở khung thiết lập tự đánh
    /// </summary>
    public void OpenUIAutoFight()
	{
        /// Nếu khung đang hiển thị
        if (this.UIAutoFight != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIAutoFight = canvas.LoadUIPrefab<UIAutoFight>("MainGame/UIAutoFight");
        canvas.AddUI(this.UIAutoFight);

		#region Dữ liệu
		/// Auto farm
		this.UIAutoFight.AutoFarm.AutoFireCamp = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsAutoFireCamp;
        this.UIAutoFight.AutoFarm.FarmAround = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsRadius;
        this.UIAutoFight.AutoFarm.ScanRange = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.Raidus;
        this.UIAutoFight.AutoFarm.SingleTarget = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.AttackMode == 1;
        this.UIAutoFight.AutoFarm.IgnoreBoss = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsSkipBoss;
        this.UIAutoFight.AutoFarm.LowHPTargetPriority = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsLowHpSelect;
        this.UIAutoFight.AutoFarm.UseNewbieSkill = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.UseNewbieSkill;
        this.UIAutoFight.AutoFarm.AutoUseWine = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.AutoDrinkWine;
        this.UIAutoFight.AutoFarm.UseSkillByCombo = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.UseSkillByCombo;
        this.UIAutoFight.AutoFarm.Skills = KTAutoAttackSetting._AutoConfig._AutoTrainConfig.SkillSelect;

        /// Auto nhặt
        this.UIAutoFight.AutoPickUpItem.AutoPickUpItem = KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoPickUp;
        this.UIAutoFight.AutoPickUpItem.PickUpRange = KTAutoAttackSetting._AutoConfig._PickItemConfig.RadiusPick;
        this.UIAutoFight.AutoPickUpItem.PickUpCrystalStone = KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickCrytal;
        this.UIAutoFight.AutoPickUpItem.PickUpCrystalStoneLevel = KTAutoAttackSetting._AutoConfig._PickItemConfig.CrytalLevel;
        this.UIAutoFight.AutoPickUpItem.PickUpEquip = KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickEquip;
        this.UIAutoFight.AutoPickUpItem.PickUpEquipStar = KTAutoAttackSetting._AutoConfig._PickItemConfig.StarPick;
        this.UIAutoFight.AutoPickUpItem.AutoSortBag = KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSort;
        this.UIAutoFight.AutoPickUpItem.AutoBackAndSellTrashes = KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSellItem;
        this.UIAutoFight.AutoPickUpItem.AutoSellTrashEquipBelowStar = KTAutoAttackSetting._AutoConfig._PickItemConfig.StarWillSell;
        this.UIAutoFight.AutoPickUpItem.PickUpOtherItems = KTAutoAttackSetting._AutoConfig._PickItemConfig.PickUpOtherItems;

        /// Auto PK
        this.UIAutoFight.AutoPK.AutoUseMedicine = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoHp;
        this.UIAutoFight.AutoPK.AutoEatFood = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoEat;
        this.UIAutoFight.AutoPK.AutoInviteToTeam = KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoInviter;
        this.UIAutoFight.AutoPK.AutoAcceptInviteToTeam = KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoAccect;
        this.UIAutoFight.AutoPK.AutoUseMedicineHP = KTAutoAttackSetting._AutoConfig._AutoPKConfig.HpPercent;
        this.UIAutoFight.AutoPK.AutoUseMedicineMP = KTAutoAttackSetting._AutoConfig._AutoPKConfig.MpPercent;
        this.UIAutoFight.AutoPK.HPMedicineID = KTAutoAttackSetting._AutoConfig._AutoPKConfig.HPMedicine;
        this.UIAutoFight.AutoPK.MPMedicineID = KTAutoAttackSetting._AutoConfig._AutoPKConfig.MPMedicine;
        this.UIAutoFight.AutoPK.EMAutoBuff = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsBuffNM;
        this.UIAutoFight.AutoPK.EMAutoHP = KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoNMPercent;
        this.UIAutoFight.AutoPK.EMAutoRevive = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoReviceTeam;
        this.UIAutoFight.AutoPK.AutoReflectAttack = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoPKAgain;
        this.UIAutoFight.AutoPK.SeriesConquePriority = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsElementalSelect;
        this.UIAutoFight.AutoPK.LowHPEnemyPriority = KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsLowHpSelect;
        this.UIAutoFight.AutoPK.ShowNearbyEnemy = KTAutoAttackSetting._AutoConfig._AutoPKConfig.DisplayEnemyUI;
        this.UIAutoFight.AutoPK.UseNewbieSkill = KTAutoAttackSetting._AutoConfig._AutoPKConfig.UsingBaseSkill;
        this.UIAutoFight.AutoPK.ChaseTarget = KTAutoAttackSetting._AutoConfig._AutoPKConfig.ChaseTarget;
        this.UIAutoFight.AutoPK.UseSkillByCombo = KTAutoAttackSetting._AutoConfig._AutoPKConfig.UseSkillByCombo;
        this.UIAutoFight.AutoPK.Skills = KTAutoAttackSetting._AutoConfig._AutoPKConfig.SkillPKSelect;
		#endregion

		this.UIAutoFight.Close = this.CloseUIAutoFight;
        this.UIAutoFight.SaveSetting = () => {
            /// Auto farm
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsAutoFireCamp = this.UIAutoFight.AutoFarm.AutoFireCamp;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsRadius = this.UIAutoFight.AutoFarm.FarmAround;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.Raidus = this.UIAutoFight.AutoFarm.ScanRange;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.AttackMode = this.UIAutoFight.AutoFarm.SingleTarget ? 1 : 2;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsSkipBoss = this.UIAutoFight.AutoFarm.IgnoreBoss;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsLowHpSelect = this.UIAutoFight.AutoFarm.LowHPTargetPriority;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.UseNewbieSkill = this.UIAutoFight.AutoFarm.UseNewbieSkill;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.AutoDrinkWine = this.UIAutoFight.AutoFarm.AutoUseWine;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.UseSkillByCombo = this.UIAutoFight.AutoFarm.UseSkillByCombo;
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.SkillSelect = this.UIAutoFight.AutoFarm.Skills;
            KTAutoAttackSetting.SetSkillTrain(this.UIAutoFight.AutoFarm.Skills);

            /// Auto nhặt
            KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoPickUp = this.UIAutoFight.AutoPickUpItem.AutoPickUpItem;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.RadiusPick = this.UIAutoFight.AutoPickUpItem.PickUpRange;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickCrytal = this.UIAutoFight.AutoPickUpItem.PickUpCrystalStone;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.CrytalLevel = this.UIAutoFight.AutoPickUpItem.PickUpCrystalStoneLevel;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickEquip = this.UIAutoFight.AutoPickUpItem.PickUpEquip;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.StarPick = this.UIAutoFight.AutoPickUpItem.PickUpEquipStar;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSort = this.UIAutoFight.AutoPickUpItem.AutoSortBag;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSellItem = this.UIAutoFight.AutoPickUpItem.AutoBackAndSellTrashes;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.StarWillSell = this.UIAutoFight.AutoPickUpItem.AutoSellTrashEquipBelowStar;
            KTAutoAttackSetting._AutoConfig._PickItemConfig.PickUpOtherItems = this.UIAutoFight.AutoPickUpItem.PickUpOtherItems;

            /// Auto PK
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoHp = this.UIAutoFight.AutoPK.AutoUseMedicine;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoEat = this.UIAutoFight.AutoPK.AutoEatFood;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoInviter = this.UIAutoFight.AutoPK.AutoInviteToTeam;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoAccect = this.UIAutoFight.AutoPK.AutoAcceptInviteToTeam;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.HpPercent = this.UIAutoFight.AutoPK.AutoUseMedicineHP;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.MpPercent = this.UIAutoFight.AutoPK.AutoUseMedicineMP;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.HPMedicine = this.UIAutoFight.AutoPK.HPMedicineID;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.MPMedicine = this.UIAutoFight.AutoPK.MPMedicineID;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsBuffNM = this.UIAutoFight.AutoPK.EMAutoBuff;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoNMPercent = this.UIAutoFight.AutoPK.EMAutoHP;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoReviceTeam = this.UIAutoFight.AutoPK.EMAutoRevive;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoPKAgain = this.UIAutoFight.AutoPK.AutoReflectAttack;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsElementalSelect = this.UIAutoFight.AutoPK.SeriesConquePriority;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsLowHpSelect = this.UIAutoFight.AutoPK.LowHPEnemyPriority;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.DisplayEnemyUI = this.UIAutoFight.AutoPK.ShowNearbyEnemy;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.UsingBaseSkill = this.UIAutoFight.AutoPK.UseNewbieSkill;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.ChaseTarget = this.UIAutoFight.AutoPK.ChaseTarget;
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.UseSkillByCombo = this.UIAutoFight.AutoPK.UseSkillByCombo;
            KTAutoAttackSetting.SetSkillPK(this.UIAutoFight.AutoPK.Skills);

            /// Lưu thiết lập vào hệ thống
            KTAutoAttackSetting.SaveSettings();

            KTGlobal.AddNotification("Lưu thiết lập tự đánh thành công!");

            /// Thực thi thiết lập hệ thống
            this.ExecuteAutoSettings();

            /// Đóng khung
            this.CloseUIAutoFight();

            /// Nếu đang tự đánh
            if (KTAutoFightManager.Instance.IsAutoFighting)
            {
                /// Tắt tự đánh
                KTAutoFightManager.Instance.StopAutoFight();

                /// Nếu đang mở Auto Farm
                if (this.UIBottomBar.UISkillBar.AutoFarmEnable)
				{
                    /// Mở tự đánh
                    KTAutoFightManager.Instance.StartAutoFarm();
                }
                else if (this.UIBottomBar.UISkillBar.AutoPKEnable)
				{
                    /// Mở tự đánh
                    KTAutoFightManager.Instance.StartAutoPK();
                }
            }
        };
    }

    /// <summary>
    /// Đóng khung thiết lập tự đánh
    /// </summary>
    public void CloseUIAutoFight()
	{
        if (this.UIAutoFight != null)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            canvas.RemoveUI(this.UIAutoFight);
            this.UIAutoFight = null;
        }
    }

    /// <summary>
    /// Thực thi thiết lập tự đánh
    /// </summary>
    public void ExecuteAutoSettings()
    {
		KTAutoAttackSetting.SaveSettings();
	}
    #endregion

    #region Thiết lập hệ thống
    /// <summary>
    /// Khung thiết lập hệ thống
    /// </summary>
    public UISystemSetting UISystemSetting { get; protected set; } = null;

    /// <summary>
    /// Hiển thị thiết lập hệ thống
    /// </summary>
    public void ShowSystemSetting()
    {
        /// Nếu khung đang hiển thị
        if (this.UISystemSetting != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UISystemSetting = canvas.LoadUIPrefab<UISystemSetting>("MainGame/UISystemSetting");
        if (this.UISystemSetting == null)
        {
            return;
        }
        /// Thêm vào Canvas
        canvas.AddUI(this.UISystemSetting, true);

        /// Control thuộc tính của khung
        this.UISystemSetting.Close = () => {
            this.CloseSystemSetting();
        };
        this.UISystemSetting.SaveSetting = () => {
            KTSystemSetting.MusicVolume = (int) (this.UISystemSetting.MusicVolume * 100);
            KTSystemSetting.SkillVolume = (int) (this.UISystemSetting.EffectSoundVolume * 100);
            KTSystemSetting.FieldOfView = (int) this.UISystemSetting.FieldOfView;
            KTSystemSetting.HideOtherName = this.UISystemSetting.HideOtherName;
            KTSystemSetting.HideOtherHPBar = this.UISystemSetting.HideOtherHPBar;
            KTSystemSetting.HideRole = this.UISystemSetting.HideLeader;
            KTSystemSetting.HideOtherRole = this.UISystemSetting.HideOtherRole;
            KTSystemSetting.HideNPC = this.UISystemSetting.HideMonsterAndNPC;
            KTSystemSetting.HideSkillCastEffect = this.UISystemSetting.HideSkillCastEffect;
            KTSystemSetting.HideSkillExplodeEffect = this.UISystemSetting.HideSkillExplodeEffect;
            KTSystemSetting.HideSkillBuffEffect = this.UISystemSetting.HideSkillBuffEffect;
            KTSystemSetting.HideSkillBullet = this.UISystemSetting.HideBullet;
            KTSystemSetting.DisableTrailEffect = this.UISystemSetting.DisableTrailEffect;
            KTSystemSetting.HideWeaponEnhanceEffect = this.UISystemSetting.HideWeaponEnhanceEffect;
            KTSystemSetting.EffectQualitySetting = this.UISystemSetting.EffectQualitySetting;

            /// Lưu thiết lập vào hệ thống
            KTSystemSetting.SaveSettings();

            /// Thông báo
            KTGlobal.AddNotification("Lưu thiết lập hệ thống thành công!");

            /// Thực thi thiết lập hệ thống
            this.ExecuteSystemSettings();

            /// Đóng khung
            this.CloseSystemSetting();
        };
        this.UISystemSetting.QuitGame = () => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        };
        this.UISystemSetting.MusicVolume = KTSystemSetting.MusicVolume / 100f;
        this.UISystemSetting.EffectSoundVolume = KTSystemSetting.SkillVolume / 100f;
        this.UISystemSetting.FieldOfView = KTSystemSetting.FieldOfView;
        this.UISystemSetting.HideOtherName = KTSystemSetting.HideOtherName;
        this.UISystemSetting.HideOtherHPBar = KTSystemSetting.HideOtherHPBar;
        this.UISystemSetting.HideLeader = KTSystemSetting.HideRole;
        this.UISystemSetting.HideOtherRole = KTSystemSetting.HideOtherRole;
        this.UISystemSetting.HideMonsterAndNPC = KTSystemSetting.HideNPC;
        this.UISystemSetting.HideSkillCastEffect = KTSystemSetting.HideSkillCastEffect;
        this.UISystemSetting.HideSkillExplodeEffect = KTSystemSetting.HideSkillExplodeEffect;
        this.UISystemSetting.HideSkillBuffEffect = KTSystemSetting.HideSkillBuffEffect;
        this.UISystemSetting.HideBullet = KTSystemSetting.HideSkillBullet;
        this.UISystemSetting.DisableTrailEffect = KTSystemSetting.DisableTrailEffect;
        this.UISystemSetting.HideWeaponEnhanceEffect = KTSystemSetting.HideWeaponEnhanceEffect;
        this.UISystemSetting.EffectQualitySetting = KTSystemSetting.EffectQualitySetting;
    }

    /// <summary>
    /// Đóng thiết lập hệ thống
    /// </summary>
    public void CloseSystemSetting()
    {
        if (this.UISystemSetting != null)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            canvas.RemoveUI(this.UISystemSetting);
            this.UISystemSetting = null;
        }
    }

    /// <summary>
    /// Thực thi thiết lập hệ thống
    /// </summary>
    public void ExecuteSystemSettings()
    {
        KTSystemSetting.Apply();
    }
    #endregion
}
