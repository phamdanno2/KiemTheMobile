using System;
using Server.Data;
using FSPlay.GameEngine.Teleport;

namespace FSPlay.GameEngine.Logic
{
	/// <summary>
	/// Chuyển Map theo Teleport
	/// </summary>
	public class MapConversionByTeleportCodeEventArgs : EventArgs
    {
        /// <summary>
        /// Teleport
        /// </summary>
        public GTeleport Teleport
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Sự kiện thông báo tới đối tượng
    /// </summary>
    public class SpriteNotifyEventArgs : EventArgs
    {
        /// <summary>
        /// ID đối tượng
        /// </summary>
        public int RoleID
        {
            get;
            set;
        }

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public GSpriteTypes SpriteType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Thông tin quái chờ tải
    /// </summary>
    public class BurstMonsterItem
    {
        /// <summary>
        /// Dữ liệu quái
        /// </summary>
        public MonsterData MonsterData
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ X
        /// </summary>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ Y
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Hướng quay
        /// </summary>
        public int Direction
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Thông tin người chơi khác
    /// </summary>
    public class OtherRoleItem
    {
        /// <summary>
        /// Dữ liệu nhân vật
        /// </summary>
        public RoleData RoleData
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ X
        /// </summary>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ Y
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Hướng
        /// </summary>
        public int Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Động tác hiện tại
        /// </summary>
        public int CurrentAction
        {
            get;
            set;
        }
    }
}
