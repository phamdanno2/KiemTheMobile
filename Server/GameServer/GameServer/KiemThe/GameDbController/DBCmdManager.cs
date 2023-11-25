using GameServer.Server;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Logic
{
    /// <summary>
    /// 数据库命令队列管理
    /// </summary>
    public class DBCmdManager
    {
        /// <summary>
        /// 数据库命令池
        /// </summary>
        private DBCmdPool _DBCmdPool = new DBCmdPool(1000);

        /// <summary>
        /// 等待处理的数据库命令队列
        /// </summary>
        private Queue<DBCommand> _DBCmdQueue = new Queue<DBCommand>(1000);

        /// <summary>
        /// 添加一个新的数据库命令到队列中
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="cmdText"></param>
        public void AddDBCmd(int cmdID, string cmdText, DBCommandEventHandler dbCommandEvent, int serverId)
        {
            Global.ExecuteDBCmd(cmdID, cmdText, serverId);
            return;

            DBCommand dbCmd = _DBCmdPool.Pop();
            if (null == dbCmd)
            {
                dbCmd = new DBCommand();
            }

            dbCmd.DBCommandID = cmdID;
            dbCmd.DBCommandText = cmdText;
            if (null != dbCommandEvent)
            {
                dbCmd.DBCommandEvent += dbCommandEvent;
            }

            lock (_DBCmdQueue)
            {
                _DBCmdQueue.Enqueue(dbCmd);
            }
        }

        /// <summary>
        /// 获取等待处理的DBCmd数量个数
        /// </summary>
        /// <returns></returns>
        public int GetDBCmdCount()
        {
            lock (_DBCmdQueue)
            {
                return _DBCmdQueue.Count;
            }
        }

        /// <summary>
        /// 执行数据库命令
        /// </summary>
        /// <param name="tcpClientPool"></param>
        /// <param name="pool"></param>
        /// <param name="dbCmd"></param>
        /// <returns></returns>
        private TCPProcessCmdResults DoDBCmd(TCPClientPool tcpClientPool, TCPOutPacketPool pool, DBCommand dbCmd, out byte[] bytesData)
        {
            bytesData = Global.SendAndRecvData(dbCmd.DBCommandID, dbCmd.DBCommandText, dbCmd.ServerId);
            if (null == bytesData || bytesData.Length <= 0)
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            return TCPProcessCmdResults.RESULT_OK;
        }

        /// <summary>
        /// 执行数据库命令
        /// </summary>
        public void ExecuteDBCmd(TCPClientPool tcpClientPool, TCPOutPacketPool pool)
        {
            lock (_DBCmdQueue)
            {
                if (_DBCmdQueue.Count <= 0) return;
            }

            List<DBCommand> dbCmdList = new List<DBCommand>();
            lock (_DBCmdQueue)
            {
                while (_DBCmdQueue.Count > 0)
                {
                    dbCmdList.Add(_DBCmdQueue.Dequeue());
                }
            }

            Int32 length = 0;
            string strData = null;
            string[] fieldsData = null;
            byte[] bytesData = null;
            TCPProcessCmdResults result;
            for (int i = 0; i < dbCmdList.Count; i++)
            {
                result = DoDBCmd(tcpClientPool, pool, dbCmdList[i], out bytesData);
                if (result == TCPProcessCmdResults.RESULT_FAILED)
                {
                    //写日志
                    LogManager.WriteLog(LogTypes.Error, string.Format("向DBServer请求执行命令失败, CMD={0}", (TCPGameServerCmds)dbCmdList[i].DBCommandID));
                }
                else
                {
                    //解析返回值
                    length = BitConverter.ToInt32(bytesData, 0);
                    strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

                    //解析客户端的指令
                    fieldsData = strData.Split(':');
                }

                //执行事件
                dbCmdList[i].DoDBCommandEvent(new DBCommandEventArgs() { Result = result, fields = fieldsData });

                //还回队列
                _DBCmdPool.Push(dbCmdList[i]);
            }
        }
    }
}