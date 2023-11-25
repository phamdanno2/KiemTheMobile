using GameServer.Core.Executor;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{
    public class ItemDropManager
    {
        public static string DropBuild_XML = "Config/KT_Drop/DropBuild.xml";

        public static string MapDropProfile_XML = "Config/KT_Drop/MapDropProfile.xml";

        public static string PickUpItemNotifyfile_XML = "Config/KT_Drop/PickUpItemNotify.xml";

        /// <summary>
        /// File quy định danh sách tỷ lệ rơi ở quái
        /// </summary>
        public static string MonsterDropRateFile_XML = "Config/KT_Drop/MonsterDropRate.xml";

        public static List<DropProfile> TotalProfile = new List<DropProfile>();

        public static List<MapDropInfo> TotalMapDrop = new List<MapDropInfo>();

        /// <summary>
        /// Danh sách tỷ lệ rơi đồ ở quái
        /// </summary>
        public static Dictionary<int, MonsterDropRate> MonsterDrops { get; } = new Dictionary<int, MonsterDropRate>();

        /// <summary>
        /// Danh sách vật phẩm sẽ thông báo khi nhặt
        /// </summary>
        public static PickUpItemNotify PickUpItemNotify { get; private set; }

        public static int ItemRandomRager = 1000000;

        /// <summary>
        /// Loading all Drop
        /// </summary>
        public static void Setup()
        {
            Console.WriteLine("Loading Drop Profile..");
            string Files = Global.GameResPath(DropBuild_XML);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<DropProfile>));
                TotalProfile = serializer.Deserialize(stream) as List<DropProfile>;
            }

            Console.WriteLine("Loading Monster Drop Config..");
            Files = Global.GameResPath(MapDropProfile_XML);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<MapDropInfo>));
                TotalMapDrop = serializer.Deserialize(stream) as List<MapDropInfo>;
            }

            Console.WriteLine("Loading Pick Up Item Notify..");
            Files = Global.GameResPath(PickUpItemNotifyfile_XML);
            XElement pickUpItemNotifyNode = XElement.Parse(File.ReadAllText(Files));
            ItemDropManager.PickUpItemNotify = PickUpItemNotify.Parse(pickUpItemNotifyNode);

            Console.WriteLine("Loading Monster Drop Rate..");
            Files = Global.GameResPath(MonsterDropRateFile_XML);
            XElement monsterDropRateNode = XElement.Parse(File.ReadAllText(Files));
            foreach (XElement node in monsterDropRateNode.Elements("DropInfo"))
            {
                MonsterDropRate dropRate = MonsterDropRate.Parse(node);
                MonsterDrops[dropRate.MonsterID] = dropRate;
            }
        }

        /// <summary>
        /// Gọi đến khi quái chết để rơi vật phẩm tương ứng
        /// </summary>
        /// <param name="MonsertID"></param>
        /// <param name="Player"></param>
        public static void GetDropMonsterDie(Monster Monster, KPlayer Player)
        {
            //Lấy ra may mắn hiện tại
            int Lucky = Player.m_nCurLucky;

            //if ((int)Monster.MonsterType == 4)
            //{
            //    if (!PirateTaskManager.getInstance().IsHavePitateQuest(Player))
            //    {
            //        return;
            //    }
            //}

            List<DropProfile> _TotalDropCount = ItemDropManager.GetDropItem(Monster, Player, Lucky);
            if (_TotalDropCount == null)
            {
                return;
            }

            // Tổng số lượng Bạc Khóa sẽ nhận
            int BindMoney = 0;

            // Tổng số lượng Bạc sẽ nhận
            int Money = 0;

            List<ItemData> ListItemDataDrop = new List<ItemData>();

            if (_TotalDropCount.Count > 0)
            {
                foreach (DropProfile _Drop in _TotalDropCount)
                {
                    if (_Drop.BindMoney > 0)
                    {   // Tổng lượng bạc khóa
                        BindMoney += _Drop.BindMoney;
                    }
                    if (_Drop.Money > 0)
                    {   // Tổng lượng bạc nhận được
                        Money += _Drop.Money;
                    }

                    if (_Drop.ItemID > 0)
                    {
                        if (ItemManager._TotalGameItem.ContainsKey(_Drop.ItemID))
                        {
                            ItemData _TMPITEM = ItemManager._TotalGameItem[_Drop.ItemID];

                            ListItemDataDrop.Add(_TMPITEM);
                        }
                    }
                }
            }

            // Thực hiện add bạc khóa cho người chơi
            if (BindMoney > 0)
            {
                GameManager.ClientMgr.AddUserBoundMoney(Player, BindMoney, "MONSTERDROP | " + Monster.MonsterInfo.ExtensionID + "");
            }

            // Thực hiện add bạc cho người chơi
            if (Money > 0)
            {
                GameManager.ClientMgr.AddMoney(Player, Money, "MONSTERDROP | " + Monster.MonsterInfo.ExtensionID + "");
            }

            // Thực hiện cho DROP ở đây
            if (ListItemDataDrop.Count > 0)
            {
                CreateDropToMap(Player, ListItemDataDrop, Monster);
            }
        }

        public static GoodsData CreateGoodDataFromItemData(ItemData ListItemDataDrop)
        {
            GoodsData _GoodData = new GoodsData();
            _GoodData.Id = -1;
            _GoodData.GoodsID = ListItemDataDrop.ItemID;
            _GoodData.GCount = 1;
            _GoodData.Props = ItemManager.GenerateProbs(ListItemDataDrop);
            _GoodData.Series = ItemManager.GetItemSeries(ListItemDataDrop.Series);
            _GoodData.Site = 0;
            _GoodData.Strong = 100;
            _GoodData.Using = -1;
            _GoodData.BagIndex = -1;
            _GoodData.Binding = 0;
            _GoodData.Forge_level = 0;

            return _GoodData;
        }

        public static void CreateDropMapFromSingerGooods(KPlayer client, GoodsData _Data)
        {
            Dictionary<string, bool> gridDict = new Dictionary<string, bool>();

            GoodsPackItem goodsPackItem = new GoodsPackItem()
            {
                AutoID = GameManager.GoodsPackMgr.GetNextAutoID(),
                OwnerRoleID = client.RoleID,
                OwnerRoleName = client.RoleName,
                GoodsPackType = 1,
                ProduceTicks = TimeUtil.NOW(),
                LockedRoleID = -1,
                ItemInPacker = _Data,
                TeamRoleIDs = null,
                MapCode = client.MapCode,
                CopyMapID = client.CopyMapID,
                // SET BELLONG = -1 để không check sở hữu
                BelongTo = -1,
                FallLevel = 0,
                TeamID = -1,
            };


            if ((_Data.GoodsID >= 190 && _Data.GoodsID <= 194) || (_Data.GoodsID >= 393 && _Data.GoodsID <= 396))
            {
                ItemData _Template = ItemManager.GetItemTemplate(_Data.GoodsID);
                if (_Template != null)
                {
                    LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][VỨT VẬT PHẨM][" + _Data.GoodsID + "][" + _Template.Name + "] ra ngoài đất");
                }

            }



            // Rơi ra ngay vị trí thằng vứt
            goodsPackItem.FallPoint = GameManager.GoodsPackMgr.GetFallGoodsPosition(ObjectTypes.OT_GOODSPACK, client.MapCode, gridDict, new Point((int)(client.CurrentGrid.X), (int)(client.CurrentGrid.Y)), client.CopyMapID, client);

            lock (GameManager.GoodsPackMgr.GoodsPackDict)
            {
                GameManager.GoodsPackMgr.GoodsPackDict[goodsPackItem.AutoID] = goodsPackItem;
            }
            GameManager.GoodsPackMgr.ProcessGoodsPackItem(client, client, goodsPackItem, 1);
        }

        /// <summary>
        /// Tạo Drop ở MAP
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ListItemDataDrop"></param>
        /// <param name="Monster"></param>
        public static void CreateDropToMap(KPlayer client, List<ItemData> ListItemDataDrop, Monster Monster)
        {

            KPlayer TmpClient = client;

            bool IsDropFromBoss = false;

            if ((int)Monster.MonsterType == 3)
            {
                var FindTop = Monster.DamageTakeRecord.Values.OrderByDescending(x => x.TotalDamage).FirstOrDefault();

                if (FindTop != null)
                {
                    if (FindTop.TotalDamage > 0)
                    {
                        if (!FindTop.IsTeam)
                        {
                            KPlayer _client = GameManager.ClientMgr.FindClient(FindTop.ID);
                            if (_client != null)
                            {
                                client = _client;
                            }
                        }
                        else
                        {
                            KPlayer _client = KTTeamManager.GetTeamLeader(FindTop.ID);
                            if (_client != null)
                            {
                                client = _client;
                            }
                        }

                    }
                }

                IsDropFromBoss = true;
            }

            List<GoodsPackItem> goodsPackItemList = new List<GoodsPackItem>();

            Dictionary<string, bool> gridDict = new Dictionary<string, bool>();

            for (int i = 0; i < ListItemDataDrop.Count; i++)
            {
                GoodsData _GoodDrop = CreateGoodDataFromItemData(ListItemDataDrop[i]);
                GoodsPackItem goodsPackItem = new GoodsPackItem()
                {
                    AutoID = GameManager.GoodsPackMgr.GetNextAutoID(),
                    OwnerRoleID = client.RoleID,
                    OwnerRoleName = client.RoleName,
                    GoodsPackType = 0,
                    ProduceTicks = TimeUtil.NOW(),
                    LockedRoleID = -1,
                    ItemInPacker = _GoodDrop,
                    TeamRoleIDs = null,
                    MapCode = TmpClient.MapCode,
                    CopyMapID = TmpClient.CopyMapID,
                    BelongTo = 1,
                    FallLevel = 0,
                    TeamID = client.TeamID,
                    DropFromBoss = IsDropFromBoss,

                };

                goodsPackItem.FallPoint = GameManager.GoodsPackMgr.GetFallGoodsPosition(ObjectTypes.OT_GOODSPACK, Monster.CurrentMapCode, gridDict, new Point((int)(Monster.CurrentGrid.X), (int)(Monster.CurrentGrid.Y)), TmpClient.CopyMapID, TmpClient);
                goodsPackItemList.Add(goodsPackItem);

                lock (GameManager.GoodsPackMgr.GoodsPackDict)
                {
                    GameManager.GoodsPackMgr.GoodsPackDict[goodsPackItem.AutoID] = goodsPackItem;
                }

                for (int j = 0; j < goodsPackItemList.Count; j++)
                {
                    GameManager.GoodsPackMgr.ProcessGoodsPackItem(client, client, goodsPackItemList[i], 1);
                }
            }
        }

        /// <summary>
        /// Tạo Drop ở MAP
        /// </summary>
        /// <param name="ListItemDataDrop"></param>
        /// <param name="Monster"></param>
        public static void CreateDropToMap(List<ItemData> ListItemDataDrop, Monster monster)
        {
            List<GoodsPackItem> goodsPackItemList = new List<GoodsPackItem>();

            Dictionary<string, bool> gridDict = new Dictionary<string, bool>();

            for (int i = 0; i < ListItemDataDrop.Count; i++)
            {
                GoodsData _GoodDrop = CreateGoodDataFromItemData(ListItemDataDrop[i]);
                GoodsPackItem goodsPackItem = new GoodsPackItem()
                {
                    AutoID = GameManager.GoodsPackMgr.GetNextAutoID(),
                    OwnerRoleID = -1,
                    OwnerRoleName = "",
                    GoodsPackType = 0,
                    ProduceTicks = TimeUtil.NOW(),
                    LockedRoleID = -1,
                    ItemInPacker = _GoodDrop,
                    TeamRoleIDs = null,
                    MapCode = monster.CurrentMapCode,
                    CopyMapID = monster.CurrentCopyMapID,
                    BelongTo = 1,
                    FallLevel = 0,
                    TeamID = -1,
                };

                goodsPackItem.FallPoint = GameManager.GoodsPackMgr.GetFallGoodsPosition(ObjectTypes.OT_GOODSPACK, monster.CurrentMapCode, gridDict, new Point((int)(monster.CurrentGrid.X), (int)(monster.CurrentGrid.Y)), monster.CurrentCopyMapID, null);
                goodsPackItemList.Add(goodsPackItem);

                lock (GameManager.GoodsPackMgr.GoodsPackDict)
                {
                    GameManager.GoodsPackMgr.GoodsPackDict[goodsPackItem.AutoID] = goodsPackItem;
                }

                for (int j = 0; j < goodsPackItemList.Count; j++)
                {
                    GameManager.GoodsPackMgr.ProcessGoodsPackItem(null, null, goodsPackItemList[i], 1);
                }
            }
        }

        /// <summary>
        /// Tạo Drop ở MAP
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ListItemDataDrop"></param>
        /// <param name="Monster"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public static void CreateDropToMap(KPlayer client, List<ItemData> ListItemDataDrop, int posX, int posY)
        {
            List<GoodsPackItem> goodsPackItemList = new List<GoodsPackItem>();

            Dictionary<string, bool> gridDict = new Dictionary<string, bool>();

            for (int i = 0; i < ListItemDataDrop.Count; i++)
            {
                GoodsData _GoodDrop = CreateGoodDataFromItemData(ListItemDataDrop[i]);
                GoodsPackItem goodsPackItem = new GoodsPackItem()
                {
                    AutoID = GameManager.GoodsPackMgr.GetNextAutoID(),
                    OwnerRoleID = client.RoleID,
                    OwnerRoleName = client.RoleName,
                    GoodsPackType = 0,
                    ProduceTicks = TimeUtil.NOW(),
                    LockedRoleID = -1,
                    ItemInPacker = _GoodDrop,
                    TeamRoleIDs = null,
                    MapCode = client.MapCode,
                    CopyMapID = client.CopyMapID,
                    BelongTo = 1,
                    FallLevel = 0,
                    TeamID = client.TeamID,
                };

                goodsPackItem.FallPoint = GameManager.GoodsPackMgr.GetFallGoodsPosition(ObjectTypes.OT_GOODSPACK, client.MapCode, gridDict, new Point(posX, posY), client.CopyMapID, client);
                goodsPackItemList.Add(goodsPackItem);

                lock (GameManager.GoodsPackMgr.GoodsPackDict)
                {
                    GameManager.GoodsPackMgr.GoodsPackDict[goodsPackItem.AutoID] = goodsPackItem;
                }

                for (int j = 0; j < goodsPackItemList.Count; j++)
                {
                    GameManager.GoodsPackMgr.ProcessGoodsPackItem(client, client, goodsPackItemList[i], 1);
                }
            }
        }

        /// <summary>
        /// Tạo Drop ở MAP
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapCode"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="items"></param>
        public static void CreateDropToMap(int mapCode, int copyMapCode, int posX, int posY, List<ItemData> items)
        {
            List<GoodsPackItem> goodsPackItemList = new List<GoodsPackItem>();

            Dictionary<string, bool> gridDict = new Dictionary<string, bool>();

            for (int i = 0; i < items.Count; i++)
            {
                GoodsData _GoodDrop = CreateGoodDataFromItemData(items[i]);
                GoodsPackItem goodsPackItem = new GoodsPackItem()
                {
                    AutoID = GameManager.GoodsPackMgr.GetNextAutoID(),
                    OwnerRoleID = -1,
                    OwnerRoleName = "",
                    GoodsPackType = 0,
                    ProduceTicks = TimeUtil.NOW(),
                    LockedRoleID = -1,
                    ItemInPacker = _GoodDrop,
                    TeamRoleIDs = null,
                    MapCode = mapCode,
                    CopyMapID = copyMapCode,
                    BelongTo = 1,
                    FallLevel = 0,
                    TeamID = -1,
                };

                goodsPackItem.FallPoint = GameManager.GoodsPackMgr.GetFallGoodsPosition(ObjectTypes.OT_GOODSPACK, mapCode, gridDict, new Point(posX, posY), copyMapCode, null);
                goodsPackItemList.Add(goodsPackItem);

                lock (GameManager.GoodsPackMgr.GoodsPackDict)
                {
                    GameManager.GoodsPackMgr.GoodsPackDict[goodsPackItem.AutoID] = goodsPackItem;
                }

                for (int j = 0; j < goodsPackItemList.Count; j++)
                {
                    GameManager.GoodsPackMgr.ProcessGoodsPackItem(null, null, goodsPackItemList[i], 1);
                }
            }
        }

        public static int GetMaxCount(int TYPEMON, int LucKey)
        {
            if (TYPEMON == 0)
            {
                return 1;
            }
            else if (TYPEMON == 1)
            {
                int Min = 3;

                int _Random = KTGlobal.GetRandomNumber(0, 100);

                //Tăng tỉ lệ rơi đồ nếu LUCKY > 0
                if (LucKey > 0)
                {
                    _Random = _Random + (_Random * LucKey / 100);
                }
                if (_Random < (100 - KTGlobal.DropRate))
                {
                    return Min;
                }

                return 5;
            }
            else if (TYPEMON == 2)
            {
                int Min = 4;

                int _Random = KTGlobal.GetRandomNumber(0, 100);

                //Tăng tỉ lệ rơi đồ nếu LUCKY > 0
                if (LucKey > 0)
                {
                    _Random = _Random + (_Random * LucKey / 100);
                }
                if (_Random < (100 - KTGlobal.DropRate))
                {
                    return Min;
                }

                return 6;
            }
            else if (TYPEMON == 3)
            {
                int Min = 5;

                int _Random = KTGlobal.GetRandomNumber(0, 100);

                //Tăng tỉ lệ rơi đồ nếu LUCKY > 0
                if (LucKey > 0)
                {
                    _Random = _Random + (_Random * LucKey / 100);
                }
                if (_Random < (100 - KTGlobal.DropRate))
                {
                    return Min;
                }

                return 8;
            }
            else if (TYPEMON == 4)
            {
                int Min = 3;

                int _Random = KTGlobal.GetRandomNumber(0, 100);

                //Tăng tỉ lệ rơi đồ nếu LUCKY > 0
                if (LucKey > 0)
                {
                    _Random = _Random + (_Random * LucKey / 100);
                }
                if (_Random < (100 - KTGlobal.DropRate))
                {
                    return Min;
                }

                return 6;
            }

            return 1;
        }

        public static List<DropProfile> GetDropItem(Monster Monster, KPlayer player, int LucKey = 0)
        {
            List<DropProfile> _DropSelect = new List<DropProfile>();


            MapDropInfo MonsterFind = TotalMapDrop.Where(x => x.MonsterID == Monster.MonsterInfo.ExtensionID).FirstOrDefault();

            if (MonsterFind == null)
            {
                return null;
            }

            int ItemCount = ItemDropManager.GetMaxCount((int)Monster.MonsterType, LucKey);

            if (MonsterFind != null)
            {
                //Sắp xếp theo tỉ lệ giảm dàn của RATE
                List<DropProfile> TotalDrop = MonsterFind.TotalFile.OrderByDescending(x => x.Rate).ToList();

                //Lấy ra tổng RATE
                int SumDrop = TotalDrop.Sum(x => x.Rate);

                // Lần ramdom này xác định đồ có rơi hay không rơi
                int _Random = KTGlobal.GetRandomNumber(0, 100);

                //Tăng tỉ lệ rơi đồ nếu LUCKY > 0
                if (LucKey > 0)
                {
                    _Random = _Random + (_Random * LucKey / 100);
                }


                /// Nếu có tỷ lệ trong File XML
                if (ItemDropManager.MonsterDrops.TryGetValue(Monster.MonsterInfo.ExtensionID, out MonsterDropRate dropRate))
                {
                    /// Nếu không thuộc bản đồ tương ứng
                    if (dropRate.MapIDs != null && !dropRate.MapIDs.Contains(Monster.CurrentMapCode))
                    {
                        /// Toác
                        return null;
                    }

                    /// Nếu không thuộc phụ bản
                    if (dropRate.CopySceneOnly && !CopySceneEventManager.IsCopySceneExist(Monster.CurrentCopyMapID, Monster.CurrentMapCode))
                    {
                        /// Toác
                        return null;
                    }

                    /// Nếu có quy định nhiệm vụ
                    if (dropRate.QuestIDs != null)
                    {
                        /// Đánh dấu tìm thấy nhiệm vụ
                        bool foundCorrespondingTask = false;
                        /// Duyệt danh sách nhiệm vụ
                        foreach (TaskData taskData in player.TaskDataList)
                        {
                            /// Nếu tồn tại nhiệm vụ tương ứng yêu cầu
                            if (dropRate.QuestIDs.Contains(taskData.DoingTaskID))
                            {
                                /// Tìm thấy
                                foundCorrespondingTask = true;
                                /// Thoát
                                break;
                            }
                        }

                        /// Nếu không tìm thấy nhiệm vụ tương ứng
                        if (!foundCorrespondingTask)
                        {
                            /// Toác
                            return null;
                        }
                    }

                    /// Tỷ lệ rơi cố định
                    int nRate = KTGlobal.GetRandomNumber(0, 10000);
                    /// Nếu nhân phẩm đéo đủ
                    if (nRate > dropRate.Rate)
                    {
                        /// Toác
                        return null;
                    }
                }
                /// Loại khác
                else
                {
                    // Nếu là quái thường xác định xem lần giết này có rơi vật phẩm hay không
                    if ((int)Monster.MonsterType == 0 || (int)Monster.MonsterType == 5 || (int)Monster.MonsterType == 6)
                    {
                        if ((int)Monster.MonsterType == 5 || (int)Monster.MonsterType == 6)
                        {
                            if (_Random < 50)
                            {
                                return null;
                            }
                        }
                        else
                        {
                            // Nếu random mà nhỏ hơn tỉ lệ rơi glolal thì ko rơi gì

                            if (_Random < (100 - KTGlobal.DropRate))
                            {
                                return null;
                            }
                        }

                    }
                }


                {
                    // Thực hiện rơi đủ các vật phẩm
                    for (int j = 0; j < ItemCount; j++)
                    {
                        // Nếu mà đã rơi rồi thì thực hiện random lại để lấy ra vật phẩm theo RATE
                        int RandomSelect = KTGlobal.GetRandomNumber(0, SumDrop);

                        //SET CHỈ SỐ BAN ĐẦU
                        int Count = 0;

                        for (int i = 0; i < TotalDrop.Count; ++i)
                        {
                            DropProfile TmpDrop = TotalDrop[i];

                            // MỖI LẦN + THÊM 1 LƯỢNG MAY MẮN NẾU ĐỎ THÌ SẼ NHẬN ĐƯỢC ĐỒ PHẨM CHẤT CAO
                            int RateL = TmpDrop.Rate + LucKey;

                            //MỖI LẦN TĂNG THÊM TÝ
                            Count += RateL;

                            //NẾU ĐÃ CHẠM NGƯỠNG RƠI THÌ ADD ĐỒ RƠI VÀO
                            if (Count > RandomSelect)
                            {
                                _DropSelect.Add(TmpDrop);
                                break;
                            }
                        }
                    }
                }
            }

            return _DropSelect;
        }
    }
}