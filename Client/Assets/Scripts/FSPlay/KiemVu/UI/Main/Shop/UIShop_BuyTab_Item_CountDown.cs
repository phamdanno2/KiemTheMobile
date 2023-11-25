﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;

namespace FSPlay.KiemVu.UI.Main.Shop
{
    /// <summary>
    /// Đồng hồ đếm lùi thời hạn vật phẩm trong Tab mua vật phẩm
    /// </summary>
    public class UIShop_BuyTab_Item_CountDown : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text hiển thị thời gian đếm lùi
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Timer;
        #endregion

        #region Private fields

        #endregion

        #region Properties
        /// <summary>
        /// Hiển thị của đối tượng
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
            set
            {
                this.gameObject.SetActive(value);
                if (value)
                {
                    this.StopAllCoroutines();
                    this.StartCoroutine(this.CountDown());
                }
            }
        }

        /// <summary>
        /// Thời gian còn lại (đơn vị giây)
        /// </summary>
        private float timeLeft;
        /// <summary>
        /// Thời gian còn lại (đơn vị Milis)
        /// </summary>
        public long TickTime
        {
            set
            {
                this.timeLeft = value / 1000f;
            }
        }

        /// <summary>
        /// Tự ẩn khi hết thời gian
        /// </summary>
        public bool InvisibleWhenTimeout { get; set; }

        /// <summary>
        /// Sự kiện khi hết thời gian
        /// </summary>
        public Action Timeout { get; set; }
        #endregion

        #region Core MonoBehavour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.StopAllCoroutines();
            this.StartCoroutine(this.CountDown());
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {

        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực hiện đếm lùi
        /// </summary>
        /// <returns></returns>
        private IEnumerator CountDown()
        {
            while (true)
            {
                /// Nếu đã hết thời gian
                if (this.timeLeft <= 0)
                {
                    /// Thực hiện sự kiện hết thời gian
                    this.Timeout?.Invoke();
                    /// Ẩn khung
                    if (this.InvisibleWhenTimeout)
                    {
                        this.Visible = false;
                    }
                    /// Thoát
                    yield break;
                }

                /// Giảm thời gian
                this.timeLeft--;
                /// Hiển thị thời gian lên Text
                this.UIText_Timer.text = KTGlobal.DisplayTime(this.timeLeft);

                /// Đợi 1 giây
                yield return new WaitForSeconds(1f);
            }
        }
        #endregion

        #region Public methods

        #endregion
    }
}
