using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Các phương thức hỗ trợ
    /// </summary>
    public static partial class SkillManager
    {
        /// <summary>
        /// Phạm vi tìm mục tiêu
        /// </summary>
        public const int FindEnemyRange = 800;

        /// <summary>
        /// Trả về lượng máu hiện tại của mục tiêu
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int GetTargetTrueHP(GSprite target)
        {
            /// Nếu không có dữ liệu
            if (target == null)
            {
                return 0;
            }

            /// Nếu là người chơi khác
            if (Global.Data.OtherRoles.TryGetValue(target.RoleID, out RoleData rd))
            {
                return rd.CurrentHP;
            }
            /// Nếu là quái
            else if (Global.Data.SystemMonsters.TryGetValue(target.RoleID, out MonsterData md))
            {
                return md.HP;
            }
            /// Mặc định thì = 0
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Trả về danh sách kẻ địch xung quanh theo khoảng cách từ gần tới xa
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static List<GSprite> FindNearestEnemies(GSprite sprite)
        {
            /// Tìm danh sách mục tiêu là kẻ địch
            List<GSprite> listEnemies = KTObjectsManager.Instance.FindObjects<GSprite>(x => SkillManager.IsValidTarget(x)).ToList();
            listEnemies.Sort((o1, o2) =>
            {
                /// Khoảng cách hiện tại đến mục tiêu
                float distanceO1 = Vector2.Distance(o1.PositionInVector2, sprite.PositionInVector2);
                float distanceO2 = Vector2.Distance(o2.PositionInVector2, sprite.PositionInVector2);

                return (int)(distanceO1 - distanceO2);
            });
            return listEnemies;
        }

        /// <summary>
        /// Tìm kẻ địch gần nhất thỏa mãn điều kiện
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static GSprite FindNearestEnemy(GSprite sprite, Predicate<GSprite> predicate, int MaxRanger = -1)
        {
            /// Mục tiêu
            GSprite target = null;
            /// Tìm danh sách mục tiêu là kẻ địch
            List<GSprite> listEnemies = SkillManager.FindNearestEnemies(sprite);

            ///Danh sách mục tiêu nằm trong tầm đánh của kỹ năng
            if (MaxRanger != -1)
            {
                // Lấy ra toàn bộ mục tiêu trong ranger của skill
                List<GSprite> listEnemiesInRanger = new List<GSprite>();

                var FitterValue = listEnemies.Where(x => (Vector2.Distance(sprite.PositionInVector2, x.PositionInVector2) <= MaxRanger));

                if (FitterValue != null)
                {
                    listEnemiesInRanger = FitterValue.ToList();
                }

                // Nếu có mục tiêu nằm trong ranger của kỹ năng
                if (listEnemiesInRanger.Count > 0)
                {
                    // Nếu là chế độ chỉ giết người
                    if (!KTAutoAttackSetting.IsTrainMode)
                    {
                        // Nếu ưu tiên khắc hệ và máu ít
                        if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsElementalSelect && KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsLowHpSelect)
                        {
                            FitterValue = listEnemiesInRanger.Where(x=>x.SpriteType == GSpriteTypes.Other).OrderBy(x => (KTGlobal.g_IsConquer(sprite.RoleData.FactionID, x.RoleData.FactionID))).ThenByDescending(x => x.HP);

                            if (FitterValue != null)
                            {
                                listEnemiesInRanger = FitterValue.ToList();
                            }
                        }
                        else if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsElementalSelect && !KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsLowHpSelect) // Nếu ưu tiên mỗi khắc hệ
                        {
                            FitterValue = listEnemiesInRanger.Where(x => x.SpriteType == GSpriteTypes.Other).OrderBy(x => (KTGlobal.g_IsConquer(sprite.RoleData.FactionID, x.RoleData.FactionID)));

                            if (FitterValue != null)
                            {
                                listEnemiesInRanger = FitterValue.ToList();
                            }
                        }
                        else if (!KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsElementalSelect && KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsLowHpSelect) // Nếu ưu tiên mỗi mục tiêu ít máu
                        {
                            FitterValue = listEnemiesInRanger.Where(x => x.SpriteType == GSpriteTypes.Other).OrderBy(x => x.HP);

                            if (FitterValue != null)
                            {
                                listEnemiesInRanger = FitterValue.ToList();
                            }
                        }

                    }
                    else // Nếu chỉ là chế độ train
                    {
                        if (KTAutoAttackSetting._AutoConfig._AutoTrainConfig.IsLowHpSelect)
                        {
                            FitterValue = listEnemiesInRanger.Where(x => x.SpriteType == GSpriteTypes.Monster).OrderBy(x => x.HP);

                            if (FitterValue != null)
                            {
                                listEnemiesInRanger = FitterValue.ToList();
                            }
                        }
                    }
                    for (int i = 0; i < listEnemiesInRanger.Count; i++)
                    {
                        /// Nếu thỏa mãn điều kiện
                        if ((predicate == null || predicate.Invoke(listEnemiesInRanger[i])))
                        {
                            target = listEnemiesInRanger[i];
                            break;
                        }
                    }
                }
            }
            // Nếu taget vẫn == null tức là đéo có mục tiêu tiềm năng trong ranger của skill
            if (target == null)
            {
                for (int i = 0; i < listEnemies.Count; i++)
                {
                    float Dis = Vector2.Distance(sprite.PositionInVector2, listEnemies[i].PositionInVector2);

                    /// Nếu thỏa mãn điều kiện
                    if ((predicate == null || predicate.Invoke(listEnemies[i])) && Dis <= SkillManager.FindEnemyRange)
                    {
                        target = listEnemies[i];
                        break;
                    }
                }
            }

            /// Trả về mục tiêu
            return target;
        }

        /// <summary>
        /// Tìm kẻ địch gần nhất
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static GSprite FindNearestEnemy(GSprite sprite)
        {
            return SkillManager.FindNearestEnemy(sprite, null);
        }

        /// <summary>
        /// Mục tiêu có phù hợp để tấn công không
        /// <para>Áp dụng cho Leader</para>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetMustBeEnemy"></param>
        /// <param name="targetMustAlive"></param>
        /// <returns></returns>
        public static bool IsValidTarget(GSprite target, bool targetMustBeEnemy = true, bool targetMustAlive = true)
        {
            /// Nếu không có mục tiêu
            if (target == null)
            {
                return false;
            }

            /// Nếu yêu cầu mục tiêu phải còn sống nhưng thực tế mục tiêu đã chết
            if (targetMustAlive && (target.IsDeath || SkillManager.GetTargetTrueHP(target) <= 0))
            {
                return false;
            }
            /// Nếu yêu cầu mục tiêu đã chết nhưng thực tế mục tiêu còn sống
            else if (!targetMustAlive && !(target.IsDeath || SkillManager.GetTargetTrueHP(target) <= 0))
            {
                return false;
            }

            /// Nếu trạng thái PK không phù hợp
            if (targetMustBeEnemy && !KTGlobal.CanAttack(target))
            {
                return false;
            }
            else if (!targetMustBeEnemy && KTGlobal.CanAttack(target))
            {
                return false;
            }

            /// Nếu thỏa mãn
            return true;
        }
    }
}