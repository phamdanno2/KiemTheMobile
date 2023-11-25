using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Thông tin gia tộc trong khung danh sách thành viên
	/// </summary>
	public class UIGuildMemberList_FamilyInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle;

		/// <summary>
		/// Text tên tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;

		/// <summary>
		/// Text tổng số thành viên
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MemberCount;

		/// <summary>
		/// Text tổng uy danh
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_TotalPoints;
		#endregion

		#region Properties
		private FamilyObj _Data;
		/// <summary>
		/// Thông tin gia tộc
		/// </summary>
		public FamilyObj Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.UIText_Name.text = value.FamilyName;
				this.UIText_MemberCount.text = value.MemberCount.ToString();
				this.UIText_TotalPoints.text = KTGlobal.GetDisplayNumber(value.TotalpPrestige);
			}
		}

		/// <summary>
		/// Sự kiện chọn tộc
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
