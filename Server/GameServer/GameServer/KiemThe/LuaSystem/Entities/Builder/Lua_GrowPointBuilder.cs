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
    public class Lua_GrowPointBuilder
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
        /// Tên điểm thu thập
        /// </summary>
        [MoonSharpHidden]
        public string Name { get; set; } = "";

        /// <summary>
        /// Thời gian tái sinh
        /// </summary>
        [MoonSharpHidden]
        public long RespawnTime { get; set; } = -1;

        /// <summary>
        /// Thời gian thu thập
        /// </summary>
        public long CollectTick { get; set; }

        /// <summary>
        /// Bị mất trạng thái thu thập nếu chịu sát thương
        /// </summary>
        public bool InteruptIfTakeDamage { get; set; }

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// Tag
        /// </summary>
        [MoonSharpHidden]
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện khi hoàn tất tạo quái
        /// </summary>
        [MoonSharpHidden]
        public Action<GrowPoint> Done { get; set; }

        /*
        /// <summary>
        /// Thiết lập bản đồ đang đứng tương ứng
        /// </summary>
        /// <param name="scene"></param>
        public Lua_GrowPointBuilder SetScene(Lua_Scene scene)
        {
            /// Nếu bản đồ không tồn tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_GrowPointBuilder:SetScene, scene is not exist!"));
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
        public Lua_GrowPointBuilder SetResID(int id)
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
        public Lua_GrowPointBuilder SetPos(int posX, int posY)
        {
            this.Pos = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetPos(Lua_Vector2 pos)
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
        public Lua_GrowPointBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Thiết lập thời gian sống lại
        /// <para>-1 nếu không sống lại</para>
        /// </summary>
        /// <param name="respawnTime"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetRespawnTime(long respawnTime)
        {
            this.RespawnTime = respawnTime;
            return this;
        }

        /// <summary>
        /// Thiết lập thời gian thu thập
        /// </summary>
        /// <param name="collectTick"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetCollectTick(long collectTick)
        {
            this.CollectTick = collectTick;
            return this;
        }

        /// <summary>
        /// Thiết lập có bị mất trạng thái thu thập nếu chịu sát thương không
        /// </summary>
        /// <param name="interuptIfTakeDamage"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetInteruptIfTakeDamage(bool interuptIfTakeDamage)
        {
            this.InteruptIfTakeDamage = interuptIfTakeDamage;
            return this;
        }

        /// <summary>
        /// Thiết lập ScriptID
        /// <para>-1 nếu không có Script</para>
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetScriptID(int scriptID)
        {
            this.ScriptID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetTag(string tag)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        /// Thiết lập thực thi sự kiện khi hoàn tất
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_GrowPointBuilder SetDone(Closure func)
        {
            this.Done = (growPoint) => {
                object[] parameters = new object[]
                {
                    new Lua_GrowPoint()
                    {
                        CurrentScene = this.Scene,
                        RefObject = growPoint,
                    }
                };
                KTLuaScript.Instance.ExecuteFunctionAsync("GrowPointBuilder:Done", func, parameters, null);
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
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_GrowPointBuilder:Build, scene is NIL!"));
                return false;
            }

            GrowPoint growPoint = KTGrowPointManager.Add(this.Scene.RefObject.MapCode, -1, GrowPointXML.Parse(this.ResID, this.Name, this.RespawnTime, this.ScriptID, this.CollectTick, this.InteruptIfTakeDamage), (int) this.Pos.X, (int) this.Pos.Y);
            this.Done?.Invoke(growPoint);
            return true;
        }
    }
}
