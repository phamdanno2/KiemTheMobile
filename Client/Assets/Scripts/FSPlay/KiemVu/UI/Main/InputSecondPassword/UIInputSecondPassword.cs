using System;
using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main
{
	/// <summary>
	/// Khung nhập mật khẩu cấp 2
	/// </summary>
	public class UIInputSecondPassword : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Input nhập mật khẩu
		/// </summary>
		[SerializeField]
		private TMP_InputField UIInput_Password;

		/// <summary>
		/// Button xác nhận
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Accept;
		#endregion

		#region Properties
		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// Sự kiện xác nhận
		/// </summary>
		public Action<string> Accept { get; set; }
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
			this.UIButton_Accept.onClick.AddListener(this.ButtonAccept_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}

		/// <summary>
		/// Sự kiện xác nhận
		/// </summary>
		private void ButtonAccept_Clicked()
		{
			string password = this.UIInput_Password.text.Trim();
			/// Nếu không phải số
			if (!int.TryParse(password, out _))
			{
				KTGlobal.AddNotification("Mật khẩu cấp 2 chỉ bao gồm số!");
				return;
			}
			/// Nếu độ dài không thỏa mãn
			else if (password.Length != 6)
			{
				KTGlobal.AddNotification("Mật khẩu cấp 2 phải bao gồm 6 chữ số!");
				return;
			}

			/// Thực thi sự kiện
			this.Accept?.Invoke(password);
		}
		#endregion
	}
}
