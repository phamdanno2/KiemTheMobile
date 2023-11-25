using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Server.Data;
using System.Threading.Tasks;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// thông tin thành viên gia tộc
    /// </summary>
    public class UIFamily_MemberList_MemberInfo : MonoBehaviour 
    {
        #region Define
        /// <summary>
        /// Toggle
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UIToggle;

        /// <summary>
        /// Text tên thành viên
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MemberName;

        /// <summary>
        /// Text môn phái
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MemberFaction;

        /// <summary>
        /// Text cấp độ
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MemberLevel;

        /// <summary>
        /// Text chức vụ trong bang
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MemberRank;

        /// <summary>
        /// Text trạng thái Online
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MemberState;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện khi đối tượng được chọn
        /// </summary>
        public Action Select { get; set; }

        private FamilyMember _Data;
        /// <summary>
        /// thông tin thành viên
        /// </summary>
        public FamilyMember Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;
                this.Refresh();
            }
        }
        #endregion

        #region core MonoBehaviour
        /// <summary>
        /// Hàm này được ở frame đầu tiền
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        private void InitPrefabs()
        {
            this.UIToggle.onValueChanged.AddListener(this.Toggle_Selected);
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

		#region Public methods
        /// <summary>
        /// Làm mới dữ liệu
        /// </summary>
        public void Refresh()
		{
            this.UIText_MemberName.text = this.Data.RoleName;
            this.UIText_MemberFaction.text = KTGlobal.GetFactionName(this.Data.FactionID, out Color factionColor);
            this.UIText_MemberFaction.color = factionColor;
            this.UIText_MemberLevel.text = this.Data.Level.ToString();
            this.UIText_MemberRank.text = KTGlobal.GetFamilyRankName(this.Data.Rank);
            this.UIText_MemberState.text = this.Data.OnlienStatus == 1 ? "<color=green>ONLINE</color>" : "<color=red>OFFLINE</color>";
        }
		#endregion
	}
}
