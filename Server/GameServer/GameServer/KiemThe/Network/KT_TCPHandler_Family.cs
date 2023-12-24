using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Logic.Name;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Text;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý gói tin
    /// </summary>

    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Tạo 1 gia tộc
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_CREATE(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.m_cPlayerFaction.GetFactionId() == 0)
                {
                    PlayerManager.ShowNotification(client, "Phải vào phái mới có thể tạo lập gia tộc");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string name = fields[1];

                if (!Utils.CheckValidString(name))
                {
                    PlayerManager.ShowNotification(client, "Tên gia tộc không được chứa ký tự đặc biệt");
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-100", nID);

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (!NameManager.Instance().IsNameLengthOK(name))
                {
                    PlayerManager.ShowNotification(client, "Tên gia tộc phải từ 6 tới 18 ký tự");
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-100", nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// -100 Uy Danh không đủ
                //if (client.Prestige < 20)
                //{
                //    PlayerManager.ShowNotification(client, "Uy danh không đủ");
                //    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-100", nID);
                //    return TCPProcessCmdResults.RESULT_DATA;
                //}

                if (!KTGlobal.IsHaveMoney(client, 1, Entities.MoneyType.Bac))//100000
                {
                    PlayerManager.ShowNotification(client, "Bạc trên người không đủ");
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-500", nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                if (client.FamilyID > 0)
                {
                    PlayerManager.ShowNotification(client, "Bạn đã vào 1 gia tộc khác");

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-300", nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                //-200 Cấp độ không đủ
                if (client.m_Level < 30)
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-200", nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);
                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        int VALUE = Int32.Parse(strCmdResult);

                        if (VALUE > 0)
                        {
                            KTGlobal.SubMoney(client, 1, Entities.MoneyType.Bac, "CREATE FAMILY");



                            client.FamilyID = VALUE;
                            client.FamilyName = fields[1];
                            client.FamilyRank = (int)GameServer.KiemThe.Entities.FamilyRank.Master;

                            KT_TCPHandler.NotifyOthersMyTitleChanged(client);

                            KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);

                            PlayerManager.ShowMessageBox(client, "Thông báo", "Tạo gia tộc thành công");

                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        else
                        {
                            if (VALUE == -1)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Tên gia tộc đã tồn tại");
                            }
                            else if (VALUE == -2)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang bận vui lòng quay lại sau");
                            }
                            else if (VALUE == -3)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn đã vào 1 gia tộc khác");
                            }
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "'", nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Yêu cầu tham gia gia tộc
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_REQUESTJOIN(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int ROLEID = Int32.Parse(fields[0]);

                int FAMILYID = Int32.Parse(fields[1]);

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);

                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        int VALUE = Int32.Parse(strCmdResult);

                        if (VALUE > 0)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Gửi đơn xin tham gia vào gia tộc thành công");

                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, 0 + "", nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        else
                        {
                            if (VALUE == -5)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Gia tộc đã đủ thành viên không thể nhận thêm");

                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }
                            else if (VALUE == -1)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn đã gửi đơn xin gia nhập gia tộc này rồi");

                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);
                                return TCPProcessCmdResults.RESULT_DATA;
                            }
                            else if (VALUE == -3 || VALUE == -4)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang tạm bảo trì vui lòng quay lại sau");

                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);
                                return TCPProcessCmdResults.RESULT_DATA;
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

        /// <summary>
        /// Kick 1 thành viên bang hội
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_KICKMEMBER(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                // nếu không có gia tộc thì chim cút
                if (client.FamilyID <= 0)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                //  Nếu là tộc trưởng hoặc phó tộc thì mới có quyền kíck người
                if (client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.Master || client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.ViceMaster)
                {
                    int ROLEID = Int32.Parse(fields[0]);

                    int FAMILYID = Int32.Parse(fields[1]);

                    if (client.RoleID == ROLEID)
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn không thể khai trừ chính bạn,Nếu muốn giải tán gia tộc vui lòng tới NPC Hoàng Thường");
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    // Tìm thằng muốn kick
                    KPlayer player = GameManager.ClientMgr.FindClient(ROLEID);

                    if (player != null)
                    {
                        if (player.FamilyRank == (int)FamilyRank.Master)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn không thể trục xuất được tộc trưởng");
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }

                    // Gửi yêu cầu kick người chơi vào DATABASE SERVER
                    TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);

                    if (null != tcpOutPacket)
                    {
                        string strCmdResult = null;
                        tcpOutPacket.GetPacketCmdData(out strCmdResult);
                        if (null != strCmdResult)
                        {
                            int VALUE = Int32.Parse(strCmdResult);

                            if (VALUE > 0)
                            {
                                if (player != null)
                                {
                                    // SET LẠI DANH HIỆU CHO THẰNG NÀY

                                    player.FamilyID = 0;
                                    player.FamilyName = "";
                                    player.FamilyRank = 0;




                                    player.GuildID = 0;
                                    player.GuildName = "";
                                    player.GuildRank = 0;

                                    KT_TCPHandler.NotifyOthersMyTitleChanged(player);

                                    KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(player);
                                }

                                PlayerManager.ShowMessageBox(client, "Thông báo", "Kick người chơi thành công");

                                client.SendPacket(nID, "1:" + ROLEID);

                                // Nếu nhận được số 0 thì sẽ remove thằng này khỏi danh sách gia tộc
                                //  tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0:" + ROLEID, nID);
                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else
                            {
                                PlayerManager.ShowMessageBox(client, "Thông Báo", "Có lỗi khi kick thành viên");

                                //tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "-1:" + VALUE, nID);
                                return TCPProcessCmdResults.RESULT_OK;
                            }
                        }
                    }
                    else
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi xảy ra vui lòng thử lại sau");

                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    return TCPProcessCmdResults.RESULT_OK;
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Lấy ra danh sách gia tộc đang có ở máy chủ
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_GETLISTFAMILY(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                // Gửi vào DATABASE để xử lý
                return Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// packet giải tán gia tộc
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_DESTROY(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int FamilyID = Int32.Parse(fields[0]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.FamilyID <= 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông Báo", "Bạn không có trong gia tộc nào");

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // thằng này ko phải gia tộc này nên ko có quyền giải tán gia tộc
                if (client.FamilyID != FamilyID)
                {
                    PlayerManager.ShowMessageBox(client, "Thông Báo", "Gia tộc không phải của bạn");

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // nếu không phải bang chủ thì ko có quyền giải tán bang
                if (client.FamilyRank != (int)GameServer.KiemThe.Entities.FamilyRank.Master)
                {
                    PlayerManager.ShowMessageBox(client, "Thông Báo", "Chỉ có tộc trưởng mới có quyền giải tán gia tộc");

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);

                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        int VALUE = Int32.Parse(strCmdResult);

                        if (VALUE > 0)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Giải tán gia tộc thành công");

                            KPlayer gc = null;

                            int index = 0;

                            while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
                            {
                                if (gc.FamilyID == client.FamilyID)
                                {
                                    gc.FamilyID = 0;
                                    gc.FamilyName = "";
                                    gc.FamilyRank = (int)FamilyRank.Member;


                                    gc.GuildID = 0;
                                    gc.GuildName = "";
                                    gc.GuildRank = 0;

                                    KT_TCPHandler.NotifyOthersMyTitleChanged(gc);

                                    KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);
                                }
                            }

                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);

                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                        else if (VALUE < 0)
                        {
                            if (VALUE == -1)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang bảo trì");
                            }
                            else if (VALUE == -2)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Gia tộc không tồn tại");
                            }

                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);

                            return TCPProcessCmdResults.RESULT_DATA;
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

        /// <summary>
        /// Thay đổi notify bang hội
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_CHANGENOTIFY(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";

            try
            {
                ChangeSlogenFamily changeSlogan = null;
                try
                {
                    /// Giải mã gói tin đẩy về
                    changeSlogan = DataHelper.BytesToObject<ChangeSlogenFamily>(data, 0, count);
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                bool CanChange = false;
                // nếu không phải bang chủ thì không có quyền đổi chiêu mộ
                if (client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.Master || client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.ViceMaster)
                {
                    CanChange = true;
                }

                if (!CanChange)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);

                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        int VALUE = Int32.Parse(strCmdResult);

                        if (VALUE > 0)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Đổi thông báo thành công");
                        }
                        else if (VALUE == -1)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Thông báo chỉ được phép < 200 ký tự");
                        }
                        else
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang bảo trì vui lòng thử lại sau");
                        }

                        /// Đẩy lại dữ liệu về cho Client
                        client.SendPacket<ChangeSlogenFamily>(nID, changeSlogan);

                        return TCPProcessCmdResults.RESULT_OK;
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

        /// <summary>
        /// Thay đổi thông báo yêu cầu khi tham gia gia tộc
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                bool CanChange = false;
                // nếu không phải bang chủ thì không có quyền đổi chiêu mộ
                if (client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.Master || client.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.ViceMaster)
                {
                    CanChange = true;
                }

                if (!CanChange)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo ", "Chỉ có tộc trưởng hoặc tộc phó mới có thể thay đổi thông báo gia tộc");
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);

                if (null != tcpOutPacket)
                {
                    string strCmdResult = null;
                    tcpOutPacket.GetPacketCmdData(out strCmdResult);
                    if (null != strCmdResult)
                    {
                        int VALUE = Int32.Parse(strCmdResult);

                        if (VALUE > 0)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Đổi thông báo thành công");
                        }
                        else if (VALUE == -1)
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Thông báo chỉ được phép < 200 ký tự");
                        }
                        else
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang bảo trì vui lòng thử lại sau");
                        }

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, VALUE + "", nID);

                        return TCPProcessCmdResults.RESULT_DATA;
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

        /// <summary>
        /// Thay đổi cấp bậc của 1 người chơi khác
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_CHANGE_RANK(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.FamilyID <= 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Phải là tộc trưởng mới có thể thay đổi chức vụ cho thành viên");

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (client.FamilyRank != (int)FamilyRank.Master)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Phải là tộc trưởng mới có thể thay đổi chức vụ cho thành viên");

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int RolID = Int32.Parse(fields[0]);

                int Rank = Int32.Parse(fields[1]);

                KPlayer findrole = GameManager.ClientMgr.FindClient(RolID);

                if (findrole != null)
                {
                    if (findrole.RoleID == client.RoleID)
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn không thể thay đổi chức vụ cho chính bạn");

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    if (findrole.FamilyID != client.FamilyID)
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Người kế nhiệm phải cùng 1 gia tộc");

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    // Nếu là nhường chức cho thằng này
                    if (Rank == (int)FamilyRank.Master)
                    {
                        if (findrole.FamilyRank == (int)FamilyRank.ViceMaster)
                        {
                            // Thực hiện hạ chức tộc trưởng

                            string CMDBUILD = client.RoleID + ":" + client.FamilyID + ":" + (int)FamilyRank.Member + ":" + -1;

                            byte[] ByteSendToDB = Encoding.ASCII.GetBytes(CMDBUILD);

                            TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, ByteSendToDB, count, out tcpOutPacket, client.ServerId);

                            if (null != tcpOutPacket)
                            {
                                string strCmdResult = null;
                                tcpOutPacket.GetPacketCmdData(out strCmdResult);
                                if (null != strCmdResult)
                                {
                                    string[] Pram = strCmdResult.Split(':');

                                    int Status = Int32.Parse(Pram[0]);

                                    if (Status > 0)
                                    {
                                        string CMDBUILD2 = findrole.RoleID + ":" + findrole.FamilyID + ":" + (int)FamilyRank.Master + ":" + client.RoleID;

                                        byte[] ByteSendToDB2 = Encoding.ASCII.GetBytes(CMDBUILD2);

                                        //SEND TIẾP PHÁT NỮA CHUYỂN THẰNG KAI LÊN BANG CHỦ
                                        result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, ByteSendToDB2, count, out TCPOutPacket tcpOutPacket2, client.ServerId);

                                        string strCmdResult2 = null;

                                        tcpOutPacket2.GetPacketCmdData(out strCmdResult2);

                                        if (strCmdResult2 != null)
                                        {
                                            string[] Pram2 = strCmdResult2.Split(':');

                                            Status = Int32.Parse(Pram2[0]);

                                            if (Status > 0)
                                            {
                                                // THỰC HIỆN SET RANK LẦN CUỐI
                                                client.FamilyRank = (int)FamilyRank.Member;

                                                findrole.FamilyRank = (int)FamilyRank.Master;


                                                // Nếu thằng này không những làm tộc trưởng còn làm cả bang chủ
                                                if(client.GuildRank == (int)GuildRank.Master)
                                                {


                                                    client.GuildRank = (int)GuildRank.Member;
                                                    findrole.GuildRank = (int)GuildRank.Master;
                                                    // thì phế chức bang chủ luôn



                                                    // Gửi packet set lại rank bang cho thằng A
                                                    string responseData = string.Format("{0}", client.GuildRank);
                                                    client.SendPacket((int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK, responseData);

                                                    // Gửi packet set lại rank bang cho thằng B
                                                    responseData = string.Format("{0}", findrole.GuildRank);
                                                    findrole.SendPacket((int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK, responseData);


                                                }

                                                KT_TCPHandler.NotifyOthersMyTitleChanged(client);

                                                KT_TCPHandler.NotifyOthersMyTitleChanged(findrole);

                                                KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);

                                                KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(findrole);

                                                PlayerManager.ShowMessageBox(client, "Thông báo", "Chuyển giao chức vụ thành công");

                                                client.SendPacket(nID, string.Format("{0}:{1}", client.RoleID, client.FamilyRank));
                                                client.SendPacket(nID, string.Format("{0}:{1}", findrole.RoleID, findrole.FamilyRank));

                                                return TCPProcessCmdResults.RESULT_OK;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi trong quá tình chuyển giao");
                                        return TCPProcessCmdResults.RESULT_OK;
                                    }
                                }
                            }
                        }
                        else
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn chỉ có thể truyền chức lại cho tộc phó");

                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        string CMDBUILD2 = findrole.RoleID + ":" + findrole.FamilyID + ":" + Rank + ":" + -1;

                        byte[] ByteSendToDB2 = Encoding.ASCII.GetBytes(CMDBUILD2);

                        //SEND TIẾP PHÁT NỮA CHUYỂN THẰNG KAI LÊN BANG CHỦ
                        TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, ByteSendToDB2, count, out TCPOutPacket tcpOutPacket2, client.ServerId);

                        string strCmdResult2 = null;

                        tcpOutPacket2.GetPacketCmdData(out strCmdResult2);

                        if (strCmdResult2 != null)
                        {
                            string[] Pram2 = strCmdResult2.Split(':');

                            int Status = Int32.Parse(Pram2[0]);

                            if (Status > 0)
                            {
                                findrole.FamilyRank = Rank;

                                KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(findrole);
                                KT_TCPHandler.NotifyOthersMyTitleChanged(findrole);

                                client.SendPacket(nID, string.Format("{0}:{1}", findrole.RoleID, findrole.FamilyRank));

                                PlayerManager.ShowMessageBox(client, "Thông báo", "Thay đổi chức vụ thành công");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else if (Status == -2)
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Mỗi tộc chỉ có 1 phó tộc");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else
                            {
                                PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi khi thay đổi chức vụ của thành viên");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                        }
                        else
                        {
                            PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi khi thay đổi chức vụ của thành viên");

                            return TCPProcessCmdResults.RESULT_OK;
                        }
                    }
                }
                else
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Người chơi chỉ định không online");
                }

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Trả lời yêu cầu tham gia của người chơi
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_RESPONSE_REQUEST(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int RoleID = Int32.Parse(fields[0]);

                int Accpect = Int32.Parse(fields[1]);

                int FamilyID = Int32.Parse(fields[2]);

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.FamilyID <= 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn không có gia tộc");

                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (client.FamilyRank == (int)FamilyRank.Master || client.FamilyRank == (int)FamilyRank.ViceMaster)
                {
                    TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out TCPOutPacket tcpOutPacket2, client.ServerId);

                    string strCmdResult2 = null;

                    tcpOutPacket2.GetPacketCmdData(out strCmdResult2);

                    if (strCmdResult2 != null)
                    {
                        string[] Pram2 = strCmdResult2.Split(':');

                        int Status = Int32.Parse(Pram2[0]);

                        int RoleIDNeedDelete = Int32.Parse(Pram2[1]);

                        string PRAMEXT = Status + ":" + RoleIDNeedDelete;

                        if (Status > 0)
                        {
                            if (Status == 100)
                            {
                                PlayerManager.ShowNotification(client, "Từ chối gia nhập gia tộc thành công");

                                client.SendPacket(nID, PRAMEXT);

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else if (Status == 200)
                            {
                                PlayerManager.ShowNotification(client, "Đồng ý cho thành viên gia nhập gia tộc");
                                client.SendPacket(nID, strCmdResult2);

                                KPlayer FindClient = GameManager.ClientMgr.FindClient(RoleID);

                                if (FindClient != null)
                                {
                                    FindClient.FamilyID = client.FamilyID;
                                    FindClient.FamilyName = client.FamilyName;
                                    FindClient.FamilyRank = (int)FamilyRank.Member;

                                    FindClient.GuildID = client.GuildID;
                                    FindClient.GuildName = client.GuildName;
                                    FindClient.GuildRank = (int)GuildRank.Member;

                                    KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(FindClient);
                                    KT_TCPHandler.NotifyOthersMyTitleChanged(FindClient);
                                }

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else if (Status == -200)
                            {
                                PlayerManager.ShowNotification(client, "Người chơi đã có gia tộc rồi");

                                client.SendPacket(nID, PRAMEXT);

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else if (Status == -300)
                            {
                                client.SendPacket(nID, PRAMEXT);

                                PlayerManager.ShowNotification(client, "Thành viên đã đầy không thể nhận thêm người vào tộc");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else if (Status == -1)
                            {
                                PlayerManager.ShowNotification(client, "Gia tộc không còn tồn tại");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                            else
                            {
                                PlayerManager.ShowNotification(client, "Có lõi xảy ra khi tham gia gia tộc");

                                return TCPProcessCmdResults.RESULT_OK;
                            }
                        }
                    }
                    else
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi khi thay đổi chức vụ của thành viên");

                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Chỉ có tộc trưởng hoặc tộc phó mới có quyền duyệt thành viên");

                    return TCPProcessCmdResults.RESULT_OK;
                }

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Tự thoát khỏi gia tộc
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
        public static TCPProcessCmdResults CMD_KT_FAMILY_QUIT(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.FamilyID <= 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Bạn không có gia tộc");

                    return TCPProcessCmdResults.RESULT_OK;
                }

                TCPProcessCmdResults result = Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out TCPOutPacket tcpOutPacket2, client.ServerId);

                string strCmdResult2 = null;

                tcpOutPacket2.GetPacketCmdData(out strCmdResult2);

                if (strCmdResult2 != null)
                {
                    string[] Pram2 = strCmdResult2.Split(':');

                    int Status = Int32.Parse(Pram2[0]);

                    if (Status > 0)
                    {
                        client.FamilyID = 0;
                        client.FamilyName = "";
                        client.FamilyRank = 0;


                        client.GuildID = 0;
                        client.GuildName = "";
                        client.GuildRank = 0;

                        KT_TCPHandler.NotifyOthersMyTitleChanged(client);

                        KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);

                        PlayerManager.ShowNotification(client, "Thoát khỏi gia tộc thành công");

                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    else
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi khi thoát khỏi gia tộc");

                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Có lỗi khi thoát khỏi gia tộc");

                    return TCPProcessCmdResults.RESULT_OK;
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults CMD_KT_FAMILY_OPEN(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = "";
            try
            {
                /// Giải mã gói tin đẩy về dạng string
                cmdData = new ASCIIEncoding().GetString(data);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error while getting DATA, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);

                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                if (client.FamilyID <= 0)
                {
                    PlayerManager.ShowNotification(client, "Bạn không có gia tộc");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                // Gửi vào DATABASE để xử lý
                return Global.TransferRequestToDBServer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket, client.ServerId);
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Giải tán Gia tộc
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <param name="RoleID"></param>
        public static void DestroyFamily(int RoleID, int FamilyID)
        {
            KPlayer client = GameManager.ClientMgr.FindClient(RoleID);

            if (null == client)
            {
                return;
            }

            if (client.FamilyID <= 0)
            {
                PlayerManager.ShowMessageBox(client, "Thông Báo", "Bạn không có trong gia tộc nào");

                return;
            }

            // thằng này ko phải gia tộc này nên ko có quyền giải tán gia tộc
            if (client.FamilyID != FamilyID)
            {
                PlayerManager.ShowMessageBox(client, "Thông Báo", "Gia tộc không phải của bạn");

                return;
            }

            // nếu không phải bang chủ thì ko có quyền giải tán bang
            if (client.FamilyRank != (int)GameServer.KiemThe.Entities.FamilyRank.Master)
            {
                PlayerManager.ShowMessageBox(client, "Thông Báo", "Chỉ có tộc trưởng mới có quyền giải tán gia tộc");
            }

            string CMD = FamilyID + "";
            // HÀM EXECUTE TỪ DB RA
            string[] result = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_FAMILY_DESTROY, CMD, client.ServerId);

            if (null != result)
            {
                int VALUE = Int32.Parse(result[0]);

                if (VALUE > 0)
                {
                    PlayerManager.ShowMessageBox(client, "Thông báo", "Giải tán gia tộc thành công");

                    KPlayer gc = null;

                    int index = 0;

                    while ((gc = GameManager.ClientMgr.GetNextClient(ref index)) != null)
                    {
                        if (gc.FamilyID == client.FamilyID)
                        {
                            gc.FamilyID = 0;
                            gc.FamilyName = "";
                            gc.FamilyRank = (int)FamilyRank.Member;


                            gc.GuildID = 0;
                            gc.GuildName = "";
                            gc.GuildRank = (int)GuildRank.Member;


                            KT_TCPHandler.NotifyOthersMyTitleChanged(gc);

                            KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(gc);
                        }
                    }

                    return;
                }
                else if (VALUE < 0)
                {
                    if (VALUE == -1)
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Hệ thống đang bảo trì");
                    }
                    else if (VALUE == -2)
                    {
                        PlayerManager.ShowMessageBox(client, "Thông báo", "Gia tộc không tồn tại");
                    }

                    return;
                }
            }
        }
    }
}