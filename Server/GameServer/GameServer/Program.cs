using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Core.Rechage;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using GameServer.Server;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Tmsk.Contract;

namespace GameServer
{
    public class Program : IConnectInfoContainer
    {
#if VERIFY

        // VERRIFY MÁY TÍNH SỬ DỤNG MÁY CHỦ
        private static PZWJ pz = new PZWJ();

#endif

        public static FileVersionInfo AssemblyFileVersion;

#if Windows

        #region LOCK BUTTION CLOSE windows

        public delegate bool ControlCtrlDelegate(int CtrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);

        private static ControlCtrlDelegate newDelegate = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:

                    break;

                case 2:

                    break;
            }

            return true;
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// Khóa nút Close
        /// </summary>
        private static void HideCloseBtn()
        {
            Console.Title = "Server_" + Global.GetRandomNumber(0, 100000);
            IntPtr windowHandle = FindWindow(null, Console.Title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }

        #endregion LOCK BUTTION CLOSE windows

#endif

        /// <summary>
        /// Global Console Install
        /// </summary>
        public static Program ServerConsole = new Program();

        /// <summary>
        /// Xử lý callback CMD
        /// </summary>
        /// <param name="cmd"></param>
        public delegate void CmdCallback(String cmd);

        /// <summary>
        /// Tòa bộ thư viện lệnh
        /// </summary>
        private static Dictionary<String, CmdCallback> CmdDict = new Dictionary<string, CmdCallback>();

        public static Boolean NeedExitServer = false;

        #region Xử lý khi server Dumps

        // Thư mục dump
        private static string DumpBaseDir = "d:\\dumps\\";

        // Sau khi dump có thực hiện thoát server không?
        private static bool bDumpAndExit_ServerRunOk = false;

        /// <summary>
        /// Chặn ngoại lệ khi xảy ra crash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception exception = e.ExceptionObject as Exception;
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(exception, "CurrentDomain_UnhandledException", UnhandedException.ShowErrMsgBox, true);

                if (bDumpAndExit_ServerRunOk)
                {
                    if (!Directory.Exists(DumpBaseDir))
                    {
                        Directory.CreateDirectory(DumpBaseDir);
                    }

                    SysConOut.WriteLine("");
                    SysConOut.WriteLine("I had a problem, and i'm writting `dump` now, please wait for a moment...");

                    Process process = Process.Start(@"C:\Program Files\Debugging Tools for Windows (x64)\adplus.exe",
                        "-hang -o " + DumpBaseDir + " -p " + Process.GetCurrentProcess().Id.ToString());
                    process.WaitForExit();

                    Thread.Sleep(5000);
                }
            }
            catch
            {
            }
            finally
            {
                if (bDumpAndExit_ServerRunOk)
                {
                    Process.GetCurrentProcess().Kill();
                    Process.GetCurrentProcess().WaitForExit();
                }
            }
        }

        /// <summary>
        /// Xử lý ngoại lệ khi chết Threading
        /// </summary>
        private static void ExceptionHook()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        #endregion Xử lý khi server Dumps

        #region Khi đóng trương trình

        /// <summary>
        /// 删除某个指定的文件
        /// </summary>
        public static void DeleteFile(String strFileName)
        {
            String strFullFileName = System.IO.Directory.GetCurrentDirectory() + "\\" + strFileName;
            if (File.Exists(strFullFileName))
            {
                FileInfo fi = new FileInfo(strFullFileName);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                {
                    fi.Attributes = FileAttributes.Normal;
                }

                File.Delete(strFullFileName);
            }
        }

        /// <summary>
        /// Ghi lại thông tin prosecc
        /// </summary>
        public static void WritePIDToFile(String strFile)
        {
            String strFileName = System.IO.Directory.GetCurrentDirectory() + "\\" + strFile;

            Process processes = Process.GetCurrentProcess();
            int nPID = processes.Id;
            File.WriteAllText(strFileName, "" + nPID);
        }

        /// <summary>
        /// Ghi lại logs khi server stop
        /// </summary>
        public static int GetServerPIDFromFile()
        {
            String strFileName = System.IO.Directory.GetCurrentDirectory() + "\\GameServerStop.txt";
            if (File.Exists(strFileName))
            {
                string str = File.ReadAllText(strFileName);
                return int.Parse(str);
            }

            return 0;
        }

        #endregion Khi đóng trương trình

        #region Main Console

        private static void Main(string[] args)
        {
            // Thực hiện xóa files trước khi start server
            DeleteFile("Start.txt");
            DeleteFile("Stop.txt");
            DeleteFile("GameServerStop.txt");

            if (!GCSettings.IsServerGC && Environment.ProcessorCount > 2)
            {
                SysConOut.WriteLine(string.Format("Server Running Model  :{0}, {1}", GCSettings.IsServerGC ? "GameCenter" : "GameServer", GCSettings.LatencyMode));

                Console.WriteLine("Settings GameCenter ! ");

                string configFile = Process.GetCurrentProcess().MainModule.FileName + ".config";
                XElement xml = XElement.Load(configFile);
                XElement xml1 = xml.Element("runtime");
                if (null == xml1)
                {
                    xml.SetElementValue("runtime", "");
                    xml1 = xml.Element("runtime");
                }
                xml1.SetElementValue("gcServer", "");
                xml1.Element("gcServer").SetAttributeValue("enabled", "true");
                xml.Save(configFile);

                Console.WriteLine("Settings Server OK! Please Restart!");
                Console.Read();
                return;
            }
            else
            {
                SysConOut.WriteLine(string.Format("Server Running Model :{0}, {1}", GCSettings.IsServerGC ? "GameCenter" : "GameServer", GCSettings.LatencyMode));
            }
#if Windows

            #region Prosecc Console windows

            HideCloseBtn();

            SetConsoleCtrlHandler(newDelegate, true);

            if (Console.WindowWidth < 88)
            {
                Console.BufferWidth = 88;
                Console.WindowWidth = 88;
            }

            #endregion Prosecc Console windows

#endif
            ///Xử lý ngoại lệ
            ExceptionHook();

            ///Khởi tạo biến thời gian Tick Toàn bộ các sự kiện theo hiệu năng của máy chủ |
            // Máy chủ khỏe sẽ tick nhanh | Máy yếu sẽ tick chậm
            TimeUtil.Init();

            InitCommonCmd();

            //Check Duplicat In all num
            Global.CheckCodes();

            //Khởi động máy chủ
            OnStartServer();

            WritePIDToFile("Start.txt");

            bDumpAndExit_ServerRunOk = true;

            Thread thread = new Thread(ConsoleInputThread);
            thread.IsBackground = true;
            thread.Start();
            while (!NeedExitServer || !ServerConsole.MustCloseNow || ServerConsole.MainDispatcherWorker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            thread.Abort();
            Process.GetCurrentProcess().Kill();
        }

        public static void ConsoleInputThread(object obj)
        {
            String cmd = null;
            while (!NeedExitServer)
            {
                cmd = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(cmd))
                {
                    //ctrl + c
                    if (null != cmd && 0 == cmd.CompareTo("exit"))
                    {
                        SysConOut.WriteLine("Press Y to Exit Server?");
                        cmd = System.Console.ReadLine();
                        if (0 == cmd.CompareTo("y"))
                        {
                            break;
                        }
                    }

                    ParseInputCmd(cmd);
                }
            }

            // Exitding Server
            OnExitServer();
        }

        /// <summary>
        /// Xử lý lệnh khi gõ CMD
        /// </summary>
        /// <param name="cmd"></param>
        private static void ParseInputCmd(String cmd)
        {
            CmdCallback cb = null;
            int index = cmd.IndexOf('/');
            string cmd0 = cmd;
            if (index > 0)
            {
                cmd0 = cmd.Substring(0, index - 1).TrimEnd();
            }
            if (CmdDict.TryGetValue(cmd0, out cb) && null != cb)
            {
                cb(cmd);
            }
            else
            {
                SysConOut.WriteLine("Unknow command, input 'help' to get the data.");
            }
        }

        /// <summary>
        /// Start Servers
        /// </summary>
        private static void OnStartServer()
        {
            ServerConsole.InitServer();

            Console.Title = string.Format("KIEMTHE SERVER {0} Version : {1} : {2}", GameManager.ServerLineID, GetVersionDateTime(), ProgramExtName);
        }

        /// <summary>
        /// 进程退出
        /// </summary>
        private static void OnExitServer()
        {
            ServerConsole.ExitServer();
        }

        public static void Exit()
        {
            NeedExitServer = true;
            //主线程处于接收输入状态，如何唤醒呢？
        }

        #endregion Main Console

        #region 命令功能

        private static void InitCommonCmd()
        {
            CmdDict.Add("help", ShowCmdHelpInfo);
            CmdDict.Add("gc", GarbageCollect);
            CmdDict.Add("getfree", GetFreeThreading);
            CmdDict.Add("show dbconnect", ShowDBConnectInfo);
            CmdDict.Add("show baseinfo", ShowServerBaseInfo);
            CmdDict.Add("show tcpinfo", ShowServerTCPInfo);
            CmdDict.Add("show copymapinfo", ShowCopyMapInfo);
            CmdDict.Add("show gcinfo", ShowGCInfo);
            CmdDict.Add("show roleinfo", ShowRoleInfo);

            CmdDict.Add("reloadgs", ReloadGs);

            CmdDict.Add("resetbullettimer", ResetBulletTimers);
            CmdDict.Add("resetbufftimer", ResetBuffTimers);
            CmdDict.Add("resetmonstertimer", ResetMonsterTimers);
            CmdDict.Add("clearscheduletask", ClearScheduleTasks);

            CmdDict.Add("patch", RunPatchFromConsole);

            CmdDict.Add("clear", (x) => { Console.Clear(); });

            CmdDict.Add("report", (x) => { GameManager.ServerMonitor.CheckReport(); });
        }

        private static void GetFreeThreading(string cmd)
        {
            int ONE1 = 0; int TWO = 0;

            ThreadPool.GetAvailableThreads(out ONE1, out TWO);

            Console.WriteLine("workerThreads :" + ONE1 + "| completionPortThreads:" + TWO);
        }

        private static void ReloadGs(string cmd)
        {
            StartThreadPoolDriverTimer();
        }

        private static void ResetBulletTimers(string cmdID)
        {
            KTBulletManager.ForceResetBulletTimer();
            SysConOut.WriteLine("Reset bullet timer ok!!!");
        }

        private static void ClearScheduleTasks(string cmdID)
        {
            KTScheduleTaskManager.Instance.ClearScheduleTasks();
            SysConOut.WriteLine("Clear ScheduleTask timer ok!!!");
        }

        private static void ResetBuffTimers(string cmdID)
        {
            KTBuffManager.Instance.ClearAllBuffTimers();
            SysConOut.WriteLine("Reset buff timer ok!!!");
        }

        private static void ResetMonsterTimers(string cmdID)
        {
            KTMonsterTimerManager.Instance.ClearMonsterTimers();
            SysConOut.WriteLine("Reset monster timer ok!!!");
        }

        public static void LoadIPList(string strCmd)
        {
            try
            {
                if (String.IsNullOrEmpty(strCmd))
                {
                    strCmd = GameManager.GameConfigMgr.GetGameConfigItemStr("whiteiplist", "");
                }

                bool enabeld = true;
                string[] ipList = strCmd.Split(',');
                List<string> resultList = Global._TCPManager.MySocketListener.InitIPWhiteList(ipList, enabeld);

                if (resultList.Count > 0)
                {
                    Console.WriteLine("Ip Band List :");
                    foreach (var ip in resultList)
                    {
                        Console.WriteLine(ip);
                    }
                }
                else
                {
                    Console.WriteLine("No Band Ip List Found!");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Proplem when loading Band Ip List:\n" + ex.ToString());
            }
        }

        private static int[] GCCollectionCounts = new int[3];
        private static int[] GCCollectionCounts1 = new int[3];
        private static int[] GCCollectionCounts5 = new int[3];
        private static int[] GCCollectionCountsNow = new int[3];
        private static int[] MaxGCCollectionCounts1s = new int[3];
        private static int[] MaxGCCollectionCounts5s = new int[3];
        private static long[] MaxGCCollectionCounts1sTicks = new long[3];
        private static long[] MaxGCCollectionCounts5sTicks = new long[3];

        /// <summary>
        /// Tính toán số lượng GC và thông tin tần số
        /// </summary>
        public static void CalcGCInfo()
        {
            long ticks = TimeUtil.NOW();
            for (int i = 0; i < 3; i++)
            {
                GCCollectionCounts1[i] = GC.CollectionCount(i);
                if (GCCollectionCounts[i] != 0)
                {
                    int count = GCCollectionCounts1[i] - GCCollectionCounts[i];
                    if (ticks >= MaxGCCollectionCounts1sTicks[i] + 1000)
                    {
                        if (count > MaxGCCollectionCounts1s[i])
                        {
                            MaxGCCollectionCounts1s[i] = count;
                        }
                        MaxGCCollectionCounts1sTicks[i] = ticks;
                    }
                    if (ticks >= MaxGCCollectionCounts5sTicks[i] + 5000)
                    {
                        if (GCCollectionCounts5[i] != 0)
                        {
                            int count5s = GCCollectionCounts1[i] - GCCollectionCounts5[i];
                            if (count5s > MaxGCCollectionCounts5s[i])
                            {
                                MaxGCCollectionCounts5s[i] = count5s;
                            }
                        }
                        MaxGCCollectionCounts5sTicks[i] = ticks;
                        GCCollectionCounts5[i] = GCCollectionCounts1[i];
                    }
                    GCCollectionCountsNow[i] = count;
                }

                GCCollectionCounts[i] = GCCollectionCounts1[i];
            }
        }

        /// <summary>
        /// GC info
        /// </summary>
        private static void ShowGCInfo(String cmd = null)
        {
            try
            {
                Console.WriteLine(string.Format("GC    {0,-10} {1,-10} {2,-10}", "0 gen", "1 gen", "2 gen"));
                Console.WriteLine(string.Format("Total GC    {0,-10} {1,-10} {2,-10}", GCCollectionCounts[0], GCCollectionCounts[1], GCCollectionCounts[2]));
                Console.WriteLine(string.Format("GC Per Sec    {0,-10} {1,-10} {2,-10}", GCCollectionCountsNow[0], GCCollectionCountsNow[1], GCCollectionCountsNow[2]));
                Console.WriteLine(string.Format("GC Max Per Sec     {0,-10} {1,-10} {2,-10}", MaxGCCollectionCounts1s[0], MaxGCCollectionCounts1s[1], MaxGCCollectionCounts1s[2]));
                Console.WriteLine(string.Format("Max 5 GC     {0,-10} {1,-10} {2,-10}", MaxGCCollectionCounts5s[0], MaxGCCollectionCounts5s[1], MaxGCCollectionCounts5s[2]));
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "ShowGCInfo()", false);
            }
        }

        /// <summary>
        /// 显示帮助信息
        /// </summary>
        private static void ShowCmdHelpInfo(String cmd = null)
        {
        }

        /// <summary>
        /// 垃圾回收
        /// </summary>
        private static void GarbageCollect(String cmd = null)
        {
            try
            {
                //释放内存
                GC.Collect();
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "GarbageCollect()", false);
            }
        }

        /// <summary>
        /// 读取密码,以星号显示
        /// </summary>
        /// <returns></returns>
        private static string ReadPasswd()
        {
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo k;
            while (true)
            {
                k = Console.ReadKey();

                if (k.Key == ConsoleKey.Enter)
                {
                    return sb.ToString();
                }
                if (Console.CursorLeft > 0)
                {
                    Console.CursorLeft--;
                    Console.Write("*");
                    sb.Append(k.KeyChar);
                }
            }
        }

        /// <summary>
        /// 切换压测模式
        /// </summary>
        /// <param name="cmd"></param>
        private static void SetTestMode(String cmd = null)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }

            if ("tmsk201405" == ReadPasswd())
            {
                if (cmd.IndexOf("testmode 5") == 0)
                {
                    GameManager.TestGamePerformanceMode = true;
                    GameManager.TestGamePerformanceAllPK = true;
                    Console.WriteLine("开启压测模式,全体PK");
                }
                else if (cmd.IndexOf("testmode 1") == 0)
                {
                    GameManager.TestGamePerformanceMode = true;
                    GameManager.TestGamePerformanceAllPK = false;
                    Console.WriteLine("开启压测模式,和平模式");
                }
                else
                {
                    GameManager.TestGamePerformanceMode = false;
                    GameManager.TestGamePerformanceAllPK = false;
                    Console.WriteLine("关闭压测模式");
                }
            }
        }

        private delegate string PatchDelegate(string[] args);

        public static void RunPatchFromConsole(string cmd)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd))
                {
                    return;
                }

                string arg = null;
                if ("tmsk201405" != ReadPasswd())
                {
                    return;
                }
                Console.WriteLine("输入补丁信息:");

                arg = Console.ReadLine();
                RunPatch(arg);
            }
            catch (System.Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, "执行修补程序异常");
            }
        }

        public static void RunPatch(string arg, bool console = true)
        {
            try
            {
                if (string.IsNullOrEmpty(arg))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(arg))
                {
                    char[] spliteChars = new char[] { ' ' };
                    string[] args = arg.Split(spliteChars, StringSplitOptions.RemoveEmptyEntries);
                    if (null != args && args.Length >= 3 && !string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]) && !string.IsNullOrEmpty(args[2]))
                    {
                        string assemblyName = DataHelper.CurrentDirectory + args[0];
                        if (File.Exists(assemblyName))
                        {
                            //加载程序集
                            Assembly t = Assembly.LoadFrom(assemblyName);
                            if (null != t)
                            {
                                //加载类型
                                Type a = t.GetType(args[1]);
                                if (null != a)
                                {
                                    MethodInfo mi1 = a.GetMethod(args[2], BindingFlags.NonPublic | BindingFlags.Static);
                                    if (null != mi1)
                                    {
                                        //静态方法的调用
                                        object[] param = new object[1] { args };
                                        string s2 = (string)mi1.Invoke(null, param);
                                        LogManager.WriteLog(LogTypes.SQL, "执行修补程序" + arg + ",结果:" + s2);
                                        if (console && null != s2 && s2.Length < 4096)
                                        {
                                            Console.WriteLine(s2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, "执行修补程序异常");
            }
        }

        /// <summary>
        /// 数据库链接信息
        /// </summary>
        private static void ShowDBConnectInfo(String cmd = null)
        {
            try
            {
                foreach (var item in ServerConsole.DBServerConnectDict)
                {
                    SysConOut.WriteLine(item.Value);
                }

                foreach (var item in ServerConsole.LogDBServerConnectDict)
                {
                    SysConOut.WriteLine(item.Value);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "ShowDBConnectInfo()", false);
            }
        }

        /// <summary>
        /// Notification interface to modify the number of connections
        /// </summary>
        private static void ShowServerBaseInfo(String cmd = null)
        {
            // Notification interface to modify the number of connections
            SysConOut.WriteLine(string.Format("Online quantity {0}/{1}", GameManager.ClientMgr.GetClientCount(), Global._TCPManager.MySocketListener.ConnectedSocketsCount));

            int workerThreads = 0;
            int completionPortThreads = 0;
            System.Threading.ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

            SysConOut.WriteLine(string.Format("Thread pool informatio workerThreads={0}, completionPortThreads={1}", workerThreads, completionPortThreads));

            SysConOut.WriteLine(string.Format("Number of TCP event read and write buffers readPool={0}/{2}, writePool={1}/{2}", Global._TCPManager.MySocketListener.ReadPoolCount, Global._TCPManager.MySocketListener.WritePoolCount, Global._TCPManager.MySocketListener.numConnections * 3));

            SysConOut.WriteLine(string.Format("Number of database instructions {0}", GameManager.DBCmdMgr.GetDBCmdCount()));

            SysConOut.WriteLine(string.Format("Number of connections to DbServer{0}/{1}", Global._TCPManager.tcpClientPool.GetPoolCount(), Global._TCPManager.tcpClientPool.InitCount));

            SysConOut.WriteLine(string.Format("Number of TcpOutPacketPool:{0}, Instance: {1}, Number of TcpInPacketPool:{2}, Instance: {3}, Number of TCPCmdWrapper: {4}, SendPacketWrapper: {5}", Global._TCPManager.TcpOutPacketPool.Count, TCPOutPacket.GetInstanceCount(), Global._TCPManager.TcpInPacketPool.Count, TCPInPacket.GetInstanceCount(), TCPCmdWrapper.GetTotalCount(), SendPacketWrapper.GetInstanceCount()));

            //显示内存缓存信息
            string info = Global._MemoryManager.GetCacheInfoStr();

            SysConOut.WriteLine(info);

            info = Global._FullBufferManager.GetFullBufferInfoStr();

            SysConOut.WriteLine(info);

            info = Global._TCPManager.GetAllCacheCmdPacketInfo();
            SysConOut.WriteLine(info);
        }

        /// <summary>

        /// </summary>
        private static void ShowServerTCPInfo(String cmd = null)
        {
            bool clear = cmd.Contains("/c");
            bool detail = cmd.Contains("/d");

            string info = "";
            DateTime now = TimeUtil.NowDateTime();
            SysConOut.WriteLine(string.Format("SystemTime:{0},Statistical time:{1}", now.ToString("yyyy-MM-dd HH:mm:ss"), (now - ProcessSessionTask.StartTime).ToString()));
            if (clear)
            {
                detail = true; //清除命令默认打印详细指令信息
                ProcessSessionTask.StartTime = now;
            }

            SysConOut.WriteLine(string.Format("Total received bytes: {0:0.00} MB", Global._TCPManager.MySocketListener.TotalBytesReadSize / (1024.0 * 1024.0)));
            SysConOut.WriteLine(string.Format("Total sent bytes: {0:0.00} MB", Global._TCPManager.MySocketListener.TotalBytesWriteSize / (1024.0 * 1024.0)));

            SysConOut.WriteLine(string.Format("Total number of processing instructions {0}", TCPCmdHandler.TotalHandledCmdsNum));
            SysConOut.WriteLine(string.Format("The number of threads currently processing instructions {0}", TCPCmdHandler.GetHandlingCmdCount()));
            SysConOut.WriteLine(string.Format("Maximum time consumed by a single instruction {0}", TCPCmdHandler.MaxUsedTicksByCmdID));
            SysConOut.WriteLine(string.Format("Maximum time consumed instruction ID {0}", (TCPGameServerCmds)TCPCmdHandler.MaxUsedTicksCmdID));
            SysConOut.WriteLine(string.Format("Total number of calls sent {0}", Global._TCPManager.MySocketListener.GTotalSendCount));
            SysConOut.WriteLine(string.Format("The size of the largest packet sent {0}", Global._SendBufferManager.MaxOutPacketSize));
            SysConOut.WriteLine(string.Format("Command ID of the largest packet sent {0}", (TCPGameServerCmds)Global._SendBufferManager.MaxOutPacketSizeCmdID));

            //////////////////////////////////////////
            SysConOut.WriteLine(string.Format("Average instruction processing time {0}", ProcessSessionTask.processCmdNum != 0 ? TimeUtil.TimeMS(ProcessSessionTask.processTotalTime / ProcessSessionTask.processCmdNum) : 0));
            SysConOut.WriteLine(string.Format("Order processing time-consuming details"));

            try
            {
                if (detail)
                {
                    if (Console.WindowWidth < 160)
                    {
                        Console.WindowWidth = 160;
                    }
                }
                else
                {
                    if (Console.WindowWidth >= 88)
                    {
                        Console.WindowWidth = 88;
                    }
                }
            }
            catch
            {
            }

            int count = 0;
            lock (ProcessSessionTask.cmdMoniter)
            {
                foreach (GameServer.Logic.PorcessCmdMoniter m in ProcessSessionTask.cmdMoniter.Values)
                {
                    Console.ForegroundColor = (ConsoleColor)(count % 5 + ConsoleColor.Green); //逐行设置字体颜色
                    if (detail)
                    {
                        if (count++ == 0)
                        {
                            SysConOut.WriteLine(string.Format("{0, -48}{1, 6}{2, 7}{3, 7} {4, 7} {5, 4} {6, 4} {7, 5}", "news", "Processed times", "Average processing time", "Total time consumed", "Total number of bytes", "The number of transmissions", "Number of bytes sent", "failure/success/data"));
                        }
                        info = string.Format("{0, -50}{1, 11}{2, 13:0.##}{3, 13:0.##} {4, 13:0.##} {5, 8} {6, 12} {7, 4}/{8}/{9}", (TCPGameServerCmds)m.cmd, m.processNum, TimeUtil.TimeMS(m.avgProcessTime()), TimeUtil.TimeMS(m.processTotalTime), m.GetTotalBytes(), m.SendNum, m.OutPutBytes, m.Num_Faild, m.Num_OK, m.Num_WithData);
                        SysConOut.WriteLine(info);
                    }
                    else
                    {
                        if (count++ == 0)
                        {
                            SysConOut.WriteLine(string.Format("{0, -48}{1, 6}{2, 7}{3, 7}", "news", "Processed times", "Average processing time", "Total time consumed"));
                        }
                        info = string.Format("{0, -50}{1, 11}{2, 13:0.##}{3, 13:0.##}", (TCPGameServerCmds)m.cmd, m.processNum, TimeUtil.TimeMS(m.avgProcessTime()), TimeUtil.TimeMS(m.processTotalTime));
                        SysConOut.WriteLine(info);
                    }
                    if (clear)
                    {
                        m.Reset();
                    }
                }
                Console.ForegroundColor = ConsoleColor.White; //恢复字体颜色
            }
        }

        /// <summary>
        /// 显示副本相关信息
        /// </summary>
        private static void ShowCopyMapInfo(String cmd = null)
        {
        }

        /// <summary>
        /// 显示所以角色的地图和位置(性能分析)
        /// </summary>
        /// <param name="cmd"></param>
        private static void ShowRoleInfo(String cmd = null)
        {
            StringBuilder sb = new StringBuilder();
            int count = GameManager.ClientMgr.GetMaxClientCount();
            for (int i = 0; i < count; i++)
            {
                KPlayer client = GameManager.ClientMgr.FindClientByNid(i);
                if (null != client)
                {
                }
            }
            if (sb.Length == 0)
            {
                SysConOut.WriteLine("没有玩家在线");
            }
            else
            {
                SysConOut.WriteLine(sb.ToString());
            }
        }

        #endregion 命令功能

        #region 外部调用接口

        /// <summary>
        /// 与dbserver的链接信息词典
        /// </summary>
        public Dictionary<int, String> DBServerConnectDict = new Dictionary<int, string>();

        /// <summary>
        /// 增加数据服务器链接信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="info"></param>
        public void AddDBConnectInfo(int index, String info)
        {
            lock (DBServerConnectDict)
            {
                if (DBServerConnectDict.ContainsKey(index))
                {
                    DBServerConnectDict[index] = info;
                }
                else
                {
                    DBServerConnectDict.Add(index, info);
                }
            }
        }

        /// <summary>
        /// 与dbserver的链接信息词典
        /// </summary>
        public Dictionary<int, String> LogDBServerConnectDict = new Dictionary<int, string>();

        /// <summary>
        /// 增加数据服务器链接信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="info"></param>
        public void AddLogDBConnectInfo(int index, String info)
        {
            lock (LogDBServerConnectDict)
            {
                if (LogDBServerConnectDict.ContainsKey(index))
                {
                    LogDBServerConnectDict[index] = info;
                }
                else
                {
                    LogDBServerConnectDict.Add(index, info);
                }
            }
        }

        #endregion 外部调用接口

        #region 游戏服务器具体功能部分

        /// <summary>
        /// 程序额外的名称
        /// </summary>
        private static string ProgramExtName = "";

        /// <summary>
        /// 初始化应用程序名称
        /// </summary>
        /// <returns></returns>
        private static void InitProgramExtName()
        {
            ProgramExtName = DataHelper.CurrentDirectory;
        }

        /// <summary>
        /// Call khởi chạy máy chủ
        ///  Window_Loaded(object sender, RoutedEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InitServer()
        {
            InitProgramExtName();

            ThreadPool.SetMaxThreads(1000, 1000);

            if (!File.Exists(@"Policy.xml"))
            {
                throw new Exception(string.Format("Can't find files: {0}", @"Policy.xml"));
            }

            TCPPolicy.LoadPolicyServerFile(@"Policy.xml");

            Global.LoadLangDict();
            SearchTable.Init(20);

            SysConOut.WriteLine("Load Game Server Settings!");

            //Read Config XMl
            XElement xml = InitGameResPath();

            try
            {
                SysConOut.WriteLine("Connect to Database Server...");
                this.InitTCPManager(xml, true);

                SysConOut.WriteLine("Loading all game objects data Npcs, Monsters, Items...");
                this.InitGameManager(xml);

                SysConOut.WriteLine("Loading Maps and Monsters...");

                SysConOut.WriteLine("Initializing KTData...");
                /// Sói add Kiếm Thế
                KTMain.InitFirst();
                /// End
                /// Khởi tạo bản đồ và quái
                this.InitGameMapsAndMonsters();

                /// Sói add Kiếm Thế
                KTMain.InitAfterLoadingMap();

                GlobalServiceManager.Initialize();

                GlobalServiceManager.Startup();

                CreateRoleLimitManager.Instance().LoadConfig();
            }
            catch (System.Exception ex)
            {
                SysConOut.WriteLine("Exception was detected when Starting the Server... Kill Process after 5s!");
                Thread.Sleep(5000);
                LogManager.WriteException(ex.ToString());
                Process.GetCurrentProcess().Kill();
            }

            SysConOut.WriteLine("Starting all BackgroundWorkers...");

            SysConOut.WriteLine("BackgroundWorker Command Start");
            dbCommandWorker = new BackgroundWorker();
            dbCommandWorker.DoWork += dbCommandWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker LogsCommand Start");
            logDBCommandWorker = new BackgroundWorker();
            logDBCommandWorker.DoWork += logDBCommandWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker Clients Start");
            clientsWorker = new BackgroundWorker();
            clientsWorker.DoWork += clientsWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker spriteDBWorker Start");
            spriteDBWorker = new BackgroundWorker();
            spriteDBWorker.DoWork += spriteDBWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker othersWorker Start");
            othersWorker = new BackgroundWorker();
            othersWorker.DoWork += othersWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker dbWriterWorker Start");
            dbWriterWorker = new BackgroundWorker();
            dbWriterWorker.DoWork += dbWriterWorker_DoWork;

            SysConOut.WriteLine("Restore SocketBufferData for Client");
            SocketSendCacheDataWorker = new BackgroundWorker();
            SocketSendCacheDataWorker.DoWork += SocketSendCacheDataWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker MainDispatcherWorker Start");
            MainDispatcherWorker = new BackgroundWorker();
            MainDispatcherWorker.DoWork += MainDispatcherWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker socketCheckWorker Start");
            socketCheckWorker = new BackgroundWorker();
            socketCheckWorker.DoWork += SocketCheckWorker_DoWork;

            SysConOut.WriteLine("BackgroundWorker chat Start");
            chatMsgWorker = new BackgroundWorker();
            chatMsgWorker.DoWork += chatMsgWorker_DoWork;

            //Gird9UpdateWorkers = new BackgroundWorker[ServerConfig.Instance.MaxUpdateGridThread];
            //for (int nThread = 0; nThread < ServerConfig.Instance.MaxUpdateGridThread; nThread++)
            //{
            //    Gird9UpdateWorkers[nThread] = new BackgroundWorker();
            //    Gird9UpdateWorkers[nThread].DoWork += Gird9UpdateWorker_DoWork;
            //}

            UnhandedException.ShowErrMsgBox = false;

            Global._TCPManager.MySocketListener.DontAccept = false;

            if (!MainDispatcherWorker.IsBusy)
            {
                MainDispatcherWorker.RunWorkerAsync();
            }

            //for (int nThread = 0; nThread < ServerConfig.Instance.MaxUpdateGridThread; nThread++)
            //{
            //    if (!Gird9UpdateWorkers[nThread].IsBusy)
            //    {
            //        Gird9UpdateWorkers[nThread].RunWorkerAsync(nThread);
            //    }
            //}

            StartThreadPoolDriverTimer();

            GameManager.GameConfigMgr.SetGameConfigItem("gameserver_version", GetVersionDateTime());

            Global.UpdateDBGameConfigg("gameserver_version", GetVersionDateTime());

            SysConOut.WriteLine("Bring Tcp Protocal...");

            Thread.Sleep(3000);
            InitTCPManager(xml, false);

            SysConOut.WriteLine("Get all MailList from Db");
            GroupMailManager.RequestNewGroupMailList();
            // Nếu không phải liên máy chủ thì thực hiện start webservice
            // Ở liên máy chủ không cần start webservice
            SysConOut.WriteLine("BackgroundWorker WEBAPI Start");
            WebAPIService = new BackgroundWorker();
            WebAPIService.DoWork += LoadWebApi_DoWork;
            SysConOut.WriteLine("Server is Started..");

            Utils.SendTelegram("Máy Chủ sv [" + GameManager.ServerLineID + "] đã khởi chạy !!!!!");

            GameManager.ServerStarting = false;
        }

        public static string GetLocalIPAddress()
        {
            if (GameManager.ServerLineID == 9)
            {
                return "66.42.60.246";
            }
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void LoadWebApi_DoWork(object sender, EventArgs e)
        {
            HttpListener web = new HttpListener();

            Console.WriteLine("IP LA: " + GetLocalIPAddress());
            web.Prefixes.Add("http://" + GetLocalIPAddress() + ":" + GameManager.HttpServiceCode + "/");
            string msg = "OK";

            web.Start();

            SysConOut.WriteLine("HTTP Service Bring Port: " + GameManager.HttpServiceCode);
            while (true)
            {
                try
                {
                    HttpListenerContext context = web.GetContext();

                    if (context.Request.QueryString["Rechage"] != null && context.Request.QueryString["KeyStore"] != null)
                    {
                        string MD5Key = RechageServiceManager.MakeMD5Hash(KT_TCPHandler.WebKey);

                        string KeyStore = context.Request.QueryString["KeyStore"].ToString().Trim();

                        if (KeyStore == MD5Key)
                        {
                            string RoleID = context.Request.QueryString["RoleID"].ToString().Trim();

                            string TransID = context.Request.QueryString["TransID"].ToString().Trim();

                            string PackageName = context.Request.QueryString["PackageName"].ToString().Trim();

                            string Sing = context.Request.QueryString["Sing"].ToString().Trim();

                            string TimeBuy = context.Request.QueryString["TimeBuy"].ToString().Trim();

                            string ActiveCardMonth = context.Request.QueryString["ActiveCardMonth"].ToString().Trim();

                            bool IsOK = false;

                            if (ActiveCardMonth == "1")
                            {
                                IsOK = true;
                            }

                            RechageModel _Rechage = new RechageModel();

                            _Rechage.RoleID = Int32.Parse(RoleID);
                            _Rechage.TransID = TransID;
                            _Rechage.TimeBuy = TimeBuy;
                            _Rechage.PackageName = PackageName;
                            _Rechage.Sing = Sing;
                            _Rechage.ActiveCardMonth = IsOK;

                            LogManager.WriteLog(LogTypes.Rechage, "[PackageName] Add yêu cầu mua gói :" + PackageName + "|RoleId :" + RoleID);

                            RechageServiceManager.ReachageData.Add(_Rechage);
                        }
                    }

                    if (context.Request.QueryString["CcuOnline"] != null && context.Request.QueryString["KeyStore"] != null)
                    {
                        string MD5Key = RechageServiceManager.MakeMD5Hash(KT_TCPHandler.WebKey);

                        string KeyStore = context.Request.QueryString["KeyStore"].ToString().Trim();

                        if (KeyStore == MD5Key)
                        {
                            msg = GameManager.ClientMgr.GetClientCount() + "";
                        }
                    }

                    if (context.Request.QueryString["SendNotify"] != null && context.Request.QueryString["KeyStore"] != null)
                    {
                        string MD5Key = RechageServiceManager.MakeMD5Hash(KT_TCPHandler.WebKey);

                        string KeyStore = context.Request.QueryString["KeyStore"].ToString().Trim();

                        if (KeyStore == MD5Key)
                        {
                            string MSGENCODE = context.Request.QueryString["Msg"].ToString().Trim();

                            string Notify = DataHelper.DecodeBase64(MSGENCODE);

                            KTGMCommandManager.SendSystemEventNotification(Notify);
                        }
                    }

                    if (context.Request.QueryString["ShutDownServer"] != null && context.Request.QueryString["KeyStore"] != null)
                    {
                        string MD5Key = RechageServiceManager.MakeMD5Hash(KT_TCPHandler.WebKey);

                        string KeyStore = context.Request.QueryString["KeyStore"].ToString().Trim();

                        if (KeyStore == MD5Key)
                        {
                            Thread _Thread = new Thread(() => OnExitServer());
                            _Thread.Start();
                        }
                    }
                    HttpListenerResponse response = context.Response;

                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                    response.ContentLength64 = buffer.Length;
                    Stream st = response.OutputStream;
                    st.Write(buffer, 0, buffer.Length);
                    context.Response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        //关闭服务器
        public void ExitServer()
        {
            if (NeedExitServer)
            {
                return;
            }

            GlobalServiceManager.Showdown();

            GlobalServiceManager.Destroy();

            Global._TCPManager.Stop(); //停止TCP的侦听，否则mono无法正常退出

            Window_Closing();

            SysConOut.WriteLine("Server is shutting down...");

            if (0 == GetServerPIDFromFile())
            {
                String cmd = System.Console.ReadLine();

                while (true)
                {
                    if (MainDispatcherWorker.IsBusy)
                    {
                        SysConOut.WriteLine("Trying to shut down Server...");
                        cmd = System.Console.ReadLine();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                StopThreadPoolDriverTimer();
            }
        }

        #region 初始化部分

        /// <summary>
        /// 初始化游戏资源目录
        /// </summary>
        private XElement InitGameResPath()
        {
            XElement xml = null;

            try
            {
                xml = XElement.Load(@"AppConfig.xml");
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Can't Find ResPath", @"AppConfig.xml"));
            }

            Global.AbsoluteGameResPath = Global.GetSafeAttributeStr(xml, "Resource", "Path");

            string appPath = DataHelper.CurrentDirectory;
            if (Global.AbsoluteGameResPath.IndexOf("$SERVER$") >= 0)
            {
                Global.AbsoluteGameResPath = Global.AbsoluteGameResPath.Replace("$SERVER$", appPath);
            }

            if (!string.IsNullOrEmpty(Global.AbsoluteGameResPath))
            {
                Global.AbsoluteGameResPath = Global.AbsoluteGameResPath.Replace("\\", "/");
                Global.AbsoluteGameResPath = Global.AbsoluteGameResPath.TrimEnd('/');
            }

            Global.CheckConfigPathType();

            return xml;
        }

        private void ExitOnError(string msg, Exception ex)
        {
            LogManager.WriteLog(LogTypes.Fatal, msg + ex.ToString());
            Console.ReadLine();
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// Khởi tạo bản đồ và quái
        /// </summary>
        private void InitGameMapsAndMonsters()
        {
            XElement xml = null;

            try
            {
                xml = Global.GetGameResXml(@"Config/KT_Map/MapConfig.xml");
            }
            catch (Exception ex)
            {
                ExitOnError(string.Format("XML file not found: {0}", @"MapConfig.xml"), ex);
            }

            IEnumerable<XElement> mapItems = xml.Elements();

            GameManager.ClientMgr.Initialize(mapItems);
            GameManager.MonsterMgr.Initialize(mapItems);
            GameManager.MonsterZoneMgr.LoadAllMonsterXml();

            foreach (XElement mapItem in mapItems)
            {
                int mapLevel = Convert.ToInt32(Global.GetSafeAttribute(mapItem, "MapLevel").Value.ToString());
                string mapType = Global.GetSafeAttribute(mapItem, "MapType").Value.ToString();

                int mapCode = (int)Global.GetSafeAttributeLong(mapItem, "ID");
                string mapResName = Global.GetSafeAttributeStr(mapItem, "MapCode");

                string name = string.Format("MapConfig/{0}/Obs.xml", mapResName);
                XElement xmlMask = null;

                try
                {
                    xmlMask = Global.GetResXml(name);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Load XML file {0} faild, error {1}", name, ex.ToString()));
                }

                int mapWidth = Convert.ToInt32(Global.GetSafeAttribute(xmlMask, "MapWidth").Value.ToString());
                int mapHeight = Convert.ToInt32(Global.GetSafeAttribute(xmlMask, "MapHeight").Value.ToString());

                /// Khởi tạo bản đồ
                GameMap gameMap = GameManager.MapMgr.InitAddMap(mapCode, Global.GetSafeAttributeStr(mapItem, "Name"), mapResName, mapWidth, mapHeight, mapLevel, mapType);

                /// Khởi tạo lưới bản đồ
                GameManager.MapGridMgr.InitAddMapGrid((int)Global.GetSafeAttributeLong(mapItem, "ID"), mapWidth, mapHeight, GameManager.GridSize, GameManager.GridSize, gameMap);

                /// Tải danh sách quái trong Map
                GameManager.MonsterZoneMgr.AddMapMonsters((int)Global.GetSafeAttributeLong(mapItem, "ID"), mapResName, gameMap);

                /// Tải danh sách NPC trong Map
                NPCGeneralManager.LoadMapNPCs((int)Global.GetSafeAttributeLong(mapItem, "ID"), mapResName, gameMap);

                /// Tải danh sách điểm thu thập trong Map
                KTGrowPointManager.LoadMapGrowPoints((int)Global.GetSafeAttributeLong(mapItem, "ID"), mapResName);
            }

            SysConOut.WriteLine(StringUtil.substitute("Map Loading Report => Total Monster: {0}", GameManager.MonsterMgr.GetMonstersCount()));
        }

        /// <summary>
        /// 缓存信息初始化
        /// </summary>
        private void InitCache(XElement xml)
        {
            //Quản lý lỗi bộ nhớ đệm
            Global._FullBufferManager = new FullBufferManager();
            //Dánh ách bộ nhớ đệm
            Global._SendBufferManager = new SendBufferManager();
            //Tiếu thiểu 50 minis
            SendBuffer.SendDataIntervalTicks = Global.GMax(20, Global.GMin(500, (int)Global.GetSafeAttributeLong(xml, "SendDataParam", "SendDataIntervalTicks")));
            //这个值必须小于10240,大于等于1500
            SendBuffer.MaxSingleSocketSendBufferSize = Global.GMax(18000, Global.GMin(256000, (int)Global.GetSafeAttributeLong(xml, "SendDataParam", "MaxSingleSocketSendBufferSize")));
            //发送超时判读 单位毫秒
            SendBuffer.SendDataTimeOutTicks = Global.GMax(3000, Global.GMin(20000, (int)Global.GetSafeAttributeLong(xml, "SendDataParam", "SendDataTimeOutTicks")));
            //
            SendBuffer.MaxBufferSizeForLargePackge = SendBuffer.MaxSingleSocketSendBufferSize * 2 / 3;

            //内存管理器
            Global._MemoryManager = new MemoryManager();

            string cacheMemoryBlocks = Global.GetSafeAttributeStr(xml, "CacheMemoryParam", "CacheMemoryBlocks");
            if (string.IsNullOrWhiteSpace(cacheMemoryBlocks))
            {
                Global._MemoryManager.AddBatchBlock(100, 1500);
                Global._MemoryManager.AddBatchBlock(600, 400);
                Global._MemoryManager.AddBatchBlock(600, 50);
                Global._MemoryManager.AddBatchBlock(600, 100);
            }
            else
            {
                string[] items = cacheMemoryBlocks.Split('|');
                foreach (var item in items)
                {
                    string[] pair = item.Split(',');
                    int blockSize = int.Parse(pair[0]);
                    int blockNum = int.Parse(pair[1]);
                    blockNum = Global.GMax(blockNum, 80); //缓存数不少于80
                    if (blockSize > 0 && blockNum > 0)
                    {
                        Global._MemoryManager.AddBatchBlock(blockNum, blockSize);
                        GameManager.MemoryPoolConfigDict[blockSize] = blockNum;
                    }
                }
            }
        }

        /// <summary>
        /// Khởi tạo đối tượng quản lý giao tiếp
        /// </summary>
        private void InitTCPManager(XElement xml, bool bConnectDB)
        {
            if (bConnectDB)
            {
                // Phiên bản Code
                GameManager.DefaultMapCode = (int)Global.GetSafeAttributeLong(xml, "Map", "Code");

                // Main Code
                GameManager.ClientCore = (int)Global.GetSafeAttributeLong(xml, "ClientVersion", "Core");

                GameManager.ClientResVer = (int)Global.GetSafeAttributeLong(xml, "ClientVersion", "Resource");

                // Main Code
                GameManager.MainMapCode = (int)Global.GetSafeAttributeLong(xml, "Map", "MainCode");

                GameManager.HttpServiceCode = (int)Global.GetSafeAttributeLong(xml, "APIWebServer", "Port");

                // Khởi tạo server LINE
                GameManager.ServerLineID = (int)Global.GetSafeAttributeLong(xml, "Server", "LineID");

                if (GameManager.ServerLineID > 9000)
                {
                    GameManager.IsKuaFuServer = true;
                }
                else
                {
                    GameManager.IsKuaFuServer = false;
                }

                // Logs ra xem có phải đây là KUNGFU server không
                Console.WriteLine("Server Line : " + GameManager.ServerLineID + "| IsKuaFuServer :" + GameManager.IsKuaFuServer.ToString());

                GameManager.ActiveGiftCodeUrl = Global.GetSafeAttributeStr(xml, "GiftCode", "Url");

                Console.WriteLine("ActiveGiftCode Service : " + GameManager.ActiveGiftCodeUrl);

                // Dánh sách vật phẩm sẽ tặng cho người chơi khi tạo nhân vật
                List<int> testvp = new List<int>();
                testvp.Add(781);
                GameManager.AutoGiveGoodsIDList = testvp; //
                //GameManager.AutoGiveGoodsIDList.Add(781);
                //GameManager.AutoGiveGoodsIDList.Add(768);

                // Logs Level Lưu
                LogManager.LogTypeToWrite = (LogTypes)(int)Global.GetSafeAttributeLong(xml, "Server", "LogType");

                Console.WriteLine("Logs level Set : " + LogManager.LogTypeToWrite);

                // 事件日志级别
                GameManager.SystemServerEvents.EventLevel = (EventLevels)(int)Global.GetSafeAttributeLong(xml, "Server", "EventLevel");
                GameManager.SystemRoleLoginEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleLogoutEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleTaskEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleDeathEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBuyWithTongQianEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBuyWithBoundTokenEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBuyWithYinPiaoEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBuyWithYuanBaoEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleSaleEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleExchangeEvents1.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleExchangeEvents2.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleExchangeEvents3.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleUpgradeEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleGoodsEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleFallGoodsEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBoundTokenEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleHorseEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBangGongEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleJingMaiEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleRefreshQiZhenGeEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleWaBaoEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleMapEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleFuBenAwardEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleWuXingAwardEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRolePaoHuanOkEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleYaBiaoEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleLianZhanEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleHuoDongMonsterEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleDigTreasureWithYaoShiEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleAutoSubYuanBaoEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleAutoSubBoundMoneyEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleAutoSubEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBuyWithTianDiJingYuanEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleFetchVipAwardEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;

                GameManager.SystemRoleFetchMailMoneyEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;

                GameManager.SystemRoleBuyWithBoundMoneyEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;
                GameManager.SystemRoleBoundMoneyEvents.EventLevel = GameManager.SystemServerEvents.EventLevel;

                int dbLog = Global.GMax(0, (int)Global.GetSafeAttributeLong(xml, "DBLog", "DBLogEnable"));

                //Khởi tạo Xml sẽ cache
                InitCache(xml);

                //try
                //{
                //    Global.Flag_NameServer = true;
                //    NameServerNamager.Init(xml);
                //}
                //catch (System.Exception ex)
                //{
                //    Global.Flag_NameServer = false;
                //    Console.WriteLine(ex.ToString());
                //    //throw ex;
                //}

                int nCapacity = (int)Global.GetSafeAttributeLong(xml, "Socket", "capacity") * 3;

                TCPManager.getInstance().initialize(nCapacity);

                Global._TCPManager = TCPManager.getInstance();

                Global._TCPManager.tcpClientPool.RootWindow = this;
                Global._TCPManager.tcpClientPool.Init(
                    (int)Global.GetSafeAttributeLong(xml, "DBServer", "pool"),
                    Global.GetSafeAttributeStr(xml, "DBServer", "ip"),
                    (int)Global.GetSafeAttributeLong(xml, "DBServer", "port"),
                    "DBServer");

                Global._TCPManager.tcpLogClientPool.RootWindow = this;
                Global._TCPManager.tcpLogClientPool.Init(
                    (int)Global.GetSafeAttributeLong(xml, "LogDBServer", "pool"),
                    Global.GetSafeAttributeStr(xml, "LogDBServer", "ip"),
                    (int)Global.GetSafeAttributeLong(xml, "LogDBServer", "port"),
                    "LogDBServer");
            }
            else
            {
                KT_TCPHandler.KeySHA1 = Global.GetSafeAttributeStr(xml, "Token", "sha1");
                KT_TCPHandler.KeyData = Global.GetSafeAttributeStr(xml, "Token", "data");
                KT_TCPHandler.WebKey = Global.GetSafeAttributeStr(xml, "Token", "webkey");
                KT_TCPHandler.WebKeyLocal = KT_TCPHandler.WebKey;
                string loginWebKey = GameManager.GameConfigMgr.GetGameConfigItemStr("loginwebkey", KT_TCPHandler.WebKey);
                if (!string.IsNullOrEmpty(loginWebKey) && loginWebKey.Length >= 5)
                {
                    KT_TCPHandler.WebKey = loginWebKey;
                }

                Global._TCPManager.tcpRandKey.Init(
                (int)Global.GetSafeAttributeLong(xml, "Token", "count"),
                (int)Global.GetSafeAttributeLong(xml, "Token", "randseed"));

                //启动通讯管理对象
                Global._TCPManager.RootWindow = this;
                Global._TCPManager.Start(Global.GetSafeAttributeStr(xml, "Socket", "ip"),
                    (int)Global.GetSafeAttributeLong(xml, "Socket", "port"));
            }
        }

        /// <summary>
        /// Khởi tạo game
        /// </summary>
        private void InitGameManager(XElement xml)
        {
            GameManager.AppMainWnd = this;

            /// Tải danh sách NPC
            GameManager.SystemNPCsMgr.LoadFromXMlFile("Config/KT_NPC/NPCs.xml", "NPCs", "ID");

            /// Tải danh sách quái
            GameManager.systemMonsterMgr.LoadFromXMlFile("Config/KT_Monster/Monsters.xml", "Monsters", "ID");

            //从数据库中获取配置参数
            GameManager.GameConfigMgr.LoadGameConfigFromDBServer();

            // 装入白名单
            LoadIPList("");

            //初始化和数据库存储的配置相关的数据
            InitGameConfigWithDB();

            GameManager.loginWaitLogic.LoadConfig();
        }

        /// <summary>
        /// 初始化游戏配置(数据库相关)
        /// </summary>
        private void InitGameConfigWithDB()
        {
            //初始化服务器ID(服务器区号)
            GameManager.ServerId = Global.SendToDB<int, string>((int)TCPGameServerCmds.CMD_DB_GET_SERVERID, "", GameManager.LocalServerId);

            GameManager.Flag_OptimizationBagReset = GameManager.GameConfigMgr.GetGameConfigItemInt("optimization_bag_reset", 1) > 0;
            GameManager.SetLogFlags(GameManager.GameConfigMgr.GetGameConfigItemInt("logflags", 0x7fffffff));

            //以下是平台相关的
            string platformType = GameManager.GameConfigMgr.GetGameConfigItemStr("platformtype", "app");
            for (PlatformTypes i = PlatformTypes.Tmsk; i < PlatformTypes.Max; i++)
            {
                if (0 == string.Compare(platformType, i.ToString(), true))
                {
                    GameManager.PlatformType = i;
                    return;
                }
            }

            //处理拼写不规范的配置
            if (platformType == "andrid")
            {
                GameManager.PlatformType = PlatformTypes.Android;
            }
            else
            {
                GameManager.PlatformType = PlatformTypes.APP;
            }

            GameManager.LoadGameConfigFlags();
        }

        /// <summary>
        /// 初始化怪物管理对象
        /// </summary>
        private void InitMonsterManager()
        {
            //GameManager.MonsterMgr.CycleExecute += ExecuteBackgroundWorkers;
        }

        /// <summary>
        private static Timer ThreadPoolDriverTimer = null;

        /// <summary>
        /// 日志线程池驱动定时器
        /// </summary>
        private static Timer LogThreadPoolDriverTimer = null;

        /// <summary>
        /// 初始化线程池驱动定时器
        /// </summary>
        protected static void StartThreadPoolDriverTimer()
        {
            ThreadPoolDriverTimer = new Timer(ThreadPoolDriverTimer_Tick, null, 1000, 1000);
            LogThreadPoolDriverTimer = new Timer(LogThreadPoolDriverTimer_Tick, null, 500, 500);
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        protected static void StopThreadPoolDriverTimer()
        {
            ThreadPoolDriverTimer.Change(Timeout.Infinite, Timeout.Infinite);
            LogThreadPoolDriverTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        protected static void ThreadPoolDriverTimer_Tick(Object sender)
        {
            try
            {
                //驱动后台线程池
                ServerConsole.ExecuteBackgroundWorkers(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                {
                    // 格式化异常错误信息
                    DataHelper.WriteFormatExceptionLog(ex, "ThreadPoolDriverTimer_Tick", false);
                    //throw ex;
                }//);
            }
        }

        public static void LogThreadPoolDriverTimer_Tick(Object sender)
        {
            try
            {
                ServerConsole.ExecuteBackgroundLogWorkers(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "LogThreadPoolDriverTimer_Tick", false);
            }
        }

        /// <summary>
        /// 停止定时器
        /// </summary>

        /// <summary>
        /// 线程驱动定时器
        /// </summary>
        /// <param name="sender"></param>
        //protected static void ThreadPoolDriverTimer_Tick(Object sender)
        //{
        //    try
        //    {
        //        //驱动后台线程池
        //        ServerConsole.ExecuteBackgroundWorkers(null, EventArgs.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.WriteLog(LogTypes.Exception, "ThreadPoolDriverTimer BUG :" + ex.ToString());
        //    }
        //}

        /// <summary>
        /// 日志线程驱动定时器
        /// </summary>
        /// <param name="sender"></param>

        #endregion 初始化部分

        #region 线程部分

        /// 数据库命令执行线程
        /// </summary>
        private BackgroundWorker dbCommandWorker;

        /// <summary>
        /// 日志数据库命令执行线程
        /// </summary>
        private BackgroundWorker logDBCommandWorker;

        /// <summary>
        /// 角色调度线程
        /// </summary>
        private BackgroundWorker clientsWorker;

        /// <summary>
        /// 角色DB线程
        /// </summary>
        private BackgroundWorker spriteDBWorker;

        /// <summary>
        /// 后台处理线程
        /// </summary>
        private BackgroundWorker othersWorker;

        /// </summary>
        private BackgroundWorker dbWriterWorker;

        private BackgroundWorker chatMsgWorker;

        private BackgroundWorker WebAPIService;

        /// <summary>
        /// 套接字缓冲数据发送线程
        /// </summary>
        private BackgroundWorker SocketSendCacheDataWorker;

        /// <summary>
        /// 主调度线程,这个线程一直处于循环状态，不断的处理各种逻辑判断,相当于原来的主界面线程
        /// </summary>
        private BackgroundWorker MainDispatcherWorker;

        /// <summary>
        /// socket检查线程
        /// </summary>
        private BackgroundWorker socketCheckWorker;

        // Danh sách các BG update tầm nhìn cho client
        private BackgroundWorker[] Gird9UpdateWorkers;

        /// <summary>
        /// 是否是要立刻关闭
        /// </summary>
        private bool MustCloseNow = false;

        /// <summary>
        /// 是否进入了关闭模式
        /// </summary>
        private bool EnterClosingMode = false;

        /// <summary>
        /// 60秒钟的倒计时器
        /// </summary>
        private int ClosingCounter = 30 * 200;

        /// <summary>
        /// 最近一次写数据库日志的时间
        /// </summary>
        private long LastWriteDBLogTicks = TimeUtil.NOW();

        /// <summary>
        /// 执行日志后台线程对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteBackgroundLogWorkers(object sender, EventArgs e)
        {
            try
            {
                if (!logDBCommandWorker.IsBusy) { logDBCommandWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "logDBCommandWorker", false);
            }
        }

        /// <summary>
        /// 执行后台线程对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteBackgroundWorkers(object sender, EventArgs e)
        {
            try
            {
                if (!dbCommandWorker.IsBusy) { dbCommandWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "dbCommandWorker BUG :" + ex.ToString());
            }
            try
            {
                if (!clientsWorker.IsBusy) { clientsWorker.RunWorkerAsync(0); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "clientsWorker BUG :" + ex.ToString());
            }

            try
            {
                if (!spriteDBWorker.IsBusy) { spriteDBWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "spriteDBWorker BUG :" + ex.ToString());
            }

            try
            {
                if (!othersWorker.IsBusy) { othersWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "othersWorker BUG :" + ex.ToString());
            }

            try
            {
                if (!dbWriterWorker.IsBusy) { dbWriterWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "dbWriterWorker BUG :" + ex.ToString());
            }
            try
            {
                if (!SocketSendCacheDataWorker.IsBusy) { SocketSendCacheDataWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "SocketSendCacheDataWorker BUG :" + ex.ToString());
            }

            try
            {
                if (!socketCheckWorker.IsBusy) { socketCheckWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, "socketCheckWorker BUG :" + ex.ToString());
            }

            try
            {
                if (!chatMsgWorker.IsBusy) { chatMsgWorker.RunWorkerAsync(); }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "chatMsgWorker", false);
            }

            if (WebAPIService != null)
            {
                try
                {
                    if (!WebAPIService.IsBusy) { WebAPIService.RunWorkerAsync(); }
                }
                catch (Exception ex)
                {
                    DataHelper.WriteFormatExceptionLog(ex, "WebAPIService", false);
                }
            }


            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);

            LogManager.WriteLog(LogTypes.Alert, "WorkerThreads: " + workerThreads + ", CompletionPortThreads: " + completionPortThreads);

            if (completionPortThreads < 950)
            {
                long ticks = TimeUtil.NOW();

                if (ticks - LastTelegramSpam > 5000)
                {
                    LastTelegramSpam = ticks;

                    Utils.SendTelegram("[S" + GameManager.ServerLineID + "] CompletionPortThreads: " + completionPortThreads);
                }
            }


            CalcGCInfo();
        }

        private long LastTelegramSpam = TimeUtil.NOW();


        /// <summary>
        /// 原来的 closingTimer_Tick(object sender, EventArgs e)
        /// 显示关闭信息的计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closingTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string title = "";

                //关闭角色
                KPlayer client = GameManager.ClientMgr.GetRandomClient();
                if (null != client)
                {
                    /**/
                    title = string.Format("GameServer {0} is closing, total {1} clients left.", GameManager.ServerLineID, GameManager.ClientMgr.GetClientCount());
                    Global.ForceCloseClient(client, "GameServer force close", true);
                }
                else
                {
                    //关闭倒计时
                    ClosingCounter -= 200;

                    //判断DB的命令队列是否已经执行完毕?
                    if (ClosingCounter <= 0)
                    {
                        //不再发送数据
                        Global._SendBufferManager.Exit = true;

                        //是否立刻关闭
                        MustCloseNow = true;

                        //程序主窗口
                        //GameManager.AppMainWnd.Close();
                        //Window_Closing();//没必要调用
                    }
                    else
                    {
                        int counter = GameManager.DBCmdMgr.GetDBCmdCount() + (ClosingCounter / 200);
                        /**/
                        title = string.Format("Server {0} is shutting down, Time left: {1}", GameManager.ServerLineID, counter);
                    }
                }

                //设置标题
                Console.Title = title;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                {
                    // 格式化异常错误信息
                    DataHelper.WriteFormatExceptionLog(ex, "closingTimer_Tick", false);
                    //throw ex;
                }//);
            }
        }

        private long LastAuxiliaryTicks = TimeUtil.NOW();

        /// <summary>
        /// 怪物Ai攻击索引
        /// </summary>
        //private int IndexOfMonsterAiAttack = 0;

        /// <summary>
        /// 计时器函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void auxiliaryTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                long ticks = TimeUtil.NOW();

                if (ticks - LastAuxiliaryTicks > 1000)
                {
                    DoLog(String.Format("\r\nAuxiliaryTimer took much times to process: {0}ms", ticks - LastAuxiliaryTicks));
                }

                LastAuxiliaryTicks = ticks;

                ticks = TimeUtil.NOW();
                Global._TCPManager.tcpClientPool.Supply();
                if (TimeUtil.NOW() - ticks >= 500)
                {
                    LogManager.WriteLog(LogTypes.Error, "AuxiliaryTimer - Tick 1 took too much times: " + (TimeUtil.NOW() - ticks));
                }

                ticks = TimeUtil.NOW();
                Global._TCPManager.tcpLogClientPool.Supply();
                if (TimeUtil.NOW() - ticks >= 500)
                {
                    LogManager.WriteLog(LogTypes.Error, "AuxiliaryTimer - Tick 2 took too much times: " + (TimeUtil.NOW() - ticks));
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// 记录日志，控制台打印或者记录到文件
        /// </summary>
        /// <param name="warning"></param>
        private void DoLog(String warning)
        {
            LogManager.WriteLog(LogTypes.Error, warning);
        }

        /// <summary>
        /// 后台主调度线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        public long LastReport = 0;

        private void MainDispatcherWorker_DoWork(object sender, EventArgs e)
        {
            long lastTicks = TimeUtil.NOW();
            long startTicks = TimeUtil.NOW();
            long endTicks = TimeUtil.NOW();

            //睡眠时间
            int maxSleepMs = 100;
            int sleepMs = 100;

            int nTimes = 0;

            while (true)
            {
                try
                {
                    startTicks = TimeUtil.NOW();

                    //if (nTimes % 5 == 0)
                    if (startTicks - lastTicks >= 500)
                    {
                        GameManager.GM_NoCheckTokenTimeRemainMS -= startTicks - lastTicks;
                        lastTicks = startTicks;

                        //辅助调度--->500毫秒执行一次
                        auxiliaryTimer_Tick(null, null);
                    }

                    if (startTicks - LastReport >= 5000)
                    {
                        LastReport = startTicks;
                        LogManager.WriteLog(LogTypes.TimerReport, "MainDispatcherWorker_DoWork ===> LIVE");
                    }

                    if (NeedExitServer)
                    {
                        //调度关闭操作--->原来200毫秒执行一次
                        closingTimer_Tick(null, null);

                        //关闭完毕，自己也该退出了
                        if (MustCloseNow)
                        {
                            break;
                        }
                    }

                    endTicks = TimeUtil.NOW();

                    //最多睡眠100毫秒，最少睡眠1毫秒
                    sleepMs = (int)Math.Max(5, maxSleepMs - (endTicks - startTicks));

                    Thread.Sleep(sleepMs);

                    nTimes++;

                    if (nTimes >= 100000)
                    {
                        nTimes = 0;
                    }
                    if (0 != GetServerPIDFromFile())
                    {
                        OnExitServer();
                    }
                }
                catch (Exception ex)
                {
                    DataHelper.WriteFormatExceptionLog(ex, "MainDispatcherWorker_DoWork", false);
                }
            }

            if (0 != GetServerPIDFromFile())
            {
                // 结束时将进程ID写入文件
                WritePIDToFile("Stop.txt");

                StopThreadPoolDriverTimer();
            }
        }

        /// <summary>
        /// Xử lý đóng các client bị delay packet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbCommandWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GameManager.DBCmdMgr.ExecuteDBCmd(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "dbCommandWorker_DoWork", false);
            }
        }

        /// <summary>
        /// Xử lý lệnh với LOGS server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logDBCommandWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GameManager.logDBCmdMgr.ExecuteDBCmd(Global._TCPManager.tcpLogClientPool, Global._TCPManager.TcpOutPacketPool);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "logDBCommandWorker_DoWork", false);
            }
        }

        /// <summary>
        /// 角色和包裹后台工作函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientsWorker_DoWork(object sender, EventArgs e)
        {
            DoWorkEventArgs de = e as DoWorkEventArgs;

            try
            {
                GameManager.ClientMgr.DoSpriteBackgourndWork(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, string.Format("clientsWorker_DoWork{0}", (int)de.Argument), false);
            }
        }

        /// <summary>
        /// 角色DB后台工作函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spriteDBWorker_DoWork(object sender, EventArgs e)
        {
            try
            {
                
                GameManager.ClientMgr.DoSpriteDBWork(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, string.Format("spriteDBWorker_DoWork"), false);
            }
        }

        /// <summary>
        /// Other Work quản lý toàn bộ vật phẩm | Item rơi trên map,bản tin ,Fake Role,Tiêu vv....| quản lý toàn bộ các sự kiện theo thởi gian
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void othersWorker_DoWork(object sender, EventArgs e)
        {
            //long ticksA = TimeUtil.NOW();

            /// Xử lý vật phẩm drop xuống đất
            GameManager.GoodsPackMgr.ProcessAllGoodsPackItems(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool);

            // Xử lý các sự kiện liên quan tới hàm đợi khi login
            GameManager.loginWaitLogic.Tick();

            // Tự điều chỉnh hiệu năng server
            TimeUtil.RecordTimeAnchor();

            // REPORT HIỆU NĂNG MÁY CHUHR TỚI GM TOOl
            // GameManager.ServerMonitor.CheckReport();

            //// XỬ LÝ CÁC TRƯỜNG HỢP KHÓA TRADE
            //TradeBlackManager.Instance().Update();

            //long ticksB = TimeUtil.NOW();

            //if (ticksB > ticksA + 1000)
            //{
            //    DoLog(String.Format("othersWorker_DoWork 消耗:{0}毫秒", ticksB - ticksA));
            //}
        }

        private void chatMsgWorker_DoWork(object sender, EventArgs e)
        {
            try
            {
                long ticksA = TimeUtil.NOW();

                BanChatManager.GetBanChatDictFromDBServer();

                GameManager.ClientMgr.HandleTransferChatMsg();

                long ticksB = TimeUtil.NOW();

                if (ticksB > ticksA + 1000)
                {
                    DoLog(String.Format("chatMsgWorker_DoWork 消耗:{0}毫秒", ticksB - ticksA));
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "chatMsgWorker_DoWork", false);
            }
        }

        /// <summary>
        /// Thực hiện ghi vào DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbWriterWorker_DoWork(object sender, EventArgs e)
        {
            try
            {
                long ticks = TimeUtil.NOW();
                if (ticks - LastWriteDBLogTicks < (30 * 1000))
                {
                    return;
                }

                LastWriteDBLogTicks = ticks;

                Global._TCPManager.MySocketListener.ClearTimeoutSocket();

            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "dbWriterWorker_DoWork", false);
            }
        }

        /// <summary>
        /// 客户端套接字发送数据线程后台工作函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketSendCacheDataWorker_DoWork(object sender, EventArgs e)
        {
            try
            {
                Global._SendBufferManager.TrySendAll();
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "SocketFlushBuffer_DoWork", false);
            }
        }

        ///// <summary>
        ///// Worker xử lý Update vị trí người chơi (TOÁC VKL)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Gird9UpdateWorker_DoWork(object sender, EventArgs e)
        //{
        //    DoWorkEventArgs de = e as DoWorkEventArgs;

        //    while (!NeedExitServer)
        //    {
        //        try
        //        {
        //            GameManager.ClientMgr.DoSpritesMapGridMove((int)de.Argument);
        //            Thread.Sleep(100);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.WriteLog(LogTypes.Exception, ex.ToString());
        //        }
        //    }

        //    SysConOut.WriteLine(string.Format("Update role grid worker ID {0} was terminated...", (int)de.Argument));
        //}

        private long LastSocketCheckTicks = TimeUtil.NOW();

        /// <summary>
        /// socket检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketCheckWorker_DoWork(object sender, EventArgs e)
        {
            try
            {
                long now = TimeUtil.NOW();
                //if (now - LastSocketCheckTicks < (1 * 60 * 1000))
                if (now - LastSocketCheckTicks < (5 * 60 * 1000))
                    return;

                LastSocketCheckTicks = now;

                //int timeCount = 1 * 60 * 1000;
                int timeCount = 15 * 60 * 1000;
                List<TMSKSocket> socketList = GameManager.OnlineUserSession.GetSocketList();
                foreach (TMSKSocket socket in socketList)
                {
                    long nowSocket = TimeUtil.NOW();
                    long spanSocket = nowSocket - socket.session.SocketTime[0];
                    if (socket.session.SocketState < 4 && spanSocket > timeCount)
                    {
                        KPlayer otherClient = GameManager.ClientMgr.FindClient(socket);
                        if (null == otherClient)
                            Global.ForceCloseSocket(socket, "被GM踢了, 但是这个socket上没有对应的client");
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "SocketCheckWorker_DoWork", false);
            }
        }

        #endregion 线程部分

        /// <summary>
        /// 退出程序
        /// 原来的Window_Closing(object sender, CancelEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing()
        {
            //是否立刻关闭
            if (MustCloseNow)
            {
                return;
            }

            //已经进入了关闭模式
            if (EnterClosingMode)
            {
                return;
            }

            //是否进入了关闭模式
            EnterClosingMode = true;

            //设置不再接受新的请求，就是接受到后，立刻关闭
            //是否不再接受新的用户
            Global._TCPManager.MySocketListener.DontAccept = true;

            LastWriteDBLogTicks = 0; //强迫写缓存

            //设置退出标志
            NeedExitServer = true;
        }

        #endregion 游戏服务器具体功能部分

        #region 获取编译日期

        /// <summary>
        /// 获取程序的编译日期
        /// </summary>
        /// <returns></returns>
        public static string GetVersionDateTime()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            int revsion = assembly.GetName().Version.Revision;//获取修订号
            int build = assembly.GetName().Version.Build;//获取内部版本号
            DateTime dtbase = new DateTime(2000, 1, 1, 0, 0, 0);//微软编译基准时间
            TimeSpan tsbase = new TimeSpan(dtbase.Ticks);
            TimeSpan tsv = new TimeSpan(tsbase.Days + build, 0, 0, revsion * 2);//编译时间，注意修订号要*2
            DateTime dtv = new DateTime(tsv.Ticks);//转换成编译时间
                                                   //return dtv.ToString("yyyy-MM-dd HH") + string.Format(" {0}", AssemblyFileVersion.FilePrivatePart);

            string version = "0.0";
            return dtv.ToString("yyyy-MM-dd_HH") + string.Format("_{0}", version);
        }

        #endregion 获取编译日期
    }
}