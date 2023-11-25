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
        /// <summary>
        /// Giải tán gia tộc
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public int DestroyFamily(int FamilyID)
        {
            try
            {
                var find = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();
                if (find != null)
                {
                    this.KickAllRoleInDb(FamilyID);

                    this.DeleteAllJoinRequest(FamilyID);

                    this.DeleteFamilyInDB(FamilyID);

                    //REMOVE FAMILY KHỎI THỰC THỂ HIỆN TẠI
                    TotalFamily.TryRemove(FamilyID, out Family _out);

                    foreach(FamilyMember _member in _out.Members)
                    {
                        DBRoleInfo roleInfo = _Database.GetDBRoleInfo(_member.RoleID);
                        if (roleInfo != null)
                        {
                            lock (roleInfo)
                            {
                                roleInfo.FamilyID = 0;
                                roleInfo.FamilyName = "";
                                roleInfo.FamilyRank = (int)FamilyRank.Member;
                            }
                        }
                    }

                    //KICK GIA TỘC NÀY RA KHỎI BANG NẾU BANG CÓ TỒN TẠI
                    GuildManager.getInstance().KickFamilyIfExis(FamilyID);

                    return 100;
                }
            }catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Family, "BUG :" + ex.ToString());
            }
            return -2;
        }


        /// <summary>
        /// Kick toàn bộ thành viên khỏi DB
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public bool KickAllRoleInDb(int FamilyID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = string.Format("Update t_roles set familyid = 0,familyname ='',familyrank = 0 where familyid = " + FamilyID + "");

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

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
        /// Xóa toàn bộ yêu cầu tham gia
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public bool DeleteAllJoinRequest(int FamilyID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = "Delete from t_familyrequestjoin where FamilyID = " + FamilyID + "";

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

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
        /// Xóa toàn bộ gia tộc
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <returns></returns>
        public bool DeleteFamilyInDB(int FamilyID)
        {
            MySQLConnection conn = null;

            try
            {
                conn = this._Database.DBConns.PopDBConnection();

                string cmdText = "Delete from t_family where FamilyID = " + FamilyID + "";

                MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                cmd.ExecuteNonQuery();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                cmd.Dispose();
                cmd = null;

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
    }
}