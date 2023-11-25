using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ProtoBuf;

namespace GameServer.KiemThe.Core.Activity.DaySeriesLoginEvent
{
	[XmlRoot(ElementName = "AwardItem")]
	[ProtoContract]
	public class RollAwardItem
	{
		[XmlAttribute(AttributeName = "ItemID")]
		[ProtoMember(1)]
		public int ItemID { get; set; }

		[XmlAttribute(AttributeName = "Number")]
		[ProtoMember(2)]
		public int Number { get; set; }

		[XmlAttribute(AttributeName = "Rate")]
		[ProtoMember(3)]
		public int Rate { get; set; }
	}

	[XmlRoot(ElementName = "SevenDaysLoginItem")]
	[ProtoContract]
	public class SevenDaysLoginItem
	{
		[XmlElement(ElementName = "RollAwardItem")]
		[ProtoMember(1)]
		public List<RollAwardItem> RollAwardItem { get; set; }

		[XmlAttribute(AttributeName = "ID")]
		[ProtoMember(2)]
		public int ID { get; set; }

		[XmlAttribute(AttributeName = "Days")]
		[ProtoMember(3)]
		public int Days { get; set; }
	}

	[XmlRoot(ElementName = "SevenDaysLogin")]
	[ProtoContract]
	public class SevenDaysLogin
	{
		[XmlElement(ElementName = "Item")]
		[ProtoMember(1)]
		public List<SevenDaysLoginItem> SevenDaysLoginItem { get; set; }
	
		[XmlAttribute(AttributeName = "Name")]
		[ProtoMember(2)]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "IsOpen")]
		[ProtoMember(3)]
		public bool IsOpen { get; set; }

		/// <summary>
		/// Số ngày đã đăng nhập liên tiếp
		/// </summary>
		[ProtoMember(4)]
		public int SeriesLoginNum { get; set; }

		/// <summary>
		/// ID nhận thưởng lần trước
		/// </summary>
		[ProtoMember(5)]
		public int SeriesLoginGetAwardStep { get; set; }

		/// <summary>
		/// Danh sách vật phẩm
		/// </summary>
		[ProtoMember(6)]
		public string SeriesLoginAwardGoodsID { get; set; }
	}
}
