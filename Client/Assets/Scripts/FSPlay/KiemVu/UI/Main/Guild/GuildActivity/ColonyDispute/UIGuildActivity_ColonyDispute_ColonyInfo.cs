using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildActivity.ColonyDispute
{
	/// <summary>
	/// Thông tin lãnh thổ trong khung hoạt động tranh đoạt lãnh thổ
	/// </summary>
	public class UIGuildActivity_ColonyDispute_ColonyInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle;

		/// <summary>
		/// Text tên lãnh thổ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Name;

		/// <summary>
		/// Text thuế
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Tax;

		/// <summary>
		/// Text số sao
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Star;

		/// <summary>
		/// Text thành chính
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MainCastle;
		#endregion

		#region Properties
		private Territory _Data;
		/// <summary>
		/// Thông tin lãnh thổ
		/// </summary>
		public Territory Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.UIText_Name.text = value.MapName;
				this.UIText_Tax.text = string.Format("{0}%", value.Tax);
				this.UIText_Star.text = value.Star.ToString();
				this.UIText_MainCastle.text = value.IsMainCity == 1 ? "Có" : "Không";
			}
		}

		/// <summary>
		/// Sự kiện chọn lãnh thổ
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
