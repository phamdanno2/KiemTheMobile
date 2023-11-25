using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;
using FSPlay.GameEngine.Logic;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Khung danh sách thành viên bang hội
	/// </summary>
	public class UIGuildMemberList : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Prefab thông tin thành viên
		/// </summary>
		[SerializeField]
		private UIGuildMemberList_MemberInfo UI_MemberInfo_Prefab;

		/// <summary>
		/// Prefab thông tin tộc
		/// </summary>
		[SerializeField]
		private UIGuildMemberList_FamilyInfo UI_FamilyInfo_Prefab;

		/// <summary>
		/// Text tổng số tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_FamilyCount;

		/// <summary>
		/// Button trục xuất tộc
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_KickFamily;

		/// <summary>
		/// Khung tương tác với thành viên
		/// </summary>
		[SerializeField]
		private UIGuildMemberList_MemberInteractableFrame UI_MemberInteractableFrame;

		/// <summary>
		/// Khung đổi chức vị thành viên
		/// </summary>
		[SerializeField]
		private UIGuildMemberList_SetMemberRankFrame UI_SetMemberRankFrame;

		/// <summary>
		/// Button trang trước
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Previous;

		/// <summary>
		/// Button trang tiếp theo
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Next;

		/// <summary>
		/// Text thông tin trang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_PageNumber;
		#endregion

		#region Private fields
		/// <summary>
		/// Dữ liệu
		/// </summary>
		public GuildMemberData Data { get; set; }

		/// <summary>
		/// RectTransform danh sách thành viên
		/// </summary>
		private RectTransform transformMemberList = null;

		/// <summary>
		/// RectTransform danh sách tộc
		/// </summary>
		private RectTransform transformFamilyList = null;

		/// <summary>
		/// Gia tộc đang được chọn
		/// </summary>
		private FamilyObj selectedFamily = null;
		#endregion

		#region Properties

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện trục xuất gia tộc
		/// </summary>
		public Action<int> KickOutFamily { get; set; }

		/// <summary>
		/// Sự kiện xem thông tin thành viên
		/// </summary>
		public Action<int> BrowseInfo { get; set; }

		/// <summary>
		/// Sự kiện chat mật với thành viên
		/// </summary>
		public Action<string> PrivateChat { get; set; }

		/// <summary>
		/// Sự kiện thêm bạn với thành viên
		/// </summary>
		public Action<int> AddFriend { get; set; }

		/// <summary>
		/// Sự kiện bổ nhiệm thành viên
		/// </summary>
		public Action<int, int> Approve { get; set; }

		/// <summary>
		/// Truy vấn thông tin thành viên theo trang
		/// </summary>
		public Action<int> QueryGuildMemberList { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformMemberList = this.UI_MemberInfo_Prefab.transform.parent.GetComponent<RectTransform>();
			this.transformFamilyList = this.UI_FamilyInfo_Prefab.transform.parent.GetComponent<RectTransform>();
		}

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
			this.UIButton_KickFamily.onClick.AddListener(this.ButtonKickOutFamily_Clicked);
			this.UIButton_Previous.onClick.AddListener(this.ButtonPreviousPage_Clicked);
			this.UIButton_Next.onClick.AddListener(this.ButtonNextPage_Clicked);
			this.UIButton_KickFamily.gameObject.SetActive(false);
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button trục xuất tộc được ấn
		/// </summary>
		private void ButtonKickOutFamily_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu thành viên!");
				return;
			}
			/// Nếu không phải Bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master)
			{
				KTGlobal.AddNotification("Chỉ có bang chủ mới có quyền trục xuất gia tộc!");
				return;
			}
			/// Nếu chưa chọn tộc
			else if (this.selectedFamily == null)
			{
				KTGlobal.AddNotification("Hãy chọn một gia tộc!");
				return;
			}

			KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận trục xuất gia tộc <color=#ffb442>[{0}]</color> ra khỏi bang hội?", this.selectedFamily.FamilyName), () => {
				this.KickOutFamily?.Invoke(this.selectedFamily.FamilyID);
			}, true);
		}

		/// <summary>
		/// Sự kiện khi Button trang trước được ấn
		/// </summary>
		private void ButtonPreviousPage_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu thành viên!");
				return;
			}
			/// Nếu đang ở trang đầu tiên
			else if (this.Data.PageIndex <= 1)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildMemberList?.Invoke(this.Data.PageIndex - 1);

			/// Khóa Button
			this.UIButton_Previous.interactable = false;
		}

		/// <summary>
		/// Sự kiện khi Button trang tiếp theo được ấn
		/// </summary>
		private void ButtonNextPage_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu thành viên!");
				return;
			}
			/// Nếu đang ở trang cuối cùng
			else if (this.Data.PageIndex >= this.Data.TotalPage)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildMemberList?.Invoke(this.Data.PageIndex + 1);

			/// Khóa Button
			this.UIButton_Next.interactable = false;
		}
		#endregion

		#region Private methods
		/// <summary>
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
		/// <param name="rectTransform"></param>
		private void RebuildLayout(RectTransform rectTransform)
		{
			/// Nếu đối tượng không được kích hoạt
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			/// Thực hiện xây lại giao diện ở Frame tiếp theo
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}));
		}

		/// <summary>
		/// Xây lại giao diện danh sách thành viên
		/// </summary>
		private void RebuildMemberListLayout()
		{
			this.RebuildLayout(this.transformMemberList);
		}

		/// <summary>
		/// Xây lại giao diện danh sách gia tộc
		/// </summary>
		private void RebuildFamilyListLayout()
		{
			this.RebuildLayout(this.transformFamilyList);
		}

		/// <summary>
		/// Xóa rỗng danh sách thành viên
		/// </summary>
		private void ClearMemberList()
		{
			foreach (Transform child in this.transformMemberList.transform)
			{
				if (child.gameObject != this.UI_MemberInfo_Prefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Xóa rỗng danh sách tộc
		/// </summary>
		private void ClearFamilyList()
		{
			foreach (Transform child in this.transformFamilyList.transform)
			{
				if (child.gameObject != this.UI_FamilyInfo_Prefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Thêm thành viên vào danh sách
		/// </summary>
		/// <param name="data"></param>
		private void AddMember(GuildMember data)
		{
			UIGuildMemberList_MemberInfo uiMemberInfo = GameObject.Instantiate<UIGuildMemberList_MemberInfo>(this.UI_MemberInfo_Prefab);
			uiMemberInfo.transform.SetParent(this.transformMemberList, false);
			uiMemberInfo.gameObject.SetActive(true);
			uiMemberInfo.Data = data;
			uiMemberInfo.Select = () => {
				/// Nếu là chính mình thì thôi
				if (data.RoleID == Global.Data.RoleData.RoleID)
				{
					return;
				}

				/// Mở giao diện tương tác với thành viên
				this.UI_MemberInteractableFrame.EnableChangeRank = Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster;
				this.UI_MemberInteractableFrame.BrowseInfo = () => {
					this.BrowseInfo?.Invoke(data.RoleID);
					this.UI_MemberInteractableFrame.Hide();
				};
				this.UI_MemberInteractableFrame.PrivateChat = () => {
					this.PrivateChat?.Invoke(data.RoleName);
					this.UI_MemberInteractableFrame.Hide();
				};
				this.UI_MemberInteractableFrame.AddFriend = () => {
					this.AddFriend?.Invoke(data.RoleID);
					this.UI_MemberInteractableFrame.Hide();
				};
				this.UI_MemberInteractableFrame.ChangeRank = () => {
					Dictionary<int, string> rankDict = new Dictionary<int, string>();
					/// Nếu là bang chủ
					if (Global.Data.RoleData.GuildRank == (int) GuildRank.Master)
					{
						rankDict[(int) GuildRank.ViceMaster] = KTGlobal.GetGuildRankName((int) GuildRank.ViceMaster);
					}
					rankDict[(int) GuildRank.Ambassador] = KTGlobal.GetGuildRankName((int) GuildRank.Ambassador);
					rankDict[(int) GuildRank.Elite] = KTGlobal.GetGuildRankName((int) GuildRank.Elite);
					rankDict[(int) GuildRank.Member] = KTGlobal.GetGuildRankName((int) GuildRank.Member);
					this.UI_SetMemberRankFrame.Data = rankDict;
					this.UI_SetMemberRankFrame.OK = (rank) => {
						this.UI_SetMemberRankFrame.Hide();
						this.Approve?.Invoke(data.RoleID, rank);
					};
					this.UI_SetMemberRankFrame.Show();
					this.UI_MemberInteractableFrame.Hide();
				};
				this.UI_MemberInteractableFrame.Show();
			};
		}

		/// <summary>
		/// Thêm tộc vào danh sách
		/// </summary>
		/// <param name="data"></param>
		private void AddFamily(FamilyObj data)
		{
			UIGuildMemberList_FamilyInfo uiFamilyInfo = GameObject.Instantiate<UIGuildMemberList_FamilyInfo>(this.UI_FamilyInfo_Prefab);
			uiFamilyInfo.transform.SetParent(this.transformFamilyList, false);
			uiFamilyInfo.gameObject.SetActive(true);
			uiFamilyInfo.Data = data;
			uiFamilyInfo.Select = () => {
				this.selectedFamily = data;
				/// Mở Button chức năng
				this.UIButton_KickFamily.interactable = true;
			};
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		public void Refresh()
		{
			/// Xóa rỗng danh sách thành viên
			this.ClearMemberList();
			/// Xóa rỗng danh sách tộc
			this.ClearFamilyList();
			/// Hủy button chức năng
			this.UIButton_KickFamily.gameObject.SetActive(false);
			this.UIButton_KickFamily.interactable = false;
			/// Hủy thiết lập tổng số tộc
			this.UIText_FamilyCount.text = "0";
			/// Thiết lập số trang
			this.UIText_PageNumber.text = "0/0";
			/// Thiết lập Button phân trang
			this.UIButton_Previous.interactable = false;
			this.UIButton_Next.interactable = false;
			/// Hủy tộc đang chọn
			this.selectedFamily = null;

			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Nếu tồn tại danh sách thành viên
			if (this.Data.TotalGuildMember != null)
			{
				/// Duyệt danh sách thành viên
				foreach (GuildMember guildMember in this.Data.TotalGuildMember)
				{
					/// Thêm thành viên
					this.AddMember(guildMember);
				}
			}
			
			/// Nếu tồn tại danh sách tộc
			if (this.Data.TotalFamilyMemeber != null)
			{
				/// Duyệt danh sách tộc
				foreach (FamilyObj familyData in this.Data.TotalFamilyMemeber)
				{
					/// Thêm tộc
					this.AddFamily(familyData);
				}

				/// Chọn thành viên đầu tiên
				if (this.Data.TotalFamilyMemeber.Count > 0)
				{
					this.selectedFamily = this.Data.TotalFamilyMemeber.First();
					/// Thiết lập Button chức năng
					if (Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster)
					{
						this.UIButton_KickFamily.interactable = true;
					}
				}
				/// Thiết lập tổng số tộc
				this.UIText_FamilyCount.text = this.Data.TotalFamilyMemeber.Count.ToString();
			}

			/// Thiết lập số trang
			this.UIText_PageNumber.text = string.Format("{0}/{1}", this.Data.PageIndex, this.Data.TotalPage);
			/// Thiết lập Button phân trang
			if (this.Data.PageIndex > 1)
			{
				this.UIButton_Previous.interactable = true;
			}
			if (this.Data.PageIndex < this.Data.TotalPage)
			{
				this.UIButton_Next.interactable = true;
			}

			/// Thiết lập Button chức năng
			if (Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster)
			{
				this.UIButton_KickFamily.gameObject.SetActive(true);
			}

			/// Xây lại danh sách thành viên
			this.RebuildMemberListLayout();
			/// Xây lại danh sách tộc
			this.RebuildFamilyListLayout();
		}
		#endregion
	}
}
