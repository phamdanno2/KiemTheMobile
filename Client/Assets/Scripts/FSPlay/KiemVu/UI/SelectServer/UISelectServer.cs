using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Server.Data;
using FSPlay.GameEngine.Logic;
using UnityEngine.Networking;
using FSPlay.GameFramework.Logic;
using Server.Tools;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Network.Protocol;
using System.Text;
using FSPlay.GameEngine.Scene;
using HSGameEngine.GameEngine.Network;
using HSGameEngine.GameEngine.Network.Protocol;

namespace FSPlay.KiemVu.UI.SelectServer
{
    /// <summary>
    /// Khung chọn Server
    /// </summary>
    public class UISelectServer : MonoBehaviour
    {
        /// <summary>
        /// Button thông tin máy chủ
        /// </summary>
        [Serializable]
        private class ButtonServerDetails
        {
            /// <summary>
            /// Button 
            /// </summary>
            public UnityEngine.UI.Button UIButton_ServerDetails;

            /// <summary>
            /// Text tên máy chủ
            /// </summary>
            public TextMeshProUGUI UIText_ServerName;

            /// <summary>
            /// Trạng thái máy chủ
            /// </summary>
            public UnityEngine.UI.Image UIImage_ServerStatus;

            /// <summary>
            /// Text trạng thái máy chủ
            /// </summary>
            public TextMeshProUGUI UIText_ServerDescription;

            /// <summary>
            /// Bundle chứa ảnh
            /// </summary>
            public string BundleDir;

            /// <summary>
            /// Atlas
            /// </summary>
            public string AtlasName;

            /// <summary>
            /// Tên Sprite ở trạng thái bảo trì
            /// </summary>
            public string MaintenanceSprite;

            /// <summary>
            /// Tên Sprite ở trạng thái tốt
            /// </summary>
            public string GoodSprite;

            /// <summary>
            /// Tên Sprite ở trạng thái bận
            /// </summary>
            public string BusySprite;

            /// <summary>
            /// Tên Sprite ở trạng thái đày
            /// </summary>
            public string FullSprite;
        }

        #region Define
        /// <summary>
        /// Button quay lại màn hình Login
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_BackToLogin;

        /// <summary>
        /// Khung chơi ngay
        /// </summary>
        [SerializeField]
        private GameObject UIFrame_PlayNow;

        /// <summary>
        /// Khung chọn máy chủ
        /// </summary>
        [SerializeField]
        private GameObject UIFrame_SelectServer;

        #region Select Server
        /// <summary>
        /// Prefab danh sách cụm máy chủ
        /// </summary>
        [SerializeField]
        private UISelectServer_ToggleGroupServer UIToggle_ServerGroupPrefab;

        /// <summary>
        /// Tập hợp Buttons thông tin máy chủ trong cụm
        /// </summary>
        [SerializeField]
        private ButtonServerDetails[] UIButtons_ListServer;
        #endregion

        #region Play Now
        /// <summary>
        /// Text tên máy chủ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayNowServerName;

        /// <summary>
        /// Text trạng thái máy chủ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayNowServerStatus;

        /// <summary>
        /// Nút chơi ngay
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_PlayNow;

        /// <summary>
        /// Nút quay lại màn chọn máy chủ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_BackToSelectServer;
        #endregion
        #endregion

        /// <summary>
        /// Gọi đến khi có sự kiện chọn Server
        /// </summary>
        public EventHandler NextStep { get; set; }

        /// <summary>
        /// Socket
        /// </summary>
        private TCPClient tcpClient = new TCPClient(2);


        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi dến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.StartCoroutine(this.GetFullData());
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_BackToLogin.onClick.AddListener(this.ButtonBackToLogin_Click);
            this.UIButton_BackToSelectServer.onClick.AddListener(this.ShowSelectServerFrame);
        }

        /// <summary>
        /// Sự kiện khi nút quay lại màn hình Login được ấn
        /// </summary>
        private void ButtonBackToLogin_Click()
        {
            GameObject.Destroy(this.gameObject);
            Super.ShowGameLogin();
        }

        /// <summary>
        /// Sự kiện khi Server được chọn
        /// </summary>
        /// <param name="serverInfo"></param>
        private void ButtonServer_Click(BuffServerInfo serverInfo)
        {
            if (serverInfo == null)
            {
                Super.ShowMessageBox("Lỗi", "Máy chủ được lựa chọn không chính xác.", true);
                return;
            }
            if (serverInfo.nStatus == 1)
            {
                Super.ShowMessageBox("Lỗi", "Máy chủ được chọn hiện đang bảo trì.", true);
                return;
            }

            if (serverInfo != null)
            {
                Global.RootParams["serverip"] = serverInfo.strURL;
                Global.RootParams["gameport"] = "" + serverInfo.nServerPort;
                Global.RootParams["loginport"] = "" + serverInfo.nServerPort;

                if (Global.Data.ServerData != null)
                {
                    Global.Data.ServerData.LastServer = serverInfo;
                }

                PlayerPrefs.SetInt("NewLastServerInfoID", serverInfo.nServerID);
                this.ConnectToLoginServer();
            }
        }

        /// <summary>
        /// Hiển thị màn hình chơi
        /// </summary>
        private void ShowPlayNowFrame()
        {
            this.UIFrame_PlayNow.SetActive(true);
            this.UIFrame_SelectServer.SetActive(false);
        }

        /// <summary>
        /// Hiển thị màn hình chọn máy chủ
        /// </summary>
        private void ShowSelectServerFrame()
        {
            this.UIFrame_PlayNow.SetActive(false);
            this.UIFrame_SelectServer.SetActive(true);
        }

        /// <summary>
        /// Cập nhật thông tin Server ở màn hình chơi ngay
        /// </summary>
        /// <param name="serverInfo"></param>
        private void ShowPlayNowServerInfo(BuffServerInfo serverInfo)
        {
            if (serverInfo == null)
            {
                this.UpdateGroupServerInfo();
                return;
            }

            this.ShowPlayNowFrame();

            this.UIText_PlayNowServerName.text = serverInfo.strServerName;
            switch (serverInfo.nStatus)
            {
                case 4:                     // Tốt
                    this.UIText_PlayNowServerStatus.text = "Trạng thái: <color=#00ff00>Tốt</color>";
                    break;
                case 3:                     // Bận
                    this.UIText_PlayNowServerStatus.text = "Trạng thái: <color=#ffa333>Bận</color>";
                    break;
                case 2:                     // Đầy
                    this.UIText_PlayNowServerStatus.text = "Trạng thái: <color=#ff2e2e>Đầy</color>";
                    break;
                default:                    // Bảo trì
                    this.UIText_PlayNowServerStatus.text = "Trạng thái: <color=#adadad>Bảo trì</color>";
                    break;
            }

            this.UIButton_PlayNow.onClick.RemoveAllListeners();
            this.UIButton_PlayNow.onClick.AddListener(() =>
            {
                this.ButtonServer_Click(serverInfo);
            });
        }

        /// <summary>
        /// Cập nhật danh sách máy chủ
        /// </summary>
        /// <param name="listServer"></param>
        private void UpdateServerInfo(List<BuffServerInfo> listServer)
        {
            for (int i = 0; i < this.UIButtons_ListServer.Length; i++)
            {
                this.UIButtons_ListServer[i].UIButton_ServerDetails.gameObject.SetActive(false);
            }

            if (listServer == null)
            {
                return;
            }

            for (int i = 0; i < Math.Min(listServer.Count, this.UIButtons_ListServer.Length); i++)
            {
                BuffServerInfo serverInfo = listServer[i];
                this.UIButtons_ListServer[i].UIButton_ServerDetails.gameObject.SetActive(true);
                this.UIButtons_ListServer[i].UIText_ServerName.text = serverInfo.strServerName;
                SpriteFromAssetBundle sprite = this.UIButtons_ListServer[i].UIImage_ServerStatus.gameObject.GetComponent<SpriteFromAssetBundle>();
                sprite.BundleDir = this.UIButtons_ListServer[i].BundleDir;
                sprite.AtlasName = this.UIButtons_ListServer[i].AtlasName;
                switch (serverInfo.nStatus)
                {
                    case 4:                     // Tốt
                        sprite.SpriteName = this.UIButtons_ListServer[i].GoodSprite;
                        break;
                    case 3:                     // Bận
                        sprite.SpriteName = this.UIButtons_ListServer[i].BusySprite;
                        break;
                    case 2:                     // Đầy
                        sprite.SpriteName = this.UIButtons_ListServer[i].FullSprite;
                        break;
                    default:                    // Bảo trì
                        sprite.SpriteName = this.UIButtons_ListServer[i].MaintenanceSprite;
                        break;
                }
                sprite.Load();
                //this.UIButtons_ListServer[i].UIText_ServerDescription.text = string.Format("Số lượng online: <color=#00ff00>{0}</color>", listServer[i].nOnlineNum);
                //this.UIButtons_ListServer[i].UIText_ServerDescription.text = string.Format("Server ID: <color=#00ff00>{0}</color> - IP: <color=#00ff00>{1}</color> - Port: <color=#00ff00>{2}</color>", listServer[i].nServerID, listServer[i].strURL, listServer[i].nServerPort);
                this.UIButtons_ListServer[i].UIText_ServerDescription.text = listServer[i].Msg;
                this.UIButtons_ListServer[i].UIButton_ServerDetails.onClick.RemoveAllListeners();
                this.UIButtons_ListServer[i].UIButton_ServerDetails.onClick.AddListener(() => {
                    this.ButtonServer_Click(serverInfo);
                });
            }
        }

        /// <summary>
        /// Cập nhật danh sách cụm máy chủ
        /// </summary>
        private void UpdateGroupServerInfo()
        {
            this.ShowSelectServerFrame();

            XuanFuServerData serverData = Global.Data.ServerData;

            if (serverData == null)
            {
                return;
            }
            int count = serverData.ServerInfos.Count;

            int totalServerItemCount = 0;
            int modelCount = 0;
            if (count % 10 == 0)
            {
                totalServerItemCount = count / 10;
            }
            else
            {
                totalServerItemCount = count / 10 + 1;
            }
            modelCount = count % 10;
            int startIndex = 0;
            int endIndex = 0;

            UISelectServer_ToggleGroupServer toggleRecommendedListServer = GameObject.Instantiate<UISelectServer_ToggleGroupServer>(this.UIToggle_ServerGroupPrefab);
            toggleRecommendedListServer.Group = this.UIToggle_ServerGroupPrefab.transform.parent.gameObject.GetComponent<UnityEngine.UI.ToggleGroup>();
            toggleRecommendedListServer.transform.SetParent(this.UIToggle_ServerGroupPrefab.transform.parent, false);
            toggleRecommendedListServer.gameObject.SetActive(true);
            toggleRecommendedListServer.Name = "Máy chủ đề cử";
            toggleRecommendedListServer.OnActivated = (isSelected) =>
            {
                toggleRecommendedListServer.Active = isSelected;
                if (isSelected)
                {
                    this.UpdateServerInfo(serverData.RecommendServerInfos);
                }
            };
            toggleRecommendedListServer.Active = false;
            IEnumerator SelectFirst()
            {
                yield return null;
                toggleRecommendedListServer.Active = true;
            }
            this.StartCoroutine(SelectFirst());

            for (int i = totalServerItemCount - 1; i >= 0; i--)
            {
                UISelectServer_ToggleGroupServer toggleListServerByGroup = GameObject.Instantiate<UISelectServer_ToggleGroupServer>(this.UIToggle_ServerGroupPrefab);
                toggleListServerByGroup.Group = this.UIToggle_ServerGroupPrefab.transform.parent.gameObject.GetComponent<UnityEngine.UI.ToggleGroup>();
                toggleListServerByGroup.transform.SetParent(this.UIToggle_ServerGroupPrefab.transform.parent, false);
                toggleListServerByGroup.gameObject.SetActive(true);
                if (i < totalServerItemCount - 1)
                {
                    startIndex = i * 10;
                    endIndex = i * 10 + 9;
                    toggleListServerByGroup.Name = string.Format("Cụm {0} - {1}", (startIndex + 1), (endIndex + 1));
                }
                else
                {
                    startIndex = i * 10;
                    if (modelCount != 0)
                    {
                        endIndex = i * 10 + modelCount - 1;
                    }
                    else
                    {
                        endIndex = i * 10 + 10 - 1;
                    }
                    toggleListServerByGroup.Name = string.Format("Cụm {0} - {1}", (startIndex + 1), (endIndex + 1));
                }

                List<BuffServerInfo> listServers = new List<BuffServerInfo>();
                for (int j = startIndex; j <= endIndex; j++)
                {
                    listServers.Add(serverData.ServerInfos[j]);
                }
                toggleListServerByGroup.OnActivated = (isSelected) =>
                {
                    toggleListServerByGroup.Active = isSelected;
                    if (isSelected)
                    {
                        this.UpdateServerInfo(listServers);
                    }
                };
                toggleListServerByGroup.Active = false;
            }
        }
        #endregion

        #region Network
        /// <summary>
        /// Lấy dữ liệu từ Socket vào màn hình chọn Server
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetFullData()
        {


            string url = MainGame.GameInfo.ServerListURL;

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("strUID", "-1");

            UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
            yield return www.SendWebRequest();

            if (!string.IsNullOrEmpty(www.error))
            {
                Super.ShowMessageBox("Lỗi đăng nhập", "Không thể tải danh sách máy chủ. Hãy thử thoát game và đăng nhập lại.", true);
                yield break;
            }

            BuffServerListDataEx listDataEx = DataHelper.BytesToObject<BuffServerListDataEx>(www.downloadHandler.data, 0, www.downloadHandler.data.Length);

            if (listDataEx == null || listDataEx.RecommendListServerData == null || listDataEx.RecommendListServerData.Count == 0)
            {
                Super.ShowMessageBox("Lỗi đăng nhập", "Dịch vụ đang bảo trì, hãy thử đăng nhập lại sau.", true);
                yield break;
            }


            Global.Data.ServerData = new XuanFuServerData();

            Global.Data.ServerData.ServerInfos = listDataEx.ListServerData;


            if (listDataEx.RecommendListServerData.Count > 0)
            {
                Global.Data.ServerData.RecommendServer = listDataEx.RecommendListServerData[0];
            }
            if (listDataEx.ListServerData.Count > 0)
            {
                Global.Data.ServerData.LastServer = listDataEx.ListServerData[0];
            }
            else
            {
                if (listDataEx.RecommendListServerData.Count > 0)
                {
                    Global.Data.ServerData.LastServer = listDataEx.RecommendListServerData[0];
                }
            }

            www.Dispose();
            www = null;

            Super.HideNetWaiting();
            if (listDataEx.LastServerLogin != null)
            {
                bool isFound = false;

                foreach (BuffServerInfo data in listDataEx.ListServerData)
                {
                    if (data.nServerID == listDataEx.LastServerLogin.nServerID)
                    {
                        listDataEx.LastServerLogin = data;
                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                {
                    this.ShowPlayNowServerInfo(listDataEx.LastServerLogin);
                    yield break;
                }
            }
            this.UpdateGroupServerInfo();
        }


        #region TCP Client
        /// <summary>
        /// Kết nối tới Server
        /// </summary>		
        public void ConnectToLoginServer()
        {
            this.ResetTCPClient();

            this.tcpClient.SocketConnect += this.SocketConnect;
            String loginIP = Super.GetXapParamByName("serverip", "127.0.0.1");
            Super.ShowNetWaiting("Đang kết nối tới Server...");
            this.tcpClient.Connect(loginIP, Global.GetUserLoginPort());
        }

        /// <summary>
        /// Làm mới Socket
        /// </summary>		
        public void ResetTCPClient()
        {
            if (null != tcpClient)
            {
                this.tcpClient.Disconnect();
                this.tcpClient.Destroy();
                this.tcpClient = new TCPClient(2);
            }
        }

        /// <summary>
        /// Hàm gọi đến mỗi khi gói tin được nhận
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketConnect(object sender, SocketConnectEventArgs e)
        {
            MainGame.Instance.QueueOnMainThread(() =>
            {
                int netSocketType = e.NetSocketType;
                switch (netSocketType)
                {
                    case (int)NetSocketTypes.SOCKET_CONN:
                        if (e.Error == "Success")
                        {
                            String strcmd = "";
                            byte[] bytesCmd = null;
                            int loginCmdID = -1;


                            String uid = Global.StringReplaceAll(Super.GetXapParamByName("uid", ""), ":", "");
                            String name = Global.StringReplaceAll(Super.GetXapParamByName("n", ""), ":", "");
                            String lastTime = Global.StringReplaceAll(Super.GetXapParamByName("t", ""), ":", "");
                            String isadult = Global.StringReplaceAll(Super.GetXapParamByName("cm", ""), ":", "");
                            String token = Global.StringReplaceAll(Super.GetXapParamByName("token", ""), ":", "");
                            loginCmdID = (int)(TCPLoginServerCmds.CMD_LOGIN_ON2);

                            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", (int)(TCPCmdProtocolVer.VerSign), uid, name, lastTime, isadult, token);
                            bytesCmd = new UTF8Encoding().GetBytes(strcmd);

                            TCPOutPacket tcpOutPacket = new TCPOutPacket();
                            tcpOutPacket.PacketCmdID = (short)(loginCmdID);
                            tcpOutPacket.FinalWriteData(bytesCmd, 0, bytesCmd.Length);
                            this.tcpClient.SendData(tcpOutPacket);
                        }
                        else
                        {
                            Super.HideNetWaiting();
                            Super.ShowMessageBox("Lỗi đăng nhập", "Kết nối tới máy chủ thất bại, xin hãy thử lại sau.", true);
                        }
                        break;
                    case (int)NetSocketTypes.SOCKET_SEND:
                        Super.HideNetWaiting();
                        Super.ShowMessageBox("Lỗi đăng nhập", "Không thể gửi gói tin tới máy chủ.", true);
                        break;
                    case (int)NetSocketTypes.SOCKET_RECV:
                        break;
                    case (int)NetSocketTypes.SOCKET_CLOSE:
                        GScene.ServerStopGame();
                        Super.HideNetWaiting();
                        Super.ShowMessageBox("Lỗi đăng nhập", "Kết nối tới máy chủ bị gián đoạn.", true);
                        break;
                    case (int)NetSocketTypes.SOCKT_CMD:
                        this.tcpClient.Disconnect();
                        if ("-1" == e.fields[0])
                        {
                            Super.HideNetWaiting();
                            Super.ShowMessageBox("Lỗi đăng nhập", "Tài khoản hoặc mật khẩu nhập vào không chính xác.", true);
                        }
                        else if ("-2" == e.fields[0])
                        {
                            Super.HideNetWaiting();
                            Super.ShowMessageBox("Lỗi đăng nhập", "Phiên bản Client đã cũ, hãy tiến hành cập nhật trước tiên.", true);
                        }
                        else
                        {
                            GameInstance.Game.CurrentSession.UserID = e.fields[0];
                            GameInstance.Game.CurrentSession.UserName = e.fields[1];
                            GameInstance.Game.CurrentSession.UserToken = e.fields[2];
                            GameInstance.Game.CurrentSession.UserIsAdult = Convert.ToInt32(e.fields[3]);
                            Super.HideNetWaiting();
                            this.tcpClient.SocketConnect -= this.SocketConnect;
                            this.tcpClient.Destroy();
                            this.tcpClient = null;
                            
                            this.NextStep?.Invoke(this, EventArgs.Empty);
                        }
                        break;
                    default:
                        throw new Exception("Error on Socket PlatformUserLogin");
                }
            });
        }
        #endregion
        #endregion
    }
}