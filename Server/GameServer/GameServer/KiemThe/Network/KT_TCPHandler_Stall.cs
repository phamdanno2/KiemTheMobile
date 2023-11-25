using GameServer.Core.Executor;
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
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Class quản lý việc bày bán
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Bày Bán

        /// <summary>
        /// Bày bán 1 xập hàng
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessSpriteGoodsStallCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception) //解析错误
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi khi kiểm tra dữ liệu gửi lên, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                //Đọc dữ liệu
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi ở tham số gửi lên, CMD={0}, Client={1}, Recv={2}",
                        (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != roleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Không tìm thấy người chơi, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), roleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                /// Ngừng di chuyển
                KTPlayerStoryBoardEx.Instance.Remove(client);

                /// Bản đồ tương ứng
                GameMap map = GameManager.MapMgr.GetGameMap(client.MapCode);
                /// Nếu bản đồ không cho phép mở sạp
                if (!map.AllowStall)
                {
                    PlayerManager.ShowNotification(client, "Bản đồ hiện tại không cho phép mở sạp hàng!");
                    return TCPProcessCmdResults.RESULT_OK;
                }

                if (client.ClientSocket.IsKuaFuLogin)
                {
                    return TCPProcessCmdResults.RESULT_OK;
                }

                int stallType = Convert.ToInt32(fields[1]);
                int extTag1 = Convert.ToInt32(fields[2]);
                string extTag2 = fields[3];

                // Thực hiện bày bán
                if (stallType == (int)GoodsStallCmds.Request)
                {
                    //Nếu cấp độ của thằng play < 10 thì ko cho bày absn
                    if (client.m_Level < 10)
                    {
                        //Cấp quá nhỏ ko thể bày bán
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -9, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /*
                    TẠM THỜI BY PASS
                    /// Kiểm tra xem có nằm trong bản đồ bày bán không
                    if (!Global.AllowStartStall(client))
                    {
                        //Bản đồ hiện tại không cho bày bán
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -8, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                    */

                    /// Nếu đang cưỡi ngựa thì bỏ qua
                    if (client.IsRiding)
                    {
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    //Nếu mà sạp hàng hiện tại đang rỗng
                    if (client.StallDataItem == null)
                    {
                        //Thưc hiện tạo mới 1 sập hàng
                        StallData sd = new StallData()
                        {
                            StallID = 0,
                            RoleID = client.RoleID,
                            StallName = extTag2,
                            StallMessage = "",
                            GoodsList = new List<GoodsData>(),
                            GoodsPriceDict = new Dictionary<int, int>(),
                            AddDateTime = TimeUtil.NOW(),
                        };

                        client.StallDataItem = sd;

                        //Thông báo về client là mở cửa hàng thành công với data kia
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, sd);

                        //Update trạng thái đang mở cửa hàng cho client
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, 0, stallType);
                    }
                    else
                    {
                        //Nếu đã mở cửa hàng rồi thì báo đã mở rồi
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -10, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else if (stallType == (int)GoodsStallCmds.Start)
                {
                    /// Nếu đang cưỡi ngựa thì bỏ qua
                    if (client.IsRiding)
                    {
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    //Nếu mà đã có sập hàng
                    if (client.StallDataItem != null)
                    {
                        lock (client.StallDataItem)
                        {
                            client.StallDataItem.Start = 1;
                        }

                        //Thông báo về client bắt đầu bán bàng
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, 0, stallType);

                        //Thông báo thằng này bắt đầu bán hàng cho bọn xung quanh
                        GameManager.ClientMgr.NotifySpriteStartStall(tcpMgr.MySocketListener, pool, client);


                    }
                    else
                    {
                        // Nếu chưa tạo gian hàng mà đã đòi bán thì chim cút
                        //Bạn chưa có gian hàng không thể mở bán
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -10, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else if (stallType == (int)GoodsStallCmds.Cancel)
                {
                    //Hủy bỏ gian hàng
                    if (client.StallDataItem != null) // Hủy bán sạp hàng
                    {
                        bool notify = false;
                        lock (client.StallDataItem)
                        {
                            notify = (client.StallDataItem.Start == 1);
                            client.StallDataItem.StallName = "";
                        }

                        if (notify)
                        {
                            // Thông báo cho bọn xung quanh biết là thằng này đóng cửa hàng ko bán nữa
                            GameManager.ClientMgr.NotifySpriteStartStall(tcpMgr.MySocketListener, pool, client);
                        }

                        //KHôi phục lại vật phẩm về túi đồ
                        Global.RestoreStallData(client, client.StallDataItem);

                        client.StallDataItem = null;

                        //Notify về client CLEAR GOODS
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, 0, stallType);
                    }
                    else
                    {
                        //Lỗi đéo hủy bán được
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -10, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }
                }
                else if (stallType == (int)GoodsStallCmds.AddGoods) //Thêm vật phẩm vào sập hàng đang bán
                {
                    // vật phẩm bày bán và giá
                    int addGoodsDbID = extTag1;

                    int price = Convert.ToInt32(extTag2);


                    if (client.StallDataItem != null)
                    {
                        StallData sd = client.StallDataItem;

                        // Thêm vật phẩm vào gian hàng
                        Global.AddGoodsDataIntoStallData(client, addGoodsDbID, sd, price);

                        //Notify lại data bày bán của thằng này
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, sd);
                    }
                }
                else if (stallType == (int)GoodsStallCmds.RemoveGoods) //Xóa vật phẩm khỏi bày bán
                {
                    int addGoodsDbID = extTag1;

                    //Xóa vật phẩm khỏi bày bán
                    if (client.StallDataItem != null)
                    {
                        StallData sd = client.StallDataItem;

                        //Xóa bỏ bày bán
                        Global.RemoveGoodsDataFromStallData(client, addGoodsDbID, sd);

                        //Notify lại dữ liệu bày bán của thằng này
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, sd);
                    }
                }
                else if (stallType == (int)GoodsStallCmds.UpdateMessage) //Update tên cửa hàng + Text
                {
                    string stallName = extTag2;

                    //Tìm gian hàng
                    if (client.StallDataItem != null)
                    {
                        StallData sd = client.StallDataItem;

                        lock (sd)
                        {
                            sd.StallName = stallName;
                        }

                        //Update lại dữ liệu bày bán
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, sd);

                        //Thông báo về client bắt đầu mở sạp hàng
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, 0, stallType);
                    }
                }
                else if (stallType == (int)GoodsStallCmds.ShowStall) //Packet xem cửa hàng
                {
                    int otherRoleID = extTag1;

                    // Tìm gian hàng của thằng đang được xem
                    KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                    if (null == otherClient)
                    {
                        //Nếu ko tìm thấy gian hàng thông báo về lỗi
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -1, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Vị trí hiện tại của chủ cửa hàng
                    UnityEngine.Vector2 ownerPos = new UnityEngine.Vector2((int) otherClient.CurrentPos.X, (int) otherClient.CurrentPos.Y);
                    /// Vị trí của người chơi
                    UnityEngine.Vector2 playerPos = new UnityEngine.Vector2((int) client.CurrentPos.X, (int) client.CurrentPos.Y);
                    /// Khoảng cách
                    float distance = UnityEngine.Vector2.Distance(ownerPos, playerPos);
                    /// Nếu quá xa
                    if (distance > 50)
                    {
                        PlayerManager.ShowNotification(client, "Khoảng cách quá xa, không thể mở cửa hàng!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    if (otherClient.StallDataItem == null) //Nếu ko có gian hàng thông báo về ko có gian hàng
                    {
                        //Thông báo gian hàng ko tồn tịa
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -2, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Gửi gói tin mở cửa hàng của thằng muốn xem
                    GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, 1, stallType);

                    ///Trả về thông tin gian hàng cho thằng muốn xem
                    GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, otherClient.StallDataItem);
                }
                else if (stallType == (int)GoodsStallCmds.BuyGoods) //购买物品
                {
                    int otherRoleID = extTag1;
                    int goodsDbId = Convert.ToInt32(extTag2);

                    //Tìm xem thằng kia là thằng nào
                    KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
                    if (null == otherClient)
                    {
                        //Ko tìm thấy thì toang luôn
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -1, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    /// Vị trí hiện tại của chủ cửa hàng
                    UnityEngine.Vector2 ownerPos = new UnityEngine.Vector2((int) otherClient.CurrentPos.X, (int) otherClient.CurrentPos.Y);
                    /// Vị trí của người chơi
                    UnityEngine.Vector2 playerPos = new UnityEngine.Vector2((int) client.CurrentPos.X, (int) client.CurrentPos.Y);
                    /// Khoảng cách
                    float distance = UnityEngine.Vector2.Distance(ownerPos, playerPos);
                    /// Nếu quá xa
                    if (distance > 50)
                    {
                        PlayerManager.ShowNotification(client, "Khoảng cách quá xa, không thể mua vật phẩm!");
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    if (otherClient.StallDataItem == null)
                    {
                        //Thông báo ko tìm thấy
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, -2, stallType);
                        return TCPProcessCmdResults.RESULT_OK;
                    }

                    StallData sd = otherClient.StallDataItem;
                    if (null != sd && sd.Start > 0)
                    {

                        // Thực hiện mua hàng
                        int ret = Global.BuyFromStallData(client, otherClient, sd, goodsDbId);

                        //Gửi lại thông tin sập hàng cho thằng bán
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, client, otherClient.StallDataItem);

                        //Gửi lại thông tin sập hàng cho thằng mua
                        GameManager.ClientMgr.NotifyGoodsStallData(tcpMgr.MySocketListener, pool, otherClient, otherClient.StallDataItem);

                        //Thông báo kết quả mua thành công
                        GameManager.ClientMgr.NotifyGoodsStallCmd(tcpMgr.MySocketListener, pool, client, ret, stallType);
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

        #endregion Bày Bán

    }
}
