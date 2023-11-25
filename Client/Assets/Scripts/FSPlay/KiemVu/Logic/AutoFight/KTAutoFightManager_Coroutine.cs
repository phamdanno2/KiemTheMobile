using FSPlay.KiemVu.Logic.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Luồng thực thi tự đánh
    /// </summary>
    public partial class KTAutoFightManager
    {
        /// <summary>
        /// Luồng thực thi tự ăn thức ăn và thuốc
        /// </summary>
        /// <returns></returns>
        private IEnumerator AutoUseFoodAndMedicine()
		{
            /// Lặp liên tục
            while (true)
			{
               
                /// Tự dùng thức ăn nếu có thiết lập
                this.AutoUseFood();
                /// Tự dùng thuốc nếu có thiết lập
                this.AutoUseMedicine();
                /// Nghỉ giãn cách
                yield return new WaitForSeconds(1f);
			}
		}
	}
}
