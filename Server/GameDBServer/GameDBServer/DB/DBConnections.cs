using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLDriverCS;
using System.Data;
using System.Windows;
using Server.Tools;
using System.Threading;
using System.Collections.Concurrent;

namespace GameDBServer.DB
{
    /// <summary>
    /// Quản lý kết nối DB
    /// </summary>
    public class DBConnections
    {
        /// <summary>
        /// Tên DB
        /// </summary>
        public static string dbNames = "";

        /// <summary>
        /// Đối tượng Semaphore
        /// </summary>
        private Semaphore SemaphoreClients = null;

        /// <summary>
        /// Danh sách kết nối đang chờ
        /// </summary>
        private ConcurrentQueue<MySQLConnection> DBConns = new ConcurrentQueue<MySQLConnection>();

        /// <summary>
        /// MUTEX dùng trong LOCK
        /// </summary>
        private object Mutex = new object();

        /// <summary>
        /// Chuỗi kết nói
        /// </summary>
        private string ConnectionString;

        /// <summary>
        /// Tổng số kết nối
        /// </summary>
        private int CurrentCount;

        /// <summary>
        /// Kết nối tối đa
        /// </summary>
        private int MaxCount;

        /// <summary>
        /// Tạo kết nối mới tới Database
        /// </summary>
        /// <param name="connStr"></param>
        public void BuidConnections(MySQLConnectionString connStr, int maxCount)
        {
            //lock (this.Mutex)
            {
                ConnectionString = connStr.AsString;
                MaxCount = 100;
                SemaphoreClients = new Semaphore(0, 100);

                for (int i = 0; i < 100; i++)
                {
                    MySQLConnection dbConn = CreateAConnection();
                    if (null == dbConn)
                    {
                        throw new Exception(string.Format("Connect to MySQL faild"));
                    }
                }
            }
        }

        /// <summary>
        /// Tạo kết nối
        /// </summary>
        /// <returns></returns>
        private MySQLConnection CreateAConnection()
        {
            try
            {
                MySQLConnection dbConn = null;
                dbConn = new MySQLConnection(ConnectionString);
                dbConn.Open();
                if (!string.IsNullOrEmpty(dbNames))
                {
                    using (MySQLCommand cmd = new MySQLCommand(string.Format("SET names '{0}'", dbNames), dbConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                //lock (this.Mutex)
                {
                    DBConns.Enqueue(dbConn);
                    CurrentCount++;
                    this.SemaphoreClients.Release();
                }

                return dbConn;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, string.Format("Create database connection exception: \r\n{0}", ex.ToString()));
            }

            return null;
        }

        /// <summary>
        /// Duy trì kết nối
        /// </summary>
        /// <returns></returns>
        public bool SupplyConnections()
        {
            bool result = false;
            //lock (this.Mutex)
            {
                if (CurrentCount < MaxCount)
                {
                    CreateAConnection();
                }
            }

            return result;
        }

        /// <summary>
        /// Trả về tổng số kết nối đến Database
        /// </summary>
        /// <returns></returns>
        public int GetDBConnsCount()
        {
            //lock (this.Mutex)
            {
                return this.DBConns.Count;
            }
        }

        /// <summary>
        /// Lấy kết nối đến Database trong hàng đợi để thực thi
        /// </summary>
        /// <returns></returns>
        public MySQLConnection PopDBConnection()
        {
            MySQLConnection conn = null;
            bool lost = false;

            do
            {
                string cmdText = @"select 1";
                lost = true;
                SemaphoreClients.WaitOne();

                /// Toác
                if (!this.DBConns.TryDequeue(out conn))
                {
                    return null;
                }

                try
                {
                    using (MySQLCommand cmd = new MySQLCommand(cmdText, conn))
                    {
                        try
                        {
                            cmd.ExecuteNonQuery();
                            lost = false;
                        }
                        catch (System.Exception ex)
                        {
                            LogManager.WriteLog(LogTypes.Exception, string.Format("Exception occurred when executing Database Query: {0}\r\n{1}", cmdText, ex.ToString()));
                        }
                    }
                }
                catch(Exception ex)
                {
                    LogManager.WriteException(ex.ToString());
                }
                finally
                {
                    if (lost)
                    {
                        //lock (this.Mutex)
                        {
                            CurrentCount--;
                        }
                    }
                }
            }
            while (lost);

            return conn;
        }

        /// <summary>
        /// Thực thi kết nối
        /// </summary>
        /// <param name="conn"></param>
        public void PushDBConnection(MySQLConnection conn)
        {
            if (null != conn)
            {
                //lock (this.Mutex)
                {
                    this.DBConns.Enqueue(conn);
                }

                SemaphoreClients.Release();
            }
        }
    }
}
