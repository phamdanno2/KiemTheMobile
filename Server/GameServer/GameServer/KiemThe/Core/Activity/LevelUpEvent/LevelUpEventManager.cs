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

namespace GameServer.KiemThe.Core.Activity.LevelUpEvent
{
    public class LevelUpEventManager
    {
        public static LevelUpGiftConfig _Event = new LevelUpGiftConfig();

        public static string KTEveryDayEvent_XML = "Config/KT_Activity/KTLevelUpEvent.xml";

        public static void Setup()
        {
            string Files = Global.GameResPath(KTEveryDayEvent_XML);

            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(LevelUpGiftConfig));
                _Event = serializer.Deserialize(stream) as LevelUpGiftConfig;
            }
        }

        #region TPC_NETWORK

        public static TCPProcessCmdResults ProcessQueryUpLevelGiftFlagList(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception) //解析错误
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析指令字符串错误, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
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
                    LogManager.WriteLog(LogTypes.Error, string.Format("Căn cứ Định vị RoleID  Đối tượng GameClient  thất bại, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                List<int> flagList;
                flagList = Global.GetRoleParamsIntListFromDB(client, RoleParamName.UpLevelGiftFlags);

                int Recorey = client.GetValueOfForeverRecore(RecoreDef.LevelUpEventRecore);

                if (flagList.Count == 0 && Recorey==-1)
                {
                    client.SetValueOfForeverRecore(RecoreDef.LevelUpEventRecore, 1);

                    foreach (LevelUpItem level in LevelUpEventManager._Event.LevelUpItem)
                    {
                        Global.SetBitValue(ref flagList, level.ID * 2 + 1, 0);
                    }

                    Global.SaveRoleParamsIntListToDB(client, flagList, RoleParamName.UpLevelGiftFlags, true);
                }

                LevelUpGiftConfig levelup = new LevelUpGiftConfig()
                {
                    LevelUpItem = LevelUpEventManager._Event.LevelUpItem,
                    BitFlags = flagList,
                };
                byte[] _cmdData = DataHelper.ObjectToBytes<LevelUpGiftConfig>(levelup);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, _cmdData, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception e)
            {
                LogManager.WriteException(e.ToString());
            }
            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static TCPProcessCmdResults ProcessGetUpLevelGiftAward(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析指令字符串错误, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("指令参数个数错误, CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int id = Convert.ToInt32(fields[1]);
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("根据RoleID定位GameClient对象失败, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int ret = -101;

                LevelUpItem upLevelItem = GetAllGiftCanGet(id);

                if (null != upLevelItem && client.m_Level >= upLevelItem.ToLevel)
                {
                    List<int> flagList = Global.GetRoleParamsIntListFromDB(client, RoleParamName.UpLevelGiftFlags);

                    //if (Global.GetBitValue(flagList, upLevelItem.ID * 2) == 0)
                    //{
                    //    ret = -101;
                    //    cmdData = string.Format("{0}:{1}", ret, id);
                    //    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, nID);
                    //    return TCPProcessCmdResults.RESULT_DATA;
                    //}

                    /// Đã nhận rồi
                    if (1 == Global.GetBitValue(flagList, upLevelItem.ID * 2 + 1))
                    {
                        ret = -103;
                        cmdData = string.Format("{0}:{1}", ret, id);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    // Lưu vào db là đã nhận mốc này
                    Global.SetBitValue(ref flagList, upLevelItem.ID * 2 + 1, 1);

                    Global.SaveRoleParamsIntListToDB(client, flagList, RoleParamName.UpLevelGiftFlags, true);

                    bool KQ = CanGetAward(upLevelItem, client);
                    if (KQ)
                    {
                        ret = 1;
                    }

                    cmdData = string.Format("{0}:{1}", ret, id);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                else
                {
                    cmdData = string.Format("{0}:{1}", -101, id);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdData, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
            }
            catch (Exception e)
            {
                LogManager.WriteException(e.ToString());
            }
            return TCPProcessCmdResults.RESULT_FAILED;
        }

        #endregion TPC_NETWORK

        public static LevelUpItem GetAllGiftCanGet(int Index)
        {
            LevelUpItem _Total = new LevelUpItem();

            _Total = _Event.LevelUpItem.Where(x => x.ID == Index).FirstOrDefault();

            return _Total;
        }

        public static bool CanGetAward(LevelUpItem _Gift, KPlayer player)
        {
            string TotalItemCanGet = _Gift.LevelUpGift;

            string[] Pram = TotalItemCanGet.Split('|');

            int COUNT = 10;//Pram.Count();

            if (!KTGlobal.IsHaveSpace(COUNT, player))
            {
                PlayerManager.ShowNotification(player, "Cần sắp xếp 10 ô trống trong túi đồ!");
                return false;
            }



            foreach (string Item in Pram)
            {
                string[] ItemPram = Item.Split(',');

                int ItemID = Int32.Parse(ItemPram[0]);
                int Number = Int32.Parse(ItemPram[1]);

                // Mặc định toàn bộ vật phẩm nhận từ event này sẽ khóa hết
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, ItemID, Number, 0, "LEVELUPEVENT", false, 1, false, Global.ConstGoodsEndTime, "", -1, "", 0, 1, true))
                {
                    PlayerManager.ShowNotification(player, "Có lỗi khi nhận vật phẩm chế tạo");
                }
            }

            return true;
        }
    }
}