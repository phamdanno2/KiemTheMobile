using System;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.LocalMap.ColonyMap
{
	/// <summary>
	/// Thông tin bản đồ trong bản đồ lãnh thổ
	/// </summary>
	public class UILocalMap_ColonyMapTab_NodeInfo : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton;

		/// <summary>
		/// Icon lãnh thổ
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Image UI_ColonyIcon;

		/// <summary>
		/// Icon thành chính
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Image UI_MainCityIcon;

		/// <summary>
		/// Icon tân thủ thôn
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Image UI_VillageIcon;

		/// <summary>
		/// Icon tấn công
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Image UI_AttackIcon;

		/// <summary>
		/// Icon lân cận
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Image UI_NearbyIcon;

		/// <summary>
		/// Hiệu ứng đang diễn ra tranh đoạt
		/// </summary>
		[SerializeField]
		private RectTransform UI_DisputeAnimation;
		#endregion

		#region Properties
		private Color _Color;
		/// <summary>
		/// Màu nút
		/// </summary>
		public Color Color
		{
			get
			{
				return this._Color;
			}
			set
			{
				this._Color = value;
				this.UI_ColonyIcon.color = value;
				this.UI_MainCityIcon.color = value;
				this.UI_VillageIcon.color = value;
				this.UI_AttackIcon.color = value;
			}
		}

		/// <summary>
		/// ID bản đồ
		/// </summary>
		public int MapID { get; set; }

		private int _MapType;
		/// <summary>
		/// Loại bản đồ
		/// </summary>
		public int MapType
        {
            get
            {
				return this._MapType;
            }
            set
            {
				/// Ẩn hết
				this.UI_VillageIcon.gameObject.SetActive(false);
				this.UI_MainCityIcon.gameObject.SetActive(false);
				this.UI_AttackIcon.gameObject.SetActive(false);
				this.UI_ColonyIcon.gameObject.SetActive(false);
				this.UI_NearbyIcon.gameObject.SetActive(false);
				this.UI_DisputeAnimation.gameObject.SetActive(false);

				/// Lãnh thổ
				if (value == 0)
                {
					this.UI_ColonyIcon.gameObject.SetActive(true);
				}
				/// Thành chính
				else if (value == 1)
                {
					this.UI_MainCityIcon.gameObject.SetActive(true);
				}
				/// Tân thủ thôn
				else if (value == 2)
                {
					this.UI_VillageIcon.gameObject.SetActive(true);
				}
				/// Tấn công (nhấp nháy)
				else if (value == 3)
                {
					this.UI_AttackIcon.gameObject.SetActive(true);
					this.UI_DisputeAnimation.gameObject.SetActive(true);
				}
				/// Lân cận (nhấp nháy)
				else if (value == 4)
                {
					this.UI_NearbyIcon.gameObject.SetActive(true);
					this.UI_DisputeAnimation.gameObject.SetActive(true);
				}
				/// Lân cận
				else if (value == 5)
                {
					this.UI_NearbyIcon.gameObject.SetActive(true);
				}
            }
        }

		/// <summary>
		/// Sự kiện Click
		/// </summary>
		public Action Click { get; set; }
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
			this.UIButton.onClick.AddListener(this.Button_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button được ấn
		/// </summary>
		private void Button_Clicked()
		{
			this.Click?.Invoke();
		}
		#endregion
	}
}
