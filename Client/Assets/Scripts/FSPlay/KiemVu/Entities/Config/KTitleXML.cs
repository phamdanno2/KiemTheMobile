using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Entities.Config
{
	/// <summary>
	/// Định nghĩa danh hiệu của người chơi
	/// </summary>
	public class KTitleXML
	{
		/// <summary>
		/// ID danh hiệu
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Tên danh hiệu
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Mô tả danh hiệu
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Thời gian tồn tại (giờ)
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		/// Chuyển đối tượng từ XMLNode
		/// </summary>
		/// <param name="xmlNode"></param>
		/// <returns></returns>
		public static KTitleXML Parse(XElement xmlNode)
		{
			return new KTitleXML()
			{
				ID = int.Parse(xmlNode.Attribute("ID").Value),
				Text = xmlNode.Attribute("Text").Value,
				Description = xmlNode.Attribute("Description").Value,
				Duration = int.Parse(xmlNode.Attribute("Duration").Value),
			};
		}
	}
}

