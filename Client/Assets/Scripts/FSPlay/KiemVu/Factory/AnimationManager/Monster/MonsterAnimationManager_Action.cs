using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Factory.Animation
{
    /// <summary>
    /// Quản lý động tác quái
    /// </summary>
    public partial class MonsterAnimationManager
    {
        /// <summary>
        /// Lấy tên động tác dựa theo loại
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public string GetActionName(MonsterActionType actionType)
        {
            string actionName = "";
            switch (actionType)
            {
                case MonsterActionType.FightStand:
                    actionName = "FightStand";
                    break;
                case MonsterActionType.NormalStand:
                    actionName = "NormalStand1";
                    break;
                case MonsterActionType.Run:
                    actionName = "NormalRun";
                    break;
                case MonsterActionType.RunAttack:
                    actionName = "NormalRun";
                    break;
                case MonsterActionType.Wound:
                    actionName = "Wound";
                    break;
                case MonsterActionType.NormalAttack:
                    actionName = "Attack1";
                    break;
                case MonsterActionType.CritAttack:
                    actionName = "Attack2";
                    break;
                case MonsterActionType.Die:
                    actionName = "Die";
                    break;
            }
            return actionName;
        }
    }
}
