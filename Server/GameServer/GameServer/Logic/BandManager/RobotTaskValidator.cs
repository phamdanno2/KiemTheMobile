using System.Collections.Generic;
using Tmsk.Tools.Tools;

namespace GameServer.Logic
{
    internal class RobotTaskValidator
    {
        private RobotTaskValidator()
        {
        }

        private static RobotTaskValidator instance = new RobotTaskValidator();

        public static RobotTaskValidator getInstance()
        {
            return instance;
        }

        private object m_Mutex = new object();

        #region 加密初始，记录进程，配置

        public bool Initialize(bool client, int seed, int randomCount, string pubKey)
        {
            return true;
        }

        public bool LoadRobotTaskData()
        {
            return true;
        }

        #endregion 加密初始，记录进程，配置

        public void RobotDataReset(KPlayer client)
        {
            if (client == null) return;
        }

        public string GetIp(KPlayer client)
        {
            int _canLogIp = 1;
            string ip = "0";
            switch (_canLogIp)
            {
                case 1:
                    ip = Global.GetIPAddress(client.ClientSocket);
                    break;

                case 2:
                    string ipStr = Global.GetIPAddress(client.ClientSocket);
                    ip = IpHelper.IpToInt(ipStr).ToString();
                    break;
            }

            return ip;
        }
    }
}