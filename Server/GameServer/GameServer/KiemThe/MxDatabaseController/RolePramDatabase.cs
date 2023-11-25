using GameServer.Logic;
using Server.Tools;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace GameServer.KiemThe.MxDatabaseController
{
    public static class RolePramDatabase
    {
        /// <summary>
        /// Đọc ra biến lưu trữ bởi key
        /// </summary>
        /// <param name="client"></param>
        /// <param name="RolePramId"></param>
        /// <returns></returns>
        public static int ReadRolePramInt(KPlayer client, int RolePramId)
        {
            int Value = -1;

            using (var db = new KiemTheMixDbEntities())
            {
                var find = db.RoleParamInts.Where(x => x.RoleID == client.RoleID && x.ZoneID == client.ZoneID && x.ParamID == RolePramId).FirstOrDefault();
                if (find != null)
                {
                    return (int)find.ParamValue;
                }
            }

            return Value;
        }

        /// <summary>
        /// Add or Update Value
        /// </summary>
        /// <param name="client"></param>
        /// <param name="RolePramId"></param>
        /// <param name="Value"></param>
        public static bool AddOrUpdate(KPlayer client, int RolePramId, int Value)
        {
            using (var db = new KiemTheMixDbEntities())
            {
                var find = db.RoleParamInts.Where(x => x.RoleID == client.RoleID && x.ZoneID == client.ZoneID && x.ParamID == RolePramId).FirstOrDefault();
                if (find != null)
                {
                    find.ParamValue = Value;
                    db.RoleParamInts.AddOrUpdate(find);
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, "[SQLMIXDATABASE][" + client.RoleID + "][" + RolePramId + "] WRITER VALUE :" + Value + "|BUG :" + ex.ToString());
                        return false;
                    }
                }
                else
                {
                    RoleParamInt Role = new RoleParamInt();
                    Role.RoleID = client.RoleID;
                    Role.ParamID = RolePramId;
                    Role.ParamValue = Value;
                    Role.ZoneID = client.ZoneID;
                    db.RoleParamInts.Add(Role);
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, "[SQLMIXDATABASE][" + client.RoleID + "][" + RolePramId + "] WRITER VALUE :" + Value + "|BUG :" + ex.ToString());
                        return false;
                    }
                }
            }
        }
    }
}