using FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Đối tượng chứa danh sách các cấu hình trong game
    /// </summary>
    public static partial class Loader
    {
        #region Danh hiệu đặc biệt
        #region Phi phong
        /// <summary>
        /// Danh sách danh hiệu Phi Phong
        /// </summary>
        public static List<MantleTitleXML> MantleTitles { get; private set; } = new List<MantleTitleXML>();

        /// <summary>
        /// Đọc dữ liệu từ file AnimatedTitle/MantleTitle.xml trong Bundle
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadMantleTitles(XElement xmlNode)
        {
            Loader.MantleTitles.Clear();

            foreach (XElement node in xmlNode.Elements("Title"))
            {
                MantleTitleXML mantleTitle = MantleTitleXML.Parse(node);
                Loader.MantleTitles.Add(mantleTitle);
            }
        }
        #endregion

        #region Quan hàm
        /// <summary>
        /// Danh sách danh hiệu quan hàm
        /// </summary>
        public static List<OfficeTitleXML> OfficeTitles { get; private set; } = new List<OfficeTitleXML>();

        /// <summary>
        /// Đọc dữ liệu từ file AnimatedTitle/OfficeTitle.xml trong Bundle
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadOfficeTitles(XElement xmlNode)
		{
            Loader.OfficeTitles.Clear();

            foreach (XElement node in xmlNode.Elements("Rank"))
			{
                OfficeTitleXML officeTitle = OfficeTitleXML.Parse(node);
                Loader.OfficeTitles.Add(officeTitle);
            }
        }
        #endregion
        #endregion

        #region Hệ thống danh hiệu cá nhân
        /// <summary>
        /// Danh sách danh hiệu cá nhân
        /// </summary>
        public static Dictionary<int, KTitleXML> RoleTitles { get; } = new Dictionary<int, KTitleXML>();

        /// <summary>
        /// Tải dữ liệu Title.xml
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadRoleTitles(XElement xmlNode)
		{
            Loader.RoleTitles.Clear();
            /// Duyệt danh sách
            foreach (XElement node in xmlNode.Elements("Title"))
			{
                KTitleXML titleInfo = KTitleXML.Parse(node);
                Loader.RoleTitles[titleInfo.ID] = titleInfo;
			}
		}
		#endregion
	}
}
