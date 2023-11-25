//#define USE_AS3_COMPAITABLE_ASTAR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FSPlay.Drawing;
using UnityEngine;
using FSPlay.GameEngine.Data;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Teleport;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Control.Map;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Utilities.Algorithms;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Entities.LoadObject;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Factory.ObjectsManager;
using FSPlay.KiemVu.Factory.UIManager;
using UnityEngine.Networking;

namespace FSPlay.GameEngine.Scene
{
    /// <summary>
    /// Quản lý tài nguyên bản đồ
    /// </summary>
    public partial class GScene
    {
        #region Khởi tạo ban đầu
        /// <summary>
        /// Đối tượng chứa toàn bộ thông tin bản đồ
        /// </summary>
        private GameObject Root2DScene;

        /// <summary>
        /// Ký hiệu chọn mục tiêu
        /// </summary>
        private SelectTargetDeco SelectTargetDeco;

        /// <summary>
        /// Ký hiệu vị trí Click di chuyển tới
        /// </summary>
        private Effect ClickMoveDeco;

        /// <summary>
        /// Danh sách điểm truyền tống trong bản đồ
        /// </summary>
        private readonly List<GTeleport> listTeleport = new List<GTeleport>();

        /// <summary>
        /// Hủy tài nguyên bản đồ cũ
        /// </summary>		
        public void ClearScene()
        {
            this.EnableChangMap = false;

            /// Ngừng di chuyển Leader
            if (null != Leader)
            {
                KTLeaderMovingManager.StopMove();
                KTLeaderMovingManager.StopChasingTarget();
            }

            /// Làm rỗng dữ liệu AutoFight
            KTAutoFightManager.Instance.Clear();

            /// Làm mới dữ liệu các Object kỹ năng
            SkillManager.Refresh();

            /// Xóa dữ liệu Leader
            this.Leader = null;

            /// Xóa toàn bộ Storyboard
            KTStoryBoard.Instance.RemoveAllStoryBoards();

            /// Xóa dữ liệu người chơi xung quanh
            Global.Data.OtherRoles.Clear();
            Global.Data.OtherRolesByName.Clear();

            /// Xóa dữ liệu bot xung quanh
            Global.Data.Bots.Clear();

            /// Xóa dữ liệu quái xung quanh
            Global.Data.SystemMonsters.Clear();

            /// Hủy đối tượng tự tìm đường
            this.pathFinderFast = null;

            /// Làm rỗng danh sách cổng dịch chuyển
            if (this.listTeleport != null)
            {
                this.listTeleport.Clear();
            }

            /// Hủy đối tượng ký hiệu chọn mục tiêu
            this.SelectTargetDeco = null;

            /// Hủy đối tượng ký hiệu Click-Move
            this.ClickMoveDeco = null;

            /// Xóa danh sách chờ tải
            this.waitToBeAddedMonster.Clear();
            this.waitToBeAddedRole.Clear();
            this.waitToBeAddedRole.Clear();
            this.waitToBeAddedMonster.Clear();
            this.waitToBeAddedGrowPoint.Clear();
            this.waitToBeAddedGoodsPack.Clear();
            this.waitToBeAddedDynamicArea.Clear();
            this.waitToBeAddedBot.Clear();

            /// Làm rỗng UI
            UIHintItemManager.Instance.Clear();
            UIBottomTextManager.Instance.Clear();
            /// Xóa toàn bộ các bản thể soi trước
            KTRolePreviewManager.Instance.RemoveAllInstances();
            // nếu không phải đang bán đồ thì có thể hủy tự đánh
            if (!KTAutoFightManager.Instance.DoingAutoSell)
            {  /// Hủy tự động đánh
                KTAutoFightManager.Instance.StopAutoFight();

            }

            /// Xóa dữ liệu bản đồ
            if (this.CurrentMapData != null)
			{
                this.CurrentMapData.Dispose();
            }
            this.CurrentMapData = null;
            this.MapCode = -1;

            /// Làm rỗng danh sách đối tượng đang hiển thị
            KTGlobal.RemoveAllObjects();
        }

        /// <summary>
        /// Tải xuống bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="leaderX"></param>
        /// <param name="leaderY"></param>
        /// <param name="direction"></param>		
        public void LoadScene(int mapCode, double leaderX, double leaderY, double direction)
        {
            this.MapCode = mapCode;
            this.EnableChangMap = true;

            this.LoadMapData();
            this.LoadLeader((int)leaderX, (int)leaderY, (int)direction);
        }
        #endregion

        #region Tải xuống dữ liệu bản đồ
        /// <summary>
        /// Tải dữ liệu bản đồ
        /// </summary>
        private void LoadMapData()
        {
            this.Root2DScene = GameObject.Find("Scene 2D Root");
            if (this.Root2DScene == null)
            {
                this.Root2DScene = new GameObject("Scene 2D Root");
                this.Root2DScene.transform.localPosition = Vector3.zero;
            }

            this.SelectTargetDeco = Object2DFactory.MakeSelectTargetDeco();
            this.SelectTargetDeco.gameObject.transform.SetParent(this.Root2DScene.transform, false);
            this.RemoveSelectTarget();

            this.ClickMoveDeco = Object2DFactory.MakeEffect();
            this.ClickMoveDeco.gameObject.transform.SetParent(this.Root2DScene.transform, false);
            this.ClickMoveDeco.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            if (Loader.ListEffects.TryGetValue(75, out KiemVu.Entities.Config.StateEffectXML effectXML))
            {
                KiemVu.Entities.Config.StateEffectXML cloneEffectXML = effectXML.Clone();
                cloneEffectXML.Loop = true;
                this.ClickMoveDeco.Data = cloneEffectXML;
                this.ClickMoveDeco.RefreshAction();
                this.ClickMoveDeco.Play();
            }
            this.RemoveClickMovePos();

            this.CurrentMapData = new GMapData();
            Global.CurrentMapData = this.CurrentMapData;

            this.CurrentMapData.RealMapSize = new Vector2(Loader.Maps[this.MapCode].Width, Loader.Maps[this.MapCode].Height);

            /// Tải xuống Bundle
            AssetBundle assetBundle = this.LoadMapBundle(this.MapCode);
            //foreach (string assetName in assetBundle.GetAllAssetNames())
            //{
            //    KTDebug.LogError("Asset: " + assetName);
            //}
            this.LoadMapSetting(assetBundle);
            this.LoadObstruction(assetBundle);
            this.LoadTeleportList(assetBundle);
            this.LoadMinimapMonsterList(assetBundle);
            this.LoadMinimapNPCList(assetBundle);
            this.LoadMinimapGrowPointList(assetBundle);
            this.LoadMinimapZone(assetBundle);
            /// Giải phóng Bundle
            assetBundle.Unload(true);
            /// Xóa Bundle
            GameObject.Destroy(assetBundle);
        }

        /// <summary>
        /// Tải xuống AssetBundle chứa cấu hình bản đồ tương ứng
        /// </summary>
        private AssetBundle LoadMapBundle(int mapCode)
		{
            /// Thông tin bản đồ tương ứng
            if (Loader.Maps.TryGetValue(mapCode, out KiemVu.Entities.Config.Map mapData))
			{
				/// Đường dẫn File Bundle
				string bundleDir = Global.WebPath(string.Format("Data/MapConfig/{0}.unity3d", mapData.Code));
                /// Tải xuống Bundle
                return ResourceLoader.LoadAssetBundle(bundleDir, true);
			}
			else
			{
                return null;
			}
        }

        /// <summary>
        /// Tải thiết lập bản đồ
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadMapSetting(AssetBundle bundle)
        {
            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            XElement xmlNode = ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].MapSetting);
            mapData.Setting = MapSetting.Parse(xmlNode);
        }

        /// <summary>
        /// Tải danh sách Monster hiện trên bản đồ nhỏ từ bundle
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadMinimapGrowPointList(AssetBundle bundle)
        {
            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            /// Nếu không có file điểm thu thập
            if (string.IsNullOrEmpty(Loader.Maps[mapId].GrowPointList))
            {
                return;
            }

            XElement xmlNode = FSPlay.KiemVu.Loader.ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].GrowPointList);

            List<KeyValuePair<string, Point>> growPointList = new List<KeyValuePair<string, Point>>();
            Dictionary<int, Point> growPointListByID = new Dictionary<int, Point>();
            foreach (XElement node in xmlNode.Element("GrowPoints").Elements("GrowPoint"))
            {
                /// ID tĩnh của điểm thu thập
                int staticID = int.Parse(node.Attribute("Code").Value);

                string name = node.Attribute("Name").Value;
                if (string.IsNullOrEmpty(name) && Loader.ListMonsters.TryGetValue(staticID, out MonsterDataXML monsterData))
                {
                    name = monsterData.Name;
                }
                int posX = int.Parse(node.Attribute("PosX").Value);
                int posY = int.Parse(node.Attribute("PosY").Value);
                growPointList.Add(new KeyValuePair<string, Point>(name, new Point(posX, posY)));
                growPointListByID[staticID] = new Point(posX, posY);
            }
            mapData.GrowPointList = growPointList;
            mapData.GrowPointListByID = growPointListByID;
        }

        /// <summary>
        /// Tải danh sách điểm truyền tống gợi ý trên bản đồ nhỏ từ bundle
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadTeleportList(AssetBundle bundle)
        {
            this.listTeleport.Clear();

            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            XElement xmlNode = FSPlay.KiemVu.Loader.ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].Teleport);

            List<KeyValuePair<string, Point>> teleportList = new List<KeyValuePair<string, Point>>();
            foreach (XElement node in xmlNode.Element("Teleports").Elements("Teleport"))
            {
                int id = int.Parse(node.Attribute("Code").Value);
                string name = node.Attribute("Tip").Value;
                int posX = int.Parse(node.Attribute("X").Value);
                int posY = int.Parse(node.Attribute("Y").Value);
                int toMapCode = int.Parse(node.Attribute("To").Value);
                teleportList.Add(new KeyValuePair<string, Point>(name, new Point(posX, posY)));

                GTeleport teleport = Global.GetTeleport(node);
                teleport.SpriteType = GSpriteTypes.Teleport;
                teleport.BaseID = SpriteBaseIds.TeleportBaseId + id;
                if (Loader.Maps.TryGetValue(toMapCode, out KiemVu.Entities.Config.Map staticMapData))
                {
                    teleport.ToLevel = staticMapData.Level;
                    string mapTypeString = staticMapData.MapType;
                    switch (mapTypeString)
                    {
                        case "village":
                            teleport.ToType = KiemVu.Entities.Enum.MapType.Village;
                            break;
                        case "city":
                            teleport.ToType = KiemVu.Entities.Enum.MapType.City;
                            break;
                        case "faction":
                            teleport.ToType = KiemVu.Entities.Enum.MapType.Faction;
                            break;
                        case "fight":
                            teleport.ToType = KiemVu.Entities.Enum.MapType.Fight;
                            break;
                    }
                    this.Add(teleport);
                    this.listTeleport.Add(teleport);
                    teleport.Start();
                }
                else
                {
                    KTDebug.LogError("TOANG TELEPORT ~~~ Code = " + id);
                }
            }
            mapData.Teleport = teleportList;
        }

        /// <summary>
        /// Tải danh sách Monster hiện trên bản đồ nhỏ từ bundle
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadMinimapMonsterList(AssetBundle bundle)
        {
            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            XElement xmlNode = FSPlay.KiemVu.Loader.ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].MonsterList);

            List<KeyValuePair<string, Point>> monsterList = new List<KeyValuePair<string, Point>>();
            Dictionary<int, Point> monsterListByID = new Dictionary<int, Point>();
            foreach (XElement node in xmlNode.Element("Monsters").Elements("Monster"))
            {
                /// ID tĩnh của quái
                int staticID = int.Parse(node.Attribute("Code").Value);

                string name = "";
                if (Loader.ListMonsters.TryGetValue(staticID, out KiemVu.Entities.Config.MonsterDataXML monsterData))
                {
                    name = monsterData.Name;
                }
                int posX = int.Parse(node.Attribute("X").Value);
                int posY = int.Parse(node.Attribute("Y").Value);
                monsterList.Add(new KeyValuePair<string, Point>(name, new Point(posX, posY)));
                monsterListByID[staticID] = new Point(posX, posY);
            }
            mapData.MonsterList = monsterList;
            mapData.MonsterListByID = monsterListByID;
        }

        /// <summary>
        /// Tải danh sách Npc hiện trên bản đồ nhỏ từ bundle
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadMinimapNPCList(AssetBundle bundle)
        {
            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            XElement xmlNode = FSPlay.KiemVu.Loader.ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].NPCList);

            List<KeyValuePair<string, Point>> npcList = new List<KeyValuePair<string, Point>>();
            Dictionary<int, Point> npcListByID = new Dictionary<int, Point>();
            foreach (XElement node in xmlNode.Element("NPCs").Elements("NPC"))
            {
                bool visibleOnMinimap = false;
                if (node.Attribute("VisibleOnMinimap") != null)
                {
                    visibleOnMinimap = int.Parse(node.Attribute("VisibleOnMinimap").Value) == 1;
                }

                /// Nếu không cho phép hiển thị ở Minimap
                if (!visibleOnMinimap)
                {
                    continue;
                }

                /// ID tĩnh của NPC
                int staticID = int.Parse(node.Attribute("Code").Value);

                string name = "";
                if (node.Attribute("Name") != null)
                {
                    name = node.Attribute("Name").Value;
                    if (node.Attribute("MinimapName") != null)
                    {
                        string mName = node.Attribute("MinimapName").Value;
                        if (!string.IsNullOrEmpty(mName))
                        {
                            name = mName;
                        }
                    }
                }
                else if (Loader.ListMonsters.TryGetValue(staticID, out KiemVu.Entities.Config.MonsterDataXML monsterData))
                {
                    name = monsterData.Name;
                }
                int posX = int.Parse(node.Attribute("X").Value);
                int posY = int.Parse(node.Attribute("Y").Value);
                npcList.Add(new KeyValuePair<string, Point>(name, new Point(posX, posY)));
                npcListByID[staticID] = new Point(posX, posY);
            }
            mapData.NpcList = npcList;
            mapData.NpcListByID = npcListByID;
        }

        /// <summary>
        /// Tải danh sách các vùng hiện trên bản đồ nhỏ từ bundle
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadMinimapZone(AssetBundle bundle)
        {
            GMapData mapData = this.CurrentMapData;
            int mapId = this.MapCode;

            XElement xmlNode = FSPlay.KiemVu.Loader.ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].Zone);

            List<KeyValuePair<string, Point>> zone = new List<KeyValuePair<string, Point>>();
            if (xmlNode != null)
            {
                foreach (XElement node in xmlNode.Element("Zones").Elements("Zone"))
                {
                    string name = node.Attribute("Name").Value;
                    int posX = int.Parse(node.Attribute("X").Value);
                    int posY = int.Parse(node.Attribute("Y").Value);
                    zone.Add(new KeyValuePair<string, Point>(name, new Point(posX, posY)));
                }
            }
            mapData.Zone = zone;
        }

        /// <summary>
        /// Đọc dữ liệu các ô vật cản
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadObstruction(AssetBundle bundle)
        {
            int mapId = this.MapCode;

            XElement xml = ResourceLoader.LoadXMLFromBundle(bundle, Loader.Maps[mapId].Obstruction);

            this.CurrentMapData.MapWidth = int.Parse(xml.Attribute("MapWidth").Value);
            this.CurrentMapData.MapHeight = int.Parse(xml.Attribute("MapHeight").Value);

            this.CurrentMapData.GridSizeX = 20;
            this.CurrentMapData.GridSizeY = 20;

            this.CurrentMapData.OriginGridSizeXNum = int.Parse(xml.Attribute("OriginGridSizeXNum").Value);
            this.CurrentMapData.OriginGridSizeYNum = int.Parse(xml.Attribute("OriginGridSizeYNum").Value);

            int wGridsNum = (this.CurrentMapData.MapWidth - 1) / this.CurrentMapData.GridSizeX + 1;
            int hGridsNum = (this.CurrentMapData.MapHeight - 1) / this.CurrentMapData.GridSizeY + 1;

            wGridsNum = (int)Math.Ceiling(Math.Log(wGridsNum, 2));
            wGridsNum = (int)Math.Pow(2, wGridsNum);

            hGridsNum = (int)Math.Ceiling(Math.Log(hGridsNum, 2));
            hGridsNum = (int)Math.Pow(2, hGridsNum);

            this.CurrentMapData.GridSizeXNum = wGridsNum;
            this.CurrentMapData.GridSizeYNum = hGridsNum;

            this.CurrentMapData.Obstructions = new byte[wGridsNum, hGridsNum];
            this.CurrentMapData.BlurPositions = new byte[wGridsNum, hGridsNum];

            byte[] obsBytes = ResourceLoader.LoadBytesFromBundle(bundle, "Obs.txt");
            byte[] blurBytes = ResourceLoader.LoadBytesFromBundle(bundle, "Blur.txt");

            this.CurrentMapData.Obstructions.FromBytes<byte>(obsBytes);
            this.CurrentMapData.BlurPositions.FromBytes<byte>(blurBytes);

            /// Dọn rác
            obsBytes = null;
            blurBytes = null;
        }
        #endregion
    }
}
