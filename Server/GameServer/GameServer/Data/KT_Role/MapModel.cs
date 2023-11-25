using System.Collections.Generic;

namespace GameServer.Logic
{
    /// <summary>
    /// Thông tin điểm truyền tống
    /// </summary>
    public class MapTeleport
    {
        /// <summary>
        /// ID cổng trong bản đồ
        /// </summary>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// ID bản đồ
        /// </summary>
        public int MapID
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
        /// Radius
        /// </summary>
        public int Radius
        {
            get;
            set;
        }

        /// <summary>
        /// ID bản đồ tới
        /// </summary>
        public int ToMapID
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ tới X
        /// </summary>
        public int ToX
        {
            get;
            set;
        }

        /// <summary>
        /// Tọa độ tới Y
        /// </summary>
        public int ToY
        {
            get;
            set;
        }

        /// <summary>
        /// Phe nào được sử dụng dịch chuyển này
        /// </summary>
        public int Camp
        {
            get; set;
        }
    }

    /// <summary>
    /// Danh sách các đối tượng trong bản đồ
    /// </summary>
    public struct MapGridSpriteItem
    {
        /// <summary>
        /// Đối tượng dùng để sử dụng khóa LOCK
        /// </summary>
        public object GridLock { get; set; }

        /// <summary>
        /// Danh sách đối tượng
        /// </summary>
        public List<object> ObjsList { get; set; }

        /// <summary>
        /// Tổng số đối tượng người chơi
        /// </summary>
        public short RoleNum { get; set; }

        /// <summary>
        /// Tổng số đối tượng quái
        /// </summary>
        public short MonsterNum { get; set; }

        /// <summary>
        /// Tổng số NPC
        /// </summary>
        public short NPCNum { get; set; }

        /// <summary>
        /// Tổng số vật phẩm
        /// </summary>
        public short GoodsPackNum { get; set; }

        /// <summary>
        /// Tổng số bẫy
        /// </summary>
        public short TrapNum { get; set; }

        /// <summary>
        /// Tổng số điểm thu thập
        /// </summary>
        public short GrowPointNum { get; set; }

        /// <summary>
        /// Tổng số khu vực động
        /// </summary>
        public short DynamicAreaNum { get; set; }

        /// <summary>
        /// Tổng số BOT
        /// </summary>
        public short BotNum { get; set; }
    }

    public static class SpriteBaseIds
    {
        public const int RoleBaseId = 0;

        public const int MonsterBaseId = 0x7F010000;
        public const int PetBaseId = 0x7F400000;
        public const int BiaoCheBaseId = 0x7F410000;
        public const int JunQiBaseId = 0x7F420000;
        public const int FakeRoleBaseId = 0x7F430000;
        public const int MaxId = 0x7F500000;
    }
}