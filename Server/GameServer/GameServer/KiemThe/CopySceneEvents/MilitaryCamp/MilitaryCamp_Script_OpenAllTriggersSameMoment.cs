using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ mở toàn bộ cơ quan cùng lúc
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Danh sách cơ quan đã mở
        /// </summary>
        private readonly HashSet<int> openedTriggers = new HashSet<int>();

        /// <summary>
        /// Bắt đầu nhiệm vụ mở toàn bộ cơ quan cùng lúc
        /// </summary>
        /// <param name="task"></param>
        private void Begin_OpenAllTriggersSameMoment(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo cơ quan
            this.CreateIndexTriggers(task.IndexTriggers, (trigger, player, triggerInfo) => {
                /// Nếu chưa có cơ quan nào được mở
                if (this.openedTriggers.Count <= 0)
                {
                    /// Đếm lùi 3s sẽ thất bại
                    this.SetTimeout(3000, () => {
                        /// Nếu tất cả cơ quan chưa được mở
                        if (this.openedTriggers.Count < task.IndexTriggers.Triggers.Count)
                        {
                            /// Thông báo thất bại
                            this.NotifyAllPlayers("Khai mở cơ quan thất bại. Cần mở đồng loạt cơ quan cùng lúc!");
                            /// Xóa danh sách cơ quan đã mở
                            this.openedTriggers.Clear();
                        }
                    });
                }

                /// Thêm vào danh sách cơ quan đã mở
                this.openedTriggers.Add(trigger.ID);
            });
            /// Xóa danh sách cơ quan đã mở
            this.openedTriggers.Clear();
        }

        /// <summary>
        /// Theo dõi nhiệm vụ mở toàn bộ cơ quan cùng lúc
        /// </summary>
        /// <param name="task"></param>
        private bool Track_OpenAllTriggersSameMoment(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi đã mở toàn bộ cơ quan
            return this.openedTriggers.Count >= task.IndexTriggers.Triggers.Count;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở toàn bộ cơ quan cùng lúc
        /// </summary>
        private void Reset_OpenAllTriggersSameMoment()
        {
            this.openedTriggers.Clear();
        }
    }
}
