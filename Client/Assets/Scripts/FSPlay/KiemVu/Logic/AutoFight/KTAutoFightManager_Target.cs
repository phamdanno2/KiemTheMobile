using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Quản lý tìm mục tiêu
    /// </summary>
    public partial class KTAutoFightManager
    {
        public int TrigerAttackID = -1;

        public bool IsTriger = false;

        public void ForceTaget(int RoleID)
        {
         
            // Nếu đang bật tự động đánh
            if (this.IsAutoFighting)
            {
                // Nếu có config tự động đánh trả
                if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoPKAgain)
                {
                    // Tìm thử xem có thấy thằng kia xung quanh mình không
                    GSprite players = KTObjectsManager.Instance.FindObjects<GSprite>(x => x.SpriteType == GSpriteTypes.Other && x.RoleData.RoleID == RoleID).FirstOrDefault();
                    if (players != null)
                    {
                        if (!players.IsDeath && players.HP > 0)
                        {

                            TrigerAttackID = RoleID;
                            // reset lại thời gian bị triger
                            TrigerTime = Stopwatch.StartNew();
                            // set cho nó đang bị triger
                            this.IsTriger = true;
                            // Đổi mục tiêu sẽ đấm là thằng này
                            this.ChangeAutoFightTarget(players);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tìm mục tiêu phù hợp nhất
        /// </summary>
        /// <param name="ignoredTarget"></param>
        /// <param name="ignoreLastTarget"></param>
        /// <returns></returns>
        private GSprite FindBestTarget(List<GSprite> ignoredTarget = null, bool ignoreLastTarget = false,int AttackRadius = -1)
        {
            KTDebug.LogWarning("SCAN TAGET WITH RAGNER OF SKILL :" + AttackRadius);
            GSprite newTarget = null;

            /// Nếu không phải chế độ đánh quái==> tức là chỉ đánh người
            if (!KTAutoAttackSetting.IsTrainMode || this.IsTriger)
            {
               
                // nếu đang bị tấn công
                if (this.IsTriger && TrigerAttackID != -1)
                {
                    // Tìm thử xem có thấy thằng kia xung quanh mình không
                    GSprite players = KTObjectsManager.Instance.FindObjects<GSprite>(x => x.SpriteType == GSpriteTypes.Other && x.RoleData.RoleID == TrigerAttackID).FirstOrDefault();
                   
                    if (players != null)
                    {
                        // Nếu thằng này chưa chết và có HP > 0

                        if (!players.IsDeath && players.HP > 0)
                        {
                            float Dis = Vector2.Distance(players.PositionInVector2, Global.Data.Leader.PositionInVector2);

                            // Nếu khoảng cách còn trong 1 màn hình thì vã nó
                            if (Dis < 600f)
                            {
                                return players;
                            }
                        }
                        else // Nếu thấy thằng này đã chết thì xóa triger cho nhân vật
                        {
                            this.TrigerTime.Stop();
                            TrigerAttackID = -1;
                            this.IsTriger = false;
                        }
                    }
                }

                newTarget = SkillManager.FindNearestEnemy(Global.Data.Leader, (sprite) =>
                {
                    if (Global.Data.OtherRoles.TryGetValue(sprite.RoleID, out RoleData rd))
                    {
                        if (rd.CurrentHP <= 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    return (!ignoreLastTarget || sprite != this.AutoFightLastTarget) && (ignoredTarget == null || (ignoredTarget != null && !ignoredTarget.Contains(sprite)));
                }, AttackRadius);

                /// Nếu chỉ đánh người mà không tìm thấy người thì thôi
                if (newTarget == null && !KTAutoAttackSetting.IsTrainMode)
                {
                    return null;
                }

                /// Nếu tìm thấy mục tiêu thì trả về kết quả luôn
                if (newTarget != null)
                {
                    return newTarget;
                }
            }

         
            /// Tìm mục tiêu gần nhất khác với mục tiêu hiện tại
            newTarget = SkillManager.FindNearestEnemy(Global.Data.Leader, (sprite) =>
            {
                /// Dữ liệu đối tượng
                if (Global.Data.SystemMonsters.TryGetValue(sprite.RoleID, out MonsterData md))
                {
                    /// Nếu có thiết lập bỏ qua Boss
                    if (KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsSkipBoss && md.MonsterType != (int)MonsterTypes.Normal && md.MonsterType != (int)MonsterTypes.Hater && md.MonsterType != (int) MonsterTypes.Special_Normal && md.MonsterType != (int) MonsterTypes.Static && md.MonsterType != (int) MonsterTypes.Static_ImmuneAll)
                    {
                        return false;
                    }
                    else if (md.HP <= 0)
                    {
                        return false;
                    }

                    //if(KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsRadius)
                    //{
                    //    float DistanceCheckWithRadius = Vector2.Distance(startPos, sprite.PositionInVector2);
                    //    // Nếu con quái này nằm ngoài phạm vi thì thôi
                    //    if (DistanceCheckWithRadius> KTAutoAttackSetting._AutoConfig._AutoTrainConfig.Raidus)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (Global.Data.OtherRoles.TryGetValue(sprite.RoleID, out RoleData rd))
                {
                    if (rd.CurrentHP <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                return (!ignoreLastTarget || sprite != this.AutoFightLastTarget) && (ignoredTarget == null || (ignoredTarget != null && !ignoredTarget.Contains(sprite)));
            }, AttackRadius);

            /// Trả về kết quả
            return newTarget;
        }
    }
}