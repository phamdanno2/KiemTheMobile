using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.Utilities.UnityUI;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using System;
using System.Linq;

namespace FSPlay.KiemVu.UI.CoreUI
{
    /// <summary>
    /// Khung chiếu đối tượng trên bản đồ nhỏ
    /// </summary>
    public class UIMinimapReference : MonoBehaviour
    {
        /// <summary>
        /// Hiệu ứng trạng thái nhiệm vụ tương ứng
        /// </summary>
        [Serializable]
        private class QuestStateIconPrefab
        {
            /// <summary>
            /// Trạng thái nhiệm vụ
            /// </summary>
            public NPCTaskStates State;

            /// <summary>
            /// Đối tượng thực thi hiệu ứng
            /// </summary>
            public UIAnimatedSprite UIAnimatedSprite;
        }

        #region Define
        /// <summary>
        /// Tên đối tượng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Name;

        /// <summary>
        /// Ảnh đại diện ở Minimap
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UISprite_MinimapIcon;

        /// <summary>
        /// Tọa độ đặt Text (theo màn hình)
        /// </summary>
        [SerializeField]
        private Vector2 _TextOffset;

        /// <summary>
        /// Hiệu ứng trạng thái nhiệm vụ tương ứng
        /// </summary>
        [SerializeField]
        private QuestStateIconPrefab[] QuestStateIconPrefabs;
        #endregion

        #region Properties
        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public string Name
        {
            get
            {
                return this.UIText_Name.text;
            }
            set
            {
                this.UIText_Name.text = value;
            }
        }

        /// <summary>
        /// Màu chữ tên đối tượng
        /// </summary>
        public Color NameColor
        {
            get
            {
                return this.UIText_Name.color;
            }
            set
            {
                this.UIText_Name.color = value;
            }
        }

        /// <summary>
        /// Hiển thị tên đối tượng
        /// </summary>
        public bool ShowName
        {
            get
            {
                return this.UIText_Name.gameObject.activeSelf;
            }
            set
            {
                this.UIText_Name.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Hiển thị Icon đối tượng
        /// </summary>
        public bool ShowIcon
        {
            get
            {
                return this.UISprite_MinimapIcon.gameObject.activeSelf;
            }
            set
            {
                this.UISprite_MinimapIcon.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Đường dẫn Bundle chứa ảnh
        /// </summary>
        public string BundleDir
        {
            get
            {
                return this.UISprite_MinimapIcon.BundleDir;
            }
            set
            {
                this.UISprite_MinimapIcon.BundleDir = value;
            }
        }

        /// <summary>
        /// Tên Atlas chứa ảnh
        /// </summary>
        public string AtlasName
        {
            get
            {
                return this.UISprite_MinimapIcon.AtlasName;
            }
            set
            {
                this.UISprite_MinimapIcon.AtlasName = value;
            }
        }

        /// <summary>
        /// Tên ảnh Icon đối tượng
        /// </summary>
        public string SpriteName
        {
            get
            {
                return this.UISprite_MinimapIcon.SpriteName;
            }
            set
            {
                this.UISprite_MinimapIcon.SpriteName = value;
                this.UpdateIcon();
            }
        }

        /// <summary>
        /// Tọa độ điểm đặt (tính theo màn hình)
        /// </summary>
        public Vector2 TextOffset
        {
            get
            {
                return this._TextOffset;
            }
            set
            {
                this._TextOffset = value;
            }
        }

        /// <summary>
        /// Kích thước Icon
        /// </summary>
        public Vector2 IconSize
        {
            get
            {
                return this.UISprite_MinimapIcon.gameObject.GetComponent<RectTransform>().sizeDelta;
            }
            set
            {
                this.UISprite_MinimapIcon.gameObject.GetComponent<RectTransform>().sizeDelta = value;
            }
        }

        private NPCTaskStates _NPCTaskState = NPCTaskStates.None;
        /// <summary>
        /// Trạng thái nhiệm vụ của NPC
        /// </summary>
        public NPCTaskStates NPCTaskState
        {
            get
            {
                return this._NPCTaskState;
            }
            set
            {
                this._NPCTaskState = value;

                /// Ẩn toàn bộ trạng thái TaskState của NPC
                this.DisableAllNPCStatesIcon();

                QuestStateIconPrefab uiStatePrefab = this.QuestStateIconPrefabs.Where(x => x.State == value).FirstOrDefault();
                /// Nếu tìm thấy
                if (uiStatePrefab != null)
                {
                    uiStatePrefab.UIAnimatedSprite.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject { get; set; }
        #endregion

        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Đợi chút (tránh bug chữ trắng)
        /// </summary>
        private bool skipFirst;

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
            }
            this.StartCoroutine(DoSkip(0.1f));

            this.UIText_Name.gameObject.GetComponent<RectTransform>().anchoredPosition = this._TextOffset;
        }

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame
        /// </summary>
        private void Update()
        {
            if (this.skipFirst)
            {
                return;
            }
            if (this.ReferenceObject == null)
            {
                return;
            }
            this.gameObject.transform.position = new Vector3(this.ReferenceObject.transform.position.x, this.ReferenceObject.transform.position.y, 0);
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
            }
            this.StartCoroutine(DoSkip(0.1f));

            this.UIText_Name.gameObject.GetComponent<RectTransform>().anchoredPosition = this._TextOffset;
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng bị hủy kích hoạt
        /// </summary>
        private void OnDisable()
        {

        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
        }

        /// <summary>
        /// Cập nhật ảnh Icon đối tượng
        /// </summary>
        private void UpdateIcon()
        {
            this.UISprite_MinimapIcon.Load();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Ẩn toàn bộ trạng thái Task của NPC
        /// </summary>
        private void DisableAllNPCStatesIcon()
        {
            /// Làm rỗng hiển thị
            foreach (QuestStateIconPrefab _uiStatePrefab in this.QuestStateIconPrefabs)
            {
                _uiStatePrefab.UIAnimatedSprite.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            this.StopAllCoroutines();
            this.NameColor = default;
            this.Name = "";
            this.ShowName = false;
            this.ShowIcon = false;
            this.BundleDir = "";
            this.AtlasName = "";
            this.SpriteName = "";
            this.TextOffset = default;
            this.IconSize = default;
            this.ReferenceObject = null;
            this.DisableAllNPCStatesIcon();
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }
        #endregion
    }
}
