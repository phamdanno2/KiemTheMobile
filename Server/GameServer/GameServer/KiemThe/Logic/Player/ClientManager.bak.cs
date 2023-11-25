using GameServer.Core.Executor;
using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.Interface;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.Logic.ActivityNew;
using GameServer.Logic.ActivityNew.SevenDay;
using GameServer.Logic.CheatGuard;
using GameServer.Logic.FluorescentGem;
using GameServer.Logic.UnionAlly;
using GameServer.Logic.UnionPalace;
using GameServer.Logic.UserReturn;
using GameServer.Logic.YueKa;

//using System.Windows.Documents;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using Tmsk.Contract;

namespace GameServer.Logic
{
    /// <summary>
    /// 游戏客户端管理
    /// </summary>
    public class ClientManager
    {
        #region 基本属性和方法

        private const int MAX_CLIENT_COUNT = 2000;

        public int GetMaxClientCount()
        {
            return MAX_CLIENT_COUNT;
        }

        /// <summary>
        /// 客户端队列
        /// </summary>
        //private List<KPlayer> _ListClients = new List<KPlayer>(1000);
        private KPlayer[] _ArrayClients = new KPlayer[MAX_CLIENT_COUNT];

        /// <summary>
        /// 客户端映射对象
        /// </summary>
        //private Dictionary<TMSKSocket, KPlayer> _DictClients = new Dictionary<TMSKSocket, KPlayer>(1000);
        // roleid->nid的字典
        private Dictionary<int, int> _DictClientNids = new Dictionary<int, int>(MAX_CLIENT_COUNT);

        /// <summary>
        /// 空闲列表
        /// </summary>
        private List<int> _FreeClientList = new List<int>(MAX_CLIENT_COUNT);

        /// <summary>
        /// 客户端容器对象
        /// </summary>
        private SpriteContainer Container = new SpriteContainer();

        public void initialize(IEnumerable<XElement> mapItems)
        {
            Container.initialize(mapItems);

            for (int i = 0; i < MAX_CLIENT_COUNT; i++)
            {
                _ArrayClients[i] = null;
                // 初始化时增加到空闲列表
                _FreeClientList.Add(i);
            }
        }

        /// <summary>
        /// 添加一个新的客户端
        /// </summary>
        /// <param name="client"></param>
        public bool AddClient(KPlayer client)
        {
            try
            {
                /*lock (_ListClients)
                {
                    if (_ListClients.FindIndex((x) => { return x.RoleID == client.RoleID && x.ClosingClientStep == 0; }) >= 0)
                    {
                        return false;
                    }
                    _ListClients.Add(client);
                }*/

                KPlayer gc = FindClient(client.RoleID);
                if (null != gc)
                {
                    // 要把无心跳的客户端先删掉
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

                lock (_DictClientNids)
                {
                    _DictClientNids[client.RoleID] = index;
                }

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
        /// 添加一个新的客户端到容器
        /// </summary>
        /// <param name="client"></param>
        public void AddClientToContainer(KPlayer client)
        {
            // 也添加到客户端容器对象中
            Container.AddObject(client.RoleID, client.MapCode, client);
        }

        /// <summary>
        /// 删除一个客户端
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

            lock (_DictClientNids)
            {
                try
                {
                    _DictClientNids.Remove(client.RoleID);
                }
                catch (Exception ex)
                {
                    LogManager.WriteException(ex.ToString());
                    try
                    {
                        _DictClientNids.Remove(client.RoleID);
                    }
                    catch (Exception ex2)
                    {
                        LogManager.WriteException(string.Format("try agin:{0}", ex2.ToString()));
                    }
                }
            }

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
        /// 从容器删除一个客户端
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClientFromContainer(KPlayer client)
        {
            GameMap gameMap = null;
            //从格子的定位队列中删除
            if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap) || null == gameMap)
            {
                LogManager.WriteLog(LogTypes.Error, "RemoveClientFromContainer 错误的地图编号：" + client.MapCode);
                return;
            }

            bool removed = GameManager.MapGridMgr.DictGrids[client.MapCode].RemoveObject(client);

            //也从客户端容器对象中删除
            removed = Container.RemoveObject(client.RoleID, client.MapCode, client) && removed;

            if (!removed)
            {
                foreach (var mc in GameManager.MapMgr.DictMaps.Keys)
                {
                    GameManager.MapGridMgr.DictGrids[mc].RemoveObject(client);
                    Container.RemoveObject(client.RoleID, mc, client);
                }
            }
        }

        /// <summary>
        /// 根据玩家的roleid返回流水号 便于快速查找到GameClient
        /// </summary>
        public int FindClientNid(int RoleID)
        {
            int nNid = -1;
            lock (_DictClientNids)
            {
                if (!_DictClientNids.TryGetValue(RoleID, out nNid))
                {
                    return -1;
                }
            }
            return nNid;
        }

        /// <summary>
        /// 通过TMSKSocket查找一个客户端
        /// </summary>
        /// <param name="client"></param>
        public KPlayer FindClientByNid(int nNid)
        {
            if (nNid < 0 || nNid >= MAX_CLIENT_COUNT) return null;

            return _ArrayClients[nNid];
        }

        /// <summary>
        /// 通过TMSKSocket查找一个客户端
        /// </summary>
        /// <param name="client"></param>
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
        /// 判断客户端是否存在
        /// </summary>
        /// <param name="client"></param>
        public bool ClientExists(KPlayer client)
        {
            object obj = null;
            lock (Container.ObjectDict)
            {
                Container.ObjectDict.TryGetValue(client.RoleID, out obj);
            }

            return (null != obj);
        }

        /// <summary>
        /// 获取下一个客户端
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
                    // 便于循环取得下一个client
                    nNid++;
                    break;
                }
            }

            return client;
        }

        /// <summary>
        /// 获取地图中的所有的角色
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public List<Object> GetMapClients(int mapCode)
        {
            return Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
        }

        public List<KPlayer> GetMapGameClients(int mapCode)
        {
            List<object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            List<KPlayer> clientList = new List<KPlayer>();
            if (null != objsList)
            {
                foreach (var obj in objsList)
                {
                    KPlayer client = obj as KPlayer;
                    if (null != client)
                    {
                        clientList.Add(client);
                    }
                }
            }

            return clientList;
        }

        /// <summary>
        /// 返回地图中活着的玩家列表
        /// </summary>
        /// <returns></returns>
        public List<KPlayer> GetMapAliveClients(int mapCode)
        {
            List<KPlayer> lsAliveClient = new List<KPlayer>();

            KPlayer client = null;

            List<Object> lsObjects = GetMapClients(mapCode);
            if (null == lsObjects)
            {
                return lsAliveClient;
            }

            for (int n = 0; n < lsObjects.Count; n++)
            {
                client = lsObjects[n] as KPlayer;
                if (null != client && client.m_CurrentLife > 0)
                {
                    lsAliveClient.Add(client);
                }
            }

            return lsAliveClient;
        }

        public List<KPlayer> GetMapAliveClientsEx(int mapCode, bool writeLog = true)
        {
            List<KPlayer> lsAliveClient = new List<KPlayer>();

            KPlayer client = null;
            List<Object> lsObjects = Container.GetObjectsByMap(mapCode);
            if (null == lsObjects)
            {
                return lsAliveClient;
            }

            for (int n = 0; n < lsObjects.Count; n++)
            {
                client = lsObjects[n] as KPlayer;
                if (null != client && client.m_CurrentLife > 0)
                {
                    bool valid = false;
                    if (!client.WaitingNotifyChangeMap && !client.WaitingForChangeMap)
                    {
                        if (client.MapCode == mapCode && Global.IsPosReachable(mapCode, client.PosX, client.PosY))
                        {
                            valid = true;
                            lsAliveClient.Add(client);
                        }

                        if (writeLog && !valid)
                        {
                            /**/
                            string reason = string.Format("存活玩家坐标非法:{6}({7}) mapCode:{0},clientMapCode{1}:,WaitingNotifyChangeMap:{2},WaitingForChangeMap:{3},PosX:{4},PosY{5}",
                            mapCode, client.MapCode, client.WaitingNotifyChangeMap, client.WaitingForChangeMap, client.PosX,
                            client.PosY, client.RoleID, client.RoleName);
                            LogManager.WriteLog(LogTypes.Error, reason);
                        }
                    }
                }
            }

            return lsAliveClient;
        }

        public int GetMapAliveClientCountEx(int mapCode)
        {
            int aliveClientCount = 0;
            KPlayer client = null;
            List<Object> lsObjects = Container.GetObjectsByMap(mapCode);
            if (null == lsObjects)
            {
                return aliveClientCount;
            }

            for (int n = 0; n < lsObjects.Count; n++)
            {
                client = lsObjects[n] as KPlayer;
                if (null != client && client.m_CurrentLife > 0)
                {
                    if (!client.WaitingNotifyChangeMap && !client.WaitingForChangeMap)
                    {
                        if (client.MapCode == mapCode && !Global.InOnlyObsByXY(ObjectTypes.OT_CLIENT, mapCode, client.PosX, client.PosY))
                        {
                            aliveClientCount++;
                        }
                    }
                }
            }

            return aliveClientCount;
        }

        /// <summary>
        /// 获取地图上的用户的个数
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public int GetMapClientsCount(int mapCode)
        {
            return Container.GetObjectsCountByMap(mapCode);
        }

        /// <summary>
        /// 获取在线客户端的个数
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

        public int GetClientCountFromDict()
        {
            int count = 0;
            lock (_DictClientNids)
            {
                count = _DictClientNids.Count;
            }

            return count;
        }

        public string GetAllMapRoleNumStr()
        {
            return Container.GetAllMapRoleNumStr();
        }

        /// <summary>
        /// 获取第一个用户
        /// </summary>
        /// <returns></returns>
        public KPlayer GetFirstClient()
        {
            KPlayer client = null;

            lock (_DictClientNids)
            {
                if (_DictClientNids.Count > 0)
                {
                    foreach (var item in _DictClientNids)
                    {
                        return FindClientByNid(item.Value);
                    }
                }
            }

            return client;
        }

        /// <summary>
        /// 获取一个随机用户
        /// </summary>
        /// <returns></returns>
        public KPlayer GetRandomClient()
        {
            lock (_DictClientNids)
            {
                if (_DictClientNids.Count > 0)
                {
                    int[] array = new int[MAX_CLIENT_COUNT];
                    _DictClientNids.Values.CopyTo(array, 0);
                    int index = Global.GetRandomNumber(0, _DictClientNids.Count);
                    return FindClientByNid(array[index]);
                }
            }

            return null;
        }

        #endregion 基本属性和方法

        #region 扩展属性和方法

        #region 公用的发送方法

        public void PushBackTcpOutPacket(TCPOutPacket tcpOutPacket)
        {
            if (null != tcpOutPacket)
            {
                //还回tcpoutpacket
                Global._TCPManager.TcpOutPacketPool.Push(tcpOutPacket);
            }
        }

        /// <summary>
        /// 将消息包发送到其他用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients(SocketListener sl, TCPOutPacketPool pool, object self, List<object> objsList, byte[] bytesData, int cmdID)
        {
            if (null == objsList) return;

            TCPOutPacket tcpOutPacket = null;
            try
            {
                for (int i = 0; i < objsList.Count; i++)
                {
                    //是否跳过自己
                    if (null != self && self == objsList[i])
                    {
                        continue;
                    }

                    KPlayer c = objsList[i] as KPlayer;
                    if (null == c)
                    {
                        continue;
                    }

                    if (c.LogoutState) //如果已经退出了
                    {
                        continue;
                    }

                    if (null == tcpOutPacket)
                    {
                        tcpOutPacket = pool.Pop();
                        tcpOutPacket.PacketCmdID = (UInt16)cmdID;
                        tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);
                    }

                    if (!sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false))
                    {
                        //
                        /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                            tcpOutPacket.PacketCmdID,
                            tcpOutPacket.PacketDataSize,
                            (objsList[i] as KPlayer).RoleID,
                            (objsList[i] as KPlayer).RoleName));*/
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 将消息包发送到其他用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients(SocketListener sl, TCPOutPacketPool pool, object self, List<object> objsList, string strCmd, int cmdID)
        {
            if (null == objsList) return;

            TCPOutPacket tcpOutPacket = null;
            try
            {
                for (int i = 0; i < objsList.Count; i++)
                {
                    //是否跳过自己
                    if (null != self && self == objsList[i])
                    {
                        continue;
                    }

                    KPlayer c = objsList[i] as KPlayer;
                    if (null == c)
                    {
                        continue;
                    }

                    if (c.LogoutState) //如果已经退出了
                    {
                        continue;
                    }

                    if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strCmd, cmdID);
                    if (!sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false))
                    {
                        //
                        /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                            tcpOutPacket.PacketCmdID,
                            tcpOutPacket.PacketDataSize,
                            (objsList[i] as KPlayer).RoleID,
                            (objsList[i] as KPlayer).RoleName));*/
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 将消息包发送到其他用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients<T>(SocketListener sl, TCPOutPacketPool pool, object self, List<object> objsList, T scData, int cmdID)
        {
            if (null == objsList) return;

            TCPOutPacket tcpOutPacket = null;
            try
            {
                for (int i = 0; i < objsList.Count; i++)
                {
                    //是否跳过自己
                    if (null != self && self == objsList[i])
                    {
                        continue;
                    }

                    if (!(objsList[i] is KPlayer))
                    {
                        continue;
                    }

                    if ((objsList[i] as KPlayer).LogoutState) //如果已经退出了
                    {
                        continue;
                    }

                    (objsList[i] as KPlayer).sendCmd(cmdID, scData);

                    //if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strCmd, cmdID);
                    //if (!sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false))
                    //{
                    //    //
                    //    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    //        tcpOutPacket.PacketCmdID,
                    //        tcpOutPacket.PacketDataSize,
                    //        (objsList[i] as KPlayer).RoleID,
                    //        (objsList[i] as KPlayer).RoleName));*/
                    //}
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 将消息包发送到其他用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClients<T1, T2>(SocketListener sl, TCPOutPacketPool pool, object self, List<T1> objsList, T2 data, int cmdID, int hideFlag, int includeRoleId)
        {
            if (null == objsList) return;

            TCPOutPacket tcpOutPacket = null;
            try
            {
                for (int i = 0; i < objsList.Count; i++)
                {
                    //是否跳过自己
                    if (null != self && self == (object)objsList[i])
                    {
                        continue;
                    }

                    KPlayer c = objsList[i] as KPlayer;
                    if (null == c)
                    {
                        continue;
                    }

                    if (c.RoleID != includeRoleId && (c.ClientEffectHideFlag1 & hideFlag) > 0)
                    {
                        continue;
                    }

                    if (c.LogoutState) //如果已经退出了
                    {
                        continue;
                    }

                    if (null == tcpOutPacket)
                    {
                        if (data is string)
                        {
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, data as string, cmdID);
                        }
                        else if (data is byte[])
                        {
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, data as byte[], cmdID);
                        }
                        else
                        {
                            return; //尚未实现
                        }
                    }
                    if (!sl.SendData((objsList[i] as KPlayer).ClientSocket, tcpOutPacket, false))
                    {
                        //
                        /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                            tcpOutPacket.PacketCmdID,
                            tcpOutPacket.PacketDataSize,
                            (objsList[i] as KPlayer).RoleID,
                            (objsList[i] as KPlayer).RoleName));*/
                    }
                }
            }
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 将消息包发送到某用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strCmd, int cmdID)
        {
            if (null == client) return;

            if (client.LogoutState) //如果已经退出了
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strCmd, cmdID);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// 将消息包发送到某用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(KPlayer client, string strCmd, int cmdID)
        {
            if (null == client) return;

            if (client.LogoutState) //如果已经退出了
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strCmd, cmdID);

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
        /// 将消息包发送到某用户
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="tcpOutPacket"></param>
        public void SendToClient(KPlayer client, byte[] buffer, int cmdID)
        {
            if (null == client) return;

            if (client.LogoutState) //如果已经退出了
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, buffer, 0, buffer.Length, cmdID);

            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 公用的发送方法

        #region 通知角色打开窗口

        /// <summary>
        /// 通知客户端打开窗口
        /// </summary>
        /// <param name="client"></param>
        /// <param name="windowType"></param>
        /// <param name="strParams"></param>
        public void NotifyClientOpenWindow(KPlayer client, int windowType, String strParams)
        {
            String cmd = String.Format("{0}:{1}:{2}", client.RoleID, windowType, strParams);

            SendToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, cmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYOPENWINDOW);
        }

        #endregion 通知角色打开窗口

        #region 角色数据通知

        /// <summary>
        /// Thông báo cho người chơi khác BẢN THÂN ĐANG CHẠY TỚI
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersIamComing(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList, int cmd)
        {
            if (null == objsList) return;

            RoleData roleData = KTLogic.ClientToRoleData2(client);
            byte[] bytesData = DataHelper.ObjectToBytes<RoleData>(roleData);

            //群发消息
            SendToClients(sl, pool, client, objsList, bytesData, cmd);
        }

        /// <summary>
        /// Thông báo tới bản thân các đối tượng khác xung quanh
        /// </summary>
        /// <param name="client"></param>
        public int NotifySelfOnlineOthers(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList, int cmd)
        {
            if (null == objsList) return 0;

            int totalCount = 0;
            for (int i = 0; i < objsList.Count && i < 30; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (client.RoleID == (objsList[i] as KPlayer).RoleID)
                {
                    continue;
                }

                TCPOutPacket tcpOutPacket = null;
                RoleDataMini roleDataMini = Global.ClientToRoleDataMini((objsList[i] as KPlayer));
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDataMini>(roleDataMini, pool, cmd);

                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                    break;
                }

                totalCount++;
            }

            return totalCount;
        }

        /// <summary>
        /// Thông báo cho người chơi khác biết giữ liệu của ROLE DATA
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfOtherData(SocketListener sl, TCPOutPacketPool pool, KPlayer client, RoleDataEx roleDataEx, int cmd)
        {
            RoleData roleData = null;
            if (null != roleDataEx)
            {
                roleData = KTLogic.RoleDataExToRoleData(roleDataEx);
            }

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData>(roleData, pool, cmd);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 角色数据通知

        #region 加载和移动通知

        /// <summary>
        /// Thông báo cho bản thân có đối tượng khác mới xuất hiện xung quanh
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="otherRoleID"></param>
        /// <param name="mapCode"></param>
        /// <param name="action"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cmd"></param>
        /// <param name="moveCost"></param>
        /// <param name="extAction"></param>
        public void NotifyMyselfOtherLoadAlready(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int otherRoleID, int currentX, int currentY, int toX, int toY, int currentDirection, int action, string paths)
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
            };

            TCPOutPacket tcpOutPacket = null;
            byte[] bytes = DataHelper.ObjectToBytes<LoadAlreadyData>(loadAlreadyData);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_LOADALREADY);
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
        /// 通知其他人自己开始移动(同一个地图才需要通知)
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
        /// 通知其他人自己移动结束(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMyMovingEnd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int mapCode, int action, int toX, int toY, int direction, int tryRun, bool sendToSelf, List<Object> objsList = null)
        {
            if (null == objsList)
            {
                objsList = Global.GetAll9Clients(client);
            }

            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", client.RoleID, mapCode, action, toX, toY, direction, tryRun);

            SCMoveEnd scData = new SCMoveEnd(client.RoleID, mapCode, action, toX, toY, direction, tryRun);
            //群发消息
            SendToClients<SCMoveEnd>(sl, pool, sendToSelf ? null : client, objsList, scData, (int)TCPGameServerCmds.CMD_SPR_MOVEEND);
            //SendToClients(sl, pool, sendToSelf ? null : client, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_MOVEEND);
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
                PosX = (int) obj.CurrentPos.X,
                PosY = (int) obj.CurrentPos.Y,
                MoveSpeed = obj.GetCurrentRunSpeed(),
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
                StartMoveTick = startMoveTicks,
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
        /// <param name="otherRoleID"></param>
        /// <param name="mapCode"></param>
        /// <param name="action"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cmd"></param>
        /// <param name="moveCost"></param>
        /// <param name="extAction"></param>
        public void NotifyMyselfMonsterLoadAlready(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int monsterID, int currentX, int currentY, int toX, int toY, int currentDirection, int action, string paths)
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
            };

            TCPOutPacket tcpOutPacket = null;
            byte[] bytes = DataHelper.ObjectToBytes<LoadAlreadyData>(loadAlreadyData);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_LOADALREADY);
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
        /// 通知自己怪物的移动
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="otherRoleID"></param>
        /// <param name="mapCode"></param>
        /// <param name="action"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cmd"></param>
        /// <param name="moveCost"></param>
        /// <param name="extAction"></param>
        public void NotifyMyselfMonsterMoving(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int monsterID, int mapCode, int action, long startMoveTicks, int fromX, int fromY, int toX, int toY, int cmd, double moveCost = 1.0, int extAction = 0, String pathString = "")
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", monsterID, mapCode, action, toX, toY, moveCost, extAction, fromX, fromY, startMoveTicks, pathString);//怪物宠物镖车等的移动历史路径为空
            SpriteNotifyOtherMoveData moveData = new SpriteNotifyOtherMoveData()
            {
                RoleID = monsterID,
                FromX = fromX,
                FromY = fromY,
                ToX = toX,
                ToY = toY,
                PathString = pathString,
                StartMoveTick = startMoveTicks,
            };
            TCPOutPacket tcpOutPacket = null;
            //tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, cmd);
            byte[] bytes = DataHelper.ObjectToBytes<SpriteNotifyOtherMoveData>(moveData);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, cmd);
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
        /// 通知自己怪物们的的移动
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="objsList"></param>
        public void NotifyMyselfMonstersMoving(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is Monster))
                {
                    continue;
                }

                //判断如果是在移动中
                if ((objsList[i] as Monster).m_eDoing == KiemThe.Entities.KE_NPC_DOING.do_walk || (objsList[i] as Monster).m_eDoing == KiemThe.Entities.KE_NPC_DOING.do_run)
                {
                    Monster monster = objsList[i] as Monster;

                    //通知自己其他人的移动
                    GameManager.ClientMgr.NotifyMyselfMonsterMoving(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                        (objsList[i] as Monster).RoleID,
                        (objsList[i] as Monster).MonsterZoneNode.MapCode,
                        (int)(objsList[i] as Monster).m_eDoing,
                        Global.GetMonsterStartMoveTicks((objsList[i] as Monster)),
                        (int)(objsList[i] as Monster).CurrentPos.X,
                        (int)(objsList[i] as Monster).CurrentPos.Y,
                        0,
                        0,
                        (int)TCPGameServerCmds.CMD_SPR_MOVE, (objsList[i] as Monster).GetCurrentRunSpeed(), 0, ""/*null != monster? monster.PathString : ""*/);
                }
            }
        }

        #endregion 加载和移动通知

        #region 动作通知

        /// <summary>
        /// 通知其他人自己开始做动作(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMyAction(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int mapCode, int direction, int action, int x, int y, int targetX, int targetY, int yAngle, int moveToX, int moveToY, int cmd)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", roleID, mapCode, direction, action, x, y, targetX, targetY, yAngle, moveToX, moveToY);

            SpriteActionData cmdData = new SpriteActionData();
            cmdData.roleID = roleID;
            cmdData.mapCode = mapCode;
            cmdData.direction = direction;
            cmdData.action = action;
            cmdData.toX = x;
            cmdData.toY = y;
            cmdData.targetX = targetX;
            cmdData.targetY = targetY;
            cmdData.yAngle = yAngle;
            cmdData.moveToX = moveToX;
            cmdData.moveToY = moveToY;

            //群发消息
            SendToClients(sl, pool, null, objsList, /*strcmd*/  DataHelper.ObjectToBytes<SpriteActionData>(cmdData), cmd);
        }

        /// <summary>
        /// 通知其他人怪(宠物/卫兵)开始做动作(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersDoAction(SocketListener sl, TCPOutPacketPool pool, IObject obj, int mapCode, int copyMapID, int roleID, int direction, int action, int x, int y, int targetX, int targetY, int cmd, List<Object> objsList)
        {
            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, x, y, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (null == objsList) return;

            //             string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", roleID, mapCode, direction, action, x, y, targetX, targetY, -1, 0, 0);
            //
            //             //群发消息
            //             SendToClients(sl, pool, null, objsList, strcmd, cmd);

            SpriteActionData cmdData = new SpriteActionData();
            cmdData.roleID = roleID;
            cmdData.mapCode = mapCode;
            cmdData.direction = direction;
            cmdData.action = action;
            cmdData.toX = x;
            cmdData.toY = y;
            cmdData.targetX = targetX;
            cmdData.targetY = targetY;
            cmdData.yAngle = -1;
            cmdData.moveToX = 0;
            cmdData.moveToY = 0;

            //群发消息
            SendToClients(sl, pool, null, objsList, /*strcmd*/  DataHelper.ObjectToBytes<SpriteActionData>(cmdData), cmd);
        }

        #endregion 动作通知

        #region 旋转角度通知

        /// <summary>
        /// 通知其他人自己开始旋转的角度(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersChangeAngle(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int direction, int yAngle, int cmd)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", roleID, direction, yAngle);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, cmd);
        }

        #endregion 旋转角度通知

        #region 技能使用

        /// <summary>
        /// 通知其他人自己开始做动作的准备工作(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMagicCode(SocketListener sl, TCPOutPacketPool pool, IObject attacker, int roleID, int mapCode, int magicCode, int cmd)
        {
            List<Object> objsList = Global.GetAll9Clients(attacker);
            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}", roleID, mapCode, magicCode);
            SpriteMagicCodeData cmdData = new SpriteMagicCodeData();

            cmdData.roleID = roleID;
            cmdData.mapCode = mapCode;
            cmdData.magicCode = magicCode;

            //群发消息
            SendToClients(sl, pool, attacker, objsList, /*strcmd*/DataHelper.ObjectToBytes(cmdData), cmd);
        }

        #endregion 技能使用

        #region 伤害命中通知

        /// <summary>
        /// 通知所有在线用户某个精灵的被命中(无伤害时才需要单独发送)(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteHited(SocketListener sl, TCPOutPacketPool pool, IObject attacker, int enemy, int enemyX, int enemyY, int magicCode)
        {
            List<Object> objsList = Global.GetAll9Clients(attacker);
            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", attacker.GetObjectID(), enemy, enemyX, enemyY, magicCode);
            SpriteHitedData hitedData = new SpriteHitedData();

            hitedData.roleId = attacker.GetObjectID();
            hitedData.enemy = enemy;
            hitedData.magicCode = magicCode;
            if (enemy < 0)
            {
                hitedData.enemyX = enemyX;
                hitedData.enemyY = enemyY;
            }

            //2015-9-16消息流量优化
            if (!GameManager.FlagEnableHideFlags || !GameManager.HideFlagsMapDict.ContainsKey(attacker.CurrentMapCode))
            {
                SendToClients(sl, pool, null, objsList, DataHelper.ObjectToBytes<SpriteHitedData>(hitedData), (int)TCPGameServerCmds.CMD_SPR_HITED);
            }
            else
            {
                //2015-9-16消息流量优化,根据客户端的当前显示需要,客户端忽略其他人击中怪物的消息
                GSpriteTypes spriteType = Global.GetSpriteType((uint)enemy);
                KPlayer client = attacker as KPlayer;
                if (null != client)
                {
                    client.sendCmd((int)TCPGameServerCmds.CMD_SPR_HITED, hitedData);
                }

                if (spriteType == GSpriteTypes.Other && (null != client || GameManager.FlagHideFlagsType == 0))
                {
                    SendToClients(sl, pool, attacker, objsList, DataHelper.ObjectToBytes<SpriteHitedData>(hitedData), (int)TCPGameServerCmds.CMD_SPR_HITED, ClientHideFlags.HideOtherMagicAndInjured, enemy);
                }
            }
        }

        #endregion 伤害命中通知

        #region 复活相关

        /// <summary>
        /// Thông báo cho tất cả người chơi xung quanh bản thân sống lại
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersRealive(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int posX, int posY, int direction)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, posX, posY, direction);
            MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
            {
                RoleID = roleID,
                PosX = posX,
                PosY = posY,
                Direction = direction,
                CurrentHP = client.m_CurrentLife,
                CurrentMP = client.m_CurrentMana,
                CurrentStamina = client.m_CurrentStamina,
            };
            byte[] bytes = DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData);

            //群发消息
            SendToClients(sl, pool, client, objsList, /*strcmd*/bytes, (int)TCPGameServerCmds.CMD_SPR_REALIVE);
        }

        /// <summary>
        /// 通知队友自己要复活
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamRealive(SocketListener sl, TCPOutPacketPool pool, int roleID, int posX, int posY, int direction)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, posX, posY, direction);

            //判断精灵是否组队中，如果是，则也通知九宫格之外的队友
            KPlayer otherClient = FindClient(roleID);
            if (null != otherClient)
            {
                if (otherClient.TeamID > 0)
                {
                    //查找组队数据
                    TeamData td = GameManager.TeamMgr.FindData(otherClient.TeamID);
                    if (null != td)
                    {
                        List<int> roleIDsList = new List<int>();

                        //锁定组队数据
                        lock (td)
                        {
                            for (int i = 0; i < td.TeamRoles.Count; i++)
                            {
                                if (roleID == td.TeamRoles[i].RoleID)
                                {
                                    continue;
                                }

                                roleIDsList.Add(td.TeamRoles[i].RoleID);
                            }
                        }
                        TCPOutPacket tcpOutPacket = null;
                        try
                        {
                            for (int i = 0; i < roleIDsList.Count; i++)
                            {
                                KPlayer gc = FindClient(roleIDsList[i]);
                                if (null == gc) continue;

                                if (null == tcpOutPacket)
                                {
                                    MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
                                    {
                                        RoleID = roleID,
                                        PosX = posX,
                                        PosY = posY,
                                        Direction = direction,
                                    };
                                    byte[] bytes = DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData);
                                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_REALIVE);
                                }
                                if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                                {
                                    //
                                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                        tcpOutPacket.PacketCmdID,
                                        tcpOutPacket.PacketDataSize,
                                        gc.RoleID,
                                        gc.RoleName));*/
                                }
                            }
                        }
                        finally
                        {
                            PushBackTcpOutPacket(tcpOutPacket);
                        }
                    }
                }
            }
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
        public void NotifyMySelfRealive(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int posX, int posY, int direction)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, posX, posY, direction);
            MonsterRealiveData monsterRealiveData = new MonsterRealiveData()
            {
                RoleID = roleID,
                PosX = posX,
                PosY = posY,
                Direction = direction,
                CurrentHP = client.m_CurrentLife,
                CurrentMP = client.m_CurrentMana,
                CurrentStamina = client.m_CurrentStamina,
            };
            byte[] bytes = DataHelper.ObjectToBytes<MonsterRealiveData>(monsterRealiveData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_REALIVE);
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

        #endregion 复活相关

        #region 离线/离开地图相关

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
        /// 怪物离开(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMonsterLeave(SocketListener sl, TCPOutPacketPool pool, Monster monster, List<Object> objsList)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", monster.RoleID, (int)GSpriteTypes.Monster);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
        }

        /// <summary>
        /// 通知自己其他角色离线(同一个地图才需要通知)
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

                if (client.RoleID == (objsList[i] as KPlayer).RoleID) //跳过自己
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}", (objsList[i] as KPlayer).RoleID, (int)GSpriteTypes.Other);

                TCPOutPacket tcpOutPacket = null;
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                    break;
                }
            }
        }

        /// <summary>
        /// 通知自己怪物离开自己(同一个地图才需要通知)
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

                //通知自己怪物离开自己(同一个地图才需要通知)
                if (!NotifyMyselfLeaveMonsterByID(sl, pool, client, (objsList[i] as Monster).RoleID))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 通知自己怪物离开自己(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public bool NotifyMyselfLeaveMonsterByID(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int monsterID)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("九宫格: 发送删除怪物给客户端: {0}, {1}", (objsList[i] as Monster).VSName, (objsList[i] as Monster).Name));

            string strcmd = string.Format("{0}:{1}", monsterID, (int)GSpriteTypes.Monster);

            TCPOutPacket tcpOutPacket = null;
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LEAVE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
                return false;
            }

            return true;
        }

        #endregion 离线/离开地图相关

        #region 角色死亡

        /// <summary>
        /// 判断如果对方已经无血，但是还存活着，则立刻发送死亡消息
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void JugeSpriteDead(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.m_CurrentLife > 0)
            {
                return;
            }

            GameManager.SystemServerEvents.AddEvent(string.Format("角色强制死亡, roleID={0}({1}), Life={2}", client.RoleID, client.RoleName, client.m_CurrentLife), EventLevels.Debug);
            NotifySpriteInjured(sl, pool, client, client.MapCode, -1, client.RoleID, 0, 0, 0, client.m_Level, new Point(-1, -1));
        }

        #endregion 角色死亡

        #region 角色生命值变化

        /// <summary>
        /// Thông báo máu của đối tượng khác thay đổi
        /// </summary>
        /// <param name="client"></param>
        public bool NotifyOthersRelife(SocketListener sl, TCPOutPacketPool pool, IObject obj, int mapCode, int copyMapID, int roleID, int x, int y, int direction, int hp, int mp, int stamina, int cmd, List<Object> objsList)
        {
            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, x, y, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (null == objsList) return true;

            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", roleID, x, y, direction, lifeV, magicV, force);
            SpriteRelifeData relifeData = new SpriteRelifeData();
            relifeData.RoleID = roleID;
            relifeData.Direction = direction;
            relifeData.HP = hp;
            relifeData.MP = mp;
            relifeData.Stamina = stamina;

            //2015-9-16消息流量优化
            if (!GameManager.FlagEnableHideFlags)
            {
                relifeData.PosX = x;
                relifeData.PosY = y;
            }

            //群发消息
            SendToClients(sl, pool, null, objsList, /*strcmd*/DataHelper.ObjectToBytes<SpriteRelifeData>(relifeData), cmd);

            return true;
        }

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

            //             string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, client.m_CurrentLifeMax, client.m_CurrentManaMax,
            //                 client.m_CurrentLife, client.m_CurrentMana);
            SpriteLifeChangeData lifeChangeData = new SpriteLifeChangeData();

            /*lifeChangeData.roleID = client.RoleID;
            lifeChangeData.lifeV = client.m_CurrentLifeMax;
            lifeChangeData.magicV = client.m_CurrentManaMax;
            lifeChangeData.currentLifeV = client.m_CurrentLife;
            lifeChangeData.currentMagicV = client.m_CurrentMana;
            lifeChangeData.currentStanminaV = client.m_CurrentStamina;
            lifeChangeData.stanminaV = client.m_CurrentStaminaMax;*/

            byte[] cmdData = DataHelper.ObjectToBytes<SpriteLifeChangeData>(lifeChangeData);

            if (!allSend)
            {
                if (null != client)
                {
                    TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_UPDATE_ROLEDATA);
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

                return;
            }

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            //群发消息
            SendToClients(sl, pool, null, objsList, /*strcmd*/cmdData, (int)TCPGameServerCmds.CMD_SPR_UPDATE_ROLEDATA);
        }

        #endregion 角色生命值变化

        #region Dịch chuyển

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
            client.LastChangeMapTicks = TimeUtil.NOW();

            int defaultBirthPosX = GameManager.MapMgr.DictMaps[client.MapCode].DefaultBirthPosX;
            int defaultBirthPosY = GameManager.MapMgr.DictMaps[client.MapCode].DefaultBirthPosY;
            int defaultBirthRadius = GameManager.MapMgr.DictMaps[client.MapCode].BirthRadius;

            int posX = toPosX;
            int posY = toPosY;

            //如果外部不配置坐标，则回到复活点
            if (-1 == posX || -1 == posY)
            {
                //从配置根据地图取默认位置
                Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, client.MapCode, defaultBirthPosX, defaultBirthPosY, defaultBirthRadius);
                posX = (int)newPos.X;
                posY = (int)newPos.Y;
            }

            if (direction >= 0)
            {
                client.RoleDirection = direction;
            }

            GameManager.ClientMgr.ChangePosition(sl, pool,
                client, (int)posX, (int)posY, direction, (int)TCPGameServerCmds.CMD_SPR_CHANGEPOS);
        }

        #endregion Dịch chuyển

        #region Nhân vật thay đổi trang phục

        /// <summary>
        /// Thay đổi quần áo cùng vũ khí ( Cùng một cái địa đồ mới cần thông tri )
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersChangeCode(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int bodyCode, int weaponCode, int refreshNow)
        //{
        //    List<Object> objsList = Global.GetAll9Clients(client);
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, bodyCode, weaponCode, refreshNow);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGCODE);
        //}

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

            //群发消息
            SendToClients(sl, pool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_CHGCODE);
        }

        #endregion Nhân vật thay đổi trang phục

        #region 角色PK模式

        /// <summary>
        /// PK模式变化通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersPKModeChanged(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.PKMode);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPKMODE);
        }

        #endregion 角色PK模式



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
            // 判空
            if (null == client)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("client不存在，无法给与新手任务"));
                return false;
            }

            // Trao cho nhân vật nhiệm vụ đầu tiên
            int nRoleID = client.RoleID;

            try
            {
                // 给与魔剑士新手任务 [XSea 2015/4/9]
                //                if (null == Global.GetTaskData(client, MagicSwordData.InitTaskID)
                //                    && GameManager.MagicSwordMgr.IsFirstLoginMagicSword(client, MagicSwordData.InitChangeLifeCount))
                //                {
                //                    // 循环将魔剑士初始任务以前的任务标记为已完成
                //                    int tmpRes = GameManager.ClientMgr.AutoCompletionTaskByTaskID(tcpMgr, tcpClientPool, pool, tcpRandKey, client, MagicSwordData.InitPrevTaskID);

                //                    // 失败
                //                    if (tmpRes != 0)
                //                    {
                //                        LogManager.WriteLog(LogTypes.Error, string.Format("魔剑士任务初始化失败，无法创建魔剑士, RoleID={0}", nRoleID));
                //                        return false;
                //                    }

                //                    // 这里的 需要等以前任务标记初始化完成才可以执行
                //                    client.MainTaskID = MagicSwordData.InitPrevTaskID; // 上一个完成的任务

                //                    // 将新手魔剑士放到专属场景
                //                    client.MapCode = MagicSwordData.InitMapID;

                //                    TCPOutPacket tcpOutPacketTemp = null;
                //                    // 给与新手任务
                //                    Global.TakeNewTask(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, (int)TCPGameServerCmds.CMD_SPR_NEWTASK, client, nRoleID,
                //                        MagicSwordData.InitTaskID, MagicSwordData.InitTaskNpcID, out tcpOutPacketTemp);
                //                }
                //#if 移植
                //                // 给与召唤师新手任务
                //                if (null == Global.GetTaskData(client, SummonerData.InitTaskID)
                //                    && GameManager.SummonerMgr.IsFirstLoginSummoner(client, SummonerData.InitChangeLifeCount))
                //                {
                //                    // 循环将召唤师初始任务以前的任务标记为已完成
                //                    int tmpRes = GameManager.ClientMgr.AutoCompletionTaskByTaskID(tcpMgr, tcpClientPool, pool, tcpRandKey, client, SummonerData.InitPrevTaskID);

                //                    // 失败
                //                    if (tmpRes != 0)
                //                    {
                //                        LogManager.WriteLog(LogTypes.Error, string.Format("魔剑士任务初始化失败，无法创建魔剑士, RoleID={0}", nRoleID));
                //                        return false;
                //                    }

                //                    // 这里的 需要等以前任务标记初始化完成才可以执行
                //                    client.MainTaskID = SummonerData.InitPrevTaskID; // 上一个完成的任务

                //                    // 将新手魔剑士放到专属场景
                //                    client.MapCode = SummonerData.InitMapID;

                //                    TCPOutPacket tcpOutPacketTemp = null;
                //                    // 给与新手任务
                //                    Global.TakeNewTask(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, (int)TCPGameServerCmds.CMD_SPR_NEWTASK, client, nRoleID,
                //                        SummonerData.InitTaskID, SummonerData.InitTaskNpcID, out tcpOutPacketTemp);
                //                }
                //#endif
                //                // 给战士、法师、弓箭手新手任务
                //                else if (bNeedTakeStartTask && null == Global.GetTaskData(client, 1000) && !GameManager.MagicSwordMgr.IsMagicSword(client))
                //                {
                //                    // 新手场景id
                //                    client.MainTaskID = 106;
                //                    TCPOutPacket tcpOutPacketTemp = null;
                //                    Global.AddOldTask(client, 106);
                //                    Global.TakeNewTask(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, (int)TCPGameServerCmds.CMD_SPR_NEWTASK, client, nRoleID, 1000, 60900, out tcpOutPacketTemp);
                //                }

                return true;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
                //throw ex;
                //});
            }
            return false;
        }

        #endregion 任务相关

        #region 通知客户端角色的数值属性

        /// <summary>
        /// 获取属性字符串
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //private string GetEquipPropsStr(KPlayer client)
        private EquipPropsData GetEquipPropsStr(KPlayer client)
        {
            /// SOI COMMENT
            if (true)
            {
                return new EquipPropsData();
            }

            // 属性改造 给客户端显示的内容 重新组织下 [8/15/2013 LiaoWei]
            // 0. 人物ROLEID 1.力量 2.智力 3.体力 4.敏捷 5.最小物理攻击力 6.最大物理攻击力 7.最小物理防御 8.最大物理防御 9.魔法技能增幅 10.最小魔法攻击力
            // 11.最大魔法攻击力 12.最小魔法防御力 13.最大魔法防御力 14. 物理技能增幅 15.生命上限 16.魔法上限 17.攻击速度 18.命中 19.闪避 20.总属性点 21.转生计数, 22.战斗力

            client.propsCacheModule.ResetAllProps();
            AdvanceBufferPropsMgr.DoSpriteBuffers(client);

            double nMinAttack = RoleAlgorithm.GetMinAttackV(client);

            double nMaxAttack = RoleAlgorithm.GetMaxAttackV(client);
            //LogManager.WriteLog(LogTypes.Error, string.Format("--------------nMaxAttack={0}", nMaxAttack));
            double nMinDefense = RoleAlgorithm.GetMinADefenseV(client);

            double nMaxDefense = RoleAlgorithm.GetMaxADefenseV(client);

            double nMinMAttack = RoleAlgorithm.GetMinMagicAttackV(client);

            double nMaxMAttack = RoleAlgorithm.GetMaxMagicAttackV(client);

            double nMinMDefense = RoleAlgorithm.GetMinMDefenseV(client);

            double nMaxMDefense = RoleAlgorithm.GetMaxMDefenseV(client);

            double nHit = RoleAlgorithm.GetHitV(client);

            double nDodge = RoleAlgorithm.GetDodgeV(client);

            double addAttackInjure = RoleAlgorithm.GetAddAttackInjureValue(client);

            double decreaseInjure = RoleAlgorithm.GetDecreaseInjureValue(client);

            double nMaxHP = client.m_CurrentLifeMax;

            double nMaxMP = client.m_CurrentManaMax;

            double nLifeSteal = RoleAlgorithm.GetLifeStealV(client);

            // add元素属性战斗力 [XSea 2015/8/24]
            double dFireAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Fire);
            double dWaterAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Water);
            double dLightningAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Lightning);
            double dSoilAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Soil);
            double dIceAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Ice);
            double dWindAttack = GameManager.ElementsAttackMgr.GetElementAttack(client, EElementDamageType.EEDT_Wind);

            // 战斗力 [12/17/2013 LiaoWei]  改成一项了 [3/5/2014 LiaoWei]
            //int nOccup = Global.CalcOriginalOccupationID(client);

            int nStrength = DBRoleBufferManager.GetTimeAddProp(client, BufferItemTypes.ADDTEMPStrength);
            int nIntelligence = DBRoleBufferManager.GetTimeAddProp(client, BufferItemTypes.ADDTEMPIntelligsence);
            int nDexterity = DBRoleBufferManager.GetTimeAddProp(client, BufferItemTypes.ADDTEMPDexterity);
            int nConstitution = DBRoleBufferManager.GetTimeAddProp(client, BufferItemTypes.ADDTEMPConstitution);

            int addStrength = (int)RoleAlgorithm.GetStrength(client, false);
            int addIntelligence = (int)RoleAlgorithm.GetIntelligence(client, false);
            int addDexterity = (int)RoleAlgorithm.GetDexterity(client, false);
            int addConstitution = (int)RoleAlgorithm.GetConstitution(client, false);

            int addAll = addStrength + addIntelligence + addDexterity + addConstitution;

            EquipPropsData equipPropsData = new EquipPropsData()
            {
                RoleID = client.RoleID,
                Strength = addStrength + nStrength,          // 1
                Intelligence = addIntelligence + nIntelligence,      // 2
                Dexterity = addDexterity + nDexterity,         // 3
                Constitution = addConstitution + nConstitution,      // 4
                MinAttack = nMinAttack,                                 // 5
                MaxAttack = nMaxAttack,                                 // 6
                MinDefense = nMinDefense,                                // 7
                MaxDefense = nMaxDefense,                                // 8
                MagicSkillIncrease = RoleAlgorithm.GetMagicSkillIncrease(client),// 9
                MinMAttack = nMinMAttack,                                // 10
                MaxMAttack = nMaxMAttack,                                // 11
                MinMDefense = nMinMDefense,                               // 12
                MaxMDefense = nMaxMDefense,                               // 13
                PhySkillIncrease = RoleAlgorithm.GetPhySkillIncrease(client),  // 14
                MaxHP = nMaxHP,                                     // 15
                MaxMP = nMaxMP,                                     // 16
                AttackSpeed = RoleAlgorithm.GetAttackSpeed(client),       // 17
                Hit = nHit,                                       // 18
                Dodge = nDodge,                                     // 19
                //TotalPropPoint = Global.GetRoleParamsInt32FromDB(client, RoleParamName.TotalPropPoint),// 20
                ChangeLifeCount = client.ChangeLifeCount,          // 21

                TEMPStrength = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropStrengthChangeless),              // 23
                TEMPIntelligsence = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropIntelligenceChangeless),     // 24
                TEMPDexterity = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropDexterityChangeless),            // 25
                TEMPConstitution = Global.GetRoleParamsInt32FromDB(client, RoleParamName.sPropConstitutionChangeless),       // 26
            };

            //equipPropsData.TotalPropPoint += nStrength + nIntelligence + nDexterity + nConstitution;
            //equipPropsData.TotalPropPoint += (int)client.PropsCacheManager.GetBaseProp((int)UnitPropIndexes.Str)
            //    + (int)client.PropsCacheManager.GetBaseProp((int)UnitPropIndexes.Int)
            //    + (int)client.PropsCacheManager.GetBaseProp((int)UnitPropIndexes.Dex)
            //    + (int)client.PropsCacheManager.GetBaseProp((int)UnitPropIndexes.Sta);
#if 移植
            equipPropsData.Toughness = client.PropsCacheManager.GetExtProp((int)ExtPropIndexes.Toughness);
#endif
            //return strcmd;
            return equipPropsData;
        }

        /// <summary>
        /// 装备属性更新通知
        /// </summary>
        public void NotifyUpdateEquipProps(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            //string strcmd = GetEquipPropsStr(client);
            //TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETATTRIB2);

            EquipPropsData equipPropsData = GetEquipPropsStr(client);
            byte[] bytes = DataHelper.ObjectToBytes<EquipPropsData>(equipPropsData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_GETATTRIB2);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// 重量属性更新通知
        /// </summary>
        public void NotifyUpdateWeights(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            // 属性改造 去掉 重量属性[8/15/2013 LiaoWei]
            /* string strcmd = StringUtil.substitute("{0}:{1}:{2}:{3}",
                 client.RoleID,
                 client.WeighItems.Weights[(int)WeightIndexes.HandWeight],
                 client.WeighItems.Weights[(int)WeightIndexes.BagWeight],
                 client.WeighItems.Weights[(int)WeightIndexes.DressWeight]
                 );

             TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_UPDATEWEIGHTS);
             if (!sl.SendData(client.ClientSocket, tcpOutPacket))
             {
                 //
                 /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                     tcpOutPacket.PacketCmdID,
                     tcpOutPacket.PacketDataSize,
                     client.RoleID,
                     client.RoleName));*/
            //}*/
        }

        /// <summary>
        /// 装备属性更新通知
        /// </summary>
        public void NotifyUpdateEquipProps(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer otherClient)
        {
            //
            //string strcmd = GetEquipPropsStr(otherClient);
            //TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETATTRIB2);

            EquipPropsData equipPropsData = GetEquipPropsStr(otherClient);
            byte[] bytes = DataHelper.ObjectToBytes<EquipPropsData>(equipPropsData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_GETATTRIB2);

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

        #endregion 通知客户端角色的数值属性

        #region 角色加减血

        /// <summary>
        /// 给某个客户端加血
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="addedVal"></param>
        public void AddSpriteLifeV(SocketListener sl, TCPOutPacketPool pool, KPlayer c, double lifeV, string reason)
        {
            //如果已经死亡，则不再调度
            if (c.m_CurrentLife <= 0)
            {
                return;
            }

            //判断如果血量少于最大血量
            if (c.m_CurrentLife < c.m_CurrentLifeMax)
            {
                RoleRelifeLog relifeLog = new RoleRelifeLog(c.RoleID, c.RoleName, c.MapCode, reason);
                relifeLog.hpModify = true;
                relifeLog.oldHp = c.m_CurrentLife;
                c.m_CurrentLife = (int)Global.GMin(c.m_CurrentLifeMax, c.m_CurrentLife + lifeV);
                relifeLog.newHp = c.m_CurrentLife;
                MonsterAttackerLogManager.Instance().AddRoleRelifeLog(relifeLog);
                //GameManager.SystemServerEvents.AddEvent(string.Format("角色加血, roleID={0}({1}), Add={2}, Life={3}", c.RoleID, c.RoleName, lifeV, c.m_CurrentLife), EventLevels.Debug);

                //通知客户端怪已经加血加魔
                List<Object> listObjs = Global.GetAll9Clients(c);
                GameManager.ClientMgr.NotifyOthersRelife(sl, pool, c, c.MapCode, c.CopyMapID, c.RoleID, (int)c.PosX, (int)c.PosY, (int)c.RoleDirection, c.m_CurrentLife, c.m_CurrentMana, c.m_CurrentStamina, (int)TCPGameServerCmds.CMD_SPR_RELIFE, listObjs);
            }
        }

        /// <summary>
        /// 给某个客户端减血
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="addedVal"></param>
        public void SubSpriteLifeV(SocketListener sl, TCPOutPacketPool pool, KPlayer c, double lifeV)
        {
            //如果已经死亡，则不再调度
            if (c.m_CurrentLife <= 0)
            {
                return;
            }

            c.m_CurrentLife = (int)Global.GMax(0.0, c.m_CurrentLife - lifeV);
            //GameManager.SystemServerEvents.AddEvent(string.Format("角色减血, roleID={0}({1}), Sub={2}, Life={3}", c.RoleID, c.RoleName, lifeV, c.m_CurrentLife), EventLevels.Debug);

            //通知客户端怪已经加血加魔
            List<Object> listObjs = Global.GetAll9Clients(c);
            GameManager.ClientMgr.NotifyOthersRelife(sl, pool, c, c.MapCode, c.CopyMapID, c.RoleID, (int)c.PosX, (int)c.PosY, (int)c.RoleDirection, c.m_CurrentLife, c.m_CurrentMana, c.m_CurrentStamina, (int)TCPGameServerCmds.CMD_SPR_RELIFE, listObjs);
        }

        #endregion 角色加减血

        #region 角色加减魔

        /// <summary>
        /// 给某个客户端加魔
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="addedVal"></param>
        public void AddSpriteMagicV(SocketListener sl, TCPOutPacketPool pool, KPlayer c, double magicV, string reason)
        {
            //如果已经死亡，则不再调度
            if (c.m_CurrentLife <= 0)
            {
                return;
            }

            //判断如果魔量少于最大魔量
            if (c.m_CurrentMana < c.m_CurrentManaMax)
            {
                RoleRelifeLog relifeLog = new RoleRelifeLog(c.RoleID, c.RoleName, c.MapCode, reason);
                relifeLog.mpModify = true;
                relifeLog.oldMp = c.m_CurrentMana;
                c.m_CurrentMana = (int)Global.GMin(c.m_CurrentManaMax, c.m_CurrentMana + magicV);
                relifeLog.newMp = c.m_CurrentMana;

                MonsterAttackerLogManager.Instance().AddRoleRelifeLog(relifeLog);
                //GameManager.SystemServerEvents.AddEvent(string.Format("角色加魔, roleID={0}({1}), Add={2}, Magic={3}", c.RoleID, c.RoleName, magicV, c.m_CurrentMana), EventLevels.Debug);

                //通知客户端怪已经加血加魔
                List<Object> listObjs = Global.GetAll9Clients(c);
                GameManager.ClientMgr.NotifyOthersRelife(sl, pool, c, c.MapCode, c.CopyMapID, c.RoleID, (int)c.PosX, (int)c.PosY, (int)c.RoleDirection, c.m_CurrentLife, c.m_CurrentMana, c.m_CurrentStamina, (int)TCPGameServerCmds.CMD_SPR_RELIFE, listObjs);
            }
        }

        /// <summary>
        /// 给某个客户端减魔
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="addedVal"></param>
        public void SubSpriteMagicV(SocketListener sl, TCPOutPacketPool pool, KPlayer c, double magicV)
        {
           

            if (c.m_CurrentLife <= 0)
            {
                return;
            }

            c.m_CurrentMana = (int)Global.GMax(0.0, c.m_CurrentMana - magicV);

            //GameManager.SystemServerEvents.AddEvent(string.Format("角色减魔, roleID={0}({1}), Sub={2}, Magic={3}", c.RoleID, c.RoleName, magicV, c.m_CurrentMana), EventLevels.Debug);

            //通知客户端怪已经加血加魔
            List<Object> listObjs = Global.GetAll9Clients(c);
            GameManager.ClientMgr.NotifyOthersRelife(sl, pool, c, c.MapCode, c.CopyMapID, c.RoleID, (int)c.PosX, (int)c.PosY, (int)c.RoleDirection, c.m_CurrentLife, c.m_CurrentMana, c.m_CurrentStamina, (int)TCPGameServerCmds.CMD_SPR_RELIFE, listObjs);
        }

        #endregion 角色加减魔

        #region 宠物相关

        /// <summary>
        /// 通知角色宠物的指令信息(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyPetCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int petType, int extTag1, string extTag2, List<Object> objsList)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", status, client.RoleID, petType, extTag1, extTag2);

            if (null == objsList)
            {
                TCPOutPacket tcpOutPacket = null;
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_PET);
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
            else
            {
                //群发消息
                SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_PET);
            }
        }

        #endregion 宠物相关

        #region 坐骑相关

        /// <summary>
        /// 通知角色骑乘的指令信息(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyHorseCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int horseType, int horseDbID, int horseID, int horseBodyID, List<Object> objsList)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", status, client.RoleID, horseType, horseDbID, horseID, horseBodyID);

            if (null == objsList)
            {
                TCPOutPacket tcpOutPacket = null;
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_HORSE);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                }
            }
            else
            {
                //群发消息
                SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_HORSE);
            }
        }

        /// <summary>
        /// Notify ngựa về client
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>

        public void NotifySelfOnHorse(KPlayer client)
        {
            //通知骑乘的的指令信息
            //if (client.HorseDbID > 0)
            //{
            //    HorseData horseData = Global.GetHorseDataByDbID(client, client.HorseDbID);
            //    if (null != horseData)
            //    {
            //        //计算坐骑的积分值
            //        client.RoleHorseJiFen = Global.CalcHorsePropsJiFen(horseData);

            //        GameManager.ClientMgr.NotifyHorseCmd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, 0,
            //            (int)HorseCmds.On, horseData.DbID, horseData.HorseID, horseData.BodyID, null);
            //    }
            //}
        }

        /// <summary>
        /// Notify ngựa cho người chơi khác
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfOtherHorse(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer otherClient)
        {
            if (null == otherClient) return;
            if (client.RoleID == otherClient.RoleID) //跳过自己
            {
                return;
            }
        }

        #endregion 坐骑相关

        #region 经脉相关

        /// <summary>
        /// 通知角色结束冲穴状态的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyEndChongXueCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}", client.RoleID);
            TCPOutPacket tcpOutPacket = null;
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYENDCHONGXUE);
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

        #endregion 经脉相关

        #region 好友和敌人相关

        /// <summary>
        /// 删除时间最早的敌人
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

            //查找第一符合指定类型的队列
            FriendData friendData = Global.FindFirstFriendDataByType(client, 2);
            if (null == friendData)
            {
                return true;
            }

            //删除好友、黑名单、仇人列表
            return GameManager.ClientMgr.RemoveFriend(tcpMgr, tcpClientPool, pool, client, friendData.DbID);
        }

        /// <summary>
        /// 加入到敌人列表中
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="friendType"></param>
        public void AddToEnemyList(TCPManager tcpMgr, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int killedRoleID)
        {
            //竞技场 和 炎黄战场中，不记忆仇人
            if (client.MapCode == GameManager.BattleMgr.BattleMapCode
                || client.MapCode == GameManager.ArenaBattleMgr.BattleMapCode)
            {
                return;
            }

            KPlayer findClient = FindClient(killedRoleID);
            if (null == findClient)
            {
                return;
            }

            //先判断是否先删除
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
                    StringUtil.substitute(Global.GetLang("您的仇人列表已经满, 最多不能超过{0}个"),
                    (int)FriendsConsts.MaxEnemiesNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);

                return;
            }

            //shizhu added: 被好友击杀不加入到仇人列表
            if (friendData == null || (friendData.FriendType != 0 && friendData.FriendType != 2))
            {
                AddFriend(tcpMgr, tcpClientPool, pool, findClient, friendDbID, client.RoleID, Global.FormatRoleName(client, client.RoleName), 2);
            }
        }

        /// <summary>
        /// 删除好友
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

                TCPOutPacket tcpOutPacket = null;
                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, client.ClientSocket, tcpClientPool, null, pool, (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND, bytesCmd, bytesCmd.Length, out tcpOutPacket, client.ServerId);
                if (TCPProcessCmdResults.RESULT_FAILED != result)
                {
                    //处理本地精简的好友数据
                    string strData = new UTF8Encoding().GetString(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);

                    //解析客户端的指令
                    string[] fields = strData.Split(':');
                    if (fields.Length == 3 && Convert.ToInt32(fields[2]) >= 0)
                    {
                        Global.RemoveFriendData(client, dbID);
                    }

                    ret = true;
                }

                //发送消息给客户端
                if (!tcpMgr.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return ret;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
                //throw ex;
                //});
            }

            return ret;
        }

        /// <summary>
        /// 加入朋友列表中
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

            //禁止将自己加入仇人列表
            if (friendType == 2 && otherRoleID == client.RoleID)
            {
                return false;
            }

            try
            {
                FriendData friendData = null;
                if (otherRoleID > 0)
                {
                    friendData = Global.FindFriendData(client, otherRoleID);
                    if (null != friendData)
                    {
                        if (friendData.FriendType == friendType) //已经存在
                        {
                            return ret;
                        }
                    }
                }

                //判断是否数量已经满了
                int friendTypeCount = Global.GetFriendCountByType(client, friendType);
                if (0 == friendType) //好友
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxFriendsNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("您的好友列表已经满, 最多不能超过{0}个"), (int)FriendsConsts.MaxFriendsNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return ret;
                    }
                }
                else if (1 == friendType) //黑名单
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxBlackListNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("您的黑名单列表已经满, 最多不能超过{0}个"), (int)FriendsConsts.MaxBlackListNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return ret;
                    }
                }
                else if (2 == friendType) //仇人
                {
                    if (friendTypeCount >= (int)FriendsConsts.MaxEnemiesNum)
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, StringUtil.substitute(Global.GetLang("您的仇人列表已经满, 最多不能超过{0}个"), (int)FriendsConsts.MaxEnemiesNum), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
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

                //处理本地精简的好友列表数据
                friendData = DataHelper.BytesToObject<FriendData>(tcpOutPacket.GetPacketBytes(), 6, tcpOutPacket.PacketDataSize - 6);
                if (null != friendData && friendData.DbID >= 0)
                {
                    ret = true;

                    Global.RemoveFriendData(client, friendData.DbID); //防止加入重复数据
                    Global.AddFriendData(client, friendData);

                    if (0 == friendType) //好友
                    {
                        friendTypeCount = Global.GetFriendCountByType(client, friendType);
                        if (1 == friendTypeCount)
                        {
                            //成就相关---第一次拥有了一个好友
                            ChengJiuManager.OnFirstAddFriend(client);
                        }
                    }

                    //通知对方自己将他加为了好友
                    //查看用户是否在本服务器上，如果没有，则查询从其他服务器查询，并且转发给自己的用户(只针对当前服务器，不转发)
                    KPlayer otherClient = GameManager.ClientMgr.FindClient(friendData.OtherRoleID);
                    if (null != otherClient)
                    {
                        if (friendData.FriendType == 0)
                        {
                            string typeName = Global.GetLang("好友");
                            /*if (friendData.FriendType == 1)
                            {
                                typeName = Global.GetLang("黑名单");
                            }
                            else if (friendData.FriendType == 2)
                            {
                                typeName = Global.GetLang("仇人");
                            }*/

                            /// 通知在线的对方(不限制地图)个人紧要消息
                            GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, otherClient, StringUtil.substitute(Global.GetLang("【{0}】将您加入了{1}列表"), Global.FormatRoleName(client, client.RoleName), typeName), GameInfoTypeIndexes.Normal, ShowGameInfoTypes.ErrAndBox);
                        }
                    }
                }

                //发送消息给客户端
                if (!tcpMgr.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return ret;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(client.ClientSocket), false);
                //throw ex;
                //});
            }
            return ret;
        }

        #endregion 好友和敌人相关

        #region 点将台相关

        /// <summary>
        /// 通知点将台房间数据的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDianJiangData(SocketListener sl, TCPOutPacketPool pool, DJRoomData roomData)
        {
            if (null != roomData)
            {
                byte[] bytesData = null;
                lock (roomData)
                {
                    bytesData = DataHelper.ObjectToBytes<DJRoomData>(roomData);
                }

                if (null != bytesData && bytesData.Length > 0)
                {
                    TCPOutPacket tcpOutPacket = null;
                    int index = 0;
                    KPlayer client = null;
                    while ((client = GetNextClient(ref index)) != null)
                    {
                        if (!client.ViewDJRoomDlg)
                        {
                            continue;
                        }

                        tcpOutPacket = pool.Pop();
                        tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_DIANJIANGDATA;
                        tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

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
                }
            }
        }

        /// <summary>
        /// 通知点将台房间成员数据的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDJRoomRolesData(SocketListener sl, TCPOutPacketPool pool, DJRoomRolesData djRoomRolesData)
        {
            if (null != djRoomRolesData)
            {
                lock (djRoomRolesData)
                {
                    byte[] bytesData = DataHelper.ObjectToBytes<DJRoomRolesData>(djRoomRolesData);

                    TCPOutPacket tcpOutPacket = null;
                    for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                    {
                        KPlayer client = FindClient(djRoomRolesData.Team1[i].RoleID);
                        if (null == client) continue;

                        tcpOutPacket = pool.Pop();
                        tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_DJROOMROLESDATA;
                        tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

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

                    for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                    {
                        KPlayer client = FindClient(djRoomRolesData.Team2[i].RoleID);
                        if (null == client) continue;

                        tcpOutPacket = pool.Pop();
                        tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_DJROOMROLESDATA;
                        tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

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
                }
            }
        }

        /// <summary>
        /// 通知角色点将台的指令信息(所有打开了点将台窗口的都通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDianJiangCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int djCmdType, int extTag1, string extTag2, bool allSend = false)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", status, client.RoleID, djCmdType, extTag1, extTag2);

            if (!allSend)
            {
                TCPOutPacket tcpOutPacket = null;
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANG);
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
            else
            {
                int index = 0;
                client = null;
                TCPOutPacket tcpOutPacket = null;
                try
                {
                    while ((client = GetNextClient(ref index)) != null)
                    {
                        if (!client.ViewDJRoomDlg)
                        {
                            continue;
                        }

                        if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANG);
                        if (!sl.SendData(client.ClientSocket, tcpOutPacket, false))
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
                finally
                {
                    PushBackTcpOutPacket(tcpOutPacket);
                }
            }
        }

        /// <summary>
        /// 销毁点将台房间
        /// </summary>
        public int DestroyDianJiangRoom(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.DJRoomID <= 0)
            {
                return -1;
            }

            //查找房间数据
            DJRoomData djRoomData = GameManager.DJRoomMgr.FindRoomData(client.DJRoomID);
            if (null == djRoomData)
            {
                return -2;
            }

            //判断自己是否是房间的创建者
            if (djRoomData.CreateRoleID != client.RoleID)
            {
                return -3;
            }

            //判断房间的是否已经开始了战斗，开始了则无法直接删除了
            lock (djRoomData)
            {
                if (djRoomData.PKState > 0)
                {
                    return -4;
                }
            }

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(client.DJRoomID);
            if (null == djRoomRolesData)
            {
                return -5;
            }

            int roomID = client.DJRoomID;

            //从内存中清空
            GameManager.DJRoomMgr.RemoveRoomData(roomID);
            GameManager.DJRoomMgr.RemoveRoomRolesData(roomID);

            lock (djRoomRolesData)
            {
                djRoomRolesData.Removed = 1;

                for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                {
                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                    if (null != gc)
                    {
                        gc.DJRoomID = -1;
                        gc.DJRoomTeamID = -1;
                        gc.HideSelf = 0;
                    }
                }

                for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                {
                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                    if (null != gc)
                    {
                        gc.DJRoomID = -1;
                        gc.DJRoomTeamID = -1;
                        gc.HideSelf = 0;
                    }
                }
            }

            //发送错误信息
            GameManager.ClientMgr.NotifyDianJiangCmd(sl, pool, client, 0, (int)DianJiangCmds.RemoveRoom, roomID, "", true);
            return 0;
        }

        /// <summary>
        /// 离开点将台房间
        /// </summary>
        public int LeaveDianJiangRoom(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.DJRoomID <= 0)
            {
                return -1;
            }

            //查找房间数据
            DJRoomData djRoomData = GameManager.DJRoomMgr.FindRoomData(client.DJRoomID);
            if (null == djRoomData)
            {
                return -2;
            }

            //判断自己是否是房间的创建者
            if (djRoomData.CreateRoleID == client.RoleID)
            {
                return -3;
            }

            //判断房间的是否已经开始了战斗，开始了则无法直接删除了
            lock (djRoomData)
            {
                if (djRoomData.PKState > 0)
                {
                    return -4;
                }
            }

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(client.DJRoomID);
            if (null == djRoomRolesData)
            {
                return -5;
            }

            int roomID = client.DJRoomID;

            bool found = false;
            lock (djRoomRolesData)
            {
                if (djRoomRolesData.Removed > 0)
                {
                    return -6;
                }

                if (djRoomRolesData.Locked > 0)
                {
                    return -7;
                }

                for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                {
                    if (client.RoleID == djRoomRolesData.Team1[i].RoleID)
                    {
                        found = true;
                        djRoomRolesData.Team1.RemoveAt(i);
                        break;
                    }
                }

                if (!found)
                {
                    for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                    {
                        if (client.RoleID == djRoomRolesData.Team2[i].RoleID)
                        {
                            found = true;
                            djRoomRolesData.Team2.RemoveAt(i);
                            break;
                        }
                    }
                }

                djRoomRolesData.TeamStates.Remove(client.RoleID);
                djRoomRolesData.RoleStates.Remove(client.RoleID);
            }

            if (found)
            {
                lock (djRoomData)
                {
                    djRoomData.PKRoleNum--;
                }
            }

            client.DJRoomID = -1;
            client.DJRoomTeamID = -1;
            client.HideSelf = 0;

            //发送房间数据
            GameManager.ClientMgr.NotifyDianJiangData(sl, pool, djRoomData);

            //通知点将台房间成员数据的指令信息
            GameManager.ClientMgr.NotifyDJRoomRolesData(sl, pool, djRoomRolesData);

            //发送信息
            GameManager.ClientMgr.NotifyDianJiangCmd(sl, pool, client, 0, (int)DianJiangCmds.LeaveRoom, roomID, Global.FormatRoleName(client, client.RoleName), true);
            return 0;
        }

        /// <summary>
        /// 通知点将台房间内的玩家传动到点将台地图
        /// </summary>
        public int TransportDianJiangRoom(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.DJRoomID <= 0)
            {
                return -1;
            }

            //查找房间数据
            DJRoomData djRoomData = GameManager.DJRoomMgr.FindRoomData(client.DJRoomID);
            if (null == djRoomData)
            {
                return -2;
            }

            //判断自己是否是房间的创建者
            if (djRoomData.CreateRoleID != client.RoleID)
            {
                return -3;
            }

            //判断房间的是否已经开始了战斗，开始了则无法直接删除了
            lock (djRoomData)
            {
                if (djRoomData.PKState <= 0)
                {
                    return -4;
                }
            }

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(client.DJRoomID);
            if (null == djRoomRolesData)
            {
                return -5;
            }

            lock (djRoomRolesData)
            {
                if (djRoomRolesData.Locked <= 0)
                {
                    return -6;
                }

                for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                {
                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                    if (null != gc)
                    {
                        GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                            gc, Global.DianJiangTaiMapCode, -1, -1, -1);
                    }
                }

                for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                {
                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                    if (null != gc)
                    {
                        GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                            gc, Global.DianJiangTaiMapCode, -1, -1, -1);
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 通知角色点将台房间内战斗的指令信息(参战者，观众)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDianJiangFightCmd(SocketListener sl, TCPOutPacketPool pool, DJRoomData djRoomData, int djCmdType, string extTag2, KPlayer toClient = null)
        {
            if (null == djRoomData) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}", djRoomData.RoomID, djCmdType, 0, extTag2);

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(djRoomData.RoomID);
            if (null == djRoomRolesData)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;

            if (null != toClient)
            {
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGFIGHT);
                if (!sl.SendData(toClient.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        toClient.RoleID,
                        toClient.RoleName));*/
                }

                return;
            }

            lock (djRoomRolesData)
            {
                tcpOutPacket = null;
                try
                {
                    for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                        if (null != gc)
                        {
                            if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGFIGHT);
                            if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                            {
                                //
                                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                    tcpOutPacket.PacketCmdID,
                                    tcpOutPacket.PacketDataSize,
                                    gc.RoleID,
                                    gc.RoleName));*/
                            }
                        }
                    }

                    for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                        if (null != gc)
                        {
                            if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGFIGHT);
                            if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                            {
                                //
                                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                    tcpOutPacket.PacketCmdID,
                                    tcpOutPacket.PacketDataSize,
                                    gc.RoleID,
                                    gc.RoleName));*/
                            }
                        }
                    }
                }
                finally
                {
                    PushBackTcpOutPacket(tcpOutPacket);
                }

                tcpOutPacket = null;
                try
                {
                    if (null != djRoomRolesData.ViewRoles)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}", djRoomData.RoomID, djCmdType, 1, extTag2);
                        for (int i = 0; i < djRoomRolesData.ViewRoles.Count; i++)
                        {
                            KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.ViewRoles[i].RoleID);
                            if (null != gc)
                            {
                                if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGFIGHT);
                                if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                                {
                                    //
                                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                        tcpOutPacket.PacketCmdID,
                                        tcpOutPacket.PacketDataSize,
                                        gc.RoleID,
                                        gc.RoleName));*/
                                }
                            }
                        }
                    }
                }
                finally
                {
                    PushBackTcpOutPacket(tcpOutPacket);
                }
            }
        }

        /// <summary>
        /// 通知角色点将台房间内战斗的指令信息(参战者，观众)离开离开场景消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDJFightRoomLeaveMsg(SocketListener sl, TCPOutPacketPool pool, DJRoomData djRoomData)
        {
            if (null == djRoomData) return;

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(djRoomData.RoomID);
            if (null == djRoomRolesData)
            {
                return;
            }

            lock (djRoomRolesData)
            {
                for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                {
                    int state = 0;
                    djRoomRolesData.RoleStates.TryGetValue(djRoomRolesData.Team1[i].RoleID, out state);
                    if (1 != state) //不再当前地图
                    {
                        continue;
                    }

                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                    if (null != gc)
                    {
                        GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                            gc, gc.LastMapCode, gc.LastPosX, gc.LastPosY, -1);
                    }
                }

                for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                {
                    int state = 0;
                    djRoomRolesData.RoleStates.TryGetValue(djRoomRolesData.Team2[i].RoleID, out state);
                    if (1 != state) //不再当前地图
                    {
                        continue;
                    }

                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                    if (null != gc)
                    {
                        GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                            gc, gc.LastMapCode, gc.LastPosX, gc.LastPosY, -1);
                    }
                }

                if (null != djRoomRolesData.ViewRoles)
                {
                    for (int i = 0; i < djRoomRolesData.ViewRoles.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.ViewRoles[i].RoleID);
                        if (null != gc)
                        {
                            GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                                gc, gc.LastMapCode, gc.LastPosX, gc.LastPosY, -1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送点将台房间的战斗结果
        /// </summary>
        public void NotifyDianJiangRoomRolesPoint(SocketListener sl, TCPOutPacketPool pool, DJRoomRolesPoint djRoomRolesPoint)
        {
            if (null == djRoomRolesPoint) return;

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(djRoomRolesPoint.RoomID);
            if (null == djRoomRolesData)
            {
                return;
            }

            byte[] bytesData = DataHelper.ObjectToBytes<DJRoomRolesPoint>(djRoomRolesPoint);
            if (null == bytesData)
            {
                return;
            }

            TCPOutPacket tcpOutPacket = null;
            lock (djRoomRolesData)
            {
                try
                {
                    for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                        if (null != gc)
                        {
                            if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytesData, 0, bytesData.Length, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGPOINT);
                            if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                            {
                                //
                                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                    tcpOutPacket.PacketCmdID,
                                    tcpOutPacket.PacketDataSize,
                                    gc.RoleID,
                                    gc.RoleName));*/
                            }
                        }
                    }

                    for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                        if (null != gc)
                        {
                            if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytesData, 0, bytesData.Length, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGPOINT);
                            if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                            {
                                //
                                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                    tcpOutPacket.PacketCmdID,
                                    tcpOutPacket.PacketDataSize,
                                    gc.RoleID,
                                    gc.RoleName));*/
                            }
                        }
                    }

                    if (null != djRoomRolesData.ViewRoles)
                    {
                        for (int i = 0; i < djRoomRolesData.ViewRoles.Count; i++)
                        {
                            KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.ViewRoles[i].RoleID);
                            if (null != gc)
                            {
                                if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytesData, 0, bytesData.Length, (int)TCPGameServerCmds.CMD_SPR_DIANJIANGPOINT);
                                if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                                {
                                    //
                                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                        tcpOutPacket.PacketCmdID,
                                        tcpOutPacket.PacketDataSize,
                                        gc.RoleID,
                                        gc.RoleName));*/
                                }
                            }
                        }
                    }
                }
                finally
                {
                    PushBackTcpOutPacket(tcpOutPacket);
                }
            }
        }

        /// <summary>
        /// 删除点将台房间
        /// </summary>
        public void RemoveDianJiangRoom(SocketListener sl, TCPOutPacketPool pool, DJRoomData djRoomData)
        {
            if (null == djRoomData) return;

            //查找房间角色数据
            DJRoomRolesData djRoomRolesData = GameManager.DJRoomMgr.FindRoomRolesData(djRoomData.RoomID);
            if (null == djRoomRolesData)
            {
                return;
            }

            int roomID = djRoomData.RoomID;

            //从内存中清空
            GameManager.DJRoomMgr.RemoveRoomData(roomID);
            GameManager.DJRoomMgr.RemoveRoomRolesData(roomID);

            lock (djRoomRolesData)
            {
                djRoomRolesData.Removed = 1;

                for (int i = 0; i < djRoomRolesData.Team1.Count; i++)
                {
                    int state = 0;
                    djRoomRolesData.RoleStates.TryGetValue(djRoomRolesData.Team1[i].RoleID, out state);
                    if (1 != state) //不再当前地图
                    {
                        continue;
                    }

                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team1[i].RoleID);
                    if (null != gc)
                    {
                        gc.DJRoomID = -1;
                        gc.DJRoomTeamID = -1;
                        gc.HideSelf = 0;
                    }
                }

                for (int i = 0; i < djRoomRolesData.Team2.Count; i++)
                {
                    int state = 0;
                    djRoomRolesData.RoleStates.TryGetValue(djRoomRolesData.Team2[i].RoleID, out state);
                    if (1 != state) //不再当前地图
                    {
                        continue;
                    }

                    KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.Team2[i].RoleID);
                    if (null != gc)
                    {
                        gc.DJRoomID = -1;
                        gc.DJRoomTeamID = -1;
                        gc.HideSelf = 0;
                    }
                }

                if (null != djRoomRolesData.ViewRoles)
                {
                    for (int i = 0; i < djRoomRolesData.ViewRoles.Count; i++)
                    {
                        KPlayer gc = GameManager.ClientMgr.FindClient(djRoomRolesData.ViewRoles[i].RoleID);
                        if (null != gc)
                        {
                            gc.DJRoomID = -1;
                            gc.DJRoomTeamID = -1;
                            gc.HideSelf = 0;
                        }
                    }
                }
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, -1, (int)DianJiangCmds.RemoveRoom, roomID, "noHint");

            int index = 0;
            KPlayer client = null;
            TCPOutPacket tcpOutPacket = null;
            try
            {
                while ((client = GetNextClient(ref index)) != null)
                {
                    if (!client.ViewDJRoomDlg)
                    {
                        continue;
                    }

                    if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DIANJIANG);
                    if (!sl.SendData(client.ClientSocket, tcpOutPacket, false))
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
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        #endregion 点将台相关

        #region 竞技场决斗赛相关

        /// <summary>
        /// 通知角色大乱斗的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyArenaBattleCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int battleType, int extTag1, int leftSecs)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", status, client.RoleID, battleType, extTag1, leftSecs);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_ARENABATTLE);
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
        /// 通知在线的所有人(不限制地图)大乱斗邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllArenaBattleInviteMsg(SocketListener sl, TCPOutPacketPool pool, int minLevel, int battleType, int extTag1, int leftSecs)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.m_Level < minLevel) //最低级别要求
                {
                    continue;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, client.RoleID, battleType, extTag1, leftSecs);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_ARENABATTLE);
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
        }

        /// <summary>
        /// 通知在线的所有人(仅限在大乱斗地图上)大乱斗邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyArenaBattleInviteMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode, int battleType, int extTag1, int leftSecs)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, -1, battleType, extTag1, leftSecs);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_ARENABATTLE);
        }

        /// <summary>
        /// 通知角色大乱斗中杀人个数的指令,系统统一通知，每隔30秒一次，通知竞技场地图的玩家
        /// </summary>
        /// <param name="client"></param>
        public void NotifyArenaBattleKilledNumCmd(SocketListener sl, TCPOutPacketPool pool, int roleNumKilled, int roleNumOnStart, int rowNumNow)
        {
            List<Object> objsList = Container.GetObjectsByMap(GameManager.ArenaBattleMgr.BattleMapCode);
            if (null == objsList) return;

            string strcmd = "";

            KPlayer client = null;

            for (int n = 0; n < objsList.Count; n++)
            {
                client = objsList[n] as KPlayer;
                if (null != client)
                {
                    // MU 改造 只发个人积分、剩余人数
                    //strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, client.ArenaBattleKilledNum, roleNumKilled, roleNumOnStart, rowNumNow);

                    strcmd = string.Format("{0}:{1}", client.KingOfPkCurrentPoint, rowNumNow);
                    SendToClient(sl, pool, client, strcmd, (int)TCPGameServerCmds.CMD_SPR_ARENABATTLEKILLEDNUM);
                }
            }
        }

        #endregion 竞技场决斗赛相关

        #region 炎黄战场相关

        /// <summary>
        /// 通知角色大乱斗的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBattleCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int battleType, int extTag1, int leftSecs)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", status, client.RoleID, battleType, extTag1, leftSecs);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLE);
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
        /// 通知在线的所有人(不限制地图)大乱斗邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllBattleInviteMsg(SocketListener sl, TCPOutPacketPool pool, int minLevel, int battleType, int extTag1, int leftSecs)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.m_Level < minLevel) //最低级别要求
                {
                    continue;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, client.RoleID, battleType, extTag1, leftSecs);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLE);
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
        }

        /// <summary>
        /// 通知在线的所有人(仅限在大乱斗地图上)大乱斗邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBattleInviteMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode, int battleType, int extTag1, int leftSecs)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, -1, battleType, extTag1, leftSecs);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLE);
        }

        /// <summary>
        /// 开始时强制清场(仅限在大乱斗地图上)离开大乱斗场景消息
        /// </summary>
        /// <param name="client"></param>
        public void BattleBeginForceLeaveg(SocketListener sl, TCPOutPacketPool pool, int mapCode)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode);
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer client = objsList[i] as KPlayer;
                if (null == client) continue;

                Container.RemoveObject(client.RoleID, mapCode, client);
            }
        }

        /// <summary>
        /// 通知在线的所有人(仅限在大乱斗地图上)离开大乱斗场景消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBattleLeaveMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode);
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer client = objsList[i] as KPlayer;
                if (null == client) continue;

                int toMapCode = GameManager.MainMapCode;
                int toPosX = -1;
                int toPosY = -1;

                //判断下，如果上一次的地图为空，或则不是普通地图，则强制回主城
                if (client.LastMapCode != -1 && client.LastPosX != -1 && client.LastPosY != -1)
                {
                    if (MapTypes.Normal == Global.GetMapType(client.LastMapCode))
                    {
                        toMapCode = client.LastMapCode;
                        toPosX = client.LastPosX;
                        toPosY = client.LastPosY;
                    }
                }

                GameMap gameMap = null;
                if (GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out gameMap)) //确认地图编号是否有效
                {
                    GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                        client, toMapCode, toPosX, toPosY, -1);
                }
            }
        }

        /// <summary>
        /// 通知角色大乱斗中杀人个数的指令[旧的通知函数，通知所有人]
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyBattleKilledNumCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int suiJiFen, int tangJiFen)
        //{
        //    List<Object> objsList = Container.GetObjectsByMap(client.MapCode);
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, client.BattleKilledNum, suiJiFen, tangJiFen);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLEKILLEDNUM);
        //}

        /// <summary>
        /// 通知角色大乱斗中杀人个数的指令[新的通知函数，只通知自己]
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBattleKilledNumCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int tangJiFen, int suiJiFen)
        {
            // 阵营战场改造 1.角色roleid 2.个人积分 3.本场最高分 4.教团得分 5.联盟得分 [12/23/2013 LiaoWei]
            int nTotal = BattleManager.BattleMaxPointNow;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, client.BattleKilledNum, nTotal, tangJiFen, suiJiFen);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLEKILLEDNUM);
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
        /// 通知角色大乱斗中杀人个数的指令,系统统一通知，每隔30秒一次，通知隋唐战场地图的玩家
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBattleKilledNumCmd(SocketListener sl, TCPOutPacketPool pool, int suiJiFen, int tangJiFen)
        {
            List<Object> objsList = Container.GetObjectsByMap(GameManager.BattleMgr.BattleMapCode);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, -1, -1, tangJiFen, suiJiFen);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLEKILLEDNUM);
        }

        /// <summary>
        /// 通知角斗场称号的信息
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void NotifyRoleBattleNameInfo(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.BattleNameStart, client.BattleNameIndex);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGBATTLENAMEINFO);
        }

        /// <summary>
        /// 处理通知角斗场称号的超时
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void ProcessRoleBattleNameInfoTimeOut(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            //无称号
            if (client.BattleNameIndex <= 0)
            {
                return;
            }

            long ticks = TimeUtil.NOW();
            if (ticks - client.BattleNameStart < Global.MaxBattleNameTicks) //有称号，并且在有效时间中
            {
                return;
            }

            //有称号，已经超过了指定的时间
            client.BattleNameIndex = 0;

            //异步写数据库，写入当前的角斗场称号信息
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBATTLENAME,
                string.Format("{0}:{1}:{2}", client.RoleID, client.BattleNameStart, client.BattleNameIndex),
                null, client.ServerId);

            //通知角斗场称号的信息
            GameManager.ClientMgr.NotifyRoleBattleNameInfo(sl, pool, client);
        }

        /// <summary>
        /// 通知角斗场开始的人数和已经死亡的人数
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void NotifyRoleBattleRoleInfo(SocketListener sl, TCPOutPacketPool pool, int mapCode, int startTotalRoleNum, int allKilledRoleNum)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给地图的用户
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", startTotalRoleNum, allKilledRoleNum);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYBATTLEROLEINFO);
        }

        /// <summary>
        /// 通知角斗场结束时的信息
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void NotifyRoleBattleEndInfo(SocketListener sl, TCPOutPacketPool pool, int mapCode, List<BattleEndRoleItem> endRoleItemList)
        {
            if (endRoleItemList.Count <= 0) return;

            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给地图的用户
            if (null == objsList) return;

            byte[] bytesData = DataHelper.ObjectToBytes<List<BattleEndRoleItem>>(endRoleItemList);

            //群发消息
            SendToClients(sl, pool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_NOTIFYBATTLEENDINFO);
        }

        /// <summary>
        /// 通知角斗场阵营的信息变更
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void NotifyRoleBattleSideInfo(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.BattleWhichSide);
            TCPOutPacket tcpOutPacket = null;
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYBATTLESIDE);
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
        /// 通知角斗场双方人员数量 [1/20/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void NotifyRoleBattlePlayerSideNumberEndInfo(SocketListener sl, TCPOutPacketPool pool, int mapCode, int nNum1, int nNum2)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给地图的用户
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", nNum1, nNum2);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_BATTLEPLAYERNUMNOTIFY);
        }

        #endregion 炎黄战场相关

        #region 自动战斗相关

        /// <summary>
        /// 通知角色自动战斗的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAutoFightCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int fightType, int extTag1)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}", status, client.RoleID, fightType, extTag1);
            //TCPOutPacket tcpOutPacket = null;

            //tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_AUTOFIGHT);
            //if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            //{
            //    //
            //    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
            //        tcpOutPacket.PacketCmdID,
            //        tcpOutPacket.PacketDataSize,
            //        client.RoleID,
            //        client.RoleName));*/
            //}

            SCAutoFight scData = new SCAutoFight(status, client.RoleID, fightType, extTag1);
            client.sendCmd((int)TCPGameServerCmds.CMD_SPR_AUTOFIGHT, scData);
        }

        #endregion 自动战斗相关

        #region 组队相关

        /// <summary>
        /// 通知角色组队的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int teamType, int extTag1, string extTag2, int nOccu = -1, int nLev = -1, int nChangeLife = -1)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", status, client.RoleID, teamType, extTag1, extTag2, nOccu, nLev, nChangeLife);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_TEAM);
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
        /// 通知组队数据的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamData(SocketListener sl, TCPOutPacketPool pool, TeamData td)
        {
            if (null != td)
            {
                lock (td)
                {
                    byte[] bytesData = DataHelper.ObjectToBytes<TeamData>(td);
                    TCPOutPacket tcpOutPacket = null;
                    for (int i = 0; i < td.TeamRoles.Count; i++)
                    {
                        KPlayer client = FindClient(td.TeamRoles[i].RoleID);
                        if (null == client) continue;

                        tcpOutPacket = pool.Pop();
                        tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_TEAMDATA;
                        tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

                        if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                        {
                            //
                            /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                tcpOutPacket.PacketCmdID,
                                tcpOutPacket.PacketDataSize,
                                client.RoleID,
                                client.RoleName));*/
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Thông báo tổ đội thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersTeamIDChanged(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.TeamID, Global.GetGameClientTeamLeaderID(client));

            //Gửi packet về cho game client
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_TEAMID);
        }

        /// <summary>
        /// Gửi thông báo về cho GameClient Tổ đội desotry
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersTeamDestroy(SocketListener sl, TCPOutPacketPool pool, KPlayer client, TeamData td)
        {
            if (null != td)
            {
                lock (td)
                {
                    for (int i = 0; i < td.TeamRoles.Count; i++)
                    {
                        KPlayer gameClient = FindClient(td.TeamRoles[i].RoleID);
                        if (null == gameClient) continue;
                        if (client == gameClient) continue;

                        gameClient.TeamID = 0;
                        GameManager.TeamMgr.RemoveRoleID2TeamID(gameClient.RoleID);

                        NotifyOthersTeamIDChanged(sl, pool, gameClient);
                    }
                }
            }
        }

        /// <summary>
        /// 通知组队中的其他队员自己的级别发生了变化
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamUpLevel(SocketListener sl, TCPOutPacketPool pool, KPlayer client, bool zhuanShengChanged = false)
        {
            if (client.TeamID <= 0) //如果没有队伍
            {
                return;
            }

            //查找组队的数据
            TeamData td = GameManager.TeamMgr.FindData(client.TeamID);
            if (null == td) //没有找到组队数据
            {
                return;
            }

            lock (td)
            {
                TCPOutPacket tcpOutPacket = null;
                for (int i = 0; i < td.TeamRoles.Count; i++)
                {
                    KPlayer gc = FindClient(td.TeamRoles[i].RoleID);
                    if (null == gc) continue;

                    if (td.TeamRoles[i].RoleID == client.RoleID)
                    {
                        td.TeamRoles[i].Level = client.m_Level; //更新级别
                        td.TeamRoles[i].ChangeLifeLev = client.ChangeLifeCount; //更新级别
                    }

                    string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.m_Level, client.ChangeLifeCount);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYTEAMCHGLEVEL);
                    if (!sl.SendData(gc.ClientSocket, tcpOutPacket))
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
        }

        /// <summary>
        /// 通知自己战力改变
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ZhanLi"></param>
        public void NotifySelfChgZhanLi(KPlayer client, int ZhanLi)
        {
            client.sendCmd((int)TCPGameServerCmds.CMD_SPR_NOTIFYSELFCHGZHANLI, ZhanLi);
        }

        /// <summary>
        /// 通知组队中的其他队员自己的战力发生了变化
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamCHGZhanLi(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.TeamID <= 0) //如果没有队伍
            {
                return;
            }

            //查找组队的数据
            TeamData td = GameManager.TeamMgr.FindData(client.TeamID);
            if (null == td) //没有找到组队数据
            {
                return;
            }

            lock (td)
            {
                TCPOutPacket tcpOutPacket = null;
                string strcmd = string.Format("{0}:{1}", client.RoleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYTEAMCHGZHANLI);
                for (int i = 0; i < td.TeamRoles.Count; i++)
                {
                    KPlayer gc = FindClient(td.TeamRoles[i].RoleID);
                    if (null == gc) continue;

                    if (td.TeamRoles[i].RoleID == client.RoleID)
                    {
                        td.TeamRoles[i].CombatForce = 0; //更新战力
                    }

                    //若客户端修订版本小于1,不发送此消息
                    if (gc.CodeRevision < 1)
                    {
                        continue;
                    }

                    if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                    {
                        //
                        /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                            tcpOutPacket.PacketCmdID,
                            tcpOutPacket.PacketDataSize,
                            client.RoleID,
                            client.RoleName));*/
                    }
                }
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        #endregion 组队相关

        #region 物品交易

        /// <summary>
        /// 通知请求物品交易的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGoodsExchangeCmd(SocketListener sl, TCPOutPacketPool pool, int roleID, int otherRoleID, KPlayer client, KPlayer otherClient, int status, int exchangeType)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", status, roleID, otherRoleID, exchangeType);
            TCPOutPacket tcpOutPacket = null;

            if (null != client)
            {
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE);
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

            if (null != otherClient)
            {
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE);
                if (!sl.SendData(otherClient.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        otherClient.RoleID,
                        otherClient.RoleName));*/
                }
            }
        }

        /// <summary>
        /// 通知请求物品交易数据的指令信息
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
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }

            tcpOutPacket = pool.Pop();
            tcpOutPacket.PacketCmdID = (UInt16)TCPGameServerCmds.CMD_SPR_EXCHANGEDATA;
            tcpOutPacket.FinalWriteData(bytesData, 0, (int)bytesData.Length);

            if (!sl.SendData(otherClient.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    otherClient.RoleID,
                    otherClient.RoleName));*/
            }
        }

        #endregion 物品交易

        #region 掉落包裹相关

        /// <summary>
        /// 物品掉落通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersNewGoodsPack(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int ownerRoleID, string ownerRoleName, int autoID, int goodsPackID, int mapCode, int toX, int toY, int goodsID, int goodsNum, long productTicks, int teamID, string teamRoleIDs)
        //{
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", ownerRoleID, ownerRoleName, autoID, goodsPackID, toX, toY, goodsID, goodsNum, productTicks, teamID, teamRoleIDs);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_NEWGOODSPACK);
        //}

        /// <summary>
        /// 物品掉落通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewGoodsPack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int ownerRoleID, string ownerRoleName, int autoID, int goodsPackID, int mapCode, int toX, int toY, int goodsID, int goodsNum, long productTicks, int teamID, string teamRoleIDs, int lucky, int excellenceInfo, int appendPropLev, int forge_Level)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}", ownerRoleID, ownerRoleName, autoID, goodsPackID, toX, toY, goodsID, goodsNum, productTicks, teamID, teamRoleIDs, lucky, excellenceInfo, appendPropLev, forge_Level);

            NewGoodsPackData newGoodsPackData = new NewGoodsPackData()
            {
                OwnerRoleID = ownerRoleID,

                AutoID = autoID,
                PosX = toX,
                PosY = toY,
                GoodsID = goodsID,
                GoodCount = goodsNum,
                ProductTicks = productTicks,
                TeamID = teamID,
                HTMLColor = "",


            };

            byte[] bytes = DataHelper.ObjectToBytes<NewGoodsPackData>(newGoodsPackData);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_SPR_NEWGOODSPACK);

            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {

            }
        }

        /// <summary>
        /// 物品拾取通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfGetThing(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int goodsDbID)
        {
            string strcmd = "";
            strcmd = string.Format("{0}:{1}", client.RoleID, goodsDbID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETTHING);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket)) //告诉客户端已经获取的物品
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
        /// 物品掉落消失通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersDelGoodsPack(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int mapCode, int autoID, int toRoleID)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", autoID, toRoleID);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELGOODSPACK);
        }

        /// <summary>
        /// 物品掉落消失通知自己(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelGoodsPack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int autoID)
        {
            //string strcmd = string.Format("{0}:{1}", autoID, client.RoleID);
            string strcmd = string.Format("{0}:{1}", autoID, -1);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELGOODSPACK);
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

        #endregion 掉落包裹相关

        #region 通知客户端的伤害消息

        /// <summary>
        /// 向自己发送敌人受伤的信息
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="roleID"></param>
        /// <param name="enemy"></param>
        /// <param name="burst"></param>
        /// <param name="injure"></param>
        /// <param name="cmd"></param>
        /// <param name="enemyLife"></param>
        public static void NotifySelfEnemyInjured(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int roleID, int enemy, int burst, int injure, double enemyLife, long newExperience)
        {
            //string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", enemy, burst, injure, enemyLife, newExperience, client.Experience, client.m_Level);
            SpriteAttackResultData attackResultData = new SpriteAttackResultData();
            attackResultData.enemy = enemy;
            attackResultData.burst = burst;
            attackResultData.injure = injure;
            attackResultData.enemyLife = enemyLife;
            attackResultData.newExperience = newExperience;
            attackResultData.currentExperience = client.m_Experience;
            attackResultData.newLevel = client.m_Level;

            byte[] cmdData = DataHelper.ObjectToBytes<SpriteAttackResultData>(attackResultData);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_ATTACK);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// 获取受伤的对象
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="injuredRoleID"></param>
        /// <returns></returns>
        private IObject GetInjuredObject(int mapCode, int injuredRoleID)
        {
            IObject injuredObj = null;

            //根据敌人ID判断对方是系统爆的怪还是其他玩家
            GSpriteTypes st = Global.GetSpriteType((UInt32)injuredRoleID);
            if (st == GSpriteTypes.Monster)
            {
                //通知敌人自己开始攻击他，并造成了伤害
                Monster monster = GameManager.MonsterMgr.FindMonster(mapCode, injuredRoleID);
                if (null != monster)
                {
                    injuredObj = monster;
                }
            }
            else if (st == GSpriteTypes.BiaoChe) //如果是镖车
            {
                //暂时系统不支持，也不增加了
                BiaoCheManager.FindBiaoCheByRoleID(injuredRoleID);
            }
            else if (st == GSpriteTypes.JunQi) //如果是帮旗
            {
                return JunQiManager.FindJunQiByID(injuredRoleID);
            }
            else
            {
                //通知敌人自己开始攻击他，并造成了伤害
                KPlayer obj = GameManager.ClientMgr.FindClient(injuredRoleID);
                if (null != obj)
                {
                    injuredObj = obj;
                }
            }

            return injuredObj;
        }

        /// <summary>
        /// Thông báo mục tiêu bị sát thương
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteInjured(SocketListener sl, TCPOutPacketPool pool, IObject attacker, int mapCode, int attackerRoleID, int injuredRoleID, int burst, int injure, double injuredRoleLife, int attackerLevel, Point hitToGrid)
        {
            if (hitToGrid.X < 0 || hitToGrid.Y < 0)
            {
                hitToGrid.X = 0;
                hitToGrid.Y = 0;
            }

            //获取受伤的对象
            IObject injuredObj = GetInjuredObject(mapCode, injuredRoleID);
            if (null == injuredObj)
            {
                return;
            }

            List<Object> objsList = Global.GetAll9Clients(attacker);
            if (null == objsList)
            {
                objsList = new List<object>();
            }

            //2015-9-16消息流量优化
            if (GameManager.FlagHideFlagsType == -1 || !GameManager.HideFlagsMapDict.ContainsKey(mapCode))
            {
                if (objsList.IndexOf(injuredObj) < 0)
                {
                    objsList.Add(injuredObj);
                }

                int injuredRoleMagic = 0;
                int injuredRoleMaxMagicV = 0;
                int injuredRoleMaxLifeV = 0;
                KPlayer injuredClient = FindClient(injuredRoleID);
                if (null != injuredClient)
                {
                    injuredRoleMagic = injuredClient.m_CurrentMana;
                    injuredRoleMaxMagicV = injuredClient.m_CurrentManaMax;
                    injuredRoleMaxLifeV = injuredClient.m_CurrentLifeMax;
                }

                SpriteInjuredData injuredData = new SpriteInjuredData();
                injuredData.attackerRoleID = attackerRoleID;
                injuredData.injuredRoleID = injuredRoleID;
                injuredData.burst = burst;
                injuredData.injure = injure;
                injuredData.injuredRoleLife = injuredRoleLife;
                injuredData.attackerLevel = attackerLevel;
                injuredData.injuredRoleMaxLifeV = injuredRoleMaxLifeV;
                injuredData.injuredRoleMagic = injuredRoleMagic;
                injuredData.injuredRoleMaxMagicV = injuredRoleMaxMagicV;
                injuredData.hitToGridX = (int)hitToGrid.X;
                injuredData.hitToGridY = (int)hitToGrid.Y;

                byte[] bytesCmd = DataHelper.ObjectToBytes<SpriteInjuredData>(injuredData);
                //群发消息
                SendToClients(sl, pool, null, objsList, /*strcmd*/bytesCmd, (int)TCPGameServerCmds.CMD_SPR_INJURE);

                //判断精灵是否组队中，如果是，则也通知九宫格之外的队友
                //通知被伤害的用户的队友伤害的数据
                NotifySpriteTeamInjured(sl, pool, injuredRoleID, /*strcmd*/bytesCmd, mapCode);
            }
            else
            {
                int injuredRoleMagic = 0;
                int injuredRoleMaxMagicV = 0;
                int injuredRoleMaxLifeV = 0;
                SpriteInjuredData injuredData = new SpriteInjuredData();

                //先准备发给别人的数据
                injuredData.injuredRoleID = injuredRoleID;
                injuredData.injuredRoleLife = injuredRoleLife;
                injuredData.burst = burst;
                injuredData.injure = injure;
                if (hitToGrid.X > 0 || hitToGrid.Y > 0)
                {
                    injuredData.hitToGridX = (int)hitToGrid.X;
                    injuredData.hitToGridY = (int)hitToGrid.Y;
                    injuredData.attackerRoleID = attackerRoleID; //击退时,需要知道攻击者,以便计算
                }

                if (null != injuredObj && injuredObj.ObjectType == ObjectTypes.OT_CLIENT)
                {
                    if (objsList.IndexOf(injuredObj) < 0)
                    {
                        objsList.Add(injuredObj);
                    }
                }

                bool dead = (injuredRoleLife <= 0);
                if (dead) injuredData.attackerRoleID = attackerRoleID;

                byte[] bytesCmd = DataHelper.ObjectToBytes<SpriteInjuredData>(injuredData);
                if (dead)
                {
                    SendToClients(sl, pool, null, objsList, /*strcmd*/bytesCmd, (int)TCPGameServerCmds.CMD_SPR_INJURE, ClientHideFlags.HideOtherMagicAndInjured, injuredRoleID);
                }
                else
                {
                    SendToClients(sl, pool, null, objsList, /*strcmd*/bytesCmd, (int)TCPGameServerCmds.CMD_SPR_INJURE, ClientHideFlags.HideOtherMagicAndInjured, injuredRoleID);
                }

                NotifySpriteTeamInjured(sl, pool, injuredRoleID, /*strcmd*/bytesCmd, mapCode);
            }
        }

        //         / <summary>
        /// 通知被伤害的用户的队友伤害的数据
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteTeamInjured(SocketListener sl, TCPOutPacketPool pool, int injuredRoleID, /*string*/byte[] bytesCmd, int mapCode)
        {
            //判断精灵是否组队中，如果是，则也通知九宫格之外的队友
            KPlayer otherClient = FindClient(injuredRoleID);
            if (null != otherClient)
            {
                if (otherClient.TeamID > 0)
                {
                    //查找组队数据
                    TeamData td = GameManager.TeamMgr.FindData(otherClient.TeamID);
                    if (null != td)
                    {
                        List<int> roleIDsList = new List<int>();

                        //锁定组队数据
                        lock (td)
                        {
                            for (int i = 0; i < td.TeamRoles.Count; i++)
                            {
                                if (injuredRoleID == td.TeamRoles[i].RoleID)
                                {
                                    continue;
                                }

                                roleIDsList.Add(td.TeamRoles[i].RoleID);
                            }
                        }
                        TCPOutPacket tcpOutPacket = null;
                        try
                        {
                            for (int i = 0; i < roleIDsList.Count; i++)
                            {
                                KPlayer gc = FindClient(roleIDsList[i]);
                                if (null == gc) continue;
                                if (gc.MapCode != mapCode)
                                {
                                    continue;
                                }

                                if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, /*strcmd*/bytesCmd, 0, bytesCmd.Length, (int)TCPGameServerCmds.CMD_SPR_INJURE);
                                if (!sl.SendData(gc.ClientSocket, tcpOutPacket, false))
                                {
                                    //
                                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                                        tcpOutPacket.PacketCmdID,
                                        tcpOutPacket.PacketDataSize,
                                        gc.RoleID,
                                        gc.RoleName));*/
                                }
                            }
                        }
                        finally
                        {
                            PushBackTcpOutPacket(tcpOutPacket);
                        }
                    }
                }
            }
        }

        #endregion 通知客户端的伤害消息

        #region 个人紧要消息通知

        /// <summary>
        /// 通知在线的所有人(不限制地图)个人紧要消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllImportantMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string msgText, GameInfoTypeIndexes typeIndex, ShowGameInfoTypes showGameInfoType, int errCode = 0, int minZhuanSheng = 0, int minLevel = 0)
        {
            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (null != client && gc == client)
                {
                    continue;
                }
                if (null != gc && Global.GetUnionLevel(gc) < Global.GetUnionLevel(minZhuanSheng, minLevel))
                {
                    continue;
                }

                //通知在线的对方(不限制地图)个人紧要消息
                NotifyImportantMsg(sl, pool, gc, msgText, typeIndex, showGameInfoType, errCode);
            }
        }

        /// <summary>
        /// 通知在线的所有帮会的人(不限制地图)个人紧要消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBangHuiImportantMsg(SocketListener sl, TCPOutPacketPool pool, int faction, string msgText, GameInfoTypeIndexes typeIndex, ShowGameInfoTypes showGameInfoType, int errCode = 0)
        {
            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (faction != gc.GuildID)
                {
                    continue;
                }

                //通知在线的对方(不限制地图)个人紧要消息
                NotifyImportantMsg(sl, pool, gc, msgText, typeIndex, showGameInfoType, errCode);
            }
        }

        /// <summary>
        /// 通知在线的对方(不限制地图)个人紧要消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyImportantMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string msgText, GameInfoTypeIndexes typeIndex, ShowGameInfoTypes showGameInfoType, int errCode = 0)
        {
            //替换非法字符
            msgText = msgText.Replace(":", "``");

            TCPOutPacket tcpOutPacket = null;
            string strcmd = string.Format("{0}:{1}:{2}:{3}", (int)showGameInfoType, (int)typeIndex, msgText, errCode);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYMSG);
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

        public void NotifyImportantMsg(KPlayer client, string msgText, GameInfoTypeIndexes typeIndex, ShowGameInfoTypes showGameInfoType, int errCode = 0)
        {
            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, msgText, typeIndex, showGameInfoType, errCode);
        }

        public void NotifyAddExpMsg(KPlayer client, long addExp)
        {
            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                client, string.Format(Global.GetLang("您获得了：经验+{0}"), addExp),
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.NoTongQian);
        }

        public void NotifyAddJinBiMsg(KPlayer client, int addJinBi)
        {
            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                client, StringUtil.substitute(Global.GetLang("您获得了：金币 + {0}"), addJinBi),
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.NoTongQian);
        }

        /// <summary>
        /// 通知客户端显示提示信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void NotifyHintMsg(KPlayer client, string msg)
        {
            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                client, msg,
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.None);
        }

        /// <summary>
        /// 通知客户端显示提示信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void NotifyCopyMapHintMsg(KPlayer client, string msg)
        {
            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                client, msg,
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.None);
        }

        #endregion 个人紧要消息通知

        #region 公告发布

        /// <summary>
        /// 通知GM授权消息
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="auth"></param>
        public void NotifyGMAuthCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int auth)
        {
            TCPOutPacket tcpOutPacket = null;
            string strcmd = string.Format("{0}:{1}", client.RoleID, auth);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GMAUTH);
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
        /// 通知在线的所有人(不限制地图)公告消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllBulletinMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, BulletinMsgData bulletinMsgData, int minZhuanSheng = 0, int minLevel = 0)
        {
            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (null != client && gc == client)
                {
                    continue;
                }
                if (Global.GetUnionLevel(gc) < Global.GetUnionLevel(minZhuanSheng, minLevel))
                {
                    continue;
                }

                if (gc.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //通知在线的对方(不限制地图)公告消息
                NotifyBulletinMsg(sl, pool, gc, bulletinMsgData);
            }
        }

        /// <summary>
        /// 通知在线的对方(不限制地图)公告消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBulletinMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, BulletinMsgData bulletinMsgData)
        {
            TCPOutPacket tcpOutPacket = null;
            tcpOutPacket = DataHelper.ObjectToTCPOutPacket<BulletinMsgData>(bulletinMsgData, pool, (int)TCPGameServerCmds.CMD_SPR_BULLETINMSG);
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

        #endregion 公告发布

        #region PK值/PK点通知

        /// <summary>
        /// 通知PK值和PK点更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeRolePKValueAndPKPoint(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer enemy)
        {
            ///野蛮冲撞时，自己会受伤
            if (client == enemy)
            {
                return;
            }

            //角斗场 和 炎黄战场 中，红名功能失效
            if (client.MapCode == GameManager.BattleMgr.BattleMapCode
                || client.MapCode == GameManager.ArenaBattleMgr.BattleMapCode)
            {
                return;
            }

            //如果在王城争霸赛其间，失效
            if (WangChengManager.IsInCityWarBattling(client))
            {
                return;
            }

            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap))
            {
                return;
            }

            //判断地图的PK模式
            //PKMode, 0普通地图表示由用户的pk模式决定(加PK值), 1战斗地图表示强制PK模式(不加PK值), 2安全地图表示不允许PK;
            if ((int)MapPKModes.Normal != gameMap.PKMode)
            {
                return;
            }

            client.PKValue = client.PKValue + 1;

            //根据PK点计算出颜色索引值(0: 白色, 1:黄色, 2:红色)
            int enemyNameColorIndex = Global.GetNameColorIndexByPKPoints(enemy.PKPoint);
            if (enemyNameColorIndex < 2) //杀红名不记PK点
            {
                //是否是紫名
                if (!Global.IsPurpleName(enemy))
                {
                    client.PKPoint = Global.GMin(Global.MaxPKPointValue, client.PKPoint + Global.PKValueEqPKPoints);
                }
            }
            else if (Global.IsRedName(client))
            {
                if (Global.AddToTodayRoleKillRoleSet(client.RoleID, enemy.RoleID))
                {
                    client.PKPoint = Global.GMax(0, client.PKPoint - Global.PKValueEqPKPoints / 2);
                }
            }

            // 给玩家更新红名处罚BUFFER [4/21/2014 LiaoWei]
            Global.ProcessRedNamePunishForDebuff(client);

            //更新PKValue
            //GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD,
            //    string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint),
            //    null);

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPKVAL);
        }

        /// <summary>
        /// 通知PK值和PK点更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        //public void SubRedNameRolePKValueAndPKPoint(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer enemy)
        //{
        //    //角斗场 和 炎黄战场 中，红名功能失效
        //    if (client.MapCode == GameManager.BattleMgr.BattleMapCode
        //        || client.MapCode == GameManager.ArenaBattleMgr.BattleMapCode)
        //    {
        //        return;
        //    }

        //    GameMap gameMap = null;
        //    if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap))
        //    {
        //        return;
        //    }

        //    //判断地图的PK模式
        //    //PKMode, 0普通地图表示由用户的pk模式决定(加PK值), 1战斗地图表示强制PK模式(不加PK值), 2安全地图表示不允许PK;
        //    if (1 == gameMap.PKMode)
        //    {
        //        return;
        //    }

        //    //自己是红名被杀，则减少自己的PK值
        //    if (client.PKPoint < Global.MinRedNamePKPoints)
        //    {
        //        return;
        //    }

        //    //client.PKValue = Global.GMax(0, client.PKValue - 1);
        //    client.PKPoint = Global.GMax(0, client.PKPoint - Global.RedNameBeKilledSubPKPoints);

        //    List<Object> objsList = Global.GetAll9Clients(client);
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPKVAL);
        //}

        /// <summary>
        /// 设置PK值(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void SetRolePKValuePoint(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int pkValue, int pkPoint, bool writeToDB = true)
        {
            client.PKValue = pkValue;
            client.PKPoint = pkPoint;

            //更新红名BUFF
            Global.ProcessRedNamePunishForDebuff(client);

            if (writeToDB)
            {
                //更新PKValue
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD,
                    string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint),
                    null, client.ServerId);

                long nowTicks = TimeUtil.NOW();
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD, nowTicks);
            }

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPKVAL);
        }

        #endregion PK值/PK点通知

        #region 紫名管理

        /// <summary>
        /// 通知紫名信息(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeRolePurpleName(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer enemy)
        {
            ///野蛮冲撞自己对自己伤害，不记灰名
            if (client == enemy)
            {
                return;
            }

            //角斗场  和 炎黄战场 中，紫名功能失效
            if (client.MapCode == GameManager.BattleMgr.BattleMapCode
                || client.MapCode == GameManager.ArenaBattleMgr.BattleMapCode)
            {
                return;
            }

            //杀红名不会紫名
            if (enemy.PKPoint >= Global.MinRedNamePKPoints)
            {
                return;
            }

            GameMap gameMap = null;
            if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap))
            {
                return;
            }

            //判断地图的PK模式
            //PKMode, 0普通地图表示由用户的pk模式决定(加PK值), 1战斗地图表示强制PK模式(不加PK值), 2安全地图表示不允许PK;
            if ((int)MapPKModes.Normal != gameMap.PKMode)
            {
                return;
            }

            //攻击紫名不会紫名
            //if (Global.IsPurpleName(enemy))
            //{
            //    return;
            //}

            //bool oldPurpleName = Global.IsPurpleName(client);

            //设置紫名的时间
            client.StartPurpleNameTicks = TimeUtil.NOW();

            //是否是紫名, (重复更新紫名信息，会导致频繁的广播通讯)
            //if (oldPurpleName)
            //{
            //    return;
            //}

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StartPurpleNameTicks);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPURPLENAME);
        }

        /// <summary>
        /// 通知紫名信息(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ForceChangeRolePurpleName2(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            //是否是紫名, (重复更新紫名信息，会导致频繁的广播通讯)
            if (Global.IsPurpleName(client))
            {
                return;
            }

            //设置紫名的时间
            client.StartPurpleNameTicks = TimeUtil.NOW();

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StartPurpleNameTicks);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPURPLENAME);
        }

        /// <summary>
        /// 播报紫名的消失事件
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void BroadcastRolePurpleName(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (client.StartPurpleNameTicks <= 0)
            {
                return;
            }

            if (Global.IsPurpleName(client))
            {
                return;
            }

            client.StartPurpleNameTicks = 0;

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StartPurpleNameTicks);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGPURPLENAME);
        }

        #endregion 紫名管理

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
            this.SendChatMessage(sl, pool, client, null, client, content, ChatChannel.System);
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
            this.SendChatMessage(sl, pool, client, null, client, content, ChatChannel.Default);
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
        public void SendChatMessage(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer fromClient, KPlayer toClient, string content, ChatChannel channel)
        {
            SpriteChat chat = new SpriteChat()
            {
                FromRoleName = fromClient == null ? "" : fromClient.RoleName,
                ToRoleName = toClient.RoleName,
                Channel = (int)channel,
                Content = content,
                EquipInfos = null,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int)TCPGameServerCmds.CMD_SPR_CHAT);
            sl.SendData(client.ClientSocket, tcpOutPacket);
        }

        /// <summary>
        /// Gửi tin nhắn thoại tới người chơi tương ứng
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client">Đối tượng được gửi gói tin về</param>
        /// <param name="fromClient"></param>
        /// <param name="toClient"></param>
        /// <param name="voiceData"></param>
        /// <param name="channel"></param>
        public void SendVoiceMessage(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer fromClient, KPlayer toClient, byte[] voiceData, ChatChannel channel)
        {
            /*SpriteChat chat = new SpriteChat()
            {
                FromRoleName = fromClient == null ? "" : fromClient.RoleName,
                ToRoleName = toClient.RoleName,
                Channel = (int) channel,
                Content = null,
                EquipInfos = null,
                VoiceContent = voiceData,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<SpriteChat>(chat);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, 0, cmdData.Length, (int) TCPGameServerCmds.CMD_SPR_CHAT);
            sl.SendData(client.ClientSocket, tcpOutPacket);*/
        }

        #endregion Chat

        #region 角色攻击角色

        /// <summary>
        /// 记录战斗中的敌人
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        public void LogBatterEnemy(KPlayer attacker, KPlayer victim)
        {
            attacker.RoleIDAttackebByMyself = victim.RoleID;
            victim.RoleIDAttackMe = attacker.RoleID;
        }

        /// <summary>
        /// Thông báo tấn công tới game CLIENT
        /// </summary>
        /// <param name="client"></param>
        public int NotifyOtherInjured(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer enemy, int burst, int injure, double injurePercnet, int attackType, bool forceBurst, int addInjure, double attackPercent, int addAttackMin, int addAttackMax, int skillLevel, double skillBaseAddPercent, double skillUpAddPercent, bool ignoreDefenseAndDodge = false, bool dontEffectDSHide = false, double baseRate = 1.0, int addVlue = 0, int nHitFlyDistance = 0)
        {
            int ret = 0;
            object obj = enemy;
            if ((obj as KPlayer).m_CurrentLife > 0)
            {
                //Ghi lại logs
                LogBatterEnemy(client, enemy);

                // TÍnh toán sát thương
                if (injure <= 0)
                {
                    if ((int)AttackType.PHYSICAL_ATTACK == attackType) //物理攻击
                    {
                        RoleAlgorithm.AttackEnemy(client, (obj as KPlayer), forceBurst, injurePercnet, addInjure, attackPercent, addAttackMin, addAttackMax, out burst, out injure, ignoreDefenseAndDodge, baseRate, addVlue);
                    }
                    else if ((int)AttackType.MAGIC_ATTACK == attackType) //魔法攻击
                    {
                        RoleAlgorithm.MAttackEnemy(client, (obj as KPlayer), forceBurst, injurePercnet, addInjure, attackPercent, addAttackMin, addAttackMax, out burst, out injure, ignoreDefenseAndDodge, baseRate, addVlue);
                    }
                }

                bool selfLifeChanged = false;
                if (injure > 0)
                {
                    RoleRelifeLog relifeLog = new RoleRelifeLog(
                        client.RoleID, client.RoleName, client.MapCode,
                        /**/string.Format("打人rid={0},rname={1}击中恢复", enemy.RoleID, enemy.RoleName));
                    int lifeSteal = (int)RoleAlgorithm.GetLifeStealV(client);
                    if (lifeSteal > 0 && client.m_CurrentLife < client.m_CurrentLifeMax)
                    {
                        relifeLog.hpModify = true;
                        relifeLog.oldHp = client.m_CurrentLife;
                        selfLifeChanged = true;
                        client.m_CurrentLife += lifeSteal;
                    }
                    if (client.m_CurrentLife > client.m_CurrentLifeMax)
                    {
                        client.m_CurrentLife = client.m_CurrentLifeMax;
                    }
                    relifeLog.newHp = client.m_CurrentLife;
                    MonsterAttackerLogManager.Instance().AddRoleRelifeLog(relifeLog);
                }

                //将对敌人的伤害进行处理
                injure = InjureToEnemy(sl, pool, enemy, injure, attackType, ignoreDefenseAndDodge, skillLevel);

                /// 被攻击时吸收一部分伤害(护身戒指)
                //injure -= SpecialEquipMgr.DoSubInJure((obj as KPlayer), (int)ItemCategories.FashionWeapon, injure);

                //处理角色克星
                injure = DBRoleBufferManager.ProcessAntiRole(client, (obj as KPlayer), injure);

                // PK的伤害为50% ChenXiaojun
                injure = injure / 2;

                // 校正
                (obj as KPlayer).m_CurrentLife = Global.GMax((obj as KPlayer).m_CurrentLife, 0);

                // 卓越属性 有几率完全恢复血和蓝 [12/27/2013 LiaoWei]
                if (client.ExcellenceProp[(int)ExcellencePorp.EXCELLENCEPORP15] > 0.0)
                {
                    int nRan = Global.GetRandomNumber(0, 101);
                    if (nRan <= client.ExcellenceProp[(int)ExcellencePorp.EXCELLENCEPORP15] * 100)
                    {
                        client.m_CurrentLife = client.m_CurrentLifeMax;
                        selfLifeChanged = true; // 校正 血蓝改变 需要通知客户端 [XSea 2015/8/10]
                    }
                }

                if (client.ExcellenceProp[(int)ExcellencePorp.EXCELLENCEPORP16] > 0.0)
                {
                    int nRan = Global.GetRandomNumber(0, 101);
                    if (nRan <= client.ExcellenceProp[(int)ExcellencePorp.EXCELLENCEPORP16] * 100)
                    {
                        client.m_CurrentMana = client.m_CurrentManaMax;
                        selfLifeChanged = true; // 校正 血蓝改变 需要通知客户端 [XSea 2015/8/10]
                    }
                }

                int enemyLife = (obj as KPlayer).m_CurrentLife;

                (obj as KPlayer).UsingEquipMgr.InjuredSomebody((obj as KPlayer));

                SpriteInjure2Blood(sl, pool, client, injure);

                Point hitToGrid = new Point(-1, -1);

                if (nHitFlyDistance > 0)
                {
                    MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[client.MapCode];

                    int nGridNum = nHitFlyDistance * 100 / mapGrid.MapGridWidth;

                    if (nGridNum > 0)
                        hitToGrid = ChuanQiUtils.HitFly(client, enemy, nGridNum);
                }

                NotifySpriteInjured(sl, pool, client, client.MapCode, client.RoleID, (obj as KPlayer).RoleID, burst, injure, enemyLife, client.m_Level, hitToGrid); // 加上梅林伤害与类型 [XSea 2015/6/26]

                //向自己发送敌人受伤的信息
                {
                    NotifySelfEnemyInjured(sl, pool, client, client.RoleID, enemy.RoleID, burst, injure, enemyLife, 0); // 加上梅林伤害与类型 [XSea 2015/6/26]
                }

                //Xóa bỏ buff tàng hình
                if (!dontEffectDSHide)
                {
                    if (client.DSHideStart > 0)
                    {
                        KTLogic.RemoveBufferData(client, (int)BufferItemTypes.DSTimeHideNoShow);
                        client.DSHideStart = 0;
                        GameManager.ClientMgr.NotifyDSHideCmd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);
                    }
                }

                //Xóa bỏ buff tàng hình
                if (enemy.DSHideStart > 0)
                {
                    KTLogic.RemoveBufferData(enemy, (int)BufferItemTypes.DSTimeHideNoShow);
                    enemy.DSHideStart = 0;
                    GameManager.ClientMgr.NotifyDSHideCmd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, enemy);
                }

                //受到其他角色攻击就取消采集状态
                CaiJiLogic.CancelCaiJiState(enemy);

                if (enemyLife <= 0)
                {
                    Global.ProcessRoleDieForRoleAttack(sl, pool, client, (obj as KPlayer));
                }

                //通知紫名信息(限制当前地图)
                GameManager.ClientMgr.ChangeRolePurpleName(sl, pool, client, enemy);

                // 反射伤害处理 [12/27/2013 LiaoWei]
                // Damege Phản đồn
                Global.ProcessDamageThorn(sl, pool, client, enemy, injure);

                if (injure > 0)
                {
                    enemy.passiveSkillModule.OnInjured(enemy);
                }

                if (selfLifeChanged)
                {
                    GameManager.ClientMgr.NotifyOthersLifeChanged(sl, pool, client);
                }

                GameManager.damageMonitor.Out(client);
            }

            return ret;
        }

        #endregion 角色攻击角色

        #region 怪物攻击角色

        /// <summary>
        /// Thông báo sát thương tới người chơi khác
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOtherInjured(SocketListener sl, TCPOutPacketPool pool, Monster monster, KPlayer enemy, int burst, int injure, double injurePercnet, int attackType, bool forceBurst, int addInjure, double attackPercent, int addAttackMin, int addAttackMax, int skillLevel, double skillBaseAddPercent, double skillUpAddPercent, bool ignoreDefenseAndDodge = false, double baseRate = 1.0, int addVlue = 0, int nHitFlyDistance = 0)
        {
            Object obj = enemy;
            if (null != obj)
            {
                if (enemy.m_CurrentLife > 0) //做次判断，否则会杀死多次
                {
                    //记录战斗中的敌人
                    enemy.RoleIDAttackMe = monster.RoleID;

                    // 怪物攻击角色的计算公式
                    if (injure <= 0)
                    {
                        if ((int)AttackType.PHYSICAL_ATTACK == attackType) //物理攻击
                        {
                            RoleAlgorithm.AttackEnemy(monster, (obj as KPlayer), false, 1.0, 0, attackPercent, addAttackMin, addAttackMax, out burst, out injure, ignoreDefenseAndDodge, baseRate, addVlue);
                        }
                        else if ((int)AttackType.MAGIC_ATTACK == attackType) //魔法攻击
                        {
                            RoleAlgorithm.MAttackEnemy(monster, (obj as KPlayer), false, 1.0, 0, attackPercent, addAttackMin, addAttackMax, out burst, out injure, ignoreDefenseAndDodge, baseRate, addVlue);
                        }
                        else //道术攻击
                        {
                            // 属性改造 去掉 道术攻击[8/15/2013 LiaoWei]
                            //RoleAlgorithm.DSAttackEnemy(monster, (obj as KPlayer), false, 1.0, 0, attackPercent, addAttack, out burst, out injure, ignoreDefenseAndDodge);
                        }
                    }

                    //将对敌人的伤害进行处理
                    injure = InjureToEnemy(sl, pool, (obj as KPlayer), injure, attackType, ignoreDefenseAndDodge, skillLevel);

                    // 技能中可配置伤害百分比
                    injure = (int)(injure * injurePercnet);

                    /// 被攻击时吸收一部分伤害(护身戒指)
                    //injure -= SpecialEquipMgr.DoSubInJure((obj as KPlayer), (int)ItemCategories.FashionWeapon, injure);

                    // 校正
                    (obj as KPlayer).m_CurrentLife = Global.GMax((obj as KPlayer).m_CurrentLife, 0);

                    //生命值为0时,立即回复100%生命值
                    //SpecialEquipMgr.DoEquipRestoreBlood((obj as KPlayer), (int)ItemCategories.FashionWeapon);

                    int enemyLife = (obj as KPlayer).m_CurrentLife;

                    //被攻击时减少装备耐久度
                    (obj as KPlayer).UsingEquipMgr.InjuredSomebody((obj as KPlayer));

                    //为什么不取消隐身状态？

                    //取消被攻击玩家的采集状态
                    CaiJiLogic.CancelCaiJiState(obj as KPlayer);

                    //判断是否将给敌人的伤害转化成自己的血量增长

                    if (enemyLife <= 0) //怪物杀死了角色
                    {
                        Global.ProcessRoleDieForMonsterAttack(sl, pool, monster, enemy);
                    }

                    //GameManager.SystemServerEvents.AddEvent(string.Format("角色减血, roleID={0}({1}), Injure={2}, Life={3}", (obj as KPlayer).RoleID, (obj as KPlayer).RoleName, injure, enemyLife), EventLevels.Debug);

                    // 处理击飞 怪物击飞玩家 todo...[3/15/2014 LiaoWei]
                    /*Point hitToGrid = new Point(-1, -1);
                    if (nHitFlyDistance > 0)
                    {
                        MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[client.MapCode];

                        int nGridNum = nHitFlyDistance / mapGrid.MapGridWidth;

                        if (nGridNum > 0)
                            hitToGrid = ChuanQiUtils.HitFly(client, enemy, nGridNum);
                    }*/

                    Point hitToGrid = new Point(-1, -1);

                    // 处理击飞 [3/15/2014 LiaoWei]
                    if (nHitFlyDistance > 0)
                    {
                        MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[(obj as KPlayer).MapCode];

                        int nGridNum = nHitFlyDistance * 100 / mapGrid.MapGridWidth;

                        if (nGridNum > 0)
                            hitToGrid = ChuanQiUtils.HitFly((obj as KPlayer), enemy, nGridNum);
                    }

                    if (injure > 0)
                    {
                        enemy.passiveSkillModule.OnInjured(enemy);
                    }

                    NotifySpriteInjured(sl, pool, (obj as KPlayer), monster.MonsterZoneNode.MapCode, monster.RoleID, (obj as KPlayer).RoleID, burst, injure, enemyLife, monster.MonsterInfo.Level, hitToGrid); // 加上梅林伤害与类型 [XSea 2015/6/26]

                    // 反射伤害处理 [6/9/2014 LiaoWei]
                    Global.ProcessDamageThorn(sl, pool, monster, (obj as KPlayer), injure);
                }
            }
        }

        #endregion 怪物攻击角色

        #region 精灵搜索怪物

        /// <summary>
        /// 供精灵使用，自动搜索怪物
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        public Point SeekMonsterPosition(KPlayer client, int centerX, int centerY, int radiusGridNum, out int totalMonsterNum)
        {
            totalMonsterNum = 0;
            Point pt = new Point(centerX, centerY);
            List<Object> objsList = GameManager.MonsterMgr.GetObjectsByMap(client.MapCode);
            if (null == objsList) return pt;

            int radiusXY = radiusGridNum * 64;

            //totalMonsterNum = objsList.Count;
            int lastDistance = 2147483647;
            Monster monster = null, findMonster = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                if (objsList[i] is Monster)
                {
                    monster = objsList[i] as Monster;

                    //如果怪物已经死亡
                    if (monster.m_CurrentLife <= 0 || !monster.Alive)
                    {
                        continue;
                    }

                    //如果是弓箭手，则忽略
                    if ((int)MonsterTypes.CityGuard == monster.MonsterType)
                    {
                        continue;
                    }

                    //如果怪物不是自己可以攻击的，则不返回
                    if (!Global.IsOpposition(client, monster))
                    {
                        continue;
                    }

                    //如果是在副本地图中
                    if (monster.CurrentCopyMapID > 0)
                    {
                        if (monster.CurrentCopyMapID != client.CopyMapID) //副本地图的ID必须相等，否则忽略
                        {
                            continue;
                        }
                    }

                    if (!Global.InCircle(monster.CurrentPos, pt, radiusXY))
                    {
                        continue;
                    }

                    totalMonsterNum++;

                    int distance = (int)Global.GetTwoPointDistance(pt, monster.CurrentPos);
                    if (distance < lastDistance) //使用此算法查找离自己最近的怪物
                    {
                        lastDistance = distance;
                        findMonster = monster;
                    }
                }
            }

            if (null != findMonster)
            {
                return new Point(findMonster.CurrentPos.X, findMonster.CurrentPos.Y);
            }

            //return new Point(centerX, centerY);
            return new Point(client.PosX, client.PosY);
        }

        #endregion 精灵搜索怪物

        #region 查找指定范围内的角色

        /// <summary>
        /// 查找指定圆周范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircle(KPlayer client, int mapCode, int toX, int toY, int radius, List<int> enemiesList)
        {
            List<Object> objList = new List<object>();
            LookupEnemiesInCircle(client, mapCode, toX, toY, radius, objList);
            for (int i = 0; i < objList.Count; i++)
            {
                enemiesList.Add((objList[i] as KPlayer).RoleID);
            }
        }

        /// <summary>
        /// 查找指定圆周范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircle(KPlayer client, int mapCode, int toX, int toY, int radius, List<Object> enemiesList, int nTargetType = -1)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)toX, (int)toY, radius);
            if (null == objsList) return;

            Point center = new Point(toX, toY);
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (null != client && client.RoleID == (objsList[i] as KPlayer).RoleID) continue;

                //非敌对对象
                if ((null != client && !Global.IsOpposition(client, (objsList[i] as KPlayer))) && nTargetType != 2)
                {
                    continue;
                }

                //不在同一个副本
                if (null != client && client.CopyMapID != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);
                if (Global.InCircle(target, center, (double)radius))
                {
                    enemiesList.Add(objsList[i]);
                }
            }
        }

        /// <summary>
        /// 查找指定圆周范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircle(int mapCode, int copyMapCode, int toX, int toY, int radius, List<Object> enemiesList)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)toX, (int)toY, radius);
            if (null == objsList) return;

            Point center = new Point(toX, toY);
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                //不在同一个副本
                if (copyMapCode != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);
                if (Global.InCircle(target, center, (double)radius))
                {
                    enemiesList.Add(objsList[i]);
                }
            }
        }

        /// <summary>
        /// 查找指定半圆范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircleByAngle(KPlayer client, int direction, int mapCode, int toX, int toY, int radius, List<int> enemiesList, double angle, bool near180)
        {
            List<Object> objList = new List<Object>();

            LookupEnemiesInCircleByAngle(client, direction, mapCode, toX, toY, radius, objList, angle, near180);
            for (int i = 0; i < objList.Count; i++)
            {
                enemiesList.Add((objList[i] as KPlayer).RoleID);
            }
        }

        /// <summary>
        /// 查找指定半圆范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircleByAngle(KPlayer client, int direction, int mapCode, int toX, int toY, int radius, List<Object> enemiesList, double angle, bool near180)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)toX, (int)toY, radius);
            if (null == objsList) return;

            double loAngle = 0.0, hiAngle = 0.0;
            Global.GetAngleRangeByDirection(direction, angle, out loAngle, out hiAngle);

            double loAngleNear = 0.0, hiAngleNear = 0.0;
            Global.GetAngleRangeByDirection(direction, 360, out loAngleNear, out hiAngleNear);

            Point center = new Point(toX, toY);
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (client.RoleID == (objsList[i] as KPlayer).RoleID) continue;

                //非敌对对象
                if (null != client && !Global.IsOpposition(client, (objsList[i] as KPlayer)))
                {
                    continue;
                }

                //不在同一个副本
                if (null != client && client.CopyMapID != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);
                if (Global.InCircleByAngle(target, center, (double)radius, loAngle, hiAngle))
                {
                    enemiesList.Add((objsList[i]));
                }
                //else if (Global.InCircleByAngle(target, center, (double)200, loAngleNear, hiAngleNear))
                else if (Global.InCircle(target, center, (double)100))
                {
                    enemiesList.Add((objsList[i]));
                }
            }
        }

        /// <summary>
        /// 查找指定半圆范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircleByAngle(int direction, int mapCode, int copyMapCode, int toX, int toY, int radius, List<int> enemiesList, double angle, bool near180)
        {
            List<Object> objList = new List<Object>();

            LookupEnemiesInCircleByAngle(direction, mapCode, copyMapCode, toX, toY, radius, objList, angle, near180);
            for (int i = 0; i < objList.Count; i++)
            {
                enemiesList.Add((objList[i] as KPlayer).RoleID);
            }
        }

        /// <summary>
        /// 查找指定半圆范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesInCircleByAngle(int direction, int mapCode, int copyMapCode, int toX, int toY, int radius, List<Object> enemiesList, double angle, bool near180)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)toX, (int)toY, radius);
            if (null == objsList) return;

            double loAngle = 0.0, hiAngle = 0.0;
            Global.GetAngleRangeByDirection(direction, angle, out loAngle, out hiAngle);

            double loAngleNear = 0.0, hiAngleNear = 0.0;
            Global.GetAngleRangeByDirection(direction, 360, out loAngleNear, out hiAngleNear);

            int nAddRadius = 100;

            Point center = new Point(toX, toY);
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                //不在同一个副本
                if (copyMapCode != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);
                if (Global.InCircleByAngle(target, center, (double)radius, loAngle, hiAngle))
                {
                    enemiesList.Add((objsList[i]));
                }
                //else if (Global.InCircleByAngle(target, center, (double)200, loAngleNear, hiAngleNear))
                else if (Global.InCircle(target, center, (double)nAddRadius))
                {
                    enemiesList.Add((objsList[i]));
                }
            }
        }

        /// <summary>
        /// 查找指定圆周范围内的玩家
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupRolesInCircle(KPlayer client, int mapCode, int toX, int toY, int radius, List<Object> rolesList)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)toX, (int)toY, radius);
            if (null == objsList) return;

            Point center = new Point(toX, toY);
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if (null != client && client.RoleID == (objsList[i] as KPlayer).RoleID) continue;

                //不在同一个副本
                if (null != client && client.CopyMapID != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);
                if (Global.InCircle(target, center, (double)radius))
                {
                    rolesList.Add(objsList[i]);
                }
            }
        }

        // 增加扫描类型 矩形扫描 [11/27/2013 LiaoWei]
        /// <summary>
        /// 查找指定矩形范围内的玩家
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        //public void LookupRolesInSquare(KPlayer client, int mapCode, int toX, int toY, int radius, int nWidth, List<Object> rolesList)
        public void LookupRolesInSquare(KPlayer client, int mapCode, int radius, int nWidth, List<Object> rolesList)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects((int)client.PosX, (int)client.PosY, radius);
            if (null == objsList) return;

            // 源点
            Point source = new Point(client.PosX, client.PosY);

            Point toPos = Global.GetAPointInCircle(source, radius, client.RoleYAngle);

            int toX = (int)toPos.X;
            int toY = (int)toPos.Y;

            // 矩形的中心点
            Point center = new Point();
            center.X = (client.PosX + toX) / 2;
            center.Y = (client.PosY + toY) / 2;

            // 矩形方向向量
            int fDirectionX = toX - client.PosX;
            int fDirectionY = toY - client.PosY;
            //Point center = new Point(toX, toY);

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                    continue;

                if ((objsList[i] as KPlayer).m_CurrentLifeMax <= 0)
                    continue;

                if (null != client && client.RoleID == (objsList[i] as KPlayer).RoleID)
                    continue;

                // 不在同一个副本
                if (null != client && client.CopyMapID != (objsList[i] as KPlayer).CopyMapID)
                    continue;

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);

                if (Global.InSquare(center, target, radius, nWidth, fDirectionX, fDirectionY))
                    rolesList.Add(objsList[i]);
                else if (Global.InCircle(target, source, (double)100))  // 补充扫描
                    rolesList.Add((objsList[i]));
            }
        }

        /// <summary>
        /// 查找指定矩形范围内的玩家(矩形扫描)
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupRolesInSquare(int mapCode, int copyMapId, int srcX, int srcY, int toX, int toY, int radius, int nWidth, List<Object> rolesList)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            List<Object> objsList = mapGrid.FindObjects(srcX, srcY, radius);
            if (null == objsList) return;

            // 源点
            Point source = new Point(srcX, srcY);

            // 矩形的中心点
            Point center = new Point();
            center.X = (srcX + toX) / 2;
            center.Y = (srcY + toY) / 2;

            // 矩形方向向量
            int fDirectionX = toX - srcX;
            int fDirectionY = toY - srcY;
            //Point center = new Point(toX, toY);

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                    continue;

                if ((objsList[i] as KPlayer).m_CurrentLifeMax <= 0)
                    continue;

                // 不在同一个副本
                if (copyMapId != (objsList[i] as KPlayer).CopyMapID)
                    continue;

                Point target = new Point((objsList[i] as KPlayer).PosX, (objsList[i] as KPlayer).PosY);

                if (Global.InSquare(center, target, radius, nWidth, fDirectionX, fDirectionY))
                    rolesList.Add(objsList[i]);
                else if (Global.InCircle(target, source, (double)100))  // 补充扫描
                    rolesList.Add((objsList[i]));
            }
        }

        #endregion 查找指定范围内的角色

        #region 查找指定格子内的角色

        /// <summary>
        /// 查找指定格子内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupEnemiesAtGridXY(IObject attacker, int gridX, int gridY, List<Object> enemiesList)
        {
            int mapCode = attacker.CurrentMapCode;
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];

            List<Object> objsList = mapGrid.FindObjects((int)gridX, (int)gridY);
            if (null == objsList) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                //if (null != client && client.RoleID == (objsList[i] as KPlayer).RoleID)
                //{
                //    continue;
                //}

                //非敌对对象
                //if (null != client && !Global.IsOpposition(client, (objsList[i] as KPlayer)))
                //{
                //    continue;
                //}

                //不在同一个副本
                if (null != attacker && attacker.CurrentCopyMapID != (objsList[i] as KPlayer).CopyMapID)
                {
                    continue;
                }

                enemiesList.Add(objsList[i]);
            }
        }

        /// <summary>
        /// 查找指定格子内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupAttackEnemies(IObject attacker, int direction, List<Object> enemiesList)
        {
            int mapCode = attacker.CurrentMapCode;
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];

            Point grid = attacker.CurrentGrid;
            int gridX = (int)grid.X;
            int gridY = (int)grid.Y;

            Point p = Global.GetGridPointByDirection(direction, gridX, gridY);

            //查找指定格子内的敌人
            LookupEnemiesAtGridXY(attacker, (int)p.X, (int)p.Y, enemiesList);
        }

        /// <summary>
        /// 查找指定格子内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupAttackEnemyIDs(IObject attacker, int direction, List<int> enemiesList)
        {
            List<Object> objList = new List<Object>();
            LookupAttackEnemies(attacker, direction, objList);
            for (int i = 0; i < objList.Count; i++)
            {
                enemiesList.Add((objList[i] as KPlayer).RoleID);
            }
        }

        /// <summary>
        /// 查找指定给子范围内的敌人
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public void LookupRangeAttackEnemies(IObject obj, int toX, int toY, int direction, string rangeMode, List<Object> enemiesList)
        {
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[obj.CurrentMapCode];

            int gridX = toX / mapGrid.MapGridWidth;
            int gridY = toY / mapGrid.MapGridHeight;

            //根据传入的格子坐标和方向返回指定方向的格子列表
            List<Point> gridList = Global.GetGridPointByDirection(direction, gridX, gridY, rangeMode);
            if (gridList.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < gridList.Count; i++)
            {
                //查找指定格子内的敌人
                LookupEnemiesAtGridXY(obj, (int)gridList[i].X, (int)gridList[i].Y, enemiesList);
            }
        }

        #endregion 查找指定格子内的角色

        #region 伤害转化

        /// <summary>
        /// 忽视防御的物理攻击，魔法盾的最高吸收伤害比例
        /// </summary>
        private static double[] IgnoreDefenseAndDogeSubPercent = { 0.05, 0.10, 0.20 };

        /// <summary>
        /// 将对敌人的伤害进行处理
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="injured"></param>
        public int InjureToEnemy(SocketListener sl, TCPOutPacketPool pool, KPlayer enemy, int injured, int attackType, bool ignoreDefenseAndDodge, int skillLevel)
        {
            double totalSubValue = 0;

            //是否有防护罩，减伤
            totalSubValue += enemy.RoleMagicHelper.GetSubInjure();

            // MU项目 防护罩
            totalSubValue += enemy.RoleMagicHelper.MU_GetSubInjure2();

            //结算百分比减伤和固定值减伤
            injured = (int)(injured - totalSubValue); //这里要求先固定值后百分比
            injured = (int)(injured * (1 - enemy.RoleMagicHelper.GetSubInjure1()) * (1 - enemy.RoleMagicHelper.MU_GetSubInjure1()));
            if (injured <= 0)
            {
                return 0;
            }

            //是否可以用魔法消耗来代替伤害
            double percent = enemy.RoleMagicHelper.GetInjure2Magic();
            if (percent > 0.0)
            {
                double magicV = percent * injured;
                magicV = Global.GMin(magicV, injured);
                magicV = Global.GMin(enemy.m_CurrentMana, magicV);

                injured -= (int)magicV;
                //injured = Global.GMax(1, injured);

                //通知减少魔量
                SubSpriteMagicV(sl, pool, enemy, magicV);
            }

            //是否可以用魔法消耗来代替伤害
            double injured2Magic = enemy.RoleMagicHelper.GetNewInjure2Magic();
            if (injured2Magic > 0.0)
            {
                injured2Magic = Global.GMin(injured2Magic, injured);
                injured2Magic = Global.GMin(enemy.m_CurrentMana, injured2Magic);

                injured -= (int)injured2Magic;
                //injured = Global.GMax(1, injured);

                //通知减少魔量
                SubSpriteMagicV(sl, pool, enemy, injured2Magic);
            }

            //是否可以用魔法消耗来代替伤害3
            percent = enemy.RoleMagicHelper.GetNewInjure2Magic3();
            if (percent > 0.0)
            {
                double magicV = percent * injured;
                magicV = Global.GMin(magicV, injured);
                magicV = Global.GMin(enemy.m_CurrentMana, magicV);

                injured -= (int)magicV;
                //injured = Global.GMax(1, injured);

                //通知减少魔量
                SubSpriteMagicV(sl, pool, enemy, magicV);
            }

            //是否可以用魔法消耗来代替伤害
            percent = enemy.RoleMagicHelper.GetNewMagicSubInjure();
            if (percent > 0.0)
            {
                if (0 == attackType) //物理攻击
                {
                    if (ignoreDefenseAndDodge) //忽视防御
                    {
                        skillLevel = Math.Min(skillLevel, IgnoreDefenseAndDogeSubPercent.Length - 1);
                        skillLevel = Math.Max(0, skillLevel);
                        percent = Math.Min(percent, IgnoreDefenseAndDogeSubPercent[skillLevel]);
                    }
                }

                double magicV = percent * injured;
                magicV = Global.GMin(magicV, injured);
                magicV = Global.GMin(enemy.m_CurrentMana, magicV);

                injured -= (int)magicV;
                //injured = Global.GMax(1, injured);

                //通知减少魔量
                //SubSpriteMagicV(sl, pool, enemy, magicV);
            }

            injured = DBRoleBufferManager.ProcessHuZhaoSubLifeV(enemy, Math.Max(0, injured));
            injured = DBRoleBufferManager.ProcessWuDiHuZhaoNoInjured(enemy, Math.Max(0, injured));

            return Math.Max(0, injured);
        }

        /// <summary>
        /// 将对敌人的伤害转化成为自己的血量
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="injured"></param>
        public void SpriteInjure2Blood(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int injured)
        {
            double percent = client.RoleMagicHelper.GetInjure2Life();
            if (0.0 >= percent) return;

            injured = (int)(injured * percent);
            AddSpriteLifeV(sl, pool, client, injured, "击中恢复");
        }

        #endregion 伤害转化

        #region 地图和位置切换

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
        public bool NotifyChangeMap(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int toMapCode, int maxX = -1, int mapY = -1, int direction = -1, int relife = 0)
        {
            if (client.CheckCheatData.GmGotoShadowMapCode != toMapCode)
            {
                if (client.ClientSocket.IsKuaFuLogin && client.KuaFuChangeMapCode != toMapCode)
                {
                    KuaFuManager.getInstance().GotoLastMap(client);
                    return true;
                }

                if (client.ClientSocket.IsKuaFuLogin != KuaFuManager.getInstance().IsKuaFuMap(toMapCode))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("GotoMap denied, mapCode={0},IsKuaFuLogin={1}", toMapCode, client.ClientSocket.IsKuaFuLogin));
                    return false;
                }
            }

            client.WaitingNotifyChangeMap = true;
            client.WaitingChangeMapToMapCode = toMapCode;
            client.WaitingChangeMapToPosX = maxX;
            client.WaitingChangeMapToPosY = mapY;

            /// TODO

            if ("1" == GameManager.GameConfigMgr.GetGameConfigItemStr("log-changmap", "0"))
            {
                if (client.LastNotifyChangeMapTicks >= TimeUtil.NOW() - 12000)
                {
                    try
                    {
                        DataHelper.WriteStackTraceLog(string.Format(Global.GetLang("地图传送频繁,记录堆栈信息备查 role={3}({4}) toMapCode={0} pt=({1},{2})"),
                            toMapCode, maxX, mapY, client.RoleName, client.RoleID));
                    }
                    catch (Exception) { }
                }
            }
            client.LastNotifyChangeMapTicks = TimeUtil.NOW();

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, toMapCode, maxX, mapY, direction, relife);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYCHGMAP);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }

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
            // ĐÁNH DẤU LẠI THỜI GIAN GẦN ĐÂY NHẤT CHUYỂN BẢN ĐỒ
            client.LastChangeMapTicks = TimeUtil.NOW();

            /// Ngừng di chuyển
            KTPlayerStoryBoard.Instance.Remove(client);

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

            /// Lấy danh sách các đối tượng xung quanh
            List<Object> objsList = Global.GetAll9Clients(client);
            /// Thông báo đối tượng rời khỏi vị trí cho các đối tượng xung quan
            GameManager.ClientMgr.NotifyOthersLeave(sl, pool, client, objsList);

            /// Xóa bản đồ khi client thoát
            Global.ClearCopyMap(client);

            /// Xóa đối tượng khỏi vị trí hiện tại được quản lý bởi MapGrid
            GameManager.ClientMgr.RemoveClientFromContainer(client);

            /// Nếu tồn tại vị trí đích đến
            if (toMapX <= 0 || toMapY <= 0)
            {
                int defaultBirthPosX = GameManager.MapMgr.DictMaps[toMapCode].DefaultBirthPosX;
                int defaultBirthPosY = GameManager.MapMgr.DictMaps[toMapCode].DefaultBirthPosY;
                int defaultBirthRadius = GameManager.MapMgr.DictMaps[toMapCode].BirthRadius;

                Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, toMapCode, defaultBirthPosX, defaultBirthPosY, defaultBirthRadius);
                toMapX = (int)newPos.X;
                toMapY = (int)newPos.Y;
            }

            /// Đánh dấu đang chờ chuyển map
            client.WaitingForChangeMap = true;

            /// ID bản đồ cũ
            int oldMapCode = client.MapCode;
            /// Thiết lập vị trí và bản đồ mới
            client.MapCode = toMapCode;
            client.PosX = toMapX;
            client.PosY = toMapY;
            client.ReportPosTicks = 0;

            /// Cập nhật động tác của đối tượng
            client.CurrentAction = (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand;

            /// Giảm bộ đếm người chơi của quái
            client.ClearVisibleObjects(true);

            /// Lưu lại vị trí đích đến
            client.DestPoint = new Point(-1, -1);

            /// Thêm đối tượng vào danh sách quản lý
            GameManager.ClientMgr.AddClientToContainer(client);

            /// Dịch đối tượng sang vị trí mới quản lý bởi MapGrid
            if (!GameManager.MapGridMgr.DictGrids[client.MapCode].MoveObject(-1, -1, client.PosX, client.PosY, client))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("Can not leave Map: Cmd={0}, RoleID={1}, Closed", (TCPGameServerCmds)nID, client.RoleID));
                return false;
            }

            /// ĐÉO BIẾT
            client.ClearChangeGrid();

            Global.RecordClientPosition(client);

            client.CheckCheatData.LastNotifyLeaveGuMuTick = 0;

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
            client.sendCmd((int)TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

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
            if (2 != animation) //如果不是跑步方式才在服务器端改变
            {
                //Console.WriteLine("CHANGEPOS => Stop StoryBoard");

                /// Ngừng StoryBoard
                KTPlayerStoryBoard.Instance.Remove(client);

                if (toMapX <= 0 || toMapY <= 0)
                {
                    int defaultBirthPosX = GameManager.MapMgr.DictMaps[client.MapCode].DefaultBirthPosX;
                    int defaultBirthPosY = GameManager.MapMgr.DictMaps[client.MapCode].DefaultBirthPosY;
                    int defaultBirthRadius = GameManager.MapMgr.DictMaps[client.MapCode].BirthRadius;

                    //从配置根据地图取默认位置
                    Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, client.MapCode, defaultBirthPosX, defaultBirthPosY, defaultBirthRadius);
                    toMapX = (int)newPos.X;
                    toMapY = (int)newPos.Y;
                }

                //此处不用做互斥，因为已经将客户端从队列中拿出了, 客户端切换地图时一定要启用阻塞的操作，防止用户再操作
                int oldX = client.PosX;
                int oldY = client.PosY;

                client.PosX = toMapX;
                client.PosY = toMapY;
                client.ReportPosTicks = 0;

                if (toMapDirection > 0)
                {
                    client.RoleDirection = toMapDirection;
                }

                //将精灵放入格子
                if (!GameManager.MapGridMgr.DictGrids[client.MapCode].MoveObject(-1, -1, client.PosX, client.PosY, client))
                {
                    client.PosX = oldX; //还原
                    client.PosY = oldY; //还原
                    client.ReportPosTicks = 0;

                    //LogManager.WriteLog(LogTypes.Warning, string.Format("精灵移动超出了地图边界: Cmd={0}, RoleID={1}, 关闭连接", (TCPGameServerCmds)nID, client.RoleID));
                    //return false;
                }

                /// 玩家进行了移动
                if (GameManager.Update9GridUsingNewMode <= 0)
                {
                    ClientManager.DoSpriteMapGridMove(client);
                }
                else
                {
                    Global.GameClientMoveGrid(client);
                }

                //Thread.Sleep((int)Global.GetRandomNumber(100, 201)); ///模拟npc对话框窗口不出来的操作

                Global.RecordClientPosition(client);
            }

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return true;

            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, toMapX, toMapY, toMapDirection);
            SendToClients(sl, pool, null, objsList, strcmd, nID);
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

        #endregion 地图和位置切换

        #region 物品管理

        /// <summary>
        /// Thông báo add vật phẩm về CLIENT
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfAddGoods(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int id, int goodsID, int forgeLevel, int goodsNum, int binding, int site, int newHint, string newEndTime,
            int strong,int bagIndex = 0,int isusing = -1,String Props = "",int Series = -1, Dictionary<ItemPramenter, int> OtherParams =null)
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
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// 物品修改通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyModGoods(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int modType, int id, int isusing, int site, int gcount, int bagIndex, int newHint)
        {
           
            //SCModGoods scData = new SCModGoods(0, modType, id, isusing, site, gcount, bagIndex, newHint);
            //client.sendCmd((int)TCPGameServerCmds.CMD_SPR_MOD_GOODS, scData);
        }

        /// <summary>
        /// 物品移动通知
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
                GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, gd.Id, gd.GoodsID, gd.Forge_level, gd.GCount, gd.Binding, gd.Site,1, gd.Endtime, gd.Strong, gd.BagIndex,gd.Using,gd.Props,gd.Series,gd.OtherParams);
            }
        }

       


        #endregion 物品管理

        #region 物品消耗

        /// <summary>
        /// 从用户物品中扣除消耗的数量【存在多个数量，仅仅扣除一个】
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        public bool NotifyUseGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int dbID, bool usingGoods, bool dontCalcLimitNum = false)
        {
            //修改内存中物品记录
            GoodsData goodsData = null;
            goodsData = Global.GetGoodsByDbID(client, dbID);
            return NotifyUseGoods(sl, tcpClientPool, pool, client, goodsData, 1, usingGoods, dontCalcLimitNum);
        }

        /// <summary>
        /// 从用户物品中扣除消耗的数量[将dbID对应的物品全部扣除,单个dbid对应的数量为多个也一起扣除]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        public bool NotifyUseGoodsByDbId(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int dbID, int useCount, bool usingGoods, bool dontCalcLimitNum = false)
        {
            //修改内存中物品记录
            GoodsData goodsData = null;
            goodsData = Global.GetGoodsByDbID(client, dbID);

            return NotifyUseGoods(sl, tcpClientPool, pool, client, goodsData, useCount, usingGoods, dontCalcLimitNum);
        }

        /// <summary>
        /// 重载物品消耗接口 从用户物品中扣除消耗的数量[将dbID对应的物品全部扣除,单个dbid对应的数量为多个也一起扣除] [6/9/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        public bool NotifyUseGoodsByDbId(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int dbID, int useCount, bool usingGoods, out bool usedBinding, out bool usedTimeLimited, bool dontCalcLimitNum = false)
        {
            //修改内存中物品记录
            GoodsData goodsData = null;
            goodsData = Global.GetGoodsByDbID(client, dbID);

            return NotifyUseGoods(sl, tcpClientPool, pool, client, goodsData.GoodsID, useCount, usingGoods, out usedBinding, out usedTimeLimited, dontCalcLimitNum);
        }

        /// <summary>
        /// 重载物品消耗接口 [6/9/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        public bool NotifyUseGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, GoodsData goodsData, int useCount, bool usingGoods, out bool usedBinding, out bool usedTimeLimited, bool dontCalcLimitNum = false)
        {
            usedBinding = false;
            usedTimeLimited = false;
            bool ret = false;

            lock (client.GoodsDataList)
            {
                if (Global.IsGoodsTimeOver(goodsData) || Global.IsGoodsNotReachStartTime(goodsData))
                {
                    return ret; //已经超时无法再使用
                }

                if (!usedBinding)
                {
                    usedBinding = (goodsData.Binding > 0); //判断是否使用了绑定的物品
                }

                if (!usedTimeLimited)
                {
                    usedTimeLimited = Global.IsTimeLimitGoods(goodsData);
                }

                ret = NotifyUseGoods(sl, tcpClientPool, pool, client, goodsData, useCount, usingGoods, dontCalcLimitNum);
            }

            return ret;
        }

        /// <summary>
        /// 从用户物品中扣除消耗的数量,usingGoods参数用于激活使用物品时的相关脚本,如果是日程普通扣除使用，需要设置false
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        public bool NotifyUseGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, GoodsData goodsData, int subNum, bool usingGoods, bool dontCalcLimitNum = false)
        {
            //修改内存中物品记录
            if (null == goodsData)
            {
                //不做处理
                return false;
            }

            //判断物品使用次数限制
            if (!dontCalcLimitNum)
            {
                if (!Global.HasEnoughGoodsDayUseNum(client, goodsData.GoodsID, subNum))
                {
                    return false;
                }
            }

            if (Global.IsGoodsTimeOver(goodsData) || Global.IsGoodsNotReachStartTime(goodsData))
            {
                return false; //已经超时无法再使用
            }

            if (goodsData.GCount <= 0)
            {
                return false; //个数已经为0，无法使用， 防止0个数刷物品后，使用。
            }

            if (goodsData.GCount < subNum)
            {
                return false; //如果要使用的已经小于已经有的，则返回失败
            }

            if (subNum <= 0) //无意义，防止外挂
            {
                return false;
            }

            List<MagicActionItem> magicActionItemList = null;
            int categoriy = 0;
            if (usingGoods)
            {
                // 处理物品的使用功能
                int verifyResult = UsingGoods.ProcessUsingGoodsVerify(client, goodsData.GoodsID, goodsData.Binding, out magicActionItemList, out categoriy);
                if (verifyResult < 0)
                {
                    return false;
                }
                else if (verifyResult == 0)
                {
                    // 升级物品 特殊判断 [8/16/2014 LiaoWei]
                    for (int j = 0; j < magicActionItemList.Count; j++)
                    {
                        if (magicActionItemList[j].MagicActionID == MagicActionIDs.UP_LEVEL)
                        {
                            int nLev = 0;
                            int nAddValue = (int)magicActionItemList[j].MagicActionParams[0];

                            bool bCanUp = true;
                            if (nAddValue > 0)
                            {
                                if (client.ChangeLifeCount > GameManager.ChangeLifeMgr.m_MaxChangeLifeCount)
                                {
                                    bCanUp = false;
                                }
                                else if (client.ChangeLifeCount == GameManager.ChangeLifeMgr.m_MaxChangeLifeCount)
                                {
                                    ChangeLifeDataInfo infoTmp = null;

                                    infoTmp = GameManager.ChangeLifeMgr.GetChangeLifeDataInfo(client);

                                    if (infoTmp == null)
                                        bCanUp = false;
                                    else
                                    {
                                        nLev = infoTmp.NeedLevel;

                                        if (client.m_Level >= nLev)
                                            bCanUp = false;
                                    }
                                }
                                else
                                {
                                    ChangeLifeDataInfo infoTmp = null;

                                    infoTmp = GameManager.ChangeLifeMgr.GetChangeLifeDataInfo(client, client.ChangeLifeCount + 1);

                                    if (infoTmp == null)
                                        bCanUp = false;
                                    else
                                    {
                                        nLev = infoTmp.NeedLevel;

                                        if (client.m_Level >= nLev)
                                            bCanUp = false;
                                    }
                                }

                                if (!bCanUp)
                                {
                                    GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                                    StringUtil.substitute(Global.GetLang("您的等级已达上限")),
                                                                                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.LevelOutOfRange);
                                    return false;
                                }

                                if ((client.m_Level + nAddValue) > nLev)
                                {
                                    GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                                    StringUtil.substitute(Global.GetLang("无法使用该物品")),
                                                                                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.LevelOutOfRange);
                                    return false;
                                }

                                if (client.m_CurrentLife <= 0)
                                {
                                    GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                                    StringUtil.substitute(Global.GetLang("死亡状态下无法使用该物品")),
                                                                                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.LevelOutOfRange);
                                    return false;
                                }
                            }
                        }
                        else if (magicActionItemList[j].MagicActionID == MagicActionIDs.ADD_GOODWILL)
                        {
                            // ITEM HỆ THỐNG CƯỚI ĐÃ XÓA
                        }
                        else if (magicActionItemList[j].MagicActionID == MagicActionIDs.MU_GETSHIZHUANG)
                        {
                            // Hệ thống thời tgrang đã xóa
                        }
                    }
                }
                else if (verifyResult == 1)
                {
                    usingGoods = false;
                }
            }

            int gcount = goodsData.GCount;
            string strcmd = "";

            gcount = goodsData.GCount - subNum;
            TCPOutPacket tcpOutPacket = null;

            //向DBServer请求修改物品
            string[] dbFields = null;
            strcmd = Global.FormatUpdateDBGoodsStr(client.RoleID, goodsData.Id, "*", "*", "*", "*", "*", "*", "*", gcount, "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*"); // 卓越信息 [12/13/2013 LiaoWei] 装备转生
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields, client.ServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(-1, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                /*
                strcmd = string.Format("{0}:{1}:{2}", -1, goodsData.Id, gcount);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                 */
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return false;
            }

            if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[1]) < 0)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(-2, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                /*
                strcmd = string.Format("{0}:{1}:{2}", -2, goodsData.Id, gcount);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                 */
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return false;
            }

            //修改内存中物品记录
            if (gcount > 0)
            {
                goodsData.GCount = gcount;
            }
            else if ((int)SaleGoodsConsts.ElementhrtsGoodsID == goodsData.Site || (int)SaleGoodsConsts.UsingElementhrtsGoodsID == goodsData.Site)
            {
                goodsData.GCount = 0;
                ElementhrtsManager.RemoveElementhrtsData(client, goodsData);
            }
            else if ((int)SaleGoodsConsts.FluorescentGemBag == goodsData.Site)
            {
                goodsData.GCount = 0;
                GameManager.FluorescentGemMgr.RemoveFluorescentGemData(client, goodsData);
            }
            else if ((int)SaleGoodsConsts.SoulStoneBag == goodsData.Site)
            {
                goodsData.GCount = 0;
                SoulStoneManager.Instance().RemoveSoulStoneGoods(client, goodsData, goodsData.Site);
            }
            else
            {
                goodsData.GCount = 0;
                Global.RemoveGoodsData(client, goodsData);
            }

            if (usingGoods)
            {
                // 处理物品的使用功能
                UsingGoods.ProcessUsingGoods(client, goodsData.GoodsID, goodsData.Binding, magicActionItemList, categoriy);
            }

            //更新物品使用次数限制
            if (!dontCalcLimitNum)
            {
                Global.AddGoodsLimitNum(client, goodsData.GoodsID, subNum);
            }

            //写入角色物品的得失行为日志(扩展)
            Global.ModRoleGoodsEvent(client, goodsData, -subNum, "物品使用");
            EventLogManager.AddGoodsEvent(client, OpTypes.AddOrSub, OpTags.None, goodsData.GoodsID, goodsData.Id, -subNum, goodsData.GCount, "物品使用");

            // 七日活动
            SevenDayGoalEventObject evObj = SevenDayGoalEvPool.Alloc(client, ESevenDayGoalFuncType.UseGoodsCount);
            evObj.Arg1 = goodsData.GoodsID;
            evObj.Arg2 = subNum;
            GlobalEventSource.getInstance().fireEvent(evObj);

            tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(0, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
            /*
            strcmd = string.Format("{0}:{1}:{2}", 0, goodsData.Id, gcount);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
             */
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }

            // 属性改造 去掉 负重[8/15/2013 LiaoWei]
            //重新计算角色负重(单个物品)
            /*if (Global.UpdateGoodsWeight(client, goodsData, subNum, false))
            {
                //重量属性更新通知
                GameManager.ClientMgr.NotifyUpdateWeights(sl, pool, client);
            }*/

            return true;
        }

        /// <summary>
        /// 从用户物品中扣除消耗的数量
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        public bool NotifyUseGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int goodsID, int totalNum, bool usingGoods, out bool usedBinding, out bool usedTimeLimited, bool dontCalcLimitNum = false)
        {
            usedBinding = false;
            usedTimeLimited = false;
            bool ret = false;
            int count = 0;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID)
                    {
                        if (Global.IsGoodsTimeOver(client.GoodsDataList[i]) || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        {
                            continue; //已经超时无法再使用
                        }

                        if (!usedBinding)
                        {
                            usedBinding = (client.GoodsDataList[i].Binding > 0); //判断是否使用了绑定的物品
                        }

                        if (!usedTimeLimited)
                        {
                            usedTimeLimited = Global.IsTimeLimitGoods(client.GoodsDataList[i]);
                        }

                        int gcount = client.GoodsDataList[i].GCount;
                        int subNum = Global.GMin(gcount, totalNum - count);
                        ret = NotifyUseGoods(sl, tcpClientPool, pool, client, client.GoodsDataList[i], subNum, usingGoods, dontCalcLimitNum);
                        if (!ret)
                        {
                            break;
                        }

                        count += subNum;
                        if (count >= totalNum)
                        {
                            break;
                        }

                        if (subNum >= gcount)
                        {
                            i--;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 从用户绑定物品中扣除消耗的数量 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        public bool NotifyUseBindGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int goodsID, int totalNum, bool usingGoods, out bool usedBinding, out bool usedTimeLimited, bool dontCalcLimitNum = false)
        {
            usedBinding = false;
            usedTimeLimited = false;
            bool ret = false;
            int count = 0;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID)
                    {
                        if (Global.IsGoodsTimeOver(client.GoodsDataList[i]) || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        {
                            continue; //已经超时无法再使用
                        }

                        if (client.GoodsDataList[i].Binding < 1)
                        {
                            continue;
                        }

                        if (!usedBinding)
                        {
                            usedBinding = (client.GoodsDataList[i].Binding > 0); //判断是否使用了绑定的物品
                        }

                        if (!usedTimeLimited)
                        {
                            usedTimeLimited = Global.IsTimeLimitGoods(client.GoodsDataList[i]);
                        }

                        int gcount = client.GoodsDataList[i].GCount;
                        int subNum = Global.GMin(gcount, totalNum - count);
                        ret = NotifyUseGoods(sl, tcpClientPool, pool, client, client.GoodsDataList[i], subNum, usingGoods, dontCalcLimitNum);
                        if (!ret)
                        {
                            break;
                        }

                        count += subNum;
                        if (count >= totalNum)
                        {
                            break;
                        }

                        if (subNum >= gcount)
                        {
                            i--;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 从用户非绑定物品中扣除消耗的数量 [4/30/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsID"></param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        public bool NotifyUseNotBindGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int goodsID, int totalNum, bool usingGoods, out bool usedBinding, out bool usedTimeLimited, bool dontCalcLimitNum = false)
        {
            usedBinding = false;
            usedTimeLimited = false;
            bool ret = false;
            int count = 0;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].GoodsID == goodsID)
                    {
                        if (Global.IsGoodsTimeOver(client.GoodsDataList[i]) || Global.IsGoodsNotReachStartTime(client.GoodsDataList[i]))
                        {
                            continue; //已经超时无法再使用
                        }

                        if (client.GoodsDataList[i].Binding > 0)
                        {
                            continue;
                        }

                        if (!usedBinding)
                        {
                            usedBinding = (client.GoodsDataList[i].Binding > 0); //判断是否使用了绑定的物品
                        }

                        if (!usedTimeLimited)
                        {
                            usedTimeLimited = Global.IsTimeLimitGoods(client.GoodsDataList[i]);
                        }

                        int gcount = client.GoodsDataList[i].GCount;
                        int subNum = Global.GMin(gcount, totalNum - count);
                        ret = NotifyUseGoods(sl, tcpClientPool, pool, client, client.GoodsDataList[i], subNum, usingGoods, dontCalcLimitNum);
                        if (!ret)
                        {
                            break;
                        }

                        count += subNum;
                        if (count >= totalNum)
                        {
                            break;
                        }

                        if (subNum >= gcount)
                        {
                            i--;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 物品掉落
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        /// <returns></returns>
        public bool FallRoleGoods(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, GoodsData goodsData)
        {
            //修改内存中物品记录
            if (null == goodsData)
            {
                //不做处理
                return false;
            }

            if (Global.IsGoodsTimeOver(goodsData) || Global.IsGoodsNotReachStartTime(goodsData))
            {
                return false; //已经超时无法再使用
            }

            if (goodsData.GCount <= 0)
            {
                return false; //个数已经为0，无法使用， 防止0个数刷物品后，使用。
            }

            int gcount = goodsData.GCount;
            string strcmd = "";

            int subNum = 1;
            if (Global.GetGoodsDefaultCount(goodsData.GoodsID) > 1)
            {
                subNum = goodsData.GCount;
            }

            gcount = goodsData.GCount - subNum;
            TCPOutPacket tcpOutPacket = null;

            //向DBServer请求修改物品
            string[] dbFields = null;
            strcmd = Global.FormatUpdateDBGoodsStr(client.RoleID, goodsData.Id, "*", "*", "*", "*", "*", "*", "*", gcount, "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*"); // 卓越信息 [12/13/2013 LiaoWei] 装备转生
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields, client.ServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(-1, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                /*
                strcmd = string.Format("{0}:{1}:{2}", -1, goodsData.Id, gcount);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);*/
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return false;
            }

            if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[1]) < 0)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(-2, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
                /*
                strcmd = string.Format("{0}:{1}:{2}", -2, goodsData.Id, gcount);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);*/
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                return false;
            }

            //修改内存中物品记录
            if (gcount > 0)
            {
                goodsData.GCount = gcount;
            }
            else
            {
                goodsData.GCount = 0;
                Global.RemoveGoodsData(client, goodsData);
            }

            //写入角色物品的得失行为日志(扩展)
            Global.ModRoleGoodsEvent(client, goodsData, -subNum, "物品掉落");
            EventLogManager.AddGoodsEvent(client, OpTypes.AddOrSub, OpTags.None, goodsData.GoodsID, goodsData.Id, -subNum, goodsData.GCount, "物品掉落");

            tcpOutPacket = DataHelper.ObjectToTCPOutPacket(new SC_SprUseGoods(0, goodsData.Id, gcount), pool, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);
            /*
            strcmd = string.Format("{0}:{1}:{2}", 0, goodsData.Id, gcount);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USEGOODS);*/
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }

            // 属性改造 去掉 负重[8/15/2013 LiaoWei]
            //重新计算角色负重(单个物品)
            /*if (Global.UpdateGoodsWeight(client, goodsData, subNum, false, goodsData.Using > 0))
            {
                //重量属性更新通知
                GameManager.ClientMgr.NotifyUpdateWeights(sl, pool, client);
            }*/

            return true;
        }

        #endregion 物品消耗

        #region 金币处理

        /// <summary>
        /// Thông báo tiền thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfMoneyChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.Money1, client.Gold);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_MONEYCHANGE);
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

        public bool SubUserYinLiang(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subYinLiang, string strFrom)
        {
            int oldYinLiang = client.YinLiang;

            //先锁定
            lock (client.YinLiangMutex)
            {
                if (client.YinLiang < subYinLiang)
                {
                    return false; //银两余额不足
                }

                // 记录旧值
                int oldValue = client.YinLiang;
                // 优先修改gs的缓存值
                client.YinLiang -= subYinLiang;

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, -subYinLiang); //只发减少的量
                string[] dbFields = null;

                try
                {
                    dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERYINLIANG_CMD, strcmd, client.ServerId);
                }
                catch (Exception ex)
                {
                    DataHelper.WriteExceptionLogEx(ex, string.Format("CMD_DB_UPDATEUSERYINLIANG_CMD Faild"));
                    // 如果扣钱的时候出现异常，这时不能判断db是否执行了扣费操作
                    // 为了保证不出问题，不恢复原始值，使gs的缓存小于db的缓存
                    // 避免db扣费后，gs的缓存大于db缓存
                    // client.YinLiang = oldValue;
                    return false;
                }

                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    // 失败后回滚原值
                    client.YinLiang = oldValue;
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    // 失败后回滚原值
                    client.YinLiang = oldValue;
                    return false; //银两扣除失败，余额不足
                }

                client.YinLiang = Convert.ToInt32(dbFields[1]);

                if (0 != subYinLiang)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "金币", strFrom, client.RoleName, "系统", "减少", subYinLiang, client.ZoneID, client.strUserID, client.YinLiang, client.ServerId);
                }
            }

            // 银两更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserYinLiangChange(sl, pool, client);

            //写入角色银两增加/减少日志
            Global.AddRoleYinLiangEvent(client, oldYinLiang);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.Use, MoneyTypes.YinLiang, -subYinLiang, client.YinLiang, strFrom);

            return true;
        }

        public bool AddUserYinLiang(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addYinLiang, string strFrom)
        {
            int oldYinLiang = client.YinLiang;

            //先锁定
            lock (client.YinLiangMutex)
            {
                // 已经超过上限就直接存入仓库里
                if (oldYinLiang >= Global.Max_Role_YinLiang)
                {
                    return AddUserStoreYinLiang(sl, tcpClientPool, pool, client, addYinLiang, strFrom);
                }

                if (oldYinLiang + addYinLiang > Global.Max_Role_YinLiang)
                {
                    long newValue = Global.GMax(0, oldYinLiang + addYinLiang - Global.Max_Role_YinLiang);
                    // 超过上限的部分就直接存入仓库里
                    addYinLiang = Global.GMax(0, Global.Max_Role_YinLiang - oldYinLiang);
                    AddUserStoreYinLiang(sl, tcpClientPool, pool, client, newValue, strFrom);
                }

                if (0 == addYinLiang)
                {
                    return true;
                }

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, addYinLiang); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERYINLIANG_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //银两添加失败
                }

                // 先锁定
                client.YinLiang = Convert.ToInt32(dbFields[1]);

                if (0 != addYinLiang)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "金币", strFrom, "系统", client.RoleName, "增加", addYinLiang, client.ZoneID, client.strUserID, client.YinLiang, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YinLiang, addYinLiang, client.YinLiang, strFrom);
                }
            }

            //银两增加的时候通知成就系统
            if (addYinLiang > 0)
            {
                ChengJiuManager.OnTongQianIncrease(client);
            }

            // 银两更新通知(只通知自己)
            //GameManager.ClientMgr.NotifySelfUserYinLiangChange(sl, pool, client);
            GameManager.ClientMgr.NotifySelfUserMoneyChange(sl, pool, client);

            //写入角色银两增加/减少日志
            Global.AddRoleYinLiangEvent(client, oldYinLiang);

            return true;
        }

        public void NotifySelfUserYinLiangChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.YinLiang);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USERYINLIANGCHANGE);
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
        /// 添加游戏金币1
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddMoney1(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addMoney, string strFrom, bool writeToDB = true)
        {
            int oldMoney = client.Money1;

            // 已经超过上限就直接存入仓库里
            if (oldMoney >= Global.Max_Role_Money)
            {
                return AddUserStoreMoney(sl, tcpClientPool, pool, client, addMoney, strFrom);
            }

            if (oldMoney + addMoney > Global.Max_Role_YinLiang)
            {
                long newValue = Global.GMax(0, oldMoney + addMoney - Global.Max_Role_YinLiang);
                // 超过上限的部分就直接存入仓库里
                addMoney = Global.GMax(0, Global.Max_Role_Money - oldMoney);
                AddUserStoreMoney(sl, tcpClientPool, pool, client, newValue, strFrom);
            }

            if (0 == addMoney)
            {
                return true;
            }

            if (writeToDB)
            {
                //先DBServer请求扣费
                //string[] dbFields = null;
                string strcmd = string.Format("{0}:{1}", client.RoleID, client.Money1 + addMoney);
                //TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD, strcmd, out dbFields);
                //if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
                //{
                //    return false;
                //}

                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD,
                    strcmd,
                    null, client.ServerId);

                long nowTicks = TimeUtil.NOW();
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD, nowTicks);
            }

            client.Money1 = client.Money1 + addMoney; //加钱

            // 钱更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);
            if (0 != addMoney)
            {
                GameManager.logDBCmdMgr.AddDBLogInfo(-1, "绑金", strFrom, "系统", client.RoleName, "增加", addMoney, client.ZoneID, client.strUserID, client.Money1, client.ServerId);
                EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.Use, MoneyTypes.TongQian, addMoney, client.Money1, strFrom);
            }

            GameManager.SystemServerEvents.AddEvent(string.Format("角色添加金钱, roleID={0}({1}), Money={2}, addMoney={3}", client.RoleID, client.RoleName, client.Money1, addMoney), EventLevels.Record);
            return true;
        }

        /// <summary>
        /// 添加游戏金币1
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addMoney"></param>
        /// <param name="strFrom"></param>
        /// <param name="writeToDB"></param>
        /// <returns></returns>
        public bool AddMoney1(KPlayer client, int addMoney, string strFrom, bool writeToDB = true)
        {
            return AddMoney1(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, addMoney, strFrom, writeToDB);
        }

        /// <summary>
        /// 扣除游戏金币1
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubMoney1(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, string strFrom)
        {
            if (client.Money1 - subMoney < 0)
            {
                subMoney = client.Money1;
            }

            //先DBServer请求扣费
            //string[] dbFields = null;
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.Money1 - subMoney);
            //TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD, strcmd, out dbFields);
            //if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            //{
            //    return false;
            //}
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD,
                strcmd,
                null, client.ServerId);

            long nowTicks = TimeUtil.NOW();
            Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD, nowTicks);

            // 先锁定
            client.Money1 = client.Money1 - subMoney; //扣费

            // 钱更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);
            if (0 != subMoney)
            {
                GameManager.logDBCmdMgr.AddDBLogInfo(-1, "绑金", strFrom, client.RoleName, "系统", "减少", subMoney, client.ZoneID, client.strUserID, client.Money1, client.ServerId);
            }
            GameManager.SystemServerEvents.AddEvent(string.Format("角色扣除金钱, roleID={0}({1}), Money={2}, subMoney={3}", client.RoleID, client.RoleName, client.Money1, subMoney), EventLevels.Record);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.Use, MoneyTypes.TongQian, -subMoney, client.Money1, strFrom);

            return true;
        }

        #endregion 金币处理

        #region 元宝处理

        /// <summary>
        /// Thông báo giá trị đồng, đồng khóa thay đổi
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserMoneyChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.UserMoney, client.YinLiang);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USERMONEYCHANGE);
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
        /// 通知用户更新钻石
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserMoneyChange(KPlayer client)
        {
            client.sendCmd((int)TCPGameServerCmds.CMD_SPR_USERMONEYCHANGE, string.Format("{0}:{1}", client.RoleID, client.UserMoney));
        }

        /// <summary>
        /// 添加用户点卷
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addMoney, string msg, ActivityTypes result = ActivityTypes.None, string param = "")
        {
            //先锁定
            lock (client.UserMoneyMutex)
            {
                //先DBServer请求扣费
                //只发增量
                string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, addMoney, (int)result, param);// 发放钻石的活动类型
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //元宝添加失败
                }

                // 先锁定
                client.UserMoney = Convert.ToInt32(dbFields[1]);
                int nTotalMoney = Convert.ToInt32(dbFields[2]);

                //尝试更新钻皇等级,登录的时候也要判断，防止用户离线充值，钻皇等级得不到触发更新
                //Global.TryToActivateSpecialZuanHuangLevel(client);

                // 增加Vip经验 Begin[2/20/2014 LiaoWei]
                if (nTotalMoney > 0)
                {
                    Global.ProcessVipLevelUp(client);
                }

                // 增加Vip经验 End[2/20/2014 LiaoWei]

                //添加日志
                Global.AddRoleUserMoneyEvent(client, "+", addMoney, msg);

                // 添加日志到日志数据库
                if (0 != addMoney)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "钻石", msg, "系统", client.RoleName, "增加", addMoney, client.ZoneID, client.strUserID, client.UserMoney, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanBao, addMoney, client.UserMoney, msg);
                }
            }

            // 钱更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserMoneyChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// 添加离线用户点卷
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddOfflineUserMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int otherRoleID, string roleName, int addMoney, string msg, int zoneid, string userid)
        {
            //先锁定
            {
                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", otherRoleID, addMoney); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD, strcmd, GameManager.LocalServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //元宝添加失败
                }

                //添加日志
                Global.AddRoleUserMoneyEvent(otherRoleID, "+", addMoney, msg);

                // 添加日志到日志数据库
                if (0 != addMoney)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "钻石", msg, "系统", roleName, "增加", addMoney, zoneid, userid, Convert.ToInt32(dbFields[1]), GameManager.LocalServerId);
                    EventLogManager.AddMoneyEvent(GameManager.ServerId, zoneid, userid, otherRoleID, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanBao, addMoney, -1, msg);
                }
            }

            return true;
        }

        /// <summary>
        /// 扣除用户点卷
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubUserMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, string msg, bool bIsAddVipExp = true, int type = 1, bool isAddFund = true)
        {
            //先锁定
            lock (client.UserMoneyMutex)
            {
                subMoney = Math.Abs(subMoney);
                if (client.UserMoney < subMoney)
                {
                    return false; //元宝余额不足
                }

                // 记录原始值
                int oldValue = client.UserMoney;
                // 优先把GameServer缓存更新掉
                client.UserMoney -= subMoney;

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, -subMoney); //只发减少的量
                string[] dbFields = null;

                try
                {
                    dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD, strcmd, client.ServerId);
                }
                catch (Exception ex)
                {
                    DataHelper.WriteExceptionLogEx(ex, string.Format("CMD_DB_UPDATEUSERMONEY_CMD Faild"));

                    // 如果扣钱的时候出现异常，这时不能判断db是否执行了扣费操作
                    // 为了保证不出问题，不恢复原始值，使gs的缓存小于db的缓存
                    // 避免db扣费后，gs的缓存大于db缓存
                    // client.UserMoney = oldValue;
                    return false;
                }

                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    // 扣钱失败恢复原始值
                    client.UserMoney = oldValue;
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    // 扣钱失败恢复原始值
                    client.UserMoney = oldValue;
                    return false; //元宝扣除失败，余额不足
                }

                client.UserMoney = Convert.ToInt32(dbFields[1]);

                // 钻石消费后，刷新相应的图标状态
                client._IconStateMgr.FlushUsedMoneyconState(client);
                client._IconStateMgr.CheckJieRiActivity(client, false);
                client._IconStateMgr.SendIconStateToClient(client);

                //每笔消费都存盘
                if (bIsAddVipExp)
                {
                    Global.SaveConsumeLog(client, subMoney, type);

                    if (isAddFund)
                        FundManager.FundMoneyCost(client, subMoney);

                    SpecialActivity act = HuodongCachingMgr.GetSpecialActivity();
                    if (act != null)
                    {
                        act.MoneyConst(client, subMoney);
                    }
                }
                // 添加日志
                Global.AddRoleUserMoneyEvent(client, "-", subMoney, msg);

                // 添加日志到日志数据库
                if (0 != subMoney)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "钻石", msg, client.RoleName, "系统", "减少", subMoney, client.ZoneID, client.strUserID, client.UserMoney, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanBao, -subMoney, client.UserMoney, msg);
                }
            }

            // 钱更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserMoneyChange(sl, pool, client);

            return true;
        }

        /// <summary>
        /// 扣除钻石
        /// </summary>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <param name="bIsAddVipExp"></param>
        ///  <param name="type">消费钻石的类型</param>
        /// <returns></returns>
        public bool SubUserMoney(KPlayer client, int subMoney, string msg, bool savedb = true, bool bIsAddVipExp = true, int type = 1, bool isAddFund = true)
        {
            //先锁定
            lock (client.UserMoneyMutex)
            {
                subMoney = Math.Abs(subMoney);
                if (client.UserMoney < subMoney)
                {
                    return false; //元宝余额不足
                }

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, -subMoney); //只发减少的量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 3)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //元宝扣除失败，余额不足
                }

                client.UserMoney = Convert.ToInt32(dbFields[1]);

                // 钻石消费后，刷新相应的图标状态
                client._IconStateMgr.FlushUsedMoneyconState(client);
                client._IconStateMgr.SendIconStateToClient(client);
                //每笔消费都存盘
                if (savedb)
                {
                    Global.SaveConsumeLog(client, subMoney, type);

                    if (isAddFund)
                        FundManager.FundMoneyCost(client, subMoney);

                    SpecialActivity act = HuodongCachingMgr.GetSpecialActivity();
                    if (act != null)
                    {
                        act.MoneyConst(client, subMoney);
                    }
                }

                //添加日志
                Global.AddRoleUserMoneyEvent(client, "-", subMoney, msg);

                // 添加日志到日志数据库
                if (0 != subMoney)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "钻石", msg, client.RoleName, "系统", "减少", subMoney, client.ZoneID, client.strUserID, client.UserMoney, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanBao, -subMoney, client.UserMoney, msg);
                }
            }

            // 钱更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserMoneyChange(client);

            return true;
        }

        /// <summary>
        /// 扣除用户点卷[有金子，扣金子，没有金子或者金子不足，再扣元宝]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        //public bool SubUserMoney2(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, out int subYuanBao, out int subGold)
        //{
        //    //优先扣除金币
        //    //需要扣除的金币
        //    subGold = 0;

        //    //需要扣除的元宝
        //    subYuanBao = 0;

        //    //允许扣除金子，则扣除金子，默认是允许扣除金子
        //    if ("1" == GameManager.GameConfigMgr.GetGameConfigItemStr("allowsubgold", "1"))
        //    {
        //        if (client.Gold > 0)
        //        {
        //            subGold = Global.GMin(client.Gold, subMoney);
        //        }
        //    }

        //    subYuanBao = subMoney - subGold;

        //    //扣除金币
        //    if (subGold > 0)
        //    {
        //        //先DBServer请求扣费
        //        //扣除用户点卷
        //        if (!GameManager.ClientMgr.SubUserGold(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, subGold))
        //        {
        //            return false;
        //        }
        //    }

        //    //扣除元宝
        //    if (subYuanBao > 0)
        //    {
        //        //先DBServer请求扣费
        //        //扣除用户点卷
        //        if (!GameManager.ClientMgr.SubUserMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, subYuanBao))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// 判断总的元宝和金币个数
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        //public int GetCanUseUserMoneyAndGold(KPlayer client)
        //{
        //    int currentGold = 0;

        //    //允许扣除金子，则扣除金子，默认是允许扣除金子
        //    if ("1" == GameManager.GameConfigMgr.GetGameConfigItemStr("allowsubgold", "1"))
        //    {
        //        if (client.Gold > 0)
        //        {
        //            currentGold = client.Gold;
        //        }
        //    }

        //    return client.UserMoney + currentGold;
        //}

        /// <summary>
        /// 查询历史充值记录
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
        /// 查询历史充值记录
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public int QueryTotaoChongZhiMoney(string userID, int zoneID, int ServerId)
        {
            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}", userID, zoneID);
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYCHONGZHIMONEY, strcmd, ServerId);
            if (null == dbFields) return 0;
            if (dbFields.Length != 1)
            {
                return 0;
            }

            return Global.SafeConvertToInt32(dbFields[0]);
        }

        /// <summary>
        /// 查询今天的充值额
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
            if (null == dbFields) return 0;
            if (dbFields.Length != 1)
            {
                return 0;
            }

            return Global.SafeConvertToInt32(dbFields[0]);
        }

        /// <summary>
        /// 添加用户点卷(不在线的情况下)
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserMoneyOffLine(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int roleID, int addMoney, string msg, int zoneid, string userid)
        {
            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}", roleID, addMoney); //只发增量
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD, strcmd, GameManager.LocalServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 3)
            {
                return false;
            }

            // 先锁定
            if (Convert.ToInt32(dbFields[1]) < 0)
            {
                return false; //元宝添加失败
            }

            //添加日志
            Global.AddRoleUserMoneyEvent(roleID, "+", addMoney, msg);

            // 添加日志到日志数据库
            if (0 != addMoney)
            {
                GameManager.logDBCmdMgr.AddDBLogInfo(-1, "钻石", msg, "系统", "" + roleID, "增加", addMoney, zoneid, userid, Convert.ToInt32(dbFields[1]), GameManager.LocalServerId);
                EventLogManager.AddMoneyEvent(GameManager.ServerId, zoneid, userid, roleID, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanBao, addMoney, -1, msg);
            }

            return true;
        }

        #endregion 元宝处理

        #region 绑定元宝处理

        /// <summary>
        /// 金币通知(只通知自己)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserGoldChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.Gold);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_USERGOLDCHANGE);
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
        /// 用户添加绑钻
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addGold"></param>
        /// <returns></returns>
        public bool AddUserGold(KPlayer client, int addGold, string strFrom)
        {
            return AddUserGold(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, addGold, strFrom);
        }

        /// <summary>
        /// 添加用户绑钻
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserGold(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int addGold, string strFrom = "")
        {
            int oldGold = client.Gold;

            //先锁定
            lock (client.GoldMutex)
            {
                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, addGold); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERGOLD_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //金币添加失败
                }

                // 先锁定
                client.Gold = Convert.ToInt32(dbFields[1]);

                // 添加日志到日志数据库
                if (0 != addGold)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "绑定钻石", strFrom, client.RoleName, "系统", "增加", addGold, client.ZoneID, client.strUserID, client.Gold, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.BindYuanBao, addGold, client.Gold, strFrom);
                }
            }

            // 金币更新通知(只通知自己)
            //GameManager.ClientMgr.NotifySelfUserGoldChange(sl, pool, client);
            GameManager.ClientMgr.NotifySelfMoneyChange(sl, pool, client);

            //写入角色金币增加/减少日志
            Global.AddRoleGoldEvent(client, oldGold);

            return true;
        }

        /// <summary>
        /// 添加用户金币[离线添加--->如果突然在线，走在线添加路线]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserGoldOffLine(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int roleID, int addGold, string strFrom = "", String strUserID = "")
        {
            KPlayer client = GameManager.ClientMgr.FindClient(roleID);
            if (null != client)
            {
                return AddUserGold(sl, tcpClientPool, pool, client, addGold, strFrom);
            }
            else
            {
                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", roleID, addGold); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERGOLD_CMD, strcmd, GameManager.LocalServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //金币添加失败
                }

                if (0 != addGold)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "绑定钻石", strFrom, "" + roleID, "系统", "增加", addGold, 0, strUserID, Convert.ToInt32(dbFields[1]), GameManager.LocalServerId);
                    EventLogManager.AddMoneyEvent(GameManager.ServerId, 0, strUserID, roleID, OpTypes.AddOrSub, OpTags.None, MoneyTypes.BindYuanBao, addGold, -1, strFrom);
                }
            }

            return true;
        }

        /// <summary>
        /// 扣除用户绑钻
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubUserGold(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subGold, string msg = "无")
        {
            int oldGold = client.Gold;

            //先锁定
            lock (client.GoldMutex)
            {
                if (client.Gold < subGold)
                {
                    return false; //金币余额不足
                }

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, -subGold); //只发减少的量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERGOLD_CMD, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false; //银两扣除失败，余额不足
                }

                client.Gold = Convert.ToInt32(dbFields[1]);

                // 添加日志到日志数据库
                if (0 != subGold)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "绑定钻石", msg, client.RoleName, "系统", "减少", subGold, client.ZoneID, client.strUserID, client.Gold, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.BindYuanBao, -subGold, client.Gold, msg);
                }
            }

            // 金币更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserGoldChange(sl, pool, client);

            //写入角色银两增加/减少日志
            Global.AddRoleGoldEvent(client, oldGold);

            return true;
        }

        /// <summary>
        /// 扣除用户绑钻
        /// </summary>
        /// <param name="client"></param>
        /// <param name="subGold"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SubUserGold(KPlayer client, int subGold, string msg = "无")
        {
            return SubUserGold(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, subGold, msg);
        }

        #endregion 绑定元宝处理

        #region 银两处理

        /// <summary>
        /// 添加离线用户银两
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddOfflineUserYinLiang(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, string userID, int roleID, string roleName, int addYinLiang, string strFrom, int zoneid)
        {
            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}", roleID, addYinLiang); //只发增量
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEUSERYINLIANG_CMD, strcmd, GameManager.LocalServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 2)
            {
                return false;
            }

            // 先锁定
            if (Convert.ToInt32(dbFields[1]) < 0)
            {
                return false; //银两添加失败
            }

            if (0 != addYinLiang)
            {
                GameManager.logDBCmdMgr.AddDBLogInfo(-1, "金币", strFrom, "系统", "" + roleID, "增加", addYinLiang, zoneid, userID, Convert.ToInt32(dbFields[1]), GameManager.LocalServerId);
            }
            //写入角色银两增加/减少日志
            Global.AddRoleYinLiangEvent2(userID, roleID, roleName, addYinLiang);

            return true;
        }

        #endregion 银两处理

        #region 角色间物品交换处理

        /// <summary>
        /// 将某指定的物品转移给某个角色
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="gd"></param>
        /// <param name="toClient"></param>
        /// <param name="bAddToTarget">是否添加到目标角色 ChenXiaojun</param>
        /// <returns></returns>
        public bool MoveGoodsDataToOtherRole(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, GoodsData gd,
                                             KPlayer fromClient, KPlayer toClient, bool bAddToTarget = true)
        {
            //先DBServer请求扣费
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

    
            EventLogManager.AddGoodsEvent(fromClient, OpTypes.AddOrSub, OpTags.None, gd.GoodsID, gd.Id, -gd.GCount, 0, "物品转给别人");
            GameManager.logDBCmdMgr.AddDBLogInfo(gd.Id, Global.ModifyGoodsLogName(gd), "物品转给别人(在线)", fromClient.RoleName, toClient.RoleName, "移动", -gd.GCount, fromClient.ZoneID, toClient.strUserID, -1, GameManager.LocalServerId, gd);
            if (bAddToTarget)
            {
                string[] dbFields2 = null;
                gd.BagIndex = Global.GetIdleSlotOfBagGoods(toClient); //找到空闲的包裹格子
                strcmd = Global.FormatUpdateDBGoodsStr(toClient.RoleID, gd.Id, "*", "*", "*", "*", "*", "*", "*", "*", "*", gd.BagIndex, "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*"); // 卓越一击 [12/13/2013 LiaoWei] 装备转生
                Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields2, GameManager.LocalServerId);

                // 先锁定
                Global.AddGoodsData(toClient, gd);

                EventLogManager.AddGoodsEvent(toClient, OpTypes.AddOrSub, OpTags.None, gd.GoodsID, gd.Id, gd.GCount, gd.GCount, "得到他人物品");
                GameManager.logDBCmdMgr.AddDBLogInfo(gd.Id, Global.ModifyGoodsLogName(gd), "得到他人物品(在线)", fromClient.RoleName, toClient.RoleName, "移动", gd.GCount, toClient.ZoneID, toClient.strUserID, -1, GameManager.LocalServerId, gd);
                // 处理任务
                ProcessTask.Process(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient, -1, -1, gd.GoodsID, TaskTypes.BuySomething);

                GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient,
                    gd.Id, gd.GoodsID, gd.Forge_level, gd.GCount, gd.Binding, gd.Site, 1, gd.Endtime, gd.Strong, gd.BagIndex, gd.Using, gd.Props,gd.Series,gd.OtherParams);
            }

            return true;
        }

        /// <summary>
        /// 将某指定的物品转移给某个角色
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="gd"></param>
        /// <param name="toClient"></param>
        /// <param name="bAddToTarget">是否添加到目标角色 ChenXiaojun</param>
        /// <returns></returns>
        public bool MoveGoodsDataToOfflineRole(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, GoodsData gd,
                                               string fromUserID, int fromRoleID, string fromRoleName, int fromRoleLevel, string toUserID,
                                               int toRoleID, string toRoleName, int toRoleLevel, bool bAddToTarget, int zoneid)
        {
            //先DBServer请求扣费
            string[] dbFields = null;
            string strcmd = string.Format("{0}:{1}:{2}", toRoleID, fromRoleID, gd.Id);
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_MOVEGOODS_CMD, strcmd, out dbFields, GameManager.LocalServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                LogManager.WriteLog(LogTypes.SQL, string.Format("向DB请求转移物品时失败{0}->{1}", fromRoleName, toRoleName));
                return false;
            }

            if (dbFields.Length < 4 || Convert.ToInt32(dbFields[3]) < 0)
            {
                LogManager.WriteLog(LogTypes.SQL, string.Format("向DB请求转移物品时失败{0}->{1},错误码{2}", fromRoleName, toRoleName, dbFields[3]));
                return false;
            }

           
            GameManager.logDBCmdMgr.AddDBLogInfo(gd.Id, Global.ModifyGoodsLogName(gd), "物品转给别人(离线)", fromRoleName/*fromUserID*/, toRoleName/*toUserID*/, "移动", -gd.GCount, zoneid, fromUserID, -1, GameManager.LocalServerId, gd);
            if (bAddToTarget)
            {
                
                GameManager.logDBCmdMgr.AddDBLogInfo(gd.Id, Global.ModifyGoodsLogName(gd), "得到他人物品(离线)", fromRoleName/*fromUserID*/, toRoleName/*toUserID*/, "移动", gd.GCount, zoneid, toUserID, -1, GameManager.LocalServerId, gd);

                KPlayer toClient = GameManager.ClientMgr.FindClient(toRoleID);
                if (null != toClient)
                {
                    string[] dbFields2 = null;
                    gd.BagIndex = Global.GetIdleSlotOfBagGoods(toClient); //找到空闲的包裹格子
                    strcmd = Global.FormatUpdateDBGoodsStr(toRoleID, gd.Id, "*", "*", "*", "*", "*", "*", "*", "*", "*", gd.BagIndex, "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*"); // 卓越一击 [12/13/2013 LiaoWei] 装备转生
                    Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields2, GameManager.LocalServerId);

                    Global.AddGoodsData(toClient, gd);

                    // 处理任务
                    ProcessTask.Process(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient, -1, -1, gd.GoodsID, TaskTypes.BuySomething);

                    GameManager.ClientMgr.NotifySelfAddGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, toClient,
                   gd.Id, gd.GoodsID, gd.Forge_level, gd.GCount, gd.Binding, gd.Site, 1, gd.Endtime, gd.Strong, gd.BagIndex, gd.Using, gd.Props,gd.Series,gd.OtherParams);
                }
            }

            return true;
        }

        #endregion 角色间物品交换处理

        #region 摆摊处理

        /// <summary>
        /// 通知请求角色摆摊的指令信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyGoodsStallCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int status, int stallType)
        {
            string strcmd = string.Format("{0}:{1}:{2}", status, client.RoleID, stallType);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GOODSSTALL);
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
        /// 通知请求物品摆摊数据的指令信息
        /// </summary>
        /// <param name="client"></param>
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
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        /// <summary>
        /// 通知所有在线用户某个精灵的开始摆摊(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteStartStall(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            if (null == client.StallDataItem) return;

            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StallDataItem.StallName);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_STALLNAME);
        }

        #endregion 摆摊处理

        #region 交易市场购买

        /// <summary>
        /// 通知用户某个精灵的购买了某个物品
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteMarketBuy(SocketListener sl, TCPOutPacketPool pool, KPlayer client, KPlayer otherClient, int result, int buyType, int goodsDbID, int goodsID, int nID = (int)TCPGameServerCmds.CMD_SPR_MARKETBUYGOODS)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", result, buyType, client.RoleID, null != otherClient ? otherClient.RoleID : -1, null != otherClient ? Global.FormatRoleName(otherClient, otherClient.RoleName) : "", goodsDbID, goodsID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
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
        /// 通知用户某个精灵的购买了某个物品
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteMarketBuy2(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int otherRoleID, int result, int buyType, int goodsDbID, int goodsID, int otherRoleZoneID, string otherRoleName, int nID = (int)TCPGameServerCmds.CMD_SPR_MARKETBUYGOODS)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", result, buyType, client.RoleID, otherRoleID, Global.FormatRoleName3(otherRoleID, otherRoleName), goodsDbID, goodsID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
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
        /// 通知用户某个精灵的交易市场的名称（开放状态）
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteMarketName(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string marketName, int offlineMarket)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, marketName, offlineMarket);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_OPENMARKET);
        }

        #endregion 交易市场购买

        #region 冷却时间处理

        /// <summary>
        /// 消除冷却时间处理
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public void RemoveCoolDown(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int type, int code)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, type, code);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_REMOVE_COOLDOWN);
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

        #endregion 冷却时间处理

        #region 角色在线时长

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
            if (client.ForceShenFenZheng)
            {
                client.ForceShenFenZheng = false;
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

            DailyActiveManager.ProcessOnlineForDailyActive(client);

            client._IconStateMgr.CheckFuMeiRiZaiXian(client);
            client._IconStateMgr.SendIconStateToClient(client);

            int isAdult = GameManager.OnlineUserSession.FindUserAdult(client.ClientSocket);
            if (isAdult > 0) //
            {
                return;
            }
        }

        #endregion 角色在线时长

        #region 特效播放

        /// <summary>
        /// 通知其自己，开始播放特效
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfDeco(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int decoID, int decoType, int toBody, int toX, int toY, int shakeMap, int toX1, int toY1, int moveTicks, int alphaTicks)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", client.RoleID, decoID, decoType, toBody, toX, toY, shakeMap, toX1, toY1, moveTicks, alphaTicks);
            TCPOutPacket tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYDECO);
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
        /// 通知其自己和其他人，自己开始播放特效(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMyDeco(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int decoID, int decoType, int toBody, int toX, int toY, int shakeMap, int toX1, int toY1, int moveTicks, int alphaTicks, List<Object> objsList = null)
        {
            if (null == objsList)
            {
                objsList = Global.GetAll9Clients(client);
            }

            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", client.RoleID, decoID, decoType, toBody, toX, toY, shakeMap, toX1, toY1, moveTicks, alphaTicks);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYDECO);
        }

        /// <summary>
        /// 通知其自己和其他人，自己开始播放特效(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersMyDeco(SocketListener sl, TCPOutPacketPool pool, IObject obj, int mapCode, int copyMapID, int decoID, int decoType, int toBody, int toX, int toY, int shakeMap, int toX1, int toY1, int moveTicks, int alphaTicks, List<Object> objsList = null)
        {
            if (null == objsList)
            {
                if (null == obj)
                {
                    objsList = Global.GetAll9Clients2(mapCode, toX, toY, copyMapID);
                }
                else
                {
                    objsList = Global.GetAll9Clients(obj);
                }
            }

            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", -1, decoID, decoType, toBody, toX, toY, shakeMap, toX1, toY1, moveTicks, alphaTicks);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYDECO);
        }

        #endregion 特效播放

        #region Buffer数据处理

        /// <summary>
        /// 将新的Buffer数据通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBufferData(KPlayer client, BufferData bufferData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<BufferData>(bufferData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_BUFFERDATA);
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
        /// 将新的Buffer数据通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOtherBufferData(IObject self, BufferData bufferData)
        {
            OtherBufferData otherBufferData = new OtherBufferData()
            {
                BufferID = bufferData.BufferID,
                BufferVal = bufferData.BufferVal,
                BufferType = bufferData.BufferType,
                BufferSecs = bufferData.BufferSecs,
                StartTime = bufferData.StartTime,
            };

            switch (self.ObjectType)
            {
                case ObjectTypes.OT_MONSTER:
                    otherBufferData.RoleID = (self as Monster).RoleID;
                    break;

                case ObjectTypes.OT_CLIENT:
                    otherBufferData.RoleID = (self as KPlayer).RoleID;
                    break;

                case ObjectTypes.OT_NPC:
                    otherBufferData.RoleID = (self as NPC).NPCID;
                    break;

                case ObjectTypes.OT_FAKEROLE:
                    otherBufferData.RoleID = (self as FakeRoleItem).FakeRoleID;
                    break;

                default:
                    return;//暂不支持其它类型
            }

            byte[] bytes = DataHelper.ObjectToBytes<OtherBufferData>(otherBufferData);

            List<Object> objsList = Global.GetAll9Clients(self);
            if (null == objsList)
            {
                objsList = new List<object>();
            }

            if (objsList.IndexOf(self) < 0)
            {
                objsList.Add(self);
            }

            foreach (var obj in objsList)
            {
                KPlayer c = obj as KPlayer;
                if (null != c && c.CodeRevision >= 2)
                {
                    SendToClient(c, bytes, (int)TCPGameServerCmds.CMD_SPR_NOTIFYOTHERBUFFERDATA);
                }
            }
        }

        #endregion Buffer数据处理

        #region 角色收获经验

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
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
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

            // Nếu mà online quá số giờ chơi thì thôi 
            if (enableFilter)
            {
                experience = Global.FilterValue(client, experience);
            }

            // Nếu số EXP add vào lớn hơn 0
            if (experience > 0)
            {
                int oldLevel = client.m_Level;

                //Hàm xử lý các sự kiện khác kèm theo khi EXP tăng
                Global.EarnExperience(client, experience);

                if (writeToDB || (oldLevel != client.m_Level)) //Thực hiện update vào DB
                {
                    GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL,
                        string.Format("{0}:{1}:{2}", client.RoleID, client.m_Level, client.m_Experience),
                        null, client.ServerId);

                    long nowTicks = TimeUtil.NOW();
                    Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL, nowTicks);
                }

                // NẾU 2 LEVE KHÁC NHAU
                if (oldLevel != client.m_Level)
                {
                    /// Gọi sự kiện thay đổi cấp độ của nhân vật
                    client.OnLevelChanged();

                    // RERESSH LẠI ĐỒ ĐẠC NẾU YÊU CẦU CẤP ĐỘ
                    GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);

                    // NOTITFY SỐ LẦN CHUYỂN SINH
                    GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, true, true);

                    // NOTIFY LÊN LEVEL CHO TEAM
                    GameManager.ClientMgr.NotifyTeamUpLevel(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);

                

                    //Ghi lại logs tăng level cho nhân vật
                    Global.AddRoleUpgradeEvent(client, oldLevel);

                 
                    //Nhận quà thăng cấp
                    HuodongCachingMgr.ProcessKaiFuGiftAward(client);

                    /// Nhận quà thăng cấp tiếp
                    HuodongCachingMgr.ProcessUpLevelAward4_60Level_100Level(client, oldLevel, client.m_Level);

                    // Tặng cái buff j đó sau này sẽ xóa
                    WorldLevelManager.getInstance().UpddateWorldLevelBuff(client);

                    GlobalEventSource.getInstance().fireEvent(SevenDayGoalEvPool.Alloc(client, ESevenDayGoalFuncType.RoleLevelUp));
                    // MỞ KHÓA TRADE NẾU ĐỦ CÁP ĐỘ
                    TradeBlackManager.Instance().UpdateObjectExtData(client);
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
                string.Format("{0}:{1}:{2}", player.RoleID, level, player.m_Experience),
                null, player.ServerId);

            /// Thiết lập cấp độ cho nhân vật
            player.m_Level = level;

            /// Gọi sự kiện thay đổi cấp độ của nhân vật
            player.OnLevelChanged();

            // RERESSH LẠI ĐỒ ĐẠC NẾU YÊU CẦU CẤP ĐỘ
            GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player);

            // NOTITFY SỐ LẦN CHUYỂN SINH
            GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, true, true);

            // NOTIFY LÊN LEVEL CHO TEAM
            GameManager.ClientMgr.NotifyTeamUpLevel(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player);

            //Ghi lại logs thay đổi level cho nhân vật
            Global.AddRoleUpgradeEvent(player, oldLevel);

            //Nhận quà thăng cấp
            HuodongCachingMgr.ProcessKaiFuGiftAward(player);

            /// Nhận quà thăng cấp tiếp
            HuodongCachingMgr.ProcessUpLevelAward4_60Level_100Level(player, oldLevel, player.m_Level);

            // Tặng cái buff j đó sau này sẽ xóa
            WorldLevelManager.getInstance().UpddateWorldLevelBuff(player);

            GlobalEventSource.getInstance().fireEvent(SevenDayGoalEvPool.Alloc(player, ESevenDayGoalFuncType.RoleLevelUp));
            // MỞ KHÓA TRADE NẾU ĐỦ CÁP ĐỘ
            TradeBlackManager.Instance().UpdateObjectExtData(player);

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
                needExperience = KPlayerSetting.GetNeedExpUpExp(client.m_Level + 1);
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
        /// 为所有在线角色添加经验
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

                //为在线角色添加经验
                AddOnlieRoleExperience(client, addPercent);
            }
        }

        /// <summary>
        /// Trả về về lượng EXP cần thiết để lên level [XSea 2015/4/8]
        /// </summary>
        /// <param name="client">角色</param>
        public long GetCurRoleLvUpNeedExp(KPlayer client)
        {
            if (client == null)
                return 0;

            if (client.m_Level >= KPlayerSetting.GetMaxLevel())
                return 0;

            long lNeedExp = 0; // Số exp cần ban đầu

            // Cần bao nhiêu kinh nghiệm để nâng cấp cấp độ từ bảng kinh nghiệm
            lNeedExp = KPlayerSetting.GetNeedExpUpExp(client.m_Level);


            return lNeedExp;
        }

        #endregion 角色收获经验

      

        #region 搜索当前地图的用户并返回列表

        /// <summary>
        /// 搜索符合角色名符合字符串的用户并返回列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="startIndex"></param>
        public void SearchRolesByStr(KPlayer client, string roleName, int startIndex)
        {
            int index = startIndex, addCount = 0;
            KPlayer otherClient = null;
            List<SearchRoleData> roleDataList = new List<SearchRoleData>();
            while ((otherClient = GetNextClient(ref index)) != null)
            {
                if (-1 == otherClient.RoleName.IndexOf(roleName))
                {
                    continue;
                }

                roleDataList.Add(new SearchRoleData()
                {
                    RoleID = otherClient.RoleID,
                    RoleName = Global.FormatRoleName(otherClient, otherClient.RoleName),
                    RoleSex = otherClient.RoleSex,
                    Level = otherClient.m_Level,
                    Occupation = client.m_cPlayerFaction.GetFactionId(),
                    MapCode = otherClient.MapCode,
                    PosX = otherClient.PosX,
                    PosY = otherClient.PosY,
                    ChangeLifeLev = otherClient.ChangeLifeCount,
                });

                addCount++;
                if (addCount >= (int)SearchResultConsts.MaxSearchRolesNum)
                {
                    break;
                }
            }

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<SearchRoleData>>(roleDataList, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_SEARCHROLES);
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
        /// 列举用户并返回列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="startIndex"></param>
        public void ListMapRoles(KPlayer client, int startIndex)
        {
            ListRolesData listRolesData = new ListRolesData()
            {
                StartIndex = startIndex,
                TotalRolesCount = 0,
                PageRolesCount = (int)SearchResultConsts.MaxSearchRolesNum,
                SearchRoleDataList = new List<SearchRoleData>(),
            };

            List<SearchRoleData> roleDataList = listRolesData.SearchRoleDataList;

            List<Object> objsList = GetMapClients(client.MapCode);
            objsList = Global.FilterHideObjsList(objsList);
            if (null == objsList || objsList.Count <= 0)
            {
                SendListRolesDataResult(client, listRolesData);
                return;
            }

            List<KPlayer> clients = new List<KPlayer>();
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is KPlayer))
                {
                    continue;
                }

                if ((objsList[i] as KPlayer).TeamID > 0) //已经组队的不再显示
                {
                    continue;
                }

                clients.Add(objsList[i] as KPlayer);
            }

            listRolesData.TotalRolesCount = clients.Count;
            if (listRolesData.TotalRolesCount <= 0)
            {
                SendListRolesDataResult(client, listRolesData);
                return;
            }

            if (startIndex >= clients.Count)
            {
                startIndex = 0; //从0开始
            }

            int index = startIndex, addCount = 0;
            KPlayer otherClient = null;
            for (int i = 0; i < clients.Count; i++)
            {
                if (i < startIndex)
                {
                    continue;
                }

                otherClient = clients[i];
                roleDataList.Add(new SearchRoleData()
                {
                    RoleID = otherClient.RoleID,
                    RoleName = Global.FormatRoleName(otherClient, otherClient.RoleName),
                    RoleSex = otherClient.RoleSex,
                    Level = otherClient.m_Level,
                    Occupation = client.m_cPlayerFaction.GetFactionId(),
                    MapCode = otherClient.MapCode,
                    PosX = otherClient.PosX,
                    PosY = otherClient.PosY,

                    ChangeLifeLev = otherClient.ChangeLifeCount
                });

                addCount++;
                if (addCount >= (int)SearchResultConsts.MaxSearchRolesNum)
                {
                    break;
                }
            }

            SendListRolesDataResult(client, listRolesData);
        }

        /// <summary>
        /// 发送列列举地图上的角色的数据给客户端
        /// </summary>
        /// <param name="listRolesData"></param>
        private void SendListRolesDataResult(KPlayer client, ListRolesData listRolesData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<ListRolesData>(listRolesData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_LISTROLES);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 搜索当前地图的用户并返回列表

        #region 队伍查询

        /// <summary>
        /// 列举组队的队伍并返回列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="startIndex"></param>
        public void ListAllTeams(KPlayer client, int startIndex)
        {
            SearchTeamData searchTeamData = new SearchTeamData()
            {
                StartIndex = startIndex,
                TotalTeamsCount = 0,
                PageTeamsCount = (int)SearchResultConsts.MaxSearchTeamsNum,
                TeamDataList = null,
            };

            searchTeamData.TotalTeamsCount = GameManager.TeamMgr.GetTotalDataCount();
            if (searchTeamData.TotalTeamsCount <= 0)
            {
                SendListTeamsDataResult(client, searchTeamData);
                return;
            }

            if (startIndex >= searchTeamData.TotalTeamsCount)
            {
                startIndex = 0; //从0开始
            }

            searchTeamData.TeamDataList = GameManager.TeamMgr.GetTeamDataList(startIndex, searchTeamData.PageTeamsCount);
            SendListTeamsDataResult(client, searchTeamData);
        }

        /// <summary>
        /// 发送队伍列表的数据给客户端
        /// </summary>
        /// <param name="listRolesData"></param>
        private void SendListTeamsDataResult(KPlayer client, SearchTeamData searchTeamData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<SearchTeamData>(searchTeamData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_LISTTEAMS);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 队伍查询

        #region 日常任务

        /// <summary>
        /// 将新的日常任务数据通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDailyTaskData(KPlayer client)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<DailyTaskData>>(client.MyDailyTaskDataList, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_DAILYTASKDATA);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 日常任务

        #region 副本系统

        /// <summary>
        /// 将新的副本的数据通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyFuBenData(KPlayer client, FuBenData fuBenData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<FuBenData>(fuBenData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_FUBENDATA);
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
        /// 通知副本的开始信息(每一层图怪物清空时也会调用)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyFuBenBeginInfo(KPlayer client)
        {
            string strcmd = "";
            TCPOutPacket tcpOutPacket = null;

           

            int fuBenSeqID = FuBenManager.FindFuBenSeqIDByRoleID(client.RoleID);
            if (fuBenSeqID <= 0) //如果副本不存在
            {
                return;
            }

            int copyMapID = client.CopyMapID;
            if (copyMapID <= 0) //如果不是在副本地图中
            {
                return;
            }

            int fuBenID = FuBenManager.FindFuBenIDByMapCode(client.MapCode);
            if (fuBenID <= 0)
            {
                return;
            }

            FuBenInfoItem fuBenInfoItem = FuBenManager.FindFuBenInfoBySeqID(fuBenSeqID);
            if (null == fuBenInfoItem)
            {
                return;
            }

            CopyMap copyMap = GameManager.CopyMapMgr.FindCopyMap(copyMapID);
            if (null == copyMap)
            {
                return;
            }

            // 剧情副本 [7/25/2014 LiaoWei]
            if (Global.IsStoryCopyMapScene(client.MapCode))
            {
                SystemXmlItem systemFuBenItem = null;
                if (GameManager.systemFuBenMgr.SystemXmlItemDict.TryGetValue(copyMap.FubenMapID, out systemFuBenItem) && systemFuBenItem != null)
                {
                    int nBossID = -1;
                    nBossID = systemFuBenItem.GetIntValue("BossID");

                    int nNum = 0;
                    nNum = GameManager.MonsterZoneMgr.GetMapMonsterNum(client.MapCode, nBossID);

                    if (nNum == 0)
                        Global.NotifyClientStoryCopyMapInfo(copyMap.CopyMapID, 1);
                    else
                        Global.NotifyClientStoryCopyMapInfo(copyMap.CopyMapID, 2);
                }
            }

            long startTicks = fuBenInfoItem.StartTicks;
            long endTicks = fuBenInfoItem.EndTicks;
            int killedNormalNum = copyMap.KilledNormalNum;
            int totalNormalNum = copyMap.TotalNormalNum;
            int killedBossNum = copyMap.KilledBossNum;
            int totalBossNum = copyMap.TotalBossNum;

            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                client.RoleID,
                fuBenID,
                startTicks,
                endTicks,
                killedNormalNum,
                totalNormalNum,
                killedBossNum,
                totalBossNum);

            tcpOutPacket = null;
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETFUBENBEGININFO);
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

        /// <summary>
        /// 通知副本地图上的所有人副本信息(同一个副本地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllFuBenBeginInfo(KPlayer client, bool allKilled)
        {
            int fuBenSeqID = FuBenManager.FindFuBenSeqIDByRoleID(client.RoleID);
            if (fuBenSeqID <= 0) //如果副本不存在
            {
                return;
            }

            int copyMapID = client.CopyMapID;
            if (copyMapID <= 0) //如果不是在副本地图中
            {
                return;
            }

            int fuBenID = FuBenManager.FindFuBenIDByMapCode(client.MapCode);
            if (fuBenID <= 0)
            {
                return;
            }

            FuBenInfoItem fuBenInfoItem = FuBenManager.FindFuBenInfoBySeqID(fuBenSeqID);
            if (null == fuBenInfoItem)
            {
                return;
            }

            CopyMap copyMap = GameManager.CopyMapMgr.FindCopyMap(copyMapID);
            if (null == copyMap)
            {
                return;
            }

            long startTicks = fuBenInfoItem.StartTicks;
            long endTicks = fuBenInfoItem.EndTicks;
            int killedNormalNum = copyMap.KilledNormalNum;
            int totalNormalNum = copyMap.TotalNormalNum;
            if (allKilled)
            {
                killedNormalNum = totalNormalNum;
            }

            int killedBossNum = copyMap.KilledBossNum;
            int totalBossNum = copyMap.TotalBossNum;
            if (allKilled)
            {
                killedBossNum = totalBossNum;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                client.RoleID,
                fuBenID,
                startTicks,
                endTicks,
                killedNormalNum,
                totalNormalNum,
                killedBossNum,
                totalBossNum);

            List<Object> objsList = GetMapClients(client.MapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(client.MapCode, client.CopyMapID, objsList);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETFUBENBEGININFO);
        }

        /// <summary>
        /// 通知副本所有子地图上的所有人副本结束或开始信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllMapFuBenBeginInfo(KPlayer client, bool allKilled)
        {
            if (client.FuBenSeqID <= 0 || client.FuBenID <= 0)
                return;

            FuBenInfoItem fuBenInfoItem = FuBenManager.FindFuBenInfoBySeqID(client.FuBenSeqID);
            if (null == fuBenInfoItem)
            {
                return;
            }

            CopyMap copyMap = GameManager.CopyMapMgr.FindCopyMap(client.CopyMapID);
            if (null == copyMap)
            {
                return;
            }

            long startTicks = fuBenInfoItem.StartTicks;
            long endTicks = fuBenInfoItem.EndTicks;
            int killedNormalNum = copyMap.KilledNormalNum;
            int totalNormalNum = copyMap.TotalNormalNum;
            if (allKilled)
            {
                killedNormalNum = totalNormalNum;
            }

            int killedBossNum = copyMap.KilledBossNum;
            int totalBossNum = copyMap.TotalBossNum;
            if (allKilled)
            {
                killedBossNum = totalBossNum;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                client.RoleID,
                client.FuBenID,
                startTicks,
                endTicks,
                killedNormalNum,
                totalNormalNum,
                killedBossNum,
                totalBossNum);

            List<Object> objsList = new List<Object>();

            //根据副本编号获取副本地图编号列表
            List<int> mapCodeList = FuBenManager.FindMapCodeListByFuBenID(copyMap.FubenMapID);
            if (null != mapCodeList)
            {
                //多地图副本需要处理各个地图内所有玩家
                foreach (int mapcode in mapCodeList)
                {
                    int copyMapID = GameManager.CopyMapMgr.FindCopyID(copyMap.FuBenSeqID, mapcode);
                    if (copyMapID >= 0)
                    {
                        CopyMap child_map = GameManager.CopyMapMgr.FindCopyMap(copyMapID);
                        if (null != child_map)
                        {
                            objsList.AddRange(child_map.GetClientsList());
                        }
                    }
                }
            }

            if (0 == objsList.Count)
            {
                return;
            }

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETFUBENBEGININFO);
        }

        /// <summary>
        /// 通知副本地图上的所有人副本通关奖励信息(同一个副本地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllFuBenTongGuanJiangLi(KPlayer client, byte[] bytesData)
        {
            int fuBenSeqID = FuBenManager.FindFuBenSeqIDByRoleID(client.RoleID);
            if (fuBenSeqID <= 0) //如果副本不存在
            {
                return;
            }

            int copyMapID = client.CopyMapID;
            if (copyMapID <= 0) //如果不是在副本地图中
            {
                return;
            }

            int fuBenID = FuBenManager.FindFuBenIDByMapCode(client.MapCode);
            if (fuBenID <= 0)
            {
                return;
            }

            FuBenInfoItem fuBenInfoItem = FuBenManager.FindFuBenInfoBySeqID(fuBenSeqID);
            if (null == fuBenInfoItem)
            {
                return;
            }

            CopyMap copyMap = GameManager.CopyMapMgr.FindCopyMap(copyMapID);
            if (null == copyMap)
            {
                return;
            }

            List<Object> objsList = GetMapClients(client.MapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(client.MapCode, client.CopyMapID, objsList);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_FUBENPASSNOTIFY);
        }

        /// <summary>
        /// 通知副本地图上的所有人怪物数量(同一个副本地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllFuBenMonstersNum(KPlayer client, bool allKilled)
        {
            // 血色城堡副本 不发该消息 [7/8/2014 LiaoWei]
            if (GameManager.BloodCastleCopySceneMgr.IsBloodCastleCopyScene(client.FuBenID))
            {
                return;
            }

            int fuBenSeqID = FuBenManager.FindFuBenSeqIDByRoleID(client.RoleID);
            if (fuBenSeqID <= 0) //如果副本不存在
            {
                return;
            }

            int copyMapID = client.CopyMapID;
            if (copyMapID <= 0) //如果不是在副本地图中
            {
                return;
            }

            CopyMap copyMap = GameManager.CopyMapMgr.FindCopyMap(copyMapID);
            if (null == copyMap)
            {
                return;
            }

            int killedNormalNum = copyMap.KilledNormalNum;
            int totalNormalNum = copyMap.TotalNormalNum;
            if (allKilled)
            {
                killedNormalNum = totalNormalNum;
            }

            int killedBossNum = copyMap.KilledBossNum;
            int totalBossNum = copyMap.TotalBossNum;
            if (allKilled)
            {
                killedBossNum = totalBossNum;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}",
                copyMap.GetGameClientCount(), //client.RoleID, //不发RoleID,改发副本内人数
                killedNormalNum,
                totalNormalNum,
                killedBossNum,
                totalBossNum);

            List<Object> objsList = GetMapClients(client.MapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(client.MapCode, client.CopyMapID, objsList);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_COPYMAPMONSTERSNUM);
        }

        #endregion 副本系统

        #region 每日冲穴次数

        /// <summary>
        /// 将新的每日冲穴次数数据通知自己
        /// </summary>
        /// <param name="client"></param>

        #endregion 每日冲穴次数

        #region 随身仓库

        /// <summary>
        /// 将新的随身仓库数据通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyPortableBagData(KPlayer client)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<PortableBagData>(client.MyPortableBagData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_PORTABLEBAGDATA);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 随身仓库

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

        #region 组队副本通知消息

        /// <summary>
        /// 通知组队副本进入的消息
        /// </summary>
        /// <param name="roleIDsList"></param>
        /// <param name="minLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="mapCode"></param>
        public void NotifyTeamMemberFuBenEnterMsg(KPlayer client, int leaderRoleID, int fuBenID, int fuBenSeqID)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, leaderRoleID, fuBenID, fuBenSeqID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYENTERFUBEN);
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
        /// 通知组队副本进入的消息
        /// </summary>
        /// <param name="roleIDsList"></param>
        /// <param name="minLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="mapCode"></param>
        public void NotifyTeamFuBenEnterMsg(List<int> roleIDsList, int minLevel, int maxLevel, int leaderMapCode, int leaderRoleID, int fuBenID, int fuBenSeqID, int enterNumber, int maxFinishNum, bool igoreNumLimit = false)
        {
            if (null == roleIDsList || roleIDsList.Count <= 0) return;
            for (int i = 0; i < roleIDsList.Count; i++)
            {
                KPlayer otherClient = FindClient(roleIDsList[i]);
                if (null == otherClient) continue; //不在线，则不通知

                //和队长不在同一个地图则不通知
                if (otherClient.MapCode != leaderMapCode)
                {
                    continue;
                }

                //级别不匹配，则不通知
                int unionLevel = Global.GetUnionLevel(otherClient.ChangeLifeCount, otherClient.m_Level);
                if (unionLevel < minLevel || unionLevel > maxLevel)
                {
                    continue;
                }

                if (!igoreNumLimit)
                {
                    FuBenData fuBenData = Global.GetFuBenData(otherClient, fuBenID);
                    int nFinishNum;
                    int haveEnterNum = Global.GetFuBenEnterNum(fuBenData, out nFinishNum);
                    if ((enterNumber >= 0 && haveEnterNum >= enterNumber) || (maxFinishNum >= 0 && nFinishNum >= maxFinishNum))
                    {
                        continue;
                    }
                }

                //通知组队副本进入的消息
                NotifyTeamMemberFuBenEnterMsg(otherClient, leaderRoleID, fuBenID, fuBenSeqID);
            }
        }

        #endregion 组队副本通知消息

        #region 连斩管理

        /// <summary>
        /// 通知连斩值更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeRoleLianZhan(SocketListener sl, TCPOutPacketPool pool, KPlayer client, Monster monster)
        {
            //传奇版本禁止掉连斩

            //怪物等级不得小于人物等级10级以上，否则不计连斩
            //if (monster.MonsterInfo.VLevel <= (client.m_Level - Global.MaxLianZhanSubLevel))
            //{
            //    return;
            //}

            //通知连斩值更新(限制当前地图)
            //ChangeRoleLianZhan2(sl, pool, client, 1, false);
        }

        /// <summary>
        /// 通知连斩值更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeRoleLianZhan2(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int addNum, bool force)
        {
            int oldLianZhanNum = client.TempLianZhan;

            //是否能继续计算连斩
            if (force || (Global.CanContinueLianZhan(client) && oldLianZhanNum < 999))
            {
                //获取旧的连斩的Buffer值
                int oldLianZhanBufferVal = Global.GetLianZhanBufferVal(oldLianZhanNum);

                //累计计数
                client.TempLianZhan = client.TempLianZhan + addNum;

                //记录连斩的最大值
                client.LianZhan = Global.GMax(client.LianZhan, client.TempLianZhan);

                //获取新的连斩的Buffer值
                int newLianZhanBufferVal = Global.GetLianZhanBufferVal(client.TempLianZhan);
                if (oldLianZhanBufferVal != newLianZhanBufferVal && newLianZhanBufferVal > 0)
                {
                    //更新BufferData
                    double[] actionParams = new double[2];
                    actionParams[0] = 30.0;
                    actionParams[1] = (double)newLianZhanBufferVal;
                    Global.UpdateBufferData(client, BufferItemTypes.AntiBoss, actionParams);
                }

                //连斩提示
                Global.BroadcastLianZhanNum(client, oldLianZhanNum, client.TempLianZhan);
            }
            else
            {
                //重新开始计数
                client.TempLianZhan = 1;
            }

            client.StartLianZhanTicks = TimeUtil.NOW();

            //获取连斩的时间间隔
            double secs = Global.GetLianZhanSecs(client.TempLianZhan);
            client.WaitingLianZhanTicks = (int)(secs * 1000);

            //给角色添加Buffer

            //只有一个连斩时不通知
            if (client.TempLianZhan <= 1)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.TempLianZhan, secs);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGLIANZHAN);
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

        #endregion 连斩管理

        #region 更新角色的日常数据

        /// <summary>
        /// 更新角色的日常数据_经验
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

        /// <summary>
        /// 更新角色的日常数据_灵力
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_LingLi(KPlayer client, int newLingLi)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.LingLiDayID)
            {
                client.MyRoleDailyData.TodayLingLi += newLingLi;
            }
            else
            {
                client.MyRoleDailyData.LingLiDayID = dayID;
                client.MyRoleDailyData.TodayLingLi = newLingLi;
            }
        }

        /// <summary>
        /// 更新角色的日常数据_杀BOSS数量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_KillBoss(KPlayer client, int newKillBoss)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.KillBossDayID)
            {
                client.MyRoleDailyData.TodayKillBoss += newKillBoss;
            }
            else
            {
                client.MyRoleDailyData.KillBossDayID = dayID;
                client.MyRoleDailyData.TodayKillBoss = newKillBoss;
            }
        }

        /// <summary>
        /// 更新角色的日常数据_通关副本数量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_FuBenNum(KPlayer client, int newFuBenNum, int nLev, bool bActiveChenJiu = true)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.FuBenDayID)
            {
                client.MyRoleDailyData.TodayFuBenNum += newFuBenNum;
            }
            else
            {
                client.MyRoleDailyData.FuBenDayID = dayID;
                client.MyRoleDailyData.TodayFuBenNum = newFuBenNum;
            }

            DailyActiveManager.ProcessCompleteCopyMapForDailyActive(client, nLev);

            // 副本完成成就 [3/12/2014 LiaoWei]
            if (bActiveChenJiu)
                ChengJiuManager.ProcessCompleteCopyMapForChengJiu(client, nLev);
        }

        /// <summary>
        /// 更新角色的日常数据_五行奇阵领取奖励数量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_WuXingNum(KPlayer client, int newWuXingNum)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.WuXingDayID)
            {
                client.MyRoleDailyData.WuXingNum += newWuXingNum;
            }
            else
            {
                client.MyRoleDailyData.WuXingDayID = dayID;
                client.MyRoleDailyData.WuXingNum = newWuXingNum;
            }
        }

        /// <summary>
        /// 更新角色的日常数据_扫荡次数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newExperience"></param>
        public void UpdateRoleDailyData_SweepNum(KPlayer client, int newWuXingNum)
        {
            if (null == client.MyRoleDailyData)
            {
                client.MyRoleDailyData = new RoleDailyData();
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.MyRoleDailyData.WuXingDayID)
            {
                client.MyRoleDailyData.WuXingNum += newWuXingNum;
            }
            else
            {
                client.MyRoleDailyData.WuXingDayID = dayID;
                client.MyRoleDailyData.WuXingNum = newWuXingNum;
            }
        }

        /// <summary>
        /// 将新角色每日数据通知客户端
        /// </summary>
        /// <param name="client"></param>
        public void NotifyRoleDailyData(KPlayer client)
        {
            RoleDailyData roleDailyData = client.MyRoleDailyData;
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDailyData>(roleDailyData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETROLEDAILYDATA);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 更新角色的日常数据

        #region 杀BOSS数量更新

        /// <summary>
        /// 更新杀BOSS的数量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="killBossNum"></param>
        public void UpdateKillBoss(KPlayer client, int killBossNum, Monster monster, bool writeToDB = false)
        {
            //如果不是BOSS，则不处理
            if ((int)MonsterTypes.Boss != monster.MonsterType)
            {
                return;
            }

            int[] ids = GameManager.systemParamsList.GetParamValueIntArrayByName("NotTuMo");
            if (null != ids && ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (monster.MonsterInfo.ExtensionID == ids[i])
                    {
                        return;
                    }
                }
            }

            client.KillBoss += killBossNum;

            //更新每日的杀BOSS的数据
            UpdateRoleDailyData_KillBoss(client, killBossNum);

            if (writeToDB)
            {
                //更新PKValue
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEKILLBOSS,
                    string.Format("{0}:{1}", client.RoleID, client.KillBoss),
                    null, client.ServerId);

                long nowTicks = TimeUtil.NOW();
                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEKILLBOSS, nowTicks);
            }
        }

        #endregion 杀BOSS数量更新

        #region 英雄逐擂的到达层数更新

        /// <summary>
        /// 通知英雄逐擂到达层数更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeRoleHeroIndex(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int heroIndex, bool force = false)
        {
        }

        #endregion 英雄逐擂的到达层数更新

        #region BOSS刷新数据

        /// <summary>
        /// 将BOSS刷新数据通知客户端
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBossInfoDictData(KPlayer client)
        {
            //BOSS刷新数据字典
            Dictionary<int, BossData> dict = MonsterBossManager.GetBossDictData();
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Dictionary<int, BossData>>(dict, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETBOSSINFODICT);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion BOSS刷新数据

        #region 镖车相关

        /// <summary>
        /// 将押镖数据通知客户端
        /// </summary>
        /// <param name="client"></param>
        public void NotifyYaBiaoData(KPlayer client)
        {
        }

        /// <summary>
        /// 镖车血变化(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOtherBiaoCheLifeV(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int biaoCheID, int currentLifeV)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", biaoCheID, currentLifeV);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGBIAOCHELIFEV);
        }

        /// <summary>
        /// 镖车通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersNewBiaoChe(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, BiaoCheItem biaoCheItem)
        //{
        //    if (null == objsList) return;

        //    //镖车项到镖车数据的转换
        //    BiaoCheData biaoCheData = Global.BiaoCheItem2BiaoCheData(biaoCheItem);

        //    byte[] bytesData = DataHelper.ObjectToBytes<BiaoCheData>(biaoCheData);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_NEWBIAOCHE);
        //}

        /// <summary>
        /// 镖车通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewBiaoChe(SocketListener sl, TCPOutPacketPool pool, KPlayer client, BiaoCheItem biaoCheItem)
        {
            //镖车项到镖车数据的转换
            BiaoCheData biaoCheData = Global.BiaoCheItem2BiaoCheData(biaoCheItem);

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<BiaoCheData>(biaoCheData, pool, (int)TCPGameServerCmds.CMD_SPR_NEWBIAOCHE);
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
        /// 镖车消失通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersDelBiaoChe(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int biaoCheID)
        //{
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}", biaoCheID);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELBIAOCHE);
        //}

        /// <summary>
        /// 镖车消失通知自己(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelBiaoChe(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int biaoCheID)
        {
            string strcmd = string.Format("{0}", biaoCheID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELBIAOCHE);
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

        #endregion 镖车相关

        #region 弹窗管理

        /// <summary>
        /// 通知在线的所有人(不限制地图)弹窗消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllPopupWinMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strcmd)
        {
            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (null != client && gc == client)
                {
                    continue;
                }

                if (gc.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //通知在线的对方(不限制地图)公告消息
                NotifyPopupWinMsg(sl, pool, gc, strcmd);
            }
        }

        /// <summary>
        /// 通知在线的对方(不限制地图)弹窗消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyPopupWinMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strcmd)
        {
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOFITYPOPUPWIN);
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

        #endregion 弹窗管理

        #region 登录次数自动增加

        /// <summary>
        /// 重新计算按照日来判断的登录次数
        /// </summary>
        /// <param name="client"></param>
        private void ChangeDayLoginNum(KPlayer client)
        {
            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.LoginDayID)
            {
                return;
            }

            client.LoginDayID = dayID;

            /// 当角色在节日期间登录游戏的时候--->每天只会被调用一次
            HuodongCachingMgr.OnJieriRoleLogin(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            //更新成就相关登录次数--->一定要在UpdateWeekLoginNum 前面调用，保证 MyHuodongData中LastDayID未被更改
            ChengJiuManager.OnRoleLogin(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            //更新前七天的每天在线累计时长--->一定要在UpdateWeekLoginNum 前面调用，保证 MyHuodongData中LastDayID未被更改
            HuodongCachingMgr.ProcessDayOnlineSecs(client, Global.SafeConvertToInt32(client.MyHuodongData.LastDayID));

            //更新周连续登录的次数
            bool notifyHuodDongData = Global.UpdateWeekLoginNum(client);

            //更新限时累计登录次数
            notifyHuodDongData |= Global.UpdateLimitTimeLoginNum(client);
            if (notifyHuodDongData) //是否通知客户端
            {
                GameManager.ClientMgr.NotifyHuodongData(client);
            }

            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //跨天的时候
            Global.GiveGuMuTimeLimitAward(client);

            //在线跨天的时候处理每日任务
            Global.InitRoleDailyTaskData(client, true);

            //跨天时清除采集数据，记录资源找回数据，必须在InitRoleOldResourceInfo之前调用
            CaiJiLogic.InitRoleDailyCaiJiData(client, false, true);

            HuanYingSiYuanManager.getInstance().InitRoleDailyHYSYData(client);

            Global.ProcessUpdateFuBenData(client);

            //初始化资源找回数据
            CGetOldResourceManager.InitRoleOldResourceInfo(client, true);

            // 更新合服登陆活动
            Global.UpdateHeFuLoginFlag(client);

            // 更新合服累计登陆活动的计数
            Global.UpdateHeFuTotalLoginFlag(client);

            // 向客户端推送图标变化
            //if (client._IconStateMgr.CheckHeFuLogin(client)
            //    || client._IconStateMgr.CheckHeFuTotalLogin(client)
            //    || client._IconStateMgr.CheckHeFuPKKing(client))
            if (client._IconStateMgr.CheckHeFuActivity(client)
                || client._IconStateMgr.CheckSpecialActivity(client))
                client._IconStateMgr.SendIconStateToClient(client);

            // 更新在线用户的月卡信息
            YueKaManager.UpdateNewDay(client);

            //玩家召回
            UserReturnManager.getInstance().initUserReturnData(client);

            FundManager.initFundData(client);

            // 跨天更新角色登陆记录
            Global.UpdateRoleLoginRecord(client);

            JieriGiveActivity giveAct = HuodongCachingMgr.GetJieriGiveActivity();
            if (giveAct != null)
            {
                giveAct.UpdateNewDay(client);
            }

            JieriRecvActivity recvAct = HuodongCachingMgr.GetJieriRecvActivity();
            if (recvAct != null)
            {
                recvAct.UpdateNewDay(client);
            }

            // 对于跨天的同步处理
            client.sendCmd((int)TCPGameServerCmds.CMD_SYNC_CHANGE_DAY_SERVER, string.Format("{0}", TimeUtil.NOW() * 10000));

            // 七日活动
            SevenDayActivityMgr.Instance().OnNewDay(client);

            ZhengBaManager.Instance().OnNewDay(client);
        }

        /*/// <summary>
        //  新增加一个接口 处理连续登陆[1/19/2014 LiaoWei]
        /// </summary>
        /// <param name="client"></param>
        private void ProcessSeriesLogin(KPlayer client)
        {
            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == client.RoleLoginDayID)
                return;

            client.MyHuodongData.EveryDayOnLineAwardStep = 0;    // 设置下每日在线奖励的领取到了第几步

            client.MyHuodongData.SeriesLoginGetAwardStep = 0;    // 设置下连续登陆奖励的领取到了第几步

            client.DayOnlineSecond = 0;      // 每日在线时长重置

            Global.UpdateSeriesLoginInfo(client);

            //client.RoleLoginDayID = dayID;

            Global.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);
        }*/

        #endregion 登录次数自动增加

        #region 加成属性索引管理

        /// <summary>
        /// 通知全套加成属性值更新(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeAllThingAddPropIndexs(KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, client.AllQualityIndex, client.AllForgeLevelIndex, client.AllJewelLevelIndex, client.AllZhuoYueNum);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_UPDATEALLTHINGINDEXS);
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

        #endregion 加成属性索引管理

        #region 银两折半优惠通知

        /// <summary>
        /// 通知所有在线用户银两折半优惠发生了改变
        /// </summary>
        /// <param name="bigAwardID"></param>
        /// <param name="songLiID"></param>
        public void NotifyAllChangeHalfYinLiangPeriod(int halfYinLiangPeriod)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                string strcmd = string.Format("{0}:{1}", client.RoleID, halfYinLiangPeriod);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGHALFYINLIANGPERIOD);
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

        #region 帮派管理

        /// <summary>
        /// 通知帮派信息(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeBangHuiName(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, client.GuildID, client.GuildName, client.GuildRank);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGBANGHUIINFO);
        }

        /// <summary>
        /// 通知帮会职务变更(限制当前地图)
        /// </summary>
        /// <param name="client"></param>
        public void ChangeBangHuiZhiWu(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.GuildID, client.GuildRank);

            // 公告委任通知 [1/16/2014 LiaoWei]
            if (client.GuildRank > 0)
            {
                string sBusiness = "";

                if (client.GuildRank == 1)
                    sBusiness = Global.GetLang("首领");
                else if (client.GuildRank == 2)
                    sBusiness = Global.GetLang("副首领");
                else if (client.GuildRank == 3)
                    sBusiness = Global.GetLang("左将军");
                else if (client.GuildRank == 4)
                    sBusiness = Global.GetLang("右将军");

                //string sMsg = client.RoleName + "被委任为" + sBusiness;

                //GameManager.ClientMgr.NotifyFactionChatMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.GuildID, sMsg);

                Global.BroadcastBangHuiMsg(client.RoleID, client.GuildID,
                    StringUtil.substitute(Global.GetLang("【{0}】被委任为『{1}』"), client.RoleName, sBusiness),
                    true, GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyChatBox);
            }

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYBHZHIWU);
        }

        /// <summary>
        /// 通知所有在线的帮会管理用户，某个用户申请了加入帮派
        /// </summary>
        /// <param name="bigAwardID"></param>
        /// <param name="songLiID"></param>
        public void NotifyOnlineBangHuiMgrRoleApplyMsg(int roleID, string roleName, int bhid, string bhName, string roleList)
        {
            if (string.IsNullOrEmpty(roleList))
            {
                return;
            }

            string[] fields = roleList.Split(',');
            if (null == fields || fields.Length <= 0) return;

            // 增加申请者的职业和等级信息 [12/31/2013 LiaoWei]
            KPlayer clientApply = null;
            clientApply = GameManager.ClientMgr.FindClient(roleID);

            if (clientApply == null)
                return;

            KPlayer client = null;
            for (int i = 0; i < fields.Length; i++)
            {
                int bhMgrRoleID = Global.SafeConvertToInt32(fields[i]);
                if (bhMgrRoleID <= 0) continue;

                client = GameManager.ClientMgr.FindClient(bhMgrRoleID);
                if (null == client)
                {
                    continue;
                }

                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", roleID, roleName, clientApply.m_cPlayerFaction.GetFactionId(), clientApply.m_Level, clientApply.ChangeLifeCount, bhid, bhName);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_APPLYTOBHMEMBER);
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

        /// <summary>
        /// 加入帮派邀请通知通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyInviteToBangHui(SocketListener sl, TCPOutPacketPool pool, KPlayer otherClient, int inviteRoleID, string inviteRoleName, int bhid, string bhName, int nChangelifeLev)
        {
            //如果等级不到，则不发送邀请通知
            if (Global.GetUnionLevel(otherClient) < Global.JoinBangHuiNeedLevel)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}:{2}:{3}", inviteRoleID, inviteRoleName, bhid, bhName, nChangelifeLev);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_INVITETOBANGHUI);
            if (!sl.SendData(otherClient.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    otherClient.RoleID,
                    otherClient.RoleName));*/
            }
        }

        /// <summary>
        /// 通知某个角色被加入了某个帮派
        /// </summary>
        /// <param name="client"></param>
        public void NotifyJoinBangHui(SocketListener sl, TCPOutPacketPool pool, KPlayer otherClient, int bhid, string bhName)
        {
            //防止重复设置帮会
            if (otherClient.GuildID > 0)
            {
                return;
            }

            //修改加入帮派的角色的信息
            otherClient.GuildID = bhid;
            otherClient.GuildName = bhName;
            otherClient.GuildRank = 0;

            //通知附近用户，某用户的帮派信息进行了修改
            //通知帮派信息(限制当前地图)
            GameManager.ClientMgr.ChangeBangHuiName(sl, pool, otherClient);
            GlobalEventSource4Scene.getInstance().fireEvent(new PostBangHuiChangeEventObject(otherClient, bhid), (int)SceneUIClasses.All);
            Global.SaveRoleParamsInt32ValueToDB(otherClient, RoleParamName.EnterBangHuiUnixSecs, DataHelper.UnixSecondsNow(), true);

            int junQiLevel = JunQiManager.GetJunQiLevelByBHID(otherClient.GuildID);

            //更新BufferData
            double[] actionParams = new double[1];
            actionParams[0] = (double)junQiLevel - 1;
            Global.UpdateBufferData(otherClient, BufferItemTypes.JunQi, actionParams, 1);

            //通知本帮派的所有在线的人，某人加入了本帮派
            Global.BroadcastBangHuiMsg(otherClient.RoleID, bhid,
                StringUtil.substitute(Global.GetLang("『{0}』加入了『{1}』战盟"), otherClient.RoleName, otherClient.GuildName),
                true, GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyChatBox);

            //成就处理第一次加入帮会
            ChengJiuManager.OnFirstInFaction(otherClient);
            UnionPalaceManager.initSetUnionPalaceProps(otherClient, true);

            otherClient._IconStateMgr.CheckGuildIcon(otherClient, false);
        }

        /// <summary>
        /// 通知某个角色离开了某个帮派
        /// </summary>
        /// <param name="client"></param>
        public void NotifyLeaveBangHui(SocketListener sl, TCPOutPacketPool pool, KPlayer otherClient, int bhid, string bhName, int leaveType)
        {
            //防止重复设置
            if (otherClient.GuildID <= 0)
            {
                return;
            }

            //通知本帮派的所有在线的人，某人加入了本帮派
            Global.BroadcastBangHuiMsg(otherClient.RoleID, bhid,
                StringUtil.substitute(Global.GetLang("『{0}』{1}『{2}』战盟"), otherClient.RoleName, leaveType <= 0 ? Global.GetLang("被开除出了") : Global.GetLang("脱离了"), bhName),
                true, GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyChatBox);

            //修改加入帮派的角色的信息
            otherClient.GuildID = 0;
            otherClient.GuildName = "";
            otherClient.GuildRank = 0;
            //otherClient.BangGong = 0; // 离开帮会 帮贡不清空 [5/11/2014 LiaoWei]

            //通知附近用户，某用户的帮派信息进行了修改
            //通知帮派信息(限制当前地图)
            GameManager.ClientMgr.ChangeBangHuiName(sl, pool, otherClient);
            GlobalEventSource4Scene.getInstance().fireEvent(new PostBangHuiChangeEventObject(otherClient, bhid), (int)SceneUIClasses.All);

            //帮贡变化通知(只通知自己)
            //GameManager.ClientMgr.NotifySelfBangGongChange(sl, pool, otherClient);

            //从buffer数据到列表删除指定的临时Buffer
            KTLogic.RemoveBufferData(otherClient, (int)BufferItemTypes.JunQi);

            UnionPalaceManager.initSetUnionPalaceProps(otherClient, true);

            //通知用户数值发生了变化
            GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, otherClient);

            // 总生命值和魔法值变化通知(同一个地图才需要通知)
            GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, otherClient);
        }

        /// <summary>
        /// 通知所有指定帮会的在线用户帮会已经解散
        /// </summary>
        /// <param name="bigAwardID"></param>
        /// <param name="songLiID"></param>
        public void NotifyBangHuiDestroy(int retCode, int roleID, int bhid)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.GuildID != bhid)
                {
                    continue;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //修改加入帮派的角色的信息
                client.GuildID = 0;
                client.GuildName = "";
                client.GuildRank = 0;
                // 帮贡不清
                //client.BangGong = 0;

                string strcmd = string.Format("{0}:{1}:{2}", retCode, roleID, bhid);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DESTROYBANGHUI);
                if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                    //
                    /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                        tcpOutPacket.PacketCmdID,
                        tcpOutPacket.PacketDataSize,
                        client.RoleID,
                        client.RoleName));*/
                }

                //从buffer数据到列表删除指定的临时Buffer
                KTLogic.RemoveBufferData(client, (int)BufferItemTypes.JunQi);

                UnionPalaceManager.initSetUnionPalaceProps(client, true);

                client.AllyList = null;

                //通知用户数值发生了变化
                GameManager.ClientMgr.NotifyUpdateEquipProps(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);

                // 总生命值和魔法值变化通知(同一个地图才需要通知)
                GameManager.ClientMgr.NotifyOthersLifeChanged(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);
            }
        }

        public void NotifyBangHuiUpLevel(int bhid, int serverID, int level, bool isKF)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.GuildID != bhid || client.ClientSocket.IsKuaFuLogin)
                    continue;

                UnionPalaceManager.initSetUnionPalaceProps(client, true);
            }

            if (AllyManager.getInstance().IsAllyOpen(level))
                AllyManager.getInstance().UnionDataChange(bhid, serverID);
        }

        public void NotifyBangHuiChangeName(int bhid, string newName)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.GuildID != bhid)
                {
                    continue;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //修改加入帮派的角色的信息
                client.GuildName = newName;

                string strcmd = string.Format("{0}:{1}", bhid, newName);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_NTF_BANGHUI_CHANGE_NAME);
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

        /// <summary>
        /// 拒绝申请加入帮派的操作
        /// </summary>
        /// <param name="client"></param>
        public void NotifyRefuseApplyToBHMember(KPlayer otherClient, string bhRoleName, string bhName)
        {
            if (otherClient.GuildID > 0) //已经加入帮派
            {
                return;
            }

            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, otherClient,
                StringUtil.substitute(Global.GetLang("『{0}』拒绝了你加入『{1}』战盟的申请"), bhRoleName, bhName),
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
        }

        /// <summary>
        /// 拒绝邀请加入帮派的操作
        /// </summary>
        /// <param name="client"></param>
        public void NotifyRefuseInviteToBHMember(KPlayer otherClient, string bhRoleName, string bhName)
        {
            if (otherClient.GuildID <= 0) //已经无帮派
            {
                return;
            }

            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, otherClient,
                StringUtil.substitute(Global.GetLang("『{0}』拒绝了你加入『{1}』战盟的邀请"), bhRoleName, bhName),
                GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
        }

        #endregion 帮派管理

        #region 帮贡处理

        /// <summary>
        /// 帮贡变化通知(只通知自己)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfBangGongChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, client.BangGong, client.BGMoney);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_BANGGONGCHANGE);
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
        /// 添加用户帮贡
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddBangGong(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, ref int addBangGong, AddBangGongTypes addBangGongType, int nBangGongLimit = 0)
        {
            // ADD CÔNG HIẾN BANG
            return true;
        }

        public bool AddBangGong(KPlayer client, ref int addBangGong, AddBangGongTypes addBangGongType, int nBangGongLimit = 0)
        {
            return AddBangGong(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, ref addBangGong, addBangGongType, nBangGongLimit);
        }

        /// <summary>
        /// 扣除用户帮贡
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubUserBangGong(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subBangGong)
        {
            int oldBangGong = client.BangGong;

            if (client.BangGong < subBangGong)
            {
                return false; //帮贡余额不足
            }
            // Trừ tiền trong bang hội

            return true;
        }

        #endregion 帮贡处理

        #region 帮会库存铜钱处理

        /// <summary>
        /// 添加帮会库存铜钱
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddBangHuiTongQian(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int bhid, int addMoney)
        {
            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}:{2}",
                client.RoleID,
                bhid,
                addMoney);

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_ADDBHTONGQIAN_CMD, strcmd, client.ServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 2)
            {
                return false;
            }

            // 先锁定
            if (Convert.ToInt32(dbFields[0]) < 0)
            {
                return false;
            }

            GameManager.ClientMgr.NotifyBangHuiZiJinChanged(client, bhid);

            return true;
        }

        /// <summary>
        /// 扣除帮会库存铜钱
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool SubBangHuiTongQian(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int subMoney, out int bhZoneID)
        {
            bhZoneID = 0;
            if (client.GuildID <= 0)
            {
                return false;
            }

            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}:{2}",
                client.RoleID,
                client.GuildID,
                subMoney);

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBHTONGQIAN_CMD, strcmd, client.ServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 2)
            {
                return false;
            }

            // 先锁定
            if (Convert.ToInt32(dbFields[0]) < 0)
            {
                return false; //帮会库存扣除失败，余额不足
            }

            bhZoneID = Global.SafeConvertToInt32(dbFields[1]);
            GameManager.ClientMgr.NotifyBangHuiZiJinChanged(client, client.GuildID);
            return true;
        }

        /// <summary>
        /// 通知帮会自己变化
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bhid"></param>
        public void NotifyBangHuiZiJinChanged(KPlayer client, int bhid)
        {
            int roleID = client.RoleID;

            BangHuiDetailData bangHuiDetailData = Global.GetBangHuiDetailData(roleID, bhid);
            if (null != bangHuiDetailData)
            {
                KPlayer clientBZ = client;
                if (roleID != bangHuiDetailData.BZRoleID)
                {
                    clientBZ = GameManager.ClientMgr.FindClient(bangHuiDetailData.BZRoleID);
                    if (null != clientBZ)
                    {
                        clientBZ.sendCmd((int)TCPGameServerCmds.CMD_SPR_SERVERUPDATE_ZHANMENGZIJIN, string.Format("{0}:{1}", bhid, bangHuiDetailData.TotalMoney));
                    }
                }

                if (client.GuildID == bhid)
                {
                    client.sendCmd((int)TCPGameServerCmds.CMD_SPR_SERVERUPDATE_ZHANMENGZIJIN, string.Format("{0}:{1}", bhid, bangHuiDetailData.TotalMoney));
                }
            }
        }

        #endregion 帮会库存铜钱处理

        #region 插帮旗相关

        /// <summary>
        /// 帮旗血变化(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOtherJunQiLifeV(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int junQiID, int currentLifeV)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", junQiID, currentLifeV);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGJUNQILIFEV);
        }

        /// <summary>
        /// 帮旗通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersNewJunQi(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, JunQiItem junQiItem)
        //{
        //    if (null == objsList) return;

        //    //帮旗项到帮旗数据的转换
        //    JunQiData junQiData = Global.JunQiItem2JunQiData(junQiItem);

        //    byte[] bytesData = DataHelper.ObjectToBytes<JunQiData>(junQiData);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_NEWJUNQI);
        //}

        /// <summary>
        /// 帮旗通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewJunQi(SocketListener sl, TCPOutPacketPool pool, KPlayer client, JunQiItem junQiItem)
        {
            //帮旗项到帮旗数据的转换
            JunQiData junQiData = Global.JunQiItem2JunQiData(junQiItem);

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<JunQiData>(junQiData, pool, (int)TCPGameServerCmds.CMD_SPR_NEWJUNQI);
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
        /// 帮旗消失通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        //public void NotifyOthersDelJunQi(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int junQiID)
        //{
        //    if (null == objsList) return;

        //    string strcmd = string.Format("{0}", junQiID);

        //    //群发消息
        //    SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELJUNQI);
        //}

        /// <summary>
        /// 镖车消失通知自己(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelJunQi(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int junQiID)
        {
            string strcmd = string.Format("{0}", junQiID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELJUNQI);
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
        /// 领地帮会和税收变更消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyLingDiForBHMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strcmd)
        {
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LINGDIFORBH);
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
        /// 通知在线的所有人(不限制地图)领地帮会和税收变更消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllLingDiForBHMsg(SocketListener sl, TCPOutPacketPool pool, int lingDiID, int bhid, int zoneID, string bhName, int tax)
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", lingDiID, bhid, zoneID, bhName, tax);

            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (gc.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //领地帮会和税收变更消息
                NotifyLingDiForBHMsg(sl, pool, gc, strcmd);
            }
        }

        /// <summary>
        /// 广播单个帮会领地信息
        /// </summary>
        /// <param name="bangHuiLingDiItemData"></param>
        public void NotifyAllLuoLanChengZhanRequestInfoList(List<LuoLanChengZhanRequestInfoEx> list)
        {
            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (gc.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                NotifyLuoLanChengZhanRequestInfoList(gc, list);
            }
        }

        /// <summary>
        /// 向玩家发送单个领地信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bangHuiLingDiItemData"></param>
        public void NotifyLuoLanChengZhanRequestInfoList(KPlayer client, List<LuoLanChengZhanRequestInfoEx> list)
        {
            client.sendCmd((int)TCPGameServerCmds.CMD_SPR_GET_LUOLANCHENGZHAN_REQUEST_INFO_LIST, list);
        }

        /// <summary>
        /// 通知在线的指定帮会的所有人帮旗升级了，更新buffer
        /// </summary>
        /// <param name="client"></param>
        public void HandleBHJunQiUpLevel(int bhid, int junQiLevel)
        {
            //更新BufferData
            double[] actionParams = new double[1];
            actionParams[0] = (double)junQiLevel - 1;

            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (gc.GuildID != bhid)
                {
                    continue;
                }

                Global.UpdateBufferData(gc, BufferItemTypes.JunQi, actionParams, 1);
            }
        }

        #endregion 插帮旗相关

        #region 处理假人相关消息

        /// <summary>
        /// 假人血变化(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOtherFakeRoleLifeV(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int FakeRoleID, int currentLifeV)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", FakeRoleID, currentLifeV);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGFAKEROLELIFEV);
        }

        /// <summary>
        /// 假人通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewFakeRole(SocketListener sl, TCPOutPacketPool pool, KPlayer client, FakeRoleItem FakeRoleItem)
        {
            //假人项到假人数据的转换
            FakeRoleData FakeRoleData = Global.FakeRoleItem2FakeRoleData(FakeRoleItem);

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<FakeRoleData>(FakeRoleData, pool, (int)TCPGameServerCmds.CMD_SPR_NEWFAKEROLE);
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
        /// 假人消失通知自己(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelFakeRole(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int FakeRoleID)
        {
            string strcmd = string.Format("{0}", FakeRoleID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELFAKEROLE);
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
        /// 假人血变化(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllDelFakeRole(SocketListener sl, TCPOutPacketPool pool, FakeRoleItem fakeRoleItem)
        {
            List<Object> objsList = Global.GetAll9Clients(fakeRoleItem);

            string strcmd = string.Format("{0}", fakeRoleItem.FakeRoleID);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELFAKEROLE);
        }

        #endregion 处理假人相关消息

        #region 皇城战相关

        /// <summary>
        /// 皇帝角色ID变更消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyChgHuangDiRoleIDMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string strcmd)
        {
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGHUANGDIROLEID);
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
        /// 通知在线的所有人(不限制地图)皇帝角色ID变更消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllChgHuangDiRoleIDMsg(SocketListener sl, TCPOutPacketPool pool, int oldHuangDiRoleID, int huangDiRoleID)
        {
            string strcmd = string.Format("{0}:{1}", oldHuangDiRoleID, huangDiRoleID);

            int index = 0;
            KPlayer gc = null;
            while ((gc = GetNextClient(ref index)) != null)
            {
                if (gc.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }

                //皇帝角色ID变更消息
                NotifyChgHuangDiRoleIDMsg(sl, pool, gc, strcmd);
            }
        }

        #endregion 皇城战相关

        #region 册封和废黜皇后

        /// <summary>
        /// 通知选为皇妃的命令
        /// </summary>
        /// <param name="client"></param>
        public void NotifyInviteAddHuangFei(KPlayer client, int otherRoleID, string otherRoleName, int randNum)
        {
            string strcmd = string.Format("{0}:{1}:{2}", otherRoleID, otherRoleName, randNum);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_INVITEADDHUANGFEI);
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

        #endregion 册封和废黜皇后

        #region 领地地图信息数据

        /// <summary>
        /// 领地信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyLingDiMapInfoData(KPlayer client, LingDiMapInfoData lingDiMapInfoData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<LingDiMapInfoData>(lingDiMapInfoData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETLINGDIMAPINFO);
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

        /// <summary>
        /// 通知在线的所有人(不限制地图)领地信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllLingDiMapInfoData(int mapCode, LingDiMapInfoData lingDiMapInfoData)
        {
            List<Object> objsList = GetMapClients(mapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(mapCode, -1, objsList);

            byte[] bytesData = DataHelper.ObjectToBytes<LingDiMapInfoData>(lingDiMapInfoData);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_GETLINGDIMAPINFO);
        }

        #endregion 领地地图信息数据

        #region 皇城地图信息数据

        /// <summary>
        /// 皇城信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyHuangChengMapInfoData(KPlayer client, HuangChengMapInfoData huangChengMapInfoData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<HuangChengMapInfoData>(huangChengMapInfoData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETHUANGCHENGMAPINFO);
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

        /// <summary>
        /// 通知在线的所有人(不限制地图)皇城信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllHuangChengMapInfoData(int mapCode, HuangChengMapInfoData huangChengMapInfoData)
        {
            List<Object> objsList = GetMapClients(mapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(mapCode, -1, objsList);

            byte[] bytesData = DataHelper.ObjectToBytes<HuangChengMapInfoData>(huangChengMapInfoData);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_GETHUANGCHENGMAPINFO);
        }

        #endregion 皇城地图信息数据

        #region 王城地图信息数据

        /// <summary>
        /// 皇城信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyWangChengMapInfoData(KPlayer client, WangChengMapInfoData wangChengMapInfoData)
        {
            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<WangChengMapInfoData>(wangChengMapInfoData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_GETWANGCHENGMAPINFO);
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

        /// <summary>
        /// 通知在线的所有人(不限制地图)王城信息数据通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllWangChengMapInfoData(WangChengMapInfoData wangChengMapInfoData)
        {
            /*List<Object> objsList = GetMapClients(mapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(mapCode, -1, objsList);

            byte[] bytesData = DataHelper.ObjectToBytes<WangChengMapInfoData>(wangChengMapInfoData);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, bytesData, (int)TCPGameServerCmds.CMD_SPR_GETWANGCHENGMAPINFO);*/

            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                NotifyWangChengMapInfoData(client, wangChengMapInfoData);
            }
        }

        #endregion 王城地图信息数据

        #region 领地税收

        /// <summary>
        /// 添加帮会领地税收
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddLingDiTaxMoney(int bhid, int lingDiID, int addMoney)
        {
            //先DBServer请求扣费
            string strcmd = string.Format("{0}:{1}:{2}",
                bhid,
                lingDiID,
                addMoney);

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_SPR_ADDLINGDITAXMONEY, strcmd, GameManager.LocalServerId);
            if (null == dbFields) return false;
            if (dbFields.Length != 4)
            {
                return false;
            }

            // 先锁定
            if (Convert.ToInt32(dbFields[0]) < 0)
            {
                return false;
            }

            return true;
        }

        #endregion 领地税收

        #region 隋唐争霸赛（角斗场）最后经验奖励提示

        /// <summary>
        /// 隋唐争霸赛（角斗场）最后经验奖励提示信息 (只通知自己)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfSuiTangBattleAward(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int nPoint1, int nPoint2, long experience, int bindYuanBao, int chengJiu, bool bIsSuccess, int paiMing, string awardsGoods)
        {
            int nSelfPoint = client.BattleKilledNum;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}", client.RoleID, bIsSuccess, nPoint1, nPoint2, nSelfPoint, experience, chengJiu, bindYuanBao, paiMing, awardsGoods);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYBATTLEAWARD);
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

        #endregion 隋唐争霸赛（角斗场）最后经验奖励提示

        #region 邮件相关

        /// <summary>
        /// 通知新邮件
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool NotifyLastUserMail(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, int mailID)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, mailID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_RECEIVELASTMAIL);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
                //
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }

            return true;
        }

        /// <summary>
        /// 一键完成任务时 背包满了--发邮件 [6/24/2014 LiaoWei]
        /// </summary>
        public void SendMailWhenPacketFull(KPlayer client, List<GoodsData> awardsItemList, string sContent, string sSubject)
        {
            int nTotalGroup = 0;
            nTotalGroup = awardsItemList.Count / 5;

            int nRemain = 0;
            nRemain = awardsItemList.Count % 5;

            int nCount = 0;
            if (nTotalGroup > 0)
            {
                for (int i = 0; i < nTotalGroup; ++i)
                {
                    List<GoodsData> goods = new List<GoodsData>();

                    for (int n = 0; n < 5; ++n)
                    {
                        goods.Add(awardsItemList[nCount]);
                        ++nCount;
                    }

                    Global.UseMailGivePlayerAward2(client, goods, sContent, sSubject);
                }
            }

            if (nRemain > 0)
            {
                List<GoodsData> goods1 = new List<GoodsData>();
                for (int i = 0; i < nRemain; ++i)
                {
                    goods1.Add(awardsItemList[nCount]);
                    ++nCount;
                }

                Global.UseMailGivePlayerAward2(client, goods1, sContent, sSubject);
            }
        }

        #endregion 邮件相关

        #region 生肖运程竞猜相关

        /// <summary>
        /// 通知在线的所有人(限制生肖地图)生肖运程竞猜状态信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllShengXiaoGuessStateMsg(SocketListener sl, TCPOutPacketPool pool, int shengXiaoGuessState, int extraParams, int minLevel, int preGuessResult)
        {
            string strcmd = string.Format("{0}:{1}:{2}", shengXiaoGuessState, extraParams, preGuessResult);

            //先找出当前生肖地图中的所有的或者的人
            List<Object> objsList = GameManager.ClientMgr.GetMapClients(GameManager.ShengXiaoGuessMgr.GuessMapCode);
            if (null == objsList) return;
            TCPOutPacket tcpOutPacket = null;
            try
            {
                for (int i = 0; i < objsList.Count; i++)
                {
                    KPlayer client = objsList[i] as KPlayer;
                    if (client == null) continue;

                    if (client.m_Level < minLevel) //最低级别要求
                    {
                        continue;
                    }

                    if (null == tcpOutPacket) tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYSHENGXIAOGUESSSTAT);
                    if (!sl.SendData(client.ClientSocket, tcpOutPacket, false))
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
            finally
            {
                PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 通知玩家生肖运程竞猜结果
        /// </summary>
        /// <param name="client"></param>
        public void NotifyShengXiaoGuessResultMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, string sResult)
        {
            if (null == client)
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}", client.RoleID, sResult);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYSHENGXIAOGUESSRESULT);
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
        /// 通知某个刚刚上线的角色生肖运程竞猜状态信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyClientShengXiaoGuessStateMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int shengXiaoGuessState, int extraParams, int minLevel, int preGuessResult)
        {
            if (null == client || client.m_Level < minLevel) //最低级别要求
            {
                return;
            }

            string strcmd = string.Format("{0}:{1}:{2}", shengXiaoGuessState, extraParams, preGuessResult);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYSHENGXIAOGUESSSTAT);
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

        #endregion 生肖运程竞猜相关

        #region NPC 相关

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

        #endregion NPC 相关

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

        #region 角色故事版移动处理

        //Unix秒的起始计算毫秒时间(相对系统时间)
        public const long Before1970Ticks = 62135625600000;

        /// <summary>
        /// 尝试模仿怪物直接移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="startMoveTicks"></param>
        private bool TryDirectMove(KPlayer client, long startMoveTicks, List<Point> path)
        {
            int endGridX = (int)path[path.Count - 1].X;
            int endGridY = (int)path[path.Count - 1].Y;

            if (Global.GetTwoPointDistance(client.CurrentGrid, new Point(endGridX, endGridY)) >= 3.0)
            {
                return false;
            }

            if (path.Count > 2)
            {
                return false;
            }

            for (int i = 0; i < path.Count; i++)
            {
                Point clientGrid = client.CurrentGrid;

                MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[client.MapCode];
                int gridX = (int)path[i].X;
                int gridY = (int)path[i].Y;

                //已经在位置上了，则跳过
                if (gridX == (int)clientGrid.X && gridY == (int)clientGrid.Y)
                {
                    continue;
                }

                //服务器端不判断障碍(根据俊武的建议，应该加入判断，否则客户端会使用外挂抛入障碍物中)
                if (Global.InObsByGridXY(ObjectTypes.OT_CLIENT, client.MapCode, (int)gridX, gridY, 0))
                {
                    int direction = client.RoleDirection;
                    int tryRun = 0;

                    //通知其他人自己开始移动
                    GameManager.ClientMgr.NotifyOthersMyMovingEnd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                        client, client.MapCode, (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand, client.PosX, client.PosY, direction, tryRun, true);

                    //LogManager.WriteLog(LogTypes.Error, string.Format("TryDirectMove, send moveend, roleName={0}", client.RoleName));

                    break;
                }

                int toX = gridX * mapGrid.MapGridWidth + mapGrid.MapGridWidth / 2;
                int toY = gridY * mapGrid.MapGridHeight + mapGrid.MapGridHeight / 2;

                //确保在中心位置
                client.PosX = toX;
                client.PosY = toY;

                //立即处理格子的移动
                mapGrid.MoveObject(-1, -1, toX, toY, client);

                /// 当前正在做的动作
                client.CurrentAction = (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand;

                //System.Diagnostics.Debug.WriteLine(string.Format("TryDirectMove, toX={0}, toY={1}", toX, toY));
            }

            return true;
        }

        /// <summary>
        /// 角色开始故事版的移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="startMoveTicks"></param>
        /// <param name="pathString"></param>
        public void StartClientStoryboard(KPlayer client, long startMoveTicks)
        {
            StoryBoard4Client.RemoveStoryBoard(client.RoleID);

            string unZipPathString = DataHelper.UnZipStringToBase64(client.RolePathString);
            //System.Diagnostics.Debug.WriteLine(string.Format("解开压缩后，压缩比原始小: {0}, 压缩比例: {1}%", unZipPathString.Length - client.RolePathString.Length, client.RolePathString.Length * 100 / unZipPathString.Length));

            //Console.WriteLine("Start Move WITH PATH :" + unZipPathString);

            List<Point> path = Global.TransStringToPathArr(unZipPathString);
            if (path.Count <= 1) //后边会删掉一个
            {
                return;
            }

            path.RemoveAt(0); //删除第一个格子，因为无必要

            //尝试模仿怪物直接移动
            if (TryDirectMove(client, startMoveTicks, path))
            {
                //Console.WriteLine("TryDirectMove !");
                /// 玩家进行了移动
                if (GameManager.Update9GridUsingNewMode <= 0)
                {
                    //Console.WriteLine("SEND CMD REMOVE AND ADD MONSTER !");
                    ClientManager.DoSpriteMapGridMove(client);
                }

                return;
            }

            //只有自动寻路才使用服务器端故事版算法

           // Console.WriteLine("RUN StoryBoard4Client FOR TU TIM DUONG!");

            StoryBoard4Client sb = new StoryBoard4Client(client.RoleID);
            sb.Completed = Move_Completed;

            GameMap gameMap = GameManager.MapMgr.DictMaps[client.MapCode];

            long ticks = TimeUtil.NOW() * 10000 - (Before1970Ticks * 10000);
            ticks /= 10000;

            startMoveTicks -= Before1970Ticks;

            //long elapsedTicks = ticks - startMoveTicks; //就是这里导致了有些客户端会自动寻路时导致周围角色和怪物隐身
            long elapsedTicks = 0;
            sb.Start(client, path, gameMap.MapGridWidth, gameMap.MapGridHeight, elapsedTicks);

            sb.Binding(); //先开始，后绑定
        }

        /// <summary>
        /// 移动结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Move_Completed(object sender, EventArgs e)
        {
            StoryBoard4Client sb = sender as StoryBoard4Client;
            StoryBoard4Client.RemoveStoryBoard(sb.RoleID);

            //如果是遇到障碍停止，才通知客户端
            if (sb.IsStopped())
            {
                //Console.WriteLine("AREADY CALL STOP!");

                KPlayer client = GameManager.ClientMgr.FindClient(sb.RoleID);
                if (null != client)
                {
                    GameMap gameMap = GameManager.MapMgr.DictMaps[client.MapCode];
                    int toX = gameMap.CorrectWidthPointToGridPoint(client.PosX);
                    int toY = gameMap.CorrectHeightPointToGridPoint(client.PosY);

                    Console.WriteLine("SET POINT X :" + toX + " Y :" + toY);

                    //确保在中心位置
                    client.PosX = toX;
                    client.PosY = toY;

                    /// 当前正在做的动作
                    client.CurrentAction = (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand;

                    int direction = client.RoleDirection;
                    int tryRun = 1;

                    //通知其他人自己开始移动
                    GameManager.ClientMgr.NotifyOthersMyMovingEnd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                        client, client.MapCode, (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand, toX, toY, direction, tryRun, true);

                    /// 玩家进行了移动
                    if (GameManager.Update9GridUsingNewMode <= 0)
                    {
                        ClientManager.DoSpriteMapGridMove(client);
                    }
                }
            }
            else
            {
                KPlayer client = GameManager.ClientMgr.FindClient(sb.RoleID);
                if (null != client)
                {
                    Console.WriteLine("MOVE DONE SWITCH ACTION TO STAND !");
                    /// 当前正在做的动作
                    client.CurrentAction = (int)GameServer.KiemThe.Entities.KE_NPC_DOING.do_stand;

                    /// 移动的速度
                    client.MoveSpeed = 1.0;

                    /// 移动的目的地坐标点
                    client.DestPoint = new Point(client.PosX, client.PosY);
                    //System.Diagnostics.Debug.WriteLine(string.Format("EndStoryboard, PosX={0}, PosY={1}", client.PosX, client.PosY));

                    /// 玩家进行了移动
                    if (GameManager.Update9GridUsingNewMode <= 0)
                    {
                        Console.WriteLine("Notify Remove Monster");
                        ClientManager.DoSpriteMapGridMove(client);
                    }
                }
            }
        }

        /// <summary>
        /// 角色停止故事版的移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="startMoveTicks"></param>
        /// <param name="pathString"></param>
        public void StopClientStoryboard(KPlayer client, int stopIndex = -1)
        {
            if (stopIndex > 0)
            {
                StoryBoard4Client.StopStoryBoard(client.RoleID, stopIndex);
            }
            else
            {
                StoryBoard4Client.RemoveStoryBoard(client.RoleID);
            }
        }

        /// <summary>
        /// 获取角色故事版的最终位置
        /// </summary>
        /// <param name="client"></param>
        /// <param name="startMoveTicks"></param>
        /// <param name="pathString"></param>
        public bool GetClientStoryboardLastPoint(KPlayer client, out Point lastPoint)
        {
            lastPoint = new Point(0, 0);
            StoryBoard4Client sb = StoryBoard4Client.FindStoryBoard(client.RoleID);
            if (null != sb)
            {
                lastPoint = sb.LastPoint;
                return true;
            }

            return false;
        }

        #endregion 角色故事版移动处理

        #region 装备耐久度管理

        /// <summary>
        /// LƯU LẠI ĐỘ BỀN CỦA TRANG BỊ
        /// </summary>
        /// <param name="goodsData"></param>
        /// <param name="subStrong"></param>
        public bool AddEquipStrong(KPlayer client, GoodsData goodsData, int subStrong)
        {
            //LẤY RA ĐỘ BỀN TỐI ĐA
            int maxStrong = Global.GetEquipGoodsMaxStrong(goodsData.GoodsID);
            if (goodsData.Strong >= maxStrong)
            {
                return false;
            }

            int oldStrong = goodsData.Strong;
            int modValue1 = goodsData.Strong / Global.MaxNotifyEquipStrongValue;

            // Độ bền = độ bền tối thiểu
            goodsData.Strong = Math.Min(goodsData.Strong + subStrong, maxStrong);

            int modValue2 = goodsData.Strong / Global.MaxNotifyEquipStrongValue;

            bool hasNotifyClient = false;

            //Ngăn ko cho thông báo thường xuyên tới client
            if (modValue1 != modValue2)
            {
                //Thực hiện ghi theo QUEE để đảm bảo rằng CSDL ko bị chết
                if (GameManager.FlagOptimizeAlgorithm_Props)
                {
                    if (goodsData.Strong < maxStrong)
                    {
                        // Thực hiện lưu lại độ bền
                        GameDb.SetLastDBEquipStrongCmdTicks(client, goodsData.Id, TimeUtil.NOW(), false);
                    }
                    else
                    {
                        GameDb.SetLastDBEquipStrongCmdTicks(client, goodsData.Id, TimeUtil.NOW() - GameDb.MaxDBEquipStrongCmdSlot, false);
                    }
                }

                // Thông báo cho CLIENT về sự thay đổi về độ bền trang bị
                NotifyMySelfEquipStrong(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, goodsData);

                hasNotifyClient = true;
            }
            else
            {
                // Ghi lại thời gian gần đây nhất update độ bền trang bị
                GameDb.SetLastDBEquipStrongCmdTicks(client, goodsData.Id, TimeUtil.NOW(), false);
            }

            //Nếu độ bền bị thay đổi
            if (oldStrong < maxStrong && goodsData.Strong >= maxStrong)
            {
                if (!hasNotifyClient)
                {
                    //Ghi lại độ bền trang bị
                    GameDb.UpdateEquipStrong(client, goodsData);

                    // Notify lại cho người chơi độ bền thay đổi
                    NotifyMySelfEquipStrong(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, goodsData);
                }
                // Refresh lại thuộc tính item và gửi về cho client
                Global.RefreshEquipPropAndNotify(client);
            }

            return true;
        }

        /// <summary>
        /// Trừ thuộc tính độ bền của trang bị
        /// </summary>
        /// <param name="goodsData"></param>
        /// <param name="subStrong"></param>
        public int SubEquipStrong(KPlayer client, GoodsData goodsData, int subStrong)
        {
            int modValue1 = goodsData.Strong / Global.MaxNotifyEquipStrongValue;

            goodsData.Strong = Math.Max(0, goodsData.Strong - subStrong);

            int modValue2 = goodsData.Strong / Global.MaxNotifyEquipStrongValue;

            //Nếu 2 giá trị khác nhau
            if (modValue1 != modValue2)
            {
                //Ghi lại độ bền vào DB
                GameDb.UpdateEquipStrong(client, goodsData);

                // Thông báo cho GAME CLIENT
                NotifyMySelfEquipStrong(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, goodsData);
            }
            else
            {
                GameDb.SetLastDBEquipStrongCmdTicks(client, goodsData.Id, TimeUtil.NOW(), false);
            }

            return modValue2;
        }

        /// <summary>
        /// Notiffy về cho CLIENT
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfEquipStrong(SocketListener sl, TCPOutPacketPool pool, KPlayer client, GoodsData goodsData)
        {
            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, goodsData.Id, goodsData.Strong);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_NOTIFYEQUIPSTRONG);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        #endregion 装备耐久度管理

        #region 道术隐身命令

        /// <summary>
        /// 发送道术隐身命令
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDSHideCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.DSHideStart);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DSHIDECMD);
        }

        /// <summary>
        /// 定时检查道术隐身装备
        /// </summary>
        /// <param name="client"></param>
        public void CheckDSHideState(KPlayer client)
        {
            if (client.DSHideStart <= 0)
            {
                return;
            }

            long nowTicks = TimeUtil.NOW();
            if (nowTicks < client.DSHideStart)
            {
                return;
            }

            KTLogic.RemoveBufferData(client, (int)BufferItemTypes.DSTimeHideNoShow);
            client.DSHideStart = 0;
            GameManager.ClientMgr.NotifyDSHideCmd(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);
        }

        #endregion 道术隐身命令

        #region 法师的护盾和被道士下毒的相关命令

        /// <summary>
        /// 发送角色状态相关的命令
        /// </summary>
        /// <param name="client"></param>
        public void NotifyRoleStatusCmd(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int statusID, long startTicks, int slotSeconds, double tag = 0.0)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, statusID, startTicks, slotSeconds, tag);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_ROLESTATUSCMD);
        }

        /// <summary>
        /// 发送怪物状态相关的命令
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMonsterStatusCmd(SocketListener sl, TCPOutPacketPool pool, Monster monster, int statusID, long startTicks, int slotSeconds, double tag = 0.0)
        {
            List<Object> objsList = Global.GetAll9Clients(monster);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", monster.RoleID, statusID, startTicks, slotSeconds, tag);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_ROLESTATUSCMD);
        }

        #endregion 法师的护盾和被道士下毒的相关命令

        #region 地图特效 相关

        /// <summary>
        /// Deco创建通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfNewDeco(SocketListener sl, TCPOutPacketPool pool, KPlayer client, Decoration deco)
        {
            if (null == deco)
            {
                return;
            }

            DecorationData decoData = new DecorationData()
            {
                AutoID = deco.AutoID,
                DecoID = deco.DecoID,
                MapCode = deco.MapCode,
                PosX = (int)deco.Pos.X,
                PosY = (int)deco.Pos.Y,
                StartTicks = deco.StartTicks,
                MaxLiveTicks = deco.MaxLiveTicks,
                AlphaTicks = deco.AlphaTicks,
            };

            TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<DecorationData>(decoData, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_SPR_NEWDECO);
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
        /// Decoration移除通知
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMySelfDelDeco(SocketListener sl, TCPOutPacketPool pool, KPlayer client, Decoration deco)
        {
            if (null == deco)
            {
                return;
            }

            string strcmd = string.Format("{0}", deco.AutoID);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELDECO);
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
        /// 特效消失通知(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyOthersDelDeco(SocketListener sl, TCPOutPacketPool pool, List<Object> objsList, int mapCode, int autoID)
        {
            if (null == objsList) return;

            string strcmd = string.Format("{0}", autoID);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_DELDECO);
        }

        #endregion 地图特效 相关

        #region 角色基础参数读写 装备积分 猎杀值 悟性值 真气值 天地精元值 试炼令[===>通天令值]值 经脉等级 武学等级 军功值, +角色在线奖励天ID

        /*
         * 悟性值的修改，会触发武学等级的变化，武学等级的变化，会触发武学buffer的变化
         * 成就值的修改，【会触发成就隐含等级的变化，这个等级的变化，表现为成就buffer的变化】
         * 角色每次登录时，也会判断这几个buffer是否应该切换，此外，每个buffer是24小时消失，
         * buffer消失的时候，也需要判断一下，buffer是否需要再次激活
         */

        /// <summary>
        /// 修改成就点 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyChengJiuPointsValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            client.ChengJiuPoints += addValue;
            client.ChengJiuPoints = Math.Max(client.ChengJiuPoints, 0);
            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "成就", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, client.ChengJiuPoints, client.ServerId);

            // 更新到数据库
            // 成就变动时，强制写到数据库 ChenXiaojun
            ChengJiuManager.ModifyChengJiuExtraData(client, (uint)client.ChengJiuPoints, ChengJiuExtraDataField.ChengJiuPoints, true);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.ChengJiu, addValue, client.ChengJiuPoints, strFrom);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ChengJiu, client.ChengJiuPoints);
            }

            client._IconStateMgr.CheckChengJiuUpLevelState(client);
        }

        /// <summary>
        /// 读取成就点
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetChengJiuPointsValue(KPlayer client)
        {
            client.ChengJiuPoints = (int)ChengJiuManager.GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.ChengJiuPoints);

            return client.ChengJiuPoints;
        }

        /// <summary>
        /// 修改成就等级
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int SetChengJiuLevelValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            client.ChengJiuLevel = ChengJiuManager.GetChengJiuLevel(client);
            client.ChengJiuLevel += addValue;

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "成就等级", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, client.ChengJiuLevel, client.ServerId);
            ChengJiuManager.SetChengJiuLevel(client, client.ChengJiuLevel, true);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ChengJiuLevel, client.ChengJiuLevel);
            }

            return client.ChengJiuLevel;
        }

        /// <summary>
        /// 修改装备积分 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZhuangBeiJiFenValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetZhuangBeiJiFenValue(client) + addValue;

            //更新到数据库
            SaveZhuangBeiJiFenValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZhuangBeiJiFen, newValue);
            }
        }

        /// <summary>
        /// 保存装备积分
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZhuangBeiJiFenValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZhuangBeiJiFen, nValue, writeToDB);
        }

        /// <summary>
        /// 读取装备积分
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZhuangBeiJiFenValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZhuangBeiJiFen);
        }

        /// <summary>
        /// 修改猎杀值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyLieShaValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetLieShaValue(client) + addValue;

            //更新到数据库
            SaveLieShaValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.LieShaZhi, newValue);
            }
        }

        /// <summary>
        /// 保存猎杀值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveLieShaValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.LieShaZhi, nValue, writeToDB);
        }

        /// <summary>
        /// 读取猎杀值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetLieShaValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.LieShaZhi);
        }

        /// <summary>
        /// 修改悟性值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyWuXingValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true, bool doChangeWuXueLevel = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetWuXingValue(client) + addValue;

            //更新到数据库
            SaveWuXingValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.WuXingZhi, newValue);
            }

            //如果悟性值增加，则进行下一武学等级激活的判断，有的武学等级是不需要消耗任何物品直接激活的,当悟性值不够的时候，武学等级会下降
            if (doChangeWuXueLevel)
            {
                if (addValue > 0)
                {
                    Global.TryToActivateSpecialWuXueLevel(client);
                }
                else
                {
                    Global.TryToDeActivateSpecialWuXueLevel(client);
                }
            }
        }

        /// <summary>
        /// 保存悟性值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveWuXingValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.WuXingZhi, nValue, writeToDB);
        }

        /// <summary>
        /// 读取悟性值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetWuXingValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.WuXingZhi);
        }

        /// <summary>
        /// 修改真气值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZhenQiValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetZhenQiValue(client) + addValue;

            //更新到数据库
            SaveZhenQiValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZhenQiZhi, newValue);
            }
        }

        /// <summary>
        /// 保存真气值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZhenQiValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZhenQiZhi, nValue, writeToDB);
        }

        /// <summary>
        /// 读取真气值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZhenQiValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZhenQiZhi);
        }

        /// <summary>
        /// 修改星魂值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyStarSoulValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            client.StarSoul += addValue;
            if (client.StarSoul < 0)
                client.StarSoul = 0;

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "星魂", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, client.StarSoul, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.XingHun, addValue, client.StarSoul, strFrom);

            //更新到数据库
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.StarSoul, client.StarSoul, true);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.StarSoulValue, client.StarSoul);
            }
        }

        /// <summary>
        /// 修改精灵积分 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyPetJiFenValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int nPetJiFen = Convert.ToInt32(Global.GetRoleParamByName(client, RoleParamName.PetJiFen)) + addValue;

            //更新到数据库
            GameDb.UpdateRoleParamByName(client, RoleParamName.PetJiFen, nPetJiFen.ToString(), true);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.PetJiFen, nPetJiFen);
            }
            // 日志
            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "精灵积分", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, nPetJiFen, client.ServerId);
        }

        /// <summary>
        /// 修改元素粉末值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyYuanSuFenMoValue(KPlayer client, int addValue, string strFrom, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int currPowder = Global.GetRoleParamsInt32FromDB(client, RoleParamName.ElementPowderCount);
            int newPowder = currPowder + addValue;
            // if (newPowder < 0)
            //    newPowder = 0;

            if (newPowder == currPowder)
                return;

            addValue = newPowder - currPowder;

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "元素粉末", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newPowder, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.YuanSuFenMo, addValue, newPowder, strFrom);

            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.ElementPowderCount, newPowder, true);

            if (notifyClient)
            {
                GameManager.ClientMgr.NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.YuansuFenmo, newPowder);
            }
        }

        /// <summary>
        /// 修改灵晶值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyMUMoHeValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetMUMoHeValue(client) + addValue;
            // newValue = Math.Max(0, newValue);
            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "魔核", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.MUMoHe, addValue, newValue, strFrom);

            //更新到数据库
            SaveMUMoHeValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.MUMoHe, newValue);
            }
        }

        /// <summary>
        /// 保存天地精元值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveMUMoHeValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.MUMoHe, nValue, writeToDB);
        }

        /// <summary>
        /// 读取魔核值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetMUMoHeValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.MUMoHe);
        }

        /// <summary>
        /// 修改天地精元值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyTianDiJingYuanValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            long oldValue = GetTianDiJingYuanValue(client);
            long targetValue = oldValue + addValue;
            int newValue = targetValue > int.MaxValue ? int.MaxValue : (int)targetValue;

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "魔晶", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.JingYuanZhi, addValue, newValue, strFrom);

            //更新到数据库
            SaveTianDiJingYuanValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.TianDiJingYuan, newValue);
            }

            // 七日活动
            GlobalEventSource.getInstance().fireEvent(SevenDayGoalEvPool.Alloc(client, ESevenDayGoalFuncType.MoJingCntInBag));
        }

        /// <summary>
        /// 保存天地精元值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveTianDiJingYuanValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.TianDiJingYuan, nValue, writeToDB);
        }

        /// <summary>
        /// 读取天地精元值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetTianDiJingYuanValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.TianDiJingYuan);
        }

        #region 再造点

        /// <summary>
        /// 再造点——修改 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZaiZaoValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
                return;

            int newValue = GetZaiZaoValue(client) + addValue;
            // newValue = Math.Max(newValue, 0);
            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "再造点", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, -1, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.ZaiZao, addValue, newValue, strFrom);

            //更新到数据库
            SaveZaiZaoValue(client, newValue, writeToDB);

            if (notifyClient)
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZaiZaoPoint, newValue);
        }

        /// <summary>
        /// 再造点——保存
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZaiZaoValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZaiZaoPoint, nValue, writeToDB);
        }

        /// <summary>
        /// 再造点——读取
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZaiZaoValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZaiZaoPoint);
        }

        #endregion 再造点

        /// <summary>
        /// 修改试炼令值 addValue > 0,增加，小于0，减少 ===>通天令值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyShiLianLingValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetShiLianLingValue(client) + addValue;

            //更新到数据库
            SaveShiLianLingValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ShiLianLing, newValue);
            }
        }

        /// <summary>
        /// 保存试炼令值===>通天令值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveShiLianLingValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ShiLianLing, nValue, writeToDB);
        }

        /// <summary>
        /// 读取试炼令值===>通天令值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetShiLianLingValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ShiLianLing);
        }

        /// <summary>
        /// 修改经脉等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        /*public void ModifyJingMaiLevelValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetJingMaiLevelValue(client) + addValue;

            //更新到数据库
            SaveJingMaiLevelValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.JingMaiLevel, newValue);
            }

            //经脉等级变化 激活新的经脉buffer 这个是永久buffer
            Global.ActiveJinMaiBuffer(client, true);

            //处理经脉成就 如果gm命令，可能会减少经脉等级，减少就不取消成就了
            if (addValue > 0)
            {
                ChengJiuManager.OnJingMaiLevelUp(client, newValue);
            }
        }*/

        /// <summary>
        /// 保存经脉等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveJingMaiLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.JingMaiLevel, nValue, writeToDB);
        }

        /// <summary>
        /// 读取经脉等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetJingMaiLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.JingMaiLevel);
        }

        /// <summary>
        /// 修改武学等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        /*public void ModifyWuXueLevelValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetWuXueLevelValue(client) + addValue;

            //更新到数据库
            SaveWuXueLevelValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.WuXueLevel, newValue);
            }

            //如果武学等级变化，武学等级可能下降，尝试激活新的武学buffer
            Global.TryToActiveNewWuXueBuffer(client, true);

            //处理武学成就 如果gm命令，可能会减少武学等级，减少就不取消成就了
            if (addValue > 0)
            {
                ChengJiuManager.OnWuXueLevelUp(client, newValue);
            }
        }*/

        /// <summary>
        /// 保存武学等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveWuXueLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.WuXueLevel, nValue, writeToDB);
        }

        /// <summary>
        /// 读取武学等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetWuXueLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.WuXueLevel);
        }

        /// <summary>
        /// 修改钻皇等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZuanHuangLevelValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetZuanHuangLevelValue(client) + addValue;

            //更新到数据库
            SaveZuanHuangLevelValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZuanHuangLevel, newValue);
            }
        }

        /// <summary>
        /// 保存钻皇等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZuanHuangLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZuanHuangLevel, nValue, writeToDB);
        }

        /// <summary>
        /// 读取钻皇等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZuanHuangLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZuanHuangLevel);
        }

        /// <summary>
        /// addValue 是激活项索引
        /// 修改系统激活项值 addValue 必须大于等于0，且小于等于31
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifySystemOpenValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 非法参数不进行任何处理
            if (addValue < 0 || addValue > 31)
            {
                return;
            }

            int newValue = GetSystemOpenValue(client) | (int)(1 << addValue);

            //更新到数据库
            SaveSystemOpenValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.SystemOpenValue, newValue);
            }
        }

        /// <summary>
        /// 保存系统激活项值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveSystemOpenValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.SystemOpenValue, nValue, writeToDB);
        }

        /// <summary>
        /// 读取系统激活项值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetSystemOpenValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.SystemOpenValue);
        }

        /// <summary>
        /// 修改军功值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyJunGongValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetJunGongValue(client) + addValue;

            //更新到数据库
            SaveJunGongValue(client, newValue, writeToDB);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.JunGongZhi, addValue, newValue, "none");

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.JunGong, newValue);
            }
        }

        /// <summary>
        /// 保存军功值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveJunGongValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.JunGong, nValue, writeToDB);
        }

        /// <summary>
        /// 读取军功值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetJunGongValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.JunGong);
        }

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

        #region 藏宝秘境

        /// <summary>
        /// 修改藏宝积分 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyTreasureJiFenValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
                return;

            int newValue = GetTreasureJiFen(client) + addValue;

            //更新到数据库
            SaveTreasureJiFenValue(client, newValue, writeToDB);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.BaoZangJiFen, addValue, newValue, "none");

            if (notifyClient)                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.TreasureJiFen, newValue);
        }

        public void ModifyTreasureXueZuanValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
                return;

            int newValue = GetTreasureXueZuan(client) + addValue;

            //更新到数据库
            SaveTreasureXueZuanValue(client, newValue, writeToDB);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.BaoZangXueZuan, addValue, newValue, "none");

            if (notifyClient)                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.TreasureXueZuan, newValue);
        }

        /// <summary>
        /// 获取一个角色的藏宝积分
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetTreasureJiFen(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.TreasureJiFen);
        }

        /// <summary>
        /// 保存一个角色的藏宝积分
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public void SaveTreasureJiFenValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.TreasureJiFen, nValue, writeToDB);
        }

        /// <summary>
        /// 获取一个角色的藏宝血钻
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetTreasureXueZuan(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.TreasureXueZuan);
        }

        /// <summary>
        /// 保存一个角色的藏宝血钻
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public void SaveTreasureXueZuanValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.TreasureXueZuan, nValue, writeToDB);
        }

        #endregion 藏宝秘境

        /// <summary>
        /// 修改战魂 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZhanHunValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetZhanHunValue(client) + addValue;

            //更新到数据库
            SaveZhanHunValue(client, newValue, writeToDB);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.ZhanHun, addValue, newValue, "none");

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZhanHun, newValue);
            }
        }

        /// <summary>
        /// 修改天梯荣耀 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public bool ModifyTianTiRongYaoValue(KPlayer client, int addValue, string strFrom, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 != addValue)
            {
                client.TianTiData.RongYao += addValue;
                RoleAttributeValueData roleAttributeValueData = new RoleAttributeValueData()
                {
                    RoleAttribyteType = (int)RoleAttribyteTypes.RongYao,
                    Targetvalue = client.TianTiData.RongYao,
                    AddVAlue = addValue,
                };

                Global.sendToDB<int, int[]>((int)TCPGameServerCmds.CMD_DB_TIANTI_UPDATE_RONGYAO, new int[] { client.RoleID, client.TianTiData.RongYao }, client.ServerId);

                // 日志
                GameManager.logDBCmdMgr.AddDBLogInfo(-1, "荣耀", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, client.TianTiData.RongYao, client.ServerId);
                EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.TianTiRongYao, addValue, client.TianTiData.RongYao, strFrom);

                if (notifyClient)
                {
                    client.sendCmd((int)TCPGameServerCmds.CMD_SPR_ROLE_ATTRIBUTE_VALUE, roleAttributeValueData);
                }
            }

            return true;
        }

        /// <summary>
        /// 保存战魂
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZhanHunValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZhanHun, nValue, writeToDB);
        }

        /// <summary>
        /// 读取战魂
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZhanHunValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZhanHun);
        }

        /// <summary>
        /// 修改荣誉 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyRongYuValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetRongYuValue(client) + addValue;

            //更新到数据库
            SaveRongYuValue(client, newValue, writeToDB);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.RongYu, addValue, newValue, "none");

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.RongYu, newValue);
            }
        }

        /// <summary>
        /// 保存荣誉
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveRongYuValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.RongYu, nValue, writeToDB);
        }

        /// <summary>
        /// 读取荣誉
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetRongYuValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.RongYu);
        }

        /// <summary>
        /// 修改战魂等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZhanHunLevelValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetZhanHunLevelValue(client) + addValue;

            //更新到数据库
            SaveZhanHunLevelValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZhanHunLevel, newValue);
            }

            //战魂等级变化 激活新的战魂buffer 这个是永久buffer
            Global.ActiveZhanHunBuffer(client, true);
        }

        /// <summary>
        /// 保存战魂等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveZhanHunLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ZhanHunLevel, nValue, writeToDB);
        }

        /// <summary>
        /// 读取战魂等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZhanHunLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ZhanHunLevel);
        }

        /// <summary>
        /// 修改荣誉等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyRongYuLevelValue(KPlayer client, int addValue, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetRongYuLevelValue(client) + addValue;

            //更新到数据库
            SaveRongYuLevelValue(client, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.RongYuLevel, newValue);
            }
        }

        /// <summary>
        /// 保存荣誉等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveRongYuLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.RongYuLevel, nValue, writeToDB);
        }

        /// <summary>
        /// 读取荣誉等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetRongYuLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.RongYuLevel);
        }

        /// <summary>
        /// 修改声望 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyShengWangValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetShengWangValue(client) + addValue;
            //newValue = Math.Max(newValue, 0);
            // 更新到数据库
            // 声望改变时，强制写到数据 ChenXiaojun
            SaveShengWangValue(client, newValue, true);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ShengWang, newValue);
            }

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "声望", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.ShengWang, addValue, newValue, strFrom);

            // 增加竞技场声望时，刷新图标状态
            if (addValue > 0)
            {
                client._IconStateMgr.SendIconStateToClient(client);
            }
        }

        /// <summary>
        /// 修改狼魂粉末 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyLangHunFenMoValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            if (client == null) return;

            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            long lNewValue = (long)GetLangHunFenMoValue(client) + addValue;
            lNewValue = Math.Min(lNewValue, int.MaxValue);

            int newValue = (int)lNewValue;
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.LangHunFenMo, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.LangHunFenMo, newValue);
            }

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "狼魂粉末", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.LangHunFenMo, addValue, newValue, strFrom);
        }

        /// <summary>
        /// 获取狼魂粉末值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetLangHunFenMoValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.LangHunFenMo);
        }

        /// <summary>
        /// 修改王者点数 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyKingOfBattlePointValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            if (client == null) return;

            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            long lNewValue = (long)GetKingOfBattlePointValue(client) + addValue;
            lNewValue = Math.Min(lNewValue, int.MaxValue);

            int newValue = (int)lNewValue;
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.KingOfBattlePoint, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.KingOfBattlePoint, newValue);
            }

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "王者争霸点数", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.KingOfBattlePoint, addValue, newValue, strFrom);
        }

        /// <summary>
        /// 获取王者争霸点数
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetKingOfBattlePointValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.KingOfBattlePoint);
        }

        /// <summary>
        /// 修改狼魂粉末 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyZhengBaPointValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            if (client == null) return;

            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            long lNewValue = (long)GetZhengBaPointValue(client) + addValue;
            lNewValue = Math.Min(lNewValue, int.MaxValue);

            int newValue = (int)lNewValue;
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.ZhengBaPoint, newValue, writeToDB);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ZhengBaPoint, newValue);
            }

            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "争霸点", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);
            EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.ZhengBaPoint, addValue, newValue, strFrom);
        }

        /// <summary>
        /// 获取狼魂粉末值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetZhengBaPointValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32FromDB(client, RoleParamName.ZhengBaPoint);
        }

        #region 万魔塔通关层数

        /// <summary>
        /// 保存万魔塔通关层数
        /// </summary>
        public void SaveWanMoTaPassLayerValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.WanMoTaCurrLayerOrder, nValue, true);
        }

        /// <summary>
        /// 读取万魔塔通关层数
        /// </summary>
        public int GetWanMoTaPassLayerValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.WanMoTaCurrLayerOrder);
        }

        #endregion 万魔塔通关层数

        /// <summary>
        /// 保存声望
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveShengWangValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ShengWang, nValue, writeToDB);
        }

        /// <summary>
        /// 读取声望
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetShengWangValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ShengWang);
        }

        /// <summary>
        /// 修改声望等级值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyShengWangLevelValue(KPlayer client, int addValue, string strFrom, bool writeToDB = false, bool notifyClient = true)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetShengWangLevelValue(client) + addValue;

            //更新到数据库
            SaveShengWangLevelValue(client, newValue, writeToDB);
            GameManager.logDBCmdMgr.AddDBLogInfo(-1, "声望等级", strFrom, "系统", client.RoleName, "修改", addValue, client.ZoneID, client.strUserID, newValue, client.ServerId);

            // MU成就处理 -- 军衔成就 [3/30/2014 LiaoWei]
            ChengJiuManager.OnRoleJunXianChengJiu(client);

            if (notifyClient)
            {
                //通知自己
                NotifySelfParamsValueChange(client, RoleCommonUseIntParamsIndexs.ShengWangLevel, newValue);
            }

            // 七日活动
            GlobalEventSource.getInstance().fireEvent(SevenDayGoalEvPool.Alloc(client, ESevenDayGoalFuncType.JunXianLevel));
        }

        /// <summary>
        /// 保存声望等级值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveShengWangLevelValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.ShengWangLevel, nValue, writeToDB);

            //[bing] 刷新客户端活动叹号
            if (client._IconStateMgr.CheckJieRiFanLi(client, ActivityTypes.JieriMilitaryRank)
                || client._IconStateMgr.CheckSpecialActivity(client))
            {
                client._IconStateMgr.AddFlushIconState((ushort)ActivityTipTypes.JieRiActivity, client._IconStateMgr.IsAnyJieRiTipActived());
                client._IconStateMgr.SendIconStateToClient(client);
            }
        }

        /// <summary>
        /// 读取声望等级值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetShengWangLevelValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.ShengWangLevel);
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

        /// <summary>
        /// 修改离线摆摊时长值 addValue > 0,增加，小于0，减少
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void ModifyLiXianBaiTanTicksValue(KPlayer client, int addValue, bool writeToDB = false)
        {
            //对 0 参数不进行任何处理
            if (0 == addValue)
            {
                return;
            }

            int newValue = GetLiXianBaiTanTicksValue(client) + addValue;

            //更新到数据库
            SaveLiXianBaiTanTicksValue(client, newValue, writeToDB);
        }

        /// <summary>
        /// 保存离线摆摊时长值
        /// </summary>
        /// <param name="client"></param>
        /// <param name="jiFen"></param>
        /// <param name="writeToDB"></param>
        public void SaveLiXianBaiTanTicksValue(KPlayer client, int nValue, bool writeToDB = false)
        {
            Global.SaveRoleParamsInt32ValueWithTimeStampToDB(client, RoleParamName.LiXianBaiTanTicks, nValue, writeToDB);
        }

        /// <summary>
        /// 读取离线摆摊时长值
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetLiXianBaiTanTicksValue(KPlayer client)
        {
            return Global.GetRoleParamsInt32ValueWithTimeStampFromDB(client, RoleParamName.LiXianBaiTanTicks);
        }

        #endregion 角色基础参数读写 装备积分 猎杀值 悟性值 真气值 天地精元值 试炼令[===>通天令值]值 经脉等级 武学等级 军功值, +角色在线奖励天ID

        #region 游戏特殊效果播放(下雨，下雪，落花，烟花等)

        /// <summary>
        /// 播放游戏特殊效果(只给指定的角色)
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="lifeTicks"></param>
        public void SendGameEffect(KPlayer client, string effectName, int lifeTicks, GameEffectAlignModes alignMode = GameEffectAlignModes.None, string mp3Name = "")
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}",
                effectName,
                lifeTicks,
                (int)alignMode,
                mp3Name);

            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYGAMEEFFECT);
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

        /// <summary>
        /// 播放游戏特殊效果
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copyMapID"></param>
        /// <param name="effectName"></param>
        /// <param name="lifeTicks"></param>
        /// <param name="alignMode"></param>
        public void BroadCastGameEffect(int mapCode, int copyMapID, string effectName, int lifeTicks, GameEffectAlignModes alignMode = GameEffectAlignModes.None, string mp3Name = "")
        {
            string strcmd = string.Format("{0}:{1}:{2}:{3}",
                effectName,
                lifeTicks,
                (int)alignMode,
                mp3Name);

            List<Object> objsList = GetMapClients(mapCode);
            if (null == objsList)
            {
                return;
            }

            objsList = Global.ConvertObjsList(mapCode, copyMapID, objsList);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYGAMEEFFECT);
        }

        #endregion 游戏特殊效果播放(下雨，下雪，落花，烟花等)

        #region 节日称号管理

        /// <summary>
        /// 播报节日称号
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public void BroadcastJieriChengHao(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, client.JieriChengHao);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_CHGJIERICHENGHAO);
        }

        #endregion 节日称号管理

        #region 砸金蛋积分奖励相关

        /// <summary>
        /// 将砸金蛋积分奖励相关通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyZaJinDanKAwardDailyData(KPlayer client)
        {
            int jiFen = Global.GetZaJinDanJifen(client);
            int jiFenBits = Global.GetZaJinDanJiFenBits(client);

            string strcmd = string.Format("{0}:{1}", jiFen, jiFenBits);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_ZJDJIFEN);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 砸金蛋积分奖励相关

        #region 冥想

        /// <summary>
        /// 通知用户某个精灵的冥想状态改变
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteMeditate(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int meditate)
        {
            List<Object> objsList = Global.GetAll9Clients(client);
            if (null == objsList) return;

            string strcmd = string.Format("{0}:{1}", client.RoleID, meditate);

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, (int)TCPGameServerCmds.CMD_SPR_STARTMEDITATE);
        }

        /// <summary>
        /// 将冥想的时间累计通知自己
        /// </summary>
        /// <param name="client"></param>
        public void NotifyMeditateTime(KPlayer client)
        {
            int msecs1 = Global.GetRoleParamsInt32FromDB(client, RoleParamName.MeditateTime) / 1000;
            int msecs2 = Global.GetRoleParamsInt32FromDB(client, RoleParamName.NotSafeMeditateTime) / 1000;

            string strcmd = string.Format("{0}:{1}:{2}", client.RoleID, msecs1, msecs2);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_GETMEDITATETIMEINFO);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 冥想

        #region 通知boss动画

        /// <summary>
        /// 将boss要刷新的动画消息通知客户端（因为一个地图可能存在多个角色，因此，只有单人副本才能配置boss动画，只发送给第一个角色）
        /// </summary>
        /// <param name="client"></param>
        public void NotifyPlayBossAnimation(KPlayer client, int monsterID, int mapCode, int toX, int toY, int effectX, int effectY)
        {
            long ticks = TimeUtil.NOW();
            int checkCode = Global.GetBossAnimationCheckCode(monsterID, mapCode, toX, toY, effectX, effectY, ticks);

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", client.RoleID, monsterID, mapCode, toX, toY, effectX, effectY, ticks, checkCode);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_PLAYBOSSANIMATION);
            if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
            {
                /*LogManager.WriteLog(LogTypes.Error, string.Format("向用户发送tcp数据失败: ID={0}, Size={1}, RoleID={2}, RoleName={3}",
                    tcpOutPacket.PacketCmdID,
                    tcpOutPacket.PacketDataSize,
                    client.RoleID,
                    client.RoleName));*/
            }
        }

        #endregion 通知boss动画

        #region 通知扩展属性命中

        /// <summary>
        /// 通知所有在线用户某个精灵的扩展属性被命中(同一个地图才需要通知)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySpriteExtensionPropsHited(SocketListener sl, TCPOutPacketPool pool, IObject attacker, int enemy, int enemyX, int enemyY, int extensionPropID)
        {
            List<Object> objsList = Global.GetAll9Clients(attacker);
            if (null == objsList) return;

            SpriteExtensionPropsHitedData hitedData = new SpriteExtensionPropsHitedData();

            hitedData.roleId = attacker.GetObjectID();
            hitedData.enemy = enemy;
            hitedData.enemyX = enemyX;
            hitedData.enemyY = enemyY;
            hitedData.ExtensionPropID = extensionPropID;

            SendToClients(sl, pool, null, objsList, DataHelper.ObjectToBytes<SpriteExtensionPropsHitedData>(hitedData), (int)TCPGameServerCmds.CMD_SPR_EXTENSIONPROPSHITED);
        }

        #endregion 通知扩展属性命中

        #region 向地图内的角色广播消息

        /// <summary>
        /// 向地图中的用户广播特殊提示信息
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialHintText(int mapCode, int copyMapID, string text)
        {
            List<Object> objsList = GameManager.ClientMgr.GetMapClients(mapCode);
            if (null == objsList || objsList.Count <= 0) return;

            List<Object> objsList2 = new List<Object>();
            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i] as KPlayer;
                if (c == null) continue;
                if (c.CopyMapID != copyMapID) continue;

                objsList2.Add(c);
            }

            text = text.Replace(":", " ");
            string strcmd = string.Format("{0}", text);

            //群发消息
            SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, objsList2, strcmd, (int)TCPGameServerCmds.CMD_SPR_BROADSPECIALHINTTEXT);
        }

        /// <summary>
        /// 向地图中的用户广播地图事件(清除光幕等)
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialMapAIEvent(int mapCode, int copyMapID, int guangMuID, int show)
        {
            string strcmd = string.Format("{0}:{1}", guangMuID, show);

            List<Object> objsList = GameManager.ClientMgr.GetMapClients(mapCode);
            if (null == objsList || objsList.Count <= 0) return;
            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i] as KPlayer;
                if (c == null) continue;
                if (c.CopyMapID != copyMapID) continue;
                c.sendCmd((int)TCPGameServerCmds.CMD_SPR_MAPAIEVENT, strcmd);
            }
        }

        /// <summary>
        /// 向地图中的用户广播消息(Boss动画等)
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialMapMessage(int cmdID, string strcmd, int mapCode, int copyMapID)
        {
            List<Object> objsList = GameManager.ClientMgr.GetMapClients(mapCode);
            if (null == objsList || objsList.Count <= 0) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i] as KPlayer;
                if (c == null) continue;

                if (c.CopyMapID != copyMapID) continue;

                c.sendCmd(cmdID, strcmd);
            }
        }

        /// <summary>
        /// 向地图中的用户广播消息
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialMapMessage(TCPOutPacket tcpOutPacket, int mapCode, int copyMapID, bool pushBack = true)
        {
            List<Object> objsList = GameManager.ClientMgr.GetMapClients(mapCode);
            if (null == objsList || objsList.Count <= 0) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i] as KPlayer;
                if (c == null) continue;

                if (c.CopyMapID != copyMapID) continue;

                c.sendCmd(tcpOutPacket, false);
            }

            if (pushBack)
            {
                Global.PushBackTcpOutPacket(tcpOutPacket);
            }
        }

        /// <summary>
        /// 向副本地图中的用户广播消息
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="strcmd"></param>
        /// <param name="copyMap"></param>
        /// <param name="insertRoleID"></param>
        public void BroadSpecialCopyMapMessageStr(int cmdID, string strcmd, CopyMap copyMap, bool insertRoleID = false)
        {
            List<KPlayer> objsList = copyMap.GetClientsList();
            if (null == objsList || objsList.Count <= 0) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i];
                if (c == null) continue;

                if (c.CopyMapID != copyMap.CopyMapID) continue;

                if (insertRoleID)
                {
                    c.sendCmd(cmdID, strcmd.Insert(0, string.Format("{0}:", c.RoleID)));
                }
                else
                {
                    c.sendCmd(cmdID, strcmd);
                }
            }
        }

        /// <summary>
        /// 向地图中的用户广播消息(Boss动画等)
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialCopyMapMessage<T>(int cmdID, T data, CopyMap copyMap)
        {
            List<KPlayer> objsList = copyMap.GetClientsList();
            if (null == objsList || objsList.Count <= 0) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i];
                if (c != null && c.CopyMapID == copyMap.CopyMapID)
                {
                    c.sendCmd(cmdID, data);
                }
            }
        }

        /// <summary>
        /// 向地图中的用户广播消息(Boss动画等)
        /// </summary>
        /// <param name="text"></param>
        public void BroadSpecialCopyMapMessage(int cmdID, string strcmd, List<KPlayer> objsList, bool insertRoleID = false)
        {
            if (null == objsList || objsList.Count <= 0) return;

            for (int i = 0; i < objsList.Count; i++)
            {
                KPlayer c = objsList[i];
                if (c == null) continue;

                if (insertRoleID)
                {
                    c.sendCmd(cmdID, strcmd.Insert(0, string.Format("{0}:", c.RoleID)));
                }
                else
                {
                    c.sendCmd(cmdID, strcmd);
                }
            }
        }

        /// <summary>
        /// 副本地图群发提示信息
        /// </summary>
        /// <param name="copymap"></param>
        /// <param name="msg"></param>
        public void BroadSpecialCopyMapHintMsg(CopyMap copymap, string msg)
        {
            try
            {
                msg = msg.Replace(":", "``");
                string strcmd = string.Format("{0}:{1}:{2}:{3}", (int)ShowGameInfoTypes.ErrAndBox, (int)GameInfoTypeIndexes.Error, msg, (int)HintErrCodeTypes.None);
                BroadSpecialCopyMapMessageStr((int)TCPGameServerCmds.CMD_SPR_NOTIFYMSG, strcmd, copymap);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 副本地图群发提示信息
        /// </summary>
        /// <param name="copymap"></param>
        /// <param name="msg"></param>
        public void BroadSpecialCopyMapMsg(CopyMap copymap, string msg, ShowGameInfoTypes showGameInfoType = ShowGameInfoTypes.OnlySysHint, GameInfoTypeIndexes infoType = GameInfoTypeIndexes.Hot, int error = (int)HintErrCodeTypes.None)
        {
            try
            {
                msg = msg.Replace(":", "``");
                string strcmd = string.Format("{0}:{1}:{2}:{3}", (int)showGameInfoType, (int)infoType, msg, error);
                BroadSpecialCopyMapMessageStr((int)TCPGameServerCmds.CMD_SPR_NOTIFYMSG, strcmd, copymap);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 通知客户端切换到常规地图
        /// </summary>
        /// <param name="client"></param>
        public void NotifyChangMap2NormalMap(KPlayer client)
        {
            if (Global.CanChangeMap(client, client.LastMapCode, client.LastPosX, client.LastPosY, true))
            {
                GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, client.LastMapCode, client.LastPosX, client.LastPosY, -1);
            }
            else
            {
                GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, GameManager.MainMapCode, -1, -1, -1);
            }
        }

        #endregion 向地图内的角色广播消息

        #endregion 扩展属性和方法

        #region 后台工作线程调用方法

        /// <summary>
        /// 补血补魔
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteLifeMagic(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            long subTicks = 0;
            long ticks = TimeUtil.NOW();
            subTicks = ticks - c.LastLifeMagicTick;

            //如果还没到时间，则跳过
            if (subTicks < (10 * 1000))
            {
                return;
            }

            c.LastLifeMagicTick = ticks;
            RoleRelifeLog relifeLog = new RoleRelifeLog(c.RoleID, c.RoleName, c.MapCode, "自然恢复补血补蓝");

            //如果已经死亡，则不再调度
            if (c.m_CurrentLife > 0)
            {
                bool doRelife = false;

                //判断如果血量少于最大血量
                if (c.m_CurrentLife < c.m_CurrentLifeMax)
                {
                    doRelife = true;
                    relifeLog.hpModify = true;
                    relifeLog.oldHp = c.m_CurrentLife;

                    double percent = RoleAlgorithm.GetLifeRecoverValPercentV(c);
                    double lifeMax = percent * c.m_CurrentLifeMax;
                    lifeMax = lifeMax * (1.0 + RoleAlgorithm.GetLifeRecoverAddPercentV(c) + DBRoleBufferManager.ProcessHuZhaoRecoverPercent(c) + RoleAlgorithm.GetLifeRecoverAddPercentOnlySandR(c));

                    //if (c.CurrentAction == (int)GActions.Sit) //如果是在打坐中，则快
                    //{
                    //    lifeMax *= 2;
                    //}

                    lifeMax += c.m_CurrentLife;
                    c.m_CurrentLife = (int)Global.GMin(c.m_CurrentLifeMax, lifeMax);
                    relifeLog.newHp = c.m_CurrentLife;
                    //GameManager.SystemServerEvents.AddEvent(string.Format("角色加血, roleID={0}({1}), Add={2}, Life={3}", c.RoleID, c.RoleName, percent * c.m_CurrentLifeMax, c.m_CurrentLife), EventLevels.Debug);
                }

                //判断如果魔量少于最大魔量
                if (c.m_CurrentMana < c.m_CurrentManaMax)
                {
                    doRelife = true;
                    relifeLog.mpModify = true;
                    relifeLog.oldMp = c.m_CurrentMana;

                    double percent = RoleAlgorithm.GetMagicRecoverValPercentV(c);
                    double magicMax = percent * c.m_CurrentManaMax;
                    magicMax = magicMax * (1.0 + RoleAlgorithm.GetMagicRecoverAddPercentV(c) + RoleAlgorithm.GetMagicRecoverAddPercentOnlySandR(c));

                    //if (c.CurrentAction == (int)GActions.Sit) //如果是在打坐中，则快
                    //{
                    //    magicMax *= 2;
                    //}

                    magicMax += c.m_CurrentMana;
                    c.m_CurrentMana = (int)Global.GMin(c.m_CurrentManaMax, magicMax);
                    relifeLog.newMp = c.m_CurrentMana;
                    //GameManager.SystemServerEvents.AddEvent(string.Format("角色加魔, roleID={0}({1}), Add={2}, Magic={3}", c.RoleID, c.RoleName, percent * c.m_CurrentManaMax, c.m_CurrentMana), EventLevels.Debug);
                }

                if (doRelife)
                {
                    //通知客户端怪已经加血加魔
                    List<Object> listObjs = Global.GetAll9Clients(c);
                    GameManager.ClientMgr.NotifyOthersRelife(sl, pool, c, c.MapCode, c.CopyMapID, c.RoleID, (int)c.PosX, (int)c.PosY, (int)c.RoleDirection, c.m_CurrentLife, c.m_CurrentMana, c.m_CurrentStamina, (int)TCPGameServerCmds.CMD_SPR_RELIFE, listObjs);
                }

                MonsterAttackerLogManager.Instance().AddRoleRelifeLog(relifeLog);
            }
        }

        //2011-05-31 精简指令
        /// <summary>
        /// 与DBServer保持心跳连接
        /// </summary>
        /// <param name="client"></param>
        //private void KeepHeartWithDBServer(KPlayer client)
        //{
        //    string strcmd = string.Format("{0}", client.RoleID);
        //    string[] fields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_ROLE_HEART, strcmd);
        //    if (null == fields) return;
        //    if (fields.Length != 2)
        //    {
        //        return;
        //    }

        //    //判断是否更新了游戏元宝
        //    int addUserMoney = Convert.ToInt32(fields[1]);
        //    if (addUserMoney > 0)
        //    {
        //        client.UserMoney += addUserMoney; //更新用户的元宝

        //        // 钱更新通知(只通知自己)
        //        GameManager.ClientMgr.NotifySelfUserMoneyChange(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client);
        //    }
        //}

        /// <summary>
        /// 玩家心跳
        /// </summary>
        private void DoSpriteHeart(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            // 玩家每日在线时长[1/16/2014 LiaoWei]
            // client.DayOnlineSecond += Math.Max(0, (int)(addTicks / 1000));
            // 玩家每日在线时长[05/25/2014 ChenXiaojun]
            // 移到同一个线程处理
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
        /// 自动挂机的调度
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteAutoFight(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            long nowTicks = TimeUtil.NOW();

            //自动挂机自动大范围拾取
            c.AutoGetThingsOnAutoFight(nowTicks);

            //判断如果是在自动战斗中，则消减自动挂机时间
            //if (c.AutoFighting)
            //{
            //    if (c.AutoFightingProctect <= 0)
            //    {
            //        long ticks = TimeUtil.NOW();
            //        if (ticks - c.LastAutoFightTicks >= (5 * 60 * 1000)) //超过5分钟，才进入被保护状态
            //        {
            //            //处理挂机保护卡
            //            if (DBRoleBufferManager.ProcessAutoFightingProtect(c))
            //            {
            //                c.AutoFightingProctect = 1;

            //                GameManager.ClientMgr.NotifyImportantMsg(sl, pool, c,
            //                    StringUtil.substitute(Global.GetLang("自动战斗进入了【战斗保护】状态")),
            //                    GameInfoTypeIndexes.Hot, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.EnterAFProtect);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Hàm AFK được exp cái này sau này có thể chế thành ăn bạch cầu hoàn
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteSitExp(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
           
        }

        /// <summary>
        /// 处理消减PK点
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteSubPKPoint(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
          

            long subTicks = 0;
            long ticks = TimeUtil.NOW();
            subTicks = ticks - c.LastSiteSubPKPointTicks;

            //如果还没到时间，则跳过
            if (subTicks < (60 * 1000)) // 原来是36 现在改成 60秒   [4/21/2014 LiaoWei]
            {
                return;
            }

            c.LastSiteSubPKPointTicks = ticks;
            if (c.PKPoint <= 0) //已经为0，不需要再消减
            {
                return;
            }


            int oldPKPoint = c.PKPoint;

            //消减PK值
            c.PKPoint = Global.GMax(c.PKPoint - Data.ConstSubPKPointPerMin, 0);    // 原来是-1 现在改成-2 [4/21/2014 LiaoWei]

            c.TmpPKPoint += Data.ConstSubPKPointPerMin;

            if (oldPKPoint != c.PKPoint)
            {
                // 设置PK值(限制当前地图)
                if (c.TmpPKPoint >= 60)
                {
                    SetRolePKValuePoint(sl, pool, c, c.PKValue, c.PKPoint, true);
                    c.TmpPKPoint = 0;
                }
                else
                    SetRolePKValuePoint(sl, pool, c, c.PKValue, c.PKPoint, false);
            }
        }

        /// <summary>
        /// 处理DBBuffer中的项
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteBuffers(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            //处理经验buffer，定时给经验
            DBRoleBufferManager.ProcessAutoGiveExperience(c);

            //去除生命符咒
            DBRoleBufferManager.RemoveUpLifeLimitStatus(c);

            //去除攻击的buffer
            DBRoleBufferManager.RemoveAttackBuffer(c);

            //去除防御的buffer
            DBRoleBufferManager.RemoveDefenseBuffer(c);

            //刷新战斗属性
            {
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.TimeAddDefense);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.TimeAddMDefense);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.TimeAddAttack);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.TimeAddMAttack);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.TimeAddDSAttack);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.PKKingBuffer);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.DSTimeShiDuNoShow);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.DSTimeAddLifeNoShow);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.DSTimeAddDefenseNoShow);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.DSTimeAddMDefenseNoShow);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.MU_LUOLANCHENGZHAN_QIZHI1);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.MU_LUOLANCHENGZHAN_QIZHI2);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.MU_LUOLANCHENGZHAN_QIZHI3);
            }

            // 属性改造 一级属性 [8/15/2013 LiaoWei]
            {
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.ADDTEMPStrength);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.ADDTEMPIntelligsence);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.ADDTEMPDexterity);
                DBRoleBufferManager.RefreshTimePropBuffer(c, BufferItemTypes.ADDTEMPConstitution);
            }

            //处理药品buffer，定时不计生命和蓝
            DBRoleBufferManager.ProcessTimeAddLifeMagic(c);

            //处理药品buffer，定时不计生命
            DBRoleBufferManager.ProcessTimeAddLifeNoShow(c);

            //处理药品buffer，定时不计蓝
            DBRoleBufferManager.ProcessTimeAddMagicNoShow(c);

            //处理道士加血buffer，定时不计生命
            DBRoleBufferManager.ProcessDSTimeAddLifeNoShow(c);

            //处理道士释放毒的buffer, 定时伤害
            DBRoleBufferManager.ProcessDSTimeSubLifeNoShow(c);

            //处理持续伤害的新的扩展buffer, 定时伤害
            DBRoleBufferManager.ProcessAllTimeSubLifeNoShow(c);

            AdvanceBufferPropsMgr.DoSpriteBuffers(c);

            long subTicks = 0;
            long ticks = TimeUtil.NOW();
            subTicks = ticks - c.LastProcessBufferTicks;

            //如果还没到时间，则跳过
            if (subTicks < (60 * 1000)) //生命和魔法储备不再使用， 武学和成就的激活1分钟的间隔足够了
            {
                return;
            }

            c.LastProcessBufferTicks = ticks;

            //处理生命和魔法储备
            //DBRoleBufferManager.ProcessLifeVAndMagicVReserve(sl, pool, c);

            //判断是否需要激活新的成就 武学 经脉buffer
            //Global.TryToActiveNewWuXueBuffer(c, true);    注释掉 [5/7/2014 LiaoWei]

            // 不需要再自动更新 ChengXiaojun
            //ChengJiuManager.TryToActiveNewChengJiuBuffer(c, true);

            /// 刷新初始化节日称号
            Global.RefreshJieriChengHao(c);
        }

        /// <summary>
        /// 处理角色的地图限制字段
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteMapLimitTimes(SocketListener sl, TCPOutPacketPool pool, KPlayer c)
        {
            long subTicks = 0;
            DateTime dateTime = TimeUtil.NowDateTime();
            long ticks = dateTime.Ticks / 10000;
            subTicks = ticks - c.LastProcessMapLimitTimesTicks;

            //如果还没到时间，则跳过
            if (subTicks < (60 * 1000))
            {
                return;
            }

            int elapsedSecs = (int)((ticks - c.LastProcessMapLimitTimesTicks) / 1000);
            c.LastProcessMapLimitTimesTicks = ticks;

            //判断是否超出了地图的时间限制
            if (!Global.CanMapInLimitTimes(c.MapCode, dateTime))
            {
                GameManager.ClientMgr.NotifyImportantMsg(sl, pool, c,
                    StringUtil.substitute(Global.GetLang("你在『{0}』地图中停留的时间超过了限制，被系统自动传回主城"), Global.GetMapName(c.MapCode)),
                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);

                GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                    c, GameManager.MainMapCode, -1, -1, -1);
            }

            //处理每日停留时间限制
            Global.ProcessDayLimitSecsByClient(c, elapsedSecs);
        }

        /// <summary>
        /// 处理角色的地图限时，角色使用某些道具到某个地图只能停留有限时间
        /// </summary>
        /// <param name="client"></param>
        private static void DoSpriteMapTimeLimit(KPlayer client)
        {
            long nowTicks = TimeUtil.NOW();
            long elapseTicks = nowTicks - client.LastMapLimitUpdateTicks;

            //判断上次限时地图更新判断的时间
            if (elapseTicks < 3000) //每3秒更新判断一次
            {
                return;
            }

            //冥界地图
            Global.ProcessMingJieMapTimeLimit(client, elapseTicks);

            //古墓地图
            Global.ProcessGuMuMapTimeLimit(client, elapseTicks);

            //记录本次的时间
            client.LastMapLimitUpdateTicks = nowTicks;
        }

        /// <summary>
        /// 处理角色登录的客户端的修订版本号低于服务器端版本号时, 每隔1分钟推送一次给用户,提示用户更新客户端
        /// </summary>
        /// <param name="client"></param>
        private static void DoSpriteHintToUpdateClient(KPlayer client)
        {
            long nowTicks = TimeUtil.NOW();
            long elapseTicks = nowTicks - client.LastHintToUpdateClientTicks;

            //判断上次限时地图更新判断的时间
            if (elapseTicks < (60 * 1000)) //每1分钟判断一次
            {
                return;
            }

            //记录本次的时间
            client.LastHintToUpdateClientTicks = nowTicks;

            int forceHintAppVer = GameManager.GameConfigMgr.GetGameConfigItemInt("hint-appver", 0);
            if (client.MainExeVer > 0 && client.MainExeVer < forceHintAppVer)
            {
                string msgID = "1";
                int minutes = 1;
                int playNum = 1;
                //string bulletinText = "服务器端检测到你的客户端版本过低, 请完全退出游戏后再启动自动更新, 如果没有自动更新, 请重新下载安装游戏!";
                string bulletinText = Global.GetLang("尊敬的用户，您当前的客户端版本过低可能会导致各种异常，建议您重新下载最新的客户端！");

                BulletinMsgData bulletinMsgData = new BulletinMsgData()
                {
                    MsgID = msgID,
                    PlayMinutes = minutes,
                    ToPlayNum = playNum,
                    BulletinText = bulletinText,
                    BulletinTicks = TimeUtil.NOW(),
                    MsgType = 0,
                };

                //将本条消息广播给所有在线的客户端
                GameManager.ClientMgr.NotifyBulletinMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, bulletinMsgData);
            }
            else if (client.ResVer > 0) //判断资源是否需要更新
            {
                int forceHintResVer = GameManager.GameConfigMgr.GetGameConfigItemInt("hint-resver", 0);
                if (client.ResVer < forceHintResVer)
                {
                    string msgID = "1";
                    int minutes = 1;
                    int playNum = 1;
                    //string bulletinText = "服务器端检测到你的客户端版本过低, 请完全退出游戏后再启动自动更新, 如果没有自动更新, 请重新下载安装游戏!";
                    string bulletinText = Global.GetLang("尊敬的用户，您当前的客户端游戏资源版本过低可能会导致无法游戏，建议您退出游戏后重新启动会自动更新到最新版本！");

                    BulletinMsgData bulletinMsgData = new BulletinMsgData()
                    {
                        MsgID = msgID,
                        PlayMinutes = minutes,
                        ToPlayNum = playNum,
                        BulletinText = bulletinText,
                        BulletinTicks = TimeUtil.NOW(),
                        MsgType = 0,
                    };

                    //将本条消息广播给所有在线的客户端
                    GameManager.ClientMgr.NotifyBulletinMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, bulletinMsgData);
                }
            }
        }

        /// <summary>
        /// 处理角色的物品限时
        /// </summary>
        /// <param name="client"></param>
        private static void DoSpriteGoodsTimeLimit(KPlayer client)
        {
            bool isFashion = false;
            bool isGoods = false;

            long nowTicks = TimeUtil.NOW();
            long elapseTicks = nowTicks - client.LastGoodsLimitUpdateTicks;
            long fashionTicks = nowTicks - client.LastFashionLimitUpdateTicks;

            List<GoodsData> expiredList = null;

            //判断上次限时地图更新判断的时间
            if (elapseTicks >= 30 * 1000) //每30秒更新判断一次
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
                //这儿进行物品摧毁操作
                for (int n = 0; n < expiredList.Count; n++)
                {
                    GoodsData goods = expiredList[n];
                    if (Global.DestroyGoods(client, goods))
                    {
                        Global.SendMail(client, Global.GetLang("物品过期通知"), string.Format(
                            Global.GetLang("限时物品【{0}】已过期，自动销毁"), Global.GetGoodsNameByID(goods.GoodsID)));
                    }
                }
            }

            if (isGoods) client.LastGoodsLimitUpdateTicks = nowTicks;

            if (isFashion) client.LastFashionLimitUpdateTicks = nowTicks;
        }

        /// <summary>
        /// Hàm này thực hiện Logic cập nhật vị trí người chơi để kiểm tra các đối tượng xung quanh
        /// </summary>
        /// <param name="client"></param>
        public static void DoSpriteMapGridMove(KPlayer client, long extTicks = 1000)
        {
            long ticks = TimeUtil.NOW();

            lock (client.Current9GridMutex)
            {
                long slotTicks = 500;

                if (ticks - client.LastRefresh9GridObjectsTicks >= slotTicks)
                {
                    client.LastRefresh9GridObjectsTicks = ticks;
                    Global.GameClientMoveGrid(client);
                }
            }
        }

        /// <summary>
        // 处理冥想计时 [3/18/2014 LiaoWei]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteMeditateTime(KPlayer c)
        {
            long lTicks = 0;
            long lCurrticks = TimeUtil.NOW();

            lTicks = lCurrticks - c.MeditateTicks;

            if (lTicks < 10 * 1000) //每10秒判断是否可进入冥想状态
            {
                return;
            }

            //判断是否可自动进入冥想状态
            //if (c.StartMeditate <= 0 && c.MeditateTime + c.NotSafeMeditateTime < Global.ConstMaxMeditateTime)
            if (c.StartMeditate <= 0)
            {
                if (c.LastMovePosTicks == 0 || c.Last10sPosX != c.PosX || c.Last10sPosY != c.PosY)
                {
                    c.Last10sPosX = c.PosX;
                    c.Last10sPosY = c.PosY;
                    c.LastMovePosTicks = lCurrticks;
                }
                //else if (c.StallDataItem != null)
                //{
                //    //摆摊状态不进入冥想
                //}
                else if (!GlobalNew.IsGongNengOpened(c, GongNengIDs.MingXiang))
                {
                    //未达到冥想功能开启等级
                }
                else if (lCurrticks - c.LastMovePosTicks > 60 * 1000)
                {
                    Global.StartMeditate(c);
                    lTicks = 60 * 1000; //强制下面的代码执行,以便同步刷新
                }
            }

            // 每分钟计时一次
            if (lTicks < (60 * 1000))
            {
                return;
            }

            c.MeditateTicks = lCurrticks;

            //是否进入了冥想状态
            if (c.StartMeditate <= 0)
            {
                return;
            }

            // 判断是否在安全区中
            bool bIsInsafeArea = true;

            //总是算做安全区时间,这样避免导致每次下线后总时间回到11:59分
            //GameMap gameMap = null;
            //if (GameManager.MapMgr.DictMaps.TryGetValue(c.MapCode, out gameMap))
            //    bIsInsafeArea = gameMap.InSafeRegionList(c.CurrentGrid);

            if (bIsInsafeArea)
            {
                int nTime = Global.GetRoleParamsInt32FromDB(c, RoleParamName.MeditateTime);
                int nTime2 = Global.GetRoleParamsInt32FromDB(c, RoleParamName.NotSafeMeditateTime);
                if ((nTime + nTime2) < Global.ConstMaxMeditateTime)
                {
                    long msecs = Math.Max(lCurrticks - c.BiGuanTime, 0);
                    msecs = Math.Min(msecs + nTime, Global.ConstMaxMeditateTime - nTime2);   // 12个小时

                    c.MeditateTime = (int)msecs;
                    Global.SaveRoleParamsInt32ValueToDB(c, RoleParamName.MeditateTime, (int)msecs, false);
                }
            }
            else
            {
                int nTime = Global.GetRoleParamsInt32FromDB(c, RoleParamName.MeditateTime);
                int nTime2 = Global.GetRoleParamsInt32FromDB(c, RoleParamName.NotSafeMeditateTime);

                if ((nTime + nTime2) < Global.ConstMaxMeditateTime)
                {
                    long msecs = Math.Max(lCurrticks - c.BiGuanTime, 0);
                    msecs = Math.Min(msecs + nTime2, Global.ConstMaxMeditateTime - nTime);   // 12个小时

                    c.NotSafeMeditateTime = (int)msecs;
                    Global.SaveRoleParamsInt32ValueToDB(c, RoleParamName.NotSafeMeditateTime, (int)msecs, false);
                }
            }

            // 重置时间
            c.BiGuanTime = lCurrticks;

            GameManager.ClientMgr.NotifyMeditateTime(c);

            return;
        }

        /// <summary>
        // 处理死亡计时 [9/23/2014 lt]
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        private void DoSpriteDeadTime(KPlayer c)
        {
            long lTicks = 0;
            long lCurrticks = TimeUtil.NOW();
            lTicks = lCurrticks - c.LastProcessDeadTicks;
            if (c.m_CurrentLife > 0 || lTicks < 3 * 1000) //每3秒判断是否应处理死亡状态
            {
                return;
            }

            c.LastProcessDeadTicks = lCurrticks;
            //if (c.LastRoleDeadTicks + 12000 < lCurrticks) //死亡12秒内,不触发死亡复活容错自动复活机制
            //{
            //    return;
            //}
            ProcessSpriteDead(c, lCurrticks);
        }

        private void ProcessSpriteDead(KPlayer client, long nowTicks)
        {
            int posX = -1, posY = -1;

            //如果是超时机制复活，需要判断死亡时间是否超过特定时间
            if ((int)RoleReliveTypes.TimeWaiting == Global.GetRoleReliveType(client) || (int)RoleReliveTypes.TimeWaitingRandomAlive == Global.GetRoleReliveType(client))
            {
                long elapseTicks = nowTicks - client.LastRoleDeadTicks;
                if (elapseTicks / 1000 < Global.GetRoleReliveWaitingSecs(client) + 3000)
                {
                    return;
                }
            }
            else if ((int)RoleReliveTypes.TimeWaitingOrRelifeNow == Global.GetRoleReliveType(client))
            {
                long elapseTicks = TimeUtil.NOW() - client.LastRoleDeadTicks;
                if (elapseTicks / 1000 < Global.GetRoleReliveWaitingSecs(client) + 3000)
                {
                    return;
                }
                posX = -1;
                posY = -1;
            }
            else if ((int)RoleReliveTypes.HomeOrHere == Global.GetRoleReliveType(client))
            {
                if (nowTicks - client.LastRoleDeadTicks < 35000)
                {
                    return;
                }
            }
            else if ((int)RoleReliveTypes.Home == Global.GetRoleReliveType(client))
            {
                if (nowTicks - client.LastRoleDeadTicks < 5000)
                {
                    return;
                }
            }
            else
            {
                return; //地图编号为-1或未来新加的复活方式,以后再加
            }

            //如果是在皇城地图上
            if (Global.IsHuangChengMapCode(client.MapCode) || Global.IsHuangGongMapCode(client.MapCode))
            {
                posX = -1;
                posY = -1;
            }

            //如果玩家在炎黄战场内，则强行传送回本阵营复活点复活
            if (Global.IsBattleMap(client))
            {
                int toMapCode = GameManager.BattleMgr.BattleMapCode;
                GameMap gameMap = null;
                if (GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out gameMap)) //确认地图编号是否有效
                {
                    client.m_CurrentLife = client.m_CurrentLifeMax;
                    client.m_CurrentMana = client.m_CurrentManaMax;

                    int defaultBirthPosX = gameMap.DefaultBirthPosX;
                    int defaultBirthPosY = gameMap.DefaultBirthPosY;
                    int defaultBirthRadius = gameMap.BirthRadius;

                    Global.GetBattleMapPos(client, ref defaultBirthPosX, ref defaultBirthPosY, ref defaultBirthRadius);

                    //从配置根据地图取默认位置
                    Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, toMapCode, defaultBirthPosX, defaultBirthPosY, defaultBirthRadius);
                    posX = (int)newPos.X;
                    posY = (int)newPos.Y;

                    //角色复活
                    Global.ClientRealive(client, posX, posY, client.RoleDirection);
                }

                //只要进入这个分支，强行返回 ok
                return;
            }

            /// 是否是领地战地图
            if (Global.IsLingDiZhanMapCode(client))
            {
                int toMapCode = client.MapCode;
                GameMap gameMap = null;
                if (GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out gameMap)) //确认地图编号是否有效
                {
                    client.m_CurrentLife = client.m_CurrentLifeMax;
                    client.m_CurrentMana = client.m_CurrentManaMax;

                    //随机点
                    Point newPos = Global.GetRandomPoint(ObjectTypes.OT_CLIENT, toMapCode);

                    posX = (int)newPos.X;
                    posY = (int)newPos.Y;

                    //角色复活
                    Global.ClientRealive(client, posX, posY, client.RoleDirection);
                }

                //只要进入这个分支，强行返回 ok
                return;
            }

            //竞技场决斗赛死亡后，强制回城复活
            if (GameManager.ArenaBattleMgr.IsInArenaBattle(client))
            {
                posX = -1;
                posY = -1;
            }

            //如果是回城复活
            if (posX == -1 || posY == -1)
            {
                // 复活改造 [3/19/2014 LiaoWei]
                /*int toMapCode = GameManager.MainMapCode;
                //if (client.MapCode == GameManager.DefaultMapCode) //新手村死亡后，回城复活，是回新手村得出生点，而不是扬州城
                //某些地图回城复活不回主城，回本地图复活点
                if (GameManager.systemParamsList.GetParamValueIntArrayByName("MainReliveCity").ToList<int>().IndexOf(client.MapCode) >= 0)
                {
                    toMapCode = client.MapCode;
                }*/
                int toMapCode = -1;
                toMapCode = Global.GetMapRealiveInfoByCode(client.MapCode);

                // 保证能回到主城
                if (toMapCode <= -1)
                {
                    toMapCode = GameManager.MainMapCode;
                }
                else
                {
                    if (toMapCode == 0 || GameManager.ArenaBattleMgr.IsInArenaBattle(client))
                        toMapCode = GameManager.MainMapCode;
                    else if (toMapCode == 1)
                        toMapCode = client.MapCode;
                }

                //现在没有坐牢机制
                /*if (client.MapCode == Global.GetLaoFangMapCode()) //牢房中(死亡同时被传入牢房)死亡后，回城复活，是回牢房的得出生点，而不是扬州城
                {
                    toMapCode = Global.GetLaoFangMapCode();
                }*/

                if (toMapCode >= 0)
                {
                    GameMap gameMap = null;
                    if (GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out gameMap)) //确认地图编号是否有效
                    {
                        int defaultBirthPosX = GameManager.MapMgr.DictMaps[toMapCode].DefaultBirthPosX;
                        int defaultBirthPosY = GameManager.MapMgr.DictMaps[toMapCode].DefaultBirthPosY;
                        int defaultBirthRadius = GameManager.MapMgr.DictMaps[toMapCode].BirthRadius;

                        //从配置根据地图取默认位置
                        Point newPos = Global.GetMapPoint(ObjectTypes.OT_CLIENT, toMapCode, defaultBirthPosX, defaultBirthPosY, defaultBirthRadius);
                        posX = (int)newPos.X;
                        posY = (int)newPos.Y;

                        client.m_CurrentLife = client.m_CurrentLifeMax;
                        client.m_CurrentMana = client.m_CurrentManaMax;

                        client.MoveAndActionNum = 0;

                        //通知队友自己要复活
                        GameManager.ClientMgr.NotifyTeamRealive(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.RoleID, posX, posY, client.RoleDirection);

                        //马上通知切换地图---->这个函数每次调用前，如果地图未发生发变化，则直接通知其他人自己位置变动
                        //比如在扬州城死 回 扬州城复活，就是位置变化
                        if (toMapCode != client.MapCode)
                        {
                            //通知自己要复活
                            GameManager.ClientMgr.NotifyMySelfRealive(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, client.RoleID, client.PosX, client.PosY, client.RoleDirection);

                            GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, toMapCode, posX, posY, -1, 1);
                        }
                        else
                        {
                            Global.ClientRealive(client, posX, posY, client.RoleDirection);
                            //NotifyMySelfRealive
                        }

                        //LogManager.WriteLog(LogTypes.Error, string.Format("成功处理复活通知1, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                        return;
                    }
                }

                //LogManager.WriteLog(LogTypes.Error, string.Format("成功处理复活通知2, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                return;
            }
        }

        /// <summary>
        /// 处理角色的后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteWorks(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            //long startTicks = TimeUtil.NOW();

            // 更新在线时长
            DoSpriteHeart(sl, pool, client);

            //自动挂机的调度
            DoSpriteAutoFight(sl, pool, client);

            // 处理打坐收益
            DoSpriteSitExp(sl, pool, client);

            //处理消减PK点
            DoSpriteSubPKPoint(sl, pool, client);

            //播报紫名的消失事件
            BroadcastRolePurpleName(sl, pool, client);

            //执行角色排队的命令
            Global.ProcessQueueCmds(client);

            //处理通知角斗场称号的超时
            ProcessRoleBattleNameInfoTimeOut(sl, pool, client);

            //重新计算按照日来判断的登录次数
            ChangeDayLoginNum(client);

            //处理角色的心跳时间, 如果超时，则执行清除工作
            Global.ProcessClientHeart(client);

            //处理角色的地图限制字段
            DoSpriteMapLimitTimes(sl, pool, client);

            //处理地图限时
            DoSpriteMapTimeLimit(client);

            //提示客户端更新版本
            DoSpriteHintToUpdateClient(client);

            //处理物品使用限时
            DoSpriteGoodsTimeLimit(client);

            // 冥想计时处理 [3/18/2014 LiaoWei]
            DoSpriteMeditateTime(client);

            //处理超时的死亡状态,强制他复活
            DoSpriteDeadTime(client);

            // 处理叹号的定时
            client._IconStateMgr.DoSpriteIconTicks(client);

            // 处理群邮件的定时
            GroupMailManager.CheckRoleGroupMail(client);

            GetInterestingDataMgr.Instance().Update(client);

            //updateEveryDayData(client);
            //long endTicks = TimeUtil.NOW();
            //System.Diagnostics.Debug.WriteLine(string.Format("DoSpriteWorks 消耗: {0} 毫秒", endTicks - startTicks));
        }

        //更新用户的每日数据//是否需要锁？
        //public void updateEveryDayData(KPlayer client)
        //{
        //    int today = TimeUtil.NowDateTime().DayOfYear;
        //    if (today == client.EveryDayUpDate) return;
        //    client.EveryDayUpDate = today;

        //    UnionPalaceManager.getInstance().initTodayData(client);
        //    PetSkillManager.getInstance().initTodayData(client);
        //}

        /// <summary>
        /// 角色后台工作
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

                /// 处理角色的后台工作
                DoSpriteWorks(sl, pool, client);
            }
        }

        /// <summary>
        /// 处理角色buffers的后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoBuffersWorks(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            // 补血补魔
            DoSpriteLifeMagic(sl, pool, client);

            //处理DBBuffer中的项
            DoSpriteBuffers(sl, pool, client);

            //定时检查道术隐身装备
            CheckDSHideState(client);
        }

        /// <summary>
        /// 处理角色Extension的后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoBuffersExtension(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            long nowTicks = TimeUtil.NOW();

            //Thực hiện tấn công nhiều giai đoạn
            if (GameManager.FlagManyAttackOp)
                SpriteAttack.ExecMagicsManyTimeDmageQueueEx(client);
            else
                SpriteAttack.ExecMagicsManyTimeDmageQueue(client);

            //Buff类
            client.bufferPropsManager.TimerUpdateProps(nowTicks);

            //延时执行的代码
            client.delayExecModule.ExecDelayProcs(client);

            // Thực hiện các hiệu ứng slow stun vvv
            SpriteMagicHelper.ExecuteAllItems(client);
        }

        /// <summary>
        /// 处理角色DB的后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoDBWorks(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            DoSpriteDBHeart(sl, pool, client);

            // Lưu các thông khác vào DB
            GameDb.ProcessDBCmdByTicks(client, false);

            //Lưu thông tin về skill vào DB
            GameDb.ProcessDBSkillCmdByTicks(client);

            //Lưu các thông tin Pram vào DB
            GameDb.ProcessDBRoleParamCmdByTicks(client, false);

            //Lưu độ bền trang bị vào DB
            GameDb.ProcessDBEquipStrongCmdByTicks(client, false);
        }

        /// <summary>
        /// Thực thi BUFF cho tất cả người chơi
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteBuffersWork(SocketListener sl, TCPOutPacketPool pool)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (client.ClosingClientStep > 0)
                {
                    continue;
                }

                /// 处理角色的后台工作
                DoBuffersWorks(sl, pool, client);

                //  KTRoleBuffManager.Process(sl, pool, client);
            }
        }

        /// <summary>
        /// 角色Extension后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteExtensionWork(SocketListener sl, TCPOutPacketPool pool, int nThead, int nMaxThread)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                if (nMaxThread > 0)
                {
                    //判断如果角色的角色ID的2的模是否等于线程编号
                    if (client.RoleID % nMaxThread != nThead)
                    {
                        continue;
                    }
                }
                if (client.ClosingClientStep > 0)
                {
                    continue;
                }

                /// Thực thi các hiệu ứng SLOW cho client
                DoBuffersExtension(sl, pool, client);
            }
        }

        /// <summary>
        /// 角色Extension后台工作，分地图
        /// </summary>
        /// <param name="client"></param>
        public void DoSpriteExtensionWorkByPerMap(int mapCode = -1, int subMapCode = -1)
        {
            SocketListener sl = Global._TCPManager.MySocketListener;
            TCPOutPacketPool tp = Global._TCPManager.TcpOutPacketPool;

            List<Object> mapClients = GameManager.ClientMgr.GetMapClients(mapCode);
            if (null == mapClients || mapClients.Count == 0)
            {
                return;
            }

            foreach (Object obj in mapClients)
            {
                if (null == obj)
                {
                    continue;
                }

                KPlayer client = obj as KPlayer;
                if (null == client)
                {
                    continue;
                }

                // 只处理当前副本地图的
                if (subMapCode >= 0 && client.CopyMapID != subMapCode)
                {
                    continue;
                }

                if (client.ClosingClientStep > 0)
                {
                    continue;
                }

                /// 处理角色的扩展工作
                DoBuffersExtension(sl, tp, client);
            }
        }

        /// <summary>
        /// 角色DB指令后台工作
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
        /// 角色的定时舞台对象刷新后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoSpritesMapGridMove(int nThead)
        {
            if (GameManager.Update9GridUsingPosition > 0)
            {
                return;
            }

            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                //判断如果角色的角色ID的2的模是否等于线程编号
                if (client.RoleID % Program.MaxGird9UpdateWorkersNum != nThead)
                {
                    continue;
                }

                //处理角色的地图移动延迟刷新
                DoSpriteMapGridMove(client);

                //故意降低cpu消耗
                if (GameManager.MaxSleepOnDoMapGridMoveTicks > 0)
                {
                    Thread.Sleep(GameManager.MaxSleepOnDoMapGridMoveTicks);
                }
            }
        }

        /// <summary>
        /// 角色的定时舞台对象刷新后台工作
        /// </summary>
        /// <param name="client"></param>
        public void DoSpritesMapGridMoveNewMode(int nThead)
        {
            KPlayer client = null;
            int index = 0;
            while ((client = GetNextClient(ref index)) != null)
            {
                //判断如果角色的角色ID的2的模是否等于线程编号
                if (client.RoleID % Program.MaxGird9UpdateWorkersNum != nThead)
                {
                    continue;
                }

                //处理角色的地图移动延迟刷新
                Global.GameClientMoveGrid(client);
            }
        }

        #endregion 后台工作线程调用方法

        #region 血色堡垒

        /// <summary>
        /// 通知在线的所有人(仅限在血色堡垒地图上)血色堡垒邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBloodCastleMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode, int nCmdID, int nTimer = 0, int nValue = 0, int nType = 0, int nPlayerNum = 0)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEBEGINFIGHT)
            {
                strcmd = string.Format("{0}:{1}", mapCode, nTimer);  // 1.mapID 2.时间(秒)
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEKILLMONSTERSTATUS)
            {
                strcmd = string.Format("{0}:{1}", nValue, nType);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, nCmdID);
        }

        /// <summary>
        /// 通知在线的所有人(仅限在血色堡垒地图上)血色堡垒邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBloodCastleCopySceneMsg(SocketListener sl, TCPOutPacketPool pool, CopyMap mapInfo, int nCmdID, int nTimer = 0, int nValue = 0, int nType = 0, int nPlayerNum = 0, KPlayer client = null)
        {
            List<KPlayer> objsList = mapInfo.GetClientsList(); //发送给所有地图的用户
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEBEGINFIGHT)
            {
                strcmd = string.Format("{0}:{1}", mapInfo.FubenMapID, nTimer);  // 1.mapID 2.时间(秒)
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEKILLMONSTERSTATUS)
            {
                strcmd = string.Format("{0}:{1}", nValue, nType);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEPREPAREFIGHT)
            {
                BloodCastleDataInfo bcDataTmp = null;

                if (!Data.BloodCastleDataInfoList.TryGetValue(mapInfo.FubenMapID, out bcDataTmp) || bcDataTmp == null)
                    return;

                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", mapInfo.FubenMapID, nTimer, bcDataTmp.NeedKillMonster1Num, 1, bcDataTmp.NeedKillMonster2Num, 1, 1, 1);
            }

            //群发消息
            for (int i = 0; i < objsList.Count; i++)
            {
                if (objsList[i] != client)
                {
                    SendToClient(sl, pool, objsList[i], strcmd, nCmdID);
                }
            }

            if (null != client)
            {
                SendToClient(sl, pool, client, strcmd, nCmdID);
            }
        }

        /// <summary>
        /// 通知在线的所有人(仅限在血色堡垒地图上)血色堡垒结束战斗
        /// </summary>
        /// <param name="client"></param>
        public void NotifyBloodCastleCopySceneMsgEndFight(SocketListener sl, TCPOutPacketPool pool, CopyMap mapInfo, BloodCastleScene bcTmp, int nCmdID, int nTimer, int nTimeAward)
        {
            string strcmd = "";

            BloodCastleDataInfo bcDataTmp = null;

            //bcDataTmp = Data.BloodCastleDataInfoList[mapCode];
            if (!Data.BloodCastleDataInfoList.TryGetValue(mapInfo.FubenMapID, out bcDataTmp))
                return;

            if (bcTmp == null || bcDataTmp == null)
                return;

            bcTmp.m_bEndFlag = true;

            List<KPlayer> objsList = mapInfo.GetClientsList(); //发送给所有地图的用户
            if (null == objsList)
                return;

            for (int i = 0; i < objsList.Count; ++i)
            {
                KPlayer client = objsList[i];

                if (client.FuBenID > 0 && !GameManager.BloodCastleCopySceneMgr.IsBloodCastleCopyScene(client.FuBenID))
                    continue;

                string AwardItem1 = null;
                string AwardItem2 = null;

                client.BloodCastleAwardPoint += nTimeAward;
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.BloodCastlePlayerPoint, client.BloodCastleAwardPoint, true);

                if (client.RoleID == bcTmp.m_nRoleID)
                {
                    for (int j = 0; j < bcDataTmp.AwardItem1.Length; ++j)
                    {
                        AwardItem1 += bcDataTmp.AwardItem1[j];
                        if (j != bcDataTmp.AwardItem1.Length - 1)
                            AwardItem1 += "|";
                    }
                }

                for (int n = 0; n < bcDataTmp.AwardItem2.Length; ++n)
                {
                    AwardItem2 += bcDataTmp.AwardItem2[n];
                    if (n != bcDataTmp.AwardItem2.Length - 1)
                        AwardItem2 += "|";
                }

                int nFlag = 0;
                if (bcTmp.m_bIsFinishTask)
                    nFlag = 1;

                // 保存完成状态
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.BloodCastleSceneFinishFlag, nFlag, true);

                // 1.离场倒计时开始 2.是否成功完成 3.玩家的积分 4.玩家经验奖励 5.玩家的金钱奖励 6.玩家物品奖励1(只有提交大天使武器的玩家才有 其他人为null) 7.玩家物品奖励2(通用奖励 大家都有的)
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", nTimer, nFlag, client.BloodCastleAwardPoint, Global.CalcExpForRoleScore(client.BloodCastleAwardPoint, bcDataTmp.ExpModulus),
                                        client.BloodCastleAwardPoint * bcDataTmp.MoneyModulus, AwardItem1, AwardItem2);

                GameManager.ClientMgr.SendToClient(client, strcmd, nCmdID);
            }
        }

        #endregion 血色堡垒

        #region 恶魔广场

        /// <summary>
        /// 恶魔广场广播信息(仅限在恶魔广场地图上)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDaimonSquareMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode, int nCmdID, int nSection, int nTimer, int nWave, int nNum, int nPlayerNum)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_QUERYDAIMONSQUAREMONSTERWAVEANDPOINTRINFO)
            {
                strcmd = string.Format("{0}:{1}", nWave, nNum);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_QUERYDAIMONSQUARETIMERINFO)
            {
                strcmd = string.Format("{0}:{1}", nSection, nTimer);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_DAIMONSQUAREPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, nCmdID);
        }

        /// <summary>
        /// 通知在线的所有人(仅限在血色堡垒地图上)血色堡垒邀请消息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDaimonSquareCopySceneMsg(SocketListener sl, TCPOutPacketPool pool, CopyMap mapInfo, int nCmdID, int nTimer = 0, int nValue = 0, int nType = 0, int nPlayerNum = 0)
        {
            List<KPlayer> objsList = mapInfo.GetClientsList(); //发送给所有地图的用户
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_DAIMONSQUAREPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }

            //群发消息
            for (int i = 0; i < objsList.Count; i++)
                SendToClient(sl, pool, objsList[i], strcmd, nCmdID);
        }

        /// <summary>
        /// 恶魔广场副本广播信息(仅限在恶魔广场地图上)
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDaimonSquareCopySceneMsg(SocketListener sl, TCPOutPacketPool pool, CopyMap mapInfo, int nCmdID, int nSection, int nTimer, int nWave, int nNum, int nPlayerNum)
        {
            List<KPlayer> objsList = mapInfo.GetClientsList();
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_QUERYDAIMONSQUAREMONSTERWAVEANDPOINTRINFO)
            {
                strcmd = string.Format("{0}:{1}", nWave, nNum);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_QUERYDAIMONSQUARETIMERINFO)
            {
                strcmd = string.Format("{0}:{1}", nSection, nTimer);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_DAIMONSQUAREPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }

            //群发消息
            for (int i = 0; i < objsList.Count; i++)
                SendToClient(sl, pool, objsList[i], strcmd, nCmdID);
        }

        /// <summary>
        /// 通知在线的所有人(仅限在血色堡垒地图上)血色堡垒结束战斗
        /// </summary>
        /// <param name="client"></param>
        public void NotifyDaimonSquareCopySceneMsgEndFight(SocketListener sl, TCPOutPacketPool pool, CopyMap mapInfo, DaimonSquareScene dsInfo, int nCmdID, int nTimeAward)
        {
            string strcmd = "";

            DaimonSquareDataInfo bcDataTmp = null;

            if (!Data.DaimonSquareDataInfoList.TryGetValue(mapInfo.FubenMapID, out bcDataTmp))
                return;

            if (dsInfo == null || bcDataTmp == null)
                return;

            dsInfo.m_bEndFlag = true;

            List<KPlayer> objsList = mapInfo.GetClientsList(); //发送给所有地图的用户
            if (null == objsList)
                return;

            for (int i = 0; i < objsList.Count; ++i)
            {
                if (!(objsList[i] is KPlayer))
                    continue;

                KPlayer client = (objsList[i] as KPlayer);

                if (client.FuBenID > 0 && !GameManager.DaimonSquareCopySceneMgr.IsDaimonSquareCopyScene(client.FuBenID))
                    continue;

                string sAwardItem = null;

                client.DaimonSquarePoint += nTimeAward;
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.DaimonSquarePlayerPoint, client.DaimonSquarePoint, true);

                for (int n = 0; n < bcDataTmp.AwardItem.Length; ++n)
                {
                    sAwardItem += bcDataTmp.AwardItem[n];
                    if (n != bcDataTmp.AwardItem.Length - 1)
                        sAwardItem += "|";
                }

                int nFlag = 0;
                if (dsInfo.m_bIsFinishTask)
                    nFlag = 1;

                // 1.是否成功完成 2.玩家的积分 3.玩家经验奖励 4.玩家的金钱奖励 5.玩家物品奖励
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", nFlag, client.DaimonSquarePoint, Global.CalcExpForRoleScore(client.DaimonSquarePoint, bcDataTmp.ExpModulus),
                                        client.DaimonSquarePoint * bcDataTmp.MoneyModulus, sAwardItem);

                GameManager.ClientMgr.SendToClient(client, strcmd, nCmdID);
            }
        }

        #endregion 恶魔广场

        #region 天使神殿

        /// <summary>
        /// 通知在线的所有人(仅限在天使神殿地图上)一些信息
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAngelTempleMsg(SocketListener sl, TCPOutPacketPool pool, int mapCode, int nCmdID, AngelTemplePointInfo[] array, int nSection, int nTimer = 0, int nValue = 0, int nType = 0, int nPlayerNum = 0, double nBossHP = 0)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList)
                return;

            string strcmd = "";

            if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_ANGELTEMPLETIMERINFO)
            {
                strcmd = string.Format("{0}:{1}", nSection, nTimer);  // 1.哪个时间段 2.时间(秒)
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEKILLMONSTERSTATUS)
            {
                strcmd = string.Format("{0}:{1}", nValue, nType);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_BLOODCASTLEPLAYERNUMNOTIFY)
            {
                strcmd = string.Format("{0}", nPlayerNum);
            }
            else if (nCmdID == (int)TCPGameServerCmds.CMD_SPR_ANGELTEMPLEFIGHTINFOALL)
            {
                string sName1 = "";
                string sName2 = "";
                string sName3 = "";
                string sName4 = "";
                string sName5 = "";

                double dValue1 = 0.0;
                double dValue2 = 0.0;
                double dValue3 = 0.0;
                double dValue4 = 0.0;
                double dValue5 = 0.0;

                dValue1 = Math.Round(((double)array[0].m_DamagePoint / (double)GameManager.AngelTempleMgr.m_BossHP), 2);
                sName1 = array[0].m_RoleName;
                dValue2 = Math.Round(((double)array[1].m_DamagePoint / (double)GameManager.AngelTempleMgr.m_BossHP), 2);
                sName2 = array[1].m_RoleName;
                dValue3 = Math.Round(((double)array[2].m_DamagePoint / (double)GameManager.AngelTempleMgr.m_BossHP), 2);
                sName3 = array[2].m_RoleName;
                dValue4 = Math.Round(((double)array[3].m_DamagePoint / (double)GameManager.AngelTempleMgr.m_BossHP), 2);
                sName4 = array[3].m_RoleName;
                dValue5 = Math.Round(((double)array[4].m_DamagePoint / (double)GameManager.AngelTempleMgr.m_BossHP), 2);
                sName5 = array[4].m_RoleName;

                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}", Math.Round(nBossHP / GameManager.AngelTempleMgr.m_BossHP, 2), sName1, dValue1, sName2, dValue2, sName3, dValue3, sName4, dValue4, sName5, dValue5);
            }

            //群发消息
            SendToClients(sl, pool, null, objsList, strcmd, nCmdID);
        }

        /// <summary>
        /// 通知在线的所有人(仅限在天使神殿地图上)天使神殿Boss消失
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAngelTempleMsgBossDisappear(SocketListener sl, TCPOutPacketPool pool, int mapCode)
        {
            List<Object> objsList = Container.GetObjectsByMap(mapCode); //发送给所有地图的用户
            if (null == objsList)
                return;

            for (int i = 0; i < objsList.Count; ++i)
            {
                if (!(objsList[i] is KPlayer))
                    continue;

                KPlayer client = (objsList[i] as KPlayer);

                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                        StringUtil.substitute(Global.GetLang("Boss未被击杀已自动消失")),
                                                                        GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.None);
            }
        }

        #endregion 天使神殿

        #region 队伍广播

        /// <summary>
        /// 通知队伍有人加入、离开
        /// </summary>
        /// <param name="client"></param>
        public void NotifyTeamMemberMsg(SocketListener sl, TCPOutPacketPool pool, KPlayer client, TeamData td, TeamCmds nCmd)
        {
            if (null != td)
            {
                lock (td)
                {
                    for (int i = 0; i < td.TeamRoles.Count; i++)
                    {
                        KPlayer gameClient = FindClient(td.TeamRoles[i].RoleID);
                        if (null == gameClient)
                            continue;

                        if (nCmd == TeamCmds.Quit)
                        {
                            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, gameClient,
                                                                        StringUtil.substitute(Global.GetLang("『{0}』离开了队伍"), client.RoleName),
                                                                        GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyChatBox, (int)HintErrCodeTypes.None);
                        }
                        else if (nCmd == TeamCmds.AgreeApply)
                        {
                            GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, gameClient,
                                                                        StringUtil.substitute(Global.GetLang("『{0}』加入了队伍"), client.RoleName),
                                                                        GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyChatBox, (int)HintErrCodeTypes.None);
                        }
                    }
                }
            }
        }

        #endregion 队伍广播

        #region 取得地图当中的玩家列表

        /// <summary>
        /// 根据玩家所在地图 取得所有玩家
        /// </summary>
        /// <param name="client"></param>
        public List<Object> GetPlayerByMap(KPlayer client)
        {
            List<Object> newObjList = null;

            newObjList = Container.GetObjectsByMap(client.MapCode);

            return newObjList;
        }

        #endregion 取得地图当中的玩家列表

        #region 仓库货币处理

        /// <summary>
        /// 银两通知(只通知自己)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserStoreYinLiangChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StoreYinLiang);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_STORE_YINLIANG_CHANGE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// 银两通知(只通知自己)
        /// </summary>
        /// <param name="client"></param>
        public void NotifySelfUserStoreMoneyChange(SocketListener sl, TCPOutPacketPool pool, KPlayer client)
        {
            string strcmd = string.Format("{0}:{1}", client.RoleID, client.StoreMoney);
            TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_SPR_STORE_MONEY_CHANGE);
            if (!sl.SendData(client.ClientSocket, tcpOutPacket))
            {
            }
        }

        /// <summary>
        /// 添加用户仓库金币
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserStoreYinLiang(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, long addYinLiang, string strFrom)
        {
            if (0 == addYinLiang)
            {
                return true;
            }

            long oldYinLiang = client.StoreYinLiang;

            //先锁定
            lock (client.StoreYinLiangMutex)
            {
                if (addYinLiang < 0 && oldYinLiang < Math.Abs(addYinLiang))
                {
                    return false;
                }

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, addYinLiang); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_ADD_STORE_YINLIANG, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt64(dbFields[1]) < 0)
                {
                    return false; //银两添加失败
                }

                // 先锁定
                client.StoreYinLiang = Convert.ToInt64(dbFields[1]);

                // 更新通知(只通知自己)
                GameManager.ClientMgr.NotifySelfUserStoreYinLiangChange(sl, pool, client);
                if (0 != addYinLiang)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "仓库金币", strFrom, "系统", client.RoleName, "增加", (int)addYinLiang, client.ZoneID, client.strUserID, (int)client.StoreYinLiang, client.ServerId);
                    EventLogManager.AddMoneyEvent(client, OpTypes.AddOrSub, OpTags.None, MoneyTypes.StoreYinLiang, addYinLiang, client.StoreYinLiang, strFrom);
                }
            }

            //写入角色银两增加/减少日志
            Global.AddRoleStoreYinLiangEvent(client, oldYinLiang);

            return true;
        }

        /// <summary>
        /// 添加用户仓库绑定金币
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="subMoney"></param>
        /// <returns></returns>
        public bool AddUserStoreMoney(SocketListener sl, TCPClientPool tcpClientPool, TCPOutPacketPool pool, KPlayer client, long addMoney, string strFrom)
        {
            if (0 == addMoney)
            {
                return true;
            }

            long oldMoney = client.StoreMoney;

            //先锁定
            lock (client.StoreMoneyMutex)
            {
                if (addMoney < 0 && oldMoney < Math.Abs(addMoney))
                {
                    return false;
                }

                //先DBServer请求扣费
                string strcmd = string.Format("{0}:{1}", client.RoleID, addMoney); //只发增量
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_ADD_STORE_MONEY, strcmd, client.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }

                // 先锁定
                if (Convert.ToInt64(dbFields[1]) < 0)
                {
                    return false; //银两添加失败
                }

                // 先锁定
                client.StoreMoney = Convert.ToInt64(dbFields[1]);

                if (0 != addMoney)
                {
                    GameManager.logDBCmdMgr.AddDBLogInfo(-1, "仓库绑定金币", strFrom, "系统", client.RoleName, "增加", (int)addMoney, client.ZoneID, client.strUserID, (int)client.StoreMoney, client.ServerId);
                }
            }

            // 更新通知(只通知自己)
            GameManager.ClientMgr.NotifySelfUserStoreMoneyChange(sl, pool, client);

            //写入角色银两增加/减少日志
            Global.AddRoleStoreMoneyEvent(client, oldMoney);

            return true;
        }

        #endregion 仓库货币处理

        /// <summary>
        /// 通知所有人节日/合服开启关闭状态
        /// </summary>
        /// <param name="client"></param>
        public void NotifyAllActivityState(int type, int state, string activityTimeBegin = "", string activityTimeEnd = "", int activityID = 0)
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", type, state, activityTimeBegin, activityTimeEnd, activityID);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_JIERIACT_STATE);
                if (!Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket))
                {
                }
            }
        }

        /// <summary>
        /// 在线所有角色重新生成专属活动数据
        /// </summary>
        public void ReGenerateSpecActGroup()
        {
            SpecialActivity act = HuodongCachingMgr.GetSpecialActivity();
            if (null == act)
                return;

            int index = 0;
            KPlayer client = null;
            while ((client = GetNextClient(ref index)) != null)
            {
                act.OnRoleLogin(client, false);

                // 感叹号
                if (client._IconStateMgr.CheckSpecialActivity(client))
                    client._IconStateMgr.SendIconStateToClient(client);
            }
        }
    }
}