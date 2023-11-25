using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ hộ tống NPC
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Đối tượng NPC di chuyển
        /// </summary>
        private Monster movingNPC;

        /// <summary>
        /// Bắt đầu nhiệm vụ hộ tống NPC
        /// </summary>
        /// <param name="task"></param>
        private void Begin_TransferNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo NPC di chuyển
            this.CreateMovingNPC(task.MovingNPC, (npc) => {
                /// Lưu lại thông tin NPC di chuyển
                this.movingNPC = npc;
            });
        }

        /// <summary>
        /// Theo dõi nhiệm vụ hộ tống NPC
        /// </summary>
        /// <param name="task"></param>
        private bool Track_TransferNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi NPC đã đến đích
            return KTGlobal.GetDistanceBetweenPoints(this.movingNPC.CurrentPos, new System.Windows.Point(task.MovingNPC.ToPosX, task.MovingNPC.ToPosY)) <= 10;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ hộ tống NPC
        /// </summary>
        private void Reset_TransferNPC()
        {
            this.movingNPC = null;
        }
    }
}
