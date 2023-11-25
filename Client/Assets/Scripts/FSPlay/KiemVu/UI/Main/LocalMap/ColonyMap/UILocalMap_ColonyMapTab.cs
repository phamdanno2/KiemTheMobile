using FSPlay.KiemVu.Entities.Config;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.LocalMap.ColonyMap
{
	/// <summary>
	/// Tab bản đồ lãnh thổ
	/// </summary>
	public class UILocalMap_ColonyMapTab : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Prefab bản đồ lãnh thổ
		/// </summary>
		[SerializeField]
		private UILocalMap_ColonyMapTab_NodeInfo UI_NodeInfoPrefab;

		/// <summary>
		/// Khung thông tin bản đồ lãnh thổ
		/// </summary>
		[SerializeField]
		private UILocalMap_ColonyMapTab_MapInfoFrame UI_MapInfoFrame;

		/// <summary>
		/// Prefab thông tin bang hội chiếm hữu
		/// </summary>
		[SerializeField]
		private UILocalMap_ColonyMapTab_GuildInfoNode UI_GuildInfoPrefab;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách nút
		/// </summary>
		private RectTransform transformNodeList = null;

		/// <summary>
		/// RectTransform danh sách bang hội chiếm hữu
		/// </summary>
		private RectTransform transformGuildInfoList = null;
		#endregion

		#region Properties
		private List<GuildWarMiniMap> _Data;
		/// <summary>
		/// Danh sách thông tin lãnh thổ
		/// </summary>
		public List<GuildWarMiniMap> Data
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

		/// <summary>
		/// Sự kiện truy vấn thông tin danh sách lãnh thổ
		/// </summary>
		public Action QueryTerritoryList { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformNodeList = this.UI_NodeInfoPrefab.transform.parent.GetComponent<RectTransform>();
			this.transformGuildInfoList = this.UI_GuildInfoPrefab.transform.parent.GetComponent<RectTransform>();
		}

		/// <summary>
		/// Hàm này gọi ở Frame đầu tiên
		/// </summary>
		private void Start()
		{
			this.InitPrefabs();
			this.BuildColonyMap();
		}

		/// <summary>
		/// Hàm này gọi khi đối tượng được kích hoạt
		/// </summary>
        private void OnEnable()
        {
			this.QueryTerritoryList?.Invoke();
		}
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
		{
			
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
		/// Xây lại giao diện danh sách bang hội chiếm hữu
		/// </summary>
		private void RebuildGuildInfoLayout()
        {
			if (!this.gameObject.activeSelf)
            {
				return;
            }
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformGuildInfoList);
			}));
        }

		/// <summary>
		/// Làm rỗng danh sách bang hội chiếm hữu
		/// </summary>
		private void ClearGuildInfoList()
        {
			foreach (Transform child in this.transformGuildInfoList.transform)
			{
				if (child.gameObject != this.UI_GuildInfoPrefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Làm rỗng danh sách
		/// </summary>
		private void ClearMapList()
		{
			foreach (Transform child in this.transformNodeList.transform)
			{
				if (child.gameObject != this.UI_NodeInfoPrefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Xây danh sách bang hội chiếm hữu
		/// </summary>
		private void BuildGuildInfoList()
        {
			/// Danh sách các bang hội
			List<GuildWarMiniMap> guilds = this._Data.GroupBy(x => x.GuildID).Select(x => x.FirstOrDefault()).ToList();
			/// Duyệt danh sách bang
			foreach (GuildWarMiniMap guildInfo in guilds)
            {
				this.AddGuild(guildInfo);
            }

			/// Xây lại giao diện
			this.RebuildGuildInfoLayout();
        }

		/// <summary>
		/// Thêm bang hội tương ứng vào danh sách bang chiếm hữu
		/// </summary>
		/// <param name="guildInfo"></param>
		private void AddGuild(GuildWarMiniMap guildInfo)
        {
			UILocalMap_ColonyMapTab_GuildInfoNode uiNodeInfo = GameObject.Instantiate<UILocalMap_ColonyMapTab_GuildInfoNode>(this.UI_GuildInfoPrefab);
			uiNodeInfo.transform.SetParent(this.transformGuildInfoList, false);
			uiNodeInfo.gameObject.SetActive(true);
			ColorUtility.TryParseHtmlString(guildInfo.HexColor, out Color color);
			uiNodeInfo.Color = color;
			uiNodeInfo.GuildName = guildInfo.GuildName;
		}

		/// <summary>
		/// Thêm bản đồ lãnh thổ
		/// </summary>
		/// <param name="colonyMapData"></param>
		private void AddMap(ColonyMapXML colonyMapData)
		{
			UILocalMap_ColonyMapTab_NodeInfo uiNodeInfo = GameObject.Instantiate<UILocalMap_ColonyMapTab_NodeInfo>(this.UI_NodeInfoPrefab);
			uiNodeInfo.transform.SetParent(this.transformNodeList, false);
			uiNodeInfo.gameObject.SetActive(true);
			uiNodeInfo.MapID = colonyMapData.MapID;
			uiNodeInfo.Click = () => {
				this.UI_MapInfoFrame.MapID = colonyMapData.MapID;
				this.UI_MapInfoFrame.Data = null;
				this.UI_MapInfoFrame.GuildTotalColony = 0;
				this.UI_MapInfoFrame.Show();
			};
			uiNodeInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(colonyMapData.PosX, colonyMapData.PosY);
		}

		/// <summary>
		/// Tìm nút lãnh thổ có ID bản đồ tương ứng
		/// </summary>
		/// <param name="mapID"></param>
		/// <returns></returns>
		private UILocalMap_ColonyMapTab_NodeInfo FindNode(int mapID)
		{
			foreach (Transform child in this.transformNodeList.transform)
			{
				if (child.gameObject != this.UI_NodeInfoPrefab.gameObject)
				{
					UILocalMap_ColonyMapTab_NodeInfo uiNodeInfo = child.GetComponent<UILocalMap_ColonyMapTab_NodeInfo>();
					if (uiNodeInfo.MapID == mapID)
					{
						return uiNodeInfo;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Xây danh sách lãnh thổ
		/// </summary>
		private void BuildColonyMap()
		{
			/// Làm rỗng danh sách
			this.ClearMapList();

			/// Duyệt danh sách lãnh thổ
			foreach (ColonyMapXML data in Loader.Loader.ColonyMaps.Values)
			{
				/// Thêm lãnh thổ vào danh sách
				this.AddMap(data);
			}
		}

		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		private void DoRefresh()
		{
			/// Xóa danh sách cũ
			this.ClearGuildInfoList();

			/// Nếu không có dữ liệu từ GS đổ về thì thôi
			if (this._Data == null)
			{
				return;
			}

			/// Duyệt danh sách lãnh thổ từ GS đổ về
			foreach (GuildWarMiniMap gsData in this._Data)
			{
				/// Nút thông tin bản đồ tương ứng
				UILocalMap_ColonyMapTab_NodeInfo uiNodeInfo = this.FindNode(gsData.MapID);
				/// Nếu tìm thấy
				if (uiNodeInfo != null)
				{
					/// Màu tương ứng
					if (!ColorUtility.TryParseHtmlString(gsData.HexColor, out Color color))
                    {
						color = Color.white;
                    }
					uiNodeInfo.Color = color;
					uiNodeInfo.MapType = gsData.MapType;
					uiNodeInfo.Click = () => {
						this.UI_MapInfoFrame.MapID = gsData.MapID;
						this.UI_MapInfoFrame.Data = gsData;
						this.UI_MapInfoFrame.GuildTotalColony = this._Data.Where(x => x.GuildID == gsData.GuildID).Count();
						this.UI_MapInfoFrame.Show();
					};
				}
			}

			/// Xây lại danh sách bang hội chiếm hữu
			this.BuildGuildInfoList();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Cập nhật dữ liệu
		/// </summary>
		public void Refresh()
		{
			this.DoRefresh();
		}
		#endregion
	}
}
