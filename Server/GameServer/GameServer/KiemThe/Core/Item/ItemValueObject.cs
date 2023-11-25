using GameServer.KiemThe.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{

    [XmlRoot(ElementName = "ItemValueCaculation")]
    public class ItemValueCaculation
    {
        [XmlElement(ElementName = "Magic_Combine_Def")]
        public Magic_Combine Magic_Combine_Def { get; set; }

        [XmlElement(ElementName = "List_Equip_Type_Rate")]
        public List<Equip_Type_Rate> List_Equip_Type_Rate { get; set; }

        [XmlElement(ElementName = "List_Enhance_Value")]
        public List<Enhance_Value> List_Enhance_Value { get; set; }

        [XmlElement(ElementName = "List_Strengthen_Value")]
        public List<Strengthen_Value> List_Strengthen_Value { get; set; }

        [XmlElement(ElementName = "List_Equip_StarLevel")]
        public List<Equip_StarLevel> List_Equip_StarLevel { get; set; }

        [XmlElement(ElementName = "List_Equip_Random_Pos")]
        public List<Equip_Random_Pos> List_Equip_Random_Pos { get; set; }

        [XmlElement(ElementName = "List_Equip_Level")]
        public List<Equip_Level> List_Equip_Level { get; set; }


        [XmlElement(ElementName = "List_StarLevelStruct")]
        public List<StarLevelStruct> List_StarLevelStruct { get; set; }

    }

    [XmlRoot(ElementName = "MagicSource")]

    public class MagicSource
    {
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }

    }
    [XmlRoot(ElementName = "StarLevelStruct")]
    public class StarLevelStruct
    {
        [XmlAttribute(AttributeName = "Value")]
        public long Value { get; set; }
        [XmlAttribute(AttributeName = "StarLevel")]
        public int StarLevel { get; set; }
        [XmlAttribute(AttributeName = "NameColor")]

        public string NameColor { get; set; }
        [XmlAttribute(AttributeName = "EmptyStar")]
        public int EmptyStar { get; set; }
        [XmlAttribute(AttributeName = "FillStar")]
        public int FillStar { get; set; }


    }


   

    [XmlRoot(ElementName = "MagicDesc")]
    public class MagicDesc
    {
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        [XmlAttribute(AttributeName = "ListValue")]
        public List<int> ListValue { get; set; }

    }


    [XmlRoot(ElementName = "Magic_Combine")]
    public class Magic_Combine
    {
        [XmlElement(ElementName = "MagicSourceDef")]
        public List<MagicSource> MagicSourceDef { get; set; }

        [XmlElement(ElementName = "MagicDescDef")]
        public List<MagicDesc> MagicDescDef { get; set; }
    }
    [XmlRoot(ElementName = "Equip_Type_Rate")]
    public class Equip_Type_Rate
    {
        [XmlAttribute(AttributeName = "EquipType")]
        public KE_ITEM_EQUIP_DETAILTYPE EquipType { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Enhance_Value")]
    public class Enhance_Value
    {
        [XmlAttribute(AttributeName = "EnhanceTimes")]
        public int EnhanceTimes { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Equip_StarLevel")]
    public class Equip_StarLevel
    {
        [XmlAttribute(AttributeName = "EQUIP_DETAIL_TYPE")]
        public int EQUIP_DETAIL_TYPE { get; set; }
        [XmlAttribute(AttributeName = "STAR_LEVEL")]
        public int STAR_LEVEL { get; set; }
        [XmlAttribute(AttributeName = "EQUIP_LEVEL_1")]
        public long EQUIP_LEVEL_1 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_2")]
        public long EQUIP_LEVEL_2 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_3")]
        public long EQUIP_LEVEL_3 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_4")]
        public long EQUIP_LEVEL_4 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_5")]
        public long EQUIP_LEVEL_5 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_6")]
        public long EQUIP_LEVEL_6 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_7")]
        public long EQUIP_LEVEL_7 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_8")]
        public long EQUIP_LEVEL_8 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_9")]
        public long EQUIP_LEVEL_9 { get; set; }

        [XmlAttribute(AttributeName = "EQUIP_LEVEL_10")]
        public long EQUIP_LEVEL_10 { get; set; }
    }

    [XmlRoot(ElementName = "Equip_Random_Pos")]
    public class Equip_Random_Pos
    {
        [XmlAttribute(AttributeName = "MAGIC_POS")]
        public int MAGIC_POS { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Strengthen_Value")]
    public class Strengthen_Value
    {
        [XmlAttribute(AttributeName = "StrengthenTimes")]
        public int StrengthenTimes { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Equip_Level")]
    public class Equip_Level
    {
        [XmlAttribute(AttributeName = "Level")]
        public int Level { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }


    public class ComposeItem
    {
        public ItemData nItemMinLevel { get; set; }
        public int nMinLevelRate { get; set; }


        public ItemData nItemMaxLevel { get; set; }

        public int nMaxLevelRate { get; set; }

        public int nFee { get; set; }
    }

    public class CalcProb
    {
        public double nProb { get; set; }

        public long nMoney { get; set; }

        public double nTrueProb { get; set; }
    }
}
