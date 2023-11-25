﻿using GameServer.KiemThe.Entities;
using ProtoBuf;
using Server.Data;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{
    [ProtoContract]
    public class ItemGenByteData
    {
        [ProtoMember(1)]
        public int BasicPropCount { get; set; }

        [ProtoMember(2)]
        public List<int> BasicPropValue { get; set; }

        [ProtoMember(3)]
        public bool HaveBookProperties { get; set; }

        [ProtoMember(4)]
        public List<int> BookPropertyValue { get; set; }

        [ProtoMember(5)]
        public int GreenPropCount { get; set; }

        [ProtoMember(6)]
        public List<int> GreenPropValue { get; set; }

        [ProtoMember(7)]
        public int HiddenProbsCount { get; set; }

        [ProtoMember(8)]
        public List<int> HiddenProbsValue { get; set; }
    }

    [XmlRoot(ElementName = "Refine")]
    public class Refine
    {
        [XmlAttribute(AttributeName = "RefineId")]
        public int RefineId { get; set; }

        [XmlAttribute(AttributeName = "SourceItem")]
        public int SourceItem { get; set; }

        [XmlAttribute(AttributeName = "ProduceItem")]
        public int ProduceItem { get; set; }

        [XmlAttribute(AttributeName = "Fee")]
        public int Fee { get; set; }
    }

    public class WaitBeRemove
    {
        public GoodsData _Good { get; set; }

        public int ItemLess { get; set; }
    }

    public class ActiveByItem
    {
        public int Pos1 { get; set; }
        public int Pos2 { get; set; }
    }

    /// <summary>
    /// Classs socket đảm nhận việc kích hoạt hay disable thuộc tính
    /// </summary>
    public class KSOCKET
    {
        public KMagicAttrib sMagicAttrib { get; set; }
        public bool bActive { get; set; }

        public int Index { get; set; }
    };

    [XmlRoot(ElementName = "Stuff")]
    public class Stuff
    {
        [XmlAttribute(AttributeName = "StuffDetail")]
        public int StuffDetail { get; set; }

        [XmlAttribute(AttributeName = "StuffParticular")]
        public int StuffParticular { get; set; }
    }

    [XmlRoot(ElementName = "BookAttr")]
    public class BookAttr
    {
        [XmlAttribute(AttributeName = "StrInitMin")]
        public int StrInitMin { get; set; }

        [XmlAttribute(AttributeName = "StrInitMax")]
        public int StrInitMax { get; set; }

        [XmlAttribute(AttributeName = "DexInitMin")]
        public int DexInitMin { get; set; }

        [XmlAttribute(AttributeName = "DexInitMax")]
        public int DexInitMax { get; set; }

        [XmlAttribute(AttributeName = "VitInitMin")]
        public int VitInitMin { get; set; }

        [XmlAttribute(AttributeName = "VitInitMax")]
        public int VitInitMax { get; set; }

        [XmlAttribute(AttributeName = "EngInitMin")]
        public int EngInitMin { get; set; }

        [XmlAttribute(AttributeName = "EngInitMax")]
        public int EngInitMax { get; set; }

        [XmlAttribute(AttributeName = "SkillID1")]
        public int SkillID1 { get; set; }

        [XmlAttribute(AttributeName = "SkillID2")]
        public int SkillID2 { get; set; }

        [XmlAttribute(AttributeName = "SkillID3")]
        public int SkillID3 { get; set; }

        [XmlAttribute(AttributeName = "SkillID4")]
        public int SkillID4 { get; set; }
    }

    [XmlRoot(ElementName = "ENH")]
    public class ENH
    {
        /// <summary>
        /// Lần cường hóa
        /// </summary>
        [XmlAttribute(AttributeName = "EnhTimes")]
        public int EnhTimes { get; set; }

        /// <summary>
        /// Sysmboy Hiệu ứng
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAName")]
        public string EnhMAName { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA1Min")]
        public int EnhMAPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA1Max")]
        public int EnhMAPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA2Min")]
        public int EnhMAPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA2Max")]
        public int EnhMAPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA3Min")]
        public int EnhMAPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA3Max")]
        public int EnhMAPA3Max { get; set; }

        /// <summary>
        /// Index Số thứ tự của Thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "RiderProp")]
    public class RiderProp
    {
        /// <summary>
        /// Lần cường hóa
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropType")]
        public string RidePropType { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA1Min")]
        public int RidePropPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA1Max")]
        public int RidePropPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA2Min")]
        public int RidePropPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA2Max")]
        public int RidePropPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA3Min")]
        public int RidePropPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA3Max")]
        public int RidePropPA3Max { get; set; }

        /// <summary>
        /// Index Số thứ tự của Thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "Strengthen")]
    public class Strengthen
    {
        /// <summary>
        /// Lần cường hóa
        /// </summary>
        [XmlAttribute(AttributeName = "StrTimes")]
        public int StrTimes { get; set; }

        /// <summary>
        /// Sysmboy Hiệu ứng
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAName")]
        public string StrMAName { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA1Min")]
        public int StrMAPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA1Max")]
        public int StrMAPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA2Min")]
        public int StrMAPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA2Max")]
        public int StrMAPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA3Min")]
        public int StrMAPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "StrMAPA3Max")]
        public int StrMAPA3Max { get; set; }

        /// <summary>
        /// Index Số thứ tự của Thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "SingNetExp")]
    public class SingNetExp
    {
        [XmlAttribute(AttributeName = "Level")]
        public int Level { get; set; }

        [XmlAttribute(AttributeName = "UpgardeExp")]
        public int UpgardeExp { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }
    }

    [XmlRoot(ElementName = "ExtPram")]
    public class ExtPram
    {
        [XmlAttribute(AttributeName = "Pram")]
        public int Pram { get; set; }

        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "Medicine")]
    public class Medicine
    {
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }

        [XmlAttribute(AttributeName = "Time")]
        public int Time { get; set; }

        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "PropMagic")]
    public class PropMagic
    {     /// <summary>
          /// ID của Thuộc tính
          /// </summary>
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        /// <summary>
        /// Level thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "MagicLevel")]
        public int MagicLevel { get; set; }

        /// <summary>
        /// Dòng có kích hoạt hay không
        /// </summary>
        [XmlAttribute(AttributeName = "IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Số thứ tự của dòng
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "ReqProp")]
    public class ReqProp
    {
        /// <summary>
        /// Kiểu yêu cầu
        /// </summary>
        [XmlAttribute(AttributeName = "ReqPropType")]
        public int ReqPropType { get; set; }

        /// <summary>
        /// Giá trị yêu cầu
        /// </summary>
        [XmlAttribute(AttributeName = "ReqPropValue")]
        public int ReqPropValue { get; set; }

        /// <summary>
        /// Thứ tự Của Req
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "BasicProp")]
    public class BasicProp
    {
        /// <summary>
        /// Symboy thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropType")]
        public string BasicPropType { get; set; }

        /// <summary>
        /// Giá trị MIN thứ 1 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA1Min")]
        public int BasicPropPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 1 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA1Max")]
        public int BasicPropPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN thứ 2 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA2Min")]
        public int BasicPropPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 2 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA2Max")]
        public int BasicPropPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN thứ 3 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA3Min")]
        public int BasicPropPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 3 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA3Max")]
        public int BasicPropPA3Max { get; set; }

        /// <summary>
        /// Giá trị INDEX
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public int Index { get; set; }
    }

    [XmlRoot(ElementName = "ItemData")]
    public class ItemData
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [XmlAttribute(AttributeName = "ItemID")]
        public int ItemID { get; set; }

        /// <summary>
        /// Tên trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Loại trang bị DESC
        /// </summary>
        [XmlAttribute(AttributeName = "Kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Phẩm chất
        /// </summary>
        [XmlAttribute(AttributeName = "QualityPrefix")]
        public int QualityPrefix { get; set; }

        /// <summary>
        /// Loại trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "Category")]
        public int Category { get; set; }

        /// <summary>
        /// Thể loại đồ
        /// </summary>
        [XmlAttribute(AttributeName = "Genre")]
        public int Genre { get; set; }

        /// <summary>
        /// Xếp chồng bao nhiêu
        /// </summary>
        [XmlAttribute(AttributeName = "Stack")]
        public int Stack { get; set; }

        /// <summary>
        /// Kiểu chi tiết
        /// </summary>
        [XmlAttribute(AttributeName = "DetailType")]
        public int DetailType { get; set; }

        /// <summary>
        /// Sử dụng để tính toán ID
        /// </summary>
        [XmlAttribute(AttributeName = "ParticularType")]
        public int ParticularType { get; set; }

        /// <summary>
        /// SỬ dụng để phân phẩm chất của vật phẩm | Và itnsh toán ID
        /// </summary>
        [XmlAttribute(AttributeName = "Level")]
        public int Level { get; set; }

        /// <summary>
        /// /Thuộc tính này để quy định ID của bộ
        /// </summary>
        [XmlAttribute(AttributeName = "SuiteID")]
        public int SuiteID { get; set; }

        /// <summary>
        /// Icon của vật phẩm
        /// </summary>
        [XmlAttribute(AttributeName = "Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Khi click vào vật phẩm thì nhìn hình ở góc như thế nào
        /// </summary>
        [XmlAttribute(AttributeName = "View")]
        public string View { get; set; }

        /// <summary>
        /// Khi click vào vật phẩm thì nhìn hình ở góc như thế nào
        /// </summary>
        [XmlAttribute(AttributeName = "Intro")]
        public string Intro { get; set; }

        /// <summary>
        /// Khi click vào vật phẩm thì nhìn hình ở góc như thế nào
        /// </summary>
        [XmlAttribute(AttributeName = "Help")]
        public string Help { get; set; }

        /// <summary>
        /// Object ID
        /// </summary>
        [XmlAttribute(AttributeName = "ObjID")]
        public int ObjID { get; set; }

        /// <summary>
        /// BindType

        /// </summary>
        [XmlAttribute(AttributeName = "BindType")]
        public int BindType { get; set; }

        /// <summary>
        /// NGũ hành của trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "Series")]
        public int Series { get; set; }

        /// <summary>
        /// Yêu cầu level sử dụng
        /// </summary>
        [XmlAttribute(AttributeName = "ReqLevel")]
        public int ReqLevel { get; set; }

        /// <summary>
        /// Yêu cầu level sử dụng
        /// </summary>
        [XmlAttribute(AttributeName = "CDType")]
        public int CDType { get; set; }

        /// <summary>
        /// Giá của vật phẩm
        /// </summary>
        [XmlAttribute(AttributeName = "Price")]
        public int Price { get; set; }

        /// <summary>
        ///  Kiểu tiền
        /// </summary>
        [XmlAttribute(AttributeName = "ItemValue")]
        public int ItemValue { get; set; }

        /// <summary>
        /// Giá tiền phi shop
        /// </summary>
        [XmlAttribute(AttributeName = "MakeCost")]
        public int MakeCost { get; set; }

        /// <summary>
        /// Res Nam
        /// </summary>
        [XmlAttribute(AttributeName = "ResMale")]
        public int ResMale { get; set; }

        /// <summary>
        /// Res Nữ
        /// </summary>

        [XmlAttribute(AttributeName = "ResFemale")]
        public int ResFemale { get; set; }

        /// <summary>
        /// Danh sách thuộc tính cơ bản của ITEM
        /// </summary>
        [XmlElement(ElementName = "ListBasicProp")]
        public List<BasicProp> ListBasicProp { get; set; }

        /// <summary>
        ///  YÊU CẦU NGŨ HÀNH ĐỂ TRANG BỊ
        [XmlElement(ElementName = "ListReqProp")]
        public List<ReqProp> ListReqProp { get; set; }

        /// <summary>
        /// Danh sách thuộc tính cường hóa
        /// </summary>
        [XmlElement(ElementName = "ListEnhance")]
        public List<ENH> ListEnhance { get; set; }

        /// <summary>
        /// Thuộc tính của BOOK
        /// </summary>

        [XmlElement(ElementName = "BookAttr")]
        public BookAttr BookProperty { get; set; }

        [XmlElement(ElementName = "MedicineProp")]
        public List<Medicine> MedicineProp { get; set; }

        /// <summary>
        /// Danh sách thuộc tính dòng xanh
        /// Từ MAGIC1 -> MAGIC3
        /// </summary>
        [XmlElement(ElementName = "GreenProp")]
        public List<PropMagic> GreenProp { get; set; }

        /// <summary>
        /// Danh sách thuộc tính dòng ẩn
        /// TỪ MAGIC 4-> MAGIC 6
        /// </summary>
        [XmlElement(ElementName = "HiddenProp")]
        public List<PropMagic> HiddenProp { get; set; }

        /// <summary>
        /// Danh sách thuộc tính dòng ẩn
        /// TỪ MAGIC 4-> MAGIC 6
        /// </summary>
        [XmlElement(ElementName = "ListExtPram")]
        public List<ExtPram> ListExtPram { get; set; }

        /// <summary>
        /// Thuộc tính kích hoạt khi đủ bộ
        /// </summary>
        [XmlElement(ElementName = "ActiveBySuit")]
        public List<Strengthen> ActiveBySuit { get; set; }

        /// <summary>
        /// Danh sách Thuộc tính của NGỰA
        /// </summary>
        [XmlElement(ElementName = "RiderProp")]
        public List<RiderProp> RiderProp { get; set; }

        /// <summary>
        /// Hạn sử dụng của vật phẩm
        /// </summary>
        [XmlElement(ElementName = "UnLockInterval")]
        public int UnLockInterval { get; set; }

        [XmlAttribute(AttributeName = "FightPower")]
        public int FightPower { get; set; }

        /// <summary>
        /// Có phải vũ khí hoàng kim không
        /// <para>Vũ khí hoàng kim có thể luyện hóa được</para>
        /// </summary>
        [XmlAttribute(AttributeName = "IsArtifact")]
        public bool IsArtifact { get; set; } = false;

        /// <summary>
        /// ID Script điều khiển nếu là vật phẩm
        /// </summary>
        [XmlAttribute(AttributeName = "ScriptID")]
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// Có phải vật phẩm có Script điều khiển không
        /// </summary>
        public bool IsScriptItem
        {
            get
            {
                if (ItemManager.KD_ISEQUIP(this.Genre))
                {
                    return false;
                }

                if (this.Genre == 18 && this.ScriptID != -1)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Xóa vật phẩm sau khi sử dụng
        /// </summary>
        [XmlAttribute(AttributeName = "DeductOnUse")]
        public bool DeductOnUse { get; set; } = false;

        /// <summary>
        /// Có phải thuốc không
        /// </summary>
        public bool IsMedicine
        {
            get
            {
                if (ItemManager.KD_ISEQUIP(this.Genre))
                {
                    return false;
                }

                if (this.Genre == 17)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Có phải thuốc không
        /// </summary>
        [XmlAttribute(AttributeName = "BuffID")]
        public int BuffID { get; set; } = -1;

        /// <summary>
        /// ID kỹ năng trận pháp
        /// </summary>
        [XmlAttribute(AttributeName = "ZhenSkillID")]
        public int ZhenSkillID { get; set; } = -1;

        /// <summary>
        /// ID kỹ năng trận pháp toàn bản đồ
        /// </summary>
        [XmlAttribute(AttributeName = "NearbyZhenSkill")]
        public int NearbyZhenSkill { get; set; } = -1;

        /// <summary>
        /// Quy tắc kích hoạt trận
        /// </summary>
        [XmlAttribute(AttributeName = "ZhenActivateRule")]
        public int ZhenActivateRule { get; set; } = -1;

        /// <summary>
        /// Chuyển đối tượng về dạng String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("ID = {0}, Name = {1}, Value = {2}", this.ItemID, this.Name, this.ItemValue);
        }
    }

    [XmlRoot(ElementName = "ItemRandom")]
    public class ItemRandom
    {
        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; set; }

        [XmlAttribute(AttributeName = "ItemID")]
        public int ItemID { get; set; }

        [XmlAttribute(AttributeName = "Number")]
        public int Number { get; set; }

        [XmlAttribute(AttributeName = "Series")]
        public int Series { get; set; }

        [XmlAttribute(AttributeName = "TimeLimit")]
        public int TimeLimit { get; set; }

        [XmlAttribute(AttributeName = "Rate")]
        public int Rate { get; set; }

        /// <summary>
        /// 0 : Là không khóa | 1 là có
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "Lock")]
        public int Lock { get; set; }
    }

    [XmlRoot(ElementName = "ConfigBox")]
    public class ConfigBox
    {
        [XmlElement(ElementName = "Boxs")]
        public List<RandomBox> Boxs { get; set; }
    }

    [XmlRoot(ElementName = "RandomBox")]
    public class RandomBox
    {
        /// <summary>
        /// Đánh dấu ID hộp
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "IDBox")]
        public int IDBox { get; set; }

        /// <summary>
        /// Đánh dấu tên hộp nào đang mở
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "BoxName")]
        public string BoxName { get; set; }

        /// <summary>
        /// Tổng số RATE
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "TotalRate")]
        public int TotalRate { get; set; }

        /// <summary>
        /// Mở tối đa trên 1 tuần
        /// </summary>
        [XmlAttribute(AttributeName = "LimitWeek")]
        public int LimitWeek { get; set; }

        /// <summary>
        /// Mở tối đa trên ngày
        /// </summary>
        [XmlAttribute(AttributeName = "LimitDay")]
        public int LimitDay { get; set; }

        [XmlElement(ElementName = "Items")]
        public List<ItemRandom> Items { get; set; }
    }

    [XmlRoot(ElementName = "ExpBookLoading")]
    public class ExpBookLoading
    {
        [XmlAttribute(AttributeName = "BookLevel")]
        public int BookLevel { get; set; }

        [XmlAttribute(AttributeName = "Level")]
        public int Level { get; set; }

        [XmlAttribute(AttributeName = "Exp")]
        public int Exp { get; set; }
    }
}