using MySQLDriverCS;
using Server.Tools;
using System;
using System.Linq;

namespace GameDBServer.Logic.FamilyLogic
{
    public partial class FamilyManager
    {
        /// <summary>
        /// Update notify request join
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <param name="Notify"></param>
        /// <returns></returns>
        public int UpdateNotifyRequestJoin(int FamilyID, string Notify)
        {
            if (Notify.Length > 1000)
            {
                return -1;
            }
            var _Family = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();

            if (_Family != null)
            {
                _Family.RequestNotify = Notify;

                MySQLConnection conn = null;

                try
                {
                    conn = this._Database.DBConns.PopDBConnection();

                    string NotifyEcoding = DataHelper.Base64Encode(Notify);

                    string cmdText = string.Format("Update t_family set RequestNotify = '" + NotifyEcoding + "' where FamilyID = " + FamilyID + "");

                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                    cmd.ExecuteNonQuery();

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                    cmd.Dispose();
                    cmd = null;

                    return 100;
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Error, "BUG :" + ex.ToString());

                    return -2;
                }
                finally
                {
                    if (null != conn)
                    {
                        this._Database.DBConns.PushDBConnection(conn);
                    }
                }
            }

            return -3;
        }

        /// <summary>
        /// Update notify
        /// </summary>
        /// <param name="FamilyID"></param>
        /// <param name="Notify"></param>
        /// <returns></returns>
        public int UpdateNotify(int FamilyID, string Notify)
        {
            if (Notify.Length > 1000)
            {
                return -1;
            }
            var _Family = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();

            if (_Family != null)
            {
                string NotifyEcoding = DataHelper.Base64Encode(Notify);

                _Family.Notification = Notify;

                MySQLConnection conn = null;

                try
                {
                    conn = this._Database.DBConns.PopDBConnection();

                    string cmdText = string.Format("Update t_family set Notify = '" + NotifyEcoding + "' where FamilyID = " + FamilyID + "");

                    MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                    cmd.ExecuteNonQuery();

                    GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                    cmd.Dispose();
                    cmd = null;

                    return 100;
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Error, "BUG :" + ex.ToString());

                    return 2;
                }
                finally
                {
                    if (null != conn)
                    {
                        this._Database.DBConns.PushDBConnection(conn);
                    }
                }
            }

            return -3;
        }
    }
}