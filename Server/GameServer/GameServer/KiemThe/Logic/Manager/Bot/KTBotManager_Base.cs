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
    public static partial class KTBotManager
    {
        /// <summary>
        /// Đối tượng quản lý ID tự tăng có thể tái sử dụng
        /// </summary>
        private static readonly AutoIndexReusablePattern AutoIndexManager = new AutoIndexReusablePattern();

        /// <summary>
        /// Thêm khu bot vào danh sách quản lý
        /// </summary>
        /// <param name="bot">Đối tượng BOT</param>
        public static void Add(KTBot bot)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[bot.CurrentMapCode];
            lock (KTBotManager.Mutex)
            {
                KTBotManager.bots[bot.RoleID] = bot;
                /// Thêm BOT vào đối tượng quản lý map
                KTBotManager.AddToMap(bot);
            }
        }

        /// <summary>
        /// Xóa BOT khỏi danh sách
        /// </summary>
        /// <param name="areaID">ID BOT</param>
        public static void Remove(int areaID)
        {
            lock (KTBotManager.Mutex)
            {
                if (KTBotManager.bots.TryGetValue(areaID, out KTBot bot))
                {
                    KTBotManager.RemoveFromMap(bot);
                    KTBotManager.bots.Remove(areaID);
                    /// Trả ID lại để tái sử dụng
                    KTBotManager.AutoIndexManager.ReturnID(bot.RoleID);
                }
            }
        }

        /// <summary>
        /// Xóa BOT khỏi danh sách
        /// </summary>
        /// <param name="bot">BOT</param>
        public static void Remove(KTBot bot)
        {
            if (bot == null)
            {
                return;
            }

            lock (KTBotManager.Mutex)
            {
                KTBotManager.RemoveFromMap(bot);
                KTBotManager.bots.Remove(bot.RoleID);
                /// Trả ID lại để tái sử dụng
                KTBotManager.AutoIndexManager.ReturnID(bot.RoleID);
            }
        }

        /// <summary>
        /// Thêm BOT vào Map
        /// </summary>
        /// <param name="bot">Đối tượng BOT</param>
        /// <returns></returns>
        private static bool AddToMap(KTBot bot)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(bot.CurrentMapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            /// Thêm BOT vào Map
            if (mapGrid.MoveObject(-1, -1, (int) bot.CurrentPos.X, (int) (bot.CurrentPos.Y), bot))
            {
                //  KTBotManager.NotifyNearClientsToAddSelf(bot);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Xóa BOT tương ứng khỏi bản đồ
        /// </summary>
        /// <param name="bot">Đối tượng BOT</param>
        /// <returns></returns>
        private static bool RemoveFromMap(KTBot bot)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(bot.CurrentMapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            if (KTBotManager.bots.TryGetValue(bot.RoleID, out _))
            {
                /// Xóa BOT khỏi Map
                mapGrid.RemoveObject(bot);
                KTBotManager.NotifyNearClientsToRemoveSelf(bot);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Trả về danh sách BOT trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static List<KTBot> GetMapBots(int mapCode, int copyMapID = -1)
        {
            List<KTBot> results = new List<KTBot>();

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
            foreach (KeyValuePair<int, KTBot> item in KTBotManager.bots)
            {
                if (item.Value.CurrentMapCode == mapCode && (copyMapID == -1 || item.Value.CurrentCopyMapID == copyMapID))
                {
                    results.Add(item.Value);
                }
            }

            /// Trả về kết quả
            return results;
        }

        /// <summary>
        /// Xóa toàn bộ BOT khỏi bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static void RemoveMapBots(int mapCode, int copyMapID = -1)
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
            foreach (KeyValuePair<int, KTBot> item in KTBotManager.bots)
            {
                if (item.Value.CurrentMapCode == mapCode && (copyMapID == -1 || item.Value.CurrentCopyMapID == copyMapID))
                {
                    mapGrid.RemoveObject(item.Value);
                    keysToDel.Add(item.Key);
                }
            }

            /// Xóa các bản ghi đã tìm bên trên
            foreach (int key in keysToDel)
            {
                KTBotManager.bots.Remove(key);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh thêm đối tượng vào danh sách hiển thị
        /// </summary>
        /// <param name="bot">Đối tượng BOT</param>
        private static void NotifyNearClientsToAddSelf(KTBot bot)
        {
            List<Object> objsList = Global.GetAll9Clients(bot);

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

                GameManager.ClientMgr.NotifyMySelfBot(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, bot);
            }
        }

        /// <summary>
        /// Thông báo toàn bộ người chơi xung quanh xóa đối tượng khỏi danh sách hiển thị
        /// </summary>
        /// <param name="bot">Đối tượng BOT</param>
        private static void NotifyNearClientsToRemoveSelf(KTBot bot)
        {
            List<Object> objsList = Global.GetAll9Clients2(bot.CurrentMapCode, (int) bot.CurrentPos.X, (int) bot.CurrentPos.Y, bot.CurrentCopyMapID);
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

                GameManager.ClientMgr.NotifyMySelfDelBot(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, bot.CurrentMapCode, bot.RoleID);
            }
        }

        /// <summary>
        /// Gửi danh sách BOT xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách BOT</param>
        public static void SendMySelfBots(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            KTBot bot = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                bot = objsList[i] as KTBot;
                if (null == bot)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfBot(sl, pool, client, bot);
            }
        }

        /// <summary>
        /// Xóa BOT với người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList">Danh sách BOT</param>
        public static void DelMySelfBots(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList)
            {
                return;
            }
            KTBot bot;
            for (int i = 0; i < objsList.Count; i++)
            {
                bot = objsList[i] as KTBot;
                if (null == bot)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDelBot(sl, pool, client, client.CurrentMapCode, bot.RoleID);
            }
        }
    }
}
