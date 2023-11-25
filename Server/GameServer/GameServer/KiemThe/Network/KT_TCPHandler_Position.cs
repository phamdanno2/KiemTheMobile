using GameServer.Interface;
using GameServer.KiemThe.Logic.Manager;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý gói tin
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Tin tưởng vị trí gửi lên từ Client không
        /// </summary>
        public const bool TrustClientPos = false;

		#region Position
		/// <summary>
		/// Tick kiểm tra vị trí của Client và Server
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="nID"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static TCPProcessCmdResults ProcessSpritePosCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            SpritePositionData cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SpritePositionData>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                if (null == cmdData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Wrong packet params, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = cmdData.RoleID;
                /// Bản đồ đang đứng
                int mapCode = cmdData.MapCode;
                int posX = cmdData.PosX;
                int posY = cmdData.PosY;
                /// Danh sách người chơi đang trong tầm nhìn của Client
                int[] nearbyPlayers = cmdData.NearbyPlayers;
                /// Nếu Null thì tạo mới
                if (nearbyPlayers == null)
				{
                    nearbyPlayers = new int[] { };
                }

                /// Đối tượng người chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Thông tin bản đồ hiện tại
                GameMap gameMap = GameManager.MapMgr.GetGameMap(client.CurrentMapCode);
                if (null == gameMap)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu bản đồ ở Client khác bản đồ hiện tại và không phải đang đợi chuyển map
                if (mapCode != client.CurrentMapCode && !client.WaitingForChangeMap)
				{
                    LogManager.WriteLog(LogTypes.RolePosition, string.Format("Bug VKL different map => Server MapID = {0}, Client MapID = {1}", client.CurrentMapCode, mapCode));

                    /// Buộc thực hiện chuyển bản đồ
                    SCMapChange mapChange = new SCMapChange()
                    {
                        MapCode = client.CurrentMapCode,
                        PosX = posX,
                        PosY = posY,
                        RoleID = client.RoleID,
                        Direction = client.RoleDirection,
                        TeleportID = -1,
                        ErrorCode = -1,
                    };
                    client.SendPacket<SCMapChange>((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, mapChange);
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Bản đồ tương ứng
                MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[client.MapCode];

                /// Duyệt danh sách tầm nhìn của Client
                foreach (int playerRoleID in nearbyPlayers)
				{
                    /// Nếu là bản thân thì bỏ qua
                    if (playerRoleID == roleID)
					{
                        continue;
					}
                    /// Nếu không có trong tầm nhìn ở GS, hoặc trong trạng thái tàng hình và bản thân không thấy được
                    if (!client.NearbyPlayers.TryGetValue(playerRoleID, out KPlayer player) || (player.IsInvisible() && !player.VisibleTo(client)))
					{
                        /// Gửi gói tin về Client xóa người chơi tương ứng
                        string strcmd = string.Format("{0}:{1}", playerRoleID, (int) GSpriteTypes.Other);
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_LEAVE, strcmd);
                    }
				}

                List<int> keys = client.NearbyPlayers.Keys.ToList();
                /// Duyệt danh sách tầm nhìn ở GS
                foreach (int playerRoleID in keys)
				{
                    /// Nếu là bản thân thì bỏ qua
                    if (playerRoleID == roleID)
                    {
                        continue;
                    }
                    /// Nếu không có trong danh sách ở Client
                    if (!nearbyPlayers.Contains(playerRoleID))
					{
                        /// Nếu không tồn tại trong danh sách, hoặc trong trạng thái tàng hình và bản thân không thấy được
                        if (!client.NearbyPlayers.TryGetValue(playerRoleID, out KPlayer player) || (player.IsInvisible() && !player.VisibleTo(client)))
						{
                            continue;
						}
                        /// Nếu không tồn tại hoặc đã Offline thì thôi
                        if (player == null || player.LogoutState)
                        {
                            continue;
                        }

                        /// Gửi gói tin về Client thêm người chơi tương ứng
                        RoleDataMini roleDataMini = Global.ClientToRoleDataMini(player);
                        client.SendPacket<RoleDataMini>((int) TCPGameServerCmds.CMD_OTHER_ROLE, roleDataMini);
                    }
                }

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
		#endregion

		#region New objects
		/// <summary>
		/// Có đối tượng mới xung quanh
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="nID"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static TCPProcessCmdResults ProcessSpriteLoadAlreadyCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int otherRoleID = Convert.ToInt32(fields[1]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Người chơi khác
                KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                /// Nếu là người chơi
                if (otherClient != null)
                {
                    Global.HandleGameClientLoaded(client, otherClient);
                }
                else
                {
                    /// Quái
                    Monster monster = GameManager.MonsterMgr.FindMonster(client.MapCode, otherRoleID);
                    /// Nếu là quái
                    if (monster != null)
                    {
                        Global.HandleMonsterLoaded(client, monster);
                    }
                    else
                    {
                        /// Bot
                        KTBot bot = KTBotManager.FindBot(otherRoleID);
                        /// Nếu là bot
                        if (bot != null)
                        {
                            Global.HandleBotLoaded(client, bot);
                        }
                    }
                }

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
		#endregion
	}
}
