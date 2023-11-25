using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.Logic;
using GameServer.Logic.CheatGuard;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using Tmsk.Contract;

namespace GameServer.Server.CmdProcesser
{
    /// <summary>
    /// 装备洗练
    /// </summary>
    public class SaleCmdsProcessor : ICmdProcessor
    {
        private TCPManager tcpMgr { get { return TCPManager.getInstance(); } }
        private TCPOutPacketPool pool { get { return TCPManager.getInstance().TcpOutPacketPool; } }
        private TCPClientPool tcpClientPool { get { return TCPManager.getInstance().tcpClientPool; } }

        private TCPGameServerCmds CmdID = TCPGameServerCmds.CMD_SPR_OPENMARKET2;

        public SaleCmdsProcessor(TCPGameServerCmds cmdID)
        {
            CmdID = cmdID;
        }

        public static SaleCmdsProcessor getInstance(TCPGameServerCmds cmdID)
        {
            return new SaleCmdsProcessor(cmdID);
        }

        private bool CanUseMarket(KPlayer client)
        {
            try
            {
               
             

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 交易所、拍卖行指令处理
        /// </summary>
        public bool processCmd(KPlayer client, string[] cmdParams)
        {
            int nID = (int)CmdID;

            switch (CmdID)
            {
                case TCPGameServerCmds.CMD_SPR_OPENMARKET2:
                    return OpenMarket(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_MARKETSALEMONEY2:
                    return MarketSaleMoney(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_SALEGOODS2:
                    return SaleGoods(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_SELFSALEGOODSLIST2:
                    return SelfSaleGoodsList(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_OTHERSALEGOODSLIST2:
                    return OtherSaleGoodsList(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_MARKETROLELIST2:
                    return MarketRoleList(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_MARKETGOODSLIST2:
                    return MarketGoodsList(client, cmdParams);

                case TCPGameServerCmds.CMD_SPR_MARKETBUYGOODS2:
                    return MarketBuyGoods(client, cmdParams);
            }

            return true;
        }

        private bool OpenMarket(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);
            int offlineMarket = Convert.ToInt32(fields[1]);
            string marketName = fields[2];

            string strcmd = "";
            if (string.IsNullOrEmpty(marketName)) //停止摆摊
            {
                strcmd = string.Format("{0}:{1}:{2}", roleID, marketName, offlineMarket);
                client.SendPacket((int)CmdID, strcmd);
                return true;
            }

            marketName = marketName.Substring(0, Math.Min(10, marketName.Length));

            //判断是否有需要摆摊的物品
            if (client.SaleGoodsDataList.Count <= 0)
            {
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                    client, StringUtil.substitute(Global.GetLang("请至少上架一件物品才能摆摊!")),
                    GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                return true;
            }

            return true;
        }

        private bool MarketSaleMoney(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);
            int saleOutMoney = Math.Max(0, Convert.ToInt32(fields[1]));
            int TokenPrice = Math.Max(0, Convert.ToInt32(fields[2]));

            if (client.ClientSocket.IsKuaFuLogin)
            {
                return true;
            }

            //是否禁用交易市场购买功能
            int disableMarket = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-market", 0);
            if (disableMarket > 0)
            {
                return true;
            }

            if (!CanUseMarket(client))
            {
                return true;
            }

            //if (TradeBlackManager.Instance().IsBanTrade(client.RoleID))
            //{
            //    string tip = Global.GetLang("您目前被禁止使用交易行");
            //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, tip, GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
            //    return true;
            //}

            string strcmd = "";
            if (saleOutMoney > client.BoundToken)
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, roleID, saleOutMoney, TokenPrice, 0);
                client.SendPacket((int)CmdID, strcmd);
                return true;
            }

            //扣除银两
            if (!GameManager.ClientMgr.SubBoundToken(tcpMgr.MySocketListener, tcpClientPool, pool, client, saleOutMoney, "交易市场一"))
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -2, roleID, saleOutMoney, TokenPrice, 0);
                client.SendPacket((int)CmdID, strcmd);
                return true;
            }

            GoodsData goodsData = Global.GetNewGoodsData((int)SaleGoodsConsts.BaiTanJinBiGoodsID, 0);
            goodsData.Site = (int)SaleGoodsConsts.SaleGoodsID;

            Global.AddSaleGoodsData(client, goodsData);

            EventLogManager.AddGoodsEvent(client, OpTypes.Move, OpTags.None, goodsData.GoodsID, goodsData.Id, 0, goodsData.GCount, "铜钱交易上架");

            //将新修改的物品加入出售物品管理列表
            SaleGoodsItem saleGoodsItem = new SaleGoodsItem()
            {
                GoodsDbID = goodsData.Id,
                SalingGoodsData = goodsData,
                Client = client,
            };
            SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);

            //如果从0到1，则加入摊位管理
            if (1 == client.SaleGoodsDataList.Count)
            {
                SaleRoleManager.AddSaleRoleItem(client);
            }

            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 0, roleID, saleOutMoney, TokenPrice, goodsData.Id);
            client.SendPacket((int)CmdID, strcmd);
            return true;
        }

        private bool SaleGoods(KPlayer client, string[] fields)
        {
            TCPGameServerCmds nID = CmdID;

            int roleID = Convert.ToInt32(fields[0]);
            int goodsDbID = Convert.ToInt32(fields[1]);
            int site = Convert.ToInt32(fields[2]);
            int saleMoney = Convert.ToInt32(fields[3]);
            int saleYuanBao = Convert.ToInt32(fields[4]);
            int saleYinPiao = Convert.ToInt32(fields[5]);
            int saleGoodsCount = Convert.ToInt32(fields[6]);

            // 金币也可以作上架用 ChenXiaojun
            // saleMoney = 0;
            saleYinPiao = 0;

            //如果出售金币和元宝都大于0,则提交数据有误,拒绝执行
            if ((saleMoney > 0) && (saleYuanBao > 0))
            {
                return true;
            }

            if (client.ClientSocket.IsKuaFuLogin)
            {
                return true;
            }

            //是否禁用交易市场购买功能
            int disableMarket = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-market", 0);
            if (disableMarket > 0)
            {
                return true;
            }

            if (!CanUseMarket(client))
            {
                return true;
            }

            //if (TradeBlackManager.Instance().IsBanTrade(client.RoleID))
            //{
            //    string tip = Global.GetLang("您目前被禁止使用交易行");
            //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, tip, GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
            //    return true;
            //}

            //如果已经在摆摊中，则不能再上线物品 // 策划要求去掉这个判断 2014.4.11
            /*if (client.AllowMarketBuy)
            {
                return true;
            }*/

            string strcmd = "";
            int bagIndex = 0; //找到空闲的包裹格子

            //修改内存中物品记录
            GoodsData goodsData = Global.GetGoodsByDbID(client, goodsDbID);
            if (null == goodsData)
            {
                goodsData = Global.GetSaleGoodsDataByDbID(client, goodsDbID);
                if (null == goodsData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("从交易市场定位物品对象失败, CMD={0}, Client={1}, RoleID={2}, GoodsDbID={3}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(client.ClientSocket), roleID, goodsDbID));
                    return true;
                }
                else
                {
                    if (!Global.CanAddGoods(client, goodsData.GoodsID, goodsData.GCount, goodsData.Binding, goodsData.Endtime, true))
                    {
                        GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool,
                            client, StringUtil.substitute(Global.GetLang("背包已满，无法将物品从市场下架到背包中")),
                            GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                        return true;
                    }
                    bagIndex = KTGlobal.GetIdleSlotOfBagGoods(client); //找到空闲的包裹格子
                }
            }
            else //如果是从背包到挂售的列表，则判断此物品是否可以挂售
            {
                if (goodsData.Using > 0)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", StdErrorCode.Error_Goods_Is_Using, roleID, goodsDbID, site, saleMoney, saleYuanBao);
                    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                    return true;
                }

                if (goodsData.Binding > 0)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", -100, roleID, goodsDbID, site, saleMoney, saleYuanBao);
                    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                    return true;
                }

                if (Global.IsTimeLimitGoods(goodsData))
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", -101, roleID, goodsDbID, site, saleMoney, saleYuanBao);
                    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                    return true;
                }

                //判断已经挂售的物品是否超过了最大限制
                if (Global.GetSaleGoodsDataCount(client) >= (int)SaleManager.MaxSaleNum)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", -110, roleID, goodsDbID, site, saleMoney, saleYuanBao);
                    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                    return true;
                }

                //判断如果要挂售的物品可以叠加，并且出售的数量小于物品的数量，则执行拆分操作
                int gridNum = Global.GetGoodsGridNumByID(goodsData.GoodsID);

                //不做任何处理
                if (gridNum > 1 && saleGoodsCount > 0 && saleGoodsCount < goodsData.GCount)
                {
                    //根据参数命令拆分物品
                    //if (TCPProcessCmdResults.RESULT_OK != Global.SplitGoodsByCmdParams(client, client.ClientSocket, (int)TCPGameServerCmds.CMD_SPR_SPLIT_GOODS, roleID, goodsData.Id, goodsData.Site, goodsData.GoodsID, goodsData.GCount - saleGoodsCount, false))
                    //{
                    //    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", -201, roleID, goodsDbID, site, saleMoney, saleYuanBao);
                    //    client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                    //    return true;
                    //}
                }
            }

            //向DBServer请求修改物品
            string[] dbFields = null;

            strcmd = Global.FormatUpdateDBGoodsStr(roleID, goodsDbID, "*", "*", "*", "*", site, "*", "*", "*", "*", bagIndex, saleMoney, saleYuanBao, saleYinPiao, "*", "*", "*", "*", "*", "*", "*", "*"); // 卓越一击 [12/13/2013 LiaoWei] 装备转生
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields, client.ServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", -1, roleID, goodsDbID, site, saleMoney, saleYuanBao, saleYinPiao);
                client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                return true;
            }

            if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[1]) < 0)
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", -10, roleID, goodsDbID, site, saleMoney, saleYuanBao, saleYinPiao);
                client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
                return true;
            }
            goodsData.BagIndex = bagIndex;
            if (goodsData.Site != site) //位置没有改变
            {
                if (goodsData.Site == 0 && site == (int)SaleGoodsConsts.SaleGoodsID) //原来在背包, 现在到出售列表
                {
                    Global.RemoveGoodsData(client, goodsData);

                    goodsData.Site = site;

                    Global.AddSaleGoodsData(client, goodsData);

                    EventLogManager.AddGoodsEvent(client, OpTypes.Move, OpTags.None, goodsData.GoodsID, goodsData.Id, 0, goodsData.GCount, "交易上架");

                    //将新修改的物品加入出售物品管理列表
                    SaleGoodsItem saleGoodsItem = new SaleGoodsItem()
                    {
                        GoodsDbID = goodsData.Id,
                        SalingGoodsData = goodsData,
                        Client = client,
                    };
                    SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);

                    //如果从0到1，则加入摊位管理
                    if (1 == client.SaleGoodsDataList.Count)
                    {
                        SaleRoleManager.AddSaleRoleItem(client);
                    }
                }
                else if (goodsData.Site == (int)SaleGoodsConsts.SaleGoodsID && site == 0) //原来在出售列表, 现在到背包
                {
                    //从出售列表中删除
                    SaleGoodsManager.RemoveSaleGoodsItem(goodsData.Id);

                    Global.RemoveSaleGoodsData(client, goodsData);

                    if ((int)SaleGoodsConsts.BaiTanJinBiGoodsID != goodsData.GoodsID)
                    {
                        goodsData.Site = site;

                        Global.AddGoodsData(client, goodsData);

                        EventLogManager.AddGoodsEvent(client, OpTypes.Move, OpTags.None, goodsData.GoodsID, goodsData.Id, 0, goodsData.GCount, "交易下架");
                    }
                    else
                    {
                        //Loại tiền tệ j đó đã xóa
                    }

                    //如果从1到0，则删除摊位管理
                    if (0 == client.SaleGoodsDataList.Count)
                    {
                        SaleRoleManager.RemoveSaleRoleItem(client.RoleID);
                    }

                    // 属性改造 去掉 负重[8/15/2013 LiaoWei]
                    //更新重量
                    //Global.UpdateGoodsWeight(client, goodsData, goodsData.GCount, true, false);
                }
            }

            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", 0, roleID, goodsDbID, site, saleMoney, saleYuanBao, saleYinPiao);
            client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SALEGOODS2, strcmd);
            return true;
        }

        private bool SelfSaleGoodsList(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);

            if (client.ClientSocket.IsKuaFuLogin)
            {
                client.SendPacket<List<GoodsData>>((int)TCPGameServerCmds.CMD_SPR_SELFSALEGOODSLIST2, null);
                return true;
            }

            List<GoodsData> saleGoodsDataList = client.SaleGoodsDataList;
            client.SendPacket((int)TCPGameServerCmds.CMD_SPR_SELFSALEGOODSLIST2, saleGoodsDataList);
            return true;
        }

        private bool OtherSaleGoodsList(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);
            int otherRoleID = Convert.ToInt32(fields[1]);

            if (client.ClientSocket.IsKuaFuLogin)
            {
                client.SendPacket<List<GoodsData>>((int)TCPGameServerCmds.CMD_SPR_OTHERSALEGOODSLIST2, null);
                return true;
            }

            List<GoodsData> saleGoodsDataList = new List<GoodsData>();
            KPlayer otherClient = GameManager.ClientMgr.FindClient(otherRoleID);
            if (null != otherClient)
            {
                saleGoodsDataList = otherClient.SaleGoodsDataList;
            }

            client.SendPacket((int)TCPGameServerCmds.CMD_SPR_OTHERSALEGOODSLIST2, saleGoodsDataList);
            return true;
        }

        private bool MarketRoleList(KPlayer client, string[] fields)
        {
            List<SaleRoleData> saleRoleDataList = SaleRoleManager.GetSaleRoleDataList();
            client.SendPacket((int)TCPGameServerCmds.CMD_SPR_MARKETROLELIST2, saleRoleDataList);
            return true;
        }

        /// <summary>
        /// Get ra danh sách vật phẩm ở SHOP
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private bool MarketGoodsList(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);
            int marketSearchType = Convert.ToInt32(fields[1]);
            int startIndex = Convert.ToInt32(fields[2]);
            int maxCount = Convert.ToInt32(fields[3]);
            string marketSearchText = fields[4];

            if (client.ClientSocket.IsKuaFuLogin)
            {
                client.SendPacket<SaleGoodsSearchResultData>((int)TCPGameServerCmds.CMD_SPR_MARKETGOODSLIST2, null);
                return true;
            }

            // Limit Reresh SHOP liên tục
            if (CreateRoleLimitManager.Instance().RefreshMarketSlotTicks > 0 &&
                TimeUtil.NOW() - client._RefreshMarketTicks < CreateRoleLimitManager.Instance().RefreshMarketSlotTicks)
            {
                // 返回错误信息
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                    StringUtil.substitute(Global.GetLang("您操作过快，请稍后再试")), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                return true;
            }
            client._RefreshMarketTicks = TimeUtil.NOW();

            SaleGoodsSearchResultData saleGoodsSearchResultData = new SaleGoodsSearchResultData();
            if ((int)MarketSearchTypes.SearchAll == marketSearchType) //返回全部
            {
                saleGoodsSearchResultData.saleGoodsDataList = SaleGoodsManager.GetSaleGoodsDataList();
            }
            else if ((int)MarketSearchTypes.SearchGoodsIDs == marketSearchType) //根据物品ID匹配返回
            {
                Dictionary<int, bool> goodsIDDict = new Dictionary<int, bool>();
                string[] searchFileds = marketSearchText.Split(',');
                if (null != searchFileds && searchFileds.Length > 0)
                {
                    for (int i = 0; i < searchFileds.Length; i++)
                    {
                        int searchGoodsID = Global.SafeConvertToInt32(searchFileds[i]);
                        goodsIDDict[searchGoodsID] = true;
                    }

                    saleGoodsSearchResultData.saleGoodsDataList = SaleGoodsManager.FindSaleGoodsDataList(goodsIDDict);
                }
            }
            else if ((int)MarketSearchTypes.SearchRoleName == marketSearchType) //根据角色名称模糊匹配返回
            {
                saleGoodsSearchResultData.saleGoodsDataList = SaleGoodsManager.FindSaleGoodsDataListByRoleName(marketSearchText);
            }
            else if ((int)MarketSearchTypes.TypeAndFilterOpts == marketSearchType)
            {
                string[] searchParams = marketSearchText.Split('$');
                if (searchParams.Length >= 6)
                {
                    int type = Global.SafeConvertToInt32(searchParams[0]);
                    int id = Global.SafeConvertToInt32(searchParams[1]);
                    int moneyFlags = Global.SafeConvertToInt32(searchParams[2]);
                    int colorFlags = Global.SafeConvertToInt32(searchParams[3]);
                    int orderBy = Global.SafeConvertToInt32(searchParams[4]);
                    int orderTypeFlags = 1;
                    List<int> goodsIDs;
                    if (searchParams.Length >= 7)
                    {
                        orderTypeFlags = Global.SafeConvertToInt32(searchParams[5]);
                        goodsIDs = Global.StringToIntList(searchParams[6], '#');
                    }
                    else
                    {
                        goodsIDs = Global.StringToIntList(searchParams[5], '#');
                    }

                    saleGoodsSearchResultData.Type = type;
                    saleGoodsSearchResultData.ID = id;
                    saleGoodsSearchResultData.MoneyFlags = moneyFlags;
                    saleGoodsSearchResultData.ColorFlags = colorFlags;
                    saleGoodsSearchResultData.OrderBy = orderBy;
                    if (moneyFlags <= 0)
                    {
                        moneyFlags = SaleManager.ConstAllMoneyFlags;
                    }
                    if (colorFlags <= 0)
                    {
                        colorFlags = SaleManager.ConstAllColorFlags;
                    }
                    SearchArgs args = new SearchArgs(id, type, moneyFlags, colorFlags, orderBy, orderTypeFlags);
                    if (goodsIDs.IsNullOrEmpty())
                    {
                        saleGoodsSearchResultData.saleGoodsDataList = SaleManager.GetSaleGoodsDataList(args, null);
                        if (null != saleGoodsSearchResultData.saleGoodsDataList)
                        {
                            saleGoodsSearchResultData.TotalCount = saleGoodsSearchResultData.saleGoodsDataList.Count;
                        }
                    }
                    else
                    {
                        //maxCount = Global.GMax(maxCount, (int)SaleGoodsConsts.MaxReturnNum);
                        saleGoodsSearchResultData.saleGoodsDataList = SaleManager.GetSaleGoodsDataList(args, goodsIDs);
                        if (null == saleGoodsSearchResultData.saleGoodsDataList || saleGoodsSearchResultData.saleGoodsDataList.Count == 0)
                        {
                            saleGoodsSearchResultData.TotalCount = -1;
                        }
                        else
                        {
                            saleGoodsSearchResultData.TotalCount = saleGoodsSearchResultData.saleGoodsDataList.Count;
                        }
                    }

                    if (null != saleGoodsSearchResultData.saleGoodsDataList && saleGoodsSearchResultData.saleGoodsDataList.Count > 0)
                    {
                        saleGoodsSearchResultData.StartIndex = startIndex;

                        if (startIndex >= saleGoodsSearchResultData.TotalCount)
                        {
                            saleGoodsSearchResultData.saleGoodsDataList = null;
                        }
                        else
                        {
                            startIndex = Global.GMin(startIndex, saleGoodsSearchResultData.saleGoodsDataList.Count - 1);
                            maxCount = Global.GMin(maxCount, saleGoodsSearchResultData.saleGoodsDataList.Count - startIndex);
                            saleGoodsSearchResultData.saleGoodsDataList = saleGoodsSearchResultData.saleGoodsDataList.GetRange(startIndex, maxCount);
                        }
                    }
                }
            }

            client.SendPacket<SaleGoodsSearchResultData>((int)TCPGameServerCmds.CMD_SPR_MARKETGOODSLIST2, saleGoodsSearchResultData);
            return true;
        }


        /// <summary>
        /// HÀM THỰC HIỆN MUA ĐÃ XÓA ĐI RẤT NHIỀU CODE ĐỢI LÚC NÀO SỬ DỤNG THÌ CHECK LẠI
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private bool MarketBuyGoods(KPlayer client, string[] fields)
        {
            int roleID = Convert.ToInt32(fields[0]);
            int goodsDbID = Convert.ToInt32(fields[1]);
            int goodsID = Convert.ToInt32(fields[2]);
            int clientMoneyType = Convert.ToInt32(fields[3]);
            int clientMoneyValue = Convert.ToInt32(fields[4]);
            int tax = 0;

            if (client.ClientSocket.IsKuaFuLogin)
            {
                return true;
            }

            //是否禁用交易市场购买功能
            int disableMarket = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-market", 0);
            if (disableMarket > 0)
            {
                return true;
            }

            if (!CanUseMarket(client))
            {
                return true;
            }

            //if (TradeBlackManager.Instance().IsBanTrade(client.RoleID))
            //{
            //    string tip = Global.GetLang("您目前被禁止使用交易行");
            //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, tip, GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
            //    return true;
            //}

            int salePrice = 0;
            int otherRID = 0;

            KPlayer otherClient = null;
            SaleGoodsItem saleGoodsItem = SaleGoodsManager.RemoveSaleGoodsItem(goodsDbID);

            if (null != saleGoodsItem)
            {
                //对方的角色
                otherClient = GameManager.ClientMgr.FindClient(saleGoodsItem.Client.RoleID); //查找，确保还在线
                if (null != otherClient)
                {
                    if (otherClient.RoleID == client.RoleID)
                    {
                        //不能购买自己的物品
                        SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);
                        GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, -30, 0, goodsDbID, goodsID, (int)CmdID);
                        return true;
                    }
                }
                otherRID = saleGoodsItem.Client.RoleID;
            }

            if (null != saleGoodsItem && null != otherClient) //在线购买
            {
                //if (TradeBlackManager.Instance().IsBanTrade(otherClient.RoleID))
                //{
                //    SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);
                //    string tip = Global.GetLang("对方目前被禁止使用交易行");
                //    GameManager.ClientMgr.NotifyImportantMsg(tcpMgr.MySocketListener, pool, client, tip, GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                //    return true;
                //}

                GoodsData goodsData = Global.GetSaleGoodsDataByDbID(otherClient, goodsDbID);
                if (null == goodsData)
                {
                    //返回管理
                    SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);

                    //发送错误消息
                    GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, -3, 0, goodsDbID, goodsID, (int)CmdID);
                    return true;
                }

                if (goodsData.GoodsID != goodsID)
                {
                    //返回管理
                    SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);

                    //发送错误消息
                    GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, -1003, 0, goodsDbID, goodsID, (int)CmdID);
                    return true;
                }

                //如果不是特殊的摆摊金币物品
                if ((int)SaleGoodsConsts.BaiTanJinBiGoodsID != goodsData.GoodsID)
                {
                    //判断背包是否够用
                    if (!Global.CanAddGoods(client, goodsData.GoodsID, goodsData.GCount, 0, goodsData.Endtime, true))
                    {
                        //返回管理
                        SaleGoodsManager.AddSaleGoodsItem(saleGoodsItem);

                        //发送错误消息
                        GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, -5, 0, goodsDbID, goodsID, (int)CmdID);
                        return true;
                    }
                }

                int site = goodsData.Site;

                goodsData.Site = 0;

                Global.RemoveSaleGoodsData(otherClient, goodsData);

                bool bMoveToTarget = true;
                // 如果不是特殊的摆摊金币物品
                if ((int)SaleGoodsConsts.BaiTanJinBiGoodsID != goodsData.GoodsID)
                {
                    bMoveToTarget = true;
                }
                else
                {
                    // 金币物品不移到玩家身上，直接加金币
                    bMoveToTarget = false;
                }

                //转移物品
                bool ret = GameManager.ClientMgr.MoveGoodsDataToOtherRole(Global._TCPManager.MySocketListener,
                    Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                    goodsData, otherClient, client, bMoveToTarget);

                if (!ret)
                {
                    GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, -100, 0, goodsDbID, goodsID, (int)CmdID);
                    return true;
                }

                // 特殊的摆摊金币物品
                if (!bMoveToTarget)
                {
                }

                GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, otherClient, client, 0, 1, goodsDbID, goodsID, (int)CmdID);

                //通知自己
                GameManager.ClientMgr.NotifySpriteMarketBuy(tcpMgr.MySocketListener, pool, client, otherClient, 0, 0, goodsDbID, goodsID, (int)CmdID);

                // 交易黑名单事件
                // TradeBlackManager.Instance().OnMarketBuy(client.RoleID, otherClient.RoleID, tradeBlackCopy);
            }

            // number log
            int tradelog_num_minamount = GameManager.GameConfigMgr.GetGameConfigItemInt(GameConfigNames.tradelog_num_minamount, 5000);
            if (salePrice >= tradelog_num_minamount)
            {
             
            }

            // freq log

            // 记录花钱的人的记录
            int freqNumber = Global.IncreaseTradeCount(client, RoleParamName.SaleTradeDayID, RoleParamName.SaleTradeCount);
            int tradelog_freq_sale = GameManager.GameConfigMgr.GetGameConfigItemInt(GameConfigNames.tradelog_freq_sale, 10);
            if (freqNumber >= tradelog_freq_sale)
            {
               
            }

            return true;
        }

        /// <summary>
        ///KHI mua ko thành công thì trả lại tiền
        /// </summary>
        /// <param name="client"></param>
        /// <param name="SalingGoodsData"></param>
        private void GiveBackSaleGoodsMoney(KPlayer client, KPlayer saller, GoodsData SalingGoodsData, int saleMoney, int saleYuanBao, int site)
        {
            //SalingGoodsData.SaleMoney = saleMoney;
            //SalingGoodsData.SaleYuanBao = saleYuanBao;
            //SalingGoodsData.Site = site;
            //int tax = 0;
            //int backSaleMoney = CalcRealMoneyAfterTax(saleMoney, MoneyTypes.BoundToken, out tax);
            //int backSaleYuanBao = CalcRealMoneyAfterTax(saleYuanBao, MoneyTypes.YuanBao, out tax);

            //if (SalingGoodsData.SaleYuanBao > 0)
            //{
            //    if (!GameManager.ClientMgr.AddToken(tcpMgr.MySocketListener, tcpClientPool, pool, client, SalingGoodsData.SaleYuanBao, "新交易市场购买失败退回"))
            //    {
            //        LogManager.WriteLog(LogTypes.SQL, string.Format("新交易市场购买失败退回钻石失败:", Global.FormatRoleName4(client), SalingGoodsData.SaleYuanBao));
            //    }
            //    if (!GameManager.ClientMgr.AddToken(tcpMgr.MySocketListener, tcpClientPool, pool, saller, -backSaleYuanBao, "新交易市场购买失败退回"))
            //    {
            //        LogManager.WriteLog(LogTypes.SQL, string.Format("新交易市场购买失败退回钻石失败:", Global.FormatRoleName4(client), -backSaleYuanBao));
            //    }
            //}
        }

        /// <summary>
        /// 购买失败时,将玩家从交易市场购买时的花费还回,将离线卖家获得的钱扣除
        /// </summary>
        /// <param name="client"></param>
        /// <param name="SalingGoodsData"></param>
        private void GiveBackSaleGoodsMoneyOffline(KPlayer client, string userID, int sallerRoleID, string sallerName, GoodsData SalingGoodsData, int saleMoney, int saleYuanBao, int site)
        {
            //SalingGoodsData.SaleMoney = saleMoney;
            //SalingGoodsData.SaleYuanBao = saleYuanBao;
            //SalingGoodsData.Site = site;
            //int tax = 0;
            //int backSaleMoney = CalcRealMoneyAfterTax(saleMoney, MoneyTypes.BoundToken, out tax);
            //int backSaleYuanBao = CalcRealMoneyAfterTax(saleYuanBao, MoneyTypes.YuanBao, out tax);

            //if (SalingGoodsData.SaleYuanBao > 0)
            //{
            //    if (!GameManager.ClientMgr.AddToken(tcpMgr.MySocketListener, tcpClientPool, pool, client, SalingGoodsData.SaleYuanBao, "新交易市场购买失败退回"))
            //    {
            //        LogManager.WriteLog(LogTypes.SQL, string.Format("新交易市场购买失败退回钻石失败:", Global.FormatRoleName4(client), SalingGoodsData.SaleYuanBao));
            //    }
            //    if (!GameManager.ClientMgr.AddTokenOffLine(tcpMgr.MySocketListener, tcpClientPool, pool, sallerRoleID, -backSaleYuanBao, "新交易市场购买失败退回", client.ZoneID, client.strUserID))
            //    {
            //        LogManager.WriteLog(LogTypes.SQL, string.Format("新交易市场购买失败退回钻石失败:", Global.FormatRoleName4(client), -backSaleYuanBao));
            //    }
            //}
        }
    }
}