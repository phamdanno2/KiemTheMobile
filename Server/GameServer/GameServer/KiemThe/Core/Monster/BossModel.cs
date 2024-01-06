using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Data
{

    [XmlRoot(ElementName = "BossModel")]
    public class BossModel
    {
        public List<RespanBoss> TotalBoss { get; set; }
    }
    /// <summary>
    /// Respan Boss
    /// </summary>
    ///

    [XmlRoot(ElementName = "Postion")]
    public class Postion
    {
        [XmlAttribute(AttributeName = "MapName")]
        public string MapName { get; set; }

        [XmlAttribute(AttributeName = "MapCode")]
        public int MapCode { get; set; }

        [XmlAttribute(AttributeName = "PosX")]
        public int PosX { get; set; }

        [XmlAttribute(AttributeName = "PosY")]
        public int PosY { get; set; }

    }
    [XmlRoot(ElementName = "RespanBoss")]
    public class RespanBoss
    {   /// <summary>
        /// Id boss
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "BossID")]
        public int BossID { get; set; }
        /// <summary>
        /// Tên boss
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "BossName")]
        public string BossName { get; set; }
        /// <summary>
        /// Cấp độ boss
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "BossLevel")]
        public int BossLevel { get; set; }
        /// <summary>
        /// Thời gian ra boss tách = | thời gian
        /// </summary>
        ///
        [XmlAttribute(AttributeName = "TimePoint")]
        public string TimePoint { get; set; }

        /// <summary>
        /// Map code random sẽ ra | Boss sẽ chọn random các map này để lên
        /// </summary>
        ///
        [XmlElement(ElementName = "ListMap")]
        public List<Postion> ListMap { get; set; }


        [XmlIgnore]
        public List<TimeRespan> _Time = new List<TimeRespan>();


        [XmlIgnore]
        public List<TimeRespan> _TimeRespan {

            get{

                if (_Time.Count == 0)
                {

                    string[] Time = TimePoint.Split('|');

                    foreach (string _time in Time)
                    {
                        int Hour = int.Parse(_time.Split(':')[0]);
                        int Min = int.Parse(_time.Split(':')[1]);
                        TimeRespan _Create = new TimeRespan();
                        _Create.Hour = Hour;
                        _Create.Min = Min;
                        _Create.IsRespan = 0;

                        _Time.Add(_Create);

                    }

                }

                return _Time;


            }

        }


    }


    public class TimeRespan
    {
        public int Hour { get; set; }

        public int Min { get; set; }

        public int IsRespan { get; set; }
    }
}
