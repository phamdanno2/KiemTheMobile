using System;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.SpecialEvents
{
    /// <summary>
    /// Quản lý Button các sự kiện đặc biệt
    /// </summary>
    public class UISpecialEventButtons : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button mở bảng xếp hạng Tống Kim
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_SongJin_OpenRankingBoard;

        /// <summary>
        /// Button bảng xếp hạng môn phái
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_FactionBattle_OpenBoard;

        /// <summary>
        /// Button mở bảng thông tin Tranh đoạt lãnh thổ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ColonyDispute_OpenBoard;
        #endregion

        #region Properties
        /// <summary>
        /// Mở bảng xếp hạng Tống Kim
        /// </summary>
        public Action OpenSongJinRankingBoard { get; set; }

        /// <summary>
        /// Mở Bảng Xếp Hạng Thi đấu môn phái
        /// </summary>
        public Action OpenFactionBattleBoard { get; set; }

        /// <summary>
        /// Mở bảng thông tin Tranh đoạt lãnh thổ
        /// </summary>
        public Action OpenColonyDisputeBoard { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.HideAllButtons();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_SongJin_OpenRankingBoard.onClick.AddListener(() => {
                this.OpenSongJinRankingBoard?.Invoke();
            });
            this.UIButton_FactionBattle_OpenBoard.onClick.AddListener(() => {
                this.OpenFactionBattleBoard?.Invoke();
            });
            this.UIButton_ColonyDispute_OpenBoard.onClick.AddListener(() => {
                this.OpenColonyDisputeBoard?.Invoke();
            });
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiển thị Button ID tương ứng hoạt động
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetButtonState(int id, bool state)
        {
            switch (id)
            {
                /// Tống Kim
                case 20:
                {
                    this.UIButton_SongJin_OpenRankingBoard.gameObject.SetActive(state);
                    break;
                }
                /// Thi đấu môn phái
                case 21:
                {
                    this.UIButton_FactionBattle_OpenBoard.gameObject.SetActive(state);
                    break;
                }
                /// Tranh đoạt lãnh thổ
                case 50:
                {
                    this.UIButton_ColonyDispute_OpenBoard.gameObject.SetActive(state);
                    break;
                }
            }
        }

        /// <summary>
        /// Ẩn toàn bộ Button
        /// </summary>
        public void HideAllButtons()
        {
            this.UIButton_SongJin_OpenRankingBoard.gameObject.SetActive(false);
            this.UIButton_FactionBattle_OpenBoard.gameObject.SetActive(false);
            this.UIButton_ColonyDispute_OpenBoard.gameObject.SetActive(false);
        }
        #endregion
    }
}
