using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildOfficialRank
{
	/// <summary>
	/// Khung thông tin quan hàm bang hội
	/// </summary>
	public class UIGuildOfficialRank : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Text cấp độ bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildLevel;

		/// <summary>
		/// Prefab thông tin thành viên
		/// </summary>
		[SerializeField]
		private UIGuildOfficialRank_MemberInfo UI_MemberInfo_Prefab;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách thành viên
		/// </summary>
		private RectTransform transformMemberList = null;
		#endregion

		#region Properties
		/// <summary>
		/// Dữ liệu
		/// </summary>
		public OfficeRankInfo Data { get; set; }

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }
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
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
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
		/// Thêm thành viên vào danh sách
		/// </summary>
		/// <param name="data"></param>
		private void AddMember(OfficeRankMember data)
		{
			UIGuildOfficialRank_MemberInfo uiMemberInfo = GameObject.Instantiate<UIGuildOfficialRank_MemberInfo>(this.UI_MemberInfo_Prefab);
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

			/// Thiết lập lại Text
			this.UIText_GuildLevel.text = "Cấp 0";

			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Nếu tồn tại danh sách quan hàm
			if (this.Data.OffcieRankMember != null)
			{
				/// Duyệt danh sách
				foreach (OfficeRankMember memberInfo in this.Data.OffcieRankMember)
				{
					/// Thêm thành viên
					this.AddMember(memberInfo);
				}
			}

			/// Thiết lập cấp bang
			this.UIText_GuildLevel.text = string.Format("Cấp {0}", Math.Max(0, this.Data.GuildRank));

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion
	}
}
