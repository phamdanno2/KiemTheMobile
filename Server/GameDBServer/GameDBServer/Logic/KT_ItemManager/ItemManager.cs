using GameDBServer.Core;
using GameDBServer.DB;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameDBServer.Logic.KT_ItemManager
{
    public class ItemManager
    {
        private static ItemManager instance = new ItemManager();

        public static ItemManager getInstance()
        {
            return instance;
        }


        public void ResetItemUpdate()
        {

            this.IsItemProseccSing = false;
            // Reset lại mảng
            CardProsecc = new BlockingCollection<ItemCacheModel>();

        }

        public void AddItemProsecc(ItemCacheModel _Model)
        {
            CardProsecc.Add(_Model);
        }

        public BlockingCollection<ItemCacheModel> CardProsecc = new BlockingCollection<ItemCacheModel>();

        public bool IsItemProseccSing = false;

        // Cứ 2s tick 1 lần
        private const long MaxDBRoleParamCmdSlot = (2 * 1000);

        public long LastUpdateItem = 0;

        public void DoUpdateItem(DBManager dbMgr)
        {
            long Now = TimeUtil.NOW();

            // Nếu đang xử lý dở thì thôi đợi lần sau
            if (IsItemProseccSing)
            {
                return;
            }


            if (Now - LastUpdateItem > MaxDBRoleParamCmdSlot)
            {
                LastUpdateItem = Now;

                IsItemProseccSing = true;

                Console.WriteLine("DOUPDATE ITEM ==>TOTALCOUNT :" + CardProsecc.Count);

                while (CardProsecc.Count > 0)
                {
                    var ItemGet = CardProsecc.Take();

                    try
                    {
                      //  Console.WriteLine("GET INFO ROLEID :" + ItemGet.RoleId);

                        DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(ItemGet.RoleId);
                        if (null == dbRoleInfo)
                        {
                            LogManager.WriteLog(LogTypes.Error, "[ItemQueue][" + ItemGet.RoleId + "]Source player is not exist, RoleID=" + ItemGet.RoleId);
                            continue;
                        }

                       // Console.WriteLine("GET INFO ROLEID DONE :" + ItemGet.RoleId);

                        //Lấy ra vật phẩm trên người của thằng này
                        GoodsData itemGD = Global.GetGoodsDataByDbID(dbRoleInfo, ItemGet.ItemID);

                        //Lấy thông tin GOOD TỪ DATA RA Nếu không tìm thấy vật phẩm thì return lỗi -1000
                        if (null == itemGD)
                        {
                            LogManager.WriteLog(LogTypes.Error, "[ItemQueue][" + ItemGet.RoleId + "]Item Not Found=" + ItemGet.ItemID);
                            continue;
                        }

                      //  Console.WriteLine("GET ITEMDONE:" + ItemGet.RoleId);
                        //Thực hiện update vào DB theo lệnh
                        int ret = DBWriter.UpdateGoods(dbMgr, ItemGet.ItemID, ItemGet.Fields, 2);
                        if (ret < 0)
                        {
                            LogManager.WriteLog(LogTypes.Error, "[ItemQueue][" + ItemGet.RoleId + "]Update Item TO DB BUG :" + ItemGet.ItemID + "|PRAM :" + string.Join("|", ItemGet.Fields));
                            continue;
                        }
                        else
                        {

                          //  Console.WriteLine("GET ITEMDONE11:" + ItemGet.RoleId);
                            int gcount = DataHelper.ConvertToInt32(ItemGet.Fields[8], itemGD.GCount);

                            if (gcount > 0)
                            {
                                int newSite = DataHelper.ConvertToInt32(ItemGet.Fields[6], itemGD.Site);

                                itemGD.Using = DataHelper.ConvertToInt32(ItemGet.Fields[2], itemGD.Using);
                                itemGD.Forge_level = DataHelper.ConvertToInt32(ItemGet.Fields[3], itemGD.Forge_level);
                                itemGD.Starttime = DataHelper.ConvertToStr(ItemGet.Fields[4].Replace("#", ":"), itemGD.Starttime);
                                itemGD.Endtime = DataHelper.ConvertToStr(ItemGet.Fields[5].Replace("#", ":"), itemGD.Endtime);
                                itemGD.Site = newSite;
                                itemGD.Props = DataHelper.ConvertToStr(ItemGet.Fields[7], itemGD.Props);
                                itemGD.GCount = gcount;
                                itemGD.Binding = DataHelper.ConvertToInt32(ItemGet.Fields[9], itemGD.Binding);
                                itemGD.BagIndex = DataHelper.ConvertToInt32(ItemGet.Fields[10], itemGD.BagIndex);
                                itemGD.Strong = DataHelper.ConvertToInt32(ItemGet.Fields[11], itemGD.Strong);
                                itemGD.Series = DataHelper.ConvertToInt32(ItemGet.Fields[12], itemGD.Series);

                                string otherpramer = ItemGet.Fields[13];
                                if (otherpramer.Length > 10)
                                {
                                    byte[] Base64Decode = Convert.FromBase64String(otherpramer);

                                    Dictionary<ItemPramenter, string> _OtherParams = DataHelper.BytesToObject<Dictionary<ItemPramenter, string>>(Base64Decode, 0, Base64Decode.Length);
                                    itemGD.OtherParams = _OtherParams;
                                }

                                itemGD.GoodsID = DataHelper.ConvertToInt32(ItemGet.Fields[14], itemGD.GoodsID);
                            }
                            else
                            {
                                if (GameDBManager.Flag_t_goods_delete_immediately)
                                {
                                    DBWriter.MoveGoodsDataToBackupTable(dbMgr, ItemGet.ItemID);
                                }

                                /// Xóa vật phẩm khỏi danh sách
                                dbRoleInfo.GoodsDataList.TryRemove(itemGD.Id, out _);
                            }

                           // Console.WriteLine("GET ITEMDONE22:" + ItemGet.RoleId);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.SQL, "[ITEMQUEUE][" + ItemGet.RoleId + "]Exception=" + ex.ToString());
                    }
                }

                //TODO THỰC HIỆN UPDATE Ở ĐÂY

                IsItemProseccSing = false;
                // THỰC HIỆN CÁC TRUY VẤN Ở ĐÂY ĐỂ FILL RA BẢNG
            }
        }
    }
}