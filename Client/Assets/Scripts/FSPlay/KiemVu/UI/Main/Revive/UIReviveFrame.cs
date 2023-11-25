using System;
using UnityEngine;
using TMPro;
using System.Collections;

namespace FSPlay.KiemVu.UI.Main.Revive
{
    /// <summary>
    /// Khung hồi sinh
    /// </summary>
    public class UIReviveFrame : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text nội dung
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Message;

        /// <summary>
        /// Button sử dụng Cửu Chuyển Tục Mệnh Hoàn
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_UseRevivePill;

        /// <summary>
        /// Button hồi sinh tại chỗ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ReviveAtPos;

        /// <summary>
        /// Button về thành
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_BackToCity;

        /// <summary>
        /// Text thời gian đếm lui
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_CountDown;
        #endregion

        #region Properties
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Message
        {
            get
            {
                return this.UIText_Message.text;
            }
            set
            {
                this.UIText_Message.text = value;
            }
        }

        /// <summary>
        /// Cho phép hồi sinh tại chỗ
        /// </summary>
        public bool AllowReviveAtPos
        {
            get
            {
                return this.UIButton_ReviveAtPos.interactable;
            }
            set
            {
                this.UIButton_ReviveAtPos.interactable = value;
            }
        }

        /// <summary>
        /// Thời gian đếm lùi trước khi tự về thành
        /// </summary>
        public const float CountDownTime = 300;

        /// <summary>
        /// Sự kiện khi Button dùng Cửu Chuyển Tục Mệnh Hoàn được ấn
        /// </summary>
        public Action UseRevivePill { get; set; }

        /// <summary>
        /// Sự kiện khi Button hồi sinh tại chỗ được ấn
        /// </summary>
        public Action ReviveAtPos { get; set; }

        /// <summary>
        /// Sự kiện khi Button về thành được ấn
        /// </summary>
        public Action BackToCity { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.StartCountDown();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_UseRevivePill.onClick.AddListener(this.ButtonUseRevivePill_Clicked);
            this.UIButton_ReviveAtPos.onClick.AddListener(this.ButtonReviveAtPos_Clicked);
            this.UIButton_BackToCity.onClick.AddListener(this.ButtonBackToCity_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button sử dụng Cửu Chuyển Tục Mệnh Hoàn được ấn
        /// </summary>
        private void ButtonUseRevivePill_Clicked()
        {
            this.UseRevivePill?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button hồi sinh tại chỗ được ấn
        /// </summary>
        private void ButtonReviveAtPos_Clicked()
        {
            this.ReviveAtPos?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button về thành được ấn
        /// </summary>
        private void ButtonBackToCity_Clicked()
        {
            this.BackToCity?.Invoke();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực hiện đếm lùi trước khi mặc định chọn chức năng tự về thành
        /// </summary>
        private void StartCountDown()
        {
            IEnumerator CountDown()
            {
                float lifeTime = UIReviveFrame.CountDownTime;
                this.UIText_CountDown.text = KTGlobal.DisplayTime(lifeTime);
                yield return new WaitForSeconds(1f);
                while (lifeTime > 0)
                {
                    lifeTime -= 1f;
                    this.UIText_CountDown.text = KTGlobal.DisplayTime(lifeTime);
                    yield return new WaitForSeconds(1f);
                }
                lifeTime = 0f;
                this.UIText_CountDown.text = KTGlobal.DisplayTime(lifeTime);

                this.BackToCity?.Invoke();
            }
            this.StartCoroutine(CountDown());
        }
        #endregion
    }
}
