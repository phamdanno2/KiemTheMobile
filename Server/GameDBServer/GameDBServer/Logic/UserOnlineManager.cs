using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 用户在线管理
    /// </summary>
    public class UserOnlineManager
    {
        #region 基础数据

        /// <summary>
        /// 角色在线词典定义
        /// </summary>
        private static Dictionary<string, int> _RegUserIDDict = new Dictionary<string, int>();

        #endregion 基础数据

        #region 操作方法和函数

        /// <summary>
        /// 获取指定的用户在线的服务器线路ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static int GetUserOnlineState(string userID)
        {
            int state = 0;
            int oldServerLineID = -1;
            lock (_RegUserIDDict)
            {
                if (!_RegUserIDDict.TryGetValue(userID, out oldServerLineID))
                {
                    oldServerLineID = -1;
                }
            }

            if (oldServerLineID <= -1) return state;

            //判断是否服务器已经当掉
            if (LineManager.GetLineHeartState(oldServerLineID) > 0)
            {
                state = 1;
            }

            return state;
        }

        /// <summary>
        /// 添加用户ID到字典中
        /// </summary>
        /// <param name="roleID"></param>
        public static bool RegisterUserID(string userID, int serverLineID, int state)
        {
            bool ret = true;
            lock (_RegUserIDDict)
            {
                int oldServerLineID = -1;
                if (state <= 0) //注销, 下边代码加入限制，防止错误的注销，导致的重复登录。
                {
                    //注册前先判断是否已经存在
                    if (_RegUserIDDict.TryGetValue(userID, out oldServerLineID))
                    {
                        //判断是否服务器ID是否相同
                        if (oldServerLineID == serverLineID)
                        {
                            _RegUserIDDict.Remove(userID);
                        }
                    }
                }
                else //注册
                {
                    //注册前先判断是否已经存在
                    if (_RegUserIDDict.TryGetValue(userID, out oldServerLineID))
                    {
                        //判断是否服务器已经当掉
                        if (LineManager.GetLineHeartState(oldServerLineID) > 0)
                        {
                            // 这里加日志，是为了确定打完跨服偶尔会登陆不上原服务器，看看是为什么.
                            LogManager.WriteLog(LogTypes.Error, string.Format("账号 {0} 请求注册登录到 {1} 线，但是该账号已经被注册到 {2} 线", userID, serverLineID, oldServerLineID));

                            ret = false; //已经在线
                        }
                        else
                        {
                            _RegUserIDDict[userID] = serverLineID;
                        }
                    }
                    else
                    {
                        _RegUserIDDict[userID] = serverLineID;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 清空指定线路ID对应的所有用户数据
        /// </summary>
        /// <param name="serverLineID"></param>
        public static void ClearUserIDsByServerLineID(int serverLineID)
        {
            lock (_RegUserIDDict)
            {
                List<string> userIDsList = new List<string>();
                foreach (var userID in _RegUserIDDict.Keys)
                {
                    int oldServerLineID = _RegUserIDDict[userID];
                    if (oldServerLineID == serverLineID)
                    {
                        userIDsList.Add(userID);
                    }
                }

                for (int i = 0; i < userIDsList.Count; i++)
                {
                    _RegUserIDDict.Remove(userIDsList[i]);
                }
            }
        }

        #endregion 操作方法和函数
    }
}
