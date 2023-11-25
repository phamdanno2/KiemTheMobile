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
    public static partial class KTDynamicAreaManager
    {
        /// <summary>
        /// Đối tượng quản lý ID tự tăng có thể tái sử dụng
        /// </summary>
        private static readonly AutoIndexReusablePattern AutoIndexManager = new AutoIndexReusablePattern();

        /// <summary>
        /// Thêm khu vực động vào danh sách quản lý
        /// </summary>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="copySceneID">ID phụ bản</param>
        /// <param name="name">Tên khu vực động</param>
        /// <param name="resID">ID Res trong file Monster quy định</param>
        /// <param name="posX">Tọa độ X</param>
        /// <param name="posY">Tọa độ Y</param>
        /// <param name="lifeTime">Thời gian tồn tại đơn vị giây (-1 là vĩnh viễn)</param>
        /// <param name="tick">Thời gian tick đơn vị giây (-1 là vĩnh viễn)</param>
        /// <param name="radius">Bán kính quét</param>
        /// <param name="scriptID">Tọa độ Y</param>
        /// <param name="tag">Tag</param>
        public static KDynamicArea Add(int mapCode, int copySceneID, string name, int resID, int posX, int posY, float lifeTime, float tick, int radius, int scriptID, string tag = "")
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            KDynamicArea dynArea = new KDynamicArea()
            {
                ID = KTDynamicAreaManager.AutoIndexManager.GetNewID(),
                Name = name,
                ObjectType = ObjectTypes.OT_DYNAMIC_AREA,
                MapCode = mapCode,
                CurrentCopyMapID = copySceneID,
                CurrentPos = new System.Windows.Point(posX, posY),
                CurrentGrid = new System.Windows.Point(posX / gameMap.MapGridWidth, posY / gameMap.MapGridHeight),
                LifeTime = lifeTime,
                Tick = tick,
                ResID = resID,
                Radius = radius,
                ScriptID = scriptID,
                Tag = tag,
                StartTicks = KTGlobal.GetCurrentTimeMilis(),
            };
            KTDynamicAreaManager.dynamicAreas[dynArea.ID] = dynArea;
            /// Thêm khu vực động vào đối tượng quản lý map
            KTDynamicAreaManager.AddToMap(dynArea);
            return dynArea;
        }

        /// <summary>
        /// Xóa khu vực động khỏi danh sách
        /// </summary>
        /// <param name="areaID">ID khu vực động</param>
        public static void Remove(int areaID)
        {
            if (KTDynamicAreaManager.dynamicAreas.TryGetValue(areaID, out KDynamicArea dynArea))
            {
                dynArea.Clear();
                KTDynamicAreaManager.RemoveFromMap(dynArea);
                KTDynamicAreaManager.dynamicAreas.TryRemove(areaID, out _);
                /// Trả ID lại để tái sử dụng
                KTDynamicAreaManager.AutoIndexManager.ReturnID(dynArea.ID);
            }
        }

        /// <summary>
        /// Xóa khu vực động khỏi danh sách
        /// </summary>
        /// <param name="dynArea">Khu vực động</param>
        public static void Remove(KDynamicArea dynArea)
        {
            if (dynArea == null)
            {
                return;
            }

            dynArea.Clear();
            KTDynamicAreaManager.RemoveFromMap(dynArea);
            KTDynamicAreaManager.dynamicAreas.TryRemove(dynArea.ID, out _);
            /// Trả ID lại để tái sử dụng
            KTDynamicAreaManager.AutoIndexManager.ReturnID(dynArea.ID);
        }

        /// <summary>
        /// Thêm khu vực động vào Map
        /// </summary>
        /// <param name="dynArea">Đối tượng khu vực động</param>
        /// <returns></returns>
        private static bool AddToMap(KDynamicArea dynArea)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(dynArea.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            /// Thêm khu vực động vào Map
            if (mapGrid.MoveObject(-1, -1, (int) dynArea.CurrentPos.X, (int) (dynArea.CurrentPos.Y), dynArea))
            {
              //  KTDynamicAreaManager.NotifyNearClientsToAddSelf(dynArea);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Xóa khu vực động tương ứng khỏi bản đồ
        /// </summary>
        /// <param name="dynArea">Đối tượng khu vực động</param>
        /// <returns></returns>
        private static bool RemoveFromMap(KDynamicArea dynArea)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(dynArea.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            if (KTDynamicAreaManager.dynamicAreas.TryGetValue(dynArea.ID, out _))
            {
                /// Xóa khu vực động khỏi Map
                mapGrid.RemoveObject(dynArea);
                KTDynamicAreaManager.NotifyNearClientsToRemoveSelf(dynArea);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Trả về danh sách khu vực động trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static List<KDynamicArea> GetMapDynamicAreas(int mapCode, int copyMapID = -1)
        {
            List<KDynamicArea> results = new List<KDynamicArea>();

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
            foreach (KeyValuePair<int, KDynamicArea> item in KTDynamicAreaManager.dynamicAreas)
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
        /// Xóa toàn bộ khu vực động khỏi bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static void RemoveMapDynamicAreas(int mapCode, int copyMapID = -1)
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
            foreach (KeyValuePair<int, KDynamicArea> item in KTDynamicAreaManager.dynamicAreas)
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
                KTDynamicAreaManager.dynamicAreas.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh thêm đối tượng vào danh sách hiển thị
        /// </summary>
        /// <param name="dynArea">Đối tượng khu vực động</param>
        private static void NotifyNearClientsToAddSelf(KDynamicArea dynArea)
        {
            List<Object> objsList = Global.GetAll9Clients(dynArea);

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

                GameManager.ClientMgr.NotifyMySelfDynamicArea(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, dynArea);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh xóa đối tượng khỏi danh sách hiển thị
        /// </summary>
        /// <param name="dynArea">Đối tượng khu vực động</param>
        private static void NotifyNearClientsToRemoveSelf(KDynamicArea dynArea)
        {
            List<Object> objsList = Global.GetAll9Clients2(dynArea.MapCode, (int) dynArea.CurrentPos.X, (int) dynArea.CurrentPos.Y, dynArea.CurrentCopyMapID);
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

                GameManager.ClientMgr.NotifyMySelfDelDynamicArea(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, dynArea.MapCode, dynArea.ID);
            }
        }

        /// <summary>
        /// Gửi danh sách khu vực động xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách khu vực động</param>
        public static void SendMySelfDynamicAreas(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            KDynamicArea dynArea = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                dynArea = objsList[i] as KDynamicArea;
                if (null == dynArea)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDynamicArea(sl, pool, client, dynArea);
            }
        }

        /// <summary>
        /// Xóa khu vực động với người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách khu vực động</param>
        public static void DelMySelfDynamicAreas(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            KDynamicArea dynArea;
            for (int i = 0; i < objsList.Count; i++)
            {
                dynArea = objsList[i] as KDynamicArea;
                if (null == dynArea)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDelDynamicArea(sl, pool, client, dynArea.CurrentMapCode, dynArea.ID);
            }
        }
    }
}
