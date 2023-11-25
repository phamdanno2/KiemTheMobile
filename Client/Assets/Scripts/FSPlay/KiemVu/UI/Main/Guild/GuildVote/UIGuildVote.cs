using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildVote
{
	/// <summary>
	/// Khung bầu ưu tú bang hội
	/// </summary>
	public class UIGuildVote : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Text số tuần
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_WeekNumber;

		/// <summary>
		/// Text khoảng thời gian tính
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_DateTime;

		/// <summary>
		/// Text tên thành viên bản thân đã bầu cho
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_VotedFor;

		/// <summary>
		/// Text thứ hạng
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MemberIndex;

		/// <summary>
		/// Text tên thành viên
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MemberName;

		/// <summary>
		/// Text cấp độ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MemberLevel;

		/// <summary>
		/// Text môn phái
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MemberFaction;

		/// <summary>
		/// Text tổng số phiếu
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_VoteCount;

		/// <summary>
		/// Prefab thông tin thành viên
		/// </summary>
		[SerializeField]
		private UIGuildVote_MemberInfo UI_MemberInfo_Prefab;

		/// <summary>
		/// Button bầu
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Vote;

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
		/// RectTransform danh sách thành viên
		/// </summary>
		private RectTransform transformMemberList = null;

		/// <summary>
		/// Thành viên được chọn
		/// </summary>
		private GuildMember selectedMember = null;
		#endregion

		#region Properties
		/// <summary>
		/// Thông tin bầu ưu tú
		/// </summary>
		public GuildVoteInfo Data { get; set; }

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện bầu cho thành viên
		/// </summary>
		public Action<int> Vote { get; set; }

		/// <summary>
		/// Truy vấn thông tin ưu tú thành viên theo trang
		/// </summary>
		public Action<int> QueryGuildVote { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformMemberList = this.UI_MemberInfo_Prefab.transform.parent.GetComponent<RectTransform>();
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
			this.UIButton_Vote.onClick.AddListener(this.ButtonVote_Clicked);
			this.UIButton_Previous.onClick.AddListener(this.ButtonPreviousPage_Clicked);
			this.UIButton_Next.onClick.AddListener(this.ButtonNextPage_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button bầu được ấn
		/// </summary>
		private void ButtonVote_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu ưu tú bang hội.");
				return;
			}
			/// Nếu đã bầu rồi
			else if (!string.IsNullOrEmpty(this.Data.VoteFor))
			{
				KTGlobal.AddNotification("Bạn đã bầu cho người khác.");
				return;
			}
			/// Nếu chưa chọn thành viên
			else if (this.selectedMember == null)
			{
				KTGlobal.AddNotification("Hãy chọn một thành viên sau đó ấn nút Bỏ phiếu.");
				return;
			}

			/// Thực thi sự kiện
			this.Vote?.Invoke(this.selectedMember.RoleID);
		}

		/// <summary>
		/// Sự kiện khi Button trang trước được ấn
		/// </summary>
		private void ButtonPreviousPage_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu ưu tú bang hội.");
				return;
			}
			/// Nếu đang ở trang đầu tiên
			else if (this.Data.PageIndex <= 1)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildVote?.Invoke(this.Data.PageIndex - 1);

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
				KTGlobal.AddNotification("Không có dữ liệu ưu tú bang hội.");
				return;
			}
			/// Nếu đang ở trang cuối cùng
			else if (this.Data.PageIndex >= this.Data.TotalPage)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildVote?.Invoke(this.Data.PageIndex + 1);

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
		private void RebuildLayout()
		{
			/// Nếu đối tượng không được kích hoạt
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			/// Thực hiện xây lại giao diện
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformMemberList);
			}));
		}

		/// <summary>
		/// Xóa danh sách thành viên
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
		/// Thêm thành viên tương ứng
		/// </summary>
		/// <param name="data"></param>
		private void AddMember(GuildMember data)
		{
			UIGuildVote_MemberInfo uiMemberInfo = GameObject.Instantiate<UIGuildVote_MemberInfo>(this.UI_MemberInfo_Prefab);
			uiMemberInfo.transform.SetParent(this.transformMemberList, false);
			uiMemberInfo.gameObject.SetActive(true);
			uiMemberInfo.Data = data;
			uiMemberInfo.Select = () => {
				/// Đánh dấu thằng được chọn
				this.selectedMember = data;

				/// Thiết lập Text
				this.UIText_MemberIndex.text = data.VoteRank.ToString();
				this.UIText_MemberName.text = data.RoleName;
				this.UIText_MemberLevel.text = data.Level.ToString();
				this.UIText_MemberFaction.text = KTGlobal.GetFactionName(data.FactionID, out Color factionColor);
				this.UIText_MemberFaction.color = factionColor;
				this.UIText_VoteCount.text = data.TotalVote.ToString();

				/// Nếu chưa bỏ phiếu cho ai
				if (string.IsNullOrEmpty(this.Data.VoteFor))
				{
					this.UIButton_Vote.interactable = true;
				}
			};
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		public void Refresh()
		{
			/// Xóa danh sách thành viên
			this.ClearMemberList();
			/// Thiết lập số trang
			this.UIText_PageNumber.text = "0/0";
			/// Thiết lập Button phân trang
			this.UIButton_Previous.interactable = false;
			this.UIButton_Next.interactable = false;
			/// Khóa Button chức năng
			this.UIButton_Vote.interactable = false;
			/// Xóa thành viên được chọn
			this.selectedMember = null;
			/// Hủy thông tin Text
			this.UIText_WeekNumber.text = "0";
			this.UIText_DateTime.text = "";
			this.UIText_VotedFor.text = "";
			this.UIText_MemberIndex.text = "";
			this.UIText_MemberName.text = "";
			this.UIText_MemberLevel.text = "";
			this.UIText_MemberFaction.text = "";
			this.UIText_VoteCount.text = "";

			/// Nếu không tồn tại dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Thiết lập Text
			this.UIText_WeekNumber.text = this.Data.WEEID.ToString();
			this.UIText_DateTime.text = string.Format("{0} - {1}", this.Data.Start, this.Data.End);
			if (!string.IsNullOrEmpty(this.Data.VoteFor))
			{
				this.UIText_VotedFor.text = this.Data.VoteFor;
			}

			/// Nếu tồn tại danh sách thành viên
			if (this.Data.GuildMember != null)
			{
				/// Duyệt danh sách thành viên
				foreach (GuildMember memberInfo in this.Data.GuildMember)
				{
					/// Thêm thành viên
					this.AddMember(memberInfo);
				}

				/// Nếu danh sách có trên 1 thành viên
				if (this.Data.GuildMember.Count >= 1)
				{
					/// Chọn thành viên đầu tiên
					this.selectedMember = this.Data.GuildMember.FirstOrDefault();

					/// Mở Button chức năng
					this.UIButton_Vote.interactable = string.IsNullOrEmpty(this.Data.VoteFor);

					/// Thiết lập Text
					this.UIText_MemberIndex.text = this.selectedMember.VoteRank.ToString();
					this.UIText_MemberName.text = this.selectedMember.RoleName;
					this.UIText_MemberLevel.text = this.selectedMember.Level.ToString();
					this.UIText_MemberFaction.text = KTGlobal.GetFactionName(this.selectedMember.FactionID, out Color factionColor);
					this.UIText_MemberFaction.color = factionColor;
					this.UIText_VoteCount.text = this.selectedMember.TotalVote.ToString();
				}
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

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion
	}
}
