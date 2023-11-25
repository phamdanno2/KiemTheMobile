using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Server.TCP;
using Server.Protocol;
using GameServer;
using Server.Data;
using ProtoBuf;
using GameServer.Server;
using Server.Tools;
using System.Xml;
using System.Xml.Linq;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using System.Collections.Concurrent;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý NPC
    /// </summary>
    public class NPCGeneralManager
    {
        /// <summary>
        /// Danh sách NPC
        /// <para>Key: MapCode_CopyMapCode_gridX_gridY</para>
        /// <para>Value: Đối tượng NPC</para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, NPC> listNpc = new ConcurrentDictionary<string, NPC>();

        #region Khởi tạo
        /// <summary>
        /// Tải danh sách NPC trong bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="mapName"></param>
        /// <param name="gameMap"></param>
        public static bool LoadMapNPCs(int mapCode, string mapName, GameMap gameMap)
        {
            string fileName = string.Format("MapConfig/{0}/npcs.xml", mapName);
            XElement xml = GeneralCachingXmlMgr.GetXElement(Global.ResPath(fileName));
            if (null == xml)
            {
                return false;
            }

            IEnumerable<XElement> items = xml.Elements("NPCs").Elements();
            foreach (var item in items)
            {
                NPC myNpc = new NPC();

                myNpc.ResID = Convert.ToInt32((string)item.Attribute("Code"));

                if (!GameManager.SystemNPCsMgr.SystemXmlItemDict.TryGetValue(myNpc.ResID, out SystemXmlItem systemNPC))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find NPC, Code = {0} of MapID = {1}", myNpc.ResID, mapCode));
                    continue;
                }

                myNpc.MapCode = mapCode;
                myNpc.CopyMapID = -1;
                myNpc.CurrentPos = new Point(Convert.ToInt32((string)item.Attribute("X")), Convert.ToInt32((string)item.Attribute("Y")));                

                if (item.Attribute("Dir") != null)
                {
                    myNpc.CurrentDir = (KiemThe.Entities.Direction)Global.GetSafeAttributeLong(item, "Dir");
                }
                else
                {
                    myNpc.CurrentDir = KiemThe.Entities.Direction.DOWN;
                }

                if (item.Attribute("ScriptID") != null)
                {
                    myNpc.ScriptID = int.Parse(item.Attribute("ScriptID").Value);
                }
                else
                {
                    myNpc.ScriptID = -1;
                }

                if (item.Attribute("Name") != null)
                {
                    myNpc.Name = item.Attribute("Name").Value;
                    if (string.IsNullOrEmpty(myNpc.Name))
                    {
                        myNpc.Name = systemNPC.GetStringValue("Name");
                    }
                }
                else
                {
                    myNpc.Name = systemNPC.GetStringValue("Name");
                }

                if (item.Attribute("Title") != null)
                {
                    myNpc.Title = item.Attribute("Title").Value;
                }
                else
                {
                    myNpc.Title = systemNPC.GetStringValue("Title");
                }

                if (item.Attribute("MinimapName") != null)
                {
                    myNpc.MinimapName = item.Attribute("MinimapName").Value;
                }
                else
                {
                    myNpc.MinimapName = myNpc.Name;
                }

                if (item.Attribute("VisibleOnMinimap") != null)
                {
                    myNpc.VisibleOnMinimap = int.Parse(item.Attribute("VisibleOnMinimap").Value) == 1;
                }
                else
                {
                    myNpc.VisibleOnMinimap = false;
                }

                NPCGeneralManager.AddNpcToMap(myNpc);
            }

            return true;
        }

        /// <summary>
        /// Tạo đối tượng NPC theo cấu hình
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        /// <param name="resID"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="dir"></param>
        /// <param name="scriptID"></param>
        /// <param name="tag"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public static bool AddNewNPC(int mapCode, int copyMapID, int resID, int toX, int toY, string name, string title, KiemThe.Entities.Direction dir, int scriptID, string tag, Action<NPC> callBack)
        {
            if (!GameManager.SystemNPCsMgr.SystemXmlItemDict.TryGetValue(resID, out SystemXmlItem systemNPCItem))
            {
                return false;
            }
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out GameMap gameMap))
            {
                return false;
            }

            NPC myNpc = new NPC();
            /// Nếu có thiết lập tên
            if (!string.IsNullOrEmpty(name))
            {
                myNpc.Name = name;
            }
            else
            {
                myNpc.Name = systemNPCItem.GetStringValue("Name");
            }
            /// Nếu có thiết lập danh hiệu
            if (!string.IsNullOrEmpty(title))
            {
                myNpc.Title = title;
            }
            else
            {
                myNpc.Title = systemNPCItem.GetStringValue("Title");
            }
            myNpc.ResID = resID;
            myNpc.CopyMapID = copyMapID;
            myNpc.MapCode = mapCode;
            myNpc.CurrentPos = new Point(toX, toY);
            myNpc.CurrentDir = dir;
            myNpc.ScriptID = scriptID;
            myNpc.Tag = tag;

            /// Thêm NPC vào danh sách quản lý
            NPCGeneralManager.AddNpcToMap(myNpc);

            /// Thực thi sự kiện sau khi tạo NPC hoàn tất
            callBack?.Invoke(myNpc);

            return true; 
        }
        #endregion

        #region Thêm, sửa, xóa
        /// <summary>
        /// Mutex dùng khóa LOCK
        /// </summary>
        public static object mutexAddNPC = new object();

        /// <summary>
        /// Thêm NPC vào bản đồ
        /// </summary>
        /// <param name="myNpc"></param>
        /// <returns></returns>
        public static bool AddNpcToMap(NPC myNpc)
        {
            MapGrid mapGrid;
            lock (GameManager.MapGridMgr.DictGrids)
            {
                mapGrid = GameManager.MapGridMgr.GetMapGrid(myNpc.MapCode);
            }

            if (null == mapGrid)
            {
                return false;
            }

            lock (mutexAddNPC)
            {
                string sNpcKey = String.Format("{0}_{1}_{2}_{3}", myNpc.MapCode, myNpc.CopyMapID, myNpc.GridPoint.X, myNpc.GridPoint.Y);

                /// NPC cũ
                NPC oldNpc = null;

                if (NPCGeneralManager.listNpc.TryGetValue(sNpcKey, out oldNpc))
                {
                    NPCGeneralManager.listNpc.TryRemove(sNpcKey, out _);
                    mapGrid.RemoveObject(oldNpc);
                }

                GameMap gameMap = GameManager.MapMgr.DictMaps[myNpc.MapCode];

                /// Thêm NPC vào Map
                if (mapGrid.MoveObject(-1, -1, (int)(gameMap.MapGridWidth * myNpc.GridPoint.X), (int)(gameMap.MapGridHeight * myNpc.GridPoint.Y), myNpc))
                {
                    NPCGeneralManager.listNpc[sNpcKey] = myNpc;

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Trả về toàn bộ NPC trong bản đồ hoặc phụ bản
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static List<NPC> GetMapNPCs(int mapCode, int copyMapID = -1)
        {
            List<NPC> results = new List<NPC>();

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
            foreach (KeyValuePair<string, NPC> item in NPCGeneralManager.listNpc)
            {
                if (item.Value.MapCode == mapCode && (copyMapID == -1 || item.Value.CopyMapID == copyMapID))
                {
                    results.Add(item.Value);
                }
            }

            /// Trả về kết quả
            return results;
        }

        /// <summary>
        /// Xóa toàn bộ NPC khỏi bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        public static void RemoveMapNpcs(int mapCode, int copyMapID = -1)
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

            List<string> keysToDel = new List<string>();

            /// Duyệt toàn bộ danh sách NPC
            foreach (var item in NPCGeneralManager.listNpc)
            {
                if (item.Value.MapCode == mapCode && (copyMapID == -1 || item.Value.CopyMapID == copyMapID))
                {
                    mapGrid.RemoveObject(item.Value);
                    keysToDel.Add(item.Key);
                }
            }

            /// Xóa các bản ghi đã tìm bên trên
            foreach (var key in keysToDel)
            {
                NPCGeneralManager.listNpc.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// Xóa NPC tương ứng khỏi bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        /// <param name="npcID"></param>
        public static void RemoveMapNpc(int mapCode, int copyMapID, int npcID)
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

            /// Duyệt danh sách NPC trong bản đồ
            foreach (var item in NPCGeneralManager.listNpc)
            {
                if (item.Value.MapCode == mapCode && item.Value.CopyMapID == copyMapID && item.Value.NPCID == npcID)
                {
                    mapGrid.RemoveObject(item.Value);
                    NPCGeneralManager.listNpc.TryRemove(item.Key, out _);

                    return;
                }
            }
        }
        #endregion

        #region Tìm kiếm
        /// <summary>
        /// Tìm đối tượng NPC tương ứng theo ID
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static NPC FindNPC(int mapCode, int copyMapID, int npcID)
        {
            foreach (var item in NPCGeneralManager.listNpc)
            {
                if (item.Value.MapCode == mapCode && item.Value.CopyMapID == copyMapID && item.Value.NPCID == npcID)
                {
                    return item.Value;
                }
            }

            return null;
        }

        /// Tìm đối tượng NPC tương ứng theo ID
        public static NPC FindNPCBYRES(int mapCode, int copyMapID, int npcID)
        {
            foreach (var item in NPCGeneralManager.listNpc)
            {
                if (item.Value.MapCode == mapCode && item.Value.CopyMapID == copyMapID && item.Value.ResID == npcID)
                {
                    return item.Value;
                }
            }

            return null;
        }


        #endregion

        #region Socket
        /// <summary>
        /// Gửi danh sách NPC xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static void SendMySelfNPCs(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;
            NPC npc = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                npc = objsList[i] as NPC;
                if (null == npc)
                {
                    continue;
                }

                if (!npc.ShowNpc)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfNewNPC(sl, pool, client, npc);
            }
        }

        /// <summary>
        /// Xóa NPC khỏi danh sách xung quanh người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static void DelMySelfNpcs(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;
            NPC npc = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                npc = objsList[i] as NPC;
                if (null == npc)
                {
                    continue;
                }

                GameManager.ClientMgr.NotifyMySelfDelNPC(sl, pool, client, npc);
            }
        }
        #endregion
    }
}
