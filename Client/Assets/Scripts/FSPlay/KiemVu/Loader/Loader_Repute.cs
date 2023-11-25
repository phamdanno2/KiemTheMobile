using Assets.Scripts.FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Đối tượng chứa danh sách các cấu hình trong game
    /// </summary>
    public static partial class Loader
    {
        #region Danh vọng
        /// <summary>
        /// Danh vọng hệ thống
        /// </summary>
        public static Repute Reputes { get; private set; } = null;

        /// <summary>
        /// Tải danh sách danh vọng hệ thống
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadReputes(XElement xmlNode)
        {
            Loader.Reputes = Repute.Parse(xmlNode);
        }
        #endregion
    }
}
