﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu
{
    /// <summary>
    /// Các hàm toàn cục dùng trong Kiếm Thế
    /// </summary>
    public static partial class KTGlobal
    {
        #region Device Info

        /// <summary>
        /// Thông tin thiết bị
        /// </summary>
        public static string DeviceInfo { get; set; }

        /// <summary>
        /// Trả về thông tin thiết bị
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceInfo()
        {
            return KTGlobal.DeviceInfo;
        }

        /// <summary>
        /// Trả về loại thiết bị
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceGeneration()
        {
#if UNITY_EDITOR
            return "Editor";
#elif UNITY_ANDROID
			AndroidJavaClass model = new AndroidJavaClass("android.os.Build");
			string name = model.GetStatic<string>("MODEL");
			return name;
#elif UNITY_IPHONE
			string devInfo = UnityEngine.iOS.Device.generation.ToString();
			return devInfo;
#else
			return "-1";
#endif
        }

        /// <summary>
        /// Trả về Model của thiết bị
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceModel()
        {
#if UNITY_EDITOR
            return "Editor";
#elif UNITY_ANDROID
			AndroidJavaClass model = new AndroidJavaClass("android.os.Build");
			string name = model.GetStatic<string>("MODEL");
			return name;
#elif UNITY_IOS
            string deviceModel = UnityEngine.SystemInfo.deviceModel;
			return deviceModel;
#else
			return "Unknow";
#endif
        }

        /// <summary>
        /// Trả về khoảng trống dung lượng trong thiết bị
        /// </summary>
        /// <returns></returns>
        public static bool GetFreeDiskSpaces(int byteSize)
        {
            /// TODO
            return true;
        }

        #endregion Device Info

        #region System Time

        /// <summary>
        /// Trả về giờ hệ thống hiện tại dưới đơn vị Mili giây
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimeMilis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }


        /// <summary>
        /// Độ lệch so với giờ Server gần đây nhất
        /// </summary>
        public static long LastDiffTimeToServer { get; set; }

        /// <summary>
        /// Trả về giờ Server tương ứng
        /// </summary>
        /// <returns></returns>
        public static long GetServerTime()
        {
            return KTGlobal.GetCurrentTimeMilis() + KTGlobal.LastDiffTimeToServer;
        }

        #endregion System Time
    }
}
