using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using GameServer.Server;
using System.Windows;
using Server.Tools;
using Server.Data;
using Server.TCP;
using Server.Protocol;
using GameServer.Core.Executor;

namespace GameServer.Logic
{
    /// <summary>
    /// 从用户ID管理中删除用户ID
    /// </summary>
    public class DelayUnRegisterUserIDItem
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 开始的时间
        /// </summary>
        public long StartTicks
        {
            get;
            set;
        }

        /// <summary>
        /// 角色所在的服务器ID
        /// </summary>
        public int ServerId;
    }

    /// <summary>
    /// 取消用户ID的注册的管理
    /// </summary>
    public class UnregisterUserIDMgr
    {
        /// <summary>
        /// 等待处理的队列
        /// </summary>
        private static List<DelayUnRegisterUserIDItem> UnRegisterUserIDsList = new List<DelayUnRegisterUserIDItem>();

        /// <summary>
        /// 添加要反注册的用户ID
        /// </summary>
        /// <param name="userID"></param>
        public static void AddUnRegisterUserID(string userID, int serverId)
        {
            lock (UnRegisterUserIDsList)
            {
                UnRegisterUserIDsList.Add(new DelayUnRegisterUserIDItem()
                {
                    UserID = userID,
                    StartTicks = TimeUtil.NOW(),
                    ServerId = serverId,
                });
            }
        }

        /// <summary>
        /// 处理反注册的用户ID
        /// </summary>
        public static void ProcessUnRegisterUserIDsQueue()
        {
            long nowTicks = TimeUtil.NOW();
            DelayUnRegisterUserIDItem item = null;
            lock (UnRegisterUserIDsList)
            {
                if (UnRegisterUserIDsList.Count > 0)
                {
                    if (nowTicks - UnRegisterUserIDsList[0].StartTicks >= (30 * 1000)) //留出30秒钟后给系统处理，提示用户一分钟后才能登录
                    {
                        item = UnRegisterUserIDsList[0];
                        UnRegisterUserIDsList.RemoveAt(0);
                    }
                }
            }

            if (null != item)
            {
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_REGUSERID,
                    string.Format("{0}:{1}:{2}", item.UserID, GameManager.ServerLineID, 0),
                    null, item.ServerId);
            }
        }
    }
}
