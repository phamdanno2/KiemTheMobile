using GameServer.Core.Executor;
using GameServer.Interface;
using GameServer.KiemThe;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý người chơi
    /// </summary>
    public class ClientManager
    {
        #region Số Client tối đa

        private const int MAX_CLIENT_COUNT = 2000;

        public int GetMaxClientCount()
        {
            return MAX_CLIENT_COUNT;
        }

        /// <summary>
        /// Danh sách người chơi theo thứ tự
        /// </summary>
        private KPlayer[] _ArrayClients = new KPlayer[MAX_CLIENT_COUNT];
        private static KPlayer[] _ArrayClientsNew = new KPlayer[MAX_CLIENT_COUNT];

        /// <summary>
        /// Danh sách người chơi theo NID
        /// </summary>
        private ConcurrentDictionary<int, int> _DictClientNids = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// Danh sách người chơi Free
        /// </summary>
        private List<int> _FreeClientList = new List<int>(MAX_CLIENT_COUNT);

        /// <summary>
        /// Quản lý người
        /// </summary>
        private SpriteContainer Container = new SpriteContainer();

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="mapItems"></param>
        public void Initialize(IEnumerable<XElement> mapItems)
        {
            Container.Initialize(mapItems);

            for (int i = 0; i < MAX_CLIENT_COUNT; i++)
            {
                _ArrayClients[i] = null;
                _FreeClientList.Add(i);
            }
        }

        #region GetDatabaseServerChat

        /// <summary>
        /// Thời gian xử lý chat từ GameDatabase truyền về
        /// </summary>
        private long LastTransferTicks = 0;

        /// <summary>
        /// Nhận toàn bộ chat từ Dabase server về
        /// </summary>
        public void HandleTransferChatMsg()
        {
            long ticks = TimeUtil.NOW();
            if (ticks - LastTransferTicks < (5 * 1000))
            {
                return;
            }

            LastTransferTicks = ticks;

            string strcmd = "";

            TCPOutPacket tcpOutPacket = null;

            strcmd = string.Format("{0}:{1}:{2}:{3}", GameManager.ServerLineID, GameManager.ClientMgr.GetClientCount(), Global.SendServerHeartCount, "");

            Global.SendServerHeartCount++;

            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer2(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_DB_GET_CHATMSGLIST, strcmd, out tcpOutPacket, GameManager.LocalServerId);

            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Không kết nối được với DBServer để lấy danh sách thư khi xử lý thư được chuyển tiếp"));
                return;
            }

            if (null == tcpOutPacket)
            {
                return;
            }

            List<string> chatMsgList = DataHelper.BytesToObject<List<string>>(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);

            Global._TCPManager.TcpOutPacketPool.Push(tcpOutPacket);

            if (null == chatMsgList || chatMsgList.Count <= 0)
            {
                return;
            }

            // Thực hiện chat
            for (int i = 0; i < chatMsgList.Count; i++)
            {
                TransferChatMsg(chatMsgList[i]);
            }

            chatMsgList = null;
        }

        /// <summary>
        /// Function xử lý chat gửi từ DATABASE QUA
        /// </summary>
        /// <param name="chatMsg"></param>
        public void TransferChatMsg(string chatMsg)
        {
            try
            {
                string[] fields = chatMsg.Split(':');
                if (fields.Length != 9)
                {
                    return;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string roleName = fields[1];
                int status = Convert.ToInt32(fields[2]);
                string toRoleName = fields[3];
                int index = Convert.ToInt32(fields[4]);
                string textMsg = fields[5];
                int chatType = Convert.ToInt32(fields[6]);
                int extTag1 = Convert.ToInt32(fields[7]);
                int serverLineID = Convert.ToInt32(fields[8]);

                // Dữ liệu chat có mã hóa hay không
                if (status == 1)
                {
                    textMsg = DataHelper.DecodeBase64(textMsg);
                }

                // nếu như máy chủ thực hiện chát lại chính là máy chủ này thì thôi đéo làm gì cả
                if (serverLineID == GameManager.ServerLineID)
                {
                    return;
                }

                // Nếu là lệnh từ DB gửi ra để thực hiện 1 điều gì đó thì đó
                // Như thông báo, add tiền cho ai đó,Reload data với client
                // Đây là nơi trò chuyện với GS hoặc client từ GAMEDB
                if (KTGlobal.ProseccDatabaseServerChat(textMsg, extTag1))
                {
                    return;
                }
                else
                {
                    // Nếu là chát faction
                    if (index == (int)ChatChannel.Faction)
                    {
                        // Thực hiện gửi nó cho các thành viên trong faction của mình
                        KTGlobal.SendFactionChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, extTag1, textMsg);
                    }
                    if (index == (int)ChatChannel.Guild)
                    {
                        // Thực hiện gửi nó cho
                        KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, extTag1, textMsg, null, roleName);
                    }
                    else if (index == (int)ChatChannel.Family)
                    {
                        //Gửi chat cho 1 gia tộc nào đó
                        KTGlobal.SendFamilyChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, extTag1, textMsg, null, roleName);
                    }
                    else if (index == (int)ChatChannel.Private)
                    {
                        // Gửi tin nhắn cá nhân cho nó
                        KTGlobal.SendPrivateChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, extTag1, textMsg);
                    }
                    else if (index == (int)ChatChannel.KuaFuLine)
                    {
                        // Nếu đây là máy chủ liên server thì sẽ thực hiện add msg này vào db cho các db khác với line khác line đã gửi
                        // Nếu đây là máy chủ thông thường thì sẽ thực hiện gửi về client dưới dạng kênh liên máy chủ

                        if (GameManager.IsKuaFuServer)
                        {
                            KuaFuChatData _ChatData = new KuaFuChatData();
                            _ChatData.chatType = chatType;
                            _ChatData.extTag1 = extTag1;
                            _ChatData.index = index;
                            _ChatData.roleID = roleID;
                            _ChatData.roleName = roleName;
                            _ChatData.serverLineID = serverLineID;
                            _ChatData.status = status;
                            _ChatData.textMsg = textMsg;
                            _ChatData.toRoleName = toRoleName;

                            // Nếu đây là liên server thì có nhiệm vụ nói chuyện với các gamedb khác và đẩy tin nhắn này vào cho bên đó xử lý
                            KuaFuManager.getInstance().PushChatData(_ChatData);

                            KTGlobal.SendKuaFuServerChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, textMsg, roleName);
                        }
                        else // nếu đây là 1 server thông thường được link với nó thì thực hiện bắn chat lên
                        {
                            KTGlobal.SendKuaFuServerChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, textMsg, roleName);
                        }
                    }
                    else
                    {
                        LogManager.WriteLog(LogTypes.Data, "ProseccDatabaseServerChat :" + textMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "TransferChatMsg", false);
            }
        }

        #endregion GetDatabaseServerChat

        /// <summary>
        /// Thêm 1 client vào
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool AddClient(KPlayer client)
        {
            try
            {
                KPlayer gc = FindClient(client.RoleID);
                if (null != gc)
                {
                    if (gc.ClosingClientStep > 0)
                    {
                        RemoveClient(gc);
                    }
                    else
                    {
                        return false;
                    }
                }

                int index = -1;
                lock (_FreeClientList)
                {
                    if (null == _FreeClientList || _FreeClientList.Count <= 0)
                    {
                        LogManager.WriteLog(LogTypes.Error,
                            string.Format("ClientManager::AddClient _FreeClientList.Count <= 0"));
                        return false;
                    }
                    index = _FreeClientList[0];
                    _FreeClientList.RemoveAt(0);
                }

                _ArrayClients[index] = client;
                client.ClientSocket.Nid = index;

                _DictClientNids[client.RoleID] = index;

                AddClientToContainer(client);
            }
            catch (Exception e)
            {
                LogManager.WriteLog(LogTypes.Error,
                        string.Format("ClientManager::AddClient ==>{0}", e.ToString()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Add client vào contaier chứa
        /// </summary>
        /// <param name="client"></param>
        public void AddClientToContainer(KPlayer client)
        {
            Container.AddObject(client);
        }

        /// <summary>
        /// Xóa bỏ 1 client
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(KPlayer client)
        {
            try
            {
                int nNid = FindClientNid(client.RoleID);
                if (nNid != client.ClientSocket.Nid)
                {
                    LogManager.WriteLog(LogTypes.Error,
                        string.Format("ClientManager::RemoveClient nNid={0}, client.ClientSocket.Nid={1]", nNid, client.ClientSocket.Nid));
                }
            }
            catch (Exception e)
            {
            }

            _DictClientNids.TryRemove(client.RoleID, out _);

            if (client.ClientSocket.Nid >= 0 && client.ClientSocket.Nid < MAX_CLIENT_COUNT)
            {
                _ArrayClients[client.ClientSocket.Nid] = null;
                lock (_FreeClientList)
                {
                    _FreeClientList.Add(client.ClientSocket.Nid);
                }
            }
            //if (client.ClientSocket.Nid < 0 || client.ClientSocket.Nid >= MAX_CLIENT_COUNT)
            else
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("ClientManager::RemoveClient nid={0} out range", client.ClientSocket.Nid));
                //return;
            }

            client.ClientSocket.Nid = -1;
            RemoveClientFromContainer(client);
        }

        /// <summary>
        /// Xóa người chơi khỏi MapGrid
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClientFromContainer(KPlayer client)
        {
            GameMap gameMap = null;
            /// Nếu bản đồ không tồn tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap) || null == gameMap)
            {
                return;
            }

            /// Xóa khỏi bản đồ
            GameManager.MapGridMgr.DictGrids[client.MapCode].RemoveObject(client);
            /// Xóa khỏi Container
            this.Container.RemoveObject(client);
        }

        /// <summary>
        /// Tìm người chơi theo điều kiện tương ứng
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<KPlayer> FindClients(Predicate<KPlayer> predicate)
        {
            lock (this._ArrayClients)
            {
                return this._ArrayClients.Where(x => x != null && predicate(x)).ToList();
            }
        }

        /// <summary>
        /// Tìm người chơi theo roleID
        /// </summary>
        public int FindClientNid(int RoleID)
        {
            int nNid = -1;
            if (!_DictClientNids.TryGetValue(RoleID, out nNid))
            {
                return -1;
            }
            return nNid;
        }

        public KPlayer FindClientByNid(int nNid)
        {
            if (nNid < 0 || nNid >= MAX_CLIENT_COUNT) return null;

            return _ArrayClients[nNid];
        }

        /// <summary>
        /// Tìm client theo socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public KPlayer FindClient(TMSKSocket socket)
        {
            if (null == socket) return null;

            return FindClientByNid(socket.Nid);
        }

        /// <summary>
        /// Tìm Client theo RoleID
        /// </summary>
        /// <param name="client"></param>
        public KPlayer FindClient(int roleID)
        {
            int nNid = FindClientNid(roleID);

            return FindClientByNid(nNid);
        }

        /// <summary>
        /// Lấy ra clien tiếp theo
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public KPlayer GetNextClient(ref int nNid)
        {
            if (nNid < 0 || nNid >= MAX_CLIENT_COUNT) return null;

            KPlayer client = null;
            for (; nNid < MAX_CLIENT_COUNT; nNid++)
            {
                if (null != _ArrayClients[nNid])
                {
                    client = _ArrayClients[nNid];

                    nNid++;
                    break;
                }
            }

            return client;
        }

        /// <summary>
        /// Trả về danh sách người chơi trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<KPlayer> GetMapClients(int mapCode, int copySceneID = -1)
        {
            /// Trả về kết quả
            return this.Container.GetObjectsByMap(mapCode, copySceneID);
        }

        /// <summary>
        /// Trả về danh sách người chơi trong bản đồ thỏa mãn điều kiện
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<KPlayer> GetMapClients(Predicate<KPlayer> predicate, int mapCode, int copySceneID = -1)
        {
            /// Trả về kết quả
            return this.Container.GetObjectsByMap(predicate, mapCode, copySceneID);
        }

        /// <summary>
        /// Đếm số người chơi trên bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetMapClientsCount(int mapCode, int copySceneID = -1)
        {
            return this.Container.GetObjectsCountByMap(mapCode, copySceneID);
        }

        /// <summary>
        /// Lấy ra tổng số client
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetClientCount()
        {
            int count = 0;
            lock (_FreeClientList)
            {
                count = _FreeClientList.Count;
            }

            return MAX_CLIENT_COUNT - count;
        }

        /// <summary>
        /// Lấy ra ngẫu nhiên 1 client
        /// </summary>
        /// <returns></returns>
        public KPlayer GetRandomClient()
        {
            if (_DictClientNids.Count > 0)
            {
                int[] array = new int[MAX_CLIENT_COUNT];
                _DictClientNids.Values.CopyTo(array, 0);
                int index = Global.GetRandomNumber(0, _DictClientNids.Count);
                return FindClientByNid(array[index]);
            }

            return null;
        }

        #endregion Số Client tối đa

        #region

        #region Gửi gói tin đến người chơi

        /// <summary>
        /// Thêm TCPOutPacket vào luồng gửi lại
        /// </summary>
        /// <param name="tcpOutPacket"></param>
        public void PushBackTcpOutPacket(TCPOutPacket tcpOutPacket)
        {
            if (null != tcpOutPacket)
            {
                Global._TCPManager.TcpOutPacketPool.Push(tcpOutPacket);
            }
        }

        /// <summary>
        /// Gửi gói tin đến tất cả người chơi xung quanh, nếu nhìn thấy bản thân
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="self"></param>
        /// <param name="objsList"></param>
        /// <param name="bytesData"></param>
        /// <param name="cmdID"></param>
        public void SendToClients(SocketListener sl, TCPOutPacketPool pool, object self, List<object> objsList, byte[] bytesData, int cmdID)
        {
            if (null == objsList)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;
            try
            {
                /// Duyệt danh sách người chơi
                for (int i = 0; i < objsList.Count; i++)
                {
                    /// Nếu là bản thân thì bỏ qua
                    if (self == objsList[i])
                    {
                        continue;
                    }

                    /// Người chơi đang xét
                    KPlayer client = objsList[i] as KPlayer;
                    /// Nếu người chơi không tồn tại
                    if (client == null)
                    {
                        continue;
                    }
                    /// Nếu đã Logout
                    if (client.LogoutState)
                    {
                        continue;
                    }

                    /// Nếu có thể nhìn thấy
                    if (self == null || (self is GameObject go && (!go.IsInvisible() || go.VisibleTo(client))))
                    {
                        if (null == tcpOutPacket)
                        {
                            tcpOutPacket = pool.Pop();
                            tcpOutPacket.PacketCmdID = (UInt16)cmdID;
                            tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);
                        }

                        /// Gửi gói tin đi
                        sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false);
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// Gửi gói tin đến tất cả người chơi xung quanh, nếu nhìn thấy bản thân
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients(SocketListener sl, TCPOutPacketPool pool, object self, List<object> objsList, string strCmd, int cmdID)
        {
            if (null == objsList)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;
            try
            {
                /// Duyệt danh sách người chơi
                for (int i = 0; i < objsList.Count; i++)
                {
                    /// Nếu là bản thân thì bỏ qua
                    if (self == objsList[i])
                    {
                        continue;
                    }

                    /// Người chơi đang xét
                    KPlayer client = objsList[i] as KPlayer;
                    /// Nếu người chơi không tồn tại
                    if (client == null)
                    {
                        continue;
                    }
                    /// Nếu đã Logout
                    if (client.LogoutState)
                    {
                        continue;
                    }

                    /// Nếu có thể nhìn thấy
                    if (self == null || (self is GameObject go && (!go.IsInvisible() || go.VisibleTo(client))))
                    {
                        if (null == tcpOutPacket)
                        {
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strCmd, cmdID);
                        }

                        /// Gửi gói tin đi
                        sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false);
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// Gửi gói tin đến tất cả người chơi xung quanh, nếu nhìn thấy bản thân
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients<T>(object self, List<object> objsList, T scData, int cmdID)
        {
            if (null == objsList)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;
            try
            {
                /// Duyệt danh sách người chơi
                for (int i = 0; i < objsList.Count; i++)
                {
                    /// Nếu là bản thân thì bỏ qua
                    if (self == objsList[i])
                    {
                        continue;
                    }

                    /// Người chơi đang xét
                    KPlayer client = objsList[i] as KPlayer;
                    /// Nếu người chơi không tồn tại
                    if (client == null)
                    {
                        continue;
                    }
                    /// Nếu đã Logout
                    if (client.LogoutState)
                    {
                        continue;
                    }

                    /// Nếu có thể nhìn thấy
                    if (self == null || (self is GameObject go && (!go.IsInvisible() || go.VisibleTo(client))))
                    {
                        /// Gửi gói tin đi
                        client.SendPacket(cmdID, scData);
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// Gửi gói tin đến người chơi
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strCmd, int cmdID)
        {
            /// Nếu người chơi không tồn tại
            if (client == null)
            {
                return;
            }
            /// Nếu đã Logout
            if (client.LogoutState)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strCmd, cmdID);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin đến người chơi
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(KPlayer client, string strCmd, int cmdID)
        {
            /// Nếu người chơi không tồn tại
            if (client == null)
            {
                return;
            }
            /// Nếu đã Logout
            if (client.LogoutState)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strCmd, cmdID);
            Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin đến người chơi
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(KPlayer client, byte[] buffer, int cmdID)
        {
            /// Nếu người chơi không tồn tại
            if (client == null)
            {
                return;
            }
            /// Nếu đã Logout
            if (client.LogoutState)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, buffer, 0, buffer.Length, cmdID);
            Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Gửi gói tin đến người chơi

        #region Notify

        /// <summary>
        /// Thông báo cho bản thân có đối tượng khác mới xuất hiện xung quanh
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="otherRoleID"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="currentDirection"></param>
        /// <param name="action"></param>
        /// <param name="paths"></param>
        /// <param name="camp"></param>
        public void NotifyMyselfOtherLoadAlready(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int otherRoleID, int currentX, int currentY, int toX, int toY, int currentDirection, int action, string paths, int camp)
        {
            LoadAlreadyData loadAlreadyData = new LoadAlreadyData()
            {
                RoleID = otherRoleID,
                PosX = currentX,
                PosY = currentY,
                Direction = currentDirection,
                Action = action,
                PathString = paths,
                ToX = toX,
                ToY = toY,
                Camp = camp,
            };

            TCPOutPacket tcpOutPacket = null;
            byte[] bytes = DataHelper.ObjectToBytes<LoadAlreadyData>(loadAlreadyData);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_LOADALREADY);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Notify cho ngươi chơi khác biết mình đang di chuyển
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMyMoving(SocketListener sl, TCPOutPacketPool pool, SpriteNotifyOtherMoveData moveData, KPlayer client, int cmd, List<Object> objsList = null)
        {
            if (null == objsList)
            {
                objsList = Global.GetAll9Clients(client);
            }

            if (null == objsList) return;

            SendToClients(sl, pool, client, objsList, DataHelper.ObjectToBytes<SpriteNotifyOtherMoveData>(moveData), cmd);
        }

        /// <summary>
        /// Thông báo cho người chơi khác đối tượng ngừng di chuyển
        /// </summary>
        /// <param name="obj"></param>
        public void NotifyOthersStopMyMoving(SocketListener sl, TCPOutPacketPool pool, GameObject obj)
        {
            List<Object> objsList = Global.GetAll9Clients(obj);

            /// Tạo mới gói tin
            byte[] data = DataHelper.ObjectToBytes<SpriteStopMove>(new SpriteStopMove()
            {
                RoleID = obj.RoleID,
                PosX = (int)obj.CurrentPos.X,
                PosY = (int)obj.CurrentPos.Y,
                MoveSpeed = obj.GetCurrentRunSpeed(),
                Direction = (int)obj.CurrentDir,
            });

            this.SendToClients(sl, pool, obj, objsList, data, (int)TCPGameServerCmds.CMD_SPR_STOPMOVE);
        }

        /// <summary>
        /// Thông báo đối tượng di chuyển
        /// </summary>
        /// <param name="client"></param>
        public bool NotifyOthersToMoving(SocketListener sl, TCPOutPacketPool pool, IObject obj, int mapCode, int copyMapID, int roleID, long startMoveTicks, int currentX, int currentY, int action, int toX, int toY, int cmd, int moveSpeed, String pathString = "", List<Object> objsList = null, KiemThe.Entities.Direction Dir = KiemThe.Entities.Direction.NONE)
        {
            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, currentX, currentY, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (null == objsList) return true;

            SpriteNotifyOtherMoveData moveData = new SpriteNotifyOtherMoveData()
            {
                RoleID = roleID,
                FromX = currentX,
                FromY = currentY,
                ToX = toX,
                ToY = toY,
                PathString = pathString,
                Action = action,
            };
            this.SendToClients(sl, pool, null, objsList, DataHelper.ObjectToBytes<SpriteNotifyOtherMoveData>(moveData), cmd);

            return true;
        }

        /// <summary>
        /// Thông báo tới bản thân có quái mới xuất hiện
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="monsterID"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="currentDirection"></param>
        /// <param name="action"></param>
        /// <param name="paths"></param>
        /// <param name="camp"></param>
        public void NotifyMyselfMonsterLoadAlready(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int monsterID, int currentX, int currentY, int toX, int toY, int currentDirection, int action, string paths, int camp)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}", monsterID, mapCode, currentX, currentY, currentDirection, action, toX, toY, moveCost, extAction, startMoveTicks, pathString, currentPathIndex);//怪物路径列表默认是空

            LoadAlreadyData loadAlreadyData = new LoadAlreadyData()
            {
                RoleID = monsterID,
                PosX = currentX,
                PosY = currentY,
                Direction = currentDirection,
                Action = action,
                PathString = paths,
                ToX = toX,
                ToY = toY,
                Camp = camp,
            };

            TCPOutPacket tcpOutPacket = null;
            byte[] bytes = DataHelper.ObjectToBytes<LoadAlreadyData>(loadAlreadyData);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_LOADALREADY);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Thông báo tới bản thân có BOT mới xuất hiện
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="botID"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="currentDirection"></param>
        /// <param name="action"></param>
        /// <param name="paths"></param>
        /// <param name="camp"></param>
        public void NotifyMyselfBotLoadAlready(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int botID, int currentX, int currentY, int toX, int toY, int currentDirection, int action, string paths, int camp)
        {
            LoadAlreadyData loadAlreadyData = new LoadAlreadyData()
            {
                RoleID = botID,
                PosX = currentX,
                PosY = currentY,
                Direction = currentDirection,
                Action = action,
                PathString = paths,
                ToX = toX,
                ToY = toY,
                Camp = camp,
            };

            byte[] bytes = DataHelper.ObjectToBytes<LoadAlreadyData>(loadAlreadyData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_LOADALREADY);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Notify

        #region Sống lại khi chết

        /// <summary>
        /// Thông báo cho tất cả người chơi xung quanh bản thân sống lại
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersRealive(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int posX, int posY, int direction, int hp)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
            {
                RoleID = roleID,
                PosX = posX,
                PosY = posY,
                Direction = direction,
                CurrentHP = hp,
                CurrentMP = 0,
                CurrentStamina = 0,
            };
            byte[] bytes = DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData);

            SendToClients(sl, pool, client, objsList, /*strcmd*/bytes, (int)TCPGameServerCmds.CMD_SPR_REALIVE);
        }

        /// <summary>
        /// Thông báo bản thân sống lại
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="roleID"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="direction"></param>
        /// <param name="cmd"></param>
        public void NotifyMySelfRealive(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int posX, int posY, int direction, int hp, int mp, int stamina)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, posX, posY, direction);
            MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
            {
                RoleID = roleID,
                PosX = posX,
                PosY = posY,
                Direction = direction,
                CurrentHP = hp,
                CurrentMP = mp,
                CurrentStamina = stamina,
            };
            byte[] bytes = DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_REALIVE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Thông báo quái tái sinh
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="obj"></param>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        /// <param name="roleID"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="direction"></param>
        /// <param name="series"></param>
        /// <param name="objsList"></param>
        public void NotifyMonsterRelive(SocketListener sl, TCPOutPacketPool pool, IObject obj, int mapCode, int copyMapID, int roleID, int posX, int posY, int direction, int series, int hp, List<Object> objsList)
        {
            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, posX, posY, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, posX, posY, direction);
            MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
            {
                RoleID = roleID,
                PosX = posX,
                PosY = posY,
                Direction = direction,
                Series = series,
                CurrentHP = hp,
            };

            //群发消息
            SendToClients(sl, pool, null, objsList, /*strcmd*/DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData), (int)TCPGameServerCmds.CMD_SPR_REALIVE);
        }

        #endregion Sống lại khi chết

        #region Notify leave cho người chơi xung quanh

        /// <summary>
        /// 角色离线(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersLeave(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, (int)GSpriteTypes.Other);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
        }

        /// <summary>
        /// Thông báo bản thân bị xóa cho tất cả người chơi khác
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMyselfLeaveOthers(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (client.RoleID == (objsList[i] as KPlayer).RoleID)
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}", (objsList[i] as KPlayer).RoleID, (int)GSpriteTypes.Other);

                TCPOutPacket tcpOutPacket = null;
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Notify xóa quái
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMyselfLeaveMonsters(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is Monster))
                {
                    continue;
                }

                if (!NotifyMyselfLeaveMonsterByID(sl, pool, client, (objsList[i] as Monster).RoleID))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Notify xóa quái
        /// </summary>
        /// <param name="client"></param>
        public bool NotifyMyselfLeaveMonsterByID(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int monsterID)
        {
            string strcmd = string.Format("{0}:{1}", monsterID, (int)GSpriteTypes.Monster);

            TCPOutPacket tcpOutPacket = null;
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                return false;
            }

            return true;
        }

        #endregion Notify leave cho người chơi xung quanh

        #region Thông báo HP

        /// <summary>
        /// ĐÉO DÙNG
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersLifeChanged(SocketListener sl, TCPOutPacketPool pool, KPlayer client, bool allSend = true, bool resetMax = false)
        {
            if (!resetMax)
            {
                client.m_CurrentLife = Global.GMin(client.m_CurrentLife, client.m_CurrentLifeMax);
                client.m_CurrentMana = Global.GMin(client.m_CurrentMana, client.m_CurrentManaMax);
            }
            else
            {
                client.m_CurrentLife = client.m_CurrentLifeMax;
                client.m_CurrentMana = client.m_CurrentManaMax;
            }

            SpriteLifeChangeData lifeChangeData = new SpriteLifeChangeData();

            byte[] cmdData = DataHelper.ObjectToBytes<SpriteLifeChangeData>(lifeChangeData);

            if (!allSend)
            {
                if (null != client)
                {
                    TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_UPDATE_ROLEDATA);
                    if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                    {
                    }
                }

                return;
            }

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            SendToClients(sl, pool, null, objsList, /*strcmd*/cmdData, (int)TCPGameServerCmds.CMD_SPR_UPDATE_ROLEDATA);
        }

        #endregion Thông báo HP

        #region Dịch chuyển
        /// <summary>
        /// Cập nhật vị trí người chơi mà không thông báo về Client
        /// </summary>
        /// <param name="player"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public void ForceChangePosWithoutNotify(KPlayer player, int posX, int posY)
        {
            /// Thiết lập vị trí hiện tại là vị trí ở Client truyền lên
            player.CurrentPos = new Point(posX, posY);

            /// Dịch trong MapGrid tương ứng
            GameManager.MapGridMgr.DictGrids[player.MapCode].MoveObject(-1, -1, posX, posY, player);
        }

        /// <summary>
        /// Cập nhật vị trí người chơi và chỉ thông báo tới bản thân
        /// </summary>
        /// <param name="player"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public void ForceChangePosNotifySelfOnly(KPlayer player, int posX, int posY)
        {
            /// Thiết lập vị trí hiện tại là vị trí ở Client truyền lên
            player.CurrentPos = new Point(posX, posY);

            /// Ghi lại vị trí đích đến
            player.LastChangedPosition = new Point(posX, posY);

            /// Dịch trong MapGrid tương ứng
            GameManager.MapGridMgr.DictGrids[player.MapCode].MoveObject(-1, -1, posX, posY, player);

            /// Gửi Packet
            string strcmd = string.Format("{0}:{1}:{2}:{3}", player.RoleID, posX, posY, player.RoleDirection);
            this.SendToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, strcmd, (int) TCPGameServerCmds.CMD_SPR_CHANGEPOS);
        }

        /// <summary>
        /// Thông báo đối tượng khác bản thân dịch đến vị trí tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="roleID"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="direction"></param>
        /// <param name="cmd"></param>
        public void NotifyOthersGoBack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int toPosX = -1, int toPosY = -1, int direction = -1)
        {
            /// Đánh dấu đợi chuyển Pos
            client.WaitingForChangePos = true;
            /// Đánh dấu thời điểm lần cuối đợi chuyển Pos
            client.LastChangePosTicks = KTGlobal.GetCurrentTimeMilis();

            int posX = toPosX;
            int posY = toPosY;

            /// Lỗi tọa độ thì lấy tân thủ thôn
            if (-1 == posX || -1 == posY)
            {
                /// Mặc định kết quả
                KiemThe.Entities.NewbieVillage newbieVillage = KTGlobal.NewbieVillages[KTGlobal.GetRandomNumber(0, KTGlobal.NewbieVillages.Count - 1)];
                int newbieMapCode = newbieVillage.ID;
                int newbiePosX = newbieVillage.Position.X;
                int newbiePosY = newbieVillage.Position.Y;
                Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, newbieMapCode, newbiePosX, newbiePosY, 0);
                posX = (int)newPos.X;
                posY = (int)newPos.Y;
            }

            if (direction >= 0)
            {
                client.RoleDirection = direction;
            }

            GameManager.ClientMgr.ChangePosition(sl, pool, client, (int)posX, (int)posY, direction, (int) TCPGameServerCmds.CMD_SPR_CHANGEPOS);
        }

        #endregion Dịch chuyển

        #region Đổi trang bị

        /// <summary>
        /// Thông báo bản thân đổi trang bị
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        /// <param name="refreshNow"></param>
        public void NotifyOthersChangeEquip(SocketListener sl, TCPOutPacketPool pool, KPlayer client, GoodsData goodsData, int refreshNow)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            ChangeEquipData changeEquipData = new ChangeEquipData()
            {
                RoleID = client.RoleID,
                EquipGoodsData = goodsData,
            };

            byte[] bytesData = DataHelper.ObjectToBytes<ChangeEquipData>(changeEquipData);

            SendToClients(sl, pool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_CHGCODE);
        }

        #endregion Đổi trang bị

        #region Update Quest

        /// <summary>
        /// Update tình trạng nhiệm vụ
        /// </summary>
        public void NotifyUpdateTask(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int dbID, int taskID, int taskVal1, int taskVal2, int taskFocus)
        {
            //
            string strcmd = "";
            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", dbID, taskID, taskVal1, taskVal2, taskFocus);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_MODTASK);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Notify về trạng thái quest của NPC
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="npcID"></param>
        /// <param name="state"></param>
        public void NotifyUpdateNPCTaskSate(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int npcID, int state)
        {
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", npcID, state);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_UPDATENPCSTATE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Gửi trạng thái về cho NPC có task hay không có task
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="npcTaskStatList"></param>
        public void NotifyNPCTaskStateList(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<NPCTaskState> npcTaskStatList)
        {
            //
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<NPCTaskState>>(npcTaskStatList, pool, (int)TCPGameServerCmds.CMD_SPR_NPCSTATELIST);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
            }
        }

        /// <summary>
        /// 给与新手任务 [XSea 2015/4/14]
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="client">角色</param>
        /// <param name="nNeedTakeStartTask">是否需要起始任务</param>
        /// <returns>true=成功，false=失败</returns>
        public bool GiveFirstTask(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, TCPRandKey tcpRandKey, KPlayer client, bool bNeedTakeStartTask)
        {
            if (null == client)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("client不存在，无法给与新手任务"));
                return false;
            }

            // Trao cho nhân vật nhiệm vụ đầu tiên
            int nRoleID = client.RoleID;

            try
            {
                if (MainTaskManager.getInstance().CanTakeNewTask(client, 100))
                {
                    MainTaskManager.getInstance().AppcepTask(null, client, 100);
                }

                return true;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
            }
            return false;
        }

        #endregion Update Quest

        #region Sổ đen - bạn bè hảo hữu

        /// <summary>
        /// Xóa kẻ thù cũ
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="killedRoleID"></param>
        public bool RemoveOldestEnemy(TCPManager tcpMgr, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client)
        {
            int totalCount = Global.GetFriendCountByType(client, 2); //获取敌人的数量
            if (totalCount < (int)FriendsConsts.MaxEnemiesNum)
            {
                return true;
            }

            FriendData friendData = Global.FindFirstFriendDataByType(client, 2);
            if (null == friendData)
            {
                return true;
            }

            return GameManager.ClientMgr.RemoveFriend(tcpMgr, tcpClientPool, pool, client, friendData.DbID);
        }

        /// <summary>
        /// Thêm danh sách kẻ thủ
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="friendType"></param>
        public void AddToEnemyList(TCPManager tcpMgr, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int killedRoleID)
        {
            KPlayer findClient = FindClient(killedRoleID);
            if (null == findClient)
            {
                return;
            }

            //Xóa bỏ kẻ thù cũ
            if (!RemoveOldestEnemy(tcpMgr, tcpClientPool, pool, findClient))
            {
                return;
            }

            int friendDbID = -1;
            FriendData friendData = Global.FindFriendData(findClient, client.RoleID);
            if (null != friendData)
            {
                friendDbID = friendData.DbID;
            }

            int enemyCount = Global.GetFriendCountByType(findClient, 2);
            if (enemyCount >= (int)FriendsConsts.MaxEnemiesNum)
            {
                GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, findClient,
                    StringUtil.substitute(Global.GetLang("Danh sách kẻ thù đã đầy"),
                    (int)FriendsConsts.MaxEnemiesNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);

                return;
            }

            if (friendData == null || (friendData.FriendType != 0 && friendData.FriendType != 2))
            {
                AddFriend(tcpMgr, tcpClientPool, pool, findClient, friendDbID, client.RoleID, Global.FormatRoleName(client, client.RoleName), 2);
            }
        }

        /// <summary>
        /// Xóa khỏi danh sách bạn
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="dbID"></param>
        public bool RemoveFriend(TCPManager tcpMgr, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int dbID)
        {
            bool ret = false;

            try
            {
                string strcmd = string.Format("{0}:{1}", dbID, client.RoleID);
                byte[] bytesCmd = new UTF8Encoding().GetBytes(strcmd);

                /// Thông tin kết bạn
                FriendData friendData = Global.GetFriendData(client, dbID);
                /// Nếu thông tin không tồn tại
                if (friendData == null)
                {
                    PlayerManager.ShowNotification(client, "Không tìm thấy thông tin bạn bè tương ứng, hãy thử thoát Game vào lại!");
                    return false;
                }

                TCPOutPacket tcpOutPacket = null;
                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, client.ClientSocket, tcpClientPool, null, pool, (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND, bytesCmd, bytesCmd.Length, out tcpOutPacket, client.ServerId);
                if (TCPProcessCmdResults.RESULT_FAILED != result)
                {
                    string strData = new UTF8Encoding().GetString(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);
                    string[] fields = strData.Split(':');
                    if (fields.Length == 3 && Convert.ToInt32(fields[2]) >= 0)
                    {
                        Global.RemoveFriendData(client, dbID);
                        /// Gửi gói tin đi
                        byte[] _cmdData = new UTF8Encoding().GetBytes(string.Format("{0}:{1}:{2}", dbID, friendData.OtherRoleName, friendData.OtherRoleID));
                        GameManager.ClientMgr.SendToClient(client, _cmdData, (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND);

                        /// Thằng kia
                        KPlayer otherClient = GameManager.ClientMgr.FindClient(friendData.OtherRoleID);
                        /// Nếu đang online
                        if (otherClient != null)
                        {
                            Global.RemoveFriendData(otherClient, dbID);
                            /// Gửi gói tin đi
                            byte[] __cmdData = new UTF8Encoding().GetBytes(string.Format("{0}:{1}:{2}", dbID, client.RoleName, client.RoleID));
                            GameManager.ClientMgr.SendToClient(otherClient, __cmdData, (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND);
                        }
                    }

                    ret = true;
                }
                return ret;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
            }

            return ret;
        }

        /// <summary>
        /// Thêm bạn - Kẻ thù -Sổ đen
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="friendType"></param>
        public bool AddFriend(TCPManager tcpMgr, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int dbID, int otherRoleID, string otherRoleName, int friendType)
        {
            bool ret = false;

            if (client.ClientSocket.IsKuaFuLogin)
            {
                return false;
            }

            // Bạn không thể thêm chính bạn
            if (friendType == 2 && otherRoleID == client.RoleID)
            {
                return false;
            }

            try
            {
                FriendData friendData = null;
                if (otherRoleID > 0)
                {
                    // nếu thằng kia đã có trong danh sách bạn rồi thì thôi
                    friendData = Global.FindFriendData(client, otherRoleID);
                    if (null != friendData)
                    {
                        if (friendData.FriendType == friendType) // Nếu đã là bạn rồi
                        {
                            return ret;
                        }
                    }
                }

                //Đếm xem type đã có bao nhiêu
                int friendTypeCount = Global.GetFriendCountByType(client, friendType);
                if (0 == friendType)
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxFriendsNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, "Danh sách bạn bè đã đầy", GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return ret;
                    }
                }
                else if (1 == friendType)
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxBlackListNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, "Danh sách sổ đen đã đầy", GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return ret;
                    }
                }
                else if (2 == friendType)
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxEnemiesNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, "Danh sách kẻ thù đã đầy", GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return ret;
                    }
                }

                string strcmd = string.Format("{0}:{1}:{2}:{3}", dbID, client.RoleID, otherRoleName, friendType);
                byte[] bytesCmd = new UTF8Encoding().GetBytes(strcmd);

                TCPOutPacket tcpOutPacket = null;
                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, client.ClientSocket, tcpClientPool, null, pool, (int)TCPGameServerCmds.CMD_SPR_ADDFRIEND, bytesCmd, bytesCmd.Length, out tcpOutPacket, client.ServerId);

                if (null == tcpOutPacket)
                {
                    return ret;
                }

                friendData = DataHelper.BytesToObject<FriendData>(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);
                /// Thằng kia
                KPlayer otherClient = null;
                if (null != friendData && friendData.DbID >= 0)
                {
                    ret = true;
                    otherClient = GameManager.ClientMgr.FindClient(friendData.OtherRoleID);
                }

                /// Pha của Client
                {
                    Global.RemoveFriendData(client, friendData.DbID);
                    Global.AddFriendData(client, friendData);

                    /// Cập nhật thông tin
                    friendData.MapCode = otherClient == null ? -1 : otherClient.CurrentMapCode;
                    /// Dấu tọa độ
                    friendData.PosX = 0;
                    friendData.PosY = 0;
                    /// Cập nhật trạng thái Online
                    friendData.OnlineState = otherClient != null ? 1 : 0;

                    if (0 == friendType) //Nếu là bạn
                    {
                        friendTypeCount = Global.GetFriendCountByType(client, friendType);
                        if (1 == friendTypeCount)
                        {
                            //Hoàn thành achement thêm 1 người bạn đầu tiên
                            ChengJiuManager.OnFirstAddFriend(client);
                        }
                    }

                    /// Gửi gói tin đi
                    byte[] _cmdData = DataHelper.ObjectToBytes<FriendData>(friendData);
                    GameManager.ClientMgr.SendToClient(client, _cmdData, (int)TCPGameServerCmds.CMD_SPR_ADDFRIEND);
                }

                /// Pha của OtherPlayer
                if (otherClient != null)
                {
                    /// Nếu là thêm bạn
                    if (friendData.FriendType == 0)
                    {
                        /// Tạo FriendData mới chứa thông tin của Client cho otherClient
                        FriendData otherFriendData = new FriendData()
                        {
                            DbID = friendData.DbID,
                            FriendType = friendData.FriendType,
                            Relationship = friendData.Relationship,
                            FactionID = client.m_cPlayerFaction.GetFactionId(),
                            GuildID = client.GuildID,
                            MapCode = client.CurrentMapCode,
                            /// Dấu tọa độ
                            PosX = 0,
                            PosY = 0,

                            OnlineState = 1,
                            OtherLevel = client.m_Level,
                            OtherRoleID = client.RoleID,
                            OtherRoleName = client.RoleName,
                            PicCode = client.RolePic,
                            SpouseId = -1,
                        };

                        Global.RemoveFriendData(otherClient, otherFriendData.DbID);
                        Global.AddFriendData(otherClient, otherFriendData);

                        /// Gửi gói tin đi
                        byte[] _cmdData = DataHelper.ObjectToBytes<FriendData>(otherFriendData);
                        GameManager.ClientMgr.SendToClient(otherClient, _cmdData, (int)TCPGameServerCmds.CMD_SPR_ADDFRIEND);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
            }
            return ret;
        }

        #endregion Sổ đen - bạn bè hảo hữu

        #region Giao dịch vật phẩm

        /// <summary>
        /// Thông báo thông tin giao dịch về client
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGoodsExchangeCmd(SocketListener sl, TCPOutPacketPool pool, int roleID, int otherRoleID, KPlayer client, KPlayer otherClient, int status, int exchangeType)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", status, roleID, otherRoleID, exchangeType);
            TCPOutPacket tcpOutPacket = null;

            //Gửi về cho thằng gửi lời mời giao dịch
            if (null != client)
            {
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                }
            }
            // Gửi cho thằng được mời giao dịch
            if (null != otherClient)
            {
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE);
                if (!sl.SendData(otherClient.ClientSocket, tcpOutPacket))
                {
                }
            }
        }

        /// <summary>
        /// Gửi thông tin giao dịch về Client
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGoodsExchangeData(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer otherClient, ExchangeData ed)
        {
            byte[] bytesData = null;

            lock (ed)
            {
                bytesData = DataHelper.ObjectToBytes<ExchangeData>(ed);
            }

            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = pool.Pop();
            tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_EXCHANGEDATA;
            tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }

            tcpOutPacket = pool.Pop();
            tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_EXCHANGEDATA;
            tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

            if (!sl.SendData(otherClient.ClientSocket, tcpOutPacket))
            {
            }
        }

        #endregion Giao dịch vật phẩm

        #region Vật phẩm rơi ở Map

        /// <summary>
        /// Thông báo có vật phẩm rơi ở Map
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="ownerRoleID"></param>
        /// <param name="autoID"></param>
        /// <param name="goodsData"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="productTicks"></param>
        /// <param name="teamID"></param>
        public void NotifyMySelfNewGoodsPack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int ownerRoleID, int autoID, GoodsData goodsData, int toX, int toY, long productTicks, int teamID)
        {
            /// Số sao
            int nStar = 0;
            StarLevelStruct starStruct = ItemManager.ItemValueCalculation(goodsData, out long itemValue);
            /// Nếu không có sao
            if (starStruct != null)
            {
                nStar = starStruct.StarLevel / 2;
            }

            NewGoodsPackData newGoodsPackData = new NewGoodsPackData()
            {
                OwnerRoleID = ownerRoleID,
                AutoID = autoID,
                PosX = toX,
                PosY = toY,
                GoodsID = goodsData.GoodsID,
                GoodCount = goodsData.GCount,
                ProductTicks = productTicks,
                TeamID = teamID,
                HTMLColor = KTGlobal.GetItemNameColor(goodsData),
                Star = nStar,
                EnhanceLevel = goodsData.Forge_level,
            };

            byte[] bytes = DataHelper.ObjectToBytes<NewGoodsPackData>(newGoodsPackData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_NEWGOODSPACK);

            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Thông báo về là nhặt được cái gì
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfGetThing(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int goodsDbID)
        {
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", client.RoleID, goodsDbID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETTHING);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Thông báo tới tất cả người chơi xóa vật phẩm rơi ở Map
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersDelGoodsPack(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int mapCode, int autoID, int toRoleID)
        {
            if (null == objsList) return;
            string strcmd = string.Format("{0}:{1}", autoID, toRoleID);
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELGOODSPACK);
        }

        /// <summary>
        /// Thông báo tới bản thân xóa vật phẩm rơi ở Map
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelGoodsPack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int autoID)
        {
            string strcmd = string.Format("{0}:{1}", autoID, -1);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELGOODSPACK);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Vật phẩm rơi ở Map

        #region 个人紧要消息通知

        /// <summary>
        /// 通知在线的对方(不限制地图)个人紧要消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyImportantMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string msgText, GameInfoTypeIndexes typeIndex, ShowGameInfoTypes showGameInfoType, int errCode = 0)
        {
            if (showGameInfoType == ShowGameInfoTypes.ErrAndBox)
            {
                PlayerManager.ShowMessageBox(client, "Thông báo", msgText);
            }
            else
            {
                PlayerManager.ShowNotification(client, msgText);
            }
        }

        #endregion 个人紧要消息通知

        #region Chat

        /// <summary>
        /// Gửi tin nhắn hệ thống đến người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="content"></param>
        public void SendSystemChatMessageToClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string content)
        {
            this.SendChatMessage(sl, pool, client, null, client, content, ChatChannel.System, null);
        }

        /// <summary>
        /// Gửi tin nhắn hệ thống đến người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="content"></param>
        public void SendSystemChatMessageToClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string content, List<GoodsData> items)
        {
            this.SendChatMessage(sl, pool, client, null, client, content, ChatChannel.System, items);
        }

        /// <summary>
        /// Gửi tin nhắn kênh mặc định đến người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="content"></param>
        public void SendDefaultTypeChatMessageToClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string content)
        {
            this.SendChatMessage(sl, pool, client, null, client, content, ChatChannel.Default, null);
        }

        /// <summary>
        /// Gửi tin nhắn tới người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client">Đối tượng được gửi gói tin về</param>
        /// <param name="fromClient"></param>
        /// <param name="toClient"></param>
        /// <param name="content"></param>
        /// <param name="channel"></param>
        public void SendChatMessage(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer fromClient, KPlayer toClient, string content, ChatChannel channel, List<GoodsData> items)
        {
            string FromRole = "";

            if (fromClient != null)
            {
                FromRole = fromClient.RoleName;

                if (KTGMCommandManager.IsGMByRoleID(client.RoleID))
                {
                    FromRole = fromClient.RoleID + "|" + fromClient.RoleName;
                }
            }

            SpriteChat chat = new SpriteChat()
            {
                FromRoleName = FromRole,
                ToRoleName = toClient.RoleName,
                Channel = (int)channel,
                Content = content,
                Items = items,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_CHAT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        public void SendChatMessageWithName(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string fromClient, KPlayer toClient, string content, ChatChannel channel, List<GoodsData> items)
        {
            SpriteChat chat = new SpriteChat()
            {
                FromRoleName = fromClient,
                ToRoleName = toClient.RoleName,
                Channel = (int)channel,
                Content = content,
                Items = items,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_CHAT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Chat

        #region Chuyển cảnh

        /// <summary>
        /// Người chơi đổi bản đồ
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="toMapCode"></param>
        /// <param name="toMapX"></param>
        /// <param name="toMapY"></param>
        /// <param name="toMapDirection"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public bool NotifyChangeMap(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int toMapCode, int mapX = -1, int mapY = -1, int direction = -1, bool toEnterCopyScene = false)
        {
            /// Bản đồ tương ứng
            GameMap gameMap = GameManager.MapMgr.GetGameMap(toMapCode);
            /// Nếu bản đồ không tồn tại
            if (gameMap == null)
            {
                PlayerManager.ShowNotification(client, "Bản đồ này chưa được mở!");
                return false;
            }
            /// Nếu là phụ bản
            if (gameMap != null && gameMap.IsCopyScene && !toEnterCopyScene)
            {
                PlayerManager.ShowNotification(client, "Không thể tiến vào bản đồ phụ bản!");
                return false;
            }

            /// Thực thi sự kiện trước khi chuyển bản đồ
            client.OnPreChangeMap(gameMap);

            /// Đánh dấu đang đợi chuyển Map
            client.WaitingForChangeMap = true;
            /// ĐÁNH DẤU LẠI THỜI GIAN GẦN ĐÂY NHẤT CHUYỂN BẢN ĐỒ
            client.LastChangeMapTicks = KTGlobal.GetCurrentTimeMilis();

            /// Lấy danh sách các đối tượng xung quanh
            List<Object> objsList = Global.GetAll9Clients(client);
            /// Thông báo đối tượng rời khỏi vị trí cho các đối tượng xung quanh
            GameManager.ClientMgr.NotifyOthersLeave(sl, pool, client, objsList);
            /// Xóa toàn bộ những thằng xung quanh của mình
            client.ClearVisibleObjects(true);

            /// Xóa khỏi bản đồ
            GameManager.MapGridMgr.DictGrids[client.MapCode].RemoveObject(client);
            /// Đánh dấu vị trí đích cần dịch đến
            client.WaitingChangeMapCode = toMapCode;
            client.WaitingChangeMapPosX = mapX;
            client.WaitingChangeMapPosY = mapY;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, toMapCode, mapX, mapY, direction, 0);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYCHGMAP);
            sl.SendData(client.ClientSocket, tcpOutPacket);

            return true;
        }

        /// <summary>
        /// Thực hiện chuyển bản đồ
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="toMapCode"></param>
        /// <param name="toMapX"></param>
        /// <param name="toMapY"></param>
        /// <param name="toMapDirection"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public bool ChangeMap(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int teleport, int toMapCode, int toMapX, int toMapY, int toMapDirection, int nID)
        {
            /// ĐÁNH DẤU LẠI THỜI GIAN GẦN ĐÂY NHẤT CHUYỂN BẢN ĐỒ
            client.LastChangeMapTicks = KTGlobal.GetCurrentTimeMilis();
            /// Đánh dấu đang chờ chuyển map
            client.WaitingForChangeMap = true;

            /// Ngừng di chuyển
            KTPlayerStoryBoardEx.Instance.Remove(client);
            /// Ngừng Blink
            client.StopBlink();

            /// Xóa đối tượng khỏi vị trí hiện tại được quản lý bởi MapGrid
            GameManager.ClientMgr.RemoveClientFromContainer(client);

            if (toMapCode > 0)
            {
                GameMap gameMap = GameManager.MapMgr.GetGameMap(toMapCode);
                /// Nếu bản đồ tồn tại
                if (null != gameMap)
                {
                    /// Nếu bản đồ không thể vào được
                    if (!gameMap.CanMove(toMapX / gameMap.MapGridWidth, toMapY / gameMap.MapGridHeight))
                    {
                        toMapX = -1;
                        toMapY = -1;
                    }
                }
                /// Nếu bản đồ không tồn tại
                else
                {
                    toMapCode = -1;
                }
            }

            GameMap toMap = null;
            /// Nếu tồn tại vị trí đích đến
            if (toMapX <= 0 || toMapY <= 0)
            {
                KiemThe.Entities.NewbieVillage newbieVillage = KTGlobal.NewbieVillages[KTGlobal.GetRandomNumber(0, KTGlobal.NewbieVillages.Count - 1)];
                int newbieMapCode = newbieVillage.ID;
                int newbiePosX = newbieVillage.Position.X;
                int newbiePosY = newbieVillage.Position.Y;
                toMap = GameManager.MapMgr.DictMaps[newbieMapCode];
                Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, newbieMapCode, newbiePosX, newbiePosY, 0);
                toMapX = (int)newPos.X;
                toMapY = (int)newPos.Y;
            }

            /// ID bản đồ cũ
            int oldMapCode = client.MapCode;

            /// Thiết lập vị trí và bản đồ mới
            client.MapCode = toMapCode;
            client.PosX = toMapX;
            client.PosY = toMapY;
            client.ReportPosTicks = 0;

            /// Ghi lại vị trí đích đến
            client.LastChangedPosition = new Point(toMapX, toMapY);

            /// Cập nhật động tác của đối tượng
            client.CurrentAction = (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand;

            /// Thêm đối tượng vào danh sách quản lý
            GameManager.ClientMgr.AddClientToContainer(client);

            /// Dịch đối tượng sang vị trí mới quản lý bởi MapGrid
            if (!GameManager.MapGridMgr.DictGrids[client.MapCode].MoveObject(-1, -1, toMapX, toMapY, client))
            {
                PlayerManager.ShowNotification(client, "Không thể chuyển bản đồ do lỗi!");
                LogManager.WriteLog(LogTypes.Warning, string.Format("Can not leave Map: Cmd={0}, RoleID={1}, Closed", (TCPGameServerCmds)nID, client.RoleID));
                return false;
            }

            /// Gửi gói tin về Client
            SCMapChange scData = new SCMapChange()
            {
                MapCode = toMapCode,
                PosX = toMapX,
                PosY = toMapY,
                RoleID = client.RoleID,
                TeleportID = teleport,
                ErrorCode = -1,
                Direction = toMapDirection,
            };
            client.SendPacket((int)TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

            return true;
        }

        /// <summary>
        /// Thay đổi vị trí của đối tượng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="toMapX"></param>
        /// <param name="toMapY"></param>
        /// <param name="toMapDirection"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public bool ChangePosition(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int toMapX, int toMapY, int toMapDirection, int nID, int animation = 0)
        {
            //Console.WriteLine(new System.Diagnostics.StackTrace());

            /// Ngừng StoryBoard
            KTPlayerStoryBoardEx.Instance.Remove(client);
            /// Ngừng Blink
            client.StopBlink();

            if (toMapX <= 0 || toMapY <= 0)
            {
                KiemThe.Entities.NewbieVillage newbieVillage = KTGlobal.NewbieVillages[KTGlobal.GetRandomNumber(0, KTGlobal.NewbieVillages.Count - 1)];
                int newbieMapCode = newbieVillage.ID;
                int newbiePosX = newbieVillage.Position.X;
                int newbiePosY = newbieVillage.Position.Y;
                Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, newbieMapCode, newbiePosX, newbiePosY, 0);
                toMapX = (int)newPos.X;
                toMapY = (int)newPos.Y;
            }

            ///// Lấy danh sách các đối tượng xung quanh
            //List<Object> nearbyClients = Global.GetAll9Clients(client);
            ///// Thông báo đối tượng rời khỏi vị trí cho các đối tượng xung quanh
            //GameManager.ClientMgr.NotifyMyselfLeaveOthers(sl, pool, client, nearbyClients);
            ///// Xóa toàn bộ những thằng xung quanh của mình
            //client.ClearVisibleObjects(true);

            /// Vị trí cũ
            int oldX = client.PosX;
            int oldY = client.PosY;

            /// Cập nhật tọa độ
            client.PosX = toMapX;
            client.PosY = toMapY;
            client.ReportPosTicks = 0;

            /// Ghi lại vị trí đích đến
            client.LastChangedPosition = new Point(toMapX, toMapY);

            /// Cập nhật hướng
            if (toMapDirection > 0)
            {
                client.RoleDirection = toMapDirection;
            }

            /// Dịch đối tượng đến vị trí mới
            if (!GameManager.MapGridMgr.DictGrids[client.MapCode].MoveObject(-1, -1, toMapX, toMapY, client))
            {
                client.PosX = oldX;
                client.PosY = oldY;
                client.ReportPosTicks = 0;
            }

            //ClientManager.DoSpriteMapGridMove(client);

            /// Thông báo thằng này thay đổi tọa độ đến bọn xung quanh
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return true;

            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, toMapX, toMapY, toMapDirection);
            SendToClients(sl, pool, null, objsList, strcmd, nID);

            /// Bỏ đánh dấu đợi chuyển Pos
            client.WaitingForChangePos = false;

            return true;
        }

        /// <summary>
        /// 切换在地图上的位置2
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="toMapX"></param>
        /// <param name="toMapY"></param>
        /// <param name="toMapDirection"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public bool ChangePosition2(SocketListener sl, TCPOutPacketPool pool, IObject obj, int roleID, int mapCode, int copyMapID, int toMapX, int toMapY, int toMapDirection, List<Object> objsList)
        {
            int nID = (int)TCPGameServerCmds.CMD_SPR_CHANGEPOS;

            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, toMapX, toMapY, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (objsList == null) return true;

            string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, toMapX, toMapY, toMapDirection);
            SendToClients(sl, pool, null, objsList, strcmd, nID);
            return true;
        }

        #endregion Chuyển cảnh

        #region Thông báo vật phẩm

        /// <summary>
        /// Thông báo add vật phẩm về CLIENT
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfAddGoods(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int id, int goodsID, int forgeLevel, int goodsNum, int binding, int site, int newHint, string newEndTime,
            int strong, int bagIndex = 0, int isusing = -1, String Props = "", int Series = -1, Dictionary<ItemPramenter, string> OtherParams = null)
        {
            newEndTime = newEndTime.Replace(":", "$");

            AddGoodsData addGoodsData = new AddGoodsData()
            {
                RoleID = client.RoleID,
                ID = id,
                GoodsID = goodsID,
                ForgeLevel = forgeLevel,
                GoodsNum = goodsNum,
                Binding = binding,
                Site = site,
                NewHint = newHint,
                NewEndTime = newEndTime,
                Strong = strong,
                BagIndex = bagIndex,
                Using = isusing,
                OtherParams = OtherParams,
                Props = Props,
                Series = Series,
            };

            byte[] bytes = DataHelper.ObjectToBytes<AddGoodsData>(addGoodsData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_ADD_GOODS);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Move GoodData
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMoveGoods(SocketListener sl, TCPOutPacketPool pool, KPlayer client, GoodsData gd, int moveType)
        {
            if (0 == moveType)
            {
                string strcmd = string.Format("{0}:{1}", client.RoleID, gd.Id);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_MOVEGOODSDATA);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                }
            }
            else
            {
                GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, gd.Id, gd.GoodsID, gd.Forge_level, gd.GCount, gd.Binding, gd.Site, 0, gd.Endtime, gd.Strong, gd.BagIndex, gd.Using, gd.Props, gd.Series, gd.OtherParams);
            }
        }

        #endregion Thông báo vật phẩm

        #region MONEY CHANGE

        /// <summary>
        /// Thông báo số bạc và bạc khóa thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfMoneyChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.Money, client.BoundMoney);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_MONEYCHANGE);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Trừ đồng khóa của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subBoundToken"></param>
        /// <param name="strFrom"></param>
        /// <returns></returns>
        public bool SubBoundToken(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subBoundToken, string strFrom, bool NeedWriterLogs = true)
        {
            int oldBoundToken = client.BoundToken;

            //Lock lại tiền trước khi thao tác với nó
            lock (client.BoundTokenMutex)
            {
                if (client.BoundToken < subBoundToken)
                {
                    return false; //Nếu số tiền không đủ thì chim cút
                }

                // Tiền trước đó
                int oldValue = client.BoundToken;
                // Sau khi trừ còn bao nhiêu
                client.BoundToken -= subBoundToken;

                //CMD SEND TO DB
                string strcmd = string.Format("{0}:{1}", client.RoleID, -subBoundToken);
                string[] dbFields = null;

                try
                {
                    dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBoundToken_CMD, strcmd, client.ServerId);
                }
                catch (Exception ex)
                {
                    DataHelper.WriteExceptionLogEx(ex, string.Format("Nếu toạch thì return FALSE"));

                    return false;
                }

                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    client.BoundToken = oldValue;
                    return false;
                }

                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    client.BoundToken = oldValue;
                    return false;
                }

                client.BoundToken = Convert.ToInt32(dbFields[1]);

                if (0 != subBoundToken)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "SUB";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldValue + "";
                        string OtherPram_2 = client.BoundToken + "";
                        string OtherPram_3 = MoneyType.DongKhoa + "";
                        string OtherPram_4 = "-" + subBoundToken;

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.DongKhoa, -subBoundToken, client.BoundToken, strFrom);
                }
            }

            // THỰC HIỆN NOTIFY TỚI NGƯỜI CHƠI
            GameManager.ClientMgr.NotifySelfTokenChange(sl, pool, client);

            return true;
        }

        /// <summary>
        ///  Sub Guild Money - Trừ tiền bang hội
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subBoundToken"></param>
        /// <param name="strFrom"></param>
        /// <returns></returns>
        public bool SubGuildMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subBoundToken, string strFrom, bool NeedWriterLogs = false)
        {
            if (client.RoleGuildMoney < subBoundToken)
            {
                return false; //Nếu số tiền không đủ thì chim cút
            }
            // Nếu ko có bang thì cũng cho toạch luôn
            if (client.GuildID <= 0)
            {
                return false;
            }
            // Tiền trước đó
            int oldValue = client.RoleGuildMoney;
            // Sau khi trừ còn bao nhiêu
            client.RoleGuildMoney -= subBoundToken;

            //CMD SEND TO DB
            string strcmd = string.Format("{0}:{1}", client.RoleID, -subBoundToken);
            string[] dbFields = null;

            try
            {
                dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBANGGONG_CMD, strcmd, client.ServerId);
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("Nếu toạch thì return FALSE"));

                return false;
            }

            // nếu lỗi DB thì set lại tiền = tiền trước đó
            if (null == dbFields)
            {
                client.RoleGuildMoney = oldValue;
                return false;
            }

            if (dbFields.Length != 2)
            {
                client.RoleGuildMoney = oldValue;
                return false;
            }

            if (Convert.ToInt32(dbFields[1]) < 0)
            {
                if (Convert.ToInt32(dbFields[1]) == -3)
                {
                    PlayerManager.ShowNotification(client, "Số lượng muốn rút đã vượt quá số % lợi tức thiết lập của bang chủ");
                }

                client.RoleGuildMoney = oldValue;
                return false;
            }

            client.RoleGuildMoney = Convert.ToInt32(dbFields[1]);

            if (0 != subBoundToken)
            {
                if (NeedWriterLogs)
                {
                    int RoleID = client.RoleID;
                    string AccountName = client.strUserID;
                    string RecoreType = "MONEY";
                    string RecoreDesc = strFrom;
                    string Source = "SYSTEM";
                    string Taget = client.RoleName;
                    string OptType = "SUB";

                    int ZONEID = client.ZoneID;

                    string OtherPram_1 = oldValue + "";
                    string OtherPram_2 = client.RoleGuildMoney + "";
                    string OtherPram_3 = MoneyType.GuildMoney + "";
                    string OtherPram_4 = "-" + subBoundToken;

                    //Thực hiện việc ghi LOGS vào DB
                    GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                }
            }

            return true;
        }

        /// <returns></returns>
        public bool AddGuildMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int AddGuildMoney, string strFrom, bool NeedWriterLogs = false)
        {
            int oldGuildMoney = client.RoleGuildMoney;

            if (oldGuildMoney >= Global.Max_Role_Money)
            {
                return false;
            }

            if (oldGuildMoney + AddGuildMoney > Global.Max_Role_Money)
            {
                return false;
            }

            if (0 == AddGuildMoney)
            {
                return true;
            }

            string strcmd = string.Format("{0}:{1}", client.RoleID, AddGuildMoney);

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBANGGONG_CMD, strcmd, client.ServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 2)
            {
                return false;
            }
            if (Convert.ToInt32(dbFields[1]) < 0)
            {
                return false;
            }

            client.RoleGuildMoney = Convert.ToInt32(dbFields[1]);

            if (0 != AddGuildMoney)
            {
                if (NeedWriterLogs)
                {
                    int RoleID = client.RoleID;
                    string AccountName = client.strUserID;
                    string RecoreType = "MONEY";
                    string RecoreDesc = strFrom;
                    string Source = "SYSTEM";
                    string Taget = client.RoleName;
                    string OptType = "" + AddGuildMoney;

                    int ZONEID = client.ZoneID;

                    string OtherPram_1 = oldGuildMoney + "";
                    string OtherPram_2 = client.RoleGuildMoney + "";
                    string OtherPram_3 = MoneyType.GuildMoney + "";
                    string OtherPram_4 = "NONE";

                    //Thực hiện việc ghi LOGS vào DB
                    GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                }

                // EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.GuildMoney, AddGuildMoney, client.BoundToken, strFrom);
            }

            PlayerManager.ShowNotification(client, KTGlobal.CreateStringByColor("Tài sản bang hội cá nhân gia tăng :" + AddGuildMoney + "", ColorType.Blue));

            // GameManager.ClientMgr.NotifySelfTokenChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// Thêm đồng khóa cho người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="addBoundToken"></param>
        /// <param name="strFrom"></param>
        /// <returns></returns>
        public bool AddBoundToken(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addBoundToken, string strFrom, bool NeedWriterLogs = true)
        {
            int oldBoundToken = client.BoundToken;
            lock (client.BoundTokenMutex)
            {
                if (oldBoundToken >= Global.Max_Role_BoundToken)
                {
                    return false;
                }

                if (oldBoundToken + addBoundToken > Global.Max_Role_BoundToken)
                {
                    return false;
                }

                if (0 == addBoundToken)
                {
                    return true;
                }

                string strcmd = string.Format("{0}:{1}", client.RoleID, addBoundToken);
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBoundToken_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                client.BoundToken = Convert.ToInt32(dbFields[1]);

                if (0 != addBoundToken)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "" + addBoundToken;

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldBoundToken + "";
                        string OtherPram_2 = client.BoundToken + "";
                        string OtherPram_3 = MoneyType.DongKhoa + "";
                        string OtherPram_4 = "NONE";

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.DongKhoa, addBoundToken, client.BoundToken, strFrom);
                }
            }

            GameManager.ClientMgr.NotifySelfTokenChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// Thêm bạc cho người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addMoney, string strFrom, bool writeToDB = true, bool NeedWriterLogs = true, int TradeSession = -1)
        {
            lock (client.GetMoneyLock)
            {
                int oldMoney = client.Money;
                /// Nếu số bạc đang có vượt quá ngưỡng thì tự thêm vào kho
                if (oldMoney >= Global.Max_Role_Money)
                {
                    return AddUserStoreMoney(sl, tcpClientPool, pool, client, addMoney, strFrom);
                }

                /// Nếu số bạc mang theo vượt quá ngưỡng thì tự thêm vào kho
                if (oldMoney + addMoney > Global.Max_Role_BoundToken)
                {
                    int newValue = Global.GMax(0, oldMoney + addMoney - Global.Max_Role_BoundToken);
                    addMoney = Global.GMax(0, Global.Max_Role_Money - oldMoney);
                    /// Thêm vào kho
                    AddUserStoreMoney(sl, tcpClientPool, pool, client, newValue, strFrom);
                }

                if (0 == addMoney)
                {
                    return true;
                }

                if (writeToDB)
                {
                    string strcmd = string.Format("{0}:{1}", client.RoleID, client.Money + addMoney);

                    GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD,
                        strcmd,
                        null, client.ServerId);

                    long nowTicks = TimeUtil.NOW();
                    Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD, nowTicks);
                }

                client.Money = client.Money + addMoney;
                GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);
                if (0 != addMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "ADD";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldMoney + "";
                        string OtherPram_2 = client.Money + "";
                        string OtherPram_3 = MoneyType.Bac + "";
                        string OtherPram_4 = "" + addMoney;
                        if (TradeSession != -1)
                        {
                            OtherPram_4 = TradeSession + "";
                        }

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.Bac, addMoney, client.Money, strFrom);
                }
            }

            return true;
        }

        /// <summary>
        /// Thêm bạc cho người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addMoney"></param>
        /// <param name="strFrom"></param>
        /// <param name="writeToDB"></param>
        /// <returns></returns>
        public bool AddMoney(KPlayer client, int addMoney, string strFrom, bool writeToDB = true)
        {
            return AddMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, addMoney, strFrom, writeToDB);
        }

        /// <summary>
        /// Trừ bạc của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, string strFrom, bool NeedWriterLogs = true, int TradeSession = -1)
        {
            lock (client.GetMoneyLock)
            {
                int oldMoney = client.Money;

                if (client.Money - subMoney < 0)
                {
                    return false;
                }

                string strcmd = string.Format("{0}:{1}", client.RoleID, client.Money - subMoney);
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD,
                    strcmd,
                    null, client.ServerId);

                long nowTicks = TimeUtil.NOW();
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD, nowTicks);

                client.Money = client.Money - subMoney;
                GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);
                if (0 != subMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "SUB";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldMoney + "";
                        string OtherPram_2 = client.Money + "";
                        string OtherPram_3 = MoneyType.Bac + "";

                        string OtherPram_4 = "-" + subMoney;
                        if (TradeSession != -1)
                        {
                            OtherPram_4 = TradeSession + "";
                        }

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.Bac, -subMoney, client.Money, strFrom);
                }
            }
            return true;
        }

        /// <summary>
        /// Thông báo giá trị đồng, đồng khóa thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfTokenChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.Token, client.BoundToken);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_TokenCHANGE);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Thông báo số bạc, bạc khóa thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfTokenChange(KPlayer client)
        {
            NotifySelfTokenChange(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);
        }

        /// <summary>
        /// Thêm đồng cho người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddToken(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addMoney, string strFrom, ActivityTypes result = ActivityTypes.None, bool NeedWriterLogs = true)
        {
            lock (client.TokenMutex)
            {
                int oldMoney = client.Token;

                string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, addMoney, (int)result, strFrom);
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEToken_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    return false;
                }

                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                client.Token = Convert.ToInt32(dbFields[1]);
                int nTotalMoney = Convert.ToInt32(dbFields[2]);

                if (0 != addMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "ADD";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldMoney + "";
                        string OtherPram_2 = client.Token + "";
                        string OtherPram_3 = MoneyType.Dong + "";
                        string OtherPram_4 = addMoney + "";

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.Dong, addMoney, client.Money, strFrom);
                }
            }

            GameManager.ClientMgr.NotifySelfTokenChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// Trừ đồng của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubToken(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, string strFrom, bool bIsAddVipExp = true, int type = 1, bool isAddFund = true, bool NeedWriterLogs = true)
        {
            lock (client.TokenMutex)
            {
                subMoney = Math.Abs(subMoney);
                if (client.Token < subMoney)
                {
                    return false;
                }

                int oldValue = client.Token;
                client.Token -= subMoney;

                string strcmd = string.Format("{0}:{1}", client.RoleID, -subMoney);
                string[] dbFields = null;

                try
                {
                    dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEToken_CMD, strcmd, client.ServerId);
                }
                catch (Exception ex)
                {
                    DataHelper.WriteExceptionLogEx(ex, string.Format("CMD_DB_UPDATEToken_CMD Faild"));

                    return false;
                }

                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    client.Token = oldValue;
                    return false;
                }

                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    client.Token = oldValue;
                    return false;
                }

                client.Token = Convert.ToInt32(dbFields[1]);

                /// ghi lại tiêu phí
                Global.SaveConsumeLog(client, subMoney, type);

                if (0 != subMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "SUB";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldValue + "";
                        string OtherPram_2 = client.Token + "";
                        string OtherPram_3 = MoneyType.Dong + "";
                        string OtherPram_4 = "-" + subMoney;

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.Dong, -subMoney, client.Money, strFrom);
                }
            }

            GameManager.ClientMgr.NotifySelfTokenChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// Thêm Bạc khóa cho người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addBoundMoney"></param>
        /// <returns></returns>
        public bool AddUserBoundMoney(KPlayer client, int addBoundMoney, string strFrom)
        {
            return AddUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, addBoundMoney, strFrom);
        }

        /// <summary>
        /// Thêm bạc khóa cho người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserBoundMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addBoundMoney, string strFrom = "", bool NeedWriterLogs = false)
        {
            int oldBoundMoney = client.BoundMoney;

            lock (client.BoundMoneyMutex)
            {
                string strcmd = string.Format("{0}:{1}", client.RoleID, addBoundMoney);
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERBoundMoney_CMD, strcmd, client.ServerId);
                if (null == dbFields)
                    return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                client.BoundMoney = Convert.ToInt32(dbFields[1]);

                if (0 != addBoundMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "ADD";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldBoundMoney + "";
                        string OtherPram_2 = client.BoundMoney + "";
                        string OtherPram_3 = MoneyType.BacKhoa + "";
                        string OtherPram_4 = addBoundMoney + "";

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.BacKhoa, addBoundMoney, client.BoundMoney, strFrom);
                }
            }

            GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);

            Global.AddRoleBoundMoneyEvent(client, oldBoundMoney);

            return true;
        }

        /// <summary>
        /// Trừ bạc khóa của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubUserBoundMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subBoundMoney, string strFrom = "", bool NeedWriterLogs = false)
        {
            int oldBoundMoney = client.BoundMoney;

            lock (client.BoundMoneyMutex)
            {
                if (client.BoundMoney < subBoundMoney)
                {
                    return false;
                }

                string strcmd = string.Format("{0}:{1}", client.RoleID, -subBoundMoney);
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERBoundMoney_CMD, strcmd, client.ServerId);
                if (null == dbFields)
                    return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                client.BoundMoney = Convert.ToInt32(dbFields[1]);

                if (0 != subBoundMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "SUB";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldBoundMoney + "";
                        string OtherPram_2 = client.BoundMoney + "";
                        string OtherPram_3 = MoneyType.BacKhoa + "";
                        string OtherPram_4 = "-" + subBoundMoney;

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.BacKhoa, -subBoundMoney, client.BoundMoney, strFrom);
                }
            }

            GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);

            Global.AddRoleBoundMoneyEvent(client, oldBoundMoney);

            return true;
        }

        #endregion MONEY CHANGE

        #region OTHER MONEY - NEED TO CHECK OR DELETE

        /// <summary>
        /// Kiểm tra tổng nạp của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public int QueryTotaoChongZhiMoney(KPlayer client)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            int zoneID = client.ZoneID;

            return QueryTotaoChongZhiMoney(userID, zoneID, client.ServerId);
        }

        /// <summary>
        /// Kiểm tra nạp
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public int QueryTotaoChongZhiMoney(string userID, int zoneID, int ServerId)
        {
            //Lấy thông tin tổng nạp từ DB SERVER
            string strcmd = string.Format("{0}:{1}", userID, zoneID);
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYCHONGZHIMONEY, strcmd, ServerId);
            if (null == dbFields)
                return 0;
            if (dbFields.Length != 1)
            {
                return 0;
            }

            return Global.SafeConvertToInt32(dbFields[0]);
        }

        /// <summary>
        /// Kiểm tra tổng nạp hôm nay của người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public int QueryTotaoChongZhiMoneyToday(KPlayer client)
        {
            string userID = GameManager.OnlineUserSession.FindUserID(client.ClientSocket);
            int zoneID = client.ZoneID;

            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}", userID, zoneID);
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYTODAYCHONGZHIMONEY, strcmd, client.ServerId);
            if (null == dbFields)
                return 0;
            if (dbFields.Length != 1)
            {
                return 0;
            }

            return Global.SafeConvertToInt32(dbFields[0]);
        }

        #endregion OTHER MONEY - NEED TO CHECK OR DELETE

        #region Thực hiện move đồ cho 1 thằng khác

        /// <summary>
        /// Hàm chuyển vật phẩm từ thằng này cho thằng khác
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="gd"></param>
        /// <param name="toClient"></param>
        /// <param name="bAddToTarget">Có thưc hiện update lại vị trí cho thằng nhận không, Nếu là add vào túi đồ thì cần,Nếu add vào kho thì kệ mẹ nó</param>
        /// <returns></returns>
        public bool MoveGoodsDataToOtherRole(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, GoodsData gd,
                                             KPlayer fromClient, KPlayer toClient, bool bAddToTarget = true, string FROM = "", int SesionI = -1)
        {
            //Thao tác với DB SV thực hiện xáo vật phẩm của thằng đầu tiên và move cho thằng thứ 2

            string[] dbFields = null;
            string strcmd = string.Format("{0}:{1}:{2}", toClient.RoleID, fromClient.RoleID, gd.Id);
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_MOVEGOODS_CMD, strcmd, out dbFields, GameManager.LocalServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                return false;
            }

            if (dbFields.Length < 4 || Convert.ToInt32(dbFields[3]) < 0)
            {
                return false;
            }

            {
                int RoleID = fromClient.RoleID;
                string AccountName = fromClient.strUserID;
                string RecoreType = FROM;

                string RecoreDesc = "Delete item [" + gd.GoodsID + "][" + gd.Id + "] from [" + fromClient.RoleID + "] move to [" + toClient.RoleID + "]";

                string Source = fromClient.RoleID + "";

                string Taget = toClient.RoleID + "";

                string OptType = "SUB";

                int ZONEID = fromClient.ZoneID;

                string OtherPram_1 = gd.Id + "";
                string OtherPram_2 = gd.GoodsID + "";
                string OtherPram_3 = gd.GCount + "";
                string OtherPram_4 = SesionI + "";

                GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, fromClient.ServerId);
            }

            if (bAddToTarget)
            {
                //Update lại vị trí đồ cho thằng thứ 2 nhận đồ
                string[] dbFields2 = null;
                gd.BagIndex = KTGlobal.GetIdleSlotOfBagGoods(toClient);

                Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

                TotalUpdate.Add(UPDATEITEM.ROLEID, toClient.RoleID);
                TotalUpdate.Add(UPDATEITEM.ITEMDBID, gd.Id);

                TotalUpdate.Add(UPDATEITEM.BAGINDEX, gd.BagIndex);

                string ScriptUpdateBuild = KTGlobal.ItemUpdateScriptBuild(TotalUpdate);

                dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, ScriptUpdateBuild, out dbFields, GameManager.LocalServerId);

                if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
                {
                    return false;
                }
                else if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
                {
                    Global.AddGoodsData(toClient, gd);

                    {
                        int RoleID = toClient.RoleID;
                        string AccountName = toClient.strUserID;
                        string RecoreType = FROM;

                        string RecoreDesc = "Revice Item [" + gd.Id + "] from [" + fromClient.RoleID + "]";

                        string Source = fromClient.RoleID + "";

                        string Taget = toClient.RoleID + "";

                        string OptType = "ADD";

                        int ZONEID = fromClient.ZoneID;

                        string OtherPram_1 = gd.Id + "";
                        string OtherPram_2 = gd.GoodsID + "";
                        string OtherPram_3 = gd.GCount + "";
                        string OtherPram_4 = SesionI + "";

                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, fromClient.ServerId);
                    }

                    // Thực hiện task mua cái gì đó
                    ProcessTask.ProseccTaskBeforeDoTask(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient);

                    // NOTIFY ADD GOOLDS VỀ CLIENT
                    GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient,
                        gd.Id, gd.GoodsID, gd.Forge_level, gd.GCount, gd.Binding, gd.Site, 0, gd.Endtime, gd.Strong, gd.BagIndex, gd.Using, gd.Props, gd.Series, gd.OtherParams);

                    return true;
                }
            }

            return true;
        }

        #endregion Thực hiện move đồ cho 1 thằng khác

        #region Gian hàng

        /// <summary>
        /// Thông báo cho ngươi chời khác mở cửa hàng
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGoodsStallCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int stallType)
        {
            string strcmd = string.Format("{0}:{1}:{2}", status, client.RoleID, stallType);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSSTALL);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        public void NotifyGoodsStallData(SocketListener sl, TCPOutPacketPool pool, KPlayer client, StallData sd)
        {
            byte[] bytesData = null;

            lock (sd)
            {
                bytesData = DataHelper.ObjectToBytes<StallData>(sd);
            }

            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = pool.Pop();
            tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_STALLDATA;
            tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Thông báo cho bọn xung quanh biết thằng này bày bán
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteStartStall(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (null == client.StallDataItem)
            {
                return;
            }

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StallDataItem.StallName);

            //Gửi về cho thằng khác biết là thằng này đang bày bán
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_STALLNAME);
        }

        #endregion Gian hàng

        #region Mua hàng trên chợ

        /// <summary>
        /// MMua trên chợ
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteMarketBuy(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer otherClient, int result, int buyType, int goodsDbID, int goodsID, int nID = (int)TCPGameServerCmds.CMD_SPR_MARKETBUYGOODS)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", result, buyType, client.RoleID, null != otherClient ? otherClient.RoleID : -1, null != otherClient ? Global.FormatRoleName(otherClient, otherClient.RoleName) : "", goodsDbID, goodsID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        #endregion Mua hàng trên chợ

        #region Cập nhật thời gian online

        /// <summary>
        /// Cập nhật thời gina online
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addTicks"></param>
        private void UpdateRoleOnlineTimes(KPlayer client, long addTicks)
        {
            if (client.FirstPlayStart)
            {
                return;
            }

            // Bắt buộc ngắt kết nối mạng trong trường hợp cần thiết
            if (client.ForceDisconnect)
            {
                client.ForceDisconnect = false;
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                    client, StringUtil.substitute("Hệ thống bắt buộc bạn phải đăng xuất ngay lập tức."),
                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.ForceShenFenZheng);
                return;
            }

            int oldTotalOnlineHours = (int)(client.TotalOnlineSecs / 3600);

            client.TotalOnlineSecs += Math.Max(0, (int)(addTicks / 1000));

            // Thực hiện việc mở túi đồ
            if (client.BagNum < Global.MaxBagGridNum)
            {
                client.OpenGridTime += Math.Max(0, (int)(addTicks / 1000));
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.OpenGridTick, client.OpenGridTime, false);
            }

            if (client.MyPortableBagData.ExtGridNum < Global.MaxPortableGridNum)
            {
                client.OpenPortableGridTime += Math.Max(0, (int)(addTicks / 1000));
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.OpenPortableGridTick, client.OpenPortableGridTime, false);
            }

            int newTotalOnlineHours = (int)(client.TotalOnlineSecs / 3600);

            if (oldTotalOnlineHours != newTotalOnlineHours)
            {
                //Tích lũy quà tặng online
                HuodongCachingMgr.ProcessKaiFuGiftAward(client);
            }

            int monthID = TimeUtil.NowDateTime().Month;
            if (client.MyHuodongData.CurMID == monthID.ToString())
            {
                client.MyHuodongData.CurMTime += Math.Max(0, (int)(addTicks / 1000));
            }
            else
            {
                client.MyHuodongData.OnlineGiftState = 0;
                client.MyHuodongData.CurMID = monthID.ToString();
                client.MyHuodongData.LastMTime = client.MyHuodongData.CurMTime;
                client.MyHuodongData.CurMTime = 0;
                client.MyHuodongData.CurMTime += Math.Max(0, (int)(addTicks / 1000));
            }
        }

        #endregion Cập nhật thời gian online

        #region Thông báo các thuộc tính cá nhân

        /// <summary>
        /// Thông báo kinh nghiệm hoặc cấp độ thay đổi về Client
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfExperience(SocketListener sl, TCPOutPacketPool pool, KPlayer client, long newExperience)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, client.m_Level, client.m_Experience, KPlayerSetting.GetNeedExpUpExp(client.m_Level));

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_EXPCHANGE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// Xử lý sự kiện tăng EXP cho người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="experience"></param>
        public void ProcessRoleExperience(KPlayer client, long experience, bool enableFilter = true, bool writeToDB = true, bool checkDead = false, string strFrom = "none")
        {
            // Nếu đang chết hoặc máu <0 thì thôi
            if (checkDead && client.m_CurrentLife <= 0)
                return;
            //Nếu exp < 0 thì bỏ qua
            if (experience <= 0) return;

            //// Nếu mà online quá số giờ chơi thì thôi
            //if (enableFilter)
            //{
            //    experience = Global.FilterValue(client, experience);
            //}

            // Nếu số EXP add vào lớn hơn 0
            if (experience > 0)
            {
                int oldLevel = client.m_Level;

                //Hàm xử lý các sự kiện khác kèm theo khi EXP tăng
                bool ret = Global.EarnExperience(client, experience);
                if (!ret)
                {
                    return;
                }

                if (writeToDB || (oldLevel != client.m_Level)) //Thực hiện update vào DB
                {
                    GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL,
                        string.Format("{0}:{1}:{2}:{3}", client.RoleID, client.m_Level, client.m_Experience, client.Prestige),
                        null, client.ServerId);

                    long nowTicks = TimeUtil.NOW();
                    Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL, nowTicks);
                }

                // NẾU 2 LEVE KHÁC NHAU
                if (oldLevel != client.m_Level)
                {
                    /// Gọi sự kiện thay đổi cấp độ của nhân vật
                    client.OnLevelChanged();

                    //// RERESSH LẠI ĐỒ ĐẠC NẾU YÊU CẦU CẤP ĐỘ
                    //GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);

                    // NOTITFY SỐ LẦN CHUYỂN SINH
                    GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, true, true);

                    //Ghi lại logs tăng level cho nhân vật
                    Global.AddRoleUpgradeEvent(client, oldLevel);

                    //Nhận quà thăng cấp
                    HuodongCachingMgr.ProcessKaiFuGiftAward(client);

                    ////// MỞ KHÓA TRADE NẾU ĐỦ CÁP ĐỘ
                    //TradeBlackManager.Instance().UpdateObjectExtData(client);
                }

                //UPDATE TỔNG EXP KIẾM ĐƯỢC TRONG NGÀY
                GameManager.ClientMgr.UpdateRoleDailyData_Exp(client, experience);

                // NOTIFY EXPX VỀ CHO NGƯỜI CHƠI
                GameManager.ClientMgr.NotifySelfExperience(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, experience);
            }
        }

        /// <summary>
        /// Thiết lập cấp độ cho người chơi
        /// </summary>
        public void SetRoleLevel(KPlayer player, int level)
        {
            int oldLevel = player.m_Level;
            if (level == player.m_Level)
            {
                return;
            }

            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL,
                string.Format("{0}:{1}:{2}:{3}", player.RoleID, level, player.m_Experience, player.Prestige),
                null, player.ServerId);

            /// Thiết lập cấp độ cho nhân vật
            player.m_Level = level;

            /// Gọi sự kiện thay đổi cấp độ của nhân vật
            player.OnLevelChanged();

            //// RERESSH LẠI ĐỒ ĐẠC NẾU YÊU CẦU CẤP ĐỘ
            //GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player);

            // NOTITFY SỐ LẦN CHUYỂN SINH
            GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, true, true);

            //Ghi lại logs thay đổi level cho nhân vật
            Global.AddRoleUpgradeEvent(player, oldLevel);

            //// MỞ KHÓA TRADE NẾU ĐỦ CÁP ĐỘ
            //TradeBlackManager.Instance().UpdateObjectExtData(player);

            // NOTIFY EXPX VỀ CHO NGƯỜI CHƠI
            GameManager.ClientMgr.NotifySelfExperience(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, 0);
        }

        /// <summary>
        /// Add cho tất cả người chơi online EXP
        /// </summary>
        /// <param name="addPercent"></param>
        public void AddOnlieRoleExperience(KPlayer client, int addPercent)
        {
            long needExperience = 0;
            if (client.m_Level < KPlayerSetting.GetMaxLevel())
            {
                needExperience = KPlayerSetting.GetNeedExpUpExp(client.m_Level);
            }

            if (needExperience <= 0)
            {
                return;
            }

            int addExperience = (int)(needExperience * ((double)addPercent / 100.0));

            //Add Exp Vào nhân vật ProseccDB
            ProcessRoleExperience(client, addExperience, false, false);
        }

        /// <summary>
        /// Add cho toàn bộ người chơi 1 lượng EXO nào đó
        /// </summary>
        /// <param name="addPercent"></param>
        public void AddAllOnlieRoleExperience(int addPercent)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.ClosingClientStep > 0)
                {
                    continue;
                }

                //Số % EXP
                AddOnlieRoleExperience(client, addPercent);
            }
        }

        #endregion Thông báo các thuộc tính cá nhân

        #region Thông báo liên quan tới nhiệm vụ

        /// <summary>
        /// Thông báo nhiệm vụ hàng ngày
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDailyTaskData(KPlayer client)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<DailyTaskData>>(client.MyDailyTaskDataList, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_DAILYTASKDATA);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        #endregion Thông báo liên quan tới nhiệm vụ

        #region Thương khố

        /// <summary>
        /// Thông báo thông tin thương khố bản thân
        /// </summary>
        /// <param name="client"></param>
        public void NotifyPortableBagData(KPlayer client)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<PortableBagData>(client.MyPortableBagData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_PORTABLEBAGDATA);
            Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Thương khố

        #region 系统送礼

        /// <summary>
        /// 向客户端发送活动数据
        /// </summary>
        /// <param name="client"></param>
        public void NotifyHuodongData(KPlayer client)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<HuodongData>(client.MyHuodongData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETHUODONGDATA);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// 向客户端发送领取升级有礼完毕
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGetLevelUpGiftData(KPlayer client, int newLevel)
        {
            GameManager.ClientMgr.SendToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, String.Format("{0}:{1}", client.RoleID, newLevel), (int)TCPGameServerCmds.CMD_SPR_GETUPLEVELGIFTOK);
        }

        /// <summary>
        /// 通知所有在线用户活动ID发生了改变
        /// </summary>
        /// <param name="bigAwardID"></param>
        /// <param name="songLiID"></param>
        public void NotifyAllChangeHuoDongID(int bigAwardID, int songLiID)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, bigAwardID, songLiID);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGHUODONGID);
                if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }
            }
        }

        #endregion 系统送礼

        #region Dữ liệu hàng ngày

        /// <summary>
        /// Ghi lại thông tin kinh nghiệm nhận được trong ngày
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_Exp(KPlayer client, long newExperience)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.ExpDayID)
            {
                client.MyRoleDailyData.TodayExp += (int)newExperience;
            }
            else
            {
                client.MyRoleDailyData.ExpDayID = dayID;
                client.MyRoleDailyData.TodayExp = (int)newExperience;
            }
        }

        #endregion Dữ liệu hàng ngày

        #region 银两折半优惠通知

        /// <summary>
        /// 通知所有在线用户银两折半优惠发生了改变
        /// </summary>
        /// <param name="bigAwardID"></param>
        /// <param name="songLiID"></param>
        public void NotifyAllChangeHalfBoundTokenPeriod(int halfBoundTokenPeriod)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                string strcmd = string.Format("{0}:{1}", client.RoleID, halfBoundTokenPeriod);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGHALFBoundTokenPERIOD);
                if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }
            }
        }

        #endregion 银两折半优惠通知

        #region Tương quan Email

        /// <summary>
        /// Kiểm tra xem có email không
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sendToClient"></param>
        /// <returns></returns>
        public static bool CheckEmailCount(KPlayer client)
        {
            bool result;
            string cmd = string.Format("{0}:{1}:{2}", client.RoleID, 1, 1);
            int emailCount = Global.SendToDB<int, string>((int)TCPGameServerCmds.CMD_SPR_GETUSERMAILCOUNT, cmd, client.ServerId);
            if (emailCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Tương quan Email

        #region NPC

        /// <summary>
        /// Gửi gói tin thông báo có NPC ở gần người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewNPC(SocketListener sl, TCPOutPacketPool pool, KPlayer client, NPC npc)
        {
            if (null == npc)
            {
                return;
            }

            NPCRole npcRole = new NPCRole()
            {
                NPCID = npc.NPCID,
                ResID = npc.ResID,
                Name = npc.Name,
                Title = npc.Title,
                MapCode = npc.CurrentMapCode,
                PosX = (int)npc.CurrentPos.X,
                PosY = (int)npc.CurrentPos.Y,
                Dir = (int)npc.CurrentDir,
                VisibleOnMinimap = npc.VisibleOnMinimap,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<NPCRole>(npcRole);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_NEWNPC);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// Gửi gói tin thông báo có NPC xung quanh người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewNPCBy9Grid(SocketListener sl, TCPOutPacketPool pool, NPC npc)
        {
            if (null == npc)
            {
                return;
            }
#if TestGrid2
            List<Object> objsList = Global.GetAll9GridGameClient(npc);
#else
            List<Object> objsList = Global.GetAll9GridObjects(npc);
#endif
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                NotifyMySelfNewNPC(sl, pool, objsList[i] as KPlayer, npc);
            }
        }

        /// <summary>
        /// NPC移除通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelNPC(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, int npcID)
        {
            string strcmd = string.Format("{0}:{1}", npcID, mapCode);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELNPC);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// Thông báo xóa NPC
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelNPC(SocketListener sl, TCPOutPacketPool pool, KPlayer client, NPC npc)
        {
            NotifyMySelfDelNPC(sl, pool, client, npc.MapCode, npc.NPCID);
        }

        /// <summary>
        /// 通知指定npc附近的角色npc删除
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelNPCBy9Grid(SocketListener sl, TCPOutPacketPool pool, NPC npc)
        {
#if TestGrid2
            List<Object> objsList = Global.GetAll9GridGameClient(npc);
#else
            List<Object> objsList = Global.GetAll9GridObjects(npc);
#endif
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                NotifyMySelfDelNPC(sl, pool, objsList[i] as KPlayer, npc.MapCode, npc.NPCID);
            }
        }

        #endregion NPC

        #region Trap

        /// <summary>
        /// Gửi gói tin thông báo có bẫy ở gần người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfTrap(SocketListener sl, TCPOutPacketPool pool, KPlayer client, Trap trap)
        {
            if (null == trap)
            {
                return;
            }

            TrapRole trapRole = new TrapRole()
            {
                ID = trap.TrapID,
                ResID = trap.ResID,
                PosX = (int)trap.CurrentPos.X,
                PosY = (int)trap.CurrentPos.Y,
                LifeTime = trap.LifeTime,
                CasterID = trap.Owner.RoleID,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<TrapRole>(trapRole);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_KT_SPR_NEWTRAP);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin thông báo xóa bẫy tới người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="mapCode"></param>
        /// <param name="trapID"></param>
        public void NotifyMySelfDelTrap(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, long trapID)
        {
            string strcmd = string.Format("{0}:{1}", trapID, mapCode);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_KT_SPR_DELTRAP);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Trap

        #region Grow Point

        /// <summary>
        /// Gửi gói tin thông báo có điểm thu thập ở gần người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfGrowPoint(SocketListener sl, TCPOutPacketPool pool, KPlayer client, GrowPoint growPoint)
        {
            if (null == growPoint)
            {
                return;
            }

            GrowPointObject growPointObject = new GrowPointObject()
            {
                ID = growPoint.ID,
                Name = growPoint.Data.Name,
                ResID = growPoint.Data.ResID,
                PosX = (int)growPoint.CurrentPos.X,
                PosY = (int)growPoint.CurrentPos.Y,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<GrowPointObject>(growPointObject);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_KT_G2C_NEW_GROWPOINT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin thông báo xóa điểm thu thập tới người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="mapCode"></param>
        /// <param name="growPointID"></param>
        public void NotifyMySelfDelGrowPoint(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, int growPointID)
        {
            string strcmd = string.Format("{0}:{1}", growPointID, mapCode);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_KT_G2C_DEL_GROWPOINT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Grow Point

        #region Đối tượng động

        /// <summary>
        /// Gửi gói tin thông báo có khu vực động ở gần người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDynamicArea(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KDynamicArea dynArea)
        {
            if (null == dynArea)
            {
                return;
            }

            DynamicArea area = new DynamicArea()
            {
                ID = dynArea.ID,
                Name = dynArea.Name,
                ResID = dynArea.ResID,
                PosX = (int)dynArea.CurrentPos.X,
                PosY = (int)dynArea.CurrentPos.Y,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<DynamicArea>(area);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_KT_G2C_NEW_DYNAMICAREA);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin thông báo xóa khu vực động tới người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="mapCode"></param>
        /// <param name="areaID"></param>
        public void NotifyMySelfDelDynamicArea(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, int areaID)
        {
            string strcmd = string.Format("{0}:{1}", areaID, mapCode);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_KT_G2C_DEL_DYNAMICAREA);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Đối tượng động

        #region Đối BOT

        /// <summary>
        /// Gửi gói tin thông báo có BOT ở gần người chơi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfBot(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KTBot bot)
        {
            if (null == bot)
            {
                return;
            }

            RoleDataMini rd = new RoleDataMini()
            {
                RoleID = bot.RoleID,
                RoleName = bot.RoleName,
                Title = !string.IsNullOrEmpty(bot.TempTitle) ? bot.TempTitle : bot.Title,
                RoleSex = bot.RoleSex,
                FactionID = bot.FactionID,
                RouteID = bot.RouteID,
                Level = bot.m_Level,
                AvartaID = bot.RolePic,
                ArmorID = bot.ArmorID,
                WeaponID = bot.WeaponID,
                WeaponEnhanceLevel = bot.WeaponEnhanceLevel,
                WeaponSeries = bot.WeaponSeries,
                HelmID = bot.HelmID,
                HorseID = bot.HorseID,
                MantleID = bot.MantleID,
                BufferDataList = bot.Buffs.ToBufferData(),
                AttackSpeed = bot.GetCurrentAttackSpeed(),
                CastSpeed = bot.GetCurrentCastSpeed(),
                CurrentDir = (int)bot.CurrentDir,
                HP = bot.m_CurrentLife,
                MaxHP = bot.m_CurrentLifeMax,
                IsRiding = bot.IsRiding,
                MoveSpeed = bot.GetCurrentRunSpeed(),
                PosX = (int)bot.CurrentPos.X,
                PosY = (int)bot.CurrentPos.Y,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<RoleDataMini>(rd);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_KT_G2C_NEW_BOT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi gói tin thông báo xóa BOT tới người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="mapCode"></param>
        /// <param name="areaID"></param>
        public void NotifyMySelfDelBot(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, int botID)
        {
            string strcmd = string.Format("{0}:{1}", botID, mapCode);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_KT_G2C_DEL_BOT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        #endregion Đối BOT

        /// <summary>
        /// dayID 是激活项索引
        /// 修改DayID 必须大于等于1，且小于等于7
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyKaiFuOnlineDayID(KPlayer client, int dayID, bool writeToDB = false, bool notifyClient = true)
        {
            //对 非法参数不进行任何处理
            if (dayID < 1 || dayID > 7)
            {
                return;
            }

            //更新到数据库
            SaveKaiFuOnlineDayID(client, dayID, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.KaiFuOnlineDayID, dayID);
            }
        }

        /// <summary>
        /// 保存开服在线奖励DayID
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveKaiFuOnlineDayID(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.KaiFuOnlineDayID, nValue, writeToDB);
        }

        /// <summary>
        /// 读取开服在线奖励DayID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetKaiFuOnlineDayID(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.KaiFuOnlineDayID);
        }

        /// <summary>
        /// ID 是记忆索引
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyTo60or100ID(KPlayer client, int nID, bool writeToDB = false, bool notifyClient = true)
        {
            //更新到数据库
            SaveTo60or100ID(client, nID, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.To60or100, nID);
            }
        }

        /// <summary>
        /// 保存开服在线奖励DayID
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveTo60or100ID(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.To60or100, nValue, writeToDB);
        }

        /// <summary>
        /// 读取开服在线奖励DayID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetTo60or100ID(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.To60or100);
        }

        /// <summary>
        /// 通知客户端角色参数发生变化
        /// </summary>
        /// <param name="client"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void NotifySelfParamsValueChange(KPlayer client, RoleCommonUseIntParamsIndexs index, int value)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, (int)index, value);

            SendToClient(client, strcmd, (int)TCPGameServerCmds.CMD_SPR_ROLEPARAMSCHANGE);
        }

        #endregion

        #region Background works

        /// <summary>
        /// Ghia lại thời gian online
        /// </summary>
        private void DoSpriteHeart(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            c.DayOnlineSecond = c.BakDayOnlineSecond + (int)((TimeUtil.NOW() - c.DayOnlineRecSecond) / 1000);
        }

        /// <summary>
        /// Duy trì kết nối với GAME DB SERVICE
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteDBHeart(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            long subTicks = 0;
            long ticks = TimeUtil.NOW();
            subTicks = ticks - c.LastDBHeartTicks;

            if (subTicks < (10 * 1000))
            {
                return;
            }

            long remainder = 0;
            Math.DivRem(subTicks, 1000, out remainder);
            subTicks -= remainder;
            ticks -= remainder;
            c.LastDBHeartTicks = ticks;

            //Thực hiện cập nhật thời gian online cho GAMECLIENT
            UpdateRoleOnlineTimes(c, subTicks);
        }

        /// <summary>
        /// Thực hiện xử lý vật phẩm hết thời gian
        /// </summary>
        /// <param name="client"></param>
        private static void DoSpriteGoodsTimeLimit(KPlayer client)
        {
            bool isFashion = false;
            bool isGoods = false;

            long nowTicks = TimeUtil.NOW();
            long elapseTicks = nowTicks - client.LastGoodsLimitUpdateTicks;

            List<GoodsData> expiredList = null;

            //Đánh giá thời gian của lần cập nhật bản đồ có giới hạn thời gian cuối cùng
            if (elapseTicks >= 30 * 1000) //Sau mỗi 30s kiểm tra lại
            {
                List<GoodsData> goodsList = Global.GetGoodsTimeExpired(client);
                if (goodsList != null)
                {
                    if (expiredList == null)
                        expiredList = goodsList;
                    else
                        expiredList.AddRange(Global.GetGoodsTimeExpired(client));
                }

                isGoods = true;
            }

            if (null != expiredList && expiredList.Count > 0)
            {
                //Tiêu hủy các vật phẩm ở đây
                for (int n = 0; n < expiredList.Count; n++)
                {
                    GoodsData goods = expiredList[n];

                    if (ItemManager.DestroyGoods(client, goods, "HẾT HẠN VẬT PHẨM"))
                    {
                        GameManager.ClientMgr.SendSystemChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, "Trên người bạn, vật phẩm [" + KTGlobal.GetItemName(goods) + "] đã hết hạn, tự động bị xóa bỏ!");

                       // KTMailManager.SendSystemMailToPlayer(client, "Vật phẩm hết thời hạn", "Trên người bạn, vật phẩm [" + KTGlobal.GetItemName(goods) + "] đã hết hạn, tự động bị xóa bỏ!");
                    }
                }
            }

            if (isGoods) client.LastGoodsLimitUpdateTicks = nowTicks;
        }

        /// <summary>
        /// Hàm này thực hiện Logic cập nhật vị trí người chơi để kiểm tra các đối tượng xung quanh
        /// </summary>
        /// <param name="client"></param>
        /// <param name="minTicks"></param>
        public static void DoSpriteMapGridMove(KPlayer client, int minTicks = 200)
        {
            long ticks = KTGlobal.GetCurrentTimeMilis();
            //Console.WriteLine("ticks: " + ticks + "-minTicks: " + minTicks + "- client.LastUpdateGridTicks: " + client.LastUpdateGridTicks);
            if (ticks - client.LastUpdateGridTicks >= minTicks)
            {
                client.LastUpdateGridTicks = ticks;
                Global.GameClientMoveGrid(client);
            }
            
        }

        /// <summary>
        /// Thực hiện các công việc trên 1 người chơi
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteWorks(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            /// Ghi lại thời gian Online
            this.DoSpriteHeart(sl, pool, client);

            /// Tự xóa các danh hiệu hết thời hạn
            client.AutoRemoveTimeoutTitles();

            /// Xử lý các sự kiện qua ngày
            KTLogic.ChangeDayLoginNum(client);

            /// Tick kiểm tra gì đó
            Global.ProcessClientHeart(client);

            /// Thực hiện xóa bỏ các vật phẩm hết hạn
            ClientManager.DoSpriteGoodsTimeLimit(client);

            /// Thực hiện update trạng thái các ICON Tới client
            client._IconStateMgr.DoSpriteIconTicks(client);

            /// Thời gian xử lý thư nhóm
            GroupMailManager.CheckRoleGroupMail(client);

            GetInterestingDataMgr.Instance().Update(client);
        }

        /// <summary>
        /// Thực hiện các cồng việc nền cho toàn bộ các client
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteBackgourndWork(SocketListener sl, TCPOutPacketPool pool)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.ClosingClientStep > 0)
                {
                    continue;
                }

                /// Xử lý công việc nền cho các client
                DoSpriteWorks(sl, pool, client);
            }
        }

        /// <summary>
        /// Thực hiện cập nhật DB
        /// </summary>
        /// <param name="client"></param>
        public void DoDBWorks(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            DoSpriteDBHeart(sl, pool, client);

            // Lưu các thông khác vào DB
            GameDb.ProcessDBCmdByTicks(client, false);

            //Lưu thông tin về skill vào DB
            GameDb.ProcessDBSkillCmdByTicks(client, false);

            //Lưu các thông tin Pram vào DB
            GameDb.ProcessDBRoleParamCmdByTicks(client, false);

           
        }

        /// <summary>
        /// Thực hiện cập nhật thông tin toàn bộ người chơi vào DB
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteDBWork(SocketListener sl, TCPOutPacketPool pool)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.ClosingClientStep > 0)
                {
                    continue;
                }
                /// 处理角色DB的后台工作
                DoDBWorks(sl, pool, client);
            }
        }

        /// <summary>
        /// Hàm này gọi liên tục ở cơ chế đa luồng để cập nhật vị trí và các mục tiêu xung quanh người chơi (có delay cho mỗi Client)
        /// </summary>
        /// <param name="client"></param>
        public void DoSpritesMapGridMove(int nThead)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.RoleID % ServerConfig.Instance.MaxUpdateGridThread != nThead)
                {
                    continue;
                }

                DoSpriteMapGridMove(client);

                //if (GameManager.MaxSleepOnDoMapGridMoveTicks > 0)
                //{
                //    Thread.Sleep(GameManager.MaxSleepOnDoMapGridMoveTicks);
                //}
            }
        }

        /// <summary>
        /// Hàm này gọi liên tục ở cơ chế đa luồng để cập nhật vị trí và các mục tiêu xung quanh người chơi (không delay)
        /// </summary>
        /// <param name="client"></param>
        public static void DoSpritesMapGridMoveNewMode(int nThead)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClientNew(ref index)) != null)
            {
                if (client.RoleID % ServerConfig.Instance.MaxUpdateGridThread != nThead)
                {
                    continue;
                }

                Global.GameClientMoveGrid(client);
            }
        }

        public static KPlayer GetNextClientNew(ref int nNid)
        {
            if (nNid < 0 || nNid >= MAX_CLIENT_COUNT) return null;

            KPlayer client = null;
            for (; nNid < MAX_CLIENT_COUNT; nNid++)
            {
                if (null != _ArrayClientsNew[nNid])
                {
                    client = _ArrayClientsNew[nNid];

                    nNid++;
                    break;
                }
            }

            return client;
        }
        #endregion

        #region Bạc trong thương khố

        /// <summary>
        /// Thông báo về Client bạc trong kho thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserStoreMoneyChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StoreMoney);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_STORE_MONEY_CHANGE);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Thêm bạc trong kho cho người chơi
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserStoreMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addMoney, string strFrom, bool NeedWriterLogs = true)
        {
            if (0 == addMoney)
            {
                return true;
            }

            long oldMoney = client.StoreMoney;

            lock (client.StoreMoneyMutex)
            {
                if (addMoney < 0 && oldMoney < Math.Abs(addMoney))
                {
                    return false;
                }

                /// Gói tin gửi lên GameDB
                string strcmd = string.Format("{0}:{1}", client.RoleID, addMoney);
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_ADD_STORE_MONEY, strcmd, client.ServerId);
                /// Nếu DB không trả ra kết quả
                if (null == dbFields || dbFields.Length != 2)
                {
                    return false;
                }

                /// Nếu số lượng bạc trong kho hiện có < 0 thì toác
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                /// Cập nhật lại số bạc trong kho
                client.StoreMoney = Convert.ToInt32(dbFields[1]);

                if (0 != addMoney)
                {
                    if (NeedWriterLogs)
                    {
                        int RoleID = client.RoleID;
                        string AccountName = client.strUserID;
                        string RecoreType = "MONEY";
                        string RecoreDesc = strFrom;
                        string Source = "SYSTEM";
                        string Taget = client.RoleName;
                        string OptType = "ADD";

                        int ZONEID = client.ZoneID;

                        string OtherPram_1 = oldMoney + "";
                        string OtherPram_2 = client.BoundMoney + "";
                        string OtherPram_3 = MoneyType.StoreMoney + "";
                        string OtherPram_4 = "NONE";

                        //Thực hiện việc ghi LOGS vào DB
                        GameManager.logDBCmdMgr.WriterLogs(RoleID, AccountName, RecoreType, RecoreDesc, Source, Taget, OptType, ZONEID, OtherPram_1, OtherPram_2, OtherPram_3, OtherPram_4, client.ServerId);
                    }

                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyType.StoreMoney, addMoney, client.StoreMoney, strFrom);
                }
            }

            /// Thông báo số bạc trong kho thay đổi
            GameManager.ClientMgr.NotifySelfUserStoreMoneyChange(sl, pool, client);

            /// Sự kiện gì đó
            Global.AddRoleStoreMoneyEvent(client, oldMoney);

            return true;
        }

        #endregion
    }
}