using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using FSPlay.KiemVu.Entities.Config;
using System.Linq;
using FSPlay.KiemVu.Control.Component;

namespace FSPlay.KiemVu.UI.CoreUI
{
    /// <summary>
    /// Lớp quản lý text trên thông tin nhân vật
    /// </summary>
    public class UIRoleHeader : TTMonoBehaviour
    {
        /// <summary>
        /// Icon trạng thái PK
        /// </summary>
        [Serializable]
        private class PKValueIconSprite
        {
            /// <summary>
            /// Trị PK
            /// </summary>
            public int Value;

            /// <summary>
            /// Sprite Icon
            /// </summary>
            public string SpriteName;
        }

        #region Define
        /// <summary>
        /// Thanh máu
        /// </summary>
        [SerializeField]
        private Slider UISlider_HPBar;

        /// <summary>
        /// Thanh mana
        /// </summary>
        [SerializeField]
        private Slider UISlider_MPBar;

        /// <summary>
        /// Text tên nhân vật
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Name;

        /// <summary>
        /// Text danh hiệu bang hội
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_GuildTitle;

        /// <summary>
        /// Danh hiệu nhân vật
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RoleTitle;

        /// <summary>
        /// Text danh hiệu tạm
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Title;

        /// <summary>
        /// Ảnh danh hiệu quan hàm
        /// </summary>
        [SerializeField]
        private UIAnimatedSprite UIImage_OfficeTitle;

        /// <summary>
        /// Ảnh danh hiệu phi phong
        /// </summary>
        [SerializeField]
        private UIAnimatedSprite UIImage_MantleTitle;

        /// <summary>
        /// Tọa độ đặt
        /// </summary>
        [SerializeField]
        private Vector2 _Offset;

        /// <summary>
        /// Tọa độ đặt khi cưỡi ngựa
        /// </summary>
        [SerializeField]
        private Vector2 _RiderOffset;

        /// <summary>
        /// Icon trưởng nhóm
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_TeamLeaderIcon;

        /// <summary>
        /// Icon đội viên
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_TeamMemberIcon;

        /// <summary>
        /// Icon lượng sát khí hiện có
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_PKValueIcon;

        /// <summary>
        /// Danh sách Sprite tương đương sát khí
        /// </summary>
        [SerializeField]
        private PKValueIconSprite[] PKValueSprites;
        #endregion

        #region Private fields
        /// <summary>
        /// Foreground của thanh máu
        /// </summary>
        private UnityEngine.UI.Image hpBarForeground;

        /// <summary>
        /// RectTransform của đối tượng
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// RectTransform Box tên đối tượng
        /// </summary>
        private RectTransform nameBoxTransform;

        /// <summary>
        /// Màu ban đầu của thanh HP Bar
        /// </summary>
        private Color initHPBarColor;

        /// <summary>
        /// Màu ban đầu của thanh HP Bar
        /// </summary>
        private Color initNameColor;

        /// <summary>
        /// Component Character
        /// </summary>
        private Character componentCharacter = null;

        /// <summary>
        /// Luồng cập nhật vị trí
        /// </summary>
        private Coroutine updatePosCoroutine = null;
        #endregion

        #region Properties
        /// <summary>
        /// Màu của thanh máu
        /// </summary>
        public Color HPBarColor
        {
            get
            {
                return this.hpBarForeground.color;
            }
            set
            {
                this.hpBarForeground.color = value;
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

        /// <summary>
        /// Màu của danh hiệu bang
        /// </summary>
        public Color GuildTitleColor
        {
            get
            {
                return this.UIText_GuildTitle.color;
            }
            set
            {
                this.UIText_GuildTitle.color = value;
            }
        }

        /// <summary>
        /// Hiển thị thanh khí
        /// </summary>
        public bool IsShowMPBar
        {
            get
            {
                return this.UISlider_MPBar.gameObject.activeSelf;
            }
            set
            {
                this.UISlider_MPBar.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// % máu
        /// </summary>
        public int HPPercent
        {
            get
            {
                return (int) (this.UISlider_HPBar.value * 100);
            }
            set
            {
                this.UISlider_HPBar.value = value / 100f;
            }
        }

        /// <summary>
        /// % khí
        /// </summary>
        public int MPPercent
        {
            get
            {
                return (int) (this.UISlider_MPBar.value * 100);
            }
            set
            {
                this.UISlider_MPBar.value = value / 100f;
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
                this.RebuildLayout(this.nameBoxTransform);
            }
        }

        private string _GuildTitle = "";
        /// <summary>
        /// Danh hiệu bang hội
        /// </summary>
        public string GuildTitle
        {
            get
            {
                return this._GuildTitle;
            }
            set
            {
                this._GuildTitle = value;

                if (string.IsNullOrEmpty(value))
                {
                    this.UIText_GuildTitle.gameObject.SetActive(false);
                }
                else
                {
                    this.UIText_GuildTitle.gameObject.SetActive(true);
                    this.UIText_GuildTitle.text = value;
                }
                this.RebuildLayout(this.nameBoxTransform);
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
                this.RebuildLayout(this.nameBoxTransform);
            }
        }

        private GameObject _ReferenceObject;
        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GameObject ReferenceObject
        {
            get
            {
                return this._ReferenceObject;
            }
            set
            {
                this._ReferenceObject = value;
                if (value != null)
                {
                    /// Gắn lại thành phần tương ứng
                    this.componentCharacter = value.GetComponent<Character>();
                }
                else
                {
                    this.componentCharacter = null;
                }
            }
        }

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

        /// <summary>
        /// Đánh dấu có phải Leader không
        /// </summary>
        public bool IsLeader { get; set; } = false;

        private int _PKValue = 0;
        /// <summary>
        /// Trị PK
        /// </summary>
        public int PKValue
        {
            get
            {
                return this._PKValue;
            }
            set
            {
                this._PKValue = value;
                /// Nếu không có sát khí
                if (value <= 0)
                {
                    this.UIImage_PKValueIcon.gameObject.SetActive(false);
                }
                else
                {
                    this.UIImage_PKValueIcon.gameObject.SetActive(true);

                    int nValue = value;
                    if (nValue >= this.PKValueSprites.Length)
                    {
                        nValue = this.PKValueSprites.Length - 1;
                    }
                    this.UIImage_PKValueIcon.SpriteName = this.PKValueSprites[nValue].SpriteName;
                    this.UIImage_PKValueIcon.Load();
                }
            }
        }

        /// <summary>
        /// Hiển thị thanh máu không
        /// </summary>
        public bool SystemSettingShowHPBar
        {
            get
            {
                return this.UISlider_HPBar.gameObject.activeSelf;
            }
            set
            {
                this.UISlider_HPBar.gameObject.SetActive(value);
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
                if (!string.IsNullOrEmpty(this.GuildTitle))
                {
                    this.UIText_GuildTitle.gameObject.SetActive(value);
                }
                //this.UIImage_CoatTitle.gameObject.SetActive(value);
                //this.UIImage_RoyalSealTitle.gameObject.SetActive(value);
            }
        }

        private int _TeamType = 0;
        /// <summary>
        /// Loại tổ đội
        /// <para>1: Đội trưởng, 2: Đội viên, Giá trị khác: Ẩn icon</para>
        /// </summary>
        public int TeamType
        {
            get
            {
                return this._TeamType;
            }
            set
            {
                this._TeamType = value;

                this.UIImage_TeamLeaderIcon.gameObject.SetActive(false);
                this.UIImage_TeamMemberIcon.gameObject.SetActive(false);
                if (value == 1)
                {
                    this.UIImage_TeamLeaderIcon.gameObject.SetActive(true);
                }
                else if (value == 2)
                {
                    this.UIImage_TeamMemberIcon.gameObject.SetActive(true);
                }
            }
        }

        private long _RoleValue = 0;
        /// <summary>
        /// Vinh dự tài phú của nhân vật
        /// </summary>
        public long RoleValue
        {
            get
            {
                return this._RoleValue;
            }
            set
            {
                this._RoleValue = value;

                /// Danh hiệu Phi phong tương ứng
                MantleTitleXML mantleTitle = Loader.Loader.MantleTitles.OrderByDescending(x => x.RoleValue).Where(x => x.RoleValue <= value / 10000f).FirstOrDefault();
                /// Nếu không tìm thấy
                if (mantleTitle == null)
                {
                    this.UIImage_MantleTitle.gameObject.SetActive(false);
                }
                /// Nếu tìm thấy
                else
                {
                    this.UIImage_MantleTitle.AutoPlay = false;
                    this.UIImage_MantleTitle.BundleDir = mantleTitle.BundleDir;
                    this.UIImage_MantleTitle.AtlasName = mantleTitle.AtlasName;
                    this.UIImage_MantleTitle.SpriteNames = mantleTitle.SpriteNames.ToArray();
                    this.UIImage_MantleTitle.Duration = mantleTitle.AnimationSpeed;
                    this.UIImage_MantleTitle.PixelPerfect = true;
                    this.UIImage_MantleTitle.Scale = mantleTitle.Scale;
                    this.UIImage_MantleTitle.gameObject.SetActive(true);
                    this.UIImage_MantleTitle.Play();
                    /// Xây lại giao diện
                    this.RebuildLayout(this.UIImage_MantleTitle.transform.parent.GetComponent<RectTransform>());
                }
            }
        }

        private int _RoleOfficeRank = -1;
        /// <summary>
        /// ID quan hàm hiện tại
        /// </summary>
        public int RoleOfficeRank
		{
			get
			{
                return this._RoleOfficeRank;
			}
			set
			{
                this._RoleOfficeRank = value;

                /// Tìm thông tin danh hiệu tương ứng
                OfficeTitleXML officeTitle = Loader.Loader.OfficeTitles.Where(x => x.ID == value).FirstOrDefault();

                /// Nếu không tìm thấy
                if (officeTitle == null)
				{
                    this.UIImage_OfficeTitle.gameObject.SetActive(false);
                    /// Xây lại giao diện
                    this.RebuildLayout(this.UIImage_OfficeTitle.transform.parent.GetComponent<RectTransform>());
                }
				/// Nếu tìm thấy
				else
				{
                    this.UIImage_OfficeTitle.AutoPlay = false;
                    this.UIImage_OfficeTitle.BundleDir = officeTitle.BundleDir;
                    this.UIImage_OfficeTitle.AtlasName = officeTitle.AtlasName;
                    this.UIImage_OfficeTitle.SpriteNames = officeTitle.SpriteNames.ToArray();
                    this.UIImage_OfficeTitle.Duration = officeTitle.AnimationSpeed;
                    this.UIImage_OfficeTitle.PixelPerfect = true;
                    this.UIImage_OfficeTitle.Scale = officeTitle.Scale;
                    this.UIImage_OfficeTitle.gameObject.SetActive(true);
                    this.UIImage_OfficeTitle.Play();
                    /// Xây lại giao diện
                    this.RebuildLayout(this.UIImage_OfficeTitle.transform.parent.GetComponent<RectTransform>());
                }
            }
		}

        private int _CurrentRoleTitle = -1;
        /// <summary>
        /// Danh hiệu nhân vật hiện tại
        /// </summary>
        public int CurrentRoleTitle
		{
			get
			{
                return this._CurrentRoleTitle;
			}
			set
			{
                this._CurrentRoleTitle = value;

                /// Thông tin danh hiệu tương ứng
                if (Loader.Loader.RoleTitles != null && Loader.Loader.RoleTitles.TryGetValue(value, out KTitleXML titleXML))
				{
                    this.UIText_RoleTitle.text = titleXML.Text;
                    /// Xây lại giao diện
                    this.RebuildLayout(this.UIText_RoleTitle.transform.parent.GetComponent<RectTransform>());
                }
                /// Nếu không tồn tại
				else
				{
                    this._CurrentRoleTitle = -1;
                    this.UIText_RoleTitle.text = "";
                    /// Xây lại giao diện
                    this.RebuildLayout(this.UIText_RoleTitle.transform.parent.GetComponent<RectTransform>());
                }
			}
		}
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.nameBoxTransform = this.UIText_Name.transform.parent.GetComponent<RectTransform>();
            this.hpBarForeground = this.UISlider_HPBar.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
            this.initHPBarColor = this.hpBarForeground.color;
            this.initNameColor = this.UIText_Name.color;
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.StopAllCoroutines();
            this.StartCoroutine(this.ExecuteAfterSec(0.2f, () => {
                /// Nếu không phải Leader
                if (!this.IsLeader)
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    /// Cập nhật tọa độ
                    this.updatePosCoroutine = this.StartCoroutine(this.UpdatePosContinuously());
                }
                else
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    this.updatePosCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(60, () => {
                        this.UpdatePos_Leader();
                    }));
                }
            }));
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            this.gameObject.transform.position = new Vector2(-100000, -100000);

            this.StopAllCoroutines();
            this.StartCoroutine(this.ExecuteAfterSec(0.2f, () => {
                /// Nếu không phải Leader
                if (!this.IsLeader)
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    /// Cập nhật tọa độ
                    this.updatePosCoroutine = this.StartCoroutine(this.UpdatePosContinuously());
                }
                else
                {
                    if (this.updatePosCoroutine != null)
                    {
                        this.StopCoroutine(this.updatePosCoroutine);
                    }
                    this.updatePosCoroutine = this.StartCoroutine(this.ExecuteSkipFrames(60, () => {
                        this.UpdatePos_Leader();
                    }));
                }
            }));
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
        /// Thực thi sự kiện sau khoảng thời gian
        /// </summary>
        /// <param name="delaySec"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteAfterSec(float delaySec, Action work)
		{
            yield return new WaitForSeconds(delaySec);
            work?.Invoke();
		}

        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout(RectTransform transform)
        {
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
            }));
        }

        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIText_Name.text = "";
            this.UIText_Title.text = "";
            this.UIText_GuildTitle.text = "";
            this.UIText_RoleTitle.text = "";
            this.UISlider_HPBar.value = 0;
            this.UISlider_MPBar.value = 0;
            this.UIImage_MantleTitle.gameObject.SetActive(false);
            this.UIImage_OfficeTitle.gameObject.SetActive(false);
        }

        /// <summary>
        /// Cập nhật vị trí của Leader
        /// </summary>
        private void UpdatePos_Leader()
        {
            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + (this.componentCharacter != null && this.componentCharacter.RefObject.RoleData.IsRiding ? this._RiderOffset : this._Offset);
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(0, screenPos.y);

            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, this.GetComponent<RectTransform>( ).anchoredPosition.y);
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

            Vector2 worldUIPos = (Vector2) this.ReferenceObject.transform.position + (this.componentCharacter != null && this.componentCharacter.RefObject.RoleData.IsRiding ? this._RiderOffset : this._Offset);
            Vector2 screenPos = Global.MainCamera.WorldToScreenPoint(worldUIPos);
            this.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y);
        }

        /// <summary>
        /// Thực thi cập nhật vị trí liên tục
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePosContinuously()
        {
            /// Lặp liên tục
            while (true)
            {
                /// Bỏ qua Frame
                yield return null;
                /// Cập nhật tọa độ
                this.UpdatePos();
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
            this.HPBarColor = this.initHPBarColor;
            this.NameColor = this.initNameColor;
            this.HPPercent = 0;
            this.Name = "";
            this.Title = "";
            this.IsShowMPBar = false;
            this.MPPercent = 0;
            this.RoleValue = 0;
            this.GuildTitle = "";
            this.ReferenceObject = null;
            this.SystemSettingShowName = true;
            this.SystemSettingShowHPBar = true;
            this.PKValue = 0;
            this.IsLeader = false;
            KTUIElementPoolManager.Instance.ReturnToPool(this.rectTransform);
        }

        /// <summary>
        /// Buộc đối tượng cập nhật lại vị trí của mình
        /// </summary>
        public void ForceSynsPositionImmediately()
        {
            /// Nếu là Leader
            if (this.IsLeader)
            {
                this.UpdatePos_Leader();
            }
            else
            {
                this.UpdatePos();
            }
        }

        /// <summary>
        /// Chuyển màu tên, thanh máu về như ban đầu
        /// </summary>
        public void RestoreColor()
        {
            this.HPBarColor = this.initHPBarColor;
            this.NameColor = this.initNameColor;
        }
        #endregion
    }
}

