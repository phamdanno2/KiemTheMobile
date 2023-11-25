using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
//using System.Windows.Resources;
using System.Windows;
//using System.Windows.Media.Animation;
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
    class NewZoneActiveMgr
    {

        public static NewZoneActiveData NewZoneFanli = null;
        /// <summary>
        /// 新区活动排行，王类排名
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="minGateValueList"></param>
        /// <param name="activityType"></param>
        /// <param name="midDate"></param>
        /// <param name="maxPaiHang"></param>
        /// <returns></returns>
        private static List<PaiHangItemData> GetActiveKingTypeRanklist(DBManager dbMgr, List<int> minGateValueList, int activityType, string midDate, int maxPaiHang = 10)
        {
            //返回排行信息【这个排行信息是真实的排行信息，没有处理排行值限制，活动奖励的时候，需要根据配置限制信息动态调整排行】
            List<HuoDongPaiHangData> listPaiHangReal = DBQuery.GetActivityPaiHangListNearMidTime(dbMgr, activityType, midDate, maxPaiHang);

            List<PaiHangItemData> listPaiHang = new List<PaiHangItemData>();

            //上一个玩家的排行 0 表示上个玩家没有排行
            int preUserPaiHang = 0;
            int preValueid = 0;
            bool bFirst = true;
            //重整排行表
            for (int n = 0; n < listPaiHangReal.Count; n++)
            {
                HuoDongPaiHangData phData = listPaiHangReal[n];
                phData.PaiHang = -1;

                //不满足该排行位置需要的最低数值要求，则依次降低排名
                for (int i = 0; i < minGateValueList.Count; i++)
                {
                    if (phData.PaiHangValue >= minGateValueList[i])
                    {
                       
                        PaiHangItemData item = new PaiHangItemData();
                        if (bFirst)
                        {
                            phData.PaiHang = i + 1;
                        }
                        else
                        {
                            if (i == preValueid)
                            {
                                phData.PaiHang = preUserPaiHang + 1;
                            }
                            else
                            {
                                if (i + 1 > preUserPaiHang)
                                    phData.PaiHang = i + 1;
                                else
                                    phData.PaiHang = preUserPaiHang + 1;
                            }
                            
                        }
                        
                        item.RoleID = phData.RoleID;
                        item.RoleName = phData.RoleName;
                        item.Val2 = phData.PaiHang;
                        item.Val1 = phData.PaiHangValue;
                        listPaiHang.Add(item);
                        preValueid = i;
                        preUserPaiHang = phData.PaiHang;
                        bFirst = false;
                        break;
                    }
                }

                //这个靠前的排行数据没有满足排行值限制的最小要求，则其后的排行数据也满足不了
                if (phData.PaiHang < 0 || phData.PaiHang >= minGateValueList.Count)
                {
                    break;
                }
            }

            return listPaiHang;
        }

        /// <summary>
        /// 返回新区活动排行数据列表，不包括冲级狂人
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="minGateValueList"></param>
        /// <param name="maxPaiHang"></param>
        /// <returns></returns>
        private static List<PaiHangItemData> GetRankListByActiveLimit(DBManager dbMgr, string fromDate, string toDate, List<int> minGateValueList,int activeId, int maxPaiHang = 3)
        {
            //返回排行信息
            List<InputKingPaiHangData> listPaiHangReal = new List<InputKingPaiHangData>();
            List<PaiHangItemData> ranklist = new List<PaiHangItemData>();
            switch (activeId)
            {
                case (int)ActivityTypes.NewZoneUpLevelMadman:
                    break;
                case (int)ActivityTypes.NewZoneBosskillKing:
                    string paiHangDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //如果今天活动结束，就取活动结束时间作为排行读取时间点
                    if (!Global.IsInActivityPeriod(fromDate, toDate))
                    {
                        paiHangDate = toDate;
                    }
                    return GetActiveKingTypeRanklist(dbMgr, minGateValueList, activeId, paiHangDate, maxPaiHang);
                   // break;
                case (int)ActivityTypes.NewZoneConsumeKing:
                    //listPaiHangReal = DBQuery.GetUserUsedMoneyPaiHang(dbMgr, fromDate, toDate, maxPaiHang);

                    listPaiHangReal = Global.GetUsedMoneyKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, null, maxPaiHang);
                    break;
                case (int)ActivityTypes.NewZoneFanli:
                    {
                        DateTime now = DateTime.Now;
                        DateTime huodongStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
                        DateTime.TryParse(fromDate, out huodongStartTime);
                        //int roleYuanBaoInPeriod = 0;
                        //if (now.Ticks <= (huodongStartTime.Ticks + (10000L * 1000L * 24L * 60L * 60L)))
                        //{

                        //    return ranklist;
                        //}

                        /// 获取一个增加了-1天时间的DateTime
                        DateTime sub1DayDateTime = Global.GetAddDaysDataTime(now, -1, true);

                        string startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                        string endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
                        //listPaiHangReal = DBQuery.GetUserInputPaiHang(dbMgr, startTime, endTime, maxPaiHang);

                        listPaiHangReal = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, null, maxPaiHang);
                    }
                    
                    break;
                case (int)ActivityTypes.NewZoneRechargeKing:
                    //listPaiHangReal = DBQuery.GetUserInputPaiHang(dbMgr, fromDate, toDate, maxPaiHang);

                    listPaiHangReal = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, null, maxPaiHang);
                    break;
            }

          //  List<InputKingPaiHangData> listPaiHang = new List<InputKingPaiHangData>();
           
            //上一个玩家的排行 0 表示上个玩家没有排行
            int preUserPaiHang = 0;
            int preValueid=0;
            string uid = "";
            bool bFirst = true;
            //重整排行表
            for (int n = 0; n < listPaiHangReal.Count; n++)
            {
                InputKingPaiHangData phData = listPaiHangReal[n];
                phData.PaiHang = -1;
                if (activeId != (int)ActivityTypes.NewZoneConsumeKing)
                    Global.GetUserMaxLevelRole(dbMgr, phData.UserID, out phData.MaxLevelRoleName, out phData.MaxLevelRoleZoneID);
                else
                    Global.GetRoleNameAndUserID(dbMgr, Global.SafeConvertToInt32(phData.UserID), out phData.MaxLevelRoleName, out uid);
               
                //不满足该排行位置需要的最低数值要求，则依次降低排名
                for (int i = 0; i < minGateValueList.Count; i++)
                {
                    int values = phData.PaiHangValue;//Global.TransMoneyToYuanBao(phData.PaiHangValue);
                    if (activeId == (int)ActivityTypes.NewZoneConsumeKing)
                    {
                        values = phData.PaiHangValue;
                    }
                    if (values >= minGateValueList[i])
                    {
                        if (bFirst)
                            phData.PaiHang = i + 1;
                        else
                        {
                            if (i == preValueid)
                            {
                                phData.PaiHang = preUserPaiHang + 1;
                            }
                            else
                            {
                                if(i+1>preUserPaiHang)
                                    phData.PaiHang = i + 1;
                                else
                                    phData.PaiHang = preUserPaiHang + 1;
                            }
                               
                        }
                        PaiHangItemData item = new PaiHangItemData();
                        item.Val1 = values;
                        if (activeId == (int)ActivityTypes.NewZoneConsumeKing)
                        {
                            item.RoleID = Convert.ToInt32(phData.UserID);
                            
                        }
                        item.RoleName = phData.MaxLevelRoleName;
                        item.Val2 = phData.PaiHang;
                        item.uid = phData.UserID;
                        ranklist.Add(item);
                        preValueid=i;
                        preUserPaiHang = phData.PaiHang;
                        bFirst = false;
                        break;
                    }
                }

                //这个靠前的排行数据没有满足排行值限制的最小要求，则其后的排行数据也满足不了
                if (phData.PaiHang < 0 || phData.PaiHang >= minGateValueList.Count)
                {
                    break;
                }
            }

            //计算充值返利
            if(activeId==(int)ActivityTypes.NewZoneFanli)
            {
                for (int i=0;i<ranklist.Count;i++)
                {
                    int rank = i + minGateValueList.Count - ranklist.Count;
                    ranklist[i].Val1 = ranklist[i].Val1 * minGateValueList[rank]/100;
                }
            }
            
            return ranklist;
        }

        /// <summary>
        /// 新服活动查询，除了冲级狂人
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
            string cmdData = null;
           
            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {

                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                int activeid = Global.SafeConvertToInt32(fields[4]);
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                
                if (null == roleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("发起请求的角色不存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                //返回排行信息,返回消费王排行数据列表,可能没有第一名等名次
                List<PaiHangItemData> listPaiHang = NewZoneActiveMgr.GetRankListByActiveLimit(dbMgr, fromDate, toDate, minGateValueList, activeid, minGateValueList.Count);

                //更新最大等级角色名称 和区号
                //foreach (var item in listPaiHang)
                //{
                //    string id = "";
                //    Global.GetRoleNameAndUserID(dbMgr, item.RoleID, out item.RoleName, out id);
                //}
                //活动时间范围内的充值数，真实货币单位
                int inputMoneyInPeriod = 0;
                
              
                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                switch ((ActivityTypes)activeid)
                {
                    case ActivityTypes.NewZoneConsumeKing:
                       // huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);
                        //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                        DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneConsumeKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                        //这个活动每次每个用户最多领取一次
                        if (hasgettimes > 0)
                            hasgettimes = 1;
                        break;
                    case ActivityTypes.NewZoneRechargeKing:
                        {
                           // huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);
                            //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                            DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneRechargeKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                            //这个活动每次每个用户最多领取一次
                            if (hasgettimes > 0)
                                hasgettimes = 1;
                        }
                        break;
                    case ActivityTypes.NewZoneBosskillKing:
                        {
                            // huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);
                            //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                            DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, activeid, huoDongKeyStr, out hasgettimes, out lastgettime);

                            //这个活动每次每个角色最多领取一次
                            if (hasgettimes > 0)
                                hasgettimes = 1;
                        }
                        break;
                    case ActivityTypes.NewZoneFanli:
                        {
                            inputMoneyInPeriod = NewZoneActiveMgr.ComputTotalFanliValue(dbMgr, roleInfo, activeid, fromDate, toDate, minGateValueList);
                            if (inputMoneyInPeriod == 0)
                            {
                                hasgettimes = 1;
                            }
                        }

                        break;
                }

                
                NewZoneActiveData consumedata = new NewZoneActiveData()
                {
                    YuanBao = inputMoneyInPeriod,
                    ActiveId = activeid,
                    Ranklist = listPaiHang,
                    GetState = hasgettimes
                };
                //生成排行信息的tcp对象流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<NewZoneActiveData>(consumedata, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
              
                DataHelper.WriteFormatExceptionLog(ex, "", false);
               
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取屠魔勇士奖励
        /// </summary>
        /// <param name="cmdData"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults GetBossKillAward(DBManager dbMgr, TCPOutPacketPool pool, int nID, int roleID, int activeid, string fromDate, string toDate, List<int> minGateValueList, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            
            try
            {
                
                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string paiHangDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //如果今天活动结束，就取活动结束时间作为排行读取时间点,活动介绍之前能否领取，由配置文件配置，gameserver在领取时会进行限制
                if (!Global.IsInActivityPeriod(fromDate, toDate))
                {
                    paiHangDate = toDate;
                }

                //返回排行信息【这个排行信息了处理排行值限制，活动奖励的时候，需要根据配置限制信息动态调整排行】
                List<HuoDongPaiHangData> listPaiHang = Global.GetPaiHangItemListByHuoDongLimit(dbMgr, minGateValueList, (int)ActivityTypes.NewZoneBosskillKing, paiHangDate);

                //排行值
                int paiHang = -1;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (null != listPaiHang[n] && roleInfo.RoleID == listPaiHang[n].RoleID)
                    {
                        paiHang = listPaiHang[n].PaiHang + minGateValueList.Count - listPaiHang.Count;//这个值和 n+1 是不一样的，因为listPaiHang中的数据可能有排行空缺
                        break;
                    }
                }

                //判断是否在排行内，不可能
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, activeid);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)ActivityTypes.NewZoneBosskillKing, huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, activeid);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)ActivityTypes.NewZoneBosskillKing, huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, activeid);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                //发送命令给gameserver 由gameserver 统一奖励
                strcmd = string.Format("{0}:{1}:{2}", 1, paiHang, activeid);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
           
            
        }

        /// <summary>
        /// 消费达人领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults GetConsumeKingAward(DBManager dbMgr, TCPOutPacketPool pool, int nID, int roleID, int activeid, string fromDate, string toDate, List<int> minGateValueList, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

           
            try
            {
               
                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //返回排行信息,返回消费王排行数据列表,可能没有第一名等名次
              //  List<InputKingPaiHangData> listPaiHang = Global.GetUsedMoneyKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList, minGateValueList.Count);
             List<PaiHangItemData> listPaiHang =   NewZoneActiveMgr.GetRankListByActiveLimit(dbMgr, fromDate, toDate, minGateValueList, activeid, minGateValueList.Count);
                //排行值
                int paiHang = -1;
                //活动时间范围的充值，真实货币单位
                int inputMoneyInPeriod = 0;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (roleInfo.RoleID == listPaiHang[n].RoleID) //这里返回的是角色的ID
                    {
                        paiHang = listPaiHang[n].Val2;//得到排行值
                        inputMoneyInPeriod = listPaiHang[n].Val1;
                    }
                }

                //判断是否在排行内
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内获取的元宝数
                int roleGetYuanBaoInPeriod = inputMoneyInPeriod;

                
                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneConsumeKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneConsumeKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, paiHang, activeid);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
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

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值达人领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults GetRechargeKingAward(DBManager dbMgr, TCPOutPacketPool pool, int nID, int roleID, int activeid, string fromDate,string toDate, List<int> minGateValueList, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            

            try
            {
              

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
              //  List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList);
                List<PaiHangItemData> listPaiHang = NewZoneActiveMgr.GetRankListByActiveLimit(dbMgr, fromDate, toDate, minGateValueList, activeid, minGateValueList.Count);
                //排行值
                int paiHang = -1;
                //活动时间范围的充值，真实货币单位
                int inputMoneyInPeriod = 0;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (roleInfo.UserID == listPaiHang[n].uid)
                    {
                        paiHang = listPaiHang[n].Val2;//得到排行值
                        inputMoneyInPeriod = listPaiHang[n].Val1;
                    }
                }

                //判断是否在排行内
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内获取的元宝数
                int roleGetYuanBaoInPeriod = inputMoneyInPeriod;//Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                //对排行进行修正之后，超出了奖励范围,即充值元宝数量没有达到最低的元宝数量要求
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneRechargeKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneRechargeKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, paiHang, activeid);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
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

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 新区充值返利领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults GetNewFanliAward(DBManager dbMgr, TCPOutPacketPool pool, int nID, int roleID, int activeid,string fromDate,string todate,  List<int> minGateValueList, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;
            string strcmd = "";
            
            try
            {
             

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:{2}", -1001, roleID,activeid);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

               int roleYuanBaoInPeriod = NewZoneActiveMgr.ComputTotalFanliValue(dbMgr, roleInfo, activeid, fromDate, todate, minGateValueList);

               
                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                   
                   
                    if (roleYuanBaoInPeriod > 0)
                    {
                        //活动时间取前一天的
                        DateTime sub1DayDateTime = Global.GetAddDaysDataTime(DateTime.Now, -1, true);

                        DateTime startTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 0, 0, 0);
                        DateTime endTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 23, 59, 59);
                        string huoDongKeyStr = Global.GetHuoDongKeyString(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        //更新已领取状态
                        int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.NewZoneFanli), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:{2}", -1008, roleID, activeid);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                   
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleYuanBaoInPeriod, activeid);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
               
                DataHelper.WriteFormatExceptionLog(ex, "", false);
               
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        

        /// <summary>
        /// 获取某个时间段内的返利，这里是返回某一天的返利
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleInfo"></param>
        /// <param name="activeid"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="minGateValueList"></param>
        /// <returns></returns>
       static int ComputNewFanLiValue(DBManager dbMgr, DBRoleInfo roleInfo, int activeid, string fromdate, string todate, List<int> minGateValueList)
        {
            int retvalue = 0;
           
            //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
            List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromdate, todate, minGateValueList, minGateValueList.Count);

            //查询时如果当前时间小于结束时间，就用采用结束时间也行
            //活动时间范围内的充值数，真实货币单位
            //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromdate, todate);
            RankDataKey key = new RankDataKey(RankType.Charge, fromdate, todate, null);
            int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);

            if (inputMoneyInPeriod < 0)
            {
                inputMoneyInPeriod = 0;
            }

            for (int i = 0; i < listPaiHang.Count; i++)
            {
                if (listPaiHang[i].UserID == roleInfo.UserID)
                {
                    inputMoneyInPeriod = inputMoneyInPeriod * minGateValueList[listPaiHang[i].PaiHang - 1] / 100;
                    //转换为元宝数
                    retvalue = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);
                    break;
                }
            }
            
            return retvalue;
        }
        
        

        /// <summary>
        /// 获取从活动开始到现在累计的返利
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleInfo"></param>
        /// <param name="activeid"></param>
        /// <param name="fromDate"></param>
        /// <param name="todate"></param>
        /// <param name="minGateValueList"></param>
        /// <returns></returns>
       static int ComputTotalFanliValue(DBManager dbMgr, DBRoleInfo roleInfo, int activeid, string fromDate, string todate, List<int> minGateValueList)
        {
            DateTime now = DateTime.Now;
            DateTime huodongStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime huodongEndTime = new DateTime();
            DateTime.TryParse(fromDate, out huodongStartTime);
            DateTime.TryParse(todate, out huodongEndTime);

            int retvalue = 0;
            //活动距离开始时间不足24小时
            if (now.Ticks <= (huodongStartTime.Ticks + (10000L * 1000L * 24L * 60L * 60L)))
            {
                return 0;
            }
            for (int i = 1; i <= 7; i++)
            {
                /// 获取一个增加了几天时间的DateTime
                DateTime sub1DayDateTime = Global.GetAddDaysDataTime(now, -i, true);

                DateTime startTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 0, 0, 0);
                DateTime endTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 23, 59, 59);

                string huoDongKeyStr = Global.GetHuoDongKeyString(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
               
                if (startTime < huodongStartTime)
                    break;
                //获取活动的历史记录
                int hasgettimes = 0;
                string lastgettime = "";
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, activeid, huoDongKeyStr, out hasgettimes, out lastgettime);
                if (hasgettimes > 0)
                    break;
                retvalue += ComputNewFanLiValue(dbMgr, roleInfo, activeid, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), minGateValueList);
            }
            return retvalue;
           
        }
        /// <summary>
        /// 新区获得奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessGetNewzoneActiveAward(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
             tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit, CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            TCPProcessCmdResults ret = TCPProcessCmdResults.RESULT_FAILED;
            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                int activetype =Global.SafeConvertToInt32( fields[4]);
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                
                switch (activetype)
                {
                    case (int)ActivityTypes.NewZoneBosskillKing:
                        {
                            ret = GetBossKillAward(dbMgr, pool, nID, roleID, activetype, fromDate,toDate, minGateValueList, out tcpOutPacket);
                            break;
                        }
                    case (int)ActivityTypes.NewZoneConsumeKing:
                        {
                            ret = GetConsumeKingAward(dbMgr, pool, nID, roleID, activetype, fromDate, toDate, minGateValueList, out tcpOutPacket); 
                            break;
                        }
                    case (int)ActivityTypes.NewZoneFanli:
                        {
                            ret = GetNewFanliAward(dbMgr, pool, nID, roleID, activetype, fromDate,toDate, minGateValueList, out tcpOutPacket);
                            break;
                        }
                    case (int)ActivityTypes.NewZoneRechargeKing:
                        {
                            ret = GetRechargeKingAward(dbMgr, pool, nID, roleID, activetype, fromDate, toDate, minGateValueList, out tcpOutPacket);
                            break;
                        }
                    case (int)ActivityTypes.NewZoneUpLevelMadman:
                        {
                            
                            break;
                        }
                   // tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)ret);
                   
                }

            }catch(Exception ex)
            {
            }
            return ret;
        }
       
    }
}
