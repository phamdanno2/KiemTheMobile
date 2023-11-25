using GameServer.KiemThe.Entities;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Thực hiện Logic kỹ năng bị động
    /// </summary>
    public static partial class KTSkillManager
    {
        /// <summary>
        /// Thực hiện kỹ năng bị động
        /// </summary>
        /// <param name="caster">Đối tượng xuất chiêu</param>
        /// <param name="skill">Kỹ năng</param>
        private static void DoSkillPassive(GameObject caster, SkillLevelRef skill)
        {
            BuffDataEx buff = new BuffDataEx()
            {
                Duration = -1,
                LoseWhenUsingSkill = false,
                Skill = skill,
                SaveToDB = false,
                StartTick = KTGlobal.GetCurrentTimeMilis(),
            };
            caster.Buffs.AddBuff(buff);
        }
    }
}
