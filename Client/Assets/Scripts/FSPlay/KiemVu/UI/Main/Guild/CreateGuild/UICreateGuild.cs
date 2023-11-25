using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main.Guild
{
    /// <summary>
    /// Khung tạo bang
    /// </summary>
    public class UICreateGuild : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Text phí tạo bang
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Fee;

        /// <summary>
        /// Input tên bang
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_GuildName;

        /// <summary>
        /// Button tạo bang
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_CreateGuild;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện tạo bang
        /// </summary>
        public Action<string> CreateGuild { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
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
            this.UIText_Fee.text = string.Format("{0} Bạc", KTGlobal.GetDisplayMoney(KTGlobal.CreateGuildFee));

            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_CreateGuild.onClick.AddListener(this.ButtonCreateGuild_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }
        
        /// <summary>
        /// Sự kiện khi Button tạo bang được ấn
        /// </summary>
        private void ButtonCreateGuild_Clicked()
        {
            /// Nếu bản thân đã có bang
            if (Global.Data.RoleData.GuildID > 0)
            {
                KTGlobal.AddNotification("Bản thân đã có bang hội, không thể tạo thêm!");
                return;
            }

            /// Tên bang hội
            string guildName = Utils.BasicNormalizeString(this.UIInput_GuildName.text);

            /// Nếu chưa nhập tên
            if (string.IsNullOrEmpty(guildName))
            {
                KTGlobal.AddNotification("Hãy nhập tên bang hội!");
                return;
            }
            /// Kiểm tra có ký tự đặc biệt không
            else if (!guildName.CheckValidString())
            {
                KTGlobal.AddNotification("Tên không thể chứa ký tự đặc biệt!");
                return;
            }

            /// Thực thi sự kiện tạo bang
            this.CreateGuild?.Invoke(guildName);
        }
        #endregion
    }
}
