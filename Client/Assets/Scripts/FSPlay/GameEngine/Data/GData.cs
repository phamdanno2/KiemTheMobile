using System.Collections.Generic;
using Server.Data;
using FSPlay.Drawing;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Scene;
using System;
using UnityEngine;
using FSPlay.GameFramework.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Entities.Config;

namespace FSPlay.GameEngine.Data
{
    /// <summary>
    /// Dữ liệu của Game
    /// </summary>
    public class GData
    {
        #region Bản đồ

        /// <summary>
        /// Bản đồ hiện tại
        /// </summary>
        public GScene GameScene { get; set; } = null;

        /// <summary>
        /// Nhân vật hiện tại
        /// </summary>
        public GSprite Leader
        {
            get
            {
                if (this.GameScene == null)
                {
                    return null;
                }
                return this.GameScene.GetLeader();
            }
        }

        /// <summary>
        /// Đang đợi chuyển bản đồ
        /// </summary>
        public bool WaitingForMapChange { get; set; } = false;

        #endregion

        #region Tải
        /// <summary>
        /// Tên bản đồ đang thực hiện tải xuống
        /// </summary>
        public string CurrentDownloadingMapName { get; set; } = "";

        /// <summary>
        /// Tiến độ đang tải bản đồ hiện tại
        /// </summary>
        public int CurrentDownloadingMapProgress { get; set; } = 0;
        #endregion

        #region Người chơi

        /// <summary>
        /// ID người chơi
        /// </summary>
        public string UserID
        {
            get { return GameInstance.Game.CurrentSession.UserID; }
        }

        /// <summary>
        /// Đánh dấu đã vào Game chưa
        /// </summary>
        public bool PlayGame
        {
            get { return GameInstance.Game.CurrentSession.PlayGame; }
        }

        /// <summary>
        /// Dữ liệu nhân vật
        /// </summary>
        public RoleData RoleData
        {
            get { return GameInstance.Game.CurrentSession.roleData; }
        }

        /// <summary>
        /// ID máy chủ
        /// </summary>
        public int GameServerID
        {
           // get { return GameInstance.Game.CurrentSession.GameServerID; }
            get 
            {
                int serverID = 1;
                //最后登录的服务器数据，重新获取一下吧。
                int lastServerId = PlayerPrefs.GetInt("NewLastServerInfoID");
                if (lastServerId != 0)
                {
                    serverID = lastServerId;
                }

                //if (ServerData != null)
                //{
                //    if(ServerData.LastServer == null)
                //    {
                //        //最后登录的服务器数据，重新获取一下吧。
                //        int lastServerId = PlayerPrefs.GetInt("NewLastServerInfoID");
                //        if (lastServerId != 0)
                //        {
                //            serverID = lastServerId;
                //        }
                //    }
                //    else 
                //    {
                //        serverID = ServerData.LastServer.ServerID;
                //    }
                //}
                //else
                //{
                //    //最后登录的服务器数据，重新获取一下吧。
                //    int lastServerId = PlayerPrefs.GetInt("NewLastServerInfoID");
                //    if (lastServerId != 0)
                //    {
                //        serverID = lastServerId;
                //    }
                //}

                return serverID;
            }  
        }
        #endregion


        #region Thông tin máy chủ

        /// <summary>
        /// Thông tin máy chủ
        /// </summary>
        public XuanFuServerData ServerData { get; set; }

        #endregion


        #region Tương quan nhiệm vụ
        /// <summary>
        /// Danh sách nhiệm vụ đã hoàn thành
        /// </summary>
        public Dictionary<int, HashSet<int>> CompletedTasks { get; } = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// Xây danh sách nhiệm vụ đã hoàn thành
        /// </summary>
        public void BuildCompletedTasks()
        {
            /// Làm rỗng danh sách nhiệm vụ đã hoàn thành
            this.CompletedTasks.Clear();

            /// Duyệt danh sách nhiệm vụ đã hoàn thành
            foreach (QuestInfo info in Global.Data.RoleData.QuestInfo)
            {
                /// Nếu chưa tồn tại thì tạo mới danh sách
                if (!this.CompletedTasks.ContainsKey(info.TaskClass))
                {
                    this.CompletedTasks[info.TaskClass] = new HashSet<int>();
                }

                /// ID nhiệm vụ hoàn thành lần cuối
                int lastCompletedTaskID = info.CurTaskIndex;

                /// ID nhiệm vụ đang kiểm tra hiện tại
                int currentTaskID = lastCompletedTaskID;
                do
                {
                    /// Nếu nhiệm vụ không tồn tại
                    if (!Loader.Tasks.TryGetValue(currentTaskID, out TaskDataXML taskDataXML))
                    {
                        break;
                    }

                    /// Danh mục nhiệm vụ
                    int category = taskDataXML.TaskClass;

                    /// Thên nhiệm vụ đã hoàn thành vào danh sách hiện tại
                    this.CompletedTasks[category].Add(currentTaskID);

                    /// Nhiệm vụ trước đó
                    int prevTaskID = taskDataXML.PrevTask;

                    /// Tiếp tục kiểm tra nhiệm vụ sau đó
                    currentTaskID = prevTaskID;
                }
                while (true);
            }
        }
        #endregion



        #region Tương quan bản đồ
        /// <summary>
        /// Danh sách thành viên trong nhóm
        /// </summary>
        public List<RoleDataMini> Teammates { get; set; } = new List<RoleDataMini>();

        /// <summary>
        /// Danh sách người chơi khác theo ID
        /// </summary>
        public Dictionary<int, RoleData> OtherRoles { get; set; } = new Dictionary<int, RoleData>(500);

        /// <summary>
        /// Danh sách BOT theo ID
        /// </summary>
        public Dictionary<int, RoleData> Bots { get; set; } = new Dictionary<int, RoleData>(500);

        /// <summary>
        /// Danh sách người chơi khác theo tên
        /// </summary>
        public Dictionary<string, RoleData> OtherRolesByName { get; set; } = new Dictionary<string, RoleData>(500);

        /// <summary>
        /// Danh sách quái theo ID
        /// </summary>
        public Dictionary<int, MonsterData> SystemMonsters { get; set; } = new Dictionary<int, MonsterData>(500);
        
        /// <summary>
        /// ID mục tiêu được chọn
        /// </summary>
        public int TargetNpcID { get; set; } = -1;
        #endregion


        #region Tương quan PK
        /// <summary>
        /// Khóa di chuyển bằng JoyStick
        /// <para>Khi chức năng này được kích hoạt thì khi dùng JoyStick sẽ chỉ đổi hướng nhân vật về hướng tương ứng</para>
        /// </summary>
        public bool LockJoyStickMove { get; set; } = false;
        #endregion


        #region Tương quan sự kiện và hoạt động
        /// <summary>
        /// Đánh dấu có đang hiện khung Mini hoạt động
        /// </summary>
        public bool ShowUIMiniEventBroadboard { get; set; } = false;
        #endregion


        #region Tương quan hệ thống
        /// <summary>
        /// Đang hiển thị Set dự phòng
        /// </summary>
        public bool ShowReserveEquip { get; set; } = false;

        /// <summary>
        /// Danh sách vật phẩm trong thương khố
        /// </summary>
        public List<GoodsData> PortableGoodsDataList { get; set; } = null;

        /// <summary>
        /// Danh sách thư
        /// </summary>
        public List<MailData> MailDataList { get; set; } = null;

        /// <summary>
        /// Danh sách bạn bè theo loại
        /// </summary>
        public List<FriendData> FriendDataList { get; set; } = null;

        /// <summary>
        /// Danh sách người chơi đang chờ thêm bạn
        /// </summary>
        public List<RoleDataMini> AskToBeFriendList { get; } = new List<RoleDataMini>();

        /// <summary>
        /// ID phiên giao dịch
        /// </summary>
        public int ExchangeID { get; set; } = -1;

        /// <summary>
        /// Dữ liệu vật phẩm và tiền tệ giao dịch
        /// </summary>
        public ExchangeData ExchangeDataItem { get; set; } = null;

        /// <summary>
        /// Dữ liệu gian hàng của bản thân
        /// </summary>
        public StallData StallDataItem { get; set; } = null;

        /// <summary>
        /// Dữ liệu gian hàng đang xem của người khác
        /// </summary>
        public StallData OtherStallDataItem { get; set; } = null;
        #endregion

    }
}
