using System;
using Server.Data;

namespace FSPlay.GameEngine.Network
{
    public class SDKSession
    {
        public static string AccessToken { get; set; }

        public static string TrasnID { get; set; }
    }
    /// <summary>
    /// Quản lý Session của game
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Quản lý Session của game
        /// </summary>
        public Session()
        {
        }

        #region Thông tin nhân vật

        /// <summary>
        /// ID Tài khoản
        /// </summary>
        public string UserID = "";

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string UserName = "";

        /// <summary>
        /// Token
        /// </summary>
        public string UserToken = "";



     

        /// <summary>
        /// IP đăng nhập lần trước
        /// </summary>
        public string LastLoginIP = "";

        /// <summary>
        /// Thời gian đăng nhập lần trước
        /// </summary>
        public string LastLoginTime = "";

        /// <summary>
        /// Unknow
        /// </summary>
        public string Cm { get; set; }

        /// <summary>
        /// Unknow
        /// </summary>
        public long TimeActive { get; set; }

        /// <summary>
        /// Unknow
        /// </summary>
        public string TokenGS { get; set; }

        /// <summary>
        /// Lớn chưa
        /// </summary>
        public int UserIsAdult = 0;

        /// <summary>
        /// Random token
        /// </summary>
        public int RoleRandToken = -1;

        /// <summary>
        /// ID nhân vật
        /// </summary>
        public int RoleID = -1;

        /// <summary>
        /// Giới tính
        /// </summary>
        public int RoleSex = 0;

        /// <summary>
        /// Tên nhân vật
        /// </summary>
        public string RoleName = "";

        /// <summary>
        /// Đã chơi game chưa
        /// </summary>
        public bool PlayGame = false;

        /// <summary>
        /// Dứ liệu nhân vật
        /// </summary>

        public RoleData roleData = null;

        /// <summary>
        /// Chuỗi hướng dẫn tìm đường hiện tại
        /// </summary>
        public string RolePathString = "";

        /// <summary>
        /// ID Server
        /// </summary>
        public int GameServerID = 1;
        #endregion Thông tin nhân vật
    }
}

