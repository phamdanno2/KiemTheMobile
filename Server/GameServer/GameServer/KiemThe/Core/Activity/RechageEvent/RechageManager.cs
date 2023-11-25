using GameServer.KiemThe.Core.Item;
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

namespace GameServer.KiemThe.Core.Activity.RechageEvent
{
    public class RechageManager
    {
        // Config quà nạp đầu
        public static FistRechage _FistRechageConfig = new FistRechage();

        //Config quà nạp ngày

        public static DayRechage _DayRechage = new DayRechage();

        public static TotalRechage _TotalRechage = new TotalRechage();

        public static TotalConsume _TotalConsume = new TotalConsume();

        public static string KT_FistRechage_XML = "Config/KT_Activity/KT_FistRechage.xml";

        public static string KTDayRechage_XML = "Config/KT_Activity/KTDayRechage.xml";

        public static string TotalRechage_XML = "Config/KT_Activity/KTTotalRechage.xml";

        public static string KTTotalConsume_XML = "Config/KT_Activity/KTTotalConsume.xml";

        // Load thực thể
        public static void Setup()
        {
            string Files = Global.GameResPath(KT_FistRechage_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(FistRechage));

                _FistRechageConfig = serializer.Deserialize(stream) as FistRechage;
            }

            Files = Global.GameResPath(KTDayRechage_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(DayRechage));

                _DayRechage = serializer.Deserialize(stream) as DayRechage;
            }

            Files = Global.GameResPath(TotalRechage_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(TotalRechage));

                _TotalRechage = serializer.Deserialize(stream) as TotalRechage;
            }

            Files = Global.GameResPath(KTTotalConsume_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(TotalConsume));

                _TotalConsume = serializer.Deserialize(stream) as TotalConsume;
            }
        }

        #region TCP_NETWORK

        private static bool GetCmdDataField(TMSKSocket socket, int nID, byte[] data, int count, out string[] fields)
        {
            string cmdData = null;
            fields = null;
            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lỗi dữ liệu đầu vào, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return false;
            }

            fields = cmdData.Split(':');
            return true;
        }

        /// <summary>
        /// Lấy ra trạng thái của nút dựa vào số tiền đã nạp
        /// </summary>
        /// <param name="money"></param>
        /// <param name="minMoney"></param>
        /// <param name="recode"></param>
        /// <returns></returns>
        public static int GetBtnIndexState(int money, int minMoney, bool recode)
        {
            // Ko thể nhận
            int ret = 0;

            // Đã nhận
            if (money >= minMoney && recode)
            {
                ret = 2;
            }
            // Chưa nhận
            if (money >= minMoney && recode == false)
            {
                ret = 1;
            }
            return ret;
        }

        private static string GetBtnIndexStateListStr(KPlayer client, int money, ActivityTypes type, string[] records)
        {
            string ret = "";

            List<int> condision = new List<int>();

            switch (type)
            {
                // Nạp ngày
                case ActivityTypes.MeiRiChongZhiHaoLi:
                    {
                        foreach (DayRechageAward DayRechageAward in _DayRechage.DayRechageAward)
                        {
                            condision.Add(DayRechageAward.MinYuanBao);
                        }
                    }

                    break;
                // Tích nạp
                case ActivityTypes.TotalCharge:
                    {
                        foreach (TotalRechageAward _Rechage in _TotalRechage.TotalRechageAward)
                        {
                            condision.Add(_Rechage.MinYuanBao);
                        }
                    }
                    break;
                // Tích tiêu
                case ActivityTypes.TotalConsume:
                    {
                        foreach (ConsumeAward _Rechage in _TotalConsume.ConsumeAward)
                        {
                            condision.Add(_Rechage.MinYuanBao);
                        }
                    }
                    break;
            }

            for (int i = 0; i < condision.Count; i++)
            {
                bool rec = false;
                if (i < records.Length)
                    rec = records[i] == "2" ? true : false;
                ret += GetBtnIndexState(money, condision[i], rec);
                if (i < condision.Count - 1)
                    ret += ",";
            }

            return ret;
        }

        /// <summary>
        /// Thông tin hoạt động phản hồi truy vấn, lần nạp đầu tiên, nạp tiền hàng ngày, nạp tiền tích lũy, tiêu thụ tích lũy
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
        ///

        public static string GetFistRechage(KPlayer client)
        {
            int totalChongZhiMoney = 0;

            totalChongZhiMoney = GameManager.ClientMgr.QueryTotaoChongZhiMoney(client);

            totalChongZhiMoney = Global.TransMoneyToYuanBao(totalChongZhiMoney);

            string resoult = GetBtnIndexState(totalChongZhiMoney, 2000, !(Global.CanGetFirstChongZhiDaLiByUserID(client))) + ":" + totalChongZhiMoney;

            return resoult;
        }

        public static string GetDayRechage(KPlayer client, TCPClientPool tcpClientPool, TCPOutPacketPool pool)
        {
            string[] dbFields = null;
            int totalChongZhiMoney = 0;
            string[] fields = null;

            int totalChongZhiMoneyToday = GameManager.ClientMgr.QueryTotaoChongZhiMoneyToday(client);
            totalChongZhiMoney = Global.TransMoneyToYuanBao(totalChongZhiMoneyToday);

            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", client.RoleID, (int)ActivityTypes.MeiRiChongZhiHaoLi), out dbFields, client.ServerId);
            if (null == dbFields)
                return "TOACH";
            if (dbFields.Length != 3)
                return "TOACH";
            string[] rec = dbFields[1].Split(',');
            string resoult = GetBtnIndexStateListStr(client, totalChongZhiMoney, ActivityTypes.MeiRiChongZhiHaoLi, rec);
            resoult += ":" + totalChongZhiMoney;

            return resoult;
        }

        public static string GeTotalCustome(KPlayer client, TCPClientPool tcpClientPool, TCPOutPacketPool pool)
        {
            string[] dbFields = null;

            string[] fields = null;

            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", client.RoleID, (int)ActivityTypes.TotalConsume), out dbFields, client.ServerId);
            if (null == dbFields)
                return "TOACH";
            if (dbFields.Length != 3)
                return "TOACH";
            int totalusedmoney = Global.SafeConvertToInt32(dbFields[2]);
            string[] rec = dbFields[1].Split(',');
            string resoult = GetBtnIndexStateListStr(client, totalusedmoney, ActivityTypes.TotalConsume, rec);
            resoult = string.Format("{0}:{1}", resoult, totalusedmoney);

            return resoult;
        }

        public static string GetTotalRechage(KPlayer client, TCPClientPool tcpClientPool, TCPOutPacketPool pool)
        {
            string[] dbFields = null;
            int totalChongZhiMoney = 0;
            string[] fields = null;

            totalChongZhiMoney = GameManager.ClientMgr.QueryTotaoChongZhiMoney(client);
            totalChongZhiMoney = Global.TransMoneyToYuanBao(totalChongZhiMoney);
            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", client.RoleID, (int)ActivityTypes.TotalCharge), out dbFields, client.ServerId);
            if (null == dbFields)
                return "TOACH";
            if (dbFields.Length != 3)
                return "TOACH";
            string[] rec = dbFields[1].Split(',');
            string resoult = GetBtnIndexStateListStr(client, totalChongZhiMoney, ActivityTypes.TotalCharge, rec);
            resoult = string.Format("{0}:{1}", resoult, totalChongZhiMoney);

            return resoult;
        }

        public static TCPProcessCmdResults QueryRechargeRepayActive(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            if (!GetCmdDataField(socket, nID, data, count, out fields))
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            if (fields.Length != 1)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Sai cú pháp gửi lên, CMD={0}, Client={1}, Recv={2}",
                    (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            int roleID = Convert.ToInt32(fields[0]);


            KPlayer client = GameManager.ClientMgr.FindClient(socket);
            if (null == client || client.RoleID != roleID)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Không tìm thấy client này, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }



            RechageAcitivty _RechageAcitivy = new RechageAcitivty();

            _RechageAcitivy._DayRechage = _DayRechage;
            //Lấy ra trạng thái nút nạp ngày
            _RechageAcitivy._DayRechage.BtnState = GetDayRechage(client, tcpClientPool, pool);

            _RechageAcitivy._FistRechage = _FistRechageConfig;
            // Lấy ra trạng thái nút của Nạp đầu
            _RechageAcitivy._FistRechage.BtnState = GetFistRechage(client);

            _RechageAcitivy._TotalConsume = _TotalConsume;
            _RechageAcitivy._TotalConsume.BtnState = GeTotalCustome(client, tcpClientPool, pool);

            _RechageAcitivy._TotalRechage = _TotalRechage;
            _RechageAcitivy._TotalRechage.BtnState = GetTotalRechage(client, tcpClientPool, pool);

            byte[] Data = DataHelper.ObjectToBytes<RechageAcitivty>(_RechageAcitivy);

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, Data, 0, Data.Length, nID);

            return TCPProcessCmdResults.RESULT_DATA;
        }

        public static void JugeCompleteChongZhiSecondTask(KPlayer client, int taskID)
        {
            if (client.CZTaskID == taskID) return;

            client.CZTaskID = taskID;

            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATECZTASKID,
                string.Format("{0}:{1}", client.RoleID, taskID),
                null, client.ServerId);
        }

        public static string BuildWriteActiveRecordStr(string record, int nBtnIndex)
        {
            string activeRecord = "";
            string[] recordlist = record.Split(',');

            List<string> writeRecord = new List<string>();
            int cout = nBtnIndex;
            if (nBtnIndex < recordlist.Length)
                cout = recordlist.Length;
            for (int i = 0; i < cout; i++)
            {
                if (i < recordlist.Length)
                    writeRecord.Add(recordlist[i]);
                else
                {
                    writeRecord.Add("1");//默认是未领取
                }
            }
            writeRecord[nBtnIndex - 1] = "2";//3代表领取
            for (int i = 0; i < writeRecord.Count; i++)
            {
                activeRecord += writeRecord[i];
                if (i < writeRecord.Count - 1)
                    activeRecord += ",";
            }
            return activeRecord;
        }

        private static TCPProcessCmdResults GetFirstChargeAward(TMSKSocket socket, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string[] fields = null;
            if (!GetCmdDataField(socket, nID, data, count, out fields))
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Pram gửi lên có vấn đề, CMD={0}, Client={1}, Recv={2}",
                   (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("KHông tìm thấy ROLEID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }
                string strcmd = "";

                // Nếu đã liếm rồi thì thôi
                if (client.CZTaskID > 0)
                {
                    strcmd = string.Format("{0}:{1}:{2}", -1, (int)ActivityTypes.InputFirst, 1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // Check nó đã nạp chưa
                if (!Global.CanGetFirstChongZhiDaLiByUserID(client))
                {
                    strcmd = string.Format("{0}:{1}:{2}", -10, (int)ActivityTypes.InputFirst, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int TotalItemCount = 10;//_FistRechageConfig.TotalItem.Count;

                // Nếu không đủ chỗ nhận thì báo về là không đủ
                if (!KTGlobal.IsHaveSpace(TotalItemCount, client))
                {
                    strcmd = string.Format("{0}:{1}:{2}", -20, (int)ActivityTypes.InputFirst, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // Lấy ra số đã nạp
                int totalChongZhiMoney = GameManager.ClientMgr.QueryTotaoChongZhiMoney(client);

                // Nếu chưa nạp tý nào
                if (totalChongZhiMoney <= 0)
                {
                    strcmd = string.Format("{0}:{1}:{2}", -30, (int)ActivityTypes.InputFirst, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                foreach (TotalItem _ItemGet in _FistRechageConfig.TotalItem)
                {
                    if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, _ItemGet.ItemID, _ItemGet.Number, 0, "FISTRECHAGE", false, 1, false, Global.ConstGoodsEndTime))
                    {
                        PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm");
                    }
                }

                //Update vào DB là nó đã liếm rồi
                JugeCompleteChongZhiSecondTask(client, 1);

                // TODO : REFRESH ICON CLIENT Ở ĐÂY

                strcmd = string.Format("{0}:{1}:{2}", 0, (int)ActivityTypes.InputFirst, 2);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults ProcessGetRepayAwardCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string[] fields = null;
            if (!GetCmdDataField(socket, nID, data, count, out fields))
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                int nRoleID = Convert.ToInt32(fields[0]);
                int nActivityType = Global.SafeConvertToInt32(fields[1]);
                int nBtnIndex = Convert.ToInt32(fields[2]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != nRoleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Nhận thưởng, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), nRoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string strcmd = "";

                int result = (int)ActivityErrorType.RECEIVE_SUCCEED;
                string nRetValue = "";

                ActivityTypes tmpActType = (ActivityTypes)nActivityType;
                switch (tmpActType)
                {
                    // Xử lý xong nhận nạp đầu
                    case ActivityTypes.InputFirst:
                        {
                            return RechageManager.GetFirstChargeAward(socket, pool, nID, data, count, out tcpOutPacket);
                        }
                    case ActivityTypes.MeiRiChongZhiHaoLi:
                        {
                            DayRechageAward _Rechage = _DayRechage.DayRechageAward.Where(x => x.ID == nBtnIndex).FirstOrDefault();

                            // Nếu mà null thì chim cút
                            if (_Rechage == null)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ACTIVITY_NOTEXIST, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string ItemPrams = _Rechage.GoodsIDs;

                            int Count = 10;//ItemPrams.Split('|').Length;

                            if (!KTGlobal.IsHaveSpace(Count, client))
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.BAG_NOTENOUGH, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string[] dbFields = null;
                            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", nRoleID, (int)tmpActType), out dbFields, client.ServerId);
                            if (dbFields == null)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            if (dbFields != null && dbFields.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            int retcode = Global.SafeConvertToInt32(dbFields[0]);
                            if (retcode != 1)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            string[] retIndexarry = dbFields[1].Split(',');

                            if (nBtnIndex > 0 && nBtnIndex <= retIndexarry.Length && retIndexarry[nBtnIndex - 1] == "2")
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ALREADY_GETED, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            // Xem cái mốc nó muốn nhận có tồn tại không

                            int totalChongZhiMoneyToday = GameManager.ClientMgr.QueryTotaoChongZhiMoneyToday(client);

                            totalChongZhiMoneyToday = Global.TransMoneyToYuanBao(totalChongZhiMoneyToday);

                            // Nếu tổng số đã nạp hôm nay mà không đủ thì báo toạch
                            if (totalChongZhiMoneyToday < _Rechage.MinYuanBao)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.MINAWARDCONDIONVALUE, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            //Ghi lịa nhật ký vào CSDL là nó đã nhận rồi
                            string[] dbFields2 = null;

                            string writerec = RechageManager.BuildWriteActiveRecordStr(dbFields[1], nBtnIndex);

                            Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_GET_REPAYACTIVEAWARD, string.Format("{0}:{1}:{2}", nRoleID, (int)tmpActType, writerec.Replace(",", "")), out dbFields2, client.ServerId);
                            if (dbFields2 == null || dbFields2.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;

                            string[] TotalItem = _Rechage.GoodsIDs.Split('|');

                            foreach (string item in TotalItem)
                            {
                                int ItemID = Int32.Parse(item.Split(',')[0]);
                                int Number = Int32.Parse(item.Split(',')[1]);

                                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, ItemID, Number, 0, "DAYRECHAGE", false, 1, false, Global.ConstGoodsEndTime, "", -1, "", 0, 1, true))
                                {
                                    PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm");
                                }
                            }

                            // TODO : RESET ICON Ở CLIENT

                            nRetValue = writerec;
                            break;
                        }
                    case ActivityTypes.TotalCharge:
                        {
                            TotalRechageAward _Find = _TotalRechage.TotalRechageAward.Where(x => x.ID == nBtnIndex).FirstOrDefault();

                            // nếu ko tìm thấy thì báo không tìm thấy hoạt động
                            if (_Find == null)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ACTIVITY_NOTEXIST, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string ItemPrams = _Find.GoodsIDs;

                            int Count = 10;//ItemPrams.Split('|').Length;

                            if (!KTGlobal.IsHaveSpace(Count, client))
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.BAG_NOTENOUGH, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string[] dbFields = null;
                            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", nRoleID, (int)tmpActType), out dbFields, client.ServerId);
                            if (dbFields == null)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            if (dbFields != null && dbFields.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            int retcode = Global.SafeConvertToInt32(dbFields[0]);
                            if (retcode != 1)
                                return TCPProcessCmdResults.RESULT_FAILED;

                            string[] retIndexarry = dbFields[1].Split(',');

                            //Kiểm tra xem đã nhận mốc này chưa
                            if (nBtnIndex > 0 && nBtnIndex <= retIndexarry.Length && retIndexarry[nBtnIndex - 1] == "2")
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ALREADY_GETED, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            //Lấy ra tổng số tiền đã nạp
                            int totalMoney = GameManager.ClientMgr.QueryTotaoChongZhiMoney(client);

                            // Convert theo rate
                            totalMoney = Global.TransMoneyToYuanBao(totalMoney);

                            // Nếu nạp chưa đủ thì báo về là không thỏa mãn điều kiện
                            if (totalMoney < _Find.MinYuanBao)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.NOTCONDITION, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            // Ghi vào DB đã nhận thưởng mốc này theo quy tắc mã hóa
                            string[] dbFields2 = null;

                            string writerec = BuildWriteActiveRecordStr(dbFields[1], nBtnIndex);
                            Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_GET_REPAYACTIVEAWARD, string.Format("{0}:{1}:{2}", nRoleID, (int)ActivityTypes.TotalCharge, writerec.Replace(",", "")), out dbFields2, client.ServerId);
                            if (dbFields2 == null || dbFields2.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;

                            // Phát quà
                            string[] TotalItem = _Find.GoodsIDs.Split('|');

                            foreach (string item in TotalItem)
                            {
                                int ItemID = Int32.Parse(item.Split(',')[0]);
                                int Number = Int32.Parse(item.Split(',')[1]);

                                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, ItemID, Number, 0, "TOTALRECHAGE", false, 1, false, Global.ConstGoodsEndTime, "", -1, "", 0, 1, true))
                                {
                                    PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm");
                                }
                            }

                            // TODO : F5 LẠI ICON  Ở CLIENT

                            nRetValue = writerec;
                            break;
                        }
                    case ActivityTypes.TotalConsume:
                        {
                            ConsumeAward _Find = _TotalConsume.ConsumeAward.Where(x => x.ID == nBtnIndex).FirstOrDefault();

                            // nếu ko tìm thấy thì báo không tìm thấy hoạt động
                            if (_Find == null)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ACTIVITY_NOTEXIST, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string ItemPrams = _Find.GoodsIDs;

                            int Count = 10;//ItemPrams.Split('|').Length;

                            // Kiểm tra xem đủ chỗ không
                            if (!KTGlobal.IsHaveSpace(Count, client))
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.BAG_NOTENOUGH, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string[] dbFields = null;
                            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", nRoleID, (int)ActivityTypes.TotalConsume), out dbFields, client.ServerId);
                            if (dbFields == null)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            if (dbFields != null && dbFields.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            int retcode = Global.SafeConvertToInt32(dbFields[0]);
                            if (retcode != 1)
                                return TCPProcessCmdResults.RESULT_FAILED;
                            string[] retIndexarry = dbFields[1].Split(',');

                            //Kiểm tra xem đã nhận chưa
                            if (nBtnIndex <= retIndexarry.Length && retIndexarry[nBtnIndex - 1] == "2")
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.ALREADY_GETED, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            int totalMoney = Global.SafeConvertToInt32(dbFields[2]);

                            // Nếu ko đủ điều kiện nhận thì thôi
                            if (_Find.MinYuanBao > totalMoney)
                            {
                                strcmd = string.Format("{0}:{1}:", (int)ActivityErrorType.NOTCONDITION, nActivityType);
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            string[] dbFields2 = null;
                            string writerec = BuildWriteActiveRecordStr(dbFields[1], nBtnIndex);
                            Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_GET_REPAYACTIVEAWARD, string.Format("{0}:{1}:{2}", nRoleID, (int)tmpActType, writerec.Replace(",", "")), out dbFields2, client.ServerId);
                            if (dbFields2 == null || dbFields2.Length != 3)
                                return TCPProcessCmdResults.RESULT_FAILED;

                            // Phát quà
                            string[] TotalItem = _Find.GoodsIDs.Split('|');

                            foreach (string item in TotalItem)
                            {
                                int ItemID = Int32.Parse(item.Split(',')[0]);
                                int Number = Int32.Parse(item.Split(',')[1]);

                                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, client, ItemID, Number, 0, "TOTALCONSUME", false, 1, false, Global.ConstGoodsEndTime, "", -1, "", 0, 1, true))
                                {
                                    PlayerManager.ShowNotification(client, "Có lỗi khi nhận vật phẩm");
                                }
                            }

                            // TODO : F5 LẠI ICON  Ở CLIENT

                            nRetValue = writerec;
                            break;
                        }

                    default:
                        break;
                }

                strcmd = string.Format("{0}:{1}:{2}", result, nActivityType, nRetValue);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults QueryAllRechargeRepayActiveInfo(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;
            if (!GetCmdDataField(socket, nID, data, count, out fields))
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            if (fields.Length != 1)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("指令参数个数错误, CMD={0}, Client={1}, Recv={2}",
                    (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                return TCPProcessCmdResults.RESULT_FAILED;
            }
            int roleID = Convert.ToInt32(fields[0]);
            KPlayer client = GameManager.ClientMgr.FindClient(socket);
            if (null == client || client.RoleID != roleID)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("根据RoleID定位GameClient对象失败, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            int totalChongZhiMoney = GameManager.ClientMgr.QueryTotaoChongZhiMoney(client);
            totalChongZhiMoney = Global.TransMoneyToYuanBao(totalChongZhiMoney);

            int totalChongZhiMoneyToday = GameManager.ClientMgr.QueryTotaoChongZhiMoneyToday(client);
            totalChongZhiMoneyToday = Global.TransMoneyToYuanBao(totalChongZhiMoneyToday);

            string[] dbFields = null;
            TCPProcessCmdResults retcmd = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO, string.Format("{0}:{1}", roleID, (int)ActivityTypes.TotalConsume), out dbFields, client.ServerId);
            if (null == dbFields)
                return TCPProcessCmdResults.RESULT_FAILED;
            if (dbFields.Length != 3)
                return TCPProcessCmdResults.RESULT_FAILED;
            int totalusedmoney = Global.SafeConvertToInt32(dbFields[2]);

            // Trả về trạng thái nạp đầu,đã nạp hôm nay,tổng đã nạp, tổng đã sử dụng
            string strcmd = string.Format("{0}:{1}:{2}:{3}", totalChongZhiMoney, totalChongZhiMoneyToday, totalChongZhiMoney, totalusedmoney);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        #endregion TCP_NETWORK
    }
}