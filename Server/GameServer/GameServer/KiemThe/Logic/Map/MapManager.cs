using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng quản lý bản đồ
    /// </summary>
    public class MapManager
    {
        /// <summary>
        /// Đối tượng quản lý bản đồ
        /// </summary>
        public MapManager()
        {
        }

        /// <summary>
        /// Danh sách bản đồ theo ID
        /// </summary>
        private Dictionary<int, GameMap> _DictMaps = new Dictionary<int, GameMap>(10);

        /// <summary>
        /// Danh sách bản đồ theo ID
        /// </summary>
        public Dictionary<int, GameMap> DictMaps
        {
            get { return _DictMaps; }
        }

        /// <summary>
        /// Thêm bản đồ vào danh sách
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="mapName"></param>
        /// <param name="mapConfigDir"></param>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="mapLevel"></param>
        /// <param name="mapType"></param>
        public GameMap InitAddMap(int mapCode, string mapName, string mapConfigDir, int mapWidth, int mapHeight, int mapLevel, string mapType)
        {
            GameMap gameMap = new GameMap()
            {
                MapCode = mapCode,
                MapName = mapName,
                MapConfigDir = mapConfigDir,
                MapWidth = mapWidth,
                MapHeight = mapHeight,
                MapLevel = mapLevel,
                MapType = mapType,
            };

            gameMap.InitMap();

            lock (this._DictMaps)
            {
                this._DictMaps.Add(mapCode, gameMap);
            }

            return gameMap;
        }

        /// <summary>
        /// Trả về bản đồ có ID tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public GameMap GetGameMap(int mapCode)
        {
            if (this._DictMaps.TryGetValue(mapCode, out GameMap gameMap) && gameMap != null)
            {
                return gameMap;
            }
            return null;
        }
    }
}
