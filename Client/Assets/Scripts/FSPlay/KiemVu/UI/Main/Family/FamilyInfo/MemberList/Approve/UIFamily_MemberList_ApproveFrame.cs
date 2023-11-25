using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FSPlay.KiemVu.UI.Main.Family
{
    /// <summary>
    /// Khung thay đổi chức vụ
    /// </summary>
    public class UIFamily_MemberList_ApproveFrame : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Prefab chức vụ
        /// </summary>
        [SerializeField]
        private UIFamily_MemberList_ApproveFrame_RankInfo UI_RankInfoPrefab;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách chức vụ
        /// </summary>
        private RectTransform transformRankList = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện chọn chức vụ
        /// </summary>
        public Action<int> Select { get; set; }

        /// <summary>
        /// Danh sách chức vụ
        /// </summary>
        public List<int> RankList { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformRankList = this.UI_RankInfoPrefab.transform.parent.GetComponent<RectTransform>();
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
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Hide();
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
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformRankList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách chức vụ
        /// </summary>
        private void ClearRankList()
        {
            foreach (Transform child in this.transformRankList.transform)
            {
                if (child.gameObject != this.UI_RankInfoPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Làm mới danh sách chức vụ
        /// </summary>
        private void Refresh()
        {
            /// Làm rỗng danh sách
            this.ClearRankList();

            /// Nếu danh sách chức vụ rỗng
            if (this.RankList == null)
            {
                return;
            }

            /// Duyệt và thêm chức vụ tương ứng vào danh sách
            foreach (int rankID in this.RankList)
            {
                UIFamily_MemberList_ApproveFrame_RankInfo uiRankInfo = GameObject.Instantiate<UIFamily_MemberList_ApproveFrame_RankInfo>(this.UI_RankInfoPrefab);
                uiRankInfo.transform.SetParent(this.transformRankList, false);
                uiRankInfo.gameObject.SetActive(true);

                uiRankInfo.RankName = KTGlobal.GetFamilyRankName(rankID);
                uiRankInfo.Click = () => {
                    /// Ẩn khung
                    this.Hide();
                    /// Thực thi sự kiện
                    this.Select?.Invoke(rankID);
                };
            }

            /// Xây lại danh sách
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
        #endregion
    }
}
