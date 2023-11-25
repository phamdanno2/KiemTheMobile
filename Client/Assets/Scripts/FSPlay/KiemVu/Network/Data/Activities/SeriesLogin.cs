using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FSPlay.GameEngine.Logic;
using ProtoBuf;

namespace Server.Data
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

		private List<KeyValuePair<int, int>> _Items = null;
		/// <summary>
		/// Danh sách vật phẩm
		/// </summary>
		public List<KeyValuePair<int, int>> Items
        {
            get
            {
				if (this._Items != null)
                {
					return this._Items;
                }

				List<KeyValuePair<int, int>> items = new List<KeyValuePair<int, int>>();
				string[] keyPairs = this.GoodsID.Split('|');
				foreach (string keyPair in keyPairs)
                {
                    try
                    {
						string[] para = keyPair.Split(',');
						int itemID = int.Parse(para[0]);
						int number = int.Parse(para[1]);
						items.Add(new KeyValuePair<int, int>(itemID, number));
					}
					catch (Exception) { }
                }
				return items;
            }
        }
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

		/// <summary>
		/// Trả về thông tin phần quà tại mốc có ID tương ứng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Gift GetAward(int id)
        {
			return this.Gift.Where(x => x.ID == id).FirstOrDefault();
		}

		/// <summary>
		/// Trả về trạng thái tại mốc tương ứng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int GetState(int id)
        {
			/// Phần quà mốc tương ứng
			Gift awardInfo = this.GetAward(id);
			/// Nếu không tìm thấy
			if (awardInfo == null)
            {
				return 0;
            }

			/// Nếu chưa đủ ngày
			if (this.LoginNum < awardInfo.TimeOl)
            {
				return 0;
            }

			int value = this.Flag & Global.GetBitValue(id + 1);
			/// Nếu có thể nhận
			if (value == 0)
            {
				return 1;
            }
			/// Nếu đã nhận rồi
			else
            {
				return 2;
            }
		}

		/// <summary>
		/// Có gì có thể nhận được không
		/// </summary>
		public bool HasSomethingToGet
		{
			get
			{
				/// Tổng số mốc
				int count = this.Gift.Count;
				/// Duyệt từng mốc
				for (int i = 1; i <= count; i++)
				{
					/// Phần quà mốc tương ứng
					Gift awardInfo = this.Gift[i - 1];

					/// Trạng thái tương ứng
					int value = this.Flag & Global.GetBitValue(i);
					/// Nếu chưa nhận
					if (value == 0 && this.LoginNum >= awardInfo.TimeOl)
					{
						return true;
					}
				}
				/// Không có gì để nhận
				return false;
			}
		}
	}
}
