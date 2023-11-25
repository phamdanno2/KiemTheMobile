using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Server.Tools;
using GameServer.Interface;
using GameServer.KiemThe.Logic;
using System.Collections.Concurrent;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý bản đồ dạng lưới
    /// </summary>
    public class MapGrid
    {
        /// <summary>
        /// Đối tượng quản lý bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="mapGridWidth"></param>
        /// <param name="mapGridHeight"></param>
        /// <param name="gameMap"></param>
        public MapGrid(int mapCode, int mapWidth, int mapHeight, int mapGridWidth, int mapGridHeight, GameMap gameMap)
        {
            this.MapCode = mapCode;
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;
            this._MapGridWidth = mapGridWidth;
            this._MapGridHeight = mapGridHeight;

            this._MapGridXNum = (MapWidth - 1) / _MapGridWidth + 1;
            this._MapGridYNum = (MapHeight - 1) / _MapGridHeight + 1;
            this._MapGridTotalNum = _MapGridXNum * _MapGridYNum;
            this._GameMap = gameMap;

            this.MyMapGridSpriteItem = new MapGridSpriteItem[_MapGridTotalNum];
            for (int i = 0; i < MyMapGridSpriteItem.Length; i++)
            {
                this.MyMapGridSpriteItem[i].GridLock = new object();
                this.MyMapGridSpriteItem[i].ObjsList = new List<object>();
            }
        }

        /// <summary>
        /// Đối tượng bản đồ
        /// </summary>
        private GameMap _GameMap = null;

        /// <summary>
        /// ID bản đồ
        /// </summary>
        private readonly int MapCode;

        /// <summary>
        /// Chiều rộng
        /// </summary>
        private readonly int MapWidth;

        /// <summary>
        /// Chiều cao
        /// </summary>
        private readonly int MapHeight;

        /// <summary>
        /// Chiều rộng kích thước lưới
        /// </summary>
        private readonly int _MapGridWidth;

        /// <summary>
        /// Chiều rộng kích thước lưới
        /// </summary>
        public int MapGridWidth
        {
            get { return _MapGridWidth; }
        }

        /// <summary>
        /// Chiều dài kích thước lưới
        /// </summary>
        private readonly int _MapGridHeight;

        /// <summary>
        /// Chiều dài kích thước lưới
        /// </summary>
        public int MapGridHeight
        {
            get { return _MapGridHeight; }
        }

        /// <summary>
        /// Số điểm theo trục X trong lưới
        /// </summary>
        private readonly int _MapGridXNum = 0;

        /// <summary>
        /// Số điểm theo trục X trong lưới
        /// </summary>
        public int MapGridXNum
        {
            get { return _MapGridXNum; }
        }

        /// <summary>
        /// Số điểm theo trục Y trong lưới
        /// </summary>
        private readonly int _MapGridYNum = 0;

        /// <summary>
        /// Số điểm theo trục Y trong lưới
        /// </summary>
        public int MapGridYNum
        {
            get { return _MapGridYNum; }
        }

        private int _MapGridTotalNum = 0;

        /// <summary>
        /// ID đối tượng tương ứng
        /// </summary>
        private readonly ConcurrentDictionary<object, int> _Obj2GridDict = new ConcurrentDictionary<object, int>();

        /// <summary>
        /// Danh sách đối tượng trong lưới
        /// </summary>
        private MapGridSpriteItem[] MyMapGridSpriteItem = null;

        /// <summary>
        /// Trả về vị trí trong danh sách lưới
        /// </summary>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <returns></returns>
        private int GetGridIndex(int gridX, int gridY)
        {
            return (this._MapGridXNum * gridY) + gridX;
        }

        /// <summary>
        /// Thay đổi số lượng đối tượng trong lưới
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="addOrSubNum"></param>
        private void ChangeMapGridsSpriteNum(int index, IObject obj, short addOrSubNum)
        {
            switch (obj.ObjectType)
            {
                case ObjectTypes.OT_CLIENT:
                    this.MyMapGridSpriteItem[index].RoleNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].RoleNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].RoleNum);
                    break;
                case ObjectTypes.OT_MONSTER:
                    this.MyMapGridSpriteItem[index].MonsterNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].MonsterNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].MonsterNum);
                    break;
                case ObjectTypes.OT_NPC:
                    this.MyMapGridSpriteItem[index].NPCNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].NPCNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].NPCNum);
                    break;
                case ObjectTypes.OT_GOODSPACK:
                    this.MyMapGridSpriteItem[index].GoodsPackNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].GoodsPackNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].GoodsPackNum);
                    break;
                case ObjectTypes.OT_TRAP:
                    this.MyMapGridSpriteItem[index].TrapNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].TrapNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].TrapNum);
                    break;
                case ObjectTypes.OT_GROWPOINT:
                    this.MyMapGridSpriteItem[index].GrowPointNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].GrowPointNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].GrowPointNum);
                    break;
                case ObjectTypes.OT_DYNAMIC_AREA:
                    this.MyMapGridSpriteItem[index].DynamicAreaNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].DynamicAreaNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].DynamicAreaNum);
                    break;
                case ObjectTypes.OT_BOT:
                    this.MyMapGridSpriteItem[index].BotNum += addOrSubNum;
                    this.MyMapGridSpriteItem[index].BotNum = (short)Global.GMax(0, this.MyMapGridSpriteItem[index].BotNum);
                    break;
            }
        }


        /// <summary>
        /// Di chuyển đối tượng trên bản đồ
        /// </summary>
        /// <param name="oldX">Tọa độ thực cũ X</param>
        /// <param name="oldY">Tọa độ thực cũ Y</param>
        /// <param name="newX">Tọa độ thực mới X</param>
        /// <param name="newY">Tọa độ thực mới Y</param>
        public bool MoveObject(int oldX, int oldY, int newX, int newY, IObject obj)
        {
            if (newX < 0 || newY < 0 || newX >= MapWidth || newY >= MapHeight)
            {
                return false;
            }

            int gridX = newX / this._MapGridWidth;
            int gridY = newY / this._MapGridHeight;
            int oldGridIndex = -1;
            if (!this._Obj2GridDict.TryGetValue(obj, out oldGridIndex))
            {
                oldGridIndex = -1;
            }

            int gridIndex = this.GetGridIndex(gridX, gridY);
            if (-1 != oldGridIndex && oldGridIndex == gridIndex)
            {
                return true;
            }

            if (-1 != oldGridIndex)
            {
                lock (this.MyMapGridSpriteItem[oldGridIndex].GridLock)
                {
                    if (!this.MyMapGridSpriteItem[oldGridIndex].ObjsList.Remove(obj))
                    {
                        return false;
                    }

                    this.ChangeMapGridsSpriteNum(oldGridIndex, obj, -1);
                }
            }

            lock (this.MyMapGridSpriteItem[gridIndex].GridLock)
            {
                this.MyMapGridSpriteItem[gridIndex].ObjsList.Add(obj);
                this.ChangeMapGridsSpriteNum(gridIndex, obj, 1);
            }

            this._Obj2GridDict[obj] = gridIndex;

            return true;
        }

        /// <summary>
        /// Xóa đối tượng khỏi lưới
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="obj"></param>
        public bool RemoveObject(IObject obj)
        {
            int oldGridIndex = -1;
            if (!this._Obj2GridDict.TryGetValue(obj, out oldGridIndex))
            {
                oldGridIndex = -1;
            }
            else
            {
                this._Obj2GridDict.TryRemove(obj, out _);
            }

            if (-1 == oldGridIndex) return false;

            lock (this.MyMapGridSpriteItem[oldGridIndex].GridLock)
            {
                if (this.MyMapGridSpriteItem[oldGridIndex].ObjsList.Remove(obj))
                {
                    this.ChangeMapGridsSpriteNum(oldGridIndex, obj, -1);
                }
            }

            return true;
        }

        /// <summary>
        /// Tìm đối tượng tương ứng trong bản đồ tại vị trí tương ứng
        /// </summary>
        /// <param name="gridX">Tọa độ lưới X</param>
        /// <param name="gridY">Tọa độ lưới Y</param>
        public List<Object> FindObjects(int gridX, int gridY)
        {
            int gridIndex = (_MapGridXNum * gridY) + gridX;
            if (gridIndex < 0 || gridIndex >= _MapGridTotalNum)
            {
                return null;
            }

            List<object> listObjs2 = null;

            lock (this.MyMapGridSpriteItem[gridIndex].GridLock)
            {
                listObjs2 = this.MyMapGridSpriteItem[gridIndex].ObjsList;
                if (listObjs2.Count == 0)
                {
                    return null;
                }

                listObjs2 = listObjs2.GetRange(0, listObjs2.Count);
            }

            return listObjs2;
        }

        /// <summary>
        /// Tìm vật phẩm tương ứng tại vị trí
        /// </summary>
        /// <param name="gridX">Tọa độ lưới X</param>
        /// <param name="gridY">Tọa độ lưới Y</param>
        /// <returns></returns>
        public List<Object> FindGoodsPackItems(int gridX, int gridY)
        {
            int gridIndex = (this._MapGridXNum * gridY) + gridX;
            if (gridIndex < 0 || gridIndex >= this._MapGridTotalNum)
            {
                return null;
            }

            List<object> listObjs2 = null;

            lock (this.MyMapGridSpriteItem[gridIndex].GridLock)
            {
                if (this.MyMapGridSpriteItem[gridIndex].GoodsPackNum > 0)
                {
                    listObjs2 = this.MyMapGridSpriteItem[gridIndex].ObjsList.GetRange(0, this.MyMapGridSpriteItem[gridIndex].ObjsList.Count);
                }
            }

            return listObjs2;
        }

        /// <summary>
        /// Tìm đối tượng xung quanh vị trí
        /// </summary>
        /// <param name="toX">Tọa độ thực X</param>
        /// <param name="toY">Tọa độ thực Y</param>
        /// <param name="radius">Phạm vi xung quanh</param>
        /// <param name="sortList">Sắp xếp danh sách theo khoảng cách tăng dần</param>
        /// <returns></returns>
        public List<Object> FindObjects(int toX, int toY, int radius, bool sortList = false)
        {
            if (toX < 0 || toY < 0 || toX >= this.MapWidth || toY >= this.MapHeight)
            {
                return null;
            }

            int gridX = toX / this._MapGridWidth;
            int gridY = toY / this._MapGridHeight;

            List<object> listObjs = new List<Object>();
            List<object> listObjs2 = null;

            int gridRadiusWidthNum = ((radius - 1) / this._MapGridWidth) + 1;
            int gridRadiusHeightNum = ((radius - 1) / this._MapGridHeight) + 1;

            int lowGridY = gridY - gridRadiusHeightNum;
            int hiGridY = gridY + gridRadiusHeightNum;

            int lowGridX = gridX - gridRadiusWidthNum;
            int hiGridX = gridX + gridRadiusWidthNum;

            for (int y = lowGridY; y <= hiGridY; y++)
            {
                for (int x = lowGridX; x <= hiGridX; x++)
                {
                    listObjs2 = this.FindObjects(x, y);
                    if (null != listObjs2)
                    {
                        listObjs.AddRange(listObjs2);
                    }
                }
            }

            /// Nếu có yêu cầu sắp xếp danh sách tăng dần theo khoảng cách
            if (sortList)
            {
                /// Vị trí đang tìm kiếm
                UnityEngine.Vector2 pos = new UnityEngine.Vector2(toX, toY);

                /// Sắp xếp danh sách theo khoảng cách tăng dần
                listObjs.Sort((obj1, obj2) => {
                    IObject o1 = (IObject) obj1;
                    IObject o2 = (IObject) obj2;

                    UnityEngine.Vector2 o1Pos = new UnityEngine.Vector2((int) o1.CurrentPos.X, (int) o1.CurrentPos.Y);
                    UnityEngine.Vector2 o2Pos = new UnityEngine.Vector2((int) o2.CurrentPos.X, (int) o2.CurrentPos.Y);

                    return (int) (UnityEngine.Vector2.Distance(pos, o1Pos) - UnityEngine.Vector2.Distance(pos, o2Pos));
                });
            }

            return listObjs;
        }
    }
}
