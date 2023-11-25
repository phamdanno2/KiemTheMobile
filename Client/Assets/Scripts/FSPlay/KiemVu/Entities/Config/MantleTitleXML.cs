using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Entities.Config
{
    /// <summary>
    /// Danh hiệu phi phong
    /// </summary>
    public class MantleTitleXML
    {
        /// <summary>
        /// Giá trị tài phú nhân vật
        /// </summary>
        public int RoleValue { get; set; }

        /// <summary>
        /// Thời gian thực thi hiệu ứng
        /// </summary>
        public float AnimationSpeed { get; set; }

        /// <summary>
        /// Độ thu phóng so với kích thước gốc
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Đường dẫn file Bundle chứa ảnh
        /// </summary>
        public string BundleDir { get; set; }

        /// <summary>
        /// Đường dẫn file Atlas chứa ảnh
        /// </summary>
        public string AtlasName { get; set; }

        /// <summary>
        /// Danh sách Sprite tạo nên hiệu ứng
        /// </summary>
        public List<string> SpriteNames { get; set; }

        /// <summary>
        /// Chuyển đối tượng từ XMLNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static MantleTitleXML Parse(XElement xmlNode)
        {
            MantleTitleXML mantleTitle = new MantleTitleXML()
            {
                RoleValue = int.Parse(xmlNode.Attribute("RoleValue").Value),
                AnimationSpeed = float.Parse(xmlNode.Attribute("AnimationSpeed").Value),
                Scale = float.Parse(xmlNode.Attribute("Scale").Value),
                BundleDir = xmlNode.Attribute("BundleDir").Value,
                AtlasName = xmlNode.Attribute("AtlasName").Value,
                SpriteNames = new List<string>(),
            };

            foreach (XElement node in xmlNode.Elements("Sprite"))
            {
                string spriteName = node.Attribute("Name").Value;
                mantleTitle.SpriteNames.Add(spriteName);
            }

            return mantleTitle;
        }
    }
}
