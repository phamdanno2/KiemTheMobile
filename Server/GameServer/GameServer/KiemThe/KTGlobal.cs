using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Repute;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager.Shop;
using GameServer.KiemThe.Utilities;
using GameServer.KiemThe.Utilities.Algorithms;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;
using static GameServer.KiemThe.Logic.KTTaskManager;

namespace GameServer.KiemThe
{
    /// <summary>
    /// Các phương thức và đối tượng toàn cục của Kiếm Thế
    /// </summary>
    public static class KTGlobal
    {
        #region Config

        #region PK

        /// <summary>
        /// Thời gian miễn nhiễm sát thương của đối phương sau tỷ thí
        /// </summary>
        public const long ChallengeImmuneToEachOtherDamagesTick = 2000;

        /// <summary>
        /// Thời gian giãn cách khi chuyển trạng thái PK từ đánh nhau sang luyện công
        /// </summary>
        public const long TimeCooldownChangingPKModeToFight = 60000;

        /// <summary>
        /// Trị PK tối đa sau đó sẽ bị chuyển vào nhà lao tự động
        /// </summary>
        public const int MaxPKValueToForceSendToJail = 10;

        /// <summary>
        /// Thời gian giãn cách để xóa trạng thái tuyên chiến kể từ lần cuối tấn công
        /// </summary>
        public const long ActiveFightPKTick = 600000;

        /// <summary>
        /// ID bản đồ nhà lao
        /// </summary>
        public const int JailMapCode = 2130;

        /// <summary>
        /// Tọa độ X nhà lao
        /// </summary>
        public const int JailPosX = 4585;

        /// <summary>
        /// Tọa độ Y nhà lao
        /// </summary>
        public const int JailPosY = 2462;

        #endregion PK

        #region Trang bị

        /// <summary>
        /// Cấp độ cường hóa trang bị tối thiểu được tách Huyền Tinh
        /// </summary>
        public const int KD_MIN_ENHLEVEL_TO_SPLIT = 8;

        /// <summary>
        /// Hệ số giá trị Huyền Tinh có được so với gốc sau khi tách từ trang bị
        /// </summary>
        public const float KD_SPLIT_VALUE_COST = 0.7f;

        public const float KD_SPLITCRYTAL_VALUE_COST = 0.8f;

        #endregion Trang bị

        #region Kinh nghiệm

        /// <summary>
        /// Tổng số EXP max sẽ share cho bọn đồng đội
        /// </summary>
        public const int KD_MAX_TEAMATE_EXP_SHARE = 60;

        #endregion Kinh nghiệm

        #region Lưới Radar quét mục tiêu

        /// <summary>
        /// Nửa chiều rộng phạm vi lưới quét đối tượng xung quanh
        /// </summary>
        public const int RadarHalfWidth = 40;

        /// <summary>
        /// Nửa chiều cao phạm vi lưới quét đối tượng xung quanh
        /// </summary>
        public const int RadarHalfHeight = 40;

        #endregion Lưới Radar quét mục tiêu

        #region Khoảng cách chống BUG tốc chạy

        /// <summary>
        /// Khoảng cách lệch tối đa cho phép Client và Server
        /// <para>Tính cả Delay packet, etc...</para>
        /// </summary>
        public const float MaxClientServerMoveDistance = 100f;

        /// <summary>
        /// Thời gian Delay packet gửi từ Client về Server chấp nhận được
        /// </summary>
        public const long MaxClientPacketDelayAllowed = 500;

        #endregion Khoảng cách chống BUG tốc chạy

        #region Tỷ thí

        /// <summary>
        /// Thời gian tỷ thí tối đa
        /// </summary>
        public const long ChallengeMaxTimeTick = 600000;

        /// <summary>
        /// Kết thúc tỷ thí phục hồi sinh, nội, thể lực % tương ứng
        /// </summary>
        public const int ChallengeFinishHPMPStaminaReplenishPercent = 20;

        #endregion Tỷ thí

        #region Nhặt đồ

        /// <summary>
        /// Thời gian thằng khác có thể nhặt
        /// </summary>
        public static int GoodsPackOvertimeTick = 15;

        /// <summary>
        /// Xóa gói sau 90S
        /// </summary>
        public static int PackDestroyTimeTick = 60;

        /// <summary>
        /// Số task tối đa có thể theo dõi là mấy
        /// </summary>
        public static int TaskMaxFocusCount = 4;

        #endregion Nhặt đồ

        #region UpdateGuildMoney

        /// <summary>
        /// Update tiền cho bang hội
        /// </summary>
        /// <param name="GuildMoney"></param>
        /// <param name="GuildID"></param>
        /// <returns></returns>
        public static bool UpdateGuildMoney(int GuildMoney, int GuildID, KPlayer client)
        {
            if (client.GuildID == GuildID)
            {
                string strcmd = string.Format("{0}:{1}", GuildMoney, GuildID);

                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_UPDATE_ROLEGUILDMONEY, strcmd, GameManager.ServerId);
                if (null == dbFields) return false;
                if (dbFields.Length != 2)
                {
                    return false;
                }
                if (Convert.ToInt32(dbFields[1]) < 0)
                {
                    return false;
                }

                PlayerManager.ShowNotification(client, KTGlobal.CreateStringByColor("Quỹ phúc lợi bang hội gia tăng :" + GuildMoney + "", ColorType.Blue));
            }
            return true;
        }

        #endregion UpdateGuildMoney

        #region Quan Thiên Quyển

        /// <summary>
        /// Vật phẩm Quan Thiên Quyển dùng để kiểm tra vị trí người chơi
        /// </summary>
        public static List<int> CheckPositionScroll { get; } = new List<int>()
        {
            205,
        };

        #endregion Quan Thiên Quyển

        #region Loa chat

        /// <summary>
        /// Vật phẩm dùng trong kênh Chat đặc biệt
        /// </summary>
        public static List<int> SpecialChatMaterial { get; } = new List<int>()
        {
            382,
        };

        /// <summary>
        /// Vật phẩm dùng trong kênh Chat liên máy chủ
        /// </summary>
        public static List<int> CrossServerChatMaterial { get; } = new List<int>()
        {
            383,
        };

        #endregion Loa chat

        #region Hồi sinh

        /// <summary>
        /// Vật phẩm Cửu Chuyển Tục Mệnh Hoàn
        /// </summary>
        public static List<int> ReviveMedicine { get; } = new List<int>()
        {
            223, 538,
        };

        /// <summary>
        /// Hồi sinh sẽ phục hồi % sinh lực
        /// </summary>
        public const int DefaultReliveHPPercent = 20;

        /// <summary>
        /// Hồi sinh sẽ phục hồi % nội lực
        /// </summary>
        public const int DefaultReliveMPPercent = 20;

        /// <summary>
        /// Hồi sinh sẽ phục hồi % thể lực
        /// </summary>
        public const int DefaultReliveStaminaPercent = 20;

        #endregion Hồi sinh

        #region Phát hiện bẫy và tàng hình

        /// <summary>
        /// Khoảng cách cấp độ tối đa để có thể tự thấy đối phương ẩn thân mà không cần trạng thái phát hiện ẩn thân
        /// </summary>
        public const int DiffLevelToSeeInvisibleState = 9;

        /// <summary>
        /// Khoảng cách về cấp độ để có thể tự phát hiện bẫy mà không cần có trạng thái phát hiện bẫy
        /// </summary>
        public const int DiffLevelToDetectTrap = 9;

        #endregion Phát hiện bẫy và tàng hình

        #region Giãn cách thay đổi trạng thái cưỡi

        /// <summary>
        /// Thời gian giãn cách thay đổi trạng thái cưỡi ngựa liên tục
        /// </summary>
        public const long TickHorseStateChange = 5000;

        #endregion Giãn cách thay đổi trạng thái cưỡi

        #region Lua

        /// <summary>
        /// ID Script mặc định tiếp nhận các sự kiện của người chơi
        /// </summary>
        public const int DefaultPlayerScriptID = 999999;

        #endregion Lua

        #region Thời gian đặc biệt

        /// <summary>
        /// Giới hạn mỗi lần thao tác với thư
        /// </summary>
        public const long LimitMailCheckTick = 500;

        #endregion Thời gian đặc biệt

        #region Dialog

        /// <summary>
        /// Thời gian DELAY khi thực hiện Click vào NPC Dialog hoặc ItemDialog
        /// </summary>
        public const long DialogClickDelay = 500;

        /// <summary>
        /// Thời gian DELAY khi thực hiện Click vào vật phẩm để dùng
        /// </summary>
        public const long ItemClickDelay = 500;

        #endregion Dialog

        #endregion Config

        #region Items
        /// <summary>
        /// ID vật phẩm Thẻ đổi tên
        /// </summary>
        public const int ChangeNameCardItemID = 2167;
        #endregion

        #region RESETBANG

        /// <summary>
        /// Hàm thực hiện sắp xếp lại túi đồ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="notifyClient"></param>
        public static void ResetBagAllGoods(KPlayer client, bool notifyClient = true)
        {
            byte[] bytesCmd = null;
            // Nếu là túi đồ khác rỗng
            if (client.GoodsDataList != null)
            {
                // Lock danh sách trước
                lock (client.GoodsDataList)
                {
                    int index = 0;

                    //Duyệt từ đầu tới cuối vật phẩm
                    for (int i = 0; i < client.GoodsDataList.Count; i++)
                    {
                        // Nếu đồ đang mặc thifbor qua
                        if (client.GoodsDataList[i].Using > -1)
                        {
                            continue;
                        }

                        client.GoodsDataList[i].BagIndex = index++;

                        if (!Global.ResetBagGoodsData(client, client.GoodsDataList[i]))
                        {
                            break;
                        }
                    }

                    bytesCmd = DataHelper.ObjectToBytes<List<GoodsData>>(client.GoodsDataList);
                }
            }
            else
            {
                return;
            }

            if (notifyClient)
            {
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, bytesCmd, 0, bytesCmd.Length, (int)TCPGameServerCmds.CMD_SPR_RESETBAG);

                Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
            }
        }

        /// <summary>
        /// Hàm thực hiện sắp xếp lại túi đồ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="notifyClient"></param>
        public static void ResetPortalBagAllGoods(KPlayer client, bool notifyClient = true)
        {
            byte[] bytesCmd = null;
            // Nếu là túi đồ khác rỗng
            if (client.PortableGoodsDataList != null)
            {
                // Lock danh sách trước
                lock (client.PortableGoodsDataList)
                {
                    int index = 0;

                    //Duyệt từ đầu tới cuối vật phẩm
                    for (int i = 0; i < client.PortableGoodsDataList.Count; i++)
                    {
                        // Nếu đồ đang mặc thifbor qua
                        if (client.PortableGoodsDataList[i].Using > -1)
                        {
                            continue;
                        }

                        client.PortableGoodsDataList[i].BagIndex = index++;

                        if (!Global.ResetBagGoodsData(client, client.PortableGoodsDataList[i]))
                        {
                            break;
                        }
                    }

                    bytesCmd = DataHelper.ObjectToBytes<List<GoodsData>>(client.PortableGoodsDataList);
                }
            }
            else
            {
                return;
            }

            if (notifyClient)
            {
                TCPOutPacket tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(Global._TCPManager.TcpOutPacketPool, bytesCmd, 0, bytesCmd.Length, (int)TCPGameServerCmds.CMD_SPR_RESETPORTABLEBAG);

                Global._TCPManager.MySocketListener.SendData(client.ClientSocket, tcpOutPacket);
            }
        }

        #endregion RESETBANG

        #region Tốc chạy và tốc đánh

        #region Tốc chạy

        /// <summary>
        /// Chuyển tốc độ di chuyển sang dạng lưới Pixel
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <returns></returns>
        public static int MoveSpeedToPixel(int moveSpeed)
        {
            return moveSpeed * 15;
        }

        #endregion Tốc chạy

        #region Tốc đánh

        /// <summary>
        /// Kiểm tra đối tượng đã kết thúc thực thi động tác xuất chiêu chưa
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attackSpeed"></param>
        /// <returns></returns>
        public static bool FinishedUseSkillAction(GameServer.Logic.GameObject obj, int attackSpeed)
        {
            /// Tổng thời gian
            float frameDuration = KTGlobal.AttackSpeedToFrameDuration(attackSpeed);
            /// Tổng thời gian đã qua
            long tick = KTGlobal.GetCurrentTimeMilis() - obj.LastAttackTicks;
            /// Tổng thời gian yêu cầu
            long requireTick = (long)(frameDuration * 1000);

            //if (tick < requireTick)
            //{
            //    LogManager.WriteLog(LogTypes.RolePosition, string.Format("Toác VKL => Tick = {0}, Require Tick = {1}", tick, requireTick));
            //}

            /// Trả ra kết quả
            return tick >= requireTick;
        }

        /// <summary>
        /// Thời gian thực hiện động tác xuất chiêu tối thiểu
        /// </summary>
        public const float MinAttackActionDuration = 0.2f;

        /// <summary>
        /// Thời gian thực hiện động tác xuất chiêu tối đa
        /// </summary>
        public const float MaxAttackActionDuration = 0.8f;

        /// <summary>
        /// Thời gian cộng thêm giãn cách giữa các lần ra chiêu
        /// </summary>
        public const float AttackSpeedAdditionDuration = 0.1f;

        /// <summary>
        /// Tốc đánh tối thiểu
        /// </summary>
        public const int MinAttackSpeed = 0;

        /// <summary>
        /// Tốc đánh tối đa
        /// </summary>
        public const int MaxAttackSpeed = 100;

        /// <summary>
        /// Chuyển tốc độ đánh sang thời gian thực hiện động tác xuất chiêu
        /// </summary>
        /// <param name="attackSpeed"></param>
        /// <returns></returns>
        public static float AttackSpeedToFrameDuration(int attackSpeed)
        {
            /// Nếu tốc đánh nhỏ hơn tốc tối thiểu
            if (attackSpeed < KTGlobal.MinAttackSpeed)
            {
                attackSpeed = KTGlobal.MinAttackSpeed;
            }
            /// Nếu tốc đánh vượt quá tốc tối đa
            if (attackSpeed > KTGlobal.MaxAttackSpeed)
            {
                attackSpeed = KTGlobal.MaxAttackSpeed;
            }

            /// Tỷ lệ % so với tốc đánh tối đa
            float percent = attackSpeed / (float)KTGlobal.MaxAttackSpeed;

            /// Thời gian thực hiện động tác xuất chiêu
            float animationDuration = KTGlobal.MinAttackActionDuration + (KTGlobal.MaxAttackActionDuration - KTGlobal.MinAttackActionDuration) * (1f - percent);

            /// Trả về kết quả
            return animationDuration;
        }

        #endregion Tốc đánh

        #endregion Tốc chạy và tốc đánh

        #region Position

        /// <summary>
        /// Số lần thử tối đa tìm điểm ngẫu nhiên xung quanh không chứa vật cản mà có thể trực tiếp đi tới được theo đường thẳng
        /// </summary>
        private const int TryGetRandomLinearNoObsPointMaxTimes = 10;

        /// <summary>
        /// Chuyển từ tọa độ thực sang tọa độ lưới
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 WorldPositionToGridPosition(GameMap map, Vector2 position)
        {
            return new Vector2(position.x / map.MapGridWidth, position.y / map.MapGridHeight);
        }

        /// <summary>
        /// Chuyển từ tọa độ lưới sang tọa độ thực
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 GridPositionToWorldPosition(GameMap map, Vector2 position)
        {
            return new Vector2(position.x * map.MapGridWidth, position.y * map.MapGridHeight);
        }

        /// <summary>
        /// Chuyển từ tọa độ thực sang tọa độ lưới
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2Int WorldPositionToGridPosition(GameMap map, Vector2Int position)
        {
            return new Vector2Int(position.x / map.MapGridWidth, position.y / map.MapGridHeight);
        }

        /// <summary>
        /// Chuyển từ tọa độ lưới sang tọa độ thực
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2Int GridPositionToWorldPosition(GameMap map, Vector2Int position)
        {
            return new Vector2Int(position.x * map.MapGridWidth, position.y * map.MapGridHeight);
        }

        /// <summary>
        /// Tìm điểm đầu tiên nằm trên đường đi không chứa vật cản
        /// </summary>
        /// <param name="map"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <returns></returns>
        public static Vector2 FindLinearNoObsPoint(GameMap map, Vector2 fromPos, Vector2 toPos)
        {
            /// Vị trí hiện tại theo tọa độ lưới
            Vector2 fromGridPos = KTGlobal.WorldPositionToGridPosition(map, fromPos);
            /// Vị trí đích đến theo tọa độ lưới
            Vector2 toGridPos = KTGlobal.WorldPositionToGridPosition(map, toPos);

            Point fromGridPOINT = new Point((int)fromGridPos.x, (int)fromGridPos.y);
            Point toGridPOINT = new Point((int)toGridPos.x, (int)toGridPos.y);

            /// Nếu 2 vị trí trùng nhau
            if (fromGridPOINT == toGridPOINT)
            {
                return fromPos;
            }

            /// Nếu tìm thấy điểm không chứa vật cản
            if (Global.FindLinearNoObsPoint(map, fromGridPOINT, toGridPOINT, out Point newNoObsPoint))
            {
                Vector2 newNoObsPos = new Vector2((int)newNoObsPoint.X, (int)newNoObsPoint.Y);
                /// Trả ra kết quả
                return KTGlobal.GridPositionToWorldPosition(map, newNoObsPos);
            }
            /// Trả ra kết quả nếu không tìm thấy vị trí thỏa mãn
            return fromPos;
        }

        /// <summary>
        /// Trả về vị trí ngãu nhiên xung quanh không chứa vật cản có thể di chuyển trực tiếp đến được theo đường thẳng
        /// </summary>
        /// <param name="map"></param>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 GetRandomLinearNoObsPoint(GameMap map, Vector2 pos, float distance)
        {
            int triedTime = 0;
            do
            {
                triedTime++;

                //Vector2 randPos = KTMath.GetRandomPointInCircle(pos, distance);
                Vector2 randPos = KTMath.GetRandomPointAroundPos(pos, distance);

                /*
                Vector2 randGridPos = KTGlobal.WorldPositionToGridPosition(map, randPos);
                Point randGridPOINT = new Point((int) randGridPos.x, (int) randGridPos.y);

                /// Nếu vị trí có thể đến được
                if (map.MyNodeGrid.isWalkable((int) randGridPOINT.X, (int) randGridPOINT.Y))
                {
                    return randPos;
                }
                */

                Vector2 noObsPos = KTGlobal.FindLinearNoObsPoint(map, pos, randPos);
                if (noObsPos != pos)
                {
                    return noObsPos;
                }
            }
            while (triedTime <= KTGlobal.TryGetRandomLinearNoObsPointMaxTimes);

            return pos;
        }

        #endregion Position

        #region Thông báo hệ thống

        /// <summary>
        /// Gửi tin nhắn dưới kênh hệ thống đồng thời hiển thị ở dòng chữ chạy ngang trêu đầu tới toàn bộ người chơi
        /// </summary>
        /// <param name="message"></param>
        /// <param name="items">Danh sách vật phẩm đính kèm</param>
        public static void SendSystemEventNotification(string message, List<GoodsData> items = null)
        {
            int idx = 0;
            KPlayer player;
            while ((player = GameManager.ClientMgr.GetNextClient(ref idx)) != null)
            {
                GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, null, player, message, ChatChannel.System_Broad_Chat, items);
            }
        }

        /// <summary>
        /// Gửi tin nhắn chữ đỏ tới người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public static void SendDefaultChat(KPlayer player, string message)
        {
            string strinfo = string.Format("<color=red>{0}</color>", message);
            GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, strinfo);
        }

        /// <summary>
        /// Gửi tin nhắn dưới kênh hệ thống tới toàn bộ người chơi
        /// </summary>
        /// <param name="message">Tin nhắn</param>
        /// <param name="items">Danh sách vật phẩm đính kèm</param>
        public static void SendSystemChat(string message, List<GoodsData> items = null)
        {
            int idx = 0;
            KPlayer player;
            while ((player = GameManager.ClientMgr.GetNextClient(ref idx)) != null)
            {
                GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, null, player, message, ChatChannel.System, items);
            }
        }

        public static bool ProseccDatabaseServerChat(string MSG, int ExTag)
        {
            if (MSG.Contains("|"))
            {
                string[] Prams = MSG.Split('|');

                string CMD = Prams[0];

                switch (CMD)
                {
                    case "ADDBACKHOA":

                        int RoleID = Int32.Parse(Prams[1]);
                        string RoleName = Prams[2];
                        int MoneyAdd = Int32.Parse(Prams[3]);
                        string From = Prams[4];

                        KPlayer player = GameManager.ClientMgr.FindClient(RoleID);
                        if (player != null)
                        {
                            KTGlobal.AddMoney(player, MoneyAdd, MoneyType.BacKhoa, "PROFIT_GUILD_" + From);
                        }
                        else
                        {
                            KTGlobal.SendMail(null, RoleID, RoleName, "Trao thưởng ưu tú", "Trong tuần này bạn đã nằm trong top 5 thành viên ưu tú của bang hội!Vui lòng nhận thưởng đính kèm trong thư", 0, MoneyAdd);
                        }

                        LogManager.WriteLog(LogTypes.Guild, "Xử lý thêm bạc khóa thưởng ưu tú cho người chơi :" + RoleID + "|" + RoleName + "| Số bạc :" + MoneyAdd);

                        // Gửi thông báo tới toàn bang hội
                        KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, ExTag, "Người chơi [" + RoleName + "] đã nằm trong TOP 5 người chơi ưu tú của bang.Nhận được " + MoneyAdd + " bạc khóa từ Quỹ Thưởng của bang hội", null);

                        break;

                    case "CHANGERANK":

                        int RoleIDDes = Int32.Parse(Prams[1]);
                        int GuildID = Int32.Parse(Prams[2]);
                        int Rank = Int32.Parse(Prams[3]);

                        KPlayer playerFind = GameManager.ClientMgr.FindClient(RoleIDDes);
                        if (playerFind != null)
                        {
                            playerFind.GuildID = GuildID;

                            playerFind.GuildRank = Rank;
                            /// Thông báo danh hiệu thay đổi
                            KT_TCPHandler.NotifyOthersMyTitleChanged(playerFind);

                            /// Thông báo cập nhật thông tin gia tộc và bang hội
                            KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(playerFind);
                        }

                        break;
                }

                Console.WriteLine(MSG);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Thực hiện gửi thư cho 1 người chơi bất kể offline hay online
        /// </summary>
        /// <param name="goodsData"></param>
        /// <param name="RoleID"></param>
        /// <param name="RoleName"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="BoundToken"></param>
        /// <param name="BoundMoney"></param>
        /// <returns></returns>
        public static bool SendMail(List<GoodsData> goodsData, int RoleID, string RoleName, string Title, string Content, int BoundToken, int BoundMoney)
        {
            string mailGoodsString = "";

            /// Nếu danh sách vật phẩm tồn tại
            if (null != goodsData && goodsData.Count > 0)
            {
                /// Duyệt danh sách vật phẩm
                foreach (GoodsData itemGD in goodsData)
                {
                    mailGoodsString += KTMailManager.GoodsToMailGoodsString(itemGD) + "|";
                }
                /// Xóa ký tự thừa ở cuối
                mailGoodsString = mailGoodsString.Remove(mailGoodsString.Length - 1);
            }

            string strDbCmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", -1, "Hệ thống", RoleID, RoleName, Title, Content, BoundMoney, BoundToken, mailGoodsString);

            string[] fieldsData = null;
            /// Thực thi gửi thư
            fieldsData = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_SENDUSERMAIL, strDbCmd, GameManager.LocalServerId);

            /// Nếu có lỗi gì đó
            if (null == fieldsData || fieldsData.Length != 3)
            {
                return false;
            }

            /// Trả về ID thư
            return true;
        }

        /// <summary>
        /// Gửi chat mật cho ai đó
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="RoleID"></param>
        /// <param name="msg"></param>
        /// <param name="sender"></param>
        public static void SendPrivateChat(SocketListener sl, TCPOutPacketPool pool, int RoleID, string msg, KPlayer sender = null)
        {
            KPlayer FindPlayer = GameManager.ClientMgr.FindClient(RoleID);

            if (FindPlayer != null)
            {
                GameManager.ClientMgr.SendChatMessage(sl, pool, FindPlayer, sender, FindPlayer, msg, ChatChannel.Private, null);
            }
        }

        /// <summary>
        /// Chat bang
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="GuildID"></param>
        /// <param name="msg"></param>
        /// <param name="sender"></param>
        public static void SendGuildChat(SocketListener sl, TCPOutPacketPool pool, int GuildID, string msg, KPlayer sender = null, string SendName = "", List<GoodsData> Good = null)
        {
            if (GuildID <= 0)
            {
                return;
            }

            if (sender != null)
            {
                SendName = sender.RoleName;
            }

            int index = 0;
            KPlayer gc = null;

            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                if (gc.GuildID == GuildID)
                {
                    if (sender != null && gc.ClientSocket.IsKuaFuLogin)
                    {
                        if (sender.ZoneID == gc.ZoneID)
                        {
                            GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Guild, Good);
                        }
                    }
                    else
                    {
                        GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Guild, Good);
                    }
                }
            }
        }

        /// <summary>
        /// Gửi tin nhắn cho phái
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="FamilyID"></param>
        /// <param name="cmdText"></param>
        /// <param name="sender"></param>
        public static void SendFactionChat(SocketListener sl, TCPOutPacketPool pool, int FactionID, string msg, KPlayer sender = null)
        {
            if (FactionID <= 0)
            {
                return;
            }

            int index = 0;
            KPlayer gc = null;

            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                if (gc.m_cPlayerFaction.GetFactionId() == FactionID)
                {
                    if (sender != null && gc.ClientSocket.IsKuaFuLogin)
                    {
                        // Check nếu mà cùng máy chủ thì cho phép nhận tin nhắn
                        if (sender.ZoneID == gc.ZoneID)
                        {
                            GameManager.ClientMgr.SendChatMessage(sl, pool, gc, sender, gc, msg, ChatChannel.Faction, null);
                        }
                    }
                    else
                    {
                        GameManager.ClientMgr.SendChatMessage(sl, pool, gc, sender, gc, msg, ChatChannel.Faction, null);
                    }
                }
            }
        }

        /// <summary>
        /// Gửi tin nhắn cho cả gia tộc
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="FamilyID"></param>
        /// <param name="cmdText"></param>
        /// <param name="sender"></param>
        public static void SendFamilyChat(SocketListener sl, TCPOutPacketPool pool, int FamilyID, string msg, KPlayer sender = null, string SendName = "", List<GoodsData> Good = null)
        {
            if (FamilyID <= 0)
            {
                return;
            }

            if (sender != null)
            {
                SendName = sender.RoleName;
            }
            int index = 0;
            KPlayer gc = null;

            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                if (gc.FamilyID == FamilyID)
                {
                    if (sender != null && gc.ClientSocket.IsKuaFuLogin)
                    {
                        if (sender.ZoneID == gc.ZoneID)
                        {
                            // TODO FORWaRDING MSG VỀ READ SV
                            GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Family, Good);
                        }
                    }
                    else
                    {
                        GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Family, Good);
                    }
                }
            }
        }


        public static void SendTeamChat(SocketListener sl, TCPOutPacketPool pool, int TeamID, string msg, KPlayer sender = null, string SendName = "", List<GoodsData> Good = null)
        {
            if (TeamID < 0)
            {
                return;
            }

            if (sender != null)
            {
                SendName = sender.RoleName;
            }
            int index = 0;
            KPlayer gc = null;

            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                if (gc.TeamID == TeamID)
                {
                    if (sender != null && gc.ClientSocket.IsKuaFuLogin)
                    {
                        if (sender.ZoneID == gc.ZoneID)
                        {
                            // TODO FORWaRDING MSG VỀ READ SV
                            GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Team, Good);
                        }
                    }
                    else
                    {
                        GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, SendName, gc, msg, ChatChannel.Team, Good);
                    }
                }
            }
        }

        /// <summary>
        /// Kênh chát liên máy chủ dữ liệu sễ bắn ra từ GAMEDB
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="FamilyID"></param>
        /// <param name="msg"></param>
        /// <param name="sender"></param>
        public static void SendKuaFuServerChat(SocketListener sl, TCPOutPacketPool pool, string msg, string sender = "")
        {
            int index = 0;
            KPlayer gc = null;

            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
            {
                GameManager.ClientMgr.SendChatMessageWithName(sl, pool, gc, sender, gc, msg, ChatChannel.KuaFuLine, null);
            }
        }

        #endregion Thông báo hệ thống

        #region Path Finder

        /// <summary>
        /// Kiểm tra có đường đi giữa 2 nút không
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <returns></returns>
        public static bool HasPath(int mapCode, Point fromPos, Point toPos)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                return false;
            }

            /// Tọa độ lưới
            Vector2 fromGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, new UnityEngine.Vector2((int)fromPos.X, (int)fromPos.Y));
            Vector2 toGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, new UnityEngine.Vector2((int)toPos.X, (int)toPos.Y));
            /// Trả về kết quả
            return gameMap.MyNodeGrid.HasPath(new Point(fromGridPos.x, fromGridPos.y), new Point(toGridPos.x, toGridPos.y));
        }

        /// <summary>
        /// Kiểm tra có đường đi giữa 2 nút không
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <returns></returns>
        public static bool HasPath(int mapCode, UnityEngine.Vector2 fromPos, UnityEngine.Vector2 toPos)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                return false;
            }

            /// Tọa độ lưới
            Vector2 fromGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, fromPos);
            Vector2 toGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, toPos);
            /// Trả về kết quả
            return gameMap.MyNodeGrid.HasPath(new Point(fromGridPos.x, fromGridPos.y), new Point(toGridPos.x, toGridPos.y));
        }

        /// <summary>
        /// Tìm đường sử dụng giải thuật A*
        /// </summary>
        /// <param name="fromPos">Tọa độ thực điểm bắt đầu</param>
        /// <param name="toPos">Tọa độ thực điểm kết thúc</param>
        /// <returns></returns>
        private static List<Vector2> FindPathUsingAStar(int mapCode, Point fromPos, Point toPos)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            if (null == gameMap)
            {
                return new List<Vector2>();
            }

            /// Tìm đường sử dụng giải thuật A*
            List<int[]> nodeList = GlobalNew.FindPath(fromPos, toPos, mapCode);
            if (nodeList == null)
            {
                return new List<Vector2>();
            }
            nodeList.Reverse();

            List<Vector2> path = new List<Vector2>();

            for (int i = 0; i < nodeList.Count; i++)
            {
                path.Add(new Vector2(nodeList[i][0], nodeList[i][1]));
            }

            /// Làm mịn
            path = LineSmoother.SmoothPath(path, gameMap.MyNodeGrid.GetFixedObstruction());

            return path;
        }

        /// <summary>
        /// Tìm đường giữa 2 vị trí
        /// <para>Sử dụng giải thuật A* để tìm đường ngắn nhất</para>
        /// <para>Sử dụng thuật toán Ramer–Douglas–Peucker để làm mịn đường đi</para>
        /// </summary>
        /// <param name="go">Đối tượng</param>
        /// <param name="fromPos">Tọa độ thực vị trí bắt đầu</param>
        /// <param name="toPos">Tọa độ thực vị trí kết thúc</param>
        public static List<Vector2> FindPath(GameServer.Logic.GameObject go, Vector2 fromPos, Vector2 toPos)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[go.CurrentMapCode];
            if (null == gameMap)
            {
                return new List<Vector2>();
            }

            /// Nếu không có đường đi giữa 2 nút
            if (!KTGlobal.HasPath(gameMap.MapCode, fromPos, toPos))
            {
                return new List<Vector2>();
            }

            Vector2 fromGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, fromPos);
            Vector2 toGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, toPos);

            Point fromGridPOINT = new Point((int)fromGridPos.x, (int)fromGridPos.y);
            Point toGridPOINT = new Point((int)toGridPos.x, (int)toGridPos.y);

            /// Nếu vị trí đầu và cuối cùng một ô lưới thì cho chạy giữa 2 vị trí này luôn
            if (fromGridPOINT == toGridPOINT)
            {
                return new List<Vector2>()
                {
                    fromPos, toPos
                };
            }

            Point fromPosPOINT = new Point((int)fromPos.x, (int)fromPos.y);
            Point toPosPOINT = new Point((int)toPos.x, (int)toPos.y);

            /// Nếu đang ở vị trí có vật cản
            if (!gameMap.CanMove(fromPosPOINT))
            {
                /// Tìm vị trí bắt đầu mới không có vật cản
                Point noObsPos = Global.GetAGridPointIn4Direction(go.ObjectType, fromGridPOINT, go.CurrentMapCode);
                /// Đánh dấu lại vị trí bắt đầu
                fromGridPOINT = noObsPos;
                fromPos = KTGlobal.GridPositionToWorldPosition(gameMap, new Vector2((int)fromGridPOINT.X, (int)fromGridPOINT.Y));
                fromPosPOINT = new Point(fromPos.x, fromPos.y);
            }

            /// Nếu vị trí đích đến có vật cản
            if (!gameMap.CanMove(toPosPOINT))
            {
                Point noObsPos;
                /// Tìm vị trí kết thúc mới mà không có vật cản
                if (Global.FindLinearNoObsPoint(gameMap, fromGridPOINT, toGridPOINT, out noObsPos))
                {
                    /// Đánh dấu lại vị trí kết thúc
                    toGridPOINT = noObsPos;
                    toPos = KTGlobal.GridPositionToWorldPosition(gameMap, new Vector2((int)toGridPOINT.X, (int)toGridPOINT.Y));
                }
                /// Lỗi gì đó thì vào đây
				else
                {
                    /// Tìm vị trí kết thúc mới không có vật cản
                    noObsPos = Global.GetAGridPointIn4Direction(go.ObjectType, toGridPOINT, go.CurrentMapCode);
                    /// Đánh dấu lại vị trí kết thúc
                    toGridPOINT = noObsPos;
                    toPos = KTGlobal.GridPositionToWorldPosition(gameMap, new Vector2((int)toGridPOINT.X, (int)toGridPOINT.Y));
                }
                toPosPOINT = new Point(toPos.x, toPos.y);
            }

            /// Nếu vẫn không có đường đi giữa 2 vị trí
            if (!gameMap.CanMove((int)fromGridPOINT.X, (int)fromGridPOINT.Y) || !gameMap.CanMove((int)toGridPOINT.X, (int)toGridPOINT.Y))
            {
                return new List<Vector2>();
            }

            /// Sử dụng A* tìm đường đi
            List<Vector2> nodes = KTGlobal.FindPathUsingAStar(go.CurrentMapCode, fromPosPOINT, toPosPOINT);

            /// Nếu danh sách nút tìm được nhỏ hơn 2
            if (nodes.Count < 2)
            {
                return new List<Vector2>();
            }

            /// Danh sách điểm trên đường đi
            List<Vector2> result = new List<Vector2>();
            result.Add(fromPos);

            /// Thêm tất cả các nút tìm được trên đường đi vào danh sách
            for (int i = 1; i < nodes.Count; i++)
            {
                result.Add(KTGlobal.GridPositionToWorldPosition(gameMap, nodes[i]));
            }
            result[result.Count - 1] = toPos;

            return result;
        }

        #endregion Path Finder

        #region PropertyDictionary

        /// <summary>
        /// Đọc dữ liệu PropertyDictionary
        /// </summary>
        public static void ReadPropertyDictionary()
        {
            XElement xmlNode = Global.GetGameResXml("Config/KT_Skill/PropertyDictionary.xml");
            PropertyDefine.Parse(xmlNode);
        }

        #endregion PropertyDictionary

        #region PKRegular

        // TODO SAU CHUYỂN HẾT THÀNH CONFIG

        public static int PKDamageRate = 25;

        public static int NpcPKDamageRate = 750;

        public static int EnmityExpLimitPercent = -50;

        public static int FightExpLimitPercent = -50;

        public static int PKStateChangeLimitTime = 180;

        #endregion PKRegular

        #region SkillSettings

        public static int[] m_arPhysicsEnhance = new int[(int)KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_NUM];

        public static int HitPercentMin = 5;// Tỉ lệ chính xác nhỏ nhất

        public static int HitPercentMax = 95; // TỈ lệ chính xác tối đa

        public static int StateBaseRateParam = 250; // tỉ lệ dính phải trạng thái base
        public static int StateBaseTimeParam = 250; // Thời gian tồn tại trạng thái base

        public static int DeadlyStrikeBaseRate = 2000; // tỉ lệ dính phải trạng thái base
        public static int DeadlyStrikeDamagePercent = 180; // Thời gian tồn tại trạng thái base

        public static int DefenceMaxPercent = 85; // Bor qua kháng max

        public static int IngoreResistMaxP = 95; // Bor qua kháng max

        public static int SeriesTrimMax = 95;

        public static int FatallyStrikePercent = 50; // Chí mạng Apply Dame tối đa

        public static int SeriesTrimParam1 = 1;

        public static int SeriesTrimParam2 = 8;
        public static int SeriesTrimParam3 = 200;

        public static int SeriesTrimParam4 = 0; // Cường hóa sát thương của thằng đánh
        public static int SeriesTrimParam5 = 10;

        public static int WeakDamagePercent = 50; // Tỉ lệ thiệt hại khi bị suy yếu
        public static int SlowAllPercent = 50; // Tốc dộ sẽ làm chậm
        public static int BurnDamagePercent = 150; // Dame bị bỏng tính thêm

        public static int AngerTime = 10; // Thời gian nộ tính bằng giây

        public static int DropRate = 5; // Tỉ lệ rơi ở quái thường mỗi đơn vị là 1 % MAX là 100%

        public static void NotifyGoood(KPlayer _Play, int BangIndex, int Count, int IsUsing, ModGoodsTypes TypeMod, int GoodID, int Hit, int Site, int State)
        {
            SCModGoods scData = new SCModGoods()
            {
                BagIndex = BangIndex,
                Count = Count,
                IsUsing = IsUsing,
                ModType = (int)TypeMod,
                ID = GoodID,
                NewHint = Hit,
                Site = Site,
                State = State,
            };

            _Play.SendPacket((int)TCPGameServerCmds.CMD_SPR_MOD_GOODS, scData);
        }

        /// <summary>
        /// Cập nhật thông tin vật phẩm theo danh sách Modify trong túi đồ người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="DBID"></param>
        /// <param name="ModifyDict"></param>
        public static void ModifyGoods(KPlayer client, int DBID, Dictionary<UPDATEITEM, object> ModifyDict)
        {
            if (null == client.GoodsDataList)
                return;

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    if (client.GoodsDataList[i].Id == DBID)
                    {
                        if (ModifyDict.ContainsKey(UPDATEITEM.BAGINDEX))
                        {
                            client.GoodsDataList[i].BagIndex = (int)ModifyDict[UPDATEITEM.BAGINDEX];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.SERIES))
                        {
                            client.GoodsDataList[i].Series = (int)ModifyDict[UPDATEITEM.SERIES];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.SITE))
                        {
                            client.GoodsDataList[i].Site = (int)ModifyDict[UPDATEITEM.SITE];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.END_TIME))
                        {
                            client.GoodsDataList[i].Endtime = ((string)ModifyDict[UPDATEITEM.END_TIME]).Replace("#", ":");
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.OTHER_PRAM))
                        {
                            client.GoodsDataList[i].OtherParams = (Dictionary<ItemPramenter, string>)ModifyDict[UPDATEITEM.OTHER_PRAM];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.PROPS))
                        {
                            client.GoodsDataList[i].Props = (string)ModifyDict[UPDATEITEM.PROPS];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.STRONG))
                        {
                            client.GoodsDataList[i].Strong = (int)ModifyDict[UPDATEITEM.STRONG];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.BINDING))
                        {
                            client.GoodsDataList[i].Binding = (int)ModifyDict[UPDATEITEM.BINDING];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.FORGE_LEVEL))
                        {
                            client.GoodsDataList[i].Forge_level = (int)ModifyDict[UPDATEITEM.FORGE_LEVEL];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.USING))
                        {
                            client.GoodsDataList[i].Using = (int)ModifyDict[UPDATEITEM.USING];
                        }

                        if (ModifyDict.ContainsKey(UPDATEITEM.GCOUNT))
                        {
                            if ((int)ModifyDict[UPDATEITEM.GCOUNT] <= 0)
                            {

                                /// Đánh dấu vào đây cho chắc ăn
                                client.GoodsDataList[i].GCount = 0;
                                client.GoodsDataList.RemoveAt(i);
                            }
                            else
                            {
                                client.GoodsDataList[i].GCount = (int)ModifyDict[UPDATEITEM.GCOUNT];
                            }
                        }

                        break;
                    }
                }
            }

            return;
        }

        public static string ItemUpdateScriptBuild(Dictionary<UPDATEITEM, object> Input)
        {
            string OutPut = "";

            for (int i = 0; i <= 14; i++)
            {
                UPDATEITEM _Item = (UPDATEITEM)i;

                if (Input.ContainsKey(_Item))
                {
                    if (Input[_Item].GetType() == typeof(string))
                    {
                        string Value = (string)Input[_Item];

                        OutPut += Value + ":";
                    }
                    else if (Input[_Item].GetType() == typeof(int))
                    {
                        int Value = (int)Input[_Item];

                        OutPut += Value + ":";
                    }
                    else if (Input[_Item].GetType() == typeof(Dictionary<ItemPramenter, string>))
                    {
                        Dictionary<ItemPramenter, string> Value = (Dictionary<ItemPramenter, string>)Input[_Item];

                        byte[] ItemDataByteArray = DataHelper.ObjectToBytes(Value);

                        string otherPramer = Convert.ToBase64String(ItemDataByteArray);

                        OutPut += otherPramer + ":";
                    }
                }
                else
                {
                    OutPut += "*:";
                }
            }

            return OutPut;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static String SexToString(int Sex)
        {
            if (Sex == 0)
                return "Nam";
            else
                return "Nữ";
        }

        public static string GetSeriesText(int seriesID)
        {
            switch (seriesID)
            {
                case 1:
                    return string.Format("<color={0}>{1}</color>", "#ffe552", "Kim");

                case 2:
                    return string.Format("<color={0}>{1}</color>", "#77ff33", "Mộc");

                case 3:
                    return string.Format("<color={0}>{1}</color>", "#61d7ff", "Thủy");

                case 4:
                    return string.Format("<color={0}>{1}</color>", "#ff4242", "Hỏa");

                case 5:
                    return string.Format("<color={0}>{1}</color>", "#debba0", "Thổ");

                default:
                    return string.Format("<color={0}>{1}</color>", "#cccccc", "Vô");
            }
        }

        public static int GetSkillStyleDef(string StyleInput)
        {
            switch (StyleInput)
            {
                case "meleephysicalattack":
                    return 1;

                case "rangephysicalattack":
                    return 2;

                case "rangemagicattack":
                    return 3;

                case "aurarangemagicattack":
                    return 4;

                case "defendcurse":
                    return 5;

                case "attackcurse":
                    return 6;

                case "fightcurse":
                    return 7;

                case "curse":
                    return 8;

                case "trap":
                    return 9;

                case "initiativeattackassistant":
                    return 10;

                case "initiativedefendassistant":
                    return 11;

                case "initiativefightassistant":
                    return 12;

                case "stolenassistant":
                    return 13;

                case "initiativeattackassistantally":
                    return 14;

                case "initiativedefendassistantally":
                    return 15;

                case "initiativefightassistantally":
                    return 16;

                case "specialattack":
                    return 17;

                case "disable":
                    return 18;

                case "assistant":
                    return 19;

                case "nonpcskill":
                    return 20;
            }
            return -1;
        }

        #endregion SkillSettings

        #region NGUHANHTUONGKHAC

        public static int[] g_nAccrueSeries = new int[(int)KE_SERIES_TYPE.series_num]; // TƯƠNG SINH
        public static int[] g_nConquerSeries = new int[(int)KE_SERIES_TYPE.series_num]; // TƯƠNG KHẮC
        public static int[] g_nAccruedSeries = new int[(int)KE_SERIES_TYPE.series_num]; // ĐƯỢC TƯƠNG SINH
        public static int[] g_nConqueredSeries = new int[(int)KE_SERIES_TYPE.series_num]; // BỊ KHẮC

        public static void LoadAccrueSeries()
        {
            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_none] = (int)KE_SERIES_TYPE.series_none;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_none] = (int)KE_SERIES_TYPE.series_none;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_none] = (int)KE_SERIES_TYPE.series_none;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_none] = (int)KE_SERIES_TYPE.series_none;

            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_metal] = (int)KE_SERIES_TYPE.series_water;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_metal] = (int)KE_SERIES_TYPE.series_wood;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_metal] = (int)KE_SERIES_TYPE.series_earth;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_metal] = (int)KE_SERIES_TYPE.series_fire;
            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_wood] = (int)KE_SERIES_TYPE.series_fire;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_wood] = (int)KE_SERIES_TYPE.series_earth;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_wood] = (int)KE_SERIES_TYPE.series_water;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_wood] = (int)KE_SERIES_TYPE.series_metal;
            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_water] = (int)KE_SERIES_TYPE.series_wood;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_water] = (int)KE_SERIES_TYPE.series_fire;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_water] = (int)KE_SERIES_TYPE.series_metal;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_water] = (int)KE_SERIES_TYPE.series_earth;
            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_fire] = (int)KE_SERIES_TYPE.series_earth;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_fire] = (int)KE_SERIES_TYPE.series_metal;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_fire] = (int)KE_SERIES_TYPE.series_wood;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_fire] = (int)KE_SERIES_TYPE.series_water;
            g_nAccrueSeries[(int)KE_SERIES_TYPE.series_earth] = (int)KE_SERIES_TYPE.series_metal;
            g_nConquerSeries[(int)KE_SERIES_TYPE.series_earth] = (int)KE_SERIES_TYPE.series_water;
            g_nAccruedSeries[(int)KE_SERIES_TYPE.series_earth] = (int)KE_SERIES_TYPE.series_fire;
            g_nConqueredSeries[(int)KE_SERIES_TYPE.series_earth] = (int)KE_SERIES_TYPE.series_wood;
        }

        public static bool g_IsAccrue(int nSrcSeries, int nDesSeries)
        {
            return g_InternalIsAccrueConquer(g_nAccrueSeries, nSrcSeries, nDesSeries);
        }

        public static bool g_IsConquer(int nSrcSeries, int nDesSeries)
        {
            return g_InternalIsAccrueConquer(g_nConquerSeries, nSrcSeries, nDesSeries);
        }

        public static bool g_InternalIsAccrueConquer(int[] pAccrueConquerTable, int nSrcSeries, int nDesSeries)
        {
            if (nSrcSeries < (int)KE_SERIES_TYPE.series_none || nSrcSeries >= (int)KE_SERIES_TYPE.series_num)
                return false;

            return nDesSeries == pAccrueConquerTable[nSrcSeries];
        }

        #endregion NGUHANHTUONGKHAC

        #region Màu trang bị

        #region RANDOMSTRING

        public static string RandomStr(int leng)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new System.Random();
            var result = new string(
                Enumerable.Repeat(chars, leng)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            string gifcode = result;

            return gifcode;
        }

        #endregion RANDOMSTRING

        /// <summary>
        /// Trả về mã màu trang bị tương ứng
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static string GetItemNameColor(GoodsData itemGD)
        {
            /// Nếu không có dữ liệu hoặc không phải trang bị thì màu trắng
            if (itemGD == null || !ItemManager.IsEquip(itemGD))
            {
                return "#ffffff";
            }

            long Taiphu = 0;

            StarLevelStruct starLevelStruct = ItemManager.ItemValueCalculation(itemGD, out Taiphu);
            if (starLevelStruct == null)
            {
                return "#ffffff";
            }
            else
            {
                switch (starLevelStruct.NameColor)
                {
                    case "green":
                        {
                            return "#78ff1f";
                        }
                    case "blue":
                        {
                            return "#14a9ff";
                        }
                    case "purple":
                        {
                            return "#d82efa";
                        }
                    case "orange":
                        {
                            return "#ff8e3d";
                        }
                    case "yellow":
                        {
                            return "#fffb29";
                        }
                    default:
                        {
                            return "#ffffff";
                        }
                }
            }
        }

        /// <summary>
        /// Trả về chuỗi mô tả thông tin vật phẩm ở kênh Chat
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static string GetItemDescInfoStringForChat(GoodsData itemGD)
        {
            if (itemGD == null)
            {
                return "";
            }

            /// Dữ liệu vật phẩm
            string itemName = KTGlobal.GetItemName(itemGD);
            string itemDescString = string.Format("<color={0}><link=\"ITEM_{1}\">[{2}]</link></color>", KTGlobal.GetItemNameColor(itemGD), itemGD.Id, itemName);

            /// Trả về kết quả
            return itemDescString;
        }

        /// <summary>
        /// Trả về tên vật phẩm tương ứng
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static string GetItemName(GoodsData itemGD)
        {
            /// Nếu vật phẩm không tồn tại
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                return "";
            }

            /// Tên vật phẩm
            string name = itemData.Name;

            /// Tên đi kèm đằng sau loại trang bị
            string suffixName = "";

            /// Nếu tồn tại danh sách thuộc tính ẩn
            if (itemData.HiddenProp != null)
            {
                if (itemData.HiddenProp.Count > 0)
                {
                    /// Tìm danh sách thuộc tính tương ứng đảo ngược
                    List<PropMagic> props = itemData.HiddenProp.OrderByDescending(x => x.Index).ToList();

                    /// Duyệt danh sách thuộc tính
                    foreach (PropMagic prop in props)
                    {
                        if (prop.MagicName.Length > 0)
                        {
                            /// Thuộc tính tương ứng
                            MagicAttribLevel magicAttrib = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == prop.MagicName && x.Level == prop.MagicLevel).FirstOrDefault();

                            /// Nếu tìm thấy
                            if (magicAttrib != null)
                            {
                                /// Trả về tên đi kèm đằng sau loại trang bị
                                suffixName = magicAttrib.Suffix;
                                break;
                            }
                        }
                    }
                }
            }

            /// Nếu tên đi kèm đằng sau chưa tồn tại
            if (string.IsNullOrEmpty(suffixName))
            {
                /// Nếu tồn tại danh sách thuộc tính ngẫu nhiên
                if (itemData.GreenProp != null)
                {
                    if (itemData.GreenProp.Count > 0)
                    {
                        /// Tìm danh sách thuộc tính tương ứng đảo ngược
                        List<PropMagic> props = itemData.GreenProp.OrderByDescending(x => x.Index).ToList();

                        /// Duyệt danh sách thuộc tính
                        foreach (PropMagic prop in props)
                        {
                            if (prop.MagicName.Length > 0)
                            {
                                /// Thuộc tính tương ứng
                                MagicAttribLevel magicAttrib = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == prop.MagicName && x.Level == prop.MagicLevel).FirstOrDefault();

                                /// Nếu tìm thấy
                                if (magicAttrib != null)
                                {
                                    /// Trả về tên đi kèm đằng sau loại trang bị
                                    suffixName = magicAttrib.Suffix;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            /// Nếu tồn tại tên đi kèm loại trang bị
            if (!string.IsNullOrEmpty(suffixName))
            {
                name += string.Format(" - {0}", suffixName);
            }

            /// Nếu là trang bị có thể cường hóa
            if (ItemManager.KD_ISEQUIP(itemGD.GoodsID) && ItemManager.CanEquipBeEnhance(itemData))
            {
                /// Cấp cường hóa nếu có
                if (itemGD.Forge_level > 0)
                {
                    name += string.Format(" +{0}", itemGD.Forge_level);
                }
                else
                {
                    name += " - Chưa cường hóa";
                }
            }

            /// Trả về kết quả
            return name;
        }

        /// <summary>
        /// Trả về tên vật phẩm kèm mã màu HTML dạng RichText
        /// </summary>
        /// <param name="itemGD"></param>
        /// <returns></returns>
        public static string GetItemNameWithHTMLColor(GoodsData itemGD)
        {
            string htmlColor = KTGlobal.GetItemNameColor(itemGD);
            string itemName = KTGlobal.GetItemName(itemGD);

            return string.Format("<color={0}>[{1}]</color>", htmlColor, itemName);
        }

        /// <summary>
        /// Trả về tên vật phẩm có ID tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetItemName(int id)
        {
            ItemData itemData = ItemManager.GetItemTemplate(id);
            if (itemData == null)
            {
                return "";
            }
            return itemData.Name;
        }

        #endregion Màu trang bị

        #region GETMONEYHIENTAI

        public static int GetMoneyhave(KPlayer player, MoneyType Type)
        {
            switch (Type)
            {
                case MoneyType.Bac:
                    {
                        return player.Money;
                    }
                case MoneyType.BacKhoa:
                    {
                        return player.BoundMoney;
                    }
                case MoneyType.Dong:
                    {
                        return player.Token;
                    }
                case MoneyType.DongKhoa:
                    {
                        return player.BoundToken;
                    }
            }

            return 0;
        }

        #endregion GETMONEYHIENTAI

        #region CheckTuiDo

        /// <summary>
        /// Trả về vị trí đầu tiên trống trống ở túi đồ
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <returns></returns>
        public static int GetIdleSlotOfBagGoods(KPlayer client)
        {
            int idelPos = -1;

            if (client.GoodsDataList == null)
            {
                return 0;
            }

            lock (client.GoodsDataList)
            {
                for (int i = 0; i <= client.BagNum; i++)
                {
                    var FindByPos = client.GoodsDataList.Where(x => x != null && x.BagIndex == i && x.Site == 0).FirstOrDefault();
                    if (FindByPos == null)
                    {
                        idelPos = i;
                        break;
                    }
                }
            }

            return idelPos;
        }

        /// <summary>
        /// Trả về vị trí đầu tiên trống trong thương khố
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetIdleSlotOfPortableBag(KPlayer client)
        {
            int idelPos = -1;

            if (client.PortableGoodsDataList == null)
            {
                return 0;
            }

            lock (client.PortableGoodsDataList)
            {
                for (int i = 0; i <= 100; i++)
                {
                    var FindByPos = client.PortableGoodsDataList.Where(x => x != null && x.BagIndex == i && x.Site == 1).FirstOrDefault();
                    if (FindByPos == null)
                    {
                        idelPos = i;
                        break;
                    }
                }
            }

            return idelPos;
        }

        /// <summary>
        /// Trả về tổng số ô trống cần để nhận vật phẩm tương ứng
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int GetTotalSpacesNeedToTakeItem(int itemID, int count)
        {
            if (ItemManager._TotalGameItem.TryGetValue(itemID, out ItemData itemData))
            {
                /// Nếu là trang bị
                if (ItemManager.KD_ISEQUIP(itemData.Genre))
                {
                    return count;
                }
                /// Tổng stack trên 1 ô
                int nStack = itemData.Stack;
                if (nStack <= 0)
                {
                    nStack = 1;
                }

                /// Trả về số lượng
                return count / nStack + (count % nStack == 0 ? 0 : 1);
            }
            return count;
        }

        /// <summary>
        /// Kiểm tra người chơi có đủ số ô trống tương ứng trong túi đồ không
        /// </summary>
        /// <param name="spaceNeed"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsHaveSpace(int spaceNeed, KPlayer player)
        {
            bool IsHave = false;

            /// Nếu danh sách vật phẩm NULL
            if (player.GoodsDataList == null)
            {
                return true;
            }

            int totalGridNum = 0;
            lock (player.GoodsDataList)
            {
                totalGridNum = player.GoodsDataList.Where(x => x.Site == 0 && x.GCount > 0 && x.Using < 0).Count();
            }

            // TODO LẤY RA SỐ Ô HIỆN TẠI CỦA CLIENT
            int totalMaxGridCount = player.BagNum;

            int LessSpace = totalMaxGridCount - totalGridNum;

            if (LessSpace >= spaceNeed)
            {
                IsHave = true;
            }
            else
            {
                IsHave = false;
            }

            return IsHave;
        }

        /// <summary>
        /// Số ô tối đa trong thương khố
        /// </summary>
        public const int MaxPortableBagItemCount = 100;

        #endregion CheckTuiDo

        #region LIENSV

        public static bool GotoMap(KPlayer client, int toMapCode)
        {
            TCPManager tcpMgr = TCPManager.getInstance();
            TCPOutPacketPool pool = TCPOutPacketPool.getInstance();

            if (!GameManager.MapMgr.DictMaps.ContainsKey(toMapCode))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Bản đồ không tồn tại, mapCode={0}", toMapCode));
                return false;
            }

            // nếu client đang ở máy chủ liên sv
            if (client.ClientSocket.IsKuaFuLogin)
            {
                Global.ModifyMapRecordData(client, (ushort)toMapCode, 0, 0, 0);

                KuaFuManager.getInstance().GotoLastMap(client);

                return true;
            }
            else if (KuaFuManager.getInstance().IsKuaFuMap(toMapCode))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("GotoMap denied, mapCode={0},IsKuaFuLogin={1}", toMapCode, client.ClientSocket.IsKuaFuLogin));
                return false;
            }

            GameMap gameMap = null;
            if (GameManager.MapMgr.DictMaps.TryGetValue(toMapCode, out gameMap))
            {
                int toMapX = -1;
                int toMapY = -1;
                int toDirection = Global.GetRandomNumber(0, 8);

                // NOTIFY Về cho nó đổi bản đồ
                GameManager.ClientMgr.NotifyChangeMap(tcpMgr.MySocketListener, pool, client, toMapCode, toMapX, toMapY, toDirection);
            }

            return true;
        }

        #endregion LIENSV

        #region String Color

        public static string CreateStringByColor(string Input, ColorType Color)
        {
            string Ref = "";

            if (Color == ColorType.Done)
            {
                Ref = "<color=#00EC1C>" + Input + "</color>";
            }
            else if (Color == ColorType.Accpect)
            {
                Ref = "<color=#ECD600>" + Input + "</color>";
            }
            else if (Color == ColorType.Importal)
            {
                Ref = "<color=#ec0000>" + Input + "</color>";
            }
            else if (Color == ColorType.Green)
            {
                Ref = "<color=#00ff2a>" + Input + "</color>";
            }
            else if (Color == ColorType.Pure)
            {
                Ref = "<color=#e100ff>" + Input + "</color>";
            }
            else if (Color == ColorType.Blue)
            {
                Ref = "<color=#001eff>" + Input + "</color>";
            }
            else if (Color == ColorType.Yellow)
            {
                Ref = "<color=yellow>" + Input + "</color>";
            }
            else if (Color == ColorType.Normal)
            {
                Ref = Input;
            }

            return Ref;
        }

        #endregion String Color

        public static int DEF_7DAY_TIME = 60 * 24 * 7;

        public static int DEF_30DAY_TIME = 60 * 24 * 30;

        #region HSDOFSPECIALITEM

        public static int GetItemExpiesTime(int ItemID)
        {
            switch (ItemID)
            {
                case 496:
                    return DEF_7DAY_TIME;

                case 555:
                    return DEF_30DAY_TIME;

                case 557:
                    return DEF_30DAY_TIME;
            }

            return -1;
        }

        #endregion HSDOFSPECIALITEM

        /// <summary>
        /// Trả về vinh dự tài phú tương ứng
        /// </summary>
        /// <param name="ValuInput"></param>
        /// <returns></returns>
        public static int GetRankHonor(long ValuInput)
        {
            int FinalRank = (int)(ValuInput / 10000);

            if (FinalRank >= 600 && FinalRank < 1000)
            {
                return 1;
            }
            else if (FinalRank >= 1000 && FinalRank < 1500)
            {
                return 2;
            }
            else if (FinalRank >= 1500 && FinalRank < 3000)
            {
                return 3;
            }
            else if (FinalRank >= 3000 && FinalRank < 6000)
            {
                return 4;
            }
            else if (FinalRank >= 6000 && FinalRank < 12000)
            {
                return 5;
            }
            else if (FinalRank >= 12000 && FinalRank < 24000)
            {
                return 6;
            }
            else if (FinalRank >= 24000 && FinalRank < 60000)
            {
                return 7;
            }
            else if (FinalRank >= 60000 && FinalRank < 120000)
            {
                return 8;
            }
            else if (FinalRank >= 120000 && FinalRank < 300000)
            {
                return 9;
            }
            else if (FinalRank >= 300000)
			{
                return 10;
            }

            return 0;
        }

        /// <summary>
        /// Đối tượng Random
        /// </summary>
        private static readonly System.Random random = new System.Random();

        /// <summary>
        /// Trả về số nguyên trong khoảng tương ứng
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            if (max < min)
            {
                max = min;
            }
            lock (KTGlobal.random)
            {
                int nRand = KTGlobal.random.Next(min, max + 1);
                if (nRand > max)
                {
                    nRand = max;
                }
                return nRand;
            }
        }

        /// <summary>
        /// Trả về số thực trong khoảng tương ứng
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetRandomNumber(float min, float max)
        {
            lock (KTGlobal.random)
            {
                return (float)KTGlobal.random.NextDouble() * (max - min) + min;
            }
        }

        /// <summary>
        /// Trả về số thực trong khoảng tương ứng
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double GetRandomNumber(double min, double max)
        {
            lock (KTGlobal.random)
            {
                return KTGlobal.random.NextDouble() * (max - min) + min;
            }
        }

        #region Newbie Villages

        /// <summary>
        /// Danh sách các tân thủ thôn khi tạo nhân vật sẽ được vào
        /// </summary>
        public static List<NewbieVillage> NewbieVillages { get; } = new List<NewbieVillage>();

        /// <summary>
        /// Đọc dữ liệu danh sách các tân thủ thôn khi tạo nhân vật sẽ được vào
        /// </summary>
        public static void LoadNewbieVillages()
        {
            KTGlobal.NewbieVillages.Clear();

            XElement xmlNode = Global.GetGameResXml("Config/KT_Setting/NewbieVillages.xml");
            foreach (XElement node in xmlNode.Elements())
            {
                NewbieVillage newbieVillage = NewbieVillage.Parse(node);
                KTGlobal.NewbieVillages.Add(newbieVillage);
            }
        }

        #endregion Newbie Villages

        #region KTPK

        public static string PKPunishStr = "Config/KT_Setting/PKPunish.xml";

        public static int KD_MAX_PKVALUE = 10;

        public static List<PKPunish> LstPKPunish = new List<PKPunish>();

        public static PKPunish GetPkConfig(int PkValue)
        {
            var find = LstPKPunish.Where(x => x.PKValue == PkValue).FirstOrDefault();
            if (find != null)
            {
                return find;
            }
            return null;
        }

        public static void LoadPKPunish()
        {
            string Files = Global.GameResPath(PKPunishStr);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(List<PKPunish>));
                LstPKPunish = serializer.Deserialize(stream) as List<PKPunish>;
            }
        }

        #endregion KTPK

        #region Timer

        /// <summary>
        /// Tạo luồng thực hiện công việc sau khoảng thời gian
        /// </summary>
        /// <param name="work">Công việc</param>
        /// <param name="interval">Thời gian chờ trước khi thực thi</param>
        /// <returns></returns>
        public static KTSchedule SetTimeout(Action work, float interval)
        {
            KTSchedule schedule = new KTSchedule()
            {
                Name = "Timer from GLOBAL",
                Interval = interval,
                Loop = false,
                Work = work,
            };
            KTTaskManager.Instance.AddSchedule(schedule);
            return schedule;
        }

        #endregion Timer

        /// <summary>
        /// Trả về giờ hệ thống hiện tại dưới đơn vị Mili giây
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimeMilis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Trả về giờ quốc tế dưới đơn vị Mili giây
        /// </summary>
        /// <returns></returns>
        public static long GetGlobalTimeMilis()
        {
            return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Trả về thứ tự tuần trong tháng
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentWeekOfMonth()
        {
            CultureInfo curr = CultureInfo.CurrentCulture;
            int week = curr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week;
        }

        #region Date Time

        /// <summary>
        /// Hiển thị thời gian
        /// <para>Thời gian dạng giây</para>
        /// </summary>
        public static string DisplayTime(float timeInSec)
        {
            int sec = (int)timeInSec;
            if (sec >= 86400)
            {
                int nDay = sec / 86400;
                return string.Format("{0} ngày", nDay);
            }
            else if (sec >= 3600)
            {
                int nHour = sec / 3600;
                return string.Format("{0} giờ", nHour);
            }
            else
            {
                int nMinute = sec / 60;
                int nSecond = sec - nMinute * 60;
                string secondString = nSecond.ToString();
                while (secondString.Length < 2)
                {
                    secondString = "0" + secondString;
                }
                return string.Format("{0} phút {1} giây", nMinute, secondString);
            }
        }

        #endregion Date Time

        #region MoneyPlayer

        public static bool IsHaveMoney(KPlayer player, int MoneyNeed, MoneyType Type)
        {
            int MoneyHave = 0;

            if (Type == MoneyType.Bac)
            {
                MoneyHave = player.Money;
            }
            else if (Type == MoneyType.BacKhoa)
            {
                MoneyHave = player.BoundMoney;
            }
            else if (Type == MoneyType.Dong)
            {
                MoneyHave = player.Token;
            }
            else if (Type == MoneyType.DongKhoa)
            {
                MoneyHave = player.BoundToken;
            }
            else if (Type == MoneyType.GuildMoney)
            {
                MoneyHave = player.RoleGuildMoney;
            }


            if (MoneyHave < MoneyNeed)
            {
                return false;
            }
            else
            {
                return true;
            }

            // Các loại tiền khác để TODO sắp tới thêm sau
        }

        public static SubRep SubMoney(KPlayer player, int MoneySub, MoneyType Type, string From)
        {
            SubRep _SubRep = new SubRep();
            _SubRep.IsOK = false;
            _SubRep.CountLess = 0;
            if (MoneySub <= 0)
            {
                _SubRep.IsOK = false;
                return _SubRep;
            }

            switch (Type)
            {
                case MoneyType.Bac:
                    {
                        _SubRep.IsOK = GameManager.ClientMgr.SubMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneySub, From);
                        _SubRep.CountLess = player.Money;

                        return _SubRep;
                    }
                case MoneyType.BacKhoa:
                    {
                        _SubRep.IsOK = GameManager.ClientMgr.SubUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneySub, From);

                        _SubRep.CountLess = player.BoundMoney;

                        return _SubRep;
                    }
                case MoneyType.Dong:
                    {
                        _SubRep.IsOK = GameManager.ClientMgr.SubToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneySub, From);

                        _SubRep.CountLess = player.Token;

                        return _SubRep;
                    }
                case MoneyType.DongKhoa:
                    {
                        _SubRep.IsOK = GameManager.ClientMgr.SubBoundToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneySub, From);
                        _SubRep.CountLess = player.BoundToken;

                        return _SubRep;
                    }

                case MoneyType.GuildMoney:
                    {
                        _SubRep.IsOK = GameManager.ClientMgr.SubGuildMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneySub, From);
                        _SubRep.CountLess = player.RoleGuildMoney;

                        return _SubRep;
                    }
            }

            return _SubRep;
        }

        public static void AddRepute(KPlayer player, int PointType, long experience)
        {
            var FindPoint = player.GetRepute().Where(x => x.DBID == PointType).FirstOrDefault();

            if (FindPoint != null)
            {
                int LevelCur = FindPoint.Level;

                int _Class = FindPoint.Class;

                int _Camp = FindPoint.Camp;

                Camp _CampFind = ReputeManager._ReputeConfig.Camp.Where(x => x.Id == _Camp).FirstOrDefault();
                if (_CampFind != null)
                {
                    Class _ClassFind = _CampFind.Class.Where(x => x.Id == _Class).FirstOrDefault();
                    if (_ClassFind != null)
                    {
                        var FindLevel = _ClassFind.Level.Where(x => x.Id == LevelCur).FirstOrDefault();

                        if (FindLevel != null)
                        {
                            // get ra max level của danh vọng này
                            int MaxLevel = _ClassFind.Level.Max(x => x.Id);
                            int nNeedExp = FindLevel.LevelUp;

                            if (FindPoint.Level < MaxLevel && FindPoint.Exp + experience >= nNeedExp)
                            {
                                experience = (FindPoint.Exp + experience) - nNeedExp;

                                FindPoint.Exp = 0;
                                FindPoint.Level = FindPoint.Level + 1;

                                AddRepute(player, PointType, experience);
                            }
                            else
                            {
                                if (FindPoint.Level < MaxLevel)
                                {
                                    FindPoint.Exp += (int)experience;
                                }
                                else
                                {
                                    FindPoint.Exp = 0;
                                }
                            }

                            if (player.IsOnline())
                            {
                                ReputeInfo _Info = new ReputeInfo();
                                _Info.DBID = PointType;
                                _Info.Exp = FindPoint.Exp;
                                _Info.Level = FindPoint.Level;

                                player.SendPacket<ReputeInfo>((int)TCPGameServerCmds.CMD_KT_UPDATE_REPUTE, _Info);
                            }
                        }
                    }
                }
            }
        }

        public static void SetRepute(KPlayer player, int PointType, int Level, long experience)
        {
            var FindPoint = player.GetRepute().Where(x => x.DBID == PointType).FirstOrDefault();

            if (FindPoint != null)
            {
                FindPoint.Level = Level;
                FindPoint.Exp = (int)experience;


                if (player.IsOnline())
                {
                    ReputeInfo _Info = new ReputeInfo();
                    _Info.DBID = PointType;
                    _Info.Exp = FindPoint.Exp;
                    _Info.Level = FindPoint.Level;

                    player.SendPacket<ReputeInfo>((int)TCPGameServerCmds.CMD_KT_UPDATE_REPUTE, _Info);
                }
            }
        }

        ///// <summary>
        ///// Hàm add danh vọng cho người chơi
        ///// </summary>
        ///// <param name="PointType"></param>
        ///// <param name="PointAdd"></param>
        //public static void AddRepute(KPlayer player, int PointType, int PointAdd)
        //{
        //    var FindPoint = player.GetRepute().Where(x => x.DBID == PointType).FirstOrDefault();

        //    if (FindPoint != null)
        //    {
        //        int LevelCur = FindPoint.Level;

        //        int _Class = FindPoint.Class;

        //        int _Camp = FindPoint.Camp;

        //        Camp _CampFind = ReputeManager._ReputeConfig.Camp.Where(x => x.Id == _Camp).FirstOrDefault();
        //        if (_CampFind != null)
        //        {
        //            Class _ClassFind = _CampFind.Class.Where(x => x.Id == _Class).FirstOrDefault();
        //            if (_ClassFind != null)
        //            {
        //                var FindLevel = _ClassFind.Level.Where(x => x.Id == LevelCur).FirstOrDefault();

        //                if (FindLevel != null)
        //                {
        //                    // get ra max level của danh vọng này
        //                    int MaxLevel = _ClassFind.Level.Max(x => x.Id);

        //                    // Nếu level nhỏ hơn max thì được thăng cấp
        //                    if (FindPoint.Level < MaxLevel)
        //                    {
        //                        int ExpLess = PointAdd;

        //                        //// Nếu exp mà vẫn còn
        //                        //while (ExpLess > 0)
        //                        //{
        //                        //    // exp cần để lên cấp
        //                        //    int ExpNeed = FindLevel.LevelUp;

        //                        //    // Nếu lượng exp add thêm + với lượng exp hiện tại mà quá lượng exp cần
        //                        //    if (ExpLess + FindPoint.Exp >= ExpNeed)
        //                        //    {
        //                        //        // Tính toán lại lượng EXP còn
        //                        //        ExpLess = (ExpLess + FindPoint.Exp) - ExpNeed;

        //                        //        FindPoint.Level = FindPoint.Level + 1;
        //                        //        FindPoint.Exp = 0;

        //                        //        if (FindPoint.Level == MaxLevel)
        //                        //        {
        //                        //            FindPoint.Exp = 0;
        //                        //        }
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        FindPoint.Exp = ExpLess;
        //                        //        ExpLess = 0;
        //                        //    }
        //                        //}
        //                        if (player.IsOnline())
        //                        {
        //                            ReputeInfo _Info = new ReputeInfo();
        //                            _Info.DBID = PointType;
        //                            _Info.Exp = FindPoint.Exp;
        //                            _Info.Level = FindPoint.Level;

        //                            player.SendPacket<ReputeInfo>((int)TCPGameServerCmds.CMD_KT_UPDATE_REPUTE, _Info);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        PlayerManager.ShowNotification(player, "Danh vọng đã đạt cấp tối đa");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Hàm cộng tiền cho người chơi
        /// </summary>
        /// <param name="player"></param>
        /// <param name="MoneyAdd"></param>
        /// <param name="Type"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static SubRep AddMoney(KPlayer player, int MoneyAdd, MoneyType Type, string Source = "")
        {
            SubRep _SubRep = new SubRep();
            _SubRep.IsOK = false;
            _SubRep.CountLess = 0;

            switch (Type)
            {
                case MoneyType.Bac:
                    {
                        if (player.Money + MoneyAdd > 1000000000)
                        {
                            PlayerManager.ShowMessageBox(player, "Lỗi", "Bạc mang theo không thể quá 1 tỉ");
                        }
                        else
                        {
                            _SubRep.IsOK = GameManager.ClientMgr.AddMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneyAdd, Source);
                            _SubRep.CountLess = player.Money;
                        }

                        return _SubRep;
                    }
                case MoneyType.BacKhoa:
                    {
                        if (player.BoundMoney + MoneyAdd > 1000000000)
                        {
                            PlayerManager.ShowMessageBox(player, "Lỗi", "Bạc khóa mang theo không thể quá 1 tỉ");
                        }
                        else
                        {
                            _SubRep.IsOK = GameManager.ClientMgr.AddUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneyAdd, Source);

                            _SubRep.CountLess = player.BoundMoney;
                        }

                        return _SubRep;
                    }
                case MoneyType.Dong:
                    {
                        if (player.Token + MoneyAdd > 1000000000)
                        {
                            PlayerManager.ShowMessageBox(player, "Lỗi", "Đồng mang theo không thể quá 1 tỉ");
                        }
                        else
                        {
                            _SubRep.IsOK = GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneyAdd, Source);

                            _SubRep.CountLess = player.Token;
                        }

                        return _SubRep;
                    }
                case MoneyType.DongKhoa:
                    {
                        if (player.BoundToken + MoneyAdd > 1000000000)
                        {
                            PlayerManager.ShowMessageBox(player, "Lỗi", "Đồng khóa mang theo không thể quá 1 tỉ");
                        }
                        else
                        {
                            _SubRep.IsOK = GameManager.ClientMgr.AddBoundToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneyAdd, Source);
                            _SubRep.CountLess = player.BoundToken;
                        }

                        return _SubRep;
                    }

                case MoneyType.GuildMoney:
                    {
                        if (player.RoleGuildMoney + MoneyAdd > 1000000000)
                        {
                            PlayerManager.ShowMessageBox(player, "Lỗi", "Tiền bang hội mang theo không thể quá 1 tỉ");
                        }
                        else
                        {
                            _SubRep.IsOK = GameManager.ClientMgr.AddGuildMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, MoneyAdd, Source);
                            _SubRep.CountLess = player.RoleGuildMoney;
                        }

                        return _SubRep;
                    }
            }

            return _SubRep;
        }

        #endregion MoneyPlayer

        #region System Global Parameters

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameter(int id, int value)
        {
            GameDb.SetSystemGlobalParameters(id, value.ToString());
        }

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameter(int id, long value)
        {
            GameDb.SetSystemGlobalParameters(id, value.ToString());
        }

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameter(int id, float value)
        {
            GameDb.SetSystemGlobalParameters(id, value.ToString());
        }

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameter(int id, double value)
        {
            GameDb.SetSystemGlobalParameters(id, value.ToString());
        }

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameter(int id, string value)
        {
            GameDb.SetSystemGlobalParameters(id, value);
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetSystemGlobalParameterInt32(int id)
        {
            string value = GameDb.GetSystemGlobalParameter(id);
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static long GetSystemGlobalParameterInt64(int id)
        {
            string value = GameDb.GetSystemGlobalParameter(id);
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static float GetSystemGlobalParameterFloat(int id)
        {
            string value = GameDb.GetSystemGlobalParameter(id);
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return (float)Convert.ToDouble(value);
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double GetSystemGlobalParameterDouble(int id)
        {
            string value = GameDb.GetSystemGlobalParameter(id);
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Check ký tự đặc biệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSystemGlobalParameterString(int id)
        {
            string value = GameDb.GetSystemGlobalParameter(id);
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            return value;
        }

        #endregion System Global Parameters

        #region Khoảng cách

        /// <summary>
        /// Trả về khoảng cách giữa 2 điểm
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float GetDistanceBetweenPoints(Point p1, Point p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        /// <summary>
        /// Trả về khoảng cách giữa 2 người chơi
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns></returns>
        public static float GetDistanceBetweenPlayers(KPlayer player1, KPlayer player2)
        {
            return KTGlobal.GetDistanceBetweenPoints(player1.CurrentPos, player2.CurrentPos);
        }

        #endregion Khoảng cách

        // Quản lý toàn bộ ghi chép của người chơi theo thời gian

        #region RecorePlayer

        /// <summary>
        /// Lấy ra giá trị đã đánh dấu
        /// Cái này sử dụng để đánh dấu tháng này người chơi đã nhận cái gì chưa
        /// Hoặc sử dụng trong rất nhiều trường hợp khác cần đánh dấu
        /// Ví dụ : Cần ghi tháng 2 Người chơi đã nhận quà tháng tiêu dao cốc
        /// Thì tháng 2 là KEY | MarkType : Là Tiêu Dao Cốc
        /// </summary>
        /// <param name="client"></param>
        /// <param name="MarkType"></param>
        /// <param name="MarkType"></param>
        /// <returns></returns>
        public static int GetMarkValue(KPlayer client, string MarkKey, int MarkType)
        {
            string CMDBUILD = client.RoleID + "#" + MarkKey + "#" + MarkType;

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_GETMARKVALUE, CMDBUILD, GameManager.LocalServerId);

            if (null == dbFields || dbFields.Length != 2)
            {
                LogManager.WriteLog(LogTypes.Error, "[GetMarkValue][" + client.RoleID + "][" + MarkKey + "] Lấy giá trị đánh dấu bị lỗi");
            }
            else
            {
                return Int32.Parse(dbFields[1]);
            }

            return -1;
        }

        /// <summary>
        /// Hàm này để ghi vào 1 giá trị vào DB theo KEY và TYPE
        /// Ví dụ muốn đánh tháng 2 người chơi ABC đã nhận Thưởng TIÊU DAO CỐC
        /// MarkKEy là tháng 2,MarkType là Tiêu dao cốc,MarkValue truyền vào 1 để thể hiện là đã nhận 1 lần
        /// NẾu muốn cho nhận nhiều lần thì MARKValue có thể đảm nhận việc này
        /// Nếu MarkKey Và MarkType đã tồn tại trong DB thì MarkValue mới sẽ được thay thế MarkValue cũ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="MarkKey"></param>
        /// <param name="MarkType"></param>
        /// <param name="MarkValue"></param>
        public static bool UpdateMarkValue(KPlayer client, string MarkKey, int MarkType, int MarkValue)
        {
            string CMDBUILD = client.RoleID + "#" + MarkKey + "#" + MarkValue + "#" + MarkType;

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_UPDATEMARKVALUE, CMDBUILD, GameManager.LocalServerId);

            if (null == dbFields || dbFields.Length != 2)
            {
                LogManager.WriteLog(LogTypes.Error, "[UpdateMarkValue][" + client.RoleID + "][" + MarkKey + "] Lấy giá trị đánh dấu bị lỗi");
            }
            else
            {
                if (Int32.Parse(dbFields[1]) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }


        /// <summary>
        /// Hàm này để ghi lại tích lũy bất kỳ của nhân vật trong thời điểm thực hiện lệnh này
        /// Hàm này có thể sử dụng để ghi lại nhật ký của người chơi khi đi tiêu dao cốc
        /// MarkTYpe là key của TIÊU DAO CỐC
        /// MarkVALUE là giá trị điểm đạt được của nhân vật
        /// Khi đọc ra chỉ cần chuyền vào TIMER RANGER từ A tới B hệ thống sẽ SUM toàn bộ VALUE này và trả về cho hệ thống
        /// </summary>
        /// <param name="client"></param>
        /// <param name="MarkType"></param>
        /// <param name="MarkValue"></param>
        /// <returns></returns>
        public static bool AddRecoreByType(KPlayer client, int MarkType, int MarkValue)
        {
            string CMDBUILD = client.RoleID + "|" + client.RoleName + "|" + MarkType + "|" + MarkValue;
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_ADD_RECORE_BYTYPE, CMDBUILD, GameManager.LocalServerId);
            if (null == dbFields || dbFields.Length != 2)
            {
                LogManager.WriteLog(LogTypes.Error, "[AddRecoreByType][" + client.RoleID + "][" + MarkType + "] Lấy giá trị đánh dấu bị lỗi :" + MarkValue);
            }
            else
            {
                if (Int32.Parse(dbFields[1]) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Hàm lấy ra tổng tích lũy theo 1 khoảng thời gian
        /// Ví dụ tháng này người chơi A được bao nhiêu điểm thi đấu
        /// </summary>
        /// <param name="client"></param>
        /// <param name="MarkKey"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public static int GetRecoreByType(KPlayer client, int MarkKey, DateTime Start, DateTime End)
        {
            string StartTime = Start.ToString("yyyy-MM-dd HH:mm:ss");
            string EndTime = End.ToString("yyyy-MM-dd HH:mm:ss");
            string CMDBUILD = client.RoleID + "|" + MarkKey + "|" + StartTime + "|" + EndTime;

            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_GET_RECORE_BYTYPE, CMDBUILD, GameManager.LocalServerId);
            if (null == dbFields || dbFields.Length != 2)
            {
                LogManager.WriteLog(LogTypes.Error, "[GetMarkValue][" + client.RoleID + "][" + MarkKey + "] Lấy giá trị đánh dấu bị lỗi");
            }
            else
            {
                return Int32.Parse(dbFields[1]);
            }

            return -1;
        }

        /// <summary>
        /// Lấy ra top X người chơi theo Markey đã đánh dấu trong 1 khoảng thời gian
        /// Ví dụ lấy ra top 100 thằng có điểm tích lũy cao nhất trong TIÊU DAO CỐC
        /// MarkKey : Tiêu dao cốc
        /// Start : Thời gian bắt đầu của tháng trước đó
        /// End : Thời gian kết thúc của tháng trươc đó
        /// LitmitCount : Lấy ra bao nhiêu thằng => 100 nếu lấy 100 thằng
        /// ZONEID : ZONEID hiện tại
        /// </summary>
        /// <param name="MarkKey"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="LitmitCount"></param>
        /// <param name="ZoneID"></param>
        /// <returns></returns>
        public static List<RecoreRanking> GetRankByMarkAndTimeRanger(int MarkKey, DateTime Start, DateTime End, int LitmitCount, int ZoneID)
        {
            string StartTime = Start.ToString("yyyy-MM-dd HH:mm:ss");
            string EndTime = End.ToString("yyyy-MM-dd HH:mm:ss");

            string CMDBUILD = MarkKey + "#" + LitmitCount + "#" + StartTime + "#" + EndTime;

            byte[] bytesData = null;
            byte[] ByteSendToDB = Encoding.ASCII.GetBytes(CMDBUILD);

            TCPProcessCmdResults result = Global.ReadDataFromDb((int)TCPGameServerCmds.CMD_KT_GETRANK_RECORE_BYTYPE, ByteSendToDB, ByteSendToDB.Length, out bytesData, ZoneID);

            if (TCPProcessCmdResults.RESULT_FAILED != result)
            {
                //Get đồ từ DB ra trả về client
                List<RecoreRanking> goodsDataList = DataHelper.BytesToObject<List<RecoreRanking>>(bytesData, 6, bytesData.Length - 6);
                return goodsDataList;


            }
            return null;
        }



        #endregion RecorePlayer
    }
}