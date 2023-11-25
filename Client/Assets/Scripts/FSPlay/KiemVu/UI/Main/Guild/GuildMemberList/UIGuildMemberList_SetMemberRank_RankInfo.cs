using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Thông tin chức vị trong khung danh sách thành viên
	/// </summary>
	public class UIGuildMemberList_SetMemberRank_RankInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle;

		/// <summary>
		/// Tên chức vị
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_RankName;
		#endregion

		#region Properties
		/// <summary>
		/// Tên chức vị
		/// </summary>
		public string Name
		{
			get
			{
				return this.UIText_RankName.text;
			}
			set
			{
				this.UIText_RankName.text = value;
			}
		}

		/// <summary>
		/// Sự kiện chọn chức vị
		/// </summary>
		public Action Select { get; set; }
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
			this.UIToggle.onValueChanged.AddListener(this.Toggle_Selected);
		}

		/// <summary>
		/// Sự kiện khi Toggle được chọn
		/// </summary>
		/// <param name="isSelected"></param>
		private void Toggle_Selected(bool isSelected)
		{
			if (isSelected)
			{
				this.Select?.Invoke();
			}
		}
		#endregion
	}
}
