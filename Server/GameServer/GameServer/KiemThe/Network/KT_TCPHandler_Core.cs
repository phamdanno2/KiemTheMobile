using GameServer.Core.Executor;
using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.CopySceneEvents.XiaoYaoGu;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.GameEvents.BaiHuTang;
using GameServer.KiemThe.GameEvents.EmperorTomb;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.Logic;
using GameServer.Logic.CheatGuard;
using GameServer.Logic.LoginWaiting;
using GameServer.Logic.Name;
using GameServer.Logic.SecondPassword;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Tmsk.Contract;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý gói tin
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Special Device Name
        /// <summary>
        /// Gói tin thông báo tên thiết bị
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpecialDeviceName(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }
                /// Thiết lập DeviceGeneration
                client.DeviceGeneration = fields[1];

                //Console.WriteLine("Client = {0}, Device = {1}", client.RoleName, client.DeviceGeneration);
                LogManager.WriteLog(LogTypes.DeviceInfo, string.Format("Client = {0} (ID: {1}), DeviceModel = {2}, DeviceGeneration = {3}", client.RoleName, client.RoleID, client.DeviceModel, client.DeviceGeneration));

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
        #endregion

        #region Play Game

        /// <summary>
        /// Gói tin bắt đầu vào Game hoặc kết nối lại
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessStartPlayGameCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Thiết lập hướng mặc định
                client.CurrentDir = KiemThe.Entities.Direction.DOWN;

                /// Thực hiện di chuyển đối tượng vào Grid
                GameManager.MapGridMgr.DictGrids[client.MapCode].MoveObject(-1, -1, client.PosX, client.PosY, client);

                /// Thiết lập thời gian cập nhật tiền
                long nowTicks = TimeUtil.NOW();
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD, nowTicks);
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_ROLE_AVARTA, nowTicks);
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL, nowTicks);

                /// Clear tầm hình của nhân vật
                client.ClearVisibleObjects(false);

                /// Trả về NPC STATE của client này
                TaskManager.getInstance().ComputeNPCTaskState(client);

                /// Gửi gói tin thông báo danh sách kỹ năng về Client
                KT_TCPHandler.SendRenewSkillList(client);
                /// Khởi tạo Buff
                client.Buffs.ExportBuffTree();

                /// Đánh dấu không đợi chuyển Map
                client.WaitingForChangeMap = false;

                socket.session.SetSocketTime(4);

                GameManager.loginWaitLogic.RemoveAllow(client.strUserID);

                string strcmd = "";
                strcmd = string.Format("{0}", roleID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                /// Nếu lần đầu vào Game
                if (client.FirstEnterPlayGameCmd)
                {
                    /// Hủy đánh dấu lần đầu vào Game
                    client.FirstEnterPlayGameCmd = false;
                    /// Thực hiện gửi thông tin tải lần đầu
                    ///KT_TCPHandler.SendDownloadBonus(client);
					/// ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, 781, 1, 0, "JOINFACTION", false, 1, false, Global.ConstGoodsEndTime);
					/// ItemManager.CreateItem(pool, client, client, 1, 0, "QUEST|" , false, 1, false, Global.ConstGoodsEndTime);
                }

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Play Game

        #region Init Game

        /// <summary>
        /// Hàm Định nghĩa khi nhân vật vào game
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessInitGameCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}", (TCPGameServerCmds)nID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                /// 3 tham biến là Client cũ chưa Update, 4 tham biến là Client mới đã Update
                if (fields.Length != 3 && fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), cmdData));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string userID = fields[0];
                int roleID = Convert.ToInt32(fields[1]);
                string deviceModel = "";
                string deviceID = "";
                /// Client bản cũ (sau có thể bỏ)
                if (fields.Length == 3)
                {
                    deviceID = fields[2];
                }
                /// Client bản mới
                else
                {
                    deviceID = fields[2];
                    deviceModel = fields[3];
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null != client)
                {
                    if (client.RoleID == roleID)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Role ID đã tồn tại không cần tìm từ DB(ProcessInitGameCmd), CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                    else
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Role ID không nhất quán, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                        return TCPProcessCmdResults.RESULT_FAILED;
                    }
                }

                /// Thông tin Socket
                IPEndPoint socketInfo = socket.RemoteEndPoint as IPEndPoint;
                /// Địa chỉ IP
                string ipAddress = socketInfo.Address.ToString();
                /// Nếu đã vượt quá số lượng kết nối
                if (GameManager.OnlineUserSession.GetClientCountsByIPAddress(ipAddress) >= ServerConfig.Instance.LimitAccountPerIPAddress)
                {
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = -1 }, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (roleID < 0 || !GameManager.TestGamePerformanceMode && userID != GameManager.OnlineUserSession.FindUserID(socket))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Plugin login, no need SocketSession, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_OK;
                }
                // Nếu user đã bị band thì chim cút luôn
                if (BanManager.IsBanInMemory(userID))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                // Thời gian hàng đợi đăng nhập
                int waitSecs = Global.GetSwitchServerWaitSecs(socket);
                if (waitSecs > 0)
                {
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = StdErrorCode.Error_40, TotalValue = waitSecs }, pool, nID);
                    socket.session.SetSocketTime(5);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (socket.IsKuaFuLogin)
                {
                    //Nếu là liên sv thì trả về dữ liệu login của máy chủ liên sv
                    roleID = socket.ClientKuaFuServerLoginData.RoleId;
                }

                // Truy vấn nhân vật từ GAME DB SERVER
                byte[] bytesData = null;
                if (TCPProcessCmdResults.RESULT_FAILED == Global.TransferRequestToDBServer2(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out bytesData, socket.ServerId))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                Int32 length = BitConverter.ToInt32(bytesData, 0);
                UInt16 cmd = BitConverter.ToUInt16(bytesData, 4);

                ///Chuyển đội BYTE thành các đối tượng
                RoleDataEx roleDataEx = DataHelper.BytesToObject<RoleDataEx>(bytesData, 6, length - 2);

                if (roleDataEx.RoleID < 0)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Không lấy được nhân vật từ CSDL: Cmd={0}, RoleID={1}, đóng kết nối ", (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = roleDataEx.RoleID }, pool, cmd);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // Nếu không phải là máy chủ liên server
                if (!socket.IsKuaFuLogin)
                {
                    bool isKuaFuMap = KuaFuManager.getInstance().IsKuaFuMap(roleDataEx.MapCode);

                    if (isKuaFuMap)
                    {
                        KPlayer fakeClient = Global.MakeGameClientForGetRoleParams(roleDataEx);

                        KT_TCPHandler.GetPlayerCopySceneInfoFromDB(fakeClient, out int copySceneIDOld, out int MapcodeOld, out int XPosOld, out int YPosOld, out long copySceneCreateTicksOld);

                        roleDataEx.MapCode = MapcodeOld;

                        roleDataEx.PosX = (int)XPosOld;
                        roleDataEx.PosY = (int)YPosOld;
                    }

                    //NẾu không phải GM cần xác mình và đứng ở hàng đợi
                    if (!socket.session.IsGM)
                    {
                        // Xác định xem có cần phải đợi để đăng nhập không, Có phải xác minh mật khẩu câp s2 không
                        if (roleDataEx.LastOfflineTime < TimeUtil.CurrentTicksInexact - 5 * TimeUtil.MINITE)
                        {
                            if (!GameManager.loginWaitLogic.IsInAllowDict(userID))
                            {
                                LoginWaitLogic.UserType userType = GameManager.loginWaitLogic.GetUserType(userID);
                                // Kiểm tra xem có cần xếp hàng ko
                                if (GameManager.loginWaitLogic.GetUserCount() >= GameManager.loginWaitLogic.GetConfig(userType, LoginWaitLogic.ConfigType.NeedWaitNum))
                                {
                                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = StdErrorCode.Error_60 }, pool, nID);
                                    //LogManager.WriteLog(LogTypes.Error, string.Format("CMD_INIT_GAME user {0}进入排队列表", userID));
                                    socket.session.SetSocketTime(5);
                                    return TCPProcessCmdResults.RESULT_DATA;
                                }
                            }

                            // NẾu trạng thái cần kiểm tra mật khẩu phụ thì yêu cầu nhập mật khẩu phụ
                            SecPwdState pwdState = SecondPasswordManager.GetSecPwdState(userID);
                            if (pwdState != null && pwdState.NeedVerify)
                            {
                                // Nếu không xác minh thì không thể đăng nhập
                                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = StdErrorCode.Error_30 }, pool, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }
                        }
                        else
                        {
                            SecPwdState pwdState = SecondPasswordManager.GetSecPwdState(userID);
                            if (pwdState != null && pwdState.NeedVerify)
                            {
                                pwdState.NeedVerify = false;
                            }
                        }
                    }
                }

                //FILL RA TOÀN BỘ ĐỒ ĐẠC | CHỖ NÀY SẼ REMOVE ĐI KHI LÀM ITEM
                if (null == roleDataEx.SaleGoodsDataList)
                {
                    roleDataEx.SaleGoodsDataList = new List<GoodsData>();
                }
                if (null == roleDataEx.GoodsDataList)
                {
                    roleDataEx.GoodsDataList = new List<GoodsData>();
                }
                if (null == roleDataEx.GroupMailRecordList)
                {
                    roleDataEx.GroupMailRecordList = new List<int>();
                }

                //Xác định xem nhân vật có bị cấm đăng nhập không
                int leftSecs = 78;
                int reason = BanManager.IsBanRoleName(Global.FormatRoleName3(roleDataEx.ZoneID, roleDataEx.RoleName), out leftSecs);
                if (reason > 0)
                {
                    int _phony_rid = reason == (int)BanManager.BanReason.UseSpeedSoftware ? StdErrorCode.Error_20 :
                        reason == (int)BanManager.BanReason.RobotTask ? StdErrorCode.Error_50 :
                        StdErrorCode.Error_80;
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = _phony_rid, TotalValue = leftSecs }, pool, cmd);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (!socket.IsKuaFuLogin)
                {
                    //Nếu bản đồ không tồn tại
                    if (!Global.MapExists(roleDataEx.MapCode))
                    {
                        LogManager.WriteLog(LogTypes.Warning, string.Format("Bản đồ không tồn tại ==> chuyển về bản đồ mặc định: MapCode={0}", roleDataEx.MapCode));

                        KPlayer fakeClient = Global.MakeGameClientForGetRoleParams(roleDataEx);

                        KT_TCPHandler.GetPlayerDefaultRelivePos(fakeClient, out int MapCodeOut, out int PosXOut, out int PosYOut);

                        roleDataEx.MapCode = MapCodeOut;

                        // FILL TỌA ĐỘ HỒI SINH VÀO DÂY
                        roleDataEx.PosX = PosXOut;
                        roleDataEx.PosY = PosYOut;
                    }
                }

                /// Nếu đã vượt quá số CCU và không phải GM
                if (GameManager.ClientMgr.GetClientCount() >= ServerConfig.Instance.MaxCCU && !KTGMCommandManager.IsGM(socket, roleDataEx.RoleID))
                {
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = -2 }, pool, cmd);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// KHỞI TẠO GAMECLIENT
                KPlayer gameClient = new KPlayer()
                {
                    RoleData = roleDataEx,
                    ReportPosTicks = 0,
                    WaitingForChangeMap = true,
                    ClientSocket = socket,
                    TeamID = -1,
                    m_eDoing = KE_NPC_DOING.do_stand,
                    DeviceModel = deviceModel,
                };
                /// Kiểm tra Avarta nếu không hợp lệ thì chọn Avarta mặc định
                if (!KRoleAvarta.IsAvartaValid(gameClient, gameClient.RolePic))
                {
                    /// Chọn 1 Avarta đầu tiên trong danh sách có giới tính phù hợp
                    RoleAvartaXML roleAvarta = KRoleAvarta.GetMyDefaultAvarta(gameClient);
                    /// Nếu tồn tại
                    if (roleAvarta != null)
                    {
                        gameClient.RolePic = roleAvarta.ID;
                    }
                }

                /// Chuyển PKMode về hòa bình
                gameClient.PKMode = (int) PKMode.Peace;

                /// Thêm kỹ năng tân thủ cho người chơi
                KTLogic.AddNewbieAttackSkills(gameClient);

                // Khởi tạo lại danh sách TMP lưu lại vật phẩm của thằng người chơi đã bán trong phiên login đó

                gameClient.ClearOwnShop();
                /// Gọi tới khi hoàn tất quá trình tải xuống RoleDataEx
                gameClient.OnEnterGame();

                // SET LẠI DỮ LIỆU TÀI KHOẢN | THẺ THÁNG
                gameClient.strUserID = GameManager.OnlineUserSession.FindUserID(gameClient.ClientSocket);
                gameClient.strUserName = GameManager.OnlineUserSession.FindUserName(socket);
                gameClient.deviceID = deviceID;

                // Giao nhiệm vụ cho người chơi mới [XSea 2015/4/14]
                GameManager.ClientMgr.GiveFirstTask(tcpMgr, socket, tcpClientPool, pool, tcpRandKey, gameClient, true);

                /// Bản đồ tương ứng
                GameMap gameMap = GameManager.MapMgr.GetGameMap(roleDataEx.MapCode);

                /// Thông tin phụ bản lần tước
                KT_TCPHandler.GetPlayerCopySceneInfoFromDB(gameClient, out int copySceneID, out int preMapCode, out int prePosX, out int prePosY, out long copySceneCreateTicks);
                /// Nếu là liên Server
                if (socket.IsKuaFuLogin)
                {
                    if (!KuaFuManager.getInstance().OnInitGame(gameClient))
                    {
                        tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(new RoleData() { RoleID = StdErrorCode.Error_Operation_Denied }, pool, cmd);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                /// Nếu có phụ bản
                else if (copySceneID != -1)
                {
                    /// Nếu phụ bản vẫn đang tồn tại
                    if (CopySceneEventManager.IsCopySceneExist(copySceneID, gameClient.CurrentMapCode, copySceneCreateTicks))
                    {
                        /// Thông tin phụ bản tương ứng
                        KTCopyScene copyScene = CopySceneEventManager.GetCopyScene(copySceneID, gameClient.CurrentMapCode);
                        /// Nếu phụ bản không chấp nhận cho kết nối lại
                        if (!copyScene.AllowReconnect)
                        {
                            /// Xóa thông tin phụ bản tương ứng
                            gameClient.CurrentCopyMapID = -1;
                            ///// Thiết lập vị trí của người chơi là vị trí cũ
                            //gameClient.MapCode = preMapCode;
                            //gameClient.PosX = prePosX;
                            //gameClient.PosY = prePosY;
                            /// Thông tin điểm về thành gần nhất
                            KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                            /// Đưa người chơi trở về bản đồ mặc định
                            gameClient.MapCode = mapCode;
                            gameClient.PosX = posX;
                            gameClient.PosY = posY;
                        }
                        else
                        {
                            /// Đưa người chơi trở lại phụ bản
                            gameClient.CurrentCopyMapID = copySceneID;
                            /// Thực hiện sự kiện Reconnect
                            CopySceneEventManager.OnPlayerReconnected(copyScene, gameClient);
                        }
                    }
                    /// Nếu phụ bản đã hết thời gian
                    else
                    {
                        /// Xóa thông tin phụ bản tương ứng
                        gameClient.CurrentCopyMapID = -1;
                        ///// Thiết lập vị trí của người chơi là vị trí cũ
                        //gameClient.MapCode = preMapCode;
                        //gameClient.PosX = prePosX;
                        //gameClient.PosY = prePosY;
                        /// Thông tin điểm về thành gần nhất
                        KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                        /// Đưa người chơi trở về bản đồ mặc định
                        gameClient.MapCode = mapCode;
                        gameClient.PosX = posX;
                        gameClient.PosY = posY;
                        /// Xóa thông tin phụ bản cũ
                        KT_TCPHandler.RecordCopySceneInfoToDB(gameClient, -1, -1, -1, -1, -1);
                    }
                }
                /// Nếu không tồn tại bản đồ tương ứng hoặc đây là bản đồ phụ bản
                else if (gameMap == null || gameMap.IsCopyScene)
                {
                    /// Thông tin điểm về thành gần nhất
                    KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                    /// Đưa người chơi trở về bản đồ mặc định
                    gameClient.MapCode = mapCode;
                    gameClient.PosX = posX;
                    gameClient.PosY = posY;
                    /// Xóa thông tin phụ bản cũ
                    KT_TCPHandler.RecordCopySceneInfoToDB(gameClient, -1, -1, -1, -1, -1);
                }

                /// Nếu ở Tống Kim
                if (gameClient.IsInSongJin() || gameClient.IsInFacetionBattleJin())
                {
                    /// Thông tin điểm về thành gần nhất
                    KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                    /// Đưa người chơi trở về bản đồ mặc định
                    gameClient.MapCode = mapCode;
                    gameClient.PosX = posX;
                    gameClient.PosY = posY;
                }
                /// Nếu ở bản đồ chờ Tiêu Dao Cốc
                else if (gameClient.CurrentMapCode == XoYo.WaitingMap.ID)
                {
                    /// Đẩy về bản đồ báo danh Tiêu Dao Cốc
                    gameClient.MapCode = XoYo.RegisterMap.ID;
                    gameClient.PosX = XoYo.RegisterMap.EnterPosX;
                    gameClient.PosY = XoYo.RegisterMap.EnterPosY;
                    /// Xóa thông tin phụ bản cũ
                    KT_TCPHandler.RecordCopySceneInfoToDB(gameClient, -1, -1, -1, -1, -1);
                }
                /// Nếu đang ở Bạch Hổ Đường
                else if (BaiHuTang.IsInBaiHuTang(gameClient))
                {
                    /// Thông tin điểm về thành gần nhất
                    KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                    /// Đưa người chơi trở về bản đồ mặc định
                    gameClient.MapCode = mapCode;
                    gameClient.PosX = posX;
                    gameClient.PosY = posY;
                }
                /// Nếu đang ở Hội trường liên đấu
                else if (TeamBattle.IsInTeamBattleMap(gameClient))
                {
                    /// Thông tin điểm về thành gần nhất
                    KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                    /// Đưa người chơi trở về bản đồ mặc định
                    gameClient.MapCode = mapCode;
                    gameClient.PosX = posX;
                    gameClient.PosY = posY;
                }
                /// Nếu đang ở bản đồ Tần Lăng
                else if (EmperorTomb.IsInsideEmperorTombMap(gameClient))
                {
                    /// Thông tin điểm về thành gần nhất
                    KT_TCPHandler.GetPlayerDefaultRelivePos(gameClient, out int mapCode, out int posX, out int posY);
                    /// Đưa người chơi trở về bản đồ mặc định
                    gameClient.MapCode = mapCode;
                    gameClient.PosX = posX;
                    gameClient.PosY = posY;
                }

                /// Nếu không phải ở bản đồ bí cảnh
                if (gameClient.CurrentMapCode != MiJing.Map.ID)
                {
                    /// Xóa Buff x2 kinh nghiệm bí cảnh
                    gameClient.Buffs.RemoveBuff(MiJing.DoubleExpBuff);
                }

                /// INIT TOÀN BỘ THUỘC TÍNH PHỤ CỦA NGƯỜI CHƠI
                KTLogic.InitRoleLoginPrams(gameClient);

                /// Lấy thông tin Bách Bảo Rương
                gameClient.ReadSeashellCircleParamFromDB();

                /// Lấy thông tin Chúc phúc
                gameClient.ReadPrayDataFromDB();

                /// Lấy thông tin Tu Luyện
                int xiuLianZhu_Exp = Global.GetRoleParamsInt32FromDB(gameClient, RoleParamName.XiuLianZhu);
                /// BUG
                if (xiuLianZhu_Exp > 500000)
                {
                    xiuLianZhu_Exp = 500000;
                }
                gameClient.XiuLianZhu_Exp = xiuLianZhu_Exp;
                gameClient.XiuLianZhu_TotalTime = Global.GetRoleParamsInt32FromDB(gameClient, RoleParamName.XiuLianZhu_TotalTime);

                // set -1 BEFORE
                gameClient.CurenQuestIDBVD = -1;

                //Fix nhiệm vụ chính tuyến cho bọn bị lỗi
                TaskManager.getInstance().FixMainTaskID(gameClient);

                // Give BVD TASK
                TaskDailyArmyManager.getInstance().GiveTaskArmyDaily(gameClient);

                PirateTaskManager.getInstance().GiveTaskPirate(gameClient);
                // Kiểm tra xem đã hết hạn thẻ tháng chưa
                CardMonthManager.CheckValid(gameClient);

                // Khởi tạo việc lưu trữ lại thành tích | Đăng nhập | Giết quái vvv
                ChengJiuManager.InitRoleChengJiuData(gameClient);

                // Thực hiện attak toàn bộ đồ đang sử dụng vào Dict
                gameClient.GetPlayEquipBody().InitEquipBody();

                // Ghi lại tích lũy đăng nhập
                ChengJiuManager.OnRoleLogin(gameClient, Global.SafeConvertToInt32(gameClient.MyHuodongData.LastDayID));

                //Update thời gian online trong ngày
                HuodongCachingMgr.ProcessDayOnlineSecs(gameClient, Global.SafeConvertToInt32(gameClient.MyHuodongData.LastDayID));

                // Update tuần nếu sang tuần mới
                Global.UpdateWeekLoginNum(gameClient);

                // Lưu lại số lần login
                Global.UpdateRoleLoginRecord(gameClient);

                //Cập nhật số lần đăng nhập tích lũy trong thời gian giới hạn
                Global.UpdateLimitTimeLoginNum(gameClient);

                //Update lại tích sự kiện đăng nhập
                GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, gameClient);

                // Ghi lại thời gian để set hoạt động quà tặng cho các người chơi mới
                Global.InitNewStep(gameClient);

                // Set lại active userFlag
                gameClient.DailyActiveDayLginSetFlag = false;

                // Kiểm tra xem có phỉa thằng này login lần đầu tiên hay không
                bool isFirstLogin = false;

                int dayID = TimeUtil.NowDateTime().DayOfYear;
                if (dayID != Global.SafeConvertToInt32(gameClient.MyHuodongData.LastDayID))
                {
                    isFirstLogin = true;
                }

                //Lọc tất cả các task không hợp lệ
                Global.RemoveAllInvalidTasks(gameClient);

                //Xử lý các task còn đang dang dở hoặc từ bỏ
                Global.ProcessTaskData(gameClient);

                //Kiểm tra vật phẩm hết hạn hoặc không hợp lệ
                Global.CheckGoodsDataValid(gameClient);

                //Thực hiện add người chơi vào bộ quản lý
                if (!GameManager.ClientMgr.AddClient(gameClient))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not add role to Manager, name: {0}", Global.FormatRoleName(gameClient, gameClient.RoleName)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                // Khởi tạo tên người chơi nếu đang ở máy chủ liên server
                string roleName;
                if (socket.IsKuaFuLogin)
                {
                    roleName = Global.FormatRoleNameWithZoneId(gameClient);

                    gameClient.GetRoleData().RoleName = roleName;
                }
                else
                {
                    roleName = Global.FormatRoleName(gameClient, gameClient.RoleName);
                }

                RoleName2IDs.AddRoleName(roleName, gameClient.RoleID);

                //Ghi lại nhật ký login
                Global.AddRoleLoginEvent(gameClient);

                //Nếu số làn login < 0 thì ghi lại số lần offline của người chơi
                if (gameClient.LoginNum <= 0)
                {
                    double currSec = Global.GetOffsetSecond(TimeUtil.NowDateTime());
                    GameDb.UpdateRoleParamByName(gameClient, RoleParamName.CallPetFreeTime, currSec.ToString(), true);
                }

                /// Lấy thông tin ngựa tương ứng ở trang bị
                GoodsData horseGD = gameClient.GoodsDataList?.Where(x => x.Using == (int)KE_EQUIP_POSITION.emEQUIPPOS_HORSE).FirstOrDefault();
                /// Nếu không có ngựa
                if (horseGD == null)
                {
                    /// Hủy trạng thái cưỡi
                    gameClient.IsRiding = false;
                }

                /// Thêm trạng thái bảo hộ
                gameClient.SendChangeMapProtectionBuff();

                /// Nếu máu, mana, thể lực = 0 hết thì BUG gì đó
                if (gameClient.m_CurrentLife <= 0)
                {
                    gameClient.m_CurrentLife = gameClient.m_CurrentLifeMax;
                    gameClient.m_CurrentMana = gameClient.m_CurrentManaMax;
                    gameClient.m_CurrentStamina = gameClient.m_CurrentStaminaMax;
                }
                gameClient.m_eDoing = KE_NPC_DOING.do_stand;

                //FILL DỮ LIỆU CHO ROLEDATA
                RoleData roleData = KTLogic.GetMyselfRoleData(gameClient);

                ///// Ghi LOG lại tài phú
                //LogManager.WriteLog(LogTypes.Analysis, string.Format("{0} (ID: {1}), TotalValue = {2}", gameClient.RoleName, gameClient.RoleID, gameClient.GetTotalValue()));

                roleData.RoleName = roleName;
                ////Thu dọn lại ba lô cho người chơi
                //if (GameManager.Flag_OptimizationBagReset)
                //{
                //    Global.ResetBagAllGoods(gameClient, false);
                //}

                //Kích hoạt sự kiện đăng nhập t
                GlobalEventSource.getInstance().fireEvent(new PlayerInitGameEventObject(gameClient, isFirstLogin));

                socket.session.SetSocketTime(3);

                try
                {
                    string ip = RobotTaskValidator.getInstance().GetIp(gameClient);

                    string analysisLog = string.Format("login server={0} account={1} player={2} level={3} map={4} exp={5} dev_id={6} platform={7} viplevel={8} port={9} regtick={10}",
                        GameManager.ServerId, gameClient.strUserID, gameClient.RoleID, gameClient.m_Level,
                        gameClient.MapCode, ip,
                        string.IsNullOrEmpty(gameClient.deviceID) ? "" : gameClient.deviceID, GameCoreInterface.getinstance().GetPlatformType().ToString(), 0,
                        ((IPEndPoint)socket.RemoteEndPoint).Port.ToString(), gameClient.RegTime * 10000);
                    LogManager.WriteLog(LogTypes.Analysis, analysisLog);
                }
                catch { }

                /// Thêm vào danh sách IP
                GameManager.OnlineUserSession.AddClientToIPAddressList(gameClient);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(roleData, pool, cmd);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Init Game

        #region Get Role List

        /// <summary>
        /// Trả về danh sách nhân vật trong tài khoản tương ứng
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessGetRoleListCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length < 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int zoneID = Convert.ToInt32(fields[1]);

                if (!GameManager.TestGamePerformanceMode && userID != GameManager.OnlineUserSession.FindUserID(socket))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("No SocketSession, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (!socket.session.IsGM && !GameManager.loginWaitLogic.IsInAllowDict(userID))
                {
                    //int KTCOIN = GameManager.ClientMgr.QueryTotaoChongZhiMoney(userID, -1, -1);

                    //if (KTCOIN < 50000)
                    //{
                    //    LoginWaitLogic.UserType userType = GameManager.loginWaitLogic.GetUserType(userID);

                    //    if (GameManager.loginWaitLogic.GetUserCount() >= GameManager.loginWaitLogic.GetConfig(userType, LoginWaitLogic.ConfigType.NeedWaitNum))
                    //    {
                    //        if (!GameManager.loginWaitLogic.AddToWait(userID, zoneID, userType, socket))
                    //        {
                    //            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-2:", nID);
                    //        }
                    //        else
                    //        {
                    //            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-1:", nID);
                    //            socket.session.SetSocketTime(5);
                    //        }

                    //        return TCPProcessCmdResults.RESULT_DATA;
                    //    }
                    //}
                }

                //if (!socket.IsKuaFuLogin)
                //{
                //    ChangeNameInfo info = NameManager.Instance().GetChangeNameInfo(userID, zoneID, socket.ServerId);
                //    if (info != null)
                //    {
                //        tcpMgr.MySocketListener.SendData(socket, DataHelper.ObjectToTCPOutPacket(info, pool, (int)TCPGameServerCmds.CMD_NTF_EACH_ROLE_ALLOW_CHANGE_NAME));
                //    }
                //}

                socket.session.SetSocketTime(0);
                socket.session.SetSocketTime(2);
                return Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, socket.ServerId);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Get Role List

        #region Get Newbie villages

        /// <summary>
        /// Lấy danh sách Tân Thủ Thôn
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults PrecessGetNewbieVillages(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// UserID
                string userID = fields[0];
                /// UserName
                string userName = fields[1];

                if (!GameManager.TestGamePerformanceMode && userID != GameManager.OnlineUserSession.FindUserID(socket))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Plugin login, no need SocketSession, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_OK;
                }

                List<int> villages = new List<int>();
                foreach (NewbieVillage village in KTGlobal.NewbieVillages)
                {
                    villages.Add(village.ID);
                }
                string strcmd = string.Join(":", villages);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
                //throw ex;
                //});
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Get Newbie villages

        #region Create Role

        /// <summary>
        /// Xử lý packet tạo nhân vật
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessCreateRoleCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                string cmdData = new UTF8Encoding().GetString(data, 0, count);
                string[] fields = cmdData.Split(':');
                if (fields.Length != 7)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// UserID
                string userID = fields[0];
                /// UserName
                string userName = fields[1];
                /// Giới tính
                int sex = Convert.ToInt32(fields[2]);
                /// ID môn phái
                int factionID = Convert.ToInt32(fields[3]);
                /// Tên và loại thiết bị
                string[] nameAndPlatformID = fields[4].Split('$');
                /// ID máy chủ
                int serverID = Convert.ToInt32(fields[5]);
                /// ID tân thủ thôn
                int villageID = Convert.ToInt32(fields[6]);

                NewbieVillage village = KTGlobal.NewbieVillages.Where(x => x.ID == villageID).FirstOrDefault();
                if (village == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Newbie village is not Exist VillageID={3} CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData, villageID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string positionInfo = string.Format("{0},{1},{2},{3}", villageID, 1, village.Position.X, village.Position.Y);
                cmdData += ":" + positionInfo;
                data = new UTF8Encoding().GetBytes(cmdData);

                /// ID thiết bị
                string deviceID = socket.DeviceID;

                if (!GameManager.TestGamePerformanceMode && userID != GameManager.OnlineUserSession.FindUserID(socket))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Plugin login, no need SocketSession, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_OK;
                }

                string strcmd = "";
                if (socket.IsKuaFuLogin || sex < 0 || sex > 1 || factionID != 0)
                {
                    strcmd = string.Format("{0}:{1}", StdErrorCode.Error_Operation_Denied, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string name = nameAndPlatformID[0];
                if (!Utils.CheckValidString(name))
                {
                    strcmd = string.Format("{0}:{1}", -1, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Kiểm tra độ dài tên hợp lệ không
                if (!NameManager.Instance().IsNameLengthOK(name))
                {
                    strcmd = string.Format("{0}:{1}", -1, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int NotifyLeftTime = 0;
                if (!CreateRoleLimitManager.Instance().IfCanCreateRole(userID, userName, deviceID, ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(), out NotifyLeftTime))
                {
                    strcmd = string.Format("{0}:{1}", -7, NotifyLeftTime);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, socket.ServerId);
                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        string[] ResultField = strCmdResult.Split(':');
                        if (ResultField.Length == 2 && Global.SafeConvertToInt32(ResultField[0]) == 1)
                        {
                            CreateRoleLimitManager.Instance().ModifyCreateRoleNum(userID, userName, deviceID, ((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Create Role

        #region Login

        /// <summary>
        /// Mã SHA
        /// </summary>
        public static string KeySHA1 { get; set; } = "abcde";

        /// <summary>
        /// Mã khóa
        /// </summary>
        public static string KeyData { get; set; } = "12345";

        /// <summary>
        /// Mã ở Web
        /// </summary>
        public static string WebKey { get; set; } = "12345";

        /// <summary>
        /// Mã Web local
        /// </summary>
        public static string WebKeyLocal { get; set; } = "12345";

        /// <summary>
        /// Thời gian hết hạn phiên đăng nhập
        /// </summary>
        public static long MaxTicks { get; set; } = (60L * 60L * 24 * 1000L * 10000L);

        /// <summary>
        /// Gói tin đăng nhập hệ thống
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessUserLogin2Cmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception) //解析错误
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), cmdData));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int verSign = 0;
                string userID = fields[1];
                string userName = fields[2];
                string lastTime = fields[3];
                string isadult = fields[4];
                string signCode = fields[5].ToLower();

                if (!int.TryParse(fields[0], out verSign))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("ProcessUserLogin2Cmd, verSign={0} userID={1}", fields[0], userID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string key = WebKey;
                key = "9377(*)#mst9";
                string strVal = userID + userName + lastTime + isadult + key;
                string strMD5 = MD5Helper.get_md5_string(strVal).ToLower();
                if (strMD5 != signCode)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Sign code check failed, CMD={0}, Client={1}, UserID={2}, IsAdult={3}, LastTime={4}, SignCode={5}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), userID, isadult, lastTime, signCode));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                //验证失败可能：Token被GM封了、没有跨服登录权限、服务器不允许非跨服登录
                if (!KuaFuManager.getInstance().OnUserLogin2(socket, verSign, userID, userName, lastTime, isadult, signCode))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}:{1}:{2}:{3}", StdErrorCode.Error_Connection_Disabled, "", "", ""), (int)TCPGameServerCmds.CMD_LOGIN_ON2);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";
                string clientIPPort = Global.GetSocketRemoteEndPoint(socket);
                if (clientIPPort.IndexOf("127.0.0.1") < 0 && GameManager.GM_NoCheckTokenTimeRemainMS <= 0) //如果不是本地连接就要严格检查时间了
                {
                    //验证用户登陆的时间是否超时
                    int oldLastTime = Convert.ToInt32(lastTime);
                    int nowSecs = DataHelper.UnixSecondsNow();

                    if (nowSecs - oldLastTime >= (60 * 60 * 24))
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}", StdErrorCode.Error_Token_Expired, "", "", "");
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON2);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                if (verSign != (int)TCPCmdProtocolVer.VerSign)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", StdErrorCode.Error_Version_Not_Match, "", "", "");
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON2);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                UserLoginToken ult = new UserLoginToken()
                {
                    UserID = userID,
                    RandomPwd = tcpRandKey.GetKey()
                };

                string userToken = ult.GetEncryptString(KeySHA1, KeyData);
                strcmd = string.Format("{0}:{1}:{2}:{3}", userID, userName, userToken, isadult);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON2);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Thực hiện Cho USER LOGIN
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessUserLoginCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6 && fields.Length != 12 && fields.Length != 13)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), cmdData));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string userID = fields[0];
                string userName = fields[1];
                string userToken = fields[2];

                LogManager.WriteLog(LogTypes.Analysis, "Login Token: " + userToken);

                int roleRandToken = Convert.ToInt32(fields[3]);
                int verSign = Convert.ToInt32(fields[4]);
                int userIsAdult = Convert.ToInt32(fields[5]);

                string strcmd = "";

                /// Đánh dấu có phải GM không
                socket.session.IsGM = KTGMCommandManager.IsGM(socket, Convert.ToInt32(fields[7]));

                socket.session.InUseridWhiteList = false;

                if (!socket.session.IsGM && !Global.CheckAnyForMultipleCondition(socket.session.InIpWhiteList, socket.session.InUseridWhiteList))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if ((tcpMgr.MySocketListener.ConnectedSocketsCount - 1) >= (tcpMgr.MaxConnectedClientLimit + (tcpMgr.MaxConnectedClientLimit / 20)))
                {
                    if (!socket.session.IsGM)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Already max CCU, can not login, CMD={0}, Client={1}, UserID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), userID));

                        strcmd = string.Format("{0}", StdErrorCode.Error_Server_Connections_Limit);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                if (verSign != (int)TCPCmdProtocolVer.VerSign)
                {
                    strcmd = string.Format("{0}", StdErrorCode.Error_Version_Not_Match2);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool verified = true;
                int RandPwd = -1;
                UserLoginToken ult = new UserLoginToken();
                int verifyResult = ult.SetEncryptString(userToken, KeySHA1, KeyData, MaxTicks);
                if (verifyResult < 0)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Login Token is not correct, CMD={0}, Client={1}, VerifyResult={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), verifyResult));
                    verified = false;
                }
                else
                {
                    userID = ult.UserID;
                    RandPwd = ult.RandomPwd;
                    if (!tcpRandKey.FindKey(RandPwd))
                    {
                        verified = false;
                    }
                }

                if (fields.Length >= 12)
                {
                    int roleId = 0;
                    long gameId = 0;
                    int gameType = 0;
                    int serverId = 0;
                    string ip = "";
                    int port = 0;

                    if (fields.Length == 13)
                    {
                        socket.DeviceID = fields[6];
                        roleId = Convert.ToInt32(fields[7]);
                        gameId = Convert.ToInt64(fields[8]);
                        gameType = Convert.ToInt32(fields[9]);
                        serverId = Convert.ToInt32(fields[10]);
                        ip = fields[11];
                        port = Convert.ToInt32(fields[12]);
                    }
                    else
                    {
                        roleId = Convert.ToInt32(fields[6]);
                        gameId = Convert.ToInt64(fields[7]);
                        gameType = Convert.ToInt32(fields[8]);
                        serverId = Convert.ToInt32(fields[9]);
                        ip = fields[10];
                        port = Convert.ToInt32(fields[11]);
                    }

                    string lastTime = DataHelper.UnixSecondsNow().ToString();
                    string strVal = userID + userName + lastTime + userIsAdult + WebKey;
                    string signCode = MD5Helper.get_md5_string(strVal).ToLower();

                    bool result = KuaFuManager.getInstance().OnUserLogin(socket, verSign, userID, userName, lastTime, userToken, userIsAdult.ToString(), signCode, serverId, ip, port, roleId, gameType, gameId);
                    if (!result)
                    {
                        string strResult = string.Format("{0}:{1}:{2}:{3}", StdErrorCode.Error_Redirect_Orignal_Server, "", "", "");
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strResult, (int)TCPGameServerCmds.CMD_LOGIN_ON);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                if (socket.ServerId == 0)
                {
                    socket.ServerId = GameManager.ServerId;
                }

                bool alreadyOnline = false;

                if (!verified)
                {
                    strcmd = string.Format("{0}", StdErrorCode.Error_Token_Expired);
                }
                else
                {
                    if (!GameManager.OnlineUserSession.AddSession(socket, userID))
                    {
                        strcmd = string.Format("{0}", StdErrorCode.Error_Connection_Closing2);

                        LogManager.WriteLog(LogTypes.Error, string.Format("Already login, Client={0}, UserName={1}", Global.GetSocketRemoteEndPoint(socket), userName));

                        alreadyOnline = true;
                    }
                    else
                    {
                        int regUserID = GameDb.RegisterUserIDToDBServer(userID, 1, socket.ServerId, ref socket.session.LastLogoutServerTicks);
                        if (regUserID <= 0)
                        {
                            strcmd = string.Format("{0}", StdErrorCode.Error_Connection_Closing2);

                            LogManager.WriteLog(LogTypes.Error, string.Format("Tài khoản đã có người khác đăng ký，Client={0}, UserName={1}", Global.GetSocketRemoteEndPoint(socket), userName));

                            alreadyOnline = true;
                        }
                        else
                        {
                            /// Thêm phiên đăng nhập tương ứng
                            GameManager.OnlineUserSession.AddUserName(socket, userName);

                            int waitSecs = Global.GetSwitchServerWaitSecs(socket);
                            strcmd = string.Format("{0}:{1}", tcpRandKey.GetKey(), waitSecs);
                        }
                    }
                }

                bool bHasOtherSocket = false;
                /// Nếu đang đăng nhập nơi khác
                if (alreadyOnline)
                {
                    TMSKSocket clientSocket = GameManager.OnlineUserSession.FindSocketByUserName(userName);
                    if (null != clientSocket)
                    {
                        bHasOtherSocket = true;
                        if (clientSocket == socket)
                        {
                            LogManager.WriteLog(LogTypes.Error, string.Format("Người dùng đã đăng nhập sẽ gửi lại hướng dẫn đăng nhập để đóng kết nối Client ={0}, UserName={1}", Global.GetSocketRemoteEndPoint(socket), userName));
                            return TCPProcessCmdResults.RESULT_FAILED;
                        }

                        KPlayer otherClient = GameManager.ClientMgr.FindClient(clientSocket);
                        if (null == otherClient)
                        {
                            Global.ForceCloseSocket(clientSocket, "Other client NULL");
                        }
                        else
                        {
                            Global.ForceCloseClient(otherClient, "Rejected");
                        }
                    }
                    else
                    {
                        string gmCmdData = string.Format("-kicku {0} {1} {2}", userName, GameManager.ServerLineID, TimeUtil.NowRealTime());

                        GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_SPR_CHAT,
                            string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", 0, "", 0, "", 0, gmCmdData, 0, 0, GameManager.ServerLineIdAllLineExcludeSelf),
                            null, socket.ServerId);
                    }

                    LogManager.WriteLog(LogTypes.Error, string.Format("Account is already online, disconnect the last one..., Client={0}, UserName={1}", Global.GetSocketRemoteEndPoint(socket), userName));
                }

                socket.session.SetSocketTime(1);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_LOGIN_ON);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Login

        #region Client heart

        /// <summary>
        /// Xử lý phản hồi PING từ Client
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteClientHeartCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            SCClientHeart cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SCClientHeart>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                int roleID = cmdData.RoleID;
                int roleRandToken = cmdData.RandToken;

                /// Người chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Client not found, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Kiểm tra Random Token có hợp lệ không
                if (!tcpRandKey.FindKey(roleRandToken))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Random token check faild, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                long nowTicks = KTGlobal.GetCurrentTimeMilis();
                client.LastClientHeartTicks = nowTicks;
                client.ClientHeartCount++;

                SpeedUpTickCheck.Instance().OnClientHeart(client, cmdData.ClientTicks);
                SCClientHeart scData = new SCClientHeart()
                {
                    RoleID = roleID,
                    RandToken = roleRandToken,
                    Ticks = KTGlobal.GetCurrentTimeMilis(),
                };
                client.SendPacket((int)TCPGameServerCmds.CMD_SPR_CLIENTHEART, scData);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion Client heart

        #region Second password

        /// <summary>
        /// Xử lý yêu cầu từ Client nhập mật khẩu cấp 2
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ResponseInputSecondPassword(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData;

            try
            {
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), cmdData.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// TODO
                PlayerManager.ShowNotification(client, "Pass 2: " + fields[0]);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Gửi yêu cầu mở khung nhập mật khẩu cấp 2 về Client
        /// </summary>
        /// <param name="player"></param>
        public static void SendOpenInputSecondPassword(KPlayer player)
        {
            try
            {
                player.SendPacket((int)TCPGameServerCmds.CMD_KT_INPUT_SECONDPASSWORD, "");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        #endregion Second password

        #region Syns time
        /// <summary>
        /// Cập nhật thời gian ở Client
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessTimeSyncGameCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                long clientTicks = Convert.ToInt64(fields[1]);
                long serverTicks = TimeUtil.NOW() * 10000;

                string strcmd = "";
                strcmd = string.Format("{0}:{1}:{2}", roleID, clientTicks, serverTicks);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
        #endregion

        #region Check
        /// <summary>
        /// Kiểm tra Ping các thứ
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessCheck(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int processSubTicks = Convert.ToInt32(fields[1]);
                int dateTimeSubTicks = Convert.ToInt32(fields[2]);

                string strcmd = "";

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}",
                                                                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                client.LastClientHeartTicks = TimeUtil.NOW();

                strcmd = string.Format("{0}", 1);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
        #endregion
    }
}