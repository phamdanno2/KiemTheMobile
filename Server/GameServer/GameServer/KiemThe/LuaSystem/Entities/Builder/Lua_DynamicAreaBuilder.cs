using GameServer.KiemThe.Core;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.LuaSystem.Entities.Math;
using GameServer.Logic;
using MoonSharp.Interpreter;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.LuaSystem.Entities.Builder
{
    /// <summary>
    /// Đối tượng Builder dùng cho Lua để xây khu vực động
    /// </summary>
    [MoonSharpUserData]
    public class Lua_DynamicAreaBuilder
    {
        /// <summary>
        /// Bản đồ đang đứng
        /// </summary>
        [MoonSharpHidden]
        public Lua_Scene Scene { get; set; }

        /// <summary>
        /// ID Res
        /// </summary>
        [MoonSharpHidden]
        public int ResID { get; set; }

        /// <summary>
        /// Vị trí đang đứng
        /// </summary>
        [MoonSharpHidden]
        public Lua_Vector2 Pos { get; set; }

        /// <summary>
        /// Tên quái
        /// <para>Bỏ trống sẽ lấy tên ở file cấu hình</para>
        /// </summary>
        [MoonSharpHidden]
        public string Name { get; set; } = "";

        /// <summary>
        /// Bán kính quét
        /// </summary>
        [MoonSharpHidden]
        public int Radius { get; set; }

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// Thời gian tồn tại
        /// </summary>
        [MoonSharpHidden]
        public float LifeTime { get; set; } = -1;

        /// <summary>
        /// Thời gian Tick
        /// </summary>
        [MoonSharpHidden]
        public float Tick { get; set; } = 1f;

        /// <summary>
        /// Tag
        /// </summary>
        [MoonSharpHidden]
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện khi hoàn tất tạo quái
        /// </summary>
        [MoonSharpHidden]
        public Action<KDynamicArea> Done { get; set; }

        /*
        /// <summary>
        /// Thiết lập bản đồ đang đứng tương ứng
        /// </summary>
        /// <param name="scene"></param>
        public Lua_DynamicAreaBuilder SetScene(Lua_Scene scene)
        {
            /// Nếu bản đồ không tồn tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_DynamicAreaBuilder:SetScene, scene is not exist!"));
                return this;
            }

            this.Scene = scene;
            return this;
        }
        */

        /// <summary>
        /// Thiết lập ResID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetResID(int id)
        {
            this.ResID = id;
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetPos(int posX, int posY)
        {
            this.Pos = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetPos(Lua_Vector2 pos)
        {
            this.Pos = pos;
            return this;
        }

        /// <summary>
        /// Thiết lập tên
        /// <para>Bỏ trống sẽ lấy tên ở file cấu hình</para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Thiết lập bán kính quét
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetRadius(int radius)
        {
            this.Radius = radius;
            return this;
        }

        /// <summary>
        /// Thiết lập ScriptID
        /// <para>-1 nếu không có Script</para>
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetScriptID(int scriptID)
        {
            this.ScriptID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập thời gian tồn tại
        /// </summary>
        /// <param name="lifeTime"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetLifeTime(float lifeTime)
        {
            this.LifeTime = lifeTime;
            return this;
        }

        /// <summary>
        /// Thiết lập thời gian Tick
        /// </summary>
        /// <param name="lifeTime"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetTick(float tick)
        {
            this.Tick = tick;
            return this;
        }

        /// <summary>
        /// Thiết lập Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetTag(string tag)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        /// Thiết lập thực thi sự kiện khi hoàn tất
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_DynamicAreaBuilder SetDone(Closure func)
        {
            this.Done = (dynArea) => {
                object[] parameters = new object[]
                {
                    new Lua_DynamicArea()
                    {
                        CurrentScene = this.Scene,
                        RefObject = dynArea,
                    }
                };
                KTLuaScript.Instance.ExecuteFunctionAsync("DynamicAreaBuilder:Done", func, parameters, null);
            };
            return this;
        }

        /// <summary>
        /// Tạo khu vực động tương ứng
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            /// Nếu bản đồ không tồn tại
            if (this.Scene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_DynamicAreaBuilder:Build, scene is NIL!"));
                return false;
            }

            KDynamicArea dynArea = KTDynamicAreaManager.Add(this.Scene.RefObject.MapCode, -1, this.Name, this.ResID, (int) this.Pos.X, (int) this.Pos.Y, this.LifeTime, this.Tick, this.Radius, this.ScriptID, this.Tag);
            this.Done?.Invoke(dynArea);
            return true;
        }
    }
}
