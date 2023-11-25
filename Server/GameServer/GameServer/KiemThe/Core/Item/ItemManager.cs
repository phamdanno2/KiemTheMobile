using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.LuaSystem;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{
    public class ItemManager
    {
        public static Dictionary<int, ActiveByItem> g_anEquipActive = new Dictionary<int, ActiveByItem>(); // KEY CẦN 2 VALUE ĐỂ KÍCH HOẠT

        public static ItemValueCaculation _Calutaion = new ItemValueCaculation();

        public static List<ExpBookLoading> _TotalExpBook = new List<ExpBookLoading>();

        public static List<SingNetExp> _TotalSingNetExp = new List<SingNetExp>();

        // public static Dictionary<int, ActiveByItem> g_anEquipActived = new Dictionary<int, ActiveByItem>(); // KEY SẼ KÍCH HOẠT CHO 2 VALUE

        public static int[] g_anEquipPos = { -1, (int) KE_EQUIP_POSITION.emEQUIPPOS_WEAPON, (int) KE_EQUIP_POSITION.emEQUIPPOS_WEAPON, (int) KE_EQUIP_POSITION.emEQUIPPOS_BODY, (int) KE_EQUIP_POSITION.emEQUIPPOS_RING, (int) KE_EQUIP_POSITION.emEQUIPPOS_NECKLACE, (int) KE_EQUIP_POSITION.emEQUIPPOS_AMULET, (int) KE_EQUIP_POSITION.emEQUIPPOS_FOOT, (int) KE_EQUIP_POSITION.emEQUIPPOS_BELT, (int) KE_EQUIP_POSITION.emEQUIPPOS_HEAD, (int) KE_EQUIP_POSITION.emEQUIPPOS_CUFF, (int) KE_EQUIP_POSITION.emEQUIPPOS_PENDANT, (int) KE_EQUIP_POSITION.emEQUIPPOS_HORSE, (int) KE_EQUIP_POSITION.emEQUIPPOS_MASK, (int) KE_EQUIP_POSITION.emEQUIPPOS_BOOK, (int) KE_EQUIP_POSITION.emEQUIPPOS_ZHEN, (int) KE_EQUIP_POSITION.emEQUIPPOS_SIGNET, (int) KE_EQUIP_POSITION.emEQUIPPOS_MANTLE, (int) KE_EQUIP_POSITION.emEQUIPPOS_CHOP };

        public static int[] g_anEquipSubPos = { -1, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_WEAPON, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_WEAPON, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_BODY, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_RING, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_NECKLACE, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_AMULET, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_FOOT, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_BELT, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_HEAD, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_CUFF, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_PENDANT, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_HORSE, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_MASK, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_BOOK, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_ZHEN, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_SIGNET, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_MANTLE, (int) KE_EQUIP_POSITION_SUB.emEQUIPPOS_CHOP };

        /// <summary>
        /// TOTAL BASIC ITEM LOAD
        /// </summary>
        public static string BasicItem_amulet = "Config/KT_Item/BasicItem/amulet.xml";

        public static string BasicItem_armor = "Config/KT_Item/BasicItem/armor.xml";
        public static string BasicItem_belt = "Config/KT_Item/BasicItem/belt.xml";
        public static string BasicItem_book = "Config/KT_Item/BasicItem/book.xml";
        public static string BasicItem_boots = "Config/KT_Item/BasicItem/boots.xml";
        public static string BasicItem_chop = "Config/KT_Item/BasicItem/chop.xml";
        public static string BasicItem_cuff = "Config/KT_Item/BasicItem/cuff.xml";
        public static string BasicItem_helm = "Config/KT_Item/BasicItem/helm.xml";
        public static string BasicItem_horse = "Config/KT_Item/BasicItem/horse.xml";
        public static string BasicItem_mantle = "Config/KT_Item/BasicItem/mantle.xml";
        public static string BasicItem_meleeweapon = "Config/KT_Item/BasicItem/meleeweapon.xml";
        public static string BasicItem_necklace = "Config/KT_Item/BasicItem/necklace.xml";
        public static string BasicItem_pendant = "Config/KT_Item/BasicItem/pendant.xml";
        public static string BasicItem_rangeweapon = "Config/KT_Item/BasicItem/rangeweapon.xml";
        public static string BasicItem_ring = "Config/KT_Item/BasicItem/ring.xml";
        public static string BasicItem_signet = "Config/KT_Item/BasicItem/signet.xml";
        public static string BasicItem_zhen = "Config/KT_Item/BasicItem/zhen.xml";

        /// <summary>
        /// TOTAL ITEM CRAFTING LOAD
        /// </summary>
        public static string Crafting_amulet = "Config/KT_Item/Crafting/amulet.xml";

        public static string Crafting_armor = "Config/KT_Item/Crafting/armor.xml";
        public static string Crafting_belt = "Config/KT_Item/Crafting/belt.xml";
        public static string Crafting_boots = "Config/KT_Item/Crafting/boots.xml";
        public static string Crafting_cuff = "Config/KT_Item/Crafting/cuff.xml";
        public static string Crafting_helm = "Config/KT_Item/Crafting/helm.xml";
        public static string Crafting_meleeweapon = "Config/KT_Item/Crafting/meleeweapon.xml";
        public static string Crafting_necklace = "Config/KT_Item/Crafting/necklace.xml";
        public static string Crafting_pendant = "Config/KT_Item/Crafting/pendant.xml";
        public static string Crafting_rangeweapon = "Config/KT_Item/Crafting/rangeweapon.xml";
        public static string Crafting_ring = "Config/KT_Item/Crafting/ring.xml";

        /// <summary>
        /// LOAD SET ITEM
        /// </summary>
        public static string SetItem_greenequip = "Config/KT_Item/SetItem/greenequip.xml";

        /// <summary>
        /// BookEXP
        /// </summary>
        public static string Book_ExpPath = "Config/KT_Item/ItemExp/BookExp.xml";

        public static string SignetExpPath = "Config/KT_Item/ItemExp/SignnetExp.xml";

        /// <summary>
        /// LOAD OTHER ITEM
        /// </summary>
        public static string Other_medicine = "Config/KT_Item/Other/medicine.xml";

        public static string Other_scriptitem = "Config/KT_Item/Other/scriptitem.xml";
        public static string Other_stuffitem = "Config/KT_Item/Other/stuffitem.xml";
        public static string Other_taskquest = "Config/KT_Item/Other/taskquest.xml";

        /// <summary>
        /// Load config action Hidden Efffect | Green Efffect | Active SET
        /// </summary>
        public static string MagicAttribLevel = "Config/KT_Item/MagicAttribLevel.xml";

        public static string SuiteActiveProp = "Config/KT_Item/SuiteActiveProp.xml";

        public static string ItemCacluation = "Config/KT_Item/ItemValueCaculation.xml";

        public static List<MagicAttribLevel> TotalMagicAttribLevel = new List<MagicAttribLevel>();

        public static List<SuiteActiveProp> TotalSuiteActiveProp = new List<SuiteActiveProp>();

        public static List<ItemData> TotalItem = new List<ItemData>();



        public static Dictionary<int, ItemData> _TotalGameItem { get; private set; } = new Dictionary<int, ItemData>();




        /// <summary>
        /// Danh sách ngũ hành ấn
        /// </summary>
        public static List<ItemData> Signets { get; private set; } = new List<ItemData>();

        /// <summary>
        /// Danh sách phi phong
        /// </summary>
        public static List<ItemData> Mantles { get; private set; } = new List<ItemData>();

        /// <summary>
        /// Danh sách quan ấn
        /// </summary>
        public static List<ItemData> Chops { get; private set; } = new List<ItemData>();


        /// <summary>
        /// Trả về cấp độ kích hoạt của trận pháp tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemZhen"></param>
        /// <param name="requireStayNearby"></param>
        /// <returns></returns>
        public static int GetZhenLevel(KPlayer player, ItemData itemZhen, bool requireStayNearby)
        {
            int level = 1;

            /// Quy tắc hoạt trận
            switch (itemZhen.ZhenActivateRule)
            {
                /// Ngũ hành
                case 1:
                {
                    /// Danh sách ngũ hành có
                    HashSet<KE_SERIES_TYPE> markupSeries = new HashSet<KE_SERIES_TYPE>();
                    /// Duyệt danh sách thành viên
                    foreach (KPlayer teammate in player.Teammates)
                    {
                        /// Nếu không cùng bản đồ
                        if (teammate.CurrentMapCode != player.CurrentMapCode || teammate.CurrentCopyMapID != player.CurrentCopyMapID)
                        {
                            continue;
                        }
                        /// Nếu yêu cầu khoảng cách và đội viên này lại nằm ngoài khoảng cách
                        if (requireStayNearby && KTGlobal.GetDistanceBetweenPlayers(player, teammate) > 600)
                        {
                            continue;
                        }
                        /// Nếu chưa có trong danh sách ngũ hành thì thêm vào
                        if (!markupSeries.Contains(teammate.m_Series))
                        {
                            markupSeries.Add(teammate.m_Series);
                        }
                    }
                    /// Cấp độ của trận là tổng số ngũ hành trong nhóm có
                    level = markupSeries.Count;
                    break;
                }
                /// Cấp 69
                case 2:
                {
                    /// Tổng số thành viên thỏa mãn
                    int count = 0;
                    /// Duyệt danh sách thành viên
                    foreach (KPlayer teammate in player.Teammates)
                    {
                        /// Nếu không cùng bản đồ
                        if (teammate.CurrentMapCode != player.CurrentMapCode || teammate.CurrentCopyMapID != player.CurrentCopyMapID)
                        {
                            continue;
                        }
                        /// Nếu yêu cầu khoảng cách và đội viên này lại nằm ngoài khoảng cách
                        if (requireStayNearby && KTGlobal.GetDistanceBetweenPlayers(player, teammate) > 600)
                        {
                            continue;
                        }
                        /// Nếu trên cấp 69
                        if (teammate.m_Level >= 69)
                        {
                            count++;
                        }
                    }
                    /// Cấp độ của trận là tổng số thành viên cấp độ lớn hơn 69
                    level = count;
                    break;
                }
                /// Cấp 100
                case 3:
                {
                    /// Tổng số thành viên thỏa mãn
                    int count = 0;
                    /// Duyệt danh sách thành viên
                    foreach (KPlayer teammate in player.Teammates)
                    {
                        /// Nếu không cùng bản đồ
                        if (teammate.CurrentMapCode != player.CurrentMapCode || teammate.CurrentCopyMapID != player.CurrentCopyMapID)
                        {
                            continue;
                        }
                        /// Nếu yêu cầu khoảng cách và đội viên này lại nằm ngoài khoảng cách
                        if (requireStayNearby && KTGlobal.GetDistanceBetweenPlayers(player, teammate) > 600)
                        {
                            continue;
                        }
                        /// Nếu trên cấp 100
                        if (teammate.m_Level >= 100)
                        {
                            count++;
                        }
                    }
                    /// Cấp độ của trận là tổng số thành viên cấp độ lớn hơn 100
                    level = count;
                    break;
                }
                default:
                {
                    break;
                }
            }

            /// Nếu vượt quá cấp độ
            if (level > 5)
            {
                level = 5;
            }

            return level;
        }

        /// <summary>
        /// Kiểm tra có phải trang bị không
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static bool IsEquip(GoodsData itemGD)
        {
            /// Nếu vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                return false;
            }

            return ItemManager.KD_ISEQUIP(itemData.Genre);
        }

        /// <summary>
        /// Hàm check xme có phải trang bị hay không
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static bool KD_ISEQUIP(int general)
        {
            if (general >= (int) KE_ITEM_GENRE.item_equip_general && general <= (int) KE_ITEM_GENRE.item_equip_green)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra trang bị tương ứng có cường hóa được không
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        public static bool CanEquipBeEnhance(ItemData itemData)
        {
            return itemData.DetailType <= 11;
        }

        /// <summary>
        /// Hàm check xem có phải thuộc tính bộ hay không
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static bool KD_ISSUITE(int general)
        {
            if (general == (int) KE_ITEM_GENRE.item_equip_BoundMoney || general == (int) KE_ITEM_GENRE.item_equip_green)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm check xem có phải là vũ khí hay không
        /// </summary>
        /// <param name="DetailType"></param>
        /// <returns></returns>
        public static bool KD_ISWEAPON(int DetailType)
        {
            if (DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_meleeweapon || DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_rangeweapon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KD_ISORNAMENT(int DetailType)
        {
            if (DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_ring || DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_necklace || DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_amulet || DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_pendant)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KD_ISSIGNET(int DetailType)
        {
            if (DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_signet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  Nếu là kim tê
        /// </summary>
        /// <param name="DetailType"></param>
        /// <returns></returns>
        public static bool KD_ISJINXI(int ItemID)
        {
            if (ItemID == 195 || ItemID == 196 || ItemID == 197 || ItemID == 198 || ItemID == 199)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsQianKunFu(int ItemID)
        {
            if (ItemID == 344 || ItemID == 354)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KD_ISZHEN(int DetailType)
        {
            if (DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_zhen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KD_ISBOOK(int DetailType)
        {
            if (DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_book)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static KE_ITEM_EQUIP_DETAILTYPE GetItemType(int DetailType)
        {
            return (KE_ITEM_EQUIP_DETAILTYPE) DetailType;
        }

        public static void LoadItemFromPath(string FilesPath)
        {
            string Files = Global.GameResPath(FilesPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<ItemData>));
                List<ItemData> _Item = serializer.Deserialize(stream) as List<ItemData>;
                TotalItem.AddRange(_Item);
            }
        }

        #region SignNetManager

        public static void LoadSigNetXml(string FilesPath)
        {
            string Files = Global.GameResPath(FilesPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<SingNetExp>));

                _TotalSingNetExp = serializer.Deserialize(stream) as List<SingNetExp>;
            }
        }

        #endregion SignNetManager

        #region BookManager

        /// <summary>
        /// Lấy ra exp của sách
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="BookType"></param>
        /// <returns></returns>
        public static int GetMaxbookEXP(int Level, int BookType)
        {
            int Exp = 0;

            var find = _TotalExpBook.Where(x => x.BookLevel == BookType && x.Level == Level).FirstOrDefault();
            if (find != null)
            {
                Exp = find.Exp;
            }

            return Exp;
        }

        /// <summary>
        /// Hàm trả về Exp nhận được của mật tịch khi giết 1 con quái
        /// </summary>
        /// <param name="ExpInput"></param>
        /// <returns></returns>
        public static int GetEXPEarnPerExp(int ExpInput)
        {
            return (4 * ExpInput) / 100;
        }

        public static void LoadingBookExp(string FilesPath)
        {
            string Files = Global.GameResPath(FilesPath);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<ExpBookLoading>));

                _TotalExpBook = serializer.Deserialize(stream) as List<ExpBookLoading>;
            }
        }

        #endregion BookManager

        /// <summary>
        /// Trả về tên loại vũ khí
        /// </summary>
        /// <param name="weaponKind"></param>
        /// <returns></returns>
        public static string GetWeaponKind(int weaponKind)
        {
            switch (weaponKind)
            {
                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_HAND:
                    return "Triền thủ";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_SWORD:
                    return "Kiếm";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_KNIFE:
                    return "Đao";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_STICK:
                    return "Côn";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_SPEAR:
                    return "Thương";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_HAMMER:
                    return "Chùy";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_FLYBAR:
                    return "Phi đao";

                case (int) KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_ARROW:
                    return "Tụ tiễn";

                default:
                    return null;
            }
        }

        public static void ActiveConfig()
        {
            //emEQUIPPOS_HEAD
            ActiveByItem _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BODY;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_NECKLACE;
            g_anEquipActive.Add(0, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_RING;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BELT;
            g_anEquipActive.Add(1, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_PENDANT;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_CUFF;
            g_anEquipActive.Add(2, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_NECKLACE;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BODY;
            g_anEquipActive.Add(3, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_WEAPON;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_HEAD;
            g_anEquipActive.Add(4, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_FOOT;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_AMULET;
            g_anEquipActive.Add(5, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_HEAD;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_WEAPON;
            g_anEquipActive.Add(6, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_CUFF;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_PENDANT;
            g_anEquipActive.Add(7, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BELT;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_RING;
            g_anEquipActive.Add(8, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_AMULET;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_FOOT;
            g_anEquipActive.Add(9, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_HORSE;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_HORSE;
            g_anEquipActive.Add(10, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_MASK;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_MASK;
            g_anEquipActive.Add(11, _Active);

            _Active = new ActiveByItem();
            _Active.Pos1 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BOOK;
            _Active.Pos2 = (int) KE_EQUIP_POSITION.emEQUIPPOS_BOOK;
            g_anEquipActive.Add(12, _Active);

            // END KÍCH HOẠT

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_AMULET;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_FOOT;
            //g_anEquipActived.Add(0, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_HEAD;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_WEAPON;
            //g_anEquipActived.Add(1, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BODY;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_NECKLACE;
            //g_anEquipActived.Add(2, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_FOOT;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_AMULET;
            //g_anEquipActived.Add(3, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_CUFF;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_PENDANT;
            //g_anEquipActived.Add(4, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BELT;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_RING;
            //g_anEquipActived.Add(5, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_PENDANT;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_CUFF;
            //g_anEquipActived.Add(6, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_NECKLACE;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BODY;
            //g_anEquipActived.Add(7, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_WEAPON;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_HEAD;
            //g_anEquipActived.Add(8, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_RING;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BELT;
            //g_anEquipActived.Add(9, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_HORSE;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_HORSE;
            //g_anEquipActived.Add(10, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_MASK;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_MASK;
            //g_anEquipActived.Add(11, _Active);

            //_Active = new ActiveByItem();
            //_Active.Pos1 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BOOK;
            //_Active.Pos2 = (int)KE_EQUIP_POSITION.emEQUIPPOS_BOOK;
            //g_anEquipActived.Add(12, _Active);

            //Console.WriteLine("Toal Active ItemRule : " + g_anEquipActived.Count);
        }

        public static void ItemSetup()
        {
            LoadSigNetXml(SignetExpPath);
            // LOAD BASSIC ITEM
            LoadingBookExp(Book_ExpPath);
            //Console.WriteLine("Loading Basic Amulet...");
            LoadItemFromPath(BasicItem_amulet);
            //Console.WriteLine("Loading Basic Armor...");
            LoadItemFromPath(BasicItem_armor);
            //Console.WriteLine("Loading Basic Belt...");
            LoadItemFromPath(BasicItem_belt);
            //Console.WriteLine("Loading Basic Book...");
            LoadItemFromPath(BasicItem_book);
            //Console.WriteLine("Loading Basic Boots...");
            LoadItemFromPath(BasicItem_boots);
            //Console.WriteLine("Loading Basic Chop...");
            LoadItemFromPath(BasicItem_chop);
            //Console.WriteLine("Loading Basic Cuff...");
            LoadItemFromPath(BasicItem_cuff);
            //Console.WriteLine("Loading Basic Helm...");
            LoadItemFromPath(BasicItem_helm);
            //Console.WriteLine("Loading Basic Horse...");
            LoadItemFromPath(BasicItem_horse);
            //Console.WriteLine("Loading Basic Mantle...");
            LoadItemFromPath(BasicItem_mantle);
            //Console.WriteLine("Loading Basic Meleeweapon...");
            LoadItemFromPath(BasicItem_meleeweapon);
            //Console.WriteLine("Loading Basic Necklace...");
            LoadItemFromPath(BasicItem_necklace);
            //Console.WriteLine("Loading Basic Pendant...");
            LoadItemFromPath(BasicItem_pendant);
            //Console.WriteLine("Loading Basic Rangeweapon...");
            LoadItemFromPath(BasicItem_rangeweapon);
            //Console.WriteLine("Loading Basic Ring...");
            LoadItemFromPath(BasicItem_ring);
            //Console.WriteLine("Loading Basic Signet...");
            LoadItemFromPath(BasicItem_signet);
            //Console.WriteLine("Loading Basic Zhen...");
            LoadItemFromPath(BasicItem_zhen);

            //Console.WriteLine("Loading Crafting Amulet...");
            ///Loadding All Crafting Item
            LoadItemFromPath(Crafting_amulet);

            //Console.WriteLine("Loading Crafting Armor...");
            LoadItemFromPath(Crafting_armor);

            //Console.WriteLine("Loading Crafting Belt...");
            LoadItemFromPath(Crafting_belt);

            //Console.WriteLine("Loading Crafting Boots...");
            LoadItemFromPath(Crafting_boots);

            //Console.WriteLine("Loading Crafting Cuff...");
            LoadItemFromPath(Crafting_cuff);

            //Console.WriteLine("Loading Crafting Helm...");
            LoadItemFromPath(Crafting_helm);

            //Console.WriteLine("Loading Crafting Meleeweapon...");
            LoadItemFromPath(Crafting_meleeweapon);

            //Console.WriteLine("Loading Crafting Necklace...");
            LoadItemFromPath(Crafting_necklace);

            //Console.WriteLine("Loading Crafting Pendant...");
            LoadItemFromPath(Crafting_pendant);

            //Console.WriteLine("Loading Crafting Rangeweapon...");
            LoadItemFromPath(Crafting_rangeweapon);

            //Console.WriteLine("Loading Crafting Ring...");
            LoadItemFromPath(Crafting_ring);

            /// Load ITEM SET
            //Console.WriteLine("Loading SetItem Green Equip...");
            LoadItemFromPath(SetItem_greenequip);

            // LOAD Other Item
            //Console.WriteLine("Loading Other Medicine...");
            LoadItemFromPath(Other_medicine);
            //Console.WriteLine("Loading Script Item...");
            LoadItemFromPath(Other_scriptitem);
            //Console.WriteLine("Loading Stuff Item...");
            LoadItemFromPath(Other_stuffitem);
            //Console.WriteLine("Loading Task Quest...");
            LoadItemFromPath(Other_taskquest);

            string Files = Global.GameResPath(MagicAttribLevel);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<MagicAttribLevel>));
                TotalMagicAttribLevel = serializer.Deserialize(stream) as List<MagicAttribLevel>;

                //Console.WriteLine("Loading TotalMagicAttribLevel Done : " + TotalMagicAttribLevel.Count);
            }

            Files = Global.GameResPath(SuiteActiveProp);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<SuiteActiveProp>));
                TotalSuiteActiveProp = serializer.Deserialize(stream) as List<SuiteActiveProp>;

                //Console.WriteLine("Loading TotalSuiteActiveProp Done : " + TotalSuiteActiveProp.Count);
            }

            _TotalGameItem = TotalItem.ToDictionary(x => x.ItemID, x => x);

            //Console.WriteLine("Total Item Loading: " + _TotalGameItem.Count);

            ActiveConfig();

            Files = Global.GameResPath(ItemCacluation);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(ItemValueCaculation));

                _Calutaion = serializer.Deserialize(stream) as ItemValueCaculation;

                //Console.WriteLine("Loading Item Calculation DONE ");
            }


            /// Làm rỗng danh sách đặc biệt
            ItemManager.Mantles.Clear();
            ItemManager.Signets.Clear();
            ItemManager.Chops.Clear();
            /// Duyệt danh sách vật phẩm
            foreach (ItemData itemData in ItemManager.TotalItem)
            {
                /// Nếu đây là ngũ hành ấn
                if (itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_signet)
                {
                    ItemManager.Signets.Add(itemData);
                }
                /// Nếu đây là phi phong
                else if (itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_mantle)
                {
                    ItemManager.Mantles.Add(itemData);
                }
                /// Nếu đây là quan ấn
                else if (itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_chop)
                {
                    ItemManager.Chops.Add(itemData);
                }
            }
        }

        /// <summary>
        /// Kiểm tra vật phẩm có bán được không
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static bool IsCanBeSold(GoodsData itemGD)
        {
            /// Nếu thông tin vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                return itemData.BindType < 3;
            }
            /// Nếu là trang bị và có cường hóa
            if (ItemManager.IsEquip(itemGD) && itemGD.Forge_level > 0)
            {
                return false;
            }
            /// Trả về không thể bán
            return false;
        }

        public static ItemData GetItemTemplate(int ItemID)
        {
            if (_TotalGameItem.ContainsKey(ItemID))
            {
                return _TotalGameItem[ItemID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tính giá trị của vật phẩm
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static StarLevelStruct ItemValueCalculation(GoodsData itemGD, out long ItemValueTaiPhu)
        {
            ItemValueTaiPhu = 0;
            /// Nếu không phải trang bị
            if (!ItemManager.IsEquip(itemGD))
            {
                return null;
            }
            int LevelCuongHoa = itemGD.Forge_level;
            int LevelLuyenHoa = 0;

            ItemData _ItemData = null;
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out _ItemData))
            {
                return null;
            }

            double TotalValue = _ItemData.ItemValue;

            /// Nếu vật phẩm có thể cường hóa
            if (CanEquipBeEnhance(_ItemData))
            {
                double nLevelRate = 0;
                double nTypeRate = 0;

                double nEnhValue = 0;
                double nStrValue = 0;

                Equip_Type_Rate TypeRate = _Calutaion.List_Equip_Type_Rate.Where(x => (int) x.EquipType == _ItemData.DetailType).FirstOrDefault();
                if (TypeRate != null)
                {
                    nTypeRate = TypeRate.Value;
                }

                Equip_Level LevelRate = _Calutaion.List_Equip_Level.Where(x => x.Level == _ItemData.Level).FirstOrDefault();
                if (LevelRate != null)
                {
                    nLevelRate = LevelRate.Value;
                }

                for (int i = 0; i <= LevelCuongHoa; i++)
                {
                    Enhance_Value _Enhance_Value = _Calutaion.List_Enhance_Value.Where(x => x.EnhanceTimes == i).FirstOrDefault();
                    if (_Enhance_Value != null)
                    {
                        nEnhValue = nEnhValue + _Enhance_Value.Value;
                    }
                }

                if (LevelLuyenHoa > 0)
                {
                    Strengthen_Value _Strengthen_Value = _Calutaion.List_Strengthen_Value.Where(x => x.StrengthenTimes == LevelLuyenHoa).FirstOrDefault();
                    if (_Strengthen_Value != null)
                    {
                        nStrValue = _Strengthen_Value.Value;
                    }
                }

                List<PropMagic> GreenProp = _ItemData.GreenProp;

                List<PropMagic> HiddenProb = _ItemData.HiddenProp;

                Dictionary<int, double> tbValue = new Dictionary<int, double>();

                List<PropMagic> TotalProbs = new List<PropMagic>();

                if (GreenProp != null)
                {
                    TotalProbs.AddRange(GreenProp);
                }
                if (HiddenProb != null)
                {
                    TotalProbs.AddRange(HiddenProb);
                }

                int MagicCount = 1;

                foreach (PropMagic _probs in TotalProbs)
                {
                    double Rate = 100;

                    Equip_Random_Pos _Rate = _Calutaion.List_Equip_Random_Pos.Where(x => x.MAGIC_POS == MagicCount).FirstOrDefault();
                    if (_Rate != null)
                    {
                        Rate = (double) _Rate.Value / 100;
                    }

                    MagicAttribLevel _Atribute = TotalMagicAttribLevel.Where(x => x.MagicName == _probs.MagicName && x.Level == _probs.MagicLevel).FirstOrDefault();
                    if (_Atribute != null)
                    {
                        int Value = _Atribute.ItemValue;

                        double FinalValue = Math.Floor(Rate * Value);

                        tbValue.Add(MagicCount, FinalValue);

                        TotalValue += FinalValue;
                    }
                    else
                    {
                        //Console.WriteLine("TOANG");
                    }

                    MagicCount++;
                }

                for (int i = 1; i <= TotalProbs.Count; i++)
                {
                    PropMagic SourceProb = TotalProbs[i - 1];

                    for (int j = 1; j <= TotalProbs.Count; j++)
                    {
                        PropMagic DescProb = TotalProbs[j - 1];

                        MagicSource _FindMagicSource = _Calutaion.Magic_Combine_Def.MagicSourceDef.Where(x => x.MagicName == SourceProb.MagicName).FirstOrDefault();

                        if (_FindMagicSource != null)
                        {
                            int SelectValue = _FindMagicSource.Index;

                            MagicDesc _FindMagicDest = _Calutaion.Magic_Combine_Def.MagicDescDef.Where(x => x.MagicName == DescProb.MagicName).FirstOrDefault();
                            if (_FindMagicDest != null)
                            {
                                try
                                {
                                    if (_FindMagicDest.ListValue.Count() > SelectValue)
                                    {
                                        //Console.WriteLine("CHECK : " + SourceProb.MagicName + "===>" + DescProb.MagicName);

                                        double Value = _FindMagicDest.ListValue[SelectValue];

                                        double nRate = Math.Sqrt(Value) / 10;

                                        nRate = (nRate - 1) * SourceProb.MagicLevel * DescProb.MagicLevel / 400;

                                        double FinalValue = Math.Floor((tbValue[i] + tbValue[j]) * nRate);

                                        //  Console.WriteLine("FINAL VALUE :" + FinalValue);

                                        TotalValue += FinalValue;
                                    }
                                }
                                catch (Exception exx)
                                {
                                    // Console.WriteLine(exx.ToString());
                                }
                            }
                        }
                    }
                }

                TotalValue = Math.Floor(TotalValue / 100 * nLevelRate);

                TotalValue = Math.Floor(TotalValue / 100 * nTypeRate);

                TotalValue = TotalValue + Math.Floor(nEnhValue / 100 * nTypeRate);

                TotalValue = TotalValue + Math.Floor(nStrValue / 100 * nTypeRate);

                //  Console.WriteLine("FINAL TAI PHU :" + TotalValue);
            }
            /// Nếu là Ngũ Hành Ấn
            else if (ItemManager.KD_ISSIGNET(_ItemData.DetailType))
            {
                try
                {
                    /// Cường hóa ngũ hành tương khắc
                    {
                        string[] param = itemGD.OtherParams[ItemPramenter.Pram_1].Split('|');
                        int seriesEnhance = int.Parse(param[0]);
                        int seriesEnhanceExp = int.Parse(param[1]);
                        /// Tăng tài phú tương ứng
                        TotalValue += ItemManager._TotalSingNetExp[seriesEnhance - 1].Value;
                        /// Cấp tiếp theo
                       // int nextLevel = seriesEnhance + 1;
                        /// Thông tin cấp kế
                        //SingNetExp nextLevelInfo = ItemManager._TotalSingNetExp.Where(x => x.Level == nextLevel).FirstOrDefault();
                        ///// Nếu cấp tiếp theo tồn tại
                        //if (nextLevelInfo != null)
                        //{
                        //    /// Độ lệch Exp
                        //    int subtract = nextLevelInfo.Value - ItemManager._TotalSingNetExp[seriesEnhance].Value;
                        //    /// Phần trăm kinh nghiệm hiện tại
                        //    float percent = seriesEnhanceExp / ItemManager._TotalSingNetExp[seriesEnhance].UpgardeExp;
                        //    /// Tăng lượng tài phú cộng thêm
                        //    TotalValue += subtract * percent;
                        //}
                    }
                    /// Nhược hóa ngũ hành tương khắc
                    {
                        string[] param = itemGD.OtherParams[ItemPramenter.Pram_2].Split('|');
                        int seruesConque = int.Parse(param[0]);
                        int seriesConqueExp = int.Parse(param[1]);
                        /// Tăng tài phú tương ứng
                        TotalValue += ItemManager._TotalSingNetExp[seruesConque - 1].Value;
                        ///// Cấp tiếp theo
                        //int nextLevel = seruesConque + 1;
                        ///// Thông tin cấp kế
                        //SingNetExp nextLevelInfo = ItemManager._TotalSingNetExp.Where(x => x.Level == nextLevel).FirstOrDefault();
                        ///// Nếu cấp tiếp theo tồn tại
                        //if (nextLevelInfo != null)
                        //{
                        //    /// Độ lệch Exp
                        //    int subtract = nextLevelInfo.Value - ItemManager._TotalSingNetExp[seruesConque].Value;
                        //    /// Phần trăm kinh nghiệm hiện tại
                        //    float percent = seriesConqueExp / ItemManager._TotalSingNetExp[seruesConque].UpgardeExp;
                        //    /// Tăng lượng tài phú cộng thêm
                        //    TotalValue += subtract * percent;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.ToString());
                }
            }

            ItemValueTaiPhu = (long) TotalValue;

            int ItemType = _ItemData.DetailType;
            int ItemLevel = _ItemData.Level;

            Equip_StarLevel List_Equip_StarLevel = null;
            int LevelStart = 0;

            if (ItemLevel == 1)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_1).LastOrDefault();
            }
            else if (ItemLevel == 2)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_2).LastOrDefault();
            }
            else if (ItemLevel == 3)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_3).LastOrDefault();
            }
            else if (ItemLevel == 4)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_4).LastOrDefault();
            }
            else if (ItemLevel == 5)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_5).LastOrDefault();
            }
            else if (ItemLevel == 6)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_6).LastOrDefault();
            }
            else if (ItemLevel == 7)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_7).LastOrDefault();
            }
            else if (ItemLevel == 8)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_8).LastOrDefault();
            }
            else if (ItemLevel == 9)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_9).LastOrDefault();
            }
            else if (ItemLevel == 10)
            {
                List_Equip_StarLevel = _Calutaion.List_Equip_StarLevel.Where(x => x.EQUIP_DETAIL_TYPE == ItemType && TotalValue >= x.EQUIP_LEVEL_10).LastOrDefault();
            }
            if (List_Equip_StarLevel != null)
            {
                LevelStart = List_Equip_StarLevel.STAR_LEVEL;
            }

            StarLevelStruct _LevelSelect = _Calutaion.List_StarLevelStruct.Where(x => x.StarLevel == LevelStart).FirstOrDefault();
            if (_LevelSelect == null)
            {
                return null;
            }

            _LevelSelect.Value = (long) TotalValue;
            return _LevelSelect;
        }

        /// <summary>
        /// Funtion GenItemData
        /// </summary>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public static string GenerateProbs(ItemData ItemData)
        {
            if (!KD_ISEQUIP(ItemData.Genre))
            {
                return "";
            }
            ItemGenByteData _ItemBuild = new ItemGenByteData();

            // WRITER PROPS
            if (ItemData.ListBasicProp.Count > 0)
            {
                List<BasicProp> List = ItemData.ListBasicProp.OrderBy(x => x.Index).ToList();

                _ItemBuild.BasicPropCount = ItemData.ListBasicProp.Count;
                _ItemBuild.BasicPropValue = new List<int>();

                foreach (BasicProp _Probs in List)
                {
                    if (_Probs.BasicPropPA1Min > 0)
                    {
                        int RandomValue = KTGlobal.GetRandomNumber(_Probs.BasicPropPA1Min, _Probs.BasicPropPA1Max);

                        int DIV = RandomValue - _Probs.BasicPropPA1Min;
                        _ItemBuild.BasicPropValue.Add(DIV);
                    }
                    else
                    {
                        _ItemBuild.BasicPropValue.Add(-1);
                    }
                    if (_Probs.BasicPropPA2Min > 0)
                    {
                        int RandomValue = KTGlobal.GetRandomNumber(_Probs.BasicPropPA2Min, _Probs.BasicPropPA2Max);

                        int DIV = RandomValue - _Probs.BasicPropPA2Min;
                        _ItemBuild.BasicPropValue.Add(DIV);
                    }
                    else
                    {
                        _ItemBuild.BasicPropValue.Add(-1);
                    }
                    if (_Probs.BasicPropPA3Min > 0)
                    {
                        int RandomValue = KTGlobal.GetRandomNumber(_Probs.BasicPropPA3Min, _Probs.BasicPropPA3Max);

                        int DIV = RandomValue - _Probs.BasicPropPA3Min;
                        _ItemBuild.BasicPropValue.Add(DIV);
                    }
                    else
                    {
                        _ItemBuild.BasicPropValue.Add(-1);
                    }
                }
            }
            // WRITER BookProperty
            if (ItemData.BookProperty != null)
            {
                _ItemBuild.HaveBookProperties = true;

                _ItemBuild.BookPropertyValue = new List<int>();

                if (ItemData.BookProperty.StrInitMin > 0)
                {
                    int RandomValue = KTGlobal.GetRandomNumber(ItemData.BookProperty.StrInitMin / 100, ItemData.BookProperty.StrInitMax / 100);

                    int DIV = RandomValue - ItemData.BookProperty.StrInitMin / 100;

                    _ItemBuild.BookPropertyValue.Add(DIV);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.DexInitMin > 0)
                {
                    int RandomValue = KTGlobal.GetRandomNumber(ItemData.BookProperty.DexInitMin / 100, ItemData.BookProperty.DexInitMax / 100);

                    int DIV = RandomValue - ItemData.BookProperty.DexInitMin / 100;

                    _ItemBuild.BookPropertyValue.Add(DIV);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.VitInitMin > 0)
                {
                    int RandomValue = KTGlobal.GetRandomNumber(ItemData.BookProperty.VitInitMin / 100, ItemData.BookProperty.VitInitMax / 100);

                    int DIV = RandomValue - ItemData.BookProperty.VitInitMin / 100;

                    _ItemBuild.BookPropertyValue.Add(DIV);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.EngInitMin > 0)
                {
                    int RandomValue = KTGlobal.GetRandomNumber(ItemData.BookProperty.EngInitMin / 100, ItemData.BookProperty.EngInitMax / 100);

                    int DIV = RandomValue - ItemData.BookProperty.EngInitMin / 100;

                    _ItemBuild.BookPropertyValue.Add(DIV);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.SkillID1 > 0)
                {
                    _ItemBuild.BookPropertyValue.Add(ItemData.BookProperty.SkillID1);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.SkillID2 > 0)
                {
                    _ItemBuild.BookPropertyValue.Add(ItemData.BookProperty.SkillID2);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.SkillID3 > 0)
                {
                    _ItemBuild.BookPropertyValue.Add(ItemData.BookProperty.SkillID3);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }

                if (ItemData.BookProperty.SkillID4 > 0)
                {
                    _ItemBuild.BookPropertyValue.Add(ItemData.BookProperty.SkillID4);
                }
                else
                {
                    _ItemBuild.BookPropertyValue.Add(-1);
                }
            }

            if (ItemData.GreenProp == null)
            {
                _ItemBuild.GreenPropCount = 0;
            }
            else
            {
                _ItemBuild.GreenPropCount = ItemData.GreenProp.Count;

                _ItemBuild.GreenPropValue = new List<int>();

                List<PropMagic> List = ItemData.GreenProp.OrderBy(x => x.Index).ToList();

                foreach (PropMagic _Probs in List)
                {
                    if (_Probs.MagicName.Length > 0)
                    {
                        MagicAttribLevel FindMagic = TotalMagicAttribLevel.Where(x => x.MagicName == _Probs.MagicName && x.Level == _Probs.MagicLevel).FirstOrDefault();

                        if (FindMagic != null)
                        {
                            if (FindMagic.MA1Min > 0)
                            {
                                int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA1Min, FindMagic.MA1Max);

                                int DIV = RandomValue - FindMagic.MA1Min;

                                _ItemBuild.GreenPropValue.Add(DIV);
                            }
                            else
                            {
                                _ItemBuild.GreenPropValue.Add(-1);
                            }

                            if (FindMagic.MA2Min > 0)
                            {
                                int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA2Min, FindMagic.MA2Max);

                                int DIV = RandomValue - FindMagic.MA2Min;

                                _ItemBuild.GreenPropValue.Add(DIV);
                            }
                            else
                            {
                                _ItemBuild.GreenPropValue.Add(-1);
                            }

                            if (FindMagic.MA3Min > 0)
                            {
                                int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA3Min, FindMagic.MA3Max);

                                int DIV = RandomValue - FindMagic.MA3Min;

                                _ItemBuild.GreenPropValue.Add(DIV);
                            }
                            else
                            {
                                _ItemBuild.GreenPropValue.Add(-1);
                            }
                        }
                        else // Nếu không có set MAGIC LEVEL  -1
                        {
                            _ItemBuild.GreenPropValue.Add(-100);
                        }
                    }
                    else
                    {
                        _ItemBuild.GreenPropValue.Add(-2);
                    }
                }
            }

            if (ItemData.HiddenProp == null)
            {
                _ItemBuild.HiddenProbsCount = 0;
            }
            else
            {
                if (ItemData.HiddenProp.Count > 0)
                {
                    _ItemBuild.HiddenProbsCount = ItemData.HiddenProp.Count;

                    _ItemBuild.HiddenProbsValue = new List<int>();

                    List<PropMagic> List = ItemData.HiddenProp.OrderBy(x => x.Index).ToList();

                    foreach (PropMagic _Probs in List)
                    {
                        if (_Probs.MagicName.Length > 0)
                        {
                            MagicAttribLevel FindMagic = TotalMagicAttribLevel.Where(x => x.MagicName == _Probs.MagicName && x.Level == _Probs.MagicLevel).FirstOrDefault();

                            if (FindMagic != null)
                            {
                                if (FindMagic.MA1Min > 0)
                                {
                                    int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA1Min, FindMagic.MA1Max);

                                    int DIV = RandomValue - FindMagic.MA1Min;

                                    _ItemBuild.HiddenProbsValue.Add(DIV);
                                }
                                else
                                {
                                    _ItemBuild.HiddenProbsValue.Add(-1);
                                }

                                if (FindMagic.MA2Min > 0)
                                {
                                    int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA2Min, FindMagic.MA2Max);

                                    int DIV = RandomValue - FindMagic.MA2Min;

                                    _ItemBuild.HiddenProbsValue.Add(DIV);
                                }
                                else
                                {
                                    _ItemBuild.HiddenProbsValue.Add(-1);
                                }

                                if (FindMagic.MA3Min > 0)
                                {
                                    int RandomValue = KTGlobal.GetRandomNumber(FindMagic.MA3Min, FindMagic.MA3Max);

                                    int DIV = RandomValue - FindMagic.MA3Min;

                                    _ItemBuild.HiddenProbsValue.Add(DIV);
                                }
                                else
                                {
                                    _ItemBuild.HiddenProbsValue.Add(-1);
                                }
                            }
                            else // Nếu không có set MAGIC LEVEL  -1
                            {
                                _ItemBuild.HiddenProbsValue.Add(-100);
                            }
                        }
                        else
                        {
                            _ItemBuild.HiddenProbsValue.Add(-2);
                        }
                    }
                }
            }

            byte[] ItemDataByteArray = DataHelper.ObjectToBytes(_ItemBuild);
            if (ItemDataByteArray.Length == 0)
            {
                return "ERORR";
            }
            else
            {
                return Convert.ToBase64String(ItemDataByteArray);
            }
        }

        /// <summary>
        /// Cập nhật số lượng vật phẩm tương ứng
        /// Chú ý trước hàm này không được là 1 vòng FOREACH hoặc vòng for với LOCK ITEM ở đằng trước
        /// Nếu không vào trong này sẽ bọ LOCK 1 lần nữa dẫn tưới BUG DUPPER ITEM
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool UpdateItemCount(GoodsData data, KPlayer client, int Count, string From = "")
        {
            //Console.WriteLine("GoodsID :" + data.GoodsID + "ITEMDBID :" + data.Id + "| Count" + Count);

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Set Số lượng thất bại vật phẩm không tồn tại");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Set số lượng thất bại vật phẩm không hợp lệ");

                return false;
            }

            if (data.GCount < 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Set số lượng thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            if (Count < 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Set số lượng thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            // Xây dựng DICT update
            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, data.Id);
            TotalUpdate.Add(UPDATEITEM.GCOUNT, Count);

            string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

            string[] dbFields = null;

            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, client.ServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Set số lượng vật phẩm thất bại :" + data.Id + " Số lượng không hợp lệ");

                return false;
            }
            else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
            {
                /// Thực hiện update lại vật phẩm trong túi đồ
                KTGlobal.ModifyGoods(client, data.Id, TotalUpdate);

                /// Gửi lại dữ liệu cho client thay đổi thành công
                SCModGoods scData = new SCModGoods()
                {
                    BagIndex = data.BagIndex,
                    Count = data.GCount,
                    IsUsing = data.Using,
                    ModType = (int) ModGoodsTypes.ModValue,
                    ID = data.Id,
                    NewHint = 0,
                    Site = data.Site,
                    State = 0,
                };
                client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MOD_GOODS, scData);
                if (Count == 0)
                {
                    LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + data.GoodsID + "]["+data.GCount+"][" + data.ItemName + "][" + From + "] Xóa vật phẩm thành công : " + data.Id);
                }
                else
                {
                    LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + data.GoodsID + "][" + data.ItemName + "][" + From + "] Set số lượng vật phẩm về [" + Count + "] thành công : " + data.Id);
                }

                return true;
            }

            return false;
        }


        public static bool IsMaterial(ItemData _Item)
        {

            if(_Item.Genre == 1 && _Item.DetailType==17 && _Item.Category ==1)
            {
                return true;
            }



            return false;


        }
        /// <summary>
        /// Xóa bỏ vật phẩm đã hết hạn
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public static bool DestroyGoods(KPlayer client, GoodsData goodsData, string FROM = "")
        {
            String cmdData = "";

            if (goodsData.Using > 0)
            {
                // Nếu vật phẩm đang mặc
                // thì thào đồ ra khỏi người sau đó xóa
                client.GetPlayEquipBody().EquipUnloadMain(goodsData);

                ItemManager.AbandonItem(goodsData, client, false, FROM);
            }
            else
            {
                ItemManager.AbandonItem(goodsData, client, false, FROM);
            }

            return true;
        }

        /// <summary>
        /// Hàm sửa đổi thuộc tính trang bị
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="gd"></param>
        public static void NotifyGoodsInfo(KPlayer client, GoodsData gd)
        {
            client.SendPacket<GoodsData>((int) TCPGameServerCmds.CMD_SPR_NOTIFYGOODSINFO, gd);
        }

        /// <summary>
        /// Sửa chữa thuộc tính của 1 item DATA
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool ItemModValue(GoodsData data, KPlayer client, int NewSite)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]ModValue thất bại vật phẩm không tồn tại");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Modvalue thất bại vật phẩm không hợp lệ");

                return false;
            }

            if (data.GCount < 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Modvalue thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            int NewBagIndex = 0;

            // Xây dựng DICT update
            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, data.Id);

            /// Nếu mà Site == 1 tức là lấy bỏ đồ từ túi sang Thủ khố
            if (NewSite == 1)
            {
                int SlotIndex = KTGlobal.GetIdleSlotOfPortableBag(client);

                if (SlotIndex != -1 && SlotIndex < 100)
                {
                    TotalUpdate.Add(UPDATEITEM.SITE, NewSite);
                    TotalUpdate.Add(UPDATEITEM.BAGINDEX, SlotIndex);

                    NewBagIndex = SlotIndex;
                }
                else
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Kho đã đầy không thể cất đi");
                    return false;
                }
            }
            /// Nếu mà Site == 0 tức là lấy từ kho ra
            else if (NewSite == 0)
            {
                int SlotIndex = KTGlobal.GetIdleSlotOfBagGoods(client);

                if (SlotIndex != -1 && SlotIndex < client.BagNum)
                {
                    TotalUpdate.Add(UPDATEITEM.SITE, NewSite);
                    TotalUpdate.Add(UPDATEITEM.BAGINDEX, SlotIndex);

                    NewBagIndex = SlotIndex;
                }
                else
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Túi đồ đã đầy không thể lấy ra");
                    return false;
                }
            }
            else
            {
                return false;
            }

            string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

            string[] dbFields = null;

            // Thực hiện update vào DB thông tin của đồ
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, client.ServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "] Có lỗi xảy ra khi thao tác với DB SV:" + data.Id);

                return false;
            }
            else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
            {
                // Tức là lấy đồ từ túi sang thủ khố
                if (NewSite == 1)
                {
                    //Xóa trên nhân vật

                    string strcmd = string.Format("{0}:{1}", client.RoleID, data.Id);

                    // SEND VỀ CLLIENT XÓA VẬT PHẨM TRONG TÚI ĐỒ
                    client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MOVEGOODSDATA, strcmd);

                    // XÓA VẬT PHẨM TRONG TÚI ĐỒ Ở GS
                    Global.RemoveGoodsData(client, data);

                    // Gán lại vị trí mới cho vật phẩm ở thương khố
                    data.Site = NewSite;
                    data.BagIndex = NewBagIndex;

                    // ADD VẬT PHẨM VÀO TÚI ĐỒ
                    Global.AddPortableGoodsData(client, data);

                    // SEND PACKET ADD VẬT PHẨM VÀO THƯƠNG KHỐ THEO SITE MỚI
                    GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, data.Id, data.GoodsID, data.Forge_level, data.GCount, data.Binding, NewSite, 0, data.Endtime, data.Strong, data.BagIndex, -1, data.Props, data.Series, data.OtherParams);
                }
                // Nếu lấy từ thủ khố sang bên này
                else if (NewSite == 0)
                {
                    string strcmd = string.Format("{0}:{1}", client.RoleID, data.Id);

                    // SEND VỀ CLLIENT XÓA Ở THỦ KHỐ
                    client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MOVEGOODSDATA, strcmd);

                    // Xóa ở thủ khố
                    Global.RemovePortableGoodsData(client, data);

                    // Gán lại vị trí mới cho vật phẩm ở thương khố
                    data.Site = NewSite;
                    data.BagIndex = NewBagIndex;

                    // Add vào client
                    Global.AddGoodsData(client, data);

                    // SEND PACKET ADD VẬT PHẨM VÀO TÚI ĐỒ THEO SITE VÀ BAGINDEX MỚI
                    GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, data.Id, data.GoodsID, data.Forge_level, data.GCount, data.Binding, NewSite, 0, data.Endtime, data.Strong, data.BagIndex, -1, data.Props, data.Series, data.OtherParams);
                }

                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "] Thay đổi vị trí đồ thành công :" + data.Id);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Hàm tách 1 vật phẩm làm 2 vật phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool SplitItem(GoodsData data, KPlayer client, int Count)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Tách thất bại vật phẩm không tồn tại");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Tách thất bại vật phẩm không hợp lệ");

                return false;
            }

            if (data.GCount < 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Tách thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            if (data.GCount <= Count)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "]Số lượng muốn tách không hợp lệ ");

                return false;
            }

            int ItemLess = data.GCount - Count;

            if (ItemManager.UpdateItemCount(data, client, ItemLess, "SPLITITEM"))
            {
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, data.GoodsID, Count, 0, "SPLITITEM", false, data.Binding, false, Global.ConstGoodsEndTime, data.Props, data.Series, data.Creator, data.Forge_level, 0, false))
                {
                    PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm chế tạo");
                }
            }

            return false;
        }

        /// <summary>
        /// Xóa vật phẩm khỏi túi đồ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        ///

        public static bool AbadonItem(int ItemDBID, KPlayer client, bool DropToMap, string From = "")
        {
            GoodsData FindGoldById = Global.GetGoodsByDbID(client, ItemDBID);
            if (FindGoldById != null)
            {
                return AbandonItem(FindGoldById, client, DropToMap, From);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Funtion thực hiện update cho vật phẩm
        /// </summary>
        /// <param name="PramsInput"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public static bool UpdateItemPrammenter(Dictionary<UPDATEITEM, object> TotalUpdate, GoodsData data, KPlayer client, string From = "", bool UpdateNow = true)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Cập nhật thất bại vật phẩm không tồn tại");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Cập nhật thất bại vật phẩm không hợp lệ");

                return false;
            }

            if (data.GCount <= 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "Cập nhật thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            if (client.GoodsDataList == null)
            {
                return false;
            }

            GoodsData FindItem = null;
            lock (client.GoodsDataList)
            {
                FindItem = client.GoodsDataList.Where(x => x.Id == data.Id && x.GCount > 0).FirstOrDefault();
            }
            if (FindItem == null)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Cập nhật thất bại do vật phẩm không phải của chủ nhân");

                return false;
            }

            if (UpdateNow)
            {
                string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

                string[] dbFields = null;

                TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, client.ServerId);

                if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
                {
                    LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Xóa vật phẩm thất bại :" + data.Id + "|" + data.GoodsID + " Số lượng không hợp lệ");

                    return false;
                }
                else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
                {
                    // Thực hiện update lại vật phẩm trong túi đồ
                    KTGlobal.ModifyGoods(client, data.Id, TotalUpdate);

                    GoodsData _Data = Global.GetGoodsByDbID(client, data.Id);

                    ItemManager.NotifyGoodsInfo(client, _Data);

                    // LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]cập nhật phẩm thành công :" + data.Id);

                    return true;
                }
            }
            else
            {
                // Nếu ko cần update ngay chỉ mofify tạm chứ ko spam vào db
                KTGlobal.ModifyGoods(client, data.Id, TotalUpdate);

                GoodsData _Data = Global.GetGoodsByDbID(client, data.Id);

                ItemManager.NotifyGoodsInfo(client, _Data);

                // LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]cập nhật phẩm thành công :" + data.Id);

                return true;
            }

            return false;
        }

        public static bool AbandonItem(GoodsData data, KPlayer client, bool DropToMap, string From = "")
        {
            GoodsData _Template = new GoodsData();

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "][" + data.GoodsID + "]Hủy vật phẩm thất bại vật phẩm không tồn tại");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "][" + data.GoodsID + "]Hủy thất bại vật phẩm không hợp lệ");

                return false;
            }

            if (data.GCount <= 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "][" + data.GoodsID + "]Hủy thất bại số lượng còn lại không hợp lệ ");

                return false;
            }

            var FindItem = client.GoodsDataList?.Where(x => x.Id == data.Id && x.GCount > 0).FirstOrDefault();
            if (FindItem == null)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "][" + data.GoodsID + "]Hủy vật phẩm thất bại do vật phẩm không phải của chủ nhân");

                return false;
            }

            // Xây dựng DICT update
            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, data.Id);

            TotalUpdate.Add(UPDATEITEM.USING, -1);

            TotalUpdate.Add(UPDATEITEM.BAGINDEX, -1);
            // Set số lượng ==0 tức là xóa
            TotalUpdate.Add(UPDATEITEM.GCOUNT, 0);

            string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

            string[] dbFields = null;

            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, client.ServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Xóa vật phẩm thất bại :" + data.Id + " Số lượng không hợp lệ");

                return false;
            }
            else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
            {
                if (DropToMap)
                {
                    _Template.BagIndex = -1;
                    _Template.GoodsID = data.GoodsID;
                    _Template.GCount = data.GCount;
                    _Template.Forge_level = data.Forge_level;
                    _Template.OtherParams = data.OtherParams;
                    _Template.Props = data.Props;
                    _Template.Series = data.Series;
                    _Template.Site = 0;
                    _Template.Endtime = data.Endtime;
                    _Template.Strong = data.Strong;
                    _Template.Using = -1;

                    // Thả ra map ngay cho thằng khác có thể nhặt
                    ItemDropManager.CreateDropMapFromSingerGooods(client, _Template);
                }

                // Thực hiện update lại vật phẩm trong túi đồ
                KTGlobal.ModifyGoods(client, data.Id, TotalUpdate);

                SCModGoods scData = new SCModGoods()
                {
                    BagIndex = data.BagIndex,
                    Count = -1,
                    IsUsing = data.Using,
                    ModType = (int) ModGoodsTypes.Abandon,
                    ID = data.Id,
                    NewHint = 0,
                    Site = data.Site,
                    State = 0,
                };
                client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MOD_GOODS, scData);

                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "]Xóa vật phẩm thành công :" + data.GoodsID + " | COUNT : "+data.GCount+" |" + data.Id + "| " + data.ItemName + "| Forge :" + data.Forge_level);

                return true;
            }

            return false;
        }

        public static string GetNameItem(GoodsData data)
        {
            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                return "";
            }
            else
            {
                ItemData _Item = _TotalGameItem[data.GoodsID];
                return _Item.Name;
            }
        }

        /// <summary>
        /// Set cấp cường hóa cho trang bị
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="forgeLevel"></param>
        /// <returns></returns>
        public static bool SetEquipForgeLevel(GoodsData data, KPlayer client, int forgeLevel, int Lock = 0)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (!_TotalGameItem.ContainsKey(data.GoodsID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + data.GoodsID + "]Nâng cấp cường hóa thất bại :" + forgeLevel + " vật phẩm không tồn tại");

                return false;
            }

            ItemData _Item = _TotalGameItem[data.GoodsID];

            if (!KD_ISEQUIP(_Item.Genre))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + data.GoodsID + "]Cường hóa thất bại :" + forgeLevel + " vật phẩm không phải là trang bị không thể nâng cấp cường hóa");

                return false;
            }

            if (data.Id == -1)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + data.GoodsID + "]Cường hóa thất bại :" + forgeLevel + " Vật phẩm không hợp lệ");

                return false;
            }

            // Xây dựng DICT update
            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, data.Id);
            TotalUpdate.Add(UPDATEITEM.BINDING, Lock);
            TotalUpdate.Add(UPDATEITEM.FORGE_LEVEL, forgeLevel);

            string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

            string[] dbFields = null;

            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, client.ServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.Item, "Nâng cấp cường hóa thất bại");

                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, client.RoleID, data.Id, data.Forge_level, data.Binding);
                client.SendPacket((int) TCPGameServerCmds.CMD_SPR_FORGE, strcmd);

                return false;
            }
            else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
            {
                // Thực hiện update lại vật phẩm trong túi đồ
                KTGlobal.ModifyGoods(client, data.Id, TotalUpdate);

                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 1, client.RoleID, data.Id, data.Forge_level, data.Binding);
                client.SendPacket((int) TCPGameServerCmds.CMD_SPR_FORGE, strcmd);

                LogManager.WriteLog(LogTypes.Item, "Nâng cấp thành công!");

                return true;
            }

            return true;
        }

        public static int GetItemSeries(int InputSeri)
        {
            int Series = InputSeri;

            if (Series == -1)
            {
                Series = KTGlobal.GetRandomNumber(1, 5);
            }

            return Series;
        }

        public static bool IsExits(GoodsData Input)
        {
            if (Input == null || !_TotalGameItem.ContainsKey(Input.GoodsID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int TotalSpaceNeed(int ItemID, int Count)
        {
            if (!_TotalGameItem.ContainsKey(ItemID))
            {
                return -1;
            }

            ItemData _Item = _TotalGameItem[ItemID];

            if (Count > _Item.Stack)
            {
                return Count / _Item.Stack;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Tạo vật phẩm từ ID tương ứng
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static GoodsData CreateGoodsFromItemID(int itemID)
        {
            /// Nếu không tồn tại vật phẩm
            if (!ItemManager._TotalGameItem.TryGetValue(itemID, out ItemData itemData))
            {
                return null;
            }

            GoodsData itemGD = new GoodsData()
            {
                GoodsID = itemID,
                Props = ItemManager.KD_ISEQUIP(itemData.Genre) ? ItemManager.GenerateProbs(itemData) : "",
                Series = ItemManager.KD_ISEQUIP(itemData.Genre) ? itemData.Series >= (int) KE_SERIES_TYPE.series_metal && itemData.Series <= (int) KE_SERIES_TYPE.series_earth ? itemData.Series : KTGlobal.GetRandomNumber((int) KE_SERIES_TYPE.series_metal, (int) KE_SERIES_TYPE.series_earth) : 0,
            };
            return itemGD;
        }

        /// <summary>
        /// Call hàm này để tạo ITEM Cho 1 GameClient Bất Kỳ
        /// From : là vật phẩm tới từ đâu | Sự kiện nào  | Note cái này theo quy tắc để sau này check logs cho tiện
        /// EndTime : Hạn sử dụng nếu vật phẩm có hạn sử dụng
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="ItemID"></param>
        /// <param name="ItemNum"></param>
        /// <returns></returns>
        public static bool CreateItem(TCPOutPacketPool pool, KPlayer client, int ItemID, int ItemNum, int Site, string From, bool useOldGrid, int IsBinding, bool bIsFromMap, string EndTime, string InputProb = "", int InputSeri = -1, string Author = "", int enhanceLevel = 0, int HitItem = 1, bool IsNeedWriterLogs = false)
        {
            /// Kiểm tra xem vật phẩm có tồn tại hay không
            if (!_TotalGameItem.ContainsKey(ItemID))
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "] Create Item Error : " + ItemID + " => Item Don't Exits");

                return false;
            }
            if (ItemNum <= 0)
            {
                LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "] Create Item Error : " + ItemID + " => Number Item Not Support :" + ItemNum);

                return false;
            }

            int nBakGoodsNum = ItemNum;

            ItemData ItemData = _TotalGameItem[ItemID];

            string TmpEnd = "";
            string TmpStart = "";

            TmpEnd = EndTime;

            if (TmpEnd == "")
            {
                TmpEnd = Global.ConstGoodsEndTime;
            }

            int GetTimeEndOfSpecialItem = KTGlobal.GetItemExpiesTime(ItemID);

            if (GetTimeEndOfSpecialItem != -1)
            {
                DateTime dt = DateTime.Now.AddMinutes(GetTimeEndOfSpecialItem);

                // "1900-01-01 12:00:00";
                TmpEnd = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }

            int Series = ItemData.Series;

            if (InputSeri != -1)
            {
                Series = InputSeri;
            }
            else
            {
                if (Series == -1)
                {
                    Series = KTGlobal.GetRandomNumber(1, 5);
                }
            }

            TmpStart = Global.ConstGoodsEndTime;

            int Quality = 1;
            int ForgeLevel = enhanceLevel;

            string Probs = "";

            int NewHit = 1;// 1 : Có thông báo cho client nhận được vật phẩm mới

            string JewelList = "";

            /// NẾU LÀ TRANG BỊ
            if (KD_ISEQUIP(ItemData.Genre))
            {
                if (InputProb == "")
                {
                    // ItemNum = 1;
                    Probs = ItemManager.GenerateProbs(ItemData);
                }
                else
                {
                    //  ItemNum = 1;
                    Probs = InputProb;
                }
            }

            // NẾU KHÔNG PHẢI TRANG BỊ
            {
                int MaxStack = ItemData.Stack;

                // Nếu tận dụng lại các ô đồ cũ
                if (useOldGrid)
                {
                    // Nếu mà item có stack > 1 thì mới có tác dụng trong việc sử dụng OLDGIRL
                    if (MaxStack > 1)
                    {
                        int startIndex = 0;

                        GoodsData goodsData = Global.GetGoodsByID(client, ItemID, IsBinding, TmpEnd, ref startIndex);

                        // Lấy ra vật phẩm xem còn vật phẩm nào còn có thể STACK thêm được không

                        while (null != goodsData && ItemNum > 0)
                        {
                            if (goodsData.GCount < MaxStack)
                            {
                                int newGoodsNum = 0;
                                int newNum = ItemNum + goodsData.GCount;
                                if (newNum > MaxStack)
                                {
                                    newGoodsNum = MaxStack;
                                    ItemNum = newNum - MaxStack;
                                }
                                else
                                {
                                    newGoodsNum = goodsData.GCount + ItemNum;
                                    ItemNum = 0;
                                }

                                // Xây dựng DICT update
                                Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

                                TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
                                TotalUpdate.Add(UPDATEITEM.ITEMDBID, goodsData.Id);

                                TotalUpdate.Add(UPDATEITEM.GCOUNT, newGoodsNum);

                                string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

                                string[] UpdateFriend = null;

                                // Request Sửa Số lượng vào DB
                                Global.RequestToDBServer(Global._TCPManager.tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out UpdateFriend, client.ServerId);

                                // Modify Danh sách vật phẩm hiện tại tại GS
                                KTGlobal.ModifyGoods(client, goodsData.Id, TotalUpdate);

                                KTGlobal.NotifyGoood(client, goodsData.BagIndex, newGoodsNum, goodsData.Using, ModGoodsTypes.ModValue, goodsData.Id, 1, goodsData.Site, 0);
                            }
                            // THỰC HIỆN LOOP HẾT CÁC ITEM NẾU CHƯA FILL HẾT CÁC ITEM CÒN THIẾU
                            goodsData = Global.GetGoodsByID(client, ItemID, IsBinding, TmpEnd, ref startIndex);
                        }
                    }

                    if (ItemNum <= 0) // NẾU KHÔNG CẦN VADD THÊM VẬT PHẨM NỮA THÌ RETURN 0 ==> ADD VẬT PHẨM THÀNH CÔNG
                    {
                        ProcessTask.ProseccTaskBeforeDoTask(Global._TCPManager.MySocketListener, pool, client);
                        return true;
                    }
                }

                // Vòng while thứ 2 để add tới khi nào đủ số item yêu cầu thì thôi
                while (ItemNum > 0)
                {
                    int ItemCountAdd = 0;

                    if (ItemNum > MaxStack)
                    {
                        ItemCountAdd = MaxStack;
                        ItemNum = ItemNum - MaxStack;
                    }
                    else
                    {
                        ItemCountAdd = ItemNum;
                        ItemNum = 0;
                    }

                    int BagIndex = 0;
                    // Nếu là thêm vật phẩm thẳng vào túi đồ trên người
                    if (0 == Site)
                    {
                        BagIndex = KTGlobal.GetIdleSlotOfBagGoods(client);
                    }
                    else if ((int) SaleGoodsConsts.PortableGoodsID == Site)
                    {
                        BagIndex = KTGlobal.GetIdleSlotOfPortableBag(client);
                    }

                    // COde lại nếu mà vị trí túi đồ < 0 hoặc là vị trí túi đồ > 100 thì là không còn chỗ trống
                    if (BagIndex < 0 || BagIndex > client.BagNum)
                    {
                        PlayerManager.ShowNotification(client, "Túi đồ đã đầy! Vui lòng dọn túi đồ và thử lại!");
                        return false;
                    }

                    string newEndTime = TmpEnd.Replace(":", "$");
                    string newStartTime = TmpStart.Replace(":", "$");

                    Dictionary<ItemPramenter, string> OtherParams = new Dictionary<ItemPramenter, string>();

                    // Đoạn này để xử lý toàn bộ các item cần lưu thuộc tính OTHER PRAMM ngay ban đầu ví dụ như mật tịch
                    if (KD_ISEQUIP(ItemData.Genre))
                    {
                        if (KD_ISBOOK(ItemData.DetailType))
                        {
                            string LeveBegin = "1";
                            string ExpBegin = "0";
                            OtherParams.Add(ItemPramenter.Pram_1, LeveBegin);
                            OtherParams.Add(ItemPramenter.Pram_2, ExpBegin);
                            OtherParams.Add(ItemPramenter.Pram_3, GetMaxbookEXP(1, ItemData.Level) + "");
                        }
                        if (KD_ISSIGNET(ItemData.DetailType))
                        {
                            string LeveBegin = "1";
                            string ExpBegin = "0";

                            string Final = LeveBegin + "|" + ExpBegin;

                            OtherParams.Add(ItemPramenter.Pram_1, Final);
                            OtherParams.Add(ItemPramenter.Pram_2, Final);
                        }
                    }
                    else
                    {
                        // Nếu nó là KIM tê thì ghi lại số đơn vị có thể sửa chữa đồ tối đa
                        if (KD_ISJINXI(ItemData.ItemID))
                        {
                            OtherParams.Add(ItemPramenter.Pram_1, ItemData.ListExtPram[0].Pram + "");
                        }

                        // Nếu vật phẩm là càn khôn phù
                        if (IsQianKunFu(ItemData.ItemID))
                        {
                            if (ItemData.ItemID == 354)
                            {
                                OtherParams.Add(ItemPramenter.Pram_1, 100 + "");
                            }

                            if (ItemData.ItemID == 344)
                            {
                                OtherParams.Add(ItemPramenter.Pram_1, 10 + "");
                            }
                        }
                    }

                    if (Author != "")
                    {
                        OtherParams.Add(ItemPramenter.Creator, Author);
                    }

                    byte[] ItemDataByteArray = DataHelper.ObjectToBytes(OtherParams);

                    string otherPramer = Convert.ToBase64String(ItemDataByteArray);

                    //SEND DATA TO DB SERVER REQUEST SAVE TO DB
                    string[] dbFields = null;
                    string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}",
                        client.RoleID, ItemData.ItemID, ItemCountAdd, Probs, ForgeLevel, IsBinding, Site, BagIndex,
                        newStartTime, newEndTime, 100, Series, otherPramer);

                    TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(Global._TCPManager.tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_ADDGOODS_CMD, strcmd, out dbFields, client.ServerId);
                    // Nếu lỗi khi kết nối tới GAMEDB
                    if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
                    {
                        LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "] Create Item Error : " + ItemID + " => DB Writer BUG Connect To DbServer :" + ItemCountAdd + " | ITEMCMD :" + strcmd);
                        return false;
                    }

                    // Nếu CÓ LỖI KHI ADD VÀO DB THÌ TRẢ VỀ MÃ LỖI
                    if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[0]) < 0)
                    {
                        LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "] Create Item Error : " + ItemID + " => DB Writer BUG Connect To DbServer :" + ItemCountAdd + " | EROR CODE  :" + Convert.ToInt32(dbFields[0]));

                        return false;
                    }

                    int DbItemID = Convert.ToInt32(dbFields[0]);

                    GoodsData gd = null;

                    if (Site == 0)
                    {
                        gd = Global.AddGoodsData(client, DbItemID, ItemData.ItemID, ForgeLevel, Quality, ItemCountAdd, IsBinding, Site, TmpStart, TmpEnd,
                            100, BagIndex, -1, Probs, Series, OtherParams);

                        // Xử lý task => đối với nhiệm vụ yêu cầu mua vật phẩm gì đó
                        // ProcessTask.Process(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, -1, -1, ItemData.ItemID, TaskTypes.BuySomething);
                    }
                    else if (Site == (int) SaleGoodsConsts.PortableGoodsID)//Update danh sách vật phẩm trong kho
                    {
                        // Đây là add đồ vào THỦ KHỐ
                        gd = Global.AddPortableGoodsData(client, DbItemID, ItemData.ItemID, ForgeLevel, Quality, ItemCountAdd, IsBinding, Site, JewelList, TmpEnd,
                            0, 0, 0, 0, 0, 0, 0);
                    }

                    if (null != gd)
                    {
                        // Nếu mà cần ghi logs vật phẩm này
                        if (IsNeedWriterLogs)
                        {
                            int RoleID = client.RoleID;
                            string AccountName = client.strUserID;
                            string RecoreType = "ITEMCREATE";
                            string RecoreDesc = From;
                            string Source = "SYSTEM";
                            string Taget = client.RoleName;
                            string OptType = "ADD";

                            int ZONEID = client.ZoneID;

                            string OtherPram_1 = DbItemID + "";
                            string OtherPram_2 = gd.GoodsID + "";
                            string OtherPram_3 = gd.GCount + "";
                            string OtherPram_4 = "NONE";

                            //Thực hiện việc ghi LOGS vào DB
                            GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                        }

                        // LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][" + From + "] CreateItem : " + ItemData.Name + "|" + ItemData.ItemID + "| Count :" + ItemCountAdd);
                    }

                    // SEND NOTIFY về vật phẩm mới được add vào TÚI đồ
                    GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, DbItemID, ItemData.ItemID, ForgeLevel, ItemCountAdd, IsBinding, Site, HitItem, newEndTime, 100, BagIndex, -1, Probs, Series, OtherParams);
                }

                // Thực hiện update các vật phẩm liên quan tới nhiệm vụ cần có vật phẩm
                ProcessTask.ProseccTaskBeforeDoTask(Global._TCPManager.MySocketListener, pool, client);

                // Kiểm tra vị trí trống của BALO

                return true;
            }
        }

        #region Vật phẩm

        /// <summary>
        /// Ghép vật phẩm
        /// </summary>
        /// <param name="inputClientItems"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool MergeItems(List<GoodsData> inputClientItems, KPlayer player)
        {
            /// Toác
            if (inputClientItems == null)
            {
                PlayerManager.ShowNotification(player, "Dữ liệu không hợp lệ!");
                return false;
            }

            /// Tổng số vật phẩm
            int gCount = 0;
            /// Mảng đánh dấu đã tồn tại ID vật phẩm tương ứng chưa
            HashSet<int> availableItems = new HashSet<int>();
            /// ID vật phẩm
            int itemID = -1;
            /// Có tìm thấy vật phẩm khóa không
            bool foundBound = false;
            /// Có tìm thấy vật phẩm không khóa không
            bool foundUnbound = false;
            /// Duyệt danh sách
            foreach (GoodsData itemGD in inputClientItems)
            {
                /// Toác
                if (itemGD == null)
                {
                    continue;
                }

                /// Thông tin vật phẩm thật ở trong túi
                GoodsData gd = Global.GetGoodsByDbID(player, itemGD.Id);
                /// Toác
                if (gd == null)
                {
                    PlayerManager.ShowNotification(player, "Dữ liệu không hợp lệ!");
                    continue;
                }

                /// Nếu có hạn sử dụng
                if (gd.Endtime != Global.ConstGoodsEndTime)
                {
                    PlayerManager.ShowNotification(player, "Chỉ có đồ không có hạn sử dụng mới có thể xếp chồng!");
                    return false;
                }

                /// Nếu chưa tìm thấy vật phẩm
                if (itemID == -1)
                {
                    itemID = gd.GoodsID;
                }

                /// Nếu ID không khớp nhau
                if (gd.GoodsID != itemID)
                {
                    PlayerManager.ShowNotification(player, "Chỉ có vật phẩm cùng loại mới có thể xếp chồng!");
                    return false;
                }

                /// Nếu số lượng không thỏa mãn
                if (gd.GCount <= 0)
                {
                    PlayerManager.ShowNotification(player, "Dữ liệu không hợp lệ!");
                    return false;
                }

                /// Nếu đã tồn tại => Có BUG Duplicate vật phẩm ở Client gửi lên
                if (availableItems.Contains(gd.Id))
                {
                    PlayerManager.ShowNotification(player, "Dữ liệu không hợp lệ!");
                    return false;
                }
                /// Thêm vào danh sách
                availableItems.Add(gd.Id);

                /// Nếu vật phẩm khóa
                if (gd.Binding == 1)
                {
                    foundBound = true;
                }
                /// Nếu vật phẩm không khóa
                else
                {
                    foundUnbound = true;
                }

                /// Tăng tổng số vật phẩm tìm thấy lên
                gCount += gd.GCount;
            }

            /// Danh sách truyền về không khớp
            if (availableItems.Count != inputClientItems.Count)
            {
                PlayerManager.ShowNotification(player, "Dữ liệu không hợp lệ");
                return false;
            }

            /// Nếu có ít hơn 2 vật phẩm
            if (availableItems.Count < 2)
            {
                PlayerManager.ShowNotification(player, "Phải nhiều hơn 1 vật phẩm mới có thể xếp chồng");
                return false;
            }

            /// Thông tin vật phẩm tương ứng
            ItemData itemData = ItemManager.GetItemTemplate(itemID);
            if (itemData != null)
            {
                if (itemData.Stack <= 1)
                {
                    PlayerManager.ShowNotification(player, "Vật phẩm này không thể xếp chồng!");
                    return false;
                }
            }

            /// Nếu tìm thấy cả vật phẩm khóa và không khóa
            if ((foundBound && foundUnbound) || (!foundBound && !foundUnbound))
            {
                PlayerManager.ShowNotification(player, "Đồ phải loại khóa hoặc không khóa mới có thể xếp chồng!");
                return false;
            }

            /// Duyệt danh sách
            foreach (int itemDBID in availableItems)
            {
                /// Xóa vật phẩm
                if (!ItemManager.AbadonItem(itemDBID, player, false, "MergeItems"))
                {
                    PlayerManager.ShowNotification(player, "Có sai sót dữ liệu");
                    return false;
                }
            }

            /// Tạo vật phẩm mới số lượng tương ứng
            if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, itemID, gCount, 0, "MergeItems", false, foundBound ? 1 : 0, false, Global.ConstGoodsEndTime))
            {
                PlayerManager.ShowNotification(player, "Có lỗi khi nhận vật phẩm chế tạo");
            }

            /// Trả về kết quả
            return true;
        }

        /// <summary>
        /// Trả về tổng số vật phẩm tương ứng trong túi người chơi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int GetItemCountInBag(KPlayer player, int itemID, int Binding = -1)
        {
            lock (player.GoodsDataList)
            {
                if (player.GoodsDataList == null)
                {
                    return 0;
                }

                //int count = player.GoodsDataList.Sum((itemGD) =>
                //{
                //    /// Nếu ID vật phẩm trùng với ID cần đếm, và nằm trong túi đồ
                //    if (itemGD.GoodsID == itemID && itemGD.Using == -1 && itemGD.GCount>0)
                //    {
                //        return itemGD.GCount;
                //    }
                //    return 0;
                //});

                int count = 0;

                if (Binding == -1)
                {
                    count = player.GoodsDataList.Where(x => x.GoodsID == itemID && x.Site == 0 && x.Using == -1 && x.GCount > 0).Sum(x => x.GCount);
                }
                else
                {
                    count = player.GoodsDataList.Where(x => x.GoodsID == itemID && x.Site == 0 && x.Using == -1 && x.GCount > 0 && x.Binding == Binding).Sum(x => x.GCount);
                }

                return count;
            }
        }

        /// <summary>
        /// Xóa vật phẩm số lượng tương ứng khỏi túi người chơi
        ///
        /// Hàm này anh thanh edit lại ngày 16/8 tránh lock 2 lần gây bug
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        public static bool RemoveItemFromBag(KPlayer player, int itemID, int count, int Binding = -1, string From = "")
        {
            /// Số lượng hiện có
            int itemCount = ItemManager.GetItemCountInBag(player, itemID);

            /// Nếu không đủ lượng xóa
            if (itemCount < count)
            {
                return false;
            }

            List<WaitBeRemove> _WaitBeRemove = new List<WaitBeRemove>();
            /// Số lượng cần xóa còn lại
            int quantityLeft = count;

            lock (player.GoodsDataList)
            {
                /// Duyệt toàn bộ danh sách vật phẩm
                for (int i = 0; i < player.GoodsDataList.Count; i++)
                {
                    /// Vật phẩm tương ứng
                    GoodsData itemGD = player.GoodsDataList[i];

                    /// Nếu ID vật phẩm trùng với ID cần xóa, và nằm trong túi đồ
                    if (itemGD.GoodsID == itemID && itemGD.Using == -1)
                    {
                        /// Nếu chỉ xóa vật phẩm khóa hoặc không khóa
                        if (Binding != -1)
                        {
                            // Nếu mà có khóa hoặc không khóa thì sẽ check xem vật phẩm có thuộc tính khóa hoặc không khóa theo yêu cầu không
                            if (itemGD.Binding != Binding)
                            {
                                continue;
                            }
                        }

                        // Trường hợp 1 nếu mà vật phẩm có số lượng nhiều hơn số lượng cần
                        if (itemGD.GCount >= quantityLeft)
                        {
                            /// Giảm số lượng tương ứng

                            int ItemLess = itemGD.GCount - quantityLeft;

                            /// Update lại số lượng vật phẩm
                            // ItemManager.UpdateItemCount(itemGD, player, ItemLess);

                            WaitBeRemove Item = new WaitBeRemove();
                            Item._Good = itemGD;
                            Item.ItemLess = ItemLess;

                            _WaitBeRemove.Add(Item);

                            /// Cập nhật số lượng còn lại cần xóa
                            quantityLeft = 0;
                        }
                        else
                        {
                            /// Cập nhật số lượng cần xóa
                            quantityLeft = quantityLeft - itemGD.GCount;

                            WaitBeRemove Item = new WaitBeRemove();
                            Item._Good = itemGD;
                            Item.ItemLess = 0;

                            _WaitBeRemove.Add(Item);
                            /// Thực hiện Update lên DB
                           // ItemManager.UpdateItemCount(itemGD, player, 0);
                        }
                    }

                    /// Nếu số lượng cần xóa còn lại <= 0
                    if (quantityLeft <= 0)
                    {
                        break;
                    }
                }
            }

            //Thực hiện xóa lần lượt
            foreach (WaitBeRemove _Wait in _WaitBeRemove)
            {
                ItemManager.UpdateItemCount(_Wait._Good, player, _Wait.ItemLess, From);
            }

            Console.WriteLine(quantityLeft);

            return true;
        }

        /// <summary>
        /// Xóa vật phẩm sau khi sử dụng (nếu có)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemGD"></param>
        public static bool DeductItemOnUse(KPlayer player, GoodsData itemGD, string FROM = "")
        {
            /// Nếu vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                PlayerManager.ShowNotification(player, "Vật phẩm không tồn tại!");
                return false;
            }

            /// Nếu vật phẩm có thiết lập xóa sau khi sử dụng
            if (itemData.DeductOnUse)
            {
                /// Số lượng còn lại
                itemGD.GCount--;

                /// Cập nhật số lượng còn lại
                return ItemManager.UpdateItemCount(itemGD, player, itemGD.GCount, FROM);
            }

            return false;
        }

        public static bool RemoveItemByCount(KPlayer player, GoodsData itemGD, int Count, string From)
        {
            /// Nếu không có vật phẩm
            if (itemGD == null)
            {
                PlayerManager.ShowNotification(player, "Vật phẩm không tồn tại!");
                return false;
            }

            /// Nếu vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                PlayerManager.ShowNotification(player, "Vật phẩm không tồn tại!");
                return false;
            }

            if (itemGD.GCount < Count)
            {
                PlayerManager.ShowNotification(player, "Số lượng vật phẩm không đủ!");
                return false;
            }

            /// Số lượng còn lại
            itemGD.GCount = itemGD.GCount - Count;

            /// Cập nhật số lượng còn lại
            return ItemManager.UpdateItemCount(itemGD, player, itemGD.GCount, From);
        }

        /// <summary>
        /// Sử dụng vật phẩm tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemGD"></param>
        public static void UseItem(KPlayer player, GoodsData itemGD)
        {
            /// Nếu trong trạng thái không thể dùng vật phẩm
            if (!player.IsCanDoLogic())
            {
                //PlayerManager.ShowNotification(player, "Trong trạng thái khống chế không thể sử dụng vật phẩm!");
                return;
            }

            /// Nếu vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                PlayerManager.ShowNotification(player, "Vật phẩm không tồn tại!");
                return;
            }

            /// Nếu là trang bị
            if (ItemManager.KD_ISEQUIP(itemData.Genre))
            {
                return;
            }
            /// Nếu cấp độ không đủ
            else if (itemData.ReqLevel > player.m_Level)
            {
                PlayerManager.ShowNotification(player, string.Format("Cần đạt cấp {0} trở lên mới có thể sử dụng vật phẩm này!", itemData.ReqLevel));
                return;
            }

            /// Nếu có Script điều khiển
            if (itemData.IsScriptItem)
            {
                /// Script điều khiển tương ứng
                KTLuaScript.LuaScript script = KTLuaScript.Instance.GetScriptByID(itemData.ScriptID);
                /// Nếu Script không tồn tại
                if (script == null)
                {
                    LogManager.WriteLog(LogTypes.Item, string.Format("Player '{0}' uses item ID '{1}', calls script ID '{2}' NOT FOUND!!!", player.RoleName, itemGD.GoodsID, itemData.ScriptID));
                    return;
                }

                /// Bản đồ tương ứng
                if (!GameManager.MapMgr.DictMaps.TryGetValue(player.MapCode, out GameMap map))
                {
                    return;
                }

                /// Danh sách các tham biến khác
                Dictionary<int, string> otherParams = new Dictionary<int, string>();

                /// Thực thi hàm kiểm tra điều kiện ở Script tương ứng
                KTLuaEnvironment.ExecuteItemScript_OnPreCheckCondition(map, itemGD, player, itemData.ScriptID, otherParams, (res) =>
                {
                    /// Nếu kiểm tra điều kiện thất bại
                    if (!res)
                    {
                        return;
                    }

                    /// Thực thi hàm dùng vật phẩm ở Script tương ứng
                    KTLuaEnvironment.ExecuteItemScript_OnUse(map, itemGD, player, itemData.ScriptID, otherParams);

                    /// Thực thi sự kiện dùng vật phẩm
                    player.UseItemCompleted(itemGD);
                });
            }
            /// Nếu là thuốc
            else if (itemData.IsMedicine)
            {
                /// Nếu Prop không tồn tại
                if (itemData.MedicineProp == null || itemData.MedicineProp.Count <= 0 || itemData.BuffID == -1)
                {
                    return;
                }

                /// ID kỹ năng tương ứng
                int skillID = itemData.BuffID;
                /// Thời gian tồn tại
                int duration = itemData.MedicineProp[0].Time * 1000 / 18;
                /// Có lưu vào DB không
                bool saveToDB = itemData.BuffID == 100020;

                /// Kỹ năng tương ứng
                SkillDataEx skill = KSkill.GetSkillData(skillID);

                /// Nếu kỹ năng không tồn tại
                if (skill == null)
                {
                    return;
                }

                /// Dữ liệu kỹ năng tương ứng
                SkillLevelRef skillRef = new SkillLevelRef()
                {
                    Data = skill,
                    AddedLevel = 1,
                    BonusLevel = 0,
                    CanStudy = false,
                };
                BuffDataEx buff = new BuffDataEx()
                {
                    Skill = skillRef,
                    Duration = duration,
                    LoseWhenUsingSkill = false,
                    SaveToDB = saveToDB,
                    StackCount = 1,
                    StartTick = KTGlobal.GetCurrentTimeMilis(),
                    CustomProperties = new PropertyDictionary(),
                };

                /// Duyệt toàn bộ thuộc tính của thuốc
                foreach (Medicine prop in itemData.MedicineProp)
                {
                    string propName = prop.MagicName;
                    int value = prop.Value;

                    /// Lấy thuộc tính tương ứng theo tên
                    if (PropertyDefine.PropertiesBySymbolName.TryGetValue(propName, out PropertyDefine.Property property))
                    {
                        KMagicAttrib magicAttrib = new KMagicAttrib()
                        {
                            nAttribType = (MAGIC_ATTRIB) property.ID,
                            nValue = new int[] { value, 0, 0 },
                        };

                        buff.CustomProperties.Set<KMagicAttrib>(property.ID, magicAttrib);
                    }
                }
                /// Thêm Buff vào cho người chơi
                player.Buffs.AddBuff(buff);

                /// Nếu vật phẩm có thiết lập xóa sau khi sử dụng
                if (itemData.DeductOnUse)
                {
                    /// Số lượng còn lại
                    itemGD.GCount--;

                    /// Cập nhật số lượng còn lại
                    ItemManager.UpdateItemCount(itemGD, player, itemGD.GCount, "USINGITEM");
                }

                /// Thực thi sự kiện dùng vật phẩm
                player.UseItemCompleted(itemGD);
            }
        }

        #endregion Vật phẩm
        #region BoundItemFaction

        /// <summary>
        /// Hàm này add vật phẩm cho người chơi khi tham gia vào môn phái nào đó
        /// </summary>
        public static void AddFactionItem(KPlayer client, int FactionJoin)
        {
            switch (FactionJoin)
            {
                case 1:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10031, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10041, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        break;
                    }
                case 2:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10061, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10051, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        break;
                    }
                case 3:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 12628, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 12638, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 4:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10071, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10081, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        break;
                    }
                case 5:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10111, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10121, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }

                case 6:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10091, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10121, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 7:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10151, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10131, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 8:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10161, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10051, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 9:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10181, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10191, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 10:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10171, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10201, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 11:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10941, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10931, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 12:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10121, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 10101, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 15:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 31776, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 31780, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
                case 16:
                    {
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 41175, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
                        ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 41176, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);

                        break;
                    }
            }
        }

        #endregion BoundItemFaction
    }
}