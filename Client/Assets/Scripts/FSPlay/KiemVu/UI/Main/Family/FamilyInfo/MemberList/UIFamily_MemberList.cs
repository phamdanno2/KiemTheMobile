using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// Khung danh sách thành viên gia tộc
    /// </summary>
    public class UIFamily_MemberList : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Prefab thông tin thành viên
        /// </summary>
        [SerializeField]
        private UIFamily_MemberList_MemberInfo UI_MemberInfoPrefab;

        /// <summary>
        /// Button bổ nhiệm
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Approve;

        /// <summary>
        /// Khung Bổ nhiệm
        /// </summary>
        [SerializeField]
        private UIFamily_MemberList_ApproveFrame UIApproveMemberFrame;

        /// <summary>
        /// Button khai trừ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_KickOut;

        /// <summary>
        /// Button Rời tộc
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_QuitFamily;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách thành viên
        /// </summary>
        private RectTransform transformMemberList = null;

        /// <summary>
        /// Thành viên đang được chọn
        /// </summary>
        private FamilyMember selectedMember = null;
        #endregion

        #region Properties
        /// <summary>
        /// Danh sách thành viên
        /// </summary>
        public List<FamilyMember> MembersList { get; set; }

        /// <summary>
        /// sự kiện thay đổi chức vụ
        /// </summary>
        public Action<FamilyMember, int> Approve { get; set; }

        /// <summary>
        /// sự kiện Khai trừ ra khỏi tộc
        /// </summary>
        public Action<FamilyMember> KickOut { get; set; }

        /// <summary>
        /// Sự kiện rời tộc
        /// </summary>
        public Action QuitFamily { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformMemberList = this.UI_MemberInfoPrefab.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này được ở frame đầu tiền
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.Refresh();
        }
        #endregion

        #region Code UI

        private void InitPrefabs()
        {
            this.UIButton_Approve.onClick.AddListener(this.ButtonApprove_Clicked);
            this.UIButton_QuitFamily.onClick.AddListener(this.ButtonQuit_Clicked);
            this.UIButton_KickOut.onClick.AddListener(this.ButtonKickOut_Clicked);

            /// Nếu bản thân là tộc trưởng thì không cho thoát
            if (Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master)
			{
                this.UIButton_QuitFamily.gameObject.SetActive(false);
			}
        }

        /// <summary>
        /// Sự kiện đổi chức cho thành viên
        /// </summary>
        private void ButtonApprove_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.MembersList == null || this.MembersList.Count <= 0)
            {
                KTGlobal.AddNotification("Không có dữ liệu!");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
            {
                KTGlobal.AddNotification("Chỉ có tộc trưởng hoặc tộc phó mới có quyền thao tác!");
                return;
            }
            /// Nếu không có thằng nào được chọn
            else if (this.selectedMember == null)
			{
                KTGlobal.AddNotification("Hãy chọn một thành viên!");
                return;
			}
            /// Nếu chọn bản thân
            else if (this.selectedMember.RoleID == Global.Data.RoleData.RoleID)
            {
                KTGlobal.AddNotification("Không thể thao tác với chính mình!");
                return;
            }
            /// Nếu bản thân là tộc phó mà thằng được chọn lại là tộc trưởng
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster && this.selectedMember.Rank == (int) FamilyRank.Master)
            {
                KTGlobal.AddNotification("Không thể thao tác với tộc trưởng");
                return;
            }

            /// Mở khung bổ nhiệm thành viên
            this.UIApproveMemberFrame.Select = (rankID) => {
                /// Xác nhận
                KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận bổ nhiệm <color=#0ac6ff>[{0}]</color> thành <color=orange>{1}</color>?", this.selectedMember.RoleName, KTGlobal.GetFamilyRankName(rankID)), () => {
                    /// Đóng khung
                    this.UIApproveMemberFrame.Hide();
                    /// Thực thi sự kiện bổ nhiệm thành viên
                    this.Approve?.Invoke(this.selectedMember, rankID);
                }, true);
            };
            /// Danh sách chức vụ
            List<int> ranks = new List<int>();
            /// Nếu bản thân là tộc trưởng
            if (Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master)
			{
                ranks.Add((int) FamilyRank.Master);
                ranks.Add((int) FamilyRank.ViceMaster);
            }
            ranks.Add((int) FamilyRank.Member);
            this.UIApproveMemberFrame.RankList = ranks;
            this.UIApproveMemberFrame.Show();
        }

        /// <summary>
        /// Sự kiện thoát tộc
        /// </summary>
        private void ButtonQuit_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.MembersList == null || this.MembersList.Count <= 0)
            {
                KTGlobal.AddNotification("Không có dữ liệu!");
                return;
            }
            /// Nếu bản thân là tộc trưởng thì không cho thoát
            else if (Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master)
            {
                KTGlobal.AddNotification("Trưởng lão không thể chủ động thoát gia tộc!");
                return;
            }

            /// Xác nhận
            KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận thoát khỏi gia tộc?"), () => {
                /// Thực thi sự kiện trục xuất thành viên
                this.QuitFamily?.Invoke();
            }, true);
        }

        /// <summary>
        /// Sự kiện trục xuất thành viên
        /// </summary>
        private void ButtonKickOut_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.MembersList == null || this.MembersList.Count <= 0)
            {
                KTGlobal.AddNotification("Không có dữ liệu!");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
            {
                KTGlobal.AddNotification("Chỉ có tộc trưởng hoặc tộc phó mới có quyền thao tác!");
                return;
            }
            /// Nếu không có thằng nào được chọn
            else if (this.selectedMember == null)
            {
                KTGlobal.AddNotification("Hãy chọn một thành viên!");
                return;
            }
            /// Nếu chọn bản thân
            else if (this.selectedMember.RoleID == Global.Data.RoleData.RoleID)
            {
                KTGlobal.AddNotification("Không thể thao tác với chính mình!");
                return;
            }
            /// Nếu bản thân là tộc phó mà thằng được chọn lại là tộc trưởng
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster && this.selectedMember.Rank == (int) FamilyRank.Master)
            {
                KTGlobal.AddNotification("Không thể thao tác với tộc trưởng");
                return;
            }

            /// Xác nhận
            KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận trục xuất <color=#0ac6ff>[{0}]</color> khỏi gia tộc?", this.selectedMember.RoleName), () => {
                /// Thực thi sự kiện trục xuất thành viên
                this.KickOut?.Invoke(this.selectedMember);
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Toggle chọn thành viên trong danh sách được ấn
        /// </summary>
        /// <param name="memberInfo"></param>
        private void ToggleMember_Selected(FamilyMember memberInfo)
		{
            /// Nếu không có dữ liệu
            if (this.MembersList == null || this.MembersList.Count <= 0)
			{
                return;
			}
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
			{
                return;
			}
            /// Nếu chọn bản thân
            else if (memberInfo.RoleID == Global.Data.RoleData.RoleID)
			{
                return;
			}
            /// Nếu bản thân là tộc phó mà thằng được chọn lại là tộc trưởng
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster && memberInfo.Rank == (int) FamilyRank.Master)
			{
                return;
			}

            /// Thiết lập thành viên đang được chọn
            this.selectedMember = memberInfo;
            /// Hiển thị Button chức năng
            this.UIButton_Approve.gameObject.SetActive(true);
            this.UIButton_KickOut.gameObject.SetActive(true);
        }
        #endregion

        #region Private Method

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
            /// Nếu đối tượng chưa được kích hoạt thì bỏ qua
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            /// Thực hiện xây lại giao diện
            this.StartCoroutine(this.ExecuteSkipFrames(1, () =>
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformMemberList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách thành viên
        /// </summary>
        private void ClearMemberList()
        {
            foreach (Transform child in this.transformMemberList.transform)
            {
                if (child.gameObject != this.UI_MemberInfoPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Thêm thành viên vào danh sách
        /// </summary>
        /// <param name="guildInfo"></param>
        private void AddMember(FamilyMember memberInfo)
        {
            UIFamily_MemberList_MemberInfo uiMemberInfo = GameObject.Instantiate<UIFamily_MemberList_MemberInfo>(this.UI_MemberInfoPrefab);
            uiMemberInfo.transform.SetParent(this.transformMemberList, false);
            uiMemberInfo.transform.gameObject.SetActive(true);

            uiMemberInfo.Data = memberInfo;
            uiMemberInfo.Select = () => {
                this.ToggleMember_Selected(memberInfo);
            };
        }

        /// <summary>
        /// Tìm vị trí người chơi tương ứng trong danh sách
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private UIFamily_MemberList_MemberInfo FindMember(int roleID)
        {
            foreach (Transform child in this.transformMemberList.transform)
            {
                if (child.gameObject != this.UI_MemberInfoPrefab.gameObject)
                {
                    UIFamily_MemberList_MemberInfo uiPlayerInfo = child.GetComponent<UIFamily_MemberList_MemberInfo>();
                    if (uiPlayerInfo.Data.RoleID == roleID)
                    {
                        return uiPlayerInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Xóa thành viên tương ứng khỏi danh sách
        /// </summary>
        /// <param name="rd"></param>
        private void RemovePlayer(int roleID)
        {
            /// Vị trí người chơi
            UIFamily_MemberList_MemberInfo uiMemberInfo = this.FindMember(roleID);
            /// Nếu không tồn tại thì bỏ qua
            if (uiMemberInfo == null)
            {
                return;
            }
            /// Thực hiện xóa khỏi danh sách
            GameObject.Destroy(uiMemberInfo.gameObject);
        }

        /// <summary>
        /// Làm mới dữ liệu
        /// </summary>
        private void Refresh()
		{
            /// Xóa danh sách thành viên
            this.ClearMemberList();

            /// Hủy Button chức năng
            this.UIButton_Approve.gameObject.SetActive(false);
            this.UIButton_KickOut.gameObject.SetActive(false);

            /// Nếu dữ liệu rỗng
            if (this.MembersList == null || this.MembersList.Count <= 0)
			{
                return;
			}

            /// Duyệt danh sách thành viên
            foreach (FamilyMember memberInfo in this.MembersList)
			{
                /// Thêm thành viên vào danh sách
                this.AddMember(memberInfo);
			}

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
        }

        /// <summary>
        /// Ẩn khung
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Thêm thành viên vào danh sách
        /// </summary>
        /// <param name="rd"></param>
        public void Add(FamilyMember rd)
        {
            /// Thông tin cũ
            UIFamily_MemberList_MemberInfo uiPlayerInfo = this.FindMember(rd.RoleID);
            /// Nếu đã tồn tại thì cập nhật thay đổi
            if (uiPlayerInfo != null)
            {
                uiPlayerInfo.Data = rd;
                return;
            }
            /// Tiến hành thêm vào danh sách
            this.AddMember(rd);
        }

        /// <summary>
		/// Xóa thành viên khỏi danh sách
		/// </summary>
		/// <param name="roleID"></param>
		public void Remove(int roleID)
        {
            /// Thực hiện xóa
            this.RemovePlayer(roleID);
            /// Xây lại giao diện
            this.RebuildLayout();
        }

        /// <summary>
        /// Làm mới dữ liệu thành viên
        /// </summary>
        /// <param name="roleID"></param>
        public void RefreshMemberData(int roleID)
		{
            /// Thông tin cũ
            UIFamily_MemberList_MemberInfo uiPlayerInfo = this.FindMember(roleID);
            /// Nếu đã tồn tại thì cập nhật thay đổi
            if (uiPlayerInfo != null)
            {
                uiPlayerInfo.Refresh();
                return;
            }

            /// Nếu là bản thân
            if (roleID == Global.Data.RoleData.RoleID && Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
			{
                /// Hủy button chức năng
                this.UIButton_Approve.gameObject.SetActive(false);
                this.UIButton_KickOut.gameObject.SetActive(false);
			}
        }
        #endregion
    }
}