using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Interface;
using GameServer.Server;
using System.Windows;
using Server.Tools;
using Server.Data;
using ProtoBuf;
using Tmsk.Contract;
using HSGameEngine.Tools.AStar;
using UnityEngine;
using GameServer.KiemThe.Core.Task;

namespace GameServer.Logic
{
    public class GlobalNew
    {
       
        


        #region 数据库服务器连接

        public static TCPClient PopGameDbClient(int serverId, int poolId)
        {
#if BetaConfig
            if (serverId <= 0)
#else
            if (serverId <= 0 || serverId == GameManager.ServerId)
#endif
            {
                if (poolId == 0)
                {
                    return Global._TCPManager.tcpClientPool.Pop();
                }
                else// if(poolId == 1)
                {
                    return Global._TCPManager.tcpLogClientPool.Pop();
                }
            }
            else
            {
                return KuaFuManager.getInstance().PopGameDbClient(serverId, poolId);
            }
        }

        public static void PushGameDbClient(int serverId, TCPClient tcpClient, int poolId)
        {
#if BetaConfig
            if (serverId <= 0)
#else
            if (serverId <= 0 || serverId == GameManager.ServerId)
#endif
            {
                if (poolId == 0)
                {
                    Global._TCPManager.tcpClientPool.Push(tcpClient);
                }
                else// if(poolId == 1)
                {
                    Global._TCPManager.tcpLogClientPool.Push(tcpClient);
                }
            }
            else
            {
                KuaFuManager.getInstance().PushGameDbClient(serverId, tcpClient, poolId);
            }
        }

        #endregion 数据库服务器连接

        #region 跨服

        public static void UpdateKuaFuRoleDayLogData(int serverId, int roleId, DateTime now, int zoneId, int signUpCount, int startGameCount, int successCount, int faildCount, int gameType)
        {
            Global.SendToDB<RoleKuaFuDayLogData>((int)TCPGameServerCmds.CMD_LOGDB_UPDATE_ROLE_KUAFU_DAY_LOG, new RoleKuaFuDayLogData()
            {
                RoleID = roleId,
                Day = now.Date.ToString("yyyy-MM-dd"),
                ZoneId = zoneId,
                SignupCount = signUpCount,
                StartGameCount = startGameCount,
                SuccessCount = successCount,
                FaildCount = faildCount,
                GameType = gameType,
            }, serverId);
        }

        public static void RecordSwitchKuaFuServerLog(KPlayer client)
        {
            ushort LastMapCode = 0, LastPosX = 0, LastPosY = 0;
            if (SceneUIClasses.Normal == Global.GetMapSceneType(client.MapCode))
            {
                LastMapCode = (ushort)client.CurrentMapCode;
                LastPosX = (ushort)client.CurrentGrid.X;
                LastPosY = (ushort)client.CurrentGrid.Y;
            }

            Global.ModifyMapRecordData(client, LastMapCode, LastPosX, LastPosY, (int)MapRecordIndexes.InitGameMapPostion);

            KuaFuServerLoginData kuaFuServerLoginData = Global.GetClientKuaFuServerLoginData(client);
            LogManager.WriteLog(LogTypes.Error, string.Format("RoleId={0},GameId={1},SrcServerId={2},KfIp={3},KfPort={4}", kuaFuServerLoginData.RoleId, kuaFuServerLoginData.GameId, kuaFuServerLoginData.ServerId, kuaFuServerLoginData.ServerIp, kuaFuServerLoginData.ServerPort));
        }

        #endregion 跨服


        #region 寻路
        /// <summary>
        /// Làm mịn đường đi
        /// </summary>
        /// <param name="srcLine"></param>
        public static void LineSmoothEx(ref List<int[]> srcLine)
        {
            int count = srcLine.Count;
            int[] nodeBefore = null;
            int[] nodeAfter = null;
            int distanceUnit = 4;
            int needInsertCount = 0;
            float distance = 0.0f;

            for (int i = 0; i < srcLine.Count - 1; i++)
            {
                nodeBefore = srcLine[i];
                nodeAfter = srcLine[i + 1];
                distance = Mathf.Sqrt((nodeAfter[0] - nodeBefore[0]) * (nodeAfter[0] - nodeBefore[0]) + (nodeAfter[1] - nodeBefore[1]) * (nodeAfter[1] - nodeBefore[1]));

                if (distance > distanceUnit)
                {
                    needInsertCount = (int)distance / distanceUnit;
                    float delta = 1.0f / (needInsertCount + 1.0f);

                    for (int k = 0; k < needInsertCount; k++)
                    {
                        Vector2 vector = Vector2.Lerp(new Vector2(nodeBefore[0], nodeBefore[1]), new Vector2(nodeAfter[0], nodeAfter[1]), delta * (k + 1));
                        srcLine.Insert(i + k + 1, new int[] { (int)vector.x, (int)vector.y });
                    }
                    i += needInsertCount; //将索引作变换
                }
            }
        }


        private static Stack<PathFinderFast> _pathStack = new Stack<PathFinderFast>();
        public static List<int[]> FindPath(Point startPoint,Point endPoint,int mapCode)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                return null;
            }

            PathFinderFast pathFinderFast = null;
            if (_pathStack.Count<=0)
            {
                pathFinderFast = new PathFinderFast(gameMap.MyNodeGrid.GetFixedObstruction())
                {
                    Formula = HeuristicFormula.Manhattan,
                    Diagonals = true,
                    HeuristicEstimate = 2,
                    ReopenCloseNodes = true,
                    SearchLimit = 2147483647,
                    Punish = null,
                    MaxNum = Global.GMax(gameMap.MapGridWidth, gameMap.MapGridHeight),
                };
            }
            else
            {
                pathFinderFast = _pathStack.Pop();
            }

            startPoint.X =  gameMap.CorrectWidthPointToGridPoint((int)startPoint.X) / gameMap.MapGridWidth;
            startPoint.Y = gameMap.CorrectHeightPointToGridPoint((int)startPoint.Y) / gameMap.MapGridHeight;
            endPoint.X = gameMap.CorrectWidthPointToGridPoint((int)endPoint.X) / gameMap.MapGridWidth;
            endPoint.Y = gameMap.CorrectHeightPointToGridPoint((int)endPoint.Y) / gameMap.MapGridHeight;

            pathFinderFast.EnablePunish = false;
            List<PathFinderNode> nodeList = pathFinderFast.FindPath(startPoint, endPoint);
            if (null == nodeList || nodeList.Count <= 0)
            {
                return null;
            }

            List<int[]> path = new List<int[]>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                path.Add(new int[] { nodeList[i].X, nodeList[i].Y });
            }

            //push

            return path;
        }


        #endregion

    }   //class
}   //namespace
