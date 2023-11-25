using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Tools;
using System;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
	/// <summary>
	/// Quản lý tầng Network của StoryBoard
	/// </summary>
	public partial class KTPlayerStoryBoardEx
	{
        #region Network
        /// <summary>
        /// Gửi gói tin người chơi di chuyển tới người chơi xung quanh
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="pathString"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="direction"></param>
        /// <param name="action"></param>
        private void SendPlayerMoveToClient(KPlayer player, string pathString, Point fromPos, Point toPos, KiemThe.Entities.Direction direction, KE_NPC_DOING action)
        {
            try
            {
                int fromPosX = (int) fromPos.X;
                int fromPosY = (int) fromPos.Y;
                int toPosX = (int) toPos.X;
                int toPosY = (int) toPos.Y;

                string zipPathString = pathString;

                int moveSpeed = player.GetCurrentRunSpeed();
                GameManager.ClientMgr.NotifyOthersToMoving(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, player.MapCode, player.CopyMapID, player.RoleID, KTGlobal.GetCurrentTimeMilis(), fromPosX, fromPosY, (int) action, toPosX, toPosY, (int) TCPGameServerCmds.CMD_SPR_MOVE, moveSpeed, zipPathString, null, direction);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }
        #endregion
    }
}
