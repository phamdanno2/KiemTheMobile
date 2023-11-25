using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildOfficialRank
{
	/// <summary>
	/// Thông tin thành viên trong khung quan hàm bang hội
	/// </summary>
	public class UIGuildOfficialRank_MemberInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Text tên thành viên
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;

		/// <summary>
		/// Text chức quan
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Rank;

		/// <summary>
		/// Text quan hàm
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_OfficialRank;
		#endregion

		#region Properties
		private OfficeRankMember _Data;
		/// <summary>
		/// Thông tin quan hàm thành viên
		/// </summary>
		public OfficeRankMember Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.UIText_Name.text = value.RoleName;
				this.UIText_Rank.text = value.RankTile;
				this.UIText_OfficialRank.text = value.OfficeRankTitle;
			}
		}
		#endregion
	}
}
