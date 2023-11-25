using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FSPlay.GameEngine.SilverLight
{
    /// <summary>
    /// Quản lý danh sách Timer
    /// </summary>
	public static class DispatcherTimerDriver
	{
        /// <summary>
        /// Danh sách Timer
        /// </summary>
        private static List<DispatcherTimer> TimersList = new List<DispatcherTimer>();

        /// <summary>
        /// Thêm Timer
        /// </summary>
        /// <param name="timer"></param>
        public static void AddTimer(DispatcherTimer timer)
        {
            DispatcherTimerDriver.TimersList.Add(timer);
        }

        /// <summary>
        /// Xóa Timer
        /// </summary>
        /// <param name="timer"></param>
        public static void RemoveTimer(DispatcherTimer timer)
        {
            DispatcherTimerDriver.TimersList.Remove(timer);
        }

        /// <summary>
        /// Xóa toàn bộ Timer
        /// </summary>
        public static void ClearAll()
        {
            DispatcherTimerDriver.TimersList.Clear();
        }

        /// <summary>
        /// Thực thi
        /// </summary>
        public static void ExecuteTimers()
        {
            int count = DispatcherTimerDriver.TimersList.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    DispatcherTimerDriver.TimersList[i].ExecuteTimer();
                }
                catch (System.Exception ex)
                {
                    KTDebug.LogException(ex);
                }                
            }
        }
	}
}
