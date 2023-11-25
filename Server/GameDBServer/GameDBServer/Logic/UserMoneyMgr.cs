using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDBServer.DB;
using Server.Tools;
using Server.Data;
using GameDBServer.Logic.Rank;
using GameDBServer.Server;
using Server.Protocol;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 用户重置管理类
    /// </summary>
    public class UserMoneyMgr
    {
        #region 获取用户充值，同步到角色表中

        /// <summary>
        /// 上次扫描更新用户重置的元宝数据的时间
        /// </summary>
        private static long LastUpdateUserMoneyTicks = 0;

        private static bool ChargeDataLogState = false;

        /// <summary>
        /// 更新用户充值的元宝数据
        /// </summary>
      

        private static void _ProcessBuyYueKa(DBManager dbMgr, DBUserInfo dbUserInfo)
        {
            int rid = DBQuery.LastLoginRole(dbMgr, dbUserInfo.UserID);
            string gmCmdData = string.Format("-buyyueka {0} {1}", dbUserInfo.UserID, rid);
            LogManager.WriteLog(LogTypes.Error, string.Format("Resolve buy month card，userid={0}, roleid={1}", dbUserInfo.UserID, rid));
            ChatMsgManager.AddGMCmdChatMsg(-1, gmCmdData);
        }

     

        #endregion 获取用户充值，同步到角色表中


        #region Truy vấn tổng số đồng hiện có trong Server

        /// <summary>
        /// Thời điểm truy vấn tổng số đông fhiện có trước trong Server
        /// </summary>
        private static DateTime LastLastQueryServerTotalUserMoneyTime = DateTime.MinValue;

        /// <summary>
        /// Truy vấn tổng số đồng hiện có trong Server
        /// </summary>
        public static void QueryTotalUserMoney()
        {
            if (GameDBManager.Flag_Query_Total_UserMoney_Minute < 5) 
                return;

            DateTime now = DateTime.Now;

            bool bInCheckTime = false;
            long elapsedMilliseconds = (now.Ticks - LastLastQueryServerTotalUserMoneyTime.Ticks) / TimeSpan.TicksPerMillisecond;
            if (elapsedMilliseconds >= GameDBManager.Flag_Query_Total_UserMoney_Minute * 60 * 1000)
            {
                bInCheckTime = true;
            }
            else if (now.Day != LastLastQueryServerTotalUserMoneyTime.Day)
            {
                bInCheckTime = true;
            }

            if (!bInCheckTime)
                return;

            LastLastQueryServerTotalUserMoneyTime = now;
            LogManager.WriteLog(LogTypes.TotalUserMoney, string.Format("{0}\t{1}\t{2}", 10000, GameDBManager.ZoneID, DBQuery.QueryServerTotalUserMoney()));
        }

        #endregion 



        #region RechageLogs
        public static TCPProcessCmdResults CMT_KT_LOG_RECHAGE(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }


                string UserID = fields[0];

                int Money = Int32.Parse(fields[1]);

                int RoleID = Int32.Parse(fields[2]);

                string RETURN = "";


                GameDBManager.RankCacheMgr.OnUserDoSomething(RoleID, RankType.Charge, Money);

                if (DBWriter.UpdateRechageData(dbMgr, UserID, Money) && DBWriter.UpdatetInputMoney(dbMgr, UserID, Money, GameDBManager.ZoneID))
                {
                    RETURN = "1:1";
                }
                else
                {
                    RETURN = "-1:-1";
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, RETURN, nID);


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

        #endregion
    }
}
