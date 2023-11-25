namespace GameServer.Core.GameEvent
{
    /// <summary>
    /// 全局的事件源
    /// </summary>
    public class GlobalEventSource : EventSource
    {
        private static GlobalEventSource instance = new GlobalEventSource();

        private GlobalEventSource()
        {
        }

        public static GlobalEventSource getInstance()
        {
            return instance;
        }
    }

    public class GlobalEventSource4Scene : SceneEventSource
    {
        private static GlobalEventSource4Scene instance = new GlobalEventSource4Scene();

        private GlobalEventSource4Scene()
        {
        }

        public static GlobalEventSource4Scene getInstance()
        {
            return instance;
        }
    }
}