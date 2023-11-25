using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic.BackgroundWork;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;

namespace FSPlay.KiemVu.Logic.Settings
{
    /// <summary>
    /// Thực thi thiết lập hệ thống
    /// </summary>
    public static partial class KTSystemSetting
    {
        /// <summary>
        /// Thực thi thiết lập hệ thống
        /// </summary>
        public static void Apply()
        {
            /// Nếu thiết lập hiệu ứng ẩn toàn bộ
            if (KTSystemSetting.EffectQualitySetting == 0)
            {
                Global.RenderPipeline2D.SetActive(false);
                Global.MainCamera.GetComponent<PostProcessLayer>().enabled = false;
                Global.MainCamera.GetComponent<MobilePostProcessing>().enabled = false;
            }
            /// Nếu thiết lập hiệu ứng tầm thấp
            else if (KTSystemSetting.EffectQualitySetting == 1)
            {
                Global.RenderPipeline2D.SetActive(false);
                Global.MainCamera.GetComponent<PostProcessLayer>().enabled = false;
                Global.MainCamera.GetComponent<MobilePostProcessing>().enabled = true;
            }
            /// Nếu thiết lập hiệu ứng tầm trung
            else if (KTSystemSetting.EffectQualitySetting == 2)
            {
                Global.RenderPipeline2D.SetActive(true);
                Global.MainCamera.GetComponent<PostProcessLayer>().enabled = true;
                Global.RenderPipeline2D.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>().diffusion.overrideState = true;
                Global.MainCamera.GetComponent<MobilePostProcessing>().enabled = false;
            }
            /// Nếu thiết lập hiệu ứng tầm cao
            else
            {
                Global.RenderPipeline2D.SetActive(true);
                Global.MainCamera.GetComponent<PostProcessLayer>().enabled = true;
                Global.RenderPipeline2D.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>().diffusion.overrideState = false;
                Global.MainCamera.GetComponent<MobilePostProcessing>().enabled = false;
            }

            /// Cập nhật tầm nhìn Camera
            Global.MainCamera.orthographicSize = KTSystemSetting.FieldOfView;

            /// Cập nhật lên toàn bộ đối tượng động
            KTBackgroundWorkManager.Instance.ExecuteSystemSettingOnGameObjects();
        }
    }
}
