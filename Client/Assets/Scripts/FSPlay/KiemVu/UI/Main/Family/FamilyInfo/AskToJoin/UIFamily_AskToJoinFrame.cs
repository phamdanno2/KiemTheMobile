using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Utilities.UnityUI;
using KiemVu.UI.Main.Family;
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
    /// Khung xin gia nhập gia tộc
    /// </summary>
    public class UIFamily_AskToJoinFrame : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button xác nhận thêm thành viên
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Accept;

        /// <summary>
        /// Button từ chối thêm thành viên
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Refuse;

        /// <summary>
        /// Button trở lại
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Back;

        /// <summary>
        /// Prefab thông tin người chơi
        /// </summary>
        [SerializeField]
        private UIFamily_AskToJoinFrame_PlayerInfo UI_PlayerInfoPrefab;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách người chơi
        /// </summary>
        private RectTransform transformPlayersList = null;

        /// <summary>
        /// Người chơi được chọn
        /// </summary>
        private RequestJoin selectedPlayer = null;
        #endregion

        #region Properties
        /// <summary>
		/// Sự kiện đồng ý
		/// </summary>
		public Action<int> Accept { get; set; }

        /// <summary>
        /// Sự kiện từ chối
        /// </summary>
        public Action<int> Refuse { get; set; }

        /// <summary>
        /// Sự kiện quay trở lại
        /// </summary>
        public Action Back { get; set; }

        /// <summary>
        /// Danh sách người chơi xin gia nhập
        /// </summary>
        public List<RequestJoin> Data { get; set; }
        #endregion

        #region Core MonoBehaviour

        /// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
        {
            this.transformPlayersList = this.UI_PlayerInfoPrefab.transform.parent.GetComponent<RectTransform>();
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
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Back.onClick.AddListener(this.ButtonBack_Clicked);
            this.UIButton_Accept.onClick.AddListener(this.ButtonAccept_Clicked);
            this.UIButton_Refuse.onClick.AddListener(this.ButtonRefuse_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button quay trở lại được ấn
        /// </summary>
        private void ButtonBack_Clicked()
		{
            this.Hide();
            this.Back?.Invoke();
		}

        /// <summary>
		/// Sự kiện khi Button xác nhận được ấn
		/// </summary>
		private void ButtonAccept_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null || this.Data.Count <= 0)
			{
                KTGlobal.AddNotification("Không có dữ liệu!");
                return;
			}
            /// Nếu chưa chọn người chơi
            else if (this.selectedPlayer == null)
            {
                KTGlobal.AddNotification("Hãy chọn người chơi!");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
			{
                KTGlobal.AddNotification("Chỉ tộc trưởng hoặc tộc phó mới có quyền thao tác!");
                return;
            }

            /// Xác nhận
            KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận cho người chơi <color=#0ac6ff>[{0}]</color> gia nhập gia tộc?", this.selectedPlayer.RoleName), () => {
                /// Thực thi sự kiện đồng ý
                this.Accept?.Invoke(this.selectedPlayer.RoleID);
                /// Hủy Button chức năng
                this.UIButton_Accept.gameObject.SetActive(false);
                this.UIButton_Refuse.gameObject.SetActive(false);
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Button từ chối được ấn
        /// </summary>
        private void ButtonRefuse_Clicked()
        {
            /// Nếu không có dữ liệu
            if (this.Data == null || this.Data.Count <= 0)
            {
                KTGlobal.AddNotification("Không có dữ liệu!");
                return;
            }
            /// Nếu chưa chọn người chơi
            else if (this.selectedPlayer == null)
            {
                KTGlobal.AddNotification("Hãy chọn người chơi!");
                return;
            }
            /// Nếu không phải tộc trưởng hoặc tộc phó
            else if (Global.Data.RoleData.FamilyRank != (int) FamilyRank.Master && Global.Data.RoleData.FamilyRank != (int) FamilyRank.ViceMaster)
            {
                KTGlobal.AddNotification("Chỉ tộc trưởng hoặc tộc phó mới có quyền thao tác!");
                return;
            }

            KTGlobal.ShowMessageBox("Thông báo", string.Format("Xác nhận từ chối người chơi <color=#0ac6ff>[{0}]</color> gia nhập gia tộc?", this.selectedPlayer.RoleName), () => {
                /// Thực thi sự kiện từ chối
                this.Refuse?.Invoke(this.selectedPlayer.RoleID);
                /// Hủy Button chức năng
                this.UIButton_Accept.interactable = false;
                this.UIButton_Refuse.interactable = false;
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Button người chơi được ấn
        /// </summary>
        /// <param name="rd"></param>
        private void ButtonPlayer_Clicked(RequestJoin rd)
        {
            this.selectedPlayer = rd;
            this.UIButton_Accept.interactable = true;
            this.UIButton_Refuse.interactable = true;
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
            /// Nếu đối tượng chưa được kích hoạt thì thôi
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            /// Thực hiện xây lại giao diện ở Frame tiếp theo
            this.StartCoroutine(this.ExecuteSkipFrames(1, () =>
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformPlayersList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách người chơi
        /// </summary>
        private void ClearPlayersList()
        {
            foreach (Transform child in this.transformPlayersList.transform)
            {
                if (child.gameObject != this.UI_PlayerInfoPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Thêm người chơi tương ứng vào danh sách
        /// </summary>
        /// <param name="rd"></param>
        private void AddPlayer(RequestJoin rd)
        {
            UIFamily_AskToJoinFrame_PlayerInfo uiPlayerInfo = GameObject.Instantiate<UIFamily_AskToJoinFrame_PlayerInfo>(this.UI_PlayerInfoPrefab);
            uiPlayerInfo.transform.SetParent(this.transformPlayersList, false);
            uiPlayerInfo.gameObject.SetActive(true);
            uiPlayerInfo.Data = rd;
            uiPlayerInfo.Select = () => {
                this.ButtonPlayer_Clicked(rd);
            };
        }

        /// <summary>
        /// Tìm vị trí người chơi tương ứng trong danh sách
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private UIFamily_AskToJoinFrame_PlayerInfo FindPlayer(int roleID)
        {
            foreach (Transform child in this.transformPlayersList.transform)
            {
                if (child.gameObject != this.UI_PlayerInfoPrefab.gameObject)
                {
                    UIFamily_AskToJoinFrame_PlayerInfo uiPlayerInfo = child.GetComponent<UIFamily_AskToJoinFrame_PlayerInfo>();
                    if (uiPlayerInfo.Data.RoleID == roleID)
                    {
                        return uiPlayerInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Xóa người chơi tương ứng khỏi danh sách
        /// </summary>
        /// <param name="rd"></param>
        private void RemovePlayer(int roleID)
        {
            /// Vị trí người chơi
            UIFamily_AskToJoinFrame_PlayerInfo uiPlayerInfo = this.FindPlayer(roleID);
            /// Nếu không tồn tại thì bỏ qua
            if (uiPlayerInfo == null)
            {
                return;
            }
            /// Thực hiện xóa khỏi danh sách
            GameObject.Destroy(uiPlayerInfo.gameObject);

            /// Nếu còn người chơi trong danh sách chờ
            if (this.Data.Count > 0)
			{
                /// Mặc định chọn thằng đầu tiên
                this.selectedPlayer = this.Data.FirstOrDefault();
                /// Mở Button chức năng
                this.UIButton_Accept.interactable = true;
                this.UIButton_Refuse.interactable = true;
            }
        }

        /// <summary>
		/// Làm mới dữ liệu khung
		/// </summary>
		private void Refresh()
        {
            /// Xóa danh sách người chơi
            this.ClearPlayersList();

            /// Hủy Button chức năng
            this.UIButton_Accept.gameObject.SetActive(false);
            this.UIButton_Refuse.gameObject.SetActive(false);
            /// Xóa thằng đang được chọn
            this.selectedPlayer = null;

            /// Nếu không có dữ liệu
            if (this.Data == null)
			{
                return;
			}

            /// Duyệt danh sách
            foreach (RequestJoin playerInfo in this.Data)
			{
                /// Thêm người chơi vào danh sách
                this.AddPlayer(playerInfo);
			}

            /// Nếu có người chơi trong danh sách chờ
            if (this.Data.Count > 0)
			{
                /// Mặc định chọn thằng đầu tiên
                this.selectedPlayer = this.Data.FirstOrDefault();
                /// Mở Button chức năng
                this.UIButton_Accept.interactable = true;
                this.UIButton_Refuse.interactable = true;
            }


            /// Nếu là tộc trưởng hoặc tộc phó
            if (Global.Data.RoleData.FamilyRank == (int) FamilyRank.Master || Global.Data.RoleData.FamilyRank == (int) FamilyRank.ViceMaster)
            {
                /// Mở Button chức năng
                this.UIButton_Accept.gameObject.SetActive(true);
                this.UIButton_Refuse.gameObject.SetActive(true);
            }

            /// Xây lại giao diện
            this.RebuildLayout();
        }
        #endregion

        #region Public Method

        /// <summary>
		/// Xóa người chơi khỏi danh sách
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
        /// Thêm người chơi vào danh sách
        /// </summary>
        /// <param name="rd"></param>
        public void Add(RequestJoin rd)
        {
            /// Thông tin cũ
            UIFamily_AskToJoinFrame_PlayerInfo uiPlayerInfo = this.FindPlayer(rd.RoleID);
            /// Nếu đã tồn tại thì cập nhật thay đổi
            if (uiPlayerInfo == null)
            {
                uiPlayerInfo.Data = rd;
                return;
            }
            /// Tiến hành thêm vào danh sách
            this.AddPlayer(rd);
        }

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
        #endregion
    }
}