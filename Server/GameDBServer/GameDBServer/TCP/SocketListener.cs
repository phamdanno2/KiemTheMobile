using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using Server.Tools;
//using System.Windows.Forms;
using Server.Protocol;

namespace Server.TCP
{
    /*����������ָ���������:
     * 1. ���A socket�Ľ����¼��ڴ�������У��������̷߳���ʱ���󣬹ر�socket����ʱTCPInPacket�е�socket����ᱻ��գ����´��󡣵��������߳������������ã��޷������
     * ���ڴ����TCPInPacket�ڲ����ݣ����Բ��ᵼ����ָ�����򣬵��ǵ�������쳣���ٴδ���ر�ʱ�������޷����ֵ����ҵ�TCPInPacket���ԣ��޷���TCPInPacket���ٴ���TCPInPacket
     * �������̴߳�����յ�����ʱ�߼�����ȷ�ģ�ֻ�ǻ��ٲ����жϵ�����µ��¶�μ���socket������
     * 2. ����ʱ�������رղ������ᵼ�µ��Է��رգ��Լ�����ʧ��ʱ�����ִ�йرղ��������Ƕ���ָ��������û��Ӱ�졣
     * 3. ���A socket�Ľ���ʱ���ڴ�������У�ִ�з��ͳ�����ر�socket��ִ������ڴ�Ĳ�������ʱTCPInPacket�е�socket����ᱻ��գ����´��󡣲���û����ֹ�����������ݹ�ѭ������
     * ͬʱ�����Լ���TCPInPacket�黹���˶����У��������վͻ��л���ȡ��ʹ�ã�������socket ����ȡ��ʹ��ʱ�������¶���socket��ֵ������A socket�Ľ����еķ��ʹ����߼��������͸���
     * ����Ŀͻ��˶���ͬʱ�����ڴ�ʱ�Լ��ĵݹ�ѭ���˳�ʱ���ܻ��������ָ�����ݡ�����ʹ�����TCPInPacket��socket���յ������ݣ���ȥ��������д�����ᵼ�������ָ�������֡�
     * ����A ���������ָ�������������B���µ�ָ�����ݣ�����B��������ȫ�������Ǻǡ����ڸ㶮�ˣ�YES��
     * �������:
     * a. ����ʱ�����йرմ�����������ȥ����
     * b. ����ʱ��������ִ�����ֹ�ݹ�ѭ�����˳���
     * c. ���ղ��֣��������ݴ������������ִ�йر�Socket������
     * d. ����ԭ���ϣ��رղ�����������ȥ����
     */

    /// <summary>
    /// ������������
    /// </summary>
    public sealed class SocketListener
    {
        /// <summary>
        /// �������Ĵ�С
        /// </summary>
        Int32 ReceiveBufferSize;

        /// <summary>
        /// Represents a large reusable set of buffers for all socket operations.
        /// </summary>
        private BufferManager bufferManager;

        /// <summary>
        /// The socket used to listen for incoming connection requests.
        /// </summary>
        private Socket listenSocket;

        /// <summary>
        /// The total number of clients connected to the server.
        /// </summary>
        private Int32 numConnectedSockets;

        /// <summary>
        /// �Ѿ����ӵ�Socket�ʵ����
        /// </summary>
        private Dictionary<Socket, bool> ConnectedSocketsDict;

        /// <summary>
        /// �ⲿ��ȡ�ܵ����Ӹ���
        /// </summary>
        public Int32 ConnectedSocketsCount
        {
            get
            {
                Int32 n = 0;
                Interlocked.Exchange(ref n, this.numConnectedSockets);
                return n;
            }
        }

        /// <summary>
        /// the maximum number of connections the sample is designed to handle simultaneously.
        /// </summary>
        private Int32 numConnections;

        /// <summary>
        /// Read, write (don't alloc buffer space for accepts).
        /// </summary>
        private const Int32 opsToPreAlloc = 1;

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects for read socket operations.
        /// </summary>
        private SocketAsyncEventArgsPool readPool; //�̰߳�ȫ

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects for write socket operations.
        /// </summary>
        private SocketAsyncEventArgsPool writePool; //�̰߳�ȫ

        /// <summary>
        /// Controls the total number of clients connected to the server.
        /// </summary>
        private Semaphore semaphoreAcceptedClients;

        /// <summary>
        /// Total # bytes counter received by the server.
        /// </summary>
        private Int32 totalBytesRead;

        /// <summary>
        /// ��ȡ�ܵĽ��յ��ֽ���
        /// </summary>
        public Int32 TotalBytesReadSize
        {
            get
            {
                Int32 n = 0;
                Interlocked.Exchange(ref n, this.totalBytesRead);
                return n;
            }
        }

        /// <summary>
        /// Total # bytes counter received by the server.
        /// </summary>
        private Int32 totalBytesWrite;

        /// <summary>
        /// ��ȡ�ܵķ��͵��ֽ���
        /// </summary>
        public Int32 TotalBytesWriteSize
        {
            get
            {
                Int32 n = 0;
                Interlocked.Exchange(ref n, this.totalBytesWrite);
                return n;
            }
        }

        /// ���ӳɹ�֪ͨ����
        public event SocketConnectedEventHandler SocketConnected = null;

        /// �Ͽ��ɹ�֪ͨ����
        public event SocketClosedEventHandler SocketClosed = null;

        /// ��������֪ͨ����
        public event SocketReceivedEventHandler SocketReceived = null;

        /// ��������֪ͨ����
        public event SocketSendedEventHandler SocketSended = null;

        /// <summary>
        /// Create an uninitialized server instance.  
        /// To start the server listening for connection requests
        /// call the Init method followed by Start method.
        /// </summary>
        /// <param name="numConnections">Maximum number of connections to be handled simultaneously.</param>
        /// <param name="receiveBufferSize">Buffer size to use for each socket I/O operation.</param>
        internal SocketListener(Int32 numConnections, Int32 receiveBufferSize)
        {
            this.totalBytesRead = 0;
            this.totalBytesWrite = 0;
            this.numConnectedSockets = 0;
            this.numConnections = numConnections;
            this.ReceiveBufferSize = receiveBufferSize;

            // Allocate buffers such that the maximum number of sockets can have one outstanding read and 
            // write posted to the socket simultaneously .
            this.bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToPreAlloc,
                receiveBufferSize);

            // �Ѿ����ӵ�Socket�ʵ����
            this.ConnectedSocketsDict = new Dictionary<Socket, bool>(numConnections);

            this.readPool = new SocketAsyncEventArgsPool(numConnections);
            this.writePool = new SocketAsyncEventArgsPool(numConnections * 5);
            this.semaphoreAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        /// <summary>
        /// ���socket����
        /// </summary>
        /// <param name="socket"></param>
        private void AddSocket(Socket socket)
        {
            lock (this.ConnectedSocketsDict)
            {
                this.ConnectedSocketsDict.Add(socket, true);
            }
        }

        /// <summary>
        /// ɾ��socket����
        /// </summary>
        /// <param name="socket"></param>
        private void RemoveSocket(Socket socket)
        {
            lock (this.ConnectedSocketsDict)
            {
                this.ConnectedSocketsDict.Remove(socket);
            }
        }

        /// <summary>
        /// ����Socket����
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private bool FindSocket(Socket socket)
        {
            bool ret = false;
            lock (this.ConnectedSocketsDict)
            {
                ret = this.ConnectedSocketsDict.ContainsKey(socket);
            }

            return ret;
        }

        /// <summary>
        /// Close the socket associated with the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken aut = e.UserToken as AsyncUserToken;

            try
            {
                Socket s = aut.CurrentSocket;
                if (!FindSocket(s)) //�Ѿ��ر���
                {
                    return;
                }

                RemoveSocket(s);

                try
                {
                    LogManager.WriteLog(LogTypes.Info, string.Format("Close socket: {0}, last operation: {1}, error: {2}", s.RemoteEndPoint, e.LastOperation, e.SocketError));
                }
                catch (Exception)
                {
                }

                // Decrement the counter keeping track of the total number of clients connected to the server.
                this.semaphoreAcceptedClients.Release(); //��ʱ�ᳬ�������
                Interlocked.Decrement(ref this.numConnectedSockets);

                /// �Ͽ��ɹ�֪ͨ����
                if (null != SocketClosed)
                {
                    SocketClosed(this, e);
                }

                try
                {
                    s.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // Throws if client process has already closed.
                }

                try
                {
                    s.Close();
                }
                catch (Exception)
                {
                }
            }
            finally
            {
                aut.CurrentSocket = null; //�ͷ�
                aut.Tag = null; //�ͷ�

                // Free the SocketAsyncEventArg so they can be reused by another client.
                if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    e.SetBuffer(null, 0, 0); //�����ڴ�
                    this.writePool.Push(e);
                }
                else if (e.LastOperation == SocketAsyncOperation.Receive)
                {
                    this.readPool.Push(e);
                }
            }
        }

        /// <summary>
        /// Initializes the server by preallocating reusable buffers and 
        /// context objects.  These objects do not need to be preallocated 
        /// or reused, but it is done this way to illustrate how the API can 
        /// easily be used to create reusable objects to increase server performance.
        /// </summary>
        internal void Init()
        {
            // Allocates one large Byte buffer which all I/O operations use a piece of. This guards 
            // against memory fragmentation.
            this.bufferManager.InitBuffer();

            // Preallocate pool of SocketAsyncEventArgs objects.
            SocketAsyncEventArgs readWriteEventArg;

            for (Int32 i = 0; i < this.numConnections; i++)
            {
                // Preallocate a set of reusable SocketAsyncEventArgs.
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                readWriteEventArg.UserToken = new AsyncUserToken() { CurrentSocket = null, Tag = null };

                // Assign a Byte buffer from the buffer pool to the SocketAsyncEventArg object.
                this.bufferManager.SetBuffer(readWriteEventArg);

                // Add SocketAsyncEventArg to the pool.
                this.readPool.Push(readWriteEventArg);

                for (Int32 j = 0; j < 5; j++)
                {
                    readWriteEventArg = new SocketAsyncEventArgs();
                    readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                    readWriteEventArg.UserToken = new AsyncUserToken() { CurrentSocket = null, Tag = null };
                    this.writePool.Push(readWriteEventArg);
                }
            }
        }

        /// <summary>
        /// Callback method associated with Socket.AcceptAsync 
        /// operations and is invoked when an accept operation is complete.
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (null == this.listenSocket) return; //�Ѿ���������

                this.ProcessAccept(e);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("SocketListener::_ReceiveAsync got exception {0}", ex.ToString()));
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                    DataHelper.WriteFormatExceptionLog(ex, "", false);
                //});
            }
        }

        /// <summary>
        /// Callback called whenever a receive or send operation is completed on a socket.
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                // Determine which type of operation just completed and call the associated handler.
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        this.ProcessReceive(e);
                        break;
                    case SocketAsyncOperation.Send:
                        this.ProcessSend(e);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("SocketListener::_ReceiveAsync got exception {0}", ex.ToString()));
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                    DataHelper.WriteFormatExceptionLog(ex, "", false);
                //});
            }
        }

        /// <summary>
        /// ��socket��������
        /// </summary>
        /// <param name="s"></param>
        /// <param name="readEventArgs"></param>
        private bool _ReceiveAsync(SocketAsyncEventArgs readEventArgs)
        {
            try
            {
                Socket s = (readEventArgs.UserToken as AsyncUserToken).CurrentSocket;
                return s.ReceiveAsync(readEventArgs);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("SocketListener::_ReceiveAsync got exception {0}", ex.ToString()));
                this.CloseClientSocket(readEventArgs);
                return true;
            }
        }

        /// <summary>
        /// ��Socket��������
        /// </summary>
        /// <param name="writeEventArgs"></param>
        /// <returns></returns>
        private bool _SendAsync(SocketAsyncEventArgs writeEventArgs, out bool exception)
        {
            exception = false;

            try
            {
                Socket s = (writeEventArgs.UserToken as AsyncUserToken).CurrentSocket;
                return s.SendAsync(writeEventArgs);
            }
            catch (Exception ex) //�˴��п����Ƕ���Ƿ����쳣, ����Socket�����Ѿ���Ч
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("SocketListener::_ReceiveAsync got exception {0}", ex.ToString()));
                exception = true;
                //this.CloseClientSocket(writeEventArgs);
                return true;
            }
        }

        /// <summary>
        /// ��ͻ��˷�������
        /// </summary>
        /// <param name="data"></param>
        internal bool SendData(Socket s, TCPOutPacket tcpOutPacket)
        {
            SocketAsyncEventArgs writeEventArgs = this.writePool.Pop(); //�̰߳�ȫ�Ĳ���
            if (null == writeEventArgs)
            {
                writeEventArgs = new SocketAsyncEventArgs();
                writeEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                writeEventArgs.UserToken = new AsyncUserToken() { CurrentSocket = null, Tag = null };
            }

            //�ֽ�����
            //DataHelper.SortBytes(tcpOutPacket.GetPacketBytes(), 0, tcpOutPacket.PacketDataSize);

            writeEventArgs.SetBuffer(tcpOutPacket.GetPacketBytes(), 0, tcpOutPacket.PacketDataSize);
            (writeEventArgs.UserToken as AsyncUserToken).CurrentSocket = s;
            (writeEventArgs.UserToken as AsyncUserToken).Tag = (object)tcpOutPacket;

            bool exception = false;
            Boolean willRaiseEvent = _SendAsync(writeEventArgs, out exception);
            if (!willRaiseEvent)
            {
                this.ProcessSend(writeEventArgs);
            }

            return (!exception);
        }

        /// <summary>
        /// Process the accept for the socket listener.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs readEventArgs = null;

            //���Ӽ�����
            Interlocked.Increment(ref this.numConnectedSockets);

            // Get the socket for the accepted client connection and put it into the 
            // ReadEventArg object user token.
            Socket s = e.AcceptSocket;
            readEventArgs = this.readPool.Pop();
            if (null == readEventArgs)
            {
                try
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("New connection: {0}, force close coz connection pool is full, count: {1}", s.RemoteEndPoint, ConnectedSocketsCount));
                }
                catch (Exception)
                {
                }

                try
                {
                    s.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // Throws if client process has already closed.
                }

                try
                {
                    s.Close();
                }
                catch (Exception)
                {
                }

                //���ټ�����
                Interlocked.Decrement(ref this.numConnectedSockets);
                this.StartAccept(e);
                return;
            }

            (readEventArgs.UserToken as AsyncUserToken).CurrentSocket = e.AcceptSocket;

            byte[] inOptionValues = new byte[sizeof(uint) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)120000).CopyTo(inOptionValues, sizeof(uint));
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, sizeof(uint) * 2);
            s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            AddSocket(s);

            try
            {
                LogManager.WriteLog(LogTypes.Info, string.Format("New socket connection: {0}, total count: {1}", s.RemoteEndPoint, numConnectedSockets));
            }
            catch (Exception)
            {
            }

            if (null != SocketConnected)
            {
                SocketConnected(this, readEventArgs);
            }

            // As soon as the client is connected, post a receive to the connection.
            Boolean willRaiseEvent = _ReceiveAsync(readEventArgs);
            if (!willRaiseEvent)
            {
                this.ProcessReceive(readEventArgs);
            }

            // Accept the next connection request.

            this.StartAccept(e);
        }

        /// <summary>
        /// This method is invoked when an asynchronous receive operation completes. 
        /// If the remote host closed the connection, then the socket is closed.  
        /// If data was received then the data is echoed back to the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed receive operation.</param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            // Check if the remote host closed the connection.
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                // Increment the count of the total bytes receive by the server.
                Interlocked.Add(ref this.totalBytesRead, e.BytesTransferred);

                bool recvReturn = true;

                // Get the message received from the listener.
                //e.Buffer, e.Offset, e.bytesTransferred);
                //֪ͨ�ⲿ���µ�socket����
                if (null != SocketReceived)
                {
                    //�ֽ�����
                    //DataHelper.SortBytes(e.Buffer, e.Offset, e.BytesTransferred);
                    try
                    {
                        recvReturn = SocketReceived(this, e);
                    }
                    catch (System.Exception ex)
                    {
                        //�����쳣Ӧ�����²㲶�񣬲�Ӧ�׵������
                        LogManager.WriteException(ex.ToString());
                        recvReturn = false;
                    }

                }

                if (recvReturn)
                {
                    //������������(��������Ϊ����)
                    Boolean willRaiseEvent = _ReceiveAsync(e);
                    if (!willRaiseEvent)
                    {
                        this.ProcessReceive(e);
                    }
                }
                else
                {
                    this.CloseClientSocket(e);
                }
            }
            else
            {
                this.CloseClientSocket(e);
            }
        }

        /// <summary>
        /// This method is invoked when an asynchronous send operation completes.  
        /// The method issues another receive on the socket to read any additional 
        /// data sent from the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send operation.</param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
             /// ��������֪ͨ����
            if (null != SocketSended)
            {
                SocketSended(this, e);
            }

            if (e.SocketError == SocketError.Success)
            {
                Interlocked.Add(ref this.totalBytesWrite, e.BytesTransferred);
            }
            else
            {
                //this.CloseClientSocket(e);
            }

            //ʲô���鶼����, �ջ�ʹ�õ�e��buffer
            // Free the SocketAsyncEventArg so they can be reused by another client.
            e.SetBuffer(null, 0, 0); //�����ڴ�
            (e.UserToken as AsyncUserToken).CurrentSocket = null; //�ͷ�
            (e.UserToken as AsyncUserToken).Tag = null; //�ͷ�
            this.writePool.Push(e);
        }

        /// <summary>
        /// Starts the server such that it is listening for incoming connection requests.    
        /// </summary>
        /// <param name="localEndPoint">The endpoint which the server will listening for connection requests on.</param>
        internal void Start(string ip, int port)
        {
            if ("" == ip) ip = "0.0.0.0"; //��ֹIP��Ч

            // Get endpoint for the listener.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            // Create the socket which listens for incoming connections.
            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Associate the socket with the local endpoint.
            this.listenSocket.Bind(localEndPoint);

            // Start the server with a listen backlog of 100 connections.
            this.listenSocket.Listen(100);

            // Post accepts on the listening socket.
            this.StartAccept(null);
        }
        
        /// <summary>
        /// �ر�������Socket
        /// </summary>
        public void Stop()
        {
            Socket s = this.listenSocket;
            this.listenSocket = null;
            s.Close();
        }

        /// <summary>
        /// �ر�ָ����Socket����
        /// </summary>
        /// <param name="s"></param>
        private void CloseSocket(Socket s)
        {
            try
            {
                s.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                // Throws if client process has already closed.
            }

            //�ɽ����¼�ȥ�ͷŴ���
        }

        /// <summary>
        /// Begins an operation to accept a connection request from the client.
        /// </summary>
        /// <param name="acceptEventArg">The context object to use when issuing 
        /// the accept operation on the server's listening socket.</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                // Socket must be cleared since the context object is being reused.
                acceptEventArg.AcceptSocket = null;
            }

            this.semaphoreAcceptedClients.WaitOne(); //�����ܵ�������
            Boolean willRaiseEvent = this.listenSocket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
            {
                this.ProcessAccept(acceptEventArg);
            }
        }
    }
}
