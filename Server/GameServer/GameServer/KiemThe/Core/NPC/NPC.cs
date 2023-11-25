using GameServer.Interface;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng NPC
    /// </summary>
    public class NPC : IObject
    {
        /// <summary>
        /// ID tự tăng
        /// </summary>
        private static int AutoID = 0;

        /// <summary>
        /// Đối tượng NPC
        /// </summary>
        public NPC()
        {
            /// Chống Bug làm tràn ID
            /// Chỉ NPC tạo ra ở Phụ Bản mới vượt quá ngưỡng này
            if (NPC.AutoID >= 10000007)
            {
                NPC.AutoID = 100000;
            }
            this.NPCID = NPC.AutoID++;
        }

        /// <summary>
        /// ID NPC
        /// </summary>
        public int NPCID { get; private set; }

        /// <summary>
        /// ID của files cấu hình
        /// </summary>
        public int ResID { get; set; }

        /// <summary>
        /// ID Map
        /// </summary>
        public int MapCode { get; set; } = -1;

        /// <summary>
        /// Tọa độ lưới
        /// </summary>
        public Point GridPoint { get; set; }

        /// <summary>
        /// ID phụ bản
        /// </summary>
        public int CopyMapID { get; set; } = -1;

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        public int ScriptID { get; set; }

        /// <summary>
        /// Tên NPC
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên ở bản đồ nhỏ
        /// </summary>
        public string MinimapName { get; set; }

        /// <summary>
        /// Có hiển thị ở bản đồ khu vực không
        /// </summary>
        public bool VisibleOnMinimap { get; set; }

        /// <summary>
        /// Danh hiệu NPC
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện khi NPC được Click
        /// </summary>
        public Action<KPlayer> Click { get; set; }

        #region Kế thừa IObject

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public ObjectTypes ObjectType
        {
            get { return ObjectTypes.OT_NPC; }
        }

        /// <summary>
        /// Thời gian Tick hồi máu trước đó
        /// </summary>
        public long LastLifeMagicTick { get; set; }

        /// <summary>
        /// Vị trí hiện tại (tọa độ lưới)
        /// </summary>
        public Point CurrentGrid
        {
            get
            {
                return this.GridPoint;
            }

            set
            {
                this.GridPoint = value;
            }
        }

        /// <summary>
        /// Vị trí hiện tại
        /// </summary>
        private Point _CurrentPos = new Point(0, 0);

        /// <summary>
        /// Vị trí hiện tại
        /// </summary>
        public Point CurrentPos
        {
            get
            {
                return _CurrentPos;
            }

            set
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MapCode];
                this.GridPoint = new Point((int) (value.X / gameMap.MapGridWidth), (int) (value.Y / gameMap.MapGridHeight));
                _CurrentPos = value;
            }
        }

        /// <summary>
        /// ID map hiện tại
        /// </summary>
        public int CurrentMapCode
        {
            get
            {
                return this.MapCode;
            }
        }

        /// <summary>
        /// ID phụ bản hiện tại
        /// </summary>
        public int CurrentCopyMapID
        {
            get
            {
                return this.CopyMapID;
            }
        }

        /// <summary>
        /// Hướng hiện tại
        /// </summary>
        public KiemThe.Entities.Direction CurrentDir
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Hiển thị NPC
        /// </summary>
        public bool ShowNpc { get; set; } = true;
    }
}
