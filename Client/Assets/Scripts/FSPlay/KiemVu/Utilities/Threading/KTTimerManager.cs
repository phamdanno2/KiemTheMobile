using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Utilities.Threading
{
    /// <summary>
    /// Định nghĩa Timer thực thi trong hệ thống
    /// </summary>
    public class KTTimerManager : TTMonoBehaviour
    {
        #region Singleton - Instance
        /// <summary>
        /// Định nghĩa Timer thực thi trong hệ thống
        /// </summary>
        public static KTTimerManager Instance { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            KTTimerManager.Instance = this;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi Delay Task
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteDelayTask(float delay, Action work)
        {
            yield return new WaitForSeconds(delay);
            work?.Invoke();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Thực thi sự kiện sau khoảng tương ứng
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="work"></param>
        public void SetTimeout(float delay, Action work)
        {
            this.StartCoroutine(this.ExecuteDelayTask(delay, work));
        }
        #endregion
    }
}
