using GameServer.Logic;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Activity.DownloadBouns
{
    public class DownloadBounsManager
    {
        public static BonusDownload _ConfigBounds = new BonusDownload();

        public static string BonusDownload_XML = "Config/KT_Activity/KTBonusDownload.xml";

        public static void Setup()
        {
            string Files = Global.GameResPath(BonusDownload_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(BonusDownload));

                _ConfigBounds = serializer.Deserialize(stream) as BonusDownload;
            }
        }



    }
}
