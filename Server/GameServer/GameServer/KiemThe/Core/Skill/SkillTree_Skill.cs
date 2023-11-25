using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Entities
{
    /// <summary>
    /// Quản lý Logic kỹ năng
    /// </summary>
    public partial class SkillTree
    {
        /// <summary>
        /// Danh sách kỹ năng mà nhân vật học được
        /// <para>Key: ID kỹ năng</para>
        /// <para>Value: Cấp độ kỹ năng</para>
        /// </summary>
        private readonly ConcurrentDictionary<int, SkillLevelRef> listStudiedSkills = new ConcurrentDictionary<int, SkillLevelRef>();

        /// <summary>
        /// Điểm kỹ năng đã cộng vào
        /// </summary>
        public int AddedSkillPoints
        {
            get
            {
                return this.listStudiedSkills.Where(x => x.Value.CanAddPoint).Sum(x => x.Value.AddedLevel);
            }
        }

        /// <summary>
        /// Danh sách kỹ năng của người chơi
        /// </summary>
        public List<SkillLevelRef> ListSkills
        {
            get
            {
                return this.listStudiedSkills.Values.ToList<SkillLevelRef>();
            }
        }

        private int _AllSkillBonusLevel = 0;
        /// <summary>
        /// Điểm cộng thêm cho tất cả kỹ năng
        /// </summary>
        public int AllSkillBonusLevel
		{
			get
			{
                return this._AllSkillBonusLevel;
			}
			set
			{
                this._AllSkillBonusLevel = value;

                /// Mức máu hiện tại
                int currentHP = this.Player.m_CurrentLife;
                /// Mức khí hiện tại
                int currentMP = this.Player.m_CurrentMana;
                /// Mức thể lực hiện tại
                int currentStamina = this.Player.m_CurrentStamina;

                /// ID kỹ năng vòng sáng trước
                int lastAuraSkill = -1;
                /// Nếu có vòng sáng đang kích hoạt
                if (this.Player.Buffs.CurrentArua != null)
                {
                    lastAuraSkill = this.Player.Buffs.CurrentArua.Skill.SkillID;
                }

                /// Hủy kỹ năng bị động và vòng sáng
                this.Player.DeactivateAuraPassiveAndEnchantSkills();

                /// Duyệt danh sách kỹ năng và thêm điểm cộng vào
                foreach (SkillLevelRef skillRef in this.ListSkills)
                {
                    skillRef.BonusLevel = value;
                }

                /// Kích hoạt lại kỹ năng bị động và vòng sáng
                this.Player.ReactivateAuraPassiveAndEnchantSkills();

                /// Nếu có kỹ năng vòng sáng hiện tại
                if (lastAuraSkill != -1 && this.HasSkill(lastAuraSkill))
                {
                    /// Thực hiện kỹ năng vòng sáng
                    KTSkillManager.UseSkill(this.Player, this.Player, this.GetSkillLevelRef(lastAuraSkill), true);
                }

                /// Cập nhật mức máu
                this.Player.m_CurrentLife = Math.Min(this.Player.m_CurrentLifeMax, currentHP);
                /// Cập nhật mức khí
                this.Player.m_CurrentMana = Math.Min(this.Player.m_CurrentManaMax, currentMP);
                /// Cập nhật mức thể lực
                this.Player.m_CurrentStamina = Math.Min(this.Player.m_CurrentStaminaMax, currentStamina);

                /// Cập nhật lại danh sách kỹ năng gửi về Client
                this.ExportSkillTree();
            }
        }

        /// <summary>
        /// Thêm kinh nghiệm cho kỹ năng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exp"></param>
        public void AddSkillExp(int id, int exp)
        {
            /// Nếu kỹ năng tồn tại
            if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skillRef))
            {
                /// Kỹ năng tương ứng
                SkillDataEx skillData = KSkill.GetSkillData(id);
                if (skillData != null)
                {
                    /// Nếu không phải kỹ năng cần mật tịch để thăng cấp
                    if (!skillData.IsExpSkill)
                    {
                        return;
                    }

                    /// ProDict tương ứng
                    PropertyDictionary skillPd = skillData.Properties[skillRef.AddedLevel];
                    /// Nếu không tồn tại Symbol exp
                    if (!skillPd.ContainsKey((int) MAGIC_ATTRIB.magic_skill_skillexp_v))
                    {
                        return;
                    }

                    /// Cấp hiện tại
                    int currentLevel = skillRef.AddedLevel;
                    /// Kinh nghiệm thêm vào
                    int nAddExp = skillRef.Exp + exp;
                    do
                    {
                        /// Nếu cấp độ vượt quá ngưỡng
                        if (skillRef.AddedLevel >= skillData.MaxSkillLevel)
                        {
                            break;
                        }

                        /// ProDict tương ứng
                        skillPd = skillData.Properties[skillRef.AddedLevel];
                        /// Kinh nghiệm cấp hiện tại cần
                        int thisLevelMaxExp = skillPd.Get<KMagicAttrib>((int) MAGIC_ATTRIB.magic_skill_skillexp_v).nValue[0];

                        /// Nếu kinh nghiệm vượt quá ngưỡng thăng cấp
                        if (nAddExp >= thisLevelMaxExp)
                        {
                            nAddExp -= thisLevelMaxExp;
                            /// Thăng cấp kỹ năng
                            skillRef.AddedLevel++;
                        }
                        else
                        {
                            /// Cập nhật lượng kinh nghiệm thêm vào
                            skillRef.Exp = nAddExp;
                            break;
                        }
                    }
                    while (true);
                    /// Cập nhật lượng kinh nghiệm thêm vào
                    skillRef.Exp = nAddExp;

                    /// Nếu cấp độ thay đổi
                    if (currentLevel != skillRef.Level)
                    {
                        /// Nếu là kỹ năng bị động hoặc vòng sáng đang được kích hoạt
                        if (skillRef.Data.Type == 3 || (skillRef.Data.IsArua && this.Player.Buffs.CurrentArua != null && this.Player.Buffs.CurrentArua.Skill.SkillID == skillRef.SkillID))
                        {
                            /// Thực hiện lại kỹ năng bị động
                            KTSkillManager.ProcessPassiveSkill(this.Player, skillRef);
                        }

                        /// Thực hiện cập nhật danh sách hỗ trợ sát thương cho kỹ năng khác
                        this.DoProcessSkillAppendDamages(skillRef.Data, skillRef.Level, skillRef.Level);

                        /// Đổ dữ liệu về RoleDataEx
                        this.ExportSkillTree();

                        /// Thực hiện lại các kỹ năng được hỗ trợ
                        this.ProcessEnchantSkills();
                    }

                    ///// Cập nhật thông tin kỹ năng vào DB
                    //KT_TCPHandler.UpdateSkillInfoFromDB(this.Player, id, skillRef.AddedLevel, this.GetSkillLastUsedTick(id), this.GetSkillCooldown(id), skillRef.Exp);
                }
            }
        }

        /// <summary>
        /// Thêm kỹ năng
        /// <para>Kỹ năng được thêm vào sẽ mang cấp 0</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        public void AddSkill(int id)
        {
            if (!this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skillRef))
            {
                SkillDataEx skillData = KSkill.GetSkillData(id);
                if (skillData != null && !this.listStudiedSkills.ContainsKey(id))
                {
                    SkillLevelRef levelRef = new SkillLevelRef()
                    {
                        AddedLevel = 0,
                        Data = skillData,
                        BonusLevel = this._AllSkillBonusLevel,
                        CanStudy = true,
                        Exp = 0,
                    };
                    this.listStudiedSkills[skillData.ID] = levelRef;

                    if (!levelRef.CanStudy)
                    {
                        /// Cái này đảm bảo cái hàm Verify đằng sau chắc chắn nó sẽ trả ra lỗi
                        levelRef.AddedLevel = 999999;
                    }
                    else
                    {
                        KT_TCPHandler.AddSkillToDB(this.Player, id);
                    }
                }
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Thêm danh sách kỹ năng
        /// <para>Kỹ năng được thêm vào sẽ mang cấp 0</para>
        /// </summary>
        /// <param name="skillIds"></param>
        /// <param name="notifyToClient"></param>
        public void AddSkills(ICollection<int> skillIds)
        {
            foreach (int id in skillIds)
            {
                if (!this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skillRef))
                {
                    SkillDataEx skillData = KSkill.GetSkillData(id);
                    if (skillData != null && !this.listStudiedSkills.ContainsKey(id))
                    {
                        SkillLevelRef levelRef = new SkillLevelRef()
                        {
                            AddedLevel = 0,
                            Data = skillData,
                            BonusLevel = this._AllSkillBonusLevel,
                            CanStudy = true,
                            Exp = 0,
                        };
                        this.listStudiedSkills[skillData.ID] = levelRef;

                        if (!levelRef.CanStudy)
                        {
                            /// Cái này đảm bảo cái hàm Verify đằng sau chắc chắn nó sẽ trả ra lỗi
                            levelRef.AddedLevel = 9999999;

                            break;
                        }

                        KT_TCPHandler.AddSkillToDB(this.Player, id);
                    }
                }
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Cập nhật dữ liệu cấp độ kỹ năng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        public void AddSkillLevel(int id, int level)
        {
            if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skillRef))
            {
                /// Nếu là kỹ năng bị động hoặc vòng sáng đang được kích hoạt
                if (skillRef.Data.Type == 3 || (skillRef.Data.IsArua && this.Player.Buffs.CurrentArua != null && this.Player.Buffs.CurrentArua.Skill.SkillID == skillRef.SkillID))
                {
                    /// Lấy giá trị Buff cũ
                    BuffDataEx buff = this.Player.Buffs.GetBuff(skillRef.SkillID);
                    if (buff != null)
                    {
                        /// Xóa Buff cũ đi
                        this.Player.Buffs.RemoveBuff(buff, true, false);
                    }
                }
                int oldLevel = skillRef.Level;
                skillRef.AddedLevel += level;
                skillRef.AddedLevel = System.Math.Min(skillRef.AddedLevel, skillRef.Data.MaxSkillLevel);

                /// Nếu là kỹ năng bị động hoặc vòng sáng đang được kích hoạt
                if (skillRef.Data.Type == 3 || (skillRef.Data.IsArua && this.Player.Buffs.CurrentArua != null && this.Player.Buffs.CurrentArua.Skill.SkillID == skillRef.SkillID))
                {
                    /// Thực hiện lại kỹ năng bị động
                    KTSkillManager.ProcessPassiveSkill(this.Player, skillRef);
                }

                /// Thực hiện cập nhật danh sách hỗ trợ sát thương cho kỹ năng khác
                this.DoProcessSkillAppendDamages(skillRef.Data, oldLevel, skillRef.Level);

                /// Cập nhật thông tin kỹ năng vào DB
                KT_TCPHandler.UpdateSkillInfoFromDB(this.Player, id, skillRef.AddedLevel, this.GetSkillLastUsedTick(id), this.GetSkillCooldown(id), skillRef.Exp);
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Cập nhật dữ liệu danh sách cấp độ kỹ năng
        /// </summary>
        /// <param name="data"></param>
        public void AddSkillsLevel(ICollection<KeyValuePair<int, int>> data)
        {
            foreach (KeyValuePair<int, int> pair in data)
            {
                if (this.listStudiedSkills.TryGetValue(pair.Key, out SkillLevelRef skillRef))
                {
                    /// Nếu là kỹ năng bị động hoặc vòng sáng đang được kích hoạt
                    if (skillRef.Data.Type == 3 || (skillRef.Data.IsArua && this.Player.Buffs.CurrentArua != null && this.Player.Buffs.CurrentArua.Skill.SkillID == skillRef.SkillID))
                    {
                        /// Lấy giá trị Buff cũ
                        BuffDataEx buff = this.Player.Buffs.GetBuff(skillRef.SkillID);
                        if (buff != null)
                        {
                            /// Xóa Buff cũ đi
                            this.Player.Buffs.RemoveBuff(buff, true, false);
                        }
                    }

                    int oldLevel = skillRef.Level;
                    skillRef.AddedLevel += pair.Value;
                    skillRef.AddedLevel = System.Math.Min(skillRef.AddedLevel, skillRef.Data.MaxSkillLevel);

                    /// Nếu là kỹ năng bị động hoặc vòng sáng đang được kích hoạt
                    if (skillRef.Data.Type == 3 || (skillRef.Data.IsArua && this.Player.Buffs.CurrentArua != null && this.Player.Buffs.CurrentArua.Skill.SkillID == skillRef.SkillID))
                    {
                        /// Thực hiện lại kỹ năng bị động
                        KTSkillManager.ProcessPassiveSkill(this.Player, skillRef);
                    }

                    /// Thực hiện cập nhật danh sách hỗ trợ sát thương cho kỹ năng khác
                    this.DoProcessSkillAppendDamages(skillRef.Data, oldLevel, skillRef.Level);

                    /// Cập nhật thông tin kỹ năng vào DB
                    KT_TCPHandler.UpdateSkillInfoFromDB(this.Player, pair.Key, skillRef.AddedLevel, this.GetSkillLastUsedTick(skillRef.SkillID), this.GetSkillCooldown(skillRef.SkillID), skillRef.Exp);
                }
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Tẩy điểm kỹ năng trong danh sách tương ứng
        /// </summary>
        /// <param name="data"></param>
        public void ResetSkillsLevel(ICollection<int> data)
        {
            foreach (int id in data)
            {
                if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skill))
                {
                    /// Xóa Buff tương ứng nếu là kỹ năng bị động
                    if (skill.Data.Type == 3)
                    {
                        this.Player.Buffs.RemoveBuff(skill.SkillID);
                    }

                    /// Thiết lập cấp độ về 0
                    this.listStudiedSkills[id].AddedLevel = 0;

                    /// Xóa sát thương hỗ trợ
                    this.appendSkillDamages.TryRemove(id, out _);
                    this.appendSkillDamagesPercent.TryRemove(id, out _);

                    /// Lưu kỹ năng vào DB
                    KT_TCPHandler.UpdateSkillInfoFromDB(this.Player, skill.SkillID, skill.AddedLevel, this.GetSkillLastUsedTick(skill.SkillID), this.GetSkillCooldown(skill.SkillID), skill.Exp);
                }
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Tẩy lại toàn bộ điểm kỹ năng
        /// </summary>
        public void ResetAllSkillsLevel()
        {
            /// Duyệt toàn bộ danh sách
            foreach (SkillLevelRef skillRef in this.listStudiedSkills.Values)
            {
                /// Xóa Buff tương ứng nếu là kỹ năng bị động
                if (skillRef.Data.Type == 3)
                {
                    this.Player.Buffs.RemoveBuff(skillRef.SkillID);
                }

                /// Thiết lập cấp độ về 0
                skillRef.AddedLevel = 0;

                /// Xóa sát thương hỗ trợ
                this.appendSkillDamages.Clear();
                this.appendSkillDamagesPercent.Clear();

                /// Lưu kỹ năng vào DB
                KT_TCPHandler.UpdateSkillInfoFromDB(this.Player, skillRef.SkillID, skillRef.AddedLevel, this.GetSkillLastUsedTick(skillRef.SkillID), this.GetSkillCooldown(skillRef.SkillID), skillRef.Exp);
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Xóa kỹ năng
        /// </summary>
        public void RemoveSkill(int id)
        {
            if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skill))
            {
                /// Xóa Buff tương ứng nếu là kỹ năng bị động
                if (skill.Data.Type == 3)
                {
                    this.Player.Buffs.RemoveBuff(skill.SkillID);
                }

                /// Xóa kỹ năng khỏi danh sách
                this.listStudiedSkills.TryRemove(id, out _);

                /// Thực hiện giảm sát thương được hỗ trợ
                this.DoProcessSkillAppendDamages(skill.Data, skill.Level, 0);

                /// Xóa kỹ năng khỏi DB
                KT_TCPHandler.DeleteSkillFromDB(this.Player, id);
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Xóa kỹ năng
        /// </summary>
        /// <param name="skillIds"></param>
        public void RemoveSkills(ICollection<int> skillIds)
        {
            foreach (int id in skillIds)
            {
                if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef skill))
                {
                    /// Xóa Buff tương ứng nếu là kỹ năng bị động
                    if (skill.Data.Type == 3)
                    {
                        this.Player.Buffs.RemoveBuff(skill.SkillID);
                    }

                    /// Xóa kỹ năng khỏi danh sách
                    this.listStudiedSkills.TryRemove(id, out _);

                    /// Xóa kỹ năng khỏi DB
                    KT_TCPHandler.DeleteSkillFromDB(this.Player, id);
                }
            }
            this.Player.RefreshSkillPoints();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.ExportSkillTree();

            /// Thực hiện lại các kỹ năng được hỗ trợ
            this.ProcessEnchantSkills();
        }

        /// <summary>
        /// Trả về thông tin kỹ năng
        /// </summary>
        /// <param name="id"></param>
        public SkillLevelRef GetSkillLevelRef(int id)
        {
            if (this.listStudiedSkills.TryGetValue(id, out SkillLevelRef levelRef))
            {
                return levelRef;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra trong SkillTree có kỹ năng tương ứng không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasSkill(int id)
        {
            return this.GetSkillLevelRef(id) != null;
        }
    }
}
