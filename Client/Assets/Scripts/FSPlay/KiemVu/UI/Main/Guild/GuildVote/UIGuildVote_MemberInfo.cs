using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildVote
{
	/// <summary>
	/// Thông tin thành viên trong khung bầu ưu tú bang hội
	/// </summary>
	public class UIGuildVote_MemberInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle;

		/// <summary>
		/// Text thứ tự
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Index;

		/// <summary>
		/// Text tên
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;

		/// <summary>
		/// Text số phiếu
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_VoteCount;

		/// <summary>
		/// Text gia tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Family;
		#endregion

		#region Properties
		private GuildMember _Data;
		/// <summary>
		/// Dữ liệu ưu tú
		/// </summary>
		public GuildMember Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.UIText_Index.text = value.VoteRank.ToString();
				this.UIText_Name.text = value.RoleName;
				this.UIText_VoteCount.text = value.TotalVote.ToString();
				this.UIText_Family.text = value.FamilyName;
			}
		}

		/// <summary>
		/// Sự kiện chọn thành viên
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
		/// <param name="isSelecte"></param>
		private void Toggle_Selected(bool isSelecte)
		{
			if (isSelecte)
			{
				this.Select?.Invoke();
			}
		}
		#endregion
	}
}
