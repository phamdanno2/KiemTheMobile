using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDBServer.Logic;
using GameDBServer.Server;
using Server.Protocol;
using Server.Tools;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 聊天发言限制管理
    /// </summary>
    public class BanChatManager
    {
        #region 基础数据

        /// <summary>
        /// 线程锁
        /// </summary>
        private static object _Mutex = new object();

        /// <summary>
        /// 禁止聊天发言的词典
        /// </summary>
        private static Dictionary<string, long> _BanChatDict = new Dictionary<string,long>();

        #endregion 基础数据

        #region 基础操作

        /// <summary>
        /// 获取禁止聊天发言的词典发送tcp对象
        /// </summary>
        public static TCPOutPacket GetBanChatDictTCPOutPacket(TCPOutPacketPool pool, int cmdID)
        {
            TCPOutPacket tcpOutPacket = null;
            lock (_Mutex)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Dictionary<string, long>>(_BanChatDict, pool, cmdID);
            }

            return tcpOutPacket;
        }

        /// <summary>
        /// 添加禁止发言的角色名称到字典中
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="banHours"></param>
        public static void AddBanRoleName(string roleName, int banHours)
        {
            lock (_Mutex)
            {
                if (null == _BanChatDict)
                {
                    _BanChatDict = new Dictionary<string, long>();
                }

                if (banHours > 0)
                {
                    _BanChatDict[roleName] = (DateTime.Now.Ticks / 10000) + (banHours * 60 * 60 * 1000);
                }
                else
                {
                    _BanChatDict.Remove(roleName);
                }
            }
        }

        #endregion 基础操作
    }
}
