using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.CoreUI;
using Server.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component
{
    public partial class Monster : IDisplayUI
    {
        #region Define
        /// <summary>
        /// Khung trên đầu nhân vật
        /// </summary>
        private UIMonsterHeader UIHeader;

        /// <summary>
        /// Khung biểu diễn trên Minimap
        /// </summary>
        private UIMinimapReference UIMinimapReference;
        #endregion

        #region Kế thừa IDisplayUI
        /// <summary>
        /// Hiện UI
        /// </summary>
        public void DisplayUI()
        {
            if (this.UIHeader == null)
            {
                CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
                this.UIHeader = KTUIElementPoolManager.Instance.Instantiate<UIMonsterHeader>("UIMonsterHeader");
                this.UIHeader.ReferenceObject = this.gameObject;
                canvas.AddUnderLayerUI(this.UIHeader);
                this.UIHeader.gameObject.SetActive(true);
            }

            if (this.UIMinimapReference == null)
            {
                MinimapCanvasManager canvas = Global.MinimapCanvas.GetComponent<MinimapCanvasManager>();
                this.UIMinimapReference = KTUIElementPoolManager.Instance.Instantiate<UIMinimapReference>("UIMinimapReference");
                this.UIMinimapReference.ReferenceObject = this.gameObject;
                canvas.AddUI(this.UIMinimapReference);
                this.UIMinimapReference.gameObject.SetActive(true);
            }

            if (this.RefObject != null)
            {
                if (this.UIHeader != null)
                {
                    this.UIHeader.ShowHPBar = this._ShowHPBar;
                    this.UIHeader.ShowElemental = this._ShowElemental;
                    this.UIHeader.Name = this.RefObject.RoleName;
                    this.UIHeader.NameColor = this._NameColor;
                    this.UIHeader.Title = this.RefObject.Title;
                    this.UIHeader.Elemental = this.RefObject.Elemental;
                    this.UIHeader.Offset = this._UIHeaderOffset;
                    this.UIHeader.IsBoss = this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.Normal && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.Hater && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.Special_Normal && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.Static && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.Static_ImmuneAll && this.RefObject.MonsterData?.MonsterType != (int) MonsterTypes.DynamicNPC;
                }

                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.ShowIcon = this._ShowMinimapIcon;
                    this.UIMinimapReference.ShowName = this._ShowMinimapName;
                    this.UIMinimapReference.Name = this.RefObject.RoleName;
                    this.UIMinimapReference.NameColor = this._MinimapNameColor;
                    this.UIMinimapReference.BundleDir = KTGlobal.MinimapIconBundleDir;
                    this.UIMinimapReference.AtlasName = KTGlobal.MinimapIconAtlasName;
                    this.UIMinimapReference.SpriteName = this.RefObject.SpriteType == GSpriteTypes.NPC || this.RefObject.MonsterData?.MonsterType == (int) MonsterTypes.DynamicNPC ? KTGlobal.MinimapNPCIconSpriteName : this.RefObject.SpriteType == GSpriteTypes.Monster ? KTGlobal.MinimapMonsterIconSpriteName : this.RefObject.SpriteType == GSpriteTypes.GrowPoint ? KTGlobal.MinimapGrowPointIconSpriteName : "";
                    this.UIMinimapReference.IconSize = this._MinimapIconSize;
                }
            }

            this.UpdateHP();
            this.UpdateTitle();
        }

        /// <summary>
        /// Xóa UI
        /// </summary>
        public void DestroyUI()
        {
            if (this.UIHeader != null)
            {
                this.UIHeader.Destroy();
            }

            if (this.UIMinimapReference != null)
            {
                this.UIMinimapReference.Destroy();
            }
        }
        #endregion

        #region Update changes
        /// <summary>
        /// Cập nhật máu
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="maxHP"></param>
        public void UpdateHP()
        {
            if (this.UIHeader == null)
            {
                return;
            }

            if (this.RefObject.HPMax != 0)
            {
                this.UIHeader.HPPercent = (int) (this.RefObject.HP * (long) 100 / this.RefObject.HPMax);
            }
        }

        /// <summary>
        /// Cập nhật tên đối tượng
        /// </summary>
        public void UpdateName()
        {
            if (this.UIHeader == null)
            {
                return;
            }
            this.UIHeader.Name = this.RefObject.RoleName;
        }

        /// <summary>
        /// Cập nhật danh hiệu đối tượng
        /// </summary>
        public void UpdateTitle()
        {
            if (this.UIHeader == null)
            {
                return;
            }
            this.UIHeader.Title = this.RefObject.Title;
        }

        /// <summary>
        /// Cập nhật hiển thị trạng thái nhiệm vụ của NPC
        /// </summary>
        /// <param name="taskState"></param>
        public void UpdateMinimapNPCTaskState(NPCTaskStates taskState)
        {
            if (this.UIMinimapReference == null)
            {
                return;
            }
            this.UIMinimapReference.NPCTaskState = taskState;
        }
        #endregion
    }
}
