

namespace GameDBServer.Logic
{
    /// <summary>
    /// 功能模块管理器接口
    /// </summary>
    public interface IManager
    {
        bool initialize();

        bool startup();

        bool showdown();

        bool destroy();
    }

    /// <summary>
    /// 全局服务管理器
    /// 负责统一开启和关闭各个功能模块服务
    /// </summary>
    public class GlobalServiceManager
    {
        public static void initialize()
        {
          
        
        }

        public static void startup()
        {
        
        }

        public static void showdown()
        {
          
        }

        public static void destroy()
        {
            // 七日活动
          
        }
    }
}