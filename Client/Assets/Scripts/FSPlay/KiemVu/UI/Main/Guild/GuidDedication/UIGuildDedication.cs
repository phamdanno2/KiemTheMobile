using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main.Guild.GuidDedication
{
	/// <summary>
	/// Khung cống hiến bang hội
	/// </summary>
	public class UIGuildDedication : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Text số bạc của bang hội
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildMoney;

		/// <summary>
		/// Button nhập số lượng cống hiến vào bang
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_InputDedicatedMoney;

		/// <summary>
		/// Text số bạc cống hiến vào bang
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_DedicatedMoney;

		/// <summary>
		/// Button cống hiến
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Dedicate;
		#endregion

		#region Private fields
		/// <summary>
		/// Số bạc cống hiến vào bang
		/// </summary>
		private int dedicatedAmount = 0;
		#endregion

		#region Properties
		/// <summary>
		/// Số bạc của bang
		/// </summary>
		public int GuildMoney { get; set; }

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện cống hiến bạc
		/// </summary>
		public Action<int> Dedicate { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi ở Frame đàu tiên
		/// </summary>
		private void Start()
		{
			this.InitPrefabs();
			this.Refresh();
		}
		#endregion

		#region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
		{
			this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
			this.UIButton_InputDedicatedMoney.onClick.AddListener(this.ButtonInputDedicateMoney_Clicked);
			this.UIButton_Dedicate.onClick.AddListener(this.ButtonDedicate_Clicked);
			this.UIText_DedicatedMoney.text = "0";
			this.UIButton_Dedicate.interactable = false;
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện khi Button nhập số lượng cống hiến được ấn
		/// </summary>
		private void ButtonInputDedicateMoney_Clicked()
		{
			KTGlobal.ShowInputNumber("Nhập số bạc muốn cống hiến vào bang hội.", 1, Global.Data.RoleData.Money, (amount) => {
				this.dedicatedAmount = amount;
				this.UIText_DedicatedMoney.text = KTGlobal.GetDisplayMoney(amount);
				this.UIButton_Dedicate.interactable = true;
			});
		}

		/// <summary>
		/// Sự kiện khi Button cống hiến được ấn
		/// </summary>
		private void ButtonDedicate_Clicked()
		{
			/// Nếu chưa cống hiến
			if (this.dedicatedAmount <= 0)
			{
				KTGlobal.AddNotification("Chưa nhập số bạc muốn cống hiến!");
				return;
			}
			KTGlobal.ShowMessageBox("Cống hiến", string.Format("Xác nhận cống hiến {0} bạc vào Bang quỹ?", KTGlobal.GetDisplayMoney(this.dedicatedAmount)), () => {
				this.Dedicate?.Invoke(this.dedicatedAmount);
			}, true);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		public void Refresh()
		{
			this.UIText_GuildMoney.text = KTGlobal.GetDisplayMoney(this.GuildMoney);
			this.UIText_DedicatedMoney.text = "0";
			this.UIButton_Dedicate.interactable = false;
		}
		#endregion
	}
}
