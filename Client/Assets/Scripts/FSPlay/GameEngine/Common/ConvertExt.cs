using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSPlay.Drawing;
using UnityEngine;

namespace FSPlay.GameEngine.Common
{
    /// <summary>
    /// 数据类型转换扩展
    /// </summary>
	public class ConvertExt
	{
        #region 类型转换

        /// <summary>
        /// 将类型数组转换为字符串
        /// </summary>
        /// <param name="intArr">Int类型数组</param>
        /// <returns></returns>
        public static string Array2String<T>(T[] intArr , char ch = ',')
        {
            if (null == intArr || intArr.Length <= 0) return "";

            string str = "";
            for (int i = 0; i < intArr.Length; i++)
            {
                str += (intArr[i] + ch.ToString());
            }

            return str.Substring(0 , str.Length - 1);
        }


        /// <summary>
        /// 安全的字符串到整型的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int SafeConvertToInt32(string str)
        {
            str = str.Trim();
            int iresoult = 0;
            if (int.TryParse(str, out iresoult))
            {
                return iresoult;
            }
            
            return 0;
        }

        /// <summary>
        /// 安全的字符串到长整型的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long SafeConvertToInt64(string str)
        {
            str = str.Trim();
            long iresoult = 0;
            if (long.TryParse(str, out iresoult))
            {
                return iresoult;
            }

            return 0;
        }


        /// <summary>
        /// 将日期时间字符串转为整数表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long SafeConvertToTicks(string strDateTime)
        {
            DateTime dt;
            if (!DateTime.TryParse(strDateTime, out dt))
            {
                return 0L;
            }

            return dt.Ticks / 10000;
        }

        /// <summary>
        /// 安全的字符串到浮点的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double SafeConvertToDouble(string str)
        {
            str = str.Trim();
            
            double dresout = 0.0;
            if (double.TryParse(str, out dresout))
            {
                return dresout;
            }
            return 0.0;
        }

        /// <summary>
        /// 安全的字符串到Float的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float SafeConvertToFloat(string str, float defaultVal = 0.0f)
        {
            str = str.Trim();

            float dresout = 0.0f;
            if (float.TryParse(str, out dresout))
            {
                return dresout;
            }
            return defaultVal;
        }

        /// <summary>
        /// 将字符串转换为double类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static double[] String2DoubleArray(string str, char ch)
        {
            if (str.Trim() == "") return null;
            string[] sa = str.Split(ch);
            if (sa == null) return null;
            double[] da = new double[sa.Length];
            for (int i = 0; i < sa.Length; i++)
            {
                if (sa[i].Trim() == "") continue;
                da[i] = SafeConvertToDouble(sa[i].Trim());
            }

            return da;
        }

        /// <summary>
        /// 将字符串转换为Int类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static int[] String2IntArray(string str, char ch)
        {
            if (str.Trim() == "") return null;
            string[] sa = str.Split(ch);
            if (sa == null) return null;
            int[] da = new int[sa.Length];
            for (int i = 0; i < sa.Length; i++)
            {
                if (sa[i].Trim() == "") continue;
                da[i] = SafeConvertToInt32(sa[i].Trim());
            }

            return da;
        }

        /// <summary>
        /// 将字符串转换为Int类型List
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static List<int> String2IntList(string str, char ch)
        {
            if (str.Trim() == "") return null;
            string[] sa = str.Split(ch);
            if (sa == null) return null;
            List<int> da = new List<int>();
            for (int i = 0; i < sa.Length; i++)
            {
                if (sa[i].Trim() == "") continue;
                da.Insert(da.Count, SafeConvertToInt32(sa[i].Trim()));
            }

            return da;
        }

        /// <summary>
        /// 将字符串数组转换为double类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static double[] StringArray2DoubleArray(string[] sa)
        {
            double[] da = new double[sa.Length];
            for (int i = 0; i < sa.Length; i++)
            {
                if (sa[i].Trim() == "") continue;
                da[i] = SafeConvertToDouble(sa[i].Trim());
            }

            return da;
        }

        /// <summary>
        /// 将字符串数组转换为Int类型数组
        /// </summary>
        /// <param name="ss">字符串数组</param>
        /// <returns></returns>
        public static int[] StringArray2IntArray(string[] sa)
        {
            int[] da = new int[sa.Length];
            for (int i = 0; i < sa.Length; i++)
            {
                if (sa[i].Trim() == "") continue;
                da[i] = SafeConvertToInt32(sa[i].Trim());
            }

            return da;
        }

        /// <summary>
        /// 将字符串转换为Point类型数组, x1:y1,x2:y2,...
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public static Point[] StrToPointArray(string str)
        {
            str = str.Trim();
            if (str == "") return null;
            string[] fields = str.Split(',');
            if (fields.Length <= 0) return null;

            Point[] pts = new Point[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                string[] ptFields = fields[i].Split(':');
                if (ptFields.Length != 2)
                {
                    continue;
                }

                if (ptFields[0].Trim() == "" || ptFields[1].Trim() == "") continue;

                pts[i] = new Point(SafeConvertToInt32(ptFields[0]), SafeConvertToInt32(ptFields[1]));
            }

            return pts;
        }

        /// <summary>
        /// 将字符串转换为Point类型数组, x1_y1,x2_y2,...
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public static Point[] StrToPointArray2(string str)
        {
            str = str.Trim();
            if (str == "") return null;

            string[] fields = str.Split(',');
            if (fields.Length <= 0) return null;

            Point[] pts = new Point[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                string[] ptFields = fields[i].Split('_');
                if (ptFields.Length != 2)
                {
                    continue;
                }

                if (ptFields[0].Trim() == "" || ptFields[1].Trim() == "") continue;

                pts[i] = new Point(SafeConvertToInt32(ptFields[0]), SafeConvertToInt32(ptFields[1]));
            }

            return pts;
        }

        /// <summary>
        /// 将Int类型数组转换为字符串
        /// </summary>
        /// <param name="intArr">Int类型数组</param>
        /// <returns></returns>
        public static string IntArray2String(int[] intArr, char ch = ',')
        {
            if (null == intArr || intArr.Length <= 0) return "";

            string str = "";
            for (int i = 0; i < intArr.Length; i++)
            {
                str += (intArr[i] + ch.ToString());
            }

            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 将String类型数组转换为字符串
        /// </summary>
        /// <param name="intArr">String类型数组</param>
        /// <returns></returns>
        public static string StringArray2String(string[] strArr , char ch = ',')
        {
            if (null == strArr || strArr.Length <= 0) return "";

            string str = "";
            for (int i = 0; i < strArr.Length; i++)
            {
                str += (strArr[i] + ch.ToString());
            }

            return str.Substring(0 , str.Length - 1);
        }
		
		/// <summary>
		/// 将米转换为厘米
		/// </summary>
		/// <returns>
		/// The to centimeter.
		/// </returns>
		/// <param name='v'>
		/// V.
		/// </param>
		public static Vector3 MeterToCentimeter(Vector3 v)
		{
			return new Vector3(v.x * 100.0f, v.y * 100.0f, v.z * 100.0f);
		}
		
		/// <summary>
		/// 将厘米转换为米
		/// </summary>
		/// <returns>
		/// The to centimeter.
		/// </returns>
		/// <param name='v'>
		/// V.
		/// </param>
        public static Vector3 CentimeterMeter(Vector3 v)
		{
			return new Vector3(v.x / 100.0f, v.y / 100.0f, v.z / 100.0f);
		}

        public static byte ToByte(string v)
        {
            byte bV = 0;
            if (byte.TryParse(v, out bV))
            {
                return bV;
            }
            return 0;
        }

        public static Int64 SafeToInt64(string v)
        {
            Int64 i64 = 0;
            if (Int64.TryParse(v, out i64))
                return i64;
            return 0;
        }

        #endregion //类型转换
	}
}
