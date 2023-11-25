using GameDBServer.Data;
using GameDBServer.Logic;
using GameDBServer.Logic.Ten;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameDBServer.DB
{
    /// <summary>
    /// 数据库查询(要有控制，禁止频繁查询)
    /// </summary>
    public class DBQuery
    {
        #region 查询操作



        /// <summary>
        /// 查询游戏配置参数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static Dictionary<string, string> QueryGameConfigDict(DBManager dbMgr)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = "SELECT paramname, paramvalue FROM t_config";
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    string paramName = reader["paramname"].ToString();
                    string paramVal = reader["paramvalue"].ToString();
                    dict[paramName] = paramVal;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return dict;
        }

        /// <summary>
        /// 查询元宝充值临时记录表中，新充值的记录的用户ID列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void QueryTempMoney(DBManager dbMgr, List<string> userIDList, List<int> userMoneyList)
        {
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = "SELECT id, uid, addmoney FROM t_tempmoney";
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int minId = int.MaxValue;
                int maxId = 0;
                bool bHadRecord = false;
                while (reader.Read())
                {
                    bHadRecord = true;
                    minId = Math.Min(minId, Convert.ToInt32(reader["id"].ToString()));
                    maxId = Math.Max(maxId, Convert.ToInt32(reader["id"].ToString()));
                    userIDList.Add(reader["uid"].ToString());
                    userMoneyList.Add(Convert.ToInt32(reader["addmoney"].ToString()));

                    LogManager.WriteLog(LogTypes.Error, string.Format("从t_tempmoney 找到 UID={0}，money={1}", reader["uid"].ToString(), Convert.ToInt32(reader["addmoney"].ToString())));
                }

                /*if (bHadRecord)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("本次处理的t_tempmoney id范围[{0} - {1}]", minId, maxId));
                }*/
                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                //清空表
                if (userIDList.Count > 0)
                {
                    cmdText = string.Format("DELETE FROM t_tempmoney WHERE id<={0}", maxId);
                    cmd = new MySQLCommand(cmdText, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd = null;

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);
                }
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        /// <summary>
        /// 查询礼品码
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static Dictionary<string, LiPinMaItem> QueryLiPinMaDict(DBManager dbMgr)
        {
            Dictionary<string, LiPinMaItem> dict = new Dictionary<string, LiPinMaItem>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = "SELECT lipinma, huodongid, maxnum, usednum, ptid, ptrepeat FROM t_linpinma";
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    LiPinMaItem liPinMaItem = new LiPinMaItem()
                    {
                        LiPinMa = reader["lipinma"].ToString(),
                        HuodongID = Convert.ToInt32(reader["huodongid"].ToString()),
                        MaxNum = Convert.ToInt32(reader["maxnum"].ToString()),
                        UsedNum = Convert.ToInt32(reader["usednum"].ToString()),
                        PingTaiID = Convert.ToInt32(reader["ptid"].ToString()),
                        PingTaiRepeat = Convert.ToInt32(reader["ptrepeat"].ToString()),
                    };

                    dict[liPinMaItem.LiPinMa] = liPinMaItem;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return dict;
        }

        /// <summary>
        /// 根据角色名称查询平台用户ID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static string QueryUserIDByRoleName(DBManager dbMgr, string otherRoleName, int zoneID)
        {
            string ret = "";
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT userid FROM t_roles WHERE rname='{0}' AND zoneid={1}", otherRoleName, zoneID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    ret = reader["userid"].ToString();
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 根据平台用户ID查询元宝和真实的充值钱数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void QueryUserMoneyByUserID(DBManager dbMgr, string userID, out int userMoney, out int realMoney)
        {
            userMoney = 0;
            realMoney = 0;

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT money, realmoney FROM t_money WHERE userid='{0}'", userID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    userMoney = Convert.ToInt32(reader["money"].ToString());
                    realMoney = Convert.ToInt32(reader["realmoney"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        /// <summary>
        /// 根据平台用户ID查询今日元宝和真实的充值钱数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void QueryTodayUserMoneyByUserID(DBManager dbMgr, string userID, int zoneID, out int userMoney, out int realMoney)
        {
            userMoney = 0;
            realMoney = 0;

            DateTime now = DateTime.Now;
            string todayStart = string.Format("{0:0000}-{1:00}-{2:00} 00:00:00", now.Year, now.Month, now.Day);
            string todayEnd = string.Format("{0:0000}-{1:00}-{2:00} 23:59:59", now.Year, now.Month, now.Day);

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT SUM(amount) AS totalmoney FROM t_inputlog WHERE u='{0}' AND zoneID={1} AND inputtime>='{2}' AND inputtime<='{3}'", userID, zoneID, todayStart, todayEnd);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        userMoney = Convert.ToInt32(reader["totalmoney"].ToString());
                        realMoney = userMoney;
                    }
                    catch (Exception)
                    {
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        /// <summary>
        /// 根据平台用户ID查询今日元宝和真实的充值钱数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void QueryTodayUserMoneyByUserID2(DBManager dbMgr, string userID, int zoneID, out int userMoney, out int realMoney)
        {
            userMoney = 0;
            realMoney = 0;

            DateTime now = DateTime.Now;
            string todayStart = string.Format("{0:0000}-{1:00}-{2:00} 00:01:01", now.Year, now.Month, now.Day);
            string todayEnd = string.Format("{0:0000}-{1:00}-{2:00} 23:59:59", now.Year, now.Month, now.Day);

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT SUM(amount) AS totalmoney FROM t_inputlog2 WHERE u='{0}' AND zoneID={1} AND inputtime>='{2}' AND inputtime<='{3}'", userID, zoneID, todayStart, todayEnd);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        userMoney = Convert.ToInt32(reader["totalmoney"].ToString());
                        realMoney = userMoney;
                    }
                    catch (Exception)
                    {
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        /// <summary>
        /// 获取角色参数表
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static String GetRoleParamByName(DBManager dbMgr, int roleID, string paramName)
        {
            List<PaiHangItemData> list = new List<PaiHangItemData>();
            MySQLConnection conn = null;

            String sValue = "";

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                RoleParamType roleParamType = RoleParamNameInfo.GetRoleParamType(paramName);
                string cmdText = string.Format("SELECT p.rid, p.{2} FROM {3} as p "
                    + " where p.{4}={0} and p.rid={1}", roleParamType.KeyString, roleID, roleParamType.ColumnName, roleParamType.TableName, roleParamType.IdxName);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    sValue = reader[1].ToString();
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return sValue;
        }

        #endregion 查询操作

        #region 帮会相关

        public static void QueryPreDeleteRoleDict(DBManager dbMgr, Dictionary<int, DateTime> preDeleteRoleDict)
        {
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid, predeltime FROM t_roles WHERE isdel=0 and predeltime IS NOT NULL");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    int rid = Convert.ToInt32(reader["rid"].ToString());

                    DateTime preDelTime;
                    if (DateTime.TryParse(reader["predeltime"].ToString(), out preDelTime))
                    {
                        preDeleteRoleDict[rid] = preDelTime;
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        #endregion 帮会相关

        #region 限时抢购相关

        /// <summary>
        /// 获取某角色某项抢购购买数据记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int QueryQiangGouBuyItemNumByRoleID(DBManager dbMgr, int roleID, int goodsID, int qiangGouID, int random, int actStartDay)
        {
            int count = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = "";
                if (random <= 0)
                {
                    cmdText = string.Format("SELECT SUM(goodsnum) AS totalgoodsnum FROM t_qianggoubuy WHERE rid={0} and goodsid={1} and qianggouid={2} and actstartday={3}", roleID, goodsID, qiangGouID, actStartDay);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    string todayStart = string.Format("{0:0000}-{1:00}-{2:00} 00:01:01", now.Year, now.Month, now.Day);
                    string todayEnd = string.Format("{0:0000}-{1:00}-{2:00} 23:59:59", now.Year, now.Month, now.Day);
                    cmdText = string.Format("SELECT SUM(goodsnum) AS totalgoodsnum FROM t_qianggoubuy WHERE rid={0} and goodsid={1} and qianggouid={2} and buytime>='{3}' and buytime<='{4}'",
                        roleID, goodsID, qiangGouID, todayStart, todayEnd);
                }

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        count = Convert.ToInt32(reader["totalgoodsnum"].ToString());
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return count;
        }

        /// <summary>
        /// 获取某项抢购购买数据记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int QueryQiangGouBuyItemNum(DBManager dbMgr, int goodsID, int qiangGouID, int random, int actStartDay)
        {
            int count = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = "";
                if (random <= 0)
                {
                    cmdText = string.Format("SELECT SUM(goodsnum) AS totalgoodsnum FROM t_qianggoubuy WHERE goodsid={0} and qianggouid={1} and actstartday={2}", goodsID, qiangGouID, actStartDay);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    string todayStart = string.Format("{0:0000}-{1:00}-{2:00} 00:01:01", now.Year, now.Month, now.Day);
                    string todayEnd = string.Format("{0:0000}-{1:00}-{2:00} 23:59:59", now.Year, now.Month, now.Day);
                    cmdText = string.Format("SELECT SUM(goodsnum) AS totalgoodsnum FROM t_qianggoubuy WHERE goodsid={0} and qianggouid={1} and buytime>='{2}' and buytime<='{3}'",
                        goodsID, qiangGouID, todayStart, todayEnd);
                }

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        count = Convert.ToInt32(reader["totalgoodsnum"].ToString());
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return count;
        }

        #endregion 限时抢购相关

        #region 礼品码相关

        /// <summary>
        /// 通过活动ID查询平台ID
        /// </summary>
        /// <param name="huodongID"></param>
        /// <returns></returns>
        public static int QueryPingTaiIDByHuoDongID(DBManager dbMgr, int huodongID, int rid, int pingTaiID)
        {
            int ret = -1;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT ptid FROM t_usedlipinma WHERE huodongid={0} AND rid={1} AND ptid={2}", huodongID, rid, pingTaiID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    ret = Convert.ToInt32(reader["ptid"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 通过活动ID查询平台ID
        /// </summary>
        /// <param name="huodongID"></param>
        /// <returns></returns>
        public static int QueryUseNumByHuoDongID(DBManager dbMgr, int huodongID, int rid, int pingTaiID)
        {
            int ret = -1;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT count(ptid) as ptidCount FROM t_usedlipinma WHERE huodongid={0} AND rid={1} AND ptid={2}", huodongID, rid, pingTaiID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    ret = Convert.ToInt32(reader["ptidCount"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        #endregion 礼品码相关

        #region 充值相关

        /// <summary>
        /// 通过用户ID查询充值记录
        /// </summary>
        /// <param name="huodongID"></param>
        /// <returns></returns>
        public static int QueryTotalChongZhiMoney(DBManager dbMgr, string userID, int zoneID)
        {
            int ret = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT SUM(amount) AS totalmoney FROM t_inputlog WHERE u='{0}' AND zoneid={1}", userID, zoneID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        ret = Convert.ToInt32(reader["totalmoney"].ToString());
                    }
                    catch (Exception)
                    {
                        ret = 0;
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 通过用户ID和充值钱数查指定的充值记录
        /// </summary>
        /// <param name="huodongID"></param>
        /// <returns></returns>
        public static int QueryChargeMoney(DBManager dbMgr, string userID, int zoneID, int addmoney)
        {
            int ret = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT count(amount) as num FROM t_inputlog WHERE u='{0}' AND zoneid={1} AND amount={2}", userID, zoneID, addmoney);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    try
                    {
                        ret = Convert.ToInt32(reader["num"].ToString());
                    }
                    catch (Exception)
                    {
                        ret = 0;
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// Truy vấn tổng số Đồng hiện có trong Server
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static long QueryServerTotalUserMoney()
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                return conn.GetSingleLong("SELECT IFNULL(SUM(money),0) as money FROM t_money");
            }
        }

        #endregion 充值相关

        #region 活动奖励相关

        /// <summary>
        /// 返回充值排行信息 key:userid, value:totalmoney 的list,排行小于等于maxPaiHang的被返回,如果两个玩家充值额相等，先达到值的排名靠前
        /// dictionay顺序未知，直接返回排好序的list,这个函数返回的排行值是具体的真实货币值，外部使用时需要自行转换为元宝数，不在这转换
        /// 此函数作为基础函数，实际操作请使用带有缓存的GetInputKingPaiHangListByHuoDongLimit
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime">字符串时间</param>
        /// <param name="endTime">字符串时间</param>
        /// <param name="maxPaiHang">必须大于等于1,默认返回排名前三的信息</param>
        /// <returns></returns>
        public static List<InputKingPaiHangData> GetUserInputPaiHang(DBManager dbMgr, string startTime, string endTime, int maxPaiHang = 3)
        {
            List<InputKingPaiHangData> lsPaiHang = new List<InputKingPaiHangData>();

            if (maxPaiHang < 1)
            {
                return lsPaiHang;
            }

            string userid = ""; int totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                //这个位置需要采用unix时间戳进行定位,普通情况下，unix时间戳和inputtime是同一个值，单inputtime时间戳比unix要大一点
                //对用户充值而言，采用unix时间戳比较准确,此外，默认任务玩家充值信息只在t_inputlog表中有，而gm充值信息只在t_inputlog2表中，
                //如果存在某个玩家在两个表中都有充值，该玩家可能有两条数据位于取出的数据中，这时，需要做过滤操作，所以，返回数据最大行数乘以2,
                //然后再对返回的数据进行重复玩家过滤

                //string cmdText = string.Format("SELECT u, sum(amount) as totalmoney, max(time) as time from t_inputlog where t_inputlog.u IN (select DISTINCT  userid from t_roles where t_roles.isdel=0) and inputtime>='{0}' and inputtime<='{1}' and result='success' GROUP by u " +
                //                 " union " +
                //                 " SELECT u, sum(amount) as totalmoney, max(time) as time from t_inputlog2 where t_inputlog2.u IN (select DISTINCT userid from t_roles where t_roles.isdel=0) and  inputtime>='{0}' and inputtime<='{1}' and result='success' GROUP by u order by totalmoney desc,time asc " +
                //                 " limit 0, {2} ", startTime, endTime, maxPaiHang * 2);

                // 直接在数据库中联合两个表进行排序
                string cmdText = string.Format(@"SELECT u, sum(totalmoney) as totalmoney, max(time) from
                    (
                    SELECT u, sum(amount) as totalmoney, max(time) as time from t_inputlog where t_inputlog.u IN (select DISTINCT  userid from t_roles where t_roles.isdel=0) and inputtime>='{0}' and inputtime<='{1}' and result='success'
                    GROUP by u
                    union ALL
                    SELECT u, sum(amount) as totalmoney, max(time) as time from t_inputlog2 where t_inputlog2.u IN (select DISTINCT  userid from t_roles where t_roles.isdel=0) and inputtime>='{0}' and inputtime<='{1}' and result='success'
                    GROUP by u
                    ) a group by u order by  sum(totalmoney) desc,max(time) ASC limit {2};", startTime, endTime, maxPaiHang);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //最多返回maxPaiHang 条数据
                while (reader.Read())
                {
                    count++;

                    userid = reader["u"].ToString();
                    totalmoney = Convert.ToInt32(reader["totalmoney"].ToString());
                    InputKingPaiHangData phData = new InputKingPaiHangData
                    {
                        UserID = userid,
                        PaiHang = count,
                        PaiHangTime = now,
                        PaiHangValue = totalmoney
                    };

                    lsPaiHang.Add(phData);
                }

                //对返回结果进行排序，按照PaiHangValue 由大到小排序，然后更新PaiHang值

                Comparison<InputKingPaiHangData> com = new Comparison<InputKingPaiHangData>(InputKingPaiHangDataCompare);
                lsPaiHang.Sort(com);

                for (int n = 0; n < lsPaiHang.Count; n++)
                {
                    lsPaiHang[n].PaiHang = n + 1;
                    // 排行榜统一在这里转换成钻石
                    lsPaiHang[n].PaiHangValue = Global.TransMoneyToYuanBao(lsPaiHang[n].PaiHangValue);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lsPaiHang;
        }

        /// <summary>
        /// 这样写导致排序时降序排列
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int InputKingPaiHangDataCompare(InputKingPaiHangData left, InputKingPaiHangData right)
        {
            return right.PaiHangValue - left.PaiHangValue;
        }

        /// <summary>
        /// Thực hiện truy vấn mysql lấy ra tổng lượng đã nạp trong ngày
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetUserInputMoney(DBManager dbMgr, string userid, int zoneid, string startTime, string endTime)
        {
            int totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT u, sum(amount) as totalmoney, max(time) as time from t_inputlog where inputtime>='{0}' and inputtime<='{1}' and u='{2}' and zoneid={3} and result='success' GROUP by u ", startTime, endTime, userid, zoneid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    totalmoney += Convert.ToInt32(reader["totalmoney"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return totalmoney;
        }

        /// <summary>
        /// Lấy ra tổng giá trị đã đánh dấu trong tháng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="RoleID"></param>
        /// <param name="RecoreType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ZoneID"></param>
        /// <returns></returns>
        public static int GetSumRecoreValue(DBManager dbMgr, int RoleID, int RecoreType, string startTime, string endTime, int ZoneID)
        {
            int TotalValue = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = "SELECT RoleID, SUM(ValueRecore) as TotalValue from t_recore where DateRecore>='" + startTime + "' and DateRecore<='" + endTime + "' and RoleID= " + RoleID + " and ZoneID= " + ZoneID + " and RecoryType = " + RecoreType + " GROUP by RoleID";

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    TotalValue += Convert.ToInt32(reader["TotalValue"].ToString());
                }


                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return TotalValue;
        }

        /// <summary>
        /// Lấy ra giá trị trước đó đã nhận hay chưa
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="RoleID"></param>
        /// <param name="TimeRanger"></param>
        /// <param name="RecoreType"></param>
        /// <returns></returns>
        public static int GetTMark(DBManager dbMgr, int RoleID, string TimeRanger, int RecoreType)
        {
            int T_MarkValue = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = "SELECT MarkValue from t_mark where RoleID = " + RoleID + " and TimeRanger = '" + TimeRanger + "' and MarkType = " + RecoreType + "";

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    T_MarkValue += Convert.ToInt32(reader["MarkValue"].ToString());
                }

                // GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return T_MarkValue;
        }

        /// <summary>
        /// 返回角色奖励领取记录,主要用于针对角色的活动奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetAwardHistoryForRole(DBManager dbMgr, int rid, int zoneid, int activitytype, string keystr, out int hasgettimes, out string lastgettime)
        {
            hasgettimes = 0;
            lastgettime = "";
            int ret = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT hasgettimes, lastgettime from t_huodongawardrolehist where rid={0} and zoneid={1} and activitytype={2} and keystr='{3}' ",
                    rid, zoneid, activitytype, keystr);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                //这儿应该最多只有一条数据
                if (reader.Read())
                {
                    hasgettimes = Convert.ToInt32(reader["hasgettimes"].ToString());
                    lastgettime = reader["lastgettime"].ToString();
                    ret = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 返回角色奖励领取记录,主要用于针对用户账号【一个账号会有多个角色】的活动奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetAwardHistoryForUser(DBManager dbMgr, string userid, int activitytype, string keystr, out long hasgettimes, out string lastgettime)
        {
            hasgettimes = 0;
            lastgettime = "";
            int ret = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT hasgettimes, lastgettime from t_huodongawarduserhist where userid='{0}' and activitytype={1} and keystr='{2}' ",
                    userid, activitytype, keystr);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                //这儿应该最多只有一条数据
                if (reader.Read())
                {
                    hasgettimes = Convert.ToInt64(reader["hasgettimes"].ToString());
                    lastgettime = reader["lastgettime"].ToString();
                    ret = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 返回角色奖励领取记录,主要用于针对用户账号【一个账号会有多个角色】的活动奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetAwardHistoryForUser(DBManager dbMgr, string userid, int activitytype, string keystr, out int hasgettimes, out string lastgettime)
        {
            hasgettimes = 0;
            lastgettime = "";
            int ret = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT hasgettimes, lastgettime from t_huodongawarduserhist where userid='{0}' and activitytype={1} and keystr='{2}' ",
                    userid, activitytype, keystr);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                //这儿应该最多只有一条数据
                if (reader.Read())
                {
                    hasgettimes = Convert.ToInt32(reader["hasgettimes"].ToString());
                    lastgettime = reader["lastgettime"].ToString();
                    ret = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// 返回角色活动排行列表数据,最靠近且MidTime的排行信息,最多maxPaiHang条数据,即排行最大值maxPaiHang
        /// 如果两个排行时间离MidTime一样进，则取比midtime大一点的那个时间的排行信息
        /// 没有对活动的特殊限制条件进行过滤，过滤条件可能变化，所以由外部临时处理
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<HuoDongPaiHangData> GetActivityPaiHangListNearMidTime(DBManager dbMgr, int huoDongType, string midTime, int maxPaiHang = 10)
        {
            List<HuoDongPaiHangData> lsPaiHang = new List<HuoDongPaiHangData>();

            MySQLConnection conn = null;
            try
            {
                string minTime = DateTime.Parse(midTime).AddHours(-36).ToString();
                string maxTime = DateTime.Parse(midTime).AddHours(36).ToString();

                conn = dbMgr.DBConns.PopDBConnection();

                //时间差值diff升序,时间降序,paihang升序，保证取到的数据是最接近midTime从小到大的排行信息，不需要用paihang<=maxPaiHang，采用排行递增排序和条数限制就行
                string cmdText = string.Format("SELECT rid, rname, zoneid, type, paihang, phvalue, paihangtime, ABS(datediff(paihangtime, '{0}')) as diff " +
                    " from t_huodongpaihang where type={1} and paihangtime<='{2}' and paihangtime>='{3}' ORDER by diff ASC, paihangtime desc, paihang ASC LIMIT 0, {4}",
                    midTime, huoDongType, maxTime, minTime, maxPaiHang);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                string sPaiHangTime = "";

                //查询得到的数据以最靠近midTime的排行为准，写t_huodongpaihang时采用同一个paihangtime，这儿使用paihangtime进行不一致的排行时间过滤
                while (reader.Read())
                {
                    HuoDongPaiHangData ph = new HuoDongPaiHangData();
                    ph.RoleID = Convert.ToInt32(reader["rid"].ToString());
                    ph.RoleName = reader["rname"].ToString();
                    ph.ZoneID = Convert.ToInt32(reader["zoneid"].ToString());
                    ph.Type = Convert.ToInt32(reader["type"].ToString());
                    ph.PaiHang = Convert.ToInt32(reader["paihang"].ToString());
                    ph.PaiHangValue = Convert.ToInt32(reader["phvalue"].ToString());
                    ph.PaiHangTime = reader["paihangtime"].ToString();

                    //确保返回的数据是同一时间点排行，尽管数量可能比maxPaiHang小
                    if (string.IsNullOrEmpty(sPaiHangTime))
                    {
                        sPaiHangTime = ph.PaiHangTime;
                    }
                    else if (string.Compare(sPaiHangTime, ph.PaiHangTime) != 0)
                    {
                        break;
                    }

                    lsPaiHang.Add(ph);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lsPaiHang;
        }

        #endregion 活动奖励相关

        #region 物品限购

        /// <summary>
        /// 通过角色ID和物品ID查询物品每日的已经购买数量
        /// </summary>
        /// <param name="huodongID"></param>
        /// <returns></returns>
        public static int QueryLimitGoodsUsedNumByRoleID(DBManager dbMgr, int roleID, int goodsID, out int dayID, out int usedNum)
        {
            dayID = 0;
            usedNum = 0;

            int ret = -1;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT dayid, usednum FROM t_limitgoodsbuy WHERE rid={0} AND goodsid={1}", roleID, goodsID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    dayID = Convert.ToInt32(reader["dayid"].ToString());
                    usedNum = Convert.ToInt32(reader["usednum"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                ret = 0;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        #endregion 物品限购

        #region 邮件相关

        /// <summary>
        /// Trả về danh sách Mail thu gọn của người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static List<MailData> GetMailItemDataList(DBManager dbMgr, int rid)
        {
            List<MailData> list = new List<MailData>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT mailid,senderrid,senderrname,sendtime,receiverrid,reveiverrname,readtime,isread," +
                                                " mailtype,hasfetchattachment,subject,content,bound_money,bound_token" +
                                                " from t_mail where receiverrid={0} ORDER by sendtime desc limit 100", rid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    MailData mailItemData = new MailData()
                    {
                        MailID = Convert.ToInt32(reader["mailid"].ToString()),
                        SenderRID = Convert.ToInt32(reader["senderrid"].ToString()),
                        SenderRName = DataHelper.Base64Decode(reader["senderrname"].ToString()),
                        SendTime = reader["sendtime"].ToString(),
                        ReceiverRID = Convert.ToInt32(reader["receiverrid"].ToString()),
                        ReveiverRName = DataHelper.Base64Decode(reader["reveiverrname"].ToString()),
                        ReadTime = reader["readtime"].ToString(),
                        IsRead = Convert.ToInt32(reader["isread"].ToString()),
                        MailType = Convert.ToInt32(reader["mailtype"].ToString()),
                        HasFetchAttachment = Convert.ToInt32(reader["hasfetchattachment"].ToString()),
                        Subject = DataHelper.Base64Decode(reader["subject"].ToString()),
                        Content = "",
                        BoundMoney = Convert.ToInt32(reader["bound_money"].ToString()),
                        BoundToken = Convert.ToInt32(reader["bound_token"].ToString()),
                    };

                    list.Add(mailItemData);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy tổng số vật phẩm đính kèm trong Mail
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="excludeIsRead">排除邮件读取状态(默认排除已读的)</param>
        /// <returns></returns>
        public static int GetMailItemDataCount(DBManager dbMgr, int rid, int excludeReadState = 0, int limitCount = 1)
        {
            MySQLConnection conn = null;
            int count = 0;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT mailid from t_mail where receiverrid={0} and isread<>{1} LIMIT 0,{2}", rid, excludeReadState, limitCount);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    count++;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return count;
        }

        /// <summary>
        /// Lấy thông tin đầy đủ của Mail có ID tương ứng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static MailData GetMailItemData(DBManager dbMgr, int rid, int mailID)
        {
            MailData mailItemData = null;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT mailid,senderrid,senderrname,sendtime,receiverrid,reveiverrname,readtime,isread," +
                                                " mailtype,hasfetchattachment,subject,content,bound_money,bound_token" +
                                                " from t_mail where receiverrid={0} and mailid={1} ORDER by sendtime desc", rid, mailID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                //有且仅有一封
                if (reader.Read())
                {
                    mailItemData = new MailData()
                    {
                        MailID = Convert.ToInt32(reader["mailid"].ToString()),
                        SenderRID = Convert.ToInt32(reader["senderrid"].ToString()),
                        SenderRName = DataHelper.Base64Decode(reader["senderrname"].ToString()),
                        SendTime = reader["sendtime"].ToString(),
                        ReceiverRID = Convert.ToInt32(reader["receiverrid"].ToString()),
                        ReveiverRName = DataHelper.Base64Decode(reader["reveiverrname"].ToString()),
                        ReadTime = reader["readtime"].ToString(),
                        IsRead = Convert.ToInt32(reader["isread"].ToString()),
                        MailType = Convert.ToInt32(reader["mailtype"].ToString()),
                        HasFetchAttachment = Convert.ToInt32(reader["hasfetchattachment"].ToString()),
                        Subject = DataHelper.Base64Decode(reader["subject"].ToString()),
                        Content = DataHelper.Base64Decode(reader["content"].ToString()),
                        BoundMoney = Convert.ToInt32(reader["bound_money"].ToString()),
                        BoundToken = Convert.ToInt32(reader["bound_token"].ToString()),
                    };
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            if (null != mailItemData)
            {
                mailItemData.GoodsList = GetMailGoodsDataList(dbMgr, mailID);
            }

            return mailItemData;
        }

        /// <summary>
        /// Trả về thông tin vật phẩm đính kèm trong thư
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="mailID"></param>
        /// <returns></returns>
        public static List<GoodsData> GetMailGoodsDataList(DBManager dbMgr, int mailID)
        {
            List<GoodsData> list = new List<GoodsData>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format(@"SELECT Id, mailid, goodsid, forge_level, Props, gcount, binding, series, otherpramer, strong from t_mailgoods where mailid={0}", mailID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    GoodsData mailItemData = new GoodsData()
                    {
                        Id = Convert.ToInt32(reader["id"].ToString()),
                        GoodsID = Convert.ToInt32(reader["goodsid"].ToString()),
                        Forge_level = Convert.ToInt32(reader["forge_level"].ToString()),
                        Props = reader["Props"].ToString(),
                        GCount = Convert.ToInt32(reader["gcount"].ToString()),
                        Binding = Convert.ToInt32(reader["binding"].ToString()),
                        Series = Convert.ToInt32(reader["series"].ToString()),
                        Strong = Convert.ToInt32(reader["strong"].ToString()),
                    };
                    byte[] Base64Decode = Convert.FromBase64String(reader["otherpramer"].ToString());
                    mailItemData.OtherParams = DataHelper.BytesToObject<Dictionary<ItemPramenter, string>>(Base64Decode, 0, Base64Decode.Length);

                    list.Add(mailItemData);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return list;
        }

        /*
        /// <summary>
        /// 获取邮件附件数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static List<MailGoodsData> GetMailGoodsDataList(DBManager dbMgr, int mailID)
        {
            List<MailGoodsData> list = new List<MailGoodsData>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format(@"SELECT id,mailid,goodsid,forge_level,quality,Props,gcount,binding,origholenum,rmbholenum,jewellist,addpropindex,bornindex,lucky,
                                                        strong,excellenceinfo,appendproplev,equipchangelife from t_mailgoods where mailid={0}", mailID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    MailGoodsData mailItemData = new MailGoodsData()
                    {
                        Id = Convert.ToInt32(reader["id"].ToString()),
                        GoodsID = Convert.ToInt32(reader["goodsid"].ToString()),
                        Forge_level = Convert.ToInt32(reader["forge_level"].ToString()),
                        Props = reader["Props"].ToString(),
                        GCount = Convert.ToInt32(reader["gcount"].ToString()),
                        Binding = Convert.ToInt32(reader["binding"].ToString()),
                        Strong = Convert.ToInt32(reader["strong"].ToString()),
                    };

                    list.Add(mailItemData);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return list;
        }
        */

        /// <summary>
        /// 扫描新邮件信息，并且写入日志中 返回roleid, mailid 对应的列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static Dictionary<int, int> ScanLastMailIDListFromTable(DBManager dbMgr)
        {
            Dictionary<int, int> lastMailDct = new Dictionary<int, int>();

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                //每次最多处理20条,同一用户取mailid最大的那一条
                string cmdText = string.Format("SELECT MAX(mailid) as mailid, receiverrid from t_mailtemp  GROUP by mailid,receiverrid ORDER by receiverrid asc limit 0, 20");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    int receiverrid = Convert.ToInt32(reader["receiverrid"].ToString());
                    if (!lastMailDct.ContainsKey(receiverrid))
                    {
                        lastMailDct.Add(receiverrid, Convert.ToInt32(reader["mailid"].ToString()));
                    }
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lastMailDct;
        }

        #endregion 邮件相关

        #region 角色ID查询

        /// <summary>
        /// 通过角色名称在数据库查询角色ID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int GetRoleIDByRoleName(DBManager dbMgr, string roleName, int zoneid)
        {
            int rid = -1;

            //传入非法的角色名称直接返回负数
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return rid;
            }

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid from t_roles WHERE rname='{0}' and zoneid={1}", roleName, zoneid);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    rid = Convert.ToInt32(reader["rid"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return rid;
        }

        #endregion 角色ID查询

        #region 数据库ID字段自增长相关

        /// <summary>
        /// 返回最大的mailID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int GetMaxMailID(DBManager dbMgr)
        {
            int maxValue = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT MAX(mailid) as mymaxvalue from t_mail");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    maxValue = Global.SafeConvertToInt32(reader["mymaxvalue"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return maxValue;
        }

        /// <summary>
        /// 返回最大的角色ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int GetMaxRoleID(DBManager dbMgr)
        {
            int maxValue = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT MAX(rid) as mymaxvalue from t_roles");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    maxValue = Global.SafeConvertToInt32(reader["mymaxvalue"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return maxValue;
        }

        public static int GetMaxFamilyID(DBManager dbMgr)
        {
            int maxValue = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT MAX(FamilyID) as mymaxvalue from t_family");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    maxValue = Global.SafeConvertToInt32(reader["mymaxvalue"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return maxValue;
        }

        public static int GetMaxGuildID(DBManager dbMgr)
        {
            int maxValue = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT MAX(GuildID) as mymaxvalue from t_guild");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    maxValue = Global.SafeConvertToInt32(reader["mymaxvalue"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return maxValue;
        }

        /// <summary>
        /// 返回最大的抢购项ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int GetMaxQiangGouItemID(DBManager dbMgr)
        {
            int maxValue = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT MAX(Id) as mymaxvalue from t_qianggouitem");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    maxValue = Global.SafeConvertToInt32(reader["mymaxvalue"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return maxValue;
        }

        #endregion 数据库ID字段自增长相关

        #region 首充大礼/每日充值大礼

        /// <summary>
        /// 通过用户ID，查询是否已经领取过首充大礼
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int GetFirstChongZhiDaLiNum(DBManager dbMgr, string userID)
        {
            int totalNum = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT COUNT(rid) AS totalnum from t_roles WHERE userid='{0}' and cztaskid>0", userID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    if (reader.Read())
                    {
                        totalNum = Convert.ToInt32(reader["totalnum"].ToString());
                    }
                }
                catch (Exception)
                {
                    totalNum = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return totalNum;
        }

        #endregion 首充大礼/每日充值大礼

        #region 开服在线大礼

        /// <summary>
        /// 通过天数，查询那个角色能够获取开服在线大礼
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int GetKaiFuOnlineAwardRoleID(DBManager dbMgr, int dayID, out int totalRoleNum)
        {
            totalRoleNum = 0;
            int roleID = -1;
            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                RoleParamType roleParamType = RoleParamNameInfo.GetRoleParamType(RoleParamName.KaiFuOnlineDayID);
                string cmdText = string.Format("SELECT rid,{0} FROM {1} WHERE {2}={3}", roleParamType.ColumnName, roleParamType.TableName, roleParamType.IdxName, roleParamType.KeyString);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                List<int> roleIDs = new List<int>();

                try
                {
                    while (reader.Read())
                    {
                        int pvalue = Global.SafeConvertToInt32(reader[1].ToString());
                        if (pvalue < dayID)
                        {
                            continue;
                        }

                        roleIDs.Add(Global.SafeConvertToInt32(reader["rid"].ToString()));
                    }
                }
                catch (Exception)
                {
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                if (roleIDs.Count > 0)
                {
                    Random rand = new Random();
                    roleID = roleIDs[rand.Next(0, roleIDs.Count)];
                    totalRoleNum = roleIDs.Count;
                }
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return roleID;
        }

        /// <summary>
        /// 查询开服在线大礼的列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static List<KaiFuOnlineAwardData> GetKaiFuOnlineAwardDataList(DBManager dbMgr, int zoneID)
        {
            List<KaiFuOnlineAwardData> kaiFuOnlineAwardDataList = new List<KaiFuOnlineAwardData>();
            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT K.rid, R.zoneid, R.rname, K.dayid, K.totalrolenum FROM t_kfonlineawards AS K, t_roles AS R WHERE K.rid=R.rid AND K.zoneid={0}", zoneID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    while (reader.Read())
                    {
                        kaiFuOnlineAwardDataList.Add(new KaiFuOnlineAwardData()
                        {
                            RoleID = Global.SafeConvertToInt32(reader["rid"].ToString()),
                            ZoneID = Global.SafeConvertToInt32(reader["zoneid"].ToString()),
                            RoleName = reader["rname"].ToString(),
                            DayID = Global.SafeConvertToInt32(reader["dayid"].ToString()),
                            TotalRoleNum = Global.SafeConvertToInt32(reader["totalrolenum"].ToString()),
                        });
                    }
                }
                catch (Exception)
                {
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return kaiFuOnlineAwardDataList;
        }

        #endregion 开服在线大礼

        #region Quản lý các biến cấu hình hệ thống

        /// <summary>
        /// Trả về danh sách các tham biến hệ thống
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="index"></param>
        public static ConcurrentDictionary<int, string> GetSystemGlobalValues(DBManager dbMgr)
        {
            ConcurrentDictionary<int, string> parameters = new ConcurrentDictionary<int, string>();
            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = string.Format("SELECT id, value FROM t_systemglobalvalue");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"].ToString());
                    string value = reader["value"].ToString();
                    parameters[id] = value;
                }
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
            return parameters;
        }

        /// <summary>
        /// Lưu tham biến hệ thống
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SaveSystemGlobalValue(DBManager dbMgr, int index, string value)
        {
            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                /// Kiểm tra tồn tại chưa
                bool isExist = false;
                {
                    string cmdText = string.Format("SELECT value FROM t_systemglobalvalue WHERE id = {0}", index);
                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                    MySQLDataReader reader = cmd.ExecuteReaderEx();
                    /// Nếu có dữ liệu nghĩa là đã tồn tại
                    isExist = reader.Read();
                    cmd.Dispose();
                    /// Giải phóng
                    cmd = null;
                }

                /// Nếu đã tồn tại thì Update
                if (isExist)
                {
                    string cmdText = string.Format("UPDATE t_systemglobalvalue SET value = {0} WHERE id = {1}", value, index);
                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    /// Giải phóng
                    cmd = null;
                }
                /// Nếu chưa tồn tại thì Insert
                else
                {
                    string cmdText = string.Format("INSERT INTO t_systemglobalvalue(id, value) VALUES({0}, {1})", index, value);
                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    /// Giải phóng
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
            return "";
        }

        #endregion Quản lý các biến cấu hình hệ thống

        #region 角色消费活动相关

        /// <summary>
        /// 返回充值排行信息 key:userid, value:totalmoney 的list,排行小于等于maxPaiHang的被返回,如果两个玩家消费额相等，先达到值的排名靠前
        /// dictionay顺序未知，直接返回排好序的list,这个函数返回的排行值是具体的真实货币值，外部使用时需要自行转换为元宝数，不在这转换
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime">字符串时间</param>
        /// <param name="endTime">字符串时间</param>
        /// <param name="maxPaiHang">必须大于等于1,默认返回排名前三的信息</param>
        /// <returns></returns>
        public static List<InputKingPaiHangData> GetUserUsedMoneyPaiHang1(DBManager dbMgr, string startTime, string endTime, int maxPaiHang = 3)
        {
            List<InputKingPaiHangData> lsPaiHang = new List<InputKingPaiHangData>();

            if (maxPaiHang < 1)
            {
                return lsPaiHang;
            }

            int rid = -1, totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                //这个位置需要采用unix时间戳进行定位,普通情况下，unix时间戳和inputtime是同一个值，单inputtime时间戳比unix要大一点
                //对用户充值而言，采用unix时间戳比较准确,此外，默认任务玩家充值信息只在t_mallbuy表中有，而gm充值信息只在t_qizhengebuy表中，t_zajindanhist
                //如果存在某个玩家在两个表中都有充值，该玩家可能有3条数据位于取出的数据中，这时，需要做过滤操作，所以，返回数据最大行数乘以3,
                //然后再对返回的数据进行重复玩家过滤
                string cmdText = string.Format("SELECT t_mallbuy.rid, sum(t_mallbuy.totalprice) as totalmoney, max(t_mallbuy.buytime) as time from t_mallbuy,t_roles  where t_mallbuy.rid=t_roles.rid and buytime>='{0}' and buytime<='{1}' and t_roles.isdel=0 GROUP by rid " +
                                 " union " +
                                 " SELECT t_zajindanhist.rid, sum(usedyuanbao/timesselected) as totalmoney, max(operationtime) as time from t_zajindanhist,t_roles where t_zajindanhist.rid=t_roles.rid and t_roles.isdel=0 and operationtime>='{0}' and operationtime<='{1}' GROUP by rid " +
                                 " union " +
                                 " SELECT t_qizhengebuy.rid, sum(totalprice) as totalmoney, max(buytime) as time from t_qizhengebuy,t_roles where buytime>='{0}' and buytime<='{1}' and t_qizhengebuy.rid=t_roles.rid and t_roles.isdel=0  GROUP by rid order by totalmoney desc,time asc " +
                                 " limit 0, {2} ", startTime, endTime, maxPaiHang * 3);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                List<int> tmp = new List<int>();
                int count = 0;
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //最多返回maxPaiHang 条数据
                while (reader.Read())
                {
                    count++;

                    rid = Convert.ToInt32(reader["rid"].ToString());
                    totalmoney = (int)Convert.ToDouble(reader["totalmoney"].ToString());

                    if (totalmoney > 0)
                    {
                        if (!tmp.Contains(rid))
                        {
                            tmp.Add(rid);

                            InputKingPaiHangData phData = new InputKingPaiHangData
                            {
                                UserID = rid.ToString(),
                                PaiHang = count,
                                PaiHangTime = now,
                                PaiHangValue = totalmoney
                            };

                            lsPaiHang.Add(phData);
                        }
                        else
                        {
                            //对在两个表中都有充值[一般是gm]特殊处理
                            InputKingPaiHangData phData = lsPaiHang[tmp.IndexOf(rid)];
                            phData.PaiHangValue += totalmoney;
                        }
                    }

                    //最多取回 maxPaiHang条
                    if (lsPaiHang.Count >= maxPaiHang)
                    {
                        break;
                    }
                }

                //对返回结果进行排序，按照PaiHangValue 由大到小排序，然后更新PaiHang值

                Comparison<InputKingPaiHangData> com = new Comparison<InputKingPaiHangData>(InputKingPaiHangDataCompare);
                lsPaiHang.Sort(com);

                for (int n = 0; n < lsPaiHang.Count; n++)
                {
                    lsPaiHang[n].PaiHang = n + 1;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lsPaiHang;
        }

        /// <summary>
        /// 返回玩家某时间内在某区的充值总额【这儿返回的是真实货币单位，要转换为元宝，必须调用相应的转换函数】
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetUserUsedMoney1(DBManager dbMgr, int rid, string startTime, string endTime)
        {
            int totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                //这个位置需要采用unix时间戳进行定位,普通情况下，unix时间戳和inputtime是同一个值，单inputtime时间戳比unix要大一点
                //对用户充值而言，采用unix时间戳比较准确,此外，默认任务玩家充值信息只在t_mallbuy表中有，而gm充值信息只在t_qizhengebuy表中，t_zajindanhist
                //如果存在某个玩家在两个表中都有充值，该玩家可能有3条数据位于取出的数据中，这时，需要做过滤操作，所以，返回数据最大行数乘以3,
                //然后再对返回的数据进行重复玩家过滤
                string cmdText = string.Format("SELECT rid, sum(totalprice) as totalmoney, max(buytime) as time from t_mallbuy where buytime>='{0}' and buytime<='{1}' and rid={2} GROUP by rid " +
                                 " union " +
                                 " SELECT rid, sum(usedyuanbao/timesselected) as totalmoney, max(operationtime) as time from t_zajindanhist where operationtime>='{0}' and operationtime<='{1}' and rid={2} GROUP by rid " +
                                 " union " +
                                 " SELECT rid, sum(totalprice) as totalmoney, max(buytime) as time from t_qizhengebuy where buytime>='{0}' and buytime<='{1}' and rid={2} GROUP by rid"
                                 , startTime, endTime, rid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    totalmoney += (int)Convert.ToDouble(reader["totalmoney"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return totalmoney;
        }

        #endregion 角色消费活动相关



        #region 血色堡垒

        /// <summary>
        /// 返回角色当天进入血色堡垒的次数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetBloodCastleEnterCount(DBManager dbMgr, int roleid, int nDate, int activityid)
        {
            int ret = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT triggercount from t_dayactivityinfo where roleid={0} and activityid={1} and timeinfo={2} ",
                    roleid, activityid, nDate);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                    ret = Convert.ToInt32(reader["triggercount"].ToString());

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return ret;
        }

        #endregion 血色堡垒

        #region 日常活动最高积分

        /// <summary>
        /// 返回日常活动最高积分
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<int> GetDayActivityTotlePoint(DBManager dbMgr, int activityid)
        {
            List<int> lData = new List<int>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT roleid, totalpoint FROM t_dayactivityinfo WHERE totalpoint>0 AND activityid = {0} ORDER BY totalpoint DESC LIMIT 1", activityid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int nRoleid = -1;
                int nPoint = -1;

                if (reader.Read())
                {
                    nRoleid = Convert.ToInt32(reader["roleid"].ToString());
                    nPoint = Convert.ToInt32(reader["totalpoint"].ToString());
                }

                if (nRoleid != -1 && nPoint != -1)
                {
                    lData.Add(nRoleid);
                    lData.Add(nPoint);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lData;
        }

        /// <summary>
        /// 返回玩家日常活动最高积分
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetRoleDayActivityPoint(DBManager dbMgr, int nRole, int activityid)
        {
            int nPoint = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT totalpoint FROM t_dayactivityinfo WHERE roleid = {0} AND activityid = {1}", nRole, activityid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    nPoint = (int)Math.Max(0, Math.Min(int.MaxValue / 2, Convert.ToInt64(reader["totalpoint"].ToString())));
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return nPoint;
        }

        #endregion 日常活动最高积分

        #region 崇拜

        /// <summary>
        /// 查询是否崇拜了某人
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int QueryPlayerAdmiredAnother(DBManager dbMgr, int roleAID, int roleBID, int nDate)
        {
            int nID = -1;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT adorationroleid from t_adorationinfo where roleid={0} and adorationroleid={1} and dayid={2}", roleAID, roleBID, nDate);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                    nID = Convert.ToInt32(reader["adorationroleid"].ToString());

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return nID;
        }

        #endregion 崇拜

        #region 每日在线奖励相关

        /// <summary>
        /// 查询每日在线奖励数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static List<int> QueryPlayerEveryDayOnLineAwardGiftInfo(DBManager dbMgr, int roleID)
        {
            List<int> lData = new List<int>();

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT everydayonlineawardstep, geteverydayonlineawarddayid from t_huodong where roleid={0}", roleID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    int nValue = 0;

                    nValue = Convert.ToInt32(reader["everydayonlineawardstep"].ToString());
                    lData.Add(nValue);

                    nValue = Convert.ToInt32(reader["geteverydayonlineawarddayid"].ToString());

                    lData.Add(nValue);
                }
                else
                    lData = null;

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lData;
        }

        #endregion 每日在线奖励相关

        #region 推送相关

        /// <summary>
        /// 查询在条件范围内的玩家 返回列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static List<PushMessageData> QueryPushMsgUerList(DBManager dbMgr, int nCondition)
        {
            List<PushMessageData> list = new List<PushMessageData>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                DateTime time = DateTime.Now;

                string cmdText = string.Format("SELECT userid, pushid, lastlogintime from t_pushmessageinfo where NOW() <= ADDDATE(lastlogintime, {0})", nCondition + 1);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    PushMessageData PushMsgData = new PushMessageData()
                    {
                        UserID = reader["userid"].ToString(),
                        PushID = reader["pushid"].ToString(),
                        LastLoginTime = reader["lastlogintime"].ToString()
                    };

                    list.Add(PushMsgData);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return list;
        }

        #endregion 推送相关

        #region 魔晶兑换相关

        /// <summary>
        /// 查询绑定钻石兑换信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static Dictionary<int, int> QueryMoJingExchangeDict(DBManager dbMgr, int nRoleid, int nDayID)
        {
            Dictionary<int, int> TmpDict = new Dictionary<int, int>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = string.Format("SELECT exchangeid, exchangenum FROM t_mojingexchangeinfo WHERE roleid = {0} AND dayid = {1}", nRoleid, nDayID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    int nExchangeid = Convert.ToInt32(reader["exchangeid"].ToString());
                    int nNum = Convert.ToInt32(reader["exchangenum"].ToString());

                    TmpDict.Add(nExchangeid, nNum);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return TmpDict;
        }

        #endregion 魔晶兑换相关

        #region 查询资源找回数据

        /// <summary>
        /// 查询资源找回数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static Dictionary<int, OldResourceInfo> QueryResourceGetInfo(DBManager dbMgr, int nRoleid)
        {
            Dictionary<int, OldResourceInfo> datadict = new Dictionary<int, OldResourceInfo>();
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                DateTime time = DateTime.Now;

                string cmdText = string.Format("SELECT type, exp, leftCount,mojing,bandmoney,zhangong,chengjiu,shengwang,bangzuan,xinghun,yuansufenmo from t_resourcegetinfo where roleid = {0} AND hasget = {1}", nRoleid, 0);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    OldResourceInfo data = new OldResourceInfo()
                    {
                        type = Global.SafeConvertToInt32(reader["type"].ToString()),
                        exp = Global.SafeConvertToInt32(reader["exp"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["exp"].ToString()) : 0,
                        leftCount = Global.SafeConvertToInt32(reader["leftCount"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["leftCount"].ToString()) : 0,
                        mojing = Global.SafeConvertToInt32(reader["mojing"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["mojing"].ToString()) : 0,
                        bandmoney = Global.SafeConvertToInt32(reader["bandmoney"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["bandmoney"].ToString()) : 0,
                        zhangong = Global.SafeConvertToInt32(reader["zhangong"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["zhangong"].ToString()) : 0,
                        chengjiu = Global.SafeConvertToInt32(reader["chengjiu"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["chengjiu"].ToString()) : 0,
                        shengwang = Global.SafeConvertToInt32(reader["shengwang"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["shengwang"].ToString()) : 0,
                        bandDiamond = Global.SafeConvertToInt32(reader["bangzuan"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["bangzuan"].ToString()) : 0,
                        xinghun = Global.SafeConvertToInt32(reader["xinghun"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["xinghun"].ToString()) : 0,
                        yuanSuFenMo = Global.SafeConvertToInt32(reader["yuansufenmo"].ToString()) > 0 ? Global.SafeConvertToInt32(reader["yuansufenmo"].ToString()) : 0,
                        roleId = nRoleid
                    };

                    datadict[data.type] = data;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return datadict;
        }

        #endregion 查询资源找回数据

        #region 消费排行相关

        /// <summary>
        /// 新查找消费数额
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetUserUsedMoney(DBManager dbMgr, int rid, string startTime, string endTime)
        {
            int totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid, sum(amount) as totalmoney  from t_consumelog where cdate>='{0}' and cdate<='{1}' and rid={2} GROUP by rid "
                                 , startTime, endTime, rid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    totalmoney = (int)Convert.ToDouble(reader["totalmoney"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return totalmoney;
        }

        /// <summary>
        /// Lấy ra số ĐỒng đã tiêu trong toàn bộ thời gian
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetUserUsedMoney(DBManager dbMgr, int rid)
        {
            int totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid, sum(amount) as totalmoney  from t_consumelog where rid={0} GROUP by rid"
                                 , rid);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    totalmoney = (int)Convert.ToDouble(reader["totalmoney"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return totalmoney;
        }

        /// <summary>
        /// 新消费排行查询
        /// 此函数作为基础函数，实际操作请使用带有缓存的GetUsedMoneyKingPaiHangListByHuoDongLimit
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="maxPaiHang"></param>
        /// <returns></returns>
        public static List<InputKingPaiHangData> GetUserUsedMoneyPaiHang(DBManager dbMgr, string startTime, string endTime, int maxPaiHang = 3)
        {
            List<InputKingPaiHangData> lsPaiHang = new List<InputKingPaiHangData>();

            if (maxPaiHang < 1)
            {
                return lsPaiHang;
            }

            int rid = -1, totalmoney = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT t_consumelog.rid, sum(t_consumelog.amount) as totalmoney, max(cdate) as time from t_consumelog,t_roles  where t_consumelog.rid=t_roles.rid and cdate>='{0}' and cdate<='{1}' and t_roles.isdel=0 GROUP by rid "
                                                + " order by totalmoney desc,time asc " +
                                                 " limit 0, {2} "
                                 , startTime, endTime, maxPaiHang * 2);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                List<int> tmp = new List<int>();
                int count = 0;
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //最多返回maxPaiHang 条数据
                while (reader.Read())
                {
                    count++;

                    rid = Convert.ToInt32(reader["rid"].ToString());
                    totalmoney = (int)Convert.ToDouble(reader["totalmoney"].ToString());

                    if (totalmoney > 0)
                    {
                        if (!tmp.Contains(rid))
                        {
                            tmp.Add(rid);

                            InputKingPaiHangData phData = new InputKingPaiHangData
                            {
                                UserID = rid.ToString(),
                                PaiHang = count,
                                PaiHangTime = now,
                                PaiHangValue = totalmoney
                            };

                            lsPaiHang.Add(phData);
                        }
                        else
                        {
                            //对在两个表中都有充值[一般是gm]特殊处理
                            InputKingPaiHangData phData = lsPaiHang[tmp.IndexOf(rid)];
                            phData.PaiHangValue += totalmoney;
                        }
                    }

                    //最多取回 maxPaiHang条
                    if (lsPaiHang.Count >= maxPaiHang)
                    {
                        break;
                    }
                }

                //对返回结果进行排序，按照PaiHangValue 由大到小排序，然后更新PaiHang值

                Comparison<InputKingPaiHangData> com = new Comparison<InputKingPaiHangData>(InputKingPaiHangDataCompare);
                lsPaiHang.Sort(com);

                for (int n = 0; n < lsPaiHang.Count; n++)
                {
                    lsPaiHang[n].PaiHang = n + 1;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return lsPaiHang;
        }

        #endregion 消费排行相关

        #region MU VIP相关

        /// <summary>
        /// 查询VIP等级奖励标记信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int QueryVipLevelAwardFlagInfo(DBManager dbMgr, int nRoldID, int nZoneID)
        {
            int nFlag = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT vipawardflag from t_roles WHERE rid = '{0}' and zoneid={1}", nRoldID, nZoneID); // 加区id判断 因合服可能产生相同的帐号vipawardflag字段不同 导致读取错误 [XSea 2015/7/8]
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    if (reader.Read())
                    {
                        nFlag = Convert.ToInt32(reader["vipawardflag"].ToString());
                    }
                }
                catch (Exception)
                {
                    nFlag = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return nFlag;
        }

        /// <summary>
        /// 根据账号查询VIP等级奖励标记信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int QueryVipLevelAwardFlagInfoByUserID(DBManager dbMgr, string struseid, int nRoleID, int nZoneID)
        {
            int nFlag = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT vipawardflag FROM t_roles WHERE userid = '{0}' and zoneid = {1} and rid != {2}", struseid, nZoneID, nRoleID); // 加区id和roleid判断 因合服可能产生相同的帐号vipawardflag字段不同 导致读取错误 [XSea 2015/7/8]
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader["vipawardflag"].ToString()) > 0)
                        {
                            nFlag = Convert.ToInt32(reader["vipawardflag"].ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    nFlag = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return nFlag;
        }

        #endregion MU VIP相关

        #region 最后一次登陆的角色

        public static int LastLoginRole(DBManager dbMgr, string uid)
        {
            int rid = 0;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid FROM t_roles WHERE userid = '{0}' AND isdel=0   ORDER BY lasttime DESC LIMIT 1 ", uid);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader["rid"].ToString()) > 0)
                        {
                            rid = Convert.ToInt32(reader["rid"].ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    rid = 0;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return rid;
        }

        #endregion 最后一次登陆的角色

        #region 检查用户有没有这个角色

        public static bool GetUserRole(DBManager dbMgr, string userID, int roleID)
        {
            int rid = 0;

            MySQLConnection conn = null;
            bool result = false;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT rid FROM t_roles WHERE userid = '{0}' AND rid={1} AND isdel=0", userID, roleID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                try
                {
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader["rid"].ToString()) == roleID)
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception)
                {
                    result = false;
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return result;
        }

        #endregion 检查用户有没有这个角色

        #region 结婚数据查询

        /// <summary>
        /// 查询结婚数据
        /// </summary>
        public static MarriageData GetMarriageData(DBManager dbMgr, int nRoleID)
        {
            bool bRet = false;

            MySQLConnection conn = null;

            MarriageData clientmarriagedata = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT spouseid, marrytype, ringid, goodwillexp, goodwillstar, goodwilllevel, givenrose, lovemessage, autoreject,changtime FROM t_marry WHERE roleid = {0}",
                                                nRoleID);

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                try
                {
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    if (reader.Read())
                    {
                        clientmarriagedata = new MarriageData()
                        {
                            nSpouseID = Global.SafeConvertToInt32(reader["spouseid"].ToString()),
                            byMarrytype = Convert.ToSByte(reader["marrytype"].ToString()),
                            nRingID = Global.SafeConvertToInt32(reader["ringid"].ToString()),
                            nGoodwillexp = Global.SafeConvertToInt32(reader["goodwillexp"].ToString()),
                            byGoodwillstar = Convert.ToSByte(reader["goodwillstar"].ToString()),
                            byGoodwilllevel = Convert.ToSByte(reader["goodwilllevel"].ToString()),
                            nGivenrose = Global.SafeConvertToInt32(reader["givenrose"].ToString()),
                            strLovemessage = reader["lovemessage"].ToString(),
                            byAutoReject = Convert.ToSByte(reader["autoreject"].ToString()),
                            ChangTime = reader["changtime"].ToString(),
                        };
                    }
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("查询数据库失败: {0}", cmdText));
                }

                cmd.Dispose();
                cmd = null;

                bRet = true;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return clientmarriagedata;
        }

        #endregion 结婚数据查询

        #region 读取婚宴列表

        public static void QueryMarryPartyList(DBManager dbMgr, Dictionary<int, MarryPartyData> partyList)
        {
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT roleid, partytype, joincount, starttime, husbandid, wifeid, (SELECT rname FROM t_roles WHERE rid = husbandid) AS husbandname, (SELECT rname FROM t_roles WHERE rid = wifeid) AS wifename FROM t_marryparty");

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                try
                {
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    while (reader.Read())
                    {
                        MarryPartyData data = new MarryPartyData()
                        {
                            RoleID = Convert.ToInt32(reader["roleid"].ToString()),
                            PartyType = Convert.ToInt32(reader["partytype"].ToString()),
                            JoinCount = Convert.ToInt32(reader["joincount"].ToString()),
                            StartTime = DataHelper.ConvertToTicks(reader["starttime"].ToString()),
                            HusbandRoleID = Convert.ToInt32(reader["husbandid"].ToString()),
                            WifeRoleID = Convert.ToInt32(reader["wifeid"].ToString()),
                            HusbandName = reader["husbandname"].ToString(),
                            WifeName = reader["wifename"].ToString(),
                        };

                        partyList.Add(data.RoleID, data);
                    }
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("查询数据库失败: {0}", cmdText));
                }

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }

        #endregion 读取婚宴列表



        /// <summary>
        /// 扫描新的群邮件信息，并且写入日志中 返回roleid, mailid 对应的列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static List<GroupMailData> ScanNewGroupMailFromTable(DBManager dbMgr, int beginID)
        {
            List<GroupMailData> GroupMailList = null;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //每次都去取id大于上次去过的id
                string cmdText = string.Format("SELECT * from t_groupmail where gmailid > {0} and endtime > '{1}'", beginID, today);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    GroupMailData gmailData = new GroupMailData();
                    gmailData.GMailID = Convert.ToInt32(reader["gmailid"].ToString());
                    gmailData.Subject = reader["subject"].ToString();
                    gmailData.Content = reader["content"].ToString();//Encoding.Default.GetString((byte[])reader["content"]);
                    gmailData.Conditions = reader["conditions"].ToString();
                    gmailData.InputTime = DateTime.Parse(reader["inputtime"].ToString()).Ticks;
                    gmailData.EndTime = DateTime.Parse(reader["endtime"].ToString()).Ticks;
                    gmailData.Yinliang = Convert.ToInt32(reader["yinliang"].ToString());
                    gmailData.Tongqian = Convert.ToInt32(reader["tongqian"].ToString());
                    gmailData.YuanBao = Convert.ToInt32(reader["yuanbao"].ToString());
                    gmailData.GoodsList = Global.ParseGoodsDataList(reader["goodlist"].ToString());

                    if (null == GroupMailList)
                    {
                        GroupMailList = new List<GroupMailData>();
                    }

                    GroupMailList.Add(gmailData);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return GroupMailList;
        }

        public static bool IsBlackUserID(DBManager dbMgr, string userid)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("SELECT count(*) from t_blackuserid where userid='{0}' limit 1;", userid);
                return conn.GetSingleInt(cmdText) > 0;
            }
        }

        public static RoleMiniInfo QueryRoleMiniInfo(long rid)
        {
            RoleMiniInfo roleMiniInfo = null;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("SELECT zoneid,userid from t_roles where rid={0};", rid);
                MySQLDataReader reader = conn.ExecuteReader(cmdText);
                if (reader.Read())
                {
                    roleMiniInfo = new RoleMiniInfo();
                    roleMiniInfo.roleId = rid;
                    roleMiniInfo.zoneId = Convert.ToInt32(reader["zoneid"].ToString());
                    roleMiniInfo.userId = reader["userid"].ToString();
                }
            }

            return roleMiniInfo;
        }

        #region ten

        /// <summary>
        /// 扫描新的发奖数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static List<TenAwardData> ScanNewGroupTenFromTable(DBManager dbMgr)
        {
            List<TenAwardData> groupList = null;

            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                //每次都去取id大于上次去过的id
                string cmdText = string.Format("SELECT * FROM t_ten WHERE state=0 ORDER BY id LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    TenAwardData d = new TenAwardData();
                    d.DbID = Convert.ToInt32(reader["id"].ToString());
                    d.RoleID = Convert.ToInt32(reader["roleID"].ToString());
                    d.AwardID = Convert.ToInt32(reader["giftID"].ToString());
                    d.UserID = reader["uID"].ToString();

                    if (null == groupList)
                        groupList = new List<TenAwardData>();

                    groupList.Add(d);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return groupList;
        }

        public static int TenOnlyNum(DBManager dbMgr, string userID, int awardID)
        {
            int totalNum = 0;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = string.Format("SELECT COUNT(*) AS totalnum FROM t_ten WHERE giftID='{0}' and uID='{1}' and state>1", awardID, userID);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                    totalNum = Convert.ToInt32(reader["totalnum"].ToString());

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                    dbMgr.DBConns.PushDBConnection(conn);
            }

            return totalNum;
        }

        public static int TenDayNum(DBManager dbMgr, string userID, int awardID)
        {
            int totalNum = 0;
            MySQLConnection conn = null;

            try
            {
                string strBegin = string.Format("{0:yyyy-MM-dd 00:00:00}", DateTime.Now);
                string strEnd = string.Format("{0:yyyy-MM-dd 23:59:59}", DateTime.Now);
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format(
                    "SELECT COUNT(*) AS totalnum FROM t_ten WHERE giftID='{0}' and uID='{1}' and state>1 and updatetime>='{2}' and updatetime<='{3}';",
                    awardID, userID, strBegin, strEnd);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                    totalNum = Convert.ToInt32(reader["totalnum"].ToString());

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                    dbMgr.DBConns.PushDBConnection(conn);
            }

            return totalNum;
        }

        #endregion ten

        #region activate

        public static bool ActivateStateGet(DBManager dbMgr, string userID)
        {
            bool isActivate = false;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = string.Format("SELECT * FROM t_activate WHERE userID='{0}' LIMIT 1;", userID);
                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read()) isActivate = true;

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                    dbMgr.DBConns.PushDBConnection(conn);
            }

            return isActivate;
        }

        #endregion activate

        #region GiftCode

        /// <summary>
        /// 扫描新的礼包码信息
        /// </summary>
        public static List<GiftCodeAwardData> ScanNewGiftCodeFromTable(DBManager dbMgr)
        {
            List<GiftCodeAwardData> GiftList = null;
            MySQLConnection conn = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                string cmdText = string.Format("SELECT * FROM t_giftcode WHERE mailid=0 ORDER BY id asc LIMIT 100");
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();
                while (reader.Read())
                {
                    GiftCodeAwardData data = new GiftCodeAwardData();
                    data.Dbid = Convert.ToInt32(reader["id"].ToString());
                    data.UserId = reader["userid"].ToString();
                    data.RoleID = Convert.ToInt32(reader["rid"].ToString());
                    data.GiftId = reader["giftid"].ToString();
                    data.CodeNo = reader["codeno"].ToString();

                    if (null == GiftList)
                    {
                        GiftList = new List<GiftCodeAwardData>();
                    }
                    GiftList.Add(data);
                }
                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return GiftList;
        }

        #endregion GiftCode
    }
}