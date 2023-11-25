using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FSPlay.KiemVu.Entities.Config
{
    [XmlRoot(ElementName = "MagicAttribLevel")]
    public class MagicAttribLevel
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Suffix")]
        public string Suffix { get; set; }
        [XmlAttribute(AttributeName = "IsDarkness")]
        public int IsDarkness { get; set; }
        [XmlAttribute(AttributeName = "Level")]
        public int Level { get; set; }



        [XmlAttribute(AttributeName = "MagicName")]
        public string MagicName { get; set; }
        [XmlAttribute(AttributeName = "MA1Min")]
        public int MA1Min { get; set; }
        [XmlAttribute(AttributeName = "MA1Max")]
        public int MA1Max { get; set; }
        [XmlAttribute(AttributeName = "MA2Min")]
        public int MA2Min { get; set; }
        [XmlAttribute(AttributeName = "MA2Max")]
        public int MA2Max { get; set; }
        [XmlAttribute(AttributeName = "MA3Min")]
        public int MA3Min { get; set; }
        [XmlAttribute(AttributeName = "MA3Max")]
        public int MA3Max { get; set; }
        [XmlAttribute(AttributeName = "ReqLevel")]
        public double ReqLevel { get; set; }
        [XmlAttribute(AttributeName = "ItemValue")]
        public int ItemValue { get; set; }
        [XmlAttribute(AttributeName = "Series")]
        public int Series { get; set; }



    }
}
