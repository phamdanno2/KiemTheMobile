using GameServer.Core.Executor;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using GameServer.Server;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Activity.DaySeriesLoginEvent
{
    public class SevenDayLoginManager
    {
        public static string KTSevenDaysLogin_XML = "Config/KT_Activity/KTSevenDaysLogin.xml";

        public static SevenDaysLogin _Event = new SevenDaysLogin();

        public static void Setup()
        {
            string Files = Global.GameResPath(KTSevenDaysLogin_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(SevenDaysLogin));
                _Event = serializer.Deserialize(stream) as SevenDaysLogin;
            }
        }



        /// <summary>
        /// Lấy ra toàn bộ danh sách quà tặng có thể nhận khi online
        /// </summary>
        /// <param name="TimeSec"></param>
        /// <returns></returns>
        public static SevenDaysLoginItem GetAllGiftCanGet(int Days)
        {
            SevenDaysLoginItem _Total = new  SevenDaysLoginItem();

            _Total = _Event.SevenDaysLoginItem.Where(x => x.Days== Days).OrderBy(x => x.ID).FirstOrDefault();

            return _Total;
        }

        public static void GetAwardByTime(int Sec, RollAwardItem ItemGet, KPlayer player)
        {
            var ReviveItem = KTKTAsyncTask.Instance.ScheduleExecuteAsync(new DelayAsyncTask("SEVENRDAYLOGIN", ItemGet, player, TimerProc), Sec * 1000);
            // Giải phóng task sau khi hoàn tất
            //ReviveItem.Wait();
            //ReviveItem.Dispose();


        }

        private static void TimerProc(object sender, EventArgs e)
        {
            DelayAsyncTask _Task = (DelayAsyncTask)sender;

            RollAwardItem _ItemGet = (RollAwardItem)_Task.Tag;

            KPlayer _Player = _Task.Player;

           // Console.WriteLine(_ItemGet.ItemID);

            if (_ItemGet != null)
            {
                // Thưc hiện add vật phẩm vào kỹ năng sôngs
                // Mặc định toàn bộ vật phẩm nhận từ event này sẽ khóa hết
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, _Task.Player, _ItemGet.ItemID, _ItemGet.Number, 0, "SEVENRDAYLOGIN", false, 1, false, Global.ConstGoodsEndTime))
                {
                    PlayerManager.ShowNotification(_Player, "Có lỗi khi nhận vật phẩm chế tạo");
                }
            }



            //throw new NotImplementedException();
        }

        public static RollAwardItem GetItemAward(int Days)
        {
            SevenDaysLoginItem _GiftSelect = _Event.SevenDaysLoginItem.Where(x => x.Days == Days).FirstOrDefault();

            RollAwardItem _SelectItem = null;

            if (_GiftSelect != null)
            {
                List<RollAwardItem> RollAwardItem = _GiftSelect.RollAwardItem.ToList();

                int Random = KTGlobal.GetRandomNumber(0, 100);

                foreach (RollAwardItem _Item in RollAwardItem)
                {
                    Random = Random - _Item.Rate;

                    if (Random <= 0)
                    {
                        _SelectItem = _Item;

                        break;
                    }
                }
            }

            return _SelectItem;
        }


        #region SEVENDAYLOGIN_AWARD

        /// <summary>
        /// Lấy ra thông tin quà dăng nhập 7 ngày
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
        public static TCPProcessCmdResults ProcessSpriteUpdateEverydaySeriesLoginInfoCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);

                //string strcmd = "";

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}",
                                                                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                // Danh Sách trả về bao gồm | ROLEID | NGÀY THỨ MẤY ĐANG NHẬN | ĐÃ NHẬN MỐC NÀO RỒI | TRƯỚC ĐÓ ĐÃ NHẬN CÁI GÌ ĐỂ CÒN HIỂN THỊ TRÊN BẢNG RANDOM (TIEMID,SOLUONG|ITEMID,SOLUONG)
                // Nếu chưa hiểu thì hỏi lại anh
                //strcmd = string.Format("{0}:{1}:{2}:{3}", client.RoleID, Global.GMin(client.SeriesLoginNum, 7), client.MyHuodongData.SeriesLoginGetAwardStep, client.MyHuodongData.SeriesLoginAwardGoodsID);

                SevenDaysLogin sevenDaysLogin = new SevenDaysLogin()
                {
                    IsOpen = SevenDayLoginManager._Event.IsOpen,
                    Name = SevenDayLoginManager._Event.Name,
                    SevenDaysLoginItem = SevenDayLoginManager._Event.SevenDaysLoginItem,
                    SeriesLoginNum = Global.GMin(client.SeriesLoginNum, 7)+1,
                    SeriesLoginGetAwardStep = client.MyHuodongData.SeriesLoginGetAwardStep,
                    SeriesLoginAwardGoodsID = client.MyHuodongData.SeriesLoginAwardGoodsID,
                };
                byte[] _cmdData = DataHelper.ObjectToBytes<SevenDaysLogin>(sevenDaysLogin);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, _cmdData, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Nhận quà 7 ngày đăng nhập
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGetSeriesLoginAwardGiftCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}",
                                                                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int nDay = TimeUtil.NowDateTime().DayOfYear;

                /// Toác
                if (client.MyHuodongData == null)
                {
                    PlayerManager.ShowNotification(client, "Không có phần thưởng để nhận!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                // Check xem nếu đã bú quà này rồi thì báo đã nhận rồi
                if (client.MyHuodongData.SeriesLoginAwardDayID == nDay && client.MyHuodongData.SeriesLoginGetAwardStep >= client.SeriesLoginNum)
                {
                    PlayerManager.ShowNotification(client, "Bạn đã nhận thưởng rồi không thể nhận lại!");
                    return TCPProcessCmdResults.RESULT_OK;
                }


                // LẤY RA QUÀ TẶNG CỦA NGÀY HÔM NAY
                RollAwardItem _ItemSelect = SevenDayLoginManager.GetItemAward(client.MyHuodongData.SeriesLoginGetAwardStep + 1);
                /// Toác
                if (_ItemSelect == null)
                {
                    PlayerManager.ShowNotification(client, "Không có phần thưởng để nhận!");
                    return TCPProcessCmdResults.RESULT_OK;
                }


                if (!KTGlobal.IsHaveSpace(10, client))
				{
                    PlayerManager.ShowNotification(client, "Túi đồ không đủ chỗ trống để nhận thưởng\n Cần ít nhất 10 ô trống");
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, -1, client.MyHuodongData.SeriesLoginGetAwardStep, client.SeriesLoginNum, _ItemSelect.ItemID + "," + _ItemSelect.Number);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                SevenDaysLoginItem GetAllGiftCanGet = SevenDayLoginManager.GetAllGiftCanGet(client.MyHuodongData.SeriesLoginGetAwardStep + 1);

                // Thực hiện DELAY 5s xong add quà
                SevenDayLoginManager.GetAwardByTime(7, _ItemSelect, client);

                // Update Step đã nhận cho client biết
                client.MyHuodongData.SeriesLoginGetAwardStep++;

                //Update đã nhận được cái gì rồi dựa trên những thứ đã nhận trước đó
                client.MyHuodongData.SeriesLoginAwardGoodsID = client.MyHuodongData.SeriesLoginAwardGoodsID + "|" + _ItemSelect.ItemID + "," + _ItemSelect.Number;


                // Nhận tới ngày 7 lại reset về ngày 0 cho nó nhận lại
                //if(client.MyHuodongData.SeriesLoginGetAwardStep>=7)
                //{
                //    client.MyHuodongData.SeriesLoginGetAwardStep = 0;
                //    client.MyHuodongData.SeriesLoginAwardGoodsID = "";
                //}

                // GHI LẠI TÍCH LŨY VÀO DB ĐÃ LIẾM QUÀ TRÁNH ROLLBACK LẠI ĂN LẠI ĐƯỢC LẦN NỮA
                GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

                //Gửi về client 6 PRAMENTER
                //ID THẰNG QUAY - 1 : QUAY THÀNH CÔNG | NGÀY THỰC HIỆN QUAY | SỐ VÒNG SẼ QUAY
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, 1, client.MyHuodongData.SeriesLoginGetAwardStep, client.SeriesLoginNum, _ItemSelect.ItemID + "," + _ItemSelect.Number);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                // TODO : THỰC HIỆN UPDATE ICON PHÁT SÁNG Ở ĐÂY NẾU KHI NÓ ĐÃ NHẬN

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion SEVENDAYLOGIN_AWARD
    }
}