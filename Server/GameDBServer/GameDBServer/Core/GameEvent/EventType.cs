using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Core.GameEvent
{
    /// <summary>
    /// 事件类型定义，只允许定义大事件，同一类事件需要细分类型，请在各自逻辑内定义自己的常量来区分（示例：参见战盟事件 logic/BangHui/ZhanMengShiJian/ZhanMengShiJianManager.cs)
    /// </summary>
    public enum EventTypes
    {
        PlayerLogin,//玩家登陆
        PlayerLogout,//玩家登出
    }
}
