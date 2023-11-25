using GameServer.KiemThe.Core.Trade;
using GameServer.Logic.LoginWaiting;
using Server.Tools;
using System.Collections.Concurrent;
using System.Collections.Generic;


using Tmsk.Contract;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý toàn bộ trò chơi
    /// </summary>
    public class GameManager
    {
        #region Quản lý bản đồ

        /// <summary>
        /// Kích thước lưới
        /// </summary>
        public static int GridSize { get; set; } = 20;

        #endregion 

        #region Quản lý application

        /// <summary>
        /// 程序主窗口
        /// </summary>
        public static Program AppMainWnd = null;

        /// <summary>
        /// Bản đồ làng mặc định
        /// </summary>
        public static int ClientCore = 0;

        /// <summary>
        /// Bản đồ làng mặc định
        /// </summary>
        public static int ClientResVer = 0;

        /// <summary>
        /// Bản đồ làng mặc định
        /// </summary>
        public static int DefaultMapCode = 1;

        /// <summary>
        /// Bản đồ thành phố chính mặc định
        /// </summary>
        public static int MainMapCode = 2;

        public static int HttpServiceCode = 8888;

        // 新增加一个默认的地图编号 DefaultMapCode 这个在配置文件里面配成6090 新手场景了 [12/12/2013 LiaoWei]
        /// <summary>
        /// Bản đồ mặc định
        /// </summary>
        public static int NewDefaultMapCode = 1;

        /// <summary>
        /// Server线路ID
        /// </summary>
        public static int ServerLineID = 1;

        public static string ActiveGiftCodeUrl = "";

        public static string KTCoinService = "http://sdk.kt2009.mobi:8887/UserKTCoinService.aspx";


        /// <summary>
        /// 代表所有线路,包括跨服线路
        /// </summary>
        public const int ServerLineIdAllIncludeKuaFu = 0;

        /// <summary>
        /// 代表所有线路,包括跨服线路,但不包括自己
        /// </summary>
        public const int ServerLineIdAllLineExcludeSelf = -1000;

        /// <summary>
        /// 本服务器的ID,当前等于区号
        /// 数据库操作相关函数的serverId参数,为避免这个值为未正确初始化而影响使用,用LocalServerId,不要用这个值
        /// </summary>
        public static int ServerId = 1;

        /// <summary>
        /// 平台类型
        /// </summary>
        public static PlatformTypes PlatformType = PlatformTypes.APP;

        /// <summary>
        /// 自动给予到仓库的物品的ID列表
        /// </summary>
        public static List<int> AutoGiveGoodsIDPortableList = null;

        /// <summary>
        /// Tự động cho đến kho vật phẩm ID Liệt biểu
        /// </summary>
        public static List<int> AutoGiveGoodsIDList = null;

        /// <summary>
        /// 最大的九宫格间隔时间
        /// </summary>
        public static int MaxSlotOnUpdate9GridsTicks = 1000;

        /// <summary>
        /// 最大的休眠时间
        /// </summary>
        public static int MaxSleepOnDoMapGridMoveTicks = 5;

        /// <summary>
        /// 最大的缓存怪物发送给客户端的对象数据时间
        /// </summary>
        public static int MaxCachingMonsterToClientBytesDataTicks = (30 * 1000);

        /// <summary>
        /// 最大的缓存客户端发送给客户端的对象数据时间
        /// </summary>
        public static int MaxCachingClientToClientBytesDataTicks = (30 * 1000);

        /// <summary>
        /// 更新九宫格的模式
        /// </summary>
        public static int Update9GridUsingPosition = 1;

        /// <summary>
        /// 更新移动位置时九宫格的时间
        /// </summary>
        public static int MaxSlotOnPositionUpdate9GridsTicks = 2000;

        /// <summary>
        /// 是否启用新的驱动的九宫格模式
        /// </summary>
        public const int Update9GridUsingNewMode = 0;

        /// <summary>
        /// 启用RoleDataMin模式
        /// </summary>
        public static int RoleDataMiniMode = 1;

        /// <summary>
        /// 是否启用多段攻击
        /// </summary>
        public const bool FlagManyAttack = true;

        /// <summary>
        /// 优化多段攻击代码
        /// </summary>
        public const bool FlagManyAttackOp = true;

        /// <summary>
        /// 是否启用锁优化(将锁和Socket绑定)
        /// </summary>
        public const bool FlagOptimizeLock = true;

        /// <summary>
        /// 去掉发送完成时锁BuffLock的过程
        /// </summary>
        public const bool FlagOptimizeLock2 = true;

        /// <summary>
        /// TCPSession锁
        /// </summary>
        public const bool FlagOptimizeLock3 = true;

        /// <summary>
        /// 优化路径字符串处理的消耗
        /// </summary>
        public const bool FlagOptimizePathString = false;

        /// <summary>
        /// 是否禁用记录socket错误状态计数
        /// </summary>
        public const bool FlagOptimizeLockTrace = false;

        /// <summary>
        /// 优化一些运算过程
        /// </summary>
        public const bool FlagOptimizeAlgorithm = true;

        /// <summary>
        /// 线程绑定的缓存(内存)池,本线程用,本线程还,不会跨线程
        /// </summary>
        public const bool FlagOptimizeThreadPool = true;

        /// <summary>
        /// 线程绑定的缓存(内存)池,会跨线程还回
        /// </summary>
        public const bool FlagOptimizeThreadPool2 = false;

        /// <summary>
        /// 线程绑定的缓存(参数)池,会跨线程还回
        /// </summary>
        public const bool FlagOptimizeThreadPool3 = true;

        /// <summary>
        /// 优化BuffLock的锁占用时间
        /// </summary>
        public const bool FlagOptimizeThreadPool4 = true;

        /// <summary>
        /// 优化SendBuff的内存块申请和还回次数,这项优化要求必须开启FlagOptimizeThreadPool4
        /// </summary>
        public const bool FlagOptimizeThreadPool5 = true;

        /// <summary>
        /// 测试参数,禁用所有发送逻辑
        /// </summary>
        public const bool FlagSkipSendDataCall = false;

        /// <summary>
        /// 测试参数,禁用调用AddBuff函数
        /// </summary>
        public const bool FlagSkipAddBuffCall = false;

        /// <summary>
        /// 测试参数,禁用调用TrySend函数
        /// </summary>
        public const bool FlagSkipTrySendCall = false;

        /// <summary>
        /// 测试参数,禁用发送调用
        /// </summary>
        public const bool FlagSkipSocketSend = false;

        /// <summary>
        /// 内存泄漏检测
        /// </summary>
        public const bool FlagTraceMemoryPool = false;

        /// <summary>
        /// 详细记录一些信息
        /// </summary>
        public const bool FlagTraceTCPEvent = false;

        /// <summary>
        /// 详细属性信息
        /// </summary>
        public const bool FlagTracePropsValues = false;

        /// <summary>
        /// 是否禁用名字服务器
        /// </summary>
        public const bool FlagDisableNameServer = true;

        /// <summary>
        /// 不能跳过的发送包数
        /// </summary>
        public const int CostSkipSendCount = 900;

        /// <summary>
        /// 测试特别问题时使用
        /// </summary>
        public static int FlagSleepTime = 0;

        public static int FlagLiXianGuaJi = 0;

        /// <summary>
        /// 跨服切换时，检查服务器时间是否不同，指定0点前后一段时间（分钟数）
        /// </summary>
        public static int ConstCheckServerTimeDiffMinutes = 3;

        /// <summary>
        /// 指令时间统计记录模式,0 记录较大的时间(不太准确),1 记录所有的指令的时间(不太准确),2 精确记录
        /// </summary>
        public static int StatisticsMode = 1;

        /// <summary>
        /// 优化背包整理性能（禁止保存仅格子位置变更的数据到数据库）
        /// </summary>
        public static bool Flag_OptimizationBagReset = true;

        /// <summary>
        /// 配置文件中的配置,内存池各内存块缓存的数量(Size,Num)
        /// </summary>
        public static Dictionary<int, int> MemoryPoolConfigDict = new Dictionary<int, int>();

        /// <summary>
        /// 开启压力测试模式,在此期间登录的帐号视为压力测试帐号
        /// </summary>
        public static bool TestGamePerformanceMode = false;

        public static List<System.Windows.Point> TestBirthPointList1 = new List<System.Windows.Point>();
        public static List<System.Windows.Point> TestBirthPointList2 = new List<System.Windows.Point>();

        /// <summary>
        /// 压力测试,且测试新手场景
        /// </summary>
        public static int TestGamePerformanceMapCode = 1;

        public static int TestGamePerformanceMapMode = 0; //压测模式: 0 指定地图, 1 新手场景, 2 多主线地图, 3 剧情副本地图
        public static bool TestGamePerformanceAllPK = false; //开启全体PK模式
        public static bool TestGamePerformanceLockLifeV = true; //是否锁定血量,不见血
        public static bool TestGamePerformanceForAllUser = false; //为所有角色开启压测模式

        public static bool FlagUseWin32Decrypt = false;//true;

        /// <summary>
        /// 是否显示假人
        /// </summary>
        public static bool TestGameShowFakeRoleForUser = false;

        /// <summary>
        /// 压测帐号的装备列表
        /// </summary>
        public static int[][] TestRoleEquipsArrays = new int[4][]
        {
            new int[]{ 1005005, 1005005, 1000105, 1000005, 1000505, 1000205, 1000605, 1000605, 1000305, 1000405, /*套装*/1032212/*12阶护符*/},
            new int[]{ 1015105, /*单手*/ 1000105, 1010005, 1010505, 1010205, 1010605, 1010605, 1010305, 1010405, /*套装*/1032212/*12阶护符*/},
            new int[]{ 1025405, 1025505, 1020105, 1020005, 1020505, 1020205, 1020605, 1020605, 1020305, 1020405, /*套装*/1032212/*12阶护符*/},
            new int[]{ 1005006, 1005006, 1000106, 1000006, 1000506, 1000206, 1000606, 1000606, 1000306, 1000406, /*套装*/1032212/*12阶护符*/},
        };

        /// <summary>
        /// 暂时禁用token时间验证的剩余毫秒数，供测试调服务器时间做测试用
        /// </summary>
        public static long GM_NoCheckTokenTimeRemainMS = 0;

        /// <summary>
        /// 优化属性计算
        /// </summary>
        public const bool FlagOptimizeAlgorithm_Props = true;

        /// <summary>
        /// 是否允许未注册指令(版本兼容选项)
        /// </summary>
#if BetaConfigMy
        public static bool FlagAlowUnRegistedCmd = true;
#else
        public static bool FlagAlowUnRegistedCmd = false;
#endif

        public const bool FlagEnableMultiLineServer = false;

        /// <summary>
        /// 本地服务器伪ID
        /// </summary>
        public const int LocalServerId = 0;

        /// <summary>
        /// 本地服务器伪ID
        /// </summary>
        public const int LocalServerIdForNotImplement = 0;

        /// <summary>
        /// 是否检查地图不匹配状态（外挂修改）
        /// </summary>
        public static bool CheckMismatchMapCode = true;

        /// <summary>
        /// 是否检查玩家伪造坐标瞬移
        /// </summary>
        public static bool CheckCheatPosition = true;

        /// <summary>
        /// 玩家速度判断周期
        /// </summary>
        public static double CheckPositionInterval = 5000;

        /// <summary>
        /// 每次移动的最大距离（本机测试小于300，考虑到网络延时，放宽条件）
        /// </summary>
        public static double CheckCheatMaxDistance = 600;

        /// <summary>
        /// 服务器启动完毕
        /// </summary>
        public static bool ServerStarting = true;

        /// <summary>
        /// 攻击/施法时,禁止移动
        /// </summary>
        public static bool FlagDisableMovingOnAttack = true;

        /// <summary>
        /// 过滤重复的怪物死亡事件
        /// </summary>
        public static bool FlaDisablegFilterMonsterDeadEvent = false;

        /// <summary>
        /// 跨服活动服务器明确的只能跨服登录进行跨服活动
        /// </summary>
        public static bool FlagKuaFuServerExplicit = true;

        /// <summary>
        /// 是否跨服服务器
        /// </summary>
        public static bool IsKuaFuServer = false;

        /// <summary>
        /// 开启隐藏效果的地图
        /// </summary>
        public static ConcurrentDictionary<int, int> HideFlagsMapDict = new ConcurrentDictionary<int, int>();

        /// <summary>
        ///
        /// </summary>
        public static bool FlagEnableHideFlags = false;

        /// <summary>
        /// 启用隐藏效果的地图编号(仅攻击怪物)
        /// </summary>
        public static int FlagHideFlagsType = 0;

        static GameManager()
        {
            ResetHideFlagsMaps(true, 1);
        }

        public static void ResetHideFlagsMaps(bool enable, int type)
        {
            try
            {
                HideFlagsMapDict.Clear();
                FlagEnableHideFlags = enable;
                FlagHideFlagsType = type;
                HideFlagsMapDict.TryAdd(3000, 1);
                if (FlagHideFlagsType == 1)
                {
                    HideFlagsMapDict.TryAdd(5300, 1);
                    HideFlagsMapDict.TryAdd(5301, 1);
                    HideFlagsMapDict.TryAdd(5302, 1);
                    HideFlagsMapDict.TryAdd(5303, 1);
                    HideFlagsMapDict.TryAdd(7000, 1);
                    HideFlagsMapDict.TryAdd(7001, 1);
                    HideFlagsMapDict.TryAdd(7002, 1);
                    HideFlagsMapDict.TryAdd(7003, 1);
                }
            }
            catch (System.Exception ex)
            {
                LogManager.WriteException(ex.ToString());
            }
        }

        #endregion 全局变量

        #region Quản lý các thành phần

        /// <summary>
        /// 暂时关闭皇城战和王城战功能,防止和罗兰城战冲突
        /// </summary>
        public const int OPT_ChengZhanType = 1;

        /// <summary>
        /// 军旗配置方式
        /// </summary>
        public const bool OPT_OldJuQiConfig = true;

        /// <summary>
        /// 开启货币日志记录
        /// </summary>
        public static bool FlagEnableMoneyEventLog = true;

        /// <summary>
        /// 开启道具日志
        /// </summary>
        public static bool FlagEnableGoodsEventLog = true;

        /// <summary>
        /// 开启活动日志
        /// </summary>
        public static bool FlagEnableGameEventLog = true;

        /// <summary>
        /// 开启操作日志
        /// </summary>
        public static bool FlagEnableOperatorEventLog = true;

        /// <summary>
        /// 开启技能相关日志
        /// </summary>
        public static bool FlagEnableRoleSkillLog = true;

        public static bool FlagEnablePetSkillLog = true;
        public static bool FlagEnableUnionPalaceLog = true;

        public static void SetLogFlags(long flags)
        {
            FlagEnableMoneyEventLog = ((flags & 1) != 0);
            FlagEnableGoodsEventLog = ((flags & 2) != 0);
            FlagEnableGameEventLog = ((flags & 4) != 0);
            FlagEnableOperatorEventLog = ((flags & 8) != 0);
            FlagEnableRoleSkillLog = ((flags & 16) != 0);
            FlagEnablePetSkillLog = ((flags & 32) != 0);
            FlagEnableUnionPalaceLog = ((flags & 64) != 0);
        }

        /// <summary>
        /// 重新计算角色属性的最短时间
        /// </summary>
        public static long FlagRecalcRolePropsTicks = 700;

        /// <summary>
        /// 校验客户端位置
        /// </summary>
        public static int FlagCheckCmdPosition = 1;

        /// <summary>
        /// 从数据库更新配置变量
        /// </summary>
        public static void LoadGameConfigFlags()
        {
            FlagRecalcRolePropsTicks = GameManager.GameConfigMgr.GetGameConfigItemInt("recalcrolepropsticks", 700);
            FlagCheckCmdPosition = GameManager.GameConfigMgr.GetGameConfigItemInt(GameConfigNames.check_cmd_position, 1);
        }

        #endregion 功能开关

        #region 全局对象

        /// <summary>
        /// 在线用户回话管理对象
        /// </summary>
        public static UserSession OnlineUserSession = new UserSession();

        /// <summary>
        /// 怪物的ID生成对象
        /// </summary>
        public static MonsterIDManager MonsterIDMgr = new MonsterIDManager();

        /// <summary>
        /// 物品掉落管理
        /// </summary>
        public static GoodsPackManager GoodsPackMgr = new GoodsPackManager();

      

        /// <summary>
        /// 地图管理对象
        /// </summary>
        public static MapManager MapMgr = new MapManager();

        /// <summary>
        /// 地图格子管理对象
        /// </summary>
        public static MapGridManager MapGridMgr = new MapGridManager();

        /// <summary>
        /// 在线客户的管理对象
        /// </summary>
        public static ClientManager ClientMgr = new ClientManager();

        /// <summary>
        /// 地图爆怪区域管理类
        /// </summary>
        public static MonsterZoneManager MonsterZoneMgr = new MonsterZoneManager();

        /// <summary>
        /// 地图爆怪管理对象
        /// </summary>
        public static MonsterManager MonsterMgr = new MonsterManager();

        /// <summary>
        /// 数据库命令队列管理
        /// </summary>
        public static DBCmdManager DBCmdMgr = new DBCmdManager();

        /// <summary>
        /// 日志数据库命令队列管理
        /// </summary>
        public static LogDBCmdManager logDBCmdMgr = new LogDBCmdManager();

        /// <summary>
        /// NPC和任务的映射管理
        /// </summary>
        public static NPCTasksManager NPCTasksMgr = new NPCTasksManager();

        public static GoodsExchangeManager GoodsExchangeMgr = new GoodsExchangeManager();

      

  
        public static GameConfig GameConfigMgr = new GameConfig();

     

      


        public static LoginWaitLogic loginWaitLogic = new LoginWaitLogic();

        /// <summary>
        /// Cpu And Memory
        /// </summary>
        public static ServerMonitorManager ServerMonitor = new ServerMonitorManager();

        /// <summary>
        /// 上次刷怪时间  TimeUtil.NOW()
        /// </summary>
        public static long LastFlushMonsterMs;

        #endregion 全局对象

        #region Xml缓存对象

        /// <summary>
        /// NPC列表管理
        /// </summary>
        public static SystemXmlItems SystemNPCsMgr = new SystemXmlItems();

        /// <summary>
        /// 系统操作列表管理
        /// </summary>
        public static SystemXmlItems SystemOperasMgr = new SystemXmlItems();

        /// <summary>
        /// 技能列表管理
        /// </summary>
        public static SystemXmlItems SystemMagicsMgr = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品列表xml管理
        /// </summary>
        public static SystemXmlItems SystemMonsterGoodsList = new SystemXmlItems();

        /// <summary>
        /// 限制时间爆怪的物品列表xml管理
        /// </summary>
        public static SystemXmlItems SystemLimitTimeMonsterGoodsList = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品品质ID列表管理
        /// </summary>
        public static SystemXmlItems SystemGoodsQuality = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品级别ID列表管理
        /// </summary>
        public static SystemXmlItems SystemGoodsLevel = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品天生ID列表管理
        /// </summary>
        public static SystemXmlItems SystemGoodsBornIndex = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品追加ID列表管理
        /// </summary>
        public static SystemXmlItems SystemGoodsZhuiJia = new SystemXmlItems();

        /// <summary>
        /// 爆怪的物品卓越ID列表管理
        /// </summary>
        public static SystemXmlItems SystemGoodsExcellenceProperty = new SystemXmlItems();

        /// <summary>
        /// 炎黄战场的调度xml管理
        /// </summary>
        public static SystemXmlItems SystemBattle = new SystemXmlItems();

        /// <summary>
        /// 阵营战的排名奖励表
        /// </summary>
        public static SystemXmlItems SystemBattlePaiMingAwards = new SystemXmlItems();

        /// <summary>
        /// 竞技场决斗赛的调度xml管理
        /// </summary>
        public static SystemXmlItems SystemArenaBattle = new SystemXmlItems();

        /// <summary>
        /// NPC功能脚本列表管理
        /// </summary>
        public static SystemXmlItems systemNPCScripts = new SystemXmlItems();

        /// <summary>
        /// 宠物列表管理
        /// </summary>
        public static SystemXmlItems systemPets = new SystemXmlItems();

        /// <summary>
        /// 坐骑数据字典
        /// </summary>
        public static Dictionary<int, SystemXmlItems> SystemHorseDataDict = new Dictionary<int, SystemXmlItems>();

        /// <summary>
        /// 物品合成类型列表管理
        /// </summary>
        public static SystemXmlItems systemGoodsMergeTypes = new SystemXmlItems();

        /// <summary>
        /// 物品合成类型项管理
        /// </summary>
        public static SystemXmlItems systemGoodsMergeItems = new SystemXmlItems();

        /// <summary>
        /// 闭关收益表
        /// </summary>
        public static SystemXmlItems systemBiGuanMgr = new SystemXmlItems();

        /// <summary>
        /// 商城物品列表
        /// </summary>
        public static SystemXmlItems systemMallMgr = new SystemXmlItems();

        /// <summary>
        /// 冲穴经验收益表
        /// </summary>
        public static SystemXmlItems systemJingMaiExpMgr = new SystemXmlItems();

        /// <summary>
        /// 物品包配置管理
        /// </summary>
        public static SystemXmlItems systemGoodsBaoGuoMgr = new SystemXmlItems();

        /// <summary>
        /// 挖宝设置表
        /// </summary>
        public static SystemXmlItems systemWaBaoMgr = new SystemXmlItems();

        /// <summary>
        /// 周连续登录送礼配置表
        /// </summary>
        public static SystemXmlItems systemWeekLoginGiftMgr = new SystemXmlItems();

        /// <summary>
        /// 当月在线时长送礼配置表
        /// </summary>
        public static SystemXmlItems systemMOnlineTimeGiftMgr = new SystemXmlItems();

        /// <summary>
        /// 新手见面送礼配置表
        /// </summary>
        public static SystemXmlItems systemNewRoleGiftMgr = new SystemXmlItems();

        /// <summary>
        /// 升级有礼配置表
        /// </summary>
        public static SystemXmlItems systemUpLevelGiftMgr = new SystemXmlItems();

        /// <summary>
        /// 副本配置表
        /// </summary>
        public static SystemXmlItems systemFuBenMgr = new SystemXmlItems();

        /// <summary>
        /// 押镖配置表
        /// </summary>
        public static SystemXmlItems systemYaBiaoMgr = new SystemXmlItems();

        /// <summary>
        /// 特殊的时间表
        /// </summary>
        public static SystemXmlItems systemSpecialTimeMgr = new SystemXmlItems();

        /// <summary>
        /// 英雄逐擂配置表
        /// </summary>
        public static SystemXmlItems systemHeroConfigMgr = new SystemXmlItems();

        /// <summary>
        /// 帮旗升级配置表
        /// </summary>
        public static SystemXmlItems systemBangHuiFlagUpLevelMgr = new SystemXmlItems();

        /// <summary>
        /// 帮旗属性配置表
        /// </summary>
        public static SystemXmlItems systemJunQiMgr = new SystemXmlItems();

        /// <summary>
        /// 旗座位置配置表
        /// </summary>
        public static SystemXmlItems systemQiZuoMgr = new SystemXmlItems();

        /// <summary>
        /// 领地所属地图旗帜配置表
        /// </summary>
        public static SystemXmlItems systemLingQiMapQiZhiMgr = new SystemXmlItems();

        /// <summary>
        /// 奇珍阁物品配置表
        /// </summary>
        public static SystemXmlItems systemQiZhenGeGoodsMgr = new SystemXmlItems();

        /// <summary>
        /// 皇城复活点配置表
        /// </summary>
        public static SystemXmlItems systemHuangChengFuHuoMgr = new SystemXmlItems();

        /// <summary>
        /// 隋唐战场定时经验表
        /// </summary>
        public static SystemXmlItems systemBattleExpMgr = new SystemXmlItems();

        /// <summary>
        /// 皇城，血战地府，领地战定时给予的收益表
        /// </summary>
        public static SystemXmlItems systemBangZhanAwardsMgr = new SystemXmlItems();

        /// <summary>
        /// 隋唐战场出生点表
        /// </summary>
        public static SystemXmlItems systemBattleRebirthMgr = new SystemXmlItems();

        /// <summary>
        /// 隋唐战场奖励表
        /// </summary>
        public static SystemXmlItems systemBattleAwardMgr = new SystemXmlItems();

        /// <summary>
        /// 装备天生洗练表
        /// </summary>
        public static SystemXmlItems systemEquipBornMgr = new SystemXmlItems();

        /// <summary>
        /// 装备天生属性级别名称表
        /// </summary>
        public static SystemXmlItems systemBornNameMgr = new SystemXmlItems();

        /// <summary>
        /// Vip每日奖励缓存表
        /// </summary>
        public static SystemXmlItems systemVipDailyAwardsMgr = new SystemXmlItems();

        /// <summary>
        /// 活动引导提示缓存表
        /// </summary>
        public static SystemXmlItems systemActivityTipMgr = new SystemXmlItems();

        /// <summary>
        /// 杨公宝库幸运值奖励缓存表
        /// </summary>
        public static SystemXmlItems systemLuckyAwardMgr = new SystemXmlItems();

        /// <summary>
        /// 幸运金蛋幸运值奖励缓存表
        /// </summary>
        public static SystemXmlItems systemLuckyAward2Mgr = new SystemXmlItems();

        /// <summary>
        /// 杨公宝库幸运值规则表
        /// </summary>
        public static SystemXmlItems systemLuckyMgr = new SystemXmlItems();

        /// <summary>
        /// 成就配置管理
        /// </summary>
        public static SystemXmlItems systemChengJiu = new SystemXmlItems();

        /// <summary>
        /// 成就Buffer配置管理
        /// </summary>
        public static SystemXmlItems systemChengJiuBuffer = new SystemXmlItems();

        /// <summary>
        /// 武器通灵配置管理
        /// </summary>
        public static SystemXmlItems systemWeaponTongLing = new SystemXmlItems();

        /// <summary>
        /// 乾坤袋配置管理
        /// </summary>
        //public static SystemXmlItems systemQianKunMgr = new SystemXmlItems();

        /// <summary>
        /// 祈福分级配置管理 [8/28/2014 LiaoWei]
        /// </summary>
        public static SystemXmlItems systemImpetrateByLevelMgr = new SystemXmlItems();

        /// <summary>
        /// 幸运抽奖配置管理
        /// </summary>
        public static SystemXmlItems systemXingYunChouJiangMgr = new SystemXmlItems();

        /// <summary>
        /// 月度抽奖配置管理
        /// </summary>
        public static SystemXmlItems systemYueDuZhuanPanChouJiangMgr = new SystemXmlItems();

        /// <summary>
        /// 每日在线奖励管理 [1/12/2014 LiaoWei]
        /// </summary>
        public static SystemXmlItems systemEveryDayOnLineAwardMgr = new SystemXmlItems();

        /// <summary>
        /// 连续登陆奖励管理 [1/17/2014 LiaoWei]
        /// </summary>
        public static SystemXmlItems systemSeriesLoginAwardMgr = new SystemXmlItems();

        /// <summary>
        /// 怪物管理
        /// </summary>
        public static SystemXmlItems systemMonsterMgr = new SystemXmlItems();

        /// <summary>
        /// 经脉等级管理
        /// </summary>
        public static SystemXmlItems SystemJingMaiLevel = new SystemXmlItems();

        /// <summary>
        /// 武学等级管理
        /// </summary>
        public static SystemXmlItems SystemWuXueLevel = new SystemXmlItems();

        /// <summary>
        /// 过场动画文件管理
        /// </summary>
        public static SystemXmlItems SystemTaskPlots = new SystemXmlItems();

        /// <summary>
        /// 抢购管理
        /// </summary>
        public static SystemXmlItems SystemQiangGou = new SystemXmlItems();

        /// <summary>
        /// 合服抢购管理
        /// </summary>
        public static SystemXmlItems SystemHeFuQiangGou = new SystemXmlItems();

        /// <summary>
        /// 节日抢购管理
        /// </summary>
        public static SystemXmlItems SystemJieRiQiangGou = new SystemXmlItems();

        /// <summary>
        /// 钻皇等级管理
        /// </summary>
        public static SystemXmlItems SystemZuanHuangLevel = new SystemXmlItems();

        /// <summary>
        /// 系统激活项管理
        /// </summary>
        public static SystemXmlItems SystemSystemOpen = new SystemXmlItems();

        /// <summary>
        /// 系统掉落金钱管理管理
        /// </summary>
        public static SystemXmlItems SystemDropMoney = new SystemXmlItems();

        /// <summary>
        /// 系统限时连续登录送大礼活动配置文件
        /// </summary>
        public static SystemXmlItems SystemDengLuDali = new SystemXmlItems();

        /// <summary>
        /// 系统限时补偿活动配置文件
        /// </summary>
        public static SystemXmlItems SystemBuChang = new SystemXmlItems();

        /// <summary>
        /// 战魂等级管理
        /// </summary>
        public static SystemXmlItems SystemZhanHunLevel = new SystemXmlItems();

        /// <summary>
        /// 荣誉等级管理
        /// </summary>
        public static SystemXmlItems SystemRongYuLevel = new SystemXmlItems();

        //         /// <summary>
        //         /// 军衔等级管理
        //         /// </summary>
        //         public static SystemXmlItems SystemShengWangLevel = new SystemXmlItems();

        /// <summary>
        /// 魔晶和祈福兑换
        /// </summary>
        public static SystemXmlItems SystemExchangeMoJingAndQiFu = new SystemXmlItems();

        /// <summary>
        /// 冥想
        /// </summary>
        //public static SystemXmlItems SystemMeditateInfo = new SystemXmlItems();

        /// <summary>
        /// 每日活跃信息配置管理
        /// </summary>
        public static SystemXmlItems systemDailyActiveInfo = new SystemXmlItems();

        /// <summary>
        /// 每日活跃奖励配置管理
        /// </summary>
        public static SystemXmlItems systemDailyActiveAward = new SystemXmlItems();

        /// <summary>
        /// 天使神殿配置数据
        /// </summary>
        public static SystemXmlItems systemAngelTempleData = new SystemXmlItems();

        /// <summary>
        /// 天使神殿排名奖励表
        /// </summary>
        public static SystemXmlItems AngelTempleAward = new SystemXmlItems();

        /// <summary>
        /// 天使神殿幸运奖励表
        /// </summary>
        public static SystemXmlItems AngelTempleLuckyAward = new SystemXmlItems();

        /// <summary>
        /// 任务章节
        /// </summary>
        public static SystemXmlItems TaskZhangJie = new SystemXmlItems();

        /// <summary>
        /// 任务ID区间对应的这个任务需要加成的章节属性配置
        /// </summary>
        public static List<RangeKey> TaskZhangJieDict = new List<RangeKey>();

        /// <summary>
        /// 交易物品类别表
        /// </summary>
        public static SystemXmlItems JiaoYiTab = new SystemXmlItems();

        /// <summary>
        /// 交易物品类别定义表
        /// </summary>
        public static SystemXmlItems JiaoYiType = new SystemXmlItems();

        /// <summary>
        /// 战盟建设
        /// </summary>
        public static SystemXmlItems SystemZhanMengBuild = new SystemXmlItems();

        /// <summary>
        /// 翅膀进阶配置表
        /// </summary>
        public static SystemXmlItems SystemWingsUp = new SystemXmlItems();

        /// <summary>
        /// Boss AI配置表
        /// </summary>
        public static SystemXmlItems SystemBossAI = new SystemXmlItems();

        /// <summary>
        /// 拓展属性配置表
        /// </summary>
        public static SystemXmlItems SystemExtensionProps = new SystemXmlItems();

        /// <summary>
        /// 采集物管理
        /// </summary>
        public static SystemXmlItems systemCaiJiMonsterMgr = new SystemXmlItems();

        /// <summary>
        /// 精灵升级xml管理
        /// </summary>
        public static SystemXmlItems SystemDamonUpgrade = new SystemXmlItems();

        #endregion Xml缓存对象

        #region 事件日志对象

        /// <summary>
        /// 服务器端普通日志事件
        /// </summary>
        public static ServerEvents SystemServerEvents = new ServerEvents() { EventRootPath = "Events", EventPreFileName = "Event" };

        /// <summary>
        /// 服务器端角色登录日志事件
        /// </summary>
        public static ServerEvents SystemRoleLoginEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Login" };

        /// <summary>
        /// 服务器端角色登出日志事件
        /// </summary>
        public static ServerEvents SystemRoleLogoutEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Logout" };

        /// <summary>
        /// 服务器端角色完成任务日志事件
        /// </summary>
        public static ServerEvents SystemRoleTaskEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Task" };

        /// <summary>
        /// 服务器端角色死亡日志事件
        /// </summary>
        public static ServerEvents SystemRoleDeathEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Death" };

        /// <summary>
        /// 服务器端角色铜钱购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithTongQianEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "TongQianBuy" };

        /// <summary>
        /// 服务器端角色银两购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithBoundTokenEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "BoundTokenBuy" };

        /// <summary>
        /// 服务器端角色军贡购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithJunGongEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "JunGongBuy" };

        /// <summary>
        /// 服务器端角色银票购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithYinPiaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "YinPiaoBuy" };

        /// <summary>
        /// 服务器端角色元宝购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithYuanBaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "YuanBaoBuy" };

        /// <summary>
        /// 服务器端角色元宝奇珍阁购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleQiZhenGeBuyWithYuanBaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "QiZhenGeBuy" };

        /// <summary>
        /// 服务器端角色元宝商城抢购购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleQiangGouBuyWithYuanBaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "QiangGouBuy" };

        /// <summary>
        /// 服务器端角色出售物品日志事件
        /// </summary>
        public static ServerEvents SystemRoleSaleEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Sale" };

        /// <summary>
        /// 服务器端角色交易日志事件(物品交易)
        /// </summary>
        public static ServerEvents SystemRoleExchangeEvents1 = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Exchange1" };

        /// <summary>
        /// 服务器端角色交易日志事件(银两交易)
        /// </summary>
        public static ServerEvents SystemRoleExchangeEvents2 = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Exchange2" };

        /// <summary>
        /// 服务器端角色交易日志事件(元宝交易)
        /// </summary>
        public static ServerEvents SystemRoleExchangeEvents3 = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Exchange3" };

        /// <summary>
        /// 服务器端升级日志事件
        /// </summary>
        public static ServerEvents SystemRoleUpgradeEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Upgrade" };

        /// <summary>
        /// 服务器端物品相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleGoodsEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Goods" };

        /// <summary>
        /// 服务器端掉落被拾取的物品相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleFallGoodsEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "FallGoods" };

        /// <summary>
        /// 服务器端银两获取和使用的相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleBoundTokenEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "BoundToken" };

        /// <summary>
        /// 服务器端仓库金币获取和使用的相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleStoreBoundTokenEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "StoreBoundToken" };

        /// <summary>
        /// 服务器端仓库绑定金币获取和使用的相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleStoreMoneyEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "StoreMoney" };

        /// <summary>
        /// 服务器端坐骑幸运点日志事件
        /// </summary>
        public static ServerEvents SystemRoleHorseEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Horse" };

        /// <summary>
        /// 服务器端帮贡获取和使用的相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleBangGongEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "BangGong" };

        /// <summary>
        /// 服务器端经脉相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleJingMaiEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "JingMai" };

        /// <summary>
        /// 服务器端刷新奇珍阁日志事件
        /// </summary>
        public static ServerEvents SystemRoleRefreshQiZhenGeEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "RefreshQiZhenGe" };

        /// <summary>
        /// 服务器端挖宝事件
        /// </summary>
        public static ServerEvents SystemRoleWaBaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "WaBao" };

        /// <summary>
        /// 服务器端地图进入事件
        /// </summary>
        public static ServerEvents SystemRoleMapEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Map" };

        /// <summary>
        /// 副本奖励领取事件
        /// </summary>
        public static ServerEvents SystemRoleFuBenAwardEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "FuBenAward" };

        /// <summary>
        /// 五行奇阵奖励领取事件
        /// </summary>
        public static ServerEvents SystemRoleWuXingAwardEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "WuXingAward" };

        /// <summary>
        /// 跑环完成事件
        /// </summary>
        public static ServerEvents SystemRolePaoHuanOkEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "PaoHuanOk" };

        /// <summary>
        /// 押镖事件
        /// </summary>
        public static ServerEvents SystemRoleYaBiaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "YaBiao" };

        /// <summary>
        /// 连斩事件
        /// </summary>
        public static ServerEvents SystemRoleLianZhanEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "LianZhan" };

        /// <summary>
        /// 活动怪物的事件
        /// </summary>
        public static ServerEvents SystemRoleHuoDongMonsterEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "HuoDongMonster" };

        /// <summary>
        /// 服务器端角色精雕细琢[钥匙类]挖宝事件
        /// </summary>
        public static ServerEvents SystemRoleDigTreasureWithYaoShiEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "DigTreasureWithYaoShi" };

        /// <summary>
        /// 服务器端自动扣除元宝事件
        /// </summary>
        public static ServerEvents SystemRoleAutoSubYuanBaoEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "AutoSubYuanBao" };

        /// <summary>
        /// 服务器端自动扣除金币事件
        /// </summary>
        public static ServerEvents SystemRoleAutoSubBoundMoneyEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "AutoSubBoundMoney" };

        /// <summary>
        /// 服务器端自动扣除金币-元宝事件
        /// </summary>
        public static ServerEvents SystemRoleAutoSubEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "AutoSub" };

        /// <summary>
        /// 角色提取邮件元宝，银两，铜钱事件
        /// </summary>
        public static ServerEvents SystemRoleFetchMailMoneyEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "MailMoneyFetch" };

        /// <summary>
        /// 服务器端角色天地精元兑换日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithTianDiJingYuanEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "TianDiJingYuanBuy" };

        /// <summary>
        /// 角色Vip奖励的元宝，银两，铜钱, 灵力事件
        /// </summary>
        public static ServerEvents SystemRoleFetchVipAwardEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "VipAwardGet" };

        /// <summary>
        /// 服务器端角色金币购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithBoundMoneyEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "BoundMoneyBuy" };

        /// <summary>
        /// 服务器端金币获取和使用的相关日志事件
        /// </summary>
        public static ServerEvents SystemRoleBoundMoneyEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "BoundMoney" };

        //**************
        /// <summary>
        /// 服务器端角色精元值购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithJingYuanZhiEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "JingYuanZhiBuy" };

        /// <summary>
        /// 服务器端角色猎杀值购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithLieShaZhiEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "LieShaZhiBuy" };

        /// <summary>
        /// 服务器端角色装备积分值购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithZhuangBeiJiFenEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "ZhuangBeiJiFenBuy" };

        /// <summary>
        /// 服务器端角色军功值购买日志事件===》注意，军功值需要和旧的 帮会的 军贡值相区别
        /// </summary>
        public static ServerEvents SystemRoleBuyWithJunGongZhiEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "JunGongZhiBuy" };

        /// <summary>
        /// 服务器端角色战魂值购买日志事件
        /// </summary>
        public static ServerEvents SystemRoleBuyWithZhanHunEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "ZhanHunBuy" };

        /// <summary>
        /// 服务器端角色元宝日志事件
        /// </summary>
        public static ServerEvents SystemRoleTokenEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "Token" };

        /// <summary>
        /// 服务器端角色活动类日志事件
        /// </summary>
        public static ServerEvents SystemRoleGameEvents = new ServerEvents() { EventRootPath = "RoleEvents", EventPreFileName = "RoleGame" };

        /// <summary>
        /// 服务器端全局活动类日志事件
        /// </summary>
        public static ServerEvents SystemGlobalGameEvents = new ServerEvents() { EventRootPath = "Events", EventPreFileName = "GameLog" };

        #endregion 事件日志对象
    }
}