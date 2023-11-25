using GameServer.Logic;

namespace GameServer.Core.GameEvent.EventOjectImpl
{
    /// <summary>
    /// 玩家升级事件
    /// </summary>
    public class PlayerLevelupEventObject : EventObject
    {
        private KPlayer player;

        public PlayerLevelupEventObject(KPlayer player)
            : base((int)EventTypes.PlayerLevelup)
        {
            this.player = player;
        }

        public KPlayer Player
        {
            get { return this.player; }
        }
    }
}