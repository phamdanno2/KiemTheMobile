using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Activity.DaySeriesLoginEvent;
using GameServer.KiemThe.Core.Activity.DownloadBouns;
using GameServer.KiemThe.Core.Activity.EveryDayOnlineEvent;
using GameServer.KiemThe.Core.Activity.LevelUpEvent;
using GameServer.KiemThe.Core.Activity.LuckyCircle;
using GameServer.KiemThe.Core.Activity.PlayerPray;
using GameServer.KiemThe.Core.Activity.RechageEvent;
using GameServer.KiemThe.Core.Activity.SeashellCircle;
using GameServer.KiemThe.Core.Activity.TotalLoginEvent;
using GameServer.KiemThe.Core.BulletinManager;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.MonsterAIScript;
using GameServer.KiemThe.Core.Rechage;
using GameServer.KiemThe.Core.Repute;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Core.Title;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameEvents;
using GameServer.KiemThe.GameEvents.GuildWarManager;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Logic.Manager;
using GameServer.KiemThe.Logic.Manager.Shop;
using GameServer.KiemThe.Logic.Manager.Skill.PoisonTimer;
using GameServer.KiemThe.LuaSystem;
using GameServer.Logic;
using GameServer.Logic.RefreshIconState;
using System;
using System.Threading;

namespace GameServer.KiemThe
{
    /// <summary>
    /// Gọi đến ở quá trình đọc dữ liệu
    /// </summary>
    public static class KTMain
    {
        /// <summary>
        /// Khởi tạo dữ liệu trước khi tải danh sách bản đồ
        /// </summary>
        public static void InitFirst()
        {
            /// Thiết lập hệ thống
            Console.WriteLine("Loading ServerConfig...");
            ServerConfig.Init();

            /// Đọc dữ liệu thuộc tính cộng của môn phái và nhánh
            KFaction.Init();

            /// Đọc dữ liệu thuộc tính cơ bản của đối tượng
            KNpcSetting.Init();

            /// Đọc dữ liệu thuộc tính cơ bản khi thăng cấp của nhân vật
            KPlayerSetting.Init();

            /// Tải danh sách Tân Thủ Thôn cho phép người chơi mới tạo được vào
            KTGlobal.LoadNewbieVillages();


            KTGlobal.LoadPKPunish();

            /// Đọc dữ liệu lọc nội dung Chat
            KTChatFilter.Init();

            /// Đọc dữ liệu danh hiệu
            KTTitleManager.Init();

            /// Ngũ hành tương khắc
            KTGlobal.LoadAccrueSeries();
            KSpecialStateManager.Init();

            /// Đọc dữ liệu PropertyDictionary
            KTGlobal.ReadPropertyDictionary();

            /// Đọc dữ liệu kỹ năng
            KSkill.LoadSkillData();

            /// Đọc dữ liệu Avarta nhân vật
            KRoleAvarta.Init();

            #region Timers
            /// Khởi tạo luồng quản lý Captcha
            Console.WriteLine("Starting KTCaptchaManager...");
            KTCaptchaManager.Init();

            /// Khởi tạo luồng quản lý thời gian của Người
            Console.WriteLine("Starting KTPlayerTimerManager...");
            KTPlayerTimerManager.Init();

            /// Khởi tạo luồng quản lý thời gian của Quái
            Console.WriteLine("Starting KTMonsterTimerManager...");
            KTMonsterTimerManager.Init();

            /// Khởi tạo luồng quản lý thời gian của BOT
            Console.WriteLine("Starting KTBotTimerManager...");
            KTBotTimerManager.Init();

            /// Khởi tạo luồng quản lý kỹ năng
            Console.WriteLine("Starting KTSkillManager...");
            KTSkillManager.Init();

            /// Khởi tạo luồng quản lý đạn bay
            Console.WriteLine("Starting KTBulletManager...");
            KTBulletManager.Init();

            /// Khởi tạo luông quản lý Task
            Console.WriteLine("Starting KTTaskManager...");
            KTTaskManager.Init();

            /// Khởi tạo luông quản lý ScheduleTask
            Console.WriteLine("Starting KTScheduleTaskManager...");
            KTScheduleTaskManager.Init();

            /// Khởi tạo luông quản lý Trúng độc
            Console.WriteLine("Starting KTPoisonTimerManager...");
            KTPoisonTimerManager.Init();

            /// Khởi tạo luồng quản lý Buff
            Console.WriteLine("Starting KTBuffManager...");
            KTBuffManager.Init();

            /// Khởi tạo luồng quản lý di chuyển Quái
            Console.WriteLine("Starting KTMonsterStoryBoard...");
            KTMonsterStoryBoardEx.Init();

            /// Khởi tạo luồng quản lý di chuyển Người
            Console.WriteLine("Starting KTPlayerStoryBoard...");
            KTPlayerStoryBoardEx.Init();

            /// Khởi tạo luồng quản lý di chuyển các đối tượng khác
            Console.WriteLine("Starting KTOtherObjectStoryBoard...");
            KTOtherObjectStoryBoard.Init();

            /// Khởi tạo quản lý phụ bản
            Console.WriteLine("Starting KTCopySceneTimerManager...");
            CopySceneEventManager.Init();

            /// Khởi tạo luồng quản lý khu vực động
            Console.WriteLine("Starting KTDynamicAreaTimerManager...");
            KTDynamicAreaTimerManager.Init();
            #endregion

            /// Khởi tạo Monster AI Script
            KTMonsterAIScriptManager.Init();

            Console.WriteLine("Starting Lua-Env...");
            /// Khởi tạo môi trường Lua
            KTLuaEnvironment.Init();

            /// Tải danh sách GM
            Console.WriteLine("Loading GMList...");
            KTGMCommandManager.LoadGMList();

            /// Tải danh sách vật phẩm
            Console.WriteLine("Loading Items...");
            ItemManager.ItemSetup();

            // Tải danh sách kỹ năng sống
            Console.WriteLine("Loading LifeSkill.....");
            ItemCraftingManager.Setup();

            Console.WriteLine("Loading ItemRefine.....");
            ItemRefine.Setup();

            TaskManager.getInstance().Setup();


            Console.WriteLine("Loading Randombox.....");
            ItemRandomBox.Setup();




            /// Tải danh sách vật phẩm Drop
            Console.WriteLine("Loading ItemDrop...");
            ItemDropManager.Setup();

            /// Tải dữ liệu Tu Luyện Châu
            Console.WriteLine("Loading XiuLianZhu...");
            ItemXiuLianZhuManager.Init();

            /// Tải danh sách kinh nghiệm khi quái chết
            Console.WriteLine("Loading MonsterDeadExp...");
            MonsterDeadHelper.ExpInit();



            EliteMonsterManager.getInstance().SetupAllBoss();

            /// Tải danh sách cửa hàng
            Console.WriteLine("Loading Shops...");
            ShopManager.Setup();
            // Quản lý nạp thẻ
            RechageServiceManager.StartService();

            /// Tải danh sách sự kiện
            Console.WriteLine("Loading Activities...");
            KTActivityManager.Init();

            BulletinManager.Setup();

            #region Activity
            /// Khởi tạo dữ liệu vòng quay sò
            KTSeashellCircleManager.Init();
            /// Khởi tạo dữ liệu vòng quay may mắn
            KTLuckyCircleManager.Init();
            /// Khởi tạo dữ liệu chúc phúc
            KTPlayerPrayManager.Init();

            EveryDayOnlineManager.Setup();
            SevenDayLoginManager.Setup();
            TotalLoginManager.Setup();
            CardMonthManager.Setup();
            LevelUpEventManager.Setup();
            RechageManager.Setup();
            IconStateManager.Setup();
            ReputeManager.Setup();

            DownloadBounsManager.Setup();
            #endregion

            #region Sự kiện Game
            /// Khởi tạo các sự kiện trong Game
            GameMapEventsManager.Init();

            GuidWarManager.getInstance().Setup();


            #endregion
        }

        /// <summary>
        /// Khởi tạo dữ liệu sau khi tải danh sách bản đồ
        /// </summary>
        public static void InitAfterLoadingMap()
        {
            /// Tải danh sách tự tìm đường
            Console.WriteLine("Initializing AutoPath...");
            KTAutoPathManager.Init();

            /// Thông báo thông tin số luồng hệ thống
            KTMain.ReportThreadsCount();
        }

        /// <summary>
        /// Thông báo thông tin số luồng của hệ thống
        /// </summary>
        private static void ReportThreadsCount()
        {
            Console.WriteLine("============SYSTEM THREADING REPORTS============");
            ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine("Thread pool limitation: Workers = {0}, CompletionPorts = {1}", workerThreads, completionPortThreads);
            Console.WriteLine("================================================");
        }
    }
}
