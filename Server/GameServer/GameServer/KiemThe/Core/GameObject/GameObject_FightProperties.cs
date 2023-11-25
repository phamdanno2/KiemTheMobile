using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.KiemThe;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.GameEvents.BaiHuTang;
using GameServer.KiemThe.GameEvents.EmperorTomb;
using GameServer.KiemThe.GameEvents.FactionBattle;
using GameServer.KiemThe.GameEvents.SpecialEvent;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager.Battle;
using GameServer.KiemThe.Logic.Manager.Skill.PoisonTimer;
using GameServer.KiemThe.Utilities;
using GameServer.Server;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý thuộc tính chiến đấu của đối tượng
    /// </summary>
    public partial class GameObject
    {
        #region Define

        /// <summary>
        /// Danh sách kỹ năng tự động kích hoạt
        /// </summary>
        private readonly ConcurrentDictionary<int, KNpcAutoSkill> listAutoSkills = new ConcurrentDictionary<int, KNpcAutoSkill>();

        private long _LastPlayBulletExplodeEffectTicks = 0;

        private long _LastNotifyAttackEffectTicks = 0;

        /// <summary>
        /// Thời điểm lần trước thực hiện hiệu ứng đạn nổ
        /// </summary>
        ///
        public long LastNotifyAttackTicks
        {
            get
            {
                return this._LastNotifyAttackEffectTicks;
            }
            set
            {
                this._LastNotifyAttackEffectTicks = value;
            }
        }

        public long LastPlayBulletExplodeEffectTicks
        {
            get
            {
                return this._LastPlayBulletExplodeEffectTicks;
            }
            set
            {
                this._LastPlayBulletExplodeEffectTicks = value;
            }
        }

        /// <summary>
        /// Toàn bộ công kích ngũ hành của đối tượng
        /// </summary>
        public KNpcAttribGroup_Damage[] m_damage { get; private set; } = new KNpcAttribGroup_Damage[(int)DAMAGE_TYPE.damage_num]
        {
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
            new KNpcAttribGroup_Damage(),
        };

        /// <summary>
        /// Toàn bộ hiệu ứng của đối tượng
        /// </summary>
        public KNpcAttribGroup_State[] m_state { get; private set; } = new KNpcAttribGroup_State[(int)KE_STATE.emSTATE_ALLNUM]
        {
            new KNpcAttribGroup_State(KE_STATE.emSTATE_BEGIN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SERISE_BEGIN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_HURT),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_WEAK),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SLOWALL),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_BURN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_STUN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SERISE_END),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SPECIAL_BEGIN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_FIXED),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_PALSY),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SLOWRUN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_FREEZE),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_CONFUSE),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_KNOCK),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_DRAG),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SILENCE),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_ZHICAN),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_FLOAT),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SPECIAL_END),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_NUM),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_POISON),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_HIDE),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SHIELD),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_SUDDENDEATH),
            new KNpcAttribGroup_State(KE_STATE.emSTATE_IGNORETRAP),
        };

        /// <summary>
        /// Thời điểm lần cuối dùng kỹ năng khinh công
        /// </summary>
        public long LastUseFlyingTicks { get; set; } = 0;

        /// <summary>
        /// Thời gian trúng độc
        /// </summary>
        public int m_nPoisonDuration { get; set; }

        /// <summary>
        /// Sát thương độc
        /// </summary>
        public int m_nPoisonDamage { get; set; }

        /// <summary>
        /// Cấp độ hiệu ứng ẩn thân hiện tại
        /// <para>-1 nghĩa là không tồn tại hiệu ứng</para>
        /// </summary>
        public int m_InvisibleLevel { get; set; } = -1;

        /// <summary>
        /// Loại ẩn thân
        /// <para>-1: Không mất</para>
        /// <para>0: Mất nếu dùng kỹ năng hoặc chết</para>
        /// <para>1: Mất nếu dùng kỹ năng hoặc chết hoặc chịu sát thương</para>
        /// </summary>
        public int m_InvisibleType { get; set; } = 0;

        /// <summary>
        /// Số Frame duy trì hiệu ứng ẩn thân
        /// </summary>
        public int m_InvisibleFrameCount { get; set; } = 0;

        /// <summary>
        /// Danh sách kỹ năng sử dụng không làm mất trạng thái ẩn thân
        /// </summary>
        public HashSet<int> m_InvisibleNoLostOnUseSkills { get; private set; } = new HashSet<int>();

        /// <summary>
        /// Thời điểm lần trước sử dụng kỹ năng làm mất trạng thái tàng hình
        /// </summary>
        public long m_LastUseSkillCauseLosingInvisibleTick { get; set; } = 0;

        /// <summary>
        /// Tính thêm dame vào tầm xa
        /// </summary>
        public KNPC_RDCLIFEWITHDIS m_sRdcLifeWithDis { get; private set; } = new KNPC_RDCLIFEWITHDIS();

        /// <summary>
        /// Trong trạng thái bảo vệ
        /// </summary>
        public bool m_bProtected { get; set; }

        /// <summary>
        /// Bỏ qua tấn công của đối thủ mỗi khoảng
        /// <para>Value[0]: Giá trị</para>
        /// <para>Value[1]: Chưa rõ</para>
        /// <para>Value[2]: Vô nghĩa</para>
        /// </summary>
        public KNPC_IGNOREATTACK m_sIgnoreAttack { get; set; } = new KNPC_IGNOREATTACK();

        /// <summary>
        /// Tỷ lệ bỏ qua kháng ngũ hành tương ứng
        /// <para>Value[0]: Giá trị Min</para>
        /// <para>Value[1]: Giá trị Max</para>
        /// <para>Value[2]: Ngũ hành</para>
        /// </summary>
        public Dictionary<int, KNPC_IGNORERESIST> m_sIgnoreResists { get; set; } = new Dictionary<int, KNPC_IGNORERESIST>()
        {
            { (int) KE_SERIES_TYPE.series_metal, new KNPC_IGNORERESIST() { nIgnoreResistPMin = 0, nIgnoreResistPMax = 0 } },
            { (int) KE_SERIES_TYPE.series_wood, new KNPC_IGNORERESIST() { nIgnoreResistPMin = 0, nIgnoreResistPMax = 0 } },
            { (int) KE_SERIES_TYPE.series_water, new KNPC_IGNORERESIST() { nIgnoreResistPMin = 0, nIgnoreResistPMax = 0} },
            { (int) KE_SERIES_TYPE.series_fire, new KNPC_IGNORERESIST() { nIgnoreResistPMin = 0, nIgnoreResistPMax = 0 } },
            { (int) KE_SERIES_TYPE.series_earth, new KNPC_IGNORERESIST() { nIgnoreResistPMin = 0, nIgnoreResistPMax = 0 } },
        };

        /// <summary>
        /// Thời gian sát thương thêm ngũ hành tương đương ngũ hành hiện tại
        /// </summary>
        public int m_nAddedSeriesStateRate { get; set; }

        /// <summary>
        /// Hiển thị toàn bộ bẫy
        /// </summary>
        public bool m_DetectTrap { get; set; }

        /// <summary>
        /// Thời gian tấn công  gần đây
        /// </summary>
        private long _LastAttackTicks = 0;

        /// <summary>
        /// Thời gian tấn công gần đây nhất
        /// </summary>
        public long LastAttackTicks
        {
            get
            {
                return this._LastAttackTicks;
            }
            set
            {
                this._LastAttackTicks = value;
            }
        }

        /// <summary>
        /// Thời điểm dùng kỹ năng không ảnh hưởng tốc đánh lần trước
        /// </summary>
        public long LastUseSkillNoAffectAtkSpeedTick { get; set; }

        /// <summary>
        /// Thời điểm thực hiện khinh công lần trước
        /// </summary>
        public long LastBlinkTicks { get; set; }

        #endregion Define

        #region Public methods

        /// <summary>
        /// Có thể sử dụng kỹ năng hoặc làm các thao tác tương tự không
        /// </summary>
        /// <returns></returns>
        public bool IsCanDoLogic()
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }
            if (this.HaveState(KE_STATE.emSTATE_HURT) || this.HaveState(KE_STATE.emSTATE_STUN) || this.HaveState(KE_STATE.emSTATE_FREEZE) || this.HaveState(KE_STATE.emSTATE_PALSY) || this.HaveState(KE_STATE.emSTATE_CONFUSE) || this.HaveState(KE_STATE.emSTATE_DRAG) || this.HaveState(KE_STATE.emSTATE_KNOCK) || this.HaveState(KE_STATE.emSTATE_FLOAT))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Có thể di chuyển không
        /// </summary>
        /// <returns></returns>
        public bool IsCanMove()
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }
            if (this.HaveState(KE_STATE.emSTATE_HURT) || this.HaveState(KE_STATE.emSTATE_STUN) || this.HaveState(KE_STATE.emSTATE_FREEZE) || this.HaveState(KE_STATE.emSTATE_PALSY) || this.HaveState(KE_STATE.emSTATE_DRAG) || this.HaveState(KE_STATE.emSTATE_KNOCK) || this.HaveState(KE_STATE.emSTATE_FIXED) || this.HaveState(KE_STATE.emSTATE_FLOAT))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Có thể chủ động di chuyển không
        /// </summary>
        /// <returns></returns>
        public bool IsCanPositiveMove()
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }
            if (this.HaveState(KE_STATE.emSTATE_HURT) || this.HaveState(KE_STATE.emSTATE_STUN) || this.HaveState(KE_STATE.emSTATE_FREEZE) || this.HaveState(KE_STATE.emSTATE_PALSY) || this.HaveState(KE_STATE.emSTATE_CONFUSE) || this.HaveState(KE_STATE.emSTATE_DRAG) || this.HaveState(KE_STATE.emSTATE_KNOCK) || this.HaveState(KE_STATE.emSTATE_FIXED) || this.HaveState(KE_STATE.emSTATE_FLOAT))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Miễn nhiễm mọi sát thương và trạng thái
        /// </summary>
        /// <returns></returns>
        public bool IsImmuneToAllDamageAndStates()
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }
            return this.HaveState(KE_STATE.emSTATE_FLOAT) || this.HaveState(KE_STATE.emSTATE_FREEZE);
        }

        /// <summary>
        /// Thiết lập trạng thái bảo vệ
        /// </summary>
        /// <param name="bProtected"></param>
        /// <returns></returns>
        public bool SetProtected(bool bProtected)
        {
            if (this.m_bProtected == bProtected)
            {
                return true;
            }

            this.m_bProtected = bProtected;

            /// GỬI PACKET VỀ CLIENT

            return true;
        }

        #region Trạng thái ngũ hành

        /// <summary>
        /// Kiểm tra có trạng thái ngũ hành tương ứng không
        /// </summary>
        /// <param name="eState"></param>
        /// <returns></returns>
        public bool HaveState(KE_STATE eState)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }

            if (eState < KE_STATE.emSTATE_BEGIN || eState >= KE_STATE.emSTATE_ALLNUM)
            {
                return false;
            }

            return !this.m_state[(int)eState].IsOver;
        }

        /// <summary>
        /// Xóa trạng thái ngũ hành
        /// </summary>
        /// <param name="bSyncClient"></param>
        public void ClearSpecialState(bool bSyncClient)
        {
            for (int i = (int)KE_STATE.emSTATE_BEGIN; i < (int)KE_STATE.emSTATE_ALLNUM; ++i)
            {
                this.RemoveSpecialState((KE_STATE)i, bSyncClient);
            }
        }

        /// <summary>
        /// Xóa trạng thái ngũ hành tương ứng
        /// </summary>
        /// <param name="eState"></param>
        /// <param name="bSyncClient"></param>
        public bool RemoveSpecialState(KE_STATE eState, bool bSyncClient)
        {
            try
            {
                /// Nếu không có hiệu ứng này thì thôi
                if (this.m_state[(int)eState].Duration <= 0 || this.m_state[(int)eState].StartTick <= 0)
                {
                    return false;
                }
                /// Reset lại thời gian hiệu ứng
                this.m_state[(int)eState].ClearState();

                if (bSyncClient)
                {
                    /// ĐỒNG BỘ VỀ CLIENT
                    KT_TCPHandler.NotifySpriteSeriesState(this, eState, 0);
                }

                /// Lấy thông tin trạng thái tương ứng
                KSpecialState kState = KSpecialStateManager.g_arSpecialState.KSpecialState.Where(x => x.StateID == eState.ToString()).FirstOrDefault();
                if (kState != null)
                {
                    this.Buffs.RemoveBuff(kState.SkillID);
                }

                /// Trạng thái đặc biệt gì
                if (eState == KE_STATE.emSTATE_SLOWALL)
                {
                    this._m_RunSpeedReduceP = 0;
                    this.SyncMoveSpeed();
                    this._m_AttackSpeedReduceP = 0;
                    this.SynsAttackSpeed();
                }
                else if (eState == KE_STATE.emSTATE_SLOWRUN)
                {
                    this._m_RunSpeedReduceP = 0;
                    this.SyncMoveSpeed();
                }
                else if (eState == KE_STATE.emSTATE_POISON)
                {
                    this.m_nLastPoisonDamageIdx = null;

                    /// Xóa đối tượng khỏi Timer trúng độc
                    KTPoisonTimerManager.Instance.RemovePoisonState(this);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Thêm trạng thái ngũ hành cho đối tượng
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="skillPos"></param>
        /// <param name="eState"></param>
        /// <param name="nTime"></param>
        /// <param name="nParam"></param>
        /// <param name="bSyncClient"></param>
        /// <returns></returns>
        public bool AddSpecialState(GameObject attacker, UnityEngine.Vector2 skillPos, KE_STATE eState, int nTime, int nParam, bool bSyncClient)
        {
            /// Nếu không chịu ảnh hưởng bởi trạng thái bất lợi
            if (this.m_IgnoreAllSeriesStates/* && eState != KE_STATE.emSTATE_POISON*/)
            {
                return false;
            }

            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }

            /// Nếu đang bất tử thì thôi
            if (this.m_CurrentInvincibility == 1 || (this is KPlayer player && player.GM_Immortality))
            {
                return false;
            }

            try
            {
                /// Nếu đã có trạng thái này
                if (this.HaveState(eState))
                {
                    return false;
                }

                /// Thông tin trạng thái tương ứng
                KNpcAttribGroup_State state = this.m_state[(int)eState];

                /// Lấy thông tin trạng thái tương ứng
                KSpecialState kState = KSpecialStateManager.g_arSpecialState.KSpecialState.Where(x => x.StateID == Enum.GetName(eState.GetType(), eState)).FirstOrDefault();
                if (kState != null)
                {
                    SkillDataEx skill = KSkill.GetSkillData(kState.SkillID);
                    if (skill != null)
                    {
                        SkillLevelRef skillRef = new SkillLevelRef()
                        {
                            Data = skill,
                            AddedLevel = 1,
                            BonusLevel = 0,
                            CanStudy = false,
                        };
                        /// Thực hiện tạo Buff với trạng thái ngũ hành tương ứng
                        BuffDataEx seriesEffect = new BuffDataEx()
                        {
                            LoseWhenUsingSkill = false,
                            SaveToDB = false,
                            StartTick = KTGlobal.GetCurrentTimeMilis(),
                            Duration = nTime * 1000 / 18,
                            Skill = skillRef,
                        };
                        this.Buffs.AddBuff(seriesEffect);
                    }
                }

                /// Giảm thời gian xuống 1 chút để Client không bị Rollback
                int gsTime = nTime - 9;
                if (gsTime <= 0)
                {
                    gsTime = 3;
                }

                /// Thực hiện biểu diễn trạng thái ngũ hành đặc biệt
                UnityEngine.Vector2 dragPos = default;
                switch (eState)
                {
                    case KE_STATE.emSTATE_DRAG:
                        {
                            dragPos = this.DoDrag(attacker, skillPos, gsTime, nParam);
                            break;
                        }
                    case KE_STATE.emSTATE_KNOCK:
                        {
                            dragPos = this.DoKnockback(attacker, skillPos, gsTime, nParam);
                            break;
                        }
                    case KE_STATE.emSTATE_CONFUSE:
                        {
                            this.DoConfuse(attacker, nTime);
                            break;
                        }
                    case KE_STATE.emSTATE_SLOWALL:
                        {
                            this._m_RunSpeedReduceP = 50;
                            this.SyncMoveSpeed();
                            this._m_AttackSpeedReduceP = 50;
                            this.SynsAttackSpeed();
                            break;
                        }
                    case KE_STATE.emSTATE_SLOWRUN:
                        {
                            this._m_RunSpeedReduceP = 50;
                            this.SyncMoveSpeed();
                            break;
                        }
                }

                /// Thêm trạng thái vào
                this.m_state[(int)eState].StartTick = KTGlobal.GetCurrentTimeMilis();
                this.m_state[(int)eState].Duration = gsTime;
                this.m_state[(int)eState].OtherParam = nParam;

                /// Nếu không thể di chuyển
                if (!this.IsCanMove())
                {
                    if (this is KPlayer)
                    {
                        KTPlayerStoryBoardEx.Instance.Remove(this as KPlayer);
                    }
                    else if (this is Monster)
                    {
                        KTMonsterStoryBoardEx.Instance.Remove(this as Monster);
                    }
                }

                /// Nếu có đánh dấu gửi trạng thái về Client
                if (bSyncClient)
                {
                    KT_TCPHandler.NotifySpriteSeriesState(this, eState, 1, nTime / 18f, (int)dragPos.x, (int)dragPos.y);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
            return false;
        }

        #endregion Trạng thái ngũ hành

        /// <summary>
        /// Thêm trạng thái khiên đỡ sát thương
        /// </summary>
        /// <param name="nDamage"></param>
        /// <param name="nTime"></param>
        public void AddShieldState(int nDamage, int nTime)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return;
            }

            if (nDamage <= 0 || nTime <= 0)
            {
                return;
            }

            /// Thêm trạng thái tương ứng
            if (!this.AddSpecialState(this, default, KE_STATE.emSTATE_SHIELD, nTime, nDamage, true))
            {
                this.m_state[(int)KE_STATE.emSTATE_SHIELD].OtherParam = Math.Max(this.m_state[(int)KE_STATE.emSTATE_SHIELD].OtherParam, nDamage);
                this.m_state[(int)KE_STATE.emSTATE_SHIELD].Duration = Math.Max(this.m_state[(int)KE_STATE.emSTATE_SHIELD].Duration, nTime);
            }
        }

        /// <summary>
        /// Thêm trạng thái trúng độc
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="nDamage"></param>
        /// <param name="nTime"></param>
        public void AddPoisonState(GameObject attacker, int nDamage, int nTime)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return;
            }

            /// Nếu không có damage
            if (nDamage < 0)
            {
                return;
            }

            /// Đoạn này giảm thời gian trúng độc
            if (m_CurrentPoisonTimeReducePercent > 0)
            {
                if (m_CurrentPoisonTimeReducePercent >= 75)
                {
                    nTime /= 4;
                }
                else
                {
                    nTime = nTime * (100 - m_CurrentPoisonTimeReducePercent) / 100;
                }
            }

            /// Nếu thời gian < 0
            if (nTime < 0)
            {
                return;
            }

            if (!this.AddSpecialState(attacker, default, KE_STATE.emSTATE_POISON, nTime, nDamage, true)) // ADd trạng thái trúng độc cho thằng bị tấn công
            {
                int d = m_state[(int)KE_STATE.emSTATE_POISON].OtherParam;
                int t = m_state[(int)KE_STATE.emSTATE_POISON].Duration;
                if (d > 0 && nDamage > 0)
                {
                    m_state[(int)KE_STATE.emSTATE_POISON].OtherParam = d + nDamage;
                    m_state[(int)KE_STATE.emSTATE_POISON].Duration = (t * d + nTime * nDamage) / (d + nDamage);
                }
            }

            this.m_nLastPoisonDamageIdx = attacker;

            /// Thêm đối tượng vào Timer trúng độc
            KTPoisonTimerManager.Instance.AddPoisonState(this);
        }

        /// <summary>
        /// Thiết lập trạng thái hiện tại
        /// </summary>
        /// <param name="eDoing"></param>
        public void SetDoing(KE_NPC_DOING eDoing)
        {
            if (eDoing == KE_NPC_DOING.do_jump && m_eDoing == KE_NPC_DOING.do_jumpattack)
                return;
            m_eDoing = eDoing;
        }

        #region Trạng thái ẩn thân

        /// <summary>
        /// Đang trong trạng thái tàng hình
        /// </summary>
        /// <returns></returns>
        public bool IsInvisible()
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }

            return (this is KPlayer player && player.GM_Invisiblity) || this.m_InvisibleLevel != -1;
        }

        /// <summary>
        /// Xóa trạng thái tàng hình
        /// </summary>
        public void RemoveInvisibleState()
        {
            this.m_InvisibleLevel = -1;
            this.m_InvisibleFrameCount = 0;
            this.m_InvisibleType = 0;

            /// Nếu có GM tàng hình thì thôi
            if (this.IsInvisible())
            {
                return;
            }

            ///// Cập nhật tình hình các đối tượng trong danh sách hiển thị xung quanh
            //List<KPlayer> playersAround = KTLogic.GetNearByObjectsAtPos<KPlayer>(this.CurrentMapCode, this.CurrentCopyMapID, new UnityEngine.Vector2((int) this.CurrentPos.X, (int) this.CurrentPos.Y), 1000);
            //foreach (KPlayer player in playersAround)
            //{
            //    Global.GameClientMoveGrid(player);
            //}
            /// Gửi thông báo về Client đối tượng mất trạng thái ẩn thân
            KT_TCPHandler.SendObjectInvisibleState(this, 0);
            /// Thực hiện sự kiện khi mất trạng thái tàng hình
            this.OnExitInvisibleState();
        }

        /// <summary>
        /// Thêm trạng thái tàng hình
        /// </summary>
        /// <param name="level"></param>
        /// <param name="frameCount"></param>
        /// <param name="type"></param>
        public void AddInvisibleState(int level, int frameCount, int type)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return;
            }

            this.m_InvisibleLevel = level;
            this.m_InvisibleFrameCount = frameCount;
            this.m_InvisibleType = type;
            ///// Cập nhật tình hình các đối tượng trong danh sách hiển thị xung quanh
            //List<KPlayer> playersAround = KTLogic.GetNearByObjectsAtPos<KPlayer>(this.CurrentMapCode, this.CurrentCopyMapID, new UnityEngine.Vector2((int) this.CurrentPos.X, (int) this.CurrentPos.Y), 1000);
            //foreach (KPlayer player in playersAround)
            //{
            //    Global.GameClientMoveGrid(player);
            //}
            /// Gửi thông báo về Client đối tượng vào trạng thái ẩn thân
            KT_TCPHandler.SendObjectInvisibleState(this, 1);
            /// Thực hiện sự kiện khi đối tượng bắt đầu ẩn thân
            this.OnEnterInvisibleState();
        }

        /// <summary>
        /// Kiểm tra đối tượng có thể nhìn thấy bản thân không
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool VisibleTo(GameObject go)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return false;
            }

            /// Nếu là chính đối tượng
            if (go == this)
            {
                return true;
            }
            /// Nếu là GM
            else if (this is KPlayer player && player.GM_Invisiblity)
            {
                //return go is KPlayer _player && KTGMCommandManager.IsGM(_player);
                return false;
            }
            /// Nếu cấp độ của đối tượng hiện tại dưới cấp độ của đối tượng tương ứng
            else if (this.m_Level < go.m_Level - KTGlobal.DiffLevelToSeeInvisibleState)
            {
                return true;
            }
            /// Nếu đối tượng có trạng thái phát hiện tàng hình và cấp độ thỏa mãn
            else if (go.m_CurrentShowHide && go.m_Level >= this.m_Level - KTGlobal.DiffLevelToSeeInvisibleState)
            {
                return true;
            }
            else
            {
                /// Nếu là người chơi
                if (this is KPlayer)
                {
                    KPlayer thisPlayer = this as KPlayer;
                    /// Nếu đối tượng là người chơi
                    if (go is KPlayer)
                    {
                        KPlayer goPlayer = go as KPlayer;
                        /// Nếu trong cùng nhóm
                        if (KTLogic.IsTeamMate(goPlayer, thisPlayer))
                        {
                            /// Nếu đang trong trạng thái tỷ thí
                            if (goPlayer.IsChallengeWith(thisPlayer))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion Trạng thái ẩn thân

        #region Chết

        /// <summary>
        /// Tạm khóa đối tượng tránh BUG chết nhiều lần
        /// </summary>
        private object DeadMutex = new object();

        /// <summary>
        /// Thực hiện cho đối tượng chết
        /// </summary>
        /// <param name="nAttacker"></param>
        public void DoDeath(GameObject nAttacker)
        {
            //lock (this.DeadMutex)
            {
                try
                {
                    /// Thiết lập mức máu
                    this.m_CurrentLife = 0;

                    /// Nếu đang trong trạng thái chết thì thôi
                    if (this.m_eDoing == KE_NPC_DOING.do_death)
                    {
                        return;
                    }

                    /// Xóa toàn bộ trạng thái ngũ hành
                    this.ClearSpecialState(true);

                    /// Hủy trạng thái trúng độc
                    this.m_nLastPoisonDamageIdx = null;
                    this.RemoveSpecialState(KE_STATE.emSTATE_POISON, true);

                    /// Nếu cả 2 cùng là người chơi
                    if (this is KPlayer && nAttacker is KPlayer)
                    {
                        KPlayer thisPlayer = this as KPlayer;
                        KPlayer attackerPlayer = nAttacker as KPlayer;

                        /// Nếu đang tỷ thí với nhau
                        if (thisPlayer.IsChallengeTime() && thisPlayer.IsChallengeWith(attackerPlayer))
                        {
                            /// Ngừng đồ sát
                            attackerPlayer.StopActiveFight(thisPlayer);

                            /// Kết thúc tỷ thí
                            thisPlayer.StopChallenge(attackerPlayer);

                            /// Xóa trạng thái trúng độc
                            attackerPlayer.m_nLastPoisonDamageIdx = null;
                            attackerPlayer.RemoveSpecialState(KE_STATE.emSTATE_POISON, true);

                            /// Xóa toàn bộ trạng thái ngũ hành cũ
                            for (int i = (int)KE_STATE.emSTATE_BEGIN; i < (int)KE_STATE.emSTATE_ALLNUM; ++i)
                            {
                                if (this.HaveState((KE_STATE)i))
                                {
                                    /// Xóa hiệu ứng tương ứng khỏi nhân vật
                                    this.RemoveSpecialState((KE_STATE)i, true);
                                }
                            }

                            /// Miễn nhiễm sát thương của đối phương trong 2s
                            thisPlayer.AddImmuneToAllDamagesOf(attackerPlayer, KTGlobal.ChallengeImmuneToEachOtherDamagesTick);
                            attackerPlayer.AddImmuneToAllDamagesOf(thisPlayer, KTGlobal.ChallengeImmuneToEachOtherDamagesTick);

                            /// Thiết lập sinh lực, nội lực, thể lực về 20%
                            this.m_CurrentLife = this.m_CurrentLifeMax * KTGlobal.ChallengeFinishHPMPStaminaReplenishPercent / 100;
                            //this.m_CurrentMana = this.m_CurrentManaMax * KTGlobal.ChallengeFinishHPMPStaminaReplenishPercent / 100;
                            //this.m_CurrentStamina = this.m_CurrentStaminaMax * KTGlobal.ChallengeFinishHPMPStaminaReplenishPercent / 100;
                            /// Đồng bộ thông tin sinh lực, nội lực, thể lực về Client
                            this.ProcessSynsHPMPStaminaToClient();

                            /// Thoát hàm không xử lý nữa
                            return;
                        }
                    }

                    /// Nếu đang tàng hình
                    if (this.IsInvisible() && this.m_InvisibleType >= 0)
                    {
                        this.RemoveInvisibleState();
                    }

                    /// Thực thi động tác chết
                    this.SetDoing(KE_NPC_DOING.do_death);

                    /// Nếu là quái thì thực hiện hàm OnDeath
                    if (this is Monster)
                    {
                        Monster monster = this as Monster;
                        GameManager.MonsterMgr.AddDelayDeadMonster(monster);

                        /// Xử lý các sự kiện nếu Người giết quái
                        if (nAttacker is KPlayer)
                        {
                            /// Thực hiện cho DROP Item
                            ItemDropManager.GetDropMonsterDie(monster, nAttacker as KPlayer);

                            /// Thực hiện Add EXP, Set Quest State.........
                            MonsterDeadHelper.ProcessMonsterDeadByClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, nAttacker as KPlayer, this as Monster);

                            /// Kích hoạt sự kiện Người giết quái
                            GlobalEventSource.getInstance().fireEvent(new MonsterDeadEventObject(monster, nAttacker as KPlayer));

                            /// Thực thi hàm CallBack khi quái chết ở sự kiện đặc biệt
                            SpecialEvent_Logic.ProcessMonsterDie(nAttacker, monster);
                        }
                    }

                    /// Xử lý các sự kiện người giết người
                    if (this.IsPlayer())
                    {
                        /// Bản đồ hiện tại
                        GameMap gameMap = GameManager.MapMgr.DictMaps[this.CurrentMapCode];
                        /// Đối tượng người chơi
                        KPlayer player = this as KPlayer;
                        /// Cập nhật thời điểm chết
                        player.LastDeadTicks = KTGlobal.GetCurrentTimeMilis();

                        /// Thông báo
                        PlayerManager.ShowNotification(player, string.Format("<color=red>Bạn đã bị <color=yellow>{0}</color> đánh trọng thương.", nAttacker.RoleName));
                        /// Thông báo mở bảng hồi sinh về Client
                        string message = string.Format("Địa điểm trọng thương: <color=green>{0}</color>, vị trí <color=#42efff>({1}, {2})</color>.\nĐối tượng đả thương: <color=yellow>{3}</color>.", gameMap.MapName, (int)this.CurrentGrid.X, (int)this.CurrentGrid.Y, nAttacker.RoleName);
                        KT_TCPHandler.ShowClientReviveFrame(player, message, false);

                        /// Đánh dấu thằng giết mình
                        this.WhoKilledMeName = nAttacker.RoleName;

                        /// Nếu đối tượng tấn công là người chơi khác
                        if (nAttacker.IsPlayer())
                        {
                            KPlayer attackerPlayer = nAttacker as KPlayer;
                            /// Thông báo đã đánh trọng thương người chơi tương ứng
                            PlayerManager.ShowNotification(attackerPlayer, string.Format("<color=red><color=yellow>{0}</color> bị bạn đánh trọng thương.</color>", player.RoleName));

                            // Kích hoạt sự kiện người chơi giết người người chơi
                            GlobalEventSource.getInstance().fireEvent(new PlayerDeadEventObject(player, attackerPlayer));

                            /// Nếu trạng thái PK không đặc biệt
                            if (player.PKMode != (int)KiemThe.Entities.PKMode.Custom && attackerPlayer.PKMode != (int)KiemThe.Entities.PKMode.Custom)
                            {
                                /// Nếu đối tượng tấn công là kẻ chủ động tuyên chiến
                                //if (player.GetActiveFightStarter(attackerPlayer) == attackerPlayer)
                                {
                                    /// Nếu không phải bản đồ tự do PK
                                    if (!gameMap.FreePK)
                                    {
                                        /// Nếu bản thân trong trạng thái luyện công
                                        if (player.PKMode == (int)KiemThe.Entities.PKMode.Peace)
                                        {
                                            /// Nếu đối tượng tấn công có sát khí hoặc bản thân không có sát khí
                                            if (attackerPlayer.PKValue > 0 || player.PKValue <= 0)
                                            {
                                                /// Nếu sát khí chưa tới 10
                                                if (attackerPlayer.PKValue < 10)
                                                {
                                                    PlayerManager.ShowNotification(player, string.Format("Bạn đã sát hại đối phương, tăng 1 điểm sát khí."));
                                                    /// Tăng sát khí lên
                                                    attackerPlayer.PKValue++;
                                                }
                                            }
                                        }

                                        /// Nếu bản thân có sát khí
                                        if (player.PKValue > 0)
                                        {
                                            PlayerManager.ShowNotification(player, string.Format("Bạn đã tử nạn, giảm 1 điểm sát khí."));
                                            player.PKValue--;
                                        }
                                    }
                                }
                            }
                        }

                        /// Kết thúc toàn bộ trạng thái đồ sát của đối tượng chết
                        player.StopAllActiveFights();
                    }

                    /// Xóa toàn bộ trạng thái ngũ hành cũ
                    for (int i = (int)KE_STATE.emSTATE_BEGIN; i < (int)KE_STATE.emSTATE_ALLNUM; ++i)
                    {
                        if (this.HaveState((KE_STATE)i))
                        {
                            /// Xóa hiệu ứng tương ứng khỏi nhân vật
                            this.RemoveSpecialState((KE_STATE)i, true);
                        }
                    }

                    /// Xóa toàn bộ Buff có thuộc tính xóa khi chết
                    this.Buffs.ClearBuffsOnDead();

                    /// Thực hiện sự kiện gọi khi chết
                    this.OnDie(nAttacker);

                    /// Thực thi sự kiện OnKillObject
                    nAttacker.OnKillObject(this);


                    //Thực thi hàm trừ kinh nghiệm hỏng đồ
                    this.DeathPunish(nAttacker);

                    /// Gửi gói tin đối tượng chết về Client
                    KT_TCPHandler.SendObjectDiedToClients(this);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                }
            }
        }

        /// <summary>
        /// Giảm độ bền trang bị và kinh nghiệm khi chết
        /// </summary>
        /// <param name="nAttacker"></param>
        public void DeathPunish(GameObject nAttacker)
        {
            if (IsPlayer())
            {

                GameMap gameMap = GameManager.MapMgr.DictMaps[this.CurrentMapCode];

                KPlayer client = this as KPlayer;

                /// Nếu trong bản đồ phụ bản, tống kim, thi đấu môn phái, bạch hổ đường, tần lăng hoặc võ lâm liên đấu
                if (client.CurrentCopyMapID > 0 || FactionBattleManager.IsInFactionBattle(client) || Battel_SonJin_Manager.IsInBattle(client) || BaiHuTang.IsInBaiHuTang(client) || TeamBattle.IsInTeamBattlePKMap(client) || TeamBattle.IsInTeamBattleMap(client) || EmperorTomb.IsInsideEmperorTombMap(client) || client.PKMode == (int)PKMode.Custom)
                {
                    return;
                }

                // Nếu cấp nhỏ hơn 50 thì ko bị hỏng đồ
                if (this.m_Level < 50)
                {
                    return;
                }

                // nếu thằng giết thằng này là quái thì giảm 1 độ bền của đồ bất kỳ
                if (nAttacker is Monster)
                {
                    // Nếu bị giết bởi quái thì sẽ bị trừ  1điểm bất kỳ
                    client.GetPlayEquipBody().AbradeInDeath(3);
                }
                else if (nAttacker is KPlayer) // Nếu như thằng giết là
                {
                    // temp
                    int nPKValue = 0;

                    // Lấy ra trị pk hiện tại của nhân vật
                    nPKValue = client.PKValue;
                    if (nPKValue < 0)
                        nPKValue = 0;
                    if (nPKValue > KTGlobal.KD_MAX_PKVALUE)
                        nPKValue = KTGlobal.KD_MAX_PKVALUE;

                    PKPunish PkFind = KTGlobal.GetPkConfig(nPKValue);

                    if (PkFind != null)
                    {
                        long nLevelExp = client.m_nNextLevelExp;

                        long nLoseExp = 0;

                        if (nLevelExp >= 100000)
                        {
                            nLoseExp = (nLevelExp / 50) * PkFind.ExpDropRate;
                        }
                        else
                        {
                            nLoseExp = nLevelExp * PkFind.ExpDropRate / 50;
                        }

                        int nMaxDropExp = PkFind.ExpDropULimit;

                        if (nMaxDropExp > 0)
                        {
                            if (nLoseExp > nMaxDropExp)
                            {
                                nLoseExp = nMaxDropExp;
                            }
                        }

                        int nDec = 0;
                        if (nLoseExp > 2000000000)
                        {
                            nDec = 2000000000;
                        }
                        else
                        {
                            nDec = (int)nLoseExp;
                        }

                        /// Lượng kinh nghiệm giảm đi khi chết
                        nDec -= nDec * client.m_nSubExpPLost / 50;

                        // Nếu là free PK thì không bị trừ kinh nghiệm
                        if (gameMap.FreePK)
                        {
                            if (client.m_Experience > nDec)
                            {
                                client.m_Experience -= nDec;
                            }
                            else
                            {
                                client.m_Experience = 0;
                            }


                            PlayerManager.ShowNotification(client, KTGlobal.CreateStringByColor("Các hạ đã tử nạn, giảm: " + nDec + " kinh nghiệm.", ColorType.Importal));
                        }

                        client.GetPlayEquipBody().AbradeInDeath(PkFind.EquipAbradeRate);
                        GameManager.ClientMgr.NotifySelfExperience(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, -nDec);
                    }
                }
            }
        }

        /// <summary>
        /// Thực hiện hồi sinh
        /// </summary>
        public void DoRelive()
        {
            this.m_eDoing = KE_NPC_DOING.do_stand;

            /// Nếu là người chơi
            if (this is KPlayer)
            {
                /// Nếu đang ở trong phụ bản
                if (CopySceneEventManager.IsCopySceneExist(this.CurrentCopyMapID, this.CurrentMapCode))
                {
                    /// Thực thi sự kiện người chơi hồi sinh ở phụ bản
                    CopySceneEventManager.OnPlayerRelive(CopySceneEventManager.GetCopyScene(this.CurrentCopyMapID, this.CurrentMapCode), this as KPlayer);
                }

                /// Làm mới lại tốc độ di chuyển và tốc độ xuất chiêu
                this._m_RunSpeedReduceP = 0;
                this.SyncMoveSpeed();
                this._m_AttackSpeedReduceP = 0;
                this.SynsAttackSpeed();

                /// Bỏ đánh dấu thằng giết mình
                this.WhoKilledMeName = "Chưa rõ";
            }
        }

        #endregion Chết

        #region Biểu diễn trạng thái

        /// <summary>
        /// Thực hiện hiệu ứng kéo lại
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="skillPos"></param>
        /// <param name="durationFrame"></param>
        /// <param name="distanceEachFrame"></param>
        public UnityEngine.Vector2 DoDrag(GameObject attacker, UnityEngine.Vector2 skillPos, int durationFrame, int distanceEachFrame)
        {
            /// Vị trí hiện tại
            UnityEngine.Vector2 selfPos = new UnityEngine.Vector2((int)this.CurrentPos.X, (int)this.CurrentPos.Y);
            /// Nếu đã chết
            if (this.IsDead())
            {
                return selfPos;
            }
            /// Nếu đang bị đẩy lui hoặc kéo lại
            else if (this.HaveState(KE_STATE.emSTATE_DRAG) || this.HaveState(KE_STATE.emSTATE_KNOCK))
            {
                return selfPos;
            }
            /// Nếu thời gian kéo lại <= 0
            else if (durationFrame <= 0)
            {
                return selfPos;
            }

            /// Ngừng Storyboard đang thực thi
            if (this is Monster)
            {
                KTMonsterStoryBoardEx.Instance.Remove(this as Monster);
            }
            else if (this is KPlayer)
            {
                KTPlayerStoryBoardEx.Instance.Remove(this as KPlayer);
            }

            /// Bản đồ hiện tại
            GameMap gameMap = GameManager.MapMgr.DictMaps[this.CurrentMapCode];
            /// Vector chỉ hướng
            UnityEngine.Vector2 dirVector = skillPos - selfPos;
            /// Khoảng cách đến vị trí đích
            float distance = UnityEngine.Vector2.Distance(selfPos, skillPos);
            /// Vị trí đích đến
            UnityEngine.Vector2 toPos = KTMath.FindPointInVectorWithDistance(selfPos, dirVector, Math.Min(distance, durationFrame * distanceEachFrame));
            /// Vị trí không có vật cản trên đường đi
            toPos = KTGlobal.FindLinearNoObsPoint(gameMap, selfPos, toPos);

            /// Thực hiện Logic giật lùi nhân vật về vị trí tương ứng
            KTSkillManager.SetTimeout(durationFrame / 18f, () =>
            {
                this.CurrentPos = new System.Windows.Point((int)toPos.x, (int)toPos.y);

                /// Cập nhật vị trí đối tượng vào Map
                GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                if (mapGrid != null)
                {
                    mapGrid.MoveObject(-1, -1, (int)this.CurrentPos.X, (int)this.CurrentPos.Y, this);
                }

                //if (this is KPlayer)
                //{
                //    /// Thực hiện gọi hàm cập nhật di chuyển
                //    ClientManager.DoSpriteMapGridMove(this as KPlayer);
                //}
            });

            /// Trả về vị trí đích đến
            return toPos;
        }

        /// <summary>
        /// Thực hiện hiệu ứng bị đẩy lui
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="skillPos"></param>
        /// <param name="durationFrame"></param>
        /// <param name="distanceEachFrame"></param>
        public UnityEngine.Vector2 DoKnockback(GameObject attacker, UnityEngine.Vector2 skillPos, int durationFrame, int distanceEachFrame)
        {
            /// Vị trí hiện tại
            UnityEngine.Vector2 selfPos = new UnityEngine.Vector2((int)this.CurrentPos.X, (int)this.CurrentPos.Y);

            /// Nếu đã chết
            if (this.IsDead())
            {
                return selfPos;
            }
            /// Nếu đang bị đẩy lui hoặc kéo lại
            else if (this.HaveState(KE_STATE.emSTATE_DRAG) || this.HaveState(KE_STATE.emSTATE_KNOCK))
            {
                return selfPos;
            }
            /// Nếu thời gian kéo lại <= 0
            else if (durationFrame <= 0)
            {
                return selfPos;
            }

            /// Ngừng Storyboard đang thực thi
            if (this is Monster)
            {
                KTMonsterStoryBoardEx.Instance.Remove(this as Monster);
            }
            else if (this is KPlayer)
            {
                KTPlayerStoryBoardEx.Instance.Remove(this as KPlayer);
            }

            /// Bản đồ hiện tại
            GameMap gameMap = GameManager.MapMgr.DictMaps[this.CurrentMapCode];
            /// Vector chỉ hướng
            UnityEngine.Vector2 dirVector = selfPos - skillPos;
            /// Vị trí đích đến
            UnityEngine.Vector2 toPos = KTMath.FindPointInVectorWithDistance(selfPos, dirVector, durationFrame * distanceEachFrame);
            /// Vị trí không có vật cản trên đường đi
            toPos = KTGlobal.FindLinearNoObsPoint(gameMap, selfPos, toPos);

            /// Thực hiện Logic giật lùi nhân vật về vị trí tương ứng
            KTSkillManager.SetTimeout(durationFrame / 18f, () =>
            {
                this.CurrentPos = new System.Windows.Point((int)toPos.x, (int)toPos.y);

                /// Cập nhật vị trí đối tượng vào Map
                GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                if (mapGrid != null)
                {
                    mapGrid.MoveObject(-1, -1, (int)this.CurrentPos.X, (int)this.CurrentPos.Y, this);
                }

                //if (this is KPlayer)
                //{
                //    /// Thực hiện gọi hàm cập nhật di chuyển
                //    ClientManager.DoSpriteMapGridMove(this as KPlayer);
                //}
            });

            /// Trả về vị trí đích đến
            return toPos;
        }

        /// <summary>
        /// Thực hiện hiệu ứng hỗn loạn
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="durationFrame"></param>
        public void DoConfuse(GameObject attacker, int durationFrame)
        {
            /// Vị trí hiện tại
            UnityEngine.Vector2 selfPos = new UnityEngine.Vector2((int)this.CurrentPos.X, (int)this.CurrentPos.Y);

            /// Nếu đã chết
            if (this.IsDead())
            {
                return;
            }
            /// Nếu đang bị mù
            else if (this.HaveState(KE_STATE.emSTATE_CONFUSE))
            {
                return;
            }
            /// Nếu thời gian hỗn lại <= 0
            else if (durationFrame <= 0)
            {
                return;
            }

            /// Bản đồ hiện tại
            GameMap gameMap = GameManager.MapMgr.DictMaps[this.CurrentMapCode];

            /// Thực thi Timer
            KTSkillManager.SetSchedule(1f, durationFrame / 18f, () =>
            {
                /// Tốc độ hiện tại
                int moveSpeed = KTGlobal.MoveSpeedToPixel(this.GetCurrentRunSpeed()) / 2;
                /// Tinh khoảng cách dịch được tối đa trong 1s
                float distance = moveSpeed * 1f;

                /// Vị trí hiện tại
                selfPos = new UnityEngine.Vector2((int)this.CurrentPos.X, (int)this.CurrentPos.Y);
                /// Vị trí ngẫu nhiên sẽ dịch tới
                UnityEngine.Vector2 _randPos = KTGlobal.GetRandomLinearNoObsPoint(gameMap, selfPos, distance);

                /// Nếu không thể dịch đến đích được thì thôi
                if (!KTGlobal.HasPath(this.CurrentMapCode, selfPos, _randPos))
                {
                    return;
                }

                /// Thực hiện di chuyển đến vị trí chỉ định
                if (this is Monster)
                {
                    KTMonsterStoryBoardEx.Instance.Remove(this as Monster);
                    KTMonsterStoryBoardEx.Instance.Add(this as Monster, this.CurrentPos, new System.Windows.Point((int)_randPos.x, (int)_randPos.y), KE_NPC_DOING.do_walk, true);
                }
                else
                {
                    KTPlayerStoryBoardEx.Instance.Remove(this as KPlayer);
                    KTPlayerStoryBoardEx.Instance.Add(this as KPlayer, this.CurrentPos, new System.Windows.Point((int)_randPos.x, (int)_randPos.y), KE_NPC_DOING.do_walk, true);
                }
            });
        }

        #endregion Biểu diễn trạng thái

        /// <summary>
        /// Hủy bỏ trạng thái của kỹ năng
        /// </summary>
        /// <param name="nSkillId"></param>
        /// <param name="bSyncClient"></param>
        public void RemoveStateSkillEffect(int nSkillId, bool bSyncClient)
        {
            /*
            KStateMap::iterator it = m_mapState.find(nSkillId);
            if (it == m_mapState.end())
                return;

            KStateNode* pInvalidNode = it->second;
            if (!pInvalidNode)
                return;

            RemoveStateNode(*pInvalidNode, bSyncClient);

            RemoveStateInfo(pInvalidNode->m_nStateGraphics, pInvalidNode->m_nStatePriority);

            SAFE_DELETE(pInvalidNode);
            m_mapState.erase(it);

            if (IsPlayer())
                g_CoreEventNotify(emCOREEVENT_BUFF_CHANGE);
            */
        }

        #endregion Public methods

        /// <summary>
        /// Nhận sát thương
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damageTaken"></param>
        public virtual void TakeDamage(GameObject attacker, int damageTaken)
        {
            /// Nếu đã chết
            if (this.IsDead())
            {
                return;
            }

            /// Nếu không có attacker
            if (attacker == null)
            {
                return;
            }

            /// Nếu đang trong trạng thái tàng hình
            if (this.IsInvisible())
            {
                /// Nếu mất trạng thái tàng hình khi chịu sát thương
                if (this.m_InvisibleType >= 1)
                {
                    this.RemoveInvisibleState();
                }
            }

            /// Nếu bản thân là người chơi
            if (this is KPlayer client)
            {
                /// Nếu thằng tấn công là người chơi
                if (attacker is KPlayer attackclient)
                {
                    /// Nếu thằng này đang để đồ sát
                    if (attackclient.PKMode == (int)PKMode.All || attackclient.IsActiveFightWith(client))
                    {
                        /// Nếu thời gian bị tấn công trước đó mà cách 5s thì notify về cho thằng này đang bị đánh
                        if (KTGlobal.GetCurrentTimeMilis() - LastNotifyAttackTicks >= 5000)
                        {
                            LastNotifyAttackTicks = KTGlobal.GetCurrentTimeMilis();

                            //GameManager.ClientMgr.SendSystemChatMessageToClient(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, "Bạn đang bị người chơi " + KTGlobal.CreateStringByColor(attackclient.RoleName, ColorType.Importal) + " tấn công!");

                            PlayerManager.ShowNotification(client, "Bạn đang bị người chơi " + KTGlobal.CreateStringByColor(attackclient.RoleName, ColorType.Importal) + " tấn công!");
                            /// Thông báo về client là thằng này đang bị đồ sát
                            client.SendPacket((int)TCPGameServerCmds.CMD_KT_TAKEDAMAGE, client.RoleID + ":" + attacker.RoleID);
                        }
                    }
                }

                /// Cho hỏng đồ nào đó
                client.GetPlayEquipBody().InjuredSomebody();
            }
        }

        /// <summary>
        /// Luồng thực hiện Blink
        /// </summary>
        private ScheduleTask blinkTimer = null;

        /// <summary>
        /// Đang thực hiện Blink không
        /// </summary>
        /// <returns></returns>
        public bool IsBlinking()
        {
            return this.blinkTimer != null;
        }

        /// <summary>
        /// Ngừng thực hiện Blink
        /// </summary>
        public void StopBlink()
        {
            if (this.blinkTimer != null)
            {
                this.blinkTimer.Stop();
                this.blinkTimer = null;
            }
        }

        /// <summary>
        /// Thực hiện Blink đến vị trí chỉ định trong khoảng thời gian tương ứng
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="duration"></param>
        /// <param name="tick"></param>
        /// <param name="finish"></param>
        public void BlinkTo(int posX, int posY, float duration, Action tick = null, Action finish = null)
        {
            /// Ngừng Blink
            this.StopBlink();

            /// Thời điểm bắt đầu bay
            long startTick = KTGlobal.GetCurrentTimeMilis();

            /// Vector hướng di chuyển
            UnityEngine.Vector2 dirVector = new UnityEngine.Vector2(posX - (int)this.CurrentPos.X, posY - (int)this.CurrentPos.Y);
            /// Vị trí xuất phát
            UnityEngine.Vector2 startPos = new UnityEngine.Vector2((int)this.CurrentPos.X, (int)this.CurrentPos.Y);

            /// Tạo Timer thực hiện
            this.blinkTimer = new ScheduleTask()
            {
                PeriodTick = 100,
                Duration = (int)(duration * 1000),
                Work = () =>
                {
                    /// Thời điểm khinh công lần trước
                    this.LastBlinkTicks = KTGlobal.GetCurrentTimeMilis();

                    /// Thời gian tồn tại
                    float lifeTime = (KTGlobal.GetCurrentTimeMilis() - startTick) / 1000f;

                    /// % thời gian qua
                    float percent = lifeTime / duration;
                    /// Vị trí mới
                    UnityEngine.Vector2 currentPos = startPos + dirVector * percent;
                    this.CurrentPos = new System.Windows.Point(currentPos.x, currentPos.y);

                    /// Cập nhật vị trí đối tượng vào Map
                    GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                    if (mapGrid != null)
                    {
                        mapGrid.MoveObject(-1, -1, (int)currentPos.x, (int)currentPos.y, this);
                    }

                    /// Thực thi sự kiện Tick
                    tick?.Invoke();
                },
                Finish = () =>
                {
                    this.CurrentPos = new System.Windows.Point(posX, posY);

                    /// Cập nhật vị trí đối tượng vào Map
                    GameManager.MapGridMgr.DictGrids.TryGetValue(this.CurrentMapCode, out MapGrid mapGrid);
                    if (mapGrid != null)
                    {
                        mapGrid.MoveObject(-1, -1, posX, posY, this);
                    }

                    /// Thực thi sự kiện Finish
                    finish?.Invoke();

                    /// Ngừng Blink
                    this.StopBlink();
                },
            };
            this.blinkTimer.Start();
        }
    }
}