using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
//using System.Windows.Forms;
using GameDBServer.DB;
using GameDBServer.Logic;
using GameDBServer.Core;

namespace GameDBServer.Server
{
    /// <summary>
    /// TCP管理对象
    /// </summary>
    class TCPManager
    {
        private static TCPManager instance = new TCPManager();

        public static long processCmdNum = 0;
        public static long processTotalTime = 0;
        public static Dictionary<int, PorcessCmdMoniter> cmdMoniter = new Dictionary<int, PorcessCmdMoniter>();

        private TCPManager() { }

        public static TCPManager getInstance()
        {
            return instance;
        }

        public void initialize(int capacity)
        {
            capacity = Math.Max(capacity, 250);
            socketListener = new SocketListener(capacity, (int)TCPCmdPacketSize.MAX_SIZE / 4);
            socketListener.SocketClosed += SocketClosed;
            socketListener.SocketConnected += SocketConnected;
            socketListener.SocketReceived += SocketReceived;
            socketListener.SocketSended += SocketSended;

            tcpInPacketPool = new TCPInPacketPool(capacity);
/*            tcpOutPacketPool = new TCPOutPacketPool(capacity * 5);*/
            tcpOutPacketPool = TCPOutPacketPool.getInstance();
            tcpOutPacketPool.initialize(capacity * 5);
            TCPCmdDispatcher.getInstance().initialize();
            dictInPackets = new Dictionary<Socket, TCPInPacket>(capacity);
            gameServerClients = new Dictionary<Socket, GameServerClient>();
        }

        public GameServerClient getClient(Socket socket)
        {
            GameServerClient client = null;
            gameServerClients.TryGetValue(socket, out client);
            return client;
        }

        /// <summary>
        /// 服务器端的侦听对象
        /// </summary>
        private SocketListener socketListener = null;

        /// <summary>
        /// 服务器端的侦听对象
        /// </summary>
        public SocketListener MySocketListener
        {
            get { return socketListener; }
        }

        /// <summary>
        /// 接收的命令包缓冲池
        /// </summary>
        private TCPInPacketPool tcpInPacketPool = null;

        /// <summary>
        /// 发送的命令包缓冲池
        /// </summary>
        private TCPOutPacketPool tcpOutPacketPool = null;

        /// <summary>
        /// 主窗口对象
        /// </summary>
        public Program RootWindow
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库服务对象
        /// </summary>
        public DBManager DBMgr
        {
            get;
            set;
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="port"></param>
        public void Start(string ip, int port)
        {
            socketListener.Init();
            socketListener.Start(ip, port);
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        /// <param name="port"></param>
        public void Stop()
        {
            socketListener.Stop();
        }

        #region 事件处理

        [ThreadStatic]
        public static GameServerClient CurrentClient;

        /// <summary>
        /// 命令包接收完毕后的回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool TCPCmdPacketEvent(object sender)
        {
            TCPInPacket tcpInPacket = sender as TCPInPacket;

            //接收到了完整的命令包
            TCPOutPacket tcpOutPacket = null;
            TCPProcessCmdResults result = TCPProcessCmdResults.RESULT_FAILED;

            GameServerClient client = null;
            if (!gameServerClients.TryGetValue(tcpInPacket.CurrentSocket, out client))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("未建立会话或会话已关闭: {0},{1}, 关闭连接", (TCPGameServerCmds)tcpInPacket.PacketCmdID, Global.GetSocketRemoteEndPoint(tcpInPacket.CurrentSocket)));
                return false;
            }

            CurrentClient = client;

            //接收到了完整的命令包
            long processBeginTime = TimeUtil.NowEx();

            result = TCPCmdHandler.ProcessCmd(client, DBMgr, tcpOutPacketPool, tcpInPacket.PacketCmdID, tcpInPacket.GetPacketBytes(), tcpInPacket.PacketDataSize, out tcpOutPacket);

            long processTime = (TimeUtil.NowEx() - processBeginTime);
            if (result == TCPProcessCmdResults.RESULT_DATA && null != tcpOutPacket)
            {
                //向对方发送数据
                socketListener.SendData(tcpInPacket.CurrentSocket, tcpOutPacket);
            }
            else if (result == TCPProcessCmdResults.RESULT_FAILED)//解析失败, 直接关闭连接
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析并执行命令失败: {0},{1}, 关闭连接", (TCPGameServerCmds)tcpInPacket.PacketCmdID, Global.GetSocketRemoteEndPoint(tcpInPacket.CurrentSocket)));
                //socketListener.CloseSocket(tcpInPacket.CurrentSocket);
                return false;
            }

            lock (cmdMoniter) //锁定命令监控对象
            {
                int cmdID = tcpInPacket.PacketCmdID;
                PorcessCmdMoniter moniter = null;
                if (!cmdMoniter.TryGetValue(cmdID, out moniter))
                {
                    moniter = new PorcessCmdMoniter(cmdID, processTime);
                    cmdMoniter.Add(cmdID, moniter);
                }

                moniter.onProcessNoWait(processTime);
            }

            CurrentClient = null;
            return true;
        }

        //接收的数据包队列
        private Dictionary<Socket, TCPInPacket> dictInPackets = null;
        //GameServer会话实例集合
        private Dictionary<Socket, GameServerClient> gameServerClients = null;

        /// <summary>
        /// 连接成功通知函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketConnected(object sender, SocketAsyncEventArgs e)
        {
            SocketListener sl = sender as SocketListener;
            //RootWindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
            //{
                RootWindow.TotalConnections = sl.ConnectedSocketsCount;
            //});
                //第一次建立连接时创建会话实例
                lock (gameServerClients)
                {
                    GameServerClient client = null;
                    Socket s = (e.UserToken as AsyncUserToken).CurrentSocket;
                    if (!gameServerClients.TryGetValue(s, out client))
                    {
                        client = new GameServerClient(s);
                        gameServerClients.Add(s, client);
                    }
                }
        }

        /// <summary>
        /// 断开成功通知函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketClosed(object sender, SocketAsyncEventArgs e)
        {
            SocketListener sl = sender as SocketListener;
            Socket s = (e.UserToken as AsyncUserToken).CurrentSocket;

            //将接收的缓冲删除
            lock (dictInPackets)
            {
                if (dictInPackets.ContainsKey(s))
                {
                    TCPInPacket tcpInPacket = dictInPackets[s];
                    tcpInPacketPool.Push(tcpInPacket); //缓冲回收
                    dictInPackets.Remove(s);
                }
            }

            //断开连接时，清除会话实例
            lock (gameServerClients)
            {
                GameServerClient client = null;
                if (gameServerClients.TryGetValue(s, out client))
                {
                    client.release();
                    gameServerClients.Remove(s);
                }
            }

            //通知主窗口显示连接数
            //RootWindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
            //{
                RootWindow.TotalConnections = sl.ConnectedSocketsCount;
            //});
        }

        /// <summary>
        /// 接收数据通知函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool SocketReceived(object sender, SocketAsyncEventArgs e)
        {
            SocketListener sl = sender as SocketListener;
            TCPInPacket tcpInPacket = null;
            Socket s = (e.UserToken as AsyncUserToken).CurrentSocket;
            lock (dictInPackets) //锁定接收包队列
            {
                if (!dictInPackets.TryGetValue(s, out tcpInPacket))
                {
                    tcpInPacket = tcpInPacketPool.Pop(s, TCPCmdPacketEvent);
                    dictInPackets[s] = tcpInPacket;
                }
            }

            //处理收到的包
            if (!tcpInPacket.WriteData(e.Buffer, e.Offset, e.BytesTransferred))
            {
                //LogManager.WriteLog(LogTypes.Error, string.Format("接收到非法数据长度的tcp命令, 需要立即端断开!"));
                //socketListener.CloseSocket(tcpInPacket.CurrentSocket);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发送数据通知函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketSended(object sender, SocketAsyncEventArgs e)
        {
            //SocketListener sl = sender as SocketListener;
            //Socket s = (e.UserToken as AsyncUserToken).CurrentSocket;
            TCPOutPacket tcpOutPacket = (e.UserToken as AsyncUserToken).Tag as TCPOutPacket;
            tcpOutPacketPool.Push(tcpOutPacket);
        }

        #endregion //事件处理
    }
}
