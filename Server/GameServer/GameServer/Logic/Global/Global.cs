#define OldGrid2
#define TestGrid

using GameServer.Core.Executor;

using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.Interface;
using GameServer.KiemThe;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;

using GameServer.Logic.CheatGuard;
using GameServer.Server;
using HSGameEngine.Tools.AStarEx;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Linq;

using Tmsk.Contract;

namespace GameServer.Logic
{
    public class Global
    {
        static Global()
        {
            XmlInfo = new Dictionary<string, XElement>();
        }

        #region 静态变量

        /// <summary>
        /// Có mở giao dịch không
        /// </summary>
        public const bool Flag_MUSale = true;

        /// <summary>
        /// Có mở name server không
        /// </summary>
        public static bool Flag_NameServer = false;

        /// <summary>
        /// Số đồng khóa được mang theo tối đa
        /// </summary>
        public const int Max_Role_BoundToken = 1000000000;

        /// <summary>
        /// Số bạc được mang theo tối đa
        /// </summary>
        public const int Max_Role_Money = 1000000000;

        /// <summary>
        /// 代码检查
        /// </summary>
        public static void CheckCodes()
        {
            bool result = true;

            result &= CheckDuplicateEnum(typeof(SceneUIClasses));

            result &= CheckDuplicateEnum(typeof(ActivityTypes));
            result &= CheckDuplicateFieldValue(typeof(RoleParamName));
        }

        public static string[] DontCheckEnumNames = new string[] { "Max", "Max_Configed", };

        /// <summary>
        /// Check xem enum có bị dupper ko
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckDuplicateEnum(Type type)
        {
            bool result = true;
            Array array;
            HashSet<int> hashSet = new HashSet<int>();

            array = type.GetEnumValues();
            foreach (var v0 in array)
            {
                int v = (int)v0;
                if (!hashSet.Add(v) && !DontCheckEnumNames.Contains(v0.ToString()))
                {
                    LogManager.WriteLog(LogTypes.Fatal, string.Format("Dupper [{0}][{1}] Please check and remove ", type.ToString(), v));
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Check duper gái trị của field
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckDuplicateFieldValue(Type type)
        {
            bool result = true;
            HashSet<string> hashSet = new HashSet<string>();

            FieldInfo[] fields = type.GetFields();
            foreach (var field in fields)
            {
                string v = field.GetValue(null).ToString();
                if (!hashSet.Add(v))
                {
                    LogManager.WriteLog(LogTypes.Fatal, string.Format("类型[{0}]定义常量值[{1}]重复", type.ToString(), v));
                    result = false;
                }
            }

            return result;
        }

        #endregion 静态变量

        #region Tầng mạng

        /// <summary>
        /// _TCPManager
        /// </summary>
        public static TCPManager _TCPManager = null;

        /// <summary>
        /// Quản lý việc gửi gói tin
        /// </summary>
        public static SendBufferManager _SendBufferManager = null;

        /// <summary>
        /// QUản lý bộ nhớ
        /// </summary>
        public static MemoryManager _MemoryManager = null;

        /// <summary>
        /// Quản lý ghi gửi nhận phụ bản
        /// </summary>
        public static FullBufferManager _FullBufferManager = null;

        #endregion Tầng mạng

        /// <summary>
        /// Quản lsy XML
        /// </summary>
        public static Dictionary<string, XElement> XmlInfo;

        #region Cầu hình tài nguyên

        /// <summary>
        /// 游戏资源的存放位置
        /// </summary>
        public static string AbsoluteGameResPath = "";

        private static int ConfigPathStructType = 0;

        /// <summary>
        /// Kiểm tra các folder thiết lập cấu hình có hợp lệ không
        /// </summary>
        public static void CheckConfigPathType()
        {
            ConfigPathStructType = 1;
            if (Directory.Exists(Global.GameResPath("")) && Directory.Exists(Global.ResPath("MapConfig")))
            {
                return;
            }

            ConfigPathStructType = 0;
            if (Directory.Exists(Global.GameResPath("")) && Directory.Exists(Global.ResPath("MapConfig")))
            {
                return;
            }

            LogManager.WriteLog(LogTypes.Fatal, "Config data structure is not correct!");
        }

        /// <summary>
        /// Đường dẫn folder Config
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GameResPath(string uri)
        {
            if (ConfigPathStructType == 1)
            {
                return string.Format("{0}/{1}", Global.AbsoluteGameResPath, uri);
            }
            else
            {
                return string.Format("{0}/{1}", Global.AbsoluteGameResPath, uri);
            }
        }

        /// <summary>
        /// Đường dẫn Res thường (MapConfig,...)
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ResPath(string uri)
        {
            if (ConfigPathStructType == 1)
            {
                return string.Format("{0}/{1}", Global.AbsoluteGameResPath, uri);
            }
            else
            {
                return string.Format("{0}/{1}", Global.AbsoluteGameResPath, uri);
            }
        }

        public static string IsolateResPath(string uri)
        {
            if (ConfigPathStructType == 1)
            {
                return string.Format("{0}/ServerRes/{1}/IsolateRes/{2}", Global.AbsoluteGameResPath, /*GameManager.ServerLineID*/1, uri);
            }
            else
            {
                return string.Format("{0}/GameRes/IsolateRes/{1}", Global.AbsoluteGameResPath, uri);
            }
        }

        #endregion Cầu hình tài nguyên

        #region Xử lý XMl

        /// <summary>
        /// 从游戏资源获取XML
        /// </summary>
        /// <param name="uri">XML文件的相对的路径</param>
        /// <returns>XML对象</returns>
        public static XElement GetGameResXml(string uri)
        {
            return XElement.Load(GameResPath(uri));
        }

        public static XElement GetResXml(string uri)
        {
            return XElement.Load(ResPath(uri));
        }

        /// <summary>
        /// 获取指定的xml节点的节点路径
        /// </summary>
        /// <param name="element"></param>
        public static string GetXElementNodePath(XElement element)
        {
            try
            {
                string path = element.Name.ToString();
                element = element.Parent;
                while (null != element)
                {
                    path = element.Name.ToString() + "/" + path;
                    element = element.Parent;
                }

                return path;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static XElement GetSafeXElement(XElement XML, string newroot, string attribute, string value)
        {
            try
            {
                return XML.DescendantsAndSelf(newroot).Single(X => X.Attribute(attribute).Value == value);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Read: {0}/{1}={2} failed, Error: {3} ", newroot, attribute, value, GetXElementNodePath(XML)) + ex.Message);
            }
        }

        public static XAttribute GetSafeAttribute(XElement XML, string attribute)
        {
            try
            {
                XAttribute attrib = XML.Attribute(attribute);
                if (null == attrib)
                {
                    throw new Exception(string.Format("Get attribute failed: {0}, Error: {1}", attribute, GetXElementNodePath(XML)));
                }

                return attrib;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Get attribute: {0} failed: {0}, Error: {1}", attribute, GetXElementNodePath(XML)));
            }
        }

        public static XAttribute GetSafeAttributeAppectNull(XElement XML, string attribute)
        {
            try
            {
                XAttribute attrib = XML.Attribute(attribute);
                if (null == attrib)
                {
                    return null;
                }

                return attrib;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetSafeAttributeStr(XElement XML, string attribute)
        {
            XAttribute attrib = GetSafeAttribute(XML, attribute);
            return (string)attrib;
        }

        public static string GetDefAttributeStr(XElement XML, string attribute, string strdef)
        {
            XAttribute attrib = XML.Attribute(attribute);
            if (null == attrib)
                return strdef;

            return (string)attrib;
        }

        public static long GetSafeAttributeLong(XElement XML, string attribute)
        {
            XAttribute attrib = GetSafeAttribute(XML, attribute);
            string str = (string)attrib;
            if (null == str || str == "") return -1;

            try
            {
                return (long)Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "EX :" + ex.ToString());

                return 0;
            }
        }

        public static long GetSafeAttributeLongWithNull(XElement XML, string attribute)
        {
            XAttribute attrib = GetSafeAttributeAppectNull(XML, attribute);
            if (attrib == null)
            {
                return -1;
            }

            string str = (string)attrib;
            if (null == str || str == "") return -1;

            try
            {
                return (long)Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "EX :" + ex.ToString());

                return 0;
            }
        }

        public static double GetSafeAttributeDouble(XElement XML, string attribute)
        {
            XAttribute attrib = GetSafeAttribute(XML, attribute);
            string str = (string)attrib;
            if (null == str || str == "") return 0.0;

            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Read attribute: {0} failed: {0}, Error: {1}", attribute, GetXElementNodePath(XML)));
            }
        }

        public static XAttribute GetSafeAttribute(XElement XML, string root, string attribute)
        {
            try
            {
                XAttribute attrib = XML.Element(root).Attribute(attribute);
                if (null == attrib)
                {
                    throw new Exception(string.Format("Read attribute: {0}/{1} failed: {0}, Error: {2}", root, attribute, GetXElementNodePath(XML)));
                }

                return attrib;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Read attribute: {0}/{1} failed: {0}, Error: {2}", root, attribute, GetXElementNodePath(XML)));
            }
        }

        public static string GetSafeAttributeStr(XElement XML, string root, string attribute)
        {
            XAttribute attrib = GetSafeAttribute(XML, root, attribute);
            return (string)attrib;
        }

        public static long GetSafeAttributeLong(XElement XML, string root, string attribute)
        {
            XAttribute attrib = GetSafeAttribute(XML, root, attribute);
            string str = (string)attrib;
            if (null == str || str == "") return -1;

            try
            {
                return (long)Convert.ToDouble(str);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Read attribute: {0}/{1} failed: {0}, Error: {2}", root, attribute, GetXElementNodePath(XML)));
            }
        }

        #endregion Xử lý XMl

        #region Xử lý lượng giác

        /// <summary>
        /// Có nằm trong bán kính hay không
        /// </summary>
        /// <param name="target">目标点坐标</param>
        /// <param name="center">圆心坐标</param>
        /// <param name="radius">圆半径</param>
        /// <returns></returns>
        public static bool InCircle(Point target, Point center, double radius)
        {
            double lenght1 = Math.Pow(target.X - center.X, 2) + Math.Pow(target.Y - center.Y, 2);
            double lenght2 = Math.Pow(radius, 2);
            return lenght1 <= lenght2 ? true : false;
        }

        /// <summary>
        /// Lấy khoảng cách giữa 2 điểm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetTwoPointDistance(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2));
        }

        /// <summary>
        /// Giải thuật Bresenham tìm tập hợp các điểm tạo thành đường thẳng tương ứng
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static bool Bresenham(List<ANode> s, int x1, int y1, int x2, int y2, NodeGrid nodeGrid)
        {
            int t, x, y, dx, dy, error;
            bool flag = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (flag)
            {
                t = x1; x1 = y1; y1 = t;
                t = x2; x2 = y2; y2 = t;
            }

            bool reverse = false;
            if (x1 > x2)
            {
                t = x1; x1 = x2; x2 = t;
                t = y1; y1 = y2; y2 = t;
                reverse = true;
            }
            dx = x2 - x1;
            dy = Math.Abs(y2 - y1);
            error = dx / 2;
            for (x = x1, y = y1; x <= x2; ++x)
            {
                if (flag)
                {
                    if (null != s)
                    {
                        s.Add(new ANode(y, x));
                    }
                }
                else
                {
                    if (null != s)
                    {
                        s.Add(new ANode(x, y));
                    }
                }

                error -= dy;
                if (error < 0)
                {
                    if (y1 < y2)
                        ++y;
                    else
                        --y;
                    error += dx;
                }
            }

            if (reverse)
            {
                s.Reverse();
            }

            List<ANode> s1 = GetLinearPath(s, nodeGrid);
            bool res = (s1.Count == s.Count);

            s.Clear();
            for (int i = 0; i < s1.Count; i++)
            {
                s.Add(s1[i]);
            }

            return res;
        }

        /// <summary>
        /// Tìm ô gần nhất với vị trí đích không chứa vật cản
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool FindLinearNoObsPoint(GameMap gameMap, Point p0, Point p1, out Point point)
        {
            point = new Point(0, 0);
            List<ANode> path = new List<ANode>();
            Global.Bresenham(path, (int)p0.X, (int)p0.Y, (int)p1.X, (int)p1.Y, gameMap.MyNodeGrid);
            if (path.Count > 1)
            {
                point = new Point(path[path.Count - 1].x, path[path.Count - 1].y);
                path.Clear();
                return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static List<ANode> GetLinearPath(List<ANode> s, NodeGrid nodeGrid)
        {
            List<ANode> s1 = new List<ANode>();
            for (int i = 0; i < s.Count; i++)
            {
                //不可移动点
                if (!nodeGrid.isWalkable(s[i].x, s[i].y))
                {
                    break;
                }

                s1.Add(s[i]);
            }

            return s1;
        }

        #endregion Xử lý lượng giác

        #region Thao tác với CSDL

        /// <summary>
        /// Lấy 1 dữ lệu người chơi từ DB ra
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static KPlayer GetSafeClientDataFromLocalOrDB(int otherRoleID)
        {
            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);

            if (null == otherClient)
            {
                byte[] bytesData = null;
                if (TCPProcessCmdResults.RESULT_FAILED != Global.RequestToDBServer3(TCPClientPool.getInstance(), TCPOutPacketPool.getInstance(), (int)TCPGameServerCmds.CMD_SPR_GETOTHERATTRIB2, string.Format("{0}:{1}", -1, otherRoleID), out bytesData, GameManager.LocalServerId))
                {
                    Int32 length = BitConverter.ToInt32(bytesData, 0);

                    RoleDataEx roleDataEx = DataHelper.BytesToObject<RoleDataEx>(bytesData, 6, length - 2);
                    if (roleDataEx.RoleID < 0)
                    {
                        return null;
                    }

                    otherClient.RoleData = roleDataEx;
                }
            }

            return otherClient;
        }

        /// <summary>
        /// Trả về dữ liệu trang bị nhân vật Mini, dùng trong hiển thị nhân vật khác
        /// </summary>
        /// <param name="client"></param>
        /// <returns>ID áo, ID mũ, ID vũ khí, cấp cường hóa vũ khí, ngũ hành vũ khí, ID phi phong, ID ngựa</returns>
        public static Tuple<int, int, int, int, int, int, int> GetRoleEquipDataMini(KPlayer client)
        {
            int armorID = -1, helmID = -1, weaponID = -1, weaponEnhanceLevel = 0, weaponSeries = 0, mantleID = -1, horseID = -1;
            if (client.GoodsDataList != null)
            {
                bool isRiding = false;
                List<GoodsData> equips;
                lock (client.GoodsDataList)
                {
                    equips = client.GoodsDataList.Where(x => x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_BODY || x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_HEAD || x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_WEAPON || x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_MANTLE || x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_HORSE).ToList();
                }

                int count = equips.Count;
                for (int i = 0; i < count; i++)
                {
                    GoodsData itemGD = equips[i];
                    switch (itemGD.Using)
                    {
                        case (int)KE_EQUIP_POSITION.emEQUIPPOS_BODY:
                            {
                                armorID = itemGD.GoodsID;
                                break;
                            }
                        case (int)KE_EQUIP_POSITION.emEQUIPPOS_HEAD:
                            {
                                helmID = itemGD.GoodsID;
                                break;
                            }
                        case (int)KE_EQUIP_POSITION.emEQUIPPOS_WEAPON:
                            {
                                weaponID = itemGD.GoodsID;
                                weaponEnhanceLevel = itemGD.Forge_level;
                                weaponSeries = itemGD.Series;
                                break;
                            }
                        case (int)KE_EQUIP_POSITION.emEQUIPPOS_MANTLE:
                            {
                                mantleID = itemGD.GoodsID;
                                break;
                            }
                        case (int)KE_EQUIP_POSITION.emEQUIPPOS_HORSE:
                            {
                                horseID = itemGD.GoodsID;
                                break;
                            }
                    }
                }
            }

            return new Tuple<int, int, int, int, int, int, int>(armorID, helmID, weaponID, weaponEnhanceLevel, weaponSeries, mantleID, horseID);
        }

        /// <summary>
        /// Chuyển dữ liệu nhân vật thành đối tượng RoleDataMini
        /// </summary>
        /// <param name="roleDataEx"></param>
        /// <param name="includeBuffData"></param>
        /// <returns></returns>
        public static RoleDataMini ClientToRoleDataMini(KPlayer client, bool includeBuffData = true)
        {
            Tuple<int, int, int, int, int, int, int> tuple = Global.GetRoleEquipDataMini(client);
            int armorID = tuple.Item1;
            int helmID = tuple.Item2;
            int weaponID = tuple.Item3;
            int weaponEnhanceLevel = tuple.Item4;
            int weaponSeries = tuple.Item5;
            int mantleID = tuple.Item6;
            int horseID = tuple.Item7;
            bool isRiding = client.IsRiding;

            RoleDataMini roleData = new RoleDataMini()
            {
                ZoneID = client.ZoneID,
                RoleID = client.RoleID,
                RoleName = client.RoleName,
                CurrentDir = (int)client.CurrentDir,
                HP = client.m_CurrentLife,
                MaxHP = client.m_CurrentLifeMax,
                FactionID = client.m_cPlayerFaction.GetFactionId(),
                RouteID = client.m_cPlayerFaction.GetRouteId(),
                Level = client.m_Level,
                PosX = (int)client.CurrentPos.X,
                PosY = (int)client.CurrentPos.Y,
                RoleSex = client.RoleSex,
                BufferDataList = includeBuffData ? client.Buffs.ToBufferData() : null,
                MoveSpeed = client.GetCurrentRunSpeed(),
                AttackSpeed = client.GetCurrentAttackSpeed(),
                CastSpeed = client.GetCurrentCastSpeed(),

                ArmorID = armorID,
                HelmID = helmID,
                WeaponID = weaponID,
                HorseID = horseID,
                IsRiding = isRiding,
                MantleID = mantleID,
                WeaponEnhanceLevel = weaponEnhanceLevel,
                WeaponSeries = weaponSeries,
                AvartaID = client.RolePic,

                MapCode = client.CurrentMapCode,
                TeamLeaderID = client.TeamLeader != null ? client.TeamLeader.RoleID : -1,
                TeamID = client.TeamID,

                PKMode = client.PKMode,
                PKValue = client.PKValue,
                Camp = client.Camp,

                StallName = client.StallDataItem == null ? "" : client.StallDataItem.StallName,

                Title = string.IsNullOrEmpty(client.TempTitle) ? client.Title : client.TempTitle,
                GuildTitle = !string.IsNullOrEmpty(client.GuildTitle) ? client.GuildTitle : client.FamilyTitle,

                GuildID = client.GuildID,
                GuildName = client.GuildName,
                GuildRank = client.GuildRank,
                OfficeRank = client.OfficeRank,

                FamilyID = client.FamilyID,
                FamilyName = client.FamilyName,
                FamilyRank = client.FamilyRank,

                TotalValue = client.GetTotalValue(),

                SelfCurrentTitleID = client.CurrentRoleTitleID,
            };
            return roleData;
        }

        #endregion Thao tác với CSDL

        #region Bản đồ và vị trí

        /// <summary>
        /// 强制将点校正为格子的中心点
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="p"></param>
        public static Point ForceCorrectPoint(int mapCode, Point p)
        {
            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out gameMap))
            {
                return p;
            }

            return new Point((int)(p.X / gameMap.MapGridWidth) * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, (int)(p.Y / gameMap.MapGridHeight) * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
        }

        /// <summary>
        /// 获取一个指定中心点半径内的空闲点
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point GetMapPoint(ObjectTypes objType, int mapCode, int toX, int toY, int radius)
        {
            Point p = new Point(toX, toY);
            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out gameMap))
            {
                return p;
            }

            int minX = Math.Max(0, toX - radius);
            int maxX = Math.Min(gameMap.MapWidth - 1, toX + radius);
            int minY = Math.Max(0, toY - radius);
            int maxY = Math.Min(gameMap.MapHeight - 1, toY + radius);

            Point randPoint = new Point(Global.GetRandomNumber(minX, maxX), Global.GetRandomNumber(minY, maxY));
            if (!Global.InObs(objType, mapCode, (int)randPoint.X, (int)randPoint.Y))
            {
                return Global.ForceCorrectPoint(mapCode, randPoint);
            }

            Point gridPoint = new Point((int)(randPoint.X / gameMap.MapGridWidth), (int)(randPoint.Y / gameMap.MapGridHeight));

            //从距离一个格子的4个方向中，选择一个没有障碍物的格子
            gridPoint = Global.GetAGridPointIn4Direction(objType, gridPoint, mapCode);

            // 再做一次保障判断 [5/7/2014 LiaoWei]
            if (Global.InObsByGridXY(objType, mapCode, (int)gridPoint.X, (int)gridPoint.Y))
            {
                return p;
            }

            return new Point(gridPoint.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, gridPoint.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
        }

        /// <summary>
        /// 获取一个指定中心点半径内的空闲点
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point GetMapPointByGridXY(ObjectTypes objType, int mapCode, int gridX, int gridY, int radiusNum, int holdGridNum = 0, bool bCanNotInSafeArea = false)
        {
            Point p = new Point(gridX, gridY);
            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out gameMap))
            {
                return new Point(p.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, p.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            }

            int minX = Math.Max(0, gridX - radiusNum);
            int maxX = Math.Min(gameMap.MapGridColsNum - 1, gridX + radiusNum);
            int minY = Math.Max(0, gridY - radiusNum);
            int maxY = Math.Min(gameMap.MapGridRowsNum - 1, gridY + radiusNum);

            Point randPoint = new Point(Global.GetRandomNumber(minX, maxX), Global.GetRandomNumber(minY, maxY));
            if (!Global.InObsByGridXY(objType, mapCode, (int)randPoint.X, (int)randPoint.Y, holdGridNum))// && !gameMap.InSafeRegionList((int)randPoint.X, (int)randPoint.Y))  // 是否在安全区判断 [4/14/2014 LiaoWei]
            {
                return new Point(randPoint.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, randPoint.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            }

            //return new Point(gridX * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, gridY * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);

            Point gridPoint = new Point((int)randPoint.X, (int)randPoint.Y);

            //从距离一个格子的4个方向中，选择一个没有障碍物的格子
            gridPoint = Global.GetAGridPointIn4Direction(objType, gridPoint, mapCode, holdGridNum, false);
            if (Global.InObsByGridXY(objType, mapCode, (int)gridPoint.X, (int)gridPoint.Y, holdGridNum))// && !gameMap.InSafeRegionList((int)randPoint.X, (int)randPoint.Y))  // 是否在安全区判断 [4/14/2014 LiaoWei]
            {
                return new Point(gridX * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, gridY * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            }

            return new Point(gridPoint.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, gridPoint.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
        }

        /// <summary>
        /// Trả về điểm không có vật cản gần nhất quanh điểm cho trước
        /// </summary>
        /// <param name="p"></param>
        /// <param name="obs"></param>
        /// <returns></returns>
        public static Point GetAGridPointIn4Direction(ObjectTypes objType, Point gridPoint, int mapCode, int holdGridNum = 0, bool bCanNotInSafeArea = false)   // 增加一个参数--是否不能在安全区内 [4/14/2014 LiaoWei]
        {
            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out gameMap))
            {
                return gridPoint;
            }

            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];

            int gridX = (int)gridPoint.X;
            int gridY = (int)gridPoint.Y;
            if (gameMap.MyNodeGrid.isWalkable(gridX, gridY))
            {
                return gridPoint;
            }

            Point p = gridPoint;
            int maxGridX = gameMap.MapGridColsNum - 1;
            int maxGridY = gameMap.MapGridRowsNum - 1;
            int added = 1, newX1 = 0, newY1 = 0, newX2 = 0, newY2 = 0;
            while (true)
            {
                newX1 = gridX + added;
                newY1 = gridY + added;
                newX2 = gridX - added;
                newY2 = gridY - added;

                int total = 8;

                if ((0 <= newX1 && newX1 < maxGridX) && (0 <= newY1 && newY1 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX1, newY1))
                    {
                        p = new Point(newX1, newY1);
                        break;
                    }
                }

                if ((0 <= newX1 && newX1 < maxGridX) && (0 <= newY2 && newY2 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX1, newY2))
                    {
                        p = new Point(newX1, newY2);
                        break;
                    }
                }

                if ((0 <= newX2 && newX2 < maxGridX) && (0 <= newY1 && newY1 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX2, newY1))
                    {
                        p = new Point(newX2, newY1);
                        break;
                    }
                }

                if ((0 <= newX2 && newX2 < maxGridX) && (0 <= newY2 && newY2 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX2, newY2))
                    {
                        p = new Point(newX2, newY2);
                        break;
                    }
                }

                if ((0 <= newX1 && newX1 < maxGridX))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX1, gridY))
                    {
                        p = new Point(newX1, gridY);
                        break;
                    }
                }

                if ((0 <= newY1 && newY1 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(gridX, newY1))
                    {
                        p = new Point(gridX, newY1);
                        break;
                    }
                }

                if ((0 <= newX2 && newX2 < maxGridX))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(newX2, gridY))
                    {
                        p = new Point(newX2, gridY);
                        break;
                    }
                }

                if ((0 <= newY2 && newY2 < maxGridY))
                {
                    total--;
                    if (gameMap.MyNodeGrid.isWalkable(gridX, newY2))
                    {
                        p = new Point(gridX, newY2);
                        break;
                    }
                }

                if (total >= 8)
                {
                    break;
                }

                added++;
            }

            return p;
        }

        /// <summary>
        /// 是否不小心掉到了障碍物中
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        public static bool InOnlyObs(ObjectTypes objType, int mapCode, int gridX, int gridY)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (gridX >= gameMap.MapGridColsNum || gridX < 0 || gridY >= gameMap.MapGridRowsNum || gridY < 0)
            {
                return true;
            }

            if (!gameMap.MyNodeGrid.isWalkable(gridX, gridY)) //障碍物
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否不小心掉到了障碍物中
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        public static bool InOnlyObsByXY(ObjectTypes objType, int mapCode, int toX, int toY)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (toX >= gameMap.MapWidth || toX < 0 || toY >= gameMap.MapHeight || toY < 0)
            {
                return true;
            }

            int gridX = (int)(toX / gameMap.MapGridWidth);
            int gridY = (int)(toY / gameMap.MapGridHeight);
            if (gridX >= gameMap.MapGridColsNum || gridX < 0 || gridY >= gameMap.MapGridRowsNum || gridY < 0)
            {
                return true;
            }

            if (!gameMap.MyNodeGrid.isWalkable(gridX, gridY)) //障碍物
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra vị trí đích đến (tọa độ thực) có nằm trong điểm Block không
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        public static bool InObs(ObjectTypes objType, int mapCode, int toX, int toY, int holdGridNum = 0, byte holdBitSet = 0)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (toX >= gameMap.MapWidth || toX < 0 || toY >= gameMap.MapHeight || toY < 0)
            {
                return true;
            }

            return Global.InObsByGridXY(objType, mapCode, (int)(toX / gameMap.MapGridWidth), (int)(toY / gameMap.MapGridHeight), holdGridNum, holdBitSet);
        }

        /// <summary>
        /// Kiểm tra vị trí đích đến (tọa độ lưới) có nằm trong điểm Block không
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        public static bool InObsByGridXY(ObjectTypes objType, int mapCode, int gridX, int gridY, int holdGridNum = 0, byte holdBitSet = 0)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (gridX >= gameMap.MapGridColsNum || gridX < 0 || gridY >= gameMap.MapGridRowsNum || gridY < 0)
            {
                return true;
            }

            if (!gameMap.MyNodeGrid.isWalkable(gridX, gridY))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra vị trí đích có đến được không
        /// </summary>
        public static bool IsGridReachable(int mapCode, int gridX, int gridY)
        {
            bool nCanMove = false;
            GameMap gameMap = GameManager.MapMgr.GetGameMap(mapCode);
            if (null != gameMap)
            {
                nCanMove = nCanMove | (gameMap.CanMove(gridX, gridY) ? true : false);
                nCanMove = nCanMove | (gameMap.CanMove(gridX, gridY + 1) ? true : false);
                nCanMove = nCanMove | (gameMap.CanMove(gridX, gridY - 1) ? true : false);
                nCanMove = nCanMove | (gameMap.CanMove(gridX + 1, gridY) ? true : false);
                nCanMove = nCanMove | (gameMap.CanMove(gridX - 1, gridY) ? true : false);
            }

            return nCanMove;
        }

        #endregion

        #region Lưới bản đổ

        /// <summary>
        /// 根据传入的格子坐标和方向返回指定方向的格子坐标
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <returns></returns>
        public static Point GetGridPointByDirection(int direction, int gridX, int gridY)
        {
            int nCurrX = (int)gridX;
            int nCurrY = (int)gridY;

            int nX = nCurrX;
            int nY = nCurrY;

            switch ((Direction)direction)
            {
                case Direction.DR_UP:
                    nX = nCurrX;
                    nY = nCurrY + 1;
                    break;

                case Direction.DR_UPRIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY + 1;
                    break;

                case Direction.DR_RIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY;
                    break;

                case Direction.DR_DOWNRIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY - 1;
                    break;

                case Direction.DR_DOWN:
                    nX = nCurrX;
                    nY = nCurrY - 1;
                    break;

                case Direction.DR_DOWNLEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY - 1;
                    break;

                case Direction.DR_LEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY;
                    break;

                case Direction.DR_UPLEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY + 1;
                    break;
            }

            return new Point(nX, nY);
        }

        /// <summary>
        /// 根据传入的格子坐标和方向返回指定方向的格子列表
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <returns></returns>
        public static List<Point> GetGridPointByDirection(int direction, int gridX, int gridY, int nNum)
        {
            List<Point> list = new List<Point>();

            int nCurrX = (int)gridX;
            int nCurrY = (int)gridY;

            int nX = nCurrX;
            int nY = nCurrY;

            //不考虑坐骑速度
            for (int i = 0; i < nNum; i++)
            {
                switch ((Direction)direction)
                {
                    case Direction.DR_UP:
                        nY++;
                        break;

                    case Direction.DR_UPRIGHT:
                        nX++;
                        nY++;
                        break;

                    case Direction.DR_RIGHT:
                        nX++;
                        break;

                    case Direction.DR_DOWNRIGHT:
                        nX++;
                        nY--;
                        break;

                    case Direction.DR_DOWN:
                        nY--;
                        break;

                    case Direction.DR_DOWNLEFT:
                        nX--;
                        nY--;
                        break;

                    case Direction.DR_LEFT:
                        nX--;
                        break;

                    case Direction.DR_UPLEFT:
                        nX--;
                        nY++;
                        break;
                }

                list.Add(new Point(nX, nY));
            }

            return list;
        }

        /// <summary>
        /// 判断是否在障碍物上(单纯障碍物)或者超出了地图边缘
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <returns></returns>
        public static bool JugeOnObsOrOverMap(KPlayer client, int gridX, int gridY)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[client.MapCode];
            if (gridX >= gameMap.MapGridColsNum || gridX < 0 || gridY >= gameMap.MapGridRowsNum || gridY < 0)
            {
                return true;
            }

            return !gameMap.MyNodeGrid.isWalkable(gridX, gridY);
        }

        /// <summary>
        /// 根据传入的格子坐标和方向返回指定方向的格子列表
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <returns></returns>
        public static List<Point> GetGridPointByDirection(int direction, int gridX, int gridY, string rangeMode, bool includeCenter = true)
        {
            List<Point> list = null;
            rangeMode = rangeMode.ToLower();
            if ("front2" == rangeMode) //攻击前边两个格
            {
                return Global.GetGridPointByDirection(direction, gridX, gridY, 2);
            }
            else if ("front3" == rangeMode) //攻击前边3个品字格
            {
                list = new List<Point>();
                Point p = GetGridPointByDirection(direction, gridX, gridY);
                list.Add(p);

                int direction1 = (direction + 1) % 8;
                p = GetGridPointByDirection(direction1, gridX, gridY);
                list.Add(p);

                int direction2 = direction - 1;
                if (direction2 < 0)
                {
                    direction2 = 7;
                }

                p = GetGridPointByDirection(direction2, gridX, gridY);
                list.Add(p);
            }
            else if ("front5" == rangeMode) //攻击前边5个品字格
            {
                return Global.GetGridPointByDirection(direction, gridX, gridY, 5);
            }
            else if ("1x1" == rangeMode) //3x3格子不包括中心
            {
                list = new List<Point>();
                list.Add(new Point(gridX, gridY));
            }
            else if ("3x3" == rangeMode) //3x3格子不包括中心
            {
                list = new List<Point>();
                for (int nX = gridX - 1; nX <= gridX + 1; nX++)
                {
                    for (int nY = gridY - 1; nY <= gridY + 1; nY++)
                    {
                        if (!includeCenter)
                        {
                            if (nX == gridX && nY == gridY)
                            {
                                continue;
                            }
                        }

                        list.Add(new Point(nX, nY));
                    }
                }
            }
            else if ("5x5" == rangeMode) //3x3格子不包括中心
            {
                list = new List<Point>();
                for (int nX = gridX - 2; nX <= gridX + 2; nX++)
                {
                    for (int nY = gridY - 2; nY <= gridY + 2; nY++)
                    {
                        if (!includeCenter)
                        {
                            if (nX == gridX && nY == gridY)
                            {
                                continue;
                            }
                        }

                        list.Add(new Point(nX, nY));
                    }
                }
            }
            else
            {
                int radius;
                list = new List<Point>();
                if (int.TryParse(rangeMode, out radius) && radius >= 0)
                {
                    radius = (radius - 1) / 100; //转换单位
                    for (int nX = gridX - radius; nX <= gridX + radius; nX++)
                    {
                        for (int nY = gridY - radius; nY <= gridY + radius; nY++)
                        {
                            list.Add(new Point(nX, nY));
                        }
                    }
                }
            }

            return list;
        }

        #endregion 地图格子操作相关

        #region 大小比较

        /// <summary>
        /// 获取两个整数中的小数
        /// </summary>
        /// <param name="l">第一个整数</param>
        /// <param name="r">第二个整数</param>
        /// <returns>两个数中的小数</returns>
        public static long GMin(long l, long r)
        {
            return Math.Min(l, r);
        }

        /// <summary>
        /// 获取两个整数中的大数
        /// </summary>
        /// <param name="l">第一个整数</param>
        /// <param name="r">第二个整数</param>
        /// <returns>两个数中的大数</returns>
        public static long GMax(long l, long r)
        {
            return Math.Max(l, r);
        }

        /// <summary>
        /// 获取两个整数中的小数
        /// </summary>
        /// <param name="l">第一个整数</param>
        /// <param name="r">第二个整数</param>
        /// <returns>两个数中的小数</returns>
        public static int GMin(int l, int r)
        {
            return Math.Min(l, r);
        }

        /// <summary>
        /// 获取两个整数中的大数
        /// </summary>
        /// <param name="l">第一个整数</param>
        /// <param name="r">第二个整数</param>
        /// <returns>两个数中的大数</returns>
        public static int GMax(int l, int r)
        {
            return Math.Max(l, r);
        }

        /// <summary>
        /// 获取两个浮点数中的小数
        /// </summary>
        /// <param name="l">第一个浮点数</param>
        /// <param name="r">第二个浮点数</param>
        /// <returns>两个浮点数中的小数</returns>
        public static double GMin(double l, double r)
        {
            return Math.Min(l, r);
        }

        /// <summary>
        /// 限定给定的值到给定的范围并返回
        /// </summary>
        /// <param name="v"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int Clamp(int v, int l, int r)
        {
            if (v < l)
            {
                v = l;
            }
            else if (v > r)
            {
                v = r;
            }

            return v;
        }

        /// <summary>
        /// 限定给定的值到给定的范围并返回
        /// </summary>
        /// <param name="v"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static long Clamp(long v, long l, long r)
        {
            if (v < l)
            {
                v = l;
            }
            else if (v > r)
            {
                v = r;
            }

            return v;
        }

        /// <summary>
        /// 限定给定的值到给定的范围并返回
        /// </summary>
        /// <param name="v"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double Clamp(double v, double l, double r)
        {
            if (v < l)
            {
                v = l;
            }
            else if (v > r)
            {
                v = r;
            }

            return v;
        }

        /// <summary>
        /// 获取两个浮点数中的大数
        /// </summary>
        /// <param name="l">第一个浮点数</param>
        /// <param name="r">第二个浮点数</param>
        /// <returns>两个浮点数中的大数</returns>
        public static double GMax(double l, double r)
        {
            return Math.Max(l, r);
        }

        /// <summary>
        /// 检查多个限制条件是否满足启用
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool CheckAnyForMultipleCondition(params bool?[] args)
        {
            //第一个值兼顾总配置开关，如果未配置，则其他条件不需限制
            if (args.Length == 0 || args[0] == null)
            {
                return true;
            }

            //如果都没限制，则返回true
            if (args.All((x) => x == null))
            {
                return true;
            }

            //如果有限制，则任意一个条件满足则返回true
            return args.Any((x) => x == true);
        }

        #endregion 大小比较

        #region 类型转换

        /// <summary>
        /// 安全的转换时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long SafeConvertToTicks(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return 0;
                DateTime dt;
                if (!DateTime.TryParse(str, out dt))
                {
                    return 0L;
                }

                return dt.Ticks / 10000;
            }
            catch (Exception)
            {
            }

            return 0L;
        }

        /// <summary>
        /// 安全的字符串到整型的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int SafeConvertToInt32(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
#if FastMode

            #region 优化这个函数

            bool navgite = false;
            int x = 0;
            foreach (var c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    x = x * 10 + c - '0';
                }
                else if (c == '-')
                {
                    navgite = true;
                }
                else if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                {
                    return 0;
                }
            }
            if (navgite)
            {
                x = -x;
            }
            return x;

            #endregion 优化这个函数

#else
            str = str.Trim();
            if (string.IsNullOrEmpty(str)) return 0;

            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {
            }

            return 0;
#endif
        }

        // 增加一个接口 [7/31/2013 LiaoWei]
        /// <summary>
        /// 安全的字符串到整型的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long SafeConvertToInt64(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            str = str.Trim();
            if (string.IsNullOrEmpty(str)) return 0;

            try
            {
                return Convert.ToInt64(str);
            }
            catch (Exception)
            {
            }

            return 0;
        }

        /// <summary>
        /// 安全的字符串到浮点的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double SafeConvertToDouble(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0.0;
            str = str.Trim();
            if (string.IsNullOrEmpty(str)) return 0.0;

            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
            }

            return 0.0;
        }

        /// <summary>
        /// 将字符串转换为Double类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static double[] String2DoubleArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string[] sa = str.Split(',');
            return Global.StringArray2DoubleArray(sa);
        }

        /// <summary>
        /// 将字符串数组转换为double类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static double[] StringArray2DoubleArray(string[] sa)
        {
            double[] da = new double[sa.Length];
            try
            {
                for (int i = 0; i < sa.Length; i++)
                {
                    string str = sa[i].Trim();
                    str = string.IsNullOrEmpty(str) ? "0.0" : str;
                    da[i] = Convert.ToDouble(str);
                }
            }
            catch (System.Exception ex)
            {
                string msg = ex.ToString();
            }

            return da;
        }

        /// <summary>
        /// 将字符串转换为Int类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static int[] String2IntArray(string str, char spliter = ',')
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string[] sa = str.Split(spliter);
            return Global.StringArray2IntArray(sa);
        }

        /// <summary>
        /// 将字符串转换为Int类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static string[] String2StringArray(string str, char spliter = '|')
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return str.Split(spliter);
        }

        /// <summary>
        /// 将字符串数组转换为Int类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static int[] StringArray2IntArray(string[] sa)
        {
            if (sa == null) return null;
            return StringArray2IntArray(sa, 0, sa.Length);
            /*
            int[] da = new int[sa.Length];
            for (int i = 0; i < sa.Length; i++)
            {
                string str = sa[i].Trim();
                str = string.IsNullOrEmpty(str) ? "0" : str;
                da[i] = Convert.ToInt32(str);
            }

            return da;
           */
        }

        public static int[] StringArray2IntArray(string[] sa, int start, int count)
        {
            if (sa == null) return null;
            if (start < 0 || start >= sa.Length) return null;
            if (count <= 0) return null;
            if (sa.Length - start < count) return null;

            int[] result = new int[count];
            for (int i = 0; i < count; ++i)
            {
                string str = sa[start + i].Trim();
                str = string.IsNullOrEmpty(str) ? "0" : str;
                result[i] = Convert.ToInt32(str);
            }

            return result;
        }

        /// <summary>
        /// 将字符串转换为Point类型, x1,y1
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public static Point StrToPoint(string str)
        {
            if (string.IsNullOrEmpty(str)) return new Point(double.NaN, double.NaN);
            str = str.Trim();
            if (str == "") return new Point(double.NaN, double.NaN);

            string[] fields = str.Split('|');
            if (fields.Length != 2) return new Point(double.NaN, double.NaN);

            try
            {
                string str1 = fields[0].Trim();
                str1 = string.IsNullOrEmpty(str) ? "0" : str1;

                string str2 = fields[1].Trim();
                str2 = string.IsNullOrEmpty(str) ? "0" : str2;
                return new Point(Convert.ToDouble(str1), Convert.ToDouble(str2));
            }
            catch (Exception)
            {
            }

            return new Point(double.NaN, double.NaN);
        }

        #endregion 类型转换

        #region ID duy nhất

        private static long BaseUniqueID;

        /// <summary>
        /// Trả về ID duy nhất
        /// </summary>
        /// <returns></returns>
        public static long GetUniqueID()
        {
            return Interlocked.Increment(ref BaseUniqueID);
        }

        /// <summary>
        /// Tìm đối tượng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static IObject FindSpriteByID(int id, int mapCode)
        {
            /// Người chơi
            KPlayer player = GameManager.ClientMgr.FindClient(id);
            /// Nếu là người chơi
            if (player != null)
            {
                return player;
            }
            else
            {
                /// Quái
                Monster monster = GameManager.MonsterMgr.FindMonster(mapCode, id);
                /// Nếu là quái
                if (monster != null)
                {
                    return monster;
                }
            }

            /// Không tìm thấy thì trả ra NULL
            return null;
        }

        #endregion

        #region 全局的随机数

        /// <summary>
        /// 全局的随机数对象
        /// </summary>
        private static Random GlobalRand = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// 获取全局的随机数
        /// </summary>
        /// <param name="minV"></param>
        /// <param name="MaxV"></param>
        /// <returns></returns>
        public static int GetRandomNumber(int minV, int maxV)
        {
            if (minV == maxV) return minV; //如果相等，则直接返回
            if (minV > maxV) return maxV;

            int ret = minV;
            lock (GlobalRand)
            {
                ret = GlobalRand.Next(minV, maxV);
            }

            return ret;
        }

        /// <summary>
        /// 获取全局的随机数
        /// </summary>
        /// <param name="minV"></param>
        /// <param name="MaxV"></param>
        /// <returns></returns>
        public static double GetRandom()
        {
            lock (GlobalRand)
            {
                return GlobalRand.NextDouble();
            }
        }

        #endregion 全局的随机数

        #region Quản lý vật phẩm

        /// <summary>
        /// Thời gian mặc định khi endtimme
        /// // Năm tháng ngày  = giờ phút giây
        /// </summary>
        public const string ConstGoodsEndTime = "1900-01-01 12:00:00";

        /// <summary>
        /// Trả về số lượng vật phẩm xếp chồng
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsGridNumByID(int goodsID)
        {
            if (!ItemManager._TotalGameItem.ContainsKey(goodsID))
            {
                return -1;
            }

            ItemData _Item = ItemManager._TotalGameItem[goodsID];

            return _Item.Stack;
        }

        /// <summary>
        /// Trả về bạc  [XSea 2015/8/24]
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsBoundTokenNumByID(int goodsID)
        {
            return 0;
        }

        /// <summary>
        /// 从队列中删除物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public static void RemoveGoodsData(KPlayer client, int id)
        {
            if (client.GoodsDataList == null)
            {
                return;
            }

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Id == id)
                    {
                        client.GoodsDataList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 从队列中删除物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public static bool RemoveGoodsData(KPlayer client, GoodsData gd)
        {
            if (null == gd) return false;
            if (client.GoodsDataList == null)
            {
                return false;
            }

            bool ret = false;
            lock (client.GoodsDataList)
            {
                ret = client.GoodsDataList.Remove(gd);
            }

            return ret;
        }

        /// <summary>
        /// Thêm vật phẩm vào nhân vật
        /// </summary>
        /// <param name="client"></param>
        public static void AddGoodsData(KPlayer client, GoodsData gd)
        {
            if (null == gd) return;
            if (client.GoodsDataList == null)
            {
                client.GoodsDataList = new List<GoodsData>();
            }

            lock (client.GoodsDataList)
            {
                client.GoodsDataList.Add(gd);
            }
        }

        /// <summary>
        /// Thêm vật phẩm tương ứng
        /// </summary>
        /// <param name="client"></param>
        public static GoodsData AddGoodsData(KPlayer client, int id, int goodsID, int forgeLevel, int quality, int goodsNum, int binding, int site, string startTime, string endTime, int strong, int bagIndex = 0, int IsUsing = -1, string Probs = "", int Series = -1, Dictionary<ItemPramenter, string> OtherParams = null)
        {
            GoodsData gd = new GoodsData()
            {
                Id = id,
                GoodsID = goodsID,
                Using = IsUsing,
                Forge_level = forgeLevel,
                Starttime = startTime,
                Endtime = endTime,
                Site = site,

                Props = Probs,
                GCount = goodsNum,
                Binding = binding,
                Series = Series,
                OtherParams = OtherParams,
                BagIndex = bagIndex,

                Strong = strong,
            };

            Global.AddGoodsData(client, gd);
            return gd;
        }

        /// <summary>
        /// Trả về thông tin vật phẩm theo DbID tương ứng
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetGoodsByDbID(KPlayer client, int dbID)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Id == dbID)
                    {
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Trả về thông tin vật phẩm theo DbID tương ứng
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetGoodsByDbIDPortalBag(KPlayer client, int dbID)
        {
            if (null == client.PortableGoodsDataList) return null;

            lock (client.PortableGoodsDataList)
            {
                for (int i = 0; i < client.PortableGoodsDataList.Count; i++)
                {
                    if (client.PortableGoodsDataList[i].Id == dbID)
                    {
                        return client.PortableGoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy ra các vật phẩm hết hạn
        /// </summary>
        /// <returns></returns>
        public static List<GoodsData> GetGoodsTimeExpired(KPlayer client)
        {
            List<GoodsData> expiredList = null;

            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (IsGoodsTimeOver(client.GoodsDataList[i]))
                    {
                        if (null == expiredList)
                        {
                            expiredList = new List<GoodsData>(); // Danh sách vật phẩm hết hạn
                        }

                        expiredList.Add(client.GoodsDataList[i]);
                    }
                }
            }

            return expiredList;
        }

        /// <summary>
        /// 从物品列表中查找指定的物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetGoodsByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID)
                    {
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 从物品列表中查找指定的物品并且是绑定的 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetBindGoodsByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID && client.GoodsDataList[i].Binding >= 1)
                    {
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 从物品列表中查找指定的物品并且是非绑定的 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetNotBindGoodsByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID && client.GoodsDataList[i].Binding <= 0)
                    {
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy ra danh sách vật phẩm không sử dụng)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetNotUsingGoodsByID(KPlayer client, int goodsID, int goodsLevel, int goodsQuality)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    // nếu là vật đang sử dụng thì thôi
                    if (client.GoodsDataList[i].Using >= 0)
                    {
                        continue;
                    }

                    if (client.GoodsDataList[i].GoodsID == goodsID &&
                        client.GoodsDataList[i].Forge_level == goodsLevel)
                    {
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy ra 1 vật phẩm có sẵn trên túi đồ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetGoodsByID(KPlayer client, int goodsID, int bingding, string endTime, ref int startIndex)
        {
            if (null == client.GoodsDataList) return null;

            lock (client.GoodsDataList)
            {
                if (startIndex >= client.GoodsDataList.Count) return null;

                for (int i = startIndex; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID &&
                        client.GoodsDataList[i].Binding == bingding &&
                        Global.DateTimeEqual(client.GoodsDataList[i].Endtime, endTime))
                    {
                        startIndex = i + 1;
                        return client.GoodsDataList[i];
                    }
                }
            }

            return null;
        }

        // 增加一个寻找物品的接口 根据物品一系列的品质查找 [1/23/2014 LiaoWei]
        /// <summary>
        /// 查找物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        //public static GoodsData GetGoodsByID(KPlayer client, int goodsID, int nNum, int bingding, int nforgeLev, int nAppendPro, int nLucky, int nExcellenceInfo, bool bIsusing = false)
        //{
        //    if (null == client.GoodsDataList)
        //        return null;

        //    GoodsData goods = null;
        //    lock (client.GoodsDataList)
        //    {
        //        for (int i = 0; i < client.GoodsDataList.Count; i++)
        //        {
        //            if (bingding != -1)
        //            {
        //                if (client.GoodsDataList[i].GoodsID == goodsID && client.GoodsDataList[i].GCount == nNum && client.GoodsDataList[i].Binding == bingding &&
        //                        client.GoodsDataList[i].Forge_level == nforgeLev && client.GoodsDataList[i].AppendPropLev == nAppendPro && client.GoodsDataList[i].Lucky == nLucky &&
        //                            client.GoodsDataList[i].ExcellenceInfo == nExcellenceInfo)
        //                {
        //                    goods = client.GoodsDataList[i];

        //                    if (goods != null)
        //                    {
        //                        if (bIsusing && goods.Using >= 0)
        //                        {
        //                            return goods;
        //                        }
        //                        else
        //                        {
        //                            if (goods.Using < 0)
        //                                return goods;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (client.GoodsDataList[i].GoodsID == goodsID && client.GoodsDataList[i].GCount == nNum && client.GoodsDataList[i].Forge_level == nforgeLev &&
        //                        client.GoodsDataList[i].AppendPropLev == nAppendPro && client.GoodsDataList[i].Lucky == nLucky &&
        //                            client.GoodsDataList[i].ExcellenceInfo == nExcellenceInfo)
        //                {
        //                    goods = client.GoodsDataList[i];

        //                    if (goods != null)
        //                    {
        //                        if (bIsusing && goods.Using >= 0)
        //                        {
        //                            return goods;
        //                        }
        //                        else
        //                        {
        //                            if (goods.Using < 0)
        //                                return goods;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// 从物品列表中查找指定的物品(索引)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetGoodsByIndex(KPlayer client, int index)
        {
            if (null == client.GoodsDataList) return null;

            GoodsData goodsData = null;
            lock (client.GoodsDataList)
            {
                if (index >= 0 && index < client.GoodsDataList.Count)
                {
                    goodsData = client.GoodsDataList[index];
                }
            }

            return goodsData;
        }

        /// <summary>
        /// 修改物品绑定性 [7/18/2014 LiaoWei]
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static void ModifyGoodsBindPorp(KPlayer client, int dbID, int nBindPorp)
        {
            if (null == client.GoodsDataList)
                return;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Id == dbID)
                    {
                        client.GoodsDataList[i].Binding = nBindPorp;
                        break;
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 获取物品占用的背包格子数量(已经装备的不算占用)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetGoodsUsedGrid(KPlayer client)
        {
            int ret = 0;
            if (client.GoodsDataList == null)
            {
                return ret;
            }

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Using <= 0)
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Tạm gán chết 50 ô sau này code tiếp
        /// </summary>
        /// <returns></returns>
        public static int GetTotalMaxBagGridCount(KPlayer client)
        {
            return 50;
            //return ((client.BagNum + 1) * (int)RoleBagNumPerPage.PageNum);
        }

        /// <summary>
        /// Hàm này kiểm tra xem túi đồ có bị dầy khi add vào túi đồ ko
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static bool CanAddGoods(KPlayer client, int goodsID, int newGoodsNum, int binding, string endTime = Global.ConstGoodsEndTime, bool canUseOld = true, bool bLeftGrid = false)
        {
            if (client.GoodsDataList == null)
            {
                return true;
            }

            int totalGridNum = 0;
            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Using >= 0)
                    {
                        continue;
                    }

                    totalGridNum++;
                }
            }

            int totalMaxGridCount = 50;
            return (totalGridNum < totalMaxGridCount);
        }

        public static bool CanAddGoodsDataList(KPlayer client, List<GoodsData> goodsDataList)
        {
            if (null == goodsDataList) return true;

            return KTGlobal.IsHaveSpace(goodsDataList.Count, client);
        }

        public static bool CanAddGoodsNum(KPlayer client, int newGoodsCount)
        {
            return KTGlobal.IsHaveSpace(newGoodsCount, client);
        }

        public static int GetGoodsByName(string name)
        {
            return 0;
        }

        /// <summary>
        /// 物品记录日志标记字典
        /// Thread-Safe
        /// </summary>
        private static Dictionary<int, int> logItemDict
        {
            get { lock (_logItemLock) { return _logItemDict_storage; } }
            set { lock (_logItemLock) { _logItemDict_storage = value; } }
        }

        private static object _logItemLock = new object();
        private static Dictionary<int, int> _logItemDict_storage = null;

        /// <summary>
        /// 复活时需要公告的怪物ID
        /// </summary>
        private static Dictionary<int, int> reliveMonsterGongGaoDict = new Dictionary<int, int>();

        /// <summary>
        /// 是否是需要公告怪物ID
        /// </summary>
        public static bool IsGongGaoReliveMonster(int nMonsterID)
        {
            int nGongGaoMark = 0;
            if (!reliveMonsterGongGaoDict.TryGetValue(nMonsterID, out nGongGaoMark) || 0 == nGongGaoMark)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Điều chỉnh tên khi login
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ModifyGoodsLogName(GoodsData goodsData)
        {
            if (!ItemManager._TotalGameItem.ContainsKey(goodsData.GoodsID))
            {
                return "";
            }

            ItemData _Item = ItemManager._TotalGameItem[goodsData.GoodsID];

            return _Item.Name;
        }

        /// <summary>
        /// 格式化更新数据库物品字符串
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatUpdateDBGoodsStr(params object[] args)
        {
            if (args.Length != 23)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("FormatUpdateDBGoodsStr, 参数个数不对{0}", args.Length));
                return null;
            }

            return string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}:{20}:{21}:{22}", args);
        }

        public static int UpdateGoodsProp(KPlayer client, GoodsData gd)
        {
            return 0;
        }

        /// <summary>
        /// 修改物品的宝石列表
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="gd"></param>
        /// <param name="newGoodsNum"></param>
        /// <param name="newHint"></param>
        /// <returns></returns>
        public static int ModGoodsJewelDBCommand(TCPOutPacketPool pool, KPlayer client, GoodsData gd, string jewellist, int binding)
        {
            string strcmd = "";

            //向DBServer请求修改物品2
            string[] dbFields = null;
            strcmd = Global.FormatUpdateDBGoodsStr(client.RoleID, gd.Id, "*", "*", "*", "*", "*", "*", "*", "*", jewellist, "*", "*", "*", "*", binding, "*", "*", "*", "*", "*", "*", "*"); // 卓越信息 [12/13/2013 LiaoWei] 装备转生
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(Global._TCPManager.tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields, client.ServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                return -1;
            }

            if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[1]) < 0)
            {
                return -2;
            }

            gd.Binding = binding;

            EventLogManager.AddGoodsEvent(client, OpTypes.Forge, OpTags.None, gd.GoodsID, gd.Id, 0, gd.GCount, "镶嵌修改");

            //不通知客户端，由镶嵌的操作处理，jewellist的更新问题
            return 0;
        }

        /// <summary>
        /// 数据库命令执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static int AddGoodsDBCommand(TCPOutPacketPool pool, KPlayer client, int goodsID, int goodsNum, int quality, string props, int forgeLevel, int binding, int site, string jewelList, bool useOldGrid, int newHint, string goodsFromWhere, string endTime = Global.ConstGoodsEndTime, int addPropIndex = 0, int bornIndex = 0, int lucky = 0, int strong = 0, int ExcellenceProperty = 0, int nAppendPropLev = 0, int nEquipChangeLife = 0, List<int> washProps = null, List<int> elementhrtsProps = null)
        {
            int dbRet = 0;
            int gridNum = Global.GetGoodsGridNumByID(goodsID);
            gridNum = Global.GMax(gridNum, 1);

            int addCount = (goodsNum - 1) / gridNum + 1;
            for (int i = 0; i < addCount; i++)
            {
                int thisTimeNum = gridNum;
                if (i >= (addCount - 1) && (goodsNum % gridNum) > 0)
                {
                    thisTimeNum = goodsNum % gridNum;
                }
            }

            return dbRet;

            /////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// 根据物品ID查找背包中总的数量
        /// </summary>
        /// <param name="id"></param>
        public static int GetTotalGoodsCountByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return 0;

            int ret = 0;
            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    //if (client.GoodsDataList[i].Site > 0) continue; 这个无用吧？
                    if (client.GoodsDataList[i].GoodsID != goodsID) continue;
                    if (Global.IsGoodsTimeOver(client.GoodsDataList[i])
                        || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        continue; //如果已经超时
                    ret += client.GoodsDataList[i].GCount;
                }
            }

            return ret;
        }

        /// <summary>
        /// 根据物品ID查找背包中非绑定的总的数量 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="id"></param>
        public static int GetTotalNotBindGoodsCountByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return 0;

            int ret = 0;
            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    //if (client.GoodsDataList[i].Site > 0) continue; 这个无用吧？
                    if (client.GoodsDataList[i].GoodsID != goodsID) continue;
                    if (client.GoodsDataList[i].Binding > 0) continue;
                    if (Global.IsGoodsTimeOver(client.GoodsDataList[i])
                        || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        continue; //如果已经超时
                    ret += client.GoodsDataList[i].GCount;
                }
            }

            return ret;
        }

        /// <summary>
        /// 根据物品ID查找背包中绑定的总的数量 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="id"></param>
        public static int GetTotalBindGoodsCountByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsDataList) return 0;

            int ret = 0;
            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    //if (client.GoodsDataList[i].Site > 0) continue; 这个无用吧？
                    if (client.GoodsDataList[i].GoodsID != goodsID) continue;
                    if (client.GoodsDataList[i].Binding < 1) continue;
                    if (Global.IsGoodsTimeOver(client.GoodsDataList[i])
                        || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        continue; //如果已经超时
                    ret += client.GoodsDataList[i].GCount;
                }
            }

            return ret;
        }

        /// <summary>
        /// 获取到正在装备的物品列表
        /// </summary>
        /// <param name="client"></param>
        public static List<GoodsData> GetUsingGoodsList(KPlayer client, int binding)
        {
            if (null == client.GoodsDataList) return null;

            List<GoodsData> goodsDataList = new List<GoodsData>();

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Binding != binding)
                    {
                        continue;
                    }

                    if (client.GoodsDataList[i].Using > 0)
                    {
                        goodsDataList.Add(client.GoodsDataList[i]);
                    }
                }
            }

            return goodsDataList;
        }

        /// <summary>
        /// 获取到正在装备的物品列表
        /// </summary>
        /// <param name="client"></param>
        public static List<GoodsData> GetUsingGoodsList(KPlayer clientData)
        {
            if (null == clientData.GoodsDataList) return null;

            List<GoodsData> goodsDataList = new List<GoodsData>();

            lock (clientData.GoodsDataList)
            {
                for (int i = 0; i < clientData.GoodsDataList.Count; i++)
                {
                    if (clientData.GoodsDataList[i].Using > 0)
                    {
                        goodsDataList.Add(clientData.GoodsDataList[i]);
                    }
                }
            }

            return goodsDataList;
        }

        /// <summary>
        /// Trả về giá của vật phẩm - TẠM THỜI RETRUN 0 khi nào làm tới shop thì sửa
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static int GetGoodsDataPrice(GoodsData goodsData)
        {
            if (null == goodsData) return 0;
            if (goodsData.GCount <= 0) return 0;

            return 0;
        }

        /// <summary>
        /// Trả về giá phục chế gì đó
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static int GetGoodsDataZaiZao(GoodsData goodsData)
        {
            if (null == goodsData) return 0;
            if (goodsData.GCount <= 0) return 0;
            return 0;
        }

        /// <summary>
        /// Trả về điểm gì đó
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static int GetGoodsDataJingYuan(GoodsData goodsData)
        {
            if (null == goodsData) return 0;
            if (goodsData.GCount <= 0) return 0;

            return 0;
        }

        /// <summary>
        /// Sắp xếp lại ba lô của người dùng
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //public static void ResetBagAllGoods(KPlayer client, bool notifyClient = true)
        //{
        //    byte[] bytesCmd = null;
        //    if (client.GoodsDataList != null)
        //    {
        //        lock (client.GoodsDataList)
        //        {
        //            Dictionary<string, GoodsData> oldGoodsDict = new Dictionary<string, GoodsData>();

        //            List<GoodsData> toRemovedGoodsDataList = new List<GoodsData>();

        //            for (int i = 0; i < client.GoodsDataList.Count; i++)
        //            {
        //                if (client.GoodsDataList[i].Using > 0)
        //                {
        //                    continue;
        //                }

        //                client.GoodsDataList[i].BagIndex = 0;

        //                int gridNum = Global.GetGoodsGridNumByID(client.GoodsDataList[i].GoodsID);
        //                if (gridNum <= 1)
        //                {
        //                    continue;
        //                }

        //                GoodsData oldGoodsData = null;
        //                string key = string.Format("{0}_{1}_{2}_{3}", client.GoodsDataList[i].GoodsID,
        //                    client.GoodsDataList[i].Binding,
        //                    Global.DateTimeTicks(client.GoodsDataList[i].Starttime),
        //                    Global.DateTimeTicks(client.GoodsDataList[i].Endtime));

        //                if (oldGoodsDict.TryGetValue(key, out oldGoodsData))
        //                {
        //                    int toAddNum = Global.GMin((gridNum - oldGoodsData.GCount), client.GoodsDataList[i].GCount);

        //                    oldGoodsData.GCount += toAddNum;

        //                    client.GoodsDataList[i].GCount -= toAddNum;
        //                    client.GoodsDataList[i].BagIndex = 1;
        //                    oldGoodsData.BagIndex = 1;
        //                    if (!ResetBagGoodsData(client, client.GoodsDataList[i]))
        //                    {
        //                        break;
        //                    }

        //                    EventLogManager.AddGoodsEvent(client, OpTypes.Sort, OpTags.None, oldGoodsData.GoodsID, oldGoodsData.Id, toAddNum, oldGoodsData.GCount, "整理背包");
        //                    EventLogManager.AddGoodsEvent(client, OpTypes.Sort, OpTags.None, client.GoodsDataList[i].GoodsID, client.GoodsDataList[i].Id, -toAddNum, client.GoodsDataList[i].GCount, "整理背包");

        //                    if (oldGoodsData.GCount >= gridNum) //旧的物品已经加满
        //                    {
        //                        if (client.GoodsDataList[i].GCount > 0)
        //                        {
        //                            oldGoodsDict[key] = client.GoodsDataList[i];
        //                        }
        //                        else
        //                        {
        //                            oldGoodsDict.Remove(key);
        //                            toRemovedGoodsDataList.Add(client.GoodsDataList[i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (client.GoodsDataList[i].GCount <= 0)
        //                        {
        //                            toRemovedGoodsDataList.Add(client.GoodsDataList[i]);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    oldGoodsDict[key] = client.GoodsDataList[i];
        //                }
        //            }

        //            for (int i = 0; i < toRemovedGoodsDataList.Count; i++)
        //            {
        //                client.GoodsDataList.Remove(toRemovedGoodsDataList[i]);
        //            }

        //            client.GoodsDataList.Sort(delegate (GoodsData x, GoodsData y)
        //            {
        //                return (y.GoodsID - x.GoodsID);
        //            });

        //            int index = 0;
        //            for (int i = 0; i < client.GoodsDataList.Count; i++)
        //            {
        //                if (client.GoodsDataList[i].Using > 0)
        //                {
        //                    continue;
        //                }

        //                if (GameManager.Flag_OptimizationBagReset)
        //                {
        //                    bool godosCountChanged = client.GoodsDataList[i].BagIndex > 0;
        //                    client.GoodsDataList[i].BagIndex = index++;
        //                    if (godosCountChanged)
        //                    {
        //                        if (!Global.ResetBagGoodsData(client, client.GoodsDataList[i]))
        //                        {
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    client.GoodsDataList[i].BagIndex = index++;
        //                    if (!Global.ResetBagGoodsData(client, client.GoodsDataList[i]))
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            bytesCmd = DataHelper.ObjectToBytes<List<GoodsData>>(client.GoodsDataList);
        //        }
        //    }
        //    else
        //    {
        //        return;
        //    }

        //    if (notifyClient)
        //    {
        //        TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, bytesCmd, 0, bytesCmd.Length, (int)TCPGameServerCmds.CMD_SPR_RESETBAG);

        //        Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
        //    }
        //}

        /// <summary>
        /// Trả về Loại vật phẩm
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsCatetoriy(int goodsID)
        {
            if (!ItemManager._TotalGameItem.ContainsKey(goodsID))
            {
                return -1;
            }

            ItemData _Item = ItemManager._TotalGameItem[goodsID];

            return _Item.Genre;
        }

        /// <summary>
        /// 缓存物品的阶数
        /// </summary>
        private static Dictionary<int, int> GoodsSuitCacheDict = new Dictionary<int, int>();

        /// <summary>
        /// Trả về ID bộ
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsShouShiSuitID(int goodsID)
        {
            int suit = -1;

            return suit;
        }

        public static void ClearCachedGoodsShouShiSuitID()
        {
            lock (GoodsSuitCacheDict)
            {
                GoodsSuitCacheDict.Clear();
            }
        }

        public static bool ResetBagGoodsData(KPlayer client, GoodsData goodsData)
        {
            string strcmd = "";

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;

            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, goodsData.Id);

            TotalUpdate.Add(UPDATEITEM.BAGINDEX, goodsData.BagIndex);

            string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

            string[] UpdateFriend = null;

            // Request Sửa Số lượng vào DB
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(Global._TCPManager.tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out UpdateFriend, client.ServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                return false;
            }

            if (UpdateFriend.Length <= 0 || Convert.ToInt32(UpdateFriend[1]) < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra độ hợp lệ của vật phẩm
        /// </summary>
        /// <param name="client"></param>
        public static void CheckGoodsDataValid(KPlayer client)
        {
            if (client.ClientSocket.IsKuaFuLogin)
            {
                //Nếu là center server thì bỏ qua việc check xóa bỏ
                return;
            }

            if (client.GoodsDataList == null)
            {
                return;
            }

            lock (client.GoodsDataList)
            {
                List<GoodsData> toRemovedGoodsDataList = new List<GoodsData>();
                List<GoodsData> FixTimeMaterial = new List<GoodsData>();
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {

                    ItemData _Item = ItemManager.GetItemTemplate(client.GoodsDataList[i].GoodsID);

                    if(_Item==null)
                    {
                        toRemovedGoodsDataList.Add(client.GoodsDataList[i]);
                    }
                    else
                    {
                        if(ItemManager.IsMaterial(_Item))
                        {
                            if(client.GoodsDataList[i].Endtime == Global.ConstGoodsEndTime)
                            {;
                                FixTimeMaterial.Add(client.GoodsDataList[i]);
                            }
                        }
                    }
                    // Nếu vật phẩm không còn tồn tại trong TEMPLATE thì xóa
                    //if (!ItemManager._TotalGameItem.ContainsKey(client.GoodsDataList[i].GoodsID))
                    //{
                    //    toRemovedGoodsDataList.Add(client.GoodsDataList[i]);
                    //}
                }

                for (int i = 0; i < toRemovedGoodsDataList.Count; i++)
                {
                    GoodsData goodsData = toRemovedGoodsDataList[i];
                    client.GoodsDataList.Remove(goodsData);
                    goodsData.GCount = 0;
                    Global.ResetBagGoodsData(client, goodsData);
                }

                for (int i = 0; i < FixTimeMaterial.Count; i++)
                {
                    DateTime dt = DateTime.Now.AddDays(30);

                    // "1900-01-01 12:00:00";
                   string TmpEnd = dt.ToString("yyyy-MM-dd HH#mm#ss");

                    GoodsData goodsData = FixTimeMaterial[i];

                    Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

                    TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
                    TotalUpdate.Add(UPDATEITEM.ITEMDBID, goodsData.Id);
                    TotalUpdate.Add(UPDATEITEM.END_TIME, TmpEnd);

                    if (ItemManager.UpdateItemPrammenter(TotalUpdate, goodsData, client, "FIXTIME",true))
                    {
                        LogManager.WriteLog(LogTypes.Item, "[" + client.RoleID + "][" + client.RoleName + "] FIX hạn sử dụng :" + goodsData.ItemName);
                    }

                }
            }
        }

        /// <summary>
        /// Tạm thời để lại xử lý sau
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsData GetNewGoodsData(int goodsID, int binding)
        {
            int maxStrong = 0;
            int lucky = 0;

            //对于新给的物品，gcount必须设置成1，如果是持续消耗品，由外部在修改gcount的值!!!!!
            return GetNewGoodsData(goodsID, 1, 0, 0, binding, 0, lucky, maxStrong);
        }

        /// <summary>
        /// Tạm thời để lại xử lý sau
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="gcount"></param>
        /// <param name="quality"></param>
        /// <param name="forgeLevel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static GoodsData GetNewGoodsData(int goodsID, int gcount, int quality, int forgeLevel, int binding, int bornIndex, int lucky, int strong, int nExcellenceInfo = 0, int nAppendPropLev = 0, int nChangeLife = 0)
        {
            GoodsData goodsData = new GoodsData()
            {
                Id = -1,
                GoodsID = goodsID,
                Using = 0,
                Forge_level = forgeLevel,
                Starttime = "1900-01-01 12:00:00",
                Endtime = Global.ConstGoodsEndTime,
                Site = 0,
                Props = "",
                GCount = gcount,
                Binding = binding,
                BagIndex = 0,
                Strong = strong,
            };

            return goodsData;
        }

        /// <summary>
        /// 复制GoodsData对象
        /// </summary>
        /// <param name="oldGoodsData"></param>
        /// <returns></returns>
        public static GoodsData CopyGoodsData(GoodsData oldGoodsData)
        {
            GoodsData goodsData = oldGoodsData;
            /*GoodsData goodsData = new GoodsData()
            {
                Id = oldGoodsData.Id,
                GoodsID = oldGoodsData.GoodsID,
                Using = oldGoodsData.Using,
                Forge_level = oldGoodsData.Forge_level,
                Starttime = oldGoodsData.Starttime,
                Endtime = oldGoodsData.Endtime,
                Site = oldGoodsData.Site,
                Quality = oldGoodsData.Quality,
                Props = oldGoodsData.Props,
                GCount = oldGoodsData.GCount,
                Binding = oldGoodsData.Binding,
                Jewellist = oldGoodsData.Jewellist,
                BagIndex = oldGoodsData.BagIndex,
                SaleMoney = oldGoodsData.SaleMoney,
                SaleYuanBao = oldGoodsData.SaleYuanBao,
                SaleYinPiao = oldGoodsData.SaleYinPiao,
                AddPropIndex = oldGoodsData.AddPropIndex,
                BornIndex = oldGoodsData.BornIndex,
                Lucky = oldGoodsData.Lucky,
                Strong = oldGoodsData.Strong,
                ExcellenceInfo = oldGoodsData.ExcellenceInfo,
                AppendPropLev = oldGoodsData.AppendPropLev,
                ChangeLifeLevForEquip = oldGoodsData.ChangeLifeLevForEquip,
            };*/

            return goodsData;
        }

        /// <summary>
        /// 获取字符串的Ticks
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static long DateTimeTicks(string strDateTime)
        {
            try
            {
                DateTime dt1;
                if (!DateTime.TryParse(strDateTime, out dt1))
                {
                    return 0;
                }

                return dt1.Ticks;
            }
            catch (Exception)
            {
            }

            return 0;
        }

        /// <summary>
        /// 判断两个时间是否相等
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static bool DateTimeEqual(string strDateTime1, string strDateTime2)
        {
            try
            {
                // 应该只需要比较两个字符串就行 ChenXiaojun
                return (strDateTime1 == strDateTime2);

                //DateTime dt1;
                //if (!DateTime.TryParse(strDateTime1, out dt1))
                //{
                //    return false;
                //}

                //DateTime dt2;
                //if (!DateTime.TryParse(strDateTime2, out dt2))
                //{
                //    return false;
                //}

                //return (dt1.Ticks == dt2.Ticks);
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// 是否限制时间的物品
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static bool IsTimeLimitGoods(GoodsData goodsData)
        {
            if (!string.IsNullOrEmpty(goodsData.Endtime) && !Global.DateTimeEqual(goodsData.Endtime, Global.ConstGoodsEndTime)) //限时物品
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 限制时间的物品是否过期
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static bool IsGoodsTimeOver(GoodsData goodsData)
        {
            if (!Global.IsTimeLimitGoods(goodsData)) //如果非限时物品
            {
                return false;
            }

            long nowTicks = TimeUtil.NOW() * 10000;
            long goodsEndTicks = Global.DateTimeTicks(goodsData.Endtime);
            if (nowTicks >= goodsEndTicks) return true;
            return false;
        }

        /// <summary>
        /// 检查物品是否未到使用时间
        /// </summary>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static bool IsGoodsNotReachStartTime(GoodsData goodsData)
        {
            if (!Global.IsTimeLimitGoods(goodsData)) //如果非限时物品
            {
                return false;
            }

            long nowTicks = TimeUtil.NOW() * 10000;
            long goodsStartTicks = Global.DateTimeTicks(goodsData.Starttime);
            if (nowTicks < goodsStartTicks) return true;

            return false;
        }

        /// <summary>
        /// Số lần sử dụng mặc định của vật phẩm tạm thời set là 1
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsDefaultCount(int goodsID)
        {
            return 1;
        }

        /// <summary>
        /// Số lần sử dụng mặc định của vật phẩm tạm thời set là 1
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsUsingNum(int goodsID)
        {
            return 1;
        }

        /// <summary>
        /// 绑定铜钱符每日使用次数列表缓存
        /// </summary>
        public static int[] _VipUseBindTongQianGoodsIDNum = null;

        /// <summary>
        /// Lấy ra số lượng vật phẩm sử dụng tối đa để tạm là 1
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetGoodsLimitNum(KPlayer client, int goodsID)
        {
            return 1;
        }

        /// <summary>
        /// Trả về loại tiền tệ mà good sử dụng
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="moneyType"></param>
        /// <returns></returns>
        public static int GetGoodsPriceByMoneyType(int goodsID, int moneyType)
        {
            String sPriceField = "";

            sPriceField = "PriceOne";

            return 1;
        }

        /// <summary>
        /// Update good theo pramenter
        /// </summary>
        /// <param name="fields"></param>
        public static TCPProcessCmdResults ModifyGoodsByCmdParams(KPlayer client, String cmdData)
        {
            //Nếu độ dài gửi lên mà  khác 9 thì nghỉ luôn
            string[] fields = cmdData.Split(':');
            if (fields.Length != 9)
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            int roleID = Convert.ToInt32(fields[0]);
            int modType = Convert.ToInt32(fields[1]);
            int id = Convert.ToInt32(fields[2]);
            int goodsID = Convert.ToInt32(fields[3]);
            int isusing = Convert.ToInt32(fields[4]);
            int site = Convert.ToInt32(fields[5]);
            int gcount = Convert.ToInt32(fields[6]);
            int bagindex = Convert.ToInt32(fields[7]); // Ô trên túi đồ
            String extraParams = fields[8];//Thông tin bổ sung khi gói tin cần gửi thêm

            //int nID = (int)TCPGameServerCmds.CMD_SPR_MOD_GOODS;
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;
            TCPManager tcpMgr = Global._TCPManager;
            TMSKSocket socket = client.ClientSocket;

            GoodsData goodsData = null;

            goodsData = Global.GetGoodsByDbID(client, id);

            if (goodsData == null)
            {
                goodsData = Global.GetGoodsByDbIDPortalBag(client, id);
            }

            /// Nếu vật phẩm không tồn tại
            if (null == goodsData)
            {
                // Nếu đồ không tồn tại trả về lỗi luôn
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            // nếu ID vật phẩm khác với ID truyền lên thì báo lỗi
            if (goodsData.GoodsID != goodsID)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Vật phẩm truyền lên có ID khác với ID của GS trong TEMPLATE, CMD={0}, Client={1}, RoleID={2}", TCPGameServerCmds.CMD_SPR_MOD_GOODS, Global.GetSocketRemoteEndPoint(socket), roleID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            if (modType == (int)ModGoodsTypes.SaleToNpc && (goodsData.Site != 0 || goodsData.Using > 0))
            {
                //Chỉ có thể bán các vật phẩm trông túi đồ và đang không mặc trên người
                LogManager.WriteLog(LogTypes.Error, string.Format("Chỉ có thể bán các vật phẩm đang ở hành trang, CMD={0}, Client={1}, RoleID={2}", TCPGameServerCmds.CMD_SPR_MOD_GOODS, Global.GetSocketRemoteEndPoint(socket), roleID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            /// Nếu là bán đồ vào cửa hàng, mà loại vật phẩm không bán được
            if (modType == (int)ModGoodsTypes.SaleToNpc && !ItemManager.IsCanBeSold(goodsData))
            {
                PlayerManager.ShowNotification(client, "Vật phẩm này không thể bán!");
                return TCPProcessCmdResults.RESULT_OK;
            }

            /// Nếu là trang bị nhưng trang bị cần lại không có trong túi đồ
            if (modType == (int)ModGoodsTypes.EquipLoad && (goodsData.Site != 0 || goodsData.Using > 0))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Là trang bị nhưng trang bị cần lại không có trong túi đồ, CMD={0}, Client={1}, RoleID={2}", TCPGameServerCmds.CMD_SPR_MOD_GOODS, Global.GetSocketRemoteEndPoint(socket), roleID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            int oldGoodsNum = goodsData.GCount;

            /// Kiểm tra số lượng truyền lên có đúng với số lượng hiện có không
            if (gcount != goodsData.GCount && modType != (int)ModGoodsTypes.SplitItem)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Số lượng gửi lên khác với số lượng trong túi đồ không thực hiện lệnh, CMD={0}, Client={1}, RoleID={2}, GoodsID={3}", TCPGameServerCmds.CMD_SPR_MOD_GOODS, Global.GetSocketRemoteEndPoint(socket), roleID, goodsID));
                return TCPProcessCmdResults.RESULT_OK;
            }

            // THực Hiện ghi lại nếu vật phẩm bán hoặc phá hủy

            /// Nếu thao tác giống nhau thì không phải xử lý. Điều này có thể phát sinh từ việc lag packet hoặc lostpacket | Packet bị dupper
            if (modType != (int)ModGoodsTypes.Abandon && isusing == goodsData.Using && site == goodsData.Site && gcount == goodsData.GCount && bagindex == goodsData.BagIndex)
            {
                return TCPProcessCmdResults.RESULT_OK;
            }

            /// Nếu thay đổi vị trí đồ đạc
            if (site != goodsData.Site)
            {
                /// Nếu đang mặc trên người thì bỏ qua
                if (isusing > 0)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }
            }

            /// Nếu là tháo đồ hoặc mặc đồ
            bool updateEquip = (modType >= (int)ModGoodsTypes.EquipLoad && modType <= (int)ModGoodsTypes.EquipUnload);

            bool isUsingChanged = (isusing != goodsData.Using);

            /// Nếu là thao tác mặc đồ hoặc tháo đồ
            if (updateEquip)
            {
                /// Nhưng lại không có sự thay đổi về vị trí thì không làm gì
                if (!isUsingChanged)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }
            }

            /// Nếu là mặc trang bị lên người
            if (modType == (int)ModGoodsTypes.EquipLoad)
            {
                /// Kiểm tra điều kiện xem có thể trang bị không
                if (!client.GetPlayEquipBody().CanUsingEquip(goodsData))
                {
                    /// Nếu không thể sử dụng return toang luôn
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (isusing < 100)
                {
                    /// Thực hiện mặc đồ vào cho nhân vật
                    if (!client.GetPlayEquipBody().EquipLoadMain(goodsData))
                    {
                        /// Nếu không thể sử dụng return toang luôn
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else
                {  /// Thực hiện mặc đồ vào cho nhân vật
                    if (!client.GetPlayEquipBody().EquipLoadSub(goodsData))
                    {
                        /// Nếu không thể sử dụng return toang luôn
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }

                /// Thông báo tới tất cả người chơi xung quanh
                KT_TCPHandler.SendPlayerEquipChangeToNearbyPlayers(client, goodsData, -1);
            }
            /// Nếu là tháo trang bị
            else if (modType == (int)ModGoodsTypes.EquipUnload)
            {
                int oldEquipSlot = goodsData.Using;

                if (isusing < 100)
                {
                    // Thực hiện mặc đồ vào cho nhân vật
                    if (!client.GetPlayEquipBody().EquipUnloadMain(goodsData))
                    {
                        // Nếu không thể sử dụng return toang luôn
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Thông báo tới tất cả người chơi xung quanh
                    KT_TCPHandler.SendPlayerEquipChangeToNearbyPlayers(client, goodsData, oldEquipSlot);
                }
                else
                {
                    // Thực hiện mặc đồ vào cho nhân vật
                    if (!client.GetPlayEquipBody().EquipUnloadSub(goodsData))
                    {
                        // Nếu không thể sử dụng return toang luôn
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
            }
            /// Nếu là vứt vật phẩm
            else if (modType == (int)ModGoodsTypes.Abandon)
            {
                ///// Tạm khóa
                //if (true)
                //{
                //    PlayerManager.ShowNotification(client, "Chức năng tạm thời khóa. Từ sau bảo trì tới, khi vứt vật phẩm sẽ không rơi xuống đất mà mất thẳng luôn, hãy chú ý!");
                //    return TCPProcessCmdResults.RESULT_OK;
                //}

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    PlayerManager.ShowNotification(client, "Ở liên máy chủ không thể vứt bỏ vật phẩm");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (goodsData.Binding == 1)
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm khóa không thể vứt bỏ");

                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (goodsData.Endtime != Global.ConstGoodsEndTime)
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm có hạn sử dụng không thể vứt bỏ");

                    return TCPProcessCmdResults.RESULT_OK;
                }
                if (goodsData.Forge_level > 0)
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm đã cường hóa không thể vứt ra");

                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Sói sửa không tạo vật phẩm rơi dưới đất để chống BUG
                if (!ItemManager.AbandonItem(goodsData, client, false, "Vứt vật phẩm"))
                {
                    PlayerManager.ShowNotification(client, "Có lỗi khi vứt vật phẩm");
                    return TCPProcessCmdResults.RESULT_OK;
                }
            }
            /// Nếu là thay đổi vị trí
            else if (modType == (int)ModGoodsTypes.ModValue)
            {
                if (!ItemManager.ItemModValue(goodsData, client, site))
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }
            }
            else if (modType == (int)ModGoodsTypes.SplitItem)
            {
                if (!KTGlobal.IsHaveSpace(1, client))
                {
                    PlayerManager.ShowNotification(client, "Túi đồ không đủ không đủ trỗ trống không thể tách");

                    return TCPProcessCmdResults.RESULT_OK;
                }
                else
                {
                    if (!ItemManager.SplitItem(goodsData, client, Int32.Parse(extraParams)))
                    {
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
            }

            return TCPProcessCmdResults.RESULT_OK;
        }

        #endregion 物品管理

        #region 物品使用限制管理

        /// <summary>
        /// 从物品限制列表中查找指定的物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static GoodsLimitData GetGoodsLimitByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsLimitDataList) return null;

            lock (client.GoodsLimitDataList)
            {
                for (int i = 0; i < client.GoodsLimitDataList.Count; i++)
                {
                    if (client.GoodsLimitDataList[i].GoodsID == goodsID)
                    {
                        return client.GoodsLimitDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 从物品限制列表中查找指定的物品的当日使用次数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetTodayGoodsLimitByID(KPlayer client, int goodsID)
        {
            if (null == client.GoodsLimitDataList) return 0;

            int usedNum = 0;
            lock (client.GoodsLimitDataList)
            {
                for (int i = 0; i < client.GoodsLimitDataList.Count; i++)
                {
                    if (client.GoodsLimitDataList[i].GoodsID == goodsID)
                    {
                        int dayID = TimeUtil.NowDateTime().DayOfYear;
                        if (dayID == client.GoodsLimitDataList[i].DayID)
                        {
                            usedNum = client.GoodsLimitDataList[i].UsedNum;
                        }

                        break;
                    }
                }
            }

            return usedNum;
        }

        /// <summary>
        /// 物品限制列表中加入指定的物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static void UpdateGoodsLimitByID(KPlayer client, GoodsLimitData goodsLimitData)
        {
            if (client.GoodsLimitDataList == null)
            {
                client.GoodsLimitDataList = new List<GoodsLimitData>();
            }

            lock (client.GoodsLimitDataList)
            {
                int findIndex = -1;
                for (int i = 0; i < client.GoodsLimitDataList.Count; i++)
                {
                    if (client.GoodsLimitDataList[i].GoodsID == goodsLimitData.GoodsID)
                    {
                        findIndex = i;

                        client.GoodsLimitDataList[i].DayID = goodsLimitData.DayID;
                        client.GoodsLimitDataList[i].UsedNum = goodsLimitData.UsedNum;

                        break;
                    }
                }

                if (-1 == findIndex)
                {
                    client.GoodsLimitDataList.Add(goodsLimitData);
                }
            }
        }

        #endregion 物品使用限制管理

        #region NPC相关管理

        /// <summary>
        /// NPC脚本的时间限制缓存
        /// </summary>
        private static Dictionary<int, DateTimeRange[]> _NPCScriptTimeLimitsDict = new Dictionary<int, DateTimeRange[]>();

        /// <summary>
        /// 获取NPC脚本的时间限制字段
        /// </summary>
        /// <param name="systemScriptItem"></param>
        /// <returns></returns>
        public static DateTimeRange[] GetNPCScriptTimeLimits(int npcScriptID, SystemXmlItem systemScriptItem)
        {
            DateTimeRange[] dateTimeRangeArray = null;
            lock (_NPCScriptTimeLimitsDict)
            {
                if (_NPCScriptTimeLimitsDict.TryGetValue(npcScriptID, out dateTimeRangeArray))
                {
                    return dateTimeRangeArray;
                }
            }

            string timeLimits = systemScriptItem.GetStringValue("TimeLimits");
            if (string.IsNullOrEmpty(timeLimits))
            {
                return null;
            }

            dateTimeRangeArray = Global.ParseDateTimeRangeStr(timeLimits);

            lock (_NPCScriptTimeLimitsDict)
            {
                _NPCScriptTimeLimitsDict[npcScriptID] = dateTimeRangeArray;
            }

            return dateTimeRangeArray;
        }

        /// <summary>
        /// 清空NPC脚本时间限制缓存
        /// </summary>
        public static void ClearNPCScriptTimeLimits()
        {
            lock (_NPCScriptTimeLimitsDict)
            {
                _NPCScriptTimeLimitsDict.Clear();
            }
        }

        /// <summary>
        /// 过滤NPC脚本
        /// </summary>
        /// <param name="npcScriptID"></param>
        /// <returns></returns>
        public static bool FilterNPCScriptByID(KPlayer client, int npcScriptID, out int errorCode)
        {
            errorCode = 0;
            if (npcScriptID <= 0)
            {
                errorCode = -11001;
                return true;
            }

            SystemXmlItem systemScriptItem = null;
            if (!GameManager.systemNPCScripts.SystemXmlItemDict.TryGetValue(npcScriptID, out systemScriptItem))
            {
                errorCode = -11002;
                return true;
            }

            int minLevel = systemScriptItem.GetIntValue("MinLevel");
            int maxLevel = systemScriptItem.GetIntValue("MaxLevel");
            if (client.m_Level < minLevel || client.m_Level > maxLevel)
            {
                errorCode = -11003;
                return true;
            }

            int sexCondition = systemScriptItem.GetIntValue("SexCondition");
            if (-1 != sexCondition && client.RoleSex != sexCondition)
            {
                errorCode = -11004;
                return true;
            }

            int occupCondition = systemScriptItem.GetIntValue("OccupCondition");

            // 属性改造 加上一级属性公式 区分职业[8/15/2013 LiaoWei]
            int nOcc = Global.CalcOriginalOccupationID(client);

            if (-1 != occupCondition && nOcc != occupCondition)
            {
                errorCode = -11005;
                return true;
            }

            DateTimeRange[] dateTimeRangeArray = Global.GetNPCScriptTimeLimits(npcScriptID, systemScriptItem);
            if (null != dateTimeRangeArray)
            {
                int endMinute = 0;
                if (!Global.JugeDateTimeInTimeRange(TimeUtil.NowDateTime(), dateTimeRangeArray, out endMinute))
                {
                    errorCode = -11006;
                    return true;
                }
            }

            int filterID = systemScriptItem.GetIntValue("FilterID");
            if (filterID <= 0)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// NPC功能ID的时间限制缓存
        /// </summary>
        private static Dictionary<int, DateTimeRange[]> _NPCOpterationTimeLimitsDict = new Dictionary<int, DateTimeRange[]>();

        /// <summary>
        /// 获取NPC功能ID的时间限制字段
        /// </summary>
        /// <param name="systemOperationItem"></param>
        /// <returns></returns>
        public static DateTimeRange[] GetNPCOperationTimeLimits(int npcOperationID, SystemXmlItem systemOperationItem)
        {
            DateTimeRange[] dateTimeRangeArray = null;
            lock (_NPCOpterationTimeLimitsDict)
            {
                if (_NPCOpterationTimeLimitsDict.TryGetValue(npcOperationID, out dateTimeRangeArray))
                {
                    return dateTimeRangeArray;
                }
            }

            string timeLimits = systemOperationItem.GetStringValue("TimeLimits");
            if (string.IsNullOrEmpty(timeLimits))
            {
                return null;
            }

            dateTimeRangeArray = Global.ParseDateTimeRangeStr(timeLimits);

            lock (_NPCOpterationTimeLimitsDict)
            {
                _NPCOpterationTimeLimitsDict[npcOperationID] = dateTimeRangeArray;
            }

            return dateTimeRangeArray;
        }

        /// <summary>
        /// 清除NPC功能时间缓存
        /// </summary>
        public static void ClearNPCOperationTimeLimits()
        {
            lock (_NPCOpterationTimeLimitsDict)
            {
                _NPCOpterationTimeLimitsDict.Clear();
            }
        }

        #endregion NPC相关管理

        #region 怪物管理

        /// <summary>
        /// 根据怪物ID获取怪物的名称
        /// </summary>
        /// <param name="monsterID"></param>
        /// <returns></returns>
        public static string GetMonsterNameByID(int monsterID)
        {
            return MonsterNameManager.GetMonsterName(monsterID);
        }

        #endregion 怪物管理

        #region 出售列表管理

        /// <summary>
        /// 根据物品DbID获取出售物品的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GoodsData GetSaleGoodsDataByDbID(KPlayer client, int id)
        {
            if (null == client.SaleGoodsDataList)
            {
                return null;
            }

            lock (client.SaleGoodsDataList)
            {
                for (int i = 0; i < client.SaleGoodsDataList.Count; i++)
                {
                    if (client.SaleGoodsDataList[i].Id == id)
                    {
                        return client.SaleGoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取已经挂售物品的数量
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetSaleGoodsDataCount(KPlayer client)
        {
            if (null == client.SaleGoodsDataList)
            {
                return 0;
            }

            int count = 0;
            lock (client.SaleGoodsDataList)
            {
                count = client.SaleGoodsDataList.Count;
            }

            return count;
        }

        /// <summary>
        /// 从出售的物品队列中删除物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public static bool RemoveSaleGoodsData(KPlayer client, GoodsData gd)
        {
            if (null == gd) return false;
            if (client.SaleGoodsDataList == null)
            {
                return false;
            }

            bool ret = false;
            lock (client.SaleGoodsDataList)
            {
                ret = client.SaleGoodsDataList.Remove(gd);
            }

            return ret;
        }

        /// <summary>
        /// 添加物品到出售的物品队列中
        /// </summary>
        /// <param name="client"></param>
        public static void AddSaleGoodsData(KPlayer client, GoodsData gd)
        {
            if (null == gd) return;
            if (client.SaleGoodsDataList == null)
            {
                client.SaleGoodsDataList = new List<GoodsData>();
            }

            lock (client.SaleGoodsDataList)
            {
                client.SaleGoodsDataList.Add(gd);
            }
        }

        #endregion 出售列表管理

        #region 任务管理

        /// <summary>
        /// Xử lý các nhiệm vụ đang giang dở hoặc từ bỏ
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessTaskData(KPlayer client)
        {
            if (null == client.TaskDataList) return;

            lock (client.TaskDataList)
            {
                for (int i = 0; i < client.TaskDataList.Count; i++)
                {
                    Global.ProcessTaskData(client, client.TaskDataList[i]);

                    OldTaskData oldTaskData = Global.FindOldTaskByTaskID(client, client.TaskDataList[i].DoingTaskID);
                    if (null != oldTaskData)
                    {
                        client.TaskDataList[i].DoneCount = oldTaskData.DoCount;
                    }
                }
            }
        }

        /// <summary>
        /// Xử lý trước dữ liệu nhiệm vụ trước khi gửi cho người dùng
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessTaskData(KPlayer client, TaskData taskData)
        {
        }

        /// <summary>
        /// Trả về danh sách nhiệm vụ đã làm trước đó
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static OldTaskData FindOldTaskByTaskID(KPlayer client, int taskID)
        {
            if (null == client.OldTasks)
            {
                return null;
            }

            lock (client.OldTasks)
            {
                for (int i = 0; i < client.OldTasks.Count; i++)
                {
                    if (taskID == client.OldTasks[i].TaskID)
                    {
                        return client.OldTasks[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 添加旧的任务
        /// </summary>
        /// <param name="taskID"></param>
        public static void AddOldTask(KPlayer client, int taskID)
        {
            if (null == client.OldTasks)
            {
                client.OldTasks = new List<OldTaskData>();
            }

            int findIndex = -1;

            lock (client.OldTasks)
            {
                for (int i = 0; i < client.OldTasks.Count; i++)
                {
                    if (client.OldTasks[i].TaskID == taskID)
                    {
                        findIndex = i;
                        break;
                    }
                }

                if (findIndex >= 0)
                {
                    client.OldTasks[findIndex].DoCount++;
                }
                else
                {
                    client.OldTasks.Add(new OldTaskData()
                    {
                        TaskID = taskID,
                        DoCount = 1,
                    });
                }
            }
        }

        /// <summary>
        /// 从用户的正在做的任务列表查找一个任务
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static TaskData GetTaskData(KPlayer client, int taskID)
        {
            if (null == client.TaskDataList) return null;

            lock (client.TaskDataList)
            {
                for (int i = 0; i < client.TaskDataList.Count; i++)
                {
                    if (client.TaskDataList[i].DoingTaskID == taskID)
                    {
                        return client.TaskDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 从用户的正在做的任务列表查找一个任务(根据数据库ID)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static TaskData GetTaskDataByDbID(KPlayer client, int taskDbID)
        {
            if (null == client.TaskDataList) return null;

            lock (client.TaskDataList)
            {
                for (int i = 0; i < client.TaskDataList.Count; i++)
                {
                    if (client.TaskDataList[i].DbID == taskDbID)
                    {
                        return client.TaskDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="npcID"></param>
        /// <returns></returns>
        public static NPCTaskState GetNPCTaskState(List<NPCTaskState> npcTaskStateList, int npcID)
        {
            if (null == npcTaskStateList) return null;
            for (int i = 0; i < npcTaskStateList.Count; i++)
            {
                if (npcTaskStateList[i].NPCID == npcID)
                {
                    return npcTaskStateList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra xem có nhận được nhiệm vụ mới không
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>

        /// <summary>
        /// Nhận thông tin về nguồn nhiệm vụ
        /// </summary>
        /// <param name="systemTask"></param>
        /// <param name="mapCode"></param>
        /// <param name="npcType"></param>
        /// <param name="npcID"></param>
        /// <returns></returns>
        public static bool GetTaskSourceNPCID(SystemXmlItem systemTask, out int mapCode, out int npcType, out int npcID)
        {
            mapCode = -1;
            npcType = (int)(GSpriteTypes.NPC);
            npcID = -1;
            int sourceNPC = systemTask.GetIntValue("SourceNPC");

            SystemXmlItem xmlNode = null;
            if (!GameManager.SystemNPCsMgr.SystemXmlItemDict.TryGetValue(sourceNPC, out xmlNode))
            {
                return false;
            }

            mapCode = xmlNode.GetIntValue("MapCode");
            npcType = (int)(GSpriteTypes.NPC);
            npcID = xmlNode.GetIntValue("ID");
            return true;
        }

        /// <summary>
        /// 获取任务的目标NPC信息
        /// </summary>
        /// <param name="systemTask"></param>
        /// <param name="mapCode"></param>
        /// <param name="npcType"></param>
        /// <param name="npcID"></param>
        /// <returns></returns>
        public static bool GetTaskDestNPCID(SystemXmlItem systemTask, out int mapCode, out int npcType, out int npcID)
        {
            mapCode = -1;
            npcType = (int)(GSpriteTypes.NPC);
            npcID = -1;
            int destNPC = systemTask.GetIntValue("DestNPC");

            SystemXmlItem xmlNode = null;
            if (!GameManager.SystemNPCsMgr.SystemXmlItemDict.TryGetValue(destNPC, out xmlNode))
            {
                return false;
            }

            mapCode = xmlNode.GetIntValue("MapCode");
            npcType = (int)(GSpriteTypes.NPC);
            npcID = xmlNode.GetIntValue("ID");
            return true;
        }

        /// <summary>
        /// 判断任务目标是否已经完成
        /// </summary>
        /// <param name="systemTask"></param>
        /// <param name="taskVal"></param>
        /// <returns></returns>
        public static bool JugeTaskTargetComplete(SystemXmlItem systemTask, int num, int taskVal)
        {
            if (systemTask.GetIntValue(string.Format("TargetNPC{0}", num)) < 0)
            {
                return true;
            }

            int targetNum = systemTask.GetIntValue(string.Format("TargetNum{0}", num));
            if (targetNum <= 0) targetNum = 1;
            return (taskVal >= targetNum);
        }

        /// <summary>
        /// 判断任务是否真的完成
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// 获取当前追踪的任务的个数
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetFocusTaskCount(KPlayer client)
        {
            int ret = 0;
            if (null == client.TaskDataList) return ret;

            lock (client.TaskDataList)
            {
                for (int i = 0; i < client.TaskDataList.Count; i++)
                {
                    if (client.TaskDataList[i].DoingTaskFocus > 0)
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gỡ bỏ task không hợp lý
        /// </summary>
        /// <param name="client"></param>
        public static bool RemoveInvalidTask(KPlayer client, TaskData taskData)
        {
            Task _Taskfind = TaskManager.getInstance().FindTaskById(taskData.DoingTaskID);

            if (_Taskfind != null)
            {
                return false;
            }

            int dbID = taskData.DbID;
            int taskID = taskData.DoingTaskID;

            //Xóa task
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_SPR_ABANDONTASK,
                string.Format("{0}:{1}:{2}", client.RoleID, dbID, taskID),
                null, client.ServerId);

            return true;
        }

        /// <summary>
        /// 过滤删除所有无效的任务
        /// </summary>
        /// <param name="client"></param>
        public static bool RemoveAllInvalidTasks(KPlayer client)
        {
            if (null == client.TaskDataList || client.TaskDataList.Count <= 0)
            {
                return false;
            }

            List<TaskData> abandonTaskList = new List<TaskData>();
            int abandonCount = 0;

            lock (client.TaskDataList)
            {
                for (int i = 0; i < client.TaskDataList.Count; i++)
                {
                    if (RemoveInvalidTask(client, client.TaskDataList[i]))
                    {
                        abandonTaskList.Add(client.TaskDataList[i]);
                        abandonCount++;
                    }
                }
            }

            //添加到角色的列表中
            for (int i = 0; i < abandonTaskList.Count; i++)
            {
                lock (client.TaskDataList)
                {
                    client.TaskDataList.Remove(abandonTaskList[i]);
                }

                LogManager.WriteLog(LogTypes.Error, string.Format("删除无效的任务, Client={0}({1}), TaskID={2}",
                    client.RoleID, client.RoleName, abandonTaskList[i].DoingTaskID));
            }

            return (abandonCount > 0);
        }

        #endregion 任务管理

        #region 日常任务管理

        /// <summary>
        /// Mu项目日常任务每日次数上限  [12/3/2013 LiaoWei]
        /// </summary>
        public const int MaxDailyTaskNumForMU = 10;

        /// <summary>
        /// Mu项目讨伐任务每日次数上限
        /// </summary>
        public static int MaxTaofaTaskNumForMU = 5;

        /// <summary>
        /// 初始化角色的各种每日环式任务
        /// </summary>
        /// <param name="client"></param>
        public static void InitRoleDailyTaskData(KPlayer client, bool isNewday)
        {
            DailyTaskData dailyTaskData = null;
        }

        #endregion 日常任务管理

        /// <summary>
        /// 从逗号隔开的物品字符串解析物品，fileName用于发生错误时记录日志
        /// </summary>
        /// <param name="batchGoods"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static List<GoodsData> ParseGoodsDataListFromGoodsStr(string batchGoods, string fileName = "")
        {
            string[] fields = batchGoods.Split('|');

            List<GoodsData> goodsDataList = new List<GoodsData>();
            for (int i = 0; i < fields.Length; i++)
            {
                string[] sa = fields[i].Split(',');
                if (sa.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析{0}中的奖励项时失败, 物品配置项个数错误", fileName));
                    continue;
                }

                int[] goodsFields = Global.StringArray2IntArray(sa);

                //获取物品数据
                GoodsData goodsData = Global.GetNewGoodsData(goodsFields[0], goodsFields[1], goodsFields[2], goodsFields[3], goodsFields[4], goodsFields[5], 0, 0);
                goodsDataList.Add(goodsData);
            }

            return goodsDataList;
        }

        /// <summary>
        /// Hàm Tăng Exp cho nhân vật
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="experience"></param>
        public static bool EarnExperience(KPlayer sprite, long experience)
        {
            long nNeedExp = 0;

            nNeedExp = KPlayerSetting.GetNeedExpUpExp(sprite.m_Level);

            // Nếu số EXP add vào quá số exp yêu cầu thì cho lên cấp
            if (sprite.m_Level < KPlayerSetting.GetMaxLevel() && sprite.m_Experience + experience >= nNeedExp)
            {
                /// Giới hạn cấp
                if (sprite.m_Level >= ServerConfig.Instance.LimitLevel)
                {
                    return false;
                }

                int oldLevel = sprite.m_Level;

                sprite.m_Level += 1;

                HuodongCachingMgr.ProcessGetUpLevelGift(sprite);

                experience = sprite.m_Experience + experience - nNeedExp;

                sprite.m_Experience = 0;

                GlobalEventSource.getInstance().fireEvent(new PlayerLevelupEventObject(sprite));

                EarnExperience(sprite, experience);

                // SEND VIDEO J ĐÓ TẠM XÓA
                // VideoLogic.GetOrSendPlayerVideoStatus(sprite, sprite.RoleCommonUseIntPamams);
            }
            else
            {
                sprite.m_Experience += (int)experience;
                sprite.m_Experience = Global.GMax(0, sprite.m_Experience);
            }

            return true;
        }

        #region Quản lý kết nối

        /// <summary>
        /// 获取GameClient的IP
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string GetSocketRemoteIP(KPlayer client, bool bForce = false)
        {
            long canRecordIp = 1;
            if (0 == canRecordIp && false == bForce)
            {
                return "";
            }

            string ipAndPort = GetSocketRemoteEndPoint(client.ClientSocket);
            int idx = ipAndPort.IndexOf(':');
            if (idx > 0)
            {
                return ipAndPort.Substring(0, idx);
            }
            else
            {
                return ipAndPort;
            }
        }

        /// <summary>
        /// Gửi packet giữ kết nối
        /// </summary>
        public static void SendGameServerHeart(TCPClient tcpClient)
        {
            if (null == tcpClient)
                return;

            string cmd = string.Format("{0}:{1}:{2}", GameManager.ServerLineID, GameManager.ClientMgr.GetClientCount(), Global.SendServerHeartCount);
            Global.SendServerHeartCount++; //为了标识是否是第一次

            //获取
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(TCPOutPacketPool.getInstance(), cmd, (int)TCPGameServerCmds.CMD_DB_ONLINE_SERVERHEART);

            //发送心跳信息
            if (null != tcpOutPacket)
            {
                byte[] bytesData = Global.SendAndRecvData(tcpClient, tcpOutPacket);
            }
        }

        public static string GetSocketRemoteEndPoint(TMSKSocket s, bool bForce = false)
        {
            try
            {
                long canRecordIp = 1;
                if (0 == canRecordIp && false == bForce)
                {
                    return "";
                }

                if (null == s)
                {
                    return "";
                }

                return string.Format("{0} ", s.RemoteEndPoint);
            }
            catch (Exception)
            {
            }

            return "";
        }

        /// <summary>
        /// 获取TMSKSocket的远端IP地址(不带端口)
        /// </summary>
        public static string GetIPAddress(TMSKSocket s)
        {
            try
            {
                if (null == s) return "";
                return ((IPEndPoint)s.RemoteEndPoint).Address.ToString();
            }
            catch (Exception)
            {
            }

            return "";
        }

        public static string GetDebugHelperInfo(TMSKSocket socket)
        {
            if (null == socket)
            {
                return "socket为null, 无法打印错误信息";
            }

            string ret = "";
            try
            {
                ret += string.Format("IP={0} ", GetSocketRemoteEndPoint(socket));
            }
            catch (Exception)
            {
            }

            try
            {
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null != client)
                {
                    ret += string.Format("RoleID={0}({1})", client.RoleID, client.RoleName);
                }
            }
            catch (Exception)
            {
            }

            return ret;
        }

        #endregion Quản lý kết nối

        #region Giao dịch

        /// <summary>
        /// Khôi phục lại đồ vào túi đồ cho thằng hủy giao dịch
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static void RestoreExchangeData(KPlayer client, ExchangeData ed)
        {
            lock (ed)
            {
                List<GoodsData> goodsDataList = null;

                if (ed.GoodsDict.TryGetValue(client.RoleID, out goodsDataList))
                {
                    for (int i = 0; i < goodsDataList.Count; i++)
                    {
                        Global.AddGoodsData(client, goodsDataList[i]);

                        GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, goodsDataList[i], 1);
                    }

                    ed.GoodsDict.Remove(client.RoleID);
                }
            }
        }

        /// <summary>
        /// Khóa giao dịch
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        /// <param name="locked"></param>
        public static void LockExchangeData(int roleID, ExchangeData ed, int locked)
        {
            lock (ed)
            {
                ed.LockDict[roleID] = locked;
            }
        }

        /// <summary>
        /// Kiểm tra xem đã kháo giao dịch chưa
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static bool IsLockExchangeData(int roleID, ExchangeData ed)
        {
            int locked = 0;
            lock (ed)
            {
                ed.LockDict.TryGetValue(roleID, out locked);
            }

            return (locked > 0);
        }

        /// <summary>
        /// 设置交易数据的同意状态
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        /// <param name="done"></param>
        public static bool DoneExchangeData(int roleID, ExchangeData ed)
        {
            bool ret = false;
            lock (ed)
            {
                if (!ed.DoneDict.ContainsKey(roleID))
                {
                    ed.DoneDict[roleID] = 1;
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 查询交易数据的同意状态
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static bool IsDoneExchangeData(int roleID, ExchangeData ed)
        {
            int done = 0;
            lock (ed)
            {
                ed.DoneDict.TryGetValue(roleID, out done);
            }

            return (done > 0);
        }

        /// <summary>
        /// Add đồ vào ô giao dịch
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool AddGoodsDataIntoExchangeData(KPlayer client, int goodsDbID, ExchangeData ed)
        {
            //Check xem đã lock chưa
            if (IsLockExchangeData(client.RoleID, ed))
            {
                return true;
            }

            lock (ed)
            {
                List<GoodsData> goodsDataList = null;
                if (!ed.GoodsDict.TryGetValue(client.RoleID, out goodsDataList))
                {
                    goodsDataList = new List<GoodsData>();
                    ed.GoodsDict[client.RoleID] = goodsDataList;
                }

                // Nếu chưa kín 12 vật phẩm
                if (goodsDataList.Count < 10)
                {
                    GoodsData gd = Global.GetGoodsByDbID(client, goodsDbID);
                    if (null == gd) return false;
                    if (gd.Binding > 0) return false; //Nếu mà vật phẩm khóa thì cút
                    if (Global.IsTimeLimitGoods(gd)) return false; //Nếu hết thời gian giao dịch thì cút
                    Global.RemoveGoodsData(client, gd);

                    // Notfify về client move GOOD vào lưới
                    GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, gd, 0);

                    if (-1 == goodsDataList.IndexOf(gd))
                    {
                        goodsDataList.Add(gd);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 将制定的物品从交易数据中删除
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool RemoveGoodsDataFromExchangeData(KPlayer client, int goodsDbID, ExchangeData ed)
        {
            //查询交易数据的锁定状态
            if (IsLockExchangeData(client.RoleID, ed))
            {
                return true;
            }

            GoodsData gd = null;
            lock (ed)
            {
                List<GoodsData> goodsDataList = null;
                if (ed.GoodsDict.TryGetValue(client.RoleID, out goodsDataList))
                {
                    for (int i = 0; i < goodsDataList.Count; i++)
                    {
                        if (goodsDataList[i].Id == goodsDbID)
                        {
                            gd = goodsDataList[i];
                            goodsDataList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            if (null == gd) return false;
            Global.AddGoodsData(client, gd);
            GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, gd, 1);

            return true;
        }

        /// <summary>
        /// Update tiền vào phiên giao dịch
        /// </summary>
        /// <param name="client"></param>
        /// <param name="money"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool UpdateExchangeDataMoney(KPlayer client, int money, ExchangeData ed)
        {
            //Nếu đã khóa rồi
            if (IsLockExchangeData(client.RoleID, ed))
            {
                return true;
            }

            lock (ed)
            {
                ed.MoneyDict[client.RoleID] = money;
            }

            return true;
        }

        /// <summary>
        /// 将制定的元宝放入交易数据中
        /// </summary>
        /// <param name="client"></param>
        /// <param name="money"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool UpdateExchangeDataYuanBao(KPlayer client, int yuanBao, ExchangeData ed)
        {
            //查询交易数据的锁定状态
            if (IsLockExchangeData(client.RoleID, ed))
            {
                return true;
            }

            lock (ed)
            {
                ed.YuanBaoDict[client.RoleID] = yuanBao;
            }

            return true;
        }

        private static string BuildTradeAnalysisLog(
            KPlayer from, KPlayer to,
            List<GoodsData> outGoods, List<GoodsData> inGoods,
            int outMoney, int inMoney,
            int outJinbi, int inJinbi)
        {
            // 统计分析 -100 金币，-101 钻石
            Dictionary<int, int> outDict = new Dictionary<int, int>();
            Dictionary<int, int> inDict = new Dictionary<int, int>();

            for (int i = 0; outGoods != null && i < outGoods.Count; ++i)
            {
                if (!outDict.ContainsKey(outGoods[i].GoodsID))
                    outDict.Add(outGoods[i].GoodsID, outGoods[i].GCount);
                else
                    outDict[outGoods[i].GoodsID] += outGoods[i].GCount;
            }

            for (int i = 0; inGoods != null && i < inGoods.Count; ++i)
            {
                if (!inDict.ContainsKey(inGoods[i].GoodsID))
                    inDict.Add(inGoods[i].GoodsID, inGoods[i].GCount);
                else
                    inDict[inGoods[i].GoodsID] += inGoods[i].GCount;
            }

            if (outJinbi > 0) outDict[-100] = outJinbi;
            if (inJinbi > 0) inDict[-100] = inJinbi;
            if (outMoney > 0) outDict[-101] = outMoney;
            if (inMoney > 0) inDict[-101] = inMoney;

            StringBuilder inSb = new StringBuilder(), outSb = new StringBuilder();
            foreach (var kvp in inDict)
            {
                inSb.Append(kvp.Key).Append(':').Append(kvp.Value).Append(',');
            }
            if (inSb.Length > 0) inSb.Remove(inSb.Length - 1, 1);

            foreach (var kvp in outDict)
            {
                outSb.Append(kvp.Key).Append(':').Append(kvp.Value).Append(',');
            }
            if (outSb.Length > 0) outSb.Remove(outSb.Length - 1, 1);

            FriendData fd = Global.FindFriendData(from, to.RoleID);
            int isFriend = 0;
            if (fd != null && fd.FriendType == 0)
                isFriend = 100;

            string sip = RobotTaskValidator.getInstance().GetIp(from);
            string tip = RobotTaskValidator.getInstance().GetIp(to);

            string analysisLog = string.Format("server={0} source={1} srcPlayer={2} target={3} dstPlayer={4} in={5} out={6} map={7} sviplevel={8} tviplevel={9} sexp={10} texp={11} friendDegree={12}",
                GameManager.ServerId, from.strUserID, from.RoleID, to.strUserID, to.RoleID, inSb.ToString(), outSb.ToString(), from.MapCode,
                0, 0, sip, tip, isFriend);

            return analysisLog;
        }

        /// <summary>
        /// Hàm thực hiện giao dịch cho 2 thằng dựa trên dữ liệu phiên giao dịch
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static int CompleteExchangeData(KPlayer client, KPlayer otherClient, ExchangeData ed)
        {
            int ret = 0;
            lock (ed)
            {
                List<GoodsData> goodsDataList1 = null;

                if (ed.GoodsDict.TryGetValue(client.RoleID, out goodsDataList1))
                {
                    // Kiểm tra xem có thể lấy đồ của thằng đầu tiên add cho thằng thứu 2 không
                    if (!Global.CanAddGoodsDataList(otherClient, goodsDataList1))
                    {
                        return -1;
                    }
                }

                List<GoodsData> goodsDataList2 = null;
                if (ed.GoodsDict.TryGetValue(otherClient.RoleID, out goodsDataList2))
                {
                    // Kiểm tra xem có thể lấy đồ của thằng thứ 2 add cho thằng thứ 1 không
                    if (!Global.CanAddGoodsDataList(client, goodsDataList2))
                    {
                        return -11;
                    }
                }

                int moveMoney = 0;
                if (ed.MoneyDict.TryGetValue(client.RoleID, out moveMoney))
                {
                    moveMoney = Global.GMax(moveMoney, 0);
                    if (moveMoney > client.Money)
                    {
                        return -2;
                    }
                }

                int moveMoney2 = 0;
                if (ed.MoneyDict.TryGetValue(otherClient.RoleID, out moveMoney2))
                {
                    moveMoney2 = Global.GMax(moveMoney2, 0);
                    if (moveMoney2 > otherClient.Money)
                    {
                        return -12;
                    }
                }

                try
                {
                    string analysisLog = BuildTradeAnalysisLog(client, otherClient, goodsDataList1, goodsDataList2, 0, 0, moveMoney, moveMoney2);

                    analysisLog = "[TRADE] " + analysisLog;

                    LogManager.WriteLog(LogTypes.Trade, analysisLog);
                }
                catch { }

                for (int i = 0; goodsDataList1 != null && i < goodsDataList1.Count; i++)
                {
                    string result = "[Giao dịch thành công]";

                    if (!GameManager.ClientMgr.MoveGoodsDataToOtherRole(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool,
                        goodsDataList1[i], client, otherClient, true, "TRADE", ed.ExchangeID))
                    {
                        GameManager.SystemServerEvents.AddEvent(string.Format("[TRADE] không thể chuyển vật phẩm, FromRole={0}({1}), ToRole={2}({3}), GoodsDbID={4}, GoodsID={5}, GoodsNum={6}",
                            client.RoleID, client.RoleName, otherClient.RoleID, otherClient.RoleName,
                            goodsDataList1[i].Id,
                            goodsDataList1[i].GoodsID,
                            goodsDataList1[i].GCount
                            ),
                            EventLevels.Important);

                        result = "[Thất bại]";
                    }

                    //Ghi lại nhật ký giao dịch
                    Global.AddRoleExchangeEvent1(client, goodsDataList1[i].GoodsID, -goodsDataList1[i].GCount, otherClient.RoleID, otherClient.RoleName, result);

                    //Ghi lại nhật ký giao dịch
                    Global.AddRoleExchangeEvent1(otherClient, goodsDataList1[i].GoodsID, goodsDataList1[i].GCount, client.RoleID, client.RoleName, result);
                }

                ed.GoodsDict.Remove(client.RoleID);

                for (int i = 0; goodsDataList2 != null && i < goodsDataList2.Count; i++)
                {
                    string result = "[Giao dịch thành công]";
                    if (!GameManager.ClientMgr.MoveGoodsDataToOtherRole(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool,
                        goodsDataList2[i], otherClient, client))
                    {
                        GameManager.SystemServerEvents.AddEvent(string.Format("[TRADE] không thể chuyển vật phẩm, FromRole={0}({1}), ToRole={2}({3}), GoodsDbID={4}, GoodsID={5}, GoodsNum={6}",
                            otherClient.RoleID, otherClient.RoleName, client.RoleID, client.RoleName,
                            goodsDataList2[i].Id,
                            goodsDataList2[i].GoodsID,
                            goodsDataList2[i].GCount
                            ),
                            EventLevels.Important);

                        result = "[Thất bại]";
                    }

                    //Ghi lại nhật ký giao dịch
                    Global.AddRoleExchangeEvent1(otherClient, goodsDataList2[i].GoodsID, -goodsDataList2[i].GCount, client.RoleID, client.RoleName, result);

                    //Ghi lại nhật ký giao dịch
                    Global.AddRoleExchangeEvent1(client, goodsDataList2[i].GoodsID, goodsDataList2[i].GCount, otherClient.RoleID, otherClient.RoleName, result);
                }
                ed.GoodsDict.Remove(otherClient.RoleID);

                if (moveMoney > 0)
                {
                    // Trừ bạc của thằng này
                    if (GameManager.ClientMgr.SubMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, client, moveMoney, "TRADE", true, ed.ExchangeID))
                    {
                        /// Bạc sau khi trừ thuế 10%
                        double nMoney = moveMoney * 0.9;

                        // Add bạc cho thằng kia
                        GameManager.ClientMgr.AddMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, otherClient, (int)nMoney, "TRADE", true, true, ed.ExchangeID);

                        ed.MoneyDict.Remove(client.RoleID);
                    }
                }

                if (moveMoney2 > 0)
                {  // Trừ bạc của thằng này
                    if (GameManager.ClientMgr.SubMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, otherClient, moveMoney2, "TRADE", true, ed.ExchangeID))
                    {
                        /// Bạc sau khi trừ thuế 10%
                        double nMoney = moveMoney2 * 0.9;

                        // Add bạc cho thằng kia
                        GameManager.ClientMgr.AddMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, client, (int)nMoney, "TRADE", true, true, ed.ExchangeID);

                        ed.MoneyDict.Remove(otherClient.RoleID);
                    }
                }

                //TradeBlackManager.Instance().OnExchange(client.RoleID, otherClient.RoleID, goodsDataList1, goodsDataList2, 0, moveMoney2);

                //int addTradeCount = 0;

                //int[] logTradeGoodsArr = null;

                //if (logTradeGoodsArr != null && logTradeGoodsArr.Count() > 0)
                //{
                //    if (goodsDataList1 != null)
                //        addTradeCount += goodsDataList1.Count(_g => logTradeGoodsArr.Contains(_g.GoodsID));
                //    if (goodsDataList2 != null)
                //        addTradeCount += goodsDataList2.Count(_g => logTradeGoodsArr.Contains(_g.GoodsID));
                //}

                //if (addTradeCount > 0)
                //{
                //    // Ghi lại tần xuất giao dịch
                //    int freqNumber = Global.IncreaseTradeCount(client, RoleParamName.FTFTradeDayID, RoleParamName.FTFTradeCount, addTradeCount);
                //    int tradelog_freq_ftf = GameManager.GameConfigMgr.GetGameConfigItemInt(GameConfigNames.tradelog_freq_ftf, 10);
                //    if (freqNumber >= tradelog_freq_ftf)
                //    {
                //        // GameManager.logDBCmdMgr.AddTradeFreqInfo(1, freqNumber, client.RoleID);
                //    }

                //    freqNumber = Global.IncreaseTradeCount(otherClient, RoleParamName.FTFTradeDayID, RoleParamName.FTFTradeCount, addTradeCount);
                //    if (freqNumber >= tradelog_freq_ftf)
                //    {
                //        //  GameManager.logDBCmdMgr.AddTradeFreqInfo(1, freqNumber, otherClient.RoleID);
                //    }
                //}
            }

            return ret;
        }

        #endregion Giao dịch

        #region Bày bán

        /// <summary>
        /// 从摆摊数据中恢复自己的数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static void RestoreStallData(KPlayer client, StallData sd)
        {
            lock (sd)
            {
                for (int i = 0; i < sd.GoodsList.Count; i++)
                {
                    Global.AddGoodsData(client, sd.GoodsList[i]);
                    GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, sd.GoodsList[i], 1);
                }

                sd.GoodsList.Clear();
            }
        }

        /// <summary>
        /// Add vật phẩm vào khung bày bán
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool AddGoodsDataIntoStallData(KPlayer client, int goodsDbID, StallData sd, int price)
        {
            lock (sd)
            {
                //if (sd.Start > 0)
                //{
                //    return true;
                //}

                if (sd.GoodsList.Count < 18)
                {
                    GoodsData gd = Global.GetGoodsByDbID(client, goodsDbID);
                    if (null == gd) return false;
                    if (gd.Binding > 0) return false; //Nếu mà khóa thì thôi
                    Global.RemoveGoodsData(client, gd);
                    GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, gd, 0);

                    if (-1 == sd.GoodsList.IndexOf(gd))
                    {
                        sd.GoodsList.Add(gd);
                    }

                    sd.GoodsPriceDict[gd.Id] = price;
                }
            }

            return true;
        }

        /// <summary>
        /// Xóa vật phẩm khỏi sạp hàng
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static bool RemoveGoodsDataFromStallData(KPlayer client, int goodsDbID, StallData sd)
        {
            GoodsData gd = null;
            lock (sd)
            {
                //if (sd.Start > 0)
                //{
                //    return true;
                //}

                for (int i = 0; i < sd.GoodsList.Count; i++)
                {
                    if (sd.GoodsList[i].Id == goodsDbID)
                    {
                        gd = sd.GoodsList[i];
                        sd.GoodsList.RemoveAt(i);
                        sd.GoodsPriceDict.Remove(gd.Id);
                        break;
                    }
                }
            }

            if (null == gd) return false;
            Global.AddGoodsData(client, gd);
            GameManager.ClientMgr.NotifyMoveGoods(_TCPManager.MySocketListener, _TCPManager.TcpOutPacketPool, client, gd, 1);

            return true;
        }

        /// <summary>
        /// Mua từ người chơi khác
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ed"></param>
        public static int BuyFromStallData(KPlayer client, KPlayer otherClient, StallData sd, int goodsDbID)
        {
            lock (sd)
            {
                if (sd.GoodsList.Count <= 0)
                {
                    return -11;
                }

                int goodsPrice = 0;
                if (sd.GoodsPriceDict.TryGetValue(goodsDbID, out goodsPrice))
                {
                    goodsPrice = Global.GMax(goodsPrice, 0);
                }

                int ret = -12;
                bool found = false;
                for (int i = 0; i < sd.GoodsList.Count; i++)
                {
                    if (sd.GoodsList[i].Id == goodsDbID)
                    {
                        if (goodsPrice > 0)
                        {
                            if (client.Money - goodsPrice < 0) //Nếu thằng mua ko đủ tiền
                            {
                                ret = -13;
                                break;
                            }

                            // Thực hiện trừ bạc của thằng mua và add cho thằng bán
                            GameManager.ClientMgr.SubMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, client, goodsPrice, "[TRỪ TIỀN TỪ VIỆC MUA VẬT PHẨM]");

                            GameManager.ClientMgr.AddMoney(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool, otherClient, (int)(goodsPrice * 0.90), "[NHẬN TIỀN TỪ VIỆC BÁN VẬT PHẨM]");
                        }

                        //Thực hiện move good data cho thằng mua
                        if (!GameManager.ClientMgr.MoveGoodsDataToOtherRole(_TCPManager.MySocketListener, _TCPManager.tcpClientPool, _TCPManager.TcpOutPacketPool,
                            sd.GoodsList[i], otherClient, client, true, "BUYFROMOTHER", -1))
                        {
                            GameManager.SystemServerEvents.AddEvent(string.Format("Mua vật phẩm thành công, FromRole={0}({1}), ToRole={2}({3}), GoodsDbID={4}, GoodsID={5}, GoodsNum={6}",
                                otherClient.RoleID, otherClient.RoleName, client.RoleID, client.RoleName,
                                sd.GoodsList[i].Id,
                                sd.GoodsList[i].GoodsID,
                                sd.GoodsList[i].GCount
                                ),
                                EventLevels.Important);
                        }

                        found = true;
                        sd.GoodsList.RemoveAt(i);
                        break;
                    }
                }

                if (!found)
                {
                    return ret;
                }

                return 0;
            }
        }

        #endregion Bày bán

        #region Quản lý bạn bè

        /// <summary>
        /// Tìm bạn bè trong danh sách bạn có ID tương ứng
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FriendData FindFriendData(KPlayer client, int otherRoleID)
        {
            if (null == client.FriendDataList) return null;
            lock (client.FriendDataList)
            {
                for (int i = 0; i < client.FriendDataList.Count; i++)
                {
                    if (client.FriendDataList[i].OtherRoleID == otherRoleID)
                    {
                        return client.FriendDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tìm bạn bè trong danh sách bạn có ID tương ứng
        /// </summary>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public static FriendData GetFriendData(KPlayer client, int dbID)
        {
            if (null == client.FriendDataList) return null;
            lock (client.FriendDataList)
            {
                for (int i = 0; i < client.FriendDataList.Count; i++)
                {
                    if (client.FriendDataList[i].DbID == dbID)
                    {
                        return client.FriendDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tìm bạn bè đầu tiên trong danh sách có loại tương ứng
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FriendData FindFirstFriendDataByType(KPlayer client, int friendType)
        {
            if (null == client.FriendDataList) return null;
            lock (client.FriendDataList)
            {
                for (int i = 0; i < client.FriendDataList.Count; i++)
                {
                    if (client.FriendDataList[i].FriendType == friendType)
                    {
                        return client.FriendDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Có phải người chơi có ID tương ứng nằm trong danh sách đen không
        /// </summary>
        /// <param name="client"></param>
        /// <param name="otherRoleID"></param>
        /// <returns></returns>
        public static bool InFriendsBlackList(KPlayer client, int otherRoleID)
        {
            FriendData friendData = Global.FindFriendData(client, otherRoleID);
            if (null == friendData) return false;
            if (friendData.FriendType == 1) //在黑名单中
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Thêm bạn bè
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void AddFriendData(KPlayer client, FriendData friendData)
        {
            if (null == client.FriendDataList)
            {
                client.FriendDataList = new List<FriendData>();
            }

            lock (client.FriendDataList)
            {
                client.FriendDataList.Add(friendData);
            }
        }

        /// <summary>
        /// Xóa bạn bè
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void RemoveFriendData(KPlayer client, int dbID)
        {
            if (null == client.FriendDataList) return;
            lock (client.FriendDataList)
            {
                for (int i = 0; i < client.FriendDataList.Count; i++)
                {
                    if (client.FriendDataList[i].DbID == dbID)
                    {
                        client.FriendDataList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Dánh sách bạn của thằng người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="friendType"></param>
        /// <returns></returns>
        public static int GetFriendCountByType(KPlayer client, int friendType)
        {
            if (null == client.FriendDataList) return 0;

            int totalCount = 0;
            lock (client.FriendDataList)
            {
                for (int i = 0; i < client.FriendDataList.Count; i++)
                {
                    if (client.FriendDataList[i].FriendType == friendType)
                    {
                        totalCount++;
                    }
                }
            }

            return totalCount;
        }

        #endregion Quản lý bạn bè

        #region 移动仓库物品管理

        /// <summary>
        /// 移动仓库最大的格子数---正好两页
        /// </summary>
        public static int MaxPortableGridNum = 120; // MU改成120 [3/13/2014 LiaoWei]

        /// <summary>
        /// 扩展单个移动仓库格子需要的元宝，固定每个格子50元宝，这个配置文件可以配置
        /// </summary>
        public static int OnePortableGridYuanBao = 5;

        /// <summary>
        /// 随身背包最大的格子数量 正好两页 每页36个,角色创建时默认开启48个
        /// </summary>
        public static int MaxBagGridNum = 100;

        /// <summary>
        /// 新角色默认开启50个背包格子
        /// </summary>
        public static int DefaultBagGridNum = 50;

        /// <summary>
        /// 新角色默认开启42个移动仓库格子
        /// </summary>
        public static int DefaultPortableGridNum = 60;  // MU改成60

        /// <summary>
        /// 扩展单个背包格子需要的元宝基数，没增加一个格子序号，消耗元宝加一个基数
        /// </summary>
        public static int OneBagGridYuanBao = 10;

        /// <summary>
        /// 金蛋仓库的最大格子数 40 * 20,正好20页
        /// </summary>
        public static int MaxJinDanGridNum = 240;   // MU 改成240

        /// <summary>
        /// 精灵装备栏，最大格子数
        /// </summary>
        public static int MaxDamonGridNum = 4;   // MU 改成4

        //每钻石对应多少开启时间
        public const int OpenGridSecondPerYuanBao = 300;

        // 增加接口开启仓库背包格子 [3/13/2014 LiaoWei]
        /// <summary>
        /// 取得需要的钻石
        /// </summary>
        /// <param name="addNum"></param>
        /// <returns></returns>
        public static int GetExtBagGridNeedYuanBaoForStorage(KPlayer client, int addNum)
        {
            int bagCapacity = Global.GetPortableBagCapacity(client);

            //扩展起始格子 和 结束格子
            int extStartGrid = bagCapacity + 1;
            int extEndGrid = bagCapacity + addNum;

            int needYuanBao = 0;
            //循环叠加每个格子需要的元宝
            for (int pos = extStartGrid; pos <= extEndGrid; pos++)
            {
                needYuanBao += GetOneBagGridExtendNeedYuanBaoForStorage(pos);//元宝数量是随位置叠加的，越往后开，基数越大
            }

            return needYuanBao;
        }

        /// <summary>
        /// 取得开启单个仓库格子需要的钻石
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int GetOneBagGridExtendNeedYuanBaoForStorage(int extendPos)
        {
            int needYuanBao = (extendPos - DefaultPortableGridNum) * OnePortableGridYuanBao; // 元宝数量是随位置叠加的，越往后开，基数越大,最多不超过1000元宝
            if (needYuanBao > 10 * OnePortableGridYuanBao)
            {
                needYuanBao = 10 * OnePortableGridYuanBao;
            }

            return needYuanBao;
        }

        /// <summary>
        /// 使用元宝扩展移动仓库
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="addGridNum"></param>
        /// <returns></returns>
        public static int ExtGridPortableBagWithYuanBao(TCPOutPacketPool pool, KPlayer client, int addGridNum, int nUseZuanShi)
        {
            int bagCapacity = Global.GetPortableBagCapacity(client);
            if (bagCapacity >= MaxPortableGridNum)
            {
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("随身仓库最大格子数为{0}, 你的最大格子数已满"), MaxPortableGridNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                return -1;
            }

            addGridNum = Global.Clamp(addGridNum, 0, MaxPortableGridNum - bagCapacity);
            if (addGridNum <= 0)
            {
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("格子扩展数量{0}非法"), addGridNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                return -2;
            }

            //扩展单个格子需要的金币数量 * 扩展数量
            int needYuanBao = GetExtBagGridNeedYuanBaoForStorage(client, addGridNum) - 1;//OnePortableGridYuanBao * addGridNum;

            // 在线时长能够让玩家少花钱 [3/13/2014 LiaoWei]
            needYuanBao -= client.OpenPortableGridTime / Global.OpenGridSecondPerYuanBao;

            //扣除元宝
            if (needYuanBao > 0)
            {
                if (nUseZuanShi >= 0 && nUseZuanShi < needYuanBao)
                {
                    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_QUERYOPENPORTABLEGRIDTICK, string.Format("{0}", client.OpenPortableGridTime));
                    return -2;
                }

                //自动扣除元宝
                //先DBServer请求扣费
                //扣除用户点卷
                if (!GameManager.ClientMgr.SubToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, needYuanBao, "扩充移动仓库"))
                {
                    GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("格子扩展所需钻石不足")), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.NoZuanShi);
                    return -170;
                }

                //必须花费元宝才能扩展背包
                client.MyPortableBagData.ExtGridNum += addGridNum;

                // 时间重置
                client.OpenPortableGridTime = 0;

                Global.SaveRoleParamsInt64ValueToDB(client, RoleParamName.OpenPortableGridTick, client.OpenPortableGridTime, true);

                //通知数据库修改
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEPBINFO, string.Format("{0}:{1}", client.RoleID, client.MyPortableBagData.ExtGridNum), null, client.ServerId);

                //将新的随身仓库数据通知自己
                GameManager.ClientMgr.NotifyPortableBagData(client);
            }

            return 1;
        }

        /// <summary>
        /// 根据物品DbID获取移动仓库物品的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GoodsData GetPortableGoodsDataByDbID(KPlayer client, int id)
        {
            if (null == client.PortableGoodsDataList)
            {
                return null;
            }

            lock (client.PortableGoodsDataList)
            {
                for (int i = 0; i < client.PortableGoodsDataList.Count; i++)
                {
                    if (client.PortableGoodsDataList[i].Id == id)
                    {
                        return client.PortableGoodsDataList[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 添加移动仓库物品
        /// </summary>
        /// <param name="goodsData"></param>
        public static void AddPortableGoodsData(KPlayer client, GoodsData goodsData)
        {
            if (goodsData.Site != (int)SaleGoodsConsts.PortableGoodsID) return;

            /// 改变内存中对应的物品的数量
            Global.UpdatePortableGoodsNum(client, 1);

            if (null == client.PortableGoodsDataList)
            {
                client.PortableGoodsDataList = new List<GoodsData>();
            }

            lock (client.PortableGoodsDataList)
            {
                client.PortableGoodsDataList.Add(goodsData);
            }
        }

        /// <summary>
        /// 添加物品到移动仓库队列中
        /// </summary>
        /// <param name="client"></param>
        public static GoodsData AddPortableGoodsData(KPlayer client, int id, int goodsID, int forgeLevel, int quality, int goodsNum, int binding, int site, string jewelList, string endTime,
            int addPropIndex, int bornIndex, int lucky, int strong, int ExcellenceProperty, int nAppendPropLev, int nEquipChangeLife)
        {
            GoodsData gd = new GoodsData()
            {
                Id = id,
                GoodsID = goodsID,
                Using = 0,
                Forge_level = forgeLevel,
                Starttime = "1900-01-01 12:00:00",
                Endtime = endTime,
                Site = site,

                Props = "",
                GCount = goodsNum,
                Binding = binding,

                BagIndex = 0,

                Strong = strong,
            };

            Global.AddPortableGoodsData(client, gd);
            return gd;
        }

        /// <summary>
        /// Thực hiện xóa vật phẩm ở thủ khố
        /// </summary>
        /// <param name="goodsData"></param>
        public static void RemovePortableGoodsData(KPlayer client, GoodsData goodsData)
        {
            Global.UpdatePortableGoodsNum(client, -1);

            if (null == client.PortableGoodsDataList) return;

            lock (client.PortableGoodsDataList)
            {
                client.PortableGoodsDataList.Remove(goodsData);
            }
        }

        /// <summary>
        /// 移动仓库是否已经满？是否可以添加指定的物品
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static bool CanPortableAddGoods(KPlayer client, int goodsID, int newGoodsNum, int binding)
        {
            int totalGridNum = client.MyPortableBagData.GoodsUsedGridNum;
            int totalMaxGridCount = Global.GetPortableBagCapacity(client);
            return (totalGridNum < totalMaxGridCount);
        }

        /// <summary>
        /// 改变内存中宠物对应的物品的数量
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="dbID"></param>
        /// <param name="added"></param>
        public static void UpdatePortableGoodsNum(KPlayer client, int addNum)
        {
            //addNum 可以是负数
            client.MyPortableBagData.GoodsUsedGridNum += addNum;
        }

        /// <summary>
        /// 获取移动仓库的容量
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public static int GetPortableBagCapacity(KPlayer client)
        {
            return client.MyPortableBagData.ExtGridNum;
        }

        /// <summary>
        /// 获取随身背包的容量
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public static int GetSelfBagCapacity(KPlayer client)
        {
            return client.BagNum;
        }

        /// <summary>
        /// 整理用户的移动仓库
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static void ResetPortableBagAllGoods(KPlayer client)
        {
            if (null != client.PortableGoodsDataList)
            {
                lock (client.PortableGoodsDataList)
                {
                    Dictionary<string, GoodsData> oldGoodsDict = new Dictionary<string, GoodsData>();
                    List<GoodsData> toRemovedGoodsDataList = new List<GoodsData>();
                    for (int i = 0; i < client.PortableGoodsDataList.Count; i++)
                    {
                        if (client.PortableGoodsDataList[i].Using > 0)
                        {
                            continue;
                        }

                        client.PortableGoodsDataList[i].BagIndex = 1;
                        int gridNum = Global.GetGoodsGridNumByID(client.PortableGoodsDataList[i].GoodsID);
                        if (gridNum <= 1)
                        {
                            continue;
                        }

                        GoodsData oldGoodsData = null;
                        string key = string.Format("{0}_{1}_{2}", client.PortableGoodsDataList[i].GoodsID,
                            client.PortableGoodsDataList[i].Binding, Global.DateTimeTicks(client.PortableGoodsDataList[i].Endtime));
                        if (oldGoodsDict.TryGetValue(key, out oldGoodsData))
                        {
                            int toAddNum = Global.GMin((gridNum - oldGoodsData.GCount), client.PortableGoodsDataList[i].GCount);

                            oldGoodsData.GCount += toAddNum;

                            client.PortableGoodsDataList[i].GCount -= toAddNum;
                            client.PortableGoodsDataList[i].BagIndex = 1;
                            oldGoodsData.BagIndex = 1;
                            if (!ResetBagGoodsData(client, client.PortableGoodsDataList[i]))
                            {
                                //出错, 停止整理
                                break;
                            }

                            if (oldGoodsData.GCount >= gridNum) //旧的物品已经加满
                            {
                                if (client.PortableGoodsDataList[i].GCount > 0)
                                {
                                    oldGoodsDict[key] = client.PortableGoodsDataList[i];
                                }
                                else
                                {
                                    oldGoodsDict.Remove(key);
                                    toRemovedGoodsDataList.Add(client.PortableGoodsDataList[i]);
                                }
                            }
                            else
                            {
                                if (client.PortableGoodsDataList[i].GCount <= 0)
                                {
                                    toRemovedGoodsDataList.Add(client.PortableGoodsDataList[i]);
                                }
                            }
                        }
                        else
                        {
                            oldGoodsDict[key] = client.PortableGoodsDataList[i];
                        }
                    }

                    for (int i = 0; i < toRemovedGoodsDataList.Count; i++)
                    {
                        client.PortableGoodsDataList.Remove(toRemovedGoodsDataList[i]);
                    }

                    //按照物品分类排序
                    client.PortableGoodsDataList.Sort(delegate (GoodsData x, GoodsData y)
                    {
                        //return (Global.GetGoodsCatetoriy(y.GoodsID) - Global.GetGoodsCatetoriy(x.GoodsID));
                        return (y.GoodsID - x.GoodsID);
                    });

                    int index = 0;
                    for (int i = 0; i < client.PortableGoodsDataList.Count; i++)
                    {
                        if (client.PortableGoodsDataList[i].Using > 0)
                        {
                            continue;
                        }

                        if (false && GameManager.Flag_OptimizationBagReset)
                        {
                            bool godosCountChanged = client.PortableGoodsDataList[i].BagIndex > 0;
                            client.PortableGoodsDataList[i].BagIndex = index++;
                            if (godosCountChanged)
                            {
                                if (!Global.ResetBagGoodsData(client, client.PortableGoodsDataList[i]))
                                {
                                    //出错, 停止整理
                                    break;
                                }
                            }
                        }
                        else
                        {
                            client.PortableGoodsDataList[i].BagIndex = index++;
                            if (!Global.ResetBagGoodsData(client, client.PortableGoodsDataList[i]))
                            {
                                //出错, 停止整理
                                break;
                            }
                        }
                    }
                }
            }

            TCPOutPacket tcpOutPacket = null;

            if (null != client.PortableGoodsDataList)
            {
                //先锁定
                lock (client.PortableGoodsDataList)
                {
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<GoodsData>>(client.PortableGoodsDataList, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_RESETPORTABLEBAG);
                }
            }
            else
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<GoodsData>>(client.PortableGoodsDataList, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_RESETPORTABLEBAG);
            }

            Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion 移动仓库物品管理

        #region AOI(Area Of Interest)九宫格子移动管理

        /// <summary>
        /// 九宫格中缓存的格子X大小
        /// </summary>
        public static int MaxCache9XGridNum = 10; //12 => 10 节省184 个格子的运算 => 8 进一步节省152个格子运算 =>10，8太小了。

        /// <summary>
        /// 九宫格中缓存的格子Y大小
        /// </summary>
        public static int MaxCache9YGridNum = 7; //12 => 10 节省184 个格子的运算 => 8 进一步节省152个格子运算 =>10，8太小了。


        public static readonly short[] ClientViewGridArray = {
                    0,0,1,0,0,1,0,-1,-1,0,-1,1,1,-1,0,-2,2,0,0,2,-1,-1,1,1,-2,0,-2,1,-2,-1,-1,-2,0,-3,-3,0,1,-2,1,2,
                    -1,2,0,3,2,-1,3,0,2,1,-4,0,0,-4,-3,-1,-1,-3,-1,3,-2,-2,3,-1,1,3,2,2,0,4,-3,1,1,-3,-2,2,2,-2,4,0,
                    3,1,-5,0,2,-3,3,2,-3,-2,4,1,-4,-1,-4,1,1,-4,5,0,2,3,1,4,-3,2,-1,4,3,-2,-1,-4,-2,-3,-2,3,0,5,4,-1,
                    0,-5,-6,0,4,-2,2,-4,-4,2,-3,-3,-4,-2,-2,-4,5,-1,-3,3,-1,5,3,-3,-5,-1,-2,4,0,6,1,5,-5,1,4,2,5,1,6,0,
                    1,-5,-1,-5,0,-6,2,4,3,3,-7,0,-6,1,-2,-5,0,7,-1,-6,4,-3,-5,-2,-3,4,-6,-1,-2,5,1,-6,5,-2,-4,-3,-3,-4,2,-5,
                    7,0,4,3,3,4,-5,2,2,5,1,6,-1,6,6,1,0,-7,6,-1,3,-4,-4,3,5,2,2,6,7,1,-1,7,5,-3,0,8,-2,6,7,-1,
                    8,0,3,5,4,4,6,-2,6,2,1,7,5,3,2,-6,-1,-7,1,-7,-6,2,-4,4,4,-4,-5,3,3,-5,0,-8,-4,-4,-2,-6,-3,-5,-7,-1,
                    -7,1,-5,-3,-6,-2,-3,5,-8,0,-1,8,8,-1,3,-6,2,7,6,-3,1,8,7,-2,-6,3,5,4,-7,2,6,3,5,-4,2,-7,3,6,-4,5,
                    7,2,4,-5,-5,4,-3,6,8,1,0,9,0,-9,-2,7,4,5,9,0,1,-8,9,-1,-1,9,0,10,1,-9,-4,6,6,-4,2,-8,9,1,8,2,
                    -5,5,4,-6,4,6,-6,4,-3,7,7,3,7,-3,5,5,6,4,-2,8,8,-2,10,0,5,-5,3,-7,3,7,2,8,1,9,0,11,11,0,1,10,
                    6,5,5,6,4,7,8,3,9,2,2,9,10,-1,5,-6,9,-2,-4,7,3,-8,-2,9,8,-3,-3,8,4,-7,1,-10,6,-5,-1,10,2,-9,7,-4,
                    -5,6,12,0,7,-5,8,-4,-4,8,6,-6,5,-7,0,12,4,-8,-2,10,3,-9,11,-1,-1,11,2,-10,9,-3,-3,9,10,-2,-2,11,8,-5,11,-2,
                    2,-11,7,-6,12,-1,4,-9,5,-8,-1,12,9,-4,6,-7,-3,10,3,-10,10,-3,-2,12,-11,2,-11,3,-10,3,-9,4,-9,5,-8,5,-8,6,-7,6,
                    -7,7,-6,7,-5,8,-5,9,-4,9,-4,10,-10,2,-9,3,-8,4,-7,5,-6,6,-5,7,-9,2,-8,3,-7,4,-6,5,-10,1,-9,1,-8,2,-7,3,
                    -8,1,-9,0,};

        public const int GoodsPackFallGridArray_Length = 100 * 2; //半径为7(100),半径为8(145)

        /// <summary>
        /// 282为最小的可见矩形区域,310为上下扩展1格,349为上下左右各扩展1格
        /// </summary>
        public const int ClientViewGridArray_Length = 282 * 2;

        public const int ClientViewOtherObject9GridNum = 252 * 2; //角色可以看到其他对象的格子范围索引数

        public const int ClientViewRole9GridMinNum = 150 * 2; //可看到其他角色的最小的可见范围(此值以后再调)
        public const int ClientViewRole9GridNormalNum = 282 * 2; //可看到其他角色的正常的可见范围参考值,矩形范围
        public const int ClientViewRole9GridCurrentNum = 282 * 2; //可看到其他角色的正常的可见范围参考值,矩形范围
        public static int ClientViewRoleGridlimitRoleNum = 1000; //单个格子显示的人数限制,当前无限制

        public const int ClientViewFakeRole9GridMinNum = 100 * 2;//可看到假人的最小的可见范围(此值以后再调)
        public const int ClientViewFakeRole9GridLimitNum = 252 * 2; //可看到其他角色的正常的可见范围参考值,16:9全屏幕可见
        public const int ClientViewFakeRole9GridCurrentNum = 252 * 2; //可看到其他角色的正常的可见范围参考值,16:9全屏幕可见
        public static int ClientViewFakeRoleGridlimitRoleNum = 10; //当前显示的假人限制每格子10个

        /// <summary>
        /// Trả ra danh sách đối tượng cần được thêm hoặc xóa xung quanh người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="toRemoveObjsList"></param>
        /// <param name="toAddObjsList"></param>
        public static void GetObjectsAroundToAddOrRemove(KPlayer client, out List<Object> toRemoveObjsList, out List<Object> toAddObjsList)
        {
            toRemoveObjsList = null;
            toAddObjsList = null;

            try
            {
                MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[client.MapCode];
                int centerCridX = client.PosX / mapGrid.MapGridWidth;
                int centerCridY = client.PosY / mapGrid.MapGridHeight;

                /// Danh sách đối tượng xung quanh
                List<Object> keysList = client.VisibleGrid9Objects.Keys.ToList();
                int keysListCount = keysList.Count;
                for (int i = 0; i < keysListCount; i++)
                {
                    Object key = keysList[i];
                    if (!client.VisibleGrid9Objects.ContainsKey(key))
					{
                        client.VisibleGrid9Objects[key] = (byte) 0;
                    }
					else
					{
                        client.VisibleGrid9Objects[key] = client.VisibleGrid9Objects[key] == 3 ? (byte) 3 : (byte) 0;
                    }
                }

                /// Danh sách cần thêm vào xóa nhưng không xóa khỏi danh sách hiển thị của đối tượng
                List<Object> toRemoveAdditionObjsList = new List<Object>();

                /// Danh sách người chơi xung quanh
                ConcurrentDictionary<int, KPlayer> players = new ConcurrentDictionary<int, KPlayer>();

                for (int l = -KTGlobal.RadarHalfWidth; l < KTGlobal.RadarHalfWidth; l++)
                {
                    for (int j = -KTGlobal.RadarHalfHeight; j < KTGlobal.RadarHalfHeight; j++)
                    {
                        int x = l + centerCridX;
                        int y = j + centerCridY;
                        if (x >= 0 && y >= 0 && x < mapGrid.MapGridXNum && y < mapGrid.MapGridYNum)
                        {
                            /// Danh sách các đối tượng tại vị trí tương ứng
                            List<Object> objsList = mapGrid.FindObjects(x, y);
                            if (null == objsList || objsList.Count <= 0)
                            {
                                continue;
                            }

                            /// Duyệt toàn bộ danh sách các đối tượng
                            for (int i = 0; i < objsList.Count; i++)
                            {
                                IObject iObj = (objsList[i]) as IObject;

                                if (iObj is KDynamicArea)
                                {
                                    KDynamicArea dynArea = iObj as KDynamicArea;
                                }

                                /// Nếu không cùng phụ bản
                                if (client.CurrentCopyMapID != iObj.CurrentCopyMapID)
                                {
                                    continue;
                                }

                                /// Nếu là người chơi thì xử lý riêng
                                if (iObj is KPlayer _player)
                                {
                                    players[_player.RoleID] = _player;
                                    continue;
                                }

                                /// Nếu đối tượng có trong danh sách xung quanh đã lưu lại
                                if (client.VisibleGrid9Objects.ContainsKey(objsList[i]))
                                {
                                    /// Nếu là đối tượng có thể thực hiện Logic
                                    if (iObj is GameObject)
                                    {
                                        GameObject go = iObj as GameObject;
                                        /// Nếu đối tượng đang trong trạng thái tàng hình, và không hiện thân với người chơi
                                        if (go.IsInvisible() && !go.VisibleTo(client))
                                        {
                                            /// Nếu đối tượng chưa tồn tại trong danh sách tạm giữ nhưng chưa làm gì
                                            if (client.VisibleGrid9Objects[objsList[i]] != 3)
                                            {
                                                /// Xóa đối tượng khỏi danh tạm giữ nhưng chưa làm gì
                                                client.VisibleGrid9Objects[objsList[i]] = 3;
                                                /// Thêm vào danh sách thêm cần xóa
                                                toRemoveAdditionObjsList.Add(objsList[i]);
                                            }
                                            continue;
                                        }
                                    }

                                    /// Nếu đối tượng có trong danh sách tạm giữ nhưng chưa làm gì
                                    if (client.VisibleGrid9Objects[objsList[i]] == 3)
                                    {
                                        /// Thêm vào danh sách cần thêm
                                        client.VisibleGrid9Objects[objsList[i]] = 2;
                                    }
                                    else
                                    {
                                        /// Giữ đối tượng
                                        client.VisibleGrid9Objects[objsList[i]] = 1;
                                    }
                                }
                                /// Nếu đối tượng không có trong danh sách xung quanh đã lưu lại
                                else
                                {
                                    /// Nếu là đối tượng có thể thực hiện Logic
                                    if (iObj is GameObject)
                                    {
                                        GameObject go = iObj as GameObject;
                                        /// Nếu đối tượng đang trong trạng thái tàng hình, và không hiện thân với người chơi
                                        if (go.IsInvisible() && !go.VisibleTo(client))
                                        {
                                            /// Xóa đối tượng khỏi danh sách tạm giữ nhưng chưa làm gì
                                            client.VisibleGrid9Objects[objsList[i]] = 3;
                                            /// Thêm vào danh sách thêm cần xóa
                                            toRemoveAdditionObjsList.Add(objsList[i]);
                                            continue;
                                        }
                                    }

                                    /// Thêm vào danh sách cần thêm
                                    client.VisibleGrid9Objects[objsList[i]] = 2;
                                }
                            }
                        }
                    }
                }

                /// Thiết lập danh sách người chơi xung quanh
                client.NearbyPlayers = players;

                /// Danh sách cần xóa
                toRemoveObjsList = new List<Object>();
                /// Danh sách cần thêm
                toAddObjsList = new List<Object>();
                List<Object> toRemoveObjsList2 = new List<Object>();

                /// Duyệt toàn bộ các đối tượng xung quanh bản thân
                List<object> keys = client.VisibleGrid9Objects.Keys.ToList();
                foreach (var key in keys)
                {
                    /// Toác
                    if (!client.VisibleGrid9Objects.TryGetValue(key, out byte value))
                    {
                        continue;
                    }

                    /// Nếu cần xóa
                    if (0 == value)
                    {
                        if (key is Monster)
                        {
                            Monster monster = key as Monster;
                            monster.VisibleClientsNum--;
                        }
                        else if (key is KDynamicArea)
                        {
                            KDynamicArea dynArea = key as KDynamicArea;
                            dynArea.VisibleClientsNum--;
                        }
                        else if (key is KTBot)
                        {
                            KTBot bot = key as KTBot;
                            bot.VisibleClientsNum--;
                        }

                        toRemoveObjsList.Add(key);
                    }
                    /// Nếu cần thêm
                    else if (2 == value)
                    {
                        if (key is Monster)
                        {
                            Monster monster = key as Monster;
                            if (monster.CurrentCopyMapID == client.CopyMapID)
                            {
                                monster.VisibleClientsNum++;
                                /// Nếu không phải loại quái đặc biệt hoặc NPC di động
                                if (monster.MonsterType != MonsterAIType.DynamicNPC && monster.MonsterType != MonsterAIType.Special_Boss && monster.MonsterType != MonsterAIType.Special_Normal)
                                {
                                    /// Thực thi Timer của quái
                                    KTMonsterTimerManager.Instance.Add(monster);
                                }
                            }
                        }
                        else if (key is KDynamicArea)
                        {
                            KDynamicArea dynArea = key as KDynamicArea;
                            if (dynArea.CurrentCopyMapID == client.CopyMapID)
                            {
                                dynArea.VisibleClientsNum++;
                                /// Thêm khu vực động vào Timer quản lý
                                KTDynamicAreaTimerManager.Instance.Add(dynArea);
                            }
                        }
                        else if (key is KTBot)
                        {
                            KTBot bot = key as KTBot;
                            if (bot.CurrentCopyMapID == client.CopyMapID)
                            {
                                bot.VisibleClientsNum++;
                                /// Thêm BOT vào Timer quản lý
                                KTBotTimerManager.Instance.Add(bot);
                            }
                        }

                        toAddObjsList.Add(key);
                    }
                }

                for (int i = 0; i < toRemoveObjsList.Count; i++)
                {
                    client.VisibleGrid9Objects.TryRemove(toRemoveObjsList[i], out _);
                }

                /// Thêm danh sách cần xóa vào
                if (toRemoveObjsList == null && toRemoveAdditionObjsList.Count > 0)
                {
                    toRemoveObjsList = toRemoveAdditionObjsList;
                }
                else if (toRemoveObjsList != null && toRemoveAdditionObjsList.Count > 0)
                {
                    toRemoveObjsList.AddRange(toRemoveAdditionObjsList);
                }

                toAddObjsList = ConvertObjsList(client.MapCode, client.CopyMapID, toAddObjsList);
            }
            catch (Exception exx)
            {
                LogManager.WriteLog(LogTypes.RolePosition, "BUG UPDATE OBJECT :" + exx.ToString());
            }
        }

        /// <summary>
        /// Trả về các đối tượng xung quanh
        /// </summary>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static List<object> GetAll9GridObjects(IObject obj)
        {
            /// Nếu không có đối tượng
            if (null == obj)
            {
                return null;
            }
            /// Nếu là người chơi
            if (obj is KPlayer player)
            {
                List<object> objs = null;
                /// Danh sách người chơi xung quanh đã được cập nhật
                List<KPlayer> playersAround = player.NearbyPlayers.Values.ToList();
                /// Duyệt danh sách người chơi xung quanh
                foreach (KPlayer _player in playersAround)
                {
                    if (objs == null)
                    {
                        objs = new List<object>();
                    }
                    objs.Add(_player);
                }
                return objs;
            }

            GameMap gameMap = GameManager.MapMgr.DictMaps[obj.CurrentMapCode];
            Point grid = obj.CurrentGrid;
            int posX = (int)(grid.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2);
            int posY = (int)(grid.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            List<object> list = GetAll9GridObjects2(obj.CurrentMapCode, posX, posY, obj.CurrentCopyMapID);

            return list;
        }

        /// <summary>
        /// Trả ra tất cả đối tượng xung quanh vị trí chỉ định
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="copyMapID"></param>
        public static List<Object> GetAll9GridObjects2(int mapCode, int toX, int toY, int copyMapID)
        {
            List<Object> objsList = new List<Object>();
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            int centerCridX = toX / mapGrid.MapGridWidth;
            int centerCridY = toY / mapGrid.MapGridHeight;

            int roleGrid9Num = 0;
            int fakeRoleGrid9Num = 0;
            for (int l = -KTGlobal.RadarHalfWidth; l < KTGlobal.RadarHalfWidth; l++)
            {
                for (int j = -KTGlobal.RadarHalfHeight; j < KTGlobal.RadarHalfHeight; j++)
                {
                    int x = l + centerCridX;
                    int y = j + centerCridY;
                    if (x >= 0 && y >= 0 && x < mapGrid.MapGridXNum && y < mapGrid.MapGridYNum)
                    {
                        List<Object> tempObjsList = mapGrid.FindObjects((int)x, (int)y);
                        if (null == tempObjsList)
                        {
                            continue;
                        }

                        for (int i = 0; i < tempObjsList.Count; i++)
                        {
                            /// Nếu không nằm trong cùng phụ bản
                            if ((tempObjsList[i] as IObject).CurrentCopyMapID != copyMapID)
                            {
                                continue;
                            }

                            objsList.Add(tempObjsList[i]);
                        }
                    }
                }
            }

            if (objsList.Count <= 0)
            {
                objsList = null;
            }

            /// 转换对象列表
            objsList = ConvertObjsList(mapCode, copyMapID, objsList);

            return objsList;
        }

        /// <summary>
        /// Trả về danh sách người chơi xung quanh
        /// </summary>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static List<object> GetAll9Clients(IObject obj)
        {
            return Global.GetAll9GridObjects(obj);
        }

        /// <summary>
        /// 获取9宫格中的所有角色对象
        /// 查找梯形范围的对象,能看到自己的所有人
        /// </summary>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static List<Object> GetAll9Clients2(int mapCode, int toX, int toY, int copyMapID)
        {
            return Global.GetAll9GridObjects2(mapCode, toX, toY, copyMapID);
        }

        /// <summary>
        /// Chuyển danh sách đối tượng nằm cùng trong phụ bản
        /// </summary>
        /// <param name="objsList"></param>
        /// <returns></returns>
        public static List<Object> ConvertObjsList(int mapCode, int copyMapID, List<Object> objsList, bool onlyGameClient = false)
        {
            /// Nếu không có phụ bản
            if (copyMapID <= 0)
            {
                return objsList;
            }

            MapTypes mapType = GetMapType(mapCode);
            if (MapTypes.Normal == mapType)
            {
                return objsList;
            }

            if (null == objsList) return null;

            List<Object> newObjsList = new List<Object>();
            for (int i = 0; i < objsList.Count; i++)
            {
                if (onlyGameClient && !(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (!(objsList[i] is IObject))
                {
                    continue;
                }

                newObjsList.Add(objsList[i]);
            }

            if (newObjsList.Count <= 0)
            {
                newObjsList = null;
            }

            return newObjsList;
        }

        /// <summary>
        /// Hàm này cập nhật các đối tượng mới xung quanh hoặc xóa các đối tượng cũ trước đó nếu ngoài phạm vi
        /// </summary>
        public static void GameClientMoveGrid(KPlayer client)
        {
            /// Nếu đang đợi chuyển Map thì thôi
            if (client.WaitingForChangeMap)
            {
                return;
            }

            Global.GetObjectsAroundToAddOrRemove(client, out List<object> toRemoveObjsList, out List<object> toAddObjsList);

            if (null != toRemoveObjsList && toRemoveObjsList.Count > 0)
            {
                Global.GameClientHandleOldObjs(client, toRemoveObjsList);
            }

            if (null != toAddObjsList && toAddObjsList.Count > 0)
            {
                Global.GameClientHandleNewObjs(client, toAddObjsList);
            }
        }

        /// <summary>
        /// Thông báo đối tượng mới xung quanh người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public static void GameClientHandleNewObjs(KPlayer client, List<Object> objsList)
        {
            //GameManager.ClientMgr.NotifySelfOnlineOthers(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList, (int)TCPGameServerCmds.CMD_OTHER_ROLE);

            GameManager.MonsterMgr.SendMonstersToClient(Global._TCPManager.MySocketListener, client, Global._TCPManager.TcpOutPacketPool, objsList, (int)TCPGameServerCmds.CMD_SYSTEM_MONSTER);

            // NOTIFY CHO NGƯỜI CHƠI KHI DI CHUYỂN TỚI NẾU THẤY VẬT PHẨM RƠI XUỐNG ĐẤT
            GameManager.GoodsPackMgr.SendMySelfGoodsPackItems(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            NPCGeneralManager.SendMySelfNPCs(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Hiển thị bẫy
            KTTrapManager.SendMySelfTraps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Hiển thị điểm thu thập
            KTGrowPointManager.SendMySelfGrowPoints(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Hiển thị khu vực động
            KTDynamicAreaManager.SendMySelfDynamicAreas(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Hiển thị BOT
            KTBotManager.SendMySelfBots(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);
        }

        /// <summary>
        /// Thông báo để xóa người chơi khỏi danh sách xung quanh của người khác
        /// </summary>
        public static void GameClientHandleOldObjs(KPlayer client, List<Object> objsList)
        {
            ///// Thông báo bản thân rời khỏi phạm vi tới người chơi khác
            //GameManager.ClientMgr.NotifyMyselfLeaveOthers(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Thông báo quái rời khỏi phạm vi bản thân
            GameManager.ClientMgr.NotifyMyselfLeaveMonsters(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa vật phẩm rơi ở MAP
            GameManager.GoodsPackMgr.DelMySelfGoodsPackItems(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa NPC
            NPCGeneralManager.DelMySelfNpcs(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa bẫy
            KTTrapManager.DelMySelfTraps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa điểm thu thập
            KTGrowPointManager.DelMySelfGrowPoints(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa khu vực động
            KTDynamicAreaManager.DelMySelfDynamicAreas(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);

            /// Xóa BOT
            KTBotManager.DelMySelfBots(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, objsList);
        }

        /// <summary>
        /// Xử lý khi đối tượng người chơi được tải xuống thành công
        /// </summary>
        /// <param name="client"></param>
        /// <param name="otherClient"></param>
        public static void HandleGameClientLoaded(KPlayer client, KPlayer otherClient)
        {
            string pathString = KTPlayerStoryBoardEx.Instance.GetCurrentPathString(otherClient);
            GameManager.ClientMgr.NotifyMyselfOtherLoadAlready(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                otherClient.RoleID,
                otherClient.PosX, otherClient.PosY,
                (int)otherClient.ToPos.X, (int)otherClient.ToPos.Y,
                (int)otherClient.CurrentDir,
                (int)otherClient.m_eDoing,
                pathString,
                otherClient.Camp);
        }

        /// <summary>
        /// Xử lý khi đối tượng quái được tải xuống thành công
        /// </summary>
        /// <param name="client"></param>
        /// <param name="otherClient"></param>
        public static void HandleMonsterLoaded(KPlayer client, Monster monster)
        {
            string pathString = KTMonsterStoryBoardEx.Instance.GetCurrentPathString(monster);
            GameManager.ClientMgr.NotifyMyselfMonsterLoadAlready(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                monster.RoleID,
                (int)monster.CurrentPos.X, (int)monster.CurrentPos.Y,
                (int)monster.ToPos.X, (int)monster.ToPos.Y,
                (int)monster.CurrentDir,
                (int)monster.m_eDoing,
                pathString,
                monster.Camp);
        }

        /// <summary>
        /// Xử lý khi đối tượng quái được tải xuống thành công
        /// </summary>
        /// <param name="client"></param>
        /// <param name="otherClient"></param>
        public static void HandleBotLoaded(KPlayer client, KTBot bot)
        {
            string pathString = KTOtherObjectStoryBoard.Instance.GetCurrentPathString(bot);
            GameManager.ClientMgr.NotifyMyselfBotLoadAlready(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                bot.RoleID,
                (int)bot.CurrentPos.X, (int)bot.CurrentPos.Y,
                (int)bot.ToPos.X, (int)bot.ToPos.Y,
                (int)bot.CurrentDir,
                (int)bot.m_eDoing,
                pathString,
                bot.Camp);
        }

        #endregion AOI(Area Of Interest)九宫格子移动管理

        #region Quản lý bản đồ

        /// <summary>
        /// Kiểm tra bản đồ có thể lưu thông tin vị trí hiện tại không
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool CanRecordPos(KPlayer client)
        {
            return true;
        }

        /// <summary>
        /// Kiểm tra bản đồ có tòn tại không
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static bool MapExists(int mapCode)
        {
            if (mapCode < 0) return false;
            return GameManager.MapMgr.DictMaps.ContainsKey(mapCode);
        }

        /// <summary>
        /// Kiểm tra có thể chuyển bản đồ không
        /// </summary>
        /// <param name="client"></param>
        /// <param name="mapCode"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public static bool CanChangeMap(KPlayer client, int mapCode, int posX, int posY, bool normalMapOlny = true)
        {
            if (normalMapOlny && MapTypes.Normal != Global.GetMapType(mapCode) || SceneUIClasses.Normal != Global.GetMapSceneType(mapCode))
            {
                return false;
            }
            if (!Global.CanChangeMapCode(client, mapCode))
            {
                return false;
            }

            GameMap gameMap = GameManager.MapMgr.GetGameMap(mapCode);
            if (null == gameMap)
            {
                return false;
            }

            if (posX < 0 || posX >= gameMap.MapWidth || posY < 0 || posY >= gameMap.MapHeight)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Trả về loại bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static MapTypes GetMapType(int mapCode)
        {
            return MapTypes.Normal;
        }

        /// <summary>
        /// Có thể chuyển bản đồ không
        /// </summary>
        /// <param name="client"></param>
        /// <param name="toMapCode"></param>
        public static bool CanChangeMapCode(KPlayer client, int toMapCode)
        {
            ///// Bản đồ tương ứng
            //GameMap map = GameManager.MapMgr.GetGameMap(toMapCode);
            ///// Nếu là bản đồ phụ bản thì toác
            //return !map.IsCopyScene;
            return true;
        }

        /// <summary>
        /// Trả về tên bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static string GetMapName(int mapCode)
        {
            string mapName = "";

            return mapName;
        }

        #endregion Quản lý bản đồ



        #region 与DBServer通讯
        public static byte[] SendAndRecvData(TCPClient tcpClient, TCPOutPacket tcpOutPacket)
        {
            byte[] bytesData = null;

            //查询
            try
            {
                //获取
                if (null != tcpClient)
                {
                    bytesData = tcpClient.SendData(tcpOutPacket);
                }

                if (null != bytesData && bytesData.Length >= 6)
                {
                    UInt16 returnCmdID = BitConverter.ToUInt16(bytesData, 4);
                    if ((UInt16)TCPGameServerCmds.CMD_DB_ERR_RETURN == returnCmdID) //返回失败的错误信息
                    {
                        //告诉外边失败
                        bytesData = null;
                        LogManager.WriteLog(LogTypes.Error, "Return from NameServer => CMD_DB_ERR_RETURN");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, "Return from NameServer => " + ex);
            }

            return bytesData;
        }

        public static byte[] SendAndRecvData(TCPOutPacket tcpOutPacket, int serverId, int PoolId)
        {
            TCPClient tcpClient = null;
            byte[] bytesData = null;

            //查询
            try
            {
                //获取
                tcpClient = GlobalNew.PopGameDbClient(serverId, PoolId);
                if (null != tcpClient)
                {
                    bytesData = tcpClient.SendData(tcpOutPacket);
                }

                if (null != bytesData && bytesData.Length >= 6)
                {
                    UInt16 returnCmdID = BitConverter.ToUInt16(bytesData, 4);
                    if ((UInt16)TCPGameServerCmds.CMD_DB_ERR_RETURN == returnCmdID) //返回失败的错误信息
                    {
                        //告诉外边失败
                        bytesData = null;
                        LogManager.WriteLog(LogTypes.Error, "Return from DBServer => CMD_DB_ERR_RETURN");
                    }
                }
            }
            finally
            {
                //还回
                if (null != tcpClient)
                {
                    GlobalNew.PushGameDbClient(serverId, tcpClient, PoolId);
                }
            }

            return bytesData;
        }

        /// <summary>
        /// 请求DBServer进行处理
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static TCPProcessCmdResults RequestToDBServer(TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, string strcmd, out string[] fields, int serverId)
        {
            fields = null;

            try
            {
                byte[] bytesData = Global.SendAndRecvData(nID, strcmd, serverId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);
                string strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

                //解析客户端的指令
                fields = strData.Split(':');
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteExceptionLogEx(ex, "RequestToDBServer");
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gửi và nhận gói tin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdId"></param>
        /// <param name="data"></param>
        /// <param name="serverId"></param>
        /// <param name="PoolId">0 GameDbServer,1 LogDbServer</param>
        /// <returns></returns>
        public static byte[] SendAndRecvData<T>(int cmdId, T data, int serverId, int PoolId = 0)
        {
            byte[] bytesData = null;
            TCPOutPacket tcpOutPacket = null;
            TCPOutPacketPool pool = TCPOutPacketPool.getInstance();

            if (null != pool)
            {
                //查询
                try
                {
                    //获取
                    if (data is string)
                    {
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, data as string, cmdId);
                    }
                    else if (data is byte[])
                    {
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, data as byte[], cmdId);
                    }
                    else
                    {
                        byte[] cmdData = DataHelper.ObjectToBytes<T>(data);
                        if (null != cmdData)
                        {
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, cmdId);
                        }
                    }

                    if (null != tcpOutPacket)
                    {
                        bytesData = Global.SendAndRecvData(tcpOutPacket, serverId, PoolId);
                    }
                }
                finally
                {
                    //还回
                    if (null != tcpOutPacket)
                    {
                        pool.Push(tcpOutPacket);
                    }
                }
            }

            return bytesData;
        }

        /// <summary>
        /// Chuyển yêu cầu sang GameDB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdId"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static T1 SendToDB<T1, T2>(int cmdId, T2 cmd, int serverId)
        {
            try
            {
                byte[] bytesData = Global.SendAndRecvData(cmdId, cmd, serverId);

                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)cmdId));
                    return default(T1);
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);

                T1 obj = DataHelper.BytesToObject<T1>(bytesData, 6, length - 2);

                return obj;
            }
            catch (Exception ex)
            {
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "SendToDB", false);
            }

            return default(T1);
        }

        /// <summary>
        /// Gửi yêu cầu sang GameDB
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="cmdData"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static string[] SendToDB(int cmdID, string cmdData, int serverId)
        {
            try
            {
                byte[] bytesData = Global.SendAndRecvData(cmdID, cmdData, serverId);

                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)cmdID));
                    return null;
                }

                int length = BitConverter.ToInt32(bytesData, 0);
                string strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

                string[] fields = strData.Split(':');
                return fields;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "SendToDB", false);
            }

            return null;
        }

        /// <summary>
        /// Chuyển yêu cầu sang GameDB
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="strcmd"></param>
        /// <returns></returns>
        public static string[] SendToDB<T>(int nCmdID, T CmdInfo, int serverId)
        {
            byte[] bytesCmd = DataHelper.ObjectToBytes<T>(CmdInfo);
            byte[] bytesData = Global.SendAndRecvData(nCmdID, bytesCmd, serverId);

            if (null == bytesData)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nCmdID));
                return null;
            }

            string[] fieldsData = null;
            Int32 length = BitConverter.ToInt32(bytesData, 0);
            string strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

            //解析客户端的指令
            fieldsData = strData.Split(':');
            if (null == fieldsData || fieldsData.Length <= 0)
            {
                return null;
            }

            return fieldsData;
        }

        /// <summary>
        /// 请求DBServer进行处理
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static TCPProcessCmdResults RequestToDBServer2(TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, string strcmd, out TCPOutPacket tcpOutPacket, int serverId)
        {
            tcpOutPacket = null;

            try
            {
                byte[] bytesData = Global.SendAndRecvData(nID, strcmd, serverId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);
                UInt16 cmd = BitConverter.ToUInt16(bytesData, 4);

                tcpOutPacket = pool.Pop();
                tcpOutPacket.PacketCmdID = (UInt16)cmd;
                tcpOutPacket.FinalWriteData(bytesData, 6, length - 2);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "RequestToDBServer2", false);
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// 请求DBServer进行处理
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static TCPProcessCmdResults RequestToDBServer3(TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, string strcmd, out byte[] bytesData, int serverId)
        {
            bytesData = null;

            try
            {
                bytesData = Global.SendAndRecvData(nID, strcmd, serverId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "RequestToDBServer3", false);
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// 请求DBServer进行处理
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static TCPProcessCmdResults RequestToDBServer4(TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, string strcmd, out byte[] bytesData, out int dataStartPos, out int dataLen, int serverId)
        {
            bytesData = null;
            dataStartPos = 0;
            dataLen = 0;

            try
            {
                bytesData = Global.SendAndRecvData(nID, strcmd, serverId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);

                dataStartPos = 6;
                dataLen = length - 2;

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "RequestToDBServer4", false);
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults TransferRequestToDBServer(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket, int serverId)
        {
            tcpOutPacket = null;

            try
            {
                byte[] bytesData = Global.SendAndRecvData(nID, data, socket.ServerId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);
                UInt16 cmd = BitConverter.ToUInt16(bytesData, 4);

                tcpOutPacket = pool.Pop();
                tcpOutPacket.PacketCmdID = (UInt16)cmd;
                tcpOutPacket.FinalWriteData(bytesData, 6, length - 2);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// 将请求转到DBServer进行处理
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static TCPProcessCmdResults TransferRequestToDBServer2(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out byte[] bytesData, int serverId)
        {
            bytesData = null;

            try
            {
                bytesData = Global.SendAndRecvData(nID, data, socket.ServerId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults ReadDataFromDb(int nID, byte[] data, int count, out byte[] bytesData, int serverId)
        {
            bytesData = null;

            try
            {
                bytesData = Global.SendAndRecvData(nID, data, serverId);
                if (null == bytesData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Send packet to GameDB faild, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //  DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static string[] ExecuteDBCmd(int nID, string strcmd, int serverId)
        {
            string[] fieldsData = null;
            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                nID, strcmd, out fieldsData, serverId))
            {
                return null;
            }

            if (null == fieldsData || fieldsData.Length <= 0)
            {
                return null;
            }

            return fieldsData;
        }

        public static string QueryTokenFromDB(int roleID, string otherRoleName, int serverId = GameManager.LocalServerId)
        {
            int Token = 0;
            int realMoney = 0;
            if (string.IsNullOrEmpty(otherRoleName))
            {
                return null;
            }

            //从DBServer获取角色的所在的线路
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_SPR_QUERYUMBYNAME, string.Format("{0}:{1}", roleID, otherRoleName), serverId);
            if (null == dbFields || dbFields.Length < 4)
            {
                if (null == dbFields)
                {
                    return null;
                }
            }
            else
            {
                //Token = Global.SafeConvertToInt32(dbFields[2]);
                //realMoney = Global.SafeConvertToInt32(dbFields[3]);
            }

            return dbFields[1];
        }

        public static string[] QeuryUserActivityInfo(KPlayer client, string keyStr, int activityType, string tag = "0")
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, keyStr, activityType, tag);
            return Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERY_USERACTIVITYINFO, strcmd, client.ServerId);
        }

        public static string[] UpdateUserActivityInfo(KPlayer client, string keyStr, int activityType, long hasGetTimes, string lastGetTime)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, keyStr, activityType, hasGetTimes, lastGetTime);
            return Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_USERACTIVITYINFO, strcmd, client.ServerId);
        }

        #endregion 与DBServer通讯

        #region 服务器心跳管理

        //2011-05-31 精简指令，减少DBServer端的压力，和CMD_DB_GET_CHATMSGLIST合并
        /// <summary>
        /// 发送的心跳的次数(便于DBServer识别是否是重新上线的服务器)
        /// </summary>
        public static int SendServerHeartCount = 0;

        #endregion 服务器心跳管理

        #region 账户管理

        public static int GetSwitchServerWaitSecs(TMSKSocket socket)
        {
            //只有每天0点前后3分钟才做限制
            TimeSpan timeOfDay = TimeUtil.NowDateTime().TimeOfDay;
            if (timeOfDay.TotalMinutes >= GameManager.ConstCheckServerTimeDiffMinutes && timeOfDay.TotalMinutes < TimeSpan.FromDays(1).TotalMinutes - GameManager.ConstCheckServerTimeDiffMinutes)
            {
                return 0;
            }

            long waitSecs = (socket.session.LastLogoutServerTicks - TimeUtil.NOW()) / 1000;
            if (waitSecs < 0 || waitSecs > 60)
            {
                if (waitSecs > 60 && waitSecs < 60 * 60)
                {
                    //超过60秒误差的，属于系统故障、测试环境或配置错误
                    LogManager.WriteLog(LogTypes.Error, string.Format("Tài khoản đăng lục lúc kiểm trắc, Server thời gian sai sót khả năng vượt qua 60 Giây，Lần này đăng nhập so với lần trước hạ tuyến thời gian sớm {0} Giây", waitSecs));
                }
                waitSecs = 0;
            }

            return (int)waitSecs;
        }

        /// <summary>
        /// 将用户ID注册到DBServer上，如果失败，则拒绝登陆
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>

        #endregion 账户管理

        #region 角色封锁

        /// <summary>
        /// 将角色登陆禁止到DBServer上
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void BanRoleNameToDBServer(string roleName, int banVal)
        {
            //请求数据库操作
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_BANROLENAME,
                string.Format("{0}:{1}", roleName, banVal),
                null, GameManager.LocalServerId);
        }

        #endregion 角色封锁

        #region Cấm Chat

        /// <summary>
        /// Cấm chat nhân vật tương ứng khỏi DB
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static void BanRoleChatToDBServer(string roleName, int banHour)
        {
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_BANROLECHAT,
                string.Format("{0}:{1}", roleName, banHour),
                null, GameManager.LocalServerId);
        }

        #endregion Cấm Chat

        #region 游戏DB配置参数管理

        /// <summary>
        /// 将配置参数更新到DBServer
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="toPlayNum"></param>
        /// <param name="bulletinText"></param>
        public static void UpdateDBGameConfigg(string paramName, string paramValue)
        {
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_GAMECONIFGITEM,
                string.Format("{0}:{1}", paramName, paramValue),
                null, GameManager.LocalServerId);
        }

        /// <summary>
        /// 从DBserver加载配置参数
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LoadDBGameConfigDict()
        {
            Dictionary<string, string> dict = null;

            byte[] bytesData = null;
            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer3(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                (int)TCPGameServerCmds.CMD_DB_GAMECONFIGDICT, string.Format("{0}", GameManager.ServerLineID), out bytesData, GameManager.LocalServerId))
            {
                return dict; //如果查询失败，就当做时不在线了
            }

            if (null == bytesData || bytesData.Length <= 6)
            {
                return dict;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);

            //获取公告消息字典
            dict = DataHelper.BytesToObject<Dictionary<string, string>>(bytesData, 6, length - 2);
            return dict;
        }

        #endregion 游戏DB配置参数管理

        #region 根据时间执行更新数据的DBServer执行

        /// <summary>
        /// 获取指定的命令的上次执行时间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbCmdID"></param>
        /// <returns></returns>

        /// <summary>
        /// 设置指定的命令的上次执行时间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbCmdID"></param>
        public static void SetLastDBCmdTicks(KPlayer client, int dbCmdID, long nowTicks)
        {
            lock (client.LastDBCmdTicksDict)
            {
                client.LastDBCmdTicksDict[dbCmdID] = nowTicks;
            }
        }

        #endregion 根据时间执行更新数据的DBServer执行

        #region 根据时间主动提交角色统计数据

        /// <summary>
        /// Ghi lại nhật ký thời gian đã update thông tin vào DB
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>

        #endregion 根据时间主动提交角色统计数据

        #region 根据时间执行更新技能数据

        /// <summary>
        /// 设置指定的技能命令的上次执行时间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbCmdID"></param>

        /// <summary>
        /// 执行指定的技能数据库命令
        /// </summary>
        /// <param name="client"></param>

        #endregion 根据时间执行更新技能数据

        #region 根据时间执行更新角色参数的信息

        /// <summary>
        /// 获取指定的角色的参数命令的上次执行时间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbCmdID"></param>
        /// <returns></returns>
        private static long GetLastDBRoleParamCmdTicks(KPlayer client, string paramName)
        {
            long lastDbRoleParamCmdTicks = 0;
            lock (client.LastDBRoleParamCmdTicksDict)
            {
                if (client.LastDBRoleParamCmdTicksDict.TryGetValue(paramName, out lastDbRoleParamCmdTicks))
                {
                    return lastDbRoleParamCmdTicks;
                }
            }

            return 0;
        }

        #endregion 根据时间执行更新角色参数的信息

        #region 根据时间执行更新装备耐久度数据

        /// <summary>
        /// 最大执行装备耐久度命令的时间间隔
        /// </summary>

        #endregion 根据时间执行更新装备耐久度数据

        #region 处理心跳超时的角色

        /// <summary>
        /// 关闭TMSKSocket连接
        /// </summary>
        /// <param name="client"></param>
        public static void ForceCloseClient(KPlayer client, string reason = "", bool sync = true)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                /**/
                reason = string.Format("RoleID={0}, RoleName={1}, 强制关闭:{2}", client.RoleID, client.RoleName, reason);
            }

            client.ClosingClientStep = 1;
            Global._TCPManager.MySocketListener.CloseSocket(client.ClientSocket, reason);
        }

        /// <summary>
        /// 关闭角色连接
        /// </summary>
        /// <param name="client"></param>
        public static void ForceCloseSocket(TMSKSocket socket, string reason = "", bool sync = true)
        {
            //Global._TCPManager.MySocketListener.CloseSocket(socket);
            if ("" != reason || string.IsNullOrEmpty(socket.CloseReason))
            {
                socket.CloseReason = reason;
            }

            Global._TCPManager.ExternalClearSocket(socket);
        }

        /// <summary>
        /// Thực thi Tick Client
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessClientHeart(KPlayer client)
        {
            long nowTicks = TimeUtil.NOW();

            if (client.CheckCheatData.ProcessBoosterTicks > 0 && nowTicks - client.CheckCheatData.ProcessBoosterTicks > 90 * TimeUtil.SECOND)
            {
                if (TimeUtil.HasTimeDrift())
                {
                    client.CheckCheatData.ProcessBoosterTicks = 0;
                }
                else
                {
                    client.CheckCheatData.ProcessBooster = true;
                }
            }

            string detail = "";
            if (nowTicks - client.LastClientHeartTicks < (60 * 3 * 1000))
            {
                if (!client.CheckCheatData.MismatchingMapCode)
                {
                    return;
                }
                else
                {
                    /**/
                    detail = "Missing map";
                }
            }
            else
            {
                detail = "Over time";
            }

            if (0 == client.ClosingClientStep)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("RoleID={0}, RoleName={1}, IP={2} no heart beat found, close socket.", client.RoleID, client.RoleName, Global.GetSocketRemoteEndPoint(client.ClientSocket)));
                ForceCloseClient(client, detail);
            }
            else if (1 == client.ClosingClientStep)
            {
                if (nowTicks - client.LastClientHeartTicks >= (60 * 4 * 1000))
                {
                    client.ClosingClientStep = 2;
                    ForceCloseSocket(client.ClientSocket);
                }
            }
        }

        #endregion 处理心跳超时的角色

        #region 系统送礼活动

        /// <summary>
        /// 数据库命令更新活动数据事件
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="horseID"></param>
        /// <param name="props"></param>
        /// <returns></returns>

        /// <summary>
        /// 计算今天是今年的第几周(一周从周日到周六，即周日是第一天)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public static int WeekOfYear(DateTime dt)
        //{
        //    DateTime print = new DateTime(dt.Year, 1, 1);
        //    return (dt.DayOfYear + print.DayOfWeek - dt.DayOfWeek - 8) / 7 + 2;
        //}

        /// <summary>
        /// 更新周连续登录的次数
        /// </summary>
        /// <param name="client"></param>
        public static bool UpdateWeekLoginNum(KPlayer client)
        {
            // 优化和DB的通信 改造下接口 -- 直接修改client.MyHuodongData 在其后进行统一的DB通知 [1/18/2014 LiaoWei]

            //HuodongData huodongData = client.MyHuodongData;    这里注释掉

            int weekID = (int)TimeUtil.NowDateTime().DayOfWeek;
            int todayID = TimeUtil.NowDateTime().DayOfYear;

            bool reset = false;
            if (1 != weekID)
            {
                if (client.MyHuodongData.LastDayID != todayID.ToString()) //判断如果不是同一天
                {
                    if (client.MyHuodongData.LastDayID == TimeUtil.NowDateTime().AddDays(-1).DayOfYear.ToString())
                    {
                        client.MyHuodongData.LastDayID = todayID.ToString();
                        client.MyHuodongData.LoginNum++;

                        //数据库命令更新活动数据事件
                        //Global.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);  这里注释掉
                        return true;
                    }
                    else
                    {
                        ;//中断了不处理了
                        reset = true;
                    }
                }
            }
            else
            {
                reset = true; //周1重新开始计数
            }

            if (reset)
            {
                client.MyHuodongData.LoginGiftState = 0;
                client.MyHuodongData.LastWeekID = weekID.ToString(); //记录下来，但是没用了
                client.MyHuodongData.LastDayID = todayID.ToString();
                client.MyHuodongData.LoginNum = 1;

                //数据库命令更新活动数据事件
                //Global.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client); 这里注释掉
                return true;
            }

            return false;
        }

        /// <summary>
        /// Quà tặng cho người mới
        /// </summary>
        /// <param name="client"></param>
        public static void InitNewStep(KPlayer client)
        {
            if (client.MyHuodongData.NewStep <= 0)
            {
                if (client.MyHuodongData.StepTime <= 0)
                {
                    client.MyHuodongData.StepTime = (TimeUtil.NOW());
                }
            }
        }

        /// <summary>
        /// Cập nhật số lần đăng nhập tích lũy trong thời gian giới hạn
        /// </summary>
        /// <param name="client"></param>
        public static bool UpdateLimitTimeLoginNum(KPlayer client)
        {
            int todayID = TimeUtil.NowDateTime().DayOfYear;
            int currentHuoDongID = 1;

            if (currentHuoDongID > 0) //判断是否在活动期间
            {
                if (currentHuoDongID == client.MyHuodongData.LastLimitTimeHuoDongID)
                {
                    if (todayID != client.MyHuodongData.LastLimitTimeDayID)
                    {
                        client.MyHuodongData.LastLimitTimeDayID = todayID;
                        client.MyHuodongData.LimitTimeLoginNum++;

                        return true;
                    }
                }
                else
                {
                    client.MyHuodongData.LastLimitTimeHuoDongID = currentHuoDongID;
                    client.MyHuodongData.LastLimitTimeDayID = todayID;
                    client.MyHuodongData.LimitTimeLoginNum = 1;
                    client.MyHuodongData.LimitTimeGiftState = 0;

                    return true;
                }
            }

            return false;
        }

        #endregion 系统送礼活动

        #region 线路管理

        /// <summary>
        /// 线路名称
        /// </summary>
        private static string[] LineNames =
        {
            Global.GetLang("零线"),
            Global.GetLang("一线"),
            Global.GetLang("二线"),
            Global.GetLang("三线"),
            Global.GetLang("四线"),
            Global.GetLang("五线"),
            Global.GetLang("六线"),
            Global.GetLang("七线"),
            Global.GetLang("八线"),
            Global.GetLang("九线"),
            Global.GetLang("十线"),
        };

        /// <summary>
        /// 获取本服的线路名称
        /// </summary>
        /// <returns></returns>
        public static string GetServerLineName1()
        {
            return Global.GetLang(Global.LineNames[GameManager.ServerLineID]);
        }

        /// <summary>
        /// 获取本服的线路名称
        /// </summary>
        /// <returns></returns>
        public static string GetServerLineName2()
        {
            //string lineName = string.Format("[{0}]", Global.GetLang(Global.LineNames[GameManager.ServerLineID]));
            string lineName = "";
            return lineName;
        }

        #endregion 线路管理

        #region 解析时间段限制字符串

        /// <summary>
        /// 解析时间段限制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTimeRange[] ParseDateTimeRangeStr(string str)
        {
            if (null == str)
            {
                return null;
            }

            str = str.Trim();
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string[] fields1 = str.Split('|');
            if (null == fields1 || fields1.Length <= 0)
            {
                return null;
            }

            DateTimeRange[] dateTimeRangeArray = new DateTimeRange[fields1.Length];
            for (int i = 0; i < fields1.Length; i++)
            {
                string timeRangeStr = fields1[i].Trim();
                if (string.IsNullOrEmpty(timeRangeStr))
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                string[] fields2 = timeRangeStr.Split('-');
                if (null == fields2 || fields2.Length != 2)
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                string timeFieldStr1 = fields2[0].Trim();
                if (string.IsNullOrEmpty(timeFieldStr1))
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                string[] fields3 = timeFieldStr1.Split(':');
                if (null == fields3 || fields3.Length != 2)
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                string timeFieldStr2 = fields2[1].Trim();
                if (string.IsNullOrEmpty(timeFieldStr2))
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                string[] fields4 = timeFieldStr2.Split(':');
                if (null == fields4 || fields4.Length != 2)
                {
                    dateTimeRangeArray[i] = null;
                    continue;
                }

                dateTimeRangeArray[i] = new DateTimeRange()
                {
                    FromHour = Global.SafeConvertToInt32(fields3[0]),
                    FromMinute = Global.SafeConvertToInt32(fields3[1]),
                    EndHour = Global.SafeConvertToInt32(fields4[0]),
                    EndMinute = Global.SafeConvertToInt32(fields4[1]),
                };
            }

            return dateTimeRangeArray;
        }

        /// <summary>
        /// 判断当前时间是否在事件段内
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeRangeArray"></param>
        /// <returns></returns>
        public static bool JugeDateTimeInTimeRange(DateTime dateTime, DateTimeRange[] dateTimeRangeArray, out int endMinute, bool equalEndTime = true)
        {
            endMinute = 0;

            if (null == dateTimeRangeArray)
            {
                return true;
            }

            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            for (int i = 0; i < dateTimeRangeArray.Length; i++)
            {
                if (null == dateTimeRangeArray[i])
                {
                    continue;
                }

                int time1 = dateTimeRangeArray[i].FromHour * 60 + dateTimeRangeArray[i].FromMinute;
                int time2 = dateTimeRangeArray[i].EndHour * 60 + dateTimeRangeArray[i].EndMinute;
                int time3 = hour * 60 + minute;

                if (!equalEndTime)
                {
                    time2 -= 1;
                }

                //判断是否在时间段内
                if (time3 >= time1 && time3 <= time2)
                {
                    //记录下结束时间
                    endMinute = time2;
                    return true;
                }
            }

            return false;
        }

        #endregion 解析时间段限制字符串

        #region 角色行为日志

        /// <summary>
        /// 是否将 rid和rname共存的 rname 去掉标识
        /// </summary>
        public static bool WithRname = false;

        /// <summary>
        /// 写入角色登录的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleLoginEvent(KPlayer client)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            string ip = Global.GetSocketRemoteEndPoint(client.ClientSocket).Replace(":", ".");

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}" : "{0} {1}	{2}	{4}	{5}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                ip
                );

            GameManager.SystemRoleLoginEvents.AddEvent(eventMsg, EventLevels.Important);

            //string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheEvent_login(-1,
            //    client.ZoneID,
            //    userID,
            //    userName,
            //    client.RoleID,
            //    client.RoleName,
            //    client.m_Level,
            //    ip,
            //    writerTime);
        }

        /// <summary>
        /// 写入角色登出的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleLogoutEvent(KPlayer client)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            string ip = Global.GetSocketRemoteEndPoint(client.ClientSocket).Replace(":", ".");

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5} {6}" : "{0}	{1}	{2}	{4}	{5} {6}";

            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                ip,
                (int)(client.CheckCheatData.MaxClientSpeed * 100)
                );

            GameManager.SystemRoleLogoutEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色完成任务的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleTaskEvent(KPlayer client, int completeTaskID)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}" : "{0}	{1}	{2}	{4}	{5}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                completeTaskID
                );

            GameManager.SystemRoleTaskEvents.AddEvent(eventMsg, EventLevels.Important);

            //string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheEvent_task(-1,
            //    client.ZoneID,
            //    userID,
            //    userName,
            //    client.RoleID,
            //    client.RoleName,
            //    client.m_Level,
            //    completeTaskID,
            //    writerTime);
        }

        /// <summary>
        /// 写入角色死亡的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleDeathEvent(KPlayer client, string deadMsg)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                client.PosX,
                client.PosY,
                deadMsg
                );

            GameManager.SystemRoleDeathEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色银两增加/减少日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBoundTokenEvent2(string userID, int roleID, string roleName, int addBoundToken)
        {
            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}" : "{0}	{1}	{2}	{4}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                roleID,
                roleName,
                addBoundToken
                );

            GameManager.SystemRoleBoundTokenEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色金币增加/减少日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBoundMoneyEvent(KPlayer client, int oldBoundMoney)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            string ip = Global.GetSocketRemoteEndPoint(client.ClientSocket).Replace(":", ".");

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}" : "{0}	{1}	{2}	{4}	{5}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                oldBoundMoney,
                client.BoundMoney
                );

            GameManager.SystemRoleBoundMoneyEvents.AddEvent(eventMsg, EventLevels.Important);

            //string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheEvent_BoundToken(-1,
            //    client.ZoneID,
            //    userID,
            //    userName,
            //    client.RoleID,
            //    client.RoleName,
            //    oldBoundToken,
            //    client.BoundToken,
            //    writerTime);
        }

        /// <summary>
        /// 写入角色仓库绑定金币日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleStoreMoneyEvent(KPlayer client, long oldBoundToken)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            string ip = Global.GetSocketRemoteEndPoint(client.ClientSocket).Replace(":", ".");

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                oldBoundToken,
                0
                );

            GameManager.SystemRoleStoreMoneyEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色金币购买的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBuyWithGlodEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                client.BoundMoney
                );

            GameManager.SystemRoleBuyWithBoundMoneyEvents.AddEvent(eventMsg, EventLevels.Important);

            //更新db金币购买记录
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDBoundMoneyBUYITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, goodsID, goodsNum, totalPrice, client.BoundMoney),
                null, client.ServerId);

            //string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheBoundTokenbuy(-1,
            //    client.ZoneID,
            //    userID,
            //    userName,
            //    client.RoleID,
            //    client.RoleName,
            //    goodsID,
            //    goodsNum,
            //    totalPrice,
            //    client.BoundToken,
            //    writerTime,
            //    Global.GetGoodsNameByID(goodsID));

            /*string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheYinpiaobuy(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                goodsID,
                goodsNum,
                totalPrice,
                client.BoundMoney,
                writerTime,
                Global.GetGoodsNameByID(goodsID));*/
        }

        /// <summary>
        /// 写入角色银票购买的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBuyWithYinPiaoEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice, int yinPiaoGoodsID)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            int leftYinPiaoNum = Global.GetTotalGoodsCountByID(client, yinPiaoGoodsID);
            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                leftYinPiaoNum
                );

            GameManager.SystemRoleBuyWithYinPiaoEvents.AddEvent(eventMsg, EventLevels.Important);

            //更新上线状态
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDYINPIAOBUYITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, goodsID, goodsNum, totalPrice, leftYinPiaoNum),
                null, client.ServerId);

            /*string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheYinpiaobuy(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                goodsID,
                goodsNum,
                totalPrice,
                leftYinPiaoNum,
                writerTime,
                Global.GetGoodsNameByID(goodsID));*/
        }

        /// <summary>
        /// 写入角色天地精元的兑换的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBuyWithTianDiJingYuanEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice, int tianDiJingYuanGoodsID)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            int leftNum = Global.GetTotalGoodsCountByID(client, tianDiJingYuanGoodsID);
            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                leftNum
                );

            GameManager.SystemRoleBuyWithTianDiJingYuanEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// Viết nhập nhân vật Nguyên bảo mua hành vi nhật ký
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleBuyWithYuanBaoEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token
                );

            GameManager.SystemRoleBuyWithYuanBaoEvents.AddEvent(eventMsg, EventLevels.Important);

            //更新上线状态
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDMALLBUYITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, goodsID, goodsNum, totalPrice, client.Token),
                null, client.ServerId);

            /*string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheMallbuy(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token,
                writerTime,
                Global.GetGoodsNameByID(goodsID));*/
        }

        /// <summary>
        /// Ghi lại lịch sử giao dịch vào DB
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleExchangeEvent1(KPlayer client, int goodsID, int goodsNum, int otherRoleID, string otherRoleName, string result)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}	{9}	{10}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum, //可以是负数
                Global.GetTotalGoodsCountByID(client, goodsID),
                otherRoleID,
                otherRoleName,
                result
                );

            GameManager.SystemRoleExchangeEvents1.AddEvent(eventMsg, EventLevels.Important);

            //SystemXmlItem systemGoods = Global.CanBroadcastOrEventGoods(goodsID);
            //if (null != systemGoods)
            //{
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDEXCHANGE1ITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, goodsID, goodsNum, Global.GetTotalGoodsCountByID(client, goodsID), otherRoleID, result),
                null, client.ServerId);
            //}
        }

        public static void AddRoleExchangeEvent3(KPlayer client, int yuanBao, int otherRoleID, string otherRoleName)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                yuanBao, //可以是负数
                client.Token,
                otherRoleID,
                otherRoleName
                );

            GameManager.SystemRoleExchangeEvents3.AddEvent(eventMsg, EventLevels.Important);

            /*string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheEvent_exchange3(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                yuanBao,
                client.Token,
                otherRoleID,
                otherRoleName,
                writerTime);*/

            //判断物品是否播报或者记录
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDEXCHANGE3ITEM,
                string.Format("{0}:{1}:{2}:{3}", client.RoleID, yuanBao, client.Token, otherRoleID),
                null, client.ServerId);
        }

        /// <summary>
        /// 写入角色升级的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleUpgradeEvent(KPlayer client, int oldLevel)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}	{9}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                oldLevel,
                client.m_Level,
                client.m_Experience,
                client.MapCode,
                client.PosX,
                client.PosY
                );

            GameManager.SystemRoleUpgradeEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色物品的得失行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleGoodsEvent(KPlayer client, int goodsDbID, int goodsID, int goodsNum, int binding, int quality, int forgeLevel, string jewelList, int site, string endTime,
            int addOrSubGoodsNum, string actionDesc, int addPropIndex, int bornIndex, int lucky, int strong, int ExcellenceProperty, int nAppendPropLev, int nChangeLifeLevForEquip)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            Global.AddRoleGoodsEvent(userID, client.RoleID, client.RoleName, client.m_Level,
                goodsDbID, goodsID, goodsNum, binding, quality, forgeLevel, jewelList, site, endTime,
                addOrSubGoodsNum, actionDesc, addPropIndex, bornIndex, lucky, strong, ExcellenceProperty, nAppendPropLev, nChangeLifeLevForEquip);
        }

        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleGoodsEvent(string userID, int roleID, string roleName, int roleLevel, int goodsDbID, int goodsID, int goodsNum, int binding, int quality, int forgeLevel, string jewelList, int site, string endTime,
            int addOrSubGoodsNum, string actionDesc, int addPropIndex, int bornIndex, int lucky, int strong, int ExcellenceProperty, int nAppendPropLev, int nChangeLifeLevForEquip)
        {
            string msg = WithRname ? "{0}	{1}	{2} {3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}	{13}	{14}	{15}	{16}	{17}	{18}	{19}	{20}	{21}	{22}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}	{13}	{14}	{15}	{16}	{17}	{18}	{19}	{20}	{21}	{22}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                roleID,
                roleName,
                roleLevel,
                goodsDbID,
                goodsID,
                goodsNum, //可以是负数
                binding,
                quality,
                forgeLevel,
                jewelList,
                site,
                endTime,
                addOrSubGoodsNum,
                actionDesc,
                addPropIndex,
                bornIndex,
                lucky,
                strong,
                ExcellenceProperty,
                nAppendPropLev,
                nChangeLifeLevForEquip);

            GameManager.SystemRoleGoodsEvents.AddEvent(eventMsg, EventLevels.Important);

            if (/**/"重置背包索引" != actionDesc)
            {
                //string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
                //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                //GameManager.DBEventsWriter.CacheEvent_goods(-1,
                //    client.ZoneID,
                //    userID,
                //    userName,
                //    client.RoleID,
                //    client.RoleName,
                //    goodsDbID,
                //    goodsID,
                //    goodsNum, //可以是负数
                //    binding,
                //    quality,
                //    forgeLevel,
                //    jewelList,
                //    site,
                //    endTime,
                //    addOrSubGoodsNum,
                //    actionDesc,
                //    writerTime);
            }
        }

        /// <summary>
        /// 写入角色物品的挖宝的物品日志(只有全区播报的物品才写)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddWaBaoGoodsEvent(KPlayer client, GoodsData goodsData)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsData.Id,
                goodsData.GoodsID,
                goodsData.GCount,
                goodsData.Binding,

                goodsData.Forge_level,

                goodsData.Site
                );

            GameManager.SystemRoleWaBaoEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的地图日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddMapEvent(KPlayer client)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            // rid和rname共存的 去掉rname
            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}" : "{0}	{1}	{2}	{4}	{5}";
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode));

            GameManager.SystemRoleMapEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的副本奖励日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddFuBenAwardEvent(KPlayer client, int fuBenID)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                fuBenID);

            GameManager.SystemRoleFuBenAwardEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的跑环日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddPaoHuanEvent(KPlayer client, int taskClass)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}" : "{0}	{1}	{2}	{4}	{5}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                taskClass);

            GameManager.SystemRolePaoHuanOkEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的押镖日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddYaBiaoEvent(KPlayer client, int yaBiaoID, string action)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                yaBiaoID,
                action);

            GameManager.SystemRoleYaBiaoEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的连斩日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddLianZhanEvent(KPlayer client, int lianZhanNum)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                lianZhanNum);

            GameManager.SystemRoleLianZhanEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色的活动日志
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddHuoDongEvent(KPlayer client, int monsterType, string monsterName)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                monsterType,
                monsterName);

            GameManager.SystemRoleHuoDongMonsterEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 服务器端角色精雕细琢[钥匙类]挖宝事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddDigTreasureWithYaoShiEvent(KPlayer client, int idYaoShi, int idXiangZi, int needSubYaoShiNum, int needSubXiangZiNum, int subMoney, int oldMoney, int nowMoney, GoodsData goodsData)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10} {11}	{12}	{13}	{14}	{15}	{16}	{17} {18}	{19} {20}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                idYaoShi, idXiangZi, needSubYaoShiNum, needSubXiangZiNum, subMoney, oldMoney, nowMoney,
                goodsData.Id,
                goodsData.GoodsID,
                goodsData.GCount,
                goodsData.Binding,

                goodsData.Forge_level,

                goodsData.Site);

            GameManager.SystemRoleDigTreasureWithYaoShiEvents.AddEvent(eventMsg, EventLevels.Important);

            /*string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheEvent_yaoshidigtreasure(-1,
                client.ZoneID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                idYaoShi, idXiangZi, needSubYaoShiNum, needSubXiangZiNum, subMoney, oldMoney, nowMoney,
                goodsData.Id,
                goodsData.GoodsID,
                goodsData.GCount,
                goodsData.Binding,
                goodsData.Quality,
                goodsData.Forge_level,
                goodsData.Jewellist,
                goodsData.Site,
                writerTime);*/
        }

        /// <summary>
        /// 服务器端自动扣除元宝事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddAutoSubYuanBaoEvent(KPlayer client, int GoodsID, int GoodsNum, int price, int subYuanBao, string reason, int oldMoney, int nowMoney)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                GoodsID, GoodsNum, price, subYuanBao, reason, oldMoney, nowMoney);

            GameManager.SystemRoleAutoSubYuanBaoEvents.AddEvent(eventMsg, EventLevels.Important);

            /*string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheEvent_autosubyuanbao(-1,
                client.ZoneID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                GoodsID, GoodsNum, price, subYuanBao, reason, oldMoney, nowMoney,
                writerTime);*/
        }

        /// <summary>
        /// 服务器端自动扣除金币事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddAutoSubBoundMoneyEvent(KPlayer client, int GoodsID, int GoodsNum, int price, int subBoundMoney, string reason, int oldMoney, int nowMoney)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                GoodsID, GoodsNum, price, subBoundMoney, reason, oldMoney, nowMoney);

            GameManager.SystemRoleAutoSubBoundMoneyEvents.AddEvent(eventMsg, EventLevels.Important);

            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheEvent_autosubBoundMoney(-1,
            //    client.ZoneID,
            //    userID,
            //    client.RoleID,
            //    client.RoleName,
            //    client.m_Level,
            //    Global.GetMapName(client.MapCode),
            //    GoodsID, GoodsNum, price, subBoundMoney, reason, oldMoney, nowMoney,
            //    writerTime);
        }

        /// <summary>
        /// 服务器端自动扣除金币-元宝事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddAutoSubEvent(KPlayer client, int GoodsID, int GoodsNum, int price, int subBoundMoney, int oldBoundMoney, int nowBoundMoney, int subYuanBao, int oldYuanBao, int nowYuanBao, string reason)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}	{11}	{12}	{13}	{14}	{15}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                GoodsID, GoodsNum, price, subBoundMoney, oldBoundMoney, nowBoundMoney, subYuanBao, oldYuanBao, nowYuanBao, reason);

            GameManager.SystemRoleAutoSubEvents.AddEvent(eventMsg, EventLevels.Important);

            //string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            //GameManager.DBEventsWriter.CacheEvent_autosubBoundMoney(-1,
            //    client.ZoneID,
            //    userID,
            //    client.RoleID,
            //    client.RoleName,
            //    client.m_Level,
            //    Global.GetMapName(client.MapCode),
            //    GoodsID, GoodsNum, price, subBoundMoney, reason, oldMoney, nowMoney,
            //    writerTime);
        }

        /// <summary>
        /// 将生肖运程竞猜结果提交给统计数据库
        /// </summary>
        public static void AddShengXiaoGuessHistoryToStaticsDB(KPlayer client, int roleID, int guessKey, int mortgage, int resultkey, int gainnum, int nowBoundMoney)
        {
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            //GameManager.DBEventsWriter.CacheShengxiaoguesshist(-1,
            //    roleID, client != null? client.RoleName:"", client !=null? client.ZoneID : -1, guessKey, mortgage,
            //    resultkey, gainnum, nowBoundMoney, writerTime);
        }

        /// <summary>
        /// 角色提取邮件元宝，银两，铜钱事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddRoleFetchMailMoneyEvent(KPlayer client, int yuanBao, int BoundToken, int tongQian)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                yuanBao, BoundToken, tongQian);

            GameManager.SystemRoleFetchMailMoneyEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 角色领取每日vip奖励的元宝，银两，铜钱事件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void AddRoleFetchVipAwardEvent(KPlayer client, int yuanBao, int BoundToken, int tongQian, int lingLi, int priority)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string msg = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}	{9}	{10}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}	{9}	{10}";
            // rid和rname共存的 去掉rname
            string eventMsg = string.Format(msg,
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                Global.GetMapName(client.MapCode),
                yuanBao, BoundToken, tongQian, lingLi, priority);

            GameManager.SystemRoleFetchVipAwardEvents.AddEvent(eventMsg, EventLevels.Important);
        }

        /// <summary>
        /// 写入角色生肖运程竞猜结果的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleShengXiaoGuessHistoryEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice)
        {
            /*
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token
                );

            GameManager.SystemRoleShengXiaoGuessEvents.AddEvent(eventMsg, EventLevels.Important);

            //更新上线状态
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDQIZHENGEBUYITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, goodsID, goodsNum, totalPrice, client.Token),
                null);

            string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheQizhengebuy(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token,
                writerTime,
                Global.GetGoodsNameByID(goodsID));
            */
        }

        /// <summary>
        /// 写入角色元宝在商城抢购购买的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleQiangGouBuyWithYuanBaoEvent(KPlayer client, int goodsID, int goodsNum, int totalPrice, int qiangGouID)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

            string eventMsg = string.Format("{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8} {9}",
                GameManager.ServerLineID,
                userID,
                client.RoleID,
                client.RoleName,
                client.m_Level,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token,
                qiangGouID
                );

            GameManager.SystemRoleQiangGouBuyWithYuanBaoEvents.AddEvent(eventMsg, EventLevels.Important);

            /*string userName = GameManager.OnlineUserSession.FindUserName(client.ClientSocket);
            string writerTime = TimeUtil.NowDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            GameManager.DBEventsWriter.CacheQianggoubuy(-1,
                client.ZoneID,
                userID,
                userName,
                client.RoleID,
                client.RoleName,
                goodsID,
                goodsNum,
                totalPrice,
                client.Token,
                writerTime,
                Global.GetGoodsNameByID(goodsID),
                qiangGouID);*/
        }

        /// <summary>
        /// 写入角色元宝的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleTokenEvent(KPlayer client, string type, int Token, string msg)
        {
            try
            {
                int hasTotalMoney = client.Token;

                string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);

                // rid和rname共存的 去掉rname
                string msg2 = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
                string eventMsg = string.Format(msg2,
                    GameManager.ServerLineID,
                    userID,
                    client.RoleID,
                    client.RoleName,
                    client.m_Level,
                    type,
                    Token,
                    hasTotalMoney,
                    msg
                    );

                GameManager.SystemRoleTokenEvents.AddEvent(eventMsg, EventLevels.Important);
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// 写入角色元宝的行为日志
        /// </summary>
        /// <param name="client"></param>
        public static void AddRoleTokenEvent(int roleID, string type, int Token, string msg)
        {
            try
            {
                int hasTotalMoney = -1;

                string userID = "";
                // rid和rname共存的 去掉rname
                string msg2 = WithRname ? "{0}	{1}	{2}	{3}	{4}	{5}	{6}	{7}	{8}" : "{0}	{1}	{2}	{4}	{5}	{6}	{7}	{8}";
                string eventMsg = string.Format(msg2,
                    GameManager.ServerLineID,
                    userID,
                    roleID,
                    "",
                    -1,
                    type,
                    Token,
                    hasTotalMoney,
                    msg
                    );

                GameManager.SystemRoleTokenEvents.AddEvent(eventMsg, EventLevels.Important);
            }
            catch (System.Exception)
            {
            }
        }

        #endregion 角色行为日志

        #region 第一次重置任务管理

        /// <summary>
        /// Nhiệm vụ nạp thẻ lần đầu
        /// </summary>
        /// <param name="taskID"></param>
        public static void JugeCompleteChongZhiSecondTask(KPlayer client, int taskID)
        {
            // Console.WriteLine("TASK ID J DO ATTACKKKKKKKKKKKK!");
        }

        /// <summary>
        /// 判断在这个区，平台账户是否领取了首充大礼
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool CanGetFirstChongZhiDaLiByUserID(KPlayer client)
        {
            //先DBServer请求查询角色
            string[] dbRoleFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYFIRSTCHONGZHIBYUSERID, string.Format("{0}", client.RoleID), client.ServerId);
            if (null == dbRoleFields || dbRoleFields.Length != 1 || int.Parse(dbRoleFields[0]) <= 0)
            {
                return true;
            }

            return false;
        }

        #endregion 第一次重置任务管理




        #region 掉落物品是否播报或者记录缓存

        /// <summary>
        /// 判断物品是否播报或者记录
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static SystemXmlItem CanBroadcastOrEventGoods(GoodsData goodsData)
        {
            return Global.CanBroadcastOrEventGoods(goodsData.GoodsID);
        }

        /// <summary>
        /// 判断物品是否播报或者记录
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static SystemXmlItem CanBroadcastOrEventGoods(int goodsID)
        {
            SystemXmlItem systemGoods = null;

            int infoClass = systemGoods.GetIntValue("InfoClass");
            //int categoriy = systemGoods.GetIntValue("Categoriy");
            //if (categoriy >= (int)ItemCategories.Weapon && categoriy < (int)ItemCategories.EquipMax)
            //{
            //    if (infoClass <= 0)
            //    {
            //        //如果是装备，则必须是紫色或则+6以上才播报
            //        if (goodsData.Quality < (int)GoodsQuality.Purple && goodsData.Forge_level < 6 && goodsData.BornIndex < 100)
            //        {
            //            return null;
            //        }
            //    }
            //}
            //else
            {
                if (infoClass <= 0)
                {
                    return null;
                }
            }

            return systemGoods;
        }

        #endregion 掉落物品是否播报或者记录缓存


        #region 构造角色名称

        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName(KPlayer client, string roleName)
        {
            return FormatRoleName2(client, roleName);
        }

        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName2(KPlayer clientData, string roleName)
        {
            //return StringUtil.substitute(Global.GetLang("[{0}区]{1}"), clientData.ZoneID, roleName);
            return roleName;
        }

        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName3(int zoneID, string roleName)
        {
            //return StringUtil.substitute(Global.GetLang("[{0}区]{1}"), zoneID, roleName);
            return roleName;
        }

        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName4(KPlayer client)
        {
            return FormatRoleName2(client, client.RoleName);
        }

        /// <summary>
        /// Format tên hiển thị theo Server (dùng ở Liên Server)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleNameWithZoneId(KPlayer client)
        {
            return StringUtil.substitute(Global.GetLang("<color=#e425e4>[S{0}]</color> {1}"), client.ZoneID, client.RoleName);
        }

        #endregion 构造角色名称

        #region Quản lý bang hội

        /// <summary>
        /// Số bạc cần để tạo bang
        /// </summary>
        public static int CreateBangHuiNeedTongQian
        {
            get
            {
                return 500000;
            }
        }

        /// <summary>
        /// 修改帮旗名称需要的铜钱
        /// </summary>
        public static int RenameBangQiNameNeedTongQian = 500000;

        /// <summary>
        /// Cấp độ yêu cầu để vào bang
        /// </summary>
        public static int JoinBangHuiNeedLevel = 0;

        /// <summary>
        /// Cấp độ yêu cầu tạo bang hội
        /// </summary>
        public static int CreateBangHuiNeedLevel
        {
            get
            {
                return 40;
            }
        }

        /// <summary>
        /// 计算贡献铜钱获取帮贡的最小单位
        /// </summary>
        public static int MinDonateTongQianPerBangGong = 100000;

        /// <summary>
        /// 贡献铜钱时的最小值
        /// </summary>
        public static int MinDonateBangGongTongQian = 10000;

        /// <summary>
        /// 每日贡献铜钱帮贡的最大值
        /// </summary>
        public static int MaxDayTongQianBangGong = 5000;

        /// <summary>
        /// 每日贡献道具帮贡的最大值
        /// </summary>
        public static int MaxDayGoodsBangGong = 10000;

        /// <summary>
        /// Cấp bang tối đa là 10
        /// </summary>
        public static int MaxBangHuiFlagLevel = 10;

        /// <summary>
        /// 插旗消耗
        /// </summary>
        public static int InstallJunQiNeedMoney = 50000;

        /// <summary>
        /// 提取舍利之源铜钱消耗
        /// </summary>
        public static int TakeSheLiZhiYuanNeedMoney = 100000;

        /// <summary>
        /// 帮会mini小数据缓存项
        /// </summary>
        public static Dictionary<int, BangHuiMiniData> DictBangHui = new Dictionary<int, BangHuiMiniData>();

        /// <summary>
        /// 帮会详细数据缓存项
        /// </summary>
        public static Dictionary<int, BangHuiDetailData> BangHuiDetailDataDict = new Dictionary<int, BangHuiDetailData>();

        /// <summary>
        /// Kiểm tra xem có bang hội chưa
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        public static bool IsHavingBangHui(KPlayer client)
        {
            return (client.GuildID > 0 && !string.IsNullOrEmpty(client.GuildName));
        }

        /// <summary>
        /// 返回帮会Mini数据
        /// </summary>
        /// <param name="bangHuiID"></param>
        /// <returns></returns>
        public static BangHuiMiniData GetBangHuiMiniData(int bangHuiID, int serverId = GameManager.LocalServerId)
        {
            BangHuiMiniData bhData = null;
            if (DictBangHui.TryGetValue(bangHuiID, out bhData))
            {
                return bhData;
            }

            byte[] bytesData = null;
            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer3(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                (int)TCPGameServerCmds.CMD_DB_GETBANGHUIMINIDATA, string.Format("{0}", bangHuiID), out bytesData, serverId))
            {
                return null;
            }

            if (null == bytesData || bytesData.Length <= 6)
            {
                return null;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);

            //获取帮会基础信息
            bhData = DataHelper.BytesToObject<BangHuiMiniData>(bytesData, 6, length - 2);

            if (null == bhData)
            {
                return null;
            }

            //锁定再更新
            lock (DictBangHui)
            {
                if (!DictBangHui.ContainsKey(bangHuiID))
                {
                    DictBangHui.Add(bangHuiID, bhData);
                }
            }

            return bhData;
        }

        /// <summary>
        /// 返回帮会详细数据
        /// </summary>
        /// <param name="bangHuiID"></param>
        /// <returns></returns>
        public static BangHuiDetailData GetBangHuiDetailData(int roleID, int bhid, int ServerID = GameManager.LocalServerId)
        {
            BangHuiDetailData bhData = null;
            lock (BangHuiDetailDataDict)
            {
                //if (BangHuiDetailDataDict.TryGetValue(bhid, out bhData))
                //{
                //    return bhData;
                //}

                bhData = Global.SendToDB<BangHuiDetailData, string>((int)TCPGameServerCmds.CMD_SPR_QUERYBANGHUIDETAIL, string.Format("{0}:{1}", roleID, bhid), ServerID);
                //BangHuiDetailDataDict[bhid] = bhData;
            }

            return bhData;
        }

        #endregion Quản lý bang hội



        #region 从语言包中获取语言

        private static object LangDict_Mutex = new object();

        /// <summary>
        /// 快速存取语言字符串的字典
        /// </summary>
        private static Dictionary<string, string> LangDict = null;

        /// <summary>
        /// 从程序资源中加载语言包字典
        /// </summary>
        /// <returns></returns>
        public static void LoadLangDict()
        {
            XElement xml = null;

            try
            {
                xml = XElement.Load("Language.xml");
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                if (null == xml) return;
                Dictionary<string, string> langDict = new Dictionary<string, string>();
                IEnumerable<XElement> langItems = xml.Elements();
                foreach (var langItem in langItems)
                {
                    langDict[Global.GetSafeAttributeStr(langItem, "ChineseText")] = Global.GetSafeAttributeStr(langItem, "OtherLangText");
                }

                lock (LangDict_Mutex)
                {
                    LangDict = langDict;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 将中文转成其他语言
        /// </summary>
        /// <param name="chineseText"></param>
        /// <returns></returns>
        public static string GetLang(string chineseText)
        {
            string otherLangText = "";

            if (null == LangDict)
            {
                return chineseText;
            }

            lock (LangDict_Mutex)
            {
                if (!LangDict.TryGetValue(chineseText, out otherLangText))
                {
                    return chineseText;
                }

                if (string.IsNullOrEmpty(otherLangText))
                {
                    return chineseText;
                }
            }

            return otherLangText;
        }

        #endregion 从语言包中获取语言

        
        #region 奖励活动相关

        /// <summary>
        /// 返回活动请求命令字符串，从gameserver发送给gamedbserver的活动请求，都需要配置好活动参数，因为gamedbserver不读具体逻辑
        /// 配置信息
        /// </summary>
        /// <returns></returns>
        public static string GetActivityRequestCmdString(ActivityTypes type, KPlayer client, int extTag = 0)
        {
            int roleID = client.RoleID;

            string sCmd = "";
            string sCmdFormat = "{0}:{1}:{2}:{3}:{4}";

            KingActivity instanceKing = null;

            switch (type)
            {
                case ActivityTypes.InputFanLi:
                    {
                        InputFanLiActivity instance = HuodongCachingMgr.GetInputFanLiActivity();

                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.FanLiPersent, extTag);
                        }

                        break;
                    }
                case ActivityTypes.InputJiaSong:
                    {
                        InputSongActivity instance = HuodongCachingMgr.GetInputSongActivity();

                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.MyAwardItem.MinAwardCondionValue, extTag);
                        }

                        break;
                    }
                case ActivityTypes.NewZoneRechargeKing:
                case ActivityTypes.InputKing:  //gwz
                    {
                        instanceKing = HuodongCachingMgr.GetInputKingActivity();
                        extTag = (int)ActivityTypes.InputKing;
                        break;
                    }
                case ActivityTypes.LevelKing:
                    {
                        instanceKing = HuodongCachingMgr.GetLevelKingActivity();
                        break;
                    }
                case ActivityTypes.NewZoneBosskillKing:
                case ActivityTypes.EquipKing: //gwz
                    {
                        instanceKing = HuodongCachingMgr.GetEquipKingActivity();
                        extTag = (int)ActivityTypes.EquipKing;
                        break;
                    }
                case ActivityTypes.HorseKing:
                    {
                        instanceKing = HuodongCachingMgr.GetHorseKingActivity();
                        break;
                    }
                case ActivityTypes.JingMaiKing:
                    {
                        instanceKing = HuodongCachingMgr.GetJingMaiKingActivity();
                        break;
                    }
                case ActivityTypes.JieriDaLiBao:
                    {
                        JieriDaLiBaoActivity instance = HuodongCachingMgr.GetJieriDaLiBaoActivity();

                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.MyAwardItem.MinAwardCondionValue, extTag);
                        }

                        break;
                    }
                case ActivityTypes.JieriDengLuHaoLi:
                    {
                        JieRiDengLuActivity instance = HuodongCachingMgr.GetJieRiDengLuActivity();

                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), Global.GetRoleParamsInt32FromDB(client, RoleParamName.JieriLoginNum), extTag);
                        }
                        break;
                    }
                case ActivityTypes.JieriCZSong:
                    {
                        JieriCZSongActivity instance = HuodongCachingMgr.GetJieriCZSongActivity();

                        if (null != instance)
                        {
                            //sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.MyAwardItem.MinAwardCondionValue, extTag);
                            // 今天
                            string FromDate = new DateTime(TimeUtil.NowDateTime().Year, TimeUtil.NowDateTime().Month, TimeUtil.NowDateTime().Day, 0, 0, 0).ToString();
                            string ToDate = new DateTime(TimeUtil.NowDateTime().Year, TimeUtil.NowDateTime().Month, TimeUtil.NowDateTime().Day, 23, 59, 59).ToString();
                            AwardItem myAwardItem = instance.GetAward(extTag);
                            if (null != myAwardItem)
                                sCmd = string.Format(sCmdFormat, roleID, FromDate.Replace(':', '$'), ToDate.Replace(':', '$'), myAwardItem.MinAwardCondionValue, extTag);
                            else
                                sCmd = string.Format(sCmdFormat, roleID, FromDate.Replace(':', '$'), ToDate.Replace(':', '$'), 0, extTag);
                        }

                        break;
                    }
                case ActivityTypes.JieriLeiJiCZ:
                    {
                        // instanceKing = HuodongCachingMgr.GetJieRiLeiJiCZActivity();
                        JieRiLeiJiCZActivity instance = HuodongCachingMgr.GetJieRiLeiJiCZActivity();
                        if (null != instance)
                        {
                            AwardItem myAwardItem = instance.GetAward(extTag);
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.GetAwardMinConditionValues(), extTag);
                        }
                        break;
                    }
                case ActivityTypes.JieriTotalConsume:
                    {
                        JieRiTotalConsumeActivity instance = HuodongCachingMgr.GetJieRiTotalConsumeActivity();
                        if (null != instance)
                        {
                            AwardItem myAwardItem = instance.GetAward(extTag);
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.GetAwardMinConditionValues(), extTag);
                        }
                        break;
                    }
                case ActivityTypes.JieriZiKa:
                    {
                        break;
                    }
                case ActivityTypes.NewZoneConsumeKing:
                    {
                        instanceKing = HuodongCachingMgr.GetXinXiaoFeiKingActivity();
                        break;
                    }
                case ActivityTypes.JieriPTXiaoFeiKing://gwz:
                    {
                        instanceKing = HuodongCachingMgr.GetJieriXiaoFeiKingActivity();
                        sCmd = string.Format(sCmdFormat, roleID, instanceKing.FromDate.Replace(':', '$'), instanceKing.ToDate.Replace(':', '$'), instanceKing.GetAwardMinConditionValues(), extTag);
                        return sCmd;
                    }

                case ActivityTypes.JieriPTCZKing:
                    {
                        instanceKing = HuodongCachingMgr.GetJieRiCZKingActivity();
                        sCmd = string.Format(sCmdFormat, roleID, instanceKing.FromDate.Replace(':', '$'), instanceKing.ToDate.Replace(':', '$'), instanceKing.GetAwardMinConditionValues(), extTag);
                        return sCmd;
                    }
                case ActivityTypes.JieriBossAttack:
                    {
                        break;
                    }
                case ActivityTypes.HeFuLogin:
                    {
                        HeFuLoginActivity instance = HuodongCachingMgr.GetHeFuLoginActivity();

                        if (null != instance)
                        {
                            //sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.NormalAward.MinAwardCondionValue, extTag);
                        }

                        break;
                    }
                //case ActivityTypes.HeFuVIP:
                //    {
                //        HeFuBattleGodActivity instance = HuodongCachingMgr.GetHeFuVIPActivity();

                //        if (null != instance)
                //        {
                //            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), Global.IsVip(client) ? 1 : 0, extTag);
                //        }

                //        break;
                //    }
                case ActivityTypes.HeFuTotalLogin:
                    {
                        HeFuTotalLoginActivity instance = HuodongCachingMgr.GetHeFuTotalLoginActivity();

                        if (null != instance)
                        {
                            //sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), instance.MyAwardItem.MinAwardCondionValue, extTag);
                        }

                        break;
                    }
                case ActivityTypes.HeFuPKKing:
                    {
                        HeFuPKKingActivity instance = HuodongCachingMgr.GetHeFuPKKingActivity();

                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), HuodongCachingMgr.GetHeFuPKKingRoleID(), extTag);
                        }

                        break;
                    }
                //case ActivityTypes.HeFuWanChengKing:
                //    {
                //        HeFuWCKingActivity instance = HuodongCachingMgr.GetHeFuWCKingActivity();

                //        if (null != instance)
                //        {
                //            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), 0, extTag);
                //        }

                //        break;
                //    }
                case ActivityTypes.HeFuRecharge:
                    {
                        /*HeFuRechargeActivity instance = HuodongCachingMgr.GetHeFuRechargeActivity();
                        if (null != instance)
                        {
                            sCmd = string.Format(sCmdFormat, roleID, instance.strcoe, "0", extTag);
                        }*/
                        break;
                    }
                case ActivityTypes.HeFuBossAttack:
                    {
                        break;
                    }
                case ActivityTypes.MeiRiChongZhiHaoLi:
                    {
                        break;
                    }
                case ActivityTypes.NewZoneUpLevelMadman:
                case ActivityTypes.ChongJiLingQuShenZhuang://gwz
                    {
                        instanceKing = HuodongCachingMgr.GetChongJiHaoLiActivity();
                        break;
                    }
                case ActivityTypes.ShenZhuangJiQingHuiKui:
                    {
                        break;
                    }
                case ActivityTypes.XingYunChouJiang:
                    {
                        break;
                    }
                case ActivityTypes.YuDuZhuanPanChouJiang:
                    {
                        YueDuZhuanPanActivity instance = HuodongCachingMgr.GetYueDuZhuanPanActivity();

                        if (null != instance)
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), 0, extTag);

                        break;
                    }

                case ActivityTypes.NewZoneFanli://gwz
                case ActivityTypes.XinCZFanLi:
                    {
                        instanceKing = HuodongCachingMgr.GetXinFanLiActivity();
                        break;
                    }

                case ActivityTypes.JieriWing:
                case ActivityTypes.JieriAddon:
                case ActivityTypes.JieriStrengthen:
                case ActivityTypes.JieriAchievement:
                case ActivityTypes.JieriMilitaryRank:
                case ActivityTypes.JieriVIPFanli:
                case ActivityTypes.JieriAmulet:
                case ActivityTypes.JieriArchangel:
                case ActivityTypes.JieriMarriage:
                    {
                        JieriFanLiActivity instance = HuodongCachingMgr.GetJieriFanLiActivity(type);
                        if (null != instance)
                        {
                            AwardItem myAwardItem = instance.GetAward(extTag);
                            sCmd = string.Format(sCmdFormat, roleID, instance.FromDate.Replace(':', '$'), instance.ToDate.Replace(':', '$'), (int)type, extTag);
                        }
                        break;
                    }
                case ActivityTypes.JieriInputPointsExchg:
                    {
                        break;
                    }

                default:
                    break;
            }

            //对王类活动统一处理
            if (null != instanceKing)
            {
                sCmd = string.Format(sCmdFormat, roleID, instanceKing.FromDate.Replace(':', '$'), instanceKing.ToDate.Replace(':', '$'), instanceKing.GetAwardMinConditionValues(), (int)type);
            }

            return sCmd;
        }

        /// <summary>
        /// 判断当前是否可以在给予奖励时间范围内，即是否活动的奖励期
        /// </summary>
        /// <returns></returns>
        public static Activity GetActivity(ActivityTypes type)
        {
            Activity instance = null;

            switch (type)
            {
                case ActivityTypes.InputFirst:
                    {
                        instance = HuodongCachingMgr.GetFirstChongZhiActivity();
                        break;
                    }
                case ActivityTypes.InputFanLi:
                    {
                        instance = HuodongCachingMgr.GetInputFanLiActivity();
                        break;
                    }
                case ActivityTypes.InputJiaSong:
                    {
                        instance = HuodongCachingMgr.GetInputSongActivity();
                        break;
                    }
                case ActivityTypes.NewZoneRechargeKing:
                case ActivityTypes.InputKing:
                    {
                        instance = HuodongCachingMgr.GetInputKingActivity();
                        break;
                    }
                case ActivityTypes.LevelKing:
                    {
                        instance = HuodongCachingMgr.GetLevelKingActivity();
                        break;
                    }
                case ActivityTypes.NewZoneBosskillKing:
                case ActivityTypes.EquipKing:
                    {
                        instance = HuodongCachingMgr.GetEquipKingActivity();
                        break;
                    }
                case ActivityTypes.HorseKing:
                    {
                        instance = HuodongCachingMgr.GetHorseKingActivity();
                        break;
                    }
                case ActivityTypes.JingMaiKing:
                    {
                        instance = HuodongCachingMgr.GetJingMaiKingActivity();
                        break;
                    }
                case ActivityTypes.JieriDaLiBao:
                    {
                        instance = HuodongCachingMgr.GetJieriDaLiBaoActivity();
                        break;
                    }
                case ActivityTypes.JieriDengLuHaoLi:
                    {
                        instance = HuodongCachingMgr.GetJieRiDengLuActivity();
                        break;
                    }
                case ActivityTypes.JieriVIP:
                    {
                        instance = HuodongCachingMgr.GetJieriVIPActivity();
                        break;
                    }
                case ActivityTypes.JieriCZSong:
                    {
                        instance = HuodongCachingMgr.GetJieriCZSongActivity();
                        break;
                    }
                case ActivityTypes.JieriLeiJiCZ:
                    {
                        instance = HuodongCachingMgr.GetJieRiLeiJiCZActivity();
                        break;
                    }
                case ActivityTypes.JieriZiKa:
                    {
                        instance = HuodongCachingMgr.GetJieRiZiKaLiaBaoActivity();
                        break;
                    }
                case ActivityTypes.NewZoneConsumeKing://gwz
                    {
                        instance = HuodongCachingMgr.GetXinXiaoFeiKingActivity();
                        break;
                    }
                case ActivityTypes.JieriPTXiaoFeiKing:
                    {
                        instance = HuodongCachingMgr.GetJieriXiaoFeiKingActivity();
                        break;
                    }
                case ActivityTypes.JieriPTCZKing:
                    {
                        instance = HuodongCachingMgr.GetJieRiCZKingActivity();
                        break;
                    }
                case ActivityTypes.JieriBossAttack:
                    {
                        break;
                    }
                case ActivityTypes.HeFuLogin:
                    {
                        instance = HuodongCachingMgr.GetHeFuLoginActivity();
                        break;
                    }
                //case ActivityTypes.HeFuVIP:
                //    {
                //        instance = HuodongCachingMgr.GetHeFuVIPActivity();
                //        break;
                //    }
                case ActivityTypes.HeFuTotalLogin:
                    {
                        instance = HuodongCachingMgr.GetHeFuTotalLoginActivity();
                        break;
                    }
                case ActivityTypes.HeFuRecharge:
                    {
                        instance = HuodongCachingMgr.GetHeFuRechargeActivity();
                        break;
                    }
                case ActivityTypes.HeFuPKKing:
                    {
                        instance = HuodongCachingMgr.GetHeFuPKKingActivity();
                        break;
                    }
                //case ActivityTypes.HeFuWanChengKing:
                //    {
                //        instance = HuodongCachingMgr.GetHeFuWCKingActivity();
                //        break;
                //    }
                case ActivityTypes.HeFuBossAttack:
                    {
                        break;
                    }
                case ActivityTypes.MeiRiChongZhiHaoLi:          // 每日充值豪礼[7/16/2013 LiaoWei]
                    {
                        instance = HuodongCachingMgr.GetMeiRiChongZhiActivity();
                        break;
                    }
                case ActivityTypes.NewZoneUpLevelMadman: //gwz
                case ActivityTypes.ChongJiLingQuShenZhuang:
                    {
                        instance = HuodongCachingMgr.GetChongJiHaoLiActivity();
                        break;
                    }
                case ActivityTypes.ShenZhuangJiQingHuiKui:
                    {
                        instance = HuodongCachingMgr.GetShenZhuangJiQiHuiKuiHaoLiActivity();
                        break;
                    }
                case ActivityTypes.NewZoneFanli://gwz
                case ActivityTypes.XinCZFanLi:
                    {
                        instance = HuodongCachingMgr.GetXinFanLiActivity();
                        break;
                    }
                case ActivityTypes.YuDuZhuanPanChouJiang:
                    {
                        instance = HuodongCachingMgr.GetYueDuZhuanPanActivity();
                        break;
                    }
                case ActivityTypes.TotalCharge:
                    {
                        instance = HuodongCachingMgr.GetTotalChargeActivity();
                        break;
                    }
                case ActivityTypes.TotalConsume:
                    {
                        instance = HuodongCachingMgr.GetTotalConsumeActivity();
                        break;
                    }
                case ActivityTypes.JieriTotalConsume:
                    {
                        instance = HuodongCachingMgr.GetJieRiTotalConsumeActivity();
                        break;
                    }
                case ActivityTypes.JieriDuoBei:
                    {
                        instance = HuodongCachingMgr.GetJieRiMultAwardActivity();
                        break;
                    }
                case ActivityTypes.HeFuLuoLan:
                    {
                        instance = HuodongCachingMgr.GetHeFuLuoLanActivity();
                        break;
                    }

                case ActivityTypes.JieriWing:
                case ActivityTypes.JieriAddon:
                case ActivityTypes.JieriStrengthen:
                case ActivityTypes.JieriAchievement:
                case ActivityTypes.JieriMilitaryRank:
                case ActivityTypes.JieriVIPFanli:
                case ActivityTypes.JieriAmulet:
                case ActivityTypes.JieriArchangel:
                case ActivityTypes.JieriMarriage:
                    {
                        instance = HuodongCachingMgr.GetJieriFanLiActivity(type);
                        break;
                    }
                case ActivityTypes.JieriInputPointsExchg:
                    {
                        break;
                    }
                default:
                    break;
            }

            return instance;
        }

        /// <summary>
        /// 返回dbserver处理活动奖励的命令ID
        /// </summary>
        /// <returns></returns>
        public static Int32 GetDBServerExecuteActivityAwardCmdID(ActivityTypes type)
        {
            Int32 nID = -1;

            switch (type)
            {
                case ActivityTypes.InputFanLi:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTFANLI;
                        break;
                    }
                case ActivityTypes.InputJiaSong:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTJIASONG;
                        break;
                    }
                case ActivityTypes.NewZoneRechargeKing:
                case ActivityTypes.InputKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTKING;
                        break;
                    }
                case ActivityTypes.ChongJiLingQuShenZhuang://gwz
                case ActivityTypes.LevelKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTELEVELKING;
                        break;
                    }
                case ActivityTypes.EquipKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEEQUIPKING;
                        break;
                    }
                case ActivityTypes.HorseKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEHORSEKING;
                        break;
                    }
                case ActivityTypes.JingMaiKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJINGMAIKING;
                        break;
                    }
                case ActivityTypes.JieriDaLiBao:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIDALIBAO;
                        break;
                    }
                case ActivityTypes.JieriDengLuHaoLi:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIDENGLU;
                        break;
                    }
                case ActivityTypes.JieriVIP:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIVIP;
                        break;
                    }
                case ActivityTypes.JieriCZSong:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZSONG;
                        break;
                    }
                case ActivityTypes.JieriLeiJiCZ:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZLEIJI;
                        break;
                    }
                case ActivityTypes.JieriTotalConsume:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERITOTALCONSUME;
                        break;
                    }
                case ActivityTypes.JieriZiKa:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIZIKA;
                        break;
                    }
                case ActivityTypes.JieriPTXiaoFeiKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIXIAOFEIKING;
                        break;
                    }
                case ActivityTypes.JieriPTCZKing:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZKING;
                        break;
                    }
                //case ActivityTypes.HeFuDaLiBao:
                //    {
                //        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUDALIBAO;
                //        break;
                //    }
                //case ActivityTypes.HeFuVIP:
                //    {
                //        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUVIP;
                //        break;
                //    }
                //case ActivityTypes.HeFuCZSong:
                //    {
                //        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUCZSONG;
                //        break;
                //    }
                //case ActivityTypes.HeFuPKKing:
                //    {
                //        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUPKKING;
                //        break;
                //    }
                //case ActivityTypes.HeFuWanChengKing:
                //    {
                //        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUWCKING;
                //        break;
                //    }
                case ActivityTypes.HeFuRecharge:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUHEFUFANLI;
                        break;
                    }
                case ActivityTypes.NewZoneFanli://gwz
                case ActivityTypes.XinCZFanLi:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_SPR_EXECUXINFANLI;
                        break;
                    }

                case ActivityTypes.JieriWing:
                case ActivityTypes.JieriAddon:
                case ActivityTypes.JieriStrengthen:
                case ActivityTypes.JieriAchievement:
                case ActivityTypes.JieriMilitaryRank:
                case ActivityTypes.JieriVIPFanli:
                case ActivityTypes.JieriAmulet:
                case ActivityTypes.JieriArchangel:
                case ActivityTypes.JieriMarriage:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_DB_EXECUXJIERIFANLI;
                        break;
                    }
                case ActivityTypes.JieriInputPointsExchg:
                    {
                        nID = (Int32)TCPGameServerCmds.CMD_DB_INPUTPOINTS_EXCHANGE;
                        break;
                    }

                default:
                    break;
            }

            return nID;
        }

        #endregion 奖励活动相关

        #region 杨公宝库积分奖励相关

        /// <summary>
        /// 判断能否获给予杨公宝库积分的奖励
        /// </summary>
        /// <param name="client"></param>
        /// <param name="priority"></param>
        /// <returns></returns>

        /// <summary>
        /// 返回积分奖励领取掩码值
        /// </summary>
        /// <param name="awardNo"></param>
        /// <returns></returns>
        public static long GetYangGongBKJiFenMaskValue(int awardNo)
        {
            //采用配置索引作为奖励判断依据
            List<int> ls = GameManager.systemLuckyAwardMgr.SystemXmlItemDict.Keys.ToList();
            ls.Sort();//从小到大排序
            int index = ls.IndexOf(awardNo);

            //小于0一旦出现，就返回第一项，awardNo在调用这个函数之前需要做合法性验证
            //if (index < 0)
            //左移得到掩码值
            long mask = 0x0000000000000001;
            if (index > 0)
            {
                mask = mask << index;//不是移动字节，而是移动位，一位一位的移动，每个index代表了index个位（bit）
            }

            return mask;
        }

        /// <summary>
        /// 返回杨公宝库对应奖励物品需要的背包位置数量
        /// </summary>
        /// <param name="awardNo"></param>
        /// <returns></returns>
        public static int GetYangGongBkAwardGoodsNum(int awardNo)
        {
            SystemXmlItem xmlItem = null;

            //没有相关奖励配置
            if (!GameManager.systemLuckyAwardMgr.SystemXmlItemDict.TryGetValue(awardNo, out xmlItem))
            {
                return 0;
            }

            //提取奖励配置信息
            string goodsIDs = xmlItem.GetStringValue("GoodsIDs");

            return goodsIDs.Split('|').Count();
        }

        /// <summary>
        /// 给予杨公宝库积分对应的奖励
        /// </summary>
        /// <param name="client"></param>
        /// <param name="priority"></param>
        public static bool GiveYangGongBKAwardForDailyJiFen(KPlayer client, int awardNo)
        {
            SystemXmlItem xmlItem = null;

            //没有相关奖励配置
            if (!GameManager.systemLuckyAwardMgr.SystemXmlItemDict.TryGetValue(awardNo, out xmlItem))
            {
                return false;
            }

            //提取奖励配置信息
            string goodsIDs = xmlItem.GetStringValue("GoodsIDs");

            List<GoodsData> listGoods = Global.ParseGoodsDataListFromGoodsStr(goodsIDs, "杨公宝库每日积分奖励配置文件");

            string awardReason = /**/"杨公宝库每日积分奖励";

            //领取物品
            for (int n = 0; n < listGoods.Count; n++)
            {
                GoodsData goodsData = listGoods[n];

                if (null == goodsData)
                {
                    continue;
                }
            }

            return true;
        }

        /// <summary>
        /// 更新杨公宝库积分奖励日常数据, awardNo小于0，表示不直接提交给dbserver，否则增加奖励历史并提交
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dayID"></param>
        /// <param name="priority"></param>

        #endregion 杨公宝库积分奖励相关

        #region 角色参数管理(注意太频繁的更新，需要外部控制是否实时提交)

        /// <summary>
        /// 获取角色参数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static string GetRoleParamByName(KPlayer client, string name)
        {
            if (null == client.RoleParamsDict) return null;

            lock (client.RoleParamsDict)
            {
                RoleParamsData roleParamsData = null;
                if (client.RoleParamsDict.TryGetValue(name, out roleParamsData))
                {
                    return roleParamsData.ParamValue;
                }
            }

            return null;
        }

        /// <summary>
        /// 角色参数增加一个值,这个参数值必须是整数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        /// <summary>
        /// 更新角色参数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>

        /// <summary>
        /// 更新离线角色参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void UpdateRoleParamByNameOffline(int roleId, string name, string value, int serverId)
        {
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEROLEPARAM, string.Format("{0}:{1}:{2}", roleId, name, value), null, serverId);
        }

        #endregion 角色参数管理(注意太频繁的更新，需要外部控制是否实时提交)

        #region 通用字符串参数列表管理

        /// <summary>
        /// 主要用于管理类似 A|zz;B|dd;E|mm 这类对map进行映射的字符串 split1 : ; split2: |
        /// </summary>
        /// <param name="warReqString"></param>
        /// <returns></returns>
        public static Dictionary<String, String> StringToMap(String strValue, char split1 = ';', char split2 = '|')
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();

            if (null == strValue || strValue.Length <= 0)
            {
                return dict;
            }

            String[] reqItems = strValue.Split(split1);
            String[] item = null;
            for (int n = 0; n < reqItems.Length; n++)
            {
                item = reqItems[n].Split(split2);

                if (item.Length != 2) continue;

                dict.Add(item[0], item[1]);
            }

            return dict;
        }

        /// <summary>
        /// 主要用于管理类似 A|zz;B|dd;E|mm 这类对map进行映射的字符串 split1 : ; split2: |
        /// </summary>
        /// <returns></returns>
        public static String MapToString(Dictionary<String, String> dict, char split1 = ';', char split2 = '|')
        {
            String strValue = "";

            for (int n = 0; n < dict.Count; n++)
            {
                if (strValue.Length > 0)
                {
                    strValue += split1;
                }
                strValue += String.Format("{0}{1}{2}", dict.ElementAt(n).Key, split2, dict.ElementAt(n).Value);
            }

            return strValue;
        }

        /// <summary>
        /// 主要用于管理类似 A;B;E 这类对List进行映射的字符串 split1 : ;
        /// </summary>
        /// <param name="warReqString"></param>
        /// <returns></returns>
        public static List<String> StringToList(String strValue, char split1 = ';')
        {
            List<String> ls = new List<String>();

            if (null == strValue || strValue.Length <= 0)
            {
                return ls;
            }

            String[] reqItems = strValue.Split(split1);
            for (int n = 0; n < reqItems.Length; n++)
            {
                ls.Add(reqItems[n]);
            }

            return ls;
        }

        /// <summary>
        /// 主要用于管理类似 A;B;E 这类对map进行映射的字符串 split1 : ;
        /// </summary>
        /// <returns></returns>
        public static String ListToString(List<String> ls, char split1 = ';')
        {
            String strValue = "";

            for (int n = 0; n < ls.Count; n++)
            {
                if (strValue.Length > 0)
                {
                    strValue += split1;
                }
                strValue += ls.ElementAt(n);
            }

            return strValue;
        }

        public static string ListToString<T>(List<T> ls, char split1 = '$')
        {
            if (null == ls)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (T v in ls)
            {
                sb.Append(v);
                sb.Append(split1);
            }
            return sb.ToString().TrimEnd(split1);
        }

        public static List<int> StringToIntList(string str, char split1 = '$')
        {
            List<int> ls = new List<int>();
            if (null != str && str.Length > 0)
            {
                string[] arr = str.Split(split1);
                foreach (var s in arr)
                {
                    ls.Add(Global.SafeConvertToInt32(s));
                }
            }
            return ls;
        }

        #endregion 通用字符串参数列表管理

        #region 坐标变换 格子 坐标 和 像素坐标的互换

        /// <summary>
        /// 格子坐标转换为像素坐标
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Point GridToPixel(int mapCode, double gridX, double gridY)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                //return new Point(gridX * 64 + 32, gridY * 32 + 16);
                return new Point(0, 0);
            }

            Point pixel = new Point(gridX * gameMap.MapGridWidth + gameMap.MapGridWidth / 2,
                gridY * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);

            return pixel;
        }

        /// <summary>
        /// 格子坐标转换为像素坐标
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Point GridToPixel(int mapCode, Point grid)
        {
            return GridToPixel(mapCode, grid.X, grid.Y);
        }

        /// <summary>
        /// 像素坐标转换为格子坐标
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static Point PixelToGrid(int mapCode, double pixelX, double pixelY)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                return new Point(pixelX / 64, pixelY / 32);
            }

            Point grid = new Point(pixelX / gameMap.MapGridWidth, pixelY / gameMap.MapGridHeight);

            return grid;
        }

        /// <summary>
        /// 像素坐标转换为格子坐标
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static Point PixelToGrid(int mapCode, Point pixel)
        {
            return PixelToGrid(mapCode, pixel.X, pixel.Y);
        }

        #endregion 坐标变换 格子 坐标 和 像素坐标的互换

        #region 玩家参数存储相关

        public static KPlayer MakeGameClientForGetRoleParams(RoleDataEx roleDataEx)
        {
            KPlayer clientData = new KPlayer()
            {
                RoleData = roleDataEx,
            };

            return clientData;
        }

        public static int GetRoleParamsInt32FromDB(KPlayer client, String roleParamsKey, int defaultValue)
        {
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return defaultValue;
            }

            return Global.SafeConvertToInt32(valueString);
        }

        public static int GetRoleParamsInt32FromDB(KPlayer client, String roleParamsKey)
        {
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return 0;
            }

            return Global.SafeConvertToInt32(valueString);
        }

        // 增加一个接口 [7/31/2013 LiaoWei]
        /// <summary>
        /// 以 整数形式 返回角色的参数数据 没有配置则返回 0
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static long GetRoleParamsInt64FromDB(KPlayer client, String roleParamsKey)
        {
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return 0;
            }

            return Global.SafeConvertToInt64(valueString);
        }

        /// <summary>
        /// 返回角色的参数数据 没有配置则返回 0
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static DateTime GetRoleParamsDateTimeFromDB(KPlayer client, String roleParamsKey)
        {
            long ticks = 0;
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (!String.IsNullOrEmpty(valueString))
            {
                ticks = Global.SafeConvertToInt64(valueString);
                if (ticks == 0 && valueString != "0")
                {
                    //为兼容转换前后的数据，以前有数据的为1，没数据的为0，在新项目中不需要如此
                    ticks = 1;
                }
            }

            return new DateTime(ticks);
        }

        /// <summary>
        /// 以 字符串形式，可能带有空字符 返回角色的参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static String GetRoleParamsStringWithNullFromDB(KPlayer client, String roleParamsKey)
        {
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return valueString;
            }

            byte[] bytes = Convert.FromBase64String(valueString);

            valueString = Encoding.GetEncoding("latin1").GetString(bytes);

            return valueString;
        }

        public static String GetRoleParamsStringWithDB(KPlayer client, String roleParamsKey)
        {
            String valueString = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return valueString;
            }

            //byte[] bytes = Convert.FromBase64String(valueString);

            //valueString = Encoding.GetEncoding("latin1").GetString(bytes);

            return valueString;
        }

        /// <summary>
        /// 以 ulong 列表 返回角色的参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<ulong> GetRoleParamsUlongListFromDB(KPlayer client, String roleParamsKey)
        {
            List<ulong> lsValues = new List<ulong>();

            String valueString = GetRoleParamsStringWithNullFromDB(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return lsValues;
            }

            int pos = 0;
            int usedLenght = 0;

            //依次生成各个64位整数
            while (usedLenght < valueString.Length)
            {
                byte[] bytes_8 = Encoding.GetEncoding("latin1").GetBytes(valueString.Substring(pos, 8));
                lsValues.Add(BitConverter.ToUInt64(bytes_8, 0));

                pos += 8;
                usedLenght += 8;
            }

            return lsValues;
        }

        /// <summary>
        /// 以 int 列表 返回角色的参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<int> GetRoleParamsIntListFromDB(KPlayer client, String roleParamsKey)
        {
            List<int> lsValues = new List<int>();

            String valueString = GetRoleParamsStringWithNullFromDB(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return lsValues;
            }

            int pos = 0;
            int usedLenght = 0;

            //依次生成各个32位整数
            while (usedLenght < valueString.Length)
            {
                byte[] bytes_4 = Encoding.GetEncoding("latin1").GetBytes(valueString.Substring(pos, 4));
                lsValues.Add(BitConverter.ToInt32(bytes_4, 0));

                pos += 4;
                usedLenght += 4;
            }

            return lsValues;
        }

        /// <summary>
        /// 以 uint 列表 返回角色的参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<uint> GetRoleParamsUIntListFromDB(KPlayer client, String roleParamsKey)
        {
            List<uint> lsValues = new List<uint>();

            String valueString = GetRoleParamsStringWithNullFromDB(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return lsValues;
            }

            int pos = 0;
            int usedLenght = 0;

            //依次生成各个32位整数
            while (usedLenght < valueString.Length)
            {
                byte[] bytes_4 = Encoding.GetEncoding("latin1").GetBytes(valueString.Substring(pos, 4));
                lsValues.Add(BitConverter.ToUInt32(bytes_4, 0));

                pos += 4;
                usedLenght += 4;
            }

            return lsValues;
        }

        /// <summary>
        /// 以 ushort 列表 返回角色的参数数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<ushort> GetRoleParamsUshortListFromDB(KPlayer client, String roleParamsKey)
        {
            List<ushort> lsValues = new List<ushort>();

            String valueString = GetRoleParamsStringWithNullFromDB(client, roleParamsKey);

            if (String.IsNullOrEmpty(valueString))
            {
                return lsValues;
            }

            int pos = 0;
            int usedLenght = 0;

            //依次生成各个16位整数
            while (usedLenght < valueString.Length)
            {
                byte[] bytes_2 = Encoding.GetEncoding("latin1").GetBytes(valueString.Substring(pos, 2));
                lsValues.Add(BitConverter.ToUInt16(bytes_2, 0));

                pos += 2;
                usedLenght += 2;
            }

            return lsValues;
        }

        /// <summary>
        /// 角色整数参数数据库参考时间,这个时间一般取必当前时间大好多年的参数
        /// </summary>
        public const String RoleInt32ParamsDBReferenceTime = "2020-12-12 12:12:12";

        /// <summary>
        /// 以 特殊字符串形式将整数数据存储到数据库角色参数部分，整数将附加上时间戳【时间挫参考某个时间点】
        /// 这样便于对该字段进行排序读取,refTime 如果是比当前时间点大得多的时间，则数据库查询时从小到大或者从大道小都好排序
        /// 如果是比当前时间小，或者用1970年的标准时间戳，那样数据库比较不好排序
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static void SaveRoleParamsInt32ValueWithTimeStampToDB(KPlayer client, String roleParamsKey, Int32 nValue, bool writeToDB = false, String refTime = RoleInt32ParamsDBReferenceTime)
        {
            DateTime referenceTime = DateTime.Parse(refTime);
            TimeSpan ts = TimeUtil.NowDateTime() - referenceTime;

            //String sValue = String.Format("{0:0000000000}_{1}", nValue, ts.TotalSeconds);
            GameDb.UpdateRoleParamByName(client, roleParamsKey, nValue.ToString(), writeToDB);
        }

        /// <summary>
        /// 返回以 特殊字符串形式将整数数据存储到数据库角色参数部分的非时间戳部分的整数，整数将附加上时间戳【时间挫参考某个时间点】
        /// 这样便于对该字段进行排序读取,refTime 如果是比当前时间点大得多的时间，则数据库查询时从小到大或者从大道小都好排序
        /// 如果是比当前时间小，或者用1970年的标准时间戳，那样数据库比较不好排序
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static Int32 GetRoleParamsInt32ValueWithTimeStampFromDB(KPlayer client, String roleParamsKey, String refTime = RoleInt32ParamsDBReferenceTime)
        {
            String sValue = Global.GetRoleParamByName(client, roleParamsKey);

            if (String.IsNullOrEmpty(sValue))
            {
                return 0;
            }

            String[] sArr = sValue.Split('_');

            return Global.SafeConvertToInt32(sArr[0]);
        }

        /// <summary>
        /// 以 字符串形式 将整数数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        /// <param name="writeToDB">变化是否频繁且不重要的数据，传false</param>
        public static void SaveRoleParamsInt32ValueToDB(KPlayer client, String roleParamsKey, Int32 nValue, bool writeToDB)
        {
            GameDb.UpdateRoleParamByName(client, roleParamsKey, nValue.ToString(), writeToDB);
        }

        // 增加一个接口 [7/31/2013 LiaoWei]
        /// <summary>
        /// 以 字符串形式 将整数数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static void SaveRoleParamsInt64ValueToDB(KPlayer client, String roleParamsKey, Int64 nValue, bool writeToDB)
        {
            GameDb.UpdateRoleParamByName(client, roleParamsKey, nValue.ToString(), writeToDB);
        }

        /// <summary>
        /// 以 字符串形式 [不能带有空字符，属于正常字符串] 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static void SaveRoleParamsStringToDB(KPlayer client, String roleParamsKey, String valueString, bool writeToDB)
        {
            GameDb.UpdateRoleParamByName(client, roleParamsKey, valueString, writeToDB);
        }

        /// <summary>
        /// 以 字符串形式 [可能带有空字符] 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static void SaveRoleParamsStringWithNullToDB(KPlayer client, String roleParamsKey, String valueString, bool writeToDB)
        {
            byte[] bytes = Encoding.GetEncoding("latin1").GetBytes(valueString);

            GameDb.UpdateRoleParamByName(client, roleParamsKey, Convert.ToBase64String(bytes), writeToDB);
        }

        /// <summary>
        /// 以 整形形式 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuString"></param>
        public static void SaveRoleParamsDateTimeToDB(KPlayer client, String roleParamsKey, DateTime dateTime, bool writeToDB)
        {
            GameDb.UpdateRoleParamByName(client, roleParamsKey, dateTime.Ticks.ToString(), writeToDB);
        }

        /// <summary>
        /// 以 ushort 列表 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="lsUint"></param>
        /// <param name="filed"></param>
        public static void SaveRoleParamsUshortListToDB(KPlayer client, List<ushort> lsUshort, String roleParamsKey, bool writeToDB = false)
        {
            //生成新数据字符串
            String newStringValue = "";

            for (int n = 0; n < lsUshort.Count; n++)
            {
                byte[] bytes = BitConverter.GetBytes(lsUshort[n]);
                newStringValue += Encoding.GetEncoding("latin1").GetString(bytes);
            }

            Global.SaveRoleParamsStringWithNullToDB(client, roleParamsKey, newStringValue, writeToDB);
        }

        /// <summary>
        /// 以 int 列表 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="lsUint"></param>
        /// <param name="filed"></param>
        public static void SaveRoleParamsIntListToDB(KPlayer client, List<int> lsInt, String roleParamsKey, bool writeToDB = false)
        {
            //生成新数据字符串
            String newStringValue = "";

            for (int n = 0; n < lsInt.Count; n++)
            {
                byte[] bytes = BitConverter.GetBytes(lsInt[n]);
                newStringValue += Encoding.GetEncoding("latin1").GetString(bytes);
            }

            Global.SaveRoleParamsStringWithNullToDB(client, roleParamsKey, newStringValue, writeToDB);
        }

        /// <summary>
        /// 以 uint 列表 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="lsUint"></param>
        /// <param name="filed"></param>
        public static void SaveRoleParamsUintListToDB(KPlayer client, List<uint> lsUint, String roleParamsKey, bool writeToDB = false)
        {
            //生成新数据字符串
            String newStringValue = "";

            for (int n = 0; n < lsUint.Count; n++)
            {
                byte[] bytes = BitConverter.GetBytes(lsUint[n]);
                newStringValue += Encoding.GetEncoding("latin1").GetString(bytes);
            }

            Global.SaveRoleParamsStringWithNullToDB(client, roleParamsKey, newStringValue, writeToDB);
        }

        /// <summary>
        /// 以 ulong 列表 将数据存储到数据库角色参数部分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="lsUint"></param>
        /// <param name="filed"></param>
        public static void SaveRoleParamsUlongListToDB(KPlayer client, List<ulong> lsUlong, String roleParamsKey, bool writeToDB = false)
        {
            //生成新数据字符串
            String newStringValue = "";

            for (int n = 0; n < lsUlong.Count; n++)
            {
                byte[] bytes = BitConverter.GetBytes(lsUlong[n]);
                newStringValue += Encoding.GetEncoding("latin1").GetString(bytes);
            }

            Global.SaveRoleParamsStringWithNullToDB(client, roleParamsKey, newStringValue, writeToDB);
        }

        #endregion 玩家参数存储相关

        #region 地图定位相关

        /// <summary>
        /// 返回地图定位数据 indexRecord 从1开始
        /// </summary>
        /// <returns></returns>
        public static bool GetMapRecordDataByField(KPlayer client, int indexRecord, out int mapCode, out int x, out int y)
        {
            mapCode = 0;
            x = 0;
            y = 0;

            List<ushort> lsUshort = Global.GetRoleParamsUshortListFromDB(client, RoleParamName.MapPosRecord);

            int realIndex = indexRecord * 3;

            if ((realIndex + 2) >= lsUshort.Count)//每个记录三条数据
            {
                return false;
            }

            mapCode = lsUshort[realIndex++];
            x = lsUshort[realIndex++];
            y = lsUshort[realIndex];

            //都是0 ，这个坐标是错误的
            if (0 == mapCode && 0 == x && 0 == y)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改地图定位数据 只能是2字节存放  indexRecord 从1开始
        /// </summary>
        /// <returns></returns>
        public static void ModifyMapRecordData(KPlayer client, ushort mapCode, ushort x, ushort y, int indexRecord)
        {
            List<ushort> lsUshort = Global.GetRoleParamsUshortListFromDB(client, RoleParamName.MapPosRecord);

            int realIndex = indexRecord * 3;

            while (lsUshort.Count < (realIndex + 3))
            {
                lsUshort.Add(0);
                lsUshort.Add(0);
                lsUshort.Add(0);
            }

            lsUshort[realIndex++] = mapCode;
            lsUshort[realIndex++] = x;
            lsUshort[realIndex] = y;

            Global.SaveRoleParamsUshortListToDB(client, lsUshort, RoleParamName.MapPosRecord, true);
        }

        /// <summary>
        /// 地图编号和场景类型映射字典
        /// </summary>
        private static Dictionary<int, SceneUIClasses> MapCodeSceneTypeDict = new Dictionary<int, SceneUIClasses>();

        /// <summary>
        /// 获得地图的场景类型
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static SceneUIClasses GetMapSceneType(int mapCode)
        {
            SceneUIClasses sceneType;
            if (!MapCodeSceneTypeDict.TryGetValue(mapCode, out sceneType))
            {
                return SceneUIClasses.Normal;
            }

            return sceneType;
        }

        #endregion 地图定位相关

        #region RELOG CONVERT RATE

        /// <summary>
        /// Lấy RATE QUY ĐÔI TỪ KNB RA KNB
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static int TransMoneyToYuanBao(int money)
        {
            int moneyToYuanBao = GameManager.GameConfigMgr.GetGameConfigItemInt("money-to-yuanbao", 10);

            int yuanBao = money * moneyToYuanBao;

            return yuanBao;
        }

        #endregion RELOG CONVERT RATE

        #region 开服时间相关

        /// <summary>
        /// 返回服务器开服时间，其实是开区时间,这儿不用缓存，参数那已经做缓存处理了
        /// </summary>
        /// <returns></returns>
        public static DateTime GetKaiFuTime()
        {
            String sTimeString = GameManager.GameConfigMgr.GetGameConfigItemStr("kaifutime", "2013-02-12 01:01:01");
            DateTime dateTime;
            DateTime.TryParse(sTimeString, out dateTime);
            return dateTime;
        }

        /// <summary>
        /// 获取一个增加了几天时间的DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="addDays"></param>
        /// <returns></returns>
        public static DateTime GetAddDaysDataTime(DateTime dateTime, int addDays, bool roundDay = true)
        {
            if (roundDay)
            {
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            }

            return new DateTime(dateTime.Ticks + ((long)addDays * 10000 * 1000 * 24 * 60 * 60));
        }

        /// <summary>
        /// 计算 left - right 的天数差，如果都是同一天，则为0，否则返回天数差值
        /// 封装成函数主要为了解决跨年，否则dayOfYear 一减就OK
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int GetDaysSpanNum(DateTime left, DateTime right, bool roundDay = true)
        {
            if (roundDay)
            {
                DateTime left1 = new DateTime(left.Year, left.Month, left.Day);
                DateTime right1 = new DateTime(right.Year, right.Month, right.Day);
                return (int)(((left1.Ticks / (10000 * 1000)) - (right1.Ticks / (10000 * 1000))) / (24 * 60 * 60));
            }
            else
            {
                return (int)(((left.Ticks / (10000 * 1000)) - (right.Ticks / (10000 * 1000))) / (24 * 60 * 60));
            }
        }

        /// <summary>
        /// 活动的时间
        /// </summary>
        /// <returns></returns>
        public static string GetHuoDongTimeByKaiFu(int addDays, int hours, int minutes, int seconds)
        {
            DateTime dateTime = Global.GetAddDaysDataTime(Global.GetKaiFuTime(), addDays, true);
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds);
            return newDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion 开服时间相关

        #region 角色创建日期

        /// <summary>
        /// 返回角色注册时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRegTime(KPlayer clientData)
        {
            DateTime dateTime = new DateTime(clientData.RegTime * 10000);
            return dateTime;
        }

        #endregion 角色创建日期

        #region 节日活动时间相关

        /// <summary>
        /// 返回服务器节日活动开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetJieriStartDay()
        {
            String sTimeString = GameManager.GameConfigMgr.GetGameConfigItemStr("jieristartday", "2000-01-01 01:01:01");
            DateTime dateTime;
            DateTime.TryParse(sTimeString, out dateTime);
            return dateTime;
        }

        /// <summary>
        /// 返回服务器节日活动持续的天数
        /// </summary>
        /// <returns></returns>
        public static int GetJieriDaysNum()
        {
            int daysNum = GameManager.GameConfigMgr.GetGameConfigItemInt("jieridaysnum", 0);
            return daysNum;
        }

        #endregion 节日活动时间相关

        /// <summary>
        /// 返回服务器时间相对于"2011-11-11"经过了多少天
        /// 可以避免使用DayOfYear产生的跨年问题
        /// </summary>
        /// <returns></returns>
        public static int GetOffsetHour(DateTime now)
        {
            TimeSpan ts = now - DateTime.Parse("2011-11-11");
            // 经过的毫秒数
            double temp = ts.TotalMilliseconds;
            int day = (int)(temp / 1000 / 60 / 60);
            return day;
        }

        /// <summary>
        /// 返回服务器时间相对于"2011-11-11"经过了多少天
        /// 可以避免使用DayOfYear产生的跨年问题
        /// </summary>
        /// <returns></returns>
        public static double GetOffsetSecond(DateTime date)
        {
            TimeSpan ts = date - DateTime.Parse("2011-11-11");
            // 经过的毫秒数
            double temp = ts.TotalMilliseconds;
            return temp / 1000;
        }

        /// <summary>
        /// 返回服务器时间相对于"2011-11-11"经过了多少天
        /// 可以避免使用DayOfYear产生的跨年问题
        /// </summary>
        /// <returns></returns>
        public static int GetOffsetDay(DateTime now)
        {
            TimeSpan ts = now - DateTime.Parse("2011-11-11");
            // 经过的毫秒数
            double temp = ts.TotalMilliseconds;
            int day = (int)(temp / 1000 / 60 / 60 / 24);
            return day;
        }

        /// <summary>
        /// 当前时间相对于"2011-11-11"经过了多少天
        /// </summary>
        /// <returns></returns>
        public static int GetOffsetDayNow()
        {
            return GetOffsetDay(TimeUtil.NowDateTime());
        }

        /// <summary>
        /// 使用服务器时间相对于"2011-11-11"经过了多少天 来返回具体的日期
        /// 可以避免使用DayOfYear产生的跨年问题
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRealDate(int day)
        {
            DateTime startDay = DateTime.Parse("2011-11-11");
            return Global.GetAddDaysDataTime(startDay, day);
        }

        /// <summary>
        /// 获得某个日期的周一相对天数
        /// </summary>
        public static int BeginOfWeek(DateTime date)
        {
            int dayofweek = (int)date.DayOfWeek;

            if (dayofweek == 0)
                dayofweek = 7;

            dayofweek--;

            int currday = GetOffsetDay(date);
            return currday - dayofweek;
        }

        #region 合服的时间相关

        /// <summary>
        /// 返回服务器合服活动开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetHefuStartDay()
        {
            String sTimeString = GameManager.GameConfigMgr.GetGameConfigItemStr("hefutime", "2000-01-01 01:01:01");
            DateTime dateTime;
            DateTime.TryParse(sTimeString, out dateTime);
            return dateTime;
        }

        /// <summary>
        /// 活动的时间
        /// </summary>
        /// <returns></returns>
        public static string GetHuoDongTimeByHeFu(int addDays, int hours, int minutes, int seconds)
        {
            DateTime dateTime = Global.GetAddDaysDataTime(Global.GetHefuStartDay(), addDays, true);
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds);
            return newDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion 合服的时间相关

        #region 补偿的时间相关

        /// <summary>
        /// 返回服务器合服补偿开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetBuChangStartDay()
        {
            String sTimeString = GameManager.GameConfigMgr.GetGameConfigItemStr("buchangtime", "2000-01-01 01:01:01");
            DateTime dateTime;
            DateTime.TryParse(sTimeString, out dateTime);
            return dateTime;
        }

        /// <summary>
        /// 补偿的时间
        /// </summary>
        /// <returns></returns>
        public static string GetTimeByBuChang(int addDays, int hours, int minutes, int seconds)
        {
            DateTime dateTime = Global.GetAddDaysDataTime(Global.GetBuChangStartDay(), addDays, true);
            DateTime newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds);
            return newDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion 补偿的时间相关

        #region 专属活动管理

        // 服务器当前专属配置文件的版本号，服务器每次重启置为1，每次热更新配置后加1
        private static int CachingSpecActXmlVersion = 1;

        #endregion 专属活动管理

        #region 大型节日活动管理

        // 服务器当前节日活动配置文件的版本号，服务器每次重启置为1，每次热更新配置后加1
        private static int CachingJieriXmlVersion = 1;

        #endregion 大型节日活动管理

        #region 龙权礼品码相关

        public static String GetGiftExchangeFileName()
        {
            string placeholder = string.Empty;
            return GetGiftExchangeFileName(out placeholder);
        }

        //得到Activities文件名
        public static String GetGiftExchangeFileName(out string sectionKey)
        {
            sectionKey = "dl_app";

            String strPlat = GameManager.GameConfigMgr.GetGameConfigItemStr("platformtype", "app");
            strPlat = strPlat.ToLower();

            if (strPlat == "app")
            {
                sectionKey = "dl_app";
            }
            else if (strPlat == "yueyu")
            {
                sectionKey = "dl_yueyu";
            }
            else if (strPlat == "andrid" || strPlat == "android" || strPlat == "yyb")
            {
                sectionKey = "dl_android";
            }

            const string fileName = "Config/Gifts/MU_Activities.xml";
            return fileName;
        }

        #endregion 龙权礼品码相关

        #region 位操作相关

        /// <summary>
        /// 根据输入数值获取位的设置值
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int GetBitValue(int whichOne)
        {
            int bitVal = 0;

            bitVal = (int)Math.Pow(2, whichOne - 1);
            return bitVal;
        }

        public static int GetBitValue(List<int> values, int whichOne)
        {
            int index = whichOne / 32;
            int bitIndex = whichOne % 32;
            if (values.Count <= index)
            {
                return 0;
            }
            int value = values[index];
            if ((value & (1 << bitIndex)) != 0)
            {
                return 1;
            }
            return 0;
        }

        public static void SetBitValue(ref List<int> values, int whichOne, int toValue)
        {
            int index = whichOne / 32;
            int bitIndex = whichOne % 32;
            while (values.Count <= index)
            {
                values.Add(0);
            }

            int value = values[index];
            if (toValue == 0)
            {
                value &= ~(1 << bitIndex);
            }
            else
            {
                value |= (1 << bitIndex);
            }
            values[index] = value;
        }

        #endregion 位操作相关

        #region 转职功能 相关接口

        public static int CalcOriginalOccupationID(KPlayer client)
        {
            return client.m_cPlayerFaction.GetFactionId();
        }

        #endregion 转职功能 相关接口

        /// <summary>
        /// Update seri login
        /// </summary>
        /// <param name="client"></param>
        public static bool UpdateSeriesLoginInfo(KPlayer client)
        {
            client.MyHuodongData.EveryDayOnLineAwardStep = 0;

            client.MyHuodongData.EveryDayOnLineAwardGoodsID = "";


            client.DayOnlineSecond = 1;
            client.BakDayOnlineSecond = client.DayOnlineSecond;
            client.DayOnlineRecSecond = TimeUtil.NOW();

            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            return true;
        }

        #region Lưu lại tích lũy tiêu

        public static void SaveConsumeLog(KPlayer client, int money, int type)
        {
            try
            {
                string dbCmds = client.RoleID + ":" + money + ":" + type;
                string[] dbFields = null;
                Global.RequestToDBServer(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_DB_SAVECONSUMELOG, dbCmds, out dbFields, client.ServerId);
            }
            catch (Exception e)
            {
                LogManager.WriteException(e.ToString());
            }
        }

        #endregion Lưu lại tích lũy tiêu

        /// <summary>
        /// Lấy ra địa chỉ IP hiện tại
        /// </summary>
        public static string GetLocalAddressIPs()
        {
            string addressIP = "";

            try
            {
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        if (addressIP == "")
                        {
                            addressIP = _IPAddress.ToString();
                        }
                        else
                        {
                            addressIP += "_" + _IPAddress.ToString();
                        }
                    }
                }
            }
            catch
            {
            }
            return addressIP;
        }

        #region Quản lý giao dịch

        /// <summary>
        /// Tăng số alanf giao dịch trong ngày
        /// </summary>
        public static int IncreaseTradeCount(KPlayer client, string strDayID, string strCount, int addCount = 1)
        {
            int currDayID = Global.GetOffsetDayNow();
            int dayID = Global.GetRoleParamsInt32FromDB(client, strDayID);
            int currCount = Global.GetRoleParamsInt32FromDB(client, strCount);

            if (dayID != currDayID)
            {
                currCount = 1;
            }
            else
            {
                currCount += addCount;
            }

            Global.SaveRoleParamsStringToDB(client, strDayID, currDayID.ToString(), true);
            Global.SaveRoleParamsStringToDB(client, strCount, currCount.ToString(), true);
            return currCount;
        }

        #endregion Quản lý giao dịch

        /// <summary>
        /// 取得一个json串
        /// </summary>
        public static string GetJson(Dictionary<string, string> jsonDict)
        {
            if (null == jsonDict || jsonDict.Count <= 0)
            {
                return "";
            }

            string strResult = "{";
            bool bFrist = true;
            foreach (var item in jsonDict)
            {
                if (!bFrist)
                {
                    strResult += ",";
                }

                strResult += string.Format("\"{0}\":\"{1}\"", item.Key, item.Value);

                bFrist = false;
            }
            strResult += "}";

            return strResult;
        }

        public static string doPost(string url, string body, int timeout = 0/*超时时间*/)
        {
            string encodingName = "utf-8";
            string str = "";
            if ((url == null) || (url == ""))
            {
                return null;
            }
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (timeout > 0)
            {
                request.Timeout = timeout;
            }

            byte[] bytes = null;
            if (body == null)
            {
                request.ContentLength = 0L;
            }
            else
            {
                try
                {
                    bytes = Encoding.Default.GetBytes(body);
                    request.ContentLength = bytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                catch (Exception e)
                {
                    DataHelper.WriteFormatExceptionLog(e, "doPost", false);
                    return null;
                }
            }

            try
            {
                Stream responseStream = request.GetResponse().GetResponseStream();
                byte[] buffer = new byte[0x200];
                for (int j = responseStream.Read(buffer, 0, 0x200); j > 0; j = responseStream.Read(buffer, 0, 0x200))
                {
                    Encoding encoding = Encoding.GetEncoding(encodingName);
                    str = str + encoding.GetString(buffer, 0, j);
                }
                return str;
            }
            catch (Exception exception2)
            {
                DataHelper.WriteFormatExceptionLog(exception2, "doPost", false);
                return null;
            }

            return str;
        }

        #region 跨服相关

        /// <summary>
        /// 获取跨服登录信息
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static KuaFuServerLoginData GetClientKuaFuServerLoginData(KPlayer client)
        {
            TMSKSocket clientSocket = client.ClientSocket;
            if (null != clientSocket)
            {
                if (null != clientSocket.ClientKuaFuServerLoginData)
                {
                    return clientSocket.ClientKuaFuServerLoginData;
                }
            }

            return new KuaFuServerLoginData();
        }

        public static bool CanEnterMap(KPlayer client, int toMapCode)
        {
            GameMap toGameMap;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out toGameMap))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("ProcessSpriteGoToMapCmd mapCode Error, mapCode={0}", toMapCode));
                return false;
            }

            return true;
        }

        #endregion 跨服相关

        #region 玩家的登陆记录

        // 记录玩家x天内登陆的记录
        private static int RoleLoginRecordDayCount = 50;

        /// <summary>
        /// Ghi lại nhật ký loign
        /// </summary>
        public static void UpdateRoleLoginRecord(KPlayer client)
        {
            int currDayID = Global.GetOffsetDayNow();

            string strParam = Global.GetRoleParamByName(client, RoleParamName.RoleLoginRecorde);

            int recordDayID = 0;
            string strRecord = "";

            string[] strFields = null == strParam ? null : strParam.Split(',');
            if (null != strFields && strFields.Length == 2)
            {
                recordDayID = Convert.ToInt32(strFields[0]);
                strRecord = strFields[1];
            }

            if (recordDayID == currDayID)
            {
                return;
            }

            if (recordDayID > 0)
            {
                for (int i = recordDayID + 1; i < currDayID; i++)
                {
                    strRecord = "0" + strRecord;
                }
            }

            strRecord = "1" + strRecord;

            if (strRecord.Length > RoleLoginRecordDayCount)
            {
                strRecord = strRecord.Substring(0, RoleLoginRecordDayCount);
            }

            string result = string.Format("{0},{1}", currDayID, strRecord);

            Global.SaveRoleParamsStringToDB(client, RoleParamName.RoleLoginRecorde, result, true);
        }

        /// <summary>
        /// 检查玩家在某段时间内是否登陆
        /// 如果输入的时间超过记录范围，默认没登陆过
        /// </summary>
        public static bool CheckRoleIsLoginByTime(KPlayer client, DateTime beginTime, DateTime endTime)
        {
            int beginDayID = Global.GetOffsetDay(beginTime);
            int endDayID = Global.GetOffsetDay(endTime);
            // 开服日期
            int startDayID = Global.GetOffsetDay(Global.GetKaiFuTime());

            string strParam = Global.GetRoleParamByName(client, RoleParamName.RoleLoginRecorde);

            int recordDayID = 0;
            string strRecord = "";

            string[] strFields = null == strParam ? null : strParam.Split(',');
            if (null != strFields && strFields.Length == 2)
            {
                recordDayID = Convert.ToInt32(strFields[0]);
                strRecord = strFields[1];
            }

            int recordDayCount = strRecord.Length;

            if (recordDayCount <= 0)
                return false;

            for (int i = beginDayID; i <= endDayID && i <= recordDayID; ++i)
            {
                // 如果要判断的这天 在开服日期之前，就认为他登陆过了
                if (i < startDayID)
                {
                    return true;
                }

                // 这天的索引
                int index = recordDayID - i;
                if (index >= recordDayCount)
                {
                    continue;
                }

                if ('1' == strRecord[index])
                {
                    return true;
                }
            }

            return false;
        }

        #endregion 玩家的登陆记录

        public static string DefaultPZ = "1";
        public static bool GAME_CONFIG_MMO = true;
    }

#if VERIFY

    public class PZWJ
    {
        public PZWJ()
        {
            NetworkAdapterInformation[] aa = NetworkAdapter.GetNetworkAdapterInformation();
            Global.DefaultPZ = MD5Helper.get_md5_string(SHA2Helper.GethostName() + SHA2Helper.GetCPUSerialNumber() + aa[0].PermanentAddress);
        }
    }

#endif
}