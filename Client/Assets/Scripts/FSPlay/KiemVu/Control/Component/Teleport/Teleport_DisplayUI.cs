using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.CoreUI;
using System;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Đối tượng điểm truyền tống
    /// </summary>
    public partial class Teleport : IDisplayUI
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
        /// Hiển thị UI
        /// </summary>
        public void DisplayUI()
        {
            if (this.UIHeader == null)
            {
                CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
                //this.UIHeader = GameObject.Instantiate<UI.CoreUI.UIMonsterHeader>(canvas.UIMonsterHeader);
                this.UIHeader = KTUIElementPoolManager.Instance.Instantiate<UIMonsterHeader>("UIMonsterHeader");
                this.UIHeader.ReferenceObject = this.gameObject;
                canvas.AddUnderLayerUI(this.UIHeader);
                this.UIHeader.gameObject.SetActive(true);
            }

            if (this.UIMinimapReference == null)
            {
                MinimapCanvasManager canvas = Global.MinimapCanvas.GetComponent<MinimapCanvasManager>();
                //this.UIMinimapReference = GameObject.Instantiate<UI.CoreUI.UIMinimapReference>(canvas.UIMinimapReference);
                this.UIMinimapReference = KTUIElementPoolManager.Instance.Instantiate<UIMinimapReference>("UIMinimapReference");
                this.UIMinimapReference.ReferenceObject = this.gameObject;
                canvas.AddUI(this.UIMinimapReference);
                this.UIMinimapReference.gameObject.SetActive(true);
            }

            if (this.UIHeader != null)
            {
                this.UIHeader.ShowHPBar = false;
                this.UIHeader.ShowElemental = false;
                this.UIHeader.Name = this.Data.Name;
                ColorUtility.TryParseHtmlString("#60ff38", out Color color);
                this.UIHeader.NameColor = color;
                this.UIHeader.Title = "";
                this.UIHeader.Offset = new Vector2(0, 50);
            }

            if (this.UIMinimapReference != null)
            {
                this.UIMinimapReference.ShowIcon = true;
                this.UIMinimapReference.ShowName = true;
                this.UIMinimapReference.Name = this.Data.Name;
                ColorUtility.TryParseHtmlString("#60ff38", out Color color);
                this.UIMinimapReference.NameColor = color;
                this.UIMinimapReference.BundleDir = KTGlobal.MinimapIconBundleDir;
                this.UIMinimapReference.AtlasName = KTGlobal.MinimapIconAtlasName;
                this.UIMinimapReference.SpriteName = KTGlobal.MinimapTeleportIconSpriteName;
                this.UIMinimapReference.IconSize = this.MinimapIconSize;
            }
        }

        /// <summary>
        /// Xóa UI
        /// </summary>
        public void DestroyUI()
        {
            if (this.UIHeader != null)
            {
                this.UIHeader.Destroy();
                this.UIHeader = null;
            }
            if (this.UIMinimapReference != null)
            {
                this.UIMinimapReference.Destroy();
                this.UIMinimapReference = null;
            }
        }
        #endregion
    }

}