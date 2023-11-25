using GameDBServer.DB;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Linq;

namespace GameDBServer.Logic.FamilyLogic
{
    public partial class FamilyManager
    {
        public int MaxMemberCanJoin = 30;

        public string ResponseRequestJoin(int RoleID, int Accpect, int FamilyID)
        {
            TotalFamily.TryGetValue(FamilyID, out Family _Family);

            if (_Family != null)
            {
                var findRequeustjoin = _Family.JoinRequest.Where(x => x.RoleID == RoleID).FirstOrDefault();

                if (findRequeustjoin != null)
                {
                    int ROLEREIDQUEUSTJOIN = findRequeustjoin.RoleID;

                    // nếu từ chối tham gia
                    if (Accpect == 0)
                    {
                        if (this.RemoveRequestJoin(ROLEREIDQUEUSTJOIN, FamilyID, false))
                        {
                            return "100:" + ROLEREIDQUEUSTJOIN;
                        }
                    }
                    else
                    {
                        // NẾU ĐỒNG Ý
                        DBRoleInfo roleInfo = _Database.GetDBRoleInfo(ROLEREIDQUEUSTJOIN);

                        if (roleInfo != null)
                        {
                            // nếu người chơi đã có gia tộc rồi
                            if (roleInfo.FamilyID != 0)
                            {
                                this.RemoveRequestJoin(ROLEREIDQUEUSTJOIN, FamilyID, false);

                                return "-200:" + ROLEREIDQUEUSTJOIN;
                            }
                            else
                            {
                                if (_Family.Members.Count + 1 > MaxMemberCanJoin)
                                {
                                    this.RemoveRequestJoin(ROLEREIDQUEUSTJOIN, FamilyID, false);
                                    return "-300:" + ROLEREIDQUEUSTJOIN;
                                }
                                else
                                {
                                    if (this.AddMemberFamily(ROLEREIDQUEUSTJOIN, _Family.FamilyID, roleInfo.RoleName, roleInfo.Occupation, 0, roleInfo.Level, roleInfo.Prestige,true))
                                    {
                                        this.RemoveRequestJoin(ROLEREIDQUEUSTJOIN, FamilyID, true);

                                        lock (roleInfo)
                                        {
                                            roleInfo.FamilyID = _Family.FamilyID;
                                            roleInfo.FamilyName = _Family.FamilyName;
                                            roleInfo.FamilyRank = (int)FamilyRank.Member;
                                        }
                                        this.PushFamilyMsg("Thành viên [" + roleInfo.RoleName + "] đã tham gia Gia tộc", _Family.FamilyID, roleInfo.RoleID, roleInfo.RoleName);

                                        return "200:" + ROLEREIDQUEUSTJOIN;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return "-1:-1";
            }
            return "-100:-1";
        }

        /// <summary>
        /// Hủy bỏ yêu cầu vào gia tộc
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="FamilyID"></param>
        public bool RemoveRequestJoin(int RoleID, int FamilyID, bool RemoveALlRequest)
        {
            if (!RemoveALlRequest)
            {
                var _Family = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();

                if (_Family != null)
                {
                    var find = _Family.JoinRequest.Where(x => x.RoleID == RoleID).FirstOrDefault();
                    if (find != null)
                    {
                        _Family.JoinRequest.Remove(find);
                    }
                }
            }

            MySQLConnection conn = null;

            try
            {
                string cmdText = "";

                conn = this._Database.DBConns.PopDBConnection();

                if (RemoveALlRequest)
                {
                    cmdText = string.Format("Delete from t_familyrequestjoin where RoleID = " + RoleID + " and FamilyID = " + FamilyID + "");
                }
                else
                {
                    cmdText = string.Format("Delete from t_familyrequestjoin where RoleID = " + RoleID + "");
                }

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
        /// Add Request JOIN
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="RoleName"></param>
        /// <param name="RoleFactionID"></param>
        /// <param name="FamilyID"></param>
        /// <param name="RoleLevel"></param>
        /// <param name="RolePrestige"></param>
        public int AddRequestJoin(int RoleID, string RoleName, int RoleFactionID, int FamilyID, int RoleLevel, int RolePrestige)
        {
            var _Family = TotalFamily.Values.Where(x => x.FamilyID == FamilyID).FirstOrDefault();

            if (_Family != null)
            {
                if (_Family.Members.Count >= MaxMemberCanJoin)
                {
                    return -5;
                }

                if(_Family.JoinRequest==null)
                {
                    _Family.JoinRequest = new System.Collections.Generic.List<RequestJoin>();
                }

                var find = _Family.JoinRequest.Where(x => x.RoleID == RoleID).FirstOrDefault();

                // Nếu thằng này đã xin gia nhập rồi
                if (find != null)
                {
                    return -1;
                }
                else
                {
                    MySQLConnection conn = null;

                    try
                    {
                        conn = this._Database.DBConns.PopDBConnection();

                        string cmdText = string.Format("Insert into t_familyrequestjoin(RoleID,RoleName,RoleFactionID,FamilyID,RoleLevel,RolePrestige,TimeRequest) VALUES (" + RoleID + ",'" + RoleName + "'," + RoleFactionID + "," + FamilyID + "," + RoleLevel + "," + RolePrestige + ",now())");

                        MySQLCommand cmd = new MySQLCommand(cmdText, conn);

                        cmd.ExecuteNonQuery();

                        GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", cmdText), EventLevels.Important);

                        cmd.Dispose();
                        cmd = null;




                        RequestJoin _Reqesut = new RequestJoin();
                        _Reqesut.FamilyID = FamilyID;
                        _Reqesut.ID = RoleID;
                        _Reqesut.RoleFactionID = RoleFactionID;
                        _Reqesut.RoleID = RoleID;
                        _Reqesut.RoleLevel = RoleLevel;
                        _Reqesut.RoleName = RoleName;
                        _Reqesut.RolePrestige = RolePrestige;
                        _Reqesut.TimeRequest = DateTime.Now;

                        _Family.JoinRequest.Add(_Reqesut);



                        return 100;



                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Family, "BUG :" + ex.ToString());

                        return -3;
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

            return -4;
        }
    }
}