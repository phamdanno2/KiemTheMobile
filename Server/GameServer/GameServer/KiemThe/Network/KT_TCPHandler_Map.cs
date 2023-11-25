using GameServer.KiemThe.Logic.Manager.Battle;
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
    /// Quản lý chuyển cảnh
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Xử lý gói tin gửi từ Client về Server thông báo đối tượng tải bản đồ thành công
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteEnterMap(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                if (!string.IsNullOrEmpty(cmdData))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Đối tượng gnười chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), client.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Bỏ đánh dấu chờ chuyển Map
                client.WaitingForChangeMap = false;
                client.WaitingForChangePos = false;

                /// Thực hiện hàm OnEnterMap
                client.OnEnterMap();

                /// Gửi gói tin đồng bộ tốc đánh và tốc chạy
                KT_TCPHandler.NotifyTargetMoveSpeedChanged(client);
                KT_TCPHandler.NotifyTargetAttackSpeedChanged(client);

                /// Điểm mặc định
                Point zeroP = new Point(0, 0);
                /// Nếu vị trí hiện tại khác vị trí được thiết lập qua hàm chuyển
                if (client.LastChangedPosition != zeroP && client.CurrentPos != client.LastChangedPosition)
                {
                    /// Ghi Log
                    LogManager.WriteLog(LogTypes.RolePosition, string.Format("Bug change map pos of Client = {0} (ID: {1}), CurrentPos = {2}, LastChangedPos = {3}", client.RoleName, client.RoleID, client.CurrentPos.ToString(), client.LastChangedPosition.ToString()));
                    /// Gắn lại vị trí
                    client.CurrentPos = client.LastChangedPosition;
                    /// Thay đổi vị trí hiện tại của Client
					GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, client.PosX, client.PosY, (int) client.CurrentDir);
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
        /// Xử lý gói tin gửi từ Client về Server thông báo đối tượng chuyển map
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteMapChangeCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            SCMapChange cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<SCMapChange>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                int roleID = cmdData.RoleID;
                int teleportID = cmdData.TeleportID;
                int newMapCode = cmdData.MapCode;
                int toNewMapX = cmdData.PosX;
                int toNewMapY = cmdData.PosY;

                /// Đối tượng gnười chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Dữ liệu trả lại Client
                SCMapChange scData = null;

                /// Bản đồ hiện tại
                GameMap oldMap = GameManager.MapMgr.DictMaps[client.CurrentMapCode];

                /// Di chuyển trực tiếp không qua điểm truyền tống
                if (teleportID < 0)
                {
                    /// Nếu không có đánh dấu đợi chuyển Map
                    if (!client.WaitingForChangeMap)
					{
                        LogManager.WriteLog(LogTypes.Robot, string.Format("{0} (ID: {1}) used Auto, BUG using direct transfer, not waiting for change map => Disconnect!", client.RoleName, client.RoleID));
                        /// Cho toác luôn
                        return TCPProcessCmdResults.RESULT_FAILED;
                    }

                    /// Gắn lại vị trí
                    newMapCode = client.WaitingChangeMapCode;
                    toNewMapX = client.WaitingChangeMapPosX;
                    toNewMapY = client.WaitingChangeMapPosY;

                    //               /// Vị trí đến khác vị trí đang chờ
                    //               if (client.WaitingChangeMapCode != newMapCode || client.WaitingChangeMapPosX != toNewMapX || client.WaitingChangeMapPosY != toNewMapY)
                    //{
                    //                   LogManager.WriteLog(LogTypes.Robot, string.Format("{0} (ID: {1}) used Auto, BUG using direct transfer, Server (ToMapCode: {2}, ToPosX: {3}, ToPosY: {4}), Client (ToMapCode: {5}, ToPosX: {6}, ToPosY: {7}) => Disconnect!", client.RoleName, client.RoleID, client.WaitingChangeMapCode, client.WaitingChangeMapPosX, client.WaitingChangeMapPosY, newMapCode, toNewMapX, toNewMapY));
                    //                   /// Cho toác luôn
                    //                   return TCPProcessCmdResults.RESULT_FAILED;
                    //               }

                    GameMap toGameMap = null;
                    /// Nếu ID bản đồ đích không tồn tại
                    if (!GameManager.MapMgr.DictMaps.TryGetValue(newMapCode, out toGameMap))
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 1,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    int toLevel = toGameMap.MapLevel;
                    /// Nếu cấp độ không đủ vào bản đồ
                    if (client.m_Level < toLevel)
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 2,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu bản đồ hiện tại không phải bản đồ liên máy chủ nhưng bản đồ muốn tới lại là bản đồ liên máy chủ
                    if (!KuaFuMapManager.getInstance().IsKuaFuMap(client.MapCode) && KuaFuMapManager.getInstance().IsKuaFuMap(newMapCode))
                    {
                        /// Lưu lại vị trí trước khi sang liên máy chủ
                        KT_TCPHandler.RecordCopySceneInfoToDB(client, -1, client.CurrentMapCode, (int)client.CurrentPos.X, (int)client.CurrentPos.Y, -1);

                        string[] cmdParams = new string[6];
                        cmdParams[0] = newMapCode + "";
                        cmdParams[1] = 1 + "";
                        cmdParams[2] = -1 + "";
                        cmdParams[3] = teleportID + "";
                        cmdParams[4] = toNewMapX + "";
                        cmdParams[5] = toNewMapY + "";

                        KuaFuMapManager.getInstance().ProcessKuaFuMapEnterCmd(client, (int)(TCPGameServerCmds.CMD_SPR_KUAFU_MAP_ENTER), null, cmdParams);

                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                /// Di chuyển thông qua điểm truyền tống
                else
                {
                    // NẾU CÓ TELEPORT ID
                    //Nếu bản đồ hiện tại không phải bản đồ liên máy chủ nhưng bản đồ muốn tới lại là bản đồ liên máy chủ
                    if (!KuaFuMapManager.getInstance().IsKuaFuMap(client.MapCode) && KuaFuMapManager.getInstance().IsKuaFuMap(newMapCode))
                    {
                        // Lưu lại vị trí trước khi sang liên máy chủ
                        KT_TCPHandler.RecordCopySceneInfoToDB(client, -1, client.CurrentMapCode, (int)client.CurrentPos.X, (int)client.CurrentPos.Y, -1);

                        string[] cmdParams = new string[4];

                        cmdParams[0] = newMapCode + "";
                        cmdParams[1] = 1 + "";
                        cmdParams[2] = -1 + "";
                        cmdParams[3] = teleportID + "";

                        KuaFuMapManager.getInstance().ProcessKuaFuMapEnterCmd(client, (int)(TCPGameServerCmds.CMD_SPR_KUAFU_MAP_ENTER), null, cmdParams);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    GameMap gameMap = null;
                    /// Nếu ID bản đồ đang đứng không tồn tại
                    if (!GameManager.MapMgr.DictMaps.TryGetValue(client.MapCode, out gameMap))
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 0,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    MapTeleport mapTeleport = null;
                    /// Nếu cổng Teleport không tồn tại
                    if (!gameMap.MapTeleportDict.TryGetValue(teleportID, out mapTeleport))
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 3,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu ở quá xa điểm truyền tống
                    if (Global.GetTwoPointDistance(client.CurrentPos, new Point(mapTeleport.X, mapTeleport.Y)) >= mapTeleport.Radius)
					{
                        /// Toác
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    GameMap toGameMap = null;
                    /// Nếu bản đồ đích không tồn tại
                    if (!GameManager.MapMgr.DictMaps.TryGetValue(mapTeleport.ToMapID, out toGameMap))
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 1,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Thực thi sự kiện trước khi chuyển bản đồ
                    client.OnPreChangeMap(toGameMap);

                    int toLevel = toGameMap.MapLevel;
                    /// Nếu cấp độ không đủ vào bản đồ
                    if (client.m_Level < toLevel)
                    {
                        scData = new SCMapChange()
                        {
                            RoleID = client.RoleID,
                            ErrorCode = 2,
                        };
                        client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Nếu là tele trong tống kim
                    if (mapTeleport.Camp == 10 || mapTeleport.Camp == 20)
                    {
                        if (!Battel_SonJin_Manager.CanUsingTeleport(client))
                        {
                            PlayerManager.ShowNotification(client, "Chiến trường chưa bắt đầu");
                            return TCPProcessCmdResults.RESULT_OK;
                        }
						else
						{
                            Battel_SonJin_Manager.UseTeleport(client);
                            return TCPProcessCmdResults.RESULT_OK;
						}
                    }

                    /// Nếu thông tin ở Client gửi lên sai thì cho cút luôn
                    if (newMapCode != mapTeleport.ToMapID || toNewMapX != mapTeleport.ToX || toNewMapY != mapTeleport.ToY)
					{
                        LogManager.WriteLog(LogTypes.Robot, string.Format("{0} (ID: {1}) used Auto, BUG using Teleport => Disconnect!", client.RoleName, client.RoleID));
                        return TCPProcessCmdResults.RESULT_FAILED;
                    }

                    /// Thiết lập lại vị trí tương đương cổng teleport
                    newMapCode = mapTeleport.ToMapID;
                    toNewMapX = mapTeleport.ToX;
                    toNewMapY = mapTeleport.ToY;
                }


                /// Nếu dùng điểm truyền tống dịch chuyển đến vị trí trong cùng bản đồ
                if (teleportID >= 0 && client.MapCode == newMapCode)
                {
                    GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, toNewMapX, toNewMapY, -1);

                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu không thể dịch chuyển đến bản đồ tương ứng
                if (!Global.CanChangeMapCode(client, newMapCode))
                {
                    scData = new SCMapChange()
                    {
                        RoleID = client.RoleID,
                        ErrorCode = 4,
                    };
                    client.SendPacket((int) TCPGameServerCmds.CMD_SPR_MAPCHANGE, scData);

                    return TCPProcessCmdResults.RESULT_OK;
                }

                // Nếu là đang là bản đồ liên máy chủ mà bản đồ muốn tele tới lại ko phỉa liên máy chủ =====> Đây là command quay về
                if (KuaFuMapManager.getInstance().IsKuaFuMap(client.MapCode) && !KuaFuMapManager.getInstance().IsKuaFuMap(newMapCode))
                {
                    // Quay về vị trí trước đó đã sang bản đồ thế giới
                    KuaFuManager.getInstance().GotoLastMap(client);

                    return TCPProcessCmdResults.RESULT_OK;
                }

                /// Nếu không thể chuyển bản đồ
                if (!GameManager.ClientMgr.ChangeMap(tcpMgr.MySocketListener, pool, client, teleportID, newMapCode, toNewMapX, toNewMapY, (int) client.CurrentDir, nID))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Change map faild, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Thiết lập vị trí đích đến
                client.ToPos = new Point(toNewMapX, toNewMapY);

                /// Thực thi sự kiện OnChangeMap
                client.OnChangeMap(oldMap);

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// Xử lý gói tin gửi từ Client về Server yêu cầu cập nhật vị trí quái đặc biệt trong toàn bản đồ
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessGetLocalMapSpecialMonsters(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new ASCIIEncoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Decode data faild, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                if (!string.IsNullOrEmpty(cmdData))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Đối tượng gnười chơi tương ứng
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Can not find player corresponding ID, CMD={0}, Client={1}", (TCPGameServerCmds) nID, Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Nếu chưa đến thời gian
                if (KTGlobal.GetCurrentTimeMilis() - client.LastUpdateLocalMapMonsterTicks < 5000)
                {
                    /// Bỏ qua
                    return TCPProcessCmdResults.RESULT_OK;
                }
                /// Đánh dấu thời gian
                client.LastUpdateLocalMapMonsterTicks = KTGlobal.GetCurrentTimeMilis();

                ///// Kết quả
                //List<LocalMapMonsterData> monsters = new List<LocalMapMonsterData>();

                ///// Tạm thời Diss bỏ
                ///// Duyệt danh sách quái trong bản đồ
                //List<Monster> objs = GameManager.MonsterMgr.GetContinuouslyUpdateToMiniMapMonstersByMap(client.CurrentMapCode, client.CurrentCopyMapID);
                ///// Toác
                //if (objs == null)
                //{
                //    /// Bỏ qua
                //    return TCPProcessCmdResults.RESULT_OK;
                //}
                ///// Duyệt danh sách
                //foreach (Monster obj in objs)
                //{
                //    monsters.Add(new LocalMapMonsterData()
                //    {
                //        Name = string.IsNullOrEmpty(obj.LocalMapName) ? obj.RoleName : obj.LocalMapName,
                //        PosX = (int) obj.CurrentPos.X,
                //        PosY = (int) obj.CurrentPos.Y,
                //        IsBoss = obj.MonsterType == Entities.MonsterAIType.Special_Boss || obj.MonsterType == Entities.MonsterAIType.Boss || obj.MonsterType == Entities.MonsterAIType.Pirate || obj.MonsterType == Entities.MonsterAIType.Elite || obj.MonsterType == Entities.MonsterAIType.Leader,
                //    });
                //}

                ///// Nếu danh sách rỗng
                //if (monsters.Count <= 0)
                //{
                //    /// Bỏ qua
                //    return TCPProcessCmdResults.RESULT_OK;
                //}

                ///// Gửi lại gói tin về Client
                //byte[] byteData = DataHelper.ObjectToBytes<List<LocalMapMonsterData>>(monsters);
                //tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, byteData, nID);
                //return TCPProcessCmdResults.RESULT_DATA;

                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }
    }
}
