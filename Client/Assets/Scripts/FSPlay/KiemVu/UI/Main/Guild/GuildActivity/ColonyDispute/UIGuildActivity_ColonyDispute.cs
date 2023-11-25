using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;
using FSPlay.GameEngine.Logic;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.Main.Guild.GuildActivity.ColonyDispute
{
	/// <summary>
	/// Khung hoạt động tranh đoạt lãnh thổ
	/// </summary>
	public class UIGuildActivity_ColonyDispute : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Text tổng số lãnh thổ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_TotalColony;

		/// <summary>
		/// Prefab thông tin lãnh thổ
		/// </summary>
		[SerializeField]
		private UIGuildActivity_ColonyDispute_ColonyInfo UI_ColonyInfo_Prefab;

		/// <summary>
		/// Button thiết lập thuế
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_SetTax;

		/// <summary>
		/// Button thiết lập thành chính
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_SetMainCastle;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách lãnh thổ
		/// </summary>
		private RectTransform transformColonyList = null;

		/// <summary>
		/// Lãnh thổ được chọn
		/// </summary>
		private Territory selectedColony = null;
		#endregion

		#region Properties
		/// <summary>
		/// Dữ liệu lãnh thổ
		/// </summary>
		public TerritoryInfo Data { get; set; }

		/// <summary>
		/// Truy vấn dữ liệu
		/// </summary>
		public Action QueryData { get; set; }

		/// <summary>
		/// Sự kiện thiết lập thuế
		/// </summary>
		public Action<int, int> SetTax { get; set; }

		/// <summary>
		/// Sự kiện thiết lập thành chính
		/// </summary>
		public Action<int> SetMainCastle { get; set; }

		/// <summary>
		/// Có đang hiển thị không
		/// </summary>
		public bool Visible
		{
			get
			{
				return this.gameObject.activeSelf;
			}
		}
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformColonyList = this.UI_ColonyInfo_Prefab.transform.parent.GetComponent<RectTransform>();
		}

		/// <summary>
		/// Hàm này gọi ở Frame đầu tiên
		/// </summary>
		private void Start()
		{
			this.InitPrefabs();
			/// Hiện bảng chờ tải
			KTGlobal.ShowLoadingFrame("Đang tải dữ liệu lãnh thổ...");
			/// Truy vấn dữ liệu
			this.QueryData?.Invoke();
		}
		#endregion

		#region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
		{
			this.UIButton_SetTax.onClick.AddListener(this.ButtonSetTax_Clicked);
			this.UIButton_SetMainCastle.onClick.AddListener(this.ButtonSetMainCastle_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button thiết lập thuế được ấn
		/// </summary>
		private void ButtonSetTax_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu lãnh thổ!");
				return;
			}
			/// Nếu không phải bang chủ hoặc phó bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master && Global.Data.RoleData.GuildRank != (int) GuildRank.ViceMaster)
			{
				KTGlobal.AddNotification("Chỉ có bang chủ hoặc phó bang chủ mới có quyền thao tác!");
				return;
			}
			/// Nếu không có lãnh thổ được chọn
			else if (this.selectedColony == null)
			{
				KTGlobal.AddNotification("Hãy chọn một lãnh thổ!");
				return;
			}

			/// Mở khung thiết lập thuế
			KTGlobal.ShowInputNumber(string.Format("Thiết lập <color=green><b>%</b></color> thuế lên <color=#42e3ff>[{0}]</color>.", this.selectedColony.MapName), 1, 5, (tax) => {
				this.SetTax?.Invoke(this.selectedColony.MapID, tax);
			});
		}

		/// <summary>
		/// Sự kiện khi Button thiết lập thành chính được ấn
		/// </summary>
		private void ButtonSetMainCastle_Clicked()
		{
			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				KTGlobal.AddNotification("Không có dữ liệu lãnh thổ!");
				return;
			}
			/// Nếu không phải bang chủ hoặc phó bang chủ
			else if (Global.Data.RoleData.GuildRank != (int) GuildRank.Master && Global.Data.RoleData.GuildRank != (int) GuildRank.ViceMaster)
			{
				KTGlobal.AddNotification("Chỉ có bang chủ hoặc phó bang chủ mới có quyền thao tác!");
				return;
			}
			/// Nếu không có lãnh thổ được chọn
			else if (this.selectedColony == null)
			{
				KTGlobal.AddNotification("Hãy chọn một lãnh thổ!");
				return;
			}

			/// Xác nhận thiết lập thành chính
			KTGlobal.ShowMessageBox("Xác nhận", string.Format("Thiết lập <color=#42e3ff>[{0}]</color> làm thành chính cần tiêu hao <color=green>200 vạn Bạc</color> quỹ bang, xác nhận không?", this.selectedColony.MapName), () => {
				this.SetMainCastle?.Invoke(this.selectedColony.MapID);
			}, true);
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Thực thi sự kiện bỏ qua một số Frame
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="work"></param>
		/// <returns></returns>
		private IEnumerator ExecuteSkipFrames(int skip, Action work)
		{
			for (int i = 1; i <= skip; i++)
			{
				yield return null;
			}
			work?.Invoke();
		}

		/// <summary>
		/// Xây lại giao diện
		/// </summary>
		private void RebuildLayout()
		{
			/// Nếu đối tượng không được kích hoạt
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			/// Thực hiện xây lại giao diện
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformColonyList);
			}));
		}

		/// <summary>
		/// Làm rỗng danh sách lãnh thổ
		/// </summary>
		private void ClearColonyList()
		{
			foreach (Transform child in this.transformColonyList.transform)
			{
				if (child.gameObject != this.UI_ColonyInfo_Prefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Thêm lãnh thổ tương ứng
		/// </summary>
		/// <param name="data"></param>
		private void AddColony(Territory data)
		{
			UIGuildActivity_ColonyDispute_ColonyInfo uiColonyInfo = GameObject.Instantiate<UIGuildActivity_ColonyDispute_ColonyInfo>(this.UI_ColonyInfo_Prefab);
			uiColonyInfo.transform.SetParent(this.transformColonyList, false);
			uiColonyInfo.gameObject.SetActive(true);
			uiColonyInfo.Data = data;
			uiColonyInfo.Select = () => {
				/// Đánh dấu lãnh thổ được chọn
				this.selectedColony = data;

				/// Mở tương tác các Button chức năng
				this.UIButton_SetTax.interactable = true;
				this.UIButton_SetMainCastle.interactable = data.IsMainCity != 1;
			};
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		public void Refresh()
		{
			/// Hủy bảng chờ tải
			KTGlobal.HideLoadingFrame();

			/// Làm rỗng danh sách lãnh thổ
			this.ClearColonyList();

			/// Hủy Text
			this.UIText_TotalColony.text = "0";
			/// Ẩn Button chức năng
			this.UIButton_SetTax.gameObject.SetActive(false);
			this.UIButton_SetTax.interactable = false;
			this.UIButton_SetMainCastle.gameObject.SetActive(false);
			this.UIButton_SetMainCastle.interactable = false;

			/// Nếu không có dữ liệu
			if (this.Data == null)
			{
				return;
			}

			/// Tổng số lãnh thổ
			this.UIText_TotalColony.text = Math.Max(0, this.Data.TerritoryCount).ToString();

			/// Nếu tồn tại danh sách lãnh thổ
			if (this.Data.TerritoryCount > 0)
			{
				/// Duyệt danh sách lãnh thổ
				foreach (Territory colonyInfo in this.Data.Territorys)
				{
					/// Thêm lãnh thổ
					this.AddColony(colonyInfo);
				}

				/// Chọn lãnh thổ đầu tiên
				this.selectedColony = this.Data.Territorys.FirstOrDefault();

				/// Mở tương tác các Button chức năng
				this.UIButton_SetTax.interactable = true;
				this.UIButton_SetMainCastle.interactable = this.selectedColony.IsMainCity != 1;
			}

			/// Nếu là bang chủ hoặc phó bang chủ
			if (Global.Data.RoleData.GuildRank == (int) GuildRank.Master || Global.Data.RoleData.GuildRank == (int) GuildRank.ViceMaster)
			{
				this.UIButton_SetTax.gameObject.SetActive(true);
				this.UIButton_SetMainCastle.gameObject.SetActive(true);
			}

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion
	}
}
