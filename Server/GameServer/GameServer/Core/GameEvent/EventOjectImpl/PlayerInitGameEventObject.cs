using GameServer.KiemThe.Entities;
using GameServer.KiemThe.LuaSystem.Entities;
using GameServer.KiemThe.LuaSystem;
using GameServer.Logic;
using GameServer.Server;
using MoonSharp.Interpreter;
using Server.Tools;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

namespace GameServer.Core.GameEvent.EventOjectImpl
{
    /// <summary>
    /// 玩家退出事件
    /// </summary>
    public class PlayerInitGameEventObject : EventObject
    {
        private KPlayer player;

        public PlayerInitGameEventObject(KPlayer player, bool isFirstLogin)
            : base((int)EventTypes.PlayerInitGame)
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

                KTLuaScript.Instance.ExecuteFunctionAsync(KTLuaEnvironment.LuaEnv, KiemThe.KTGlobal.DefaultPlayerScriptID, "OnLogin", new object[] {
                    luaScene, luaPlayer, isFirstLogin ? 1 : 0}, null);
            }
        }

        public KPlayer getPlayer()
        {
            return this.player;
        }
    }
}