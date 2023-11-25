using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 坐骑的扩展属性索引定义
    /// </summary>
    public enum HorseExtIndexes
    {
        /// <summary>
        /// 物理攻击
        /// </summary>
        Attack = 0,

        /// <summary>
        /// 物理防御
        /// </summary>
        Defense = 1,

        /// <summary>
        /// 魔法攻击
        /// </summary>
        MAttack = 2,

        /// <summary>
        /// 魔法防御
        /// </summary>
        MDefense = 3,

        /// <summary>
        /// 暴击
        /// </summary>
        Burst = 4,

        /// <summary>
        /// 命中
        /// </summary>
        Hit = 5,

        /// <summary>
        /// 闪避
        /// </summary>
        Dodge = 6,

        /// <summary>
        /// 生命值上限的次数
        /// </summary>
        MaxLife = 7,

        /// <summary>
        /// 魔法值上限的次数
        /// </summary>
        MaxMagic = 8,

        /// <summary>
        /// 最大值
        /// </summary>
        MaxVal,
    }

    /// <summary>
    /// 返回出售中的物品列表的最大数
    /// </summary>
    public enum SaleGoodsConsts
    {
        /// <summary>
        /// 出售中的物品的ID
        /// </summary>
        SaleGoodsID = -1,

        /// <summary>
        /// 随身仓库中的物品ID
        /// </summary>
        PortableGoodsID = -1000,

        /// <summary>
        /// 同时出售的物品数量
        /// </summary>
        MaxSaleNum = 16,

        /// <summary>
        /// 返回列表的最大数量
        /// </summary>
        MaxReturnNum = 250,

        /// <summary>
        /// 金蛋仓库位置【0表示背包，-1000表示随身仓库，这个值2000表示砸金蛋的仓库】
        /// </summary>
        JinDanGoodsID = 2000,

        /// <summary>
        /// 元素之心背包
        /// </summary>
        ElementhrtsGoodsID = 3000,

        /// <summary>
        /// 元素之心装备栏
        /// </summary>
        UsingElementhrtsGoodsID = 3001,

        /// <summary>
        /// 精灵装备栏
        /// </summary>
        PetBagGoodsID = 4000,

        /// <summary>
        /// 精灵装备栏
        /// </summary>
        UsingDemonGoodsID = 5000,

        /// <summary>
        /// 时装装备栏 包括称号和翅膀
        /// </summary>   panghui add
        FashionGoods = 6000,

        /// <summary>
        /// 荧光宝石背包 [XSea 2015/8/6]
        /// </summary>
        FluorescentGemBag = 7000,

        /// <summary>
        /// 荧光宝石装备栏 [XSea 2015/8/6]
        /// </summary>
        FluorescentGemEquip = 7001,

        /// <summary>
        /// 魂石背包
        /// </summary>
        SoulStoneBag = 8000,

        /// <summary>
        /// 魂石装备栏
        /// </summary>
        SoulStoneEquip = 8001,

        /// <summary>
        /// 饰品装备栏
        /// </summary>
        OrnamentGoods = 9000,

        /// <summary>
        /// 特殊的摆摊金币物品ID
        /// </summary>
        BaiTanJinBiGoodsID = 50200,
    }

    /// <summary>
    /// 好友相关常量定义
    /// </summary>
    public enum FriendsConsts
    {
        /// <summary>
        /// 好友上限
        /// </summary>
        MaxFriendsNum = 50,

        /// <summary>
        /// 黑名单上限
        /// </summary>
        MaxBlackListNum = 20,

        /// <summary>
        /// 仇人
        /// </summary>
        MaxEnemiesNum = 20,
    }

    /// <summary>
    /// 排行榜的类型
    /// </summary>
    public enum PaiHangTypes
    {
        None = 0, //无定义
        EquipJiFen = 1, //装备积分
        XueWeiNum = 2, //穴位个数
        SkillLevel = 3, //技能级别
        HorseJiFen = 4, //坐骑积分
        RoleLevel = 5, //角色等级
        RoleYinLiang = 6, //角色银两
        LianZhan = 7, //角色连斩
        KillBoss = 8, //杀BOSS数量
        BattleNum = 9, //角斗场称号次数
        HeroIndex = 10, //英雄逐擂的到达层数
        RoleGold = 11, //角色金币
        CombatForceList = 12, // 战斗力 [12/18/2013 LiaoWei]
        JingJi = 13, //竞技场
        WanMoTa = 14, //万魔塔
        Wing = 15, //翅膀
        Ring = 16, //婚戒
        Merlin = 17, // 梅林魔法书
        MaxVal, //最大值
    }

    /// <summary>
    /// 搜索的返回结果常量定义
    /// </summary>
    public enum SearchResultConsts
    {
        /// <summary>
        /// 搜索角色的返回个数
        /// </summary>
        MaxSearchRolesNum = 10,

        /// <summary>
        /// 搜索队伍的返回个数
        /// </summary>
        MaxSearchTeamsNum = 10,
    };

    /// <summary>
    /// 领地的ID定义
    /// </summary>
    public enum LingDiIDs
    {
        /// <summary>
        /// 扬州城
        /// </summary>
        YanZhou = 1,

        /// <summary>
        /// 皇城
        /// </summary>
        HuangCheng = 2,

        /// <summary>
        /// 幽州城
        /// </summary>
        YouZhou = 3,

        /// <summary>
        /// 太原城
        /// </summary>
        TaiYuan = 4,

        /// <summary>
        /// 荥阳城
        /// </summary>
        XingYang = 5,

        /// <summary>
        /// 皇宫
        /// </summary>
        HuangGong = 6,

        /// <summary>
        /// 最大值
        /// </summary>
        MaxVal = 7,
    };

    /// <summary>
    /// 皇帝特权次数
    /// </summary>
    public enum HuanDiTeQuanNum
    {
        Max = 3,
    }

    /// <summary>
    /// 帮会人数上限
    /// </summary>
    public enum BangHuiNum
    {
        Max = 100,
    }

    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActivityTypes
    {
        None = 0, //无定义
        InputFirst = 1, //首充大礼
        InputFanLi = 2, //充值返利
        InputJiaSong = 3, //充值加送
        InputKing = 4, //充值王
        LevelKing = 5, //冲级王
        EquipKing = 6, //装备王====>修改成boss王
        HorseKing = 7, //坐骑王====>修改成武学王
        JingMaiKing = 8, //经脉王====>采用新的经脉系统计算
        JieriDaLiBao = 9, //大型节日大礼包
        JieriDengLuHaoLi = 10, //节日登录豪礼
        JieriVIP = 11, //VIP大回馈
        JieriCZSong = 12, //节日期间充值大回馈
        JieriLeiJiCZ = 13, //节日期间累计充值
        JieriZiKa = 14, //节日期间字卡换礼盒
        JieriPTXiaoFeiKing = 15, //节日期间平台消费王
        JieriPTCZKing = 16, //节日期间平台充值王
        JieriBossAttack = 17, //节日期间Boss攻城
        //HeFuDaLiBao = 20, //合服大礼包*/
        //HeFuCZSong = 21, //合服充值大回馈
        //HeFuVIP = 22, //合服VIP大回馈
        //HeFuCZFanLi = 23, //合服充值返利
        //HeFuPKKing = 24, //合服PK王
        //HeFuWanChengKing = 25, //合服王城霸主
        //HeFuBossAttack = 26, //合服BOSS攻城

        HeFuLogin = 20, //合服登陆好礼
        HeFuTotalLogin = 21, //合服累计登陆
        HeFuShopLimit = 22, //合服商店限购
        HeFuRecharge = 23, //合服充值返利
        HeFuPKKing = 24, //合服PK王
        HeFuAwardTime = 25,	// 奖励翻倍（为战而生）
        HeFuBossAttack = 26, //BOSS之战

        MeiRiChongZhiHaoLi = 27,  // 每日充值豪礼 [7/15/2013 LiaoWei]
        ChongJiLingQuShenZhuang = 28,   // 冲级领取神装 [7/15/2013 LiaoWei]
        ShenZhuangJiQingHuiKui = 29,  // 神装激情回赠 [7/15/2013 LiaoWei]
        XinCZFanLi = 30, //新的开区充值返利
        XingYunChouJiang = 31,          // 幸运抽奖 [7/15/2013 LiaoWei]
        YuDuZhuanPanChouJiang = 32,          // 月度转盘 [7/15/2013 LiaoWei]

        NewZoneUpLevelMadman = 33, //-----------冲级狂人---new
        NewZoneRechargeKing = 34,//-------------充值达人---new
        NewZoneConsumeKing = 35,//------------- 消费达人---new
        NewZoneBosskillKing = 36,//-------------屠魔勇士---new
        NewZoneFanli = 37,//--------------------劲爆返利---new
        TotalCharge = 38,//累计充值   回馈
        TotalConsume = 39,//累计消费  回馈
        JieriTotalConsume = 40,     // 节日累计消费
        JieriDuoBei = 41,           // 节日多倍奖励
        JieriQiangGou = 42,         // 节日多倍奖励
        HeFuLuoLan = 43,            // 合服的罗兰城主
        SpecActivity = 44,          // 专享活动

        JieriGive = 50,         // 节日赠送
        JieriGiveKing = 51, // 节日赠送王
        JieriRecvKing = 52, //节日收取王
        JieriWing = 53,             // 节日翅膀返利
        JieriAddon = 54,            // 节日追加返利
        JieriStrengthen = 55,       // 节日强化返利
        JieriAchievement = 56,      // 节日成就返利
        JieriMilitaryRank = 57,     // 节日军衔返利
        JieriVIPFanli = 58,         // 节日VIP返利
        JieriAmulet = 59,           // 节日护身符返利
        JieriArchangel = 60,        // 节日大天使返利
        JieriLianXuCharge = 61, // 节日连续充值
        JieriMarriage = 62,         // 节日婚姻返利
        JieriRecv = 63,             // 节日收礼
        JieriInputPointsExchg = 64, // 充值积分兑换

        JieriPlatChargeKing = 100, // 节日平台充值王

        TenReturn = 999, //应用宝老玩家召回
        MaxVal, //最大值
    }

    /// <summary>
    /// VIP特权类型
    /// </summary>
    public enum VipPriorityTypes
    {
        None = 0, //无定义
        GetDailyYuanBao = 1,//每日上线即可免费领取200非绑定元宝
        GetDailyLingLi = 5,//每日上线即可免费领取10000灵力
        GetDailyYinLiang = 6,//每日上线即可免费领取2000银两
        GetDailyAttackFuZhou = 7,//每日免费领取狂攻符咒一个
        GetDailyDefenseFuZhou = 8,//每日免费领取防御符咒一个
        GetDailyLifeFuZhou = 9,//每日免费领取生命符咒一个
        GetDailyTongQian = 10,//每日上线可免费领取100000铜钱
        GetDailyZhenQi = 21,//每日上线可免费领取【幻境阵旗】20个
        MaxVal = 100, //最大值
    }

    /// <summary>
    /// 角色创建常量信息
    /// </summary>
    public enum RoleCreateConstant
    {
        GridNum = 50,//角色创建时背包有50个格子
    }

    /// <summary>
    /// 领地日志数据更新枚举
    /// </summary>
    public enum BuildingLogType
    {
        BuildLog_TaskRole = 0,  // 每日开发资源人数（1人多次计1人)
        BuildLog_Task,          // 每日开发资源次数
        BuildLog_RefreshRole,   // 每日刷新人数（1人多次计1人)
        BuildLog_Refresh,       // 每日刷新使用次数
        BuildLog_OpenRole,      // 每日购买队列人数（1人多次计1人)
        BuildLog_Open,          // 每日购买队列人数
        BuildLog_Push,          // 每日推送次数
        BuildLog_PushUse,       // 每日推送被使用次数
    }


    /// <summary>
    /// 角色参数名称--->注意 长度不能超过16字符！！！！！！！！！！！！！！！！存储内容不能超过60字节!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    public class RoleParamName
    {
        /// <summary>
        /// Chuỗi lưu danh sách danh hiệu của người chơi
        /// </summary>
        public const string RoleTitles = "RoleTitles";

        /// <summary>
        /// Chuỗi lưu thiết lập hệ thống
        /// </summary>
        public const string SystemSettings = "SystemSettings";


        /// <summary>
        /// Lưu thông tin có đang cưỡi ngựa không
        /// </summary>
        public const string HorseToggleOn = "HorseToggleOn";

        /// <summary>
        /// Lưu thông tin người chơi ở phụ bản
        /// </summary>
        public const string CopySceneRecord = "CopySceneRecord";

        /// <summary>
        /// Điểm hồi sinh ở thành thôn tương ứng
        /// </summary>
        public const string DefaultRelivePos = "DefaultRelivePos";

        /// <summary>
        /// Thông tin Bách Bảo Rương
        /// </summary>
        public const string SeashellCircleInfo = "SeashellCircleInfo";

        /// <summary>
        /// Thông tin Chúc phúc
        /// </summary>
        public const string PrayData = "PrayData";

        /// <summary>
        /// Thông tin Tu Luyện Châu
        /// </summary>
        public const string XiuLianZhu = "XiuLianZhu";

        /// <summary>
        /// Tổng thời gian Tu Luyện Châu có
        /// </summary>
        public const string XiuLianZhu_TotalTime = "XiuLianZhu_TotalTime";

        public const String MapPosRecord = "MapPosRecord";//地图定位参数，存放规则 mapid,x,y,mapid,x,y 全部是unsigned short 存放
        public const String ChengJiuFlags = "ChengJiuFlags";//成就完成与否 和 奖励领取标志位 每两bit一小分组表示一个成就
        public const String ChengJiuExtraData = "ChengJiuData";//成就相关辅助数据 比如成就点数  总杀怪数量 连续日登录次数 总日登录次数，每个都采用4字节存放
        public const String ZhuangBeiJiFen = "ZhuangBeiJiFen";//装备积分 单个整数
        public const String LieShaZhi = "LieShaZhi";//猎杀值 单个整数
        public const String WuXingZhi = "WuXingZhi";//悟性值 单个整数
        public const String ZhenQiZhi = "ZhenQiZhi";//真气值 单个整数
        public const String TianDiJingYuan = "TianDiJingYuan";//天地精元值 单个整数
        public const String ShiLianLing = "ShiLianLing";//试炼令值 单个整数 ===>通天令值
        public const String MapLimitSecs = "MapLimitSecs_";//地图时间限制前缀, 存储格式为: MapLimitSecs_XXX(地图编号), 日ID,今日已经停留时间(秒),道具额外加的时间(秒)
        public const String JingMaiLevel = "JingMaiLevel";//经脉等级值 单个整数
        public const String WuXueLevel = "WuXueLevel";//武学等级值 单个整数
        public const String ZuanHuangLevel = "ZuanHuangLevel";//砖皇等级值 单个整数
        public const String ZuanHuangAwardTime = "ZHAwardTime";//上次领取钻皇奖励的时间 相对 1970年的毫秒数字符串
        public const String SystemOpenValue = "SystemOpenValue";//系统激活项，主要用于辅助客户端记忆经脉等随等级提升的图标显示 单个整数 按位表示各个激活项，最多32个
        public const String JunGong = "JunGong";//军功值 单个整数
        public const String GuMuAwardDayID = "GuMuAwardDayID";//古墓限时奖励 单个整数
        public const String BossFuBenExtraEnterNum = "BossFuBenNum";//boss副本额外进入次数 单个整数
        public const String KaiFuOnlineDayID = "KaiFuOnlineDayID";//开服在线奖励天ID
        public const String KaiFuOnlineDayBit = "KaiFuOnlineDayBit";//开服在线奖励天的位标志
        public const String KaiFuOnlineDayTimes = "KaiFuOnlineDayTimes_";//开服在线奖励每天的在线时长
        public const String To60or100 = "To60or100"; //达到60或者100级的记忆
        public const String DayGift1 = "MeiRiChongZhiHaoLi1";  //每日充值豪礼1 [7/16/2013 LiaoWei]
        public const String DayGift2 = "MeiRiChongZhiHaoLi2";  //每日充值豪礼2 [7/16/2013 LiaoWei]
        public const String DayGift3 = "MeiRiChongZhiHaoLi3";  //每日充值豪礼3 [7/16/2013 LiaoWei]
        public const String JieriLoginNum = "JieriLoginNum"; //节日的登录次数
        public const String JieriLoginDayID = "JieriLoginDayID"; //节日的登录天ID
        public const String ZiKaDayNum = "ZiKaDayNum"; //当日已经兑换字卡的数量
        public const String ZiKaDayID = "ZiKaDayID"; //当日已经兑换字卡的天ID
        public const String FreeCSNum = "FreeCSNum"; //当日已经免费传送的次数
        public const String FreeCSDayID = "FreeCSDayID"; //当日已经免费传送的天ID
        public const String MaxTongQianNum = "MaxTongQianNum"; //角色的最大铜钱值
        public const String ErGuoTouNum = "ErGuoTouNum"; //二锅头今日的消费次数
        public const String ErGuoTouDayID = "ErGuoTouDayID"; //二锅头今日的天ID
        public const String BuChangFlag = "BuChangFlag"; //补偿的标志
        public const String ZhanHun = "ZhanHun"; //战魂
        public const String RongYu = "RongYu"; //荣誉
        public const String ZhanHunLevel = "ZhanHunLevel"; //战魂等级
        public const String RongYuLevel = "RongYuLevel"; //荣誉等级
        public const String ZJDJiFen = "ZJDJiFen"; //砸金蛋的积分
        public const String ZJDJiFenDayID = "ZJDJiFenDayID"; //砸金蛋的积分天ID
        public const String ZJDJiFenBits = "ZJDJiFenBits"; //砸金蛋的积分领取记录
        public const String ZJDJiFenBitsDayID = "ZJDJiFenBitsDayID"; //砸金蛋的积分领取记录
        public const String FashionWingsID = "FashionWingsID"; // 时装翅膀ID
        public const String TotalCostMoney = "TotalCostMoney";         // 记录玩家的充值总额和消费总额

        public const String LeftFreeChangeNameTimes = "LeftFreeChangeNameTimes"; //剩余免费改名次数
        public const String AlreadyZuanShiChangeNameTimes = "AlreadyZuanShiChangeNameTimes"; //已经使用钻石改名的次数
        public const String BuildQueueData = "BuildQueueData";     // 领地建造队列数据" openpaynum:queueid|builid|taskid:queueid|buildid|taskid"
        public const String BuildAllLevAward = "BuildAllLevAward";  // 领地总等级奖励领取状态

        public const String SettingBitFlags = "SettingBitFlags"; // 一些功能设置, 参考ESettingBitFlag
    }

    /// <summary>
    /// 角色参数名称--->注意 长度不能超过32字符！！！！！！！！！！！！！！！！存储内容不能超过60字节!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    public class RoleParamNameInfo
    {
        private static ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        #region RoleParamNameTypeExtDict

        private static Dictionary<string, RoleParamType> RoleParamNameTypeExtDict = new Dictionary<string, RoleParamType>()
        {
            //不常用的可能会作废的保留在原表
			{"ReturnCode", new RoleParamType("ReturnCode", "ReturnCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"SpreadCode", new RoleParamType("SpreadCode", "SpreadCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"VerifyCode", new RoleParamType("VerifyCode", "VerifyCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},

            {"WeekEndInputOD", new RoleParamType("WeekEndInputOpenDay", "WeekEndInputOD", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"LangHunLingYuDayAwards", new RoleParamType("LangHunLingYuDayAwardsDay", "LangHunLingYuDayAwards", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},

            {"MeiRiChongZhiHaoLi1", new RoleParamType("DayGift1", "MeiRiChongZhiHaoLi1", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"MeiRiChongZhiHaoLi2", new RoleParamType("DayGift2", "MeiRiChongZhiHaoLi2", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"MeiRiChongZhiHaoLi3", new RoleParamType("DayGift3", "MeiRiChongZhiHaoLi3", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},

            //已经明确作废的
			{"ZiKaDayID", new RoleParamType("ZiKaDayID", "ZiKaDayID", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"ZiKaDayNum", new RoleParamType("ZiKaDayNum", "ZiKaDayNum", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"PictureJudgeFlags", new RoleParamType("PictureJudgeFlags", "PictureJudgeFlags", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BloodCastleSceneTimer", new RoleParamType("BloodCastleSceneTimer", "BloodCastleSceneTimer", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"TreasureLogUpdate0", new RoleParamType("TreasureLogUpdate0", "TreasureLogUpdate0", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BuildLogUpdate", new RoleParamType("BuildLogUpdate", "BuildLogUpdate", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BuildLogUpdate0", new RoleParamType("BuildLogUpdate0", "BuildLogUpdate0", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BuildLogUpdate2", new RoleParamType("BuildLogUpdate2", "BuildLogUpdate2", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BuildLogUpdate4", new RoleParamType("BuildLogUpdate4", "BuildLogUpdate4", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},

            {"ChangeLifeCount", new RoleParamType("ChangeLifeCount", "ChangeLifeCount", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"YongZheZhanChangGroupId", new RoleParamType("YongZheZhanChangGroupId", "YongZheZhanChangGroupId", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},

            //作为前缀加数字后缀使用的
			{"InputPointExchg", new RoleParamType("InputPointExchg", "InputPointExchg", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
            {"JRExcharge", new RoleParamType("JRExcharge", "JRExcharge", "t_roleparams_2", "pvalue", "pname", 0, 0, -2)},
            {"KaiFuOnlineDayTimes_", new RoleParamType("KaiFuOnlineDayTimes_", "KaiFuOnlineDayTimes_", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
            {"MapLimitSecs_", new RoleParamType("MapLimitSecs", "MapLimitSecs_", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
            {"MeiRiChongZhiHaoLi", new RoleParamType("MeiRiChongZhiHaoLi", "MeiRiChongZhiHaoLi", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
        };

        #endregion RoleParamNameTypeExtDict

        #region RoleParamNameTypeDict

        private static Dictionary<string, RoleParamType> OldRoleParamNameTypeDict = new Dictionary<string, RoleParamType>();

        /// <summary>
        /// 角色参数表的存储位置和类型定义
        /// </summary>
        private static Dictionary<string, RoleParamType> RoleParamNameTypeDict = new Dictionary<string, RoleParamType>()
        {
            // common type

            #region KIEMTHE

            {"LifeSkill", new RoleParamType("LifeSkill", "LifeSkill", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"AutoSettings", new RoleParamType("AutoSettings", "AutoSettings", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"SystemSettings", new RoleParamType("SystemSettings", "SystemSettings", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"RoleTitles", new RoleParamType("RoleTitles", "RoleTitles", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"ReputeInfo", new RoleParamType("ReputeInfo", "ReputeInfo", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"DailyRecore", new RoleParamType("DailyRecore", "DailyRecore", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"WeekRecore", new RoleParamType("WeekRecore", "WeekRecore", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"ForeverRecore", new RoleParamType("ForeverRecore", "ForeverRecore", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},

            #endregion KIEMTHE

            {"KaiFuOnlineDayTimes_", new RoleParamType("KaiFuOnlineDayTimes", "KaiFuOnlineDayTimes_", "t_roleparams_2", "pname", "pvalue", -1, -1, 0)},
            {"MeiRiChongZhiHaoLi1", new RoleParamType("DayGift1", "MeiRiChongZhiHaoLi1", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"MeiRiChongZhiHaoLi2", new RoleParamType("DayGift2", "MeiRiChongZhiHaoLi2", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"MeiRiChongZhiHaoLi3", new RoleParamType("DayGift3", "MeiRiChongZhiHaoLi3", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"ReturnCode", new RoleParamType("ReturnCode", "ReturnCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"SpreadCode", new RoleParamType("SpreadCode", "SpreadCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"VerifyCode", new RoleParamType("VerifyCode", "VerifyCode", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"ZiKaDayNum", new RoleParamType("ZiKaDayNum", "ZiKaDayNum", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"ZiKaDayID", new RoleParamType("ZiKaDayID", "ZiKaDayID", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"BuildLogUpdate", new RoleParamType("BuildLogUpdate", "BuildLogUpdate", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"PictureJudgeFlags", new RoleParamType("PictureJudgeFlags", "PictureJudgeFlags", "t_roleparams_2", "pname", "pvalue", 0, 0, -1)},
            {"MapLimitSecs_", new RoleParamType("MapLimitSecs", "MapLimitSecs_", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
            {"JRExcharge", new RoleParamType("JRExcharge", "JRExcharge", "t_roleparams_2", "pvalue", "pname", 0, 0, -2)},
            {"InputPointExchg", new RoleParamType("InputPointExchg", "InputPointExchg", "t_roleparams_2", "pname", "pvalue", 0, 0, -2)},
#if 移植
            {"ShiPinLev", new RoleParamType("ShiPinLev", "ShiPinLev", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"MeiLi", new RoleParamType("MeiLi", "MeiLi", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"ShenQi", new RoleParamType("ShenQi", "ShenQi", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"ShenLiJingHua", new RoleParamType("ShenLiJingHua", "ShenLiJingHua", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
            {"ElementhBag", new RoleParamType("ElementhBag", "ElementhBag", "t_roleparams_2", "pname", "pvalue", 0, 0, 0)},
#endif
			/// type1
			{"ChengJiuData", new RoleParamType("ChengJiuExtraData", "ChengJiuData", "t_roleparams_char", "idx", "v0", 0, 0, 1)},
            {"DailyActiveFlag", new RoleParamType("DailyActiveFlag", "DailyActiveFlag", "t_roleparams_char", "idx", "v1", 0, 1, 1)},
            {"DailyActiveInfo1", new RoleParamType("DailyActiveInfo1", "DailyActiveInfo1", "t_roleparams_char", "idx", "v2", 0, 2, 1)},
            {"RoleLoginRecorde", new RoleParamType("RoleLoginRecorde", "RoleLoginRecorde", "t_roleparams_char", "idx", "v3", 0, 3, 1)},
            {"ChengJiuFlags", new RoleParamType("ChengJiuFlags", "ChengJiuFlags", "t_roleparams_char", "idx", "v4", 0, 4, 1)},
            {"UpLevelGiftFlags", new RoleParamType("UpLevelGiftFlags", "UpLevelGiftFlags", "t_roleparams_char", "idx", "v5", 0, 5, 1)},
            {"AchievementRune", new RoleParamType("AchievementRune", "AchievementRune", "t_roleparams_char", "idx", "v6", 0, 6, 1)},
            {"AchievementRuneUpCount", new RoleParamType("AchievementRuneUpCount", "AchievementRuneUpCount", "t_roleparams_char", "idx", "v7", 0, 7, 1)},
            {"DailyShare", new RoleParamType("DailyShare", "DailyShare", "t_roleparams_char", "idx", "v8", 0, 8, 1)},
            {"WeekEndInputFlag", new RoleParamType("WeekEndInputFlag", "WeekEndInputFlag", "t_roleparams_char", "idx", "v9", 0, 9, 1)},

            {"MapPosRecord", new RoleParamType("MapPosRecord", "MapPosRecord", "t_roleparams_char", "idx", "v0", 10, 10, 1)},
            {"ChongJiGiftList", new RoleParamType("ChongJiGiftList", "ChongJiGiftList", "t_roleparams_char", "idx", "v1", 10, 11, 1)},
            {"DailyChargeGiftFlags", new RoleParamType("DailyChargeGiftFlags", "DailyChargeGiftFlags", "t_roleparams_char", "idx", "v2", 10, 12, 1)},
            {"YueKaInfo", new RoleParamType("YueKaInfo", "YueKaInfo", "t_roleparams_char", "idx", "v3", 10, 13, 1)},
            {"TotalCostMoney", new RoleParamType("TotalCostMoney", "TotalCostMoney", "t_roleparams_char", "idx", "v4", 10, 14, 1)},
            {"PrestigeMedal", new RoleParamType("PrestigeMedal", "PrestigeMedal", "t_roleparams_char", "idx", "v5", 10, 15, 1)},
            {"PrestigeMedalUpCount", new RoleParamType("PrestigeMedalUpCount", "PrestigeMedalUpCount", "t_roleparams_char", "idx", "v6", 10, 16, 1)},
            {"TreasureData", new RoleParamType("TreasureData", "TreasureData", "t_roleparams_char", "idx", "v7", 10, 17, 1)},
            {"QingGongYanJoinFlag", new RoleParamType("QingGongYanJoinFlag", "QingGongYanJoinFlag", "t_roleparams_char", "idx", "v8", 10, 18, 1)},
            {"YongZheZhanChangAwards", new RoleParamType("YongZheZhanChangAwards", "YongZheZhanChangAwards", "t_roleparams_char", "idx", "v9", 10, 19, 1)},

            {"BuildQueueData", new RoleParamType("BuildQueueData", "BuildQueueData", "t_roleparams_char", "idx", "v0", 20, 20, 1)},
            {"BuildAllLevAward", new RoleParamType("BuildAllLevAward", "BuildAllLevAward", "t_roleparams_char", "idx", "v1", 20, 21, 1)},
            {"UnionPalaceUpCount", new RoleParamType("UnionPalaceUpCount", "UnionPalaceUpCount", "t_roleparams_char", "idx", "v2", 20, 22, 1)},
            {"UnionPalace", new RoleParamType("UnionPalace", "UnionPalace", "t_roleparams_char", "idx", "v3", 20, 23, 1)},
            {"PetSkillUpCount", new RoleParamType("PetSkillUpCount", "PetSkillUpCount", "t_roleparams_char", "idx", "v4", 20, 24, 1)},
            {"EnterKuaFuMapFlag", new RoleParamType("EnterKuaFuMapFlag", "EnterKuaFuMapFlag", "t_roleparams_char", "idx", "v5", 20, 25, 1)},
            {"SiegeWarfareEveryDayAwardDayID", new RoleParamType("SiegeWarfareEveryDayAwardDayID", "SiegeWarfareEveryDayAwardDayID", "t_roleparams_char", "idx", "v6", 20, 26, 1)},
         
#region Kiếm Thế
            {RoleParamName.PrayData, new RoleParamType(RoleParamName.PrayData, RoleParamName.PrayData, "t_roleparams_char", "idx", "v7", 20, 27, 1)},
            {RoleParamName.CopySceneRecord, new RoleParamType(RoleParamName.CopySceneRecord, RoleParamName.CopySceneRecord, "t_roleparams_char", "idx", "v1", 30, 29, 1)},
            {RoleParamName.DefaultRelivePos, new RoleParamType(RoleParamName.DefaultRelivePos, RoleParamName.DefaultRelivePos, "t_roleparams_char", "idx", "v2", 30, 30, 1)},
          
#endregion Kiếm Thế

            /// type2
            {"AddProPointForLevelUp", new RoleParamType("AddProPointForLevelUp", "AddProPointForLevelUp", "t_roleparams_long", "idx", "v0", 10000, 10000, 2)},
            {"AdmireCount", new RoleParamType("AdmireCount", "AdmireCount", "t_roleparams_long", "idx", "v1", 10000, 10001, 2)},
            {"AdmireDayID", new RoleParamType("AdmireDayID", "AdmireDayID", "t_roleparams_long", "idx", "v2", 10000, 10002, 2)},
            {"DailyActiveDayID", new RoleParamType("DailyActiveDayID", "DailyActiveDayID", "t_roleparams_long", "idx", "v3", 10000, 10003, 2)},
            {"PKKingAdmireCount", new RoleParamType("PKKingAdmireCount", "PKKingAdmireCount", "t_roleparams_long", "idx", "v4", 10000, 10004, 2)},
            {"PKKingAdmireDayID", new RoleParamType("PKKingAdmireDayID", "PKKingAdmireDayID", "t_roleparams_long", "idx", "v5", 10000, 10005, 2)},
            {"DailyActiveAwardFlag", new RoleParamType("DailyActiveAwardFlag", "DailyActiveAwardFlag", "t_roleparams_long", "idx", "v6", 10000, 10006, 2)},
            {"PropConstitution", new RoleParamType("sPropConstitution", "PropConstitution", "t_roleparams_long", "idx", "v7", 10000, 10007, 2)},
            {"PropDexterity", new RoleParamType("sPropDexterity", "PropDexterity", "t_roleparams_long", "idx", "v8", 10000, 10008, 2)},
            {"PropIntelligence", new RoleParamType("sPropIntelligence", "PropIntelligence", "t_roleparams_long", "idx", "v9", 10000, 10009, 2)},
            {"PropStrength", new RoleParamType("sPropStrength", "PropStrength", "t_roleparams_long", "idx", "v10", 10000, 10010, 2)},
            {"MeditateTime", new RoleParamType("MeditateTime", "MeditateTime", "t_roleparams_long", "idx", "v11", 10000, 10011, 2)},
            {"DefaultSkillLev", new RoleParamType("DefaultSkillLev", "DefaultSkillLev", "t_roleparams_long", "idx", "v13", 10000, 10013, 2)},
            {"DefaultSkillUseNum", new RoleParamType("DefaultSkillUseNum", "DefaultSkillUseNum", "t_roleparams_long", "idx", "v14", 10000, 10014, 2)},
            {"CurHP", new RoleParamType("CurHP", "CurHP", "t_roleparams_long", "idx", "v15", 10000, 10015, 2)},
            {"CurMP", new RoleParamType("CurMP", "CurMP", "t_roleparams_long", "idx", "v16", 10000, 10016, 2)},
            {"DayOnlineSecond", new RoleParamType("DayOnlineSecond", "DayOnlineSecond", "t_roleparams_long", "idx", "v17", 10000, 10017, 2)},
            {"NotSafeMeditateTime", new RoleParamType("NotSafeMeditateTime", "NotSafeMeditateTime", "t_roleparams_long", "idx", "v18", 10000, 10018, 2)},
            {"SeriesLoginCount", new RoleParamType("SeriesLoginCount", "SeriesLoginCount", "t_roleparams_long", "idx", "v19", 10000, 10019, 2)},
            {"OpenGridTick", new RoleParamType("OpenGridTick", "OpenGridTick", "t_roleparams_long", "idx", "v20", 10000, 10020, 2)},
            {"OpenPortableGridTick", new RoleParamType("OpenPortableGridTick", "OpenPortableGridTick", "t_roleparams_long", "idx", "v21", 10000, 10021, 2)},
            {"GuMuAwardDayID", new RoleParamType("GuMuAwardDayID", "GuMuAwardDayID", "t_roleparams_long", "idx", "v22", 10000, 10022, 2)},
            {"FightGetThings", new RoleParamType("FightGetThings", "FightGetThings", "t_roleparams_long", "idx", "v23", 10000, 10023, 2)},
            {"JieriLoginDayID", new RoleParamType("JieriLoginDayID", "JieriLoginDayID", "t_roleparams_long", "idx", "v24", 10000, 10024, 2)},
            {"JieriLoginNum", new RoleParamType("JieriLoginNum", "JieriLoginNum", "t_roleparams_long", "idx", "v25", 10000, 10025, 2)},
            {"VerifyBuffProp", new RoleParamType("VerifyBuffProp", "VerifyBuffProp", "t_roleparams_long", "idx", "v26", 10000, 10026, 2)},
            {"CallPetFreeTime", new RoleParamType("CallPetFreeTime", "CallPetFreeTime", "t_roleparams_long", "idx", "v27", 10000, 10027, 2)},
            {"MaxTongQianNum", new RoleParamType("MaxTongQianNum", "MaxTongQianNum", "t_roleparams_long", "idx", "v28", 10000, 10028, 2)},
            {"ShengWang", new RoleParamType("ShengWang", "ShengWang", "t_roleparams_long", "idx", "v29", 10000, 10029, 2)},
            {"TianDiJingYuan", new RoleParamType("TianDiJingYuan", "TianDiJingYuan", "t_roleparams_long", "idx", "v30", 10000, 10030, 2)},
            {"To60or100", new RoleParamType("To60or100", "To60or100", "t_roleparams_long", "idx", "v31", 10000, 10031, 2)},
            {"StarSoul", new RoleParamType("StarSoul", "StarSoul", "t_roleparams_long", "idx", "v32", 10000, 10032, 2)},
            {"TotalLoginAwardFlag", new RoleParamType("TotalLoginAwardFlag", "TotalLoginAwardFlag", "t_roleparams_long", "idx", "v33", 10000, 10033, 2)},
            {"TianTiDayScore", new RoleParamType("TianTiDayScore", "TianTiDayScore", "t_roleparams_long", "idx", "v34", 10000, 10034, 2)},
            {"ImpetrateTime", new RoleParamType("ImpetrateTime", "ImpetrateTime", "t_roleparams_long", "idx", "v35", 10000, 10035, 2)},
            {"ChengJiuLevel", new RoleParamType("ChengJiuLevel", "ChengJiuLevel", "t_roleparams_long", "idx", "v36", 10000, 10036, 2)},
            {"DaimonSquareSceneFinishFlag", new RoleParamType("DaimonSquareSceneFinishFlag", "DaimonSquareSceneFinishFlag", "t_roleparams_long", "idx", "v37", 10000, 10037, 2)},
            {"DaimonSquareSceneid", new RoleParamType("DaimonSquareSceneid", "DaimonSquareSceneid", "t_roleparams_long", "idx", "v38", 10000, 10038, 2)},
            {"DaimonSquareSceneTimer", new RoleParamType("DaimonSquareSceneTimer", "DaimonSquareSceneTimer", "t_roleparams_long", "idx", "v39", 10000, 10039, 2)},
            {"DaimonSquarePlayerPoint", new RoleParamType("DaimonSquarePlayerPoint", "DaimonSquarePlayerPoint", "t_roleparams_long", "idx", "v0", 10040, 10040, 2)},
            {"PropIntelligenceChangeless", new RoleParamType("sPropIntelligenceChangeless", "PropIntelligenceChangeless", "t_roleparams_long", "idx", "v1", 10040, 10041, 2)},
            {"BuChangFlag", new RoleParamType("BuChangFlag", "BuChangFlag", "t_roleparams_long", "idx", "v2", 10040, 10042, 2)},
            {"PropDexterityChangeless", new RoleParamType("sPropDexterityChangeless", "PropDexterityChangeless", "t_roleparams_long", "idx", "v3", 10040, 10043, 2)},
            {"PropConstitutionChangeless", new RoleParamType("sPropConstitutionChangeless", "PropConstitutionChangeless", "t_roleparams_long", "idx", "v4", 10040, 10044, 2)},
            {"DaimonSquareFuBenSeqID", new RoleParamType("DaimonSquareFuBenSeqID", "DaimonSquareFuBenSeqID", "t_roleparams_long", "idx", "v5", 10040, 10045, 2)},
            {"PropStrengthChangeless", new RoleParamType("sPropStrengthChangeless", "PropStrengthChangeless", "t_roleparams_long", "idx", "v6", 10040, 10046, 2)},
            {"ShengWangLevel", new RoleParamType("ShengWangLevel", "ShengWangLevel", "t_roleparams_long", "idx", "v7", 10040, 10047, 2)},
            {"MUMoHe", new RoleParamType("MUMoHe", "MUMoHe", "t_roleparams_long", "idx", "v8", 10040, 10048, 2)},
            {"WanMoTaCurrLayerOrder", new RoleParamType("WanMoTaCurrLayerOrder", "WanMoTaCurrLayerOrder", "t_roleparams_long", "idx", "v9", 10040, 10049, 2)},
            {"LeftFreeChangeNameTimes", new RoleParamType("LeftFreeChangeNameTimes", "LeftFreeChangeNameTimes", "t_roleparams_long", "idx", "v10", 10040, 10050, 2)},
            {"BloodCastleSceneFinishFlag", new RoleParamType("BloodCastleSceneFinishFlag", "BloodCastleSceneFinishFlag", "t_roleparams_long", "idx", "v11", 10040, 10051, 2)},
            {"BloodCastleSceneid", new RoleParamType("BloodCastleSceneid", "BloodCastleSceneid", "t_roleparams_long", "idx", "v12", 10040, 10052, 2)},
            {"BloodCastlePlayerPoint", new RoleParamType("BloodCastlePlayerPoint", "BloodCastlePlayerPoint", "t_roleparams_long", "idx", "v13", 10040, 10053, 2)},
            {"ElementPowder", new RoleParamType("ElementPowderCount", "ElementPowder", "t_roleparams_long", "idx", "v14", 10040, 10054, 2)},
            {"CaiJiCrystalDayID", new RoleParamType("CaiJiCrystalDayID", "CaiJiCrystalDayID", "t_roleparams_long", "idx", "v15", 10040, 10055, 2)},
            {"CaiJiCrystalNum", new RoleParamType("CaiJiCrystalNum", "CaiJiCrystalNum", "t_roleparams_long", "idx", "v16", 10040, 10056, 2)},
            {"LianZhiJinBiCount", new RoleParamType("LianZhiJinBiCount", "LianZhiJinBiCount", "t_roleparams_long", "idx", "v17", 10040, 10057, 2)},
            {"LianZhiJinBiDayID", new RoleParamType("LianZhiJinBiDayID", "LianZhiJinBiDayID", "t_roleparams_long", "idx", "v18", 10040, 10058, 2)},
            {"FTFTradeCount", new RoleParamType("FTFTradeCount", "FTFTradeCount", "t_roleparams_long", "idx", "v19", 10040, 10059, 2)},
            {"FTFTradeDayID", new RoleParamType("FTFTradeDayID", "FTFTradeDayID", "t_roleparams_long", "idx", "v20", 10040, 10060, 2)},
            {"ZhuangBeiJiFen", new RoleParamType("ZhuangBeiJiFen", "ZhuangBeiJiFen", "t_roleparams_long", "idx", "v21", 10040, 10061, 2)},
            {"LieShaZhi", new RoleParamType("LieShaZhi", "LieShaZhi", "t_roleparams_long", "idx", "v22", 10040, 10062, 2)},
            {"WuXingZhi", new RoleParamType("WuXingZhi", "WuXingZhi", "t_roleparams_long", "idx", "v23", 10040, 10063, 2)},
            {"ZhenQiZhi", new RoleParamType("ZhenQiZhi", "ZhenQiZhi", "t_roleparams_long", "idx", "v24", 10040, 10064, 2)},
            {"ShiLianLing", new RoleParamType("ShiLianLing", "ShiLianLing", "t_roleparams_long", "idx", "v25", 10040, 10065, 2)},
            {"JingMaiLevel", new RoleParamType("JingMaiLevel", "JingMaiLevel", "t_roleparams_long", "idx", "v26", 10040, 10066, 2)},
            {"WuXueLevel", new RoleParamType("WuXueLevel", "WuXueLevel", "t_roleparams_long", "idx", "v27", 10040, 10067, 2)},
            {"ZuanHuangLevel", new RoleParamType("ZuanHuangLevel", "ZuanHuangLevel", "t_roleparams_long", "idx", "v28", 10040, 10068, 2)},
            {"ZHAwardTime", new RoleParamType("ZuanHuangAwardTime", "ZHAwardTime", "t_roleparams_long", "idx", "v29", 10040, 10069, 2)},
            {"SystemOpenValue", new RoleParamType("SystemOpenValue", "SystemOpenValue", "t_roleparams_long", "idx", "v30", 10040, 10070, 2)},
            {"JunGong", new RoleParamType("JunGong", "JunGong", "t_roleparams_long", "idx", "v31", 10040, 10071, 2)},
            {"BossFuBenNum", new RoleParamType("BossFuBenExtraEnterNum", "BossFuBenNum", "t_roleparams_long", "idx", "v32", 10040, 10072, 2)},
            {"KaiFuOnlineDayID", new RoleParamType("KaiFuOnlineDayID", "KaiFuOnlineDayID", "t_roleparams_long", "idx", "v33", 10040, 10073, 2)},
            {"KaiFuOnlineDayBit", new RoleParamType("KaiFuOnlineDayBit", "KaiFuOnlineDayBit", "t_roleparams_long", "idx", "v34", 10040, 10074, 2)},
            {"FreeCSNum", new RoleParamType("FreeCSNum", "FreeCSNum", "t_roleparams_long", "idx", "v35", 10040, 10075, 2)},
            {"FreeCSDayID", new RoleParamType("FreeCSDayID", "FreeCSDayID", "t_roleparams_long", "idx", "v36", 10040, 10076, 2)},
            {"ErGuoTouNum", new RoleParamType("ErGuoTouNum", "ErGuoTouNum", "t_roleparams_long", "idx", "v37", 10040, 10077, 2)},
            {"ErGuoTouDayID", new RoleParamType("ErGuoTouDayID", "ErGuoTouDayID", "t_roleparams_long", "idx", "v38", 10040, 10078, 2)},
            {"ZhanHun", new RoleParamType("ZhanHun", "ZhanHun", "t_roleparams_long", "idx", "v39", 10040, 10079, 2)},
            {"RongYu", new RoleParamType("RongYu", "RongYu", "t_roleparams_long", "idx", "v0", 10080, 10080, 2)},
            {"ZhanHunLevel", new RoleParamType("ZhanHunLevel", "ZhanHunLevel", "t_roleparams_long", "idx", "v1", 10080, 10081, 2)},
            {"RongYuLevel", new RoleParamType("RongYuLevel", "RongYuLevel", "t_roleparams_long", "idx", "v2", 10080, 10082, 2)},
            {"ZJDJiFen", new RoleParamType("ZJDJiFen", "ZJDJiFen", "t_roleparams_long", "idx", "v3", 10080, 10083, 2)},
            {"ZJDJiFenDayID", new RoleParamType("ZJDJiFenDayID", "ZJDJiFenDayID", "t_roleparams_long", "idx", "v4", 10080, 10084, 2)},
            {"ZJDJiFenBits", new RoleParamType("ZJDJiFenBits", "ZJDJiFenBits", "t_roleparams_long", "idx", "v5", 10080, 10085, 2)},
            {"ZJDJiFenBitsDayID", new RoleParamType("ZJDJiFenBitsDayID", "ZJDJiFenBitsDayID", "t_roleparams_long", "idx", "v6", 10080, 10086, 2)},
            {"FuHuoJieZhiCD", new RoleParamType("FuHuoJieZhiCD", "FuHuoJieZhiCD", "t_roleparams_long", "idx", "v7", 10080, 10087, 2)},
            {"VIPExp", new RoleParamType("VIPExp", "VIPExp", "t_roleparams_long", "idx", "v8", 10080, 10088, 2)},
            {"BloodCastleFuBenSeqID", new RoleParamType("BloodCastleFuBenSeqID", "BloodCastleFuBenSeqID", "t_roleparams_long", "idx", "v9", 10080, 10089, 2)},
            {"LiXianBaiTanTicks", new RoleParamType("LiXianBaiTanTicks", "LiXianBaiTanTicks", "t_roleparams_long", "idx", "v10", 10080, 10090, 2)},
            {"LianZhiBangZuanCount", new RoleParamType("LianZhiBangZuanCount", "LianZhiBangZuanCount", "t_roleparams_long", "idx", "v11", 10080, 10091, 2)},
            {"LianZhiZuanShiCount", new RoleParamType("LianZhiZuanShiCount", "LianZhiZuanShiCount", "t_roleparams_long", "idx", "v12", 10080, 10092, 2)},
            {"LianZhiBangZuanDayID", new RoleParamType("LianZhiBangZuanDayID", "LianZhiBangZuanDayID", "t_roleparams_long", "idx", "v13", 10080, 10093, 2)},
            {"LianZhiZuanShiDayID", new RoleParamType("LianZhiZuanShiDayID", "LianZhiZuanShiDayID", "t_roleparams_long", "idx", "v14", 10080, 10094, 2)},
            {"HeFuLoginFlag", new RoleParamType("HeFuLoginFlag", "HeFuLoginFlag", "t_roleparams_long", "idx", "v15", 10080, 10095, 2)},
            {"HeFuTotalLoginDay", new RoleParamType("HeFuTotalLoginDay", "HeFuTotalLoginDay", "t_roleparams_long", "idx", "v16", 10080, 10096, 2)},
            {"HeFuTotalLoginNum", new RoleParamType("HeFuTotalLoginNum", "HeFuTotalLoginNum", "t_roleparams_long", "idx", "v17", 10080, 10097, 2)},
            {"HeFuTotalLoginFlag", new RoleParamType("HeFuTotalLoginFlag", "HeFuTotalLoginFlag", "t_roleparams_long", "idx", "v18", 10080, 10098, 2)},
            {"HeFuPKKingFlag", new RoleParamType("HeFuPKKingFlag", "HeFuPKKingFlag", "t_roleparams_long", "idx", "v19", 10080, 10099, 2)},
            {"GuildCopyMapAwardDay", new RoleParamType("GuildCopyMapAwardDay", "GuildCopyMapAwardDay", "t_roleparams_long", "idx", "v20", 10080, 10100, 2)},
            {"GuildCopyMapAwardFlag", new RoleParamType("GuildCopyMapAwardFlag", "GuildCopyMapAwardFlag", "t_roleparams_long", "idx", "v21", 10080, 10101, 2)},
            {"ElementGrade", new RoleParamType("ElementGrade", "ElementGrade", "t_roleparams_long", "idx", "v22", 10080, 10102, 2)},
            {"PetJiFen", new RoleParamType("PetJiFen", "PetJiFen", "t_roleparams_long", "idx", "v23", 10080, 10103, 2)},
            {"FashionWingsID", new RoleParamType("FashionWingsID", "FashionWingsID", "t_roleparams_long", "idx", "v24", 10080, 10104, 2)},
            {"FashionTitleID", new RoleParamType("FashionTitleID", "FashionTitleID", "t_roleparams_long", "idx", "v25", 10080, 10105, 2)},
            {"ArtifactFailCount", new RoleParamType("ArtifactFailCount", "ArtifactFailCount", "t_roleparams_long", "idx", "v26", 10080, 10106, 2)},
            {"ZaiZaoPoint", new RoleParamType("ZaiZaoPoint", "ZaiZaoPoint", "t_roleparams_long", "idx", "v27", 10080, 10107, 2)},
            {"LLCZAdmireCount", new RoleParamType("LLCZAdmireCount", "LLCZAdmireCount", "t_roleparams_long", "idx", "v28", 10080, 10108, 2)},
            {"LLCZAdmireDayID", new RoleParamType("LLCZAdmireDayID", "LLCZAdmireDayID", "t_roleparams_long", "idx", "v29", 10080, 10109, 2)},
            {"HysySuccessCount", new RoleParamType("HysySuccessCount", "HysySuccessCount", "t_roleparams_long", "idx", "v30", 10080, 10110, 2)},
            {"HysySuccessDayId", new RoleParamType("HysySuccessDayId", "HysySuccessDayId", "t_roleparams_long", "idx", "v31", 10080, 10111, 2)},
            {"HysyYTDSuccessCount", new RoleParamType("HysyYTDSuccessCount", "HysyYTDSuccessCount", "t_roleparams_long", "idx", "v32", 10080, 10112, 2)},
            {"HysyYTDSuccessDayId", new RoleParamType("HysyYTDSuccessDayId", "HysyYTDSuccessDayId", "t_roleparams_long", "idx", "v33", 10080, 10113, 2)},
            {"SaleTradeDayID", new RoleParamType("SaleTradeDayID", "SaleTradeDayID", "t_roleparams_long", "idx", "v34", 10080, 10114, 2)},
            {"SaleTradeCount", new RoleParamType("SaleTradeCount", "SaleTradeCount", "t_roleparams_long", "idx", "v35", 10080, 10115, 2)},
            {"HeFuLuoLanAwardFlag", new RoleParamType("HeFuLuoLanAwardFlag", "HeFuLuoLanAwardFlag", "t_roleparams_long", "idx", "v36", 10080, 10116, 2)},
            {"LHLYAdmireCount", new RoleParamType("LHLYAdmireCount", "LHLYAdmireCount", "t_roleparams_long", "idx", "v37", 10080, 10117, 2)},
            {"LHLYAdmireDayID", new RoleParamType("LHLYAdmireDayID", "LHLYAdmireDayID", "t_roleparams_long", "idx", "v38", 10080, 10118, 2)},
            {"ZhengBaPoint", new RoleParamType("ZhengBaPoint", "ZhengBaPoint", "t_roleparams_long", "idx", "v39", 10080, 10119, 2)},
            {"ZhengBaAwardFlag", new RoleParamType("ZhengBaAwardFlag", "ZhengBaAwardFlag", "t_roleparams_long", "idx", "v0", 10120, 10120, 2)},
            {"ZhengBaHintFlag", new RoleParamType("ZhengBaHintFlag", "ZhengBaHintFlag", "t_roleparams_long", "idx", "v1", 10120, 10121, 2)},
            {"ZhengBaJoinIconFlag", new RoleParamType("ZhengBaJoinIconFlag", "ZhengBaJoinIconFlag", "t_roleparams_long", "idx", "v2", 10120, 10122, 2)},
            {"BanRobotCount", new RoleParamType("BanRobotCount", "BanRobotCount", "t_roleparams_long", "idx", "v3", 10120, 10123, 2)},
            {"TreasureJiFen", new RoleParamType("TreasureJiFen", "TreasureJiFen", "t_roleparams_long", "idx", "v4", 10120, 10124, 2)},
            {"TreasureXueZuan", new RoleParamType("TreasureXueZuan", "TreasureXueZuan", "t_roleparams_long", "idx", "v5", 10120, 10125, 2)},
            {"WeekEndInputOD", new RoleParamType("WeekEndInputOpenDay", "WeekEndInputOD", "t_roleparams_long", "idx", "v6", 10120, 10126, 2)},
            {"LangHunLingYuDayAwards", new RoleParamType("LangHunLingYuDayAwardsDay", "LangHunLingYuDayAwards", "t_roleparams_long", "idx", "v7", 10120, 10127, 2)},
            {"LangHunLingYuDayAwardsFlags", new RoleParamType("LangHunLingYuDayAwardsFlags", "LangHunLingYuDayAwardsFlags", "t_roleparams_long", "idx", "v8", 10120, 10128, 2)},
            {"EnterBangHuiUnixSecs", new RoleParamType("EnterBangHuiUnixSecs", "EnterBangHuiUnixSecs", "t_roleparams_long", "idx", "v9", 10120, 10129, 2)},
            {"LastAutoReviveTicks", new RoleParamType("LastAutoReviveTicks", "LastAutoReviveTicks", "t_roleparams_long", "idx", "v10", 10120, 10130, 2)},
            {"AlreadyZuanShiChangeNameTimes", new RoleParamType("AlreadyZuanShiChangeNameTimes", "AlreadyZuanShiChangeNameTimes", "t_roleparams_long", "idx", "v11", 10120, 10131, 2)},
            {"CannotJoinKFCopyEndTicks", new RoleParamType("CannotJoinKFCopyEndTicks", "CannotJoinKFCopyEndTicks", "t_roleparams_long", "idx", "v12", 10120, 10132, 2)},
            {"ElementWarCount", new RoleParamType("ElementWarCount", "ElementWarCount", "t_roleparams_long", "idx", "v13", 10120, 10133, 2)},
          


#region KT_TANDUNG

            {"TotalGuildMoneyWithDraw", new RoleParamType("TotalGuildMoneyWithDraw", "TotalGuildMoneyWithDraw", "t_roleparams_long", "idx", "v14", 10120, 10134, 2)},
            {"TotalGuildMoneyAdd", new RoleParamType("TotalGuildMoneyAdd", "TotalGuildMoneyAdd", "t_roleparams_long", "idx", "v15", 10120, 10135, 2)},

#endregion

            //{"SpreadIsVip", new RoleParamType("SpreadIsVip", "SpreadIsVip", "t_roleparams_long", "idx", "v16", 10120, 10136, 2)},
            //{"LangHunFenMo", new RoleParamType("LangHunFenMo", "LangHunFenMo", "t_roleparams_long", "idx", "v17", 10120, 10137, 2)},
            //{"SoulStoneRandId", new RoleParamType("SoulStoneRandId", "SoulStoneRandId", "t_roleparams_long", "idx", "v18", 10120, 10138, 2)},
            //{"SettingBitFlags", new RoleParamType("SettingBitFlags", "SettingBitFlags", "t_roleparams_long", "idx", "v19", 10120, 10139, 2)},

            /// Tiềm năng có được từ bánh
            {"TotalPropPoint", new RoleParamType("TotalPropPoint", "TotalPropPoint", "t_roleparams_long", "idx", "v16", 10120, 10136, 2)},
            /// Kỹ năng có được từ bánh
            {"TotalSkillPoint", new RoleParamType("TotalSkillPoint", "TotalSkillPoint", "t_roleparams_long", "idx", "v17", 10120, 10137, 2)},
            /// Lượng kinh nghiệm Tu Luyện Châu có
            {RoleParamName.XiuLianZhu, new RoleParamType(RoleParamName.XiuLianZhu, RoleParamName.XiuLianZhu, "t_roleparams_long", "idx", "v18", 10120, 10138, 2)},
            {RoleParamName.XiuLianZhu_TotalTime, new RoleParamType(RoleParamName.XiuLianZhu_TotalTime, RoleParamName.XiuLianZhu_TotalTime, "t_roleparams_long", "idx", "v19", 10120, 10139, 2)},


            {"KaiFuOnlineDayTimes_1", new RoleParamType("KaiFuOnlineDayTimes_1", "KaiFuOnlineDayTimes_1", "t_roleparams_long", "idx", "v20", 10120, 10140, 2)},
            {"KaiFuOnlineDayTimes_2", new RoleParamType("KaiFuOnlineDayTimes_2", "KaiFuOnlineDayTimes_2", "t_roleparams_long", "idx", "v21", 10120, 10141, 2)},
            {"KaiFuOnlineDayTimes_3", new RoleParamType("KaiFuOnlineDayTimes_3", "KaiFuOnlineDayTimes_3", "t_roleparams_long", "idx", "v22", 10120, 10142, 2)},
            {"KaiFuOnlineDayTimes_4", new RoleParamType("KaiFuOnlineDayTimes_4", "KaiFuOnlineDayTimes_4", "t_roleparams_long", "idx", "v23", 10120, 10143, 2)},
            {"KaiFuOnlineDayTimes_5", new RoleParamType("KaiFuOnlineDayTimes_5", "KaiFuOnlineDayTimes_5", "t_roleparams_long", "idx", "v24", 10120, 10144, 2)},
            {"KaiFuOnlineDayTimes_6", new RoleParamType("KaiFuOnlineDayTimes_6", "KaiFuOnlineDayTimes_6", "t_roleparams_long", "idx", "v25", 10120, 10145, 2)},
            {"KaiFuOnlineDayTimes_7", new RoleParamType("KaiFuOnlineDayTimes_7", "KaiFuOnlineDayTimes_7", "t_roleparams_long", "idx", "v26", 10120, 10146, 2)},
            {"KaiFuOnlineDayTimes_8", new RoleParamType("KaiFuOnlineDayTimes_8", "KaiFuOnlineDayTimes_8", "t_roleparams_long", "idx", "v27", 10120, 10147, 2)},

#region Kiếm Thế

             // TINH HOẠT LỰC
            {"CurStamina", new RoleParamType("CurStamina", "CurStamina", "t_roleparams_long", "idx", "v28", 10120, 10148, 2)},
            {"GatherPoint", new RoleParamType("GatherPoint", "GatherPoint", "t_roleparams_long", "idx", "v29", 10120, 10149, 2)},
            {"MakePoint", new RoleParamType("MakePoint", "MakePoint", "t_roleparams_long", "idx", "v30", 10120, 10150, 2)},
            {RoleParamName.HorseToggleOn, new RoleParamType(RoleParamName.HorseToggleOn, RoleParamName.HorseToggleOn, "t_roleparams_long", "idx", "v31", 10120, 10151, 2)},

            // BẠO VĂN ĐỒNG
            {"CurBVDTaskID", new RoleParamType("CurBVDTaskID", "CurBVDTaskID", "t_roleparams_long", "idx", "v32", 10120, 10152, 2)},
            {"QuestBVDTodayCount", new RoleParamType("QuestBVDTodayCount", "QuestBVDTodayCount", "t_roleparams_long", "idx", "v33", 10120, 10153, 2)},
            {"CanncelQuestBVD", new RoleParamType("CanncelQuestBVD", "CanncelQuestBVD", "t_roleparams_long", "idx", "v34", 10120, 10154, 2)},
            {"QuestBVDStreakCount", new RoleParamType("QuestBVDStreakCount", "QuestBVDStreakCount", "t_roleparams_long", "idx", "v36", 10120, 10155, 2)},

            // HẢI TẶC
            {"PirateQuestSum", new RoleParamType("PirateQuestSum", "PirateQuestSum", "t_roleparams_long", "idx", "v37", 10120, 10156, 2)},
            {"CurPirateQuestID", new RoleParamType("CurPirateQuestID", "CurPirateQuestID", "t_roleparams_long", "idx", "v38", 10120, 10157, 2)},
            {"CurPrestigePoint", new RoleParamType("CurPrestigePoint", "CurPrestigePoint", "t_roleparams_long", "idx", "v39", 10120, 10158, 2)},

            

#endregion Kiếm Thế
        };

        #endregion RoleParamNameTypeDict

        public const int StringParamKey = 0;
        public const int LongParamKey = 10000;
        public const int SeldomStringParamKey = 20000;

        private const string LongParamTableName = "t_roleparams_long";
        private const string StringParamTableName = "t_roleparams_char";
        private const string SeldomStringParamTableName = "t_roleparams_2";
        private const string OldParamTableName = "t_roleparams";

        private static Dictionary<int, RoleParamType> RoleParamNameIndexDict = new Dictionary<int, RoleParamType>();

        private static object[] PrefixNameTree = new object[128];

        static RoleParamNameInfo()
        {
            try
            {
                foreach (var v in RoleParamNameTypeDict.Values)
                {
                    if (string.IsNullOrEmpty(v.IdxName) || string.IsNullOrEmpty(v.KeyString))
                    {
                        throw new Exception("初始数据异常:RoleParamNameTypeDict " + v.ParamName);
                    }

                    //分表
                    if (v.Type == 2 || v.Type == 1)
                    {
                        RoleParamNameIndexDict.Add(v.ParamIndex, v);
                    }

                    //前缀字符串
                    if (v.Type == -2)
                    {
                        string name = v.ParamName.ToLower();
                        object[] nextPtr = PrefixNameTree;
                        foreach (var c in name)
                        {
                            if (null == nextPtr[c] as object[])
                            {
                                nextPtr[c] = new object[128];
                            }

                            nextPtr = nextPtr[c] as object[];
                        }

                        nextPtr[0] = v;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogManager.WriteException(ex.ToString());
                throw;
            }
        }

        public static RoleParamType GetPrefixParamNameType(string paramName)
        {
            string name = paramName.ToLower();
            object[] nextPtr = PrefixNameTree;
            foreach (var c in name)
            {
                if (!char.IsDigit(c))
                {
                    nextPtr = nextPtr[c] as object[];
                    if (null == nextPtr)
                    {
                        return null;
                    }
                }
            }

            return nextPtr[0] as RoleParamType;
        }

        public static RoleParamType GetRoleParamType(string paramName, string value = null)
        {
            RoleParamType roleParamType;



            if (GameDBManager.Flag_Splite_RoleParams_Table != 0)
            {
                if (RoleParamNameTypeDict.TryGetValue(paramName, out roleParamType))
                {
                    return roleParamType;
                }

                int key;
                if (int.TryParse(paramName, NumberStyles.None, NumberFormatInfo.InvariantInfo, out key) && key >= 0)
                {
                    int rem;
                    if (key < LongParamKey)
                    {
                        Math.DivRem(key, 10, out rem);
                        roleParamType = new RoleParamType(paramName, paramName, StringParamTableName, "idx", "v" + rem, key - rem, key, (int)RoleParamType.ValueTypes.Long);
                    }
                    else if (key < SeldomStringParamKey)
                    {
                        Math.DivRem(key, 40, out rem);
                        roleParamType = new RoleParamType(paramName, paramName, LongParamTableName, "idx", "v" + rem, key - rem, key, (int)RoleParamType.ValueTypes.Long);
                    }
                    else
                    {
                        roleParamType = new RoleParamType(paramName, paramName, SeldomStringParamTableName, "pname", "pvalue", key, key, (int)RoleParamType.ValueTypes.Normal);
                    }
                }
                else
                {
                    if (null == GetPrefixParamNameType(paramName) && (!RoleParamNameTypeExtDict.TryGetValue(paramName, out roleParamType) || roleParamType.Type != -1))
                    {
                        string msg = string.Format("Unknow role parameters used: {0},{1}", paramName, value);
                        MyConsole.WriteLine(msg);
                        LogManager.WriteLog(LogTypes.Error, msg);
                    }

                    roleParamType = new RoleParamType(paramName, paramName, SeldomStringParamTableName, "pname", "pvalue", 0, 0, 0);
                }

                RoleParamNameTypeDict[paramName] = roleParamType;
            }
            else
            {
                if (OldRoleParamNameTypeDict.TryGetValue(paramName, out roleParamType))
                {
                    return roleParamType;
                }

                roleParamType = new RoleParamType(paramName, paramName, OldParamTableName, "pname", "pvalue", 0, 0, 0);
                OldRoleParamNameTypeDict[paramName] = roleParamType;
            }




            return roleParamType;
        }

        public static RoleParamType GetRoleParamType(int idx, int column)
        {
            RoleParamType roleParamType = null;
            int paramIndex = idx + column;
            string varName = paramIndex.ToString();


            if (RoleParamNameIndexDict.TryGetValue(paramIndex, out roleParamType))
            {
                return roleParamType;
            }

            if (idx < LongParamKey)
            {
                roleParamType = new RoleParamType(varName, varName, StringParamTableName, "idx", "v" + column, idx, paramIndex, 2);
            }
            else
            {
                roleParamType = new RoleParamType(varName, varName, LongParamTableName, "idx", "v" + column, idx, paramIndex, 1);
            }

            if (roleParamType != null)
            {
                RoleParamNameIndexDict[paramIndex] = roleParamType;
            }






            return roleParamType;
        }
    }

    /// <summary>
    /// 角色的一些常见的属性
    /// </summary>
    public enum RolePropIndexs
    {
        None = 0, //无定义
        BanChat = 1,//永久禁言
        BanLogin = 2,//永久禁止登陆
        BanTrade = 3, // 禁止交易
    }

    /// <summary>
    /// 战盟建筑类型定义
    /// </summary>
    public enum ZhanMengBuilds
    {
        ZhanQi = 1,             //战旗
        JiTan = 2,              //祭坛
        JunXie = 3,             //军械
        GuangHuan = 4,          //光环
    }

    /// <summary>
    /// 职业
    /// </summary>
    public enum EOccupationType
    {
        EOT_Warrior = 0,      // 战士
        EOT_Magician = 1,      // 法师
        EOT_Bow = 2,      // 弓箭手
        EOT_MagicSword = 3,      // 魔剑士
        EOT_MAX,
    }

    /// <summary>
    /// 魔剑士初始常量
    /// </summary>
    public enum EMagicSwordInitConstant
    {
        EMSIC_InitLevel = 1, // 魔剑士初始等级
        EMSIC_InitChangeLifeCount = 2, // 魔剑士初始转生数
    }

    /// <summary>
    /// 魔剑士创建条件常量
    /// </summary>
    public enum EMagicSwordCreateNeedConstant
    {
        EMSCNC_NeedLevel = 1, // 创建魔剑士所需角色等级
        EMSCNC_NeedChangeLifeCount = 3, // 创建魔剑士所需角色转生数
    }
}