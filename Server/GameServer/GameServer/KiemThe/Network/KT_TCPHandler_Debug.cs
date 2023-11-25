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
    /// Quản lý Debug
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Debug

        /// <summary>
        /// Gửi yêu cầu về Client hiện các khối Debug Object tại vị trí tương ứng
        /// </summary>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="copyMapID">ID phụ bản</param>
        /// <param name="points">Danh sách vị trí cần hiện</param>
        /// <param name="objectSize">Kích thước khối Debug (Pixel)</param>
        /// <param name="lifeTime">Thời gian tồn tại</param>
        /// <param name="player">Hiện Debug cho người chơi tương ứng (nếu NULL thì hiện cho tất cả người chơi đứng quanh phạm vi)</param>
        public static void ShowDebugObjects(int mapCode, int copyMapID, List<UnityEngine.Vector2> points, int objectSize, float lifeTime, KPlayer player = null)
        {
            if (points.Count <= 0)
            {
                return;
            }
            try
            {
                List<G2C_ShowDebugObject> objects = new List<G2C_ShowDebugObject>();
                foreach (UnityEngine.Vector2 point in points)
                {
                    G2C_ShowDebugObject showDebugObject = new G2C_ShowDebugObject()
                    {
                        PosX = (int) point.x,
                        PosY = (int) point.y,
                        Size = objectSize,
                        LifeTime = lifeTime,
                    };
                    objects.Add(showDebugObject);
                }
                byte[] cmdData = DataHelper.ObjectToBytes<List<G2C_ShowDebugObject>>(objects);

                if (player == null)
                {
                    /// Tìm tất cả người chơi xung quanh để gửi gói tin
                    List<KPlayer> listObjects = KTLogic.GetNearByObjectsAtPos<KPlayer>(mapCode, copyMapID, points[0], 1000f);

                    foreach (KPlayer obj in listObjects)
                    {
                        GameManager.ClientMgr.SendToClient(obj, cmdData, (int) TCPGameServerCmds.CMD_KT_G2C_SHOWDEBUGOBJECTS);
                    }
                }
                else
                {
                    GameManager.ClientMgr.SendToClient(player, cmdData, (int) TCPGameServerCmds.CMD_KT_G2C_SHOWDEBUGOBJECTS);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Gửi yêu cầu về Client hiện khối Debug Object tại vị trí tương ứng
        /// </summary>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="copyMapCode">ID phụ bản</param>
        /// <param name="pos">Vị trí cần hiện</param>
        /// <param name="objectSize">Kích thước khối Debug (Pixel)</param>
        /// <param name="lifeTime">Thời gian tồn tại</param>
        /// <param name="player">Hiện Debug cho người chơi tương ứng (nếu NULL thì hiện cho tất cả người chơi đứng quanh phạm vi)</param>
        public static void ShowDebugObject(int mapCode, int copyMapID, UnityEngine.Vector2 pos, int objectSize, float lifeTime, KPlayer player = null)
        {
            List<UnityEngine.Vector2> points = new List<UnityEngine.Vector2>
            {
                pos
            };
            KT_TCPHandler.ShowDebugObjects(mapCode, copyMapID, points, objectSize, lifeTime, player);
        }

        #endregion Debug
    }
}
