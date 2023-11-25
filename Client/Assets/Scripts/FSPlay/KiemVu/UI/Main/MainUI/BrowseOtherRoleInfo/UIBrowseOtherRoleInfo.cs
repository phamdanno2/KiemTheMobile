using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using UnityEngine;
using TMPro;
using Server.Data;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.GameEngine.Logic;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.Main.MainUI
{
    /// <summary>
    /// Khung kiểm tra thông tin người chơi khác
    /// </summary>
    public class UIBrowseOtherRoleInfo : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Image avarta người chơi
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_RoleAvarta;

        /// <summary>
        /// Text tên người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleName;

        /// <summary>
        /// Text cấp độ người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleLevel;

        /// <summary>
        /// Text tên môn phái của người chơi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleFaction;

        /// <summary>
        /// Button mời vào nhóm
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_InviteToTeam;

        /// <summary>
        /// Button xin vào nhóm
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AskToJoinTeam;

        /// <summary>
        /// Button mở khung thông tin trang bị
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_OpenEquipInfo;

        /// <summary>
        /// Button thêm bạn
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AddFriend;

        /// <summary>
        /// Button giao dịch
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Exchange;

        /// <summary>
        /// Button đồ sát
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Fight;

        /// <summary>
        /// Button tỷ thí
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Challenge;

        /// <summary>
        /// Button chat mật
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_PrivateChat;

        /// <summary>
        /// Button chiêu mộ tộc
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_InviteToGuild;

        /// <summary>
        /// Button xin vào bang
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AskToJoinGuild;
        #endregion

        #region Private fields

        #endregion

        #region Properties
        private RoleData _RoleData;
        /// <summary>
        /// Thông tin dữ liệu người chơi
        /// </summary>
        public RoleData RoleData
        {
            get
            {
                return this._RoleData;
            }
            set
            {
                if (this._RoleData == value)
                {
                    return;
                }

                this._RoleData = value;
                this.RefreshData();
            }
        }

        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện mời vào nhóm
        /// </summary>
        public Action InviteToTeam { get; set; }

        /// <summary>
        /// Sư kiện xin gia nhập nhóm
        /// </summary>
        public Action AskToJoinTeam { get; set; }

        /// <summary>
        /// Sự kiện mở khung thông tin trang bị
        /// </summary>
        public Action OpenEquipInfo { get; set; }

        /// <summary>
        /// Sự kiện thêm hảo hữu
        /// </summary>
        public Action AddFriend { get; set; }

        /// <summary>
        /// Sự kiện bắt đầu giao dịch
        /// </summary>
        public Action Exchange { get; set; }

        /// <summary>
        /// Sự kiện đồ sát
        /// </summary>
        public Action Fight { get; set; }

        /// <summary>
        /// Sự kiện tỷ thí
        /// </summary>
        public Action Challenge { get; set; }

        /// <summary>
        /// Sự kiện chat mật
        /// </summary>
        public Action PrivateChat { get; set; }

        /// <summary>
        /// Sự kiện chiêu mộ tộc
        /// </summary>
        public Action InviteToGuild { get; set; }

        /// <summary>
        /// Sự kiện xin vào bang
        /// </summary>
        public Action AskToJoinGuild { get; set; }
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
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_InviteToTeam.onClick.AddListener(this.ButtonInviteToTeam_Clicked);
            this.UIButton_AskToJoinTeam.onClick.AddListener(this.ButtonAskToJoinTeam_Clicked);
            this.UIButton_OpenEquipInfo.onClick.AddListener(this.ButtonBrowseEquipInfo_Clicked);
            this.UIButton_AddFriend.onClick.AddListener(this.ButtonAddFriend_Clicked);
            this.UIButton_Exchange.onClick.AddListener(this.ButtonExchange_Clicked);
            this.UIButton_Fight.onClick.AddListener(this.ButtonFight_Clicked);
            this.UIButton_Challenge.onClick.AddListener(this.ButtonChallenge_Clicked);
            this.UIButton_PrivateChat.onClick.AddListener(this.ButtonPrivateChat_Clicked);
            this.UIButton_InviteToGuild.onClick.AddListener(this.ButtonInviteToGuild_Clicked);
            this.UIButton_AskToJoinGuild.onClick.AddListener(this.ButtonAskToJoinGuild_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button mời vào nhóm được ấn
        /// </summary>
        private void ButtonInviteToTeam_Clicked()
        {
            this.InviteToTeam?.Invoke();
            this.Close?.Invoke();
        }
        
        /// <summary>
        /// Sự kiện khi Button xin gia nhập nhóm được ấn
        /// </summary>
        private void ButtonAskToJoinTeam_Clicked()
        {
            this.AskToJoinTeam?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button thông tin trang bị được ấn
        /// </summary>
        private void ButtonBrowseEquipInfo_Clicked()
        {
            this.OpenEquipInfo?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button thêm hảo hữu được ấn
        /// </summary>
        private void ButtonAddFriend_Clicked()
        {
            this.AddFriend?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button giao dịch được ấn
        /// </summary>
        private void ButtonExchange_Clicked()
        {
            this.Exchange?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button đồ sát được ấn
        /// </summary>
        private void ButtonFight_Clicked()
        {
            this.Fight?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button tỷ thí được ấn
        /// </summary>
        private void ButtonChallenge_Clicked()
        {
            this.Challenge?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button chat mật được ấn
        /// </summary>
        private void ButtonPrivateChat_Clicked()
        {
            this.PrivateChat?.Invoke();
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button chiêu mộ gia tộc vào bang
        /// </summary>
        private void ButtonInviteToGuild_Clicked()
		{
            this.InviteToGuild?.Invoke();
            this.Close?.Invoke();
		}

        /// <summary>
        /// Sự kiện khi Button xin vào bang được ấn
        /// </summary>
        private void ButtonAskToJoinGuild_Clicked()
		{
            this.AskToJoinGuild?.Invoke();
            this.Close?.Invoke();
		}
        #endregion

        #region Private methods
        /// <summary>
        /// Làm mới hiển thị
        /// </summary>
        private void RefreshData()
        {
            this.UpdateFaction();
            this.UpdateAvarta();
            this.UIText_RoleName.text = this.RoleData.RoleName;
            this.UIText_RoleLevel.text = this.RoleData.Level.ToString();

            /// Nếu bản thân là bang chủ, và đối phương là trưởng tộc và chưa có bang
            if (!(Global.Data.RoleData.GuildID > 0 && Global.Data.RoleData.GuildRank == (int) GuildRank.Master && this._RoleData.GuildID <= 0 && this._RoleData.FamilyID > 0 && this._RoleData.FamilyRank == (int) FamilyRank.Master))
            {
                this.UIButton_InviteToGuild.gameObject.SetActive(false);
            }

            /// Nếu bản thân là tộc trưởng, chưa có bang và đối phương là bang chủ bang khác
            if (!(this._RoleData.GuildID > 0 && this._RoleData.GuildRank == (int) GuildRank.Master && Global.Data.RoleData.GuildID <= 0 && Global.Data.RoleData.FamilyID > 0 && Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master))
            {
                this.UIButton_AskToJoinGuild.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Cập nhật môn phái
        /// </summary>
        private void UpdateFaction()
        {
            this.UIText_RoleFaction.text = KTGlobal.GetFactionName(this.RoleData.FactionID, out Color factionColor);
            this.UIText_RoleFaction.color = factionColor;
        }

        /// <summary>
        /// Cập nhật mặt
        /// </summary>
        private void UpdateAvarta()
        {
            if (Loader.Loader.RoleAvartas.TryGetValue(this.RoleData.RolePic, out RoleAvartaXML roleAvarta))
            {
                this.UIImage_RoleAvarta.BundleDir = roleAvarta.BundleDir;
                this.UIImage_RoleAvarta.AtlasName = roleAvarta.AtlasName;
                this.UIImage_RoleAvarta.SpriteName = roleAvarta.SpriteName;
                this.UIImage_RoleAvarta.Load();
            }
            else
            {
                KTDebug.LogError("Can not find RoleAvarta ID = " + this.RoleData.RolePic);
            }
        }

        /// <summary>
        /// Làm mới hiển thị Button
        /// </summary>
        private void RefreshButtons()
        {
            this.UIButton_InviteToTeam.gameObject.SetActive(false);
            this.UIButton_AskToJoinTeam.gameObject.SetActive(false);

            /// Nếu bản thân đã có nhóm
            if (Global.Data.RoleData.TeamID != -1)
            {
                /// Nếu đối phương chưa có nhóm
                if (this.RoleData.TeamID == -1)
                {
                    this.UIButton_InviteToTeam.gameObject.SetActive(true);
                }
            }
            /// Nếu bản thân chưa có nhóm
            else
            {
                /// Nếu đối phương đã có nhóm
                if (this.RoleData.TeamID != -1)
                {
                    this.UIButton_AskToJoinTeam.gameObject.SetActive(true);
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiện
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
            this.RefreshButtons();
        }

        /// <summary>
        /// Ẩn
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
