using System;
using System.Collections.Generic;
using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using Server.Data;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.MessageBox;
using FSPlay.KiemVu.UI.LoadingResources;
using FSPlay.KiemVu.Entities;
using FSPlay.GameFramework.Controls;
using FSPlay.KiemVu.Control;
using FSPlay.KiemVu.Factory;

namespace FSPlay.GameFramework.Logic
{
	/// <summary>
	/// Quản lý các đối tượng toàn cục dùng cho Game
	/// </summary>
	public static class Super
    {
        #region Định nghĩa biến toàn cục
        /// <summary>
        /// Kết nối tới máy chủ thất bại
        /// </summary>
        public static bool ConnectToGameServerFailed { get; set; } = false;
        #endregion


        #region UI bảng thông báo đợi
        /// <summary>
        /// Hiện UI bảng thông báo đợi
        /// </summary>  
        public static void ShowNetWaiting(string message = null)
        {
            Super.ShowMessageBox(message, false);
        }

        /// <summary>
        /// Ẩn UI bảng thông báo đợi
        /// </summary>  
        public static void HideNetWaiting()
        {
            Super.HideMessageBox();
        }

        #endregion



        #region Bảng thông báo
        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = "Thông báo";
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = false;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = null;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, bool isShowButtonOK)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = "Thông báo";
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = isShowButtonOK;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = null;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowMessageBox(string title, string content)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = title;
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = false;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = null;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowMessageBox(string title, string content, bool isShowButtonOK)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = title;
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = isShowButtonOK;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = null;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, Action OnOK)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = "Thông báo";
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = true;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = OnOK;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowMessageBox(string title, string content, Action OnOK)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = title;
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = true;
            uiMessageBox.ShowButtonCancel = false;
            uiMessageBox.OnOK = OnOK;
            uiMessageBox.OnCancel = null;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, Action OnOK, Action OnCancel)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = "Thông báo";
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = true;
            uiMessageBox.ShowButtonCancel = true;
            uiMessageBox.OnOK = OnOK;
            uiMessageBox.OnCancel = OnCancel;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="OnOK"></param>
        /// <param name="OnCancel"></param>
        public static void ShowMessageBox(string title, string content, Action OnOK, Action OnCancel)
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Title = title;
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = true;
            uiMessageBox.ShowButtonCancel = true;
            uiMessageBox.OnOK = OnOK;
            uiMessageBox.OnCancel = OnCancel;
            uiMessageBox.Show();
        }

        /// <summary>
        /// Ẩn bảng thông báo
        /// </summary>
        public static void HideMessageBox()
        {
            UIMessageBox uiMessageBox = Global.MainCanvas.GetComponent<KiemVu.UI.CanvasManager>().UIMessageBox;
            uiMessageBox.Hide();
        }
        #endregion

        #region 初始化Xap传入的自定义参数（间接调用Global中的函数，主要是为了代码一直兼容）

        /// <summary>
        /// 根据名称获取Xap的自定义参数，以字符串类型返回
        /// </summary>  
        public static String GetXapParamByName(String name, String defVal = "")
        {
            return Global.GetXapParamByName(name, defVal);
        }

        #endregion

        #region Login Scene
        /// <summary>
        /// Login Scene
        /// </summary>
        private static RectTransform LoginScene = null;

        /// <summary>
        /// Tải xuống màn Login Scene
        /// </summary>
        public static void LoadLoginScene()
        {
            if (Super.LoginScene != null)
            {
                return;
            }
            RectTransform loginScene = Resources.Load<RectTransform>("KiemVu/Prefabs/Login Scene");
            Super.LoginScene = GameObject.Instantiate<RectTransform>(loginScene);
            CanvasManager.Instance.AddUI(Super.LoginScene);
        }

        /// <summary>
        /// Xóa màn Login Scene
        /// </summary>
        public static void DestroyLoginScene()
        {
            if (Super.LoginScene != null)
            {
                GameObject.Destroy(Super.LoginScene.gameObject);
                Super.LoginScene = null;
            }
        }
        #endregion

        #region Tải xuống/Làm mới/Đăng ký/Đăng nhập trò chơi
        /// <summary>
        /// Màn hình mặc định ban đầu mở App
        /// </summary>
        private static UIFirstScreen FirstScreen = null;

        /// <summary>
        /// Hiển thị màn hình mặc định ban đầu mở App
        /// </summary>
        public static void ShowFirstScreen()
        {
            /// Nếu đang hiện thì bỏ qua
            if (Super.FirstScreen != null)
            {
                return;
            }
            Super.FirstScreen = CanvasManager.Instance.LoadUIPrefab<UIFirstScreen>("FirstScreen/UIFirstScreen");
            CanvasManager.Instance.AddUI(Super.FirstScreen);
        }

        /// <summary>
        /// Đóng màn hình mặc định ban đầu mở App
        /// </summary>
        public static void CloseFirstScreen()
        {
            /// Nếu đang hiện
            if (Super.FirstScreen != null)
            {
                /// Xóa đối tượng
                GameObject.Destroy(Super.FirstScreen.gameObject);
                Super.FirstScreen = null;
            }
        }

        /// <summary>
        /// Hiển thị màn hình Download dữ liệu
        /// </summary>
        public static void ShowLoadingResources(UpdateFiles toDownloadFiles, Action done, Action faild)
        {
            UILoadingResources loadingResource = CanvasManager.Instance.LoadUIPrefab<UILoadingResources>("LoadingResources/UILoadingResources");
            CanvasManager.Instance.AddUI(loadingResource);
            loadingResource.ToDownloadFiles = toDownloadFiles;
            loadingResource.NextStep = done;
            loadingResource.Faild = faild;

        }

        /// <summary>
        /// Hiển thị màn hình tải dữ liệu đầu game
        /// </summary>  
        public static void ShowLoadingGame()
        {
            FSPlay.KiemVu.UI.LoadingGame.UILoadingGame loadingGame = FSPlay.KiemVu.UI.CanvasManager.Instance.LoadUIPrefab<FSPlay.KiemVu.UI.LoadingGame.UILoadingGame>("LoadingGame/UILoadingGame");
            FSPlay.KiemVu.UI.CanvasManager.Instance.AddUI(loadingGame);
            loadingGame.NextStep = () => {
                FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(loadingGame);
                Super.ShowGameLogin();
            };
        }

        /// <summary>
        /// Hiển thị màn hình đăng nhập và đăng ký
        /// </summary>
        /// <param name="root"></param>
        public static void ShowGameLogin()
        {
            /// Hiển thị màn hình LoginScene
            Super.LoadLoginScene();

            FSPlay.KiemVu.UI.LoginGame.UILoginGame loginGame = FSPlay.KiemVu.UI.CanvasManager.Instance.LoadUIPrefab<FSPlay.KiemVu.UI.LoginGame.UILoginGame>("LoginGame/UILoginGame");
            FSPlay.KiemVu.UI.CanvasManager.Instance.AddUI(loginGame);
            loginGame.LoginSuccess = () => {
                Super.ShowNetWaiting("Đang tải thông tin Server...");
                FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(loginGame);
                Super.ShowSelectServer();
            };
        }

        /// <summary>
        /// Hiển thị màn hình chọn Server
        /// </summary>
        public static void ShowSelectServer()
        {
            FSPlay.KiemVu.UI.SelectServer.UISelectServer selectServer = FSPlay.KiemVu.UI.CanvasManager.Instance.LoadUIPrefab<FSPlay.KiemVu.UI.SelectServer.UISelectServer>("SelectServer/UISelectServer");
            FSPlay.KiemVu.UI.CanvasManager.Instance.AddUI(selectServer);
            selectServer.NextStep = (o, e) => {
                Super.FakeConnectToLineServer();
                Super.ShowRoleManager();
                FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(selectServer);
            };
        }

        /// <summary>
        /// Tạo kết nối giả tới Server
        /// </summary>
        public static void FakeConnectToLineServer()
        {
            string loginIP = Super.GetXapParamByName("serverip", "127.0.0.1");
            Global.LineDataList = new List<LineData>();
            LineData lineData = new LineData()
            {
                LineID = 1,
                GameServerIP = loginIP,
                GameServerPort = Global.GetGameServerPort(),
                OnlineCount = 0,
            };

            Global.LineDataList.Add(lineData);
            Global.CurrentListData = Global.LineDataList[0];
        }

        /// <summary>
        /// Hiển thị màn hình quản lý nhân vật (gồm tạo nhân vật, xóa nhân vật, chọn nhân vật)
        /// </summary>  
        public static void ShowRoleManager()
        {
            FSPlay.KiemVu.UI.RoleManager.UIRoleManager roleManager = FSPlay.KiemVu.UI.CanvasManager.Instance.LoadUIPrefab<FSPlay.KiemVu.UI.RoleManager.UIRoleManager>("RoleManager/UIRoleManager");
            FSPlay.KiemVu.UI.CanvasManager.Instance.AddUI(roleManager);
            roleManager.DirectLogin = GameInstance.Game.CurrentSession.RoleID != -1;

			if (KuaFuLoginManager.DirectLogin())
            {
                roleManager.DirectLogin = true;
            }									
            roleManager.StartGameByRole = () =>
            {
                FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(roleManager);
                Super.ShowLoadingMap((s, e) => {
                        
                });
            };

            roleManager.GoBack = () =>
            {
                FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(roleManager);
                /// Đóng Socket
                PreGameTCPCmdHandler.Instance.CloseSocket(true);
                /// Ngắt TCPGame
                GameInstance.Game.Disconnect();
                GameInstance.Game = null;
                /// Tạo mới TCPGame
                GameInstance.Game = new TCPGame();
            };
        }

        /// <summary>
        /// Màn hình tải bản đồ hiện tại
        /// </summary>
        public static FSPlay.KiemVu.UI.LoadingMap.UILoadingMap CurrentLoadingMap { get; private set; } = null;

        /// <summary>
        /// Xóa màn hình tải bản đồ
        /// </summary>
        public static void DestroyLoadingMap()
        {
            if (null == Super.CurrentLoadingMap)
            {
                return;
            }

            FSPlay.KiemVu.UI.CanvasManager.Instance.RemoveUI(Super.CurrentLoadingMap);
        }

        /// <summary>
        /// Hiển thị màn hình tải map
        /// </summary>  
        public static void ShowLoadingMap(System.EventHandler WorkFinished = null)
        {
            /// Thực hiện xóa rác
            KTResourceManager.Instance.OnSceneChanged();

            Super.DestroyLoadingMap();
            FSPlay.KiemVu.UI.LoadingMap.UILoadingMap loadingMap = FSPlay.KiemVu.UI.CanvasManager.Instance.LoadUIPrefab<FSPlay.KiemVu.UI.LoadingMap.UILoadingMap>("LoadingMap/UILoadingMap");
            loadingMap.DoDestroy = () => {
                Super.CurrentLoadingMap = null;
            };
            FSPlay.KiemVu.UI.CanvasManager.Instance.AddUI(loadingMap);
            loadingMap.MapCode = Global.Data.RoleData.MapCode;
            loadingMap.WorkFinished = (o, e) => {
                if (PlayZone.GlobalPlayZone == null)
                {
                    Super.ShowGamePlayZone();
                }
                else
                {
                    PlayZone.GlobalPlayZone.LoadScene(Global.Data.RoleData.MapCode, Global.Data.RoleData.PosX, Global.Data.RoleData.PosY, 0);
                }
                /// Xóa màn hình LoginScene
                Super.DestroyLoginScene();

                WorkFinished?.Invoke(o, e);
            };
            loadingMap.BeginLoad2DMapRes();
            Super.CurrentLoadingMap = loadingMap;
        }

        /// <summary>
        /// Hiển thị quản lý Game
        /// </summary>  
        public static void ShowGamePlayZone()
        {
            GameObject go = new GameObject("PlayZone");
            go.AddComponent<PlayZone>();
        }
        #endregion
    }
}
