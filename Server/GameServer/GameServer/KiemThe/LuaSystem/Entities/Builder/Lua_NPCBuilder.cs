using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
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
    /// Đối tượng Builder dùng cho Lua để xây NPC
    /// </summary>
    [MoonSharpUserData]
    public class Lua_NPCBuilder
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
        /// Danh hiệu quái
        /// <para>Bỏ trống sẽ lấy danh hiệu ở file cấu hình</para>
        /// </summary>
        [MoonSharpHidden]
        public string Title { get; set; } = "";

        /// <summary>
        /// Hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        [MoonSharpHidden]
        public int Direction { get; set; } = -1;

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
        public Action<NPC> Done { get; set; }

        /*
        /// <summary>
        /// Thiết lập bản đồ đang đứng tương ứng
        /// </summary>
        /// <param name="scene"></param>
        public Lua_NPCBuilder SetScene(Lua_Scene scene)
        {
            /// Nếu bản đồ không tồn tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_NPCBuilder:SetScene, scene is not exist!"));
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
        public Lua_NPCBuilder SetResID(int id)
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
        public Lua_NPCBuilder SetPos(int posX, int posY)
        {
            this.Pos = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetPos(Lua_Vector2 pos)
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
        public Lua_NPCBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Thiết lập danh hiệu
        /// <para>Bỏ trống sẽ lấy danh hiệu ở file cấu hình</para>
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetTitle(string title)
        {
            this.Title = title;
            return this;
        }

        /// <summary>
        /// Thiết lập hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetDirection(int direction)
        {
            this.Direction = direction;
            return this;
        }

        /// <summary>
        /// Thiết lập ScriptID
        /// <para>-1 nếu không có Script</para>
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetScriptID(int scriptID)
        {
            this.ScriptID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetTag(string tag)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        /// Thiết lập thực thi sự kiện khi hoàn tất
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_NPCBuilder SetDone(Closure func)
        {
            this.Done = (npc) => {
                object[] parameters = new object[]
                {
                    new Lua_NPC()
                    {
                        CurrentScene = this.Scene,
                        RefObject = npc,
                    }
                };
                KTLuaScript.Instance.ExecuteFunctionAsync("NPCBuilder:Done", func, parameters, null);
            };
            
            return this;
        }

        /// <summary>
        /// Tạo NPC tương ứng
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            /// Nếu bản đồ không tồn tại
            if (this.Scene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_NPCBuilder:Build, scene is NIL!"));
                return false;
            }
            /*
            /// Nếu phụ bản không tồn tại
            if (this.CopyScene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_NPCBuilder:Build, copyscene is NIL!"));
                return false;
            }
            */

            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Nếu có thiết lập hướng quay
            if (this.Direction > (int) KiemThe.Entities.Direction.NONE && this.Direction < (int) KiemThe.Entities.Direction.Count)
            {
                dir = (KiemThe.Entities.Direction) this.Direction;
            }

            return NPCGeneralManager.AddNewNPC(this.Scene.RefObject.MapCode, -1, this.ResID, (int) this.Pos.X, (int) this.Pos.Y, this.Name, this.Title, dir, this.ScriptID, this.Tag, (npc) => {
                this.Done?.Invoke(npc);
            });
        }
    }
}
