using System;
using System.Collections.Generic;

namespace GameServer.Logic
{
    /// <summary>
    /// 游戏配置对象
    /// </summary>
    public class GameConfig
    {
        #region 基础数据

        /// <summary>
        /// 公告字典
        /// </summary>
        private Dictionary<string, string> _GameConfigDict = new Dictionary<string, string>();

        #endregion 基础数据

        #region 基础方法

        /// <summary>
        /// 从数据库中获取配置参数
        /// </summary>
        public void LoadGameConfigFromDBServer()
        {
            //查询游戏配置参数
            //从DBserver加载配置参数
            _GameConfigDict = Global.LoadDBGameConfigDict();
            if (null == _GameConfigDict)
            {
                _GameConfigDict = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 设置游戏配置项
        /// </summary>
        public void SetGameConfigItem(string paramName, string paramValue)
        {
            lock (_GameConfigDict)
            {
                _GameConfigDict[paramName] = paramValue;
            }

            //当参数发生变化时通知
            ChangeParams(paramName, paramValue);
        }

        /// <summary>
        /// 更新服务器参数，如果当前值和目标值相同，则跳过写数据库，除非指定force参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="force"></param>
        public void UpdateGameConfigItem(string paramName, string paramValue, bool force = false)
        {
            lock (_GameConfigDict)
            {
                string oldValue;
                if (_GameConfigDict.TryGetValue(paramName, out oldValue))
                {
                    if (oldValue == paramValue && !force)
                    {
                        return;
                    }
                }
            }

            SetGameConfigItem(paramName, paramValue);

            //当参数发生变化时通知
            Global.UpdateDBGameConfigg(paramName, paramValue);
        }

        /// <summary>
        /// 在原有数值上修改游戏配置项
        /// </summary>
        public void ModifyGameConfigItem(string paramName, int paramValue)
        {
            int value = 0;
            lock (_GameConfigDict)
            {
                value = GetGameConfigItemInt(paramName, 0) + paramValue;
                _GameConfigDict[paramName] = value.ToString();
            }

            //当参数发生变化时通知
            ChangeParams(paramName, value.ToString());
        }

        /// <summary>
        /// 获取游戏配置项
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string GetGameConifgItem(string paramName)
        {
            string paramValue = null;
            lock (_GameConfigDict)
            {
                if (!_GameConfigDict.TryGetValue(paramName, out paramValue))
                {
                    paramValue = null;
                }
            }

            return paramValue;
        }

        /// <summary>
        /// 获取游戏字符串配置项
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string GetGameConfigItemStr(string paramName, string defVal)
        {
            string ret = GetGameConifgItem(paramName);
            if (string.IsNullOrEmpty(ret))
            {
                return defVal;
            }

            return ret;
        }

        /// <summary>
        /// 获取游戏整数配置项
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public int GetGameConfigItemInt(string paramName, int defVal)
        {
            string str = GetGameConifgItem(paramName);
            if (string.IsNullOrEmpty(str))
            {
                return defVal;
            }

            int ret = 0;

            try
            {
                ret = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                ret = defVal;
            }

            return ret;
        }

        /// <summary>
        /// 获取游戏浮点数配置项
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public double GetGameConfigItemDouble(string paramName, double defVal)
        {
            string str = GetGameConifgItem(paramName);
            if (string.IsNullOrEmpty(str))
            {
                return defVal;
            }

            double ret = 0.0;

            try
            {
                ret = Convert.ToDouble(str);
            }
            catch (Exception)
            {
                ret = defVal;
            }

            return ret;
        }

        /// <summary>
        /// 将所有的配置参数发送给指定的GM客户端
        /// </summary>
        public void SendAllGameConfigItemsToGM(KPlayer client)
        {
            string textMsg = "";
            string paramValue = "";
            lock (_GameConfigDict)
            {
                foreach (var key in _GameConfigDict.Keys)
                {
                    paramValue = _GameConfigDict[key];
                    textMsg = string.Format("{0}={1}",
                        key,
                        paramValue);

                    //给某个在线的角色发送系统消息
                    GameManager.ClientMgr.SendSystemChatMessageToClient(Global._TCPManager.MySocketListener,
                        Global._TCPManager.TcpOutPacketPool, client, textMsg);
                }
            }
        }

        /// <summary>
        /// 当参数发生变化时通知
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        private void ChangeParams(string paramName, string paramValue)
        {
            bool updateHuoID = false;
            if ("big_award_id" == paramName)
            {
                updateHuoID = true;
            }
            else if ("songli_id" == paramName)
            {
                updateHuoID = true;
            }

            //通知更新活动ID
            if (updateHuoID)
            {
                int bigAwardID = GameManager.GameConfigMgr.GetGameConfigItemInt("big_award_id", 0);
                int songLiID = GameManager.GameConfigMgr.GetGameConfigItemInt("songli_id", 0);

                //通知所有在线用户活动ID发生了改变
                GameManager.ClientMgr.NotifyAllChangeHuoDongID(bigAwardID, songLiID);
            }

            if ("half_BoundToken_period" == paramName)
            {
                int halfBoundTokenPeriod = GameManager.GameConfigMgr.GetGameConfigItemInt("half_BoundToken_period", 0);

                //通知所有在线用户银两折半优惠发生了改变
                GameManager.ClientMgr.NotifyAllChangeHalfBoundTokenPeriod(halfBoundTokenPeriod);
            }
        }

        #endregion 基础方法
    }
}