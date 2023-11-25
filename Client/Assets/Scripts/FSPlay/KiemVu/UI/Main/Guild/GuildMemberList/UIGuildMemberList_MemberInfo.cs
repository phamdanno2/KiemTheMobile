using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildMemberList
{
	/// <summary>
	/// Thông tin thành viên trong khung danh sách thành viên bang hội
	/// </summary>
	public class UIGuildMemberList_MemberInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle;

		/// <summary>
		/// Text tên
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;

		/// <summary>
		/// Text cấp độ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Level;

		/// <summary>
		/// Text môn phái
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Faction;

		/// <summary>
		/// Text chức vụ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Rank;

		/// <summary>
		/// Text gia tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Family;

		/// <summary>
		/// Text trạng thái Online
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Status;
		#endregion

		#region Properties
		private GuildMember _Data;
		/// <summary>
		/// Thông tin thành viên
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
				this.UIText_Name.text = value.RoleName;
				this.UIText_Level.text = value.Level.ToString();
				this.UIText_Faction.text = KTGlobal.GetFactionName(value.FactionID, out Color factionColor);
				this.UIText_Faction.color = factionColor;
				this.UIText_Rank.text = KTGlobal.GetGuildRankName(value.Rank);
				this.UIText_Family.text = value.FamilyName;
				this.UIText_Status.text = value.OnlienStatus == 1 ? "<color=green>[ONL]</color>" : "<color=red>[OFF]</color>";
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
