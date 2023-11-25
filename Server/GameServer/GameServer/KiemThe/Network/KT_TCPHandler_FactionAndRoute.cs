using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý môn phái và nhánh
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Môn phái và nhánh

        /// <summary>
        /// Thông báo môn phái và nhánh của người chơi thay đổi
        /// </summary>
        /// <param name="client"></param>
        public static void NotificationFactionChanged(KPlayer client)
        {
            try
            {
                RoleFactionChanged factionChanged = new RoleFactionChanged()
                {
                    RoleID = client.RoleID,
                    FactionID = client.m_cPlayerFaction.GetFactionId(),
                    RouteID = client.m_cPlayerFaction.GetRouteId(),
                };
                byte[] cmdData = DataHelper.ObjectToBytes<RoleFactionChanged>(factionChanged);
                /// Tìm tất cả người chơi xung quanh để gửi gói tin
                List<object> listObjects = Global.GetAll9Clients(client);
                if (listObjects == null)
                {
                    return;
                }
                /// Gửi gói tin đến tất cả người chơi xung quanh
                GameManager.ClientMgr.SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, null, listObjects, cmdData, (int) TCPGameServerCmds.CMD_KT_FACTIONROUTE_CHANGED);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Lưu lại môn phái và nhánh vào DB
        /// </summary>
        /// <param name="client"></param>
        public static void SaveFactionAndRouteToDBServer(KPlayer player)
        {
            string strcmd = string.Format("{0}:{1}:{2}", player.RoleID, player.m_cPlayerFaction.GetFactionId(), player.m_cPlayerFaction.GetRouteId());
            try
            {
                GameManager.DBCmdMgr.AddDBCmd((int) TCPGameServerCmds.CMD_DB_EXECUTECHANGEOCCUPATION, strcmd, null, player.ServerId);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        #endregion Môn phái và nhánh
    }
}
