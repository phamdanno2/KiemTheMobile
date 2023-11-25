using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main.MainUI.NearbyPlayer
{
    /// <summary>
    /// Ô thông tin người chơi
    /// </summary>
    public class UINearbyPlayer_PlayerInfo : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton;

        /// <summary>
        /// Text tên người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerName;

        /// <summary>
        /// Text cấp độ người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerLevel;

        /// <summary>
        /// Text môn phái người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_PlayerFaction;
        #endregion

        #region Properties
        private RoleData _RoleData;
        /// <summary>
        /// Dữ liệu người chơi
        /// </summary>
        public RoleData RoleData
        {
            get
            {
                return this._RoleData;
            }
            set
            {
                this._RoleData = value;
                if (value == null)
                {
                    return;
                }
                this.UIText_PlayerName.text = value.RoleName;
                this.UIText_PlayerLevel.text = value.Level.ToString();
                this.UIText_PlayerFaction.text = KTGlobal.GetFactionName(value.FactionID, out Color factionColor);
                this.UIText_PlayerFaction.color = factionColor;
            }
        }

        /// <summary>
        /// Sự kiện khi người chơi được chọn
        /// </summary>
        public Action Click { get; set; }

        /// <summary>
        /// Màu tên đối tượng
        /// </summary>
        public Color NameColor
        {
            get
            {
                return this.UIText_PlayerName.color;
            }
            set
            {
                this.UIText_PlayerName.color = value;
            }
        }

        /// <summary>
        /// Có đang hiển thị không
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.UIText_PlayerName.fontSize = 26;
            this.UIText_PlayerLevel.fontSize = 26;
            this.UIText_PlayerFaction.fontSize = 26;
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton.onClick.AddListener(this.Button_Clicked);
            this.UIText_PlayerName.autoSizeTextContainer = false;
            this.UIText_PlayerLevel.autoSizeTextContainer = false;
            this.UIText_PlayerFaction.autoSizeTextContainer = false;

            this.UIText_PlayerName.fontSize = 26;
            this.UIText_PlayerLevel.fontSize = 26;
            this.UIText_PlayerFaction.fontSize = 26;
        }

        /// <summary>
        /// Sự kiện khi Button được click
        /// </summary>
        private void Button_Clicked()
        {
            this.Click?.Invoke();
        }
        #endregion

        #region Public methods
        #endregion
    }
}
