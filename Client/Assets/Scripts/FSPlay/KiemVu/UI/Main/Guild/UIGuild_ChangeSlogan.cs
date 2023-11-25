using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main.Guild
{
	public class UIGuild_ChangeSlogan : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Input nhập tôn chỉ
		/// </summary>
		[SerializeField]
		private TMP_InputField UIInput_Slogan;

		/// <summary>
		/// Button xác nhận
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_OK;
		#endregion

		#region Properties
		/// <summary>
		/// Tôn chỉ hiện tại
		/// </summary>
		public string CurrentSlogan
		{
			get
			{
				return this.UIInput_Slogan.text;
			}
			set
			{
				this.UIInput_Slogan.text = value;
			}
		}

		/// <summary>
		/// Sự kiện xác nhận
		/// </summary>
		public Action<string> OK { get; set; }
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
			this.UIButton_OK.onClick.AddListener(this.ButtonOK_Clicked);
		}

		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Hide();
		}

		/// <summary>
		/// Sự kiện xác nhận
		/// </summary>
		private void ButtonOK_Clicked()
		{
			/// Tôn chỉ
			string slogan = this.UIInput_Slogan.text;
			/// Chuẩn hóa
			slogan = Utils.BasicNormalizeString(slogan);

			/// Nếu rỗng
			if (string.IsNullOrEmpty(slogan))
			{
				KTGlobal.AddNotification("Chưa nhập tôn chỉ!");
				return;
			}

			/// Thực thi sự kiện
			this.OK?.Invoke(slogan);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Hiện khung
		/// </summary>
		public void Show()
		{
			this.gameObject.SetActive(true);
		}

		/// <summary>
		/// Ẩn khung
		/// </summary>
		public void Hide()
		{
			this.gameObject.SetActive(false);
		}
		#endregion
	}
}
