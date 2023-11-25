using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ mở toàn bộ cơ quan theo thứ tự
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// ID cơ quan hiện tại đã mở
        /// </summary>
        private int currentOrderedTriggerID;

        /// <summary>
        /// Bắt đầu nhiệm vụ mở toàn bộ cơ quan theo thứ tự
        /// </summary>
        /// <param name="task"></param>
        private void Begin_OpenOrderedTriggers(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo cơ quan
            this.CreateIndexTriggers(task.IndexTriggers, (trigger, player, triggerInfo) => {
                /// Nếu mở sai thứ tự
                if (this.currentOrderedTriggerID != triggerInfo.Order)
                {
                    /// Hiện thông báo mở sai
                    this.NotifyAllPlayers(string.Format("Thật đáng tiếc, thứ tự cơ quan [{0}] mở không chính xác, tất cả mở lại từ đầu!", player.RoleName));
                    /// Thiết lập lại ID cơ quan đã mở
                    this.currentOrderedTriggerID = 0;
                }
                /// Nếu mở đúng thứ tự
                else
                {
                    /// Thiết lập ID cơ quan đã mở
                    this.currentOrderedTriggerID = triggerInfo.Order;
                }
            });
            /// Thiết lập lại ID cơ quan đã mở
            this.currentOrderedTriggerID = 0;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ mở toàn bộ cơ quan theo thứ tự
        /// </summary>
        /// <param name="task"></param>
        private bool Track_OpenOrderedTriggers(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi đã mở toàn bộ cơ quan
            return this.currentOrderedTriggerID >= task.IndexTriggers.Triggers.Count;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở toàn bộ cơ quan theo thứ tự
        /// </summary>
        private void Reset_OpenOrderedTriggers()
        {
            this.currentOrderedTriggerID = 0;
        }
    }
}
