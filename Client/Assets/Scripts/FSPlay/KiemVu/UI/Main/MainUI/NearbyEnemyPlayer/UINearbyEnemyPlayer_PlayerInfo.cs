using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.MainUI.NearbyEnemyPlayer
{
	/// <summary>
	/// Thông tin kẻ địch là người chơi
	/// </summary>
	public class UINearbyEnemyPlayer_PlayerInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Avarta người chơi
		/// </summary>
		[SerializeField]
		private UIRoleAvarta UI_Avarta;

		/// <summary>
		/// Tên người chơi
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;
		#endregion

		#region Properties
		private RoleData _Data;
		/// <summary>
		/// Dữ liệu người chơi
		/// </summary>
		public RoleData Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				/// Nếu có dữ liệu
				if (value != null)
				{
					/// Cập nhật Avarta
					this.UI_Avarta.RoleID = value.RoleID;
					this.UI_Avarta.AvartaID = value.RolePic;
					/// Cập nhật tên
					this.UIText_Name.text = value.RoleName;
					/// Cập nhật sự kiện Click
					this.UI_Avarta.Click = this.Click;
				}
			}
		}

		/// <summary>
		/// Sự kiện Click vào người chơi
		/// </summary>
		public Action Click { get; set; }
		#endregion
	}
}
