using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý quái vật
    /// </summary>
    public class MonsterContainer
    {
        /// <summary>
        /// Danh sách quái vật
        /// </summary>
        private readonly ConcurrentDictionary<int, Monster> Monsters = new ConcurrentDictionary<int, Monster>();

        /// <summary>
        /// Danh sách quái theo ID bản đồ
        /// </summary>
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, Monster>> MonstersByMap = new ConcurrentDictionary<int, ConcurrentDictionary<int, Monster>>();

        /// <summary>
        /// Danh sách quái được cập nhật vị trí liên tục trên bản đồ nhỏ theo ID bản đồ
        /// </summary>
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, Monster>> ContinuouslyUpdateMiniMapMonsters = new ConcurrentDictionary<int, ConcurrentDictionary<int, Monster>>();

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="mapItems"></param>
        public void Initialize(IEnumerable<XElement> mapItems)
        {
            /// Duyệt danh sách XMLNode
            foreach (XElement mapItem in mapItems)
            {
                /// ID bản đồ tương ứng
                int mapCode = (int) Global.GetSafeAttributeLong(mapItem, "ID");

                /// Tạo mới danh sách quái trong bản đồ tương ứng
                this.MonstersByMap[mapCode] = new ConcurrentDictionary<int, Monster>();

                /// Tạo mới danh sách quái trong bản đồ tương ứng
                this.ContinuouslyUpdateMiniMapMonsters[mapCode] = new ConcurrentDictionary<int, Monster>();
            }
        }

        /// <summary>
        /// Thêm quái cập nhật vị trí liên tục vào bản đồ tương ứng
        /// </summary>
        /// <param name="monster"></param>
        public void AddContinuouslyUpdateMiniMap(Monster monster)
        {
            /// Danh sách quái theo bản đồ
            if (this.ContinuouslyUpdateMiniMapMonsters.TryGetValue(monster.CurrentMapCode, out ConcurrentDictionary<int, Monster> objDict))
            {
                objDict[monster.RoleID] = monster;
            }
        }

        /// <summary>
        /// Xóa quái cập nhật vị trí liên tục khỏi bản đồ tương ứng
        /// </summary>
        /// <param name="monster"></param>
        public void RemoveContinuouslyUpdateMiniMap(Monster monster)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.ContinuouslyUpdateMiniMapMonsters.TryGetValue(monster.CurrentMapCode, out ConcurrentDictionary<int, Monster> objDict))
            {
                return;
            }

            /// Toác
            if (objDict == null)
            {
                return;
            }

            /// Xóa khỏi danh sách quái theo bản đồ
            objDict.TryRemove(monster.RoleID, out _);
        }

        /// <summary>
        /// Thêm quái vào bản đồ
        /// </summary>
        /// <param name="monster"></param>
        public void AddObject(Monster monster)
        {
            /// Thêm vào danh sách tổng
            this.Monsters[monster.RoleID] = monster;

            /// Danh sách quái theo bản đồ
            if (this.MonstersByMap.TryGetValue(monster.CurrentMapCode, out ConcurrentDictionary<int, Monster> objDict))
            {
                objDict[monster.RoleID] = monster;
            }
        }

        /// <summary>
        /// Xóa quái
        /// </summary>
        /// <param name="id"></param>
        /// <param name="?"></param>
        public void RemoveObject(Monster obj)
        {
            /// Xóa khỏi danh sách tổng
            this.Monsters.TryRemove(obj.RoleID, out _);

            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(obj.CurrentMapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return;
            }

            /// Toác
            if (monstersDict == null)
            {
                return;
            }

            /// Xóa khỏi danh sách quái theo bản đồ
            monstersDict.TryRemove(obj.RoleID, out _);
        }

        /// <summary>
        /// Trả về danh sách quái cập nhật vị trí liên tục trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<Monster> GetContinuouslyUpdateToMiniMapMonstersByMap(int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.ContinuouslyUpdateMiniMapMonsters.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return new List<Monster>();
            }

            /// Toác
            if (monstersDict == null)
            {
                return new List<Monster>();
            }

            /// Tạo danh sách
            List<Monster> monsters = new List<Monster>();

            /// Duyệt danh sách
            List<int> monsterIDs = monstersDict.Keys.ToList();
            foreach (int monsterID in monsterIDs)
            {
                /// Nếu không tồn tại
                if (!monstersDict.TryGetValue(monsterID, out Monster monster))
                {
                    continue;
                }

                /// Nếu đã chết
                if (monster.IsDead())
                {
                    continue;
                }

                /// Nếu không kiểm tra phụ bản hoặc ở trong phụ bản tương ứng
                if (copySceneID == -1 || copySceneID == monster.CurrentCopyMapID)
                {
                    /// Thêm vào danh sách
                    monsters.Add(monster);
                }
            }

            /// Trả về kết quả
            return monsters;
        }

        /// <summary>
        /// Trả về danh sách quái trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<Monster> GetObjectsByMap(int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return new List<Monster>();
            }

            /// Toác
            if (monstersDict == null)
            {
                return new List<Monster>();
            }

            /// Tạo danh sách
            List<Monster> monsters = new List<Monster>();

            /// Duyệt danh sách
            List<int> monsterIDs = monstersDict.Keys.ToList();
            foreach (int monsterID in monsterIDs)
            {
                /// Nếu không tồn tại
                if (!monstersDict.TryGetValue(monsterID, out Monster monster))
                {
                    continue;
                }

                /// Nếu đã chết
                if (monster.IsDead())
                {
                    continue;
                }

                /// Nếu không kiểm tra phụ bản hoặc ở trong phụ bản tương ứng
                if (copySceneID == -1 || copySceneID == monster.CurrentCopyMapID)
                {
                    /// Thêm vào danh sách
                    monsters.Add(monster);
                }
            }

            /// Trả về kết quả
            return monsters;
        }

        /// <summary>
        /// Trả về danh sách quái trong bản đồ hoặc phụ bản tương ứng thỏa mãn điều kiện
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<Monster> GetObjectsByMap(Predicate<Monster> predicate, int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return new List<Monster>();
            }

            /// Toác
            if (monstersDict == null)
            {
                return new List<Monster>();
            }

            /// Tạo danh sách
            List<Monster> monsters = new List<Monster>();

            /// Duyệt danh sách
            List<int> monsterIDs = monstersDict.Keys.ToList();
            foreach (int monsterID in monsterIDs)
            {
                /// Nếu không tồn tại
                if (!monstersDict.TryGetValue(monsterID, out Monster monster))
                {
                    continue;
                }

                /// Nếu đã chết
                if (monster.IsDead())
                {
                    continue;
                }

                /// Nếu không kiểm tra phụ bản hoặc ở trong phụ bản tương ứng
                if (copySceneID == -1 || copySceneID == monster.CurrentCopyMapID)
                {
                    /// Nếu thỏa mãn điều kiện
                    if (predicate(monster))
                    {
                        /// Thêm vào danh sách
                        monsters.Add(monster);
                    }
                }
            }

            /// Trả về kết quả
            return monsters;
        }

        /// <summary>
        /// Tìm quái theo ID ở bản đồ tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public Monster FindObject(int id, int mapCode)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return null;
            }

            /// Toác
            if (monstersDict == null)
            {
                return null;
            }

            /// Nếu không tồn tại quái tương ứng trong bản đồ
            if (!monstersDict.TryGetValue(id, out Monster monster))
            {
                return null;
            }

            return monster;
        }

        /// <summary>
        /// Trả về tổng số quái trong hệ thống
        /// </summary>
        /// <returns></returns>
        public int GetMonstersCount()
        {
            /// Trả về kết quả
            return this.Monsters.Count;
        }


        /// <summary>
        /// Trả về tổng số quái trong bản đồ tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetMonstersCount(int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return 0;
            }

            /// Toác
            if (monstersDict == null)
            {
                return 0;
            }

            /// Tổng số quái thỏa mãn
            int count = 0;
            /// Duyệt danh sách quái trong bản đồ
            List<int> monsterIDs = monstersDict.Keys.ToList();
            foreach (int monsterID in monsterIDs)
            {
                /// Nếu không tồn tại
                if (!monstersDict.TryGetValue(monsterID, out Monster monster))
                {
                    continue;
                }

                /// Nếu đã chết
                if (monster.IsDead())
                {
                    continue;
                }

                /// Nếu nằm trong cùng phụ bản
                if (copySceneID == -1 || monster.CurrentCopyMapID == copySceneID)
                {
                    count++;
                }
            }

            /// Trả về kết quả
            return count;
        }

        /// <summary>
        /// Trả về tổng số quái trong bản đồ tương ứng theo điều kiện
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetMonstersCount(Predicate<Monster> predicate, int mapCode, int copySceneID = -1)
        {
            /// Nếu không tồn tại bản đồ tương ứng
            if (!this.MonstersByMap.TryGetValue(mapCode, out ConcurrentDictionary<int, Monster> monstersDict))
            {
                return 0;
            }

            /// Toác
            if (monstersDict == null)
            {
                return 0;
            }

            /// Tổng số quái thỏa mãn
            int count = 0;
            /// Duyệt danh sách quái trong bản đồ
            List<int> monsterIDs = monstersDict.Keys.ToList();
            foreach (int monsterID in monsterIDs)
            {
                /// Nếu không tồn tại
                if (!monstersDict.TryGetValue(monsterID, out Monster monster))
                {
                    continue;
                }

                /// Nếu đã chết
                if (monster.IsDead())
                {
                    continue;
                }

                /// Nếu nằm trong cùng phụ bản
                if (copySceneID == -1 || monster.CurrentCopyMapID == copySceneID)
                {
                    /// Nếu thỏa mãn điều kiện
                    if (predicate(monster))
                    {
                        count++;
                    }
                }
            }

            /// Trả về kết quả
            return count;
        }
    }
}
