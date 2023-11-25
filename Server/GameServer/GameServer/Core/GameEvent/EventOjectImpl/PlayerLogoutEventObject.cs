using GameServer.Logic;

namespace GameServer.Core.GameEvent.EventOjectImpl
{
    /// <summary>
    /// 玩家退出事件
    /// </summary>
    public class PlayerLogoutEventObject : EventObject
    {
        private KPlayer player;

        public PlayerLogoutEventObject(KPlayer player)
            : base((int)EventTypes.PlayerLogout)
        {
            this.player = player;
        }

        public KPlayer getPlayer()
        {
            return this.player;
        }
    }
}