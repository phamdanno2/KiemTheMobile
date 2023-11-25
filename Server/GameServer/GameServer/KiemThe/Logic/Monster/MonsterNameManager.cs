using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý tên quái
    /// </summary>
    public class MonsterNameManager
    {
        /// <summary>
        /// Danh sách tên quái theo ID
        /// </summary>
        private static readonly Dictionary<int, string> _MonsterID2NameDict = new Dictionary<int, string>(1000);

        /// <summary>
        /// Thêm tên quái theo ID vào danh sách
        /// </summary>
        /// <param name="monsterID"></param>
        /// <param name="monsterName"></param>
        public static void AddMonsterName(int monsterID, string monsterName)
        {
            lock (MonsterNameManager._MonsterID2NameDict)
            {
                MonsterNameManager._MonsterID2NameDict[monsterID] = monsterName;
            }
        }

        /// <summary>
        /// Trả về tên quái theo ID
        /// </summary>
        /// <param name="monsterID"></param>
        /// <returns></returns>
        public static string GetMonsterName(int monsterID)
        {
            string monsterName = null;
            lock (MonsterNameManager._MonsterID2NameDict)
            {
                if (MonsterNameManager._MonsterID2NameDict.TryGetValue(monsterID, out monsterName))
                {
                    return monsterName;
                }
            }

            return "";
        }
    }
}
