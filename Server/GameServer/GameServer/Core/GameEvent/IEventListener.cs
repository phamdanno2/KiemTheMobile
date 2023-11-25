namespace GameServer.Core.GameEvent
{
    /// <summary>
    /// 事件监听器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventListener
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventObject"></param>
        void processEvent(EventObject eventObject);
    }
}