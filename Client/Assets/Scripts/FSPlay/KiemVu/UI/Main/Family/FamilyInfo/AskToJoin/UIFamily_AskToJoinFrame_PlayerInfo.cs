using FSPlay.KiemVu;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiemVu.UI.Main.Family
{
    /// <summary>
    /// Thông tin thành viên xin vào
    /// </summary>
  public  class UIFamily_AskToJoinFrame_PlayerInfo : MonoBehaviour
    { 
        #region Define
        // <summary>
        /// Toggle
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UIToggle;

        /// <summary>
        /// Text tên người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerName;

        /// <summary>
        /// Text môn phái người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerFaction;

        /// <summary>
        /// Text cấp độ người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerLevel;
        /// <summary>
        /// Text cấp độ người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_DateJoinFamily;
        /// <summary>
        /// Text cấp độ người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RolePrestige;
        #endregion

        #region Properties
        private RequestJoin _Data = null;
        /// <summary>
        /// Thông tin người chơi
        /// </summary>
        public RequestJoin Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;
                if (value != null)
                {
                    this.UIText_PlayerName.text = value.RoleName;
                    this.UIText_PlayerLevel.text = value.RoleLevel.ToString();
                    this.UIText_PlayerFaction.text = KTGlobal.GetFactionName(value.RoleFactionID, out Color factionColor);
                    this.UIText_PlayerFaction.color = factionColor;
                    this.UIText_DateJoinFamily.text=  value.TimeRequest.ToString("HH:mm - dd/MM/yyyy");
                    this.UIText_RolePrestige.text = value.RolePrestige.ToString();
                }

                else
                {
                    this.UIText_PlayerName.text = "";
                    this.UIText_PlayerFaction.text = "";
                    this.UIText_PlayerLevel.text = "";
                    this.UIText_DateJoinFamily.text = "";
                }
            }
        }

        /// <summary>
        /// Sự kiện chọn người chơi
        /// </summary>
        public Action Select { get; set; }
        #endregion

        #region core MonoBehaviour
        /// <summary>
        /// Hàm này được ở frame đầu tiền
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
        {
            this.UIToggle.onValueChanged.AddListener(this.Toggle_Selected);
        }

        /// <summary>
        /// Sự kiện khi Toggle được chọn
        /// </summary>
        /// <param name="isSelected"></param>
        private void Toggle_Selected(bool isSelected)
        {
            if (isSelected)
            {
                /// Thực thi sự kiện chọn người chơi
                this.Select?.Invoke();
            }
        }
        #endregion
    }
}
