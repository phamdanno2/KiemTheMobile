using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Entities.Config
{
    /// <summary>
    /// Đối tượng quản lý Res đạn
    /// </summary>
    public class BulletActionSetXML
    {
        /// <summary>
        /// Đường dẫn file Bundle chứa âm thanh hiệu ứng đạn
        /// </summary>
        public string SoundBundleDir { get; set; }

        /// <summary>
        /// Đối tượng dữ liệu Res đạn
        /// </summary>
        public class BulletResData
        {
            /// <summary>
            /// ID Res
            /// </summary>
            public int ResID { get; set; }

            /// <summary>
            /// Tên Res
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Đường dẫn file Bundle chứa ảnh hiệu ứng đạn
            /// </summary>
            public string BundleDir { get; set; }

            /// <summary>
            /// Có động tác bay
            /// </summary>
            public bool HasFlyAction { get; set; }

            /// <summary>
            /// Có động tác tan
            /// </summary>
            public bool HasFadeOutAction { get; set; }

            /// <summary>
            /// Có hiệu ứng nổ
            /// </summary>
            public bool HasExplodeAction { get; set; }

            /// <summary>
            /// Sử dụng 18 hướng không
            /// </summary>
            public bool Use16Dir { get; set; }

            /// <summary>
            /// Tự động quay (nếu sử dụng 16 hướng thì lựa chọn này không có tác dụng)
            /// </summary>
            public bool AutoRotate { get; set; }

            /// <summary>
            /// Sử dụng hiệu ứng tạo bóng di chuyển
            /// </summary>
            public bool UseTrailEffect { get; set; }
            
            /// <summary>
            /// Thời gian duy trì bóng
            /// </summary>
            public float TrailDuration { get; set; }

            /// <summary>
            /// Thời gian giãn cách mỗi lần tạo bóng
            /// </summary>
            public float TrailPeriod { get; set; }

            /// <summary>
            /// Chuyển đối tượng từ XMLNode
            /// </summary>
            /// <param name="xmlNode"></param>
            /// <returns></returns>
            public static BulletResData Parse(XElement xmlNode)
			{
                return new BulletResData()
                {
                    ResID = int.Parse(xmlNode.Attribute("ID").Value),
                    Name = xmlNode.Attribute("Name").Value,
                    BundleDir = xmlNode.Attribute("BundleDir").Value,
                    HasFlyAction = int.Parse(xmlNode.Attribute("HasFlyAction").Value) == 1,
                    HasFadeOutAction = int.Parse(xmlNode.Attribute("HasFadeOutAction").Value) == 1,
                    HasExplodeAction = int.Parse(xmlNode.Attribute("HasExplodeAction").Value) == 1,
                    Use16Dir = int.Parse(xmlNode.Attribute("Use16Dir").Value) == 1,
                    AutoRotate = int.Parse(xmlNode.Attribute("AutoRotate").Value) == 1,
                    UseTrailEffect = int.Parse(xmlNode.Attribute("UseTrailEffect").Value) == 1,
                    TrailDuration = float.Parse(xmlNode.Attribute("TrailDuration").Value),
                    TrailPeriod = float.Parse(xmlNode.Attribute("TrailPeriod").Value),
                };
            }
        }

        /// <summary>
        /// Danh sách hiệu ứng theo ID Res
        /// </summary>
        public Dictionary<int, BulletResData> ResDatas { get; set; }

        /// <summary>
        /// Chuyển đối tượng từ XML Node
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static BulletActionSetXML Parse(XElement xmlNode)
        {
            BulletActionSetXML actionSetXML = new BulletActionSetXML()
            {
                SoundBundleDir = xmlNode.Element("Sound").Attribute("BundleDir").Value,
                ResDatas = new Dictionary<int, BulletResData>(),
            };

            foreach (XElement node in xmlNode.Elements("Bullet"))
            {
                BulletResData resData = BulletResData.Parse(node);
                actionSetXML.ResDatas[resData.ResID] = resData;
            }

            return actionSetXML;
        }
    }
}
