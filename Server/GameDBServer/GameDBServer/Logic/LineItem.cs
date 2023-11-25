using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDBServer.Server;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 线路项
    /// </summary>
    public class LineItem
    {
        private int _LineID = 0;

        /// <summary>
        /// 线路ID
        /// </summary>
        public int LineID
        {
            get { lock (this) { return _LineID; } }
            set { lock (this) { _LineID = value; } }
        }

        private string _GameServerIP = "";

        /// <summary>
        /// 游戏服务器IP
        /// </summary>
        public string GameServerIP
        {
            get { lock (this) { return _GameServerIP; } }
            set { lock (this) { _GameServerIP = value; } }
        }

        private int _GameServerPort = 0;

        /// <summary>
        /// 游戏服务器端口
        /// </summary>
        public int GameServerPort
        {
            get { lock (this) { return _GameServerPort; } }
            set { lock (this) { _GameServerPort = value; } }
        }

        private int _OnlineCount = 0;

        /// <summary>
        /// 在线人数
        /// </summary>
        public int OnlineCount
        {
            get { lock (this) { return _OnlineCount; } }
            set { lock (this) { _OnlineCount = value; } }
        }

        private String _MapOnlineNum = "";

        /// <summary>
        /// 地图在线人数
        /// </summary>
        public String MapOnlineNum
        {
            get { lock (this) { return _MapOnlineNum; } }
            set { lock (this) { _MapOnlineNum = value; } }
        }

        private long _OnlineTicks = 0;

        /// <summary>
        /// 心跳时间
        /// </summary>
        public long OnlineTicks
        {
            get { lock (this) { return _OnlineTicks; } }
            set { lock (this) { _OnlineTicks = value; } }
        }

        /// <summary>
        /// 服务器连接对象
        /// </summary>
        public GameServerClient ServerClient;
    }
}
