using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildShare
{
	/// <summary>
	/// Khung cổ tức bang hội
	/// </summary>
	public class UIGuildShare : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Thông tin thành viên
		/// </summary>
		[SerializeField]
		private UIGuildShare_MemberInfo UI_MemberInfo_Prefab;

		/// <summary>
		/// Button mở khung quan hàm bang hội
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OpenGuildOfficialRank;

		/// <summary>
		/// Button trang trước
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Previous;

		/// <summary>
		/// Button trang sau
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Next;

		/// <summary>
		/// Text số trang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_PageNumber;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách thành viên
		/// </summary>
		private RectTransform transformMemberList = null;
		#endregion

		#region Properties
		/// <summary>
		/// Thông tin cổ tức bang hội
		/// </summary>
		public GuildShareInfo Data { get; set; }

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện mở khung thông tin quan hàm bang hội
		/// </summary>
		public Action OpenGuildOfficalRank { get; set; }

		/// <summary>
		/// Sự kiện truy vấn thông tin cổ tức ở trang tương ứng
		/// </summary>
		public Action<int> QueryGuildShare { get; set; }
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
			this.UIButton_OpenGuildOfficialRank.onClick.AddListener(this.ButtonOpenGuildOfficialRank_Clicked);
			this.UIButton_Previous.onClick.AddListener(this.ButtonPreviousPage_Clicked);
			this.UIButton_Next.onClick.AddListener(this.ButtonNextPage_Clicked);

			this.UIButton_OpenGuildOfficialRank.interactable = false;
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button mở khung quan hàm bang hội được ấn
		/// </summary>
		private void ButtonOpenGuildOfficialRank_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu cổ tức!");
				return;
			}

			/// Thực thi sự kiện
			this.OpenGuildOfficalRank?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button trang trước được ấn
		/// </summary>
		private void ButtonPreviousPage_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu cổ tức!");
				return;
			}
			/// Nếu đang ở trang đầu tiên
			else if (this.Data.PageIndex <= 1)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildShare?.Invoke(this.Data.PageIndex - 1);

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
				KTGlobal.AddNotification("Không có dữ liệu cổ tức!");
				return;
			}
			/// Nếu đang ở trang cuối cùng
			else if (this.Data.PageIndex >= this.Data.TotalPage)
			{
				return;
			}

			/// Truy vấn thông tin trang trước
			this.QueryGuildShare?.Invoke(this.Data.PageIndex + 1);

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
		private void AddMember(Server.Data.GuildShare data)
		{
			UIGuildShare_MemberInfo uiMemberInfo = GameObject.Instantiate<UIGuildShare_MemberInfo>(this.UI_MemberInfo_Prefab);
			uiMemberInfo.transform.SetParent(this.transformMemberList, false);
			uiMemberInfo.gameObject.SetActive(true);
			uiMemberInfo.Data = data;
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
			this.UIButton_OpenGuildOfficialRank.interactable = false;

			/// Nếu không tồn tại dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Nếu tồn tại danh sách thành viên
			if (this.Data.MemberList != null)
			{
				/// Duyệt danh sách thành viên
				foreach (Server.Data.GuildShare memberInfo in this.Data.MemberList)
				{
					/// Thêm thành viên
					this.AddMember(memberInfo);
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

			/// Mở Button chức năng
			this.UIButton_OpenGuildOfficialRank.interactable = true;

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion
	}
}
