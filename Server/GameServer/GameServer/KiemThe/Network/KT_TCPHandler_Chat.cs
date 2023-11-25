using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý Chat
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Nhận gói tin thông báo người chơi thực hiện Chat
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteChatCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            SpriteChat cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SpriteChat>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
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

                /// Kênh chat
                ChatChannel channel = (ChatChannel)cmdData.Channel;
                /// Nội dung chat
                string content = cmdData.Content;
                /// Xóa thẻ HTML
                content = Utils.RemoveAllHTMLTags(content);

                /// Số lần đã lặp
                int nLoop = 0;
                /// Khôi phục các thẻ HTML chứa biểu cảm
                do
                {
                    /// Tăng số lần đã lặp
                    nLoop++;
                    /// Nếu quá 100 lần thì toác
                    if (nLoop >= 100)
                    {
                        break;
                    }

                    Match match = Regex.Match(content, @"#(\d+)");
                    if (match.Groups.Count <= 1 || string.IsNullOrEmpty(match.Groups[0].Value))
                    {
                        break;
                    }
                    try
                    {
                        string emojiID = match.Groups[1].Value;
                        content = content.Replace(match.Groups[0].Value, string.Format("<sprite={0}>", emojiID));
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                while (true);

                /// Số lần đã lặp
                nLoop = 0;
                /// Khôi phục các thẻ ghi vị trí
                do
                {
                    /// Tăng số lần đã lặp
                    nLoop++;
                    /// Nếu quá 10 lần thì toác
                    if (nLoop >= 10)
                    {
                        break;
                    }

                    Match match = Regex.Match(content, @"\@GOTO_(\d+)_(\d+)_(\d+)");
                    if (match.Groups.Count <= 1 || string.IsNullOrEmpty(match.Groups[0].Value))
                    {
                        break;
                    }
                    try
                    {
                        int mapCode = int.Parse(match.Groups[1].Value);
                        int posX = int.Parse(match.Groups[2].Value);
                        int posY = int.Parse(match.Groups[3].Value);

                        /// Bản đồ tương ứng
                        GameMap gameMap = GameManager.MapMgr.GetGameMap(mapCode);
                        /// Nếu toác
                        if (gameMap == null)
                        {
                            content = content.Replace(match.Groups[0].Value, "");
                        }
                        else
                        {
                            string mapName = gameMap.MapName;
                            UnityEngine.Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(gameMap, new UnityEngine.Vector2(posX, posY));
                            int gridPosX = (int)gridPos.x;
                            int gridPosY = (int)gridPos.y;
                            content = content.Replace(match.Groups[0].Value, string.Format("<color=#3dfff9><link=\"GoTo_{0}_{1}_{2}\">[{3} ({4}, {5})]</link></color>", mapCode, posX, posY, mapName, gridPosX, gridPosY));
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                while (true);

                /// Khôi phục thẻ Chat Voice
                {
                    Match match = Regex.Match(content, @"\@VOICE_(\w+)");
                    if (match.Groups.Count > 1 && !string.IsNullOrEmpty(match.Groups[0].Value))
                    {
                        try
                        {
                            string chatID = match.Groups[1].Value;
                            content = content.Replace(match.Groups[0].Value, string.Format("<link=\"VoiceChat_{0}\"><color=#6b6bff>[Tin nhắn thoại]</color> <sprite name=\"3929-1\">.</link>", chatID));
                        }
                        catch (Exception) { }
                    }
                }

                /// Nếu bị cấm Chat
                if (client.BanChat > 0)
                {
                    string strinfo = "<color=red>Xin lỗi, bạn đang bị hạn chế Chat, hãy gửi yêu cầu giải trình lên hỗ trợ để được tư vấn giải thích.</color>";
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Nếu bị cấm Chat
                else if (BanChatManager.IsBanRoleName(Global.FormatRoleName(client, client.RoleName), client.RoleID))
                {
                    string strinfo = "<color=red>Xin lỗi, bạn đang bị hạn chế Chat, hãy gửi yêu cầu giải trình lên hỗ trợ để được tư vấn giải thích.</color>";
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Nếu không thể thực hiện Chat lúc này
                else if (!client.CanChat(channel, out long tickLeft))
                {
                    string strinfo = "<color=red>Bạn hiện không thể gửi tin nhắn, hãy liên lạc với hỗ trợ viên để được xử lý.</color>";
                    if (tickLeft != 99999999)
                    {
                        strinfo = string.Format("<color=red>Hiện không thể gửi tin nhắn ở kênh này, cần chờ sau <color=yellow>{0}</color> nữa mới có thể tiếp tục gửi.</color>", KTGlobal.DisplayTime(tickLeft / 1000f));
                    }
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Nếu không có nội dung Chat
                else if (string.IsNullOrEmpty(content))
                {
                    string strinfo = "<color=red>Không thể gửi tin nhắn khi không có nội dung.</color>";
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Nếu là Chat nhóm nhưng lại không có nhóm
                else if (cmdData.Channel == (int)ChatChannel.Team && (client.TeamID == -1 || !KTTeamManager.IsTeamExist(client.TeamID)))
                {
                    string strinfo = "<color=red>Không có đội ngũ, không thể gửi tin nhắn.</color>";
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu là chat thế giới
                else if (cmdData.Channel == (int)ChatChannel.Global && client.m_Level < 20)
                {
                    string strinfo = "<color=red>Cấp 20 trở lên mới có thể chat thế giới.</color>";
                    GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Nếu là chat kênh đặc biệt
                else if (cmdData.Channel == (int)ChatChannel.Special)
                {
                    ///// Nếu không phải GM
                    //if (!KTGMCommandManager.IsGM(client))
                    //{
                    //    string strinfo = "Chức năng tạm khóa!";
                    //    PlayerManager.ShowNotification(client, strinfo);
                    //    return TCPProcessCmdResults.RESULT_OK;
                    //}

                    /// Kiểm tra xem có vật phẩm Ốc biển truyền thanh (tiểu) không
                    GoodsData speakerItem = client.GoodsDataList.Where(x => x.GCount > 0 && KTGlobal.SpecialChatMaterial.Contains(x.GoodsID)).FirstOrDefault();
                    /// Nếu không có vật phẩm
                    if (speakerItem == null)
                    {
                        string strinfo = "Không có vật phẩm [Ốc biển truyền thanh (tiểu)], không thể Chat ở kênh đặc biệt!";
                        PlayerManager.ShowNotification(client, strinfo);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                    /// TODO thực hiện xóa vật phẩm Ốc biển truyền thanh (tiểu)
                    ItemManager.RemoveItemByCount(client, speakerItem, 1,"CHAT");
                }
                /// Nếu là kênh chat liên máy chủ
                else if (cmdData.Channel == (int)ChatChannel.KuaFuLine)
                {
                    /// Kiểm tra xem có vật phẩm Ốc biển truyền thanh (trung) không
                    GoodsData speakerItem = client.GoodsDataList.Where(x => x.GCount > 0 && KTGlobal.CrossServerChatMaterial.Contains(x.GoodsID)).FirstOrDefault();
                    /// Nếu không có vật phẩm
                    if (speakerItem == null)
                    {
                        string strinfo = "Không có vật phẩm [Ốc biển truyền thanh (trung)], không thể Chat ở kênh đặc biệt!";
                        PlayerManager.ShowNotification(client, strinfo);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                    /// TODO thực hiện xóa vật phẩm Ốc biển truyền thanh (trung)
                    ItemManager.RemoveItemByCount(client, speakerItem, 1,"CHAT");
                }

                /// Lọc nội dung Chat
                content = KTChatFilter.Filter(content);

                /// Nội dung Chat
                StringBuilder contentString = new StringBuilder(content);
                /// Danh sách vật phẩm đính kèm
                List<GoodsData> items = null;
                if (cmdData.Items != null && cmdData.Items.Count > 0)
                {
                    /// Nếu vật phẩm tồn tại trong túi người chơi thì mới cho lấy thông tin
                    items = client.GoodsDataList?.Where(x => cmdData.Items.Any(y => y.Id == x.Id) && x.GCount > 0).ToList();
                    /// Giới hạn Client truyền lên chỉ được 1 vật phẩm duy nhất
                    items = items.Take(1).ToList();

                    /// Nếu tồn tại danh sách đính kèm
                    if (items != null && items.Count > 0)
                    {
                        List<string> itemStrings = new List<string>();
                        foreach (GoodsData itemGD in items)
                        {
                            itemStrings.Add(KTGlobal.GetItemDescInfoStringForChat(itemGD));
                        }
                        contentString.AppendLine();
                        contentString.AppendLine(string.Format("<color=#a8ecff>Vật phẩm đính kèm:</color> {0}", string.Join(", ", itemStrings)));
                    }
                }

                switch (cmdData.Channel)
                {
                    case (int)ChatChannel.Near:
                        {
                            List<KPlayer> playersAround = KTLogic.GetNearByObjectsAtPos<KPlayer>(client.CurrentMapCode, client.CurrentCopyMapID, new UnityEngine.Vector2((int)client.CurrentPos.X, (int)client.CurrentPos.Y), 1000);
                            foreach (KPlayer player in playersAround)
                            {
                                GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, client, player, contentString.ToString(), channel, items);
                            }
                            break;
                        }
                    case (int)ChatChannel.Team:
                        {
                            List<KPlayer> teammates = client.Teammates;
                            foreach (KPlayer player in teammates)
                            {
                                GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, client, player, contentString.ToString(), channel, items);
                            }
                            break;
                        }
                    case (int)ChatChannel.Faction:
                        {
                            KTGlobal.SendFactionChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.m_cPlayerFaction.GetFactionId(), contentString.ToString(), client);
                            break;
                        }
                    case (int)ChatChannel.Guild:
                        {
                            KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.GuildID, contentString.ToString(), client);

                            // Thực hiện gửi sang liên máy chủ
                            KT_TCPHandler.ProseccChatKuaFu(client.RoleID, client.RoleName, 1, cmdData.ToRoleName, cmdData.Channel, contentString.ToString(), client.GuildID, client.ServerId);

                            break;
                        }
                    case (int)ChatChannel.Family:
                        {
                            KTGlobal.SendFamilyChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client.FamilyID, contentString.ToString(), client);

                            // Thực hiện gửi sang liên máy chủ
                            KT_TCPHandler.ProseccChatKuaFu(client.RoleID, client.RoleName, 1, cmdData.ToRoleName, cmdData.Channel, contentString.ToString(), client.FamilyID, client.ServerId);

                            break;
                        }
                    case (int)ChatChannel.Global:
                    case (int)ChatChannel.Special:
                        {
                            int idx = 0;
                            KPlayer player = null;

                            while ((player = GameManager.ClientMgr.GetNextClient(ref idx)) != null)
                            {
                                GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, client, player, contentString.ToString(), channel, items);
                            }

                            // KT_TCPHandler.ProseccChatKuaFu(client.RoleID, client.RoleName, 1, cmdData.ToRoleName, (int)ChatChannel.KuaFuLine, contentString.ToString(), 0, client.ServerId);

                            break;
                        }

                    // Nếu là chát liên máy chủ thì gửi tới cho toàn bộ các sv khác cùng NHÓM
                    case (int)ChatChannel.KuaFuLine:
                        {
                            //int idx = 0;
                            //KPlayer player = null;
                            //// Thực hiện gửi cho toàn bộ máy chủ bên mình trước

                            //while ((player = GameManager.ClientMgr.GetNextClient(ref idx)) != null)
                            //{
                            //    GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, client, player, contentString.ToString(), channel, items);
                            //}
                            string RoleName = Global.FormatRoleNameWithZoneId(client);
                            // Thực hiện gửi packet tới gamedb để gửi sang cho liên máy chủ
                            KT_TCPHandler.ProseccChatKuaFu(client.RoleID, RoleName, 1, cmdData.ToRoleName, cmdData.Channel, contentString.ToString(), 0, client.ServerId);

                            break;
                        }
                    case (int)ChatChannel.Private:
                        {
                            string playerName = cmdData.ToRoleName;
                            int roleID = RoleName2IDs.FindRoleIDByName(playerName);
                            if (roleID == -1)
                            {
                                string strinfo = "<color=red>Người chơi không tồn tại hoặc đã rời mạng, không thể gửi tin nhắn.</color>";
                                GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                                break;
                            }

                            KPlayer player = GameManager.ClientMgr.FindClient(roleID);
                            if (player == null)
                            {
                                string strinfo = "<color=red>Người chơi không tồn tại hoặc đã rời mạng, không thể gửi tin nhắn.</color>";
                                GameManager.ClientMgr.SendDefaultTypeChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, strinfo);
                                break;
                            }

                            GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, client, player, contentString.ToString(), channel, items);
                            GameManager.ClientMgr.SendChatMessage(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, client, player, contentString.ToString(), channel, items);
                            break;
                        }
                }
                /// Lưu thời gian Chat ở kênh tương ứng
                client.RecordChatTick(channel);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Xử lý chat liên máy chủ
        /// Add 26/8/2021
        /// </summary>
        public static void ProseccChatKuaFu(int roleID, string roleName, int status, string toRoleName, int index, string textMsg, int chatType, int ServerID)
        {
            string ChatData = roleID + ":" + roleName + ":" + status + ":" + toRoleName + ":" + index + ":" + DataHelper.EncodeBase64(textMsg) + ":" + chatType;

            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_SPR_CHAT,
                                  string.Format("{0}:{1}:{2}", ChatData, chatType, GameManager.ServerLineIdAllLineExcludeSelf),
                                  null, ServerID);
        }
    }
}