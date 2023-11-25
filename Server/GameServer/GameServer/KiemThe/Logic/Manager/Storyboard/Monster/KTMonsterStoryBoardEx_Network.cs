using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý Network StoryBoard
    /// </summary>
	public partial class KTMonsterStoryBoardEx
	{
        /// <summary>
        /// Gửi gói tin quái vật di chuyển tới người chơi xung quanh
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="pathString"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="direction"></param>
        /// <param name="action"></param>
        private void SendMonsterMoveToClient(Monster monster, string pathString, Point fromPos, Point toPos, KiemThe.Entities.Direction direction, KE_NPC_DOING action)
        {
            try
            {
                int fromPosX = (int) fromPos.X;
                int fromPosY = (int) fromPos.Y;
                int toPosX = (int) toPos.X;
                int toPosY = (int) toPos.Y;

                string zipPathString = pathString;

                int moveSpeed = monster.GetCurrentRunSpeed();
                GameManager.ClientMgr.NotifyOthersToMoving(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, monster, monster.MonsterZoneNode.MapCode, monster.CurrentCopyMapID, monster.RoleID, KTGlobal.GetCurrentTimeMilis(), fromPosX, fromPosY, (int) action, toPosX, toPosY, (int) TCPGameServerCmds.CMD_SPR_MOVE, moveSpeed, zipPathString, null, direction);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Thông báo cho người chơi khác đối tượng ngừng di chuyển
        /// </summary>
        /// <param name="obj"></param>
        private void SendMonsterStopMoveToClient(Monster monster)
        {
            try
            {
                List<object> objsList = Global.GetAll9Clients(monster);

                /// Tạo mới gói tin
                byte[] data = DataHelper.ObjectToBytes<SpriteStopMove>(new SpriteStopMove()
                {
                    RoleID = monster.RoleID,
                    PosX = (int) monster.CurrentPos.X,
                    PosY = (int) monster.CurrentPos.Y,
                    MoveSpeed = monster.GetCurrentRunSpeed(),
                });

                GameManager.ClientMgr.SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, monster, objsList, data, (int) TCPGameServerCmds.CMD_SPR_STOPMOVE);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
    }
}
