using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Core.GameEvent
{
    /// <summary>
    /// 全局的事件源
    /// </summary>
    public class GlobalEventSource : EventSource
    {
        private static GlobalEventSource instance = new GlobalEventSource();

        private GlobalEventSource() { }

        public static GlobalEventSource getInstance()
        {
            return instance;
        }
    }
}
