using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FSPlay.Drawing;
using Server.Data;
using UnityEngine;
using FSPlay.GameEngine.Common;
using FSPlay.GameEngine.Data;
using FSPlay.GameEngine.Network;
using Server.Tools.AStarEx;
using FSPlay.GameEngine.Teleport;
using FSPlay.KiemVu.Utilities.UnityComponent;
using HSGameEngine.GameEngine.Network.Tools;

namespace FSPlay.GameEngine.Logic
{
	/// <summary>
	/// Các biến toàn cục hệ thống
	/// </summary>
	public static class Global
    {
        #region KiemVu
        /// <summary>
        /// Đối tượng bản đồ hiện tại
        /// </summary>
        public static FSPlay.KiemVu.Control.Component.Map CurrentMap { get; set; } = null;

        /// <summary>
        /// Canvas chứa UI trong Game
        /// </summary>
        public static UnityEngine.Canvas MainCanvas { get; set; } = null;

        /// <summary>
        /// Canvas chứa UI trong Minimap
        /// </summary>
        public static UnityEngine.Canvas MinimapCanvas { get; set; } = null;

        /// <summary>
        /// Prefab Camera soi đối tượng
        /// </summary>
        public static Camera ObjectPreviewCameraPrefab { get; set; } = null;

        /// <summary>
        /// Đối tượng thu âm và phát lại
        /// </summary>
        public static AudioRecorderAndPlayer Recorder { get; set; } = null;

        /// <summary>
        /// Đối tượng Render Pipeline 2D
        /// </summary>
        public static GameObject RenderPipeline2D { get; set; } = null;

        /// <summary>
        /// Camera dùng trong Radar Map
        /// </summary>
        public static Camera RadarMapCamera = null;

        /// <summary>
        /// Camera chính
        /// </summary>
        public static Camera MainCamera { get; set; } = null;
        #endregion

        #region Tham biến cấu hình

        /// <summary>
        /// Danh sách các tham biến cấu hình hệ thống
        /// </summary>
        public static Dictionary<string, string> RootParams { get; set; } = new Dictionary<string, string>();

        #endregion

        #region Quản lý kết nối

        /// <summary>
        /// Danh sách kết nối
        /// </summary>
        public static List<LineData> LineDataList { get; set; } = null;

        /// <summary>
        /// Kết nối hiện tại
        /// </summary>
        public static LineData CurrentListData { get; set; } = null;

        #endregion

        #region Thiết lập
        /// <summary>
        /// Dữ liệu tạm thời
        /// </summary>
        public static GData Data { get; set; } = null;

        /// <summary>
        /// 备份roleData
        /// </summary>
        private static RoleData RoleDataBackUp = null;
        /// <summary>
        /// 备份roleData
        /// </summary>
        public static void CopyRoleData(RoleData roleDataMini)
        {
            KTDebug.LogError("CopyRoleData");
            RoleDataBackUp = roleDataMini;
        }
        // 备份一个roleData  解决跨服是出现的roleData为null的情况
        public static void SetGameRoleData()
        {
            GameInstance.Game.CurrentSession.roleData = RoleDataBackUp;
            Global.RoleDataBackUp = null;
        }

        /// <summary>
        /// Dữ liệu bản đồ hiện tại
        /// </summary>
        public static GMapData CurrentMapData { get; set; } = null;
        #endregion

        #region Đường dẫn
        /// <summary>
        /// Đánh dấu xem có phải đang Reconnect không
        /// </summary>
		public static bool g_bReconnRoleManager = false;
        
        /// <summary>
        /// Trả về đường dẫn dạng WebURL tương ứng
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="isFolder"></param>
        /// <returns></returns>
        public static string WebPath(string uri, bool isFolder = false)
        {
            return PathUtils.WebPath(uri, isFolder);
        }

        /// <summary>
        /// Trả về tên thiết bị trên WebURL
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceForWebURL()
        {
            string a = "android";
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                a = "ios";
                return "ios";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return "android";
            }
            else
            {
#if UNITY_ANDROID
                return "android";
#elif UNITY_IPHONE
                return "ios";
#endif
            }
            return a;
        }
        #endregion



        #region 类型转换（间接调用ConvertExt中的函数，这里只为了兼容以前的代码移植）

        /// <summary>
        /// Chuyeenr
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int SafeConvertToInt32(string str)
        {
            return ConvertExt.SafeConvertToInt32(str);
        }


        #endregion //类型转换

        


        #region Tải xuống điểm truyền tống

        /// <summary>
        /// Tải xuống điểm truyền tống
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static GTeleport GetTeleport(XElement xmlNode)
        {
            int code = int.Parse(xmlNode.Attribute("Code").Value);
            if (-1 == code)
            {
                return null;
            }

            GTeleport teleport = new GTeleport("passerby164");

            teleport.Name = string.Format("Teleport{0}", int.Parse(xmlNode.Attribute("Key").Value));
            teleport.Key = byte.Parse(xmlNode.Attribute("Key").Value);
            teleport.To = int.Parse(xmlNode.Attribute("To").Value);
            teleport.ToX = int.Parse(xmlNode.Attribute("ToX").Value);
            teleport.ToY = int.Parse(xmlNode.Attribute("ToY").Value);
            teleport.PosX = int.Parse(xmlNode.Attribute("X").Value);
            teleport.PosY = int.Parse(xmlNode.Attribute("Y").Value);
            teleport.Radius = int.Parse(xmlNode.Attribute("Radius").Value);
            teleport.Tip = xmlNode.Attribute("Tip").Value;

            return teleport;
        }

        #endregion


        #region Quản lý vật phẩm
        /// <summary>
        /// Hằng số mặc định thời gian sử dụng vật phẩm (loại vĩnh viễn)
        /// </summary>
        public const string ConstGoodsEndTime = "1900-01-01 12:00:00";
        #endregion




        #region Format role name
        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName(RoleData roleData)
        {
            return FormatRoleName(roleData.ZoneID, roleData.RoleName);
        }
        /// <summary>
        /// 格式化角色名称
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string FormatRoleName(int zoneID, string roleName)
        {
            // return string.Format("[{0}区]{1}", zoneID, roleName);
            return roleName;
        }
        #endregion

        #region 获取传入的参数

        /// <summary>
        /// 根据名称获取Xap的自定义参数，以字符串类型返回
        /// </summary>  
        public static string GetXapParamByName(string name, string defVal = "")
        {


            if (Global.RootParams.ContainsKey(name))
            {

                return Global.RootParams[name];
            }

            return defVal;
        }

        /// <summary>
        /// Trả về Port đăng nhập
        /// </summary>
        /// <returns></returns>
        public static int GetUserLoginPort()
        {
            return Global.SafeConvertToInt32(Global.GetXapParamByName("loginport", "4502"));
        }

        /// <summary>
        /// Trả về Port của Server
        /// </summary>
        /// <returns></returns>
        public static int GetLineServerPort()
        {
            return Global.SafeConvertToInt32(Global.GetXapParamByName("lineserverport", "4504"));
        }

        /// <summary>
        /// Trả về Port của Server
        /// </summary>
        /// <returns></returns>
        public static int GetGameServerPort()
        {
            return Global.SafeConvertToInt32(Global.GetXapParamByName("gameport", "4503"));
        }

        #endregion 获取传入的参数

        #region Quản lý thời gian

        /// <summary>
        /// Lấy thời gian tính từ năm 1970 đến hiện tại
        /// </summary>
        /// <returns></returns>
        public static int GetMyTimer()
        {
            long ticks = ((DateTime.Now.Ticks - MyDateTime.Before1970Ticks) / 10000);
            return (int) (ticks);
        }

        #endregion



        #region Thư viện dùng cho String
        /// <summary>
        /// Thay thế toàn bộ chuỗi tương ứng
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="find">替换对象</param>
        /// <param name="replacement">替换内容</param>
        /// <returns></returns>
        public static string StringReplaceAll(string source, string find, string replacement)
        {
            string str = "";
            if (source != null && replacement != null && find != null)
            {
                str = source.Replace(find, replacement);
            }

            return str;
        }
        #endregion

        #region Thư viện dùng cho chuỗi Byte
        /// <summary>
        /// Trả về giá trị BIT tại vị trí tương ứng
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int GetBitValue(int whichOne)
        {
            int bitVal = (int) Math.Pow(2, whichOne - 1);
            return bitVal;
        }

        /// <summary>
        /// Trả về giá trị BIT tại vị trí tương ứng
        /// </summary>
        /// <param name="values"></param>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int GetBitValue(List<int> values, int whichOne)
        {
            int index = whichOne / 32;
            int bitIndex = whichOne % 32;
            if (values.Count <= index)
            {
                return 0;
            }
            int value = values[index];
            if ((value & (1 << bitIndex)) != 0)
            {
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
