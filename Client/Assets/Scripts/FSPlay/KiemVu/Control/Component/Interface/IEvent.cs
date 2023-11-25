﻿using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Interface các sự kiện xảy ra với đối tượng
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Đối tượng được chọn
        /// </summary>
        void OnClick();
    }
}
