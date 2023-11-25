using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic.FamilyLogic
{
    public partial class FamilyManager
    {
        /// <summary>
        /// Tạo 1 gia tộc
        /// </summary>
        /// <param name="FamilyName"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public int CreateFamily(string FamilyName, int RoleID, int RoleLevel, int RoleFactionID, string RoleName, int Prestige)
        {

            // Check xem tên gia tộc có tồn tại không
            var find = TotalFamily.Values.Where(x => x.FamilyName == FamilyName).FirstOrDefault();

            if (find != null)
            {
                // Tên gia tộc đã tồn tại
                return -1;
            }
            else
            {
                // Check xem thằng này đã có gia tộc chưa
                if (!CheckExitsFromAnyFamily(RoleID))
                {
                    MySQLConnection conn = null;

                    try
                    {
                        string NOTIFY = DataHelper.Base64Encode("Thông báo gia tộc");

                        conn = this._Database.DBConns.PopDBConnection();

                        string cmdText = string.Format("Insert into t_family(FamilyName,Leader,Notify,DateCreate,RequestNotify) VALUES ('" + FamilyName + "'," + RoleID + ",'" + NOTIFY + "',now(),'" + NOTIFY + "')");

                        MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                        cmd.ExecuteNonQuery();

                        GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                        cmd.Dispose();
                        cmd = null;

                        cmdText = string.Format("Select FamilyID,FamilyName,Leader,Notify,RequestNotify,DateCreate from t_family where FamilyName='" + FamilyName + "'");

                        cmd = new MySQLCommand(cmdText, conn);

                        MySQLDataReader reader = cmd.ExecuteReaderEx();

                        int FAMILYREAD = 0;

                        while (reader.Read())
                        {
                            Family _Family = new Family();
                            _Family.FamilyID = Convert.ToInt32(reader["FamilyID"].ToString());
                            _Family.FamilyName = reader["FamilyName"].ToString();
                            _Family.Learder = Convert.ToInt32(reader["Leader"].ToString());
                            _Family.Notification = reader["Notify"].ToString();
                            _Family.RequestNotify = reader["RequestNotify"].ToString();
                            _Family.DateCreate = DateTime.Parse(reader["DateCreate"].ToString());
                            _Family.JoinRequest = new List<RequestJoin>();
                            FAMILYREAD = _Family.FamilyID;

                            TotalFamily.TryAdd(FAMILYREAD, _Family);
                        }

                        this.AddMemberFamily(RoleID, FAMILYREAD, RoleName, RoleFactionID, 1, RoleLevel, Prestige);

                        return FAMILYREAD;
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
                else
                {
                    return -3;
                }
            }
        }


    }
}
