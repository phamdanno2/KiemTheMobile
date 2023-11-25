using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{
    /// <summary>
    /// Danh sách vật phẩm rơi sẽ thông báo khi người chơi nhặt
    /// </summary>
    public class PickUpItemNotify
	{
        /// <summary>
        /// Thông tin vật phẩm
        /// </summary>
        public class Item
		{
            /// <summary>
            /// ID vật phẩm
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Chỉ thông báo nếu rơi ở Boss
            /// </summary>
            public bool BossOnly { get; set; }

            /// <summary>
            /// Chuyển đối tượng từ XMLNode
            /// </summary>
            /// <param name="xmlNode"></param>
            /// <returns></returns>
            public static Item Parse(XElement xmlNode)
			{
                return new Item()
                {
                    ID = int.Parse(xmlNode.Attribute("ID").Value),
                    BossOnly = bool.Parse(xmlNode.Attribute("BossOnly").Value),
                };
			}
		}

        /// <summary>
        /// Danh sách vật phẩm
        /// </summary>
        public Dictionary<int, Item> Items { get; private set; }

        /// <summary>
        /// Chuyển đối tượng từ XMLNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static PickUpItemNotify Parse(XElement xmlNode)
		{
            PickUpItemNotify notify = new PickUpItemNotify()
            {
                Items = new Dictionary<int, Item>(),
            };

            foreach (XElement node in xmlNode.Elements("Item"))
			{
                Item item = Item.Parse(node);
                notify.Items[item.ID] = item;
			}

            return notify;
		}
    }

    [XmlRoot(ElementName = "MapDropInfo")]
    public class MapDropInfo
    {
        [XmlAttribute(AttributeName = "MonsterName")]
        public string MonsterName { get; set; }
        [XmlAttribute(AttributeName = "MonsterID")]
        public int MonsterID { get; set; }


        [XmlAttribute(AttributeName = "DropCount")]
        public int DropCount { get; set; } = 1;


        [XmlAttribute(AttributeName = "DropProfile")]
        public string DropProfile { get; set; }


        public List<DropProfile> TotalFile
        {
            get
            {
                return ItemDropManager.TotalProfile.Where(x => x.DropProfileID == this.DropProfile).ToList();
            }
        }

    }

    [XmlRoot(ElementName = "DropProfile")]
    public class DropProfile
    {
        [XmlAttribute(AttributeName = "DropProfileID")]
        public string DropProfileID { get; set; }

        /// <summary>
        /// Bạc
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "Money")]
        public int Money { get; set; }

        /// <summary>
        /// Bạc khóa
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "BindMoney")]
        public int BindMoney { get; set; }

        /// <summary>
        /// IDItem
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "ItemID")]
        public int ItemID { get; set; }

        /// <summary>
        /// Tên vật phẩm
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "ItemName")]
        public string ItemName { get; set; }

        /// <summary>
        /// Tỉ lệ drop từ quái
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "Rate")]
        public int Rate { get; set; }

        /// <summary>
        /// Tỉ lệ rơi đồ từ may mắn của Char
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "LuckyRate")]
        public int LuckyRate { get; set; } = 1;
    }

    /// <summary>
    /// Tỷ lệ rơi ở quái có ID tương ứng
    /// </summary>
    public class MonsterDropRate
    {
        /// <summary>
        /// ID quái
        /// </summary>
        public int MonsterID { get; set; }

        /// <summary>
        /// Danh sách bản đồ
        /// </summary>
        public HashSet<int> MapIDs { get; set; }

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public HashSet<int> QuestIDs { get; set; }

        /// <summary>
        /// Chỉ tác dụng khi là phụ bản
        /// </summary>
        public bool CopySceneOnly { get; set; }

        /// <summary>
        /// Tỷ lệ rơi (phần 10000)
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Chuyển đối tượng từ XMLNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static MonsterDropRate Parse(XElement xmlNode)
        {
            MonsterDropRate monsterDropRate = new MonsterDropRate()
            {
                MonsterID = int.Parse(xmlNode.Attribute("MonsterID").Value),
                Rate = int.Parse(xmlNode.Attribute("Rate").Value),
                CopySceneOnly = bool.Parse(xmlNode.Attribute("CopySceneOnly").Value),
            };

            /// Chuỗi danh sách bản đồ
            string mapIDsString = xmlNode.Attribute("MapIDs").Value;
            /// Nếu có bản đồ
            if (mapIDsString != "-1")
            {
                /// Tạo mới
                monsterDropRate.MapIDs = new HashSet<int>();
                /// Duyệt danh sách bản đồ
                foreach (string mapIDString in mapIDsString.Split(';'))
                {
                    /// Thêm vào danh sách
                    monsterDropRate.MapIDs.Add(int.Parse(mapIDString));
                }
            }

            /// Chuỗi danh sách nhiệm vụ
            string questIDsString = xmlNode.Attribute("QuestIDs").Value;
            /// Nếu có bản đồ
            if (questIDsString != "-1")
            {
                /// Tạo mới
                monsterDropRate.QuestIDs = new HashSet<int>();
                /// Duyệt danh sách nhiệm vụ
                foreach (string questIDString in questIDsString.Split(';'))
                {
                    /// Thêm vào danh sách
                    monsterDropRate.QuestIDs.Add(int.Parse(questIDString));
                }
            }

            return monsterDropRate;
        }
    }
}
