using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Server.Protocol;
using Server.Tools;
//using System.Windows.Forms;
using GameDBServer.DB;
using Server.Data;
using ProtoBuf;
using GameDBServer.Logic;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 礼品码项
    /// </summary>
    public class LiPinMaItem
    {
        /// <summary>
        /// 礼品码字符串
        /// </summary>
        public string LiPinMa = "";

        /// <summary>
        /// 送礼活动ID
        /// </summary>
        public int HuodongID = 0;

        /// <summary>
        /// 单个礼品码的最大使用次数
        /// </summary>
        public int MaxNum = 0;

        /// <summary>
        /// 单个礼品码的已经使用次数
        /// </summary>
        public int UsedNum = 0;

        /// <summary>
        /// 礼品码的平台ID
        /// </summary>
        public int PingTaiID = 0;

        /// <summary>
        ///平台ID的 礼品码是否可以重复使用
        /// </summary>
        public int PingTaiRepeat = 0;
    };

    /// <summary>
    /// 礼品码管理
    /// </summary>
    public class LiPinMaManager
    {
        /// <summary>
        /// 线程互斥对象
        /// </summary>
        private static object Mutex = new object();

        /// <summary>
        /// 礼品码管理字典
        /// </summary>
        private static Dictionary<string, LiPinMaItem> _LiPinMaDict = null;

        /// <summary>
        /// 从数据库中加载品码
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static void LoadLiPinMaDB(DBManager dbMgr)
        {
            //查询礼品码
            _LiPinMaDict = DBQuery.QueryLiPinMaDict(dbMgr);
        }

        /// <summary>
        /// 从文件中重新导入礼品码(会清空现有的数据库中的礼品码的记录)
        /// </summary>
        /// <param name="dbMgr"></param>
        public static void LoadLiPinMaFromFile(DBManager dbMgr, bool toAppend = false)
        {
            try
            {
                //判断新的礼品码文件是否存在
                if (!File.Exists("./礼品码_导入文件.txt"))
                {
                    return;
                }

                StreamReader sr = new StreamReader("./礼品码_导入文件.txt", Encoding.GetEncoding("gb2312"));
                if (null == sr)
                {
                    return;
                }

                //如果不是追加，则清空原有数据
                if (!toAppend)
                {
                    //先清空原有的礼品码
                    _LiPinMaDict = null;

                    //清空礼品码的数据
                    DBWriter.ClearAllLiPinMa(dbMgr);
                }

                //查询礼品码
                Dictionary<string, LiPinMaItem> liPinMaDict = new Dictionary<string, LiPinMaItem>();

                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] sa = line.Split(' ');
                    if (sa.Length != 5)
                    {
                        continue;
                    }

                    //插入一个新的礼品码的数据
                    DBWriter.InsertNewLiPinMa(dbMgr, sa[0], sa[1], sa[2], sa[3], sa[4]);

                    LiPinMaItem liPinMaItem = new LiPinMaItem()
                    {
                        LiPinMa = sa[0],
                        HuodongID = Convert.ToInt32(sa[1]),
                        MaxNum = Convert.ToInt32(sa[2]),
                        UsedNum = 0,
                        PingTaiID = Convert.ToInt32(sa[3]),
                        PingTaiRepeat = Convert.ToInt32(sa[4]),
                    };

                    liPinMaDict[liPinMaItem.LiPinMa] = liPinMaItem;
                }

                sr.Close();
                sr = null;

                //如果不是追加，则清空原有数据
                if (!toAppend || null == _LiPinMaDict)
                {
                    //重新赋值
                    _LiPinMaDict = liPinMaDict;
                }
                else
                {
                    Dictionary<string, LiPinMaItem> oldLiPinMaDict = _LiPinMaDict;
                    foreach (var key in liPinMaDict.Keys)
                    {
                        LiPinMaItem liPinMaItem = liPinMaDict[key];

                        //防止多用户重入
                        lock (Mutex)
                        {
                            oldLiPinMaDict[key] = liPinMaItem;
                        }
                    }
                }

                //将已经导入的礼品码文件删除
                System.IO.File.Delete("./礼品码_导入文件.txt");
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                    // 格式化异常错误信息
                    DataHelper.WriteFormatExceptionLog(ex, "", false);
                    //throw ex;
                //});
            }
        }

        /// <summary>
        /// 获取某个礼品码的平台信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int GetLiPinMaPingTaiID(DBManager dbMgr, int songLiID, string liPinMa)
        {
            if (null == _LiPinMaDict) return -1010;
            Dictionary<string, LiPinMaItem> liPinMaDict = _LiPinMaDict; //先得到

            liPinMa = liPinMa.ToUpper(); //转成大写

            //防止多用户重入
            lock (Mutex)
            {
                LiPinMaItem liPinMaItem = null;
                if (!liPinMaDict.TryGetValue(liPinMa, out liPinMaItem))
                {
                    return -1020;
                }

                return liPinMaItem.PingTaiID;
            }
        }

        /// <summary>
        /// 使用某个礼品码
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int UseLiPinMa(DBManager dbMgr, int roleID, int songLiID, string liPinMa, bool insertLiPinMa = false)
        {
            if (null == _LiPinMaDict) return -1010;
            Dictionary<string, LiPinMaItem> liPinMaDict = _LiPinMaDict; //先得到

            int usedNum = 0;
            liPinMa = liPinMa.ToUpper(); //转成大写

            //防止多用户重入
            lock (Mutex)
            {
                LiPinMaItem liPinMaItem = null;
                if (!liPinMaDict.TryGetValue(liPinMa, out liPinMaItem))
                {
                    return -1020;
                }

                if (liPinMaItem.HuodongID != songLiID)
                {
                    return -1030;
                }

                if (liPinMaItem.MaxNum > 0 && liPinMaItem.UsedNum >= liPinMaItem.MaxNum)
                {
                    return -1040;
                }

                //如果不允许平台内的礼品码重复领取
                if (liPinMaItem.PingTaiRepeat <= 0)
                {
                    //通过活动ID查询平台ID
                    int pingTaiID = DBQuery.QueryPingTaiIDByHuoDongID(dbMgr, liPinMaItem.HuodongID, roleID, liPinMaItem.PingTaiID);
                    if (pingTaiID == liPinMaItem.PingTaiID)
                    {
                        return -10000;
                    }
                }

                //添加使用礼品码的平台记录
                DBWriter.AddUsedLiPinMa(dbMgr, liPinMaItem.HuodongID, liPinMaItem.LiPinMa, liPinMaItem.PingTaiID, roleID);

                liPinMaItem.UsedNum++;
                usedNum = liPinMaItem.UsedNum;
            }

            //修改一个礼品码的使用次数
            DBWriter.UpdateLiPinMaUsedNum(dbMgr, liPinMa, usedNum);

            return 0;
        }

        /// <summary>
        /// 获取某个礼品码的平台信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int GetLiPinMaPingTaiID2(DBManager dbMgr, int songLiID, string liPinMa, int roleZoneID)
        {
            liPinMa = liPinMa.ToUpper(); //转成大写

            int ptid = -1, ptrepeat = 0, zoneID = 0, maxUseNum = 0;
            if (!LiPinMaParse.ParseLiPinMa2(liPinMa, out ptid, out ptrepeat, out zoneID, out maxUseNum))
            {
                return -1020;
            }

            if (zoneID > 0 && roleZoneID != zoneID)
            {
                return -1021;
            }

            return ptid;
        }

        /// <summary>
        /// 获取某个礼品码的平台信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int GetLiPinMaPingTaiIDNX(DBManager dbMgr, int songLiID, string liPinMa, int roleZoneID)
        {
            liPinMa = liPinMa.ToUpper(); //转成大写

            int ptid = -1, ptrepeat = 0, zoneID = 0, maxUseNum = 0;
            if (!LiPinMaParse.ParseLiPinMaNX2(liPinMa, out ptid, out ptrepeat, out zoneID, out maxUseNum))
            {
                return -1020;
            }

            if (zoneID > 0 && roleZoneID != zoneID)
            {
                return -1021;
            }

            return ptid;
        }

        /// <summary>
        /// 使用某个礼品码(自己生成的特定格式的礼品码)
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int UseLiPinMa2(DBManager dbMgr, int roleID, int songLiID, string liPinMa, int roleZoneID)
        {
            if (null == _LiPinMaDict) return -1010;
            Dictionary<string, LiPinMaItem> liPinMaDict = _LiPinMaDict; //先得到

            int usedNum = 0;
            liPinMa = liPinMa.ToUpper(); //转成大写

            int ptid = -1, ptrepeat = 0, zoneID = 0, maxUseNum = 0;
            if (!LiPinMaParse.ParseLiPinMa2(liPinMa, out ptid, out ptrepeat, out zoneID, out maxUseNum))
            {
                return -1020;
            }

            if (zoneID > 0 && roleZoneID != zoneID)
            {
                return -1021;
            }

            //防止多用户重入
            lock (Mutex)
            {
                LiPinMaItem liPinMaItem = null;
                bool bIsNew = false;
                if (!liPinMaDict.TryGetValue(liPinMa, out liPinMaItem))
                {
                    liPinMaItem = new LiPinMaItem()
                    {
                        LiPinMa = liPinMa,
                        HuodongID = 1,
                        MaxNum = maxUseNum,
                        UsedNum = 0,
                        PingTaiID = ptid,
                        PingTaiRepeat = ptrepeat,
                    };

                    liPinMaDict[liPinMa] = liPinMaItem;
                    bIsNew = true;
                }

                if (liPinMaItem.HuodongID != songLiID)
                {
                    return -1030;
                }

                if (liPinMaItem.MaxNum > 0 && liPinMaItem.UsedNum >= liPinMaItem.MaxNum)
                {
                    return -1040;
                }

                //如果不允许平台内的礼品码重复领取
                if (liPinMaItem.PingTaiRepeat <= 0)
                {
                    //通过活动ID查询平台ID
                    int pingTaiID = DBQuery.QueryPingTaiIDByHuoDongID(dbMgr, liPinMaItem.HuodongID, roleID, liPinMaItem.PingTaiID);
                    if (pingTaiID == liPinMaItem.PingTaiID)
                    {
                        if (liPinMaItem.MaxNum > 1 && bIsNew)
                        {
                            int nUseNum = DBQuery.QueryUseNumByHuoDongID(dbMgr, liPinMaItem.HuodongID, roleID, liPinMaItem.PingTaiID);
                            if (nUseNum >= liPinMaItem.MaxNum)
                            {
                                return -1040;
                            }
                        }
                        else
                        {
                            return -10000;
                        }
                    }
                }

                //添加使用礼品码的平台记录
                DBWriter.AddUsedLiPinMa(dbMgr, liPinMaItem.HuodongID, liPinMaItem.LiPinMa, liPinMaItem.PingTaiID, roleID);

                liPinMaItem.UsedNum++;
                usedNum = liPinMaItem.UsedNum;
            }

            //插入一个新的礼品码的数据
            DBWriter.InsertNewLiPinMa(dbMgr, liPinMa, songLiID.ToString(), "1", ptid.ToString(), ptrepeat.ToString(), "1");

            return 0;
        }

        /// <summary>
        /// 使用某个礼品码(自己生成的特定格式的礼品码)
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fileName"></param>
        public static int UseLiPinMaNX(DBManager dbMgr, int roleID, int songLiID, string liPinMa, int roleZoneID)
        {
            if (null == _LiPinMaDict) return -1010;
            Dictionary<string, LiPinMaItem> liPinMaDict = _LiPinMaDict; //先得到

            int usedNum = 0;
            liPinMa = liPinMa.ToUpper(); //转成大写

            int ptid = -1, ptrepeat = 0, zoneID = 0, maxUseNum = 0;
            if (!LiPinMaParse.ParseLiPinMaNX2(liPinMa, out ptid, out ptrepeat, out zoneID, out maxUseNum))
            {
                return -1020;
            }

            if (zoneID > 0 && roleZoneID != zoneID)
            {
                return -1021;
            }

            //防止多用户重入
            lock (Mutex)
            {
                LiPinMaItem liPinMaItem = null;
                bool bIsNew = false;
                if (!liPinMaDict.TryGetValue(liPinMa, out liPinMaItem))
                {
                    liPinMaItem = new LiPinMaItem()
                    {
                        LiPinMa = liPinMa,
                        HuodongID = 1,
                        MaxNum = maxUseNum,
                        UsedNum = 0,
                        PingTaiID = ptid,
                        PingTaiRepeat = ptrepeat,
                    };

                    liPinMaDict[liPinMa] = liPinMaItem;
                    bIsNew = true;
                }

                if (liPinMaItem.HuodongID != songLiID)
                {
                    return -1030;
                }

                if (liPinMaItem.MaxNum > 0 && liPinMaItem.UsedNum >= liPinMaItem.MaxNum)
                {
                    return -1040;
                }

                //如果不允许平台内的礼品码重复领取
                if (liPinMaItem.PingTaiRepeat <= 0)
                {
                    //通过活动ID查询平台ID
                    int pingTaiID = DBQuery.QueryPingTaiIDByHuoDongID(dbMgr, liPinMaItem.HuodongID, roleID, liPinMaItem.PingTaiID);
                    if (pingTaiID == liPinMaItem.PingTaiID)
                    {
                        if (liPinMaItem.MaxNum > 1 && bIsNew)
                        {
                            int nUseNum = DBQuery.QueryUseNumByHuoDongID(dbMgr, liPinMaItem.HuodongID, roleID, liPinMaItem.PingTaiID);
                            if (nUseNum >= liPinMaItem.MaxNum)
                            {
                                return -1040;
                            }
                        }
                        else
                        {
                            return -10000;
                        }
                    }
                }

                //添加使用礼品码的平台记录
                DBWriter.AddUsedLiPinMa(dbMgr, liPinMaItem.HuodongID, liPinMaItem.LiPinMa, liPinMaItem.PingTaiID, roleID);

                liPinMaItem.UsedNum++;
                usedNum = liPinMaItem.UsedNum;
            }

            //插入一个新的礼品码的数据
            DBWriter.InsertNewLiPinMa(dbMgr, liPinMa, songLiID.ToString(), "1", ptid.ToString(), ptrepeat.ToString(), "1");

            return 0;
        }
    }
}
