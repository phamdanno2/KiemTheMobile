using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildShare
{
	/// <summary>
	/// Thông tin thành viên trong khung cổ tức bang hội
	/// </summary>
	public class UIGuildShare_MemberInfo : MonoBehaviour
	{
		#region Define
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
		/// Text gia tộc
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Family;

		/// <summary>
		/// Text cổ phần
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Share;

		/// <summary>
		/// Text chức vị
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Rank;
		#endregion

		#region Properties
		private Server.Data.GuildShare _Data;
		/// <summary>
		/// Thông tin thành viên
		/// </summary>
		public Server.Data.GuildShare Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.UIText_Index.text = value.ID.ToString();
				this.UIText_Name.text = value.RoleName;
				this.UIText_Family.text = value.FamilyName;
				this.UIText_Share.text = string.Format("{0}%", Utils.Truncate(value.Share, 2));
				this.UIText_Rank.text = KTGlobal.GetGuildRankName(value.Rank);
			}
		}
		#endregion
	}
}
