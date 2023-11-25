using GameDBServer.DB;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Linq;

namespace GameDBServer.Logic.GuildLogic
{
    public partial class GuildManager
    {
        /// <summary>
        /// Thay đổi chức vụ của 1 thành viên trong bang
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="GuildID"></param>
        /// <param name="Rank"></param>
        /// <returns></returns>
        public bool ChangeRank(int RoleID, int GuildID, int Rank)
        {
            // Mỗi bang chỉ có 1 phó bang chủ
            if (Rank == (int)GuildRank.ViceMaster)
            {

                if (this.CheckExitViceMaster(GuildID))
                {
                    return false;
                }
            }

            LogManager.WriteLog(LogTypes.Guild, "ROLEID :" + RoleID + "| GuildID :" + GuildID + "| Rank :" + Rank + "UPDATE DATABASE");
            // Update rank cho người chơi
            if (this.UpdateDatabaseRoleRankGuiild(RoleID, GuildID, Rank))
            {

                LogManager.WriteLog(LogTypes.Guild, "ROLEID :" + RoleID + "| GuildID :" + GuildID + "| Rank :" + Rank + "UPDATE RANK OBJECT");

                this.UpdateRoleRankObject(RoleID, GuildID, Rank);

                // Nếu chức muốn bổ nhiệm là bang chủ thì thực hiện udpate lại LEADER cho bang chủ
                if (Rank == (int)GuildRank.Master)
                {
                    LogManager.WriteLog(LogTypes.Guild, "ROLEID :" + RoleID + "| GuildID :" + GuildID + "| Rank :" + Rank + "UPDATE LẠI BANG CHỦ");
                    this.UpdateDatabaseLeader(RoleID, GuildID);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public Guild GetGuidsIfExis(int FamilyID)
        {
            foreach (Guild _Guild in TotalGuild.Values)
            {
                string[] FamilyStr = _Guild.Familys.Split('|');

                foreach (string Str in FamilyStr)
                {
                    if (Int32.Parse(Str) == FamilyID)
                    {
                        return _Guild;
                    }
                }
            }
            return null;
        }

        public Guild GetGuildByID(int GuidID)
        {
            TotalGuild.TryGetValue(GuidID, out Guild OutGuild);
            if (OutGuild != null)
            {
                return OutGuild;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Chắc chắn sẽ được chuyeuenr
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyID"></param>
        /// <param name="_RankDest"></param>
        public void ChangeRankIfExits(int RoleID, int FamilyID, FamilyRank _RankDest)
        {
            Guild _GuildFind = this.GetGuidsIfExis(FamilyID);

            // nếu thằng này có bang hội thì check xem nó đang làm chức vụ gì
            if (_GuildFind != null)
            {
                var FindMember = _GuildFind.GuildMember.Values.Where(x => x.RoleID == RoleID).FirstOrDefault();

                if (FindMember != null)
                {
                    // nếu thằng này được bổ nhiệm lên làm tộc trưởng
                    if (_RankDest == FamilyRank.Master)
                    {
                        if (FindMember.Rank == (int)GuildRank.ViceMaster || FindMember.Rank == (int)GuildRank.Master)
                        {
                            return;
                        }

                        //mà thằng này ở bên này đang không làm trưởng não
                        // thì chuyển cho thằng này làm trưởng lão
                        //if (FindMember.Rank != (int)GuildRank.Ambassador)
                        //{
                        //    this.ChangeRank(RoleID, _GuildFind.GuildID, (int)GuildRank.Ambassador);

                        //    string BUILD = "CHANGERANK|" + RoleID + "|" + _GuildFind.GuildID + "|" + (int)GuildRank.Ambassador;

                        //    this.PushGuildMsg(BUILD, _GuildFind.GuildID, RoleID, "");
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Update lại chức danh cho 1 thành viên trong hội
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="GuildID"></param>
        /// <param name="Rank"></param>
        public void UpdateRoleRankObject(int RoleID, int GuildID, int Rank)
        {
            TotalGuild.TryGetValue(GuildID, out Guild _GuildOut);
            if (_GuildOut != null)
            {
                var findrole = _GuildOut.GuildMember.Values.Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (findrole != null)
                {
                    findrole.Rank = Rank;

                    if (Rank == (int)GuildRank.Master)
                    {
                        _GuildOut.Leader = RoleID;
                    }
                    DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);
                    if (roleInfo != null)
                    {
                        lock (roleInfo)
                        {
                            roleInfo.GuildRank = Rank;
                        }
                    }
                    string MSG = "Người chơi [" + findrole.RoleName + "] đã được bổ nhiệm lên chức vụ :" + this.GetGuildTitile(Rank);

                    this.PushGuildMsg(MSG, GuildID, RoleID, findrole.RoleName);
                }
            }
        }

        /// <summary>
        /// Update rank cho 1 thành viên
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="GuildID"></param>
        /// <param name="Rank"></param>
        public bool UpdateDatabaseRoleRankGuiild(int RoleID, int GuildID, int Rank)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set guildrank = " + Rank + " where guildid = " + GuildID + " and rid = " + RoleID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                LogManager.WriteLog(LogTypes.Guild, cmdText);

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

        public bool UpdateDatabaseLeader(int RoleID, int GuildID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_guild set Leader = " + RoleID + " where GuildID = " + GuildID + "");

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
    }
}