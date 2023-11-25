using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Network;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSPlay.KiemVu.Logic.Settings
{
    /// <summary>
    /// Khung thiết lập tự đánh
    /// </summary>
    public static partial class KTAutoAttackSetting
    {
        #region SETINGSCONFIG

        /// <summary>
        /// Đây có phải chế độ đánh quái không
        /// </summary>
        public static bool IsTrainMode = false;

        /// <summary>
        /// Thiết lập Auto
        /// </summary>
        public static AutoSettingConfig _AutoConfig { get; set; } = new AutoSettingConfig();

        #endregion SETINGSCONFIG

        /// <summary>
        /// Thiết lập mặc định
        /// </summary>
        public static void SetDefaultConfig()
        {
            _AutoConfig = new AutoSettingConfig();
            _AutoConfig._AutoPKConfig = new SupportAndPkConfig();
            _AutoConfig._AutoTrainConfig = new AutoTrainConfig();
            _AutoConfig._PickItemConfig = new PickItemConfig();
            _AutoConfig._QickItemConfig = new QickItemConfig();

            // set 2 item id rỗng vào 2 nút dùng nhanh
            _AutoConfig._QickItemConfig.QuickKeySlot1 = -1;
            _AutoConfig._QickItemConfig.QuickKeySlot2 = -1;

            _AutoConfig._AutoTrainConfig.AttackMode = 1;
            _AutoConfig._AutoTrainConfig.IsAutoFireCamp = false;
            _AutoConfig._AutoTrainConfig.IsLowHpSelect = false;
            _AutoConfig._AutoTrainConfig.IsRadius = true;
            _AutoConfig._AutoTrainConfig.IsSkipBoss = false;
            _AutoConfig._AutoTrainConfig.Raidus = 200;
            _AutoConfig._AutoTrainConfig.SkillSelect = new List<int>();
            _AutoConfig._AutoTrainConfig.UseNewbieSkill = true;

            // add vào 5 skill rỗng
            _AutoConfig._AutoTrainConfig.SkillSelect.Add(-1);
            _AutoConfig._AutoTrainConfig.SkillSelect.Add(-1);
            _AutoConfig._AutoTrainConfig.SkillSelect.Add(-1);
            _AutoConfig._AutoTrainConfig.SkillSelect.Add(-1);
            _AutoConfig._AutoTrainConfig.SkillSelect.Add(-1);

            _AutoConfig._PickItemConfig.CrytalLevel = 1;
            _AutoConfig._PickItemConfig.IsAutoPickUp = true;
            _AutoConfig._PickItemConfig.IsAutoSellItem = true;
            _AutoConfig._PickItemConfig.IsAutoSort = true;
            _AutoConfig._PickItemConfig.IsOnlyPickCrytal = false;
            _AutoConfig._PickItemConfig.IsOnlyPickEquip = false;
            _AutoConfig._PickItemConfig.RadiusPick = 100;
            _AutoConfig._PickItemConfig.StarPick = 1;
            _AutoConfig._PickItemConfig.StarWillSell = 4;

            _AutoConfig._AutoPKConfig.AutoNMPercent = 20;
            _AutoConfig._AutoPKConfig.HpPercent = 50;
            _AutoConfig._AutoPKConfig.IsAutoEat = true;
            _AutoConfig._AutoPKConfig.IsAutoHp = true;
            _AutoConfig._AutoPKConfig.IsAutoPKAgain = true;
            _AutoConfig._AutoPKConfig.IsAutoReviceTeam = true;
            _AutoConfig._AutoPKConfig.IsBuffNM = true;
            _AutoConfig._AutoPKConfig.IsElementalSelect = false;
            _AutoConfig._AutoPKConfig.IsLowHpSelect = false;
            _AutoConfig._AutoPKConfig.MpPercent = 50;
            _AutoConfig._AutoPKConfig.SkillPKSelect = new List<int>();
            _AutoConfig._AutoPKConfig.AutoAccect = false;
            _AutoConfig._AutoPKConfig.AutoInviter = false;
            _AutoConfig._AutoPKConfig.DisplayEnemyUI = false;

            // add vào 5 skill rỗng
            _AutoConfig._AutoPKConfig.SkillPKSelect.Add(-1);
            _AutoConfig._AutoPKConfig.SkillPKSelect.Add(-1);
            _AutoConfig._AutoPKConfig.SkillPKSelect.Add(-1);
            _AutoConfig._AutoPKConfig.SkillPKSelect.Add(-1);
            _AutoConfig._AutoPKConfig.SkillPKSelect.Add(-1);

            // ID máu và mana nào sử dụng để auto
            _AutoConfig._AutoPKConfig.MPMedicine = -1;
            _AutoConfig._AutoPKConfig.HPMedicine = -1;
            _AutoConfig._AutoPKConfig.UsingBaseSkill = false;
            _AutoConfig._AutoPKConfig.ChaseTarget = true;

            byte[] DataByteArray = DataHelper.ObjectToBytes(_AutoConfig);

            string InfoEncoding = Convert.ToBase64String(DataByteArray);

            Global.Data.RoleData.AutoSettings = InfoEncoding;

            KT_TCPHandler.SendSaveAutoSettings();
        }

        /// <summary>
        /// Tạo thêm hàm này cho đỡ cost | Vì nếu mỗi lần nhặt phải fill data từ DICT thì sẽ rất ỉa chảy
        /// </summary>
        /// <param name="GoodPackID"></param>
        /// <returns></returns>
        public static bool IsCanPickCrytalItem(int GoodPackID, int MinLevelPick)
        {
            int ItemLevel = 0;

            if (GoodPackID >= 183 && GoodPackID <= 194)
            {
                ItemLevel = GoodPackID - 182;

                if (ItemLevel >= MinLevelPick)
                {
                    return true;
                }
            }
            else if (GoodPackID >= 385 && GoodPackID <= 396)
            {
                ItemLevel = GoodPackID - 384;

                if (ItemLevel >= MinLevelPick)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }


        public static bool IsEquip(int GooldID)
        {
            return KTGlobal.IsEquip(GooldID);
        }

        public static bool IsCrytalItem(int GoodPackID)
        {
            if (GoodPackID >= 183 && GoodPackID <= 194)
            {
                return true;
            }
            else if (GoodPackID >= 385 && GoodPackID <= 396)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Thiết lập Auto
        /// </summary>
        public static void SetAutoConfig()
        {
            try
            {
                byte[] Base64Decode = Convert.FromBase64String(Global.Data.RoleData.AutoSettings);
                _AutoConfig = DataHelper.BytesToObject<AutoSettingConfig>(Base64Decode, 0, Base64Decode.Length);
                // _AutoConfig._AutoPKConfig.SkillPKSelect[0] = 837;
            }
            catch (Exception ex)
            {
                KTAutoAttackSetting.SetDefaultConfig();
            }

            KTAutoAttackSetting.SkillAutoTrain = new List<SkillDataEx>();

            if (_AutoConfig == null)
            {
                KTAutoAttackSetting.SetDefaultConfig();
            }

            // Set skill để train quái
            foreach (int skillID in _AutoConfig._AutoTrainConfig.SkillSelect)
            {
                if (Loader.Loader.Skills.TryGetValue(skillID, out SkillDataEx skillData))
                {
                    SkillData dbSkill = Global.Data.RoleData.SkillDataList.Where(x => x.SkillID == skillID).FirstOrDefault();
                    /// Nếu tồn tại trên người và không phải kỹ năng bị động và vòng sáng
                    if (dbSkill != null && dbSkill.Level > 0 && skillData.Type != 3 && !skillData.IsArua)
                    {
                        KTAutoAttackSetting.SkillAutoTrain.Add(skillData);
                    }
                    else
                    {
                        KTAutoAttackSetting.SkillAutoTrain.Add(null);
                    }
                }
                else
                {
                    KTAutoAttackSetting.SkillAutoTrain.Add(null);
                }
            }

            KTAutoAttackSetting.SkillAutoPK = new List<SkillDataEx>();

            // set skill để pk
            foreach (int skillID in _AutoConfig._AutoPKConfig.SkillPKSelect)
            {
                if (Loader.Loader.Skills.TryGetValue(skillID, out SkillDataEx skillData))
                {
                    SkillData dbSkill = Global.Data.RoleData.SkillDataList.Where(x => x.SkillID == skillID).FirstOrDefault();
                    /// Nếu tồn tại trên người và không phải kỹ năng bị động và vòng sáng
                    if (dbSkill != null && dbSkill.Level > 0 && skillData.Type != 3 && !skillData.IsArua)
                    {
                        KTAutoAttackSetting.SkillAutoPK.Add(skillData);
                    }
                    else
                    {
                        KTAutoAttackSetting.SkillAutoPK.Add(null);
                    }
                }
                else
                {
                    KTAutoAttackSetting.SkillAutoPK.Add(null);
                }
            }
        }

        /// <summary>
        /// Thiết lập danh sách kỹ năng dùng khi Train
        /// </summary>
        /// <param name="skills"></param>
        public static void SetSkillTrain(List<int> skills)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (Loader.Loader.Skills.TryGetValue(skills[i], out SkillDataEx skillData))
                {
                    KTAutoAttackSetting.SkillAutoTrain[i] = skillData;
                }
                else
                {
                    KTAutoAttackSetting.SkillAutoTrain[i] = null;
                }
            }

            /// Lưu lại vào Config
            KTAutoAttackSetting._AutoConfig._AutoTrainConfig.SkillSelect = skills;
        }

        /// <summary>
        /// Thiết lập danh sách kỹ năng dùng khi PK
        /// </summary>
        /// <param name="skills"></param>
        public static void SetSkillPK(List<int> skills)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (Loader.Loader.Skills.TryGetValue(skills[i], out SkillDataEx skillData))
                {
                    KTAutoAttackSetting.SkillAutoPK[i] = skillData;
                }
                else
                {
                    KTAutoAttackSetting.SkillAutoPK[i] = null;
                }
            }

            /// Lưu lại vào Config
            KTAutoAttackSetting._AutoConfig._AutoPKConfig.SkillPKSelect = skills;
        }

        /// <summary>
        /// Danh sách kỹ năng sử dụng để train quái
        /// </summary>
        public static List<SkillDataEx> SkillAutoTrain { get; set; }

        /// <summary>
        /// Danh sách kỹ năng đã sử dụng để AUTO PK
        /// </summary>
        public static List<SkillDataEx> SkillAutoPK { get; set; }

        #region Public methods

        /// <summary>
        /// Chuyển thành dạng chuỗi mã hóa lưu vào DB
        /// </summary>
        /// <returns></returns>
        public static void SaveSettings()
        {
            byte[] DataByteArray = DataHelper.ObjectToBytes(_AutoConfig);

            string InfoEncoding = Convert.ToBase64String(DataByteArray);

            Global.Data.RoleData.AutoSettings = InfoEncoding;

            KT_TCPHandler.SendSaveAutoSettings();
        }

        #endregion Public methods
    }
}