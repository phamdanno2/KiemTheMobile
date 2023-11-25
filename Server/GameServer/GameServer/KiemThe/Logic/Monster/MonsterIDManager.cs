using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Tools;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý ID quái
    /// </summary>
    public class MonsterIDManager
    {
        /// <summary>
        /// Danh sách ID quái chưa được sử dụng
        /// </summary>
        private readonly List<long> IdleIDList = new List<long>();

        /// <summary>
        /// Base ID của quái
        /// </summary>
        private long _MaxID = SpriteBaseIds.MonsterBaseId;

        /// <summary>
        /// Trả về ID mới
        /// </summary>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        public long GetNewID(int mapCode)
        {
            long id = SpriteBaseIds.MonsterBaseId;
            lock (this.IdleIDList)
            {
                if (this.IdleIDList.Count > 0)
                {
                    id = IdleIDList.ElementAt(0);
                    this.IdleIDList.RemoveAt(0);
                }
                else
                {
                    id = ++this._MaxID;
                }
            }

            return id;
        }

        /// <summary>
        /// Trả lại ID quái
        /// </summary>
        /// <param name="id"></param>
        public void PushBack(long id)
        {
            lock (this.IdleIDList)
            {
                if (this.IdleIDList.IndexOf(id) < 0 && IdleIDList.Count < 10000)
                {
                    this.IdleIDList.Add(id);
                }
            }

            if (this.IdleIDList.Count > 10000)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Number of monster dynamic IDs inside MonsterIDManager were all used, size = {0}", this.IdleIDList.Count));
            }
        }
    }
}
