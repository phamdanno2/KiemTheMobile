using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using Server.TCP;
using Server.Protocol;
using GameServer;
using Server.Data;
using GameServer.Server;

namespace GameServer.Logic
{
    /// <summary>
    /// 出售物品数据项
    /// </summary>
    public class SaleGoodsItem
    {
        /// <summary>
        /// 物品的数据库ID
        /// </summary>
        public int GoodsDbID = 0;

        /// <summary>
        /// 出售的物品的数据
        /// </summary>
        public GoodsData SalingGoodsData = null;

        /// <summary>
        /// 出售者的角色数据
        /// </summary>
        public KPlayer Client = null;
    }

    /// <summary>
    /// 出售的物品的管理
    /// </summary>
    public class SaleGoodsManager
    {
        /// <summary>
        /// 缓存的数据
        /// </summary>
        private static List<SaleGoodsData> _SaleGoodsDataList = null;

        /// <summary>
        /// 保存正在出售的物品的词典
        /// </summary>
        private static Dictionary<int, SaleGoodsItem> _SaleGoodsDict = new Dictionary<int, SaleGoodsItem>();

        /// <summary>
        /// 添加出售的物品项
        /// </summary>
        /// <param name="saleGoodsItem"></param>
        public static void AddSaleGoodsItem(SaleGoodsItem saleGoodsItem)
        {
            if (Global.Flag_MUSale) SaleManager.AddSaleGoodsItem(saleGoodsItem);
            lock (_SaleGoodsDict)
            {
                _SaleGoodsDict[saleGoodsItem.GoodsDbID] = saleGoodsItem;
                _SaleGoodsDataList = null; //强迫刷新
            }
        }

        /// <summary>
        /// 将角色的所有出售的物品加入管理中
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        public static void AddSaleGoodsItems(KPlayer client)
        {
            List<GoodsData> goodsDataList = client.SaleGoodsDataList;
            if (null != goodsDataList)
            {
                lock (goodsDataList)
                {
                    for (int i = 0; i < goodsDataList.Count; i++)
                    {
                        SaleGoodsItem saleGoodsItem = new SaleGoodsItem()
                        {
                            GoodsDbID = goodsDataList[i].Id,
                            SalingGoodsData = goodsDataList[i],
                            Client = client,
                        };

                        AddSaleGoodsItem(saleGoodsItem);
                    }
                }
            }
        }

        /// <summary>
        /// 删除出售的物品项
        /// </summary>
        /// <param name="saleGoodsItem"></param>
        public static SaleGoodsItem RemoveSaleGoodsItem(int goodsDbID)
        {
            if (Global.Flag_MUSale) SaleManager.RemoveSaleGoodsItem(goodsDbID);
            lock (_SaleGoodsDict)
            {
                SaleGoodsItem saleGoodsItem = null;
                if (_SaleGoodsDict.TryGetValue(goodsDbID, out saleGoodsItem))
                {
                    _SaleGoodsDict.Remove(goodsDbID);
                }

                _SaleGoodsDataList = null; //强迫刷新
                return saleGoodsItem;
            }
        }

        /// <summary>
        /// 删除角色的所有出售的物品项
        /// </summary>
        /// <param name="saleGoodsItem"></param>
        public static void RemoveSaleGoodsItems(KPlayer client)
        {
            List<GoodsData> goodsDataList = client.SaleGoodsDataList;
            if (null != goodsDataList)
            {
                lock (goodsDataList)
                {
                    for (int i = 0; i < goodsDataList.Count; i++)
                    {
                        RemoveSaleGoodsItem(goodsDataList[i].Id);
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有的出售的物品的列表(有返回的最大数限制)
        /// </summary>
        /// <returns></returns>
        public static List<SaleGoodsData> GetSaleGoodsDataList()
        {            
            lock (_SaleGoodsDict)
            {
                if (null != _SaleGoodsDataList)
                {
                    return _SaleGoodsDataList; //防止频繁计算
                }

                List<SaleGoodsData> saleGoodsDataList = new List<SaleGoodsData>();

                foreach (var saleGoodsItem in _SaleGoodsDict.Values)
                {
                    saleGoodsDataList.Add(new SaleGoodsData()
                    {
                        GoodsDbID = saleGoodsItem.GoodsDbID,
                        SalingGoodsData = saleGoodsItem.SalingGoodsData,
                        RoleID = saleGoodsItem.Client.RoleID,
                        RoleName = Global.FormatRoleName(saleGoodsItem.Client, saleGoodsItem.Client.RoleName),
                        RoleLevel = saleGoodsItem.Client.m_Level,
                    });

                    if (saleGoodsDataList.Count >= (int)SaleGoodsConsts.MaxReturnNum)
                    {
                        break;
                    }
                }

                _SaleGoodsDataList = saleGoodsDataList;
                return saleGoodsDataList;
            }            
        }

        /// <summary>
        /// 查找指定物品的列表(有返回的最大数限制)
        /// </summary>
        /// <returns></returns>
        public static List<SaleGoodsData> FindSaleGoodsDataList(Dictionary<int, bool> goodsIDDict)
        {
            lock (_SaleGoodsDict)
            {
                List<SaleGoodsData> saleGoodsDataList = new List<SaleGoodsData>();

                foreach (var saleGoodsItem in _SaleGoodsDict.Values)
                {
                    if (!goodsIDDict.ContainsKey(saleGoodsItem.SalingGoodsData.GoodsID))
                    {
                        continue; //跳过
                    }

                    saleGoodsDataList.Add(new SaleGoodsData()
                    {
                        GoodsDbID = saleGoodsItem.GoodsDbID,
                        SalingGoodsData = saleGoodsItem.SalingGoodsData,
                        RoleID = saleGoodsItem.Client.RoleID,
                        RoleName = Global.FormatRoleName(saleGoodsItem.Client, saleGoodsItem.Client.RoleName),
                        RoleLevel = saleGoodsItem.Client.m_Level,
                    });

                    if (saleGoodsDataList.Count >= (int)SaleGoodsConsts.MaxReturnNum)
                    {
                        break;
                    }
                }

                return saleGoodsDataList;
            }   
        }

        /// <summary>
        /// 查找指定物品的列表(有返回的最大数限制)
        /// </summary>
        /// <returns></returns>
        public static List<SaleGoodsData> FindSaleGoodsDataListByRoleName(string searchText)
        {
            lock (_SaleGoodsDict)
            {
                List<SaleGoodsData> saleGoodsDataList = new List<SaleGoodsData>();

                foreach (var saleGoodsItem in _SaleGoodsDict.Values)
                {
                    if (-1 == saleGoodsItem.Client.RoleName.IndexOf(searchText))
                    {
                        continue;
                    }

                    saleGoodsDataList.Add(new SaleGoodsData()
                    {
                        GoodsDbID = saleGoodsItem.GoodsDbID,
                        SalingGoodsData = saleGoodsItem.SalingGoodsData,
                        RoleID = saleGoodsItem.Client.RoleID,
                        RoleName = Global.FormatRoleName(saleGoodsItem.Client, saleGoodsItem.Client.RoleName),
                        RoleLevel = saleGoodsItem.Client.m_Level,
                    });

                    if (saleGoodsDataList.Count >= (int)SaleGoodsConsts.MaxReturnNum)
                    {
                        break;
                    }
                }

                return saleGoodsDataList;
            }
        }

        #region 出售的摆摊铜钱唯一ID的生成

        /// <summary>
        /// 线程锁对象
        /// </summary>
        private static Object Mutex = new Object();

        /// <summary>
        /// 起始的ID值
        /// </summary>
        private static int BaseBaiTanJinBiID = -1;

        /// <summary>
        /// 获取一个新的基于ID
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public static int GetNewBaiTanJinBiID()
        {
            lock (Mutex)
            {
                int id = BaseBaiTanJinBiID;
                BaseBaiTanJinBiID--;
                return id;
            }
        }

        #endregion 出售的摆摊铜钱唯一ID的生成
    }
}
