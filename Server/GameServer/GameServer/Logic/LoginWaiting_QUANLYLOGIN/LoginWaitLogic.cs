using GameServer.Core.Executor;
using GameServer.Logic.Name;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Logic.LoginWaiting
{
    public class LoginWaitLogic
    {
        #region 配置相关
        public enum ConfigType
        {
            NeedWaitNum = 0,    // 需要排队的人数
            MaxServerNum = 1,   // 服务器最大人数
            MaxQueueNum = 2,    // 等待队列的最大数
            WaitUpdateInt = 3,  // 多久进一个人
            AllowMSeconds = 4,   // 排队后允许直接进入的时间
            LogouAllowMSeconds = 5,// 客户端登出后允许直接进入的时间
        }

        public enum UserType
        {
            Normal = 0,
            Vip = 1,
            Max_Type = 2,
        }

        private int[][] m_IntConfig = new int[(int)UserType.Max_Type][];

        // 从systemparams.xml里加载
        public void LoadConfig()
        {
            string strCfg = "1300,1500,400,30000,180000,20000";
            m_IntConfig[(int)UserType.Normal] = Global.String2IntArray(strCfg);
            strCfg = "1300,1500,400,30000,180000,20000";
            m_IntConfig[(int)UserType.Vip] = Global.String2IntArray(strCfg);
        }

        // 取得一个配置
        public int GetConfig(UserType userType, ConfigType type)
        {
            if (userType < UserType.Normal || userType >= UserType.Max_Type)
            {
                return 0;
            }
            if ((int)type < 0 || (int)type >= m_IntConfig[(int)userType].Length)
            {
                return 0;
            }

            return m_IntConfig[(int)userType][(int)type];
        }

        private int m_UserUpdateInt = 5 * 1000;

        #endregion

        #region 等待队列

        public class UserInfo
        {
            public TMSKSocket socket = null;
            public string userID = "";
            public int zoneID = 0;
            public long startTick = 0;  // 进入队列的时间
            public long updateTick = 0; // 更新时间戳
            public long firstTick = 0;  // 成为第一位的时间
            public long overTick = 0;   // 预计的能进入去时间点
        }

        // 等待登陆的玩家队列
        private List<UserInfo>[] m_UserList = new List<UserInfo>[(int)UserType.Max_Type];

        // userid
        private Dictionary<string, TMSKSocket> m_User2SocketDict = new Dictionary<string, TMSKSocket>();

        private object m_Mutex = new object();

        private long m_UpdateTick = 0;

        private long m_UpdateAllowTick = 0;

        private long m_LastEnterSecs = 30;

        private long m_LastEnterFromFirstSecs = 30;

        public LoginWaitLogic()
        {
            for (UserType i = UserType.Normal; i < UserType.Max_Type; i++)
            {
                m_UserList[(int)i] = new List<UserInfo>();
            }
        }

        // 取得正在排队玩家的总数
        public int GetTotalWaitingCount()
        {
            lock (m_Mutex)
            {
                int nCount = 0;
                foreach (var list in m_UserList)
                {
                    nCount += list.Count;
                }
                return nCount;
            }
        }

        // 取得不同类型排队玩家的总数
        public int GetWaitingCount(UserType userType)
        {
            lock (m_Mutex)
            {
                return m_UserList[(int)userType].Count;
            }
        }

        public bool IsInWait(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    //LogManager.WriteLog(LogTypes.Error, string.Format("LoginWaitLogic::IsInWait userID={0}", null == userID ? "null " : userID));
                    return false;
                }

                lock (m_Mutex)
                {
                    return m_User2SocketDict.ContainsKey(userID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::IsInWait userID={0}", userID));
                return false;
            }
        }

        // 添加进一个玩家进入排队列表
        public bool AddToWait(string userID, int zoneID, UserType userType, TMSKSocket socket)
        {
            try
            {
                lock (m_Mutex)
                {
                    if (IsInWait(userID))
                    {
                        return false;
                    }

                    // 超过队列上限
                    if (GetWaitingCount(userType) >= GetConfig(userType, ConfigType.MaxQueueNum))
                    {
                        return false;
                    }

                    m_UserList[(int)userType].Add(new UserInfo() { userID = userID, zoneID = zoneID, socket = socket, startTick = TimeUtil.NOW(), updateTick = 0 });
                    m_User2SocketDict.Add(userID, socket);

                    //SysConOut.WriteLine(string.Format("user {0}进入排队列表", userID));
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::AddToWait userID={0}", userID));
                return false;
            }

            return true;
        }

        public void RemoveWait(string userID)
        {
            try
            {
                lock (m_Mutex)
                {
                    if (!IsInWait(userID))
                    {
                        return;
                    }
                    foreach (var list in m_UserList)
                    {
                        list.RemoveAll((x) => { return x.userID == userID; });
                    }
                   
                    m_User2SocketDict.Remove(userID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::RemoveWait userID={0}", userID));
            }
        }

        // 第一名的信息
        public UserInfo TopWaiting(UserType userType)
        {
            UserInfo userInfo = null;

            try
            {
                lock (m_Mutex)
                {
                    if (GetWaitingCount(userType) <= 0)
                    {
                        return null;
                    }

                    userInfo = m_UserList[(int)userType][0];
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::TopWaiting"));
            }

            return userInfo;
        }

        // 从队列头搞一个玩家出来
        public UserInfo PopTopWaiting(UserType userType)
        {
            UserInfo userInfo = null;

            try
            {
                lock (m_Mutex)
                {
                    if (GetWaitingCount(userType) <= 0)
                    {
                        return null;
                    }

                    userInfo = m_UserList[(int)userType][0];
                    m_UserList[(int)userType].RemoveAt(0);
                    m_User2SocketDict.Remove(userInfo.userID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::PopTopWaiting"));
            }

            return userInfo;
        }

        // 输出队列玩家信息
        public void OutWaitInfo(UserType userType, int index)
        {
            UserInfo userInfo = null;
            try
            {
                lock (m_Mutex)
                {
                    if (index < 0 || index >= GetWaitingCount(userType))
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("OutWaitInfo Index Was Outside "));
                        return;
                    }
                    userInfo = m_UserList[(int)userType][index];
                    LogManager.WriteLog(LogTypes.Error, string.Format("OutWaitInfo:userID={0} zoneID={1} startTick={2} updateTick={3} firstTick={4} overTick={5}",
                        userInfo.userID, userInfo.zoneID, userInfo.startTick, userInfo.updateTick, userInfo.firstTick, userInfo.overTick));
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::PopTopWaiting"));
            }
        }

        #endregion

        #region 允许进入玩家列表

        // 免排队玩家列表
        private Dictionary<string, long> m_AllowUserDict = new Dictionary<string, long>();

        // 取得有多少人在允许列表
        public int GetAllowCount()
        {
            lock (m_AllowUserDict)
            {
                return m_AllowUserDict.Count;
            }
        }

        // 增加到允许列表
        public bool AddToAllow(string userID, int mSeconds)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    //LogManager.WriteLog(LogTypes.Error, string.Format("LoginWaitLogic::AddToAllow userID={0}", null == userID ? "null " : userID));
                    return false;
                }

                lock (m_AllowUserDict)
                {
                    if (IsInAllowDict(userID))
                    {
                        m_AllowUserDict[userID] = TimeUtil.NOW() + mSeconds;
                        return true;
                    }
                    else
                    {
                        if (GetAllowCount() < GetConfig(UserType.Normal, ConfigType.MaxServerNum))
                        {
                            m_AllowUserDict[userID] = TimeUtil.NOW() + mSeconds;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::AddToAllow userID={0}", userID));
                return false;
            }

            return false;
        }

        // 从允许列表删除
        public void RemoveAllow(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    //LogManager.WriteLog(LogTypes.Error, string.Format("LoginWaitLogic::RemoveAllow userID={0}", null == userID ? "null " : userID));
                    return;
                }

                lock (m_AllowUserDict)
                {
                    m_AllowUserDict.Remove(userID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::RemoveAllow userID={0}", userID));
            }
        }

        // 是否在允许列表
        public bool IsInAllowDict(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("LoginWaitLogic::IsInAllowDict userID={0}", null == userID ? "null " : userID));
                    return false;
                }

                lock (m_AllowUserDict)
                {
                    return m_AllowUserDict.ContainsKey(userID);
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::IsInAllowDict userID={0}", userID));
                return false;
            }
        }

        #endregion

        // 通知玩家在线信息
        public void NotifyWaitingInfo(UserInfo userInfo, int count, long seconds)
        {
            try
            {
                if (null == userInfo)
                {
                    return;
                }

                if (null == userInfo.socket || !userInfo.socket.Connected)
                {
                    return;
                }

                string strcmd = string.Format("{0}:{1}", count, seconds);
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strcmd, (int)TCPGameServerCmds.CMD_SPR_LOGIN_WAITING_INFO);
                Global._TCPManager.MySocketListener.SendData(userInfo.socket, tcpOutPacket);
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::NotifyWaitingInfo userID={0} zoneID={1}", userInfo.userID, userInfo.zoneID));
            }
        }

        // 允许玩家进入
        public bool NotifyUserEnter(UserInfo userInfo)
        {
            try
            {
                if (null == userInfo)
                {
                    return true;
                }

                if (null == userInfo.socket || !userInfo.socket.Connected)
                {
                    return true;
                }

                // 加进免排队列表
                AddToAllow(userInfo.userID, GetConfig(UserType.Normal, ConfigType.AllowMSeconds));

                //if (!userInfo.socket.IsKuaFuLogin)
                //{
                //    ChangeNameInfo info = NameManager.Instance().GetChangeNameInfo(userInfo.userID, userInfo.zoneID, userInfo.socket.ServerId);
                //    if (info != null)
                //    {
                //        Global._TCPManager.MySocketListener.SendData(userInfo.socket, DataHelper.ObjectToTCPOutPacket(info, Global._TCPManager.TcpOutPacketPool, (int)TCPGameServerCmds.CMD_NTF_EACH_ROLE_ALLOW_CHANGE_NAME));
                //    }
                //}

            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::NotifyUserEnter userID={0} zoneID={1}", userInfo.userID, userInfo.zoneID));
                return false;
            }

            string strData = "";

            // 替玩家向db请求角色列表
            try
            {
                string strcmd = string.Format("{0}:{1}", userInfo.userID, userInfo.zoneID);
                byte[] bytesData = Global.SendAndRecvData<string>((int)TCPGameServerCmds.CMD_ROLE_LIST, strcmd, userInfo.socket.ServerId);

                string[] fieldsData = null;
                Int32 length = BitConverter.ToInt32(bytesData, 0);
                strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::NotifyUserEnter 向db请求角色列表 faild！ userID={0} zoneID={1}", userInfo.userID, userInfo.zoneID));
                //return false;
                // 获取异常则通知客户端重新排队，防止客户端始终卡在那里
                strData = "-1:";
            }

            try
            {
                //SysConOut.WriteLine(strData);

                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, strData, (int)TCPGameServerCmds.CMD_ROLE_LIST);
                Global._TCPManager.MySocketListener.SendData(userInfo.socket, tcpOutPacket);

                m_LastEnterSecs = (TimeUtil.NOW() - userInfo.startTick) / 1000;
                m_LastEnterFromFirstSecs = (TimeUtil.NOW() - userInfo.firstTick) / 1000;
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::NotifyUserEnter 发送角色列表Faild userID={0} zoneID={1}", userInfo.userID, userInfo.zoneID));
                return false;
            }

            return true;
        }

        public UserType GetUserType(string userID)
        {
            LoginWaitLogic.UserType userType = LoginWaitLogic.UserType.Normal;

            try
            {
               
                if (GameManager.ClientMgr.QueryTotaoChongZhiMoney(userID, -1, -1) >= 50000)
                {
                    userType = LoginWaitLogic.UserType.Vip;
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, "LoginWaitLogic::GetUserType Exception!!!");
                return userType;
            }

            return userType;
        }

        // 当前玩家数和占坑数
        public int GetUserCount()
        {
            return GameManager.ClientMgr.GetClientCount() + GetAllowCount();
        }

        // 定时删除超时玩家 
        public void UpdateWaitingList()
        {
            long currTick = TimeUtil.NOW();

            try
            {
                // 删除掉无连接的玩家 以防万一
                lock (m_Mutex)
                {
                    foreach (var list in m_UserList)
                    {
                        list.RemoveAll((x) => { return null == x.socket || !x.socket.Connected; });
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::ProcessWaitingList 1"));
            }

            try
            {
                lock (m_AllowUserDict)
                {
                    List<string> removeList = new List<string>();
                    foreach (var userInfo in m_AllowUserDict)
                    {
                        if (currTick > userInfo.Value)
                        {
                            if (null == removeList)
                                removeList = new List<string>();
                            removeList.Add(userInfo.Key);
                        }
                    }
                    if (null == removeList || removeList.Count <= 0)
                    {
                        return;
                    }
                    foreach (string userID in removeList)
                    {
                        m_AllowUserDict.Remove(userID);
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::ProcessWaitingList 2"));
            }
        }

        public void Tick()
        {
            long currTick = TimeUtil.NOW();

            // 一秒一次
            if (currTick - m_UpdateAllowTick < 1 * 1000)
            {
                return;
            }

            m_UpdateAllowTick = currTick;

            // 剔除掉允许进入但是超时不进入的玩家
            UpdateWaitingList();

            /*for (UserType i = UserType.Normal; i < UserType.Max_Type; i++)
            {
                ProcessWaitingList(i);
            }*/
            ProcessWaitingList(UserType.Vip);
            ProcessWaitingList(UserType.Normal);
        }

        public void ProcessWaitingList(UserType userType)
        {
            try
            {
                long currTick = TimeUtil.NOW();

                // 没人排队不处理
                if (GetWaitingCount(userType) <= 0)
                {
                    return;
                }

                int currClientCount = GetUserCount();

                // 遍历当前队列，给玩家发送排队信息
                lock (m_Mutex)
                {
                    int i = 0;
                    long lastOverTick = 0;
                    long firstWaitSeconds = 0;
                    foreach (UserInfo userInfo in m_UserList[(int)userType])
                    {
                        i++;

                        // 记录玩家成为第一名的时间
                        if (1 == i && 0 == userInfo.firstTick)
                        {
                            userInfo.firstTick = TimeUtil.NOW();
                        }

                        long leftSeconds = 0;
                        // 在虚拟队列里
                        if (currClientCount + i <= GetConfig(userType, ConfigType.MaxServerNum))
                        //if (false)
                        {
                            //leftSeconds = i * m_LastEnterSecs;
                            if (0 == userInfo.overTick)
                            {
                                userInfo.overTick = ((0 == lastOverTick) ? currTick : lastOverTick) + GetConfig(userType, ConfigType.WaitUpdateInt);
                            }
                            leftSeconds = Global.GMax(1, (userInfo.overTick - currTick) / 1000);

                            // 限制一下
                            leftSeconds = Global.GMin(leftSeconds, GetConfig(userType, ConfigType.WaitUpdateInt) / 1000 * i);

                            lastOverTick = userInfo.overTick;
                        }
                        else
                        {
                            if (1 == i)
                            {
                                // 从上一个人等待的时间和自己等待的时间选一个最大的时间提示
                                leftSeconds = Global.GMax(m_LastEnterFromFirstSecs, (TimeUtil.NOW() - userInfo.firstTick) / 1000);
                                // 防止有的人在第一名一瞬间就进去了 导致计算了一个0 导致后面的人都显示小于1分钟
                                leftSeconds = Global.GMax(1, leftSeconds);
                                firstWaitSeconds = leftSeconds;
                            }
                            else
                            {
                                leftSeconds = i * firstWaitSeconds;//m_LastEnterSecs;
                            }
                        }

                        //SysConOut.WriteLine(string.Format("notify {0} 第{1}位 还有{2}秒", userInfo.userID, i, leftSeconds));

                        //每个玩家五秒钟通知一次
                        if (currTick - userInfo.updateTick <= m_UserUpdateInt)
                        {
                            lastOverTick = userInfo.overTick;
                            continue;
                        }

                        userInfo.updateTick = currTick;

                        NotifyWaitingInfo(userInfo, i, leftSeconds);
                    }
                }

                // 如果在线人数小于800 那么立刻往里放人
                if (currClientCount < GetConfig(userType, ConfigType.NeedWaitNum))
                {
                    // 每次往里放x个
                    for (int i = 0; i < 5; i++)
                    {
                        UserInfo userInfo = PopTopWaiting(userType);
                        if (null == userInfo)
                        {
                            continue;
                        }

                        //SysConOut.WriteLine(string.Format("notify {0} 登陆", userInfo.userID));
                        // 允许他们登陆
                        NotifyUserEnter(userInfo);
                    }
                }
                // 如果玩家在800-1000之间
                else if (currClientCount < GetConfig(userType, ConfigType.MaxServerNum))
                {
                    // 允许一个人登陆
                    UserInfo userInfo = TopWaiting(userType);
                    if (null == userInfo)
                    {
                        return;
                    }

                    if (userInfo.overTick <= 0)
                    {
                        return;
                    }

                    // 到时间了
                    if (currTick < userInfo.overTick)
                    {
                        return;
                    }

                    //m_UpdateTick = currTick;

                    userInfo = PopTopWaiting(userType);

                    //SysConOut.WriteLine(string.Format("notify {0} 登陆", userInfo.userID));
                    NotifyUserEnter(userInfo);
                    return;
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteExceptionLogEx(ex, string.Format("LoginWaitLogic::Tick"));
            }
        }
    }
}
