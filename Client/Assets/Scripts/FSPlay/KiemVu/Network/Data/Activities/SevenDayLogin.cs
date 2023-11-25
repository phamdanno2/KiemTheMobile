using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ProtoBuf;

namespace Server.Data
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

		/// <summary>
		/// Có phần quà có thể nhận hôm nay không
		/// </summary>
		public bool HasSomethingToGet
        {
			get
			{
				if (this.SeriesLoginNum <= this.SeriesLoginGetAwardStep + 1)
                {
					return false;
                }
				return true;
            }
        }

		/// <summary>
		/// Thông tin phần quà sẽ nhận được hôm nay
		/// </summary>
		public SevenDaysLoginItem CurrentAwardInfo
        {
            get
            {
				/// Nếu không có quà nhận
				if (!this.HasSomethingToGet)
                {
					return null;
                }

				/// Thông tin quà tương ứng
				SevenDaysLoginItem awardInfo = this.SevenDaysLoginItem.Where(x => x.Days == this.SeriesLoginGetAwardStep + 1).FirstOrDefault();
				/// Trả ra kết quả
				return awardInfo;
            }
		}

		/// <summary>
		/// Trả về ID phần quà đã nhận ở mốc tương ứng
		/// </summary>
		/// <param name="days"></param>
		/// <param name="itemID"></param>
		/// <param name="itemNumber"></param>
		/// <returns></returns>
		public bool GetReceivedAwardItemInfo(int days, out int itemID, out int itemNumber)
		{
			itemID = -1;
			itemNumber = -1;

			string[] infoParams = this.SeriesLoginAwardGoodsID.Split('|');
			try
			{
				string[] para = infoParams[days].Split(',');
				if (para.Length != 2)
				{
					return false;
				}

				itemID = int.Parse(para[0]);
				itemNumber = int.Parse(para[1]);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
