using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng lưới bản đồ
    /// </summary>
    public class MapGridManager
    {
        /// <summary>
        /// Đối tượng lưới bản đồ
        /// </summary>
        public MapGridManager()
        {
        }

        /// <summary>
        /// Danh sách quản lý theo ID map
        /// </summary>
        private Dictionary<int, MapGrid> _DictGrids = new Dictionary<int, MapGrid>(100);

        /// <summary>
        /// Danh sách quản lý theo ID map
        /// </summary>
        public Dictionary<int, MapGrid> DictGrids
        {
            get { return _DictGrids; }
        }

        /// <summary>
        /// Khởi tạo lưới bản đồ
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        public void InitAddMapGrid(int mapCode, int mapWidth, int mapHeight, int gridWidth, int gridHeight, GameMap gameMap)
        {
            MapGrid mapGrid = new MapGrid(mapCode, mapWidth, mapHeight, gridWidth, gridHeight, gameMap);

            lock (_DictGrids)
            {
                _DictGrids.Add(mapCode, mapGrid);
            }
        }

        /// <summary>
        /// Trả về lưới bản đồ theo ID bản đồ tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public MapGrid GetMapGrid(int mapCode)
        {
            MapGrid mapGrid;
            lock (_DictGrids)
            {
                if (_DictGrids.TryGetValue(mapCode, out mapGrid))
                {
                    return mapGrid;
                }

                return null;
            }
        }
    }
}
