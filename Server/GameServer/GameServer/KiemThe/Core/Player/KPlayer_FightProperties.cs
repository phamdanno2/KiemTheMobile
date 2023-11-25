using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.Entities.Sprite;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static GameServer.KiemThe.Logic.KTSkillManager;

namespace GameServer.Logic
{
    /// <summary>
    /// Thuộc tính chiến đấu của đối tượng
    /// </summary>
    public partial class KPlayer
    {
        /// <summary>
        /// Danh sách sát thương đã nhận từ đối tượng tương ứng
        /// </summary>
        private readonly ConcurrentQueue<SpriteDamageQueue> totalReceivedDamagesThisFrame = new ConcurrentQueue<SpriteDamageQueue>();

        /// <summary>
        /// Danh sách sát thương gây ra cho đối tượng tương ứng
        /// </summary>
        private readonly ConcurrentQueue<SpriteDamageQueue> totalDamageDealtThisFrame = new ConcurrentQueue<SpriteDamageQueue>();

        #region Define

        /// <summary>
        /// GM bất tử tàng hình
        /// </summary>
        public bool GM_Immortality { get; set; } = false;

        private bool _GM_Invisiblity = false;

        /// <summary>
        /// GM tàng hình
        /// </summary>
        public bool GM_Invisiblity
        {
            get
            {
                return this._GM_Invisiblity;
            }
            set
            {
                this._GM_Invisiblity = value;

                /// Nếu là hủy tàng hình
                if (!value)
                {
                    /// Nếu không phải đang tàng hình
                    if (!this.IsInvisible())
                    {
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
                }
                /// Nếu là bắt đầu tàng hình
				else
                {
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
            }
        }

        /// <summary>
        /// Cấm dùng kỹ năng
        /// </summary>
        public bool ForbidUsingSkill { get; set; } = false;

        /// <summary>
        /// Buff kích hoạt hồi sinh tại chỗ
        /// </summary>
        public BuffDataEx m_sReviveBuff { get; private set; }

        /// <summary>
        /// Thông tin tỷ thí
        /// </summary>
        private KPlayer_ChallengeInfo ChallengeInfo = null;

        /// <summary>
        /// Danh sách đang tuyên chiến
        /// </summary>
        private readonly ConcurrentDictionary<KPlayer, KPlayer_ActiveFightInfo> ActiveFightInfos = new ConcurrentDictionary<KPlayer, KPlayer_ActiveFightInfo>();

        /// <summary>
        /// Danh sách miễn nhiễm sát thương với người chơi tương ứng trong khoảng thời gian
        /// </summary>
        private readonly ConcurrentDictionary<KPlayer, KPlayer_ImmuneToAllDamagesOf> ImmuneToAllDamagesOf = new ConcurrentDictionary<KPlayer, KPlayer_ImmuneToAllDamagesOf>();

        /// <summary>
        /// Trạng thái PK
        /// </summary>
        public int PKMode
        {
            get
            {
                return this._RoleDataEx.PKMode;
            }
            set
            {
                ///// Nếu trùng với giá trị cũ thì bỏ qua
                //if (this._RoleDataEx.PKMode == value)
                //{
                //    return;
                //}

                this._RoleDataEx.PKMode = value;

                /// Gửi thông báo tới tất cả các đối tượng xung quanh trạng thái PK hoặc Camp của bản thân thay đổi
                KT_TCPHandler.SendToOthersMyPKModeAndCampChanged(this);
            }
        }

        /// <summary>
        /// Trị PK
        /// </summary>
        public int PKValue
        {
            get
            {
                return this._RoleDataEx.PKValue;
            }

            set
            {
                lock (this)
                {
                    ///// Nếu trùng với giá trị cũ thì bỏ qua
                    //if (this._RoleDataEx.PKValue == value)
                    //{
                    //    return;
                    //}

                    this._RoleDataEx.PKValue = value;

                    /// Gửi thông báo tới tất cả các đối tượng xung quanh trị PK của bản thân thay đổi
                    KT_TCPHandler.SendToOthersMyPKValueChanged(this);
                }
            }
        }

        #region Quan hàm
        /// <summary>
        /// Cấp độ quan hàm
        /// </summary>
        public int m_ChopLevel { get; set; } = 0;
        #endregion
        #endregion Define

        #region Public methods
        /// <summary>
        /// Nhận sát thương từ đối tượng tương ứng
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public void AddReceiveDamage(GameObject attacker, SkillResult type, int damage)
        {
            /// Nếu bản thân không phải người chơi
            if (!(this is KPlayer))
            {
                return;
            }
            /// Nếu không có Attacker
            else if (attacker == null)
            {
                return;
            }

            /// Thêm vào hàng đợi
            this.totalReceivedDamagesThisFrame.Enqueue(new SpriteDamageQueue()
            {
                Object = attacker,
                Damage = damage,
                Type = type,
            });
        }

        /// <summary>
        /// Nhận sát thương từ đối tượng tương ứng
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public void AddDamageDealt(GameObject target, SkillResult type, int damage)
        {
            /// Nếu bản thân không phải người chơi
            if (!(this is KPlayer))
            {
                return;
            }
            /// Nếu không có Target
            else if (target == null)
            {
                return;
            }

            /// Thêm vào hàng đợi
            this.totalDamageDealtThisFrame.Enqueue(new SpriteDamageQueue()
            {
                Object = target,
                Damage = damage,
                Type = type,
            });
        }

        /// <summary>
        /// Thiết lập trạng thái bảo vệ khi chuyển map
        /// </summary>
        public void SendChangeMapProtectionBuff()
        {
            /// Thêm hiệu ứng bảo vệ
            SkillDataEx skill = KSkill.GetSkillData(200000);
            /// Nếu kỹ năng không tồn tại
            if (skill != null)
            {
                /// Nếu đang tồn tại Buff hồi sinh cũ rồi thì thôi
                if (this.Buffs.HasBuff(200000))
                {
                    return;
                }

                /// Dữ liệu kỹ năng tương ứng
                SkillLevelRef skillRef = new SkillLevelRef()
                {
                    Data = skill,
                    AddedLevel = 1,
                    BonusLevel = 0,
                    CanStudy = false,
                };
                //PropertyDictionary skillPd = skillRef.Properties;

                long duration = 5000;//skillPd.Get<KMagicAttrib>((int)MAGIC_ATTRIB.magic_skill_statetime).nValue[0] * 1000 / 18;

                BuffDataEx buff = new BuffDataEx()
                {
                    Skill = skillRef,
                    Duration = duration,
                    LoseWhenUsingSkill = false,
                    SaveToDB = false,
                    StackCount = 1,
                    StartTick = KTGlobal.GetCurrentTimeMilis(),
                };
                this.Buffs.AddBuff(buff);
            }
        }

        /// <summary>
        /// Xóa trạng thái bảo vệ khi chuyển map
        /// </summary>
        public void RemoveChangeMapProtectionBuff()
        {
            this.Buffs.RemoveBuff(200000);
        }

        /// <summary>
        /// Hồi sinh tại chỗ do được trị thương
        /// </summary>
        /// <param name="nLifeP"></param>
        /// <param name="nManaP"></param>
        /// <param name="nStaminaP"></param>
        /// <returns></returns>
        public void ReceiveCure(int nLifeP, int nManaP, int nStaminaP)
        {
            /// Nếu bản thân không chết
            if (!this.IsDead())
            {
                return;
            }

            /// Gọi hàm hồi sinh
            PlayerManager.Relive(this, this.CurrentMapCode, (int)this.CurrentPos.X, (int)this.CurrentPos.Y, nLifeP, nManaP, nStaminaP);
        }

        /// <summary>
        /// Ngẫu nhiên đánh cắp 1 trạng thái hỗ trợ chủ động của kẻ địch xung quanh chuyển qua bản thân hoặc đồng đội đang tương tác
        /// </summary>
        /// <param name="maxStateLevel"></param>
        /// <param name="parentSkillID"></param>
        /// <param name="totalStolenChanceLeft"></param>
        public void StealRandomPositiveBuffOfNearbyEnemy(int stealPercent, int maxStateLevel, ref int totalStolenChanceLeft)
        {
            /// Nếu số lượng cần cướp dưới 0
            if (totalStolenChanceLeft <= 0)
            {
                return;
            }

            /// Danh sách kẻ địch xung quanh
			List<KPlayer> nearbyEnemyPlayers = KTLogic.GetNearByEnemies<KPlayer>(this, 1000);
            /// Duyệt danh sách kẻ địch
            foreach (KPlayer enemy in nearbyEnemyPlayers)
            {
                if (enemy == this)
                {
                    continue;
                }

                /// Lấy ngẫu nhiên 1 trạng thái hỗ trợ chủ động
                BuffDataEx buff = enemy.Buffs.GetRandomPositiveBuff();
                /// Nếu tồn tại trạng thái hỗ trợ chủ động
                if (buff != null)
                {
                    /// Tỷ lệ cướp
                    int nRate = KTGlobal.GetRandomNumber(1, 100);
                    /// Nếu không may mắn cướp được trạng thái này
                    if (nRate > stealPercent)
                    {
                        continue;
                    }

                    /// Kỹ năng tương ứng
                    SkillDataEx skill = KSkill.GetSkillData(buff.Skill.SkillID);
                    /// Tạo mới kỹ năng theo cấp
                    SkillLevelRef skillRef = new SkillLevelRef()
                    {
                        Data = skill,
                        AddedLevel = Math.Min(maxStateLevel, buff.Level),
                        BonusLevel = 0,
                        CanStudy = false,
                    };

                    /// ProDict kỹ năng
                    PropertyDictionary skillPd = skillRef.Properties;

                    /// Thời gian duy trì
                    float duration = 0;
                    if (skillPd.ContainsKey((int)MAGIC_ATTRIB.magic_skill_statetime))
                    {
                        if (skillPd.Get<KMagicAttrib>((int)MAGIC_ATTRIB.magic_skill_statetime).nValue[0] == -1)
                        {
                            duration = -1;
                        }
                        else
                        {
                            duration = skillPd.Get<KMagicAttrib>((int)MAGIC_ATTRIB.magic_skill_statetime).nValue[0] / 18f;
                        }
                    }

                    /// Tạo mới Buff tương ứng
                    BuffDataEx stealBuff = new BuffDataEx()
                    {
                        CustomProperties = null,
                        Duration = (int)(duration * 1000),
                        LoseWhenUsingSkill = buff.LoseWhenUsingSkill,
                        SaveToDB = false,
                        Skill = skillRef,
                        StartTick = KTGlobal.GetCurrentTimeMilis(),
                    };
                    /// Thêm Buff vừa đánh cắp được vào danh sách nếu có mục tiêu là đồng đội
                    if (this.CurrentTarget != null && this.CurrentTarget is KPlayer && KTLogic.IsTeamMate(this, this.CurrentTarget as KPlayer) && KTLogic.IsInDist(this, this.CurrentTarget, 1000))
                    {
                        KPlayer teammate = this.CurrentTarget as KPlayer;
                        teammate.Buffs.AddBuff(stealBuff);
                    }
                    /// Nếu không có mục tiêu là đồng đội thì thêm cho bản thân
                    else
                    {
                        PlayerManager.ShowNotification(this, "Ăn cắp trạng thái => " + stealBuff.Skill.Data.Name + " - Cấp " + stealBuff.Level);
                        this.Buffs.AddBuff(stealBuff);
                    }

                    /// Giảm tổng số cướp được
                    totalStolenChanceLeft--;

                    /// Đã đủ số lượng cướp thì thoát không duyệt thêm nữa
                    if (totalStolenChanceLeft <= 0)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Kích hoạt chế độ hồi sinh tại chỗ
        /// </summary>
        /// <param name="reviveBuff"></param>
        /// <param name="owner"></param>
        public void EnableReviveAtCurrentPos(BuffDataEx reviveBuff, GameObject owner)
        {
            this.m_sReviveBuff = reviveBuff;

            /// Thông báo cho người chơi được hồi sinh
            PlayerManager.ShowNotification(this, string.Format("<color=red><color=yellow>{0}</color> đã hồi sinh cho bạn, có thể sử dụng chức năng Hồi sinh tại chỗ để hồi sinh!</color>", owner.RoleName));

            /// Gửi yêu cầu cập nhật bảng thông báo hồi sinh
            KT_TCPHandler.ShowClientReviveFrame(this, "", true);
        }

        /// <summary>
        /// Kiểm tra đối tượng có đang trong trạng thái tỷ thí không
        /// </summary>
        /// <returns></returns>
        public bool IsChallengeTime()
        {
            if (this.ChallengeInfo == null)
            {
                return false;
            }
            return this.ChallengeInfo.IsChallengeTime;
        }

        /// <summary>
        /// Kiểm tra đối tượng có đang tỷ thí cùng người chơi tương ứng không
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsChallengeWith(KPlayer player)
        {
            /// Nếu không có mục tiêu
            if (player == null)
            {
                return false;
            }

            if (!this.IsChallengeTime())
            {
                return false;
            }
            return this.ChallengeInfo != null && this.ChallengeInfo.Target == player;
        }

        /// <summary>
        /// Bắt đàu tỷ thí
        /// </summary>
        /// <param name="target"></param>
        public void BeginChallenge(KPlayer target)
        {
            /// Nếu không có mục tiêu
            if (target == null)
            {
                return;
            }

            /// Nếu không phải đang tỷ thí
            if (!this.IsChallengeTime())
            {
                /// Tạo mới thông tin tỷ thí
                this.ChallengeInfo = new KPlayer_ChallengeInfo()
                {
                    Target = target,
                    StartTick = KTGlobal.GetCurrentTimeMilis(),
                };
                target.ChallengeInfo = new KPlayer_ChallengeInfo()
                {
                    Target = this,
                    StartTick = KTGlobal.GetCurrentTimeMilis(),
                };

                /// Gửi gói tin thông báo bắt đầu tỷ thí
                KT_TCPHandler.SendBeginChallenge(this, target);
            }
        }

        /// <summary>
        /// Ngừng tỷ thí
        /// </summary>
        /// <param name="winner"></param>
        public void StopChallenge(KPlayer winner = null)
        {
            try
            {
                /// Toác
                if (this.ChallengeInfo == null)
                {
                    return;
                }

                if (this.ChallengeInfo != null && this.IsChallengeTime())
                {
                    /// Hủy thông tin tỷ thí
                    if (this.ChallengeInfo.Target != null)
                    {
                        /// Gửi gói tin thông báo kết thúc tỷ thí
                        KT_TCPHandler.SendStopChallenge(this, this.ChallengeInfo.Target, winner);

                        this.ChallengeInfo.Target.ChallengeInfo = null;
                    }
                    this.ChallengeInfo = null;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Kiểm tra có đang tuyên chiến cùng người chơi tương ứng không
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsActiveFightWith(KPlayer target)
        {
            /// Nếu không có mục tiêu
            if (target == null)
            {
                return false;
            }

            if (this.ActiveFightInfos == null)
            {
                return false;
            }

            return this.ActiveFightInfos.ContainsKey(target);
        }

        /// <summary>
        /// Trả về đối tượng phát động tuyên chiến giữa 2 đối tượng
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public KPlayer GetActiveFightStarter(KPlayer target)
        {
            /// Nếu không có mục tiêu
            if (target == null)
            {
                return null;
            }

            if (this.ActiveFightInfos.TryGetValue(target, out KPlayer_ActiveFightInfo fightInfo))
            {
                return fightInfo.Starter;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Chủ động tuyên chiến với người chơi tương ứng
        /// </summary>
        /// <param name="target"></param>
        /// <param name="notifyToClient"></param>
        public void StartActiveFight(KPlayer target, bool notifyToClient = true)
        {
            /// Nếu không có mục tiêu
            if (target == null)
            {
                return;
            }

            /// Nếu đang tuyên chiến rồi
            if (this.ActiveFightInfos.ContainsKey(target))
            {
                return;
            }

            /// Thêm vào danh sách tuyên chiến
            this.ActiveFightInfos[target] = new KPlayer_ActiveFightInfo()
            {
                LastAttackTick = KTGlobal.GetCurrentTimeMilis(),
                Starter = this,
                Target = target,
            };

            /// Thêm vào danh sách tuyên chiến của đối tượng
            target.ActiveFightInfos[this] = new KPlayer_ActiveFightInfo()
            {
                LastAttackTick = KTGlobal.GetCurrentTimeMilis(),
                Starter = this,
                Target = this,
            };

            if (notifyToClient)
            {
                PlayerManager.ShowNotification(this, string.Format("<color=red>Tuyên chiến với <color=yellow>{0}</color>!</color>", target.RoleName));
                PlayerManager.ShowNotification(target, string.Format("<color=red><color=yellow>{0}</color> đã tuyên chiến với bạn!</color>", this.RoleName));
            }
            KT_TCPHandler.SendStartActiveFight(this, target);
        }

        /// <summary>
        /// Ngừng tuyên chiến với người chơi tương ứng
        /// </summary>
        /// <param name="target"></param>
        /// <param name="notifyToClient"></param>
        public void StopActiveFight(KPlayer target, bool notifyToClient = false)
        {
            if (target == null)
            {
                return;
            }

            /// Nếu bản thân có trạng thái tuyên chiến tương ứng
            if (this.ActiveFightInfos.ContainsKey(target))
            {
                /// Xóa khỏi danh sách tuyên chiến
                this.ActiveFightInfos.TryRemove(target, out _);
                /// Thông báo về Client
                if (notifyToClient)
                {
                    PlayerManager.ShowNotification(this, string.Format("<color=red>Kết thúc tuyên chiến cùng <color=yellow>{0}</color>!</color>", target.RoleName));
                }
            }

            /// Nếu đối phương có trạng thái tuyên chiến tương ứng
            if (target.ActiveFightInfos.ContainsKey(this))
            {
                /// Xóa khỏi danh sách tuyên chiến của đối phương
                target.ActiveFightInfos.TryRemove(this, out _);
                /// Thông báo về Client
                if (notifyToClient)
                {
                    PlayerManager.ShowNotification(target, string.Format("<color=red>Kết thúc tuyên chiến cùng <color=yellow>{0}</color>!</color>", this.RoleName));
                }
            }

            /// Gửi gói tin thông báo ngừng tuyên chiến về Client
            KT_TCPHandler.SendStopActiveFight(this, target);
        }

        /// <summary>
        /// Ngừng tuyên chiến với toàn bộ người chơi
        /// </summary>
        public void StopAllActiveFights()
        {
            /// Duyệt danh sách tuyên chiến và ngừng
            List<KPlayer> keys = this.ActiveFightInfos.Keys.ToList();
            foreach (KPlayer player in keys)
            {
                this.StopActiveFight(player);
            }
        }

        /// <summary>
        /// Thêm miễn nhiễm toàn bộ sát thương của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="tick"></param>
        public void AddImmuneToAllDamagesOf(KPlayer player, long tick)
        {
            if (player == null)
            {
                return;
            }

            this.ImmuneToAllDamagesOf[player] = new KPlayer_ImmuneToAllDamagesOf()
            {
                Player = player,
                Tick = tick,
                StartTick = KTGlobal.GetCurrentTimeMilis(),
            };
        }

        /// <summary>
        /// Xóa trạng thái miễn nhiễm toàn bộ sát thương của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        public void RemoveImmuneToAllDamagesOf(KPlayer player)
        {
            if (player == null)
            {
                return;
            }

            this.ImmuneToAllDamagesOf.TryRemove(player, out _);
        }

        /// <summary>
        /// Kiểm tra đối tượng có miễn nhiễm toàn bộ sát thương của người chơi tương ứng không
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsImmuneToAllDamagesOf(KPlayer player)
        {
            if (player == null)
            {
                return false;
            }

            return this.ImmuneToAllDamagesOf.ContainsKey(player);
        }

        #endregion Public methods
    }
}