using GameServer.Logic;
using FSPlay.Drawing;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network.Protocol;
using FSPlay.GameEngine.Scene;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Utilities;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static FSPlay.KiemVu.Entities.Enum;
using HSGameEngine.GameEngine.Network;
using HSGameEngine.GameEngine.Network.Protocol;
using HSGameEngine.GameEngine.Network.Tools;
using System.Linq;

namespace FSPlay.GameEngine.Network
{
	/// <summary>
	/// Đối tượng TCPGame mô tả Socket trong Game
	/// </summary>
	public class TCPGame
    {
        /// <summary>
        /// Trạng thái Socket
        /// </summary>
        public enum GameStates { CLIENT_READY = 0, CLIENT_CONNECTING, CLIENT_CONNECTED, CLIENT_LOGON };

        private GameStates _GameState = GameStates.CLIENT_READY;
        /// <summary>
        /// Trạng thái Game
        /// </summary>
        public GameStates GameState
        {
            get
            {
                return _GameState;
            }
            set
            {
                _GameState = value;
            }
        }

        /// <summary>
        /// Session hiện tại
        /// </summary>
        public Session CurrentSession = new Session();

        /// <summary>
        /// Kết nối đến máy chủ
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port, bool setEvent = true)
        {
            if (_GameState > GameStates.CLIENT_READY)
            {
                throw new Exception("TCPGame connect faild");
            }

            ActiveDisconnect = false;

            if (setEvent)
            {
                tcpClient.SocketConnect += SocketConnect;
            }

            tcpClient.Connect(ip, port);
            _GameState = GameStates.CLIENT_CONNECTING;
        }

        /// <summary>
        /// Ngắt kết nối
        /// </summary>
        public void Disconnect()
        {
            if (_GameState <= GameStates.CLIENT_READY)
            {
                KTDebug.LogError("TCPGame connect faild");
                return;
            }

            ActiveDisconnect = true;
            SpriteLogOut();
            tcpClient.Disconnect(SocketShutdown.Receive);

            GScene.ServerStopGame();

            _GameState = GameStates.CLIENT_READY;
        }

        /// <summary>
        /// Sự kiện kết nối thất bại
        /// </summary>
        public event SocketConnectEventHandler SocketFailed;

        /// <summary>
        /// Sự kiện kết nối thành công
        /// </summary>
        public event SocketConnectEventHandler SocketSuccess;

        /// <summary>
        /// Sự kiện kết nối thành công
        /// </summary>
        public event SocketConnectEventHandler SocketCommand;

        /// <summary>
        /// Đối tượng TCPClient
        /// </summary>
        private TCPClient tcpClient = new TCPClient(1);

        /// <summary>
        /// Đối tượng TCPClient tương ứng
        /// </summary>
        public TCPClient GameClient
        {
            get { return tcpClient; }
        }

        /// <summary>
        /// Làm mới TCPClient
        /// </summary>
        public void ResetGameClient()
        {
            tcpClient = new TCPClient(1);
        }

        /// <summary>
        /// Trạng thái kết nối
        /// </summary>
        public bool ConnectedState
        {
            get
            {
                return tcpClient.Connected;
            }
            set
            {
                if (null != tcpClient)
                {
                    tcpClient.Connected = value;
                }
            }
        }

        /// <summary>
        /// Sự kiện khi hết thời hạn Packet
        /// </summary>
        public void PingTimeOut()
        {
            BasePlayZone.InWaitPingCount = 0;
            SocketConnectEventArgs e = new SocketConnectEventArgs();
            ActiveDisconnect = true;
            string errMsg = string.Format("Connect to server faild.");
            e.ErrorMsg = errMsg;
            e.ReturnStartPage = false;
            e.ShowMsgBox = false;
            this.SocketFailed?.Invoke(this, e);
        }

        public bool _ActiveDisconnect = false;
        /// <summary>
        /// Chủ động ngắt kết nối
        /// </summary>
        public bool ActiveDisconnect
        {
            get
            {
                return _ActiveDisconnect;
            }
            set
            {
                _ActiveDisconnect = value;
            }
        }

        /// <summary>
        /// Handle kết nối thành công
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketConnect(object sender, SocketConnectEventArgs e)
        {
            switch ((NetSocketTypes) e.NetSocketType)
            {
                case NetSocketTypes.SOCKET_CONN:
                {
                    if (e.Error == "Success")
                    {
                        _GameState = GameStates.CLIENT_CONNECTED;

                        string strcmd = "";
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}",
                                                CurrentSession.UserID,
                                                CurrentSession.UserName,
                                                CurrentSession.UserToken,
                                                CurrentSession.RoleRandToken,
                                                (int) TCPCmdProtocolVer.VerSign,
                                                CurrentSession.UserIsAdult,
                                                KTGlobal.GetDeviceInfo()
                                                    );

                        strcmd = KuaFuLoginManager.GetKuaFuLoginString(strcmd);
                        tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) TCPGameServerCmds.CMD_LOGIN_ON));
                    }
                    else
                    {
                        ActiveDisconnect = true;
                        string errMsg = string.Format("Không thể kết nối tới máy chủ.");
                        e.ErrorMsg = errMsg;
                        e.ReturnStartPage = false;
                        e.ShowMsgBox = false;
                        this.SocketFailed?.Invoke(this, e);
                    }
                    break;
                }
                case NetSocketTypes.SOCKET_SEND:
                {
                    ActiveDisconnect = true;
                    string errMsg = string.Format("Không thể gửi gói tin đến máy chủ");
                    e.ErrorMsg = errMsg;
                    e.ReturnStartPage = false;
                    e.ShowMsgBox = true;
                    this.SocketFailed?.Invoke(this, e);

                    break;
                }
                case NetSocketTypes.SOCKET_RECV:
                {
                    break;
                }
                case NetSocketTypes.SOCKET_CLOSE:
                {
                    GScene.ServerStopGame();

                    if (!ActiveDisconnect)
                    {
                        string errMsg = string.Format("Máy chủ hiện tại đang bảo trì, xin hãy quay lại sau!");
                        e.ErrorMsg = errMsg;
                        e.ReturnStartPage = false;
                        e.ShowMsgBox = true;
                        this.SocketFailed?.Invoke(this, e);
                    }

                    break;
                }
                case NetSocketTypes.SOCKT_CMD:
                {
                    if (e.CmdID == (int) TCPGameServerCmds.CMD_LOGIN_ON)
                    {
                        int randToken = -1;
                        if (e.fields.Length > 0)
                        {
                            randToken = Convert.ToInt32(e.fields[0]);
                        }
                        if (StdErrorCode.Error_Token_Expired2 == randToken)
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Phiên đăng nhập đã hết hạn, hãy thoát Game và đăng nhập lại...");
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = true;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                        else if (StdErrorCode.Error_Connection_Closing2 == randToken || StdErrorCode.Error_Connection_Closing == randToken)
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Tài khoản hiện đang Online, hãy thử đăng nhập lại!");
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = false;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                        else if (StdErrorCode.Error_Version_Not_Match2 == randToken)
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Không đăng nhập được vào server game, phiên bản client quá cũ, vui lòng cập nhật client và đăng nhập lại!");
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = true;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                        else if (-10 == randToken)
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Tài khoản bị hệ thống chặn, hãy liên hệ với hỗ trợ!");
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = true;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                        else if (StdErrorCode.Error_Server_Connections_Limit == randToken)
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Máy chủ hiện đã đầy, hãy chọn máy chủ khác!");
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = true;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                        else if (randToken >= 0)
                        {
                            _GameState = GameStates.CLIENT_LOGON;
                            CurrentSession.RoleRandToken = randToken;
                            this.SocketSuccess?.Invoke(this, e);
                            BasePlayZone.IsSpeedCheck = true;
                            BasePlayZone.InWaitPingCount = 0;
                        }
                        else
                        {
                            ActiveDisconnect = true;
                            string errMsg = string.Format("Đăng nhập thất bại, mã lỗi: {0}", randToken);
                            e.ErrorMsg = errMsg;
                            e.ReturnStartPage = true;
                            e.ShowMsgBox = true;
                            this.SocketFailed?.Invoke(this, e);
                        }
                    }
                    else
                    {
                        this.SocketCommand?.Invoke(this, e);
                    }

                    break;
                }
                default:
                {
                    ActiveDisconnect = true;
                    this.SocketFailed?.Invoke(this, e);

                    throw new Exception("Socket exception");
                }
            }
        }

        #region Gói tin gửi đi
        /// <summary>
        /// Trả về danh sách nhân vật
        /// </summary>
        /// <param name="zoneID"></param>
        public void GetRoleList(int zoneID)
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}", CurrentSession.UserID, zoneID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_ROLE_LIST)));

            /// Gửi thông tin phiên bản lên Server
            this.SendVersion();
        }
        /// <summary>
        /// Tạo nhân vật
        /// </summary>
        /// <param name="sex">Giới tính</param>
        /// <param name="factionID">Môn phái</param>
        /// <param name="name">Tên nhân vật</param>
        /// <param name="serverID">ID Server</param>
        /// <param name="villageID">ID tân thủ thôn</param>
        public void CreateRole(int sex, int factionID, string name, int serverID, int villageID)
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", CurrentSession.UserID, CurrentSession.UserName, sex, factionID, name, serverID, villageID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_CREATE_ROLE)));
        }

        /// <summary>
        /// Tải dữ liệu danh sách tân thủ thôn cần thiết khi tạo nhân vật
        /// </summary>
        public void GetNewbieVillages()
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}", CurrentSession.UserID, CurrentSession.UserName);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_GET_NEWBIE_VILLAGES)));
        }

        /// <summary>
        /// Thực hiện gửi CMD_INIT_GAME lên
        /// </summary>
        public void InitPlayGame()
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}", CurrentSession.UserID, CurrentSession.RoleID, KTGlobal.GetDeviceInfo(), KTGlobal.GetDeviceModel());
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_INIT_GAME)));
        }

        /// <summary>
        /// Đồng bộ thời gian với GS
        /// </summary>
        public void TimeSynchronization()
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = "";
            strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, MyDateTime.Now().Ticks);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SYNC_TIME)));

            this.SendVersion();
        }

        /// <summary>
        /// Đồng bộ thời gian với GS
        /// </summary>
        public void TimeSynchronizationByClient()
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = "";
            strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, MyDateTime.Now().Ticks);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SYNC_TIME_BY_CLIENT)));
        }

        /// <summary>
        /// Gửi thông tin phiên bản
        /// </summary>
        public void SendVersion()
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", CurrentSession.RoleID, -1, MainGame.GameInfo.Version, MainGame.GameInfo.ResourceVersion);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_PUSH_VERSION)));
        }

        /// <summary>
        /// Gửi gói tin bắt đầu vào Game
        /// </summary>
        public void StartPlayGame()
        {
            if (ActiveDisconnect)
            {
                return;
            }

            string strcmd = "";
            strcmd = string.Format("{0}", CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_PLAY_GAME)));
        }

        /// <summary>
        /// Gói tin gửi đến Server thông báo Leader di chuyển theo quãng đường tìm sẵn
        /// </summary>
        /// <param name="fromPos">Tọa độ thực X</param>
        /// <param name="toPos">Tọa độ thực Y</param>
        /// <param name="pathString">Danh sách đường đi theo tọa độ thực</param>
        /// <param name="action"></param>
        /// <param name="extAction"></param>
        public void KTLeaderMoveTo(Point fromPos, Point toPos, string pathString)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            SpriteMoveData moveData = new SpriteMoveData()
            {
                RoleID = this.CurrentSession.RoleID,
                FromX = (int) fromPos.X,
                FromY = (int) fromPos.Y,
                ToX = (int) toPos.X,
                ToY = (int) toPos.Y,
                PathString = pathString,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteMoveData>(moveData);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_SPR_MOVE)));
            TCPPing.RecordSendCmd((int) (TCPGameServerCmds.CMD_SPR_MOVE));
        }

        /// <summary>
        /// Gửi gói tin lên Server thông báo đối tượng ngừng di chuyển
        /// </summary>
        /// <param name="index"></param>
        public void SpriteStopMove()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            GSprite leader = Global.Data.Leader;
            if (leader == null)
            {
                return;
            }

            byte[] bytes = DataHelper.ObjectToBytes<SpriteStopMove>(new SpriteStopMove()
            {
                RoleID = leader.RoleID,
                PosX = leader.PosX,
                PosY = leader.PosY,
                StopTick = KTGlobal.GetServerTime(),
                Direction = (int) leader.Direction,
            });
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_SPR_STOPMOVE)));
        }


        /// <summary>
        /// Kiểm tra Ping của đối tượng
        /// </summary>
        public void SpriteCheck(int processSubTicks, int dateTimeSubTicks)
        {
            if (!Global.Data.PlayGame || !BasePlayZone.IsSpeedCheck)
            {
                return;
            }

            BasePlayZone.IsSpeedCheck = false;
            string strcmd = string.Format("{0}:{1}:{2}", CurrentSession.RoleID, processSubTicks, dateTimeSubTicks);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_CHECK)));
            if (PlayZone.GlobalPlayZone.UIRolePart != null && PlayZone.GlobalPlayZone.UIRolePart.UIPingInfo != null)
            {
                PlayZone.GlobalPlayZone.UIRolePart.UIPingInfo.SendPing();
            }
        }

        /// <summary>
        /// Tick gửi lên Server đồng bộ vị trí của nhân vật hiện tại
        /// </summary>
        public void SpritePosition()
        {
            GSprite leader = Global.Data.Leader;
            if (leader == null)
            {
                return;
            }
            byte[] bData = DataHelper.ObjectToBytes<SpritePositionData>(new SpritePositionData()
            {
                RoleID = leader.RoleID,
                MapCode = Global.Data.RoleData.MapCode,
                PosX = leader.PosX,
                PosY = leader.PosY,
                NearbyPlayers = Global.Data.OtherRoles.Keys.ToArray(),
            });
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_POSITION)));

            TCPPing.RecordSendCmd((int) (TCPGameServerCmds.CMD_SPR_POSITION));
        }

        /// <summary>
        /// Gói tin gửi từ Client về Server thông báo đối tượng bắt đầu chuyển bản đồ
        /// </summary>
        /// <param name="teleportID"></param>
        /// <param name="toMapCode"></param>
        /// <param name="toMapX"></param>
        /// <param name="toMapY"></param>
        public void SpriteMapConversion(int teleportID, int toMapCode, int toMapX, int toMapY)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            SCMapChange mapChangeData = new SCMapChange
            {
                RoleID = CurrentSession.RoleID,
                TeleportID = teleportID,
                MapCode = toMapCode,
                PosX = toMapX,
                PosY = toMapY,
            };

            byte[] bData = DataHelper.ObjectToBytes<SCMapChange>(mapChangeData);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_MAPCHANGE)));
        }

        /// <summary>
        /// Gói tin gửi từ Client về Server thông báo bản đồ tải xuống thành công
        /// </summary>
        public void SpriteEnterMap()
        {
            byte[] bData = new ASCIIEncoding().GetBytes("");
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_ENTERMAP)));
        }

        /// <summary>
        /// Cửa hàng mua vật phẩm
        /// </summary>
        /// <param name="shopItemID">ID Vật phẩm</param>
        /// <param name="goodsNum">Số lượng vật phẩm</param>
        /// <param name="shopID">ID của shop</param>
        /// <param name="couponID">ID phiếu giảm giá</param>
        public void SpriteBuyGoods(int shopItemID, int goodsNum, int shopID, int couponID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", CurrentSession.RoleID, shopItemID, goodsNum, shopID, couponID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_NPC_BUY)));
        }

        /// <summary>
        /// Bán vật phẩm
        /// </summary>
        /// <param name="goodsDbId"></param>
        public void SpriteBuyOutGoods(int goodsDbId)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, goodsDbId);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_NPC_SALEOUT)));
        }

        /// <summary>
        /// Gửi packet cộng điểm tiềm năng về Server
        /// </summary>
        /// <param name="nStrengthPoint"></param>
        /// <param name="nIntelligencePoint"></param>
        /// <param name="nDexterityPoint"></param>
        /// <param name="nConstitutionPoint"></param>
        public void SpriteRecommendPoint(int nStrengthPoint, int nIntelligencePoint, int nDexterityPoint, int nConstitutionPoint)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            CSPropAddPoint PropAddPoint = new CSPropAddPoint();
            PropAddPoint.RoleID = CurrentSession.RoleID;
            PropAddPoint.Strength = nStrengthPoint;
            PropAddPoint.Intelligence = nIntelligencePoint;
            PropAddPoint.Dexterity = nDexterityPoint;
            PropAddPoint.Constitution = nConstitutionPoint;
            byte[] bData = PropAddPoint.toBytes();
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_EXECUTERECOMMENDPROPADDPOINT)));
        }

        /// <summary>
        /// Gửi gói tin khi mặc đồ, tháo đồ, hoặc đổi vị trí vật phẩm hoặc trang bị
        /// </summary>
        /// <param name="modType"></param>
        /// <param name="id"></param>
        /// <param name="goodsID"></param>
        /// <param name="isusing"></param>
        /// <param name="site"></param>
        /// <param name="gcount"></param>
        /// <param name="bagIndex"></param>
        /// <param name="extraParams"></param>
        public void SpriteModGoods(int modType, int id, int goodsID, int isusing, int site, int gcount, int bagIndex, string extraParams = "")
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", CurrentSession.RoleID, modType, id, goodsID, isusing, site, gcount, bagIndex, extraParams);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_MOD_GOODS)));
        }

        /// <summary>
        /// Lấy thông tin bạn bè
        /// </summary>
        public void SpriteGetFriends()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETFRIENDS)));
        }
        /// <summary>
        /// Thêm bạn
        /// </summary>
        /// <param name="dbID"></param>
        /// <param name="roleID"></param>
        /// <param name="otherName"></param>
        /// <param name="friendType"></param>
        public void SpriteAddFriend(int dbID, int roleID, string otherName, int friendType)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}:{2}:{3}", dbID, roleID, otherName, friendType);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_ADDFRIEND)));
        }
        /// <summary>
        /// Xóa bạn
        /// </summary>
        /// <param name="dbID"></param>
        public void SpriteRemoveFriend(int dbID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", dbID, CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_REMOVEFRIEND)));
        }
        /// <summary>
        /// Từ chối yêu cầu thêm bạn
        /// </summary>
        /// <param name="dbID"></param>
        public void SpriteRejectFriend(int roleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", roleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_REJECTFRIEND)));
        }
        /// <summary>
        /// Gửi lời mời thêm bạn
        /// </summary>
        /// <param name="roleID"></param>
        public void SpriteAskFriend(int roleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", roleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_ASKFRIEND)));
        }

        /// <summary>
        /// Thực hiện nhặt vật phẩm rơi ở Map
        /// </summary>
        /// <param name="autoID"></param>
        /// <param name="GoodsID"></param>
        public void SpriteGetThing(int autoID, int GoodsID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}", CurrentSession.RoleID, autoID, GoodsID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETTHING)));
        }

        /// <summary>
        /// Yêu cầu thay đổi trạng thái PK
        /// </summary>
        /// <param name="pkMode"></param>
        public void SpriteUpdatePKMode(int pkMode)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, pkMode);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_CHGPKMODE)));
        }

        /// <summary>
        /// Hủy nhiệm vụ
        /// </summary>
        /// <param name="dbID"></param>
        /// <param name="taskID"></param>
        public void SpriteAbandonTask(int dbID, int taskID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}", CurrentSession.RoleID, dbID, taskID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_ABANDONTASK)));
        }

        /// <summary>
        /// Gửi tin nhắn Chat
        /// </summary>
        /// <param name="fromRoleName"></param>
        /// <param name="toRoleName"></param>
        /// <param name="content"></param>
        /// <param name="channel"></param>
        public void SpriteSendChat(string fromRoleName, string toRoleName, string content, ChatChannel channel)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            SpriteChat chat = new SpriteChat()
            {
                FromRoleName = fromRoleName,
                ToRoleName = toRoleName,
                Content = content,
                Channel = (int) channel,
                Items = null,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_SPR_CHAT)));
        }

        /// <summary>
        /// Gửi tin nhắn Chat đính kèm vật phẩm tương ứng
        /// </summary>
        /// <param name="fromRoleName"></param>
        /// <param name="toRoleName"></param>
        /// <param name="content"></param>
        /// <param name="channel"></param>
        /// <param name="item"></param>
        public void SpriteSendChat(string fromRoleName, string toRoleName, string content, ChatChannel channel, List<GoodsData> items)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            SpriteChat chat = new SpriteChat()
            {
                FromRoleName = fromRoleName,
                ToRoleName = toRoleName,
                Content = content,
                Channel = (int) channel,
                Items = items,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_SPR_CHAT)));
        }

        /// <summary>
        /// Sử dụng vật phẩm
        /// </summary>
        /// <param name="itemDbIDs"></param>
        public void SpriteUseGoods(params int[] itemDbIDs)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            CS_SprUseGoods useGoods = new CS_SprUseGoods();
            useGoods.RoleID = Global.Data.RoleData.RoleID;
            useGoods.DbIds = itemDbIDs.ToList();
            byte[] bData = DataHelper.ObjectToBytes<CS_SprUseGoods>(useGoods);

            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_USEGOODS)));
        }

        /// <summary>
        /// Gửi yêu cầu nào đó trong giao dich với đối phương
        /// </summary>
        /// <param name="otherRoleID"></param>
        /// <param name="exchangeType"></param>
        /// <param name="exchangeID"></param>
        public void SpriteGoodsExchange(int otherRoleID, int exchangeType, int exchangeID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}:{3}", CurrentSession.RoleID, otherRoleID, exchangeType, exchangeID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE)));
        }

        /// <summary>
        /// Gửi yêu cầu nào đó liên quan đến cửa hàng người chơi về GS
        /// </summary>
        /// <param name="stallType"></param>
        /// <param name="extTag1"></param>
        /// <param name="extTag2"></param>
        public void SpriteGoodsInstall(int stallType, int extTag1, string extTag2)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}:{3}", CurrentSession.RoleID, stallType, extTag1, extTag2);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GOODSSTALL)));
        }

        /// <summary>
        /// Thông báo lên Server đối tượng tải xuống hoàn tất
        /// </summary>
        /// <param name="ohterRoleID"></param>
        public void SpriteLoadAlready(int ohterRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, ohterRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_LOADALREADY)));
        }

        /// <summary>
        /// Sắp xếp lại túi đồ
        /// </summary>
        public void SpriteSortBag()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_RESETBAG)));
        }

        /// <summary>
        /// Sắp xếp lại thương khố
        /// </summary>
        public void SpriteSortPortableBag()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_RESETPORTABLEBAG)));
        }

        /// <summary>
        /// Thực thi sự kiện HEART gửi về GS
        /// </summary>
        public void SpriteHeart()
        {
            SCClientHeart ClientHeart = new SCClientHeart();
            ClientHeart.RoleID = CurrentSession.RoleID;
            ClientHeart.RandToken = CurrentSession.RoleRandToken;
            ClientHeart.ClientTicks = DateTime.Now.Ticks;
            byte[] bData = DataHelper.ObjectToBytes<SCClientHeart>(ClientHeart);

            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, bData, 0, bData.Length, (int) (TCPGameServerCmds.CMD_SPR_CLIENTHEART)));
            TCPPing.RecordSendCmd((int) (TCPGameServerCmds.CMD_SPR_CLIENTHEART));
        }

        /// <summary>
        /// Lấy danh sách thư
        /// </summary>
        public void SpriteGetUserMailList()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", this.CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETUSERMAILLIST)));
        }

        /// <summary>
        /// Lấy thông tin thư tương ứng
        /// </summary>
        /// <param name="mailID"></param>
        public void SpriteGetUserMailData(int mailID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", this.CurrentSession.RoleID, mailID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETUSERMAILDATA)));
        }

        /// <summary>
        /// Lấy vật phẩm đính kèm khỏi thư
        /// </summary>
        /// <param name="mailID"></param>
        public void SpriteFetchMailGoods(int mailID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", this.CurrentSession.RoleID, mailID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_FETCHMAILGOODS)));
        }

        /// <summary>
        /// Xóa thư
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="mailIDs"></param>
        public void SpriteDeleteUserMail(string mailIDs)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", this.CurrentSession.RoleID, mailIDs);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_DELETEUSERMAIL)));
        }

        /// <summary>
        /// Gửi yêu cầu lấy thông tin nạp thẻ phúc lợi
        /// </summary>
        /// <param name="nRoleID"></param>
        public void QueryWelfareRechargeInfo(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_QUERY_REPAYACTIVEINFO)));
        }

        /// <summary>
        /// Gửi yêu cầu lấy thông tin Online phúc lợi
        /// </summary>
        /// <param name="nRoleID"></param>
        public void QueryWelfareEverydayOnlineInfo(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_UPDATEEVERYDAYONLINEAWARDGIFTINFO)));
        }

        /// <summary>
        /// Gửi yêu cầu lấy thông tin 7 ngày đăng nhập phúc lợi
        /// </summary>
        /// <param name="nRoleID"></param>
        public void QueryWelfareSevenDayLoginInfo(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_UPDATEEVERYDAYSERIESLOGININFO)));
        }

        /// <summary>
        /// Gửi yêu cầu lấy thông tin đăng nhập tích lũy phúc lợi
        /// </summary>
        /// <param name="nRoleID"></param>
        public void QueryWelfareSeriesLoginInfo(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_QUERYTOTALLOGININFO)));
        }

        /// <summary>
        /// Gửi yêu cầu lấy thông tin thăng cấp phúc lợi
        /// </summary>
        /// <param name="nRoleID"></param>
        public void QueryWelfareLevelUpInfo(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_QUERYUPLEVELGIFTINFO)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần thưởng đăng nhập liên tiếp
        /// </summary>
        /// <param name="nRoleID"></param>
        public void SendSpriteGetSeriesLoginAward(int nRoleID, int nIndex)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", nRoleID, nIndex);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETTOTALLOGINAWARD)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần thưởng thăng cấp
        /// </summary>
        /// <param name="nRoleID"></param>
        public void SendSpriteGetLevelUpAward(int nRoleID, int nIndex)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", nRoleID, nIndex);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETUPLEVELGIFTAWARD)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần thưởng đăng nhập 7 ngày
        /// </summary>
        /// <param name="nRoleID"></param>
        public void SendSpriteGetSevenDayLoginAward(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETEVERYDAYSERIESLOGINAWARDGIFT)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần thưởng Online mỗi ngày
        /// </summary>
        /// <param name="nRoleID"></param>
        public void SendSpriteGetEverydayOnlineAward(int nRoleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            strcmd = string.Format("{0}", nRoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GETEVERYDAYONLINEAWARDGIFT)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần thưởng nạp thẻ
        /// </summary>
        /// <param name="nRoleID"></param>
        /// <param name="nActiveID"></param>
        /// <param name="nIndex"></param>
        public void SendSpriteGetRechargeAward(int nRoleID, int nActiveID, int nIndex)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}", nRoleID, nActiveID, nIndex);
            bool bRet = tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GET_REPAYACTIVEAWARD)));
        }

        /// <summary>
        /// Gửi yêu cầu mua thẻ tháng
        /// </summary>
        public void SpriteBuyMonthCard()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = "";
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_WING_ZHUHUN)));
        }

        /// <summary>
        /// Gửi yêu cầu truy vấn thông tin thẻ tháng
        /// </summary>
        public void SpriteMonthCardInfo()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", CurrentSession.RoleID);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GET_YUEKA_DATA)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận thưởng thẻ tháng
        /// </summary>
        /// <param name="getDay"></param>
        public void SpriteGetMonthCardAwardCard(int getDay)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, getDay);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GET_YUEKA_AWARD)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận phần tải lần đầu
        /// </summary>
        public void SendGetFirstDownloadResourceAward()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, 2);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_GET_BONUS_DOWNLOAD)));
        }

        /// <summary>
        /// Gửi thông tin thiết bị lên Server
        /// </summary>
        public void SendToServerDeviceType()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, KTGlobal.GetDeviceGeneration());
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_SPECIALMACHINE)));
        }

        /// <summary>
        /// Thay đổi giá trị bạc trong thương khố
        /// </summary>
        /// <param name="type">Loại thao tác: -1: Rút, 1: Thêm</param>
        /// <param name="value"></param>
        public void ModifyStoreMoney(int type, int value)
        {
            if (type == -1)
            {
                value = -value;
            }
            else if (type != 1)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}", CurrentSession.RoleID, value);
            tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_SPR_GET_STORE_MONEY)));
        }

        /// <summary>
        /// Gửi gói tin đăng xuất
        /// </summary>
        public void SpriteLogOut()
        {
            if (tcpClient.Connected)
            {
                string strcmd = "";
                strcmd = string.Format("{0}", 0);
                tcpClient.SendData(TCPOutPacket.MakeTCPOutPacket(tcpClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_LOG_OUT)));
            }
        }
#endregion
    }
}
