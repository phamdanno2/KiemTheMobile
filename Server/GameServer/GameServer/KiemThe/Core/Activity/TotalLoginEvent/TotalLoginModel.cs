using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using GameServer.Logic;
using ProtoBuf;

namespace GameServer.KiemThe.Core.Activity.TotalLoginEvent
{
	[XmlRoot(ElementName = "Gift")]
	[ProtoContract]
	public class Gift
	{
		[XmlAttribute(AttributeName = "ID")]
		[ProtoMember(1)]
		public int ID { get; set; }

		[XmlAttribute(AttributeName = "TimeOl")]
		[ProtoMember(2)]
		public int TimeOl { get; set; }

		[XmlAttribute(AttributeName = "GoodsID")]
		[ProtoMember(3)]
		public string GoodsID { get; set; }
	}

	[XmlRoot(ElementName = "TotalDayLoginSeries")]
	[ProtoContract]
	public class TotalDayLoginSeries
	{
		[XmlElement(ElementName = "Gift")]
		[ProtoMember(1)]
		public List<Gift> Gift { get; set; }
		
		[XmlAttribute(AttributeName = "Name")]
		[ProtoMember(2)]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "IsOpen")]
		[ProtoMember(3)]
		public bool IsOpen { get; set; }

		/// <summary>
		/// Tổng số ngày đã LOGIN
		/// </summary>
		[ProtoMember(4)]
		public int LoginNum { get; set; }

		/// <summary>
		/// Trạng thái
		/// </summary>
		[ProtoMember(5)]
		public int Flag { get; set; }
	}
}
