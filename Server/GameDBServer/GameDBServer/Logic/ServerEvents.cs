using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Server.Tools;
using GameDBServer.Core;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 事件级别
    /// </summary>
    public enum EventLevels
    {
        Ignore = -1, //忽略
        Debug = 0, //调试
        Hint = 1, //提示
        Record = 2, //记录
        Important = 3, //重要
    }

    /// <summary>
    /// 服务器端日志事件
    /// </summary>
    public class ServerEvents
    {
        public ServerEvents()
        {
        }

        /// <summary>
        /// 事件队列
        /// </summary>
        private Queue<string> EventsQueue = new Queue<string>();

        /// <summary>
        /// 当前的日志级别
        /// </summary>
        public EventLevels EventLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 日志顶级目录名称
        /// </summary>
        private string _EventRootPath = "events";

        /// <summary>
        /// 日志顶级目录名称
        /// </summary>
        public string EventRootPath
        {
            get { return _EventRootPath; }
            set { _EventRootPath = value; }
        }

        /// <summary>
        /// 日志文件前缀名称
        /// </summary>
        private string _EventPreFileName = "Event";

        /// <summary>
        /// 日志文件前缀名称
        /// </summary>
        public string EventPreFileName
        {
            get { return _EventPreFileName; }
            set { _EventPreFileName = value; }
        }

        /// <summary>
        /// 添加新事件
        /// </summary>
        /// <param name="msg"></param>
        public void AddEvent(string msg, EventLevels eventLevel)
        {
            if ((int)eventLevel < (int)EventLevel) //不必记录
            {
                return;
            }

            lock (EventsQueue)
            {
                EventsQueue.Enqueue(msg);
            }
        }

        /// <summary>
        /// 日志输出目录
        /// </summary>
        private string _EventPath = string.Empty;

        /// <summary>
        /// 日志输出的年份ID
        /// </summary>
        private string _YearID = string.Empty;

        /// <summary>
        /// 日志输出的月份ID
        /// </summary>
        private string _MonthID = string.Empty;

        /// <summary>
        /// 日志输出的日期ID
        /// </summary>
        private string _DayID = string.Empty;

        /// <summary>
        /// 路径锁
        /// </summary>
        private object _PathLock = new Object();

        /// <summary>
        /// 日志输出的小时
        /// </summary>
        private int _HourID = -1;

        /// <summary>
        /// 输出流
        /// </summary>
        StreamWriter _StreamWriter = null;

        /// <summary>
        /// 日志输出目录
        /// </summary>
        public string EventPath
        {
            get
            {
                DateTime dateTime = DateTime.Now;
                lock (_PathLock)
                {
                    string yearID = dateTime.ToString("yyyy");
                    string monthID = dateTime.ToString("MM");
                    string dayID = dateTime.ToString("dd");
                    if (_EventPath == string.Empty || yearID != _YearID || monthID != _MonthID || dayID != _DayID)
                    {
                        _YearID = yearID;
                        _MonthID = monthID;
                        _DayID = dayID;
                        _EventPath = AppDomain.CurrentDomain.BaseDirectory + string.Format("{0}/", _EventRootPath);

                        try
                        {
                            if (!System.IO.Directory.Exists(_EventPath))
                            {
                                System.IO.Directory.CreateDirectory(_EventPath);
                            }
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            _EventPath = string.Format("{0}Year_{1}/", _EventPath, _YearID);
                            if (!System.IO.Directory.Exists(_EventPath))
                            {
                                System.IO.Directory.CreateDirectory(_EventPath);
                            }
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            _EventPath = string.Format("{0}Month_{1}/", _EventPath, _MonthID);
                            if (!System.IO.Directory.Exists(_EventPath))
                            {
                                System.IO.Directory.CreateDirectory(_EventPath);
                            }
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            _EventPath = string.Format("{0}{1}/", _EventPath, _DayID);
                            if (!System.IO.Directory.Exists(_EventPath))
                            {
                                System.IO.Directory.CreateDirectory(_EventPath);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                return _EventPath;
            }
        }

        /// <summary>
        /// 将事件写入日志
        /// </summary>
        public bool WriteEvent()
        {
            string msg = null;
            lock (EventsQueue)
            {
                if (EventsQueue.Count > 0)
                {
                    msg = EventsQueue.Dequeue();
                }
            }

            if (null == msg) return false;
            DateTime dateTime = TimeUtil.NowDateTime();
            string logMsg = string.Format("{0}{1}", dateTime.ToString("yyyy-MM-dd HH:mm:ss  "), msg);

            try
            {
                if (_HourID != dateTime.Hour || _StreamWriter == null)
                {
                    string fileName = string.Format("{0}{1}_{2}.log", EventPath, _EventPreFileName, dateTime.ToString("HH"));
                    if (null != _StreamWriter)
                    {
                        _StreamWriter.Flush();
                        _StreamWriter.Close();
                        _StreamWriter.Dispose();
                        _StreamWriter = null;
                    }
                    _StreamWriter = File.AppendText(fileName);
                    _HourID = dateTime.Hour;
                    _StreamWriter.AutoFlush = true;
                }

                _StreamWriter.WriteLine(logMsg);
            }
            catch
            {
                _HourID = -1;
            }

            return true;
        }
    }
}
