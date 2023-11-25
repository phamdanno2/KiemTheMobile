using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.GameEngine.Logic;
using Server.Data;
using static FSPlay.KiemVu.Entities.Enum;
using FSPlay.KiemVu.UI.Main.Guild;

namespace FSPlay.KiemVu.UI.Main
{
	/// <summary>
	/// Khung bang hội tổng quan
	/// </summary>
	public class UIGuild : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Text tên bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildName;

		/// <summary>
		/// Text tên bang chủ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildMasterName;

		/// <summary>
		/// Text tổng số tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_FamilyCount;

		/// <summary>
		/// Text tổng số thành viên bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildMemberCount;

		/// <summary>
		/// Text tổng số lãnh thổ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_ColonyCount;

		/// <summary>
		/// Text tổng số uy danh
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_TotalPoints;

		/// <summary>
		/// Text quỹ bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildMoney;

		/// <summary>
		/// Text lợi tức
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Profit;

		/// <summary>
		/// Button thiết lập lợi tức
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_SetProfit;

		/// <summary>
		/// Text quỹ thưởng
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_BonusAmount;

		/// <summary>
		/// Text tài sản cá nhân
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_SelfMoney;

		/// <summary>
		/// Button rút tài sản
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Withdraw;

		/// <summary>
		/// Text Slogan bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildSlogan;

		/// <summary>
		/// Button đổi Slogan
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_ChangeSlogan;

		/// <summary>
		/// Button mở khung quản lý thành viên bang
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildMemberList;

		/// <summary>
		/// Button mở khung cống hiến bang
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildDedication;

		/// <summary>
		/// Button mở khung cổ tức bang hội
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildShare;

		/// <summary>
		/// Button mở khung hoạt động bang
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildActivity;

		/// <summary>
		/// Button mở khung bầu ưu tú
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildExcellenceVote;

		/// <summary>
		/// Button rời bang
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_LeaveGuild;

		/// <summary>
		/// Khung sửa tôn chỉ
		/// </summary>
		[SerializeField]
		private UIGuild_ChangeSlogan UI_ChangeSloganFrame;
		#endregion

		#region Properties
		/// <summary>
		/// Dữ liệu
		/// </summary>
		public GuildInfomation Data { get; set; }

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện đổi tôn chỉ bang
		/// </summary>
		public Action<string> ChangeSlogan { get; set; }

		/// <summary>
		/// Sự kiện thay đổi lợi tức bang
		/// </summary>
		public Action<int> SetProfit { get; set; }

		/// <summary>
		/// Sự kiện rút tài sản cá nhân
		/// </summary>
		public Action<int> WithdrawSelfMoney { get; set; }

		/// <summary>
		/// Sự kiện mở khung danh sách thành viên bang hội
		/// </summary>
		public Action OpenGuildMemberList { get; set; }

		/// <summary>
		/// Sự kiện mở khung cống hiến bang hội
		/// </summary>
		public Action OpenGuildDedication { get; set; }

		/// <summary>
		/// Sự kiện mở khung hoạt động bang hội
		/// </summary>
		public Action OpenGuildActivity { get; set; }

		/// <summary>
		/// Sự kiện mở khung cổ tức bang hội
		/// </summary>
		public Action OpenGuildOpenGuildShare { get; set; }

		/// <summary>
		/// Sự kiện mở khung bầu ưu tú bang hội
		/// </summary>
		public Action OpenGuildExcellenceVote { get; set; }

		/// <summary>
		/// Sự kiện rời bang hội
		/// </summary>
		public Action LeaveGuild { get; set; }

		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi ở Frame đầu tiên
		/// </summary>
		private void Start()
		{
			this.InitPrefabs();
			this.Refresh();
		}
		#endregion

		#region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
		{
			this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
			this.UIButton_OpenGuildMemberList.onClick.AddListener(this.ButtonOpenGuildMemberList_Clicked);
			this.UIButton_OpenGuildDedication.onClick.AddListener(this.ButtonOpenGuildDedication_Clicked);
			this.UIButton_OpenGuildActivity.onClick.AddListener(this.ButtonOpenGuildActivity_Clicked);
			this.UIButton_OpenGuildShare.onClick.AddListener(this.ButtonOpenGuildShare_Clicked);
			this.UIButton_OpenGuildExcellenceVote.onClick.AddListener(this.ButtonOpenGuildExcellenceVote_Clicked);

			/// Nếu bản thân là tộc trưởng của tộc
			if (Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master)
			{
				this.UIButton_LeaveGuild.onClick.AddListener(this.ButtonLeaveGuild_Clicked);
			}
			else
			{
				this.UIButton_LeaveGuild.gameObject.SetActive(false);
			}

			this.UIButton_ChangeSlogan.onClick.AddListener(this.ButtonChangeSlogan_Clicked);
			this.UIButton_SetProfit.onClick.AddListener(this.ButtonSetProfit_Clicked);
			this.UIButton_Withdraw.onClick.AddListener(this.ButtonWithdrawSelfMoney_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button sửa tôn chỉ được ấn
		/// </summary>
		private void ButtonChangeSlogan_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu bang hội!");
				return;
			}
			/// Nếu không có bang
			else if (Global.Data.RoleData.GuildID <= 0)
			{
				KTGlobal.AddNotification("Bạn không có bang hội!");
				return;
			}
			/// Nếu không phải bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master && Global.Data.RoleData.GuildRank != (int) GuildRank.ViceMaster)
			{
				KTGlobal.AddNotification("Chỉ có Bang chủ và Phó bang chủ mới có quyền sửa tôn chỉ bang hội!");
				return;
			}

			/// Hiện khung sửa tôn chỉ
			this.UI_ChangeSloganFrame.CurrentSlogan = this.Data.Notify;
			this.UI_ChangeSloganFrame.OK = (slogan) => {
				/// Đóng khung
				this.UI_ChangeSloganFrame.Hide();
				/// Thực thi sự kiện
				this.ChangeSlogan?.Invoke(slogan);
			};
			this.UI_ChangeSloganFrame.Show();
		}

		/// <summary>
		/// Sự kiện khi Button thiết lập lợi tức được ấn
		/// </summary>
		private void ButtonSetProfit_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu bang hội!");
				return;
			}
			/// Nếu không có bang
			else if (Global.Data.RoleData.GuildID <= 0)
			{
				KTGlobal.AddNotification("Bạn không có bang hội!");
				return;
			}
			/// Nếu không phải bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master && Global.Data.RoleData.GuildRank != (int) GuildRank.ViceMaster)
			{
				KTGlobal.AddNotification("Chỉ có Bang chủ và Phó bang chủ mới có quyền sửa tôn chỉ bang hội!");
				return;
			}

			KTGlobal.ShowInputNumber("Nhập lợi tức bang hội.", 50, 100, (number) => {
				/// Thực thi sự kiện
				this.SetProfit?.Invoke(number);
			});
		}

		/// <summary>
		/// Sự kiện khi Button rút tài sản cá nhân được ấn
		/// </summary>
		private void ButtonWithdrawSelfMoney_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu bang hội!");
				return;
			}
			/// Nếu không có tài sản cá nhân
			else if (this.Data.RoleGuildMoney <= 1)
			{
				KTGlobal.AddNotification("Bạn không có tài sản cá nhân trong bang hội!");
				return;
			}

			KTGlobal.ShowInputNumber("Nhập số bạc muốn rút.", 1, this.Data.RoleGuildMoney, (amount) => {
				/// Thực thi sự kiện
				this.WithdrawSelfMoney?.Invoke(amount);
			});
		}

		/// <summary>
		/// Sự kiện khi Button mở khung danh sách thành viên bang hội được ấn
		/// </summary>
		private void ButtonOpenGuildMemberList_Clicked()
		{

			/// Thực thi sự kiện
			this.OpenGuildMemberList?.Invoke();
			/// Đóng khung
			this.ButtonClose_Clicked();
		}

		/// <summary>
		/// Sự kiện khi Button mở khung cống hiến bang hội được ấn
		/// </summary>
		private void ButtonOpenGuildDedication_Clicked()
		{

			/// Thực thi sự kiện
			this.OpenGuildDedication?.Invoke();
			/// Đóng khung
			this.ButtonClose_Clicked();
		}

		/// <summary>
		/// Sự kiện khi Button mở khung hoạt động bang hội được ấn
		/// </summary>
		private void ButtonOpenGuildActivity_Clicked()
		{

			/// Thực thi sự kiện
			this.OpenGuildActivity?.Invoke();
			/// Đóng khung
			this.ButtonClose_Clicked();
		}

		/// <summary>
		/// Sự kiện khi Button mở khung cổ tức bang hội
		/// </summary>
		private void ButtonOpenGuildShare_Clicked()
		{

			/// Thực thi sự kiện
			this.OpenGuildOpenGuildShare?.Invoke();
			/// Đóng khung
			this.ButtonClose_Clicked();
		}

		/// <summary>
		/// Sự kiện khi Button mở khung bầu ưu tú bang hội
		/// </summary>
		private void ButtonOpenGuildExcellenceVote_Clicked()
		{
			/// Thực thi sự kiện
			this.OpenGuildExcellenceVote?.Invoke();
			/// Đóng khung
			this.ButtonClose_Clicked();
		}

		/// <summary>
		/// Sự kiện khi Button rời bang hội được ấn
		/// </summary>
		private void ButtonLeaveGuild_Clicked()
		{
			/// Nếu không phải tộc trưởng
			if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master)
			{
				KTGlobal.AddNotification("Chỉ tộc trưởng mới có thể rời tộc khỏi bang hội!");
				return;
			}

			/// Thực hiện hỏi lần nữa
			KTGlobal.ShowMessageBox("Thông báo", "Xác nhận rời gia tộc khỏi bang?", () => {
				/// Thực thi sự kiện
				this.LeaveGuild?.Invoke();
				/// Đóng khung
				this.ButtonClose_Clicked();
			}, true);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		public void Refresh()
		{
			/// Đóng các Button chức năng
			this.UIButton_ChangeSlogan.gameObject.SetActive(false);
			this.UIButton_SetProfit.gameObject.SetActive(false);
			this.UIButton_Withdraw.gameObject.SetActive(false);

			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Thiết lập dữ liệu
			this.UIText_GuildName.text = this.Data.GuildName;
			this.UIText_GuildMasterName.text = this.Data.GuildMasterName;
			this.UIText_FamilyCount.text = this.Data.FamilyCount.ToString();
			this.UIText_GuildMemberCount.text = this.Data.MemberCount.ToString();
			this.UIText_ColonyCount.text = this.Data.TerritoryCount.ToString();
			this.UIText_TotalPoints.text = this.Data.TotalPrestige.ToString();
			this.UIText_GuildMoney.text = KTGlobal.GetDisplayMoney(this.Data.MoneyStore);
			this.UIText_Profit.text = string.Format("{0}%", this.Data.MaxWithDraw);
			this.UIText_BonusAmount.text = KTGlobal.GetDisplayMoney(this.Data.MoneyBound);
			this.UIText_SelfMoney.text = KTGlobal.GetDisplayMoney(this.Data.RoleGuildMoney);
			this.UIText_GuildSlogan.text = this.Data.Notify;

			/// Mở các Button chức năng
			this.UIButton_ChangeSlogan.gameObject.SetActive(Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster);
			this.UIButton_SetProfit.gameObject.SetActive(Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster);
			this.UIButton_Withdraw.gameObject.SetActive(this.Data.RoleGuildMoney > 1);
		}
		#endregion
	}
}
