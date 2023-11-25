﻿using GameDBServer.DB;
using System.Security.Cryptography;
using System.Text;

namespace GameDBServer
{
    public class HttpService
    {
        public static string WebKey = "9377(*)#mst9";

        public static string MakeMD5Hash(string input)
        {
            string result;
            using (MD5 md = MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                byte[] array = md.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("X2"));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        public static bool FixPostion(int RoleiD, DBManager _Database)
        {
            string ABC = "5:" + 0 + ":5090:3270";
            DBRoleInfo roleInfo = _Database.GetDBRoleInfo(RoleiD);

            roleInfo.Position = ABC;

            bool ret = false;

            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("UPDATE t_roles SET position={0} WHERE rid={1}", ABC, RoleiD);

                ret = conn.ExecuteNonQueryBool(cmdText);
            }

            return ret;
        }
    }
}