using GameServer.KiemThe.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.Entities
{
    /// <summary>
    /// Quản lý Logic kỹ năng bị động
    /// </summary>
    public partial class SkillTree
    {
        /// <summary>
        /// Làm mới hiệu ứng của kỹ năng bị động
        /// </summary>
        public void ProcessPassiveSkills()
        {
            /// Duyệt toàn bộ danh sách
            foreach (SkillLevelRef skill in this.listStudiedSkills.Values)
            {
                /// Nếu kỹ năng không phù hợp
                if (!this.IsValidSkill(skill))
                {
                    continue;
                }

                /// Nếu tồn tại và là kỹ năng bị động
                if (skill.Data.Type == 3 && skill.Level > 0)
                {
                    KTSkillManager.ProcessPassiveSkill(this.Player, skill);
                }
            }
        }

        /// <summary>
        /// Xóa toàn bộ hiệu ứng của kỹ năng bị động
        /// </summary>
        public void RemovePassiveSkillEffects()
		{
            /// Duyệt toàn bộ danh sách
            foreach (SkillLevelRef skill in this.listStudiedSkills.Values)
            {
                /// Nếu kỹ năng không phù hợp
                if (!this.IsValidSkill(skill))
                {
                    continue;
                }

                /// Nếu tồn tại và là kỹ năng bị động
                if (skill.Data.Type == 3 && skill.Level > 0)
                {
                    /// Nếu có Buff tương ứng
                    if (this.Player.Buffs.HasBuff(skill.SkillID))
					{
                        /// Xóa Buff tương ứng
                        this.Player.Buffs.RemoveBuff(skill.SkillID);
					}
                }
            }
        }
    }
}
