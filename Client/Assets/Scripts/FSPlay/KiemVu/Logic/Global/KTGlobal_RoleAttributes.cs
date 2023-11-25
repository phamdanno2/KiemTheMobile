using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Entities.Config;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu
{
    /// <summary>
    /// Các hàm toàn cục dùng trong Kiếm Thế
    /// </summary>
    public static partial class KTGlobal
    {
        #region Thăng cấp

        /// <summary>
        /// ID hiệu ứng thăng cấp nhân vật
        /// </summary>
        public const int RoleLevelUpEffect = 86;

        /// <summary>
        /// ID hiệu ứng thăng cấp danh vọng nhân vật
        /// </summary>
        public const int RoleAchievementUpEffect = 88;

        /// <summary>
        /// Thực thi hiệu ứng nhân vật thăng cấp
        /// </summary>
        public static void PlayRoleLevelUpEffect()
        {
            Global.Data.Leader.ComponentCharacter.AddEffect(KTGlobal.RoleLevelUpEffect, EffectType.CastEffect);
        }

        /// <summary>
        /// Thực thi hiệu ứng nhân vật thăng cấp danh vọng
        /// </summary>
        public static void PlayRoleAchievementUpEffect()
        {
            Global.Data.Leader.ComponentCharacter.AddEffect(KTGlobal.RoleAchievementUpEffect, EffectType.CastEffect);
        }

        #endregion Thăng cấp

        #region Tốc chạy và tốc đánh

        #region Tốc chạy

        /// <summary>
        /// Chuyển tốc độ di chuyển sang dạng lưới Pixel
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <returns></returns>
        public static int MoveSpeedToPixel(int moveSpeed)
        {
            return moveSpeed * 15;
        }

        /// <summary>
        /// Thời gian thực hiện động tác chạy hoặc đi bộ tối thiểu
        /// </summary>
        public const float MinWalkRunActionDuration = 0.1f;

        /// <summary>
        /// Thời gian thực hiện động tác chạy hoặc đi bộ tối đa
        /// </summary>
        public const float MaxWalkRunActionDuration = 0.8f;

        /// <summary>
        /// Tốc chạy tối thiểu
        /// </summary>
        public const int MinWalkRunSpeed = 0;

        /// <summary>
        /// Tốc chạy tối đa
        /// </summary>
        public const int MaxWalkRunSpeed = 30;

        /// <summary>
        /// Chuyển tốc độ chạy sang thời gian thực hiện động tác chạy
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <returns></returns>
        public static float MoveSpeedToFrameDuration(int moveSpeed)
        {
            /// Nếu tốc đánh nhỏ hơn tốc tối thiểu
            if (moveSpeed < KTGlobal.MinWalkRunSpeed)
            {
                moveSpeed = KTGlobal.MinWalkRunSpeed;
            }
            /// Nếu tốc đánh vượt quá tốc tối đa
            if (moveSpeed > KTGlobal.MaxWalkRunSpeed)
            {
                moveSpeed = KTGlobal.MaxWalkRunSpeed;
            }

            /// Tỷ lệ % so với tốc đánh tối đa
            float percent = moveSpeed / (float)KTGlobal.MaxWalkRunSpeed;

            /// Thời gian thực hiện động tác chạy hoặc đi bộ
            float animationDuration = KTGlobal.MinWalkRunActionDuration + (KTGlobal.MaxWalkRunActionDuration - KTGlobal.MinWalkRunActionDuration) * (1f - percent);
            if (animationDuration < KTGlobal.MinWalkRunActionDuration)
            {
                animationDuration = KTGlobal.MinWalkRunActionDuration;
            }

            /// Trả về kết quả
            return animationDuration;
        }

        #endregion Tốc chạy

        #region Tốc đánh

        /// <summary>
        /// Thời điểm dùng kỹ năng lần trước
        /// </summary>
        public static long LastUseSkillTick { get; set; }

        /// <summary>
        /// Thời điểm dùng kỹ năng không ảnh hưởng tốc đánh lần trước
        /// </summary>
        public static long LastUseSkillNoAffectAtkSpeedTick { get; set; }

        /// <summary>
        /// Đã kết thúc thực thi động tác xuất chiêu chưa
        /// </summary>
        public static bool FinishedUseSkillAction
        {
            get
            {
                /// Nếu đang đợi dùng kỹ năng thì bỏ qua
                if (SkillManager.IsWaitingToUseSkill)
                {
                    return false;
                }

                /// Nếu vừa dùng kỹ năng không ảnh hưởng bởi tốc đánh
                if (KTGlobal.GetCurrentTimeMilis() - KTGlobal.LastUseSkillNoAffectAtkSpeedTick < 100)
                {
                    return false;
                }

                /// Đối tượng Leader
                GSprite leader = Global.Data.Leader;

                /// Tốc độ xuất chiêu hệ ngoại công hiện tại
                int attackSpeed = leader.AttackSpeed;
                /// Tốc độ xuất chiêu hệ nội công hiện tại
                int castSpeed = leader.CastSpeed;

                /// Có phải hệ ngoại công không
                bool isPhysical = true;
                /// Môn phái tương ứng
                if (Loader.Loader.Factions.TryGetValue(Global.Data.RoleData.FactionID, out FactionXML faction))
                {
                    /// Nhánh tương ứng
                    if (faction.Subs.TryGetValue(Global.Data.RoleData.SubID, out FactionXML.Sub route))
                    {
                        isPhysical = route.IsPhysical;
                    }
                }

                /// Tổng thời gian
                float frameDuration = KTGlobal.AttackSpeedToFrameDuration(isPhysical ? attackSpeed : castSpeed);

                /// Trả ra kết quả
                return KTGlobal.GetCurrentTimeMilis() - KTGlobal.LastUseSkillTick >= frameDuration * 1000;
            }
        }

        /// <summary>
        /// Thời gian thực hiện động tác xuất chiêu tối thiểu
        /// </summary>
        public const float MinAttackActionDuration = 0.2f;

        /// <summary>
        /// Thời gian thực hiện động tác xuất chiêu tối đa
        /// </summary>
        public const float MaxAttackActionDuration = 0.8f;

        /// <summary>
        /// Thời gian cộng thêm giãn cách giữa các lần ra chiêu
        /// </summary>
        public const float AttackSpeedAdditionDuration = 0.1f;

        /// <summary>
        /// Tốc đánh tối thiểu
        /// </summary>
        public const int MinAttackSpeed = 0;

        /// <summary>
        /// Tốc đánh tối đa
        /// </summary>
        public const int MaxAttackSpeed = 100;

        /// <summary>
        /// Chuyển tốc độ đánh sang thời gian thực hiện động tác xuất chiêu
        /// </summary>
        /// <param name="attackSpeed"></param>
        /// <returns></returns>
        public static float AttackSpeedToFrameDuration(int attackSpeed)
        {
            /// Nếu tốc đánh nhỏ hơn tốc tối thiểu
            if (attackSpeed < KTGlobal.MinAttackSpeed)
            {
                attackSpeed = KTGlobal.MinAttackSpeed;
            }
            /// Nếu tốc đánh vượt quá tốc tối đa
            if (attackSpeed > KTGlobal.MaxAttackSpeed)
            {
                attackSpeed = KTGlobal.MaxAttackSpeed;
            }

            /// Tỷ lệ % so với tốc đánh tối đa
            float percent = attackSpeed / (float)KTGlobal.MaxAttackSpeed;

            /// Thời gian thực hiện động tác xuất chiêu
            float animationDuration = KTGlobal.MinAttackActionDuration + (KTGlobal.MaxAttackActionDuration - KTGlobal.MinAttackActionDuration) * (1f - percent);

            /// Trả về kết quả
            return animationDuration;
        }

        #endregion Tốc đánh

        #endregion Tốc chạy và tốc đánh

        #region Môn phái và ngũ hành

        /// <summary>
        /// Trả về giá trị tên môn phái theo ID
        /// </summary>
        /// <param name="factionID"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetFactionName(int factionID, out Color color)
        {
            Color _color = Color.white;
            if (KiemVu.Loader.Loader.Factions.TryGetValue(factionID, out KiemVu.Entities.Config.FactionXML faction))
            {
                string factionName = faction.Name;
                switch (faction.Elemental)
                {
                    case KiemVu.Entities.Enum.Elemental.METAL:
                        ColorUtility.TryParseHtmlString("#ffff1a", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.WOOD:
                        ColorUtility.TryParseHtmlString("#00ff00", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.EARTH:
                        ColorUtility.TryParseHtmlString("#c8bcbc", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.WATER:
                        ColorUtility.TryParseHtmlString("#8abdff", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.FIRE:
                        ColorUtility.TryParseHtmlString("#ffac47", out _color);
                        break;
                }
                color = _color;
                return factionName;
            }
            color = _color;
            return "";
        }

        /// <summary>
        /// Trả về giá trị ngũ hành môn phái
        /// </summary>
        /// <param name="factionID"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetFactionElementString(int factionID, out Color color)
        {
            Color _color = Color.white;
            if (KiemVu.Loader.Loader.Factions.TryGetValue(factionID, out KiemVu.Entities.Config.FactionXML faction))
            {
                if (KiemVu.Loader.Loader.Elements.TryGetValue(faction.Elemental, out KiemVu.Entities.Object.ElementData elementData))
                {
                    string elementName = elementData.Name;
                    switch (faction.Elemental)
                    {
                        case KiemVu.Entities.Enum.Elemental.METAL:
                            ColorUtility.TryParseHtmlString("#ffff1a", out _color);
                            break;

                        case KiemVu.Entities.Enum.Elemental.WOOD:
                            ColorUtility.TryParseHtmlString("#00ff00", out _color);
                            break;

                        case KiemVu.Entities.Enum.Elemental.EARTH:
                            ColorUtility.TryParseHtmlString("#c8bcbc", out _color);
                            break;

                        case KiemVu.Entities.Enum.Elemental.WATER:
                            ColorUtility.TryParseHtmlString("#8abdff", out _color);
                            break;

                        case KiemVu.Entities.Enum.Elemental.FIRE:
                            ColorUtility.TryParseHtmlString("#ffac47", out _color);
                            break;
                    }
                    color = _color;
                    return elementName;
                }
            }
            color = _color;
            return "Vô";
        }

        /// <summary>
        /// Trả về giá trị ngũ hành môn phái
        /// </summary>
        /// <param name="factionID"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static KiemVu.Entities.Enum.Elemental GetFactionElement(int factionID)
        {
            if (KiemVu.Loader.Loader.Factions.TryGetValue(factionID, out KiemVu.Entities.Config.FactionXML faction))
            {
                return faction.Elemental;
            }
            return KiemVu.Entities.Enum.Elemental.NONE;
        }

        public static bool g_IsConquer(int FactionSource, int FactionDesc)
        {
            int Source = 0;
            int Dest = 0;

            if (KiemVu.Loader.Loader.Factions.TryGetValue(FactionSource, out KiemVu.Entities.Config.FactionXML faction))
            {
                Source = (int)faction.Elemental;
            }
            else
            {
                Source = (int)KiemVu.Entities.Enum.Elemental.NONE;
            }

            if (KiemVu.Loader.Loader.Factions.TryGetValue(FactionDesc, out KiemVu.Entities.Config.FactionXML factiondest))
            {
                Dest = (int)factiondest.Elemental;
            }
            else
            {
                Dest = (int)KiemVu.Entities.Enum.Elemental.NONE;
            }

            if (Loader.Loader.g_IsConquer(Source, Dest))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Trả về tên trạng thái ngũ hành
        /// </summary>
        /// <param name="elemental"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetElementString(KiemVu.Entities.Enum.Elemental elemental, out Color color)
        {
            Color _color = Color.white;
            if (KiemVu.Loader.Loader.Elements.TryGetValue(elemental, out KiemVu.Entities.Object.ElementData elementData))
            {
                string elementName = elementData.Name;
                switch (elemental)
                {
                    case KiemVu.Entities.Enum.Elemental.METAL:
                        ColorUtility.TryParseHtmlString("#ffff1a", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.WOOD:
                        ColorUtility.TryParseHtmlString("#00ff00", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.EARTH:
                        ColorUtility.TryParseHtmlString("#c8bcbc", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.WATER:
                        ColorUtility.TryParseHtmlString("#8abdff", out _color);
                        break;

                    case KiemVu.Entities.Enum.Elemental.FIRE:
                        ColorUtility.TryParseHtmlString("#ffac47", out _color);
                        break;
                }
                color = _color;
                return elementName;
            }
            color = _color;
            return "Vô";
        }

        #endregion Môn phái và ngũ hành

        #region Quan hàm

        /// <summary>
        /// Thiết lập hiệu ứng quan hàm cho đối tượng
        /// </summary>
        /// <param name="sprite"></param>
        public static void RefreshOfficeRankEffect(GSprite sprite)
        {
            /// Nếu không phải người chơi
            if (sprite == null || sprite.ComponentCharacter == null)
            {
                return;
            }

            /// Quan hàm tương ứng
            int officeRank = sprite.RoleData.OfficeRank;

            /// Thông tin quan hàm tương ứng
            OfficeTitleXML currentOfficeTitleInfo = null;

            /// Duyệt danh sách lấy toàn bộ các danh hiệu cũ xóa đi
            foreach (OfficeTitleXML officeTitle in Loader.Loader.OfficeTitles)
            {
                /// Nếu là quan hàm hiện tại
                if (officeTitle.ID == officeRank)
                {
                    /// Cập nhật thông tin quan hàm tương ứng
                    currentOfficeTitleInfo = officeTitle;
                }
                /// Xóa Buff
                sprite.RemoveBuff(officeTitle.EffectID);
            }

            /// Nếu tồn tại quan hàm tương ứng
            if (currentOfficeTitleInfo != null)
            {
                /// Thêm Buff
                sprite.AddBuff(currentOfficeTitleInfo.EffectID, -1);
            }
        }

        #endregion Quan hàm
    }
}