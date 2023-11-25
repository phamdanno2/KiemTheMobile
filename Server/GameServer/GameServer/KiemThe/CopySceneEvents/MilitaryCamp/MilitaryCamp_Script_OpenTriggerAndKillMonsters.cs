using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ mở cơ quan và tiêu diệt quái
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Số lượng quái của cơ quan đã bị tiêu diệt
        /// </summary>
        private int killedTriggerMonstersCount;

        /// <summary>
        /// Bắt đầu nhiệm vụ mở cơ quan và tiêu diệt quái
        /// </summary>
        /// <param name="task"></param>
        private void Begin_OpenTriggerAndKillMonsters(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Duyệt danh sách quái
            foreach (MilitaryCamp.EventInfo.StageInfo.MonsterInfo monsterInfo in task.Monsters)
            {
                /// Tạo quái
                this.CreateMonster(monsterInfo, (_) => {
                    /// Tăng số lượng quái đã tiêu diệt
                    this.killedTriggerMonstersCount++;
                });
            }
            /// Thiết lập số lượng quái đã tiêu diệt
            this.killedTriggerMonstersCount = 0;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ mở cơ quan và tiêu diệt quái
        /// </summary>
        /// <param name="task"></param>
        private bool Track_OpenTriggerAndKillMonsters(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Nếu đã tiêu diệt toàn bộ quái thì hoàn thành
            return this.killedTriggerMonstersCount == task.Monsters.Count;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở cơ quan và tiêu diệt quái
        /// </summary>
        private void Reset_OpenTriggerAndKillMonsters()
        {
            this.killedTriggerMonstersCount = 0;
        }
    }
}
