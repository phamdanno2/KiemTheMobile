using GameServer.Core.Executor;
using GameServer.KiemThe.Entities;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý quái
    /// </summary>
    public class MonsterManager
    {
        /// <summary>
        /// Đối tượng quản lý quái
        /// </summary>
        private readonly MonsterContainer MyMonsterContainer = new MonsterContainer();

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="mapItems"></param>
        public void Initialize(IEnumerable<XElement> mapItems)
        {
            this.MyMonsterContainer.Initialize(mapItems);
        }

        #region API
        /// <summary>
        /// Thêm quái cập nhật vị trí liên tục vào bản đồ tương ứng
        /// </summary>
        /// <param name="monster"></param>
        public void AddContinuouslyUpdateMiniMap(Monster monster)
        {
            this.MyMonsterContainer.AddContinuouslyUpdateMiniMap(monster);
        }

        /// <summary>
        /// Xóa quái cập nhật vị trí liên tục khỏi bản đồ tương ứng
        /// </summary>
        /// <param name="monster"></param>
        public void RemoveContinuouslyUpdateMiniMap(Monster monster)
        {
            this.MyMonsterContainer.RemoveContinuouslyUpdateMiniMap(monster);
        }

        /// <summary>
        /// Thêm quái vào danh sách
        /// </summary>
        /// <param name="monster"></param>
        public void AddMonster(Monster monster)
        {
            this.MyMonsterContainer.AddObject(monster);
        }

        /// <summary>
        /// Xóa quái khỏi danh sách quản lý
        /// </summary>
        /// <param name="monster"></param>
        public void RemoveMonster(Monster monster)
        {
            this.MyMonsterContainer.RemoveObject(monster);
            this.MyMonsterContainer.RemoveContinuouslyUpdateMiniMap(monster);
        }

        /// <summary>
        /// Trả về tổng số quái trong danh sách quản lý
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public int GetMonstersCount()
        {
            return this.MyMonsterContainer.GetMonstersCount();
        }

        /// <summary>
        /// Trả về tổng số quái trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetMonstersCount(int mapCode, int copySceneID = -1)
        {
            return this.MyMonsterContainer.GetMonstersCount(mapCode, copySceneID);
        }

        /// <summary>
        /// Trả về tổng số quái trong bản đồ hoặc phụ bản tương ứng theo điều kiện
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public int GetMonstersCount(Predicate<Monster> predicate, int mapCode, int copySceneID = -1)
        {
            return this.MyMonsterContainer.GetMonstersCount(predicate, mapCode, copySceneID);
        }

        /// <summary>
        /// Trả về danh sách quái theo bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<Monster> GetObjectsByMap(int mapCode, int copySceneID = -1)
        {
            return this.MyMonsterContainer.GetObjectsByMap(mapCode, copySceneID);
        }

        /// <summary>
        /// Trả về danh sách quái cập nhật vị trí liên tục trong bản đồ hoặc phụ bản tương ứng
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="copySceneID"></param>
        /// <returns></returns>
        public List<Monster> GetContinuouslyUpdateToMiniMapMonstersByMap(int mapCode, int copySceneID = -1)
        {
            return this.MyMonsterContainer.GetContinuouslyUpdateToMiniMapMonstersByMap(mapCode, copySceneID);
        }

        /// <summary>
        /// Tìm quái theo ID
        /// </summary>
        /// <param name="client"></param>
        public Monster FindMonster(int mapCode, int roleID)
        {
            return this.MyMonsterContainer.FindObject(roleID, mapCode);
        }
        #endregion

        #region Socket
        /// <summary>
        /// Gửi thông tin quái về Client
        /// </summary>
        /// <param name="client"></param>
        public int SendMonstersToClient(SocketListener sl, KPlayer client, TCPOutPacketPool pool, List<Object> objList, int cmd)
        {
            if (null == objList) return 0;

            int totalCount = 0;

            for (int i = 0; i < objList.Count && i < 50; i++)
            {
                if (!(objList[i] is Monster))
                {
                    continue;
                }

                if ((objList[i] as Monster).m_CurrentLife <= 0 || !(objList[i] as Monster).Alive)
                {
                    continue;
                }

                MonsterData md = (objList[i] as Monster).GetMonsterData();

                TCPOutPacket tcpOutPacket = DataHelper.ObjectToTCPOutPacket<MonsterData>(md, pool, cmd);
                if (!sl.SendData(client.ClientSocket, tcpOutPacket))
                {
                    break;
                }

                totalCount++;
            }

            return totalCount;
        }
        #endregion

        #region Sự kiện Dead

        /// <summary>
        /// Danh sách quái đang chờ thực hiện hàm Dead
        /// </summary>
        private readonly ConcurrentDictionary<int, Monster> ListDelayDeadMonster = new ConcurrentDictionary<int, Monster>();

        /// <summary>
        /// Thêm quái vào danh sách chờ thực thi động tác chết
        /// </summary>
        /// <param name="obj"></param>
        public void AddDelayDeadMonster(Monster obj)
        {
            if (!this.ListDelayDeadMonster.ContainsKey(obj.RoleID))
            {
                obj.AddToDeadQueueTicks = TimeUtil.NOW();
                this.ListDelayDeadMonster[obj.RoleID] = obj;
            }
        }

        /// <summary>
        /// Thực hiện sự kiện Dead ngay lập tức
        /// </summary>
        /// <param name="obj"></param>
        public void DeadMonsterImmediately(Monster obj)
        {
            obj.OnDead();
            this.AddDelayDeadMonster(obj);
        }

        /// <summary>
        /// Sự kiện Dead
        /// </summary>
        public void DoMonsterDeadCall()
        {
            long nowTicks = TimeUtil.NOW();
            List<int> lsMonster = new List<int>();

            List<int> keys = this.ListDelayDeadMonster.Keys.ToList();
            foreach (int key in keys)
            {
                if (this.ListDelayDeadMonster.TryGetValue(key, out Monster monster))
                {
                    if (nowTicks - monster.AddToDeadQueueTicks >= 1500)
                    {
                        lsMonster.Add(key);
                    }
                }
            }

            foreach (int key in lsMonster)
            {
                if (this.ListDelayDeadMonster.TryGetValue(key, out Monster monster))
                {
                    /// Thực thi sự kiện OnDead
                    monster.OnDead();
                }
                this.ListDelayDeadMonster.TryRemove(key, out _);
            }
        }
        #endregion


        #region Thông tin quái

        /// <summary>
        /// Đọc dữ liệu quái vật từ XMLNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public MonsterStaticInfo GetMonsterStaticInfo(XElement xmlNode)
        {
            MonsterStaticInfo monsterInfo = new MonsterStaticInfo();
            monsterInfo.Name = xmlNode.Attribute("Name").Value;
            monsterInfo.ResName = xmlNode.Attribute("ResName").Value;
            monsterInfo.ExtensionID = int.Parse(xmlNode.Attribute("ID").Value);
            monsterInfo.SeekRange = 600;//int.Parse(xmlNode.Attribute("SeekRange").Value);
            monsterInfo.Level = int.Parse(xmlNode.Attribute("Level").Value);
            monsterInfo.Title = xmlNode.Attribute("Title").Value;
            int seriesIdx = int.Parse(xmlNode.Attribute("Elemental").Value);
            monsterInfo.Elemental = seriesIdx <= (int)KE_SERIES_TYPE.series_none || seriesIdx >= (int)KE_SERIES_TYPE.series_num ? KE_SERIES_TYPE.series_none : (KE_SERIES_TYPE)seriesIdx;
            monsterInfo.Exp = int.Parse(xmlNode.Attribute("Level").Value);
            monsterInfo.MaxHP = int.Parse(xmlNode.Attribute("MaxHP").Value);
            monsterInfo.AtkSpeed = int.Parse(xmlNode.Attribute("AtkSpeed").Value);
            monsterInfo.Camp = int.Parse(xmlNode.Attribute("Camp").Value);
            monsterInfo.AIID = int.Parse(xmlNode.Attribute("AIID").Value);
            monsterInfo.MonsterType = (MonsterAIType)int.Parse(xmlNode.Attribute("MonsterType").Value);


            monsterInfo.MoveSpeed = int.Parse(xmlNode.Attribute("MoveSpeed").Value);

            if (monsterInfo.MonsterType == MonsterAIType.Static_ImmuneAll || monsterInfo.MonsterType == MonsterAIType.Static)
            {
                monsterInfo.MoveSpeed = 0;
            }
            if (monsterInfo.MonsterType == MonsterAIType.Normal || monsterInfo.MonsterType == MonsterAIType.Hater || monsterInfo.MonsterType == MonsterAIType.Special_Normal)
            {
                monsterInfo.MoveSpeed = 8;
            }
            else if (monsterInfo.MonsterType == MonsterAIType.Elite)
            {
                monsterInfo.MoveSpeed = 10;
            }
            else if (monsterInfo.MonsterType == MonsterAIType.Leader)
            {
                monsterInfo.MoveSpeed = 12;
            }
            else if (monsterInfo.MonsterType == MonsterAIType.Boss || monsterInfo.MonsterType == MonsterAIType.Pirate || monsterInfo.MonsterType == MonsterAIType.Special_Boss)
            {
                monsterInfo.MoveSpeed = 14;
            }

            monsterInfo.ScriptID = string.IsNullOrEmpty(xmlNode.Attribute("ScriptID").Value) ? -1 : int.Parse(xmlNode.Attribute("ScriptID").Value);

            monsterInfo.Auras = xmlNode.Attribute("Auras").Value;
            monsterInfo.Skills = xmlNode.Attribute("Skills").Value;

            return monsterInfo;
        }

        #endregion

    }
}
