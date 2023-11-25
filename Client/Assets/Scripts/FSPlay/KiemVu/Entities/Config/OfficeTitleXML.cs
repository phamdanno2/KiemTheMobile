using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Entities.Config
{
	/// <summary>
	/// Thông tin danh hiệu quan hàm
	/// </summary>
	public class OfficeTitleXML
	{
		/// <summary>
		/// ID quan hàm
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Thời gian thực thi hiệu ứng danh hiệu
		/// </summary>
		public float AnimationSpeed { get; set; }

		/// <summary>
		/// Độ co dãn danh hiệu
		/// </summary>
		public float Scale { get; set; }

		/// <summary>
		/// Đường dẫn File Bundle chứa ảnh
		/// </summary>
		public string BundleDir { get; set; }

		/// <summary>
		/// Tên Atlas
		/// </summary>
		public string AtlasName { get; set; }

		/// <summary>
		/// ID hiệu ứng
		/// </summary>
		public int EffectID { get; set; }

		/// <summary>
		/// Danh sách Sprite hiệu ứng
		/// </summary>
		public List<string> SpriteNames { get; set; }

		/// <summary>
		/// Chuyển đối tượng từ XMLNode
		/// </summary>
		/// <param name="xmlNode"></param>
		/// <returns></returns>
		public static OfficeTitleXML Parse(XElement xmlNode)
		{
			OfficeTitleXML officeTitle = new OfficeTitleXML()
			{
				ID = int.Parse(xmlNode.Attribute("ID").Value),
				AnimationSpeed = float.Parse(xmlNode.Attribute("AnimationSpeed").Value),
				Scale = float.Parse(xmlNode.Attribute("Scale").Value),
				BundleDir = xmlNode.Attribute("BundleDir").Value,
				AtlasName = xmlNode.Attribute("AtlasName").Value,
				EffectID = int.Parse(xmlNode.Attribute("EffectID").Value),
				SpriteNames = new List<string>(),
			};

			/// Duyệt danh sách Sprite
			foreach (XElement node in xmlNode.Elements("Sprite"))
			{
				officeTitle.SpriteNames.Add(node.Attribute("Name").Value);
			}

			/// Trả về kết quả
			return officeTitle;
		}
	}
}
