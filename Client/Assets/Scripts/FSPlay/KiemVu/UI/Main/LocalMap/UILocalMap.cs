using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FSPlay.Drawing;
using System;
using UnityEngine.EventSystems;
using FSPlay.GameEngine.Logic;
using UnityEngine.UI;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System.Linq;
using FSPlay.KiemVu.UI.Main.LocalMap.ColonyMap;

namespace FSPlay.KiemVu.UI.Main.LocalMap
{
    /// <summary>
    /// Khung bản đồ khu vực
    /// </summary>
    public class UILocalMap : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Prefab tab vị trí các điểm dịch chuyển nhanh
        /// </summary>
        [SerializeField]
        private UILocalMap_TabItem UIToggle_LocationTabPrefab;

        /// <summary>
        /// Gốc khung chứa các điểm dịch chuyển nhanh
        /// </summary>
        [SerializeField]
        private RectTransform UI_QuickLocationRoot;

        /// <summary>
        /// Text vị trí hiện tại
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_CurrentLocation;

        /// <summary>
        /// Nút đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Tab bản đồ khu vực
        /// </summary>
        [SerializeField]
        private UILocalMap_LocalMapTab UILocalMapTab;

        /// <summary>
        /// Tab bản đồ thế giới
        /// </summary>
        [SerializeField]
        private UILocalMap_WorldMapTab UIWorldMapTab;

        /// <summary>
        /// Tab bản đồ liên máy chủ
        /// </summary>
        [SerializeField]
        private UILocalMap_CrossServerMapTab UICrossServerMapTab;

        /// <summary>
        /// Tab bản đồ lãnh thổ
        /// </summary>
        [SerializeField]
        private UILocalMap_ColonyMapTab UIColonyMapTab;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Khung có hiển thị không
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }

        /// <summary>
        /// Ảnh Map
        /// </summary>
        public UnityEngine.Sprite LocalMapSprite
        {
            get
            {
                return this.UILocalMapTab.LocalMapSprite;
            }
            set
            {
                this.UILocalMapTab.LocalMapSprite = value;
            }
        }

        /// <summary>
        /// Kích thước bản đồ thực tế
        /// </summary>
        public Vector2 RealMapSize
        {
            get
            {
                return this.UILocalMapTab.RealMapSize;
            }
            set
            {
                this.UILocalMapTab.RealMapSize = value;
            }
        }

        /// <summary>
        /// Tên Map
        /// </summary>
        public string LocalMapName { get; set; }

        /// <summary>
        /// Sự kiện khi bản đồ được Click
        /// </summary>
        public Action<Vector2> LocalMapClicked
        {
            get
            {
                return this.UILocalMapTab.LocalMapClicked;
            }
            set
            {
                this.UILocalMapTab.LocalMapClicked = value;
            }
        }

        private List<KeyValuePair<string, Point>> _ListNPCs;
        /// <summary>
        /// Danh sách NPC trong Map
        /// </summary>
        public List<KeyValuePair<string, Point>> ListNPCs
        {
            get
            {
                return this._ListNPCs;
            }
            set
            {
                this._ListNPCs = value;
                this.UILocalMapTab.ListNPCs = value;
            }
        }

        private List<KeyValuePair<string, Point>> _ListTrainArea;
        /// <summary>
        /// Danh sách bãi train trong Map
        /// </summary>
        public List<KeyValuePair<string, Point>> ListTrainArea
        {
            get
            {
                return this._ListTrainArea;
            }
            set
            {
                this._ListTrainArea = value;
                this.UILocalMapTab.ListTrainArea = value;
            }
        }

        private List<KeyValuePair<string, Point>> _ListZone;
        /// <summary>
        /// Danh sách các vùng trong bản đồ
        /// </summary>
        public List<KeyValuePair<string, Point>> ListZone
        {
            get
            {
                return this._ListZone;
            }
            set
            {
                this._ListZone = value;
                this.UILocalMapTab.ListZone = value;
            }
        }

        private List<KeyValuePair<string, Point>> _ListTeleport;
        /// <summary>
        /// Danh sách điểm truyền tống
        /// </summary>
        public List<KeyValuePair<string, Point>> ListTeleport
        {
            get
            {
                return this._ListTeleport;
            }
            set
            {
                this._ListTeleport = value;
                this.UILocalMapTab.ListTeleport = value;
            }
        }

        private Vector2 _LeaderPosition;
        /// <summary>
        /// Vị trí của Leader
        /// </summary>
        public Vector2 LeaderPosition
        {
            get
            {
                return this._LeaderPosition;
            }
            set
            {
                this._LeaderPosition = value;
                this.UILocalMapTab.LeaderPosition = value;
                this.UpdateLeaderLocation(value);
            }
        }

        /// <summary>
        /// Chuyển đến bản đồ tương ứng
        /// </summary>
        public Action<int> GoToMap
		{
			get
			{
                return this.UIWorldMapTab.GoToMap;
            }
			set
			{
                this.UIWorldMapTab.GoToMap = value;
			}
		}

        /// <summary>
        /// Chuyển đến vị trí của NPC tương ứng
        /// </summary>
        public Action<int, int> GoToNPC
		{
			get
			{
                return this.UICrossServerMapTab.GoToNPC;
			}
			set
			{
                this.UICrossServerMapTab.GoToNPC = value;
			}
		}

        /// <summary>
        /// Sự kiện truy vấn thông tin danh sách lãnh thổ
        /// </summary>
        public Action QueryTerritoryList
		{
			get
			{
                return this.UIColonyMapTab.QueryTerritoryList;
			}
			set
			{
                this.UIColonyMapTab.QueryTerritoryList = value;
			}
		}
        #endregion

        #region Private fields
        /// <summary>
        /// Rect Transform khung dịch chuyển nhanh
        /// </summary>
        private RectTransform uiQuickLocationRectTransform;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.uiQuickLocationRectTransform = this.UI_QuickLocationRoot.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.UpdateQuickLocationList();
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
        /// Sự kiện khi nút thoát được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
            this.Hide();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Cập nhật vị trí của Leader trong bản đồ
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="position"></param>
        private void UpdateLeaderLocation(Vector2 position)
        {
            Vector2 localMapPos = KTGlobal.WorldPositionToGridPosition(position);
            this.UIText_CurrentLocation.text = string.Format("<color=#00ff00>{0}</color> <color=#fcff2e>({1},{2})</color>", this.LocalMapName, (int) localMapPos.x, (int) localMapPos.y);
        }

        /// <summary>
        /// Hủy danh sách các điểm đến nhanh
        /// </summary>
        private void ClearQuickLocationList()
        {
            foreach (Transform child in this.UI_QuickLocationRoot.transform)
            {
                if (child.gameObject != this.UIToggle_LocationTabPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Cập nhật danh sách các điểm đến nhanh
        /// </summary>
        private void UpdateQuickLocationList()
        {
            this.ClearQuickLocationList();

            if (this.ListNPCs != null && this.ListNPCs.Count > 0)
            {
                UILocalMap_TabItem tabItem = GameObject.Instantiate<UILocalMap_TabItem>(this.UIToggle_LocationTabPrefab);
                tabItem.gameObject.SetActive(true);
                tabItem.transform.SetParent(this.UI_QuickLocationRoot.transform, false);
                tabItem.Text = "NPC";
                tabItem.Active = false;
                List<KeyValuePair<string, Vector2>> listItem = new List<KeyValuePair<string, Vector2>>();
                tabItem.ListItems = listItem;
                foreach (KeyValuePair<string, Point> pair in this.ListNPCs)
                {
                    listItem.Add(new KeyValuePair<string, Vector2>(pair.Key, new Vector2(pair.Value.X, pair.Value.Y)));
                }
                tabItem.LocationSelected = (pair) => {
                    this.UILocalMapTab.GoToPos(pair.Value);
                };
                tabItem.OnSelected = (isSelected) => {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(this.uiQuickLocationRectTransform);
                };
            }

            if (this.ListTrainArea != null && this.ListTrainArea.Count > 0)
            {
                UILocalMap_TabItem tabItem = GameObject.Instantiate<UILocalMap_TabItem>(this.UIToggle_LocationTabPrefab);
                tabItem.gameObject.SetActive(true);
                tabItem.transform.SetParent(this.UI_QuickLocationRoot.transform, false);
                tabItem.Text = "Bãi train";
                tabItem.Active = false;
                List<KeyValuePair<string, Vector2>> listItem = new List<KeyValuePair<string, Vector2>>();
                tabItem.ListItems = listItem;
                foreach (KeyValuePair<string, Point> pair in this.ListTrainArea)
                {
                    listItem.Add(new KeyValuePair<string, Vector2>(pair.Key, new Vector2(pair.Value.X, pair.Value.Y)));
                }
                tabItem.LocationSelected = (pair) => {
                    this.UILocalMapTab.GoToPos(pair.Value);
                };
                tabItem.OnSelected = (isSelected) => {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(this.uiQuickLocationRectTransform);
                };
            }

            if (this.ListTeleport != null && this.ListTeleport.Count > 0)
            {
                UILocalMap_TabItem tabItem = GameObject.Instantiate<UILocalMap_TabItem>(this.UIToggle_LocationTabPrefab);
                tabItem.gameObject.SetActive(true);
                tabItem.transform.SetParent(this.UI_QuickLocationRoot.transform, false);
                tabItem.Text = "Điểm truyền tống";
                tabItem.Active = false;
                List<KeyValuePair<string, Vector2>> listItem = new List<KeyValuePair<string, Vector2>>();
                tabItem.ListItems = listItem;
                foreach (KeyValuePair<string, Point> pair in this.ListTeleport)
                {
                    listItem.Add(new KeyValuePair<string, Vector2>(pair.Key, new Vector2(pair.Value.X, pair.Value.Y)));
                }
                tabItem.LocationSelected = (pair) => {
                    this.UILocalMapTab.GoToPos(pair.Value);
                };
                tabItem.OnSelected = (isSelected) => {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(this.uiQuickLocationRectTransform);
                };
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiển thị
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Ẩn
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        #region Local Map
        /// <summary>
        /// Xóa đường kẻ đường đi
        /// </summary>
        public void DestroyPathLine()
        {
            this.UILocalMapTab.DestroyPathLine();
        }

        /// <summary>
        /// Hiển thị đường kẻ đường đi
        /// </summary>
        public void ShowPathLine()
        {
            this.UILocalMapTab.ShowPathLine();
        }

        /// <summary>
        /// Làm mới hiển thị trạng thái nhiệm vụ của NPC
        /// </summary>
        public void RefreshNPCTaskStates()
        {
            this.UILocalMapTab.RefreshNPCTaskStates();
        }

        /// <summary>
        /// Làm mới thông tin đội viên
        /// </summary>
        public void RefreshTeamMembers()
        {
            this.UILocalMapTab.RefreshTeamMembers();
        }

        /// <summary>
        /// Cập nhật quái và Boss đặc biệt
        /// </summary>
        /// <param name="monsters"></param>
        public void UpdateSpecialMonsterList(List<LocalMapMonsterData> monsters)
        {
            this.UILocalMapTab.UpdateSpecialMonsterList(monsters);
        }

        /// <summary>
        /// Cập nhật vị trí cờ chỉ đường trên bản đồ nhỏ
        /// </summary>
        /// <param name="worldPos"></param>
        public void UpdateLocalMapFlagPos(Vector2 worldPos)
        {
            this.UILocalMapTab.UpdateLocalMapFlagPos(worldPos);
        }
		#endregion

		#region Colony Map
        /// <summary>
        /// Làm mới dữ liệu bản đồ lãnh thổ
        /// </summary>
        /// <param name="data"></param>
        public void RefreshColonyMap(List<GuildWarMiniMap> data)
		{
            this.UIColonyMapTab.Data = data;
            this.UIColonyMapTab.Refresh();
		}
		#endregion
		#endregion
	}
}

