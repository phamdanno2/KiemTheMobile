using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ProtoBuf;

namespace GameServer.KiemThe.Core.Activity.LevelUpEvent
{
	[XmlRoot(ElementName = "LevelUpItem")]
	[ProtoContract]
	public class LevelUpItem
	{
		[XmlAttribute(AttributeName = "ID")]
		[ProtoMember(1)]
		public int ID { get; set; }

		[XmlAttribute(AttributeName = "ToLevel")]
		[ProtoMember(2)]
		public int ToLevel { get; set; }

		[XmlAttribute(AttributeName = "LevelUpGift")]
		[ProtoMember(3)]
		public string LevelUpGift { get; set; }
	}

	[XmlRoot(ElementName = "LevelUpGiftConfig")]
	[ProtoContract]
	public class LevelUpGiftConfig
	{

		[XmlElement(ElementName = "LevelUpItem")]
		[ProtoMember(1)]
		public List<LevelUpItem> LevelUpItem { get; set; }

		/// <summary>
		/// Danh sách trạng thái
		/// </summary>
		[ProtoMember(2)]
		public List<int> BitFlags { get; set; }
	}
}
