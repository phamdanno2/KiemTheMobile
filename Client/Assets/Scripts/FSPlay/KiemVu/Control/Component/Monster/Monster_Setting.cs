using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Thiết lập hệ thống
    /// </summary>
    public partial class Monster
    {
        /// <summary>
        /// Thực thi nếu có thiết lập
        /// </summary>
        public void ExecuteSetting()
        {
            if (KTSystemSetting.SkillVolume != this.lastVolume)
            {
                this.audioPlayer.Volume = KTSystemSetting.SkillVolume / 100f;
                this.lastVolume = KTSystemSetting.SkillVolume;
            }

            if (this.lastHideRole != KTSystemSetting.HideNPC)
            {
                this.lastHideRole = KTSystemSetting.HideNPC;
                if (!this.lastHideRole)
                {
                    this.ResumeActions();
                    this.SetModelVisible(true);

                    this.ResumeCurrentAction();

                    this.uiHeaderOffsetChanged = false;
                }
                else
                {
                    this.PauseAllActions();
                    this.SetModelVisible(false);

                    this.StartCoroutine(ExecuteSkipFrames(1, () => {
                        this.UIHeaderOffset = new Vector2(0, 100);
                        this.UIHeader.gameObject.SetActive(true);
                    }));
                }
            }

            if (this.RefObject != Global.Data.Leader && this.UIHeader != null)
            {
                this.UIHeader.SystemSettingShowHPBar = !KTSystemSetting.HideOtherHPBar;
                this.UIHeader.SystemSettingShowName = !KTSystemSetting.HideOtherName;
            }
        }
    }
}
