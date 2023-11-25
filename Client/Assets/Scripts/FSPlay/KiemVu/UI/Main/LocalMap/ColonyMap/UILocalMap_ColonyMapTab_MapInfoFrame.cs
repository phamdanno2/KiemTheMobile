using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using Server.Data;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic;

namespace FSPlay.KiemVu.UI.Main.LocalMap.ColonyMap
{
	/// <summary>
	/// Khung thông tin lãnh thổ trong bản đồ lãnh thổ
	/// </summary>
	public class UILocalMap_ColonyMapTab_MapInfoFrame : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Button đóng khung
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Text tên bản đồ
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_MapName;

		/// <summary>
		/// Text tên bang chiếm hữu
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildOwnerName;

		/// <summary>
		/// Text loại
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Type;

		/// <summary>
		/// Text thuế
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_Tax;

		/// <summary>
		/// Text tên bản đồ tranh đoạt
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_FightMapName;

		/// <summary>
		/// Text tổng số lãnh thổ của bang chiếm đóng
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_GuildTotalColony;

		/// <summary>
		/// Prefab sao
		/// </summary>
		[SerializeField]
		private RectTransform UI_StarPrefabs;

		/// <summary>
		/// Button dịch đến
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_GoTo;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách sao
		/// </summary>
		private RectTransform transformStarBox = null;
		#endregion

		#region Properties
		/// <summary>
		/// ID bản đồ
		/// </summary>
		public int MapID { get; set; }

		private GuildWarMiniMap _Data;
		/// <summary>
		/// Thông tin lãnh thổ
		/// </summary>
		public GuildWarMiniMap Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		private int _GuildTotalColony;
		/// <summary>
		/// Tổng số lãnh thổ của bang chiếm đóng
		/// </summary>
		public int GuildTotalColony
		{
			get
			{
				return this._GuildTotalColony;
			}
			set
			{
				this._GuildTotalColony = value;
			}
		}
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformStarBox = this.UI_StarPrefabs.transform.parent.GetComponent<RectTransform>();
		}

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
			this.UIButton_GoTo.onClick.AddListener(this.ButtonGoTo_Clicked);
		}

		/// <summary>
		/// Sự kiện khi Button đóng khung được ấn
		/// </summary>
		private void ButtonClose_Clicked()
		{
			this.Hide();
		}

		/// <summary>
		/// Sự kiện khi Button dịch đến được ấn
		/// </summary>
		private void ButtonGoTo_Clicked()
        {
			/// Thông tin lãnh thổ
			if (!Loader.Loader.ColonyMaps.TryGetValue(this.MapID, out ColonyMapXML colonyMapData))
			{
				return;
			}
			/// Thông tin bản đồ tranh đoạt
			if (!Loader.Loader.Maps.TryGetValue(colonyMapData.MapFightID, out Entities.Config.Map fightMapData))
			{
				KTGlobal.AddNotification("Bản đồ này chưa mở tranh đoạt!");
				return;
			}

			/// Thực hiện tìm đường đến bản đồ tương ứng
			KTGlobal.QuestAutoFindPathToMap(colonyMapData.MapFightID, () => {
				AutoQuest.Instance.StopAutoQuest();
				AutoPathManager.Instance.StopAutoPath();
			});

			/// Đóng khung
			this.Hide();
			PlayZone.GlobalPlayZone.UILocalMap.Hide();
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
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformStarBox);
			}));
		}

		/// <summary>
		/// Làm rỗng danh sách sao
		/// </summary>
		private void ClearStarList()
		{
			foreach (Transform child in this.transformStarBox.transform)
			{
				if (child.gameObject != this.UI_StarPrefabs.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Thêm sao vào danh sách
		/// </summary>
		private void AddStar()
		{
			RectTransform uiStar = GameObject.Instantiate<RectTransform>(this.UI_StarPrefabs);
			uiStar.transform.SetParent(this.transformStarBox, false);
			uiStar.gameObject.SetActive(true);
		}

		/// <summary>
		/// Làm mới hiển thị
		/// </summary>
		private void Refresh()
		{
			/// Làm rỗng danh sách sao
			this.ClearStarList();

			/// Làm rỗng Text
			this.UIText_MapName.text = "";
			this.UIText_Tax.text = "";
			this.UIText_Type.text = "";
			this.UIText_GuildOwnerName.text = "";
			this.UIText_GuildTotalColony.text = "";
			this.UIText_FightMapName.text = "";

			/// Thông tin bản đồ
			if (!Loader.Loader.Maps.TryGetValue(this.MapID, out Entities.Config.Map mapData))
			{
				return;
			}
			/// Đổ dữ liệu
			this.UIText_MapName.text = mapData.Name;


			/// Thông tin lãnh thổ
			if (!Loader.Loader.ColonyMaps.TryGetValue(this.MapID, out ColonyMapXML colonyMapData))
			{
				return;
			}
			/// Duyệt danh sách sao
			for (int i = 1; i <= colonyMapData.Star; i++)
			{
				this.AddStar();
			}
			this.UIText_Type.text = colonyMapData.Type == "village" ? "Tân thủ thôn" : "Dã ngoại";

			/// Thông tin bản đồ tranh đoạt
			if (!Loader.Loader.Maps.TryGetValue(colonyMapData.MapFightID, out Entities.Config.Map fightMapData))
			{
				/// Đây là chưa mở
				if (colonyMapData.MapFightID == -1)
				{
					/// Đổ dữ liệu
					this.UIText_FightMapName.text = "Chưa mở";
				}
				return;
			}
			/// Đổ dữ liệu
			this.UIText_FightMapName.text = fightMapData.Name;

			/// Nếu không có dữ liệu
			if (this._Data == null)
			{
				return;
			}

			/// Đổ dữ liệu
			this.UIText_GuildOwnerName.text = this._Data.GuildName;
			this.UIText_Type.text = this._Data.MapType == 1 ? "Thành chính" : this._Data.MapType == 2 ? "Tân thủ thôn" : "Dã ngoại";
			this.UIText_Tax.text = string.Format("{0}%", this._Data.Tax);
			this.UIText_GuildTotalColony.text = this._GuildTotalColony.ToString();

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Hiện khung
		/// </summary>
		public void Show()
		{
			this.gameObject.SetActive(true);
			this.Refresh();
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
