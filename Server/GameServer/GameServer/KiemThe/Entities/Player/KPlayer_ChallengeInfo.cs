using GameServer.Logic;
using System;
using System.Collections.Generic;

namespace GameServer.KiemThe.Entities.Player
{
    /// <summary>
    /// Thông tin tỷ thí của người chơi tương ứng
    /// </summary>
    public class KPlayer_ChallengeInfo
    {
        /// <summary>
        /// Đối tượng tỷ thí
        /// </summary>
        public KPlayer Target { get; set; }

        /// <summary>
        /// Thời gian bắt đầu tỷ thí
        /// </summary>
        public long StartTick { get; set; }

        /// <summary>
        /// Tổng thời gian tỷ thí
        /// </summary>
        public long ChallengeDuration { get; set; }

        /// <summary>
        /// Có phải đang trong thời gian tỷ thí không
        /// </summary>
        public bool IsChallengeTime
        {
            get
            {
                return KTGlobal.GetCurrentTimeMilis() - this.StartTick >= this.ChallengeDuration;
            }
        }
    }
}
