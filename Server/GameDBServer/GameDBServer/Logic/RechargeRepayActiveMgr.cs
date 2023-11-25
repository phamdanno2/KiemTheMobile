using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using Server.Data;
using Server.TCP;
using Server.Protocol;
using Server.Tools;
using System.Net;
using System.Net.Sockets;
using GameDBServer.DB;
using System.Text.RegularExpressions;
using GameDBServer.Server;
using GameDBServer.Logic.Rank;

namespace GameDBServer.Logic
{
    class RechargeRepayActiveMgr
    {
        /// <summary>
        /// 解析指令
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        static bool GetCmdDataField( int nID, byte[] data, int count, out string[] fields)
        {
            string cmdData = null;
            fields = null;
            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}", (TCPGameServerCmds)nID));

               // tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return false;
            }

            //解析用户名称和用户密码
            fields = cmdData.Split(':');
            return true;
        }

        /// <summary>
        /// 查询回馈活动信息，每日充值，累计充值，累计消费  合服充值返利
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessQueryActiveInfo(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;
            string strcmd;

            try
            {
                if (!GetCmdDataField( nID, data, count, out fields))
                {
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int activeid = Global.SafeConvertToInt32(fields[1]);
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);

                if (null == roleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("发起请求的角色不存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                long hasgettimes = 0;
                string lastgettime = "";
                string huoDongKeyStr = "not_limit";
                string extData = "";
                switch (activeid)
                {
                    case (int)(ActivityTypes.MeiRiChongZhiHaoLi):
                        {
                            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                            huoDongKeyStr = Global.GetHuoDongKeyString(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            int money =  DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            extData = "" + money;
                        }
                        break;
                    case (int)(ActivityTypes.TotalCharge):
                        int realmoney=0;
                        int usermoney =0;
                        DBQuery.QueryUserMoneyByUserID(dbMgr, roleInfo.UserID, out usermoney, out realmoney);
                        realmoney = Global.TransMoneyToYuanBao(realmoney);
                        extData = "" + realmoney;
                        break;
                    case (int)(ActivityTypes.TotalConsume):
                        {
                            // 这个活动没有结束时间，所以不能使用缓存系统
                            string startTime = GameDBManager.GameConfigMgr.GetGameConifgItem("kaifutime");
                            string endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            //RankDataKey key = new RankDataKey(RankType.Consume, startTime, endtime);
                            //int consumeValue = roleInfo.RankValue.GetRankValue(key);

                          //  extData = DBQuery.GetUserUsedMoney(dbMgr, roleID, startTime, endtime).ToString();

                            // Tạm thời lấy ra toàn bộ thời gian
                            extData = DBQuery.GetUserUsedMoney(dbMgr, roleID).ToString();
                        }
                        break;
                    case (int)(ActivityTypes.HeFuRecharge):
                        {
                            // 检查参数个数
                            if (5 != fields.Length)
                            {
                                return TCPProcessCmdResults.RESULT_DATA;
                            }

                            int hefutime = Global.SafeConvertToInt32(fields[2]);
                            int hefuEndtime = Global.SafeConvertToInt32(fields[3]);
                            huoDongKeyStr = Global.GetHuoDongKeyString(hefutime.ToString(), hefuEndtime.ToString());

                            // 存放排名返还比例的数组
                            Dictionary<int, float> CoeDict = new Dictionary<int, float>();

                            string strconfig = fields[4];
                            string[] strattr = strconfig.Split('|');
                            for (int i = 0; i < strattr.Length; i++)
                            {
                                string[] strcoe = strattr[i].Split(',');
                                if (2 != strcoe.Length)
                                    continue;

                                int rankcfg = Global.SafeConvertToInt32(strcoe[0]);
                                float coe = (float)Convert.ToDouble(strcoe[1]);
                                CoeDict[rankcfg] = coe;
                            }

                            //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                            DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuRecharge), huoDongKeyStr, out hasgettimes, out lastgettime);

                            int lastgetday = 0;
                            if (!string.IsNullOrEmpty(lastgettime))
                                lastgetday = Global.GetOffsetDay(DateTime.Parse(lastgettime));

                            int userdayflag = 0;

                            // 如果最后一次的时间小于合服时间 直接用合服时间
                            if (lastgetday < hefutime)
                                userdayflag = hefutime;
                            // 如果领取过就用最后一次领取时的开始计算
                            else if (hasgettimes > 0)
                                userdayflag = lastgetday;
                            // 如过没领取过就用GameServer发来的合服时间开始计算
                            else
                                userdayflag = hefutime;

                            // 今天的日期
                            int currday = Global.GetOffsetDay(DateTime.Now);
                            // 昨天的日期
                            int overDay = currday - 1;
                            if (overDay > hefuEndtime)
                            {
                                overDay = hefuEndtime;
                            }

                            int userrebate = 0;
                            // 如果今天已经领取了
                            if (userdayflag == currday)
                            {
                                // 返回给GameServer告诉玩家没有可以领取的钻石了
                                userrebate = 0;
                            }
                            else
                            {
                                // 返回给GameServer
                                //extData += yesterday - userdayflag > 0 ? yesterday - userdayflag : 0;

                                // 个人能获得的返利数量
                                for (int i = userdayflag; i <= overDay; i++)
                                {
                                    DateTime now = Global.GetRealDate(i);
                                    string startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                                    string endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
                                    int userrank = GameDBManager.DayRechargeRankMgr.GetRoleRankByDay(dbMgr, roleInfo.UserID, i);

                                    // 查询玩家排名返还比例
                                    if (!CoeDict.ContainsKey(userrank))
                                    {
                                        // log it
                                        continue;
                                    }

                                    //int input = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime, endTime);
                                    RankDataKey key = new RankDataKey(RankType.Charge, startTime, endTime, null);
                                    int input = roleInfo.RankValue.GetRankValue(key);
                                    //重置排行值，将其转换为元宝数量
                                    //input = Global.TransMoneyToYuanBao(input);

                                    userrebate += (int)(input * CoeDict[userrank]);
                                }
                            }

                            extData += userrebate;
                            extData += ":";

                            // [JIRA] (MUBUG-333) 【充值返利】充值返利活动结束后第二天还可以累计返利金额
                            // 策划要求，合服充值返利界面显示今天排行帮
                            if (currday > hefuEndtime)
                            {
                                // 加上排行的个数
                                extData += "0";
                            }
                            else
                            {
                                List<InputKingPaiHangData> ranklist = GameDBManager.DayRechargeRankMgr.GetRankByDay(dbMgr, currday/*yesterday*/);
                                // 昨日排名
                                // {个数|排名1，区号1，姓名1|排名2，区号2，姓名2|排名3，区号3，姓名3|…… }

                                // 加上排行的个数
                                extData += ranklist.Count;
                                int rank = 1;
                                foreach (var item in ranklist)
                                {
                                    extData += "|";
                                    extData += rank;
                                    extData += ",";
                                    extData += item.MaxLevelRoleZoneID;
                                    extData += ",";
                                    extData += item.MaxLevelRoleName;
                                    ++rank;
                                }
                            }
                        }
                        break;
                }
                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动

                  DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, out hasgettimes, out lastgettime);



                    string getIndexstr = hasgettimes.ToString();

                    string temp = "";
                    if (hasgettimes != 0)
                    {
                        int i = 0;
                        foreach (char item in getIndexstr.ToCharArray())
                        {
                            temp += item;
                            i += 1;
                            if (i < getIndexstr.Length)
                            {
                                temp += ",";
                            }
                        }
                    }
                    if(string.IsNullOrEmpty(temp))
                        temp="1";
                    strcmd = string.Format("{0}:{1}:{2}", 1, temp, extData);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                }
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (System.Exception ex)
            {
                LogManager.WriteException(ex.ToString());
            }

            strcmd = string.Format("{0}:{1}:{2}", 0, "", "");
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }
        /// <summary>
        /// 保存领取奖励活动数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessGetActiveAwards(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            if (!GetCmdDataField(nID, data, count, out fields))
            {
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            int roleID = Convert.ToInt32(fields[0]);
            int activeid = Global.SafeConvertToInt32(fields[1]);
            int hasgettimes = Global.SafeConvertToInt32(fields[2]);
            long hasgettimesLong = Global.SafeConvertToInt64(fields[2]);
            DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);

            if (null == roleInfo)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("发起请求的角色不存在，CMD={0}, RoleID={1}",
                    (TCPGameServerCmds)nID, roleID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            string huoDongKeyStr = "not_limit";
            string extData = "";
            string strcmd = "";
            switch (activeid)
            {
                case (int)ActivityTypes.MeiRiChongZhiHaoLi:
                    {
                        DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        huoDongKeyStr = Global.GetHuoDongKeyString(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    break;
                case (int)(ActivityTypes.TotalCharge):
                case (int)(ActivityTypes.TotalConsume):
                    {
                        //避免同一角色同时多次操作
                        lock (roleInfo)
                        {
                            int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, hasgettimesLong, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            if (ret < 0)
                                ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, hasgettimesLong, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            if (ret < 0)
                            {
                                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                                return TCPProcessCmdResults.RESULT_FAILED;
                            }
                        }
                        strcmd = string.Format("{0}:{1}:{2}", 1, activeid, hasgettimesLong);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    break;
                case (int)(ActivityTypes.HeFuRecharge):
                    {
                        // 检查参数个数
                        if (5 != fields.Length)
                        {
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        int hefutime = Global.SafeConvertToInt32(fields[2]);
                        int hefuEndtime = Global.SafeConvertToInt32(fields[3]);
                        huoDongKeyStr = Global.GetHuoDongKeyString(hefutime.ToString(), hefuEndtime.ToString());

                        // 存放排名返还比例的数组
                        Dictionary<int, float> CoeDict = new Dictionary<int, float>();

                        string strconfig = fields[4];
                        string[] strattr = strconfig.Split('|');
                        for (int i = 0; i < strattr.Length; i++)
                        {
                            string[] strcoe = strattr[i].Split(',');
                            if (2 != strcoe.Length)
                                continue;

                            int rank = Global.SafeConvertToInt32(strcoe[0]);
                            float coe = (float)Convert.ToDouble(strcoe[1]);
                            CoeDict[rank] = coe;
                        }

                        int ifhastime = 0;
                        string lastgettime = "";
                        //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                        DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuRecharge), huoDongKeyStr, out ifhastime, out lastgettime);

                        int lastgetday = 0;
                        if (!string.IsNullOrEmpty(lastgettime))
                            lastgetday = Global.GetOffsetDay(DateTime.Parse(lastgettime));

                        // 计算玩家从那天开始领取
                        int userdayflag = 0;

                        // 如果最后一次的时间小于合服时间 直接用合服时间
                        if (lastgetday < hefutime)
                            userdayflag = hefutime;
                        // 如果领取过就用最后一次领取时的开始计算
                        else if (ifhastime > 0)
                            userdayflag = lastgetday;
                        // 如过没领取过就用GameServer发来的合服时间开始计算
                        else
                            userdayflag = hefutime;

                        // 如果今天已经领取了
                        int currday = Global.GetOffsetDay(DateTime.Now);
                        if (userdayflag == currday)
                        {
                            // 返回给GameServer告诉玩家没有可以领取的钻石了
                            strcmd = string.Format("{0}:{1}:{2}", 1, activeid, 0);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }

                        // 昨天的日期
                        int overDay = Global.GetOffsetDay(DateTime.Now) - 1;
                        if (overDay > hefuEndtime)
                        {
                            overDay = hefuEndtime;
                        }

                        int userrebate = 0;
                        // 从开始计算到昨天的个人能获得的返利数量
                        for (int i = userdayflag; i <= overDay; i++)
                        {
                            DateTime now = Global.GetRealDate(i);
                            string startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                            string endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
                            // 查询玩家某日充值排名
                            int rank = GameDBManager.DayRechargeRankMgr.GetRoleRankByDay(dbMgr, roleInfo.UserID, i);
                            // 查询玩家排名返还比例
                            if (!CoeDict.ContainsKey(rank))
                            {
                                // log it
                                continue;
                            }

                            // 查询玩家某日充值数量
                            //int input = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime, endTime);
                            RankDataKey key = new RankDataKey(RankType.Charge, startTime, endTime, null);
                            int input = roleInfo.RankValue.GetRankValue(key);

                            //input = Global.TransMoneyToYuanBao(input);
                            userrebate += (int)(input * CoeDict[rank]);
                        }

                        // 返回给GameServer给玩家加钻石
                        // 领取记录去真正给元宝的地方更新
                        strcmd = string.Format("{0}:{1}:{2}", 1, activeid, userrebate);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    break;
            }
            //避免同一角色同时多次操作
            lock (roleInfo)
            {
               int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
               if (ret < 0)
                   ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
               if (ret < 0)
               {
                   tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                   return TCPProcessCmdResults.RESULT_FAILED;
               }
            }
            strcmd = string.Format("{0}:{1}:{2}", 1, activeid, hasgettimes);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

    }
}
