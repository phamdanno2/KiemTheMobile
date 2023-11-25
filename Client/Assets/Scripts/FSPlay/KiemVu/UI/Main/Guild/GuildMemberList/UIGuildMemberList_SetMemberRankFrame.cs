using FSPlay.KiemVu.UI.Main.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Khung bổ nhiệm thành viên
	/// </summary>
	public class UIGuildMemberList_SetMemberRankFrame : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Prefab thông tin chức vị
		/// </summary>
		[SerializeField]
		private UIGuildMemberList_SetMemberRank_RankInfo UI_RankInfo_Prefab;

		/// <summary>
		/// Button OK
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OK;
		#endregion

		#region Private fields
		/// <summary>
		/// Chức vị được chọn
		/// </summary>
		private int selectedRank = -1;

		/// <summary>
		/// RectTransform danh sách chức vị
		/// </summary>
		private RectTransform transformRankList = null;
		#endregion

		#region Properties
		/// <summary>
		/// Danh sách chức vị
		/// </summary>
		public Dictionary<int, string> Data { get; set; }

		/// <summary>
		/// Sự kiện đồng ý
		/// </summary>
		public Action<int> OK { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformRankList = this.UI_RankInfo_Prefab.transform.parent.GetComponent<RectTransform>();
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
			this.UIButton_OK.onClick.AddListener(this.ButtonOK_Clicked);
			this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
			/// Hủy tương tác với Button OK
			this.UIButton_OK.interactable = false;
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Hide();
		}

		/// <summary>
		/// Sự kiện khi Button OK được ấn
		/// </summary>
		private void ButtonOK_Clicked()
		{
			/// Nếu chưa chọn chức vị
			if (this.selectedRank == -1)
			{
				KTGlobal.AddNotification("Hãy chọn một chức vị!");
				return;
			}
			/// Thực thi sự kiện
			this.OK?.Invoke(this.selectedRank);
		}

		/// <summary>
		/// Sự kiện khi Toggle chức vị được chọn
		/// </summary>
		/// <param name="rankID"></param>
		private void ToggleRank_Selected(int rankID)
		{
			this.selectedRank = rankID;
			/// Mở tương tác với Button OK
			this.UIButton_OK.interactable = true;
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
			/// Thực hiện xây lại giao diện ở Frame tiếp theo
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformRankList);
			}));
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

			/// Duyệt danh sách chức vị
			foreach (KeyValuePair<int, string> pair in this.Data)
			{
				/// Thêm chức vị tương ứng
				UIGuildMemberList_SetMemberRank_RankInfo uiRankInfo = GameObject.Instantiate<UIGuildMemberList_SetMemberRank_RankInfo>(this.UI_RankInfo_Prefab);
				uiRankInfo.transform.SetParent(this.transformRankList, false);
				uiRankInfo.gameObject.SetActive(true);

				uiRankInfo.Name = pair.Value;
				uiRankInfo.Select = () => {
					this.ToggleRank_Selected(pair.Key);
				};
			}

			/// Chọn chức vị ban đầu
			if (this.Data.Count > 0)
			{
				this.ToggleRank_Selected(this.Data.First().Key);
			}

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Hiện khung
		/// </summary>
		public void Show()
		{
			this.gameObject.SetActive(true);
		}

		/// <summary>
		/// Ẩn khung
		/// </summary>
		public void Hide()
		{
			this.gameObject.SetActive(false);
		}
		#endregion
	}
}
