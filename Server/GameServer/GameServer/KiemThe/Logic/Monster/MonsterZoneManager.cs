using GameServer.KiemThe;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Entities;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Threading;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Windows.Forms;

namespace GameServer.Logic
{
    /// <summary>
    /// Danh sách chờ thêm quái
    /// </summary>
    public class MonsterZoneQueueItem
    {
        /// <summary>
        /// ID phụ bản
        /// </summary>
        public int CopyMapID { get; set; } = 0;

        /// <summary>
        /// Tổng số sinh ra
        /// </summary>
        public int BirthCount { get; set; } = 0;

        /// <summary>
        /// Đối tượng khu vực quản lý
        /// </summary>
        public MonsterZone MyMonsterZone { get; set; } = null;

        /// <summary>
        /// Gốc quái vật để sinh ra quái vật ở vùng này
        /// </summary>
        public Monster SeedMonster { get; set; } = null;

        /// <summary>
        /// Tọa độ X
        /// </summary>
        public int ToX { get; set; } = 0;
        /// <summary>
        /// Tọa độ Y
        /// </summary>
        public int ToY { get; set; } = 0;

        /// <summary>
        /// Tên quái
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Danh hiệu quái
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Cấp độ
        /// <para>-1 sẽ lấy ở file cấu hình</para>
        /// </summary>
        public int Level { get; set; } = -1;

        /// <summary>
        /// Sinh lực
        /// <para>-1 sẽ lấy ở file cấu hình</para>
        /// </summary>
        public int HP { get; set; } = -1;

        /// <summary>
        /// Hướng quay quái
        /// </summary>
        public KiemThe.Entities.Direction Dir { get; set; } = KiemThe.Entities.Direction.DOWN;

        /// <summary>
        /// Ngũ hành quái
        /// </summary>
        public KE_SERIES_TYPE Series { get; set; } = KE_SERIES_TYPE.series_metal;

        /// <summary>
        /// ID Script Lua điều khiển
        /// </summary>
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        public int AIID { get; set; } = -1;

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện sau khi khởi tạo quái thành công
        /// </summary>
        public Action<Monster> Done { get; set; }

        /// <summary>
        /// Sự kiện khi quái chết
        /// </summary>
        public Action<GameObject> OnDieCallback { get; set; }

        /// <summary>
        /// Thời điểm chết
        /// </summary>
        public long DeadTick { get; set; }

        /// <summary>
        /// Thời điểm sống lại
        /// </summary>
        public long RespawnTick { get; set; } = -1;

        /// <summary>
        /// Điều kiện tái sinh quái
        /// </summary>
        public Func<bool> RespawnPredicate { get; set; } = null;

        /// <summary>
        /// Camp nếu có
        /// </summary>
        public int Camp { get; set; } = -1;

        /// <summary>
        /// Loại quái
        /// </summary>
        public MonsterAIType MonsterType { get; set; } = MonsterAIType.Normal;
    };

    /// <summary>
    /// Đối tượng quản lý khu vực quái
    /// </summary>
    public class MonsterZoneManager
    {
        /// <summary>
        /// Đối tượng quản lý khu vực quái
        /// </summary>
        public MonsterZoneManager()
        {

        }

        #region Thuộc tính
        /// <summary>
        /// Danh sách khu vực quái tạm thời
        /// </summary>
        private Dictionary<int, MonsterZone> MonsterDynamicZoneDict = new Dictionary<int, MonsterZone>(100);

        /// <summary>
        /// Danh sách khu vực quái thường
        /// </summary>
        private List<MonsterZone> MonsterZoneList = new List<MonsterZone>(100);

        /// <summary>
        /// Danh sách khu vực quái theo ID bản đồ
        /// </summary>
        private Dictionary<int, List<MonsterZone>> Map2MonsterZoneDict = new Dictionary<int, List<MonsterZone>>(100);


        /// <summary>
        /// Hàng đợi danh sách khu vực quái đang đợi được thêm vào
        /// </summary>
        private ConcurrentQueue<MonsterZoneQueueItem> WaitingAddDynamicMonsterQueue = new ConcurrentQueue<MonsterZoneQueueItem>();

        /// <summary>
        /// Danh sách quái mẫu theo ID
        /// </summary>
        private ConcurrentDictionary<int, Monster> _DictDynamicMonsterSeed = new ConcurrentDictionary<int, Monster>();

        /// <summary>
        /// Mutex dùng trong khóa Lock
        /// </summary>
        private object _allMonsterXmlMutex = new object();

        private XElement _allMonstersXml = null;


        /// <summary>
        /// XMLNode chứa danh sách quái trong hệ thống
        /// </summary>
        public XElement AllMonstersXml
        {
            get
            {
                lock (_allMonsterXmlMutex)
                {
                    return _allMonstersXml;
                }
            }
            set
            {
                lock (_allMonsterXmlMutex)
                {
                    _allMonstersXml = value;
                }
            }
        }

        /// <summary>
        /// Tải xuống danh sách quái trong hệ thống
        /// </summary>
        public void LoadAllMonsterXml()
        {
            XElement tmpXml = null;
            try
            {
                tmpXml = XElement.Load(Global.GameResPath("Config/KT_Monster/Monsters.xml"));
            }
            catch (Exception ex)
            {
            }

            if (tmpXml != null)
            {
                this.AllMonstersXml = tmpXml;
            }
        }

        #endregion

        #region Khởi tạo

        /// <summary>
        /// Mutex dùng khóa Lock
        /// </summary>
        private object InitMonsterZoneMutex = new object();

        /// <summary>
        /// Khởi tạo khu vực bản đồ
        /// </summary>
        /// <param name="monsterZone"></param>
        private void AddMap2MonsterZoneDict(MonsterZone monsterZone)
        {
            List<MonsterZone> monsterZoneList = null;
            if (Map2MonsterZoneDict.TryGetValue(monsterZone.MapCode, out monsterZoneList))
            {
                monsterZoneList.Add(monsterZone);
                return;
            }

            monsterZoneList = new List<MonsterZone>();
            Map2MonsterZoneDict[monsterZone.MapCode] = monsterZoneList;
            monsterZoneList.Add(monsterZone);
        }

        /// <summary>
        /// Chuyển chuỗi sang điểm hồi sinh
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private List<BirthTimePoint> ParseBirthTimePoints(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            string[] fields = s.Split('|');
            if (fields.Length <= 0) return null;

            List<BirthTimePoint> list = new List<BirthTimePoint>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (string.IsNullOrEmpty(fields[i]))
                {
                    continue;
                }

                string[] fields2 = fields[i].Split(':');
                if (fields2.Length != 2) continue;

                string str1 = fields2[0].TrimStart('0');
                string str2 = fields2[1].TrimStart('0');
                BirthTimePoint birthTimePoint = new BirthTimePoint()
                {
                    BirthHour = Global.SafeConvertToInt32(str1),
                    BirthMinute = Global.SafeConvertToInt32(str2),
                };

                list.Add(birthTimePoint);
            }

            return list.Count > 0 ? list : null;
        }

        /// <summary>
        /// Tải xuống danh sách quái trong bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="mapName"></param>
        /// <param name="gameMap"></param>
        public void AddMapMonsters(int mapCode, string mapName, GameMap gameMap)
        {
            /// Thêm khu vực tương ứng
            this.AddDynamicMonsterZone(mapCode);

            string fileName = string.Format("MapConfig/{0}/Monsters.xml", mapName);
            XElement xml = null;

            try
            {
                xml = XElement.Load(Global.ResPath(fileName));
            }
            catch (Exception)
            {
                throw new Exception(string.Format("XML File not found {0}!", fileName));
            }

            IEnumerable<XElement> monsterItems = xml.Elements("Monsters").Elements();
            if (null == monsterItems) return;

            bool isFuBenMap = false;

            foreach (var monsterItem in monsterItems)
            {
                String timePoints = Global.GetSafeAttributeStr(monsterItem, "TimePoints");
                int configBirthType = (int)Global.GetSafeAttributeLong(monsterItem, "BirthType");
                int realBirthType = configBirthType;

                String realTimePoints = timePoints;
                int spawnMonstersAfterKaiFuDays = 0;
                int spawnMonstersDays = 0;
                List<BirthTimeForDayOfWeek> CreateMonstersDayOfWeek = new List<BirthTimeForDayOfWeek>();
                List<BirthTimePoint> birthTimePointList = null;

                //对于开服多少天之后才开始刷怪，进行特殊配置 格式:开服多少天;连续刷多少天[负数0表示一直];刷怪方式0或1;0或1的配置
                if ((int)MonsterBirthTypes.AfterKaiFuDays == configBirthType || (int)MonsterBirthTypes.AfterHeFuDays == configBirthType || (int)MonsterBirthTypes.AfterJieRiDays == configBirthType)
                {
                    String[] arr = timePoints.Split(';');
                    if (4 != arr.Length)
                    {
                        throw new Exception(String.Format("地图{0}的类型4的刷怪配置参数个数不对!!!!", mapCode));
                    }

                    spawnMonstersAfterKaiFuDays = int.Parse(arr[0]);
                    spawnMonstersDays = int.Parse(arr[1]);
                    realBirthType = int.Parse(arr[2]);
                    realTimePoints = arr[3];

                    if ((int)MonsterBirthTypes.TimePoint != realBirthType && (int)MonsterBirthTypes.TimeSpan != realBirthType)
                    {
                        throw new Exception(String.Format("地图{0}的类型4的刷怪配置子类型不对!!!!", mapCode));
                    }
                }

                // MU新增 一周中的哪天刷 TimePoints 配置形式 周几,时间点|周几,时间点|周几,时间点... [1/10/2014 LiaoWei]
                if ((int)MonsterBirthTypes.CreateDayOfWeek == configBirthType)
                {
                    String[] arrTime = timePoints.Split('|');

                    if (arrTime.Length > 0)
                    {
                        for (int nIndex = 0; nIndex < arrTime.Length; ++nIndex)
                        {
                            string sTimePoint = null;
                            sTimePoint = arrTime[nIndex];

                            if (sTimePoint != null)
                            {
                                String[] sTime = null;
                                sTime = sTimePoint.Split(',');

                                if (sTime != null && sTime.Length == 2)
                                {
                                    string sTimeString = null;
                                    int nDayOfWeek = -1;

                                    nDayOfWeek = int.Parse(sTime[0]);
                                    sTimeString = sTime[1];

                                    if (nDayOfWeek != -1 && !string.IsNullOrEmpty(sTimeString))
                                    {
                                        string[] fields2 = sTimeString.Split(':');
                                        if (fields2.Length != 2) continue;

                                        string str1 = fields2[0].TrimStart('0');
                                        string str2 = fields2[1].TrimStart('0');

                                        BirthTimePoint birthTimePoint = new BirthTimePoint()
                                        {
                                            BirthHour = Global.SafeConvertToInt32(str1),
                                            BirthMinute = Global.SafeConvertToInt32(str2),
                                        };

                                        BirthTimeForDayOfWeek BirthTimeTmp = new BirthTimeForDayOfWeek();

                                        BirthTimeTmp.BirthDayOfWeek = nDayOfWeek;
                                        BirthTimeTmp.BirthTime = birthTimePoint;

                                        CreateMonstersDayOfWeek.Add(BirthTimeTmp);
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    birthTimePointList = ParseBirthTimePoints(realTimePoints);
                }

                int TotalNumber = (int)Global.GetSafeAttributeLong(monsterItem, "Num");
                if (TotalNumber > 10)
                {
                    TotalNumber = TotalNumber / 2;
                }
                MonsterZone monsterZone = new MonsterZone()
                {
                    MapCode = mapCode,
                    ID = (int)Global.GetSafeAttributeLong(monsterItem, "ID"),
                    Code = (int)Global.GetSafeAttributeLong(monsterItem, "Code"),
                    ToX = (int)Global.GetSafeAttributeLong(monsterItem, "X") / gameMap.MapGridWidth,
                    ToY = (int)Global.GetSafeAttributeLong(monsterItem, "Y") / gameMap.MapGridHeight,
                    Radius = (int)Global.GetSafeAttributeLong(monsterItem, "Radius") / gameMap.MapGridWidth,
                    TotalNum = TotalNumber,
                    Timeslot = (int)Global.GetSafeAttributeLong(monsterItem, "Timeslot"),
                    IsFuBenMap = isFuBenMap,
                    BirthType = realBirthType,
                    ConfigBirthType = configBirthType,
                    SpawnMonstersAfterKaiFuDays = spawnMonstersAfterKaiFuDays,
                    SpawnMonstersDays = spawnMonstersDays,
                    SpawnMonstersDayOfWeek = CreateMonstersDayOfWeek,
                    BirthTimePointList = birthTimePointList,
                    BirthRate = (int)(Global.GetSafeAttributeDouble(monsterItem, "BirthRate") * 10000),
                };

                XAttribute attrib = monsterItem.Attribute("PursuitRadius");
                if (null != attrib)
                {
                    monsterZone.PursuitRadius = (int)Global.GetSafeAttributeLong(monsterItem, "PursuitRadius");
                }
                else
                {
                    monsterZone.PursuitRadius = (int)Global.GetSafeAttributeLong(monsterItem, "Radius");
                }

                lock (InitMonsterZoneMutex)
                {
                    //加入列表
                    MonsterZoneList.Add(monsterZone);

                    //加入爆怪区域
                    AddMap2MonsterZoneDict(monsterZone);
                }

                //加载静态的怪物信息
                monsterZone.LoadStaticMonsterInfo();

                //加载怪物
                monsterZone.LoadMonsters();//暂时屏蔽怪物加载
            }
        }

        #endregion

        #region Logic các loại quái

        /// <summary>
        /// Thực hiện Logic quái thường
        /// </summary>
        public void RunMapMonsters(SocketListener sl, TCPOutPacketPool pool)
        {
            /// Tải danh sách quái trong khu vực
            for (int i = 0; i < MonsterZoneList.Count; i++)
            {
                MonsterZoneList[i].ReloadMonsters(sl, pool);
            }

            /// Xóa danh sách quái đã chết không phải ở bản đồ thường
            List<int> keys = this.MonsterDynamicZoneDict.Keys.ToList();
            foreach (int key in keys)
            {
                /// Toác
                if (!this.MonsterDynamicZoneDict.TryGetValue(key, out MonsterZone zone))
                {
                    continue;
                }
                zone?.DestroyDeadDynamicMonsters();
            }
        }

        /// <summary>
        /// Thực hiện Logic quái không phải trong bản đồ thường được phục sinh liên tục
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        public void RunMapDynamicMonsters(SocketListener sl, TCPOutPacketPool pool)
        {
            /// Danh sách quái động
            while (!this.WaitingAddDynamicMonsterQueue.IsEmpty)
            {
                if (this.WaitingAddDynamicMonsterQueue.TryDequeue(out MonsterZoneQueueItem monsterZoneQueueItem))
                {
                    /// Thêm quái động
                    monsterZoneQueueItem.MyMonsterZone.LoadDynamicMonsters(monsterZoneQueueItem);
                }
            }
        }

        /// <summary>
        /// Danh sách quái vật động chờ tái sinh
        /// </summary>
        private readonly ConcurrentDictionary<MonsterZoneQueueItem, bool> ListWaitingRespawnDynamicMonster = new ConcurrentDictionary<MonsterZoneQueueItem, bool>();

        /// <summary>
        /// Thêm quái vật cần tái sinh vào danh sách
        /// </summary>
        /// <param name="rootMonsterID"></param>
        /// <param name="queueItem"></param>
        public void AddMonsterToRespawn(MonsterZoneQueueItem queueItem)
        {
            /// Cập nhật thời điểm chết
            queueItem.DeadTick = KTGlobal.GetCurrentTimeMilis();
            /// Thêm vào danh sách
            this.ListWaitingRespawnDynamicMonster[queueItem] = true;
        }

        /// <summary>
        /// Thực hiện tái sinh quái vật tương ứng
        /// </summary>
        public void RunRespawnDynamicMonsters()
        {
            /// Nếu danh sách chờ rỗng
            if (this.ListWaitingRespawnDynamicMonster.Count <= 0)
            {
                return;
            }

            /// Đối tượng tương ứng
            HashSet<MonsterZoneQueueItem> items = new HashSet<MonsterZoneQueueItem>();

            List<MonsterZoneQueueItem> keys = this.ListWaitingRespawnDynamicMonster.Keys.ToList();
            foreach (MonsterZoneQueueItem queueItem in keys)
            {
                ///// Nếu không tồn tại trong danh sách
                //if (!this.ListWaitingRespawnDynamicMonster.TryGetValue(queueItem, out _))
                //{
                //    continue;
                //}

                /// Nếu chưa đến thời gian tái sinh thì bỏ qua
                if (KTGlobal.GetCurrentTimeMilis() - queueItem.DeadTick < queueItem.RespawnTick)
                {
                    continue;
                }

                /// Thêm vào danh sách đối tượng
                items.Add(queueItem);
                /// Xóa khỏi danh sách chờ
                this.ListWaitingRespawnDynamicMonster.TryRemove(queueItem, out _);
            }

            /// Nếu không tìm thấy
            if (items.Count <= 0)
            {
                return;
            }

            /// Duyệt danh sách tái sinh
            foreach (MonsterZoneQueueItem dataItem in items)
            {
                /// Toác gì đó
                if (dataItem.MyMonsterZone == null)
                {
                    continue;
                }
                /// Nếu đang ở trong phụ bản, nhưng phụ bản không còn tồn tại nữa
                else if (dataItem.CopyMapID != -1 && !CopySceneEventManager.IsCopySceneExist(dataItem.CopyMapID, dataItem.MyMonsterZone.MapCode))
                {
                    continue;
                }

                /// Nếu có Predicate thì thực thi xem có thỏa mãn không
                if (dataItem.RespawnPredicate != null && !dataItem.RespawnPredicate.Invoke())
                {
                    continue;
                }

                /// Thêm vào danh sách chờ tải xuống
                this.WaitingAddDynamicMonsterQueue.Enqueue(dataItem);
            }
        }

        #endregion

        #region Thống kê quái

        /// <summary>
        /// Trả về tổng số quái trong phụ bản
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="monsterType"></param>
        /// <returns></returns>
        public int GetMapTotalMonsterNum(int mapCode, MonsterTypes monsterType = MonsterTypes.Normal)
        {
            List<MonsterZone> monsterZoneList = null;
            if (!this.Map2MonsterZoneDict.TryGetValue(mapCode, out monsterZoneList))
            {
                return 0;
            }

            int totalNum = 0;
            for (int i = 0; i < monsterZoneList.Count; i++)
            {
                if (MonsterTypes.Normal != monsterType)
                {
                    if (monsterZoneList[i].MonsterType != monsterType)
                    {
                        continue;
                    }
                }

                totalNum += monsterZoneList[i].TotalNum;
            }

            return totalNum;
        }

        /// <summary>
        /// Trả về tổng số quái có ResID tương ứng trong bản đồ
        /// </summary>
        public int GetMapMonsterNum(int mapCode, int nResID)
        {
            List<MonsterZone> monsterZoneList = null;
            if (!Map2MonsterZoneDict.TryGetValue(mapCode, out monsterZoneList))
            {
                return 0;
            }

            int nCount = 0;
            for (int i = 0; i < monsterZoneList.Count; i++)
            {
                MonsterStaticInfo monsterInfo = null;
                monsterInfo = monsterZoneList[i].GetMonsterInfo();

                if (monsterInfo == null)
                    continue;

                if (monsterInfo.ExtensionID == nResID)
                    nCount += monsterZoneList[i].TotalNum;
            }

            return nCount;
        }


        #endregion


        #region Quái tạm thời

        /// <summary>
        /// Trả về mẫu loại quái dùng để clone ra quái khác trong khu vực
        /// </summary>
        /// <param name="monsterID"></param>
        /// <returns></returns>
        private Monster GetDynamicMonsterSeed(int monsterID)
        {
            Monster monster = null;

            if (this._DictDynamicMonsterSeed.TryGetValue(monsterID, out monster))
            {
                return monster;
            }

            try
            {
                this.InitDynamicMonsterSeedByMonserID(monsterID);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "InitDynamicMonsterSeed()", false);
            }

            this._DictDynamicMonsterSeed.TryGetValue(monsterID, out monster);

            return monster;
        }

        /// <summary>
        /// Khởi tạo mẫu quái theo ID Res tương ứng
        /// </summary>
        /// <param name="monsterID"></param>
        private void InitDynamicMonsterSeedByMonserID(int monsterID)
        {
            MonsterZone monsterZone = new MonsterZone();

            Monster myMonster = null;

            if (this._DictDynamicMonsterSeed.TryGetValue(monsterID, out myMonster) && null != myMonster)
            {
                return;
            }

            int ID = this._DictDynamicMonsterSeed.Count + 1;

            monsterZone.MapCode = 1;
            monsterZone.ID = ID;
            monsterZone.Code = monsterID;

            monsterZone.LoadStaticMonsterInfo();

            myMonster = monsterZone.LoadDynamicMonsterSeed();

            if (!this._DictDynamicMonsterSeed.ContainsKey(monsterID))
            {
                this._DictDynamicMonsterSeed[monsterID] = myMonster;
            }
        }

        /// <summary>
        /// Trả về khu vực quái tạm thời
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public MonsterZone GetDynamicMonsterZone(int mapCode)
        {
            MonsterZone zone = null;
            if (MonsterDynamicZoneDict.TryGetValue(mapCode, out zone))
            {
                return zone;
            }

            return null;
        }

        /// <summary>
        /// Thêm khu vực có quái không phải loại thường
        /// </summary>
        /// <param name="mapCode"></param>
        public void AddDynamicMonsterZone(int mapCode)
        {
            bool isFuBenMap = false;
            MonsterZone monsterZone = new MonsterZone()
            {
                MapCode = mapCode,
                ID = MonsterDynamicZoneDict.Count + 10000,
                Code = -1,
                ToX = -1,
                ToY = -1,
                Radius = 300,
                TotalNum = 0,
                Timeslot = 1,
                IsFuBenMap = isFuBenMap,
                BirthType = (int)MonsterBirthTypes.CrossMap,
                ConfigBirthType = -1,
                BirthTimePointList = null,
                BirthRate = 10000,
            };

            monsterZone.PursuitRadius = 0;

            lock (this.InitMonsterZoneMutex)
            {
                this.MonsterDynamicZoneDict.Add(mapCode, monsterZone);
                this.MonsterZoneList.Add(monsterZone);
                this.AddMap2MonsterZoneDict(monsterZone);
            }
        }

        /// <summary>
        /// Tạo quái di động, không cố định ở MAP, thường dùng trong phụ bản hoặc sự kiện gì đó trong bản đồ thường
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="monsterID"></param>
        /// <param name="copyMapID"></param>
        /// <param name="addNum"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="hp"></param>
        /// <param name="level"></param>
        /// <param name="dir"></param>
        /// <param name="series"></param>
        /// <param name="aiType"></param>
        /// <param name="respawnTick"></param>
        /// <param name="scriptID"></param>
        /// <param name="tag"></param>
        /// <param name="callBack"></param>
        /// <param name="camp"></param>
        /// <param name="respawnPredicate"></param>
        /// <param name="onDieCallback"></param>
        /// <returns>Đối tượng quái vật ảo, không phải thật</returns>
        public bool AddDynamicMonsters(int mapCode, int monsterID, int copyMapID, int addNum, int posX, int posY, string name, string title, int hp, int level, KiemThe.Entities.Direction dir, KE_SERIES_TYPE series, MonsterAIType aiType, long respawnTick, int scriptID, int aiID, string tag, Action<Monster> callBack, int camp = -1, Func<bool> respawnPredicate = null, Action<GameObject> onDieCallback = null)
        {
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out GameMap gameMap))
            {
                return false;
            }

            MonsterZone monsterZone = null;
            if (!MonsterDynamicZoneDict.TryGetValue(mapCode, out monsterZone))
            {
                return false;
            }

            Monster seedMonster = GetDynamicMonsterSeed(monsterID);
            if (null == seedMonster)
            {
                return false;
            }

            this.WaitingAddDynamicMonsterQueue.Enqueue(new MonsterZoneQueueItem()
            {
                CopyMapID = copyMapID,
                BirthCount = addNum,
                MyMonsterZone = monsterZone,
                SeedMonster = seedMonster,
                ToX = (int)posX,
                ToY = (int)posY,
                Name = name,
                Title = title,
                HP = hp,
                Level = level,
                Dir = dir,
                Series = series,
                ScriptID = scriptID,
                AIID = aiID,
                Tag = tag,
                RespawnTick = respawnTick,
                RespawnPredicate = respawnPredicate,
                Done = callBack,
                OnDieCallback = onDieCallback,
                Camp = camp,
                MonsterType = aiType,
            });

            return true;
        }
        #endregion

        #region Chức năng tương tác bên ngoài

        /// <summary>
        /// Trả về danh sách khu vực quái trong bản đồ
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public List<MonsterZone> GetMonsterZoneListByMapCode(int mapCode)
        {
            List<MonsterZone> list = null;
            Map2MonsterZoneDict.TryGetValue(mapCode, out list);
            return list;
        }

        /// <summary>
        /// Trả về danh sách khu vực quái theo bản đồ và ID loại quái tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public List<MonsterZone> GetMonsterZoneByMapCodeAndMonsterID(int mapCode, int monsterID)
        {
            List<MonsterZone> list2 = new List<MonsterZone>();
            List<MonsterZone> list = GetMonsterZoneListByMapCode(mapCode);
            if (null == list) return list2;

            for (int i = 0; i < list.Count; i++)
            {
                if (monsterID == list[i].Code)
                {
                    list2.Add(list[i]);
                }
            }

            return list2;
        }
        #endregion
    }
}
