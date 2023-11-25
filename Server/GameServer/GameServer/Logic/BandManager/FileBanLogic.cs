using GameServer.Core.Executor;
using GameServer.Server;
using Server.Data;
using Server.TCP;
using Server.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tmsk.Contract;

namespace GameServer.Logic
{
    internal enum MagicCrashUnityType
    {
        // 客户端执行hook检测，如果检测到hook注入，则隔一段时间退出客户端
        DetectHook = 1,

        // 客户端隔一段时候后，强制退出
        CrashTimeOut = 2,
    }

    // 单线程执行 不保证线程安全
    internal class FileBanLogic
    {
        // 待查封列表
        private static List<string> m_BanList = new List<string>();

        // 扫描文件时间戳
        private static long m_UpdateTicks = 0;

        // 清除的标记
        private static int m_IsNeedClear = 0;

        private static void LoadBanFile()
        {
            // 每分钟扫描一次
            long currTicks = TimeUtil.NOW();

            if (currTicks - m_UpdateTicks < 10 * 1000)
            {
                return;
            }

            m_UpdateTicks = currTicks;

            //检查新增文件
            var files = from file in
                            Directory.EnumerateFiles(DataHelper.CurrentDirectory, "Ban*.txt", SearchOption.AllDirectories)
                        select file;

            foreach (var file in files)
            {
                FileStream fs = null;
                try
                {
                    //先尝试锁住文件
                    fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch
                {
                    fs = null;
                }

                if (null == fs)
                {
                    continue;
                }

                StreamReader sr = new StreamReader(fs, Encoding.Default);

                string strLine = "";
                while (null != (strLine = sr.ReadLine()))
                {
                    string[] userid = strLine.Split(',');

                    if (userid.Length <= 0)
                    {
                        continue;
                    }

                    m_BanList.Add(userid[0]);
                }

                sr.Close();
                fs.Close();

                FileInfo fi = new FileInfo(file);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                {
                    fi.Attributes = FileAttributes.Normal;
                }

                File.Delete(file);
            }
        }

        public static void Tick()
        {
            LoadBanFile();

            if (null == m_BanList)
            {
                return;
            }

            // 如果需要清除 就清除掉
            if (m_IsNeedClear > 0)
            {
                m_BanList.Clear();
                m_IsNeedClear = 0;
            }

         
            int i = 20;
            while (i > 0 && m_BanList.Count > 0)
            {
                i--;

                string userID = m_BanList[m_BanList.Count - 1];
                m_BanList.RemoveAt(m_BanList.Count - 1);

                BanManager.BanUserID2Memory(userID);

                TMSKSocket clientSocket = GameManager.OnlineUserSession.FindSocketByUserID(userID);

                if (null == clientSocket)
                {
                    continue;
                }

                KPlayer gameClient = GameManager.ClientMgr.FindClient(clientSocket);

                // 封号删除
                if (null != gameClient)
                {
                    RoleData roleData = new RoleData() { RoleID = StdErrorCode.Error_70 };
                    gameClient.SendPacket((int)TCPGameServerCmds.CMD_INIT_GAME, roleData);

            

                    // log it!
                    LogManager.WriteLog(LogTypes.FileBan, string.Format("FileBanLogic ban2 userID={0} roleID={1}", userID, gameClient.RoleID));
                }
                else
                {
                    // log it!
                    //关闭连接
                    Global.ForceCloseSocket(clientSocket, "被禁止登陆");
                    LogManager.WriteLog(LogTypes.FileBan, string.Format("FileBanLogic ForceCloseSocket userID={0}", userID));
                }
            }
        }

        public static void ClearBanList()
        {
            m_IsNeedClear = 1;
        }

        public static void BroadCastDetectHook()
        {
            int index = 0;
            KPlayer client = null;
            while ((client = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                if (client == null) continue;

                /*
                if (client.ClientSocket.IsKuaFuLogin)
                {
                    continue;
                }*/

                SendMagicCrashMsg(client, MagicCrashUnityType.DetectHook);
            }
        }

        public static void BroadCastDetectHook(List<string> uidList)
        {
            if (uidList == null) return;

            foreach (var uid in uidList)
            {
                TMSKSocket clientSocket = GameManager.OnlineUserSession.FindSocketByUserID(uid);

                if (null == clientSocket)
                {
                    continue;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(clientSocket);
                if (client == null) continue;

                SendMagicCrashMsg(client, MagicCrashUnityType.DetectHook);
            }
        }

        private static void SendMagicCrashMsg(KPlayer client, MagicCrashUnityType crashType)
        {
            if (client == null) return;

            int timeOutSec = Global.GetRandomNumber(5, 15);
            string cmd = string.Format("{0}:{1}", (int)crashType, timeOutSec);
            client.SendPacket((int)TCPGameServerCmds.CMD_NTF_MAGIC_CRASH_UNITY, cmd);
        }
    }
}