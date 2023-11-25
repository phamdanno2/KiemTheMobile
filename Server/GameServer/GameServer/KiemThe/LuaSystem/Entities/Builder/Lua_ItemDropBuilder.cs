using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.LuaSystem.Entities.Math;
using GameServer.Logic;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.LuaSystem.Entities.Builder
{
    /// <summary>
    /// Builder vật phẩm rơi dưới bản đồ
    /// </summary>
    [MoonSharpUserData]
    public class Lua_ItemDropBuilder
    {
        /// <summary>
        /// Bản đồ đang đứng
        /// </summary>
        [MoonSharpHidden]
        public Lua_Scene Scene { get; set; }

        /// <summary>
        /// Danh sách vật phẩm
        /// </summary>
        [MoonSharpHidden]
        public List<ItemData> Items { get; set; } = new List<ItemData>();

        /// <summary>
        /// Chủ nhân/đội nhóm
        /// </summary>
        [MoonSharpHidden]
        public Lua_Player Owner { get; set; }

        /// <summary>
        /// Đối tượng quái chết
        /// </summary>
        [MoonSharpHidden]
        public Lua_Monster Monster { get; set; }

        /// <summary>
        /// Vị trí rơi
        /// </summary>
        [MoonSharpHidden]
        public Lua_Vector2 Position { get; set; }

        /// <summary>
        /// Thêm vật phẩm vào danh sách
        /// </summary>
        /// <param name="id"></param>
        public Lua_ItemDropBuilder AddItem(int id)
        {
            /// Nếu vật phẩm tồn tại
            if (ItemManager._TotalGameItem.TryGetValue(id, out ItemData itemData))
            {
                /// Thêm vào danh sách
                this.Items.Add(itemData);
            }
            return this;
        }

        /// <summary>
        /// Thiết lập chủ nhân và đội nhóm tương ứng
        /// </summary>
        /// <param name="player"></param>
        public Lua_ItemDropBuilder SetOwner(Lua_Player player)
        {
            this.Owner = player;
            return this;
        }

        /// <summary>
        /// Thiết lập quái rơi ra đồ
        /// </summary>
        /// <param name="monster"></param>
        public Lua_ItemDropBuilder SetFromMonster(Lua_Monster monster)
        {
            this.Monster = monster;
            return this;
        }

        /// <summary>
        /// Thiết lập vị trí rơi
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public Lua_ItemDropBuilder SetPos(int posX, int posY)
        {
            this.Position = new Lua_Vector2(posX, posY);
            return this;
        }

        /// <summary>
        /// Thiết lập vị trí rơi
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Lua_ItemDropBuilder SetPos(Lua_Vector2 pos)
        {
            this.Position = pos;
            return this;
        }

        /// <summary>
        /// Xây đối tượng
        /// </summary>
        public void Build()
        {
            /// Nếu danh sách rỗng
            if (this.Items == null || this.Items.Count <= 0)
            {
                /// Không xử lý
                return;
            }

            /// Nếu không có chủ nhân
            if (this.Owner == null)
            {
                /// Nếu không có quái
                if (this.Monster == null)
                {
                    /// Nếu không có vị trí
                    if (this.Position == null)
                    {
                        /// Không xử lý
                        return;
                    }
                    else
                    {
                        /// Tạp vật phẩm rơi ở MAP tại vị trí tương ứng
                        ItemDropManager.CreateDropToMap(this.Scene.RefObject.MapCode, -1, (int) this.Position.X, (int) this.Position.Y, this.Items);
                    }
                }
                else
                {
                    /// Tạo vật phẩm rơi ở MAP do quái chết rơi ra
                    ItemDropManager.CreateDropToMap(this.Items, this.Monster.RefObject);
                }

            }
            /// Nếu có chủ nhân
            else
            {
                /// Nếu không có quái
                if (this.Monster == null)
                {
                    /// Nếu không có vị trí rơi
                    if (this.Position == null)
                    {
                        /// Không xử lý
                        return;
                    }
                    else
                    {
                        /// Tạp vật phẩm rơi ở MAP tại vị trí tương ứng
                        ItemDropManager.CreateDropToMap(this.Scene.RefObject.MapCode, -1, (int) this.Position.X, (int) this.Position.Y, this.Items);
                    }
                }
                else
                {
                    /// Tạo vật phẩm rơi ở Map do quái chết rơi ra
                    ItemDropManager.CreateDropToMap(this.Owner.RefObject, this.Items, this.Monster.RefObject);
                }
            }
        }
    }
}
