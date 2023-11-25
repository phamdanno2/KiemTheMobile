using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ mở cơ quan
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Đánh dấu cơ quan đã mở chưa
        /// </summary>
        private bool isTriggerOpened;

        /// <summary>
        /// Bắt đầu nhiệm vụ mở cơ quan
        /// </summary>
        /// <param name="task"></param>
        private void Begin_OpenTrigger(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo cơ quan
            this.CreateTrigger(task.Trigger, (trigger, player) => {
                /// Đánh dấu đã mở cơ quan
                this.isTriggerOpened = true;
            });
            /// Đánh dấu chưa mở cơ quan
            this.isTriggerOpened = false;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ mở cơ quan
        /// </summary>
        /// <param name="task"></param>
        private bool Track_OpenTrigger(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi đã mở cơ quan
            return this.isTriggerOpened;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở cơ quan
        /// </summary>
        private void Reset_OpenTrigger()
        {
            this.isTriggerOpened = false;
        }
    }
}
