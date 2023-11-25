using GameServer.KiemThe.Entities.Player;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.KiemThe.Core.Title
{
	/// <summary>
	/// Quản lý danh hiệu của Game
	/// </summary>
	public static class KTTitleManager
	{
		/// <summary>
		/// Danh sách danh hiệu
		/// </summary>
		private static Dictionary<int, KTitleXML> Titles = new Dictionary<int, KTitleXML>();

		/// <summary>
		/// Khởi tạo dữ liệu
		/// </summary>
		public static void Init()
		{
			/// Đối tượng XML tương ứng đọc từ File
			XElement xmlNode = Global.GetGameResXml("Config/KT_Title/Title.xml");
			/// Duyệt danh sách
			foreach (XElement node in xmlNode.Elements("Title"))
			{
				KTitleXML titleData = KTitleXML.Parse(node);
				KTTitleManager.Titles[titleData.ID] = titleData;
			}
		}

		/// <summary>
		/// Trả về thông tin danh hiệu tương ứng
		/// </summary>
		/// <param name="titleID"></param>
		/// <returns></returns>
		public static KTitleXML GetTitleData(int titleID)
		{
			if (KTTitleManager.Titles.TryGetValue(titleID, out KTitleXML titleData))
			{
				return titleData;
			}
			return null;
		}

		/// <summary>
		/// Kiểm tra danh hiệu tương ứng có tồn tại không
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool IsTitleExist(int id)
		{
			return KTTitleManager.Titles.ContainsKey(id);
		}
	}
}
