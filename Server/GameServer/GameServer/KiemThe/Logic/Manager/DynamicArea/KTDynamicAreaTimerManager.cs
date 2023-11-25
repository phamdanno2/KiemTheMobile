using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.LuaSystem;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý luồng thực thi khu vực động
    /// </summary>
    public class KTDynamicAreaTimerManager
    {
        #region Singleton - Instance

        /// <summary>
        /// Đối tượng quản lý Timer của đối tượng
        /// </summary>
        public static KTDynamicAreaTimerManager Instance { get; private set; }

        /// <summary>
        /// Private constructor
        /// </summary>
        private KTDynamicAreaTimerManager() : base()
        {
            this.InitLocal();
        }

        #endregion Singleton - Instance

        #region Initialize

        /// <summary>
        /// Hàm này gọi đến khởi tạo đối tượng
        /// </summary>
        public static void Init()
        {
            KTDynamicAreaTimerManager.Instance = new KTDynamicAreaTimerManager();
        }

        #endregion Initialize

        #region Inheritance
        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private class KTDynamicAreaTimer : KTTimer
        {
            /// <summary>
            /// Đối tượng khu vực động
            /// </summary>
            public KDynamicArea Data { get; set; }
        }

        /// <summary>
        /// Inner Timer
        /// </summary>
        private class InnerTimer : KTTimerManager<KTDynamicAreaTimer>
        {
            /// <summary>
            /// Thời gian kích hoạt luồng kiểm tra
            /// </summary>
            protected override int PeriodTick
            {
                get
                {
                    return 500;
                }
            }

            /// <summary>
            /// Inner Timer
            /// </summary>
            public InnerTimer() : base()
            {

            }
        }
        #endregion

        /// <summary>
        /// Thời gian thực hiện sự kiện Tick
        /// </summary>
        private const float PeriodTick = 1f;

        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private readonly InnerTimer timer = new InnerTimer();

        /// <summary>
        /// Danh sách Timer đang thực thi
        /// </summary>
        private readonly ConcurrentDictionary<int, KTDynamicAreaTimer> dynamicAreaTimers = new ConcurrentDictionary<int, KTDynamicAreaTimer>();

        #region Core
        /// <summary>
        /// Khởi tạo ban đầu nội bộ
        /// </summary>
        private void InitLocal()
        {

        }
        #endregion

        /// <summary>
        /// Thêm đối tượng vào danh sách quản lý
        /// </summary>
        /// <param name="obj"></param>
        public void Add(KDynamicArea obj)
        {
            if (obj == null)
            {
                return;
            }

            /// Thời gian còn lại
            float totalTimes = (KTGlobal.GetCurrentTimeMilis() - obj.StartTicks) / 1000f;

            /// Nếu đã hết thời gian
            if (obj.LifeTime >= 0 && totalTimes >= obj.LifeTime)
			{
                /// Xóa khỏi luồng quản lý luôn
                KTDynamicAreaManager.Remove(obj.ID);
                return;
            }

            /// Nếu tồn tại thì bỏ qua Timer cũ
            if (this.dynamicAreaTimers.TryGetValue(obj.ID, out KTDynamicAreaTimer objectTimer))
            {
                return;
            }

            KTDynamicAreaTimer timer = new KTDynamicAreaTimer()
            {
                Name = "DynamicArea " + obj.ID,
                Alive = true,
                Interval = obj.LifeTime >= 0 ? obj.LifeTime - totalTimes : -1,
                PeriodActivation = obj.Tick,
                Data = obj,
                Destroy = () => {
                    obj.Clear();
                },
            };
            timer.Tick = () => {
                /// Nếu vi phạm điều kiện, cần xóa khỏi Timer
                if (this.NeedToBeRemoved(obj))
                {
                    this.Remove(obj);
                    return;
                }
                /// Thực thi sự kiện Tick
                obj.ProcessTick();
            };
            timer.Finish = () => {
                /// Xóa đối tượng
                this.Remove(obj);
                /// Xóa khỏi luồng quản lý luôn
                KTDynamicAreaManager.Remove(obj.ID);
            };
            this.timer.AddTimer(timer);

            this.dynamicAreaTimers[obj.ID] = timer;
        }

        /// <summary>
        /// Dừng và xóa đối tượng khỏi luồng thực thi
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(KDynamicArea obj)
        {
            /// Nếu tồn tại
            if (this.dynamicAreaTimers.TryGetValue(obj.ID, out KTDynamicAreaTimer objectTimer))
            {
                this.timer.KillTimer(objectTimer);
                this.dynamicAreaTimers.TryRemove(obj.ID, out _);
            }
        }

        #region Logic
        /// <summary>
        /// Đánh dấu có cần thiết phải xóa khu vực động này ra khỏi Timer không
        /// <param name="dynArea"></param>
        /// </summary>
        /// <returns></returns>
        private bool NeedToBeRemoved(KDynamicArea dynArea)
        {
            /// Nếu không có tham chiếu
            if (dynArea == null)
            {
                return true;
            }

            /// Nếu không tìm thấy bản đồ hiện tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(dynArea.CurrentMapCode, out GameMap map))
            {
                return true;
            }
            /// Nếu có phụ bản nhưng không tìm thấy thông tin
            if (dynArea.CurrentCopyMapID != -1 && !CopySceneEventManager.IsCopySceneExist(dynArea.CurrentCopyMapID, dynArea.CurrentMapCode))
            {
                return true;
            }

            /// Nếu không có người chơi xung quanh
            if (dynArea.VisibleClientsNum <= 0)
            {
                return true;
            }

            /// Nếu thỏa mãn tất cả điều kiện
            return false;
        }
        #endregion
    }
}