using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Server.Protocol;
using Server.Tools;
////using System.Windows.Forms;
using GameDBServer.DB;
using Server.Data;
using ProtoBuf;
using GameDBServer.Logic;


namespace GameDBServer.Logic
{
    /// <summary>
    /// 在线用户数记录
    /// </summary>
    public class OnlineUserNumMgr
    {
        #region 定时写数据库的操作

        #region 基础变量

        /// <summary>
        /// 上次写入数据库的时间
        /// </summary>
        private static long LastWriteDBTicks = DateTime.Now.Ticks / 10000;

        #endregion 基础变量

        #region 访问方法

        /// <summary>
        /// 将当前的在线人数写入数据库中
        /// </summary>
        /// <param name="dbMgr"></param>
        public static void WriteTotalOnlineNumToDB(DBManager dbMgr)
        {
            DateTime dateTime = DateTime.Now;
            long nowTicks = dateTime.Ticks / 10000;
            if (nowTicks - LastWriteDBTicks < (2L * 60L * 1000L))
            {
                return;
            }

            LastWriteDBTicks = nowTicks;

            //获取所有线路总的在线人数
            int totalNum = LineManager.GetTotalOnlineNum();
            String strMapOnlineInfo = LineManager.GetMapOnlineNum();

            //添加一个新的在线人数记录
            DBWriter.AddNewOnlineNumItem(dbMgr, totalNum, dateTime, strMapOnlineInfo);

            //GameDBManager.DBEventsWriter.CacheOnlineuer(-1,
            //    GameDBManager.ZoneID,
            //    totalNum,
            //    dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        #endregion 访问方法

        #endregion 定时写数据库的操作

        #region 实时通知在线人数的操作

        #region 基础变量

        /// <summary>
        /// 上次写入数据库的时间
        /// </summary>
        private static long LastNotifyDBTicks = DateTime.Now.Ticks / 10000;

        #endregion 基础变量

        #region 访问方法

        /// <summary>
        /// 将当前的在线人数写入通知统计服务器
        /// </summary>
        /// <param name="dbMgr"></param>
        public static void NotifyTotalOnlineNumToServer(DBManager dbMgr)
        {
            DateTime dateTime = DateTime.Now;
            long nowTicks = dateTime.Ticks / 10000;
            if (nowTicks - LastNotifyDBTicks < (30L * 1000L))
            {
                return;
            }

            LastNotifyDBTicks = nowTicks;

            //获取所有线路总的在线人数
            int totalNum = LineManager.GetTotalOnlineNum();

            //GameDBManager.DBEventsWriter.CacheRecentonline(-1,
            //    GameDBManager.ZoneID,
            //    totalNum,
            //    dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        #endregion 访问方法

        #endregion 实时通知在线人数的操作
    }
}
