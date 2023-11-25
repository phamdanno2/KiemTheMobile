using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// Thông tin gia tộc
    /// </summary>
    class UIFamily_UIFamilyList_FamilyInfo : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Toggle
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UI_Toggle;

        /// <summary>
        /// Text tên gia tộc
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyName;

        /// <summary>
        /// Text tên tộc trưởng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyMasterName;

        /// <summary>
        /// Text tổng số thành viên
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MembersCount;

        /// <summary>
        /// Text Tông uy danh 
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_TotalRank;

        /// <summary>
        /// Text tên bang
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_GuildName;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi đối tượng được chọn
        /// </summary>
        public Action Select { get; set; }

        private Server.Data.FamilyInfo _Data;
        /// <summary>
        /// Dữ liệu tộc
        /// </summary>
        public Server.Data.FamilyInfo Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                if( value == null)
                {
                    this.UIText_FamilyName.text = "";
                    this.UIText_FamilyMasterName.text = "";
                    this.UIText_MembersCount.text = "";
                    this.UIText_TotalRank.text = "";
                    this.UIText_GuildName.text = "";
                }
                else
                {
                    this._Data = value;
                    this.UIText_FamilyName.text = value.FamilyName;
                    this.UIText_FamilyMasterName.text = value.Learder;
                    this.UIText_MembersCount.text = string.Format("{0}", value.TotalMember);
                    this.UIText_TotalRank.text = value.TotalPoint.ToString();
                    this.UIText_GuildName.text = value.GuildName;
                }
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
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
            this.UI_Toggle.onValueChanged.AddListener(this.Toggle_Selected);
        }

        /// <summary>
        /// Sự kiện khi Toggle được chọn
        /// </summary>
        /// <param name="isSelected"></param>
        private void Toggle_Selected(bool isSelected)
        {
            if (isSelected)
            {
                this.Select?.Invoke();
            }
        }
        #endregion
    }
}
