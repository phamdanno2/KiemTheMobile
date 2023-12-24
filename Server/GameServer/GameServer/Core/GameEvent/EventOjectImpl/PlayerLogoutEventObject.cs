using GameServer.KiemThe.Entities;
using GameServer.KiemThe.LuaSystem.Entities;
using GameServer.KiemThe.LuaSystem;
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
            if (GameManager.MapMgr.DictMaps.TryGetValue(player.MapCode, out GameMap map))
            {
                Lua_Scene luaScene = new Lua_Scene()
                {
                    RefObject = map,
                };
                Lua_Player luaPlayer = new Lua_Player()
                {
                    RefObject = player,
                    CurrentScene = luaScene,
                };

                KTLuaScript.Instance.ExecuteFunctionAsync(KTLuaEnvironment.LuaEnv, KiemThe.KTGlobal.DefaultPlayerScriptID, "OnLogout", new object[] {
                    luaScene, luaPlayer}, null);
            }
        }

        public KPlayer getPlayer()
        {
            return this.player;
        }
    }
}