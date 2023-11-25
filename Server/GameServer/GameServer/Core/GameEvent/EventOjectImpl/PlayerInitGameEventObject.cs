using GameServer.Logic;

namespace GameServer.Core.GameEvent.EventOjectImpl
{
    /// <summary>
    /// 玩家退出事件
    /// </summary>
    public class PlayerInitGameEventObject : EventObject
    {
        private KPlayer player;

        public PlayerInitGameEventObject(KPlayer player)
            : base((int)EventTypes.PlayerInitGame)
        {
            this.player = player;
        }

        public KPlayer getPlayer()
        {
            return this.player;
        }
    }
}