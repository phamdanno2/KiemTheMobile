using GameServer.Core.Executor;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace GameServer.KiemThe.Logic.Manager.Shop
{
    public class ShopManager
    {
        public static List<ShopTab> _TotalShopTab = new List<ShopTab>();

        public static List<ShopItem> _TotalShopTtem = new List<ShopItem>();

        /// <summary>
        /// Kỳ Trân Các
        /// </summary>
        public static TokenShop TokenShop { get; private set; } = new TokenShop()
        {
            Token = new List<ShopTab>(),
            BoundToken = new List<ShopTab>(),
            StoreProducts = new List<TokenShopStoreProduct>(),
        };

        public static string ShopItem_XML = "Config/KT_Shop/ShopItem.xml";
        public static string ShopTab_XML = "Config/KT_Shop/ShopTab.xml";
        public static string TokenShopFileDir = "Config/KT_Shop/TokenShop.xml";

        public static List<int> rankOfLevel = new List<int>();
        public static List<int> rankOfMoney = new List<int>();
        public static void Setup()
        {
            LoadShopitem(ShopItem_XML);
            LoadShopTab(ShopTab_XML);
            ShopManager.LoadTokenShop();


            rankOfLevel = LoadRank("rankOfLevel.txt");
            rankOfMoney = LoadRank("rankOfMoney.txt");
        }
        public static List<int> LoadRank(string path)
        {
            var ranks = new List<int>();

            if (System.IO.File.Exists(path))
            {
                var strs = System.IO.File.ReadAllLines(path);
                foreach (var str in strs)
                {
                    var secs = str.Split('#');
                    if (secs.Length > 0)
                    {
                        ranks.Add(int.Parse(secs[0]));
                    }
                }
            }

            return ranks;
        }

        public static void LoadShopitem(string FilesPath)
        {
            string Files = Global.GameResPath(FilesPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<ShopItem>));
                _TotalShopTtem = serializer.Deserialize(stream) as List<ShopItem>;
            }
        }

        public static void LoadShopTab(string FilesPath)
        {
            string Files = Global.GameResPath(FilesPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<ShopTab>));
                _TotalShopTab = serializer.Deserialize(stream) as List<ShopTab>;
            }
        }

        /// <summary>
        /// Tải dữ liệu Kỳ Trân Các
        /// </summary>
        private static void LoadTokenShop()
        {
            string files = Global.GameResPath(ShopManager.TokenShopFileDir);
            string content = System.IO.File.ReadAllText(files);
            XElement xmlNode = XElement.Parse(content);

            /// Tiệm Đồng
            foreach (XElement tokenShopNode in xmlNode.Element("Token").Elements("Shop"))
            {
                int shopID = int.Parse(tokenShopNode.Attribute("ID").Value);

                ShopTab shopTab = ShopManager._TotalShopTab.Where(x => x.ShopID == shopID).FirstOrDefault();

                if (shopTab == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Không tìm thấy thông tin Shop tương ứng, ID={0}", shopID));
                }
                else
                {
                    shopTab.Items = ShopManager.GetItemShopFromList(shopTab.ShopItem);
                    ShopManager.TokenShop.Token.Add(shopTab);
                }
            }

            /// Tiệm Đồng khóa
            foreach (XElement tokenShopNode in xmlNode.Element("BoundToken").Elements("Shop"))
            {
                int shopID = int.Parse(tokenShopNode.Attribute("ID").Value);

                ShopTab shopTab = ShopManager._TotalShopTab.Where(x => x.ShopID == shopID).FirstOrDefault();
                if (shopTab == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Không tìm thấy thông tin Shop tương ứng, ID={0}", shopID));
                }
                else
                {
                    shopTab.Items = ShopManager.GetItemShopFromList(shopTab.ShopItem);
                    ShopManager.TokenShop.BoundToken.Add(shopTab);
                }
            }

            /// Gói hàng bán trên Store
            foreach (XElement node in xmlNode.Element("StoreProduct").Elements("Product"))
            {
                TokenShopStoreProduct storeProduct = new TokenShopStoreProduct()
                {
                    ID = node.Attribute("ID").Value,
                    Name = node.Attribute("Name").Value,
                    Hint = node.Attribute("Hint").Value,
                    Icon = node.Attribute("Icon").Value,
                    Recommend = bool.Parse(node.Attribute("Recommend").Value),
                    Price = int.Parse(node.Attribute("Price").Value),
                    Token = int.Parse(node.Attribute("Token").Value),
                    FirstBonus = int.Parse(node.Attribute("FirstBonus").Value),
                };
                ShopManager.TokenShop.StoreProducts.Add(storeProduct);
            }
        }

        public static TokenShopStoreProduct GetProductByID(string ID)
        {
            TokenShopStoreProduct _Product = new TokenShopStoreProduct();

            var find = TokenShop.StoreProducts.Where(x => x.ID == ID).FirstOrDefault();

            if (find != null)
            {
                return find;
            }
            else
            {
                return null;
            }
        }

        public static List<ShopItem> GetItemShopFromList(List<int> InputData)
        {
            List<ShopItem> _Items = new List<ShopItem>();

            foreach (int ID in InputData)
            {
                var findItem = _TotalShopTtem.Where(x => x.ID == ID).FirstOrDefault();
                if (findItem != null)
                {
                    _Items.Add(findItem);
                }
            }

            return _Items;
        }

        public static ShopTab GetShopTable(int ShopID, KPlayer player)
        {
            ShopTab _Shop = null;

            _Shop = _TotalShopTab.Where(x => x.ShopID == ShopID).FirstOrDefault();

            if (_Shop != null)
            {
                _Shop.Items = ShopManager.GetItemShopFromList(_Shop.ShopItem);
                _Shop.TotalSellItem = player.GetGoodAreadySellBefore();
            }

            return _Shop;
        }


        public static int GetGuildMoneyNeed(int OfficeRank)
        {
            if (OfficeRank == 1)
            {
                return 100000;
            }
            else if (OfficeRank == 2)
            {
                return 200000;
            }
            else if (OfficeRank == 3)
            {
                return 400000;
            }
            else if (OfficeRank == 4)
            {
                return 600000;
            }
            else if (OfficeRank == 5)
            {
                return 800000;
            }
            return -1;
        }
        /// <summary>
        /// Xử lý sự kiện mua của người chơi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ShopID"></param>
        /// <param name="ShopItemID"></param>
        /// <param name="Number"></param>
        /// <param name="counponID"></param>
        /// <returns></returns>
        public static SubRep BuyItem(KPlayer player, int ShopID, int ShopItemID, int Number, int couponID)
        {
            SubRep _SubRep = new SubRep();
            _SubRep.IsOK = false;
            _SubRep.IsBuyBack = false;
            _SubRep.CountLess = 0;

            try
            {
                /// TODO xử lý phiếu giảm giá Kỳ Trân Các

                ShopTab ShopFind = _TotalShopTab.Where(x => x.ShopID == ShopID).FirstOrDefault();

                bool NeedWriterLogs = false;

                if (ShopFind != null)
                {
                    if (!ShopFind.ShopItem.Contains(ShopItemID))
                    {
                        PlayerManager.ShowNotification(player, "Vật phẩm muốn mua không tồn tại");
                        return _SubRep;
                    }

                    ShopItem _item = _TotalShopTtem.Where(x => x.ID == ShopItemID).FirstOrDefault();

                    if (_item != null)
                    {
                        MoneyType Money = (MoneyType)ShopFind.MoneyType;

                        var FindTempate = ItemManager.GetItemTemplate(_item.ItemID);

                        if (FindTempate == null)
                        {
                            PlayerManager.ShowNotification(player, "Vật phẩm muốn mua không tồn tại");
                            return _SubRep;
                        }

                        int RATE = ShopFind.Discount;

                        int TotalMoneyNeed = 0;

                        int ItemDiscount = 0;

                        bool ItUsingTicket = false;

                        //Nếu là shop đồng hoặc đông khóa
                        if (Money == MoneyType.Dong || Money == MoneyType.DongKhoa)
                        {
                            if (couponID != -1)
                            {
                                GoodsData FindGoldById = Global.GetGoodsByDbID(player, couponID);
                                if (FindGoldById != null)
                                {
                                    ItemData TMP = ItemManager.GetItemTemplate(FindGoldById.GoodsID);
                                    if (TMP != null)
                                    {
                                        if (TMP.Genre == 18 && TMP.DetailType == 1 && TMP.ParticularType >= 1550 && TMP.ParticularType <= 1554)
                                        {
                                            ItUsingTicket = true;
                                            ItemDiscount = TMP.ItemValue;
                                        }
                                    }
                                }
                            }

                            // Chỉ ghi logs với shop đồng hoặc đồng khóa
                            NeedWriterLogs = true;
                            TotalMoneyNeed = _item.GoodsPrice * Number;
                        }
                        else
                        {
                            TotalMoneyNeed = FindTempate.Price * Number;
                        }

                        // Nếu là Shop giảm giá
                        if (RATE > 0)
                        {
                            int Discount = (RATE * TotalMoneyNeed) / 100;
                            TotalMoneyNeed = TotalMoneyNeed - Discount;
                        }

                        if (ItemDiscount > 0)
                        {
                            int Discount = (ItemDiscount * TotalMoneyNeed) / 100;

                            TotalMoneyNeed = TotalMoneyNeed - Discount;
                        }

                        int TotalBuyNumber = Number;

                        int TotalSpaceNeed = ItemManager.TotalSpaceNeed(FindTempate.ItemID, TotalBuyNumber);

                        // Check xem nếu không đủ chỗ trống để mua
                        if (!KTGlobal.IsHaveSpace(TotalSpaceNeed, player))
                        {
                            PlayerManager.ShowNotification(player, "Túi đồ không đủ chỗ trống! Cần " + TotalSpaceNeed + " vị trí trống để có thể mua vật phẩm này!");

                            return _SubRep;
                        }

                        int TempCount = 0;

                        if (_item.GoodsIndex == -1 && !KTGlobal.IsHaveMoney(player, TotalMoneyNeed, Money) && _item.GoodsIndex != -1)
                        {
                            PlayerManager.ShowNotification(player, "Tiền yêu cầu trên người không đủ!");
                            return _SubRep;
                        }
                        else
                        {
                            if (_item.ReputeDBID != -1)
                            {
                                ReputeInfo _Info = player.GetRepute().Where(x => x.DBID == _item.ReputeDBID).FirstOrDefault();
                                if (_Info.Level < _item.ReputeLevel)
                                {
                                    PlayerManager.ShowNotification(player, "Danh vọng không đủ");
                                    return _SubRep;
                                }
                            }
                            //TODO
                            if (_item.OfficialLevel != -1)
                            {

                                if (player.OfficeRank < _item.OfficialLevel)
                                {
                                    PlayerManager.ShowNotification(player, "Danh vọng lãnh thổ không đủ không thể mua!");
                                    return _SubRep;
                                }

                                int GuildMoneyNeed = GetGuildMoneyNeed(_item.OfficialLevel);

                                if (!KTGlobal.IsHaveMoney(player, GuildMoneyNeed, MoneyType.GuildMoney))
                                {
                                    PlayerManager.ShowNotification(player, "Tài sản cá nhân bang hội không đủ!");
                                    return _SubRep;
                                }

                            }

                            if (_item.Honor != -1)
                            {
                                int Rank = KTGlobal.GetRankHonor(player.GetTotalValue());
                                if (Rank < _item.Honor)
                                {
                                    PlayerManager.ShowNotification(player, "Vinh dự tài phú không đủ");
                                    return _SubRep;
                                }
                            }

                            //YÊU CẦU TIỀN CỐNG HIẾN BANG HỘI
                            if (_item.TongFund != -1)
                            {
                                int Request = _item.TongFund * Number;
                                if (player.RoleGuildMoney < Request)
                                {
                                    PlayerManager.ShowNotification(player, "Tiền bang hội không đủ");
                                }
                            }

                            if (_item.GoodsIndex != -1)
                            {
                                int NumberRequest = _item.GoodsPrice * Number;

                                int CountInBag = ItemManager.GetItemCountInBag(player, _item.GoodsIndex);

                                if (CountInBag < NumberRequest)
                                {
                                    PlayerManager.ShowNotification(player, "Đạo cụ mua không đủ");
                                    return _SubRep;
                                }
                            }

                            LimitType _ItemLimit = (LimitType)_item.LimitType;

                            if (_ItemLimit == LimitType.BuyCountPerDay)
                            {
                                string SendToDB = string.Format("{0}:{1}", player.RoleID, ShopItemID);

                                string[] DataRead = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYLIMITGOODSUSEDNUM, SendToDB, player.ServerId);

                                if (DataRead == null)
                                {
                                    PlayerManager.ShowNotification(player, "Có lỗi khi kiểm tra nhật ký");
                                    return _SubRep;
                                }

                                int Ret = Int32.Parse(DataRead[1]);

                                if (Ret == -1)
                                {
                                    PlayerManager.ShowNotification(player, "Có lỗi khi kiểm tra nhật ký");
                                    return _SubRep;
                                }
                                else
                                {
                                    int TotalBuyInDay = Int32.Parse(DataRead[3]);

                                    TempCount = TotalBuyInDay;

                                    if (TotalBuyInDay + TotalBuyNumber > _item.LimitValue)
                                    {
                                        PlayerManager.ShowNotification(player, "Hôm nay bạn đã mua hết giới hạn cho phép của vật phẩm này");

                                        return _SubRep;
                                    }
                                }
                            }

                            //TUẦN TODO
                            if (_ItemLimit == LimitType.BuyCountPerWeek)
                            {
                                int TotalBuyInWeek = player.GetValueOfWeekRecore(ShopItemID);

                                if (TotalBuyInWeek == -1)
                                {
                                    TotalBuyInWeek = 0;
                                }
                                if (TotalBuyInWeek + TotalBuyNumber > _item.LimitValue)
                                {
                                    PlayerManager.ShowNotification(player, "Tuần này bạn đã mua hết số lượt");

                                    return _SubRep;
                                }
                            }

                            //LIMIT CẢ ĐỜI
                            if (_ItemLimit == LimitType.LimitCount)
                            {
                                PlayerManager.ShowNotification(player, "Tuần này bạn đã mua hết giới hạn của vật phẩm này");

                                return _SubRep;
                            }


                            if (_item.OfficialLevel != -1)
                            {

                                int GuildMoneyNeed = GetGuildMoneyNeed(_item.OfficialLevel);

                                if (!KTGlobal.SubMoney(player, GuildMoneyNeed, MoneyType.GuildMoney, "SHOPBUYITEM").IsOK)
                                {
                                    PlayerManager.ShowNotification(player, "Tài sản cá nhân bang hội không đủ!");

                                    return _SubRep;
                                }

                            }

                            // Nếu vật phẩm yêu cầu vật phẩm thì thực hiện trừ vậ tphaamr
                            if (_item.GoodsIndex != -1)
                            {
                                int NumberRequest = _item.GoodsPrice * Number;

                                if (!ItemManager.RemoveItemFromBag(player, _item.GoodsIndex, NumberRequest, -1, "BUYSHOPITEM"))
                                {
                                    PlayerManager.ShowNotification(player, "Vật phẩm đạo cụ không đủ");

                                    return _SubRep;
                                }
                            }
                            else if (_item.TongFund != -1) // nếu vật phẩm yêu cầu tiền bang hội thì thực hiện trừ tiền bang hội
                            {
                                int Request = _item.TongFund * Number;

                                _SubRep = KTGlobal.SubMoney(player, Request, MoneyType.GuildMoney, "SHOPBUYITEM");
                                // nếu trừ tiền thành công thực hiện ADD ITEM
                                if (!_SubRep.IsOK)
                                {
                                    PlayerManager.ShowNotification(player, "Tích lũy cá nhân không thể sử dụng thêm");
                                    return _SubRep;
                                }
                            }
                            else // Nếu vật phẩm yêu cầu tiền mặt thì sử dụng tiền mặt để mua
                            {
                                _SubRep = KTGlobal.SubMoney(player, TotalMoneyNeed, Money, "SHOPBUYITEM");
                                // nếu trừ tiền thành công thực hiện ADD ITEM
                                if (!_SubRep.IsOK)
                                {
                                    PlayerManager.ShowNotification(player, "Bạc mang theo không đủ");
                                    return _SubRep;
                                }
                                if (ItUsingTicket)
                                {
                                    GoodsData FindGoldById = Global.GetGoodsByDbID(player, couponID);

                                    ItemManager.RemoveItemByCount(player, FindGoldById, 1, "SHOPTICKETDISCOUNT");

                                    LogManager.WriteLog(LogTypes.BuyNpc, "[" + player.RoleID + "][" + player.RoleName + "] Using Discount :" + FindGoldById.GoodsID);
                                }
                            }

                            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;

                            int BILLING = 1;
                            if (Money == MoneyType.Dong)
                            {
                                BILLING = 0;
                            }

                            string TimeUsing = Global.ConstGoodsEndTime;
                            if (_item.Expiry != -1)
                            {
                                DateTime dt = DateTime.Now.AddMinutes(_item.Expiry);

                                // "1900-01-01 12:00:00";
                                TimeUsing = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            // Tọa vật phẩm đã mua
                            if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, _item.ItemID, TotalBuyNumber, 0, "SHOPGETITEM", true, BILLING, false, TimeUsing, "", _item.Series, "", 0, 1, NeedWriterLogs))
                            {
                                PlayerManager.ShowNotification(player, "Có lỗi khi thực hiện thêm vật phẩm vào túi đồ vui lòng liên hệ ADM để được giúp đỡ!");

                                LogManager.WriteLog(LogTypes.BuyNpc, "Có lỗi khi thực hiện add vật phẩm :" + player.RoleID + "|" + _item.ItemID + " x " + TotalBuyNumber);
                            }
                            else
                            {
                                if (Money == MoneyType.Dong)
                                {
                                    LogManager.WriteLog(LogTypes.BuyNpc, "[SHOPKNB][" + player.RoleID + "][" + player.RoleName + "]Mua vật phẩm thành công :" + _item.ItemID + " x " + TotalBuyNumber + "==> tiêu tốn " + TotalMoneyNeed);
                                }

                                //TODO : Ghi lại nhật ký tích nạp tích tiêu
                                PlayerManager.ShowNotification(player, "Mua vật phẩm thành công!");
                            }
                            // Đánh dấu vào DB đã mua vật phẩm này
                            if (_ItemLimit == LimitType.BuyCountPerDay)
                            {
                                int dayID = TimeUtil.NowDateTime().DayOfYear;
                                int Count = TempCount + TotalBuyNumber;
                                int GooldID = ShopItemID;
                                int RoleID = player.RoleID;

                                string SendToDB = string.Format("{0}:{1}:{2}:{3}", RoleID, GooldID, dayID, Count);

                                Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATELIMITGOODSUSEDNUM, SendToDB, player.ServerId);
                            }
                            // Ghi lại tích lũy mua tuần
                            if (_ItemLimit == LimitType.BuyCountPerWeek)
                            {
                                int TotalBuyInWeek = player.GetValueOfWeekRecore(ShopItemID);

                                if (TotalBuyInWeek == -1)
                                {
                                    TotalBuyInWeek = 0;
                                }
                                player.SetValueOfWeekRecore(ShopItemID, TotalBuyInWeek + 1);
                            }
                        }
                    }
                    else
                    {
                        PlayerManager.ShowNotification(player, "Vật phẩm cần tìm không tồn tại");
                    }
                }
                else
                {
                    if (ShopID == 9999)
                    {
                        var findOldItem = player.GetGoodAreadySellBefore().ToList().Where(x => x.Id == ShopItemID).FirstOrDefault();
                        // nếu tìm thấy vật phẩm đã bán trước đó
                        if (findOldItem != null)
                        {
                            if (!KTGlobal.IsHaveSpace(1, player))
                            {
                                PlayerManager.ShowNotification(player, "Túi đồ không đủ chỗ trống! Cần " + 1 + " vị trí trống để có thể mua vật phẩm này!");

                                return _SubRep;
                            }

                            /// Giá mua lại sẽ = giá đã bán ra
                            int price = ItemManager.GetItemTemplate(findOldItem.GoodsID).Price;

                            /// Nếu đồ khóa thì mua lại sẽ cần bạc khóa, đồ không khóa mua lại sẽ cần bạc thường
                            MoneyType Type = findOldItem.Binding == 1 ? MoneyType.BacKhoa : MoneyType.Bac;

                            if (!KTGlobal.IsHaveMoney(player, price, Type))
                            {
                                PlayerManager.ShowNotification(player, "Tiền yêu cầu trên người không đủ!");
                            }
                            else
                            {
                                _SubRep = KTGlobal.SubMoney(player, price, Type, "BUYBACK");

                                _SubRep.IsBuyBack = true;
                                // nếu trừ tiền thành công thực hiện ADD ITEM
                                if (_SubRep.IsOK)
                                {
                                    if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, findOldItem.GoodsID, 1, 0, "BUYBACK", true, findOldItem.Binding, false, Global.ConstGoodsEndTime, findOldItem.Props, findOldItem.Series))
                                    {
                                        PlayerManager.ShowNotification(player, "Có lỗi khi mua lại vui lòng liên hệ với ADMIN để được giúp đỡ!");

                                        LogManager.WriteLog(LogTypes.BuyNpc, "Có lỗi khi update lại số lượng :" + player.RoleID + "|" + findOldItem.Id + " x " + 1);
                                    }
                                    else
                                    {
                                        player.RemoveItemSell(findOldItem);
                                        //TODO : Ghi lại nhật ký tích nạp tích tiêu
                                        PlayerManager.ShowNotification(player, "Mua vật phẩm thành công!");
                                    }
                                }
                                else
                                {
                                    PlayerManager.ShowNotification(player, "Trừ tiền không thành công!");
                                }
                            }
                        }
                        else
                        {
                            PlayerManager.ShowNotification(player, "Vật phẩm không còn tồn tại!");
                        }
                    }

                    // PlayerManager.ShowNotification(player, "Không tìm thấy Cửa hàng tương ứng");
                }
            }
            catch
            {
                PlayerManager.ShowNotification(player, "Có lỗi khi mua vật phẩm này");
                return _SubRep;
            }

            return _SubRep;
        }
    }
}