using GameServer.KiemThe.GameDbController;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Data;
using Server.Tools;
using System.Collections.Generic;
using System.Linq;
using System;
using GameServer.KiemThe.Utilities;

namespace GameServer.KiemThe.Entities
{
    /// <summary>
    /// Danh sách kỹ năng của nhân vật
    /// </summary>
    public partial class SkillTree
    {
        /// <summary>
        /// Đối tượng chủ nhân
        /// </summary>
        public KPlayer Player { get; private set; }

        /// <summary>
        /// Danh sách kỹ năng của nhân vật
        /// </summary>
        /// <param name="player"></param>
        public SkillTree(KPlayer player)
        {
            this.Player = player;
            this.BuildSkillTree();
        }

        /// <summary>
        /// Xây SkillTree dựa vào dữ liệu nhân vật
        /// </summary>
        private void BuildSkillTree()
        {
            this.listStudiedSkills.Clear();

            if (this.Player.SkillDataList != null)
            {
                foreach (SkillData dbSkillData in this.Player.SkillDataList)
                {
                    SkillDataEx skillData = KSkill.GetSkillData(dbSkillData.SkillID);
                    if (skillData != null)
                    {
                        SkillLevelRef levelRef = new SkillLevelRef()
                        {
                            AddedLevel = dbSkillData.SkillLevel,
                            Data = skillData,
                            BonusLevel = this._AllSkillBonusLevel,
                            CanStudy = true,
                            Exp = dbSkillData.Exp,
                        };
                        this.listStudiedSkills[skillData.ID] = levelRef;
                    }

                    /// Phục hồi dữ liệu Cooldown lấy từ DB
                    this.AddSkillCooldown(dbSkillData.SkillID, dbSkillData.LastUsedTick, dbSkillData.CooldownTick);
                }
            }
        }

        /// <summary>
        /// Đồng bộ dữ liệu SkillTree vào DB
        /// </summary>
        public void ExportSkillTree()
        {
            List<SkillData> dbSkillDataList = new List<SkillData>();
            foreach (SkillLevelRef levelRef in this.listStudiedSkills.Values)
            {
                SkillData dbSkillData = new SkillData()
                {
                    SkillID = levelRef.SkillID,
                    SkillLevel = levelRef.AddedLevel,
                    BonusLevel = this._AllSkillBonusLevel,
                    CanStudy = levelRef.CanStudy,
                    LastUsedTick = this.GetSkillLastUsedTick(levelRef.SkillID),
                    CooldownTick = this.GetSkillCooldown(levelRef.SkillID),
                    Exp = levelRef.Exp,
                };
                dbSkillDataList.Add(dbSkillData);
            }

            this.Player.SkillDataList = dbSkillDataList;

            /// Gửi tín hiệu thay đổi danh sách kỹ năng về Client
            KT_TCPHandler.SendRenewSkillList(this.Player);
        }
    }
}