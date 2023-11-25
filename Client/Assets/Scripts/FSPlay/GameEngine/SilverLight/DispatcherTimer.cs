using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FSPlay.GameEngine.SilverLight
{
    /// <summary>
    /// 回调通知函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate void DispatcherTimerEventHandler(object sender, EventArgs args);
    
    /// <summary>
    /// Unity中没有找到主线程Timer，模拟实现
    /// </summary>
	public class DispatcherTimer : IDisposable
	{
		//超过计时器间隔时发生。
		public DispatcherTimerEventHandler Tick =  null;

		private string _Name =  "未知";
        private bool _Started = false; //是否已经开始
        private long _LastTicks = 0;
		
		//必须赋值一个特殊的名称，用于监控cpu占用
        public DispatcherTimer(string name)
		{
			_Name = name;
            DispatcherTimerDriver.AddTimer(this);
		}
		
        /// <summary>
        /// 计时器名称
        /// </summary>
		public string Name
		{
			get { return _Name; }
            set { _Name = value; }
		}		
		
		private TimeSpan _Interval = TimeSpan.Zero;

        /// <summary>
        /// 设置计时器的时间间隔
        /// </summary>
		public TimeSpan Interval
		{
			get { return _Interval; }
            set { _Interval = value; }
		}
		
        /// <summary>
        /// 开始计时器
        /// </summary>
		public void Start()
		{
            _Started = true;
            _LastTicks = DateTime.Now.Ticks;
		}
		
        /// <summary>
        /// 停止计时器
        /// </summary>
		public void Stop()
		{
            _Started = false;
		}

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务
        /// </summary>
        public void Dispose()
        {
            DispatcherTimerDriver.RemoveTimer(this);
        }

        /// <summary>
        /// 执行timer
        /// </summary>
        public void ExecuteTimer()
        {
            long ticks = DateTime.Now.Ticks;
            if (ticks - _LastTicks < _Interval.Ticks)
            {
                return;
            }

            _LastTicks = ticks;

            if (null != Tick)
            {
                long startTicks = DateTime.Now.Ticks / 10000;

                Tick(this, EventArgs.Empty);

                long elapsedTicks = (DateTime.Now.Ticks / 10000) - startTicks;
                if (elapsedTicks >= 100)
                {
                    KTDebug.Log("DispatcherTimer.ExecuteTimer, Name=" + _Name + ", Used ticks=" + elapsedTicks);
                }
            }
        }
	}
}
