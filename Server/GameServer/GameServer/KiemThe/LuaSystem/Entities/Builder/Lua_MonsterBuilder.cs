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
    /// Đối tượng Builder dùng cho Lua để xây quái
    /// </summary>
    [MoonSharpUserData]
    public class Lua_MonsterBuilder
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
        /// Sinh lực tối đa
        /// <para>-1 sẽ lấy ở File cấu hình</para>
        /// </summary>
        [MoonSharpHidden]
        public int HP { get; set; } = -1;

        /// <summary>
        /// Cấp độ
        /// <para>-1 sẽ lấy ở file cấu hình</para>
        /// </summary>
        [MoonSharpHidden]
        public int Level { get; set; } = -1;

        /// <summary>
        /// Hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        [MoonSharpHidden]
        public int Direction { get; set; } = -1;

        /// <summary>
        /// Ngũ hành
        /// <para>0 nếu random ngũ hành khi sinh ra</para>
        /// </summary>
        [MoonSharpHidden]
        public int Series { get; set; } = 0;

        /// <summary>
        /// Loại AI
        /// </summary>
        [MoonSharpHidden]
        public int AIType { get; set; } = 0;

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int AIID { get; set; } = -1;

        /// <summary>
        /// Thời gian tái sinh sau khi chết
        /// <para>-1 nếu không tái sinh</para>
        /// </summary>
        [MoonSharpHidden]
        public long RespawnTick { get; set; } = -1;

        /// <summary>
        /// Sự kiện kiểm tra điều kiện để tái sinh quái sau khi chết
        /// </summary>
        [MoonSharpHidden]
        public Closure RespawnCondition { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        [MoonSharpHidden]
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện khi hoàn tất tạo quái
        /// </summary>
        [MoonSharpHidden]
        public Action<Monster> Done { get; set; }

        /// <summary>
        /// Camp
        /// </summary>
        [MoonSharpHidden]
        public int Camp { get; set; } = 65535;

        /*
        /// <summary>
        /// Thiết lập bản đồ đang đứng tương ứng
        /// </summary>
        /// <param name="scene"></param>
        public Lua_MonsterBuilder SetScene(Lua_Scene scene)
        {
            /// Nếu bản đồ không tồn tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(scene.RefObject.MapCode, out GameMap gameMap))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_MonsterBuilder:SetScene, scene is not exist!"));
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
        public Lua_MonsterBuilder SetResID(int id)
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
        public Lua_MonsterBuilder SetPos(int posX, int posY)
        {
            this.Pos = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetPos(Lua_Vector2 pos)
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
        public Lua_MonsterBuilder SetName(string name)
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
        public Lua_MonsterBuilder SetTitle(string title)
        {
            this.Title = title;
            return this;
        }

        /// <summary>
        /// Thiết lập sinh lực tối đa
        /// <para>-1 sẽ lấy ở file cấu hình</para>
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetMaxHP(int hp)
        {
            this.HP = hp;
            return this;
        }

        /// <summary>
        /// Thiết lập cấp độ
        /// <para>-1 sẽ lấy ở file cấu hình</para>
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetLevel(int level)
        {
            this.Level = level;
            return this;
        }

        /// <summary>
        /// Thiết lập hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetDirection(int direction)
        {
            this.Direction = direction;
            return this;
        }

        /// <summary>
        /// Thiết lập hướng quay
        /// <para>0 nếu random ngũ hành khi sinh ra</para>
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetSeries(int series)
        {
            this.Series = series;
            return this;
        }

        /// <summary>
        /// Thiết lập loại AI
        /// </summary>
        /// <param name="aiType"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetAIType(int aiType)
        {
            this.AIType = aiType;
            return this;
        }

        /// <summary>
        /// Thiết lập ScriptID
        /// <para>-1 nếu không có Script</para>
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetScriptID(int scriptID)
        {
            this.ScriptID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập AI Script ID
        /// <para>-1 nếu không có Script</para>
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetAIScriptID(int scriptID)
        {
            this.AIID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập thời gian tái sinh
        /// <para>-1 nếu không tái sinh</para>
        /// </summary>
        /// <param name="respawnTick"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetRespawnTick(long respawnTick)
        {
            this.RespawnTick = respawnTick;
            return this;
        }

        /// <summary>
        /// Thiết lập Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetTag(string tag)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        /// Thiết lập điều kiện để tiếp tục tái sinh quái
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetRespawnCondition(Closure func)
        {
            this.RespawnCondition = func;
            return this;
        }

        /// <summary>
        /// Thiết lập thực thi sự kiện khi hoàn tất
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetDone(Closure func)
        {
            this.Done = (monster) => {

                object[] parameters = new object[]
                {
                    new Lua_Monster()
                    {
                        CurrentScene = this.Scene,
                        RefObject = monster,
                    }
                };
                KTLuaScript.Instance.ExecuteFunctionAsync("MonsterBuilder:Done", func, parameters, null);
            };
            return this;
        }

        /// <summary>
        /// Thiết lập Camp
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public Lua_MonsterBuilder SetCamp(int camp)
        {
            this.Camp = camp;
            return this;
        }

        /// <summary>
        /// Tạo quái tương ứng
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            /// Nếu bản đồ không tồn tại
            if (this.Scene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_MonsterBuilder:Build, scene is NIL!"));
                return false;
            }
            /*
            /// Nếu phụ bản không tồn tại
            if (this.CopyScene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_MonsterBuilder:Build, copyscene is NIL!"));
                return false;
            }
            */

            /// Ngũ hành
            KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;
            /// Nếu có thiết lập ngũ hành
            if (this.Series > (int) KE_SERIES_TYPE.series_none && this.Series < (int) KE_SERIES_TYPE.series_num)
            {
                series = (KE_SERIES_TYPE) this.Series;
            }

            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Nếu có thiết lập hướng quay
            if (this.Direction > (int) KiemThe.Entities.Direction.NONE && this.Direction < (int) KiemThe.Entities.Direction.Count)
            {
                dir = (KiemThe.Entities.Direction) this.Direction;
            }

            /// AIType
            MonsterAIType aiType = MonsterAIType.Normal;
            /// Thiết lập AIType
            if (this.AIType >= (int) MonsterAIType.Normal && this.AIType < (int) MonsterAIType.Total)
            {
                aiType = (MonsterAIType) this.AIType;
            }

            /// Tạo quái tương ứng
            return GameManager.MonsterZoneMgr.AddDynamicMonsters(this.Scene.RefObject.MapCode, this.ResID, -1, 1, (int) this.Pos.X, (int) this.Pos.Y, this.Name, this.Title, this.HP, this.Level, dir, series, aiType, this.RespawnTick, this.ScriptID, this.AIID, this.Tag, (monster) => {
                /// Thực thi sự kiện tạo thành công
                this.Done?.Invoke(monster);
            }, this.Camp);
        }
    }
}
