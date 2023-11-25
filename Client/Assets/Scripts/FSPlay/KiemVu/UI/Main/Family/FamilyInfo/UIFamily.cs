using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.UI.Main.Family;
using Server.Data;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung gia tộc
    /// </summary>

    public class UIFamily : MonoBehaviour
    {
        #region Define
        /// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Text tên gia tộc
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyName;

        /// <summary>
        /// Text tên  tộc trưởng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyMasterName;

        /// <summary>
        /// Text tên tộc phó
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyViceMasterName;

        /// <summary>
        /// Text Tổng uy danh
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_TotalPoint;

        /// <summary>
        /// Text Slogan gia tộc
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilySlogan;

        /// <summary>
        /// Button Thay đổi tôn chỉ hoạt động
        /// </summary>
        [SerializeField]
        private Button UIButton_ChangeSlogan;

        /// <summary>
        /// Khung thay đổi tôn chỉ
        /// </summary>
        [SerializeField]
        private UIFamily_ChangeSlogan UI_ChangeSloganFrame;

        /// <summary>
		/// Button Thay yêu cầu tham gia vào tộc
		/// </summary>
		[SerializeField]
        private UnityEngine.UI.Button UIButton_ChangeAskToJoinFamilyRequirement;

        /// <summary>
        /// Khung thay yêu cầu tham gia tộc
        /// </summary>
        [SerializeField]
        private UIFamily_ChangeAskToJoinRequirement UI_ChangeFamilyJoinRequirementFrame;

        /// <summary>
        /// Button mở khung danh sách xin vào tộc
        /// </summary>
        [SerializeField]
        private Button UIButton_OpenAskToJoinFamilyFrame;

        /// <summary>
        /// Khung danh sách thành viên gia tộc
        /// </summary>
        [SerializeField]
        private UIFamily_MemberList UI_FamilyMembersFrame;

        /// <summary>
        /// Khung danh sách thành viên xin vào gia tộc
        /// </summary>
        [SerializeField]
        private UIFamily_AskToJoinFrame UI_AskToJoinFamilyFrame;

        /// <summary>
        /// Text tiền gia tộc
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MoneyCount;

        /// <summary>
        /// Text số làn vượt ải
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_WeeklyCopySceneTimes;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform tôn chỉ
        /// </summary>
        private RectTransform transformSloganText = null;
		#endregion

		#region Properties
		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

        /// <summary>
        /// Sự kiện thay đổi slogan
        /// </summary>
        public Action<string> ChangeSlogan { get; set; }

        /// <summary>
        /// Sự kiện thay đổi yêu cầu tham gia tộc
        /// </summary>
        public Action<string> ChangeAskToJoinFamilyRequirement { get; set; }

        /// <summary>
        /// Sự kiện thay đổi chức vụ
        /// </summary>
        public Action<FamilyMember, int> Approve { get; set; }

        /// <summary>
        /// Sự kiện Khai trừ ra khỏi tộc
        /// </summary>
        public Action<FamilyMember> KickOut { get; set; }

        /// <summary>
        /// Sự kiện đồng ý
        /// </summary>
        public Action<int> AcceptToJoinFamily { get; set; }

        /// <summary>
        /// Sự kiện từ chối
        /// </summary>
        public Action<int> RefuseToJoinFamily { get; set; }

        /// <summary>
        /// sự kiện rời tộc
        /// </summary>
        public Action QuitFamily { get; set; }

        /// <summary>
        /// Thông tin gia tộc
        /// </summary>
        public Server.Data.Family Data { get; set; }
		#endregion

		#region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
		private void Awake()
		{
            this.transformSloganText = this.UIText_FamilySlogan.transform.parent.GetComponent<RectTransform>();
		}

		/// <summary>
		/// Hàm này được ở frame đầu tiền
		/// </summary>
		private void Start()
        {
            this.InitPrefabs();
            this.Refresh();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_ChangeSlogan.onClick.AddListener(this.ButtonChangeSlogan_Clicked);
            this.UIButton_ChangeAskToJoinFamilyRequirement.onClick.AddListener(this.ButtonChangeFamilyAskToJoinRequirement_Clicked);
            this.UIButton_OpenAskToJoinFamilyFrame.onClick.AddListener(this.ButtonOpenAskToJoinFamilyFrame_Clicked);

            /// Bắt sự kiện cho các khung thành phần
            this.UI_FamilyMembersFrame.Approve = this.Approve;
            this.UI_FamilyMembersFrame.KickOut = this.KickOut;
            this.UI_FamilyMembersFrame.QuitFamily = this.QuitFamily;

            this.UI_AskToJoinFamilyFrame.Accept = this.AcceptToJoinFamily;
            this.UI_AskToJoinFamilyFrame.Refuse = this.RefuseToJoinFamily;
            this.UI_AskToJoinFamilyFrame.Back = this.OpenFamilyMemberListFrame;

            this.UI_ChangeSloganFrame.ChangeSlogan = this.ChangeSlogan;
            this.UI_ChangeSloganFrame.Back = this.OpenFamilyMemberListFrame;

            this.UI_ChangeFamilyJoinRequirementFrame.ChangeAskToJoinRequirement = this.ChangeAskToJoinFamilyRequirement;
            this.UI_ChangeFamilyJoinRequirementFrame.Back = this.OpenFamilyMemberListFrame;

            /// Nếu không phải tộc trưởng và tộc phó
            if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
			{
                /// Ẩn các Button chức năng
                this.UIButton_ChangeSlogan.gameObject.SetActive(false);
                this.UIButton_ChangeAskToJoinFamilyRequirement.gameObject.SetActive(false);
                this.UIButton_OpenAskToJoinFamilyFrame.gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Sự kiện thay đổi slowgan
        /// </summary>
        private void ButtonChangeSlogan_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null)
            {
                KTGlobal.AddNotification("Không có dữ liệu.");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
			{
                KTGlobal.AddNotification("Chỉ có tộc trưởng hoặc tộc phó mới có quyền sửa tôn chỉ gia tộc.");
                return;
            }

            /// Chuyển qua Tab sửa tôn chỉ gia tộc
            this.UI_AskToJoinFamilyFrame.gameObject.SetActive(false);
            this.UI_ChangeSloganFrame.gameObject.SetActive(true);
            this.UI_ChangeFamilyJoinRequirementFrame.gameObject.SetActive(false);
            this.UI_FamilyMembersFrame.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sự kiện khi Button thay đổi yêu cầu tham gia tộc
        /// </summary>
        private void ButtonChangeFamilyAskToJoinRequirement_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null)
            {
                KTGlobal.AddNotification("Không có dữ liệu.");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
            {
                KTGlobal.AddNotification("Chỉ có tộc trưởng hoặc tộc phó mới có quyền sửa tôn chỉ gia tộc.");
                return;
            }

            /// Chuyển qua Tab sửa yêu cầu tham gia gia tộc
            this.UI_AskToJoinFamilyFrame.gameObject.SetActive(false);
            this.UI_ChangeSloganFrame.gameObject.SetActive(false);
            this.UI_ChangeFamilyJoinRequirementFrame.gameObject.SetActive(true);
            this.UI_FamilyMembersFrame.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sự kiện mở khung xin vào gia tộc
        /// </summary>
        private void ButtonOpenAskToJoinFamilyFrame_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null)
            {
                KTGlobal.AddNotification("Không có dữ liệu.");
                return;
            }

            /// Chuyển qua Tab danh sách xin vào tộc
            this.UI_AskToJoinFamilyFrame.gameObject.SetActive(true);
            this.UI_ChangeSloganFrame.gameObject.SetActive(false);
            this.UI_ChangeFamilyJoinRequirementFrame.gameObject.SetActive(false);
            this.UI_FamilyMembersFrame.gameObject.SetActive(false);
        }
        #endregion

        #region Private methods/// <summary>
        /// Thực thi sự kiện bỏ qua một số Frame
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrames(int skip, Action work)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            work?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            /// Nếu đối tượng chưa được kích hoạt thì bỏ qua
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            /// Thực hiện xây lại giao diện
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformSloganText);
            }));
        }

        /// <summary>
        /// Mở khung danh sách thành viên tộc
        /// </summary>
        private void OpenFamilyMemberListFrame()
		{

            this.UI_AskToJoinFamilyFrame.gameObject.SetActive(false);
            this.UI_ChangeSloganFrame.gameObject.SetActive(false);
            this.UI_ChangeFamilyJoinRequirementFrame.gameObject.SetActive(false);
            this.UI_FamilyMembersFrame.gameObject.SetActive(true);
        }

        /// <summary>
        /// Làm mới dữ liệu
        /// </summary>
        private void Refresh()
		{
            /// Nếu không có dữ liệu
            if (this.Data == null)
			{
                return;
			}

            /// Tên tộc
            this.UIText_FamilyName.text = this.Data.FamilyName;
            /// Tên tộc trưởng
            string masterName = "";
            /// Tên tộc phó
            string viceMasterName = "";
            /// Duyệt danh sách thành viên
            foreach (FamilyMember memberInfo in this.Data.Members)
			{
                /// Nếu là tộc trưởng
                if (memberInfo.Rank == (int) FamilyRank.Master)
				{
                    masterName = memberInfo.RoleName;
				}
                /// Nếu là tộc phó
                else if (memberInfo.Rank == (int) FamilyRank.ViceMaster)
				{
                    viceMasterName = memberInfo.RoleName;
				}
			}
            /// Gắn tên tộc trưởng, tộc phó
            this.UIText_FamilyMasterName.text = masterName;
            this.UIText_FamilyViceMasterName.text = viceMasterName;
            /// Tổng uy danh
            this.UIText_TotalPoint.text = this.Data.TotalPoint.ToString();
            /// Tôn chỉ
            this.UIText_FamilySlogan.text = this.Data.Notification;
            /// Tiền
            this.UIText_MoneyCount.text = KTGlobal.GetDisplayMoney(this.Data.FamilyMoney);
            /// Số lần vượt ải
            this.UIText_WeeklyCopySceneTimes.text = string.Format("{0}/2", this.Data.WeeklyFubenCount);

            /// Đổ dữ liệu cho các khung thành phần
            this.UI_FamilyMembersFrame.MembersList = this.Data.Members;
            this.UI_AskToJoinFamilyFrame.Data = this.Data.JoinRequest;
            this.UI_ChangeSloganFrame.CurrentSlogan = this.Data.Notification;
            this.UI_ChangeFamilyJoinRequirementFrame.CurrentAskToJoinRequirement = this.Data.RequestNotify;

            /// Xây lại giao diện
            this.RebuildLayout();
        }
		#endregion

		#region Public methods
        /// <summary>
        /// Trục xuất thành viên ra khỏi tộc
        /// </summary>
        /// <param name="roleID"></param>
        public void ServerKickOutFamilyMember(int roleID)
		{
            this.UI_FamilyMembersFrame.Remove(roleID);
		}

        /// <summary>
        /// Thay đổi điều kiện vào tộc
        /// </summary>
        /// <param name="requirement"></param>
        public void ServerChangeAskToJoinRequiremenent(string requirement)
		{
            /// Nếu đang hiện khung nhập
            if (this.UI_ChangeFamilyJoinRequirementFrame.gameObject.activeSelf)
			{
                this.UI_ChangeFamilyJoinRequirementFrame.CurrentAskToJoinRequirement = requirement;
			}
		}

        /// <summary>
        /// Thay đổi tôn chỉ
        /// </summary>
        /// <param name="slogan"></param>
        public void ServerChangeSlogan(string slogan)
		{
            /// Nếu đang hiện khung nhập
            if (this.UI_ChangeSloganFrame.gameObject.activeSelf)
			{
                this.UI_ChangeSloganFrame.CurrentSlogan = slogan;
			}
            /// Cập nhật khung tổng
            this.UIText_FamilySlogan.text = slogan;

            /// Xây lại giao diện
            this.RebuildLayout();
        }

        /// <summary>
        /// Xóa người chơi đang yêu cầu gia nhập tộc
        /// </summary>
        /// <param name="roleID"></param>
        public void ServerRemoveAskToJoinPlayer(int roleID)
		{
            this.UI_AskToJoinFamilyFrame.Remove(roleID);
		}

        /// <summary>
        /// Thêm thành viên vào tộc
        /// </summary>
        /// <param name="member"></param>
        public void ServerAddNewMember(FamilyMember member)
		{
            this.UI_FamilyMembersFrame.Add(member);
		}

        /// <summary>
        /// Làm mới dữ liệu thành viên tộc
        /// </summary>
        /// <param name="roleID"></param>
        public void RefreshMemberData(int roleID)
		{
            this.UI_FamilyMembersFrame.RefreshMemberData(roleID);
		}
		#endregion
	}
}