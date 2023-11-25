using UnityEngine.UI;
using TMPro;
using System;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.UI.CoreUI;
using UnityEngine;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.Factory;
using Server.Data;
using static FSPlay.KiemVu.Entities.Enum;
using FSPlay.KiemVu.UI.UICore;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Quản lý hiển thị UI
    /// </summary>
    public partial class Character : IDisplayUI
    {
        #region Define
        /// <summary>
        /// Khung trên đầu nhân vật
        /// </summary>
        private UIRoleHeader UIHeader;

        /// <summary>
        /// Khung biểu diễn trên Minimap
        /// </summary>
        private UIMinimapReference UIMinimapReference;

        /// <summary>
        /// Khung tên sạp hàng đang bày bán
        /// </summary>
        private UIRoleShopName UIRoleShopName;
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
                this.UIHeader = KTUIElementPoolManager.Instance.Instantiate<UIRoleHeader>("UIRoleHeader");
                this.UIHeader.ReferenceObject = this.gameObject;
                canvas.AddUnderLayerUI(this.UIHeader);
                this.UIHeader.gameObject.SetActive(true);
                this.UIHeader.IsLeader = Global.Data.Leader == this.RefObject;
            }

            if (this.UIHeader != null)
            {
                this.UIHeader.IsLeader = Global.Data.Leader == this.RefObject;
            }

            if (this.UIMinimapReference == null)
            {
                MinimapCanvasManager canvas = Global.MinimapCanvas.GetComponent<MinimapCanvasManager>();
                this.UIMinimapReference = KTUIElementPoolManager.Instance.Instantiate<UIMinimapReference>("UIMinimapReference");
                canvas.AddUI(this.UIMinimapReference);
                this.UIMinimapReference.ReferenceObject = this.gameObject;
                this.UIMinimapReference.gameObject.SetActive(true);
            }

            if (this.RefObject != null)
            {
                if (this.UIHeader != null)
                {
                    this.UIHeader.NameColor = this._NameColor;
                    this.UIHeader.HPBarColor = this._HPBarColor;
                    this.UIHeader.IsShowMPBar = this._ShowMPBar;
                    this.UIHeader.Name = this.RefObject.RoleName;
                }
                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.ShowIcon = this._ShowMinimapIcon;
                    this.UIMinimapReference.ShowName = this._ShowMinimapName;
                    this.UIMinimapReference.Name = this.RefObject.RoleName;
                    this.UIMinimapReference.NameColor = this._MinimapNameColor;
                    this.UIMinimapReference.BundleDir = KTGlobal.MinimapIconBundleDir;
                    this.UIMinimapReference.AtlasName = KTGlobal.MinimapIconAtlasName;
                    if (this.RefObject == Global.Data.Leader)
                    {
                        this.UIMinimapReference.SpriteName = KTGlobal.MinimapLeaderIconSpriteName;
                    }
                    this.UIMinimapReference.IconSize = this._MinimapIconSize;
                }
            }

            if (this.UIRoleShopName == null)
            {
                CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
                this.UIRoleShopName = KTUIElementPoolManager.Instance.Instantiate<UIRoleShopName>("UIRoleShopName");
                this.UIRoleShopName.IsLeader = Global.Data.Leader == this.RefObject;
                this.UIRoleShopName.ReferenceObject = this.gameObject;
                this.UIRoleShopName.gameObject.SetActive(true);
                canvas.AddUnderLayerUI(this.UIRoleShopName);
                this.UIRoleShopName.Hide();
            }

            this.UpdateGuildTitle();
            this.UpdateTitle();
            this.UpdateHP();
            this.UpdateRoleValue();
            this.UpdateRoleOfficeRank();
            this.UpdateMyselfRoleTitle();
            if (Global.Data.RoleData.RoleID == this.RefObject.RoleID)
            {
                this.UpdateMP();
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
            }

            if (this.UIMinimapReference != null)
            {
                this.UIMinimapReference.Destroy();
            }

            if (this.UIRoleShopName != null)
            {
                this.UIRoleShopName.Destroy();
            }
        }

        /// <summary>
        /// Cập nhật màu tên và thanh máu căn cứ trạng thái PK
        /// </summary>
        public void UpdateUIHeaderColor()
        {
            /// Nếu đối tượng là Leader
            if (this.RefObject == Global.Data.Leader)
            {
                return;
            }
            /// Nếu không có UIHeader
            else if (this.UIHeader == null)
            {
                return;
            }

            /// Nếu mục tiêu tiềm ẩn nguy hiểm
            if (KTGlobal.IsDangerous(this.RefObject))
            {
                this.UIHeader.NameColor = KTGlobal.DangerousPlayerNameColor;
                this.UIHeader.HPBarColor = KTGlobal.DangerousPlayerNameColor;
            }
            /// Nếu là kẻ địch
            else if (KTGlobal.IsEnemy(this.RefObject))
            {
                this.UIHeader.NameColor = KTGlobal.EnemyPlayerNameColor;
                this.UIHeader.HPBarColor = KTGlobal.EnemyPlayerNameColor;

                if (this.UIMinimapReference.SpriteName != KTGlobal.MinimapEnemyRoleIconSpriteName)
                {
                    this.UIMinimapReference.SpriteName = KTGlobal.MinimapEnemyRoleIconSpriteName;
                }

            }
            /// Nếu là đồng đội
            else if (KTGlobal.IsTeammate(this.RefObject))
            {
                this.UIHeader.RestoreColor();

                if (this.UIMinimapReference.SpriteName != KTGlobal.MinimapTeammateRoleIconSpriteName)
                {
                    this.UIMinimapReference.SpriteName = KTGlobal.MinimapTeammateRoleIconSpriteName;
                }
            }
            /// Nếu không phải kẻ địch
            else
            {
                this.UIHeader.RestoreColor();

                if (this.UIMinimapReference.SpriteName != KTGlobal.MinimapOtherRoleIconSpriteName)
                {
                    this.UIMinimapReference.SpriteName = KTGlobal.MinimapOtherRoleIconSpriteName;
                }
            } 
        }

        /// <summary>
        /// Hiển thị tên sạp hàng bản thân
        /// </summary>
        /// <param name="shopName"></param>
        public void ShowMyselfShopName(string shopName)
        {
            if (this.UIRoleShopName == null)
            {
                return;
            }
            /// Nếu không có sạp hàng
            else if (string.IsNullOrEmpty(shopName))
            {
                return;
            }

            this.UIRoleShopName.ShopName = shopName;
            this.UIRoleShopName.Show();
        }

        /// <summary>
        /// Ẩn sạp hàng bản thân
        /// </summary>
        public void HideMyselfShopName()
        {
            if (this.UIRoleShopName == null)
            {
                return;
            }

            this.UIRoleShopName.ShopName = "";
            this.UIRoleShopName.Hide();
        }
        #endregion

        #region Update changes
        /// <summary>
        /// Cập nhật trạng thái cưỡi
        /// </summary>
        public void UpdateRidingState()
        {
            /// Nếu là Leader
            if (this.RefObject.RoleID == Global.Data.RoleData.RoleID)
            {
                if (this.UIHeader != null)
                {
                    this.UIHeader.ForceSynsPositionImmediately();
                }
            }
        }

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
                this.UIHeader.HPPercent = this.RefObject.HP * 100 / this.RefObject.HPMax;
            }
        }

        /// <summary>
        /// Cập nhật Mana
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="maxMP"></param>
        public void UpdateMP()
        {
            if (this.UIHeader == null)
            {
                return;
            }

            if (this.UIHeader.IsShowMPBar)
            {
                if (this.RefObject.MPMax != 0)
                {
                    this.UIHeader.MPPercent = this.RefObject.MP * 100 / this.RefObject.MPMax;
                }
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
        /// Cập nhật danh hiệu bang hội đối tượng
        /// </summary>
        public void UpdateGuildTitle()
        {
            if (this.UIHeader == null)
            {
                return;
            }
            this.UIHeader.GuildTitle = this.RefObject.GuildTitle;
        }

        /// <summary>
        /// Cập nhật vinh dự tài phú
        /// </summary>
        public void UpdateRoleValue()
        {
            if (this.UIHeader == null)
            {
                return;
            }
            this.UIHeader.RoleValue = this.RefObject.RoleData.TotalValue;
        }

        /// <summary>
        /// Cập nhật quan hàm
        /// </summary>
        public void UpdateRoleOfficeRank()
		{
            if (this.UIHeader == null)
			{
                return;
			}
            this.UIHeader.RoleOfficeRank = this.RefObject.RoleData.OfficeRank;

            /// Thêm hiệu ứng tương ứng
            KTGlobal.RefreshOfficeRankEffect(this.RefObject);
		}

        /// <summary>
        /// Cập nhật danh hiệu nhân vật hiện tại của bản thân
        /// </summary>
        public void UpdateMyselfRoleTitle()
		{
            if (this.UIHeader == null)
			{
                return;
			}
            this.UIHeader.CurrentRoleTitle = this.RefObject.RoleData.SelfCurrentTitleID;
		}
        #endregion
    }
}
