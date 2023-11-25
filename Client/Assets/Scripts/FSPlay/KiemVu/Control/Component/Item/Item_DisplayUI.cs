using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.UICore;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Hiển thị UI
    /// </summary>
    public partial class Item : IDisplayUI
    {
        #region Define
        /// <summary>
        /// Khung trên đầu vật phẩm
        /// </summary>
        private UIItemHeader UIHeader;
        #endregion

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
        }

        /// <summary>
        /// Hiện UI
        /// </summary>
        public void DisplayUI()
        {
            if (this.UIHeader == null)
            {
                CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
                //this.UIHeader = GameObject.Instantiate<UIItemHeader>(canvas.UIItemHeader);
                this.UIHeader = KTUIElementPoolManager.Instance.Instantiate<UIItemHeader>("UIItemHeader");
                this.UIHeader.ReferenceObject = this.gameObject;
                canvas.AddUnderLayerUI(this.UIHeader);
                this.UIHeader.gameObject.SetActive(true);
            }

            if (this.UIHeader != null)
            {
                GoodsData itemGD = KTGlobal.CreateItemPreview(this.ItemData);
                itemGD.Forge_level = this.RefObject.EnhanceLevel;
                this.UIHeader.Name = KTGlobal.GetItemName(itemGD);
                this.UIHeader.NameColor = this._NameColor;
                this.UIHeader.Offset = new Vector2(0, 40);
            }
        }
    }
}
