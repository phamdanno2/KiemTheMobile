using GameServer.KiemThe;
using GameServer.KiemThe.Core.MonsterAIScript;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.LuaSystem;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// AI của quái
    /// </summary>
    public partial class Monster
    {
        #region Config
        /// <summary>
        /// Số lần thử tìm vị trí xung quanh đi được tối đa
        /// </summary>
        private const int AIMaxTryFindRandomPosAroundTimes = 10;

        /// <summary>
        /// Tỷ lệ % để thực hiện di chuyển
        /// </summary>
        private const int AIRandomMoveBelowRate = 50;
        #endregion

        #region Properties
        /// <summary>
        /// Danh sách kỹ năng tự chọn
        /// <para>Nếu AIScript là mặc định thì không có tác dụng</para>
        /// </summary>
        public List<SkillLevelRef> CustomAISkills { get; private set; } = new List<SkillLevelRef>();
		#endregion

		#region Private fields
		/// <summary>
		/// Mục tiêu đuổi theo
		/// </summary>
		private GameObject chaseTarget = null;

        /// <summary>
        /// Vị trí ban đầu
        /// </summary>
        public Point StartPos { get; set; }

        /// <summary>
        /// Vị trí ban đầu khi quái được tái sinh
        /// </summary>
        public Point InitializePos { get; set; } = new Point(0, 0);
        #endregion

        #region Core
        /// <summary>
        /// Nhận sát thương
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damageTaken"></param>
        public override void TakeDamage(GameObject attacker, int damageTaken)
        {
            try
            {
                /// Nếu không có attacker
                if (attacker == null)
                {
                    return;
                }

                /// Nếu là quái tĩnh thì thôi
                if (this.IsStatic)
                {
                    return;
                }

                /// Nếu là NPC thì thôi
                if (this.MonsterType == MonsterAIType.DynamicNPC)
                {
                    return;
                }

                //base.TakeDamage(attacker, damageTaken);
                /// Thực thi sự kiện khi chịu sát thương
                this.OnTakeDamage?.Invoke(attacker, damageTaken);

                /// Nếu đang di chuyển
                if (this.IsMoving || this.IsBackingToOriginPos)
                {
                    return;
                }

                /// Nếu có mục tiêu đang đuổi theo
                if (this.chaseTarget != null && !this.chaseTarget.IsDead() && this.chaseTarget.CurrentMapCode == this.CurrentMapCode && this.chaseTarget.CurrentCopyMapID == this.CurrentCopyMapID)
                {
                    return;
                }

                /// Nếu là kẻ địch
                if (attacker != null && KTLogic.IsOpposite(this, attacker))
                {
                    /// Đuổi theo mục tiêu vừa tấn công
                    this.chaseTarget = attacker;
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hàm này gọi liên tục khi nào quái còn tồn tại
        /// </summary>
        public void AI_Tick()
        {
            try
            {
                /// Nếu là quái tĩnh thì thôi
                if (this.IsStatic)
                {
                    return;
                }

                /// Nếu là NPC thì thôi
                if (this.MonsterType == MonsterAIType.DynamicNPC)
                {
                    return;
                }

                /// Nếu vị trí ban đầu lỗi thì thiết lập lại vị trí hiện tại
                if (!GameManager.MapMgr.GetGameMap(this.CurrentMapCode).CanMove(this.StartPos))
                {
                    /// Gắn lại vị trí hiện tại
                    this.StartPos = this.CurrentPos;
                }

                /// Nếu quái đã chết
                if (this.IsDead())
                {
                    return;
                }

                /// Nếu phạm vi đuổi quá nhỏ
                if (this.SeekRange < 500)
                {
                    this.SeekRange = 500;
                }

                /// Nếu vị trí hiện tại trùng với vị trí ban đầu
                if (KTGlobal.GetDistanceBetweenPoints(this.CurrentPos, this.StartPos) <= 10)
                {
                    /// Đánh dấu không phải đang quay về vị trí ban đầu
                    this.IsBackingToOriginPos = false;
                    /// Đánh dấu đang đứng
                    this.m_eDoing = KE_NPC_DOING.do_stand;
                }

                /// Nếu đang di chuyển về vị trí ban đầu thì thôi
                if (this.IsBackingToOriginPos)
                {
                    return;
                }

                /// Nếu vị trí hiện tại quá xa so với vị trí ban đầu
                if (KTGlobal.GetDistanceBetweenPoints(this.CurrentPos, this.StartPos) >= this.SeekRange)
                {
                    /// Hủy mục tiêu đang đuổi
                    this.chaseTarget = null;
                    /// Đánh dấu đang quay trở lại vị trí ban đầu
                    this.IsBackingToOriginPos = true;
                    /// Quay trở lại vị trí ban đầu
                    this.NextMoveTo = this.StartPos;
                    /// Bỏ qua
                    return;
                }

                /// Nếu có mục tiêu đuổi nhưng đã chết, hoặc khác bản đồ
                if (this.chaseTarget != null && (this.chaseTarget.IsDead() || this.chaseTarget.CurrentMapCode != this.CurrentMapCode || this.chaseTarget.CurrentCopyMapID != this.CurrentCopyMapID))
                {
                    /// Hủy mục tiêu đang đuổi
                    this.chaseTarget = null;
                    /// Đánh dấu đang quay trở lại vị trí ban đầu
                    this.IsBackingToOriginPos = true;
                    /// Quay trở lại vị trí ban đầu
                    this.NextMoveTo = this.StartPos;
                    /// Bỏ qua
                    return;
                }

                /// Nếu có mục tiêu tàng hình và bản thân không nhìn được
                if (this.chaseTarget != null && this.chaseTarget.IsInvisible() && !this.chaseTarget.VisibleTo(this))
                {
                    /// Hủy mục tiêu đang đuổi
                    this.chaseTarget = null;
                    /// Đánh dấu đang quay trở lại vị trí ban đầu
                    this.IsBackingToOriginPos = true;
                    /// Quay trở lại vị trí ban đầu
                    this.NextMoveTo = this.StartPos;
                    /// Bỏ qua
                    return;
                }

                /// Nếu có mục tiêu đuổi
                if (this.chaseTarget != null && !this.chaseTarget.IsDead() && this.chaseTarget.CurrentMapCode == this.CurrentMapCode && this.chaseTarget.CurrentCopyMapID == this.CurrentCopyMapID)
                {
                    /// Nếu AIScript có tồn tại trong hệ thống
                    if (KTMonsterAIScriptManager.HasAIScript(this.AIID) && !this.IsDead() && this.chaseTarget != null)
                    {
                        /// Thực hiện ScriptAI tương ứng
                        UnityEngine.Vector2 destPos = KTMonsterAIScriptManager.GetAIScript(this.AIID).Process(this, this.chaseTarget);
                        /// Nếu có vị trí cần dịch đến
                        if (destPos != UnityEngine.Vector2.zero)
                        {
                            /// Di chuyển đến chỗ mục tiêu
                            this.MoveTo(new Point(destPos.x, destPos.y));
                        }
                    }
                }
                else
                {
                    /// Nếu loại quái là quái tinh anh hoặc Boss hoặc quái chữ đỏ thì sẽ tìm kiếm mục tiêu xung quanh
                    if (this.MonsterType == MonsterAIType.Elite || this.MonsterType == MonsterAIType.Leader || this.MonsterType == MonsterAIType.Boss || this.MonsterType == MonsterAIType.Pirate || this.MonsterType == MonsterAIType.Hater || this.MonsterType == MonsterAIType.Special_Normal || this.MonsterType == MonsterAIType.Special_Boss)
                    {
                        /// Tìm kẻ địch xung quanh
                        List<GameObject> enemies = KTLogic.GetNearByEnemies(this, this.SeekRange, 1);

                        /// Nếu tìm thấy mục tiêu
                        if (enemies != null && enemies.Count == 1)
                        {
                            this.chaseTarget = enemies[0];
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Di chuyển quái đến vị trí tương ứng
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="isAIRandomMove"></param>
        public void MoveTo(Point pos, bool isAIRandomMove = false)
        {
            /// Nếu quái tĩnh thì thôi
            if (this.IsStatic || this.GetCurrentRunSpeed() <= 0)
			{
                return;
			}
            /// Nếu đang quay trở lại vị trí ban đầu
            else if (this.IsBackingToOriginPos)
            {
                return;
            }

            this.NextMoveTo = pos;
            this.IsAIRandomMove = isAIRandomMove;
        }

        /// <summary>
        /// Di chuyển ngẫu nhiên đến vị trí chỉ định
        /// </summary>
        public void RandomMoveAround()
        {
            /// Nếu quái tĩnh thì thôi
            if (this.IsStatic || this.GetCurrentRunSpeed() <= 0)
            {
                return;
            }
            /// Nếu phạm vi tìm kiếm quá nhỏ
            else if (this.SeekRange <= 0)
            {
                return;
            }
            /// Nếu quái đã chết
            else if (this.IsDead())
            {
                return;
            }
            /// Nếu đang đuổi theo mục tiêu hoặc đang trở về vị trí ban đầu
            else if (this.chaseTarget != null || this.IsBackingToOriginPos)
			{
                return;
			}

            /// Lấy giá trị ngẫu nhiên để thực hiện di chuyển
            int nRand = Global.GetRandomNumber(1, 100);
            /// Nếu có thể di chuyển
            if (nRand <= Monster.AIRandomMoveBelowRate)
            {
                /// Tìm vị trí có thể đi được
                int triedTimes = 0;
                while (true)
                {
                    /// Tăng số lần thử lên 1
                    triedTimes++;

                    /// Vị trí hiện tại
                    Point curPos = this.CurrentPos;

                    /// Random vị trí đích
                    int posX = (int)curPos.X + Global.GetRandomNumber(-this.SeekRange / 2, this.SeekRange / 2);
                    int posY = (int)curPos.Y + Global.GetRandomNumber(-this.SeekRange / 2, this.SeekRange / 2);

                    /// Vị trí đích
                    Point toPos = new Point((int)posX, (int)posY);
                    if (!Global.InObs(ObjectTypes.OT_MONSTER, this.MonsterZoneNode.MapCode, (int)curPos.X, (int)curPos.Y))
                    {
                        this.MoveTo(toPos, true);
                        break;
                    }

                    /// Nếu quá số lần thử thì không thực hiện
                    if (triedTimes > Monster.AIMaxTryFindRandomPosAroundTimes)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Sử dụng kỹ năng
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="level"></param>
        /// <param name="target"></param>
        public bool UseSkill(int skillID, int level, GameObject target)
        {
            if (this.IsDead() || target.IsDead())
            {
                return false;
            }

            /// Nếu chưa đến thời gian dùng kỹ năng
            if (!KTGlobal.FinishedUseSkillAction(this, this.GetCurrentAttackSpeed()))
            {
                return false;
            }

            /// Lấy dữ liệu kỹ năng tương ứng
            SkillDataEx skillData = KSkill.GetSkillData(skillID);
            /// Nếu kỹ năng không tồn tại
            if (skillData == null)
            {
                return false;
            }

            /// Làm mới đối tượng kỹ năng theo cấp
            SkillLevelRef skill = new SkillLevelRef()
            {
                Data = skillData,
                AddedLevel = level,
                BonusLevel = 0,
            };

            /// Thực hiện sử dụng kỹ năng
            KTSkillManager.UseSkillResult result = KTSkillManager.UseSkill(this, target, skill);
            return true;
        }

        /// <summary>
        /// Làm mới đối tượng
        /// </summary>
        public void ResetAI()
        {
            /// Hủy mục tiêu đang đuổi
            this.chaseTarget = null;
        }

        /// <summary>
        /// Thiết lập lại đối tượng
        /// </summary>
        public void Reset()
		{
            /// Nếu đã chết thì thôi
            if (this.IsDead())
            {
                return;
            }

            /// Ngừng StoryBoard
            KTMonsterStoryBoardEx.Instance.Remove(this);

            /// Hủy mục tiêu đang đuổi
            this.chaseTarget = null;
            /// Hủy vị trí tiếp theo cần đến
            this.NextMoveTo = new Point(-1, -1);
            /// Hủy đánh dấu AI tự di chuyển
            this.IsAIRandomMove = false;
            /// Bỏ vị trí đích đến
            this.ToPos = new Point(-1, -1);
            /// Hủy đánh dấu đang tự chạy về vị trí ban đầu
            this.IsBackingToOriginPos = false;
			/// Nếu vị trí ban đầu lỗi thì thiết lập lại vị trí hiện tại
			if (!GameManager.MapMgr.GetGameMap(this.CurrentMapCode).CanMove(this.StartPos))
			{
				/// Gắn lại vị trí hiện tại
				this.StartPos = this.CurrentPos;

                /// Cập nhật vị trí đối tượng vào Map
                GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                if (mapGrid != null)
                {
                    mapGrid.MoveObject(-1, -1, (int) this.CurrentPos.X, (int) this.CurrentPos.Y, this);
                }
            }
			else
			{
				/// Thiết lập vị trí là vị trí ban đầu
				this.CurrentPos = this.StartPos;

                /// Cập nhật vị trí đối tượng vào Map
                GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                if (mapGrid != null)
                {
                    mapGrid.MoveObject(-1, -1, (int) this.CurrentPos.X, (int) this.CurrentPos.Y, this);
                }
            }
            /// Thiết lập đang đứng
            this.m_eDoing = KE_NPC_DOING.do_stand;
		}
        #endregion
    }
}
