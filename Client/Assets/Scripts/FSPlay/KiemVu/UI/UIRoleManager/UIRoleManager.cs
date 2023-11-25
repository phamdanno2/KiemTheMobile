using FSPlay.GameFramework.Logic;
using System;
using UnityEngine;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Logic;
using FSPlay.GameFramework.Controls;

namespace FSPlay.KiemVu.UI.RoleManager
{
	/// <summary>
	/// Màn hình quản lý nhân vật
	/// </summary>
	public class UIRoleManager : TTMonoBehaviour
    {
        #region Define
        /// <summary>
        /// Màn hình chọn nhân vật
        /// </summary>
        [SerializeField]
        private UISelectRole UISelectRole;

        /// <summary>
        /// Khung tạo nhân vật
        /// </summary>
        [SerializeField]
        private UICreateRole UICreateRole;
        #endregion

        #region Private fields

        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện vào Game với nhân vật đã chọn
        /// </summary>
        public Action StartGameByRole { get; set; } = null;

        /// <summary>
        /// Sự kiện quay trở lại màn hình chọn Server
        /// </summary>
        public Action GoBack { get; set; } = null;

        /// <summary>
        /// Đăng nhập vào Game trực tiếp không cần ID nhân vật
        /// </summary>
        public bool DirectLogin { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();

            /// Tạo mới kết nối
            PreGameTCPCmdHandler.Instance.NewConnection();

            this.Refresh();

            /// Thực hiện kiểm tra trạng thái gói tin
            PreGameTCPCmdHandler.Instance.StartCheckSocketStatus();					  
        }
        #endregion

        #region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
        {
            
        }
        #endregion

        #region Private methods
        /// <summary>
		/// Làm mới
		/// </summary>
		private void Refresh()
        {
            /// Hiện màn hình chọn nhân vật
            this.ShowSelectRole();

            /// Tạo mới TCP Socket
            PreGameTCPCmdHandler.Instance.SocketClosed = (isError) => {
                GameObject.Destroy(this.gameObject);

                /// Nếu bị lỗi
                if (isError)
                {
                    /// Hiện màn hình danh sách Server
                    Super.ShowSelectServer();
                }
            };
            PreGameTCPCmdHandler.Instance.UserConnected = () => {
                this.Refresh();
            };
            PreGameTCPCmdHandler.Instance.SocketSuccess = () => {
                if (!this.DirectLogin)
                {
                    GameInstance.Game.GetRoleList(Global.Data.GameServerID);
                }
                Super.HideNetWaiting();
                if (this.DirectLogin)
                {
                    Super.ShowNetWaiting("Đang vào game...");
                    GameInstance.Game.InitPlayGame();
                }
            };
            PreGameTCPCmdHandler.Instance.EnterGame = () => {
                this.StartGameByRole?.Invoke();
            };
            PreGameTCPCmdHandler.Instance.EnterGameWithNewCreatedRole = (e) => {
                this.UISelectRole.AddRoleList(e.fields[1], e.CmdID == (int) (TCPGameServerCmds.CMD_CREATE_ROLE));
                if (e.CmdID == (int) (TCPGameServerCmds.CMD_CREATE_ROLE))
                {
                    this.UISelectRole.DirectEnterGame();
                }
            };
            PreGameTCPCmdHandler.Instance.CreateRole = () => {
                if (this.UISelectRole.RolesCount <= 0)
                {
                    this.ShowCreateRole();
                }
            };
            PreGameTCPCmdHandler.Instance.GetNewbieVillage = (villages) => {
                this.UICreateRole.Villages = villages;
            };
            PreGameTCPCmdHandler.Instance.InitGameError = () => {
                this.GoBack?.Invoke();
            };
            PreGameTCPCmdHandler.Instance.SynsTimeEnterGame = () => {
                if (!this.DirectLogin)
                {
                    this.UISelectRole.StartGame();
                }
                else
                {
                    PreGameTCPCmdHandler.Instance.CloseSocket(false);
                    this.StartGameByRole?.Invoke();
                }
            };
        }

        /// <summary>
        /// Vào game
        /// </summary>
        private void EnterGame()
        {
            Super.ShowNetWaiting("Đang vào game...");
            GameInstance.Game.InitPlayGame();

            // Login vào server đã chọn
            if (Global.Data.ServerData != null)
            {
                if (Global.Data.ServerData.LastServer != null)
                {
                    PlatformUserLogin.RecordLoginServerIDs(Global.Data.ServerData.LastServer);
                }
            }
        }

        /// <summary>
        /// Gửi gói tin lấy danh sách tân thủ thôn
        /// </summary>
        private void SendPacketGetVillagesList()
        {
            /// Nếu gói tin không tồn tại hoặc Session chưa được tạo ra thì không làm gì cả
            if (GameInstance.Game == null || GameInstance.Game.CurrentSession == null)
            {
                Super.ShowMessageBox("Lỗi", "Có lỗi xảy ra, hãy thử đăng nhập lại trò chơi!", () => {
                    this.GoBack?.Invoke();
                    Super.ShowSelectServer();
                });
                return;
            }

            GameInstance.Game.GetNewbieVillages();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiển thị màn hình tạo nhân vật
        /// </summary>
        public void ShowCreateRole()
        {
            this.SendPacketGetVillagesList();
            
            this.UISelectRole.gameObject.SetActive(false);
            this.UICreateRole.gameObject.SetActive(true);

            this.UICreateRole.GoBack = () => {
                PreGameTCPCmdHandler.Instance.CloseSocket(true);
                this.GoBack?.Invoke();
            };
        }

        /// <summary>
        /// Hiển thị màn hình chọn nhân vật
        /// </summary>
        public void ShowSelectRole()
        {
            this.UISelectRole.gameObject.SetActive(true);
            this.UICreateRole.gameObject.SetActive(false);

            this.UISelectRole.StartGameByRole = () => {
                PreGameTCPCmdHandler.Instance.CloseSocket(false);

                this.StartGameByRole?.Invoke();
            };

            this.UISelectRole.EnterGame = () => {
                if (this.UISelectRole.LastSelectedRole == null)
                {
                    Super.ShowMessageBox("Lỗi", "Xin hãy chọn một nhân vật.", true);
                    return;
                }

                GameInstance.Game.CurrentSession.RoleID = this.UISelectRole.LastSelectedRole.ID;
                GameInstance.Game.CurrentSession.RoleSex = this.UISelectRole.LastSelectedRole.Sex;
                GameInstance.Game.CurrentSession.RoleName = this.UISelectRole.LastSelectedRole.Name;

                this.EnterGame();
            };

            this.UISelectRole.BackToSelectServer = () => {
                PreGameTCPCmdHandler.Instance.CloseSocket(true);
                this.GoBack?.Invoke();
            };

            this.UISelectRole.QuitGame = () => {
                PreGameTCPCmdHandler.Instance.CloseSocket(true);

                Application.Quit();
            };

            this.UISelectRole.CreateRole = () => {
                this.ShowCreateRole();
            };

            if (GameInstance.Game.ConnectedState)
            {
                PreGameTCPCmdHandler.Instance.NewTCPGame();
            }

            PreGameTCPCmdHandler.Instance.BeginHandle();

            if (null != Global.CurrentListData)
            {
                //Super.ShowNetWaiting("Đang tải danh sách nhân vật...");
                //Super.GData.FirstEnterGameServer = false;

                if (KuaFuLoginManager.LoginKuaFuServer(out string ip, out int port))
                {
                    GameInstance.Game.Connect(ip, port);
                }
                else
                {
                    GameInstance.Game.Connect(Global.CurrentListData.GameServerIP, Global.CurrentListData.GameServerPort);
                }
            }
        }
        #endregion
    }

}