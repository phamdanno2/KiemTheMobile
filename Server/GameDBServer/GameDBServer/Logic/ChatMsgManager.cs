﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
//using System.Windows.Forms;
using GameDBServer.DB;
using Server.Data;
using ProtoBuf;
using GameDBServer.Logic;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 聊天消息的管理
    /// </summary>
    public class ChatMsgManager
    {
        #region 数据定义

        /// <summary>
        /// 存储分线的聊天消息的队列字典
        /// </summary>
        private static Dictionary<int, Queue<string>> ChatMsgDict = new Dictionary<int, Queue<string>>();

        #endregion 数据定义

        #region 存储消息

        /// <summary>
        /// 根据线路ID获取聊天的消息
        /// </summary>
        /// <param name="serverLineID"></param>
        /// <returns></returns>
        private static Queue<string> GetChatMsgQueue(int serverLineID)
        {
            Queue<string> msgQueue = null;
            lock (ChatMsgDict)
            {
                if (!ChatMsgDict.TryGetValue(serverLineID, out msgQueue))
                {
                    msgQueue = new Queue<string>();
                    ChatMsgDict[serverLineID] = msgQueue;
                }
            }

            return msgQueue;
        }

        /// <summary>
        /// 添加GM命令消息
        /// </summary>
        /// <param name="serverLineID"></param>
        /// <param name="gmCmd"></param>
        public static void AddGMCmdChatMsg(int serverLineID, string gmCmd)
        {
            string chatMsg = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", 0, "", 0, "", 0, gmCmd, 0, 0, serverLineID);            
            List<LineItem> itemList = LineManager.GetLineItemList();
            if (null != itemList)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].LineID == serverLineID)
                    {
                        continue;
                    }

                    if (itemList[i].LineID < GameDBManager.KuaFuServerIdStartValue || itemList[i].LineID == GameDBManager.ZoneID)
                    {
                        ChatMsgManager.AddChatMsg(itemList[i].LineID, chatMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 添加GM命令消息只在任意一个Gameserver发(用于功能)
        /// </summary>
        /// <param name="gmCmd"></param>
        public static void AddGMCmdChatMsgToOneClient(string gmCmd)
        {
            string chatMsg = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", 0, "", 0, "", 0, gmCmd, 0, 0, -1);  
            List<LineItem> itemList = LineManager.GetLineItemList();
            if (null != itemList)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].LineID < GameDBManager.KuaFuServerIdStartValue || itemList[i].LineID == GameDBManager.ZoneID)
                    {
                        ChatMsgManager.AddChatMsg(itemList[i].LineID, chatMsg);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Send Chat sang sv
        /// </summary>
        /// <param name="serverLineID"></param>
        /// <param name="chatMsg"></param>
        public static void AddChatMsg(int serverLineID, string chatMsg)
        {
           // LogManager.WriteLog(LogTypes.SQL, string.Format("AddChatMsg:LineID={0},Msg={1}", serverLineID, chatMsg));
            Queue<string> msgQueue = GetChatMsgQueue(serverLineID);
            lock (msgQueue)
            {
                if (msgQueue.Count > 30000) 
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("线路{0}的转发消息太多，被丢弃，一共丢弃{1}条，请检查GameServer是否正常", serverLineID, msgQueue.Count));
                    List<string> cmdList = msgQueue.ToList();
                    msgQueue.Clear();

                    Dictionary<string, int> cmdAnalysis = new Dictionary<string, int>();
                    foreach (var cmd in cmdList)
                    {
                        string szKey = string.Empty;
                        try
                        {
                            szKey = cmd.Split(':')[5].Split(' ')[0];
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(szKey))
                        {
                            if (cmdAnalysis.ContainsKey(szKey))
                                cmdAnalysis[szKey] += 1;
                            else
                                cmdAnalysis[szKey] = 1;

                           
                            if (szKey.StartsWith("-buyyueka") || szKey.StartsWith("-updateyb") || szKey.StartsWith("-updateBindgold")
                                || szKey.StartsWith("-config"))
                            {
                                msgQueue.Enqueue(cmd);
                            }
                        }
                    }

                    if (msgQueue.Count() >= 15000)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("线路{0}丢失重要命令{1}条", serverLineID, msgQueue.Count()));
                        msgQueue.Clear();
                    }

                    List<KeyValuePair<string, int>> cmdAnaList = cmdAnalysis.ToList();
                    cmdAnaList.Sort((_left, _right) => { return _right.Value - _left.Value; });
                    StringBuilder sb = new StringBuilder();
                    sb.Append("转发消息统计,").AppendFormat("共有{0}类消息:    ", cmdAnaList.Count()) .AppendLine();
                    for (int i = 0; i < cmdAnaList.Count(); i++)
                    {
                        string _cmd = cmdAnaList[i].Key;
                        int _cnt = cmdAnaList[i].Value;

                        if (_cnt <= 10)
                            break;

                        sb.AppendFormat("   cmd={0}, cnt={1}", _cmd, _cnt).AppendLine();
                    }

                    LogManager.WriteLog(LogTypes.Error, string.Format("线路{0}的转发消息太多，丢弃日志分析如下{1}", serverLineID, sb.ToString()));
                }

                msgQueue.Enqueue(chatMsg);
            }
        }

        #endregion 存储消息

        #region 获取消息

        /// <summary>
        /// 获取指定线路上的所有正在等待的消息
        /// </summary>
        /// <param name="serverLineID"></param>
        /// <returns></returns>
        public static TCPOutPacket GetWaitingChatMsg(TCPOutPacketPool pool, int cmdID, int serverLineID)
        {
            TCPOutPacket tcpOutPacket = null;

            List<string> msgList = new List<string>();
            Queue<string> msgQueue = GetChatMsgQueue(serverLineID);
            lock (msgQueue)
            {
                while (msgQueue.Count > 0 && msgList.Count < 250)
                {
                    msgList.Add(msgQueue.Dequeue());
                }
            }

            tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<string>>(msgList, pool, cmdID);
            return tcpOutPacket;
        }

        #endregion 获取消息
    }
}
