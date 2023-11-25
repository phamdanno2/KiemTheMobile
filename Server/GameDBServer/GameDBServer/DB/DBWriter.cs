using GameDBServer.Core;
using GameDBServer.Logic;
using GameDBServer.Logic.KT_ItemManager;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.DB
{
    /// <summary>
    /// 数据库写操作类
    /// </summary>
    public class DBWriter
    {
        #region 数据库完整性校验

        /// <summary>
        /// 必须存在的表和字段
        /// </summary>
        private static readonly string[][] ValidateDatabaseTables = new string[][]{
            new string[]{"t_login", "userid"},
            new string[]{"t_usemoney_log", "Id"},
            new string[]{"t_goods_bak", "id"},
            new string[]{"t_goods_bak_1", "id"},
        };

        #endregion 数据库完整性校验

        #region 角色满了，再分配一段角色id区间

        private const string RoleExtIdKey = "role_ext_auto_increment";
        private const int RoleExtIdValidStart = 1500000000;

        #endregion 角色满了，再分配一段角色id区间

        #region 数据库表字段，FormatUpdateSQL

        private static readonly string[] _UpdateGoods_fieldNames = { "isusing", "forge_level", "starttime", "endtime", "site", "Props", "gcount", "binding", "bagindex", "strong", "series", "otherpramer", "goodsid" };
        private static readonly byte[] _UpdateGoods_fieldTypes = { 0, 0, 3, 3, 0, 1, 0, 0, 0, 0, 0, 1, 0 };

        private static readonly string[] _UpdateTask_fieldNames = { "focus", "value1", "value2" };
        private static readonly byte[] _UpdateTask_fieldTypes = { 0, 0, 0 };

        private static readonly string[] _UpdatePet_fieldNames = { "petname", "pettype", "feednum", "realivenum", "props", "isdel", "addtime", "level" };
        private static readonly byte[] _UpdatePet_fieldTypes = { 1, 0, 0, 0, 1, 0, 3, 0 };

        private static readonly string[] _UpdateActivity_fieldNames = { "loginweekid", "logindayid", "loginnum", "newstep", "steptime", "lastmtime", "curmid", "curmtime", "songliid", "logingiftstate", "onlinegiftstate", "lastlimittimehuodongid", "lastlimittimedayid", "limittimeloginnum", "limittimegiftstate", "everydayonlineawardstep", "geteverydayonlineawarddayid", "serieslogingetawardstep", "seriesloginawarddayid", "seriesloginawardgoodsid", "everydayonlineawardgoodsid" };
        private static readonly byte[] _UpdateActivity_fieldTypes = { 1, 1, 0, 0, 3, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };

        private static readonly string[] _UpdateWing_fieldNames = { "equiped", "wingid", "forgeLevel", "failednum", "starexp", "zhulingnum", "zhuhunnum" };
        private static readonly byte[] _UpdateWing_fieldTypes = { 0, 0, 0, 0, 0, 0, 0 };

        private static readonly string[] _UpdateGoods2_fieldNames = { "isusing", "forge_level", "starttime", "endtime", "site", "quality", "Props", "gcount", "jewellist", "bagindex", "salemoney1", "saleyuanbao", "saleyinpiao", "binding", "addpropindex", "bornindex", "lucky", "strong", "excellenceinfo", "appendproplev", "equipchangelife" };
        private static readonly byte[] _UpdateGoods2_fieldTypes = { 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        #endregion 数据库表字段，FormatUpdateSQL

        #region 物品备份表维护

        private static object GoodsBakTableMutex = new object();

        private static int GoodsBakTableIndex = -1;

        private static readonly string[] GoodsBakTableNames = { "t_goods_bak", "t_goods_bak_1" };

        public static string CurrentGoodsBakTableName
        {
            get
            {
                return GoodsBakTableNames[GoodsBakTableIndex % GoodsBakTableNames.Length];
            }
        }

        #endregion 物品备份表维护

        #region 辅助函数

        /// <summary>
        /// 执行一个sql语句，并返回读取的整数结果
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="sqlText"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int ExecuteSQLNoQuery(DBManager dbMgr, string sqlText, MySQLConnection conn = null)
        {
            int result = 0;
            bool keepConn = true;

            MySQLCommand cmd = null;
            try
            {
                if (conn == null)
                {
                    keepConn = false;
                    conn = dbMgr.DBConns.PopDBConnection();
                }

                using (cmd = new MySQLCommand(sqlText, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteException(ex.ToString());
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteException(ex.ToString());
                return -2;
            }
            finally
            {
                if (!keepConn && null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return result;
        }

        #endregion 辅助函数

        /// <summary>
        /// 角色是否已满
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static bool CheckRoleCountFull(DBManager dbMgr)
        {
            bool bFull = true;
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();
                MySQLCommand cmd = new MySQLCommand("SELECT max(rid) AS LastID from t_roles", conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int nCount = 0;
                while (reader.Read())
                {
                    nCount = Global.SafeConvertToInt32(reader[0].ToString());
                    // nCount = int.Parse(reader[0].ToString());
                }

                if (null != cmd)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                if ((nCount % GameDBManager.DBAutoIncreaseStepValue) >= GameDBManager.DBAutoIncreaseStepValue - 500)
                {
                    // 角色已满
                    int extId = GameDBManager.GameConfigMgr.GetGameConfigItemInt(RoleExtIdKey, 0);
                    if (extId >= RoleExtIdValidStart && nCount < extId)
                    {
                        if (0 == DBWriter.ChangeTablesAutoIncrementValue(dbMgr, "t_roles", extId))
                        {
                            bFull = false; // 增加额外角色ID区间成功
                        }
                        else bFull = true;
                    }
                    else bFull = true;
                }
                else
                {
                    // 角色未满
                    bFull = false;
                }
            }
            catch (MySQLException)
            {
                // 出现了任何异常，就不允许创建
                bFull = true;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return bFull;
        }

        /// <summary>
        /// Tạo nhân vật mới
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int CreateRole(DBManager dbMgr, string userID, string userName, int sex, int factionID, int subID, string roleName, int serverID, int bagnum, string positionInfo, int isflashplayer)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int roleID = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "INSERT INTO t_roles (userid, rname, sex, occupation, sub_id, position, regtime, lasttime, zoneid, username, bagnum, isflashplayer) VALUES('" + userID + "', '" + roleName + "', " + sex + ", " + factionID + "," + subID + ", '" + positionInfo + "', '" + today + "', '" + today + "', " + serverID + ", '" + userName + "'," + bagnum + "," + isflashplayer + ")";
                if (!conn.ExecuteNonQueryBool(cmdText)) return roleID;

                try
                {
                    roleID = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                }
                catch (MySQLException)
                {
                    roleID = -2; //创建失败(角色名称重复)
                }
            }

            return roleID;
        }

        /// <summary>
        /// 恢复预删除的一个用户角色
        /// </summary>
        public static bool UnPreRemoveRole(DBManager dbMgr, int roleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET predeltime=NULL WHERE rid={0} and predeltime IS NOT NULL", roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 预删除一个用户角色
        /// </summary>
        public static bool PreRemoveRole(DBManager dbMgr, int roleID, DateTime Now)
        {
            bool ret = false;
            string today = Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET predeltime='{0}' WHERE rid={1} and predeltime IS NULL", today, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除一个用户角色
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool RemoveRole(DBManager dbMgr, int roleID)
        {
            bool ret = false;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET isdel=1, deltime='{0}', predeltime=NULL WHERE rid={1}", today, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除一个用户角色
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool RemoveRoleByName(DBManager dbMgr, string roleName)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET isdel=1 WHERE rname='{0}'", roleName);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 恢复删除一个用户角色
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UnRemoveRole(DBManager dbMgr, string roleName)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET isdel=0 WHERE rname='{0}'", roleName);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的最后登录时间和登录次数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLoginInfo(DBManager dbMgr, int roleID, int loginNum, int loginDayID, int loginDayNum, string userid, int zoneid, string ip)
        {
            bool ret = false;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET lasttime='{0}', loginnum={1}, logindayid={2}, logindaynum={3} WHERE rid={4}", today, loginNum, loginDayID, loginDayNum, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);

                DateTime now = DateTime.Now;
                cmdText = string.Format("INSERT INTO t_login (userid,dayid,rid,logintime,logouttime,ip,mac,zoneid,onlinesecs,loginnum) " +
                                            "VALUES('{0}',{1},{2},'{3}','{4}','{5}','{6}',{7},0,1) ON DUPLICATE KEY UPDATE loginnum=loginnum+1,rid={2}"
                                            , userid, Global.GetOffsetDay(now), roleID, today, Global.GetDayEndTime(now), ip, null, zoneid);
                conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的离线时间
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLogOff(DBManager dbMgr, int roleID, string userid, int zoneid, string ip, int onlineSecs)
        {
            bool ret = false;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET logofftime='{0}' WHERE rid={1}", today, roleID);
                conn.ExecuteNonQuery(cmdText);

                DateTime now = DateTime.Now;
                cmdText = string.Format("INSERT INTO t_login (userid,dayid,rid,logintime,logouttime,ip,mac,zoneid,onlinesecs,loginnum) " +
                                            "VALUES('{0}',{1},{2},'{3}','{4}','{5}','{6}',{7},{8},1) ON DUPLICATE KEY UPDATE logouttime='{4}',onlinesecs=LEAST(onlineSecs+{8},86400);"
                                            , userid, Global.GetOffsetDay(now), roleID, Global.GetDayStartTime(now), today, ip, null, zoneid, onlineSecs);
                conn.ExecuteNonQuery(cmdText);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的相关在线时长
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleOnlineSecs(DBManager dbMgr, int roleID, int totalOnlineSecs, int antiAddictionSecs)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET totalonlinesecs={0}, antiaddictionsecs={1} WHERE rid={2}", totalOnlineSecs, antiAddictionSecs, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的充值TaskID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleCZTaskID(DBManager dbMgr, int roleID, int czTaskID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET cztaskid={0} WHERE rid={1}", czTaskID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的单次奖励历史标志位
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleOnceAwardFlag(DBManager dbMgr, int roleID, long onceawardflag)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET onceawardflag={0} WHERE rid={1}", onceawardflag, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的永久禁止登陆标志
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleBanProps(DBManager dbMgr, int roleID, string colName, long value)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET {0}={1} WHERE rid={2}", colName, value, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新任务
        /// </summary>
        public static int NewTask(DBManager dbMgr, int roleID, int npcID, int taskID, string addtime, int focus, int nStarLevel)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_tasks (taskid, rid, value1, value2, focus, isdel, addtime, starlevel) VALUES({0}, {1}, {2}, {3}, {4}, {5}, '{6}', {7})", taskID, roleID, 0, 0, focus, 0, addtime, nStarLevel);
                ret = conn.ExecuteNonQuery(cmdText);
                if (ret < 0) return ret;
                ret = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的位置信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRolePosition(DBManager dbMgr, int roleID, string position)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET position='{0}' WHERE rid={1}", position, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// Thiết lập kinh nghiệm và cấp độ cho nhân vật
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleInfo(DBManager dbMgr, int roleID, int level, long experience, int Prestige)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET level={0}, experience={1},roleprestige={2} WHERE rid={3}", level, experience, Prestige, roleID);

                Console.WriteLine("EXECUTE : " + cmdText);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// Cập nhật thông tin Avarta nhân vật
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="avartaID"></param>
        public static bool UpdateRoleAvarta(DBManager dbMgr, int roleID, int avartaID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET pic={0} WHERE rid={1}", avartaID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的游戏币1
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleMoney1(DBManager dbMgr, int roleID, int money)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET money1={0} WHERE rid={1}", money, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的银两
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleYinLiang(DBManager dbMgr, int roleID, int yinLiang)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET yinliang={0} WHERE rid={1}", yinLiang, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// Update tiền bang hội
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleID"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static bool UpdateRoleGuildMoney(DBManager dbMgr, int roleID, int Money)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET guildmoney={0} WHERE rid={1}", Money, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的金币
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleGold(DBManager dbMgr, int roleID, int gold)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET money2={0} WHERE rid={1}", gold, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的银两
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleStoreYinLiang(DBManager dbMgr, int roleID, long yinLiang)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET store_yinliang={0} WHERE rid={1}", yinLiang, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的银两
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleStoreMoney(DBManager dbMgr, int roleID, long money)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET store_money={0} WHERE rid={1}", money, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户的点卷
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateUserMoney(DBManager dbMgr, string userID, int userMoney)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, money) VALUES('{0}', {1}) ON DUPLICATE KEY UPDATE money={2}", userID, userMoney, userMoney);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户的元宝表的信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateUserMoney2(DBManager dbMgr, string userID, int userMoney, int realMoney, int giftID, int giftJiFen)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, money, realmoney, giftid, giftjifen) VALUES('{0}', {1}, {2}, {3}, {4}) ON DUPLICATE KEY UPDATE money={5}, realmoney={6}, giftid={7}, giftjifen={8}",
                    userID, userMoney, realMoney, giftID, giftJiFen, userMoney, realMoney, giftID, giftJiFen);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        public static bool UpdatetInputMoney(DBManager dbMgr, string UserID, int realmoney, int ZONEID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                DateTime Now = DateTime.Now;
                string TimeInsert = Now.ToString("yyyy-MM-dd H:mm:ss");
                long time = 1111;
                string cmdText = "";
                cmdText = "INSERT INTO t_inputlog (amount, u,order_no,cporder_no,time,sign,inputtime,result,zoneid) VALUES (" + realmoney + ",'" + UserID + "','" + UserID + "','" + UserID + "'," + time + ",'" + UserID + "','"+ TimeInsert + "','OK'," + ZONEID + ")";

                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        public static bool UpdateRechageData(DBManager dbMgr, string userID, int realmoney)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, realmoney) VALUES('{0}', {1}) ON DUPLICATE KEY UPDATE realmoney= realmoney + {2}", userID, realmoney, realmoney);

                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户的专享活动积分
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateUserSpecJiFen(DBManager dbMgr, string userID, int specjifen)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, specjifen) VALUES('{0}', {1}) ON DUPLICATE KEY UPDATE specjifen={2}", userID, specjifen, specjifen);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户的充值点
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateUserInputPoints(DBManager dbMgr, string userID, int ipoints)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, points) VALUES('{0}', {1}) ON DUPLICATE KEY UPDATE points={2}", userID, ipoints, ipoints);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户的充值积分
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateUserGiftJiFen(DBManager dbMgr, string userID, int giftJiFen)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_money (userid, giftjifen) VALUES('{0}', {1}) ON DUPLICATE KEY UPDATE giftjifen={2}", userID, giftJiFen, giftJiFen);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 移动一个物品
        /// </summary>
        public static int MoveGoods(DBManager dbMgr, int roleID, int goodsDbID)
        {
            int ret = -10;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_goods SET rid={0}, site=0 WHERE Id={1}", roleID, goodsDbID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的物品
        /// </summary>
        public static int NewGoods(DBManager dbMgr, int roleID, int goodsID, int goodsNum, string props, int forgeLevel, int binding, int site, int bagindex, string startTime, string endTime, int strong, int series, string otherpramer)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                try
                {
                    string cmdText = string.Format("INSERT INTO t_goods (rid, goodsid, Props, gcount, forge_level, binding, site, bagindex, starttime, endtime, strong, otherpramer,series) VALUES({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7}, '{8}', '{9}', {10}, '{11}', {12})",
                    roleID, goodsID, props, goodsNum, forgeLevel, binding, site, bagindex, startTime, endTime, strong, otherpramer, series);
                    ret = conn.ExecuteNonQuery(cmdText);
                    if (ret < 0) return ret;
                    ret = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                }
                catch (MySQLException)
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /// <summary>
        /// 格式化修改数据库的字符串
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="startIndex"></param>
        /// <param name="fieldNames"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldTypes"></param>
        /// <returns></returns>
        public static string FormatUpdateSQL(int id, string[] fields, int startIndex, string[] fieldNames, string tableName, byte[] fieldTypes, string idName = "Id")
        {
            //bool first = true;
            StringBuilder sb = new StringBuilder(256);
            //string sql = "UPDATE " + tableName + " SET ";
            sb.Append("UPDATE ").Append(tableName).Append(" SET ");
            for (int i = 0; i < fieldNames.Count(); i++)
            {
                if (fields[startIndex + i] == "*")
                {
                    continue;
                }

                if (fieldTypes[i] == 0) //Kiểu số
                {
                    sb.AppendFormat("{0}={1}", fieldNames[i], fields[startIndex + i]).Append(',');
                    //sql += string.Format("{0}={1}", fieldNames[i], fields[startIndex + i]);
                }
                else if (fieldTypes[i] == 1)//Kiểu chuỗi
                {
                    if (fieldNames[i] == "otherpramer" && fields[startIndex + i].Length < 10)
                    {
                        continue;
                    }
                    else
                    {
                        sb.AppendFormat("{0}='{1}'", fieldNames[i], fields[startIndex + i]).Append(',');
                    }

                    //sql += string.Format("{0}='{1}'", fieldNames[i], fields[startIndex + i]);
                }
                else if (fieldTypes[i] == 2)//Kiểu cộng với 1 giá trị
                {
                    sb.AppendFormat("{0}={1}+{2}", fieldNames[i], fieldNames[i], fields[startIndex + i]).Append(',');
                    //sql += string.Format("{0}={1}+{2}", fieldNames[i], fieldNames[i], fields[startIndex + i]);
                }
                else if (fieldTypes[i] == 3)// Kiểu thời gian
                {
                    sb.AppendFormat("{0}='{1}'", fieldNames[i], fields[startIndex + i].Replace('$', ':')).Append(',');
                    //sql += string.Format("{0}='{1}'", fieldNames[i], fields[startIndex + i].Replace('$', ':'));
                }

                // first = false;
            }
            sb[sb.Length - 1] = ' '; // remove the last character ','
            sb.AppendFormat(" WHERE {0}={1}", idName, id);
            //System.Console.WriteLine(sb.Length);
            if (sb.Length > 100)
            {
                //System.Console.WriteLine(sb.ToString());
            }
            return sb.ToString();
            //sql += string.Format(" WHERE {0}={1}", idName, id);
            //return sql;
        }

        /// <summary>
        /// Cập nhật thông tin vật phẩm
        /// </summary>
        public static int UpdateGoods(DBManager dbMgr, int id, string[] fields, int startIndex)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                try
                {
                    //Console.WriteLine("DO UPDATE GOOD");
                    string cmdText = FormatUpdateSQL(id, fields, startIndex, _UpdateGoods_fieldNames, "t_goods", _UpdateGoods_fieldTypes);
                    //Console.WriteLine(cmdText);
                    ret = conn.ExecuteNonQuery(cmdText);
                    //Console.WriteLine("DO UPDATE GOOD1111111");
                }
                catch
                {
                   // Console.WriteLine("DO UPDATE GOOD1111111TOACHHHHHHHHHHHHHHHHHHHH");
                }
            }

            return ret;
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        public static int RemoveGoods(DBManager dbMgr, int id)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_goods SET gcount=0 WHERE Id={0}", id);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        public static int MoveGoodsDataToBackupTable(DBManager dbMgr, int id)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                //string cmdText = string.Format("INSERT INTO {1} SELECT *,0,NOW(),0 FROM t_goods WHERE Id={0}", id, CurrentGoodsBakTableName);
                //conn.ExecuteNonQuery(cmdText);
                string cmdText = string.Format("DELETE FROM t_goods WHERE Id={0}", id);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        public static int SwitchGoodsBackupTable(DBManager dbMgr)
        {
            MySQLConnection conn = null;

            try
            {
                lock (GoodsBakTableMutex)
                {
                    conn = dbMgr.DBConns.PopDBConnection();

                    DateTime now = TimeUtil.NowDateTime();
                    int needIndex = now.Month % GoodsBakTableNames.Length;
                    if (GoodsBakTableIndex < 0)
                    {
                        GoodsBakTableIndex = needIndex;
                    }

                    int currentIndex = (GoodsBakTableIndex) % GoodsBakTableNames.Length;
                    if (needIndex != currentIndex)
                    {
                        string needTableName = GoodsBakTableNames[needIndex];
                        string sqlText = string.Format("SELECT id FROM {0} limit 1;", needTableName);
                        int result = ExecuteSQLReadInt(dbMgr, sqlText, conn);
                        if (result > 0)
                        {
                            sqlText = string.Format("TRUNCATE TABLE {0};", needTableName);
                            result = ExecuteSQLNoQuery(dbMgr, sqlText, conn);
                            if (result < 0)
                            {
                                LogManager.WriteLog(LogTypes.Error, "阶段物品备份表失败，不切换数据库");
                            }
                        }
                        else
                        {
                            GoodsBakTableIndex = needIndex;
                        }
                    }
                }
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return 0;
        }

        /// <summary>
        /// 完成某任务与之前所有任务(仅限对db操作完成，不走任务流程)
        /// </summary>
        /// <param name="dbMgr">db管理器</param>
        /// <param name="roleID">角色id</param>
        /// <param name="taskIDList">任务id列表</param>
        /// <returns></returns>
        public static bool WirterAutoCompletionTaskByTaskID(DBManager dbMgr, int roleID, List<int> taskIDList)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                // 循环从1开始 因为0下标保存的是roleid
                for (int i = 1; i < taskIDList.Count; i++)
                {
                    // 执行插入，如果存在 则将任务完成数count + 1
                    string cmdText = string.Format("INSERT INTO t_taskslog (taskid, rid, count) VALUES({0}, {1}, 1) ON DUPLICATE KEY UPDATE count=count+1", taskIDList[i], roleID);
                    conn.ExecuteNonQuery(cmdText);
                }
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        public static int UpdateTask(DBManager dbMgr, int dbID, string[] fields, int startIndex)
        {
            string cmdText = FormatUpdateSQL(dbID, fields, startIndex, _UpdateTask_fieldNames, "t_tasks", _UpdateTask_fieldTypes);

            DelayUpdateManager.getInstance().AddItemProsecc(cmdText);

            return 1;
        }

        /// <summary>
        /// 完成一个任务
        /// </summary>
        public static bool CompleteTask(DBManager dbMgr, int roleID, int npcID, int taskID, int dbID, int TaskClass)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_taskslog (taskid, rid, count,taskclass) VALUES({0}, {1}, 1, {2}) ON DUPLICATE KEY UPDATE count=count+1", taskID, roleID, TaskClass);
                conn.ExecuteNonQueryBool(cmdText);

                cmdText = string.Format("UPDATE t_tasks SET isdel=1 WHERE Id={0}", dbID);
                conn.ExecuteNonQueryBool(cmdText);

                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 完成一个任务
        /// </summary>
        public static bool DeleteTask(DBManager dbMgr, int roleID, int taskID, int dbID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_tasks SET isdel=1 WHERE Id={0}", dbID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// GM设置主线任务
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleID"></param>
        /// <param name="taskID"></param>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public static bool GMSetTask(DBManager dbMgr, int roleID, int taskID, List<int> taskIDList)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                for (int i = 1; i < taskIDList.Count - 1; i++)
                {
                    string cmdText = string.Format("INSERT INTO t_taskslog (taskid, rid, count) VALUES({0}, {1}, 1) ON DUPLICATE KEY UPDATE count=count+1", taskIDList[i], roleID);
                    conn.ExecuteNonQuery(cmdText);
                }
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 添加一个朋友
        /// </summary>
        public static int AddFriend(DBManager dbMgr, int dbID, int roleID, int otherID, int friendType, int relationship)
        {
            int ret = dbID;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                try
                {
                    bool error = false;
                    string cmdText = string.Format("INSERT INTO t_friends (myid, otherid, friendType, relationship) VALUES({0}, {1}, {2}, {3}) ON DUPLICATE KEY UPDATE friendType={4}", roleID, otherID, friendType, relationship, friendType);
                    error = !conn.ExecuteNonQueryBool(cmdText);
                    if (!error && dbID < 0)
                    {
                        ret = conn.GetSingleInt(string.Format("SELECT Id FROM t_friends where myid={0} and otherid={1}", roleID, otherID));
                    }
                }
                catch (MySQLException)
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /// <summary>
        /// 删除一个朋友
        /// </summary>
        public static bool RemoveFriend(DBManager dbMgr, int dbID, int roleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_friends WHERE Id={0}", dbID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 修改角色的PK模式
        /// </summary>
        public static bool UpdatePKMode(DBManager dbMgr, int roleID, int pkMode)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET pkmode={0} WHERE rid={1}", pkMode, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 修改角色的PK值和PKPoint
        /// </summary>
        public static bool UpdatePKValues(DBManager dbMgr, int roleID, int pkValue, int pkPoint)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET pkvalue={0}, pkpoint={1} WHERE rid={2}", pkValue, pkPoint, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 修改角色的杀BOSS的个数
        /// </summary>
        public static bool UpdateKillBoss(DBManager dbMgr, int roleID, int killBoss)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET killboss={0} WHERE rid={1}", killBoss, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// Lưu giá trị QuickKey của nhân vật vào DB
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleKeys(DBManager dbMgr, int roleID, int type, string keys)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = null;
                if (0 == type) //主快捷键映射
                {
                    cmdText = string.Format("UPDATE t_roles SET main_quick_keys='{0}' WHERE rid={1}", keys, roleID);
                }
                else //辅助快捷键
                {
                    cmdText = string.Format("UPDATE t_roles SET other_quick_keys='{0}' WHERE rid={1}", keys, roleID);
                }
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的剩余自动战斗时间
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLeftFightSecs(DBManager dbMgr, int roleID, int leftFightSecs)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET leftfightsecs={0} WHERE rid={1}", leftFightSecs, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新公告
        /// </summary>
        public static int NewBulletinText(DBManager dbMgr, string msgID, int toPlayNum, string bulletinText)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_bulletin (msgid, toplaynum, bulletintext, bulletintime) VALUES('{0}', {1}, '{2}', '{3}') ON DUPLICATE KEY UPDATE toplaynum={4}, bulletintext='{5}', bulletintime='{6}'", msgID, toPlayNum, bulletinText, today, toPlayNum, bulletinText, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除一个旧公告
        /// </summary>
        public static int RemoveBulletinText(DBManager dbMgr, string msgID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_bulletin WHERE msgid='{0}'", msgID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个游戏配置参数
        /// </summary>
        public static int UpdateGameConfig(DBManager dbMgr, string paramName, string paramValue)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_config (paramname, paramvalue) VALUES('{0}', '{1}') ON DUPLICATE KEY UPDATE paramvalue='{2}'", paramName, paramValue, paramValue);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// Lưu kỹ năng vào DB
        /// </summary>
        public static int AddSkill(DBManager dbMgr, int roleID, int skillID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                try
                {
                    string cmdText = string.Format("INSERT INTO t_skills (rid, skillid, skilllevel, lastusedtick, cooldowntick, exp) VALUES({0}, {1}, 0, 0, 0, 0)", roleID, skillID);
                    ret = conn.ExecuteNonQuery(cmdText);
                    if (ret >= 0)
                    {
                        ret = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                    }
                }
                catch (MySQLException)
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /// <summary>
        /// Xóa kỹ năng khỏi DB
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleID"></param>
        /// <param name="skillID"></param>
        /// <returns></returns>
        public static bool DeleteSkill(DBManager dbMgr, int roleID, int skillID)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                try
                {
                    string cmdText = string.Format("DELETE FROM t_skills WHERE rid = {0} AND skillid = {1}", roleID, skillID);
                    return conn.ExecuteNonQueryBool(cmdText);
                }
                catch (MySQLException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Làm mới thông tin kỹ năng trong DB
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="skillID"></param>
        /// <param name="skillLevel"></param>
        /// <param name="lastUsedTick"></param>
        public static bool UpdateSkillInfo(DBManager dbMgr, int roleID, int skillID, int skillLevel, long lastUsedTick, int cooldownTick, int exp)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_skills SET skilllevel={0}, lastusedtick={1}, cooldowntick={2}, exp={3} WHERE skillid={4} AND rid={5}", skillLevel, lastUsedTick, cooldownTick, exp, skillID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个用户的buffer项
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRoleBufferItem(DBManager dbMgr, int roleID, int bufferID, long startTime, long bufferSecs, long bufferVal, string customProperty)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_buffer (rid, bufferid, starttime, buffersecs, bufferval, custom_property) VALUES({0}, {1}, {2}, {3}, {4}, '{5}') ON DUPLICATE KEY UPDATE starttime={6}, buffersecs={7}, bufferval={8}",
                        roleID, bufferID, startTime, bufferSecs, bufferVal, customProperty, startTime, bufferSecs, bufferVal);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的日跑环任务数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRoleDailyTaskData(DBManager dbMgr, int roleID, int huanID, string rectime, int recnum, int taskClass, int extDayID, int extNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_dailytasks (rid, huanid, rectime, recnum, taskClass, extdayid, extnum) VALUES({0}, {1}, '{2}', {3}, {4}, {5}, {6}) ON DUPLICATE KEY UPDATE huanid={1}, rectime='{2}', recnum={3}, taskClass={4}, extdayid={5}, extnum={6}",
                        roleID, huanID, rectime, recnum, taskClass, extDayID, extNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的每日冲穴次数数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRoleDailyJingMaiData(DBManager dbMgr, int roleID, string jmTime, int jmNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_dailyjingmai (rid, jmtime, jmnum) VALUES({0}, '{1}', {2}) ON DUPLICATE KEY UPDATE jmtime='{3}', jmnum={4}",
                        roleID, jmTime, jmNum, jmTime, jmNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的已经完成的主线任务的ID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleMainTaskID(DBManager dbMgr, int roleID, int mainTaskID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET maintaskid={0} WHERE rid={1}", mainTaskID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的随身仓库信息的操作
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRolePBInfo(DBManager dbMgr, int roleID, int extGridNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_ptbag (rid, extgridnum) VALUES({0}, {1}) ON DUPLICATE KEY UPDATE extgridnum={2}",
                        roleID, extGridNum, extGridNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色背包格子数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRoleBagNum(DBManager dbMgr, int roleID, int bagNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles set bagnum={0} where rid={1}", bagNum, roleID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 创建角色的活动数据
        /// </summary>
        public static void CreateHuoDong(DBManager dbMgr, int roleID)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_huodong (rid, loginweekid, logindayid, loginnum, newstep, steptime, lastmtime, curmid, curmtime, songliid, logingiftstate, onlinegiftstate) VALUES({0}, '{1}', '{2}', {3}, {4}, '{5}', {6}, '{7}', {8}, {9}, {10}, {11})",
                    roleID, "", "", 0, 0, today, 0, "", 0, 0, 0, 0);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 修改活动数据
        /// </summary>
        public static int UpdateHuoDong(DBManager dbMgr, int id, string[] fields, int startIndex)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = FormatUpdateSQL(id, fields, startIndex, _UpdateActivity_fieldNames, "t_huodong", _UpdateActivity_fieldTypes, "rid");

                LogManager.WriteLog(LogTypes.SQL, "UPDATE UpdateHuoDong DATA :" + cmdText);

                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 清空礼品码的数据
        /// </summary>
        public static int ClearAllLiPinMa(DBManager dbMgr)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_linpinma");
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 插入一个新的礼品码的数据
        /// </summary>
        public static int InsertNewLiPinMa(DBManager dbMgr, string liPinMa, string songLiID, string maxNum, string ptid, string ptRepeat, string usedNum = "0")
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_linpinma (lipinma, huodongid, maxnum, usednum, ptid, ptrepeat) VALUES('{0}', {1}, {2}, {3}, {4}, {5})",
                    liPinMa, songLiID, maxNum, usedNum, ptid, ptRepeat);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 修改一个礼品码的使用次数
        /// </summary>
        public static int UpdateLiPinMaUsedNum(DBManager dbMgr, string liPinMa, int usedNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_linpinma SET usednum={0} WHERE lipinma='{1}'",
                    usedNum, liPinMa);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除一个用户角色的专享活动数据
        /// </summary>
        public static int DeleteSpecialActivityData(DBManager dbMgr, int roleID, int groupID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                if (groupID == 0) // 全部删除
                {
                    cmdText = string.Format("DELETE FROM t_special_activity WHERE rid={0}", roleID);
                }
                else // 按组删除
                {
                    cmdText = string.Format("DELETE FROM t_special_activity WHERE rid={0} AND groupid={1}", roleID, groupID);
                }
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的专享活动数据
        /// </summary>
        public static int UpdateSpecialActivityData(DBManager dbMgr, int roleID, SpecActInfoDB SpecAct)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format(@"INSERT INTO t_special_activity (rid, groupid, actid, purchaseNum, countNum, active) VALUES({0}, {1}, {2}, {3}, {4}, {5})
                        ON DUPLICATE KEY UPDATE groupid={1}, actid={2}, purchaseNum={3}, countNum={4}, active={5}",
                        roleID, SpecAct.GroupID, SpecAct.ActID, SpecAct.PurNum, SpecAct.CountNum, SpecAct.Active);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的副本数据 -- 副本改造 增加最快通关时间  add by liaowei
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateFuBenData(DBManager dbMgr, int roleID, int fuBenID, int dayID, int enterNum, int nQuickPassTimeSec, int nFinishNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_fuben (rid, fubenid, dayid, enternum, quickpasstimer, finishnum) VALUES({0}, {1}, {2}, {3}, {4}, {5}) ON DUPLICATE KEY UPDATE fubenid={1}, dayid={2}, enternum={3}, quickpasstimer={4}, finishnum={5}",
                        roleID, fuBenID, dayID, enterNum, nQuickPassTimeSec, nFinishNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 插入一个新的预先分配的名字
        /// </summary>
        public static int InsertNewPreName(DBManager dbMgr, string preName, int sex)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_prenames (name, sex, used) VALUES('{0}', {1}, {2})",
                       preName, sex, 0);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 修改一个预先分配的名字的使用
        /// </summary>
        public static int UpdatePreNameUsedState(DBManager dbMgr, string preName, int used)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_prenames SET used={0} WHERE name='{1}'",
                    used, preName);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 插入一个新的副本通关历史记录
        /// </summary>
        public static int InsertNewFuBenHist(DBManager dbMgr, int fuBenID, int roleID, string roleName, int usedSecs)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_fubenhist (fubenid, rid, rname, usedsecs) VALUES({0}, {1}, '{2}', {3}) ON DUPLICATE KEY UPDATE rid={4}, rname='{5}', usedsecs={6}",
                    fuBenID, roleID, roleName, usedSecs, roleID, roleName, usedSecs);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的每日数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateRoleDailyData(DBManager dbMgr, int roleID, int expDayID, int todayExp, int lingLiDayID, int todayLingLi, int killBossDayID, int todayKillBoss, int fuBenDayID, int todayFuBenNum, int wuXingDayID, int wuXingNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_dailydata (rid, expdayid, todayexp, linglidayid, todaylingli, killbossdayid, todaykillboss, fubendayid, todayfubennum, wuxingdayid, wuxingnum) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}) ON DUPLICATE KEY UPDATE expdayid={1}, todayexp={2}, linglidayid={3}, todaylingli={4}, killbossdayid={5}, todaykillboss={6}, fubendayid={7}, todayfubennum={8}, wuxingdayid={9}, wuxingnum={10}",
                        roleID, expDayID, todayExp, lingLiDayID, todayLingLi, killBossDayID, todayKillBoss, fuBenDayID, todayFuBenNum, wuXingDayID, wuXingNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        public static int UpdateRoleRanking(DBManager dbMgr, int roleID, string rname, int level, int occupation, int sub_id, int monphai, Int64 taiphu, int volam, int liendau, int uydanh)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_ranking (rid, rname, level, occupation, sub_id, monphai, taiphu, volam, liendau, uydanh) VALUES({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}) ON DUPLICATE KEY UPDATE  level={2}, monphai={5}, taiphu={6}, volam={7}, liendau={8}, uydanh={9}",
                        roleID, rname, level, occupation, sub_id, monphai, taiphu, volam, liendau, uydanh);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的押镖数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateYaBiaoData(DBManager dbMgr, int roleID, int yaBiaoID, long startTime, int state, int lineID, int touBao, int yaBiaoDayID, int yaBiaoNum, int takeGoods)
        {
            int ret = -1;
            string today = "1900-01-01 12:00:00";
            if (startTime > 0)
            {
                today = (new DateTime(startTime * 10000)).ToString("yyyy-MM-dd HH:mm:ss");
            }
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_yabiao (rid, yabiaoid, starttime, state, lineid, toubao, yabiaodayid, yabiaonum, takegoods) VALUES({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7}, {8}) ON DUPLICATE KEY UPDATE yabiaoid={1}, starttime='{2}', state={3}, lineid={4}, toubao={5}, yabiaodayid={6}, yabiaonum={7}, takegoods={8}",
                        roleID, yaBiaoID, today, state, lineID, touBao, yaBiaoDayID, yaBiaoNum, takeGoods);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新一个用户角色的押镖数据的状态
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateYaBiaoDataState(DBManager dbMgr, int roleID, int state)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_yabiao SET state={0} WHERE rid={1}",
                        state, roleID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的商城购买记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewMallBuyItem(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftMoney)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_mallbuy (rid, goodsid, goodsnum, totalprice, leftmoney, buytime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')",
                    roleID, goodsID, goodsNum, totalPrice, leftMoney, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的奇珍阁购买记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewQiZhenGeBuyItem(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftMoney)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_qizhengebuy (rid, goodsid, goodsnum, totalprice, leftmoney, buytime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')",
                                                roleID, goodsID, goodsNum, totalPrice, leftMoney, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的竞猜记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewShengXiaoGuessHistory(DBManager dbMgr, int roleID, string roleName, int zoneID, int guessKey, int mortgage, int resultKey, int gainNum, int leftMortgage)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_shengxiaoguesshist (rid, rname, guesskey, mortgage, resultkey, gainnum, leftmortgage, zoneid, guesstime) VALUES({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, '{8}')",
                                        roleID, roleName, guessKey, mortgage, resultKey, gainNum, leftMortgage, zoneID, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的银票购买记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewYinPiaoBuyItem(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftYinPiaoNum)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_yinpiaobuy (rid, goodsid, goodsnum, totalprice, leftyinpiao, buytime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')",
                    roleID, goodsID, goodsNum, totalPrice, leftYinPiaoNum, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的在线人数记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewOnlineNumItem(DBManager dbMgr, int totalNum, DateTime dateTime, String strMapOnlineInfo)
        {
            int ret = -1;
            string today = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_onlinenum (num, rectime, mapnum) VALUES({0}, '{1}', '{2}')",
                    totalNum, today, strMapOnlineInfo);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加刷新奇珍阁的记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void AddRefreshQiZhenGeRec(DBManager dbMgr, int roleID, int oldUserMoney, int leftUserMoney)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_refreshqizhen (rid, oldusermoney, leftusermoney, refreshtime) VALUES({0}, {1}, {2}, '{3}')",
                     roleID, oldUserMoney, leftUserMoney, today);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 添加使用礼品码的平台记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void AddUsedLiPinMa(DBManager dbMgr, int huodongID, string lipinMa, int pingTaiID, int roleID)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_usedlipinma (lipinma, huodongid, ptid, rid) VALUES('{0}', {1}, {2}, {3})",
                     lipinMa, huodongID, pingTaiID, roleID);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 添加元宝消费记录告警记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void AddMoneyWarning(DBManager dbMgr, int roleID, int usedMoney, int goodsMoney)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_warning (rid, usedmoney, goodsmoney, warningtime) VALUES({0}, {1}, {2}, '{3}')",
                     roleID, usedMoney, goodsMoney, today);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 添加一个新的物品购买记录【不管用什么购买的，都通过购买类型识别】,从npc购买
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewBuyItemFromNpc(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftMoney, int moneyType)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_npcbuy (rid, goodsid, goodsnum, totalprice, leftmoney, buytime, moneytype) VALUES({0}, {1}, {2}, {3}, {4}, '{5}', {6})",
                    roleID, goodsID, goodsNum, totalPrice, leftMoney, today, moneyType);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的银两购买记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewYinLiangBuyItem(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftYinLiang)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_yinliangbuy (rid, goodsid, goodsnum, totalprice, leftyinliang, buytime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')",
                    roleID, goodsID, goodsNum, totalPrice, leftYinLiang, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的金币购买记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewGoldBuyItem(DBManager dbMgr, int roleID, int goodsID, int goodsNum, int totalPrice, int leftGold)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_goldbuy (rid, goodsid, goodsnum, totalprice, leftgold, buytime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')",
                    roleID, goodsID, goodsNum, totalPrice, leftGold, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新上次扫描的充值日志的ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="id"></param>
        public static void UpdateLastScanInputLogID(DBManager dbMgr, int lastid)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_inputhist (Id, lastid) VALUES(1, {0}) ON DUPLICATE KEY UPDATE lastid={0}", lastid);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        #region 邮件相关

        /// <summary>
        /// 更新邮件已读标志[包括标志位和读取时间]
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void UpdateMailHasReadFlag(DBManager dbMgr, int mailID, int rid)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_mail SET isread=1, readtime=now() where mailid={0} and receiverrid={1}", mailID, rid);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái có thể nhận quà hay không
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateMailHasFetchGoodsFlag(DBManager dbMgr, int mailID, int rid, int hasFetchAttachment)
        {
            bool ret = true;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText;
                /// Nếu là khóa thao tác nhận
                if (hasFetchAttachment == 0)
                {
                    cmdText = string.Format("UPDATE t_mail SET hasfetchattachment={0}, bound_money={1}, bound_token={2} where mailid={3} and receiverrid={4}", hasFetchAttachment, 0, 0, mailID, rid);
                }
                else
                {
                    cmdText = string.Format("UPDATE t_mail SET hasfetchattachment={0} where mailid={1} and receiverrid={2}", hasFetchAttachment, mailID, rid);
                }
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除邮件实体数据，不包括附件列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool DeleteMailDataItemExcludeGoodsList(DBManager dbMgr, int mailID, int rid)
        {
            bool ret = true;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE from t_mail where mailid={0} and receiverrid={1}", mailID, rid);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 删除邮件附件物品列表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void DeleteMailGoodsList(DBManager dbMgr, int mailID)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE from t_mailgoods where mailid={0}", mailID);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// Tạo thư mới
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddMailBody(DBManager dbMgr, int senderrid, string senderrname, int receiverrid, string reveiverrname, string subject, string content, int hasFetchAttachment, int boundMoney, int boundToken)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int mailID = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                //添加邮件主体数据
                string cmdText = string.Format("INSERT INTO t_mail (senderrid, senderrname, sendtime, receiverrid, reveiverrname, readtime, " +
                    "isread, mailtype, hasfetchattachment, subject,content, bound_money, bound_token) VALUES (" +
                    "{0},'{1}','{2}', {3}, '{4}','{5}',{6},{7},{8},'{9}','{10}', {11}, {12})",
                    senderrid, senderrname, today, receiverrid, reveiverrname, "2000-11-11 11:11:11",
                    0, 1, hasFetchAttachment, subject, content, boundMoney, 0);
                int ret = conn.ExecuteNonQuery(cmdText);
                if (ret < 0)
                {
                    mailID = -2; //添加新邮件失败
                    return mailID;
                }

                try
                {
                    mailID = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                }
                catch (MySQLException)
                {
                    mailID = -3; //添加新邮件失败
                }
            }

            return mailID;
        }

        /// <summary>
        /// 添加邮件附件数据,如果都成功，返回成功添加的物品数量，如果存在失败，返回值小于0
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool AddMailGoodsDataItem(DBManager dbMgr, int mailID, int goodsid, int forge_level, string Props, int gcount, int binding, int series, string otherParams, int strong)
        {
            bool ret = true;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                cmdText = string.Format("INSERT INTO t_mailgoods (mailid, goodsid, forge_level, Props, gcount, binding, series, otherpramer, strong) VALUES ({0}, {1}, {2}, '{3}', {4}, {5}, {6}, '{7}', {8})",
                        mailID, goodsid, forge_level, Props, gcount, binding, series, otherParams, strong);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 在临时扫描表中添加一条新邮件ID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void UpdateLastScanMailID(DBManager dbMgr, int roleID, int mailID)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_mailtemp (receiverrid, mailid) " +
                                                "VALUES ({0}, {1})", roleID, mailID);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 清除过期邮件
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void ClearOverdueMails(DBManager dbMgr, DateTime overdueTime)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_mailgoods WHERE mailid IN (SELECT mailid FROM t_mail WHERE sendtime < '{0}');", overdueTime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (conn.ExecuteNonQuery(cmdText) < 0) return;

                cmdText = string.Format("DELETE from t_mail where sendtime < '{0}'", overdueTime.ToString("yyyy-MM-dd HH:mm:ss"));
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 删除临时扫描表中上次扫描到的新邮件ID[roleid,mailid 的辞典列表，mailid是扫描到的最大mailid]
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void DeleteLastScanMailIDs(DBManager dbMgr, Dictionary<int, int> lastMailDict)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string sWhere = "";
                //生成where语句
                foreach (var item in lastMailDict)
                {
                    if (sWhere.Length > 0)
                    {
                        sWhere += " or ";
                    }
                    else
                    {
                        sWhere = " where ";
                    }

                    sWhere += string.Format(" (mailid<={0} and receiverrid={1}) ", item.Value, item.Key);
                }
                string cmdText = string.Format("DELETE from t_mailtemp {0}", sWhere);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 删除邮件临时表中邮件ID【删除邮件时调用】
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static void DeleteMailIDInMailTemp(DBManager dbMgr, int mailID)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE from t_mailtemp where mailid={0}", mailID);
                conn.ExecuteNonQuery(cmdText);
            }
        }

        /// <summary>
        /// 更新一个离线用户角色新邮件字段
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLastMail(DBManager dbMgr, int roleID, int mailID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET lastmailid={0} WHERE rid={1}", mailID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 邮件相关

        /// <summary>
        /// 添加一个新的活动排行记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddHongDongPaiHangRecord(DBManager dbMgr, int rid, string rname, int zoneid, int huoDongType, int paihang, string paihangtime, int phvalue)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_huodongpaihang (rid, rname, zoneid, type, paihang, paihangtime, phvalue) VALUES({0}, '{1}', {2}, {3}, {4}, '{5}', {6})",
                    rid, rname, zoneid, huoDongType, paihang, paihangtime, phvalue);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的活动奖励记录,针对角色
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddHongDongAwardRecordForRole(DBManager dbMgr, int rid, int zoneid, int activitytype, string keystr, int hasgettimes, string lastgettime)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_huodongawardrolehist (rid, zoneid, activitytype, keystr, hasgettimes,lastgettime) VALUES({0}, {1}, {2}, '{3}', {4}, '{5}')",
                    rid, zoneid, activitytype, keystr, hasgettimes, lastgettime);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加一个新的活动奖励记录，针对用户
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddHongDongAwardRecordForUser(DBManager dbMgr, string userid, int activitytype, string keystr, long hasgettimes, string lastgettime)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_huodongawarduserhist (userid, activitytype, keystr, hasgettimes,lastgettime) VALUES('{0}', {1}, '{2}', {3}, '{4}')",
                    userid, activitytype, keystr, hasgettimes, lastgettime);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新活动奖励记录【根据角色id，用户id，活动类型，活动关键字作为条件更新活动奖励记录，主要用于针对角色的奖励记录】
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateHongDongAwardRecordForRole(DBManager dbMgr, int rid, int zoneid, int activitytype, string keystr, int hasgettimes, string lastgettime)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_huodongawardrolehist SET hasgettimes={0}, lastgettime='{1}' WHERE rid={2} AND zoneid={3} AND activitytype={4} AND keystr='{5}' AND hasgettimes!={0}",
                    hasgettimes, lastgettime, rid, zoneid, activitytype, keystr, hasgettimes);
                //返回影响的行数，返回 0 表示没有任何影响的行数，对于执行表结构更改等，返回-1也是成功，失败抛异常
                //这儿采用返回影响行数来判断本次操作是否真的成功,对于奖励历史记录的更新，如果hasgettimes已经被设置了
                //本次要设置的值，表示本次设置无效，这样可以避免多个指令同时采用同样的值更改这个字段
                ret = conn.ExecuteNonQuery(cmdText) > 0 ? 0 : -1;
            }

            return ret;
        }

        /// <summary>
        /// 更新活动奖励记录【根据用户id，活动类型，活动关键字作为条件更新活动奖励记录，主要用于针对用户的奖励记录,比如充值奖励】
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateHongDongAwardRecordForUser(DBManager dbMgr, string userid, int activitytype, string keystr, long hasgettimes, string lastgettime)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("update t_huodongawarduserhist set hasgettimes={0}, lastgettime='{1}' where userid='{2}' and activitytype={3} and keystr='{4}' and hasgettimes!={5}",
                    hasgettimes, lastgettime, userid, activitytype, keystr, hasgettimes);
                //返回影响的行数，返回 0 表示没有任何影响的行数，对于执行表结构更改等，返回-1也是成功，失败抛异常
                //这儿采用返回影响行数来判断本次操作是否真的成功,对于奖励历史记录的更新，如果hasgettimes已经被设置了
                //本次要设置的值，表示本次设置无效，这样可以避免多个指令同时采用同样的值更改这个字段
                ret = conn.ExecuteNonQuery(cmdText) > 0 ? 0 : -1;
            }

            return ret;
        }

        /// <summary>
        /// 添加限购物品的历史记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddLimitGoodsBuyItem(DBManager dbMgr, int roleID, int goodsID, int dayID, int usedNum)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_limitgoodsbuy (rid, goodsid, dayid, usednum) VALUES({0}, {1}, {2}, {3}) ON DUPLICATE KEY UPDATE dayID={2}, usedNum={3}",
                    roleID, goodsID, dayID, usedNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加VIP日数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddVipDailyData(DBManager dbMgr, int roleID, int priorityType, int dayID, int usedTimes)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_vipdailydata (rid, prioritytype, dayid, usedtimes) VALUES({0}, {1}, {2}, {3}) ON DUPLICATE KEY UPDATE dayid={2}, usedtimes={3}",
                    roleID, priorityType, dayID, usedTimes);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加杨公宝库每日积分奖励数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddYangGongBKDailyJiFenData(DBManager dbMgr, int roleID, int jifen, int dayID, long awardhistory)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_yangguangbkdailydata (rid, jifen, dayid, awardhistory) VALUES({0}, {1}, {2}, {3}) ON DUPLICATE KEY UPDATE jifen={1}, dayid={2}, awardhistory={3}",
                    roleID, jifen, dayID, awardhistory);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #region 数据库ID字段自增长相关

        /// <summary>
        /// 更改数据库相关表的自增长值
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="sTableName"></param>
        /// <param name="nAutoIncrementValue"></param>
        /// <returns></returns>
        public static int ChangeTablesAutoIncrementValue(DBManager dbMgr, string sTableName, int nAutoIncrementValue)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("alter table {0} auto_increment={1}", sTableName, nAutoIncrementValue);
                conn.ExecuteNonQuery(cmdText);
                return 0; //调用方要求必须返回0
            }
        }

        #endregion 数据库ID字段自增长相关

        /// <summary>
        /// 更新一个角色的物品使用次数限制
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateGoodsLimit(DBManager dbMgr, int roleID, int goodsID, int dayID, int usedNum)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_goodslimit (rid, goodsid, dayid, usednum) VALUES({0}, {1}, {2}, {3}) ON DUPLICATE KEY UPDATE dayid={2}, usednum={3}", roleID, goodsID, dayID, usedNum);
                return conn.ExecuteNonQueryBool(cmdText);
            }
        }

        /// <summary>
        /// Update role prams của nhân vật
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleParams(DBManager dbMgr, int roleID, string name, string value, RoleParamType roleParamType = null)
        {
            if (roleParamType == null)
            {
                roleParamType = RoleParamNameInfo.GetRoleParamType(name, value);
            }

            string cmdText = string.Format("INSERT INTO `{3}` (`rid`, `{4}`, `{5}`) VALUES({0}, {1}, '{2}') ON DUPLICATE KEY UPDATE `{5}`='{2}'",
                                                 roleID, roleParamType.KeyString, value, roleParamType.TableName, roleParamType.IdxName, roleParamType.ColumnName);

           // DelayUpdateManager.getInstance().AddItemProsecc(cmdText);


            try
            {
                using (MyDbConnection3 conn = new MyDbConnection3())
                {
                    // Console.WriteLine("DEALY MYSQL UDPATE  ITEM ==>TOTALCOUNT111111");
                    conn.ExecuteNonQueryBool(cmdText);
                    return true;
                    //Console.WriteLine("DEALY MYSQL UDPATE  ITEM ==>TOTALCOUNT3333333");
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.SQL, "[BUG]Exception=" + ex.ToString());
                return false;

            }

            return true;


        }

        #region 限时抢购相关



        /// <summary>
        /// 添加一个新的限时抢购项记录,返回插入ID
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewQiangGouItem(DBManager dbMgr, int group, int random, int itemid, int goodsid, int origprice, int price,
            int singlepurchase, int fullpurchase, int daystime)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_qianggouitem (itemgroup, random, itemid, goodsid, origprice, price, singlepurchase, fullpurchase, daystime," +
                    " starttime, endtime, istimeover) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}','{10}', {11})",
                    group, random, itemid, goodsid, origprice, price, singlepurchase, fullpurchase, daystime, today, today, 0);//反正没结束，开始时间和结束时间设置成同一个
                ret = conn.ExecuteNonQuery(cmdText);
                try
                {
                    if (ret >= 0)
                    {
                        ret = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                    }
                }
                catch (MySQLException)
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /// <summary>
        /// 设置抢购项结束标志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="qiangGouId"></param>
        /// <returns></returns>
        public static bool UpdateQiangGouItemTimeOverFlag(DBManager dbMgr, int qiangGouId)
        {
            bool ret = false;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_qianggouitem SET istimeover=1, endtime='{0}' WHERE Id={1}", today, qiangGouId);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 限时抢购相关

        #region 砸金蛋相关

        /// <summary>
        /// 添加一个新的砸金蛋记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewZaJinDanHistory(DBManager dbMgr, int roleID, string roleName, int zoneID, int timesselected, int usedyuanbao, int usedjindan, int gaingoodsid, int gaingoodsnum, int gaingold, int gainyinliang, int gainexp, string srtProp)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_zajindanhist (rid, rname, zoneid, timesselected, usedyuanbao, usedjindan, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp, strprop, operationtime)" +
                    " VALUES ({0},'{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},'{11}','{12}')",
                    roleID, roleName, zoneID, timesselected, usedyuanbao, usedjindan, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp, srtProp, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 砸金蛋相关

        #region 开服在线大礼

        /// <summary>
        /// 添加开服在线奖励项
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="dayID"></param>
        /// <param name="yuanBao"></param>
        /// <param name="totalRoleNum"></param>
        /// <param name="zoneID"></param>
        public static int AddKaiFuOnlineAward(DBManager dbMgr, int rid, int dayID, int yuanBao, int totalRoleNum, int zoneID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_kfonlineawards (rid, dayid, yuanbao, totalrolenum, zoneid)" +
                    " VALUES ({0},{1},{2},{3},{4}) ON DUPLICATE KEY UPDATE rid={0}, yuanbao={2}, totalrolenum={3}",
                    rid, dayID, yuanBao, totalRoleNum, zoneID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 开服在线大礼

        #region 开服在线大礼

        /// <summary>
        /// 添加系统给予的元宝记录项
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="dayID"></param>
        /// <param name="yuanBao"></param>
        /// <param name="totalRoleNum"></param>
        /// <param name="zoneID"></param>
        public static int AddSystemGiveUserMoney(DBManager dbMgr, int rid, int yuanBao, string giveType)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_givemoney (rid, yuanbao, rectime, givetype)" +
                    " VALUES ({0},{1},'{2}','{3}')",
                    rid, yuanBao, today, giveType);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 开服在线大礼

        #region 交易和掉落日志

        /// <summary>
        /// 添加物品交易日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="goodsid"></param>
        /// <param name="goodsnum"></param>
        /// <param name="leftgoodsnum"></param>
        /// <param name="otherroleid"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int AddExchange1Item(DBManager dbMgr, int rid, int goodsid, int goodsnum, int leftgoodsnum, int otherroleid, string result)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_exchange1 (rid, goodsid, goodsnum, leftgoodsnum, otherroleid, result, rectime)" +
                    " VALUES ({0},{1},{2},{3},{4},'{5}','{6}')",
                    rid, goodsid, goodsnum, leftgoodsnum, otherroleid, result, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加铜钱交易日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="goodsid"></param>
        /// <param name="goodsnum"></param>
        /// <param name="leftgoodsnum"></param>
        /// <param name="otherroleid"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int AddExchange2Item(DBManager dbMgr, int rid, int yinliang, int leftyinliang, int otherroleid)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_exchange2 (rid, yinliang, leftyinliang, otherroleid, rectime)" +
                    " VALUES ({0},{1},{2},{3},'{4}')",
                    rid, yinliang, leftyinliang, otherroleid, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 添加元宝交易日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="rid"></param>
        /// <param name="goodsid"></param>
        /// <param name="goodsnum"></param>
        /// <param name="leftgoodsnum"></param>
        /// <param name="otherroleid"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int AddExchange3Item(DBManager dbMgr, int rid, int yuanbao, int leftyuanbao, int otherroleid)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_exchange3 (rid, yuanbao, leftyuanbao, otherroleid, rectime)" +
                    " VALUES ({0},{1},{2},{3},'{4}')",
                    rid, yuanbao, leftyuanbao, otherroleid, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 交易和掉落日志

        #region 月度抽奖

        /// <summary>
        /// 添加一个月度抽奖记录
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int AddNewYueDuChouJiangHistory(DBManager dbMgr, int roleID, string roleName, int zoneID, int gaingoodsid, int gaingoodsnum, int gaingold, int gainyinliang, int gainexp)
        {
            int ret = -1;
            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_yueduchoujianghist (rid, rname, zoneid, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp, operationtime)" +
                    " VALUES ({0},'{1}',{2},{3},{4},{5},{6},{7},'{8}')",
                    roleID, roleName, zoneID, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 月度抽奖

        #region 转职

        /// <summary>
        /// Lưu thông tin môn phái và nhánh của người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleID"></param>
        /// <param name="factionID"></param>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public static bool UpdateRoleFactionAndRoute(DBManager dbMgr, int roleID, int factionID, int routeID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET occupation={0}, sub_id={1} WHERE rid={2}", factionID, routeID, roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 转职

        #region 血色堡垒

        /// <summary>
        /// 更新当天进入的次数
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateBloodCastleEnterCount(DBManager dbMgr, int nRoleID, int nDate, int nType, int nCount, string lastgettime)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_dayactivityinfo(roleid, activityid, timeinfo, triggercount, lastgettime) VALUES({0}, {1}, {2}, {3}, '{4}')",
                                                nRoleID, nType, nDate, nCount, lastgettime);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        #endregion 血色堡垒

        #region 新手场景

        /// <summary>
        /// 完成新手场景
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleInfoForFlashPlayerFlag(DBManager dbMgr, int nRoleID, int isflashplayer)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET isflashplayer={1} WHERE rid={0}", nRoleID, isflashplayer);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 新手场景logout 经验处理
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleExpForFlashPlayerWhenLogOut(DBManager dbMgr, int nRoleID)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string strQuickKey = "";
                string cmdText = string.Format("UPDATE t_roles SET experience={1}, maintaskid={1}, main_quick_keys='{2}' WHERE rid={0}", nRoleID, 0, strQuickKey);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 新手场景logout 等级处理
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLevForFlashPlayerWhenLogOut(DBManager dbMgr, int nRoleID)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET level={1} WHERE rid={0}", nRoleID, 1);

                Console.WriteLine("EXECUTE :" + cmdText);

                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// Update MarkValue
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="nRoleID"></param>
        /// <param name="TimeRager"></param>
        /// <param name="MarkValue"></param>
        /// <param name="MarkType"></param>
        /// <returns></returns>
        public static bool UpdateMarkData(DBManager dbMgr, int nRoleID, string TimeRager, int MarkValue, int MarkType)
        {
            bool bRet = false;

            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "SELECT count(*) from t_mark where RoleID= " + nRoleID + " and TimeRanger = '" + TimeRager + "' and MarkType= " + MarkType + "";
                int Value = conn.GetSingleInt(cmdText);
                if (Value > 0)
                {
                    // Update giá trị của MarkValue
                    cmdText = "Update t_mark set MarkValue = " + MarkValue + " where TimeRanger= '" + TimeRager + "' and RoleID= " + nRoleID + " and MarkType = " + MarkType + "";
                }
                else
                {
                    cmdText = "Insert into t_mark(RoleID,TimeRanger,MarkValue,MarkType) VALUES (" + nRoleID + ",'" + TimeRager + "'," + MarkValue + "," + MarkType + ")";
                }

                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// Ghi vào recore
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="nRoleID"></param>
        /// <param name="RoleName"></param>
        /// <param name="ValueRecore"></param>
        /// <param name="RecoryType"></param>
        /// <returns></returns>
        public static bool AddRecore(DBManager dbMgr, int nRoleID, string RoleName, int ValueRecore, int RecoryType)
        {
            bool bRet = false;

            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "Insert into t_recore(RoleID,RoleName,RecoryType,DateRecore,ValueRecore,ZoneID) VALUES (" + nRoleID + ",'" + RoleName + "'," + RecoryType + ",now()," + ValueRecore + "," + GameDBManager.ZoneID + ")";

                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 新手场景logout 物品处理
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleGoodsForFlashPlayerWhenLogOut(DBManager dbMgr, int nRoleID)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_goods WHERE rid = {0}", nRoleID);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 新手场景logout 任务处理
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleTasksForFlashPlayerWhenLogOut(DBManager dbMgr, int nRoleID)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                // 1. t_tasks
                string cmdText = string.Format("DELETE FROM t_tasks WHERE rid = {0}", nRoleID);
                conn.ExecuteNonQuery(cmdText);

                // 2. t_taskslog
                cmdText = string.Format("DELETE FROM t_taskslog WHERE rid = {0}", nRoleID);
                conn.ExecuteNonQuery(cmdText);

                bRet = true;
            }

            return bRet;
        }

        #endregion 新手场景

        #region 任务刷星

        /// <summary>
        /// 任务星级更新
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleTasksStarLevel(DBManager dbMgr, int nRoleID, int taskid, int StarLevel)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_tasks SET starlevel={2} WHERE rid = {0} AND Id = {1}", nRoleID, taskid, StarLevel);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        #endregion 任务刷星

        #region 崇拜

        /// <summary>
        /// 崇拜信息更新
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleAdmiredInfo1(DBManager dbMgr, int nRoleID, int nCount)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET admiredcount={1} WHERE rid={0}", nRoleID, nCount);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 崇拜信息更新--崇拜者数据t_adorationinfo表
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleAdmiredInfo2(DBManager dbMgr, int nRoleID1, int nRoleID2, int nDate)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_adorationinfo(roleid, adorationroleid, dayid) VALUES({0}, {1}, {2})", nRoleID1, nRoleID2, nDate);
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        #endregion 崇拜

        #region Cấp độ

        /// <summary>
        /// Thiết lập cấp độ cho nhân vật
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleLevel(DBManager dbMgr, int roleID, int level)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET level={0} WHERE rid={1}", level, roleID);

                Console.WriteLine("EXECUTE :::" + cmdText);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion Cấp độ

        #region 日常活动

        /// <summary>
        /// 更新日常活动数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateRoleDayActivityPoint(DBManager dbMgr, int nRoleID, int nDate, int nType, int nCount, long nValue)
        {
            bool bRet = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_dayactivityinfo(roleid, activityid, timeinfo, triggercount, totalpoint, lastgettime) VALUES({0}, {1}, {2}, {3}, {4}, '{5}')", nRoleID, nType, nDate, nCount, nValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                bRet = conn.ExecuteNonQueryBool(cmdText);
            }

            return bRet;
        }

        /// <summary>
        /// 删除玩家日常活动数据
        /// </summary>
        public static bool DeleteRoleDayActivityInfo(DBManager dbMgr, int roleID, int activityid)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_dayactivityinfo WHERE roleid = {0} AND activityid = {1}", roleID, activityid);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 日常活动

        #region 消息推送相关

        /// <summary>
        /// 更新推送ID
        /// </summary>
        public static int SetUserPushMessageID(DBManager dbMgr, string strUser, string strPushMegID)
        {
            int ret = 0;
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_pushmessageinfo(userid, pushid, lastlogintime) VALUE('{0}', '{1}', '{2}')", strUser, strPushMegID, today);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 消息推送相关

        #region 翅膀相关

        /// <summary>
        /// 添加一个新的翅膀
        /// </summary>
        public static int NewWing(DBManager dbMgr, int roleID, int wingID, int forgeLevel, string addtime, string strRoleName, int nOccupation)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                bool error = false;
                string cmdText = string.Format("INSERT INTO t_wings (rid, rname, occupation, wingid, forgeLevel, addtime, isdel, failednum, equiped, zhulingnum, zhuhunnum) VALUES({0}, '{1}', {2}, {3}, {4}, '{5}', {6}, {7}, {8}, {9}, {10})",
                    roleID, strRoleName, nOccupation, wingID, forgeLevel, addtime, 0, 0, 0, 0, 0);
                error = !conn.ExecuteNonQueryBool(cmdText);
                try
                {
                    if (!error)
                    {
                        ret = conn.GetSingleInt("SELECT LAST_INSERT_ID() AS LastID");
                    }
                }
                catch (MySQLException)
                {
                    ret = -2;
                }
            }

            return ret;
        }

        /// <summary>
        /// 修改翅膀
        /// </summary>
        public static int UpdateWing(DBManager dbMgr, int id, string[] fields, int startIndex)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = FormatUpdateSQL(id, fields, startIndex, _UpdateWing_fieldNames, "t_wings", _UpdateWing_fieldTypes);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新翎羽
        /// </summary>
        public static int UpdateLingYu(DBManager dbMgr, int roleID, int type, int level, int suit)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_lingyu(roleid, type, level, suit) VALUES({0}, {1}, {2}, {3})", roleID, type, level, suit);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 翅膀相关

        #region 图鉴系统

        /// <summary>
        /// 更新图鉴提交信息
        /// </summary>
        public static int UpdateRoleReferPictureJudgeInfo(DBManager dbMgr, int roleid, int nPictureJudgeID, int nNum)
        {
            int ret = 0;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_picturejudgeinfo(roleid, picturejudgeid, refercount) VALUES({0}, {1}, {2})", roleid, nPictureJudgeID, nNum);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 图鉴系统

        #region 魔晶兑换相关

        /// <summary>
        /// 更新绑定钻石兑换信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateMoJingExchangeDict(DBManager dbMgr, int nRoleid, int nExchangeid, int nDayID, int nNum)
        {
            int ret = 0;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_mojingexchangeinfo(roleid, exchangeid, exchangenum, dayid) VALUES({0}, {1}, {2}, {3})", nRoleid, nExchangeid, nNum, nDayID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 魔晶兑换相关

        #region 资源找回

        /// <summary>
        /// 更新资源找回数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static int UpdateResourceGetInfo(DBManager dbMgr, int nRoleid, int type, OldResourceInfo info)
        {
            int ret = 0;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                if (info == null)
                {
                    cmdText = string.Format("REPLACE INTO t_resourcegetinfo(roleid, type, exp, leftCount,mojing,bandmoney,zhangong,chengjiu,shengwang,bangzuan,xinghun,hasget,yuansufenmo) VALUES({0}, {1}, {2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})",
                    nRoleid, type, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
                }
                else
                {
                    cmdText = string.Format("REPLACE INTO t_resourcegetinfo(roleid, type, exp, leftCount,mojing,bandmoney,zhangong,chengjiu,shengwang,bangzuan,xinghun,hasget,yuansufenmo) VALUES({0}, {1}, {2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})",
                    nRoleid, info.type, info.exp, info.leftCount, info.mojing, info.bandmoney, info.zhangong, info.chengjiu, info.shengwang, info.bandDiamond, info.xinghun, 0, info.yuanSuFenMo);
                }
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 资源找回

        public static void AppendSQLForGoodsProps(StringBuilder sb, int index, object value, string[] fieldNames, byte[] fieldTypes, bool hasAppend)
        {
            if (hasAppend)
            {
                sb.Append(", ");
            }
            if (0 == fieldTypes[index])
            {
                sb.AppendFormat("{0}={1}", fieldNames[index], value);
            }
            else if (1 == fieldTypes[index])
            {
                sb.AppendFormat("{0}='{1}'", fieldNames[index], value);
            }
            else if (2 == fieldTypes[index])
            {
                sb.AppendFormat("{0}={0}+{1}", fieldNames[index], value);
            }
            else if (3 == fieldTypes[index])
            {
                sb.AppendFormat("{0}='{1}'", fieldNames[index], value.ToString().Replace('$', ':'));
            }
        }

        #region 星座系统

        /// <summary>
        /// 更新星座信息
        /// </summary>
        public static int UpdateRoleStarConstellationInfo(DBManager dbMgr, int roleid, int nStarSite, int nStarSlot)
        {
            int ret = 0;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_starconstellationinfo(roleid, starsiteid, starslotid) VALUES({0}, {1}, {2})", roleid, nStarSite, nStarSlot);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 星座系统

        #region 保存钻石消费记录

        public static int SaveConsumeLog(DBManager dbMgr, int roleid, string cdate, int ctype, int amount)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_consumelog (rid, amount, cdate, ctype) VALUES({0}, {1}, '{2}', {3})",
                    roleid, amount, cdate, ctype);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 保存钻石消费记录

        #region MU VIP相关

        /// <summary>
        /// 更新VIP等级奖励标记信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateVipLevelAwardFlagInfo(DBManager dbMgr, string strUserid, int nFlag, int nZoneID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET vipawardflag={0} WHERE userid='{1}' and zoneid = {2}", nFlag, strUserid, nZoneID); // 加区id判断 因合服可能产生相同的帐号vipawardflag字段不同 导致读取错误 [XSea 2015/7/8]
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 根据roleid更新VIP等级奖励标记信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateVipLevelAwardFlagInfoByRoleID(DBManager dbMgr, int nRoleid, int nFlag, int nZoneID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET vipawardflag={0} WHERE rid={1} and zoneid = {2}", nFlag, nRoleid, nZoneID); // 加区id判断 因合服可能产生相同的帐号vipawardflag字段不同 导致读取错误 [XSea 2015/7/8]
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion MU VIP相关

        #region 准备数据库表

        /// <summary>
        /// 执行一个sql语句，并返回读取的整数结果
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="sqlText"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int ExecuteSQLReadInt(DBManager dbMgr, string sqlText, MySQLConnection conn = null)
        {
            int result = 0;
            bool keepConn = true;

            MySQLCommand cmd = null;
            try
            {
                if (conn == null)
                {
                    keepConn = false;
                    conn = dbMgr.DBConns.PopDBConnection();
                }

                using (cmd = new MySQLCommand(sqlText, conn))
                {
                    try
                    {
                        MySQLDataReader reader = cmd.ExecuteReaderEx();
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader[0].ToString());
                        }
                        reader.Close();
                    }
                    catch (System.Exception ex)
                    {
                        LogManager.WriteException(ex.ToString());
                        return -1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogManager.WriteException(ex.ToString());
                return -2;
            }
            finally
            {
                if (!keepConn && null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return result;
        }

        /// <summary>
        /// 清理物品表的垃圾数据，转移到t_goods_bak
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int ValidateDatabase(DBManager dbMgr, string dbName)
        {
            MySQLConnection conn = null;
            string sqlText;
            int result;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                #region 验证角色表ID参数设置

                int flag_t_roles_auto_increment = GameDBManager.GameConfigMgr.GetGameConfigItemInt("flag_t_roles_auto_increment", 0);

                if (flag_t_roles_auto_increment < 200000 || (flag_t_roles_auto_increment % 100000) != 0)
                {
                    Global.LogAndExitProcess("Not yet set flag_t_roles_auto_increment");
                }

                int t_roles_auto_increment = 0;
                string[] ips = Global.GetLocalAddressIPs().Split('_');
                foreach (var ip in ips)
                {
                    int idx = ip.IndexOf('.');
                    if (idx > 0)
                    {
                        idx = ip.IndexOf('.', idx + 1);
                        if (idx > 0)
                        {
                            string ipPrefix = ip.Substring(0, idx);
                            if (GameDBManager.IPRange2AutoIncreaseStepDict.TryGetValue(ipPrefix, out t_roles_auto_increment))
                            {
                                break;
                            }
                        }
                    }
                }

                if (t_roles_auto_increment > 0)
                {
                    if (t_roles_auto_increment != flag_t_roles_auto_increment)
                    {
                        Global.LogAndExitProcess("flag_t_roles_auto_increment invalid format");
                    }
                }
                else if (t_roles_auto_increment != 0 && 200000 != flag_t_roles_auto_increment)
                {
                    Global.LogAndExitProcess("flag_t_roles_auto_increment invalid format");
                }

                GameDBManager.DBAutoIncreaseStepValue = flag_t_roles_auto_increment;

                #endregion 验证角色表ID参数设置

                foreach (var item in ValidateDatabaseTables)
                {
                    sqlText = string.Format("SELECT COUNT(*) FROM information_schema.columns WHERE table_schema='{0}' AND table_name = '{1}' AND column_name='{2}' limit 1;", dbName, item[0], item[1]);
                    result = ExecuteSQLReadInt(dbMgr, sqlText, conn);
                    if (result <= 0)
                    {
                        Global.LogAndExitProcess(string.Format("Table'{0}' does not exist or has missing columns: '{1}'", item[0], item[1]));
                    }
                }

                SwitchGoodsBackupTable(dbMgr);

                DBWriter.ClearBigTable_NameCheck(dbMgr);
            }
            catch (MySQLException ex)
            {
                LogManager.WriteException(string.Format("检查数据库是否正确时发生异常: {0}", ex.ToString()));
                throw new Exception(string.Format("检查数据库是否正确时发生异常: {0}", ex.ToString()));
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return 1;
        }

        #endregion 准备数据库表

        #region 转移和清理清理已标记删除的数据

        /// <summary>
        /// 清理物品表的垃圾数据，转移到t_goods_bak
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <returns></returns>
        public static int ClearUnusedGoodsData(DBManager dbMgr, bool clearAll = false)
        {
            int maxGoodsDBID = -1;
            int minGoodsDBID = -1;
            MySQLConnection conn = null;
            try
            {
                int toClearDBID = 0;
                string cmdText;
                conn = dbMgr.DBConns.PopDBConnection();
                MySQLCommand cmd = null;
                try
                {
                    using (cmd = new MySQLCommand("SELECT MAX(id) FROM t_goods", conn))
                    {
                        try
                        {
                            MySQLDataReader reader = cmd.ExecuteReaderEx();
                            if (reader.Read())
                            {
                                maxGoodsDBID = Convert.ToInt32(reader[0].ToString());
                            }
                            reader.Close();
                        }
                        catch
                        {
                            return -1;
                        }
                    }
                    using (cmd = new MySQLCommand("SELECT MIN(id) FROM t_goods", conn))
                    {
                        try
                        {
                            MySQLDataReader reader = cmd.ExecuteReaderEx();
                            if (reader.Read())
                            {
                                minGoodsDBID = Convert.ToInt32(reader[0].ToString());
                            }
                            reader.Close();
                        }
                        catch
                        {
                            return -1;
                        }
                    }
                }
                catch (MySQLException ex)
                {
                    LogManager.WriteException(string.Format("清理t_goods表时异常: {0}", ex.ToString()));
                    return -1;
                }

                try
                {
                    int last_goods_dbid = GameDBManager.GameConfigMgr.GetGameConfigItemInt("last_clear_goods_dbid", 0);
                    if (last_goods_dbid > minGoodsDBID)
                    {
                        //最小值设置有效的最小值
                        minGoodsDBID = last_goods_dbid;
                    }

                    int max_clear_goods_count = GameDBManager.GameConfigMgr.GetGameConfigItemInt("max_clear_goods_count", 1);
                    toClearDBID = minGoodsDBID + max_clear_goods_count;
                    if (maxGoodsDBID < toClearDBID)
                    {
                        //数量太少，不值得清理
                        return 0;
                    }

                    if (clearAll)
                    {
                        //清理所有的
                        toClearDBID = maxGoodsDBID;
                    }

                    //更新清理到的goodsDBID参数
                    GameDBManager.GameConfigMgr.UpdateGameConfigItem("last_goods_dbid", toClearDBID.ToString());
                    DBWriter.UpdateGameConfig(dbMgr, "last_goods_dbid", toClearDBID.ToString());

                    cmdText = string.Format("INSERT INTO t_goods_bak SELECT *,0,NOW(),0 FROM t_goods WHERE gcount <= 0 AND id > {0} AND id <= {1}", minGoodsDBID, toClearDBID);
                    using (cmd = new MySQLCommand(cmdText, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    cmdText = string.Format("DELETE FROM t_goods WHERE gcount <= 0 AND id > {0} AND id <= {1}", minGoodsDBID, toClearDBID);
                    using (cmd = new MySQLCommand(cmdText, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySQLException ex)
                {
                    LogManager.WriteException(string.Format("清理t_goods表时异常: {0}", ex.ToString()));
                    return -1;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteException(string.Format("清理t_goods表时异常: {0}", ex.ToString()));
                return -1;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return 1;
        }

        /// <summary>
        /// 插入天梯战报日志
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ClearBigTable_NameCheck(DBManager dbMgr)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_name_check;");
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 转移和清理清理已标记删除的数据

        #region 消费日志相关

        /// <summary>
        /// 插入万魔塔场数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int insertItemLog(DBManager dbMgr, String[] fields)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                String strTableName = "t_usemoney_log";
                string cmdText = string.Format("INSERT INTO {0} (DBId, userid, ObjName, optFrom, currEnvName, tarEnvName, optType, optTime, optAmount, zoneID, optSurplus) VALUES({1}, '{9}', '{2}', '{3}', '{4}', '{5}', '{6}', now(), {7}, {8}, {10})",
                                       strTableName, fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9]);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新跨服幻影寺院的角色活动次数相关信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int UpdateRoleKuaFuDayLog(DBManager dbMgr, RoleKuaFuDayLogData data)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_kf_day_role_log (gametype,day,rid,zoneid,signup_count, start_game_count, success_count, faild_count) " +
                                                "VALUES({0},'{1}',{2},{3},{4},{5},{6},{7}) " +
                                                "on duplicate key update zoneid={3},signup_count=signup_count+{4},start_game_count=start_game_count+{5},success_count=success_count+{6},faild_count=faild_count+{7}",
                                                data.GameType, data.Day, data.RoleID, data.ZoneId, data.SignupCount, data.StartGameCount, data.SuccessCount, data.FaildCount);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 消费日志相关



        public static bool UpdateUsrSecondPassword(DBManager dbMgr, string usrid, string secPwd)
        {
            if (string.IsNullOrEmpty(usrid))
            {
                return false;
            }

            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = "";
                if (!string.IsNullOrEmpty(secPwd))
                {
                    cmdText = string.Format("REPLACE INTO t_secondpassword(userid, secpwd) VALUES('{0}','{1}')", usrid, secPwd);
                }
                else
                {
                    cmdText = string.Format("DELETE FROM t_secondpassword WHERE userid='{0}'", usrid);
                }
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #region 结婚数据更新

        /// <summary>
        /// 更新结婚数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateMarriageData(DBManager dbMgr, int nRoleID, int nSpouseID, sbyte byMarrytype, int nRingID, int nGoodwillexp, sbyte byGoodwillStar, sbyte byGoodwilllevel, int nGivenrose, string strLovemessage, sbyte byAutoReject, string changtime)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_marry(roleid, spouseid, marrytype, ringid, goodwillexp, goodwillstar, goodwilllevel, givenrose, lovemessage, autoreject, changtime) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}', {9}, '{10}')",
                                                nRoleID, nSpouseID, byMarrytype, nRingID, nGoodwillexp, byGoodwillStar, byGoodwilllevel, nGivenrose, strLovemessage, byAutoReject, changtime);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 结婚数据更新

        #region 婚宴数据

        public static bool AddMarryParty(DBManager dbMgr, int roleID, int partyType, string startTime, int husbandRoleID, int wifeRoleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_marryparty (roleid, partytype, joinCount, startTime, husbandid, wifeid) VALUES({0}, {1}, {2}, '{3}', {4}, {5})",
                    roleID, partyType, 0, startTime, husbandRoleID, wifeRoleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        public static bool RemoveMarryParty(DBManager dbMgr, int roleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("DELETE FROM t_marryparty WHERE roleid={0}", roleID);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        /// <summary>
        /// 更新婚宴參予人数
        /// </summary>
        public static bool IncMarryPartyJoin(DBManager dbMgr, int roleID, int joinerID, int joinCount)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_marryparty SET joincount=joincount+1 WHERE roleid={0}", roleID);
                if (conn.ExecuteNonQueryBool(cmdText))
                    ret = true;

                // 更新玩家參予次数
                cmdText = string.Format("REPLACE INTO t_marryparty_join(roleid, partyroleid, joincount) VALUES({0}, {1}, {2})", joinerID, roleID, joinCount);
                if (conn.ExecuteNonQueryBool(cmdText))
                    ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 清空婚宴參予人数
        /// </summary>
        public static bool ClearMarryPartyJoin(DBManager dbMgr, int roleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText;
                if (roleID <= 0)
                {
                    cmdText = string.Format("delete from t_marryparty_join");
                }
                else
                {
                    cmdText = string.Format("delete from t_marryparty_join WHERE roleid = {0}", roleID);
                }
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 婚宴数据

        #region 群邮件相关

        public static int ModifyGMailRecord(DBManager dbMgr, int roleID, int gmailID, int mailID)
        {
            int ret = -1;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                String strTableName = "t_rolegmail_record";
                string cmdText = string.Format("REPLACE INTO {0} (roleid, gmailid, mailid) VALUES({1}, {2}, {3})",
                                                strTableName, roleID, gmailID, mailID);
                ret = conn.ExecuteNonQuery(cmdText);
            }

            return ret;
        }

        #endregion 群邮件相关

        #region 圣物数据更新

        /// <summary>
        /// 更新结婚数据
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateHolyItemData(DBManager dbMgr, int nRoleID, sbyte sShengwu_type, sbyte sPart_slot, sbyte sPart_suit, int nPart_slice, int nFail_count)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_holyitem(roleid, shengwu_type, part_slot, part_suit, part_slice, fail_count) VALUES({0}, {1}, {2}, {3}, {4}, {5})",
                                                nRoleID, sShengwu_type, sPart_slot, sPart_suit, nPart_slice, nFail_count);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 圣物数据更新

        #region 塔罗牌数据更新

        /// <summary>
        /// 塔罗牌数据更新
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateTarotData(DBManager dbMgr, int nRoleID, string tarotInfo, string kingInfo)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("REPLACE INTO t_tarot(roleid, tarotinfo, kingbuff) VALUES({0}, '{1}','{2}')", nRoleID, tarotInfo, kingInfo);

                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 塔罗牌数据更新

        #region ten发奖

        /// <summary>
        /// 更新发奖状态
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="roleName"></param>
        public static bool UpdateTenState(DBManager dbMgr, int id, int state)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_ten SET state={0} WHERE id={1}", state, id);
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion ten发奖

        #region 账号绑定

        public static bool ActivateStateSet(DBManager dbMgr, int zoneID, string userID, int roleID)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("INSERT INTO t_activate (userID, zoneID, roleID, logTime) VALUES('{0}', {1}, {2}, '{3}')",
                    userID, zoneID, roleID, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }

        #endregion 账号绑定

        #region GiftCode发奖

        /// <summary>
        /// 更新GiftCode发奖状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mailid"></param>
        public static bool UpdateGiftCodeState(DBManager dbMgr, int id, int mailid, string time)
        {
            bool ret = false;
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_giftcode SET mailid={0},usetime='{1}' WHERE id={2}", mailid, time, id);

                ret = conn.ExecuteNonQueryBool(cmdText);
            }
            return ret;
        }

        #endregion GiftCode发奖
    }
}