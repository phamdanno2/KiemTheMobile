using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Data;
using GameDBServer.DB;

namespace GameDBServer.Core.GameEvent.EventObjectImpl
{
    /// <summary>
    /// 玩家登陆事件
    /// </summary>
    public class PlayerLoginEventObject : EventObject
    {
        private DBRoleInfo roleInfo;

        public PlayerLoginEventObject(DBRoleInfo roleInfo)
            : base((int)EventTypes.PlayerLogin)
        {
            this.roleInfo = roleInfo;
        }

        public DBRoleInfo RoleInfo
        {
            get { return this.roleInfo; }
        }
    }
}
