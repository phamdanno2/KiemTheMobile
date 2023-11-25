using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FSPlay.KiemVu.Entities.Config
{
    /// <summary>
    /// Kích hoạt theo bộ
    /// </summary>
    public class ActiveByItem
    {
        /// <summary>
        /// Bộ 1
        /// </summary>
        public byte Pos1 { get; set; }

        /// <summary>
        /// Bộ 2
        /// </summary>
        public byte Pos2 { get; set; }

    }

    /// <summary>
    /// Thuộc tính sách
    /// </summary>
    [XmlRoot(ElementName = "BookAttr")]
    public class BookAttr
    {
        /// <summary>
        /// Sức Min
        /// </summary>
        [XmlAttribute(AttributeName = "StrInitMin")]
        public short StrInitMin { get; set; }
        
        /// <summary>
        /// Sức Max
        /// </summary>
        [XmlAttribute(AttributeName = "StrInitMax")]
        public short StrInitMax { get; set; }

        /// <summary>
        /// Thân Min
        /// </summary>
        [XmlAttribute(AttributeName = "DexInitMin")]
        public short DexInitMin { get; set; }

        /// <summary>
        /// Thân Max
        /// </summary>
        [XmlAttribute(AttributeName = "DexInitMax")]
        public short DexInitMax { get; set; }

        /// <summary>
        /// Ngoại Min
        /// </summary>
        [XmlAttribute(AttributeName = "VitInitMin")]
        public short VitInitMin { get; set; }

        /// <summary>
        /// Ngoại Max
        /// </summary>
        [XmlAttribute(AttributeName = "VitInitMax")]
        public short VitInitMax { get; set; }

        /// <summary>
        /// Nội Min
        /// </summary>
        [XmlAttribute(AttributeName = "EngInitMin")]
        public short EngInitMin { get; set; }

        /// <summary>
        /// Nội Max
        /// </summary>
        [XmlAttribute(AttributeName = "EngInitMax")]
        public short EngInitMax { get; set; }

        /// <summary>
        /// ID kỹ năng 1
        /// </summary>
        [XmlAttribute(AttributeName = "SkillID1")]
        public short SkillID1 { get; set; }

        /// <summary>
        /// ID kỹ năng 2
        /// </summary>
        [XmlAttribute(AttributeName = "SkillID2")]
        public short SkillID2 { get; set; }

        /// <summary>
        /// ID kỹ năng 3
        /// </summary>
        [XmlAttribute(AttributeName = "SkillID3")]
        public short SkillID3 { get; set; }

        /// <summary>
        /// ID kỹ năng 4
        /// </summary>
        [XmlAttribute(AttributeName = "SkillID4")]
        public short SkillID4 { get; set; }
    }

    /// <summary>
    /// Thuộc tính cường hóa
    /// </summary>
    [XmlRoot(ElementName = "ENH")]
    public class ENH
    {
        /// <summary>
        /// Lần cường hóa
        /// </summary>
        [XmlAttribute(AttributeName = "EnhTimes")]
        public byte EnhTimes { get; set; }

        /// <summary>
        /// Sysmboy Hiệu ứng
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAName")]
        public string EnhMAName { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA1Min")]
        public short EnhMAPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA1Max")]
        public short EnhMAPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA2Min")]
        public short EnhMAPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA2Max")]
        public short EnhMAPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA3Min")]
        public short EnhMAPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "EnhMAPA3Max")]
        public short EnhMAPA3Max { get; set; }
    }

    /// <summary>
    /// Thuộc tính thú cưỡi
    /// </summary>
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
        public short RidePropPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 1
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA1Max")]
        public short RidePropPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA2Min")]
        public short RidePropPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 2
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA2Max")]
        public short RidePropPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA3Min")]
        public short RidePropPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX của thuộc tính cường hóa 3
        /// </summary>
        [XmlAttribute(AttributeName = "RidePropPA3Max")]
        public short RidePropPA3Max { get; set; }
    }

    /// <summary>
    /// Thuộc tính thuốc
    /// </summary>
    [XmlRoot(ElementName = "Medicine")]
    public class Medicine
    {
        /// <summary>
        /// Tên thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        /// <summary>
        /// Giá trị thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "Value")]
        public short Value { get; set; }

    }

    /// <summary>
    /// Giá trị thuộc tính đồ
    /// </summary>
    [XmlRoot(ElementName = "PropMagic")]
    public class PropMagic
    {   /// <summary>
        /// ID của Thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }

        /// <summary>
        /// Level thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "MagicLevel")]
        public byte MagicLevel { get; set; }

        /// <summary>
        /// Dòng có kích hoạt hay không
        /// </summary>
        [XmlAttribute(AttributeName = "IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Số thứ tự của dòng
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public byte Index { get; set; }
    }

    /// <summary>
    /// Yêu cầu
    /// </summary>
    [XmlRoot(ElementName = "ReqProp")]
    public class ReqProp
    {
        /// <summary>
        /// Kiểu yêu cầu
        /// </summary>
        [XmlAttribute(AttributeName = "ReqPropType")]
        public byte ReqPropType { get; set; }

        /// <summary>
        /// Giá trị yêu cầu
        /// </summary>
        [XmlAttribute(AttributeName = "ReqPropValue")]
        public short ReqPropValue { get; set; }
    }

    /// <summary>
    /// Thuộc tính cơ bản
    /// </summary>
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
        public short BasicPropPA1Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 1 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA1Max")]
        public short BasicPropPA1Max { get; set; }

        /// <summary>
        /// Giá trị MIN thứ 2 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA2Min")]
        public short BasicPropPA2Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 2 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA2Max")]
        public short BasicPropPA2Max { get; set; }

        /// <summary>
        /// Giá trị MIN thứ 3 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA3Min")]
        public short BasicPropPA3Min { get; set; }

        /// <summary>
        /// Giá trị MAX thứ 3 của thuộc tính
        /// </summary>
        [XmlAttribute(AttributeName = "BasicPropPA3Max")]
        public short BasicPropPA3Max { get; set; }

        /// <summary>
        /// Giá trị INDEX
        /// </summary>
        [XmlAttribute(AttributeName = "Index")]
        public sbyte Index { get; set; }
    }

    /// <summary>
    /// Đối tượng vật phẩm
    /// </summary>
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
        /// Đường dẫn file Bundle chứa Icon
        /// </summary>
        [XmlAttribute(AttributeName = "IconBundleDir")]
        public string IconBundleDir { get; set; }

        /// <summary>
        /// Tên Atlas trong Bundle chứa Icon
        /// </summary>
        [XmlAttribute(AttributeName = "IconAtlasName")]
        public string IconAtlasName { get; set; }

        /// <summary>
        /// Đường dẫn file Bundle chứa Sprite ở Map
        /// </summary>
        [XmlAttribute(AttributeName = "MapSpriteBundleDir")]
        public string MapSpriteBundleDir { get; set; }

        /// <summary>
        /// Tên Atlas chứa Sprite ở Map
        /// </summary>
        [XmlAttribute(AttributeName = "MapSpriteAtlasName")]
        public string MapSpriteAtlasName { get; set; }

        /// <summary>
        /// Loại trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "Category")]
        public sbyte Category { get; set; }

        /// <summary>
        /// Thể loại đồ
        /// </summary>
        [XmlAttribute(AttributeName = "Genre")]
        public sbyte Genre { get; set; }

        /// <summary>
        /// Loại trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "DetailType")]
        public short DetailType { get; set; }

        /// <summary>
        /// Sử dụng để tính toán ID
        /// </summary>
        [XmlAttribute(AttributeName = "ParticularType")]
        public short ParticularType { get; set; }

        /// <summary>
        /// SỬ dụng để phân phẩm chất của vật phẩm | Và itnsh toán ID
        /// </summary>
        [XmlAttribute(AttributeName = "Level")]
        public byte Level { get; set; }

        /// <summary>
        /// /Thuộc tính này để quy định ID của bộ
        /// </summary>
        [XmlAttribute(AttributeName = "SuiteID")]
        public sbyte SuiteID { get; set; }

        /// <summary>
        /// Icon của vật phẩm
        /// </summary>
        [XmlAttribute(AttributeName = "Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Sprite ở Map
        /// </summary>
        [XmlAttribute(AttributeName = "View")]
        public string View { get; set; }

        /// <summary>
        /// Khi click vào vật phẩm thì nhìn hình ở góc như thế nào
        /// </summary>
        [XmlAttribute(AttributeName = "Intro")]
        public string Intro { get; set; }

        /// <summary>
        /// Khóa hay không
        /// </summary>
        [XmlAttribute(AttributeName = "BindType")]
        public sbyte BindType { get; set; }

        /// <summary>
        /// Ngũ hành của trang bị
        /// </summary>
        [XmlAttribute(AttributeName = "Series")]
        public sbyte Series { get; set; }

        /// <summary>
        /// Giá của vật phẩm (Thừa)
        /// </summary>
        [XmlAttribute(AttributeName = "Price")]
        public int Price { get; set; }

        /// <summary>
        ///  Tài phú
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
        public sbyte ResMale { get; set; }

        /// <summary>
        /// Res Nữ
        /// </summary>
        [XmlAttribute(AttributeName = "ResFemale")]
        public sbyte ResFemale { get; set; }

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

        /// <summary>
        /// Thuộc tính thuốc
        /// </summary>
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
        /// Danh sách Thuộc tính của NGỰA
        /// </summary>
        [XmlElement(ElementName = "RiderProp")]
        public List<RiderProp> RiderProp { get; set; }


        [XmlAttribute(AttributeName = "FightPower")]
        public short FightPower { get; set; }

        /// <summary>
        /// Có phải vũ khí hoàng kim không
        /// <para>Vũ khí hoàng kim có thể luyện hóa được</para>
        /// </summary>
        [XmlAttribute(AttributeName = "IsArtifact")]
        public bool IsArtifact { get; set; } = false;

        /// <summary>
        /// Có phải trang bị không
        /// </summary>
        public bool IsEquip
        {
            get
            {
                return KTGlobal.IsItemEquip(this.Genre);
            }
        }

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
                if (this.IsEquip)
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
        /// Có phải thuốc không
        /// </summary>
        public bool IsMedicine
        {
            get
            {
                if (this.IsEquip)
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
        /// Chuyển đối tượng về dạng String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("ID = {0}, Name = {1}, Value = {2}", this.ItemID, this.Name, this.ItemValue);
        }
    }

    /// <summary>
    /// Thông tin cường hóa Ngũ Hành Ấn
    /// </summary>
    [XmlRoot(ElementName = "SingNetExp")]
    public class SingNetExp
    {
        /// <summary>
        /// Cấp độ
        /// </summary>
        [XmlAttribute(AttributeName = "Level")]
        public short Level { get; set; }

        /// <summary>
        /// Lượng Exp cần
        /// </summary>
        [XmlAttribute(AttributeName = "UpgardeExp")]
        public int UpgardeExp { get; set; }

        /// <summary>
        /// Giá trị tài phú có được
        /// </summary>
        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }

        /// <summary>
        /// Chuyển đối tượng từ XMLNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static SingNetExp Parse(XElement xmlNode)
        {
            return new SingNetExp()
            {
                Level = short.Parse(xmlNode.Attribute("Level").Value),
                UpgardeExp = int.Parse(xmlNode.Attribute("UpgardeExp").Value),
                Value = int.Parse(xmlNode.Attribute("Value").Value),
            };
        }
    }
}
