using GameDBServer.Core;
using GameDBServer.Data;
using GameDBServer.DB;
using GameDBServer.Logic.GuildLogic;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameDBServer.Logic.FamilyLogic
{
    public partial class FamilyManager
    {
        private static FamilyManager instance = new FamilyManager();

        public ConcurrentDictionary<int, Family> TotalFamily = new ConcurrentDictionary<int, Family>();

        public DBManager _Database = null;

        public long LastUpdateStatus = 0;

        public long RefreshOnlineStatus = 10000;

        public static FamilyManager getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Lấy ra 1 gia tộc theo ID
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public Family GetFamily(int FamilyID)
        {
            TotalFamily.TryGetValue(FamilyID, out Family _out);
            if (_out != null)
            {
                return _out;
            }
            else
            {
                return null;
            }
        }


        public void ChangeFamilyMemberName(int RoleID, int FamilyID,string NewName)
        {
            var find = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();
            if (find != null)
            {
                var findmember = find.Members.Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (findmember != null)
                {
                    findmember.RoleName = NewName;
                }
            }

        }

        public FamilyMember GetMember(int RoleID, int FamilyID)
        {
            var find = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();
            if (find != null)
            {
                var findmember = find.Members.Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (findmember != null)
                {
                    return findmember;
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy ra danh sách gia tộc
        /// </summary>
        /// <returns></returns>
        public List<FamilyInfo> GetFamilyInfo()
        {
            List<FamilyInfo> AllFamily = new List<FamilyInfo>();

            foreach (Family _Family in TotalFamily.Values)
            {
                FamilyInfo _Info = new FamilyInfo();
                _Info.FamilyID = _Family.FamilyID;
                _Info.FamilyName = _Family.FamilyName;

                var FindHOst = _Family.Members.Where(x => x.Rank == (int)FamilyRank.Master).FirstOrDefault();
                if (FindHOst != null)
                {
                    _Info.Learder = FindHOst.RoleName;
                }
                else
                {
                    _Info.Learder = "SYSTEM";
                }

                _Info.GuildName = GuildManager.getInstance().GetGuildName(_Family.FamilyID + "");
                _Info.RequestNotify = _Family.RequestNotify;
                _Info.TotalPoint = _Family.Members.Sum(x => x.Prestige);

                _Info.TotalMember = _Family.Members.Count;

                AllFamily.Add(_Info);
            }

            return AllFamily;
        }

        /// <summary>
        /// Đọc ra toàn bộ Family sau khi khởi động xong máy chủ
        /// </summary>
        /// <param name="dbMgr"></param>
        public void Setup(DBManager dbMgr)
        {
            this.FillAllFamily(dbMgr);

            this.FillRequestJoin(dbMgr);

            this.FillAllMember(dbMgr);

            this._Database = dbMgr;
        }

        /// <summary>
        /// Kick 1 thành viên khỏi gia tộc
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public bool KickMember(int RoleID, int FamilyID)
        {
            var find = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();
            if (find != null)
            {
                var findmember = find.Members.Where(x => x.RoleID == RoleID).FirstOrDefault();

                if (findmember != null)
                {
                    find.Members.Remove(findmember);

                    this.UpdateFamilyRole(RoleID, "", 0, -1);

                    Guild _Guild = GuildManager.getInstance().GetGuidIfExits(FamilyID);

                    if (_Guild != null)
                    {
                        GuildManager.getInstance().RoleLeaverGuild(RoleID, _Guild.GuildID);
                    }
                    // Gửi thông báo cho các người chơi khác trong tộc
                    this.PushFamilyMsg("Người chơi [" + findmember.RoleName + "] đã rời khỏi tộc", FamilyID, 0, "");

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Thêm thành viên vào gia tộc
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyID"></param>
        /// <param name="RoleName"></param>
        /// <param name="FactionID"></param>
        /// <param name="Rank"></param>
        /// <param name="Level"></param>
        /// <param name="Prestige"></param>
        /// <returns></returns>
        public bool AddMemberFamily(int RoleID, int FamilyID, string RoleName, int FactionID, int Rank, int Level, int Prestige, bool MustJoinGuild = false)
        {
            var _Family = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();

            if (_Family != null)
            {
                // Nếu chưa từng vòa 1 gia tộc nào
                if (!CheckExitsFromAnyFamily(RoleID))
                {
                    FamilyMember _member = new FamilyMember();

                    _member.FactionID = FactionID;
                    _member.Level = Level;
                    _member.OnlienStatus = 0;
                    _member.Prestige = Prestige;
                    _member.Rank = Rank;
                    _member.RoleID = RoleID;
                    _member.RoleName = RoleName;

                    _Family.Members.Add(_member);

                    this.UpdateFamilyRole(RoleID, _Family.FamilyName, _Family.FamilyID, Rank);

                    if (MustJoinGuild)
                    {
                        // CHO THÀNH VIÊN VÀO BANG ĐỂ CHEKC LẠI

                        Guild _Guild = GuildManager.getInstance().GetGuidIfExits(FamilyID);

                        if (_Guild != null)
                        {
                            GuildManager.getInstance().RoleJoinGuild(RoleID, _Guild.GuildID);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update thông tin gia tộc vào DB cho nhân vật
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyName"></param>
        /// <param name="FamilyID"></param>
        /// <param name="Rank"></param>
        /// <returns></returns>
        public bool UpdateFamilyRole(int RoleID, string FamilyName, int FamilyID, int Rank)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set familyid = " + FamilyID + ",familyname ='" + FamilyName + "',familyrank = " + Rank + " where rid = " + RoleID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                //UPDATE VÀO ROLE HIỆN TẠI
                DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);

                if (roleInfo != null)
                {
                    roleInfo.FamilyID = FamilyID;
                    roleInfo.FamilyName = FamilyName;
                    roleInfo.FamilyRank = Rank;
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

        /// <summary>
        /// Check xem thằng này có thuộc family nào không. Cái này hơi cost khả năng chuyển về DICT cho danh sách ROLE
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public bool CheckExitsFromAnyFamily(int RoleID)
        {
            DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);
            if (roleInfo != null)
            {
                if (roleInfo.FamilyID > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Trả về tổng số lượt đi Vượt ải gia tộc đã mở trong tuần của tộc tương ứng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="familyID"></param>
        /// <returns></returns>
        public int GetWeeklyFubenCount(DBManager dbMgr, int familyID)
		{
            //MySQLConnection conn = null;
            //int nTimes = 0;

            //try
            //{
            //    conn = dbMgr.DBConns.PopDBConnection();

            //    string cmdText = string.Format("SELECT WeeklyFubenCount FROM t_family WHERE FamilyID = " + familyID);

            //    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
            //    MySQLDataReader reader = cmd.ExecuteReaderEx();

            //    if (reader.Read())
            //    {
            //        nTimes = Convert.ToInt32(reader["WeeklyFubenCount"].ToString());
            //    }

            //    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

            //    cmd.Dispose();
            //    cmd = null;
            //}
            //finally
            //{
            //    if (null != conn)
            //    {
            //        dbMgr.DBConns.PushDBConnection(conn);
            //    }
            //}

            //return nTimes;

            /// Thông tin tộc tương ứng
            Family family = this.GetFamily(familyID);
            /// Toác
            if (family == null)
			{
                return 0;
			}
            /// Số mã hóa
            int nEncryptedData = family.WeeklyFubenCount;
            /// 4 chữ số đầu đại diện cho năm
            int nYear = nEncryptedData / 1000;
            /// 2 chữ số sau đại diện cho số tuần
            int nWeek = nEncryptedData % 1000 / 10;
            /// Chữ số cuối đại diện số lượt đã mở Vượt ải gia tộc
            int nTimes = nEncryptedData % 10;

            /// Năm nay
            int currentYear = DateTime.Now.Year;
            /// Tuần này
            int currentWeek = this.GetIso8601WeekOfYear(DateTime.Now);
            /// Nếu tuần trước khác tuần này
            if (currentYear != nYear || currentWeek != nWeek)
			{
                /// Reset số lượt
                nTimes = 0;
			}

            /// Trả về kết quả
            return nTimes;
        }

        /// <summary>
        /// Thiết lập tổng số lượt đi Vượt ải gia tộc đã mở trong tuần của tộc tương ứng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="familyID"></param>
        /// <param name="nTimes"></param>
        public bool SetWeeklyFubenCount(DBManager dbMgr, int familyID, int nTimes)
		{
            /// Thông tin tộc tương ứng
            Family family = this.GetFamily(familyID);
            /// Toác
            if (family == null)
            {
                return false;
            }

            /// Năm nay
            int currentYear = DateTime.Now.Year;
            /// Tuần này
            int currentWeek = this.GetIso8601WeekOfYear(DateTime.Now);
            /// Số mã hóa
            int nEncryptedData = currentYear * 1000 + currentWeek * 10 + nTimes;


            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("UPDATE t_family SET WeeklyFubenCount = " + nEncryptedData + " WHERE FamilyID = " + familyID);
                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                /// Cập nhật thông tin
                family.WeeklyFubenCount = nEncryptedData;

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

        /// <summary>
        /// Đọc toàn bộ dữ liệu gia tộc từ DB ra cache để quản lý
        /// </summary>
        /// <param name="dbMgr"></param>
        public void FillAllFamily(DBManager dbMgr)
        {
            MySQLConnection conn = null;

            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                string cmdText = string.Format("Select FamilyID,FamilyName,Leader,FamilyMoney,WeeklyFubenCount,Notify,RequestNotify,DateCreate from t_family order by FamilyID desc");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                while (reader.Read())
                {
                    Family _Family = new Family();
                    _Family.FamilyID = Convert.ToInt32(reader["FamilyID"].ToString());
                    _Family.FamilyName = reader["FamilyName"].ToString();
                    _Family.Learder = Convert.ToInt32(reader["Leader"].ToString());
                    _Family.Notification = DataHelper.Base64Decode(reader["Notify"].ToString());
                    _Family.RequestNotify = DataHelper.Base64Decode(reader["RequestNotify"].ToString());
                    _Family.DateCreate = DateTime.Parse(reader["DateCreate"].ToString());

                    TotalFamily.TryAdd(_Family.FamilyID, _Family);
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

        public void PushFamilyMsg(string MSG, int FamilyID, int RoleID, string RoleName)
        {
            PacketSendToGs _Packet = new PacketSendToGs();
            _Packet.chatType = 0;
            _Packet.extTag1 = FamilyID;
            _Packet.index = ChatChannel.Family;
            _Packet.Msg = MSG;
            _Packet.RoleID = RoleID;
            _Packet.roleName = RoleName;
            _Packet.serverLineID = -1;
            _Packet.status = 0;
            _Packet.toRoleName = RoleName;

            // gửi lệnh phát thưởng cho CLIENT
            this.SendMsgToGameServer(_Packet);
        }

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

        /// <summary>
        /// Fill dữ liệu xin vào tộc
        /// </summary>
        /// <param name="dbMgr"></param>
        public void FillRequestJoin(DBManager dbMgr)
        {
            MySQLConnection conn = null;
            conn = dbMgr.DBConns.PopDBConnection();
            try
            {
                foreach (Family _Family in TotalFamily.Values)
                {
                    string cmdText = string.Format("Select ID,RoleID,RoleName,RoleFactionID,FamilyID,RoleLevel,RolePrestige,TimeRequest from t_familyrequestjoin where FamilyID =" + _Family.FamilyID + "");

                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    List<RequestJoin> MembersReqeustJoin = new List<RequestJoin>();

                    while (reader.Read())
                    {
                        RequestJoin _RequestJoin = new RequestJoin();
                        _RequestJoin.ID = Convert.ToInt32(reader["ID"].ToString());
                        _RequestJoin.FamilyID = Convert.ToInt32(reader["FamilyID"].ToString());
                        _RequestJoin.RoleFactionID = Convert.ToInt32(reader["RoleFactionID"].ToString());
                        _RequestJoin.RoleName = reader["RoleName"].ToString();
                        _RequestJoin.RoleID = Convert.ToInt32(reader["RoleID"].ToString());
                        _RequestJoin.RoleLevel = Convert.ToInt32(reader["RoleLevel"].ToString());
                        _RequestJoin.RolePrestige = Convert.ToInt32(reader["RolePrestige"].ToString());
                        _RequestJoin.TimeRequest = DateTime.Parse(reader["TimeRequest"].ToString());

                        MembersReqeustJoin.Add(_RequestJoin);
                    }

                    _Family.JoinRequest = MembersReqeustJoin;

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                    cmd.Dispose();
                    cmd = null;
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

        public void UpdateFamilyStatus()
        {
            long Now = TimeUtil.NOW();

            if (Now - LastUpdateStatus > RefreshOnlineStatus)
            {
                LogManager.WriteLog(LogTypes.SQL, "[Family]Update Online Status");

                LastUpdateStatus = Now;


                try
                {
                    foreach (Family _Family in TotalFamily.Values)
                    {
                        // Loop toàn bộ ngươi chơi trong bang
                        foreach (FamilyMember _Member in _Family.Members)
                        {
                            DBRoleInfo otherDbRoleInfo = this._Database.GetDBRoleInfo(_Member.RoleID);

                            _Member.OnlienStatus = Global.GetRoleOnlineState(otherDbRoleInfo);

                            _Member.Prestige = otherDbRoleInfo.Prestige;
                        }

                        _Family.Members = _Family.Members.OrderByDescending(x => x.OnlienStatus).ThenByDescending(x => x.Rank).ToList();

                        // this.FillRequestJoin(this._Database);
                    }
                }
                catch(Exception ex)
                {
                    LogManager.WriteLog(LogTypes.SQL, "TOAC :" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Fill dữ liệu xin vào tộc
        /// </summary>
        /// <param name="dbMgr"></param>
        public void FillAllMember(DBManager dbMgr)
        {
            MySQLConnection conn = null;
            conn = dbMgr.DBConns.PopDBConnection();
            try
            {
                foreach (Family _Family in TotalFamily.Values)
                {
                    string cmdText = string.Format("Select rid,rname,occupation,level,familyrank,roleprestige from t_roles where familyid =" + _Family.FamilyID + "");

                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    List<FamilyMember> MembersReqeustJoin = new List<FamilyMember>();

                    while (reader.Read())
                    {
                        FamilyMember _FamilyMember = new FamilyMember();
                        _FamilyMember.FactionID = Convert.ToInt32(reader["occupation"].ToString());
                        _FamilyMember.Level = Convert.ToInt32(reader["level"].ToString());
                        _FamilyMember.RoleID = Convert.ToInt32(reader["rid"].ToString());
                        _FamilyMember.RoleName = reader["rname"].ToString();
                        _FamilyMember.Rank = Convert.ToInt32(reader["familyrank"].ToString());
                        _FamilyMember.Prestige = Convert.ToInt32(reader["roleprestige"].ToString());

                        DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(_FamilyMember.RoleID);

                        _FamilyMember.OnlienStatus = Global.GetRoleOnlineState(otherDbRoleInfo);

                        MembersReqeustJoin.Add(_FamilyMember);
                    }

                    _Family.Members = MembersReqeustJoin;

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, ex.ToString());
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }
        }
    }
}