using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 角色在线管理
    /// </summary>
    public class RoleOnlineManager
    {
        #region 基础数据

        /// <summary>
        /// 角色在线词典定义
        /// </summary>
        private static Dictionary<int, long> _RoleOlineTicksDict = new Dictionary<int, long>();

        #endregion 基础数据

        #region 操作方法和函数

        /// <summary>
        /// 获取指定的角色的在线心跳时间
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static long GetRoleOnlineTicks(int roleID)
        {
            long ticks = 0;
            lock (_RoleOlineTicksDict)
            {
                if (!_RoleOlineTicksDict.TryGetValue(roleID, out ticks))
                {
                    ticks = 0;
                }
            }

            return ticks;
        }

        /// <summary>
        /// 更新角色的心跳时间
        /// </summary>
        /// <param name="roleID"></param>
        public static void UpdateRoleOnlineTicks(int roleID)
        {
            long ticks = DateTime.Now.Ticks / 10000;
            lock (_RoleOlineTicksDict)
            {
                if (_RoleOlineTicksDict.ContainsKey(roleID))
                {
                    _RoleOlineTicksDict[roleID] = ticks;
                }
                else
                {
                    _RoleOlineTicksDict.Add(roleID, ticks);
                }
            }
        }

        /// <summary>
        /// 删除角色的心跳时间
        /// </summary>
        /// <param name="roleID"></param>
        public static void RemoveRoleOnlineTicks(int roleID)
        {
            lock (_RoleOlineTicksDict)
            {
                _RoleOlineTicksDict.Remove(roleID);
            }
        }

        #endregion 操作方法和函数
    }
}
