using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server.Tools;
using GameDBServer.DB;
using MySQLDriverCS;

namespace GameDBServer.Logic.Name
{
    /// <summary>
    /// 曾经使用过的名字
    /// A改名为B，则A会存到这里，以后任何角色都不能再起名为A
    /// 数据库表 t_had_used_name，合区的时候要处理
    /// </summary>
    public class NameUsedMgr : SingletonTemplate<NameUsedMgr>
    {
        private NameUsedMgr() {  }

        private HashSet<string> cannotUse = new HashSet<string>();
        private HashSet<string> cannotUse_BangHui = new HashSet<string>();

        /// <summary>
        /// 从数据库加载曾经使用过的名字, 这些名字是不允许被使用的
        /// </summary>
        /// <param name="dbMgr"></param>
        public void LoadFromDatabase(DBManager dbMgr)
        {
            MySQLConnection conn = null;

            // 旧角色名
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT oldname FROM t_change_name");

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                try
                {
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    lock (cannotUse)
                    {
                        cannotUse.Clear();
                        while (reader.Read())
                        {
                            string name = reader["oldname"].ToString();
                            if (!string.IsNullOrEmpty(name) && !cannotUse.Contains(name))
                            {
                                cannotUse.Add(name);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("查询数据库失败: {0}", cmdText));
                }

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

        }

        #region 角色名
        /// <summary>
        /// 标记names中的所有名字都不可以使用
        /// </summary>
        /// <param name="names"></param>
        /// <returns>全部标记成功返回true，否则返回false</returns>
        public bool AddCannotUse_Ex(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            lock (cannotUse)
            {
                if (cannotUse.Contains(name))
                {
                    return false;
                }

                cannotUse.Add(name);

            }

            return true;
        }
        
        /// <summary>
        /// 删除名字占位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DelCannotUse_Ex(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
           
            lock (cannotUse)
            {
                if (cannotUse.Contains(name))
                {
                    cannotUse.Remove(name);
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 帮会名
        /// <summary>
        /// 标记names中的所有名字都不可以使用
        /// </summary>
        /// <param name="names"></param>
        /// <returns>全部标记成功返回true，否则返回false</returns>
        public bool AddCannotUse_BangHui_Ex(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            lock (cannotUse_BangHui)
            {
                if (cannotUse_BangHui.Contains(name))
                {
                    return false;
                }

                cannotUse_BangHui.Add(name);

            }

            return true;
        }
               
        /// <summary>
        /// 删除名字占位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DelCannotUse_BangHui_Ex(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            lock (cannotUse_BangHui)
            {
                if (cannotUse_BangHui.Contains(name))
                {
                    cannotUse_BangHui.Remove(name);
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
