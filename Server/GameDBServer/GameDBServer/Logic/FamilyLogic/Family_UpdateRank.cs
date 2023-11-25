using GameDBServer.DB;
using GameDBServer.Logic.GuildLogic;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Linq;

namespace GameDBServer.Logic.FamilyLogic
{
    public partial class FamilyManager
    {
        public string UpdateRank(int FamilyID, int Role, int Rank)
        {
            TotalFamily.TryGetValue(FamilyID, out Family _Family);
            if (_Family != null)
            {
                if (Rank == (int)FamilyRank.ViceMaster)
                {
                    var FindPhoToc = _Family.Members.Where(x => x.Rank == Rank).FirstOrDefault();
                    if (FindPhoToc != null)
                    {
                        return "-2:ERROR";
                    }
                }

                var FindMember = _Family.Members.Where(x => x.RoleID == Role).FirstOrDefault();
                if (FindMember != null)
                {
                    if (Rank == (int)FamilyRank.Master)
                    {
                        _Family.Learder = Role;
                        this.UpdateLeaderFamily(Role, FamilyID);


                        GuildManager.getInstance().ChangeRankIfExits(Role, FamilyID, FamilyRank.Master);
                    }

                    FindMember.Rank = Rank;

                    if (this.UpdateRankDatabase(Role, FamilyID, Rank))
                    {
                        return "100:ERROR";
                    }
                }
            }
            else
            {
                return "-1:ERROR";
            }

            return "-100:ERROR";
        }

        public bool UpdateLeaderFamily(int RoleID, int FamilyID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_family set Leader = " + RoleID + " where FamilyID = " + FamilyID + "");

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

        public bool UpdateRankDatabase(int RoleID, int FamilyID, int Rank)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set familyid = " + FamilyID + ",familyrank = " + Rank + " where rid = " + RoleID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

                //UPDATE VÀO ROLE HIỆN TẠI
                DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleID);

                if (roleInfo != null)
                {
                    lock(roleInfo)
                    {
                        roleInfo.FamilyRank = Rank;
                    }

                }

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