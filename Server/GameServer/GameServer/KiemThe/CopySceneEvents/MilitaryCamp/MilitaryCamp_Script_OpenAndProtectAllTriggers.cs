using GameServer.KiemThe.Core;
using GameServer.KiemThe.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ mở toàn bộ cơ quan và dập lửa
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Danh sách cơ quan đang bảo vệ
        /// </summary>
        private readonly HashSet<GrowPoint> protectingTriggers = new HashSet<GrowPoint>();

        /// <summary>
        /// Thời điểm lần trước tạo thánh hỏa
        /// </summary>
        private long lastCreateHolyFireTicks;

        /// <summary>
        /// Bắt đầu nhiệm vụ mở toàn bộ cơ quan và dập lửa
        /// </summary>
        /// <param name="task"></param>
        private void Begin_OpenAndProtectAllTriggers(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Duyệt danh sách cơ quan
            foreach (MilitaryCamp.EventInfo.StageInfo.TaskInfo.TriggerInfo triggerInfo in task.ProtectTrigger.Triggers)
            {
                /// Tạo cơ quan
                this.CreateTrigger(triggerInfo, (trigger, player) => {
                    /// Thêm vào danh sách cơ quan đã mở
                    this.protectingTriggers.Add(trigger);
                    /// Thông báo đã mở cơ quan
                    this.NotifyAllPlayers("Một tiếng động lớn, cơ quan đã mở ra. Chú ý thánh hỏa có thể phá hủy cơ quan.");
                });
            }
            
            /// Xóa danh sách cơ quan đã mở
            this.protectingTriggers.Clear();
            /// Hủy toàn bộ thánh hỏa
            this.RemoveAllHolyFires(task.ProtectTrigger.HolyFire);
            /// Đánh dấu thời gian lần cuối tạo thánh hỏa
            this.lastCreateHolyFireTicks = KTGlobal.GetCurrentTimeMilis();
        }

        /// <summary>
        /// Theo dõi nhiệm vụ mở toàn bộ cơ quan và dập lửa
        /// </summary>
        /// <param name="task"></param>
        private bool Track_OpenAndProtectAllTriggers(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Nếu đã mở toàn bộ cơ quan
            if (this.protectingTriggers.Count >= task.ProtectTrigger.Triggers.Count)
            {
                /// Hủy toàn bộ thánh hỏa
                this.RemoveAllHolyFires(task.ProtectTrigger.HolyFire);
                /// Hoàn thành nhiệm vụ
                return true;
            }

            /// Nếu đã đến thời gian sinh thánh hỏa
            if (KTGlobal.GetCurrentTimeMilis() - this.lastCreateHolyFireTicks >= task.ProtectTrigger.HolyFire.SpawnEveryTicks)
            {
                /// Duyệt danh sách cơ quan đã mở
                foreach (GrowPoint trigger in this.protectingTriggers)
                {
                    /// Vị trí hiện tại của cơ quan
                    UnityEngine.Vector2 triggerPos = new UnityEngine.Vector2((int) trigger.CurrentPos.X, (int) trigger.CurrentPos.Y);

                    /// Lấy vị trí ngẫu nhiên xung quanh
                    UnityEngine.Vector2 randomPos = KTMath.GetRandomPointAroundPos(triggerPos, KTGlobal.GetRandomNumber(task.ProtectTrigger.HolyFire.RandomRadiusMin, task.ProtectTrigger.HolyFire.RandomRadiusMax));

                    /// Tạo thánh hỏa
                    this.CreateHolyFire(task.ProtectTrigger.HolyFire, (int) randomPos.x, (int) randomPos.y, () => {
                        /// Thông báo phá hủy cơ quan
                        this.NotifyAllPlayers("Thánh hỏa đã phá hủy cơ quan, phải khai mở lại từ đầu.");
                        /// Xóa danh sách cơ quan đã mở
                        this.protectingTriggers.Clear();
                        /// Hủy toàn bộ thánh hỏa
                        this.RemoveAllHolyFires(task.ProtectTrigger.HolyFire);
                        /// Đánh dấu thời gian lần cuối tạo thánh hỏa
                        this.lastCreateHolyFireTicks = KTGlobal.GetCurrentTimeMilis();
                        /// Bỏ qua
                        return;
                    });
                }

                /// Đánh dấu thời gian lần cuối tạo thánh hỏa
                this.lastCreateHolyFireTicks = KTGlobal.GetCurrentTimeMilis();
            }

            /// Chưa hoàn thành nhiệm vụ
            return false;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở toàn bộ cơ quan cùng lúc
        /// </summary>
        private void Reset_OpenAndProtectAllTriggers()
        {
            this.protectingTriggers.Clear();
            this.lastCreateHolyFireTicks = 0;
        }
    }
}
