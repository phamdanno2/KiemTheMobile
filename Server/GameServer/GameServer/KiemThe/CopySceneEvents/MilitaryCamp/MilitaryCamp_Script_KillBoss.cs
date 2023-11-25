using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ giết Boss
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Đánh dấu Boss đã bị tiêu diệt chưa
        /// </summary>
        private bool isBossKilled;

        /// <summary>
        /// Bắt đầu nhiệm vụ tiêu diệt Boss
        /// </summary>
        /// <param name="task"></param>
        private void Begin_KillBoss(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo Boss chính
            this.CreateBoss(task.Boss, (_) => {
                /// Đánh dấu đã tiêu diệt Boss
                this.isBossKilled = true;
            });
            /// Tạo Boss phụ
            this.CreateBoss(task.ChildBoss);
            /// Đánh dấu Boss chưa bị tiêu diệt
            this.isBossKilled = false;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ tiêu diệt Boss
        /// </summary>
        /// <param name="task"></param>
        private bool Track_KillBoss(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi đã tiêu diệt Boss
            return this.isBossKilled;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ tiêu diệt Boss
        /// </summary>
        private void Reset_KillBoss()
        {
            this.isBossKilled = false;
        }
    }
}
