using GameServer.Core.GameEvent;
using GameServer.Core.GameEvent.EventOjectImpl;
using GameServer.KiemThe;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.Entities.Sprite;
using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.GameEvents;
using GameServer.KiemThe.GameEvents.FactionBattle;
using GameServer.KiemThe.GameEvents.GuildWarManager;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager.Battle;
using GameServer.KiemThe.Logic.Manager.Skill.PoisonTimer;
using GameServer.KiemThe.Network.Entities;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.KiemThe.GameEvents.EmperorTomb;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý sự kiện
    /// </summary>
    public partial class KPlayer
    {
        #region Core
        /// <summary>
        /// Sự kiện chạy liên tục mỗi khoảng 0.5s
        /// </summary>
        public override void Tick()
        {
            /// Fix lỗi âm máu, mana, thể lực
            if (this.m_CurrentLife < 0)
            {
                this.m_CurrentLife = 0;
            }
            if (this.m_CurrentMana < 0)
            {
                this.m_CurrentMana = 0;
            }
            if (this.m_CurrentStamina < 0)
            {
                this.m_CurrentStamina = 0;
            }

            /// Nếu bản thân đã chết
            if (this.IsDead())
            {
                /// Thiết lập đã chết
                this.m_eDoing = KE_NPC_DOING.do_death;
                /// Thiết lập máu = 0
                this.m_CurrentLife = 0;
                /// Gửi thông báo đến chính mình
                KT_TCPHandler.NotifySelfLifeChanged(this as KPlayer, 0);
                return;
            }

            base.Tick();

            try
            {
                if (this.LifeTime % 500 == 0)
                {
                    /// Thực hiện chạy giảm thể lực khi ở trạng thái đồ sát
                    this.ProcessPKAllSubStamina();
                    /// Thực hiện cập nhật thông tin nhóm
                    this.ProcessUpdateTeamMemberInfo();
                    /// Thực hiện đếm lùi thời gian tỷ thí
                    this.ProcessChallengeCountDown();
                    /// Thực hiện đếm lùi thời gian thao tác gì đó
                    this.ProcessProgressCountDown();
                    /// Thực thi trừ điểm sát khí
                    this.ProcessSubPKValue();
                    /// Thực thi trừ thời gian miễn nhiễm sát thương của người chơi khác
                    this.ProcessRemoveImmuneAllDamagesOfOtherPlayers();
                    /// Kích hoạt kỹ năng tự động mỗi giây
                    this.ProcessAutoSkillActivateEachSeconds();
                    /// Thực hiện gom nhóm sát thương gửi về Client
                    this.ProcessSynsDamage();
                    /// Thực hiện Logic Captcha chống Bot
                    this.ProcessCaptchaLogic();

                    /// Thực thi Tick ở các sự kiện đặc biệt
                    this.ProcessSpecialEventTick();
                }

                if (this.LifeTime % 2000 == 0)
				{
                    /// Kiểm tra mục tiêu xung quanh để kích hoạt Buff
                    this.CheckEnemiesAroundAndActivateBuff();
                }

                if (this.LifeTime % 5000 == 0)
				{
                    /// Thực hiện Logic trận
                    this.ProcessTeamZhenLogic();
				}
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        #endregion Core

        #region Logic
        /// <summary>
        /// Thực thi Tick ở các sự kiện đặc biệt
        /// </summary>
        private void ProcessSpecialEventTick()
        {
            /// Nếu đang ở trong Tần Lăng
            if (EmperorTomb.IsInsideEmperorTombMap(this))
            {
                /// Thực hiện Tick
                EmperorTomb_ActivityScript.ProcessPlayerTick(this);
            }
        }

        /// <summary>
        /// Thực hiện Logic Captcha chống BOT
        /// </summary>
        private void ProcessCaptchaLogic()
        {
            /// Nếu đang có Captcha
            if (this._CurrentCaptcha != null)
            {
                /// Nếu đã hết thời gian mà chưa trả lời thì tống vào tù
                if (this._CurrentCaptcha.IsOver)
                {
                    PlayerManager.ShowNotification(this, "Đã hết thời gian trả lời, tự chuyển vào nhà lao!");
                    /// Vào tù
                    this.SendToJail();
                    /// Hủy Captcha
                    this.RemoveCaptcha();
                    /// Thực thi hàm khi trả lời sai
                    this.AnswerCaptcha?.Invoke(false);
                    /// Hủy hàm Callback khi trả lời Captcha
                    this.AnswerCaptcha = null;
                    /// Bỏ qua
                    return;
                }
            }

            /// Nếu không kích hoạt Captcha
            if (!ServerConfig.Instance.EnableCaptcha)
            {
                /// Bỏ qua
                return;
            }
            /// Nếu không kích hoạt Captcha trong chiến trường
            if (!ServerConfig.Instance.EnableCaptchaForBattle)
            {
                /// Bỏ qua
                return;
            }
            /// Nếu có thiết lập không hiện Captcha trên thiết bị iOS
            if (!ServerConfig.Instance.EnableCaptchaForIOS)
            {
                /// Nếu đây là thiết bị iOS (phiên bản mới đã Update Client)
                if (this.DeviceModel.ToLower().Contains("iphone") || this.DeviceModel.ToLower().Contains("ipad"))
                {
                    /// Bỏ qua
                    return;
                }
                /// Nếu không phải thì DeviceModel phải rỗng (phiên bản cũ chưa Update Client)
                else if (this.DeviceModel == "")
                {
                    if (this.DeviceGeneration.ToLower().Contains("iphone") || this.DeviceGeneration.ToLower().Contains("ipad"))
                    {
                        /// Bỏ qua
                        return;
                    }
                }
            }

            /// Nếu có thẻ tháng thì thôi
            if (this.YKDetail.HasYueKa == 1)
            {
                /// Bỏ qua
                return;
            }
            
            /// Nếu chưa đến thời gian ra Captcha
            if (KTGlobal.GetCurrentTimeMilis() < this.NextCaptchaTicks)
            {
                return;
            }

            /// Thời gian hiện tại
            DateTime now = DateTime.Now;
            /// Nếu không phải thời gian hiện Captcha
            if (now < ServerConfig.Instance.CaptchaAppearFromTime || now > ServerConfig.Instance.CaptchaAppearToTime)
            {
                /// 10 giây sau sẽ hỏi lại
                this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + 10000;
                /// Bỏ qua
                return;
            }

            /// Nếu là phụ bản thì thôi
            if (this.CopyMapID != -1/* && CopySceneEventManager.IsCopySceneExist(this.CopyMapID, this.CurrentMapCode)*/)
            {
                /// 10 giây sau sẽ hỏi lại
                this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + 10000;
                /// Bỏ qua
                return;
            }

            /// Bản đồ hiện tại
            GameMap gameMap = GameManager.MapMgr.GetGameMap(this.CurrentMapCode);
            /// Nếu đang ở các bản đồ đặc biệt thì thôi
            if (gameMap.MapType == "village" || gameMap.MapType == "faction" || gameMap.MapType == "city" || gameMap.MapType == "factionbattle" || gameMap.MapType == "xisuidao" || gameMap.MapType == "xisuidao_shandong" || gameMap.MapType == "xiaoyaogubaomingdian" || gameMap.MapType == "liansaihuichang" || gameMap.MapType == "liansai" || gameMap.MapType == "xoyogame" || gameMap.MapType == "xoyogame_waitingroom")
            {
                /// 10 giây sau sẽ hỏi lại
                this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + 10000;
                /// Bỏ qua
                return;
            }

            /// Nếu chỉ kiểm tra trên người chơi có nhóm, nhưng bản thân lại không có nhóm
            if (ServerConfig.Instance.CaptchaTeamPlayersOnly && (this.TeamID == -1 || !KTTeamManager.IsTeamExist(this.TeamID) || KTTeamManager.GetTeamSize(this.TeamID) <= 1))
            {
                /// 10 giây sau sẽ hỏi lại
                this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + 10000;
                /// Bỏ qua
                return;
            }

            /// Đánh dấu thời điểm xuất hiện Captcha tiếp theo
            this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(ServerConfig.Instance.CaptchaAppearMinPeriod, ServerConfig.Instance.CaptchaAppearMaxPeriod);

            /// Hiện Captcha
            this.GenerateCaptcha();
        }

        /// <summary>
        /// Kích hoạt trận pháp của người chơi tương ứng cho bản thân
        /// </summary>
        /// <param name="fromPlayer"></param>
        private bool ActivateZhen(KPlayer fromPlayer)
		{
            /// Nếu không cùng bản đồ
            if (this.CurrentMapCode != fromPlayer.CurrentMapCode || this.CurrentCopyMapID != fromPlayer.CurrentCopyMapID)
			{
                return false;
			}

            /// Trận pháp của người chơi này là gì
            GoodsData zhenGD = null;
            if (fromPlayer.GoodsDataList != null)
            {
                lock (fromPlayer.GoodsDataList)
                {
                    zhenGD = fromPlayer.GoodsDataList.Where(x => x.Using == (int) KE_EQUIP_POSITION.emEQUIPPOS_ZHEN).FirstOrDefault();
                }
            }

            /// Nếu không có trận pháp thì thôi
            if (zhenGD == null)
			{
                return false;
			}

            /// Thông tin vật phẩm
            ItemData itemData = ItemManager.GetItemTemplate(zhenGD.GoodsID);

			/// Kỹ năng toàn bản đồ
            if (itemData.NearbyZhenSkill != -1)
			{
                /// Kỹ năng tương ứng
                SkillDataEx skillData = KSkill.GetSkillData(itemData.NearbyZhenSkill);
                /// Nếu kỹ năng tồn tại
                if (skillData != null)
				{
                    /// Tạo đối tượng kỹ năng
                    SkillLevelRef skill = new SkillLevelRef()
                    {
                        Data = skillData,
                        AddedLevel = /*fromPlayer == this ? 5 : */ItemManager.GetZhenLevel(this, itemData, false),
                    };

                    /// Buff tương ứng
                    BuffDataEx buff = new BuffDataEx()
                    {
                        Skill = skill,
                        Duration = 6000,
                        StackCount = 1,
                        StartTick = KTGlobal.GetCurrentTimeMilis(),
                        SaveToDB = false,
                    };

                    /// Kích hoạt Buff tương ứng
                    this.Buffs.AddBuff(buff);
				}
			}

            /// Kỹ năng lân cận
            if (itemData.ZhenSkillID != -1 && KTGlobal.GetDistanceBetweenPlayers(this, fromPlayer) <= 600)
            {
                /// Kỹ năng tương ứng
                SkillDataEx skillData = KSkill.GetSkillData(itemData.ZhenSkillID);
                /// Nếu kỹ năng tồn tại
                if (skillData != null)
                {
                    /// Tạo đối tượng kỹ năng
                    SkillLevelRef skill = new SkillLevelRef()
                    {
                        Data = skillData,
                        AddedLevel = /*fromPlayer == this ? 5 : */ItemManager.GetZhenLevel(this, itemData, true),
                    };

                    /// Buff tương ứng
                    BuffDataEx buff = new BuffDataEx()
                    {
                        Skill = skill,
                        Duration = 6000,
                        StackCount = 1,
                        StartTick = KTGlobal.GetCurrentTimeMilis(),
                        SaveToDB = false,
                    };

                    /// Kích hoạt Buff tương ứng
                    this.Buffs.AddBuff(buff);
                }
            }

            /// Kích hoạt thành công
            return true;
        }

        /// <summary>
        /// Thực hiện Logic trận pháp tổ đội
        /// </summary>
        private void ProcessTeamZhenLogic()
		{
            /// Nếu bản thân không có nhóm
            if (this.TeamID == -1 || !KTTeamManager.IsTeamExist(this.TeamID))
			{
                return;
			}

            /// Kích hoạt trận của bản thân
            bool ret = this.ActivateZhen(this);
            /// Nếu không có trận pháp
            if (!ret)
			{
                /// Nếu bản thân là đội trưởng thì thôi
                if (this.TeamLeader == this)
				{
                    return;
				}

                /// Trận pháp của đội trưởng
                this.ActivateZhen(this.TeamLeader);
			}
		}

        /// <summary>
        /// Thực hiện giảm thể lực khi chạy mà ở trạng thái đồ sát
        /// </summary>
        private void ProcessPKAllSubStamina()
        {
            /// Nếu đang chạy và trạng thái PK là đồ sát
            if (this.m_eDoing == KE_NPC_DOING.do_run && this.PKMode == (int)KiemThe.Entities.PKMode.All)
            {
                /// Bản đồ hiện tại
                GameMap map = GameManager.MapMgr.GetGameMap(this.CurrentMapCode);
                /// Thực hiện giảm thể lực
                this.m_CurrentStamina -= map.PKAllSubStamina;
                /// Nếu thể lực âm
                if (this.m_CurrentStamina < 0)
                {
                    this.m_CurrentStamina = 0;
                }
                /// Nếu đã hết thể lực
                if (this.m_CurrentStamina <= 0)
                {
                    /// Buộc đổi về trạng thái đi bộ
                    this.m_eDoing = KE_NPC_DOING.do_walk;
                    /// Cập nhật Storyboard
                    KTPlayerStoryBoardEx.Instance.ChangeAction(this, this.m_eDoing);
                    /// Thông báo về Client
                    KT_TCPHandler.NotifySpriteChangeAction(this);
                }
            }

            /// Nếu đang di chuyển
            if ((this.m_eDoing == KE_NPC_DOING.do_walk || this.m_eDoing == KE_NPC_DOING.do_run))
            {
                /// Nếu thể lực dưới 5%
                if (this.m_CurrentStamina * 100 / this.m_CurrentStaminaMax < 5)
                {
                    /// Nếu khác động tác hiện tại
                    if (this.m_eDoing != KE_NPC_DOING.do_walk)
                    {
                        /// Đổi về trạng thái đi bộ
                        this.m_eDoing = KE_NPC_DOING.do_walk;
                        /// Cập nhật Storyboard
                        KTPlayerStoryBoardEx.Instance.ChangeAction(this, this.m_eDoing);
                        /// Thông báo về Client
                        KT_TCPHandler.NotifySpriteChangeAction(this);
                    }
                }
                /// Nếu thể lực trên 5%
                else
                {
                    /// Nếu khác động tác hiện tại
                    if (this.m_eDoing != KE_NPC_DOING.do_run)
                    {
                        /// Đổi về trạng thái chạy
                        this.m_eDoing = KE_NPC_DOING.do_run;
                        /// Cập nhật Storyboard
                        KTPlayerStoryBoardEx.Instance.ChangeAction(this, this.m_eDoing);
                        /// Thông báo về Client
                        KT_TCPHandler.NotifySpriteChangeAction(this);
                    }
                }
            }
        }

        /// <summary>
        /// Thực thi trừ thời gian miễn nhiễm sát thương của người chơi khác
        /// </summary>
        private void ProcessRemoveImmuneAllDamagesOfOtherPlayers()
        {
            if (this.ImmuneToAllDamagesOf.Count <= 0)
            {
                return;
            }

            /// Danh sách cần xóa
            List<KPlayer> toRemoveList = null;
            /// Duyệt danh sách miễn nhiễm
            foreach (KeyValuePair<KPlayer, KPlayer_ImmuneToAllDamagesOf> pair in this.ImmuneToAllDamagesOf)
            {
                /// Nếu đã hết thời gian
                if (pair.Value.IsOver)
                {
                    if (toRemoveList == null)
                    {
                        toRemoveList = new List<KPlayer>();
                    }
                    /// Thêm vào danh sách cần xóa
                    toRemoveList.Add(pair.Key);
                }
            }

            /// Duyệt danh sách cần xóa
            if (toRemoveList != null)
            {
                foreach (KPlayer player in toRemoveList)
                {
                    this.RemoveImmuneToAllDamagesOf(player);
                }
            }
        }

        /// <summary>
        /// Thực thi trừ điểm sát khí
        /// </summary>
        private void ProcessSubPKValue()
        {
            /// Nếu không có sát khí
            if (this.PKValue <= 0)
            {
                return;
            }

            /// Nếu là phụ bản thì thôi
            if (this.CopyMapID != -1 && CopySceneEventManager.IsCopySceneExist(this.CopyMapID, this.CurrentMapCode))
            {
                return;
            }

            GameMap gameMap = GameManager.MapMgr.GetGameMap(this.CurrentMapCode);
            /// Nếu bản đồ không tự giảm sát khí sau mỗi khoảng
            if (gameMap.SubPKValueTick <= 0)
            {
                /// Thiết lập thời gian đếm lùi
                this.LastSiteSubPKPointTicks = KTGlobal.GetCurrentTimeMilis();
                return;
            }

            /// Nếu đã đến thời gian giảm sát khí
            if (KTGlobal.GetCurrentTimeMilis() - this.LastSiteSubPKPointTicks >= gameMap.SubPKValueTick)
            {
                this.PKValue--;
                this.LastSiteSubPKPointTicks = KTGlobal.GetCurrentTimeMilis();

                /// Thông báo cho người chơi
                PlayerManager.ShowNotification(this, string.Format("Sát khí giảm 1 điểm, xuống còn {0} điểm!", this.PKValue));
            }
        }

        /// <summary>
        /// Thực thi đếm lùi Progress
        /// </summary>
        private void ProcessProgressCountDown()
        {
            /// Nếu không thao tác gì
            if (this._CurrentProgress == null)
            {
                return;
            }

            /// Nếu đã hoàn tất thao tác
            if (this._CurrentProgress.Completed)
            {
                /// Thực hiện sự kiện hoàn tất
                this._CurrentProgress.Complete?.Invoke();
                /// Hủy Progress
                this._CurrentProgress = null;

                /// Gửi thông báo ngắt Progress về Client
                KT_TCPHandler.SendStopProgressBar(this);

                return;
            }

            /// Nếu đang thao tác mà dính các trạng thái đặc biệt không thể thao tác nữa thì hủy
            if (!this.IsCanDoLogic())
            {
                /// Thực thi sự kiện hủy bỏ
                this._CurrentProgress.Cancel?.Invoke();
                /// Hủy Progress
                this._CurrentProgress = null;

                /// Gửi thông báo ngắt Progress về Client
                KT_TCPHandler.SendStopProgressBar(this);

                return;
            }

            /// Thực thi sự kiện Tick
            this._CurrentProgress.Tick?.Invoke();
        }

        /// <summary>
        /// Thực hiện đếm lùi thời gian tỷ thí
        /// </summary>
        private void ProcessChallengeCountDown()
        {
            /// Nếu không có thông tin tỷ thí
            if (this.ChallengeInfo == null)
            {
                return;
            }
            /// Nếu không có đối tượng tỷ thí
            else if (this.ChallengeInfo.Target == null)
            {
                /// Ngừng tỷ thí
                this.StopChallenge();
                return;
            }
            /// Nếu đối phương đã rời mạng
            else if (this.ChallengeInfo.Target.LogoutState)
            {
                /// Ngừng tỷ thí
                this.StopChallenge();
                return;
            }
            /// Nếu đối phương khác bản đồ
            else if (this.ChallengeInfo.Target.CurrentMapCode != this.CurrentMapCode)
            {
                /// Ngừng tỷ thí
                this.StopChallenge();
                return;
            }
            /// Nếu đã hết thời gian tỷ thí
            else if (!this.ChallengeInfo.IsChallengeTime)
            {
                /// Ngừng tỷ thí
                this.StopChallenge();
                return;
            }
        }

        /// <summary>
        /// Thực hiện cập nhật thông tin nhóm
        /// </summary>
        private void ProcessUpdateTeamMemberInfo()
        {
            /// Nếu chưa đến thời gian
            if (this.LifeTime % 5000 != 0)
            {
                return;
            }

            /// Nếu nhóm có tồn tại và có từ 1 thành viên trở lên
            if (this.TeamID != -1 && KTTeamManager.IsTeamExist(this.TeamID) && KTTeamManager.GetTeamSize(this.TeamID) >= 1)
            {
                KT_TCPHandler.SendRefreshTeamMemberAttrib(this);
            }
        }

        /// <summary>
        /// Rời nhóm hiện tại
        /// </summary>
        public void LeaveTeam()
        {
            /// Nếu nhóm không tồn tại
            if (this.TeamID == -1 || !KTTeamManager.IsTeamExist(this.TeamID))
            {
                return;
            }

            /// ID nhóm cũ
            int oldTeamID = this.TeamID;

            /// Nhường ghế đội trưởng cho người chơi khác
            List<KPlayer> teammates = this.Teammates;
            /// Nếu đội trống
            if (teammates.Count <= 1)
            {
                /// Xóa người chơi khỏi nhóm tương ứng
                KTTeamManager.LeaveTeam(this);
                /// Xóa đội tương ứng
                KTTeamManager.RetainTeam(this);
                /// Gửi gói tin thông báo bản thân rời nhóm
                GameManager.ClientMgr.SendToClient(this, "", (int) TCPGameServerCmds.CMD_KT_LEAVETEAM);
            }
            /// Nếu đội vẫn còn thành viên khác và bản thân là đội trưởng
            else if (this.TeamLeader == this)
            {
                /// Chọn người chơi đầu tiên cho lên làm đội trưởng
                KPlayer firstPlayer = null;
                foreach (KPlayer teammate in teammates)
                {
                    if (teammate != null && teammate != this)
                    {
                        firstPlayer = teammate;
                        break;
                    }
                }
                if (firstPlayer != null)
                {
                    KTTeamManager.ApproveTeamLeader(firstPlayer);

                    /// Gửi gói tin thông báo bổ nhiệm đội trưởng nhóm
                    foreach (KPlayer teammate in teammates)
                    {
                        if (teammate != null && teammate != this)
                        {
                            string strCmd = string.Format("{0}:{1}", firstPlayer.RoleID, firstPlayer.RoleName);
                            GameManager.ClientMgr.SendToClient(teammate, strCmd, (int)TCPGameServerCmds.CMD_KT_APPROVETEAMLEADER);
                        }
                    }

                    /// Gửi gói tin đến tất cả người chơi xung quanh đối tượng thông báo tổ đội thay đổi
                    KT_TCPHandler.SendSpriteTeamChangedToAllOthers(firstPlayer);
                }
                /// Xóa người chơi khỏi nhóm tương ứng
                KTTeamManager.LeaveTeam(this);
                /// Gửi gói tin thông báo bản thân rời nhóm
                GameManager.ClientMgr.SendToClient(this, "", (int) TCPGameServerCmds.CMD_KT_LEAVETEAM);
            }
            else
            {
                /// Xóa người chơi khỏi nhóm tương ứng
                KTTeamManager.LeaveTeam(this);
                /// Gửi gói tin thông báo bản thân rời nhóm
                GameManager.ClientMgr.SendToClient(this, "", (int) TCPGameServerCmds.CMD_KT_LEAVETEAM);
            }

            /// Gửi thông tin có thành viên rời nhóm
            foreach (KPlayer teammate in KTTeamManager.GetTeamPlayers(oldTeamID))
            {
                if (teammate != null && teammate != this)
                {
                    TeamMemberChanged teamMember = new TeamMemberChanged()
                    {
                        RoleID = this.RoleID,
                        RoleName = this.RoleName,
                        Type = 0,
                    };
                    byte[] _cmdData = DataHelper.ObjectToBytes<TeamMemberChanged>(teamMember);
                    GameManager.ClientMgr.SendToClient(teammate, _cmdData, (int)TCPGameServerCmds.CMD_KT_TEAMMEMBERCHANGED);
                }
            }

            /// Gửi gói tin đến tất cả người chơi xung quanh đối tượng thông báo tổ đội thay đổi
            KT_TCPHandler.SendSpriteTeamChangedToAllOthers(this);
        }

        /// <summary>
        /// Đưa người chơi vào nhà lao
        /// </summary>
        public void SendToJail()
        {
            /// Nếu bản đồ hiện tại trùng với bản đồ nhà lao
            if (this.CurrentMapCode == KTGlobal.JailMapCode)
            {
                return;
            }

            /// Dịch chuyển đến nhà lao
            GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this, KTGlobal.JailMapCode, KTGlobal.JailPosX, KTGlobal.JailPosY, (int)this.CurrentDir);
        }

        /// <summary>
        /// Kích hoạt lại vòng sáng, kỹ năng bị động và kỹ năng hỗ trợ
        /// </summary>
        public void ReactivateAuraPassiveAndEnchantSkills()
        {
            /// Thực thi kỹ năng bị động
            this.Skills.ProcessPassiveSkills();

            /// Thực hiện hỗ trợ sát thương kỹ năng khác
            this.Skills.ProcessPassiveSkillsAppendDamages();

            /// Thực hiện kỹ năng hỗ trợ kỹ năng khác
            this.Skills.ProcessEnchantSkills();

            /// Làm mới danh sách kỹ năng
            KT_TCPHandler.SendRenewSkillList(this);

            /// Gửi tín hiệu về Client thông báo chỉ số đã thay đổi
            KT_TCPHandler.NotifyAttribute(this);
        }

        /// <summary>
        /// Vô hiệu hóa vòng sáng, kỹ năng bị động và kỹ năng hỗ trợ
        /// </summary>
        public void DeactivateAuraPassiveAndEnchantSkills()
        {
            /// Xóa kỹ năng bị động
            this.Skills.RemovePassiveSkillEffects();

            /// Xóa vòng sáng
            this.Buffs.RemoveAllAruas();

            /// Làm rỗng danh sách hỗ trợ sát thương kỹ năng khác
            this.Skills.ClearPassiveSkillAppendDamages();

            /// Làm rỗng danh sách kỹ năng hỗ trợ kỹ năng khác
            this.Skills.ClearEnchantSkills();
        }

        /// <summary>
        /// Vô hiệu hóa và kích hoạt lại vòng sáng, kỹ năng bị động và kỹ năng hỗ trợ
        /// </summary>
        public void DeactivateAndReactivateAuraPassiveAndEnchantSkills()
        {
            /// Mức máu hiện tại
			int currentHP = this.m_CurrentLife;
            /// Mức khí hiện tại
            int currentMP = this.m_CurrentMana;
            /// Mức thể lực hiện tại
            int currentStamina = this.m_CurrentStamina;

            /// ID kỹ năng vòng sáng trước
            int lastAuraSkill = -1;
            /// Nếu có vòng sáng đang kích hoạt
            if (this.Buffs.CurrentArua != null)
            {
                lastAuraSkill = this.Buffs.CurrentArua.Skill.SkillID;
            }

            this.DeactivateAuraPassiveAndEnchantSkills();
            this.ReactivateAuraPassiveAndEnchantSkills();

            /// Nếu có kỹ năng vòng sáng hiện tại
			if (lastAuraSkill != -1 && this.Skills.HasSkill(lastAuraSkill))
            {
                /// Thực hiện kỹ năng vòng sáng
                KTSkillManager.UseSkill(this, this, this.Skills.GetSkillLevelRef(lastAuraSkill), true);
            }

            /// Cập nhật mức máu
            this.m_CurrentLife = Math.Min(this.m_CurrentLifeMax, currentHP);
            /// Cập nhật mức khí
            this.m_CurrentMana = Math.Min(this.m_CurrentManaMax, currentMP);
            /// Cập nhật mức thể lực
            this.m_CurrentStamina = Math.Min(this.m_CurrentStaminaMax, currentStamina);
        }

        #endregion Logic

        #region Core

        /// <summary>
        /// Hàm này gọi tới khi người chơi chuẩn bị chuyển cảnh
        /// </summary>
        /// <param name="toMap"></param>
        public void OnPreChangeMap(GameMap toMap)
        {
            this.SendChangeMapProtectionBuff();
            this.StopAllActiveFights();

            /// Nếu đang ở bản đồ thi đấu môn phái
            if (FactionBattleManager.IsInFactionBattle(this))
            {
                /// Thực thi sự kiện
                FactionBattleManager.OnPlayerLeave(this, toMap);
                return;
            }
            /// Nếu đang ở bản đồ Võ lâm liên đấu
            else if (TeamBattle.IsInTeamBattleMap(this))
            {
                /// Thực thi sự kiện
                TeamBattle_ActivityScript.OnPlayerLeave(this, toMap);
                return;
            }

            /// Nếu đang trong phụ bản
            if (this.CopyMapID != -1 && CopySceneEventManager.IsCopySceneExist(this.CopyMapID, this.CurrentMapCode))
            {
                /// Phụ bản tương ứng
                KTCopyScene copyScene = CopySceneEventManager.GetCopyScene(this.CopyMapID, this.CurrentMapCode);
                /// Thực hiện hàm OnPlayerLeave
                CopySceneEventManager.OnPlayerLeave(copyScene, this, toMap);
            }
            else
            {
                GameMap gameMap = GameManager.MapMgr.GetGameMap(this.CurrentMapCode);
                /// Kích hoạt sự kiện người chơi rời bản đồ hoạt động
                GameMapEventsManager.OnPlayerLeave(this, toMap);


                GuidWarManager.getInstance().OnLeaverMap(this, toMap);
            }
        }

        /// <summary>
        /// Hàm này gọi tới khi người chơi chuyển cảnh
        /// </summary>
        /// <param name="oldMap"></param>
        public void OnChangeMap(GameMap oldMap)
        {
        }

        /// <summary>
        /// Hàm này gọi tới khi người chơi vào bản đồ
        /// </summary>
        public void OnEnterMap()
        {
            /// Reset thời gian chờ giảm trạng thái PK, tránh trường hợp cố tình chuyển vào bản đồ có thời gian ngắn để trừ
            this.LastSiteSubPKPointTicks = KTGlobal.GetCurrentTimeMilis();

            GameMap currentMap = GameManager.MapMgr.GetGameMap(this.CurrentMapCode);

            /// Nếu bản đồ không chấp nhận tổ đội và bản thân thì lại có nhóm
            if (!currentMap.AllowTeam && this.TeamID != -1 && KTTeamManager.IsTeamExist(this.TeamID))
            {
                this.LeaveTeam();
            }

            /// Nếu bản đồ buộc người chơi chuyển trạng thái PK về dạng tương ứng
            if (currentMap.ForceChangePKStatusTo != -1 && currentMap.ForceChangePKStatusTo >= (int)KiemThe.Entities.PKMode.Peace && currentMap.ForceChangePKStatusTo < (int)KiemThe.Entities.PKMode.Count)
            {
                this.PKMode = currentMap.ForceChangePKStatusTo;
                this.StopAllActiveFights();
            }
            /// Nếu là thành thị, thôn, phái thì chuyển Camp về -1 và xóa toàn bộ trạng thái tuyên chiến
            else if (currentMap.MapType == "city" || currentMap.MapType == "village" || currentMap.MapType == "faction")
			{
                this.Camp = -1;
                this.StopAllActiveFights();
                /// Nếu đang là PK đặc biệt
                if (this.PKMode == (int) KiemThe.Entities.PKMode.Custom)
				{
                    /// Chuyển về PK hòa bình
                    this.PKMode = (int) KiemThe.Entities.PKMode.Peace;
				}
			}

            /// Kích hoạt sự kiện khi người chơi vào bản đồ thành công
            GlobalEventSource.getInstance().fireEvent(new PlayerEnterMap(this, this.MapCode));
            /// Nếu đang trong phụ bản
            if (this.CopyMapID != -1 && CopySceneEventManager.IsCopySceneExist(this.CopyMapID, this.CurrentMapCode))
            {
                /// Phụ bản tương ứng
                KTCopyScene copyScne = CopySceneEventManager.GetCopyScene(this.CopyMapID, this.CurrentMapCode);
                /// Thực hiện hàm OnPlayerLeave
                CopySceneEventManager.OnPlayerEnter(copyScne, this);
            }
            else
            {
                /// Nếu sát khí quá lớn
                if (this.PKValue >= KTGlobal.MaxPKValueToForceSendToJail && this.CurrentMapCode != KTGlobal.JailMapCode)
                {
                    /// Thông báo
                    PlayerManager.ShowNotification(this, "Sát khí quá cao, tự chuyển vào nhà lao xám hối!");
                    /// Chuyển vào nhà lao
                    this.SendToJail();
                    return;
                }

                GuidWarManager.getInstance().OnEnterMap(this);
                /// Kích hoạt sự kiện người chơi vào bản đồ hoạt động
                GameMapEventsManager.OnPlayerEnter(this);
            }


        }

        /// <summary>
        /// Hàm này gọi tới khi người chơi kết nối lại
        /// </summary>
        public void OnReconnect()
        {
            /// Đánh dấu đối tượng Online
            this.LogoutState = false;

            /// Khởi tạo SkillTree
            this.Skills = new SkillTree(this);

            /// Khởi tạo BuffTree
            this.Buffs = new BuffTree(this);

            /// Khởi tạo môn phái của người chơi
            this.m_cPlayerFaction = new KPlayerFaction(this, this._RoleDataEx.FactionID, this._RoleDataEx.SubID);

            /// Thực thi kỹ năng bị động
            this.ReactivateAuraPassiveAndEnchantSkills();
        }

        /// <summary>
        /// Hàm này gọi tới khi người chơi mất kết nối tới máy chủ
        /// </summary>
        public void OnDisconnected()
        {
            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.Skills.ExportSkillTree();

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.Buffs.ExportBuffTree();

            /// Ghi dữ liệu Skill vào DB
            GameDb.ProcessDBSkillCmdByTicks(this, true);

            /// Ghi dữ liệu kỹ năng thiết lập vào ô kỹ năng nhanh vào DB
            KT_TCPHandler.SendSaveQuickKeyToDB(this);
            KT_TCPHandler.SendSaveAruaKeyToDB(this);

            /// Ghi dữ liệu Buff vào DB
            KTLogic.UpdateAllDBBufferData(this);

            /// Xóa toàn bộ Buff và dừng luồng thực thi tương ứng
            this.Buffs.RemoveAllBuffs(false, false);
            this.Buffs.RemoveAllAruas(false, false);
            this.Buffs.RemoveAllAvoidBuffs();

            this.DeactivateAuraPassiveAndEnchantSkills();

            /// Ngừng StoryBoard thực hiện di chuyển
            KTPlayerStoryBoardEx.Instance.Remove(this);
            /// Xóa luồng trúng độc
            KTPoisonTimerManager.Instance.RemovePoisonState(this);

            /// Ngừng tỷ thí
            this.StopChallenge();
            /// Ngừng tuyên chiến với toàn bộ người chơi khác
            this.StopAllActiveFights();
        }

        /// <summary>
        /// Check xem người chơi này còn online hay không
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            return this.ClientSocket.Connected;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsInSongJin()
        {
            if (Battel_SonJin_Manager.IsInBattle(this))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInFacetionBattleJin()
        {
            if (FactionBattleManager.IsInFactionBattle(this))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm này gọi tới khi người chơi đăng nhập vào game (ở CMD_INIT_GAME)
        /// </summary>
        public void OnEnterGame()
        {
            /// Thêm vào danh sách quản lý
            KTPlayerTimerManager.Instance.AddPlayer(this);
            /// Thực hiện tạo mới kỹ năng và Buff
            this.OnReconnect();
        }

        /// <summary>
        /// Hàm này gọi đến khi người chơi thoát Game
        /// </summary>
        public void OnQuitGame()
        {
            /// Xóa khỏi danh sách quản lý
            KTPlayerTimerManager.Instance.RemovePlayer(this);
            /// Thực hiện lưu kỹ năng và xóa các Buff không lưu vào DB
            this.OnDisconnected();
        }

        /// <summary>
        /// Hàm này gọi tới trước khi có sự thay đổi môn phái của người chơi
        /// </summary>
        public void OnPreFactionChanged()
        {
            /// Xóa kỹ năng bị động và kỹ năng hỗ trợ
            this.DeactivateAuraPassiveAndEnchantSkills();
        }

        /// <summary>
        /// Hàm này gọi tới khi có sự thay đổi môn phái của người chơi
        /// </summary>
        public void OnFactionChanged()
        {
            /// Kiểm tra nhiệm vụ vào phái
            ProcessTask.Process(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this, -1, -1, -1, TaskTypes.JoinFaction);

            this._RoleDataEx.FactionID = this.m_cPlayerFaction.GetFactionId();
            this._RoleDataEx.SubID = this.m_cPlayerFaction.GetRouteId();

            /// Lưu môn phái và nhánh vào DB
            KT_TCPHandler.SaveFactionAndRouteToDBServer(this);

            /// Đổ dữ liệu về RoleDataEx và lưu vào DB
            this.Skills.ExportSkillTree();

            /// Thực thi kỹ năng bị động và kỹ năng hỗ trợ
            this.ReactivateAuraPassiveAndEnchantSkills();

            /// Thay đổi QuickKey và AruaKey
            this.MainQuickBarKeys = "-1|-1|-1|-1|-1|-1|-1|-1|-1|-1";
            this.OtherQuickBarKeys = "-1_0";
            KT_TCPHandler.SendSaveQuickKeyToDB(this);
            KT_TCPHandler.SendSaveAruaKeyToDB(this);
        }

        /// <summary>
        /// Hàm này gọi tới khi có sự thay đổi điểm cộng vào kỹ năng của người chơi
        /// </summary>
        public void RefreshSkillPoints()
        {
            int point = this.m_nBaseSkillPoints + this.m_nBonusSkillPoints - this.Skills.AddedSkillPoints;
            if (point < 0)
            {
                PlayerManager.ShowNotification(this, "Có lỗi xảy ra, toàn bộ điểm kỹ năng được làm mới lại từ đầu!");
                /// Xóa nhánh
                this.m_cPlayerFaction.ChangeFactionRoute(GameDataDef.KD_FACTION_ROUTE_NONE);
                this._RoleDataEx.SubID = this.m_cPlayerFaction.GetRouteId();
                /// Tẩy điểm kỹ năng nhánh đã được phân phối
                this.m_cPlayerFaction.ResetFactionSkillsLevel();

                /// Đổ dữ liệu về RoleDataEx và lưu vào DB
                this.Skills.ExportSkillTree();

                /// Cập nhật lại vòng sáng, kỹ năng bị động và kỹ năng hỗ trợ
                this.DeactivateAndReactivateAuraPassiveAndEnchantSkills();

                /// Thay đổi QuickKey và AruaKey
                this.MainQuickBarKeys = "-1|-1|-1|-1|-1|-1|-1|-1|-1|-1";
                this.OtherQuickBarKeys = "-1_0";
                KT_TCPHandler.SendSaveQuickKeyToDB(this);
                KT_TCPHandler.SendSaveAruaKeyToDB(this);
            }
        }

        /// <summary>
        /// Hàm này gọi đến khi cấp độ nhân vật thay đổi
        /// <para>Sẽ có 2 trường hợp:</para>
        /// <para>1. Nếu cấp độ lớn hơn cấp độ trước, thì sẽ cộng thêm lượng điểm tiềm năng có được từ cấp cũ cho đến cấp hiện tại</para>
        /// <para>2. Nếu cấp độ nhỏ hơn cấp độ trước, thì sẽ tiến hành tẩy điểm tiềm năng</para>
        /// </summary>
        public void OnLevelChanged()
        {
            #region Tiềm năng

            /// Tổng điểm tiềm năng mới theo cấp có từ Base
            this.m_nBaseRemainPotential = KPlayerSetting.GetLevelPotential(this.m_Level);

            int str = 0;
            int dex = 0;
            int vit = 0;
            int ene = 0;

            if (!this.CheckAssignPotential(ref str, ref dex, ref vit, ref ene))
            {
                this.UnAssignPotential();
            }

            #endregion Tiềm năng

            #region Kỹ năng

            /// Tổng điểm kỹ năng mới theo cấp có từ Base
            this.m_nBaseSkillPoints = KPlayerSetting.GetLevelFightSkillPoint(this.m_Level);

            this.RefreshSkillPoints();

            /// Gửi tín hiệu thay đổi danh sách kỹ năng về Client
            KT_TCPHandler.SendRenewSkillList(this);

            #endregion Kỹ năng

            #region Task Tuần Hoàn

            // Khi người chơi thăng cấp Refresh lại nhiệm vụ nghĩa quân
            TaskDailyArmyManager.getInstance().GiveTaskArmyDaily(this);
            // Khi người chơi thăng cấp Refresh lại hải tặc
            PirateTaskManager.getInstance().GiveTaskPirate(this);

            TaskManager.getInstance().ComputeNPCTaskState(this);

            #endregion Task Tuần Hoàn
        }

        /// <summary>
        /// Hàm này gọi đến khi người chơi được hồi sinh
        /// </summary>
        public void OnRevive()
        {
            /// Kích hoạt Buff bảo vệ 2 phút
            this.SendChangeMapProtectionBuff();

            /// Hủy Buff hồi sinh
            this.m_sReviveBuff = null;

            /// Thực thi kỹ năng bị động
            this.Skills.ProcessPassiveSkills();
        }

        /// <summary>
        /// Sự kiện khi người chơi bị tấn công
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="nDamage"></param>
        public override void OnBeHit(GameObject attacker, int nDamage)
        {
            /// Thực hiện phương thức cũ
            base.OnBeHit(attacker, nDamage);

			///// Nếu đối phương là người chơi
			//if (attacker is KPlayer)
			//{
			//	KPlayer attackerPlayer = attacker as KPlayer;

			//	//  LogManager.WriteLog(LogTypes.Chat, string.Format("{0} (PKMode: {1}, Camp: {2}) is attacking {3} (PKMode: {4}, Camp: {5})", attacker.RoleName, (KiemThe.Entities.PKMode) attackerPlayer.PKMode, attacker.Camp, this.RoleName, (KiemThe.Entities.PKMode) this.PKMode, this.Camp));

			//	/// Nếu không phải đang tỷ thí và trạng thái PK không đặc biệt
			//	if (!attackerPlayer.IsChallengeWith(this) && attackerPlayer.PKMode != (int) KiemThe.Entities.PKMode.Custom && this.PKMode != (int) KiemThe.Entities.PKMode.Custom)
			//	{
			//		/// Nếu chưa tuyên chiến
			//		if (!this.IsActiveFightWith(attackerPlayer))
			//		{
			//			/// Tuyên chiến
			//			attackerPlayer.StartActiveFight(this, false);
			//		}
			//	}
			//}

			/// Kiểm tra nếu có Progress thì hủy bỏ
			if (this._CurrentProgress != null && this._CurrentProgress.InteruptIfTakeDamage)
            {
                this.CurrentProgress = null;
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi đánh trúng đối tượng khác
        /// </summary>
        /// <param name="target"></param>
        /// <param name="nDamage"></param>
        public override void OnHitTarget(GameObject target, int nDamage)
        {
            base.OnHitTarget(target, nDamage);

            // CALL SANG THI ĐẤU MÔN PHÁI
            FactionBattleManager.DamgeRecore(this, target, nDamage);

            /// Nếu đang ở trong phụ bản
            if (CopySceneEventManager.IsCopySceneExist(this.CurrentCopyMapID, this.CurrentMapCode) && this.CurrentCopyMapID == this.CurrentCopyMapID)
            {
                /// Thực thi sự kiện người chơi chết ở phụ bản
                CopySceneEventManager.OnHitTarget(CopySceneEventManager.GetCopyScene(this.CurrentCopyMapID, this.CurrentMapCode), this, target, nDamage);
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi chết
        /// </summary>
        /// <param name="attacker"></param>
        public override void OnDie(GameObject attacker)
        {
            base.OnDie(attacker);

            // CALL SANG THI ĐẤU MÔN PHÁI
            FactionBattleManager.OnDie(attacker, this);


            GuidWarManager.getInstance().OnDie(attacker, this);

            /// Nếu đang ở trong phụ bản
            if (CopySceneEventManager.IsCopySceneExist(this.CurrentCopyMapID, this.CurrentMapCode) && this.CurrentCopyMapID == attacker.CurrentCopyMapID)
            {
                /// Thực thi sự kiện người chơi chết ở phụ bản
                CopySceneEventManager.OnPlayerDie(CopySceneEventManager.GetCopyScene(this.CurrentCopyMapID, this.CurrentMapCode), attacker, this);
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi giết đối tượng khác
        /// </summary>
        /// <param name="deadObj"></param>
        public override void OnKillObject(GameObject deadObj)
        {
            /// Nếu đang ở trong phụ bản
            if (CopySceneEventManager.IsCopySceneExist(this.CurrentCopyMapID, this.CurrentMapCode) && this.CurrentCopyMapID == deadObj.CurrentCopyMapID)
            {
                /// Thực thi sự kiện giết đối tượng ở phụ bản
                CopySceneEventManager.OnKillObject(CopySceneEventManager.GetCopyScene(this.CurrentCopyMapID, this.CurrentMapCode), this, deadObj);
            }
            else
            {
                /// Thực thi sự kiện giết đối tượng khác trong bản đồ hoạt động
                GameMapEventsManager.OnKillObject(this, deadObj);
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi nhặt vật phẩm rơi dưới đất
        /// </summary>
        /// <param name="goodsPackItem"></param>
        public void OnPickUpItem(GoodsPackItem goodsPackItem)
		{
            /// Nếu trong danh sách thông báo khi nhặt có tồn tại
            if (ItemDropManager.PickUpItemNotify.Items.TryGetValue(goodsPackItem.ItemInPacker.GoodsID, out PickUpItemNotify.Item data))
			{
                /// Vật phẩm tương ứng
                ItemData itemData = ItemManager.GetItemTemplate(goodsPackItem.ItemInPacker.GoodsID);

                /// Thông tin vật phẩm
                string itemInfoStr = "";
                /// Nếu là đồ phải rơi ra từ boss
                if (data.BossOnly)
				{
                    /// Nếu đây đúng là vật phẩm rơi ra từ Boss
                    if (goodsPackItem.DropFromBoss)
					{
                        itemInfoStr = itemData.Name; //KTGlobal.GetItemNameWithHTMLColor(goodsPackItem.ItemInPacker);
					}
				}
                /// Nhặt từ nguồn khác
				else
				{
                    itemInfoStr = itemData.Name; //KTGlobal.GetItemNameWithHTMLColor(goodsPackItem.ItemInPacker);
                }

                /// Nếu có thông tin vật phẩm
                if (!string.IsNullOrEmpty(itemInfoStr))
				{
                    /// Tên bản đồ
                    string mapName = GameManager.MapMgr.GetGameMap(this.CurrentMapCode).MapName;

                    /// Nếu có tổ đội
                    if (this.TeamID != -1 && KTTeamManager.IsTeamExist(this.TeamID))
					{
                        string notify = string.Format("<color=#cb8fff>Đồng đội <color=#00ff2a>[{0}]</color> tại <color=yellow>[{1}]</color> đã nhặt được {2}.</color>", this.RoleName, mapName, itemInfoStr);
                        KTGlobal.SendTeamChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this.TeamID, notify, null, "");
                    }

                    /// Nếu có bang hội
                    if (this.GuildID > 0)
					{
                        string notify = string.Format("<color=#8fd8ff>Thành viên bang hội <color=#00ff2a>[{0}]</color> tại <color=yellow>[{1}]</color> đã nhặt được {2}.</color>", this.RoleName, mapName, itemInfoStr);
                        KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this.GuildID, notify, null, "");
                    }
					/// Nếu có gia tộc
					else if (this.FamilyID > 0)
					{
                        string notify = string.Format("<color=#ffbc8f>Thành viên gia tộc <color=#00ff2a>[{0}]</color> tại <color=yellow>[{1}]</color> đã nhặt được {2}.</color>", this.RoleName, mapName, itemInfoStr);
                        KTGlobal.SendFamilyChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this.FamilyID, notify, null, "");
                    }
                }
			}
        }
        #endregion Core

        #region Tính năng

        /// <summary>
        /// Hàm này gọi đến khi người chơi vào nhóm
        /// </summary>
        public void OnJoinTeam()
        {
            /// Duyệt danh sách thành viên nhóm
            foreach (KPlayer teammate in this.Teammates)
			{
                /// Xóa trạng thái tuyên chiến
                this.StopActiveFight(teammate, true);
			}
        }

        /// <summary>
        /// Hàm này gọi đến khi người chơi thoát khỏi nhóm
        /// </summary>
        public void OnLeaveTeam()
        {

        }

        #endregion Tính năng

        #region Kỹ năng

        /// <summary>
        /// Thực hiện kỹ năng tự động gọi nếu không sử dụng kỹ năng
        /// </summary>
        private void ProcessAutoSkillActivateEachSeconds()
        {
            /// Duyệt danh sách các kỹ năng tự kích hoạt
            foreach (KNpcAutoSkill autoSkill in this.GetListAutoSkills())
            {
                /// Nếu đây là loại kỹ năng kích hoạt liên tục mỗi khoảng thời gian
                if (!autoSkill.IsCoolDown && autoSkill.Info.ActivateEachFrame != -1 && KTGlobal.GetCurrentTimeMilis() - autoSkill.LastActivateTick >= autoSkill.Info.ActivateEachFrame * 1000 / 18)
                {
                    this.ActivateAutoSkill(autoSkill, this);
                }
                /// Nếu đây là loại kỹ năng kích hoạt nếu không sử dụng kỹ năng trong khoảng thời gian nhất định
                else if (!autoSkill.IsCoolDown && autoSkill.Info.ActivateIfNoUseSkillForFrame != -1 && KTGlobal.GetCurrentTimeMilis() - this.LastAttackTicks >= autoSkill.Info.ActivateIfNoUseSkillForFrame * 1000 / 18)
                {
                    this.ActivateAutoSkill(autoSkill, this);
                }
                /// Nếu đây là loại kỹ năng kích hoạt nếu không sử dụng kỹ năng làm mất trạng thái tàng hình trong khoảng thời gian nhất định
                else if (!autoSkill.IsCoolDown && autoSkill.Info.ActivateIfNoUseSkillCauseLostInvisibilityForFrame != -1 && KTGlobal.GetCurrentTimeMilis() - this.m_LastUseSkillCauseLosingInvisibleTick >= autoSkill.Info.ActivateIfNoUseSkillCauseLostInvisibilityForFrame * 1000 / 18)
                {
                    this.ActivateAutoSkill(autoSkill, this);
                }
            }
        }

        /// <summary>
        /// Thực thi xử lý đồng bộ sát thương về Client
        /// </summary>
        private void ProcessSynsDamage()
        {
            /// Nếu không có sát thương
            if (this.totalDamageDealtThisFrame.Count <= 0 && this.totalReceivedDamagesThisFrame.Count <= 0)
            {
                return;
            }

            /// Đối tượng gom nhóm Packet
            SkillResultPacketBuilder packetBuilder = new SkillResultPacketBuilder();

            /// Nếu có sát thương gây ra
            if (this.totalDamageDealtThisFrame.Count > 0)
            {
                /// Tạo mới từ điển chứa sát thương để cộng dồn
                Dictionary<GameObject, SpriteDamageInfo> damageInfo = new Dictionary<GameObject, SpriteDamageInfo>();

                /// Chừng nào hàng đợi chưa rỗng
                while (!this.totalDamageDealtThisFrame.IsEmpty)
                {
                    /// Nếu lấy ra thành công
                    if (this.totalDamageDealtThisFrame.TryDequeue(out SpriteDamageQueue result))
                    {
                        /// Thêm vào từ điển
                        if (!damageInfo.ContainsKey(result.Object))
                        {
                            damageInfo[result.Object] = new SpriteDamageInfo()
                            {
                                Object = result.Object,
                                Damages = new Dictionary<KTSkillManager.SkillResult, int>(),
                            };
                        }
                        /// Nếu loại damage này chưa tồn tại
                        if (!damageInfo[result.Object].Damages.ContainsKey(result.Type))
                        {
                            /// Thêm vào từ điển
                            damageInfo[result.Object].Damages[result.Type] = result.Damage;
                        }
                        /// Nếu đã tồn tại thì cộng dồn
                        else
                        {
                            damageInfo[result.Object].Damages[result.Type] += result.Damage;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                /// Duyệt danh sách
                foreach (SpriteDamageInfo info in damageInfo.Values)
                {
                    /// Duyệt danh sách sát thương con thêm vào gói tin
                    foreach (KeyValuePair<KTSkillManager.SkillResult, int> pair in info.Damages)
                    {
                        packetBuilder.Append(this, info.Object, pair.Key, pair.Value);
                    }
                }

                /// Làm rỗng từ điển
                damageInfo.Clear();
            }

            /// Nếu có sát thương phải nhận
            if (this.totalReceivedDamagesThisFrame.Count > 0)
            {
                /// Tạo mới từ điển chứa sát thương để cộng dồn
                Dictionary<GameObject, SpriteDamageInfo> damageInfo = new Dictionary<GameObject, SpriteDamageInfo>();

                /// Chừng nào hàng đợi chưa rỗng
                while (!this.totalReceivedDamagesThisFrame.IsEmpty)
                {
                    /// Nếu lấy ra thành công
                    if (this.totalReceivedDamagesThisFrame.TryDequeue(out SpriteDamageQueue result))
                    {
                        /// Thêm vào từ điển
                        if (!damageInfo.ContainsKey(result.Object))
                        {
                            damageInfo[result.Object] = new SpriteDamageInfo()
                            {
                                Object = result.Object,
                                Damages = new Dictionary<KTSkillManager.SkillResult, int>(),
                            };
                        }
                        /// Nếu loại damage này chưa tồn tại
                        if (!damageInfo[result.Object].Damages.ContainsKey(result.Type))
                        {
                            /// Thêm vào từ điển
                            damageInfo[result.Object].Damages[result.Type] = result.Damage;
                        }
                        /// Nếu đã tồn tại thì cộng dồn
                        else
                        {
                            damageInfo[result.Object].Damages[result.Type] += result.Damage;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                /// Duyệt danh sách
                foreach (SpriteDamageInfo info in damageInfo.Values)
                {
                    /// Duyệt danh sách sát thương con thêm vào gói tin
                    foreach (KeyValuePair<KTSkillManager.SkillResult, int> pair in info.Damages)
                    {
                        packetBuilder.Append(info.Object, this, pair.Key, pair.Value);
                    }
                }

                /// Làm rỗng từ điển
                damageInfo.Clear();
            }

            /// Nếu có danh sách sát thương
            if (packetBuilder.Count > 0)
            {
                /// Gửi gói tin về Client
                KT_TCPHandler.SendSkillResultsToMySelf(this as KPlayer, packetBuilder.Build());
                /// Hủy đối tượng
                packetBuilder.Dispose();
            }
        }

        /// <summary>
        /// Kiểm tra các kẻ địch xung quanh và kích hoạt Buff loại này
        /// </summary>
        /// <param name="go">Đối tượng</param>
        public void CheckEnemiesAroundAndActivateBuff()
        {
            /// Nếu đối tượng đã chết thì bỏ qua
            if (this.IsDead())
            {
                return;
            }

            /// Danh sách người chơi xung quanh
            List<KPlayer> players = this.NearbyPlayers.Values.ToList();
            /// Số kẻ địch xung quanh
            int enemiesCount = players.Where(x => KTLogic.IsOpposite(this, x)).Count();

            /// Kỹ năng bị động với mỗi mục tiêu xung quanh
            SkillLevelRef skill = this.Skills.GetSkillLevelRef(this.m_sAddedWithEnemy_OwnerSkillID);
            /// Nếu không tồn tại kỹ năng bị động này
            if (skill == null || skill.Level <= 0)
            {
                return;
            }

            /// Cấp độ kỹ năng con gọi đến
            int childSkillLevel = this.m_sAddedWithEnemy_SkillLevel;
            /// Nếu cấp độ kỹ năng con nhỏ hơn 0 thì bỏ qua
            if (childSkillLevel <= 0)
            {
                return;
            }

            /// Dữ liệu kỹ năng con
            SkillDataEx childSkillData = KSkill.GetSkillData(this.m_sAddedWithEnemy_SkillID);
            /// Nếu kỹ năng con không tồn tại thì bỏ qua
            if (childSkillData == null)
            {
                return;
            }
            /// Kỹ năng con
            SkillLevelRef childSkill = new SkillLevelRef()
            {
                AddedLevel = childSkillLevel,
                BonusLevel = 0,
                CanStudy = false,
                Data = childSkillData,
            };

            /// Kiểm tra số Buff tồn tại trên người nếu quá số lượng tối đa của kỹ năng thì bỏ qua
            BuffDataEx buff = this.Buffs.GetBuff(this.m_sAddedWithEnemy_SkillID);
            /// Nếu Buff chưa tồn tại
            if (buff == null)
            {
                KTSkillManager.UseSkill(this, this, childSkill, true);
            }
            /// Nếu Buff đã tồn tại thì kiểm tra Tag
            else
            {
                /// Xóa Buff cũ
                this.Buffs.RemoveBuff(buff);

                /// Số lượng cộng dồn
                int nStack = enemiesCount;
                /// Nếu quá giới hạn
                if (nStack > this.m_sAddedWithEnemy_MaxCount)
                {
                    nStack = this.m_sAddedWithEnemy_MaxCount;
                }
                buff.StackCount = nStack;
                buff.StartTick = KTGlobal.GetCurrentTimeMilis();

                //PlayerManager.ShowNotification(this, string.Format("Activate Buff '{0}', stack = {1}", buff.Skill.Data.Name, nStack));
                /// Thêm Buff mới
                this.Buffs.AddBuff(buff);
            }
        }

        #endregion Kỹ năng

        #region Thao tác

        /// <summary>
        /// Sự kiện gọi đến khi thao tác thu thập hoàn tất
        /// </summary>
        /// <param name="growPoint"></param>
        public void GrowPointCollectCompleted(GrowPoint growPoint)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            ProcessTask.Process(Global._TCPManager.MySocketListener, pool, this, growPoint.Data.ResID, growPoint.Data.ResID, -1, TaskTypes.Collect);

            //Sự kiện click vào cờ
            if (this.IsInFacetionBattleJin())
            {
                FactionBattleManager.FinishCollectFlag(this, growPoint.Data.ResID);
            }
        }

        /// <summary>
        /// Sự kiện dùng vật phẩm thành công
        /// </summary>
        /// <param name="itemGD"></param>
        public void UseItemCompleted(GoodsData itemGD)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;
        }

        #endregion Thao tác
    }
}