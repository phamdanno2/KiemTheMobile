using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Khung tương tác thành viên trong khung danh sách thành viên bang hội
	/// </summary>
	public class UIGuildMemberList_MemberInteractableFrame : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button xem thông tin
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_BrowseInfo;

		/// <summary>
		/// Button chat mật
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_PrivateChat;

		/// <summary>
		/// Button thêm bạn
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_AddFriend;

		/// <summary>
		/// Button đổi chức vụ
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_ChangeRank;
		#endregion

		#region Properties
		/// <summary>
		/// Cho phép đổi chức vụ
		/// </summary>
		public bool EnableChangeRank
		{
			get
			{
				return this.UIButton_ChangeRank.interactable;
			}
			set
			{
				this.UIButton_ChangeRank.interactable = value;
			}
		}

		/// <summary>
		/// Sự kiện xem thông tin
		/// </summary>
		public Action BrowseInfo { get; set; }

		/// <summary>
		/// Sự kiện Chat mật
		/// </summary>
		public Action PrivateChat { get; set; }

		/// <summary>
		/// Sự kiện thêm bạn
		/// </summary>
		public Action AddFriend { get; set; }

		/// <summary>
		/// Sự kiện đổi chức vị
		/// </summary>
		public Action ChangeRank { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Khởi tạo ban đầu
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
			this.UIButton_BrowseInfo.onClick.AddListener(this.ButtonBrowseInfo_Clicked);
			this.UIButton_PrivateChat.onClick.AddListener(this.ButtonPrivateChat_Clicked);
			this.UIButton_AddFriend.onClick.AddListener(this.ButtonAddFriend_Clicked);
			this.UIButton_ChangeRank.onClick.AddListener(this.ButtonChangeRank_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button xem thông tin được ấn
		/// </summary>
		private void ButtonBrowseInfo_Clicked()
		{
			this.BrowseInfo?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button chat mật được ấn
		/// </summary>
		private void ButtonPrivateChat_Clicked()
		{
			this.PrivateChat?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button thêm bạn được ấn
		/// </summary>
		private void ButtonAddFriend_Clicked()
		{
			this.AddFriend?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button thay đổi chức vụ được ấn
		/// </summary>
		private void ButtonChangeRank_Clicked()
		{
			this.ChangeRank?.Invoke();
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
