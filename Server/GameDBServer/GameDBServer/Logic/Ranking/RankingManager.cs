using GameDBServer.Core;
using GameDBServer.DB;
using GameDBServer.Server;
using MySQLDriverCS;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic
{
    /// <summary>
    /// Bảng xếp hạng
    /// </summary>
    public class RankingManager
    {
        private static RankingManager instance = new RankingManager();

        /// <summary>
        /// Nếu đang xử lý dữ liệu
        /// </summary>
        public bool IsRankProseccsing = false;

        /// <summary>
        ///  2 tiếng update bảng xếp hạng 1 lần
        /// </summary>
        private const long MaxDBRoleParamCmdSlot = (60 * 10 * 1 * 1000);

        public long LastUpdateRanking = 0;

        public static RankingManager getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Tổng số bản ghi sẽ lấy ra 1 trang
        /// </summary>
        public int TotalRankNumber = 10;

        /// <summary>
        /// Rank
        /// </summary>
        public Dictionary<RankMode, List<PlayerRanking>> RankServer = new Dictionary<RankMode, List<PlayerRanking>>();

        /// <summary>
        /// Lấy ra xếp hạng rank của 1 người chơi trong bảng xếp hạng
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="_RankInput"></param>
        /// <returns></returns>
        public int GetRankOfPlayer(int RoleID, RankMode _RankInput)
        {
            if (IsRankProseccsing)
            {
                return -1;
            }
            else
            {
                RankServer.TryGetValue(_RankInput, out List<PlayerRanking> _Rank);

                if (_Rank != null)
                {
                    var find = _Rank.Where(x => x.RoleID == RoleID).FirstOrDefault();
                    if (find != null)
                    {
                        return find.ID;
                    }
                    else
                    {
                        return -100;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public int Count(RankMode _ModeIN)
        {
            if(IsRankProseccsing)
            {
                return 0;

            }
            if(RankServer.ContainsKey(_ModeIN))
            {
                int count = 0;

                count = RankServer[_ModeIN].Count;

                return count;
            }
            return 0;
        }

        public void UpdateRank(DBManager _DbInput)
        {
            long Now = TimeUtil.NOW();

            if (Now - LastUpdateRanking > MaxDBRoleParamCmdSlot)
            {
                IsRankProseccsing = true;

                RankServer = new Dictionary<RankMode, List<PlayerRanking>>();
                LastUpdateRanking = Now;


                LogManager.WriteLog(LogTypes.SQL, "DO EXECUTE UPDATE RANK DATA!");

                RankServer[RankMode.CapDo] = GetTop100RankLevel(_DbInput);
                RankServer[RankMode.TaiPhu] = GetTop100RankTaiPhu(_DbInput);
                RankServer[RankMode.VoLam] = GetTop100RankVoLam(_DbInput);
                RankServer[RankMode.LienDau] = GetTop100RankLienDau(_DbInput);
                RankServer[RankMode.UyDanh] = GetTop100RankUyDanh(_DbInput);
                RankServer[RankMode.ThieuLam] = GetTop100Faction(_DbInput, 1);
                RankServer[RankMode.ThienVuong] = GetTop100Faction(_DbInput, 2);
                RankServer[RankMode.DuongMon] = GetTop100Faction(_DbInput, 3);
                RankServer[RankMode.NguDoc] = GetTop100Faction(_DbInput, 4);
                RankServer[RankMode.NgaMy] = GetTop100Faction(_DbInput, 5);
                RankServer[RankMode.ThuyYen] = GetTop100Faction(_DbInput, 6);
                RankServer[RankMode.CaiBang] = GetTop100Faction(_DbInput, 7);
                RankServer[RankMode.ThienNhan] = GetTop100Faction(_DbInput, 8);
                RankServer[RankMode.VoDang] = GetTop100Faction(_DbInput, 9);
                RankServer[RankMode.ConLon] = GetTop100Faction(_DbInput, 10);
                RankServer[RankMode.MinGiao] = GetTop100Faction(_DbInput, 11);
                RankServer[RankMode.DoanThi] = GetTop100Faction(_DbInput, 12);

                IsRankProseccsing = false;

                LogManager.WriteLog(LogTypes.SQL, "END  EXECUTE UPDATE RANK DATA!");
                // THỰC HIỆN CÁC TRUY VẤN Ở ĐÂY ĐỂ FILL RA BẢNG
            }
        }

        #region GetRank Cấp độ

        public List<PlayerRanking> GetTop100RankLevel(DBManager dbMgr)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,experience,level from t_roles order by level desc,experience desc  LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 100)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = reader["rname"].ToString(),
                        Type = (int)RankMode.CapDo,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["experience"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);
                    count++;
                }

              //  GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

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

            return TotalRank;
        }

        #endregion GetRank Cấp độ

        #region RANKTAIPHU

        public List<PlayerRanking> GetTop100RankTaiPhu(DBManager dbMgr)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,taiphu,level from t_ranking order by taiphu desc LIMIT 500");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 500)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = DataHelper.Base64Decode(reader["rname"].ToString()),
                        Type = (int)RankMode.TaiPhu,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["taiphu"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);

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

            return TotalRank;
        }

        #endregion RANKTAIPHU

        #region RANKTAIPHU

        public List<PlayerRanking> GetTop100RankVoLam(DBManager dbMgr)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,volam,level from t_ranking order by volam desc,level desc LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 100)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = DataHelper.Base64Decode(reader["rname"].ToString()),
                        Type = (int)RankMode.TaiPhu,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["volam"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);
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

            return TotalRank;
        }

        #endregion RANKTAIPHU

        #region RANKTAIPHU

        public List<PlayerRanking> GetTop100RankLienDau(DBManager dbMgr)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,liendau,level from t_ranking order by liendau desc,level desc LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 100)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = DataHelper.Base64Decode(reader["rname"].ToString()),
                        Type = (int)RankMode.TaiPhu,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["liendau"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);
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

            return TotalRank;
        }

        #endregion RANKTAIPHU

        #region RANKTAIPHU

        public List<PlayerRanking> GetTop100RankUyDanh(DBManager dbMgr)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,uydanh,level from t_ranking order by uydanh desc,level desc LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 100)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = DataHelper.Base64Decode(reader["rname"].ToString()),
                        Type = (int)RankMode.TaiPhu,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["uydanh"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);
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

            return TotalRank;
        }

        #endregion RANKTAIPHU

        #region RANKTAIPHU

        public List<PlayerRanking> GetTop100Faction(DBManager dbMgr, int FactionID)
        {
            List<PlayerRanking> TotalRank = new List<PlayerRanking>();

            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,sub_id,monphai,level from t_ranking where occupation = " + FactionID + " order by monphai desc,level desc LIMIT 100");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int count = 0;
                while (reader.Read() && count < 100)
                {
                    PlayerRanking paiHangItemData = new PlayerRanking()
                    {
                        ID = count,
                        RoleID = Convert.ToInt32(reader["rid"].ToString()),
                        RoleName = DataHelper.Base64Decode(reader["rname"].ToString()),
                        Type = (int)RankMode.TaiPhu,
                        FactionID = Convert.ToInt32(reader["occupation"].ToString()),
                        RouteID = Convert.ToInt32(reader["sub_id"].ToString()),
                        Level = Convert.ToInt32(reader["level"].ToString()),
                        Value = Convert.ToInt32(reader["monphai"].ToString()),
                    };

                    TotalRank.Add(paiHangItemData);
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

            return TotalRank;
        }

        #endregion RANKTAIPHU

        /// <summary>
        /// Lấy ra rank chỉ định
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<PlayerRanking> GetRank(RankMode Input, int RoleID, int PageNumber)
        {
            List<PlayerRanking> _TotalRank = new List<PlayerRanking>();

            if (IsRankProseccsing)
            {
                return _TotalRank;
            }

            try
            {
                if (RankServer.ContainsKey(Input))
                {
                    if (PageNumber < 0)
                    {
                        Console.WriteLine("Tong ket BXH Level");
                        List<string> strs = new List<string>();
                        RankServer[RankMode.CapDo].ForEach(x =>
                        {
                            var str = x.RoleID + "#" + x.RoleName;
                            strs.Add(str);
                        });
                        System.IO.File.WriteAllText("rankOfLevel.txt", string.Join(Environment.NewLine,strs));

                        Console.WriteLine("Tong ket BXH Tai Phu");
                        strs.Clear();
                        RankServer[RankMode.TaiPhu].ForEach(x =>
                        {
                            var str = x.RoleID + "#" + x.RoleName;
                            strs.Add(str);
                        });
                        System.IO.File.WriteAllText("rankofMoney.txt", string.Join(Environment.NewLine, strs));
                    }
                    else
                    {
                        int END = PageNumber * TotalRankNumber;

                        int START = END - TotalRankNumber;

                        // Nếu mà số lượng trong bản còn thấp hơn cả start của page thì trả về trống
                        if (RankServer[Input].Count <= START)
                        {
                            return _TotalRank;
                        }

                        if (RankServer[Input].Count > START && RankServer[Input].Count < END)
                        {
                            int RANGER = RankServer[Input].Count - START;
                            _TotalRank = RankServer[Input].GetRange(START, RANGER);
                        }
                        else
                        {
                            _TotalRank = RankServer[Input].GetRange(START, TotalRankNumber);
                        }
                        // Lấy ra thứ hạng bản thân

                        var find = RankServer[Input].Where(x => x.RoleID == RoleID).FirstOrDefault();
                        if (find != null)
                        {
                            _TotalRank.Add(find);
                        }
                        else
                        {
                            PlayerRanking _Rank = new PlayerRanking();
                            _Rank.RoleID = RoleID;
                            _Rank.ID = -1000;

                            _TotalRank.Add(_Rank);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);

            }

            return _TotalRank;
        }

        /// <summary>
        /// Lấy ra thứ hạng của 1 người chơi với loại rank chỉ định
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults CMD_KT_RANKING_CHECKING(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
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
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int _RankMode = Int32.Parse(fields[0]);

                int RoleID = Int32.Parse(fields[1]);

                RankMode _Mode = (RankMode)_RankMode;

                int RankingGet = RankingManager.getInstance().GetRankOfPlayer(RoleID, _Mode);

                string OUTDATA = RoleID + ":" + RankingGet;

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, OUTDATA, nID);

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
    }
}