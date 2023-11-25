using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Server;

namespace GameServer.Logic
{
    /// <summary>
    /// 角色名称到ID的映射
    /// </summary>
    class RoleName2IDs
    {
        /// <summary>
        /// 角色名称到用户ID的映射
        /// </summary>
        private static Dictionary<string, int> _S2UDict = new Dictionary<string, int>(1000);

        /// <summary>
        /// 添加一个在线的名字映射
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="userID"></param>
        public static void AddRoleName(string roleName, int roleID)
        {
            lock (_S2UDict)
            {
                _S2UDict[roleName] = roleID;
            }
        }

        /// <summary>
        /// 删除一个在线的名字映射
        /// </summary>
        /// <param name="clientSocket"></param>
        public static void RemoveRoleName(string roleName)
        {
            lock (_S2UDict)
            {
                if (_S2UDict.ContainsKey(roleName))
                {
                    _S2UDict.Remove(roleName);
                }
            }
        }

        /// <summary>
        /// 根据RoleName查找RoleID
        /// </summary>
        /// <param name="clientSocket"></param>
        public static int FindRoleIDByName(string roleName, bool queryFromDB = false)
        {
            int roleID = -1;
            lock (_S2UDict)
            {
                if (!_S2UDict.TryGetValue(roleName, out roleID))
                {
                    roleID = -1;
                }
            }

            if (roleID < 0 && queryFromDB)
            {
                //从DBServer获取角色的所在的线路
                string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_SPR_QUERYIDBYNAME, string.Format("{0}:{1}:0", 0, roleName), GameManager.LocalServerId);
                if (null != dbFields || dbFields.Length >= 5)
                {
                    roleID = Global.SafeConvertToInt32(dbFields[3]);
                } 
            }

            return roleID;
        }

        // 角色改名
        public static void OnChangeName(int roleId, string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                return;
            }

            lock (_S2UDict)
            {
                if (_S2UDict.ContainsKey(oldName))
                {
                    _S2UDict.Remove(oldName);
                    _S2UDict.Add(newName, roleId);
                }
            }
        }
    }
}
