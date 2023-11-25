using System;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý các công việc ngầm
	/// </summary>
	public partial class GScene
    {
        #region Ping đồng bộ với máy chủ

        private float LastProcessTicks = 0f; //上次发送Check指令的进程时间
        private long LastDateTimeTicks = 0; //上次发送Check指令的机器时间
        private int TimeSyncCounter = 0;   //时间同步计数器

        /// <summary>
        /// Ping đồng bộ với máy chủ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PingTimeTick(object sender, EventArgs e)
        {
            //角色是否已经正式登陆
            if (!Global.Data.PlayGame)
            {
                return;
            }

            int processSubTicks = LastProcessTicks > 0 ? (int)((UnityEngine.Time.realtimeSinceStartup - LastProcessTicks) * 1000.0f) : 0;
            int dateTimeSubTicks = LastDateTimeTicks > 0 ? (int)(DateTime.Now.Ticks / 10000 - LastDateTimeTicks) : 0;

            LastProcessTicks = UnityEngine.Time.realtimeSinceStartup;
            LastDateTimeTicks = DateTime.Now.Ticks / 10000;

            //加速效验
            GameInstance.Game.SpriteCheck(processSubTicks, dateTimeSubTicks);

            if (!GameInstance.Game.ActiveDisconnect)
            {
                //判断ping的情况
                BasePlayZone.InWaitPingCount++;
                if (BasePlayZone.InWaitPingCount * 2000 > BasePlayZone.PING_TIMEOUT)   //超时情况
                {
                    KTDebug.Log("GameInstance.Game.PingTimeOut()");
                    GameInstance.Game.PingTimeOut();
                    return;
                }
            }

            TimeSyncCounter++;
            if (TimeSyncCounter >= 60) // 暂定两分钟发送一次 
            {
                TimeSyncCounter = 0;
                GameInstance.Game.TimeSynchronizationByClient();
            }
        }

        #endregion

        #region Đồng bộ vị trí lên GS
        /// <summary>
        /// Đồng bộ vị trí hiện tại của Leader lên Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeaderMovingTick(object sender, EventArgs e)
        {
            if (!this.EnableChangMap)
            {
                return;
            }

            if (!Global.Data.PlayGame)
            {
                return;
            }

            if (null != this.Leader)
            {
                GameInstance.Game.SpritePosition();
            }
        }
        #endregion

        #region Sự kiện Heart
        /// <summary>
        /// Sự kiện Heart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClientHeartTimer_Tick(object sender, EventArgs e)
        {
            this.SendClientHeart();
        }
        #endregion
    }
}
