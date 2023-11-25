using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Tự dùng Buff
    /// </summary>
    public partial class KTAutoFightManager
    {
        /// <summary>
        /// Danh sách kỹ năng bỏ qua không sử dụng Buff
        /// </summary>
        private readonly HashSet<int> IgnoreBuffSkillID = new HashSet<int>()
        {
            861, 839
        };

        /// <summary>
        /// Tự động Buff máu cho đồng đội, áp dụng với Nga My
        /// </summary>
        private void AutoEMBuff()
        {
            /// Nếu bản thân không phải Nga My
            if (Global.Data.RoleData.FactionID != 5)
            {
                return;
            }

            /// Nếu chưa đến thời gian kiểm tra thì bỏ qua
            if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckAutoEMBuff < this.AutoFight_AutoEMBuffEveryTick)
            {
                return;
            }

            /// Đánh dấu thời gian kiểm tra tự động kích hoạt các Buff chủ động
            this.AutoFightLastCheckAutoEMBuff = KTGlobal.GetCurrentTimeMilis();

            /// Duyệt danh sách kỹ năng, tìm ra các Buff chủ động tương ứng
            foreach (SkillData skill in Global.Data.RoleData.SkillDataList)
            {
                /// Nếu chưa học kỹ năng này
                if (skill.Level <= 0)
                {
                    continue;
                }

                if (Loader.Loader.Skills.TryGetValue(skill.SkillID, out SkillDataEx skillData))
                {
                    /// Nếu không thỏa mãn điều kiện có thể dùng kỹ năng này
                    if (!SkillManager.IsAbleToUseSkill(skillData, skill.Level, false))
                    {
                        continue;
                    }
                    // Nếu bản thân là nga my
                    if (skill.SkillID == 98)
                    {
                        // Nếu có thiết lập buff nga my
                        if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsBuffNM)
                        {
                            if (Global.Data.RoleData.TeamID != -1)
                            {
                                foreach (RoleDataMini Role in Global.Data.Teammates)
                                {
                                    // % MÁU CỦA ĐỒNG ĐỘI
                                    int HpPercent = Role.HP * 100 / Role.MaxHP;

                                    if (HpPercent < KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoNMPercent)
                                    {
                                        // Tìm xem có đồng đội này xung quanh mình không
                                        //GSprite players = KTObjectsManager.Instance.FindObjects<GSprite>(x => x.SpriteType == GSpriteTypes.Other && x.RoleData.RoleID == Role.RoleID).FirstOrDefault();
                                        GSprite players = KTGlobal.FindSpriteByID(Role.RoleID);

                                        // Nếu player còn sống thì buff máu thôi
                                        if (players != null && !players.IsDeath)
                                        {
                                            KTGlobal.AddNotification("Thực hiện buff máu cho đồng đội: " + Role.RoleName);

                                            float Distance = Vector2.Distance(players.PositionInVector2, Global.Data.Leader.PositionInVector2);

                                            // Nếu khoảng cách hợp lý thì buff cho nó ngay
                                            if (Distance < 600f)
                                            {
                                                GSprite tmp = SkillManager.SelectedTarget;
                                                /// Thiết lập không có mục tiêu
                                                SkillManager.SelectedTarget = players;
                                                /// Dùng kỹ năng Buff
                                                SkillManager.LeaderUseSkill(skill.SkillID, false);
                                                /// Trả lại mục tiêu cũ
                                                SkillManager.SelectedTarget = tmp;
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Tự buff máu cho bản thân nếu hết máu
                                int HpPercent = Global.Data.RoleData.CurrentHP * 100 / Global.Data.RoleData.MaxHP;

                                if (HpPercent < KTAutoAttackSetting._AutoConfig._AutoPKConfig.AutoNMPercent)
                                {
                                    KTGlobal.AddNotification("Tự buff máu cho bản thân");

                                    GSprite tmp = SkillManager.SelectedTarget;
                                    /// Thiết lập không có mục tiêu
                                    SkillManager.SelectedTarget = Global.Data.Leader;
                                    /// Dùng kỹ năng Buff
                                    SkillManager.LeaderUseSkill(skill.SkillID, false);
                                    /// Trả lại mục tiêu cũ
                                    SkillManager.SelectedTarget = tmp;

                                    break;
                                }
                            }
                        }
                    }

                    // Nếu bản thân có skill hồi sinh đồng đội
                    if (skill.SkillID == 110)
                    {
                        // Nếu có thiết lập buff nga my và hồi sinh đồng đội
                        if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoReviceTeam)
                        {
                            if (Global.Data.RoleData.TeamID != -1)
                            {
                                foreach (RoleDataMini Role in Global.Data.Teammates)
                                {
                                    // Tìm xem có đồng đội này xung quanh mình không
                                    //GSprite players = KTObjectsManager.Instance.FindObjects<GSprite>(x => x.SpriteType == GSpriteTypes.Other && x.RoleData.RoleID == Role.RoleID).FirstOrDefault();
                                    GSprite players = KTGlobal.FindSpriteByID(Role.RoleID);

                                    // Nếu có đồng đội xung quanh đã chết thì cho nó buff
                                    if (players != null)
                                    {
                                        float Distance = Vector2.Distance(players.PositionInVector2, Global.Data.Leader.PositionInVector2);

                                        // Nếu đồng đội chết thì thực hiện hồi sinh nó
                                        if (Distance < 500f && (players.IsDeath || players.HP == 0))
                                        {
                                            KTGlobal.AddNotification("Đã hồi sinh cho đồng đội: " + Role.RoleName);

                                            GSprite tmp = SkillManager.SelectedTarget;
                                            /// Thiết lập không có mục tiêu
                                            SkillManager.SelectedTarget = players;
                                            /// Dùng kỹ năng Buff
                                            SkillManager.LeaderUseSkill(skill.SkillID, false);
                                            /// Trả lại mục tiêu cũ
                                            SkillManager.SelectedTarget = tmp;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tự động kích hoạt các Buff chủ động
        /// </summary>
        private void AutoActivatePositiveBuffs()
        {
            /// Nếu chưa đến thời gian kiểm tra thì bỏ qua
            if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckAutoActivatePositiveBuffs < this.AutoFight_CheckAndAutoActivatePositiveBuffs)
            {
                return;
            }

            /// Đánh dấu thời gian kiểm tra tự động kích hoạt các Buff chủ động
            this.AutoFightLastCheckAutoActivatePositiveBuffs = KTGlobal.GetCurrentTimeMilis();

            /// Duyệt danh sách kỹ năng, tìm ra các Buff chủ động tương ứng
            foreach (SkillData skill in Global.Data.RoleData.SkillDataList)
            {
                /// Nếu chưa học kỹ năng này
                if (skill.Level <= 0)
                {
                    continue;
                }

                if (Loader.Loader.Skills.TryGetValue(skill.SkillID, out SkillDataEx skillData))
                {
                    /// Nếu không thỏa mãn điều kiện có thể dùng kỹ năng này
                    if (!SkillManager.IsAbleToUseSkill(skillData, skill.Level, false))
                    {
                        continue;
                    }

                    /// Nếu là kỹ năng không dùng
                    if (this.IgnoreBuffSkillID.Contains(skill.SkillID))
                    {
                        continue;
                    }

                    /// Nếu là kỹ năng Buff, và chưa có hiệu ứng trên người thêm điều kiện kia cho NGA my khỏi sử dụng skill bữa bãi khi đầy máu vẫn tự bufff
                    if (skillData.Type == 2 && skill.SkillID != 110 && skill.SkillID != 98 && (skillData.TargetType == "self" || skillData.TargetType == "team") && !skill.IsCooldown && !Global.Data.RoleData.BufferDataList.Any(x => x.BufferID == skill.SkillID))
                    {
                        GSprite tmp = SkillManager.SelectedTarget;
                        /// Thiết lập không có mục tiêu
                        SkillManager.SelectedTarget = null;
                        /// Dùng kỹ năng Buff
                        SkillManager.LeaderUseSkill(skill.SkillID, false);
                        /// Trả lại mục tiêu cũ
                        SkillManager.SelectedTarget = tmp;
                        break;
                    }
                }
            }
        }
    }
}