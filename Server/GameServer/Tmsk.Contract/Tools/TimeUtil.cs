﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tmsk.Tools;
using System.Threading;
using System.Globalization;

namespace GameServer.Core.Executor
{
    public class TimeUtil
    {
        /// <summary>
        /// 毫秒
        /// </summary>
        public const int MILLISECOND = 1;
        /// <summary>
        /// 秒
        /// </summary>
        public const int SECOND = 1000 * MILLISECOND;
        /// <summary>
        /// 分钟
        /// </summary>
        public const int MINITE = 60 * SECOND;
        /// <summary>
        /// 小时
        /// </summary>
        public const int HOUR = 60 * MINITE;
        /// <summary>
        /// 天
        /// </summary>
        public const int DAY = 24 * HOUR;

        /// <summary>
        /// 如果宿主机时间回退，在这个范围内，则冻结服务器内部时间
        /// </summary>
        public const long BackFreezeTicks = -5L * MINITE * 10000L;

        private static int CurrentTickCount = 0;

        private static long CurrentTicks = DateTime.Now.Ticks / 10000;

        private static DateTime _Now = DateTime.Now;

        private static volatile int CorrectTimeSecs = 0;

        public static long CurrentTicksInexact { get { return CurrentTicks; } }

        private static bool UpdateByTimer = false;

        private static MmTimer mmTimer = null;

        /// <summary>
        /// 当前时间的字符串形式
        /// </summary>
        public static string _CurrentDataTimeString = "2011-01-01 00:00:00";

        /// <summary>
        /// 当前时间的字符串形式格式化时对应的时间
        /// </summary>
        private static long _CurrentDataTimeStringTicks = 0;

        static TimeUtil()
        {
            Init();
            TryInitMMTimer();
        }

        static ManualResetEvent waitStartEvent = new ManualResetEvent(false);
        public static void TryInitMMTimer()
        {
            try
            {
                mmTimer = new MmTimer();
                mmTimer.Interval = 1;
                mmTimer.Tick += (s, e) =>
                {
                    if (!UpdateByTimer)
                    {
                        UpdateByTimer = true;
                        waitStartEvent.Set();
                    }

                    DateTime now = DateTime.Now;
                    long ticks = now.Ticks - _Now.Ticks;
                    if (ticks < 0 && ticks > (-5L * MINITE * 10000))
                    {
                        return; //时间倒退是不允许的
                    }

                    if (CorrectTimeSecs != 0)
                    {
                        now = now.AddSeconds(CorrectTimeSecs);
                    }

                    _Now = now;
                    Thread.VolatileWrite(ref CurrentTicks, now.Ticks / 10000);
                };
                mmTimer.Disposed += (s, e) =>
                {
                    UpdateByTimer = false;
                    if (null != mmTimer)
                    {
                        mmTimer.Stop();
                        mmTimer = null;
                    }
                };

                mmTimer.Start();
                //WaitStart();
            }
            catch (System.Exception ex)
            {
                UpdateByTimer = false;
            }
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static void WaitStart()
        {
            if (waitStartEvent.WaitOne(5000))
            {
                long c = CurrentTicks;
                for (int i = 0; i < 1000; i++ )
                {
                    //Console.WriteLine(CurrentTicks);
                    while (c == Thread.VolatileRead(ref CurrentTicks))
                    {
                        Thread.Yield();
                    }
                    c = CurrentTicks;
                }
            }
        }

        public static DateTime SetTime(string timeStr)
        {
            DateTime dt;
            if (DateTime.TryParse(timeStr, out dt))
            {
                CorrectTimeSecs = (int)(dt - DateTime.Now).TotalSeconds;
                CurrentTickCount = 0;
                return dt;
            }

            return _Now;
        }

        /// <summary>
        /// 当前系统时间毫秒数
        /// </summary>
        public static long NOW()
        {
            if (!UpdateByTimer)
            {
                int tickCount = Environment.TickCount;
                if (tickCount != CurrentTickCount)
                {
                    DateTime now = DateTime.Now;
                    long ticks = now.Ticks - _Now.Ticks;
                    if (ticks < 0 && ticks > (-5L * MINITE * 10000))
                    {
                        return CurrentTicks;
                    }

                    if (CorrectTimeSecs != 0)
                    {
                        now = now.AddSeconds(CorrectTimeSecs);
                    }

                    _Now = now;
                    CurrentTickCount = tickCount;
                    CurrentTicks = now.Ticks / 10000;
                }
                return CurrentTicks;
            }
            else
            {
                return Thread.VolatileRead(ref CurrentTicks);
            }
        }

        public static long NowRealTime()
        {
            return DateTime.Now.Ticks / 10000;
        }

        public static DateTime NowDateTime()
        {
            if (!UpdateByTimer)
            {
                int tickCount = Environment.TickCount;
                if (tickCount != CurrentTickCount)
                {
                    DateTime now = DateTime.Now;
                    long ticks = now.Ticks - _Now.Ticks;
                    if (ticks < 0 && ticks > (-5L * MINITE * 10000))
                    {
                        return _Now;
                    }

                    if (CorrectTimeSecs != 0)
                    {
                        now = now.AddSeconds(CorrectTimeSecs);
                    }

                    _Now = now;
                    CurrentTickCount = tickCount;
                    CurrentTicks = now.Ticks / 10000;
                }
            }
            return _Now;
        }

        public static string NowDataTimeString(string format = "yyyy-MM-dd HH:mm:ss")
        {
            int tickCount = CurrentTickCount;
            DateTime now = NowDateTime();
            if (now.Ticks != _CurrentDataTimeStringTicks)
            {
                _CurrentDataTimeStringTicks = now.Ticks;
                _CurrentDataTimeString = now.ToString(format);
            }

            return _CurrentDataTimeString;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000000;            //除10000调整为10位
            return t;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }

        [DllImport("kernel32.dll")]
        extern static bool QueryPerformanceCounter(ref long x);
        [DllImport("kernel32.dll")]
        extern static bool QueryPerformanceFrequency(ref long x);

        private static long _StartCounter = 0;
        private static long _CounterPerSecs = 0;
        private static bool _EnabelPerformaceCounter = false;
        private static long _StartTicks = 0;

        public static long CounterPerSecs
        {
            get
            {
                return _CounterPerSecs;
            }
        }

        public static long Init()
        {
            _EnabelPerformaceCounter = QueryPerformanceFrequency(ref _CounterPerSecs);
            QueryPerformanceCounter(ref _StartCounter);
            _EnabelPerformaceCounter = (_EnabelPerformaceCounter && _CounterPerSecs > 0 && _StartCounter > 0);
            _StartTicks = DateTime.Now.Ticks;
            return _StartTicks;
        }

        public static long NowEx()
        {
            if (GameManager.StatisticsMode == 0)
            {
                return CurrentTicks;
            }
            else if (GameManager.StatisticsMode == 1)
            {
                return CurrentTicks;
            }
            else
            {
                if (_EnabelPerformaceCounter)
                {
                    long counter = 0;
                    QueryPerformanceCounter(ref counter);
                    return counter;
                }
                else
                {
                    return DateTime.Now.Ticks;
                }
            }
        }


        public static string GetDateExpriess(string InputData,int AddMin =0)
        {

            DateTime Data_In =DateTime.ParseExact(InputData, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if(AddMin>0)
            {
                Data_In = Data_In.AddMinutes(AddMin);
            }    

            DateTime Now = DateTime.Now;


            double TotalMin = (Data_In - Now).TotalMinutes;


            string Out= TotalMin / 24 / 60 + ":" + TotalMin / 60 % 24 + ':' + TotalMin % 60;


            return "Thời gian sử dụng :" + Out;
        }

        public static double TimeMS(long time, int round = 2)
        {
            if (GameManager.StatisticsMode <= 1)
            {
                return time;
            }
            else
            {
                long timeDiff = TimeDiff(time, 0);
                return Math.Round(timeDiff / 10000.0, round);
            }
        }

        public static long TimeDiff(long timeEnd, long timeStart = 0)
        {
            if (GameManager.StatisticsMode <= 1)
            {
                return timeEnd - timeStart;
            }
            else if (_EnabelPerformaceCounter)
            {
                long counter = timeEnd - timeStart;
                long count1;
                long secs = Math.DivRem(counter, _CounterPerSecs, out count1);
                return secs * 10000000 + count1 * 10000000 / _CounterPerSecs;
            }
            else
            {
                return timeEnd - timeStart;
            }
        }
        /// <summary>
        /// 服务器主机启动后流逝的时间,单位毫秒，防DateTime.Now.Ticks时间飘逸
        /// </summary>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        public static extern uint timeGetTime();

        #region 依赖于时间校正的服务器时间漂移检测

        private static long TimeDriftTicks;
        private static long LastAnchorTicks;
        private static long LastAnchorCounter;

        public static void RecordTimeAnchor()
        {
            if (_EnabelPerformaceCounter)
            {

                long ticks = NOW();
                long timeDiff0 = ticks - LastAnchorTicks;

                long count1;
                long counter = 0;
                QueryPerformanceCounter(ref counter);
                long secs = Math.DivRem(counter - LastAnchorCounter, _CounterPerSecs, out count1);
                long timeDiff1 = secs * 1000 + count1 * 1000 / _CounterPerSecs;

                //漂移量超过10%,则记录为漂移
                if (Math.Abs(timeDiff0 - timeDiff1) >= timeDiff1 / 10)
                {
                    TimeDriftTicks = ticks;
                }

                LastAnchorTicks = ticks;
                LastAnchorCounter = counter;
            }
        }

        /// <summary>
        /// 180秒内是否有时间漂移
        /// </summary>
        /// <returns></returns>
        public static bool HasTimeDrift()
        {
            if (NOW() - TimeDriftTicks < 180 * SECOND)
            {
                return true;
            }

            return false;
        }

        #endregion 依赖于时间校正的服务器时间漂移检测

        #region 时间--->int的易观察的存储方式
        /// <summary>
        /// "2016-04-23 18:21:22" ---> 2016
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int MakeYear(DateTime time)
        {
            return time.Year;
        }

        /// <summary>
        /// "2016-04-23 18:21:22" ---> 201604
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int MakeYearMonth(DateTime time)
        {
            return MakeYear(time) * 100 + time.Month;
        }

        /// <summary>
        /// "2016-04-23 18:21:22" ---> 20160423
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int MakeYearMonthDay(DateTime time)
        {
            return MakeYearMonth(time) * 100 + time.Day;
        }

        /// <summary>
        /// "2016-04-23 18:21:22" ---> 20160418
        /// 这里认为周一是一周的开始
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int MakeFirstWeekday(DateTime time)
        {
            time = time.AddDays(time.DayOfWeek == DayOfWeek.Sunday ? -6 : (1 - (int)time.DayOfWeek));
            return MakeYearMonthDay(time);
        }

        /// <summary>
        /// 周 1---7
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetWeekDay1To7(DateTime time)
        {
            switch (time.DayOfWeek)
            {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
                default: throw new Exception("unbelievable");
            }
        }
        #endregion
    }
}
