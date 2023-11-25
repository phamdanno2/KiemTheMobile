using GameServer.Logic;
using Server.Protocol;
using Server.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Đối tượng quản lý bẫy
    /// </summary>
    public static class KTTrapManager
    {
        /// <summary>
        /// Đối tượng sử dụng khóa LOCK
        /// </summary>
        private static readonly object Mutex = new object();

        /// <summary>
        /// Danh sách bẫy
        /// </summary>
        private static readonly Dictionary<int, Trap> Traps = new Dictionary<int, Trap>();

        /// <summary>
        /// Thêm bẫy vào danh sách quản lý
        /// </summary>
        /// <param name="trapID">ID bẫy</param>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="resID">ID Res</param>
        /// <param name="posX">Vị trí X</param>
        /// <param name="posY">Vị trí Y</param>
        /// <param name="lifeTime">Thời gian tồn tại</param>
        public static void AddTrap(int trapID, int mapCode, GameObject owner, int resID, int posX, int posY, float lifeTime)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            Trap trap = new Trap()
            {
                TrapID = trapID,
                MapCode = mapCode,
                Owner = owner,
                ResID = resID,
                CurrentPos = new System.Windows.Point(posX, posY),
                CurrentGrid = new System.Windows.Point(posX / gameMap.MapGridWidth, posY / gameMap.MapGridHeight),
                ObjectType = ObjectTypes.OT_TRAP,
                LifeTime = lifeTime,
            };
            lock (KTTrapManager.Mutex)
            {
                KTTrapManager.Traps[trapID] = trap;
                /// Thêm bẫy vào đối tượng quản lý map
                KTTrapManager.AddTrapToMap(trap);
            }
        }

        /// <summary>
        /// Xóa bẫy khỏi danh sách
        /// </summary>
        /// <param name="trapID">ID bẫy</param>
        public static void RemoveTrap(int trapID)
        {
            lock (KTTrapManager.Mutex)
            {
                if (KTTrapManager.Traps.TryGetValue(trapID, out Trap trap))
                {
                    KTTrapManager.RemoveTrapFromMap(trap);
                    KTTrapManager.Traps.Remove(trapID);
                }
            }
        }

        /// <summary>
        /// Thêm bẫy vào Map
        /// </summary>
        /// <param name="trap">Đối tượng bẫy</param>
        /// <returns></returns>
        private static bool AddTrapToMap(Trap trap)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(trap.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            /// Thêm bẫy vào Map
            if (mapGrid.MoveObject(-1, -1, (int) trap.CurrentPos.X, (int) (trap.CurrentPos.Y), trap))
            {
                //KTTrapManager.NotifyNearClientsToAddSelf(trap);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Xóa bẫy tương ứng khỏi bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        private static bool RemoveTrapFromMap(Trap trap)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(trap.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            if (KTTrapManager.Traps.TryGetValue(trap.TrapID, out _))
            {
                /// Xóa bẫy khỏi Map
                mapGrid.RemoveObject(trap);
                //KTTrapManager.NotifyNearClientsToRemoveSelf(trap);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh thêm đối tượng vào danh sách hiển thị
        /// </summary>
        /// <param name="trap"></param>
        private static void NotifyNearClientsToAddSelf(Trap trap)
        {
            List<Object> objsList = Global.GetAll9Clients(trap);

            if (null == objsList)
                return;

            KPlayer client = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                client = objsList[i] as KPlayer;
                if (null == client)
                {
                    continue;
                }

                if (!trap.IsVisibleTo(client))
                {
                    continue;
                }

                /// Thực hiện cập nhật đối tượng
                //Global.GameClientMoveGrid(client);

                GameManager.ClientMgr.NotifyMySelfTrap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, trap);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh xóa đối tượng khỏi danh sách hiển thị
        /// </summary>
        /// <param name="trap"></param>
        private static void NotifyNearClientsToRemoveSelf(Trap trap)
        {
            List<Object> objsList = Global.GetAll9Clients2(trap.MapCode, (int) trap.CurrentPos.X, (int) trap.CurrentPos.Y, trap.CurrentCopyMapID);
            if (null == objsList)
                return;

            KPlayer client = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                client = objsList[i] as KPlayer;
                if (null == client)
                {
                    continue;
                }

                /// Thực hiện cập nhật đối tượng
                //Global.GameClientMoveGrid(client);

                GameManager.ClientMgr.NotifyMySelfDelTrap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, trap.MapCode, trap.TrapID);
            }
        }

        /// <summary>
        /// Gửi danh sách bẫy xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách bẫy</param>
        public static void SendMySelfTraps(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
                return;
            Trap trap = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                trap = objsList[i] as Trap;
                if (null == trap)
                {
                    continue;
                }

                if (!trap.IsVisibleTo(client))
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfTrap(sl, pool, client, trap);
            }
        }

        /// <summary>
        /// Xóa bẫy với người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách bẫy</param>
        public static void DelMySelfTraps(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            Trap trap;
            for (int i = 0; i < objsList.Count; i++)
            {
                trap = objsList[i] as Trap;
                if (null == trap)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDelTrap(sl, pool, client, trap.CurrentMapCode, trap.TrapID);
            }
        }
    }
}
