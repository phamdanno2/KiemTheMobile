﻿using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSPlay.KiemVu.Control.Component
{
    public partial class Item : IEvent
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
        /// Sự kiện khi đối tượng được chọn
        /// </summary>
        public void OnClick()
        {
            Global.Data.GameScene.DropItemClick(this.RefObject);
        }
    }
}
