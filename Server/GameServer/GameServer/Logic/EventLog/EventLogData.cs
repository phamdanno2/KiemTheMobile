﻿using Server.Tools;
using System.Collections.Generic;
using System.Xml.Linq;
using Tmsk.Tools.Tools;

namespace GameServer.Logic
{
    #region 事件级别

    /// <summary>
    /// 事件级别
    /// </summary>
    public enum EventLevels
    {
        Ignore = -1, //忽略
        Debug = 0, //调试
        Hint = 1, //提示
        Record = 2, //记录
        Important = 3, //重要
    }

    #endregion 事件级别

    #region 角色容器类型

    /// <summary>
    /// 角色容器类型
    /// </summary>
    public enum RoleContainerType
    {
        /// <summary>
        /// 背包
        /// </summary>
        Bag = 0,

        /// <summary>
        /// 仓库
        /// </summary>
        Store = 1,
    }

    #endregion 角色容器类型

    #region 服务器日志枚举

    // 有些值需要客户端汇报上来 枚举名称不要随便改
    internal enum RoleEvent
    {
        Login,              // 登陆
        InitGame,           // 初始化游戏
        Logout,             // 登出
        CreateRole,         // 创角
        RemoveRole,         // 删除角色
        Report_InitGame,    // 客户端汇报初始化游戏
        Report_CreateRole,  // 客户端汇报创建角色

        // 日常流水
        Resource,           // 资源变化日志

        //逻辑相关
        DouLi,              // 斗笠日志

        WingStar,           // 翅膀升星
        WingUpgrade,        // 翅膀升阶
        ShengWu,            // 圣物日志
        BossDied,           // BOSS 死亡
        MiJi,               // 秘籍日志 使用道具增加属性点的

        XunZhang,           //勋章

        WriteMax,           // 只写到这里 后面的暂时都不写

        Task,
        Death,
        TongQianBuy,
        BoundTokenBuy,
        JunGongBuy,
        YinPiaoBuy,
        YuanBaoBuy,
        QiZhenGeBuy,
        QiangGouBuy,
        Sale,
        Exchange1,
        Exchange2,
        Exchange3,
        Upgrade,
        Goods,
        FallGoods,
        BoundToken,
        StoreBoundToken,
        StoreMoney,
        Horse,
        BangGong,
        JingMai,
        RefreshQiZhenGe,
        WaBao,
        Map,
        FuBenAward,
        WuXingAward,
        PaoHuanOk,
        YaBiao,
        LianZhan,
        HuoDongMonster,
        DigTreasureWithYaoShi,
        AutoSubYuanBao,
        AutoSubBoundMoney,
        MailMoneyFetch,
        TianDiJingYuanBuy,
        VipAwardGet,
        BoundMoneyBuy,
        BoundMoney,
        LieShaZhiBuy,
        ZhuangBeiJiFenBuy,
        JunGongZhiBuy,
        ZhanHunBuy,
        Token,

        //功能和监控新增日志
        MoneyEvent,

        GoodsEvent,
        TradeEvent,
        GameEvent,
        OperatorEvent,
        UserEvent,
        RoleSkill,

        PetSkill,
        UnionPalace,

        NewGiftCode,        // 礼包码(2016-5-19)

        EventMax,
    }

    #endregion 服务器日志枚举

    #region 道具的拾取类型

    internal enum GoodsPickUpType
    {
        ZhuangBei = 1,  //1:武器,装备
        YuanBao = 2,    //2:元宝
        JinBi = 3,      //3:金币
        YaoPin = 4,     //4:药品
        CaiLiao = 5,    //5:材料
        Other = 6,      //6:其他
    }

    #endregion 道具的拾取类型

    #region 操作

    /// <summary>
    /// 对物品、货币的操作类型
    /// </summary>
    public enum OpTypes
    {
        None = 0,//无类型

        #region 道具货币类

        AddOrSub = 1,//产生、充值、消耗
        Trade = 2,//交易（所有权转移）
        Move = 5,//转移(从背包到仓库等，所有权不变）
        Exchange = 6,//转换（从一种形式改为另一种形式，如银两兑换为银票）
        Input = 7,//导入（平衡数值）
        Destory = 8,//销毁（平衡数值）
        Set = 9,//设置为某个值(如GM设置)
        Fall = 10,//掉落
        Sort = 11, //整理

        #endregion 道具货币类

        #region 装备类

        Forge = 20, //锻造
        Upgrade = 21, //升级

        #endregion 装备类

        #region 活动记录

        Trace = 30,//记录
        Hold = 31,//举办
        Join = 32,//参加
        Enter = 33,//进入活动
        GiveAwards = 34,//进入活动
        Result = 35, //战斗结果

        #endregion 活动记录
    }

    public enum OpTags
    {
        None,
        CARDMONTH,
        EVERYDAYONLINE,
        LEVELUPEVENT,
        FISTRECHAGE,
        DAYRECHAGE,
        GM,

        TOTALRECHAGE = 50,
        TOTALCONSUME,
        SEVENRDAYLOGIN,
        ITEMCRAFTING,

        COMPOSECRYTAL = 100,
        SPLITCRYTAL,
        SPLITCRYTALFROMEQU,

        SHOPGETITEM = 200,
        BUYBACK,
        SYSTEM_EMAIL,

        Max,
    }

    public enum SkillLogTypes
    {
        PassiveSkillTrigger = 50, //被动技能触发
    }


    public enum LogRecordType
    {
        //通用格式
        MoneyEvent = 1, //通用货币类记录，记录：货币类型 操作类型 Tag 变化值 变化后的值 备注信息

        GoodsEvent = 2, //通用道具类记录，记录：操作类型 Tag 物品类型ID 物品DBID 变化数量 变化后的值 备注信息
        IntValue = 3, //整形值
        IntValueWithType = 4, //整形值 类型备注
        CommentOnly = 5, //备注信息
        OffsetDayId = 6, //天数值（自2011-11-11以来的天数）
        AwardsItemList = 7, //物品奖励列表
        IntValue2 = 8, //2个整形值
        IntValue3 = 9, //3个整形值
        IntValue4 = 10, //4个整形值
        IntValue5 = 11, //5个整形值
        IntValue6 = 12, //6个整形值
        IntValue7 = 13, //7个整形值
        IntValue8 = 14, //8个整形值
        IntValue9 = 15, //9个整形值
        Json = 16, //Json格式

        //专有格式
        LangHunLingYuResult = 100, // GameId, 城池ID, 胜利帮会ID

        ShenQiZaiZao = 101, //goodsId 是否成功(0或1) 失败次数
        UnionPalace = 102,//战盟神殿
        PetSkill = 103,//精灵技能

        RoleGameMapPosOrStateInvalid = 1000, // 角色地图位置状态不正常
        LangHunLingYuLongTaOnlyBangHuiLog = 1001, // 龙塔内唯一帮会ID,城池ID,唯一战盟ID,持续时间,描述
        QueryServerTotalToken = 10000, // 查询当前服务器总计钻石数
    }

    public enum RoleAttributeType
    {
        ShengWangLevel,
    }

    #endregion 操作

    #region 记录过滤

    public class LogFilterConfig
    {
        private static HashSet<int> NeedLogGoodsIdHashSet;
        private static HashSet<int> NeedLogMoneyTypeHashSet;
        private static HashSet<int> NoLogOperatorHashSet;
        private static HashSet<int> NoLogGameHashSet;

        public static bool InitConfig()
        {
            HashSet<int> goodsIdHashSet = new HashSet<int>();
            HashSet<int> moneyTypeHashSet = new HashSet<int>();
            HashSet<int> noLogGameHashSet = new HashSet<int>();
            HashSet<int> noLogOperatorHashSet = new HashSet<int>();

            string filePath = null;
            XElement xmlFile = null;
            try
            {
                filePath = Global.IsolateResPath("config\\Monitoring.xml");
                xmlFile = ConfigHelper.Load(filePath);

                IEnumerable<XElement> xmls = ConfigHelper.GetXElements(xmlFile, "Monitoring");
                if (null == xmls)
                {
                    LogManager.WriteLog(LogTypes.Fatal, string.Format("未找到配置文件{0},请联系策划负责人获取", filePath));
                    return false;
                }

                foreach (var xml in xmls)
                {
                    int type = (int)ConfigHelper.GetElementAttributeValueLong(xml, "Type", 0);
                    int code = (int)ConfigHelper.GetElementAttributeValueLong(xml, "Code", 0);
                    if (type == 1)
                    {
                        goodsIdHashSet.Add(code);
                    }
                    else if (type == 2)
                    {
                        moneyTypeHashSet.Add(code);
                    }
                    else if (type == 3)
                    {
                        noLogGameHashSet.Add(code);
                    }
                    else if (type == 4)
                    {
                        noLogOperatorHashSet.Add(code);
                    }
                    else
                    {
                        LogManager.WriteLog(LogTypes.Warning, string.Format("警告：配置文件{0},配置了未定义的类型!,{1}", filePath, xml.ToString()));
                    }
                }

                NeedLogGoodsIdHashSet = goodsIdHashSet;
                NeedLogMoneyTypeHashSet = moneyTypeHashSet;
                NoLogGameHashSet = noLogGameHashSet;
                NoLogOperatorHashSet = noLogOperatorHashSet;
            }
            catch (System.Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, string.Format("读取配置{0}发生失败,{1}", filePath, ex.Message), ex);
                return false;
            }

            return true;
        }

        public static bool LogGoodsIdLog(int goodsId)
        {
            HashSet<int> goodsIdHashSet = NeedLogGoodsIdHashSet;
            if (null != goodsIdHashSet)
            {
                return goodsIdHashSet.Contains(goodsId);
            }

            return true;
        }

        public static bool LogMoneyTypeLog(int moneyType)
        {
            HashSet<int> moneyTypeHashSet = NeedLogMoneyTypeHashSet;
            if (null != moneyTypeHashSet)
            {
                return moneyTypeHashSet.Contains(moneyType);
            }

            return true;
        }

        public static bool LogGameLog(int type)
        {
            HashSet<int> noLogGameHashSet = NoLogGameHashSet;
            if (null != noLogGameHashSet)
            {
                return !noLogGameHashSet.Contains(type);
            }

            return false;
        }

        public static bool LogOperatorLog(int type)
        {
            HashSet<int> noLogOperatorHashSet = NoLogOperatorHashSet;
            if (null != noLogOperatorHashSet)
            {
                return !noLogOperatorHashSet.Contains(type);
            }

            return false;
        }
    }

    #endregion 记录过滤
}