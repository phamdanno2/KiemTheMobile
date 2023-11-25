using FSPlay.GameEngine.Network;
using FSPlay.KiemVu.Entities.Config;
using HSGameEngine.GameEngine.Network.Protocol;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPlay.KiemVu.Network
{
    /// <summary>
    /// Quản lý tương tác với Socket
    /// </summary>
    public static partial class KT_TCPHandler
    {
		#region Tống Kim
		/// <summary>
		/// Gửi yêu cầu bảng điểm chiến trường Tống Kim
		/// </summary>
		public static void SendOpenSongJinRankingBoard()
        {
            string strCmd = string.Format("");
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SONGJINBATTLE_RANKING)));
        }

        /// <summary>
        /// Nhận gói tin thông báo danh sách thứ hạng Tống Kim
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveSongJinRankingInfo(int cmdID, byte[] bytes, int length)
        {
            try
            {
                SongJinBattleRankingInfo rankingInfo = DataHelper.BytesToObject<SongJinBattleRankingInfo>(bytes, 0, length);

                /// Hiển thị khung
                PlayZone.GlobalPlayZone.ShowUISongJinRankingBoard();
                PlayZone.GlobalPlayZone.UISongJinRankingBoard.Data = rankingInfo;
            }
            catch (Exception) { }
        }
        #endregion

        #region Thi đấu môn phái
        /// <summary>
        /// Gửi yêu cầu thông tin dữ liệu thi đấu môn phái
        /// </summary>
        public static void SendGetFactionBattleData()
        {
            string strCmd = string.Format("");
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_FACTION_PVP_RANKING_INFO)));
        }

        /// <summary>
        /// Nhận gói tin thông tin dữ liệu thi đấu môn phái
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveFactionBattleData(int cmdID, byte[] bytes, int length)
        {
            FACTION_PVP_RANKING_INFO rankingInfo = DataHelper.BytesToObject<FACTION_PVP_RANKING_INFO>(bytes, 0, length);
            if (rankingInfo == null)
            {
                KTGlobal.AddNotification("Bảng xếp hạng chưa cập nhật!");
                return;
            }

            /// Hiển thị khung bảng xếp hạng thi đấu
            if (rankingInfo.State == 0)
            {
                PlayZone.GlobalPlayZone.OpenUIFactionBattleRanking(rankingInfo.PlayerRanks);
            }
            /// Hiển thị thi đấu môn phái
            else if (rankingInfo.State == 1)
            {
                PlayZone.GlobalPlayZone.OpenUIFactionBattleRoundInfo(rankingInfo);
            }
        }
        #endregion

        #region Bách Bảo Rương
        /// <summary>
        /// Gửi yêu cầu mở khung Bách Bảo Rương
        /// </summary>
        public static void SendOpenSeashellCircle()
        {
            string strCmd = string.Format("{0}:{1}", 0, -1);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SEASHELL_CIRCLE)));
        }

        /// <summary>
        /// Gửi yêu cầu thực hiện quay vòng quay Bách Bảo Rương
        /// </summary>
        /// <param name="nBet"></param>
        public static void SendStartSeashellCircleTurn(int nBet)
        {
            string strCmd = string.Format("{0}:{1}", 1, nBet);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SEASHELL_CIRCLE)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận quà Bách Bảo Rương
        /// </summary>
        /// <param name="nBet"></param>
        public static void SendGetSeashellCircleAward()
        {
            string strCmd = string.Format("{0}:{1}", 2, -1);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SEASHELL_CIRCLE)));
        }

        /// <summary>
        /// Gửi yêu cầu đổi sò Bách Bảo Rương
        /// </summary>
        /// <param name="nBet"></param>
        public static void SendExchangeSeashellCircleSeashell()
        {
            string strCmd = string.Format("{0}:{1}", 3, -1);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SEASHELL_CIRCLE)));
        }

        /// <summary>
        /// Nhận gói tin phản hồi thao tác Bách Bảo Rương
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveSeashellCircleResponse(string[] fields)
        {
            try
            {
                /// Loại thao tác
                int type = int.Parse(fields[0]);
                /// Kết quả
                int ret = int.Parse(fields[1]);

                /// Xem loại thao tác là gì để xử lý
                switch (type)
                {
                    /// Mở khung quay sò
                    case 0:
                    {
                        /// Tổng số sò tích trữ
                        long totalStorage = long.Parse(fields[2]);
                        int lastStage = int.Parse(fields[3]);
                        int lastStopPos = int.Parse(fields[4]);
                        int lastBet = int.Parse(fields[5]);

                        bool enableStartTurn = int.Parse(fields[6]) == 1;
                        bool enableGet = int.Parse(fields[7]) == 1;
                        bool enableExchange = int.Parse(fields[8]) == 1;
                        bool enableBet = int.Parse(fields[9]) == 1;
                        /// Mở khung
                        PlayZone.GlobalPlayZone.OpenSeashellCircle(totalStorage, lastStage, lastStopPos, lastBet);
                        /// Cập nhật trạng thái cho các Button
                        PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                        break;
                    }
                    /// Bắt đầu quay
                    case 1:
                    {
                        /// Nếu quay thành công
                        if (ret == 1)
                        {
                            int lastStopPos = int.Parse(fields[2]);
                            int durationTick = int.Parse(fields[3]);
                            int totalCells = int.Parse(fields[4]);
                            int currentStage = int.Parse(fields[5]);

                            bool enableStartTurn = int.Parse(fields[6]) == 1;
                            bool enableGet = int.Parse(fields[7]) == 1;
                            bool enableExchange = int.Parse(fields[8]) == 1;
                            bool enableBet = int.Parse(fields[9]) == 1;

                            /// Tổng số sò tích trữ
                            long totalStorage = long.Parse(fields[10]);

                            /// Có phải Turn đầu tiên không
                            bool isFirstTurn = int.Parse(fields[11]) == 1;

                            /// Nếu đang hiện khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Cập nhật tổng số sò hệ thống tích lũy
                                PlayZone.GlobalPlayZone.UISeashellCircle.ServerTotalSeashells = totalStorage;
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Thực hiện quay
                                PlayZone.GlobalPlayZone.UISeashellCircle.StartPlayTurn(lastStopPos, totalCells, durationTick / 1000f, () => {
                                    /// Cập nhật trạng thái cho các Button
                                    PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                                    /// Cập nhật tầng hiện tại
                                    PlayZone.GlobalPlayZone.UISeashellCircle.CurrentStage = currentStage;
                                });

                                /// Nếu là Turn đầu tiên
                                if (isFirstTurn)
                                {
                                    /// Text lịch sử
                                    string historyText = string.Format("<color=#2aceef>Cược <color=green>{0}</color> <color=yellow>[{1}]</color>.</color>", PlayZone.GlobalPlayZone.UISeashellCircle.CurrentBet, KTGlobal.GetItemName(KTGlobal.SeashellItemID));
                                    /// Cập nhật lịch sử
                                    PlayZone.GlobalPlayZone.UISeashellCircle.AddHistory(historyText);
                                }
                            }
                        }
                        /// Nếu quay thất bại
                        else
                        {
                            int currentStage = int.Parse(fields[2]);
                            bool enableStartTurn = int.Parse(fields[3]) == 1;
                            bool enableGet = int.Parse(fields[4]) == 1;
                            bool enableExchange = int.Parse(fields[5]) == 1;
                            bool enableBet = int.Parse(fields[6]) == 1;

                            /// Nếu đang mở khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Cập nhật trạng thái cho các Button
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                                /// Cập nhật tầng hiện tại
                                PlayZone.GlobalPlayZone.UISeashellCircle.CurrentStage = currentStage;
                            }
                        }
                        break;
                    }
                    /// Nhận quà
                    case 2:
                    {
                        /// Nếu nhận thành công
                        if (ret == 1)
                        {
                            bool enableStartTurn = int.Parse(fields[2]) == 1;
                            bool enableGet = int.Parse(fields[3]) == 1;
                            bool enableExchange = int.Parse(fields[4]) == 1;
                            bool enableBet = int.Parse(fields[5]) == 1;
                            int awardType = int.Parse(fields[6]);
                            int itemID = int.Parse(fields[7]);
                            int itemNumber = int.Parse(fields[8]);

                            /// Nếu đang mở khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Làm rỗng dữ liệu tầng
                                PlayZone.GlobalPlayZone.UISeashellCircle.ClearStageDesc();
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Cập nhật trạng thái cho các Button
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                                /// Text lịch sử
                                string historyText = "";
                                /// Nếu là nhận vật phẩm
                                if (awardType == 0)
                                {
                                    historyText = string.Format("<color=#ff52ab>Nhận <color=green>{0} cái</color> <color=yellow>[{1}]</color>.</color>", itemNumber, KTGlobal.GetItemName(itemID));
                                }
                                /// Nếu là nhận tinh hoạt lực
                                else if (awardType == 1)
                                {
                                    historyText = string.Format("<color=#ff52ab>Nhận <color=green>{0}</color> <color=yellow>Tinh hoạt lực</color>.</color>", itemNumber);
                                }
                                /// Nếu là nhận bạc
                                else if (awardType == 2)
                                {
                                    historyText = string.Format("<color=#ff52ab>Nhận <color=green>{0}</color> <color=yellow>Bạc</color>.</color>", KTGlobal.GetDisplayMoney(itemNumber));
                                }
                                /// Nếu là nhận đồng khóa
                                else if (awardType == 3)
                                {
                                    historyText = string.Format("<color=#ff52ab>Nhận <color=green>{0}</color> <color=yellow>Đồng khóa</color>.</color>", KTGlobal.GetDisplayMoney(itemNumber));
                                }
                                /// Cập nhật lịch sử
                                PlayZone.GlobalPlayZone.UISeashellCircle.AddHistory(historyText);
                            }
                        }
                        /// Nếu nhận thất bại
                        else
                        {
                            bool enableStartTurn = int.Parse(fields[2]) == 1;
                            bool enableGet = int.Parse(fields[3]) == 1;
                            bool enableExchange = int.Parse(fields[4]) == 1;
                            bool enableBet = int.Parse(fields[5]) == 1;

                            /// Nếu đang mở khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Cập nhật trạng thái cho các Button
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                            }
                        }
                        break;
                    }
                    /// Đổi sò
                    case 3:
                    {
                        /// Nếu đổi thành công
                        if (ret == 1)
                        {
                            bool enableStartTurn = int.Parse(fields[2]) == 1;
                            bool enableGet = int.Parse(fields[3]) == 1;
                            bool enableExchange = int.Parse(fields[4]) == 1;
                            bool enableBet = int.Parse(fields[5]) == 1;
                            int totalSeashells = int.Parse(fields[6]);

                            /// Nếu đang mở khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Làm rỗng dữ liệu tầng
                                PlayZone.GlobalPlayZone.UISeashellCircle.ClearStageDesc();
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Cập nhật trạng thái cho các Button
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                                /// Text lịch sử
                                string historyText = string.Format("<color=#ff8438>Đổi được <color=green>{0}</color> <color=yellow>[{1}]</color>.</color>", totalSeashells, KTGlobal.GetItemName(KTGlobal.SeashellItemID));
                                /// Cập nhật lịch sử
                                PlayZone.GlobalPlayZone.UISeashellCircle.AddHistory(historyText);
                            }
                        }
                        /// Nếu đổi thất bại
                        else
                        {
                            bool enableStartTurn = int.Parse(fields[2]) == 1;
                            bool enableGet = int.Parse(fields[3]) == 1;
                            bool enableExchange = int.Parse(fields[4]) == 1;
                            bool enableBet = int.Parse(fields[5]) == 1;

                            /// Nếu đang mở khung
                            if (PlayZone.GlobalPlayZone.UISeashellCircle != null)
                            {
                                /// Cập nhật tổng số sò có
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateTotalSeashellsOwning();
                                /// Cập nhật trạng thái cho các Button
                                PlayZone.GlobalPlayZone.UISeashellCircle.UpdateButtonsState(enableBet, enableStartTurn, enableGet, enableExchange);
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region Vòng quay may mắn
        /// <summary>
        /// Gửi yêu cầu thực hiện quay vòng quay Vòng quay may mắn
        /// </summary>
        public static void SendStartLuckyCircleTurn(int method)
        {
            string strCmd = string.Format("{0}:{1}", 1, method);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_LUCKYCIRCLE)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận quà Vòng quay may mắn
        /// </summary>
        /// <param name="nBet"></param>
        public static void SendGetLuckyCircleAward()
        {
            string strCmd = string.Format("{0}", 2);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_LUCKYCIRCLE)));
        }

        /// <summary>
        /// Nhận gói tin phản hồi thao tác Vòng quay may mắn
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytesData"></param>
        /// <param name="length"></param>
        public static void ReceiveLuckyCircleResponse(int cmdID, byte[] bytesData, int length)
        {
            /// Dữ liệu
            G2C_LuckyCircle data = DataHelper.BytesToObject<G2C_LuckyCircle>(bytesData, 0, length);
            /// Toác
            if (data == null)
            {
                KTGlobal.AddNotification("Dữ liệu vòng quay bị lỗi, hãy thử lại!");
                return;
            }

            /// Loại thao tác
            int requestType = data.Fields[0];

            /// Nếu là mở vòng quay
            if (requestType == 0)
            {
                PlayZone.GlobalPlayZone.OpenUILuckyCircle(data);
            }
            /// Nếu là phản hồi thao tác quay
            else if (requestType == 1)
            {
                /// Vị trí dừng
                int stopPos = data.LastStopPos;
                /// Thời gian quay
                int durationSec = data.Fields[1];
                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UILuckyCircle != null)
                {
                    /// Thực hiện quay
                    PlayZone.GlobalPlayZone.UILuckyCircle.ServerStartTurn(stopPos, durationSec);
                }
            }
            /// Nếu là phản hồi thao tác nhận thưởng
            else if (requestType == 2)
            {
                /// Vị trí dừng
                int stopPos = data.LastStopPos;
                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UILuckyCircle != null)
                {
                    /// Thực hiện nhận thưởng
                    PlayZone.GlobalPlayZone.UILuckyCircle.ServerGetAward(stopPos);
                }
            }
            /// Nếu là phản hồi ẩn hiện Button chức năng
            else if (requestType == 3)
            {
                /// Kích hoạt Button quay
                bool enableStartTurn = data.Fields[1] == 1;
                /// Kích hoạt Button nhận thưởng
                bool enableGetAward = data.Fields[2] == 1;
                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UILuckyCircle != null)
                {
                    /// Thực hiện ẩn hiện Button chức nămg
                    PlayZone.GlobalPlayZone.UILuckyCircle.ServerResponseButtonState(enableStartTurn, enableGetAward);
                }
            }
        }
        #endregion

        #region Du Long Các
        /// <summary>
        /// Xử lý gói tin Du Long Các
        /// </summary>
        /// <param name="fields"></param>
        public static void ProcessYouLongPacket(string[] fields)
        {
            /// Loại thao tác
            int type = int.Parse(fields[0]);

            /// Nếu là thao tác mở khung
            if (type == 0)
            {
                /// Danh sách vật phẩm
                List<GoodsData> items = new List<GoodsData>();
                /// Chuỗi mã hóa danh sách vật phẩm
                string[] itemsPairs = fields[1].Split('|');
                /// Duyệt danh sách vật phẩm
                foreach (string itemPair in itemsPairs)
                {
                    string[] param = itemPair.Split('_');
                    int itemID = int.Parse(param[0]);
                    int quantity = int.Parse(param[1]);
                    /// Tạo vật phẩm tương ứng
                    if (Loader.Loader.Items.TryGetValue(itemID, out ItemData itemData))
                    {
                        /// Tạo vật phẩm tương ứng
                        GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
                        itemGD.GCount = quantity;
                        /// Thêm vào danh sách
                        items.Add(itemGD);
                    }

                }
                /// Mở khung
                PlayZone.GlobalPlayZone.OpenUIYouLong();
                /// Thiết lập vật phẩm
                PlayZone.GlobalPlayZone.UIYouLong.Items = items;
                PlayZone.GlobalPlayZone.UIYouLong.Refresh(0, false);
            }
            /// Nếu là thao tác cập nhật phần thưởng
            else if (type == 1)
            {
                /// Thông tin phần thưởng
                string[] param = fields[1].Split('_');
                int itemID = int.Parse(param[0]);
                int quantity = int.Parse(param[1]);
                /// Thông tin vật phẩm tương ứng
                GoodsData itemGD = null;
                /// Tạo vật phẩm tương ứng
                if (Loader.Loader.Items.TryGetValue(itemID, out ItemData itemData))
                {
                    /// Tạo vật phẩm tương ứng
                    itemGD = KTGlobal.CreateItemPreview(itemData);
                    itemGD.GCount = quantity;
                }

                /// Số tiền đổi được
                int exchangeCoinAmount = int.Parse(fields[2]);
                /// Xóa khỏi danh sách
                bool removeFromList = int.Parse(fields[3]) == 1;

                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UIYouLong != null)
                {
                    PlayZone.GlobalPlayZone.UIYouLong.CurrentAward = itemGD;
                    PlayZone.GlobalPlayZone.UIYouLong.ExchangeCoinAmount = exchangeCoinAmount;
                    PlayZone.GlobalPlayZone.UIYouLong.Refresh(1, removeFromList);
                }
            }
            /// Đổi thưởng hoàn tất
            else if (type == 2)
            {
                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UIYouLong != null)
                {
                    PlayZone.GlobalPlayZone.UIYouLong.Refresh(2, false);
                }
            }
            /// Nếu là thao tác đóng khung
            else
            {
                /// Đóng khung
                PlayZone.GlobalPlayZone.CloseUIYouLong();

            }
        }

        /// <summary>
        /// Gửi yêu cầu lấy vật phẩm bất kỳ trong danh sách Du Long
        /// </summary>
        public static void SendYouLongGetRandomAwardFromList()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("0");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận thưởng Du Long
        /// </summary>
        public static void SendYouLongGetAward()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("1");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }

        /// <summary>
        /// Gửi yêu cầu đổi tiền Du Long
        /// </summary>
        public static void SendYouLongExchangeCoin()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("2");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }

        /// <summary>
        /// Gửi yêu cầu thử lại Du Long
        /// </summary>
        public static void SendYouLongTryAgain()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("3");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }

        /// <summary>
        /// Gửi yêu cầu vòng kế tiếp Du Long
        /// </summary>
        public static void SendYouLongNextRound()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("4");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }

        /// <summary>
        /// Gửi yêu cầu thoát phụ bản Du Long
        /// </summary>
        public static void SendYouLongExit()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("-1");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_YOULONG)));
        }
		#endregion

		#region Tranh đoạt lãnh thổ
        /// <summary>
        /// Gửi yêu cầu lấy dữ liệu chiến công tranh đoạt lãnh thổ
        /// </summary>
        public static void SendGetColonyDisputeInfo()
		{
            string strCmd = string.Format("");
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GUILDWAR_RANKING)));
        }

        /// <summary>
        /// Nhận gói tin dữ liệu chiến công tranh đoạt lãnh thổ
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytesData"></param>
        /// <param name="length"></param>
        public static void ReceiveColonyDisputeInfo(int cmdID, byte[] bytesData, int length)
		{
            GuildWarReport data = DataHelper.BytesToObject<GuildWarReport>(bytesData, 0, length);
            if (data == null)
			{
                KTGlobal.AddNotification("Chưa có thông tin chiến công!");
                return;
			}

            /// Mở khung
            PlayZone.GlobalPlayZone.OpenUIColonyDisputeAchievement(data);
		}
        #endregion

        #region Võ lâm liên đấu
        /// <summary>
        /// Nhận gói tin dữ liệu xếp hạng chiến đội Võ lâm liên đấu
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytesData"></param>
        /// <param name="length"></param>
        public static void ReceiveTeamBattleRanking(int cmdID, byte[] bytesData, int length)
        {
            List<TeamBattleInfo> data = DataHelper.BytesToObject<List<TeamBattleInfo>>(bytesData, 0, length);
            if (data == null || data.Count <= 0)
            {
                KTGlobal.AddNotification("Bảng xếp hạng chưa cập nhật!");
                return;
            }

            /// Mở khung
            PlayZone.GlobalPlayZone.OpenUITeamBattleRankingBoard(data);
        }
        #endregion

        #region Chúc phúc
        /// <summary>
        /// Gửi yêu cầu mở khung chúc phúc
        /// </summary>
        public static void SendOpenPlayerPray()
		{
            string strCmd = string.Format("{0}", 0);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_G2C_PLAYERPRAY)));
        }

        /// <summary>
        /// Gửi yêu cầu bắt đầu quay chúc phúc
        /// </summary>
        public static void SendStartTurnPlayerPray()
		{
            string strCmd = string.Format("{0}", 1);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_G2C_PLAYERPRAY)));
        }

        /// <summary>
        /// Gửi yêu cầu nhận quà thưởng quay chúc phúc
        /// </summary>
        public static void SendGetPlayerPrayAward()
		{
            string strCmd = string.Format("{0}", 2);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_G2C_PLAYERPRAY)));
        }

        /// <summary>
        /// Nhận phản hồi liên quan đến tính năng chúc phúc
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceivePlayerPrayData(string[] fields)
		{
            /// Loại phản hồi
            int type = int.Parse(fields[0]);

            /// Nếu là phản hồi mở khung
            if (type == 0)
			{
                string lastTurnResult = fields[1];
                bool enableGetAward = int.Parse(fields[2]) == 1;
                bool enableStartNewTurn = int.Parse(fields[3]) == 1;
                int totalTurnLeft = int.Parse(fields[4]);
                /// Mở khung
                PlayZone.GlobalPlayZone.OpenUIPlayerPray();
                PlayZone.GlobalPlayZone.UIPlayerPray.RefreshData(lastTurnResult, enableGetAward, enableStartNewTurn, totalTurnLeft, true);
			}
            /// Nếu là phản hồi thao tác quay
            else if (type == 1)
			{
                int turnRound = int.Parse(fields[1]);
                int stopPos = int.Parse(fields[2]);

                string lastTurnResult = fields[3];
                bool enableGetAward = int.Parse(fields[4]) == 1;
                bool enableStartNewTurn = int.Parse(fields[5]) == 1;
                int totalTurnLeft = int.Parse(fields[6]);

                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UIPlayerPray != null)
				{
                    PlayZone.GlobalPlayZone.UIPlayerPray.Roll(turnRound, stopPos, () => {
                        PlayZone.GlobalPlayZone.UIPlayerPray.RefreshData(lastTurnResult, enableGetAward, enableStartNewTurn, totalTurnLeft, false);
                    });
				}
            }
			/// Nếu là phản hồi thao tác nhận thưởng
			else if (type == 2)
			{
                int totalTurnLeft = int.Parse(fields[1]);

                /// Nếu đang mở khung
                if (PlayZone.GlobalPlayZone.UIPlayerPray != null)
				{
                    PlayZone.GlobalPlayZone.UIPlayerPray.ClearData(totalTurnLeft);
                }
            }
		}
		#endregion
	}
}
