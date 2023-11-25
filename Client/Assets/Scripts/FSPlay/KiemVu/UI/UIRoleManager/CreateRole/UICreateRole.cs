using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using FSPlay.GameEngine.Logic;
using FSPlay.GameFramework.Logic;
using FSPlay.GameEngine.Network;

namespace FSPlay.KiemVu.UI.RoleManager
{
    /// <summary>
    /// Màn hình tạo nhân vật
    /// </summary>
    public class UICreateRole : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button quay trở lại
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_GoBack;

        /// <summary>
        /// Toggle giới tính nhân vật Nam
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UIToggle_Male;

        /// <summary>
        /// Toggle giới tính nhân vật nữ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UIToggle_Female;

        /// <summary>
        /// Đối tượng cha chứ danh sách tân thủ thôn
        /// </summary>
        [SerializeField]
        private RectTransform UITransform_Villages;

        /// <summary>
        /// Prefab tân thủ thôn
        /// </summary>
        [SerializeField]
        private UIToggleSprite UIToggle_VillagePrefab;

        /// <summary>
        /// Input tên nhân vật
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_RoleName;

        /// <summary>
        /// Button lấy tên ngẫu nhiên cho nhân vật
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_GetRandomName;

        /// <summary>
        /// Button tạo nhân vật
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_CreateRole;
        #endregion

        /// <summary>
        /// ID tân thủ thôn được chọn
        /// </summary>
        private int SelectedVillageID = -1;

        /// <summary>
        /// Giới tính nhân vật được chọn
        /// </summary>
        private int SelectedSex
        {
            get
            {
                if (this.UIToggle_Male.isOn)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        #region Properties
        private List<int> _Villages;
        /// <summary>
        /// Danh sách tân thủ thôn
        /// </summary>
        public List<int> Villages
        {
            get
            {
                return this._Villages;
            }
            set
            {
                this._Villages = value;
                this.RefreshVillages();
            }
        }

        /// <summary>
        /// Sự kiện khi nút quay lại được ấn
        /// </summary>
        public Action GoBack { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Thiết lập mặc định
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_GoBack.onClick.AddListener(this.ButtonGoBack_Clicked);
            this.UIButton_GetRandomName.onClick.AddListener(this.ButtonGetRandomName_Clicked);
            this.UIButton_CreateRole.onClick.AddListener(this.ButtonCreateRole_Clicked);
        }

        /// <summary>
        /// Sự kiện khi nút quay lại được ấn
        /// </summary>
        private void ButtonGoBack_Clicked()
        {
            this.GoBack?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi nút lấy ngẫu nhiên tên nhân vật được ấn
        /// </summary>
        private void ButtonGetRandomName_Clicked()
        {
            //GameInstance.Game.GetRandomPreName(this.SelectedSex);
        }

        /// <summary>
        /// Sự kiện khi nút tạo nhân vật được ấn
        /// </summary>
        private void ButtonCreateRole_Clicked()
        {
            string roleName = this.UIInput_RoleName.text;
            if (string.IsNullOrEmpty(roleName))
            {
                Super.ShowMessageBox("Lỗi nhập liệu", "Hãy nhập vào tên nhân vật!", true);
                return;
            }
            else if (!KTFormValidation.IsValidString(roleName, false, true, false, false))
            {
                Super.ShowMessageBox("Lỗi nhập liệu", "Tên nhân vật có chứa ký tự đặc biệt, hãy nhập lại!", true);
                return;
            }

            /// Tên nhân vật
            roleName = KTFormValidation.StandardizeString(roleName, true, false);

            if (roleName.Length < 6)
            {
                Super.ShowMessageBox("Lỗi nhập liệu", "Tên nhân vật phải có tối thiểu 6 ký tự!", true);
                return;
            }
            if (this.UIInput_RoleName.text.Length > 12)
            {
                Super.ShowMessageBox("Lỗi nhập liệu", "Tên nhân vật chỉ được có tối đa 12 ký tự!", true);
                return;
            }

            if (this.SelectedVillageID == -1)
            {
                Super.ShowMessageBox("Lỗi", "Hãy chọn một tân thủ thôn.", true);
                return;
            }

            int serverID = 1;
            if (Global.Data != null)
            {
                serverID = Global.Data.GameServerID;
            }

            Super.ShowNetWaiting("Đang khởi tạo nhân vật...");

            /// Giới tính
            int sex = this.SelectedSex;
            /// Môn phái
            int factionID = 0;
            /// Tân thủ thôn
            int villageID = this.SelectedVillageID;
            
            GameInstance.Game.CreateRole(sex, factionID, string.Format("{0}${1}", this.UIInput_RoleName.text.Trim(), ""), serverID, villageID);

            /// Gửi gói tin lên Server
            if (Global.Data.ServerData != null)
            {
                if (Global.Data.ServerData.LastServer != null)
                {
                    PlatformUserLogin.RecordLoginServerIDs(Global.Data.ServerData.LastServer);
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Làm mới danh sách tân thủ thôn
        /// </summary>
        private void RefreshVillages()
        {
            foreach (int villageID in this._Villages)
            {
                if (FSPlay.KiemVu.Loader.Loader.Maps.TryGetValue(villageID, out Entities.Config.Map mapInfo))
                {
                    UIToggleSprite toggle = GameObject.Instantiate<UIToggleSprite>(this.UIToggle_VillagePrefab);
                    toggle.transform.SetParent(this.UITransform_Villages, false);
                    toggle.gameObject.SetActive(true);

                    toggle.Name = mapInfo.Name;
                    toggle.OnSelected = (isSelected) => {
                        this.SelectedVillageID = villageID;
                    };
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Thiết lập tên nhân vật ngẫu nhiên
        /// </summary>
        /// <param name="name"></param>
        public void SetRandomName(string name)
        {
            this.UIInput_RoleName.text = name;
        }
        #endregion
    }
}
