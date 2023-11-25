using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.LuaSystem.Entities.Math;
using GameServer.Logic;
using MoonSharp.Interpreter;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.LuaSystem.Entities.Builder
{
    /// <summary>
    /// Đối tượng Builder dùng cho Lua để xây BOT
    /// </summary>
    [MoonSharpUserData]
    public class Lua_BotBuilder
    {
        /// <summary>
        /// Bản đồ đang đứng
        /// </summary>
        [MoonSharpHidden]
        public Lua_Scene Scene { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        [MoonSharpHidden]
        public string RoleName { get; set; }

        /// <summary>
        /// Danh hiệu đối tượng
        /// </summary>
        [MoonSharpHidden]
        public string Title { get; set; }

        /// <summary>
        /// Cấp độ
        /// </summary>
        [MoonSharpHidden]
        public int Level { get; set; }

        /// <summary>
        /// Danh sách trang bị
        /// </summary>
        [MoonSharpHidden]
        public List<GoodsData> Equips { get; set; } = new List<GoodsData>();

        /// <summary>
        /// Vị trí đang đứng
        /// </summary>
        [MoonSharpHidden]
        public Lua_Vector2 Pos { get; set; }

        /// <summary>
        /// ID môn phái
        /// </summary>
        [MoonSharpHidden]
        public int FactionID { get; set; }

        /// <summary>
        /// ID nhánh
        /// </summary>
        [MoonSharpHidden]
        public int RouteID { get; set; }

        /// <summary>
        /// ID Avarta
        /// </summary>
        [MoonSharpHidden]
        public int RolePic { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [MoonSharpHidden]
        public int RoleSex { get; set; }

        /// <summary>
        /// Có đang trong trạng thái cưỡi không
        /// </summary>
        [MoonSharpHidden]
        public bool IsRiding { get; set; }

        /// <summary>
        /// ID Script Lua điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int ScriptID { get; set; }

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        [MoonSharpHidden]
        public int AIID { get; set; }

        /// <summary>
        /// Phạm vi đuổi mục tiêu
        /// </summary>
        [MoonSharpHidden]
        public int SeekRange { get; set; } = 1000;

        /// <summary>
        /// Tag
        /// </summary>
        [MoonSharpHidden]
        public string Tag { get; set; } = "";

        /// <summary>
        /// Sự kiện khi hoàn tất tạo BOT
        /// </summary>
        [MoonSharpHidden]
        public Action<KTBot> Done { get; set; }

        /// <summary>
        /// Camp
        /// </summary>
        [MoonSharpHidden]
        public int Camp { get; set; } = -1;

        /// <summary>
        /// Hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        [MoonSharpHidden]
        public int Direction { get; set; } = -1;

        /// <summary>
        /// Sinh lực
        /// </summary>
        [MoonSharpHidden]
        public int HP { get; set; }

        /// <summary>
        /// Sinh lực cực đại
        /// </summary>
        [MoonSharpHidden]
        public int HPMax { get; set; }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetPos(int posX, int posY)
        {
            this.Pos = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập tọa độ đứng
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetPos(Lua_Vector2 pos)
        {
            this.Pos = pos;
            return this;
        }

        /// <summary>
        /// Thiết lập tên
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetName(string name)
        {
            this.RoleName = name;
            return this;
        }

        /// <summary>
        /// Thiết lập danh hiệu
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetTitle(string title)
        {
            this.Title = title;
            return this;
        }

        /// <summary>
        /// Thiết lập cấp độ
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetLevel(int level)
        {
            this.Level = level;
            return this;
        }

        /// <summary>
        /// Thiết lập môn phái và nhánh
        /// </summary>
        /// <param name="factionID"></param>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetFaction(int factionID, int routeID)
        {
            this.FactionID = factionID;
            this.RouteID = routeID;
            return this;
        }

        /// <summary>
        /// Thiết lập ID Avarta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetAvarta(int id)
        {
            this.RolePic = id;
            return this;
        }

        /// <summary>
        /// Thiết lập giới tính
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetSex(int sex)
        {
            this.RoleSex = sex;
            return this;
        }

        /// <summary>
        /// Thiết lập trạng thái cưỡi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetIsRiding(bool isRiding)
        {
            this.IsRiding = isRiding;
            return this;
        }

        /// <summary>
        /// Thiết lập phạm vi đuổi mục tiêu
        /// </summary>
        /// <param name="seekRange"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetSeekRange(int seekRange)
        {
            this.SeekRange = seekRange;
            return this;
        }

        /// <summary>
        /// Thiết lập Script LUA điều khiển
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetScriptID(int scriptID)
        {
            this.ScriptID = scriptID;
            return this;
        }

        /// <summary>
        /// Thiết lập Script AI điều khiển
        /// </summary>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetAIID(int aiID)
        {
            this.AIID = aiID;
            return this;
        }

        /// <summary>
        /// Thiết lập Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetTag(string tag)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        /// Thiết lập Camp
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetCamp(int camp)
        {
            this.Camp = camp;
            return this;
        }

        /// <summary>
        /// Thiết lập hướng quay
        /// <para>-1 sẽ chọn random 1 trong 8 hướng</para>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetDirection(int direction)
        {
            this.Direction = direction;
            return this;
        }

        /// <summary>
        /// Thiết lập sinh lực
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetHP(int hp)
        {
            this.HP = hp;
            return this;
        }

        /// <summary>
        /// Thiết lập sinh lực cực đại
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetHPMax(int hpMax)
        {
            this.HPMax = hpMax;
            return this;
        }

        /// <summary>
        /// Thêm trang bị cho BOT
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="series"></param>
        /// <param name="enhanceLevel"></param>
        /// <returns></returns>
        public Lua_BotBuilder AddEquip(int itemID, int series, int enhanceLevel)
        {
            GoodsData itemGD = new GoodsData()
            {
                GoodsID = itemID,
                Series = series,
                Forge_level = enhanceLevel,
            };
            this.Equips.Add(itemGD);

            return this;
        }

        /// <summary>
        /// Thiết lập thực thi sự kiện khi hoàn tất
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Lua_BotBuilder SetDone(Closure func)
        {
            this.Done = (bot) => {

                object[] parameters = new object[]
                {
                    new Lua_Bot()
                    {
                        CurrentScene = this.Scene,
                        RefObject = bot,
                    }
                };
                KTLuaScript.Instance.ExecuteFunctionAsync("BotBuilder:Done", func, parameters, null);
            };
            return this;
        }

        /// <summary>
        /// Xây đối tượng BOT tương ứng
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            /// Nếu bản đồ không tồn tại
            if (this.Scene == null)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Lua error on Lua_BotBuilder:Build, scene is NIL!"));
                return false;
            }

            KTBot bot = new KTBot();
            /// Nếu có phụ bản
            bot.CurrentCopyMapID = -1;
            bot.CurrentMapCode = this.Scene.RefObject.MapCode;

            /// Hướng quay
            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
            /// Nếu có thiết lập hướng quay
            if (this.Direction > (int) KiemThe.Entities.Direction.NONE && this.Direction < (int) KiemThe.Entities.Direction.Count)
            {
                bot.CurrentDir = (KiemThe.Entities.Direction) this.Direction;
            }

            bot.RoleName = this.RoleName;
            bot.Title = this.Title;
            bot.m_Level = this.Level;
            bot.CurrentPos = new System.Windows.Point(this.Pos.X, this.Pos.Y);
            bot.FactionID = this.FactionID;
            bot.RouteID = this.RouteID;
            bot.RolePic = this.RolePic;
            bot.RoleSex = this.RoleSex;
            bot.IsRiding = this.IsRiding;
            bot.ScriptID = this.ScriptID;
            bot.AIID = this.AIID;
            bot.Camp = this.Camp;
            bot.SeekRange = this.SeekRange;
            bot.Tag = this.Tag;
            bot.Camp = this.Camp;
            bot.ChangeLifeMax(this.HPMax, 0, 0);
            bot.m_CurrentLife = this.HP;
            bot.Equips = this.Equips;

            /// Thêm Bot vào danh sách quản lý
            KTBotManager.Add(bot);

            /// Thực thi sự kiện tạo thành công
            this.Done?.Invoke(bot);

            return true;
        }
    }
}
