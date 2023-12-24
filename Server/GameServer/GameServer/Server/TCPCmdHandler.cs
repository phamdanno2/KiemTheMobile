using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Activity.DaySeriesLoginEvent;
using GameServer.KiemThe.Core.Activity.EveryDayOnlineEvent;
using GameServer.KiemThe.Core.Activity.LevelUpEvent;
using GameServer.KiemThe.Core.Activity.RechageEvent;
using GameServer.KiemThe.Core.Activity.TotalLoginEvent;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System.Collections.Generic;

namespace GameServer.Server
{
    /// <summary>
    /// Danh sách Packet
    /// </summary>
    public enum TCPGameServerCmds
    {
        CMD_UNKNOWN = 0,
        CMD_LOGIN_ON1 = 1,
        CMD_LOGIN_ON2 = 20,
        CMD_NTF_CMD_BASE_ID = 21,
        CMD_LOG_OUT = 22,
        CMD_SPR_CLIENTHEART = 23,
        CMD_SPR_GENERATE_NEW_KEY = 24,

        /// <summary>
        /// Truy vấn tham biến hệ thống
        /// </summary>
        CMD_DB_QUERY_SYSPARAM = 25,

        /// <summary>
        /// Truy vấn Võ lâm liên đấu
        /// </summary>
        CMD_DB_TEAMBATTLE = 26,

        CMD_PREREMOVE_ROLE = 98, //
        CMD_UNREMOVE_ROLE = 99, // 
        CMD_LOGIN_ON = 100,
        CMD_ROLE_LIST, CMD_CREATE_ROLE, CMD_REMOVE_ROLE,
        CMD_INIT_GAME, CMD_SYNC_TIME, CMD_PLAY_GAME, CMD_SPR_MOVE, CMD_SPR_MOVEEND, CMD_SPR_MOVE2,
        CMD_OTHER_ROLE, CMD_OTHER_ROLE_DATA, CMD_SPR_POSITION, CMD_SPR_PETPOS, CMD_SPR_ACTTION, CMD_SPR_ACTTION2,
        CMD_SPR_MAGICCODE, CMD_SPR_ATTACK, CMD_SPR_INJURE,
        CMD_SPR_REALIVE, CMD_SPR_RELIFE, CMD_SPR_CLICKON, CMD_SYSTEM_MONSTER,
        CMD_SPR_MAPCHANGE, CMD_SPR_ENTERMAP, CMD_SPR_NEWTASK, CMD_SPR_GETATTRIB2,
        CMD_SPR_LEAVE, CMD_SPR_NPC_BUY, CMD_SPR_NPC_SALEOUT, CMD_SPR_ADD_GOODS, CMD_SPR_MOD_GOODS,
        CMD_SPR_MERGE_GOODS, CMD_SPR_SPLIT_GOODS, CMD_SPR_GET_MERGETYPES, CMD_SPR_GET_MERGEITEMS, CMD_SPR_GET_MERGENEWGOODS,
        CMD_SPR_CHGCODE, CMD_SPR_MONEYCHANGE, CMD_SPR_MODTASK, CMD_SPR_COMPTASK, CMD_SPR_EXPCHANGE,
        CMD_SPR_GETFRIENDS, CMD_SPR_ADDFRIEND, CMD_SPR_REMOVEFRIEND, CMD_SPR_REJECTFRIEND, CMD_SPR_ASKFRIEND,
        CMD_SPR_NEWGOODSPACK, CMD_SPR_DELGOODSPACK, CMD_SPR_CLICKONGOODSPACK,
        CMD_SPR_GETTHING, CMD_SPR_CHGPKMODE, CMD_SPR_CHGPKVAL, CMD_SPR_UPDATENPCSTATE, CMD_SPR_NPCSTATELIST, CMD_SPR_GETNEWTASKDATA,
        CMD_SPR_ABANDONTASK, CMD_SPR_HITED, CMD_SPR_MODKEYS, CMD_SPR_CHAT,
        CMD_SPR_USEGOODS, CMD_SPR_CHANGEPOS, CMD_SPR_NOTIFYCHGMAP, CMD_SPR_FORGE, CMD_SPR_ENCHANCE,
        CMD_SPR_GETOTHERATTRIB, CMD_SPR_UPDATE_ROLEDATA, CMD_SPR_REMOVE_COOLDOWN,
        CMD_SPR_MALL_BUY, CMD_SPR_BoundToken_BUY, CMD_SPR_TokenCHANGE, CMD_SPR_BoundTokenCHANGE, CMD_SPR_GOODSEXCHANGE, CMD_SPR_EXCHANGEDATA,
        CMD_SPR_MOVEGOODSDATA, CMD_SPR_GOODSSTALL, CMD_SPR_STALLDATA, CMD_SPR_STALLNAME,
        CMD_SPR_TEAM, CMD_SPR_TEAMDATA, CMD_SPR_TEAMID, CMD_SPR_BATTLE, CMD_SPR_NPCSCRIPT,
        CMD_SPR_DEAD, CMD_SPR_AUTOFIGHT, CMD_SPR_HORSE, CMD_SPR_PET,
        CMD_SPR_DIANJIANGLIST, CMD_SPR_DIANJIANGDATA, CMD_SPR_DJROOMROLESDATA, CMD_SPR_DIANJIANG, CMD_SPR_DIANJIANGFIGHT,
        CMD_SPR_DIANJIANGPOINT, CMD_SPR_GETDJPOINTS, CMD_SPR_UPDATEINTERPOWER,
        CMD_SPR_GOTOMAP, CMD_SPR_NOTIFYMSG, CMD_SPR_QUERYIDBYNAME, CMD_ADDHORSE, CMD_ADDPET, CMD_GETHORSELIST, CMD_GETOTHERHORSELIST, CMD_GETPETLIST,
        CMD_MODHORSE, CMD_MODPET, CMD_SELECTHORSE, CMD_GETGOODSLISTBYSITE, CMD_GETLINEINFO, CMD_GETJINGMAILIST, CMD_UP_JINGMAI_LEVEL,
        CMD_GETOTHERJINGMAILIST, CMD_SPR_LOADALREADY, CMD_SPR_BULLETINMSG, CMD_SPR_GMAUTH, CMD_SPR_EQUIPUPGRADE, CMD_SPR_ENCHASEJEWEL,
        CMD_SPR_SHOWBIGUAN, CMD_SPR_GETBIGUAN, CMD_SPR_UPSKILLLEVEL, CMD_SPR_ADD_SKILL, CMD_SPR_JINGMAI_INFO, CMD_SPR_HORSEENCHANCE,
        CMD_SPR_HORSEUPGRADE, CMD_SPR_SALEGOODS, CMD_SPR_SELFSALEGOODSLIST, CMD_SPR_OTHERSALEGOODSLIST, CMD_SPR_MARKETROLELIST,
        CMD_SPR_MARKETGOODSLIST, CMD_SPR_MARKETBUYGOODS, CMD_SPR_MODDEFSKILLID, CMD_SPR_MODAUTODRINK, CMD_SPR_PLAYDECO, CMD_SPR_BUFFERDATA,
        CMD_SPR_RUNTOMAP, CMD_SPR_SEARCHROLES, CMD_SPR_LISTROLES, CMD_SPR_LISTTEAMS, CMD_SPR_RESETBAG, CMD_SPR_DAILYTASKDATA, CMD_SPR_DAILYJINGMAIDATA,
        CMD_SPR_CHGNUMSKILLID, CMD_SPR_GETSKILLUSEDNUM, CMD_SPR_CHGHORSEBODY, CMD_SPR_PORTABLEBAGDATA, CMD_SPR_RESETPORTABLEBAG,
        CMD_SPR_EXECWABAO, CMD_SPR_GETWABAODATA, CMD_SPR_GETHUODONGDATA, CMD_SPR_GETWLOGINGIFT, CMD_SPR_GETNEWSTEPGIFT, CMD_SPR_GETMTIMEGIFT,
        CMD_SPR_GETBIGGIFT, CMD_SPR_GETSONGLIGIFT, CMD_SPR_CHGHUODONGID, CMD_SPR_FUBENDATA, CMD_SPR_ENTERFUBEN, CMD_SPR_NOTIFYENTERFUBEN,
        CMD_SPR_CLIENTHEART_OLD, CMD_SPR_OHTERJINGMAIEXP, CMD_GETRANDOMNAME, CMD_SKILLUSEDNUMFULL, CMD_SPR_GETFUBENHISTDATA, CMD_SPR_GETFUBENBEGININFO,
        CMD_SPR_COPYMAPMONSTERSNUM, CMD_SPR_FINDMONSTER, CMD_SPR_BATCHYINPIAO, CMD_SPR_FORCETOLAOFANG, CMD_SPR_CHGPURPLENAME, CMD_SPR_CHGLIANZHAN,
        CMD_SPR_GETROLEDAILYDATA, CMD_SPR_GETBOSSINFODICT, CMD_SPR_GETPAIHANGLIST, CMD_SPR_YABIAODATA, CMD_SPR_STARTYABIAO, CMD_SPR_ENDYABIAO,
        CMD_SPR_YABIAOTAKEGOODS, CMD_SPR_TOUBAO, CMD_SPR_GETOTHERATTRIB2, CMD_SPR_NEWBIAOCHE, CMD_SPR_DELBIAOCHE, CMD_SPR_FINDBIAOCHE,
        CMD_SPR_CHGBIAOCHELIFEV, CMD_SPR_NOTIFYENDCHONGXUE, CMD_SPR_ADDHORSELUCKY, CMD_SPR_BATTLEKILLEDNUM, CMD_SPR_CHGBATTLENAMEINFO,
        CMD_SPR_NOFITYPOPUPWIN, CMD_SPR_NOTIFYBATTLEROLEINFO, CMD_SPR_NOTIFYBATTLEENDINFO, CMD_SPR_GETCHONGZHIJIFEN, CMD_SPR_NOTIFYTEAMCHGLEVEL,
        CMD_SPR_GETFUBENHISTLISTDATA, CMD_SPR_CHGHEROINDEX, CMD_GETOTHERHORSEDATA, CMD_UPDATEALLTHINGINDEXS, CMD_SPR_CHGHALFBoundTokenPERIOD,
        CMD_SPR_GETBANGHUILIST, CMD_SPR_CREATEBANGHUI, CMD_SPR_CHGBANGHUIINFO, CMD_SPR_QUERYBANGHUIDETAIL, CMD_SPR_UPDATEBANGHUIBULLETIN,
        CMD_SPR_GETBHMEMBERDATALIST, CMD_SPR_UPDATEBHVERIFY, CMD_SPR_APPLYTOBHMEMBER, CMD_SPR_ADDBHMEMBER, CMD_SPR_REMOVEBHMEMBER,
        CMD_SPR_QUITFROMBANGHUI, CMD_SPR_DESTROYBANGHUI, CMD_SPR_BANGHUIVERIFY, CMD_SPR_INVITETOBANGHUI, CMD_SPR_CHGBHMEMBERZHIWU,
        CMD_SPR_CHGBHMEMBERCHENGHAO, CMD_SPR_SEARCHROLESFROMDB, CMD_SPR_AGREETOTOBANGHUI, CMD_SPR_REFUSEAPPLYTOBH, CMD_SPR_GETBANGGONGHIST,
        CMD_SPR_DONATEBGMONEY, CMD_SPR_DONATEBGGOODS, CMD_SPR_BANGGONGCHANGE, CMD_SPR_GETBANGQIINFO, CMD_SPR_RENAMEBANGQI, CMD_SPR_UPLEVELBANGQI,
        CMD_SPR_CHGJUNQILIFEV, CMD_SPR_NEWJUNQI, CMD_SPR_DELJUNQI, CMD_SPR_LINGDIFORBH, CMD_SPR_CHGHUANGDIROLEID, CMD_SPR_GETBHLINGDIINFODICTBYBHID,
        CMD_SPR_SETLINGDITAX, CMD_SPR_TAKELINGDITAXMONEY, CMD_SPR_GETHUANGDIBHINFO, CMD_SPR_NOTIFYBHZHIWU, CMD_SPR_OPENYANGGONGBK, CMD_SPR_REFRESHYANGGONGBK,
        CMD_SPR_CLICKYANGGONGBK, CMD_SPR_REFRESHQIZHENGE, CMD_SPR_QIZHEGEBUY, CMD_SPR_QUERYQIZHEGEBUYHIST, CMD_SPR_QUICKJINGMAI, CMD_SPR_QUICKHORSEENCHANCE,
        CMD_SPR_QUICKEQUIPENHANCE, CMD_SPR_QUICKEQUIPFORGE, CMD_SPR_GETHUANGDIROLEDATA, CMD_SPR_ADDHUANGFEI, CMD_SPR_REMOVEHUANGFEI, CMD_SPR_GETHUANGFEIDATA,
        CMD_SPR_SENDTOLAOFANG, CMD_SPR_TAKEOUTLAOFANG, CMD_SPR_BANCHAT, CMD_SPR_CHGHUANGHOU, CMD_SPR_GETLINGDIMAPINFO, CMD_SPR_GETHUANGCHENGMAPINFO,
        CMD_SPR_ADDLINGDITAXMONEY, CMD_SPR_INVITEADDHUANGFEI, CMD_SPR_AGREEADDHUANGFEI, CMD_SPR_TASKTRANSPORT, CMD_SPT_LINGLIGUANZHU, CMD_SPR_GETGOODSBYDBID,
        CMD_SPR_QUICKCOMPLETETASK, CMD_SPR_QUERYCHONGZHIMONEY, CMD_SPR_GETFIRSTCHONGZHIDALI, CMD_SPR_NOTIFYBATTLESIDE, CMD_SPR_NOTIFYBATTLEAWARD, CMD_SPR_EXECWABAOBYYAOSHI,
        CMD_SPR_SUBFORGE, CMD_SPR_GETUSERMAILLIST, CMD_SPR_GETUSERMAILDATA, CMD_SPR_FETCHMAILGOODS, CMD_SPR_DELETEUSERMAIL, CMD_SPR_GETMAILSENDCODE,
        CMD_SPR_SENDUSERMAIL, CMD_SPR_RECEIVELASTMAIL, CMD_SPR_EQUIPBORNINDEXUPDATE, CMD_SPR_EQUIPINHERIT, CMD_SPR_QUERYINPUTFANLI,
        CMD_SPR_QUERYINPUTJIASONG, CMD_SPR_QUERYINPUTKING, CMD_SPR_QUERYLEVELKING, CMD_SPR_QUERYEQUIPKING, CMD_SPR_QUERYHORSEKING, CMD_SPR_QUERYJINGMAIKING,
        CMD_SPR_QUERYAWARDHIST, CMD_SPR_EXECUTEINPUTFANLI, CMD_SPR_EXECUTEINPUTJIASONG, CMD_SPR_EXECUTEINPUTKING, CMD_SPR_EXECUTELEVELKING, CMD_SPR_EXECUTEEQUIPKING,
        CMD_SPR_EXECUTEHORSEKING, CMD_SPR_EXECUTEJINGMAIKING, CMD_SPR_MALLZHENQIBUY, CMD_SPR_FETCHACTIVITYAWARD, CMD_SPR_VIPDAILYDATA, CMD_SPR_USEVIPDAILYPRIORITY,
        CMD_SPR_ACTIVITYTRANSPORT, CMD_SPR_YANGGONGBKDAILYDATA, CMD_SPR_FETCHYANGGONGBKJIFENAWARD, CMD_SPR_QUERYSHILIANTAAWARDINFO, CMD_SPR_FETCHSHILIANTAAWARD,
        CMD_SPR_COMPLETETINYCLIENT, CMD_SPR_USERBoundMoneyCHANGE, CMD_SPR_NOTIFYSHENGXIAOGUESSSTAT, CMD_SPR_NOTIFYSHENGXIAOGUESSRESULT, CMD_SPR_ADDSHENGXIAOMORTGAGE,
        CMD_SPR_QUERYROLESHENGXIAOGUESSLIST, CMD_SPR_QUERYSHENGXIAOGUESSHISTORY, CMD_SPR_QUERYSHENGXIAORECENTRESULTLIST, CMD_SPR_QUERYSHENGXIAOGUESSSELFHISTORY,
        CMD_SPR_UPDATETENGXUNFCMRATE, CMD_SPR_NEWNPC, CMD_SPR_DELNPC, CMD_SPR_EXTGRIDBYYUANBAO, CMD_SPR_SUBMONEY, CMD_SPR_EXTBAGNUMBYYUANBAO, CMD_SPR_STOPMOVE,
        CMD_SPR_NOTIFYEQUIPSTRONG,
        CMD_SPR_EXCUTENPCLUATALK, CMD_SPR_EXCUTENPCLUAFUNCTION, CMD_SPR_ARENABATTLE, CMD_SPR_ARENABATTLEKILLEDNUM, CMD_SPR_CITYWARREQUEST, CMD_SPR_TAKELINGDIDAILYAWARD,
        CMD_SPR_NOTIFYOPENWINDOW, CMD_SPR_CHENGJIUDATA, CMD_SPR_FETCHCHENGJIUAWARD, CMD_SPR_DSHIDECMD, CMD_SPR_NEWDECO, CMD_SPR_DELDECO, CMD_SPR_MENDEQUIPMENT,
        CMD_SPR_NOTIFYGOODSINFO, CMD_SPR_ROLEPARAMSCHANGE, CMD_SPR_EQUIPFENJIE, CMD_SPR_JINGYUANEXCHANGE, CMD_SPR_HUIZHANGEXCHANGE, CMD_SPR_ACTIVATNEXTLEVELJINGMAI,
        CMD_SPR_FETCHVIPONCEAWARD, CMD_SPR_TASKTRANSPORT2, CMD_SPR_ACTIVATNEXTLEVELWUXUE, CMD_SPR_CAIJI, CMD_SPR_RUNTASKPLOTLUA,
        CMD_SPR_PLAYGAMEEFFECT, CMD_SPR_TRANSFERSOMETHING, CMD_SPR_CHANGEPETAITYPE, CMD_SPR_FETCHMALLDATA, CMD_SPR_MALLQIANGGOUBUYGOODS,
        CMD_SPR_FETCHZUANHUANGAWARD, CMD_SPR_SETSYSTEMOPENPARAMS, CMD_SPR_ENTERTASKFUBEN, CMD_SPR_GETUPLEVELGIFTOK, CMD_SPR_UPDATEWEIGHTS,
        CMD_SPR_GETTASKAWARDS, CMD_SPR_NOTIFYGETGOODSPACK, CMD_SPR_RESETJINDANBAG, CMD_SPR_GETJINDANGOODSLIST, CMD_SPR_ZAJINDAN, CMD_SPR_QUERYZAJINDANHISTORY,
        CMD_SPR_QUERYSELFZAJINDANHISTORY, CMD_SPR_GETWANGCHENGMAPINFO, CMD_SPR_GETLIMITTIMELOGINGIFT, CMD_SPR_ROLESTATUSCMD, CMD_SPR_GETTO60AWARD, CMD_SPR_GETKAIFUONLINEINFO,
        CMD_SPR_GETDAYCHONGZHIDALI, CMD_SPR_GETJIERIXMLDATA, CMD_SPR_QUERYJIERIDALIBAO, CMD_SPR_QUERYJIERIDENGLU, CMD_SPR_QUERYJIERIVIP, CMD_SPR_QUERYJIERICZSONG, CMD_SPR_QUERYJIERICZLEIJI,
        CMD_SPR_QUERYJIERIZIKA, CMD_SPR_QUERYJIERIXIAOFEIKING, CMD_SPR_QUERYJIERICZKING, CMD_SPR_EXECUTEJIERIDALIBAO, CMD_SPR_EXECUTEJIERIDENGLU, CMD_SPR_EXECUTEJIERIVIP, CMD_SPR_EXECUTEJIERICZSONG, CMD_SPR_EXECUTEJIERICZLEIJI,
        CMD_SPR_EXECUTEJIERIZIKA, CMD_SPR_EXECUTEJIERIXIAOFEIKING, CMD_SPR_EXECUTEJIERICZKING, CMD_SPR_CHGJIERICHENGHAO, CMD_SPR_FACTIVITIESDATA, CMD_SPR_YUANBAOCOMPLETETASK,
        CMD_SPR_QUERYHEFUDALIBAO, CMD_SPR_QUERYHEFUVIP, CMD_SPR_QUERYHEFUCZSONG, CMD_SPR_QUERYHEFUFANLI, CMD_SPR_QUERYHEFUPKKING, CMD_SPR_QUERYHEFUWCKING, CMD_SPR_QUERYXINFANLI,
        CMD_SPR_EXECUHEFUDALIBAO, CMD_SPR_EXECUHEFUVIP, CMD_SPR_EXECUHEFUCZSONG, CMD_SPR_EXECUHEFUFANLI, CMD_SPR_EXECUHEFUPKKING, CMD_SPR_EXECUHEFUWCKING, CMD_SPR_EXECUXINFANLI,
        CMD_SPR_ONEKEYQUICKSALEOUT, CMD_SPR_ACTIVATNEXTLEVELZHANHUN, CMD_SPR_ACTIVATNEXTLEVELRONGYU, CMD_SPR_ACTIVATRONGYUBUFFER, CMD_SPR_LIANLUJINGLIAN, CMD_SPR_ZJDJIFEN,
        CMD_SPR_FETCHZJDJIFENAWARD,
        CMD_SPR_QUERYACTIVITYINFO,  // 客户端请求活动的相关信息 -- 比如冲级豪礼名额 神装领取名额 幸运抽奖次数[7/18/2013 LiaoWei]
        CMD_SPR_XINGYUNCHOUJIANG,   // 幸运抽奖 [7/18/2013 LiaoWei]
        CMD_SPR_QUERYYUEDUCHOUJIANGHISTORY,         // 客户端请求月度抽奖历史(全服玩家)[7/23/2013 LiaoWei]
        CMD_SPR_QUERYSELFQUERYYUEDUCHOUJIANGHISTORY,// 客户端请求月度抽奖历史(玩家自己) [7/23/2013 LiaoWei]
        CMD_SPR_EXECUTEYUEDUCHOUJIANG,              // 客户端点击月度抽奖 [7/23/2013 LiaoWei]
        CMD_SPR_QUERYYUEDUCHOUJIANGINFO,            // 客户端请求月度抽奖信息--能玩的次数和活动期间充值的元宝数 [7/23/2013 LiaoWei]
        CMD_SPR_EXECUTEHUNQIEXCHANGE,               // 客户端发起魂器卖出操作 [8/7/2013 LiaoWei]
        CMD_SPR_EXECUTECHANGEOCCUPATION,            // 客户端发起转职操作  [9/28/2013 LiaoWei]
        CMD_SPR_EXECUTECHANGELIFE,                  // 客户端发起转生操作  [9/28/2013 LiaoWei]
        CMD_SPR_BEGINBLINK,                         // 闪现开始 [10/28/2013 LiaoWei]
        CMD_SPR_ENDBLINK,                           // 闪现结束 [10/28/2013 LiaoWei]
        CMD_SPR_GETROLEUSINGGOODSDATALIST,
        CMD_SPR_EXECUTEPROPADDPOINT,                // 属性加点 [10/31/2013 LiaoWei]
        CMD_SPR_EXECUTERECOMMENDPROPADDPOINT,       // 推荐属性加点 [10/31/2013 LiaoWei]
        CMD_SPR_EXECUTERECLEANPROPADDPOINT,         // 清除属性加点 [10/31/2013 LiaoWei]
        CMD_SPR_QUERYCLEANPROPADDPOINT,             // 请求清除属性加点信息 [2/11/2014 LiaoWei]
        CMD_SPR_BLOODCASTLEBEGINFIGHT,              // 血色堡垒开始战斗 -- 客户端把桥头的阻挡去掉 [11/7/2013 LiaoWei]
        CMD_SPR_BLOODCASTLEKILLMONSTERAHASDONE,     // 血色堡垒断桥怪击杀到达限额 -- 客户端把桥尾的阻挡去掉 [11/7/2013 LiaoWei]
        CMD_SPR_BLOODCASTLEENDFIGHT,                // 血色堡垒结束战斗 -- 客户端显示倒计时界面 [11/7/2013 LiaoWei]
        CMD_SPR_FUBENCLEANOUT,                      // 副本扫荡 [11/15/2013 LiaoWei]
        CMD_SPR_FUBENPASSNOTIFY,                    // 副本通关通告 [11/15/2013 LiaoWei]
        CMD_SPR_QUERYFUBENINFO,                    // 客户端请求副本信息 [11/15/2013 LiaoWei]
        CMD_SPR_ATTACK2,                           // 2号攻击请求 针对于黑龙波等特殊技能 [11/22/2013 LiaoWei]
        CMD_SPR_COMPLETEFLASHSCENE,                // 完成新手场景 [11/30/2013 LiaoWei]
        CMD_SPR_FRESHPLAYERSCENEKILLMONSTERAHASDONE,// 新手场景断桥怪击杀到达限额 -- 客户端把桥尾的阻挡去掉 [12/1/2013 LiaoWei]
        CMD_SPR_REFURBISHTASKSTARLEVEL,             // 客户端刷新任务星级 [12/3/2013 LiaoWei]
        CMD_SPR_COMPLETEDAILYCIRCLETASKFORONCECLICK,// 一键完成日常跑环任务 [12/5/2013 LiaoWei]
        CMD_SPR_ADMIREDPLAYER,                      // 客户端点击崇拜某人的操作 [12/10/2013 LiaoWei]
        CMD_SPR_QUERYBLOODCASTLEINFO,               // 请求血色堡垒基本信息 [12/14/2013 LiaoWei]
        CMD_SPR_EQUIPAPPENDPROP,                    // 装备追加消息 [12/18/2013 LiaoWei]
        CMD_SPR_BLOODCASTLEPREPAREFIGHT,            // 血色堡垒准备战斗 -- 客户端显示战斗倒计时 [12/20/2013 LiaoWei]
        CMD_SPR_BLOODCASTLECOMBATPOINT,             // 血色堡垒战斗积分 -- 客户端显示战斗积分 [12/20/2013 LiaoWei]
        CMD_SPR_BLOODCASTLEKILLMONSTERSTATUS,       // 血色堡垒杀怪状态 -- 客户端显示杀怪的状态 [12/20/2013 LiaoWei]
        CMD_SPR_QUERYCAMPBATTLEINFO,                // 请求阵营战场基本信息 [12/23/2013 LiaoWei]
        CMD_SPR_QUERYDAIMONSQUAREINFO,              // 请求恶魔广场基本信息 [12/25/2013 LiaoWei]
        CMD_SPR_QUERYDAIMONSQUARETIMERINFO,         // 恶魔广场时间信息 [12/25/2013 LiaoWei]
        CMD_SPR_QUERYDAIMONSQUAREMONSTERWAVEANDPOINTRINFO,// 恶魔广场怪物和得分信息 [12/25/2013 LiaoWei]
        CMD_SPR_DAIMONSQUAREENDFIGHT,                // 恶魔广场结束 [12/25/2013 LiaoWei]
        CMD_SPR_UPDATEEVERYDAYONLINEAWARDGIFTINFO,    // 更新玩家的每日在线信息 [1/12/2014 LiaoWei]
        CMD_SPR_GETEVERYDAYONLINEAWARDGIFT,          // 领取每日在线奖励 [1/12/2014 LiaoWei]
        CMD_SPR_UPDATEEVERYDAYSERIESLOGININFO,      // 更新玩家的连续登陆信息 [1/12/2014 LiaoWei]
        CMD_SPR_GETEVERYDAYSERIESLOGINAWARDGIFT,    // 领取连续登陆奖励 [1/12/2014 LiaoWei]
        CMD_SPR_FRESHPLAYERSCENEOVERTIME,           // 新手场景超时 通知客户端 [1/16/2014 LiaoWei]
        CMD_SPR_UPDATEGETTHINGSFLAG,                // 更新拾取设置
        CMD_SPR_BLOODCASTLEPLAYERNUMNOTIFY,         // 血色堡垒人数通知 [1/20/2014 LiaoWei]
        CMD_SPR_DAIMONSQUAREPLAYERNUMNOTIFY,        // 恶魔广场人数通知 [1/20/2014 LiaoWei]
        CMD_SPR_BATTLEPLAYERNUMNOTIFY,              // 阵营战人数通知  [1/20/2014 LiaoWei]
        CMD_SPR_EXCHANGEMOJINGANDQIFU,              // 魔晶和祈福兑换 [1/23/2014 LiaoWei]
        CMD_SPR_GETMEDITATEEXP,                     // 获取冥想经验 [1/24/2014 LiaoWei]
        CMD_SPR_GETMEDITATETIMEINFO,                // 获取冥想时间信息 [1/24/2014 LiaoWei]
        CMD_SPR_QUERYTOTALLOGININFO,                // 请求累计登陆数据 [2/11/2014 LiaoWei]
        CMD_SPR_GETTOTALLOGINAWARD,                 // 领取累计登陆奖励 [2/11/2014 LiaoWei]
        CMD_SPR_CHANGELIFEFOREQUIP,                 // 客户端装备转生操作 [2/15/2014 LiaoWei]
        CMD_SPR_FLAKEOFFCHANGELIFEFOREQUIP,         // 客户端装备转生剥离操作 [2/15/2014 LiaoWei]
        CMD_SPR_ONEKEYFINDFRIEND,                   // 点击一键征友 [2/15/2014 LiaoWei]
        CMD_SPR_ONEKEYADDFRIEND,                    // 点击一键加友 [2/15/2014 LiaoWei]
        CMD_SPR_GETVIPAWARD,                        // 获得VIP奖励 [2/20/2014 LiaoWei]
        CMD_SPR_DAILYACTIVEDATA,                    // 获取每日活跃信息 [2/25/2014 LiaoWei]
        CMD_SPR_GETDAILYACTIVEAWARD,                // 获取每日活跃奖励 [2/25/2014 LiaoWei]
        CMD_SPR_SETAUTOASSIGNPROPERTYPOINT,         // 设置自动分配点设置 [3/3/2014 LiaoWei]
        CMD_SPR_GETBLOODCASTLEAWARD,                // 领取血色堡垒的奖励 [3/8/2014 LiaoWei]
        CMD_SPR_GETDAIMONSQUAREAWARD,               // 领取恶魔广场的奖励 [3/8/2014 LiaoWei]
        CMD_SPR_GETCOPYMAPAWARD,                    // 领取副本奖励 [3/5/2014 LiaoWei]
        CMD_SPR_GETSKILLINFO,                       // 客户端请求技能信息 [3/17/2014 LiaoWei]
        CMD_SPR_EXPERIENCECOPYMAPINFO,              // 经验副本信息 [3/18/2014 LiaoWei]
        CMD_SPR_ZHANMENGSHIJIAN_DETAIL,             // 战盟事件详情 [3/14/2014 JinJieLong]
        CMD_SPR_KAIFUACTIVITYINFO,                  // 开服活动信息 [3/20/2014 LiaoWei]
        CMD_SPR_GETTHEKINGOFPKINFO,                 // 请求PK之王基本信息 [3/22/2014 LiaoWei]
        CMD_SPR_NOTIFYTHEKINGOFPKAWARDINFO,         // PK之王奖励信息 [3/22/2014 LiaoWei]
        CMD_SPR_ANGELTEMPLETIMERINFO,               // 天使神殿时间信息(准备战斗、开始战斗、结束战斗) [12/20/2013 LiaoWei]
        CMD_SPR_ANGELTEMPLEFIGHTEND,                // 天使神殿结束--显示奖励[12/20/2013 LiaoWei]
        CMD_SPR_ANGELTEMPLEFIGHTINFOALL,            // 天使神殿战斗信息--群发 [3/23/2014 LiaoWei]
        CMD_SPR_ANGELTEMPLEFIGHTINFOSINGLE,         // 天使神殿战斗信息--给自己 [3/23/2014 LiaoWei]
        CMD_SPR_ANGELTEMPLESPARK,                   // 天使神殿战力鼓舞 [3/23/2014 LiaoWei]
        CMD_SPR_GETANGELTEMPLEBASEINFO,             // 天使神殿基本信息[3/23/2014 LiaoWei]
        CMD_SPR_QUERYADRATIONPKKINGINFO,            // 请求PK之王崇拜信息[3/23/2014 LiaoWei]
        CMD_SPR_ADRATIONPKKING,                     // PK之王崇拜[3/23/2014 LiaoWei]
        CMD_SPR_JINGJI_DETAIL,                      // 竞技场详情 [3/24/2014 JinJieLong]
        CMD_SPR_JINGJI_REQUEST_CHALLENGE,           // 竞技场请求挑战 [3/24/2014 JinJieLong]
        CMD_SPR_JINGJI_CHALLENGE_END,               // 竞技场挑战结束弹出奖励窗口[3/24/2014 JinJieLong]
        CMD_SPR_JINGJI_NOTIFY_START,                // 竞技场通知开始倒计时
        CMD_SPR_JINGJI_CHALLENGEINFO,                // 竞技场战报[3/25/2014 JinJieLong]
        CMD_SPR_JINGJI_RANKING_REWARD,              // 竞技场领取排行榜奖励[3/25/2014 JinJieLong]
        CMD_SPR_JINGJICHANG_REMOVE_CD,              // 竞技场消除挑战CD [3/25/2014 JinJieLong]
        CMD_SPR_JINGJICHANG_GET_BUFF,               // 竞技场领取Buff [3/25/2014 JinJieLong]
        CMD_SPR_JINGJICHANG_JUNXIAN_LEVELUP,        // 竞技场升级军衔 [3/25/2014 JinJieLong]
        CMD_SPR_JINGJICHANG_LEAVE,                  // 离开竞技场消息[3/29/2014 JinJieLong]
        CMD_SPR_CHGFAKEROLELIFEV,                   //假人的血量修改
        CMD_SPR_NEWFAKEROLE,                        //新假人通知
        CMD_SPR_DELFAKEROLE,                        //删除假人
        CMD_SPR_OPENMARKET,                         //打开交易市场
        CMD_SPR_MARKETSALEMONEY,                    //交易市场中上架金币
        CMD_SPR_GETVIPINFO,                         // 玩家请求VIP信息 [3/28/2014 LiaoWei]
        CMD_SPR_GETVIPLEVELAWARD,                   // 玩家领取VIP等级奖励 [3/28/2014 LiaoWei]
        CMD_SPR_VIPLEVELUP,                         // 玩家VIP等级升级 [3/28/2014 LiaoWei]
        CMD_SPR_GETLIXIANBAITANTICKS,               // 获取离线摆摊时长(毫秒)
        CMD_SPR_UPDATELIXIANBAITANTICKS,            // 修改离线摆摊时长(毫秒)
        CMD_SPR_QUERYOPENGRIDTICK,                  // 请求开背包格子时间戳 [4/7/2014 LiaoWei]
        CMD_SPR_QUERYOPENPORTABLEGRIDTICK,          // 请求开随身仓库包裹的时间戳 [4/7/2014 LiaoWei]
        CMD_SPR_STARTMEDITATE,                      // 开始冥想
        CMD_SPR_ZHANMENGBUILDUPLEVEL,               // 战盟建筑升级
        CMD_SPR_ZHANMENGBUILDGETBUFFER,             // 领取战盟建筑的buffer
        CMD_SPR_GETBAITANLOG,                       // 获取摆摊日志
        CMD_SPR_GETPUSHMESSAGEINFO,                 // 客户端把推送信息发给服务器[4/23/2014 LiaoWei]
        CMD_SPR_ACTIVATIONPICTUREJUDGE,             // 激活图鉴 [5/3/2014 LiaoWei] (已废弃，请勿再使用，chenjingui 2015-05-29)
        CMD_SPR_GETNPICTUREJUDGEINFO,               // 取得图鉴信息 [5/3/2014 LiaoWei] (已废弃，请勿再使用，chenjingui 2015-05-29)
        CMD_SPR_MUEQUIPUPGRADE,                     // 装备进阶 [4/30/2014 LiaoWei]
        CMD_SPR_WINGUPSTAR,                         // 翅膀升星 [5/4/2014 liuhuicheng]
        CMD_SPR_WINGUPGRADE,                        // 翅膀进阶 [5/4/2014 liuhuicheng]
        CMD_SPR_WINGOFFON,                          // 翅膀佩戴/卸载 [5/4/2014 liuhuicheng]
        CMD_SPR_CHECK,                              // 与服务器心跳，每两秒发一次，校验是否加速
        CMD_SPR_REFERPICTUREJUDGE,                  // 提交图鉴信息 [5/17/2014 LiaoWei]
        CMD_SPR_GETMOJINGEXCHANGEINFO,              // 客户端请求魔晶钻石兑换信息 [5/21/2014 LiaoWei]
        CMD_SPR_REFRESH_ICON_STATE,                 // 刷新图标状态信息 [5/21/2014 ChenXiaojun]
        CMD_SPR_EQUIPAPPENDINHERIT,                 // 追加传承 [5/24/2014 LiaoWei]
        CMD_SPR_SWEEP_WANMOTA,                      // 扫荡万魔塔 [6/5/2014 ChenXiaojun]
        CMD_SPR_UPDATE_SWEEP_STATE,                 // 更新扫荡状态 [6/5/2014 ChenXiaojun]
        CMD_SPR_GET_WANMOTA_DETAIL,                 // 获取万魔塔信息 [6/5/2014 ChenXiaojun]
        CMD_SPR_GET_SWEEP_REWARD,                   // 领取扫荡奖励 [6/5/2014 ChenXiaojun]
        CMD_SPR_LISTCOPYTEAMS,                      // 搜索副本队伍 [6/5/2014 LiTeng]
        CMD_SPR_COPYTEAM,                           // 副本组队命令 [6/5/2014 LiTeng]
        CMD_SPR_COPYTEAMDATA,                       // 副本队伍信息 [6/5/2014 LiTeng]
        CMD_SPR_COPYTEAMSTATE,                      // 副本队伍成员状态变更 [6/5/2014 LiTeng]
        CMD_SPR_REGEVENTNOTIFY,                     // 注册事件通知(副本组队) [6/5/2014 LiTeng]
        CMD_SPR_LISTCOPYTEAMDATA,                   // 队伍列表中的队伍信息变化 [6/5/2014 LiTeng]
        CMD_SPR_COPYTEAMDAMAGEINFO,                 // 队伍成员伤害信息 [6/6/2014 LiTeng]
        CMD_SPR_BoundMoneyCOPYSCENEPREPAREFIGHT,    // 金币副本准备战斗 -- 客户端显示战斗倒计时 [6/11/2014 LiaoWei]
        CMD_SPR_BoundMoneyCOPYSCENEMONSTERWAVE,     // 金币副本刷怪波数 [6/11/2014 LiaoWei]
        CMD_SPR_GETNEWZONEACTIVEAWARD,              // 获取新区活动奖 [6/10/2014 gwz]
        CMD_SPR_QUERYUPLEVELMADMAN,                 // 冲级狂人 [6/10/2014 gwz]
        CMD_SPR_QUERYNEWZONEACTIVE,                 // 新区活动 [6/10/2014 gwz]
        CMD_SPR_QUERYUPLEVELGIFTINFO,               // 请求等级奖励领取信息 [6/16/2014 LiTeng]
        CMD_SPR_GETUPLEVELGIFTAWARD,                // 领取等级奖励奖励 [6/16/2014 LiTeng]
        CMD_SPR_JINGJI_START_FIGHT,                 // 竞技场战斗开始消息[6/17/2014 ChenXiaojun]
        CMD_SPR_QUERY_REPAYACTIVEINFO,              // 查询回馈活动信息 [6/17/2014 gwz]
        CMD_SPR_GET_REPAYACTIVEAWARD,               // 获取回馈活动奖励 [6/17/2014 gwz]
        CMD_SPR_QUERY_ALLREPAYACTIVEINFO,           // 获取所有回馈信息，充值和消费值 [6/19/2014 gwz]
        CMD_SPR_QUERYACTIVITYSOMEINFO,              // 请求活动(血色城堡、恶魔广场)的一些信息 [7/8/2014 LiaoWei]

        //CMD_SPR_DESTROYGOODS,                     //摧毁物品
        CMD_SPR_PLAYBOSSANIMATION,                  //boss出生前通知播放动画

        CMD_SPR_ENDBOSSANIMATION,                   //客户端通知服务器端开始刷新boss（防止外挂，参数有校验）
        CMD_SPR_QUERY_TODAYCANDOINFO,               //查询今日可做 [7/9/2014 gwz]
        CMD_SPR_QUERY_GETOLDRESINFO,                //查询资源找回信息 [7/9/2014 gwz]
        CMD_SPR_GET_OLDRESOURCE,                    //资源找回，领取资源 [7/9/2014 gwz]
        CMD_SPR_EXTENSIONPROPSHITED,                //拓展属性命中通知
        CMD_SPR_EXEC_WASHPROPS,                     //执行装备洗练
        CMD_SPR_EXEC_WASHPROPSINHERIT,              //执行装备洗练传承
        CMD_SPR_BATTLE_SCORE_LIST,                  //阵营战积分排名信息 [7/23/2014 lt]
        CMD_SPR_STORYCOPYMAPINFO,                   // 剧情副本信息 [7/24/2014 LiaoWei]
        CMD_SPR_GETUSERMAILCOUNT,                   //获取邮件数量 [7/28/2014 lt]
        CMD_SPR_QUERYIMPETRATEINFO,                 // 请求祈福数据 [7/30/2014 LiaoWei]
        CMD_SPR_EXECUTEIMPETRATE,                   // 执行祈福 [7/30/2014 LiaoWei]
        CMD_SPR_OPENMARKET2,                        //打开交易市场 MU交易所功能修改的第二套交易类指令开始
        CMD_SPR_MARKETSALEMONEY2,                   //交易市场中上架金币
        CMD_SPR_SALEGOODS2,
        CMD_SPR_SELFSALEGOODSLIST2,
        CMD_SPR_OTHERSALEGOODSLIST2,
        CMD_SPR_MARKETROLELIST2,
        CMD_SPR_MARKETGOODSLIST2,
        CMD_SPR_MARKETBUYGOODS2,                    //MU交易所功能修改的第二套交易类指令结束
        CMD_SPR_QUERYSTARCONSTELLATIONINFO,         // 请求星座数据 [8/1/2014 LiaoWei]
        CMD_SPR_EXECUTEACTIVATIONSTARCONSTELLATION, // 激活星座 [8/1/2014 LiaoWei]
        CMD_SPR_CHANGEANGLE, // 修改角色的360角度

        CMD_SPR_UPDATESHARESTATE,                   // 处理分享[8/6/2014 gwz]
        CMD_SPR_GETSHAREAWARD,                      // 发放分享奖励[8/6/2014 gwz]
        CMD_SPR_GETSHARESTATE,                      //获取分享状态
        CMD_SPR_BROADSPECIALHINTTEXT,               // 播放特殊的提示信息，例如boss AI描述
        CMD_SPR_MAPAIEVENT,                         // 发送地图事件，例如清除光幕
        CMD_SPR_EXEC_LIANZHI,                       // 执行炼制
        CMD_SPR_QUERY_LIANZHICOUNT,                 // 查询炼制次数
        CMD_SPR_UPGRADE_CHENGJIU,                   // 提升成就 [9/15/2014 ChenXiaojun]
        CMD_SPR_GETFIRSTCHARGEINFO,                 //获得各个充值档首次充值信息
        CMD_SPR_BATCHFETCHMAILGOODS,                //批量提取邮件
        CMD_SPR_PUSH_VERSION,                       //报告客户端代码版本号
        CMD_SPR_NOTIFYTEAMCHGZHANLI,                 //通知组队队员战力变化
        CMD_SPR_NOTIFYSELFCHGZHANLI,                 //通知自己战力变化
        CMD_SPR_NOTIFYOTHERBUFFERDATA,               //通知他人Buff变化变化
        CMD_SPR_NOTIFYSHOWGONGGAO,                   //登录后获取公告显示信息 [10/28/2014 ChenXiaojun]
        CMD_SPR_GETWINGINFO,                        //获取翅膀信息
        CMD_SPR_EMOLAIXIMONSTERINFO,                //恶魔来袭副本怪物数信息 [11/18/2014 LiTeng]
        CMD_SPR_CAIJI_START,                        //开始采集
        CMD_SPR_CAIJI_FINISH,                       //完成采集
        CMD_SPR_CAIJI_LASTNUM,                      //水晶幻境采集剩余次数
        CMD_SPR_QUERYJIERITOTALCONSUME,
        CMD_SPR_EXECUTEJIERITOTALCONSUME,
        CMD_MAP_TELEPORT,                           //地图传送点状态列表（罗兰法阵）
        CMD_SPR_SPECIALMACHINE,                     // 是否特殊的机器
        CMD_SPR_EXTRADATA,                          // 报告特殊信息
        CMD_SPR_SHOWALLICON,                        // 通知客户端showallicon

        // 避免Client和GameServer、GameServer和GameDBServer之间协议号出错的问题
        // 从1.3.0开始，协议号都要自己制定枚举值！！！！！
        CMD_SPR_GE = 699,

        CMD_SPR_CHENGZHAN_JINGJIA = 700,            //罗兰城战竞价进攻方资格
        CMD_SPR_GET_CHENGZHAN_DAILY_AWARD = 701,    //罗兰城战胜利战盟成员领取每日奖励
        CMD_SPR_LUOLANCHENGZHAN = 702,              //罗兰城战进入指令
        CMD_SPR_LUOLANCHENGZHAN_LONGTA_ROLEINFO = 703,              //罗兰城战龙塔内人数信息列表
        CMD_SPR_LUOLANCHENGZHAN_QIZHI_OWNERINFO = 704,              //罗兰城战旗帜拥有者列表
        CMD_SPR_LUOLANCHENGZHAN_LONGTA_OWNERINFO = 705,              //龙塔占有者信息
        CMD_SPR_GET_LUOLANCHENGZHU_INFO = 706,              //获取罗兰城主战盟信息
        CMD_SPR_LUOLANCHENGZHAN_RESULT_INFO = 707,         //罗兰城战结果和奖励信息
        CMD_SPR_GET_LUOLANCHENGZHAN_REQUEST_INFO_LIST = 708, //请求帮会领地信息，主要用来获取竞价信息
        CMD_SPR_SERVERUPDATE_ZHANMENGZIJIN = 709,         //服务器发送战盟当前资金信息
        CMD_SPR_MODIFY_FASHION = 710,                       //使用和卸下时装

        CMD_SPR_GETBANGHUIFUBEN = 711,                    // 取得帮会副本的信息
        CMD_SPR_GETBANGHUIFUBENAWARD = 712,               // 取得帮会副本的奖励

        CMD_SPR_GET_ELEMENTHRT_SLIST = 720,               //申请元素背包数据
        CMD_SPR_GET_ELEMENTHRTS_INFO = 721,               //申请获取猎取元素相关信息,
        CMD_SPR_USE_ELEMENTHRT = 722,                     //佩戴/卸下元素之心
        CMD_SPR_GET_SOMEELEMENTHRTS = 723,                //执行猎取操作
        CMD_SPR_POWER_ELEMENTHRT = 724,                   //强化元素之心
        CMD_SPR_RESET_EHRTSBAG = 725,                     //整理元素背包
        CMD_SPR_GET_USINGELEMENTHRT_SLIST = 726,          //申请元素装备栏数据

        CMD_SPR_HOLD_QINGGONGYAN = 730,                   // 申请举办庆功宴
        CMD_SPR_GET_QINGGONGYAN = 731,                    // 申请庆功宴信息
        CMD_SPR_JOIN_QINGGONGYAN = 732,                   // 申请参加庆功宴
        CMD_SPR_IFQINGGONGYANOPEN = 733,                // 当前是否开启了庆功宴

        CMD_SPR_GETDAMONGOODSLIST = 740,                    // 获取精灵栏精灵

        CMD_SPR_GET_PET_LIST = 750,                       // 申请精灵背包数据
        CMD_SPR_GET_PET_INFO = 751,                       // 申请召唤精灵界面信息
        CMD_SPR_CALL_PET = 752,                           // 请求精灵召唤
        CMD_SPR_MOVE_PET = 753,                           // 从精灵背包中拿出精灵
        CMD_SPR_RESET_PETBAG = 754,                       // 整理精灵背包

        CMD_SPR_FAZHEN_BOSS = 760,                        //罗兰法阵副本boss信息
        CMD_SPR_GET_STORE_BoundToken = 761,                 // 取得仓库金币
        CMD_SPR_GET_STORE_MONEY = 762,                    // 取得仓库绑定金币
        CMD_SPR_STORE_BoundToken_CHANGE = 763,              // 通知客户端仓库金币改变
        CMD_SPR_STORE_MONEY_CHANGE = 764,                 // 通知客户端仓库绑定金币改变

        CMD_SPR_JIERIACT_STATE = 770,                     // 通知客户端节日活动开启或结束
        CMD_SPR_GETJIERIFANBEI_INFO = 771,                // 客户端请求节日活动翻倍的类型

        CMD_SPR_ACHIEVEMENT_RUNE_INFO = 780,              //成就符文——提升信息
        CMD_SPR_ACHIEVEMENT_RUNE_UP = 781,                //成就符文——提升

        CMD_SPR_PRESTIGE_MEDAL_INFO = 782,              //声望勋章——提升信息
        CMD_SPR_PRESTIGE_MEDAL_UP = 783,                //声望勋章——提升

        CMD_SPR_ARTIFACT_UP = 791,                        //神器再造

        CMD_SPR_GET_LINGYU_LIST = 800,                      // 查看翎羽信息
        CMD_SPR_ADVANCE_LINGYU_LEVEL = 801,                 // 提升翎羽等级
        CMD_SPR_ADVANCE_LINGYU_SUIT = 802,                  // 提升翎羽品阶

        CMD_SPR_WING_ZHULING = 810,                         // 请求翅膀注灵
        CMD_SPR_WING_ZHUHUN = 811,                          // 请求翅膀注魂

        CMD_SPR_HYSY_LIANSHA = 818,                         // 幻影寺院连杀信息
        CMD_SPR_HYSY_STOP_LIANSHA = 819,                    // 幻影寺院终结连杀
        CMD_SPR_HYSY_ENQUEUE = 820,                         // 幻影寺院加入自动匹配队列
        CMD_SPR_HYSY_DEQUEUE = 821,                         // 幻影寺院离开自动匹配队列
        CMD_SPR_HYSY_QUEUE_PLAYER_NUM = 822,                // 幻影寺院等待队列玩家数（有效值：0-10）
        CMD_SPR_HYSY_ENTER_NOTIFY = 823,                    // 幻影寺院通知自动匹配成功
        CMD_SPR_HYSY_ENTER_RESPOND = 824,                   // 幻影寺院自动匹配回应，立即开始或暂不进入
        CMD_SPR_HYSY_AWARD = 825,                           // 幻影寺院活动结果奖励信息
        CMD_SPR_HYSY_SCORE_INFO = 826,                      // 幻影寺院通知活动双方得分状态
        CMD_SPR_NOTIFY_TIME_STATE = 827,                    // 通知活动状态和时间(通用)
        CMD_SPR_HYSY_SUCCESS_COUNT = 828,                   // 幻影寺院查询/通知今日获胜次数
        CMD_SPR_HYSY_ADD_SCORE = 829,                       // 幻影寺院通知分数增加

        CMD_SPR_REGION_EVENT = 830,                         // 地图区域事件报告
        CMD_SYNC_TIME_BY_CLIENT = 831,                      // 客户端每2分钟校验时间消息
        CMD_SYNC_TIME_BY_SERVER = 832,                      // 服务器向客户端发送时间消息
        CMD_SYNC_CHANGE_DAY_SERVER = 833,                   // 服务器通知客户端跨天了

        CMD_SPR_GETLUOLANCHENGZHU = 840,                    // 请求罗兰城主基本信息
        CMD_SPR_QUERYADRATIONLANCHENGZHUO = 841,            // 请求罗兰城主崇拜信息
        CMD_SPR_ADRATIONLANCHENGZHU = 842,                  // 罗兰城主崇拜

        CMD_SPR_GET_YUEKA_DATA = 850,                       // 请求玩家月卡信息
        CMD_SPR_GET_YUEKA_AWARD = 851,                      // 请求领取月卡返利奖励

        CMD_SECOND_PASSWORD_CHECK_STATE = 860,              // 登录时请求账号下所有角色的二级密码，客户端一定要先于CMD_ROLE_LIST发送
        CMD_SECOND_PASSWORD_SET = 861,                      // 客户端请求设置二级密码
        CMD_SECOND_PASSWORD_VERIFY = 862,                   // 客户端请求验证二级密码
        CMD_SECOND_PASSWORD_CANCEL = 863,                  // 客户端请求清除二级密码

        CMD_SPR_MARRY_FUBEN = 870,                          //[bing] 情侣副本协议
        CMD_SPR_MARRY_ROSE = 871,                           //[bing] 情侣献花
        CMD_SPR_MARRY_RING = 872,                           //[bing] 婚戒替换
        CMD_SPR_MARRY_MESSAGE = 873,                        //[bing] 爱情宣言更新
        CMD_SPR_MARRY_PARTY_QUERY = 880,                    // 获取婚宴列表
        CMD_SPR_MARRY_PARTY_CREATE = 881,                   // 举行婚宴
        CMD_SPR_MARRY_PARTY_CANCEL = 882,                   // 取消婚宴
        CMD_SPR_MARRY_PARTY_JOIN = 883,                     // 參加婚宴
        CMD_SPR_MARRY_PARTY_JOIN_LIST = 884,                // 已经參加婚宴次数列表

        CMD_SPR_MARRY_INIT = 890,                           //求婚发起
        CMD_SPR_MARRY_REPLY = 891,                          //求婚回复
        CMD_SPR_MARRY_DIVORCE = 892,                        //离婚或离婚回复
        CMD_SPR_MARRY_AUTO_REJECT = 893,                    //自动拒绝求婚
        CMD_SPR_MARRY_NOTIFY = 894,                         //回复求婚离婚通知对方
        CMD_SPR_MARRY_UPDATE = 895,                         //婚姻状态更新
        CMD_SPR_MARRY_SPOUSE_DATA = 896,                    //情侣婚姻数据发送给客户端

        //CMD_SPR_RETURN_RECELL_INFO = 900,                   // (当前推荐人信息——获取信息)
        //CMD_SPR_RETURN_RECELL_SET = 901,                    // (当前推荐人信息——设置)
        //CMD_SPR_RETURN_AWARD_INFO = 902,                    // (回归礼包——获取信息)
        //CMD_SPR_RETURN_AWARD_SET = 903,                     // (回归礼包——领取)
        //CMD_SPR_RETURN_CHECK_INFO = 904,                    // (签到信息——获取信息)
        //CMD_SPR_RETURN_CHECK_SET = 905,                     // (签到信息——签到)
        //CMD_SPR_RETURN_RECELL_AWARD_INFO = 906,             // (召回奖励——获取信息)
        //CMD_SPR_RETURN_RECELL_AWARD_SET = 907,              // (召回奖励——设置)
        ////CMD_SPR_RETURN_RECELL_EXTRA_AWARD = 908,           // (召回奖励——额外奖励领取)

        CMD_SPR_RETURN_DATA = 900,                  // (获取召回活动信息)
        CMD_SPR_RETURN_CHECK = 901,                 // (校验回归资格)
        CMD_SPR_RETURN_AWARD = 902,                 // (领取奖励)

        CMD_SPR_QUERY_JIERI_GIVE_INFO = 920,                // 查询节日赠送信息
        CMD_SPR_JIERI_GIVE_TO_OTHER = 921,                  // 节日赠送礼物给别人
        CMD_SPR_GET_JIERI_GIVE_AWARD = 922,                 // 领取节日赠送奖励

        CMD_SPR_QUERY_JIERI_GIVE_KING_INFO = 923,           // 查询节日赠送王信息
        CMD_SPR_GET_JIERI_GIVE_KING_AWARD = 924,            //领取节日赠送王奖励

        CMD_SPR_QUERY_JIERI_RECV_KING_INFO = 925,           // 查询节日收取王信息
        CMD_SPR_GET_JIERI_RECV_KING_AWARD = 926,            //领取节日赠送王奖励
        CMD_DB_EXECUXJIERIFANLI = 927,                      //[bing] 节日返利活动

        CMD_SPR_QUERY_GUARD_POINT_RECOVER = 930,    // 客户端查询守护点回收信息
        CMD_SPR_GUARD_POINT_RECOVER = 931,  // 客户端回收守护点
        CMD_SPR_QUERY_GUARD_STATUE_INFO = 932, // 客户端查询守护雕像信息
        CMD_SPR_GUARD_STATUE_LEVEL_UP = 933, // 客户端升级守护雕像信息
        CMD_SPR_GUARD_STATUE_SUIT_UP = 934, // 客户端升阶守护雕像信息
        CMD_SPR_MOD_GUARD_SOUL_EQUIP = 935, // 客户端穿戴卸下守护之灵

        CMD_SPR_QUERY_LIANXU_CHARGE_INFO = 940, // 查询连续充值活动信息
        CMD_SPR_GET_LIANXU_CHARGE_AWARD = 941, // 领取连续充值活动奖励

        CMD_SPR_QUERY_JIERI_RECV_INFO = 944,    //查询节日收礼活动信息
        CMD_SPR_GET_JIERI_RECV_AWARD = 945,   //领取节日收礼奖励
        CMD_SPR_GET_FASHION_SLIST = 946,        //获取时装列表  panghui  add

        CMD_SPR_TIANTI_JOIN = 950,  //开始匹配
        CMD_SPR_TIANTI_QUIT = 951, //取消匹配
        CMD_SPR_TIANTI_ENTER = 952, //匹配成功，通知进入活动
        CMD_SPR_TIANTI_AWARD = 953, //结果和奖励信息
        CMD_SPR_TIANTI_DAY_DATA = 954, //角色天梯数据和日排行
        CMD_SPR_TIANTI_MONTH_PAIHANG = 955, //获取月段位排行榜.
        CMD_SPR_TIANTI_GET_PAIMING_AWARDS = 956, //领取上月段位排行奖励
        CMD_SPR_ROLE_ATTRIBUTE_VALUE = 968, //通知角色货币变化(当前值和增减量)
        CMD_SPR_TIANTI_GET_LOG = 969, //获取战报列表
        CMD_SPR_EFFECT_HIDE_FLAGS = 970, //(客户端)效果屏蔽选项
        CMD_SPR_LOGIN_WAITING_INFO = 971,// 通知客户端排队信息

        CMD_SPR_MERLIN_QUERY = 981,    // 客户端请求梅林魔法书数据 [XSea 2015/6/23]
        CMD_SPR_MERLIN_STAR_UP = 982, // 客户端请求梅林魔法书升星 [XSea 2015/6/23]
        CMD_SPR_MERLIN_LEVEL_UP = 983, // 客户端请求梅林魔法书升阶 [XSea 2015/6/24]
        CMD_SPR_MERLIN_SECRET_ATTR_UPDATE = 984,    // 客户端请求擦拭梅林魔法书秘语 [XSea 2015/6/25]
        CMD_SPR_MERLIN_SECRET_ATTR_REPLACE = 985,    // 客户端请求替换梅林魔法书秘语 [XSea 2015/6/25]
        CMD_SPR_MERLIN_SECRET_ATTR_NOT_REPLACE = 986,    // 客户端请求放弃替换梅林魔法书秘语 [XSea 2015/6/25]

        // 荧光宝石 991-999 [XSea 2015/8/7]
        CMD_SPR_FLUORESCENT_GEM_RESET_BAG = 991, // 客户端请求整理荧光宝石背包 [XSea 2015/8/7]

        CMD_SPR_FLUORESCENT_GEM_EQUIP = 992, // 客户端请求装备荧光宝石
        CMD_SPR_FLUORESCENT_GEM_UN_EQUIP = 993, // 客户端请求卸下荧光宝石
        CMD_SPR_FLUORESCENT_GEM_LEVEL_UP = 994, // 客户端请求升级荧光宝石
        CMD_SPR_FLUORESCENT_GEM_DIG = 995, // 客户端请求挖掘荧光宝石
        CMD_SPR_FLUORESCENT_GEM_RESOLVE = 996, // 客户端请求分解荧光宝石
        CMD_SPR_FLUORESCENT_GEM_EQUIP_CHANGES = 997, // 通知客户端荧光宝石装备栏变动

        CMD_SPR_TALENT_OTHER = 999,         //获取天赋数据——他人
        CMD_SPR_TALENT_GET_DATA = 1000,     //获取天赋数据
        CMD_SPR_TALENT_ADD_EXP = 1001,      //注入经验
        CMD_SPR_TALENT_WASH = 1002,         //洗点
        CMD_SPR_TALENT_ADD_EFFECT = 1003,   //效果升级

        CMD_SPR_WARN_INFO = 1004, //警告信息

        CMD_SPR_ELEMENT_WAR_JOIN = 1010, // 元素试炼——开始匹配
        CMD_SPR_ELEMENT_WAR_QUIT = 1011, // 元素试炼——取消匹配
        CMD_SPR_ELEMENT_WAR_ENTER = 1012, // 元素试炼——匹配成功进入
        CMD_SPR_ELEMENT_WAR_PLAYER_NUM = 1013, // 元素试炼——匹配人数变化
        CMD_SPR_ELEMENT_WAR_CANCEL = 1016, // 元素试炼——副本取消

        CMD_SPR_ELEMENT_WAR_SCORE_INFO = 1014, // 元素试炼——得分信息
        CMD_SPR_ELEMENT_WAR_AWARD = 1015, // 元素试炼——领奖信息

        CMD_SPR_SPREAD_SIGN = 1017,//成为推广员
        CMD_SPR_SPREAD_AWARD = 1018,//领取奖励
        CMD_SPR_SPREAD_VERIFY_CODE = 1019,//填写推荐人
        CMD_SPR_SPREAD_TEL_CODE_GET = 1020,//获取验证码
        CMD_SPR_SPREAD_TEL_CODE_VERIFY = 1021,//确认验证码
        CMD_SPR_SPREAD_INFO = 1022,//推广信息

        CMD_SPR_COPY_WOLF_SCORE_INFO = 1025,//狼魂要塞——得分信息
        CMD_SPR_COPY_WOLF_AWARD = 1026,//狼魂要塞——领奖信息

        CMD_SPR_TODAY_DATA = 1030,             // 每日专项——获取数据
        CMD_SPR_TODAY_AWARD = 1031,             // 每日专项——领取奖励

        CMD_SPR_FUND_INFO = 1032,             // 基金——信息
        CMD_SPR_FUND_BUY = 1033,             // 基金——购买
        CMD_SPR_FUND_AWARD = 1034,             // 基金——领取奖励

        CMD_SPR_UNION_PALACE_DATA = 1035,
        CMD_SPR_UNION_PALACE_UP = 1036,

        CMD_SPR_PET_SKILL_UP = 1037,
        CMD_SPR_PET_SKILL_AWAKE = 1038,
        CMD_SPR_PET_SKILL_AWAKE_COST = 1039,

        CMD_SPR_ACTIVATE_INFO = 1040,
        CMD_SPR_ACTIVATE_AWARD = 1041,

        CMD_SPR_UNION_ALLY_REQUEST = 1042,
        CMD_SPR_UNION_ALLY_CANCEL = 1043,
        CMD_SPR_UNION_ALLY_REMOVE = 1044,
        CMD_SPR_UNION_ALLY_AGREE = 1045,
        CMD_SPR_UNION_ALLY_DATA = 1046,
        CMD_SPR_UNION_ALLY_LOG = 1047,
        CMD_SPR_UNION_ALLY_NUM = 1048,

        #region 1100-1199-lt

        CMD_SPR_YONGZHEZHANCHANG_JOIN = 1100, //勇者战场报名
        CMD_SPR_YONGZHEZHANCHANG_ENTER = 1101, //勇者战场进入
        CMD_SPR_YONGZHEZHANCHANG_AWARD = 1102, //勇者战场结束及奖励(通知客户端可以领取)
        CMD_SPR_YONGZHEZHANCHANG_STATE = 1103, //勇者战场报名状态
        CMD_SPR_YONGZHEZHANCHANG_SIDE_SCORE = 1104, //勇者战场分数信息
        CMD_SPR_YONGZHEZHANCHANG_SELF_SCORE = 1105, //勇者战场自己分数增加信息
        CMD_SPR_YONGZHEZHANCHANG_LIANSHA = 1106, //勇者战场连杀信息
        CMD_SPR_YONGZHEZHANCHANG_STOP_LIANSHA = 1107, //勇者战场终结连杀
        CMD_SPR_YONGZHEZHANCHANG_AWARD_GET = 1108, //领取奖励

        CMD_SPR_KUAFU_BOSS_JOIN = 1120, //跨服BOSS报名
        CMD_SPR_KUAFU_BOSS_ENTER = 1121, //跨服BOSS进入
        CMD_SPR_KUAFU_BOSS_DATA = 1122, //跨服BOSS场景数据
        CMD_SPR_KUAFU_BOSS_STATE = 1123, //跨服BOSS报名状态

        CMD_SPR_KUAFU_MAP_INFO = 1140, //Cross-server mainline map line status information
        CMD_SPR_KUAFU_MAP_ENTER = 1141, //Cross-server mainline map entry

        CMD_SPR_LANGHUNLINGYU_LONGTA_ROLEINFO = 1150,    //龙塔内人数信息列表
        CMD_SPR_LANGHUNLINGYU_QIZHI_OWNERINFO = 1151,    //旗帜拥有者列表
        CMD_SPR_LANGHUNLINGYU_LONGTA_OWNERINFO = 1152,   //龙塔占有者信息
        CMD_SPR_LANGHUNLINGYU_JOIN = 1153,               //报名
        CMD_SPR_LANGHUNLINGYU_DATA = 1154,               //请求玩家自己的圣域争霸数据
        CMD_SPR_LANGHUNLINGYU_CITY_DATA = 1155,          //请求城池占领、进攻信息数据
        CMD_SPR_LANGHUNLINGYU_WORLD_DATA = 1156,         //请求玩家自己的圣域争霸数据
        CMD_SPR_LANGHUNLINGYU_ENTER = 1157,              //进入城池
        CMD_SPR_LANGHUNLINGYU_GET_DAY_AWARD = 1158,      //获取每日奖励
        CMD_SPR_LANGHUNLINGYU_AWARD = 1159,              //服务器推送战斗结果和奖励信息
        CMD_SPR_LANGHUNLINGYU_ADMIRE_DATA = 1160,        //获取当前圣域城主膜拜数据
        CMD_SPR_LANGHUNLINGYU_ADMIRE_HIST = 1161,        //获取历届圣域城主膜拜信息
        CMD_SPR_LANGHUNLINGYU_ADMIRE = 1162,             //膜拜

        CMD_SPR_KINGOFBATTLE_JOIN = 1180, //王者战场报名
        CMD_SPR_KINGOFBATTLE_ENTER = 1181, //王者战场进入
        CMD_SPR_KINGOFBATTLE_AWARD = 1182, //王者战场结束及奖励(通知客户端可以领取)
        CMD_SPR_KINGOFBATTLE_STATE = 1183, //王者战场报名状态
        CMD_SPR_KINGOFBATTLE_SIDE_SCORE = 1184, //王者战场分数信息
        CMD_SPR_KINGOFBATTLE_SELF_SCORE = 1185, //王者战场自己分数增加信息
        CMD_SPR_KINGOFBATTLE_LIANSHA = 1186, //王者战场连杀信息
        CMD_SPR_KINGOFBATTLE_STOP_LIANSHA = 1187, //王者战场终结连杀
        CMD_SPR_KINGOFBATTLE_AWARD_GET = 1188, //领取奖励
        CMD_SPR_KINGOFBATTLE_TELEPORT = 1189, // 王者战场传送门数据

        CMD_SPR_KINGOFBATTLE_MALL_DATA = 1190, // 获取王者商店数据
        CMD_SPR_KINGOFBATTLE_MALL_BUY = 1191, // 王者商店购买
        CMD_SPR_KINGOFBATTLE_MALL_REFRESH = 1192, // 王者商店刷新

        #endregion 1100-1199-lt

        CMD_SPR_HOLYITEM_DATA = 1200,                     //返回圣物全部数据
        CMD_SPR_HOLYITEM_PART_DATA = 1201,                //返回某个圣物数据

        CMD_ID_PLACE_HOLDER_BY_CHENJG_START = 1300, // 1300---1399 被我预定了 chenjingui

        CMD_SPR_QUERY_JIERI_PLAT_CHARGE_KING = 1300, // 查询节日平台充值王活动

        CMD_SPR_MORI_JOIN = 1301, //开始匹配末日审判
        CMD_SPR_MORI_QUIT = 1302, //取消匹配
        CMD_MORI_NTF_ROLE_COUNT = 1303, // 服务器通知客户端队伍人数
        CMD_MORI_NTF_ENTER = 1304, // 服务器通知客户端可以进入末日审判了
        CMD_NTF_MORI_MONSTER_EVENT = 1305, //服务器通知客户端boss事件(出生，死亡)
        CMD_NTF_MORI_COPY_CANCEL = 1306, // 副本取消

        CMD_SPR_SEVEN_DAY_ACT_QUERY = 1310, // 查询七日活动信息
        CMD_SPR_SEVEN_DAY_ACT_GET_AWARD = 1311, // 领取七日活动奖励
        CMD_SPR_SEVEN_DAY_ACT_QIANG_GOU = 1312, // 抢购物品

        CMD_NTF_BANGHUI_CHANGE_NAME = 1315, // 通知帮会改名

        CMD_SPR_SOUL_STONE_QUERY_GET = 1320, //查询·魂石获取·额外功能
        CMD_SPR_SOUL_STONE_GET = 1321, // 获取魂石
        CMD_SPR_SOUL_STONE_LVL_UP = 1322, // 魂石升级
        CMD_SPR_SOUL_STONE_MOD_EQUIP = 1323, // 穿戴或卸下
        CMD_SPR_SOUL_STONE_RESET_BAG = 1324, // 整理魂石背包

        CMD_SPR_SET_FUNCTION_OPEN = 1330, // 设置二态功能的开启或关闭

        CMD_SPR_JINGJICHANG_GET_ROLE_LOOKS = 1340, // 竞技场排行界面中查看角色外貌
        CMD_SPR_PKKING_GET_ROLE_LOOKS = 1341,      // 查看PKKing角色
        CMD_SPR_LUOLANKING_GET_ROLE_LOOKS = 1342,  // 查看罗兰城主

        CMD_SPR_ZHENGBA_GET_MAIN_INFO = 1350,       // 众神争霸 --- 获取16强信息
        CMD_SPR_ZHENGBA_GET_ALL_PK_LOG = 1351,      // 众神争霸 --- 查看全部战报
        CMD_SPR_ZHENGBA_GET_ALL_PK_STATE = 1352,    // 众神争霸 --- 查看全部参赛状态
        CMD_SPR_ZHENGBA_GET_16_PK_STATE = 1353,     // 众神争霸 --- 获取16强中的两两pk状态
        CMD_SPR_ZHENGBA_SUPPORT = 1354,             // 众神争霸 --- 支持
        CMD_SPR_ZHENGBA_YA_ZHU = 1355,              // 众神争霸 --- 押注, 已废弃
        CMD_NTF_ZHENGBA_CAN_ENTER = 1356,           // 众神争霸 --- 服务器通知客户端可以进入
        CMD_SPR_ZHENGBA_ENTER = 1357,               // 众神争霸 --- 进入
        CMD_NTF_ZHENGBA_PK_RESULT = 1358,           // 众神争霸 --- pk结果
        CMD_SPR_ZHENGBA_GET_MINI_STATE = 1359,      // 众神争霸 --- 获取活动mini进度
        CMD_SPR_ZHENGBA_QUERY_JOIN_HINT = 1360,     // 众神争霸 --- 查询角色是否提示本月参与

        CMD_COUPLE_ARENA_GET_MAIN_DATA = 1370,      // 夫妻竞技场 --- 查询主界面信息
        CMD_COUPLE_ARENA_GET_ZHAN_BAO = 1371,       // 夫妻竞技场 --- 获取战报
        CMD_COUPLE_ARENA_GET_PAI_HANG = 1372,       // 夫妻竞技场 --- 获取排行
        CMD_COUPLE_ARENA_SET_READY = 1373,          // 夫妻竞技场 --- 设置准备状态
        CMD_COUPLE_ARENA_SINGLE_JOIN = 1374,        // 夫妻竞技场 --- 单人匹配
        CMD_COUPLE_ARENA_QUIT = 1375,               // 夫妻竞技场 --- 取消匹配
        CMD_COUPLE_ARENA_NTF_CAN_ENTER = 1376,      // 夫妻竞技场 --- 服务器通知客户端匹配成功，可以进入
        CMD_COUPLE_ARENA_ENTER = 1377,              // 夫妻竞技场 --- 进入
        CMD_COUPLE_ARENA_NTF_PK_RESULT = 1378,      // 夫妻竞技场 --- 服务器通知客户端战斗结果
        CMD_COUPLE_ARENA_NTF_COUPLE_STATE = 1379,   // 夫妻竞技场 --- 夫妻状态
        CMD_COUPLE_ARENA_REG_STATE_WATCHER = 1380,  // 夫妻竞技场 --- 注册关注夫妻状态变更
        CMD_COUPLE_ARENA_NTF_BUFF_HOLDER = 1381,    // 夫妻竞技场 --- 通知客户端buff持有信息
        CMD_COUPLE_ARENA_DB_SAVE_ZHAN_BAO = 1382,   // 夫妻竞技场 --- db存储战报
        CMD_COUPLE_ARENA_DB_CLR_ZHAN_BAO = 1383,    // 夫妻竞技场 --- 清除战报

        CMD_COUPLE_WISH_GET_MAIN_DATA = 1390,       // 情侣祝福榜 --- 查看主界面信息
        CMD_COUPLE_WISH_GET_WISH_RECORD = 1391,     // 情侣祝福榜 --- 查看祝福记录
        CMD_COUPLE_WISH_WISH_OTHER_ROLE = 1392,     // 情侣祝福榜 --- 祝福他人
        CMD_COUPLE_WISH_NTF_WISH_EFFECT = 1393,     // 情侣祝福榜 --- 显示祝福特效
        CMD_COUPLE_WISH_GET_ADMIRE_DATA = 1394,     // 情侣祝福榜 --- 获取膜拜数据
        CMD_COUPLE_WISH_ADMIRE_STATUE = 1395,       // 情侣祝福榜 --- 膜拜雕像
        CMD_COUPLE_WISH_GET_PARTY_DATA = 1396,      // 情侣祝福榜 --- 查看宴会信息
        CMD_COUPLE_WISH_JOIN_PARTY = 1397,          // 情侣祝福榜 --- 参加宴会

        CMD_ID_PLACE_HOLDER_BY_CHENJG_END = 1399, // 1300---1399 被我预定了 chenjingui

        #region 1400-1499-lt

        CMD_SPR_QUERY_OTHER_SALE_PRICE = 1400, //交易所,根据goodsId查询其他人卖出物品的价格

        #endregion 1400-1499-lt

        CMD_SPR_GETINPUT_POINTS_EXCHGINFO = 1500,        //获得充值积分相关数据
        CMD_SPR_GETWEEKEND_INPUT_DATA = 1501,         //获得周末充值相关数据
        CMD_SPR_SYNCINPUT_POINTS_ONLY = 1502,         //同步充值点积分

        CMD_SPR_SPECIALACTIVITY_GETXMLDATA = 1510,	    // 获取专属活动xml配置文件
        CMD_SPR_SPECIALACTIVITY_QUERY = 1511,			// 查询专属活动数据
        CMD_SPR_SPECIALACTIVITY_FETCHAWARD = 1512,	    // 获取专属活动奖励

        CMD_SPR_BUILD_GET_LIST = 1550,                  // 获得领地所有信息
        CMD_SPR_BUILD_EXCUTE,	                        // 执行开发任务
        CMD_SPR_BUILD_FINISH,                           // 一键完成开发任务
        CMD_SPR_BUILD_REFRESH,                          // 刷新开发任务
        CMD_SPR_BUILD_GET_ALLLEVEL_AWARD,               // 获取总等级奖励
        CMD_SPR_BUILD_GET_AWARD,                        // 获取开发奖励
        CMD_SPR_BUILD_OPEN_QUEUE,                       // 开启收费开发队列
        CMD_SPR_BUILD_GET_QUEUE,                        // 获得开发队列数据
        CMD_SPR_BUILD_GET_STATE,                        // 获得建筑物状态数据
        CMD_SPR_BUILD_GET_ALLLEVEL_AWARD_STATE,         // 同步总等级奖励领取状态
        CMD_SPR_BUILD_SYNC_SINGLE,                      // 同步单个建筑物数据

        CMD_SPR_ONEPIECE_GET_INFO = 1600,               // c2s 获取藏宝秘境相关信息
        CMD_SPR_ONEPIECE_ROLL,                          // c2s 扔骰子
        CMD_SPR_ONEPIECE_TRIGGER_EVENT,                 // c2s 客户端触发事件
        CMD_SPR_ONEPIECE_SYNC_EVENT,                    // s2c 服务器向客户端同步当前事件信息
        CMD_SPR_ONEPIECE_MOVE,                          // c2s 请求移动
        CMD_SPR_ONEPIECE_ROLL_MIRACLE,                  // c2s 扔奇迹骰子
        CMD_SPR_ONEPIECE_DICE_BUY,                      // c2s 购买骰子
        CMD_SPR_ONEPIECE_SYNC_DICE,                     // s2c 骰子数同步

        CMD_SPR_FASHION_FORGE = 1610,                   // 时装强化
        CMD_SPR_FASHION_ACTIVE = 1611,                  // 时装激活

        CMD_SPR_VIDEO_OPEN = 1700,                      //亲加视频 打开视频请求
        CMD_SPR_TAROT_UPORINIT = 1701,                  //塔罗牌升级或激活
        CMD_SPR_SET_TAROTPOS = 1702,                     //设置塔罗牌上阵数据
        CMD_SPR_USE_TAROTKINGPRIVILEGE = 1703,                //使用塔罗牌国王特权
        CMD_SPR_TAROT_DATA = 1704,                     //返回塔罗牌全部数据

        CMD_DB_START_CMD = 10000,//数据库命令
        CMD_DB_UPDATE_POS, CMD_DB_UPDATE_EXPLEVEL, CMD_DB_UPDATE_ROLE_AVARTA,
        CMD_DB_UPDATEMoney_CMD, CMD_DB_ADDGOODS_CMD, CMD_DB_UPDATEGOODS_CMD,
        CMD_DB_UPDATETASK_CMD, CMD_DB_UPDATEPKMODE_CMD, CMD_DB_UPDATEPKVAL_CMD, CMD_DB_UPDATEKEYS,
        CMD_DB_UPDATEToken_CMD, CMD_DB_UPDATEBoundToken_CMD, CMD_DB_MOVEGOODS_CMD, CMD_DB_UPDATE_LEFTFIGHTSECS,
        CMD_DB_ROLE_ONLINE, CMD_DB_ROLE_HEART, CMD_DB_ROLE_OFFLINE, CMD_DB_GET_CHATMSGLIST,
        CMD_DB_HORSEON, CMD_DB_HORSEOFF, CMD_DB_PETOUT, CMD_DB_PETIN, CMD_DB_ADDDJPOINT, CMD_DB_UPJINGMAI_LEVEL,
        CMD_DB_REGUSERID, CMD_DB_BANROLENAME, CMD_DB_BANROLECHAT, CMD_DB_GETBANROLECATDICT, CMD_DB_ADDBULLMSG, CMD_DB_REMOVEBULLMSG,
        CMD_DB_GETBULLMSGDICT, CMD_DB_UPDATEONLINETIME, CMD_DB_GAMECONFIGDICT, CMD_DB_GAMECONIFGITEM, CMD_DB_RESETBIGUAN,
        CMD_DB_ADDSKILL, CMD_DB_UPSKILLINFO, CMD_DB_UPDATEJINGMAIEXP, CMD_DB_UPDATEDEFSKILLID, CMD_DB_UPDATEAUTODRINK,
        CMD_DB_UPDATEDAILYTASKDATA, CMD_DB_UPDATEDAILYJINGMAI, CMD_DB_UPDATENUMSKILLID, CMD_DB_UPDATEPBINFO, CMD_DB_UPDATHUODONGINFO,
        CMD_DB_SUBCHONGZHIJIFEN, CMD_DB_USELIPINMA, CMD_DB_UPDATEFUBENDATA, CMD_DB_GETFUBENSEQID, CMD_DB_UPDATEROLEDAILYDATA,
        CMD_DB_UPDATEBUFFERITEM, CMD_DB_UNDELROLENAME, CMD_DB_ADDFUBENHISTDATA, CMD_DB_UPDATELIANZHAN, CMD_DB_UPDATEKILLBOSS, CMD_DB_UPDATEROLESTAT,
        CMD_DB_UPDATEYABIAODATA, CMD_DB_UPDATEYABIAODATASTATE, CMD_DB_UPDATEBATTLENAME, CMD_DB_ADDMALLBUYITEM, CMD_DB_GETLIPINMAINFO,
        CMD_DB_UPDATECZTASKID, CMD_DB_GETTOTALONLINENUM, CMD_DB_UPDATEBATTLENUM, CMD_DB_UPDATEHEROINDEX, CMD_DB_FORCERELOADPAIHANG, CMD_DB_ADDYINPIAOBUYITEM,
        CMD_DB_DELROLENAME, CMD_SPR_QUERYUMBYNAME, CMD_DB_QUERYBHMGRLIST, CMD_DB_UPDATEBANGGONG_CMD, CMD_DB_UPDATEBHTONGQIAN_CMD, CMD_DB_GETBHJUNQILIST,
        CMD_DB_GETBHLINGDIDICT, CMD_DB_UPDATELINGDIFORBH, CMD_DB_GETLEADERROLEIDBYBHID, CMD_DB_ADDBHTONGQIAN_CMD, CMD_DB_ADDQIZHENGEBUYITEM,
        CMD_DB_UPDATEJIEBIAOINFO, CMD_DB_ADDREFRESHQIZHENREC, CMD_DB_CLEARCACHINGROLEIDATA, CMD_DB_ADDMONEYWARNING, CMD_DB_QUERYCHONGZHIMONEY,
        CMD_DB_ADDBoundTokenBUYITEM, CMD_DB_ADDBANGGONGBUYITEM, CMD_DB_SENDUSERMAIL, CMD_DB_GETUSERMAILDATA, CMD_DB_FINDROLEID_BYROLENAME, CMD_DB_QUERYLIMITGOODSUSEDNUM,
        CMD_DB_UPDATELIMITGOODSUSEDNUM, CMD_DB_UPDATEDAILYVIPDATA, CMD_DB_UPDATEDAILYYANGGONGBKJIFENDATA, CMD_DB_UPDATESINGLETIMEAWARDFLAG, CMD_DB_ADDSHENGXIAOGUESSHIST,
        CMD_DB_UPDATEUSERBoundMoney_CMD, CMD_DB_ADDBoundMoneyBUYITEM, CMD_DB_UPDATEROLEBAGNUM, CMD_DB_SETLINGDIWARREQUEST, CMD_DB_UPDATEGOODSLIMIT, CMD_DB_UPDATEROLEPARAM,
        CMD_DB_ADDQIANGGOUBUYITEM, CMD_DB_ADDQIANGGOUITEM, CMD_DB_QUERYCURRENTQIANGGOUITEM, CMD_DB_QUERYQIANGGOUBUYITEMS, CMD_DB_UPDATEQIANGGOUTIMEOVER,
        CMD_DB_GETBANGHUIMINIDATA, CMD_DB_ADDBUYITEMFROMNPC, CMD_DB_ADDZAJINDANHISTORY, CMD_DB_QUERYQIANGGOUBUYITEMINFO, CMD_DB_QUERYFIRSTCHONGZHIBYUSERID,
        CMD_DB_QUERYKAIFUONLINEAWARDROLEID, CMD_DB_ADDKAIFUONLINEAWARD, CMD_DB_ADDGIVETokenITEM, CMD_DB_QUERYKAIFUONLINEAWARDLIST,
        CMD_DB_ADDEXCHANGE1ITEM, CMD_DB_ADDEXCHANGE2ITEM, CMD_DB_ADDEXCHANGE3ITEM, CMD_DB_ADDFALLGOODSITEM, CMD_DB_UPDATEROLEPROPS, CMD_DB_QUERYTODAYCHONGZHIMONEY,
        CMD_DB_QUERYDAYCHONGZHIBYUSERID, CMD_DB_CLEARALLCACHINGROLEDATA,
        CMD_DB_QUERYXINGYUNORYUEDUCHOUJIANGINFO,    // GS-DB 询问幸运或月度抽奖信息 [7/17/2013 LiaoWei]
        CMD_DB_EXECUXINGYUNORYUEDUCHOUJIANGINFO,    // GS-DB 更新幸运或月度抽奖信息 [7/17/2013 LiaoWei]
        CMD_DB_ADDYUEDUCHOUJIANGHISTORY,            // GS-DB 增加月度抽奖历史信息 [7/23/2013 LiaoWei]
        CMD_DB_EXECUTECHANGEOCCUPATION,             // GS-DB 转职操作  [9/28/2013 LiaoWei]
        CMD_DB_QUERYBLOODCASTLEENTERCOUNT,          // GS-DB 请求血色堡垒进入次数[11/6/2013 LiaoWei]
        CMD_DB_UPDATEBLOODCASTLEENTERCOUNT,         // GS-DB 更新血色堡垒进入次数[11/6/2013 LiaoWei]
        CMD_DB_QUERYFUBENHISINFO,                   // GS-DB 请求副本历史信息 [11/16/2013 LiaoWei]
        CMD_DB_CLEANDATAWHENFRESHPLAYERLOGOUT,      // GS-DB 在新手阶段掉线处理 [12/2/2013 LiaoWei]
        CMD_DB_FINISHFRESHPLAYERSTATUS,             // GS-DB 结束新手阶段 [12/2/2013 LiaoWei]
        CMD_DB_EXECUTECHANGETASKSTARLEVEL,          // GS-DB 改变任务星级等级[12/3/2013 LiaoWei]
        CMD_DB_EXECUTEUPDATEROLESOMEINFO,           // GS-DB 更新角色的一些信息 [12/17/2013 LiaoWei]
        CMD_DB_QUERYDAYACTIVITYTOTALPOINT,          // GS-DB 请求日常活动最高积分信息 [12/24/2013 LiaoWei]
        CMD_DB_QUERYDAYACTIVITYSELFPOINT,           // GS-DB 请求自己的日常活动积分信息 [12/24/2013 LiaoWei]
        CMD_DB_UPDATEDAYACTIVITYSELFPOINT,          // GS-DB 更新自己的日常活动积分信息 [12/24/2013 LiaoWei]
        CMD_DB_QUERYEVERYDAYONLINEAWARDGIFTINFO,    // GS-DB 请求每日在线奖励信息 [1/12/2014 LiaoWei]
        CMD_DB_ADD_ZHANMENGSHIJIAN,                 // GS-DB 请求请求添加战盟事件 [3/14/2014 JinJieLong]
        CMD_DB_ZHANMENGSHIJIAN_DETAIL,              // GS-DB 请求战盟事件详情 [3/14/2014 JinJieLong]
        CMD_DB_JINGJICHANG_GET_DATA,                // GS-DB 请求获取玩家竞技场数据 [3/21/2014 JinJieLong]
        CMD_DB_JINGJICHANG_GET_CHALLENGE_DATA,      // GS-DB 请求获取竞技场被挑战者mini数据 [3/21/2014 JinJieLong]
        CMD_DB_JINGJICHANG_CREATE_DATA,             // GS-DB 请求创建竞技场数据 [3/22/2014 JinJieLong]
        CMD_DB_JINGJICHANG_REQUEST_CHALLENGE,       // GS-DB 竞技场请求挑战 [3/22/2014 JinJieLong]
        CMD_DB_JINGJICHANG_CHALLENGE_END,           // GS-DB 竞技场挑战结束 [3/22/2014 JinJieLong]
        CMD_DB_JINGJICHANG_SAVE_DATA,               // GS-DB 保存竞技场数据 [3/22/2014 JinJieLong]
        CMD_DB_JINGJICHANG_ZHANBAO_DATA,            // GS-DB 获取竞技场战报数据 [3/22/2014 JinJieLong]
        CMD_DB_JINGJICHANG_REMOVE_CD,               // GS-DB 消除挑战CD [3/25/2014 JinJieLong]
        CMD_DB_JINGJICHANG_GET_RANKING_AND_NEXTREWARDTIME,// GS-DB 获取排名和下次领取奖励时间 [3/25/2014 JinJieLong]
        CMD_DB_JINGJICHANG_UPDATE_NEXTREWARDTIME,   // GS-DB  更新下次领取竞技场排行榜奖励时间 [3/25/2014 JinJieLong]
        CMD_DB_ADD_BAITANLOG,                       // GS-DB 请求请求添加摆摊日志
        CMD_DB_UPDATEPUSHMESSAGEINFO,               // GS-DB 更新推送信息 [4/23/2014 LiaoWei]
        CMD_DB_QUERYPUSHMESSAGEUSERLIST,            // GS-DB 请求要推送的玩家列表 [4/23/2014 LiaoWei]
        CMD_DB_ADDWING,                             // GS-DB 请求得到第一个翅膀[4/30/2014, liuhuicheng]
        CMD_DB_MODWING,                             // GS-DB 请求修改翅膀[4/30/2014, liuhuicheng]
        CMD_DB_REFERPICTUREJUDGE,                   // GS-DB 提交图鉴信息 [5/18/2014 LiaoWei]
        CMD_DB_QUERYMOJINGEXCHANGEINFO,             // GS-DB 请求绑定钻石兑换信息 [5/21/2014 LiaoWei]
        CMD_DB_UPDATEMOJINGEXCHANGEINFO,            // GS-DB 更新绑定钻石兑换信息 [5/21/2014 LiaoWei]
        CMD_DB_MODIFY_WANMOTA,                      // GS-DB 修改万魔塔表数据 [6/6/2014 ChenXiaojun]
        CMD_DB_GET_WANMOTA_DETAIL,                  // GS-DB 获取万魔塔信息 [6/6/2014 ChenXiaojun]
        CMD_DB_QUERY_REPAYACTIVEINFO,               // GS-DB 查询回馈活动信息
        CMD_DB_GET_REPAYACTIVEAWARD,                // GS-DB 获取回馈活动奖励
        CMD_DB_UPDATE_ACCOUNT_ACTIVE,               // GS-DB 更新帐户活跃信息  [7/9/2014 ChenXiaojun]
        CMD_DB_QUERY_GETOLDRESINFO,                 // GS-DB 查询资源找回信息 [7/11/2014 gwz]
        CMD_DB_UPDATE_OLDRESOURCE,                  // GS-DB 资源找回，领取资源 [7/11/2014 gwz]
        CMD_DB_UPDATEGOODS_CMD2,                    // GS-DB 更新物品扩展信息(装备洗练等)
        CMD_DB_UPDATESTARCONSTELLATION,             // GS-DB 激活星座更新星座信息 [8/1/2014 LiaoWei]
        CMD_DB_SAVECONSUMELOG,                      // GS-DB 保存钻石消费信息 [8/19/2014 gwz]
        CMD_DB_QUERYVIPLEVELAWARDFLAG,              // GS-DB 玩家领取VIP等级奖励 [8/21/2014 LiaoWei]
        CMD_DB_UPDATEVIPLEVELAWARDFLAG,             // GS-DB 玩家更新VIP等级奖励标记信息 [8/21/2014 LiaoWei]
        CMD_DB_UPDATEFIRSTCHARGE,                   // GS-DB 玩家更新首充信息 [9/16/2014 gwz]
        CMD_DB_FIRSTCHARGE_CONFIG,                  // GS-DB 启动时发送给db首充配置表 [12/11/2014 gwz]
        CMD_DB_UPDATEBANGHUIFUBEN,					// GS-DB 更新帮会副本信息
        CMD_DB_ADD_STORE_BoundToken,                  // GS-DB 更新玩家仓库金币信息
        CMD_DB_ADD_STORE_MONEY,                     // GS-DB 更新玩家仓库绑定金币信息
        CMD_DB_GM_UPDATE_BANGLEVEL,                   // GS-DB 更新玩家帮会战旗等级
        CMD_DB_UPDATE_LINGYU,                       // GS-DB 更新翎羽
        CMD_DB_REQUESTNEWGMAILLIST,                 // GS-DB 去GameDBServer获取新的群邮件列表
        CMD_DB_MODIFYROLEGMAIL,                     // GS-DB 去GameDBServer设置玩家群邮件读取记录

        CMD_DB_QUERYROLEMONEYINFO,                  // GS-DB 查询玩家钻石的相关数据

        CMD_DB_ALL_COMPLETION_OF_TASK_BY_TASKID,    // GS-DB 完成某任务与之前所有任务(仅限对db操作完成，不走任务流程)
        CMD_DB_ROLE_BUY_YUE_KA_BUT_OFFLINE,         // GS-DB 玩家买了月卡但是下线了，交由DB处理
        CMD_DB_MODIFY_ROLE_HUO_BI_OFFLINE,          // GS-DB 后台离线修改角色货币
        CMD_DB_UPDATE_USR_SECOND_PASSWORD,          // GS-DB 更新账号二级密码
        CMD_DB_GET_USR_SECOND_PASSWORD,             // GS-DB 获取账号的二级密码

        CMD_DB_UPDATE_MARRY_DATA,                   // GS_DB [bing] 更新婚姻数据
        CMD_DB_GET_MARRY_DATA,                      // GS_DB [bing] 根据Roleid获取婚姻数据
        CMD_DB_MARRY_PARTY_QUERY,                   // GS_DB 读取婚宴列表
        CMD_DB_MARRY_PARTY_ADD,                     // GS_DB 加婚宴
        CMD_DB_MARRY_PARTY_REMOVE,                  // GS_DB 刪婚宴
        CMD_DB_MARRY_PARTY_JOIN_INC,                // GS_DB 婚宴參予次数
        CMD_DB_MARRY_PARTY_JOIN_CLEAR,              // 清空婚宴个人參予数
        CMD_DB_TIANTI_ADD_ZHANBAO_LOG = 10200,      // 添加天梯战报日志
        CMD_DB_TIANTI_UPDATE_ROLE_DATA = 10201,      // 更新角色天梯数据
        CMD_DB_TIANTI_UPDATE_RONGYAO = 10202,      // 更新角色天梯数据

        CMD_DB_MERLIN_CREATE,                               // GS-DB 创建梅林魔法书数据
        CMD_DB_MERLIN_UPDATE,                              // GS-DB 更新梅林魔法书数据
        CMD_DB_MERLIN_QUERY,                                // GS-DB 查询梅林魔法书数据

        CMD_DB_UPDATE_HOLYITEM,                     // GS_DB [bing] 更新圣物数据

        CMD_DB_FLUORESCENT_GEM_RESET_BAG, // GS-DB 整理荧光宝石背包 [XSea 2015/8/7]
        CMD_DB_FLUORESCENT_POINT_UPDATE, // GS-DB 更新荧光粉末 [XSea 2015/8/10]
        CMD_DB_FLUORESCENT_EQUIP, // GS-DB 装备荧光宝石 [XSea 2015/8/13]
        CMD_DB_FLUORESCENT_UN_EQUIP, // GS-DB 卸下荧光宝石 [XSea 2015/8/13]
        CMD_DB_FLUORESCENT_POINT_MODIFY, // GS-DB 更新荧光粉末增加 or 减少

        #region 10220-10300-lt

        CMD_DB_QUERY_ROLEMINIINFO = 10220, //根据角色ID查询角色区号/帐号等不易变信息
        CMD_DB_QUERY_USERACTIVITYINFO = 10221, //查询帐号_活动_活动时间相对应的活动状态和奖励信息
        CMD_DB_UPDATE_USERACTIVITYINFO = 10222, //保存帐号_活动_活动时间相对应的活动状态和奖励信息

        #endregion 10220-10300-lt

        CMD_DB_GET_SERVERLIST = 11000,
        CMD_DB_ONLINE_SERVERHEART,
        CMD_DB_GET_SERVERID,
        CMD_NAME_REGISTERNAME = 12000,             //注册名字到名字服务器
        CMD_SPR_GM_SET_MAIN_TASK = 13000,          //GM命令设置任务

        //CMD_DB_RETURN_RECELL_COUNT = 13100, //玩家召回人数
        //CMD_DB_RETURN_RECELL_USER_ID = 13101, //玩家召回推荐人id
        //CMD_DB_RETURN_RECELL_USER_ID_SET = 13102, //玩家召回推荐人id设置
        //CMD_DB_RETURN_AWARD = 13103, //玩家召回奖励列表
        //CMD_DB_RETURN_AWARD_UPDATE = 13104, //玩家召回奖励
        //CMD_DB_RETURN_IS_OPEN = 13105, //玩家召回设置是否开放
        //CMD_DB_RETURN_UPDATE_STATE = 13106, //玩家找回_检查审核状态
        //CMD_DB_GM_BAN_CHACK                 = 13107, //获取玩家外挂进程列表

        CMD_DB_RETURN_IS_OPEN = 13100,      //设置开放状态
        CMD_DB_RETURN_DATA = 13101,         //玩家找回_数据
        CMD_DB_RETURN_DATA_UPDATE = 13102,  //玩家找回_数据更新
        CMD_DB_RETURN_DATA_DEL = 13103,     //玩家找回_删除
        CMD_DB_RETURN_DATA_LIST = 13104,    //玩家找回_列表
        CMD_DB_RETURN_AWARD_LIST = 13105,   //奖励列表
        CMD_DB_RETURN_AWARD_UPDATE = 13106, //奖励更新

        CMD_DB_TALENT_MODIFY = 13108, //天赋数据——基本更新
        CMD_DB_TALENT_EFFECT_MODIFY = 13109, //天赋数据——效果更新
        CMD_DB_TALENT_EFFECT_CLEAR = 13110, //天赋数据——效果清除

        CMD_DB_GM_BAN_CHACK = 13111, //获取玩家外挂进程列表
        CMD_DB_GM_BAN_LOG = 13112, //记录玩家外挂封号信息

        CMD_DB_TEN_INIT = 13113, //应用宝初始化

        CMD_DB_SPREAD_AWARD_GET = 13114,
        CMD_DB_SPREAD_AWARD_UPDATE = 13115,

        CMD_DB_FUND_INFO = 13116,
        CMD_DB_FUND_BUY = 13117,
        CMD_DB_FUND_AWARD = 13118,
        CMD_DB_FUND_MONEY = 13119,

        CMD_DB_ACTIVATE_GET = 13120,
        CMD_DB_ACTIVATE_SET = 13121,

        CMD_DB_UNION_ALLY_LOG = 13122,
        CMD_DB_UNION_ALLY_LOG_ADD = 13123,

        CMD_DB_INPUTPOINTS_EXCHANGE = 13150,  //充值积分兑换
        CMD_DB_UPDATE_INPUTPOINTS = 13151,  //更新充值积分
        CMD_DB_UPDATE_INPUTPOINTS_USERID = 13152,  //更新充值积分 by userid

        CMD_DB_UPDATE_SPECACT = 13160,  //更新专享活动数据
        CMD_DB_DELETE_SPECACT = 13161,  //删除专享活动
        CMD_DB_GET_SPECJIFENINFO = 13162,  //获得专享积分数据
        CMD_DB_UPDATE_SPECJIFEN = 13163,  //更新专享积分
        CMD_DB_GET_SPECACTINFO = 13164,  //获得专享活动数据

        CMD_DB_ROLE_JIERI_GIVE_TO_OTHER = 13200,                // GS-DB 赠送节日物品
        CMD_DB_SPR_GET_JIERI_GIVE_AWARD = 13201,                // GS-DB 角色领取节日赠送奖励
        CMD_DB_LOAD_ROLE_JIERI_GIVE_RECV_INFO = 13202,               // GS-DB 角色首次查询节日赠送信息时，从数据库加载

        CMD_DB_LOAD_JIERI_GIVE_KING_RANK = 13203,               //GS-DB 加载节日赠送王排行, 服务器启动时加载前N条
        CMD_DB_LOAD_ROLE_JIERI_GIVE_KING = 13204,               //GS-DB 角色第一次上线时，加载角色的节日赠送信息
        CMD_DB_GET_JIERI_GIVE_KING_AWARD = 13205,               //GS-DB 领取节日赠送王奖励

        CMD_DB_LOAD_JIERI_RECV_KING_RANK = 13206,               //GS-DB 加载节日收取王排行, 服务器启动时加载前N条
        CMD_DB_LOAD_ROLE_JIERI_RECV_KING = 13207,               //GS-DB 角色第一次上线时，加载角色的节日赠送信息
        CMD_DB_GET_JIERI_RECV_KING_AWARD = 13208,               //GS-DB 领取节日收取王奖励

        CMD_DB_UPDATE_ROLE_GUARD_STATUE = 13210,        // GS-DB 更新角色守护雕像信息
        CMD_DB_UPDATE_ROLE_GUARD_SOUL = 13211,              // GS-DB 更新角色守护之灵信息

        CMD_DB_QUERY_JIERI_LIANXU_CHARGE = 13214,       // GS-DB 查询节日连续充值数据
        CMD_DB_UPDATE_JIERI_LIANXU_CHARGE_AWARD = 13215,  // GS-DB 更新连续充值领奖信息

        CMD_DB_SPR_GET_JIERI_RECV_AWARD = 13218,                // GS-DB 角色领取节日收礼奖励
        CMD_DB_LOAD_ROLE_JIERI_RECV_INFO = 13219,               // GS-DB 角色首次查询节日收礼信息时，从数据库加载
        CMD_DB_UPDATE_SEVEN_DAY_ITEM_DATA = 13220,          // GS-DB 更新七日活动信息
        CMD_DB_CLEAR_SEVEN_DAY_DATA = 13221,                    // GS-DB 超时清除七日活动信息
        CMD_DB_GET_EACH_DAY_CHARGE = 13222,                        // GS-DB 查询每日充值

        CMD_DB_GET_KING_ROLE_DATA = 13230,                         // 保存王者数据，pk之王，罗兰城主
        CMD_DB_PUT_KING_ROLE_DATA = 13231,                         // 设置王者数据，pk之王，罗兰城主
        CMD_DB_CLR_KING_ROLE_DATA = 13232,                         // 清除王者数据，pk之王，罗兰城主

        CMD_DB_UPDATE_BUILDING_DATA = 13300,        // 更新建筑数据
        CMD_DB_UPDATE_BUILDING_LOG = 13301,        // 更新建筑Log

        CMD_DB_UPDATE_ONEPIECE_TREASURE_LOG = 13400,    // 更新藏宝秘境Log

        CMD_SPR_KF_SWITCH_SERVER = 14000,           //跨服_通知客户端切换服务器(包含跨服登录Token)

        CMD_SPR_CHANGE_NAME = 14001,                //角色请求改名
        CMD_NTF_EACH_ROLE_ALLOW_CHANGE_NAME = 14002, //服务器向客户端主动推送每个角色的允许改名信息
        CMD_NTF_CANNOT_JOIN_KUAFU_FU_BEN_END_TICKS = 14003, //服务器通知客户端更新禁止参与跨服副本的结束时间, DateTime.Now.Ticks   客户端说先不发这个消息 2015/08/15
        CMD_PLEASE_TELL_ME = 14004,
        CMD_DB_GET_PLAT_CHARGE_KING_LIST = 14005, // 获取平台充值王排行榜, 废弃不再使用
        CMD_SPR_CHANGE_BANGHUI_NAME = 14006, // 更改帮会名字
        CMD_DB_LOAD_TRADE_BLACK_HOUR_ITEM = 14007, // 加载交易黑名单信息
        CMD_DB_SAVE_TRADE_BLACK_HOUR_ITEM = 14008, // 保存交易黑名单信息

        CMD_NTF_MAGIC_CRASH_UNITY = 14010, //

        CMD_DB_ZHENGBA_SAVE_SUPPORT_LOG = 14011,    // 众神争霸 --- 存储支持日志
        CMD_DB_ZHENGBA_SAVE_PK_LOG = 14012,         // 众神争霸 --- 存储pk日志
        CMD_DB_ZHENGBA_LOAD_SUPPORT_LOG = 14013,    // 众神争霸 --- 加载支持日志
        CMD_DB_ZHENGBA_LOAD_PK_LOG = 14014,         // 众神争霸 --- 加载pk日志
        CMD_DB_ZHENGBA_LOAD_SUPPORT_FLAG = 14015,   // 众神争霸 --- 加载我的支持信息
        CMD_DB_ZHENGBA_LOAD_WAIT_AWARD_YAZHU = 14016,   // 众神争霸 --- 加载等待押注发奖
        CMD_DB_ZHENGBA_SET_YAZHU_AWARD_FLAG = 14017, // 众神争霸 --- 更新押注发奖标记

        CMD_LOGDB_ADD_LOG = 20000,
        CMD_LOGDB_ADD_TRADEMONEY_FREQ_LOG = 20001,
        CMD_LOGDB_ADD_TRADEMONEY_NUM_LOG = 20002,
        CMD_LOGDB_UPDATE_ROLE_KUAFU_DAY_LOG = 20003,

        CMD_DB_UPDATA_TAROT = 20100,

        /// <summary>
        /// Xóa kỹ năng khỏi DB
        /// </summary>
        CMD_DB_DEL_SKILL = 20101,

        CMD_SPR_TASKLIST_DATA = 29900,
        CMD_SPR_TASKLIST_KEY = 30000,
        CMD_SPR_TASKLIST_NOTIFY = 30001,

        // linyl
        CMD_SPR_GETATTRIBALL = 30100,   // 获取装备二级属性

        CMD_DB_ERR_RETURN = 30767, //MAX 消息定义不可超过此值

        #region KiemThe

        #region CreateRole

        /// <summary>
        /// Lấy danh sách Tân Thủ Thôn
        /// </summary>
        CMD_KT_GET_NEWBIE_VILLAGES = 40000,

        #endregion CreateRole

        #region Skill

        /// <summary>
        /// Gói tin gửi về Client làm mới lại danh sách kỹ năng
        /// </summary>
        CMD_KT_G2C_RENEW_SKILLLIST = 45000,

        /// <summary>
        /// Gói tin gửi về Server yêu cầu cộng điểm kỹ năng của Client
        /// </summary>
        CMD_KT_C2G_SKILL_ADDPOINT = 45001,

        /// <summary>
        /// Gói tin gửi từ Client lên Server lưu thiết lập kỹ năng tay vào khung sử dụng
        /// </summary>
        CMD_KT_C2G_SET_SKILL_TO_QUICKKEY = 45002,

        /// <summary>
        /// Gói tin gửi từ Client lên Server lưu thiết lập và kích hoạt vòng sáng tại ô tương ứng
        /// </summary>
        CMD_KT_C2G_SET_AND_ACTIVATE_AURA = 45003,

        /// <summary>
        /// Gói tin gửi từ Client về Server yêu cầu sử dụng kỹ năng
        /// </summary>
        CMD_KT_C2G_USESKILL = 45009,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo đối tượng sử dụng kỹ năng
        /// </summary>
        CMD_KT_G2C_USESKILL = 45010,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo kỹ năng thiết lập trạng thái chờ phục hồi
        /// </summary>
        CMD_KT_G2C_NOTIFYSKILLCOOLDOWN = 45011,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo tạo đạn
        /// </summary>
        CMD_KT_G2C_CREATEBULLET = 45012,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo đạn nổ
        /// </summary>
        CMD_KT_G2C_BULLETEXPLODE = 45013,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo kết quả kỹ năng
        /// </summary>
        CMD_KT_G2C_SKILLRESULT = 45014,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo thao tác Buff
        /// </summary>
        CMD_KT_G2C_SPRITEBUFF = 45015,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo đối tượng tốc biến tới vị trí chỉ định
        /// </summary>
        CMD_KT_G2C_BLINKTOPOSITION = 45016,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo tạo nhiều tia đạn
        /// </summary>
        CMD_KT_G2C_CREATEBULLETS = 45017,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo đạn nổ nhiều vị trí
        /// </summary>
        CMD_KT_G2C_BULLETEXPLODES = 45018,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo tốc độ di chuyển của đối tượng thay đổi
        /// </summary>
        CMD_KT_G2C_MOVESPEEDCHANGED = 45019,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo đối tượng khinh công tới vị trí chỉ định
        /// </summary>
        CMD_KT_G2C_FLYTOPOSITION = 45020,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo trạng thái ngũ hành của đối tượng thay đổi
        /// </summary>
        CMD_KT_G2C_SPRITESERIESSTATE = 45021,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo danh sách kết quả kỹ năng
        /// </summary>
        CMD_KT_G2C_SKILLRESULTS = 45022,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo trạng thái tàng hình của đối tượng thay đổi
        /// </summary>
        CMD_KT_G2C_OBJECTINVISIBLESTATECHANGED = 45023,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo làm mới tất cả thời gian hồi kỹ năng
        /// </summary>
        CMD_KT_G2C_RESETSKILLCOOLDOWN = 45024,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo tốc độ xuất chiêu của đối tượng thay đổi
        /// </summary>
        CMD_KT_G2C_ATTACKSPEEDCHANGED = 45025,

        #endregion Skill

        /// <summary>
        /// Lệnh GM
        /// </summary>
        CMD_KT_GM_COMMAND = 50000,

        #region RoleAttributes

        /// <summary>
        /// Lấy thông tin thuộc tính nhân vật
        /// </summary>
        CMD_KT_ROLE_ATRIBUTES = 50001,

        #endregion RoleAttributes

        #region Notification Tips

        /// <summary>
        /// Hiển thị NotificationTip
        /// </summary>
        CMD_KT_SHOW_NOTIFICATIONTIP = 50002,

        #endregion Notification Tips

        #region Faction and Route changed

        /// <summary>
        /// Thông báo môn phái người chơi đã thay đổi
        /// </summary>
        CMD_KT_FACTIONROUTE_CHANGED = 50003,

        #endregion Faction and Route changed

        #region Click NPC

        /// <summary>
        /// Người chơi ấn vào NPC
        /// </summary>
        CMD_KT_CLICKON_NPC = 50004,

        #endregion Click NPC

        #region NPCDialog

        /// <summary>
        /// Server gửi lệnh mở khung NPC Dialog cho Client
        /// </summary>
        CMD_KT_G2C_NPCDIALOG = 50005,

        /// <summary>
        /// Client phản hồi về Server về sự lựa chọn của người chơi vào thành phần trong khung NPC Dialog (nếu có)
        /// </summary>
        CMD_KT_C2G_NPCDIALOG = 50006,

        #endregion NPCDialog

        #region Change Action

        /// <summary>
        /// Server gửi lệnh cho Client thay đổi động tác
        /// </summary>
        CMD_KT_G2C_CHANGEACTION = 50007,

        /// <summary>
        /// Client gửi yêu cầu thay đổi động tác cho Server
        /// </summary>
        CMD_KT_C2G_CHANGEACTION = 50008,

        #endregion Change Action

        #region Debug

        /// <summary>
        /// Server gửi lệnh cho Client hiện khối Debug Object ở các vị trí
        /// </summary>
        CMD_KT_G2C_SHOWDEBUGOBJECTS = 50009,

        #endregion Debug

        #region UI

        /// <summary>
        /// Server gửi lệnh cho Client hiện khung hồi sinh
        /// </summary>
        CMD_KT_G2C_SHOWREVIVEFRAME = 50010,

        /// <summary>
        /// Gói tin phản hồi từ Client về Server phương thức hồi sinh được người chơi lựa chọn
        /// </summary>
        CMD_KT_C2G_CLIENTREVIVE = 50011,

        #endregion UI

        #region Trap

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo có bẫy tại vị trí tương ứng
        /// </summary>
        CMD_KT_SPR_NEWTRAP = 50012,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo có bẫy tại vị trí tương ứng
        /// </summary>
        CMD_KT_SPR_DELTRAP = 50013,

        #endregion Trap

        #region Settings

        /// <summary>
        /// Gói tin phản hồi từ Client về Server lưu thiết lập hệ thống
        /// </summary>
        CMD_KT_C2G_SAVESYSTEMSETTINGS = 50014,

        /// <summary>
        /// Gói tin phản hồi từ Client về Server lưu thiết lập Auto
        /// </summary>
        CMD_KT_C2G_SAVEAUTOSETTINGS = 50015,

        #endregion Settings

        #region Team

        /// <summary>
        /// Gói tin thông báo mời vào nhóm
        /// </summary>
        CMD_KT_INVITETOTEAM = 50016,

        /// <summary>
        /// Gói tin yêu cầu tạo nhóm
        /// </summary>
        CMD_KT_CREATETEAM = 50017,

        /// <summary>
        /// Gói tin đồng ý thêm vào nhóm tương ứng
        /// </summary>
        CMD_KT_AGREEJOINTEAM = 50018,

        /// <summary>
        /// Gói tin từ chối thêm vào nhóm tương ứng
        /// </summary>
        CMD_KT_REFUSEJOINTEAM = 50019,

        /// <summary>
        /// Gói tin lấy thông tin nhóm tương ứng
        /// </summary>
        CMD_KT_GETTEAMINFO = 50020,

        /// <summary>
        /// Gói tin trục xuất người chơi khỏi nhóm
        /// </summary>
        CMD_KT_KICKOUTTEAMMATE = 50021,

        /// <summary>
        /// Gói tin bổ nhiệm đội trưởng
        /// </summary>
        CMD_KT_APPROVETEAMLEADER = 50022,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo thay đổi thông tin đội viên
        /// </summary>
        CMD_KT_REFRESHTEAMMEMBERATTRIBUTES = 50023,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo thành viên thay đổi
        /// </summary>
        CMD_KT_TEAMMEMBERCHANGED = 50024,

        /// <summary>
        /// Gói tin thông báo bản thân rời nhóm
        /// </summary>
        CMD_KT_LEAVETEAM = 50025,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo cập nhật thông tin tổi đội của người chơi tương ứng
        /// </summary>
        CMD_KT_G2C_UPDATESPRITETEAMDATA = 50026,

        /// <summary>
        /// Gói tin thông báo yêu cầu xin vào nhóm của người chơi tương ứng
        /// </summary>
        CMD_KT_ASKTOJOINTEAM = 50027,

        #endregion Team

        #region ItemDialog

        /// <summary>
        /// Server gửi lệnh mở khung Item Dialog cho Client
        /// </summary>
        CMD_KT_G2C_ITEMDIALOG = 50030,

        /// <summary>
        /// Client phản hồi về Server về sự lựa chọn của người chơi vào thành phần trong khung Item Dialog (nếu có)
        /// </summary>
        CMD_KT_C2G_ITEMDIALOG = 50031,

        #endregion ItemDialog

        #region SHOPCMD

        /// <summary>
        /// Gói tin gửi từ Client lên Server yêu cầu mở Shop tương ứng
        /// </summary>
        CMD_KT_C2G_OPENSHOP = 50032,

        #endregion SHOPCMD

        /// <summary>
        /// Server lệnh cho Client đóng khung NPCDialog hoặc ItemDialog
        /// </summary>
        CMD_KT_G2C_CLOSENPCITEMDIALOG = 50033,

        #region Tỷ thí

        /// <summary>
        /// Gói tin thông báo mời tỷ thí
        /// </summary>
        CMD_KT_ASK_CHALLENGE = 50034,

        /// <summary>
        /// Gói tin thông báo nhận lời mời tỷ thí
        /// </summary>
        CMD_KT_C2G_RESPONSE_CHALLENGE = 50035,

        /// <summary>
        /// Gói tin gửi từ Server về Client bắt đầu thiết lập trạng thái tỷ thí
        /// </summary>
        CMD_KT_G2C_START_CHALLENGE = 50036,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo kết thúc tỷ thí
        /// </summary>
        CMD_KT_G2C_STOP_CHALLENGE = 50037,

        #endregion Tỷ thí

        #region Auto Path

        /// <summary>
        /// Gói tin gửi từ Client lên Server do Auto tìm đường gửi, yêu cầu dịch chuyển đến vị trí tương ứng
        /// </summary>
        CMD_KT_C2G_AUTOPATH_CHANGEMAP = 50038,

        #endregion Auto Path

        #region GrowPoint

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo có điểm thu thập mới
        /// </summary>
        CMD_KT_G2C_NEW_GROWPOINT = 50039,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo xóa điểm thu thập
        /// </summary>
        CMD_KT_G2C_DEL_GROWPOINT = 50040,

        /// <summary>
        /// Gói tin gửi từ Client về Server thông báo người chơi ấn vào điểm thu thập
        /// </summary>
        CMD_KT_C2G_GROWPOINT_CLICK = 50041,

        #endregion GrowPoint

        #region Progress Bar

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo cập nhật trạng thái chạy thanh Progess
        /// </summary>
        CMD_KT_G2C_UPDATE_PROGRESSBAR = 50042,

        #endregion Progress Bar

        #region Kỳ Trân Các

        /// <summary>
        /// Gói tin yêu cầu mở Kỳ Trân Các
        /// </summary>
        CMD_KT_OPEN_TOKENSHOP = 50043,
        CMD_KT_GOI_COIN = 50200,

        #endregion Kỳ Trân Các

        #region Mở/đóng khung bất kỳ

        /// <summary>
        /// Gói tin gửi từ Server về Client yêu cầu mở khung bất kỳ
        /// </summary>
        CMD_KT_G2C_OPEN_UI = 50044,

        /// <summary>
        /// Gói tin gửi từ Server về Client yêu cầu đóng khung bất kỳ
        /// </summary>
        CMD_KT_G2C_CLOSE_UI = 50045,

        #endregion Mở/đóng khung bất kỳ

        #region Chuyển trạng thái cưỡi

        /// <summary>
        /// Gói tin thông báo trạng thái cưỡi thay đổi
        /// </summary>
        CMD_KT_TOGGLE_HORSE_STATE = 50046,

        #endregion Chuyển trạng thái cưỡi

        #region Cường hóa trang bị

        /// <summary>
        /// Gói tin cường hóa trang bị
        /// </summary>
        CMD_KT_EQUIP_ENHANCE = 50047,

        /// <summary>
        /// Gói tin ghép Huyền Tinh
        /// </summary>
        CMD_KT_COMPOSE_CRYSTALSTONES = 50048,

        /// <summary>
        /// Gói tin tách Huyền Tinh
        /// </summary>
        CMD_KT_SPLIT_CRYSTALSTONES = 50049,

        #endregion Cường hóa trang bị

        #region Khu vực động

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo có khu vực động mới
        /// </summary>
        CMD_KT_G2C_NEW_DYNAMICAREA = 50050,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo xóa khu vực động
        /// </summary>
        CMD_KT_G2C_DEL_DYNAMICAREA = 50051,

        #endregion Khu vực động

        #region Tuyên chiến

        /// <summary>
        /// Gói tin thông báo tuyên chiến
        /// </summary>
        CMD_KT_ASK_ACTIVEFIGHT = 50052,

        /// <summary>
        /// Gói tin gửi từ Server về Client bắt đầu thiết lập trạng thái tuyên chiến
        /// </summary>
        CMD_KT_G2C_START_ACTIVEFIGHT = 50053,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo kết thúc tuyên chiến
        /// </summary>
        CMD_KT_G2C_STOP_ACTIVEFIGHT = 50054,

        #endregion Tuyên chiến

        #region Avarta

        /// <summary>
        /// Gói tin thông báo Avarta nhân vật thay đổi
        /// </summary>
        CMD_KT_CHANGE_AVARTA = 50055,

        #endregion Avarta

        #region Tinh hoạt lực, kỹ năng sống

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo giá trị Tinh lực, hoạt lực nhân vật thay đổi
        /// </summary>
        CMD_KT_G2C_UPDATE_ROLE_GATHERMAKEPOINT = 50056,

        /// <summary>
        /// Gói tin từ Server gửi về Client thông báo cấp độ kỹ năng sống và kinh nghiệm thay đổi
        /// </summary>
        CMD_KT_G2C_UPDATE_LIFESKILL_LEVEL = 50057,

        #endregion Tinh hoạt lực, kỹ năng sống

        #region Chế đồ

        /// <summary>
        /// Gói tin bắt đầu chế đồ
        /// </summary>
        CMD_KT_BEGIN_CRAFT = 50058,

        /// <summary>
        /// Gói tin kết thúc chế đồ
        /// </summary>
        CMD_KT_G2C_FINISH_CRAFT = 50059,

        #endregion Chế đồ

        #region Message Box

        /// <summary>
        /// Hiển thị bảng thông báo về Client
        /// </summary>
        CMD_KT_SHOW_MESSAGEBOX = 50060,

        #endregion Message Box

        #region BATTLE

        /// <summary>
        /// Thông báo Text sự kiện hoạt động phụ bản
        /// </summary>
        CMD_KT_EVENT_NOTIFICATION = 50061,

        /// <summary>
        /// Thông báo số liên trảm
        /// </summary>
        CMD_KT_KILLSTREAK = 50062,

        /// <summary>
        /// Thông báo trạng thái đóng mở khung Mini sự kiện hoạt động phụ bản
        /// </summary>
        CMD_KT_EVENT_STATE = 50063,

        #endregion BATTLE

        #region Hoạt động đặc biệt

        /// <summary>
        /// Bảng điểm Tống Kim
        /// </summary>
        CMD_KT_SONGJINBATTLE_RANKING = 50064,

        #endregion Hoạt động đặc biệt

        #region Tìm người chơi

        /// <summary>
        /// Gói tin tìm kiếm người chơi
        /// </summary>
        CMD_KT_BROWSE_PLAYER = 50065,

        /// <summary>
        /// Gói tin kiểm tra vị trí người chơi
        /// </summary>
        CMD_KT_CHECK_PLAYER_LOCATION = 50066,

        /// <summary>
        /// Gói tin kiểm tra thông tin người chơi
        /// </summary>
        CMD_KT_GET_PLAYER_INFO = 50099,

        #endregion Tìm người chơi

        #region Danh hiệu

        /// <summary>
        /// Cập nhật hiển thị danh hiệu
        /// </summary>
        CMD_KT_UPDATE_TITLE = 50067,

        /// <summary>
        /// Cập nhật hiển thị tên
        /// </summary>
        CMD_KT_UPDATE_NAME = 50068,

        #endregion Danh hiệu

        #region Danh vọng

        /// <summary>
        /// Cập nhật danh vọng
        /// </summary>
        CMD_KT_UPDATE_REPUTE = 50069,

        /// <summary>
        /// Cập nhật giá trị tài phú
        /// </summary>
        CMD_KT_UPDATE_TOTALVALUE = 50070,

        #endregion Danh vọng

        #region Quà Downlaod

        /// <summary>
        /// Gói tin thao tác với sự kiện tải lần đầu nhận quà
        /// </summary>
        CMD_KT_GET_BONUS_DOWNLOAD = 50071,

        #endregion Quà Downlaod

        #region Bách Bảo Rương

        /// <summary>
        /// Gói tin thao tác với hoạt động Bách Bảo Rương
        /// </summary>
        CMD_KT_SEASHELL_CIRCLE = 50072,

        #endregion Bách Bảo Rương

        #region BOT

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo có BOT mới
        /// </summary>
        CMD_KT_G2C_NEW_BOT = 50073,

        /// <summary>
        /// Gói tin gửi từ Server về Client thông báo xóa BOT
        /// </summary>
        CMD_KT_G2C_DEL_BOT = 50074,

        #endregion BOT

        #region Cường hóa Ngũ hành ấn

        /// <summary>
        /// Gói tin cường hóa Ngũ hành ấn
        /// </summary>
        CMD_KT_SIGNET_ENHANCE = 50075,

        #endregion Cường hóa Ngũ hành ấn

        #region Bảng xếp hạng

        /// <summary>
        /// Gói tin truy vấn bảng xếp hạng
        /// </summary>
        CMD_KT_QUERY_PLAYERRANKING = 50076,

        #endregion Bảng xếp hạng

        #region Set dự phòng

        /// <summary>
        /// Đổi set dự phòng
        /// </summary>
        CMD_KT_C2G_CHANGE_SUBSET = 50077,

        #endregion Set dự phòng

        #region Khung nhập vật phẩm

        /// <summary>
        /// Nhập danh sách vật phẩm
        /// </summary>
        CMD_KT_SHOW_INPUTITEMS = 50078,

        /// <summary>
        /// Nhập danh sách vật phẩm
        /// </summary>
        CMD_KT_SHOW_INPUTEQUIPANDMATERIALS = 50079,

        CMD_KT_FACTION_PVP_RANKING_INFO = 50081,

        #endregion Khung nhập vật phẩm

        #region FAMILY

        //TẠO GIA TỘC
        CMD_KT_FAMILY_CREATE = 50082,

        //YÊU CẦU THAM GIA
        CMD_KT_FAMILY_REQUESTJOIN = 50083,

        //KICK THÀNH VIÊN KHỎI GIA TỘC
        CMD_KT_FAMILY_KICKMEMBER = 50084,

        // LẤY DANH SÁCH GIA TỘC
        CMD_KT_FAMILY_GETLISTFAMILY = 50085,

        // HỦY GIA TỘC
        CMD_KT_FAMILY_DESTROY = 50086,

        // CHANGE NOTIFY GIA TỘC
        CMD_KT_FAMILY_CHANGENOTIFY = 50087,

        // CHANGE NOTIFY JOIN GIA TỘC
        CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY = 50088,

        // THAY ĐỔI CHỨC VỊ CỦA THÀNH VIÊN TRONG GIA TỘC
        CMD_KT_FAMILY_CHANGE_RANK = 50089,

        // TRẢ LỜI YÊU CẦU THAM GIA GIA TỘC
        CMD_KT_FAMILY_RESPONSE_REQUEST = 50090,

        /// <summary>
        /// Giải tán gia tộc
        /// </summary>
        CMD_KT_FAMILY_QUIT = 50091,

        /// <summary>
        /// Mở giao diện gia tộc
        /// </summary>
        CMD_KT_FAMILY_OPEN = 50092,

        /// <summary>
        /// Số lượt đã mở phụ bản Vượt ải gia tộc
        /// </summary>
        CMD_KT_FAMILY_WEEKLYFUBENCOUNT = 50093,

        #endregion FAMILY

        #region Bang hội

        /// <summary>
        /// Tạo 1 bang hội
        /// </summary>
        CMD_KT_GUILD_CREATE = 50100,

        /// <summary>
        /// Lấy thông tin bang hội
        /// </summary>
        CMD_KT_GUILD_GETINFO = 50101,

        /// <summary>
        /// Lấy ra thành viên bang hội
        /// </summary>
        CMD_KT_GUILD_GETMEMBERLIST = 50102,

        /// <summary>
        /// Thay đổi chức vị của 1 thành viên
        /// Thay đổi chức của 1 thành viên
        /// </summary>
        CMD_KT_GUILD_CHANGERANK = 50103,

        /// <summary>
        /// Kick 1 gia tộc
        /// Packet khi ấn vào ĐUỔI 1 tộc ở khung thành viên
        /// </summary>
        CMD_KT_GUILD_KICKFAMILY = 50104,

        /// <summary>
        /// Trả về danh sách ưu tú
        /// Packet khi mở khung ưu tú
        /// </summary>
        CMD_KT_GUILD_GETGIFTED = 50105,

        /// <summary>
        /// Packet khi mở giao diện quan hàm
        /// </summary>
        CMD_KT_GUILD_OFFICE_RANK = 50106,

        /// <summary>
        /// Packet bầu ưu tú cho 1 thành viên
        /// </summary>

        CMD_KT_GUILD_VOTEGIFTED = 50107,

        /// <summary>
        /// Cống hiến vào bang
        /// </summary>
        CMD_KT_GUILD_DONATE = 50108,

        /// <summary>
        /// Khi click vào khung hoạt động tranh đoạt lãnh thổ
        /// </summary>
        CMD_KT_GUILD_TERRITORY = 50109,

        /// <summary>
        /// PACKET SET THÀNH CHÍNH
        /// </summary>
        CMD_KT_GUILD_SETCITY = 50110,

        /// <summary>
        /// PACKET THIẾT LẬP THUẾ
        /// </summary>
        CMD_KT_GUILD_SETTAX = 50111,

        /// <summary>
        /// Giải tán bang hội
        /// </summary>
        CMD_KT_GUILD_QUIT = 50112,

        /// <summary>
        /// ĐIỀU CHỈNH QUỸ THƯỞNG
        /// </summary>
        CMD_KT_GUILD_CHANGE_MAXWITHDRAW = 50113,

        /// <summary>
        /// Đổi tôn chỉ bang hội
        /// </summary>

        CMD_KT_GUILD_CHANGE_NOTIFY = 50114,

        /// <summary>
        /// MỞ UI CỔ TỨC
        /// </summary>
        CMD_KT_GUILD_GETSHARE = 50115,

        /// <summary>
        /// XIn gia nhập bang hội
        /// </summary>
        CMD_KT_GUILD_ASKJOIN = 50116,

        /// <summary>
        /// Trả lời đơn xin gia nhập
        /// </summary>
        CMD_KT_GUILD_RESPONSEASK = 50117,

        /// <summary>
        /// XIn gia nhập bang hội
        /// </summary>
        CMD_KT_GUILD_INVITE = 50118,

        /// <summary>
        /// XIn gia nhập bang hội
        /// </summary>
        CMD_KT_GUILD_RESPONSEINVITE = 50119,

        /// <summary>
        /// XIn gia nhập bang hội
        /// </summary>
        CMD_KT_GUILD_DOWTIHDRAW = 50120,

        /// <summary>
        /// Gia tộc rời khỏi bang hội
        /// </summary>
        CMD_KT_GUILD_FAMILYQUIT = 50121,

        /// <summary>
        /// Cập nhật thông tin hạng bang hội và gia tộc
        /// </summary>
        CMD_KT_UPDATE_GUILDANDFAMILY_RANK = 50122,

        /// <summary>
        /// Dữ liệu lãnh thổ
        /// </summary>
        CMD_KT_GETTERRORY_DATA = 50123,

        CMD_KT_GUILDWAR_RANKING = 50124,

        #endregion Bang hội

        #region Khung Du Long Các

        /// <summary>
        /// Gói tin thông tin Du Long Các
        /// </summary>
        CMD_KT_YOULONG = 50095,

        #endregion Khung Du Long Các

        #region Danh hiệu nhân vật

        /// <summary>
        /// Gói tin thông báo thay đổi danh hiệu nhân vật hiện tại
        /// </summary>
        CMD_KT_UPDATE_CURRENT_ROLETITLE = 50130,

        /// <summary>
        /// Gói tin thông báo thêm/xóa danh hiệu nhân vật
        /// </summary>
        CMD_KT_G2C_MOD_ROLETITLE = 50131,

        #endregion Danh hiệu nhân vật

        /// <summary>
        /// Gói tin gửi lưu tích lũy nạp
        /// </summary>
        CMD_KT_G2C_RECHAGE = 50132,

        #region Uy danh và vinh dự thay đổi

        /// <summary>
        /// Cập nhật thông tin uy danh và vinh dự thay đổi
        /// </summary>
        CMD_KT_G2C_UPDATE_PRESTIGE_AND_HONOR = 50133,

        #endregion Uy danh và vinh dự thay đổi

        #region Thông báo cập nhật thông tin người chơi khác

        /// <summary>
        /// Gói tin cập nhật thông tin trang bị người chơi khác
        /// </summary>
        CMD_KT_G2C_UPDATE_OTHERROLE_EQUIP = 50134,

        #endregion Thông báo cập nhật thông tin người chơi khác

        #region Chúc phúc

        /// <summary>
        /// Chúc phúc
        /// </summary>
        CMD_KT_G2C_PLAYERPRAY = 50135,

        #endregion Chúc phúc

        #region Lua

        /// <summary>
        /// Giao tiếp giữa Lua ở Client với Server
        /// </summary>
        CMD_KT_CLIENT_SERVER_LUA = 50136,

        #endregion Lua

        #region Luyện hóa trang bị

        /// <summary>
        /// Gói tin luyện hóa trang bị
        /// </summary>
        CMD_KT_CLIENT_DO_REFINE = 50137,

        #endregion Luyện hóa trang bị

        #region Tách Ngũ Hành Hồn Thạch từ trang bị

        /// <summary>
        /// Gói tin luyện hóa trang bị
        /// </summary>
        CMD_KT_C2G_SPLIT_EQUIP_INTO_FS = 50138,

        #endregion Tách Ngũ Hành Hồn Thạch từ trang bị

        #region Báo cho đối phương đang bị tấn công,

        /// <summary>
        /// Thông báo đang bị tấn công
        /// </summary>
        CMD_KT_TAKEDAMAGE = 50139,

        #endregion Báo cho đối phương đang bị tấn công,

        #region Nhập mật khẩu cấp 2

        /// <summary>
        /// Thông báo nhập mật khẩu cấp 2
        /// </summary>
        CMD_KT_INPUT_SECONDPASSWORD = 50140,

        #endregion Nhập mật khẩu cấp 2

        #region GuildUpdateMoney

        /// <summary>
        /// Cập nhật tài sản cá nhân
        /// </summary>
        CMD_KT_UPDATE_ROLEGUILDMONEY = 50141,

        #endregion GuildUpdateMoney

        /// <summary>
        /// Kiểm tra thứ hạng bản thân
        /// </summary>
        CMD_KT_RANKING_CHECKING = 50142,

        #region GHICHEP_RECRORE

        /// <summary>
        /// Lấy ra 1 biến đánh dấu theo time ranger
        /// </summary>
        CMD_KT_GETMARKVALUE = 50143,

        /// <summary>
        /// Update 1 biến đánh dấu theo time Ranger
        /// </summary>
        CMD_KT_UPDATEMARKVALUE = 50144,

        /// <summary>
        /// Lấy ra tổng giá trị đã ghi chép trong 1 khoảng thời gain
        /// </summary>
        CMD_KT_GET_RECORE_BYTYPE = 50145,

        /// <summary>
        /// Thêm vào 1 biến ghi chép trong 1 khoảng thời gian
        /// </summary>
        CMD_KT_ADD_RECORE_BYTYPE = 50146,

        /// <summary>
        /// Lấy ra bảng xếp hạng đã ghic hép trong 1 khoảng thời gian
        /// </summary>
        CMD_KT_GETRANK_RECORE_BYTYPE = 50147,

        /// <summary>
        /// Đổi xếp hạng GM
        /// </summary>
        CMD_KT_GMCHANGERANK = 50148,

        /// <summary>
        /// Tranh đoạt lãnh thổ
        /// </summary>
        CMD_KT_GUILD_ALLTERRITORY = 50149,

        #endregion GHICHEP_RECRORE

        #region Quái đặc biệt - Bản đồ khu vực
        /// <summary>
        /// Thông tin quái ở bản đồ khu vực
        /// </summary>
        CMD_KT_UPDATE_LOCALMAP_MONSTER = 50150,
        #endregion


        //Update thông tin lãnh thổ sau khi tranh đoạt lãnh thổ
        CMD_KT_GUILD_UPDATE_TERRITORY = 50151,


        CMD_KT_GUILD_GETMINIGUILDINFO = 50152,

        #region Captcha
        /// <summary>
        /// Captcha chống BOT
        /// </summary>
        CMD_KT_CAPTCHA = 50153,
        #endregion

        #region Vòng quay may mắn
        /// <summary>
        /// Vòng quay may mắn
        /// </summary>
        CMD_KT_LUCKYCIRCLE = 50154,
        #endregion
        #endregion KiemThe

        #region Test

        /// <summary>
        /// Gói tin Test
        /// </summary>
        CMD_KT_TESTPACKET = 32123,

        #endregion Test
    };

    /// <summary>
    /// Kết quả gói tin
    /// </summary>
    public enum TCPProcessCmdResults { RESULT_OK = 0, RESULT_FAILED = 1, RESULT_DATA = 2, RESULT_UNREGISTERED = 3, };

    /// <summary>
    /// Xử lý Packet tương tác qua lại với Client
    /// </summary>
    public class TCPCmdHandler
    {
        /// <summary>
        /// Tổng số Packet đang được xử lý
        /// </summary>
        public static long TotalHandledCmdsNum = 0;

        /// <summary>
        /// ID Packet tốn nhiều thời gian xử lý nhất
        /// </summary>
        public static int MaxUsedTicksCmdID = 0;

        /// <summary>
        /// Thời gian xử lý packet dài nhất
        /// </summary>
        public static long MaxUsedTicksByCmdID = 0;

        /// <summary>
        /// Danh sách Socket hiện có
        /// </summary>
        private static Dictionary<TMSKSocket, int> HandlingCmdDict = new Dictionary<TMSKSocket, int>();

        /// <summary>
        /// Trả về tổng số Socket hiện có
        /// </summary>
        /// <returns></returns>
        public static int GetHandlingCmdCount()
        {
            lock (HandlingCmdDict)
            {
                return HandlingCmdDict.Count;
            }
        }

        /// <summary>
        /// Thực hiện kết nối với 1 session mới của GAMESERVER
        /// </summary>
        /// <param name="tcpMgr"></param>
        /// <param name="socket"></param>
        /// <param name="tcpClientPool"></param>
        /// <param name="tcpRandKey"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public static void ProcessCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count)
        {
            TCPOutPacket tcpOutPacket = null;
            TCPProcessCmdResults result = TCPProcessCmdResults.RESULT_FAILED;

            result = TCPCmdHandler.ProcessCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);

            /// Nếu có dữ liệu
            if (result == TCPProcessCmdResults.RESULT_DATA && null != tcpOutPacket)
            {
                tcpMgr.MySocketListener.SendData(socket, tcpOutPacket);
            }
            /// Nếu toác
            else if (result == TCPProcessCmdResults.RESULT_FAILED)
            {
                if (nID != (int)TCPGameServerCmds.CMD_LOG_OUT)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Resolve packet faild: {0},{1}, close socket", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                }

                tcpMgr.MySocketListener.CloseSocket(socket);
            }
        }

        /// <summary>
        /// Nhận gói tin từ CLient gửi về và giải mã nó (tương đương PlayZone_Network ở client)
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessCmd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            long startTicks = TimeUtil.NOW();

            lock (HandlingCmdDict)
            {
                HandlingCmdDict[socket] = 1;
            }

            TCPProcessCmdResults result = TCPProcessCmdResults.RESULT_FAILED;
            tcpOutPacket = null;

            socket.session.CmdID = nID;
            socket.session.CmdTime = startTicks;

            #region Danh sách Packet

            result = TCPCmdDispatcher.getInstance().dispathProcessor(socket, nID, data, count);

            long tick = KTGlobal.GetCurrentTimeMilis();

            //#if DEBUG
            //            if (nID != 112 && nID != 613 && nID != 211 && nID != 45009 && nID != 50008)
            //            {
            //                Console.WriteLine("Received: {0} (ID: {1})", (TCPGameServerCmds)nID, nID);
            //            }
            //#endif

            if (result == TCPProcessCmdResults.RESULT_UNREGISTERED)
            {
                switch (nID)
                {
                    case (int)TCPGameServerCmds.CMD_LOGIN_ON2:
                        {
                            result = KT_TCPHandler.ProcessUserLogin2Cmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_LOGIN_ON:
                        {
                            result = KT_TCPHandler.ProcessUserLoginCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_LOG_OUT:
                        {
                            result = TCPProcessCmdResults.RESULT_FAILED;
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_ROLE_LIST:
                        {
                            result = KT_TCPHandler.ProcessGetRoleListCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_CREATE_ROLE:
                        {
                            result = KT_TCPHandler.ProcessCreateRoleCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_INIT_GAME:
                        {
                            result = KT_TCPHandler.ProcessInitGameCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SYNC_TIME:
                        {
                            result = KT_TCPHandler.ProcessTimeSyncGameCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_PUSH_VERSION:
                        {
                            result = KT_TCPHandler.ProcessClientPushVersionCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_PLAY_GAME:
                        {
                            result = KT_TCPHandler.ProcessStartPlayGameCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_SPECIALMACHINE:
                        {
                            result = KT_TCPHandler.ProcessSpecialDeviceName(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_MOVE:
                        {
                            result = KT_TCPHandler.ProcessSpriteMoveCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_STOPMOVE:
                        {
                            result = KT_TCPHandler.ProcessSpriteStopMoveCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_POSITION:
                        {
                            result = KT_TCPHandler.ProcessSpritePosCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_MAPCHANGE:
                        {
                            result = KT_TCPHandler.ProcessSpriteMapChangeCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_ENTERMAP:
                        {
                            result = KT_TCPHandler.ProcessSpriteEnterMap(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_UPDATE_LOCALMAP_MONSTER:
                        {
                            result = KT_TCPHandler.ProcessGetLocalMapSpecialMonsters(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_NPC_BUY:
                        {
                            result = KT_TCPHandler.ProcessSpriteNPCBuyCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_NPC_SALEOUT:
                        {
                            result = KT_TCPHandler.ProcessSpriteNPCSaleOutCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_MOD_GOODS:
                        {
                            result = KT_TCPHandler.ProcessSpriteModGoodsCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_MERGE_GOODS:
                        {
                            result = KT_TCPHandler.ProcessSpriteMergeGoodsCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GETFRIENDS:
                        {
                            result = KT_TCPHandler.ProcessSpriteGetFriendsCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_ADDFRIEND:
                        {
                            result = KT_TCPHandler.ProcessSpriteAddFriendCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND:
                        {
                            result = KT_TCPHandler.ProcessSpriteRemoveFriendCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_ASKFRIEND:
                        {
                            result = KT_TCPHandler.ProcessSpriteAskFriend(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_REJECTFRIEND:
                        {
                            result = KT_TCPHandler.ProcessSpriteRejectFriend(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GETTHING:
                        {
                            result = KT_TCPHandler.ProcessSpriteGetThingCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_CHGPKMODE:
                        {
                            result = KT_TCPHandler.ProcessSpriteChangePKModeCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_CHAT:
                        {
                            result = KT_TCPHandler.ProcessSpriteChatCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_USEGOODS:
                        {
                            result = KT_TCPHandler.ProcessSpriteUseGoodsCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_OTHER_ROLE_DATA:
                        {
                            result = KT_TCPHandler.ResponseGetOtherRoleEquipInfo(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GOODSEXCHANGE:
                        {
                            result = KT_TCPHandler.ProcessSpriteGoodsExchangeCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GOODSSTALL:
                        {
                            result = KT_TCPHandler.ProcessSpriteGoodsStallCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_GETGOODSLISTBYSITE:
                        {
                            result = KT_TCPHandler.ProcessSpriteGetGoodsListBySiteCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_LOADALREADY:
                        {
                            result = KT_TCPHandler.ProcessSpriteLoadAlreadyCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_RESETBAG:
                        {
                            result = KT_TCPHandler.ProcessSpriteResetBagCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_RESETPORTABLEBAG:
                        {
                            result = KT_TCPHandler.ProcessSpriteResetPortableBagCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GETSONGLIGIFT:
                        {
                            result = KT_TCPHandler.ProcessSpriteGetSongLiGiftCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }


                    case (int)TCPGameServerCmds.CMD_SPR_CLIENTHEART:
                        {
                            result = KT_TCPHandler.ProcessSpriteClientHeartCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GETPAIHANGLIST:
                        {
                            result = KT_TCPHandler.ProcessGetPaiHangListCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_GETUSERMAILLIST:
                        {
                            result = KT_TCPHandler.ProcessGetUserMailListCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETUSERMAILDATA:
                        {
                            result = KT_TCPHandler.ProcessGetUserMailDataCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_FETCHMAILGOODS:
                        {
                            result = KT_TCPHandler.ProcessFetchMailGoodsCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_DELETEUSERMAIL:
                        {
                            result = KT_TCPHandler.ProcessDeleteUserMailCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_EXECUTERECOMMENDPROPADDPOINT:
                        {
                            result = KT_TCPHandler.ProcessExecuteRecommendPropAddPointCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_UPDATEEVERYDAYONLINEAWARDGIFTINFO:
                        {
                            result = EveryDayOnlineManager.ProcessSpriteUpdateEverydayOnlineAwardGiftInfoCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETEVERYDAYONLINEAWARDGIFT:
                        {
                            result = EveryDayOnlineManager.ProcessSpriteGetEveryDayOnLineAwardGiftCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_UPDATEEVERYDAYSERIESLOGININFO:
                        {
                            result = SevenDayLoginManager.ProcessSpriteUpdateEverydaySeriesLoginInfoCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETEVERYDAYSERIESLOGINAWARDGIFT:
                        {
                            result = SevenDayLoginManager.ProcessSpriteGetSeriesLoginAwardGiftCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_QUERYTOTALLOGININFO:
                        {
                            result = TotalLoginManager.ProcessSpriteQueryTotalLoginInfoCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETTOTALLOGINAWARD:
                        {
                            result = TotalLoginManager.ProcessSpriteGetTotalLoginAwardCmd(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_GET_BONUS_DOWNLOAD:
                        {
                            result = KT_TCPHandler.ProcessDownloadBonus(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETBAITANLOG:
                        {
                            result = TCPCmdDispatcher.getInstance().transmission(socket, nID, data, count);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_CHECK:
                        {
                            result = KT_TCPHandler.ProcessCheck(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_QUERYUPLEVELGIFTINFO:
                        {
                            result = LevelUpEventManager.ProcessQueryUpLevelGiftFlagList(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GETUPLEVELGIFTAWARD:
                        {
                            result = LevelUpEventManager.ProcessGetUpLevelGiftAward(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_QUERY_REPAYACTIVEINFO:
                        {
                            result = RechageManager.QueryRechargeRepayActive(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GET_REPAYACTIVEAWARD:
                        {
                            result = RechageManager.ProcessGetRepayAwardCmd(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_SPR_QUERY_ALLREPAYACTIVEINFO:
                        {
                            result = RechageManager.QueryAllRechargeRepayActiveInfo(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_SEASHELL_CIRCLE:
                        {
                            result = KT_TCPHandler.ResponseSeashellCircleRequest(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GET_STORE_MONEY:
                        {
                            result = KT_TCPHandler.ProcessModifyStoreMoney(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_WING_ZHUHUN:
                        {
                            result = CardMonthManager.ActiveCardMoth(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);

                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GET_YUEKA_DATA:
                        {
                            result = CardMonthManager.ProcessGetYueKaData(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_SPR_GET_YUEKA_AWARD:
                        {
                            result = CardMonthManager.ProcessGetYueKaAward(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_GET_NEWBIE_VILLAGES:
                        {
                            result = KT_TCPHandler.PrecessGetNewbieVillages(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_ROLE_ATRIBUTES:
                        {
                            result = KT_TCPHandler.GetRoleAttributes(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CLICKON_NPC:
                        {
                            result = KT_TCPHandler.ProcessClickOnNPC(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_NPCDIALOG:
                        {
                            result = KT_TCPHandler.ResponseNPCDialog(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_ITEMDIALOG:
                        {
                            result = KT_TCPHandler.ResponseItemDialog(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_CHANGEACTION:
                        {
                            result = KT_TCPHandler.ResponseSpriteChangeAction(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SKILL_ADDPOINT:
                        {
                            result = KT_TCPHandler.ResponseDistributeSkillPoints(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SET_SKILL_TO_QUICKKEY:
                        {
                            result = KT_TCPHandler.ResponseSetQuickKey(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SET_AND_ACTIVATE_AURA:
                        {
                            result = KT_TCPHandler.ResponseSetAndActivateAura(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_USESKILL:
                        {
                            result = KT_TCPHandler.ResponseUseSkill(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GM_COMMAND:
                        {
                            result = KT_TCPHandler.ResponseGMCommand(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_CLIENTREVIVE:
                        {
                            result = KT_TCPHandler.ResponseClientRevive(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SAVESYSTEMSETTINGS:
                        {
                            result = KT_TCPHandler.ResponseSaveSystemSettings(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SAVEAUTOSETTINGS:
                        {
                            result = KT_TCPHandler.ResponseSaveAutoSettings(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_INVITETOTEAM:
                        {
                            result = KT_TCPHandler.ResponseInviteTeammate(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CREATETEAM:
                        {
                            result = KT_TCPHandler.ResponseCreateTeam(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_AGREEJOINTEAM:
                        {
                            result = KT_TCPHandler.ResponseAgreeJoinTeam(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_REFUSEJOINTEAM:
                        {
                            result = KT_TCPHandler.ResponseRefuseJoinTeam(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GETTEAMINFO:
                        {
                            result = KT_TCPHandler.ResponseGetTeamInfo(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_KICKOUTTEAMMATE:
                        {
                            result = KT_TCPHandler.ResponseKickOutTeammate(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_LEAVETEAM:
                        {
                            result = KT_TCPHandler.ResponseLeaveTeam(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_APPROVETEAMLEADER:
                        {
                            result = KT_TCPHandler.ResponseApproveTeamLeader(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_ASKTOJOINTEAM:
                        {
                            result = KT_TCPHandler.ResponseAskToJoinTeam(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_ASK_CHALLENGE:
                        {
                            result = KT_TCPHandler.ResponseAskChallenge(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_RESPONSE_CHALLENGE:
                        {
                            result = KT_TCPHandler.ResponseResponseChallenge(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_AUTOPATH_CHANGEMAP:
                        {
                            result = KT_TCPHandler.ResponseClientAutoPathTransferMap(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_GROWPOINT_CLICK:
                        {
                            result = KT_TCPHandler.ResponseGrowPointClick(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_OPEN_TOKENSHOP:
                        {
                            result = KT_TCPHandler.ResponseOpenTokenShop(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_GOI_COIN:
                        {
                            result = KT_TCPHandler.ResponseMuaGoiKTCoin(tcpMgr, socket, tcpClientPool, tcpRandKey, pool,
                                nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_TOGGLE_HORSE_STATE:
                        {
                            result = KT_TCPHandler.ResponseToggleHorseState(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_EQUIP_ENHANCE:
                        {
                            result = KT_TCPHandler.ResponseEquipEnhance(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SIGNET_ENHANCE:
                        {
                            result = KT_TCPHandler.ResponseSignetEnhance(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CLIENT_DO_REFINE:
                        {
                            result = KT_TCPHandler.CMD_KT_CLIENT_DO_REFINE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_COMPOSE_CRYSTALSTONES:
                        {
                            result = KT_TCPHandler.ResponseComposeCrystalStones(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SPLIT_CRYSTALSTONES:
                        {
                            result = KT_TCPHandler.ResponseEquipSplitCrystalStones(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_ASK_ACTIVEFIGHT:
                        {
                            result = KT_TCPHandler.ResponseActiveFight(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CHANGE_AVARTA:
                        {
                            result = KT_TCPHandler.ResponseRoleAvartaChange(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_BEGIN_CRAFT:
                        {
                            result = KT_TCPHandler.ResponseCraftItem(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SHOW_MESSAGEBOX:
                        {
                            result = KT_TCPHandler.ResponseUIMessageBoxResult(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SONGJINBATTLE_RANKING:
                        {
                            result = KT_TCPHandler.SongJinBattleGetRanking(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_FACTION_PVP_RANKING_INFO:
                        {
                            result = KT_TCPHandler.FactionBattleRanking(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_BROWSE_PLAYER:
                        {
                            result = KT_TCPHandler.ResponseBrowsePlayers(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CHECK_PLAYER_LOCATION:
                        {
                            result = KT_TCPHandler.ResponseCheckPlayerLocation(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GET_PLAYER_INFO:
                        {
                            result = KT_TCPHandler.ResponseCheckPlayerInfo(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_QUERY_PLAYERRANKING:
                        {
                            result = KT_TCPHandler.ResponseQueryPlayerRanking(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_TESTPACKET:
                        {
                            result = KT_TCPHandler.ResponseCMDTestFromClient(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_CHANGE_SUBSET:
                        {
                            result = KT_TCPHandler.ResponseChangeSubEquipSet(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SHOW_INPUTITEMS:
                        {
                            result = KT_TCPHandler.ResponseInputItems(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_SHOW_INPUTEQUIPANDMATERIALS:
                        {
                            result = KT_TCPHandler.ResponseInputEquipAndMaterials(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_CREATE:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_CREATE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_REQUESTJOIN:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_REQUESTJOIN(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_KICKMEMBER:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_KICKMEMBER(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_GETLISTFAMILY:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_GETLISTFAMILY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_DESTROY:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_DESTROY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGENOTIFY:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_CHANGENOTIFY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_RANK:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_CHANGE_RANK(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_RESPONSE_REQUEST:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_RESPONSE_REQUEST(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_QUIT:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_QUIT(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }
                    case (int)TCPGameServerCmds.CMD_KT_FAMILY_OPEN:
                        {
                            result = KT_TCPHandler.CMD_KT_FAMILY_OPEN(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_CREATE:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_CREATE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_GETINFO:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_GETINFO(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_GETMEMBERLIST:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_GETMEMBERLIST(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_CHANGERANK(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_KICKFAMILY:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_KICKFAMILY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_GETGIFTED:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_GETGIFTED(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_OFFICE_RANK:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_OFFICE_RANK(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_VOTEGIFTED:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_VOTEGIFTED(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_DONATE:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_DONATE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_TERRITORY:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_TERRITORY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_SETCITY:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_SETCITY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_SETTAX:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_SETTAX(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_QUIT:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_QUIT(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGE_MAXWITHDRAW:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_CHANGE_MAXWITHDRAW(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGE_NOTIFY:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_CHANGE_NOTIFY(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_GETSHARE:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_GETSHARE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_ASKJOIN:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_ASKJOIN(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_RESPONSEASK:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_RESPONSEASK(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_INVITE:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_INVITE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_RESPONSEINVITE:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_RESPONSEINVITE(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_DOWTIHDRAW:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_DOWTIHDRAW(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILD_FAMILYQUIT:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILD_FAMILYQUIT(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GETTERRORY_DATA:
                        {
                            result = KT_TCPHandler.CMD_KT_GETTERRORY_DATA(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_GUILDWAR_RANKING:
                        {
                            result = KT_TCPHandler.CMD_KT_GUILDWAR_RANKING(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_YOULONG:
                        {
                            result = KT_TCPHandler.ProcessYouLongRequest(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_UPDATE_CURRENT_ROLETITLE:
                        {
                            result = KT_TCPHandler.ResponseChangeCurrentRoleTitle(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_G2C_PLAYERPRAY:
                        {
                            result = KT_TCPHandler.ResponsePlayerPray(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CLIENT_SERVER_LUA:
                        {
                            result = KT_TCPHandler.ProcessClientLua(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_C2G_SPLIT_EQUIP_INTO_FS:
                        {
                            result = KT_TCPHandler.ResponseRefineEquipToFS(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_INPUT_SECONDPASSWORD:
                        {
                            result = KT_TCPHandler.ResponseInputSecondPassword(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int)TCPGameServerCmds.CMD_KT_CAPTCHA:
                        {
                            result = KT_TCPHandler.ResponseCaptchaAnswer(tcpMgr, socket, tcpClientPool, tcpRandKey, pool, nID, data, count, out tcpOutPacket);
                            break;
                        }

                    case (int) TCPGameServerCmds.CMD_KT_LUCKYCIRCLE:
                    {
                        result = KT_TCPHandler.ResponseLuckyCircleRequest(tcpMgr, socket, tcpClientPool, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                    default:
                        {
                            //LogManager.WriteLog(LogTypes.Error, string.Format("Undefined packet, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                            if (GameManager.FlagAlowUnRegistedCmd)
                            {
                                result = TCPProcessCmdResults.RESULT_OK;
                            }
                            else
                            {
                                result = TCPProcessCmdResults.RESULT_FAILED;
                            }
                            break;
                        }
                }
            }

            #endregion Danh sách Packet

            //         if (KTGlobal.GetCurrentTimeMilis() - tick >= 100)
            //{
            //             LogManager.WriteLog(LogTypes.RolePosition, "Packet " + (TCPProcessCmdResults) nID + " took more than 0.1s...");
            //}

            TotalHandledCmdsNum++;

            long nowTicks = TimeUtil.NOW();
            long usedTicks = nowTicks - startTicks;
            if (usedTicks > 0)
            {
                if (usedTicks > MaxUsedTicksByCmdID)
                {
                    MaxUsedTicksCmdID = nID;
                    MaxUsedTicksByCmdID = usedTicks;
                }
            }

            lock (HandlingCmdDict)
            {
                HandlingCmdDict.Remove(socket);
            }

            return result;
        }
    }
}