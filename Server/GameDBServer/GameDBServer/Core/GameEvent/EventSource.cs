using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Tools;

namespace GameDBServer.Core.GameEvent
{
    /// <summary>
    /// 事件源
    /// 负责衔接各个功能模块，从而避免耦合
    /// 所有的事件源时单例的，服务器全局的
    /// </summary>
    public abstract class EventSource
    {
        //监听器缓存
        //对于某一个事件可能诸多观察者会感兴趣
        protected Dictionary<int, List<IEventListener>> listeners = new Dictionary<int, List<IEventListener>>();

        public void registerListener(int eventType, IEventListener listener)
        {
            lock (listeners)
            {
                List<IEventListener> listenerList = null;
                if (!listeners.TryGetValue(eventType, out listenerList))
                {
                    listenerList = new List<IEventListener>();
                    listeners.Add(eventType, listenerList);
                }
                listenerList.Add(listener);
            }
        }


        public void removeListener(int eventType, IEventListener listener)
        {
            lock (listeners)
            {
                List<IEventListener> listenerList = null;
                if (!listeners.TryGetValue(eventType, out listenerList))
                {
                    return;
                }
                listenerList.Remove(listener);
            }
        }

        public void fireEvent(EventObject eventObj)
        {
            if (null == eventObj || eventObj.getEventType() == -1)
                return;

            List<IEventListener> listenerList = null;

            if (!listeners.TryGetValue(eventObj.getEventType(), out listenerList))
                return;

            dispatchEvent(eventObj, listenerList);

        }

        private void dispatchEvent(EventObject eventObj, List<IEventListener> listenerList)
        {
            foreach (IEventListener listener in listenerList)
            {
                try
                {
                    listener.processEvent(eventObj);
                }
                catch (System.Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("事件处理错误: {0},{1}", (EventTypes)eventObj.getEventType(), ex));
                }

            }
        }
    }
}
