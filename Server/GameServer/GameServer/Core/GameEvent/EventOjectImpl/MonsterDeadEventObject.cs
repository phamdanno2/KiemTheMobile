using GameServer.Logic;

namespace GameServer.Core.GameEvent.EventOjectImpl
{
    /// <summary>
    /// 怪物死亡事件(人打怪，怪死了)
    /// </summary>
    public class MonsterDeadEventObject : EventObject
    {
        private Monster monster;
        private KPlayer attacker;

        public MonsterDeadEventObject(Monster monster, KPlayer attacker)
            : base((int)EventTypes.MonsterDead)
        {
            this.monster = monster;
            this.attacker = attacker;
        }

        public Monster getMonster()
        {
            return monster;
        }

        public KPlayer getAttacker()
        {
            return attacker;
        }
    }
}