using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Entities
{
    /// <summary>
    /// Môn phái của người chơi
    /// </summary>
    public class KPlayerFaction
    {
        /// <summary>
        /// Đối tượng người chơi
        /// </summary>
        private readonly KPlayer m_rPlayer;

        /// <summary>
        /// ID môn phái
        /// </summary>
        private int m_byFactionId;

        /// <summary>
        /// ID nhánh
        /// </summary>
        private int m_byRouteId;

        /// <summary>
        /// Có phải hệ ngoại công không
        /// </summary>
        private bool m_IsPhysical;

        /// <summary>
        /// Có sử dụng tốc đánh không
        /// </summary>
        private bool m_UseAS;

        /// <summary>
        /// Quản lý môn phái của người chơi
        /// </summary>
        /// <param name="rPlayer"></param>
        public KPlayerFaction(KPlayer rPlayer, int factionID, int routeID)
        {
            this.m_rPlayer = rPlayer;
            this.m_byFactionId = factionID;
            this.m_byRouteId = routeID;
            this.m_IsPhysical = KFaction.IsPhysical(this.m_byFactionId, this.m_byRouteId);
            this.m_UseAS = KFaction.UseAS(this.m_byFactionId, this.m_byRouteId);
        }

        /// <summary>
        /// Trả về ID môn phái của người chơi
        /// </summary>
        /// <returns></returns>
        public int GetFactionId()
        {
            return this.m_byFactionId;
        }

        /// <summary>
        /// Trả về tên môn phái của người chơi
        /// </summary>
        /// <returns></returns>
        public string GetFactionName()
        {
            return KFaction.GetName(this.m_byFactionId);
        }

        /// <summary>
        /// Trả về ID nhánh tu luyện trong môn phái của người chơi
        /// </summary>
        /// <returns></returns>
        public int GetRouteId()
        {
            return this.m_byRouteId;
        }

        /// <summary>
        /// Trả về tên nhánh tu luyện của người chơi
        /// </summary>
        /// <returns></returns>
        public string GetRouteName()
        {
            return KFaction.GetRouteName(this.m_byFactionId, this.m_byRouteId);
        }

        /// <summary>
        /// Có phải phái ngoại không
        /// </summary>
        /// <returns></returns>
        public bool IsPhysical()
        {
            return this.m_IsPhysical;
        }

        /// <summary>
        /// Có ảnh hưởng bởi tốc đánh không
        /// </summary>
        /// <returns></returns>
        public bool UseAS()
        {
            return this.m_UseAS;
        }

        /// <summary>
        /// Xóa kỹ năng môn phái cũ
        /// </summary>
        private void RemoveFactionSkills()
        {
            List<int> skills = KFaction.GetFactionSkills(this.m_byFactionId);
            this.m_rPlayer.Skills.RemoveSkills(skills);
        }

        /// <summary>
        /// Tẩy lại cấp độ kỹ năng môn phái
        /// </summary>
        public void ResetFactionSkillsLevel()
        {
            List<int> skills = KFaction.GetFactionSkills(this.m_byFactionId);
            this.m_rPlayer.Skills.ResetSkillsLevel(skills);
        }

        /// <summary>
        /// Thêm kỹ năng môn phái mới
        /// </summary>
        private void AddFactionSkills()
        {
            List<int> skills = KFaction.GetFactionSkills(this.m_byFactionId);
            this.m_rPlayer.Skills.AddSkills(skills);
        }

        /// <summary>
        /// Đổi môn phái
        /// </summary>
        /// <param name="byFactionId"></param>
        /// <returns></returns>
        public bool ChangeFaction(int byFactionId)
        {
            /// Nếu phái không tồn tại
            if (!KFaction.IsFactionExist(byFactionId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("KPlayerFaction::ChangeFaction => Faction ID = {0} not found", byFactionId));
                return false;
            }

            /// Thực hiện chuyển phái
            this.ProcessFactionRouteChange(this.m_byFactionId, this.m_byRouteId, byFactionId, GameDataDef.KD_FACTION_ROUTE_NONE);

            /// Trả về kết quả
            return true;
        }

        /// <summary>
        /// Đổi nhánh môn phái
        /// </summary>
        /// <param name="byRouteId"></param>
        /// <param name="isFactionChanged"></param>
        /// <returns></returns>
        public bool ChangeFactionRoute(int byRouteId)
		{
            /// Nếu phái không tồn tại
            if (!KFaction.IsFactionExist(this.m_byFactionId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("KPlayerFaction::ChangeFactionRoute => Faction ID = {0} not found", this.m_byFactionId));
                return false;
            }
            /// Nếu nhánh không tồn tại
            else if (byRouteId != GameDataDef.KD_FACTION_ROUTE_NONE && !KFaction.IsRouteExist(this.m_byFactionId, byRouteId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("KPlayerFaction::ChangeFactionRoute => Route ID = {0} not found in Faction ID = {1}", byRouteId, this.m_byFactionId));
                return false;
            }

            /// Thực hiện chuyển nhánh
            this.ProcessFactionRouteChange(this.m_byFactionId, this.m_byRouteId, this.m_byFactionId, byRouteId);

            /// Trả về kết quả
            return	true;
		}

        /// <summary>
        /// Xóa toàn bộ chỉ số cộng của môn phái và nhánh cũ
        /// </summary>
        /// <param name="oldFactionID"></param>
        /// <param name="oldRouteID"></param>
        /// <param name="nDamagePhysicsChange"></param>
        /// <param name="nDamageMagicChange"></param>
        /// <param name="nAttackRateChange"></param>
        /// <param name="nDefenceChange"></param>
        /// <param name="nLifeChange"></param>
        /// <param name="nManaChange"></param>
        private void RemoveFactionProperties(int oldFactionID, int oldRouteID, out int nDamagePhysicsChange, out int nDamageMagicChange, out int nAttackRateChange, out int nDefenceChange, out int nLifeChange, out int nManaChange)
        {
            /// Tẩy toàn bộ điểm kỹ năng nhánh tương ứng
            this.ResetFactionSkillsLevel();
            /// Gọi đến sự kiện trước khi thay đổi nhánh môn phái
            this.m_rPlayer.OnPreFactionChanged();
            /// Xóa toàn bộ hiệu ứng trang bị
            this.m_rPlayer.GetPlayEquipBody().ClearAllEffecEquipBody();

            /// Mặc định phái sử dụng tốc đánh
            this.m_UseAS = true;
            /// Mặc định là phái ngoại công
            this.m_IsPhysical = true;

            /// Vật công ngoại
            nDamagePhysicsChange = KPlayerSetting.GetStrength2DamagePhysics(oldFactionID, oldRouteID, this.m_rPlayer.GetCurStrength()) + KPlayerSetting.GetDexterity2DamagePhysics(oldFactionID, oldRouteID, this.m_rPlayer.GetCurDexterity());
            /// Vật công nội
            nDamageMagicChange = KPlayerSetting.GetEnergy2DamageMagic(oldFactionID, oldRouteID, this.m_rPlayer.GetCurEnergy());
            /// Chính xác
            nAttackRateChange = KPlayerSetting.GetDexterity2AttackRate(oldFactionID, oldRouteID, this.m_rPlayer.GetCurDexterity());
            /// Né tránh
            nDefenceChange = KPlayerSetting.GetDexterity2Defence(oldFactionID, oldRouteID, this.m_rPlayer.GetCurDexterity());
            /// Sinh lực
            nLifeChange = KPlayerSetting.GetVitality2Life(oldFactionID, oldRouteID, this.m_rPlayer.GetCurVitality());
            /// Nội lực
            nManaChange = KPlayerSetting.GetEnergy2Mana(oldFactionID, oldRouteID, this.m_rPlayer.GetCurEnergy());
        }

        /// <summary>
        /// Tính toán lại chỉ số môn phái và nhánh tương ứng
        /// </summary>
        /// <param name="byFactionID"></param>
        /// <param name="byRouteID"></param>
        /// <param name="nDamagePhysicsChange"></param>
        /// <param name="nDamageMagicChange"></param>
        /// <param name="nAttackRateChange"></param>
        /// <param name="nDefenceChange"></param>
        /// <param name="nLifeChange"></param>
        /// <param name="nManaChange"></param>
        private void CalculateFactionProperties(int byFactionID, int byRouteID, out int nDamagePhysicsChange, out int nDamageMagicChange, out int nAttackRateChange, out int nDefenceChange, out int nLifeChange, out int nManaChange)
        {
            /// Cập nhật lại môn phái và nhánh
            this.m_byFactionId = byFactionID;
            this.m_byRouteId = byRouteID;

            /// Cập nhật lại dữ liệu
            this.m_IsPhysical = KFaction.IsPhysical(byFactionID, byRouteID);
            this.m_UseAS = KFaction.UseAS(byFactionID, byRouteID);

            /// Vật công ngoại
            nDamagePhysicsChange = KPlayerSetting.GetStrength2DamagePhysics(byFactionID, byRouteID, this.m_rPlayer.GetCurStrength()) + KPlayerSetting.GetDexterity2DamagePhysics(byFactionID, byRouteID, this.m_rPlayer.GetCurDexterity());
            /// Vật công nội
            nDamageMagicChange = KPlayerSetting.GetEnergy2DamageMagic(byFactionID, byRouteID, this.m_rPlayer.GetCurEnergy());
            /// Chính xác
            nAttackRateChange = KPlayerSetting.GetDexterity2AttackRate(byFactionID, byRouteID, this.m_rPlayer.GetCurDexterity());
            /// Né tránh
            nDefenceChange = KPlayerSetting.GetDexterity2Defence(byFactionID, byRouteID, this.m_rPlayer.GetCurDexterity());
            /// Sinh lực
            nLifeChange = KPlayerSetting.GetVitality2Life(byFactionID, byRouteID, this.m_rPlayer.GetCurVitality());
            /// Nội lực
            nManaChange = KPlayerSetting.GetEnergy2Mana(byFactionID, byRouteID, this.m_rPlayer.GetCurEnergy());
        }

        /// <summary>
        /// Thực hiện thay đổi môn phái và nhánh
        /// </summary>
        /// <param name="oldFactionID"></param>
        /// <param name="oldRouteID"></param>
        /// <param name="newFactionID"></param>
        /// <param name="newRouteID"></param>
        private void ProcessFactionRouteChange(int oldFactionID, int oldRouteID, int newFactionID, int newRouteID)
        {
            /// Thực hiện xóa toàn bộ chỉ số cộng
            this.RemoveFactionProperties(oldFactionID, oldRouteID, out int oDamagePhysicsChange, out int oDamageMagicChange, out int oAttackRateChange, out int oDefenceChange, out int oLifeChange, out int oManaChange);

            /// Nếu môn phái mới khác môn phái cũ
            if (oldFactionID != newFactionID)
            {
                /// Xóa toàn bộ kỹ năng phái cũ
                this.RemoveFactionSkills();
            }

            /// Chỉ số mới
            this.CalculateFactionProperties(newFactionID, newRouteID, out int nDamagePhysicsChange, out int nDamageMagicChange, out int nAttackRateChange, out int nDefenceChange, out int nLifeChange, out int nManaChange);

            /// Nếu môn phái mới khác môn phái cũ
            if (oldFactionID != newFactionID)
            {
                /// Thêm toàn bộ kỹ năng phái mới
                this.AddFactionSkills();
            }

            /// Cập nhật lại chỉ số cơ bản cho nhân vật
            /// Vật công ngoại
            this.m_rPlayer.ChangePhysicsDamage(-oDamagePhysicsChange + nDamagePhysicsChange);
            /// Vật công nội
            this.m_rPlayer.ChangeMagicDamage(-oDamageMagicChange + nDamageMagicChange);
            /// Chính xác
            this.m_rPlayer.ChangeAttackRating(-oAttackRateChange + nAttackRateChange, 0, 0);
            /// Né tránh
            this.m_rPlayer.ChangeDefend(-oDefenceChange + nDefenceChange, 0, 0);
            /// Sinh lực
            this.m_rPlayer.ChangeLifeMax(-oLifeChange + nLifeChange, 0, 0);
            /// Nội lực
            this.m_rPlayer.ChangeManaMax(-oManaChange + nManaChange, 0, 0);

            /// Nếu môn phái mới khác môn phái cũ
            if (oldFactionID != newFactionID)
            {
                /// Thực hiện đổi ngũ hành trang bị tương ứng môn phái mới
                this.m_rPlayer.GetPlayEquipBody().ChangeEquipsCorrespondingToSeries();
            }

            /// Gọi đến sự kiện thay đổi nhánh môn phái
            this.m_rPlayer.OnFactionChanged();
            /// Thực hiện làm mới thuộc tính trang bị
            this.m_rPlayer.GetPlayEquipBody().AttackAllEquipBody();
        }
    }
}
