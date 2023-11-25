using GameServer.KiemThe.LuaSystem.Entities.Math;
using GameServer.Logic;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.LuaSystem.Entities
{
    /// <summary>
    /// Đối tượng BOT dùng trong LUA
    /// </summary>
    [MoonSharpUserData]
    public class Lua_Bot
    {
        #region Base for all objects
        /// <summary>
        /// Đối tượng tham chiếu trong hệ thống
        /// </summary>
        [MoonSharpHidden]
        public KTBot RefObject { get; set; }

        /// <summary>
        /// Trả về ID đối tượng
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return this.RefObject.RoleID;
        }

        /// <summary>
        /// Trả về tên đối tượng
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.RefObject.RoleName;
        }

        /// <summary>
        /// Trả về cấp độ đối tượng
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return this.RefObject.m_Level;
        }
        #endregion

        #region Base for all scene-objects
        /// <summary>
        /// Bản đồ hiện tại
        /// </summary>
        [MoonSharpHidden]
        public Lua_Scene CurrentScene { get; set; }

        /// <summary>
        /// Trả về vị trí của đối tượng
        /// </summary>
        /// <returns></returns>
        public Lua_Vector2 GetPos()
        {
            return new Lua_Vector2((int) this.RefObject.CurrentPos.X, (int) this.RefObject.CurrentPos.Y);
        }

        /// <summary>
        /// Trả về bản đồ hiện tại
        /// </summary>
        /// <returns></returns>
        public Lua_Scene GetScene()
        {
            return this.CurrentScene;
        }

        /// <summary>
        /// Trả về danh hiệu hiện tại
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return this.RefObject.Title;
        }

        /// <summary>
        /// Di chuyển đến vị trí chỉ định
        /// </summary>
        /// <param name="toPos"></param>
        public void MoveTo(Lua_Vector2 toPos)
        {
            this.RefObject.MoveTo(new System.Windows.Point(toPos.X, toPos.Y));
        }

        /// <summary>
        /// Trả về loại đối tượng
        /// </summary>
        /// <returns></returns>
        public int GetObjectType()
        {
            return (int) ObjectTypes.OT_MONSTER;
        }
        #endregion
    }
}
