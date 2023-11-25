using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý người chơi
    /// </summary>
    public class SpriteContainer
    {
        /// <summary>
        /// Danh sách người chơi
        /// </summary>
        private readonly ConcurrentDictionary<int, KPlayer> Players = new ConcurrentDictionary<int, KPlayer>();

        /// <summary>
        /// Danh sách người chơi theo bản đồ
        /// </summary>
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, KPlayer>> PlayersByMap = new ConcurrentDictionary<int, ConcurrentDictionary<int, KPlayer>>();

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="mapItems"></param>
        public void Initialize(IEnumerable<XElement> mapItems)
        {
            /// Duyệt danh sách XMLNode
            foreach (XElement mapItem in mapItems)
            {
                /// ID bản đồ
                int mapCode = (int)Global.GetSafeAttributeLong(mapItem, "ID");
                /// Tạo mới danh sách người chơi theo bản đồ
                this.PlayersByMap[mapCode] = new ConcurrentDictionary<int, KPlayer>();
            }
        }

        /// <summary>
        /// Thêm người chơi vào bản đồ tương ứng
        /// </summary>
        /// <param name="player"></param>
        public void AddObject(KPlayer player)
        {
            /// Thêm vào danh sách tổng
            this.Players[player.RoleID] = player;
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.PlayersByMap.TryGetValue(player.CurrentMapCode, out ConcurrentDictionary<int, KPlayer> playersDict))
            {
                return;
            }

            /// Toác
            if (playersDict == null)
            {
                return;
            }

            /// Thêm vào danh sách theo bản đồ
            playersDict[player.RoleID] = player;
        }

        /// <summary>
        /// Xóa người chơi khỏi bản đồ tương ứng
        /// </summary>
        /// <param name="player"></param>
        public void RemoveObject(KPlayer player)
        {
            /// Xóa khỏi danh sách tổng
            this.Players.TryRemove(player.RoleID, out _);

            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.PlayersByMap.TryGetValue(player.CurrentMapCode, out ConcurrentDictionary<int, KPlayer> playersDict))
            {
                return;
            }

            /// Toác
            if (playersDict == null)
            {
                return;
            }

            /// Xóa khỏi bản đồ tương ứng
            playersDict.TryRemove(player.RoleID, out _);
        }

        /// <summary>
        /// Trả về danh sách người chơi trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<KPlayer> GetObjectsByMap(int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.PlayersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, KPlayer> playersDict))
            {
                return new List<KPlayer>();
            }

            /// Toác
            if (playersDict == null)
            {
                return new List<KPlayer>();
            }

            /// Kết quả
            List<KPlayer> players = new List<KPlayer>();
            /// Duyệt danh sách
            List<int> playerIDs = playersDict.Keys.ToList();

            foreach (int playerID in playerIDs)
            {
                /// Nếu không tồn tại
                if (!playersDict.TryGetValue(playerID, out KPlayer player))
                {
                    continue;
                }

                /// Nếu không kiểm tra phụ bản hoặc ở trong phụ bản tương ứng
                if (copySceneID == -1 || copySceneID == player.CurrentCopyMapID)
                {
                    /// Thêm vào danh sách
                    players.Add(player);
                }
            }

            /// Trả về kết quả
            return players;
        }

        /// <summary>
        /// Trả về danh sách người chơi trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<KPlayer> GetObjectsByMap(Predicate<KPlayer> predicate, int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.PlayersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, KPlayer> playersDict))
            {
                return new List<KPlayer>();
            }

            /// Toác
            if (playersDict == null)
            {
                return new List<KPlayer>();
            }

            /// Kết quả
            List<KPlayer> players = new List<KPlayer>();
            /// Duyệt danh sách
            List<int> playerIDs = playersDict.Keys.ToList();
            foreach (int playerID in playerIDs)
            {
                /// Nếu không tồn tại
                if (!playersDict.TryGetValue(playerID, out KPlayer player))
                {
                    continue;
                }

                /// Nếu không kiểm tra phụ bản hoặc ở trong phụ bản tương ứng
                if (copySceneID == -1 || copySceneID == player.CurrentCopyMapID)
                {
                    /// Nếu thỏa mãn điều kiện
                    if (predicate(player))
                    {
                        /// Thêm vào danh sách
                        players.Add(player);
                    }
                }
            }

            /// Trả về kết quả
            return players;
        }

        /// <summary>
        /// Trả về tổng số người chơi trong bản đồ tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetObjectsCountByMap(int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.PlayersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, KPlayer> playersDict))
            {
                return 0;
            }

            /// Toác
            if (playersDict == null)
            {
                return 0;
            }

            /// Tổng số người chơi thỏa mãn
            int count = 0;
            /// Nếu có kiểm tra phụ bản
            if (copySceneID != -1)
            {
                /// Duyệt danh sách người chơi trong bản đồ
                List<int> playerIDs = playersDict.Keys.ToList();
                foreach (int playerID in playerIDs)
                {
                    /// Nếu không tồn tại
                    if (!playersDict.TryGetValue(playerID, out KPlayer player))
                    {
                        continue;
                    }

                    /// Nếu nằm trong cùng phụ bản
                    if (player.CurrentCopyMapID == copySceneID)
                    {
                        count++;
                    }
                }
            }
            else
            {
                count = playersDict.Count;
            }

            /// Trả về kết quả
            return count;
        }

        /// <summary>
        /// Tìm người chơi có ID tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public KPlayer FindObject(int id)
        {
            /// Nếu không tồn tại
            if (!this.Players.TryGetValue(id, out KPlayer player))
            {
                return null;
            }
            /// Trả về kết quả
            return player;
        }
    }
}
