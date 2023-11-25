using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.Main.Guild.GuildActivity.ColonyDispute;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildActivity
{
	/// <summary>
	/// Khung hoạt động bang hội
	/// </summary>
	public class UIGuildActivity : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Khung tranh đoạt lãnh thổ
		/// </summary>
		[SerializeField]
		private UIGuildActivity_ColonyDispute UI_ColonyDispute;
		#endregion

		#region Properties
		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Khung tranh đoạt lãnh thổ
		/// </summary>
		public UIGuildActivity_ColonyDispute UIColonyDispute
		{
			get
			{
				return this.UI_ColonyDispute;
			}
		}
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
	}
}
