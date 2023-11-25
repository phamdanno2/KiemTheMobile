using GameServer.Core.Executor;
using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Class quản lý việc giao dịch
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Giao Dịch
        /// <summary>
        /// Thực hiện yêu cầu giao dịch gì đó
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGoodsExchangeCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Trade, string.Format("Có lỗi khi gửi yêu cầu gio dịch , CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                //Kiểm tra chuỗi dữ liệu gửi lên
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    // Nếu khác 4 thì chim cút
                    LogManager.WriteLog(LogTypes.Trade, string.Format("Chuỗi giao dịch gửi lên không hợp lệ, CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);

                // Tìm ra thằng muốn giao dịch
                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                // Nếu mà thằng muốn giao dịch gửi lên không đúng
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Trade, string.Format("Dữ liệu sai sót khi giao dịch, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.PKMode == (int)PKMode.Custom)
                {
                    PlayerManager.ShowNotification(client, "Chế độ chiến đấu không thể giao dịch");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Bản đồ tương ứng
                GameMap map = GameManager.MapMgr.GetGameMap(client.MapCode);
                /// Nếu bản đồ không cho phép giao dịch
                if (!map.AllowTrade)
                {
                    PlayerManager.ShowNotification(client, "Bản đồ hiện tại không cho phép giao dịch!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                //Kiểm tra xem có đang cấm giao dịch toàn máy chủ không

                int disableExchange = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-exchange", 0);
                if (disableExchange > 0)
                {   // Nếu cấm giao dịch thì chim cút
                    return TCPProcessCmdResults.RESULT_OK;
                }

                // Tiến hành đọc các PRAMENTER
                int otherRoleID = Convert.ToInt32(fields[1]);
                int exchangeType = Convert.ToInt32(fields[2]);
                int exchangeID = Convert.ToInt32(fields[3]);

                string strcmd = "";

                // Nếu là kiểu yêu cầu
                if (exchangeType == (int)GoodsExchangeCmds.Request)
                {
                    long ticks = TimeUtil.NOW();

                    //// Nếu thằng này bị band tra de thì chim cút
                    //if (TradeBlackManager.Instance().IsBanTrade(client.RoleID))
                    //{
                    //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, "Bạn đã bị cấm giao dịch", GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                    //    return TCPProcessCmdResults.RESULT_OK;
                    //}

                    //Kiểm tra lại để chắc rằng nó đang không giao dịch với ai khác hoặc đang mời giao dịch mà chưa được trả lời trong 50s
                    if (client.ExchangeID <= 0 || (client.ExchangeID > 0 && (ticks - client.ExchangeTicks) >= (50 * 1000)))
                    {
                        //HỎi thằng kia xem có đồng ý không
                        KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                        if (null == otherClient) //Nếu không tìm thấy thằng kai đâu
                        {
                            // Thì trả về cho client biết là người chơi này đã off
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -1, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        // Nếu thằng kia đang giao dịch với ai đó khác thì chim cút
                        if (otherClient.ExchangeID > 0)
                        {
                            // Gửi về là thằng này đang giao dịch với thằng khác rồi -2
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -2, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        // Nếu 2 thằng đang đứng khác bản đồ thì thông báo về
                        if (otherClient.MapCode != client.MapCode)
                        {
                            // Nếu đang khác bản đồ thì thông báo về mã lỗi -3
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -3, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        // NẾu 2 thằng đang đứng cách nhau xa quá 500 PIXCEL thì báo về khoảng cách quá xa không thể giao dịch
                        if (!Global.InCircle(new Point(otherClient.PosX, otherClient.PosY), new Point(client.PosX, client.PosY), 500))
                        {
                            // Gửi về mã lỗi -4 quá xa đéo thể giao dịch
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -4, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        //// Nếu mà thằng muốn mời nó bị cấm giao dịch
                        //if (TradeBlackManager.Instance().IsBanTrade(otherClient.RoleID))
                        //{
                        //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, "Đối phương bị cấm giao dịch", GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        //    return TCPProcessCmdResults.RESULT_OK;
                        //}

                        // Khởi tạo phiên giao dịch
                        int autoID = GameManager.GoodsExchangeMgr.GetNextAutoID();

                        client.ExchangeID = autoID;
                        client.ExchangeTicks = ticks;

                        //Gửi yêu cầu giao dịch về cho thằng này là toa muốn giao dịch
                        GameManager.ClientMgr.NotifyGoodsExchangeCmd(tcpMgr.MySocketListener, pool, roleID, otherRoleID, client, otherClient, autoID, exchangeType);
                    }
                    else
                    {
                        // Bạn đang giao dịch với người khác không thể tiến hành giao dịch tiếp
                        strcmd = string.Format("{0}:{1}:{2}:{3}", -10, roleID, otherRoleID, exchangeType);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                // Nếu là CMD đồng ý
                else if (exchangeType == (int)GoodsExchangeCmds.Agree)
                {
                    // Nếu bản thân mình chưa có phên giao dịch nào
                    if (client.ExchangeID <= 0)
                    {
                        // Tìm thằng đã mời mình giao dịch
                        KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                        if (null == otherClient)
                        {
                            // nếu thằng mời mình giao dịch mà offline thì thông báo
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -1, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        // Nếu thằng mời mình giao dịch lại đang giao dịch với 1 thằng khác thì cũng chim cút
                        if (otherClient.ExchangeID <= 0 || exchangeID != otherClient.ExchangeID)
                        {
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -2, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        // Nếu thằng mời mình giao dịch mà đứng khác map với mình thì chim cút
                        if (otherClient.MapCode != client.MapCode)
                        {
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -3, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        // Nếu thằng mời mình giao dịch mà đứng xa quá 500 pixel thì báo lỗi
                        if (!Global.InCircle(new Point(otherClient.PosX, otherClient.PosY), new Point(client.PosX, client.PosY), 500)) //对方是否在距离自己500像素范围内
                        {
                            // 2 thằng đang đứng quá xa
                            strcmd = string.Format("{0}:{1}:{2}:{3}", -4, roleID, otherRoleID, exchangeType);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        //Nếu ok tất thì khởi tạo sesssion giao dịch
                        ExchangeData ed = new ExchangeData()
                        {
                            RequestRoleID = otherRoleID,
                            AgreeRoleID = roleID,
                            GoodsDict = new Dictionary<int, List<GoodsData>>(),
                            MoneyDict = new Dictionary<int, int>(),
                            LockDict = new Dictionary<int, int>(),
                            DoneDict = new Dictionary<int, int>(),
                            AddDateTime = TimeUtil.NOW(),
                            Done = 0,
                            YuanBaoDict = new Dictionary<int, int>(),
                        };

                        GameManager.GoodsExchangeMgr.AddData(exchangeID, ed);

                        client.ExchangeID = exchangeID;
                        client.ExchangeTicks = 0;

                        //Gửi thông báo tao đồng ý giao dịch về cho cả 2 thằng
                        GameManager.ClientMgr.NotifyGoodsExchangeCmd(tcpMgr.MySocketListener, pool, roleID, otherRoleID, client, otherClient, exchangeID, exchangeType);
                    }
                    else
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}", -10, roleID, otherRoleID, exchangeType);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                else if (exchangeType == (int)GoodsExchangeCmds.Refuse) //Nếu nó từ chối giao dịch
                {
                    // Tìm thằng đã mời giao dịch
                    KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                    if (null == otherClient)
                    {
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    if (otherClient.ExchangeID <= 0 || otherClient.ExchangeID != exchangeID)
                    {
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    otherClient.ExchangeID = 0;
                    otherClient.ExchangeTicks = 0;

                    //Thông báo về là nó không đồng ý giao dịch
                    GameManager.ClientMgr.NotifyGoodsExchangeCmd(tcpMgr.MySocketListener, pool, roleID, otherRoleID, null, otherClient, exchangeID, exchangeType);
                }
                else if (exchangeType == (int)GoodsExchangeCmds.Cancel)
                {
                    //Đang giao dịch mà hủy giao dịch
                    if (client.ExchangeID > 0 && client.ExchangeID == exchangeID)
                    {
                        // Tìm ra phiên giao dịch của 2 thằng
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(exchangeID);
                        if (null != ed)
                        {
                            int done = 0;
                            lock (ed)
                            {
                                done = ed.Done;
                            }

                            if (done <= 0) //Nếu mà chưa hoàn thành giao dịch
                            {
                                //Xóa good datas ra khỏi phiên giao dịch
                                GameManager.GoodsExchangeMgr.RemoveData(exchangeID);

                                // Khôi phục lại tình trạng giao dịch
                                Global.RestoreExchangeData(client, ed);

                                client.ExchangeID = 0; //Reset lại ID
                                client.ExchangeTicks = 0;

                                //Tìm thằng còn lại của phiên giao dịch
                                KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                                if (null == otherClient)
                                {
                                    return TCPProcessCmdResults.RESULT_OK;
                                }

                                if (otherClient.ExchangeID <= 0 || exchangeID != otherClient.ExchangeID)
                                {
                                    return TCPProcessCmdResults.RESULT_OK;
                                }

                                // Khôi phục lại vật phẩm của thằng còn lại
                                Global.RestoreExchangeData(otherClient, ed);

                                otherClient.ExchangeID = 0;
                                otherClient.ExchangeTicks = 0;

                                //Thông báo hủy giao dịch tới cả 2 thằng
                                GameManager.ClientMgr.NotifyGoodsExchangeCmd(tcpMgr.MySocketListener, pool, roleID, otherRoleID, null, otherClient, exchangeID, exchangeType);
                            }
                        }
                    }
                }
                // Thêm vật phẩm vào phiên giao dịch
                else if (exchangeType == (int)GoodsExchangeCmds.AddGoods)
                {
                    int addGoodsDbID = exchangeID;

                    //Nếu gói tin là add goolds
                    if (client.ExchangeID > 0)
                    {
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(client.ExchangeID);

                        if (null != ed)
                        {
                            //Add vật phẩm sang ô giao dịch
                            Global.AddGoodsDataIntoExchangeData(client, addGoodsDbID, ed);

                            //Tìm thằng đang giao dịch cùng
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                            if (null == otherClient)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            if (otherClient.ExchangeID <= 0 || client.ExchangeID != otherClient.ExchangeID)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            //Gửi về thông tin của cả phiên giao dịch cho cả 2 thằng.Bao gồm đồ update lên lưới
                            GameManager.ClientMgr.NotifyGoodsExchangeData(tcpMgr.MySocketListener, pool, client, otherClient, ed);
                        }
                    }
                }
                // Gỡ vật phẩm ra khỏi phiên giao dịch
                else if (exchangeType == (int)GoodsExchangeCmds.RemoveGoods)
                {
                    int addGoodsDbID = exchangeID;

                    //Tìm phiên giao dịch hiện tại của thằng muốn gỡ
                    if (client.ExchangeID > 0)
                    {
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(client.ExchangeID);
                        // nếu tìm thấy phiên giao dịch
                        if (null != ed)
                        {
                            Global.RemoveGoodsDataFromExchangeData(client, addGoodsDbID, ed);

                            // Tìm thằng đang giao dịch với mình
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                            if (null == otherClient)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            if (otherClient.ExchangeID <= 0 || client.ExchangeID != otherClient.ExchangeID)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            //Gửi thông báo về cho cả 2 thằng về sự thay đổi của phiên giao dịch
                            GameManager.ClientMgr.NotifyGoodsExchangeData(tcpMgr.MySocketListener, pool, client, otherClient, ed);
                        }
                    }
                }
                else if (exchangeType == (int)GoodsExchangeCmds.UpdateMoney) //Update Bạc
                {
                    int updateMoney = exchangeID;
                    updateMoney = Global.GMax(updateMoney, 0);
                    updateMoney = Global.GMin(updateMoney, client.Money);

                    //Nếu mà có tìm thấy phiên giao dịch
                    if (client.ExchangeID > 0)
                    {
                        // Tìm ra phiên giao dịch
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(client.ExchangeID);
                        if (null != ed)
                        {
                            //Update tiền vào phiên giao dịch
                            Global.UpdateExchangeDataMoney(client, updateMoney, ed);

                            //Lấy ra thằng còn lại của phiên giao dịch
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                            if (null == otherClient) // nếu tìm ra thằng còn lại
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            if (otherClient.ExchangeID <= 0 || client.ExchangeID != otherClient.ExchangeID) // Nếu tìm thấy phiên giao dịch
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            //Gửi thông tin về cho cả 2 thằng về sự thay đổi
                            GameManager.ClientMgr.NotifyGoodsExchangeData(tcpMgr.MySocketListener, pool, client, otherClient, ed);
                        }
                    }
                }
                // nếu là khóa giao dịch
                else if (exchangeType == (int)GoodsExchangeCmds.Lock)
                {
                    //Tìm ra phiên giao dịch
                    if (client.ExchangeID > 0 && exchangeID == client.ExchangeID)
                    {
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(exchangeID);
                        if (null != ed)
                        {
                            //Tiến hành lock giao dịch
                            Global.LockExchangeData(roleID, ed, 1);

                            //Tìm ra thằng còn lại
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                            if (null == otherClient)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            if (otherClient.ExchangeID <= 0 || exchangeID != otherClient.ExchangeID)
                            {
                                return TCPProcessCmdResults.RESULT_OK;
                            }

                            //Thông báo cho cả 2 thằng về sự thay đổi của việc khóa GIAO DỊCH
                            GameManager.ClientMgr.NotifyGoodsExchangeData(tcpMgr.MySocketListener, pool, client, otherClient, ed);
                        }
                    }
                }
                // Nếu cả 2 thằng đều khóa và bắt đầu ấn hoàn tất giao dịch
                else if (exchangeType == (int)GoodsExchangeCmds.Done)
                {
                    if (client.ExchangeID > 0 && exchangeID == client.ExchangeID)
                    {
                        ExchangeData ed = GameManager.GoodsExchangeMgr.FindData(exchangeID);
                        if (null != ed)
                        {
                            //Nếu mà cả 2 thằng đã khóa
                            if (Global.IsLockExchangeData(roleID, ed) && Global.IsLockExchangeData(otherRoleID, ed))
                            {
                                //Nếu  đã hoàn tất giao dịch
                                if (Global.DoneExchangeData(roleID, ed))
                                {
                                    //Nếu đã hoàn tất giao dịch
                                    if (Global.IsDoneExchangeData(otherRoleID, ed))
                                    {
                                        //Lấy ra thằng còn lại
                                        KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                                        if (null == otherClient)
                                        {
                                            return TCPProcessCmdResults.RESULT_OK;
                                        }

                                        if (otherClient.ExchangeID <= 0 || exchangeID != otherClient.ExchangeID)
                                        {
                                            return TCPProcessCmdResults.RESULT_OK;
                                        }

                                        lock (ed)
                                        {
                                            ed.Done = 1; //Set là có hoàn thành
                                        }

                                        // Thực hiện giao dịch cho 2 thằng <Swap trừ tiền>
                                        int ret = Global.CompleteExchangeData(client, otherClient, ed);

                                        //Gỡ bỏ phiên giao dịch khỏi con quản lý giao dịch
                                        GameManager.GoodsExchangeMgr.RemoveData(exchangeID);

                                        if (ret < 0)
                                        {
                                            // Nếu giao dịch thất bại thì khôi phục lại vật phẩm
                                            Global.RestoreExchangeData(client, ed);

                                            // Nếu giao dịch thất bại thì khôi phục lại vật phẩm
                                            Global.RestoreExchangeData(otherClient, ed);
                                        }

                                        otherClient.ExchangeID = 0;
                                        otherClient.ExchangeTicks = 0;

                                        client.ExchangeID = 0;
                                        client.ExchangeTicks = 0;

                                        //Thông báo về cho cả 2 thằng về thông tin phiên giao dịch
                                        GameManager.ClientMgr.NotifyGoodsExchangeCmd(tcpMgr.MySocketListener, pool, roleID, otherRoleID, client, otherClient, ret, exchangeType);
                                    }
                                }
                            }
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

        #endregion Giao Dịch
    }
}
