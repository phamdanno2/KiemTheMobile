using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.KiemVu.Factory;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.CoreUI
{
    /// <summary>
    /// Lớp quản lý text trên thông tin nhân vật
    /// </summary>
    public class UIMonsterHeader : TTMonoBehaviour
    {
        #region Define
        /// <summary>
        /// Thanh máu
        /// </summary>
        [SerializeField]
        private Slider UISlider_HPBar;

        /// <summary>
        /// Thanh máu của Boss
        /// </summary>
        [SerializeField]
        private Slider UISlider_BossHPBar;

        /// <summary>
        /// Ảnh ngũ hành
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_Elemental;

        /// <summary>
        /// Text tên quái
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Name;

        /// <summary>
        /// Text danh hiệu
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Title;

        /// <summary>
        /// Tọa độ đặt (theo màn hình)
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;
        #endregion

        #region Properties
        private bool _IsBoss = false;
        /// <summary>
        /// Có phải Boss không
        /// </summary>
        public bool IsBoss
        {
            get
            {
                return this._IsBoss;
            }
            set
            {
                this._IsBoss = value;

                /// Nếu không hiện thanh máu
                if (!this.ShowHPBar)
                {
                    this.UISlider_HPBar.gameObject.SetActive(false);
                    this.UISlider_BossHPBar.gameObject.SetActive(false);
                    return;
                }

                this.UISlider_HPBar.gameObject.SetActive(!value);
                this.UISlider_BossHPBar.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Màu của tên
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

        private int _HPPercent = 0;
        /// <summary>
        /// % máu
        /// </summary>
        public int HPPercent
        {
            get
            {
                return this._HPPercent;
            }
            set
            {
                this.UISlider_HPBar.value = value / 100f;
                this.UISlider_BossHPBar.value = value / 100f;
            }
        }

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

        private string _Title = "";
        /// <summary>
        /// Danh hiệu
        /// </summary>
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;

                if (string.IsNullOrEmpty(value))
                {
                    this.UIText_Title.gameObject.SetActive(false);
                }
                else
                {
                    this.UIText_Title.gameObject.SetActive(true);
                    this.UIText_Title.text = value;
                }
            }
        }

        /// <summary>
        /// Ngũ hành
        /// </summary>
        public Elemental Elemental
        {
            set
            {
                if (Loader.Loader.Elements.TryGetValue(value, out ElementData elementDetail))
                {
                    this.UIImage_Elemental.sprite = elementDetail.SmallSprite;
                }
                
            }
        }

        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject { get; set; }

        /// <summary>
        /// Tọa độ điểm đặt (tính theo màn hình)
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                return this._Offset;
            }
            set
            {
                this._Offset = value;
            }
        }

        private bool _ShowHPBar = false;
        /// <summary>
        /// Hiển thị thanh máu
        /// </summary>
        public bool ShowHPBar
        {
            get
            {
                return this._ShowHPBar;
            }
            set
            {
                this._ShowHPBar = value;

                if (!value)
                {
                    this.UISlider_HPBar.gameObject.SetActive(false);
                    this.UISlider_BossHPBar.gameObject.SetActive(false);
                }
                else
                {
                    this.UISlider_HPBar.gameObject.SetActive(!this.IsBoss);
                    this.UISlider_BossHPBar.gameObject.SetActive(this.IsBoss);
                }
            }
        }

        private bool _ShowElemental = true;
        /// <summary>
        /// Hiển thị ngũ hành
        /// </summary>
        public bool ShowElemental
        {
            get
            {
                return this._ShowElemental;
            }
            set
            {
                this._ShowElemental = value;
                this.UIImage_Elemental.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Hiển thị thanh máu không (dùng trong thiết lập hiển thị hệ thống)
        /// </summary>
        public bool SystemSettingShowHPBar
        {
            get
            {
                return this.UISlider_HPBar.gameObject.activeSelf || this.UISlider_BossHPBar.gameObject.activeSelf;
            }
            set
            {
                if (this._ShowHPBar)
                {
                    this.UISlider_HPBar.gameObject.SetActive(!this.IsBoss);
                    this.UISlider_BossHPBar.gameObject.SetActive(this.IsBoss);
                }
            }
        }

        /// <summary>
        /// Có hiển thị tên, danh hiệu không
        /// </summary>
        public bool SystemSettingShowName
        {
            get
            {
                return this.UIText_Name.gameObject.activeSelf;
            }
            set
            {
                this.UIText_Name.gameObject.SetActive(value);
                if (!string.IsNullOrEmpty(this.Title))
                {
                    this.UIText_Title.gameObject.SetActive(value);
                }
                if (this._ShowElemental)
                {
                    this.UIImage_Elemental.gameObject.SetActive(value);
                }
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Đợi chút (tránh bug chữ trắng)
        /// </summary>
        private bool skipFirst;

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.skipFirst = true;
            IEnumerator DoSkip(float duration)
            {
                yield return new WaitForSeconds(duration);
                this.skipFirst = false;
                //this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(0.1f));
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
            this.UpdatePos();
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
                //this.UpdatePos();
            }
            this.StartCoroutine(DoSkip(0.1f));
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng bị hủy kích hoạt
        /// </summary>
        private void OnDisable()
        {
            
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
        }

        /// <summary>
        /// Cập nhật vị trí tương ứng vị trí của đối tượng
        /// </summary>
        private void UpdatePos()
        {
            if (this.ReferenceObject == null)
            {
                return;
            }

            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + this._Offset;
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y);
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
            this.HPPercent = 0;
            this.IsBoss = false;
            this.Name = "";
            this.Title = "";
            this.Elemental = default;
            this.ShowElemental = false;
            this.ReferenceObject = null;
            this.SystemSettingShowName = true;
            this.SystemSettingShowHPBar = true;
            this._ShowElemental = true;
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }

        /// <summary>
        /// Buộc đối tượng cập nhật lại vị trí của mình
        /// </summary>
        public void ForceSynsPositionImmediately()
        {
            this.UpdatePos();
        }
        #endregion
    }
}
