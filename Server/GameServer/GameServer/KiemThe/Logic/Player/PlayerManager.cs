using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý người chơi
    /// </summary>
    public static class PlayerManager
    {
        #region Message Box
        /// <summary>
        /// Hiển thị bảng thông báo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msgBox"></param>
        private static void ShowMessageBox(KPlayer player, KMessageBox msgBox)
        {
            KT_TCPHandler.SendOpenMessageBox(player, msgBox.ID, msgBox.MessageType, msgBox.Title, msgBox.Text, msgBox.Parameters);
        }

        /// <summary>
        /// Hiện bảng thông báo có Title, Text tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowMessageBox(KPlayer player, string title, string text)
        {
            KTMessageBox msgBox = new KTMessageBox()
            {
                Owner = player,
                Text = text,
                Title = title,
                MessageType = 0,
                OK = null,
                Cancel = null,
                Parameters = new List<string>() { "0" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiện bảng thông báo có Title, Text tương ứng và kèm sự kiện OK
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="ok"></param>
        public static void ShowMessageBox(KPlayer player, string title, string text, Action ok)
        {
            KTMessageBox msgBox = new KTMessageBox()
            {
                Owner = player,
                Text = text,
                Title = title,
                MessageType = 0,
                OK = ok,
                Cancel = null,
                Parameters = new List<string>() { "0" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiện bảng thông báo có Title, Text tương ứng và kèm sự kiện OK
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="ok"></param>
        /// <param name="showButtonCancel"></param>
        public static void ShowMessageBox(KPlayer player, string title, string text, Action ok, bool showButtonCancel)
        {
            KTMessageBox msgBox = new KTMessageBox()
            {
                Owner = player,
                Text = text,
                Title = title,
                MessageType = 0,
                OK = ok,
                Cancel = null,
                Parameters = new List<string>() { showButtonCancel ? "1" : "0" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiện bảng thông báo có Title, Text tương ứng và kèm sự kiện OK, Cancel
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="ok"></param>
        /// <param name="cancel"></param>
        public static void ShowMessageBox(KPlayer player, string title, string text, Action ok, Action cancel)
        {
            KTMessageBox msgBox = new KTMessageBox()
            {
                Owner = player,
                Text = text,
                Title = title,
                MessageType = 0,
                OK = ok,
                Cancel = cancel,
                Parameters = new List<string>() { "1" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiện bảng nhập số có Text tương ứng, và sự kiện OK
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowInputNumberBox(KPlayer player, string text, Action<int> ok)
        {
            KTInputNumberBox msgBox = new KTInputNumberBox()
            {
                Owner = player,
                Text = text,
                Title = "",
                MessageType = 1,
                OK = ok,
                Cancel = null,
                Parameters = new List<string>() { "0" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiện bảng nhập số có Text tương ứng, và sự kiện OK, Cancel
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static void ShowInputNumberBox(KPlayer player, string text, Action<int> ok, Action cancel)
        {
            KTInputNumberBox msgBox = new KTInputNumberBox()
            {
                Owner = player,
                Text = text,
                Title = "",
                MessageType = 1,
                OK = ok,
                Cancel = cancel,
                Parameters = new List<string>() { "1" },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiển bảng nhập chuỗi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="initValue"></param>
        /// <param name="ok"></param>
        public static void ShowInputStringBox(KPlayer player, string description, Action<string> ok)
		{
            KTInputStringBox msgBox = new KTInputStringBox()
            {
                Owner = player,
                Text = description,
                Title = "",
                MessageType = 2,
                OK = ok,
                Cancel = null,
                Parameters = null,
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiển bảng nhập chuỗi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="initValue"></param>
        /// <param name="ok"></param>
        public static void ShowInputStringBox(KPlayer player, string description, Action<string> ok, Action cancel)
		{
            KTInputStringBox msgBox = new KTInputStringBox()
            {
                Owner = player,
                Text = description,
                Title = "",
                MessageType = 2,
                OK = ok,
                Cancel = cancel,
                Parameters = null,
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiển bảng nhập chuỗi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="initValue"></param>
        /// <param name="ok"></param>
        public static void ShowInputStringBox(KPlayer player, string description, string initValue, Action<string> ok)
		{
            KTInputStringBox msgBox = new KTInputStringBox()
            {
                Owner = player,
                Text = description,
                Title = "",
                MessageType = 2,
                OK = ok,
                Cancel = null,
                Parameters = new List<string>() { initValue },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }

        /// <summary>
        /// Hiển bảng nhập chuỗi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="initValue"></param>
        /// <param name="ok"></param>
        public static void ShowInputStringBox(KPlayer player, string description, string initValue, Action<string> ok, Action cancel)
		{
            KTInputStringBox msgBox = new KTInputStringBox()
            {
                Owner = player,
                Text = description,
                Title = "",
                MessageType = 2,
                OK = ok,
                Cancel = cancel,
                Parameters = new List<string>() { initValue },
            };
            KTMessageBoxManager.AddMessageBox(msgBox);
            PlayerManager.ShowMessageBox(player, msgBox);
        }
        #endregion

        #region Notification Tip
        /// <summary>
        /// Hiển thị thông báo Tooltip trên đầu người chơi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public static void ShowNotification(KPlayer player, string message)
        {
            KT_TCPHandler.ShowClientNotificationTip(player, message);
        }


        public static void NotifyExpChange(KPlayer client)
        {

            GameManager.ClientMgr.NotifySelfExperience(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, 0);

            client.LastClickDialog = KTGlobal.GetCurrentTimeMilis();

            // Thực hiện xếp lại túi đồ khi login
            KTGlobal.ResetBagAllGoods(client);
        }

        public static void NotifyBCH(KPlayer player,double EXPGAIN, double EXPNORMAL, double EXPPRO,int MINPRO,int MINNORMAL,int OFFLINEMIN)
        {

            GameManager.ClientMgr.ProcessRoleExperience(player, (long)EXPGAIN, true, false, true);

            string NOTIFY = "Bạn đã offline:" + KTGlobal.CreateStringByColor(OFFLINEMIN+"", ColorType.Importal) + " phút!\n\nSố phút ủy thác " + KTGlobal.CreateStringByColor("ĐẠI BẠCH CẦU HOÀN", ColorType.Done) + " là : " + KTGlobal.CreateStringByColor(MINPRO + "", ColorType.Done)  + " với "+ KTGlobal.CreateStringByColor(EXPPRO + "", ColorType.Done) + " EXP \n\nSố phút ủy thác " + KTGlobal.CreateStringByColor("BẠCH CẦU HOÀN", ColorType.Done) + " là : " + KTGlobal.CreateStringByColor(MINNORMAL + "", ColorType.Done) + " với " + KTGlobal.CreateStringByColor(EXPNORMAL + "", ColorType.Done) + " Exp\n\nTổng số EXP nhận :" + KTGlobal.CreateStringByColor(EXPGAIN + "", ColorType.Importal) + "\n\nSố phút ủy thác " + KTGlobal.CreateStringByColor("ĐẠI BẠCH CẦU HOÀN", ColorType.Done) + " còn lại là :" + KTGlobal.CreateStringByColor(player.baijuwanpro + "", ColorType.Accpect) + "\n\nSố phút ủy thác " + KTGlobal.CreateStringByColor("BẠCH CẦU HOÀN", ColorType.Done) + " còn lại là :" + KTGlobal.CreateStringByColor(player.baijuwan + "", ColorType.Accpect);

            KT_TCPHandler.OpenNPCDialog(player, 1000 , 1000, NOTIFY, new Dictionary<int, string>(), null, false, new Dictionary<int, string>());
        }

        /// <summary>
        /// Hiển thị thông báo Tooltip trên đầu tất cả thành viên trong nhóm
        /// </summary>
        /// <param name=""></param>
        /// <param name="message"></param>
        public static void ShowNotificationToAllTeammates(KPlayer player, string message)
        {
            foreach (KPlayer teammate in player.Teammates)
            {
                PlayerManager.ShowNotification(teammate, message);
            }
        }
        #endregion

        #region Gia nhập môn phái, đổi nhánh tu luyện
        /// <summary>
        /// Gia nhập môn phái
        /// </summary>
        /// <param name="player"></param>
        /// <param name="factionID"></param>
        /// <returns>-1: Player NULL, -2: Môn phái không tồn tại, 0: Giới tính không phù hợp, 1: Thành công</returns>
        public static int JoinFaction(KPlayer player, int factionID)
        {
            if (player == null)
            {
                return -1;
            }

            KFaction.KFactionAttirbute faction = KFaction.GetFactionInfo(factionID);
            if (faction == null)
            {
                PlayerManager.ShowNotification(player, "Không tìm thấy môn phái tương ứng!");
                return -2;
            }

            if (faction.nSexLimit != -1 && faction.nSexLimit != (int)player.RoleSex)
            {
                PlayerManager.ShowNotification(player, "Giới tính của bạn không phù hợp với môn phái này!");
                return 0;
            }

            if (player.m_cPlayerFaction.ChangeFaction(factionID))
            {
                ItemManager.AddFactionItem(player, factionID);
				/// Thông báo về Client môn phái thay đổi
                KT_TCPHandler.NotificationFactionChanged(player);

                /// Hủy các kỹ năng đã thiết lập ở thanh kỹ năng dùng nhanh, lưu vào DB
                player.MainQuickBarKeys = "-1|-1|-1|-1|-1|-1|-1|-1|-1|-1";
                KT_TCPHandler.SendSaveQuickKeyToDB(player);
                player.OtherQuickBarKeys = "-1_0";
                KT_TCPHandler.SendSaveAruaKeyToDB(player);

                return 1;
            }
            return -99999;
        }

        /// <summary>
        /// Đổi nhánh tu luyện
        /// </summary>
        /// <param name="player"></param>
        /// <param name="routeID"></param>
        /// <returns>-9999: Lỗi không rõ, -1: Player NULL, -2: Môn phái không tồn tại, 0: Giới tính không phù hợp, 1: Thành công</returns>
        public static int ChangeRoute(KPlayer player, int routeID)
        {
            int factionID = player.m_cPlayerFaction.GetFactionId();
            if (player == null)
            {
                return -1;
            }

            KFaction.KFactionAttirbute faction = KFaction.GetFactionInfo(factionID);
            if (faction == null)
            {
                return -2;
            }

            if (faction.nSexLimit != -1 && faction.nSexLimit != (int)player.RoleSex)
            {
                return 0;
            }

            if (player.m_cPlayerFaction.ChangeFactionRoute(routeID))
            {
                /// Thông báo về Client nhánh thay đổi
                KT_TCPHandler.NotificationFactionChanged(player);

                /// Hủy các kỹ năng đã thiết lập ở thanh kỹ năng dùng nhanh, lưu vào DB
                player.MainQuickBarKeys = "-1|-1|-1|-1|-1|-1|-1|-1|-1|-1";
                KT_TCPHandler.SendSaveQuickKeyToDB(player);
                player.OtherQuickBarKeys = "-1_0";
                KT_TCPHandler.SendSaveAruaKeyToDB(player);

                return 1;
            }
            else
            {
                return -9999;
            }
        }
        #endregion

        #region Relive
        /// <summary>
        /// Hồi sinh người chơi
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="posX">Vị trí X</param>
        /// <param name="posY">Vị trí Y</param>
        /// <param name="hpPercent">Phục hồi % sinh lực</param>
        /// <param name="mpPercent">Phục hồi % nội lực</param>
        /// <param name="staminaPercent">Phục hồi % thể lực</param>
        /// <returns>-1: Bản đồ không tồn tại, -2: Vị trí hồi sinh không thể đi vào được, 0: Không thể hồi sinh khi % sinh lực dưới 0, 1: Thành công</returns>
        public static int Relive(KPlayer player, int mapCode, int posX, int posY, int hpPercent, int mpPercent, int staminaPercent)
        {
            /// Ngừng di chuyển
            KTPlayerStoryBoardEx.Instance.Remove(player);

            /// Hủy trạng thái khinh công
            player.StopBlink();

            /// Bản đồ hồi sinh
            if (!GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out GameMap gameMap))
            {
                return -1;
            }

            /// Chuyển sang tọa độ lưới
            UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(gameMap, new UnityEngine.Vector2(posX, posY));

            /// Kiểm tra vị trí điểm hồi sinh có nằm trong vùng Block không
            if (!gameMap.CanMove((int)gridPos.x, (int)gridPos.y))
            {
                return -2;
            }

            /// Nếu % máu dưới 1 thì không thể hồi sinh được
            if (hpPercent <= 0)
            {
                return 0;
            }

            int hp = player.m_CurrentLifeMax * hpPercent / 100;
            int mp = player.m_CurrentManaMax * mpPercent / 100;
            int stamina = player.m_CurrentStaminaMax * staminaPercent / 100;

            /// Thiết lập máu của người chơi
            player.m_CurrentLife = hp;
            player.m_CurrentMana = mp;
            player.m_CurrentStamina = stamina;
            /// Thực hiện động tác đứng
            player.m_eDoing = KE_NPC_DOING.do_stand;


            /// Nếu ID bản đồ đích trùng với ID bản đồ hiện tại
            if (mapCode == player.CurrentMapCode)
            {
               
                ///// Đánh dấu đợi chuyển Pos
                //player.WaitingForChangePos = true;
                ///// Đánh dấu thời điểm lần cuối dịch Pos
                //player.LastChangePosTicks = KTGlobal.GetCurrentTimeMilis();

                /// Set lại tọa độ mới cho người chơi sau khi hồi sinh
                player.CurrentPos = new Point(posX, posY);
                /// Set lại tọa độ mới cho người chơi sau khi hồi sinh
                player.PosX = posX;
                player.PosY = posY;
                bool isAlive = !player.IsDead();
                if(isAlive)
                {
                    //dang song
                    KT_TCPHandler.ShowClientNotificationTip(player, "Hồi sinh thành công!");
                    
                    //ClientManager.DoSpritesMapGridMoveNewMode(500);
                }    
                /// Thực hiện dịch đối tượng
                GameManager.MapGridMgr.DictGrids[mapCode].MoveObject(-1, -1, posX, posY, player);
                ///// Thực hiện gọi hàm cập nhật di chuyển
                //ClientManager.DoSpriteMapGridMove(player, 200);

                /// Thông báo tới thằng khác bản thân sống lại
                GameManager.ClientMgr.NotifyOthersRealive(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, player.RoleID, posX, posY, (int) player.CurrentDir, hp);
                /// Thông báo đến mình sống lại
                GameManager.ClientMgr.NotifyMySelfRealive(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, player.RoleID, posX, posY, (int) player.CurrentDir, hp, mp, stamina);
            }
            else
            {
                /// Thông báo đến mình sống lại
                GameManager.ClientMgr.NotifyMySelfRealive(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, player.RoleID, posX, posY, (int) player.CurrentDir, hp, mp, stamina);

                /// Thực hiện dịch chuyển đến bản đồ chỉ định
                GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, mapCode, posX, posY, (int) player.CurrentDir);
            }

            /// Thực hiện thao tác sống lại
            player.DoRelive();
            /// Thực hiện sự kiện hồi sinh
            player.OnRevive();

            return 1;
        }
        #endregion

        #region Kick-out
        /// <summary>
        /// Kick người chơi tương ứng ra khỏi Game
        /// </summary>
        /// <param name="player"></param>
        public static void KickOut(KPlayer player)
        {
            /// Ngừng di chuyển
            KTPlayerStoryBoardEx.Instance.Remove(player);
            /// Đóng Socket tương ứng
            TCPManager.getInstance().MySocketListener.CloseSocket(player.ClientSocket);
        }
		#endregion

		#region Xóa vật phẩm
        /// <summary>
        /// Mở khung xóa vật phẩm
        /// </summary>
        /// <param name="player"></param>
        public static void OpenRemoveItems(KPlayer player)
		{
            /// Mở khung tiêu hủy vật phẩm
            KT_TCPHandler.SendOpenInputItems(player, "Tiêu hủy vật phẩm", "Đặt vào danh sách vật phẩm muốn tiêu hủy...", "Vật phẩm sau khi bị tiêu hủy sẽ không thể lấy lại được.", "RemoveItems");
		}

        /// <summary>
        /// Thực hiện xóa danh sách vật phẩm tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemGDs"></param>
        public static void ResolveRemoveItems(KPlayer player, List<GoodsData> itemGDs)
		{
            /// Duyệt danh sách
            foreach (GoodsData itemGD in itemGDs)
			{
                /// Thực hiện xóa vật phẩm
                ItemManager.AbandonItem(itemGD, player, false, "TIÊU HỦY VẬT PHẨM");
			}

            /// Thông báo
            PlayerManager.ShowNotification(player, "Tiêu hủy vật phẩm thành công!");
		}
		#endregion

		#region Gộp vật phẩm
        /// <summary>
        /// Mở khung ghép vật phẩm
        /// </summary>
        /// <param name="player"></param>
        public static void OpenMergeItems(KPlayer player)
		{
            /// Mở khung tiêu hủy vật phẩm
            KT_TCPHandler.SendOpenInputItems(player, "Ghép vật phẩm", "Đặt vào danh sách vật phẩm muốn ghép...", "Chỉ ghép được các vật phẩm cùng loại.", "MergeItems");
        }
        #endregion

        #region Open change signet, mantle, chopstick
        /// <summary>
        /// Mở khung đổi ngũ hành ấn, quan ấn và phi phong theo hệ của nhân vật tương ứng
        /// </summary>
        /// <param name="player"></param>
        public static void OpenChangeSignetMantleAndChopstick(KPlayer player)
        {
            /// Mở khung tiêu hủy vật phẩm
            KT_TCPHandler.SendOpenInputItems(player, "Đổi hệ trang bị", "Đặt vào <color=green>Ngũ Hành Ấn, Quan Ấn và Phi Phong</color> cần đổi hệ...", "Trang bị sau khi đổi sẽ theo môn phái và ngũ hành của nhân vật.", "ChangeSignetMantleAndChopstick");
        }

        /// <summary>
        /// Xử lý đổi ngũ hành ấn, quan ấn và phi phong theo hệ của nhân vật tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemGDs"></param>
        public static void ResolveChangeSignetMantleAndChopstick(KPlayer player, List<GoodsData> itemGDs)
        {
            /// Kết quả
            bool result = false;

            /// Môn phái nhân vật
            int factionID = player.m_cPlayerFaction.GetFactionId();
            /// Giới tính nhân vật
            int sex = player.RoleSex;
            /// Ngũ hành nhân vật
            KE_SERIES_TYPE series = player.m_Series;
            /// Thông tin TCP
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            /// Duyệt danh sách
            foreach (GoodsData itemGD in itemGDs)
            {
                /// Toác
                if (itemGD == null)
                {
                    return;
                }

                /// Vật phẩm không tồn tại
                if (!ItemManager._TotalGameItem.ContainsKey(itemGD.GoodsID))
                {
                    continue;
                }
                /// Không có DbID
                else if (itemGD.Id == -1)
                {
                    continue;
                }
                /// Số lượng không thỏa mãn
                else if (itemGD.GCount <= 0)
                {
                    continue;
                }
                
                /// Thông tin vật phẩm
                ItemData _itemData = ItemManager.GetItemTemplate(itemGD.GoodsID);
                /// Nếu không phải quan ấn, phi phong và ngũ hành ấn thì thôi
                if (_itemData.DetailType != (int) KE_ITEM_EQUIP_DETAILTYPE.equip_signet && _itemData.DetailType != (int) KE_ITEM_EQUIP_DETAILTYPE.equip_mantle && _itemData.DetailType != (int) KE_ITEM_EQUIP_DETAILTYPE.equip_chop)
                {
                    continue;
                }

                /// Vật phẩm thật trên người
                GoodsData goodsGD = player.GoodsDataList?.Where(x => x.Id == itemGD.Id && x.GCount > 0).FirstOrDefault();
                /// Nếu không tìm thấy
                if (goodsGD == null)
                {
                    continue;
                }

                /// Vật phẩm không tồn tại
                if (!ItemManager._TotalGameItem.ContainsKey(goodsGD.GoodsID))
                {
                    continue;
                }
                /// Không có DbID
                else if (goodsGD.Id == -1)
                {
                    continue;
                }
                /// Số lượng không thỏa mãn
                else if (goodsGD.GCount <= 0)
                {
                    continue;
                }
                /// ID vật phẩm khác
                else if (goodsGD.GoodsID != itemGD.GoodsID)
                {
                    continue;
                }

                /// Thông tin vật phẩm
                ItemData goodsItemData = ItemManager.GetItemTemplate(goodsGD.GoodsID);
                /// Toác
                if (goodsItemData == null)
                {
                    continue;
                }

                /// Danh sách cập nhật chỉ số trang bị
                Dictionary<UPDATEITEM, object> updateProperties = new Dictionary<UPDATEITEM, object>();
                updateProperties.Add(UPDATEITEM.ROLEID, player.RoleID);
                updateProperties.Add(UPDATEITEM.ITEMDBID, itemGD.Id);

                /// ID vật phẩm
                int goodsID = itemGD.GoodsID;
                /// Ngũ hành
                int goodsSeries = itemGD.Series;
                /// Nếu là ngũ hành ấn
                if (goodsItemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_signet)
                {
                    /// Tìm ngũ hành ấn tương ứng phái
                    ItemData itemData = ItemManager.Signets.Where((goods) => {
                        /// Nếu không cùng cấp độ
                        if (goods.Level != goodsItemData.Level)
                        {
                            return false;
                        }
                        /// Nếu phái không khớp
                        else if (!goods.ListReqProp.Any(x => x.ReqPropType == (int) KE_ITEM_REQUIREMENT.emEQUIP_REQ_FACTION && x.ReqPropValue == factionID))
                        {
                            return false;
                        }

                        /// Thỏa mãn
                        return true;
                    }).FirstOrDefault();
                    /// Nếu tìm thấy
                    if (itemData != null)
                    {
                        /// Thay đổi ID vật phẩm
                        goodsID = itemData.ItemID;
                        updateProperties.Add(UPDATEITEM.GOODSID, itemData.ItemID);
                        /// Thay đổi ngũ hành
                        goodsSeries = itemData.Series;
                        updateProperties.Add(UPDATEITEM.SERIES, itemData.Series);
                    }
                }
                /// Nếu là phi phong
                else if (goodsItemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_mantle)
                {
                    /// Tìm phi phong tương ứng hệ
                    ItemData itemData = ItemManager.Mantles.Where((goods) =>
                    {
                        /// Nếu không cùng cấp độ
                        if (goods.Level != goodsItemData.Level)
                        {
                            return false;
                        }
                        /// Nếu yêu cầu giới tính không thỏa mãn
                        else if (!goods.ListReqProp.Any(x => x.ReqPropType == (int) KE_ITEM_REQUIREMENT.emEQUIP_REQ_SEX && x.ReqPropValue == sex))
                        {
                            return false;
                        }
                        /// Nếu yêu cầu ngũ hành không thỏa mãn
                        else if (!goods.ListReqProp.Any(x => x.ReqPropType == (int) KE_ITEM_REQUIREMENT.emEQUIP_REQ_SERIES && x.ReqPropValue == (int) series))
                        {
                            return false;
                        }

                        /// Thỏa mãn
                        return true;
                    }).FirstOrDefault();
                    /// Nếu tìm thấy
                    if (itemData != null)
                    {
                        /// Thay đổi ID vật phẩm
                        goodsID = itemData.ItemID;
                        updateProperties.Add(UPDATEITEM.GOODSID, itemData.ItemID);
                        /// Thay đổi ngũ hành
                        goodsSeries = itemData.Series;
                        updateProperties.Add(UPDATEITEM.SERIES, itemData.Series);
                    }
                }
                /// Nếu là quan ấn
                else if (goodsItemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_chop)
                {
                    /// Tìm quan ấn tương ứng hệ
                    ItemData itemData = ItemManager.Chops.Where((goods) =>
                    {
                        /// Nếu không cùng cấp độ
                        if (goods.Level != goodsItemData.Level)
                        {
                            return false;
                        }
                        /// Nếu yêu cầu ngũ hành không thỏa mãn
                        else if (!goods.ListReqProp.Any(x => x.ReqPropType == (int) KE_ITEM_REQUIREMENT.emEQUIP_REQ_SERIES && x.ReqPropValue == (int) series))
                        {
                            return false;
                        }

                        /// Thỏa mãn
                        return true;
                    }).FirstOrDefault();
                    /// Nếu tìm thấy
                    if (itemData != null)
                    {
                        /// Thay đổi ID vật phẩm
                        goodsID = itemData.ItemID;
                        updateProperties.Add(UPDATEITEM.GOODSID, itemData.ItemID);
                        /// Thay đổi ngũ hành
                        goodsSeries = itemData.Series;
                        updateProperties.Add(UPDATEITEM.SERIES, itemData.Series);
                    }
                }

                /// Nếu không có gì thay đổi thì thôi
                if (goodsID == goodsGD.GoodsID)
                {
                    continue;
                }
                /// Đánh dấu có kết quả
                result = true;

                string cmdData = KTGlobal.ItemUpdateScriptBuild(updateProperties);
                TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int) TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, cmdData, out string[] dbFields, player.ServerId);
                /// Nếu thay đổi thành công
                if (dbRequestResult == TCPProcessCmdResults.RESULT_DATA)
                {
                    /// Cập nhật thay đổi ở GS
                    goodsGD.GoodsID = goodsID;
                    goodsGD.Series = goodsSeries;
                    /// Cập nhật thay đổi về Client
                    player.SendPacket<GoodsData>((int) TCPGameServerCmds.CMD_SPR_NOTIFYGOODSINFO, goodsGD);
                }
            }

            /// Nếu có kết quả
            if (result)
            {
                PlayerManager.ShowNotification(player, "Thao tác thành công!");
            }
            else
            {
                PlayerManager.ShowNotification(player, "Không có gì cần thay đổi!");
            }
        }
        #endregion

        #region Captcha
        /// <summary>
        /// Mở Captcha
        /// </summary>
        /// <param name="player"></param>
        /// <param name="onAnswer"></param>
        /// <param name="delayTicks"></param>
        public static bool OpenCaptcha(KPlayer player, Action<bool> onAnswer, int delayTicks = 30000)
        {
            /// Nếu không kích hoạt Captcha
            if (!ServerConfig.Instance.EnableCaptcha)
            {
                /// Thực hiện hàm Callback
                onAnswer?.Invoke(true);
                /// Hủy hàm Callback
                player.AnswerCaptcha = null;
                /// Bỏ qua
                return true;
            }

            /// Nếu chưa đến giờ
            if (KTGlobal.GetCurrentTimeMilis() - player.LastJailCaptchaTicks < delayTicks)
            {
                return false;
            }
            /// Đánh dấu thời gian xuất hiện Captcha
            player.LastJailCaptchaTicks = KTGlobal.GetCurrentTimeMilis();

            /// Ghi lại hàm Callback
            player.AnswerCaptcha = (isCorrect) => {
                /// Nếu trả lời đúng
                if (isCorrect)
                {
                    /// Đánh dấu thời điểm xuất hiện Captcha tiếp theo
                    player.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(ServerConfig.Instance.CaptchaAppearMinPeriod, ServerConfig.Instance.CaptchaAppearMaxPeriod);
                }

                /// Thực hiện hàm Callback
                onAnswer?.Invoke(isCorrect);
            };
            /// Thực hiện mở bảng Captcha
            player.GenerateCaptcha();
            /// Trả về kết quả
            return true;
        }
        #endregion
		
        #region Captcha For Battle
        /// <summary>
        /// Mở Captcha Chiến Trường
        /// </summary>
        /// <param name="player"></param>
        /// <param name="onAnswer"></param>
        /// <param name="delayTicks"></param>
        public static bool OpenCaptchaForBattle(KPlayer player, Action<bool> onAnswer, int delayTicks = 30000)
        {
            /// Nếu không kích hoạt Captcha
            if (!ServerConfig.Instance.EnableCaptchaForBattle)
            {
                /// Thực hiện hàm Callback
                onAnswer?.Invoke(true);
                /// Hủy hàm Callback
                player.AnswerCaptcha = null;
                /// Bỏ qua
                return true;
            }

            /// Nếu chưa đến giờ
            if (KTGlobal.GetCurrentTimeMilis() - player.LastJailCaptchaTicks < delayTicks)
            {
                return false;
            }
            /// Đánh dấu thời gian xuất hiện Captcha
            player.LastJailCaptchaTicks = KTGlobal.GetCurrentTimeMilis();

            /// Ghi lại hàm Callback
            player.AnswerCaptcha = (isCorrect) => {
                /// Nếu trả lời đúng
                if (isCorrect)
                {
                    /// Đánh dấu thời điểm xuất hiện Captcha tiếp theo
                    player.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(ServerConfig.Instance.CaptchaAppearMinPeriod, ServerConfig.Instance.CaptchaAppearMaxPeriod);
                }

                /// Thực hiện hàm Callback
                onAnswer?.Invoke(isCorrect);
            };
            /// Thực hiện mở bảng Captcha
            player.GenerateCaptcha();
            /// Trả về kết quả
            return true;
        }
        #endregion
    }
}
