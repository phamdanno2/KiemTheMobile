using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using GameServer.Server;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Activity.TotalLoginEvent
{
    public class TotalLoginManager
    {
        public static TotalDayLoginSeries _Event = new TotalDayLoginSeries();

        public static string TotalDayLoginSeries_XML = "Config/KT_Activity/KTTotalDaySeriesLogin.xml";

        public static void Setup()
        {
            string Files = Global.GameResPath(TotalDayLoginSeries_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(TotalDayLoginSeries));
                _Event = serializer.Deserialize(stream) as TotalDayLoginSeries;
            }
        }

        /// <summary>
        /// Lấy ra toàn bộ danh sách quà tặng có thể nhận khi online
        /// </summary>
        /// <param name="TimeSec"></param>
        /// <returns></returns>
        public static Gift GetAllGiftCanGet(int Index)
        {
            Gift _Total = new Gift();

            _Total = _Event.Gift.Where(x => x.ID == Index).FirstOrDefault();

            return _Total;
        }

        public static bool CanGetAward(Gift _Gift, KPlayer player)
        {
            string TotalItemCanGet = _Gift.GoodsID;

            string[] Pram = TotalItemCanGet.Split('|');

            int COUNT = 10;//Pram.Count();

            if (!KTGlobal.IsHaveSpace(COUNT, player))
            {
                PlayerManager.ShowNotification(player, "Cần sắp xếp 10 ô trống trong túi đồ để nhận thưởng!");
                return false;
            }
            foreach (string Item in Pram)
            {
                string[] ItemPram = Item.Split(',');

                int ItemID = Int32.Parse(ItemPram[0]);
                int Number = Int32.Parse(ItemPram[1]);

                // Mặc định toàn bộ vật phẩm nhận từ event này sẽ khóa hết
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, ItemID, Number, 0, "TOTALDAYLOGIN", false, 1, false, Global.ConstGoodsEndTime,"",-1,"",0,1,true))
                {
                    PlayerManager.ShowNotification(player, "Có lỗi khi nhận vật phẩm chế tạo");
                }

            }

            return true;
        }

        #region TCP_NETWORK

        /// <summary>
        /// Truy vấn phần quà
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteQueryTotalLoginInfoCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                //Thông tin gửi lên nếu không phải là 1
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

                int nFlag = 0;
                nFlag = client.GetValueOfForeverRecore(RecoreDef.TotalSeriesLogin); // Global.GetRoleParamsInt32FromDB(client, RoleParamName.TotalLoginAwardFlag);

                int nLoginNum = (int)ChengJiuManager.GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.TotalDayLogin);

                if(nFlag==-1)
                {
                    nFlag = 0;
                   client.SetValueOfForeverRecore(RecoreDef.TotalSeriesLogin, nFlag);
                }
                // THÔNG TIN nFlag được mã hóa theo  Global.GetBitValue để đánh dấu vào DB để biết được nó đã nhận ngày đó chưa.
                // Cái này phải check source cũ ở client để xem nó quy ước
                //Gửi về cho client

                // ĐÂY LÀ ĐOẠN CHECK STATE Ở CLIENT ĐỂ BIẾT NÓ ĐÃ NHẬN CHƯA THEO nFlag GỬI VỀ EM ĐỌC ĐỂ XỬ LÝ HIỂN THỊ ĐÃ NHẬN HAY CHƯA NHẬN
                //if (1 == Global.GetIntSomeBit(nFlag, nDay))
                //{
                //    MUDebug.Log(Global.GetLang("存在"));
                //    if (null != item.m_BtnLingJiang)
                //    {
                //        item.m_BtnLingJiang.gameObject.SetActive(false);
                //    }
                //    item.m_LblDayNum.gameObject.SetActive(false);
                //    bEnable = false;
                //    item.m_SpriteState.spriteName = "yilingqu";
                //    item.m_SpriteState.gameObject.SetActive(true);
                //}
                //else
                //{
                //}

                //LogManager.WriteLog(LogTypes.GameMapEvents, "nFlag :" + nFlag + "|nLoginNum :" + nLoginNum);

                //strcmd = string.Format("{0}:{1}:{2}", client.RoleID, nLoginNum, nFlag);
                TotalDayLoginSeries seriesLogin = new TotalDayLoginSeries()
                {
                    IsOpen = TotalLoginManager._Event.IsOpen,
                    Name = TotalLoginManager._Event.Name,
                    Gift = TotalLoginManager._Event.Gift,
                    LoginNum = nLoginNum,
                    Flag = nFlag,
                };
                byte[] _cmdData = DataHelper.ObjectToBytes<TotalDayLoginSeries>(seriesLogin);

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
        /// Ấn Nhận quà
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGetTotalLoginAwardCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                //Xem gửi lên có phỉa là 2 pramenter ko
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);   // ROLEDAT MUỐN NHẬN

                int nIndex = Convert.ToInt32(fields[1]);     // INDEX CỦA VỊ TRÍ MUỐN NHẬN

                string strcmd = "";

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}",
                                                                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int nRet = 0;

                int nLoginNum = (int)ChengJiuManager.GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.TotalDayLogin);

                Gift _GiftInfo = TotalLoginManager.GetAllGiftCanGet(nIndex);

                // Nếu mà chưa online đủ ngày mà đã muốn nhận thì thông báo toang về
                if (_GiftInfo.TimeOl > nLoginNum)
                {
                    strcmd = string.Format("{0}:{1}:{2}", client.RoleID, nIndex, -1);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nFlag = 0;


               // nFlag = Global.GetRoleParamsInt32FromDB(client, RoleParamName.TotalLoginAwardFlag);

                nFlag = client.GetValueOfForeverRecore(RecoreDef.TotalSeriesLogin);

                int nValue = 0;
                nValue = nFlag & Global.GetBitValue(nIndex + 1);

                // NẾU CHƯA NHẬN THÌ THỰC HIỆN CHO NHẬN
                if (nValue == 0)
                {
                    if (TotalLoginManager.CanGetAward(_GiftInfo, client))
                    {
                        // ĐÁNH DẤU VÀO DB
                        nFlag = nFlag | Global.GetBitValue(nIndex + 1);

                        client.SetValueOfForeverRecore(RecoreDef.TotalSeriesLogin, nFlag);


                      //  Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.TotalLoginAwardFlag, nFlag, true);

                        nRet = 1;
                    }
                    else
                    {
                        // Nhận có lỗi
                        strcmd = string.Format("{0}:{1}:{2}", client.RoleID, nIndex, -3);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                else
                {
                    // Nếu mà đã nhận rồi thì chim cút
                    strcmd = string.Format("{0}:{1}:{2}", client.RoleID, nIndex, -2);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // TRẢ VỀ CHO CLIENT KẾT QUẢ
                strcmd = string.Format("{0}:{1}:{2}", client.RoleID, nIndex, nRet);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion TÍCH LŨY ĐĂNG NHẬP 30 NGÀY
    }
}