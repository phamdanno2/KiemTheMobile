using GameServer.Core.Executor;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager.Shop;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace GameServer.KiemThe.Core.Rechage
{
    public class RechageServiceManager
    {
        public static BlockingCollection<RechageModel> ReachageData;

        public static List<string> AreadyProsecc = new List<string>();

        public static void SendRequestByThreading(byte[] SendDATA, NPC npc, KPlayer client)
        {
            KNPCDialog _NpcDialog = new KNPCDialog();

            WebClient wwc = new WebClient();

            try
            {
                byte[] VL = wwc.UploadData(GameManager.KTCoinService, SendDATA);

                KTCoinResponse _KTCoinResponse = DataHelper.BytesToObject<KTCoinResponse>(VL, 0, VL.Length);

                if (_KTCoinResponse.Status < 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", _KTCoinResponse.Msg);
                }
                else
                {
                    client.KTCoin = _KTCoinResponse.Value;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, "ACTIVE GIFTCODE BUG :" + ex.ToString());
            }

            Dictionary<int, string> Selections = new Dictionary<int, string>();

            string Text = "Xin chào <b>" + client.RoleName + "</b>" + " bạn hiện có  <b>" + KTGlobal.CreateStringByColor(client.KTCoin + " KTCoin", ColorType.Yellow) + "</b>\n\nVui lòng chọn gói " + KTGlobal.CreateStringByColor("Đồng", ColorType.Green) + " muốn quy đổi dưới đây\nLần mua đầu tiên sẽ nhận khuyến mại " + KTGlobal.CreateStringByColor("Đồng Khóa", ColorType.Green) + " tương ứng với gói " + KTGlobal.CreateStringByColor("Đồng", ColorType.Green) + " quy đổi\n<b><color=red>Bạn phải quy đổi tối thiểu 2000 KTCoin để nhận được quà nạp đầu</color></b>\nSau khi mua hết các gói khuyến mại bạn có thể sử dụng chức năng ĐỔI TÙY CHỌN để nhập số KTCoin muốn đổi với tỉ lệ 1:1";

            int Check1 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack1);
            int Check2 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack2);
            int Check3 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack3);
            int Check4 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack4);
            int Check5 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack5);
            int Check6 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack6);
			int Check7 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack7);
            int Check8 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack8);
            int Check9 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack9);
            int Check10 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack10);
            int Check11 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack11);
            int Check12 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack12);
            /// int Check13 = client.GetValueOfForeverRecore(RecoreDef.IAP_Pack13);
            if (Check1 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack1, "<b><color=#00ff2a>Gói 5000 Đồng</color></b> cần <b>5000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack1, "<b><color=#00ff2a>Gói 5000 Đồng</color></b> cần <b>5000 KTCoin</b>");
            }

            if (Check2 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack2, "<b><color=#00ff2a>Gói 1 vạn Đồng</color></b> cần <b>10000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack2, "<b><color=#00ff2a>Gói 1 vạn Đồng</color></b> cần <b>10000 KTCoin</b>");
            }

            if (Check3 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack3, "<b><color=#00ff2a>Gói 2 vạn Đồng</color></b> cần <b>20000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack3, "<b><color=#00ff2a>Gói 2 vạn Đồng</color></b> cần <b>20000 KTCoin</b>");
            }

            if (Check4 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack4, "<b><color=#00ff2a>Gói 5 vạn Đồng</color></b> cần <b>50000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack4, "<b><color=#00ff2a>Gói 5 vạn Đồng</color></b> cần <b>50000 KTCoin</b>");
            }

            if (Check5 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack5, "<b><color=#00ff2a>Gói 10 vạn Đồng</color></b> cần <b>100000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack5, "<b><color=#00ff2a>Gói 10 vạn Đồng</color></b> cần <b>100000 KTCoin</b>");
            }

            if (Check6 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack6, "<b><color=#00ff2a>Gói 20 vạn Đồng</color></b> cần <b>200000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack6, "<b><color=#00ff2a>Gói 20 vạn Đồng</color></b> cần <b>200000 KTCoin</b>");
            }
			if (Check7 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack7, "<b><color=#00ff2a>Gói 50 vạn Đồng</color></b> cần <b>500000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack7, "<b><color=#00ff2a>Gói 50 vạn Đồng</color></b> cần <b>500000 KTCoin</b>");
            }

            if (Check8 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack8, "<b><color=#00ff2a>Gói 100 vạn Đồng</color></b> cần <b>1000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack8, "<b><color=#00ff2a>Gói 100 vạn Đồng</color></b> cần <b>1000000 KTCoin</b>");
            }

            if (Check9 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack9, "<b><color=#00ff2a>Gói 200 vạn Đồng</color></b> cần <b>2000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack9, "<b><color=#00ff2a>Gói 200 vạn Đồng</color></b> cần <b>2000000 KTCoin</b>");
            }

            if (Check10 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack10, "<b><color=#00ff2a>Gói 500 vạn Đồng</color></b> cần <b>5000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack10, "<b><color=#00ff2a>Gói 500 vạn Đồng</color></b> cần <b>5000000 KTCoin</b>");
            }

            if (Check11 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack11, "<b><color=#00ff2a>Gói 700 vạn Đồng</color></b> cần <b>7000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack11, "<b><color=#00ff2a>Gói 700 vạn Đồng</color></b> cần <b>7000000 KTCoin</b>");
            }

            if (Check12 == -1)
            {
                Selections.Add((int)RecoreDef.IAP_Pack12, "<b><color=#00ff2a>Gói 1000 vạn Đồng</color></b> cần <b>10000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            }
            else
            {
                Selections.Add((int)RecoreDef.IAP_Pack12, "<b><color=#00ff2a>Gói 1000 vạn Đồng</color></b> cần <b>10000000 KTCoin</b>");
            }

            /// if (Check13 == -1)
            /// {
                /// Selections.Add((int)RecoreDef.IAP_Pack13, "<b><color=#00ff2a>Gói 2000 vạn Đồng</color></b> cần <b>20000000 KTCoin</b>" + KTGlobal.CreateStringByColor("(Có Khuyến Mại)", ColorType.Green) + "</b>");
            /// }
            /// else
            /// {
                /// Selections.Add((int)RecoreDef.IAP_Pack13, "<b><color=#00ff2a>Gói 2000 vạn Đồng</color></b> cần <b>20000000 KTCoin</b>");
            /// }

            Selections.Add((int)RecoreDef.ExChangeCostume, "<b><color=#FF6600>ĐỔI TÙY CHỌN(KHÔNG CÓ KHUYẾN MẠI)</color></b>");

            Action<TaskCallBack> ActionWork = (x) => DoActionSelect(client, npc, x);

            _NpcDialog.OnSelect = ActionWork;

            _NpcDialog.Selections = Selections;

            _NpcDialog.Text = Text;

            _NpcDialog.Show(npc, client);
        }

        /// <summary>
        /// Npc quản lý mua gói
        /// </summary>
        /// <param name="map"></param>
        /// <param name="npc"></param>
        /// <param name="client"></param>
        public static void ClickRechage(NPC npc, KPlayer client)
        {
            long Now = TimeUtil.NOW();

            if (Now - client.LastRequestKTCoin > 10000)
            {
                // Set số lần gần đây nhất mới thao tác
                client.LastRequestKTCoin = Now;

                KTCoinRequest _Request = new KTCoinRequest();
                _Request.RoleID = client.RoleID;
                _Request.RoleName = client.RoleName;
                _Request.SeverID = client.ZoneID;
                _Request.Type = 1;
                _Request.UserID = Int32.Parse(client.strUserID.Split('_')[0]);
                _Request.Value = 0;
                byte[] SendDATA = DataHelper.ObjectToBytes<KTCoinRequest>(_Request);

                Thread _Thread = new Thread(() => SendRequestByThreading(SendDATA, npc, client));
                _Thread.Start();
            }
            else
            {
                KNPCDialog _NpcDialog = new KNPCDialog();

                long DIV = Now - client.LastRequestKTCoin;

                float TimeLess = (10000 - DIV) / 1000;

                _NpcDialog.Text = "Vui lòng quay sau :" + TimeLess + " giây nữa";

                _NpcDialog.Show(npc, client);
            }
        }

        private static void DoActionSelect(KPlayer client, NPC npc, TaskCallBack x)
        {
            Console.WriteLine(x);

            RecoreDef SelectPack = (RecoreDef)x.SelectID;

            string PackageName = GetPackeNameByID(SelectPack);

            if (PackageName != "")
            {
                TokenShopStoreProduct _Find = ShopManager.GetProductByID(PackageName);

                if (_Find != null)
                {
                    int KTCoinNeed = _Find.Token;

                    KTCoinRequest _Request = new KTCoinRequest();
                    _Request.RoleID = client.RoleID;
                    _Request.RoleName = client.RoleName;
                    _Request.SeverID = client.ZoneID;
                    _Request.Type = 2;
                    _Request.UserID = Int32.Parse(client.strUserID.Split('_')[0]);
                    _Request.Value = KTCoinNeed;

                    byte[] SendDATA = DataHelper.ObjectToBytes<KTCoinRequest>(_Request);

                    WebClient wwc = new WebClient();

                    try
                    {
                        byte[] VL = wwc.UploadData(GameManager.KTCoinService, SendDATA);

                        KTCoinResponse _KTCoinResponse = DataHelper.BytesToObject<KTCoinResponse>(VL, 0, VL.Length);

                        if (_KTCoinResponse.Status < 0)
                        {
                            KT_TCPHandler.CloseDialog(client);
                            //PlayerManager.ShowMessageBox(client, "Thông báo", _KTCoinResponse.Msg);
                            if (_KTCoinResponse.Status == -1)
                            {
                                byte[] cmdData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", _KTCoinResponse.Status, client.strUserID.Split('_')[1]));
                                client.SendPacket((int)TCPGameServerCmds.CMD_KT_GOI_COIN, cmdData);
                            }
                            else
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", _KTCoinResponse.Msg);
                            } 
                        }
                        else
                        {
                            long TimeBuy = TimeUtil.NOW();

                            string TransID = RechageServiceManager.MakeMD5Hash(TimeBuy + PackageName + _Request.RoleID);

                            string SingCreate = RechageServiceManager.MakeMD5Hash(PackageName + _Request.RoleID + TimeBuy + TransID);

                            RechageModel _RechageModel = new RechageModel();
                            _RechageModel.ActiveCardMonth = false;
                            _RechageModel.PackageName = PackageName;
                            _RechageModel.RoleID = _Request.RoleID;
                            _RechageModel.Sing = SingCreate;
                            _RechageModel.TimeBuy = TimeBuy + "";
                            _RechageModel.TransID = TransID;

                            LogManager.WriteLog(LogTypes.Rechage, "[PackageName] Add yêu cầu mua gói :" + PackageName + "|RoleId :" + _Request.RoleID);

                            RechageServiceManager.ReachageData.Add(_RechageModel);

                            PlayerManager.ShowNotification(client, "Mua gói :" + KTGlobal.CreateStringByColor(_Find.Name, ColorType.Green) + " thành công");
                            KT_TCPHandler.CloseDialog(client);
                        }
                    }
                    catch (Exception ex)
                    {
                        PlayerManager.ShowNotification(client, "Có lỗi khi thực hiện mua gói\nVui lòng liên hệ ADM để báo lỗi");
                        KT_TCPHandler.CloseDialog(client);
                        LogManager.WriteLog(LogTypes.Error, "ACTIVE KTCOIN BUG :" + ex.ToString());
                    }
                }
            }
            else
            {
                Action<int> ActionWork = (number) => DoExChange(client, npc, number);

                PlayerManager.ShowInputNumberBox(client, "Nhập số KTCOIN muốn đổi :", ActionWork);
            }
        }

        /// <summary>
        /// API client gửi packet yêu cầu đổi KTCOIN -> KNB
        /// </summary>
        /// <param name="client"></param>
        /// <param name="PackageName"></param>
        public static void ApiGoiKTCoin(KPlayer client, string PackageName)
        {
            if (GameServer.Logic.GameManager.IsKuaFuServer)
            {
                PlayerManager.ShowMessageBox(client, "Thông báo", "Máy chủ Liên Server không thể thao tác này.");
                return;
            }
            long Now = TimeUtil.NOW();
            if (Now - client.LastRequestKTCoin > 10000)
            {
                // Set số lần gần đây nhất mới thao tác
                client.LastRequestKTCoin = Now;

                if (PackageName != "")
                {
                    TokenShopStoreProduct _Find = ShopManager.GetProductByID(PackageName);
                    if (_Find != null)
                    {
                        int KTCoinNeed = _Find.Token;
                        KTCoinRequest _Request = new KTCoinRequest();
                        _Request.RoleID = client.RoleID;
                        _Request.RoleName = client.RoleName;
                        _Request.SeverID = client.ZoneID;
                        _Request.Type = 2;
                        _Request.UserID = Int32.Parse(client.strUserID.Split('_')[0]);
                        _Request.Value = KTCoinNeed;
                        byte[] SendDATA = DataHelper.ObjectToBytes<KTCoinRequest>(_Request);
                        WebClient wwc = new WebClient();
                        try
                        {
                            byte[] VL = wwc.UploadData(GameManager.KTCoinService, SendDATA);

                            KTCoinResponse _KTCoinResponse = DataHelper.BytesToObject<KTCoinResponse>(VL, 0, VL.Length);

                            if (_KTCoinResponse.Status < 0)
                            {
                                if (_KTCoinResponse.Status == -1)
                                {
                                    //string strcmd = string.Format("{0}:{1}", _KTCoinResponse.Status, 1);
                                    //client.SendPacket((int)TCPGameServerCmds.CMD_KT_GOI_COIN, strcmd);
                                    byte[] cmdData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", _KTCoinResponse.Status, client.strUserID.Split('_')[1]));
                                    client.SendPacket((int)TCPGameServerCmds.CMD_KT_GOI_COIN, cmdData);
                                }
                                else
                                {
                                    PlayerManager.ShowMessageBox(client, "Thông báo", _KTCoinResponse.Msg);
                                }

                            }
                            else
                            {
                                long TimeBuy = TimeUtil.NOW();

                                string TransID = RechageServiceManager.MakeMD5Hash(TimeBuy + PackageName + _Request.RoleID);

                                string SingCreate = RechageServiceManager.MakeMD5Hash(PackageName + _Request.RoleID + TimeBuy + TransID);

                                RechageModel _RechageModel = new RechageModel();
                                _RechageModel.ActiveCardMonth = false;
                                _RechageModel.PackageName = PackageName;
                                _RechageModel.RoleID = _Request.RoleID;
                                _RechageModel.Sing = SingCreate;
                                _RechageModel.TimeBuy = TimeBuy + "";
                                _RechageModel.TransID = TransID;

                                LogManager.WriteLog(LogTypes.Rechage, "[PackageName] Add yêu cầu mua gói :" + PackageName + "|RoleId :" + _Request.RoleID);
                                RechageServiceManager.ReachageData.Add(_RechageModel);
                                PlayerManager.ShowNotification(client, "Mua gói :" + KTGlobal.CreateStringByColor(_Find.Name, ColorType.Green) + " thành công");

                                //string strcmd = string.Format("{0}:{1}", _KTCoinResponse.Status, 0);
                                //client.SendPacket((int)TCPGameServerCmds.CMD_KT_GOI_COIN, strcmd);
                                byte[] cmdData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", _KTCoinResponse.Status, "OK"));
                                client.SendPacket((int)TCPGameServerCmds.CMD_KT_GOI_COIN, cmdData);
                            }
                        }
                        catch (Exception ex)
                        {
                            PlayerManager.ShowNotification(client, "Có lỗi khi thực hiện mua gói\nVui lòng liên hệ ADM để báo lỗi");
                            LogManager.WriteLog(LogTypes.Error, "ACTIVE GIFTCODE BUG :" + ex.ToString());
                        }
                    }
                }
            }
            else
            {
                long DIV = Now - client.LastRequestKTCoin;
                float TimeLess = (10000 - DIV) / 1000;
                PlayerManager.ShowMessageBox(client, "Thông báo", "Thao tác quá nhanh chờ: " + TimeLess + " giây");
            }
        }

        private static void DoExChange(KPlayer client, NPC npc, int KTCoin)
        {
            int KTCoinNeed = KTCoin;

            KTCoinRequest _Request = new KTCoinRequest();
            _Request.RoleID = client.RoleID;
            _Request.RoleName = client.RoleName;
            _Request.SeverID = client.ZoneID;
            _Request.Type = 2;
            _Request.UserID = Int32.Parse(client.strUserID.Split('_')[0]);
            _Request.Value = KTCoinNeed;

            byte[] SendDATA = DataHelper.ObjectToBytes<KTCoinRequest>(_Request);

            WebClient wwc = new WebClient();

            try
            {
                byte[] VL = wwc.UploadData(GameManager.KTCoinService, SendDATA);

                KTCoinResponse _KTCoinResponse = DataHelper.BytesToObject<KTCoinResponse>(VL, 0, VL.Length);

                if (_KTCoinResponse.Status < 0)
                {
                    KT_TCPHandler.CloseDialog(client);
                    PlayerManager.ShowMessageBox(client, "Thông báo", _KTCoinResponse.Msg);
                }
                else
                {
                    long TimeBuy = TimeUtil.NOW();

                    KPlayer _FindPlayer = GameManager.ClientMgr.FindClient(_Request.RoleID);
                    if (_FindPlayer != null)
                    {
                        KTGlobal.AddMoney(_FindPlayer, KTCoinNeed, Entities.MoneyType.Dong, "RECHAGE|" + KTCoinNeed);

                        PlayerManager.ShowNotification(_FindPlayer, KTGlobal.CreateStringByColor("Quy đổi thành công! Nhận được [" + KTCoinNeed + "] đồng", ColorType.Green));

                        string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_G2C_RECHAGE, _FindPlayer.GetRoleData().userMiniData.UserId + ":" + KTCoinNeed + ":" + _Request.RoleID, GameManager.LocalServerId);
                        if (null == dbFields || dbFields.Length != 1 || dbFields[0] != "0")
                        {
                            LogManager.WriteLog(LogTypes.Rechage, "[COSTUMEXCHANGE][" + _Request.RoleID + "] Ghi lại tích lũy bị lỗi:" + KTCoinNeed);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Rechage, "[COSTUMEXCHANGE][" + _Request.RoleID + "] Ghi lại tích thành công:" + KTCoinNeed);
                        }
                    }
                    else
                    {
                        LogManager.WriteLog(LogTypes.Rechage, "[COSTUMEXCHANGE][" + _Request.RoleID + "] Người chơi không online không thể xử lý gói mua");
                        return;
                    }

                    KT_TCPHandler.CloseDialog(client);
                }
            }
            catch (Exception ex)
            {
                PlayerManager.ShowNotification(client, "Có lỗi khi thực hiện mua gói\nVui lòng liên hệ ADM để báo lỗi");
                KT_TCPHandler.CloseDialog(client);
                LogManager.WriteLog(LogTypes.Error, "ACTIVE KTCOIN BUG :" + ex.ToString());
            }
        }

        /// <summary>
        /// Start Service check nạp
        /// </summary>
        public static void StartService()
        {
            ScheduleExecutor2.Instance.scheduleExecute(new NormalScheduleTask("RECHAGETICK", ProseccRechage), 5 * 1000, 1000);

            ReachageData = new BlockingCollection<RechageModel>();
        }

        public static string GetPackeNameByID(RecoreDef InputPack)
        {
            switch (InputPack)
            {
                case RecoreDef.IAP_Pack1:
                    return "com.ktfog.mobi.pack1";

                case RecoreDef.IAP_Pack2:
                    return "com.ktfog.mobi.pack2";

                case RecoreDef.IAP_Pack3:
                    return "com.ktfog.mobi.pack3";

                case RecoreDef.IAP_Pack4:
                    return "com.ktfog.mobi.pack4";

                case RecoreDef.IAP_Pack5:
                    return "com.ktfog.mobi.pack5";

                case RecoreDef.IAP_Pack6:
                    return "com.ktfog.mobi.pack6";
					
                case RecoreDef.IAP_Pack7:
                    return "com.ktfog.mobi.pack7";

                case RecoreDef.IAP_Pack8:
                    return "com.ktfog.mobi.pack8";

                case RecoreDef.IAP_Pack9:
                    return "com.ktfog.mobi.pack9";

                case RecoreDef.IAP_Pack10:
                    return "com.ktfog.mobi.pack10";

                case RecoreDef.IAP_Pack11:
                    return "com.ktfog.mobi.pack11";

                case RecoreDef.IAP_Pack12:
                    return "com.ktfog.mobi.pack12";

                /// case RecoreDef.IAP_Pack13:
                    /// return "com.ktfog.mobi.pack13";
                      
			}

            return "";
        }

        /// <summary>
        /// Tìm gói mua
        /// </summary>
        /// <param name="PackageName"></param>
        /// <returns></returns>
        public static RecoreDef FindRecoreByPackage(string PackageName)
        {
            switch (PackageName)
            {
                case "com.ktfog.mobi.pack1":
                    return RecoreDef.IAP_Pack1;

                case "com.ktfog.mobi.pack2":
                    return RecoreDef.IAP_Pack2;

                case "com.ktfog.mobi.pack3":
                    return RecoreDef.IAP_Pack3;

                case "com.ktfog.mobi.pack4":
                    return RecoreDef.IAP_Pack4;

                case "com.ktfog.mobi.pack5":
                    return RecoreDef.IAP_Pack5;

                case "com.ktfog.mobi.pack6":
                    return RecoreDef.IAP_Pack6;

                case "com.ktfog.mobi.pack7":
                    return RecoreDef.IAP_Pack7;

                case "com.ktfog.mobi.pack8":
                    return RecoreDef.IAP_Pack8;

                case "com.ktfog.mobi.pack9":
                    return RecoreDef.IAP_Pack9;

                case "com.ktfog.mobi.pack10":
                    return RecoreDef.IAP_Pack10;

                case "com.ktfog.mobi.pack11":
                    return RecoreDef.IAP_Pack11;

                case "com.ktfog.mobi.pack12":
                    return RecoreDef.IAP_Pack12;

                /// case "com.ktfog.mobi.pack13":
                    /// return RecoreDef.IAP_Pack13; 
 }

            return RecoreDef.NULL;
        }

        /// <summary>
        /// Convert to MD5 Hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MakeMD5Hash(string input)
        {
            string result;
            using (MD5 md = MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                byte[] array = md.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("X2"));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        /// <summary>
        /// Prosecc change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ProseccRechage(object sender, EventArgs e)
        {
            try
            {
                if (ReachageData.Count > 0)
                {
                    var _Request = ReachageData.Take();

                    if (_Request != null)
                    {
                        string TYPE = "RECHAGE";

                        if (_Request.ActiveCardMonth)
                        {
                            TYPE = "ACTIVECARD";
                        }

                        LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Xử lý yêu cầu mua gói :" + _Request.PackageName);

                        string SingCreate = RechageServiceManager.MakeMD5Hash(_Request.PackageName + _Request.RoleID + _Request.TimeBuy + _Request.TransID);

                        if (SingCreate != _Request.Sing)
                        {
                            LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Sai sing gửi sang không xử lý :" + _Request.PackageName);
                            return;
                        }
                        else
                        {
                            if (AreadyProsecc.Contains(_Request.TransID))
                            {
                                LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Giao dịch đã xử lý rồi không xửu lý lại :" + _Request.PackageName);
                                return;
                            }
                            else
                            {
                                AreadyProsecc.Add(_Request.TransID);

                                if (!_Request.ActiveCardMonth)
                                {
                                    TokenShopStoreProduct _Find = ShopManager.GetProductByID(_Request.PackageName);
                                    if (_Find != null)
                                    {
                                        int TokenAdd = _Find.Token;
                                        int TOkenBound = _Find.FirstBonus;

                                        KPlayer _FindPlayer = GameManager.ClientMgr.FindClient(_Request.RoleID);
                                        if (_FindPlayer != null)
                                        {
                                            KTGlobal.AddMoney(_FindPlayer, _Find.Token, Entities.MoneyType.Dong, "RECHAGE|" + _Request.PackageName);

                                            PlayerManager.ShowNotification(_FindPlayer, KTGlobal.CreateStringByColor("Nạp gói thành công! Nhận được [" + _Find.Token + "] đồng", ColorType.Green));

                                            RecoreDef _FindRecorey = RechageServiceManager.FindRecoreByPackage(_Request.PackageName);
                                            if (_FindRecorey != RecoreDef.NULL)
                                            {
                                                int Check = _FindPlayer.GetValueOfForeverRecore(_FindRecorey);

                                                if (Check == -1)
                                                {
                                                    KTGlobal.AddMoney(_FindPlayer, _Find.FirstBonus, Entities.MoneyType.DongKhoa, "RECHAGEBOUND|" + _Request.PackageName);
                                                    // set vào là nó được thưởng nạp lần đầu
                                                    _FindPlayer.SetValueOfForeverRecore(_FindRecorey, 1);

                                                    PlayerManager.ShowNotification(_FindPlayer, KTGlobal.CreateStringByColor("Nhận thưởng nạp lần đầu gói  [" + _Find.FirstBonus + "] đồng", ColorType.Green));
                                                    // THỰC HIỆN ADD J ĐÓ
                                                }
                                            }
                                            else
                                            {
                                                LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Không tìm thấy RECORE FOREVER:" + _Request.PackageName + "| TransID :" + _Request.TransID);
                                                return;
                                            }

                                            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_G2C_RECHAGE, _FindPlayer.GetRoleData().userMiniData.UserId + ":" + TokenAdd + ":" + _Request.RoleID, GameManager.LocalServerId);
                                            if (null == dbFields || dbFields.Length != 1 || dbFields[0] != "0")
                                            {
                                                LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Ghi lại tích lũy bị lỗi:" + _Request.PackageName + "| TransID :" + _Request.TransID);
                                            }
                                            else
                                            {
                                                LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Ghi lại tích thành công:" + _Request.PackageName + "| TransID :" + _Request.TransID);
                                            }
                                        }
                                        else
                                        {
                                            LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Người chơi không online không thể xử lý gói mua :" + _Request.PackageName + "| TransID :" + _Request.TransID);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        LogManager.WriteLog(LogTypes.Rechage, "[" + TYPE + "][" + _Request.RoleID + "] Không tìm thấy gói mua này không xử lý :" + _Request.PackageName);
                                        return;
                                    }
                                }
                                else
                                {
                                    //TODO Thực hiện kích hoạt thẻ tháng ở đây
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Rechage, "Ex :" + ex.ToString());
                return;
            }
        }
    }
}