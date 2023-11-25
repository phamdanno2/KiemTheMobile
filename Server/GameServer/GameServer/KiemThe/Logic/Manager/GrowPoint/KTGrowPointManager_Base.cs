using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Utilities.Algorithms;
using GameServer.Logic;
using Server.Protocol;
using Server.TCP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.Logic.Manager
{
    /// <summary>
    /// Quản lý thêm sửa xóa
    /// </summary>
    public static partial class KTGrowPointManager
    {
        /// <summary>
        /// Đối tượng quản lý ID tự tăng có thể tái sử dụng
        /// </summary>
        public static AutoIndexReusablePattern AutoIndexManager = new AutoIndexReusablePattern();

        /// <summary>
        /// Thêm điểm thu thập vào danh sách quản lý
        /// </summary>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="copySceneID">ID phụ bản</param>
        /// <param name="data">Dữ liệu điểm thu thập</param>
        /// <param name="posX">Tọa độ X</param>
        /// <param name="posY">Tọa độ Y</param>
        /// <param name="lifeTime">Thời gian tồn tại (-1 nếu tồn tại vĩnh viễn)</param>
        public static GrowPoint Add(int mapCode, int copySceneID, GrowPointXML data, int posX, int posY, long lifeTime = -1)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            GrowPoint growPoint = new GrowPoint()
            {
                ID = KTGrowPointManager.AutoIndexManager.GetNewID(),
                Data = data,
                Name = data.Name,
                ObjectType = ObjectTypes.OT_GROWPOINT,
                MapCode = mapCode,
                CurrentCopyMapID = copySceneID,
                CurrentPos = new System.Windows.Point(posX, posY),
                CurrentGrid = new System.Windows.Point(posX / gameMap.MapGridWidth, posY / gameMap.MapGridHeight),
                RespawnTime = data.RespawnTime,
                ScriptID = data.ScriptID,
                Alive = true,
                LifeTime = lifeTime,
            };
            /// Thực hiện tự động xóa
            growPoint.ProcessAutoRemoveTimeout();
            KTGrowPointManager.GrowPoints[growPoint.ID] = growPoint;
            /// Thêm điểm thu thập vào đối tượng quản lý map
            KTGrowPointManager.AddToMap(growPoint);

            /// Trả về đối tượng
            return growPoint;
        }

        /// <summary>
        /// Xóa điểm thu thập khỏi danh sách
        /// </summary>
        /// <param name="growPointID">ID điểm thu thập</param>
        public static void Remove(int growPointID)
        {
            if (KTGrowPointManager.GrowPoints.TryGetValue(growPointID, out GrowPoint growPoint))
            {
                KTGrowPointManager.RemoveFromMap(growPoint);
                KTGrowPointManager.GrowPoints.TryRemove(growPointID, out _);
                /// Trả ID lại để tái sử dụng
                KTGrowPointManager.AutoIndexManager.ReturnID(growPoint.ID);
            }
        }

        /// <summary>
        /// Xóa điểm thu thập khỏi danh sách
        /// </summary>
        /// <param name="growPoint">Điểm thu thập</param>
        public static void Remove(GrowPoint growPoint)
        {
            if (growPoint == null)
            {
                return;
            }

            KTGrowPointManager.RemoveFromMap(growPoint);
            KTGrowPointManager.GrowPoints.TryRemove(growPoint.ID, out _);
            /// Trả ID lại để tái sử dụng
            KTGrowPointManager.AutoIndexManager.ReturnID(growPoint.ID);
        }

        /// <summary>
        /// Thêm điểm thu thập vào Map
        /// </summary>
        /// <param name="growPoint">Đối tượng điểm thu thập</param>
        /// <returns></returns>
        public static bool AddToMap(GrowPoint growPoint)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(growPoint.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            /// Thêm điểm thu thập vào Map
            if (mapGrid.MoveObject(-1, -1, (int) growPoint.CurrentPos.X, (int) (growPoint.CurrentPos.Y), growPoint))
            {
                KTGrowPointManager.NotifyNearClientsToAddSelf(growPoint);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Xóa điểm thu thập tương ứng khỏi bản đồ
        /// </summary>
        /// <param name="growPoint">Đối tượng điểm thu thập</param>
        /// <returns></returns>
        private static bool RemoveFromMap(GrowPoint growPoint)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(growPoint.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            if (KTGrowPointManager.GrowPoints.TryGetValue(growPoint.ID, out _))
            {
                /// Xóa điểm thu thập khỏi Map
                mapGrid.RemoveObject(growPoint);
                KTGrowPointManager.NotifyNearClientsToRemoveSelf(growPoint);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Xóa toàn bộ khu vực động khỏi bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static void RemoveMapGrowPoints(int mapCode, int copyMapID = -1)
        {
            if (mapCode <= 0)
            {
                return;
            }

            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            if (null == mapGrid)
            {
                return;
            }

            List<int> keysToDel = new List<int>();

            /// Duyệt toàn bộ danh sách NPC
            foreach (KeyValuePair<int, GrowPoint> item in KTGrowPointManager.GrowPoints)
            {
                if (item.Value.MapCode == mapCode && (copyMapID == -1 || item.Value.CurrentCopyMapID == copyMapID))
                {
                    mapGrid.RemoveObject(item.Value);
                    keysToDel.Add(item.Key);
                }
            }

            /// Xóa các bản ghi đã tìm bên trên
            foreach (int key in keysToDel)
            {
                KTGrowPointManager.GrowPoints.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// Trả về danh sách điểm thu thập trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static List<GrowPoint> GetMapGrowPoints(int mapCode, int copyMapID = -1)
        {
            List<GrowPoint> results = new List<GrowPoint>();

            if (mapCode <= 0)
            {
                return results;
            }

            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            if (null == mapGrid)
            {
                return results;
            }

            /// Duyệt toàn bộ danh sách NPC
            foreach (KeyValuePair<int, GrowPoint> item in KTGrowPointManager.GrowPoints)
            {
                if (item.Value.MapCode == mapCode && (copyMapID == -1 || item.Value.CurrentCopyMapID == copyMapID))
                {
                    results.Add(item.Value);
                }
            }

            /// Trả về kết quả
            return results;
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh thêm đối tượng vào danh sách hiển thị
        /// </summary>
        /// <param name="growPoint">Đối tượng điểm thu thập</param>
        private static void NotifyNearClientsToAddSelf(GrowPoint growPoint)
        {
            List<Object> objsList = Global.GetAll9Clients(growPoint);

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

                if (!growPoint.Alive)
                {
                    continue;
                }

                /// Thực hiện cập nhật đối tượng
                //Global.GameClientMoveGrid(client);

                GameManager.ClientMgr.NotifyMySelfGrowPoint(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, growPoint);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh xóa đối tượng khỏi danh sách hiển thị
        /// </summary>
        /// <param name="growPoint">Đối tượng điểm thu thập</param>
        public static void NotifyNearClientsToRemoveSelf(GrowPoint growPoint)
        {
            List<Object> objsList = Global.GetAll9Clients2(growPoint.MapCode, (int) growPoint.CurrentPos.X, (int) growPoint.CurrentPos.Y, growPoint.CurrentCopyMapID);
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

                GameManager.ClientMgr.NotifyMySelfDelGrowPoint(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, growPoint.MapCode, growPoint.ID);
            }
        }

        /// <summary>
        /// Gửi danh sách điểm thu thập xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách điểm thu thập</param>
        public static void SendMySelfGrowPoints(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
                return;
            GrowPoint growPoint = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                growPoint = objsList[i] as GrowPoint;
                if (null == growPoint)
                {
                    continue;
                }

                if (!growPoint.Alive)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfGrowPoint(sl, pool, client, growPoint);
            }
        }

        /// <summary>
        /// Xóa điểm thu thập với người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách điểm thu thập</param>
        public static void DelMySelfGrowPoints(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            GrowPoint growPoint;
            for (int i = 0; i < objsList.Count; i++)
            {
                growPoint = objsList[i] as GrowPoint;
                if (null == growPoint)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDelGrowPoint(sl, pool, client, growPoint.CurrentMapCode, growPoint.ID);
            }
        }
    }
}
