using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Server.Protocol;
using Server.TCP;
using Server.Tools;
//using System.Windows.Forms;
using System.Windows;
using Server.Data;
using ProtoBuf;
using GameServer.Logic;
using GameServer.Server;
using System.Collections;

using GameServer.Core.Executor;
using GameServer.KiemThe.GameDbController;

namespace GameServer.Logic
{
    #region 简单活动奖励项定义

    /// <summary>
    /// 周连续登录缓存数据
    /// </summary>
    public class WLoginItem
    {
        /// <summary>
        /// 要求的登录次数
        /// </summary>
        public int TimeOl = 0;

        /// <summary>
        /// 奖励的物品
        /// </summary>
        public List<GoodsData> GoodsDataList = null;
    };

    /// <summary>
    /// 限时登录次数缓存数据(不要求连续)
    /// </summary>
    public class LimitTimeLoginItem
    {
        /// <summary>
        /// 要求的登录次数
        /// </summary>
        public int TimeOl = 0;

        /// <summary>
        /// 奖励的物品
        /// </summary>
        public List<GoodsData> GoodsDataList = null;
    };

    /// <summary>
    /// 月在线时长缓存数据
    /// </summary>
    public class MOnlineTimeItem
    {
        /// <summary>
        /// 要求的在线时长(单位秒)
        /// </summary>
        public int TimeOl = 0;

        /// <summary>
        /// 奖励的银两
        /// </summary>
        public int BindYuanBao = 0;
    };

    /// <summary>
    /// 新手见面缓存数据
    /// </summary>
    public class NewStepItem
    {
        /// <summary>
        /// 要求的在线时长(单位秒)
        /// </summary>
        public int TimeSecs = 0;

        /// <summary>
        /// 奖励的物品
        /// </summary>
        public List<GoodsData> GoodsDataList = null;

        /// <summary>
        /// 绑定元宝
        /// </summary>
        public int BindYuanBao = 0;

        /// <summary>
        /// 绑定铜钱
        /// </summary>
        public int BindMoney = 0;
    };

    /// <summary>
    /// 大奖活动缓存数据
    /// </summary>
    public class BigAwardItem
    {
        /// <summary>
        /// 活动开始的时间
        /// </summary>
        public long StartTicks = 0;

        /// <summary>
        /// 活动结束的时间
        /// </summary>
        public long EndTicks = 0;

        /// <summary>
        /// 需要的积分字典
        /// </summary>
        public Dictionary<int, int> NeedJiFenDict = new Dictionary<int, int>();

        /// <summary>
        /// 奖励物品的字典
        /// </summary>
        public Dictionary<int, List<GoodsData>> GoodsDataListDict = new Dictionary<int,List<GoodsData>>();
    };

    /// <summary>
    /// 送礼活动缓存数据
    /// </summary>
    public class SongLiItem
    {
        /// <summary>
        /// 活动开始的时间
        /// </summary>
        public long StartTicks = 0;

        /// <summary>
        /// 活动结束的时间
        /// </summary>
        public long EndTicks = 0;

        /// <summary>
        /// 是否需要礼品码
        /// </summary>
        public int IsNeedCode = 0;

        /// <summary>
        /// 奖励的物品
        /// </summary>
        public Dictionary<int, List<GoodsData>> SongGoodsDataDict = new Dictionary<int,List<GoodsData>>();
    };

    /// <summary>
    /// 升级有礼缓存数据
    /// </summary>
    public class UpLevelItem
    {
        /// <summary>
        /// 序列ID,唯一且不可更改
        /// </summary>
        public int ID = 0;

        /// <summary>
        /// 要求等级,MU中改为包含转生等级的UnionLevel
        /// </summary>
        public int ToLevel = 0;

        /// <summary>
        /// 奖励的物品
        /// </summary>
        public List<GoodsData> GoodsDataList = null;

        /// <summary>
        /// 绑定元宝
        /// </summary>
        public int BindYuanBao = 0;

        /// <summary>
        /// 绑定铜钱
        /// </summary>
        public int BindMoney = 0;

        /// <summary>
        /// 奖励魔晶
        /// </summary>
        public int MoJing = 0;

        /// <summary>
        /// 职业限制
        /// </summary>
        public int FactionID = -1;
    };

    // MU 新增奖励数据 Begin [1/12/2014 LiaoWei]
    /// <summary>
    /// 每日在线奖励
    /// </summary>
    public class EveryDayOnLineAward
    {
        /// <summary>
        /// 要求的在线时长(单位秒)
        /// </summary>
        public int TimeSecs = 0;

        /// <summary>
        /// 掉了包ID
        /// </summary>
        public int FallPacketID = -1;

    };

    /// <summary>
    /// 连续登录奖励
    /// </summary>
    public class SeriesLoginAward
    {
        /// <summary>
        /// 要求连续登陆的次数
        /// </summary>
        public int NeedSeriesLoginNum = 0;

        /// <summary>
        /// 掉了包ID
        /// </summary>
        public int FallPacketID = -1;
    };

    // MU 新增奖励数据 End [1/12/2014 LiaoWei]

    #endregion 简单活动奖励项定义

    #region 模仿傲视活动奖励

    /// <summary>
    /// 奖励item，每一个item由一些条件和奖励品组成
    /// </summary>
    public class AwardItem
    {
        /// <summary>
        /// 奖励元宝数量【小于等于0没有元宝奖励】
        /// </summary>
        public int AwardYuanBao = 0;

        /// <summary>
        /// 奖励物品列表[字符串]600008,1,0,0,1,0|600008,1,0,0,1,0|600008,1,0,0,1,0
        /// </summary>
        //public string AwardGoodsStr = "";

        /// <summary>
        /// 奖励物品列表
        /// </summary>
        public List<GoodsData> GoodsDataList = new List<GoodsData>();

        /// <summary>
        /// 最小条件值，这个值对不同的活动意义不一样，对于充值送礼，是
        /// 最小元宝数 MinYuanBao， 对于 各种充值王，是每一个排行的最小
        /// 条件，比如第一名的最低等级，最低装备实力值，最低坐骑实力值等
        /// </summary>
        public int MinAwardCondionValue = 0;

        /// <summary>
        /// 扩展条件2
        /// </summary>
        public int MinAwardCondionValue2 = 0;

        /// <summary>
        /// 条件3
        /// </summary>
        public int MinAwardCondionValue3 = 0;
    }

    /// <summary>
    /// 有持续时间的奖励物品
    /// </summary>
    public class AwardEffectTimeItem
    {
        // 持续时间类型
        public enum EffectTimeType
        {
            ETT_Unknown = 0,
            ETT_LastMinutesFromNow = 1,     // 从发物品时刻，持续多久
            ETT_AbsoluteLastTime = 2,           // 绝对持续时间
        }

        public class TimeDetail
        {
            public EffectTimeType Type = EffectTimeType.ETT_Unknown;

            // 对持续时间的物品有效
            public int LastMinutes = 0;

            // 对绝对时间的物品有效
            public string AbsoluteStartTime = Global.ConstGoodsEndTime;
            public string AbsoluteEndTime = Global.ConstGoodsEndTime;
        }

        public void Init(string goodsList, string timeList, string note)
        {
            if (string.IsNullOrEmpty(goodsList) || string.IsNullOrEmpty(timeList))
                return;

            string[] szGoods = goodsList.Split('|');
            string[] szTime = timeList.Split('|');
            if (szGoods.Length != szTime.Length) return;

            GoodsDataList = HuodongCachingMgr.ParseGoodsDataList(szGoods, note);
            GoodsTimeList = HuodongCachingMgr.ParseGoodsTimeList(szTime, note);
        }

        private List<GoodsData> GoodsDataList = null;
        private List<TimeDetail> GoodsTimeList = null;

        public int GoodsCnt()
        {
            return GoodsDataList != null ? GoodsDataList.Count : 0;
        }

        public AwardItem ToAwardItem()
        {
            AwardItem result = new AwardItem();

            if (GoodsDataList != null)
            {
                for (int i = 0; i < GoodsDataList.Count; ++i)
                {
                    GoodsData goods = GoodsDataList[i];
                    bool bSetOk = false;
                    if (GoodsTimeList != null && GoodsTimeList.Count > i)
                    {
                        TimeDetail time = GoodsTimeList[i];
                        if (time.Type == EffectTimeType.ETT_LastMinutesFromNow)
                        {
                            DateTime now = TimeUtil.NowDateTime();
                            goods.Starttime = now.ToString("yyyy-MM-dd HH:mm:ss");
                            goods.Endtime = now.AddMinutes(time.LastMinutes).ToString("yyyy-MM-dd HH:mm:ss");
                            bSetOk = true;
                        }
                        else if (time.Type == EffectTimeType.ETT_AbsoluteLastTime)
                        {
                            goods.Starttime = time.AbsoluteStartTime;
                            goods.Endtime = time.AbsoluteEndTime;
                            bSetOk = true;
                        }
                    }

                    if (!bSetOk)
                    {
                        goods.Starttime = Global.ConstGoodsEndTime;
                        goods.Endtime = Global.ConstGoodsEndTime;
                    }

                    result.GoodsDataList.Add(goods);
                }

            }
            return result;
        }
    }

    /// <summary>
    /// 活动缓存基类
    /// </summary>
    public class Activity
    {
        //标题不用记录，对于海外版本，看不懂
        //public string Title = "";

        /// <summary>
        /// 开始日期
        /// </summary>
        public string FromDate = "";

        /// <summary>
        /// 结束日期
        /// </summary>
        public string ToDate = "";

        /// <summary>
        /// 奖励开始时间
        /// </summary>
        public string AwardStartDate = "";

        /// <summary>
        /// 奖励结束时间
        /// </summary>
        public string AwardEndDate = "";

        /// <summary>
        /// 活动类型标识
        /// </summary>
        public int ActivityType = -1;

        /// <summary>
        /// 参数验证码，大于0表示正常，小于0表示错误，等于0表示未验证,保护类型，派生类可以进行更多验证
        /// </summary>
        protected int CodeForParamsValidate = 0;

        public bool IsHeFuActivity(int type)
        {
            return (type >= (int)ActivityTypes.HeFuLogin && type <= (int)ActivityTypes.HeFuAwardTime);
        }

        public bool IsJieRiActivity(int type)
        {
            return (type == (int)ActivityTypes.JieriDaLiBao
                || type == (int)ActivityTypes.JieriDengLuHaoLi
                || type == (int)ActivityTypes.JieriCZSong
                || type == (int)ActivityTypes.JieriLeiJiCZ
                || type == (int)ActivityTypes.JieriZiKa
                || type == (int)ActivityTypes.JieriPTXiaoFeiKing
                || type == (int)ActivityTypes.JieriPTCZKing
                || type == (int)ActivityTypes.JieriBossAttack
                || type == (int)ActivityTypes.JieriTotalConsume
                || type == (int)ActivityTypes.JieriDuoBei
                || type == (int)ActivityTypes.JieriQiangGou
                || type == (int)ActivityTypes.JieriWing
                || type == (int)ActivityTypes.JieriAddon
                || type == (int)ActivityTypes.JieriStrengthen
                || type == (int)ActivityTypes.JieriAchievement
                || type == (int)ActivityTypes.JieriMilitaryRank
                || type == (int)ActivityTypes.JieriVIPFanli
                || type == (int)ActivityTypes.JieriAmulet
                || type == (int)ActivityTypes.JieriArchangel
                || type == (int)ActivityTypes.JieriMarriage
                || type == (int)ActivityTypes.JieriGive
                || type == (int)ActivityTypes.JieriGiveKing
                || type == (int)ActivityTypes.JieriRecvKing
                || type == (int)ActivityTypes.JieriLianXuCharge
                || type == (int)ActivityTypes.JieriInputPointsExchg
                || type == (int)ActivityTypes.JieriFuLi
                );
        }

        /// <summary>
        /// 检查是否在活动持续时间内
        /// </summary>
        public virtual bool InActivityTime()
        {
            // 判断该活动是否被后台配置
            if (IsHeFuActivity(ActivityType))
            {
                HeFuActivityConfig config = HuodongCachingMgr.GetHeFuActivityConfing();
                if (null == config)
                    return false;
                if (!config.InList(ActivityType))
                    return false;
            }

            if (IsJieRiActivity(ActivityType))
            {
                JieriActivityConfig config = HuodongCachingMgr.GetJieriActivityConfig();
                if (null == config)
                    return false;
                if (!config.InList(ActivityType))
                    return false;
            }

            DateTime startTime = DateTime.Parse(FromDate);
            DateTime endTime = DateTime.Parse(ToDate);
            return TimeUtil.NowDateTime() >= startTime && TimeUtil.NowDateTime() <= endTime;
        }

        /// <summary>
        /// 检查是否在领取期
        /// </summary>
        public virtual bool InAwardTime()
        {
            // 判断该活动是否被后台配置
            if (IsHeFuActivity(ActivityType))
            {
                HeFuActivityConfig config = HuodongCachingMgr.GetHeFuActivityConfing();
                if (null == config)
                    return false;
                if (!config.InList(ActivityType))
                    return false;
            }

            if (IsJieRiActivity(ActivityType))
            {
                JieriActivityConfig config = HuodongCachingMgr.GetJieriActivityConfig();
                if (null == config)
                    return false;
                if (!config.InList(ActivityType))
                    return false;
            }

            DateTime startTime = DateTime.Parse(AwardStartDate);
            DateTime endTime = DateTime.Parse(AwardEndDate);
            return TimeUtil.NowDateTime() >= startTime && TimeUtil.NowDateTime() <= endTime;
        }

        /// <summary>
        /// 根据奖励时间判断当前时间是否满足给予奖励时间,如果 AwardStartDate 和 AwardEndDate配置出错，则默认可领取
        /// </summary>
        /// <returns></returns>
        public bool CanGiveAward()
        {
            try
            {
                // 检查是否在领取期内
                if (!InAwardTime())
                    return false;

                return true;
            }
            catch (Exception)
            {

            }

            return false;
        }
        public virtual string GetAwardMinConditionValues()
        {
            return null;
        }
        public virtual List<int> GetAwardMinConditionlist()
        {
            return null;
        }
        public virtual bool CanGiveAward(KPlayer client, int index, int totalMoney)
        {
            return true;
        }
        /// <summary>
        /// 返回参数有效性验证码, 大于0 表示有效，小于0表示错误代码,派生类可进一步验证其他参数,当参数错误时，会记录日志
        /// </summary>
        /// <returns></returns>
        public virtual int GetParamsValidateCode()
        {
            if (0 != CodeForParamsValidate)
            {
                return CodeForParamsValidate;
            }

            int validateCode = 1;
            try
            {
                //当两个值都不是 -1, 再进行有效性判定,都是-1表示随时领取
                if (!(0 == FromDate.CompareTo("-1") &&
                   0 == ToDate.CompareTo("-1")))
                {
                    //判断起止时间
                    DateTime myFromDate = DateTime.Parse(FromDate);
                    DateTime myToDate = DateTime.Parse(ToDate);

                    if (myFromDate >= myToDate)
                    {
                        validateCode = -50001;
                    }
                }

                //判断奖励时间
                if (validateCode > 0)
                {
                    //当两个值都不是 -1, 再进行有效性判定,都是-1表示随时领取
                    if (!(0 == AwardStartDate.CompareTo("-1") &&
                        0 == AwardEndDate.CompareTo("-1")))
                    {
                        DateTime myFromDate = DateTime.Parse(AwardStartDate);
                        DateTime myToDate = DateTime.Parse(AwardEndDate);

                        if (myFromDate >= myToDate)
                        {
                            validateCode = -50002;
                        }
                    }
                }
            }
            catch (Exception)
            {
                validateCode = -50000;
            }

            if (validateCode < 0)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("活动【{0}】的参数验证失败，错误码{1}", GetActivityChineseName((ActivityTypes)ActivityType), validateCode));
            }

            CodeForParamsValidate = validateCode;

            return validateCode;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public virtual bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public virtual bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params1)
        {
            return true;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual bool GiveAward(KPlayer client, Int32 _params)
        {
            return true;
        }

        // 新增一个接口 [7/18/2013 LiaoWei]
        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual bool GiveAward(KPlayer client, Int32 _params1, Int32 _params2)
        {
            return true;
        }

        /// 给予奖励,虚函数,由派生类具体实现,_params 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual bool GiveAward(KPlayer client)
        {
            return true;
        }

        /// <summary>
        /// 将myAwardItem奖励给客户端，这个函数作为通用函数供派生类奖励时调用,保护类型，外部不可见
        /// 至于背包是否足够等，由外部调用getNeedBagSpaceForAwardGoods()进行判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool GiveAward(KPlayer client, AwardItem myAwardItem)
        {
            if (null == client || null == myAwardItem )
            {
                return false;
            }
            if (myAwardItem.GoodsDataList != null)
            {
                //获取奖励的物品
                for (int i = 0; i < myAwardItem.GoodsDataList.Count; i++)
                {
                    int nGoodsID = myAwardItem.GoodsDataList[i].GoodsID; // 物品id
                    //想DBServer请求加入某个新的物品到背包中
                    // 根据职业检查是否可以发放奖励

                }
            }


            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, myAwardItem.AwardYuanBao, string.Format("领取{0}活动奖励", (ActivityTypes)this.ActivityType));
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                               StringUtil.substitute(Global.GetLang("恭喜获得钻石 +{0}"), myAwardItem.AwardYuanBao),
                                                                               GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyErr, (int)HintErrCodeTypes.None);
                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }

        //[bing] 给予限时物品奖励
        protected bool GiveEffectiveTimeAward(KPlayer client, AwardItem myAwardItem)
        {
            if (null == client || null == myAwardItem)
            {
                return false;
            }

            if (myAwardItem.GoodsDataList != null)
            {
                //获取奖励的物品
                for (int i = 0; i < myAwardItem.GoodsDataList.Count; i++)
                {
                    int nGoodsID = myAwardItem.GoodsDataList[i].GoodsID; // 物品id

                    //想DBServer请求加入某个新的物品到背包中
                    // 根据职业检查是否可以发放奖励

                }
            }


            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, myAwardItem.AwardYuanBao, string.Format("领取{0}活动奖励", (ActivityTypes)this.ActivityType));
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                               StringUtil.substitute(Global.GetLang("恭喜获得钻石 +{0}"), myAwardItem.AwardYuanBao),
                                                                               GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyErr, (int)HintErrCodeTypes.None);
                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }


        public virtual List<int> GetAwardIDList()
        {
            return null;
        }

        // 新增接口 [7/17/2013 LiaoWei]
        /// <summary>
        /// 给予得到,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public virtual AwardItem GetAward(KPlayer client)
        {
            return null;
        }

        /// <summary>
        /// 获取奖励的虚函数
        /// </summary>
        /// <returns></returns>
        public virtual AwardItem GetAward(Int32 _params)
        {
            return null;
        }

        /// <summary>
        /// 给予得到,虚函数,由派生类具体实现,_params 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual AwardItem GetAward(KPlayer client, Int32 _params=0)
        {
            return null;
        }
        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1 和 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual AwardItem GetAward(KPlayer client, Int32 _params1 = 0, Int32 _params2 = 0)
        {
            return null;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1 和 参数是gamedbserver 记录完领取奖励记录后传递回来的值，由各个活动自己解释
        /// </summary>
        /// <returns></returns>
        public virtual AwardItem GetAward(KPlayer client, Int32 _params1 = 0, Int32 _params2 = 0, Int32 _params3 = 0)
        {
            return null;
        }
        /// <summary>
        /// 通过活动类型返回活动中文名称，记录日志用,配置文件中的中文title在海外版本会变成外语，记录了看不明白
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetActivityChineseName(ActivityTypes type)
        {
            string activityName = type.ToString();

            switch (type)
            {
                case ActivityTypes.InputFirst:
                    {
                        activityName = "首充大礼";
                        break;
                    }
                case ActivityTypes.InputFanLi:
                    {
                        activityName = "充值返利";
                        break;
                    }
                case ActivityTypes.InputJiaSong:
                    {
                        activityName = "充值加送";
                        break;
                    }
                case ActivityTypes.InputKing:
                    {
                        activityName = "充值王";
                        break;
                    }
                case ActivityTypes.LevelKing:
                    {
                        activityName = "冲级王";
                        break;
                    }
                case ActivityTypes.EquipKing:
                    {
                        activityName = "装备王";
                        break;
                    }
                case ActivityTypes.HorseKing:
                    {
                        activityName = "坐骑王";
                        break;
                    }
                case ActivityTypes.JingMaiKing:
                    {
                        activityName = "经脉王";
                        break;
                    }
                case ActivityTypes.JieriDaLiBao:
                    {
                        activityName = "节日大礼包";
                        break;
                    }
                case ActivityTypes.JieriDengLuHaoLi:
                    {
                        activityName = "节日登录豪礼";
                        break;
                    }
                case ActivityTypes.JieriVIP:
                    {
                        activityName = "节日VIP大礼";
                        break;
                    }
                case ActivityTypes.JieriCZSong:
                    {
                        activityName = "节日充值送礼";
                        break;
                    }
                case ActivityTypes.JieriLeiJiCZ:
                    {
                        activityName = "节日累计充值大礼";
                        break;
                    }
                case ActivityTypes.JieriZiKa:
                    {
                        activityName = "节日字卡换礼盒";
                        break;
                    }
                case ActivityTypes.JieriPTXiaoFeiKing:
                    {
                        activityName = "节日消费王";
                        break;
                    }
                case ActivityTypes.JieriPTCZKing:
                    {
                        activityName = "节日充值王";
                        break;
                    }
                case ActivityTypes.JieriBossAttack:
                    {
                        activityName = "节日Boss攻城";
                        break;
                    }
                case ActivityTypes.HeFuLogin:
                    {
                        activityName = "合服登陆豪礼";
                        break;
                    }
                //case ActivityTypes.HeFuVIP:
                //    {
                //        activityName = "合服VIP大礼";
                //        break;
                //    }
                case ActivityTypes.HeFuTotalLogin:
                    {
                        activityName = "合服累计登陆";
                        break;
                    }
                case ActivityTypes.HeFuPKKing:
                    {
                        activityName = "合服PK王大礼";
                        break;
                    }
                //case ActivityTypes.HeFuWanChengKing:
                //    {
                //        activityName = "合服王城霸主大礼";
                //        break;
                //    }
                case ActivityTypes.HeFuRecharge:
                    {
                        activityName = "合服充值返利";
                        break;
                    }
                case ActivityTypes.XinCZFanLi:
                    {
                        activityName = "新区充值返利";
                        break;
                    }
                case ActivityTypes.HeFuBossAttack:
                    {
                        activityName = "合服Boss攻城";
                        break;
                    }
                case ActivityTypes.MeiRiChongZhiHaoLi:          // 日常豪礼 begin [7/16/2013 LiaoWei]
                {
                    activityName = "每日充值豪礼";
                    break;
                }
                case ActivityTypes.ChongJiLingQuShenZhuang:
                {
                    activityName = "充级领取神装";
                    break;
                }
                case ActivityTypes.ShenZhuangJiQingHuiKui:
                {
                    activityName = "神装激情回赠";
                    break;
                }                                              // 日常豪礼 end [7/16/2013 LiaoWei]

                case ActivityTypes.JieriWing:
                {
                    activityName = "节日翅膀返利";
                    break;
                }
                case ActivityTypes.JieriAddon:
                {
                    activityName = "节日追加返利";
                    break;
                }
                case ActivityTypes.JieriStrengthen:
                {
                    activityName = "节日强化返利";
                    break;
                }
                case ActivityTypes.JieriAchievement:
                {
                    activityName = "节日成就返利";
                    break;
                }
                case ActivityTypes.JieriMilitaryRank:
                {
                    activityName = "节日军衔返利";
                    break;
                }
                case ActivityTypes.JieriVIPFanli:
                {
                    activityName = "节日VIP返利";
                    break;
                }
                case ActivityTypes.JieriAmulet:
                {
                    activityName = "节日护身符返利";
                    break;
                }
                case ActivityTypes.JieriArchangel:
                {
                    activityName = "节日大天使返利";
                    break;
                }
                case ActivityTypes.JieriMarriage:
                {
                    activityName = "节日婚姻返利";
                    break;
                }
                case ActivityTypes.JieriGive:
                {
                    activityName = "节日赠送";
                    break;
                }
                case ActivityTypes.JieriGiveKing:
                {
                    activityName = "节日赠送王";
                    break;
                }
                case ActivityTypes.JieriRecv:
                {
                    activityName = "节日收取";
                    break;
                }
                case ActivityTypes.JieriRecvKing:
                {
                    activityName = "节日收取王";
                    break;
                }
                case ActivityTypes.JieriLianXuCharge:
                {
                    activityName = "节日连续充值";
                    break;
                }
                case ActivityTypes.JieriInputPointsExchg:
                {
                    activityName = "节日充值点数兑换";
                    break;
                }
                    /*
                    {
                        JieriActivityConfig config = HuodongCachingMgr.GetJieriActivityConfig();
                        if(null == config)
                            break;

                        activityName = config.GetActivityName((int)type);
                    }
                    break;
                    */

                default:
                    break;
            }

            return activityName;
        }

        /// <summary>
        /// 时间转换，针对-1，时间如果配置成-1，表示没限制
        /// </summary>
        public void PredealDateTime()
        {
            //当两个值都是 -1,表示任意范围，这儿给定比较大的范围,
            //这个时间值不要乱改，因为数据库中每个活动的key都由两个值生成，
            //一旦运行中动态改动这两个值在-1情况下的默认值，则会导致多领奖励
            if ((0 == FromDate.CompareTo("-1") &&
                   0 == ToDate.CompareTo("-1")))
            {
                FromDate = "2008-08-08 08:08:08";
                ToDate = "2028-08-08 08:08:08";
            }

            //这个时间值可以随便改
            if ((0 == AwardStartDate.CompareTo("-1") &&
            0 == AwardEndDate.CompareTo("-1")))
            {
                AwardStartDate = "2008-08-08 08:08:08";
                AwardEndDate = "2028-08-08 08:08:08";
            }
        }

        /// <summary>
        /// 检查限制条件 虚函数
        /// extTag = 领取选项
        /// </summary>
        public virtual bool CheckCondition(KPlayer client, int extTag)
        {
            return true;
        }
    }

    /// <summary>
    /// 【充值王，坐骑王，装备王等】王类活动缓存基类
    /// </summary>
    public class KingActivity : Activity
    {
        public Dictionary<int, int> RoleLimit = new Dictionary<int, int>();
        /// <summary>
        /// 通用奖励映射表，key是排行值，value是对应的奖励和基础条件
        /// </summary>
        public Dictionary<int, AwardItem> AwardDict = new Dictionary<int, AwardItem>();

        /// <summary>
        /// 职业奖励映射表，key是排行值，value是对应的奖励和基础条件
        /// </summary>
        public Dictionary<int, AwardItem> AwardDict2 = new Dictionary<int, AwardItem>();

        /// <summary>
        /// 返回奖励限制字符串[依次是第一名的最小限制值，第二名的最小限制值,.....]
        /// </summary>
        /// <returns></returns>
        public override string GetAwardMinConditionValues()
        {
            StringBuilder strBuilder = new StringBuilder();
            int paiHang = 1;
            int maxPaiHang = AwardDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardDict.ContainsKey(paiHang))
                {
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append("_");
                    }

                    strBuilder.Append(AwardDict[paiHang].MinAwardCondionValue);
                }
            }
            return strBuilder.ToString();
        }
        public override List<int> GetAwardMinConditionlist()
        {
            List<int> cons = new List<int>();
            int paiHang = 1;
            int maxPaiHang = AwardDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardDict.ContainsKey(paiHang))
                    cons.Add(AwardDict[paiHang].MinAwardCondionValue);
            }
            return cons;
        }
        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是排行值
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = null;

            if (AwardDict.ContainsKey(_params))
            {
                myAwardItem = AwardDict[_params];
            }

            if (null == myAwardItem )
            {
                return false;
            }

            //调用基类奖励函数 奖励玩家物品
           // GiveAward(client, myAwardItem);

            // 给奖励2
            GiveAward(client, _params, client.m_cPlayerFaction.GetFactionId());

            return true;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1是按键索引 _params2是职业
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params1, Int32 _params2)
        {
            AwardItem myAwardItem = null;

            // 给通用奖励
            if (AwardDict.ContainsKey(_params1))
                myAwardItem = AwardDict[_params1];

            if (null == myAwardItem )
                return false;

            GiveAward(client, myAwardItem);

            //给职业奖励
            if (AwardDict2.ContainsKey(_params1))
            {
                myAwardItem = AwardDict2[_params1];
            }

            if (null == myAwardItem )
                return false;

            GiveAwardByOccupation(client, myAwardItem, _params2);

            return true;
        }

        /// <summary>
        /// 将myAwardItem奖励给客户端，这个函数作为通用函数供派生类奖励时调用,保护类型，外部不可见
        /// 至于背包是否足够等，由外部调用getNeedBagSpaceForAwardGoods()进行判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool GiveAwardByOccupation(KPlayer client, AwardItem myAwardItem, int occupation)
        {
            if (null == client || null == myAwardItem )
            {
                return false;
            }

            //获取奖励的物品
            if(myAwardItem.GoodsDataList!=null&&myAwardItem.GoodsDataList.Count>0)
            {
                int count = myAwardItem.GoodsDataList.Count; // 奖品个数

                // 发放奖品
                for (int i = 0; i < count; i++)
                {
                    //想DBServer请求加入某个新的物品到背包中
                    //添加物品
                    GoodsData data = myAwardItem.GoodsDataList[i];

                    // 根据职业检查是否可以发放奖励

                }
            }

            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                client, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType));
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                               StringUtil.substitute(Global.GetLang("恭喜获得钻石 +{0}"), myAwardItem.AwardYuanBao),
                                                                               GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyErr, (int)HintErrCodeTypes.None);
                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 nBtnIndex)
        {

            if (Global.CanAddGoodsDataList(client, AwardDict[nBtnIndex].GoodsDataList))
            {
                // 其实只有一个物品 但不想改通用的接口 所以从AwardDict2取出对应的GoodsData...
                int nOccu = Global.CalcOriginalOccupationID(client);// gwz 修改 2014.6.19
                List<GoodsData> lData = new List<GoodsData>();
                foreach (GoodsData item in AwardDict[nBtnIndex].GoodsDataList)
                {
                    lData.Add(item);
                }
                if (AwardDict2.ContainsKey(nBtnIndex))
                {
                    int count = AwardDict2[nBtnIndex].GoodsDataList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        GoodsData data = AwardDict2[nBtnIndex].GoodsDataList[i];
                        // TO DO GÌ ĐÓ Ở ĐÂY
                    }

                }


                return Global.CanAddGoodsDataList(client, lData);
            }
            return false;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            int needSpace = 0;
            int maxKey = -1;
            foreach (var key in AwardDict.Keys)
            {
                if (needSpace < AwardDict[key].GoodsDataList.Count)
                {
                    needSpace = AwardDict[key].GoodsDataList.Count;
                    maxKey = key;
                }
            }

            if (-1 == maxKey)
            {
                return true;
            }
            int nOcc = Global.CalcOriginalOccupationID(client);
            //判断背包空格是否能提交接受奖励的物品
            if (!Global.CanAddGoodsDataList(client, AwardDict[maxKey].GoodsDataList))
            {
                return false;
            }
            if (!AwardDict2.ContainsKey(maxKey) || AwardDict2[maxKey].GoodsDataList == null || AwardDict2[maxKey].GoodsDataList.Count==0)
                return true;
            if (Global.CanAddGoodsDataList(client, AwardDict2[maxKey].GoodsDataList))
            {
                // 其实只有一个物品 但不想改通用的接口 所以从AwardDict2取出对应的GoodsData...
                int nOccu = Global.CalcOriginalOccupationID(client);// gwz 修改 2014.6.17
                List<GoodsData> lData = new List<GoodsData>();
                foreach (GoodsData item in AwardDict[maxKey].GoodsDataList)
                {
                    lData.Add(item);
                }
                // 大天使武器只能发一个，原来是根据职业取
                // 但是现在魔剑士没有专属的大天使武器，魔剑士与战士法师公用大天使武器
                // 就像上面说的 只有一个物品，这里就不根据职业取了，因为魔剑士没有。。会取到数组索引越界
                // 最简单的 解决办法 直接默认取第一个
                // 再去检查背包数量就好了，一样ok [XSea 2015/6/4]
                lData.Add(AwardDict2[maxKey].GoodsDataList[0]); /*lData.Add(AwardDict2[maxKey].GoodsDataList[nOccu]); */

                return Global.CanAddGoodsDataList(client, lData);
            }

            return true;
        }
    }

    ///<summary>
    /// 首次充值大礼  [3/21/2014 LiaoWei]
    /// </summary>
    public class FirstChongZhiGift :Activity
    {
        public AwardItem AwardDict = new AwardItem();

        public AwardItem AwardDict2 = new AwardItem();

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1是按键索引 _params2是职业
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client)
        {
            // 给通用奖励
            if (null == AwardDict)
                return false;

            GiveAward(client, AwardDict);

            //给职业奖励

            if (null == AwardDict2)
                return false;

            GiveAwardByOccupation(client, AwardDict2, client.m_cPlayerFaction.GetFactionId());

            return true;
        }

        /// <summary>
        /// 将myAwardItem奖励给客户端，这个函数作为通用函数供派生类奖励时调用,保护类型，外部不可见
        /// 至于背包是否足够等，由外部调用getNeedBagSpaceForAwardGoods()进行判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool GiveAwardByOccupation(KPlayer client, AwardItem myAwardItem, int occupation)
        {
            if (null == client || null == myAwardItem )
            {
                return false;
            }

            //获取奖励的物品
            if (myAwardItem.GoodsDataList != null && myAwardItem.GoodsDataList.Count > 0)
            {
                int count = myAwardItem.GoodsDataList.Count;
                for (int i = 0; i < count; i++)
                {
                    //想DBServer请求加入某个新的物品到背包中
                    //添加物品
                    GoodsData data = myAwardItem.GoodsDataList[i];
                    // 根据职业检查是否可以发放奖励

                }
            }

            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, myAwardItem.AwardYuanBao, string.Format("领取{0}活动奖励", (ActivityTypes)this.ActivityType));
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                               StringUtil.substitute(Global.GetLang("恭喜获得钻石 +{0}"), myAwardItem.AwardYuanBao),
                                                                               GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyErr, (int)HintErrCodeTypes.None);
                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, int nOcc) //nOcc参数并未用到，使用的是角色职业
        {
            if (AwardDict.GoodsDataList.Count <= 0 && AwardDict2.GoodsDataList.Count <= 0)
            {
                return true;
            }

            if ( Global.CanAddGoodsDataList(client, AwardDict.GoodsDataList))
            {
                int nOccu = Global.CalcOriginalOccupationID(client);// gwz 修改 2014.8.1
                List<GoodsData> lData = new List<GoodsData>();
                foreach (GoodsData item in AwardDict.GoodsDataList)
                {
                    lData.Add(item);
                }

                int count = AwardDict2.GoodsDataList.Count;
                for (int i = 0; i < count; i++)
                {
                    GoodsData data = AwardDict2.GoodsDataList[i];

                        lData.Add(AwardDict2.GoodsDataList[i]);
                }
                return Global.CanAddGoodsDataList(client, lData);
            }
            return false;

        }

    }

    ///<summary>
    /// [bing] 节日返利 翅膀返利,追加返利,强化返利,成就返利,军衔返利,VIP返利,护符返利,大天使返利,婚姻返利
    /// </summary>
    public class JieriFanLiActivity : Activity
    {
        // 存放奖励的map
        // 奖励表 key=奖励类型
        public Dictionary<int, AwardItem> AwardDict = new Dictionary<int, AwardItem>();     //GoodsOne

        public Dictionary<int, AwardItem> AwardDict2 = new Dictionary<int, AwardItem>();    //GoodsTwo

        public Dictionary<int, AwardEffectTimeItem> AwardDict3 = new Dictionary<int, AwardEffectTimeItem>();    //GoodsThr

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是排行值
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = null;

            if (AwardDict.ContainsKey(_params))
            {
                myAwardItem = AwardDict[_params];
            }

            if (null == myAwardItem )
            {
                return false;
            }

            // 给奖励2
            GiveAward(client, _params, client.m_cPlayerFaction.GetFactionId());

            return true;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1是按键索引 _params2是职业
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params1, Int32 _params2)
        {
            AwardItem myAwardItem = null;

            // 给通用奖励
            if (AwardDict.ContainsKey(_params1))
                myAwardItem = AwardDict[_params1];

            if (null == myAwardItem )
                return false;

            GiveAward(client, myAwardItem);

            myAwardItem = null;

            //给职业奖励
            if (AwardDict2.ContainsKey(_params1))
            {
                myAwardItem = AwardDict2[_params1];
            }

            if (null != myAwardItem )
                GiveAwardByOccupation(client, myAwardItem, _params2);

            //给时效性奖励
            if (AwardDict3.ContainsKey(_params1))
            {
                myAwardItem = AwardDict3[_params1].ToAwardItem();
                GiveEffectiveTimeAward(client, myAwardItem);
            }

            return true;
        }

        /// <summary>
        /// 将myAwardItem奖励给客户端，这个函数作为通用函数供派生类奖励时调用,保护类型，外部不可见
        /// 至于背包是否足够等，由外部调用getNeedBagSpaceForAwardGoods()进行判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool GiveAwardByOccupation(KPlayer client, AwardItem myAwardItem, int occupation)
        {
            if (null == client || null == myAwardItem )
            {
                return false;
            }

            //获取奖励的物品
            if(myAwardItem.GoodsDataList!=null&&myAwardItem.GoodsDataList.Count>0)
            {
                int count = myAwardItem.GoodsDataList.Count;
                for (int i = 0; i < count; i++)
                {
                    //想DBServer请求加入某个新的物品到背包中
                    //添加物品
                    GoodsData data = myAwardItem.GoodsDataList[i];
                    // 根据职业检查是否可以发放奖励

                }
            }

            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                client, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType));
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                                                               StringUtil.substitute(Global.GetLang("恭喜获得钻石 +" + myAwardItem.AwardYuanBao)),
                                                                               GameInfoTypeIndexes.Normal, ShowGameInfoTypes.OnlyErr, (int)HintErrCodeTypes.None);
                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            int needSpace = 0;
            int maxKey = -1;
            int nOccu = Global.CalcOriginalOccupationID(client);
            foreach (var key in AwardDict.Keys)
            {
                int tmpNeedSpace = AwardDict[key].GoodsDataList.Count;
                tmpNeedSpace += (AwardDict2[key].GoodsDataList.Count > 0 ? 1 : 0);
                tmpNeedSpace += AwardDict3[key].GoodsCnt();

                if (needSpace < tmpNeedSpace)
                {
                    needSpace = tmpNeedSpace;
                    maxKey = key;
                }
            }

            if (-1 == maxKey)
                return true;

            List<GoodsData> lData = new List<GoodsData>();

            //One
            foreach (GoodsData item in AwardDict[maxKey].GoodsDataList)
            {
                lData.Add(item);
            }

            //Two 职业奖励
            int count = AwardDict2[maxKey].GoodsDataList.Count;
            for (int i = 0; i < count; i++)
            {
                GoodsData data = AwardDict2[maxKey].GoodsDataList[i];

            }

            //Tre
            AwardItem tmpAwardItem = AwardDict3[maxKey].ToAwardItem();
            foreach (GoodsData item in tmpAwardItem.GoodsDataList)
            {
                lData.Add(item);
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, lData);
        }

        /// <summary>
        /// 检查条件
        /// </summary>
        public override bool CheckCondition(KPlayer client, int extTag)
        {
            AwardItem myAwardItem = null;

            if (AwardDict.ContainsKey(extTag))
            {
                myAwardItem = AwardDict[extTag];
            }

            if (null == myAwardItem)
            {
                return false;
            }
            return true;
        }
    }

    ///<summary>
    /// 充值返利缓存数据
    /// </summary>
    public class InputFanLiActivity : Activity
    {
        /// <summary>
        /// 返利百分比
        /// </summary>
        public double FanLiPersent = 0;

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是活动期间充值元宝数量
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = new AwardItem();

            myAwardItem.AwardYuanBao = (Int32)(_params * FanLiPersent);

            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, myAwardItem);
        }
    };

    /// <summary>
    /// 充值送礼缓存数据
    /// </summary>
    public class InputSongActivity : Activity
    {
        public AwardItem MyAwardItem = new AwardItem();

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是角色在活动期间的元宝数量,
        /// 这个函数被调用，表示gamedbserver处理成功，可以奖励，不再需要进一步判断_params
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, MyAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (MyAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);
        }
    };

    #endregion 模仿傲视活动奖励

    #region 大型节日活动奖励

    /// <summary>
    /// 节日活动开启配置
    /// </summary>
    public class JieriActivityConfig
    {
        public Dictionary<int, string> ConfigDict = new Dictionary<int, string>();
        public Dictionary<int, string> ActivityNameDict = new Dictionary<int, string>();

        public List<int> openList = new List<int>();
        public bool InList(int type)
        {
            if (ConfigDict.ContainsKey(type))
                return true;
            return false;
        }
        public string GetFileName(int type)
        {
            if (ConfigDict.ContainsKey(type))
                return ConfigDict[type];
            return null;
        }
        public string GetActivityName(int type)
        {
            if (ConfigDict.ContainsKey(type))
                return ActivityNameDict[type];
            return null;
        }
    }

    /// <summary>
    /// 节日大礼包缓存数据
    /// </summary>
    public class JieriDaLiBaoActivity : Activity
    {
        public AwardItem MyAwardItem = new AwardItem();
        public Dictionary<int, AwardItem> OccAwardItemDict = new Dictionary<int, AwardItem>();

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            if (null == client)
                return false;

            bool result = true;
            while (true)
            {
                if (null != MyAwardItem)
                {
                    result = GiveAward(client, MyAwardItem);
                }
                if (false == result)
                {
                    break;
                }
                // 取得玩家职业
                int occupation = client.m_cPlayerFaction.GetFactionId();
                AwardItem myOccAward = GetOccAward(occupation);
                if (null != myOccAward)
                {
                    result = GiveAward(client, myOccAward);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 返回职业奖励列表
        /// </summary>
        /// <returns></returns>
        public AwardItem GetOccAward(Int32 _params)
        {
            AwardItem myOccAward = null;

            if (OccAwardItemDict.ContainsKey(_params))
                myOccAward = OccAwardItemDict[_params];

            return myOccAward;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (null == client)
                return false;

            // 取得玩家职业
            int occupation = client.m_cPlayerFaction.GetFactionId();
            AwardItem myOccAward = GetOccAward(occupation);

            if (MyAwardItem.GoodsDataList.Count <= 0 && (null == myOccAward || myOccAward.GoodsDataList.Count <= 0))
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);
        }
    };

    /// <summary>
    /// 节日累计登陆缓存数据
    /// </summary>
    public class JieRiDengLuActivity : Activity
    {
        /// <summary>
        /// 通用奖励
        /// key = 天数
        /// </summary>
        public Dictionary<int, AwardItem> AwardItemDict = new Dictionary<int, AwardItem>();

        /// <summary>
        /// 区分职业的奖励
        /// key = 天数 * 100 + 职业
        /// </summary>
        public Dictionary<int, AwardItem> OccAwardItemDict = new Dictionary<int, AwardItem>();

        /// <summary>
        /// 返回通用奖励列表
        /// </summary>
        /// <returns></returns>
        public override AwardItem GetAward(KPlayer client, int day)
        {
            AwardItem myAward = null;

            if (AwardItemDict.ContainsKey(day))
                myAward = AwardItemDict[day];

            return myAward;
        }

        /// <summary>
        /// 返回职业奖励列表
        /// </summary>
        /// <returns></returns>
        public AwardItem GetOccAward(KPlayer client, int day)
        {
            if (null == client)
                return null;

            AwardItem myOccAward = null;

            int key = day * 100 + client.m_cPlayerFaction.GetFactionId();
            if (OccAwardItemDict.ContainsKey(key))
                myOccAward = OccAwardItemDict[key];

            return myOccAward;
        }

        /// <summary>
        /// 返回职业奖励列表
        /// </summary>
        /// <returns></returns>
        public AwardItem GetOccAward(int key)
        {
            AwardItem myOccAward = null;

            if (OccAwardItemDict.ContainsKey(key))
                myOccAward = OccAwardItemDict[key];

            return myOccAward;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            if (null == client)
                return false;

            // 取得玩家职业
            AwardItem myAwardItem = GetAward(client, _params);
            bool result = true;
            while (true)
            {
                if (null != myAwardItem)
                {
                    result = GiveAward(client, myAwardItem);
                }
                if (false == result)
                {
                    break;
                }
                // 取得玩家职业
                int occupation = client.m_cPlayerFaction.GetFactionId();
                AwardItem myOccAward = GetOccAward(occupation);
                if (null != myOccAward)
                {
                    result = GiveAward(client, myOccAward);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            if (null == client)
                return false;

            // 取得玩家职业
            AwardItem myAwardItem = GetAward(client, _params);
            AwardItem myOccAward = GetOccAward(client, _params);
            List<GoodsData> GoodsDataList = new List<GoodsData>();
            if (null != myAwardItem && null != myAwardItem.GoodsDataList)
                GoodsDataList.AddRange(myAwardItem.GoodsDataList);
            if (null != myOccAward && null != myOccAward.GoodsDataList)
                GoodsDataList.AddRange(myOccAward.GoodsDataList);

            if (GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, GoodsDataList);
        }
    }

    /// <summary>
    /// 节日VIP大礼包缓存数据
    /// </summary>
    public class JieriVIPActivity : Activity
    {
        public AwardItem MyAwardItem = new AwardItem();

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是角色在活动期间的元宝数量,
        /// 这个函数被调用，表示gamedbserver处理成功，可以奖励，不再需要进一步判断_params
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, MyAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (MyAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);
        }
    };

    /// <summary>
    /// 节日充值加送大礼包缓存数据
    /// </summary>
    public class JieriCZSongActivity : Activity
    {
        // 充值加送通用奖励
        // key是档位
        public Dictionary<int, AwardItem> AwardItemDict = new Dictionary<int, AwardItem>();
        // 充值加送职业奖励
        // 改为key是档位 原key为档位*100 + 职业 [XSea 2015/6/4]
        public Dictionary<int, AwardItem> OccAwardItemDict = new Dictionary<int, AwardItem>();

        // 根据档位查找通用奖励
        public override AwardItem GetAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (AwardItemDict.ContainsKey(_params))
                myAwardItem = AwardItemDict[_params];
            return myAwardItem;
        }

        // 根据档位查找职业奖励
        public AwardItem GetOccAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (OccAwardItemDict.ContainsKey(_params))
                myAwardItem = OccAwardItemDict[_params];
            return myAwardItem;
        }

        /// <summary>
        /// 根据档位给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            bool result = true;
            while (true)
            {
                if (null != myAwardItem)
                {
                    result = GiveAward(client, myAwardItem);
                }
                if (false == result)
                {
                    break;
                }
                // 取得玩家职业
                /*int occupation = client.FactionID;*/
                AwardItem myOccAward = GetOccAward(_params); // 改为通过档位获取，发奖接口会过滤职业 [XSea 2015/6/4]
                if (null != myOccAward)
                {
                    result = GiveAward(client, myOccAward);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            AwardItem myOccAwardItem = GetOccAward(_params);
            List<GoodsData> GoodsDataList = new List<GoodsData>();
            if (null != myAwardItem )
            {
                GoodsDataList.AddRange(myAwardItem.GoodsDataList);
            }

            if (null != myOccAwardItem )
            {
                GoodsDataList.AddRange(myOccAwardItem.GoodsDataList);
            }

            if (GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, GoodsDataList);
        }
    };

    /// <summary>
    /// 节日累计充值奖励
    /// </summary>
    public class JieRiLeiJiCZActivity : Activity
    {
        // 累计充值通用奖励
        // key是档位
        public Dictionary<int, AwardItem> AwardItemDict = new Dictionary<int, AwardItem>();
        // 累计充值职业奖励
        // 改为key是档位 原key为档位*100 + 职业 [XSea 2015/6/4]
        public Dictionary<int, AwardItem> OccAwardItemDict = new Dictionary<int, AwardItem>();

        // 根据档位查找通用奖励
        public override AwardItem GetAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (AwardItemDict.ContainsKey(_params))
                myAwardItem = AwardItemDict[_params];
            return myAwardItem;
        }

        // 根据档位查找职业奖励
        public AwardItem GetOccAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (OccAwardItemDict.ContainsKey(_params))
                myAwardItem = OccAwardItemDict[_params];
            return myAwardItem;
        }

        /// <summary>
        /// 根据档位给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            bool result = true;
            while (true)
            {
                if (null != myAwardItem)
                {
                    result = GiveAward(client, myAwardItem);
                }
                if (false == result)
                {
                    break;
                }
                // 取得玩家职业
                /*int occupation = client.FactionID;*/
                AwardItem myOccAward = GetOccAward(_params); // 改为通过档位获取，发奖接口会过滤职业 [XSea 2015/6/4]
                if (null != myOccAward)
                {
                    result = GiveAward(client, myOccAward);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            AwardItem myOccAwardItem = GetOccAward(_params);
            List<GoodsData> GoodsDataList = new List<GoodsData>();
            if (null != myAwardItem )
            {
                GoodsDataList.AddRange(myAwardItem.GoodsDataList);
            }

            if (null != myOccAwardItem )
            {
                GoodsDataList.AddRange(myOccAwardItem.GoodsDataList);
            }

            if (GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, GoodsDataList);
        }

        public override string GetAwardMinConditionValues()
        {
            StringBuilder strBuilder = new StringBuilder();
            int paiHang = 1;
            int maxPaiHang = AwardItemDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardItemDict.ContainsKey(paiHang))
                {
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append("_");
                    }

                    strBuilder.Append(AwardItemDict[paiHang].MinAwardCondionValue);
                }
            }
            return strBuilder.ToString();
        }
    };

    /// <summary>
    /// 节日累计消费奖励
    /// </summary>
    public class JieRiTotalConsumeActivity : Activity
    {
        // 累计消费通用奖励
        // key是档位
        public Dictionary<int, AwardItem> AwardItemDict = new Dictionary<int, AwardItem>();
        // 累计消费职业奖励
        // 改为key是档位 原key为档位*100+职业 [XSea 2015/6/4]
        public Dictionary<int, AwardItem> OccAwardItemDict = new Dictionary<int, AwardItem>();

        // 根据档位查找通用奖励
        public override AwardItem GetAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (AwardItemDict.ContainsKey(_params))
                myAwardItem = AwardItemDict[_params];
            return myAwardItem;
        }

        // 根据档位查找职业奖励
        public AwardItem GetOccAward(Int32 _params)
        {
            AwardItem myAwardItem = null;
            if (OccAwardItemDict.ContainsKey(_params))
                myAwardItem = OccAwardItemDict[_params];
            return myAwardItem;
        }

        /// <summary>
        /// 根据档位给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            bool result = true;
            while (true)
            {
                if (null != myAwardItem)
                {
                    result = GiveAward(client, myAwardItem);
                }
                if (false == result)
                {
                    break;
                }
                // 取得玩家职业
                /*int occupation = Global.CalcChangeOccupationID(client);*/
                AwardItem myOccAward = GetOccAward(_params); // 改为通过档位获取，发奖接口会过滤职业 [XSea 2015/6/4]
                if (null != myOccAward)
                {
                    result = GiveAward(client, myOccAward);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            AwardItem myOccAwardItem = GetOccAward(_params);
            List<GoodsData> GoodsDataList = new List<GoodsData>();
            if (null != myAwardItem )
            {
                GoodsDataList.AddRange(myAwardItem.GoodsDataList);
            }

            if (null != myOccAwardItem )
            {
                GoodsDataList.AddRange(myOccAwardItem.GoodsDataList);
            }

            if (GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, GoodsDataList);
        }

        public override string GetAwardMinConditionValues()
        {
            StringBuilder strBuilder = new StringBuilder();
            int paiHang = 1;
            int maxPaiHang = AwardItemDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardItemDict.ContainsKey(paiHang))
                {
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append("_");
                    }

                    strBuilder.Append(AwardItemDict[paiHang].MinAwardCondionValue);
                }
            }
            return strBuilder.ToString();
        }
    };

    /// <summary>
    /// 节日多倍活动的子配置项
    /// </summary>
    public class JieRiMultConfig
    {
        public int index;
        public int type;
        public double Multiplying;
        public int Effective;
        public string StartDate;
        public string EndDate;

        public double GetMult()
        {
            // 激活
            if (Effective == 0)
                return 0.0;

            JieRiMultAwardActivity activity = HuodongCachingMgr.GetJieRiMultAwardActivity();
            if (null == activity)
                return 0.0;

            if (!activity.InActivityTime())
                return 0.0;

            // 时间内
            if (InActivityTime() == false)
                return 0.0;

            if (Multiplying < 1.0)
                return 0.0;

            return Multiplying - 1.0;
        }

        public bool InActivityTime()
        {
            // 判断该活动是否被后台配置
            JieriActivityConfig config = HuodongCachingMgr.GetJieriActivityConfig();
            if (null == config)
                return false;
            if (!config.InList((int)ActivityTypes.JieriDuoBei))
                return false;

            JieRiMultAwardActivity activity = HuodongCachingMgr.GetJieRiMultAwardActivity();
            if (null == activity)
                return false;

            if (!activity.InActivityTime())
                return false;

            DateTime startTime = DateTime.Parse(StartDate);
            DateTime endTime = DateTime.Parse(EndDate);
            return TimeUtil.NowDateTime() >= startTime && TimeUtil.NowDateTime() <= endTime;
        }
    }

    /// <summary>
    /// 增加节日活动奖励翻倍配置
    /// </summary>
    public class JieRiMultAwardActivity : Activity
    {
        // 参与奖励翻倍的活动类型列表
        public Dictionary<int, JieRiMultConfig> activityDict = new Dictionary<int, JieRiMultConfig>();
        // 判断活动类型是否在配置里
        public JieRiMultConfig GetConfig(int type)
        {
            JieRiMultConfig config = null;
            if (activityDict.ContainsKey(type))
                config = activityDict[type];
            return config;
        }
    }

    public class JieRiZiKa
    {
        /// <summary>
        /// 兑换的类型
        /// </summary>
        public int type;

        /// <summary>
        /// id
        /// </summary>
        public int id;

        /// <summary>
        /// 一个角色每天最多兑换的个数
        /// </summary>
        public int DayMaxTimes = 0;

        /// <summary>
        /// 合成前的需要的物品表
        /// </summary>
        public List<GoodsData> NeedGoodsList = null;

        /// <summary>
        /// 所需魔晶
        /// </summary>
        public int NeedMoJing;

        /// <summary>
        /// 所需祈福积分
        /// </summary>
        public int NeedQiFuJiFen;

        /// <summary>
        /// 所需精灵积分
        /// </summary>
        public int NeedPetJiFen;

        /// <summary>
        /// 合成的物品
        /// </summary>
        public AwardItem MyAwardItem = new AwardItem();

    }

    /// <summary>
    /// 节日字卡换礼盒缓存数据
    /// </summary>
    public class JieRiZiKaLiaBaoActivity : Activity
    {
        /// <summary>
        /// 配置字典
        /// key = id
        /// </summary>
        public Dictionary<int, JieRiZiKa> JieRiZiKaDict = new Dictionary<int, JieRiZiKa>();

        public List<int> GetIndexByType(int type)
        {
            List<int> IndexList = new List<int>();
            foreach (KeyValuePair<int, JieRiZiKa> item in JieRiZiKaDict )
            {
                if (type == item.Value.type)
                {
                    IndexList.Add(item.Key);
                }
            }
            return IndexList;
        }

        /// <summary>
        /// 根据id找到兑换的配置
        /// key = id
        /// </summary>
        public JieRiZiKa GetAward(int id)
        {
            JieRiZiKa config = null;
            if (JieRiZiKaDict.ContainsKey(id))
                config = JieRiZiKaDict[id];
            return config;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是角色在活动期间的元宝数量,
        /// 这个函数被调用，表示gamedbserver处理成功，可以奖励，不再需要进一步判断_params
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            JieRiZiKa config = GetAward(_params);
            if (null == config)
                return false;
            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, config.MyAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            JieRiZiKa config = GetAward(_params);
            if (null == config)
                return false;
            if (null == config.MyAwardItem)
                return false;
            if (config.MyAwardItem.GoodsDataList.Count <= 0)
                return true;

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, config.MyAwardItem.GoodsDataList);
        }
    };

    ///<summary>
    /// 新的充值返利缓存数据
    /// </summary>
    public class XinFanLiActivity : KingActivity
    {
        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是活动期间充值元宝数量
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = new AwardItem();

            myAwardItem.AwardYuanBao = (Int32)(_params);

            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, myAwardItem);
        }
    };

    ///<summary>
    /// 合服充值返利缓存数据
    /// </summary>
    public class HeFuFanLiActivity : KingActivity
    {
        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是活动期间充值元宝数量
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = new AwardItem();

            myAwardItem.AwardYuanBao = (Int32)(_params);

            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, myAwardItem);
        }

        /// <summary>
        /// 返回奖励限制字符串[依次是第一名的最小限制值，第二名的最小限制值,.....]
        /// </summary>
        /// <returns></returns>
        override public string GetAwardMinConditionValues()
        {
            StringBuilder strBuilder = new StringBuilder();
            int paiHang = 1;
            int maxPaiHang = AwardDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardDict.ContainsKey(paiHang))
                {
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append("_");
                    }

                    strBuilder.Append(string.Format("{0},{1}", AwardDict[paiHang].MinAwardCondionValue, AwardDict[paiHang].MinAwardCondionValue2));
                }
            }
            return strBuilder.ToString();
        }
    };

    #endregion 大型节日活动奖励

    #region 合服活动和新加的开区返利活动

    public class HeFuActivityConfig
    {
        public List<int> openList = new List<int>();
        public bool InList(int type)
        {
            foreach (var item in openList)
            {
                if (item == type)
                    return true;
            }
            return false;
        }
    }

    // 领取奖励枚举
    enum HeFuLoginAwardType
    {
        NormalAward = 1,
        VIPAward = 2,
    };

    /// <summary>
    /// 合服活动登陆送豪礼
    /// </summary>
    public class HeFuLoginActivity : Activity
    {
        // 存放奖励的map
        // 奖励表 key=奖励类型
        public Dictionary<int, AwardItem> AwardDict = new Dictionary<int, AwardItem>();


        // 取得某中的奖励列表
        public override AwardItem GetAward(Int32 _params)
        {
            AwardItem AwardList = null;
            if (AwardDict.ContainsKey(_params))
                AwardList = AwardDict[_params];
            return AwardList;
        }

        /// <summary>
        /// 背包中是否有足够的位置放置奖励
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            if (null == myAwardItem)
                return true;

            if (myAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, myAwardItem.GoodsDataList);
        }


        /// <summary>
        /// 根据类型给予玩家奖励
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            if (null == myAwardItem)
                return false;

            return GiveAward(client, myAwardItem);
        }

    };


    public class HeFuRechargeData
    {
       public float Coe;	// 返利比例
       public int LowLimit;	// 下限
    };

    /// <summary>
    /// 合服充值返利配置
    /// </summary>
    public class HeFuRechargeActivity : Activity
    {
        // 奖励表 key=累计登陆的天数
        public Dictionary<int, HeFuRechargeData> ConfigDict = new Dictionary<int, HeFuRechargeData>();

        // 取得某个名次的返利比例
        public HeFuRechargeData getDataByDay(int rank)
        {
            HeFuRechargeData data = null;
            if (ConfigDict.ContainsKey(rank))
                data = ConfigDict[rank];
            return data;
        }
        public string strcoe;
    };

    /// <summary>
    /// 合服累计登陆活动
    /// </summary>
    public class HeFuTotalLoginActivity : Activity
    {
        // 奖励表 key=累计登陆的天数
        public Dictionary<int, AwardItem> AwardDict = new Dictionary<int,AwardItem>();

        // 取得某天的奖励列表
        public override AwardItem GetAward(Int32 _params)
        {
            AwardItem AwardList = null;
            if (AwardDict.ContainsKey(_params))
                AwardList = AwardDict[_params];
            return AwardList;
        }

        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            if (null == myAwardItem)
                return true;

            if (myAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, myAwardItem.GoodsDataList);
        }

        /// <summary>
        /// 根据天数给予玩家奖励
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = GetAward(_params);
            if (null == myAwardItem)
                return false;

            return GiveAward(client, myAwardItem);
        }
    };

    /// <summary>
    /// 合服PK王缓存数据
    /// </summary>
    public class HeFuPKKingActivity : Activity
    {
        public AwardItem MyAwardItem = new AwardItem();

        // 战场之神的认定条件
        // 战场之神需要连续3天在PK之王活动中占据第一位置
        // x天 默认3天
        public int winerCount = 3;

        /// <summary>
        /// 给予奖励,虚函数,
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client)
        {
            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, MyAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (MyAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);
        }
    };

    /// <summary>
    /// 合服罗兰城主奖励数据
    /// </summary>
    public class HeFuLuoLanAward
    {
        public int winNum = 0;
        public int status = 0;
        public AwardItem awardData = new AwardItem();
    }

    /// <summary>
    /// 合服罗兰城主缓存数据
    /// </summary>
    public class HeFuLuoLanActivity : Activity
    {
        /// <summary>
        /// 奖励字典
        /// </summary>
        public Dictionary<int, HeFuLuoLanAward> HeFuLuoLanAwardDict = new Dictionary<int, HeFuLuoLanAward>();

        /// <summary>
        /// 获取奖励信息
        /// </summary>
        public HeFuLuoLanAward GetHeFuLuoLanAward(Int32 _param)
        {
            if (HeFuLuoLanAwardDict.ContainsKey(_param))
            {
                return HeFuLuoLanAwardDict[_param];
            }

            return null;
        }

        /// <summary>
        /// 获取奖励信息
        /// </summary>
        public AwardItem GetAward(Int32 _param)
        {
            if (HeFuLuoLanAwardDict.ContainsKey(_param))
            {
                return HeFuLuoLanAwardDict[_param].awardData;
            }

            return null;
        }

        /// <summary>
        /// 发放奖励 传入名次和地位
        /// </summary>
        public override bool GiveAward(KPlayer client, Int32 _param)
        {
            AwardItem awardData = GetAward(_param);

            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, awardData);
        }

        /// <summary>
        /// 判断背包是否足够 传入名次和地位
        /// </summary>
        public bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 _param)
        {
            AwardItem awardData = GetAward(_param);

            if (null == awardData || awardData.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, awardData.GoodsDataList);
        }
    }

    /// <summary>
    /// 增加合服活动奖励翻倍配置
    /// </summary>
    /// <returns></returns>
    public class HeFuAwardTimesActivity : Activity
    {
        // 参与奖励翻倍的活动类型列表
        public List<int> activityList = new List<int>();
        // 配置的活动经验倍数
        public float activityTimes;
        public int specialTimeID;
        // 判断活动类型是否在配置里
        public bool InActivityList(int value)
        {
            return activityList.Contains(value);
        }
    }

    /// <summary>
    /// 合服王城霸主缓存数据
    /// </summary>
    public class HeFuWCKingActivity : Activity
    {
        public AwardItem MyAwardItem = new AwardItem();

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,对于king类活动，_params是角色在活动期间的元宝数量,
        /// 这个函数被调用，表示gamedbserver处理成功，可以奖励，不再需要进一步判断_params
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            //调用基类奖励函数 奖励玩家物品
            return GiveAward(client, MyAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (MyAwardItem.GoodsDataList.Count <= 0)
            {
                return true;
            }

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);
        }
    };

    #endregion 合服活动和新加的开区返利活动


    // 日常豪礼相关 begin [7/16/2013 LiaoWei]
    #region 日常豪礼相关

    /// <summary>
    /// 1.每日充值豪礼
    /// </summary>
    public class MeiRiChongZhiActivity : Activity
    {
        /// <summary>
        /// 奖励映射表，key是充值金额级别，value是对应的奖励物品
        /// </summary>
        public Dictionary<int, AwardItem> AwardDict = new Dictionary<int, AwardItem>();


        // 获得AwardItem信息
        public override AwardItem GetAward(KPlayer client, Int32 _params)
        {
            if (AwardDict.ContainsKey(_params))
                return AwardDict[_params];
            else
                return null;
        }
        public override List<int> GetAwardMinConditionlist()
        {
            List<int> cons = new List<int>();
            int paiHang = 1;
            int maxPaiHang = AwardDict.Count;

            for (paiHang = 1; paiHang <= maxPaiHang; paiHang++)
            {
                if (AwardDict.ContainsKey(paiHang))
                    cons.Add(AwardDict[paiHang].MinAwardCondionValue);
            }
            return cons;
        }

        /// <summary>
        /// 根据充值要求得到对应充值金额级别
        /// </summary>
        /// <returns></returns>
        public int GetIDByYuanBao(int NeedYuanbao)
        {
            foreach (var kvp in AwardDict)
            {
                if (kvp.Value.MinAwardCondionValue == NeedYuanbao)
                    return kvp.Key;
            }
            return -1;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params是给奖励的等级
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            AwardItem myAwardItem = null;

            if (AwardDict.ContainsKey(_params))
            {
                myAwardItem = AwardDict[_params];
            }

            if (null == myAwardItem )
            {
                return false;
            }



            return GiveAward(client, myAwardItem);
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 nBtnIndex)
        {

            int needSpace = 0;


            return Global.CanAddGoodsNum(client, needSpace);
        }
    };

    /// <summary>
    /// 2.冲级豪礼   2014 6.5 gwz 修改
    /// </summary>
    public class ChongJiHaoLiActivity : KingActivity
    {
        /// <summary>
        /// 奖励映射表，key是充值金额级别，value是对应的奖励物品
        /// </summary>
       // public Dictionary<int, AwardItem> AwardDict1 = new Dictionary<int, AwardItem>();    // 通用奖励
       // public Dictionary<int, AwardItem> AwardDict2 = new Dictionary<int, AwardItem>();    // 职业奖励

        // 获得AwardItem信息
        public override AwardItem GetAward(KPlayer client, Int32 _params)
        {
            if (AwardDict.ContainsKey(_params))
                return AwardDict[_params];
            else
                return null;
        }
        // 获得AwardItem信息
        public override AwardItem GetAward(KPlayer client, Int32 _params1, Int32 _params2)
        {
            if (_params2 == 1)
            {
                if (AwardDict.ContainsKey(1))
                    return AwardDict[1];
            }
            else if (_params2 == 2)
            {
                if (AwardDict2.ContainsKey(_params1))
                    return AwardDict2[_params1];
            }

            return null;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现,_params1是按键索引 _params2是职业
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params1, Int32 _params2)
        {
            AwardItem myAwardItem = null;

            // 给通用奖励
            if (AwardDict.ContainsKey(_params1))
                myAwardItem = AwardDict[_params1];

            if (null == myAwardItem )
                return false;

            GiveAward(client, myAwardItem);

            //给职业奖励
            if (AwardDict2.ContainsKey(_params1))
            {
                myAwardItem = AwardDict2[_params1];
            }

            if (null == myAwardItem)
                return false;

            GiveAwardByOccupation(client, myAwardItem, _params2);

            return true;
        }

        /// <summary>
        /// 将myAwardItem奖励给客户端，这个函数作为通用函数供派生类奖励时调用,保护类型，外部不可见
        /// 至于背包是否足够等，由外部调用getNeedBagSpaceForAwardGoods()进行判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected  new bool GiveAwardByOccupation(KPlayer client, AwardItem myAwardItem, int occupation)
        {
            if (null == client || null == myAwardItem )
            {
                return false;
            }

            //获取奖励的物品
            if (myAwardItem.GoodsDataList != null && myAwardItem.GoodsDataList.Count > 0)
            {
                int count = myAwardItem.GoodsDataList.Count;
                for (int i = 0; i < count; i++)
                {
                    //想DBServer请求加入某个新的物品到背包中
                    //添加物品
                    GoodsData data = myAwardItem.GoodsDataList[i];
                    // 根据职业检查是否可以发放奖励

                }
            }

            //获取奖励的元宝
            if (myAwardItem.AwardYuanBao > 0)
            {
                GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, myAwardItem.AwardYuanBao, string.Format("领取{0}活动奖励", (ActivityTypes)this.ActivityType));

                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    client.RoleID, myAwardItem.AwardYuanBao, string.Format(Global.GetLang("领取{0}活动奖励"), (ActivityTypes)this.ActivityType)),
                    null, client.ServerId);
            }

            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client, Int32 nBtnIndex)
        {
            if ( Global.CanAddGoodsDataList(client, AwardDict[nBtnIndex].GoodsDataList))
            {
                // 其实只有一个物品 但不想改通用的接口 所以从AwardDict2取出对应的GoodsData...
                int nOccu = Global.CalcOriginalOccupationID(client);// gwz 修改 2014.6.19
                List<GoodsData> lData = new List<GoodsData>();
                foreach (GoodsData item in AwardDict[nBtnIndex].GoodsDataList)
                {
                    lData.Add(item);
                }
                if (AwardDict2.ContainsKey(nBtnIndex))
                {
                    int count = AwardDict2[nBtnIndex].GoodsDataList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        GoodsData data = AwardDict2[nBtnIndex].GoodsDataList[i];
                        // TODO GÌ ĐÓ
                            lData.Add(AwardDict2[nBtnIndex].GoodsDataList[i]);
                    }

                }


                return Global.CanAddGoodsDataList(client, lData);
            }
            return false;
        }
    };


    /// <summary>
    /// 3.神装激情回馈
    /// </summary>
    public class ShenZhuangHuiKuiHaoLiActivity : Activity
    {
        /// <summary>
        /// 奖励映射表，key是充值金额级别，value是对应的奖励物品
        /// </summary>
        public AwardItem MyAwardItem = new AwardItem();

        // 获得AwardItem信息
        public override AwardItem GetAward(KPlayer client)
        {
            return MyAwardItem;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            GiveAward(client, MyAwardItem);
            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            if (MyAwardItem.GoodsDataList.Count <= 0)
                return true;

            //判断背包空格是否能提交接受奖励的物品
            return Global.CanAddGoodsDataList(client, MyAwardItem.GoodsDataList);

        }
    };

    /// <summary>
    /// 4.月度大转盘
    /// </summary>
    public class YueDuZhuanPanActivity : Activity
    {
        /// <summary>
        /// 奖励映射表，key是充值金额级别，value是对应的奖励物品
        /// </summary>
        //public AwardItem MyAwardItem = new AwardItem();

        // 获得AwardItem信息
        public override AwardItem GetAward(KPlayer client)
        {
            return null;
        }

        /// <summary>
        /// 给予奖励,虚函数,由派生类具体实现
        /// </summary>
        /// <returns></returns>
        public override bool GiveAward(KPlayer client, Int32 _params)
        {
            return true;
        }

        /// <summary>
        /// 背包中是否有足够的位置
        /// </summary>
        /// <returns></returns>
        public override bool HasEnoughBagSpaceForAwardGoods(KPlayer client)
        {
            return true;
        }
    };


    #endregion 日常豪礼相关
    // 日常豪礼相关 end [7/16/2013 LiaoWei]


    #region 拉升付费和在线的新加奖励

    /// <summary>
    /// 升级到60得元宝/升级到100得IPhone5S的奖励项
    /// </summary>
    public class UpLevelAwardItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 至少几天内
        /// </summary>
        public int MinDay { get; set; }

        /// <summary>
        /// 最多几天内
        /// </summary>
        public int MaxDay { get; set; }

        /// <summary>
        /// 奖励的元宝
        /// </summary>
        public int AwardYuanBao { get; set; }
    }

    /// <summary>
    /// 开服在线奖励
    /// </summary>
    public class KaiFuGiftItem
    {
        /// <summary>
        /// Day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 最少在线(小时)
        /// </summary>
        public int MinTime { get; set; }

        /// <summary>
        /// 最低级别
        /// </summary>
        public int MinLevel { get; set; }

        /// <summary>
        /// 奖励的元宝
        /// </summary>
        public int AwardYuanBao { get; set; }
    }

    #endregion 拉升付费和在线的新加奖励

    #region 奖励缓存和处理

    /// <summary>
    /// 系统送礼中的活动配置表缓存
    /// </summary>
    public class HuodongCachingMgr
    {
        #region 静态成员

        /// <summary>
        /// 节日活动开始时推送的tick计时
        /// </summary>
        static long lastJieRiProcessTicks = 0;

        /// <summary>
        /// 节日活动开始时推送的状态
        /// </summary>
        static int JieRiState = 0;

        /// <summary>
        /// 节日活动开始时推送的状态
        /// </summary>
        static int HefuState = 1;// 默认开始

        #endregion

        #region 简单奖励处理

        #region 通用函数

        /// <summary>
        /// 将物品字符串列表解析成物品数据列表
        /// </summary>
        /// <param name="goodsStr"></param>
        /// <returns></returns>
        public static List<GoodsData> ParseGoodsDataList(string[] fields, string fileName)
        {
            List<GoodsData> goodsDataList = new List<GoodsData>();
            for (int i = 0; i < fields.Length; i++)
            {
                string[] sa = fields[i].Split(',');
                if (sa.Length != 7)
                {
                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析{0}文件中的奖励项时失败, 物品配置项个数错误", fileName));
                    continue;
                }

                int[] goodsFields = Global.StringArray2IntArray(sa);

                //获取物品数据  liaowei -- MU 改变 物品ID,物品数量,是否绑定,强化等级,追加等级,是否有幸运,卓越属性
                GoodsData goodsData = Global.GetNewGoodsData(goodsFields[0], goodsFields[1], 0, goodsFields[3], goodsFields[2], 0, goodsFields[5], 0, goodsFields[6], goodsFields[4], 0);
                goodsDataList.Add(goodsData);
            }

            return goodsDataList;
        }

        public static List<AwardEffectTimeItem.TimeDetail> ParseGoodsTimeList(string[] fields, string fileName)
        {
            if (fields == null)
                return null;

            var result = new List<AwardEffectTimeItem.TimeDetail>();

            foreach (var str in fields)
            {
                AwardEffectTimeItem.TimeDetail detail = new AwardEffectTimeItem.TimeDetail();
                string[] szTime = str.Split(',');
                int type = Convert.ToInt32(szTime[0]);
                bool bReadOK = false;
                if (type == (int)AwardEffectTimeItem.EffectTimeType.ETT_LastMinutesFromNow)
                {
                    if (szTime.Length == 2)
                    {
                        detail.Type = AwardEffectTimeItem.EffectTimeType.ETT_LastMinutesFromNow;
                        detail.LastMinutes = Convert.ToInt32(szTime[1]);
                        bReadOK = true;
                    }
                }
                else if (type == (int)AwardEffectTimeItem.EffectTimeType.ETT_AbsoluteLastTime)
                {
                    if (szTime.Length == 3)
                    {
                        detail.Type = AwardEffectTimeItem.EffectTimeType.ETT_AbsoluteLastTime;
                        detail.AbsoluteStartTime = szTime[1];
                        detail.AbsoluteEndTime = szTime[2];
                        bReadOK = true;
                    }
                }

                if (!bReadOK)
                {
                    detail.Type = AwardEffectTimeItem.EffectTimeType.ETT_AbsoluteLastTime;
                    detail.AbsoluteStartTime = Global.ConstGoodsEndTime;
                    detail.AbsoluteEndTime = Global.ConstGoodsEndTime;
                }

                result.Add(detail);
            }

            return result;
        }

        /// <summary>
        /// 将物品字符串列表解析成物品数据列表
        /// </summary>
        /// <param name="goodsStr"></param>
        /// <returns></returns>
        private static List<GoodsData> ParseGoodsDataList2(string[] fields, string fileName)
        {
            List<GoodsData> goodsDataList = new List<GoodsData>();
            for (int i = 0; i < fields.Length; i++)
            {
                string[] sa = fields[i].Split(',');
                if (sa.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析{0}文件中的奖励项时失败, 物品配置项个数错误", fileName));
                    continue;
                }

                int[] goodsFields = Global.StringArray2IntArray(sa);

                //获取物品数据
                GoodsData goodsData = Global.GetNewGoodsData(goodsFields[0], goodsFields[1], 0, 0, 0, 0, 0, 0);
                goodsDataList.Add(goodsData);
            }

            return goodsDataList;
        }

        /// <summary>
        /// 从活动配置的日期中解析时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ParseDateTime(string str)
        {
            int yue = str.IndexOf('月');
            if (-1 == yue) return "";
            int ri = str.IndexOf('日');
            if (-1 == ri) return "";
            int shi = str.IndexOf('时');
            if (-1 == shi) return "";
            int fen = str.IndexOf('分');
            if (-1 == fen) return "";

            string yueStr = str.Substring(0, yue);
            if (string.IsNullOrEmpty(yueStr)) return "";
            string riStr = str.Substring(yue + 1, ri - yue - 1);
            if (string.IsNullOrEmpty(riStr)) return "";
            string shiStr = str.Substring(ri + 1, shi - ri - 1);
            if (string.IsNullOrEmpty(shiStr)) return "";
            string fenStr = str.Substring(shi + 1, fen - shi - 1);
            if (string.IsNullOrEmpty(fenStr)) return "";

            int year = TimeUtil.NowDateTime().Year;
            return string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", year, yueStr, riStr, shiStr, fenStr, 0);
        }

        /// <summary>
        /// 将配置表中的时间转换为真正的时间[7月21日23时59分]
        /// </summary>
        /// <param name="monthDate"></param>
        /// <returns></returns>
        public static long GetHuoDongDateTime(string str)
        {
            string strDateTime = ParseDateTime(str);
            return Global.SafeConvertToTicks(strDateTime);
        }

        /// <summary>
        /// 将配置表中的时间转换为真正的时间[针对 2010-10-10 09:11:00]
        /// </summary>
        /// <param name="monthDate"></param>
        /// <returns></returns>
        public static long GetHuoDongDateTimeForCommonTimeString(string str)
        {
            return Global.SafeConvertToTicks(str);
        }

        /// <summary>
        /// 根据输入数值获取位的设置值
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static int GetBitValue(int whichOne)
        {
            int bitVal = 0;

            //标记已经领取过了指定的礼物
            if (1 == whichOne)
            {
                bitVal = 1;
            }
            else if (2 == whichOne)
            {
                bitVal = 2;
            }
            else if (3 == whichOne)
            {
                bitVal = 4;
            }
            else if (4 == whichOne)
            {
                bitVal = 8;
            }
            else if (5 == whichOne)
            {
                bitVal = 16;
            }
            else if (6 == whichOne)
            {
                bitVal = 32;
            }
            else if (7 == whichOne)
            {
                bitVal = 64;
            }

            return bitVal;
        }

        #endregion 通用函数

        #region 周连续登录奖励处理

        /// <summary>
        /// 周连续登录的项字典
        /// </summary>
        private static Dictionary<int, WLoginItem> _WLoginDict = new Dictionary<int, WLoginItem>();

        /// <summary>
        /// 获取周连续登录的物品列表
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static WLoginItem GetWLoginItem(int whichOne)
        {
            WLoginItem wLoginItem = null;
            lock (_WLoginDict)
            {
                if (_WLoginDict.TryGetValue(whichOne, out wLoginItem))
                {
                    return wLoginItem;
                }
            }

            SystemXmlItem systemWeekLoginItem = null;
            if (!GameManager.systemWeekLoginGiftMgr.SystemXmlItemDict.TryGetValue(whichOne, out systemWeekLoginItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位周连续登录配置项失败, WhichOne={0}", whichOne));
                return null;
            }

            int timeOl = systemWeekLoginItem.GetIntValue("TimeOl");
            wLoginItem = new WLoginItem()
            {
                TimeOl = timeOl,
                GoodsDataList = null,
            };

            lock (_WLoginDict)
            {
                _WLoginDict[whichOne] = wLoginItem;
            }

            List<GoodsData> goodsDataList = null;
            string goodsIDs = systemWeekLoginItem.GetStringValue("GoodsIDs");
            if (string.IsNullOrEmpty(goodsIDs))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位周连续登录配置项中的物品奖励失败, WhichOne={0}", whichOne));
                return wLoginItem;
            }

            string[] fields = goodsIDs.Split('|');
            if (fields.Length <= 0)
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位周连续登录配置项中的物品奖励失败, WhichOne={0}", whichOne));
                return wLoginItem;
            }

            //将物品字符串列表解析成物品数据列表
            goodsDataList = ParseGoodsDataList(fields, "周连续登录配置");

            wLoginItem.GoodsDataList = goodsDataList;
            return wLoginItem;
        }

        /// <summary>
        /// 重置获取周连续登录的物品列表, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetWLoginItem()
        {
            int ret = GameManager.systemWeekLoginGiftMgr.ReloadLoadFromXMlFile();

            lock (_WLoginDict)
            {
                _WLoginDict.Clear();
            }

            return ret;
        }

        /// <summary>
        /// 处理角色获取周连续登录礼物的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetWLoginGift(KPlayer client, int whichOne)
        {
            WLoginItem wLoginItem = HuodongCachingMgr.GetWLoginItem(whichOne);
            if (null == wLoginItem)
            {
                return -1;
            }

            //如果物品不存在，或者物品个数为空
            if (null == wLoginItem.GoodsDataList || wLoginItem.GoodsDataList.Count <= 0)
            {
                return -5;
            }

            if (client.MyHuodongData.LoginNum < wLoginItem.TimeOl)
            {
                //不处理
                return -10;
            }

            //根据输入数值获取位的设置值
            int bitVal = GetBitValue(whichOne);

            //是否已经领取
            if ((client.MyHuodongData.LoginGiftState & bitVal) == bitVal)
            {
                //不处理
                return -100;
            }

            //给予礼物
            //判断背包空格是否能提交接受奖励的物品
            if (!Global.CanAddGoodsDataList(client, wLoginItem.GoodsDataList))
            {
                return -200;
            }

            //获取奖励的物品
            for (int i = 0; i < wLoginItem.GoodsDataList.Count; i++)
            {
                //想DBServer请求加入某个新的物品到背包中
                //添加物品
                //Global.AddGoodsDBCommand(Global._TCPManager.TcpOutPacketPool, client, wLoginItem.GoodsDataList[i].GoodsID, wLoginItem.GoodsDataList[i].GCount, wLoginItem.GoodsDataList[i].Quality, "", wLoginItem.GoodsDataList[i].Forge_level, wLoginItem.GoodsDataList[i].Binding, 0, "", true, 1, "周连续登录奖励", Global.ConstGoodsEndTime, wLoginItem.GoodsDataList[i].AddPropIndex, wLoginItem.GoodsDataList[i].BornIndex, wLoginItem.GoodsDataList[i].Lucky, wLoginItem.GoodsDataList[i].Strong);
                   // ADD QUÀ VÒA TÚI ĐỒ
            }

            //设置领取标志
            client.MyHuodongData.LoginGiftState = client.MyHuodongData.LoginGiftState | bitVal;

            //数据库命令更新活动数据事件
            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //发送活动数据给客户端
            GameManager.ClientMgr.NotifyHuodongData(client);

            return 0;
        }

        #endregion 周连续登录奖励处理

        #region 月连续在线奖励处理

        /// <summary>
        /// 月在线时长的项字典
        /// </summary>
        private static Dictionary<int, MOnlineTimeItem> _MonthTimeDict = new Dictionary<int, MOnlineTimeItem>();

        /// <summary>
        /// 获取月在线时长的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static MOnlineTimeItem GetMOnlineTimeItem(int whichOne)
        {
            MOnlineTimeItem mOnlineTimeItem = null;
            lock (_MonthTimeDict)
            {
                if (_MonthTimeDict.TryGetValue(whichOne, out mOnlineTimeItem))
                {
                    return mOnlineTimeItem;
                }
            }

            SystemXmlItem systemMOnlineTimeItem = null;
            if (!GameManager.systemMOnlineTimeGiftMgr.SystemXmlItemDict.TryGetValue(whichOne, out systemMOnlineTimeItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位月在线时长配置项失败, WhichOne={0}", whichOne));
                return null;
            }

            int timeOl = Global.GMax(systemMOnlineTimeItem.GetIntValue("TimeOl"), 0) * 3600;
            int bindYuanBao = Global.GMax(systemMOnlineTimeItem.GetIntValue("BindYuanBao"), 0);

            mOnlineTimeItem = new MOnlineTimeItem()
            {
                TimeOl = timeOl,
                BindYuanBao = bindYuanBao,
            };

            lock (_MonthTimeDict)
            {
                _MonthTimeDict[whichOne] = mOnlineTimeItem;
            }

            return mOnlineTimeItem;
        }

        /// <summary>
        /// 重置获取月在线时长的项, 以便下次访问时强迫从配置文件中获取
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetMOnlineTimeItem()
        {
            int ret = GameManager.systemMOnlineTimeGiftMgr.ReloadLoadFromXMlFile();

            lock (_MonthTimeDict)
            {
                _MonthTimeDict.Clear();
            }

            return ret;
        }

        /// <summary>
        /// 处理角色获取月在线时长礼物的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetMOnlineTimeGift(KPlayer client, int whichOne)
        {
            MOnlineTimeItem mOnlineTimeItem = HuodongCachingMgr.GetMOnlineTimeItem(whichOne);
            if (null == mOnlineTimeItem)
            {
                return -1;
            }

            if (client.MyHuodongData.CurMTime < mOnlineTimeItem.TimeOl)
            {
                //不处理
                return -10;
            }

            //根据输入数值获取位的设置值
            int bitVal = GetBitValue(whichOne);

            //是否已经领取
            if ((client.MyHuodongData.OnlineGiftState & bitVal) == bitVal)
            {
                //不处理
                return -100;
            }

            //获取奖励的物品
            GameManager.ClientMgr.AddUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, mOnlineTimeItem.BindYuanBao, "月在线时长礼物");

            //设置领取标志
            client.MyHuodongData.OnlineGiftState = client.MyHuodongData.OnlineGiftState | bitVal;

            //数据库命令更新活动数据事件
            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //发送活动数据给客户端
            GameManager.ClientMgr.NotifyHuodongData(client);

            return 0;
        }

        #endregion 月连续在线奖励处理

        #region 新手见面有礼

        /// <summary>
        /// 新手见面的项字典
        /// </summary>
        private static Dictionary<int, NewStepItem> _NewStepDict = new Dictionary<int, NewStepItem>();

        /// <summary>
        /// 获取新手见面的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static NewStepItem GetNewStepItem(int step)
        {
            NewStepItem newStepItem = null;
            lock (_NewStepDict)
            {
                if (_NewStepDict.TryGetValue(step, out newStepItem))
                {
                    return newStepItem;
                }
            }

            SystemXmlItem systemNewRoleItem = null;
            if (!GameManager.systemNewRoleGiftMgr.SystemXmlItemDict.TryGetValue(step, out systemNewRoleItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位见面有礼配置项失败, Step={0}", step));
                return null;
            }

            int timeSecs = Global.GMax(systemNewRoleItem.GetIntValue("TimeSecs"), 0) * 60;

            newStepItem = new NewStepItem()
            {
                TimeSecs = timeSecs,
                GoodsDataList = null,
            };

            lock (_NewStepDict)
            {
                _NewStepDict[step] = newStepItem;
            }

            List<GoodsData> goodsDataList = null;
            string goodsIDs = systemNewRoleItem.GetStringValue("GoodsIDs");
            if (string.IsNullOrEmpty(goodsIDs))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位见面有礼配置项失败, Step={0}", step));
                return newStepItem;
            }

            string[] fields = goodsIDs.Split('|');
            if (fields.Length <= 0)
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位见面有礼配置项失败, Step={0}", step));
                return newStepItem;
            }

            //将物品字符串列表解析成物品数据列表
            goodsDataList = ParseGoodsDataList(fields, "见面有礼配置项");

            newStepItem.GoodsDataList = goodsDataList;

            newStepItem.BindMoney = systemNewRoleItem.GetIntValue("BindMoney");
            newStepItem.BindYuanBao = systemNewRoleItem.GetIntValue("BindYuanBao");

            return newStepItem;
        }

        /// <summary>
        /// 重置获取新手见面的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetNewStepItem()
        {
            int ret = GameManager.systemNewRoleGiftMgr.ReloadLoadFromXMlFile();

            lock (_NewStepDict)
            {
                _NewStepDict.Clear();
            }

            return ret;
        }

        /// <summary>
        /// 处理角色获取新手见面礼物的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetNewStepGift(KPlayer client, int step)
        {
            NewStepItem newStepItem = HuodongCachingMgr.GetNewStepItem(step + 1); //配置中是从1开始的。
            if (null == newStepItem)
            {
                return -1;
            }

            //如果物品不存在，或者物品个数为空
            if (null == newStepItem.GoodsDataList || newStepItem.GoodsDataList.Count <= 0)
            {
                return -5;
            }

            //判断领取的步骤是否正确
            if ((client.MyHuodongData.NewStep) != step)
            {
                //不处理
                return -10;
            }

            //如果还未到领取的时间
            long nowTicks = TimeUtil.NOW();
            if (nowTicks - client.MyHuodongData.StepTime < (newStepItem.TimeSecs * 1000))
            {
                int subSecs = newStepItem.TimeSecs - (int)((nowTicks - client.MyHuodongData.StepTime) / 1000);
                return 0 - (10000 + subSecs);
            }

            //给予礼物
            //判断背包空格是否能提交接受奖励的物品
            if (!Global.CanAddGoodsDataList(client, newStepItem.GoodsDataList))
            {
                return -200;
            }

            //获取奖励的物品
            for (int i = 0; i < newStepItem.GoodsDataList.Count; i++)
            {
                //想DBServer请求加入某个新的物品到背包中
                //添加物品

               // ADD QUÀ VÒA TÚI ĐỒ
               // Global.AddGoodsDBCommand(Global._TCPManager.TcpOutPacketPool, client, newStepItem.GoodsDataList[i].GoodsID, newStepItem.GoodsDataList[i].GCount, newStepItem.GoodsDataList[i].Quality, "", newStepItem.GoodsDataList[i].Forge_level, newStepItem.GoodsDataList[i].Binding, 0, "", true, 1, "新手见面奖品", Global.ConstGoodsEndTime, newStepItem.GoodsDataList[i].AddPropIndex, newStepItem.GoodsDataList[i].BornIndex, newStepItem.GoodsDataList[i].Lucky, newStepItem.GoodsDataList[i].Strong);
            }

            //奖励绑定铜钱
            int tongQian = newStepItem.BindMoney;
            if (tongQian > 0)
            {


                //给用户加钱,更新用户的铜钱
                GameManager.ClientMgr.AddMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, tongQian, "Tân thủ lễ gặp mặt vật", false);
                GameManager.SystemServerEvents.AddEvent(string.Format("Lại lần nữa tay gặp mặt phần thưởng nhận lấy kim tệ, roleID={0}({1}), Money={2}, newMoney={3}", client.RoleID, client.RoleName, client.Money, tongQian), EventLevels.Record);
            }

            //奖励绑定元宝
            int bindYuanBao = newStepItem.BindYuanBao;
            if (bindYuanBao > 0)
            {
                //给用户加钱,更新用户的绑定元宝
                GameManager.ClientMgr.AddUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, bindYuanBao, "Tân thủ lễ gặp mặt vật");
                GameManager.SystemServerEvents.AddEvent(string.Format("Lại lần nữa tay gặp mặt phần thưởng nhận lấy khóa lại Nguyên bảo, roleID={0}({1}), Money={2}, newMoney={3}", client.RoleID, client.RoleName, client.Token, bindYuanBao), EventLevels.Record);
            }
            //设置领取标志
            client.MyHuodongData.NewStep += 1;
            client.MyHuodongData.StepTime = nowTicks;

            //数据库命令更新活动数据事件
            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //发送活动数据给客户端
            GameManager.ClientMgr.NotifyHuodongData(client);

            return 0;
        }

        #endregion 新手见面有礼

        #region 升级有礼

        /// <summary>
        /// 升级有礼缓存词典
        /// 结构:Dictionary<Occupation, Dictionary<unionLevel, UpLevelItem>>
        /// </summary>
        private static Dictionary<int, Dictionary<int, UpLevelItem>> _UpLevelDict = new Dictionary<int, Dictionary<int, UpLevelItem>>();

        private static void InitUpLevelDict()
        {
            Dictionary<int, UpLevelItem> dict;
            lock(_UpLevelDict)
            {
                if (_UpLevelDict.Count == 0)
                {
                    foreach (var kv in GameManager.systemUpLevelGiftMgr.SystemXmlItemDict)
                    {
                        SystemXmlItem systemItem = kv.Value;
                        UpLevelItem newStepItem = new UpLevelItem()
                        {
                            ID = systemItem.GetIntValue("ID"),
                            GoodsDataList = null,
                            BindMoney = systemItem.GetIntValue("BindMoney"),
                            //BindYuanBao = systemItem.GetIntValue("BindYuanBao"),
                            MoJing = systemItem.GetIntValue("MoJing"),
                            FactionID = systemItem.GetIntValue("Occupation"),
                        };

                        if (!_UpLevelDict.TryGetValue(newStepItem.FactionID, out dict))
                        {
                            dict = new Dictionary<int, UpLevelItem>();
                            _UpLevelDict.Add(newStepItem.FactionID, dict);
                        }
                        dict.Add(newStepItem.ToLevel, newStepItem);

                        List<GoodsData> goodsDataList = null;
                        string goodsIDs = systemItem.GetStringValue("GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            //一般都不配置物品奖励，所以执行到这也正常
                            //LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位升级有礼配置项失败, Level={0}", level));
                            continue;
                        }

                        string[] fields = goodsIDs.Split('|');
                        if (fields.Length <= 0)
                        {
                            //一般都不配置物品奖励，所以执行到这也正常
                            //LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位升级有礼配置项失败, Level={0}", level));
                            continue;
                        }

                        //将物品字符串列表解析成物品数据列表
                        goodsDataList = ParseGoodsDataList(fields, "Thăng cấp hữu lễ phối trí hạng");

                        newStepItem.GoodsDataList = goodsDataList;
                    }
                }
            }
        }

        /// <summary>
        /// 获取升级有礼的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static UpLevelItem GetUpLevelItem(int occu, int unionlevel)
        {
            InitUpLevelDict();
            lock (_UpLevelDict)
            {
                UpLevelItem newStepItem = null;
                Dictionary<int, UpLevelItem> dict;
                if (_UpLevelDict.TryGetValue(occu, out dict))
                {
                    if (dict.TryGetValue(unionlevel, out newStepItem))
                    {
                        return newStepItem;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 通过ID获取升级有礼的项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static UpLevelItem GetUpLevelItemByID(int occu, int id)
        {
            InitUpLevelDict();
            lock (_UpLevelDict)
            {
                Dictionary<int, UpLevelItem> dict;
                if (_UpLevelDict.TryGetValue(occu, out dict))
                {
                    foreach (var vk in dict)
                    {
                        if (vk.Value.ID == id)
                        {
                            return vk.Value;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 重置获取升级有礼的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetUpLevelItem()
        {
            int ret = GameManager.systemUpLevelGiftMgr.ReloadLoadFromXMlFile();

            lock (_UpLevelDict)
            {
                _UpLevelDict.Clear();
            }

            return ret;
        }

        public static int GiveUpLevelGift(KPlayer client, UpLevelItem newStepItem)
        {

            return 1;
        }

        /// <summary>
        /// 处理角色获取升级有礼礼物的操作
        /// </summary>
        /// <param name="client"></param>
        /// <param name="give">是否给予奖励</param>
        /// <returns></returns>
        public static int ProcessGetUpLevelGift(KPlayer client, bool give = false)
        {


            return 0;
        }

        #endregion 升级有礼

        #region 大奖活动的项

        /// <summary>
        /// 大奖活动的项的线程锁
        /// </summary>
        private static object _BigAwardItemMutex = new object();

        /// <summary>
        /// 大奖活动的项
        /// </summary>
        private static BigAwardItem _BigAwardItem = null;

        /// <summary>
        /// 获取大奖活动的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static BigAwardItem GetBigAwardItem()
        {
            lock (_BigAwardItemMutex)
            {
                if (_BigAwardItem != null)
                {
                    return _BigAwardItem;
                }
            }

            try
            {
                string fileName = "Config/Gifts/BigGift.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                BigAwardItem bigAwardItem = new BigAwardItem();

                XElement args = xml.Element("GiftTime");
                if (null != args)
                {
                    string fromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    string toDate = Global.GetSafeAttributeStr(args, "ToDate");

                    //时间都是负1表示永久
                    if (0 == fromDate.Trim().CompareTo("-1") && 0 == toDate.Trim().CompareTo("-1"))
                    {
                        fromDate = "2012-06-06 16:16:16";
                        toDate = "2032-06-06 16:16:16";
                    }

                    bigAwardItem.StartTicks = GetHuoDongDateTimeForCommonTimeString(fromDate);
                    bigAwardItem.EndTicks = GetHuoDongDateTimeForCommonTimeString(toDate);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            int id = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            bigAwardItem.NeedJiFenDict[id] = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "NeedJiFen"));

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大奖活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取大奖活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    bigAwardItem.GoodsDataListDict[id] = ParseGoodsDataList(fields, "大奖活动配置");
                                }
                            }
                        }
                    }
                }

                lock (_BigAwardItemMutex)
                {
                    _BigAwardItem = bigAwardItem;
                }

                return bigAwardItem;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/BigGift.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取大奖活动的项,以便下次使用次，强迫重新读配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetBigAwardItem()
        {
            string fileName = "Config/Gifts/BigGift.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_BigAwardItemMutex)
            {
                _BigAwardItem = null;
            }

            return 0;
        }

        /// <summary>
        /// 处理角色获取大奖活动礼品的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetBigAwardGift(KPlayer client, int bigAwardID, int whichOne)
        {
            BigAwardItem bigAwardItem = HuodongCachingMgr.GetBigAwardItem();
            if (null == bigAwardItem)
            {
                return -1;
            }

            if (bigAwardID != GameManager.GameConfigMgr.GetGameConfigItemInt("big_award_id", 0) ||
                GameManager.GameConfigMgr.GetGameConfigItemInt("big_award_id", 0) <= 0)
            {
                return -5;
            }

            long ticks = TimeUtil.NOW();
            if (ticks < bigAwardItem.StartTicks || ticks >= bigAwardItem.EndTicks)
            {
                return -10;
            }

            int subGiftJiFen = 0;
            if (!bigAwardItem.NeedJiFenDict.TryGetValue(whichOne, out subGiftJiFen))
            {
                return -30;
            }

            List<GoodsData> goodsDataList = null;
            if (!bigAwardItem.GoodsDataListDict.TryGetValue(whichOne, out goodsDataList))
            {
                return -50;
            }

            //给予礼物
            //判断背包空格是否能提交接受奖励的物品
            if (!Global.CanAddGoodsDataList(client, goodsDataList))
            {
                return -300;
            }

            int retJiFen = 0;

            //如果需要扣除充值的积分
            if (subGiftJiFen > 0)
            {
                string strcmd = string.Format("{0}:{1}", client.RoleID, subGiftJiFen);
                string[] fields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_SUBCHONGZHIJIFEN, strcmd, client.ServerId);
                if (null == fields || fields.Length < 2)
                {
                    return -200;
                }

                retJiFen = Convert.ToInt32(fields[1]);
                if (retJiFen < 0)
                {
                    return (retJiFen * 1000);
                }
            }

            //获取奖励的物品
            for (int i = 0; i < goodsDataList.Count; i++)
            {
                //想DBServer请求加入某个新的物品到背包中
                //添加物品
                //ADD QUÀ VÒA TÚI ĐỒ
              //  Global.AddGoodsDBCommand(Global._TCPManager.TcpOutPacketPool, client, goodsDataList[i].GoodsID, goodsDataList[i].GCount, goodsDataList[i].Quality, "", goodsDataList[i].Forge_level, goodsDataList[i].Binding, 0, "", true, 1, "充值有礼", Global.ConstGoodsEndTime, goodsDataList[i].AddPropIndex, goodsDataList[i].BornIndex, goodsDataList[i].Lucky, goodsDataList[i].Strong);
            }



            return retJiFen;
        }

        #endregion 大奖活动的项

        #region 送礼活动项

        /// <summary>
        /// 送礼活动的项的线程锁
        /// </summary>
        private static object _SongLiItemMutex = new object();

        /// <summary>
        /// 送礼活动的项
        /// </summary>
        private static SongLiItem _SongLiItem = null;

        /// <summary>
        /// 获取送礼活动的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static SongLiItem GetSongLiItem()
        {
            lock (_SongLiItemMutex)
            {
                if (_SongLiItem != null)
                {
                    return _SongLiItem;
                }
            }

            try
            {
                string sectionKey = string.Empty;
                string fileName = Global.GetGiftExchangeFileName(out sectionKey);
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                SongLiItem songLiItem = new SongLiItem();
                xml = xml.Elements().First(_xml => _xml.Attribute("TypeID").Value.ToString().ToLower() == sectionKey);
                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    string fromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    string toDate = Global.GetSafeAttributeStr(args, "ToDate");

                    //时间都是-1表示永久，这儿使用很多年时间统一处理
                    if (0 == fromDate.Trim().CompareTo("-1") && 0 == toDate.Trim().CompareTo("-1"))
                    {
                        fromDate = "2012-06-06 16:16:16";
                        toDate = "2032-06-06 16:16:16";
                    }

                    songLiItem.StartTicks = GetHuoDongDateTimeForCommonTimeString(fromDate);
                    songLiItem.EndTicks = GetHuoDongDateTimeForCommonTimeString(toDate);
                    songLiItem.IsNeedCode = (int)Global.GetSafeAttributeLong(args, "IsNeedCode");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> arglist = args.Elements();
                    if (null != arglist)
                    {
                        for (int j = 0; j < arglist.Count(); j++)
                        {
                            XElement xmlItem = arglist.ElementAt(j);
                            if (null != xmlItem)
                            {
                                int pingTaiID = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                                string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                                if (string.IsNullOrEmpty(goodsIDs))
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取送礼活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    string[] fields = goodsIDs.Split('|');
                                    if (fields.Length <= 0)
                                    {
                                        LogManager.WriteLog(LogTypes.Warning, string.Format("读取送礼活动配置文件中的物品配置项失败"));
                                    }
                                    else
                                    {
                                        //将物品字符串列表解析成物品数据列表
                                        List<GoodsData> goodsDataList = ParseGoodsDataList(fields, "送礼活动配置");
                                        songLiItem.SongGoodsDataDict[pingTaiID] = goodsDataList;
                                    }
                                }
                            }
                        }
                    }
                }

                lock (_SongLiItemMutex)
                {
                    _SongLiItem = songLiItem;
                }

                return songLiItem;
            }
            catch (Exception ex)
            {
                LogManager.WriteException("处理送礼活动配置时发生异常" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 重置送礼活动的项，强迫下次使用时重新加载
        /// </summary>
        public static int ResetSongLiItem()
        {
            //string fileName = "Config/Gifts/Activities.xml";
            string fileName = Global.GetGiftExchangeFileName();
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_SongLiItemMutex)
            {
                _SongLiItem = null;
            }

            return 0;
        }

        /// <summary>
        /// 处理角色获取送礼活动礼品的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetSongLiGift(KPlayer client, int songLiID, string liPinMa)
        {
            //if (songLiID == client.MyHuodongData.SongLiID)
            //{
            //    return -10000;
            //}

            SongLiItem songLiItem = HuodongCachingMgr.GetSongLiItem();
            if (null == songLiItem)
            {
                return -1;
            }

            if (songLiID != GameManager.GameConfigMgr.GetGameConfigItemInt("songli_id", 0) ||
                GameManager.GameConfigMgr.GetGameConfigItemInt("songli_id", 0) <= 0)
            {
                return -5;
            }

            long ticks = TimeUtil.NOW();
            if (ticks < songLiItem.StartTicks || ticks >= songLiItem.EndTicks)
            {
                return -10;
            }

            if (null == songLiItem.SongGoodsDataDict)
            {
                return -50;
            }

            string strGiftCode = "";

            if (12 == liPinMa.Length)
            {
                if (TimeUtil.NOW() * 10000 - client.GetLiPinMaTicks < 1 * 1000 * 10000)
                {
                    return 0;
                }

                client.GetLiPinMaTicks = TimeUtil.NOW() * 10000;
                strGiftCode = praseKalendsGiftCode(liPinMa);

                if (string.IsNullOrEmpty(strGiftCode))
                {
                    return -1020;
                }

                if ("-1" == strGiftCode)
                {
                    GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                                Global.GetLang("该礼品码已经被使用过了"), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox);
                    return 0;
                }
            }
            else
            {
                strGiftCode = liPinMa;
            }

            string strcmd = "";
            string[] fields = null;
            int goodsDataListID = 1;

            //判断是否需要礼品码
            if (songLiItem.IsNeedCode > 0)
            {
                strcmd = string.Format("{0}:{1}:{2}", client.RoleID, songLiID, strGiftCode);
                fields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_GETLIPINMAINFO, strcmd, client.ServerId);
                if (null == fields || fields.Length < 2)
                {
                    return -200;
                }

                int retCode = Convert.ToInt32(fields[1]);
                if (retCode < 0)
                {
                    return retCode;
                }

                goodsDataListID = retCode;
            }

            List<GoodsData> goodsDataList = null;
            if (!songLiItem.SongGoodsDataDict.TryGetValue(goodsDataListID, out goodsDataList) || null == goodsDataList)
            {
                return -50;
            }

            //给予礼物
            //判断背包空格是否能提交接受奖励的物品
            if (!Global.CanAddGoodsDataList(client, goodsDataList))
            {
                return -400;
            }

            //判断是否需要礼品码
            if (songLiItem.IsNeedCode > 0)
            {
                if (string.IsNullOrEmpty(strGiftCode))
                {
                    return -100;
                }

                strcmd = string.Format("{0}:{1}:{2}", client.RoleID, songLiID, strGiftCode);
                fields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_USELIPINMA, strcmd, client.ServerId);
                if (null == fields || fields.Length < 2)
                {
                    return -200;
                }

                int retCode = Convert.ToInt32(fields[1]);
                if (retCode < 0)
                {
                    return retCode;
                }
            }

            if (12 == liPinMa.Length)
            {
                // 通知中心这个码成功使用了
                praseKalendsGiftCode(liPinMa, 1);
            }

            client.MyHuodongData.SongLiID = songLiID; //防止一个角色重复领取

            //获取奖励的物品
            for (int i = 0; i < goodsDataList.Count; i++)
            {
                //ADD QUÀ VÀO TÚI ĐỒ
                //添加物品
                //Global.AddGoodsDBCommand(Global._TCPManager.TcpOutPacketPool, client,
                //    goodsDataList[i].GoodsID, goodsDataList[i].GCount,
                //    goodsDataList[i].Quality, "", goodsDataList[i].Forge_level,
                //    goodsDataList[i].Binding, 0, "", true, 1, /**/"系统送礼", Global.ConstGoodsEndTime, goodsDataList[i].AddPropIndex, goodsDataList[i].BornIndex, goodsDataList[i].Lucky, goodsDataList[i].Strong);
            }

            return 0;
        }

        private static string praseKalendsGiftCode(string liPinMa, int used = 0)
        {
            try
            {
                string url = GameManager.GameConfigMgr.GetGameConfigItemStr(GameConfigNames.kl_giftcode_u_r_l, "");
                if (string.IsNullOrEmpty(url))
                {
                    return null;
                }

                url = "http://" + url;

                string strMD5Key = GameManager.GameConfigMgr.GetGameConfigItemStr(GameConfigNames.kl_giftcode_md5key, "tmsk_mu_06");
                if (string.IsNullOrEmpty(strMD5Key))
                {
                    return null;
                }

                int timeout = GameManager.GameConfigMgr.GetGameConfigItemInt(GameConfigNames.kl_giftcode_timeout, 200);

                long lTime = DataHelper.UnixSecondsNow();
                string strMD5 = MD5Helper.get_md5_string(liPinMa + lTime + used + strMD5Key);

                Dictionary<string, string> resultDict = new Dictionary<string, string>();
                resultDict["giftid"] = liPinMa;
                resultDict["time"] = lTime.ToString();
                resultDict["used"] = used.ToString(); ;
                resultDict["sign"] = strMD5;

                string strBody = Global.GetJson(resultDict);

                string strResult = Global.doPost(url, strBody, timeout);
                if (string.IsNullOrEmpty(strResult))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode text null "));
                    return null;
                }

                int result = 0;
                if (int.TryParse(strResult, out result))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode return error : {0}", strResult));
                    return null;
                }

                var rspTable = (Hashtable)MUJson.jsonDecode(strResult);
                if (null == rspTable)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode rspTable null : {0}", strResult));
                    return null;
                }

                string strGiftID = rspTable["giftid"].ToString();
                if (string.IsNullOrEmpty(strGiftID))
                {
                    //LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode strGiftID null : {0}", strResult));
                    return null;
                }

                string strTime = rspTable["time"].ToString();
                if (string.IsNullOrEmpty(strTime))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode time null : {0}", strResult));
                    return null;
                }

                long.TryParse(strTime, out lTime);

                string strSign = rspTable["sign"].ToString();
                if (string.IsNullOrEmpty(strSign))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode sign null : {0}", strResult));
                    return null;
                }

                strSign = strSign.ToLower();

                string strMD5Param = strGiftID + lTime + strMD5Key;

                String sign = MD5Helper.get_md5_string(strMD5Param);
                sign = sign.ToLower();

                // MD5校验
                if (sign != strSign)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode MD5 error : {0}", strResult));
                    return null;
                }

                if ("-1" != strGiftID)
                {
                    // 可能返回-2 -3 -4 做错误返回 要记录以下方便查错
                    if (strGiftID.Length < 5)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode GiftCode Length error : {0}", strResult));
                        return null;
                    }

                    /*if (strGiftID.Substring(0, 2) != "NZ")
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("kl_giftcode Prefix error : {0}", strResult));
                        return null;
                    }*/
                }

                return strGiftID;
            }

            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "praseKalendsGiftCode", false);
                return null;
            }
            return null;
        }

        #endregion 送礼活动项

        #region 限时累计登录奖励处理


        private static DateTime _LimitTimeLoginStartTime = new DateTime(1971, 1, 1);


        private static DateTime _LimitTimeLoginEndTime = new DateTime(1971, 1, 1);


        private static Dictionary<int, LimitTimeLoginItem> _LimitTimeLoginDict = new Dictionary<int, LimitTimeLoginItem>();





        /// <summary>
        /// 获取限时累计登录的物品列表
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static LimitTimeLoginItem GetLimitTimeLoginItem(int whichOne)
        {
            LimitTimeLoginItem limitTimeLoginItem = null;
            lock (_LimitTimeLoginDict)
            {
                if (_LimitTimeLoginDict.TryGetValue(whichOne, out limitTimeLoginItem))
                {
                    return limitTimeLoginItem;
                }
            }

            SystemXmlItem systemLimitTimeLoginItem = null;
            if (!GameManager.SystemDengLuDali.SystemXmlItemDict.TryGetValue(whichOne, out systemLimitTimeLoginItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位限时累计登录配置项失败, WhichOne={0}", whichOne));
                return null;
            }

            int timeOl = systemLimitTimeLoginItem.GetIntValue("TimeOl");
            limitTimeLoginItem = new LimitTimeLoginItem()
            {
                TimeOl = timeOl,
                GoodsDataList = null,
            };

            lock (_LimitTimeLoginDict)
            {
                _LimitTimeLoginDict[whichOne] = limitTimeLoginItem;
            }

            List<GoodsData> goodsDataList = null;
            string goodsIDs = systemLimitTimeLoginItem.GetStringValue("GoodsIDs");
            if (string.IsNullOrEmpty(goodsIDs))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位限时累计登录配置项中的物品奖励失败, WhichOne={0}", whichOne));
                return limitTimeLoginItem;
            }

            string[] fields = goodsIDs.Split('|');
            if (fields.Length <= 0)
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位限时累计登录配置项中的物品奖励失败, WhichOne={0}", whichOne));
                return limitTimeLoginItem;
            }

            //将物品字符串列表解析成物品数据列表
            goodsDataList = ParseGoodsDataList(fields, "限时累计登录配置");

            limitTimeLoginItem.GoodsDataList = goodsDataList;
            return limitTimeLoginItem;
        }

        /// <summary>
        /// 重置获取限时累计登录的物品列表, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetLimitTimeLoginItem()
        {
            int ret = GameManager.SystemDengLuDali.ReloadLoadFromXMlFile();

            lock (_LimitTimeLoginDict)
            {
                _LimitTimeLoginStartTime = new DateTime(1971, 1, 1);
                _LimitTimeLoginEndTime = new DateTime(1971, 1, 1);
                _LimitTimeLoginDict.Clear();
            }

            return ret;
        }



        #endregion 限时累计登录奖励处理

        #region 每日在线奖励

        /// <summary>
        /// 每日在线奖励的项字典
        /// </summary>
        private static Dictionary<int, EveryDayOnLineAward> _EveryDayOnLineAwardDict = new Dictionary<int, EveryDayOnLineAward>();

        /// <summary>
        /// 获取每日在线奖励的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int GetEveryDayOnLineItemCount()
        {
            return GameManager.systemEveryDayOnLineAwardMgr.SystemXmlItemDict.Count;//_EveryDayOnLineAwardDict.Count;
        }

        /// <summary>
        /// 获取每日在线奖励的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static EveryDayOnLineAward GetEveryDayOnLineItem(int step)
        {
            EveryDayOnLineAward EveryDayOnLineAwardItem = null;
            lock (_EveryDayOnLineAwardDict)
            {
                if (_EveryDayOnLineAwardDict.TryGetValue(step, out EveryDayOnLineAwardItem))
                {
                    return EveryDayOnLineAwardItem;
                }
            }

            SystemXmlItem systemEveryDayOnLineAwardItem = null;
            if (!GameManager.systemEveryDayOnLineAwardMgr.SystemXmlItemDict.TryGetValue(step, out systemEveryDayOnLineAwardItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位每日在线奖励配置项失败, Step={0}", step));
                return null;
            }

            int timeSecs = Global.GMax(systemEveryDayOnLineAwardItem.GetIntValue("TimeSecs"), 0) * 60;

            EveryDayOnLineAwardItem = new EveryDayOnLineAward()
            {
                TimeSecs = timeSecs,
                FallPacketID = -1,
            };

            lock (_EveryDayOnLineAwardDict)
            {
                _EveryDayOnLineAwardDict[step] = EveryDayOnLineAwardItem;
            }

            int FallIDs = -1;
            FallIDs = systemEveryDayOnLineAwardItem.GetIntValue("FallID");
            if (FallIDs == -1)
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位每日在线奖励配置项失败, Step={0}", step));
                return EveryDayOnLineAwardItem;
            }

            EveryDayOnLineAwardItem.FallPacketID = FallIDs;

            return EveryDayOnLineAwardItem;
        }

        /// <summary>
        /// 重置获取每日在线奖励的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetEveryDayOnLineAwardItem()
        {
            int ret = GameManager.systemEveryDayOnLineAwardMgr.ReloadLoadFromXMlFile();

            lock (_EveryDayOnLineAwardDict)
            {
                _EveryDayOnLineAwardDict.Clear();
            }

            return ret;
        }

        /// <summary>
        /// 处理角色获取每日在线奖励的操作
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ProcessGetEveryDayOnLineAwardGift(KPlayer client, List<GoodsData> goodsDataList, int nType = 0)
        {
            // 如果已经跨天了 则从第一个奖励开始检测
            int nDate = TimeUtil.NowDateTime().DayOfYear;

            if (client.MyHuodongData.GetEveryDayOnLineAwardDayID != nDate)
            {
                client.MyHuodongData.EveryDayOnLineAwardStep     = 0;
                client.MyHuodongData.GetEveryDayOnLineAwardDayID = nDate;
            }

            // 一共能领几次
            int nTotal = HuodongCachingMgr.GetEveryDayOnLineItemCount();

            if (nTotal == client.MyHuodongData.EveryDayOnLineAwardStep)
            {
                // 今天已经全部领完了
                return -1;
            }

            bool bIsSuc = false;
            int nRet = 1;
            int nIndex1 = nTotal - client.MyHuodongData.EveryDayOnLineAwardStep;

            EveryDayOnLineAward EveryDayOnLineAwardItem = null;

            for (int n = client.MyHuodongData.EveryDayOnLineAwardStep + 1; n <= nTotal; ++n)
            {
                EveryDayOnLineAwardItem = HuodongCachingMgr.GetEveryDayOnLineItem(n);
                if (null == EveryDayOnLineAwardItem)
                {
                    // 配置错误
                    return -2;
                }

                //如果还未到领取的时间
                if (client.DayOnlineSecond < EveryDayOnLineAwardItem.TimeSecs)
                {
                    if (!bIsSuc)
                        return -3;
                    else
                        return 1;
                    //break;
                }

                bIsSuc = true;

                //设置领取标志
                client.MyHuodongData.EveryDayOnLineAwardStep += 1;
            }

            client.MyHuodongData.GetEveryDayOnLineAwardDayID = nDate;

            //数据库命令更新活动数据事件
            GameDb.UpdateHuoDongDBCommand(Global._TCPManager.TcpOutPacketPool, client);

            //发送活动数据给客户端
            GameManager.ClientMgr.NotifyHuodongData(client);

            return nRet;
        }



        #endregion 每日在线奖励

        #region 连续登陆奖励

        /// <summary>
        /// 连续登陆map
        /// </summary>
        private static Dictionary<int, SeriesLoginAward> _SeriesLoginAward = new Dictionary<int, SeriesLoginAward>();

        /// <summary>
        /// 获取每日在线奖励的项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int GetSeriesLoginCount()
        {
            return GameManager.systemSeriesLoginAwardMgr.SystemXmlItemDict.Count;
        }

        /// <summary>
        /// 获取连续登录奖励项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        private static SeriesLoginAward GetSeriesLoginAward(int whichOne)
        {
            SeriesLoginAward SeriesLoginItem = null;
            lock (_SeriesLoginAward)
            {
                if (_SeriesLoginAward.TryGetValue(whichOne, out SeriesLoginItem))
                {
                    return SeriesLoginItem;
                }
            }

            SystemXmlItem systemSeriesLoginItem = null;
            if (!GameManager.systemSeriesLoginAwardMgr.SystemXmlItemDict.TryGetValue(whichOne, out systemSeriesLoginItem))
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位连续登录奖励配置项失败, WhichOne={0}", whichOne));
                return null;
            }

            int LoginTime = systemSeriesLoginItem.GetIntValue("LoginTime");
            SeriesLoginItem = new SeriesLoginAward()
            {
                NeedSeriesLoginNum = LoginTime,
                FallPacketID = -1,
            };

            lock (_SeriesLoginAward)
            {
                _SeriesLoginAward[whichOne] = SeriesLoginItem;
            }

            int FallIDs = -1;
            FallIDs = systemSeriesLoginItem.GetIntValue("FallID");
            if (FallIDs == -1)
            {
                LogManager.WriteLog(LogTypes.Warning, string.Format("根据奖励类型定位连续登陆奖励配置项失败, Step={0}", whichOne));
                return SeriesLoginItem;
            }

            SeriesLoginItem.FallPacketID = FallIDs;

            return SeriesLoginItem;
        }

        /// <summary>
        /// 重置获取连续登录的物品列表, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetSeriesLoginItem()
        {
            int ret = GameManager.systemSeriesLoginAwardMgr.ReloadLoadFromXMlFile();

            lock (_SeriesLoginAward)
            {
                _SeriesLoginAward.Clear();
            }

            return ret;
        }





        #endregion 连续登陆奖励

        #endregion 简单奖励处理

        #region 模仿傲视活动奖励处理

        /// <summary>
        /// 首次充值大礼活动
        /// </summary>
        private static FirstChongZhiGift _FirstChongZhiActivity = null;

        /// <summary>
        /// 充值返利活动锁
        /// </summary>
        private static object _FirstChongZhiActivityMutex = new object();

        /// <summary>
        /// 充值返利活动
        /// </summary>
        private static InputFanLiActivity _InputFanLiActivity = null;

        /// <summary>
        /// 充值返利活动锁
        /// </summary>
        private static object _InputFanLiActivityMutex = new object();

        /// <summary>
        /// 充值送礼活动
        /// </summary>
        private static InputSongActivity _InputSongActivity = null;

        /// <summary>
        /// 充值送礼活动锁
        /// </summary>
        private static object _InputSongActivityMutex = new object();

        /// <summary>
        /// 充值王活动
        /// </summary>
        private static KingActivity _InputKingActivity = null;

        /// <summary>
        /// 充值王活动锁
        /// </summary>
        private static object _InputKingActivityMutex = new object();

        /// <summary>
        /// 冲级王活动
        /// </summary>
        private static KingActivity _LevelKingActivity = null;

        /// <summary>
        /// 冲级王活动锁
        /// </summary>
        private static object _LevelKingActivityMutex = new object();

        /// <summary>
        /// 装备王活动
        /// </summary>
        private static KingActivity _EquipKingActivity = null;

        /// <summary>
        /// 装备王活动锁
        /// </summary>
        private static object _EquipKingActivityMutex = new object();

        /// <summary>
        /// 坐骑王活动
        /// </summary>
        private static KingActivity _HorseKingActivity = null;

        /// <summary>
        /// 坐骑王活动锁
        /// </summary>
        private static object _HorseKingActivityMutex = new object();

        /// <summary>
        /// 经脉王活动
        /// </summary>
        private static KingActivity _JingMaiKingActivity = null;

        /// <summary>
        /// 经脉王活动锁
        /// </summary>
        private static object _JingMaiKingActivityMutex = new object();

        /// <summary>
        /// 新服消费达人
        /// </summary>
        private static KingActivity _XinXiaofeiKingActivity = null;
        /// <summary>
        /// 消费达人活动锁
        /// </summary>
        private static object _XinXiaofeiKingMutex = new object();


        #region 首次充值大礼
        /// <summary>
        /// 获取首次充值活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static FirstChongZhiGift GetFirstChongZhiActivity()
        {
            lock (_FirstChongZhiActivityMutex)
            {
                if (_FirstChongZhiActivity != null)
                {
                    return _FirstChongZhiActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/FirstCharge.xml";

                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));

                if (null == xml) return null;

                FirstChongZhiGift activity = new FirstChongZhiGift();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");

                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取首充活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取首充活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "首充活动配置");
                                }
                            }

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取首充活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取首充活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "首充活动配置");
                                }
                            }

                            //设置排行奖励
                            activity.AwardDict = myAwardItem;
                            activity.AwardDict2= myAwardItem2;
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_FirstChongZhiActivityMutex)
                {
                    _FirstChongZhiActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/FirstCharge.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取首次充值活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        /// <summary>
        public static int ResetFirstChongZhiGift()
        {
            string fileName = "Config/Gifts/FirstCharge.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_FirstChongZhiActivityMutex)
            {
                _FirstChongZhiActivity = null;
            }

            return 0;
        }


        #endregion 首次充值大礼

        #region 充值返利
        /// <summary>
        /// 获取充值返利活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static InputFanLiActivity GetInputFanLiActivity()
        {
            lock (_InputFanLiActivityMutex)
            {
                if (_InputFanLiActivity != null)
                {
                    return _InputFanLiActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/FanLi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                InputFanLiActivity activity = new InputFanLiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(7, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(8, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(8, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        activity.FanLiPersent = Global.GetSafeAttributeDouble(xmlItem, "FanLi");
                        if (activity.FanLiPersent < 0)
                        {
                            activity.FanLiPersent = 0;
                        }
                    }
                }

                lock (_InputFanLiActivityMutex)
                {
                    _InputFanLiActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/FanLi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取充值返利活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetInputFanLiActivity()
        {
            string fileName = "Config/Gifts/FanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_InputFanLiActivityMutex)
            {
                _InputFanLiActivity = null;
            }

            return 0;
        }

        #endregion 充值返利

        #region 充值送礼

        /// <summary>
        /// 获取充值送礼活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static InputSongActivity GetInputSongActivity()
        {
            lock (_InputSongActivityMutex)
            {
                if (_InputSongActivity != null)
                {
                    return _InputSongActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/ChongZhiSong.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                InputSongActivity activity = new InputSongActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                }

                activity.MyAwardItem = new AwardItem();

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        activity.MyAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                        activity.MyAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值加送活动配置文件中的物品配置项1失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值加送活动配置文件中的物品配置项失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "充值加送活动配置");
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_InputSongActivityMutex)
                {
                    _InputSongActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/ChongZhiSong.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取充值送礼活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetInputSongActivity()
        {
            string fileName = "Config/Gifts/ChongZhiSong.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_InputSongActivityMutex)
            {
                _InputSongActivity = null;
            }

            return 0;
        }

        #endregion 充值送礼

        #region 充值王

        /// <summary>
        /// 获取充值王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetInputKingActivity()
        {
            lock (_InputKingActivityMutex)
            {
                if (_InputKingActivity != null)
                {
                    return _InputKingActivity;
                }
            }

            try
            {
                // MU 充值改造 [3/21/2014 LiaoWei]
                //string fileName = "Config/Gifts/ChongZhiKing.xml";

                string fileName = "Config/XinFuGifts/MuChongZhi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));

                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(8, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            //myAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "充值王活动配置");
                                }
                            }

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取充值王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "充值王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "ID");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                                activity.AwardDict2.Add(paiHang, myAwardItem2);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_InputKingActivityMutex)
                {
                    _InputKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/XinFuGifts/MuChongZhi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取充值王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetInputKingActivity()
        {
            string fileName = "Config/Gifts/MuChongZhi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_InputKingActivityMutex)
            {
                _InputKingActivity = null;
            }

            return 0;
        }

        #endregion 充值王

        #region 冲级王

        /// <summary>
        /// 获取冲级王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetLevelKingActivity()
        {
            lock (_LevelKingActivityMutex)
            {
                if (_LevelKingActivity != null)
                {
                    return _LevelKingActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/LevelKing.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(10, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinLevel"));
                            myAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取冲级王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取冲级王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "冲级王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "Ranking");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_LevelKingActivityMutex)
                {
                    _LevelKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/LevelKing.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取冲级王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetLevelKingActivity()
        {
            string fileName = "Config/Gifts/LevelKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_LevelKingActivityMutex)
            {
                _LevelKingActivity = null;
            }

            return 0;
        }

        #endregion 冲级王

        #region 装备王====>修改成boss王 必须采用boss王的配置文件

        /// <summary>
        /// 获取装备王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetEquipKingActivity()
        {
            lock (_EquipKingActivityMutex)
            {
                if (_EquipKingActivity != null)
                {
                    return _EquipKingActivity;
                }
            }

            try
            {
                // MU 改造之 [3/20/2014 LiaoWei]

                //string fileName = "Config/Gifts/BossKing.xml"; //"Config/Gifts/EquipKing.xml";

                string fileName = "Config/XinFuGifts/MuBoss.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(8, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinBoss")); //"MinEquip"));
                            //myAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                            string goodsIDs = null;
                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");

                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取Boss王活动配置文件中的物品配置项1失败"));//读取装备王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取Boss王活动配置文件中的物品配置项失败"));//读取装备王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "Boss王活动配置");//装备王活动配置");
                                }
                            }

                            goodsIDs = null;
                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");

                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取Boss王活动配置文件中的物品配置项1失败"));//读取装备王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取Boss王活动配置文件中的物品配置项失败"));//读取装备王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "Boss王活动配置");//装备王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "ID");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                                activity.AwardDict2.Add(paiHang, myAwardItem2);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_EquipKingActivityMutex)
                {
                    _EquipKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/XinFuGifts/MuBoss.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取装备王活动的配置项, 以便下次访问强迫读取配置文件==>已经修改成boss王
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetEquipKingActivity()
        {
            string fileName = "Config/Gifts/MuBoss.xml"; //"Config/Gifts/EquipKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_EquipKingActivityMutex)
            {
                _EquipKingActivity = null;
            }

            return 0;
        }

        #endregion 装备王

        #region 坐骑王====>修改成武学王 必须读武学王的配置文件

        /// <summary>
        /// 获取坐骑王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetHorseKingActivity()
        {
            lock (_HorseKingActivityMutex)
            {
                if (_HorseKingActivity != null)
                {
                    return _HorseKingActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/WuXueKing.xml";//"Config/Gifts/HorseKing.xml";坐骑王====>修改成武学王 必须读武学王的配置文件
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");


                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(10, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinWuXue"));
                            myAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取武学王活动配置文件中的物品配置项1失败"));//string.Format("读取坐骑王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取武学王活动配置文件中的物品配置项失败"));//string.Format("读取坐骑王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "武学王活动配置");//"坐骑王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "Ranking");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_HorseKingActivityMutex)
                {
                    _HorseKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/WuXueKing.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取坐骑王活动的配置项, 以便下次访问强迫读取配置文件,已经修改成武学王
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHorseKingActivity()
        {
            string fileName = "Config/Gifts/WuXueKing.xml";//"Config/Gifts/HorseKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_HorseKingActivityMutex)
            {
                _HorseKingActivity = null;
            }

            return 0;
        }

        #endregion 坐骑王

        #region 经脉王

        /// <summary>
        /// 获取经脉王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetJingMaiKingActivity()
        {
            lock (_JingMaiKingActivityMutex)
            {
                if (_JingMaiKingActivity != null)
                {
                    return _JingMaiKingActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/JingMaiKing.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 7, 10, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(10, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinJingMai"));
                            myAwardItem.AwardYuanBao = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "YuanBao"));

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取经脉王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取经脉王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "经脉王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "Ranking");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JingMaiKingActivityMutex)
                {
                    _JingMaiKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/JingMaiKing.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取经脉王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJingMaiKingActivity()
        {
            string fileName = "Config/Gifts/JingMaiKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_JingMaiKingActivityMutex)
            {
                _JingMaiKingActivity = null;
            }

            return 0;
        }

        #endregion 经脉王

        #region 新服消费达人  gwz

        /// <summary>
        /// 获取新服消费达人活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetXinXiaoFeiKingActivity()
        {
            lock (_XinXiaofeiKingMutex)
            {
                if (_XinXiaofeiKingActivity != null)
                {
                    return _XinXiaofeiKingActivity;
                }
            }

            try
            {
                // MU 改造
                string fileName = "Config/XinFuGifts/MuXiaoFei.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {

                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(7, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(8, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取新服消费达人活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取新服消费达人活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "新服消费达人活动配置");
                                }
                            }

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("新服消费达人活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("新服消费达人活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "新服消费达人活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "ID");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                                activity.AwardDict2.Add(paiHang, myAwardItem2);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_XinXiaofeiKingMutex)
                {
                    _XinXiaofeiKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/XinFuGifts/MuXiaoFei.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重新设置节日活动是否开启的配置项
        /// </summary>
        public static int ResetXinXiaoFeiKingActivity()
        {
            string fileName = "Config/JieRiGifts/MuXiaoFei.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_XinXiaofeiKingMutex)
            {
                _XinXiaofeiKingActivity = null;
            }

            return 0;
        }

        #endregion 新服消费达人


      public static void ReadAwardConfig(XElement args, out Dictionary<int, AwardItem> AwardDict,out Dictionary<int, AwardItem> AwardDict2)
      {
        AwardDict = new Dictionary<int, AwardItem>();
        AwardDict2 = new Dictionary<int, AwardItem>();
        if (null != args)
        {
            IEnumerable<XElement> xmlItems = args.Elements();
            foreach (var xmlItem in xmlItems)
            {
                if (null != xmlItem)
                {
                    AwardItem myAwardItem = new AwardItem();
                    AwardItem myAwardItem2 = new AwardItem();

                    XAttribute hasAttr = xmlItem.Attribute("MinYuanBao");
                    if(hasAttr != null)
                        myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                    myAwardItem.AwardYuanBao = 0;

                    string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                    if (string.IsNullOrEmpty(goodsIDs))
                    {
                        LogManager.WriteLog(LogTypes.Warning, string.Format("读取新服消费达人活动配置文件中的物品配置项1失败"));
                    }
                    else
                    {
                        string[] fields = goodsIDs.Split('|');
                        if (fields.Length <= 0)
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取新服消费达人活动配置文件中的物品配置项失败"));
                        }
                        else
                        {
                            //将物品字符串列表解析成物品数据列表
                            myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "新服消费达人活动配置");
                        }
                    }

                    goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                    if (string.IsNullOrEmpty(goodsIDs))
                    {
                        LogManager.WriteLog(LogTypes.Warning, string.Format("新服消费达人活动配置文件中的物品配置项1失败"));
                    }
                    else
                    {
                        string[] fields = goodsIDs.Split('|');
                        if (fields.Length <= 0)
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("新服消费达人活动配置文件中的物品配置项失败"));
                        }
                        else
                        {
                            //将物品字符串列表解析成物品数据列表
                            myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "新服消费达人活动配置");
                        }
                    }

                    string indexstr = Global.GetSafeAttributeStr(xmlItem, "ID");
                    int index = Global.SafeConvertToInt32(indexstr);


                        AwardDict.Add(index, myAwardItem);
                        AwardDict2.Add(index, myAwardItem2);
                    }
                }
            }
         }

      public static void ReadAwardConfig(XElement args, out Dictionary<int, AwardItem> AwardDict, out Dictionary<int, AwardItem> AwardDict2, out Dictionary<int, AwardEffectTimeItem> AwardDict3)
        {
          AwardDict = new Dictionary<int, AwardItem>();
          AwardDict2 = new Dictionary<int, AwardItem>();
          AwardDict3 = new Dictionary<int, AwardEffectTimeItem>();
          if (null != args)
          {
              IEnumerable<XElement> xmlItems = args.Elements();
              foreach (var xmlItem in xmlItems)
              {
                  if (null != xmlItem)
                  {
                      AwardItem myAwardItem = new AwardItem();
                      AwardItem myAwardItem2 = new AwardItem();
                      AwardEffectTimeItem myAwardItem3 = new AwardEffectTimeItem();

                      XAttribute hasAttr = xmlItem.Attribute("MinYuanBao");
                      if (hasAttr != null)
                          myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                      myAwardItem.AwardYuanBao = 0;

                      string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                      if (string.IsNullOrEmpty(goodsIDs))
                      {
                          LogManager.WriteLog(LogTypes.Warning, string.Format("节日活动返利配置文件中的物品配置项1失败"));
                      }
                      else
                      {
                          string[] fields = goodsIDs.Split('|');
                          if (fields.Length <= 0)
                          {
                              LogManager.WriteLog(LogTypes.Warning, string.Format("节日活动返利配置文件中的物品配置项失败"));
                          }
                          else
                          {
                              //将物品字符串列表解析成物品数据列表
                              myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "节日活动返利配置");
                          }
                      }

                      goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                      if (string.IsNullOrEmpty(goodsIDs))
                      {
                          LogManager.WriteLog(LogTypes.Warning, string.Format("节日活动返利配置文件中的物品配置项1失败"));
                      }
                      else
                      {
                          string[] fields = goodsIDs.Split('|');
                          if (fields.Length <= 0)
                          {
                              LogManager.WriteLog(LogTypes.Warning, string.Format("节日活动返利配置文件中的物品配置项失败"));
                          }
                          else
                          {
                              //将物品字符串列表解析成物品数据列表
                              myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "节日活动返利配置");
                          }
                      }

                      goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsThr");
                      if (false == string.IsNullOrEmpty(goodsIDs))
                      {
                          string[] fields = goodsIDs.Split('|');
                          if (fields.Length > 0)
                          {
                              //将物品字符串列表解析成物品数据列表
                              myAwardItem3.Init(goodsIDs, Global.GetSafeAttributeStr(xmlItem, "EffectiveTime"), "节日返利");
                          }
                      }

                      string indexstr = Global.GetSafeAttributeStr(xmlItem, "ID");
                      int index = Global.SafeConvertToInt32(indexstr);


                      AwardDict.Add(index, myAwardItem);
                      AwardDict2.Add(index, myAwardItem2);
                      AwardDict3.Add(index, myAwardItem3);
                  }
              }
          }
        }

        /// <summary>
        /// 获得累计充值对象
        /// </summary>
        /// <returns></returns>
        public static TotalChargeActivity GetTotalChargeActivity()
        {
            lock (_TotalChargeActivityMutex)
            {
                if (_TotalChargeActivity != null)
                {
                    return _TotalChargeActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/LeiJiChongZhi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                TotalChargeActivity activity = new TotalChargeActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)ActivityTypes.TotalCharge;

                }
                args = xml.Element("GiftList");
                HuodongCachingMgr.ReadAwardConfig(args, out activity.AwardDict, out activity.AwardDict2);

                activity.PredealDateTime();

                lock (_TotalChargeActivityMutex)
                {
                    _TotalChargeActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/LeiJiChongZhi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获得累计充值对象
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        /// <summary>
        public static int ResetTotalChargeActivity()
        {
            string fileName = "Config/Gifts/LeiJiChongZhi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_TotalChargeActivityMutex)
            {
                _TotalChargeActivity = null;
            }

            return 0;
        }


        /// <summary>
        /// 获得累计消费对象
        /// </summary>
        /// <returns></returns>
        public static TotalConsumeActivity GetTotalConsumeActivity()
        {
            lock (_TotalConsumeActivityMutex)
            {
                if (_TotalConsumeActivity != null)
                {
                    return _TotalConsumeActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/LeiJiXiaoFei.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                TotalConsumeActivity activity = new TotalConsumeActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)ActivityTypes.TotalConsume;

                }
                args = xml.Element("GiftList");
                HuodongCachingMgr.ReadAwardConfig(args, out activity.AwardDict, out activity.AwardDict2);

                activity.PredealDateTime();

                lock (_TotalConsumeActivityMutex)
                {
                    _TotalConsumeActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/LeiJiXiaoFei.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获得累计消费对象
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        /// <summary>
        public static int ResetTotalConsumeActivity()
        {
            string fileName = "Config/Gifts/LeiJiXiaoFei.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_TotalConsumeActivityMutex)
            {
                _TotalConsumeActivity = null;
            }

            return 0;
        }

        /// <summary>
        /// [bing] 获得节日返利活动xml数据项
        /// </summary>
        /// <returns></returns>
        public static JieriFanLiActivity GetJieriFanLiActivity(ActivityTypes nActType)
        {
            int nArrayIdx = 0;
            string attrstr = "";
            switch(nActType)
            {
                case ActivityTypes.JieriWing:
                    nArrayIdx = 0;
                    attrstr = "WingLevel";
                    break;
                case ActivityTypes.JieriAddon:
                    nArrayIdx = 1;
                    attrstr = "ZhuiJiaLevel";
                    break;
                case ActivityTypes.JieriStrengthen:
                    nArrayIdx = 2;
                    attrstr = "QiangHuaLevel";
                    break;
                case ActivityTypes.JieriAchievement:
                    nArrayIdx = 3;
                    attrstr = "ChengJiuLevel";
                    break;
                case ActivityTypes.JieriMilitaryRank:
                    nArrayIdx = 4;
                    attrstr = "JunXianLevel";
                    break;
                case ActivityTypes.JieriVIPFanli:
                    nArrayIdx = 5;
                    attrstr = "VIPLevel";
                    break;
                case ActivityTypes.JieriAmulet:
                    nArrayIdx = 6;
                    attrstr = "HuShenFuLevel";
                    break;
                case ActivityTypes.JieriArchangel:
                    nArrayIdx = 7;
                    attrstr = "DaTianShiLevel";
                    break;
                case ActivityTypes.JieriMarriage:
                    nArrayIdx = 8;
                    attrstr = "GoodWillSuit";
                    break;
            }

            lock (_JieriWingFanliActMutex)
            {
                if (null != _JieriWingFanliAct[nArrayIdx])
                {
                    return _JieriWingFanliAct[nArrayIdx];
                }
            }

            string fileName = "";

            try
            {
                XElement xml = null;

                fileName = "Config/JieRiGifts/";

                switch (nActType)
                {
                    case ActivityTypes.JieriWing:
                        fileName += "WingFanLi.xml";
                        break;
                    case ActivityTypes.JieriAddon:
                        fileName += "ZhuiJiaFanLi.xml";
                        break;
                    case ActivityTypes.JieriStrengthen:
                        fileName += "QiangHuaFanLi.xml";
                        break;
                    case ActivityTypes.JieriAchievement:
                        fileName += "ChengJiuFanLi.xml";
                        break;
                    case ActivityTypes.JieriMilitaryRank:
                        fileName += "JunXianFanLi.xml";
                        break;
                    case ActivityTypes.JieriVIPFanli:
                        fileName += "VIPFanLi.xml";
                        break;
                    case ActivityTypes.JieriAmulet:
                        fileName += "HuShenFuFanLi.xml";
                        break;
                    case ActivityTypes.JieriArchangel:
                        fileName += "DaTianShiFanLi.xml";
                        break;
                    case ActivityTypes.JieriMarriage:
                        fileName += "HunYinFanLi.xml";
                        break;
                }

                xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieriFanLiActivity activity = new JieriFanLiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)nActType;

                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");

                }
                args = xml.Element("GiftList");
                HuodongCachingMgr.ReadAwardConfig(args, out activity.AwardDict, out activity.AwardDict2, out activity.AwardDict3);

                activity.PredealDateTime();

                //解析条件
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            string indexstr = Global.GetSafeAttributeStr(xmlItem, "ID");
                            int index = Global.SafeConvertToInt32(indexstr);

                            indexstr = Global.GetSafeAttributeStr(xmlItem, attrstr);

                            string[] attrarray = indexstr.Split(',');

                            activity.AwardDict[index].MinAwardCondionValue = Convert.ToInt32(attrarray[0]);

                            if(attrarray.Length > 1)
                                activity.AwardDict[index].MinAwardCondionValue2 = Convert.ToInt32(attrarray[1]);
                        }
                    }
                }

                lock (_JieriWingFanliActMutex)
                {
                    _JieriWingFanliAct[nArrayIdx] = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, string.Format("{0}解析出现异常", fileName), ex);
            }

            return null;
        }




        #endregion 模仿傲视活动奖励处理

        #region 拉升付费和在线的新加奖励

        #region 升到60级获取元宝/升到100级获取IPhone5S的活动

        /// <summary>
        /// 缓存字典
        /// </summary>
        public static Dictionary<int, UpLevelAwardItem> UpLevelAwardItemDict = null;

        /// <summary>
        /// 初始化缓存字典
        /// </summary>
        private static void InitUpLevelAwardItemDict()
        {
            if (null != UpLevelAwardItemDict)
            {
                return;
            }

            try
            {
                string fileName = "Config/Gifts/UpLevelAward.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return;

                Dictionary<int, UpLevelAwardItem> upLevelAwardItemDict = new Dictionary<int, UpLevelAwardItem>();

                IEnumerable<XElement> args = xml.Elements("Level");
                if (null != args)
                {
                    foreach (var xmlItem in args)
                    {
                        UpLevelAwardItem upLevelAwardItem = new UpLevelAwardItem()
                        {
                            ID = (int)Global.GetSafeAttributeLong(xmlItem, "ID"),
                            MinDay = (int)Global.GetSafeAttributeLong(xmlItem, "MinDay"),
                            MaxDay = (int)Global.GetSafeAttributeLong(xmlItem, "MaxDay"),
                            AwardYuanBao = (int)Global.GetSafeAttributeLong(xmlItem, "AwardYuanBao")
                        };

                        upLevelAwardItemDict[upLevelAwardItem.ID] = upLevelAwardItem;
                    }
                }

                UpLevelAwardItemDict = upLevelAwardItemDict;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/UpLevelAward.xml解析出现异常", ex);
            }
        }

        /// <summary>
        /// 处理角色的升级
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessUpLevelAward4_60Level_100Level(KPlayer client, int oldLevel, int newLevel)
        {
            InitUpLevelAwardItemDict();

            Dictionary<int, UpLevelAwardItem> upLevelAwardItemDict = UpLevelAwardItemDict;
            if (null == upLevelAwardItemDict)
            {
                return;
            }

            int elapsedDays = Global.GetDaysSpanNum(TimeUtil.NowDateTime(), Global.GetRegTime(client), false);
            elapsedDays += 1; //加1才和配置文件中一致

            int bitValue = 0;
            if (oldLevel < 60 && newLevel >= 60)
            {
                for (int i = 0; i < upLevelAwardItemDict.Values.Count; i++)
                {
                    UpLevelAwardItem upLevelAwardItem = upLevelAwardItemDict.Values.ElementAt(i);
                    if (elapsedDays >= upLevelAwardItem.MinDay && elapsedDays <= upLevelAwardItem.MaxDay)
                    {
                        bitValue = (int)Math.Pow(2, i);
                    }
                }
            }
            else if (oldLevel < 100 && newLevel >= 100)
            {
                if (elapsedDays >= 1 && elapsedDays <= 100)
                {
                    bitValue = 16;
                }
            }

            if (bitValue <= 0)
            {
                return;
            }

            int nID = GameManager.ClientMgr.GetTo60or100ID(client);
            if ((nID & bitValue) == bitValue)
            {
                return;
            }

            nID = nID | bitValue;
            GameManager.ClientMgr.ModifyTo60or100ID(client, nID, true, true);
        }

        /// <summary>
        /// 处理角色的升级
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessGetUpLevelAward4_60Level_100Level(KPlayer client, int awardID)
        {
            InitUpLevelAwardItemDict();

            Dictionary<int, UpLevelAwardItem> upLevelAwardItemDict = UpLevelAwardItemDict;
            if (null == upLevelAwardItemDict)
            {
                return;
            }

            UpLevelAwardItem upLevelAwardItem = null;
            if (!upLevelAwardItemDict.TryGetValue(awardID, out upLevelAwardItem))
            {
                return;
            }

            if (null == upLevelAwardItem)
            {
                return;
            }

            //是否禁用达到60级获取奖励的功能
            int disableTo60level = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-to60level", 0);
            if (disableTo60level > 0)
            {
                return;
            }

            int bitValue = 32;
            int nID = GameManager.ClientMgr.GetTo60or100ID(client);
            if ((nID & bitValue) == bitValue)
            {

                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                    Global.GetLang("达到60级的奖励已经领取过了，无法再次领取"), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.None);
                return; //已经领取过了
            }

            nID = nID | bitValue;
            GameManager.ClientMgr.ModifyTo60or100ID(client, nID, true, true);

            bool ret = false;
            if (null != client)
            {
                /*ret = GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                     client, upLevelAwardItem.AwardYuanBao);*/
                ret = GameManager.ClientMgr.AddUserBoundMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, upLevelAwardItem.AwardYuanBao, "达到60级绑定元宝奖励");
            }

            if (!ret)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("处理达到60级绑定元宝奖励时，为角色名称={0}, 添加绑定元宝{1} 失败", client.RoleName, upLevelAwardItem.AwardYuanBao));

                /// 通知在线的对方(不限制地图)个人紧要消息
                GameManager.ClientMgr.NotifyImportantMsg(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client,
                    Global.GetLang("60级的奖励领取时发生错误: -1"), GameInfoTypeIndexes.Error, ShowGameInfoTypes.ErrAndBox, (int)HintErrCodeTypes.None);
            }
            else
            {





            }
        }

        #endregion 升到60级获取元宝/升到100级获取IPhone5S的活动

        #region 升到60级获取元宝/升到100级获取IPhone5S的活动

        /// <summary>
        /// 缓存字典
        /// </summary>
        public static Dictionary<int, KaiFuGiftItem> KaiFuGiftItemDict = null;

        /// <summary>
        /// 初始化缓存字典
        /// </summary>
        private static void InitKaiFuGiftItemDict()
        {
            if (null != KaiFuGiftItemDict)
            {
                return;
            }

            try
            {
                string fileName = "Config/Gifts/KaiFuGift.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return;

                Dictionary<int, KaiFuGiftItem> kaiFuGiftItemDict = new Dictionary<int, KaiFuGiftItem>();

                IEnumerable<XElement> args = xml.Elements("KaiFu");
                if (null != args)
                {
                    foreach (var xmlItem in args)
                    {
                        KaiFuGiftItem kaiFuGiftItem = new KaiFuGiftItem()
                        {
                            Day = (int)Global.GetSafeAttributeLong(xmlItem, "Day"),
                            MinTime = (int)Global.GetSafeAttributeLong(xmlItem, "MinTime"),
                            MinLevel = (int)Global.GetSafeAttributeLong(xmlItem, "MinLevel"),
                            AwardYuanBao = (int)Global.GetSafeAttributeLong(xmlItem, "AwardYuanBao")
                        };

                        kaiFuGiftItemDict[kaiFuGiftItem.Day] = kaiFuGiftItem;
                    }
                }

                KaiFuGiftItemDict = kaiFuGiftItemDict;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/KaiFuGift.xml解析出现异常", ex);
            }
        }

        /// <summary>
        /// 处理角色的在线累计
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessKaiFuGiftAward(KPlayer client)
        {
            int level = client.m_Level;
            if (level < 40) //强制，提高效率
            {
                return;
            }

            int hours = client.TotalOnlineSecs / 3600;
            if (hours < 2) //强制，提高效率 (外部以小时为单位来进行判断)
            {
                return;
            }

            // InitKaiFuGiftItemDict();

            int elapsedDays = Global.GetDaysSpanNum(TimeUtil.NowDateTime(), Global.GetKaiFuTime());
            elapsedDays += 1; //加1才和配置文件中一致

            Dictionary<int, KaiFuGiftItem> kaiFuGiftItemDict = KaiFuGiftItemDict;
            if (null == kaiFuGiftItemDict)
            {
                return;
            }

            KaiFuGiftItem kaiFuGiftItem = null;
            if (!kaiFuGiftItemDict.TryGetValue(elapsedDays, out kaiFuGiftItem))
            {
                return;
            }

            if (level < kaiFuGiftItem.MinLevel)
            {
                return;
            }

            if (hours < kaiFuGiftItem.MinTime)
            {
                return;
            }

            int dayID = GameManager.ClientMgr.GetKaiFuOnlineDayID(client);
            if (elapsedDays == dayID)
            {
                return;
            }

            GameManager.ClientMgr.ModifyKaiFuOnlineDayID(client, elapsedDays, true, true);

            int kaiFuOnlineDayBit = Global.GetRoleParamsInt32FromDB(client, RoleParamName.KaiFuOnlineDayBit);
            kaiFuOnlineDayBit = kaiFuOnlineDayBit | (int)Math.Pow(2, elapsedDays - 1);
            Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.KaiFuOnlineDayBit, kaiFuOnlineDayBit, true);
        }

        /// <summary>
        /// Xử lý sự kiện khi nhân vật đăng nhập ingame
        /// </summary>
        /// <param name="client"></param>
        /// <param name="preLoginDay"></param>
        public static void ProcessDayOnlineSecs(KPlayer client, int preLoginDay)
        {
            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == preLoginDay)
            {
                return;
            }

            int elapsedDays = Global.GetDaysSpanNum(TimeUtil.NowDateTime(), Global.GetKaiFuTime());
            elapsedDays += 1; //加1才和配置文件中一致

            if (elapsedDays <= 1)
            {
                return;
            }

            if (elapsedDays >= 9)
            {
                return;
            }

            int onlineSecs = client.TotalOnlineSecs;
            Global.SaveRoleParamsInt32ValueToDB(client, string.Format("{0}{1}", RoleParamName.KaiFuOnlineDayTimes, elapsedDays - 1), onlineSecs, true);
        }

        /// <summary>
        /// 处理角色的在线累计
        /// </summary>
        /// <param name="client"></param>
        public static bool GetCurrentDayKaiFuOnlineSecs(KPlayer client, out int totalOnlineSecs, out int dayID)
        {
            totalOnlineSecs = 0;
            dayID = 0;

            int elapsedDays = Global.GetDaysSpanNum(TimeUtil.NowDateTime(), Global.GetKaiFuTime());
            elapsedDays += 1; //加1才和配置文件中一致

            if (elapsedDays >= 8)
            {
                return false;
            }

            totalOnlineSecs = client.TotalOnlineSecs;
            dayID = elapsedDays;

            return true;
        }

        /// <summary>
        /// 处理开服在线有礼的动作
        /// </summary>
        public static void ProcessKaiFuGiftAwardActions()
        {
            /// 处理角色的在线累计
            HuodongCachingMgr.ProcessGetKaiFuGiftAward();



        }

        /// <summary>
        /// 上次判断给予在线奖励的时间
        /// </summary>
        private static int LastProcessGetKaiFuGiftAwardDayID = 0;

        /// <summary>
        /// 处理开服在线奖励的小时
        /// </summary>
        public static int ProcessKaiFuGiftAwardHour = 12;

        /// <summary>
        /// 处理角色的在线累计
        /// </summary>
        /// <param name="client"></param>
        public static void ProcessGetKaiFuGiftAward()
        {
            int elapsedDays = Global.GetDaysSpanNum(TimeUtil.NowDateTime(), Global.GetKaiFuTime());
            elapsedDays += 1; //加1才和配置文件中一致

            if (elapsedDays <= 1)
            {
                return;
            }

            if (elapsedDays >= 9)
            {
                return;
            }

            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == LastProcessGetKaiFuGiftAwardDayID)
            {
                return;
            }

            if (ProcessKaiFuGiftAwardHour != TimeUtil.NowDateTime().Hour)
            {
                return;
            }

            LastProcessGetKaiFuGiftAwardDayID = dayID;

            //是否禁用自动抽奖元宝的功能
            int disableKaifuaward = GameManager.GameConfigMgr.GetGameConfigItemInt("disable-kaifuaward", 0);
            if (disableKaifuaward > 0)
            {
                return;
            }

            //处理获奖
            //从DBServer获取副本顺序ID
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_QUERYKAIFUONLINEAWARDROLEID, string.Format("{0}", elapsedDays - 1), GameManager.LocalServerId);
            if (null == dbFields || dbFields.Length < 4)
            {
                return;
            }

            int roleID = Global.SafeConvertToInt32(dbFields[0]);
            if (roleID <= 0)
            {
                return;
            }

            int zoneID = Global.SafeConvertToInt32(dbFields[1]);
            string roleName = dbFields[2];
            int totalRoleNum = Global.SafeConvertToInt32(dbFields[3]);

            // InitKaiFuGiftItemDict();

            Dictionary<int, KaiFuGiftItem> kaiFuGiftItemDict = KaiFuGiftItemDict;
            if (null == kaiFuGiftItemDict)
            {
                return;
            }

            KaiFuGiftItem kaiFuGiftItem = null;
            if (!kaiFuGiftItemDict.TryGetValue(elapsedDays - 1, out kaiFuGiftItem))
            {
                return;
            }

            bool ret = false;
            KPlayer client = GameManager.ClientMgr.FindClient(roleID);
            if (null != client)
            {
                ret = GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, kaiFuGiftItem.AwardYuanBao, "开服在线奖励");
            }
            else
            {
              //  ret = GameManager.ClientMgr.AddTokenOffLine(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, roleID, kaiFuGiftItem.AwardYuanBao, "开服在线奖励", zoneID, Global.QueryTokenFromDB(roleID, roleName));
            }

            if (!ret)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("处理开服在线奖励活动时，为角色名称={0}, 添加元宝{1} 失败", roleName, kaiFuGiftItem.AwardYuanBao));
            }
            else
            {
                //添加历史记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDKAIFUONLINEAWARD, string.Format("{0}:{1}:{2}:{3}:{4}",
                    roleID, Math.Max(1, elapsedDays - 1), kaiFuGiftItem.AwardYuanBao, totalRoleNum, zoneID),
                    null, GameManager.LocalServerId);

                //添加获取元宝记录
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_ADDGIVETokenITEM, string.Format("{0}:{1}:{2}",
                    roleID, kaiFuGiftItem.AwardYuanBao, Global.GetLang("开服在线奖励")),
                    null, GameManager.LocalServerId);


            }
        }



        #endregion 升到60级获取元宝/升到100级获取IPhone5S的活动

        #endregion 拉升付费和在线的新加奖励



        #region 大型节日兑换字卡的数量控制

        /// <summary>
        /// 获取角色今日兑换字卡的剩余
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetZiKaTodayLeftMergeNum(KPlayer client, int index)
        {
            JieRiZiKaLiaBaoActivity instance = HuodongCachingMgr.GetJieRiZiKaLiaBaoActivity();
            if (null == instance)
                return 0;

            JieRiZiKa config = instance.GetAward(index);
            if (null == config)
                return 0;

            int currday = Global.GetOffsetDay(TimeUtil.NowDateTime());
            int lastday = 0;
            int count = 0;

            string strFlag = RoleParamName.JieRiExchargeFlag + index;
            String JieRiExchargeFlag = Global.GetRoleParamByName(client, strFlag);
            // day:count
            if (null != JieRiExchargeFlag)
            {
                string[] fields = JieRiExchargeFlag.Split(',');
                if (2 == fields.Length)
                {
                    lastday = Convert.ToInt32(fields[0]);
                    count = Convert.ToInt32(fields[1]);
                }
            }

            if (currday == lastday)
            {
                return (config.DayMaxTimes - count);
            }

            return config.DayMaxTimes;
        }

        /// <summary>
        /// 修改今日字卡可以兑换的数量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addNum"></param>
        public static int ModifyZiKaTodayLeftMergeNum(KPlayer client, int index, int addNum = 1)
        {
            int currday = Global.GetOffsetDay(TimeUtil.NowDateTime());
            string strFlag = RoleParamName.JieRiExchargeFlag + index;
            String JieRiExchargeFlag = Global.GetRoleParamByName(client, strFlag);

            int lastday = 0;
            int count = 0;
            if (null != JieRiExchargeFlag)
            {
                // day:count
                string[] fields = JieRiExchargeFlag.Split(',');
                if (2 != fields.Length)
                    return 0;

                lastday = Convert.ToInt32(fields[0]);
                count = Convert.ToInt32(fields[1]);
            }

            if (currday == lastday)
            {
                count += addNum;
            }
            else
            {
                lastday = currday;
                count = addNum;
            }

            string result = string.Format("{0},{1}", lastday, count);
            Global.SaveRoleParamsStringToDB(client, strFlag, result, true);
            return count;
        }

        /// <summary>
        /// 兑换字卡
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string MergeZiKa(KPlayer client, int index)
        {
            string strcmd = string.Format("{0}:{1}:{2}", 0, client.RoleID, (int)ActivityTypes.JieriZiKa);

            if (GetZiKaTodayLeftMergeNum(client, index) <= 0)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20000, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            //首先判断需要的原材料够不够
            JieRiZiKaLiaBaoActivity instance = HuodongCachingMgr.GetJieRiZiKaLiaBaoActivity();
            if (null == instance)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20001, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            JieRiZiKa config = instance.GetAward(index);
            if (null == config)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20001, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            if (null == config.MyAwardItem)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20001, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            if (null == config.MyAwardItem.GoodsDataList)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20001, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            if (null != config.NeedGoodsList)
            {
                // 检查道具是否足够
                for (int i = 0; i < config.NeedGoodsList.Count; i++)
                {
                    if (Global.GetTotalGoodsCountByID(client, config.NeedGoodsList[i].GoodsID) < config.NeedGoodsList[i].GCount)
                    {
                        strcmd = string.Format("{0}:{1}:{2}", -20003, client.RoleID, (int)ActivityTypes.JieriZiKa);
                        return strcmd;
                    }
                }
            }


            bool dbRet = instance.GiveAward(client, index);
            //给予新的装备--->不用管成功失败 既然材料扣除了，那这儿的操作就不管成功失败都执行
            //int dbRet = Global.AddGoodsDBCommand(Global._TCPManager.TcpOutPacketPool, client, instance.MyAwardItem.GoodsDataList[0].GoodsID, instance.MyAwardItem.GoodsDataList[0].GCount, instance.MyAwardItem.GoodsDataList[0].Quality, "", instance.MyAwardItem.GoodsDataList[0].Forge_level,
            //    instance.MyAwardItem.GoodsDataList[0].Binding, 0, instance.MyAwardItem.GoodsDataList[0].Jewellist, false, 1, "字卡兑换礼盒", Global.ConstGoodsEndTime, instance.MyAwardItem.GoodsDataList[0].AddPropIndex, instance.MyAwardItem.GoodsDataList[0].BornIndex, instance.MyAwardItem.GoodsDataList[0].Lucky, instance.MyAwardItem.GoodsDataList[0].Strong);

            if (dbRet ==  false)
            {
                strcmd = string.Format("{0}:{1}:{2}", -20005, client.RoleID, (int)ActivityTypes.JieriZiKa);
                return strcmd;
            }

            //修改今日字卡可以兑换的数量
            int leftNum = Math.Max(0, config.DayMaxTimes - ModifyZiKaTodayLeftMergeNum(client, index));

            //strcmd = string.Format("{0}:{1}:{2}:{3}", leftNum, client.RoleID, (int)ActivityTypes.JieriZiKa, index);
            strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 1, client.RoleID, (int)ActivityTypes.JieriZiKa, leftNum, index);
            return strcmd;
        }

        #endregion 大型节日兑换字卡的数量控制

        #region 大型节日活动奖励

        /// <summary>
        /// 节日活动开启配置
        /// </summary>
        private static JieriActivityConfig _JieriActivityConfig = null;

        /// <summary>
        /// 节日活动开启配置锁
        /// </summary>
        private static object _JieriActivityConfigMutex = new object();

        /// <summary>
        /// 节日大礼包活动
        /// </summary>
        private static JieriDaLiBaoActivity _JieriDaLiBaoActivity = null;

        /// <summary>
        /// 节日大礼包活动锁
        /// </summary>
        private static object _JieriDaLiBaoActivityMutex = new object();

        /// <summary>
        /// 节日登录活动
        /// </summary>
        private static JieRiDengLuActivity _JieRiDengLuActivity = null;

        /// <summary>
        /// 节日登录活动锁
        /// </summary>
        private static object _JieriDengLuActivityMutex = new object();

        /// <summary>
        /// 节日VIP活动
        /// </summary>
        private static JieriVIPActivity _JieriVIPActivity = null;

        /// <summary>
        /// 节日VIP活动锁
        /// </summary>
        private static object _JieriVIPActivityMutex = new object();



        /// <summary>
        /// 节日赠送活动的锁
        /// </summary>
        private static object _JieriGiveMutex = new object();



        /// <summary>
        /// 节日收取活动的锁
        /// </summary>
        private static object _JieriRecvMutex = new object();


        /// <summary>
        /// 节日赠送王锁
        /// </summary>
        private static object _JieriGiveKingMutex = new object();



        /// <summary>
        /// 节日收取王锁
        /// </summary>
        private static object _JieriRecvKingMutex = new object();



        /// <summary>
        /// 节日福利锁
        /// </summary>
        private static object _JieriFuLiMutex = new object();

        /// <summary>
        /// 节日充值加送活动
        /// </summary>
        private static JieriCZSongActivity _JieriCZSongActivity = null;

        /// <summary>
        /// 节日充值加送活动锁
        /// </summary>
        private static object _JieriCZSongActivityMutex = new object();

        /// <summary>
        /// 专享活动锁
        /// </summary>
        private static object _SpecialActivityMutex = new object();



        /// <summary>
        /// 充值点兑换活动锁
        /// </summary>
        private static object _JieriIPointsExchgActivityMutex = new object();



       /// <summary>
        /// 周末充值活动锁
        /// </summary>
        private static object _WeedEndInputActivityMutex = new object();

        /// <summary>
        /// 节日累计充值活动
        /// </summary>
        private static JieRiLeiJiCZActivity _JieRiLeiJiCZActivity = null;

        /// <summary>
        /// 节日累计充值活动锁
        /// </summary>
        private static object _JieRiLeiJiCZActivityMutex = new object();

        /// <summary>
        /// 节日累计消费活动
        /// </summary>
        private static JieRiTotalConsumeActivity _JieRiTotalConsumeActivity = null;

        /// <summary>
        /// 节日累计消费活动锁
        /// </summary>
        private static object _JieRiTotalConsumeActivityMutex = new object();

        /// <summary>
        /// 节日多倍奖励活动
        /// </summary>
        private static JieRiMultAwardActivity _JieRiMultAwardActivity = null;

        /// <summary>
        /// 节日多倍奖励活动锁
        /// </summary>
        private static object _JieRiMultAwardActivityMutex = new object();

        /// <summary>
        /// 节日字卡换礼盒活动
        /// </summary>
        private static JieRiZiKaLiaBaoActivity _JieRiZiKaLiaBaoActivity = null;

        /// <summary>
        /// 节日字卡换礼盒活动锁
        /// </summary>
        private static object _JieRiZiKaLiaBaoActivityMutex = new object();

        /// <summary>
        /// 节日消费王活动
        /// </summary>
        private static KingActivity _JieRiXiaoFeiKingActivity = null;

        /// <summary>
        /// 节日消费王活动锁
        /// </summary>
        private static object _JieRiXiaoFeiKingActivityMutex = new object();

        /// <summary>
        /// 节日充值王活动
        /// </summary>
        private static KingActivity _JieRiCZKingActivity = null;

        /// <summary>
        /// 节日充值王活动锁
        /// </summary>
        private static object _JieRiCZKingActivityMutex = new object();

        /// <summary>
        /// 累计充值
        /// </summary>
        private static TotalChargeActivity _TotalChargeActivity = null;
        /// <summary>
        /// 累计充值锁
        /// </summary>
        private static object _TotalChargeActivityMutex = new object();

        /// <summary>
        /// 累计消费
        /// </summary>
        private static TotalConsumeActivity _TotalConsumeActivity = null;
        /// <summary>
        /// 累计消费锁
        /// </summary>
        private static object _TotalConsumeActivityMutex = new object();

        /// <summary>
        /// [bing] 节日翅膀返利
        /// </summary>
        private static JieriFanLiActivity[] _JieriWingFanliAct = new JieriFanLiActivity[9];
        /// <summary>
        /// 累计消费锁
        /// </summary>
        private static object _JieriWingFanliActMutex = new object();

        /// <summary>
        /// 节日连续充值锁
        /// </summary>
        private static object _JieriLianXuChargeMutex = new object();



        /// <summary>
        /// 节日平台充值王锁
        /// </summary>
        private static object _JieriPlatChargeKingMutex = new object();





        #region 节日活动的开启配置

        /// <summary>
        /// 获取节日活动是否开启的配置项
        /// </summary>
        public static JieriActivityConfig GetJieriActivityConfig()
        {
            lock (_JieriActivityConfigMutex)
            {
                if (_JieriActivityConfig != null)
                {
                    return _JieriActivityConfig;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/MuJieRiType.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieriActivityConfig config = new JieriActivityConfig();
                IEnumerable<XElement> xmlItems = xml.Elements();

                foreach (var xmlItem in xmlItems)
                {
                    int activityid = Convert.ToInt32(Global.GetSafeAttributeStr(xmlItem, "Type"));
                    string filename = Global.GetSafeAttributeStr(xmlItem, "PeiZhi");
                    config.ConfigDict[activityid] = filename;
                    config.openList.Add(activityid);

                    //[bing] 把活动名字也写入缓存吧
                    filename = Global.GetSafeAttributeStr(xmlItem, "Name");
                    config.ActivityNameDict[activityid] = filename;
                }

                lock (_JieriActivityConfigMutex)
                {
                    _JieriActivityConfig = config;
                }

                return config;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/MuJieRiType.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重新设置节日活动是否开启的配置项
        /// </summary>
        public static int ResetJieriActivityConfig()
        {
            string fileName = "Config/JieRiGifts/MuJieRiType.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriActivityConfigMutex)
            {
                _JieriActivityConfig = null;
            }

            return 0;
        }

        #endregion

        #region 节日免费大礼包

        /// <summary>
        /// 获取节日大礼包活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static JieriDaLiBaoActivity GetJieriDaLiBaoActivity()
        {
            lock (_JieriDaLiBaoActivityMutex)
            {
                if (_JieriDaLiBaoActivity != null)
                {
                    return _JieriDaLiBaoActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiLiBao.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieriDaLiBaoActivity activity = new JieriDaLiBaoActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                activity.MyAwardItem = new AwardItem();

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        activity.MyAwardItem.MinAwardCondionValue = 0;
                        activity.MyAwardItem.AwardYuanBao = 0;

                        // 通用奖励
                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日礼包活动配置文件中的物品配置项1失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日礼包活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日礼包配置1");
                            }
                        }

                        // 职业奖励
                        goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日礼包活动配置文件中的物品配置项2失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|'); // 每一个奖励物品
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日礼包活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                List<GoodsData> GoodsDataList = ParseGoodsDataList(fields, "大型节日礼包配置2");
                                foreach (var item in GoodsDataList)
                                {
                                    SystemXmlItem systemGoods = null;

                                    // TOD O GÌ ĐÓ Ở ĐÂY
                                    int toOccupation =1;
                                    AwardItem myOccAward = activity.GetOccAward(toOccupation);
                                    if (null == myOccAward)
                                    {
                                        myOccAward = new AwardItem();
                                        myOccAward.GoodsDataList.Add(item);
                                        activity.OccAwardItemDict[toOccupation] = myOccAward;
                                    }
                                    else
                                    {
                                        myOccAward.GoodsDataList.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieriDaLiBaoActivityMutex)
                {
                    _JieriDaLiBaoActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiLiBao.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日大礼包活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieriDaLiBaoActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiLiBao.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriDaLiBaoActivityMutex)
            {
                _JieriDaLiBaoActivity = null;
            }

            return 0;
        }

        #endregion 节日免费大礼包

        #region 节日登录豪礼

        /// <summary>
        /// 获取节日登录豪礼配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        //public static KingActivity GetJieRiDengLuActivity()
        public static JieRiDengLuActivity GetJieRiDengLuActivity()
        {
            lock (_JieriDengLuActivityMutex)
            {
                if (_JieRiDengLuActivity != null)
                {
                    return _JieRiDengLuActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiDengLu.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieRiDengLuActivity activity = new JieRiDengLuActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();

                            myAwardItem.MinAwardCondionValue = 0;
                            myAwardItem.AwardYuanBao = 0;
                            int day = Convert.ToInt32(Global.GetSafeAttributeStr(xmlItem, "TimeOl"));

                            // 读取通用奖励配置
                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日登录有礼活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日登录有礼活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "节日登录有礼配置");
                                }
                            }
                            activity.AwardItemDict[day] = myAwardItem;

                            // 读取职业奖励配置
                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日登录有礼活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日登录有礼活动配置文件中的物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    List<GoodsData> GoodsDataList = ParseGoodsDataList(fields, "节日登录有礼配置2");
                                    foreach (var item in GoodsDataList)
                                    {
                                        SystemXmlItem systemGoods = null;



                                        // TODO J ĐÓ Ở ĐÂY
                                        int key = day * 100 + 1;
                                        AwardItem myOccAward = activity.GetOccAward(key);
                                        if (null == myOccAward)
                                        {
                                            myOccAward = new AwardItem();
                                            myOccAward.GoodsDataList.Add(item);
                                            activity.OccAwardItemDict[key] = myOccAward;
                                        }
                                        else
                                        {
                                            myOccAward.GoodsDataList.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieriDengLuActivityMutex)
                {
                    _JieRiDengLuActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiDengLu.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取充值王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiDengLuActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiDengLu.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriDengLuActivityMutex)
            {
                _JieRiDengLuActivity = null;
            }

            return 0;
        }

        #endregion 节日登录豪礼

        #region VIP大回馈

        /// <summary>
        /// 获取节日VIP活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static JieriVIPActivity GetJieriVIPActivity()
        {
            lock (_JieriVIPActivityMutex)
            {
                if (_JieriVIPActivity != null)
                {
                    return _JieriVIPActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiVip.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieriVIPActivity activity = new JieriVIPActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                activity.MyAwardItem = new AwardItem();

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        activity.MyAwardItem.MinAwardCondionValue = 0;
                        activity.MyAwardItem.AwardYuanBao = 0;

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日VIP活动配置文件中的物品配置项1失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日VIP活动配置文件中的物品配置项失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日VIP配置");
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieriVIPActivityMutex)
                {
                    _JieriVIPActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiVip.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日VIP活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieriVIPActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiVip.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriVIPActivityMutex)
            {
                _JieriVIPActivity = null;
            }

            return 0;
        }

        #endregion VIP大回馈












        #endregion



        #region 节日充值加送

        /// <summary>
        /// 获取节日充值加送活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static JieriCZSongActivity GetJieriCZSongActivity()
        {
            lock (_JieriCZSongActivityMutex)
            {
                if (_JieriCZSongActivity != null)
                {
                    return _JieriCZSongActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiDayChongZhi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieriCZSongActivity activity = new JieriCZSongActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");

                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            int id = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日充值送活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日充值送活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日充值送配置1");
                                }
                            }
                            activity.AwardItemDict[id] = myAwardItem;

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日充值送活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日充值送活动配置文件中的物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    List<GoodsData> GoodsDataList = ParseGoodsDataList(fields, "大型节日充值送配置2");
                                    foreach (var item in GoodsDataList)
                                    {
                                        SystemXmlItem systemGoods = null;


                                        /*int toOccupation = Global.GetMainOccupationByGoodsID(item.GoodsID);*/
                                        int key = id; /**100 + toOccupation;*/ //原key为档位*100+职业 因魔剑士 调整为key为档位，按职业发奖会进行判断发奖 [XSea 2015/6/4]
                                        AwardItem myOccAward = activity.GetOccAward(key);
                                        if (null == myOccAward)
                                        {
                                            myOccAward = new AwardItem();
                                            myOccAward.GoodsDataList.Add(item);
                                            myOccAward.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                                            activity.OccAwardItemDict[key] = myOccAward;
                                        }
                                        else
                                        {
                                            myOccAward.GoodsDataList.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieriCZSongActivityMutex)
                {
                    _JieriCZSongActivity = activity;
                }

                return activity;
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日充值加送活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieriCZSongActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiDayChongZhi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriCZSongActivityMutex)
            {
                _JieriCZSongActivity = null;
            }

            return 0;
        }

        #endregion 节日充值加送

        #region 节日累计充值

        /// <summary>
        /// 获取节日累计充值配置项
        /// </summary>
        /// <returns></returns>
        public static JieRiLeiJiCZActivity GetJieRiLeiJiCZActivity()
        {
            lock (_JieRiLeiJiCZActivityMutex)
            {
                if (_JieRiLeiJiCZActivity != null)
                {
                    return _JieRiLeiJiCZActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiLeiJi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieRiLeiJiCZActivity activity = new JieRiLeiJiCZActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            int id = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日累计充值活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日累计充值活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "节日累计充值配置1");
                                }
                            }
                            activity.AwardItemDict[id] = myAwardItem;

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                //LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日累计充值活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日累计充值活动配置文件中的物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    List<GoodsData> GoodsDataList = ParseGoodsDataList(fields, "节日累计充值配置2");
                                    foreach (var item in GoodsDataList)
                                    {
                                        SystemXmlItem systemGoods = null;


                                        /*int toOccupation = Global.GetMainOccupationByGoodsID(item.GoodsID);*/
                                        int key = id; /**100 + toOccupation;*/ //原key为档位*100+职业 因魔剑士 调整为key为档位，按职业发奖会进行判断发奖 [XSea 2015/6/4]
                                        AwardItem myOccAward = activity.GetOccAward(key);
                                        if (null == myOccAward)
                                        {
                                            myOccAward = new AwardItem();
                                            myOccAward.GoodsDataList.Add(item);
                                            myOccAward.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                                            activity.OccAwardItemDict[key] = myOccAward;
                                        }
                                        else
                                        {
                                            myOccAward.GoodsDataList.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieRiLeiJiCZActivityMutex)
                {
                    _JieRiLeiJiCZActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiLeiJi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日累计充值的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiLeiJiCZActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiLeiJi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiLeiJiCZActivityMutex)
            {
                _JieRiLeiJiCZActivity = null;
            }

            return 0;
        }

        #endregion 节日累计充值


        #region 节日累计消费

        /// <summary>
        /// 获取节日累计消费配置项
        /// </summary>
        /// <returns></returns>
        public static JieRiTotalConsumeActivity GetJieRiTotalConsumeActivity()
        {
            lock (_JieRiTotalConsumeActivityMutex)
            {
                if (_JieRiTotalConsumeActivity != null)
                {
                    return _JieRiTotalConsumeActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiLeiJiXiaoFei.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieRiTotalConsumeActivity activity = new JieRiTotalConsumeActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            int id = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日累计消费活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日累计消费活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "节日累计消费配置1");
                                }
                            }
                            activity.AwardItemDict[id] = myAwardItem;

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                //LogManager.WriteLog(LogTypes.Warning, string.Format("读取节日累计充值活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析节日累计消费活动配置文件中的物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    List<GoodsData> GoodsDataList = ParseGoodsDataList(fields, "节日累计消费配置2");
                                    foreach (var item in GoodsDataList)
                                    {
                                        SystemXmlItem systemGoods = null;


                                        /*int toOccupation = Global.GetMainOccupationByGoodsID(item.GoodsID);*/
                                        int key = id; /**100 + toOccupation;*/ //原key为档位*100+职业 因魔剑士 调整为key为档位，按职业发奖会进行判断发奖 [XSea 2015/6/4]
                                        AwardItem myOccAward = activity.GetOccAward(key);
                                        if (null == myOccAward)
                                        {
                                            myOccAward = new AwardItem();
                                            myOccAward.GoodsDataList.Add(item);
                                            myOccAward.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                                            activity.OccAwardItemDict[key] = myOccAward;
                                        }
                                        else
                                        {
                                            myOccAward.GoodsDataList.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieRiTotalConsumeActivityMutex)
                {
                    _JieRiTotalConsumeActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiLeiJiXiaoFei.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日累计充值的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiTotalConsumeActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiLeiJiXiaoFei.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiTotalConsumeActivityMutex)
            {
                _JieRiTotalConsumeActivity = null;
            }

            return 0;
        }

        #endregion 节日累计消费

        #region 节日多倍奖励

        /// <summary>
        /// 获取节日多倍奖励配置项
        /// </summary>
        /// <returns></returns>
        public static JieRiMultAwardActivity GetJieRiMultAwardActivity()
        {
            lock (_JieRiMultAwardActivityMutex)
            {
                if (_JieRiMultAwardActivity != null)
                {
                    return _JieRiMultAwardActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiDuoBei.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieRiMultAwardActivity activity = new JieRiMultAwardActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");

                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            JieRiMultConfig config = new JieRiMultConfig();
                            config.index = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            config.type = (int)Global.GetSafeAttributeLong(xmlItem, "TypeID");
                            config.Multiplying = Global.GetSafeAttributeDouble(xmlItem, "Multiplying");
                            config.Effective = (int)Global.GetSafeAttributeLong(xmlItem, "Effective");
                            config.StartDate = Global.GetSafeAttributeStr(xmlItem, "AwardStartDate");
                            config.EndDate = Global.GetSafeAttributeStr(xmlItem, "AwardEndDate");
                            activity.activityDict[config.type] = config;
                        }
                    }
                }

                lock (_JieRiMultAwardActivityMutex)
                {
                    _JieRiMultAwardActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiDuoBei.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日累计充值的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiMultAwardActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiDuoBei.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiMultAwardActivityMutex)
            {
                _JieRiMultAwardActivity = null;
            }

            return 0;
        }

        /// <summary>
        /// 重置获取节日返利9个活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiFanLiAwardActivity()
        {
            string fileName = "Config/JieRiGifts/WingFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/ZhuiJiaFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/QiangHuaFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/ChengJiuFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/JunXianFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/VIPFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/HuShenFuFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/DaTianShiFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            fileName = "Config/JieRiGifts/HunYinFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieriWingFanliActMutex)
            {
                for (int i = 0; i < _JieriWingFanliAct.Length; ++i)
                    _JieriWingFanliAct[i] = null;
            }

            return 0;
        }

        #endregion 节日多倍奖励

        #region 节日字卡换礼盒

        /// <summary>
        /// 获取节日字卡换礼盒活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static JieRiZiKaLiaBaoActivity GetJieRiZiKaLiaBaoActivity()
        {
            lock (_JieRiZiKaLiaBaoActivityMutex)
            {
                if (_JieRiZiKaLiaBaoActivity != null)
                {
                    return _JieRiZiKaLiaBaoActivity;
                }
            }

            try
            {
                // public Dictionary<int, JieRiZiKa> JieRiZiKaDict = new Dictionary<int, JieRiZiKa>();
                string fileName = "Config/JieRiGifts/JieRiBaoXiang.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                JieRiZiKaLiaBaoActivity activity = new JieRiZiKaLiaBaoActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");

                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            JieRiZiKa config = new JieRiZiKa();
                            config.id = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            config.type = (int)Global.GetSafeAttributeLong(xmlItem, "Type");
                            config.NeedMoJing = (int)Global.GetSafeAttributeLong(xmlItem, "MoJing");
                            config.NeedQiFuJiFen = (int)Global.GetSafeAttributeLong(xmlItem, "JiFen");
                            config.DayMaxTimes = (int)Global.GetSafeAttributeLong(xmlItem, "DayMaxTimes");
                            //config.NeedPetJiFen = (int)Global.GetSafeAttributeLong(xmlItem, "xxx");
                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "DuiHuanGoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                //LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日字卡换礼盒活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日字卡换礼盒活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    config.NeedGoodsList = ParseGoodsDataList2(fields, "大型节日字卡换礼盒配置1");
                                }
                            }

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "NewGoodsID");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日字卡换礼盒活动配置文件中的合成物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日字卡换礼盒活动配置文件中的合成物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    config.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日字卡换礼盒合成配置2");
                                }
                            }

                            activity.JieRiZiKaDict[config.id] = config;
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieRiZiKaLiaBaoActivityMutex)
                {
                    _JieRiZiKaLiaBaoActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiBaoXiang.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取节日字卡换礼盒活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiZiKaLiaBaoActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiBaoXiang.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiZiKaLiaBaoActivityMutex)
            {
                _JieRiZiKaLiaBaoActivity = null;
            }

            return 0;
        }

        #endregion 节日充值加送

        #region 大型节日消费王

        /// <summary>
        /// 获取大型节日消费王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetJieriXiaoFeiKingActivity()
        {
            lock (_JieRiXiaoFeiKingActivityMutex)
            {
                if (_JieRiXiaoFeiKingActivity != null)
                {
                    return _JieRiXiaoFeiKingActivity;
                }
            }

            try
            {
                // MU 改造
                string fileName = "Config/JieRiGifts/JieRiXiaoFeiKing.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日消费王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日消费王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日消费王活动配置");
                                }
                            }

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                //LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日消费王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日消费王活动配置文件中的物品配置项失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "大型节日消费王活动配置");
                                }
                            }

                            string rankings = Global.GetSafeAttributeStr(xmlItem, "ID");
                            string[] paiHangs = rankings.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                                activity.AwardDict2.Add(paiHang, myAwardItem2);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieRiXiaoFeiKingActivityMutex)
                {
                    _JieRiXiaoFeiKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiXiaoFeiKing.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置大型节日消费王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiXiaoFeiKingActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiXiaoFeiKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiXiaoFeiKingActivityMutex)
            {
                _JieRiXiaoFeiKingActivity = null;
            }

            return 0;
        }

        #endregion 大型节日消费王

        #region 大型节日充值王

        /// <summary>
        /// 获取大型节日充值王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetJieRiCZKingActivity()
        {
            lock (_JieRiCZKingActivityMutex)
            {
                if (_JieRiCZKingActivity != null)
                {
                    return _JieRiCZKingActivity;
                }
            }

            try
            {
                string fileName = "Config/JieRiGifts/JieRiChongZhiKing.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                KingActivity activity = new KingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            int rank = Convert.ToInt32(Global.GetSafeAttributeStr(xmlItem, "ID"));
                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.AwardYuanBao = 0;

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日充值王活动配置文件中的物品配置项1失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日充值王活动配置文件中的物品配置项1失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日充值王活动配置1");
                                }
                            }
                            activity.AwardDict.Add(rank, myAwardItem);

                            AwardItem myOccAwardItem = new AwardItem();
                            myOccAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myOccAwardItem.AwardYuanBao = 0;

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型节日充值王活动配置文件中的物品配置项2失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析大型节日充值王活动配置文件中的物品配置项2失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    myOccAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型节日充值王活动配置2");
                                }
                            }

                            activity.AwardDict2.Add(rank, myOccAwardItem);
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_JieRiCZKingActivityMutex)
                {
                    _JieRiCZKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/JieRiGifts/JieRiChongZhiKing.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置大型节日充值王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetJieRiCZKingActivity()
        {
            string fileName = "Config/JieRiGifts/JieRiChongZhiKing.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_JieRiCZKingActivityMutex)
            {
                _JieRiCZKingActivity = null;
            }

            return 0;
        }

        #endregion 大型节日充值王

        #endregion 大型节日活动奖励

        #region 合服活动和新加的开区返利活动

        /// <summary>
        /// 合服活动配置
        /// </summary>
        private static HeFuActivityConfig _HeFuActivityConfig = null;

        /// <summary>
        /// 合服活动配置锁
        /// </summary>
        private static object _HeFuActivityConfigMutex = new object();

        /// <summary>
        /// 合服大礼包活动
        /// </summary>
        private static HeFuLoginActivity _HeFuLoginActivity = null;

        /// <summary>
        /// 合服大礼包活动锁
        /// </summary>
        private static object _HeFuLoginActivityMutex = new object();

        /// <summary>
        /// 合服充值返利
        /// </summary>
        private static HeFuRechargeActivity _HeFuRechargeActivity = null;

        /// <summary>
        /// 合服充值返利活动锁
        /// </summary>
        private static object _HeFuRechargeActivityMutex = new object();

        /// <summary>
        /// 合服登陆活动
        /// </summary>
        private static HeFuTotalLoginActivity _HeFuTotalLoginActivity = null;

        /// <summary>
        /// 合服登陆活动锁
        /// </summary>
        private static object _HeFuTotalLoginActivityMutex = new object();

        /// <summary>
        /// 合服PK王活动
        /// </summary>
        private static HeFuPKKingActivity _HeFuPKKingActivity = null;

        /// <summary>
        /// 合服奖励翻倍
        /// </summary>
        private static object _HeFuAwardTimeActivityMutex = new object();

        /// <summary>
        /// 合服奖励翻倍
        /// </summary>
        private static HeFuAwardTimesActivity _HeFuAwardTimeActivity = null;

        /// <summary>
        /// 合服罗兰城主活动
        /// </summary>
        private static HeFuLuoLanActivity _HeFuLuoLanActivity = null;

        /// <summary>
        /// 合服罗兰城主活动锁
        /// </summary>
        private static object _HeFuLuoLanActivityMutex = new object();

        /// <summary>
        /// 合服PK王活动锁
        /// </summary>
        private static object _HeFuPKKingActivityMutex = new object();

        /// <summary>
        /// 合服王城霸主活动
        /// </summary>
        private static HeFuWCKingActivity _HeFuWCKingActivity = null;

        /// <summary>
        /// 合服王城霸主活动锁
        /// </summary>
        private static object _HeFuWCKingActivityMutex = new object();

        /// <summary>
        /// 新开区充值返利活动
        /// </summary>
        private static XinFanLiActivity _XinFanLiActivity = null;

        /// <summary>
        /// 新开区充值返利活动锁
        /// </summary>
        private static object _XinFanLiActivityMutex = new object();

        /// <summary>
        /// 每日充值豪礼活动锁
        /// </summary>
        private static object _MeiRiChongZhiHaoLiActivityMutex = new object();

        /// <summary>
        /// 每日充值豪礼活动
        /// </summary>
        private static MeiRiChongZhiActivity _MeiRiChongZhiHaoLiActivity = null;

        /// <summary>
        /// 冲级豪礼活动锁
        /// </summary>
        private static object _ChongJiHaoLiActivityMutex = new object();

        /// <summary>
        /// 冲级豪礼活动
        /// </summary>
        private static ChongJiHaoLiActivity _ChongJiHaoLiActivity = null;

        /// <summary>
        /// 神装激情回馈豪礼活动锁
        /// </summary>
        private static object _ShenZhuangJiQingHuiKuiHaoLiActivityMutex = new object();

        /// <summary>
        /// 神装激情回馈豪礼活动
        /// </summary>
        private static ShenZhuangHuiKuiHaoLiActivity _ShenZhuangJiQingHuiKuiHaoLiActivity = null;


        /// <summary>
        /// 月度大转盘抽奖活动锁
        /// </summary>
        private static object _YueDuZhuanPanActivityMutex = new object();

        /// <summary>
        /// 月度大转盘抽奖活动
        /// </summary>
        private static YueDuZhuanPanActivity _YueDuZhuanPanActivity = null;


        #region 统一初始化合服活动奖励

        /// <summary>
        /// 加载合服活动配置项,失败抛出异常
        /// </summary>
        /// <returns></returns>
        public static bool LoadHeFuActivitiesConfig()
        {
            string strError = "";

            while (true)
            {
                Activity instance = HuodongCachingMgr.GetHeFuLoginActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "合服大礼包活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetHeFuTotalLoginActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "合服累计登陆活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetHeFuPKKingActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "合服PK王活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetHeFuWCKingActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "合服王城霸主活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetHeFuRechargeActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "合服充值返利活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetXinFanLiActivity();
                if (null == instance || instance.GetParamsValidateCode() < 0)
                {
                    strError = "新的新区返利活动配置项出错";
                    break;
                }

                instance = HuodongCachingMgr.GetHeFuLuoLanActivity();
                if (null == instance)
                {
                    strError = "合服罗兰城主活动配置项出错";
                    break;
                }

                break;
            }

            if (!string.IsNullOrEmpty(strError))
            {
                LogManager.WriteLog(LogTypes.Fatal, strError);
                return false;
            }

            return true;
        }
        #endregion 统一初始化合服活动奖励

        /// <summary>
        /// 获取合服活动的配置项
        /// </summary>
        /// <returns></returns>
        ///
        public static HeFuActivityConfig GetHeFuActivityConfing()
        {
            lock (_HeFuActivityConfigMutex)
            {
                if (_HeFuActivityConfig != null)
                {
                    return _HeFuActivityConfig;
                }
            }
            try
            {
                string fileName = "Config/HeFuGifts/HeFuType.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuActivityConfig config = new HeFuActivityConfig();
                IEnumerable<XElement> xmlItems = xml.Elements();

                foreach (var xmlItem in xmlItems)
                {
                    int activityid = Convert.ToInt32(Global.GetSafeAttributeStr(xmlItem, "ID"));
                    config.openList.Add(activityid);
                }

                lock (_HeFuActivityConfigMutex)
                {
                    _HeFuActivityConfig = config;
                }

                return config;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuType.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置合服活动配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuActivityConfig()
        {
            string fileName = "Config/HeFuGifts/HeFuType.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuActivityConfigMutex)
            {
                _HeFuActivityConfig = null;
            }

            return 0;
        }


        #region 合服免费大礼包

        /// <summary>
        /// 获取合服大礼包活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuLoginActivity GetHeFuLoginActivity()
        {
            lock (_HeFuLoginActivityMutex)
            {
                if (_HeFuLoginActivity != null)
                {
                    return _HeFuLoginActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/HeFuLiBao.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuLoginActivity activity = new HeFuLoginActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("Time");

                // 活动默认7天，领奖默认7天
                int ActivityTime = 7;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString() ) );
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString() ) );
                }
                else
                {

                }
                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime - 1, 23, 59, 59);

                args = xml.Element("GiftList");
                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        AwardItem NormalAward = new AwardItem();

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取Config/HeFuGifts/HeFuLiBao.xml的普通奖励失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析Config/HeFuGifts/HeFuLiBao.xml的普通奖励失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                NormalAward.GoodsDataList = ParseGoodsDataList(fields, "大型合服礼包配置");
                            }
                        }

                        activity.AwardDict[(int)HeFuLoginAwardType.NormalAward] = NormalAward;

                        AwardItem VIPAward = new AwardItem();
                        string VIPGoodsIDs = Global.GetSafeAttributeStr(xmlItem, "VIPGoodsIDs");
                        if (string.IsNullOrEmpty(VIPGoodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取Config/HeFuGifts/HeFuLiBao.xml的VIP奖励失败"));
                        }
                        else
                        {
                            string[] fields = VIPGoodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析Config/HeFuGifts/HeFuLiBao.xml的VIP奖励失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                VIPAward.GoodsDataList = ParseGoodsDataList(fields, "大型合服礼包配置");
                            }
                        }
                        activity.AwardDict[(int)HeFuLoginAwardType.VIPAward] = VIPAward;
                    }

                }

                lock (_HeFuLoginActivityMutex)
                {
                    _HeFuLoginActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuLiBao.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服大礼包活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuLoginActivity()
        {
            string fileName = "Config/HeFuGifts/HeFuLiBao.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuLoginActivityMutex)
            {
                _HeFuLoginActivity = null;
            }

            return 0;
        }

        #endregion 合服免费大礼包

        /// <summary>
        /// 获取合服充值加送活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuTotalLoginActivity GetHeFuTotalLoginActivity()
        {
            lock (_HeFuTotalLoginActivityMutex)
            {
                if (_HeFuTotalLoginActivity != null)
                {
                    return _HeFuTotalLoginActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/HeFuDengLu.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuTotalLoginActivity activity = new HeFuTotalLoginActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("Time");

                // 活动默认7天，领奖默认7天
                int ActivityTime = 7;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString() ) );
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString() ) );
                }

                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime - 1, 23, 59, 59);

                args = xml.Element("GiftList");

                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        int day = Convert.ToInt32(Global.GetSafeAttributeStr(xmlItem, "TimeOl"));
                        AwardItem myAwardItem = new AwardItem();

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取合服累计登陆配置文件中的GoodsIDs失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析合服累计登陆配置文件中的GoodsIDs失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "合服累计登陆配置");
                            }
                        }
                        activity.AwardDict[day] = myAwardItem;
                    }
                }

                lock (_HeFuTotalLoginActivityMutex)
                {
                    _HeFuTotalLoginActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuDengLu.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服累计登陆的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuTotalLoginActivity()
        {
            string fileName = "Config/HeFuGifts/HeFuDengLu.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuTotalLoginActivityMutex)
            {
                _HeFuTotalLoginActivity = null;
            }

            return 0;
        }


        #region 合服PK王

        /// <summary>
        /// 获取合服的PK王
        /// </summary>
        /// <returns></returns>
        public static int GetHeFuPKKingRoleID()
        {
            HeFuPKKingActivity activity = GetHeFuPKKingActivity();

            int hefuPKKingRoleID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefupkking", 0);
            int hefuPKKingNum = GameManager.GameConfigMgr.GetGameConfigItemInt("hefupkkingnum", 0);

            if (null != activity && !activity.InActivityTime() && !activity.InAwardTime())
                return 0;

            if (hefuPKKingRoleID > 0 && hefuPKKingNum >= activity.winerCount)
                return hefuPKKingRoleID;

            return 0;

        }

        /// <summary>
        /// 设置合服后的PK王
        /// </summary>
        /// <returns></returns>
        public static void UpdateHeFuPKKingRoleID(int roleID)
        {
            HeFuPKKingActivity activity = GetHeFuPKKingActivity();

            if (null != activity && !activity.InActivityTime())
                return ;

            int hefuPKKingRoleID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefupkking", 0);
            int hefuPKKingDayID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefupkkingdayid", 0);
            int hefuPKKingNum = GameManager.GameConfigMgr.GetGameConfigItemInt("hefupkkingnum", 0);

            // 如果已经有合服PK之王了，就不再记录了
            if (0 < GetHeFuPKKingRoleID())
                return ;

            //判断roleid是否等于是之前的PK之王 || // 不是连续获得
            int CurrDay = Global.GetOffsetDay(TimeUtil.NowDateTime());
            if (roleID != hefuPKKingRoleID || CurrDay != hefuPKKingDayID + 1)
            {
                // 重新设置pk之王
                hefuPKKingRoleID = roleID;
                hefuPKKingDayID = CurrDay;
                hefuPKKingNum = 1;
            }
            else
            {
                // 更新获得时间和计数
                hefuPKKingRoleID = roleID;
                hefuPKKingDayID = CurrDay;
                hefuPKKingNum += 1;
            }

            // 存储PK之王信息
            Global.UpdateDBGameConfigg("hefupkking", hefuPKKingRoleID.ToString());
            Global.UpdateDBGameConfigg("hefupkkingdayid", hefuPKKingDayID.ToString());
            Global.UpdateDBGameConfigg("hefupkkingnum", hefuPKKingNum.ToString());
        }

        /// <summary>
        /// 获取合服PK王活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuPKKingActivity GetHeFuPKKingActivity()
        {
            lock (_HeFuPKKingActivityMutex)
            {
                if (_HeFuPKKingActivity != null)
                {
                    return _HeFuPKKingActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/PKJiangLi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuPKKingActivity activity = new HeFuPKKingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                    activity.winerCount = Convert.ToInt32(Global.GetDefAttributeStr(args, "WinerCount", "3"));
                }

                args = xml.Element("Time");

                // 活动默认5天，领奖默认7天
                int ActivityTime = 5;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString() ) );
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString() ) );
                }

                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(activity.winerCount, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime - 1, 23, 59, 59);

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDOne");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取合服战场之神配置GoodsIDOne失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析合服战场之神配置GoodsIDOne失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "合服战场之神配置");
                            }
                        }
                    }
                }

                lock (_HeFuPKKingActivityMutex)
                {
                    _HeFuPKKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/PKJiangLi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服PK王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuPKKingActivity()
        {
            string fileName = "Config/HeFuGifts/PKJiangLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuPKKingActivityMutex)
            {
                _HeFuPKKingActivity = null;
            }

            return 0;
        }

        /// <summary>
        /// 获取合服罗兰城主活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuLuoLanActivity GetHeFuLuoLanActivity()
        {
            lock (_HeFuLuoLanActivityMutex)
            {
                if (_HeFuLuoLanActivity != null)
                {
                    return _HeFuLuoLanActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/HeFuLuoLan.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuLuoLanActivity activity = new HeFuLuoLanActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("Time");

                // 活动默认5天，领奖默认7天
                int ActivityTime = 7;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString()));
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString()));
                }

                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime - 1, 23, 59, 59);

                args = xml.Element("GiftList");

                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            HeFuLuoLanAward hefuAward = new HeFuLuoLanAward();

                            int ID = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "ID"));

                            hefuAward.winNum = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "WinNum"));
                            hefuAward.status = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "Status"));
                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取合服罗兰城主配置GoodsOne失败"));
                            }
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                {
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("解析合服罗兰城主配置GoodsOne失败"));
                                }
                                else
                                {
                                    //将物品字符串列表解析成物品数据列表
                                    hefuAward.awardData.GoodsDataList = ParseGoodsDataList(fields, "合服罗兰城主配置");
                                }
                            }

                            activity.HeFuLuoLanAwardDict[ID] = hefuAward;
                        }
                    }
                }

                lock (_HeFuLuoLanActivityMutex)
                {
                    _HeFuLuoLanActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuLuoLan.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服PK王活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuLuoLanActivity()
        {
            string fileName = "Config/HeFuGifts/HeFuLuoLan.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuLuoLanActivityMutex)
            {
                _HeFuLuoLanActivity = null;
            }

            return 0;
        }



        /// <summary>
        /// 获取合服活动奖励翻倍活动配置
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuAwardTimesActivity GetHeFuAwardTimesActivity()
        {
            lock (_HeFuAwardTimeActivityMutex)
            {
                if (_HeFuAwardTimeActivity != null)
                {
                    return _HeFuAwardTimeActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/HeFuZhangChang.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuAwardTimesActivity activity = new HeFuAwardTimesActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("Time");

                // 活动默认7天，领奖默认7天
                int ActivityTime = 7;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString() ) );
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString() ) );
                }

                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime - 1, 23, 59, 59);

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        string ActivitiesIDs = Global.GetSafeAttributeStr(xmlItem, "ActivitiesIDs");
                        if (string.IsNullOrEmpty(ActivitiesIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取合服为战而生配置ActivitiesIDs失败"));
                        }
                        else
                        {
                            string[] fields = ActivitiesIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("解析合服战场之神配置GoodsIDOne失败"));
                            }
                            else
                            {
                                for (int i = 0; i < fields.Length; i++)
                                    activity.activityList.Add(Convert.ToInt32(fields[i]));
                            }
                        }
                        activity.activityTimes = (float)Convert.ToDouble(Global.GetDefAttributeStr(xmlItem, "Override", "2"));
                        activity.specialTimeID = Convert.ToInt32(Global.GetDefAttributeStr(xmlItem, "SpecialTimeID", "0"));
                    }
                }

                lock (_HeFuAwardTimeActivityMutex)
                {
                    _HeFuAwardTimeActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuZhangChang.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服奖励翻倍活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuAwardTimeActivity()
        {
            string fileName = "Config/HeFuGifts/HeFuZhangChang.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuAwardTimeActivityMutex)
            {
                _HeFuAwardTimeActivity = null;
            }

            return 0;
        }

        #endregion 合服PK王

        #region 合服王城霸主

        /// <summary>
        /// 获取合服的王城霸主
        /// </summary>
        /// <returns></returns>
        public static int GetHeFuWCKingBHID()
        {
            HeFuWCKingActivity activity = GetHeFuWCKingActivity();

            DateTime startAward = DateTime.Parse(activity.AwardStartDate);
            DateTime endAward = DateTime.Parse(activity.AwardEndDate);
            if (TimeUtil.NowDateTime() >= startAward && TimeUtil.NowDateTime() <= endAward)
            {
                int hefuWCKingBHID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefuwcking", 0);
                int hefuWCKingNum = GameManager.GameConfigMgr.GetGameConfigItemInt("hefuwckingnum", 0);
                if (hefuWCKingNum >= 3)
                {
                    return hefuWCKingBHID;
                }

                return 0;
            }

            return 0;
        }

        /// <summary>
        /// 设置合服后的王城霸主
        /// </summary>
        /// <returns></returns>
        public static void UpdateHeFuWCKingBHID(int bhid)
        {
            HeFuWCKingActivity activity = GetHeFuWCKingActivity();

            DateTime startAward = DateTime.Parse(activity.FromDate);
            DateTime endAward = DateTime.Parse(activity.ToDate);
            if (TimeUtil.NowDateTime() >= startAward && TimeUtil.NowDateTime() <= endAward)
            {
                int hefuWCKingBHID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefuwcking", 0);
                int hefuWCKingDayID = GameManager.GameConfigMgr.GetGameConfigItemInt("hefuwckingdayid", 0);
                int hefuWCKingNum = GameManager.GameConfigMgr.GetGameConfigItemInt("hefuwckingnum", 0);
                if (hefuWCKingNum < 3)
                {
                    int dayID = TimeUtil.NowDateTime().DayOfYear;
                    if (dayID != hefuWCKingDayID) //同一天，只能设置一次
                    {
                        if (hefuWCKingBHID != bhid)
                        {
                            hefuWCKingBHID = bhid;
                            hefuWCKingDayID = dayID;
                            hefuWCKingNum = 1;
                        }
                        else
                        {
                            if (hefuWCKingDayID == (dayID - 1) ||
                                (dayID == 1 && hefuWCKingDayID >= 365))
                            {
                                hefuWCKingNum = hefuWCKingNum + 1;
                            }
                            else
                            {
                                hefuWCKingNum = 1;
                            }

                            hefuWCKingBHID = bhid;
                            hefuWCKingDayID = dayID;
                        }

                        GameManager.GameConfigMgr.UpdateGameConfigItem("hefuwcking", hefuWCKingBHID.ToString());
                        GameManager.GameConfigMgr.UpdateGameConfigItem("hefuwckingdayid", hefuWCKingDayID.ToString());
                        GameManager.GameConfigMgr.UpdateGameConfigItem("hefuwckingnum", hefuWCKingNum.ToString());
                    }
                }
            }
            else //如果不在合服活动时间内
            {
                GameManager.GameConfigMgr.UpdateGameConfigItem("hefuwckingnum", "0");
            }
        }

        /// <summary>
        /// 获取合服王城霸主活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuWCKingActivity GetHeFuWCKingActivity()
        {
            lock (_HeFuWCKingActivityMutex)
            {
                if (_HeFuWCKingActivity != null)
                {
                    return _HeFuWCKingActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/WangChengJiangLi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuWCKingActivity activity = new HeFuWCKingActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByHeFu(4, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(5, 23, 59, 59);
                }

                activity.MyAwardItem = new AwardItem();

                args = xml.Element("GiftList");

                if (null != args)
                {
                    XElement xmlItem = args.Element("Award");
                    if (null != xmlItem)
                    {
                        activity.MyAwardItem.MinAwardCondionValue = 0;
                        activity.MyAwardItem.AwardYuanBao = 0;

                        string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                        if (string.IsNullOrEmpty(goodsIDs))
                        {
                            LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型合服王城争霸活动配置文件中的物品配置项1失败"));
                        }
                        else
                        {
                            string[] fields = goodsIDs.Split('|');
                            if (fields.Length <= 0)
                            {
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取大型合服王城争霸活动配置文件中的物品配置项失败"));
                            }
                            else
                            {
                                //将物品字符串列表解析成物品数据列表
                                activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "大型合服王城争霸配置");
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_HeFuWCKingActivityMutex)
                {
                    _HeFuWCKingActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/WangChengJiangLi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取合服王城霸主活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuWCKingActivity()
        {
            string fileName = "Config/HeFuGifts/WangChengJiangLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuWCKingActivityMutex)
            {
                _HeFuWCKingActivity = null;
            }

            return 0;
        }

        #endregion 合服王城霸主

        #region 合服充值返利

        /// <summary>
        /// 获取合服充值返利活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static HeFuRechargeActivity GetHeFuRechargeActivity()
        {
            lock (_HeFuRechargeActivityMutex)
            {
                if (_HeFuRechargeActivity != null)
                {
                    return _HeFuRechargeActivity;
                }
            }

            try
            {
                string fileName = "Config/HeFuGifts/HeFuFanLi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                HeFuRechargeActivity activity = new HeFuRechargeActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");
                }

                args = xml.Element("Time");

                // 活动默认7天，领奖默认7天
                int ActivityTime = 7;
                int AwardTime = 7;
                if (null != args)
                {
                    ActivityTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Activity", ActivityTime.ToString() ) );
                    AwardTime = Convert.ToInt32(Global.GetDefAttributeStr(args, "Award", AwardTime.ToString() ) );
                }

                activity.FromDate = Global.GetHuoDongTimeByHeFu(0, 0, 0, 0);
                activity.ToDate = Global.GetHuoDongTimeByHeFu(ActivityTime - 1, 23, 59, 59);
                activity.AwardStartDate = Global.GetHuoDongTimeByHeFu(1, 0, 0, 0);
                activity.AwardEndDate = Global.GetHuoDongTimeByHeFu(AwardTime, 23, 59, 59);

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            int rank = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "Level"));
                            HeFuRechargeData data = new HeFuRechargeData();
                            string strFanli = Global.GetDefAttributeStr(xmlItem, "FanLi", "0.0");
                            data.Coe = (float)Convert.ToDouble(strFanli);
                            data.LowLimit = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            activity.ConfigDict[rank] = data;

                            activity.strcoe += rank;
                            activity.strcoe += ",";
                            activity.strcoe += data.Coe;
                            activity.strcoe += "|";
                        }
                    }
                }

                lock (_HeFuRechargeActivityMutex)
                {
                    _HeFuRechargeActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/HeFuGifts/HeFuFanLi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置合服充值返利活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetHeFuRechargeActivity()
        {
            string fileName = "Config/HeFuGifts/HeFuFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_HeFuRechargeActivityMutex)
            {
                _HeFuRechargeActivity = null;
            }

            return 0;
        }

        #endregion 合服充值返利

        #region 新区充值返利

        /// <summary>
        /// 获取新区充值返利活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static XinFanLiActivity GetXinFanLiActivity()
        {
            lock (_XinFanLiActivityMutex)
            {
                if (_XinFanLiActivity != null)
                {
                    return _XinFanLiActivity;
                }
            }

            try
            {
                // MU 返利改造
                //string fileName = "Config/Gifts/XinFanLi.xml";
                string fileName = "Config/XinFuGifts/MuFanLi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));

                if (null == xml) return null;

                XinFanLiActivity activity = new XinFanLiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(1, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(7, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();

                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)(Global.GetSafeAttributeDouble(xmlItem, "FanLi") * 100));
                            myAwardItem.AwardYuanBao = 0;

                            myAwardItem.GoodsDataList = new List<GoodsData>();

                            string rans = Global.GetSafeAttributeStr(xmlItem, "ID");
                            string[] paiHangs = rans.Split('-');

                            if (paiHangs.Length <= 0)
                            {
                                continue;
                            }

                            int min = Global.SafeConvertToInt32(paiHangs[0]);
                            int max = Global.SafeConvertToInt32(paiHangs[paiHangs.Length - 1]);

                            //设置排行奖励
                            for (int paiHang = min; paiHang <= max; paiHang++)
                            {
                                activity.AwardDict.Add(paiHang, myAwardItem);
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_XinFanLiActivityMutex)
                {
                    _XinFanLiActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/XinFuGifts/MuFanLi.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置新区充值返利活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetXinFanLiActivity()
        {
            string fileName = "Config/Gifts/XinFanLi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_XinFanLiActivityMutex)
            {
                _XinFanLiActivity = null;
            }

            return 0;
        }

        #endregion 新区充值返利

        #endregion 合服活动和新加的开区返利活动


        #region 每日充值豪礼

        /// <summary>
        /// 获取每日充值豪礼活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static MeiRiChongZhiActivity GetMeiRiChongZhiActivity()
        {
            lock (_MeiRiChongZhiHaoLiActivityMutex)
            {
                if (_MeiRiChongZhiHaoLiActivity != null)
                {
                    return _MeiRiChongZhiHaoLiActivity;
                }
            }

            try
            {
                string fileName = "Config/Gifts/DayChongZhi.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                MeiRiChongZhiActivity activity = new MeiRiChongZhiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            AwardItem myAwardItem = new AwardItem();
                            myAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinYuanBao"));
                            myAwardItem.GoodsDataList = new List<GoodsData>();
                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取每日充值豪礼活动配置文件中的物品配置1失败"));
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取每日充值豪礼活动配置文件中的物品配置项失败"));
                                else
                                    myAwardItem.GoodsDataList = ParseGoodsDataList(fields, "每日充值豪礼活动"); //将物品字符串列表解析成物品数据列表
                            }

                            int nID = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            activity.AwardDict.Add(nID, myAwardItem);

                        }
                    }
                }

                activity.PredealDateTime();

                lock (_MeiRiChongZhiHaoLiActivityMutex)
                {
                    _MeiRiChongZhiHaoLiActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/Gifts/DayChongZhi.xml解析出现异常", ex);
            }

            return null;
        }


        /// <summary>
        /// 重置获取每日充值活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetMeiRiChongZhiActivity()
        {
            string fileName = "Config/Gifts/DayChongZhi.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_MeiRiChongZhiHaoLiActivityMutex)
            {
                _MeiRiChongZhiHaoLiActivity = null;
            }

            return 0;
        }

        #endregion 每日充值豪礼


        #region 冲级豪礼

        /// <summary>
        /// 获取冲级豪礼活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static KingActivity GetChongJiHaoLiActivity()
        {
            lock (_ChongJiHaoLiActivityMutex)
            {
                if (_ChongJiHaoLiActivity != null)
                    return _ChongJiHaoLiActivity;
            }

            try
            {
                //string fileName = "Config/RiChangGifts/LevelAward.xml";
                string fileName = "Config/XinFuGifts/MuLevel.xml";                                  // MU 沿用
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.IsolateResPath(fileName));
                if (null == xml) return null;

                ChongJiHaoLiActivity activity = new ChongJiHaoLiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    //activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.FromDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");
                    activity.ToDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                    //activity.AwardStartDate = Global.GetSafeAttributeStr(args, "AwardStartDate");
                    activity.AwardStartDate = Global.GetHuoDongTimeByKaiFu(0, 0, 0, 0);
                    //activity.AwardEndDate = Global.GetSafeAttributeStr(args, "AwardEndDate");
                    activity.AwardEndDate = Global.GetHuoDongTimeByKaiFu(6, 23, 59, 59);
                }

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            int nID = (int)Global.GetSafeAttributeLong(xmlItem, "ID");
                            AwardItem myAwardItem1 = new AwardItem();
                            AwardItem myAwardItem2 = new AwardItem();
                            myAwardItem1.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinZhuanSheng"));
                            myAwardItem1.MinAwardCondionValue2 = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "Roles"));
                            myAwardItem1.MinAwardCondionValue3 = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinLevel"));

                            myAwardItem2.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinZhuanSheng"));
                            myAwardItem2.MinAwardCondionValue2 = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "Roles"));
                            myAwardItem2.MinAwardCondionValue3 = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "MinLevel"));

                            myAwardItem1.GoodsDataList = new List<GoodsData>();
                            myAwardItem2.GoodsDataList = new List<GoodsData>();

                            int rolelimit = (int)Global.GetSafeAttributeLong(xmlItem, "Roles");
                            if (rolelimit==-1)
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取MuLevel.xml失败 字段：Roles"));
                            else
                                activity.RoleLimit.Add(nID, rolelimit);

                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsOne");
                            if (string.IsNullOrEmpty(goodsIDs))
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取MuLevel.xml失败 GoodsOne"));
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取MuLevel.xml 失败 奖励列表1配置错误"));
                                else
                                    myAwardItem1.GoodsDataList = ParseGoodsDataList(fields, "读取MuLevel.xml 奖励列表1"); //将物品字符串列表解析成物品数据列表
                            }
                            activity.AwardDict.Add(nID, myAwardItem1);

                            goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsTwo");
                            if (string.IsNullOrEmpty(goodsIDs))
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取MuLevel.xml失败 GoodsTwo"));
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取MuLevel.xml 失败 奖励列表2 配置错误"));
                                else
                                    myAwardItem2.GoodsDataList = ParseGoodsDataList(fields, "读取MuLevel.xml 奖励列表2"); //将物品字符串列表解析成物品数据列表
                            }
                            activity.AwardDict2.Add(nID, myAwardItem2);
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_ChongJiHaoLiActivityMutex)
                {
                    _ChongJiHaoLiActivity = activity;
                }

                return activity;
            }
            catch (Exception e)
            {
                LogManager.WriteException(e.ToString());
            }

            return null;
        }

        /// <summary>
        /// 重置获取冲级豪礼活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetChongJiHaoLiActivity()
        {
            string fileName = "Config/XinFuGifts/MuLevel.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.IsolateResPath(fileName));

            lock (_ChongJiHaoLiActivityMutex)
            {
                _ChongJiHaoLiActivity = null;
            }

            return 0;
        }

        #endregion 冲级豪礼


        #region 神装激情回馈

        /// <summary>
        /// 获取神装激情回馈活动的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static ShenZhuangHuiKuiHaoLiActivity GetShenZhuangJiQiHuiKuiHaoLiActivity()
        {
            lock (_ShenZhuangJiQingHuiKuiHaoLiActivityMutex)
            {
                if (_ShenZhuangJiQingHuiKuiHaoLiActivity != null)
                    return _ShenZhuangJiQingHuiKuiHaoLiActivity;
            }

            try
            {
                string fileName = "Config/RiChangGifts/ShenZhuangAward.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml) return null;

                ShenZhuangHuiKuiHaoLiActivity activity = new ShenZhuangHuiKuiHaoLiActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                    activity.ActivityType = (int)Global.GetSafeAttributeLong(args, "ActivityType");

                args = xml.Element("GiftList");
                if (null != args)
                {
                    IEnumerable<XElement> xmlItems = args.Elements();
                    foreach (var xmlItem in xmlItems)
                    {
                        if (null != xmlItem)
                        {
                            activity.MyAwardItem.MinAwardCondionValue = Global.GMax(0, (int)Global.GetSafeAttributeLong(xmlItem, "Roles")); // 这表示的是一共能有几个玩家获取
                            activity.MyAwardItem.GoodsDataList = new List<GoodsData>();
                            string goodsIDs = Global.GetSafeAttributeStr(xmlItem, "GoodsIDs");
                            if (string.IsNullOrEmpty(goodsIDs))
                                LogManager.WriteLog(LogTypes.Warning, string.Format("读取神装激情回馈豪礼配置文件1失败"));
                            else
                            {
                                string[] fields = goodsIDs.Split('|');
                                if (fields.Length <= 0)
                                    LogManager.WriteLog(LogTypes.Warning, string.Format("读取神装激情回馈配置文件失败"));
                                else
                                    activity.MyAwardItem.GoodsDataList = ParseGoodsDataList(fields, "神装激情回馈"); //将物品字符串列表解析成物品数据列表
                            }
                        }
                    }
                }

                activity.PredealDateTime();

                lock (_ShenZhuangJiQingHuiKuiHaoLiActivityMutex)
                {
                    _ShenZhuangJiQingHuiKuiHaoLiActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/RiChangGifts/ShenZhuangAward.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取冲级豪礼活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetShenZhuangJiQiHuiKuiHaoLiActivity()
        {
            string fileName = "Config/RiChangGifts/ShenZhuangAward.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_ShenZhuangJiQingHuiKuiHaoLiActivityMutex)
            {
                _ShenZhuangJiQingHuiKuiHaoLiActivity = null;
            }

            return 0;
        }

        #endregion 神装激情回馈


        #region 月度大装盘

        /// <summary>
        /// 获取月度大装盘的配置项
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static YueDuZhuanPanActivity GetYueDuZhuanPanActivity()
        {
            lock (_YueDuZhuanPanActivityMutex)
            {
                if (_YueDuZhuanPanActivity != null)
                    return _YueDuZhuanPanActivity;
            }

            try
            {
                string fileName = "Config/RiChangGifts/NewDig2.xml";
                XElement xml = GeneralCachingXmlMgr.GetXElement(Global.GameResPath(fileName));
                if (null == xml)
                    return null;

                YueDuZhuanPanActivity activity = new YueDuZhuanPanActivity();

                XElement args = xml.Element("Activities");
                if (null != args)
                {
                    activity.FromDate = Global.GetSafeAttributeStr(args, "FromDate");
                    activity.ToDate = Global.GetSafeAttributeStr(args, "ToDate");

                }
                lock (_YueDuZhuanPanActivityMutex)
                {
                    _YueDuZhuanPanActivity = activity;
                }

                return activity;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Fatal, "Config/RiChangGifts/NewDig2.xml解析出现异常", ex);
            }

            return null;
        }

        /// <summary>
        /// 重置获取月度抽奖活动的配置项, 以便下次访问强迫读取配置文件
        /// </summary>
        /// <param name="whichOne"></param>
        /// <returns></returns>
        public static int ResetYueDuZhuanPanActivity()
        {
            string fileName = "Config/RiChangGifts/NewDig2.xml";
            GeneralCachingXmlMgr.RemoveCachingXml(Global.GameResPath(fileName));

            lock (_YueDuZhuanPanActivityMutex)
            {
                _YueDuZhuanPanActivity = null;
            }

            return 0;
        }

        #endregion 月度大装盘



        public class TotalConsumeActivity:KingActivity
        {
            public  override bool CanGiveAward(KPlayer client, int index, int totalMoney)
            {
               // bool hasbagslot = HasEnoughBagSpaceForAwardGoods(client,index);
                bool hasGet = false;
                try
                {
                    if (AwardDict != null && AwardDict.ContainsKey(index))
                    {
                        if (AwardDict[index].MinAwardCondionValue > totalMoney)
                        {
                            hasGet = false;
                        }
                        else
                        {
                            hasGet = true;
                        }
                    }

                }
                catch (Exception e)
                {
                    LogManager.WriteException(e.ToString());
                }
                if ( hasGet)
                    return true;
                return false;
            }
        }
        /// <summary>
        ///
        /// </summary>
        public class TotalChargeActivity : KingActivity
        {
            public override bool CanGiveAward(KPlayer client, int index, int totalMoney)
            {
               // bool hasbagslot = HasEnoughBagSpaceForAwardGoods(client,index);
                bool hasGet = false;
                try
                {
                    if (AwardDict != null && AwardDict.ContainsKey(index))
                    {
                        if (AwardDict[index].MinAwardCondionValue > totalMoney)
                        {
                            hasGet = false;
                        }
                        else
                        {
                            hasGet = true;
                        }
                    }

                }
                catch (Exception e)
                {
                    LogManager.WriteException(e.ToString());
                }
                if (hasGet)
                    return true;
                return false;
            }
        }
    }


}
