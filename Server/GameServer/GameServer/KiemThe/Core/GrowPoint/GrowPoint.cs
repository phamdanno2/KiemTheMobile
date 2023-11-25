using GameServer.Interface;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.KiemThe.Core
{
    /// <summary>
    /// Đối tượng thu thập
    /// </summary>
    public class GrowPoint : IObject
    {
        /// <summary>
        /// ID thu thập
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Dữ liệu cấu hình
        /// </summary>
        public GrowPointXML Data { get; set; }

        /// <summary>
        /// Tên điểm thu thập
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Thời gian tái tạo (-1 nếu không tái tạo)
        /// <para>Đơn vị Milis</para>
        /// </summary>
        public long RespawnTime { get; set; }

        /// <summary>
        /// Thời gian tồn tại (-1 nếu tồn tại vĩnh viễn)
        /// <para>Đơn vị Milis</para>
        /// </summary>
        public long LifeTime { get; set; } = -1;

        /// <summary>
        /// ID bản đồ
        /// </summary>
        public int MapCode { get; set; }

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        public int ScriptID { get; set; }

        /// <summary>
        /// Đối tượng còn tồn tại không
        /// </summary>
        public bool Alive { get; set; }

        /// <summary>
        /// Thực thi tự động xóa khi hết thời gian
        /// </summary>
        public void ProcessAutoRemoveTimeout()
		{
            /// Nếu có thời gian tồn tại
            if (this.LifeTime > 0)
            {
                /// Thực hiện đếm lùi tự xóa
                KTTaskManager.Instance.AddSchedule(new KTTaskManager.KTSchedule()
                {
                    Name = "GrowPoint auto remove",
                    Loop = false,
                    Interval = this.LifeTime / 1000f,
                    Work = () => {
                        /// Xóa
                        this.Alive = false;
                        /// Thông báo tới người chơi xung quanh xóa đối tượng
                        KTGrowPointManager.NotifyNearClientsToRemoveSelf(this);
                    },
                });
            }
        }

        /// <summary>
        /// Đối tượng thu thập
        /// </summary>
        public GrowPoint()
		{

		}


        #region IObject
        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public ObjectTypes ObjectType { get; set; }

        /// <summary>
        /// Tọa độ lưới
        /// </summary>
        public Point CurrentGrid { get; set; }

        /// <summary>
        /// Tọa độ thực
        /// </summary>
        public Point CurrentPos { get; set; }

        /// <summary>
        /// ID bản đồ hiện tại
        /// </summary>
        public int CurrentMapCode
        {
            get
            {
                lock (this)
                {
                    return this.MapCode;
                }
            }
        }

        /// <summary>
        /// ID phụ bản hiện tại
        /// </summary>
        public int CurrentCopyMapID { get; set; } = -1;

        /// <summary>
        /// Hướng quay hiện tại
        /// </summary>
        public KiemThe.Entities.Direction CurrentDir { get; set; } = KiemThe.Entities.Direction.DOWN;

        /// <summary>
        /// Funtion thực thi thêm sau khi thu thập xong
        /// </summary>
        public Action<KPlayer> GrowPointCollectCompleted { get; set; }

        /// <summary>
        /// Kiểm tra điều kiện
        /// </summary>
        public Predicate<KPlayer> ConditionCheck { get; set; } = (player) => { return true; };
        #endregion
    }
}
