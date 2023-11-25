﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Control.Component;
using FSPlay.GameEngine.Logic;
using System;
using Server.Data;

namespace FSPlay.KiemVu.UI.RoleManager
{
    /// <summary>
    /// Khung thông tin nhân vật của màn hình chọn nhân vật
    /// </summary>
    public class UISelectRole_RoleInfoToggle : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Toggle
        /// </summary>
        [SerializeField]
        private Utilities.UnityUI.UIToggleSprite UIToggle;

        /// <summary>
        /// Ảnh hiển thị xem trước nhân vật
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.RawImage UIImage_RolePreview;

        /// <summary>
        /// Text tên nhân vật
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleName;

        /// <summary>
        /// Text cấp độ nhân vật
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleLevel;

        /// <summary>
        /// Text môn phái nhân vật
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleClass;
        #endregion

        #region Properties
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public bool Active
        {
            get
            {
                return this.UIToggle.Active;
            }
            set
            {
                this.UIToggle.Active = value;
            }
        }

        private RoleSelectorData _RoleData;
        /// <summary>
        /// Thông tin nhân vật
        /// </summary>
        public RoleSelectorData RoleData
        {
            get
            {
                return this._RoleData;
            }
            set
            {
                this._RoleData = value;
                this.UpdateRole();
            }
        }

        /// <summary>
        /// Đối tượng được tham chiếu
        /// </summary>
        private CharacterPreview previewRole;

        /// <summary>
        /// Sự kiện khi đối tượng được chon
        /// </summary>
        public Action<bool> OnSelected { get; set; }

        /// <summary>
        /// Đang bận
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        /// <summary>
        /// Sự kiện khi đối tượng đã được tải xuống hoàn tất
        /// </summary>
        public Action Ready { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng bị hủy
        /// </summary>
        private void OnDestroy()
        {
            if (this.previewRole != null)
            {
                GameObject.Destroy(this.previewRole.gameObject);
            }
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIToggle.OnSelected = this.OnSelected;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Cập nhật thông tin nhân vật
        /// </summary>
        public void UpdateRole()
        {
            if (this.previewRole == null)
            {
                this.previewRole = Object2DFactory.MakeRolePreview();
            }
            this.previewRole.Data = new RoleDataMini()
            {
                IsRiding = false,
                RoleSex = this._RoleData.Sex,
                ArmorID = this._RoleData.ArmorID,
                HelmID = this._RoleData.HelmID,
                WeaponID = this._RoleData.WeaponID,
                WeaponEnhanceLevel = this._RoleData.WeaponEnhanceLevel,
            };
            this.previewRole.UpdateRoleData();
            this.previewRole.Direction = Entities.Enum.Direction.DOWN;
            this.UIImage_RolePreview.gameObject.SetActive(true);

            this.previewRole.OnStart = () => {
                this.UIImage_RolePreview.texture = this.previewRole.ReferenceCamera.targetTexture;
            };

            this.UIText_RoleName.text = this._RoleData.Name;
            this.UIText_RoleLevel.text = this._RoleData.Level + "";
            string factionName = KTGlobal.GetFactionName(this._RoleData.FactionID, out Color color);
            this.UIText_RoleClass.text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), factionName);

            /// Bỏ đánh dấu đang bận tải
            this.IsBusy = false;
            this.Ready?.Invoke();
        }
        #endregion
    }
}