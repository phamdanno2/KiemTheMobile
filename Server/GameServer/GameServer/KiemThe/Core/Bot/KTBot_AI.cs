using GameServer.KiemThe;
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
    public partial class KTBot
    {
        #region Config
        /// <summary>
        /// Số lần thử tìm vị trí xung quanh đi được tối đa
        /// </summary>
        private const int AIMaxTryFindRandomPosAroundTimes = 10;

        /// <summary>
        /// Tỷ lệ % để thực hiện di chuyển
        /// </summary>
        private const int AIRandomMoveBelowRate = 100;
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
        #endregion

        #region Public methods
        /// <summary>
        /// Hàm này gọi liên tục khi nào quái còn tồn tại
        /// </summary>
        public void AI_Tick()
        {
            /// Nếu chưa kết thúc thi triển kỹ năng
            if (!KTGlobal.FinishedUseSkillAction(this, this.GetCurrentAttackSpeed()))
            {
                return;
            }

            /// Nếu đang đuổi theo mục tiêu
            if (this.IsChasingTarget())
            {
                /// Nếu AIScript có tồn tại trong hệ thống
                if (KTLuaScript.Instance.GetScriptByID(this.AIID) != null && !this.IsDead())
                {
                    /// ID bản đồ hiện tại
                    GameManager.MapMgr.DictMaps.TryGetValue(this.CurrentMapCode, out GameMap map);

                    /// Thực hiện ScriptLua tương ứng
                    KTLuaEnvironment.ExecuteBotAIScript_AITick(map, this, this.chaseTarget, this.AIID, (destPos) => {
                        /// Nếu không có mục tiêu
                        if (this.chaseTarget == null)
                        {
                            return;
                        }

                        /// Nếu có vị trí cần dịch đến
                        if (destPos.HasValue)
                        {
                            /// Di chuyển đến chỗ mục tiêu
                            this.MoveTo(new Point(destPos.Value.x, destPos.Value.y));
                        }
                    });
                }

                return;
            }
        }

        /// <summary>
        /// Di chuyển quái đến vị trí tương ứng
        /// </summary>
        /// <param name="pos"></param>
        public void MoveTo(Point pos)
        {
            /// Nếu quái đã chết
            if (this.IsDead())
            {
                return;
            }

            /// Vị trí hiện tại
            Point curPos = this.CurrentPos;

            /// Nếu không nằm ở vị trí có vật cản thì tiến hành tìm đường
            if (!Global.InObs(ObjectTypes.OT_MONSTER, this.CurrentMapCode, (int) curPos.X, (int) curPos.Y, 1))
            {
                /// Nếu tồn tại StoryBoard cũ thì xóa
                KTOtherObjectStoryBoard.Instance.Remove(this);

                /// Bắt đầu StoryBoard mới
                KTOtherObjectStoryBoard.Instance.Add(this, curPos, pos);
            }
        }

        /// <summary>
        /// Di chuyển ngẫu nhiên đến vị trí chỉ định
        /// </summary>
        public void RandomMoveAround()
        {
            /// Nếu quái đã chết
            if (this.IsDead())
            {
                return;
            }

            /// Nếu đang có mục tiêu bám theo
            if (this.IsChasingTarget())
            {
                return;
            }

            /// Nếu phạm vi tìm kiếm quá nhỏ
            if (this.SeekRange <= 0)
            {
                return;
            }

            /// Lấy giá trị ngẫu nhiên để thực hiện di chuyển
            int nRand = Global.GetRandomNumber(1, 100);
            /// Nếu có thể di chuyển
            if (nRand <= KTBot.AIRandomMoveBelowRate)
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
                    int posX = (int) curPos.X + Global.GetRandomNumber(-this.SeekRange, this.SeekRange);
                    int posY = (int) curPos.Y + Global.GetRandomNumber(-this.SeekRange, this.SeekRange);

                    /// Vị trí đích
                    Point toPos = new Point((int) posX, (int) posY);
                    if (!Global.InObs(ObjectTypes.OT_MONSTER, this.CurrentMapCode, (int) curPos.X, (int) curPos.Y, 1))
                    {
                        this.MoveTo(toPos);
                        break;
                    }

                    /// Nếu quá số lần thử thì không thực hiện
                    if (triedTimes > KTBot.AIMaxTryFindRandomPosAroundTimes)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Nhận sát thương
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damageTaken"></param>
        public override void TakeDamage(GameObject attacker, int damageTaken)
        {
            base.TakeDamage(attacker, damageTaken);

            /// Nếu không có attacker
            if (attacker == null)
            {
                return;
            }

            /// Nếu đang đuổi theo mục tiêu
            if (this.IsChasingTarget())
            {
                return;
            }

            /// Nếu là kẻ địch
            if (KTLogic.IsOpposite(this, attacker))
            {
                /// Đuổi theo mục tiêu vừa tấn công
                this.BeginChaseTarget(attacker);
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
            };

            /// Thực hiện sử dụng kỹ năng
            KTSkillManager.UseSkillResult result = KTSkillManager.UseSkill(this, target, skill);
            //Console.WriteLine("Monster use skill ID = {0}, Result = {1}", skillID, result);
            return true;
        }

        /// <summary>
        /// Làm mới đối tượng
        /// </summary>
        public void ResetAI()
        {
            this.StopChaseTarget();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Có đang đuổi theo mục tiêu không
        /// </summary>
        /// <returns></returns>
        private bool IsChasingTarget()
        {
            /// Nếu không có mục tiêu
            if (this.chaseTarget == null || ((this.chaseTarget.IsPlayer() && (this.chaseTarget as KPlayer).LogoutState)) || (this.chaseTarget.CurrentMapCode != this.CurrentMapCode))
            {
                /// Quay trở lại vị trí ban đầu
                this.MoveTo(this.StartPos);
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();
                return false;
            }

            /// Nếu mục tiêu đuổi theo đã tử vong
            if (this.chaseTarget.IsDead())
            {
                /// Quay trở lại vị trí ban đầu
                this.MoveTo(this.StartPos);
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();

                return false;
            }

            /// Nếu mục tiêu đuổi theo đang trong trạng thái tàng hình và bản thân không thể nhìn thấy mục tiêu
            if (this.chaseTarget.IsInvisible() && !this.chaseTarget.VisibleTo(this))
            {
                /// Quay trở lại vị trí ban đầu
                this.MoveTo(this.StartPos);
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();

                return false;
            }

            /// Vị trí lúc bắt đầu đuổi mục tiêu
            UnityEngine.Vector2 startPos = new UnityEngine.Vector2((float) this.StartPos.X, (float) this.StartPos.Y);
            /// Vị trí hiện tại của bản thân
            UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((float) this.CurrentPos.X, (float) this.CurrentPos.Y);
            /// Vị trí hiện tại của mục tiêu
            UnityEngine.Vector2 targetPos = new UnityEngine.Vector2((float) this.chaseTarget.CurrentPos.X, (float) this.chaseTarget.CurrentPos.Y);

            /// Nếu khoảng cách từ vị trí ban đầu đến vị trí hiện tại quá lớn
            if (UnityEngine.Vector2.Distance(startPos, currentPos) > this.SeekRange)
            {
                /// Quay trở lại vị trí ban đầu
                this.MoveTo(this.StartPos);
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();

                return false;
            }

            /// Nếu khoảng cách từ vị trí hiện tại đến mục tiêu vượt quá phạm vi đuổi
            if (UnityEngine.Vector2.Distance(currentPos, targetPos) > this.SeekRange)
            {
                /// Quay trở lại vị trí ban đầu
                this.MoveTo(this.StartPos);
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Bắt đầu đuổi theo mục tiêp
        /// </summary>
        /// <param name="target"></param>
        private void BeginChaseTarget(GameObject target)
        {
            if (this.IsDead() || target.IsDead())
            {
                /// Hủy mục tiêu đang đuổi
                this.StopChaseTarget();
                return;
            }

            this.chaseTarget = target;
        }

        /// <summary>
        /// Ngừng đuổi theo mục tiêu
        /// </summary>
        private void StopChaseTarget()
        {
            this.chaseTarget = null;
        }
        #endregion
    }
}
