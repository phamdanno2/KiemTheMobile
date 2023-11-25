using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.KiemThe.Logic.Manager
{
    /// <summary>
    /// Đối tượng quản lý khu vực động
    /// </summary>
    public static partial class KTBotManager
    {
        /// <summary>
        /// Đối tượng sử dụng khóa LOCK
        /// </summary>
        private static readonly object Mutex = new object();

        /// <summary>
        /// Danh sách BOT
        /// </summary>
        private static readonly Dictionary<int, KTBot> bots = new Dictionary<int, KTBot>();

        /// <summary>
        /// Tìm BOT có ID tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static KTBot FindBot(int id)
        {
            lock (KTBotManager.bots)
            {
                if (KTBotManager.bots.TryGetValue(id, out KTBot bot))
                {
                    return bot;
                }
                return null;
            }
        }
    }
}
