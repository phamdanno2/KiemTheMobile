using FSPlay.GameEngine.Logic;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// khung danh sách gia tộc chiêu mộ
    /// </summary>
    public class UIFamily_UIFamilyList : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Text điều kiện vào tộc
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_FamilyAskToJoinRequirement;

        /// <summary>
        /// Prefab thông tin Tộc
        /// </summary>
        [SerializeField]
        private UIFamily_UIFamilyList_FamilyInfo UI_FamilyInfoPrefab;

        /// <summary>
        /// Button xin vào bang
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AskToJoin;

        #endregion Define

        #region Private fields
        /// <summary>
        /// RectTransform danh sách Gia tộc
        /// </summary>
        private RectTransform transformFamilyList = null;

        /// <summary>
        /// RectTransform Text yêu cầu vào tộc
        /// </summary>
        private RectTransform transformRequirementText = null;

        /// <summary>
        /// Gia tộc được chọn
        /// </summary>
        private FamilyInfo selectedFamily = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện xin vào tộc
        /// </summary>
        public Action<int> AskToJoinGuild { get; set; }

        /// <summary>
        /// Dữ liệu gia tộc
        /// </summary>
        public List<FamilyInfo> Data { get; set; } 
        #endregion Properties

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformFamilyList = this.UI_FamilyInfoPrefab.transform.parent.GetComponent<RectTransform>();
            this.transformRequirementText = this.UIText_FamilyAskToJoinRequirement.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.Refresh();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_AskToJoin.onClick.AddListener(this.ButtonAskToJoin_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button xin gia nhập Tộc được ấn
        /// </summary>
        private void ButtonAskToJoin_Clicked()
        {
            /// Nếu chưa có gia tộc nào được chọn
            if (this.selectedFamily == null)
            {
                KTGlobal.AddNotification("Hãy chọn một gia tộc!");
                return;
            }
            /// Nếu bản thân đã có tộc
            else if (Global.Data.RoleData.FamilyID > 0)
            {
                KTGlobal.AddNotification("Bạn có gia tộc, không thể gia nhập gia tộc khác!");
                return;
            }
            /// Nếu gia tộc đã quá số thành viên
            else if (this.selectedFamily.TotalMember >= 30)
			{
                KTGlobal.AddNotification("Gia tộc này đã đầy, hãy chọn một gia tộc khác!");
                return;
            }

            /// Xác nhận
            KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận xin vào gia tộc <color=yellow>[{0}]</color> không?", this.selectedFamily.FamilyName), () => {
                /// Thực thi sự kiện tham gia gia tộc
                this.AskToJoinGuild?.Invoke(this.selectedFamily.FamilyID);
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Toggle gia tộc được chọn
        /// </summary>
        /// <param name="familyInfo"></param>
        private void ToggleGuild_Selected(FamilyInfo familyInfo)
        {
            /// Đánh dấu gia tộc được chọn
            this.selectedFamily = familyInfo;
            /// Thiết lập Text điều kiện vào tộc
            this.UIText_FamilyAskToJoinRequirement.text = familyInfo.RequestNotify;
            /// Xây lại giao diện
            this.RebuildLayout(this.transformRequirementText);
            /// Nếu bản thân đã có gia tộc
            if (Global.Data.RoleData.FamilyID > 0)
			{
                return;
			}
            /// Kích hoạt Button gia nhập
            this.UIButton_AskToJoin.gameObject.SetActive(true);
        }
        #endregion Code UI

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
        private void RebuildLayout(RectTransform transform)
        {
            if (!this.gameObject.activeSelf)
			{
                return;
			}

            /// Thực hiện xây lại giao diện
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách gia tộc
        /// </summary>
        private void ClearMemberList()
        {
            foreach (Transform child in this.transformFamilyList.transform)
            {
                if (child.gameObject != this.UI_FamilyInfoPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        /// <summary>
        /// Thêm gia tộc vào danh sách
        /// </summary>
        /// <param name="familyInfo"></param>
        private void AddFamily(FamilyInfo familyInfo)
        {
            UIFamily_UIFamilyList_FamilyInfo uiGuild = GameObject.Instantiate<UIFamily_UIFamilyList_FamilyInfo>(this.UI_FamilyInfoPrefab);
            uiGuild.transform.SetParent(this.transformFamilyList, false);
            uiGuild.transform.gameObject.SetActive(true);
            uiGuild.Data = familyInfo;

            uiGuild.Select = () => {
                this.ToggleGuild_Selected(familyInfo);
            };
        }
        /// <summary>
        /// Làm mới danh sách Gia tộc
        /// </summary>
        private void Refresh()
        {
            /// Xóa danh sách thành viên
            this.ClearMemberList();

            /// Thiết lập Text điều kiện vào tộc
            this.UIText_FamilyAskToJoinRequirement.text = "";
            /// Xây lại giao diện
            this.RebuildLayout(this.transformRequirementText);
            /// Ẩn Button chức năng
            this.UIButton_AskToJoin.gameObject.SetActive(false);

            /// Nếu không có dữ liệu
            if (Data == null)
            {
                return;
            }

            /// Duyệt danh sách gia tộc
            foreach (FamilyInfo familyInfo in this.Data)
            {
                this.AddFamily(familyInfo);
            }

            /// Nếu có tộc
            if (this.Data.Count > 0)
			{
                /// Mặc định chọn tộc đầu tiên
                this.selectedFamily = this.Data.FirstOrDefault();
                /// Thiết lập Text điều kiện vào tộc
                this.UIText_FamilyAskToJoinRequirement.text = this.selectedFamily.RequestNotify;
                /// Xây lại giao diện
                this.RebuildLayout(this.transformRequirementText);
                /// Nếu bản thân chưa có gia tộc
                if (Global.Data.RoleData.FamilyID <= 0)
                {
                    /// Kích hoạt Button gia nhập
                    this.UIButton_AskToJoin.gameObject.SetActive(true);
                }
            }

            /// Xây lại giao diện
            this.RebuildLayout(this.transformFamilyList);
        }
        #endregion
    }
}