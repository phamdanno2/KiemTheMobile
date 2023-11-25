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

namespace GameServer.KiemThe.Core.Activity.EveryDayOnlineEvent
{
    public class EveryDayOnlineManager
    {
        public static EveryDayOnLineEvent _Event = new EveryDayOnLineEvent();

        public static string KTEveryDayEvent_XML = "Config/KT_Activity/KTEveryDayEvent.xml";

        public static void Setup()
        {
            string Files = Global.GameResPath(KTEveryDayEvent_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(EveryDayOnLineEvent));
                _Event = serializer.Deserialize(stream) as EveryDayOnLineEvent;
            }
        }

        /// <summary>
        /// Lấy ra toàn bộ danh sách quà tặng có thể nhận khi online
        /// </summary>
        /// <param name="TimeSec"></param>
        /// <returns></returns>
        public static List<EveryDayOnLine> GetAllGiftCanGet(int TimeSec, int Step)
        {
            List<EveryDayOnLine> _Total = new List<EveryDayOnLine>();

            _Total = _Event.Item.Where(x => x.TimeSecs < TimeSec && x.StepID > Step).OrderBy(x => x.StepID).ToList();

            return _Total;
        }

        public static void GetAwardByTime(int Sec, AwardItem ItemGet, KPlayer player)
        {
            var ReviveItem = KTKTAsyncTask.Instance.ScheduleExecuteAsync(new DelayAsyncTask("OnlineRecvice", ItemGet, player, TimerProc), Sec * 1000);
            // Giải phóng task sau khi hoàn tất
            //ReviveItem.Wait();
            //ReviveItem.Dispose();
        }

        private static void TimerProc(object sender, EventArgs e)
        {
            DelayAsyncTask _Task = (DelayAsyncTask)sender;

            KPlayer _Player = _Task.Player;

            AwardItem _ItemGet = (AwardItem)_Task.Tag;

           // Console.WriteLine(_ItemGet.ItemID);

            if (_ItemGet != null)
            {
                // Thưc hiện add vật phẩm vào kỹ năng sôngs
                // Mặc định toàn bộ vật phẩm nhận từ event này sẽ khóa hết
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, _Task.Player, _ItemGet.ItemID, _ItemGet.Number, 0, "EVERYDAYONLINE", false, 1, false, Global.ConstGoodsEndTime,"",-1,"",0,1,false))
                {
                    PlayerManager.ShowNotification(_Player, "Có lỗi khi nhận vật phẩm chế tạo");
                }
            }

            //throw new NotImplementedException();
        }

        public static AwardItem GetItemAward(int StepID)
        {
            EveryDayOnLine _GiftSelect = _Event.Item.Where(x => x.StepID == StepID).FirstOrDefault();

            AwardItem _SelectItem = null;

            if (_GiftSelect != null)
            {
                List<AwardItem> RollAwardItem = _GiftSelect.RollAwardItem.ToList();

                int Random = KTGlobal.GetRandomNumber(0, 100);

                int idx = 0;
                foreach (AwardItem _Item in RollAwardItem)
                {
                    Random = Random - _Item.Rate;

                    if (Random <= 0)
                    {
                        _SelectItem = _Item;
                        break;
                    }
                    idx++;
                }
            }

            return _SelectItem;
        }

        #region TCP_NETWORK

        /// <summary>
        /// Lấy thông tin online nhận thưởng của người chơi
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteUpdateEverydayOnlineAwardGiftInfoCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Onlien Nhận thưởng lỗi , CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
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

                int nDate = TimeUtil.NowDateTime().DayOfYear;

                // Nếu mà ngày hôm nay mà khác ngày trong CSDL đẩy ra thì create ngày mới ==> Đoạn code này đề phòng nếu qua 12h đêm chuyển sang ngày mới thì F5 lại danh sách nhận cho nó
                if (client.MyHuodongData.GetEveryDayOnLineAwardDayID != nDate)
                {
                    client.MyHuodongData.EveryDayOnLineAwardStep = 0;
                    client.MyHuodongData.GetEveryDayOnLineAwardDayID = nDate;
                    client.MyHuodongData.EveryDayOnLineAwardGoodsID = "";
                }

                EveryDayOnLineEvent _Online = new EveryDayOnLineEvent();
                _Online.IsOpen = EveryDayOnlineManager._Event.IsOpen;
                _Online.Item = EveryDayOnlineManager._Event.Item;
                _Online.DayOnlineSecond = client.DayOnlineSecond;
                _Online.EveryDayOnLineAwardStep = client.MyHuodongData.EveryDayOnLineAwardStep;
                _Online.EveryDayOnLineAwardGoodsID = client.MyHuodongData.EveryDayOnLineAwardGoodsID;

                client.SendPacket<EveryDayOnLineEvent>(nID, _Online);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Class xử lý việc nhận quà onlien nhận thưởng
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGetEveryDayOnLineAwardGiftCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                //Lấy ra thông tin gửi lên
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);

                // Nếu nTimer là 1 là thì là online nhận thưởng
                // Nếu nTimer là 2 thì là đăng nhập 7 ngày

                string strcmd = "";

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}",
                                                                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int nDate = TimeUtil.NowDateTime().DayOfYear;

                // Nếu mà ngày hôm nay mà khác ngày trong CSDL đẩy ra thì create ngày mới ==> Đoạn code này đề phòng nếu qua 12h đêm chuyển sang ngày mới thì F5 lại danh sách nhận cho nó
                if (client.MyHuodongData.GetEveryDayOnLineAwardDayID != nDate)
                {
                    client.MyHuodongData.EveryDayOnLineAwardStep = 0;
                    client.MyHuodongData.GetEveryDayOnLineAwardDayID = nDate;
                    client.MyHuodongData.EveryDayOnLineAwardGoodsID = "";
                }

                int CurentStep = client.MyHuodongData.EveryDayOnLineAwardStep;

                // Lấy ra toàn bộ danh sách quà tặng có thể nhận trọng ngày hôm nay

                List<EveryDayOnLine> GetAllGiftCanGet = EveryDayOnlineManager.GetAllGiftCanGet(client.DayOnlineSecond, CurentStep);


                // Nếu có quà có thể nhận
                if (GetAllGiftCanGet.Count > 0)
                {
                    // Lấy ra mốc đầu tiên
                    EveryDayOnLine _SelectQua = GetAllGiftCanGet.FirstOrDefault();

                    int SpaceNeed = 10;

                    //foreach(AwardItem _item in _SelectQua.RollAwardItem)
                    //{

                    //    SpaceNeed += ItemManager.TotalSpaceNeed(_item.ItemID, _item.Number);
                    //}    


                    // Thực hiện xoay random xem nhận được quà gì

                    if (KTGlobal.IsHaveSpace(SpaceNeed, client))
                    {
                        Core.Activity.EveryDayOnlineEvent.AwardItem _ItemSelect = EveryDayOnlineManager.GetItemAward(_SelectQua.StepID);

                        // Thực hiện DELAY 7s xong add quà
                        EveryDayOnlineManager.GetAwardByTime(7, _ItemSelect, client);

                        // Update Step đã nhận cho client biết
                        client.MyHuodongData.EveryDayOnLineAwardStep = _SelectQua.StepID;

                        //Update đã nhận được cái gì rồi dựa trên những thứ đã nhận trước đó

                        client.MyHuodongData.EveryDayOnLineAwardGoodsID = client.MyHuodongData.EveryDayOnLineAwardGoodsID + "|" + _ItemSelect.ItemID + "," + _ItemSelect.Number;

                        // GHI LẠI TÍCH LŨY VÀO DB ĐÃ LIẾM QUÀ TRÁNH ROLLBACK LẠI ĂN LẠI ĐƯỢC LẦN NỮA
                        GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

                        //Gửi về client 6 PRAMENTER
                        //ID THẰNG QUAY - 1 : QUAY THÀNH CÔNG | MỐC SẼ THỰC HIỆN ANIMATION QUAY | THỜI GIAN ONLINE HIỆN TẠI | VẬT PHẨM SẼ QUAY VÀO
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, 1, _SelectQua.StepID, client.DayOnlineSecond, _ItemSelect.ItemID + "," + _ItemSelect.Number);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                        // TODO : THỰC HIỆN UPDATE ICON PHÁT SÁNG Ở ĐÂY NẾU KHI NÓ ĐÃ NHẬN

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    else
                    {
                        PlayerManager.ShowNotification(client, "Cần sắp xếp 10 ô trống trong túi đồ để nhận thưởng!");

                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, -1, CurentStep, client.DayOnlineSecond, "");

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                else // NẾU ĐÉO CÓ MỐC NÀO CÓ THỂ NHẬN NỮA ===> TRƯỜNG HỢP NÀY KHÔNG THỂ VÀO NHƯNG ĐỂ CHẮC CHẮN CLIENT CHECK KHÔNG CHUẨN TIMER THÌ GỬI VỀ THÔNG BÁO BẠN KHÔNG THỂ NHẬN QUÀ
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, -1, CurentStep, client.DayOnlineSecond, "");

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                    return TCPProcessCmdResults.RESULT_DATA;
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion TCP_NETWORK
    }
}