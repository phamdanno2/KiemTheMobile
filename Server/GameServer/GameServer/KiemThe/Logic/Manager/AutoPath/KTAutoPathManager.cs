using GameServer.KiemThe.Entities;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.KiemThe.Logic.Manager
{
    /// <summary>
    /// Lớp quản lý tự tìm đường
    /// </summary>
    public static class KTAutoPathManager
    {
        /// <summary>
        /// Thông tin tự tìm đường
        /// </summary>
        private static AutoPathXML AutoPaths = null;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public static void Init()
        {
            XElement xmlNode = Global.GetGameResXml("Config/KT_AutoPath/AutoPath.xml");
            KTAutoPathManager.AutoPaths = AutoPathXML.Parse(xmlNode);
        }

        /// <summary>
        /// Kiểm tra trong danh sách tự tìm đường có điểm dịch chuyển ở NPC tương ứng không
        /// </summary>
        /// <param name="player"></param>
        /// <param name="toMapCode"></param>
        /// <param name="npcResID"></param>
        /// <returns></returns>
        public static bool CanTransferFromNPCToMap(KPlayer player, int toMapCode, out AutoPathXML.Node node)
        {
            int currentMapCode = player.CurrentMapCode;
            int posX = (int) player.CurrentPos.X;
            int posY = (int) player.CurrentPos.Y;
            UnityEngine.Vector2 fromPos = new UnityEngine.Vector2(posX, posY);
            node = KTAutoPathManager.AutoPaths.TransferNPCs.Where((nodeInfo) => {
                if (nodeInfo.FromMapCode != player.CurrentMapCode || nodeInfo.ToMapCode != toMapCode)
                {
                    return false;
                }
                UnityEngine.Vector2 npcPos = new UnityEngine.Vector2(nodeInfo.PosX, nodeInfo.PosY);
                return UnityEngine.Vector2.Distance(fromPos, npcPos) <= 100;
            }).FirstOrDefault();

            /// Trả về giá trị nếu NODE tồn tại
            return node != null;
        }

        /// <summary>
        /// Kiểm tra trong danh sách tự tìm đường có điểm dịch chuyển theo vật phẩm ID tương ứng không
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemID"></param>
        /// <param name="toMapCode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool CanTransferByUsingTeleportItem(KPlayer player, int itemID, int toMapCode, out AutoPathXML.Node node)
        {
            /// Mặc định Node NULL
            node = null;
            /// Nếu vật phẩm không tồn tại trong danh sách
            if (!KTAutoPathManager.AutoPaths.TeleportItems.TryGetValue(itemID, out List<AutoPathXML.Node> paths))
            {
                return false;
            }

            int currentMapCode = player.CurrentMapCode;
            int posX = (int) player.CurrentPos.X;
            int posY = (int) player.CurrentPos.Y;
            UnityEngine.Vector2 fromPos = new UnityEngine.Vector2(posX, posY);
            node = paths.Where((nodeInfo) => {
                if ((nodeInfo.FromMapCode != 0 && nodeInfo.FromMapCode != player.CurrentMapCode) || nodeInfo.ToMapCode != toMapCode)
                {
                    return false;
                }
                UnityEngine.Vector2 pos = new UnityEngine.Vector2(nodeInfo.PosX, nodeInfo.PosY);

                /// Nếu không có tọa độ
                if (pos.x == -1 && pos.y == -1)
                {
                    return true;
                }
                else
                {
                    return UnityEngine.Vector2.Distance(fromPos, pos) <= 100;
                }
            }).FirstOrDefault();

            /// Trả về giá trị nếu NODE tồn tại
            return node != null;
        }
    }
}
