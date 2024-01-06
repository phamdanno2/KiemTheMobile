using GameDBServer.Core;
using GameDBServer.Data;
using GameDBServer.DB;
using GameDBServer.Logic.FamilyLogic;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameDBServer.Logic.GuildLogic
{
    public partial class GuildManager
    {
        private static GuildManager instance = new GuildManager();

        public int PageDisplay = 10;

        public DBManager _Database = null;

        public int ThisWeek = 0;

        public long LastUpdateStatus = 0;
        public long LastUpdateShare = 0;

        public long RefreshOnlineStatus = 10000;

        public long RefreshShareStatus = 65000;

        public ConcurrentDictionary<int, Guild> TotalGuild = new ConcurrentDictionary<int, Guild>();


        /// <summary>
        /// Lấy ra lãnh thổ của toàn máy chủ
        /// </summary>
        /// <returns></returns>
        public TerritoryInfo GetTotalTerritoryInfo()
        {
            TerritoryInfo _Total = new TerritoryInfo();


            List<Territory> _TotalTerritory = new List<Territory>();

            MySQLConnection conn = null;

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select ID,MapID,MapName,GuildID,Star,Tax,ZoneID,IsMainCity from t_territory order by ID desc");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    Territory _Territory = new Territory();

                    _Territory.ID = Convert.ToInt32(reader["ID"].ToString());
                    _Territory.MapID = Convert.ToInt32(reader["MapID"].ToString());

                    _Territory.MapName = DataHelper.Base64Decode(reader["MapName"].ToString());

                    _Territory.GuildID = Convert.ToInt32(reader["GuildID"].ToString());

                    _Territory.Star = Convert.ToInt32(reader["Star"].ToString());

                    _Territory.Tax = Convert.ToInt32(reader["Tax"].ToString());

                    _Territory.ZoneID = Convert.ToInt32(reader["ZoneID"].ToString());

                    _Territory.IsMainCity = Convert.ToInt32(reader["IsMainCity"].ToString());

                    //FILL TÊN BANG VÀO ĐÂY ĐỂ PHỤC VỤ CHO TRANH ĐOẠT LÃNH THỔ
                    if (TotalGuild.TryGetValue(_Territory.GuildID, out Guild _Out))
                    {
                        _Territory.GuildName = _Out.GuildName;
                    }

                    _TotalTerritory.Add(_Territory);
                }


                _Total.Territorys = _TotalTerritory;
                _Total.TerritoryCount = _TotalTerritory.Count;


                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            return _Total;

        }


        public static GuildManager getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Load ra toàn bộ bang hội
        /// </summary>
        /// <param name="_Db"></param>
        public void Setup(DBManager _Db)
        {
            this._Database = _Db;

            this.LoadAllGuild();

            this.ThisWeek = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);
        }

        #region LoadingAllGuilData


        /// <summary>
        /// Thực hiện ReloadTerorry cho toàn bộ
        /// </summary>
        public void ReloadTerritorys()
        {
            foreach (Guild _Guild in TotalGuild.Values)
            {
                lock (_Guild)
                {
                    _Guild.Territorys = this.GetTerritory(_Guild.GuildID, _Guild.GuildName);
                }
            }
        }


        /// <summary>
        /// Load toàn bộ dánh sách người chơi
        /// </summary>
        public void LoadAllGuild()
        {
            MySQLConnection conn = null;

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select GuildID,GuildName,MoneyBound,MoneyStore,ZoneID,Notify,TotalTerritory,MaxWithDraw,Leader,DateCreate,FamilyMember from t_guild order by GuildID desc");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    Guild _Guild = new Guild();

                    _Guild.GuildID = Convert.ToInt32(reader["GuildID"].ToString());
                    _Guild.GuildName = reader["GuildName"].ToString();
                    _Guild.MoneyBound = Convert.ToInt32(reader["MoneyBound"].ToString());
                    _Guild.MoneyStore = Convert.ToInt32(reader["MoneyStore"].ToString());

                    _Guild.ZoneID = Convert.ToInt32(reader["ZoneID"].ToString());

                    _Guild.Notify = DataHelper.Base64Decode(reader["Notify"].ToString());

                    _Guild.TotalTerritory = Convert.ToInt32(reader["TotalTerritory"].ToString());

                    Console.WriteLine("TODO get territory data list");

                    _Guild.MaxWithDraw = Convert.ToInt32(reader["MaxWithDraw"].ToString());

                    _Guild.Leader = Convert.ToInt32(reader["Leader"].ToString());

                    _Guild.DateCreate = DateTime.Parse(reader["DateCreate"].ToString());

                    string FamilyMember = reader["FamilyMember"].ToString();

                    // Read ra danh sách lãnh thổ
                    _Guild.Territorys = this.GetTerritory(_Guild.GuildID, _Guild.GuildName);
                    //Đọc ra dánh ách tộc
                    _Guild.Familys = FamilyMember;
                    //Đọc ra danh sách vote ưu tú
                    _Guild.GuildVotes = GetVoteGuild(_Guild.GuildID);

                    //Đọc ra danh sách thành viên từ các gia tộc trng bang
                    _Guild.GuildMember = this.GetGuildMemberFromFamilyStr(FamilyMember, _Guild.GuildVotes);

                    //Tính toán tỉ lệ cổ tức từ danh sách thành viên và tài sản cá nhân
                    _Guild.GuildShare = this.GetGuildShare(_Guild.GuildMember);

                    //Cache lại danh sách thành viên đã có cổ tức
                    _Guild.CacheGuildShare = this.CacheAllGuildShare(_Guild.GuildID);

                    // Thực hiện add vào danh sách bang hội
                    TotalGuild.TryAdd(_Guild.GuildID, _Guild);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }

        /// <summary>
        /// Tính toán lại cỏ phần
        /// </summary>
        /// <param name="GuildID"></param>
        /// <returns></returns>
        public List<GuildShare> GetGuildShare(ConcurrentDictionary<int, GuildMember> _ListMember)
        {
            List<GuildShare> _TotalGuildShare = new List<GuildShare>();

            /// Lấy ra tổng số tiền của bang này
            double MaxMoneyGuild = _ListMember.Values.Sum(x => x.GuildMoney);

            if (_ListMember.Count == 0)
            {
                return _TotalGuildShare;
            }

            List<GuildMember> TmpGuildMember = _ListMember.Values.Where(x => x.GuildMoney > 0).ToList();

            int i = 1;
            foreach (GuildMember member in _ListMember.Values)
            {
                GuildShare _Share = new GuildShare();

                double Percent = MaxMoneyGuild <= 0 ? 0 : Math.Round(((member.GuildMoney * 100) / MaxMoneyGuild), 2);

                _Share.FactionID = member.FactionID;
                _Share.FamilyID = member.FamilyID;
                _Share.FamilyName = member.FamilyName;

                _Share.Rank = member.Rank;
                _Share.ID = i;
                _Share.RoleID = member.RoleID;
                _Share.RoleLevel = member.Level;
                _Share.RoleName = member.RoleName;
                _Share.Share = Percent;

                _TotalGuildShare.Add(_Share);

                i++;
            }

            return _TotalGuildShare;
        }

        public List<GuildVote> GetVoteGuild(int GuildID)
        {
            int WEEKID = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);

            List<GuildVote> _TotalVoteGuild = new List<GuildVote>();

            MySQLConnection conn = null;

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select ID,ZoneID,GuildID,RoleVote,VoteCount,WeekID,RoleReviceVote from t_voteguild where WeekID = " + WEEKID + " order by ID desc");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    GuildVote _GuildVote = new GuildVote();

                    _GuildVote.ID = Convert.ToInt32(reader["ID"].ToString());
                    _GuildVote.ZoneID = Convert.ToInt32(reader["ZoneID"].ToString());

                    _GuildVote.GuildID = Convert.ToInt32(reader["GuildID"].ToString());

                    _GuildVote.RoleVote = Convert.ToInt32(reader["RoleVote"].ToString());

                    _GuildVote.VoteCount = Convert.ToInt32(reader["VoteCount"].ToString());

                    _GuildVote.WeekID = Convert.ToInt32(reader["WeekID"].ToString());

                    _GuildVote.RoleReviceVote = Convert.ToInt32(reader["RoleReviceVote"].ToString());

                    _TotalVoteGuild.Add(_GuildVote);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            return _TotalVoteGuild;
        }

        public string GetGuildName(string FamilyID)
        {
            foreach (Guild _Guild in TotalGuild.Values)
            {
                string[] FamilySTR = _Guild.Familys.Split('|');
                if (FamilySTR.Contains(FamilyID))
                {
                    return _Guild.GuildName;
                }
            }

            return "Chưa có bang";
        }

        /// <summary>
        /// Lấy ra danh sách lãnh thổ của bang
        /// </summary>
        /// <param name="GuildID"></param>
        /// <returns></returns>
        public List<Territory> GetTerritory(int GuildID, string GuildName)
        {
            List<Territory> _TotalTerritory = new List<Territory>();

            MySQLConnection conn = null;

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select ID,MapID,MapName,GuildID,Star,Tax,ZoneID,IsMainCity from t_territory where GuildID = " + GuildID + " order by ID desc");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    Territory _Territory = new Territory();

                    _Territory.ID = Convert.ToInt32(reader["ID"].ToString());
                    _Territory.MapID = Convert.ToInt32(reader["MapID"].ToString());

                    _Territory.MapName = DataHelper.Base64Decode(reader["MapName"].ToString());

                    _Territory.GuildID = Convert.ToInt32(reader["GuildID"].ToString());

                    _Territory.Star = Convert.ToInt32(reader["Star"].ToString());

                    _Territory.Tax = Convert.ToInt32(reader["Tax"].ToString());

                    _Territory.GuildName = GuildName;

                    _Territory.ZoneID = Convert.ToInt32(reader["ZoneID"].ToString());

                    _Territory.IsMainCity = Convert.ToInt32(reader["IsMainCity"].ToString());

                    _TotalTerritory.Add(_Territory);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
            return _TotalTerritory;
        }

        /// <summary>
        /// Lấy ra toàn bộ danh sách gia tộ có trong danh sách
        /// </summary>
        /// <param name="FamilyStr"></param>
        /// <returns></returns>
        public ConcurrentDictionary<int, GuildMember> GetGuildMemberFromFamilyStr(string FamilyStr, List<GuildVote> GuildVotes)
        {
            ConcurrentDictionary<int, GuildMember> _TotalMember = new ConcurrentDictionary<int, GuildMember>();

            string[] Pram = FamilyStr.Split('|');

            foreach (string FamilyIDStr in Pram)
            {
                int FamilyID = Int32.Parse(FamilyIDStr);

                Family _Find = FamilyManager.getInstance().GetFamily(FamilyID);
                if (_Find != null)
                {
                    this.FillMemberToGuild(_Find.FamilyID, _TotalMember, GuildVotes);
                }
            }

            return _TotalMember;
        }

        /// <summary>
        /// Fill danh sách thành viên vào bang
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <param name="_Input"></param>
        /// <param name="GuildVotes"></param>
        public void FillMemberToGuild(int FamilyID, ConcurrentDictionary<int, GuildMember> _Input, List<GuildVote> GuildVotes)
        {
            MySQLConnection conn = null;

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select rid,rname,occupation,guildid,level,guildrank,guildmoney,zoneid,roleprestige,familyname,familyid from t_roles where familyid = " + FamilyID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    GuildMember _GuidMember = new GuildMember();

                    _GuidMember.RoleID = Convert.ToInt32(reader["rid"].ToString());

                    _GuidMember.FactionID = Convert.ToInt32(reader["occupation"].ToString());

                    _GuidMember.GuildID = Convert.ToInt32(reader["guildid"].ToString());

                    _GuidMember.FamilyID = Convert.ToInt32(reader["familyid"].ToString());

                    _GuidMember.FamilyName = reader["familyname"].ToString();

                    _GuidMember.GuildMoney = Convert.ToInt32(reader["guildmoney"].ToString());

                    _GuidMember.Level = Convert.ToInt32(reader["level"].ToString());

                    _GuidMember.Prestige = Convert.ToInt32(reader["roleprestige"].ToString());

                    _GuidMember.Rank = Convert.ToInt32(reader["guildrank"].ToString());

                    _GuidMember.RoleName = reader["rname"].ToString();

                    DBRoleInfo otherDbRoleInfo = this._Database.GetDBRoleInfo(_GuidMember.RoleID);

                    _GuidMember.OnlienStatus = Global.GetRoleOnlineState(otherDbRoleInfo);

                    _GuidMember.ZoneID = Convert.ToInt32(reader["zoneid"].ToString());

                    var FindVote = GuildVotes.Where(x => x.RoleReviceVote == _GuidMember.RoleID);

                    if (FindVote != null)
                    {
                        int VoteCount = FindVote.Sum(x => x.VoteCount);
                        _GuidMember.TotalVote = VoteCount;
                    }
                    else
                    {
                        _GuidMember.TotalVote = 0;
                    }

                    _Input.TryAdd(_GuidMember.RoleID, _GuidMember);
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }

        #endregion LoadingAllGuilData

        #region DoGiftProsecc



        public List<GuildShare> CacheAllGuildShare(int GuildID)
        {
            MySQLConnection conn = null;

            List<GuildShare> _TotalGuildShare = new List<GuildShare>();

            try
            {
                conn = _Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Select RoleID,`Share`,GuildID,RoleName,`Rank`,FamilyID,FamilyName from t_guildshare where GuildID = " + GuildID + "");
                //

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();



                while (reader.Read())
                {

                    GuildShare _Share = new GuildShare();

                    _Share.RoleID = Convert.ToInt32(reader["RoleID"].ToString());
                    _Share.Share = Convert.ToDouble(reader["Share"].ToString());
                    _Share.RoleName = reader["RoleName"].ToString();
                    _Share.Rank = Convert.ToInt32(reader["Rank"].ToString());
                    _Share.FamilyID = Convert.ToInt32(reader["FamilyID"].ToString());
                    _Share.FamilyName = reader["FamilyName"].ToString();
                    _Share.ID = Convert.ToInt32(reader["Rank"].ToString());
                    _Share.RoleLevel = 1;
                    _Share.FactionID = 1;


                    _TotalGuildShare.Add(_Share);

                }


                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;


            }

            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            //trả về toàn bộ guild share
            return _TotalGuildShare;
        }

        /// <summary>
        /// Xóa toàn bộ GuildShare của 1 thằng nào đó
        /// </summary>
        /// <param name="Guild"></param>
        /// <returns></returns>
        public bool RemoveAllGuildShare(int Guild)
        {

            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Delete from t_guildshare where GuildID = " + Guild + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Guild, "BUG :" + ex.ToString());
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            return false;
        }

        /// <summary>
        /// Thêm mới guild Share
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="RoleID"></param>
        /// <param name="Share"></param>
        /// <param name="GuildID"></param>
        /// <param name="Rank"></param>
        /// <param name="FamilyID"></param>
        /// <param name="FamilyName"></param>
        /// <returns></returns>
        public bool InsertGuildShare(string RoleName, int RoleID, double Share, int GuildID, int Rank, int FamilyID, string FamilyName)
        {
            MySQLConnection conn = null;

            try
            {
                //Console.WriteLine(RoleID+" " + RoleName);
                conn = this._Database.DBConns.PopDBConnection();
                //----------fix jackson loi sap hang quan ham Rank -> `Rank`
                string cmdText = "Insert into t_guildshare (RoleID,`Share`,GuildID,RoleName,`Rank`,FamilyID,FamilyName) VALUES (" + RoleID + "," + Share + "," + GuildID + ",'" + RoleName + "'," + Rank + "," + FamilyID + ",'" + FamilyName + "')";
                //Console.WriteLine(cmdText);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Guild, "BUG :" + ex.ToString());

                return false;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }
        /// <summary>
        /// Hàm Này reload vào mỗi chủ nhật
        /// </summary>
        public void SetupGuildSharePerWeak()
        {
            //Console.WriteLine("SetupGuildSharePerWeak");
            foreach (Guild _Guild in TotalGuild.Values)
            {
                /// Lấy ra tổng số tiền của bang này

                // Nếu mà bang hội không có thành viên thì tiếp tục
                if (_Guild.GuildMember.Count == 0)
                {
                    continue;
                }

                // Lấy ra tổng số tiền
                double MaxMoneyGuild = _Guild.GuildMember.Values.Sum(x => x.GuildMoney);


                // Lấy ra 10 thằng có cổ tức cao nhất ở thời điểm hiện tại
                List<GuildMember> TmpGuildMember = _Guild.GuildMember.Values.Where(x => x.GuildMoney > 0).OrderByDescending(x => x.GuildMoney).Take(10).ToList();

                int i = 1;
                Console.WriteLine("Sap xep TOP 10 quan ham");
                // Ghi lại 10 thằng có cổ tức cao nhất này vào DB
                foreach (GuildMember member in TmpGuildMember)
                {
                    
                    double Percent = MaxMoneyGuild <= 0 ? 0 : Math.Round(((member.GuildMoney * 100) / MaxMoneyGuild), 2);

                    this.InsertGuildShare(member.RoleName, member.RoleID, Percent, member.GuildID, i, member.FactionID, member.FamilyName);

                    i++;

                }



            }

        }



        public void DoGiftProsecc()
        {
            foreach (Guild _Guild in TotalGuild.Values)
            {
                try
                {
                    // lấy 5 người ưu tú hàng tuần để liếm bạc
                    List<GuildMember> _TotalMember = _Guild.GuildMember.Values.OrderByDescending(x => x.TotalVote).Take(5).ToList();

                    Console.WriteLine(_Guild.GuildName + "|MONEY :" + _Guild.MoneyBound);
                    // lấy ra số tiền thưởng của bang hiện có
                    int _TotalMoneyBound = _Guild.MoneyBound;

                    if (_TotalMoneyBound > 5)
                    {
                        // Tiền chia cho 5 thằng đầu tiên
                        int MoneyGift = _TotalMoneyBound / 5;

                        foreach (GuildMember _Member in _TotalMember)
                        {
                            PacketSendToGs _Packet = new PacketSendToGs();
                            _Packet.chatType = 0;
                            _Packet.extTag1 = _Guild.GuildID;
                            _Packet.index = ChatChannel.Guild;
                            _Packet.Msg = "ADDBACKHOA|" + _Member.RoleID + "|" + _Member.RoleName + "|" + MoneyGift + "|GUILD";
                            _Packet.RoleID = _Member.RoleID;
                            _Packet.roleName = _Member.RoleName;
                            _Packet.serverLineID = -1;
                            _Packet.status = 0;
                            _Packet.toRoleName = _Member.RoleName;

                            // gửi lệnh phát thưởng cho CLIENT
                            this.SendMsgToGameServer(_Packet);

                            LogManager.WriteLog(LogTypes.Guild, "[" + _Guild.GuildID + "][" + _Guild.GuildName + "][ADDTHUONGUUTU] : " + _Member.RoleID + "==>" + MoneyGift);
                        }

                        //Set lại tiền cho bang này
                        int MoneyReduct = (_Guild.MoneyBound / 2) * -1;

                        // Thực hiện set lại quỹ tiền bang
                        this.UpdateGuildMoneyBound(MoneyReduct, _Guild.GuildID);

                        //Set lại tiền cho bang hội
                        _Guild.MoneyBound = _Guild.MoneyBound + MoneyReduct;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Guild, "[BUG] : " + ex.ToString());
                }
            }
        }

        #endregion DoGiftProsecc

        /// <summary>
        /// Update tiền cho bang hội
        /// </summary>
        /// <param name="MoneyAdd"></param>
        /// <param name="GuildId"></param>
        /// <returns></returns>
        public bool UpdateGuildMoneyBound(int MoneyAdd, int GuildId)
        {
            TotalGuild.TryGetValue(GuildId, out Guild _OutGuild);

            if (_OutGuild != null)
            {
                MySQLConnection conn = null;

                try
                {
                    conn = this._Database.DBConns.PopDBConnection();

                    string cmdText = string.Format("Update t_guild set MoneyBound =  MoneyBound + " + MoneyAdd + " where guildid = " + GuildId + "");

                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                    cmd.ExecuteNonQuery();

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                    cmd.Dispose();
                    cmd = null;

                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Guild, "BUG :" + ex.ToString());
                }
                finally
                {
                    if (null != conn)
                    {
                        this._Database.DBConns.PushDBConnection(conn);
                    }
                }
            }

            return false;
        }

        public bool ChangeNotifyGuild(int GuildID, string NotifyIn)
        {
            if (NotifyIn.Length > 1000)
            {
                return false;
            }

            string NotifyCode = DataHelper.Base64Encode(NotifyIn);

            if (this.UpdateGuildNofity(GuildID, NotifyCode))
            {
                TotalGuild.TryGetValue(GuildID, out Guild _OutGuild);

                if (_OutGuild != null)
                {
                    _OutGuild.Notify = NotifyIn;
                }
                this.PushGuildMsg("Tôn chỉ bang đã thay đổi :" + NotifyIn, GuildID, 0, "");

                return true;
            }

            return false;
        }

        public bool UpdateGuildNofity(int GuildId, string Notify)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_guild set Notify = '" + Notify + "' where GuildID = " + GuildId + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Guild, "BUG :" + ex.ToString());
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            return false;
        }


        public void UpdateGuildShareAutoMatic()
        {

            foreach (Guild _Guild in TotalGuild.Values)
            {
                //Xóa toàn bộ thông tin cổ tức để udpate lại
                this.RemoveAllGuildShare(_Guild.GuildID);

            }

            this.SetupGuildSharePerWeak();


            //Cache lại toàn bộ guild share
            foreach (Guild _Guild in TotalGuild.Values)
            {
                //Xóa toàn bộ thông tin cổ tức để udpate lại
                _Guild.CacheGuildShare = this.CacheAllGuildShare(_Guild.GuildID);

            }
        }

        /// <summary>
        /// Reset vote khi sang tuần mới
        /// </summary>
        public void UpdateGuildVoteStatus()
        {
            try
            {
                int WeekID = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);
                // Nếu sang tuần mới thì thực hiện phát thưởng
                if (this.ThisWeek != WeekID)
                {
                    this.ThisWeek = WeekID;

                    LogManager.WriteLog(LogTypes.SQL, "DO DoGiftProsecc!");
                    //Thực hiện add thưởng ưu tú cho tất cả bang hội
                    this.DoGiftProsecc();
                    //Clear vote guild
                    this.ClearVoteGuild();


                    this.UpdateGuildShareAutoMatic();

                    LogManager.WriteLog(LogTypes.SQL, "End DoGiftProsecc!");
                }

                long Now = TimeUtil.NOW();

                if (Now - LastUpdateStatus > RefreshOnlineStatus)
                {

                    LogManager.WriteLog(LogTypes.SQL, "[GUILD]Update Online Status");

                    LastUpdateStatus = Now;

                    foreach (Guild _Guild in TotalGuild.Values)
                    {
                        // Loop toàn bộ ngươi chơi trong bang
                        foreach (GuildMember _Member in _Guild.GuildMember.Values)
                        {
                            DBRoleInfo otherDbRoleInfo = this._Database.GetDBRoleInfo(_Member.RoleID);

                            _Member.OnlienStatus = Global.GetRoleOnlineState(otherDbRoleInfo);
                        }

                        ConcurrentDictionary<int, GuildMember> concurrentDictionary = new ConcurrentDictionary<int, GuildMember>(_Guild.GuildMember.OrderByDescending(x => x.Value.OnlienStatus).ThenByDescending(x => x.Value.Rank));
                    }
                }

                // Vẫn để hàm update cổ tức liên tục này
                if (Now - LastUpdateShare > RefreshShareStatus)
                {

                    LogManager.WriteLog(LogTypes.SQL, "[GUILD]Update Share Status");

                    LastUpdateShare = Now;

                    foreach (Guild _Guild in TotalGuild.Values)
                    {
                        // Thiết lập lại cổ tức
                        _Guild.GuildShare = this.GetGuildShare(_Guild.GuildMember);

                    }
                }


                // Viết thêm 1 hàm tính toán lại rank cho bọn nó ở đây gọi là cache cổ tức




            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Guild, "BUG ::" + ex.ToString());
            }
        }

        /// <summary>
        /// Set toàn bộ Guild Memeber về 0 VOTE
        /// </summary>
        public void ClearVoteGuild()
        {
            try
            {
                ///LOOP toàn bộ bang hội
                foreach (Guild _Guild in TotalGuild.Values)
                {
                    // Loop toàn bộ ngươi chơi trong bang
                    foreach (GuildMember _Member in _Guild.GuildMember.Values)
                    {
                        // set VOTE =0;
                        _Member.TotalVote = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Guild, "BUG :" + ex.ToString());
            }
        }



        /// <summary>
        /// Send MSG TO GS
        /// </summary>
        /// <param name="serverLineID"></param>
        /// <param name="gmCmd"></param>
        public void SendMsgToGameServer(PacketSendToGs SendToGs)
        {
            string Build = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", SendToGs.RoleID, SendToGs.roleName, SendToGs.status, SendToGs.toRoleName, (int)(SendToGs.index), SendToGs.Msg, SendToGs.chatType, SendToGs.extTag1, SendToGs.serverLineID);

            List<LineItem> itemList = LineManager.GetLineItemList();

            if (null != itemList)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].LineID < GameDBManager.KuaFuServerIdStartValue || itemList[i].LineID == GameDBManager.ZoneID)
                    {
                        ChatMsgManager.AddChatMsg(itemList[i].LineID, Build);
                    }
                }
            }
        }

        public bool UpdateRoleJoinGuild(int RoleID, string GuildName, int GuildID, int Rank)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set guildid = " + GuildID + ",guildname ='" + GuildName + "',guildrank = " + Rank + " where rid = " + RoleID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                //UPDATE VÀO ROLE HIỆN TẠI
                DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);

                if (roleInfo != null)
                {
                    lock (roleInfo)
                    {
                        roleInfo.GuildID = GuildID;
                        roleInfo.GuildName = GuildName;
                        roleInfo.FamilyRank = Rank;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Family, "BUG :" + ex.ToString());

                return false;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }

        public bool UpdateLeaverGuild(int RoleID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set guildid = 0,guildname = '',guildrank = 0,guildmoney = 0 where rid = " + RoleID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                //UPDATE VÀO ROLE HIỆN TẠI
                DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);

                if (roleInfo != null)
                {
                    roleInfo.GuildID = 0;
                    roleInfo.GuildName = "";
                    roleInfo.FamilyRank = 0;
                    roleInfo.RoleGuildMoney = 0;


                    Global.UpdateRoleParamByName(this._Database, roleInfo, "TotalGuildMoneyAdd", "0", null);
                    Global.UpdateRoleParamByName(this._Database, roleInfo, "TotalGuildMoneyWithDraw", "0", null);


                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Family, "BUG :" + ex.ToString());

                return false;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }

        public void RoleJoinGuild(int RoleID, int GuildID)
        {
            TotalGuild.TryGetValue(GuildID, out Guild _outGuild);
            if (_outGuild != null)
            {
                if (this.UpdateRoleJoinGuild(RoleID, _outGuild.GuildName, _outGuild.GuildID, (int)GuildRank.Member))
                {
                    GuildMember __GuidMember = new GuildMember();

                    DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);

                    if (roleInfo != null)
                    {
                        __GuidMember.FactionID = roleInfo.Occupation;
                        __GuidMember.FamilyID = roleInfo.FamilyID;
                        __GuidMember.FamilyName = roleInfo.FamilyName;
                        __GuidMember.GuildID = roleInfo.GuildID;
                        __GuidMember.GuildMoney = 0;
                        __GuidMember.Level = roleInfo.Level;
                        __GuidMember.OnlienStatus = 1;
                        __GuidMember.Prestige = roleInfo.Prestige;
                        __GuidMember.Rank = (int)GuildRank.Member;
                        __GuidMember.RoleID = roleInfo.RoleID;
                        __GuidMember.RoleName = roleInfo.RoleName;
                        __GuidMember.TotalVote = 0;
                        __GuidMember.VoteRank = 0;
                        __GuidMember.ZoneID = roleInfo.ZoneID;

                        _outGuild.GuildMember.TryAdd(__GuidMember.RoleID, __GuidMember);
                    }
                }
            }
        }

        public void RoleLeaverGuild(int RoleID, int GuildID)
        {
            TotalGuild.TryGetValue(GuildID, out Guild _outGuild);
            if (_outGuild != null)
            {
                if (this.UpdateLeaverGuild(RoleID))
                {
                    _outGuild.GuildMember.TryRemove(RoleID, out GuildMember _out);
                }
            }
        }

        public List<TerritoryData> GetAllTerritoryData()
        {
            List<TerritoryData> Data = new List<TerritoryData>();

            foreach (Guild _Guild in TotalGuild.Values)
            {
                if (_Guild.Territorys.Count > 0)
                {
                    foreach (Territory _GuildTerorry in _Guild.Territorys)
                    {
                        TerritoryData _Territory = new TerritoryData();
                        _Territory.GuildID = _Guild.GuildID;
                        _Territory.GuildName = _Guild.GuildName;
                        _Territory.IsCity = _GuildTerorry.IsMainCity;
                        _Territory.MapID = _GuildTerorry.MapID;
                        // _Territory.MapName = _GuildTerorry.MapName;
                        _Territory.Tax = _GuildTerorry.Tax;
                        Data.Add(_Territory);
                    }
                }
            }

            return Data;
        }

        public Guild GetGuidIfExits(int FamilyID)
        {
            Guild _guild = null;

            foreach (Guild _Guild in TotalGuild.Values)
            {
                string[] FamilyStr = _Guild.Familys.Split('|');

                foreach (string Str in FamilyStr)
                {
                    if (Int32.Parse(Str) == FamilyID)
                    {
                        _guild = _Guild;

                        break;
                    }
                }
            }

            return _guild;
        }

        public void KickFamilyIfExis(int FamilyID)
        {
            foreach (Guild _Guild in TotalGuild.Values)
            {
                string[] FamilyStr = _Guild.Familys.Split('|');

                foreach (string Str in FamilyStr)
                {
                    if (Int32.Parse(Str) == FamilyID)
                    {
                        this.KickFamily(FamilyID, _Guild.GuildID);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Kick 1 gia tộc ra khỏi bang hội
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <param name="GuilID"></param>
        /// <returns></returns>
        public string KickFamily(int FamilyID, int GuilID)
        {
            // Lấy ra cái bang có cái bang chứa cái gia tộc này
            TotalGuild.TryGetValue(GuilID, out Guild _Guild);

            // nếu ko tìm thấy bang thì báo lỗi
            if (_Guild == null)
            {
                return "-1:ERROR";
            }

            bool IsExits = false;

            string[] FamilyStr = _Guild.Familys.Split('|');

            foreach (string Str in FamilyStr)
            {
                if (Int32.Parse(Str) == FamilyID)
                {
                    IsExits = true;
                    break;
                }
            }

            // nếu ko có gia tộc này trong bang thì báo lỗi
            if (!IsExits)
            {
                return "-2:ERROR";
            }

            //Update gia tộc này không thuộc về bang nào
            if (this.UpdateRoleJoinGuild(FamilyID, 0, "", (int)GuildRank.Member, 0))
            {
                string BUILD = "";

                foreach (string Str in FamilyStr)
                {
                    if (Int32.Parse(Str) == FamilyID)
                    {
                        continue;
                    }
                    else
                    {
                        BUILD = BUILD + Str + "|";
                    }
                }

                char Last = BUILD.Last();

                // Loại bỏ dấu | ở cuối cùng
                if (Last == '|')
                {
                    BUILD = BUILD.Remove(BUILD.Length - 1);
                }

                //Update lại thành viên
                _Guild.Familys = BUILD;

                // Update lại danh sách thành viên của bang

                if (this.UpdateFamilyStr(_Guild.GuildID, _Guild.Familys))
                {
                    // Update lại vote
                    _Guild.GuildVotes = GetVoteGuild(_Guild.GuildID);

                    // Update lại danh sách thành viên
                    _Guild.GuildMember = this.GetGuildMemberFromFamilyStr(_Guild.Familys, _Guild.GuildVotes);

                    //Update lại cổ tức
                    _Guild.GuildShare = this.GetGuildShare(_Guild.GuildMember);

                    string Msg = "Gia tộc đã rời khỏi bang hội [" + _Guild.GuildName + "]";

                    Family _FamilyOK = FamilyManager.getInstance().GetFamily(FamilyID);

                    foreach (FamilyMember _Member in _FamilyOK.Members)
                    {
                        DBRoleInfo roleInfo = _Database.GetDBRoleInfo(_Member.RoleID);
                        if (roleInfo != null)
                        {
                            roleInfo.GuildID = 0;
                            roleInfo.GuildName = "";
                            roleInfo.GuildRank = (int)GuildRank.Member;
                            roleInfo.RoleGuildMoney = 0;
                        }
                    }

                    // Gửi thông báo cho cả tộc biết đã bị đá khỏi bang hội
                    FamilyManager.getInstance().PushFamilyMsg(Msg, FamilyID, -1, "");
                }
            }

            // Nếu  thành công thì trả về ID bang và tên bang hội để update vào RoleData
            return "0:" + _Guild.GuildID + ":" + _Guild.GuildName;
        }

        #region CMD_KT_GUILD_GETINFO

        /// <summary>
        /// CMD_KT_GUILD_GETINFO
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public GuildInfomation GetGuildInfo(int GuildID, int RoleID)
        {
            GuildInfomation _Info = new GuildInfomation();

            LogManager.WriteLog(LogTypes.Guild, "GET GUILD INFO :" + GuildID);

            TotalGuild.TryGetValue(GuildID, out Guild _OutGuild);

            if (_OutGuild != null)
            {
                var find = _OutGuild.GuildMember.Values.Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (find != null)
                {
                    int MoneyHave = find.GuildMoney;

                    string[] Pram = _OutGuild.Familys.Split('|');

                    _Info.FamilyCount = Pram.Count();
                    _Info.GuildID = _OutGuild.GuildID;
                    _Info.GuildName = _OutGuild.GuildName;
                    _Info.MaxWithDraw = _OutGuild.MaxWithDraw;
                    _Info.MemberCount = _OutGuild.GuildMember.Count;
                    _Info.MoneyBound = _OutGuild.MoneyBound;
                    _Info.MoneyStore = _OutGuild.MoneyStore;
                    _Info.Notify = _OutGuild.Notify;
                    _Info.RoleGuildMoney = MoneyHave;
                    _Info.TerritoryCount = _OutGuild.Territorys.Count;
                    _Info.TotalPrestige = _OutGuild.GuildMember.Values.Sum(x => x.Prestige);
                    _Info.GuildMasterName = _OutGuild.GuildMember.Values.Where(x => x.RoleID == _OutGuild.Leader).FirstOrDefault().RoleName;
                }
            }
            else
            {
                LogManager.WriteLog(LogTypes.Guild, "CANT FIND GUILD ID :" + GuildID);
            }

            return _Info;
        }

        #endregion CMD_KT_GUILD_GETINFO

        /// <summary>
        /// Kiểm tra xem 1 thành viên đã có bang chưa
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public bool RoleExitsGuild(int RoleID)
        {
            DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);
            if (roleInfo != null)
            {
                if (roleInfo.GuildID > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check xem Guild đã tồn tại chưa
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="GuildName"></param>
        /// <returns></returns>
        public bool IsGuildExist(DBManager dbMgr, string GuildName)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("SELECT count(*) from t_guild where GuildName='" + GuildName + "'");
                return conn.GetSingleInt(cmdText) > 0;
            }
        }

        /// <summary>
        /// Check xem có guild ID không
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="GuildName"></param>
        /// <returns></returns>
        public int GetGuildID(string GuildName)
        {
            int GuildID = -1;

            MySQLConnection conn = null;
            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("SELECT GuildID from t_guild where GuildName = '" + GuildName + "'");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                if (reader.Read())
                {
                    GuildID = Global.SafeConvertToInt32(reader["GuildID"].ToString());
                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }

            return GuildID;
        }

        /// <summary>
        /// Lấy ra danh hiệu của bang
        /// </summary>
        /// <param name="_Rank"></param>
        /// <returns></returns>
        public string GetGuildTitile(int _Rank)
        {
            if (_Rank <= 0)
            {
                return "";
            }

            /// Danh hiệu theo chức vụ
            string guildRankName = "";

            if (_Rank <= (int)GuildRank.Member)
            {
                guildRankName = "Bang chúng";
            }
            else if (_Rank == (int)GuildRank.Master)
            {
                guildRankName = "Bang chủ";
            }
            else if (_Rank == (int)GuildRank.ViceMaster)
            {
                guildRankName = "Phó bang chủ";
            }
            else if (_Rank == (int)GuildRank.Ambassador)
            {
                guildRankName = "Trưởng lão";
            }
            else if (_Rank == (int)GuildRank.Elite)
            {
                guildRankName = "Tinh anh";
            }

            /// Trả về kết quả
            return guildRankName;
        }

        public bool CheckExitViceMaster(int GuildID)
        {
            TotalGuild.TryGetValue(GuildID, out Guild _GuildOut);

            if (_GuildOut != null)
            {
                var findrole = _GuildOut.GuildMember.Values.Where(x => x.Rank == (int)GuildRank.ViceMaster).FirstOrDefault();
                if (findrole != null)
                {
                    return true;
                }
            }
            return false;
        }


        public void ChangeGuildMemberName(int GuildID, int RoleID, string NewName)
        {
            TotalGuild.TryGetValue(GuildID, out Guild _GuildOut);

            if (_GuildOut != null)
            {
                var findrole = _GuildOut.GuildMember.Values.Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (findrole != null)
                {
                    findrole.RoleName = NewName;
                }
            }

        }

        /// <summary>
        /// Gửi tin nhắn về GAMESERVER QUA TOÀN BỘ KÊNH BANG
        /// </summary>
        /// <param name="MSG"></param>
        /// <param name="GuildID"></param>
        /// <param name="RoleID"></param>
        /// <param name="RoleName"></param>
        public void PushGuildMsg(string MSG, int GuildID, int RoleID, string RoleName)
        {
            PacketSendToGs _Packet = new PacketSendToGs();
            _Packet.chatType = 0;
            _Packet.extTag1 = GuildID;
            _Packet.index = ChatChannel.Guild;
            _Packet.Msg = MSG;
            _Packet.RoleID = RoleID;
            _Packet.roleName = RoleName;
            _Packet.serverLineID = -1;
            _Packet.status = 0;
            _Packet.toRoleName = RoleName;

            // gửi lệnh phát thưởng cho CLIENT
            this.SendMsgToGameServer(_Packet);
        }

        /// <summary>
        /// Update lại danh sách thành viên khi có gia tộc mới gia nhập
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="FamilySTR"></param>
        /// <returns></returns>
        public bool UpdateFamilyStr(int GuildID, string FamilySTR)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_guild set FamilyMember = '" + FamilySTR + "' where guildid = " + GuildID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, "BUG :" + ex.ToString());
                return false;
            }
            finally
            {
                if (null != conn)
                {
                    this._Database.DBConns.PushDBConnection(conn);
                }
            }
        }
    }
}