using GameServer.KiemThe;
using GameServer.KiemThe.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Sự kiện
    /// </summary>
    public partial class Monster
    {
        #region Define
        /// <summary>
        /// Vị trí tiếp theo cần tới
        /// </summary>
        public Point NextMoveTo { get; set; }

        /// <summary>
        /// Có phải AI tự chạy không
        /// </summary>
        public bool IsAIRandomMove { get; set; }

        /// <summary>
        /// Có đang quay trở lại vị trí ban đầu không
        /// </summary>
        public bool IsBackingToOriginPos { get; set; } = false;
        #endregion

        #region Tick move
        /// <summary>
        /// Tick di chuyển quái
        /// </summary>
        public void MonsterAI_TickMove()
        {
            try
            {
                /// Nếu quái đã chết
                if (this.IsDead())
                {
                    return;
                }
                /// Nếu đang di chuyển và đang quay trở lại vị trí ban đầu
                if (KTMonsterStoryBoardEx.Instance.HasStoryBoard(this) && this.IsBackingToOriginPos)
                {
                    return;
                }
                /// Nếu không đuổi mục tiêu và không có lệnh AI Random chạy và cũng không có lệnh về vị trí ban đầu
                else if (this.chaseTarget == null && !this.IsAIRandomMove && !this.IsBackingToOriginPos)
                {
                    return;
                }
                /// Nếu không có vị trí đích
                else if (this.NextMoveTo == new Point(-1, -1) || this.NextMoveTo == new Point(0, 0))
                {
                    return;
                }

                /// Vị trí hiện tại
                Point curPos = this.CurrentPos;

                /// Bỏ vị trí đích đến
                this.ToPos = this.NextMoveTo;

                /// Nếu không nằm ở vị trí có vật cản thì tiến hành tìm đường
                if (KTGlobal.HasPath(this.CurrentMapCode, curPos, this.ToPos) && !Global.InObs(ObjectTypes.OT_MONSTER, this.MonsterZoneNode.MapCode, (int)curPos.X, (int)curPos.Y, 1) || !Global.InObs(ObjectTypes.OT_MONSTER, this.MonsterZoneNode.MapCode, (int)this.NextMoveTo.X, (int)this.NextMoveTo.Y, 1))
                {
                    /// Nếu tồn tại StoryBoard cũ thì xóa
                    KTMonsterStoryBoardEx.Instance.Remove(this);

                    /// Nếu là AI tự chạy
                    if (this.IsAIRandomMove)
                    {
                        /// Nếu là AI tự chạy
                        this.StartPos = this.NextMoveTo;
                    }

                    /// Thực hiện chạy đến vị trí đích
                    KTMonsterStoryBoardEx.Instance.Add(this, this.CurrentPos, this.NextMoveTo, KiemThe.Entities.KE_NPC_DOING.do_run, false, this.UseAStarPathFinder);
                }

                /// Hủy vị trí tiếp theo cần đến
                this.NextMoveTo = new Point(-1, -1);
                /// Hủy đánh dấu AI tự di chuyển
                this.IsAIRandomMove = false;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
