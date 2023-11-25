﻿using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Utilities.UnityComponent;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Quản lý các sự kiện xảy ra với đối tượng
    /// </summary>
    public partial class Monster : IEvent
    {
        /// <summary>
        /// Hàm này gọi đến ngay khi đối tượng được tạo ra
        /// </summary>
        private void InitEvents()
        {
            if (this.Model.GetComponent<ClickableCollider2D>() != null)
            {
                this.Model.GetComponent<ClickableCollider2D>().OnClick = () => {
                    this.OnClick();
                };
            }
        }

        /// <summary>
        /// Sự kiện khi đối tượng được Click
        /// </summary>
        public void OnClick()
        {
            if (this.RefObject != null)
            {
                /// Nếu là NPC
                if (this.RefObject.SpriteType == GSpriteTypes.NPC)
                {
                    Global.Data.TargetNpcID = this.RefObject.RoleID - GameFramework.Logic.SpriteBaseIds.NpcBaseId;
                    //KTDebug.LogError(string.Format("{0} is clicked.", this.RefObject.RoleName));
                    Global.Data.GameScene.NPCClick(this.RefObject);
                }
                /// Nếu là quái
                else if (this.RefObject.SpriteType == GSpriteTypes.Monster && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.DynamicNPC)
                {
                    SkillManager.SelectedTarget = this.RefObject;
                    KTAutoFightManager.Instance.ChangeAutoFightTarget(this.RefObject);
                    //KTDebug.LogError(string.Format("{0} - ID = {1} is clicked.", this.RefObject.RoleName, this.RefObject.RoleID));
                    Global.Data.GameScene.MonsterClick(this.RefObject);
                }
                /// Nếu là điểm thu thập
                else if (this.RefObject.SpriteType == GSpriteTypes.GrowPoint)
                {
                    //KTDebug.LogError(string.Format("{0} is clicked.", this.RefObject.RoleName));
                    Global.Data.GameScene.GrowPointClick(this.RefObject);
                }
            }
        }

        /// <summary>
        /// Sự kiện khi vị trí của đối tượng thay đổi
        /// </summary>
        public void OnPositionChanged()
        {
            
        }
    }
}
